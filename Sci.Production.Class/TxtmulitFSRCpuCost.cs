using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtmultifactory
    /// </summary>
    public partial class TxtmulitFSRCpuCost : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtmultifactory"/> class.
        /// </summary>
        public TxtmulitFSRCpuCost()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql = "select distinct ShipperID from FSRCpuCost WITH (NOLOCK)";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "Shipper", "10", this.Text, null, null, null);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }
    }
}
