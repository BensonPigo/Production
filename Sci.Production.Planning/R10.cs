using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using Sci.Win;
using Sci.Utility.Excel;

// 此程式前兩個用統計和半月統計是複製Trade Planning R02
namespace Sci.Production.Planning
{
    /// <summary>
    /// R10
    /// </summary>
    public partial class R10 : Sci.Win.Tems.PrintForm
    {
        private DateTime currentTime = System.DateTime.Now;

        private int ReportType = 1;
        private string BrandID = string.Empty;
        private string ArtWorkType = string.Empty;
        private bool isSCIDelivery = true;
        private int intYear;
        private int intMonth;
        private string SourceStr;
        private int sheetStart = 5;

        private string M;
        private string Fty;

        string title = string.Empty;
        private string cmd;
        private string cmd2;
        private string cmd3;
        private string dStart;
        private string dEnd;
        private string LastDay;
        private string dLoad;
        private string STARTday;
        private string endDay;
        private string o;
        private string s;
        private string b;
        private DataTable dt;
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        private DualResult dtresult;

        private System.Data.DataTable[] dt2;
        private System.Data.DataTable dt3fty;
        private System.Data.DataTable dt2Factory = null;
        private System.Data.DataTable dt2All = null;

        /// <summary>
        /// R10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.print.Visible = false;
            this.txtM.Text = Sci.Env.User.Keyword;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (this.numYear.Text == string.Empty)
            {
                this.ShowErr("Year can't be  blank");
                return false;
            }

            if (this.radioSemimonthlyReport.Checked)
            {
                if (this.numMonth.Text == string.Empty)
                {
                    this.ShowErr("Month can't be  blank");
                    return false;
                }
            }

            if (!this.checkOrder.Checked && !this.checkForecast.Checked && !this.checkFty.Checked)
            {
                this.ShowErr("Order, Forecast , Fty Local Order must select one at least ");
                return false;
            }

            this.ReportType = this.radioMonthlyReport.Checked ? 1 : 2;
            this.BrandID = this.txtBrand.Text;
            this.ArtWorkType = this.comboReport.SelectedValue.ToString();
            this.isSCIDelivery = (this.comboDate.SelectedItem.ToString() == "SCI Delivery") ? true : false;
            this.M = this.txtM.Text;
            this.Fty = this.txtFactory.Text;

            this.intYear = Convert.ToInt32(this.numYear.Value);
            this.intMonth = Convert.ToInt32(this.numMonth.Value);
            this.SourceStr = (this.checkOrder.Checked ? "Order," : string.Empty)
                + (this.checkForecast.Checked ? "Forecast," : string.Empty)
                + (this.checkFty.Checked ? "Fty Local Order," : string.Empty);
            return true;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (IsFormClosed) return;

            labelMonth.Visible = false;
            numMonth.Visible = false;

            checkOrder.Checked = true;
            checkForecast.Checked = true;
            checkFty.Checked = true;

            numYear.Value = currentTime.Year;
            numMonth.Value = currentTime.Month;
            numMonth.Visible = false;

            comboDate.Add("SCI Delivery", "S");
            comboDate.Add("Buyer Delivery", "B");
            comboDate.SelectedIndex = 0;

            #region 取得 Report 資料
            string sql = @"Select ID,ID as NAME, SEQ From ArtworkType WITH (NOLOCK) where ReportDropdown = 1 union Select 'All', 'ALL', '0000' order by SEQ";
            DataTable dt_ref = null;
            DualResult result = DBProxy.Current.Select(null, sql, out dt_ref);

            comboReport.DataSource = dt_ref;
            comboReport.DisplayMember = "NAME";
            comboReport.ValueMember = "ID";
            comboReport.SelectedValue = "SEWING";
            #endregion

        }


        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (radioMonthlyReport.Checked || radioSemimonthlyReport.Checked)
            {
                DualResult result = Result.True;
                try
                {
                    List<string> ArtworkLis = new List<string>();
                    
                    if (ArtWorkType == "All")
                    {
                        DataTable dt = (DataTable)comboReport.DataSource;
                        ArtworkLis = dt.AsEnumerable()
                            .Where(row => row.Field<string>("ID") != "All")
                            .Select(row => row.Field<string>("ID").ToString()).ToList();
                    }
                    else
                    {
                        ArtworkLis.Add(ArtWorkType);
                    }


                    string xltPath = "";
                    string strHeaderRange = "";
                    if (ReportType == 1)
                    {
                        xltPath = @"Planning_R10_01.xltx";
                        strHeaderRange = "A2:O4";
                    }
                    else
                    {
                        xltPath = @"Planning_R10_02.xltx";
                        strHeaderRange = "A2:Q4";
                    }
                    SaveXltReportCls sxrc = new SaveXltReportCls(xltPath);
                    Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;

                    Dictionary<string, DataTable[]> dic = new Dictionary<string, DataTable[]>();
                    foreach (string art in ArtworkLis)
                    {
                        DataTable[] datas;
                        DualResult res = DBProxy.Current.SelectSP("", "Planning_Report_R10"
                            , new List<SqlParameter> { new SqlParameter("@ReportType", ReportType)
                        , new SqlParameter("@BrandID", BrandID)
                        , new SqlParameter("@ArtWorkType", art)
                        , new SqlParameter("@isSCIDelivery", isSCIDelivery)
                        , new SqlParameter("@Year", intYear)
                        , new SqlParameter("@Month", intMonth)
                        , new SqlParameter("@SourceStr", SourceStr)
                        , new SqlParameter("@M", M)
                        , new SqlParameter("@Fty", Fty)                            
                            }, out datas);

                        if (res && datas[1].Rows.Count > 0)
                        {
                            dic.Add(art, datas);
                        }
                        else
                        {
                            dic.Add(art, null);
                        }
                    }

                    sheetStart = 5; //起始位置
                    int ArtWorkStart = 2;


#if DEBUG
                sxrc.ExcelApp.Visible = true;
#endif

                    foreach (string art in ArtworkLis)
                    {
                        //CopyHeader
                        if (ArtworkLis.IndexOf(art) > 0)
                        {
                            ArtWorkStart = sheetStart;

                            Microsoft.Office.Interop.Excel.Range desRg = wks.get_Range(string.Format("A{0}:A{0}", sheetStart.ToString()));
                            wks.get_Range(strHeaderRange).Copy();
                            desRg.PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAll); //Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAllExceptBorders
                            sheetStart += 3;
                        }

                        if (dic[art] != null)
                        {
                            if (ReportType == 1)
                            {
                                transferReport1(dic[art], sxrc.ExcelApp.ActiveSheet);
                            }
                            else
                            {
                                transferReport2(dic[art], sxrc.ExcelApp.ActiveSheet);
                            }
                        }
                        else
                        {
                            wks.Cells[sheetStart, 1].Value = string.Format("{0} data not found.", art);
                            //載入失敗
                        }

                        //修改Header
                        if (ReportType == 1)
                        {
                            wks.Cells[ArtWorkStart, 1].Value = string.Format("Factory Capacity by Month Report  {0}", (art == "CPU" ? art : art + " TMS/Min"));
                            wks.Cells[ArtWorkStart + 1, 1].Value = string.Format("Year:{0}", intYear);
                            wks.Cells[ArtWorkStart + 1, 3].Value = string.Format("Print Type:< {0} >", SourceStr);
                            wks.Cells[ArtWorkStart + 1, 8].Value = string.Format("By {0}                             Buyer : {1}"
                                , (isSCIDelivery ? "Sci Delivery" : "Buyer Delivery")
                                , BrandID);
                        }
                        else
                        {
                            wks.Cells[ArtWorkStart, 1].Value = string.Format("Factory Capacity by Month Report  (Half Month)", (art == "CPU" ? art : art + " TMS/Min"));
                            wks.Cells[ArtWorkStart + 1, 1].Value = string.Format("Year:{0} Month:{1}", intYear, intMonth);
                            wks.Cells[ArtWorkStart + 1, 8].Value = "By " + (isSCIDelivery ? "Sci Delivery" : "Buyer Delivery");
                        }

                        sheetStart += 3; //每個Artwork間隔 n - 1 格
                    }

                    sxrc.Save(Sci.Production.Class.MicrosoftFile.GetName("Planning_Report_R10"));
                    return new DualResult(true);
                }
                catch (Exception ex)
                {
                    return new DualResult(false, "data loading error.", ex);
                }
            }
            else
            {
                DateTime oDate = new DateTime(intYear, intMonth, 1).AddMonths(1).AddDays(-1);
                DateTime FDate = new DateTime(intYear, intMonth, 1);
                #region --Report Title & 撈資料的日期區間
                if (intMonth == DateTime.Now.Month)
                {
                    string m = DateTime.Now.AddDays(-1).Month.ToString();//前一天的月
                    string day = DateTime.Now.AddDays(-1).Day.ToString();//前一天的日
                    string w = DateTime.Now.AddDays(-1).DayOfWeek.ToString();
                    title = m + "/" + day + " " + w + "." + " Production Status";
                    dStart = DateTime.Now.AddDays(-1 - DateTime.Now.Day + 1).ToShortDateString();
                    dEnd = DateTime.Now.AddDays(-1).ToShortDateString();//前一天
                    dLoad = DateTime.Now.AddDays(-1).ToShortDateString();//前一天

                }
                if (intMonth < DateTime.Now.Month)
                {
                    string LastDayM = oDate.Month.ToString(); // 最後一天的月
                    LastDay = oDate.Day.ToString();   // 最後一天的日
                    string LastDayWeek = oDate.DayOfWeek.ToString();
                    dLoad = oDate.ToShortDateString();//輸入年月的最後一天
                    title = LastDayM + "/" + LastDay + " " + LastDayWeek + "." + " Production Status";
                }
                if (intMonth > DateTime.Now.Month)
                {

                    string FirstDayM = FDate.Month.ToString(); //第一天的月
                    string FirstDay = FDate.Day.ToString(); //第一天的日
                    string FirstDayWeek = FDate.DayOfWeek.ToString();
                    dLoad = FDate.ToShortDateString();
                    title = FirstDayM + "/" + FirstDay + " " + FirstDayWeek + "." + " Production Status";
                }
                if (intMonth != DateTime.Now.Month)
                {
                    dStart = intYear + "/" + intMonth + "/01";
                    dEnd = oDate.ToShortDateString();//輸入年月的最後一天
                }
                STARTday = FDate.ToShortDateString();//輸入年月的第一天
                endDay = oDate.ToShortDateString();//輸入年月的最後一天
                #endregion
                string sqlWhere = ""; string load = ""; string work = "";
                List<string> sqlWheres = new List<string>();
                List<string> LoadingWheres = new List<string>();
                List<string> WorkWheres = new List<string>();
                #region --組WHERE--
                if (!this.txtM.Text.Empty())
                {
                    sqlWheres.Add(" f.MDivisionID = '" + M + "'");
                    LoadingWheres.Add("o.MDivisionID ='" + M + "'");

                    if (!this.txtFactory.Text.Empty())
                    {
                        WorkWheres.Add(" w.FactoryID ='" + Fty + "'");

                    }
                    if (this.txtFactory.Text.Empty())
                    {
                        WorkWheres.Add(" exists (select 1 from Factory WITH (NOLOCK) where MDivisionID = '" + M + "' and ID = w.FactoryID)");
                    }
                } if (!this.txtFactory.Text.Empty())
                {
                    sqlWheres.Add(" f.ID = '" + Fty + "'");
                    LoadingWheres.Add(" o.Factoryid ='" + Fty + "'");
                    if (this.txtM.Text.Empty()) { WorkWheres.Add(" w.FactoryID ='" + Fty + "'"); }

                }
                if (!this.txtBrand.Text.Empty())
                {
                    LoadingWheres.Add("o.BrandID = '" + BrandID + "'");
                }
                sqlWhere = string.Join(" and ", sqlWheres);
                if (!sqlWhere.Empty())
                {
                    sqlWhere = " and" + sqlWhere;
                }

                load = string.Join(" and ", LoadingWheres);
                if (!load.Empty())
                {
                    load = "and " + load;
                }
                work = string.Join("and ", WorkWheres);
                if (!work.Empty())
                {
                    work = "where " + work;
                }
                #endregion

                #region --Prouction Status Excel第一個頁籤SQL
                cmd = string.Format(@"
                            -- 先撈出工廠的Capacity
                            Select f.CountryID, f.ID, (ft.TMS*3600)/(select StdTMS from System WITH (NOLOCK)) as Capacity 
                            into  #tmpFtyCapacity
                            From Factory f WITH (NOLOCK), Factory_TMS ft WITH (NOLOCK)
                            where f.ID = ft.ID and f.Junk = 0 and ft.Year = '{0}' and ft.Month ='{1}' and ft.ArtworkTypeID = 'SEWING'" + sqlWhere +

                @" 
                            -- 撈出各工廠的Loading
                            Select a.CountryID, a.Alias, a.MDivisionID, a.FactoryID, a.Capacity, SUM(a.LoadCPU) as LoadCPU, Max(a.MaxOutputDate) as MaxOutputDate, sum(a.CPU) as CPU
                            Into #tmpLoad
                            From (Select t.CountryID,
                                         c.Alias,
                                         o.MDivisionID,
                                         o.FactoryID,
                                         t.Capacity,
                                         IIF(o.LocalOrder = 0 and o.IsForecast = 0, ot.TMS/(select StdTMS from System WITH (NOLOCK))*o.CPUFactor*o.Qty, o.Qty*o.CPU*o.CPUFactor) as LoadCPU,
                                         si.MaxOutputDate, 
                                         IIF(o.LocalOrder = 0, st.TMS/(select StdTMS from System WITH (NOLOCK))*si.OutputAmount, IIF(o.IsForecast = 0, ot.TMS/(select StdTMS from System WITH (NOLOCK) )*si.OutputAmount*o.CPUFactor,0)) as CPU
                	              From Orders o WITH (NOLOCK) 
                	              Left Join Style_TmsCost st WITH (NOLOCK) on o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = 'SEWING'
                	              Left Join Order_TmsCost ot WITH (NOLOCK) on o.ID = ot.ID and ot.ArtworkTypeID = 'SEWING'
                	              Left Join #tmpFtyCapacity t on t.ID = o.FactoryID
                	              Left Join Country c WITH (NOLOCK) on c.ID = t.CountryID
                	              Cross Apply getOutputInformation(o.ID, '{3}') si
                	              Where o.BuyerDelivery between '{2}' and '{3}'
                	              And o.Junk = 0
                	              And o.SubconInSisterFty = 0
                	              " + load + @"
                	              ) a
                            Group by a.CountryID, a.Alias, a.MDivisionID, a.FactoryID, a.Capacity
                            
                            --	撈出各工廠每天的平均工時
                            Select DISTINCT w.FactoryID, w.Date, AVG(w.Hours)over (partition by FactoryID,date order by FactoryID, Date) as AVGHours
                            into #tmpWorkHours
                            From WorkHour w WITH (NOLOCK)
                           " + work + @"
                            order by w.FactoryID, w.Date 
                
                            Select a.*, DATENAME(weekday,a. MaxOutputDate) as DateName,
                                   IIF(a.CountDay=0,0,round(a.LoadCPU/a.CountDay,0)) as DailyCPU,
                                   IIF(AccuHours* MonthHours=0,0,round(a.LoadCPU/AccuHours* MonthHours,0)) as AccuLoad
                            into  #printdata
                            From (Select t.*, 
                                        isnull((select sum(AVGHours) 
                                                from #tmpWorkHours 
                                                where FactoryID = t.FactoryID 
                                                      and Date <= '{4}'),0) as AccuHours, 
                                         isnull((select sum(AVGHours) 
                                                 from #tmpWorkHours 
                                                 where FactoryID = t.FactoryID 
                                                       and Date between '{2}' and '{5}'),0) as MonthHours, 
                                         isnull((select Count(FactoryID) 
                                                 from #tmpWorkHours 
                                                 where FactoryID = t.FactoryID 
                                                       and Date between '{2}' and '{5}'  and AVGHours > 0),0) as CountDay
                                    From #tmpLoad t
                            ) a 
                
                            SELECT MDivisionID,
                                   FactoryID,
                	               isnull(sum(Capacity),0)Capacity,
                	               sum(LoadCPU)LoadCPU,
                	               loadingRate= isnull(sum(Capacity)/ sum(LoadCPU),0),
                	               MaxOutputDate,
                	               DateName,
                	               sum(DailyCPU)DailyCPU,
                	               sum(AccuLoad)AccuLoad,
                	               sum(CPU)CPU,
                	               cpuVariance=sum(CPU)- sum(AccuLoad),
                	               percentage=iif(sum(AccuLoad)=0,0,sum(CPU)/sum(AccuLoad)),
                	               equivalent=iif(sum(DailyCPU)=0,0,(sum(CPU)- sum(AccuLoad))/ sum(DailyCPU))
                            FROM #printdata
                            group by MDivisionID,FactoryID,MaxOutputDate,  DateName
                
                
                            DROP TABLE #tmpFtyCapacity
                            DROP TABLE #tmpLoad
                            DROP TABLE #tmpWorkHours
                            DROP TABLE  #printdata
                
                            ", intYear, intMonth, dStart, dEnd, dLoad, endDay);
                #endregion
                cmd3 = string.Format(@"
                            --	撈出各工廠每天的平均工時
                            Select DISTINCT w.FactoryID, w.Date, AVG(w.Hours)over (partition by FactoryID,date order by FactoryID, Date) as AVGHours
                            into #tmpWorkHours
                            From WorkHour w WITH (NOLOCK) 
                           " + work + @" 
                            order by w.FactoryID, w.Date 
                
                            -- Excel 第二個頁籤
                            Select  FactoryID, Date, AVGHours, sum(AVGHours) over (partition by FactoryID,date order by FactoryID, Date) as RunningTotal
                            Into  #tmpWorkingHour
                            From #tmpWorkHours WK
                		    order by WK.FactoryID, WK.Date
                            --
                            select DISTINCT FactoryID from #tmpWorkingHour 
                
                            DROP TABLE #tmpWorkHours
                            DROP TABLE #tmpWorkingHour
                            ", STARTday, endDay);
                dtresult = DBProxy.Current.Select("", cmd3, out dt3fty);
                if (!dtresult)
                {
                    ShowErr(dtresult);
                    return dtresult;
                }
                if (dt3fty == null || dt3fty.Rows.Count == 0) { return new DualResult(true); }

                dt2Factory = dt3fty;
                dic.Clear();
                if (s != null) { s = ""; }
                if (b != null) { b = ""; }
                for (int i = 0; i < dt2Factory.Rows.Count; i++)
                {

                    dic.Add(dt2Factory.Rows[i]["FactoryID"].ToString(), string.Format("{0},{1}", ((i * 2) + 2), ((i * 2) + 3)));
                    string sss = dt2Factory.Rows[i]["FactoryID"].ToString();
                    o = string.Format(" outer apply (select  AVGHours,RunningTotal from  #tmpWorkingHour k where date between '{0}' and '{1}' and FactoryID='{2}' and w.Date=k.Date )as {2}", STARTday, endDay, sss);
                    string a = string.Format("isnull({0}.AVGHours,0)AVGHours,isnull({0}.RunningTotal,0)RunningTotal", sss);
                    b += a + ",";
                    if (i == dt2Factory.Rows.Count - 1)
                    {
                        b = b.Substring(0, b.Length - 1);
                    }

                    s += o + Environment.NewLine;
                }

                #region --Prouction Status Excel第二個頁籤SQL
                cmd2 = string.Format(@"
                            --	撈出各工廠每天的平均工時 
                            Select DISTINCT w.FactoryID, w.Date, AVG(w.Hours)over (partition by FactoryID,date order by FactoryID, Date) as AVGHours
                            into #tmpWorkHours
                            From WorkHour w WITH (NOLOCK) 
                           " + work + @" 
                            order by w.FactoryID, w.Date 
                
                            -- Excel 第二個頁籤
                            Select  FactoryID, Date, AVGHours, sum(AVGHours) over (partition by FactoryID,date order by FactoryID, Date) as RunningTotal
                            Into  #tmpWorkingHour
                            From #tmpWorkHours WK
                		    order by WK.FactoryID, WK.Date
                            --
                            select DISTINCT Date into #t from #tmpWorkingHour  where date between '{0}'  and  '{1}'
                            select DISTINCT FactoryID from #tmpWorkingHour 
                            select DISTINCT Date from #tmpWorkingHour  where date between '{0}'  and  '{1}'
                            select distinct date,{3} from #t  w  {2}
                            DROP TABLE #tmpWorkHours
                            DROP TABLE #tmpWorkingHour
                            DROP TABLE #t
                            ", STARTday, endDay, s, b);


                dtresult = DBProxy.Current.Select("", cmd, out dt);
                if (!dtresult)
                {
                    ShowErr(dtresult);
                    return dtresult;
                }
                dtresult = DBProxy.Current.Select("", cmd2, out dt2);

                if (dt2 == null)
                {
                    return new DualResult(true);
                }
                dt2All = dt2[2];
                #endregion
            }

            return new DualResult(true);
        }

        /// <summary>
        /// Report1，開啟xlt填入資料
        /// </summary>
        private DualResult transferReport1(DataTable[] datas, Microsoft.Office.Interop.Excel.Worksheet wks)
        {
            //Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;
            //sxrc.ExcelApp.Visible = true;

            //For Country
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
                            DataRow[] rows = dtFactory.Select(string.Format("Month = '{0}' and FactoryID = '{1}'", intYear.ToString() + mon.ToString("00"), FactoryID));
                            wks.Cells[sheetStart, mon + 1].Value = (rows.Length > 0) ? rows[0]["Capacity"] : 0;
                        }
                        wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);

                        DataRow[] tmprows = dtFactory.Select(string.Format("FactoryID = '{0}'", FactoryID));
                        //var tms = dtFactory.Select(string.Format("FactoryID = '{0}'", FactoryID))[0]["Tms"];
                        wks.Cells[sheetStart, 15].Value = (tmprows.Length > 0 && tmprows[0]["Tms"] != DBNull.Value ? tmprows[0]["Tms"] : 0);
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
                        var ForCapa = dtForecastCapacityByMDV.Compute("SUM(Capacity)", string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
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
                    string MaxSewOutPut = dtOutputMDV.Compute("MAX(SewingYYMM)", "").ToString();
                    MaxSewOutPut = MaxSewOutPut.Length > 0 ? MaxSewOutPut.Substring(5, MaxSewOutPut.Length - 5) : "";
                    setTableToRow(wks, sheetStart, string.Format("{0} Output ({1})", MDivisionID, MaxSewOutPut), dtOutputMDV);
                    sheetStart += 1;

                    //MDV Output  Rate
                    lisPercent.Add(sheetStart.ToString());
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
                    var ForCapa = dtForecastCapacityByCty.Compute("SUM(Capacity)", string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
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
                setFormulaToRow(wks, sheetStart, string.Format("{0} FILL Rate", CountryID), string.Format("=IF({{0}}{1}>0,{{0}}{0}/{{0}}{1},0)", sheetStart - 3, lisCtyIdx[lisCtyIdx.Count - 1]));
                sheetStart += 1;

                //CountryID Output()
                lisOutputIdx.Add(sheetStart.ToString());
                DataTable dtOutputCty = safeGetDt(dt4, string.Format("CountryID = '{0}'", CountryID));
                string MaxSewOutPutCty = dtOutputCty.Compute("MAX(SewingYYMM)", "").ToString();
                MaxSewOutPutCty = MaxSewOutPutCty.Length > 0 ? MaxSewOutPutCty.Substring(5, MaxSewOutPutCty.Length - 5) : "";

                setTableToRow(wks, sheetStart, string.Format("{0} Output ({1})", CountryID, MaxSewOutPutCty), dtOutputCty);
                sheetStart += 1;

                //CountryID Output  Rate
                lisPercent.Add(sheetStart.ToString());
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
                var ForCapa = dtForecastCapacity.Compute("SUM(Capacity)", string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
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
            string MaxSewOutPutT = dtOutput.Compute("MAX(SewingYYMM)", "").ToString();
            MaxSewOutPutT = MaxSewOutPutT.Length > 0 ? MaxSewOutPutT.Substring(5, MaxSewOutPutT.Length - 5) : "";

            setFormulaToRow(wks, sheetStart, string.Format("Output ({0})", MaxSewOutPutT), OutPutStr);
            sheetStart += 1;

            //Output  Rate
            lisPercent.Add(sheetStart.ToString());
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

            return Result.True;
        }

        /// <summary>
        /// Report2，開啟xlt填入資料
        /// </summary>
        private DualResult transferReport2(DataTable[] datas, Microsoft.Office.Interop.Excel.Worksheet wks)
        {
            //string xltPath = @"Planning_R02_02.xlt";
            //SaveXltReportCls sxrc = new SaveXltReportCls(xltPath);
            //Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;
            //sxrc.ExcelApp.Visible = true;

            int ArtworkStart = sheetStart;

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
                            DataRow[] rows = dtFactory.Select(string.Format("FactoryID = '{0}' and MONTH = '{1}'", FactoryID, getCurrMonth(intYear, mon)));
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
                            DataRow[] rows = dtLoadCPU.Select(string.Format("MONTH = '{0}'", getCurrMonth(intYear, mon)));
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
                        DataRow[] rows = dtForecastCapacityByMDV.Select(string.Format("MONTH = '{0}'", getCurrMonth(intYear, mon)));
                        decimal Capacity1 = 0;
                        decimal Capacity2 = 0;
                        if (rows.Length > 0)
                        {
                            for (int tmpRow = 0; tmpRow < rows.Length; tmpRow++)
                            {
                                Capacity1 += rows[tmpRow]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[tmpRow]["Capacity1"]) : 0;
                                Capacity2 += rows[tmpRow]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[tmpRow]["Capacity2"]) : 0;
                            }
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
                    DataRow[] rows = dtForecastCapacity.Select(string.Format("MONTH = '{0}'", getCurrMonth(intYear, mon)));
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
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + ArtworkStart.ToString(), lastCell);
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
                      
            GC.Collect();

            return Result.True;

        }


        #region 減少Code用

        private string getCurrMonth(int intYear, int month)
        {
            if (month > 12)
                return (intYear + 1).ToString() + (month % 12).ToString("00");
            else
                return intYear.ToString() + month.ToString("00");
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

        private void setTableToRow(Microsoft.Office.Interop.Excel.Worksheet wks, int sheetStart, string Cell1Str, DataTable dt)
        {
            wks.Cells[sheetStart, 1].Value = Cell1Str;
            for (int mon = 1; mon < 13; mon++)
            {
                DataRow[] rows = dt.Select(string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                decimal v = 0;
                if (rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        decimal decCapacity;
                        Decimal.TryParse(rows[i]["Capacity"].ToString(), out decCapacity);
                        v += decCapacity;
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

        #endregion


        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region raProductionStatus
            if (radioProductionStatus.Checked == true)
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Planning_R10_ProuctionStatus.xltx", keepApp: true);
                xl.BoOpenFile = true;

                Sci.Utility.Excel.SaveXltReportCls.XltRptTable dt1 = new SaveXltReportCls.XltRptTable(dt);
                Microsoft.Office.Interop.Excel.Worksheet wks = xl.ExcelApp.ActiveSheet;
                xl.DicDatas.Add("##title", title);
                dt1.ShowHeader = false;
                xl.DicDatas.Add("##dt", dt1);

                Sci.Utility.Excel.SaveXltReportCls.XltRptTable dt2 = new SaveXltReportCls.XltRptTable(dt2All);
                dt2.ShowHeader = false;
                xl.DicDatas.Add("##dt2", dt2);

                Sci.Utility.Excel.SaveXltReportCls.ReplaceAction a = setRow1;
                xl.DicDatas.Add("##setRow1", a);

                xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Planning_R10_ProuctionStatus"));

                int startRow = 3; //title有2列
                int lastRow = dt2.Rows.Count + 3 ;
                int wt = dt2.Columns.Count-1;
                wks.Cells[lastRow, 1] = "Total:";
                string wt2,wt3;
                for (int i = 0; i < wt; i++)
                {
                    wt2 = MyExcelPrg.GetExcelColumnName(i + 2);
                    wt3 = string.Format("=SUM({0}{1}:{0}{2})", wt2, startRow, lastRow - 1);
                    wks.Cells[lastRow, (i + 2)] = wt3;
                }

                xl.FinishSave();
            }
            #endregion
            return true;
        }

        void setRow1(Microsoft.Office.Interop.Excel.Worksheet oSheet, int rowNo, int columnNo)
        {
            int idx = 0;
            foreach (DataRow row in dt2[0].Rows)
            {
                oSheet.Cells[1, 2 + idx].Value = row["FactoryID"];
                idx += 2;
            }
        }

        private void rdMonth_CheckedChanged(object sender, EventArgs e)
        {
            labelMonth.Visible = !radioMonthlyReport.Checked;
            numMonth.Visible = !radioMonthlyReport.Checked;

            if (radioMonthlyReport.Checked)
            {
                numMonth.Value = 0;
            }
        }

        private void radioSemimonthlyReport_CheckedChanged(object sender, EventArgs e)
        {
            labelMonth.Visible = radioSemimonthlyReport.Checked;
            numMonth.Visible = radioSemimonthlyReport.Checked;

            if (radioSemimonthlyReport.Checked)
            {
                numMonth.Value = System.DateTime.Today.Month;
            }

        }

        private void radioProductionStatus_CheckedChanged(object sender, EventArgs e)
        {
            labelMonth.Visible = radioProductionStatus.Checked;
            numMonth.Visible = radioProductionStatus.Checked;
            labelDate.Visible = !radioProductionStatus.Checked;
            comboDate.Visible = !radioProductionStatus.Checked;
            labelReport.Visible = !radioProductionStatus.Checked;
            comboReport.Visible = !radioProductionStatus.Checked;
            labelSource.Visible = !radioProductionStatus.Checked;
            checkOrder.Visible = !radioProductionStatus.Checked;
            checkForecast.Visible = !radioProductionStatus.Checked;
            checkFty.Visible = !radioProductionStatus.Checked;
        }
    }
}
