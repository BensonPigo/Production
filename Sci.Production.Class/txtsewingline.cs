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
        public Control factoryObject;	//欄位.存入要取值的<控制項>

        // 屬性. 利用Control來設定要存取的<控制項>
        [Category("Custom Properties")]
        public Control factoryobjectName
        {
            set
            {
                factoryObject = value;
            }
            get
            {
                return factoryObject;
            }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            fty = factoryObject.Text;
            string sql = string.Format("Select ID,FactoryID,Description From SewingLine Where FactoryId = '{0}'", fty);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "2,8,16", this.Text, false, ",");
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
                if (this.factoryObject == null)
                {
                    string tmp = MyUtility.GetValue.Lookup("ID", str, "SewingLine", "id");
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
                    if (!string.IsNullOrWhiteSpace((string)this.factoryObject.Text))
                    {
                        string selectCommand = string.Format("select ID from SewingLine where FactoryID = '{0}' and ID = '{1}'", (string)this.factoryObject.Text, this.Text.ToString());
                        if (!MyUtility.Check.Seek(selectCommand, null))
                        {
                            MessageBox.Show(string.Format("< Sewing Line: {0} > not found!!!", this.Text.ToString()));
                            this.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }
        public txtsewingline()
        {
            this.Width = 60;
        }

    }
}