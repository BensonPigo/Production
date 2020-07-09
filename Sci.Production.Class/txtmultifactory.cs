using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtmultifactory
    /// </summary>
    public partial class Txtmultifactory : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtmultifactory"/> class.
        /// </summary>
        public Txtmultifactory()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        /// <summary>
        /// check Produce Fty
        /// </summary>
        public bool CheckProduceFty { get; set; } = false;

        /// <inheritdoc/>
        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sqlWhere = "select ID from Factory WITH (NOLOCK) where Junk = 0 order by ID";
            if (this.CheckProduceFty)
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
