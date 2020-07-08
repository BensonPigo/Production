using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtbuyer : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID, NameEN from Buyer WITH (NOLOCK) where Junk = 0 order by ID", "10,50", this.Text, false, ",");

            // select id, NameEN from buyer where junk = 0
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
                if (MyUtility.Check.Seek(str, "Buyer", "id") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Buyer : {0} > not found!!!", str));
                    return;
                }
            }
        }

        public txtbuyer()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
