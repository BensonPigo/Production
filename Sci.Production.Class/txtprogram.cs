using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtprogram : Sci.Win.UI.TextBox
    {
        private string brand;
        private Control brandObject;    // 欄位.存入要取值的<控制項>

        // 屬性. 利用字串來設定要存取的<控制項>
        [Category("Custom Properties")]
        public Control BrandObjectName
        {
            get
            {
                return this.brandObject;
            }

            set
            {
                this.brandObject = value;
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            this.brand = this.brandObject.Text;
            string sql = string.Format("Select id,BrandID from Program WITH (NOLOCK) where Brandid = '{0}'", this.brand);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "12,8", this.Text, false, ",");
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
                    string tmp = MyUtility.GetValue.Lookup("Id", str, "Program", "Id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Program> : {0} not found!!!", str));
                        return;
                    }
                }
                else
                {
                    this.brand = this.brandObject.Text;
                    string tmp = MyUtility.GetValue.Lookup("id", str + this.brand, "Program", "Id+Brandid");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Program> : {0} not found!!!", str));
                        return;
                    }
                }
            }
        }

        public txtprogram()
        {
            this.Width = 95;
        }
    }
}