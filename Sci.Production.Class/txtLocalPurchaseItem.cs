using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtLocalPurchaseItem : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            string sqlWhere = "Where isLocalPurchase = 1";
            string sqlCmd = string.Empty;

            sqlCmd = "select ID, Abbreviation from ArtworkType WITH (NOLOCK)" + sqlWhere + " order by Seq";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20,4", this.Text, false, ",");
            item.Size = new System.Drawing.Size(435, 510);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

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

        public txtLocalPurchaseItem()
        {
            this.Size = new System.Drawing.Size(140, 23);
        }
    }
}
