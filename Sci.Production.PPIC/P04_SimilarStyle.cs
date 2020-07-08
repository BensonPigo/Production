using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_SimilarStyle
    /// </summary>
    public partial class P04_SimilarStyle : Win.Subs.Base
    {
        private string styleUkey;

        /// <summary>
        /// P04_SimilarStyle
        /// </summary>
        /// <param name="styleUkey">string StyleUkey</param>
        public P04_SimilarStyle(string styleUkey)
        {
            this.InitializeComponent();
            this.styleUkey = styleUkey;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            DataRow styleSimilarData;
            string masterStyleUKey = null;
            if (MyUtility.Check.Seek(string.Format("select MasterBrandID,MasterStyleID,MasterSeasonID,MasterStyleUkey from Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey = {0}", this.styleUkey), out styleSimilarData))
            {
                this.displayBrand.Value = styleSimilarData["MasterBrandID"].ToString();
                this.displayMasterStyle.Value = styleSimilarData["MasterStyleID"].ToString();
                this.displaySeason.Value = styleSimilarData["MasterSeasonID"].ToString();
                masterStyleUKey = styleSimilarData["MasterStyleUkey"].ToString();
            }
            else
            {
                if (MyUtility.Check.Seek(string.Format("select MasterBrandID,MasterStyleID,MasterSeasonID,MasterStyleUkey from Style_SimilarStyle WITH (NOLOCK) where ChildrenStyleUkey = {0}", this.styleUkey), out styleSimilarData))
                {
                    this.displayBrand.Value = styleSimilarData["MasterBrandID"].ToString();
                    this.displayMasterStyle.Value = styleSimilarData["MasterStyleID"].ToString();
                    this.displaySeason.Value = styleSimilarData["MasterSeasonID"].ToString();
                    masterStyleUKey = styleSimilarData["MasterStyleUkey"].ToString();
                }
            }

            // 撈Grid資料
            string sqlCmd = string.Format(@"select *, '' as CreateBy,'' as EditBy  from Style_SimilarStyle WITH (NOLOCK) where {0} order by ChildrenStyleID", masterStyleUKey == null ? "1=2" : "MasterStyleUkey = " + masterStyleUKey);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            foreach (DataRow gridData in selectDataTable.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " " + (MyUtility.Check.Empty(gridData["AddDate"]) ? string.Empty : ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)));
                gridData["EditBy"] = gridData["EditName"].ToString() + "  " + (MyUtility.Check.Empty(gridData["EditDate"]) ? string.Empty : ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)));
            }

            this.listControlBindingSource1.DataSource = selectDataTable;

            // 設定Grid1的顯示欄位
            this.gridChildrenStyle.IsEditingReadOnly = true;
            this.gridChildrenStyle.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridChildrenStyle)
                 .Text("ChildrenStyleID", header: "Children Style", width: Widths.AnsiChars(15))
                 .Text("ChildrenBrandID", header: "Brand", width: Widths.AnsiChars(8))
                 .Text("CreateBy", header: "Create by", width: Widths.AnsiChars(30))
                 .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(25));
        }
    }
}
