using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtFromMdivision : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID from dbo.MDivision WITH (NOLOCK) where ID <> '{0}'", Sci.Env.User.Keyword), "8,40", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

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

        public txtFromMdivision()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
