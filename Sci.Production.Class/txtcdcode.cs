using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtcdcode
    /// </summary>
    public partial class Txtcdcode : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtcdcode"/> class.
        /// </summary>
        public Txtcdcode()
        {
            this.Size = new System.Drawing.Size(54, 23);
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string textValue = this.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "CDCode", "Id"))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< CD Code : {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select ID, convert(nvarchar(5),Cpu) Cpu, Description from CDCode WITH (NOLOCK) where Junk = 0 order by ID", "7,6,45", this.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }
    }
}
