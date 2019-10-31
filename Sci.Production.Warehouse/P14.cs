﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using System.Reflection;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P14 : Sci.Win.Tems.Input6
    {
        public P14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = $@"Type = 'H' and MDivisionID = '{Sci.Env.User.Keyword}'";
        }
        protected override void ClickNewAfter()
        {
            base.ClickEditAfter();
            this.CurrentMaintain["Type"] = "H";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.txtfactory.ReadOnly = true;
        }
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.");
                return false;
            }
            return base.ClickEditBefore();
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateIssueDate.ReadOnly = true;
            this.txtfactory.ReadOnly = !this.checkToSisterFty.Checked;
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("Issue Date can not empty!");
                this.dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("Order ID can not empty!");
                this.txtOrderID.Focus();
                return false;
            }

            if (this.checkToSisterFty.Checked && MyUtility.Check.Empty(this.CurrentMaintain["ToFactory"]))
            {
                MyUtility.Msg.WarningBox("sister factory can't empty!!");
                this.txtfactory.Focus();
                return false;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Seq"]) || MyUtility.Convert.GetDecimal(dr["Qty"]) == 0)
                {
                    dr.Delete();
                }
            }
            if (!this.checkToSisterFty.Checked)
            {
                List<string> msgpoid = new List<string>();
                int i = -1;
                bool f = false;
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (!f)
                        {
                            i++;
                        }
                        if (MyUtility.Check.Empty(dr["poid"]))
                        {
                            msgpoid.Add(MyUtility.Convert.GetString(dr["seq"]));
                            f = true;
                        }
                    }
                }
                if (msgpoid.Count > 0)
                {
                    MyUtility.Msg.WarningBox("Seq: " + string.Join(",", msgpoid) + ". To SP# can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells[6];
                    this.detailgrid.BeginEdit(true);
                    return false;
                }
            }

            // 將表身Issue_Detail.StockType 值都更新為 ‘B’
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["StockType"] = "B";
                }
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "AT", "Issue", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }
        protected override DualResult ClickSavePre()
        {
            DualResult resultBarcodeNo = Prgs.FillIssueDetailBarcodeNo(this.DetailDatas);

            if (!resultBarcodeNo)
            {
                return resultBarcodeNo;
            }
            return base.ClickSavePre();
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select distinct a.*
	, concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
	, dbo.getmtldesc(i.OrderID,a.seq1,a.seq2,2,0) as [description]
	, p1.SuppColor
	, p1.StockUnit
	, dbo.Getlocation(c.ukey) location
from dbo.issue_detail as a WITH (NOLOCK) 
inner join issue i WITH (NOLOCK) on i.id = a.id
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = i.OrderID and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join dbo.ftyinventory c WITH (NOLOCK) on c.poid = i.OrderID and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B' 
	and isnull(c.Roll,'') = isnull(a.Roll,'') and isnull(c.Dyelot,'')  = isnull(a.Dyelot,'')
Where a.id = '{0}'", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {

            #region Seq 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = $@"
select SEQ1, SEQ2, dbo.getmtldesc(id,seq1,seq2,2,0) as [description], SuppColor, StockUnit  
from PO_Supp_Detail
where ID = '{this.CurrentMaintain["OrderID"]}'
and Junk = 0
order by SEQ1, SEQ2
";
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "4,4,30,15,15", "");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    IList<DataRow> x = item.GetSelecteds();
                    CurrentDetailData["seq"] = MyUtility.Convert.GetString(x[0]["seq1"]) + " " + MyUtility.Convert.GetString(x[0]["seq2"]);
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    CurrentDetailData["SuppColor"] = x[0]["SuppColor"];
                    CurrentDetailData["Description"] = x[0]["Description"];
                    this.CurrentDetailData.EndEdit();
                }
            };
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (e.RowIndex == -1) return;
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    CurrentDetailData["seq"] = string.Empty;
                    CurrentDetailData["seq1"] = string.Empty;
                    CurrentDetailData["seq2"] = string.Empty;
                    CurrentDetailData["stockunit"] = string.Empty;
                    CurrentDetailData["SuppColor"] = string.Empty;
                    CurrentDetailData["Description"] = string.Empty;
                    return;
                }
                string[] seq = MyUtility.Convert.GetString(e.FormattedValue).Split(' ');
                string seq1 = seq[0];
                string seq2 = string.Empty;
                if (seq.Count() > 1)
                {
                    seq2 = seq[1];
                }

                string sqlcmd = $@"
select SEQ1, SEQ2, dbo.getmtldesc(id,seq1,seq2,2,0) as [description], SuppColor, StockUnit  
from PO_Supp_Detail
where ID = '{this.CurrentMaintain["OrderID"]}'
and Junk = 0 and seq1= '{seq1}' and isnull(seq2,'') = '{seq2}'
order by SEQ1, SEQ2
";
                DataRow dr;
                if (MyUtility.Check.Seek(sqlcmd, out dr))
                {
                    CurrentDetailData["seq"] = MyUtility.Convert.GetString(dr["seq1"]) + " " + MyUtility.Convert.GetString(dr["seq2"]);
                    CurrentDetailData["seq1"] = dr["seq1"];
                    CurrentDetailData["seq2"] = dr["seq2"];
                    CurrentDetailData["stockunit"] = dr["stockunit"];
                    CurrentDetailData["SuppColor"] = dr["SuppColor"];
                    CurrentDetailData["Description"] = dr["Description"];
                    this.CurrentDetailData.EndEdit();
                }
                else
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    CurrentDetailData["seq"] = string.Empty;
                    CurrentDetailData["seq1"] = string.Empty;
                    CurrentDetailData["seq2"] = string.Empty;
                    CurrentDetailData["stockunit"] = string.Empty;
                    CurrentDetailData["SuppColor"] = string.Empty;
                    CurrentDetailData["Description"] = string.Empty;
                    this.CurrentDetailData.EndEdit();
                    e.Cancel = true;
                    return;
                }
            };
            #endregion Seq 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings poid = new DataGridViewGeneratorTextColumnSettings();
            poid.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                if (!this.checkToSisterFty.Checked)
                {
                    string sqlcmd = $@"select 1 from orders with (nolock) where id = '{e.FormattedValue}'";
                    if (!MyUtility.Check.Seek(sqlcmd))
                    {
                        MyUtility.Msg.WarningBox("Data not found.");
                        CurrentDetailData["poid"] = string.Empty;
                    }
                    else
                    {
                        CurrentDetailData["poid"] = e.FormattedValue;
                    }
                    this.CurrentDetailData.EndEdit();
                }
            };

            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("SuppColor", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("stockunit", header: "Unit", iseditingreadonly: true)
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)
            .Text("Location", header: "Bulk Location", iseditingreadonly: true)
            .Text("poid", header: "To SP#", width: Widths.AnsiChars(13), settings: poid)
            ;
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
            StringBuilder sqlupd2_B = new StringBuilder();
            string sqlupd2_FIO = "";
            #region 檢查庫存項lock
            sqlcmd = string.Format(@"
Select  d.seq1,d.seq2
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join Issue i WITH (NOLOCK) on d.id = i.id
inner join FtyInventory f WITH (NOLOCK) on i.OrderID = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and f.StockType = 'B'  
	and isnull(d.Roll,'') = isnull(f.Roll,'') and isnull(d.Dyelot,'')  = isnull(f.Dyelot,'')
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
                        ids += string.Format("Seq#: {0}{1} is locked!!" + Environment.NewLine
                            , tmp["seq1"], tmp["seq2"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存
            sqlcmd = string.Format(@"
Select distinct d.seq1,d.seq2,d.qty
        ,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,
        d.ukey
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join Issue i WITH (NOLOCK) on d.id = i.id
Left join FtyInventory f WITH (NOLOCK) on i.OrderID = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and f.StockType = 'B'  
	and isnull(d.Roll,'') = isnull(f.Roll,'') and isnull(d.Dyelot,'')  = isnull(f.Dyelot,'')
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) 
and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("Seq#: {0}{1} balance: {2} is less than issue qty: {3}" + Environment.NewLine
                            ,tmp["seq1"], tmp["seq2"], tmp["balanceqty"], tmp["qty"]);
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
                           poid = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
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
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            var bsfio = ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         .GroupBy(m => new
                         {
                             poid = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         })
                         .Select(m => new
                         {
                             m.Key.poid,
                             m.Key.seq1,
                             m.Key.seq2,
                             m.Key.stocktype,
                             m.Key.location,
                             m.Key.roll,
                             m.Key.dyelot,
                             qty = m.Sum(w => w.Field<decimal>("qty"))
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
                    _transactionscope.Dispose();
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

        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlupd3 = "";
            DualResult result;

            StringBuilder sqlupd2_B = new StringBuilder();
            string sqlupd2_FIO = "";

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
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
                           qty = -(m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));

            var bsfio = ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         .GroupBy(m => new
                         {
                             poid = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         })
                         .Select(m => new
                         {
                             m.Key.poid,
                             m.Key.seq1,
                             m.Key.seq2,
                             m.Key.stocktype,
                             m.Key.location,
                             m.Key.roll,
                             m.Key.dyelot,
                             qty = -(m.Sum(w => w.Field<decimal>("qty")))
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

        }
        protected override bool ClickPrint()
        {
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string OrderID = row["OrderID"].ToString();
            string labtofty = string.Empty;
            string tofty = string.Empty;
            if (MyUtility.Convert.GetBool(row["ToSisterFty"]))
            {
                labtofty = "To sister factory:";
                tofty = MyUtility.Convert.GetString(row["ToFactory"]);
            }

            string CDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("", @"
select NameEn
from MDivision
where id = @MDivision", pars, out dt);

            if (!result) { this.ShowErr(result); }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dt");
                return false;
            }
            
            string RptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CDate", CDate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("OrderID", OrderID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("labTosisterfactory", labtofty));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tosisterfactory", tofty));

            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"
select distinct
	seq=concat(Ltrim(Rtrim(a.seq1)), '-', a.Seq2)
	,i.OrderID
	,a.seq1,a.seq2
	, dbo.Getlocation(f.ukey) location
	, p1.StockUnit
	, a.Qty
	, a.POID
    ,a.ukey
into #tmp
from dbo.issue_detail as a WITH (NOLOCK) 
inner join issue i WITH (NOLOCK) on i.id = a.id
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = i.OrderID and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
inner join FtyInventory f WITH (NOLOCK) on i.OrderID = f.poid and a.seq1 = f.seq1 and a.seq2 = f.seq2 and f.StockType = 'B'  
	and isnull(a.Roll,'') = isnull(f.Roll,'') and isnull(a.Dyelot,'')  = isnull(f.Dyelot,'')
Where a.id = @ID
order by SEQ,POID

select
	seq
	, IIF((OrderID = lag(OrderID,1,'')over (order by OrderID,seq1,seq2) 
			AND(seq1 = lag(Seq1,1,'')over (order by OrderID,seq1,seq2))
			AND(seq2 = lag(seq2,1,'')over (order by OrderID,seq1,seq2))) 
			,'',dbo.getMtlDesc(OrderID,seq1,seq2,2,0))[desc]
	, location
	, StockUnit
	, Qty
	, POID
	,[Total]=sum(Qty) OVER (PARTITION BY seq1,seq2 )
from #tmp
drop table #tmp
";
            result = DBProxy.Current.Select(null, sqlcmd, pars, out dtDetail);

            if (!result)  this.ShowErr(result); 

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtDetail");
                return false;
            }

            // 傳 list 資料            
            List<P14_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P14_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["desc"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                    StockUnit = row1["StockUnit"].ToString().Trim(),
                    QTY = row1["Qty"].ToString().Trim(),
                    TotalQTY = row1["Total"].ToString().Trim()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P14_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P14_Print.rdlc";

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
        private void CheckToSisterFty_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.txtfactory.ReadOnly = !this.checkToSisterFty.Checked;
                this.CurrentMaintain["ToSisterFty"] = this.checkToSisterFty.Checked;
                this.CurrentMaintain.EndEdit();
                if (!this.checkToSisterFty.Checked)
                {
                    this.CurrentMaintain["ToFactory"] = string.Empty;
                    this.CurrentMaintain.EndEdit();

                    List<string> msg = new List<string>();
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        string sqlcmd = $@"select 1 from orders with (nolock) where id = '{dr["poid"]}'";
                        if (!MyUtility.Check.Empty(dr["poid"]) && !MyUtility.Check.Seek(sqlcmd))
                        {
                            msg.Add(MyUtility.Convert.GetString(dr["poid"]));
                            dr["poid"] = string.Empty;
                        }
                        dr.EndEdit();
                    }
                    if (msg.Count > 0)
                    {
                        MyUtility.Msg.WarningBox($"To SP# data not found.\r\n" + string.Join(",", msg));
                    }
                }
            }
        }
        private void TxtOrderID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = $@"
select ID
from orders o with(nolock)
where Category ='A'
and o.FtyGroup = '{Sci.Env.User.Factory}'
";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "16", this.txtOrderID.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.txtOrderID.Text = item.GetSelectedString();
        }
        private void TxtOrderID_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string oldvalue = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]);
            if (oldvalue != this.txtOrderID.Text && !MyUtility.Check.Empty(this.txtOrderID.Text))
            {
                string sqlcmd = $@"
select ID
from orders o with(nolock)
where Category ='A'
and o.FtyGroup = '{Sci.Env.User.Factory}'
and ID = '{this.txtOrderID.Text}'
";
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox("Can't found the data");
                    this.txtOrderID.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
                
                this.CurrentMaintain["OrderID"] = this.txtOrderID.Text;
                this.CurrentMaintain.EndEdit();
            }
        }
        private void Txtfactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = $@"
select ID from SCIFty where junk = 0 and Type in ('B','S') and CountryID = (select top 1 CountryID from Factory)
except
select ID from Factory
";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "16", this.txtfactory.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.txtfactory.Text = item.GetSelectedString();
        }
        private void Txtfactory_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string oldvalue = MyUtility.Convert.GetString(this.CurrentMaintain["ToFactory"]);
            if (oldvalue != this.txtfactory.Text)
            {
                string sqlcmd = $@"
select 1
from(
    select ID from SCIFty where junk = 0 and Type in ('B','S') and CountryID = (select top 1 CountryID from Factory)
    except
    select ID from Factory
)x
where x.ID = '{this.txtfactory.Text}'
";
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($"{this.txtfactory.Text} is not found");
                    this.txtfactory.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }

                this.CurrentMaintain["ToFactory"] = this.txtfactory.Text;
                this.CurrentMaintain.EndEdit();
            }
        }
    }
}
