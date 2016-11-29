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

namespace Sci.Production.Cutting
{
    public partial class P01_Cutpartchecksummary : Sci.Win.Subs.Base
    {
        private string cutid;

        private DataTable fabcodetb; //PatternPanel Table
        public P01_Cutpartchecksummary(string cID)
        {
            InitializeComponent();

            this.Text = string.Format("Cut Parts Check<SP:{0}>)", cID);
            cutid = cID;
            requery();
            gridSetup();

        }
        private void requery()
        {
            #region 找出有哪些部位
            string fabcodesql = string.Format(@"
            Select distinct a.PatternPanel
            from Order_FabricCode a ,Order_EachCons b 
            where a.id = '{0}' and a.FabricCode is not null and a.FabricCode !='' 
            and b.id = '{0}' and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel
            order by patternpanel", cutid);
            DualResult fabresult = DBProxy.Current.Select("Production", fabcodesql, out fabcodetb);
            #endregion

            #region 建立Grid
            string settbsql = "Select a.id,article,a.sizecode,a.qty,'' as complete"; //寫SQL建立Table
            foreach (DataRow dr in fabcodetb.Rows) //組動態欄位
            {
                settbsql = settbsql + ", 0 as " + dr["PatternPanel"];
            }
            settbsql = settbsql + string.Format(@" From Order_Qty a,orders b ,Order_SizeCode c
                                                Where b.cuttingsp ='{0}' and a.id = b.id 
                                                and c.id=b.poid and c.SizeCode = a.SizeCode
                                                order by id,article,c.Seq", 
                                                cutid);

            DataTable gridtb;
            DualResult gridResult = DBProxy.Current.Select(null, settbsql, out gridtb);
            #endregion

            #region 寫入部位數量
            string getqtysql = string.Format(@"
            Select b.article,b.sizecode,b.qty,c.PatternPanel,b.orderid 
            from Workorder a, workorder_Distribute b, Order_fabriccode c
            Where a.id = '{0}' and a.ukey = b.workorderukey and a.Id  = c.id and a.LectraCode = c.Lectracode 
            and b.article !=''", cutid);
            DataTable getqtytb;

            gridResult = DBProxy.Current.Select(null, getqtysql, out getqtytb);
            foreach (DataRow dr in getqtytb.Rows)
            {
                DataRow[] gridselect = gridtb.Select(string.Format("id = '{0}' and article = '{1}' and sizecode = '{2}'", dr["orderid"], dr["article"], dr["sizecode"], dr["PatternPanel"], dr["Qty"]));
                if (gridselect.Length != 0)
                {
                    gridselect[0][dr["PatternPanel"].ToString()] = MyUtility.Convert.GetDecimal((gridselect[0][dr["PatternPanel"].ToString()])) + MyUtility.Convert.GetDecimal(dr["Qty"]);
                }
            }
            #endregion

            #region 判斷是否Complete
            bool complete = true;
            DataTable panneltb;
            fabcodesql = string.Format(@"Select distinct a.Article,a.PatternPanel
            from Order_ColorCombo a ,Order_EachCons b
            where a.id = '{0}' and a.FabricCode is not null 
            and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel
            and a.FabricCode !='' order by Article,PatternPanel", cutid);
            gridResult = DBProxy.Current.Select(null, fabcodesql, out panneltb);
            foreach (DataRow dr in gridtb.Rows)
            {
                complete = false;
                DataRow[] sel = panneltb.Select(string.Format("Article = '{0}'", dr["Article"]));
                foreach (DataRow pdr in sel)
                {

                    if (MyUtility.Convert.GetDecimal(dr["Qty"]) <= MyUtility.Convert.GetDecimal(dr[pdr["Patternpanel"].ToString()])) complete = true;

                }
                if (complete) dr["Complete"] = "Y";

            }
            #endregion
            grid1.DataSource = gridtb;
        }
        private void gridSetup()
        {
            grid1.RowPostPaint += (s, e) =>
            {
                string art = "";
                for (int i = 0; i <= e.RowIndex; i++)
                {
                    if (i == 0) art = grid1.Rows[i].Cells[1].Value.ToString();
                    if (grid1.Rows[i].Cells[1].Value.ToString() != art)
                    {
                        grid1.Rows[i - 1].DefaultCellStyle.BackColor = Color.Pink;
                        art = grid1.Rows[i].Cells[1].Value.ToString();
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("id", header: "SP No", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Complete", header: "Complete", width: Widths.AnsiChars(1), iseditingreadonly: true);


            for (int i = 0; i < fabcodetb.Rows.Count; i++)
            {
                Ict.Win.UI.DataGridViewNumericBoxColumn col_color;

                Helper.Controls.Grid.Generator(grid1)
                    .Numeric(fabcodetb.Rows[i]["PatternPanel"].ToString().Trim(), header: fabcodetb.Rows[i]["PatternPanel"].ToString(), width: Widths.AnsiChars(7)).Get(out col_color);

                col_color.CellFormatting += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    DataRow dr = grid1.GetDataRow(e.RowIndex);

                    if (MyUtility.Convert.GetDecimal(dr[e.ColumnIndex]) < MyUtility.Convert.GetDecimal(dr["Qty"]))
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                };
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
