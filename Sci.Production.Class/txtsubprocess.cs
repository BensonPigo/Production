using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtsubprocess : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "Select ID From Subprocess WITH (NOLOCK) where junk=0 and IsRFIDProcess= 1";

            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: this.Text, defaultValueColumn: "ID");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();

            this.ValidateText();
        }
    }
}
