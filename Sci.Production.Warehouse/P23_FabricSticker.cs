﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P23_FabricSticker : Win.Subs.Base
    {
        private readonly object strSubTransferID;

        /// <inheritdoc/>
        public P23_FabricSticker(object strSubTransferID)
        {
            this.InitializeComponent();
            this.strSubTransferID = strSubTransferID;
            this.grid1.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Set Grid Columns
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                .Text("InventorySPNo", header: "Inventory" + Environment.NewLine + "SP#", iseditingreadonly: true)
                .Text("InventorySeq", header: "Inventory" + Environment.NewLine + "Seq#", iseditingreadonly: true)
                .Text("BulkSPNo", header: "Bulk" + Environment.NewLine + "SP#", iseditingreadonly: true)
                .Text("BulkSeq", header: "Bulk" + Environment.NewLine + "Seq", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Text("FromLocation", header: "From Location", iseditingreadonly: true, width: Widths.AnsiChars(18));

            for (int i = 0; i < this.grid1.Columns.Count; i++)
            {
                this.grid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion

            #region Set Grid Datas
            List<SqlParameter> listSqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", this.strSubTransferID),
            };

            string strQuerySql = @"
select
    Sel = 0
    , RowNo = Row_Number() over (order by std.FromPOID, std.ToSeq1, std.ToSeq2)
    , InventorySPNo = std.FromPOID
    , InventorySeq = Concat (std.FromSeq1, '-', std.FromSeq2)
    , BulkSPNo = std.ToPOID
    , BulkSeq = Concat (std.ToSeq1, '-', std.ToSeq2)
    , Roll = std.FromRoll
    , Dyelot = std.FromDyelot
    , ToFactory = std.ToFactoryID
    , Refno = isnull (psd.Refno, '')
    , Color =  IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
	            , IIF(isnull(SuppColor,'')='',isnull(dbo.GetColorMultipleID (o.BrandID, psd.ColorID), ''),SuppColor)
	            , isnull(dbo.GetColorMultipleID (o.BrandID, psd.ColorID), ''))
    , StockUnit = isnull (dbo.GetStockUnitBySPSeq (std.FromPOID, std.FromSeq1, std.FromSeq2), '')
    , Qty = std.Qty
    , [FromLocation]= dbo.Getlocation (fi.ukey)
    , fi.ContainerCode
from SubTransfer_Detail std WITH (NOLOCK)
left join View_WH_Orders o WITH (NOLOCK) on std.FromPOID = o.ID
left join Po_Supp_Detail psd WITH (NOLOCK) on std.FromPOID = psd.ID
								and std.FromSeq1 = psd.SEQ1
								and std.FromSeq2 = psd.SEQ2
left join FtyInventory FI on std.FromPoid = fi.poid 
                             and std.fromSeq1 = fi.seq1 
                             and std.fromSeq2 = fi.seq2
                             and std.fromRoll = fi.roll 
                             and std.fromStocktype = fi.stocktype
                             and std.fromDyelot = fi.Dyelot
left join Fabric on Fabric.SCIRefno = psd.SCIRefno
where std.ID = @ID
order by RowNo";

            DualResult result = DBProxy.Current.Select(string.Empty, strQuerySql, listSqlParameters, out DataTable dtResult);
            this.listControlBindingSource.DataSource = dtResult;

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            #endregion
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DualResult result;
            DataTable dtPrint = (DataTable)this.listControlBindingSource.DataSource;
            if (dtPrint != null
                && dtPrint.AsEnumerable().Any(row => Convert.ToBoolean(row["Sel"])))
            {
                #region Print
                dtPrint = dtPrint.AsEnumerable().Where(row => Convert.ToBoolean(row["Sel"])).CopyToDataTable();

                string strDtSortSQL = @"
select NewRowNo = Row_Number() over (order by RowNo)
       , *
from #tmp
order by NewRowNo";

                result = MyUtility.Tool.ProcessWithDatatable(dtPrint, string.Empty, strDtSortSQL, out dtPrint);

                if (result == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                List<P23_FabricSticker_PrintData> listData = dtPrint.AsEnumerable().Select(row => new P23_FabricSticker_PrintData()
                {
                    RowNo = Convert.ToInt32(row["NewRowNo"]),
                    InventorySPNo = row["InventorySPNo"].ToString().Trim(),
                    InventorySeq = row["InventorySeq"].ToString().Trim(),
                    BulkSPNo = row["BulkSPNo"].ToString().Trim(),
                    BulkSeq = row["BulkSeq"].ToString().Trim(),
                    Roll = row["Roll"].ToString().Trim(),
                    Dyelot = row["Dyelot"].ToString().Trim(),
                    ToFactory = row["ToFactory"].ToString().Trim(),
                    RefNo = row["RefNo"].ToString().Trim(),
                    Color = row["Color"].ToString().Trim(),
                    StockUnit = row["StockUnit"].ToString().Trim(),
                    Qty = Convert.ToDouble(row["Qty"]),
                    FromLocation = row["FromLocation"].ToString().Trim() + Environment.NewLine + row["ContainerCode"].ToString().Trim(),
                }).ToList();

                ReportDefinition report = new ReportDefinition
                {
                    ReportDataSource = listData,
                };

                // 指定是哪個 RDLC
                Type reportResourceNamespace = typeof(P23_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P23_FabricSticker_Print.rdlc";

                if ((result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)) == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
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
            else
            {
                MyUtility.Msg.InfoBox("Select data first.");
            }
        }
    }
}
