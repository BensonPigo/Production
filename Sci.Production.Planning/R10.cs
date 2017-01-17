using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Production.Report;
//using Sci.Production.Class.Commons;
using System.IO;
using Sci.Utility.Excel;
using Sci.Production.Report.GSchemas;

namespace Sci.Production.Planning
{
    public partial class R10 : Sci.Win.Tems.PrintForm
    {
        string temfile;

        DateTime currentTime = System.DateTime.Now;
        DataTable dtPrint;

        private int ReportType = 1;
        private string BrandID = "";
        private string ArtWorkType = "";
        private bool isSCIDelivery = true;
        private int intYear;
        private int intMonth;
        private string SourceStr;

        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;
            print.Visible = false;
        }


        protected override bool ValidateInput()  //欄位檢核
        {

            if (numYear1.Text == "")
            {
                ShowErr("Year can't be  blank");
                return false;
            }
            if (rdHalfMonth.Checked)
            {
                if (numMonth.Text == "")
                {
                    ShowErr("Month can't be  blank");
                    return false;
                }
            }

            if (!chkOrder.Checked && !chkForecast.Checked && !chkFty.Checked)
            {
                ShowErr("Order, Forecast , Fty Local Order must select one at least ");
                return false;
            }

            ReportType = rdMonth.Checked ? 1 : 2;
            BrandID = txtBrand1.Text;
            ArtWorkType = cbReportType.SelectedValue.ToString();
            isSCIDelivery = (cbDateType.SelectedItem.ToString() == "SCI Delivery") ? true : false;

            intYear = Convert.ToInt32(numYear1.Value);
            intMonth = Convert.ToInt32(numMonth.Value);
            SourceStr = (chkOrder.Checked ? "Order," : "")
                + (chkForecast.Checked ? "Forecast," : "")
                + (chkFty.Checked ? "Fty Local Order," : "");

            return true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (IsFormClosed) return;

            lbMonth.Visible = false;
            numMonth.Visible = false;

            chkOrder.Checked = true;
            chkForecast.Checked = true;
            chkFty.Checked = true;

            numYear1.Value = currentTime.Year;
            numMonth.Value = currentTime.Month;
            numMonth.Visible = false;

            cbDateType.Add("SCI Delivery", "S");
            cbDateType.Add("Buyer Delivery", "B");
            cbDateType.SelectedIndex = 0;

            #region 取得 Report 資料
            string sql = @"Select ID,ID as NAME From ArtworkType WITH (NOLOCK) where ReportDropdown = 1 union Select 'CPU', 'CPU' ";
            DataTable dt_ref = null;
            DualResult result = DBProxy.Current.Select(null, sql, out dt_ref);

            cbReportType.DataSource = dt_ref;
            cbReportType.DisplayMember = "NAME";
            cbReportType.ValueMember = "ID";
            cbReportType.SelectedValue = "SEWING";
            #endregion

        }


        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            try
            {
                DataTable[] datas;
                DualResult res = DBProxy.Current.SelectSP("", "Planning_Report_R02"
                , new List<SqlParameter> { new SqlParameter("@ReportType", ReportType)
                , new SqlParameter("@BrandID", BrandID)
                , new SqlParameter("@ArtWorkType", ArtWorkType)
                , new SqlParameter("@isSCIDelivery", isSCIDelivery)
                , new SqlParameter("@Year", intYear)
                , new SqlParameter("@Month", intMonth)
                , new SqlParameter("@SourceStr", SourceStr)}, out datas);

                if (res)
                {
                    if (ReportType == 1)
                    {
                        transferReport1(datas);
                    }
                    else
                    {
                        transferReport2(datas);
                    }
                }

                return new DualResult(true);
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }

        }

        /// <summary>
        /// Report1，開啟xlt填入資料
        /// </summary>
        private DualResult transferReport1(DataTable[] datas)
        {
            string xltPath = @"Planning_R10_01.xlt";
            SaveXltReportCls sxrc = new SaveXltReportCls(xltPath);
            Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;
            //sxrc.ExcelApp.Visible = true;

            //For Country
            int sheetStart = 5;
            int MDVIdx = 0; //每個MDV所在的Index，抓sheetStart，在Country下面
            int MDVTotalIdx = 0;
            List<string> lisCtyIdx = new List<string>();
            List<string> lisMDVTTLIdx = new List<string>();
            List<string> lisOutputIdx = new List<string>(); //By Country

            DataTable dtList = datas[0];
            DataTable dt0 = datas[1]; //[0] Country Capacity
            DataTable dt1 = datas[2]; //[1] By Factory Capacity
            DataTable dt2 = datas[3]; //[2] non Sister
            DataTable dt3 = datas[4]; //[3] For Forecast shared
            DataTable dt4 = datas[5]; //[4] For Output, 及Output後面的Max日期

            DataTable dtCountryList = dtList.DefaultView.ToTable(true, "CountryID");
            List<string> lisBold = new List<string>();
            List<string> lisPercent = new List<string>();
            for (int idxCty = 0; idxCty < dtCountryList.Rows.Count; idxCty++)
            {
                lisBold.Add(sheetStart.ToString());
                string CountryID = dtCountryList.Rows[idxCty]["CountryID"].ToString();
                DataTable dtCountry = safeGetDt(dt0, string.Format("CountryID = '{0}'", CountryID));
                if (dtCountry.Rows.Count == 0) continue;
                string CountryName = dtCountry.Rows[0]["CountryName"].ToString();

                lisCtyIdx.Add(sheetStart.ToString());
                setTableToRow(wks, sheetStart, CountryName, dtCountry);
                sheetStart += 1;

                DataTable dtMDVList = safeGetDt(dtList, string.Format("CountryID = '{0}'", CountryID)).DefaultView.ToTable(true, "MDivisionID");
                List<string> lisSumFtyNonSis = new List<string>();

                setColumnToBack(dtMDVList, "MDivisionID", "Sample");
                setColumnToBack(dtMDVList, "MDivisionID", "");
                bool isSample = false;
                for (int idxMDV = 0; idxMDV < dtMDVList.Rows.Count; idxMDV++)
                {
                    lisBold.Add(sheetStart.ToString());
                    //3 單一某個MDV加總
                    MDVIdx = sheetStart;
                    string MDivisionID = dtMDVList.Rows[idxMDV]["MDivisionID"].ToString();

                    isSample = MDivisionID == "Sample";

                    DataTable dtOneMDV = safeGetDt(dtCountry, string.Format("MDivisionID = '{0}'", MDivisionID));
                    setTableToRow(wks, sheetStart, MDivisionID, dtOneMDV);
                    sheetStart += 1;

                    //4 Factory Data，這裡需要迴圈For每個工廠
                    DataTable dtFactory = safeGetDt(dt1, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", CountryID, MDivisionID));
                    DataTable dtFactoryList = safeGetDt(dtList, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", CountryID, MDivisionID)).DefaultView.ToTable(true, "FactoryID");
                    int ftyStart = sheetStart;
                    foreach (DataRow row in dtFactoryList.Rows)
                    {
                        string FactoryID = row["FactoryID"].ToString();
                        wks.Cells[sheetStart, 1].Value = FactoryID;

                        for (int mon = 1; mon < 13; mon++)
                        {
                            DataRow[] rows = dtFactory.Select(string.Format("Month = '{0}' and FactoryID = '{1}'", mon.ToString("00"), FactoryID));
                            wks.Cells[sheetStart, mon + 1].Value = (rows.Length > 0) ? rows[0]["Capacity"] : 0;
                        }
                        wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);
                        var tms = dtFactory.Select(string.Format("FactoryID = '{0}'", FactoryID))[0]["Tms"];
                        wks.Cells[sheetStart, 15].Value = tms == DBNull.Value ? 0 : tms;
                        sheetStart += 1;
                    }

                    //5 By non-sister
                    int nonSisStart = sheetStart;
                    DataTable dtByNonSister = safeGetDt(dt2, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", CountryID, MDivisionID));
                    setTableToRow(wks, sheetStart, "non-sister sub-in", dtByNonSister);
                    drawBottomLine(wks, sheetStart, 1);
                    sheetStart += 1;

                    if (isSample)
                        continue;

                    //MDV total
                    MDVTotalIdx = sheetStart;
                    setFormulaToRow(wks, sheetStart, MDivisionID + " total", string.Format("=SUM({{0}}{0}:{{0}}{1})", ftyStart, nonSisStart));

                    drawBottomLine(wks, sheetStart, 1);
                    sheetStart += 1;

                    lisSumFtyNonSis.Add(ftyStart.ToString() + "," + nonSisStart.ToString());

                    //6 ForecastCapacity
                    lisPercent.Add(sheetStart.ToString());
                    DataTable dtForecastCapacityByMDV = safeGetDt(dt3, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", CountryID, MDivisionID));
                    wks.Cells[sheetStart, 1].Value = string.Format("{0} Forecast shared", MDivisionID);
                    for (int mon = 1; mon < 13; mon++)
                    {
                        var ForCapa = dtForecastCapacityByMDV.Compute("SUM(Capacity)", string.Format("Month = '{0}'", mon.ToString("00")));
                        ForCapa = (ForCapa == DBNull.Value) ? 0 : ForCapa;
                        wks.Cells[sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), MDVTotalIdx, ForCapa);
                    }
                    var sumforcapaMDV = dtForecastCapacityByMDV.Compute("SUM(Capacity)", "");
                    wks.Cells[sheetStart, 14].Value = string.Format("=({0}) / SUM({1}{3}:{2}{3})", (sumforcapaMDV == DBNull.Value) ? 0 : sumforcapaMDV, MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), MDVTotalIdx);
                    sheetStart += 1;

                    //MDV 1 Loading - CAPA
                    setFormulaToRow(wks, sheetStart, string.Format("{0} Loading - CAPA", MDivisionID), string.Format("=({{0}}{0} - {{0}}{1})", MDVTotalIdx, MDVIdx));
                    sheetStart += 1;


                    //MDV FILL RATE
                    lisPercent.Add(sheetStart.ToString());
                    setFormulaToRow(wks, sheetStart, string.Format("{0} FILL RATE", MDivisionID), string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", MDVIdx, MDVTotalIdx));

                    drawBottomLine(wks, sheetStart, 1);
                    sheetStart += 1;

                    // Max(OutputDate)
                    // Order+FactoryOrder 的 SewCapacity
                    DataTable dtOutputMDV = safeGetDt(dt4, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", CountryID, MDivisionID));
                    string MaxSewOutPut = dtOutputMDV.Compute("MAX(Month)", "").ToString();
                    setTableToRow(wks, sheetStart, string.Format("{0} Output ({1})", MDivisionID, MaxSewOutPut), dtOutputMDV);
                    sheetStart += 1;

                    //MDV Output  Rate
                    setFormulaToRow(wks, sheetStart, string.Format("{0} Output  Rate", MDivisionID), string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", MDVTotalIdx, sheetStart - 1));

                    drawBottomLine(wks, sheetStart, 2);

                    sheetStart += 1;

                }

                //CountryID Grand TTL
                MDVTotalIdx = sheetStart;
                lisMDVTTLIdx.Add(sheetStart.ToString());
                string sumFtyStr = "=";
                foreach (string str in lisSumFtyNonSis)
                {
                    sumFtyStr += string.Format("+SUM({{0}}{0}:{{0}}{1})", str.Split(',')[0], str.Split(',')[1]);
                }
                setFormulaToRow(wks, sheetStart, string.Format("{0} Grand TTL", CountryID), sumFtyStr);

                drawBottomLine(wks, sheetStart, 1);
                sheetStart += 1;

                //CountryID Forecast shared
                lisPercent.Add(sheetStart.ToString());
                DataTable dtForecastCapacityByCty = safeGetDt(dt3, string.Format("CountryID = '{0}'", CountryID));
                wks.Cells[sheetStart, 1].Value = string.Format("{0} Forecast shared", CountryID);
                for (int mon = 1; mon < 13; mon++)
                {
                    var ForCapa = dtForecastCapacityByCty.Compute("SUM(Capacity)", string.Format("Month = '{0}'", mon.ToString("00")));
                    ForCapa = (ForCapa == DBNull.Value) ? 0 : ForCapa;
                    wks.Cells[sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), MDVTotalIdx, ForCapa);
                }
                var sumforcapaCty = dtForecastCapacityByCty.Compute("SUM(Capacity)", "");
                wks.Cells[sheetStart, 14].Value = string.Format("=({0}) / SUM({1}{3}:{2}{3})", (sumforcapaCty == DBNull.Value) ? 0 : sumforcapaCty, MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), MDVTotalIdx);
                sheetStart += 1;


                //CountryID Loading - CAPA
                setFormulaToRow(wks, sheetStart, string.Format("{0} Loading - CAPA", CountryID), string.Format("=({{0}}{0} - {{0}}{1})", sheetStart - 2, lisCtyIdx[lisCtyIdx.Count - 1]));
                sheetStart += 1;


                //CountryID FILL Rate
                lisPercent.Add(sheetStart.ToString());
                setFormulaToRow(wks, sheetStart, string.Format("{0} FILL Rate", CountryID), string.Format("=IF({{0}}{1}>0,{{0}}{0}/{{0}}{1},0)", sheetStart - 2, lisCtyIdx[lisCtyIdx.Count - 1]));
                sheetStart += 1;

                //CountryID Output()
                lisOutputIdx.Add(sheetStart.ToString());
                DataTable dtOutputCty = safeGetDt(dt4, string.Format("CountryID = '{0}'", CountryID));
                string MaxSewOutPutCty = dtOutputCty.Compute("MAX(Month)", "").ToString();
                setTableToRow(wks, sheetStart, string.Format("{0} Output ({1})", CountryID, MaxSewOutPutCty), dtOutputCty);
                sheetStart += 1;

                //CountryID Output  Rate
                setFormulaToRow(wks, sheetStart, string.Format("{0} Output  Rate", CountryID), string.Format("=IF({0}{1} > 0, {0}{2} / {0}{1},0)", "{0}", MDVTotalIdx, sheetStart - 1));

                drawBottomLine(wks, sheetStart, 3);

                sheetStart += 1;

            }


            //Total Capacity
            lisBold.Add(sheetStart.ToString());
            string TotalStr = "=";
            foreach (string str in lisCtyIdx)
            {
                TotalStr += string.Format("+{0}{1}", "{0}", str);
            }
            setFormulaToRow(wks, sheetStart, "Total Capacity", TotalStr);

            sheetStart += 1;


            //Total Loading - CAPA
            lisBold.Add(sheetStart.ToString());
            string TotalLoadStr = "=";
            foreach (string str in lisMDVTTLIdx)
            {
                TotalLoadStr += string.Format("+{0}{1}", "{0}", str);
            }
            setFormulaToRow(wks, sheetStart, "Total Loading", TotalLoadStr);

            sheetStart += 1;


            //Total Forecast shared
            lisBold.Add(sheetStart.ToString());
            lisPercent.Add(sheetStart.ToString());
            DataTable dtForecastCapacity = dt3;
            wks.Cells[sheetStart, 1].Value = "Total FC shared";
            for (int mon = 1; mon < 14; mon++)
            {
                var ForCapa = dtForecastCapacity.Compute("SUM(Capacity)", string.Format("Month = '{0}'", mon.ToString("00")));
                ForCapa = (ForCapa == DBNull.Value) ? 0 : ForCapa;
                wks.Cells[sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), sheetStart - 1, ForCapa);
            }
            sheetStart += 1;

            //Total Loading - CAPA
            lisBold.Add(sheetStart.ToString());
            setFormulaToRow(wks, sheetStart, "Loading-CAPA", string.Format("=({0}{1} - {0}{2})", "{0}", sheetStart - 2, sheetStart - 3));
            sheetStart += 1;

            //FILL Rate
            lisBold.Add(sheetStart.ToString());
            lisPercent.Add(sheetStart.ToString());
            setFormulaToRow(wks, sheetStart, "FIll RATE", string.Format("=IF({0}{2}>0,{0}{1}/{0}{2},0)", "{0}", sheetStart - 3, sheetStart - 4));
            sheetStart += 1;

            //Output()
            lisBold.Add(sheetStart.ToString());
            string OutPutStr = "=";
            foreach (string str in lisOutputIdx)
            {
                OutPutStr += string.Format("+{{0}}{0}", str);
            }

            DataTable dtOutput = dt4;
            string MaxSewOutPutT = dtOutput.Compute("MAX(Month)", "").ToString();
            setFormulaToRow(wks, sheetStart, string.Format("Output ({0})", MaxSewOutPutT), OutPutStr);
            sheetStart += 1;

            //Output  Rate
            lisBold.Add(sheetStart.ToString());
            setFormulaToRow(wks, sheetStart, "Output  Rate", string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", sheetStart - 5, sheetStart - 1));



            //第一排置中
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + "5", MyExcelPrg.GetExcelColumnName(1) + sheetStart.ToString());
            rg.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            //Country, MDvision, Total 第一格粗體
            foreach (string idx in lisBold)
            {
                string rgStr = string.Format("{0}{1}:{0}{1}", MyExcelPrg.GetExcelColumnName(1), idx);
                rg = wks.get_Range(rgStr);
                rg.Font.Bold = true;
            }

            ////數值格式
            string lastCell = MyExcelPrg.GetExcelColumnName(15) + sheetStart.ToString();
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(2) + "5", lastCell);
            rg.Cells.NumberFormat = "##,###,##0";

            //Total欄左右邊線
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(14) + "5", MyExcelPrg.GetExcelColumnName(14) + sheetStart.ToString());
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            foreach (string idx in lisPercent)
            {
                string rgStr = string.Format("{0}{2}:{1}{2}", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(15), idx);
                rg = wks.get_Range(rgStr);
                rg.Cells.NumberFormat = "###,##0.00%";
            }

            //第一欄位Auto Fit
            rg = wks.get_Range("A:A");
            rg.Columns.AutoFit();

            sxrc.dicDatas.Add("##Year", intYear);
            sxrc.dicDatas.Add("##Month", intMonth);
            sxrc.dicDatas.Add("##ArtworkType", ArtWorkType == "CPU" ? ArtWorkType : ArtWorkType + " TMS/Min");
            sxrc.dicDatas.Add("##Source", SourceStr);
            sxrc.dicDatas.Add("##Brand", BrandID);
            string dTypeStr = isSCIDelivery ? "Sci Delivery" : "Buyer Delivery";
            sxrc.dicDatas.Add("##DateType", dTypeStr);

            sxrc.Save();


            wks = null;
            sxrc = null;
            GC.Collect();

            return Result.True;
        }

        /// <summary>
        /// Report2，開啟xlt填入資料
        /// </summary>
        private DualResult transferReport2(DataTable[] datas)
        {
            string xltPath = @"Planning_R10_02.xlt";
            SaveXltReportCls sxrc = new SaveXltReportCls(xltPath);
            Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;
            //sxrc.ExcelApp.Visible = true;

            int sheetStart = 5;

            //Set Header
            DateTime startDate = new DateTime(intYear, intMonth, 1);
            for (int mon = 0; mon < 6; mon++)
            {
                DateTime nextDate = startDate.AddMonths(mon);
                wks.Cells[sheetStart - 1, 5 + mon * 2].Value = new DateTime(nextDate.Year, nextDate.Month, 22).ToShortDateString();
                wks.Cells[sheetStart - 1, 6 + mon * 2].Value = new DateTime(nextDate.Year, nextDate.Month, 7).AddMonths(1).ToShortDateString();
            }

            DataTable dtList = datas[0];
            DataTable dt0 = datas[1]; //[0] By Factory 最細的上下半月Capacity
            DataTable dt1 = datas[2]; //[1] By Factory Loading CPU
            DataTable dt2 = datas[3]; //[2] For Forecast shared

            DataTable dtCountryList = dtList.DefaultView.ToTable(true, "CountryID");
            List<string> lisPercent = new List<string>();

            //For Country
            for (int idxCty = 0; idxCty < dtCountryList.Rows.Count; idxCty++)
            {
                string CountryID = dtCountryList.Rows[idxCty]["CountryID"].ToString();
                DataTable dtCountry = safeGetDt(dt0, string.Format("CountryID = '{0}'", CountryID));
                if (dtCountry.Rows.Count == 0) continue;
                string CountryName = dtCountry.Rows[0]["CountryName"].ToString();
                wks.Cells[sheetStart, 1].Value = CountryName;

                DataTable dtMDVList = safeGetDt(dtList, string.Format("CountryID = '{0}'", CountryID)).DefaultView.ToTable(true, "MDivisionID");

                List<string> lisCapaCty = new List<string>();
                List<string> lisLoadingCty = new List<string>();
                setColumnToBack(dtMDVList, "MDivisionID", "Sample");
                setColumnToBack(dtMDVList, "MDivisionID", "");
                int idx = 0;

                for (int idxMDV = 0; idxMDV < dtMDVList.Rows.Count; idxMDV++)
                {
                    string MDivisionID = dtMDVList.Rows[idxMDV]["MDivisionID"].ToString();
                    wks.Cells[sheetStart, 2].Value = MDivisionID;

                    DataTable dtOneMDV = safeGetDt(dtCountry, string.Format("MDivisionID = '{0}'", MDivisionID));
                    DataTable dtFactory = safeGetDt(dtOneMDV, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", CountryID, MDivisionID));
                    DataTable dtFactoryList = safeGetDt(dtList, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", CountryID, MDivisionID)).DefaultView.ToTable(true, "FactoryID");
                    int ftyStart = sheetStart;
                    List<string> lisCapa = new List<string>();
                    List<string> lisLoading = new List<string>();
                    foreach (DataRow row in dtFactoryList.Rows)
                    {
                        string FactoryID = row["FactoryID"].ToString();
                        wks.Cells[sheetStart, 3].Value = FactoryID;
                        wks.Cells[sheetStart, 4].Value = "Capa.";
                        lisCapa.Add(sheetStart.ToString());
                        lisCapaCty.Add(sheetStart.ToString());
                        idx = 0;
                        for (int mon = intMonth; mon < intMonth + 6; mon++)
                        {
                            DataRow[] rows = dtFactory.Select(string.Format("FactoryID = '{0}' and MONTH = '{1}'", FactoryID, getCurrMonth(mon)));
                            decimal Capacity1 = 0;
                            decimal Capacity2 = 0;
                            if (rows.Length > 0)
                            {
                                Capacity1 = rows[0]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity1"]) : 0;
                                Capacity2 = rows[0]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity2"]) : 0;
                            }
                            wks.Cells[sheetStart, 5 + idx * 2].Value = Capacity1;
                            wks.Cells[sheetStart, 5 + idx * 2 + 1].Value = Capacity2;
                            idx += 1;
                        }
                        wks.Cells[sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), sheetStart);

                        sheetStart += 1;

                        lisLoading.Add(sheetStart.ToString());
                        lisLoadingCty.Add(sheetStart.ToString());
                        wks.Cells[sheetStart, 4].Value = "Load.";
                        idx = 0;
                        DataTable dtLoadCPU = safeGetDt(dt1, string.Format("CountryID = '{0}' And MDivisionID = '{1}' and FactoryID = '{2}'", CountryID, MDivisionID, FactoryID));
                        for (int mon = intMonth; mon < intMonth + 6; mon++)
                        {
                            DataRow[] rows = dtLoadCPU.Select(string.Format("MONTH = '{0}'", getCurrMonth(mon)));
                            decimal Capacity1 = 0;
                            decimal Capacity2 = 0;
                            if (rows.Length > 0)
                            {
                                Capacity1 = rows[0]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity1"]) : 0;
                                Capacity2 = rows[0]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity2"]) : 0;
                            }
                            wks.Cells[sheetStart, 5 + idx * 2].Value = Capacity1;
                            wks.Cells[sheetStart, 5 + idx * 2 + 1].Value = Capacity2;
                            idx += 1;
                        }
                        wks.Cells[sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), sheetStart);

                        sheetStart += 1;

                        wks.Cells[sheetStart, 4].Value = "Vari.";
                        for (int i = 5; i <= 17; i++)
                        {
                            string str = string.Format("={0}{1} - {0}{2}", MyExcelPrg.GetExcelColumnName(i), sheetStart - 1, sheetStart - 2);
                            wks.Cells[sheetStart, i] = str;
                        }

                        drawBottomLine(wks, sheetStart, 4, 3, 17);

                        sheetStart += 1;

                    }

                    //Total Capa.
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Capa.", MDivisionID);
                    string totalCapa = "=";
                    for (int i = 0; i < lisCapa.Count; i++)
                    {
                        totalCapa += string.Format("+{{0}}{0}", lisCapa[i]);
                    }
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format(totalCapa, MyExcelPrg.GetExcelColumnName(i));
                        wks.Cells[sheetStart, i] = str;
                    }
                    sheetStart += 1;

                    //Total Load.
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Load.", MDivisionID);
                    string totalLoad = "=";
                    for (int i = 0; i < lisLoading.Count; i++)
                    {
                        totalLoad += string.Format("+{{0}}{0}", lisLoading[i]);
                    }
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format(totalLoad, MyExcelPrg.GetExcelColumnName(i));
                        wks.Cells[sheetStart, i] = str;
                    }
                    sheetStart += 1;

                    //6 ForecastCapacity
                    lisPercent.Add(sheetStart.ToString());
                    idx = 0;
                    DataTable dtForecastCapacityByMDV = safeGetDt(dt2, string.Format("CountryID = '{0}' And MDivisionID = '{1}' ", CountryID, MDivisionID));
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total FC Shared", MDivisionID);
                    for (int mon = intMonth; mon < intMonth + 6; mon++)
                    {
                        DataRow[] rows = dtForecastCapacityByMDV.Select(string.Format("MONTH = '{0}'", getCurrMonth(mon)));
                        decimal Capacity1 = 0;
                        decimal Capacity2 = 0;
                        if (rows.Length > 0)
                        {
                            Capacity1 = rows[0]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity1"]) : 0;
                            Capacity2 = rows[0]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity2"]) : 0;
                        }
                        wks.Cells[sheetStart, 5 + idx * 2].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + idx * 2), sheetStart - 1, Capacity1);
                        wks.Cells[sheetStart, 5 + idx * 2 + 1].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + idx * 2 + 1), sheetStart - 1, Capacity2);
                        idx += 1;
                    }
                    var sumforcapaMDV = dtForecastCapacityByMDV.Compute("SUM(Capacity1)+SUM(Capacity2)", "");
                    wks.Cells[sheetStart, 17] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(17), sheetStart - 1, (sumforcapaMDV == DBNull.Value) ? 0 : sumforcapaMDV);

                    sheetStart += 1;



                    //Total Vari.
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Vari.", MDivisionID);
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format("={0}{1} - {0}{2}", MyExcelPrg.GetExcelColumnName(i), sheetStart - 2, sheetStart - 3);
                        wks.Cells[sheetStart, i] = str;
                    }
                    sheetStart += 1;

                    //Total Fill Rate
                    lisPercent.Add(sheetStart.ToString());
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Fill Rate", MDivisionID);
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format("=IF({0}{2}>0,{0}{1} / {0}{2},0)", MyExcelPrg.GetExcelColumnName(i), sheetStart - 3, sheetStart - 4);
                        wks.Cells[sheetStart, i] = str;
                    }

                    drawBottomLine(wks, sheetStart, 4, 2, 17);

                    sheetStart += 1;
                }

                //Country Total Capa.
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Capa.", CountryID);
                string totalCapaCty = "={0}";
                totalCapaCty += string.Join("+{0}", lisCapaCty);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format(totalCapaCty, MyExcelPrg.GetExcelColumnName(i));
                    wks.Cells[sheetStart, i] = str;
                }
                sheetStart += 1;

                //Country Total Load.
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Load.", CountryID);
                string totalLoadCty = "={0}";
                totalLoadCty += string.Join("+{0}", lisLoadingCty);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format(totalLoadCty, MyExcelPrg.GetExcelColumnName(i));
                    wks.Cells[sheetStart, i] = str;
                }
                sheetStart += 1;


                //Country FC Shared
                lisPercent.Add(sheetStart.ToString());
                idx = 0;
                DataTable dtForecastCapacity = safeGetDt(dt2, string.Format("CountryID = '{0}'", CountryID));
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total FC Shared", CountryID);
                for (int mon = intMonth; mon < intMonth + 6; mon++)
                {
                    DataRow[] rows = dtForecastCapacity.Select(string.Format("MONTH = '{0}'", getCurrMonth(mon)));
                    decimal Capacity1 = 0;
                    decimal Capacity2 = 0;
                    if (rows.Length > 0)
                    {
                        Capacity1 = rows[0]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity1"]) : 0;
                        Capacity2 = rows[0]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity2"]) : 0;
                    }
                    wks.Cells[sheetStart, 5 + idx * 2].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + idx * 2), sheetStart - 1, Capacity1);
                    wks.Cells[sheetStart, 5 + idx * 2 + 1].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + idx * 2 + 1), sheetStart - 1, Capacity2);
                    idx += 1;
                }
                var sumforcapaMDVCty = dtForecastCapacity.Compute("SUM(Capacity1)+SUM(Capacity2)", "");
                wks.Cells[sheetStart, 17] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(17), sheetStart - 1, (sumforcapaMDVCty == DBNull.Value) ? 0 : sumforcapaMDVCty);

                sheetStart += 1;

                //Country Total Vari.
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Vari.", CountryID);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format("={0}{1} - {0}{2}", MyExcelPrg.GetExcelColumnName(i), sheetStart - 2, sheetStart - 3);
                    wks.Cells[sheetStart, i] = str;
                }
                sheetStart += 1;

                //Country Total Fill Rate
                lisPercent.Add(sheetStart.ToString());
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Fill Rate", CountryID);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format("=IF({0}{2}>0,{0}{1} / {0}{2},0)", MyExcelPrg.GetExcelColumnName(i), sheetStart - 3, sheetStart - 4);
                    wks.Cells[sheetStart, i] = str;
                }

                drawBottomLine(wks, sheetStart, 5, 1, 17);

                sheetStart += 1;

            }

            sheetStart -= 1;

            //欄位以直線區隔
            string lastCell = MyExcelPrg.GetExcelColumnName(17) + sheetStart.ToString();
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + "5", lastCell);
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;

            //數值格式
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(5) + "5", lastCell);
            rg.Cells.NumberFormat = "##,###,##0";

            foreach (string idx in lisPercent)
            {
                string rgStr = string.Format("{0}{2}:{1}{2}", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(17), idx);
                rg = wks.get_Range(rgStr);
                rg.Cells.NumberFormat = "###,##0.00%";
            }

            sxrc.dicDatas.Add("##Year", intYear);
            sxrc.dicDatas.Add("##Month", intMonth);
            sxrc.dicDatas.Add("##ArtworkType", ArtWorkType == "CPU" ? ArtWorkType : ArtWorkType + " TMS/Min");
            string dTypeStr = isSCIDelivery ? "Sci Delivery" : "Buyer Delivery";
            sxrc.dicDatas.Add("##DateType", dTypeStr);

            sxrc.Save();

            wks = null;
            sxrc = null;
            GC.Collect();

            return Result.True;

        }

        private string getCurrMonth(int month)
        {
            if (month == 12) return month.ToString("00");
            return (month % 12).ToString("00");
        }

        void setColumnToBack(DataTable dt, string column, string value)
        {
            int idx = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][column].ToString() == value)
                {
                    idx = i;
                    break;
                }
            }
            if (idx != -1)
            {
                DataRow r = dt.NewRow();
                r.ItemArray = dt.Rows[idx].ItemArray;
                dt.Rows.RemoveAt(idx);
                dt.Rows.Add(r);
            }
        }

        private void drawBottomLine(Microsoft.Office.Interop.Excel.Worksheet wks, int sheetStart, int LineType, int sIdx = 1, int eIdx = 15)
        {
            string rgStr = string.Format("{0}{2}:{1}{2}", MyExcelPrg.GetExcelColumnName(sIdx), MyExcelPrg.GetExcelColumnName(eIdx), sheetStart);

            if (LineType == 1)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 1.5;
            }
            if (LineType == 2)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDashDot;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
            }
            if (LineType == 3)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 4;
            }

            if (LineType == 4)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            }

            if (LineType == 5)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 4;
            }
        }

        /// <summary>
        /// For簡化Code用
        /// </summary>
        private void setTableToRow(Microsoft.Office.Interop.Excel.Worksheet wks, int sheetStart, string Cell1Str, DataTable dt)
        {
            wks.Cells[sheetStart, 1].Value = Cell1Str;
            for (int mon = 1; mon < 13; mon++)
            {
                DataRow[] rows = dt.Select(string.Format("Month = '{0}'", mon.ToString("00")));
                decimal v = 0;
                if (rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        v += Convert.ToDecimal(rows[i]["Capacity"]);
                    }
                }
                wks.Cells[sheetStart, mon + 1].Value = v;
            }
            wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);
        }

        private void setFormulaToRow(Microsoft.Office.Interop.Excel.Worksheet wks, int sheetStart, string Cell1Str, string formula)
        {
            wks.Cells[sheetStart, 1].Value = Cell1Str;
            for (int i = 2; i <= 14; i++)
            {
                string str = string.Format(formula, MyExcelPrg.GetExcelColumnName(i));
                wks.Cells[sheetStart, i] = str;
            }
            //wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);

        }

        private DataTable safeGetDt(DataTable dt, string filterStr)
        {
            DataRow[] rows = dt.Select(filterStr);
            DataTable dtOutput = (rows.Length > 0) ? rows.CopyToDataTable() : dt.Clone();
            return dtOutput;
        }



        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            DualResult result = Result.True;
            ShowInfo("報表查詢完成"); //自動開啟Excel存檔畫面             
            return true;
        }

        private void rdMonth_CheckedChanged(object sender, EventArgs e)
        {
            lbMonth.Visible = !rdMonth.Checked;
            numMonth.Visible = !rdMonth.Checked;

            if (rdMonth.Checked)
            {
                numMonth.Value = 0;
            }
        }

        private void rdHalfMonth_CheckedChanged(object sender, EventArgs e)
        {
            lbMonth.Visible = rdHalfMonth.Checked;
            numMonth.Visible = rdHalfMonth.Checked;

            if (rdHalfMonth.Checked)
            {
                numMonth.Value = System.DateTime.Today.Month;
            }

        }

    }
}
