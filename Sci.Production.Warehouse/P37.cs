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
using Sci.Win;
using System.Data.SqlClient;
using Sci.Production.Automation;
using System.Threading.Tasks;

namespace Sci.Production.Warehouse
{
    public partial class P37 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P37(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            //
        }

        public P37(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format(" id='{0}'", transID);
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        //print
        protected override bool ClickPrint()
        {
            // dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string CDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt1;
            DualResult result1 = DBProxy.Current.Select("",
            @"select    
            b.nameEN 
            from dbo.ReturnReceipt  a WITH (NOLOCK) 
            inner join dbo.mdivision  b WITH (NOLOCK) 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt1);
            if (!result1) { this.ShowErr(result1); }
            string RptTitle = dt1.Rows[0]["nameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CDate", CDate));
           
            DataTable dtRefund;
            string RefundResult;
            DBProxy.Current.Select("",
          @"Select R.whsereasonid,W.Description
            from dbo.returnReceipt R WITH (NOLOCK) 
		    LEFT join dbo.WhseReason W WITH (NOLOCK) 
		    ON W.type='RR'AND W.ID = R.WhseReasonId
		    WHERE R.id = @ID", pars, out dtRefund);
            if (dtRefund.Rows.Count == 0)
                RefundResult = "";
            else
                RefundResult = dtRefund.Rows[0]["Description"].ToString();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RefundResult", RefundResult));

            DataTable dtAction;
            string ActionResult ;
            DBProxy.Current.Select("",
          @"Select  R.whsereasonid,[desc] = W.Description   
                from dbo.returnReceipt R WITH (NOLOCK) 
		        LEFT join dbo.WhseReason W WITH (NOLOCK) 	ON W.type='RA'AND W.ID = R.ActionID
		        WHERE R.id = @ID", pars, out dtAction);
             if (dtAction.Rows.Count == 0)
                   ActionResult = "";
            else
                 ActionResult = dtAction.Rows[0]["desc"].ToString();
             report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ActionResult", ActionResult));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion

            #region -- 撈表身資料 --
            DataTable dtDetail;
            string sqlcmd = @"
select  ROW_NUMBER() OVER(ORDER BY R.POID,R.SEQ1,R.SEQ2) AS NoID
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
        result1 = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result1) { this.ShowErr(sqlcmd, result1); }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", caption: "");
                return false ;
            }
            // 傳 list 資料            
            List<P37_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P37_PrintData()
                {
                    NoID = row1["NoID"].ToString(),
                    OrderID = row1["SP"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    Desc = row1["desc"].ToString(),
                    Unit = row1["StockUnit"].ToString(),
                    Roll = row1["Roll"].ToString(),
                    Dyelot = row1["dyelot"].ToString(),
                    Qty = row1["qty"].ToString(),
                    StockType = row1["StockType"].ToString(),
                    Location = row1["Location"].ToString(),
                    TotalQty = row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P37_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P37_Print.rdlc";


            IReportResource reportresource;
            if (!(result1 = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
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

        // Print - subreport
        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();
            #region -- 必輸檢查 --

            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["WhseReasonId"]))
            {
                MyUtility.Msg.WarningBox("< Refund Reason >  can't be empty!", "Warning");
                txtwhseReasonRefundReason.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["actionid"]))
            {
                MyUtility.Msg.WarningBox("< Action>  can't be empty!", "Warning");
                txtwhseRefundAction.TextBox1.Focus();
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "RT", "ReturnReceipt", (DateTime)CurrentMaintain["Issuedate"]);
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
            this.txtwhseRefundAction.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", "RA" + this.txtwhseRefundAction.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            this.txtwhseReasonRefundReason.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.txtwhseReasonRefundReason.Type.ToString() + this.txtwhseReasonRefundReason.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["Stocktype"] = 'B';
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings SupportNegative = new DataGridViewGeneratorNumericColumnSettings();
            SupportNegative.IsSupportNegative = true;
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
                        e.Value = "Scrap";
                        break;
                }
            };
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //5
            .Text("StockType", header: "StockType", iseditingreadonly: true, settings: ns)    //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: SupportNegative)    //6
            .Text("Location", header: "Location", iseditingreadonly: true)    //7
            ;     //
            #endregion 欄位設定

            detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            
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
            String upd_MD_2T = "";
            String upd_MD_8T = "";
            String upd_Fty_2T = "";

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.returnreceipt_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion
            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.ReturnReceipt_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新庫存數量  ftyinventory --
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
                        if (!location.EqualString(""))
                        {
                            DataRow newDr = newDt.NewRow();
                            newDr.ItemArray = dtr.ItemArray;
                            newDr["location"] = location;
                            newDt.Rows.Add(newDr);
                        }
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
                             qty = - b.Field<decimal>("qty"),
                             location = b.Field<string>("location"),
                             roll = b.Field<string>("roll"),
                             dyelot = b.Field<string>("dyelot"),
                         }).ToList();
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion

            #region -- update mdivisionPoDetail --
            var data_MD_2T = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           location = m.First().Field<string>("location"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            var data_MD_8T = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                        group b by new
                        {
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                            stocktype = b.Field<string>("stocktype").Trim()
                        } into m
                        select new Prgs_POSuppDetailData
                        {
                            poid = m.First().Field<string>("poid"),
                            seq1 = m.First().Field<string>("seq1"),
                            seq2 = m.First().Field<string>("seq2"),
                            stocktype = m.First().Field<string>("stocktype"),
                            location = m.First().Field<string>("location"),
                            qty = - (m.Sum(w => w.Field<decimal>("qty")))
                        }).ToList();

            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update ReturnReceipt set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

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
                    if (data_MD_2T.Count > 0)
                        upd_MD_2T = Prgs.UpdateMPoDetail(2, null, true, sqlConn:sqlConn);
                    if (data_MD_8T.Count > 0)
                        upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);

                    if (data_MD_2T.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, "", upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (data_MD_8T.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, "", upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    #endregion 

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
           
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
            StringBuilder sqlupd4 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            String upd_MD_2F = "";
            String upd_MD_8F = "";
            String upd_Fty_2F = "";

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.ReturnReceipt_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion
            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.ReturnReceipt_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_2F = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion

            #region -- update mdivisionPoDetail --
            var data_MD_2F = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            var data_MD_8F = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                        group b by new
                        {
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                            stocktype = b.Field<string>("stocktype").Trim()
                        } into m
                        select new Prgs_POSuppDetailData
                        {
                            poid = m.First().Field<string>("poid"),
                            seq1 = m.First().Field<string>("seq1"),
                            seq2 = m.First().Field<string>("seq2"),
                            stocktype = m.First().Field<string>("stocktype"),
                            qty = m.Sum(w => w.Field<decimal>("qty"))
                        }).ToList();

            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update ReturnReceipt set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

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
                    if (data_MD_2F.Count > 0)
                        upd_MD_2F = Prgs.UpdateMPoDetail(2, null, false, sqlConn: sqlConn);
                    if (data_MD_8F.Count > 0)
                        upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn);

                    if (data_MD_2F.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, "", upd_MD_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (data_MD_8F.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, "", upd_MD_8F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    #endregion 

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
           
        }

        private void SentToGensong_AutoWHFabric()
        {
            // AutoWHFabric WebAPI for Gensong       
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                DataTable dtMain = CurrentMaintain.Table.Clone();
                dtMain.ImportRow(CurrentMaintain);
                Task.Run(() => new Gensong_AutoWHFabric().SentReturnReceiptToGensongAutoWHFabric(dtMain))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
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

        //Delete empty qty
        private void btnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P37_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void btnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P37_AccumulatedQty(CurrentMaintain);
            frm.P37 = this;
            frm.ShowDialog(this);
        }

        //Locate for (find)
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        private void txtwhseReasonRefundReason_Validated(object sender, EventArgs e)
        {
            this.txtwhseRefundAction.TextBox1.Text = "";
            this.txtwhseRefundAction.DisplayBox1.Text = "";
            this.txtwhseRefundAction.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue()); 
        }    
    }
}