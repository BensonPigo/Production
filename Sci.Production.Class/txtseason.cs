using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win.UI;
using Sci.Data;
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

        private Control myBrand;    // 欄位.存入要取值的<控制項>

        /// <summary>
        /// Season.BrandID
        /// </summary>
        [Category("Custom Properties")]
        public Control BrandObjectName { get; set; }

        internal Txtstyle objStyle { get; set; } = null;

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

            string strStyle = string.Empty;
            string strBrand = string.Empty;
            if (this.objStyle != null)
            {
                strStyle = ((Txtstyle)this.objStyle).Text;
            }

            if (this.BrandObjectName != null && !string.IsNullOrWhiteSpace((string)this.BrandObjectName.Text))
            {
                strBrand = this.BrandObjectName.Text;
            }

            Win.Tools.SelectItem item;
            string selectCommand = $@"
Select distinct ID From Season WITH (NOLOCK)
Where ('{strBrand}' = '' or BrandID in (select Data From SplitString('{strBrand}', ','))) 
And ('{strStyle}' = '' or ID in ( select SeasonID from Style where Style.ID = '{strStyle}'))
order by id desc
";
            DualResult result = DBProxy.Current.Select("Production", selectCommand, out DataTable dt);
            item = new Win.Tools.SelectItem(dt, "ID", "11", this.Text)
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
