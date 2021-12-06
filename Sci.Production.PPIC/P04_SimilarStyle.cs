using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@styleUkey", this.styleUkey) };

            string sqlCheckMaster = @"
select MasterBrandID,MasterStyleID 
from Style_SimilarStyle WITH (NOLOCK) 
where exists( select 1 from Style s with (nolock) 
                where s.Ukey = @styleUkey and Style_SimilarStyle.MasterStyleID = s.ID and Style_SimilarStyle.MasterBrandID = s.BrandID)
union all
select MasterBrandID,MasterStyleID 
from Style_SimilarStyle WITH (NOLOCK) 
where exists( select 1 from Style s with (nolock) 
                where s.Ukey = @styleUkey and Style_SimilarStyle.ChildrenStyleID = s.ID and Style_SimilarStyle.ChildrenBrandID = s.BrandID)
";

            // 撈Grid資料
            string sqlCmd = string.Empty;

            if (MyUtility.Check.Seek(sqlCheckMaster, listPar, out styleSimilarData))
            {
                this.displayBrand.Value = styleSimilarData["MasterBrandID"].ToString();
                this.displayMasterStyle.Value = styleSimilarData["MasterStyleID"].ToString();
                this.displaySeason.Value = MyUtility.GetValue.Lookup($"select Stuff((select concat( ',',SeasonID) from Style with (nolock) where ID = '{styleSimilarData["MasterStyleID"]}' and BrandID = '{styleSimilarData["MasterBrandID"]}' order by SeasonID FOR XML PATH('')),1,1,'') ");

                sqlCmd = $@"
select s.ChildrenBrandID, s.ChildrenStyleID, seasonList.Season, s.AddName, s.AddDate, s.EditDate, s.EditName, '' as CreateBy,'' as EditBy  
from Style_SimilarStyle s WITH (NOLOCK)
Outer Apply (
	select Season = stuff(
		(
		select ',' + ID 
		from (
			select isnull(Season.ID,tmpS.SeasonID) ID, isnull(Season.Month, 0) Month
			from Style tmpS
			left join Season on tmpS.SeasonID = Season.ID and Season.BrandID = tmpS.BrandID
			where tmpS.BrandID = s.ChildrenBrandID and tmpS.Id = s.ChildrenStyleID
			) tmp
		order by ID, Month desc
		for xml PATH('')
		), 1, 1, '')
) seasonList
where   s.MasterStyleID = '{styleSimilarData["MasterStyleID"]}' and s.MasterBrandID = '{styleSimilarData["MasterBrandID"]}'
order by ChildrenStyleID";
            }

            if (MyUtility.Check.Empty(sqlCmd))
            {
                return;
            }

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, listPar, out selectDataTable);
            if (!selectResult1)
            {
                this.ShowErr(selectResult1);
                return;
            }

            foreach (DataRow gridData in selectDataTable.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " " + (MyUtility.Check.Empty(gridData["AddDate"]) ? string.Empty : ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat)));
                gridData["EditBy"] = gridData["EditName"].ToString() + "  " + (MyUtility.Check.Empty(gridData["EditDate"]) ? string.Empty : ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat)));
            }

            this.listControlBindingSource1.DataSource = selectDataTable;

            // 設定Grid1的顯示欄位
            this.gridChildrenStyle.IsEditingReadOnly = true;
            this.gridChildrenStyle.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridChildrenStyle)
                 .Text("ChildrenStyleID", header: "Children Style", width: Widths.AnsiChars(15))
                 .Text("ChildrenBrandID", header: "Brand", width: Widths.AnsiChars(8))
                 .Text("Season", header: "Season", width: Widths.AnsiChars(16), iseditingreadonly: true)
                 .Text("CreateBy", header: "Create by", width: Widths.AnsiChars(30))
                 .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(25));
        }
    }
}
