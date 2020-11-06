using System;
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
        private DateTime? outputDate1;
        private DateTime? outputDate2;
        private DataTable printData;
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
            this.comboM.SetDefalutIndex();
            this.comboFactory.SetDefalutIndex(string.Empty);
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
            if (MyUtility.Check.Empty(this.dateOutputDate.Value1) && this.radioDetail.Checked)
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

            DBProxy.Current.DefaultTimeout = 2700;  // timeout時間改為45分鐘

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionStrings = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                if (ss.IndexOf("testing_PMS", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    continue;
                }

                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionStrings.Add(connections);
            }

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

                    if (this.printData == null)
                    {
                        this.printData = dt;
                    }
                    else
                    {
                        this.printData.Merge(dt);
                    }
                }
            }

            if (this.printData != null && this.printData.Rows.Count > 0)
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
	, t.SeasonID
	, t.BrandID
	, t.Fabrication
	, t.ProductGroup
	, t.ProductFabrication
	, [GSD] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0, t.[GSD], cast((sq.TMS / 60) * (sl.Rate / 100) as varchar(100)))
	, t.Earnedhours
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
                result = MyUtility.Tool.ProcessWithDatatable(this.printData, null, sqlCmd, out this.printData, conn: sqlConnection);
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
            if (this.printData == null || this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Starting EXCEL...");

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R07.xltx"); // 預先開啟excel app
            Excel.Worksheet objSheets1 = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   // 取得工作表

            this.SetExcelSheet1(objApp, objSheets1);
            if (this.radioDetail.Checked)
            {
                objSheets2.Visible = Excel.XlSheetVisibility.xlSheetHidden;
            }
            else
            {
                this.SetExcelSheet2(objSheets2);
                objSheets1.get_Range("X:Z").EntireColumn.Hidden = true;
            }

            objSheets2.Columns.AutoFit();

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
            this.HideWaitMessage();
            return true;
        }

        private void SetExcelSheet1(Excel.Application objApp, Excel.Worksheet objSheets)
        {
            int cMax = 100000;
            for (int i = 0; i <= this.printData.Rows.Count / cMax; i++)
            {
                int cSkip = cMax * i;
                int cTake = i == 0 ? i : cSkip;
                MyUtility.Excel.CopyToXls(
                    this.printData.AsEnumerable().Skip(cSkip).Take(cMax).CopyToDataTable(),
                    null,
                    "Planning_R07.xltx",
                    headerRow: cTake + 1,
                    excelApp: objApp,
                    showExcel: false,
                    showSaveMsg: false,
                    wSheet: objSheets);
            }

            objApp.Visible = false;
            objSheets.get_Range("AA:AD").EntireColumn.Hidden = true;
        }

        private void SetExcelSheet2(Excel.Worksheet objSheets)
        {
            List<string> countrys = this.printData.AsEnumerable()
                            .GroupBy(x => new { Country = x.Field<string>("Country"), OrderBySeq = x.Field<int>("Orderseq") })
                            .OrderBy(x => x.Key.OrderBySeq)
                            .Select(x => x.Key.Country)
                            .ToList();

            List<string> sessions = this.printData.AsEnumerable()
                            .GroupBy(x => new { SeasonID = x.Field<string>("SeasonID") })
                            .OrderBy(x => x.Key.SeasonID)
                            .Select(x => x.Key.SeasonID)
                            .ToList();

            var countryAndMonth = this.printData.AsEnumerable()
                            .OrderBy(x => x.Field<int>("Orderseq")).ThenBy(x => x.Field<DateTime?>("OutputDate"))
                            .GroupBy(x => new { Country = x.Field<string>("Country"), Month = x.Field<string>("Month") })
                            .Select(x => new { x.Key.Country, x.Key.Month })
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
                    objArrayTop = new object[1, 3];
                    objArrayTop[0, 0] = string.Format("=IFERROR(SUMIFS(Detail!V:V, Detail!AA:AA, ${0}$1, Detail!K:K, B{1}) / SUMIFS(Detail!R:R, Detail!AA:AA,${0}$1, Detail!K:K, B{1}), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), j + 3);
                    objArrayTop[0, 1] = string.Format("=IFERROR(SUMIFS(Detail!Q:Q, Detail!AA:AA, ${0}$1, Detail!K:K, B{1}) / SUMIFS(Detail!R:R,Detail!AA:AA, ${0}$1, Detail!K:K, B{1}), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), j + 3);
                    objArrayTop[0, 2] = string.Format("=IFERROR(INDIRECT(ADDRESS(ROW(), {0}))-INDIRECT(ADDRESS(ROW(), {1})), \"\")", ((i + 1) * 3) + 1, (i + 1) * 3);
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
            foreach (var item in countryAndMonth)
            {
                if (initCountry.Empty())
                {
                    initCountry = item.Country;
                }

                objArray[0, 0] = item.Country;
                objArray[0, 1] = item.Month;
                objArray[0, 2] = string.Format("=SUMIFS(Detail!V:V, Detail!AA:AA, B{0}, Detail!AB:AB, C{0})", initR);
                objArray[0, 3] = string.Format("=SUMIFS(Detail!R:R, Detail!AA:AA, B{0}, Detail!AB:AB, C{0})", initR);
                objArray[0, 4] = "=IFERROR(ROUND(INDIRECT(ADDRESS(ROW(), 4))/INDIRECT(ADDRESS(ROW(), 5)),2), \"\")";
                objArray[0, 5] = string.Format("=SUMIFS(Detail!Q:Q, Detail!AA:AA, B{0}, Detail!AB:AB, C{0})", initR);
                objArray[0, 6] = string.Format("=SUMIFS(Detail!R:R, Detail!AA:AA, B{0}, Detail!AB:AB, C{0})", initR);
                objArray[0, 7] = "=IFERROR(ROUND(INDIRECT(ADDRESS(ROW(), 7))/INDIRECT(ADDRESS(ROW(), 8)),2), \"\")";
                objArray[0, 8] = "=IFERROR(INDIRECT(ADDRESS(ROW(), 9))-INDIRECT(ADDRESS(ROW(), 6)), \"\")";
                objArray[0, 9] = string.Format("=IFERROR((SUMIFS(Detail!I:I, Detail!AA:AA, B{0}, Detail!AB:AB, C{0}, Detail!AC:AC, \"V\") * SUMIFS(Detail!P:P, Detail!AA:AA, B{0}, Detail!AB:AB, C{0}, Detail!AC:AC, \"V\")) / SUMIFS(Detail!Q:Q, Detail!AA:AA, B{0}, Detail!AB:AB, C{0}, Detail!AC:AC, \"V\"), \"\")", initR);
                objSheets.Range[string.Format("B{0}:K{0}", initR)].Value2 = objArray;
                objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Interior.ColorIndex = this.SetExcelColor(item.Country);
                objSheets.get_Range(string.Format("B{0}:K{0}", initR)).Borders.LineStyle = 1;
                initR++;
                initI++;

                if (initI >= countryAndMonth.Count || !initCountry.EqualString(countryAndMonth[initI].Country))
                {
                    int rcount = countryAndMonth.Where(x => x.Country.EqualString(item.Country)).Count();

                    objArray[0, 0] = item.Country;
                    objArray[0, 1] = "YTD";
                    objArray[0, 2] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 2), initR - rcount, initR - 1);
                    objArray[0, 3] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 3), initR - rcount, initR - 1);
                    objArray[0, 4] = "=IFERROR(ROUND(INDIRECT(ADDRESS(ROW(), 4))/INDIRECT(ADDRESS(ROW(), 5)),2), \"\")";
                    objArray[0, 5] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 5), initR - rcount, initR - 1);
                    objArray[0, 6] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 6), initR - rcount, initR - 1);
                    objArray[0, 7] = "=IFERROR(ROUND(INDIRECT(ADDRESS(ROW(), 7))/INDIRECT(ADDRESS(ROW(), 8)),2), \"\")";
                    objArray[0, 8] = "=IFERROR(INDIRECT(ADDRESS(ROW(), 9))-INDIRECT(ADDRESS(ROW(), 6)), \"\")";
                    objArray[0, 9] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 9), initR - rcount, initR - 1);
                    objSheets.Range[string.Format("B{0}:K{0}", initR)].Value2 = objArray;
                    objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Interior.ColorIndex = this.SetExcelColor(item.Country);
                    objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Font.Bold = true;
                    objSheets.get_Range(string.Format("B{0}:K{0}", initR)).Borders.LineStyle = 1;
                    initCountry = initI >= countryAndMonth.Count ? item.Country : countryAndMonth[initI].Country;
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
