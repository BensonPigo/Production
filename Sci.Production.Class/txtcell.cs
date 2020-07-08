using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtCell : Win.UI.TextBox
    {
        private string mdivision = string.Empty;
        private string where = string.Empty;   // " Where junk = 0";

        [Category("Custom Properties")]
        public string MDivisionID
        {
            get { return this.mdivision; }
            set { this.mdivision = value; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;
            if (!string.IsNullOrWhiteSpace(this.mdivision))
            {
                this.where = string.Format(" Where junk = 0 and mdivisionid = '{0}'", this.mdivision);
            }

            sql = "select distinct id from Production.dbo.CutCell WITH (NOLOCK) " + this.where;
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "2", this.Text, false, ",");
            item.Width = 300;
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
                string tmp = null;
                if (!string.IsNullOrWhiteSpace(this.mdivision))
                {
                    tmp = MyUtility.GetValue.Lookup("id", this.mdivision + str, "Production.dbo.Cutcell", "mdivisionid+id");
                }
                else
                {
                    tmp = MyUtility.GetValue.Lookup("id", str, "Production.dbo.Cutcell", "id");
                }

                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Cut Cell> : {0} not found!!!", str));
                    return;
                }
                else
                {
                    string cJunk = null;
                    if (!string.IsNullOrWhiteSpace(this.mdivision))
                    {
                        cJunk = MyUtility.GetValue.Lookup("Junk", this.mdivision + str, "Production.dbo.CutCell", "mdivisionid+id");
                    }
                    else
                    {
                        cJunk = MyUtility.GetValue.Lookup("Junk", str, "Production.dbo.CutCell", "id");
                    }

                    if (cJunk == "True")
                    {
                        this.Text = string.Empty;
                        MyUtility.Msg.WarningBox(string.Format("Cut Cell already junk, you can't choose!!"));
                    }
                }
            }
        }

        public txtCell()
        {
            this.Width = 30;
        }
    }
}