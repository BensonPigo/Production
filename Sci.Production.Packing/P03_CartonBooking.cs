using Ict;
using Sci.Production.PublicPrg;
using System;
using System.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P03_CartonBooking
    /// </summary>
    public partial class P03_CartonBooking : Win.Tems.QueryForm
    {
        private DataRow dataRow;

        /// <summary>
        /// Initializes a new instance of the <see cref="P03_CartonBooking"/> class.
        /// </summary>
        /// <param name="rowMaster">Master DataRow</param>
        public P03_CartonBooking(DataRow rowMaster)
        {
            this.InitializeComponent();
            this.dataRow = rowMaster;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            DateTime? dtBooking = MyUtility.Convert.GetDate(this.dataRow["EstCTNBooking"]);
            DateTime? dtArrived = MyUtility.Convert.GetDate(this.dataRow["EstCTNArrive"]);
            if (dtBooking.HasValue)
            {
                this.dateBoxCartonEstBooking.Text = dtBooking.Value.ToString("yyyy/MM/dd");
            }

            if (dtArrived.HasValue)
            {
                this.dateBoxCartonEstArrived.Text = dtArrived.Value.ToString("yyyy/MM/dd");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DateTime? dtBooking = MyUtility.Convert.GetDate(this.dateBoxCartonEstBooking.Text);
            DateTime? dtArrived = MyUtility.Convert.GetDate(this.dateBoxCartonEstArrived.Text);
            DualResult result = Prgs.UpdPackingListCTNBookingAndArrive(this.dataRow["ID"].ToString(), dtBooking, dtArrived);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Save fail!\r\n" + result.ToString());
                return;
            }

            MyUtility.Msg.InfoBox("Success");
            this.BtnClose_Click(null, null);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
