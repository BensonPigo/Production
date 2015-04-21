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
    public partial class txtsewingline : Sci.Win.UI.TextBox
    {
        private string cfty = "";
        private Control cfactoryobject;	//欄位.存入要取值的<控制項>

        // 屬性. 利用字串來設定要存取的<控制項>
        [Category("Custom Properties")]
        public string strFactoryObjectName
        {
            set
            { this.getAllControls(this.FindForm(), value); }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("SewingLine.id,factoryid", "30,30", this.Text, false, ",");
            //select id from SewingLine where factoryid = cfty and !junk
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (this.cfactoryobject==null)
                {
                    string tmp = myUtility.Lookup("id", str, "SewingLine", "id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Sewing Line> : {0} not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    cfty = this.cfactoryobject.Text;
                    string tmp = myUtility.Lookup("id", cfty + str, "SewingLine", "factoryid+id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Sewing Line> : {0} not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }
        // 取回指定的 Control 並存入 this.BuyerObject
        private void getAllControls(Control container, string SearchName)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Name.ToString() == SearchName)
                {
                    this.cfactoryobject = c;
                }
                else
                {
                    if (c.Controls.Count > 0) this.getAllControls(c, SearchName);
                }
            }
        }
        public txtsewingline()
        {
            this.Width = 60;
        }

    }
}