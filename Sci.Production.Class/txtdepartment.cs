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
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name from Department where Junk = 0 order by ID", "10,50", this.Text);
            // select  id,Name from department where junk = 0
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            this.OnValidated(e);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (myUtility.Seek(str,"department","id")==false)
                {
                    MessageBox.Show(string.Format("< Department : {0} > not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        public txtdepartment()
        {
            this.Size = new System.Drawing.Size(80, 23);
            //this._Alias = "department";
            //this._Tag = "ID";
            //this.HelpColumnWidths = "10,50";
            //this.HelpRecordSource = "department.id,Name";
            //this.Size = new System.Drawing.Size(100, 22);

            //this.IsSupportSytsemContextMenu = false;

        }
    }
}
