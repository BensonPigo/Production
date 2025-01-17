using Ict;
using Ict.Win;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P36 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;

            // MDivisionID 是 P36 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("Type='C' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P36(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='C' and id='{0}'", transID);
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
            this.CurrentMaintain["Type"] = "C";
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
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append($"SP#: {row["frompoid"]} Seq#: {row["fromseq1"]}-{row["fromseq2"]} can't be empty" + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format("SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty", row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["fromroll"]) || MyUtility.Check.Empty(row["fromdyelot"])))
                {
                    warningmsg.Append(string.Format("SP#: {0}  Seq#: {1}-{2}  Roll#:{3}  Dyelot:{4} Roll and Dyelot can't be empty", row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = string.Empty;
                    row["todyelot"] = string.Empty;
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword, "SubTransfer", (DateTime)this.CurrentMaintain["Issuedate"]);
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
            #region -- Location 右鍵開窗 --

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(
                        this.CurrentDetailData["tostocktype"].ToString(),
                        this.CurrentDetailData["tolocation"].ToString());
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
            #endregion Location 右鍵開窗

            Ict.Win.UI.DataGridViewTextBoxColumn from_ContainerCode;
            Ict.Win.UI.DataGridViewTextBoxColumn to_ContainerCode;

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) // 4
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 5
                .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
                .Text("location", header: "From Scrap\r\nLocation", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("FromContainerCode", header: "From" + Environment.NewLine + "Container Code", iseditingreadonly: true).Get(out from_ContainerCode)
                .Text("ToLocation", header: "To Inventory\r\nLocation", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(30))
                .Text("ToContainerCode", header: "To" + Environment.NewLine + "Container Code", iseditingreadonly: true).Get(out to_ContainerCode)
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            from_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            to_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
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
            DataTable datacheck;

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P36"))
            {
                return;
            }
            #endregion

            #region -- 檢查庫存項lock --
            bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            if (!mtlAutoLock)
            {
                sqlcmd = string.Format(
                    @"
Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.frompoid = f.POID and d.fromSeq1 = f.Seq1 and d.fromseq2 = f.seq2 and d.fromStockType = f.StockType and d.fromRoll = f.Roll and d.fromDyelot = f.Dyelot
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
                            ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!", tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["Dyelot"]) + Environment.NewLine;
                        }

                        MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                        return;
                    }
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "SubTransfer_Detail_From"))
            {
                return;
            }
            #endregion

            #region 檢查From/To Location是否為空值
            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                // From Location
                DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;
                DataRow[] dtArry = dtDetail.Select(@"Fromlocation = '' or Fromlocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtFromLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtFromLocation_Empty.Columns["FromPoId"].ColumnName = "SP#";
                    dtFromLocation_Empty.Columns["Fromseq"].ColumnName = "Seq";
                    dtFromLocation_Empty.Columns["FromRoll"].ColumnName = "Roll";
                    dtFromLocation_Empty.Columns["FromDyelot"].ColumnName = "Dyelot";

                    Prgs.ChkLocationEmpty(dtFromLocation_Empty, "From", "SP#,Seq,Roll,Dyelot");
                    return;
                }

                // To Location
                dtArry = dtDetail.Select(@"ToLocation = '' or ToLocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtToLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtToLocation_Empty.Columns["ToPoid"].ColumnName = "SP#";
                    dtToLocation_Empty.Columns["toseq"].ColumnName = "Seq";
                    dtToLocation_Empty.Columns["ToRoll"].ColumnName = "Roll";
                    dtToLocation_Empty.Columns["ToDyelot"].ColumnName = "Dyelot";

                    Prgs.ChkLocationEmpty(dtToLocation_Empty, "To", "SP#,Seq,Roll,Dyelot");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.frompoid = f.POID and d.fromSeq1 = f.Seq1 and d.fromseq2 = f.seq2 and d.fromStockType = f.StockType and d.fromRoll = f.Roll and d.fromDyelot = f.Dyelot
where (isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["frompoid"]} Seq#: {tmp["fromseq1"]}-{tmp["fromseq2"]} Roll#: {tmp["fromroll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_4T16T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                                 group b by new
                                 {
                                     poid = b.Field<string>("frompoid"),
                                     seq1 = b.Field<string>("fromseq1"),
                                     seq2 = b.Field<string>("fromseq2"),
                                     stocktype = b.Field<string>("fromstocktype"),
                                 }
                        into m
                                 select new
                                 {
                                     poid = m.First().Field<string>("frompoid"),
                                     Seq1 = m.First().Field<string>("fromseq1"),
                                     Seq2 = m.First().Field<string>("fromseq2"),
                                     Stocktype = m.First().Field<string>("fromstocktype"),
                                     Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                 }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail Inventory 數 --
            var data_MD_8T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("topoid"),
                                  seq1 = b.Field<string>("toseq1"),
                                  seq2 = b.Field<string>("toseq2"),
                                  stocktype = b.Field<string>("tostocktype"),
                              }
                    into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("topoid"),
                                  Seq1 = m.First().Field<string>("toseq1"),
                                  Seq2 = m.First().Field<string>("toseq2"),
                                  Stocktype = m.First().Field<string>("tostocktype"),
                                  Qty = m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                              }).ToList();

            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4T = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   qty = m.Field<decimal>("qty"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot"),
                               }).ToList();

            var data_Fty_2T = (from b in this.DetailDatas
                               select new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype"),
                                   qty = b.Field<decimal>("qty"),
                                   location = b.Field<string>("ToLocation"),
                                   roll = b.Field<string>("toroll"),
                                   dyelot = b.Field<string>("todyelot"),
                               }).ToList();
            string upd_Fty_4T = Prgs.UpdateFtyInventory_IO(4, null, true);
            string upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量  ftyinventory

            Exception errMsg = null;
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
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, string.Empty, upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        #region MDivisionPoDetail
                        StringBuilder upd_MD_4T = new StringBuilder();
                        StringBuilder upd_MD_16T = new StringBuilder();
                        upd_MD_4T.Append(Prgs.UpdateMPoDetail(4, null, true, sqlConn: sqlConn));
                        upd_MD_16T.Append(Prgs.UpdateMPoDetail(16, null, true, sqlConn: sqlConn));
                        string upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T16T, string.Empty, upd_MD_4T.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T16T, string.Empty, upd_MD_16T.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        if (!(result = DBProxy.Current.Execute(null, $"update SubTransfer set status='Confirmed', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = Prgs.UpdateFtyInventoryTone((DataTable)this.detailgridbs.DataSource)))
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
Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
and d.toDyelot = f.Dyelot
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
Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
and d.toDyelot = f.Dyelot
where (isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                        ids += $"SP#: {tmp["topoid"]} Seq#: {tmp["toseq1"]}-{tmp["toseq2"]} Roll#: {tmp["toroll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_4F16F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                                 group b by new
                                 {
                                     poid = b.Field<string>("frompoid"),
                                     seq1 = b.Field<string>("fromseq1"),
                                     seq2 = b.Field<string>("fromseq2"),
                                     stocktype = b.Field<string>("fromstocktype"),
                                 }
                        into m
                                 select new
                                 {
                                     poid = m.First().Field<string>("frompoid"),
                                     Seq1 = m.First().Field<string>("fromseq1"),
                                     Seq2 = m.First().Field<string>("fromseq2"),
                                     Stocktype = m.First().Field<string>("fromstocktype"),
                                     Qty = m.Sum(w => w.Field<decimal>("qty")),
                                 }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail Inventory數 --
            var data_MD_8T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("topoid"),
                                  seq1 = b.Field<string>("toseq1"),
                                  seq2 = b.Field<string>("toseq2"),
                                  stocktype = b.Field<string>("tostocktype"),
                              }
                    into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("topoid"),
                                  Seq1 = m.First().Field<string>("toseq1"),
                                  Seq2 = m.First().Field<string>("toseq2"),
                                  Stocktype = m.First().Field<string>("tostocktype"),
                                  Qty = -m.Sum(w => w.Field<decimal>("qty")),
                              }).ToList();

            #endregion
            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   qty = -m.Field<decimal>("qty"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot"),
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

                        #region MDivisionPoDetail
                        StringBuilder upd_MD_4F = new StringBuilder();
                        StringBuilder upd_MD_16F = new StringBuilder();
                        upd_MD_4F.Append(Prgs.UpdateMPoDetail(4, null, false, sqlConn: sqlConn));
                        upd_MD_16F.Append(Prgs.UpdateMPoDetail(16, null, false, sqlConn: sqlConn));
                        string upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F16F, string.Empty, upd_MD_4F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F16F, string.Empty, upd_MD_16F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        if (!(result = DBProxy.Current.Execute(null, $"update SubTransfer set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
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
select [Selected] = 0 
,a.id
,a.FromFtyinventoryUkey
,a.FromPoId
,a.FromSeq1
,a.FromSeq2
,concat(Ltrim(Rtrim(a.FromSeq1)), ' ', a.FromSeq2) as FromSeq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) as [description]
,a.FromRoll
,a.FromDyelot
,a.FromStockType
,a.Qty
,a.ToPoId
,concat(Ltrim(Rtrim(a.ToSeq1)), ' ', a.ToSeq2) as ToSeq
,a.ToSeq1
,a.ToSeq2
,a.ToDyelot
,a.ToRoll
,a.ToStockType
,a.ToLocation
,a.ToContainerCode
,Fromlocation = Fromlocation.listValue
,[FromContainerCode] = c.ContainerCode
,a.ukey
,location = dbo.Getlocation(c.ukey)
from dbo.SubTransfer_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join dbo.ftyinventory c WITH (NOLOCK) on c.poid = p1.id and c.seq1 = p1.seq1 and c.seq2  = p1.seq2 and c.stocktype = 'O' 
    and c.roll = a.FromRoll and c.Dyelot = FromDyelot
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select 	distinct
						fd.MtlLocationID
					from FtyInventory_Detail fd
					left join MtlLocation ml on ml.ID = fd.MtlLocationID
					where fd.Ukey = c.Ukey
					and ml.Junk = 0 
					and ml.StockType = a.ToStockType
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
Where a.id = '{0}'", masterID);
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
            var frm = new P36_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P36_AccumulatedQty(this.CurrentMaintain);
            frm.P36 = this;
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
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            WH_FromTo_Print p = new WH_FromTo_Print(this.CurrentMaintain, "P36")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                DataRow row = this.CurrentMaintain;
                string id = row["ID"].ToString();
                string remark = this.CurrentMaintain["Remark"].ToString();
                string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
                DataTable dt;
                DualResult result = DBProxy.Current.Select(string.Empty, @"Select NameEN from MDivision where id = @MDivision", pars, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!", "DataTable dt");
                    return false;
                }

                string rptTitle = dt.Rows[0]["NameEN"].ToString();
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
                #endregion
                #region  抓表身資料
                pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", id));
                DataTable dd;
                string cmdd =
                    @"select a.FromPOID
                     ,a.FromSeq1+'-'+a.Fromseq2 as SEQ
	                 ,IIF((b.ID = lag(b.ID,1,'')over (order by b.ID,b.seq1,b.seq2) 
				      AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
				      AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
				      ,'',dbo.getMtlDesc(a.FromPOID,a.FromSeq1,a.Fromseq2,2,0))[DESC]
			         ,unit = b.StockUnit
			         ,a.FromRoll
                     ,a.FromDyelot
		             ,a.Qty [QTY]
			         ,a.ToLocation
                     ,a.ToContainerCode
                     ,[Total]=sum(a.Qty) OVER (PARTITION BY a.FromPOID ,a.FromSeq1,a.Fromseq2 )    
             from dbo.Subtransfer_detail a WITH (NOLOCK) 
             left join dbo.PO_Supp_Detail b WITH (NOLOCK) 
                on b.id=a.FromPOID and b.SEQ1=a.FromSeq1 and b.SEQ2=a.FromSeq2 and b.id=a.id
             where a.id= @ID";
                result = DBProxy.Current.Select(
                    string.Empty, cmdd, pars, out dd);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dd == null || dd.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!", "DataTable dd");
                    return false;
                }

                // 傳 list 資料
                List<P36_PrintData> data = dd.AsEnumerable()
                    .Select(row1 => new P36_PrintData()
                    {
                        FromPOID = row1["FromPOID"].ToString().Trim(),
                        SEQ = row1["SEQ"].ToString().Trim(),
                        DESC = row1["DESC"].ToString().Trim(),
                        Unit = row1["unit"].ToString().Trim(),
                        FromRoll = row1["FromRoll"].ToString().Trim(),
                        FromDyelot = row1["FromDyelot"].ToString().Trim(),
                        QTY = row1["QTY"].ToString().Trim(),
                        ToLocation = row1["ToLocation"].ToString().Trim() + Environment.NewLine + row1["ToContainerCode"].ToString().Trim(),
                        Total = row1["Total"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;
                #endregion

                // 指定是哪個 RDLC
                #region  指定是哪個 RDLC

                // DualResult result;
                Type reportResourceNamespace = typeof(P36_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P36_Print.rdlc";

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
            }

            return true;
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P36", this);
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