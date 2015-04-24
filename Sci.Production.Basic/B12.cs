using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
namespace Sci.Production.Basic
{
    public partial class B12 : Sci.Win.Tems.Input6
    {
        public B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailGridSetup()
        {
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("UnitTo", header: "Conversion unit", width: Widths.AnsiChars(8))
            .Text("Rate", header: "Conversion rate", width: Widths.AnsiChars(15))
            .Text("AddName", header: "Create by", width: Widths.AnsiChars(8))
            .DateTime("AddDate", header: "Create at", width: Widths.AnsiChars(15))
            .Text("EditName", header: "Edit by", width: Widths.AnsiChars(8))
            .DateTime("EditDate", header: "Edit at", width: Widths.AnsiChars(15));
        }

    }
}
