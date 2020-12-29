using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P13 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(this.viewer);

            // MDivisionID 是 P13 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("Type='D' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P13(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='D' and id='{0}'", transID);
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
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "D";
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
        protected override DualResult ClickDeletePost()
        {
            // 當表身被刪除時，要判斷是否[Issue_Detail.ukey]有在[FIR_Physical].[Issue_DetailUkey]中，若有則將[FIR_Physical].[Issue_DetailUkey]更新成0。
            string iD = this.CurrentMaintain["ID"].ToString();

            List<string> ukeyList = this.DetailDatas.AsEnumerable().Select(o => o["Ukey"].ToString()).ToList();
            string ukeys = string.Join(",", ukeyList);
            string cmd = $@"
UPDATE FIR_Physical
SET Issue_DetailUkey = 0
WHERE Issue_DetailUkey IN ({ukeys})
";
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, cmd)))
            {
                return upResult;
            }

            return base.ClickDeletePost();
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

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = row["Remark"].ToString();
            string cDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@MDivision", Env.User.Keyword),
                new SqlParameter("@ID", id),
            };
            DualResult result = DBProxy.Current.Select(string.Empty, @"select NameEn from MDivision where id = @MDivision", pars, out DataTable dt);

            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dt");
                return false;
            }

            string ftyGroup = string.Empty;
            foreach (DataRow item in ((DataTable)this.detailgridbs.DataSource).DefaultView.ToTable(true, "FtyGroup").Rows)
            {
                ftyGroup += MyUtility.Convert.GetString(item["FtyGroup"]) + ",";
            }

            ftyGroup = ftyGroup.Substring(0, ftyGroup.Length - 1 >= 0 ? ftyGroup.Length - 1 : 0);

            string rptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("CDate", cDate));
            report.ReportParameters.Add(new ReportParameter("FtyGroup", ftyGroup));

            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
            };
            string sqlcmd = @"
select t.POID,
	    t.seq1+ '-' +t.seq2 as SEQ,
        p.Scirefno,
	    p.seq1,
	    p.seq2,
	    [desc] =IIF((p.ID = lag(p.ID,1,'')over (order by t.POID,p.seq1,p.seq2, t.Dyelot,t.Roll) 
			    AND(p.seq1 = lag(p.seq1,1,'')over (order by t.POID,p.seq1,p.seq2, t.Dyelot,t.Roll))
			    AND(p.seq2 = lag(p.seq2,1,'')over (order by t.POID,p.seq1,p.seq2, t.Dyelot,t.Roll))) 
			    ,''
                ,dbo.getMtlDesc(t.poid,t.seq1,t.seq2,2,0)),
        MDesc = iif(FabricType='F', 'Relaxation Type：'+(select FabricRelaxationID from [dbo].[SciMES_RefnoRelaxtime] where Refno = p.Refno), ''),
	    t.Roll,
	    t.Dyelot,
	    t.Qty,
	    p.StockUnit,
        dbo.Getlocation(fi.ukey) [location],
	    [Total]=sum(t.Qty) OVER (PARTITION BY t.POID ,t.seq1,t.seq2 )            
from dbo.Issue_Detail t WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id= t.poid and p.SEQ1 = t.Seq1 and p.seq2 = t.Seq2
left join FtyInventory FI on t.poid = fi.poid and t.seq1 = fi.seq1 and t.seq2 = fi.seq2 and t.Dyelot = fi.Dyelot
    and t.roll = fi.roll and t.stocktype = fi.stocktype
where t.id= @ID
order by t.POID,SEQ, t.Dyelot,t.Roll
";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtDetail);
            if (!result)
            {
                this.ShowErr(sqlcmd, result);
            }

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
                    MDESC = row1["Mdesc"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                    StockUnit = row1["StockUnit"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    DYELOT = row1["Dyelot"].ToString().Trim(),
                    QTY = row1["Qty"].ToString().Trim(),
                    TotalQTY = row1["Total"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P13_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P13_Print.rdlc";

            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report)
            {
                MdiParent = this.MdiParent,
            };
            frm.Show();

            return base.ClickPrint();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Whsereasonid"]))
            {
                MyUtility.Msg.WarningBox("< Reason >  can't be empty!", "Warning");
                this.txtwhseReason.TextBox1.Focus();
                return false;
            }
            else
            {
                if (this.CurrentMaintain["Whsereasonid"].ToString() == "00005" && MyUtility.Check.Empty(this.CurrentMaintain["remark"]))
                {
                    MyUtility.Msg.WarningBox("< Remark >  can't be empty!", "Warning");
                    this.editRemark.Focus();
                    return false;
                }
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

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "II", "Issue", (DateTime)this.CurrentMaintain["Issuedate"]);
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
        protected override DualResult ClickSavePre()
        {
            return base.ClickSavePre();
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
            this.txtwhseReason.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.txtwhseReason.Type.ToString() + this.txtwhseReason.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            #region Status Label

            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (this.CurrentMaintain["status"].ToString().EqualString("Confirmed"))
            {
                this.btnPrintFabricSticker.Enabled = true;
            }
            else
            {
                this.btnPrintFabricSticker.Enabled = false;
            }

            this.DetailGridColVisibleByReason();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FtyGroup", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                .EditText("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 8
                .Numeric("NetQty", header: "Used Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("LossQty", header: "Loss Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
                .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
            ;
            #endregion 欄位設定

        }

        /// <inheritdoc/>
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
            StringBuilder sqlupd2_B = new StringBuilder();
            string sqlupd2_FIO = string.Empty;
            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = $@"update Issue set status='Confirmed', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'";

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    // 更新Ftyinventory barcode
                    this.FtyBarcodeData(true);
                    this.SentToGensong_AutoWHFabric(true);
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
            this.SentToGensong_AutoWH_ACC(true);
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
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
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            StringBuilder sqlupd2_B = new StringBuilder();
            string sqlupd2_FIO = string.Empty;

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot   
from dbo.Issue_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料
            sqlupd3 = $@"update Issue set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'";

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));

            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    // 刪除Ftyinventory barcode
                    this.FtyBarcodeData(false);
                    this.SentToGensong_AutoWHFabric(false);
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
            this.SentToGensong_AutoWH_ACC(false);
        }

        private void SentToGensong_AutoWHFabric(bool isConfirmed)
        {
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                DataTable dtDetail = new DataTable();
                string sqlGetData = string.Empty;
                sqlGetData = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = 'D'
,[CutPlanID] = ''
,[EstCutdate] = null
,[SpreadingNoID] = ''
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Roll] = i2.Roll
,[Dyelot] = i2.Dyelot
,[Barcode] = Barcode.value
,[NewBarcode] = NewBarcode.value
,[Qty] = i2.Qty
,[Ukey] = i2.ukey
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,CmdTime = GetDate()
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id
left join Production.dbo.Cutplan c on c.ID = i.CutplanID
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
outer apply(
	select value = fb.Barcode
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey and fb.TransactionID = i2.Id
)NewBarcode
where i.Type = 'D'
and exists(
		select 1 from Production.dbo.PO_Supp_Detail 
		where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
		and FabricType='F'
	)
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where f.Ukey = fd.Ukey
	and ml.IsWMS = 1
)
and i.id = '{this.CurrentMaintain["ID"]}'

";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                Task.Run(() => new Gensong_AutoWHFabric().SentIssue_DetailToGensongAutoWHFabric(dtDetail))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = $@"
select f.Ukey,fb.TransactionID
,[Barcode1] = f.Barcode
,[Barcode2] = fb.Barcode
,[balanceQty] = f.InQty-f.OutQty+f.AdjustQty
,[NewBarcode] = ''
,i2.Id,i2.POID,i2.Seq1,i2.Seq2,i2.StockType,i2.Roll,i2.Dyelot
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.POID
    and f.Seq1 = i2.Seq1 and f.Seq2 = i2.Seq2
    and f.Roll = i2.Roll and f.Dyelot = i2.Dyelot
    and f.StockType = i2.StockType
left join FtyInventory_Barcode fb on f.Ukey = fb.Ukey
and fb.TransactionID = i2.Id
where i.Type =  'D'
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                string strBarcode = MyUtility.Check.Empty(dr["Barcode2"]) ? dr["Barcode1"].ToString() : dr["Barcode2"].ToString();

                // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                if (!MyUtility.Check.Empty(dr["balanceQty"]) && !MyUtility.Check.Empty(strBarcode))
                {
                    if (strBarcode.Contains("-"))
                    {
                        dr["NewBarcode"] = strBarcode.Substring(0, 13) + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                    }
                    else
                    {
                        dr["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                    }
                }
                else
                {
                    // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                    dr["NewBarcode"] = strBarcode;
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

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"

select  o.FtyGroup
        , a.id
        , a.PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , p1.FabricType
        , p1.stockunit
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
        , a.Roll
        , a.Dyelot
        , a.Qty
        , a.StockType
        , Isnull(c.inqty-c.outqty + c.adjustqty,0.00) as balance
        , dbo.Getlocation(c.ukey) location
        , a.ukey
		, p1.NetQty
		, p1.LossQty
        , [Article] = case  when a.Seq1 like 'T%' then Stuff((Select distinct concat( ',',tcd.Article) 
			                                                         From dbo.Orders as o 
			                                                         Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
			                                                         Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
			                                                         Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey 
			                                                         where	o.POID = a.PoId and
			                                                         		tcd.SuppId = p.SuppId and
			                                                         		tcd.SCIRefNo   = p1.SCIRefNo	and
			                                                         		tcd.ColorID	   = p1.ColorID
			                                                         FOR XML PATH('')),1,1,'') 
                            else '' end
from dbo.issue_detail as a WITH (NOLOCK) 
left join Orders o on a.poid = o.id
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join PO_Supp p WITH (NOLOCK) on p.ID = p1.ID and p1.seq1 = p.SEQ1
left join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.poid and c.seq1 = a.seq1 and c.seq2  = a.seq2 
    and c.stocktype = 'B' and c.roll=a.roll and a.Dyelot = c.Dyelot
Where a.id = '{0}'", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            // ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => ((DataTable)detailgridbs.DataSource).Rows.Remove(r));
            // 不可以用 Remove()，因為datatable還沒有更新回資料庫中，被Remove的資料沒有真正刪除。
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["WhseReasonID"]))
            {
                MyUtility.Msg.WarningBox("Please choose reason first !!");
                return;
            }

            var frm = new P13_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P13_AccumulatedQty(this.CurrentMaintain)
            {
                P13 = this,
            };
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

        private void BtCutRef_Click(object sender, EventArgs e)
        {
            var frm = new P10_CutRef(this.CurrentMaintain);
            frm.ShowDialog(this);
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P13_FabricSticker(this.CurrentMaintain["ID"], "Issue_Detail").ShowDialog();
        }

        private void TxtwhseReason_Validated(object sender, EventArgs e)
        {
            this.DetailGridColVisibleByReason();
        }

        private void TxtwhseReason_Leave(object sender, EventArgs e)
        {
            this.DetailGridColVisibleByReason();
        }

        private void DetailGridColVisibleByReason()
        {
            if (this.CurrentMaintain["whseReasonID"].ToString() == "00006")
            {
                this.detailgrid.Columns["NetQty"].Visible = true;
                this.detailgrid.Columns["LossQty"].Visible = true;
                this.detailgrid.Columns["Article"].Visible = true;
            }
            else
            {
                this.detailgrid.Columns["NetQty"].Visible = false;
                this.detailgrid.Columns["LossQty"].Visible = false;
                this.detailgrid.Columns["Article"].Visible = false;
            }
        }

        /// <summary>
        ///  AutoWH ACC WebAPI for Gensong
        /// </summary>
        private void SentToGensong_AutoWH_ACC(bool isConfirmed)
        {
            // AutoWHACC WebAPI for Gensong
            if (Gensong_AutoWHAccessory.IsGensong_AutoWHAccessoryEnable)
            {
                DataTable dtDetail = new DataTable();
                string sqlGetData = string.Empty;
                sqlGetData = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = i.Type
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[StockType] = i2.StockType
,[Qty] = i2.Qty
,[StockPOID] = po3.StockPOID
,[StockSeq1] = po3.StockSeq1
,[StockSeq2] = po3.StockSeq2
,[Ukey] = i2.ukey
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,CmdTime = GetDate()
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
left join PO_Supp_Detail po3 on po3.ID = i2.POID
	and po3.SEQ1 = i2.Seq1 and po3.SEQ2 = i2.Seq2
where i.Type = 'D'
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='A'
)
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where f.Ukey = fd.Ukey
	and ml.IsWMS = 1
)
and i.id = '{this.CurrentMaintain["ID"]}'

";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                Task.Run(() => new Gensong_AutoWHAccessory().SentIssue_DetailToGensongAutoWHAccessory(dtDetail))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}