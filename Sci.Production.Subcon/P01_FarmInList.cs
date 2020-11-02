using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P01_FarmInList : Win.Subs.Base
    {
        private DataRow dr;

        /// <inheritdoc/>
        public P01_FarmInList(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(
                @"SELECT B.id, A.issuedate, B.QTY, A.Status , A.ADDDATE 
                                FROM FarmIn A WITH (NOLOCK) , FarmIn_Detail B WITH (NOLOCK) 
                                WHERE artworkpoid = '{0}' AND A.id = B.id AND b.artworkpo_detailukey = {1}",
                this.dr["id"].ToString(), this.dr["ukey"].ToString());
            DataTable selectDataTable1;

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridFarmInList.IsEditingReadOnly = true;
            this.gridFarmInList.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridFarmInList)
                 .Text("id", header: "Farm-In#", width: Widths.AnsiChars(13))
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(13))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Text("Status", header: "Status", width: Widths.AnsiChars(8))
                 .DateTime("adddate", header: "Create Date", width: Widths.AnsiChars(20));
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
