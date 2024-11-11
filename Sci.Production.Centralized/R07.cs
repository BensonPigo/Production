﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Prg;

namespace Sci.Production.Centralized
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
        private string brand;
        private DateTime? outputDate1;
        private DateTime? outputDate2;
        private DataTable[] printData;
        private int bolSintexEffReportCompare = 0;
        private StringBuilder condition = new StringBuilder();
        private DateTime currentTime = DateTime.Now;
        private string productType;
        private string fabricType;
        private string lining;
        private string gender;
        private string construction;

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, ",,0,Day+Night,1,Subcon-In,2,Subcon-Out");
            this.comboM.SetDefalutIndex();
            this.comboFactory.SetDefalutIndex(string.Empty);
            this.txtbrand1.MultiSelect = true;
            this.comboProductType1.SetDataSource();
            this.comboFabricType1.SetDataSource();
            this.comboLining1.SetDataSource();
            this.comboGender1.SetDataSource();
            this.comboConstruction1.SetDataSource();
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

            this.mdivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.cdCode = this.txtCDCode.Text;
            this.shift = this.comboShift.SelectedValue.ToString();
            this.brand = this.txtbrand1.Text;
            this.bolSintexEffReportCompare = this.radioSintexEffReportCompare.Checked ? 1 : 0;
            this.productType = this.comboProductType1.Text;
            this.fabricType = this.comboFabricType1.Text;
            this.lining = this.comboLining1.Text;
            this.gender = this.comboGender1.Text;
            this.construction = this.comboConstruction1.Text;
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.printData = new DataTable[3];
            string sqlCmd = string.Format(
                "exec dbo.GetAdidasEfficiencyReport '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}'",
                this.outputDate1.Value.ToString("yyyy/MM/dd"),
                this.outputDate2.Value.ToString("yyyy/MM/dd"),
                this.mdivision,
                this.factory,
                this.cdCode,
                this.shift,
                this.brand,
                this.bolSintexEffReportCompare,
                this.productType,
                this.fabricType,
                this.lining,
                this.gender,
                this.construction);

            DBProxy.Current.DefaultTimeout = 2700;  // timeout時間改為45分鐘

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            List<string> connectionStrings = CentralizedClass.AllFactoryConnectionString();

            if (connectionStrings == null || connectionStrings.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            DualResult result = new DualResult(true);
            foreach (string conString in connectionStrings)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    result = DBProxy.Current.SelectByConn(conn, sqlCmd, null, out DataTable dt);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    if (this.printData[0] == null || this.printData[0].Rows.Count == 0)
                    {
                        this.printData[0] = dt;
                    }
                    else
                    {
                        this.printData[0].Merge(dt);
                    }
                }
            }

            if (this.printData != null && this.printData[0].Rows.Count > 0)
            {
                sqlCmd = @"
select t.OutputDate
	, t.FactoryID
	, t.SewingLineID
	, t.Shift
	, t.Category
	, t.StyleID
	, t.Manpower
	, t.ManHour
	, t.TotalOutput
	, t.CD
    , t.CDCodeNew
	, t.SeasonID
	, t.BrandID
	, t.Fabrication
	, t.ProductGroup
	, t.ProductFabrication
	, [GSD] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0, t.[GSD], (sq.TMS / 60) * (sl.Rate / 100))
	, [Earnedhours] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0 or isnull(t.TotalOutput, 0) = 0, t.[Earnedhours], (sq.TMS / 60) * (sl.Rate / 100) * t.TotalOutput / 60)
	, t.TotalWorkingHours
	, t.CumulateDaysofDaysinProduction
    , t.EfficiencyLine
	, t.GSDProsmv
	, t.Earnedhours2
	, t.EfficiencyLine2
	, t.NoofInlineDefects
	, t.NoofEndlineDefectiveGarments
	, t.WFT
	, t.Country
	, t.[Month]
	, [IsGSDPro] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0, t.IsGSDPro, '')
	, t.Orderseq
    , t.Team
from #tmp t
left join Style s on t.StyleID = s.Id and t.BrandID = s.BrandID and t.SeasonID = s.SeasonID
left join Style_Location sl on s.Ukey = sl.StyleUkey and RIGHT(t.CD, 1) = sl.Location
outer apply (
	select TMS = sum(sq.TMS)
	from Style_Quotation sq
	where s.Ukey = sq.StyleUkey and sq.Article = ''
	and sq.ArtworkTypeID in ('SEWING', 'PRESSING', 'PACKING', 'Seamseal', 'Ultrasonic')
)sq
";

                DBProxy.Current.OpenConnection("Trade", out SqlConnection sqlConnection);
                result = MyUtility.Tool.ProcessWithDatatable(this.printData[0], null, sqlCmd, out this.printData[0], conn: sqlConnection);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            // timeout時間改回5分鐘
            DBProxy.Current.DefaultTimeout = 300;
            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // var tmpDt = this.printData[0];

            // Linq 整理資料
            this.printData = this.OrganizeData(this.printData);

            if (this.printData == null || this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);
            this.ShowWaitMessage("Starting EXCEL...");

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
            Excel.Worksheet objSheets1 = objApp.ActiveWorkbook.Worksheets[1];   // Detail
            Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   // Sintex Eff Report Compare
            Excel.Worksheet objSheets3 = objApp.ActiveWorkbook.Worksheets[3];   // Adidas data

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
                objSheets3.Visible = Excel.XlSheetVisibility.xlSheetHidden;
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
            Marshal.ReleaseComObject(objSheets3);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private DataTable[] OrganizeData(DataTable[] dt)
        {
            DataTable[] rtnDataTable = new DataTable[3];
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
                        Team = x.Field<string>("Team"),
                        Category = x.Field<string>("Category"),
                        StyleID = x.Field<string>("StyleID"),
                        Manpower = x.Field<decimal?>("Manpower"),
                        ManHour = x.Field<decimal?>("ManHour"),
                        TotalOutput = x.Field<int>("TotalOutput"),
                        CD = x.Field<string>("CD"),
                        CDCodeNew = x.Field<string>("CDCodeNew"),
                        SeasonID = x.Field<string>("SeasonID"),
                        BrandID = x.Field<string>("BrandID"),
                        Fabrication = string.Format("=IFERROR(VLOOKUP(LEFT(K{0}, 2),'Adidas data '!$A$2:$H$116, 4, FALSE), \"\")", index + 2),
                        ProductGroup = string.Format("=IFERROR(VLOOKUP(LEFT(K{0}, 2),'Adidas data '!$A$2:$H$116, 7, FALSE), \"\")", index + 2),
                        ProductFabrication = string.Format("=O{0}&N{0}", index + 2),
                        GSD = x.Field<decimal?>("GSD"),
                        Earnedhours = string.Format("=IF(J{0}=\"\", \"\", IFERROR((J{0}*R{0})/60, \"\"))", index + 2),
                        TotalWorkingHours = string.Format("=I{0}*H{0}", index + 2),
                        CumulateDaysofDaysinProduction = x.Field<int>("CumulateDaysofDaysinProduction"),
                        EfficiencyLine = string.Format("=S{0}/T{0}", index + 2),
                        GSDProsmv = x.Field<decimal?>("GSDProsmv"),
                        Earnedhours2 = string.Format("=IF(J{0}=\"\", \"\", IFERROR(J{0}*W{0}/60, \"\"))", index + 2),
                        EfficiencyLine2 = string.Format("=X{0}/T{0}", index + 2),
                        NoofInlineDefects = x.Field<int>("NoofInlineDefects"),
                        NoofEndlineDefectiveGarments = x.Field<int>("NoofEndlineDefectiveGarments"),
                        WFT = string.Format("=IFERROR((Y{0}+Z{0})/I{0}, \"\")", index + 2),
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
                        SIOEarnedHrs = x.Sum(y => y.Field<decimal>("Earnedhours")),
                        SIOManhours = x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                        SamplesSIO = x.Sum(y => y.Field<decimal>("Earnedhours")) == 0 ? 0 : x.Where(y => y.Field<string>("IsGSDPro").EqualString("V")).Sum(y => y.Field<int>("TotalOutput") * y.Field<decimal>("GSD") / 60) /
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
                        ProEff = x.Sum(y => y.Field<decimal>("TotalWorkingHours")) == 0 ? 0 : x.Sum(y => y.Field<decimal>("Earnedhours2")) / x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                        SIOEff = x.Sum(y => y.Field<decimal>("TotalWorkingHours")) == 0 ? 0 : x.Sum(y => y.Field<decimal>("Earnedhours")) / x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                    })
                    .OrderBy(x => x.Country)
                    .ThenBy(x => x.SeasonID)
                    .ToList();

                rtnDataTable[1] = PublicPrg.ListToDataTable.ToDataTable(querySintexReportSeason);

                var querySintexReportMonthByYTD = dt[0].AsEnumerable()
                        .GroupBy(x => new { Country = x.Field<string>("Country") })
                        .Select(x => new
                        {
                            x.Key.Country,
                            SamplesSIO = x.Sum(y => y.Field<decimal>("Earnedhours")) == 0 ? 0 : x.Where(y => y.Field<string>("IsGSDPro").EqualString("V")).Sum(y => y.Field<int>("TotalOutput") * y.Field<decimal>("GSD") / 60) /
                                         x.Sum(y => y.Field<decimal>("Earnedhours")),
                        })
                        .ToList();

                rtnDataTable[2] = PublicPrg.ListToDataTable.ToDataTable(querySintexReportMonthByYTD);
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

            objSheets.get_Range("AB:AE").EntireColumn.Hidden = true;
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

            for (int i = 1; i < sessions.Count; i++)
            {
                Excel.Range r = objSheets.get_Range($"A3", Type.Missing).EntireRow;
                r.Copy();
                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
            }

            for (int i = 0; i <= sessions.Count - 1; i++)
            {
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
                    objSheets.Range[string.Format("{0}{2}:{1}{2}", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2), j + 3)].NumberFormat = "##.##%";
                }
            }

            #endregion

            #region 下半部
            int initR = sessions.Count + 6;
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
                objArray[0, 4] = string.Format("=D{0}/E{0}", initR);
                objArray[0, 5] = dr["SIOEarnedHrs"].ToString();
                objArray[0, 6] = dr["SIOManhours"].ToString();
                objArray[0, 7] = string.Format("=G{0}/H{0}", initR);
                objArray[0, 8] = string.Format("=I{0}-F{0}", initR);
                objArray[0, 9] = dr["SamplesSIO"].ToString();
                objSheets.Range[string.Format("B{0}:K{0}", initR)].Value2 = objArray;
                objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Interior.ColorIndex = this.SetExcelColor(dr["Country"].ToString());
                objSheets.get_Range(string.Format("B{0}:K{0}", initR)).Borders.LineStyle = 1;
                initR++;
                initI++;

                if (initI >= dt[0].Rows.Count || !initCountry.EqualString(dt[0].Rows[initI]["Country"]))
                {
                    int rcount = dt[0].AsEnumerable().Where(x => x.Field<string>("Country").EqualString(dr["Country"].ToString())).Count();
                    DataRow drYTD = dt[2].AsEnumerable().Where(x => x.Field<string>("Country").EqualString(dr["Country"].ToString())).FirstOrDefault();

                    objArray[0, 0] = dr["Country"].ToString();
                    objArray[0, 1] = "YTD";
                    objArray[0, 2] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 2), initR - rcount, initR - 1);
                    objArray[0, 3] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 3), initR - rcount, initR - 1);
                    objArray[0, 4] = string.Format("=IFERROR(ROUND({0}{2}/{1}{2}, 2), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 2), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 3), initR);
                    objArray[0, 5] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 5), initR - rcount, initR - 1);
                    objArray[0, 6] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 6), initR - rcount, initR - 1);
                    objArray[0, 7] = string.Format("=IFERROR(ROUND({0}{2}/{1}{2}, 2), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 5), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 6), initR);
                    objArray[0, 8] = string.Format("=IFERROR({0}{2}-{1}{2}, \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 7), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 4), initR);
                    objArray[0, 9] = drYTD["SamplesSIO"].ToString();
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
