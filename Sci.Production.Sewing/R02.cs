﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Class.Command;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private string line1;
        private string line2;
        private string factory;
        private string factoryName;
        private string mDivision;
        private DateTime? date1;
        private DateTime? date2;
        private int reportType;
        private int orderby;
        private DataTable SewOutPutData;
        private DataTable printData;
        private DataTable excludeInOutTotal;
        private DataTable NonSisterInTotal;
        private DataTable SisterInTotal;
        private DataTable cpuFactor;
        private DataTable subprocessData;
        private DataTable subprocessSubconInData;
        private DataTable subprocessSubconOutData;
        private DataTable subconData;
        private DataTable vphData;
        private List<APIData> pams = new List<APIData>();
        private int workDay;

        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.label10.Text = "** The value in this report are all excluded subcon-out,\r\n unless the column with \"included subcon-out\".";
            DataTable factory, mDivision;
            DBProxy.Current.Select(
                null,
                string.Format(@"select '' as FtyGroup 
union all
select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup"),
                out factory);

            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            MyUtility.Tool.SetupCombox(this.comboReportType, 1, 1, "By Date,By Sewing Line,By Sewing Line By Team");
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Sewing Line,CPU/Sewer/HR");
            this.comboReportType.SelectedIndex = 0;
            this.comboFactory.Text = Env.User.Factory;
            this.comboOrderBy.SelectedIndex = 0;
            this.comboM.Text = Env.User.Keyword;
        }

        // Date
        private void DateDateStart_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateDateStart.Value))
            {
                this.dateDateEnd.Value = null;
            }
            else
            {
                this.dateDateEnd.Value = Convert.ToDateTime(this.dateDateStart.Value).AddDays(1 - Convert.ToDateTime(this.dateDateStart.Value).Day).AddMonths(1).AddDays(-1);
            }
        }

        // Report Type
        private void ComboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelOrderBy.Visible = true;
            this.comboOrderBy.Visible = true;
            switch (this.comboReportType.SelectedIndex)
            {
                case 0:
                case 2:
                    this.labelOrderBy.Visible = false;
                    this.comboOrderBy.Visible = false;
                    break;
            }
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}", MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" where FactoryID = '{0}'", this.comboFactory.Text));
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "3", line, false, ",")
            {
                Width = 300,
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return string.Empty;
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        // Sewing Line
        private void TxtSewingLineStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineStart.Text = this.SelectSewingLine(this.txtSewingLineStart.Text);
        }

        // Sewing Line
        private void TxtSewingLineEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineEnd.Text = this.SelectSewingLine(this.txtSewingLineEnd.Text);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDateStart.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (this.comboReportType.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Report type can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.comboM.Text))
            {
                MyUtility.Msg.WarningBox("M can't empty!!");
                return false;
            }

            if (this.comboReportType.SelectedIndex == 1)
            {
                if (this.comboFactory.SelectedIndex == -1 || this.comboFactory.SelectedIndex == 0)
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    return false;
                }

                if (this.comboOrderBy.SelectedIndex == -1)
                {
                    MyUtility.Msg.WarningBox("Order by can't empty!!");
                    return false;
                }
            }

            if (this.comboReportType.SelectedIndex == 2)
            {
                if (this.comboFactory.SelectedIndex == -1 || this.comboFactory.SelectedIndex == 0)
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    return false;
                }
            }

            this.date1 = this.dateDateStart.Value;
            this.date2 = this.dateDateEnd.Value;
            this.line1 = this.txtSewingLineStart.Text;
            this.line2 = this.txtSewingLineEnd.Text;
            this.factory = this.comboFactory.Text;
            this.mDivision = this.comboM.Text;
            this.reportType = this.comboReportType.SelectedIndex;
            this.orderby = this.comboOrderBy.SelectedIndex;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // 若有變更到  Total CPU Included Subcon In 的相關計算規則&篩選條件, 要一併變更 sql的table function GetCMPDetail (這是只有FMS要用的)
            StringBuilder sqlCmd = new StringBuilder();
            DualResult failResult;
            DBProxy.Current.DefaultTimeout = 600; // 加長時間成10分鐘, 避免time out
            Sewing_R02 biModel = new Sewing_R02();

            #region 組撈全部Sewing output data SQL
            Sewing_R02_ViewModel sewing_R02_Model = new Sewing_R02_ViewModel()
            {
                StartOutputDate = this.dateDateStart.Value,
                EndOutputDate = this.dateDateEnd.Value,
                Factory = this.factory,
                M = this.mDivision,
                ReportType = this.reportType + 1,
                StartSewingLine = this.txtSewingLineStart.Text,
                EndSewingLine = this.txtSewingLineEnd.Text,
                OrderBy = this.orderby + 1,
                ExcludeNonRevenue = this.chkExcludeNonRevenue.Checked,
                ExcludeSampleFactory = this.checkSampleFty.Checked,
                ExcludeOfMockUp = this.checkExcludeOfMockUp.Checked,
            };

            Base_ViewModel resultReport = biModel.GetMonthlyProductionOutputReport(sewing_R02_Model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.SewOutPutData = resultReport.DtArr[0];
            this.printData = resultReport.DtArr[1];

            #endregion

            #region 整理Total Exclude Subcon-In & Out
            try
            {
                resultReport = biModel.GetTotalExcludeSubconIn(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.excludeInOutTotal = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Total Exclude Subcon-In & Out total data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理non Sister SubCon In
            try
            {
                resultReport = biModel.GetNoNSisterSubConIn(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.NonSisterInTotal = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Non Sister SubCon In data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理Sister SubCon In
            try
            {
                resultReport = biModel.GetSisterSubConIn(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.SisterInTotal = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Sister SubCon In data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理CPU Factor
            try
            {
                resultReport = biModel.GetCPUFactor(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.cpuFactor = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Query CPU factor data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理Subprocess資料
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubprocess(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subprocessData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subprocess by Company Subcon-In資料 Orders.program
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubprocessbyCompanySubconIn(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subprocessSubconInData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subprocess by Company Subcon-Out資料 SewingOutput.SubconOutFty
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubprocessbyCompanySubconOut(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subprocessSubconOutData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subcon資料
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubcon(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subconData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query subcon data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理工作天數
            sewing_R02_Model = new Sewing_R02_ViewModel()
            {
                IsCN = Env.User.Keyword.EqualString("CM1") || Env.User.Keyword.EqualString("CM2"),
                M = this.mDivision,
                StartDate = this.date1.Value,
                EndDate = this.date2.Value,
            };

            resultReport = biModel.GetWorkDay(this.SewOutPutData, sewing_R02_Model);
            if (!resultReport.Result)
            {
                failResult = new DualResult(false, "Query Work Day fail\r\n" + resultReport.Result.Messages.ToString());
                return failResult;
            }

            this.workDay = resultReport.IntValue;
            #endregion

            #region Direct Manpower(From PAMS)
            sewing_R02_Model = new Sewing_R02_ViewModel()
            {
                M = this.mDivision,
                Factory = this.factory,
                StartDate = this.date1.Value,
                EndDate = this.date2.Value,
            };

            this.pams = biModel.GetPAMS(sewing_R02_Model);
            #endregion

            if (MyUtility.Check.Empty(this.factory) && !MyUtility.Check.Empty(this.mDivision))
            {
                this.factoryName = MyUtility.GetValue.Lookup(string.Format("select Name from Mdivision WITH (NOLOCK) where ID = '{0}'", this.mDivision));
            }
            else
            {
                this.factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", this.factory));
            }

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            Excel.Range rngToInsert;
            Excel.Range rngBorders;

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir;
            switch (this.reportType)
            {
                case 0:
                    strXltName += "\\Sewing_R02_MonthlyReportByDate.xltx";
                    break;
                case 1:
                    strXltName += "\\Sewing_R02_MonthlyReportBySewingLine.xltx";
                    break;
                case 2:
                    strXltName += "\\Sewing_R02_MonthlyReportBySewingLineByTeam.xltx";
                    break;
            }

            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format("{0}", this.factoryName);
            worksheet.Cells[3, 1] = string.Format(
                "All Factory Monthly CMP Report, MTH:{1}",
                MyUtility.Check.Empty(this.factory) ? "All Factory" : this.factory,
                Convert.ToDateTime(this.date1).ToString("yyyy/MM"));

            int insertRow;
            object[,] objArray = new object[1, 15];

            // Top Table建立
            this.SetExcelTopTable(out insertRow, this.pams, worksheet, excel);

            // CPU Factor
            insertRow = insertRow + 2;
            worksheet.Cells[insertRow, 3] = "Total CPU Included Subcon-In";
            insertRow++;
            if (this.cpuFactor.Rows.Count > 2)
            {
                // 插入Record
                for (int i = 0; i < this.cpuFactor.Rows.Count - 2; i++)
                {
                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            objArray = new object[1, 4];
            foreach (DataRow dr in this.cpuFactor.Rows)
            {
                objArray[0, 0] = string.Format("CPU * {0}", MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["CPUFactor"]), 1)));
                objArray[0, 1] = dr["QAQty"];
                objArray[0, 2] = dr["CPU"];
                objArray[0, 3] = dr["Style"];

                worksheet.Range[string.Format("A{0}:D{0}", insertRow)].Value2 = objArray;
                insertRow++;
                rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            this.DeleteExcelRow(2, insertRow, excel);

            // Subprocess
            insertRow = insertRow + 2;
            int insertRec = 0;
            foreach (DataRow dr in this.subprocessData.Rows)
            {
                insertRec++;
                if (insertRec % 2 == 1)
                {
                    worksheet.Cells[insertRow, 2] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["rs"]));
                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr["Price"]);
                }
                else
                {
                    worksheet.Cells[insertRow, 6] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["rs"]));
                    worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(dr["Price"]);
                    insertRow++;

                    // 插入一筆Record
                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            insertRow = insertRow + 3;
            worksheet.Cells[insertRow, 1] = "Total work day:";
            worksheet.Cells[insertRow, 3] = this.workDay;

            // Subcon
            int revenueStartRow = 0;
            insertRow = insertRow + 2;
            int insertSubconIn = 0, insertSubconOut = 0;
            objArray = new object[1, 3];
            if (this.subconData.Rows.Count > 0)
            {
                foreach (DataRow dr in this.subconData.Rows)
                {
                    if (MyUtility.Convert.GetString(dr["Type"]) == "I")
                    {
                        insertRow++;
                        insertSubconIn = 1;
                        objArray[0, 0] = dr["Company"];
                        objArray[0, 1] = string.Empty;
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[string.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        // 插入一筆Record
                        rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);

                        #region Sub-Process Total Revenue for Company Subcon-In
                        insertRec = 0;
                        if (this.subprocessSubconInData.AsEnumerable().Where(s => s["Company"].Equals(dr["Company"])).Any())
                        {
                            insertRow++;
                            revenueStartRow = insertRow;

                            // title
                            worksheet.Cells[insertRow, 1] = "Sub-Process Total Revenue";
                            worksheet.Cells[insertRow, 9] = "(Unit:US$)";

                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            Marshal.ReleaseComObject(rngToInsert);

                            insertRow++;
                            foreach (DataRow dr_sub in this.subprocessSubconInData.Select($" Company = '{dr["Company"]}'"))
                            {
                                insertRec++;
                                if (insertRec % 2 == 1)
                                {
                                    worksheet.Cells[insertRow, 2] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                }
                                else
                                {
                                    worksheet.Cells[insertRow, 6] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                    insertRow++;

                                    // 插入一筆Record
                                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                                    Marshal.ReleaseComObject(rngToInsert);
                                }
                            }

                            // 畫框線
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{1}", MyUtility.Convert.GetString(revenueStartRow), MyUtility.Convert.GetString(insertRow)), Type.Missing);
                            rngBorders.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());     // 給單元格加邊框
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(revenueStartRow)), Type.Missing);
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                            // 插入一筆Record
                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            Marshal.ReleaseComObject(rngToInsert);
                        }
                        #endregion
                    }
                    else
                    {
                        if (insertSubconOut == 0)
                        {
                            if (insertSubconIn == 0)
                            {
                                this.DeleteExcelRow(2, insertRow, excel);
                                insertRow = insertRow + 3;
                            }
                            else
                            {
                                insertRow = insertRow + 5;
                            }
                        }

                        insertSubconOut = 1;
                        insertRow++;
                        objArray[0, 0] = dr["Company"].Equals("Other") ? "SUBCON-OUT TO OTHER COMPANIES" : dr["Company"];
                        objArray[0, 1] = string.Empty;
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[string.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        #region Sub-Process Total Revenue for Company Subcon-OUT
                        insertRec = 0;
                        if (this.subprocessSubconOutData.AsEnumerable().Where(s => s["Company"].Equals(dr["Company"])).Any())
                        {
                            insertRow++;
                            revenueStartRow = insertRow;

                            // title
                            worksheet.Cells[insertRow, 1] = "Sub-Process Total Revenue";
                            worksheet.Cells[insertRow, 9] = "(Unit:US$)";

                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            Marshal.ReleaseComObject(rngToInsert);

                            insertRow++;
                            foreach (DataRow dr_sub in this.subprocessSubconOutData.Select($" Company = '{dr["Company"]}'"))
                            {
                                insertRec++;
                                if (insertRec % 2 == 1)
                                {
                                    worksheet.Cells[insertRow, 2] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                }
                                else
                                {
                                    worksheet.Cells[insertRow, 6] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                    insertRow++;

                                    // 插入一筆Record
                                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                                    Marshal.ReleaseComObject(rngToInsert);
                                }
                            }

                            // 畫框線
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{1}", MyUtility.Convert.GetString(revenueStartRow), MyUtility.Convert.GetString(insertRow)), Type.Missing);
                            rngBorders.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());     // 給單元格加邊框
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(revenueStartRow)), Type.Missing);
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                        }
                        #endregion

                        // 插入一筆Record
                        rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }
                }

                if (insertSubconOut == 0)
                {
                    // 刪除資料
                    this.DeleteExcelRow(2, insertRow + 5, excel);
                }
            }
            else
            {
                this.DeleteExcelRow(9, insertRow, excel);
            }

            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(this.reportType == 0 ? "Sewing_R02_MonthlyReportByDate" : "Sewing_R02_MonthlyReportBySewingLine");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Visible = true;
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            #endregion
            return true;
        }

        private void DeleteExcelRow(int rowCount, int rowLocation, Excel.Application excel)
        {
            for (int i = 1; i <= rowCount; i++)
            {
                Excel.Range rng = (Excel.Range)excel.Rows[rowLocation];

                // rng.Select();
                rng.Delete();
            }
        }

        private void SetExcelTopTable(out int insertRow, List<APIData> pams, Excel.Worksheet worksheet, Excel.Application excel)
        {
            insertRow = 5;
            Excel.Range rngToInsert;
            object[,] objArray = new object[1, 18];
            int iQAQty = 2, iTotalCPU = 3, iCPUSewer = 6, iAvgWorkHour = 7, iManHour = 9, iEff = 10;
            string sEff;
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr[0];
                objArray[0, 1] = dr[1];
                objArray[0, 2] = dr[2];
                objArray[0, 3] = dr[3];
                objArray[0, 4] = dr[4];
                objArray[0, 5] = dr[5];
                objArray[0, 6] = dr[6];
                objArray[0, 7] = dr[7];
                objArray[0, 8] = dr[8];

                if (this.reportType == 2)
                {
                    objArray[0, 9] = dr[9];

                    // EFF %  欄位公式
                    objArray[0, 10] = string.Format("=IF(J{0}=0,0,ROUND((D{0}/(J{0}*3600/1400))*100,1))", insertRow);
                }
                else
                {
                    // EFF %  欄位公式
                    objArray[0, 9] = string.Format("=IF(I{0}=0,0,ROUND((C{0}/(I{0}*3600/1400))*100,1))", insertRow);

                    if (this.reportType == 0)
                    {
                        if (pams != null && pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).Count() > 0)
                        {
                            // Total Manpower (PAMS)
                            objArray[0, 11] = pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().SewTtlManpower;

                            // Total Manhours (PAMS)
                            objArray[0, 12] = pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().SewTtlManhours;

                            string holiday = (pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().Holiday == 1) ? "Y" : string.Empty;

                            /*
                            // Test
                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerIn = 2.5M;
                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerOut = 1.5M;

                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursIn = 2.5M;
                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursOut = 6.7M;
                            */

                            // Holiday (PAMS)
                            objArray[0, 14] = holiday;
                            decimal transferManpower = MyUtility.Convert.GetDecimal(pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerIn
                                - pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerOut);

                            decimal transferManhours = MyUtility.Convert.GetDecimal(pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursIn
                                - pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursOut);

                            // Transfer Manpower(PAMS)
                            objArray[0, 15] = transferManpower;

                            // Transfer Manhours(PAMS)]
                            objArray[0, 16] = transferManhours;
                        }
                        else
                        {
                            objArray[0, 11] = 0;
                            objArray[0, 12] = 0;
                        }

                        // Average Working Hour(PAMS)
                        objArray[0, 10] = MyUtility.Convert.GetDouble(objArray[0, 11]) == 0 ? 0 : MyUtility.Convert.GetDouble(objArray[0, 12]) / MyUtility.Convert.GetDouble(objArray[0, 11]);

                        // EFF % (PAMS) 欄位公式
                        objArray[0, 13] = string.Format("=IF(M{0}=0,0,ROUND((C{0}/(M{0}*3600/1400))*100,1))", insertRow);

                        // Remark
                        objArray[0, 17] = string.Empty;
                    }
                }

                worksheet.Range[string.Format("A{0}:R{0}", insertRow)].Value2 = objArray;
                insertRow++;

                // 插入一筆Record
                rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            // 將多出來的Record刪除
            this.DeleteExcelRow(2, insertRow, excel);

            // Total
            if (this.reportType == 2)
            {
                iQAQty = 3;
                iTotalCPU = 4;
                iCPUSewer = 7;
                iAvgWorkHour = 8;
                iManHour = 10;
                iEff = 11;
                worksheet.Cells[insertRow, 3] = string.Format("=SUM(C5:C{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 4] = string.Format("=SUM(D5:D{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 5] = string.Format("=SUM(E5:E{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 6] = string.Format("=SUM(F5:F{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 7] = string.Format("=ROUND(D{0}/J{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 8] = string.Format("=ROUND(J{0}/I{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 9] = string.Format("=SUM(I5:I{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 10] = string.Format("=SUM(J5:J{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 11] = string.Format("=ROUND(D{0}/(J{0}*60*60/1400)*100,1)", insertRow);
            }
            else
            {
                worksheet.Cells[insertRow, 2] = string.Format("=SUM(B5:B{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 3] = string.Format("=SUM(C5:C{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 4] = string.Format("=SUM(D5:D{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 5] = string.Format("=SUM(E5:E{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 6] = string.Format("=ROUND(C{0}/I{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 7] = string.Format("=ROUND(I{0}/H{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 8] = string.Format("=SUM(H5:H{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 9] = string.Format("=SUM(I5:I{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 10] = string.Format("=ROUND(C{0}/(I{0}*60*60/1400)*100,1)", insertRow);
                if (this.reportType == 0)
                {
                    worksheet.Cells[insertRow, 11] = string.Format("=ROUND(M{0}/L{0},2)", MyUtility.Convert.GetString(insertRow));
                    worksheet.Cells[insertRow, 12] = string.Format("=SUM(L5:L{0})", MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 13] = string.Format("=SUM(M5:M{0})", MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 14] = string.Format("=ROUND(C{0}/(M{0}*60*60/1400)*100,1)", insertRow);
                    worksheet.Cells[insertRow, 16] = $"=SUM(P5:P{MyUtility.Convert.GetString(insertRow - 1)})";
                    worksheet.Cells[insertRow, 17] = $"=SUM(Q5:Q{MyUtility.Convert.GetString(insertRow - 1)})";
                }
            }

            insertRow++;

            // Excluded non sister Subcon In
            sEff = this.reportType == 2 ? string.Format("=ROUND((D{0}/(J{0}*3600/1400))*100,1)", insertRow) : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            worksheet.Cells[insertRow, iQAQty] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, iTotalCPU] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, iCPUSewer] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, iAvgWorkHour] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["AvgWorkHour"]);
            worksheet.Cells[insertRow, iManHour] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, iEff] = (this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : sEff;
            insertRow++;

            // non sister Subcon In
            sEff = this.reportType == 2 ? string.Format("=ROUND((D{0}/(J{0}*3600/1400))*100,1)", insertRow) : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            worksheet.Cells[insertRow, iQAQty] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, iTotalCPU] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, iCPUSewer] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, iManHour] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, iEff] = (this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : sEff;
            insertRow++;

            // sister Subcon In
            sEff = this.reportType == 2 ? string.Format("=ROUND((D{0}/(J{0}*3600/1400))*100,1)", insertRow) : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            worksheet.Cells[insertRow, iQAQty] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, iTotalCPU] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, iCPUSewer] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, iManHour] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, iEff] = (this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : sEff;
        }
    }
}
