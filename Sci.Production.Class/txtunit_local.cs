using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtunit_local : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID from dbo.LocalUnit WITH (NOLOCK) where junk = 0 order by ID", "10,40", this.Text, false, ",");
            item.Width = 260;
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
                if (MyUtility.Check.Seek($"select id from dbo.LocalUnit where id ='{str}' and junk = 0") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Unit : {0} > not found!!!", str));
                    return;
                }
            }
        }

        public txtunit_local()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
