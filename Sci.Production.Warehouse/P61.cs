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
    public partial class P61 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P61(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("Type='H' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            //
        }


        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "H";
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
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string CDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string MDivisionID = CurrentMaintain["MDivisionID"].ToString();
            string Remark = row["Remark"].ToString().Trim();

            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CDate", CDate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("MDivisionID", MDivisionID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            #endregion

            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;

            string sqlcmd = string.Format(@"select a.PoId,a.scirefno
                                                ,dbo.getItemDesc('PM1',a.scirefno) as [description]
                                                ,a.sizespec
                                                ,a.Qty
                                            from dbo.Issue_Summary as a 
                                            Where a.id = '{0}'", id, Sci.Env.User.Keyword);

            result = DBProxy.Current.Select("", sqlcmd, out dtDetail);
            if (!result) { this.ShowErr(sqlcmd, result); }
            
            // 傳 list 資料            
            List<P61_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P61_PrintData()
                {
                    POID = row1["PoId"].ToString(),
                    SCIREFNO = row1["scirefno"].ToString(),
                    DESCRIPTION = row1["description"].ToString(),
                    SIZESPEC = row1["sizespec"].ToString(),
                    QTY = row1["Qty"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            Type ReportResourceNamespace = typeof(P61_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P61_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
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

            

            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["scirefno"]) )
                {
                    warningmsg.Append(string.Format(@"SP#: {0} refno#: {1} can't be empty"
                        , row["poid"], row["scirefno"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} refno#: {1} Issue Qty can't be empty"
                        , row["poid"], row["scirefno"]) + Environment.NewLine);
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IO", "Issue", (DateTime)CurrentMaintain["Issuedate"]);
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
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("scirefno", header: "Refno", width: Widths.AnsiChars(26), iseditingreadonly: true)  //1
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("sizespec", header: "Thread Color#", iseditingreadonly: true)    //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
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

            
            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.scirefno,d.sizespec,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Summary d left join localinventory f
on f.MDivisionID = '{1}' and f.OrderID = d.Poid and f.Refno = d.SCIRefno and f.ThreadColorID = d.sizespec
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'"
                , CurrentMaintain["id"],CurrentMaintain["mdivisionid"]);
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
                        ids += string.Format("SP#: {0} Ref#: {1} color#: {2}'s balance: {3} is less than issue qty: {4}" + Environment.NewLine
                            , tmp["poid"], tmp["scirefno"], tmp["sizespec"], tmp["balanceqty"], tmp["qty"]);
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

            #region 更新庫存數量  LocalInventory

            sqlupd2.Append(string.Format(@"update LocalInventory 
set OutQty = OutQty + S.Qty
from LocalInventory l
inner join Issue_Summary s on s.Poid = l.OrderID 
and s.SCIRefno = l.Refno 
and s.SizeSpec = l.ThreadColorID 
and l.MDivisionID = '{0}'
and s.Id = '{1}';",CurrentMaintain["mdivisionid"],CurrentMaintain["id"]));


            #endregion 更新庫存數量  LocalInventory

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

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.scirefno,d.sizespec,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Summary d left join localinventory f
on f.MDivisionID = '{1}' and f.OrderID = d.Poid and f.Refno = d.SCIRefno and f.ThreadColorID = d.SizeSpec
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'"
                , CurrentMaintain["id"], CurrentMaintain["mdivisionid"]);
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
                        ids += string.Format("SP#: {0} Ref#: {1} color#: {2}'s balance: {3} is less than issue qty: {4}" + Environment.NewLine
                            , tmp["poid"], tmp["scirefno"], tmp["sizespec"], tmp["balanceqty"], tmp["qty"]);
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

            #region 更新庫存數量  LocalInventory

            sqlupd2.Append(string.Format(@"update LocalInventory 
set OutQty = OutQty - S.Qty
from LocalInventory l
inner join Issue_Summary s on s.Poid = l.OrderID 
and s.SCIRefno = l.Refno 
and s.SizeSpec = l.ThreadColorID 
and l.MDivisionID = '{0}'
and s.Id = '{1}';", CurrentMaintain["mdivisionid"], CurrentMaintain["id"]));


            #endregion 更新庫存數量  LocalInventory

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
            this.DetailSelectCommand = string.Format(@"select a.id,a.PoId,a.scirefno
,dbo.getItemDesc('{1}',a.scirefno) as [description]
,a.sizespec
,a.Qty
,a.ukey
from dbo.Issue_Summary as a 
Where a.id = '{0}'", masterID,Sci.Env.User.Keyword);
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
            var frm = new Sci.Production.Warehouse.P61_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
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