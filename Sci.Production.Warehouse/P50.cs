using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P50 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer();
            this.viewer.Dock = DockStyle.Fill;
            this.Controls.Add(this.viewer);

            this.DefaultFilter = string.Format("Type='F' and MDivisionID = '{0}'", Env.User.Keyword);
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P50(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='F' and id='{0}'", transID);
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.IsSupportCheck = false;
            this.IsSupportUncheck = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MyUtility.Tool.SetupCombox(this.comboStockType, 2, 1, "B,Bulk,I,Inventory");
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "F";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
            this.CurrentMaintain["stocktype"] = "B";
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

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
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
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format("SP#: {0} Seq#: {1}-{2} can't be empty", row["poid"], row["seq1"], row["seq2"]) + Environment.NewLine);
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "SF", "StockTaking", (DateTime)this.CurrentMaintain["Issuedate"]);
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

            #region Automation = 1 && Location is WMS then open Check,UnCheck

            bool existsWMS = false;
            if (this.DetailDatas != null && this.DetailDatas.Count > 0)
            {
                var wMS_List = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(x => x["IsWMS"].EqualDecimal(1)).ToList();
                if (wMS_List.Any())
                {
                    existsWMS = true;
                }
            }

            if (UtilityAutomation.IsAutomationEnable && existsWMS)
            {
                this.IsSupportUncheck = true;
                this.IsSupportCheck = true;

                this.CheckChkValue = "New";
                this.ApvChkValue = "Checked";
                this.UncheckChkValue = "Checked";

                if (this.EditMode == true && this.CurrentMaintain["Status"].ToString() == "Checked")
                {
                    this.dateIssueDate.Enabled = false;
                    this.comboStockType.Enabled = false;
                    this.editRemark.Enabled = false;
                    this.gridicon.Remove.Visible = false;
                    this.gridicon.Remove.Enabled = false;
                    this.btngenerate.Enabled = false;
                }
            }
            else
            {
                this.IsSupportUncheck = false;
                this.IsSupportCheck = false;
                this.ApvChkValue = "New";
                this.CheckChkValue = string.Empty;
                this.UnApvChkValue = string.Empty;

                this.dateIssueDate.Enabled = true;
                this.comboStockType.Enabled = true;
                this.editRemark.Enabled = true;
                this.gridicon.Remove.Visible = true;
                this.gridicon.Remove.Enabled = true;
                this.btngenerate.Enabled = this.EditMode;
            }

            this.EnsureToolbarExt();
            #endregion
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
        }

        /// <inheritdoc/>
        protected override void ClickCheck()
        {
            base.ClickCheck();

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }
            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "StockTaking_detail") == false)
            {
                return;
            }
            #endregion

            string sqlcmd = $@"
update StockTaking
set Status = 'Checked'
, editname='{Env.User.UserID}', editdate=GETDATE() 
where id = '{this.CurrentMaintain["ID"]}'
";
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Checked successful");
        }

        /// <inheritdoc/>
        protected override void ClickUncheck()
        {
            base.ClickUncheck();

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新
            DataTable detailTable = (DataTable)this.detailgridbs.DataSource;

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock(detailTable, detailTable, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            string sqlcmd = $@"
update StockTaking
set Status = 'New'
, editname='{Env.User.UserID}', editdate=GETDATE() 
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                Prgs_WMS.WMSUnLock(false, detailTable, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, detailTable);
                this.ShowErr(sqlcmd, result);
                return;
            }

            // PMS 更新之後,才執行WMS
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            Prgs_WMS.WMSprocess(false, detailTable, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, detailTable, typeCreateRecord: 1, autoRecord: autoRecordList);
            Prgs_WMS.WMSprocess(false, detailTable, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, detailTable, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnChecked successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Text("Location", header: "Book Location", iseditingreadonly: true) // 4
            .Text("ContainerCode", header: "Container Code", width: Widths.AnsiChars(18), iseditingreadonly: true) // 4
            .Numeric("qtybefore", header: "Book Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 5
            .Numeric("qtyafter", header: "Actual Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
            .Numeric("variance", header: "Variance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
            .Text("refno", header: "Ref#", iseditingreadonly: true) // 8
            .Text("Colorid", header: "Color", iseditingreadonly: true) // 9
            .Text("stockunit", header: "Stock Unit", iseditingreadonly: true) // 10
            .ComboBox("FabricType", header: "Material Type", iseditable: false).Get(out cbb_fabrictype) // 11
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 12
            ;

            #endregion 欄位設定
            cbb_fabrictype.DataSource = new BindingSource(this.di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
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

            DualResult result;
            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P50"))
            {
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "StockTaking_detail") == false)
            {
                return;
            }
            #endregion

            #region store procedure parameters
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter();
            sp_StocktakingID.ParameterName = "@StocktakingID";
            sp_StocktakingID.Value = this.CurrentMaintain["id"].ToString();
            cmds.Add(sp_StocktakingID);
            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivisionid";
            sp_mdivision.Value = Env.User.Keyword;
            cmds.Add(sp_mdivision);
            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@Factoryid";
            sp_factory.Value = Env.User.Factory;
            cmds.Add(sp_factory);
            System.Data.SqlClient.SqlParameter sp_loginid = new System.Data.SqlClient.SqlParameter();
            sp_loginid.ParameterName = "@loginid";
            sp_loginid.Value = Env.User.UserID;
            cmds.Add(sp_loginid);
            #endregion

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            if (!(result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory)))
            {
                this.ShowErr(result);
                return;
            }

            DataTable dtAdjust_Detail = new DataTable();
            string formName = string.Empty;
            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_StocktakingEncode", cmds)))
                    {
                        throw result.GetException();
                    }

                    string sqlcmd = $@"select sd.* from Adjust s inner join Adjust_Detail sd on sd.ID = s.ID where StocktakingID = '{this.CurrentMaintain["ID"]}'";
                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out dtAdjust_Detail)))
                    {
                        throw result.GetException();
                    }

                    if (dtAdjust_Detail.Rows.Count > 0)
                    {
                        string adjustID = MyUtility.Convert.GetString(dtAdjust_Detail.Rows[0]["ID"]);
                        formName = adjustID.Contains("AI") ? "P34" : "P35";

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, dtAdjust_Detail, formName, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }
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
            if (dtAdjust_Detail.Rows.Count > 0)
            {
                Prgs_WMS.WMSprocess(false, dtAdjust_Detail, formName, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select a.id
,a.PoId,a.Seq1,a.Seq2
,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,[location] = dbo.Getlocation(fi.ukey)
,[IsWMS] = IIF(WMS.IsWMS = 1, 1, 0)
,a.QtyBefore
,a.QtyAfter
,a.QtyAfter - a.QtyBefore as variance
,a.StockType
,psd.Refno
,ColorID = isnull(psdsC.SpecValue, '')
,psd.FabricType
,psd.stockunit
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.ukey
,a.ftyinventoryukey
,FI.ContainerCode
from dbo.StockTaking_detail as a WITH (NOLOCK) 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.PoId and psd.seq1 = a.SEQ1 and psd.SEQ2 = a.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2= fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
outer apply(
	select top 1 IsWMS from MtlLocation ml
	inner join SplitString(dbo.Getlocation(fi.ukey),',') sp on sp.Data = ml.ID
	where ml.IsWMS= 1
)WMS
Where a.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void Btngenerate_Click(object sender, EventArgs e)
        {
            var frm = new P50_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void ComboStockType_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.comboStockType.SelectedValue) && this.comboStockType.SelectedValue != this.comboStockType.OldValue)
            {
                if (this.detailgridbs.DataSource != null)
                {
                    ((DataTable)this.detailgridbs.DataSource).Rows.Clear();  // 清空表身資料
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P50_Print p = new P50_Print(this.CurrentDataRow);
            p.ShowDialog();
            return true;
        }
    }
}