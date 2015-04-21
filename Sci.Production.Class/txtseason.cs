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
            //this.PopUp += (s, e) =>
            //{
            //    Sci.Win.Tools.SelectItem item;
            //    if (this.BrandObject == null)
            //    {
            //        string str = string.Format("select Id from Season where junk = 0 and BrandID = '{0}' ", this.BrandObject.Text);
            //        item = new Sci.Win.Tools.SelectItem("Season.Id", "20", this.Text, false, ",");
            //        // select Id from Season where junk = 0 and BrandID = this.BrandObject.Text
            //    }
            //    else
            //    {
            //        MessageBox.Show(this.BrandObject.Text.ToString());
            //        string str = string.Format("select Id from Season where junk = 0 and BrandID = '{0}' ", this.BrandObject.Text);
            //        DataTable dt;
            //        DualResult Result = DBProxy.Current.Select(null, str, out dt);
            //        if (dt.Rows.Count == 0)
            //        {
            //            MessageBox.Show("Data not found!!!" + str);
            //            return;
            //        }
            //        else
            //        {
            //            item = new Sci.Win.Tools.SelectItem(dt, "Id", "20", this.Text, false, ",");
            //        }
            //        // select Id from Season where junk = 0
            //    }
            //    DialogResult result2 = item.ShowDialog();
            //    if (result2 == DialogResult.Cancel) { return; }
            //    this.Text = item.GetSelectedString();
            //};
        }

        private Control BrandObject;
        public string strBrandObjectName
        {
            set { this.getAllControls(this.FindForm(), value); }
        }

        // 取回指定的 Control 並存入 this.BrandObject
        private void getAllControls(Control container, string SearchName)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Name.ToString() == SearchName)
                {
                    this.BrandObject = c;
                }
                else
                {
                    if (c.Controls.Count > 0) this.getAllControls(c, SearchName);
                }
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (!myUtility.Seek(str, "Season", "Id"))
                {
                    MessageBox.Show(string.Format("< Season : {0} > not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (this.BrandObject != null)
                    {
                        string lookupResult = myUtility.Lookup("BrandID", this.Text.ToString(), "Season", "ID");
                        string brandValue = (string)this.BrandObject.Text;
                        if (lookupResult == brandValue)
                        {
                            MessageBox.Show(string.Format("< Season: {0} > not found!!!", str));
                            this.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            if (this.BrandObject == null)
            {
                //string str = string.Format("select Id from Season where junk = 0 and BrandID = '{0}' ", this.BrandObject.Text);
                item = new Sci.Win.Tools.SelectItem("Season.Id", "20", this.Text, false, ",");
                // select Id from Season where junk = 0 and BrandID = this.BrandObject.Text
            }
            else
            {
                //MessageBox.Show(this.BrandObject.Text.ToString());

                //string str = string.Format("select Id from Season where junk = 0 and BrandID = '{0}' ", this.BrandObject.Text);
                //DataTable dt;
                //DualResult Result = DBProxy.Current.Select(null, str, out dt);
                //if (dt.Rows.Count == 0)
                //{
                //    MessageBox.Show("Data not found!!!" + str);
                //    return;
                //}
                //else
                //{
                //    item = new Sci.Win.Tools.SelectItem(dt, "Id", "20", this.Text, false, ",");
                //}
                // select Id from Season where junk = 0

                item = new Sci.Win.Tools.SelectItem("Season.Id", "20", this.Text, false, ",");
            }
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
