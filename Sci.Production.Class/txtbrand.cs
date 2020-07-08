using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtbrand : Sci.Win.UI.TextBox
    {
        private bool multi_select = false;

        public bool MultiSelect
        {
            set
            {
               this.multi_select = value;
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Production.dbo.Brand WITH (NOLOCK) WHERE Junk=0  ORDER BY Id";
            if (this.multi_select)
            {
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sqlWhere, string.Empty, "10,29,35", string.Empty, null, null, null);
                item.Size = new System.Drawing.Size(810, 666);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
            }
            else
            {
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "10,29,35", this.Text, false, ",");
                item.Size = new System.Drawing.Size(777, 666);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
            }

            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string[] str_multi = str.Split(',');
                if (str_multi.Length > 1)
                {
                    string err_brand = string.Empty;
                    foreach (string chk_str in str_multi)
                    {
                        if (MyUtility.Check.Seek(chk_str, "Brand", "id", "Production") == false)
                        {
                            err_brand += "," + chk_str;
                        }
                    }

                    if (!err_brand.Equals(string.Empty))
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Brand : {0} > not found!!!", err_brand.Substring(1)));
                        return;
                    }
                }
                else
                {
                    if (MyUtility.Check.Seek(str, "Brand", "id", "Production") == false)
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Brand : {0} > not found!!!", str));
                        return;
                    }
                }
            }
        }

        public txtbrand()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
