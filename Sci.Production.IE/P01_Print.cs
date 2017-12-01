using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_Print
    /// </summary>
    public partial class P01_Print : Sci.Win.Tems.PrintForm
    {
        private DataRow masterData;
        private string artworktype;
        private string custcdID;
        private string machineID;
        private decimal efficiency;
        private DataTable printData;
        private DataTable artworkType;

        /// <summary>
        /// P01_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P01_Print(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            DataTable artworkType;
            string sqlCmd = string.Format(
                @"
Select distinct ATD.ArtworkTypeID 
from MachineType MT WITH (NOLOCK) , TimeStudy_Detail WITH (NOLOCK) ,Artworktype_Detail ATD WITH (NOLOCK)
where MT.ID = TimeStudy_Detail.MachineTypeID 
and MT.ID=ATD.MachineTypeID
and TimeStudy_Detail.ID = {0}",
                MyUtility.Convert.GetString(this.masterData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkType);
            MyUtility.Tool.SetupCombox(this.comboArtworkType, 1, artworkType);
            this.comboArtworkType.SelectedIndex = -1;
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
            this.artworktype = this.comboArtworkType.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.custcdID = MyUtility.GetValue.Lookup(string.Format("select CdCodeID from Style WITH (NOLOCK) where ID = '{0}' and SeasonID = '{1}' and BrandID = '{2}'", MyUtility.Convert.GetString(this.masterData["StyleID"]), MyUtility.Convert.GetString(this.masterData["SeasonID"]), MyUtility.Convert.GetString(this.masterData["BrandID"])));
            this.machineID = MyUtility.GetValue.Lookup(string.Format(
                @"select CONCAT(b.Machine,', ') from (
select MachineTypeID+'*'+CONVERT(varchar,cnt) as Machine from (
select td.MachineTypeID,COUNT(td.MachineTypeID) as cnt from TimeStudy_Detail td WITH (NOLOCK) left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID where td.ID = {0} and td.MachineTypeID <> ''{1} group by MachineTypeID) a) b
FOR XML PATH('')", MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(this.artworktype) ? string.Empty : " and m.ArtworkTypeID = '" + this.artworktype + "'"));
            string sqlCmd = string.Format(
                @"select td.Seq,td.OperationID,td.MachineTypeID,td.Mold,td.Frequency,td.SMV,td.PcsPerHour,td.Sewer,
td.Annotation,o.DescEN
from TimeStudy_Detail td WITH (NOLOCK) 
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON m.ID=ATD.MachineTypeID
where td.ID = {0}{1}
order by td.Seq", MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(this.artworktype) ? string.Empty : string.Format(" and ATD.ArtworkTypeID = '{0}'", this.artworktype));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(
                @"select isnull(m.ArtworkTypeID,'') as ArtworkTypeID,sum(td.SMV) as ttlSMV
from TimeStudy_Detail td WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON m.ID=ATD.MachineTypeID
where td.ID = {0}{1}
group by isnull(m.ArtworkTypeID,'')", MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(this.artworktype) ? string.Empty : string.Format(" and ATD.ArtworkTypeID = '{0}'", this.artworktype));
            result = DBProxy.Current.Select(null, sqlCmd, out this.artworkType);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

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

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P01_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = string.Format("Factory GSD - {0}", MyUtility.Convert.GetString(this.masterData["Phase"]));
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(this.masterData["StyleID"]);
            worksheet.Cells[3, 5] = MyUtility.Convert.GetString(this.masterData["SeasonID"]);
            worksheet.Cells[3, 7] = this.custcdID;
            worksheet.Cells[3, 11] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 12] = MyUtility.Convert.GetString(this.efficiency) + "%";

            // 填內容值
            int intRowsStart = 5;
            object[,] objArray = new object[1, 12];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = intRowsStart - 4;
                objArray[0, 1] = dr["Seq"];
                objArray[0, 2] = dr["OperationID"];
                objArray[0, 3] = dr["MachineTypeID"];
                objArray[0, 4] = dr["Mold"];
                objArray[0, 5] = dr["DescEN"];
                objArray[0, 6] = dr["Annotation"];
                objArray[0, 7] = dr["Frequency"];
                objArray[0, 8] = dr["SMV"];
                objArray[0, 9] = dr["PcsPerHour"];
                objArray[0, 10] = dr["Sewer"];
                objArray[0, 11] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["PcsPerHour"]) * (this.efficiency / 100), 1);

                worksheet.Range[string.Format("A{0}:L{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Machine:";
            worksheet.Range[string.Format("C{0}:L{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("C{0}:L{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            // 避免machineID為空所產生的錯誤
            if (this.machineID.Length > 2)
            {
                worksheet.Cells[intRowsStart, 3] = this.machineID.Substring(0, this.machineID.Length - 2);
            }

            worksheet.Range[string.Format("A{0}:L{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 3; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A{0}:L{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Total Sewing Time/Pc:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(this.masterData["TotalSewingTime"]);
            worksheet.Cells[intRowsStart, 5] = "Sec.";
            worksheet.Cells[intRowsStart, 7] = "Prepared by:";
            worksheet.Range[string.Format("H{0}:L{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:F{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:F{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            StringBuilder artwork = new StringBuilder();
            foreach (DataRow dr in this.artworkType.Rows)
            {
                artwork.Append(string.Format("{0}: {1} Sec\r\n", Convert.ToString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["ttlSMV"]), 0))));
            }

            worksheet.Cells[intRowsStart, 1] = artwork.ToString();

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Number of Sewer:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(this.masterData["NumberSewer"]);
            worksheet.Cells[intRowsStart, 5] = "Sewer";
            worksheet.Cells[intRowsStart, 7] = "Approved by:";
            worksheet.Range[string.Format("H{0}:L{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "100% Output/Hr:";

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]), 0);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = string.Format("{0}%", MyUtility.Convert.GetString(this.efficiency));

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) * this.efficiency / 100, 0);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "PCS/HR/Sewer:";

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]), 2);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";
            worksheet.Cells[intRowsStart, 7] = "Noted by:";
            worksheet.Range[string.Format("H{0}:L{0}", intRowsStart)].Merge(Type.Missing);

            excel.Cells.EntireRow.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_P01_Print");
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
