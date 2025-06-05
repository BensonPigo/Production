using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_SubconGarmentWash : Sci.Win.Tems.QueryForm
    {
        private string orderID;

        /// <inheritdoc/>
        public P01_SubconGarmentWash(string orderId)
        {
            this.InitializeComponent();
            this.orderID = orderId;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetupFarmOutGrid();
            this.SetupFarmInGrid();
            this.GetData();
        }

        private void SetupFarmOutGrid()
        {
            this.gridFarmOut.DataSource = null;
            this.Helper.Controls.Grid.Generator(this.gridFarmOut)
                .Date("FarmOutDate", "Date", width: Widths.AnsiChars(11))
                .Numeric("Qty", "Qty", width: Widths.AnsiChars(8));
        }

        private void SetupFarmInGrid()
        {
            this.gridFarmIn.DataSource = null;
            this.Helper.Controls.Grid.Generator(this.gridFarmIn)
                .Date("FarmInDate", "Date", width: Widths.AnsiChars(11))
                .Numeric("Qty", "Qty", width: Widths.AnsiChars(8));
        }

        private void GetData()
        {
            var sqlcmd = @"
select OrderId, FarmOutDate, Qty = SUM(Qty)
from View_SewingOutput_FarmInOutDate 
where OrderId = @OrderID
group by OrderId, FarmOutDate

select OrderId, FarmInDate, Qty = SUM(Qty)
from View_SewingOutput_FarmInOutDate 
where OrderId = @OrderID
group by OrderId, FarmInDate";

            using (var result = DBProxy.Current.SelectEx<DataSet>(sqlcmd, false, "@OrderID", this.orderID))
            {
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return;
                }

                var dtFarmOut = result.ExtendedData.Tables[0];
                var dtFarmIn = result.ExtendedData.Tables[1];

                this.gridFarmOut.DataSource = dtFarmOut;
                this.gridFarmIn.DataSource = dtFarmIn;
            }
        }
    }
}
