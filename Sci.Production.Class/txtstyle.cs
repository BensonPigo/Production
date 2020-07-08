using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;
using Ict;

namespace Sci.Production.Class
{
    public partial class txtstyle : Win.UI.TextBox
    {
        public txtstyle()
        {
            this.Size = new System.Drawing.Size(130, 23);
        }

        public txtbrand tarBrand { get; set; }

        public txtseason tarSeason { get; set; }

        private Control brandObject;

        [Category("Custom Properties")]
        public Control BrandObjectName
        {
            get { return this.brandObject; }
            set { this.brandObject = value; }
        }

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
                    if (this.brandObject != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.brandObject.Text))
                        {
                            string selectCommand = string.Format("select ID from Style WITH (NOLOCK) where BrandID = '{0}' and ID = '{1}'", (string)this.brandObject.Text, this.Text.ToString());
                            if (!MyUtility.Check.Seek(selectCommand, "Production"))
                            {
                                this.Text = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< Brand + Style: {0} + {1} > not found!!!", (string)this.brandObject.Text, textValue));
                                return;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item;
            string selectCommand;
            selectCommand = "select ID,SeasonID,Description,BrandID from Production.dbo.Style WITH (NOLOCK) where Junk = 0 order by ID";
            if (this.brandObject != null && !string.IsNullOrWhiteSpace((string)this.brandObject.Text))
            {
                selectCommand = string.Format("select ID,SeasonID,Description,BrandID from Production.dbo.Style WITH (NOLOCK) where Junk = 0 and BrandID = '{0}' order by ID", this.brandObject.Text);
            }

            item = new Win.Tools.SelectItem(selectCommand, "12,5,38,10", this.Text);
            item.Size = new System.Drawing.Size(757, 530);
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            if (this.tarBrand != null && this.tarSeason != null)
            {
                this.ValidateControl();
                this.tarBrand.Text = item.GetSelecteds()[0]["BrandID"].ToString();
                this.tarBrand.ValidateControl();
                this.tarSeason.Text = item.GetSelecteds()[0]["SeasonID"].ToString();
                this.tarSeason.ValidateControl();
            }
        }
    }
}
