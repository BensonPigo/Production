using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10_RelaxationSticker : Win.Subs.Base
    {
        private string strIssueID;

        /// <inheritdoc/>
        public P10_RelaxationSticker(string issueID)
        {
            this.InitializeComponent();
            this.strIssueID = issueID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region set Grid Columns
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                .Text("SPNO", header: "SP#", iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Refno", header: "Refno", iseditingreadonly: true)
                .Text("Color", header: "Color", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 2, iseditingreadonly: true);
            #endregion

            for (int i = 0; i < this.grid1.Columns.Count; i++)
            {
                this.grid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            string sqlcmd = $@"
SELECT
    Sel = 0
   ,RowNo = ROW_NUMBER() OVER (ORDER BY psd.Refno, isd.POID, isd.Roll)
   ,SPNo = isd.POID
   ,Seq = CONCAT(isd.Seq1, '-', isd.Seq2)
   ,Roll = isd.Roll
   ,Dyelot = isd.Dyelot
   ,Refno = ISNULL(psd.Refno, '')
   ,Color = ISNULL(psdsC.SpecValue, '')
   ,Qty = isd.Qty
   ,i.CutplanID
   ,EstCutdate = FORMAT(EstCutdate.EstCutdate, 'yyyy/MM/dd')
   ,Relaxtime = CONCAT(CAST(fr.Relaxtime AS FLOAT), ' hrs')
   ,UnrollStartDate = FORMAT(fu.UnrollStartTime, 'yyyy/MM/dd')
   ,UnrollStartTime = FORMAT(fu.UnrollStartTime, 'hh:mm:ss')
   ,RelaxationEndDate= FORMAT(fu.RelaxationEndTime, 'yyyy/MM/dd')
   ,RelaxationEndTime = FORMAT(fu.RelaxationEndTime, 'hh:mm:ss')
FROM Issue_Detail isd
INNER JOIN Issue i WITH (NOLOCK) ON i.ID = isd.ID
INNER JOIN Cutplan cp WITH (NOLOCK) ON cp.ID = Ｉ.CutplanID
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = isd.POID
LEFT JOIN Po_Supp_Detail psd WITH (NOLOCK) ON isd.POID = psd.ID AND isd.Seq1 = psd.SEQ1 AND isd.Seq2 = psd.SEQ2
LEFT JOIN PO_Supp_Detail_Spec psdsC WITH (NOLOCK) ON psdsC.ID = psd.id AND psdsC.seq1 = psd.seq1 AND psdsC.seq2 = psd.seq2 AND psdsC.SpecColumnID = 'Color'
LEFT JOIN CutPlan_IssueCutDate cpi WITH (NOLOCK) ON cpi.ID = cp.ID AND cpi.Refno = psd.Refno AND cpi.Colorid = psdsC.SpecValue
OUTER APPLY (SELECT EstCutdate = IIF(cpi.EstCutDate IS NOT NULL, cpi.EstCutDate, cp.EstCutdate)) EstCutdate
LEFT JOIN [SciMES_RefnoRelaxtime] rr WITH (NOLOCK) ON rr.Refno = psd.Refno
LEFT JOIN [SciMES_FabricRelaxation] fr WITH (NOLOCK) ON rr.FabricRelaxationID = fr.ID
INNER JOIN WHBarcodeTransaction wbt WITH (NOLOCK) ON isd.Id = wbt.TransactionID AND isd.ukey = wbt.TransactionUkey AND wbt.Action = 'Confirm'
LEFT JOIN Fabric_UnrollandRelax fu WITH (NOLOCK) ON fu.Barcode = wbt.To_NewBarcode
WHERE isd.ID = '{this.strIssueID}'
ORDER BY RowNo
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource.DataSource = dt;
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
select NewRowNo = Row_Number() over (order by RowNo), *
from #tmp
order by NewRowNo";
                result = MyUtility.Tool.ProcessWithDatatable(dtPrint, string.Empty, strDtSortSQL, out dtPrint);

                if (result == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                List<P10_RelaxationSticker_Data> listData = dtPrint.AsEnumerable().Select(row => new P10_RelaxationSticker_Data()
                {
                    RowNo = Convert.ToInt32(row["NewRowNo"]),
                    SPNo = row["SPNo"].ToString().Trim(),
                    Seq = row["Seq"].ToString().Trim(),
                    Refno = row["Refno"].ToString().Trim(),
                    Color = row["Color"].ToString().Trim(),
                    Roll = row["Roll"].ToString().Trim(),
                    Dyelot = row["Dyelot"].ToString().Trim(),
                    Qty = Convert.ToDouble(row["Qty"]),
                    CutplanID = MyUtility.Convert.GetString(row["CutplanID"]),
                    EstCutdate = MyUtility.Convert.GetString(row["EstCutdate"]),
                    Relaxtime = MyUtility.Convert.GetString(row["Relaxtime"]),
                    UnrollStartDate = MyUtility.Convert.GetString(row["UnrollStartDate"]),
                    UnrollStartTime = MyUtility.Convert.GetString(row["UnrollStartTime"]),
                    RelaxationEndDate = MyUtility.Convert.GetString(row["RelaxationEndDate"]),
                    RelaxationEndTime = MyUtility.Convert.GetString(row["RelaxationEndTime"]),
                }).ToList();

                ReportDefinition report = new ReportDefinition
                {
                    ReportDataSource = listData,
                };

                // 指定是哪個 RDLC
                Type reportResourceNamespace = typeof(P10_RelaxationSticker_Data);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P10_RelaxationSticker_Print.rdlc";

                IReportResource reportresource;

                if ((result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)) == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report);
                frm.MdiParent = this.MdiParent;
                frm.ShowDialog();

                // 關閉視窗
                if (frm.DialogResult == DialogResult.Cancel)
                {
                    this.Close();
                }
                #endregion
            }
            else
            {
                MyUtility.Msg.InfoBox("Select data first.");
            }
        }
    }
}
