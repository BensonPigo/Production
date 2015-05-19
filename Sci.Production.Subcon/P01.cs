using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Data;
using Sci;
using Sci.Win;

namespace Sci.Production.Subcon
{
    public partial class P01 : Sci.Win.Tems.Input6
    {
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        //protected override void OnDetailGridSetup()
        //{
        //    Helper.Controls.Grid.Generator(this.detailgrid)
        //    .Text("orderid", header: "SP#", width: Widths.AnsiChars(13))
        //    .Text("Style", header: "Style", width: Widths.AnsiChars(13))
        //    .Text("PoQty", header: "PO Qty", width: Widths.AnsiChars(10))
        //    .Text("Invoice", header: "Invoice#", width: Widths.AnsiChars(10))
        //    .Date("ETA", header: "ETA", width: Widths.AnsiChars(10))
        //    .Text("Color", header: "Color Code", width: Widths.AnsiChars(5))
        //    .Text("PKQty", header: "Packing List Qty", width: Widths.AnsiChars(5))
        //    .Text("ActualQty", header: "Actual Rced Qty", width: Widths.AnsiChars(5))
        //    .Text("TtRequestY", header: "Inspection Total \n Replacement Requeset \n Qty", width: Widths.AnsiChars(5));
        //}

    }
}
