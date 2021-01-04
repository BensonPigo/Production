using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class LinkLabelRFCardEraseBeforePrinting : LinkLabel
    {
        /// <inheritdoc/>
        protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        {
            this.UpdateStatus();
            this.SetText();
            base.OnLinkClicked(e);
        }

        /// <summary>
        /// Set Text
        /// </summary>
        public void SetText()
        {
            this.Text = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select RFCardEraseBeforePrinting from [System]")) ?
                         "RF. Erase ON" : "RF. Erase OFF";
        }

        private void UpdateStatus()
        {
            string sqlCmd = "Update s set RFCardEraseBeforePrinting = iif(RFCardEraseBeforePrinting = 0, 1, 0) from System s";
            Data.DBProxy.Current.Execute("Production", sqlCmd);
        }
    }
}
