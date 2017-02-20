using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P04_SimilarStyle : Sci.Win.Subs.Base
    {
        private string styleUkey;
        public P04_SimilarStyle(string StyleUkey)
        {
            InitializeComponent();
            styleUkey = StyleUkey;
        }

        protected override void OnFormLoaded()
        {
            DataRow StyleSimilarData;
            string masterStyleUKey = null;
            if (MyUtility.Check.Seek(string.Format("select MasterBrandID,MasterStyleID,MasterSeasonID,MasterStyleUkey from Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey = {0}", styleUkey), out StyleSimilarData))
            {
                displayBox1.Value = StyleSimilarData["MasterBrandID"].ToString();
                displayBox2.Value = StyleSimilarData["MasterStyleID"].ToString();
                displayBox3.Value = StyleSimilarData["MasterSeasonID"].ToString();
                masterStyleUKey = StyleSimilarData["MasterStyleUkey"].ToString();
            }
            else
            {
                if (MyUtility.Check.Seek(string.Format("select MasterBrandID,MasterStyleID,MasterSeasonID,MasterStyleUkey from Style_SimilarStyle WITH (NOLOCK) where ChildrenStyleUkey = {0}", styleUkey), out StyleSimilarData))
                {
                    displayBox1.Value = StyleSimilarData["MasterBrandID"].ToString();
                    displayBox2.Value = StyleSimilarData["MasterStyleID"].ToString();
                    displayBox3.Value = StyleSimilarData["MasterSeasonID"].ToString();
                    masterStyleUKey = StyleSimilarData["MasterStyleUkey"].ToString();
                }
            }

            //撈Grid資料
            string sqlCmd = string.Format(@"select *, '' as CreateBy,'' as EditBy  from Style_SimilarStyle WITH (NOLOCK) where {0} order by ChildrenStyleID", masterStyleUKey == null ? "1=2" : "MasterStyleUkey = " + masterStyleUKey);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            foreach (DataRow gridData in selectDataTable.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " " + (MyUtility.Check.Empty(gridData["AddDate"]) ? "" : ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)));
                gridData["EditBy"] = gridData["EditName"].ToString() + "  " + (MyUtility.Check.Empty(gridData["EditDate"]) ? "" : ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)));
            }
            
            listControlBindingSource1.DataSource = selectDataTable;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("ChildrenStyleID", header: "Children Style", width: Widths.AnsiChars(15))
                 .Text("ChildrenBrandID", header: "Brand", width: Widths.AnsiChars(8))
                 .Text("CreateBy", header: "Create by", width: Widths.AnsiChars(30))
                 .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(25));
        }
    }
}
