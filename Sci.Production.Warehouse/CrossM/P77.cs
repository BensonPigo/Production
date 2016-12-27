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
using Sci.Production.Warehouse.CrossM;
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    public partial class P77 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P77(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("Type='G' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            //
        }

        public P77(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='G' and id='{0}'", transID);
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
            CurrentMaintain["Type"] = "G";
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

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox3.Focus();
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty"
                        , row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "XF", "Issue", (DateTime)CurrentMaintain["Issuedate"]);
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
            Ict.Win.DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            ns.CellFormatting = (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                switch (dr["StockType"].ToString())
                {
                    case "B":
                        e.Value = "Bulk";
                        break;
                    case "I":
                        e.Value = "Inventory";
                        break;
                    case "O":
                        e.Value = "Obsolete";
                        break;
                }
            };
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("stocktype", header: "Stock Type", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ns)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("stockunit", header: "Unit", iseditingreadonly: true)
            .Numeric("qty", header: "Return Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)
            .Text("Location", header: "Bulk Location", iseditingreadonly: true)
            ;     //
            #endregion 欄位設定
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_B = "";
            string sqlupd2_FIO = "";

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d inner join FtyInventory f
on --d.ftyinventoryukey = f.ukey
d.MDivisionID = f.MDivisionID
and d.POID = f.POID
and d.Seq1 = f.Seq1
and d.Seq2 = f.Seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d left join FtyInventory f
on --d.ftyinventoryukey = f.ukey
d.MDivisionID = f.MDivisionID
and d.POID = f.POID
and d.Seq1 = f.Seq1
and d.Seq2 = f.Seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);

            var bsfio = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = b.Field<string>("mdivisionid"),
                             poid = b.Field<string>("poid"),
                             seq1 = b.Field<string>("seq1"),
                             seq2 = b.Field<string>("seq2"),
                             stocktype = b.Field<string>("stocktype"),
                             qty = b.Field<decimal>("qty"),
                             location = b.Field<string>("location"),
                             roll = b.Field<string>("roll"),
                             dyelot = b.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    //if (!(result = MyUtility.Tool.ProcessWithDatatable
                    //     ((DataTable)detailgridbs.DataSource, "", sqlupd2_FIO.ToString(), out resulttb, "#TmpSource")))
                    //{
                    //    _transactionscope.Dispose();
                    //    ShowErr(result);
                    //    return;
                    //}
                    //Console.WriteLine("TTTTTTTTTTT" + sqlupd2.ToString());
                    //if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    //{
                    //    _transactionscope.Dispose();
                    //    ShowErr(sqlupd2.ToString(), result2);
                    //    return;
                    //}
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

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            string sqlupd2_B = "";
            string sqlupd2_FIO = "";

            #region 603: WAREHOUSE_P77 。若P78已經收料了，則不能unconfirm。
            string P78_status = MyUtility.GetValue.Lookup(string.Format(@"
select status from RequestCrossM 
where ReferenceID='{0}'", CurrentMaintain["id"]));
            if (P78_status.ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This request id already confirmed in P78 , can't unconfirm.", "Warning");
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d inner join FtyInventory f
on --d.ftyinventoryukey = f.ukey
d.MDivisionID = f.MDivisionID
and d.POID = f.POID
and d.Seq1 = f.Seq1
and d.Seq2 = f.Seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d left join FtyInventory f
on --d.ftyinventoryukey = f.ukey
d.MDivisionID = f.MDivisionID
and d.POID = f.POID
and d.Seq1 = f.Seq1
and d.Seq2 = f.Seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();

            if (bs1.Count > 0)
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);

            var bsfio = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = b.Field<string>("mdivisionid"),
                             poid = b.Field<string>("poid"),
                             seq1 = b.Field<string>("seq1"),
                             seq2 = b.Field<string>("seq2"),
                             stocktype = b.Field<string>("stocktype"),
                             qty = - (b.Field<decimal>("qty")),
                             location = b.Field<string>("location"),
                             roll = b.Field<string>("roll"),
                             dyelot = b.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    //if (!(result = MyUtility.Tool.ProcessWithDatatable
                    //     ((DataTable)detailgridbs.DataSource, "", sqlupd2_FIO.ToString(), out resulttb, "#TmpSource")))
                    //{
                    //    _transactionscope.Dispose();
                    //    ShowErr(result);
                    //    return;
                    //}
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
            this.DetailSelectCommand = string.Format(@"select a.id,a.MDivisionID,a.PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.Roll
,a.Dyelot
,a.Qty
,a.StockType
,(select t.MtlLocationID+',' from (select mtllocationid from dbo.ftyinventory_detail fd where fd.Ukey = a.FtyInventoryUkey) t 
	for xml path('')) location
,a.ukey
from dbo.issue_detail as a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
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
            if (MyUtility.Check.Empty(CurrentMaintain["cutplanid"]))
            {
                MyUtility.Msg.WarningBox("< Request Id >  can't be empty!!");
                txtRequestID.Focus();
                return;
            }
            var frm = new Sci.Production.Warehouse.P77_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
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
            string borrow = row["cutplanid"].ToString();

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("borrow", borrow));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", issuedate));

            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            string sqlcmd = @"
            select issd.POID,
	               issd.Seq1+'-'+issd.Seq2 as seq,
	               issd.roll,
	               issd.Dyelot,
	               issd.StockType,
	               dbo.getMtlDesc(issd.poid,issd.seq1,issd.seq2,2,iif(psd.scirefno = lag(psd.scirefno,1,'') over (order by psd.refno,psd.seq1,psd.seq2),1,0)) [desc],
	               psd.stockunit,
	               issd.qty,
	               dbo.Getlocation(issd.FtyInventoryUkey) [location],
	               [Total]=sum(issd.Qty) OVER (PARTITION BY issd.POID ,issd.seq1,issd.seq2 ) 
            from Issue_Detail issd
            left join PO_Supp_Detail psd on psd.ID = issd.PoId and psd.seq1 = issd.SEQ1 and psd.SEQ2 = issd.seq2
            WHERE issd.ID= @ID";
            DualResult res;
            res = DBProxy.Current.Select("", sqlcmd, pars, out dt);
            if (!res)
            {
                this.ShowErr(res);
                return res;
            }

            // 傳 list 資料            
            List<P77_PrintData> data = dt.AsEnumerable()
                .Select(row1 => new P77_PrintData()
                {
                    POID = row1["POID"].ToString(),
                    SEQ = row1["seq"].ToString(),
                    Roll = row1["roll"].ToString(),
                    Dyelot = row1["Dyelot"].ToString(),
                    StockType = row1["StockType"].ToString(),
                    DESC = row1["desc"].ToString(),
                    Unit = row1["stockunit"].ToString(),
                    ReturnQTY = row1["Qty"].ToString(),
                    BulkLocation = row1["location"].ToString(),
                    TotalQTY = row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P77_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P77_Print.rdlc";

            IReportResource reportresource;
            if (!(res = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();
            return true;
        }
    }
}