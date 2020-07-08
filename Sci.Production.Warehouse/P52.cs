using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P52 : Sci.Win.Tems.Input6
    {
        public P52(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            #region Set ComboBox Dictionary
            Dictionary<string, string> dicForwardBack = new Dictionary<string, string>();
            dicForwardBack.Add("F", "Forward");
            dicForwardBack.Add("B", "Back");
            this.comboBoxForwardBack.DataSource = new BindingSource(dicForwardBack, null);
            this.comboBoxForwardBack.ValueMember = "key";
            this.comboBoxForwardBack.DisplayMember = "value";
            Dictionary<string, string> dicStockType = new Dictionary<string, string>();
            dicStockType.Add("B", "Bulk");
            dicStockType.Add("O", "Scrap");
            this.comboBoxStockType.DataSource = new BindingSource(dicStockType, null);
            this.comboBoxStockType.ValueMember = "key";
            this.comboBoxStockType.DisplayMember = "value";
            #endregion
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region SP#
            DataGridViewGeneratorTextColumnSettings gridSPSetting = new DataGridViewGeneratorTextColumnSettings();
            gridSPSetting.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                string strOldSP = this.CurrentDetailData["POID"].ToString();
                string strNewSP = e.FormattedValue.ToString();
                #region NewSP is Empty 該筆資料自動清空
                if (strNewSP.Empty())
                {
                    this.CurrentDetailData["POID"] = string.Empty;
                    this.gridRowDataReSet(this.CurrentDetailData);
                    return;
                }
                #endregion
                #region check SP#
                bool boolCheckSPNum = MyUtility.Check.Seek(string.Format(
                    @"
select Linv.*
from LocalInventory Linv
inner join orders o on Linv.OrderID = o.ID
where   Linv.OrderID = '{0}'
        and o.MDivisionID = '{1}'", strNewSP,
                    Sci.Env.User.Keyword));
                if (boolCheckSPNum)
                {
                    if (!strOldSP.EqualString(strNewSP))
                    {
                        this.CurrentDetailData["POID"] = strNewSP;
                        this.gridRowDataReSet(this.CurrentDetailData);
                    }
                }
                else
                {
                    e.Cancel = true;
                    this.gridRowDataReSet(this.CurrentDetailData);
                    MyUtility.Msg.WarningBox(string.Format("SP# : {0} not found.", strNewSP));
                }
                #endregion
            };
            #endregion
            #region Ref#
            DataGridViewGeneratorTextColumnSettings gridRefNumSetting = new DataGridViewGeneratorTextColumnSettings();
            gridRefNumSetting.EditingMouseDown += (s, e) =>
            {
                if (e.Button != System.Windows.Forms.MouseButtons.Right || !this.EditMode)
                {
                    return;
                }
                #region Ref# 右鍵開窗
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                    string.Format(
                    @"
select  LInv.Refno
        , LInv.ThreadColorID
        , LI.Description
        , Location = case '{0}'
							when 'B' then LInv.ALocation
							when 'O' then LInv.CLocation
					 end
        , QtyBefore = BookQty.value
        , LInv.UnitID
from LocalInventory LInv
Inner join LocalItem LI on LInv.Refno = LI.Refno
outer apply (
	select value = case '{0}'
						when 'B' then LInv.InQty - LInv.OutQty + LInv.AdjustQty
						when 'O' then LInv.LobQty
				   end
) BookQty
Where LInv.OrderID = '{1}'", this.CurrentMaintain["StockType"],
                    this.CurrentDetailData["POID"]), null, null);
                #endregion
                DialogResult result = item.ShowDialog();
                #region 開窗後選擇的結果
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    this.CurrentDetailData["Refno"] = item.GetSelecteds()[0]["Refno"];
                    this.CurrentDetailData["Color"] = item.GetSelecteds()[0]["ThreadColorID"];
                    this.CurrentDetailData["Location"] = item.GetSelecteds()[0]["Location"];
                    this.CurrentDetailData["QtyBefore"] = item.GetSelecteds()[0]["QtyBefore"];
                    this.CurrentDetailData["UnitID"] = item.GetSelecteds()[0]["UnitID"];
                    this.CurrentDetailData["Variance"] = Convert.ToDecimal(this.CurrentDetailData["QtyAfter"]) - Convert.ToDecimal(this.CurrentDetailData["QtyBefore"]);
                    this.CurrentDetailData.EndEdit();
                }
                #endregion
            };
            #endregion
            #region Color
            DataGridViewGeneratorTextColumnSettings gridColorSetting = new DataGridViewGeneratorTextColumnSettings();
            gridColorSetting.EditingMouseDown += (s, e) =>
            {
                if (e.Button != System.Windows.Forms.MouseButtons.Right || !this.EditMode)
                {
                    return;
                }
                #region Ref# 右鍵開窗
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                    string.Format(
                    @"
select  LInv.ThreadColorID
        , LI.Description
        , Location = case '{0}'
							when 'B' then LInv.ALocation
							when 'O' then LInv.CLocation
					 end
        , QtyBefore = BookQty.value
        , LInv.UnitID
from LocalInventory LInv
Inner join LocalItem LI on LInv.Refno = LI.Refno
outer apply (
	select value = case '{0}'
						when 'B' then LInv.InQty - LInv.OutQty + LInv.AdjustQty
						when 'O' then LInv.LobQty
				   end
) BookQty
Where   LInv.OrderID = '{1}'
        and LInv.Refno = '{2}'", this.CurrentMaintain["StockType"],
                    this.CurrentDetailData["POID"],
                    this.CurrentDetailData["Refno"]), null, null);
                #endregion
                DialogResult result = item.ShowDialog();
                #region 開窗後選擇的結果
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    this.CurrentDetailData["Color"] = item.GetSelecteds()[0]["ThreadColorID"];
                    this.CurrentDetailData["Location"] = item.GetSelecteds()[0]["Location"];
                    this.CurrentDetailData["QtyBefore"] = item.GetSelecteds()[0]["QtyBefore"];
                    this.CurrentDetailData["UnitID"] = item.GetSelecteds()[0]["UnitID"];
                    this.CurrentDetailData["Variance"] = Convert.ToDecimal(this.CurrentDetailData["QtyAfter"]) - Convert.ToDecimal(this.CurrentDetailData["QtyBefore"]);
                    this.CurrentDetailData.EndEdit();
                }
                #endregion
            };
            #endregion
            #region QtyAfter
            DataGridViewGeneratorNumericColumnSettings gridQtyAfterSetting = new DataGridViewGeneratorNumericColumnSettings();
            gridQtyAfterSetting.CellValidating += (s, e) =>
            {
                this.CurrentDetailData["QtyAfter"] = e.FormattedValue;
                this.CurrentDetailData["Variance"] = Convert.ToDecimal(this.CurrentDetailData["QtyAfter"]) - Convert.ToDecimal(this.CurrentDetailData["QtyBefore"]);
            };
            #endregion
            #region Set Grid Columns
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("POID", header: "SP#", settings: gridSPSetting)
                .Text("Refno", header: "Ref#", iseditingreadonly: true, settings: gridRefNumSetting)
                .Text("Color", header: "Color", iseditingreadonly: true, settings: gridColorSetting)
                .Text("Location", header: "Book Location", iseditingreadonly: true)
                .Numeric("QtyBefore", header: "Book Qty", iseditingreadonly: true)
                .Numeric("QtyAfter", header: "Actual Qty", settings: gridQtyAfterSetting)
                .Numeric("Variance", header: "Variance", iseditingreadonly: true)
                .Text("UnitID", header: "Unit", iseditingreadonly: true);
            #endregion
        }

        /// <summary>
        /// 清空一列資料
        /// </summary>
        /// <param name="dataRow">需要清空的資料列</param>
        private void gridRowDataReSet(DataRow dataRow)
        {
            dataRow["Refno"] = string.Empty;
            dataRow["Color"] = string.Empty;
            dataRow["Location"] = string.Empty;
            dataRow["QtyBefore"] = 0;
            dataRow["QtyAfter"] = 0;
            dataRow["Variance"] = 0;
            dataRow["UnitID"] = string.Empty;
            dataRow.EndEdit();
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string strMasterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select	STLD.ID
        , STLD.Ukey
        , STLD.POID
		, STLD.Refno
		, STLD.Color
        , STLD.StockType
		, Location = case STLD.StockType
							when 'B' then LI.ALocation
							when 'O' then LI.CLocation
					 end
		, STLD.QtyBefore
		, STLD.QtyAfter
		, Variance = STLD.QtyAfter - STLD.QtyBefore
		, LI.UnitId
from StocktakingLocal_Detail STLD
left join LocalInventory LI on  STLD.POID = LI.OrderID
								 and STLD.Refno = LI.Refno
								 and STLD.Color = LI.ThreadColorID
where id = '{0}'
order by STLD.POID, STLD.Refno, STLD.Color
", strMasterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void comboBoxStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.EditMode || this.detailgrid.Rows.Count == 0)
            {
                return;
            }

            DataTable dtDetailGrid = (DataTable)((BindingSource)this.detailgrid.DataSource).DataSource;
            #region 移除表身所有資料
            for (int i = 0; i < dtDetailGrid.Rows.Count;)
            {
                DataRowState rowState = dtDetailGrid.Rows[i].RowState;
                if (rowState != DataRowState.Deleted && rowState != DataRowState.Detached)
                {
                    dtDetailGrid.Rows[i].Delete();
                    if (rowState == DataRowState.Added)
                    {
                        continue;
                    }
                }

                i++;
            }
            #endregion
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["IssueDate"] = DateTime.Today;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Type"] = "F";
            this.CurrentMaintain["StockType"] = "B";
            this.CurrentMaintain["Status"] = "New";
        }

        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain != null && this.CurrentMaintain["Status"].EqualString("Confirmed"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain != null && this.CurrentMaintain["Status"].EqualString("Confirmed"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        protected override bool ClickSaveBefore()
        {
            DualResult result;
            DataTable dtDetailGrid = (DataTable)((BindingSource)this.detailgrid.DataSource).DataSource;
            #region 表頭 StockType 不可為空值
            if (this.CurrentMaintain != null && this.CurrentMaintain["StockType"].Empty())
            {
                MyUtility.Msg.WarningBox("Stock Type can't be empty.");
                return false;
            }
            #endregion
            #region 表身【SP#, Refno, Color】不可重複
            DataTable dtCheckDuplicateData;
            string strCheckDuplicateData = @"
select  Poid
        , Refno
        , Color
from (
    select  Poid
            , Refno
            , Color
            , value = count(*)
    from #tmp
    group by Poid, Refno, Color
) countDuplicate
where countDuplicate.value > 1";
            result = MyUtility.Tool.ProcessWithDatatable(dtDetailGrid, null, strCheckDuplicateData, out dtCheckDuplicateData);
            if (result)
            {
                if (dtCheckDuplicateData != null && dtCheckDuplicateData.Rows.Count > 0)
                {
                    StringBuilder strErrMsg = new StringBuilder();
                    foreach (DataRow dr in dtCheckDuplicateData.Rows)
                    {
                        strErrMsg.Append(string.Format("SP#:{0} Refno#:{1} Color:{2}'s", dr["Poid"], dr["Refno"].ToString().Trim(), dr["Color"]) + Environment.NewLine);
                    }

                    strErrMsg.Append("SP#, Refno#, Color can't be duplicate.");
                    MyUtility.Msg.WarningBox(strErrMsg.ToString());
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox(result.Description, "表身【SP#, Refno, Color】不可重複");
                return false;
            }
            #endregion
            #region 移除表身 SP# is empty 的資料
            for (int i = 0; i < dtDetailGrid.Rows.Count;)
            {
                DataRowState rowState = dtDetailGrid.Rows[i].RowState;
                if (rowState != DataRowState.Deleted && rowState != DataRowState.Detached)
                {
                    if (dtDetailGrid.Rows[i]["POID"].Empty())
                    {
                        dtDetailGrid.Rows[i].Delete();
                        if (rowState == DataRowState.Added)
                        {
                            continue;
                        }
                    }
                }

                i++;
            }
            #endregion
            #region Actual Qty 不可為負數
            foreach (DataRow dr in ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (Convert.ToDecimal(dr["QtyAfter"]) < 0)
                    {
                        MyUtility.Msg.WarningBox("ActualQty can't be negative number");
                        return false;
                    }
                }
            }
            #endregion
            #region 檢查 【Forward : 表身至少要有一筆資料】【Back : 表身允許為空值】
            bool boolCheckDetailFail = false;
            switch (this.CurrentMaintain["Type"].ToString())
            {
                case "F":
                    if (this.detailgrid.Rows.Count == 0)
                    {
                        boolCheckDetailFail = true;
                    }

                    break;
            }

            if (boolCheckDetailFail)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty");
                return false;
            }
            #endregion
            #region 表身資料補上 StockType
            foreach (DataRow dr in dtDetailGrid.Rows)
            {
                if (dr.RowState != DataRowState.Detached && dr.RowState != DataRowState.Deleted)
                {
                    dr["StockType"] = this.CurrentMaintain["StockType"];
                }
            }
            #endregion
            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "SL", "StocktakingLocal", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }
            #endregion
            return base.ClickSaveBefore();
        }

        protected override void ClickConfirm()
        {
            DualResult result;
            DataTable dtDetailGrid = (DataTable)((BindingSource)this.detailgrid.DataSource).DataSource;
            #region 檢查庫存是否足夠
            DataTable dtCheckStockQty;
            string strCheckStockQty = string.Format(
                @"
select  #tmp.POID
        , #tmp.Refno
        , #tmp.Color
        , Balance = stockQty.value
        , AdjustQty = StockTackingAdjust.value
from #tmp
left join LocalInventory LInv on  #tmp.POID = LInv.OrderID
								  and #tmp.Refno = LInv.Refno
								  and #tmp.Color = LInv.ThreadColorID
outer apply (
    select value = case '{0}'
                        when 'B' then isnull (LInv.InQty, 0) - isnull (LInv.OutQty, 0) + isnull (LInv.AdjustQty, 0)
                        when 'O' then isnull (LInv.LobQty, 0)
                   end
) stockQty 
outer apply (
    select value = #tmp.QtyAfter - #tmp.QtyBefore
) StockTackingAdjust
where stockQty.value + StockTackingAdjust.value < 0
", this.comboBoxStockType.SelectedValue);
            result = MyUtility.Tool.ProcessWithDatatable(dtDetailGrid, null, strCheckStockQty, out dtCheckStockQty);
            if (result)
            {
                if (dtCheckStockQty != null && dtCheckStockQty.Rows.Count > 0)
                {
                    StringBuilder strErrMsg = new StringBuilder();
                    foreach (DataRow dr in dtCheckStockQty.Rows)
                    {
                        strErrMsg.Append(string.Format(
                            "SP#:{0} Refno#:{1} Color:{2}'s balance:{3} is less than Adjust qty:{4}",
                            dr["POID"],
                            dr["Refno"],
                            dr["Color"],
                            dr["Balance"],
                            dr["AdjustQty"]) + Environment.NewLine);
                    }

                    strErrMsg.Append("Balacne Qty is not enough!!");
                    MyUtility.Msg.WarningBox(strErrMsg.ToString(), "檢查庫存是否足夠");
                    return;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox(result.Description);
                return;
            }
            #endregion
            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            using (_transactionscope)
            using (sqlConn)
            {
                string strAdjustID = string.Empty;
                #region 調整庫存
                DataTable dtUpdateStockQty;
                string strUpdateStockQty = string.Format(
                    @"
merge LocalInventory as LI
using #tmp as tmp on  LI.OrderID = tmp.POID
								   and LI.Refno = tmp.Refno
								   and LI.ThreadColorID = tmp.Color
when matched then 
		update set {0};",
                    this.CurrentMaintain["StockType"].EqualString("O") ? "LI.LobQty = LI.LobQty + (tmp.QtyAfter - tmp.QtyBefore)" : "LI.AdjustQty = LI.AdjustQty + (tmp.QtyAfter - tmp.QtyBefore)");
                result = MyUtility.Tool.ProcessWithDatatable(dtDetailGrid, null, strUpdateStockQty, out dtUpdateStockQty);
                if (!result)
                {
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox(result.Description);
                    return;
                }
                #endregion
                if (dtDetailGrid.AsEnumerable().Any(row => !row["QtyAfter"].EqualDecimal(row["QtyBefore"])))
                {
                    dtDetailGrid = dtDetailGrid.AsEnumerable().Where(row => !row["QtyAfter"].EqualDecimal(row["QtyBefore"])).CopyToDataTable();
                    #region 庫存足夠，【QtyAfter != QtyBefore】建立調整單
                    #region 取 Adjust 單號
                    string strLBLC = this.CurrentMaintain["StockType"].EqualString("O") ? "LC" : "LB";
                    strAdjustID = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + strLBLC, "AdjustLocal", (DateTime)this.CurrentMaintain["Issuedate"], 2, "ID", null);
                    if (MyUtility.Check.Empty(strAdjustID))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Get document ID fail!!");
                        return;
                    }
                    #endregion
                    #region 新增 Adjust 表頭
                    string strInsertAdjustLocal = string.Format(
                        @"
insert into AdjustLocal 
    (ID				, MDivisionID	, FactoryID	, IssueDate	, Remark
    , Status		, AddName		, AddDate	, Type		, StocktakingID) 
values 
    ('{0}'			, '{1}'			, '{2}'		, GETDATE()	, 'Add by stocktaking'
    , 'Confirmed'	, '{3}'			, GETDATE()	, '{4}'		, '{5}');",
                        strAdjustID,
                        this.CurrentMaintain["MDivisionID"],
                        this.CurrentMaintain["FactoryID"],
                        this.CurrentMaintain["AddName"],
                        this.CurrentMaintain["StockType"].EqualString("O") ? "C" : "A",
                        this.CurrentMaintain["ID"]);
                    result = DBProxy.Current.Execute(null, strInsertAdjustLocal);
                    if (!result)
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.Description, "Insert Adjust Title");
                        return;
                    }
                    #endregion
                    #region 新增 Adjust 表身
                    DataTable dtInsertAdjustLocalDetail;
                    string strInsertAdjustLocalDetail = string.Format(
                        @"
insert into AdjustLocal_Detail (
        ID				    , MDivisionID	    , POID		    , Refno		        , Color
        , StockType         , QtyBefore		    , QtyAfter	    , ReasonId
)
select  '{0}'			    , '{1}'			    , #tmp.POID	    , #tmp.Refno        , #tmp.Color
        , '{2}'              , #tmp.QtyBefore	, #tmp.QtyAfter	, ReasonID.value
from #tmp
outer apply (
    select value = iif (#tmp.QtyAfter > #tmp.QtyBefore, '00010', '00011')
) ReasonID", strAdjustID,
                        this.CurrentMaintain["MDivisionID"],
                        this.CurrentMaintain["StockType"]);
                    result = MyUtility.Tool.ProcessWithDatatable(dtDetailGrid, null, strInsertAdjustLocalDetail, out dtInsertAdjustLocalDetail);
                    if (!result)
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.Description, "Insert Adjust Detail");
                        return;
                    }
                    #endregion
                    #endregion
                }
                #region 庫存足夠，【QtyAfter = QtyBefore】不建立調整單
                #endregion
                #region Update Status & Adjust ID
                string strUpdateStatus = string.Format(
                    @"
update STL
set STL.Status = 'Confirmed'
    , STL.AdjustID = '{1}'
from StocktakingLocal STL
where STL.ID = '{0}'", this.CurrentMaintain["ID"],
                    strAdjustID);
                result = DBProxy.Current.Execute(null, strUpdateStatus);
                if (!result)
                {
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox(result.Description, "Update Status");
                    return;
                }
                #endregion
                _transactionscope.Complete();
                _transactionscope.Dispose();
            }

            base.ClickConfirm();
        }

        protected override bool ClickPrint()
        {
            DataTable dtDetailGrid = (DataTable)((BindingSource)this.detailgrid.DataSource).DataSource;
            var frm = new P52_Print(this.CurrentMaintain, dtDetailGrid);
            frm.ShowDialog();
            return false;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            #region checkStockType
            if (this.CurrentMaintain["StockType"].Empty())
            {
                return;
            }
            #endregion
            var frm = new P52_Import(this.CurrentMaintain["StockType"]);
            frm.ShowDialog();
            if (frm.getBoolImport())
            {
                this.ShowWaitMessage("Import data...");
                #region Import Data
                DataTable dtResultImportData = frm.getResultImportDatas();
                if (dtResultImportData != null && dtResultImportData.Rows.Count > 0)
                {
                    DataTable dtDetailGrid = (DataTable)((BindingSource)this.detailgrid.DataSource).DataSource;
                    foreach (DataRow dr in dtResultImportData.Rows)
                    {
                        if (!dtDetailGrid.AsEnumerable().Any(row => row["Poid"].EqualString(dr["Poid"])
                                                                    && row["Refno"].EqualString(dr["Refno"])
                                                                    && row["Color"].EqualString(dr["Color"])))
                        {
                            DataRow newDr = dtDetailGrid.NewRow();
                            newDr["Poid"] = dr["Poid"];
                            newDr["Refno"] = dr["Refno"];
                            newDr["Color"] = dr["Color"];
                            newDr["UnitID"] = dr["UnitID"];
                            newDr["Location"] = dr["Location"];
                            newDr["QtyBefore"] = dr["QtyBefore"];
                            newDr["QtyAfter"] = 0;
                            newDr["Variance"] = dr["Variance"];
                            dtDetailGrid.Rows.Add(newDr);
                        }
                    }
                }
                #endregion
                this.HideWaitMessage();
            }
        }
    }
}
