using Ict;
using Ict.Win;
using Ict.Win.Defs;
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
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P06_Packing
    /// </summary>
    public partial class P16_Separate_TK_Group : Sci.Win.Tems.QueryForm
    {
        private string transferExportID;

        /// <summary>
        /// P06_Packing
        /// </summary>
        /// <param name="transferExportID">transferExportID</param>
        public P16_Separate_TK_Group(string transferExportID)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.transferExportID = transferExportID;

            this.gridCartonList.DataSource = this.bindingSourceCartonList;
            this.gridPackingList.DataSource = this.bindingSourcePackingList;
            this.gridGroupSummaryInfo.DataSource = this.bindingSourceGroupSummary;

            string ftyStatus = MyUtility.GetValue.Lookup($"select FtyStatus from TransferExport with (nolock) where ID = '{transferExportID}'");
            if (ftyStatus == TK_FtyStatus.Send)
            {
                this.btnRequestSeparate.Visible = true;
                this.btnEditSave.Visible = true;
            }
            else
            {
                this.btnRequestSeparate.Visible = false;
                this.btnEditSave.Visible = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.btnEditSave == null)
            {
                return;
            }

            this.btnEditSave.Text = this.EditMode ? "Save" : "Edit";
            this.btnRequestSeparate.Enabled = !this.EditMode;
            if (!this.EditMode)
            {
                this.Query();
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings colGroupID = new DataGridViewGeneratorTextColumnSettings();
            colGroupID.CellValidating += (s, e) =>
            {
                DataRow curRow = this.gridCartonList.GetDataRow(e.RowIndex);
                curRow["GroupID"] = e.FormattedValue.ToString();
                DataTable dtCartonList = (DataTable)this.bindingSourceCartonList.DataSource;

                DataTable dtGroupSummary = (DataTable)this.bindingSourceGroupSummary.DataSource;
                dtGroupSummary.Clear();
                foreach (var groupItem in dtCartonList.AsEnumerable().GroupBy(group => new { GroupID = group["GroupID"].ToString() }))
                {
                    DataRow drNew = dtGroupSummary.NewRow();
                    drNew["GroupID"] = groupItem.Key.GroupID;
                    drNew["CartonCnt"] = groupItem.Count();
                    drNew["NetKg"] = groupItem.Sum(item => MyUtility.Convert.GetDecimal(item["NetKg"]));
                    drNew["WeightKg"] = groupItem.Sum(item => MyUtility.Convert.GetDecimal(item["WeightKg"]));
                    drNew["CBM"] = groupItem.Sum(item => MyUtility.Convert.GetDecimal(item["CBM"]));

                    dtGroupSummary.Rows.Add(drNew);
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridCartonList)
                .Text("Carton", header: "Assign Carton No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NetKg", header: "N.W. (kg)", width: Widths.AnsiChars(12), decimal_places: 2, iseditingreadonly: true)
                .Numeric("WeightKg", header: "G.W. (kg)", width: Widths.AnsiChars(12), decimal_places: 2, iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", width: Widths.AnsiChars(12), decimal_places: 5, iseditingreadonly: true)
                .Text("GroupID", header: "Fty Group ID", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: colGroupID);

            this.Helper.Controls.Grid.Generator(this.gridPackingList)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("StockQty", header: "Export Qty", width: Widths.AnsiChars(13), decimal_places: 2, iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridGroupSummaryInfo)
                .Text("GroupID", header: "Fty Group ID", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CartonCnt", header: "# of Carton", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NetKg", header: "Ttl N.W. (kg)", width: Widths.AnsiChars(12), decimal_places: 2, iseditingreadonly: true)
                .Numeric("WeightKg", header: "Ttl G.W. (kg)", width: Widths.AnsiChars(12), decimal_places: 2, iseditingreadonly: true)
                .Numeric("CBM", header: "Ttl CBM", width: Widths.AnsiChars(12), decimal_places: 5, iseditingreadonly: true)
                ;

            this.gridCartonList.Columns["GroupID"].DefaultCellStyle.BackColor = Color.Pink;

            this.Query();
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                DualResult result = this.SaveTransferExport_Detail_Carton();
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            this.EditMode = !this.EditMode;
        }

        private DualResult SaveTransferExport_Detail_Carton()
        {
            DualResult resultFtyStatusCheck = this.FtyStatusCheck();

            if (!resultFtyStatusCheck)
            {
                this.EditMode = false;
                return resultFtyStatusCheck;
            }

            DataTable dtCartonList = (DataTable)this.bindingSourceCartonList.DataSource;
            string sqlUpdate = string.Empty;

            foreach (DataRow dr in dtCartonList.Rows)
            {
                sqlUpdate += $"update TransferExport_Detail_Carton set GroupID = '{dr["GroupID"]}' where ID = '{this.transferExportID}' and Carton = '{dr["Carton"]}'";
            }

            return DBProxy.Current.Execute(null, sqlUpdate);
        }

        private void Query()
        {
            string sqlQuery = $@"
select	ted.InventoryPOID,
		[FromSeq] = Concat(ted.InventorySeq1, ' ', ted.InventorySeq2),
		ted.PoID,
		[ToSEQ] = Concat(ted.Seq1, ' ', ted.Seq2),
		tedc.Roll,
		[Dyelot] = tedc.LotNo,
		tedc.StockQty,
		tedc.Carton,
        tedc.GroupID,
        tedc.NetKg,
        tedc.WeightKg,
        tedc.CBM
into #tmpPackinglist
from TransferExport_Detail ted with (nolock)
inner join TransferExport_Detail_Carton tedc with (nolock) on tedc.TransferExport_DetailUkey = ted.Ukey
where ted.ID = '{this.transferExportID}' and tedc.Carton <> ''

select  Carton,
        [NetKg] = sum(NetKg),
        [WeightKg] = sum(WeightKg),
        [CBM] = sum(CBM),
        GroupID
into #tmpCartonList
from #tmpPackinglist
group by Carton, GroupID

select * from #tmpPackinglist

select * from #tmpCartonList

select  GroupID,
        CartonCnt = count(1),
        [NetKg] = sum(NetKg),
        [WeightKg] = sum(WeightKg),
        [CBM] = sum(CBM)
from #tmpCartonList
group by GroupID

drop table #tmpPackinglist, #tmpCartonList
";

            DataTable[] dtResult;

            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtResult[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data Found");
                return;
            }

            DataTable dtPackinglist = dtResult[0];
            DataTable dtCartonList = dtResult[1];
            DataTable dtGroupSummary = dtResult[2];

            this.bindingSourcePackingList.DataSource = dtPackinglist;
            this.bindingSourceCartonList.DataSource = dtCartonList;
            this.bindingSourceGroupSummary.DataSource = dtGroupSummary;

            this.gridCartonList.SelectRowTo(0);

        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridCartonList_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridCartonList.SelectedRows.Count == 0)
            {
                return;
            }

            DataRow drSelected = this.gridCartonList.GetDataRow(this.gridCartonList.SelectedRows[0].Index);
            this.bindingSourcePackingList.Filter = $"Carton = '{drSelected["Carton"]}'";
        }

        private void BtnRequestSeparate_Click(object sender, EventArgs e)
        {
            DataTable dtCartonList = (DataTable)this.bindingSourceCartonList.DataSource;
            bool isGroupIDEmpty = dtCartonList.AsEnumerable().Any(s => MyUtility.Check.Empty(s["GroupID"]));
            if (isGroupIDEmpty)
            {
                MyUtility.Msg.WarningBox("Fty Group ID cannot be empty.");
                return;
            }

            DualResult resultFtyStatusCheck = this.FtyStatusCheck();

            if (!resultFtyStatusCheck)
            {
                this.ShowErr(resultFtyStatusCheck);
                this.Query();
                return;
            }

            string updateSql = $@"
update TransferExport set   Status = '{TK_TpeStatus.RequestSeparate}',
                            FtyStatus = '{TK_FtyStatus.RequestSeparate}',
                            FtyRequestSeparateDate = getdate()
where   ID = '{this.transferExportID}'

insert into TransferExport_StatusHistory(ID, OldStatus, NewStatus, OldFtyStatus, NewFtyStatus, UpdateDate)
values('{this.transferExportID}', '{TK_TpeStatus.Sent}', '{TK_TpeStatus.RequestSeparate}', '{TK_FtyStatus.Send}', '{TK_FtyStatus.RequestSeparate}', getdate())
";

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

                result = APITransfer.SendRequestSeparate(this.transferExportID);
                if (!result)
                {
                    transactionScope.Dispose();
                    this.ShowErr(result);
                    return;
                }

                transactionScope.Complete();
            }
        }

        private DualResult FtyStatusCheck()
        {
            string ftyStatus = MyUtility.GetValue.Lookup($"select FtyStatus from TransferExport with (nolock) where ID = '{this.transferExportID}'");

            switch (ftyStatus)
            {
                case TK_FtyStatus.New:
                    return new DualResult(false, "WH not confirm packing list yet.");
                case TK_FtyStatus.Confirmed:
                    return new DualResult(false, "TK already confirmed.");
                case TK_FtyStatus.RequestSeparate:
                    return new DualResult(false, "Already sent request to TPE.");
                case TK_FtyStatus.WHSeparateConfirm:
                case TK_FtyStatus.ShippingSeparateConfirm:
                    return new DualResult(false, "TK already separated.");
                default:
                    return new DualResult(true);
            }

        }
    }
}
