using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtmultifactory : Win.UI.TextBox
    {
        public txtmultifactory()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        public bool checkProduceFty = false;

        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sqlWhere = "select ID from Factory WITH (NOLOCK) where Junk = 0 order by ID";
            if (this.checkProduceFty)
            {
                sqlWhere = "select ID from Factory WITH (NOLOCK) where Junk = 0 and IsProduceFty = 1 order by ID";
            }

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, "Factory", "10", this.Text, null, null, null);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }
    }
}
