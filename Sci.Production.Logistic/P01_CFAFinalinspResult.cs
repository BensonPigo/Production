using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class P01_CFAFinalinspResult : Sci.Win.Tems.QueryForm
    {
        private string OrderID;

        /// <inheritdoc/>
        public P01_CFAFinalinspResult(string orderID)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.OrderID = orderID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.grid)
               .Text("Seq", header: "Seq", width: Widths.AnsiChars(4), iseditingreadonly: true)
               .Text("ShipmodeID", header: "ShipMode", width: Widths.AnsiChars(6), iseditingreadonly: true)
               .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Stage", header: "Stage", width: Widths.AnsiChars(6), iseditingreadonly: true)
               .Text("Result", header: "Result", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Date("CFAFinalInspectDate", header: "Last Audit Date", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.QueryData();
        }

        private void QueryData()
        {
            string sqlcmd = $@"
select Seq
,ShipmodeID
,BuyerDelivery
,Stage = 'Final'
,Result = case when CFAFinalInspectResult in ('Pass','Fail but release') then 'Pass'
else ISNULL( CFAFinalInspectResult,'') end
,CFAFinalInspectDate
from Order_QtyShip
where Id='{this.OrderID}'
order by seq
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.grid.DataSource = dtResult;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}