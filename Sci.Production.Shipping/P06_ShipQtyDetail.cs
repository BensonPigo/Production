using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P06_ShipQtyDetail : Sci.Win.Subs.Input8A
    {
        public P06_ShipQtyDetail()
        {
            InitializeComponent();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            displaySPNo.Value = MyUtility.Convert.GetString(CurrentDetailData["OrderID"]);
            displaySEQ.Value = MyUtility.Convert.GetString(CurrentDetailData["OrderShipmodeSeq"]);
            displayPackingNo.Value = MyUtility.Convert.GetString(CurrentDetailData["PackingListID"]);
            displayShipMode.Value = MyUtility.Convert.GetString(CurrentDetailData["ShipmodeID"]);
            displayStatus.Value = MyUtility.Convert.GetString(CurrentDetailData["StatusExp"]);
            numOrderQty.Value = MyUtility.Convert.GetInt(CurrentDetailData["OrderQty"]);
            numOrderQtybySeq.Value = MyUtility.Convert.GetInt(CurrentDetailData["ShipModeSeqQty"]);
            numShipQty.Value = MyUtility.Convert.GetInt(CurrentDetailData["ShipQty"]);

        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(grid)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Q'ty by Seq", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(10), iseditingreadonly: true);

            for (int i = 0; i < grid.ColumnCount; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            return true;
        }
    }
}
