using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B08
    /// </summary>
    public partial class B08 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Numeric("Day", header: "Day")
                .Numeric("Efficiency", header: "Efficiency (%)");
        }
    }
}
