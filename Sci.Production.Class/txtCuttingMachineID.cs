using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtCuttingMachineID
    /// </summary>
    public partial class TxtCuttingMachineID : Win.UI.TextBox
    {
        private string mdivision = string.Empty;
        private string where = string.Empty;   // " Where junk = 0";

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtCuttingMachineID"/> class.
        /// </summary>
        public TxtCuttingMachineID()
        {
            this.Width = 150;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;

            sql = "select distinct id from CuttingMachine WITH (NOLOCK) where junk = 0";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15", this.Text, false, ",")
            {
                Width = 300,
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string tmp = null;
                tmp = MyUtility.GetValue.Lookup("id", str, "CuttingMachine", "id");

                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< CuttingMachineID > : {0} not found!!!", str));
                    return;
                }
                else
                {
                    string cJunk = null;
                    cJunk = MyUtility.GetValue.Lookup("Junk", str, "CuttingMachine", "id");

                    if (cJunk == "True")
                    {
                        this.Text = string.Empty;
                        MyUtility.Msg.WarningBox(string.Format("Cut Cell already junk, you can't choose!!"));
                    }
                }
            }
        }
    }
}