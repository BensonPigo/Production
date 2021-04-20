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
    /// <inheritdoc/>
    public partial class P15 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        /// <inheritdoc/>
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer();
            this.viewer.Dock = DockStyle.Fill;
            this.Controls.Add(this.viewer);

            // MDivisionID 是 P15 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("FabricType='A' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P15(ToolStripMenuItem menuitem, string transID, string callFrom = "")
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("FabricType='A' and id='{0}'", transID);
            if (!callFrom.Equals("PPIC.P15"))
            {
                this.IsSupportNew = false;
                this.IsSupportEdit = false;
                this.IsSupportDelete = false;
                this.IsSupportClose = false;
                this.IsSupportUnclose = false;
                this.IsSupportConfirm = false;
                this.IsSupportUnconfirm = false;
            }

            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        // PPIC_P15 Called
        public static void Call(string pPIC_id, Form mdiParent)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is P15)
                {
                    form.Activate();
                    P15 activateForm = (P15)form;
                    return;
                }
            }

            ToolStripMenuItem p15MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("Issue Transaction"))
                            {
                                foreach (var endMenuItem in ((ToolStripMenuItem)subMenuItem).DropDown.Items)
                                {
                                    if (endMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                                    {
                                        if (((ToolStripMenuItem)endMenuItem).Text.EqualString("P15. Issue Accessory Lacking  && Replacement"))
                                        {
                                            p15MenuItem = (ToolStripMenuItem)endMenuItem;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            P15 call = new P15(p15MenuItem, pPIC_id, "PPIC.P15");
            call.MdiParent = mdiParent;
            call.Show();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 新增Batch Shipment Finished按鈕
            Win.UI.Button btnUnFinish = new Win.UI.Button();
            btnUnFinish.Text = "UnFinish";
            btnUnFinish.Click += new EventHandler(this.Unfinish);
            this.browsetop.Controls.Add(btnUnFinish);
            btnUnFinish.Size = new Size(180, 30); // 預設是(80,30)
            btnUnFinish.Visible = true;
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Dictionary<string, string> di_type = new Dictionary<string, string>();
            di_type.Add("L", "Lacking");
            di_type.Add("R", "Replacement");
            this.comboType.DataSource = new BindingSource(di_type, null);
            this.comboType.ValueMember = "Key";
            this.comboType.DisplayMember = "Value";
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["FabricType"] = "A";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
            this.txtLocalSupp1.TextBox1.ReadOnly = true;
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

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtLocalSupp1.TextBox1.ReadOnly = true;
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["Requestid"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                this.txtRequest.Focus();
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

                DataTable dr = (DataTable)this.detailgridbs.DataSource;
                DataRow[] temp = dr.Select(string.Format("poid='{0}' and seq1='{1}' and seq2='{2}'", row["poid"], row["seq1"], row["seq2"]));
                if (temp.Count() > 1)
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be duplicate",
                        row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IL", "IssueLack", (DateTime)this.CurrentMaintain["Issuedate"]);
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
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            // Lack.ApvDate
            if (!MyUtility.Check.Empty(MyUtility.GetValue.Lookup(string.Format(@"select apvdate from lack WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["requestid"]))))
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format(@"select apvdate,Shift,SubconName from lack WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["requestid"]), out dr))
                {
                    this.displayApvDate.Text = ((DateTime)dr["apvdate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
                    this.displayBoxShift.Text = dr["Shift"].Equals("D") ? "Day" : dr["Shift"].Equals("N") ? "Night" : "Subcon-Out";
                    this.txtLocalSupp1.TextBox1.Text = dr["SubconName"].ToString();
                }
            }
            else
            {
                this.displayApvDate.Text = string.Empty;
                this.displayBoxShift.Text = string.Empty;
                this.txtLocalSupp1.TextBox1.Text = string.Empty;
            }

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
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1

                // .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
                // .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
            .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
            ;
            #endregion 欄位設定
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
            StringBuilder sqlupd4 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2, result3;
            DataTable datacheck;
            string sqlupd2_FIO = string.Empty;
            string sqlupd2_B = string.Empty;
            string issuelackid = MyUtility.GetValue.Lookup(string.Format(@"Select issuelackid from dbo.lack WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["requestid"]));
            if (!MyUtility.Check.Empty(issuelackid))
            {
                MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", this.CurrentMaintain["requestid"], issuelackid), "Can't Confirmed");
                return;
            }

            if (this.CurrentMaintain["type"].ToString() == "R")
            {
                #region 檢查物料Location 是否存在WMS
                if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P15"))
                {
                    MyUtility.Msg.WarningBox("Material Location is from WMS system cannot confirmed or unconfirmed. ", "Warning");
                    return;
                }
                #endregion
                #region -- 檢查庫存項lock --
                sqlcmd = string.Format(
                    @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueLack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
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
                if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "IssueLack_Detail"))
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
from dbo.IssueLack_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
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
                #region -- 更新庫存數量  ftyinventory --
                sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);
                #endregion 更新庫存數量  ftyinventory
            }
            #region -- 更新表頭狀態資料 --
            string nowtime = DateTime.Now.ToAppDateTimeFormatString();
            sqlupd3 = string.Format(
                @"update IssueLack set status='Confirmed', editname = '{0}' , editdate = '{2}',apvname = '{0}',apvdate = '{2}'
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"], nowtime);

            #endregion 更新表頭狀態資料
            #region -- update Lack.issuelackid & Lack.issuelackdt

            sqlupd4.Append(string.Format(
                @"update dbo.Lack set dbo.Lack.IssueLackDT='{0}'
, IssueLackId = '{1}' where id = '{2}';", nowtime, this.CurrentMaintain["id"], this.CurrentMaintain["requestid"]));
            sqlupd4.Append(Environment.NewLine);

            sqlupd4.Append(string.Format(
                @"update dbo.Lack_Detail  set IssueQty = t.qty
from (select seq1,seq2,sum(qty) qty from dbo.IssueLack_Detail WITH (NOLOCK) where id='{0}' group by seq1,seq2) t
where dbo.Lack_Detail.id = '{1}' and dbo.Lack_Detail.seq1 = t.Seq1 and dbo.Lack_Detail.seq2 = t.Seq2", this.CurrentMaintain["id"], this.CurrentMaintain["requestid"]));

            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (this.CurrentMaintain["type"].ToString() == "R")
                    {
                        DataTable resulttb;
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
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb,
                            "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(
                            (DataTable)this.detailgridbs.DataSource, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result3 = DBProxy.Current.Execute(null, sqlupd4.ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd4.ToString(), result3);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }

                // AutoWHACC WebAPI for Vstrong
                if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
                {
                    DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                    Task.Run(() => new Vstrong_AutoWHAccessory().SentIssue_Detail_New(dtDetail, "P15", "New"))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
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
            DualResult result, result2, result3;
            string sqlupd2_FIO = string.Empty;
            string sqlupd2_B = string.Empty;

            if (this.CurrentMaintain["type"].ToString() == "R")
            {
                #region 檢查物料Location 是否存在WMS
                if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P15"))
                {
                    MyUtility.Msg.WarningBox("Material Location is from WMS system cannot confirmed or unconfirmed. ", "Warning");
                    return;
                }
                #endregion

                #region -- 檢查庫存項lock --
                sqlcmd = string.Format(
                    @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueLack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
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
                if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "IssueLack_Detail"))
                {
                    return;
                }
                #endregion

                #region -- 檢查負數庫存 --

                sqlcmd = string.Format(
                    @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueLack_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
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

                #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
                if (!Prgs.ChkWMSCompleteTime(dt, "IssueLack_Detail"))
                {
                    return;
                }
                #endregion

                #region UnConfirmed 先檢查WMS是否傳送成功
                if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
                {
                    DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                    if (!Vstrong_AutoWHAccessory.SentIssue_Detail_Delete(dtDetail, "P15", "UnConfirmed"))
                    {
                        return;
                    }
                }
                #endregion

                #region -- 更新庫存數量  ftyinventory --
                sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);
                #endregion 更新庫存數量  ftyinventory
            }
            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"update IssueLack set status='New', editname = '{0}' , editdate = GETDATE(),apvname = '',apvdate = null
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- update Lack.issuelackid & Lack.issuelackdt
            sqlupd4.Append(string.Format(
                @"update dbo.Lack set dbo.Lack.IssueLackDT= DEFAULT
, IssueLackId = DEFAULT where id = '{2}';", this.CurrentMaintain["issuedate"], this.CurrentMaintain["id"], this.CurrentMaintain["requestid"]));
            sqlupd4.Append(Environment.NewLine);
            sqlupd4.Append(string.Format(@"update dbo.Lack_Detail  set IssueQty = DEFAULT where dbo.Lack_Detail.id = '{0}' ", this.CurrentMaintain["requestid"]));
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (this.CurrentMaintain["type"].ToString() == "R")
                    {
                        DataTable resulttb;
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

                        var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                                     select new
                                     {
                                         poid = m.Field<string>("poid"),
                                         seq1 = m.Field<string>("seq1"),
                                         seq2 = m.Field<string>("seq2"),
                                         stocktype = m.Field<string>("stocktype"),
                                         qty = -m.Field<decimal>("qty"),
                                         roll = m.Field<string>("roll"),
                                         dyelot = m.Field<string>("dyelot"),
                                     }).ToList();
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb,
                            "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }

                        DataTable dx = (DataTable)this.detailgridbs.DataSource;
                        if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result3 = DBProxy.Current.Execute(null, sqlupd4.ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd4.ToString(), result3);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickClose()
        {
            base.ClickClose();
            string sqlcmd;
            sqlcmd = string.Format(
                "update IssueLack set status = 'Closed' , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnclose()
        {
            base.ClickUnclose();
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unclose it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            string sqlcmd;
            sqlcmd = string.Format(
                "update IssueLack set status = 'Confirmed' , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
--,a.Roll
--,a.Dyelot
,p1.stockunit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Qty
,a.StockType
,dbo.Getlocation(f.Ukey)  as location
,a.ukey
,a.FtyInventoryUkey
from dbo.IssueLack_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory f WITH (NOLOCK) on a.POID=f.POID and a.Seq1=f.Seq1 and a.Seq2=f.Seq2 and a.Roll=f.Roll and a.Dyelot=f.Dyelot and a.StockType=f.StockType
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["requestid"]))
            {
                MyUtility.Msg.WarningBox("Request# can't be empty, Please fill it first!!");
                return;
            }

            // string aa = comboBox1.Text;
            var frm = new P15_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.comboType.Text, "P15_Import");
            frm.P15 = this;
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P15_AccumulatedQty(this.CurrentMaintain);
            frm.P15 = this;
            frm.ShowDialog(this);
        }

        // Unfinish
        private void Unfinish(object sender, EventArgs e)
        {
            var frm = new P15_Unfinish(P15_Unfinish.TypeAccessory, "P15_Unfinish");
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

        // Request ID
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void TxtRequest_Validating(object sender, CancelEventArgs e)
        {
            DataRow dr;
            if (!MyUtility.Check.Seek(
                string.Format(
                @"select [type],[apvdate],[issuelackid],[Shift],[SubconName] from dbo.lack WITH (NOLOCK) 
where id='{0}' and fabrictype='A' and mdivisionid='{1}'",
                this.txtRequest.Text, Env.User.Keyword), out dr, null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Please check requestid is Accessory.", "Data not found!!");
                this.txtRequest.Text = string.Empty;
                return;
            }
            else
            {
                if (MyUtility.Check.Empty(dr["apvdate"]))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Request is not approved!!");
                    return;
                }

                if (!MyUtility.Check.Empty(dr["issuelackid"]))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", this.txtRequest.Text, dr["issuelackid"]));
                    return;
                }
            }

            this.CurrentMaintain["requestid"] = this.txtRequest.Text;
            this.CurrentMaintain["type"] = dr["type"].ToString();
            this.displayApvDate.Text = ((DateTime)dr["apvdate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
            this.displayBoxShift.Text = dr["Shift"].Equals("D") ? "Day" : dr["Shift"].Equals("N") ? "Night" : "Subcon-Out";
            this.txtLocalSupp1.TextBox1.Text = dr["SubconName"].ToString();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["Status"].ToString() != "Confirmed")
            {
                this.ShowErr("Data is not confirmed, can't print");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = row["Remark"].ToString();
            string requestid = row["Requestid"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string appvdate = MyUtility.Check.Empty(this.displayApvDate.Text) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.displayApvDate.Text)).ToString("yyyy/MM/dd HH:mm:ss");
            string confirmDate = MyUtility.Convert.GetDate(this.CurrentMaintain["ApvDate"]).HasValue ? MyUtility.Convert.GetDate(this.CurrentMaintain["ApvDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            DataTable dt;
            string cmd = @"
select NameEn
from MDivision
where id = @MDivision";
            DualResult result = DBProxy.Current.Select(string.Empty, cmd, pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dt");
                return false;
            }

            string rptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("Requestid", requestid));
            report.ReportParameters.Add(new ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new ReportParameter("appvdate", appvdate));
            report.ReportParameters.Add(new ReportParameter("ConfirmDate", confirmDate));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;

            string cmdText = @"
select a.POID
    ,a.Seq1+'-'+a.seq2 as SEQ
	,IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
		AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
		AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
		,'',dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))[DESC]
	,unit = b.StockUnit
	,ReqQty=c.Requestqty 
	,a.Qty
    ,[Location]=dbo.Getlocation(fi.ukey)
    ,[Total]=sum(a.Qty) OVER (PARTITION BY a.POID ,a.Seq1,a.Seq2 )	        
from dbo.IssueLack_Detail a WITH (NOLOCK) 
left join dbo.IssueLack d WITH (NOLOCK) on d.id=a.ID
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
left join dbo.Lack_Detail c WITH (NOLOCK) on c.id=d.RequestID and c.Seq1=a.Seq1 and c.Seq2=a.Seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2 and a.Dyelot = fi.Dyelot
    and a.roll = fi.roll and a.stocktype = fi.stocktype
where a.id= @ID";

            result = DBProxy.Current.Select(string.Empty, cmdText, pars, out dd);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dd == null || dd.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dd");
                return false;
            }

            // 傳 list 資料
            List<P15_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P15_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["DESC"].ToString().Trim(),
                    Unit = row1["unit"].ToString().Trim(),
                    ReqQty = row1["ReqQty"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC

            // DualResult result;
            Type reportResourceNamespace = typeof(P15_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P15_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();

            return true;
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P15", this);
        }
    }
}