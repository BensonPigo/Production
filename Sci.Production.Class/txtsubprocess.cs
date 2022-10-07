using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtsubprocess
    /// </summary>
    public partial class Txtsubprocess : Win.UI.TextBox
    {
        /// <summary>
        /// MultiSelect
        /// </summary>
        [Description("是否要多選")]
        public bool MultiSelect { get; set; } = true;

        /// <summary>
        /// IsRFIDProcess
        /// </summary>
        public bool IsRFIDProcess { get; set; } = true;

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "Select ID From Subprocess WITH (NOLOCK) where junk=0 ";

            if (this.IsRFIDProcess)
            {
                sqlWhere += " and IsRFIDProcess= 1";
            }

            if (this.MultiSelect)
            {
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: this.Text, defaultValueColumn: "ID");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
            }
            else
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: this.Text, defaultValueColumn: "ID");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
            }

            this.ValidateText();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            string sqlCheck = $"select 1 from Subprocess WITH (NOLOCK) where junk=0 and ID = '{this.Text}'";
            if (this.IsRFIDProcess)
            {
                sqlCheck += " and IsRFIDProcess= 1";
            }

            if (!this.MultiSelect && !MyUtility.Check.Empty(this.Text))
            {
                if (!MyUtility.Check.Seek(sqlCheck))
                {
                    MyUtility.Msg.WarningBox($"<Subprocess> {this.Text} not found");
                    e.Cancel = true;
                    return;
                }
            }

            base.OnValidating(e);
        }
    }
}
