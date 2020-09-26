using Sci.Win.Tems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using System.Reflection;
using Ict.Win;

namespace Sci.Production.Warehouse
{
    public partial class P10_PrintBarcode : PrintForm
    {
        private DataTable printData;
        private string issueIdFrom = string.Empty;
        private string issueIdTo = string.Empty;
        private DualResult result;

        public P10_PrintBarcode(string inputIssueID)
        {
            this.InitializeComponent();
            this.txtIssueIdFrom.Text = inputIssueID;
            this.txtIssueIdTo.Text = inputIssueID;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.issueIdFrom = this.txtIssueIdFrom.Text;
            this.issueIdTo = this.txtIssueIdTo.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result;
            string printDataSql = $@"
select 
[Factory] = o.FtyGroup,
[SP] = isd.POID,
isd.Dyelot,
isd.Roll,
isu.Colorid,
[Yardage] = isd.Qty,
[BarcodeNo] = '*' + isd.BarcodeNo + '*'
from Issue_Detail isd with (nolock)
left join Issue_Summary isu with (nolock) on isu.Ukey = isd.Issue_SummaryUkey
left join orders o with (nolock) on o.ID = isd.POID
where isd.Id >= '{this.issueIdFrom}' and isd.Id <= '{this.issueIdTo}'";

            result = DBProxy.Current.Select(null, printDataSql, out this.printData);

            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToPrint(ReportDefinition report)
        {
            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            DataTable dtFirstColumn;
            DataTable dtSecondColumn;

            int count = 1;
            dtFirstColumn = this.printData.Clone();
            dtSecondColumn = this.printData.Clone();

            foreach (DataRow dr in this.printData.Rows)
            {
                // 第一列資料
                if (count % 2 == 1)
                {
                    dtFirstColumn.ImportRow(dr);
                }

                // 第二列資料
                if (count % 2 == 0)
                {
                    dtSecondColumn.ImportRow(dr);
                }

                count++;
            }

            // 傳 list 資料
            List<P10_PrintBarcodeData> finalData = dtFirstColumn.AsEnumerable()
                .Select(newRow => new P10_PrintBarcodeData
                {
                    Factory = newRow["Factory"].ToString(),
                    SP = newRow["SP"].ToString(),
                    Dyelot = newRow["Dyelot"].ToString(),
                    Roll = newRow["Roll"].ToString(),
                    Colorid = newRow["Colorid"].ToString(),
                    Yardage = newRow["Yardage"].ToString(),
                    BarcodeNo = newRow["BarcodeNo"].ToString(),
                }).ToList();

            finalData.AddRange(
                dtSecondColumn.AsEnumerable()
                .Select(newRow => new P10_PrintBarcodeData
                {
                    Factory2 = newRow["Factory"].ToString(),
                    SP2 = newRow["SP"].ToString(),
                    Dyelot2 = newRow["Dyelot"].ToString(),
                    Roll2 = newRow["Roll"].ToString(),
                    Colorid2 = newRow["Colorid"].ToString(),
                    Yardage2 = newRow["Yardage"].ToString(),
                    BarcodeNo2 = newRow["BarcodeNo"].ToString(),
                }).ToList());

            report.ReportDataSource = finalData;

            Type reportResourceNamespace = typeof(P10_PrintBarcodeData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P10_PrintBarcode.rdlc";

            IReportResource reportresource;
            if (!(this.result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                this.ShowException(this.result);
                return this.result;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;

            // frm.DirectPrint = true;
            frm.ShowDialog();

            // 關閉視窗
            if (frm.DialogResult == DialogResult.Cancel)
            {
                this.Close();
            }

            return true;
        }
    }
}
