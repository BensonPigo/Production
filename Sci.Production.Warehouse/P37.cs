using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
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
    /// <inheritdoc/>
    public partial class P37 : Win.Tems.Input6
    {
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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

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

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            WH_Print p = new WH_Print(this.CurrentMaintain, "P37")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
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
        ,fi.ContainerCode
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
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        TotalQty = row1["Total"].ToString(),
                    }).ToList();

                report.ReportDataSource = data;
                #endregion
                #region 指定是哪個 RDLC

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
                #endregion
            }

            return true;
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        /// <inheritdoc/>
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
                    warningmsg.Append($@"SP#: {row["poid"]} Seq#: {row["seq1"]}-{row["seq2"]} can't be empty" + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append($@"SP#: {row["poid"]} Seq#: {row["seq1"]}-{row["seq2"]} Roll#:{row["roll"]} Dyelot:{row["dyelot"]} Issue Qty can't be empty" + Environment.NewLine);
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

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtwhseRefundAction.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", "RA" + this.txtwhseRefundAction.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            this.txtwhseReasonRefundReason.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.txtwhseReasonRefundReason.Type.ToString() + this.txtwhseReasonRefundReason.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["Stocktype"] = 'B';
        }

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

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;
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
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;
            DataTable datacheck;

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region -- 檢查庫存項lock --
            string sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.returnreceipt_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
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

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "ReturnReceipt_Detail") == false)
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
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
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
            string upd_Fty_37T = Prgs.UpdateFtyInventory_IO(37, null, true);
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

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                    using (sqlConn)
                    {
                        /*
                         * 先更新 FtyInventory 後更新 MDivisionPoDetail
                         * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                         * 因為要在同一 SqlConnection 之下執行
                         */
                        // FtyInventory 庫存
                        DataTable resulttb;
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_37T, string.Empty, upd_Fty_37T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        #region MDivisionPoDetail
                        if (data_MD_37T.Count > 0)
                        {
                            string upd_MD_37T = Prgs.UpdateMPoDetail(37, null, true, sqlConn: sqlConn);
                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_37T, string.Empty, upd_MD_37T, out resulttb, "#TmpSource", conn: sqlConn)))
                            {
                                throw result.GetException();
                            }
                        }
                        #endregion

                        if (!(result = DBProxy.Current.Execute(null, $"update ReturnReceipt set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        transactionscope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            DataTable datacheck;
            string ids = string.Empty;

            #region -- 檢查庫存項lock --
            string sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.ReturnReceipt_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
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
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

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
            string upd_Fty_37F = Prgs.UpdateFtyInventory_IO(37, null, false);
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

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        /*
                         * 先更新 FtyInventory 後更新 MDivisionPoDetail
                         * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                         * 因為要在同一 SqlConnection 之下執行
                         */
                        // FtyInventory 庫存
                        DataTable resulttb;
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_37F, string.Empty, upd_Fty_37F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        #region MDivisionPoDetail
                        if (data_MD_37F.Count > 0)
                        {
                            string upd_MD_37F = Prgs.UpdateMPoDetail(37, null, false, sqlConn: sqlConn);
                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_37F, string.Empty, upd_MD_37F, out resulttb, "#TmpSource", conn: sqlConn)))
                            {
                                throw result.GetException();
                            }
                        }
                        #endregion

                        if (!(result = DBProxy.Current.Execute(null, $"update ReturnReceipt set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                        Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select a.id,a.PoId,a.Seq1,a.Seq2
,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,p1.stockunit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Qty
,a.StockType
,dbo.Getlocation(fi.ukey) location
,[ContainerCode] = FI.ContainerCode
, FI.ContainerCode
,a.ukey
,a.FtyInventoryUkey
from dbo.ReturnReceipt_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot 
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P37_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P37_AccumulatedQty(this.CurrentMaintain);
            frm.P37 = this;
            frm.ShowDialog(this);
        }

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
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
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