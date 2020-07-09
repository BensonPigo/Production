using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtsewingline
    /// </summary>
    public partial class Txtsewingline : Win.UI.TextBox
    {
        private string fty = string.Empty;

        /// <summary>
        /// SewingLine.FactoryID
        /// </summary>
        [Category("Custom Properties")]
        public Control FactoryobjectName { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtsewingline"/> class.
        /// </summary>
        public Txtsewingline()
        {
            this.Width = 60;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            if (this.FactoryobjectName == null || MyUtility.Check.Empty(this.FactoryobjectName.Text))
            {
                this.fty = string.Empty;
            }
            else
            {
                this.fty = this.FactoryobjectName.Text;
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

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (this.FactoryobjectName == null || MyUtility.Check.Empty(this.FactoryobjectName.Text))
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
                    if (!string.IsNullOrWhiteSpace((string)this.FactoryobjectName.Text))
                    {
                        string selectCommand = string.Format("select ID from Production.dbo.SewingLine WITH (NOLOCK) where FactoryID = '{0}' and ID = '{1}'", (string)this.FactoryobjectName.Text, this.Text.ToString());
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
    }
}