using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P01_Ap : Win.Subs.Base
    {
        private DataRow dr;

        /// <inheritdoc/>
        public P01_Ap(DataRow data)
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
                @"SELECT B.ID,A.issuedate,B.ApQty,B.PRICE,A.Handle,A.apvdate 
                                                    FROM ArtworkAP A WITH (NOLOCK) , ArtworkAP_Detail B WITH (NOLOCK) 
	                                                WHERE A.ID = B.ID  AND B.artworkPOID = '{0}' and b.artworkpo_detailukey = {1}",
                this.dr["id"].ToString(), this.dr["ukey"].ToString());
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccountPayable.IsEditingReadOnly = true;
            this.gridAccountPayable.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccountPayable)
                 .Text("id", header: "A/P#", width: Widths.AnsiChars(13))
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(13))
                 .Numeric("ApQty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), integer_places: 13, decimal_places: 4)
                 .Text("Handle", header: "Handle", width: Widths.AnsiChars(8))
                 .DateTime("apvdate", header: "Approve Date", width: Widths.AnsiChars(20));
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
