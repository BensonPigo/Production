using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Collections.Generic;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R07
    /// </summary>
    public partial class R07 : Win.Tems.PrintForm
    {
        private string factory;
        private string mdivision;
        private string cdCode;
        private string shift;
        private DateTime? outputDate1;
        private DateTime? outputDate2;
        private DataTable[] printData = new DataTable[2];
        private int bolSintexEffReportCompare = 0;
        private StringBuilder condition = new StringBuilder();
        private DateTime currentTime = DateTime.Now;

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, ",,D,Day,N,Night");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.IsFormClosed)
            {
                return;
            }

            this.numYear.Value = this.currentTime.Year;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if ((MyUtility.Check.Empty(this.dateOutputDate.Value1) || MyUtility.Check.Empty(this.dateOutputDate.Value2)) && this.radioDetail.Checked)
            {
                MyUtility.Msg.WarningBox(" < Output Date > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            if (this.radioDetail.Checked)
            {
                this.outputDate1 = this.dateOutputDate.Value1;
                this.outputDate2 = this.dateOutputDate.Value2;
            }
            else
            {
                this.outputDate1 = MyUtility.Convert.GetDate(this.numYear.Value.ToString() + "/01/01");
                this.outputDate2 = MyUtility.Convert.GetDate(this.numYear.Value.ToString() + "/12/31");
            }
            #endregion
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.cdCode = this.txtCDCode.Text;
            this.shift = this.comboShift.SelectedValue.ToString();
            this.bolSintexEffReportCompare = this.radioSintexEffReportCompare.Checked ? 1 : 0;
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                "exec dbo.GetAdidasEfficiencyReport '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}",
                this.outputDate1.Value.ToString("yyyy/MM/dd"),
                this.outputDate2.Value.ToString("yyyy/MM/dd"),
                this.mdivision,
                this.factory,
                this.cdCode,
                this.shift,
                this.bolSintexEffReportCompare);

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), null, out this.printData);

            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // Linq 整理資料
            this.printData = this.OrganizeData(this.printData);

            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);

            if (this.printData[0].Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData[0].Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R07.xltx"); // 預先開啟excel app
            Excel.Worksheet objSheets1 = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   // 取得工作表

            objApp.Visible = false;
            if (this.radioDetail.Checked)
            {
                this.SetExcelSheet1(objApp, objSheets1, this.printData[0]);
                objSheets2.Visible = Excel.XlSheetVisibility.xlSheetHidden;
            }
            else
            {
                // this.SetExcelSheet1(objApp, objSheets1, tmpDt);
                this.SetExcelSheet2(objSheets2, this.printData);
                objSheets1.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                objSheets2.Columns.AutoFit();
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Planning_R07");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets1);
            Marshal.ReleaseComObject(objSheets2);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private DataTable[] OrganizeData(DataTable[] dt)
        {
            DataTable[] rtnDataTable = new DataTable[2];
            if (this.bolSintexEffReportCompare == 0)
            {
                var queryDt = dt[0].AsEnumerable()
                    .OrderBy(x => x.Field<DateTime?>("OutputDate"))
                    .ThenBy(x => x.Field<string>("FactoryID") + x.Field<string>("SewingLineID") + x.Field<string>("SeasonID") + x.Field<string>("BrandID") + x.Field<string>("StyleID"))
                    .Select((x, index) => new
                    {
                        OutputDate = x.Field<DateTime?>("OutputDate"),
                        FactoryID = x.Field<string>("FactoryID"),
                        SewingLineID = x.Field<string>("SewingLineID"),
                        Shift = x.Field<string>("Shift"),
                        Category = x.Field<string>("Category"),
                        StyleID = x.Field<string>("StyleID"),
                        Manpower = x.Field<decimal>("Manpower"),
                        ManHour = x.Field<decimal>("ManHour"),
                        TotalOutput = x.Field<int>("TotalOutput"),
                        CD = x.Field<string>("CD"),
                        SeasonID = x.Field<string>("SeasonID"),
                        BrandID = x.Field<string>("BrandID"),
                        Fabrication = string.Format("=IFERROR(VLOOKUP(LEFT(J{0}, 2),'Adidas data '!$A$2:$G$116, 4, FALSE), \"\")", index + 2),
                        ProductGroup = string.Format("=IFERROR(VLOOKUP(LEFT(J{0}, 2),'Adidas data '!$A$2:$G$116, 7, FALSE), \"\")", index + 2),
                        ProductFabrication = string.Format("=N{0}&M{0}", index + 2),
                        GSD = x.Field<decimal>("GSD"),
                        Earnedhours = string.Format("=IF(I{0}=\"\", \"\", IFERROR((I{0}*P{0})/60, \"\"))", index + 2),
                        TotalWorkingHours = string.Format("=H{0}*G{0}", index + 2),
                        CumulateDaysofDaysinProduction = x.Field<int>("CumulateDaysofDaysinProduction"),
                        EfficiencyLine = string.Format("=Q{0}/R{0}", index + 2),
                        GSDProsmv = x.Field<decimal>("GSDProsmv"),
                        Earnedhours2 = string.Format("=IF(I{0}=\"\", \"\", IFERROR(I{0}*U{0}/60, \"\"))", index + 2),
                        EfficiencyLine2 = string.Format("=V{0}/R{0}", index + 2),
                        NoofInlineDefects = x.Field<int>("NoofInlineDefects"),
                        NoofEndlineDefectiveGarments = x.Field<int>("NoofEndlineDefectiveGarments"),
                        WFT = string.Format("=IFERROR((X{0}+Y{0})/I{0}, \"\")", index + 2),
                        Country = x.Field<string>("Country"),
                        Month = x.Field<string>("Month"),
                        IsGSDPro = x.Field<string>("IsGSDPro"),
                        Orderseq = x.Field<int>("Orderseq"),
                    })
                    .ToList();

                rtnDataTable[0] = PublicPrg.ListToDataTable.ToDataTable(queryDt);
            }
            else
            {
                var querySintexReportMonth = dt[0].AsEnumerable()
                    .OrderBy(x => x.Field<int>("Orderseq"))
                    .ThenBy(x => x.Field<DateTime?>("OutputDate"))
                    .GroupBy(x => new { Country = x.Field<string>("Country"), Month = x.Field<string>("Month"), Orderseq = x.Field<int>("Orderseq") })
                    .Select((x, index) => new
                    {
                        x.Key.Country,
                        x.Key.Month,
                        x.Key.Orderseq,
                        PROEarnedHrs = x.Sum(y => y.Field<decimal>("Earnedhours2")),
                        PROManhours = x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                        PROEfficiency = string.Format("=D{0}/E{0}", index + 13),
                        SIOEarnedHrs = x.Sum(y => y.Field<decimal>("Earnedhours")),
                        SIOManhours = x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                        SIOEfficiency = string.Format("=G{0}/H{0}", index + 13),
                        PROSIOGap = string.Format("=I{0}-F{0}", index + 13),
                        SamplesSIO = x.Where(y => y.Field<string>("IsGSDPro").EqualString("V")).Sum(y => y.Field<int>("TotalOutput")) *
                                     x.Where(y => y.Field<string>("IsGSDPro").EqualString("V")).Sum(y => y.Field<decimal>("GSD")) /
                                     60 /
                                     x.Sum(y => y.Field<decimal>("Earnedhours")),
                    })
                    .ToList();

                rtnDataTable[0] = PublicPrg.ListToDataTable.ToDataTable(querySintexReportMonth);

                var querySintexReportSeason = dt[0].AsEnumerable()
                    .GroupBy(x => new { Country = x.Field<string>("Country"), SeasonID = x.Field<string>("SeasonID"), Orderseq = x.Field<int>("Orderseq") })
                    .Select((x, index) => new
                    {
                        x.Key.Country,
                        x.Key.SeasonID,
                        x.Key.Orderseq,
                        ProEff = x.Sum(y => y.Field<decimal>("Earnedhours2")) / x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                        SIOEff = x.Sum(y => y.Field<decimal>("Earnedhours")) / x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                    })
                    .OrderBy(x => x.Country)
                    .ThenBy(x => x.SeasonID)
                    .ToList();

                rtnDataTable[1] = PublicPrg.ListToDataTable.ToDataTable(querySintexReportSeason);
            }

            return rtnDataTable;
        }

        private void SetExcelSheet1(Excel.Application objApp, Excel.Worksheet objSheets, DataTable dt)
        {
            int cMax = 100000;
            for (int i = 0; i <= dt.Rows.Count / cMax; i++)
            {
                int cSkip = cMax * i;
                int cTake = i == 0 ? i : cSkip;
                MyUtility.Excel.CopyToXls(
                    dt.AsEnumerable().Skip(cSkip).Take(cMax).CopyToDataTable(),
                    null,
                    "Planning_R07.xltx",
                    headerRow: cTake + 1,
                    excelApp: objApp,
                    showExcel: false,
                    showSaveMsg: false,
                    wSheet: objSheets);
            }

            objSheets.get_Range("AA:AD").EntireColumn.Hidden = true;
        }

        private void SetExcelSheet2(Excel.Worksheet objSheets, DataTable[] dt)
        {
            List<string> countrys = dt[1].AsEnumerable()
                            .GroupBy(x => new { Country = x.Field<string>("Country"), OrderBySeq = x.Field<int>("Orderseq") })
                            .OrderBy(x => x.Key.OrderBySeq)
                            .Select(x => x.Key.Country)
                            .ToList();

            List<string> sessions = dt[1].AsEnumerable()
                            .GroupBy(x => new { SeasonID = x.Field<string>("SeasonID") })
                            .OrderBy(x => x.Key.SeasonID)
                            .Select(x => x.Key.SeasonID)
                            .ToList();

            #region 上半部

            for (int i = 0; i <= sessions.Count - 1; i++)
            {
                if (i > 0)
                {
                    objSheets.get_Range(string.Format("B{0}", i + 2)).Copy();
                    objSheets.get_Range(string.Format("B{0}", i + 3)).PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                }

                objSheets.Cells[i + 3, 2] = sessions[i];
            }

            for (int i = 0; i <= countrys.Count - 1; i++)
            {
                objSheets.Cells[1, (i + 1) * 3] = countrys[i];
                objSheets.get_Range(string.Format("{0}1:{1}1", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2))).Merge();

                object[,] objArrayTop = new object[1, 3];
                objArrayTop[0, 0] = "PRO Eff.";
                objArrayTop[0, 1] = "SIO Eff.";
                objArrayTop[0, 2] = "PRO/SIO Gap";
                objSheets.Range[string.Format("{0}2:{1}2", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2))].Value2 = objArrayTop;

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Interior.ColorIndex = this.SetExcelColor(countrys[i]);

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Font.Name = "Segoe UI";

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Font.Bold = true;

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Borders.LineStyle = 1;

                for (int j = 0; j <= sessions.Count - 1; j++)
                {
                    var querySessionRow = dt[1].AsEnumerable()
                        .Where(x => x.Field<string>("Country").EqualString(countrys[i]) &&
                                    x.Field<string>("SeasonID").EqualString(sessions[j]))
                        .Select(x => new
                        {
                            ProEff = x.Field<decimal?>("ProEff"),
                            SIOEff = x.Field<decimal?>("SIOEff"),
                        })
                        .FirstOrDefault();

                    objArrayTop = new object[1, 3];
                    objArrayTop[0, 0] = querySessionRow != null && querySessionRow.ProEff.HasValue ? querySessionRow.ProEff : 0;
                    objArrayTop[0, 1] = querySessionRow != null && querySessionRow.SIOEff.HasValue ? querySessionRow.SIOEff : 0;
                    objArrayTop[0, 2] = string.Format("=IFERROR({0}{2}-{1}{2}, \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 1), MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), j + 3);
                    objSheets.Range[string.Format("{0}{2}:{1}{2}", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2), j + 3)].Value2 = objArrayTop;
                }
            }

            #endregion

            #region 下半部
            int initR = 13;
            int initC = 2;
            int initI = 0;
            string initCountry = string.Empty;
            object[,] objArray = new object[1, 10];
            foreach (DataRow dr in dt[0].Rows)
            {
                if (initCountry.Empty())
                {
                    initCountry = dr["Country"].ToString();
                }

                objArray[0, 0] = dr["Country"].ToString();
                objArray[0, 1] = dr["Month"].ToString();
                objArray[0, 2] = dr["PROEarnedHrs"].ToString();
                objArray[0, 3] = dr["PROManhours"].ToString();
                objArray[0, 4] = dr["PROEfficiency"].ToString();
                objArray[0, 5] = dr["SIOEarnedHrs"].ToString();
                objArray[0, 6] = dr["SIOManhours"].ToString();
                objArray[0, 7] = dr["SIOEfficiency"].ToString();
                objArray[0, 8] = dr["PROSIOGap"].ToString();
                objArray[0, 9] = dr["SamplesSIO"].ToString();
                objSheets.Range[string.Format("B{0}:K{0}", initR)].Value2 = objArray;
                objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Interior.ColorIndex = this.SetExcelColor(dr["Country"].ToString());
                objSheets.get_Range(string.Format("B{0}:K{0}", initR)).Borders.LineStyle = 1;
                initR++;
                initI++;

                if (initI >= dt[0].Rows.Count || !initCountry.EqualString(dt[0].Rows[initI]["Country"]))
                {
                    int rcount = dt[0].AsEnumerable().Where(x => x.Field<string>("Country").EqualString(dr["Country"].ToString())).Count();

                    objArray[0, 0] = dr["Country"].ToString();
                    objArray[0, 1] = "YTD";
                    objArray[0, 2] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 2), initR - rcount, initR - 1);
                    objArray[0, 3] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 3), initR - rcount, initR - 1);
                    objArray[0, 4] = string.Format("=IFERROR(ROUND({0}{2}/{1}{2}, 2), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 2), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 3), initR);
                    objArray[0, 5] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 5), initR - rcount, initR - 1);
                    objArray[0, 6] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 6), initR - rcount, initR - 1);
                    objArray[0, 7] = string.Format("=IFERROR(ROUND({0}{2}/{1}{2}, 2), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 5), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 6), initR);
                    objArray[0, 8] = string.Format("=IFERROR({0}{2}-{1}{2}, \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 7), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 4), initR);
                    objArray[0, 9] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 9), initR - rcount, initR - 1);
                    objSheets.Range[string.Format("B{0}:K{0}", initR)].Value2 = objArray;
                    objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Interior.ColorIndex = this.SetExcelColor(dr["Country"].ToString());
                    objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Font.Bold = true;
                    objSheets.get_Range(string.Format("B{0}:K{0}", initR)).Borders.LineStyle = 1;
                    initCountry = initI >= dt[0].Rows.Count ? dr["Country"].ToString() : dt[0].Rows[initI]["Country"].ToString();
                    initR++;
                }
            }

            #endregion
        }

        private int SetExcelColor(string country)
        {
            int rtnVal = 1;
            switch (country.ToUpper())
            {
                case "PHILIPPINES":
                    rtnVal = 17;
                    break;
                case "VIETNAM":
                    rtnVal = 16;
                    break;
                case "CAMBODIA":
                    rtnVal = 35;
                    break;
                case "CHINA":
                    rtnVal = 45;
                    break;
            }

            return rtnVal;
        }

        private void RadioDetail_CheckedChanged(object sender, EventArgs e)
        {
            this.dateOutputDate.Visible = true;
            this.numYear.Visible = false;
        }

        private void RadioSintexEffReportCompare_CheckedChanged(object sender, EventArgs e)
        {
            this.dateOutputDate.Visible = false;
            this.numYear.Visible = true;
        }
    }
}
