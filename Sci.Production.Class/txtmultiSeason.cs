using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtmultiSeason : Win.UI.TextBox
    {
        public txtmultiSeason()
        {
            this.Size = new System.Drawing.Size(80, 23);
            this.ReadOnly = true;
        }

        private Control brandObject;

        [Category("Custom Properties")]
        public Control BrandObjectName
        {
            get { return this.brandObject; }
            set { this.brandObject = value; }
        }

        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem2 item;
            string selectCommand = "select distinct ID from Production.dbo.Season WITH (NOLOCK) order by id desc";
            if (this.brandObject != null && !string.IsNullOrWhiteSpace((string)this.brandObject.Text))
            {
                selectCommand = string.Format("select distinct ID from Production.dbo.Season WITH (NOLOCK) where BrandID = '{0}' order by id desc", this.brandObject.Text);
            }

            item = new Win.Tools.SelectItem2(selectCommand, "Season ID", this.Text, null, null, null);
            item.Width = 300;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }
    }
}
