﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Sci.Production.Automation;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P35 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_reason = new Ict.Win.UI.DataGridViewTextBoxColumn();

        /// <summary>
        /// Initializes a new instance of the <see cref="P35"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P35(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;

            this.DefaultFilter = string.Format("Type='A' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="P35"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        /// <param name="transID">trans ID</param>
        public P35(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
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
            this.CurrentMaintain["Type"] = "A";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

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

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["poid"],
                        row["seq1"],
                        row["seq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["AdjustQty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Adjust Qty can't be empty",
                        row["poid"],
                        row["seq1"],
                        row["seq2"],
                        row["roll"],
                        row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Reason ID can't be empty",
                        row["poid"],
                        row["seq1"],
                        row["seq2"],
                        row["roll"],
                        row["dyelot"]) + Environment.NewLine);
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "AB", "Adjust", (DateTime)this.CurrentMaintain["Issuedate"]);
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

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        /// <inheritdoc/>
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
                if (this.EditMode && e.Button == MouseButtons.Right )
                {
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable poitems);
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
                        "ID,Name")
                    {
                        Width = 600,
                    };
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
                            string.Format("select id, Name from Reason WITH (NOLOCK) where id = '{0}' and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue),
                            out DataRow dr,
                            null))
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
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Numeric("qtybefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 5
            .Numeric("qtyafter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns) // 6
            .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
            .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4)) // 8
            .Text("Location", header: "Location", iseditingreadonly: true) // 9
            .Text("reasonid", header: "Reason ID", settings: ts).Get(out this.col_reason) // 10
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 11
            ;
            #endregion 欄位設定

            this.detailgrid.Columns["qtyafter"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2_B = new StringBuilder();
            StringBuilder sqlupd2_FIO = new StringBuilder();

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

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
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["Dyelot"]);
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

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,isnull(d.QtyAfter,0.00) - isnull(d.QtyBefore,0.00) qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Adjust_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot
where (isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) + (isnull(d.QtyAfter,0) - isnull(d.QtyBefore,0)) < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than Adjust qty: {5}" + Environment.NewLine,
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["balanceqty"],
                            tmp["qty"],
                            tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Adjust set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'",
                Env.User.UserID,
                this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新 MdivisionPoDetail --
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
                           Qty = m.Sum(w => w.Field<decimal>("qtyafter") - w.Field<decimal>("qtybefore")),
                       }).ToList();

            sqlupd2_B.Append(Prgs.UpdateMPoDetail(32, null, true));
            #endregion
            #region 更新庫存數量  ftyinventory
            DataTable dtio = (DataTable)this.detailgridbs.DataSource;
            dtio.ColumnsDecimalAdd("qty", expression: "QtyAfter- QtyBefore");
            sqlupd2_FIO.Append(Prgs.UpdateFtyInventory_IO(8, null, true));
            #endregion 更新庫存數量  ftyinventory

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out DataTable resulttb, "#TmpSource")))
                    {
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        dtio, string.Empty, sqlupd2_FIO.ToString(), out resulttb, "#TmpSource")))
                    {
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        return;
                    }

                    this.FtyBarcodeData(true);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    result = new DualResult(false, string.Join("Commit transaction error.", Environment.NewLine, ex));
                }
            }

            if (result)
            {
                MyUtility.Msg.InfoBox("Confirmed successful");
                DataTable dt = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

                // AutoWH ACC WebAPI for Vstrong
                if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
                {
                    Task.Run(() => new Vstrong_AutoWHAccessory().SentAdjust_Detail_New(dt, "New"))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }

                // AutoWH Fabric WebAPI for Gensong
                if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                {
                    Task.Run(() => new Gensong_AutoWHFabric().SentAdjust_Detail_New(dt))
                   .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            else
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["stocktakingid"]))
            {
                MyUtility.Msg.WarningBox("This adjust is created by stocktaking, can't unconfirm!!", "Warning");
                return;
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            StringBuilder sqlupd2_B = new StringBuilder();
            StringBuilder sqlupd2_FIO = new StringBuilder();

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
Select d.poid,d.seq1,d.seq2,d.Roll,isnull(d.QtyAfter,0.00) - isnull(d.QtyBefore,0.00) qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Adjust_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot
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
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["Dyelot"]);
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

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,isnull(d.QtyAfter,0.00) - isnull(d.QtyBefore,0.00) qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Adjust_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot
where (isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - (isnull(d.QtyAfter,0.00) - isnull(d.QtyBefore,0.00)) < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    // 資料量太多，超過20筆的話訊息不要全部跑完
                    int index = 0;
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than Adjust qty: {5}" + Environment.NewLine,
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["balanceqty"],
                            tmp["qty"],
                            tmp["Dyelot"]);
                        if (index > 20)
                        {
                            ids += "......and more.";
                            break;
                        }

                        index++;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region UnConfirmed 先檢查WMS是否傳送成功

            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

            bool accLock = true;
            bool fabricLock = true;

            // 主副料都有情況
            if (Prgs.Chk_Complex_Material(this.CurrentMaintain["ID"].ToString(), "Adjust_Detail"))
            {
                if (!Vstrong_AutoWHAccessory.SentAdjust_Detail_Delete(dtDetail, "Lock", isComplexMaterial: true))
                {
                    accLock = false;
                }

                if (!Gensong_AutoWHFabric.SentAdjust_Detail_Delete(dtDetail, "Lock", isComplexMaterial: true))
                {
                    fabricLock = false;
                }

                // 如果WMS連線都成功,則直接unconfirmed刪除
                if (accLock && fabricLock)
                {
                    Vstrong_AutoWHAccessory.SentAdjust_Detail_Delete(dtDetail, "UnConfirmed", isComplexMaterial: true);
                    Gensong_AutoWHFabric.SentAdjust_Detail_Delete(dtDetail, "UnConfirmed", isComplexMaterial: true);
                }
                else
                {
                    // 個別成功的,傳WMS UnLock狀態並且都不能刪除
                    if (accLock)
                    {
                        Vstrong_AutoWHAccessory.SentAdjust_Detail_Delete(dtDetail, "UnLock", isComplexMaterial: true);
                    }

                    if (fabricLock)
                    {
                        Gensong_AutoWHFabric.SentAdjust_Detail_Delete(dtDetail, "UnLock", isComplexMaterial: true);
                    }

                    return;
                }
            }
            else
            {
                if (!Vstrong_AutoWHAccessory.SentAdjust_Detail_Delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }

                if (!Gensong_AutoWHFabric.SentAdjust_Detail_Delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }
            #endregion

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Adjust set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'",
                Env.User.UserID,
                this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新 MdivisionPoDetail --
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
                           Qty = -m.Sum(w => w.Field<decimal>("qtyafter") - w.Field<decimal>("qtybefore")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(32, null, false));
            #endregion
            #region 更新庫存數量  ftyinventory
            DataTable dtio = (DataTable)this.detailgridbs.DataSource;
            dtio.Columns.Add("Qty", typeof(decimal));
            foreach (DataRow dtr in dtio.Rows)
            {
                dtr["Qty"] = -((decimal)dtr["QtyAfter"] - (decimal)dtr["QtyBefore"]);
            }

            sqlupd2_FIO.Append(Prgs.UpdateFtyInventory_IO(8, null, false));
            #endregion 更新庫存數量  ftyinventory

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out DataTable resulttb, "#TmpSource")))
                    {
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        dtio, string.Empty, sqlupd2_FIO.ToString(), out resulttb, "#TmpSource")))
                    {
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        return;
                    }

                    this.FtyBarcodeData(false);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    result = new DualResult(false, string.Join("Commit transaction error.", Environment.NewLine, ex));
                }
            }

            if (result)
            {
                MyUtility.Msg.InfoBox("UnConfirmed successful");
            }
            else
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select a.id,a.PoId,a.Seq1,a.Seq2
    ,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
    ,p1.FabricType
    ,p1.stockunit
    ,a.Roll
    ,a.Dyelot
    ,a.QtyAfter
    ,a.QtyBefore
    ,isnull(a.QtyAfter,0.00) - isnull(a.QtyBefore,0.00) adjustqty
    ,a.ReasonId
    ,(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND ID= A.ReasonId) reason_nm
    ,a.StockType
    ,a.ukey
    ,a.ftyinventoryukey
    ,[location] = Getlocation.Value 
    ,[ColorID] = ColorID.Value
    ,[Description] = dbo.getmtldesc(p1.id,p1.seq1,p1.seq2,2,0)
from dbo.Adjust_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
outer apply (
    select   [Value] = stuff((	select ',' + MtlLocationID
									from (	
										select d.MtlLocationID	
										from dbo.FtyInventory_Detail d WITH (NOLOCK) 
										where	ukey =   fi.ukey
												and d.MtlLocationID != ''
												and d.MtlLocationID is not null) t
									for xml path('')) 
								, 1, 1, '')
) as Getlocation
outer apply (
	select	[Value] = stuff((select '/' + m.ColorID 
			   from dbo.Color as c 
			   LEFT join dbo.Color_multiple as m on m.ID = c.ID 
				  								    and m.BrandID = c.BrandId 
			   where c.ID = p1.ColorID and c.BrandId =  p1.BrandId 
			   order by m.Seqno 
			   for xml path('') ) 
			 , 1, 1, '')  
) as ColorID
Where a.id = '{0}'
", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void Button9_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P35_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
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
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P35", this);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@MDivision", this.CurrentMaintain["MDivisionID"]),
                new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()),
            };
            DualResult result = DBProxy.Current.Select(string.Empty, @"select NameEn from MDivision where id = @MDivision", pars, out DataTable dt);

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
	, [DESC] =IIF((ad.ID = lag(ad.ID,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll) 
			    AND(ad.seq1 = lag(ad.seq1,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll))
			    AND(ad.seq2 = lag(ad.seq2,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll))) 
			    ,''
                ,dbo.getMtlDesc(ad.poid, ad.seq1, ad.seq2, 2, 0))
	, [Location] = dbo.Getlocation(fi.ukey)
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
                    Location = row1["Location"].ToString().Trim(),
                    StockUnit = row1["StockUnit"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    DYELOT = row1["Dyelot"].ToString().Trim(),
                    QTY = row1["Qty"].ToString().Trim(),
                    TotalQTY = row1["TotalQTY"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
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

            return base.ClickPrint();
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = $@"
select
[Barcode1] = f.Barcode
,[OriBarcode] = fbOri.Barcode
,[balanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[NewBarcode] = isnull(fbOri.Barcode,f.Barcode)
,i2.Id,i2.POID,i2.Seq1,i2.Seq2,i2.StockType,i2.Roll,i2.Dyelot
from Production.dbo.Adjust_Detail i2
inner join Production.dbo.Adjust i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.POID
and f.Seq1 = i2.Seq1 and f.Seq2 = i2.Seq2
and f.Roll = i2.Roll and f.Dyelot = i2.Dyelot
and f.StockType = i2.StockType
outer apply(
	select *
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = i2.ID
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
            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;
            DataTable resulttb;
            if (data_Fty_Barcode.Count >= 1)
            {
                if (isConfirmed)
                {
                    // 更新Ftyinventory_Barcode 第二層
                    upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(71, null, true);

                    // 若Balance = 0 清空Ftyinventory.Barcode
                    upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(70, null, false);

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
                else
                {
                    // 更新Ftyinventory_Barcode 第二層補回到第一層
                    upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(72, null, true);
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V1, out resulttb, "#TmpSource")))
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
            }
        }
    }
}