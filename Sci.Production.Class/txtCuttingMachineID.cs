using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Sci;
using Ict;
using Sci.Win;

namespace Sci.Production.Class
{  
    public partial class txtCuttingMachineID : Sci.Win.UI.TextBox
    {
        private string mdivision = "";
        private string where = "";   //" Where junk = 0";

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;

            sql = "select distinct id from CuttingMachine WITH (NOLOCK) where junk = 0";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "15", this.Text, false, ",");
            item.Width = 300;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
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
                    this.Text = "";
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
                        this.Text = "";
                        MyUtility.Msg.WarningBox(string.Format("Cut Cell already junk, you can't choose!!"));
                    }

                }
            }
        }
        public txtCuttingMachineID()
        {
            this.Width = 150;
        }

    }
}