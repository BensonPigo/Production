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
using Ict;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    public partial class txtcdcode : Sci.Win.UI.TextBox
    {
        public txtcdcode() 
        {
            this.Size = new System.Drawing.Size(60, 23);
        }

        private Control CPUObject;
        public string strCPUObjectName
        {
            set { this.getAllControls(this.FindForm(), value); }
        }

        // 取回指定的 Control 並存入 this.CPUObject
        private void getAllControls(Control container, string SearchName)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Name.ToString() == SearchName)
                {
                    this.CPUObject = c;
                }
                else
                {
                    if (c.Controls.Count > 0) this.getAllControls(c, SearchName);
                }
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (!myUtility.Seek(str, "CDCode", "Id"))
                {
                    MessageBox.Show(string.Format("< CD Code : {0} > not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (this.CPUObject != null)
            {
                this.CPUObject.Text = myUtility.Lookup("CPU", this.Text.ToString(), "CDCode", "ID");
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("CDCode.ID,Cpu,Description", "8,8,100", this.Text);
            // Select ID, Cpu, Description from CDCode where Junk = 0 order by ID
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
