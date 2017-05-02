﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class P60 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P60(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            gridicon.RemoveClick += (s, e) =>
            {
                computeTotalQty();
            };
        }

        public P60(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format(" id='{0}'", transID);
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
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            //DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            //DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() == "CONFIRMED")
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

            if (MyUtility.Check.Empty(CurrentMaintain["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("< Local Supplier >  can't be empty!", "Warning");
                txtsubconLocalSupplier.Focus();
                return false;
            }

            #endregion 必輸檢查

            #region -- 刪除數量為零或refno為空的資料 --
            var select = ((DataTable)detailgridbs.DataSource).Select("qty = 0 or refno=''");
            foreach (DataRow dr in select)
            {
                dr.Delete();
            }
            #endregion

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "RL", "LocalReceiving", (DateTime)CurrentMaintain["IssueDate"]);
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
            txtTotal.Text = MyUtility.GetValue.Lookup(string.Format(@"
select sum(Qty) 
from LocalReceiving_Detail
where ID = '{0}'", CurrentMaintain["ID"].ToString()));
            #endregion Status Label
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region -- QTY 不可超過 On Road --

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = CurrentDetailData;
                    var v = detailgrid.Rows[e.RowIndex].Cells["onRoad"].Value;
                    string st =  v.Empty() ? "0" : v.ToString();
                    if (decimal.Parse(e.FormattedValue.ToString()) > decimal.Parse(st))
                    {
                        MyUtility.Msg.WarningBox("Qty can't be over on road qty!!");
                        dr["Qty"] = dr["onRoad"];
                    }
                    else
                    {
                        dr["Qty"] = e.FormattedValue;
                    }
                    dr.EndEdit();
                    computeTotalQty();
                }
            };
            #endregion 

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("localpoid", header: "Local PO", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ORDERid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("refno", header: "Refno", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("threadcolorid", header: "Color Shade", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("poqty", header: "PO Qty", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 6, iseditingreadonly: true)
            .Text("unitId", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
            .Numeric("onRoad", header: "On Road", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 6, iseditingreadonly: true)
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 6, minimum: -999999, settings: ns)
            // 2017/03/20 暫時 移除 Location
            //.Text("location", header: "Location", width: Widths.AnsiChars(20))
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            ;     //
            #endregion 欄位設定

            detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns["location"].DefaultCellStyle.BackColor = Color.Pink;
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

            #region 檢查負數庫存

            sqlcmd = string.Format(@"
Select   d.OrderId
         ,d.Refno
         ,d.ThreadColorID
         ,d.Qty
         ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
from dbo.LocalReceiving_Detail d WITH (NOLOCK) 
left join dbo.LocalInventory f WITH (NOLOCK) on d.OrderId = f.OrderID and d.Refno = f.Refno 
      and d.ThreadColorID = f.ThreadColorID
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
                        ids += string.Format("SP#: {0} Refno#: {1} ThreadColorId: {2}'s balance: {3} is less than receiving qty: {4}" + Environment.NewLine
                            , tmp["orderid"], tmp["refno"], tmp["threadColorId"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update LocalReceiving set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  Local Inventory

            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           orderid = b.Field<string>("orderid"),
                           refno = b.Field<string>("refno"),
                           threadcolorid = b.Field<string>("threadcolorid"),
                           unitid = b.Field<string>("unitid")
                       } into m
                       select new
                       {
                           orderid = m.First().Field<string>("orderid"),
                           refno = m.First().Field<string>("refno"),
                           threadcolorid = m.First().Field<string>("threadcolorid"),
                           unitid = m.First().Field<string>("unitid"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(string.Format(@"
merge into dbo.localinventory t
using (select '{1}','{2}','{3}',{0},'{4}') as s (orderid,refno,threadcolorid,qty,unitid)
on t.orderid = s.orderid and t.refno = s.refno and t.threadcolorid = s.threadcolorid 
when matched then
      update set inqty = inqty + s.qty
when not matched then
      insert (orderid, refno, threadcolorid, inqty, unitid) 
      values (s.orderid,s.refno,s.threadcolorid,s.qty,s.unitid);"
                    , item.qty, item.orderid, item.refno, item.threadcolorid,item.unitid));
            }

            #endregion 更新庫存數量  Local Inventory

            #region -- 更新local po inqty --
            sqlcmd = DoUpdateLocalPoInQty();
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd, result);
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

            #region 檢查負數庫存

            sqlcmd = string.Format(@"
Select  d.OrderId
        ,d.Refno
        ,d.ThreadColorID
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
from dbo.LocalReceiving_Detail d WITH (NOLOCK) 
left join dbo.LocalInventory f WITH (NOLOCK) on d.OrderId = f.OrderID and d.Refno = f.Refno 
      and d.ThreadColorID = f.ThreadColorID
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
                        ids += string.Format("SP#: {0} Refno#: {1} ThreadColorId: {2}'s balance: {3} is less than receiving qty: {4}" + Environment.NewLine
                            , tmp["orderid"], tmp["refno"], tmp["threadColorId"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update dbo.LocalReceiving set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  Local Inventory

            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           orderid = b.Field<string>("orderid"),
                           refno = b.Field<string>("refno"),
                           threadcolorid = b.Field<string>("threadcolorid"),
                           unitid = b.Field<string>("unitid")
                       } into m
                       select new
                       {
                           orderid = m.First().Field<string>("orderid"),
                           refno = m.First().Field<string>("refno"),
                           threadcolorid = m.First().Field<string>("threadcolorid"),
                           unitid = m.First().Field<string>("unitid"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(string.Format(@"
merge into dbo.localinventory t
using (select '{1}','{2}','{3}',{0},'{4}') as s (orderid,refno,threadcolorid,qty,unitid)
on t.orderid = s.orderid and t.refno = s.refno and t.threadcolorid = s.threadcolorid 
when matched then
      update set inqty = inqty - s.qty
when not matched then
      insert (orderid,refno,threadcolorid,inqty,unitid) 
      values (s.orderid,s.refno,s.threadcolorid,0 - s.qty,s.unitid);"
                    , item.qty, item.orderid, item.refno, item.threadcolorid,item.unitid));
            }

            #endregion 更新庫存數量  Local Inventory


            #region -- 更新local po inqty --
             sqlcmd = DoUpdateLocalPoInQty();
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd, result);
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        private string DoUpdateLocalPoInQty()
        {
            string sqlcmd = string.Format(@"
update dbo.LocalPO_Detail 
      set InQty = isnull((select sum(d.Qty) ttl_receiving 
                          from dbo.LocalReceiving c 
                          inner join dbo.LocalReceiving_Detail d on c.id = d.id  
                          where c.Status = 'Confirmed' and d.LocalPo_detailukey = LocalPO_Detail.Ukey 
                              and d.LocalPoId = LocalPO_Detail.Id)
                        , 0)
from LocalPO_Detail 
join (select LocalPoId,LocalPo_detailukey 
      from dbo.LocalReceiving a 
      inner join dbo.LocalReceiving_Detail b on b.id = a.id where a.id='{0}'
) s on LocalPO_Detail.id = s.LocalPoId and LocalPO_Detail.ukey = s.LocalPo_detailukey", CurrentMaintain["id"]);

            return sqlcmd;

        }
        
        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.*
,b.qty - b.inqty [onRoad]
,b.Qty poqty,b.Price
,dbo.getItemDesc(a.category,a.Refno) [description],b.UnitId
from dbo.LocalReceiving_Detail a WITH (NOLOCK) left join dbo.LocalPO_Detail b WITH (NOLOCK) 
on b.id = a.LocalPoId and b.Ukey = a.LocalPo_detailukey
Where a.id = '{0}' ", masterID);                        
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void btnDelete_Click(object sender, EventArgs e)
        {
            ((DataTable)detailgridbs.DataSource).Select("").ToList().ForEach(r => r.Delete());
            
        }

        //find
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("OrderId", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        //Batch Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("Please select local supplier first");
                txtsubconLocalSupplier.Focus();
                return;
            }
            var frm = new Sci.Production.Warehouse.P60_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            computeTotalQty();
            this.RenewData();
        }

        private void computeTotalQty()
        {
            txtTotal.Text = detailgrid.Rows.Cast<DataGridViewRow>().AsEnumerable().Sum(row => decimal.Parse(row.Cells["Qty"].Value.ToString())).ToString();         
        }

        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string Issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string Invoice = row["InvNo"].ToString();
            string Remarks = row["Remark"].ToString();
            string Rpttitle = Sci.Env.User.Factory;
           

            #region -- 撈表頭資料 --
            DataTable dt;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result = DBProxy.Current.Select("",
            @"select l.localsuppid + s.Abb as Supplier
            from Localreceiving l WITH (NOLOCK) 
            left join localsupp s WITH (NOLOCK) on l.LocalSuppID=s.id
            where l.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string Supplier = dt.Rows[0]["Supplier"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", Invoice));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remarks));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", Issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", Rpttitle));
        

            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"
            select ld.LocalPoId,
	               ld.OrderId,
	               ld.Refno,
	               IIF((ld.OrderId = lag(ld.OrderId,1,'')over (order by ld.LocalPoId,ld.OrderId,lpd.refno)
				       AND (ld.refno = lag(ld.refno,1,'')over (order by ld.LocalPoId,ld.OrderId,lpd.refno))
				       ),'',dbo.getItemDesc(ld.Category,ld.Refno))[desc],
	               lpd.qty [poqty],
	               lpd.UnitId,
	               lpd.Price,
	               ld.OnRoad,
	               ld.qty,
	               ld.Remark
            from LocalReceiving_Detail ld WITH (NOLOCK) 
            left join LocalPO_Detail lpd WITH (NOLOCK) on ld.LocalPoId=lpd.Id and ld.LocalPo_detailukey=lpd.Ukey
            where ld.ID= @ID";
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result) { this.ShowErr(sqlcmd, result); }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "");
                return false;
            }
          
            // 傳 list 資料            
            List<P60_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P60_PrintData()
                {
                    LocalPOID = row1["LocalPoId"].ToString(),
                    SPNo = row1["OrderId"].ToString(),
                    RefNo = row1["Refno"].ToString(),
                    Desc = row1["desc"].ToString(),
                    POQTY = row1["poqty"].ToString(),
                    Unit = row1["UnitId"].ToString(),
                    POPrice = row1["Price"].ToString(),
                    OnRoad = row1["OnRoad"].ToString(),
                    QTY = row1["qty"].ToString(),
                    Remark = row1["Remark"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P60_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P60_Print.rdlc";

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

            return true;
        }
    }
}