using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.MyUtility;
using static Sci.Production.Class.MailTools;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P70 : Win.Tems.Input6
    {
        private DualResult result;
        /// <inheritdoc/>
        public P70(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" MDivisionID = '{Env.User.Keyword}'";
            this.detailgrid.RowsAdded += this.Detailgrid_RowsAdded;
        }

        private void Detailgrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataRow curDr = this.detailgrid.GetDataRow(e.RowIndex);
            curDr["StockType"] = "B";
        }

        /// <inheritdoc/>
        public P70(ToolStripMenuItem menuitem, string transID)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"ID ='{transID}' AND MDivisionID = '{Env.User.Keyword}'";
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
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.detailgrid.Rows.RemoveAt(0);
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
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

            bool isFabricRollDyelotEmpty = this.DetailDatas
                            .Where(s => s["FabricType"].ToString() == "F" && (MyUtility.Check.Empty(s["Roll"]) || MyUtility.Check.Empty(s["Dyelot"])))
                            .Any();
            if (isFabricRollDyelotEmpty)
            {
                MyUtility.Msg.WarningBox("[Fabric] Roll and Dyelot cannot be empty.");
                return false;
            }

            string sqlcmd = $@"
            Select 
            [SP#] = lord.Poid,
            [Seq] = Concat (lord.Seq1, ' ', lord.Seq2),
            [Roll] = lord.Roll,
            [Dyelot] = lord.Dyelot,
            [Receiving ID] = lord.ID
            from #tmp t 
            inner join LocalOrderReceiving_Detail lord on t.Poid = lord.Poid and
                                                          t.Seq1 = lord.Seq1 and
                                                          t.Seq2 = lord.Seq2 and
                                                          t.Roll = lord.Roll and
                                                          t.Dyelot = lord.Dyelot and
                                                          lord.ID <> '{this.CurrentMaintain["ID"]}'
            ";
            DataTable dtInvShort;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), null, sqlcmd, out dtInvShort);

            if (dtInvShort.Rows.Count > 0)
            {
                Class.MsgGrid form = new Class.MsgGrid(dtInvShort, "These fabric already exists, cannot be saved.");
                form.ShowDialog(this);
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "OR", "LocalOrderReceiving", (DateTime)this.CurrentMaintain["AddDate"]);
            }

            foreach (var item in this.DetailDatas)
            {
                item["Roll"] = item["FabricType"].ToString() == "A" ? string.Empty : item["Roll"];
                item["Dyelot"] = item["FabricType"].ToString() == "A" ? string.Empty : item["Dyelot"];
            }

            return base.ClickSaveBefore();
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

            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["WhseArrival"]))
            {
                MyUtility.Msg.WarningBox("Arrive WH Date cannot be empty.", "Warning");
                this.dateArriveWHDate.Focus();
                return;
            }
            #endregion

            // 取得 LocalOrderInventory 資料
            DualResult result = Prgs.GetLocalOrderInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtLocalOrderInventory);
            string sq = string.Empty;

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            string sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0)
        , d.Dyelot
from dbo.LocalOrderReceiving_Detail d WITH (NOLOCK) 
left join LocalOrderInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) + d.Qty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
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

            #region -- 更新庫存數量  LocalOrderInventory --
            var data_Fty_2T = (from m in this.DetailDatas
                               select new
                               {
                                   poid = m.Field<string>("poid"),
                                   seq1 = m.Field<string>("seq1"),
                                   seq2 = m.Field<string>("seq2"),
                                   stocktype = m.Field<string>("stocktype"),
                                   qty = m.Field<decimal>("Qty"),
                                   location = m.Field<string>("location"),
                                   FabricType = m.Field<string>("FabricType"),
                                   roll = m.Field<string>("roll"),
                                   dyelot = m.Field<string>("dyelot"),
                                   tone = m.Field<string>("Tone"),
                               }).ToList();
            #endregion 更新庫存數量  LocalOrderInventory

            #region 檢查Barcode是否有在其他單子重複，有重複就update成空白, where 拆開來是因為效能(有index但有時候無效)
            string sqlCheckBarcode = $@"
update rd set rd.Barcode = ''
from LocalOrderReceiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from LocalOrderReceiving_Detail rd2 with (nolock) where rd2.ID <> rd.ID and rd2.Barcode = rd.Barcode)

update rd set rd.Barcode = ''
from LocalOrderReceiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where wht.Action = 'Confirm' and [Function] = 'P70' and wht.TransactionID <> rd.ID and wht.To_NewBarcode = rd.Barcode)

update rd set rd.Barcode = ''
from LocalOrderReceiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P70' and wht.From_OldBarcode = rd.Barcode)

update rd set rd.Barcode = ''
from LocalOrderReceiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P70' and wht.From_NewBarcode = rd.Barcode)

update rd set rd.Barcode = ''
from LocalOrderReceiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P70' and wht.To_OldBarcode = rd.Barcode)

update rd set rd.Barcode = ''
from LocalOrderReceiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P70' and wht.To_NewBarcode = rd.Barcode)
";
            #endregion

            DBProxy.Current.DefaultTimeout = 900;  // 加長時間為15分鐘，避免timeout
            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 15, 0)))
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        DataTable resulttb;

                        // LocalOrderInventory 庫存
                        string upd_Fty_2T = Prgs.UpdateLocalOrderInventory_IO("In", null, true);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // LocalOrderInventory Tone一定要在更新庫存後面執行
                        string upd_Fty_Tone = Prgs.UpdateLocalOrderInventory_IO("Tone", null, true);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_Tone, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // 檢查Barcode是否有在其他單子重複，有的畫清空，UpdateWH_Barcode會重編新的
                        result = DBProxy.Current.ExecuteByConn(sqlConn, sqlCheckBarcode);
                        if (!result)
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 LocalOrderInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtLocalOrderInventory, isLocalOrder: true)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = DBProxy.Current.Execute(null, $"update LocalOrderReceiving set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
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

            DBProxy.Current.DefaultTimeout = 300;  // 恢復時間為5分鐘
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

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "LocalOrderReceiving_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存
            string sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0)
        , d.Dyelot
from dbo.LocalOrderReceiving_Detail d WITH (NOLOCK) 
left join LocalOrderInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - d.Qty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
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

            #region 檢查單據有主料則 Barcode不可為空
            if (!Prgs.CheckBarCode(dtLocalOrderInventory, this.Name, isLocalOrderInventory: true))
            {
                return;
            }
            #endregion

            #region 更新庫存數量 LocalOrderInventory
            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("poid"),
                                   seq1 = m.Field<string>("seq1"),
                                   seq2 = m.Field<string>("seq2"),
                                   stocktype = m.Field<string>("stocktype"),
                                   qty = -m.Field<decimal>("Qty"),
                                   location = m.Field<string>("location"),
                                   roll = m.Field<string>("roll"),
                                   dyelot = m.Field<string>("dyelot"),
                                   tone = m.Field<string>("Tone"),
                               }).ToList();
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
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 15, 0)))
            {
                try
                {
                    DataTable resulttb;

                    // LocalOrderInventory 庫存
                    string upd_Fty_2F = Prgs.UpdateLocalOrderInventory_IO("In", null, false);
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 LocalOrderInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtLocalOrderInventory, isLocalOrder: true)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $@"update LocalOrderReceiving set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
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
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, cannot print.");
                return false;
            }

            ReportDefinition rd = new ReportDefinition();
            if (!(this.result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(this.GetType()), this.GetType(), "P70_Print.rdlc", out IReportResource reportresource)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
            }
            else
            {
                #region -- 整理表頭資料 --
                // 抓M的EN NAME
                string rptTitle = MyUtility.GetValue.Lookup($@"select NameEN from MDivision where ID='{Env.User.Keyword}'");
                DataRow row = this.CurrentMaintain;
                rd.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
                rd.ReportParameters.Add(new ReportParameter("ID", row["ID"].ToString()));
                rd.ReportParameters.Add(new ReportParameter("Remark", row["Remark"].ToString()));
                rd.ReportParameters.Add(new ReportParameter("IssueDate", ((DateTime)MyUtility.Convert.GetDate(row["WhseArrival"])).ToString("yyyy/MM/dd")));
                #endregion

                string sqlcmd = $@"
                SELECT 
                lord.*,
                [Seq] = Concat (lord.Seq1, ' ', lord.Seq2),
                [Desc] = IIF((lord.ID = lag(lord.ID,1,'') over (order by lord.ID,lord.seq1,lord.Seq2) 
		                 AND (lord.seq1 = lag(lord.seq1,1,'')over (order by lord.ID,lord.seq1,lord.Seq2)
		                 AND (lord.seq2 = lag(lord.seq2,1,'')over (order by lord.ID,lord.seq1,lord.Seq2))))
			                , ''
                            , concat(lom.[Desc],char(10),'Color : ', lom.Color)),
                    lom.Unit,
                    lom.Color,
	                lom.[Desc]
                FROM LocalOrderReceiving_Detail lord
                LEFT JOIN LocalOrderMaterial lom  ON lord.POID = lom.POID AND lord.Seq1 = lom.Seq1 AND lord.Seq2 = lom.Seq2
                WHERE lord.ID = '{row["ID"]}'
                ";
                DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dataTable);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                List<P70_PrintData> data = dataTable.AsEnumerable()
                    .Select(row1 => new P70_PrintData()
                    {
                        POID = row1["POID"].ToString().Trim(),
                        SEQ = row1["Seq"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        DYELOT = row1["Dyelot"].ToString().Trim(),
                        DESC = row1["Desc"].ToString().Trim(),
                        Unit = row1["Unit"].ToString().Trim(),
                        QTY = row1["QTY"].ToString().Trim(),
                        ToneGrp = row1["Tone"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim(),
                    }).ToList();

                rd.ReportDataSource = data;
                rd.ReportResource = reportresource;
                var frm1 = new Win.Subs.ReportView(rd)
                {
                    MdiParent = this.MdiParent,
                    TopMost = true,
                };
                frm1.Show();
            }

            return base.ClickPrint();
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
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
            SELECT 
            [POID] = lord.POID,
            [Seq] = Concat (lord.Seq1, ' ', lord.Seq2),
            [Seq1] = lord.Seq1,
            [Seq2] = lord.Seq2,
            [FabricType] = lom.FabricType,
            [MaterialType] = IIF(lom.FabricType = 'F' ,Concat ('Fabric-', lom.MtlType),Concat ('Accessory-', lom.MtlType)),
            [Roll] = lord.Roll,
            [Dyelot] = lord.Dyelot,
            [StockType] = lord.StockType,
            [Weight] = lord.Weight,
            [ActualWeight] = lord.ActualWeight,
            [Tone] = lord.Tone,
            [Qty] = lord.Qty,
            [Unit] = lom.Unit,
            [Location] = lord.Location,
            [Refno] = lom.Refno,
            [Color] = lom.Color,
            [QrCode] = lord.Barcode,
            [ContainerCode] = lord.ContainerCode,
            [Ukey] = lord.ukey
            FROM LocalOrderReceiving_Detail lord
            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = lord.POID AND lord.Seq1 = lom.Seq1 AND lord.Seq2 = lom.Seq2
            WHERE lord.ID = '{masterID}'
            ";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region SP event
            DataGridViewGeneratorTextColumnSettings colSP = new DataGridViewGeneratorTextColumnSettings();

            colSP.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["POID"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                List<SqlParameter> par = new List<SqlParameter>() { new SqlParameter("@poid", newvalue) };
                bool isPOIDnotExists = !MyUtility.Check.Seek("select 1 from orders with (nolock) where poid = @poid and localOrder = 1", par);

                if (isPOIDnotExists)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"Cannot found local order {newvalue}.");
                    return;
                }

                this.CurrentDetailData["POID"] = newvalue;
            };
            #endregion
            #region Seq event
            DataGridViewGeneratorTextColumnSettings colSeq = new DataGridViewGeneratorTextColumnSettings();

            colSeq.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Seq"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                int spaceIndex = newvalue.IndexOf(' ');
                if (spaceIndex < 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"Cannot found Seq {newvalue}");
                    return;
                }

                string seq1 = newvalue.Substring(0, spaceIndex);
                string seq2 = newvalue.Substring(spaceIndex + 1);

                List<SqlParameter> par = new List<SqlParameter>()
                {
                    new SqlParameter("@POID", this.CurrentDetailData["POID"]),
                    new SqlParameter("@Seq1", seq1),
                    new SqlParameter("@Seq2", seq2),
                };
                DataRow drSeq;

                string sqlcmd = $@"
                select
                [Seq] = Concat (lom.Seq1, ' ', lom.Seq2),
                [Material Type] = IIF(lom.FabricType = 'F' ,Concat ('Fabric-', lom.MtlType),Concat ('Accessory-', lom.MtlType)),
                [Ref#] = lom.Refno,
                [Weave Type] = WeaveType,
                [Unit] = lom.Unit, 
                [Color] = lom.Color
                from LocalOrderMaterial lom WITH(NOLOCK)
                WHERE 
                lom.POID = @POID AND
                lom.Seq1 = @Seq1 AND
                lom.Seq2 = @Seq2                
                ";
                bool isPOIDnotExists = !MyUtility.Check.Seek(sqlcmd, par, out drSeq);

                if (isPOIDnotExists)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Seq is not exist.");
                    return;
                }

                string materialType = drSeq["Material Type"].ToString();
                int fabricTypeIndex = materialType.IndexOf('-');
                string fabricType = materialType.Substring(0, fabricTypeIndex) == "Fabric" ? "F" : "A";

                this.CurrentDetailData["FabricType"] = fabricType;
                this.CurrentDetailData["Seq"] = newvalue;
                this.CurrentDetailData["Seq1"] = seq1;
                this.CurrentDetailData["Seq2"] = seq2;
                this.CurrentDetailData["MaterialType"] = drSeq["Material Type"].ToString();
                this.CurrentDetailData["Unit"] = drSeq["Unit"].ToString();
                this.CurrentDetailData["Refno"] = drSeq["Ref#"].ToString();
                this.CurrentDetailData["Color"] = drSeq["Color"].ToString();
            };

            colSeq.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = $@"
                    select
                    [Seq] = Concat (lom.Seq1, ' ', lom.Seq2),
                    [Material Type] = IIF(lom.FabricType = 'F' ,Concat ('Fabric-', lom.MtlType),Concat ('Accessory-', lom.MtlType)),
                    [Ref#] = lom.Refno,
                    [Weave Type] = WeaveType,
                    [Unit] = lom.Unit, 
                    [Color] = lom.Color
                    from LocalOrderMaterial lom WITH(NOLOCK)
                    WHERE lom.POID = '{this.CurrentDetailData["POID"]}'                
                    ";

                    SelectItem item = new SelectItem(sqlcmd, null, null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    string seq = item.GetSelecteds()[0]["Seq"].ToString();
                    int seqIndex = seq.IndexOf(' ');

                    string seq1 = seq.Substring(0, seqIndex);
                    string seq2 = seq.Substring(seqIndex + 1);

                    string materialType = item.GetSelecteds()[0]["Material Type"].ToString();
                    int spaceIndex = materialType.IndexOf('-');
                    string fabricType = materialType.Substring(0, spaceIndex) == "Fabric" ? "F" : "A";

                    this.CurrentDetailData["FabricType"] = fabricType;
                    this.CurrentDetailData["Seq"] = item.GetSelecteds()[0]["Seq"];
                    this.CurrentDetailData["Seq1"] = seq1;
                    this.CurrentDetailData["Seq2"] = seq2;
                    this.CurrentDetailData["MaterialType"] = item.GetSelecteds()[0]["Material Type"];
                    this.CurrentDetailData["Unit"] = item.GetSelecteds()[0]["Unit"];
                    this.CurrentDetailData["Refno"] = item.GetSelecteds()[0]["Ref#"];
                    this.CurrentDetailData["Color"] = item.GetSelecteds()[0]["Color"];
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion
            #region Location event

            DataGridViewGeneratorTextColumnSettings colLocation = new DataGridViewGeneratorTextColumnSettings();
            colLocation.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation("B", this.CurrentDetailData["Location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Location"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };

            colLocation.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Location"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = $@"
                    SELECT id 
                    FROM MtlLocation WITH (NOLOCK)
                    WHERE 
                    StockType='B' AND
                    junk != '1'";
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                    string[] getLocation = this.CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
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
                        MyUtility.Msg.WarningBox("Cannot found Location " + string.Join(",", errLocation.ToArray()) + ".", "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["Location"] = string.Join(",", trueLocation.ToArray());
                }
            };
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;
            #endregion Location 右鍵開窗
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), settings: colSP)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), settings: colSeq)
            .Text("MaterialType", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8))
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
            .Numeric("Weight", header: "G.W(kg)", decimal_places: 2, width: Widths.AnsiChars(8))
            .Numeric("ActualWeight", header: "Act.(kg)", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8))
            .Numeric("Qty", header: "Qty", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), settings: colLocation)
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("QrCode", header: "QR Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ContainerCode", header: "ContainerCode", width: Widths.AnsiChars(20), iseditingreadonly: true).Get(out cbb_ContainerCode)
            ;
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
        }

        private void BtnDownloadSampleFile_Click(object sender, EventArgs e)
        {
            // 呼叫執行檔絕對路徑
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
            string strXltName = Env.Cfg.XltPathDir + "\\Warehouse_P70_DownloadSampleFile.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas.Count == 0)
            {
                return;
            }

            var frm = new P70_AccumulatedQty((DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog();
        }

        private void BtnImportFromExcel_Click(object sender, EventArgs e)
        {
            P70_ExcelImport callNextForm = new P70_ExcelImport((DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}
