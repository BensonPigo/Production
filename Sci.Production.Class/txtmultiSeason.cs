using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtmultiSeason : Sci.Win.UI.TextBox
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
            set { this.brandObject = value; }
            get { return this.brandObject; }
        }

        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem2 item;
            string selectCommand = "select distinct ID from Production.dbo.Season WITH (NOLOCK) order by id desc";
            if (this.brandObject != null && !string.IsNullOrWhiteSpace((string)this.brandObject.Text))
            {
                selectCommand = string.Format("select distinct ID from Production.dbo.Season WITH (NOLOCK) where BrandID = '{0}' order by id desc", this.brandObject.Text);
            }
            item = new Sci.Win.Tools.SelectItem2(selectCommand, "Season ID", this.Text, null, null, null);
            item.Width = 300;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
