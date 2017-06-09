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

namespace Sci.Production.Class
{
    public partial class txtfactory : Sci.Win.UI.TextBox
    {
        public bool _IssupportJunk = false;

        public bool IssupportJunk
        {
            get { return _IssupportJunk; }
            set { _IssupportJunk = value;}
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sqlcmd;
            if (IssupportJunk)
            {
                sqlcmd = "Select DISTINCT FtyGroup as Factory from Factory WITH (NOLOCK) order by FtyGroup";
            }
            else
            {
                sqlcmd = "Select DISTINCT FtyGroup as Factory from Factory WITH (NOLOCK) where Junk = 0 order by FtyGroup";
            }
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            string sqlcmd;
            if (IssupportJunk)
            {
                sqlcmd = string.Format("Select DISTINCT FtyGroup from Factory WITH (NOLOCK) where FtyGroup='{0}'", str);
            }
            else
            {
                sqlcmd = string.Format("Select DISTINCT FtyGroup from Factory WITH (NOLOCK) where FtyGroup='{0}' and Junk = 0", str);
            }
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(sqlcmd) == false)
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                    return;
                }
            }
        }

        public txtfactory()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
