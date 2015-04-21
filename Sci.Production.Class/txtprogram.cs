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
    public partial class txtprogram : Sci.Win.UI.TextBox
    {
        private string brand = "";
        private Control brandObject;	//欄位.存入要取值的<控制項>

       
        // 屬性. 利用字串來設定要存取的<控制項>
        [Category("Custom Properties")]
        public string BrandObjectName
        {
            set
            { 
                this.getAllControls(this.FindForm(), value); 
            }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Program.Id,BrandID", "12,8", this.Text, false, ",");
            //select id from Program where Brandid = brand
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (this.brandObject == null)
                {
                    string tmp = myUtility.Lookup("Id", str, "Program", "Id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Program> : {0} not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    brand = this.brandObject.Text;
                    string tmp = myUtility.Lookup("id", str+brand, "Program", "Id+Brandid");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Program> : {0} not found!!!", str));
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
                    this.brandObject = c;
                }
                else
                {
                    if (c.Controls.Count > 0) this.getAllControls(c, SearchName);
                }
            }
        }
        public txtprogram()
        {
            this.Width =90;
        }

    }
}