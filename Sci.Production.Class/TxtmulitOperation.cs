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
    /// TxtmulitOperation
    /// </summary>
    public partial class TxtmulitOperation : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtmulitOperation"/> class.
        /// </summary>
        public TxtmulitOperation()
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

            string sqlcmd = $"select o.ID, o.DescEN, o.SMV, o.MachineTypeID, o.MasterPlusGroup, o.SeamLength from Operation o WITH (NOLOCK) {whereM} ";
            result = DBProxy.Current.Select(null, sqlcmd, out dt);

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(dt, "ID,DescEN,SMV,MachineTypeID,MasterPlusGroup,SeamLength", "ID,Description,S.M.V,ST/MC Type,Machine Group,Seam Leagth", "15,15,5,5,5,5", this.Text);
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
