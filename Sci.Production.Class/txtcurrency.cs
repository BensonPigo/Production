using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtcurrency
    /// </summary>
    public partial class Txtcurrency : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtcurrency"/> class.
        /// </summary>
        public Txtcurrency()
        {
            this.Size = new System.Drawing.Size(48, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql = "select ID,NameCH,NameEN from Currency WITH (NOLOCK) where Junk = 0 order by ID";
            DataTable tbCurrency;
            DBProxy.Current.Select("Production", sql, out tbCurrency);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbCurrency, "ID,NameCH,NameEn", "5,14,30", this.Text, "ID,NameCH,NameEn");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(str, "currency", "id", "Production") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Currency : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
