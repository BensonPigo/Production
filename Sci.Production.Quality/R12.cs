using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R12 : Win.Tems.PrintForm
    {
        private readonly List<SqlParameter> parameters = new List<SqlParameter>();
        private string Sqlcmd;
        private DataTable[] PrintData;
        private string baseReceivingSql = @"
select	f.POID,
		[Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2),
		a.ExportId,
		f.ReceivingID,
		o.StyleID,
		o.BrandID,
		f.Suppid,
		psd.Refno,
		psd.ColorID,
		[ArriveWH_Date] = a.WhseArrival,
        {0}
from Fir f with (nolock)
inner join Receiving a with (nolock) on a.Id = f.ReceivingID
inner join Receiving_Detail b with (nolock) on a.id = b.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
left join Orders o with (nolock) on o.ID = f.POID
left join PO_Supp_Detail psd with (nolock) on psd.ID = f.POID and psd.SEQ1 = f.SEQ1 and psd.SEQ2 = f.SEQ2
left join Fabric fa with (nolock) on fa.SCIRefno = psd.SCIRefno
left join {1} i with (nolock) on i.ID = f.ID and i.Roll = b.Roll and i.Dyelot = b.Dyelot
{3}
where 1 = 1
{2}
";

        private string baseTransferInSql = @"
select	f.POID,
		[Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2),
		[ExportId] = '',
		f.ReceivingID,
		o.StyleID,
		o.BrandID,
		f.Suppid,
		psd.Refno,
		psd.ColorID,
		[ArriveWH_Date] = a.IssueDate,
        {0}
from Fir f with (nolock)
inner join TransferIn a with (nolock) on a.Id = f.ReceivingID
inner join TransferIn_Detail b with (nolock) on b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
left join Orders o with (nolock) on o.ID = f.POID
left join PO_Supp_Detail psd with (nolock) on psd.ID = f.POID and psd.SEQ1 = f.SEQ1 and psd.SEQ2 = f.SEQ2
left join Fabric fa with (nolock) on fa.SCIRefno = psd.SCIRefno
left join {1} i with (nolock) on i.ID = f.ID and i.Roll = b.Roll and i.Dyelot = b.Dyelot
{3}
where 1 = 1
{2}
";

        /// <inheritdoc/>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboInspection.SelectedIndex = 0;
            this.comboInspectionResult.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.dateArriveWHDate.Value1.Empty() &&
                this.txtSP1.Text.Empty() && this.txtSP2.Text.Empty() &&
                this.txtWK1.Text.Empty() && this.txtWK2.Text.Empty() &&
                !this.dateInspectionDate.HasValue)
            {
                MyUtility.Msg.WarningBox("Arrive W/H Date, SP#, WK# and Inspection Date can't all empty!");
                return false;
            }

            this.Sqlcmd = string.Empty;
            this.parameters.Clear();

            string where1 = string.Empty;
            string where2 = string.Empty;

            if (!this.dateArriveWHDate.Value1.Empty())
            {
                where1 += $"and a.WhseArrival between @Date1 and @Date2" + Environment.NewLine;
                where2 += $"and a.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                this.parameters.Add(new SqlParameter("@Date1", this.dateArriveWHDate.Value1));
                this.parameters.Add(new SqlParameter("@Date2", this.dateArriveWHDate.Value2));
            }

            if (!this.txtSP1.Text.Empty())
            {
                where1 += $"and f.POID between @SP1 and @SP2" + Environment.NewLine;
                where2 += $"and f.POID between @SP1 and @SP2" + Environment.NewLine;
                this.parameters.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                this.parameters.Add(new SqlParameter("@SP2", this.txtSP2.Text));
            }

            if (!this.txtWK1.Text.Empty())
            {
                where1 += $"and a.ExportID between @ExportID1 and @ExportID2" + Environment.NewLine;
                this.parameters.Add(new SqlParameter("@ExportID1", this.txtSP1.Text));
                this.parameters.Add(new SqlParameter("@ExportID2", this.txtSP2.Text));
            }

            if (this.dateInspectionDate.HasValue)
            {
                where1 += $"and i.InspDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                where2 += $"and i.InspDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                this.parameters.Add(new SqlParameter("@InsDate1", this.dateInspectionDate.Value1));
                this.parameters.Add(new SqlParameter("@InsDate2", this.dateInspectionDate.Value2));
            }

            switch (this.comboInspectionResult.Text)
            {
                case "Pass":
                    where1 += $"and i.Result = 'Pass'" + Environment.NewLine;
                    where2 += $"and i.Result = 'Pass'" + Environment.NewLine;
                    break;
                case "Fail":
                    where1 += $"and i.Result = 'Fail'" + Environment.NewLine;
                    where2 += $"and i.Result = 'Fail'" + Environment.NewLine;
                    break;
                case "Pass/Fail":
                    where1 += $"and i.Result in ('Pass', 'Fail')" + Environment.NewLine;
                    where2 += $"and i.Result in ('Pass', 'Fail')" + Environment.NewLine;
                    break;
                case "Not yet inspected":
                    where1 += $"and (i.Result = '' or i.Result is null)" + Environment.NewLine;
                    where2 += $"and (i.Result = '' or i.Result is null)" + Environment.NewLine;
                    break;
                default:
                    break;
            }

            #region Physical
            if (this.comboInspection.Text == "Physical" || this.comboInspection.Text == "All")
            {
                string joinPhysical = @"
left join pass1 p1 with (nolock) on p1.id = f.PhysicalInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";
                string colPhysical = @"
[NonPhysical] = iif(f.NonPhysical = 1, 'Y', ''),
[PhysicalInspector] = Concat(p1.ID, '-', p1.Name),
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate,
i.Roll,
i.Dyelot,
i.TicketYds,
i.ActualYds,
[DiffLth] = i.ActualYds - i.TicketYds,
i.TransactionID,
fa.Width,
i.FullWidth,
i.ActualWidth,
i.TotalPoint,
i.PointRate,
i.Result,
i.Grade,
[Moisture] = iif(i.Moisture = 1, 'Y', ''),
i.Remark,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name)
";
                this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colPhysical, "FIR_Physical", where1, joinPhysical)}
union all
{string.Format(this.baseTransferInSql, colPhysical, "FIR_Physical", where2, joinPhysical)}
";
            }
            #endregion

            #region Weight
            if (this.comboInspection.Text == "Weight" || this.comboInspection.Text == "All")
            {
                string joinWeight = @"
left join pass1 p1 with (nolock) on p1.id = f.WeightInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";

                string colWeight = @"
[NonWeight] = iif(f.NonWeight = 1, 'Y', ''),
[WeightInspector] = Concat(p1.ID, '-', p1.Name),
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate,
i.Roll,
i.Dyelot,
i.WeightM2,
i.AverageWeightM2,
i.Difference,
i.Result,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colWeight, "FIR_Weight", where1, joinWeight)}
union all
{string.Format(this.baseTransferInSql, colWeight, "FIR_Weight", where2, joinWeight)}
";
            }
            #endregion

            #region Shade Band
            if (this.comboInspection.Text == "Shade Band" || this.comboInspection.Text == "All")
            {
                string joinShadeBand = @"
left join pass1 p1 with (nolock) on p1.id = f.ShadeboneInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";

                string colShadeBand = @"
[NonShadeBond] = iif(f.NonShadeBond = 1, 'Y', ''),
[ShadeboneInspector] = Concat(p1.ID, '-', p1.Name),
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate,
i.Roll,
i.Dyelot,
i.TicketYds,
i.Scale,
i.Result,
i.Tone,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colShadeBand, "FIR_Shadebone", where1, joinShadeBand)}
union all
{string.Format(this.baseTransferInSql, colShadeBand, "FIR_Shadebone", where2, joinShadeBand)}
";
            }
            #endregion

            #region Continuity
            if (this.comboInspection.Text == "Continuity" || this.comboInspection.Text == "All")
            {
                string joinContinuity = @"
left join pass1 p1 with (nolock) on p1.id = f.ContinuityInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";

                string colContinuity = @"
[NonContinuity] = iif(f.NonContinuity = 1, 'Y', ''),
[ContinuityInspector] = Concat(p1.ID, '-', p1.Name),
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate,
i.Roll,
i.Dyelot,
i.TicketYds,
i.Scale,
i.Result,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colContinuity, "FIR_Continuity", where1, joinContinuity)}
union all
{string.Format(this.baseTransferInSql, colContinuity, "FIR_Continuity", where2, joinContinuity)}
";
            }
            #endregion

            #region Odor
            if (this.comboInspection.Text == "Odor" || this.comboInspection.Text == "All")
            {
                string joinOdor = @"
left join pass1 p1 with (nolock) on p1.id = f.OdorInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";

                string colOdor = @"
[NonOdor] = iif(f.NonOdor = 1, 'Y', ''),
[OdorInspector] = Concat(p1.ID, '-', p1.Name),
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate,
i.Roll,
i.Dyelot,
i.Result,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colOdor, "FIR_Odor", where1, joinOdor)}
union all
{string.Format(this.baseTransferInSql, colOdor, "FIR_Odor", where2, joinOdor)}
";
            }
            #endregion

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.Sqlcmd.ToString(), this.parameters, out this.PrintData);
        }

        private void CreateExcel(string filename, string excelTitle, DataTable dtSrc)
        {
            int excelMaxRow = 1000000;
            int maxLoadRow = 250000;
            int rowCnt = dtSrc.Rows.Count;
            int loadTimes = Convert.ToInt32(Math.Ceiling((decimal)excelMaxRow / (decimal)maxLoadRow));
            DataTable dt;

            if (rowCnt > excelMaxRow)
            {
                Excel.Application excel = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);

                excel.DisplayAlerts = false;
                Utility.Report.ExcelCOM comDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excel);

                int sheetCnt = Convert.ToInt32(Math.Ceiling((decimal)rowCnt / (decimal)excelMaxRow));

                for (int i = 0; i < sheetCnt; i++)
                {
                    Microsoft.Office.Interop.Excel.Worksheet worksheetA = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1];
                    Microsoft.Office.Interop.Excel.Worksheet worksheetB = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 2];
                    worksheetA.Copy(worksheetB);
                    ((Microsoft.Office.Interop.Excel.Worksheet)excel.Sheets[i + 1]).Select();
                    int sheetMaxRow = (i + 1) * excelMaxRow;

                    for (int j = 0; j < loadTimes; j++)
                    {
                        int startRow = (i * excelMaxRow) + (j * maxLoadRow);
                        dt = dtSrc.AsEnumerable().Skip(startRow).Take(maxLoadRow).CopyToDataTable();

                        if (startRow > rowCnt)
                        {
                            break;
                        }

                        comDetail.WriteTable(dt, (j * maxLoadRow) + 2);
                    }
                }

                ((Microsoft.Office.Interop.Excel.Worksheet)excel.Sheets[sheetCnt + 1]).Delete();
                ((Microsoft.Office.Interop.Excel.Worksheet)excel.Sheets[1]).Select();
                this.SaveExcelwithName(excel, excelTitle);
            }
            else
            {
                dt = dtSrc;
                Excel.Application excelDetail = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                Utility.Report.ExcelCOM comDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelDetail)
                {
                    ColumnsAutoFit = true,
                };

                comDetail.WriteTable(dt, 2);
                Excel.Worksheet worksheetScrapDetail = excelDetail.ActiveWorkbook.Worksheets[1];   // 取得工作表
                this.SaveExcelwithName(excelDetail, excelTitle);
            }
        }

        private void SaveExcelwithName(Excel.Application excelapp, string filename)
        {
            string strExcelName = Class.MicrosoftFile.GetName(filename);
            Excel.Workbook workbook = excelapp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelapp.Quit();
            Marshal.ReleaseComObject(excelapp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);
            strExcelName.OpenFile();
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.PrintData[0].Rows.Count);

            if (this.PrintData[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            this.ShowWaitMessage("Starting EXCEL...");
            if (this.comboInspection.Text == "All")
            {
                this.CreateExcel("Quality_R12_Physical.xltx", "R12 Fabric Physical Inspection List", this.PrintData[0]);
                this.CreateExcel("Quality_R12_Weight.xltx", "R12 Fabric Weight Test List", this.PrintData[1]);
                this.CreateExcel("Quality_R12_ShadeBand.xltx", "R12 Fabric Shade Band Test List", this.PrintData[2]);
                this.CreateExcel("Quality_R12_Continuity.xltx", "R12 Fabric Continuilty Test List", this.PrintData[3]);
                this.CreateExcel("Quality_R12_Odor.xltx", "R12 Fabric Odor Test List", this.PrintData[4]);
            }
            else
            {
                switch (this.comboInspection.Text)
                {
                    case "Physical":
                        this.CreateExcel("Quality_R12_Physical.xltx", "R12 Fabric Physical Inspection List", this.PrintData[0]);
                        break;
                    case "Weight":
                        this.CreateExcel("Quality_R12_Weight.xltx", "R12 Fabric Weight Test List", this.PrintData[0]);
                        break;
                    case "Shade Band":
                        this.CreateExcel("Quality_R12_ShadeBand.xltx", "R12 Fabric Shade Band Test List", this.PrintData[0]);
                        break;
                    case "Continuity":
                        this.CreateExcel("Quality_R12_Continuity.xltx", "R12 Fabric Continuilty Test List", this.PrintData[0]);
                        break;
                    case "Odor":
                        this.CreateExcel("Quality_R12_Odor.xltx", "R12 Fabric Odor Test List", this.PrintData[0]);
                        break;
                    default:
                        break;
                }
            }
            this.HideWaitMessage();
            return true;
        }
    }
}
