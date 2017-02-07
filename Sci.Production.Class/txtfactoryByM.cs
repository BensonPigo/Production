using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using Ict;

namespace Sci.Production.Class
{
    public partial class txtfactoryByM : Sci.Win.UI.TextBox
    {
        public string mDivisionID { get; set; }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID,NameEN from Factory where Junk = 0 and MDivisionID = '{0}' order by ID", mDivisionID), "8,40", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                DataTable dt;
                DualResult result = DBProxy.Current.Select(null, string.Format("select ID,NameEN from Factory where Junk = 0 and MDivisionID = '{0}' and ID = '{1}'", mDivisionID, str), out dt);
                if (result.Result)
                {
                    if (dt != null && dt.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox(result.Description);
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        public txtfactoryByM()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
