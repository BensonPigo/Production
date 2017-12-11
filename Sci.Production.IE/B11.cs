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
    /// IE_B11
    /// </summary>
    public partial class B11 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B11
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }
    }
}
