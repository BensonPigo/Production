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
    public partial class P31 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P31(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("Type='A' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            //
        }

        public P31(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "A";
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
            string Estbackdate = row["Estbackdate"].ToString();
            string Remark = row["Remark"].ToString();
            string mdivisionid = row["mdivisionid"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtt;
            DualResult result1 = DBProxy.Current.Select("",
            @"select    
            b.name 
            from dbo.Borrowback a 
            inner join dbo.mdivision  b 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dtt);
            if (!result1) { this.ShowErr(result1); }
            string RptTitle = dtt.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Estbackdate", Estbackdate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("mdivisionid", mdivisionid));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            #endregion


            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result1 = DBProxy.Current.Select("",
            @"select a.FromPOID,a.FromSeq1+'-'+a.Fromseq2 as SEQ
	         ,dbo.getMtlDesc(a.FromPOID,a.FromSeq1,a.Fromseq2,2,iif(scirefno = lag(scirefno,1,'') over (order by b.refno, b.seq1,b.seq2),1,0))[DESC] 
			 ,unit = b.StockUnit
			 ,a.FromRoll,a.FromDyelot
		     ,a.Qty
		     ,dbo.Getlocation(a.FromFtyInventoryUkey)[From_Location]
			 ,a.ToLocation       
             from dbo.Subtransfer_detail a 
             INNER join dbo.PO_Supp_Detail b
             on 
             b.id=a.FromPOID and b.SEQ1=a.FromSeq1 and b.SEQ2=a.FromSeq2
             where a.id= @ID", pars, out dd);
            if (!result1) { this.ShowErr(result1); }

            // 傳 list 資料            
            List<P31_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P31_PrintData()
                {
                    FromSP = row1["FromSP"].ToString(),
                    TOSP= row1["TOSP"].ToString(),
                    DESC = row1["DESC"].ToString(),
                    StockType = row1["StockType"].ToString(),
                    Location = row1["Location"].ToString(),
                    unit = row1["unit"].ToString(),
                    Roll = row1["Roll"].ToString(),
                    Dyelot = row1["Dyelot"].ToString(),
                    QTY = row1["QTY"].ToString(),
                    Total = row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P31_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P31_Print.rdlc";

            IReportResource reportresource1;
            if (!(result1 = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource1)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource1;
            #endregion

            // 開啟 report view
            var frm1 = new Sci.Win.Subs.ReportView(report);
            frm1.MdiParent = MdiParent;
            frm1.Show();

            return true;
        
    


            Sci.Win.ReportDefinition rd = new Sci.Win.ReportDefinition();
            DualResult result;

            IReportResource reportresource;

            DataTable dt = (DataTable)gridbs.DataSource;
            DataTable dtmaster = new DataTable();

            if (!(result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(GetType()), GetType(), "P13Detail.rdlc", out reportresource)))
            {
                ShowErr(result);
            }
            else
            {
                rd.ReportResource = reportresource;
                rd.ReportDataSources.Add(new System.Collections.Generic.KeyValuePair<string, object>("DataSet1", dtmaster));
                // Assign subreport datasource, 如果不是 master-detail report 則以下的指令不必指定.
                rd.SubreportDataSource("RepDetail", "DetailData", (DataTable)this.detailgridbs.DataSource);
                using (var frm = new Sci.Win.Subs.ReportView(rd))
                {
                    frm.ShowDialog(this);
                }
            }

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

            if (MyUtility.Check.Empty(CurrentMaintain["estbackdate"]))
            {
                MyUtility.Msg.WarningBox("< Est. Return Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            #endregion 必輸檢查

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

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["toroll"]) || MyUtility.Check.Empty(row["todyelot"])))
                {
                    warningmsg.Append(string.Format(@"To SP#: {0} To Seq#: {1}-{2} To Roll#:{3} To Dyelot:{4} Roll and Dyelot can't be empty"
                        , row["topoid"], row["toseq1"], row["toseq2"], row["toroll"], row["todyelot"]) + Environment.NewLine);
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "MB", "BorrowBack", (DateTime)CurrentMaintain["Issuedate"]);
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
            CurrentDetailData["frommdivisionid"] = Sci.Env.User.Keyword;
            CurrentDetailData["tomdivisionid"] = Sci.Env.User.Keyword;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("fromseq", header: "From" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("fromroll", header: "From" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("fromdyelot", header: "From" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)    //5
            .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true)    //6
            .Text("topoid", header: "To" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //7
            .Text("toseq", header: "To" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //8
            .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6))  //9
            .Text("todyelot", header: "To" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //10
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //11
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)    //12
            ;     //
            #endregion 欄位設定
            this.detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
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

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.BorrowBack_Detail d inner join FtyInventory f
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
from dbo.BorrowBack_Detail d left join FtyInventory f
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

            #region -- 檢查目的Roll是否已存在資料 --

            sqlcmd = string.Format(@"Select d.ToPoid,d.ToSeq1,d.toseq2,d.ToRoll,d.ToDyelot,d.Qty
,f.InQty
from dbo.BorrowBack_Detail d inner join FtyInventory f
on d.tomdivisionid = f.mdivisionid
and d.ToPoid = f.PoId
and d.ToSeq1 = f.Seq1
and d.toseq2 = f.seq2
and d.ToStocktype = f.StockType
and d.ToRoll = f.Roll
and d.ToDyelot != f.dyelot
where f.InQty > 0 and toroll !='' and toroll is not null and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("Seq#: {1}-{2} Roll#: {3} exist in SP#: {0} but dyelot is not {4}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"],tmp["todyelot"]);
                    }
                    MyUtility.Msg.WarningBox(ids + Environment.NewLine + "Please change roll# !!", "Warning");
                    return;
                }
            }

            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update BorrowBack set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新mdivisionpodetail 借出數 --
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
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, item.qty, true, item.stocktype, item.mdivisionid));
                if (item.stocktype == "I") sqlupd2.Append(Prgs.UpdateMPoDetail(8, item.poid, item.seq1, item.seq2, item.qty, true, item.stocktype, item.mdivisionid));
            }
            #endregion
            #region -- 更新mdivisionPoDetail 借入數 --
            bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       mdivisionid = b.Field<string>("tomdivisionid"),
                       poid = b.Field<string>("topoid"),
                       seq1 = b.Field<string>("toseq1"),
                       seq2 = b.Field<string>("toseq2"),
                       stocktype = b.Field<string>("tostocktype")
                   } into m
                   select new
                   {
                       mdivisionid = m.First().Field<string>("tomdivisionid"),
                       poid = m.First().Field<string>("topoid"),
                       seq1 = m.First().Field<string>("toseq1"),
                       seq2 = m.First().Field<string>("toseq2"),
                       stocktype = m.First().Field<string>("tostocktype"),
                       qty = m.Sum(w => w.Field<decimal>("qty"))
                   }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(2, item.poid, item.seq1, item.seq2, item.qty, true, item.stocktype, item.mdivisionid));
            }

            #endregion 
            #region -- 更新庫存數量 ftyinventory --

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                // 借出扣數
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["frommdivisionid"].ToString(), item["frompoid"].ToString(), item["fromseq1"].ToString(), item["fromseq2"].ToString()
                    , (decimal)item["qty"], item["fromroll"].ToString(), item["fromdyelot"].ToString(), item["fromstocktype"].ToString(), true, item["location"].ToString()));
                // 借出加量
                sqlupd2.Append(Prgs.UpdateFtyInventory(2, item["tomdivisionid"].ToString(), item["topoid"].ToString(), item["toseq1"].ToString(), item["toseq2"].ToString()
                    , (decimal)item["qty"], item["toroll"].ToString(), item["todyelot"].ToString(), item["tostocktype"].ToString(), true, item["location"].ToString()));
            }
            sqlupd2.Append("drop table #tmp;" + Environment.NewLine);

            

            #endregion 更新庫存數量 ftyinventory

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
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.BorrowBack_Detail d inner join FtyInventory f
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
from dbo.BorrowBack_Detail d left join FtyInventory f
on d.toMdivisionid = f.Mdivisionid
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than borrowed qty: {5}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update BorrowBack set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新MdivisionPoDetail 借出數 --
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
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, item.qty, false, item.stocktype, item.mdivisionid));
                if (item.stocktype == "I") sqlupd2.Append(Prgs.UpdateMPoDetail(8, item.poid, item.seq1, item.seq2, item.qty, false, item.stocktype, item.mdivisionid));
            }
            #endregion
            #region -- 更新MdivisionPoDetail 借入數 --
            bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       mdivisionid = b.Field<string>("tomdivisionid"),
                       poid = b.Field<string>("topoid"),
                       seq1 = b.Field<string>("toseq1"),
                       seq2 = b.Field<string>("toseq2"),
                       stocktype = b.Field<string>("tostocktype")
                   } into m
                   select new
                   {
                       mdivisionid = m.First().Field<string>("tomdivisionid"),
                       poid = m.First().Field<string>("topoid"),
                       seq1 = m.First().Field<string>("toseq1"),
                       seq2 = m.First().Field<string>("toseq2"),
                       stocktype = m.First().Field<string>("tostocktype"),
                       qty = m.Sum(w => w.Field<decimal>("qty"))
                   }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(2, item.poid, item.seq1, item.seq2, item.qty, false, item.stocktype, item.mdivisionid));
            }
            #endregion
            #region -- 更新庫存數量  ftyinventory --

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                // 借出扣數
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["frommdivisionid"].ToString(), item["frompoid"].ToString(), item["fromseq1"].ToString(), item["fromseq2"].ToString(), (decimal)item["qty"]
                    , item["fromroll"].ToString(), item["fromdyelot"].ToString(), item["fromstocktype"].ToString(), false, item["location"].ToString()));
                // 借出加量
                sqlupd2.Append(Prgs.UpdateFtyInventory(2, item["tomdivisionid"].ToString(), item["topoid"].ToString(), item["toseq1"].ToString(), item["toseq2"].ToString(), (decimal)item["qty"]
                    , item["toroll"].ToString(), item["todyelot"].ToString(), item["tostocktype"].ToString(), false, item["location"].ToString()));
            }
            sqlupd2.Append("drop table #tmp;" + Environment.NewLine);
            
            #endregion 更新庫存數量  ftyinventory

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
            this.DetailSelectCommand = string.Format(@"select a.id,a.frommdivisionid,a.FromPoId,a.FromSeq1,a.FromSeq2
,left(a.FromSeq1+' ',3)+a.FromSeq2 as Fromseq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) as [description]
,a.FromRoll
,a.FromDyelot
,a.FromStocktype
,a.Qty
,a.ToMDivisionID
,a.ToPoid,a.ToSeq1,a.ToSeq2,left(a.ToSeq1+' ',3)+a.ToSeq2 as toseq
,a.ToRoll
,a.ToDyelot
,a.ToStocktype
,a.fromftyinventoryukey
,a.ukey
,(select mtllocationid+',' from (select mtllocationid from dbo.ftyinventory_detail fd 
where ukey= a.fromftyinventoryukey)t for xml path('')) location
from dbo.BorrowBack_detail a left join PO_Supp_Detail p1 on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
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
            var frm = new Sci.Production.Warehouse.P31_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P31_AccumulatedQty(CurrentMaintain);
            frm.ShowDialog(this);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("frompoid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }
    }
}