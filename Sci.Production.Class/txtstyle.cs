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
    public partial class txtstyle : Sci.Win.UI.TextBox
    {
        public txtstyle()
        {
            this.Size = new System.Drawing.Size(130, 23);
        }

        private Control brandObject;
        [Category("Custom Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Control BrandObjectName
        {
            set { this.brandObject = value; }
            get { return this.brandObject; }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string textValue = this.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!myUtility.Seek(textValue, "Style", "ID"))
                {
                    MessageBox.Show(string.Format("< Style : {0} > not found!!!", textValue));
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
                            string selectCommand = string.Format("select ID from Style where BrandID = '{0}' and ID = '{1}'", (string)this.brandObject.Text, this.Text.ToString());
                            if (!myUtility.Seek(selectCommand,null))
                            {
                                MessageBox.Show(string.Format("< Brand + Style: {0} + {1} > not found!!!", (string)this.brandObject.Text, textValue));
                                this.Text = "";
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item;
            string selectCommand;
            selectCommand = "select ID,SeasonID,Description,BrandID from Style where Junk = 0 order by ID";
            if (this.brandObject != null && !string.IsNullOrWhiteSpace((string)this.brandObject.Text))
            {
                selectCommand = string.Format("select ID,SeasonID,Description,BrandID from Style where Junk = 0 and BrandID = '{0}' order by ID", this.brandObject.Text);
            }
            item = new Sci.Win.Tools.SelectItem(selectCommand, "16,10,50,8", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
