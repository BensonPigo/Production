using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
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
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P16 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        /// <inheritdoc/>
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(this.viewer);

            // MDivisionID 是 P16 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("FabricType='F' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P16(ToolStripMenuItem menuitem, string transID, string callFrom = "")
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("FabricType='F' and id='{0}'", transID);
            if (!callFrom.Equals("PPIC.P16"))
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
                if (form is P16)
                {
                    form.Activate();
                    P16 activateForm = (P16)form;
                    return;
                }
            }

            ToolStripMenuItem p16MenuItem = null;
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
                                        if (((ToolStripMenuItem)endMenuItem).Text.EqualString("P16. Issue Fabric Lacking  && Replacement"))
                                        {
                                            p16MenuItem = (ToolStripMenuItem)endMenuItem;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            P16 call = new P16(p16MenuItem, pPIC_id, "PPIC.P16")
            {
                MdiParent = mdiParent,
            };
            call.Show();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 新增Batch Shipment Finished按鈕
            Win.UI.Button btnUnFinish = new Win.UI.Button
            {
                Text = "UnFinish",
            };
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
            Dictionary<string, string> di_type = new Dictionary<string, string>
            {
                { "L", "Lacking" },
                { "R", "Replacement" },
            };
            this.comboBox1.DataSource = new BindingSource(di_type, null);
            this.comboBox1.ValueMember = "Key";
            this.comboBox1.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["FabricType"] = "F";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
            this.txtLocalSupp1.TextBox1.ReadOnly = true;
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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();
            #region -- 必輸檢查 --

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateBox3.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Requestid"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                this.txtRequestNo.Focus();
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

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IF", "IssueLack", (DateTime)this.CurrentMaintain["Issuedate"]);
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
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (this.CurrentMaintain["status"].ToString() == "Confirmed")
            {
                this.btnPrintFabricSticker.Enabled = true;
            }
            else
            {
                this.btnPrintFabricSticker.Enabled = false;
            }

            // Lack.ApvDate
            if (!MyUtility.Check.Empty(MyUtility.GetValue.Lookup(string.Format(@"select apvdate from lack WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["requestid"]))))
            {
                if (MyUtility.Check.Seek(string.Format(@"select apvdate,Shift,SubconName,Dept from lack WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["requestid"]), out DataRow dr))
                {
                    this.displayApvDate.Text = ((DateTime)dr["apvdate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
                    this.displayBoxShift.Text = dr["Shift"].Equals("D") ? "Day" : dr["Shift"].Equals("N") ? "Night" : "Subcon-Out";
                    this.txtLocalSupp1.TextBox1.Text = dr["SubconName"].ToString();
                    this.displayDept.Text = dr["Dept"].ToString();
                }
            }
            else
            {
                this.displayApvDate.Text = string.Empty;
                this.displayBoxShift.Text = string.Empty;
                this.txtLocalSupp1.TextBox1.Text = string.Empty;
            }

            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED"))
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
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
            .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true);
            #endregion 欄位設定
        }

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
                #region -- 檢查庫存項lock --
                sqlcmd = string.Format(
                    @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueLack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
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
                            ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
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
            sqlupd3 = $@"update IssueLack set status='Confirmed', editname = '{Env.User.UserID}' , editdate = '{nowtime}',apvname = '{Env.User.UserID}',apvdate = '{nowtime}' where id = '{this.CurrentMaintain["id"]}'";

            #endregion 更新表頭狀態資料
            #region -- update Lack.issuelackid & Lack.issuelackdt
            sqlupd4.Append($@"update dbo.Lack set dbo.Lack.IssueLackDT='{nowtime}'
, IssueLackId = '{this.CurrentMaintain["id"]}' where id = '{this.CurrentMaintain["requestid"]}';");
            sqlupd4.Append(Environment.NewLine);

            sqlupd4.Append($@"
update dbo.Lack_Detail  
set IssueQty = t.qty
from (
    select seq1
           , seq2
           , sum(qty) qty 
    from dbo.IssueLack_Detail WITH (NOLOCK) 
    where id = '{this.CurrentMaintain["id"]}' 
    group by seq1, seq2
) t
where dbo.Lack_Detail.id = '{this.CurrentMaintain["requestid"]}' 
      and dbo.Lack_Detail.seq1 = t.Seq1 
      and dbo.Lack_Detail.seq2 = t.Seq2");
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (this.CurrentMaintain["type"].ToString() == "R")
                    {
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
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out DataTable resulttb, "#TmpSource")))
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
                    this.FtyBarcodeData(true);

                    // AutoWH Fabric WebAPI for Gensong
                    if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                    {
                        DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                        Task.Run(() => new Gensong_AutoWHFabric().SentIssue_Detail_New(dtDetail, "P16"))
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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
            StringBuilder sqlupd4 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2, result3;
            string sqlupd2_FIO = string.Empty;
            string sqlupd2_B = string.Empty;

            if (this.CurrentMaintain["type"].ToString() == "R")
            {
                #region -- 檢查庫存項lock --
                sqlcmd = string.Format(
                    @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueLack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
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
                #region 檢查庫存項WMSLock
                if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "IssueLack_Detail"))
                {
                    return;
                }
                #endregion

                #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
                if (!Prgs.ChkWMSCompleteTime(dt, "IssueLack_Detail"))
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
                            ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
                        }

                        MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                        return;
                    }
                }

                #endregion 檢查負數庫存
                #region UnConfirmed 先檢查WMS是否傳送成功

                if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                {
                    DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                    if (!Gensong_AutoWHFabric.SentIssue_Detail_Delete(dtDetail, "P16", "UnConfirmed"))
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

            sqlupd3 = $@"update IssueLack set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE(),apvname = '',apvdate = null
where id = '{this.CurrentMaintain["id"]}'";

            #endregion 更新表頭狀態資料
            #region -- update Lack.issuelackid & Lack.issuelackdt
            sqlupd4.Append($@"update dbo.Lack set dbo.Lack.IssueLackDT= DEFAULT
, IssueLackId = DEFAULT where id = '{this.CurrentMaintain["requestid"]}';");
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

                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out DataTable resulttb, "#TmpSource")))
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]

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
from Production.dbo.Issuelack_Detail i2
inner join Production.dbo.Issuelack i on i2.Id=i.Id 
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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickClose()
        {
            base.ClickClose();
            string sqlcmd = $"update IssueLack set status = 'Closed' , editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'";

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
            sqlcmd = $"update IssueLack set status = 'Confirmed' , editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'";

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select a.id
	   , a.PoId
	   , a.Seq1
	   , a.Seq2
	   , seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
	   , a.Roll
	   , a.Dyelot
	   , p1.stockunit
	   , [Description] = dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) 
	   , a.Qty
	   , a.StockType
	   , location = dbo.Getlocation(f.Ukey) 
	   , a.ukey
	   , a.FtyInventoryUkey
       , a.Remark
from dbo.IssueLack_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId 
											 and p1.seq1 = a.SEQ1 
											 and p1.SEQ2 = a.seq2
left join FtyInventory f WITH (NOLOCK) on a.POID = f.POID 
										  and a.Seq1 = f.Seq1 
										  and a.Seq2 = f.Seq2 
										  and a.Roll = f.Roll 
										  and a.Dyelot = f.Dyelot 
										  and a.StockType = f.StockType
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

            var frm = new P16_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.comboBox1.Text, "P16_Import")
            {
                P16 = this,
            };
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P16_AccumulatedQty(this.CurrentMaintain)
            {
                P16 = this,
            };
            frm.ShowDialog(this);
        }

        // Unfinish
        private void Unfinish(object sender, EventArgs e)
        {
            var frm = new P15_Unfinish(P15_Unfinish.TypeFabric, "P16_Unfinish");
            frm.ShowDialog(this);
        }

        // Locate for (find)
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("poid", this.textBox1.Text.TrimEnd());
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
        private void TxtRequestNo_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Seek(
                $@"select [type],[apvdate],[issuelackid],[Shift],[SubconName] from dbo.lack WITH (NOLOCK) 
where id='{this.txtRequestNo.Text}' and fabrictype='F' and mdivisionid='{Env.User.Keyword}'", out DataRow dr))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Please check requestid is Fabric.", "Data not found!!");
                this.txtRequestNo.Text = string.Empty;
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
                    MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", this.txtRequestNo.Text, dr["issuelackid"]));
                    return;
                }
            }

            this.CurrentMaintain["requestid"] = this.txtRequestNo.Text;
            this.CurrentMaintain["type"] = dr["type"].ToString();
            this.displayApvDate.Text = ((DateTime)dr["apvdate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
            this.displayBoxShift.Text = dr["Shift"].Equals("D") ? "Day" : dr["Shift"].Equals("N") ? "Night" : "Subcon-Out";
            this.txtLocalSupp1.TextBox1.Text = dr["SubconName"].ToString();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string requestno = row["requestid"].ToString();
            string remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
                new SqlParameter("@MDivision", Env.User.Keyword),
            };
            DualResult result = DBProxy.Current.Select(string.Empty, @"select NameEN from MDivision where id = @MDivision", pars, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            string rptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Request", requestno));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            report.ReportParameters.Add(new ReportParameter("Dept", this.displayDept.Text));

            string sqlcmd = @"
select b.ApvDate
from dbo.Issuelack  a WITH (NOLOCK) 
left join dbo.lack  b WITH (NOLOCK) 
on b.id = a.requestid
where b.id = a.requestid
and a.id = @ID";
            DualResult apvResult = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtApv);

            if (!apvResult)
            {
                this.ShowErr(apvResult);
            }

            if (dtApv == null || dtApv.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtApv");
                return false;
            }

            string apvDate = MyUtility.Check.Empty(dtApv.Rows[0]["ApvDate"]) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(dtApv.Rows[0]["ApvDate"])).ToString("yyyy/MM/dd HH:mm:ss");
            report.ReportParameters.Add(new ReportParameter("ApvDate", apvDate));

            string confirmDate = MyUtility.Convert.GetDate(this.CurrentMaintain["ApvDate"]).HasValue ? MyUtility.Convert.GetDate(this.CurrentMaintain["ApvDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;
            report.ReportParameters.Add(new ReportParameter("ConfirmDate", confirmDate));

            #endregion

            #region -- 撈表身資料 --
            pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
            };
            sqlcmd = @"
select  a.POID
        ,a.Seq1+'-'+a.seq2 as SEQ
        ,a.Roll
        ,a.Dyelot 
        ,[Description] =IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
					AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
					AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
					,''
					,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))
		,MDesc = 'Relaxation Type：'+(select FabricRelaxationID from [dbo].[SciMES_RefnoRelaxtime] where Refno = b.Refno)
        ,b.StockUnit
        ,a.Qty
        ,dbo.Getlocation(fi.ukey)[Location] 
        ,a.Remark
from dbo.IssueLack_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
where a.id= @ID";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtDetail);

            if (!result)
            {
                this.ShowErr(result);
            }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtDetail");
                return false;
            }

            // 傳 list 資料
            List<P16_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P16_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    DYELOT = row1["DYELOT"].ToString().Trim(),
                    DESC = row1["Description"].ToString().Trim(),
                    MDESC = row1["MDesc"].ToString().Trim(),
                    StockUnit = row1["StockUnit"].ToString().Trim(),
                    QTY = Convert.ToDecimal(row1["QTY"]),
                    Location = row1["Location"].ToString().Trim(),
                    Remark = row1["Remark"].ToString().Trim(),
                }).ToList();
            #endregion
            report.ReportDataSource = data;

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P16_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P16_Print.rdlc";

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

            return true;
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P13_FabricSticker(this.CurrentMaintain["ID"], "Issuelack_Detail").ShowDialog();
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P16", this);
        }
    }
}