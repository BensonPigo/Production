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
    public partial class P23 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P23(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {
            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "B";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            // 從DB取得最新Status, 避免多工時, 畫面上資料不是最新的狀況
            this.RenewData();
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");

                // 重新整理畫面
                this.OnRefreshClick();
                return false;
            }

            // 重新整理畫面
            this.OnRefreshClick();
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

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append(string.Format("SP#: {0} Seq#: {1}-{2} can't be empty", row["frompoid"], row["fromseq1"], row["fromseq2"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format("SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty", row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "PI", "SubTransfer", (DateTime)this.CurrentMaintain["Issuedate"]);
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
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (this.CurrentMaintain["status"].ToString().EqualString("Confirmed"))
            {
                this.btnPrintFabricSticker.Enabled = true;
            }
            else
            {
                this.btnPrintFabricSticker.Enabled = false;
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
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;
            Ict.Win.UI.DataGridViewTextBoxColumn from_ContainerCode;
            Ict.Win.UI.DataGridViewTextBoxColumn to_ContainerCode;

            #region -- To Location 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.CurrentDetailData["tostocktype"].ToString(), this.CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK)
WHERE   StockType='{0}'
        and junk != '1'", this.CurrentDetailData["tostocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["tolocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["tolocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("frompoid", header: "Inventory" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("fromseq", header: "Inventory" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)            
            .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Size", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 5
            .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
            .Text("FromContainerCode", header: "From" + Environment.NewLine + "Container Code", iseditingreadonly: true).Get(out from_ContainerCode)
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns).Get(out col_Qty) // 7
            .Text("topoid", header: "Bulk" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 8
            .Text("toseq", header: "Bulk" + Environment.NewLine + " Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 9
            .Text("toLocation", header: "To" + Environment.NewLine + "Location", settings: ts2, iseditingreadonly: false).Get(out col_tolocation) // 10
            .Text("ToContainerCode", header: "To" + Environment.NewLine + "Container Code", iseditingreadonly: true).Get(out to_ContainerCode)
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            from_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            to_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
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
            if (!(result = Prgs.P23confirm(this.CurrentMaintain["ID"].ToString(), (DataTable)this.detailgridbs.DataSource)))
            {
                this.ShowErr(result);
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
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        , f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.toPoId = f.PoId
                                            and d.toSeq1 = f.Seq1
                                            and d.toSeq2 = f.seq2
                                            and d.toStocktype = f.StockType
                                            and d.toRoll = f.Roll
                                            and d.toDyelot = f.Dyelot
where   f.lock = 1 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["topoid"]} Seq#: {tmp["toseq1"]}-{tmp["toseq2"]} Roll#: {tmp["toroll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "SubTransfer_Detail_To"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "SubTransfer_Detail_To"))
            {
                return;
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        , d.toDyelot
from (
		Select  topoid
                , toseq1
                , toseq2
                , toRoll
                , Qty = sum(Qty)
                , toStocktype
                , toDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty > 0
		Group by topoid, toseq1, toseq2, toRoll, toStocktype, toDyelot
) as d
left join FtyInventory f WITH (NOLOCK) on   d.toPoId = f.PoId 
                                            and d.toSeq1 = f.Seq1 
                                            and d.toSeq2 = f.seq2 
                                            and d.toStocktype = f.StockType 
                                            and d.toRoll = f.Roll
                                            and d.toDyelot = f.Dyelot
where (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0) + d.Qty < 0) ", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["topoid"]} Seq#: {tmp["toseq1"]}-{tmp["toseq2"]} Roll#: {tmp["toroll"]} Dyelot: {tmp["toDyelot"]}'s balance: {tmp["balanceqty"]} is less than transfer qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Bulk balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            sqlcmd = string.Format(
                @"
Select  d.frompoid
        , d.fromseq1
        , d.fromseq2
        , d.fromRoll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        , d.FromDyelot
from (
		Select  frompoid
                , fromseq1
                , fromseq2
                , fromRoll
                , Qty = sum (Qty)
                , FromStockType
                , FromDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty < 0
		Group by frompoid, fromseq1, fromseq2, fromRoll, FromStockType, FromDyelot
) as d 
left join FtyInventory f WITH (NOLOCK) on   d.FromPOID = f.POID 
                                            and d.FromRoll = f.Roll 
                                            and d.FromSeq1 =f.Seq1 
                                            and d.FromSeq2 = f.Seq2 
                                            AND D.FromStockType = F.StockType
                                            and d.FromDyelot = f.Dyelot
where (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0) < -d.Qty) ", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["frompoid"]} Seq#: {tmp["fromseq1"]}-{tmp["fromseq2"]} Roll#: {tmp["fromroll"]} Dyelot: {tmp["FromDyelot"]}'s balance: {tmp["balanceqty"]} is less than transfer qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Inventory balance Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查InQty - 轉倉數量 + 調整數量 < 轉出數量, 則不能unconfirmed
            sqlcmd = string.Format(
                @"
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        , d.toDyelot
from (
		Select  topoid
                , toseq1
                , toseq2
                , toRoll
                , Qty = sum(Qty)
                , toStocktype
                , toDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty > 0
		Group by topoid, toseq1, toseq2, toRoll, toStocktype, toDyelot
) as d
left join FtyInventory f WITH (NOLOCK) on   d.toPoId = f.PoId 
                                            and d.toSeq1 = f.Seq1 
                                            and d.toSeq2 = f.seq2 
                                            and d.toStocktype = f.StockType 
                                            and d.toRoll = f.Roll
                                            and d.toDyelot = f.Dyelot
where (isnull(f.InQty, 0) - d.Qty + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)) < isnull (f.OutQty, 0) 
  ", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    ids = string.Empty;
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"Bulk SP#: {tmp["topoid"]} {tmp["toseq1"]}-{tmp["toseq2"]} already exceeded the release qty({tmp["qty"]}), cann't be unconfirm!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox(ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 更新MdivisionPoDetail B倉數量 --
            var data_MD_4F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("frompoid"),
                                  seq1 = b.Field<string>("fromseq1"),
                                  seq2 = b.Field<string>("fromseq2"),
                                  stocktype = b.Field<string>("fromstocktype"),
                              }
                        into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("frompoid"),
                                  Seq1 = m.First().Field<string>("fromseq1"),
                                  Seq2 = m.First().Field<string>("fromseq2"),
                                  Stocktype = m.First().Field<string>("fromstocktype"),
                                  Qty = -m.Sum(w => w.Field<decimal>("qty")),
                              }).ToList();

            var data_MD_8F = data_MD_4F.Select(data => new Prgs_POSuppDetailData
            {
                Poid = data.Poid,
                Seq1 = data.Seq1,
                Seq2 = data.Seq2,
                Stocktype = data.Stocktype,
                Qty = -data.Qty,
            }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail A倉數 --
            var data_MD_2F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("Topoid"),
                                  seq1 = b.Field<string>("Toseq1"),
                                  seq2 = b.Field<string>("Toseq2"),
                                  stocktype = b.Field<string>("Tostocktype"),
                              }
                        into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("Topoid"),
                                  Seq1 = m.First().Field<string>("Toseq1"),
                                  Seq2 = m.First().Field<string>("Toseq2"),
                                  Stocktype = m.First().Field<string>("Tostocktype"),
                                  Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                              }).ToList();

            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               group m by new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot"),
                               }
                                into g
                               select new
                               {
                                   poid = g.Key.poid,
                                   seq1 = g.Key.seq1,
                                   seq2 = g.Key.seq2,
                                   stocktype = g.Key.stocktype,
                                   qty = -g.Sum(m => m.Field<decimal>("qty")),
                                   location = g.Key.location,
                                   roll = g.Key.roll,
                                   dyelot = g.Key.dyelot,
                               }).ToList();

            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("topoid"),
                                   seq1 = m.Field<string>("toseq1"),
                                   seq2 = m.Field<string>("toseq2"),
                                   stocktype = m.Field<string>("tostocktype"),
                                   qty = -m.Field<decimal>("qty"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("toroll"),
                                   dyelot = m.Field<string>("todyelot"),
                               }).ToList();
            string upd_Fty_4F = Prgs.UpdateFtyInventory_IO(4, null, false);
            string upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

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
                        DataTable resulttb;
                        #region FtyInventory
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F, string.Empty, upd_Fty_4F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        #region MDeivisionPoDetail
                        string upd_MD_4F = Prgs.UpdateMPoDetail(4, null, false, sqlConn: sqlConn);
                        string upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn);
                        string upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F, string.Empty, upd_MD_4F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        if (!(result = DBProxy.Current.Execute(null, $"update SubTransfer set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        // 在更新 FtyInventory 之後, 更新 SubTransfer_Detail.FromBalanceQty = (From)FtyInventory 剩餘量
                        if (!(result = Prgs.UpdateSubTransfer_DetailFromBalanceQty(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), false)))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
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
select  [Selected] = 0 
        , a.id    
        , a.FromPoId
        , a.FromSeq1
        , a.FromSeq2
        , Fromseq = concat (Ltrim (Rtrim (a.FromSeq1)), ' ', a.FromSeq2)
        , p1.FabricType
        , stockunit = dbo.GetStockUnitBySPSeq (p1.ID, p1.seq1, p1.seq2)
        , [description] = dbo.getmtldesc (a.FromPoId, a.FromSeq1, a.FromSeq2, 2, 0)
        , p1.Refno
        , [Size] = psdsS.SpecValue
        , a.FromRoll
        , a.FromDyelot
        , fi.Tone
        , a.FromStocktype
        , a.Qty
        , a.ToPoid
        , a.ToSeq1
        , a.ToSeq2
        , toseq = concat (Ltrim (Rtrim (a.ToSeq1)), ' ', a.ToSeq2)
        , a.ToRoll
        , a.ToDyelot
        , a.ToStocktype
        , a.ToLocation
        , a.ToContainerCode
        , Fromlocation = Fromlocation.listValue
        , [FromContainerCode] = fi.ContainerCode
        , a.fromftyinventoryukey
        , a.ukey
        , location = dbo.Getlocation (fi.ukey)
        ,[Color] = Color.Value
from dbo.SubTransfer_detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId 
                                             and p1.seq1 = a.FromSeq1 
                                             and p1.SEQ2 = a.FromSeq2
left join FtyInventory fi on a.FromPoid = fi.poid 
                             and a.fromSeq1 = fi.seq1 
                             and a.fromSeq2 = fi.seq2
                             and a.fromRoll = fi.roll 
                             and a.fromStocktype = fi.stocktype
                             and a.fromDyelot = fi.Dyelot
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = p1.id and psdsC.seq1 = p1.seq1 and psdsC.seq2 = p1.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = p1.id and psdsS.seq1 = p1.seq1 and psdsS.seq2 = p1.seq2 and psdsS.SpecColumnID = 'Size'
left join orders o on o.id = p1.id
left join Fabric f WITH (NOLOCK) on f.SCIRefno = p1.SCIRefno

outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select 	distinct
						fd.MtlLocationID
					from FtyInventory_Detail fd
					left join MtlLocation ml on ml.ID = fd.MtlLocationID
					where fd.Ukey = fi.Ukey
					and ml.Junk = 0 
					and ml.StockType = a.ToStockType
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
OUTER APPLY(
 SELECT [Value]=
	CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(p1.SuppColor = '' or p1.SuppColor is null, dbo.GetColorMultipleID(o.BrandID, psdsC.SpecValue), p1.SuppColor)
	ELSE dbo.GetColorMultipleID(o.BrandID, psdsC.SpecValue)
	END
)Color
Where a.id = '{0}'
", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P23_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P23_AccumulatedQty(this.CurrentMaintain);
            frm.P23 = this;
            frm.ShowDialog(this);
        }

        // Locate
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("frompoid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
            }
            else
            {
                WH_FromTo_Print p = new WH_FromTo_Print(this.CurrentMaintain, "P23")
                {
                    CurrentDataRow = this.CurrentMaintain,
                };

                p.ShowDialog();

                // 代表要列印 RDLC
                if (p.IsPrintRDLC)
                {
                    if (!MyUtility.Check.Seek($"select NameEN from MDivision where id = '{Env.User.Keyword}'", out DataRow dr))
                    {
                        MyUtility.Msg.WarningBox("Data not found!", "Title");
                        return false;
                    }

                    ReportDefinition report = new ReportDefinition();
                    report.ReportParameters.Add(new ReportParameter("RptTitle", MyUtility.Convert.GetString(dr["NameEN"])));
                    report.ReportParameters.Add(new ReportParameter("ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                    report.ReportParameters.Add(new ReportParameter("Remark", MyUtility.Convert.GetString(this.CurrentMaintain["Remark"])));
                    report.ReportParameters.Add(new ReportParameter("issuedate", ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["issuedate"])).ToShortDateString()));
                    report.ReportParameters.Add(new ReportParameter("Factory", MyUtility.Convert.GetString(this.CurrentMaintain["MdivisionID"])));

                    #region -- 撈表身資料 --
                    string sqlcmd = $@"
select  t.frompoid
        ,t.fromseq1 + '-' +t.fromseq2 as SEQ
        ,t.topoid,t.toseq1  + '-' +t.toseq2 as TOSEQ
        ,[desc] = IIF ((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2) 
                        and (p.seq1 = lag(p.seq1,1,'') over (order by p.ID,p.seq1,p.seq2)) 
                        and (p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))
                       ) 
                       , ''
                       , dbo.getMtlDesc(t.FromPOID,t.FromSeq1,t.FromSeq2,2,0)
                      )
        , t.fromroll
        , t.fromdyelot
        , p.StockUnit
        , [BULKLOCATION] = dbo.Getlocation(fi.ukey) 
        , fi.ContainerCode
        , t.Tolocation
        , t.ToContainerCode
        , t.Qty
        , [Total] = sum(t.Qty) OVER (PARTITION BY t.frompoid ,t.FromSeq1,t.FromSeq2 )         
from dbo.Subtransfer_detail t WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id= t.FromPOID 
                                                and p.SEQ1 = t.FromSeq1 
                                                and p.seq2 = t.FromSeq2 
left join dbo.FtyInventory FI on t.fromPoid = fi.poid 
                                 and t.fromSeq1 = fi.seq1 
                                 and t.fromSeq2 = fi.seq2
                                 and t.fromRoll = fi.roll 
                                 and t.fromStocktype = FI.stocktype
                                 and t.fromDyelot = FI.Dyelot
where t.id = '{this.CurrentMaintain["ID"]}'
order by t.frompoid,SEQ,BULKLOCATION,t.fromroll,t.FromDyelot
";
                    DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtDetail);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return false;
                    }

                    if (dtDetail.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!", "Detail");
                        return false;
                    }

                    // 傳 list 資料
                    report.ReportDataSource = dtDetail.AsEnumerable()
                        .Select(row1 => new P23_PrintData()
                        {
                            StockSP = row1["frompoid"].ToString().Trim(),
                            StockSEQ = row1["SEQ"].ToString().Trim(),
                            IssueSP = row1["topoid"].ToString().Trim(),
                            SEQ = row1["TOSEQ"].ToString().Trim(),
                            DESC = row1["desc"].ToString().Trim(),
                            Roll = row1["fromroll"].ToString().Trim(),
                            DYELOT = row1["fromdyelot"].ToString().Trim(),
                            Unit = row1["StockUnit"].ToString().Trim(),
                            BULKLOCATION = row1["BULKLOCATION"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                            INVENTORYLOCATION = row1["Tolocation"].ToString().Trim() + Environment.NewLine + row1["ToContainerCode"].ToString().Trim(),
                            QTY = Convert.ToDecimal(row1["Qty"].ToString()),
                            TotalQTY = row1["Total"].ToString().Trim(),
                        }).ToList();

                    #endregion

                    result = ReportResources.ByEmbeddedResource(typeof(P23_PrintData), "P23_Print.rdlc", out IReportResource reportresource);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox(result.ToString());
                        return false;
                    }

                    report.ReportResource = reportresource;
                    new Win.Subs.ReportView(report) { MdiParent = this.MdiParent }.Show();
                }
            }

            return true;
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P23_FabricSticker(this.CurrentMaintain["ID"]).ShowDialog();
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P23", this);
        }

        private void BtnUpdateLocation_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            List<DataRow> dataRows = this.DetailDatas.Where(x => x["Selected"].EqualDecimal(1)).ToList();
            foreach (DataRow dr in dataRows)
            {
                dr["ToLocation"] = dr["Fromlocation"];
                dr.EndEdit();
            }
        }
    }
}