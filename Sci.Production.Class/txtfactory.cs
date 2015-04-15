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
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Factory.id, NameEN", "10,50", this.Text, false, ",");
            // SELECT Id, NameEN FROM Factory WHERE Junk = 0
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (myUtility.Seek(str,"factory","id")==false)
                {
                    MessageBox.Show(string.Format("< Factory : {0} > not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
               
            }
        }

        public txtfactory()
        {
        }
    }
}
