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
using System.Reflection;
using System.Data.SqlClient;
using Sci.Win;
using Sci.Production.Automation;
using System.Threading.Tasks;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P36 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;

            // MDivisionID 是 P36 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("Type='C' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P36(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='C' and id='{0}'", transID);
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

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "C";
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

        // save前檢查 & 取id

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["frompoid"], row["fromseq1"], row["fromseq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty",
                        row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["fromroll"]) || MyUtility.Check.Empty(row["fromdyelot"])))
                {
                    warningmsg.Append(string.Format(
                        @" SP#: {0}  Seq#: {1}-{2}  Roll#:{3}  Dyelot:{4} Roll and Dyelot can't be empty",
                        row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = string.Empty;
                    row["todyelot"] = string.Empty;
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword, "SubTransfer", (DateTime)this.CurrentMaintain["Issuedate"]);
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
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region -- Location 右鍵開窗 --

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(
                        this.CurrentDetailData["tostocktype"].ToString(),
                        this.CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", this.CurrentDetailData["tostocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["tolocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["tolocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) // 4
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 5
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
            .Text("ToLocation", header: "Location", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(30)) // 7
            ;
            #endregion 欄位設定
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
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

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;
            StringBuilder upd_MD_4T = new StringBuilder();
            StringBuilder upd_MD_16T = new StringBuilder();
            string upd_MD_8T = string.Empty;
            string upd_Fty_4T = string.Empty;
            string upd_Fty_2T = string.Empty;

            #region -- 檢查庫存項lock --
            bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            if (!mtlAutoLock)
            {
                sqlcmd = string.Format(
                    @"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.frompoid = f.POID and d.fromSeq1 = f.Seq1 and d.fromseq2 = f.seq2 and d.fromStockType = f.StockType and d.fromRoll = f.Roll and d.fromDyelot = f.Dyelot
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
                            ids += string.Format(
                                "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                                tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["Dyelot"]);
                        }

                        MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                        return;
                    }
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.frompoid = f.POID and d.fromSeq1 = f.Seq1 and d.fromseq2 = f.seq2 and d.fromStockType = f.StockType and d.fromRoll = f.Roll and d.fromDyelot = f.Dyelot
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than qty: {5}" + Environment.NewLine,
                            tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"update SubTransfer set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_4T16T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype"),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("frompoid"),
                           Seq1 = m.First().Field<string>("fromseq1"),
                           Seq2 = m.First().Field<string>("fromseq2"),
                           Stocktype = m.First().Field<string>("fromstocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail Inventory 數 --
            var data_MD_8T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       poid = b.Field<string>("topoid"),
                       seq1 = b.Field<string>("toseq1"),
                       seq2 = b.Field<string>("toseq2"),
                       stocktype = b.Field<string>("tostocktype"),
                   }
                    into m
                       select new Prgs_POSuppDetailData
                   {
                       Poid = m.First().Field<string>("topoid"),
                       Seq1 = m.First().Field<string>("toseq1"),
                       Seq2 = m.First().Field<string>("toseq2"),
                       Stocktype = m.First().Field<string>("tostocktype"),
                       Qty = m.Sum(w => w.Field<decimal>("qty")),
                       Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                   }).ToList();

            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4T = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("tolocation"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();

            var data_Fty_2T = (from b in this.DetailDatas
                               select new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype"),
                                   qty = b.Field<decimal>("qty"),
                                   location = b.Field<string>("ToLocation"),
                                   roll = b.Field<string>("toroll"),
                                   dyelot = b.Field<string>("todyelot"),
                               }).ToList();
            upd_Fty_4T = Prgs.UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, string.Empty, upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_4T.Append(Prgs.UpdateMPoDetail(4, null, true, sqlConn: sqlConn));
                    upd_MD_16T.Append(Prgs.UpdateMPoDetail(16, null, true, sqlConn: sqlConn));
                    upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T16T, string.Empty, upd_MD_4T.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T16T, string.Empty, upd_MD_16T.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    this.FtyBarcodeData(true);
                    this.SentToGensong_AutoWHFabric(true);
                    this.SentToGensong_AutoWH_ACC(true);
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
        }

        // Unconfirm

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO")
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
            StringBuilder upd_MD_4F = new StringBuilder();
            StringBuilder upd_MD_16F = new StringBuilder();
            string upd_MD_8T = string.Empty;
            string upd_Fty_4F = string.Empty;
            string upd_Fty_2F = string.Empty;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
and d.toDyelot = f.Dyelot
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
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
and d.toDyelot = f.Dyelot
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than qty: {5}" + Environment.NewLine,
                            tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"update SubTransfer set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_4F16F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype"),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("frompoid"),
                           Seq1 = m.First().Field<string>("fromseq1"),
                           Seq2 = m.First().Field<string>("fromseq2"),
                           Stocktype = m.First().Field<string>("fromstocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail Inventory數 --
            var data_MD_8T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       poid = b.Field<string>("topoid"),
                       seq1 = b.Field<string>("toseq1"),
                       seq2 = b.Field<string>("toseq2"),
                       stocktype = b.Field<string>("tostocktype"),
                   }
                    into m
                   select new Prgs_POSuppDetailData
                   {
                       Poid = m.First().Field<string>("topoid"),
                       Seq1 = m.First().Field<string>("toseq1"),
                       Seq2 = m.First().Field<string>("toseq2"),
                       Stocktype = m.First().Field<string>("tostocktype"),
                       Qty = -m.Sum(w => w.Field<decimal>("qty")),
                   }).ToList();

            #endregion
            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = -m.Field<decimal>("qty"),
                             location = m.Field<string>("tolocation"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();
            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = -m.Field<decimal>("qty"),
                               location = m.Field<string>("tolocation"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            upd_Fty_4F = Prgs.UpdateFtyInventory_IO(4, null, false);
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F, string.Empty, upd_Fty_4F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_4F.Append(Prgs.UpdateMPoDetail(4, null, false, sqlConn: sqlConn));
                    upd_MD_16F.Append(Prgs.UpdateMPoDetail(16, null, false, sqlConn: sqlConn));
                    upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F16F, string.Empty, upd_MD_4F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F16F, string.Empty, upd_MD_16F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    this.FtyBarcodeData(false);
                    this.SentToGensong_AutoWHFabric(false);
                    this.SentToGensong_AutoWH_ACC(false);
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
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select 
a.id
,a.FromFtyinventoryUkey
,a.FromPoId
,a.FromSeq1
,a.FromSeq2
,concat(Ltrim(Rtrim(a.FromSeq1)), ' ', a.FromSeq2) as FromSeq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) as [description]
,a.FromRoll
,a.FromDyelot
,a.FromStockType
,a.Qty
,a.ToPoId
,a.ToSeq1
,a.ToSeq2
,a.ToDyelot
,a.ToRoll
,a.ToStockType
,a.ToLocation
,a.ukey
from dbo.SubTransfer_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 
and p1.SEQ2 = a.FromSeq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P36_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P36_AccumulatedQty(this.CurrentMaintain);
            frm.P36 = this;
            frm.ShowDialog(this);
        }

        // Locate
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("frompoid", this.txtLocateForSP.Text.TrimEnd());
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool ClickPrint()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = this.CurrentMaintain["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(
                string.Empty,
                @"Select NameEN from MDivision where id = @MDivision", pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dt");
                return false;
            }

            string rptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            #endregion
            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            string cmdd =
                @"select a.FromPOID
                     ,a.FromSeq1+'-'+a.Fromseq2 as SEQ
	                 ,IIF((b.ID = lag(b.ID,1,'')over (order by b.ID,b.seq1,b.seq2) 
				      AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
				      AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
				      ,'',dbo.getMtlDesc(a.FromPOID,a.FromSeq1,a.Fromseq2,2,0))[DESC]
			         ,unit = b.StockUnit
			         ,a.FromRoll
                     ,a.FromDyelot
		             ,a.Qty [QTY]
			         ,a.ToLocation
                     ,[Total]=sum(a.Qty) OVER (PARTITION BY a.FromPOID ,a.FromSeq1,a.Fromseq2 )    
             from dbo.Subtransfer_detail a WITH (NOLOCK) 
             left join dbo.PO_Supp_Detail b WITH (NOLOCK) 
                on b.id=a.FromPOID and b.SEQ1=a.FromSeq1 and b.SEQ2=a.FromSeq2 and b.id=a.id
             where a.id= @ID";
            result = DBProxy.Current.Select(
                string.Empty, cmdd, pars, out dd);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dd == null || dd.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dd");
                return false;
            }

            // 傳 list 資料
            List<P36_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P36_PrintData()
                {
                    FromPOID = row1["FromPOID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["DESC"].ToString().Trim(),
                    Unit = row1["unit"].ToString().Trim(),
                    FromRoll = row1["FromRoll"].ToString().Trim(),
                    FromDyelot = row1["FromDyelot"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    ToLocation = row1["ToLocation"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC

            // DualResult result;
            Type reportResourceNamespace = typeof(P36_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P36_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();

            return true;
        }

        private void SentToGensong_AutoWHFabric(bool isConfirmed)
        {
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                dtMain.ImportRow(this.CurrentMaintain);
                Task.Run(() => new Gensong_AutoWHFabric().SentSubTransfer_DetailToGensongAutoWHFabric(dtMain, isConfirmed))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <summary>
        ///  AutoWH ACC WebAPI for Gensong
        /// </summary>
        private void SentToGensong_AutoWH_ACC(bool isConfirmed)
        {
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHAccessory.IsGensong_AutoWHAccessoryEnable)
            {
                DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                Task.Run(() => new Gensong_AutoWHAccessory().SentSubTransfer_DetailToGensongAutoWHAccessory(dtMain, isConfirmed))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = string.Empty;
            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;

            #region From
            sqlcmd = $@"
select f.Ukey,fb.TransactionID
,[Barcode1] = f.Barcode
,[Barcode2] = fb.Barcode
,[balanceQty] = f.InQty-f.OutQty+f.AdjustQty
,[NewBarcode] = ''
,[Poid] = i2.FromPOID
,[Seq1] = i2.FromSeq1
,[Seq2] = i2.FromSeq2
,[Roll] = i2.FromRoll
,[Dyelot] = i2.FromDyelot
,[StockType] = i2.FromStockType
from Production.dbo.SubTransfer_Detail i2
inner join Production.dbo.SubTransfer i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.FromPOID
    and f.Seq1 = i2.FromSeq1 and f.Seq2 = i2.FromSeq2
    and f.Roll = i2.FromRoll and f.Dyelot = i2.FromDyelot
    and f.StockType = i2.FromStockType
left join FtyInventory_Barcode fb on f.Ukey = fb.Ukey
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.FromPoid and seq1=i2.FromSeq1 and seq2=i2.FromSeq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                string strBarcode = MyUtility.Check.Empty(dr["Barcode2"]) ? dr["Barcode1"].ToString() : dr["Barcode2"].ToString();

                // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                if (!MyUtility.Check.Empty(dr["balanceQty"]))
                {
                    if (strBarcode.Contains("-"))
                    {
                        dr["NewBarcode"] = strBarcode.Substring(0, 13) + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                    }
                    else
                    {
                        dr["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                    }
                }
                else
                {
                    // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                    dr["NewBarcode"] = strBarcode;
                }
            }

            var data_From_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                        select new
                                        {
                                            TransactionID = this.CurrentMaintain["ID"].ToString(),
                                            poid = m.Field<string>("poid"),
                                            seq1 = m.Field<string>("seq1"),
                                            seq2 = m.Field<string>("seq2"),
                                            stocktype = m.Field<string>("stocktype"),
                                            roll = m.Field<string>("roll"),
                                            dyelot = m.Field<string>("dyelot"),
                                            Barcode = m.Field<string>("NewBarcode"),
                                        }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            upd_Fty_Barcode_V1 = isConfirmed ? Prgs.UpdateFtyInventory_IO(70, null, !isConfirmed) : Prgs.UpdateFtyInventory_IO(72, null, true);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resultFrom;
            if (data_From_FtyBarcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultFrom, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultFrom, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion

            #region To

            sqlcmd = $@"
select f.Ukey,fb.TransactionID
,[Barcode1] = f.Barcode
,[Barcode2] = fb.Barcode
,[balanceQty] = f.InQty-f.OutQty+f.AdjustQty
,[NewBarcode] = ''
,[Poid] = i2.ToPOID
,[Seq1] = i2.ToSeq1
,[Seq2] = i2.ToSeq2
,[Roll] = i2.ToRoll
,[Dyelot] = i2.ToDyelot
,[StockType] = i2.ToStockType
from Production.dbo.SubTransfer_Detail i2
inner join Production.dbo.SubTransfer i on i2.Id=i.Id 
left join FtyInventory f on f.POID = i2.ToPOID
    and f.Seq1 = i2.ToSeq1 and f.Seq2 = i2.ToSeq2
    and f.Roll = i2.ToRoll and f.Dyelot = i2.ToDyelot
    and f.StockType = i2.ToStockType
left join FtyInventory_Barcode fb on f.Ukey = fb.Ukey
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.ToPoid and seq1=i2.ToSeq1 and seq2=i2.ToSeq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                string strBarcode = MyUtility.Check.Empty(dr["Barcode2"]) ? dr["Barcode1"].ToString() : dr["Barcode2"].ToString();

                // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                if (!MyUtility.Check.Empty(dr["balanceQty"]))
                {
                    if (strBarcode.Contains("-"))
                    {
                        dr["NewBarcode"] = strBarcode.Substring(0, 13) + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                    }
                    else
                    {
                        dr["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                    }
                }
                else
                {
                    // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                    dr["NewBarcode"] = strBarcode;
                }
            }

            var data_To_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                      select new
                                      {
                                          TransactionID = this.CurrentMaintain["ID"].ToString(),
                                          poid = m.Field<string>("poid"),
                                          seq1 = m.Field<string>("seq1"),
                                          seq2 = m.Field<string>("seq2"),
                                          stocktype = m.Field<string>("stocktype"),
                                          roll = m.Field<string>("roll"),
                                          dyelot = m.Field<string>("dyelot"),
                                          Barcode = m.Field<string>("NewBarcode"),
                                      }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, isConfirmed);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resultTo;
            if (data_To_FtyBarcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultTo, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultTo, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion
        }
    }
}