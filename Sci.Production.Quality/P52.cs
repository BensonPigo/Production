using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Prg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P52 : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P12"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P52(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            MyUtility.Tool.SetupCombox(this.comboMaterialType, 2, 1, @"F,Fabric,A,Accessory");
        }

        private DataTable dt1;
        private DataSet materialSet = null;
        private MaterialtableDataSet materialSet2 = null;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_TestReportCheckClima;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_Inspection_Report_FtyReceivedDate;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_TestReport_FtyReceivedDate;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_ContinuityCard_FtyReceivedDate;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_FirstDyelot_FtyReceivedDate;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_T2InspYds;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_T2DefectPoint;

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetupGrid_Material();
            this.SetupGrid_Document();
            this.SetupGrid_Report(string.Empty);
            this.SetupGrid_TabPage2();
        }

        private void SetupGrid_TabPage2()
        {
            #region tabPage2
            #region settings Event

            DataGridViewGeneratorTextColumnSettings refno = new DataGridViewGeneratorTextColumnSettings();
            refno.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Convert.GetInt(dr["bitRefnoColor"]) == 1)
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }
            };
            DataGridViewGeneratorDateColumnSettings inspection = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorDateColumnSettings test = new DataGridViewGeneratorDateColumnSettings();
            inspection.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!MyUtility.Check.Empty(e.Value))
                {
                    e.CellStyle.Font = new Font("Ariel", 10, FontStyle.Underline);
                    e.CellStyle.ForeColor = Color.Blue;
                }
            };
            test.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!MyUtility.Check.Empty(e.Value))
                {
                    e.CellStyle.Font = new Font("Ariel", 10, FontStyle.Underline);
                    e.CellStyle.ForeColor = Color.Blue;
                }
            };

            // 帶出grade
            DataGridViewGeneratorNumericColumnSettings t2IY = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings t2DP = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings col_CheckClima = new DataGridViewGeneratorCheckBoxColumnSettings();

            t2IY.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                dr["T2Inspected_Yards"] = e.FormattedValue;
                dr.EndEdit();
                this.T2Validating(s, e);
            };
            t2DP.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                dr["T2Defect_Points"] = e.FormattedValue;
                dr.EndEdit();
                this.T2Validating(s, e);
            };

            col_CheckClima.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["Clima"]))
                {
                    e.IsEditable = false;
                }
            };
            #endregion settings Event
            #region Set_grid1 Columns
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("selected", header: string.Empty, trueValue: 1, falseValue: 0, iseditable: true)
            .Text("ExportID", header: "WK#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("InvoiceNo", header: "Invoice#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("ReceivingID", header: "Receiving ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("WhseArrival", header: "ATA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("ETA", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("PoID", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("Seq", header: "Seq#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SuppName", header: "Supp Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(14), iseditingreadonly: true, settings: refno)
            .Text("WeaveTypeID", header: "Weave Type", width: Widths.AnsiChars(8), iseditingreadonly: true)

            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Date("Inspection_Report_FtyReceivedDate", header: "Inspection Report\r\nFty Received Date", width: Widths.AnsiChars(10), iseditingreadonly: false).Get(out this.col_Inspection_Report_FtyReceivedDate) // W (Pink)
            .Date("Inspection_Report_TPESentDate", header: "Inspection Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: inspection)
            .Date("TestReport_FtyReceivedDate", header: "Test Report\r\nFty Received Date", width: Widths.AnsiChars(10), iseditingreadonly: false).Get(out this.col_TestReport_FtyReceivedDate) // W (Pink)
            .CheckBox("TestReportCheckClima", header: "Test Report\r\n Check Clima", trueValue: 1, falseValue: 0, iseditable: true, settings: col_CheckClima).Get(out this.col_TestReportCheckClima)
            .Date("TestReport_TPESentDate", header: "Test Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: test)
            .Date("ContinuityCard_FtyReceivedDate", header: "Continuity Card\r\nFty Received Date", width: Widths.AnsiChars(10), iseditingreadonly: false).Get(out this.col_ContinuityCard_FtyReceivedDate) // W (Pink)
            .Date("ContinuityCard_TPESentDate", header: "Continuity Card\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ContinuityCard_AWB", header: "Continuity Card\r\nAWB#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Date("1stBulkDyelot_FtyReceivedDate", header: "1st Bulk Dyelot\r\nFty Received Date", width: Widths.AnsiChars(10), iseditingreadonly: false).Get(out this.col_FirstDyelot_FtyReceivedDate) // W
            .Text("1stBulkDyelot_TPESentDate", header: "1st Bulk Dyelot\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("T2Inspected_Yards", header: "T2 Inspected Yards", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: t2IY, iseditingreadonly: false).Get(out this.col_T2InspYds) // W
            .Numeric("T2Defect_Points", header: "T2 Defect Points", width: Widths.AnsiChars(8), integer_places: 5, settings: t2DP, iseditingreadonly: false).Get(out this.col_T2DefectPoint) // W
            .Text("Grade", header: "Grade", width: Widths.AnsiChars(8), iseditingreadonly: true) // W
            .Text("T1Inspected_Yards", header: "T1 Inspected Yards", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("T1Defect_Points", header: "T1 Defect Points", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;
            #endregion Set_grid2 Columns
            #region Color
            this.grid1.Columns["Inspection_Report_FtyReceivedDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["TestReport_FtyReceivedDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["ContinuityCard_FtyReceivedDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["1stBulkDyelot_FtyReceivedDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["T2Inspected_Yards"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["T2Defect_Points"].DefaultCellStyle.BackColor = Color.Pink;
            this.Change_Color_Edit();
            #endregion Color
            #endregion tabPage2_grid2
        }

        private void Change_Color_Edit()
        {
            this.col_TestReportCheckClima.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["Clima"]) || dr["NewSentReport_Exists"].ToString() == "N")
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            this.col_TestReportCheckClima.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["Clima"]) || dr["NewSentReport_Exists"].ToString() == "N")
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.col_Inspection_Report_FtyReceivedDate.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["Inspection_Report_TPESentDate"]))
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            this.col_Inspection_Report_FtyReceivedDate.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["Inspection_Report_TPESentDate"]))
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.col_Inspection_Report_FtyReceivedDate.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow r = grid.GetDataRow<DataRow>(e.RowIndex);
                var newValue = e.FormattedValue;
                DateTime? reportDate = MyUtility.Convert.GetDate(newValue);
                if (this.CheckDate(reportDate))
                {
                    r["Inspection_Report_FtyReceivedDate"] = MyUtility.Convert.GetDate(newValue).ToYYYYMMDD();
                    r.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };

            this.col_TestReport_FtyReceivedDate.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["TestReport_TPESentDate"]))
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            this.col_TestReport_FtyReceivedDate.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["TestReport_TPESentDate"]))
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.col_TestReport_FtyReceivedDate.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow r = grid.GetDataRow<DataRow>(e.RowIndex);
                var newValue = e.FormattedValue;
                DateTime? reportDate = MyUtility.Convert.GetDate(newValue);
                if (this.CheckDate(reportDate))
                {
                    r["TestReport_FtyReceivedDate"] = MyUtility.Convert.GetDate(newValue).ToYYYYMMDD();
                    r.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };

            this.col_ContinuityCard_FtyReceivedDate.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["ContinuityCard_TPESentDate"]))
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            this.col_ContinuityCard_FtyReceivedDate.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["ContinuityCard_TPESentDate"]))
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.col_ContinuityCard_FtyReceivedDate.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow r = grid.GetDataRow<DataRow>(e.RowIndex);
                var newValue = e.FormattedValue;
                DateTime? reportDate = MyUtility.Convert.GetDate(newValue);
                if (this.CheckDate(reportDate))
                {
                    r["ContinuityCard_FtyReceivedDate"] = MyUtility.Convert.GetDate(newValue).ToYYYYMMDD();
                    r.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };

            this.col_FirstDyelot_FtyReceivedDate.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["1stBulkDyelot_TPESentDate"]))
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            this.col_FirstDyelot_FtyReceivedDate.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["1stBulkDyelot_TPESentDate"]))
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.col_FirstDyelot_FtyReceivedDate.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow r = grid.GetDataRow<DataRow>(e.RowIndex);
                var newValue = e.FormattedValue;
                DateTime? reportDate = MyUtility.Convert.GetDate(newValue);
                if (this.CheckDate(reportDate))
                {
                    r["1stBulkDyelot_FtyReceivedDate"] = MyUtility.Convert.GetDate(newValue).ToYYYYMMDD();
                    r.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };

            this.col_T2InspYds.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (dr["NewSentReport_Exists"].ToString() == "N")
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            this.col_T2InspYds.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (dr["NewSentReport_Exists"].ToString() == "N")
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.col_T2DefectPoint.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (dr["NewSentReport_Exists"].ToString() == "N")
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            this.col_T2DefectPoint.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (dr["NewSentReport_Exists"].ToString() == "N")
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };
        }

        private void SetupGrid_Material()
        {
            this.Helper.Controls.Grid.Generator(this.grid_Material)
               .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
               .Text("Season", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("SuppGroup", header: "Supp Group", width: Widths.AnsiChars(17), iseditingreadonly: true)
               .Text("supplier", header: "Supplier", width: Widths.AnsiChars(17), iseditingreadonly: true)
               .Text("Refno", header: "Refno", width: Widths.AnsiChars(17), iseditingreadonly: true)
               .Text("BrandRefno", header: "BrandRefno", width: Widths.AnsiChars(17), iseditingreadonly: true)
               .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(11), iseditingreadonly: true, integer_places: 8, decimal_places: 2)
               ;

            this.SetGrid_HeaderBorderStyle(this.grid_Material, false);
        }

        private void SetupGrid_Document()
        {
            this.Helper.Controls.Grid.Generator(this.grid_Document)
                .Text("DocumentName", header: "Document Name", width: Widths.AnsiChars(24), iseditingreadonly: true)
                ;

            this.SetGrid_HeaderBorderStyle(this.grid_Document, false);
        }

        private void SetupGrid_Report(string fileRule)
        {
            this.grid_Report.Columns.Clear();
            switch (fileRule)
            {
                case "1":
                case "2":
                    this.grid_Report.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.grid_Report)
                .Button("...", header: "File", width: Widths.AnsiChars(8), onclick: this.ClickClip)
                .Date("TestReport", header: "Upload date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("TestReportTestDate", header: "Test Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("TestSeasonID", header: "Test Season", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("DueDate", header: "Due Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("FTYReceivedReport", header: "FTY Received Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                ;
                    break;
                case "3":
                    this.grid_Report.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.grid_Report)
                    .Text("DocSeason", header: "Doc Season", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Numeric("Period", header: "Period", width: Widths.AnsiChars(15), integer_places: 3, decimal_places: 0, iseditingreadonly: true)
                    .Text("LOT", header: "LOT#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Text("ReceivedRemark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Text("TestDocFactoryGroup", header: "FTY ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("FirstDyelot", header: "Sent Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("AWBno", header: "AWBno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("FTYReceivedReport", header: "FTY Received Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                ;
                    break;
                case "4":
                case "5":
                    this.grid_Report.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.grid_Report)
                    .Button("...", header: "File", width: Widths.AnsiChars(8), onclick: this.ClickClip)
                    .Date("ReportDate", header: "Upload date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("AWBno", header: "AWBno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("ExportID", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("FTYReceivedReport", header: "FTY Received Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    ;
                    break;
            }

            this.SetGrid_HeaderBorderStyle(this.grid_Report, false);
        }

        private void SetGrid_HeaderBorderStyle(Sci.Win.UI.Grid grid, bool withAlternateRowStyle = true)
        {
            grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;

            grid.RowHeadersVisible = true;
            grid.RowHeadersWidth = 22;
            grid.RowTemplate.Height = 19; // 小於19 的話會導致 checkBox column 不見

            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = System.Drawing.Color.DimGray;
            headerStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.WhiteSmoke;
            headerStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            headerStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            headerStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        private void ClickClip(object sender, EventArgs e)
        {
            var row = this.grid_Report.GetCurrentDataRow();
            if (row == null)
            {
                return;
            }

            var id = row["UniqueKey"].ToString();
            if (id.IsNullOrWhiteSpace())
            {
                return;
            }

            string tableName = string.Empty;

            switch (row["FileRule"].ToString())
            {
                case "1":
                case "2":
                    tableName = "UASentReport";
                    break;
                case "4":
                    tableName = "NewSentReport";
                    break;
                case "5":
                    tableName = "ExportRefnoSentReport";
                    break;
            }

            string sqlcmd = $@"select 
            [FileName] = TableName + PKey,
            SourceFile,
            AddDate
            from GASAClip
            where TableName = '{tableName}' and 
            UniqueKey = '{id}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
            }

            List<string> list = new List<string>();
            string filePath = MyUtility.GetValue.Lookup($"select [path] from CustomizedClipPath where TableName = '{tableName}'");

            // 組ClipPath
            string clippath = MyUtility.GetValue.Lookup($"select ClipPath from System");
            foreach (DataRow dataRow in dt.Rows)
            {
                string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                string saveFilePath = Path.Combine(clippath, yyyyMM);
                string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                lock (FileDownload_UpData.DownloadFileAsync($"{PmsWebAPI.PMSAPApiUri}/api/FileDownload/GetFile", filePath + "\\" + yyyyMM, fileName, saveFilePath))
                {
                }
            }

            using (var dlg = new PublicForm.ClipGASA(tableName, id, false, row, apiUrlFile: $"{PmsWebAPI.PMSAPApiUri}/api/FileDelete/RemoveFile"))
            {
                dlg.ShowDialog();

                foreach (DataRow dataRow in dt.Rows)
                {
                    string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                    string saveFilePath = Path.Combine(clippath, yyyyMM);
                    string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                    string deleteFile = Path.Combine(saveFilePath, fileName);
                    if (File.Exists(deleteFile))
                    {
                        File.Delete(deleteFile);
                    }
                }
            }
        }

        private MaterialtableDataSet LoadMaterialtableData()
        {
            #region 必輸入條件
            if (MyUtility.Check.Empty(this.txtSP.Text) &&
                MyUtility.Check.Empty(this.txtSeq1.Text) &&
                MyUtility.Check.Empty(this.txtseq2.Text) &&
                MyUtility.Check.Empty(this.txtSeason.Text) &&
                MyUtility.Check.Empty(this.txtBrand.Text) &&
                MyUtility.Check.Empty(this.txtMultiSupplier1.Text) &&
                MyUtility.Check.Empty(this.txtRefno.Text) &&
                MyUtility.Check.Empty(this.txtColor.Text) &&
                MyUtility.Check.Empty(this.txtfactory.Text) &&
                MyUtility.Check.Empty(this.dateETA.Value1) &&
                MyUtility.Check.Empty(this.dateATA1.Value1) &&
                MyUtility.Check.Empty(this.txtBrandRefno.Text))
            {
                MyUtility.Msg.WarningBox("Please input any one filter before query!");
                this.txtSP.Select();
                return null;
            }
            #endregion

            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                where += $@" and o.id = '{this.txtSP.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSeq1.Text))
            {
                where += $@" and p3.Seq1 = '{this.txtSeq1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtseq2.Text))
            {
                where += $@" and p3.Seq2 = '{this.txtseq2.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSeason.Text))
            {
                where += $@" and o.SeasonID = '{this.txtSeason.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $@" and o.BrandID = '{this.txtBrand.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtMultiSupplier1.Text))
            {
                where += $@" and s2.ID in (select data from splitstring('{this.txtMultiSupplier1.Text}', ','))";
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                where += $@" and p3.Refno = '{this.txtRefno.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                where += $@" and psds.SpecValue = '{this.txtColor.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                where += $@" and o.FactoryID = '{this.txtfactory.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtBrandRefno.Text))
            {
                where += $@" and f.BrandRefno = '{this.txtBrandRefno.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.dateETA.Value1) || !MyUtility.Check.Empty(this.dateETA.Value2))
            {
                where += $@" 
--Export ETA
and exists (
select 1 from Export_Detail ed
inner join Export on Export.id = ed.ID and Export.Confirm = 1
where ed.PoID = p3.id and ed.Seq1 = p3.Seq1 and ed.Seq2 = p3.Seq2 
and Export.Eta between '{((DateTime)this.dateETA.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateETA.Value2).ToString("yyyy/MM/dd")}') 
";
            }

            if (!MyUtility.Check.Empty(this.dateATA1.Value1) || !MyUtility.Check.Empty(this.dateATA1.Value2))
            {
                where += $@" 
--Export ATA
and exists (
select 1 from Export_Detail ed
inner join Export on Export.id = ed.ID and Export.Confirm = 1
where ed.PoID = p3.id and ed.Seq1 = p3.Seq1 and ed.Seq2 = p3.Seq2 
and Export.WhseArrival between '{((DateTime)this.dateATA1.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateATA1.Value2).ToString("yyyy/MM/dd")}') 
";
            }

            if (!MyUtility.Check.Empty(this.comboMaterialType.SelectedValue.ToString()))
            {
                where += $@"
and  f.Type = '{this.comboMaterialType.SelectedValue.ToString()}'
";
            }

            #endregion

            string sqlcmd = $@"
use Production
IF OBJECT_ID('tempdb..#POList') IS NOT NULL
    DROP TABLE #POList

Select distinct
o.FactoryID,
       MtltypeId,
	   POID = p3.ID,
       Seq = p3.Seq1 + '-' + p3.Seq2,
	   Season = o.SeasonID,	   
       p3.Seq1,
       p3.Seq2,
       SuppGroup = Concat(s2.ID, '-', s2.AbbEN),
       p2.SuppID,
       supplier = IIF(Isnull(su.AbbEN, '') = '', su.ID, Concat(su.ID, '-', su.AbbEN)),
       FinalETD=IsNull(p3.CfmETD, p3.SystemETD),
       p3.Refno,
       Color = psds.SpecValue,
       p3.Qty,
       p3.ShipQty,
       p3.ShipETA,
       o.ProgramID,
       f.Type,
       Season.Month,
       o.BrandID,
       Category = ddl.Name,
       f.BrandRefno
into #POList
From Orders o with(nolock)
inner Join dbo.PO_Supp p2 with(nolock) on p2.ID = o.ID
inner Join dbo.PO_Supp_Detail p3 with(nolock) on p3.ID = p2.ID and p3.Seq1 = p2.SEQ1 
 	and IsNull(P3.Junk, 0) = 0 --作廢不顯示
	and (IsNull(p3.Qty, 0) > 0 or IsNull(p3.foc, 0) > 0) --數量為0不顯示
left join PO_Supp_Detail_spec  psds on psds.id = p3.ID and psds.Seq1 = p3.SEQ1 and psds.Seq2 = p3.SEQ2 and psds.SpecColumnID = 'Color'
inner Join dbo.Supp su with(nolock) on su.ID = p2.SuppID
Inner Join BrandRelation as bs WITH (NOLOCK) ON bs.BrandID = o.BrandID and bs.SuppID = su.ID
Inner Join Supp s2 WITH (NOLOCK) on bs.SuppGroup = s2.ID
inner join dbo.Fabric f with(nolock) on p3.SciRefno = f.SciRefno
inner JOIN Season WITH (NOLOCK) on o.SeasonID = Season.ID and o.BrandID = Season.BrandID
LEFT JOIN DropDownList ddl WITH (NOLOCK) on ddl.type ='Category' and o.Category = ddl.ID
where 1=1
and f.BrandRefNo <> ''
{where}


select * from #POList
Order by POID,Seq

Select distinct md.DocumentName, md.FileRule, po.Seq, po.POID, md.BrandID
FROM MaterialDocument md
inner join #POList po on md.BrandID = po.BrandID or 
exists (
	select 1 from MaterialDocument_Brand mdb
	where md.DocumentName = mdb.DocumentName
	and mdb.MergedBrand = po.BrandID
	and md.BrandID = mdb.BrandID
)
Outer apply ( 
	SELECT value = STUFF((SELECT CONCAT(',', MtlType.MtltypeId) 
	FROM MaterialDocument_MtlType MtlType WITH (NOLOCK)
	WHERE MtlType.DocumentName = md.DocumentName and MtlType.BrandID = md.BrandID 
	FOR XML PATH (''))
	, 1, 1, '')
) MtlType
Outer apply ( 
	SELECT value = STUFF((SELECT CONCAT(',', supp.SuppID) 
	FROM MaterialDocument_Supplier supp WITH (NOLOCK)
	WHERE supp.DocumentName = md.DocumentName AND supp.BrandID = md.BrandID 
	FOR XML PATH ('')), 1, 1, '')
) supp
WHERE md.FabricType = po.Type 
and po.Month >= (select month From Season where id =md.ActiveSeason and BrandID = md.BrandID)
and (isnull(md.EndSeason,'') = '' or po.Month <= (select month From Season where id =md.EndSeason and BrandID = md.BrandID))
and (isnull(md.ExcludeProgram,'') = '' or po.ProgramID not in (select data from splitstring(md.ExcludeProgram,',')))
and (md.ExcludeReplace = 0 or po.seq1 < '50' or po.seq1 > '69')
and (md.ExcludeStock = 0 or po.seq1 < '70')
and (isnull(md.MtlTypeClude,'') = '' or isnull(MtlType.value,'') = '' 
    or (md.MtlTypeClude = 'E' and po.MtltypeId not in (select data from splitstring(MtlType.value, ',')))
    or (md.MtlTypeClude = 'I' and po.MtltypeId in (select data from splitstring(MtlType.value, ',')))
    )
and (isnull(md.SupplierClude,'') = '' or isnull(supp.value,'') = ''
    or (md.SupplierClude = 'E' and po.SuppID not in (select data from splitstring(supp.value, ',')))
    or (md.SupplierClude = 'I' and po.SuppID in (select data from splitstring(supp.value, ',')))
    )
and (po.Category in (select data from splitstring(md.Category,',')))
and md.junk=0
IF OBJECT_ID('tempdb..#POList') IS NOT NULL
DROP TABLE #POList

";

            if (!PublicPrg.Prgs.SelectSet(string.Empty, sqlcmd, out DataSet ds))
            {
                return null;
            }

            var tmpSet = new MaterialtableDataSet();
            var dt1 = ds.Tables[0];
            var dt2 = ds.Tables[1];
            ds.Tables.Remove(dt1);
            ds.Tables.Remove(dt2);
            dt1.TableName = "PO";
            dt2.TableName = "Document";
            tmpSet.Tables.Add(dt1);
            tmpSet.Tables.Add(dt2);

            tmpSet.RelationDocument = new DataRelation(
             "DocSeq",
             new DataColumn[] { tmpSet.PO.Columns["POID"], tmpSet.PO.Columns["seq"] },
             new DataColumn[] { tmpSet.Document.Columns["POID"], tmpSet.Document.Columns["seq"] });
            return tmpSet;
        }

        private void T2Validating(object s, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
            if (dr == null)
            {
                return;
            }

            decimal pointRate = 0;
            if (MyUtility.Convert.GetDecimal(dr["T2Inspected_Yards"]).EqualDecimal(0))
            {
                pointRate = 0;
            }
            else
            {
                pointRate = MyUtility.Convert.GetDecimal(dr["T2Defect_Points"]) / MyUtility.Convert.GetDecimal(dr["T2Inspected_Yards"]) * 100;
            }

            string sqlWT = $@"
            SELECT WeaveTypeId
            FROM Fabric F WITH(NOLOCK) 
            LEFT JOIN PO_Supp_Detail  PSD WITH(NOLOCK)  ON PSD.SCIRefno = F.SCIRefno
            WHERE PSD.ID='{dr["poid"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'";

            string sqlIG = $@"
            SELECT InspectionGroup
            FROM Fabric F WITH(NOLOCK) 
            LEFT JOIN PO_Supp_Detail  PSD WITH(NOLOCK)  ON PSD.SCIRefno = F.SCIRefno
            WHERE PSD.ID='{dr["poid"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'";
            string strInspectionGroup = MyUtility.GetValue.Lookup(sqlIG);
            string strWeaveTypeId = MyUtility.GetValue.Lookup(sqlWT);
            string brandID = dr["BrandID"].ToString();

            string sqlWEAVETYPEID = string.Empty;

            if (brandID == "LLL")
            {
                sqlWEAVETYPEID = $@"
                DECLARE @Point as int = {pointRate}
                DECLARE @BrandID as varchar(15) = '{brandID}'
                DECLARE @WeaveTypeID as varchar(15) = '{strWeaveTypeId}'
                DECLARE @InspectionGroup varchar(15) = '{strInspectionGroup}'
                DECLARE @IsEmptyResult as bit = 0 

                SELECT @IsEmptyResult = IIF(Result = '' ,1 ,0)
                FROM FIR_Grade f WITH (NOLOCK) 
                where BrandID=@BrandID
                and InspectionGroup=@InspectionGroup
                and WeaveTypeID=@WeaveTypeID

                IF @IsEmptyResult = 1
                BEGIN

	                SELECT TOP 1 ShowGrade = Grade 
	                FROM FIR_Grade WITH (NOLOCK) 
	                WHERE WeaveTypeID = @WeaveTypeID
	                AND BrandID=@BrandID
	                and InspectionGroup=@InspectionGroup

                END

                ELSE 
                BEGIN
	                SELECT TOP 1 ShowGrade = ( IIF(isFormatInP01 = 1 ,ShowGrade , Grade))
	                FROM FIR_Grade WITH (NOLOCK) 
	                WHERE WeaveTypeID = @WeaveTypeID
	                AND Percentage >= IIF(@Point > 100, 100, @Point)
	                AND BrandID=@BrandID
	                and InspectionGroup=@InspectionGroup
	                ORDER BY Percentage --IIF(isFormatInP01 = 1 ,ShowGrade , Grade)
                END";
                dr["Grade"] = MyUtility.GetValue.Lookup(sqlWEAVETYPEID);
            }
            else
            {
                sqlWEAVETYPEID = $@"
                ---- 1. 取得預設的布種的等級
                SELECT [Grade]=MIN(Grade)
                INTO #default
                FROM FIR_Grade f WITH (NOLOCK) 
                WHERE f.WeaveTypeID= (
	                SELECT WeaveTypeId 
	                FROM Fabric F
	                LEFT JOIN PO_Supp_Detail  PSD ON PSD.SCIRefno = F.SCIRefno
	                WHERE PSD.ID='{dr["poid"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'
                ) 
                AND PERCENTAGE >= IIF({pointRate} > 100, 100, {pointRate} )
                AND BrandID=''

                ---- 2. 取得該品牌布種的等級
                SELECT [Grade]=MIN(Grade)
                INTO #withBrandID
                FROM FIR_Grade f WITH (NOLOCK) 
                WHERE f.WeaveTypeID= (
	                SELECT WeaveTypeId 
	                FROM Fabric F
	                LEFT JOIN PO_Supp_Detail  PSD ON PSD.SCIRefno = F.SCIRefno
	                WHERE PSD.ID='{dr["poid"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'
                ) 
                AND PERCENTAGE >= IIF({pointRate} > 100, 100, {pointRate} )
                AND BrandID='{brandID}'

                ---- 若該品牌有另外設定等級，就用該設定，不然用預設（主索引鍵是WeaveTypeID + Percentage + BrandID，因此不會找到多筆預設的Grade）
                SELECT ISNULL(Brand.Grade, ISNULL((SELECT Grade FROM #default),'') ) 
                FROM #withBrandID brand

                DROP TABLE #default,#withBrandID";
                dr["Grade"] = MyUtility.GetValue.Lookup(sqlWEAVETYPEID);
            }

            dr.EndEdit();
        }

        private void Find()
        {
            if (this.materialSet != null)
            {
                this.bs_Material.DataSource = null;
                this.bs_Document.DataSource = null;
                this.bs_Report.DataSource = null;
                this.materialSet.Dispose();
                this.materialSet = null;
            }

            var dataLoaded = this.LoadMaterialtableData();

            if (dataLoaded == null)
            {
                return;
            }

            if (dataLoaded.PO.Rows.Count == 0)
            {
                this.bs_Material.DataSource = null;
                this.bs_Document.DataSource = null;
                this.bs_Report.DataSource = null;
            }
            else
            {
                this.materialSet2 = dataLoaded;
                this.materialSet = dataLoaded;
                this.bs_Material.DataMember = "PO";
                this.bs_Material.DataSource = dataLoaded;
                this.bs_Document.DataMember = dataLoaded.RelationDocument.RelationName;
                this.bs_Document.DataSource = this.bs_Material;
                this.Bs_Document_PositionChanged(null, null);
            }
        }

        /// <summary>
        /// ISP20230564 移除 TabPage 1 存檔編輯功能
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.bs_Report.DataSource == null)
            {
                return;
            }

            DataTable dt = (DataTable)this.bs_Report.DataSource;
            DualResult result;
            string updSql = string.Empty;

            var row = this.grid_Document.GetDataRow(this.bs_Document.Position);
            if (dt.Rows.Count > 0)
            {
                switch (row["FileRule"].ToString())
                {
                    case "1":
                    case "2":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from UASentReport t
inner join #tmp s on s.Ukey = t.Ukey
";
                        break;
                    case "3":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from FirstDyelot t
inner join #tmp s on s.SuppID = t.SuppID
and s.TestDocFactoryGroup = t.TestDocFactoryGroup
and s.BrandRefno = t.BrandRefno
and s.ColorID = t.ColorID
and s.SeasonID = t.SeasonID
and t.deleteColumn = 0
";
                        break;
                    case "4":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from NewSentReport t
inner join #tmp s on s.Ukey = t.Ukey
";
                        break;
                    case "5":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from ExportRefnoSentReport t
inner join #tmp s on s.Ukey = t.Ukey
";
                        break;
                    default:
                        break;
                }

                result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, updSql, out DataTable odt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            // 記住當前Grid位置
            int a = this.bs_Material.Position;
            int b = this.bs_Document.Position;
            int c = this.bs_Report.Position;

            this.Find();

            // 給與上次的位置紀錄
            this.bs_Material.Position = a;
            this.bs_Document.Position = b;
            this.bs_Report.Position = c;
        }

        private void BtnSave2_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            if (this.dt1 == null || this.dt1.Rows.Count == 0)
            {
                return;
            }

            if (this.dt1.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).ToList().Count == 0)
            {
                MyUtility.Msg.WarningBox("No data changes.");
                return;
            }

            DataTable changedt = this.dt1.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).CopyToDataTable();

            string sqlupdate = $@"
update t
set t.FTYReceivedReport = s.ContinuityCard_FtyReceivedDate
,t.TestReportCheckClima = isnull(s.TestReportCheckClima,0)
,t.AWBno = s.ContinuityCard_AWB
,t.T2InspYds = isnull(s.T2Inspected_Yards, 0)
,t.T2DefectPoint = isnull(s.T2Defect_Points, 0)
,t.T2Grade = isnull(s.Grade, '')
,t.EditDate = GETDATE()
,t.Editname = '{Env.User.UserID}'
from NewSentReport t
inner join #tmp s on t.ExportID = s.ExportID
and t.PoID = s.POID
and t.Seq1 = s.Seq1
and t.Seq2 = s.Seq2
and t.DocumentName = 'Continuity card'
and t.BrandID = s.BrandID

update t
set t.FTYReceivedReport = s.Inspection_Report_FtyReceivedDate
,t.TestReportCheckClima = isnull(s.TestReportCheckClima,0)
,t.T2InspYds = isnull(s.T2Inspected_Yards, 0)
,t.T2DefectPoint = isnull(s.T2Defect_Points, 0)
,t.T2Grade = isnull(s.Grade, '')
,t.EditDate = GETDATE()
,t.Editname = '{Env.User.UserID}'
from NewSentReport t
inner join #tmp s on t.ExportID = s.ExportID
and t.PoID = s.POID
and t.Seq1 = s.Seq1
and t.Seq2 = s.Seq2
and t.DocumentName = 'Inspection report'
and t.BrandID = s.BrandID


update t
set t.FTYReceivedReport = s.TestReport_FtyReceivedDate
,t.TestReportCheckClima = isnull(s.TestReportCheckClima,0)
,t.T2InspYds = isnull(s.T2Inspected_Yards, 0)
,t.T2DefectPoint = isnull(s.T2Defect_Points, 0)
,t.T2Grade = isnull(s.Grade, '')
,t.EditDate = GETDATE()
,t.Editname = '{Env.User.UserID}'
from NewSentReport t
inner join #tmp s on t.ExportID = s.ExportID
and t.PoID = s.POID
and t.Seq1 = s.Seq1
and t.Seq2 = s.Seq2
and t.DocumentName = 'Test report'
and t.BrandID = s.BrandID

update t
set t.FTYReceivedReport = s.[1stBulkDyelot_FtyReceivedDate]
,t.EditDate = GETDATE()
,t.Editname = '{Env.User.UserID}'
from FirstDyelot t
inner join #tmp s on t.SuppID = s.SuppID
and t.TestDocFactoryGroup = s.TestDocFactoryGroup
and t.BrandRefno = s.FirstDyelot_BrandRefno
and t.ColorID = s.Color
and t.SeasonID = s.FirstDyelot_SeasonID
and t.deleteColumn = 0
;
";
            DataTable odt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(changedt, string.Empty, sqlupdate, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Success!");
            this.Page2_Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private string DocumentName;

        private void Bs_Document_PositionChanged(object sender, EventArgs e)
        {
            this.ChangeBsReport();
        }

        private void ChangeBsReport()
        {
            if (this.bs_Document.Current == null)
            {
                this.bs_Report.DataSource = null;
                return;
            }

            if (this.bs_Document.Position == -1)
            {
                this.bs_Report.DataSource = null;
                return;
            }

            var row = this.grid_Document.GetDataRow(this.bs_Document.Position);
            if (row == null)
            {
                row = ((System.Data.DataRowView)this.bs_Document.Current).Row;
                if (row == null)
                {
                    this.DocumentName = string.Empty;
                    this.bs_Report.DataSource = null;
                    return;
                }
            }

            var mainrow = this.grid_Material.GetDataRow(this.bs_Material.Position);
            if (mainrow == null)
            {
                this.bs_Report.DataSource = null;
                return;
            }

            this.SetupGrid_Report(row["FileRule"].ToString());
            List<SqlParameter> parmes = new List<SqlParameter>();
            string sql = string.Empty;
            switch (row["FileRule"].ToString())
            {
                case "1":
                    sql = @"
Select sr.Ukey  
       ,sr.UniqueKey
       ,TestReportTestDate = CONVERT(VARCHAR(10), sr.TestReportTestDate, 23)
	   ,TestReport = CONVERT(VARCHAR(10), sr.TestReport, 23) 
       ,TestSeasonID
       ,DueSeason
       ,DueDate
       ,FTYReceivedReport
       ,sr.AddName
       ,sr.AddDate
       ,sr.EditName
       ,sr.EditDate
       ,FileRule = '1'
FROM UASentReport sr
WHERE exists (
    select 1
    from BrandRelation b
    where b.BrandID = sr.BrandID
    and b.SuppGroup = sr.SuppID
    and b.SuppID = @SuppID)
and (sr.BrandRefno = @BrandRefno  or sr.BrandRefno = @Refno)
and sr.BrandID = @BrandID
and sr.DocumentName = @DocumentName
                    ";
                    parmes.Add(new SqlParameter("@SuppID", mainrow["SuppID"]));
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@BrandID", row["BrandID"].ToString()));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"].ToString()));
                    break;
                case "2":
                    sql = @"
Select sr.Ukey
       ,sr.UniqueKey
       ,TestReportTestDate = CONVERT(VARCHAR(10), sr.TestReportTestDate, 23)
	   ,TestReport = CONVERT(VARCHAR(10), sr.TestReport, 23) 
       ,TestSeasonID
       ,DueSeason
       ,DueDate
       ,FTYReceivedReport
       ,sr.AddName
       ,sr.AddDate
       ,sr.EditName
       ,sr.EditDate
       ,FileRule = '2'
FROM UASentReport sr
WHERE exists (
    select 1
    from BrandRelation b
    where b.BrandID = sr.BrandID
    and b.SuppGroup = sr.SuppID
    and b.SuppID = @SuppID)
and (sr.BrandRefno = @BrandRefno  or sr.BrandRefno = @Refno)
and sr.ColorID = @ColorID 
and sr.BrandID = @BrandID
and sr.DocumentName = @DocumentName 
                    ";
                    parmes.Add(new SqlParameter("@SuppID", mainrow["SuppID"]));
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@BrandID", row["BrandID"].ToString()));
                    parmes.Add(new SqlParameter("@ColorID", mainrow["Color"].ToString()));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"].ToString()));
                    break;
                case "3":
                    sql = @"
if object_id('tempdb..#probablySeasonList') is not null 
Drop Table #probablySeasonList

Select RowNo = ROW_NUMBER() OVER(ORDER by Month), ID 
Into #probablySeasonList
FROM(
    Select DISTINCT Month, ID
    From Season where BrandID = @brandID or BrandID in (select BrandID From MaterialDocument_Brand Where DocumentName = @documentName and MergedBrand = @BrandID)
)a

Select DocSeason = f.SeasonID
       ,f.Period 
       ,f.TestDocFactoryGroup
       ,f.FTYReceivedReport
       ,f.ReceivedRemark
       ,f.FirstDyelot
       ,f.AddName
       ,f.AddDate
       ,f.EditName
       ,f.EditDate
       ,FileRule = '3'
       ,f.SuppID
       ,f.AWBno
       ,f.BrandRefno
       ,f.ColorID
       ,f.SeasonID
       ,f.LOT
FROM dbo.FirstDyelot f
INNER JOIN #probablySeasonList season ON f.SeasonID = season.ID
INNER JOIN #probablySeasonList seasonSCI ON seasonSCI.ID = @SeasonID
WHERE exists (
    select 1
    from BrandRelation b
    where b.BrandID = f.BrandID
    and b.SuppGroup = f.SuppID
    and b.SuppID = @SuppID)
and f.BrandRefno = @BrandRefno
and f.ColorID = @ColorID 
and f.BrandID = @BrandID 
and f.DocumentName = @DocumentName
and ((@FactoryID = 'SPR' and f.TestDocFactoryGroup in ('SPR', 'SPX'))
    or (@FactoryID != 'SPR' and f.TestDocFactoryGroup = @FactoryID))
and f.deleteColumn = 0
Order by f.SeasonID desc";
                    parmes.Add(new SqlParameter("@SuppID", mainrow["SuppID"]));
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@BrandID", row["BrandID"]));
                    parmes.Add(new SqlParameter("@ColorID", mainrow["Color"]));
                    parmes.Add(new SqlParameter("@SeasonID", mainrow["Season"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    parmes.Add(new SqlParameter("@FactoryID", Env.User.Factory));
                    break;
                case "4":
                    sql = @"
Select Ukey
       ,UniqueKey
       ,ReportDate = CONVERT(VARCHAR(10), ReportDate, 23)
       ,AWBNO 
       ,ExportID
       ,FTYReceivedReport
       ,AddName
       ,AddDate
       ,EditName
       ,EditDate
       ,FileRule = '4'
FROM NewSentReport 
WHERE PoID = @PoID and Seq1 = @Seq1 and Seq2 = @Seq2 and BrandID = @BrandID and DocumentName = @DocumentName
                    ";
                    parmes.Add(new SqlParameter("@POID", mainrow["POID"]));
                    parmes.Add(new SqlParameter("@Seq1", mainrow["Seq1"]));
                    parmes.Add(new SqlParameter("@Seq2", mainrow["Seq2"]));
                    parmes.Add(new SqlParameter("@BrandID", mainrow["BrandID"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    break;
                case "5":
                    sql = @"
Select  sr.Ukey
       ,sr.UniqueKey
       ,ReportDate = CONVERT(VARCHAR(10), sr.ReportDate, 23)
       ,sr.AWBNO 
       ,FTYReceivedReport
       ,sr.ExportID
       ,sr.AddName
       ,sr.AddDate
       ,sr.EditName
       ,sr.EditDate
       ,FileRule = '5'
FROM ExportRefnoSentReport sr
INNER JOIN Export e on sr.ExportID = e.ID
WHERE sr.ColorID = @ColorID 
and (sr.BrandRefno = @BrandRefno  or sr.BrandRefno = @Refno)
and sr.BrandID = @BrandID 
and sr.DocumentName = @DocumentNames
and exists(
    select 1 FROM Export_Detail ed WITH (NOLOCK) 
	inner join PO_Supp_Detail psd WITH (NOLOCK)  on psd.ID = ed.PoID
	and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
    inner join Fabric f2 WITH (NOLOCK) on f2.SCIRefno = psd.SCIRefNo 
    where ed.ID = e.ID 
    and ed.POID = @POID and f2.BrandRefno = sr.BrandRefno
)
                    ";
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@POID", mainrow["POID"]));
                    parmes.Add(new SqlParameter("@ColorID", mainrow["Color"]));
                    parmes.Add(new SqlParameter("@BrandID", mainrow["BrandID"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    break;
            }

            DualResult result = DBProxy.Current.Select(string.Empty, sql, parmes, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.bs_Report.DataSource = dt;
        }

        private void Btn_ToExcel_Click(object sender, EventArgs e)
        {
            if ((MaterialtableDataSet)((BindingSource)this.grid_Material.DataSource).DataSource == null)
            {
                return;
            }

            DataTable dtMaterial = ((MaterialtableDataSet)((BindingSource)this.grid_Material.DataSource).DataSource).PO;
            if (dtMaterial.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not Found.");
                return;
            }

            string sqlcmd = $@"
Select RowNo = ROW_NUMBER() OVER(ORDER by Month), ID 
Into #probablySeasonList
From SeasonSCI

select distinct
selected = cast(0 as bit),
FileExistI= cast(0 as bit),
FileExistT= cast(0 as bit),
ed.id,
ed.InvoiceNo,
Export.ETA,
ATA = Export.WhseArrival,
seasonID = s.ID,
ed.PoID,
seq=ed.seq1+'-'+ed.seq2,
o.BrandId,
SuppID = s2.ID,
s2.AbbEN,
psd.Refno,
f.BrandRefNo,
f.WeaveTypeID,
ColorID = isnull(psdsC.SpecValue ,''),
[ColorName] = c.ColorName,
Qty = isnull(ed.Qty,0) + isnull(ed.Foc,0),
FirstDyelot.FirstDyelot, 
FirstDyelot_FTYReceivedReport = FirstDyelot.FTYReceivedReport,
a.T1InspectedYards,
b.T1DefectPoints,
ed.seq1,
ed.seq2,
f.Clima,
Export.CloseDate,
--o.SeasonID,
f.RibItem
into #tmpBasc
from Export_Detail ed with(nolock)
inner join Export with(nolock) on Export.id = ed.id and Export.Confirm = 1
inner join orders o with(nolock) on o.id = ed.PoID
left join Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
left join PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
left join Supp su with(nolock) on su.ID = ps.SuppID
left join BrandRelation as bs WITH (NOLOCK) ON bs.BrandID = o.BrandID and bs.SuppID = su.ID
left Join Supp s2 WITH (NOLOCK) on bs.SuppGroup = s2.ID
left join Season s with(nolock) on s.ID=o.SeasonID and s.BrandID = o.BrandID
left join Factory fty with (nolock) on fty.ID = o.FactoryID
left join Fabric f with(nolock) on f.SCIRefno =psd.SCIRefno
Left join #probablySeasonList seasonSCI on seasonSCI.ID = s.SeasonSCIID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	Select Top 1 FirstDyelot,FTYReceivedReport,SeasonID,TestDocFactoryGroup,BrandRefno
	From dbo.FirstDyelot fd
	Inner join #probablySeasonList season on fd.SeasonID = season.ID
	WHERE fd.BrandRefno = f.BrandRefNo
    and fd.ColorID = isnull(psdsC.SpecValue ,'') 
    and fd.SuppID = s2.ID
    and fd.TestDocFactoryGroup = fty.TestDocFactoryGroup
    and seasonSCI.RowNo >= season.RowNo
    and fd.deleteColumn = 0
	Order by season.RowNo Desc
)FirstDyelot
outer apply(
	select T1InspectedYards=sum(fp.ActualYds)
	from fir f
	left join FIR_Physical fp on fp.id=f.id
	left join Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)a
outer apply(
	select T1DefectPoints = sum(fp.TotalPoint)
	from fir f
	left join FIR_Physical fp on fp.id=f.id
	left join Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)b
outer apply(
	select [ColorName] = iif(c.Varicolored > 1, c.Name, c.ID)
	from Color c
	where c.ID = isnull(psdsC.SpecValue ,'')
	and c.BrandID = psd.BrandID 
)c
where 1=1
and exists(
	select * from #tmp t
	where t.poid = ed.PoID
	and t.Seq1 = ed.Seq1	
	and t.seq2 = ed.Seq2
	and t.Type = f.Type
)

select t.*
	,sr.documentName
	,sr.ReportDate
    ,sr.T2InspYds
    ,sr.T2DefectPoint
    ,sr.T2Grade
	,sr2.AWBno
    ,sr2.TestReportCheckClima
into #tmpReportDate
from #tmpBasc t
left join NewSentReport sr with (nolock) on sr.exportID = t.ID and sr.poid = t.PoID and sr.Seq1 =t.Seq1 and sr.Seq2 = t.Seq2
outer apply (
	select sr2.AWBno,sr2.TestReportCheckClima
	from NewSentReport sr2 with (nolock) 
	where sr2.exportID = t.ID and sr2.poid = t.PoID and sr2.Seq1 =t.Seq1 and sr2.Seq2 = t.Seq2
	and sr2.documentName = 'Continuity card'
)sr2



select t.*
	,sr.documentName
    ,sr.FTYReceivedReport
into #tmpFTYReceivedReport
from #tmpBasc t
left join NewSentReport sr with (nolock) on sr.exportID = t.ID and sr.poid = t.PoID and sr.Seq1 =t.Seq1 and sr.Seq2 = t.Seq2


select 
[WK#] = a.ID
,[Invoice#] = a.InvoiceNo
,[ATA] = a.ATA
,[ETA] = a.Eta
,[Season] = a.seasonID
,[SP#] = a.PoID
,[Seq#] = a.seq
,[Brand] = a.BrandID
,[SuppID] = a.SuppID
,[Supp Name] = a.AbbEN
,[Ref#] = a.Refno
,[Weave Type] = a.WeaveTypeID
,[Color] = a.ColorID
,[Qty] = a.Qty
,[Inspection Report Fty Received Date] = c.[Inspection Report]
,[Inspection Report Supp Sent Date] = b.[Inspection Report]
,[Test Report Fty Received Date] = c.[Test report]
,[Test Report Supp Sent Date] = b.[Test report]
,[Continuity Card Fty Received Date] = c.[Continuity card]
,[Continuity Card Supp Sent Date] = b.[Continuity card]
,[Continuity Card AWB#] = b.AWBno
,[1st Bulk Dyelot Fty Received Date] = a.FirstDyelot_FTYReceivedReport
,[1st Bulk Dyelot Supp Sent Date] = a.FirstDyelot
,[T2 Inspected Yards] = b.T2InspYds
,[T2 Defect Points] = b.T2DefectPoint
,[Grade] = b.T2Grade
,[T1 Inspected Yards] = b.T1InspectedYards
,[T1 Defect Points] = b.T1DefectPoints
from #tmpBasc a
inner join (
	select *
	from(
		select ID,PoID,Seq1,Seq2,ReportDate ,documentname,AWBno,T2InspYds,T2DefectPoint,T2Grade,T1InspectedYards,T1DefectPoints,TestReportCheckClima
		from #tmpReportDate
	) s	
	pivot(
		max(ReportDate)
		for documentname in([Continuity card],[Inspection Report],[Test report])
	) aa
)b on a.ID = b.ID and a.PoID = b.PoID and a.Seq1=b.Seq1 and a.Seq2=b.Seq2
inner join (
	select *
	from(
		select ID,PoID,Seq1,Seq2,FTYReceivedReport ,documentname
		from #tmpFTYReceivedReport 
	)s	
	pivot(
		max(FTYReceivedReport)
		for documentname in([Continuity card],[Inspection Report],[Test report])
	) aa
)c on a.ID = c.ID and a.PoID = c.PoID and a.Seq1 = c.Seq1 and a.Seq2 = c.Seq2


drop table #probablySeasonList,#tmpBasc,#tmpFTYReceivedReport,#tmpReportDate
";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtMaterial, string.Empty, sqlcmd, out DataTable dtEx, "#tmp");
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dtEx.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not Found.");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P52.xltx"); // 預先開啟excel app
            objApp.Visible = false;
            MyUtility.Excel.CopyToXls(dtEx, string.Empty, "Quality_P52.xltx", 1, false, null, objApp);      // 將datatable copy to excel
            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
            objApp.Cells.EntireRow.AutoFit();       ////自動欄高

            string excelFile = Class.MicrosoftFile.GetName("Quality_P52");
            objApp.ActiveWorkbook.SaveAs(excelFile);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            excelFile.OpenFile();
        }

        private void ComboMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.isFormLoaded)
            {
                this.Find();
            }
        }

        private void Bs_Material_PositionChanged(object sender, EventArgs e)
        {
            this.ChangeBsReport();
        }

        private void BtnQuery2_Click(object sender, EventArgs e)
        {
            this.Page2_Query();
        }

        private void Page2_Query()
        {
            // 檢查[表頭][ETA+SP#+PO#] 如果全為空請跳出訊息並return
            if (MyUtility.Check.Empty(this.dateATA.Value1) && MyUtility.Check.Empty(this.dateATA.Value2) && MyUtility.Check.Empty(this.dateRangeETA.Value1) && MyUtility.Check.Empty(this.dateRangeETA.Value1) && MyUtility.Check.Empty(this.txtsp2.Text) && MyUtility.Check.Empty(this.txtpo.Text))
            {
                MyUtility.Msg.WarningBox("Please select ATA or ETA or SP# or PO# at least one field entry.");
                return;
            }

            this.listControlBindingSource1.DataSource = null;

            #region Where
            string sqlwhere = string.Empty;
            List<string> sqlwheres = new List<string>();
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            if (!MyUtility.Check.Empty(this.dateRangeETA.Value1) && !MyUtility.Check.Empty(this.dateRangeETA.Value2))
            {
                listSQLParameter.Add(new SqlParameter("@ETA1", this.dateRangeETA.Value1));
                listSQLParameter.Add(new SqlParameter("@ETA2", this.dateRangeETA.Value2));
                sqlwheres.Add(" r.ETA between @ETA1 and @ETA2 ");
            }

            if (!MyUtility.Check.Empty(this.dateATA.Value1) && !MyUtility.Check.Empty(this.dateATA.Value2))
            {
                listSQLParameter.Add(new SqlParameter("@ATA1", this.dateATA.Value1));
                listSQLParameter.Add(new SqlParameter("@ATA2", this.dateATA.Value2));
                sqlwheres.Add(" r.WhseArrival between @ATA1 and @ATA2 ");
            }

            if (!MyUtility.Check.Empty(this.txtsp2.Text))
            {
                listSQLParameter.Add(new SqlParameter("@sp", this.txtsp2.Text));
                sqlwheres.Add(" ed.PoID = @sp ");
            }

            if (!MyUtility.Check.Empty(this.txtSeq.Seq1))
            {
                listSQLParameter.Add(new SqlParameter("@seq1", this.txtSeq.Seq1));
                sqlwheres.Add(" ed.Seq1 = @seq1 ");
            }

            if (!MyUtility.Check.Empty(this.txtSeq.Seq2))
            {
                listSQLParameter.Add(new SqlParameter("@seq2", this.txtSeq.Seq2));
                sqlwheres.Add(" ed.Seq2 = @seq2 ");
            }

            if (!MyUtility.Check.Empty(this.txtpo.Text))
            {
                listSQLParameter.Add(new SqlParameter("@po", this.txtpo.Text));
                sqlwheres.Add(" o.CustPONo = @po ");
            }

            if (sqlwheres.Count > 0)
            {
                sqlwhere = "where " + string.Join(" and ", sqlwheres);
            }
            #endregion Where

            #region Sqlcmd
            string sqlcmd = $@"
Select RowNo = ROW_NUMBER() OVER(ORDER by Month), ID 
Into #probablySeasonList
From SeasonSCI

select distinct
selected = cast(0 as bit),
FileExistI= cast(0 as bit),
FileExistT= cast(0 as bit),
ed.id,
ed.InvoiceNo,
[ReceivingID] = r.ID,
r.ETA,
r.WhseArrival,
seasonID = s.ID,
ed.PoID,
seq=ed.seq1+'-'+ed.seq2,
o.BrandId,
SuppID = s2.ID,
s2.AbbEN,
psd.Refno,
f.BrandRefNo,
f.WeaveTypeID,
ColorID = isnull(psdsC.SpecValue ,''),
[ColorName] = c.ColorName,
Qty = isnull(ed.Qty,0) + isnull(ed.Foc,0),
FirstDyelot.FirstDyelot, 
FirstDyelot_FTYReceivedReport = FirstDyelot.FTYReceivedReport,
FirstDyelot_BrandRefno = FirstDyelot.BrandRefno,
FirstDyelot.TestDocFactoryGroup,
FirstDyelot_SeasonID = FirstDyelot.SeasonID,
a.T1InspectedYards,
b.T1DefectPoints,
ed.seq1,
ed.seq2,
f.Clima,
Export.CloseDate,
f.RibItem
into #tmpBasc
from Receiving r WITH(NOLOCK)
inner join Receiving_Detail rd WITH(NOLOCK) on r.Id=rd.id	
inner join Export_Detail ed WITH(NOLOCK) on ed.PoID = rd.PoId and ed.seq1 = rd.seq1 and ed.Seq2 = rd.seq2 and r.ExportID = ed.id
inner join Export with(nolock) on Export.id = ed.id and Export.Confirm = 1
inner join orders o with(nolock) on o.id = ed.PoID
left join Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
left join PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
left join Supp su with(nolock) on su.ID = ps.SuppID
left join BrandRelation as bs WITH (NOLOCK) ON bs.BrandID = o.BrandID and bs.SuppID = su.ID
left Join Supp s2 WITH (NOLOCK) on bs.SuppGroup = s2.ID
left join Season s with(nolock) on s.ID=o.SeasonID and s.BrandID = o.BrandID
left join Factory fty with (nolock) on fty.ID = o.FactoryID
left join Fabric f with(nolock) on f.SCIRefno =psd.SCIRefno
Left join #probablySeasonList seasonSCI on seasonSCI.ID = s.SeasonSCIID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	Select Top 1 FirstDyelot,FTYReceivedReport,SeasonID,TestDocFactoryGroup,BrandRefno
	From dbo.FirstDyelot fd
	Inner join #probablySeasonList season on fd.SeasonID = season.ID
	WHERE fd.BrandRefno = f.BrandRefNo
    and fd.ColorID = isnull(psdsC.SpecValue ,'') 
    and fd.SuppID = s2.ID
    and fd.TestDocFactoryGroup = fty.TestDocFactoryGroup
    and seasonSCI.RowNo >= season.RowNo
    and fd.deleteColumn = 0
	Order by season.RowNo Desc
)FirstDyelot
outer apply(
	select T1InspectedYards=sum(fp.ActualYds)
	from fir f
	left join FIR_Physical fp on fp.id=f.id
	left join Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)a
outer apply(
	select T1DefectPoints = sum(fp.TotalPoint)
	from fir f
	left join FIR_Physical fp on fp.id=f.id
	left join Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)b
outer apply(
	select [ColorName] = iif(c.Varicolored > 1, c.Name, c.ID)
	from Color c
	where c.ID = isnull(psdsC.SpecValue ,'')
	and c.BrandID = psd.BrandID 
)c
{sqlwhere}
and psd.FabricType = 'F'
and (ed.qty + ed.Foc) > 0
and o.Category in('B','M')

-- 準備樞紐temptable
select t.*
	,sr.documentName
	,sr.ReportDate
    ,sr.T2InspYds
    ,sr.T2DefectPoint
    ,sr.T2Grade
	,sr2.AWBno
	,sr2.TestReportCheckClima
into #tmpReportDate
from #tmpBasc t
left join NewSentReport sr with (nolock) on sr.exportID = t.ID and sr.poid = t.PoID and sr.Seq1 =t.Seq1 and sr.Seq2 = t.Seq2
outer apply (
	select sr2.AWBno,sr2.TestReportCheckClima
	from NewSentReport sr2 with (nolock) 
	where sr2.exportID = t.ID and sr2.poid = t.PoID and sr2.Seq1 =t.Seq1 and sr2.Seq2 = t.Seq2
	and sr2.documentName = 'Continuity card'
)sr2


select t.*
	,sr.documentName
    ,sr.FTYReceivedReport
into #tmpFTYReceivedReport
from #tmpBasc t
left join NewSentReport sr with (nolock) on sr.exportID = t.ID and sr.poid = t.PoID and sr.Seq1 =t.Seq1 and sr.Seq2 = t.Seq2

-- 將上面2 個temp table 用樞紐轉成一筆資料
select distinct
a.selected
,[ExportID] = a.ID
,[InvoiceNo] = a.InvoiceNo
,[WhseArrival] = a.WhseArrival
,[ReceivingID] = a.ReceivingID
,[ETA] = a.Eta
,[SeasonID] = a.seasonID
,[POID] = a.PoID
,[Seq] = a.seq
,a.Seq1,a.Seq2
,[BrandID] = a.BrandID
,[SuppID] = a.SuppID -- FirstDyelot PKey
,[SuppName] = a.AbbEN
,[RefNo] = a.Refno
,a.TestDocFactoryGroup -- FirstDyelot PKey
,a.FirstDyelot_BrandRefno-- FirstDyelot PKey
,a.FirstDyelot_SeasonID -- FirstDyelot PKey
,a.Clima
,[WeaveTypeID] = a.WeaveTypeID
,[Color] = a.ColorID -- FirstDyelot PKey
,[Qty] = a.Qty
,[Inspection_Report_FtyReceivedDate] = c.[Inspection Report]
,[Inspection_Report_TPESentDate] = b.[Inspection Report]
,[TestReport_FtyReceivedDate] = c.[Test report]
,[TestReport_TPESentDate] = b.[Test report]
,[TestReportCheckClima] = b.TestReportCheckClima
,[ContinuityCard_FtyReceivedDate] = c.[Continuity card]
,[ContinuityCard_TPESentDate] = b.[Continuity card]
,[ContinuityCard_AWB] = b.AWBno
,[1stBulkDyelot_FtyReceivedDate] = a.FirstDyelot_FTYReceivedReport
,[1stBulkDyelot_TPESentDate] = a.FirstDyelot
,[T2Inspected_Yards] = b.T2InspYds
,[T2Defect_Points] = b.T2DefectPoint
,[Grade] = b.T2Grade
,[T1Inspected_Yards] = b.T1InspectedYards
,[T1Defect_Points] = b.T1DefectPoints
,[bitRefnoColor] = case when a.Clima = 1 then ROW_NUMBER() over(partition by Clima, SuppID, Refno, ColorID, CloseDate order by CloseDate) else 0 end
,[NewSentReport_Exists] = IIF(b.[Inspection Report] != '' or b.[Test report] != '' or b.[Continuity card] != '', 'Y','N')
from #tmpBasc a
inner join (
	select *
	from(
		select ID,PoID,Seq1,Seq2,ReportDate ,documentname,AWBno,T2InspYds,T2DefectPoint,T2Grade,T1InspectedYards,T1DefectPoints,TestReportCheckClima
		from #tmpReportDate
	) s	
	pivot(
		max(ReportDate)
		for documentname in([Continuity card],[Inspection Report],[Test report])
	) aa
)b on a.ID = b.ID and a.PoID = b.PoID and a.Seq1=b.Seq1 and a.Seq2=b.Seq2
inner join (
	select *
	from(
		select ID,PoID,Seq1,Seq2,FTYReceivedReport ,documentname
		from #tmpFTYReceivedReport 
	)s	
	pivot(
		max(FTYReceivedReport)
		for documentname in([Continuity card],[Inspection Report],[Test report])
	) aa
)c on a.ID = c.ID and a.PoID = c.PoID and a.Seq1 = c.Seq1 and a.Seq2 = c.Seq2


drop table #probablySeasonList,#tmpBasc,#tmpFTYReceivedReport,#tmpReportDate
";
            #endregion Sqlcmd
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listSQLParameter, out this.dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.dt1.AcceptChanges();
            this.listControlBindingSource1.DataSource = this.dt1;
        }

        private bool CheckDate(DateTime? dateValue)
        {
            bool isBetween2000And2099 = dateValue.Value.Year >= 2000 && dateValue.Value.Year <= 2099;

            if (isBetween2000And2099 == false)
            {
                MyUtility.Msg.WarningBox("Date shoule be between 2000 ~ 2099");
                return false;
            }

            return true;
        }
    }

    public class MaterialtableDataSet : DataSet
    {
        private DataRelation _RelationDocument = null;

        /// <summary>
        /// 由採購資料找DocumentName
        /// </summary>
        public DataRelation RelationDocument
        {
            get
            {
                return this._RelationDocument;
            }

            set
            {
                this._RelationDocument = value;
                this.Relations.Add(value);
            }
        }

        private DataRelation _RelationDocumentReport;

        /// <summary>
        /// 從採購資料找ReportData
        /// </summary>
        public DataRelation RelationDocumentReport
        {
            get
            {
                return this._RelationDocumentReport;
            }

            set
            {
                this._RelationDocumentReport = value;
                this.Relations.Add(value);
            }
        }

        /// <inheritdoc />
        public DataTable PO
        {
            get
            {
                return this.Tables["PO"];
            }
        }

        /// <inheritdoc />
        public DataTable Document
        {
            get
            {
                return this.Tables["Document"];
            }
        }

        /// <inheritdoc />
        public DataTable Report
        {
            get
            {
                return this.Tables["Report"];
            }
        }

        /// <summary>
        /// use po data to lookup dataRow
        /// </summary>
        public ILookup<string, DataRow> LookupMaterialDataRow { get; set; } = null;
    }
}
