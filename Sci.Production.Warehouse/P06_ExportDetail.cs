using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P06_ExportDetail
    /// </summary>
    public partial class P06_ExportDetail : Win.Forms.Base
    {
        private string transferExport_DetailUkey;

        /// <summary>
        /// P06_ExportDetail
        /// </summary>
        /// <param name="drMaster">drMaster</param>
        public P06_ExportDetail(DataRow drMaster)
        {
            this.InitializeComponent();
            this.transferExport_DetailUkey = drMaster["Ukey"].ToString();
            this.displayFromSP.Text = drMaster["InventoryPOID"].ToString();
            this.displayFromSeq.Text = drMaster["FromSeq"].ToString();
            this.displayRefno.Text = drMaster["Refno"].ToString();
            this.displayColorID.Text = drMaster["ColorID"].ToString();
            this.displayToSP.Text = drMaster["PoID"].ToString();
            this.displayToSeq.Text = drMaster["ToSEQ"].ToString();
            this.displayExportQty.Text = drMaster["ExportQty"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string sqlcmd = $@"
select  tdc.Carton,
        tdc.LotNo,
        tdc.StockQty,
        tdc.StockUnitID,
        tdc.Qty,
        ted.UnitID,
        tdc.EditName,
        tdc.EditDate
from TransferExport_Detail_Carton tdc WITH (NOLOCK)
inner join TransferExport_Detail ted with (nolock) on tdc.TransferExport_DetailUkey = ted.Ukey
where TransferExport_DetailUkey = '{this.transferExport_DetailUkey}'";
            DataTable dt;
            DualResult result;
            result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            this.Helper.Controls.Grid.Generator(this.gridExportMaterial)
            .Text("Carton", header: "Roll", width: Widths.AnsiChars(10))
            .Text("LotNo", header: "Dyelot", width: Widths.AnsiChars(10))
            .Numeric("StockQty", header: "Export Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(10))
            .Text("StockUnitID", header: "Stock Unit", width: Widths.AnsiChars(6))
            .Numeric("Qty", header: "Export Qty" + Environment.NewLine + "(Unit)", width: Widths.AnsiChars(10))
            .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6))
            .Text("EditName", header: "EditName", width: Widths.AnsiChars(15))
            .DateTime("EditDate", header: "EditDate", width: Widths.AnsiChars(20))
            ;
            this.gridExportMaterial.AutoResizeColumns();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
