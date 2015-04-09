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

namespace Sci.Production.Class
{
    public partial class txtdepartment : Sci.Win.UI.TextBox
    {
        public txtdepartment()
        {
            //this._Alias = "department";
            //this._Tag = "ID";
            //this.HelpColumnWidths = "10,50";
            //this.HelpRecordSource = "department.id,Name";
            //this.Size = new System.Drawing.Size(100, 22);

            //this.IsSupportSytsemContextMenu = false;
            this.PopUp += (s, e) =>
            {
                //Sci.Win.UI.TextBox textBox = (Sci.Win.UI.TextBox) s;
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("department.id,Name", "10,50", this.Text, false, ","); 
                // select  id,Name from department where junk = 0
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = item.GetSelectedString();
            };

            this.Validating += (s, e) =>
            {
                string str = this.Text;
                if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
                {
                    string tmp = myUtility.Lookup("id", str, "department", "id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Department : {0} > not found!!!", str));
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
        }
    }
}
