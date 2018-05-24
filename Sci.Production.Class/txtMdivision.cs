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
    public partial class txtMdivision : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID from Production.dbo.MDivision WITH (NOLOCK) ", "8", this.Text, false, ",");
            item.Size = new System.Drawing.Size(300, 250);
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
                if (MyUtility.Check.Seek(string.Format("select ID from Production.dbo.MDivision WITH (NOLOCK) where id = '{0}'", str)) == false)
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< M : {0} > not found!!!", str));
                    return;
                }
            }
        }

        public txtMdivision()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
