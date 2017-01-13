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

            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
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
            CurrentMaintain["Status"] = "New";
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
            string Remark = row["Remark"].ToString();
            string CDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt1;
            DualResult result1 = DBProxy.Current.Select("",
            @"select    
            b.name 
            from dbo.ReturnReceipt  a 
            inner join dbo.mdivision  b 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt1);
            if (!result1) { this.ShowErr(result1); }
            string RptTitle = dt1.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CDate", CDate));
           
            DataTable dtRefund;
            string RefundResult;
            DBProxy.Current.Select("",
          @"Select R.whsereasonid,W.Description
            from dbo.returnReceipt R
		    LEFT join dbo.WhseReason W 
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
                from dbo.returnReceipt R
		        LEFT join dbo.WhseReason W 	ON W.type='RA'AND W.ID = R.ActionID
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
            string sqlcmd = @"select  
			ROW_NUMBER() OVER(ORDER BY R.POID,R.SEQ1,R.SEQ2) AS NoID
			,R.poid AS SP,R.seq1  + '-' +R.seq2 as SEQ
			,Replace(dbo.getMtlDesc(R.POID,R.Seq1,R.Seq2,2,iif(p.scirefno = lag(p.scirefno,1,'') over (order by p.refno,p.seq1,p.seq2),1,0)) , Char(13) + Char(10), '') [desc]
            ,p.StockUnit,R.Roll,R.dyelot,R.qty
		    ,case R.StockType
			WHEN 'I'THEN 'Inventory'
			WHEN 'B'THEN 'Bulk'
			ELSE R.StockType
			end StockType
			,dbo.Getlocation(R.FtyInventoryUkey) [Location]
			,[Total]=sum(R.Qty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
            from dbo.ReturnReceipt_Detail R
            LEFT join dbo.PO_Supp_Detail p 
            on 
           p.ID = R.POID and  p.SEQ1 = R.Seq1 and P.seq2 = R.Seq2 
            where R.id= @ID";
            result1 = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result1) { this.ShowErr(sqlcmd, result1); }

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
                dateBox3.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["WhseReasonId"]))
            {
                MyUtility.Msg.WarningBox("< Refund Reason >  can't be empty!", "Warning");
                txtwhseReason1.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["actionid"]))
            {
                MyUtility.Msg.WarningBox("< Action>  can't be empty!", "Warning");
                txtwhseRefundAction1.TextBox1.Focus();
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
            this.txtwhseRefundAction1.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", "RA" + this.txtwhseRefundAction1.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            this.txtwhseReason1.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.txtwhseReason1.Type.ToString() + this.txtwhseReason1.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
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
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //5
            .Text("StockType", header: "StockType", iseditingreadonly: true, settings: ns)    //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
            .Text("Location", header: "Location", iseditingreadonly: true)    //7
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
            String sqlupd2_B = "";
            String sqlupd2_BI = "";
            String sqlupd2_FIO = "";

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.returnreceipt_Detail d inner join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.MDivisionID = f.MDivisionID and d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType 
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
            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.ReturnReceipt_Detail d left join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.MDivisionID = f.MDivisionID and d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType 
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

            var bsfio = (from b in newDt.AsEnumerable()
                         select new
                         {
                             mdivisionid = b.Field<string>("mdivisionid"),
                             poid = b.Field<string>("poid"),
                             seq1 = b.Field<string>("seq1"),
                             seq2 = b.Field<string>("seq2"),
                             stocktype = b.Field<string>("stocktype"),
                             qty = - b.Field<decimal>("qty"),
                             location = b.Field<string>("location"),
                             roll = b.Field<string>("roll"),
                             dyelot = b.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion

            #region -- update mdivisionPoDetail --
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
                           location = m.First().Field<string>("location"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                        group b by new
                        {
                            mdivisionid = b.Field<string>("mdivisionid").Trim(),
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                            stocktype = b.Field<string>("stocktype").Trim()
                        } into m
                        select new Prgs_POSuppDetailData
                        {
                            mdivisionid = m.First().Field<string>("mdivisionid"),
                            poid = m.First().Field<string>("poid"),
                            seq1 = m.First().Field<string>("seq1"),
                            seq2 = m.First().Field<string>("seq2"),
                            stocktype = m.First().Field<string>("stocktype"),
                            location = m.First().Field<string>("location"),
                            qty = - (m.Sum(w => w.Field<decimal>("qty")))
                        }).ToList();
            if (bs1.Count > 0)
                sqlupd2_B = Prgs.UpdateMPoDetail(2, null, true);
            if (bs1I.Count > 0)
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, true);
            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update ReturnReceipt set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

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
                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, "", sqlupd2_BI, out resulttb, "#TmpSource")))
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
            StringBuilder sqlupd4 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            String sqlupd2_B = "";
            String sqlupd2_BI = "";
            String sqlupd2_FIO = "";

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.ReturnReceipt_Detail d inner join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.MDivisionID = f.MDivisionID and d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType 
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
            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.ReturnReceipt_Detail d left join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.MDivisionID = f.MDivisionID and d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType 
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

            #region -- 更新庫存數量  ftyinventory --
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
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion

            #region -- update mdivisionPoDetail --
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
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                        group b by new
                        {
                            mdivisionid = b.Field<string>("mdivisionid").Trim(),
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                            stocktype = b.Field<string>("stocktype").Trim()
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
            if (bs1.Count > 0)
                sqlupd2_B = Prgs.UpdateMPoDetail(2, null, false);
            if (bs1I.Count > 0)
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, false);
            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update ReturnReceipt set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

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
                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, "", sqlupd2_BI, out resulttb, "#TmpSource")))
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.mdivisionid,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,p1.stockunit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Qty
,a.StockType
,stuff((select t.MtlLocationID+',' from (select mtllocationid from dbo.ftyinventory_detail fd where fd.Ukey = a.FtyInventoryUkey) t 
	for xml path('')), 1, 1, '') location
,a.ukey
,a.FtyInventoryUkey
from dbo.ReturnReceipt_Detail a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //Delete empty qty
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P37_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P37_AccumulatedQty(CurrentMaintain);
            frm.P37 = this;
            frm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Locate for (find)
        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }
        private void txtwhseReason1_Validated(object sender, EventArgs e)
        {
            this.txtwhseRefundAction1.TextBox1.Text = "";
            this.txtwhseRefundAction1.DisplayBox1.Text = "";
            this.txtwhseRefundAction1.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue()); 
        }
    
    }
}