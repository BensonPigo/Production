using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P16_ExportMaterial : Sci.Win.Subs.Input6A
    {
        private DataRow mainrow;

        /// <inheritdoc/>
        public P16_ExportMaterial(DataRow maindr)
        {
            this.InitializeComponent();
            this.mainrow = maindr;
            this.EditMode = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.dispFromSeq.Value = this.mainrow["InventorySEQ"].ToString();
            this.dispToSeq.Value = this.mainrow["SEQ"].ToString();
            this.dispColor.Value = this.mainrow["Color"].ToString();
            this.dispExportQty.Value = MyUtility.GetValue.Lookup($@"
select ttlQty = sum(Qty) from TransferExport_Detail_Carton where TransferExport_DetailUkey = '{this.mainrow["Ukey"]}'");
            this.GridSetUp();
        }

        /// <inheritdoc/>
        protected override void OnPostFormLoaded()
        {
            string sqlCmd =
             $@"
select td.UnitID,* 
from TransferExport_Detail_Carton tc
inner join TransferExport_Detail td on td.Ukey = tc.TransferExport_DetailUkey
where TransferExport_DetailUkey = '{this.mainrow["Ukey"]}'";
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
            base.OnPostFormLoaded();
        }

        /// <inheritdoc/>
        public void GridSetUp()
        {
            this.Helper.Controls.Grid.Generator(this.grid3)
            .Text("Carton", header: "Carton", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("LotNo", header: "LotNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("StockQty", header: "Export Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
            .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("Qty", header: "Export Qty" + Environment.NewLine + "(Unit)", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
            .Text("UnitID", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("EditName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(18), iseditingreadonly: true)
                 ;
        }
    }
}
