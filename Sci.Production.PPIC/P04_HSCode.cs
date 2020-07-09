using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_HSCode
    /// </summary>
    public partial class P04_HSCode : Win.Subs.Base
    {
        private string styleUkey;

        /// <summary>
        /// P04_HSCode
        /// </summary>
        /// <param name="styleUkey">string styleUkey</param>
        public P04_HSCode(string styleUkey)
        {
            this.InitializeComponent();
            this.styleUkey = styleUkey;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            // 撈Grid資料
            string sqlCmd = string.Format(
                @"select sh.*,sh.CountryID + '-' + isnull(c.Alias,'') as Country, '' as CreateBy,'' as EditBy
from Style_HSCode sh WITH (NOLOCK) 
left join Country c WITH (NOLOCK) on sh.CountryID = c.ID
where sh.StyleUkey = {0}
order by sh.Continent,sh.CountryID,sh.Article", this.styleUkey);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            foreach (DataRow gridData in selectDataTable.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " " + (MyUtility.Check.Empty(gridData["AddDate"]) ? string.Empty : ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat)));
                gridData["EditBy"] = gridData["EditName"].ToString() + "  " + (MyUtility.Check.Empty(gridData["EditDate"]) ? string.Empty : ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat)));
            }

            this.listControlBindingSource1.DataSource = selectDataTable;

            // 設定Grid1的顯示欄位
            this.gridHSCode.IsEditingReadOnly = true;
            this.gridHSCode.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridHSCode)
                 .Text("Continent", header: "Continent", width: Widths.AnsiChars(2))
                 .Text("Country", header: "Country", width: Widths.AnsiChars(15))
                 .Text("Article", header: "Article", width: Widths.AnsiChars(8))
                 .Text("HSCode1", header: "HS Code", width: Widths.AnsiChars(14))
                 .Text("CATNo1", header: "CAT#", width: Widths.AnsiChars(3))
                 .Text("HSCode2", header: "HS Code", width: Widths.AnsiChars(14))
                 .Text("CATNo2", header: "CAT#", width: Widths.AnsiChars(3))
                 .Text("CreateBy", header: "Create by", width: Widths.AnsiChars(25))
                 .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(10));
        }
    }
}
