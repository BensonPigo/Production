using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B10
    /// </summary>
    public partial class B10 : Win.Tems.Input1
    {
        /// <summary>
        /// B10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["TCPCode"]))
            {
                MyUtility.Msg.WarningBox(" <TCP Code> cannot be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
