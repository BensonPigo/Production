using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B06
    /// </summary>
    public partial class B06 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B06
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.displayMasterGroupID.Text = this.grid.SelectedRows[0].Cells["MachineGroup"].Value.ToString().Substring(0, 2);
        }
    }
}
