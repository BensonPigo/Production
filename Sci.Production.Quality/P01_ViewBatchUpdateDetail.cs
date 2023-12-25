using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_ViewBatchUpdateDetail : Win.Tems.QueryForm
    {
        private string poid;

        /// <inheritdoc/>
        public P01_ViewBatchUpdateDetail(string poid)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.poid = poid;
            this.displaySP.Text = poid;
            this.displaySP2.Text = poid;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ExportID", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ReceivingID", header: "ReceivingID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(6), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Text("Scale", header: "Scale", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ShadebandDocLocationID", header: "ShadeBand Location", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Date("Inspdate", header: "Insp.Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Name", header: "Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(8), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.grid2)
                .Text("ExportID", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ReceivingID", header: "ReceivingID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(6), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Text("Scale", header: "Scale", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Date("Inspdate", header: "Insp.Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Name", header: "Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(8), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
SELECT fs.*
    ,Name = (SELECT Name FROM Pass1 WHERE ID = fs.Inspector)
    ,FIR.ReceivingID
    ,r.ExportId
    ,SEQ = CONCAT(FIR.SEQ1, '-', FIR.SEQ2)
FROM FIR_Shadebone fs WITH(NOLOCK)
INNER JOIN FIR WITH(NOLOCK) ON FIR.ID = fs.ID
LEFT JOIN View_AllReceiving r WITH(NOLOCK) ON r.ID = FIR.ReceivingID
WHERE FIR.POID = '{this.poid}'
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            sqlcmd = $@"
SELECT fs.*
    ,Name = (SELECT Name FROM Pass1 WHERE ID = fs.Inspector)
    ,FIR.ReceivingID
    ,r.ExportId
    ,SEQ = CONCAT(FIR.SEQ1, '-', FIR.SEQ2)
FROM FIR_Continuity fs WITH(NOLOCK)
INNER JOIN FIR WITH(NOLOCK) ON FIR.ID = fs.ID
LEFT JOIN View_AllReceiving r WITH(NOLOCK) ON r.ID = FIR.ReceivingID
WHERE FIR.POID = '{this.poid}'
";
            result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource2.DataSource = dt2;

            this.SetComboBox(dt, dt2);
        }

        private void SetComboBox(DataTable dt, DataTable dt2)
        {
            var seqList = dt.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["SEQ"])).Distinct().ToList();
            seqList.Insert(0, string.Empty);
            this.comboBoxSEQ.DataSource = seqList;
            this.comboBoxSEQ.SelectedIndex = 0;
            this.comboBoxSEQ.SelectedItem = string.Empty;

            var seqList2 = dt2.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["SEQ"])).Distinct().ToList();
            seqList2.Insert(0, string.Empty);
            this.comboBoxSEQ2.DataSource = seqList2;
            this.comboBoxSEQ2.SelectedIndex = 0;
            this.comboBoxSEQ2.SelectedItem = string.Empty;

            var exportIdList = dt.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["ExportId"])).Distinct().ToList();
            exportIdList.Insert(0, string.Empty);
            this.comboBoxWKNo.DataSource = exportIdList;
            this.comboBoxWKNo.SelectedIndex = 0;
            this.comboBoxWKNo.SelectedItem = string.Empty;

            var exportIdList2 = dt2.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["ExportId"])).Distinct().ToList();
            exportIdList2.Insert(0, string.Empty);
            this.comboBoxWKNo2.DataSource = exportIdList2;
            this.comboBoxWKNo2.SelectedIndex = 0;
            this.comboBoxWKNo2.SelectedItem = string.Empty;

            var receivingIDList = dt.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["ReceivingID"])).Distinct().ToList();
            receivingIDList.Insert(0, string.Empty);
            this.comboBoxReceivingID.DataSource = receivingIDList;
            this.comboBoxReceivingID.SelectedIndex = 0;
            this.comboBoxReceivingID.SelectedItem = string.Empty;

            var receivingIDList2 = dt2.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["ReceivingID"])).Distinct().ToList();
            receivingIDList2.Insert(0, string.Empty);
            this.comboBoxReceivingID2.DataSource = receivingIDList2;
            this.comboBoxReceivingID2.SelectedIndex = 0;
            this.comboBoxReceivingID2.SelectedItem = string.Empty;
        }

        private void ApplyFilters()
        {
            List<string> filters = new List<string>();
            string selectedSeq = this.comboBoxSEQ.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedSeq))
            {
                filters.Add($"SEQ = '{selectedSeq}'");
            }

            string selectedExportId = this.comboBoxWKNo.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedExportId))
            {
                filters.Add($"ExportId = '{selectedExportId}'");
            }

            string selectedReceivingID = this.comboBoxReceivingID.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedReceivingID))
            {
                filters.Add($"ReceivingID = '{selectedReceivingID}'");
            }

            if (this.chkPasteShadebandTime.Checked)
            {
                filters.Add("PasteTime IS NOT NULL");
            }

            string combinedFilter = string.Join(" AND ", filters);
            this.listControlBindingSource1.Filter = combinedFilter;
        }

        // 每個下拉框的 SelectedIndexChanged 事件中都調用 ApplyFilters
        private void ComboBoxSEQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        private void ComboBoxWKNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        private void ComboBoxReceivingID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        private void ChkPasteShadebandTime_CheckedChanged(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        private void ApplyFilters2()
        {
            List<string> filters = new List<string>();
            string selectedSeq = this.comboBoxSEQ2.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedSeq))
            {
                filters.Add($"SEQ = '{selectedSeq}'");
            }

            string selectedExportId = this.comboBoxWKNo2.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedExportId))
            {
                filters.Add($"ExportId = '{selectedExportId}'");
            }

            string selectedReceivingID = this.comboBoxReceivingID2.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedReceivingID))
            {
                filters.Add($"ReceivingID = '{selectedReceivingID}'");
            }

            if (this.chkCheckByQC.Checked)
            {
                filters.Add("Inspector <> '' AND InspDate IS NOT NULL");
            }

            string combinedFilter = string.Join(" AND ", filters);
            this.listControlBindingSource2.Filter = combinedFilter;
        }

        private void ComboBoxSEQ2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ApplyFilters2();
        }

        private void ComboBoxWKNo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ApplyFilters2();
        }

        private void ComboBoxReceivingID2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ApplyFilters2();
        }

        private void ChkCheckByQC_CheckedChanged(object sender, EventArgs e)
        {
            this.ApplyFilters2();
        }
    }
}
