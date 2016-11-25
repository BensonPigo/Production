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
    public partial class txtcustcd : Sci.Win.UI.TextBox
    {
        public txtcustcd()
        {
            this.Size = new System.Drawing.Size(125, 23);
        }

        private Control brandObject;
        [Category("Custom Properties")]
        public Control BrandObjectName
        {
            set { this.brandObject = value; }
            get { return this.brandObject; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item;
            //20161124 如果沒選Brandid,則條件帶空值,取消帶全部資料
            string selectCommand = "select ID, CountryID, City from CustCD order by ID";
            if (this.brandObject != null )//&& !string.IsNullOrWhiteSpace((string)this.brandObject.Text))
            {
                selectCommand = string.Format("select ID, CountryID, City from CustCD where BrandID = '{0}' order by ID", this.brandObject.Text);
            }
            item = new Sci.Win.Tools.SelectItem(selectCommand, "17,3,17", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string textValue = this.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "CustCD", "ID"))
                {
                    MyUtility.Msg.WarningBox(string.Format("< CustCD : {0} > not found!!!", textValue));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (this.brandObject != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.brandObject.Text))
                        {
                            string selectCommand = string.Format("select ID from CustCD where BrandID = '{0}' and ID = '{1}'", (string)this.brandObject.Text,this.Text.ToString());
                            if (!MyUtility.Check.Seek(selectCommand, null))
                            {
                                MyUtility.Msg.WarningBox(string.Format("< CustCD: {0} > not found!!!", textValue));
                                this.Text = "";
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
