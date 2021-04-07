using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// B08
    /// </summary>
    public partial class B08 : Win.Tems.Input1
    {
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtRefno.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Refno"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Unit"]))
            {
                MyUtility.Msg.WarningBox("<Refno> & <Unit> cannot be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
