using Ict;
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

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P35 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

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
                if (this.EditMode && e.Button == MouseButtons.Right)
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
            .Text("reasonid", header: "Reason ID", settings: ts) // 10
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
                this.SentToVstrong_AutoWH_ACC(true);
                this.SentToGensong_AutoWHFabric(true);
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
                this.SentToVstrong_AutoWH_ACC(false);
                this.SentToGensong_AutoWHFabric(true);
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

        /// <summary>
        ///  AutoWH ACC WebAPI for Vstrong
        /// </summary>
        private void SentToVstrong_AutoWH_ACC(bool isConfirmed)
        {
            // AutoWHFabric WebAPI for Vstrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                Task.Run(() => new Vstrong_AutoWHAccessory().SentAdjust_DetailToVstrongAutoWHAccessory(dtMain, isConfirmed))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void SentToGensong_AutoWHFabric(bool isConfirmed)
        {
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                Task.Run(() => new Gensong_AutoWHFabric().SentAdjust_DetailToGensongAutoWHAccessory(dtMain, isConfirmed))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}