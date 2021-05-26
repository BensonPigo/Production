﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Sci.Production.Automation;
using System.Linq;
using System.Threading.Tasks;
using Sci.Production.PublicPrg;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P50 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
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

        // 新增時預設資料

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

        // delete前檢查

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

        // edit前檢查

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

        // save前檢查 & 取id

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
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

        // grid 加工填值

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh

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

            string sqlcmd = $@"
update StockTaking
set Status = 'Checked'
, editname='{Env.User.UserID}', editdate=GETDATE() 
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

            // AutoWHACC WebAPI for Vstrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
                {
                    Task.Run(() => new Vstrong_AutoWHAccessory().SentStocktaking_Detail_New(dtDetail, "New"))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }

            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                Task.Run(() => new Gensong_AutoWHFabric().SentStocktaking_Detail_New(dtDetail, "New"))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        /// <inheritdoc/>
        protected override void ClickUncheck()
        {
            base.ClickUncheck();

            #region UnConfirmed 先檢查WMS是否傳送成功

            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

            bool accLock = true;
            bool fabricLock = true;

            // 主副料都有情況
            if (Prgs.Chk_Complex_Material(this.CurrentMaintain["ID"].ToString(), "Stocktaking_Detail"))
            {
                if (!Vstrong_AutoWHAccessory.SentStocktaking_Detail_Delete(dtDetail, "Lock", isComplexMaterial: true))
                {
                    accLock = false;
                }

                if (!Gensong_AutoWHFabric.SentStocktaking_Detail_Delete(dtDetail, "Lock", isComplexMaterial: true))
                {
                    fabricLock = false;
                }

                // 如果WMS連線都成功,則直接unconfirmed刪除
                if (accLock && fabricLock)
                {
                    Vstrong_AutoWHAccessory.SentStocktaking_Detail_Delete(dtDetail, "UnConfirmed", isComplexMaterial: true);
                    Gensong_AutoWHFabric.SentStocktaking_Detail_Delete(dtDetail, "UnConfirmed", isComplexMaterial: true);
                }
                else
                {
                    // 個別成功的,傳WMS UnLock狀態並且都不能刪除
                    if (accLock)
                    {
                        Vstrong_AutoWHAccessory.SentStocktaking_Detail_Delete(dtDetail, "UnLock", isComplexMaterial: true);
                    }

                    if (fabricLock)
                    {
                        Gensong_AutoWHFabric.SentStocktaking_Detail_Delete(dtDetail, "UnLock", isComplexMaterial: true);
                    }

                    return;
                }
            }
            else
            {
                if (!Vstrong_AutoWHAccessory.SentStocktaking_Detail_Delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }

                if (!Gensong_AutoWHFabric.SentStocktaking_Detail_Delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }
            #endregion

            string sqlcmd = $@"
update StockTaking
set Status = 'New'
, editname='{Env.User.UserID}', editdate=GETDATE() 
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定

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

        // Confirm

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P50"))
            {
                return;
            }
            #endregion

            DualResult result;
            #region store procedure parameters
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter();
            sp_StocktakingID.ParameterName = "@StocktakingID";
            sp_StocktakingID.Value = dr["id"].ToString();
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
            if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_StocktakingEncode", cmds)))
            {
                // MyUtility.Msg.WarningBox(result.Messages[1].ToString());
                Exception ex = result.GetException();
                MyUtility.Msg.WarningBox(ex.Message);
                return;
            }

            string adjID = MyUtility.GetValue.Lookup($"select id from Adjust where StocktakingID ='{this.CurrentMaintain["ID"].ToString()}'");
            if (!MyUtility.Check.Empty(adjID))
            {
                DataTable dtID = new DataTable();
                DataRow drID;
                dtID.Columns.Add("ID", typeof(string));
                drID = dtID.NewRow();
                drID["ID"] = adjID;
                dtID.Rows.Add(drID);

                // AutoWH ACC WebAPI for Vstrong
                if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
                {
                    Task.Run(() => new Vstrong_AutoWHAccessory().SentAdjust_Detail_New(dtID, "New"))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }

                // AutoWHFabric WebAPI for Gensong
                if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                {
                    Task.Run(() => new Gensong_AutoWHFabric().SentAdjust_Detail_New(dtID))
                   .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                }

                // confirmed adjust 調整Barcode
                this.FtyBarcodeData(adjID);
            }
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.id
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
,p1.Refno
,p1.colorid
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.ukey
,a.ftyinventoryukey
from dbo.StockTaking_detail as a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2= fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
outer apply(
	select top 1 IsWMS from MtlLocation ml
	inner join SplitString(dbo.Getlocation(fi.ukey),',') sp on sp.Data = ml.ID
	where ml.IsWMS= 1
)WMS
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // Import
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

        private void FtyBarcodeData(string AdjustID)
        {
            // Adjust 產生barcode
            DataTable dt = new DataTable();
            string sqlcmd = $@"
select
[Barcode1] = f.Barcode
,[OriBarcode] = fbOri.Barcode
,[balanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[NewBarcode] = isnull(fbOri.Barcode,f.Barcode)
,i2.Id,i2.POID,i2.Seq1,i2.Seq2,i2.StockType,i2.Roll,i2.Dyelot
from Production.dbo.Adjust_Detail i2
inner join Production.dbo.Adjust i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.POID
and f.Seq1 = i2.Seq1 and f.Seq2 = i2.Seq2
and f.Roll = i2.Roll and f.Dyelot = i2.Dyelot
and f.StockType = i2.StockType
outer apply(
	select *
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = i2.ID
)fbOri
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
and i2.id = '{AdjustID}'

";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

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
            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;
            DataTable resulttb;
            DualResult result;
            if (data_Fty_Barcode.Count >= 1)
            {
                // 更新Ftyinventory_Barcode 第二層
                upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(71, null, true);

                // 若Balance = 0 清空Ftyinventory.Barcode
                upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(70, null, false);

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
    }
}