using System;
using System.Data;
using System.Text;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_History_Print
    /// </summary>
    public partial class P01_History_Print : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DataTable artworkType;
        private DataRow masterData;
        private string status;
        private string custCD;
        private string machineID;
        private int ttlSewingTime;
        private int sewer;
        private int efficiency;

        /// <summary>
        /// P01_History_Print
        /// </summary>
        /// <param name="masterData">MasterData</param>
        /// <param name="status">Status</param>
        /// <param name="custCD">CustCD</param>
        /// <param name="ttlSewingTime">TtlSewingTime</param>
        /// <param name="sewer">Sewer</param>
        public P01_History_Print(DataRow masterData, string status, string custCD, int ttlSewingTime, int sewer)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.status = status;
            this.custCD = custCD;
            this.ttlSewingTime = ttlSewingTime;
            this.sewer = sewer;
        }

        /// <summary>
        /// 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.numEfficiencySetting.Value))
            {
                MyUtility.Msg.WarningBox("Efficiency setting can't empty!!");
                return false;
            }

            this.efficiency = MyUtility.Convert.GetInt(this.numEfficiencySetting.Value);

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                @"select t.ID,t.Phase,t.Version,td.Seq,td.OperationID,td.MachineTypeID,td.Mold,td.Frequency,
td.SMV,td.PcsPerHour,td.Sewer,td.Annotation,o.DescEN
from TimeStudyHistory t WITH (NOLOCK) 
inner join TimeStudyHistory_Detail td WITH (NOLOCK) on t.ID = td.ID
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
where t.StyleID = '{0}'
and t.SeasonID = '{1}'
and t.ComboType = '{2}'
and t.BrandID = '{3}'
and LEFT(t.Phase+REPLICATE(' ',10),10)+' VER-'+t.Version = '{4}'
order by td.Seq", MyUtility.Convert.GetString(this.masterData["StyleID"]),
                MyUtility.Convert.GetString(this.masterData["SeasonID"]),
                MyUtility.Convert.GetString(this.masterData["ComboType"]),
                MyUtility.Convert.GetString(this.masterData["BrandID"]),
                this.status);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            if (this.printData.Rows.Count <= 0)
            {
                return Result.True;
            }

            string id = MyUtility.Convert.GetString(this.printData.Rows[0]["ID"]);
            sqlCmd = string.Format(
                @"select isnull(m.ArtworkTypeID,'') as ArtworkTypeID,sum(td.SMV) as ttlSMV
from TimeStudyHistory_Detail td WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
--LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON m.ID=ATD.MachineTypeID
where td.ID = {0}
group by isnull(m.ArtworkTypeID,'')", id);
            result = DBProxy.Current.Select(null, sqlCmd, out this.artworkType);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            this.machineID = MyUtility.GetValue.Lookup(string.Format(
                @"select CONCAT(b.Machine,', ') from (
select MachineTypeID+'*'+CONVERT(varchar,cnt) as Machine from (
select td.MachineTypeID,COUNT(td.MachineTypeID) as cnt from TimeStudyHistory_Detail td WITH (NOLOCK) where td.ID = {0} and td.MachineTypeID <> '' group by MachineTypeID) a) b
FOR XML PATH('')", id));
            return Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P01_History_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = string.Format("Factory GSD - {0}, V={1}", MyUtility.Convert.GetString(this.printData.Rows[0]["Phase"]), MyUtility.Convert.GetString(this.printData.Rows[0]["Version"]));
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(this.masterData["StyleID"]);
            worksheet.Cells[3, 4] = MyUtility.Convert.GetString(this.masterData["SeasonID"]);
            worksheet.Cells[3, 6] = this.custCD;
            worksheet.Cells[3, 8] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(this.efficiency) + "%";

            // 填內容值
            int intRowsStart = 5;
            object[,] objArray = new object[1, 9];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["Seq"];
                objArray[0, 1] = dr["MachineTypeID"];
                objArray[0, 2] = dr["Mold"];
                objArray[0, 3] = dr["OperationID"];
                objArray[0, 4] = dr["DescEN"];
                objArray[0, 5] = dr["SMV"];
                objArray[0, 6] = dr["PcsPerHour"];
                objArray[0, 7] = dr["Sewer"];
                decimal txt_efficiency = MyUtility.Convert.GetDecimal(this.efficiency) / 100;
                objArray[0, 8] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["PcsPerHour"]) * txt_efficiency, 1);

                worksheet.Range[string.Format("A{0}:I{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            intRowsStart++;
            worksheet.Cells[intRowsStart, 1] = "Machine:";
            worksheet.Range[string.Format("B{0}:I{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("B{0}:I{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 2] = this.machineID.Length < 3 ? this.machineID.ToString() : this.machineID.Substring(0, this.machineID.Length - 2);
            worksheet.Range[string.Format("A{0}:I{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 3; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A{0}:I{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Total Sewing Time/Pc:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Convert.GetString(this.ttlSewingTime);
            worksheet.Cells[intRowsStart, 4] = "Sec.";
            worksheet.Cells[intRowsStart, 5] = "Prepared by:";
            worksheet.Range[string.Format("E{0}:E{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            worksheet.Range[string.Format("F{0}:I{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:D{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:D{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            StringBuilder artwork = new StringBuilder();
            foreach (DataRow dr in this.artworkType.Rows)
            {
                artwork.Append(string.Format("{0}: {1} Sec\r\n", Convert.ToString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["ttlSMV"]), 0))));
            }

            worksheet.Cells[intRowsStart, 1] = artwork.ToString();

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Number of Sewer:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Convert.GetString(this.sewer);
            worksheet.Cells[intRowsStart, 4] = "Sewer";
            worksheet.Cells[intRowsStart, 5] = "Approved by:";
            worksheet.Range[string.Format("E{0}:E{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            worksheet.Range[string.Format("F{0}:I{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "100% Output/Hr:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.ttlSewingTime) * MyUtility.Convert.GetDecimal(this.sewer) * this.efficiency) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.ttlSewingTime) * MyUtility.Convert.GetDecimal(this.sewer), 0);
            worksheet.Cells[intRowsStart, 4] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = string.Format("{0}%", MyUtility.Convert.GetString(this.efficiency));
            worksheet.Cells[intRowsStart, 3] = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.ttlSewingTime) * MyUtility.Convert.GetDecimal(this.sewer) * this.efficiency) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.ttlSewingTime) * MyUtility.Convert.GetDecimal(this.sewer) * this.efficiency / 100, 0);
            worksheet.Cells[intRowsStart, 4] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "PCS/HR/Sewer:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.ttlSewingTime)) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.ttlSewingTime), 2);
            worksheet.Cells[intRowsStart, 4] = "Pcs";
            worksheet.Cells[intRowsStart, 5] = "Noted by:";
            worksheet.Range[string.Format("E{0}:E{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            worksheet.Range[string.Format("F{0}:I{0}", intRowsStart)].Merge(Type.Missing);

            excel.Cells.EntireRow.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_P01_History_Print");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
