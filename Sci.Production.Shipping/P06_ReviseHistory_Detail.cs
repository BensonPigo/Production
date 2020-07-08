using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P06_ReviseHistory_Detail
    /// </summary>
    public partial class P06_ReviseHistory_Detail : Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P06_ReviseHistory_Detail
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P06_ReviseHistory_Detail(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.displaySPNo.Value = MyUtility.Convert.GetString(this.masterData["OrderID"]);
            this.displayStatusOld.Value = MyUtility.Convert.GetString(this.masterData["OldStatusExp"]);
            this.displayStatusRevised.Value = MyUtility.Convert.GetString(this.masterData["NewStatusExp"]);
            this.displayRevisedStatus.Value = MyUtility.Convert.GetString(this.masterData["ReviseStatus"]);
            this.numShipQtyOld.Value = MyUtility.Convert.GetInt(this.masterData["OldShipQty"]);
            this.numShipQtyRevised.Value = MyUtility.Convert.GetInt(this.masterData["NewShipQty"]);

            this.gridReviseHistoryDetail.IsEditingReadOnly = true;
            this.gridReviseHistoryDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReviseHistoryDetail)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("OldShipQty", header: "Old Q'ty", width: Widths.AnsiChars(6))
                .Numeric("NewShipQty", header: "New Q'ty", width: Widths.AnsiChars(6));

            string sqlCmd = string.Format(
                @"select * 
from Pullout_Revise_Detail prd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = prd.OrderID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = prd.SizeCode
where Pullout_ReviseReviseKey = {0}
order by os.Seq", MyUtility.Convert.GetString(this.masterData["ReviseKey"]));

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
