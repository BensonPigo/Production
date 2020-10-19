using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P01_DailyLock : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P01_DailyLock()
        {
            this.InitializeComponent();
            this.dateLock.Value = DateTime.Now.Date.AddDays(-1);
        }

        /// <inheritdoc/>
        public DateTime? LockDate { get; set; }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DateLock_Validating(object sender, CancelEventArgs e)
        {
            if (this.dateLock.Value < DateTime.Now.Date.AddDays(-1) || this.dateLock.Value > DateTime.Now.Date)
            {
                this.dateLock.Value = DateTime.Now.Date.AddDays(-1);
                MyUtility.Msg.WarningBox("You can only enter yesterday or today");
            }
        }

        private void BtnLock_Click(object sender, EventArgs e)
        {
            if (this.dateLock.Value < DateTime.Now.Date.AddDays(-1) || this.dateLock.Value > DateTime.Now.Date)
            {
                this.dateLock.Value = DateTime.Now.Date.AddDays(-1);
                MyUtility.Msg.WarningBox("You can only enter yesterday or today");
            }

            this.LockDate = this.dateLock.Value;
        }
    }
}
