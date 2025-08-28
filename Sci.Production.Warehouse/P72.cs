using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tems;
using Sci.Win.Tools;
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
    /// <summary>
    /// P72
    /// </summary>
    public partial class P72 : Win.Tems.Input6
    {
        /// <summary>
        /// P72
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P72(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            this.DefaultFilter = $" MDivisionID = '{Env.User.Keyword}'";
        }

        /// <inheritdoc/>
        public P72(ToolStripMenuItem menuitem, string transID)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" id='{transID}' AND MDivisionID  = '{Sci.Env.User.Keyword}'";
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
SELECT loid.*
, Seq = Concat (loid.Seq1, ' ', loid.Seq2)
, [MaterialType] = CASE lom.FabricType 
					WHEN 'F' then CONCAT('Fabric-',lom.MtlType)
					WHEN 'A' THEN CONCAT('Accessory-',lom.MtlType) ELSE '' END
, [FabricType] = lom.FabricType 
, [Desc] = ISNULL(lom.[Desc],'')
, [Color] = ISNULL(lom.Color,'')
, [Tone] = ISNULL(loi.Tone,'')
, [Unit] = ISNULL(lom.Unit,'')
, [Location] = ISNULL(LOCATION.val,'')
, [ReasonName] = r.Name
, [AdjustQty] = loid.QtyAfter - loid.QtyBefore
, [LocalOrderInventoryUkey] = loi.Ukey
FROM LocalOrderAdjust_Detail loid
LEFT JOIN LocalOrderMaterial lom ON lom.POID = loid.POID AND loid.Seq1 = lom.Seq1 AND loid.Seq2 = lom.Seq2
LEFT JOIN LocalOrderInventory loi ON loid.POID = loi.POID
	AND loid.Seq1 = loi.Seq1 AND loid.Seq2 = loi.Seq2 AND loid.Roll = loi.Roll AND loid.Dyelot = loi.Dyelot AND loid.StockType = loi.StockType
LEFT JOIN Reason r ON r.ID = loid.ReasonID	and r.ReasonTypeID='Stock_Adjust'
outer apply (
	SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
	FROM LocalOrderInventory_Location loil with (nolock)
	where   loil.LocalOrderInventoryUkey	 = loi.Ukey
	FOR XML PATH('')),1,1,'')  
) Location
where   loid.ID = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorNumericColumnSettings colQtyAfter = new DataGridViewGeneratorNumericColumnSettings();
            colQtyAfter.CellValidating += (s, e) =>
            {
                this.detailgrid.EndEdit();
                decimal qtyAfter = (decimal)this.detailgrid.Rows[e.RowIndex].Cells["QtyAfter"].Value;
                decimal qtyBefore = (decimal)this.detailgrid.Rows[e.RowIndex].Cells["QtyBefore"].Value;
                this.detailgrid.Rows[e.RowIndex].Cells["AdjustQty"].Value = qtyAfter - qtyBefore;
                this.detailgrid.RefreshEdit();
            };

            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        poitems,
                        "ID,Name",
                        "5,150",
                        this.CurrentDetailData["reasonid"].ToString(),
                        "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.CurrentDetailData["reasonid"] = x[0]["id"];
                    this.CurrentDetailData["ReasonName"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["reasonid"] = string.Empty;
                        this.CurrentDetailData["ReasonName"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.CurrentDetailData["reasonid"] = e.FormattedValue;
                            this.CurrentDetailData["ReasonName"] = dr["name"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("MaterialType", header: "Material Type", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("QtyBefore", header: "Original Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, width: Widths.AnsiChars(8), settings: colQtyAfter)
            .Numeric("AdjustQty", header: "Adjust Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("reasonid", header: "Reason ID", settings: ts)
            .Text("ReasonName", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
            ;

            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
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
        protected override void ClickConfirm()
        {
            if ((this.detailgridbs.DataSource as DataTable).Select("ReasonID = ''" ).Count() > 0)
            {
                MyUtility.Msg.WarningBox("Reason ID cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("Issue Date cannot be empty.");
                return;
            }

            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            // 取得 LocalOrderInventory資料
            DualResult result = Prgs.GetLocalOrderInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtLocalOrderInventory);
            StringBuilder upd_MD_8T = new StringBuilder();
            StringBuilder upd_MD_32T = new StringBuilder();
            StringBuilder upd_Fty_8T = new StringBuilder();
            string sqlcmd = string.Empty;
            string ids = string.Empty;

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查物料不能有WMS Location && IsFromWMS = 0
            if (!PublicPrg.Prgs.Chk_WMS_Location_Adj((DataTable)this.detailgridbs.DataSource, isLocal: true) && MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
            {
                MyUtility.Msg.WarningBox("Material Location or Adjust is from WMS system cannot save or confirmed. ", "Warning");
                return;
            }
            #endregion

            #region 檢查負庫存
            if (!Prgs.CheckAdjustBalance(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: true, isLocalOrder: true))
            {
                return;
            }
            #endregion

            #region 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtLocalOrderInventory, this.Name, isLocalOrderInventory: true))
            {
                return;
            }
            #endregion

            #region 更新庫存數量  LocalOrderInventory
            DataTable data_Fty_8T = (DataTable)this.detailgridbs.DataSource;
            data_Fty_8T.ColumnsDecimalAdd("qty", expression: "QtyAfter- QtyBefore");

            upd_Fty_8T.Append(Prgs.UpdateLocalOrderInventory_IO("Adjust", null, true));
            #endregion 更新庫存數量  LocalOrderInventory

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "LocalOrderAdjust_Detail", msgType: "LocalOrder", isLocalOrder: true) == false)
            {
                return;
            }
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        data_Fty_8T, string.Empty, upd_Fty_8T.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update LocalOrderAdjust set status='Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 LocalOrderInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtLocalOrderInventory, isLocalOrder: true)))
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
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtLocalOrderInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 取得 LocalOrderInventory資料
            DualResult result = Prgs.GetLocalOrderInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtLocalOrderInventory);
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            StringBuilder upd_Fty_8F = new StringBuilder();
            string sqlcmd = string.Empty;
            string ids = string.Empty;
            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P72"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(dt, "LocalOrderAdjust_Detail"))
            {
                return;
            }
            #endregion

            // 檢查負數庫存
            if (!Prgs.CheckAdjustBalance(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: false, isLocalOrder: true))
            {
                return;
            }

            #region -- 更新庫存數量  LocalOrderInventory --

            DataTable data_Fty_8F = (DataTable)this.detailgridbs.DataSource;

            // dtio.ColumnsDecimalAdd("qty", expression: "QtyAfter- QtyBefore");
            data_Fty_8F.Columns.Add("qty", typeof(decimal));
            foreach (DataRow dx in data_Fty_8F.Rows)
            {
                dx["qty"] = -((decimal)dx["QtyAfter"] - (decimal)dx["QtyBefore"]);
            }

            upd_Fty_8F.Append(Prgs.UpdateLocalOrderInventory_IO("Adjust", null, false));

            #endregion 更新庫存數量  LocalOrderInventory

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            //// 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtLocalOrderInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        data_Fty_8F, string.Empty, upd_Fty_8F.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update LocalOrderAdjust set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 LocalOrderInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtLocalOrderInventory, isLocalOrder: true)))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtLocalOrderInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtLocalOrderInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtLocalOrderInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("No details");
                return false;
            }

            bool isDetailKeyColEmpty = this.DetailDatas
                                        .Where(s => MyUtility.Check.Empty(s["POID"]) || MyUtility.Check.Empty(s["Seq"]) || MyUtility.Check.Empty(s["reasonid"]) || (decimal)s["QtyAfter"] < 0)
                                        .Any();

            if (isDetailKeyColEmpty)
            {
                MyUtility.Msg.WarningBox("<SP#>, <Seq>, <Reason ID> cannot be empty, <Current Qty> cannot be less than 0.");
                return false;
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                {
                    warningmsg.Append(string.Format(@"[Fabric] Roll and Dyelot cannot be empty"));
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["roll"] = string.Empty;
                    row["dyelot"] = string.Empty;
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            // 檢查物料不能有WMS Location && IsFromWMS = 0
            if (!PublicPrg.Prgs.Chk_WMS_Location_Adj((DataTable)this.detailgridbs.DataSource, true) && MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
            {
                MyUtility.Msg.WarningBox("Material Location or Adjust is from WMS system cannot save or confirmed. ", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "OA", "LocalOrderAdjust", DateTime.Now);
            }

            return base.ClickSaveBefore();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.detailgrid.EndEdit();
            new P72_Import((DataTable)this.detailgridbs.DataSource).ShowDialog();
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain == null || !this.DetailDatas.Any())
            {
                return false;
            }

            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, cannot print.", "Warning");
                return false;
            }

            string preparedBy = this.editby.Text;
            #region -- 撈表頭資料 --
            string rptTitle = MyUtility.GetValue.Lookup($@"select NameEN from MDivision where ID='{Env.User.Keyword}'");
            ReportDefinition report = new ReportDefinition();
            DataRow row = this.CurrentMaintain;
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", row["ID"].ToString()));
            report.ReportParameters.Add(new ReportParameter("Remark", row["Remark"].ToString()));
            report.ReportParameters.Add(new ReportParameter("IssueDate", ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToString("yyyy/MM/dd")));
            #endregion
            #region -- 撈表身資料 --
            string sqlcmd = $@"
select  loid.*,
		[Seq] = Concat (loid.Seq1, ' ', loid.Seq2),
	    [Desc] = concat(lom.[Desc],char(10),'Color : ', lom.Color),
        lom.Unit,
        lom.Color,
        [Location] = Location.val,
		[Tone] = loi.Tone,
	    [Qty] = loid.QtyAfter - loid.QtyBefore,
		[TotalQty] = sum(loid.QtyAfter - loid.QtyBefore) OVER (PARTITION BY loid.POID ,loid.seq1,loid.Seq2)
from    LocalOrderAdjust_Detail loid
LEFT JOIN LocalOrderMaterial lom ON loid.POID = lom.POID AND loid.Seq1 = lom.Seq1 AND loid.Seq2 = lom.Seq2
LEFT JOIN LocalOrderInventory loi with (nolock) on loi.POID         = loid.POID        and
                                      loi.Seq1         = loid.Seq1        AND
									  loi.Seq2         = loid.Seq2        and
                                      loi.Roll         = loid.Roll        and
                                      loi.Dyelot       = loid.Dyelot      and
                                      loi.StockType    = loid.StockType
outer apply (SELECT val =  Stuff((select distinct concat( ',',loil.MtlLocationID)   
                                from LocalOrderInventory_Location loil with (nolock) 
								where loi.Ukey = loil.LocalOrderInventoryUkey                                
                                FOR XML PATH('')),1,1,'')  ) Location
where   loid.ID = '{row["ID"]}'
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtDetail);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            // 傳 list 資料
            List<P71_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P71_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["desc"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                    Unit = row1["Unit"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    DYELOT = row1["Dyelot"].ToString().Trim(),
                    ToneGrp = row1["Tone"].ToString().Trim(),
                    QTY = row1["Qty"].ToString().Trim(),
                    TotalQTY = row1["TotalQty"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            Type reportResourceNamespace = typeof(P71_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P72_Print.rdlc";
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
            {
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report) { MdiParent = this.MdiParent };
            frm.Show();

            return base.ClickPrint();
        }
    }
}
