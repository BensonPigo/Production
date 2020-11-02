using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtunit_local
    /// </summary>
    public partial class Txtunit_local : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtunit_local"/> class.
        /// </summary>
        public Txtunit_local()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID from dbo.LocalUnit WITH (NOLOCK) where junk = 0 order by ID", "10,40", this.Text, false, ",")
            {
                Width = 260,
            };
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
                if (MyUtility.Check.Seek($"select id from dbo.LocalUnit where id ='{str}' and junk = 0") == false)
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
