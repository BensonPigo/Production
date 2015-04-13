using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtfactory : Sci.Win.UI.TextBox
    {
        public txtfactory()
        {
            #region PopUp
            this.PopUp += (s, e) =>
            {
                //Sci.Win.UI.TextBox textBox = (Sci.Win.UI.TextBox) s;
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Factory.id, NameEN", "10,50", this.Text, false, ",");
                // SELECT Id, NameEN FROM Factory WHERE Junk = 0
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = item.GetSelectedString();
            };
            #endregion

            #region Validating
            this.Validating += (s, e) =>
            {
                string str = this.Text;
                if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
                {
                    string tmp = myUtility.Lookup("id", str, "Factory", "id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Factory : {0} > not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        this.Text = tmp;
                    }

                }
            };
            #endregion
        }
    }
}
