using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Win;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// P10_ToExcel
    /// </summary>
    public partial class P10_ToExcel : Win.Tems.PrintForm
    {
        private DataTable dtRight;
        private DataTable dtLeft;

        /// <summary>
        /// P10_ToExcel
        /// </summary>
        /// <param name="objStartDate">StartDate</param>
        /// <param name="objEndDate">EndDate</param>
        /// <param name="dtRight">dtRight</param>
        /// <param name="dtLeft">dtLeft</param>
        public P10_ToExcel(object objStartDate, object objEndDate, DataTable dtRight, DataTable dtLeft)
        {
            this.InitializeComponent();
            this.dateRange.Value1 = Convert.ToDateTime(objStartDate);
            this.dateRange.Value2 = Convert.ToDateTime(objEndDate);
            this.dtRight = dtRight;
            this.dtLeft = dtLeft;
            this.grid.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Set Data
            DataTable dtGrid;

            string strSQL = @"
select [Group]
       , CPU = null
from #tmp
group by [Group]";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtLeft, null, strSQL, out dtGrid);

            if (result == false)
            {
                MyUtility.Msg.InfoBox(result.ToString());
            }
            else
            {
                this.listControlBindingSourceGroup.DataSource = dtGrid;
            }

            strSQL = @"
select *
from (
    select [Column] = 'Efficiency'
           , Data = null
           , num = 1

    union
    select [Column] = 'Manpower'
           , Data = null
           , num = 2

    union
    select [Column] = 'Daily Working hour'
           , Data = null
           , num = 3
) tmp
order by num";

            result = DBProxy.Current.Select(null, strSQL, out dtGrid);

            if (result == false)
            {
                MyUtility.Msg.InfoBox(result.ToString());
            }
            else
            {
                this.listControlBindingSourceCapacity.DataSource = dtGrid;
            }
            #endregion
            this.GroupFillRateCheck();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.radioBtnCapacityFillRate.Checked == true)
            {
                if (((DataTable)this.listControlBindingSourceCapacity.DataSource).AsEnumerable().Any(row => row["Data"].EqualDecimal(0)
                                                                                                            || row["Data"].Empty()))
                {
                    MyUtility.Msg.WarningBox("Value can not be empty.");
                    return false;
                }
            }
            else
            {
                if (((DataTable)this.listControlBindingSourceGroup.DataSource).AsEnumerable().Any(row => row["CPU"].EqualDecimal(0)
                                                                                                            || row["CPU"].Empty()))
                {
                    MyUtility.Msg.WarningBox("Std. CPU can not be empty.");
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return Result.True;
        }

        private void GroupFillRateCheck()
        {
            #region Set Grid Column
            this.grid.Columns.Clear();
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Group", header: "Group", iseditingreadonly: true)
                .Numeric("CPU", header: "Std. CPU");
            #endregion

            #region Set Data
            this.grid.DataSource = this.listControlBindingSourceGroup.DataSource;
            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            this.grid.Columns["CPU"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        private void CapacityAndFillRateCheck()
        {
            #region Set Grid Column
            this.grid.Columns.Clear();
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Column", header: "Column", iseditingreadonly: true)
                .Numeric("Data", header: "Value");
            #endregion

            #region Set Data
            this.grid.DataSource = this.listControlBindingSourceCapacity.DataSource;
            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            this.grid.Columns["Data"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        private void RadioBtnCapacityFillRate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioBtnCapacityFillRate.Checked == true)
            {
                this.CapacityAndFillRateCheck();
            }
            else
            {
                this.GroupFillRateCheck();
            }
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");

            if (this.radioBtnCapacityFillRate.Checked == true)
            {
                this.CapacityAndFillRateToExcel();
            }
            else
            {
                this.GroupFillRateToExcel();
            }

            this.HideWaitMessage();
            return true;
        }

        /// <summary>
        /// GroupFillRateToExcel
        /// </summary>
        protected void GroupFillRateToExcel()
        {
            DualResult result;

            Excel._Application myExcel = null;
            Excel._Workbook myBook = null;
            Excel._Worksheet mySheet = null;

            try
            {
                myExcel = new Excel.Application();
                myExcel.DisplayAlerts = false;
                myBook = myExcel.Workbooks.Add(true);
                mySheet = (Excel._Worksheet)myBook.Worksheets[1];
                myExcel.Visible = false;
                DataTable dtTmpRight = this.dtRight.AsEnumerable().CopyToDataTable();

                for (int i = dtTmpRight.Columns.Count - 1; i >= 3; i--)
                {
                    if (this.dateRange.Value1 > Convert.ToDateTime(dtTmpRight.Columns[i].ColumnName)
                        || Convert.ToDateTime(dtTmpRight.Columns[i].ColumnName) > this.dateRange.Value2)
                    {
                        dtTmpRight.Columns.Remove(dtTmpRight.Columns[i].ColumnName);
                    }
                }

                if (dtTmpRight.Columns.Count > 3)
                {
                    foreach (DataRow dr in ((DataTable)this.listControlBindingSourceGroup.DataSource).Rows)
                    {
                        int intWorkSheetCount = myExcel.Worksheets.Count;
                        mySheet = (Excel.Worksheet)myExcel.Worksheets.Add(Type.Missing, myExcel.Worksheets[intWorkSheetCount]);
                        mySheet.Name = MyUtility.Check.Empty(dr["Group"]) ? " " : dr["Group"].ToString();

                        #region 依照群組 組成 CPU
                        DataTable dtGroupLeft = this.dtLeft.AsEnumerable().Where(row => row["Group"].EqualString(dr["Group"])).CopyToDataTable();
                        DataTable dtGroupRight;
                        List<string> listUkey = new List<string>();

                        foreach (DataRow drGroupLeft in dtGroupLeft.Rows)
                        {
                            listUkey.Add($"'{drGroupLeft["Ukey"].ToString()}'");
                        }

                        string strQueryDtRightByUkey = $@"
select *
from (
    select *
    from #tmp
    where OrderID = '02'

    union
    select *
    from #tmp
    where ukey in ({listUkey.JoinToString(",")})
) tmp";

                        result = MyUtility.Tool.ProcessWithDatatable(dtTmpRight, null, strQueryDtRightByUkey, out dtGroupRight);
                        #endregion

                        for (int i = 3; i < dtGroupRight.Columns.Count; i++)
                        {
                            this.ComputDailyCPU(i, dtGroupRight, dtGroupLeft);
                        }

                        dtGroupRight.Columns.Remove("ID");
                        dtGroupRight.Columns.Remove("Ukey");

                        dtGroupRight.Rows[0][0] = "Daily CPU";
                        dtGroupRight.Columns[0].ColumnName = "Output Date";

                        DataTable dtToExcel = dtGroupRight.AsEnumerable().Where(row => row["Output Date"].EqualString("Daily CPU")).CopyToDataTable();

                        for (int i = 0; i < dtToExcel.Columns.Count; i++)
                        {
                            ((Excel.Range)mySheet.Cells[1, i + 1]).Insert(Excel.XlInsertShiftDirection.xlShiftDown);

                            if (i > 0)
                            {
                                mySheet.Cells[1, i + 1] = $"'{Convert.ToDateTime(dtToExcel.Columns[i].ColumnName).ToString("MM/dd")}";
                                mySheet.Cells[2, i + 1] = dtToExcel.Rows[0][i];
                                mySheet.Cells[3, i + 1] = dr["CPU"];
                            }
                            else
                            {
                                mySheet.Cells[1, i + 1] = dtToExcel.Columns[i].ColumnName;
                                mySheet.Cells[2, i + 1] = "Daily CPU";
                                mySheet.Cells[3, i + 1] = "Std. CPU";
                            }
                        }

                        mySheet.Rows.AutoFit();
                        mySheet.Columns.AutoFit();

                        // 在工作簿 新增一張 統計圖表，單獨放在一個分頁裡面
                        myBook.Charts.Add(Type.Missing, Type.Missing, 1, Type.Missing);

                        // 選擇 統計圖表 的 圖表種類
                        myBook.ActiveChart.ChartType = Excel.XlChartType.xlColumnStacked;

                        for (int i = ((Excel.SeriesCollection)myBook.ActiveChart.SeriesCollection()).Count; i > 0; i--)
                        {
                            ((Excel.Series)myBook.ActiveChart.SeriesCollection(i)).Delete();
                        }

                        ((Excel.SeriesCollection)myBook.ActiveChart.SeriesCollection()).NewSeries();
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(1)).Name = "Daily CPU";
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(1)).Values = mySheet.get_Range("B2", $"{this.GetExcelColumn(dtToExcel.Columns.Count)}2");
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(1)).XValues = mySheet.get_Range("B1", $"{this.GetExcelColumn(dtToExcel.Columns.Count)}1");

                        ((Excel.SeriesCollection)myBook.ActiveChart.SeriesCollection()).NewSeries();
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).Name = "Std. CPU";
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).Values = mySheet.get_Range("B3", $"{this.GetExcelColumn(dtToExcel.Columns.Count)}3");
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).XValues = mySheet.get_Range("B1", $"{this.GetExcelColumn(dtToExcel.Columns.Count)}1");
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).ChartType = Excel.XlChartType.xlLine;

                        // 圖表 標題
                        myBook.ActiveChart.HasTitle = true;
                        myBook.ActiveChart.ChartTitle.Text = dr["Group"].ToString();

                        mySheet = (Excel._Worksheet)myBook.Worksheets[1];
                        myBook.ActiveChart.Location(Excel.XlChartLocation.xlLocationAsObject, mySheet.Name);

                        // 圖表 位置與大小
                        mySheet.Shapes.Item(mySheet.Shapes.Count).Height = 300;
                        mySheet.Shapes.Item(mySheet.Shapes.Count).Width = (dtToExcel.Columns.Count * 10) + 200;
                        mySheet.Shapes.Item(mySheet.Shapes.Count).Top = (mySheet.Shapes.Count - 1) * 300;
                        mySheet.Shapes.Item(mySheet.Shapes.Count).Left = 1;
                    }

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("P10_ToExcel");
                    myBook.SaveAs(strExcelName);
                    myBook.Close();
                    myExcel.Quit();
                    strExcelName.OpenFile();
                    #endregion
                }
                else
                {
                    MyUtility.Msg.InfoBox("Data not found.");
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.WarningBox(ex.ToString());
                myExcel.Visible = true;
            }
            finally
            {
                // 把執行的Excel資源釋放
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
                myExcel = null;
                myBook = null;
                mySheet = null;
            }
        }

        /// <summary>
        /// CapacityAndFillRateToExcel
        /// </summary>
        protected void CapacityAndFillRateToExcel()
        {
            Excel._Application myExcel = null;
            Excel._Workbook myBook = null;
            Excel._Worksheet mySheet = null;

            try
            {
                myExcel = new Excel.Application();
                myExcel.DisplayAlerts = false;
                myBook = myExcel.Workbooks.Add(true);
                mySheet = myBook.Worksheets[1];
                myExcel.Visible = false;

                DataTable dtRightGroupByWeek = this.dtRight.AsEnumerable().Where(row => row["Ukey"] == DBNull.Value).CopyToDataTable();
                dtRightGroupByWeek.Columns.Remove("OrderID");
                dtRightGroupByWeek.Columns.Remove("ID");
                dtRightGroupByWeek.Columns.Remove("Ukey");

                for (int i = dtRightGroupByWeek.Columns.Count - 1; i >= 0; i--)
                {
                    if (this.dateRange.Value1 > Convert.ToDateTime(dtRightGroupByWeek.Columns[i].ColumnName)
                        || Convert.ToDateTime(dtRightGroupByWeek.Columns[i].ColumnName) > this.dateRange.Value2)
                    {
                        dtRightGroupByWeek.Columns.Remove(dtRightGroupByWeek.Columns[i].ColumnName);
                    }
                }

                if (dtRightGroupByWeek.Columns.Count > 0)
                {
                    DataTable dtToExcel = new DataTable();
                    dtToExcel.Columns.Add("Week");
                    dtToExcel.Columns.Add("DateRange");
                    dtToExcel.Columns.Add("WeeklyWorkingDays");
                    dtToExcel.Columns.Add("Eff");
                    dtToExcel.Columns.Add("ManPower");
                    dtToExcel.Columns.Add("DailyWorkingHour");
                    dtToExcel.Columns.Add("WorkLoad");

                    int intStartWeek = Convert.ToInt32(dtRightGroupByWeek.Rows[0][0]);
                    int intEndWeek = Convert.ToInt32(dtRightGroupByWeek.Rows[0][dtRightGroupByWeek.Columns.Count - 1]);

                    #region set Week, Eff, ManPower, DailyWorkingHour
                    double doubleEff = Convert.ToDouble(((DataTable)this.listControlBindingSourceCapacity.DataSource).Rows[0]["Data"]);
                    doubleEff = doubleEff.Empty() ? 0 : doubleEff / 100.0;

                    int weekn = 0;
                    foreach (DataColumn dc in dtRightGroupByWeek.Columns)
                    {
                        int getweek = MyUtility.Convert.GetInt(dtRightGroupByWeek.Rows[0][dc]);
                        if (weekn != getweek)
                        {
                            weekn = getweek;
                            DataRow newRow = dtToExcel.NewRow();
                            newRow["Week"] = getweek;
                            newRow["Eff"] = doubleEff;
                            newRow["ManPower"] = ((DataTable)this.listControlBindingSourceCapacity.DataSource).Rows[1]["Data"];
                            newRow["DailyWorkingHour"] = ((DataTable)this.listControlBindingSourceCapacity.DataSource).Rows[2]["Data"];
                            dtToExcel.Rows.Add(newRow);
                        }
                    }

                    dtToExcel.EndInit();
                    #endregion

                    #region set DateRange, WeeklyWorkingDays, WorkLoad
                    int intCheckWeek = intStartWeek;
                    string dateb = string.Empty;
                    int intRowIndex = 0;
                    string sdate = string.Empty;
                    string edate = string.Empty;
                    int intCountWeekDays = 1;
                    decimal sumCPU = 0;
                    weekn = 0;
                    foreach (DataColumn dc in dtRightGroupByWeek.Columns)
                    {
                        int getweek = MyUtility.Convert.GetInt(dtRightGroupByWeek.Rows[0][dc]);
                        if (weekn != getweek)
                        {
                            weekn = getweek;
                            if (!MyUtility.Check.Empty(sdate))
                            {
                                dtToExcel.Rows[intRowIndex]["DateRange"] = sdate + "~" + (MyUtility.Check.Empty(edate) ? sdate : edate);
                                dtToExcel.Rows[intRowIndex]["WeeklyWorkingDays"] = intCountWeekDays;
                                dtToExcel.Rows[intRowIndex]["WorkLoad"] = sumCPU;
                                intRowIndex++;
                                intCountWeekDays = 1;
                            }

                            sdate = Convert.ToDateTime(dc.ColumnName).ToString("MM/dd");
                            sumCPU = MyUtility.Convert.GetDecimal(dtRightGroupByWeek.Rows[1][dc]);
                        }
                        else
                        {
                            edate = Convert.ToDateTime(dc.ColumnName).ToString("MM/dd");
                            sumCPU += MyUtility.Convert.GetDecimal(dtRightGroupByWeek.Rows[1][dc]);
                            intCountWeekDays++;
                        }
                    }

                    // for (int i = 0; i < dtRightGroupByWeek.Columns.Count; i++)
                    // {
                    //    if (dtRightGroupByWeek.Rows[0][i].EqualDecimal(intCheckWeek))
                    //    {
                    //        intCountWeekDays++;
                    //        decimal decCPU;
                    //        decimal.TryParse(dtRightGroupByWeek.Rows[1][i].ToString(), out decCPU);
                    //        intSumCPU += decCPU;
                    //        //intCountWeekDays = 1;
                    //        intSumCPU = decCPU;
                    //        strDateRange = Convert.ToDateTime(dtRightGroupByWeek.Columns[i].ColumnName).ToString("MM/dd");
                    //    }
                    //    else
                    //    {
                    //        //int intRowIndex = Convert.ToInt32(dtRightGroupByWeek.Rows[0][i]) - intStartWeek - 1;
                    //        strDateRange += $"-{Convert.ToDateTime(dtRightGroupByWeek.Columns[i - 1].ColumnName).ToString("MM/dd")}";
                    //        //DataRow newdtToExcel = dtToExcel.NewRow();

                    // dtToExcel.Rows[intRowIndex]["DateRange"] = strDateRange;
                    //        dtToExcel.Rows[intRowIndex]["WeeklyWorkingDays"] = intCountWeekDays;
                    //        dtToExcel.Rows[intRowIndex]["WorkLoad"] = intSumCPU;

                    // decimal decCPU;
                    //        decimal.TryParse(dtRightGroupByWeek.Rows[1][i].ToString(), out decCPU);
                    //        intCheckWeek = MyUtility.Convert.GetInt(dtRightGroupByWeek.Rows[0][i]);
                    //        intCountWeekDays = 1;
                    //        intSumCPU = decCPU;
                    //        strDateRange = Convert.ToDateTime(dtRightGroupByWeek.Columns[i].ColumnName).ToString("MM/dd");
                    //        intRowIndex++;
                    //    }
                    // }
                    dtToExcel.Rows[dtToExcel.Rows.Count - 1]["DateRange"] = sdate + "~" + Convert.ToDateTime(dtRightGroupByWeek.Columns[dtRightGroupByWeek.Columns.Count - 1].ColumnName).ToString("MM/dd");
                    dtToExcel.Rows[dtToExcel.Rows.Count - 1]["WeeklyWorkingDays"] = intCountWeekDays;
                    dtToExcel.Rows[dtToExcel.Rows.Count - 1]["WorkLoad"] = sumCPU;

                    dtToExcel.EndInit();
                    #endregion

                    #region set Excel Rows & Columns
                    mySheet.Cells[1][1] = "Week";
                    mySheet.Cells[1][2] = "Date";
                    mySheet.Cells[1][3] = "Weekly working days";
                    mySheet.Cells[1][4] = "Efficiency";
                    mySheet.Cells[1][5] = "Manpower";
                    mySheet.Cells[1][6] = "Daily Working hour";
                    mySheet.Cells[1][7] = "Capacity (CPU)";
                    mySheet.Cells[1][8] = "Workload (CPU)";
                    mySheet.Cells[1][9] = "Fill Rate";

                    var varStdTMS = MyUtility.GetValue.Lookup("select StdTMS from System");
                    int intStdTMS = 0;
                    intStdTMS = varStdTMS.Empty() ? 0 : Convert.ToInt32(varStdTMS);

                    for (int i = 0; i < dtToExcel.Rows.Count; i++)
                    {
                        string strColumnName = this.GetExcelColumn(i + 1);

                        mySheet.Cells[i + 2][1] = dtToExcel.Rows[i]["Week"];
                        mySheet.Cells[i + 2][2] = dtToExcel.Rows[i]["DateRange"];
                        mySheet.Cells[i + 2][3] = dtToExcel.Rows[i]["WeeklyWorkingDays"];
                        mySheet.Cells[i + 2][4] = dtToExcel.Rows[i]["Eff"];
                        mySheet.Cells[i + 2][5] = dtToExcel.Rows[i]["ManPower"];
                        mySheet.Cells[i + 2][6] = dtToExcel.Rows[i]["DailyWorkingHour"];
                        mySheet.Cells[i + 2][7] = $"=ROUND(({strColumnName}{3} * {strColumnName}{4} * {strColumnName}{5} * {strColumnName}{6} * 3600 / {intStdTMS}), 0)";
                        mySheet.Cells[i + 2][8] = dtToExcel.Rows[i]["WorkLoad"];
                        mySheet.Cells[i + 2][9] = $"=if({strColumnName}{7} = 0, 0, {strColumnName}{8} / {strColumnName}{7})";

                        ((Excel.Range)mySheet.Cells[9, i + 2]).NumberFormat = "0.00%";
                        ((Excel.Range)mySheet.Cells[4, i + 2]).NumberFormat = "0.00%";

                        mySheet.Cells[i + 2][1].Interior.ColorIndex = 45;
                        mySheet.Cells[i + 2][9].Interior.ColorIndex = 36;
                    }

                    mySheet.Rows.AutoFit();
                    mySheet.Columns.AutoFit();

                    mySheet.Cells[1][1].Interior.ColorIndex = 45;
                    mySheet.Cells[1][9].Interior.ColorIndex = 36;
                    #endregion

                    // 在工作簿 新增一張 統計圖表，單獨放在一個分頁裡面
                    myBook.Charts.Add(Type.Missing, Type.Missing, 1, Type.Missing);

                    // 選擇 統計圖表 的 圖表種類
                    myBook.ActiveChart.ChartType = Excel.XlChartType.xlColumnStacked;

                    for (int i = ((Excel.SeriesCollection)myBook.ActiveChart.SeriesCollection()).Count; i > 0; i--)
                    {
                        ((Excel.Series)myBook.ActiveChart.SeriesCollection(i)).Delete();
                    }

                    ((Excel.SeriesCollection)myBook.ActiveChart.SeriesCollection()).NewSeries();
                    ((Excel.Series)myBook.ActiveChart.SeriesCollection(1)).Name = "Workload (CPU)";
                    ((Excel.Series)myBook.ActiveChart.SeriesCollection(1)).Values = mySheet.get_Range("B8", $"{this.GetExcelColumn(dtToExcel.Rows.Count)}8");
                    ((Excel.Series)myBook.ActiveChart.SeriesCollection(1)).XValues = mySheet.get_Range("B1", $"{this.GetExcelColumn(dtToExcel.Rows.Count)}1");

                    ((Excel.SeriesCollection)myBook.ActiveChart.SeriesCollection()).NewSeries();
                    ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).Name = "Capacity (CPU)";
                    ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).Values = mySheet.get_Range("B7", $"{this.GetExcelColumn(dtToExcel.Rows.Count)}7");
                    ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).XValues = mySheet.get_Range("B1", $"{this.GetExcelColumn(dtToExcel.Rows.Count)}1");
                    ((Excel.Series)myBook.ActiveChart.SeriesCollection(2)).ChartType = Excel.XlChartType.xlLine;

                    // 移除圖表 X 軸
                    myBook.ActiveChart.HasAxis[Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary] = false;

                    // 將圖表移至 Sheet (1)
                    myBook.ActiveChart.Location(Excel.XlChartLocation.xlLocationAsObject, mySheet.Name);

                    // 圖表位置
                    mySheet.Shapes.Item(mySheet.Shapes.Count).Top = 180;
                    mySheet.Shapes.Item(mySheet.Shapes.Count).Left = 1;

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("P10_ToExcel");
                    myBook.SaveAs(strExcelName);
                    myBook.Close();
                    myExcel.Quit();
                    strExcelName.OpenFile();
                    #endregion
                }
                else
                {
                    MyUtility.Msg.InfoBox("Data not found.");
                }
            }
            catch (Exception ex)
            {
                myExcel.Visible = false;
                MyUtility.Msg.WarningBox(ex.ToString());
            }
            finally
            {
                // 把執行的Excel資源釋放
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
                myExcel = null;
                myBook = null;
                mySheet = null;
            }
        }

        /// <summary>
        /// 重算 Daily CPU
        /// </summary>
        /// <param name="columnIndex">需要重算 CPU 的欄位</param>
        /// <param name="dtGroupRight">dtGroupRight</param>
        /// <param name="dtGroupLeft">dtGroupLeft</param>
        private void ComputDailyCPU(int columnIndex, DataTable dtGroupRight, DataTable dtGroupLeft)
        {
            var varStdTMS = MyUtility.GetValue.Lookup("select StdTMS from System");
            int intStdTMS = 0;

            intStdTMS = varStdTMS.Empty() ? 0 : Convert.ToInt32(varStdTMS);

            if (dtGroupRight.Rows.Count > 0)
            {
                decimal computCPU = 0;
                long returnCPU = 0;

                if (intStdTMS.EqualDecimal(0) == true)
                {
                    returnCPU = 0;
                }
                else
                {
                    for (int i = 1; i < dtGroupRight.Rows.Count; i++)
                    {
                        if (dtGroupRight.Rows[i][columnIndex].Empty() == false)
                        {
                            DataRow drLeftGrid = dtGroupLeft.Rows[i - 1];

                            decimal qty = Convert.ToDecimal(dtGroupRight.Rows[i][columnIndex]);
                            decimal smv = Convert.ToDecimal(drLeftGrid["SMV"]);

                            computCPU += qty * smv * (decimal)60.0 / intStdTMS;
                        }
                    }

                    returnCPU = Convert.ToInt64(Math.Round(computCPU, MidpointRounding.AwayFromZero));
                }

                switch (returnCPU)
                {
                    case 0:
                        dtGroupRight.Rows[0][columnIndex] = DBNull.Value;
                        break;
                    default:
                        dtGroupRight.Rows[0][columnIndex] = computCPU;
                        break;
                }

                dtGroupRight.Rows[1].EndEdit();
            }
        }

        /// <summary>
        /// GetExcelColumn 從 A 開始算
        /// 因此 傳入參數要再 - 1
        /// </summary>
        /// <param name="columnIndex">columnIndex</param>
        /// <returns>strReturn</returns>
        private string GetExcelColumn(int columnIndex)
        {
            string strReturn = string.Empty;
            do
            {
                strReturn = (char)('A' + (columnIndex % 26)) + strReturn;
                columnIndex /= 26;
            }
            while (columnIndex-- > 0);

            return strReturn;
        }
    }
}