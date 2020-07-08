using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class B01_History : Sci.Win.Subs.Base
    {
        protected DataRow motherData;

        public B01_History(DataRow data)
        {
            this.InitializeComponent();
            this.motherData = data;
            this.displayRefno.Text = this.motherData["refno"].ToString();
            this.displayDescription.Text = this.motherData["description"].ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(
                "select issuedate,localap.id,qty,unitid,localap.currencyid, price, qty*price amount" +
                ",localsuppid,localsupp.abb from localap WITH (NOLOCK) , localap_detail WITH (NOLOCK) ,localsupp WITH (NOLOCK) " +
                "where refno = '{0}' and localap.id = localap_detail.id and localsuppid = localsupp.id", this.motherData["refno"].ToString());
            DataTable selectDataTable1;

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridHistory.IsEditingReadOnly = true;
            this.gridHistory.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridHistory)
                 .Text("issuedate", header: "A/P Date", width: Widths.AnsiChars(10))
                 .Text("id", header: "A/P No", width: Widths.AnsiChars(16))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2)
                 .Text("Unitid", header: "Unit", width: Widths.AnsiChars(8))
                 .Text("Currencyid", header: "Currency", width: Widths.AnsiChars(3))
                 .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 4)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 14, decimal_places: 4)
                 .Text("localsuppid", header: "Supplier", width: Widths.AnsiChars(8))
                 .Text("abb", header: "Supplier Abb.", width: Widths.AnsiChars(15));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
