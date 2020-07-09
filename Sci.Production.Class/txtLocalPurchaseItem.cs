using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtLocalPurchaseItem
    /// </summary>
    public partial class TxtLocalPurchaseItem : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtLocalPurchaseItem"/> class.
        /// </summary>
        public TxtLocalPurchaseItem()
        {
            this.Size = new System.Drawing.Size(140, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            string sqlWhere = "Where isLocalPurchase = 1";
            string sqlCmd = string.Empty;

            sqlCmd = "select ID, Abbreviation from ArtworkType WITH (NOLOCK)" + sqlWhere + " order by Seq";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "20,4", this.Text, false, ",");
            item.Size = new System.Drawing.Size(435, 510);
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
                string sqlWhere = string.Format("Where isLocalPurchase = 1 and id='{0}'", str);
                string sqlCmd = string.Empty;
                sqlCmd = "select ID, Abbreviation from ArtworkType WITH (NOLOCK)" + sqlWhere;

                if (MyUtility.Check.Seek(sqlCmd) == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Artworktype : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
