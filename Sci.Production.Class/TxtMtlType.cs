using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <inheritdoc />
    public partial class TxtMtlType : Win.UI.TextBox
    {
        /// <inheritdoc />
        public TxtMtlType()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT ID FROM Production.dbo.MtlType WHERE Junk=0  ORDER BY Id";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, "20,35,45", this.Text, false, ",")
            {
                Size = new System.Drawing.Size(350, 600),
            };
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
