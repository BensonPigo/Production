using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtFromMdivision
    /// </summary>
    public partial class TxtFromMdivision : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtFromMdivision"/> class.
        /// </summary>
        public TxtFromMdivision()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select ID from dbo.MDivision WITH (NOLOCK) where ID <> '{0}'", Env.User.Keyword), "8,40", this.Text, false, ",");
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
                if (MyUtility.Check.Seek(str, "Mdivision", "id") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Mdivision ID : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
