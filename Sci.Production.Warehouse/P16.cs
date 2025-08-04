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
    /// <inheritdoc/>
    public partial class P16 : Win.Tems.Input6
    {
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
            this.editBoxPPICRemark.Text = string.Empty;
            this.txtSewingLine.Text = string.Empty;
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
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("stockunit", header: "Unit", iseditingreadonly: true)
                .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)
                .Text("Location", header: "Bulk Location", iseditingreadonly: true)
                .Numeric("RemainingQty", header: "Remaining Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 11, iseditingreadonly: true)
                .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
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
            string sqlcmd = string.Empty;
            string ids = string.Empty;

            string issuelackid = MyUtility.GetValue.Lookup(string.Format(@"Select issuelackid from dbo.lack WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["requestid"]));
            if (!MyUtility.Check.Empty(issuelackid))
            {
                MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", this.CurrentMaintain["requestid"], issuelackid), "Can't Confirmed");
                return;
            }

            if (this.CurrentMaintain["type"].ToString() == "R")
            {
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
                sqlcmd = string.Format(
                    @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueLack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "IssueLack_Detail"))
                {
                    return;
                }
                #endregion
                #region 檢查Location是否為空值
                if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "IssueLack_Detail") == false)
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
            }
            #region -- update Lack.issuelackid & Lack.issuelackdt
            StringBuilder sqlupd4 = new StringBuilder();
            sqlupd4.Append($@"update dbo.Lack set dbo.Lack.IssueLackDT = GetDate(), IssueLackId = '{this.CurrentMaintain["id"]}' where id = '{this.CurrentMaintain["requestid"]}';");
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

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
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
                        string sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out DataTable resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }

                        string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(
                            (DataTable)this.detailgridbs.DataSource, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update IssueLack set status='Confirmed', editname = '{Env.User.UserID}' , editdate =  GetDate(), apvname = '{Env.User.UserID}', apvdate = GetDate() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd4.ToString())))
                    {
                        throw result.GetException();
                    }

                    // 更新完庫存後 RemainingQty
                    string sqlUpdateRemainingQty = $@"
Update sd set
    RemainingQty = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
from IssueLack_Detail sd with(nolock)
inner join Production.dbo.FtyInventory f with(nolock) on f.POID = isnull(sd.PoId, '')
    and f.Seq1 = isnull(sd.Seq1, '')
    and f.Seq2 = isnull(sd.Seq2, '')
    and f.Roll = isnull(sd.Roll, '')
	and f.Dyelot = isnull(sd.Dyelot, '')
    and f.StockType = isnull(sd.StockType, '')
    and sd.id = '{this.CurrentMaintain["ID"]}'
";
                    if (!(result = DBProxy.Current.Execute(null, sqlUpdateRemainingQty)))
                    {
                        throw result.GetException();
                    }

                    transactionscope.Complete();
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
            if (this.CurrentMaintain["type"].ToString() == "R")
            {
                Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            }

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

            // 取得 FtyInventory 資料
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty;
            string ids = string.Empty;

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
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "IssueLack_Detail"))
                {
                    return;
                }
                #endregion

                #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
                if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "IssueLack_Detail"))
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
            }

            #region -- update Lack.issuelackid & Lack.issuelackdt
            StringBuilder sqlupd4 = new StringBuilder();
            sqlupd4.Append($@"update dbo.Lack set dbo.Lack.IssueLackDT= DEFAULT
, IssueLackId = DEFAULT where id = '{this.CurrentMaintain["requestid"]}';");
            sqlupd4.Append(Environment.NewLine);
            sqlupd4.Append(string.Format(@"update dbo.Lack_Detail  set IssueQty = DEFAULT where dbo.Lack_Detail.id = '{0}' ", this.CurrentMaintain["requestid"]));
            #endregion

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (this.CurrentMaintain["type"].ToString() == "R")
            {
                if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
                {
                    return;
                }
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
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
                        string sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out DataTable resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }

                        string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update IssueLack set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE(),apvname = '',apvdate = null where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd4.ToString())))
                    {
                        throw result.GetException();
                    }

                    // 更新完庫存後 RemainingQty
                    string sqlUpdateRemainingQty = $@"
Update sd set
    RemainingQty = 0
from IssueLack_Detail sd with(nolock)
where sd.id = '{this.CurrentMaintain["ID"]}'
";
                    if (!(result = DBProxy.Current.Execute(null, sqlUpdateRemainingQty)))
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

            if (!MyUtility.Check.Empty(errMsg))
            {
                if (this.CurrentMaintain["type"].ToString() == "R")
                {
                    Prgs_WMS.WMSUnLock(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                }

                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            if (this.CurrentMaintain["type"].ToString() == "R")
            {
                Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            }

            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
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
            this.DetailSelectCommand = $@"
SELECT sd.*
	   , SEQ = concat(Ltrim(Rtrim(sd.seq1)), ' ', sd.Seq2)
	   , [Description] = dbo.getMtlDesc(sd.poid,sd.seq1,sd.seq2,2,0) 
	   , psd.stockunit
       , psd.Refno
       , f.ContainerCode
	   , f.Tone
       , [location] = dbo.Getlocation(f.Ukey)
       , [Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, ISNULL(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
FROM IssueLack_Detail sd WITH (NOLOCK) 
INNER JOIN IssueLack s WITH (NOLOCK) on sd.ID = s.ID
INNER JOIN PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = sd.PoId AND psd.seq1 = sd.SEQ1 AND psd.SEQ2 = sd.seq2
INNER JOIN Fabric WITH (NOLOCK)  on Fabric.SCIRefno = psd.SCIRefno
INNER JOIN FtyInventory f WITH (NOLOCK) on sd.POID = f.POID 
										  AND sd.Seq1 = f.Seq1 
										  AND sd.Seq2 = f.Seq2 
										  AND sd.Roll = f.Roll 
										  AND sd.Dyelot = f.Dyelot 
										  AND sd.StockType = f.StockType
LEFT JOIN PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id AND psdsC.seq1 = psd.seq1 AND psdsC.seq2 = psd.seq2 AND psdsC.SpecColumnID = 'Color'
LEFT JOIN Lack l WITH (NOLOCK) on l.POID = sd.POID AND l.ID = s.RequestID
Where sd.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

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

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P16_AccumulatedQty(this.CurrentMaintain)
            {
                P16 = this,
            };
            frm.ShowDialog(this);
        }

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

        private void TxtRequestNo_Validating(object sender, CancelEventArgs e)
        {
            string sqlcmd = $@"
select type,apvdate,issuelackid,Shift,SubconName,Remark
from lack WITH (NOLOCK) 
where id='{this.txtRequestNo.Text}'
and fabrictype='F'
and mdivisionid='{Env.User.Keyword}'
";

            if (!MyUtility.Check.Seek(sqlcmd, out DataRow dr))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Please check requestid is Fabric.", "Data not found!!");
                this.txtRequestNo.Text = string.Empty;
                return;
            }

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

            this.CurrentMaintain["requestid"] = this.txtRequestNo.Text;
            this.CurrentMaintain["type"] = dr["type"].ToString();
            this.displayApvDate.Text = ((DateTime)dr["apvdate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
            this.displayBoxShift.Text = dr["Shift"].Equals("D") ? "Day" : dr["Shift"].Equals("N") ? "Night" : "Subcon-Out";
            this.txtLocalSupp1.TextBox1.Text = dr["SubconName"].ToString();
            this.editBoxPPICRemark.Text = MyUtility.Convert.GetString(dr["Remark"]);
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

            WH_Print p = new WH_Print(this.CurrentMaintain, "P16", true)
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                DataRow row = this.CurrentMaintain;
                string id = row["ID"].ToString();
                string requestno = row["requestid"].ToString();
                string remark = row["Remark"].ToString();
                string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
                string addName = MyUtility.GetValue.Lookup($@"
select p.Name from IssueLack i
left join Pass1 p on p.ID=i.AddName
where i.Id='{row["ID"].ToString()}'");
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
                report.ReportParameters.Add(new ReportParameter("PPICRemark", this.editBoxPPICRemark.Text));
                report.ReportParameters.Add(new ReportParameter("Remark", remark));
                report.ReportParameters.Add(new ReportParameter("issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                report.ReportParameters.Add(new ReportParameter("Dept", this.displayDept.Text));
                report.ReportParameters.Add(new ReportParameter("AddName", addName));
                report.ReportParameters.Add(new ReportParameter("Line", this.txtSewingLine.Text));

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
                string strorderby = string.Empty;
                if (p.IssortbyLocationAndRoll == true)
                {
                    strorderby = $@",dbo.Getlocation(fi.ukey) 
						,TRY_CAST(LEFT(a.Roll, PATINDEX('%[^0-9]%', a.Roll + 'X') - 1) AS INT)
						,TRY_CAST(LEFT(a.Dyelot, PATINDEX('%[^0-9]%', a.Dyelot + 'X') - 1) AS INT)";
                }

                pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                };
                sqlcmd = string.Format(
                    @"
select  a.POID
        ,a.Seq1+'-'+a.seq2 as SEQ
        ,a.Roll
        ,a.Dyelot 
        ,[Description] =IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2 {0}) 
					AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2 {0}))
					AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2 {0}))) 
					,''
					,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))
		,MDesc = 'Relaxation Type：'+(select FabricRelaxationID from [dbo].[SciMES_RefnoRelaxtime] where Refno = b.Refno)
        ,b.StockUnit
        ,a.Qty
        ,dbo.Getlocation(fi.ukey)[Location] 
        ,FI.ContainerCode
        ,a.Remark
        ,FI.Tone
from dbo.IssueLack_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
where a.id= @ID
", strorderby);
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
                        Tone = row1["Tone"].ToString().Trim(),
                        MDESC = row1["MDesc"].ToString().Trim(),
                        StockUnit = row1["StockUnit"].ToString().Trim(),
                        QTY = Convert.ToDecimal(row1["QTY"]),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        Remark = row1["Remark"].ToString().Trim(),
                    }).ToList();
                #endregion
                #region 指定是哪個 RDLC

                report.ReportDataSource = data;

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
                #endregion
            }

            return true;
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P13_FabricSticker(this.CurrentMaintain["ID"], "Issuelack_Detail").ShowDialog();
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (MyUtility.Check.Empty(e.Master["ID"]))
            {
                return null;
            }

            string sqlcmd = $@"select distinct l.Remark,l.SewingLineID
                                from IssueLack il
                                inner join IssueLack_Detail ild with(nolock) on il.id = ild.id
                                left join Lack l with (nolock) on l.POID = ild.POID and l.ID = il.RequestID
                                where il.id ='{MyUtility.Convert.GetString(e.Master["ID"])}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.ErrorBox(MyUtility.Convert.GetString(dualResult.Messages));
                return dualResult;
            }

            DataRow dataRow = dt.Rows[0];
            this.editBoxPPICRemark.Text = MyUtility.Convert.GetString(dataRow["Remark"]);
            this.txtSewingLine.Text = MyUtility.Convert.GetString(dataRow["SewingLineID"]);

            return base.OnRenewDataDetailPost(e);
        }

        private void BtnRequestList_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["requestid"]))
            {
                MyUtility.Msg.WarningBox("Please fill-in the Request# first.");
                return;
            }

            P15P16_ReuqeustList win = new P15P16_ReuqeustList("P16", this.CurrentMaintain);
            win.ShowDialog(this);
        }

        private int ExtractRollSortKey(string roll)
        {
            if (string.IsNullOrWhiteSpace(roll))
            {
                return int.MaxValue;
            }

            // 嘗試擷取開頭的數字部分
            var match = System.Text.RegularExpressions.Regex.Match(roll, @"\d+");

            if (match.Success && int.TryParse(match.Value, out int value))
            {
                return value;
            }

            // 無法解析數字的話放最後
            return int.MaxValue;
        }
    }
}