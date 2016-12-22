using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    public partial class GarmentList : Sci.Win.Subs.Base
    {
        private string Styleyukey;
        private DataTable headertb;
        private string patternukey;
        public GarmentList(string cID)
        {
            InitializeComponent();
            Styleyukey = cID;
            requery();
            gridSetup();

        }
        private void requery()
        {
            
            #region 撈取Pattern Ukey  找最晚Edit且Status 為Completed
            string patidsql = String.Format(
                            @"SELECT ukey
                              FROM [Production].[dbo].[Pattern]
                              WHERE STYLEUKEY = '{0}'  and Status = 'Completed' 
                              AND EDITdATE = 
                              (
                                SELECT MAX(EditDate) 
                                from pattern 
                                where styleukey = '{0}' and Status = 'Completed'
                              )
             ", Styleyukey);
            patternukey = MyUtility.GetValue.Lookup(patidsql);
            #endregion
            #region 找ArticleGroup 當Table Header
            string headercodesql = string.Format("Select distinct ArticleGroup from Pattern_GL_LectraCode where PatternUkey = '{0}' and ArticleGroup !='F_CODE' order by ArticleGroup",patternukey);
            
            DualResult headerResult = DBProxy.Current.Select(null, headercodesql, out headertb);
            if (!headerResult)
            {
                ShowErr(headercodesql, headerResult);
                return;
            }
            #endregion
            #region 建立Table
            string tablecreatesql = "Select a.*,'' as F_CODE,b.PatternPanel";
            foreach (DataRow dr in headertb.Rows)
            {
                tablecreatesql = tablecreatesql + string.Format(" ,'' as {0}", dr["ArticleGroup"]);
            }
            tablecreatesql = tablecreatesql + string.Format(@" from Pattern_GL a inner join Pattern_GL_LectraCode b on a.PatternUKEY=b.PatternUKEY and a.SEQ = b.SEQ Where a.PatternUkey = '{0}'", patternukey);
            DataTable gridtb;
            DualResult tablecreateResult= DBProxy.Current.Select(null, tablecreatesql, out gridtb);
            if (!tablecreateResult)
            {
                ShowErr(tablecreatesql, tablecreateResult);
                return;
            }
            #endregion 
            #region 寫入FCode~CodeA~CodeZ
            string lecsql = "";
            lecsql = string.Format("Select * from Pattern_GL_LectraCode a where a.PatternUkey = '{0}'", patternukey);
            DataTable drtb;
            DualResult drre = DBProxy.Current.Select(null, lecsql, out drtb);
            if (!drre)
            {
                ShowErr(lecsql, drre);
                return;
            }
            foreach(DataRow dr in gridtb.Rows)
            {
                DataRow[] lecdrar = drtb.Select(string.Format("SEQ = '{0}'", dr["SEQ"]));
                foreach (DataRow lecdr in lecdrar)
                {
                    string artgroup = lecdr["ArticleGroup"].ToString().Trim();
                    dr[artgroup] = lecdr["LectraCode"].ToString().Trim();  //1181:CUTTING_P01_GarmentList。F_Code & CodeA的資料不正確，應為PATTERN_GL_LECTRACODE.LectraCode
                }
                if (dr["SEQ"].ToString() == "0001") dr["PatternCode"] = dr["PatternCode"].ToString().Substring(10);
            }
            #endregion

            grid1.DataSource = gridtb;
        }
        private void gridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("PatternCode", header: "Cutpart ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Artwork", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Alone", header: "Alone", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Pair", header: "Pair", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("DV", header: "DV", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("F_CODE", header: "F_Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Remarks", header: "Remarks", width: Widths.AnsiChars(15), iseditingreadonly: true);
            foreach (DataRow dr in headertb.Rows)
            {
                string header = dr["ArticleGroup"].ToString().Trim();
                Helper.Controls.Grid.Generator(this.grid1)
                 .Text(header, header: header, width: Widths.AnsiChars(8), iseditingreadonly: true);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.GarmentList_ColorArticle callNextForm =
new Sci.Production.PublicForm.GarmentList_ColorArticle(patternukey,Styleyukey);
            callNextForm.ShowDialog(this);
        }
    }
}
