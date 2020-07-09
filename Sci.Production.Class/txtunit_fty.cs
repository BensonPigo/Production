using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtunit_fty
    /// </summary>
    public partial class Txtunit_fty : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtunit_fty"/> class.
        /// </summary>
        public Txtunit_fty()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Description from Unit WITH (NOLOCK) order by ID", "10,40", this.Text, false, ",");
            item.Width = 580;
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
                if (MyUtility.Check.Seek(str, "unit", "id") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Unit : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
