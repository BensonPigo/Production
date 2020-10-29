using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtmultiSeason
    /// </summary>
    public partial class TxtmultiSeason : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtmultiSeason"/> class.
        /// </summary>
        public TxtmultiSeason()
        {
            this.Size = new System.Drawing.Size(80, 23);
            this.ReadOnly = true;
        }

        /// <summary>
        /// Season.BrandID
        /// </summary>
        [Category("Custom Properties")]
        public Control BrandObjectName { get; set; }

        /// <inheritdoc/>
        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem2 item;
            string selectCommand = "select distinct ID from Production.dbo.Season WITH (NOLOCK) order by id desc";
            if (this.BrandObjectName != null && !string.IsNullOrWhiteSpace((string)this.BrandObjectName.Text))
            {
                selectCommand = string.Format("select distinct ID from Production.dbo.Season WITH (NOLOCK) where BrandID = '{0}' order by id desc", this.BrandObjectName.Text);
            }

            item = new Win.Tools.SelectItem2(selectCommand, "Season ID", this.Text, null, null, null)
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
