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

namespace Sci.Production.Warehouse
{
    public partial class P25 : Win.Tems.Input6
    {
        public P25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;

            // MDivisionID 是 Stored Procedures 寫入 => usp_WarehouseClose
            this.DefaultFilter = string.Format("Type='D' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.gridicon.Remove.Enabled = false;
            this.gridicon.Remove.Visible = false;
        }

        public P25(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='D' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.gridicon.Remove.Enabled = false;
            this.gridicon.Remove.Visible = false;
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "D";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
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

                if (row["fabrictype"].ToString().ToUpper() == "FABRIC" && (MyUtility.Check.Empty(row["fromroll"]) || MyUtility.Check.Empty(row["fromdyelot"])))
                {
                    warningmsg.Append(string.Format(
                        @" SP#: {0}  Seq#: {1}-{2}  Roll#:{3}  Dyelot:{4} Roll and Dyelot can't be empty",
                        row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "FABRIC")
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

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings locationSet = new DataGridViewGeneratorTextColumnSettings();
            locationSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 selectItem = Prgs.SelectLocation(this.CurrentDetailData["ToStockType"].ToString());
                    DialogResult result = selectItem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["ToLocation"] = selectItem.GetSelectedString();
                }
            };

            locationSet.CellValidating += (s, e) =>
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
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("fabrictype", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)
            .Text("ToCtnNo", header: "Ctn#", width: Widths.AnsiChars(8))
            .Text("fromlocation", header: "From Bulk Location", iseditingreadonly: true, width: Widths.AnsiChars(30))
            .Text("toLocation", header: "To Scrap Location", iseditingreadonly: false, width: Widths.AnsiChars(30), settings: locationSet)
            ;
            #endregion 欄位設定
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ToCtnNo"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["toLocation"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string upd_MD_4T = string.Empty;
            string upd_MD_16T = string.Empty;
            string upd_Fty_4T = string.Empty;
            string upd_Fty_2T = string.Empty;

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            #region -- 檢查庫存項lock --
            bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            if (!mtlAutoLock)
            {
                sqlcmd = string.Format(
                    @"
Select  d.frompoid
        ,d.fromseq1
        ,d.fromseq2
        ,d.fromRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
        ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK, INDEX(MdID_POSeq))
        on d.FromPOID = f.POID and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 AND D.FromStockType = F.StockType and d.FromDyelot = f.Dyelot
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
                @"
Select  d.frompoid
        ,d.fromseq1
        ,d.fromseq2
        ,d.fromRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
        ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK, INDEX(MdID_POSeq))
        on d.FromPOID = f.POID and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 AND D.FromStockType = F.StockType and d.FromDyelot = f.Dyelot
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot:{6}'s balance: {4} is less than qty: {5}" + Environment.NewLine,
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

            #region -- 更新mdivisionpodetail Bulk 數 --
            var data_MD_4T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       poid = b.Field<string>("frompoid"),
                       seq1 = b.Field<string>("fromseq1"),
                       seq2 = b.Field<string>("fromseq2"),
                       stocktype = b.Field<string>("fromstocktype"),
                   }
                    into m
                       select new Prgs_POSuppDetailData
                   {
                       poid = m.First().Field<string>("frompoid"),
                       seq1 = m.First().Field<string>("fromseq1"),
                       seq2 = m.First().Field<string>("fromseq2"),
                       stocktype = m.First().Field<string>("fromstocktype"),
                       qty = m.Sum(w => w.Field<decimal>("qty")),
                   }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_16T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("topoid"),
                           seq1 = b.Field<string>("toseq1"),
                           seq2 = b.Field<string>("toseq2"),
                           stocktype = b.Field<string>("tostocktype"),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("topoid"),
                           seq1 = m.First().Field<string>("toseq1"),
                           seq2 = m.First().Field<string>("toseq2"),
                           stocktype = m.First().Field<string>("tostocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
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

            DataTable newDt = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dtr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                string[] dtrLocation = dtr["ToLocation"].ToString().Split(',');
                dtrLocation = dtrLocation.Distinct().ToArray();

                if (dtrLocation.Length == 1)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["ToLocation"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2T = (from b in newDt.AsEnumerable()
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
                    upd_MD_4T = Prgs.UpdateMPoDetail(4, data_MD_4T, true, sqlConn: sqlConn);
                    upd_MD_16T = Prgs.UpdateMPoDetail(16, null, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T, string.Empty, upd_MD_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_16T, string.Empty, upd_MD_16T, out resulttb, "#TmpSource", conn: sqlConn)))
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
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            string upd_MD_4F = string.Empty;
            string upd_MD_16F = string.Empty;
            string upd_Fty_4F = string.Empty;
            string upd_Fty_2F = string.Empty;

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

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select  d.topoid
        ,d.toseq1
        ,d.toseq2
        ,d.toRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
        ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK, INDEX(MdID_POSeq))
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
                @"
Select  d.topoid
        ,d.toseq1
        ,d.toseq2
        ,d.toRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
        ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK, INDEX(MdID_POSeq))
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
            #region -- 更新mdivisionpodetail Bulk數 --
            var data_MD_4F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_16F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       poid = b.Field<string>("topoid"),
                       seq1 = b.Field<string>("toseq1"),
                       seq2 = b.Field<string>("toseq2"),
                       stocktype = b.Field<string>("tostocktype"),
                   }
                    into m
                   select new
                   {
                       poid = m.First().Field<string>("topoid"),
                       seq1 = m.First().Field<string>("toseq1"),
                       seq2 = m.First().Field<string>("toseq2"),
                       stocktype = m.First().Field<string>("tostocktype"),
                       qty = -m.Sum(w => w.Field<decimal>("qty")),
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
                    upd_MD_4F = Prgs.UpdateMPoDetail(4, data_MD_4F, false, sqlConn: sqlConn);
                    upd_MD_16F = Prgs.UpdateMPoDetail(16, null, false, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F, string.Empty, upd_MD_4F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_16F, string.Empty, upd_MD_16F, out resulttb, "#TmpSource", conn: sqlConn)))
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
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select 
    a.id
    ,a.FromFtyinventoryUkey
    ,a.FromPoId
    ,a.FromSeq1
    ,a.FromSeq2
    ,FromSeq = concat(Ltrim(Rtrim(a.FromSeq1)), ' ', a.FromSeq2) 
    ,FabricType = case p1.FabricType    
                    when 'F' then 'Fabric' 
                    when 'A' then 'Accessory' 
                    when 'O' then 'Other' 
                    else p1.FabricType 
                  end  
    ,p1.stockunit
    ,[description] = dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0)
    ,a.FromRoll
    ,a.FromDyelot
    ,a.FromStockType
    ,dbo.Getlocation(f.Ukey)  as Fromlocation
    ,a.ToCtnNo
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
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join FtyInventory f WITH (NOLOCK) on a.FromPOID=f.POID and a.FromSeq1=f.Seq1 and a.FromSeq2=f.Seq2 and a.FromRoll=f.Roll and a.FromDyelot=f.Dyelot and a.FromStockType=f.StockType
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // Locate
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = -1;

            if (!this.txtSeq.CheckSeq1Empty() && this.txtSeq.CheckSeq2Empty())
            {
                index = this.detailgridbs.Find("fromseq1", this.txtSeq.Seq1);
            }
            else if (!this.txtSeq.CheckEmpty())
            {
                index = this.detailgridbs.Find("fromseq", this.txtSeq.GetSeq());
            }

            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = row["Remark"].ToString();
            string cDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(
                string.Empty,
                @"select NameEn from MDivision where id = @MDivision", pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dt");
                return false;
            }

            string rptTitle = dt.Rows[0]["NameEn"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CDate", cDate));
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion
            #region -- 撈表身資料 --
            DataTable dtDetail;
            result = DBProxy.Current.Select(
                string.Empty,
                @"
select  frompoid
        ,SEQ = a.fromseq1+'-'+a.fromseq2
        ,a.FromRoll
        ,a.FromDyelot
        ,[Description] = IIF((b.ID = lag(b.ID,1,'')over (order by b.ID,b.seq1,b.seq2) AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2)) AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
                              ,''
                              ,dbo.getMtlDesc(a.FromPOID,a.FromSeq1,a.Fromseq2,2,0))
        ,b.StockUnit
        , case b.fabrictype
            WHEN 'F'THEN 'Fabric'
            WHEN 'A'THEN 'Accessory'
            WHEN 'O'THEN 'Other'
            ELSE b.FabricType
          end MtlType
        ,a.Qty
        ,[Location] = dbo.Getlocation(fi.ukey)
from dbo.Subtransfer_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.FromPOID and b.SEQ1=a.FromSeq1 and b.SEQ2=a.FromSeq2
left join dbo.FtyInventory FI on a.fromPoid = fi.poid and a.fromSeq1 = fi.seq1 and a.fromSeq2 = fi.seq2 and a.fromDyelot = fi.Dyelot
    and a.fromRoll = fi.roll and a.fromStocktype = fi.stocktype
where a.id= @ID", pars, out dtDetail);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dtDetail");
                return false;
            }

            // 傳 list 資料
            List<P25_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P25_PrintData()
                {
                    SP = row1["frompoid"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    Roll = row1["FromRoll"].ToString().Trim(),
                    DYELOT = row1["FromDyelot"].ToString().Trim(),
                    DESC = row1["Description"].ToString().Trim(),
                    Unit = row1["StockUnit"].ToString().Trim(),
                    Type = row1["MtlType"].ToString().Trim(),
                    ActQty = row1["QTY"].ToString().Trim(),
                    OLocation = row1["Location"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P25_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P25.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();

            return true;
        }
    }
}