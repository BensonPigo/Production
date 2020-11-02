using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtseason
    /// </summary>
    public partial class Txtseason : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtseason"/> class.
        /// </summary>
        public Txtseason()
        {
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Season.BrandID
        /// </summary>
        [Category("Custom Properties")]
        public Control BrandObjectName { get; set; }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string textValue = this.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "Season", "Id", "Production"))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Season : {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    if (this.BrandObjectName != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.BrandObjectName.Text))
                        {
                            string selectCommand = string.Format("select ID from Season WITH (NOLOCK) where BrandID = '{0}' and ID = '{1}'", (string)this.BrandObjectName.Text, this.Text.ToString());
                            if (!MyUtility.Check.Seek(selectCommand, "Production"))
                            {
                                this.Text = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< Season: {0} > not found!!!", textValue));
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item;
            string selectCommand = "select distinct ID from Production.dbo.Season WITH (NOLOCK) order by id desc";
            if (this.BrandObjectName != null && !string.IsNullOrWhiteSpace((string)this.BrandObjectName.Text))
            {
                selectCommand = string.Format("select distinct ID from Production.dbo.Season WITH (NOLOCK) where BrandID = '{0}' order by id desc", this.BrandObjectName.Text);
            }

            item = new Win.Tools.SelectItem(selectCommand, "11", this.Text)
            {
                Width = 300,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }
    }
}
