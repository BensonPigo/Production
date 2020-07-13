using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtbuyer
    /// </summary>
    public partial class Txtbuyer : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtbuyer"/> class.
        /// </summary>
        public Txtbuyer()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID, NameEN from Buyer WITH (NOLOCK) where Junk = 0 order by ID", "10,50", this.Text, false, ",");

            // select id, NameEN from buyer where junk = 0
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
                if (MyUtility.Check.Seek(str, "Buyer", "id") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Buyer : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
