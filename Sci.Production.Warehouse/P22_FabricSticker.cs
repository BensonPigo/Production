using Ict;
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
    public partial class P22_FabricSticker : Win.Subs.Base
    {
        private readonly object strSubTransferID;

        /// <inheritdoc/>
        public P22_FabricSticker(object strSubTransferID)
        {
            this.InitializeComponent();
            this.strSubTransferID = strSubTransferID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Set Grid Columns
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                .Text("SPNo", header: "SP#", iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Refno", header: "Refno", iseditingreadonly: true)
                .Text("Color", header: "Color", iseditingreadonly: true)
                .Text("ToRoll", header: "Roll", iseditingreadonly: true)
                .Text("ToDyelot", header: "Dyelot", iseditingreadonly: true)
                .Text("Qty", header: "Qty", iseditingreadonly: true)
                ;

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
            string sqlcmd = @"
select Sel = 0
	, RowNo = Row_Number() over (order by std.FromPOID, std.ToSeq1, std.ToSeq2)
	, SPNo = std.FromPOID
	, Seq = Concat (std.FromSeq1, '-', std.FromSeq2)
	, Refno = isnull (psd.Refno, '')
    , Color =  IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' ,SuppColor, isnull(dbo.GetColorMultipleID (o.BrandID, psd.ColorID), ''))
	, ToRoll = std.ToRoll
	, ToDyelot = std.ToDyelot
	, Qty = std.Qty
	, ToFactory = std.ToFactoryID
	, SPDetail = std.FromPOID+' '+std.FromSeq1+'-'+std.FromSeq2
	, StockUnit = psd.StockUnit
	, ToLocation = isnull(std.ToLocation,'')
from SubTransfer_Detail std
left join Orders o on std.FromPOID = o.ID
left join Po_Supp_Detail psd on std.FromPOID = psd.ID and std.FromSeq1 = psd.SEQ1 and std.FromSeq2 = psd.SEQ2
left join Fabric on Fabric.SCIRefno = psd.SCIRefno
where std.ID = @ID
order by RowNo";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, listSqlParameters, out DataTable dtResult);
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

                List<P22_FabricSticker_PrintData> listData = dtPrint.AsEnumerable().Select(row => new P22_FabricSticker_PrintData()
                {
                    RowNo = Convert.ToInt32(row["NewRowNo"]),
                    SPNo = row["SPNo"].ToString().Trim(),
                    Seq = row["Seq"].ToString().Trim(),
                    Refno = row["Refno"].ToString().Trim(),
                    Color = row["Color"].ToString().Trim(),
                    ToRoll = row["ToRoll"].ToString().Trim(),
                    ToDyelot = row["ToDyelot"].ToString().Trim(),
                    ToFactory = row["ToFactory"].ToString().Trim(),
                    SPDetail = row["SPDetail"].ToString().Trim(),
                    ToLocation = row["ToLocation"].ToString().Trim(),
                    StockUnit = row["StockUnit"].ToString().Trim(),
                    Qty = Convert.ToDouble(row["Qty"]),
                }).ToList();

                ReportDefinition report = new ReportDefinition
                {
                    ReportDataSource = listData,
                };

                // 指定是哪個 RDLC
                Type reportResourceNamespace = typeof(P22_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P22_FabricSticker_Print.rdlc";

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
