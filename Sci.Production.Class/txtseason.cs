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
    public partial class txtseason : Sci.Win.UI.TextBox
    {
        public txtseason()
        {
            this.Size = new System.Drawing.Size(78, 23);
        }

        private Control brandObject;
        public string BrandObjectName
        {
            set { this.getAllControls(this.FindForm(), value); }
        }

        // 取回指定的 Control 並存入 this.BrandObject
        private void getAllControls(Control container, string searchName)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Name.ToString() == searchName)
                {
                    this.brandObject = c;
                }
                else
                {
                    if (c.Controls.Count > 0) this.getAllControls(c, searchName);
                }
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            string textValue = this.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!myUtility.Seek(textValue, "Season", "Id"))
                {
                    MessageBox.Show(string.Format("< Season : {0} > not found!!!", textValue));
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
                            if (!myUtility.Seek((string)this.brandObject.Text + this.Text.ToString(), "Season", "BrandID+ID"))
                            {
                                MessageBox.Show(string.Format("< Season: {0} > not found!!!", textValue));
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
            Sci.Win.Tools.SelectItem item;
            if (this.brandObject == null)
            {
                item = new Sci.Win.Tools.SelectItem("Season.Id", "20", this.Text, false, ",");
            }
            else
            {
                string selectCommand;
                if (!string.IsNullOrWhiteSpace((string)this.brandObject.Text))
                {
                    selectCommand = string.Format("select Id from Season where BrandID = '{0}' ", this.brandObject.Text);
                }
                else
                {
                    selectCommand = "select Id from Season";
                }
                DataTable selectDatatable;
                DualResult selectResult = DBProxy.Current.Select(null, selectCommand, out selectDatatable);
                //item = new Sci.Win.Tools.SelectItem(selectDatatable, "Id", "20", this.Text, false, ",");
                item = new Sci.Win.Tools.SelectItem("Season.Id", "20", this.Text, false, ",");
            }
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
