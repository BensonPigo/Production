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

        /// <summary>
        /// check Produce Fty
        /// </summary>
        public bool CheckFtyGroup { get; set; } = false;

        /// IsForPackingA2B
        /// </summary>
        public bool IsForPackingA2B { get; set; } = false;

        /// <summary>
        /// IsForPackingA2B
        /// </summary>
        public bool IsDataFromA2B { get; set; } = false;

        /// <summary>
        /// SystemName
        /// </summary>
        public string SystemName { get; set; } = string.Empty;

        /// <inheritdoc/>
        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            if (this.IsForPackingA2B)
            {
                PopupFactoryForAtoB popupFactoryForAtoB = new PopupFactoryForAtoB(this.Text);
                DialogResult result = popupFactoryForAtoB.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = popupFactoryForAtoB.ResultFactory;
                this.IsDataFromA2B = popupFactoryForAtoB.IsDataFromA2B;
                this.SystemName = popupFactoryForAtoB.SystemName;
            }
            else
            {
                string sqlWhere = "select ID from Factory WITH (NOLOCK) where Junk = 0 order by ID";
                if (this.CheckProduceFty)
                {
                    sqlWhere = "select ID from Factory WITH (NOLOCK) where Junk = 0 and IsProduceFty = 1 order by ID";
                }

                if (this.CheckFtyGroup)
                {
                    sqlWhere = "select distinct FtyGroup from Factory WITH (NOLOCK) order by FtyGroup";
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
}
