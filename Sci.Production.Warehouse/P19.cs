﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using Sci.Win;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P19 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer() { Dock = DockStyle.Fill };
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");

            this.Controls.Add(this.viewer);

            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P19(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.txtFromFactory.Text))
            {
                MyUtility.Msg.WarningBox("To Factory cannot be null! ");
                this.txtFromFactory.Focus();
                return false;
            }

            bool isConfirmed = MyUtility.Check.Seek($"select 1 from Transferout with (nolock) where ID = '{this.CurrentMaintain["ID"]}' and Status = 'Confirmed'");
            if (isConfirmed)
            {
                MyUtility.Msg.WarningBox("This record already Confirmed, can not save");
                return false;
            }
            #endregion 必輸檢查
            this.detailgrid.ValidateControl();
            this.detailgridbs.EndEdit();

            #region 檢查TransferExport 來源資料
            var detailFromTransferExport = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["TransferExportID"]));
            if (detailFromTransferExport.Any())
            {
                string whereTransferExportID = detailFromTransferExport.Select(s => $"'{s["TransferExportID"]}'").Distinct().JoinToString(",");
                string sqlCheckTransferExportStatus = $@"
select 1 from TransferExport with (nolock) where ID in ({whereTransferExportID}) and Sent <> 1
";
                if (MyUtility.Check.Seek(sqlCheckTransferExportStatus))
                {
                    MyUtility.Msg.WarningBox("TPE status is not 'Sent', cannot create transfer out record.");
                    return false;
                }

                string whereTransferExport_DetailUkey = detailFromTransferExport.Select(s => $"'{s["TransferExport_DetailUkey"]}'").JoinToString(",");
                string sqlCheckAlreadyCreated = $@"
select  [From SP#] = POID,
        [From Seq] = Concat(Seq1, ' ', Seq2), 
        [To SP#] = ToPOID, 
        [To Seq] = Concat(ToSeq1, ' ', ToSeq2),  
        [PO Qty] = Qty, 
        [Transer Out ID] = ID
from TransferOut_Detail with (nolock)
where   TransferExport_DetailUkey in ({whereTransferExport_DetailUkey}) and
        ID <> '{this.CurrentMaintain["ID"]}'
";
                DataTable dtAlreadyCreated;
                DualResult resultCheckAlreadyCreated = DBProxy.Current.Select(null, sqlCheckAlreadyCreated, out dtAlreadyCreated);
                if (!resultCheckAlreadyCreated)
                {
                    this.ShowErr(resultCheckAlreadyCreated);
                    return false;
                }

                if (dtAlreadyCreated.Rows.Count > 0)
                {
                    MyUtility.Msg.ShowMsgGrid(dtAlreadyCreated, "Cannot be save, please check below transfer export material exist another transfer out record.Confirm");
                    return false;
                }
            }

            #endregion

            foreach (DataRow row in this.DetailDatas)
            {
                // TransferExportID 不為空 允許 StockType, Roll, Dyelot 為空
                if (!MyUtility.Check.Empty(row["TransferExportID"]))
                {
                    continue;
                }

                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Transfer Out Qty can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                List<string> seqList = Sci.Production.Warehouse.P19_Import.ToSeqSplit(MyUtility.Convert.GetString(row["ToSeq"]));
                row["ToSeq1"] = seqList[0];
                row["ToSeq2"] = seqList.Count > 1 ? seqList[1] : string.Empty;
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "TO", "TransferOut", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (this.CurrentMaintain["status"].ToString().EqualString("Confirmed"))
            {
                this.btnPrintFabricSticker.Enabled = true;
            }
            else
            {
                this.btnPrintFabricSticker.Enabled = false;
            }

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["Stocktype"] = 'B';
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            DataGridViewGeneratorNumericColumnSettings qtySetting = new DataGridViewGeneratorNumericColumnSettings();
            qtySetting.CellValidating += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                decimal outQty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                DataRow curRow = this.detailgrid.GetDataRow(e.RowIndex);

                if (outQty > 0 &&
                    !MyUtility.Check.Empty(curRow["TransferExportID"]) &&
                    MyUtility.Convert.GetLong(curRow["FtyInventoryUkey"]) < 0)
                {
                    e.Cancel = true;
                    curRow["qty"] = 0;
                    MyUtility.Msg.WarningBox("<Out Qty> No Stock can not modify");
                    return;
                }
            };

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("ExportId", header: "WK#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 1
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
            .Numeric("qty", header: "Out Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, settings: qtySetting) // 6
            .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), iseditable: false).Get(out cbb_stocktype) // 7
            .Text("Location", header: "Location", iseditingreadonly: true) // 8
            .Text("ToPOID", header: "To POID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ToSeq", header: "To Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("TransferExportID", header:"Transfer WK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            ;
            #endregion 欄位設定
            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Confirm

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string sqlupd2_B = string.Empty;
            string sqlupd2_BI = string.Empty;
            string sqlupd2_FIO = string.Empty;

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P19"))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.TransferOut_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "TransferOut_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
    ,Qty = sum(d.Qty)
    ,balanceQty = f.balanceQty
from dbo.TransferOut_Detail d WITH (NOLOCK)
outer apply(
	select balanceQty = sum(isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0))
	from FtyInventory f WITH (NOLOCK)
    where d.PoId = f.PoId and d.Seq1 = f.Seq1 and d.Seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
)f
where d.Id = '{this.CurrentMaintain["id"]}'
group by d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot,d.StockType ,f.balanceQty
having f.balanceQty - sum(d.Qty) < 0
";
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than Out qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update TransferOut set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新庫存數量 MDivisionPoDetail --
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       .Where(s => !MyUtility.Check.Empty(s["qty"]))
                       group b by new
                       {
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype").Trim(),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            var bs1I = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                        .Where(w => w.Field<string>("stocktype").Trim() == "I" && !MyUtility.Check.Empty(w["qty"]))
                       group b by new
                       {
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype").Trim(),
                       }
                        into m
                        select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            if (bs1.Count > 0)
            {
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);
            }

            if (bs1I.Count > 0)
            {
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, false);
            }
            #endregion

            #region 更新庫存數量  ftyinventory
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion 更新庫存數量  ftyinventory

            DataTable dtDetailExcludeQtyZero = new DataTable();
            if (this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["qty"])))
            {
                dtDetailExcludeQtyZero = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["qty"])).CopyToDataTable();
            }

            DataTable dtTransferExportDetail = new DataTable();
            string sqlInsertTransferExport_Detail_Carton = $@"

Insert into TransferExport_Detail_Carton(
TransferExport_DetailUkey  ,
ID                         ,
POID                       ,
Seq1                       ,
Seq2                       ,
Carton                     ,
LotNo                      ,
Qty                        ,
FOC                        ,
EditName                   ,
EditDate                   ,
StockUnitID                ,
StockQty
)
select  t.TransferExport_DetailUkey,
        t.TransferExportID,
        ted.POID,
        ted.Seq1,
        ted.Seq2,
        t.Roll,
        t.Dyelot,
        isnull(dbo.GetUnitQty(psdInv.StockUnit, ted.UnitID, t.Qty), 0),
        0,
        '{Env.User.UserID}',
        getdate(),
        isnull(psdInv.StockUnit, ''),
        t.Qty
from #tmp t
inner join TransferExport_Detail ted with (nolock) on ted.Ukey = t.TransferExport_DetailUkey
left join PO_Supp_Detail psdInv with (nolock) on	ted.InventoryPOID = psdInv.ID and 
													ted.InventorySeq1 = psdInv.SEQ1 and
													ted.InventorySeq2 = psdinv.SEQ2

";
            if (this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["TransferExportID"])))
            {
                dtTransferExportDetail = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["TransferExportID"])).CopyToDataTable();
            }

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, string.Empty, sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (dtDetailExcludeQtyZero.Rows.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(
                            dtDetailExcludeQtyZero, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (dtTransferExportDetail.Rows.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(
                            dtTransferExportDetail, string.Empty, sqlInsertTransferExport_Detail_Carton, out resulttb)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            // 要在confirmed 後才能取得當前Balance
            this.FtyBarcodeData(true);

            // AutoWH ACC WebAPI for Vstrong
            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                Task.Run(() => new Vstrong_AutoWHAccessory().SentIssue_Detail_New(dtDetail, "P19", "New"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }

            // AutoWH Fabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                Task.Run(() => new Gensong_AutoWHFabric().SentIssue_Detail_New(dtDetail, "P19"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            string sqlupd2_B = string.Empty;
            string sqlupd2_BI = string.Empty;
            string sqlupd2_FIO = string.Empty;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.TransferOut_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
    ,Qty = sum(d.Qty)
    ,balanceQty = f.balanceQty
from dbo.TransferOut_Detail d WITH (NOLOCK)
outer apply(
	select balanceQty = sum(isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0))
	from FtyInventory f WITH (NOLOCK)
    where d.PoId = f.PoId and d.Seq1 = f.Seq1 and d.Seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
)f
where d.Id = '{this.CurrentMaintain["id"]}'
group by d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot,d.StockType ,f.balanceQty
having f.balanceQty + sum(d.Qty) < 0
";
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than Out qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "TransferOut_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(dt, "TransferOut_Detail"))
            {
                return;
            }
            #endregion

            #region UnConfirmed 先檢查WMS是否傳送成功

            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

            bool accLock = true;
            bool fabricLock = true;

            // 主副料都有情況
            if (Prgs.Chk_Complex_Material(this.CurrentMaintain["ID"].ToString(), "TransferOut_Detail"))
            {
                if (!Vstrong_AutoWHAccessory.SentIssue_Detail_Delete(dtDetail, "P19", "Lock", isComplexMaterial: true))
                {
                    accLock = false;
                }

                if (!Gensong_AutoWHFabric.SentIssue_Detail_Delete(dtDetail, "P19", "Lock", isComplexMaterial: true))
                {
                    fabricLock = false;
                }

                // 如果WMS連線都成功,則直接unconfirmed刪除
                if (accLock && fabricLock)
                {
                    Vstrong_AutoWHAccessory.SentIssue_Detail_Delete(dtDetail, "P19", "UnConfirmed", isComplexMaterial: true);
                    Gensong_AutoWHFabric.SentIssue_Detail_Delete(dtDetail, "P19", "UnConfirmed", isComplexMaterial: true);
                }
                else
                {
                    // 個別成功的,傳WMS UnLock狀態並且都不能刪除
                    if (accLock)
                    {
                        Vstrong_AutoWHAccessory.SentIssue_Detail_Delete(dtDetail, "P19", "UnLock", isComplexMaterial: true);
                    }

                    if (fabricLock)
                    {
                        Gensong_AutoWHFabric.SentIssue_Detail_Delete(dtDetail, "P19", "UnLock", isComplexMaterial: true);
                    }

                    return;
                }
            }
            else
            {
                if (!Vstrong_AutoWHAccessory.SentIssue_Detail_Delete(dtDetail, "P19", "UnConfirmed"))
                {
                    return;
                }

                if (!Gensong_AutoWHFabric.SentIssue_Detail_Delete(dtDetail, "P19", "UnConfirmed"))
                {
                    return;
                }
            }
            #endregion

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update TransferOut set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新庫存數量 MDivisionPoDetail --
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       .Where(s => !MyUtility.Check.Empty(s["qty"]))
                       group b by new
                       {
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype").Trim(),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            var bs1I = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                        .Where(w => w.Field<string>("stocktype").Trim() == "I" && !MyUtility.Check.Empty(w["qty"]))
                        group b by new
                        {
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                            stocktype = b.Field<string>("stocktype").Trim(),
                        }
                        into m
                        select new Prgs_POSuppDetailData
                        {
                            Poid = m.First().Field<string>("poid"),
                            Seq1 = m.First().Field<string>("seq1"),
                            Seq2 = m.First().Field<string>("seq2"),
                            Stocktype = m.First().Field<string>("stocktype"),
                            Qty = m.Sum(w => w.Field<decimal>("qty")),
                        }).ToList();

            if (bs1.Count > 0)
            {
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);
            }

            if (bs1I.Count > 0)
            {
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, true);
            }
            #endregion

            #region 更新庫存數量  ftyinventory

            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         .Where(s => !MyUtility.Check.Empty(s["qty"]))
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -m.Field<decimal>("qty"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            DataTable dtTransferExportDetail = new DataTable();
            string sqlDeleteTransferExport_Detail_Carton = $@"
alter table #tmp alter column Roll varchar(8)
alter table #tmp alter column Dyelot varchar(8)

delete  tedc
from    TransferExport_Detail_Carton tedc
where   exists(select 1 from #tmp t where 
                t.TransferExport_DetailUkey = tedc.TransferExport_DetailUkey and
                t.Roll = tedc.Carton and
                t.Dyelot = tedc.LotNo
                )         
";
            if (this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["TransferExportID"])))
            {
                dtTransferExportDetail = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["TransferExportID"])).CopyToDataTable();
            }

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, string.Empty, sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (bsfio.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (dtTransferExportDetail.Rows.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(
                            dtTransferExportDetail, string.Empty, sqlDeleteTransferExport_Detail_Carton, out resulttb)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            // 要在unconfirmed 後才能取得當前Balance
            this.FtyBarcodeData(false);
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,p1.StockUnit
,a.Qty
,a.StockType
,a.ukey
,dbo.Getlocation(fi.ukey) location
,a.ftyinventoryukey
,a.ToPOID
,a.ToSeq1
,a.ToSeq2
,[ToSeq] = a.ToSeq1 +' ' + a.ToSeq2
,wk.ExportId
, p1.Refno
, Color = IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
										,IIF( p1.SuppColor = '' or p1.SuppColor is null,dbo.GetColorMultipleID(o.BrandID,p1.ColorID),p1.SuppColor)
										,dbo.GetColorMultipleID(o.BrandID,p1.ColorID)
									)
, p1.SizeSpec
,a.TransferExportID
,a.TransferExport_DetailUkey
from dbo.TransferOut_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join View_WH_Orders o WITH (NOLOCK) on p1.ID = o.ID
left join Fabric on Fabric.SCIRefno = p1.SCIRefno
left join FtyInventory FI on a.POID = FI.POID and a.Seq1 = FI.Seq1 and a.Seq2 = FI.Seq2 and a.Dyelot = FI.Dyelot
    and a.Roll = FI.Roll and a.StockType = FI.StockType
outer apply(
	select ExportId = Stuff((
		select concat(',',ExportId)
		from (
				select 	distinct r.ExportId
				from Receiving_Detail rd
				inner join Receiving r on rd.Id = r.Id
				where rd.PoId = a.POID and rd.Seq1 = a.Seq1
				and rd.Seq2 = a.Seq2 and rd.Roll = a.Roll
				and rd.Dyelot = a.Dyelot
			) s
		for xml path ('')
	) , 1, 1, '')
) WK
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete qty is empty
        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Accumulated
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P19_AccumulatedQty(this.CurrentMaintain) { P19 = this };
            frm.ShowDialog(this);
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P19_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("poid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            new P19_Print(this.CurrentMaintain).ShowDialog();

            return true;
        }

        private void TxtFromFactory_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFromFactory.Text))
            {
                return;
            }

            if (!MyUtility.Check.Seek(string.Format(@"select * from scifty WITH (NOLOCK) where id='{0}'", this.txtFromFactory.Text)))
            {
                this.txtFromFactory.Text = string.Empty;
                MyUtility.Msg.WarningBox("To Factory : " + this.txtFromFactory.Text + " not found!");
                this.txtFromFactory.Focus();
                this.txtFromFactory.Select();
            }
        }

        private void TxtFromFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string cmd = "select ID from scifty WITH (NOLOCK) where mdivisionid<>'' and Junk<>1 order by MDivisionID,ID ";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(cmd, "6", this.txtFromFactory.ToString());
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtFromFactory.Text = item.GetSelectedString();
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P19_FabricSticker(this.CurrentMaintain["ID"], MyUtility.Convert.GetString(this.CurrentMaintain["remark"])).ShowDialog();
        }

        private void BtnImportonTPE_Click(object sender, EventArgs e)
        {
            var frm = new P19_ImportbaseonTPEstock(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = $@"
select
[Barcode1] = f.Barcode
,[Barcode2] = fb.Barcode
,[OriBarcode] = fbOri.Barcode
,[balanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[NewBarcode] = ''
,i2.Id,i2.POID,i2.Seq1,i2.Seq2,i2.StockType,i2.Roll,i2.Dyelot
from Production.dbo.TransferOut_Detail i2
inner join Production.dbo.TransferOut i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.POID
and f.Seq1 = i2.Seq1 and f.Seq2 = i2.Seq2
and f.Roll = i2.Roll and f.Dyelot = i2.Dyelot
and f.StockType = i2.StockType
outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
)fb
outer apply(
	select *
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = '{this.CurrentMaintain["ID"]}'
)fbOri
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
and i2.id = '{this.CurrentMaintain["ID"]}'

";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                string strBarcode = MyUtility.Check.Empty(dr["Barcode2"]) ? dr["Barcode1"].ToString() : dr["Barcode2"].ToString();
                if (isConfirmed)
                {
                    // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                    if (!MyUtility.Check.Empty(dr["balanceQty"]) && !MyUtility.Check.Empty(strBarcode))
                    {
                        if (strBarcode.Contains("-"))
                        {
                            dr["NewBarcode"] = strBarcode.Substring(0, 13) + "-" + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                        }
                        else
                        {
                            dr["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                        }
                    }
                    else
                    {
                        // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                        dr["NewBarcode"] = dr["Barcode1"];
                    }
                }
                else
                {
                    // unConfirmed 要用自己的紀錄給補回
                    dr["NewBarcode"] = dr["OriBarcode"];
                }
            }

            var data_Fty_Barcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                    select new
                                    {
                                        TransactionID = m.Field<string>("ID"),
                                        poid = m.Field<string>("poid"),
                                        seq1 = m.Field<string>("seq1"),
                                        seq2 = m.Field<string>("seq2"),
                                        stocktype = m.Field<string>("stocktype"),
                                        roll = m.Field<string>("roll"),
                                        dyelot = m.Field<string>("dyelot"),
                                        Barcode = m.Field<string>("NewBarcode"),
                                    }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            string upd_Fty_Barcode_V1 = isConfirmed ? Prgs.UpdateFtyInventory_IO(70, null, !isConfirmed) : Prgs.UpdateFtyInventory_IO(72, null, true);
            string upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resulttb;
            if (data_Fty_Barcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V1, out resulttb, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V2, out resulttb, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P19", this);
        }

        private void BtnTransferWK_Click(object sender, EventArgs e)
        {
            new P19_TransferWKImport(this.CurrentMaintain["ID"].ToString(), (DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["MDivisionID"].ToString()).ShowDialog();
        }
    }
}