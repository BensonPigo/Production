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
            this.Size = new System.Drawing.Size(115, 23);
        }

        private Control brandObject;
        [Category("Custom Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Control BrandObjectName
        {
            set { this.brandObject = value; }
            get { return this.brandObject; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item;
            string selectCommand = "select ID, CountryID, City from CustCD order by ID";
            if (this.brandObject != null && !string.IsNullOrWhiteSpace((string)this.brandObject.Text))
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
                if (!myUtility.Seek(textValue, "CustCD", "ID"))
                {
                    MessageBox.Show(string.Format("< CustCD : {0} > not found!!!", textValue));
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
                            if (!myUtility.Seek((string)this.brandObject.Text + this.Text.ToString(), "CustCD", "BrandID+ID"))
                            {
                                MessageBox.Show(string.Format("< CustCD: {0} > not found!!!", textValue));
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
