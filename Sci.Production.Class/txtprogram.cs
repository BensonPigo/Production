using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtprogram
    /// </summary>
    public partial class Txtprogram : Win.UI.TextBox
    {
        private string brand;

        /// <summary>
        /// Program.Brandid
        /// </summary>
        [Category("Custom Properties")]
        public Control BrandObjectName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtprogram"/> class.
        /// </summary>
        public Txtprogram()
        {
            this.Width = 95;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            this.brand = this.BrandObjectName.Text;
            string sql = string.Format("Select id,BrandID from Program WITH (NOLOCK) where Brandid = '{0}'", this.brand);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "12,8", this.Text, false, ",");
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
                if (this.BrandObjectName == null)
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
                    this.brand = this.BrandObjectName.Text;
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
    }
}