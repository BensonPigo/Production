using Ict;
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
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    public partial class P36 : Sci.Win.Tems.Input6
    {
        public P36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;

            this.DefaultFilter = string.Format("Type='C' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

        }

        public P36(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='C' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "C";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
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

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["fromroll"]) || MyUtility.Check.Empty(row["fromdyelot"])))
                {
                    warningmsg.Append(string.Format(@" SP#: {0}  Seq#: {1}-{2}  Roll#:{3}  Dyelot:{4} Roll and Dyelot can't be empty"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = "";
                    row["todyelot"] = "";
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword, "SubTransfer", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

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
            #region -- Location 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(CurrentDetailData["tostocktype"].ToString()
                                                                                                 , CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            #endregion Location 右鍵開窗

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) //4
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)    //5
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
            .Text("ToLocation", header: "Location", settings: ts2, iseditingreadonly: true, width: Widths.AnsiChars(30))    //7
            ;     //
            #endregion 欄位設定
            this.detailgrid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;

        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            StringBuilder sqlupd2_B = new StringBuilder();
            string sqlupd2_A = "";
            String sqlupd2_FIO = "";
            String sqlupd2_FIO2 = "";

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.SubTransfer_Detail d inner join FtyInventory f
on d.fromftyinventoryukey = f.ukey
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.SubTransfer_Detail d left join FtyInventory f
on d.fromftyinventoryukey = f.ukey
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than qty: {5}" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update SubTransfer set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新mdivisionpodetail Scrap數 -- 
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("fromMdivisionid"),
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("fromMdivisionid"),
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = -m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(16, null, true));
            #endregion
            #region -- 更新mdivisionpodetail Inventory 數 --
            var bs2 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       mdivisionid = b.Field<string>("tomdivisionid"),
                       poid = b.Field<string>("topoid"),
                       seq1 = b.Field<string>("toseq1"),
                       seq2 = b.Field<string>("toseq2"),
                       stocktype = b.Field<string>("tostocktype")
                   } into m
                       select new Prgs_POSuppDetailData_A
                   {
                       mdivisionid = m.First().Field<string>("tomdivisionid"),
                       poid = m.First().Field<string>("topoid"),
                       seq1 = m.First().Field<string>("toseq1"),
                       seq2 = m.First().Field<string>("toseq2"),
                       stocktype = m.First().Field<string>("tostocktype"),
                       qty = m.Sum(w => w.Field<decimal>("qty")),
                       location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct())
                   }).ToList();
            sqlupd2_A = Prgs.UpdateMPoDetail_A(2, bs2, true);            
            #endregion 

            #region -- 更新庫存數量  ftyinventory --
            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = m.Field<string>("fromMdivisionid"),
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("tolocation"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();
            var bsfioto = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               mdivisionid = m.Field<string>("toMdivisionid"),
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = m.Field<decimal>("qty"),
                               location = m.Field<string>("tolocation"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            sqlupd2_FIO2 = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs2, "", sqlupd2_A, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfioto, "", sqlupd2_FIO2, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            StringBuilder sqlupd2_B = new StringBuilder();
            string sqlupd2_A = "";
            String sqlupd2_FIO = "";
            String sqlupd2_FIO2 = "";

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.SubTransfer_Detail d inner join FtyInventory f
on d.tomdivisionid = f.mdivisionid
and d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.SubTransfer_Detail d left join FtyInventory f
on d.tomdivisionid = f.mdivisionid
and d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than qty: {5}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update SubTransfer set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新mdivisionpodetail Scrap數 --
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("frommdivisionid"),
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("frommdivisionid"),
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = -m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(16, null, false));
            #endregion
            #region -- 更新mdivisionpodetail Inventory數 --
            var bs2 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       mdivisionid = b.Field<string>("tomdivisionid"),
                       poid = b.Field<string>("topoid"),
                       seq1 = b.Field<string>("toseq1"),
                       seq2 = b.Field<string>("toseq2"),
                       stocktype = b.Field<string>("tostocktype")
                   } into m
                   select new Prgs_POSuppDetailData_A
                   {
                       mdivisionid = m.First().Field<string>("tomdivisionid"),
                       poid = m.First().Field<string>("topoid"),
                       seq1 = m.First().Field<string>("toseq1"),
                       seq2 = m.First().Field<string>("toseq2"),
                       stocktype = m.First().Field<string>("tostocktype"),
                       qty = m.Sum(w => w.Field<decimal>("qty"))
                   }).ToList();
            sqlupd2_A = Prgs.UpdateMPoDetail_A(2, bs2, true);    
            #endregion
            #region -- 更新庫存數量  ftyinventory --
            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = m.Field<string>("fromMdivisionid"),
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("tolocation"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();
            var bsfioto = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               mdivisionid = m.Field<string>("toMdivisionid"),
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = m.Field<decimal>("qty"),
                               location = m.Field<string>("tolocation"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            sqlupd2_FIO2 = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs2, "", sqlupd2_A, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfioto, "", sqlupd2_FIO2, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select 
a.id
,a.FromFtyinventoryUkey
,a.FromMDivisionid
,a.FromPoId
,a.FromSeq1
,a.FromSeq2
,left(a.FromSeq1+' ',3)+a.FromSeq2 as FromSeq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) as [description]
,a.FromRoll
,a.FromDyelot
,a.FromStockType
,a.Qty
,a.ToMDivisionid
,a.ToPoId
,a.ToSeq1
,a.ToSeq2
,a.ToDyelot
,a.ToRoll
,a.ToStockType
,a.ToLocation
,a.ukey
from dbo.SubTransfer_Detail a 
left join PO_Supp_Detail p1 on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 
and p1.SEQ2 = a.FromSeq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());

        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P36_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated
        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P36_AccumulatedQty(CurrentMaintain);
            frm.ShowDialog(this);
        }

        // Locate
        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("frompoid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }


        protected override bool ClickPrint()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select b.name 
            from dbo.Subtransfer a 
            inner join dbo.mdivision  b 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string RptTitle = dt.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            #endregion


            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result = DBProxy.Current.Select("",
            @"select a.FromPOID
                     ,a.FromSeq1+'-'+a.Fromseq2 as SEQ
	                 ,[DESC]=dbo.getMtlDesc(a.FromPOID,a.FromSeq1,a.Fromseq2,2,iif(scirefno = lag(scirefno,1,'') over (order by b.refno, b.seq1,b.seq2),1,0))
			         ,unit = b.StockUnit
			         ,a.FromRoll
                     ,a.FromDyelot
		             ,a.Qty [QTY]
			         ,a.ToLocation
                     ,[Total]=sum(a.Qty) OVER (PARTITION BY a.FromPOID ,a.FromSeq1,a.Fromseq2 )    
             from dbo.Subtransfer_detail a 
             left join dbo.PO_Supp_Detail b
                on b.id=a.FromPOID and b.SEQ1=a.FromSeq1 and b.SEQ2=a.FromSeq2 and b.id=a.id
             where a.id= @ID", pars, out dd);
            if (!result) { this.ShowErr(result); }

            // 傳 list 資料            
            List<P36_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P36_PrintData()
                {
                    FromPOID = row1["FromPOID"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    DESC = row1["DESC"].ToString(),
                    unit = row1["unit"].ToString(),
                    FromRoll = row1["FromRoll"].ToString(),
                    FromDyelot = row1["FromDyelot"].ToString(),
                    QTY = row1["QTY"].ToString(),
                    ToLocation = row1["ToLocation"].ToString(),
                    Total = row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P36_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P36_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;

        }
    }
}