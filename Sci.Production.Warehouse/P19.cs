using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            // 從DB取得最新Status, 避免多工時, 畫面上資料不是最新的狀況
            this.RenewData();
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");

                // 重新整理畫面
                this.OnRefreshClick();
                return false;
            }

            // 重新整理畫面
            this.OnRefreshClick();
            return base.ClickDeleteBefore();
        }

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

        /// <inheritdoc/>
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

            #region 檢查 由工廠採購的物料是禁止轉出給其他廠(不管境內或跨國
            if (this.DetailDatas.Count > 0)
            {
                string sqlcheck = $@"
SELECT [POID], [SEQ1], [SEQ2], [Roll], [Dyelot], [StockType]
FROM #tmp t
WHERE EXISTS (--由工廠採購的物料是禁止轉出給其他廠(不管境內或跨國
    SELECT 1
    FROM Order_FtyMtlStdCost oc WITH (NOLOCK)
    WHERE oc.OrderID = t.POID
    AND oc.SCIRefno = t.SCIRefno
)
";
                DataTable dtDetail = this.DetailDatas.CopyToDataTable();
                DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(dtDetail, string.Empty, sqlcheck, out DataTable dtCheck);
                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                    return false;
                }

                if (dtCheck.Rows.Count > 0)
                {
                    string msg = "Transfer out of local procurement is prohibited!!";
                    foreach (DataRow row in dtCheck.Rows)
                    {
                        msg += $"\r\nSP#: {row["POID"]} Seq#: {row["SEQ1"]}-{row["SEQ2"]} Roll#: {row["Roll"]} Dyelot: {row["Dyelot"]} StockType: {row["StockType"]}";
                    }

                    MyUtility.Msg.WarningBox(msg, "Warning");
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
                    warningmsg.Append(string.Format("SP#: {0} Seq#: {1}-{2} can't be empty", row["poid"], row["seq1"], row["seq2"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append($"SP#: {row["poid"]} Seq#: {row["seq1"]}-{row["seq2"]} Roll#:{row["roll"]} Dyelot:{row["dyelot"]} Transfer Out Qty can't be empty" + Environment.NewLine);
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

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

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

            string sqlcmd_BtnTKSeparateHistory = $@"select 1 from TransferOut t with(nolock)
                                                    inner join TransferOut_Detail td with(nolock) on t.id = td.id
                                                    left join TransferExport te with(nolock) on td.TransferExportID = te.id
                                                    where t.id = '{this.CurrentMaintain["ID"].ToString()}' and te.Separated = 1";

            this.BtnTKSeparateHistory.ForeColor = MyUtility.Check.Seek(sqlcmd_BtnTKSeparateHistory) ? Color.Blue : DefaultForeColor;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["Stocktype"] = 'B';
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;
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
            .Text("Tone", header: "Tone/Grp", iseditingreadonly: true)
            .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), iseditable: false).Get(out cbb_stocktype) // 7
            .Text("Location", header: "Location", iseditingreadonly: true) // 8
            .Numeric("RecvKG", header: "Recv (kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("ActualWeight", header: "Act. (kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditable: true)
            .Text("StyleID", header: "Style", iseditingreadonly: true)
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            .Text("ToPOID", header: "To POID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ToSeq", header: "To Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("TransferExportID", header: "Transfer WK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            ;
            #endregion 欄位設定
            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
            DataTable dtQty = detailDt.Select("Qty > 0").TryCopyToDataTable(detailDt);
            DualResult result = Prgs.GetFtyInventoryData(dtQty, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
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
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
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

            #region 檢查Location是否為空值
            string sqlWMSLocation = $@"
select distinct td.POID,seq = concat(Ltrim(Rtrim(td.seq1)), ' ', td.Seq2),td.Roll,td.Dyelot
 , StockType = case td.StockType 
		when 'B' then 'Bulk' 
		when 'I' then 'Inventory' 
		when 'O' then 'Scrap' 
		else td.StockType 
		end
  , [Location] = dbo.Getlocation(f.ukey)
from TransferOut_Detail td
 left join Production.dbo.FtyInventory f on f.POID = td.POID 
	and f.Seq1=td.Seq1 and f.Seq2=td.Seq2 
	and f.Roll=td.Roll and f.Dyelot=td.Dyelot
    and f.StockType = td.StockType
where dbo.Getlocation(f.ukey) is null
and td.Qty != 0
and td.id = '{this.CurrentMaintain["ID"]}'
";
            if (!(result = DBProxy.Current.Select(string.Empty, sqlWMSLocation, out DataTable dtLocationDetail)))
            {
                this.ShowErr(result.ToString());
                return;
            }

            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                if (dtLocationDetail != null && dtLocationDetail.Rows.Count > 0)
                {
                    // change column name
                    dtLocationDetail.Columns["PoId"].ColumnName = "SP#";
                    dtLocationDetail.Columns["seq"].ColumnName = "Seq";
                    dtLocationDetail.Columns["Roll"].ColumnName = "Roll";
                    dtLocationDetail.Columns["Dyelot"].ColumnName = "Dyelot";
                    dtLocationDetail.Columns["StockType"].ColumnName = "Stock Type";
                    Prgs.ChkLocationEmpty(dtLocationDetail, string.Empty, "SP#,Seq,Roll,Dyelot,Stock Type");
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
having f.balanceQty - sum(d.Qty) < 0
";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
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

            #region 更新庫存總表 MDivisionPoDetail ( Pkey : poid,seq1,seq2 )
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       .Where(s => !MyUtility.Check.Empty(s["qty"]))
                       group b by new
                       {
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            var bs1I = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                        .Where(w => w.Field<string>("stocktype").Trim() == "I")
                        group b by new
                        {
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                        }
                        into m
                        select new Prgs_POSuppDetailData
                        {
                            Poid = m.First().Field<string>("poid"),
                            Seq1 = m.First().Field<string>("seq1"),
                            Seq2 = m.First().Field<string>("seq2"),
                            Qty = -m.Sum(w => w.Field<decimal>("qty")),
                        }).ToList();
            #endregion

            #region 更新庫存數量  ftyinventory
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            DataTable dtDetailExcludeQtyZero = new DataTable();
            if (this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["qty"])))
            {
                dtDetailExcludeQtyZero = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["qty"])).CopyToDataTable();
            }
            #endregion 更新庫存數量  ftyinventory

            DataTable dtTransferExportDetail = new DataTable();
            string sqlInsertTransferExport_Detail_Carton = $@"

Insert into TransferExport_Detail_Carton(
TransferExport_DetailUkey  ,
ID                         ,
POID                       ,
Seq1                       ,
Seq2                       ,
Roll                     ,
LotNo                      ,
Qty                        ,
FOC                        ,
EditName                   ,
EditDate                   ,
StockUnitID                ,
StockQty                   ,
Tone                       ,
MINDQRCode
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
        t.Qty,
        isnull(t.Tone, ''),
        [MINDQRCode] = iif(t.Qty = 0, '', ISNULL(w.MINDQRCode, ''))
from #tmp t
inner join TransferExport_Detail ted with (nolock) on ted.Ukey = t.TransferExport_DetailUkey
left join PO_Supp_Detail psdInv with (nolock) on	ted.InventoryPOID = psdInv.ID and 
													ted.InventorySeq1 = psdInv.SEQ1 and
													ted.InventorySeq2 = psdinv.SEQ2
outer apply (
	select [MINDQRCode] = iif(isnull(w.To_NewBarcodeSeq, '') = '', w.To_NewBarcode, concat(w.To_NewBarcode, '-', w.To_NewBarcodeSeq))
	from WHBarcodeTransaction w with (nolock) 
	where t.Ukey = w.TransactionUkey 
	and w.Action = 'Confirm' 
	and [Function] = 'P19'
)w
";
            if (this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["TransferExportID"])))
            {
                dtTransferExportDetail = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["TransferExportID"])).CopyToDataTable();
            }

            Exception errMsg = null;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        string sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (bs1I.Count > 0)
                    {
                        string sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, string.Empty, sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (dtDetailExcludeQtyZero.Rows.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetailExcludeQtyZero, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update TransferOut set status = 'Confirmed', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, dtQty, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    // Barcode 產生後再寫入 TransferExport_Detail_Carton
                    if (dtTransferExportDetail.Rows.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(dtTransferExportDetail, string.Empty, sqlInsertTransferExport_Detail_Carton, out resulttb)))
                        {
                            throw result.GetException();
                        }
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, dtQty, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 取得 FtyInventory 資料
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;

            // 檢查 Transfer WK# factory warehouse team already confirmed
            var detailFromTransferExport = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["TransferExportID"]));
            if (detailFromTransferExport.Any())
            {
                string whereTransferExportID = detailFromTransferExport.Select(s => $"'{s["TransferExportID"]}'").Distinct().JoinToString(",");
                string sqlCheckTransferExportStatus = $@"select ID from TransferExport with (nolock) where ID in ({whereTransferExportID}) and FtyStatus <> 'New'";
                result = DBProxy.Current.Select(null, sqlCheckTransferExportStatus, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    string wks = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ID"])).Distinct().JoinToString(",");
                    MyUtility.Msg.WarningBox($"Transfer WK# factory warehouse team already confirmed, cannot UncConfirm transfer out record.\r\n{wks}");
                    return;
                }
            }

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.TransferOut_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
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
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
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
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "TransferOut_Detail"))
            {
                return;
            }
            #endregion

            #region 更新庫存總表 MDivisionPoDetail ( Pkey : poid,seq1,seq2 )
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       .Where(s => !MyUtility.Check.Empty(s["qty"]))
                       group b by new
                       {
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            var bs1I = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                        .Where(w => w.Field<string>("stocktype").Trim() == "I")
                        group b by new
                        {
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                        }
                        into m
                        select new Prgs_POSuppDetailData
                        {
                            Poid = m.First().Field<string>("poid"),
                            Seq1 = m.First().Field<string>("seq1"),
                            Seq2 = m.First().Field<string>("seq2"),
                            Qty = m.Sum(w => w.Field<decimal>("qty")),
                        }).ToList();
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
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
            DataTable dtQty = detailDt.Select("Qty > 0").TryCopyToDataTable(detailDt);

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock(dtQty, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            DataTable dtTransferExportDetail = new DataTable();
            string sqlDeleteTransferExport_Detail_Carton = $@"
alter table #tmp alter column Roll varchar(8)
alter table #tmp alter column Dyelot varchar(8)

delete  tedc
from    TransferExport_Detail_Carton tedc
where   exists(select 1 from #tmp t where 
                t.TransferExport_DetailUkey = tedc.TransferExport_DetailUkey and
                t.Roll = tedc.Roll and
                t.Dyelot = tedc.LotNo
                )         
";
            if (this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["TransferExportID"])))
            {
                dtTransferExportDetail = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["TransferExportID"])).CopyToDataTable();
            }

            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        string sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (bs1I.Count > 0)
                    {
                        string sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, true);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, string.Empty, sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (bsfio.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update TransferOut set status = 'New', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, dtQty, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    if (dtTransferExportDetail.Rows.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(dtTransferExportDetail, string.Empty, sqlDeleteTransferExport_Detail_Carton, out resulttb)))
                        {
                            throw result.GetException();
                        }
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, dtQty, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSprocess(false, dtQty, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, dtQty, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);

            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select a.*
,[SEQ] = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
,[Description] = dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0)
,[ToSeq] = a.ToSeq1 +' ' + a.ToSeq2
,psd.StockUnit
,dbo.Getlocation(fi.ukey) location
,[ContainerCode] = FI.ContainerCode
,wk.ExportId
, psd.Refno
, Color = IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
										,IIF( psd.SuppColor = '' or psd.SuppColor is null,dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')),psd.SuppColor)
										,dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, ''))
									)
,SizeSpec= isnull(psdsS.SpecValue, '')
,fi.Tone
,[StyleID] = o.StyleID
,[RecvKG] = case when rd.ActualQty is not null 
			then case when rd.ActualQty <> a.Qty
					then ''
					else cast(iif(ISNULL(rd.ActualWeight,0) > 0, rd.ActualWeight, rd.Weight) as varchar(20))
					end
			else case when td.ActualQty <> a.Qty
					then ''
					else cast(iif(ISNULL(td.ActualWeight,0) > 0, td.ActualWeight, td.Weight) as varchar(20))
					end							
		end
    ,psd.SCIRefno
from dbo.TransferOut_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.PoId and psd.seq1 = a.SEQ1 and psd.SEQ2 = a.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join View_WH_Orders o WITH (NOLOCK) on psd.ID = o.ID
left join Fabric on Fabric.SCIRefno = psd.SCIRefno
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
Outer apply (
	select [Weight] = SUM(rd.Weight)
		, [ActualWeight] = SUM(rd.ActualWeight)
		, [ActualQty] = SUM(rd.ActualQty)
	from Receiving_Detail rd WITH (NOLOCK) 
	where fi.POID = rd.PoId
	and fi.Seq1 = rd.Seq1
	and fi.Seq2 = rd.Seq2 
	and fi.Dyelot = rd.Dyelot
	and fi.Roll = rd.Roll
	and fi.StockType = rd.StockType
    and psd.FabricType = 'F'
)rd
Outer apply (
	select [Weight] = SUM(td.Weight)
		, [ActualWeight] = SUM(td.ActualWeight)
		, [ActualQty] = SUM(td.Qty)
	from TransferIn_Detail td WITH (NOLOCK) 
	where fi.POID = td.PoId
	and fi.Seq1 = td.Seq1
	and fi.Seq2 = td.Seq2 
	and fi.Dyelot = td.Dyelot
	and fi.Roll = td.Roll
	and fi.StockType = td.StockType
    and psd.FabricType = 'F'
)td
Where a.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P19_AccumulatedQty(this.CurrentMaintain) { P19 = this };
            frm.ShowDialog(this);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            new P19_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["MDivisionID"].ToString()).ShowDialog();
            this.RenewData();
        }

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
            new P19_ImportbaseonTPEstock(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["MDivisionID"].ToString()).ShowDialog();
            this.RenewData();
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
        }

        private void BtnTransferWK_Click(object sender, EventArgs e)
        {
            new P19_TransferWKImport(this.CurrentMaintain["ID"].ToString(), (DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["MDivisionID"].ToString()).ShowDialog();
            this.RenewData();
        }

        private void BtnTKSeparateHistory_Click(object sender, EventArgs e)
        {
            P19_TKSeparateHistory windows = new P19_TKSeparateHistory(this.CurrentMaintain["ID"].ToString());
            windows.ShowDialog(this);
        }
    }
}