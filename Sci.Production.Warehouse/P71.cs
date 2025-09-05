using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicForm;
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
    /// P71
    /// </summary>
    public partial class P71 : Win.Tems.Input6
    {
        /// <summary>
        /// P71
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P71(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            this.DefaultFilter = $" MDivisionID = '{Env.User.Keyword}'";
        }

        /// <inheritdoc/>
        public P71(ToolStripMenuItem menuitem, string transID)
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
FROM LocalOrderIssue_Detail loid
LEFT JOIN LocalOrderMaterial lom ON lom.POID = loid.POID AND loid.Seq1 = lom.Seq1 AND loid.Seq2 = lom.Seq2
LEFT JOIN LocalOrderInventory loi ON loid.POID = loi.POID
	AND loid.Seq1 = loi.Seq1 AND loid.Seq2 = loi.Seq2 AND loid.Roll = loi.Roll AND loid.Dyelot = loi.Dyelot AND loid.StockType = loi.StockType
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

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("MaterialType", header: "Material Type", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("Qty", header: "Issue Qty", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;

            this.detailgrid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("Issue Date cannot be empty.");
                return;
            }

            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();

            // 取得 LocalOrderInventory資料
            DualResult result = Prgs.GetLocalOrderInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtLocalOrderInventory);
            string ids = string.Empty;
            string sqlcmd = string.Empty;

            // 檢查單據有主料則 Barcode不可為空
            if (!Prgs.CheckBarCode(dtLocalOrderInventory, this.Name, isLocalOrderInventory: true))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select   [SP#] = d.poid
        ,[Seq] = Concat (d.Seq1, ' ', d.Seq2)
        ,d.Roll
        ,d.Dyelot
        ,[Balance Qty] = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0)
from dbo.LocalOrderIssue_Detail d WITH (NOLOCK) 
left join LocalOrderInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    Class.MsgGrid form = new Class.MsgGrid(datacheck, "Balacne Qty is not enough!!");
                    form.ShowDialog(this);
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "LocalOrderIssue_Detail", msgType: "LocalOrder", isLocalOrder: true) == false)
            {
                return;
            }

            #endregion

            #region 更新庫存數量  LocalOrderInventory
            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = m.Field<decimal>("qty"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            string sqlupd2_FIO = Prgs.UpdateLocalOrderInventory_IO("Out", null, true);
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 LocalOrderInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtLocalOrderInventory, isLocalOrder: true)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update LocalOrderIssue set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
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
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 取得 LocalOrderInventory資料
            DualResult result = Prgs.GetLocalOrderInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtLocalOrderInventory);
            string ids = string.Empty;
            string sqlcmd = string.Empty;

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "LocalOrderIssue_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                MyUtility.Msg.WarningBox("Material Location is from WMS system cannot confirmed or unconfirmed. ", "Warning");
                return;
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
        ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) as balanceQty
        ,d.Dyelot   
from dbo.LocalOrderIssue_Detail d WITH (NOLOCK) 
left join LocalOrderInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    Class.MsgGrid form = new Class.MsgGrid(datacheck, "Balacne Qty is not enough!!");
                    form.ShowDialog(this);
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新庫存數量  LocalOrderInventory
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
            string sqlupd2_FIO = Prgs.UpdateLocalOrderInventory_IO("Out", null, false);
            #endregion

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
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

                    // 更新LocalOrderInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // 更新 Barcode
                    if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtLocalOrderInventory, isLocalOrder: true)))
                    {
                        throw result.GetException();
                    }

                    // 更新Status
                    if (!(result = DBProxy.Current.Execute(null, $@"update LocalOrderIssue set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
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
                                        .Where(s => MyUtility.Check.Empty(s["POID"]) || MyUtility.Check.Empty(s["Seq"]) || MyUtility.Check.Empty(s["Qty"]))
                                        .Any();

            if (isDetailKeyColEmpty)
            {
                MyUtility.Msg.WarningBox("<SP#>, <Seq>, <Qty> cannot be empty.");
                return false;
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                {
                    warningmsg.Append(string.Format(@"Roll and Dyelot can't be empty"));
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "OI", "LocalOrderIssue", DateTime.Now);
            }

            return base.ClickSaveBefore();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.detailgrid.EndEdit();
            new P71_Import((DataTable)this.detailgridbs.DataSource).ShowDialog();
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
        loi.Tone,
        [Location] = Location.val,
	    [Total]=sum(loid.Qty) OVER (PARTITION BY loid.POID ,loid.seq1,loid.Seq2)
from    LocalOrderIssue_Detail loid
LEFT JOIN LocalOrderMaterial lom ON loid.POID = lom.POID AND loid.Seq1 = lom.Seq1 AND loid.Seq2 = lom.Seq2
LEFT JOIN LocalOrderInventory loi with (nolock) ON 
									  loi.POID         = loid.POID        and
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
                    TotalQTY = row1["Total"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            Type reportResourceNamespace = typeof(P71_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P71_Print.rdlc";
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
