using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtsewingline : Win.UI.TextBox
    {
        private string fty = string.Empty;
        public Control factoryObject = null;    // 欄位.存入要取值的<控制項>

        // 屬性. 利用Control來設定要存取的<控制項>
        [Category("Custom Properties")]
        public Control factoryobjectName
        {
            get
            {
                return this.factoryObject;
            }

            set
            {
                this.factoryObject = value;
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            if (this.factoryObject == null || MyUtility.Check.Empty(this.factoryObject.Text))
            {
                this.fty = string.Empty;
            }
            else
            {
                this.fty = this.factoryObject.Text;
            }

            string ftyWhere = string.Empty;
            if (!this.fty.Empty())
            {
                ftyWhere = string.Format("Where FactoryId = '{0}'", this.fty);
            }

            string sql = string.Format("Select ID,FactoryID,Description From Production.dbo.SewingLine WITH (NOLOCK) {0} ", ftyWhere);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "2,6,16", this.Text, false, ",");
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
                if (this.factoryObject == null || MyUtility.Check.Empty(this.factoryObject.Text))
                {
                    string tmp = MyUtility.GetValue.Lookup("ID", str, "SewingLine", "id", "Production");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        e.Cancel = true;
                        this.Text = string.Empty;
                        MyUtility.Msg.WarningBox(string.Format("< Sewing Line> : {0} not found!!!", str));
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace((string)this.factoryObject.Text))
                    {
                        string selectCommand = string.Format("select ID from Production.dbo.SewingLine WITH (NOLOCK) where FactoryID = '{0}' and ID = '{1}'", (string)this.factoryObject.Text, this.Text.ToString());
                        if (!MyUtility.Check.Seek(selectCommand, null))
                        {
                            e.Cancel = true;
                            this.Text = string.Empty;
                            MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", str));
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