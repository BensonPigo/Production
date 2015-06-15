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
        private string brand ;
        private Control brandObject;	//欄位.存入要取值的<控制項>

       
        // 屬性. 利用字串來設定要存取的<控制項>
        [Category("Custom Properties")]
        public Control BrandObjectName
        {
            set
            {
                brandObject = value; 
            }
            get
            {
                return brandObject;
            }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            brand = brandObject.Text;
            string sql = string.Format("Select id,BrandID from Program where Brandid = '{0}'", brand);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "12,8", this.Text, false, ",");
            //
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) 
            { 
                return; 
            }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
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
        public txtprogram()
        {
            this.Width =95;
        }

    }
}