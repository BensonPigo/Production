using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// P19_SeparateHistory
    /// </summary>
    public partial class P19_SeparateHistory : Sci.Win.Tems.QueryForm
    {
        private string whereTransferExport_DetailUkey = string.Empty;
        private ListControlBindingSource bindingSourceTransferWKList = new ListControlBindingSource();
        private ListControlBindingSource bindingSourcePackingList = new ListControlBindingSource();

        /// <summary>
        /// P19_SeparateHistory
        /// </summary>
        /// <param name="transferExport_DetailUkeys">transferExport_DetailUkeys</param>
        /// <param name="callFrom">callFrom</param>
        public P19_SeparateHistory(long[] transferExport_DetailUkeys)
        {
            this.InitializeComponent();
            this.whereTransferExport_DetailUkey = transferExport_DetailUkeys.DefaultIfEmpty().Distinct().Select(s => $"'{s}'").JoinToString(",");
            this.gridTransferWKList.DataSource = this.bindingSourceTransferWKList;
            this.gridPackingList.DataSource = this.bindingSourcePackingList;
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridTransferWKList)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("OriPoQty", header: "Ori Po Q'ty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("StockExportQty", header: "Ori Export Q'ty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("ID", header: "Ori TK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("NewID", header: "New TK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("NewPoQty", header: "New Po Q'ty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("StockQty", header: "New Export Q'ty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StockUnitID", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("MaterialType", header: "Material Type", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("TransferExportReason", header: "Reason", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ReasonDesc", header: "Reason Desc", width: Widths.AnsiChars(3), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.gridPackingList)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ID", header: "New TK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("GroupID", header: "Fty Group ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Carton", header: "Carton No", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("StockQty", header: "Export Q'ty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NetKg", header: "N.W. (kg)", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("WeightKg", header: "G.W. (Kg)", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", width: Widths.AnsiChars(6), iseditingreadonly: true)
            ;

            foreach (DataGridViewColumn column in this.gridTransferWKList.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewColumn column in this.gridPackingList.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.Query();
        }

        private void Query()
        {
            if (MyUtility.Check.Empty(this.whereTransferExport_DetailUkey))
            {
                return;
            }

            string sqlGetData = $@"
select	ID,
		NewID,
		NewDetailUkey,
		PoQty,
		ExportQty,
		UnitID,
		StockExportQty,
		StockUnitID
into #tmpOriTK
from TransferExport_SeparateHistory ts with (nolock)
where NewDetailUkey in ({this.whereTransferExport_DetailUkey})


select	ted.InventoryPOID,
		[FromSEQ] = Concat(ted.InventorySeq1, ' ', ted.InventorySeq2),
		ted.POID,
		[ToSEQ] = Concat (ted.Seq1, ' ', ted.Seq2),
		[OriPoQty] = round(dbo.GetUnitQty(t.UnitID, t.StockUnitID, t.PoQty), 2),
		t.StockExportQty,
		t.ID,
		t.NewID,
		[NewPoQty] = round(dbo.GetUnitQty(t.UnitID, t.StockUnitID, ted.PoQty), 2),
		[StockQty] = CartonQty.val,
		t.StockUnitID,
		ted.SuppID,
		ted.Description,
		[MaterialType] =  Concat(case ted.FabricType
								     when 'F' then 'Fabric'
								     when 'A' then 'Accessory'
								     when 'O' then 'Other'
								 end
								 , '-',  f.MtlTypeID),
		[Color] = psds.SpecValue,
		ted.TransferExportReason,
		[ReasonDesc] = wr.Description
from TransferExport_Detail ted with (nolock)
inner join #tmpOriTK t on t.NewDetailUkey = ted.Ukey
left join PO_Supp_Detail psd with (nolock) on	psd.ID = ted.InventoryPOID and
												psd.SEQ1 = ted.InventorySeq1 and
												psd.SEQ2 = ted.InventorySeq2
left join PO_Supp_Detail_Spec psds with (nolock) on psd.ID = psds.ID and
													psd.SEQ1 = psds.SEQ1 and
													psd.SEQ2 = psds.SEQ2 and
													psds.SpecColumnID = 'Color'
left join Fabric f with (nolock) on f.SCIRefno = psd.SCIRefno
left join WhseReason wr with (nolock) on wr.ID = ted.TransferExportReason and wr.Type = 'TE'
outer apply(select [val] = sum(StockQty) from TransferExport_Detail_Carton tdc with (nolock) where tdc.TransferExport_DetailUkey = ted.Ukey) CartonQty
order by ted.InventoryPOID, ted.InventorySeq1, ted.InventorySeq2, ted.POID, ted.Seq1, ted.Seq2, t.NewID

select	ted.InventoryPOID,
		[FromSEQ] = Concat(ted.InventorySeq1, ' ', ted.InventorySeq2),
		ted.POID,
		[ToSEQ] = Concat (ted.Seq1, ' ', ted.Seq2),
		tedc.ID,
		tedc.GroupID,
		tedc.Carton,
		tedc.Roll,
		[Dyelot] = tedc.LotNo,
		tedc.StockQty,
		tedc.NetKg,
		tedc.WeightKg,
		tedc.CBM
from #tmpOriTK t
inner join TransferExport_Detail ted with (nolock) on t.NewDetailUkey = ted.Ukey
inner join TransferExport_Detail_Carton tedc with (nolock) on tedc.TransferExport_DetailUkey = ted.Ukey
order by ted.InventoryPOID, ted.InventorySeq1, ted.InventorySeq2, ted.POID, ted.Seq1, ted.Seq2, t.NewID

drop table #tmpOriTK
";

            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResults);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            DataTable dtTrasferWK = dtResults[0];
            DataTable dtPackingList = dtResults[1];

            if (dtTrasferWK.Rows.Count == 0)
            {
                return;
            }

            string[] arrayNewID = dtTrasferWK.AsEnumerable().Select(s => s["NewID"].ToString()).Distinct().ToArray();
            this.comboNewID.Items.Clear();
            this.comboNewID.Items.Add(string.Empty);
            this.comboNewID.Items.AddRange(arrayNewID);

            this.bindingSourceTransferWKList.DataSource = dtTrasferWK;
            this.bindingSourcePackingList.DataSource = dtPackingList;

        }

        private void ComboNewID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboNewID.SelectedIndex < 0)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.comboNewID.Text))
            {
                this.bindingSourceTransferWKList.RemoveFilter();
                this.bindingSourcePackingList.RemoveFilter();
            }
            else
            {
                this.bindingSourceTransferWKList.Filter = $"NewID = '{this.comboNewID.Text}'";
                this.bindingSourcePackingList.Filter = $"ID = '{this.comboNewID.Text}'";
            }
        }

        private void GridTransferWKList_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridTransferWKList.SelectedRows.Count == 0)
            {
                return;
            }

            DataRow drSelected = this.gridTransferWKList.GetDataRow(this.gridTransferWKList.SelectedRows[0].Index);

            foreach (DataGridViewRow drGrid in this.gridPackingList.Rows)
            {
                if (drGrid.Cells["InventoryPOID"].Value.ToString() == drSelected["InventoryPOID"].ToString() &&
                    drGrid.Cells["FromSEQ"].Value.ToString() == drSelected["FromSEQ"].ToString() &&
                    drGrid.Cells["POID"].Value.ToString() == drSelected["POID"].ToString() &&
                    drGrid.Cells["ToSEQ"].Value.ToString() == drSelected["ToSEQ"].ToString() &&
                    drGrid.Cells["ID"].Value.ToString() == drSelected["NewID"].ToString())
                {
                    this.gridPackingList.SelectRowTo(drGrid.Index);
                    return;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
