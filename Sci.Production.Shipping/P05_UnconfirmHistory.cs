using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P05_UnconfirmHistory : Win.Subs.Base
    {
        private string GMTBookingID;

        /// <summary>
        /// Initializes a new instance of the <see cref="P05_UnconfirmHistory"/> class.
        /// </summary>
        /// <param name="gMTBookingID">GMTBookingID</param>
        public P05_UnconfirmHistory(string gMTBookingID)
        {
            this.InitializeComponent();
            this.GMTBookingID = gMTBookingID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
            .Date("AddDate", header: "Unconfirm Date", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("Name", header: "Reason", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("Remark", header: "Reason Description", width: Widths.AnsiChars(40), iseditingreadonly: true)
            ;

            this.Query();
        }

        private void Query()
        {
            string cmd = string.Empty;

            cmd = $@"
SELECT g.AddDate ,r.Name,g.Remark
FROM GMTBooking_History g
LEFT JOIN Reason r ON g.ReasonID=r.ID 
WHERE g.ID='{this.GMTBookingID}'
AND g.HisType='GBUnCFM' AND r.ReasonTypeID='GMTBooking_UnCFM'
ORDER BY g.AddDate
";
            DataTable gridData;
            if (DBProxy.Current.Select(null, cmd, out gridData))
            {
                this.listControlBindingSource1.DataSource = gridData;
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query data fail!!");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
