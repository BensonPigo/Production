﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P19_TransferWKImport
    /// </summary>
    public partial class P19_TransferWKImport : Win.Subs.Base
    {
        private readonly string mainTransferOutID = string.Empty;
        private DataTable mainDetail;
        private string M;

        /// <inheritdoc/>
        public P19_TransferWKImport(string transferOutID, DataTable mainDetail, string mdivisionid)
        {
            this.InitializeComponent();
            this.mainTransferOutID = transferOutID;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",,F,Fabric,A,Accessory");
            MyUtility.Tool.SetupCombox(this.cbStockType, 2, 1, ",,B,Bulk,I,Inventory");
            this.EditMode = true;
            this.mainDetail = mainDetail;
            this.M = mdivisionid;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorCheckBoxColumnSettings settingSelect = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorNumericColumnSettings settingTransferQty = new DataGridViewGeneratorNumericColumnSettings();

            settingSelect.CellValidating += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                this.gridStock.GetDataRow(e.RowIndex)["Selected"] = e.FormattedValue;
                this.UpdateExportQty();
            };

            settingTransferQty.CellValidating += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                decimal curTransferQty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                DataRow curStockRow = this.gridStock.GetDataRow(e.RowIndex);
                if (curTransferQty > MyUtility.Convert.GetDecimal(curStockRow["StockBalance"]))
                {
                    MyUtility.Msg.WarningBox("Transfer Qty can not more than Stock Balance");
                    e.Cancel = true;
                    return;
                }

                curStockRow["TransferQty"] = e.FormattedValue;
                this.UpdateExportQty();
            };

            this.Helper.Controls.Grid.Generator(this.gridExport)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("PoStockQty", header: "Po Qty" + Environment.NewLine + "(Stock Unit)", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ExportQty", header: "Export Qty", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridStock)
                .CheckBox("Selected", trueValue: 1, falseValue: 0, iseditable: true, settings: settingSelect)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("StockTypeDesc", header: "Stock Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("StockBalance", header: "Stock Balance", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("TransferQty", header: "Transfer Qty", decimal_places: 2, width: Widths.AnsiChars(5), settings: settingTransferQty)
                .Text("Tone", header: "Tone", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Location", header: "Location", width: Widths.AnsiChars(15), iseditingreadonly: true);
        }

        private void Query()
        {
            if (MyUtility.Check.Empty(this.txtTransferExportID.Text))
            {
                MyUtility.Msg.WarningBox("Please input Transfer WK# first");
                return;
            }

            List<SqlParameter> listPar = new List<SqlParameter>();
            string where = string.Empty;

            listPar.Add(new SqlParameter("@ID", this.txtTransferExportID.Text));
            listPar.Add(new SqlParameter("@mainTransferOutID", this.mainTransferOutID));

            if (!MyUtility.Check.Empty(this.comboFabricType.Text))
            {
                where += " and ted.FabricType = @FabricType";
                listPar.Add(new SqlParameter("@FabricType", this.comboFabricType.SelectedValue));
            }

            string sqlGet = $@"
select  ted.InventoryPOID,
        [FromSEQ] = Concat (ted.InventorySeq1, ' ', ted.InventorySeq2),
        [StockUnit] = isnull(psdInv.StockUnit, ''),
        ted.PoID,
		[ToSEQ] = Concat (ted.Seq1, ' ', ted.Seq2),
        [PoStockQty] = Round(isnull(dbo.GetUnitQty(ted.UnitID , psdInv.StockUnit, ted.PoQty), 0), 2),
        [ExportQty] = 0.0,
        ted.InventorySeq1,
        ted.InventorySeq2,
        ted.Seq1,
        ted.Seq2,
        te.ID,
        ted.Ukey,
        [Description] = dbo.getMtlDesc(ted.InventoryPOID, ted.InventorySeq1, ted.InventorySeq2, 2, 0),
		MDivisionID = '{this.M}'
into #tmpTransferExport
from    TransferExport te with (nolock)
inner   join TransferExport_Detail ted with (nolock) on te.ID = ted.ID
left join PO_Supp_Detail psdInv with (nolock) on	ted.InventoryPOID = psdInv.ID and 
													ted.InventorySeq1 = psdInv.SEQ1 and
													ted.InventorySeq2 = psdinv.SEQ2
where   te.ID = @ID and
        te.FtyStatus = '{TK_FtyStatus.New}' and
        te.Sent = 1 and
        exists(select 1 from Factory f with (nolock) 
                                where f.ID = te.FromFactoryID and 
                                      f.IsproduceFty = 1 and 
                                      f.MDivisionID  = '{Env.User.Keyword}' ) and
        not exists( select 1 from TransferOut_Detail td with (nolock)
                                    where   td.TransferExport_DetailUkey = ted.Ukey and
                                            td.ID <> @mainTransferOutID
                  ) {where}

select * from #tmpTransferExport

select  [Selected] = 0,
        [Roll] = fi.Roll,
        [Dyelot] = fi.Dyelot,
        [StockTypeDesc] = iif(fi.StockType = 'I', 'Inventory', 'Bulk'),
        fi.StockType,
        [StockBalance] = fi.InQty - fi.OutQty + fi.AdjustQty,
        [TransferQty] = 0.0,
        [Location] = dbo.Getlocation(fi.ukey),
        [TransferExportDetailUkey] = te.Ukey,
        [FtyInventoryUkey] = fi.Ukey,
        te.InventoryPOID,
        te.InventorySeq1,
        te.InventorySeq2,
        te.PoID,
        te.Seq1,
        te.Seq2,
        te.FromSEQ,
        te.ToSEQ,
        te.StockUnit,
        te.Description,
        [TransferExportID] = te.ID,
        [TransferExport_DetailUkey] = te.Ukey,
        fi.Tone,
        MDivisionID
from  #tmpTransferExport te with (nolock)
inner join FtyInventory fi on te.InventoryPOID = fi.POID and
                              te.InventorySeq1 = fi.Seq1 and
                              te.InventorySeq2 = fi.Seq2 and
                              (fi.InQty - fi.OutQty + fi.AdjustQty - ReturnQty) > 0
union all
select  [Selected] = 0,
        [Roll] = '',
        [Dyelot] = '',
        [StockTypeDesc] = '',
        [StockType] = '',
        [StockBalance] = 0,
        [TransferQty] = 0.0,
        [Location] = '',
        [TransferExportDetailUkey] = te.Ukey,
        [FtyInventoryUkey] = -1,
        te.InventoryPOID,
        te.InventorySeq1,
        te.InventorySeq2,
        te.PoID,
        te.Seq1,
        te.Seq2,
        te.FromSEQ,
        te.ToSEQ,
        te.StockUnit,
        te.Description,
        [TransferExportID] = te.ID,
        [TransferExport_DetailUkey] = te.Ukey,
        Tone = '',
        MDivisionID
from    #tmpTransferExport te with (nolock)

drop table #tmpTransferExport
";
            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlGet, listPar, out dtResults);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridExport.DataSource = dtResults[0];
            this.bindingGridStock.DataSource = dtResults[1];
            if (dtResults[0].Rows.Count > 0)
            {
                this.RefreshRightGrid();
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void GridExport_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridExport.SelectedRows.Count == 0)
            {
                return;
            }

            this.RefreshRightGrid();
        }

        private void RefreshRightGrid()
        {
            long transferExportDetailUkey = MyUtility.Convert.GetLong(this.gridExport.GetDataRow(this.gridExport.GetSelectedRowIndex())["Ukey"]);

            var strWhere = string.Empty;

            if (this.cbStockType.Text != string.Empty)
            {
                strWhere = $"and StockTypeDesc = '{this.cbStockType.Text}'";
            }

            this.bindingGridStock.Filter = $"TransferExportDetailUkey = {transferExportDetailUkey} " + strWhere;
        }

        private void UpdateExportQty()
        {
            if (this.gridExport.SelectedRows.Count == 0)
            {
                return;
            }

            DataRow curExportRow = this.gridExport.GetDataRow(this.gridExport.GetSelectedRowIndex());
            long transferExportDetailUkey = MyUtility.Convert.GetLong(curExportRow["Ukey"]);

            DataTable dtStock = (DataTable)this.bindingGridStock.DataSource;

            var checkedStock = dtStock.AsEnumerable()
                .Where(s => MyUtility.Convert.GetLong(s["TransferExportDetailUkey"]) == transferExportDetailUkey && (int)s["Selected"] == 1);

            if (checkedStock.Any())
            {
                curExportRow["ExportQty"] = checkedStock.Sum(s => (decimal)s["TransferQty"]);
            }
            else
            {
                curExportRow["ExportQty"] = 0;
            }

            DataTable dtexp = (DataTable)this.gridExport.DataSource;
            this.displayTotal.Text = MyUtility.Convert.GetString(dtexp.AsEnumerable().Sum(s => (decimal)s["ExportQty"]));
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridStock.ValidateControl();
            if (this.bindingGridStock.DataSource == null)
            {
                return;
            }

            DataTable dtGridBS = (DataTable)this.bindingGridStock.DataSource;
            DataRow[] importRows = dtGridBS.Select("selected = 1");
            if (importRows.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
            }

            // 這支不用檢查Qty=0是因為這是台北端下的指令, 但工廠端有可能沒有
            foreach (DataRow dr in importRows)
            {
                // 以 GridUniqueKey 來確認不能有重複
                var existsRows = this.mainDetail.AsEnumerable()
                    .Where(w => w.RowState != DataRowState.Deleted
                        && w["mdivisionid"].EqualString(dr["mdivisionid"].ToString())
                        && w["poid"].EqualString(dr["poid"].ToString())
                        && w["seq1"].EqualString(dr["seq1"])
                        && w["seq2"].EqualString(dr["seq2"].ToString())
                        && w["roll"].EqualString(dr["roll"])
                        && w["dyelot"].EqualString(dr["dyelot"])
                        && w["stockType"].EqualString(dr["stockType"])
                        && w["ToPOID"].EqualString(dr["ToPOID"].ToString())
                        && w["Toseq1"].EqualString(dr["Toseq1"])
                        && w["Toseq2"].EqualString(dr["Toseq2"].ToString())
                        && (long)w["TransferExport_DetailUkey"] == (long)dr["TransferExport_DetailUkey"]);

                if (existsRows.Any())
                {
                    existsRows.First()["Qty"] = dr["TransferQty"];
                }
                else
                {
                    dr["ID"] = this.mainTransferOutID;
                    this.mainDetail.ImportRowAdded(dr);
                }
            }

            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridStock_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.gridStock.ValidateControl();
            this.UpdateExportQty();
        }

        private void CbStockType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.gridExport.SelectedRows.Count == 0)
            {
                return;
            }

            this.RefreshRightGrid();
        }
    }
}
