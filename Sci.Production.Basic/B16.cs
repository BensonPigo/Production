using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Basic
{
    public partial class B16 : Sci.Win.Tems.Input1
    {
        public B16(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();
        }
        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "Code", width: Widths.AnsiChars(10))
                .Text("Description", header: "Description", width: Widths.AnsiChars(50));

            return true;
        }
    }
}
