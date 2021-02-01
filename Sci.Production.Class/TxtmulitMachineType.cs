using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using System.Configuration;
using System.Xml.Linq;
using Ict;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtmulitMachineType
    /// </summary>
    public partial class TxtmulitMachineType : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtmulitMachineType"/> class.
        /// </summary>
        public TxtmulitMachineType()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        /// <summary>
        /// IsJunk
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否Junk")]
        public bool IsJunk { get; set; } = false;

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            DualResult result = Ict.Result.True;
            DataTable dt;

            string whereM = string.Empty;
            if (this.IsJunk)
            {
                whereM += " where Junk = 1 ";
            }
            else
            {
                whereM += " where Junk = 0 ";
            }

            string sqlcmd = $"select m.ID, m.DescCH, m.Description from MachineType m WITH (NOLOCK) {whereM} ";
            result = DBProxy.Current.Select(null, sqlcmd, out dt);

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(dt, "ID,DescCH,Description", "ID,DescCH,Description", "10,15,20", this.Text);
            DialogResult dialogResult = item.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }
    }
}
