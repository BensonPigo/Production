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
        private string fty = "";
        private Control factoryobject;	//欄位.存入要取值的<控制項>

        // 屬性. 利用字串來設定要存取的<控制項>
        [Category("Custom Properties")]
        public string factoryObjectName
        {
            set
            { this.getAllControls(this.FindForm(), value); }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            fty = factoryobject.Text;
            string sql = string.Format("Select id,factoryid From SewingLine Where FactoryId = '{0}'", fty);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "30,30", this.Text, false, ",");
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
                if (this.factoryobject==null)
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
                    fty = this.factoryobject.Text;
                    string tmp = myUtility.Lookup("id", fty + str, "SewingLine", "factoryid+id");
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
                    this.factoryobject = c;
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