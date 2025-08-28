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
    public partial class P43 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='O' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P43(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='O' and id='{0}'", transID);
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
            this.CurrentMaintain["Type"] = "O";
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
                if (MyUtility.Convert.GetDecimal(row["QtyAfter"]) == MyUtility.Convert.GetDecimal(row["QtyBefore"]))
                {
                    warningmsg.Append($@"SP#: {row["poid"].ToString().Trim()} SEQ#:{row["Seq"].ToString().Trim()} Roll#:{row["Roll"].ToString().Trim()}  
Original Qty and Current Qty can't be equal!!" + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append($@"SP#: {row["poid"]} SEQ#:{row["Seq"]} Roll#:{row["Roll"]}
Reason can't be empty!!" + Environment.NewLine);
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

            // 檢查物料不能有WMS Location && IsFromWMS = 0
            if (!PublicPrg.Prgs.Chk_WMS_Location_Adj((DataTable)this.detailgridbs.DataSource) && MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
            {
                MyUtility.Msg.WarningBox("Material Location or Adjust is from WMS system cannot save or confirmed. ", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "AO", "Adjust", (DateTime)this.CurrentMaintain["Issuedate"], 2, "ID", null);
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
        protected override void ClickSaveAfter()
        {
            // P43存檔,塞值 Adjust_Detail.StockType='O' , MdivisionID
            DualResult result;
            string sqlUpd = $"update Adjust_detail set StockType='O', MDivisionID='{Env.User.Keyword}' where id='{this.CurrentMaintain["ID"]}'";
            if (!(result = DBProxy.Current.Execute(null, sqlUpd)))
            {
                this.ShowErr(sqlUpd, result);
                return;
            }

            base.ClickSaveAfter();
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

            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
                {
                    this.gridicon.Remove.Visible = false;
                    this.gridicon.Remove.Enabled = false;
                    this.btnImport.Enabled = false;
                }
                else
                {
                    this.gridicon.Remove.Visible = true;
                    this.gridicon.Remove.Enabled = true;
                    this.btnImport.Enabled = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select 
AD2.id,
[Seq]= AD2.Seq1+' '+AD2.Seq2,
AD2.seq1,
AD2.seq2,
AD2.POID,
AD2.Roll,
AD2.Dyelot,
AD2.StockType,
AD2.MDivisionID,
Fa.Description,
AD2.QtyBefore,
AD2.QtyAfter,
[AdjustQty] = AD2.QtyAfter - AD2.QtyBefore,
psd.StockUnit,
[location]= dbo.Getlocation(FTI.Ukey),
FTI.ContainerCode,
AD2.ReasonId,
[reason_nm]=Reason.Name,
AD2.Ukey,
ColorID =dbo.GetColorMultipleID(psd.BrandId, isnull(psdsC.SpecValue, ''))
from Adjust_Detail AD2
inner join PO_Supp_Detail psd on psd.ID=AD2.POID 
inner join FtyInventory FTI on FTI.POID=AD2.POID and FTI.Seq1=AD2.Seq1
	and FTI.Seq2=AD2.Seq2 and FTI.Roll=AD2.Roll and FTI.StockType='O'
    and psd.SEQ1=AD2.Seq1 and psd.SEQ2=AD2.Seq2 and FTI.Dyelot = AD2.Dyelot
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
outer apply (
	select Description from Fabric where SCIRefno=psd.SCIRefno
) Fa
outer apply(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND ID= AD2.ReasonId and junk=0 ) Reason
where AD2.Id='{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region -- Current Qty Vaild 判斷 --

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();

            ns.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode && (MyUtility.Convert.GetDecimal(e.FormattedValue) == MyUtility.Convert.GetDecimal(this.CurrentDetailData["QtyBefore"])))
                {
                    MyUtility.Msg.WarningBox(@"Current Qty cannot be equal Original Qty!!");
                    e.Cancel = true;
                    return;
                }

                if (this.EditMode && string.Compare(this.CurrentDetailData["qtyafter"].ToString(), e.FormattedValue.ToString()) != 0)
                {
                    this.CurrentDetailData["QtyAfter"] = e.FormattedValue;
                    this.CurrentDetailData["AdjustQty"] = (decimal)e.FormattedValue - (decimal)this.CurrentDetailData["qtybefore"];
                }
            };

            #endregion
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
                    this.CurrentDetailData["reason_nm"] = x[0]["name"];
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
                        this.CurrentDetailData["reason_nm"] = string.Empty;
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
                            this.CurrentDetailData["reason_nm"] = dr["name"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗
            #region -- 欄位設定 --
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("QtyAfter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0, settings: ns)
            .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("StockUnit", header: "Unit", iseditingreadonly: true)
            .Text("location", header: "location", iseditingreadonly: true)
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            .Text("reasonid", header: "Reason ID", settings: ts)
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20));
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.detailgrid.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
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

            #region 檢查物料不能有WMS Location && IsFromWMS = 0
            if (!PublicPrg.Prgs.Chk_WMS_Location_Adj((DataTable)this.detailgridbs.DataSource) && MyUtility.Check.Empty(this.CurrentMaintain["IsFromWMS"]))
            {
                MyUtility.Msg.WarningBox("Material Location or Adjust is from WMS system cannot save or confirmed. ", "Warning");
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "Adjust_Detail") == false)
            {
                return;
            }
            #endregion

            // 檢查負數庫存
            if (!Prgs.CheckAdjustBalance(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), isConfirm: true))
            {
                return;
            }

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

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            var frm = new P43_Import(this.CurrentMaintain, dt);
            frm.ShowDialog(this);
            this.RenewData();
            dt.DefaultView.Sort = "poid,seq,Roll";
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

        private void UpdMDivisionPoDetail()
        {
            DataTable datacheck;
            DualResult result;
            string sqlcmd = string.Format(
                @"
SELECT 
    md.POID
    ,md.Seq1
    ,md.seq2
    ,[FTYLobQty] = sum(FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty)
FROM FtyInventory FTI
inner join Adjust_detail AD2 on FTI.POID=AD2.POID 
and FTI.Seq1=AD2.Seq1
and FTI.Seq2=AD2.Seq2 
and FTI.Roll=AD2.Roll
and FTI.Dyelot = AD2.Dyelot
inner join MDivisionPoDetail md on FTI.POID=md.POID and fti.Seq1=md.Seq1 and fti.Seq2=md.Seq2
WHERE FTI.StockType='O' and AD2.ID = '{0}'
group by md.POID,md.Seq1,md.Seq2", this.CurrentMaintain["id"].ToString());
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    if (!(result = DBProxy.Current.Execute(null, string.Format(
                        @"
Update MDivisionPoDetail
set LobQty = {0}
where POID = '{1}'
and Seq1 = '{2}' and Seq2 = '{3}' ",
                        MyUtility.Convert.GetDecimal(dr["FTYLobQty"]),
                        dr["poid"].ToString(),
                        dr["seq1"].ToString(),
                        dr["seq2"].ToString()))))
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
            }
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P43", this);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            WH_Print p = new WH_Print(this.CurrentMaintain, "P43")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                #region -- 撈表頭資料 --
                List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@MDivision", this.CurrentMaintain["MDivisionID"]),
                new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()),
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

                string rptTitle = dt.Rows[0]["NameEN"].ToString();
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new ReportParameter("ID", this.CurrentMaintain["ID"].ToString()));
                report.ReportParameters.Add(new ReportParameter("Remark", this.CurrentMaintain["Remark"].ToString()));
                report.ReportParameters.Add(new ReportParameter("CDate", ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["IssueDate"])).ToShortDateString()));
                report.ReportParameters.Add(new ReportParameter("FtyGroup", this.CurrentMaintain["FactoryID"].ToString()));

                #endregion
                #region -- 撈表身資料 --
                string sqlcmd = @"
select ad.POID
	, [SEQ] = ad.Seq1 + '-' + ad.Seq2
	, [DESC] =IIF((ad.ID = lag(ad.ID,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll) 
			    AND(ad.seq1 = lag(ad.seq1,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll))
			    AND(ad.seq2 = lag(ad.seq2,1,'')over (order by ad.POID, ad.seq1, ad.seq2, ad.Dyelot, ad.Roll))) 
			    ,''
                ,dbo.getMtlDesc(ad.poid, ad.seq1, ad.seq2, 2, 0))
	, [Location] = dbo.Getlocation(fi.ukey)
    , fi.ContainerCode
	, p.StockUnit
	, ad.Roll
	, ad.Dyelot
	, [QTY] = isnull(ad.QtyAfter,0.00) - isnull(ad.QtyBefore,0.00)
    , [TotalQTY] = sum(isnull(ad.QtyAfter,0.00) - isnull(ad.QtyBefore,0.00)) OVER (PARTITION BY ad.POID ,ad.seq1, ad.seq2)
from Adjust_Detail ad WITH (NOLOCK)
left join PO_Supp_Detail p WITH (NOLOCK) on p.ID = ad.PoId and p.seq1 = ad.SEQ1 and p.SEQ2 = ad.seq2
left join FtyInventory fi WITH (NOLOCK) on ad.poid = fi.poid and ad.seq1 = fi.seq1 and ad.seq2 = fi.seq2
						and ad.roll = fi.roll and ad.stocktype = fi.stocktype and ad.Dyelot = fi.Dyelot
where ad.ID = @ID
order by ad.POID, SEQ, ad.Dyelot, ad.Roll
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
                List<P34_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P34_PrintData()
                    {
                        POID = row1["POID"].ToString().Trim(),
                        SEQ = row1["SEQ"].ToString().Trim(),
                        DESC = row1["DESC"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        StockUnit = row1["StockUnit"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        DYELOT = row1["Dyelot"].ToString().Trim(),
                        QTY = row1["Qty"].ToString().Trim(),
                        TotalQTY = row1["TotalQTY"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;
                #endregion
                #region 指定是哪個 RDLC

                // DualResult result;
                Type reportResourceNamespace = typeof(P13_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P34_Print.rdlc";

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

            return base.ClickPrint();
        }
    }
}
