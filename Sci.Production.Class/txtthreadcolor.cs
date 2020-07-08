using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtthreadcolor : Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select id,description from threadcolor WITH (NOLOCK) where junk=0", "23,40", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string tmp = MyUtility.GetValue.Lookup("id", str, "threadcolor", "id");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Thread Color> : {0} not found!!!", str));
                    return;
                }

                string cjunk = MyUtility.GetValue.Lookup("Junk", str, "threadcolor", "id");
                if (cjunk == "True")
                {
                    this.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("Thread Color already junk, you can't choose!!"));
                }
            }
        }

        public txtthreadcolor()
        {
            this.Width = 90;
            this.IsSupportSytsemContextMenu = false;
        }
    }
}