using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtstyle
    /// </summary>
    public partial class Txtstyle : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtstyle"/> class.
        /// </summary>
        public Txtstyle()
        {
            this.Size = new System.Drawing.Size(130, 23);
        }

        /// <summary>
        /// Style.BrandID  get
        /// </summary>
        public Txtbrand TarBrand { get; set; }

        /// <summary>
        /// Style.SeasonID
        /// </summary>
        public Txtseason TarSeason { get; set; }

        /// <summary>
        /// Style.BrandID  SQLwhere
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
                if (!MyUtility.Check.Seek(textValue, "Style", "ID", "Production"))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Style : {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    if (this.BrandObjectName != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.BrandObjectName.Text))
                        {
                            string selectCommand = string.Format("select ID from Style WITH (NOLOCK) where BrandID = '{0}' and ID = '{1}'", (string)this.BrandObjectName.Text, this.Text.ToString());
                            if (!MyUtility.Check.Seek(selectCommand, "Production"))
                            {
                                this.Text = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< Brand + Style: {0} + {1} > not found!!!", (string)this.BrandObjectName.Text, textValue));
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
            string selectCommand;
            selectCommand = "select ID,SeasonID,Description,BrandID from Production.dbo.Style WITH (NOLOCK) where Junk = 0 order by ID";
            if (this.BrandObjectName != null && !string.IsNullOrWhiteSpace((string)this.BrandObjectName.Text))
            {
                selectCommand = string.Format("select ID,SeasonID,Description,BrandID from Production.dbo.Style WITH (NOLOCK) where Junk = 0 and BrandID = '{0}' order by ID", this.BrandObjectName.Text);
            }

            item = new Win.Tools.SelectItem(selectCommand, "12,5,38,10", this.Text);
            item.Size = new System.Drawing.Size(757, 530);
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            if (this.TarBrand != null && this.TarSeason != null)
            {
                this.ValidateControl();
                this.TarBrand.Text = item.GetSelecteds()[0]["BrandID"].ToString();
                this.TarBrand.ValidateControl();
                this.TarSeason.Text = item.GetSelecteds()[0]["SeasonID"].ToString();
                this.TarSeason.ValidateControl();
            }
        }
    }
}
