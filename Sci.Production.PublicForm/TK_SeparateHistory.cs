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
    /// TK_SeparateHistory
    /// </summary>
    public partial class TK_SeparateHistory : Sci.Win.Tems.QueryForm
    {
        private string transferExportID;
        private ListControlBindingSource bindingSourceTransferWKList = new ListControlBindingSource();
        private ListControlBindingSource bindingSourcePackingList = new ListControlBindingSource();
        private TK_SeparateHistoryCallFrom callFrom;
        private string canConfirmFtyStatus = string.Empty;

        /// <summary>
        /// TK_SeparateHistoryCallFrom
        /// </summary>
        public enum TK_SeparateHistoryCallFrom
        {
            /// <summary>
            /// WH_P06
            /// </summary>
            WH_P06,

            /// <summary>
            /// Shipping_P16
            /// </summary>
            Shipping_P16,
        }

        /// <summary>
        /// TK_SeparateHistory
        /// </summary>
        /// <param name="transferExportID">transferExportID</param>
        /// <param name="callFrom">callFrom</param>
        public TK_SeparateHistory(string transferExportID, TK_SeparateHistoryCallFrom callFrom)
        {
            this.InitializeComponent();
            this.transferExportID = transferExportID;
            this.gridTransferWKList.DataSource = this.bindingSourceTransferWKList;
            this.gridPackingList.DataSource = this.bindingSourcePackingList;
            this.callFrom = callFrom;
            this.EditMode = true;
            switch (this.callFrom)
            {
                case TK_SeparateHistoryCallFrom.WH_P06:
                    this.btnSeparateConfirm.Text = TK_FtyStatus.WHSeparateConfirm;
                    this.canConfirmFtyStatus = TK_FtyStatus.RequestSeparate;
                    break;
                case TK_SeparateHistoryCallFrom.Shipping_P16:
                    this.btnSeparateConfirm.Text = TK_FtyStatus.ShippingSeparateConfirm;
                    this.canConfirmFtyStatus = TK_FtyStatus.WHSeparateConfirm;
                    break;
                default:
                    break;
            }
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
where ID in (select distinct ID from TransferExport_SeparateHistory with (nolock) where NewID = '{this.transferExportID}')


select	te.ID,
		te.FtyRequestSeparateDate,
		te.TPESeparateApprovedDate,
		[GroupNewID] = (SELECT Stuff((select distinct concat( ',',NewID)   from #tmpOriTK FOR XML PATH('')),1,1,'')),
		te.WHSpearateConfirmDate,
		te.ShippingSeparateConfirmDate,
        te.FtyStatus
from TransferExport te with (nolock)
where ID = (select distinct ID from #tmpOriTK)


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

            DataTable dtMainTKInfo = dtResults[0];
            DataTable dtTrasferWK = dtResults[1];
            DataTable dtPackingList = dtResults[2];

            if (dtMainTKInfo.Rows.Count == 0)
            {
                return;
            }

            this.displayID.Text = dtMainTKInfo.Rows[0]["ID"].ToString();
            this.dateFtyRequestSeparateDate.Value = MyUtility.Convert.GetDate(dtMainTKInfo.Rows[0]["FtyRequestSeparateDate"]);
            this.dateTPESeparateApprovedDate.Value = MyUtility.Convert.GetDate(dtMainTKInfo.Rows[0]["TPESeparateApprovedDate"]);
            this.displayGroupNewID.Text = dtMainTKInfo.Rows[0]["GroupNewID"].ToString();
            this.dateWHSpearateConfirmDate.Value = MyUtility.Convert.GetDate(dtMainTKInfo.Rows[0]["WHSpearateConfirmDate"]);
            this.dateShippingSeparateConfirmDate.Value = MyUtility.Convert.GetDate(dtMainTKInfo.Rows[0]["ShippingSeparateConfirmDate"]);
            string[] arrayNewID = dtMainTKInfo.Rows[0]["GroupNewID"].ToString().Split(',');
            this.lblNumberNewTK.Text = arrayNewID.Length.ToString();
            this.comboNewID.Items.Clear();
            this.comboNewID.Items.Add(string.Empty);
            this.comboNewID.Items.AddRange(arrayNewID);

            this.bindingSourceTransferWKList.DataSource = dtTrasferWK;
            this.bindingSourcePackingList.DataSource = dtPackingList;

            this.btnSeparateConfirm.Visible = dtMainTKInfo.Rows[0]["FtyStatus"].ToString() == this.canConfirmFtyStatus;

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

        private void BtnSeparateConfirm_Click(object sender, EventArgs e)
        {
            DataRow drTransferExport;
            bool existsTransferExport = MyUtility.Check.Seek($"select FtyStatus from TransferExport with (nolock) where ID = '{this.displayID.Text}'", out drTransferExport);

            if (!existsTransferExport)
            {
                MyUtility.Msg.WarningBox("TK not exists");
                this.Query();
                return;
            }

            if (drTransferExport["FtyStatus"].ToString() != this.canConfirmFtyStatus)
            {
                switch (this.callFrom)
                {
                    case TK_SeparateHistoryCallFrom.WH_P06:
                        MyUtility.Msg.WarningBox("WH already confirmed.");
                        break;
                    case TK_SeparateHistoryCallFrom.Shipping_P16:
                        MyUtility.Msg.WarningBox("Shipping already confirmed.");
                        break;
                    default:
                        MyUtility.Msg.WarningBox("WH already confirmed.");
                        break;
                }

                this.Query();
                return;
            }

            switch (this.callFrom)
            {
                case TK_SeparateHistoryCallFrom.WH_P06:
                    this.WHSeparateConfirm();
                    break;
                case TK_SeparateHistoryCallFrom.Shipping_P16:
                    this.ShippingSeparateConfirm();
                    break;
                default:
                    break;
            }
        }

        private void WHSeparateConfirm()
        {
            string[] arrayNewTKID = this.displayGroupNewID.Text.Split(',');
            string updateSql = string.Empty;
            foreach (string id in arrayNewTKID)
            {
                updateSql += $@"
update TransferExport set FtyStatus = '{TK_FtyStatus.WHSeparateConfirm}', WHSpearateConfirmDate = getdate()
where ID = '{id}'

insert into TransferExport_StatusHistory(ID, OldStatus, NewStatus, OldFtyStatus, NewFtyStatus, UpdateDate)
values('{id}', '', '', '{TK_FtyStatus.RequestSeparate}', '{TK_FtyStatus.WHSeparateConfirm}', getdate())

";
            }

            DualResult result = DBProxy.Current.Execute(null, updateSql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Confirm success");
            this.Query();
        }

        private void ShippingSeparateConfirm()
        {
            string[] arrayNewTKID = this.displayGroupNewID.Text.Split(',');
            string updateSql = string.Empty;
            foreach (string id in arrayNewTKID)
            {
                updateSql += $@"
update TransferExport set FtyStatus = '{TK_FtyStatus.ShippingSeparateConfirm}', ShippingSeparateConfirmDate = getdate(), Status = '{TK_TpeStatus.FtyConfirm}'
where ID = '{id}'

insert into TransferExport_StatusHistory(ID, OldStatus, NewStatus, OldFtyStatus, NewFtyStatus, UpdateDate)
values('{id}', '{TK_TpeStatus.SeparateApproved}', '{TK_TpeStatus.FtyConfirm}', '{TK_FtyStatus.WHSeparateConfirm}', '{TK_FtyStatus.ShippingSeparateConfirm}', getdate())

";
            }

            TransactionOptions transactionOptions = new TransactionOptions()
            {
                Timeout = new TimeSpan(0, 5, 0),
                IsolationLevel = System.Transactions.IsolationLevel.Serializable,
            };
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                DualResult result = DBProxy.Current.Execute(null, updateSql);
                if (!result)
                {
                    transactionScope.Dispose();
                    this.ShowErr(result);
                    return;
                }

                result = APITransfer.SendShippingSeparateConfirm(this.transferExportID);
                if (!result)
                {
                    transactionScope.Dispose();
                    this.ShowErr(result);
                    return;
                }

                transactionScope.Complete();
            }
            MyUtility.Msg.InfoBox("Confirm success");
            this.Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
