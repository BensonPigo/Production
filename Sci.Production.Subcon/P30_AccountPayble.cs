using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P30_AccountPayble : Win.Subs.Base
    {
        protected string Detailukey;

        public P30_AccountPayble(string _detailukey)
        {
            this.InitializeComponent();
            this.Detailukey = _detailukey;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(
                @"
                    select LD.Id,L.IssueDate,LD.Qty,LD.Price,P.ID+' '+P.Name+ IIF(P.ExtNo='','',' Ext.'+P.ExtNo) as Handle,L.ApvDate
                    from LocalAP_Detail LD
                    inner join LocalAP L on L.Id=LD.Id
                    left join Pass1 P on L.Handle=P.ID
                    WHERE LD.LocalPo_detailukey='{0}'",
                this.Detailukey);
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
                 .Text("id", header: "A/P#", width: Widths.AnsiChars(15))
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4)
                 .Text("Handle", header: "Handle", width: Widths.AnsiChars(30))
                 .Date("ApvDate", header: "Approved", width: Widths.AnsiChars(10));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
