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
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    public partial class P13 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("Type='D' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            //
        }

        public P13(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='D' and id='{0}'", transID);
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
            CurrentMaintain["Type"] = "D";
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

        //print
        protected override bool ClickPrint()
        {
            //DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string CDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select    
            b.name 
            from dbo.Issue  a WITH (NOLOCK) 
            inner join dbo.mdivision  b WITH (NOLOCK) 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dt");
                return false;
            }
            
            string RptTitle = dt.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CDate", CDate));

            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"
            select t.POID,
	               t.seq1+ '-' +t.seq2 as SEQ,
                   p.Scirefno,
	               p.seq1,
	               p.seq2,
	               IIF((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2) 
				     AND(p.seq1 = lag(p.seq1,1,'')over (order by p.ID,p.seq1,p.seq2))
				     AND(p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))) 
				     ,'',dbo.getMtlDesc(t.poid,t.seq1,t.seq2,2,0))[desc],
	               t.Roll,
	               t.Dyelot,
	               t.Qty,
	               p.StockUnit,
                   dbo.Getlocation(t.FtyInventoryUkey) [location],
	               [Total]=sum(t.Qty) OVER (PARTITION BY t.POID ,t.seq1,t.seq2 )            
            from dbo.Issue_Detail t WITH (NOLOCK) 
                   left join dbo.PO_Supp_Detail p WITH (NOLOCK) 
                   on p.id= t.poid and p.SEQ1 = t.Seq1 and p.seq2 = t.Seq2
            where t.id= @ID";
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result) { this.ShowErr(sqlcmd, result); }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtDetail");
                return false;
            }

            // 傳 list 資料            
            List<P13_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P13_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["desc"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                    StockUnit = row1["StockUnit"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    DYELOT = row1["Dyelot"].ToString().Trim(),
                    QTY = row1["Qty"].ToString().Trim(),
                    TotalQTY = row1["Total"].ToString().Trim()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P13_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P13_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();



          
            
            return base.ClickPrint();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
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

            if (MyUtility.Check.Empty(CurrentMaintain["Whsereasonid"]))
            {
                MyUtility.Msg.WarningBox("< Reason >  can't be empty!", "Warning");
                txtwhseReason1.TextBox1.Focus();
                return false;
            }
            else
            {
                if (CurrentMaintain["Whsereasonid"].ToString() == "00005" && MyUtility.Check.Empty(CurrentMaintain["remark"]))
                {
                    MyUtility.Msg.WarningBox("< Remark >  can't be empty!", "Warning");
                    editBox1.Focus();
                    return false;
                }
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "II", "Issue", (DateTime)CurrentMaintain["Issuedate"]);
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
            this.txtwhseReason1.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.txtwhseReason1.Type.ToString() + this.txtwhseReason1.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
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
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
            .Text("Location", header: "Bulk Location", iseditingreadonly: true)    //7
            .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
            ;     //
            #endregion 欄位設定
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
            string sqlupd2_FIO = "";
            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.ftyinventoryukey = f.ukey
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
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.ftyinventoryukey = f.ukey
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
                       select new Prgs_POSuppDetailData
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = m.Field<string>("mdivisionid"),
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb
                        , "#TmpSource")))
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
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            StringBuilder sqlupd2_B = new StringBuilder();
            string sqlupd2_FIO = "";

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.ftyinventoryukey = f.ukey
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
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.ftyinventoryukey = f.ukey
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
                       select new Prgs_POSuppDetailData
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));

            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = m.Field<string>("mdivisionid"),
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = - (m.Field<decimal>("qty")),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb
                        , "#TmpSource")))
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
            this.DetailSelectCommand = string.Format(@"select a.id,a.MDivisionID,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.Roll
,a.Dyelot
,a.Qty
,a.StockType
,Isnull(c.inqty-c.outqty + c.adjustqty,0.00) as balance
,stuff((select ',' + t.MtlLocationID from (select mtllocationid from dbo.ftyinventory_detail fd WITH (NOLOCK) where fd.Ukey = a.FtyInventoryUkey) t 
	for xml path('')), 1, 1, '') location
,a.ukey
from dbo.issue_detail as a WITH (NOLOCK) left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.poid and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B' and c.roll=a.roll and c.Dyelot=a.Dyelot
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            //((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => ((DataTable)detailgridbs.DataSource).Rows.Remove(r));
            // 不可以用 Remove()，因為datatable還沒有更新回資料庫中，被Remove的資料沒有真正刪除。
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P13_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P13_AccumulatedQty(CurrentMaintain);
            frm.P13 = this;
            frm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        private void btCutRef_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P10_CutRef(CurrentMaintain);
            frm.ShowDialog(this);
        }
       
    }
}