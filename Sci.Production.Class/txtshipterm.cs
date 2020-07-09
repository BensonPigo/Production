using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtshipterm
    /// </summary>
    public partial class Txtshipterm : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtshipterm"/> class.
        /// </summary>
        public Txtshipterm()
        {
            this.Size = new System.Drawing.Size(50, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Description from ShipTerm WITH (NOLOCK) order by ID", "6,80", this.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(str, "shipterm", "id") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Ship Term : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
