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
using Microsoft.Reporting.WinForms;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Production.Automation;
using System.Threading.Tasks;
using System.Threading;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P37 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P37(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer();
            this.viewer.Dock = DockStyle.Fill;
            this.Controls.Add(this.viewer);

            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P37(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format(" id='{0}'", transID);
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

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
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

        // print

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = row["Remark"].ToString();
            string cDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt1;
            string cmdd =
                @"select    
            b.nameEN 
            from dbo.ReturnReceipt  a WITH (NOLOCK) 
            inner join dbo.mdivision  b WITH (NOLOCK) 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID";
            DualResult result1 = DBProxy.Current.Select(string.Empty, cmdd, pars, out dt1);
            if (!result1)
            {
                this.ShowErr(result1);
            }

            string rptTitle = dt1.Rows[0]["nameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("CDate", cDate));

            DataTable dtRefund;
            string refundResult;
            cmdd = @"Select R.whsereasonid,W.Description
            from dbo.returnReceipt R WITH (NOLOCK) 
		    LEFT join dbo.WhseReason W WITH (NOLOCK) 
		    ON W.type='RR'AND W.ID = R.WhseReasonId
		    WHERE R.id = @ID";
            DBProxy.Current.Select(string.Empty, cmdd, pars, out dtRefund);
            if (dtRefund.Rows.Count == 0)
            {
                refundResult = string.Empty;
            }
            else
            {
                refundResult = dtRefund.Rows[0]["Description"].ToString();
            }

            report.ReportParameters.Add(new ReportParameter("RefundResult", refundResult));

            DataTable dtAction;
            string actionResult;
            cmdd = @"Select  R.whsereasonid,[desc] = W.Description   
                from dbo.returnReceipt R WITH (NOLOCK) 
		        LEFT join dbo.WhseReason W WITH (NOLOCK) 	ON W.type='RA'AND W.ID = R.ActionID
		        WHERE R.id = @ID";
            DBProxy.Current.Select(string.Empty, cmdd, pars, out dtAction);
            if (dtAction.Rows.Count == 0)
            {
                actionResult = string.Empty;
            }
            else
            {
                actionResult = dtAction.Rows[0]["desc"].ToString();
            }

            report.ReportParameters.Add(new ReportParameter("ActionResult", actionResult));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion

            #region -- 撈表身資料 --
            DataTable dtDetail;
            string sqlcmd = @"
select  ROW_NUMBER() OVER(ORDER BY R.POID,R.SEQ1,R.SEQ2) AS NoID
        ,ROW_NUMBER() OVER(Partition by R.POID,R.SEQ1,R.SEQ2 ORDER BY R.POID,R.SEQ1,R.SEQ2) AS GroupNo
		,R.poid AS SP,R.seq1  + '-' +R.seq2 as SEQ
		,IIF((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2) 
		          AND(p.seq1 = lag(p.seq1,1,'')over (order by p.ID,p.seq1,p.seq2))
		          AND(p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))) 
	          ,''
              ,dbo.getMtlDesc(R.POID,R.Seq1,R.Seq2,2,0))[desc]
        ,p.StockUnit,R.Roll,R.dyelot,R.qty
	    ,case R.StockType
    		WHEN 'I'THEN 'Inventory'
			WHEN 'B'THEN 'Bulk'
			ELSE R.StockType
		end StockType
		,dbo.Getlocation(fi.ukey) [Location]
		,[Total]=sum(R.Qty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
from dbo.ReturnReceipt_Detail R WITH (NOLOCK) 
LEFT join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.ID = R.POID and  p.SEQ1 = R.Seq1 and P.seq2 = R.Seq2 
left join dbo.FtyInventory FI on r.poid = fi.poid and r.seq1 = fi.seq1 and r.seq2 = fi.seq2 
    and r.roll = fi.roll and r.stocktype = fi.stocktype and r.Dyelot = fi.Dyelot
where R.id= @ID";
            result1 = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dtDetail);
            if (!result1)
            {
                this.ShowErr(sqlcmd, result1);
            }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", caption: string.Empty);
                return false;
            }

            // 傳 list 資料
            List<P37_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P37_PrintData()
                {
                    NoID = row1["NoID"].ToString(),
                    GroupNo = row1["GroupNo"].ToString(),
                    OrderID = row1["SP"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    Desc = row1["desc"].ToString(),
                    Unit = row1["StockUnit"].ToString(),
                    Roll = row1["Roll"].ToString(),
                    Dyelot = row1["dyelot"].ToString(),
                    Qty = row1["qty"].ToString(),
                    StockType = row1["StockType"].ToString(),
                    Location = row1["Location"].ToString(),
                    TotalQty = row1["Total"].ToString(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P37_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P37_Print.rdlc";

            IReportResource reportresource;
            if (!(result1 = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
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

        // Print - subreport
        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();
            #region -- 必輸檢查 --

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["WhseReasonId"]))
            {
                MyUtility.Msg.WarningBox("< Refund Reason >  can't be empty!", "Warning");
                this.txtwhseReasonRefundReason.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["actionid"]))
            {
                MyUtility.Msg.WarningBox("< Action>  can't be empty!", "Warning");
                this.txtwhseRefundAction.TextBox1.Focus();
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
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

            // 檢查是否有退還料紀錄和00004負數Qty 就不能存檔
            if (!this.ChkNegativeNumber() || !this.ChkRecord())
            {
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "RT", "ReturnReceipt", (DateTime)this.CurrentMaintain["Issuedate"]);
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
            this.txtwhseRefundAction.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", "RA" + this.txtwhseRefundAction.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            this.txtwhseReasonRefundReason.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.txtwhseReasonRefundReason.Type.ToString() + this.txtwhseReasonRefundReason.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["Stocktype"] = 'B';
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings supportNegative = new DataGridViewGeneratorNumericColumnSettings();
            supportNegative.IsSupportNegative = true;
            DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            ns.CellFormatting = (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                switch (dr["StockType"].ToString())
                {
                    case "B":
                        e.Value = "Bulk";
                        break;
                    case "I":
                        e.Value = "Inventory";
                        break;
                    case "O":
                        e.Value = "Scrap";
                        break;
                }
            };
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
            .Text("StockType", header: "StockType", iseditingreadonly: true, settings: ns) // 5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: supportNegative) // 6
            .Text("Location", header: "Location", iseditingreadonly: true) // 7
            ;
            #endregion 欄位設定

            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
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
            string upd_MD_37T = string.Empty;
            string upd_Fty_37T = string.Empty;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.returnreceipt_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
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
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "ReturnReceipt_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (dt != null)
            {
                if (!Prgs.ChkWMSCompleteTime(dt, "ReturnReceipt_Detail"))
                {
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,[balanceQty] = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
    ,d.Dyelot
from dbo.ReturnReceipt_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            // 檢查是否有退還料紀錄和00004負數Qty 就不能Confirmed
            if (!this.ChkNegativeNumber() || !this.ChkRecord())
            {
                return;
            }

            #region -- 更新庫存數量  ftyinventory --

            var data_Fty_37T = (from b in this.DetailDatas
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
            upd_Fty_37T = Prgs.UpdateFtyInventory_IO(37, null, true);
            #endregion

            #region -- update mdivisionPoDetail --
            var data_MD_37T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           location = m.First().Field<string>("location"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"update ReturnReceipt set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_37T, string.Empty, upd_Fty_37T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_37T.Count > 0)
                    {
                        upd_MD_37T = Prgs.UpdateMPoDetail(37, null, true, sqlConn: sqlConn);
                    }

                    if (data_MD_37T.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_37T, string.Empty, upd_MD_37T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
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
                    DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

                    // AutoWHACC WebAPI for Vstrong
                    if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
                    {
                        Task.Run(() => new Vstrong_AutoWHAccessory().SentReturnReceipt_Detail_New(dtDetail, "New"))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                    }

                    // AutoWH Fabric WebAPI for Gensong
                    if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                    {
                        Task.Run(() => new Gensong_AutoWHFabric().SentReturnReceipt_Detail_New(dtDetail, "New"))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                    }

                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
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
            if (dResult == DialogResult.No)
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            StringBuilder sqlupd4 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            string upd_MD_37F = string.Empty;
            string upd_Fty_37F = string.Empty;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.ReturnReceipt_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
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
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "ReturnReceipt_Detail"))
            {
                return;
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.ReturnReceipt_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region UnConfirmed 先檢查WMS是否傳送成功

            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                if (!Vstrong_AutoWHAccessory.SentReturnReceipt_Detail_delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }

            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                if (!Gensong_AutoWHFabric.SentReturnReceipt_Detail_Delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }
            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_37F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            upd_Fty_37F = Prgs.UpdateFtyInventory_IO(37, null, false);
            #endregion

            #region -- update mdivisionPoDetail --
            var data_MD_37F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"update ReturnReceipt set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_37F, string.Empty, upd_Fty_37F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_37F.Count > 0)
                    {
                        upd_MD_37F = Prgs.UpdateMPoDetail(37, null, false, sqlConn: sqlConn);
                    }

                    if (data_MD_37F.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_37F, string.Empty, upd_MD_37F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
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
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = $@"
select
[Barcode1] = f.Barcode
,[Barcode2] = fb.Barcode
,[OriBarcode] = fbOri.Barcode
,[balanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[NewBarcode] = ''
,i2.Id,i2.POID,i2.Seq1,i2.Seq2,i2.StockType,i2.Roll,i2.Dyelot
from Production.dbo.returnReceipt_Detail i2
inner join Production.dbo.returnReceipt i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.POID
and f.Seq1 = i2.Seq1 and f.Seq2 = i2.Seq2
and f.Roll = i2.Roll and f.Dyelot = i2.Dyelot
and f.StockType = i2.StockType
outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
)fb
outer apply(
	select *
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = '{this.CurrentMaintain["ID"]}'
)fbOri
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
and i2.id = '{this.CurrentMaintain["ID"]}'

";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                string strBarcode = MyUtility.Check.Empty(dr["Barcode2"]) ? dr["Barcode1"].ToString() : dr["Barcode2"].ToString();
                if (isConfirmed)
                {
                    // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                    if (!MyUtility.Check.Empty(dr["balanceQty"]) && !MyUtility.Check.Empty(strBarcode))
                    {
                        if (strBarcode.Contains("-"))
                        {
                            dr["NewBarcode"] = strBarcode.Substring(0, 13) + "-" + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                        }
                        else
                        {
                            dr["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                        }
                    }
                    else
                    {
                        // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                        dr["NewBarcode"] = dr["Barcode1"];
                    }
                }
                else
                {
                    // unConfirmed 要用自己的紀錄給補回
                    dr["NewBarcode"] = dr["OriBarcode"];
                }
            }

            var data_Fty_Barcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                    select new
                                    {
                                        TransactionID = m.Field<string>("ID"),
                                        poid = m.Field<string>("poid"),
                                        seq1 = m.Field<string>("seq1"),
                                        seq2 = m.Field<string>("seq2"),
                                        stocktype = m.Field<string>("stocktype"),
                                        roll = m.Field<string>("roll"),
                                        dyelot = m.Field<string>("dyelot"),
                                        Barcode = m.Field<string>("NewBarcode"),
                                    }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            string upd_Fty_Barcode_V1 = isConfirmed ? Prgs.UpdateFtyInventory_IO(70, null, !isConfirmed) : Prgs.UpdateFtyInventory_IO(72, null, true);
            string upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resulttb;
            if (data_Fty_Barcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V1, out resulttb, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V2, out resulttb, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,p1.stockunit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Qty
,a.StockType
,dbo.Getlocation(fi.ukey) location
,a.ukey
,a.FtyInventoryUkey
from dbo.ReturnReceipt_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot 
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // Delete empty qty
        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P37_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P37_AccumulatedQty(this.CurrentMaintain);
            frm.P37 = this;
            frm.ShowDialog(this);
        }

        // Locate for (find)
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("poid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void TxtwhseReasonRefundReason_Validated(object sender, EventArgs e)
        {
            this.txtwhseRefundAction.TextBox1.Text = string.Empty;
            this.txtwhseRefundAction.DisplayBox1.Text = string.Empty;
            this.txtwhseRefundAction.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P37", this);
        }

        private bool ChkNegativeNumber()
        {
            string errormsg = string.Empty;
            if (this.CurrentMaintain["whsereasonID"].ToString() == "00004")
            {
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (MyUtility.Convert.GetDecimal(dr["Qty"]) > 0)
                    {
                        errormsg += $@"SP#: {dr["POID"]} Seq#: {dr["Seq1"]}-{dr["Seq2"]} Roll#: {dr["Roll"]} Dyelot: {dr["Dyelot"]} Issue Qty: {dr["Qty"]}" + Environment.NewLine;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errormsg))
            {
                MyUtility.Msg.WarningBox(@"The refund reason is 00004 (Excess delivery against packing list)
You should encode negative number in issue qty column." + Environment.NewLine + errormsg);
                return false;
            }

            return true;
        }

        private bool ChkRecord()
        {
            string errormsg = string.Empty;
            foreach (DataRow dr in this.DetailDatas)
            {
                string sqlChk = $@"
Select 1
from FtyInventory f WITH (NOLOCK) 
where f.poid = '{dr["POID"]}' and f.seq1 = '{dr["Seq1"]}' and f.seq2 = '{dr["Seq2"]}' and f.Dyelot = '{dr["Dyelot"]}' and f.roll = '{dr["Roll"]}' and f.stocktype = '{dr["stocktype"]}'
and (f.OutQty > 0 or f.AdjustQty != 0 or f.ReturnQty != 0)
";
                if (MyUtility.Check.Seek(sqlChk))
                {
                    errormsg += $@"SP#: {dr["POID"]} Seq#: {dr["Seq1"]}-{dr["Seq2"]} Roll#: {dr["Roll"]} Dyelot: {dr["Dyelot"]} Issue Qty: {dr["Qty"]}" + Environment.NewLine;
                }
            }

            if (!MyUtility.Check.Empty(errormsg))
            {
                MyUtility.Msg.WarningBox(@"These material have issued or adjusted or returned records , You can't return it." + Environment.NewLine + errormsg + Environment.NewLine + @"If you want to adjust these material qty, you should use W/H P34/P35 to adjust it.");
                return false;
            }

            return true;
        }
    }
}