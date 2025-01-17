using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P45 : Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ToPoid;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ToSeq;

        /// <inheritdoc/>
        public P45(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='R' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P45(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='R' and id='{0}'", transID);
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
            this.CurrentMaintain["Type"] = "R";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
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
            //     檢查明細至少存在一筆資料。
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            StringBuilder warningmsg = new StringBuilder();

            foreach (DataRow row in this.DetailDatas)
            {
                //     檢查所有明細資料的current qty 不可等於 original qty
                if (MyUtility.Convert.GetDecimal(row["QtyAfter"]) == MyUtility.Convert.GetDecimal(row["QtyBefore"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} SEQ#: {1} Roll#: {2} Dyelot: {3}
Original Qty and Current Qty can’t be equal!!",
                        row["poid"].ToString().Trim(),
                        row["seq"].ToString().Trim(),
                        row["Roll"].ToString().Trim(),
                        row["Dyelot"].ToString().Trim())
                        + Environment.NewLine);
                }

                //     檢查所有明細資料都有填入reason
                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} SEQ#: {1} Roll#: {2} Dyelot: {3}
Reason can’t be empty!!",
                        row["poid"].ToString().Trim(),
                        row["seq"].ToString().Trim(),
                        row["Roll"].ToString().Trim(),
                        row["Dyelot"].ToString().Trim())
                       + Environment.NewLine);
                }
            }

            //     全部檢查完再Message有問題的detail資料。 全部檢查完再Message沒填入reason的detail資料
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "AM", "Adjust", (DateTime)this.CurrentMaintain["Issuedate"], 2, "ID", null);
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

            // 檢查負數庫存
            if (!Prgs.CheckAdjustBalance(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: true))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P45"))
            {
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "Adjust_Detail") == false)
            {
                return;
            }
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    if (!(result = Prgs.UpdateScrappAdjustFtyInventory(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: true)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Adjust set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
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
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
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

            // 取得 FtyInventory 資料
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);

            // 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "Adjust_Detail"))
            {
                return;
            }

            // 檢查負數庫存
            if (!Prgs.CheckAdjustBalance(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: false))
            {
                return;
            }

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
                try
                {
                    if (!(result = Prgs.UpdateScrappAdjustFtyInventory(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: false)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Adjust set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
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
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain == null ||
                this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            WH_FromTo_Print p = new WH_FromTo_Print(this.CurrentMaintain, "P45")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                DateTime? issueDate = MyUtility.Convert.GetDate(this.CurrentMaintain["IssueDate"]);
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new ReportParameter("ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                report.ReportParameters.Add(new ReportParameter("IssueDate", issueDate.HasValue ? issueDate.Value.ToString("yyyy/MM/dd") : string.Empty));
                report.ReportParameters.Add(new ReportParameter("Remark", MyUtility.Convert.GetString(this.CurrentMaintain["Remark"])));

                List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])),
            };

                string sqlcmd = @"
select  ad.POID
	, [Seq] = concat(ad.Seq1, '-', ad.Seq2)
	, ad.Roll
	, ad.Dyelot
	, [ColorID] = dbo.GetColorMultipleID(psd.BrandId, isnull(psdsC.SpecValue, ''))
	, [Description] =IIF((
								ad.POID = LAG(ad.POID,1,'') OVER (ORDER BY ad.POID,ad.seq1,ad.seq2, ad.Dyelot,ad.Roll) 
								AND ad.Seq1 = LAG(ad.Seq1,1,'') OVER (ORDER BY ad.POID,ad.seq1,ad.seq2, ad.Dyelot,ad.Roll) 
								AND ad.Seq2 = LAG(ad.Seq2,1,'') OVER (ORDER BY ad.POID,ad.seq1,ad.seq2, ad.Dyelot,ad.Roll) 
							)
						,''
						, dbo.getmtldesc(ad.POID, ad.Seq1, ad.Seq2, 2, 0))
	, [AdjustQty] = ad.QtyBefore - ad.QtyAfter
	, psd.StockUnit
	, [Location] = dbo.Getlocation(fi.ukey)
    , fi.ContainerCode
    , [Total]=sum(ad.QtyBefore - ad.QtyAfter) OVER (PARTITION BY ad.POID ,ad.Seq1,ad.Seq2 )    
from Adjust a
inner join Adjust_Detail ad on a.ID = ad.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.id = ad.POID and psd.SEQ1 = ad.Seq1 and psd.SEQ2 = ad.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join FtyInventory fi WITH (NOLOCK) on fi.POID = ad.POID and fi.Seq1 = ad.Seq1 and fi.Seq2 = ad.Seq2 and fi.Roll = ad.Roll and fi.StockType = ad.StockType and fi.Dyelot = ad.Dyelot
where a.ID = @ID
and a.Status = 'Confirmed'
";
                DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtDetail);
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
                List<P45_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P45_PrintData()
                    {
                        POID = row1["POID"].ToString().Trim(),
                        SEQ = row1["SEQ"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        Dyelot = row1["Dyelot"].ToString().Trim(),
                        ColorID = row1["ColorID"].ToString().Trim(),
                        Description = row1["Description"].ToString().Trim(),
                        AdjustQty = row1["AdjustQty"].ToString().Trim(),
                        StockUnit = row1["StockUnit"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        Total = row1["Total"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;

                // 指定是哪個 RDLC
                Type reportResourceNamespace = typeof(P45_PrintData);
                System.Reflection.Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P45_Print.rdlc";

                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();
            }

            return base.ClickPrint();
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
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select
    ad.*
	,seq = concat(ad.Seq1,'-',ad.Seq2)
    ,ToSeq = concat(ad.ToSeq1,' ',ad.ToSeq2)
	,Description = dbo.getmtldesc(ad.POID, ad.Seq1, ad.Seq2, 2, 0)
	,adjustqty= ad.QtyBefore-ad.QtyAfter
	,psd.StockUnit
	,Location = dbo.Getlocation(fi.ukey)
    ,fi.ContainerCode
	,reason_nm = (select Name FROM Reason WHERE id=ReasonId AND junk = 0 and ReasonTypeID='Stock_Remove')
    ,psd.RefNo
    ,ColorID =dbo.GetColorMultipleID(psd.BrandId, isnull(psdsC.SpecValue, ''))
from Adjust_detail ad WITH (NOLOCK) 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.id = ad.POID and psd.SEQ1 = ad.Seq1 and psd.SEQ2 = ad.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join FtyInventory fi WITH (NOLOCK) on fi.POID = ad.POID and fi.Seq1 = ad.Seq1 and fi.Seq2 = ad.Seq2 and fi.Roll = ad.Roll and fi.StockType = ad.StockType and fi.Dyelot = ad.Dyelot
where ad.Id='{masterID}' 
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region -- Current Qty Vaild 判斷 --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings adjustqty = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode && (MyUtility.Convert.GetDecimal(this.CurrentDetailData["QtyBefore"]) == MyUtility.Convert.GetDecimal(e.FormattedValue)))
                {
                    MyUtility.Msg.WarningBox("Current Qty cannot equal Original Qty!");
                    e.Cancel = true;
                    return;
                }

                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue) <= 0)
                    {
                        dr["QtyAfter"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["AdjustQty"]);
                        return;
                    }
                    else
                    {
                        dr["QtyAfter"] = e.FormattedValue;
                        dr["AdjustQty"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["QtyAfter"]);
                        dr.EndEdit();
                    }
                }
            };

            adjustqty.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode)
                {
                    if (MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue) < 0 ||
                        MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["AdjustQty"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["QtyAfter"]);
                        return;
                    }
                    else
                    {
                        dr["AdjustQty"] = e.FormattedValue;
                        dr["QtyAfter"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["AdjustQty"]);
                        dr.EndEdit();
                    }
                }
            };
            #endregion

            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable poitems);
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
                        "ID,Name")
                    {
                        Width = 600,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.CurrentDetailData["reasonid"] = x[0]["id"];
                    this.CurrentDetailData["reason_nm"] = x[0]["name"];

                    if (x[0]["id"].Equals("00001") == false)
                    {
                        this.CurrentDetailData["ToPOID"] = string.Empty;
                        this.CurrentDetailData["ToSeq"] = string.Empty;
                        this.CurrentDetailData["ToSeq1"] = string.Empty;
                        this.CurrentDetailData["ToSeq2"] = string.Empty;

                        this.col_ToPoid.IsEditingReadOnly = true;
                        this.col_ToSeq.IsEditingReadOnly = true;
                    }
                    else
                    {
                        this.col_ToPoid.IsEditingReadOnly = false;
                        this.col_ToSeq.IsEditingReadOnly = false;
                    }

                    this.CurrentDetailData.EndEdit();
                }
            };
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["reasonid"] = string.Empty;
                        this.CurrentDetailData["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' and ReasonTypeID='Stock_Remove' AND junk = 0",
                            e.FormattedValue),
                            out DataRow dr,
                            null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.CurrentDetailData["reasonid"] = e.FormattedValue;
                            this.CurrentDetailData["reason_nm"] = dr["name"];
                        }
                    }

                    this.CurrentDetailData["ToPOID"] = string.Empty;
                    this.CurrentDetailData["ToSeq"] = string.Empty;
                    this.CurrentDetailData["ToSeq1"] = string.Empty;
                    this.CurrentDetailData["ToSeq2"] = string.Empty;

                }

                if (this.CurrentDetailData["reasonid"].Equals("00001") == false)
                {
                    this.col_ToPoid.IsEditingReadOnly = true;
                    this.col_ToSeq.IsEditingReadOnly = true;
                }
                else
                {
                    this.col_ToPoid.IsEditingReadOnly = false;
                    this.col_ToSeq.IsEditingReadOnly = false;
                }

                this.CurrentDetailData.EndEdit();
            };

            #endregion Seq 右鍵開窗

            #region ToPoid Seq
            DataGridViewGeneratorTextColumnSettings cs_topoid = new DataGridViewGeneratorTextColumnSettings();
            cs_topoid.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["ToPOID"] = string.Empty;
                    dr["ToSeq1"] = string.Empty;
                    dr["ToSeq2"] = string.Empty;
                    dr["ToSeq"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

                string sqlchk = $@"
select * from View_WH_Orders 
where ID = '{e.FormattedValue}'
and MDivisionID = '{Sci.Env.User.Keyword}'";
                if (!MyUtility.Check.Seek(sqlchk))
                {
                    MyUtility.Msg.WarningBox($"<Bulk SP#>:{e.FormattedValue} not found");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    string oldValue = dr["ToPOID"].ToString();
                    string newValue = e.FormattedValue.ToString();
                    if (oldValue != newValue)
                    {
                        dr["ToSeq1"] = string.Empty;
                        dr["ToSeq2"] = string.Empty;
                        dr["ToSeq"] = string.Empty;
                    }

                    dr["ToPoID"] = e.FormattedValue;
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorTextColumnSettings cs_toSeq = new DataGridViewGeneratorTextColumnSettings();
            cs_toSeq.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.CurrentDetailData["reasonid"].Equals("00001") == false)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;
                    if (MyUtility.Check.Empty(this.CurrentDetailData["ToPoID"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill in 'Bulk SP#' first.");
                        this.CurrentDetailData["ToSeq"] = string.Empty;
                        this.CurrentDetailData["ToSeq1"] = string.Empty;
                        this.CurrentDetailData["ToSeq2"] = string.Empty;
                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                    else
                    {
                        sqlcmd = $@"
select ID,[Seq] = concat(Seq1,' ',Seq2),seq1,seq2
from Po_Supp_Detail 
where ID = '{this.CurrentDetailData["ToPoID"]}'
";

                        DBProxy.Current.Select(null, sqlcmd, out DataTable poitems);

                        string columns = "ID,Seq";
                        string headersercap = "POID,Seq,";
                        string columnwidths = "14,10";
                        SelectItem item = new SelectItem(poitems, columns, columnwidths, this.CurrentDetailData["seq"].ToString(), headersercap)
                        {
                            Width = 500,
                        };
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        x = item.GetSelecteds();
                    }

                    this.CurrentDetailData["ToSeq"] = x[0]["seq"];
                    this.CurrentDetailData["ToSeq1"] = x[0]["seq1"];
                    this.CurrentDetailData["ToSeq2"] = x[0]["seq2"];

                    this.CurrentDetailData.EndEdit();
                }
            };
            cs_toSeq.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (dr == null)
                {
                    return;
                }

                if (!MyUtility.Check.Empty(this.CurrentDetailData["ToSeq"]) && MyUtility.Check.Empty(dr["ToPoID"]))
                {
                    MyUtility.Msg.WarningBox("Please fill in 'Bulk SP#' first.");
                    this.CurrentDetailData["ToSeq"] = string.Empty;
                    this.CurrentDetailData["ToSeq1"] = string.Empty;
                    this.CurrentDetailData["ToSeq2"] = string.Empty;
                    this.CurrentDetailData.EndEdit();
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["ToSeq"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["ToSeq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["ToSeq"] = string.Empty;
                        this.CurrentDetailData["ToSeq1"] = string.Empty;
                        this.CurrentDetailData["ToSeq2"] = string.Empty;
                    }
                    else
                    {
                        // check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (seq.Length < 2)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }

                        string sqlchk = $@"
select * from Po_Supp_Detail 
where ID = '{this.CurrentDetailData["ToPoID"]}'
and seq1 = '{seq[0]}'
and seq2 = '{seq[1]}'
";
                        if (!MyUtility.Check.Seek(sqlchk, out DataRow drCheck))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }
                        else
                        {
                            this.CurrentDetailData["Toseq"] = e.FormattedValue;
                            this.CurrentDetailData["Toseq1"] = seq[0];
                            this.CurrentDetailData["Toseq2"] = seq[1];
                        }
                    }
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;

            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true) // 1
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true) // 2
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true) // 4
            .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
            .Numeric("QtyAfter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0, settings: ns) // 5
            .Numeric("AdjustQty", header: "Remove Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0, settings: adjustqty) // 6
            .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 7
            .Text("Location", header: "Location", iseditingreadonly: true) // 7
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            .Text("reasonid", header: "Reason ID", settings: ts) // 8
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 9
            .Text("ToPOID", header: "Bulk SP#", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: cs_topoid).Get(out this.col_ToPoid) // 10
            .Text("ToSeq", header: "Bulk Seq", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: cs_toSeq).Get(out this.col_ToSeq) // 11
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.detailgrid.Columns["qtyafter"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["adjustqty"].DefaultCellStyle.BackColor = Color.Pink;

            // 設定detailGrid Rows 是否可以編輯
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !this.EditMode)
            {
                return;
            }

            DataRow curDr = this.detailgrid.GetDataRow(e.RowIndex);
            if (curDr["reasonid"].Equals("00001"))
            {
                this.col_ToPoid.IsEditingReadOnly = false;
                this.col_ToSeq.IsEditingReadOnly = false;
            }
            else
            {
                this.col_ToPoid.IsEditingReadOnly = true;
                this.col_ToSeq.IsEditingReadOnly = true;
                curDr["ToPOID"] = string.Empty;
                curDr["ToSeq"] = string.Empty;
                curDr["ToSeq1"] = string.Empty;
                curDr["ToSeq2"] = string.Empty;
                curDr.EndEdit();
            }
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P45_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
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

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P45", this);
        }
    }
}