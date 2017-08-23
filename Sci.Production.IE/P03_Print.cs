﻿using System;
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
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        string display, contentType, language;
        DataTable machineInv, printData, ttlCycleTime, operationCode;
        decimal styleCPU;
        public P03_Print(DataRow MasterData,decimal StyleCPU)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboLanguage, 1, 1, "English,Chinese,Cambodia,Vietnam");
            comboLanguage.Text = "English";
            masterData = MasterData;
            styleCPU = StyleCPU;
            radioU.Checked = true;
            radioDescription.Checked = true;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            display = radioU.Checked ? "U" : "Z";
            contentType = radioDescription.Checked ? "D" : "A";
            language = comboLanguage.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(@"with Attachment
as
(
 select distinct MachineTypeID,(select CONCAT(MoldID,',') from LineMapping_Detail WITH (NOLOCK) where ID = 8257 and MoldID <> '' and MachineTypeID = ld.MachineTypeID for xml path('')) as Attach
 from LineMapping_Detail ld WITH (NOLOCK) 
 where ID = {0} and MoldID <> ''
)
select b.MachineTypeID,b.ctn,iif(a.Attach is null,'',SUBSTRING(a.Attach,1,len(a.Attach)-1)) as Attach 
from (select a.MachineTypeID, count(a.MachineTypeID) as ctn 
	  from
	  (select distinct no,MachineTypeID 
	   from LineMapping_Detail WITH (NOLOCK) 
	   where ID = {0}) a
	  group by a.MachineTypeID) b
left join Attachment a on b.MachineTypeID = a.MachineTypeID", MyUtility.Convert.GetString(masterData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out machineInv);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query machine data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(@"select ld.No,ld.TotalCycle,ld.TotalGSD,ld.Cycle,ld.GroupKey,ld.MachineTypeID,isnull(e.Name,'') as EmployeeName
from LineMapping_Detail ld WITH (NOLOCK) 
left join Employee e WITH (NOLOCK) on e.ID = ld.EmployeeID
where ld.ID = {0}
order by ld.No,ld.GroupKey", MyUtility.Convert.GetString(masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(@"select distinct No,TotalCycle,{1} as TaktTime
from LineMapping_Detail WITH (NOLOCK) 
where ID = {0}
order by No", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Convert.GetString(masterData["TaktTime"]));
            result = DBProxy.Current.Select(null, sqlCmd, out ttlCycleTime);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(@"select a.*,isnull(o.DescEN,'') as DescEN,isnull(od.DescCHS,'') as DescCHS,isnull(od.DescKH,'') as DescKH,isnull(od.DescVI,'') as DescVI
from (select GroupKey,OperationID,Annotation,max(GSD) as GSD,MachineTypeID
	  from LineMapping_Detail WITH (NOLOCK) 
	  where ID = {0}
	  group by GroupKey,OperationID,Annotation,MachineTypeID) a
left join Operation o WITH (NOLOCK) on o.ID = a.OperationID
left join OperationDesc od WITH (NOLOCK) on od.ID = a.OperationID
order by a.GroupKey", MyUtility.Convert.GetString(masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out operationCode);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query operation code data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + (display == "U" ? "\\IE_P03_Print_U.xltx" : "\\IE_P03_Print_Z.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填Operation
            int intRowsStart = 2;
            object[,] objArray = new object[1, 4];
            foreach (DataRow dr in operationCode.Rows)
            {
                objArray[0, 0] = dr["GroupKey"];
                objArray[0, 1] = contentType == "A" ? dr["Annotation"] : language == "English" ? dr["DescEN"] : language == "Chinese" ? dr["DescCHS"] : language == "Cambodia" ? dr["DescKH"] : dr["DescVI"];
                objArray[0, 2] = dr["GSD"];
                objArray[0, 3] = dr["MachineTypeID"];
                worksheet.Range[String.Format("A{0}:D{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            //填Cycle Time
            worksheet = excel.ActiveWorkbook.Worksheets[3];
            intRowsStart = 2;
            objArray = new object[1, 3];
            foreach (DataRow dr in ttlCycleTime.Rows)
            {
                objArray[0, 0] = dr["No"];
                objArray[0, 1] = dr["TotalCycle"];
                objArray[0, 2] = dr["TaktTime"];
                worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            #region 新增長條圖
            //新增長條圖
            Microsoft.Office.Interop.Excel.Worksheet chartData = excel.ActiveWorkbook.Worksheets[3];
            worksheet = excel.ActiveWorkbook.Worksheets[2];
            Microsoft.Office.Interop.Excel.Range chartRange;
            object misValue = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.ChartObjects xlsCharts = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts.Add(650, 80, 700, 350);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
            chartRange = chartData.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            chartPage.SetSourceData(chartRange, misValue);

            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;
            
            //新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series1 = seriesCollection.NewSeries();
            series1.Values = chartData.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            series1.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            series1.Name = "Takt time";
            series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

            //更改圖表版面配置 && 填入圖表標題 & 座標軸標題
            chartPage.ApplyLayout(9);
            chartPage.ChartTitle.Select();
            chartPage.ChartTitle.Text = "Line Balancing Graph";
            Microsoft.Office.Interop.Excel.Axis z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            z.AxisTitle.Text = "Cycle Time (in secs)";
            z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            z.AxisTitle.Text = "Operator No.";

            //新增資料標籤
            chartPage.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false, true);
            //折線圖的資料標籤不顯示
            series1.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

            //隱藏Sheet
            chartData.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            #endregion

            //填入printed date and print by
            worksheet.Cells[36, 16] = Sci.Env.User.UserName;
            worksheet.Cells[33, 16] = DateTime.Today.ToShortDateString();

            //填Line Mapping
            worksheet.Cells[7, 5] = MyUtility.Convert.GetString(masterData["Version"]);
            worksheet.Cells[9, 5] = MyUtility.Convert.GetString(masterData["FactoryID"]);
            worksheet.Cells[11, 5] = MyUtility.Convert.GetString(masterData["SewingLineID"]);
            worksheet.Cells[15, 5] = MyUtility.Convert.GetString(masterData["StyleID"]);
            worksheet.Cells[17, 5] = styleCPU;
            worksheet.Cells[19, 5] = MyUtility.Convert.GetString(masterData["Workhour"]);
            worksheet.Cells[21, 5] = MyUtility.Convert.GetString(masterData["CurrentOperators"]);
            worksheet.Cells[23, 5] = MyUtility.Convert.GetString(masterData["StandardOutput"]);
            worksheet.Cells[25, 5] = MyUtility.Convert.GetString(masterData["DailyDemand"]);
            worksheet.Cells[27, 5] = MyUtility.Convert.GetString(masterData["TaktTime"]);
            worksheet.Cells[29, 5] = MyUtility.Math.Round(3600m/MyUtility.Convert.GetDecimal(masterData["HighestCycle"]),2);
            worksheet.Cells[31, 5] = MyUtility.Convert.GetString(masterData["TotalCycle"]);
            worksheet.Cells[33, 5] = MyUtility.Convert.GetString(masterData["TotalGSD"]);
            worksheet.Cells[35, 5] = MyUtility.Check.Empty(masterData["TotalGSD"]) ? 0 : (MyUtility.Convert.GetDecimal(masterData["TotalGSD"]) - MyUtility.Convert.GetDecimal(masterData["TotalCycle"])) / MyUtility.Convert.GetDecimal(masterData["TotalGSD"]);
            worksheet.Cells[37, 5] = MyUtility.Check.Empty(masterData["HighestCycle"]) || MyUtility.Check.Empty(masterData["CurrentOperators"]) ? 0 : MyUtility.Convert.GetDecimal(masterData["TotalCycle"]) / MyUtility.Convert.GetDecimal(masterData["HighestCycle"]) / MyUtility.Convert.GetDecimal(masterData["CurrentOperators"]);
            worksheet.Cells[39, 5] = MyUtility.Check.Empty(masterData["TaktTime"]) || MyUtility.Check.Empty(masterData["CurrentOperators"]) ? 0 : MyUtility.Convert.GetDecimal(masterData["TotalCycle"]) / MyUtility.Convert.GetDecimal(masterData["TaktTime"]) / MyUtility.Convert.GetDecimal(masterData["CurrentOperators"]);
            worksheet.Cells[41, 5] = MyUtility.Check.Empty(masterData["HighestCycle"]) || MyUtility.Check.Empty(masterData["CurrentOperators"]) ? 0 : MyUtility.Convert.GetDecimal(masterData["TotalGSD"]) / MyUtility.Convert.GetDecimal(masterData["HighestCycle"]) / MyUtility.Convert.GetDecimal(masterData["CurrentOperators"]);
            worksheet.Cells[43, 5] = MyUtility.Check.Empty(masterData["HighestCycle"]) || MyUtility.Check.Empty(masterData["CurrentOperators"]) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(masterData["HighestCycle"]), 2) * styleCPU / MyUtility.Convert.GetDecimal(masterData["CurrentOperators"]);

            //填MACHINE INVENTORY
            intRowsStart = 11;
            objArray = new object[1, 3];
            int i = 0;
            foreach (DataRow dr in machineInv.Rows)
            {
                i++;
                if (i >= 17)
                {
                    worksheet.Cells[43, 9] = "Attention. M/C TYPE is more than 16, pls check all machine.";
                    break;
                }
                else
                {
                    objArray[0, 0] = dr["MachineTypeID"];
                    objArray[0, 1] = dr["ctn"];
                    objArray[0, 2] = dr["Attach"];
                    worksheet.Range[String.Format("G{0}:I{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart += 2;
                }
            }

            //預設站數為4站，當超過4站就要新增
            decimal reccount = ttlCycleTime.Rows.Count;
            int j = 3; //資料組數，預設為3
            if (reccount > 4)
            {
                //選取要被複製的資料
                Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A64:A76").EntireRow;
                for (j = 3; j <= Convert.ToInt32(Math.Ceiling(reccount / 2)); j++)
                {
                    //選擇要被貼上的位置
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString(51 + (j - 1) *13)), Type.Missing).EntireRow;
                    //貼上
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                }
            }

            StringBuilder machine = new StringBuilder();
            int no = j * 13 + 39; //No格子上的位置Excel Y軸
            int station = 20; //紀錄寫入No值的格數     
            if (display == "U") //U字型列印
            {
                #region U字型列印
                bool positive = true; //紀錄是要填在左邊或右邊的格子
                foreach (DataRow dr in ttlCycleTime.Rows)
                {
                    if (positive)
                    {
                        no = no - 13;
                        worksheet.Cells[no, station] = MyUtility.Convert.GetString(dr["No"]);
                        machine.Clear();
                        int descCount = 1, machineCount = 1;
                        DataRow[] sdr = printData.Select(string.Format("No = '{0}'", MyUtility.Convert.GetString(dr["No"])), "No,GroupKey");
                        foreach (DataRow ddr in sdr)
                        {
                            if (descCount == 1)
                            {
                                worksheet.Cells[no + 2, station + 1] = MyUtility.Convert.GetString(ddr["TotalGSD"]);
                                worksheet.Cells[no + 7, station + 1] = MyUtility.Convert.GetString(ddr["TotalCycle"]);
                                worksheet.Cells[no, station - 4] = MyUtility.Convert.GetString(ddr["EmployeeName"]);
                                worksheet.Cells[no + 2, station - 5] = MyUtility.Check.Empty(ddr["TotalCycle"]) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(ddr["TotalCycle"]),0);
                            }
                            worksheet.Cells[no + 1 + descCount, station] = MyUtility.Convert.GetString(ddr["Cycle"]);
                            worksheet.Cells[no + 1 + descCount, station - 1] = MyUtility.Convert.GetString(ddr["GroupKey"]);

                            if (!MyUtility.Check.Empty(ddr["MachineTypeID"]) && !machine.ToString().Contains(MyUtility.Convert.GetString(ddr["MachineTypeID"])))
                            {
                                machine.Append(string.Format("{0},", MyUtility.Convert.GetString(ddr["MachineTypeID"])));
                                switch (machineCount)
                                {
                                    case 1:
                                        worksheet.Cells[no + 2, station - 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 2:
                                        worksheet.Cells[no + 7, station - 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 3:
                                        worksheet.Cells[no + 2, station - 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 4:
                                        worksheet.Cells[no + 7, station - 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    default:
                                        break;
                                }
                                machineCount++;
                            }
                            descCount++;
                            if (descCount > 11)
                            {
                                break;
                            }
                        }

                        if (no == 52)
                        {
                            positive = false;
                            no = 39;
                            station = 3;
                        }
                    }
                    else
                    {
                        no = no + 13;
                        worksheet.Cells[no, station] = MyUtility.Convert.GetString(dr["No"]);
                        machine.Clear();
                        int descCount = 1, machineCount = 1;
                        DataRow[] sdr = printData.Select(string.Format("No = '{0}'", MyUtility.Convert.GetString(dr["No"])), "No,GroupKey");
                        foreach (DataRow ddr in sdr)
                        {
                            if (descCount == 1)
                            {
                                worksheet.Cells[no + 2, station - 1] = MyUtility.Convert.GetString(ddr["TotalGSD"]);
                                worksheet.Cells[no + 7, station - 1] = MyUtility.Convert.GetString(ddr["TotalCycle"]);
                                worksheet.Cells[no, station + 1] = MyUtility.Convert.GetString(ddr["EmployeeName"]);
                                worksheet.Cells[no + 2, station + 5] = MyUtility.Check.Empty(ddr["TotalCycle"]) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(ddr["TotalCycle"]), 0);
                            }
                            worksheet.Cells[no + 1 + descCount, station] = MyUtility.Convert.GetString(ddr["Cycle"]);
                            worksheet.Cells[no + 1 + descCount, station + 1] = MyUtility.Convert.GetString(ddr["GroupKey"]);

                            if (!MyUtility.Check.Empty(ddr["MachineTypeID"]) && !machine.ToString().Contains(MyUtility.Convert.GetString(ddr["MachineTypeID"])))
                            {
                                machine.Append(string.Format("{0},", MyUtility.Convert.GetString(ddr["MachineTypeID"])));
                                switch (machineCount)
                                {
                                    case 1:
                                        worksheet.Cells[no + 2, station + 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 2:
                                        worksheet.Cells[no + 7, station + 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 3:
                                        worksheet.Cells[no + 2, station + 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 4:
                                        worksheet.Cells[no + 7, station + 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    default:
                                        break;
                                }
                                machineCount++;
                            }
                            descCount++;
                            if (descCount > 11)
                            {
                                break;
                            }
                        }
                    }
                }
                #endregion
            }
            else  //Z字型列印
            {
                #region Z字型列印
                bool firstRecord = true, rightDirection = false;
                int printRecord = 2;
                foreach (DataRow dr in ttlCycleTime.Rows)
                {
                    if (printRecord == 2)
                    {
                        no = no - 13;
                    }
                    if (firstRecord || rightDirection)
                    {
                        station = 20;
                        worksheet.Cells[no, station] = MyUtility.Convert.GetString(dr["No"]);
                        machine.Clear();
                        int descCount = 1, machineCount = 1;
                        DataRow[] sdr = printData.Select(string.Format("No = '{0}'", MyUtility.Convert.GetString(dr["No"])), "No,GroupKey");
                        foreach (DataRow ddr in sdr)
                        {
                            if (descCount == 1)
                            {
                                worksheet.Cells[no + 2, station + 1] = MyUtility.Convert.GetString(ddr["TotalGSD"]);
                                worksheet.Cells[no + 7, station + 1] = MyUtility.Convert.GetString(ddr["TotalCycle"]);
                                worksheet.Cells[no, station - 4] = MyUtility.Convert.GetString(ddr["EmployeeName"]);
                                worksheet.Cells[no + 2, station - 5] = MyUtility.Check.Empty(ddr["TotalCycle"]) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(ddr["TotalCycle"]), 0);
                            }
                            worksheet.Cells[no + 1 + descCount, station] = MyUtility.Convert.GetString(ddr["Cycle"]);
                            worksheet.Cells[no + 1 + descCount, station - 1] = MyUtility.Convert.GetString(ddr["GroupKey"]);

                            if (!MyUtility.Check.Empty(ddr["MachineTypeID"]) && !machine.ToString().Contains(MyUtility.Convert.GetString(ddr["MachineTypeID"])))
                            {
                                machine.Append(string.Format("{0},", MyUtility.Convert.GetString(ddr["MachineTypeID"])));
                                switch (machineCount)
                                {
                                    case 1:
                                        worksheet.Cells[no + 2, station - 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 2:
                                        worksheet.Cells[no + 7, station - 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 3:
                                        worksheet.Cells[no + 2, station - 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 4:
                                        worksheet.Cells[no + 7, station - 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    default:
                                        break;
                                }
                                machineCount++;
                            }
                            descCount++;
                            if (descCount > 11)
                            {
                                break;
                            }
                        }
                        printRecord++;
                        if (printRecord > 2)
                        {
                            rightDirection = false;
                            printRecord = 1;
                        }
                    }
                    else
                    {
                        station = 3;
                        worksheet.Cells[no, station] = MyUtility.Convert.GetString(dr["No"]);
                        machine.Clear();
                        int descCount = 1, machineCount = 1;
                        DataRow[] sdr = printData.Select(string.Format("No = '{0}'", MyUtility.Convert.GetString(dr["No"])), "No,GroupKey");
                        foreach (DataRow ddr in sdr)
                        {
                            if (descCount == 1)
                            {
                                worksheet.Cells[no + 2, station - 1] = MyUtility.Convert.GetString(ddr["TotalGSD"]);
                                worksheet.Cells[no + 7, station - 1] = MyUtility.Convert.GetString(ddr["TotalCycle"]);
                                worksheet.Cells[no, station + 1] = MyUtility.Convert.GetString(ddr["EmployeeName"]);
                                worksheet.Cells[no + 2, station + 5] = MyUtility.Check.Empty(ddr["TotalCycle"]) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(ddr["TotalCycle"]), 0);
                            }
                            worksheet.Cells[no + 1 + descCount, station] = MyUtility.Convert.GetString(ddr["Cycle"]);
                            worksheet.Cells[no + 1 + descCount, station + 1] = MyUtility.Convert.GetString(ddr["GroupKey"]);

                            if (!MyUtility.Check.Empty(ddr["MachineTypeID"]) && !machine.ToString().Contains(MyUtility.Convert.GetString(ddr["MachineTypeID"])))
                            {
                                machine.Append(string.Format("{0},", MyUtility.Convert.GetString(ddr["MachineTypeID"])));
                                switch (machineCount)
                                {
                                    case 1:
                                        worksheet.Cells[no + 2, station + 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 2:
                                        worksheet.Cells[no + 7, station + 6] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 3:
                                        worksheet.Cells[no + 2, station + 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    case 4:
                                        worksheet.Cells[no + 7, station + 7] = MyUtility.Convert.GetString(ddr["MachineTypeID"]);
                                        break;
                                    default:
                                        break;
                                }
                                machineCount++;
                            }
                            descCount++;
                            if (descCount > 11)
                            {
                                break;
                            }
                        }
                        printRecord++;
                        if (printRecord > 2)
                        {
                            rightDirection = true;
                            printRecord = 1;
                        }
                    }

                    if (firstRecord)
                    {
                        firstRecord = false;
                    }
                }
                #endregion
            }
            //寫此行目的是要將Excel畫面上顯示Copy給取消
            excel.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_P03_Print");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion 
            return true;
        }
    }
}
