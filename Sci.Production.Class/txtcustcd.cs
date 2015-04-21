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
        public string BrandObjectName
        {
            set { this.GetAllControls(this.FindForm(), value); }
        }

        // 取回指定的 Control 並存入 this.BrandObject
        private void GetAllControls(Control container, string searchName)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Name.ToString() == searchName)
                {
                    this.brandObject = c;
                }
                else
                {
                    if (c.Controls.Count > 0) this.GetAllControls(c, searchName);
                }
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            if (this.brandObject == null)
            {
                item = new Sci.Win.Tools.SelectItem("CustCD.ID,CountryID, City", "10,5,25", this.Text);
                // select ID, CountryID, City from CustCD
            }
            else
            {
                string selectCommand;
                if (!string.IsNullOrWhiteSpace((string)this.brandObject.Text))
                {
                    selectCommand = string.Format("select ID, CountryID, City from CustCD where BrandID = '{0}' ", this.brandObject.Text);
                }
                else
                {
                    selectCommand = "select ID, CountryID, City from CustCD";
                }
                DataTable selectDatatable;
                DualResult selectResult = DBProxy.Current.Select(null, selectCommand, out selectDatatable);
                //item = new Sci.Win.Tools.SelectItem(selectDatatable, "Id", "20", this.Text, false, ",");
                item = new Sci.Win.Tools.SelectItem("CustCD.ID,CountryID, City", "10,5,25", this.Text);
            }
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
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
