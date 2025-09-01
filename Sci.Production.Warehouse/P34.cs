using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P34 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P34(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;

            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P34(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "B";
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

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["AdjustQty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Adjust Qty can't be 0 (Original Qty and Current Qty are equal in Quantity)",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Reason ID can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }
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

            // 檢查物料不能有WMS Location && IsFromWMS = 0
            if (!PublicPrg.Prgs.Chk_WMS_Location_Adj((DataTable)this.detailgridbs.DataSource) && MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
            {
                MyUtility.Msg.WarningBox("Material Location or Adjust is from WMS system cannot save or confirmed. ", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "AI", "Adjust", (DateTime)this.CurrentMaintain["Issuedate"]);
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

            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
                {
                    this.gridicon.Remove.Visible = false;
                    this.gridicon.Remove.Enabled = false;
                    this.btnImport.Enabled = false;
                }
                else
                {
                    this.gridicon.Remove.Visible = true;
                    this.gridicon.Remove.Enabled = true;
                    this.btnImport.Enabled = true;
                }
            }
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnDetailGridSetup()
        {
            #region -- Current Qty Vaild 判斷 --

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();

            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && string.Compare(this.CurrentDetailData["qtyafter"].ToString(), e.FormattedValue.ToString()) != 0)
                {
                    this.CurrentDetailData["qtyafter"] = e.FormattedValue;
                    this.CurrentDetailData["adjustqty"] = (decimal)e.FormattedValue - (decimal)this.CurrentDetailData["qtybefore"];
                }
            };

            #endregion
            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        poitems,
                        "ID,Name",
                        "5,150",
                        this.CurrentDetailData["reasonid"].ToString(),
                        "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.CurrentDetailData["reasonid"] = x[0]["id"];
                    this.CurrentDetailData["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["reasonid"] = string.Empty;
                        this.CurrentDetailData["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.CurrentDetailData["reasonid"] = e.FormattedValue;
                            this.CurrentDetailData["reason_nm"] = dr["name"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗
            #region -- 欄位設定 --

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("qtybefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("qtyafter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns)
            .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4))
            .Text("Location", header: "Location", iseditingreadonly: true)
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            .Text("reasonid", header: "Reason ID", settings: ts)
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.detailgrid.Columns["qtyafter"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
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
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            StringBuilder upd_MD_8T = new StringBuilder();
            StringBuilder upd_MD_32T = new StringBuilder();
            StringBuilder upd_Fty_8T = new StringBuilder();
            string sqlcmd = string.Empty;
            string ids = string.Empty;
            DualResult result2;
            DataTable datacheck;

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查物料不能有WMS Location && IsFromWMS = 0
            if (!PublicPrg.Prgs.Chk_WMS_Location_Adj((DataTable)this.detailgridbs.DataSource) && MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
            {
                MyUtility.Msg.WarningBox("Material Location or Adjust is from WMS system cannot save or confirmed. ", "Warning");
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,isnull(d.QtyAfter,0.00) - isnull(d.QtyBefore,0.00) qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Adjust_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Adjust_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "Adjust_Detail") == false)
            {
                return;
            }
            #endregion

            // 檢查負數庫存
            if (!Prgs.CheckAdjustBalance(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: true))
            {
                return;
            }

            #region 更新 MdivisionPoDetail
            var data_MD_8T32T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
                                     Qty = m.Sum(w => w.Field<decimal>("QtyAfter") - w.Field<decimal>("QtyBefore")),
                                 }).ToList();

            upd_MD_8T.Append(Prgs.UpdateMPoDetail(8, data_MD_8T32T, true));
            upd_MD_32T.Append(Prgs.UpdateMPoDetail(32, null, true));
            #endregion

            #region 更新庫存數量  ftyinventory
            DataTable data_Fty_8T = (DataTable)this.detailgridbs.DataSource;
            data_Fty_8T.ColumnsDecimalAdd("qty", expression: "QtyAfter- QtyBefore");

            upd_Fty_8T.Append(Prgs.UpdateFtyInventory_IO(8, null, true));
            #endregion 更新庫存數量  ftyinventory

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T32T, string.Empty, upd_MD_8T.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T32T, string.Empty, upd_MD_32T.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        data_Fty_8T, string.Empty, upd_Fty_8T.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Adjust set status='Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
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
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["stocktakingid"]))
            {
                MyUtility.Msg.WarningBox("This adjust is created by stocktaking, can't unconfirm!!", "Warning");
                return;
            }

            if (MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 取得 FtyInventory 資料
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            StringBuilder upd_MD_8F = new StringBuilder();
            StringBuilder upd_MD_32F = new StringBuilder();
            StringBuilder upd_Fty_8F = new StringBuilder();
            string sqlcmd = string.Empty;
            string ids = string.Empty;
            DualResult result2;
            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P34"))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,isnull(d.QtyAfter,0.00) - isnull(d.QtyBefore,0.00) qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Adjust_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Adjust_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(dt, "Adjust_Detail"))
            {
                return;
            }
            #endregion

            // 檢查負數庫存
            if (!Prgs.CheckAdjustBalance(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: false))
            {
                return;
            }

            #region -- 更新 MdivisionPoDetail --
            var data_MD_8F32F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
                                     Qty = -m.Sum(w => w.Field<decimal>("QtyAfter") - w.Field<decimal>("QtyBefore")),
                                 }).ToList();

            upd_MD_8F.Append(Prgs.UpdateMPoDetail(8, data_MD_8F32F, false));
            upd_MD_32F.Append(Prgs.UpdateMPoDetail(32, null, false));
            #endregion
            #region -- 更新庫存數量  ftyinventory --

            DataTable data_Fty_8F = (DataTable)this.detailgridbs.DataSource;

            // dtio.ColumnsDecimalAdd("qty", expression: "QtyAfter- QtyBefore");
            data_Fty_8F.Columns.Add("qty", typeof(decimal));
            foreach (DataRow dx in data_Fty_8F.Rows)
            {
                dx["qty"] = -((decimal)dx["QtyAfter"] - (decimal)dx["QtyBefore"]);
            }

            upd_Fty_8F.Append(Prgs.UpdateFtyInventory_IO(8, null, false));

            #endregion 更新庫存數量  ftyinventory

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F32F, string.Empty, upd_MD_8F.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F32F, string.Empty, upd_MD_32F.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        data_Fty_8F, string.Empty, upd_Fty_8F.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Adjust set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select a.id,a.PoId,a.Seq1,a.Seq2
    ,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
    ,psd.FabricType
    ,psd.stockunit
    ,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
    ,a.Roll
    ,a.Dyelot
    ,a.QtyAfter
    ,a.QtyBefore
    ,isnull(a.QtyAfter,0.00) - isnull(a.QtyBefore,0.00) adjustqty
    ,a.ReasonId
    ,(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND ID= A.ReasonId) reason_nm
    ,a.StockType
    ,dbo.Getlocation(fi.ukey) location
    ,fi.ContainerCode
    ,a.ukey
    ,a.ftyinventoryukey
    ,ColorID =dbo.GetColorMultipleID(psd.BrandId, isnull(psdsC.SpecValue, ''))
from dbo.Adjust_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.PoId and psd.seq1 = a.SEQ1 and psd.SEQ2 = a.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
Where a.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P34_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
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

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P34", this);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            WH_Print p = new WH_Print(this.CurrentMaintain, "P34")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                DualResult result;
                #region -- 撈表頭資料 --
                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@MDivision", this.CurrentMaintain["MDivisionID"]),
                    new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()),
                };
                result = DBProxy.Current.Select(string.Empty, @"select NameEn from MDivision where id = @MDivision", pars, out DataTable dt);

                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dt");
                    return false;
                }

                string rptTitle = dt.Rows[0]["NameEN"].ToString();
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new ReportParameter("ID", this.CurrentMaintain["ID"].ToString()));
                report.ReportParameters.Add(new ReportParameter("Remark", this.CurrentMaintain["Remark"].ToString()));
                report.ReportParameters.Add(new ReportParameter("CDate", ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["IssueDate"])).ToShortDateString()));
                report.ReportParameters.Add(new ReportParameter("FtyGroup", this.CurrentMaintain["FactoryID"].ToString()));

                #endregion
                #region -- 撈表身資料 --
                string sqlcmd = @"
select ad.POID
	, [SEQ] = ad.Seq1 + '-' + ad.Seq2
	, [DESC] =IIF((ad.POID = lag(ad.POID,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll) 
			    AND(ad.seq1 = lag(ad.seq1,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll))
			    AND(ad.seq2 = lag(ad.seq2,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll))) 
			    ,''
                ,dbo.getMtlDesc(ad.poid, ad.seq1, ad.seq2, 2, 0))
	, [Location] = dbo.Getlocation(fi.ukey)
    , fi.ContainerCode
	, p.StockUnit
	, ad.Roll
	, ad.Dyelot
	, [QTY] = isnull(ad.QtyAfter,0.00) - isnull(ad.QtyBefore,0.00)
    , [TotalQTY] = sum(isnull(ad.QtyAfter,0.00) - isnull(ad.QtyBefore,0.00)) OVER (PARTITION BY ad.POID ,ad.seq1, ad.seq2)
from Adjust_Detail ad WITH (NOLOCK)
left join PO_Supp_Detail p WITH (NOLOCK) on p.ID = ad.PoId and p.seq1 = ad.SEQ1 and p.SEQ2 = ad.seq2
left join FtyInventory fi WITH (NOLOCK) on ad.poid = fi.poid and ad.seq1 = fi.seq1 and ad.seq2 = fi.seq2
						and ad.roll = fi.roll and ad.stocktype = fi.stocktype and ad.Dyelot = fi.Dyelot
where ad.ID = @ID
order by ad.POID, SEQ, ad.Dyelot, ad.Roll
";
                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtDetail);
                if (!result)
                {
                    this.ShowErr(sqlcmd, result);
                }

                if (dtDetail == null || dtDetail.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtDetail");
                    return false;
                }

                // 傳 list 資料
                List<P34_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P34_PrintData()
                    {
                        POID = row1["POID"].ToString().Trim(),
                        SEQ = row1["SEQ"].ToString().Trim(),
                        DESC = row1["DESC"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        StockUnit = row1["StockUnit"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        DYELOT = row1["Dyelot"].ToString().Trim(),
                        QTY = row1["Qty"].ToString().Trim(),
                        TotalQTY = row1["TotalQTY"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;
                #endregion
                #region 指定是哪個 RDLC

                // DualResult result;
                Type reportResourceNamespace = typeof(P13_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P34_Print.rdlc";

                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    // this.ShowException(result);
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();
                #endregion
            }

            return base.ClickPrint();
        }
    }
}