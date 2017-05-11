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
using Sci.Production.Warehouse.CrossM;
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    public partial class P76 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P76(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='D' and ToMDivisionID = '{0}' and (status = 'Sent' or status = 'Confirmed')", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            //
        }

        public P76(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {

            this.DefaultFilter = string.Format("Type='D' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["ToMDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
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
                dateIssueDate.Focus();
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
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Received Qty can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
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
            CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_Location;

            #region Location 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(CurrentDetailData["stocktype"].ToString(), CurrentDetailData["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["location"] = item.GetSelectedString();
                }
            };
            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"
SELECT  id 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}' 
        and junk != '1'
        and mdivisionid='{1}'", CurrentDetailData["stocktype"].ToString(), Sci.Env.User.Keyword);
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    CurrentDetailData["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "Bulk" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("seq", header: "Bulk" + Environment.NewLine + " Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
            .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(4))
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true, settings: ns).Get(out col_Qty)
            .Text("Location", header: "Location", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(30)).Get(out col_Location)
            ;     //
            #endregion 欄位設定
            
            col_Location.DefaultCellStyle.BackColor = Color.Pink;

        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return ;
            }

            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            string upd_MD_2T = "";
            string upd_Fty_2T = "";

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.RequestCrossM_Receive d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than receive qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update dbo.RequestCrossM set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量 mdivisionPoDetail & ftyinventory
            var data_MD_2T = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                       }).ToList();

            DataTable newDt = ((DataTable)detailgridbs.DataSource).Clone();
            foreach (DataRow dtr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                string[] dtrLocation = dtr["location"].ToString().Split(',');
                if (dtrLocation.Length == 1) {
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
                        newDr["location"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2T = (from b in newDt.AsEnumerable()
                         select new  
                         {
                             poid = b.Field<string>("poid"),
                             seq1 = b.Field<string>("seq1"),
                             seq2 = b.Field<string>("seq2"),
                             stocktype = b.Field<string>("stocktype"),
                             qty = b.Field<decimal>("qty"),
                             location = b.Field<string>("location"),
                             roll = b.Field<string>("roll"),
                             dyelot = b.Field<string>("dyelot"),
                         }).ToList();
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);

            #endregion 更新庫存數量 po_supp_detail & ftyinventory
            
            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (_transactionscope)
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, "", upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, "", upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion


                    //if (!(result = MyUtility.Tool.ProcessWithDatatable
                    //    ((DataTable)detailgridbs.DataSource, "", sqlupd2_FIO.ToString(), out resulttb, "#TmpSource")))
                    //{
                    //    _transactionscope.Dispose();
                    //    ShowErr(result);
                    //    return;
                    //}
                    // status
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
                finally
                {
                    _transactionscope.Dispose();
                    //_transactionscope = null;
                }
            }
            //this.RenewData();
            //this.OnDetailEntered();
            //this.EnsureToolbarExt();
        }

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
            string upd_MD_2F = "";
            string upd_Fty_2F = "";

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.RequestCrossM_Receive d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than received qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update RequestCrossM set status='Sent', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量 po_supp_detail & ftyinventory
            var data_MD_2F = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();            

            DataTable newDt = ((DataTable)detailgridbs.DataSource).Clone();
            foreach (DataRow dtr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                string[] dtrLocation = dtr["location"].ToString().Split(',');
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
                        newDr["location"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2F = (from b in newDt.AsEnumerable()
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
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量 po_supp_detail & ftyinventory

            
            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (_transactionscope)
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, "", upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, "", upd_MD_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion

                    //if (!(result = MyUtility.Tool.ProcessWithDatatable
                    //    ((DataTable)detailgridbs.DataSource, "", sqlupd2_FIO.ToString(), out resulttb, "#TmpSource")))
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
                    _transactionscope.Dispose();
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
            //this.RenewData();
            //this.OnDetailEntered();
            //this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select a.*
,concat(Ltrim(Rtrim(a.Seq1)), ' ', a.seq2) as seq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.PoId,a.Seq1,a.Seq2,2,0) as [description]
from dbo.RequestCrossM_Receive a  WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.Seq1 and p1.SEQ2 = a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void btnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());

        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P76_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.P76 = this;
            frm.ShowDialog(this);           
            this.RenewData();
        }
        protected override bool ClickPrint()
        {
            //DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string M = row["MDivisionID"].ToString();

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("M", M));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", issuedate));

            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            string sqlcmd = @"
            select  RC.POID,
		            RC.SEQ1+'-'+RC.Seq2 AS SEQ,
		            dbo.getMtlDesc(RC.poid,RC.seq1,RC.seq2,2,iif(psd.scirefno = lag(psd.scirefno,1,'') over (order by psd.refno,psd.seq1,psd.seq2),1,0)) [desc],
		            PSD.STOCKUNIT,
		            RC.ROLL,
		            RC.Dyelot,
                    RC.QTY,
		            RC.LOCATION,
	               [Total]=sum(RC.Qty) OVER (PARTITION BY RC.POID ,RC.seq1,RC.seq2 ) 
            from RequestCrossM_Receive RC WITH (NOLOCK) 
            left join PO_Supp_Detail PSD WITH (NOLOCK) on PSD.ID = RC.PoId and PSD.seq1 = RC.Seq1 and PSD.SEQ2 = RC.seq2
            WHERE RC.ID= @ID";
            DualResult res;
            res = DBProxy.Current.Select("", sqlcmd, pars, out dt);
            if (!res)
            {
                this.ShowErr(res);
                return res;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dt");
                return false;
            }
            
            // 傳 list 資料            
            List<P76_PrintData> data = dt.AsEnumerable()
                .Select(row1 => new P76_PrintData()
                {
                    SPNo = row1["POID"].ToString().Trim(),
                    BulkSeq = row1["SEQ"].ToString().Trim(),
                    Roll = row1["ROLL"].ToString().Trim(),
                    Dyelot = row1["Dyelot"].ToString().Trim(),
                    DESC = row1["desc"].ToString().Trim(),
                    StockUnit = row1["STOCKUNIT"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    Location = row1["LOCATION"].ToString().Trim(),
                    TotalQty = row1["Total"].ToString().Trim()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P76_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P76_Print.rdlc";

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