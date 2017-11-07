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

        private string title = string.Empty;
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
            if (this.IsFormClosed)
            {
                return;
            }

            this.labelMonth.Visible = false;
            this.numMonth.Visible = false;
            this.checkOrder.Checked = true;
            this.checkForecast.Checked = true;
            this.checkFty.Checked = true;
            this.numYear.Value = this.currentTime.Year;
            this.numMonth.Value = this.currentTime.Month;
            this.numMonth.Visible = false;
            this.comboDate.Add("SCI Delivery", "S");
            this.comboDate.Add("Buyer Delivery", "B");
            this.comboDate.SelectedIndex = 0;

            #region 取得 Report 資料
            string sql = @"Select ID,ID as NAME, SEQ From ArtworkType WITH (NOLOCK) where ReportDropdown = 1 union Select 'All', 'ALL', '0000' order by SEQ";
            DataTable dt_ref = null;
            DualResult result = DBProxy.Current.Select(null, sql, out dt_ref);

            this.comboReport.DataSource = dt_ref;
            this.comboReport.DisplayMember = "NAME";
            this.comboReport.ValueMember = "ID";
            this.comboReport.SelectedValue = "SEWING";
            #endregion

        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (this.radioMonthlyReport.Checked || this.radioSemimonthlyReport.Checked)
            {
                DualResult result = Result.True;
                try
                {
                    List<string> artworkLis = new List<string>();

                    if (this.ArtWorkType == "All")
                    {
                        DataTable dt = (DataTable)this.comboReport.DataSource;
                        artworkLis = dt.AsEnumerable()
                            .Where(row => row.Field<string>("ID") != "All")
                            .Select(row => row.Field<string>("ID").ToString()).ToList();
                    }
                    else
                    {
                        artworkLis.Add(this.ArtWorkType);
                    }

                    string xltPath = string.Empty;
                    string strHeaderRange = string.Empty;
                    if (this.ReportType == 1)
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
                    foreach (string art in artworkLis)
                    {
                        DataTable[] datas;
                        DualResult res = DBProxy.Current.SelectSP(string.Empty, "Planning_Report_R10", this.NewMethod(art), out datas);

                        if (res && datas[1].Rows.Count > 0)
                        {
                            dic.Add(art, datas);
                        }
                        else
                        {
                            dic.Add(art, null);
                        }
                    }

                    this.sheetStart = 5; // 起始位置
                    int artWorkStart = 2;

#if DEBUG
                    sxrc.ExcelApp.Visible = true;
#endif

                    foreach (string art in artworkLis)
                    {
                        // CopyHeader
                        if (artworkLis.IndexOf(art) > 0)
                        {
                            artWorkStart = this.sheetStart;

                            Microsoft.Office.Interop.Excel.Range desRg = wks.get_Range(string.Format("A{0}:A{0}", this.sheetStart.ToString()));
                            wks.get_Range(strHeaderRange).Copy();
                            desRg.PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAll);
                            this.sheetStart += 3;
                        }

                        if (dic[art] != null)
                        {
                            if (this.ReportType == 1)
                            {
                                this.TransferReport1(dic[art], sxrc.ExcelApp.ActiveSheet);
                            }
                            else
                            {
                                this.TransferReport2(dic[art], sxrc.ExcelApp.ActiveSheet);
                            }
                        }
                        else
                        {
                            wks.Cells[this.sheetStart, 1].Value = string.Format("{0} data not found.", art);

                            // 載入失敗
                        }

                        // 修改Header
                        if (this.ReportType == 1)
                        {
                            wks.Cells[artWorkStart, 1].Value = string.Format("Factory Capacity by Month Report  {0}", art == "CPU" ? art : art + " TMS/Min");
                            wks.Cells[artWorkStart + 1, 1].Value = string.Format("Year:{0}", this.intYear);
                            wks.Cells[artWorkStart + 1, 3].Value = string.Format("Print Type:< {0} >", this.SourceStr);
                            wks.Cells[artWorkStart + 1, 8].Value = string.Format("By {0}                             Buyer : {1}", this.isSCIDelivery ? "Sci Delivery" : "Buyer Delivery", this.BrandID);
                        }
                        else
                        {
                            wks.Cells[artWorkStart, 1].Value = string.Format("Factory Capacity by Month Report  (Half Month)", art == "CPU" ? art : art + " TMS/Min");
                            wks.Cells[artWorkStart + 1, 1].Value = string.Format("Year:{0} Month:{1}", this.intYear, this.intMonth);
                            wks.Cells[artWorkStart + 1, 8].Value = "By " + (this.isSCIDelivery ? "Sci Delivery" : "Buyer Delivery");
                        }

                        this.sheetStart += 3; // 每個Artwork間隔 n - 1 格
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
                DateTime oDate = new DateTime(this.intYear, this.intMonth, 1).AddMonths(1).AddDays(-1);
                DateTime fDate = new DateTime(this.intYear, this.intMonth, 1);
                #region --Report Title & 撈資料的日期區間
                if (this.intMonth == DateTime.Now.Month)
                {
                    string m = DateTime.Now.AddDays(-1).Month.ToString(); // 前一天的月
                    string day = DateTime.Now.AddDays(-1).Day.ToString(); // 前一天的日
                    string w = DateTime.Now.AddDays(-1).DayOfWeek.ToString();
                    this.title = m + "/" + day + " " + w + "." + " Production Status";
                    this.dStart = DateTime.Now.AddDays(-1 - DateTime.Now.Day + 1).ToShortDateString();
                    this.dEnd = DateTime.Now.AddDays(-1).ToShortDateString(); // 前一天
                    this.dLoad = DateTime.Now.AddDays(-1).ToShortDateString(); // 前一天
                }

                if (this.intMonth < DateTime.Now.Month)
                {
                    string lastDayM = oDate.Month.ToString(); // 最後一天的月
                    this.LastDay = oDate.Day.ToString();   // 最後一天的日
                    string lastDayWeek = oDate.DayOfWeek.ToString();
                    this.dLoad = oDate.ToShortDateString(); // 輸入年月的最後一天
                    this.title = lastDayM + "/" + this.LastDay + " " + lastDayWeek + "." + " Production Status";
                }

                if (this.intMonth > DateTime.Now.Month)
                {
                    string firstDayM = fDate.Month.ToString(); // 第一天的月
                    string firstDay = fDate.Day.ToString(); // 第一天的日
                    string firstDayWeek = fDate.DayOfWeek.ToString();
                    this.dLoad = fDate.ToShortDateString();
                    this.title = firstDayM + "/" + firstDay + " " + firstDayWeek + "." + " Production Status";
                }

                if (this.intMonth != DateTime.Now.Month)
                {
                    this.dStart = this.intYear + "/" + this.intMonth + "/01";
                    this.dEnd = oDate.ToShortDateString(); // 輸入年月的最後一天
                }

                this.STARTday = fDate.ToShortDateString(); // 輸入年月的第一天
                this.endDay = oDate.ToShortDateString(); // 輸入年月的最後一天
                #endregion
                string sqlWhere = string.Empty;
                string load = string.Empty;
                string work = string.Empty;
                List<string> sqlWheres = new List<string>();
                List<string> loadingWheres = new List<string>();
                List<string> workWheres = new List<string>();
                #region --組WHERE--
                if (!this.txtM.Text.Empty())
                {
                    sqlWheres.Add(" f.MDivisionID = '" + this.M + "'");
                    loadingWheres.Add("o.MDivisionID ='" + this.M + "'");

                    if (!this.txtFactory.Text.Empty())
                    {
                        workWheres.Add(" w.FactoryID ='" + this.Fty + "'");
                    }

                    if (this.txtFactory.Text.Empty())
                    {
                        workWheres.Add(" exists (select 1 from Factory WITH (NOLOCK) where MDivisionID = '" + this.M + "' and ID = w.FactoryID)");
                    }
                }

                if (!this.txtFactory.Text.Empty())
                {
                    sqlWheres.Add(" f.ID = '" + this.Fty + "'");
                    loadingWheres.Add(" o.Factoryid ='" + this.Fty + "'");
                    if (this.txtM.Text.Empty())
                    {
                        workWheres.Add(" w.FactoryID ='" + this.Fty + "'");
                    }
                }

                if (!this.txtBrand.Text.Empty())
                {
                    loadingWheres.Add("o.BrandID = '" + this.BrandID + "'");
                }

                sqlWhere = string.Join(" and ", sqlWheres);
                if (!sqlWhere.Empty())
                {
                    sqlWhere = " and" + sqlWhere;
                }

                load = string.Join(" and ", loadingWheres);
                if (!load.Empty())
                {
                    load = "and " + load;
                }

                work = string.Join("and ", workWheres);
                if (!work.Empty())
                {
                    work = "where " + work;
                }
                #endregion

                #region --Prouction Status Excel第一個頁籤SQL
                this.cmd = string.Format(
                    @"
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
                            ", this.intYear,
                            this.intMonth,
                            this.dStart,
                            this.dEnd,
                            this.dLoad,
                            this.endDay);
                #endregion
                this.cmd3 = string.Format(
                    @"
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
                            ", this.STARTday,
                            this.endDay);
                this.dtresult = DBProxy.Current.Select(string.Empty, this.cmd3, out this.dt3fty);
                if (!this.dtresult)
                {
                    this.ShowErr(this.dtresult);
                    return this.dtresult;
                }

                if (this.dt3fty == null || this.dt3fty.Rows.Count == 0)
                {
                    return new DualResult(true);
                }

                this.dt2Factory = this.dt3fty;
                this.dic.Clear();
                if (this.s != null)
                {
                    this.s = string.Empty;
                }

                if (this.b != null)
                {
                    this.b = string.Empty;
                }

                for (int i = 0; i < this.dt2Factory.Rows.Count; i++)
                {
                    this.dic.Add(this.dt2Factory.Rows[i]["FactoryID"].ToString(), string.Format("{0},{1}", (i * 2) + 2, (i * 2) + 3));
                    string sss = this.dt2Factory.Rows[i]["FactoryID"].ToString();
                    this.o = string.Format(" outer apply (select  AVGHours,RunningTotal from  #tmpWorkingHour k where date between '{0}' and '{1}' and FactoryID='{2}' and w.Date=k.Date )as {2}", this.STARTday, this.endDay, sss);
                    string a = string.Format("isnull({0}.AVGHours,0)AVGHours,isnull({0}.RunningTotal,0)RunningTotal", sss);
                    this.b += a + ",";
                    if (i == this.dt2Factory.Rows.Count - 1)
                    {
                        this.b = this.b.Substring(0, this.b.Length - 1);
                    }

                    this.s += this.o + Environment.NewLine;
                }

                #region --Prouction Status Excel第二個頁籤SQL
                this.cmd2 = string.Format(
                    @"
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
                            ", this.STARTday,
                            this.endDay,
                            this.s,
                            this.b);

                this.dtresult = DBProxy.Current.Select(string.Empty, this.cmd, out this.dt);
                if (!this.dtresult)
                {
                    this.ShowErr(this.dtresult);
                    return this.dtresult;
                }

                this.dtresult = DBProxy.Current.Select(string.Empty, this.cmd2, out this.dt2);

                if (this.dt2 == null)
                {
                    return new DualResult(true);
                }

                this.dt2All = this.dt2[2];
                #endregion
            }

            return new DualResult(true);
        }

        private List<SqlParameter> NewMethod(string art)
        {
            return new List<SqlParameter>
                            {
                                new SqlParameter("@ReportType", this.ReportType),
                                new SqlParameter("@BrandID", this.BrandID),
                                new SqlParameter("@ArtWorkType", art),
                                new SqlParameter("@isSCIDelivery", this.isSCIDelivery),
                                new SqlParameter("@Year", this.intYear),
                                new SqlParameter("@Month", this.intMonth),
                                new SqlParameter("@SourceStr", this.SourceStr),
                                new SqlParameter("@M", this.M),
                                new SqlParameter("@Fty", this.Fty)
                            };
        }

        /// <summary>
        /// TransferReport1
        /// </summary>
        /// <param name="datas">datas</param>
        /// <param name="wks">wks</param>
        /// <returns>DualResult</returns>
        private DualResult TransferReport1(DataTable[] datas, Microsoft.Office.Interop.Excel.Worksheet wks)
        {
            // For Country
            int mDVIdx = 0; // 每個MDV所在的Index，抓sheetStart，在Country下面
            int mDVTotalIdx = 0;
            List<string> lisCtyIdx = new List<string>();
            List<string> lisMDVTTLIdx = new List<string>();
            List<string> lisOutputIdx = new List<string>(); // By Country

            DataTable dtList = datas[0];
            DataTable dt0 = datas[1]; // [0] Country Capacity
            DataTable dt1 = datas[2]; // [1] By Factory Capacity
            DataTable dt2 = datas[3]; // [2] non Sister
            DataTable dt3 = datas[4]; // [3] For Forecast shared
            DataTable dt4 = datas[5]; // [4] For Output, 及Output後面的Max日期

            DataTable dtCountryList = dtList.DefaultView.ToTable(true, "CountryID");
            List<string> lisBold = new List<string>();
            List<string> lisPercent = new List<string>();
            for (int idxCty = 0; idxCty < dtCountryList.Rows.Count; idxCty++)
            {
                lisBold.Add(this.sheetStart.ToString());
                string countryID = dtCountryList.Rows[idxCty]["CountryID"].ToString();
                DataTable dtCountry = this.SafeGetDt(dt0, string.Format("CountryID = '{0}'", countryID));
                if (dtCountry.Rows.Count == 0)
                {
                    continue;
                }

                string countryName = dtCountry.Rows[0]["CountryName"].ToString();

                lisCtyIdx.Add(this.sheetStart.ToString());
                this.SetTableToRow(wks, this.sheetStart, countryName, dtCountry);
                this.sheetStart += 1;

                DataTable dtMDVList = this.SafeGetDt(dtList, string.Format("CountryID = '{0}'", countryID)).DefaultView.ToTable(true, "MDivisionID");
                List<string> lisSumFtyNonSis = new List<string>();

                this.SetColumnToBack(dtMDVList, "MDivisionID", "Sample");
                this.SetColumnToBack(dtMDVList, "MDivisionID", string.Empty);
                bool isSample = false;
                for (int idxMDV = 0; idxMDV < dtMDVList.Rows.Count; idxMDV++)
                {
                    lisBold.Add(this.sheetStart.ToString());

                    // 3 單一某個MDV加總
                    mDVIdx = this.sheetStart;
                    string mDivisionID = dtMDVList.Rows[idxMDV]["MDivisionID"].ToString();

                    isSample = mDivisionID == "Sample";

                    DataTable dtOneMDV = this.SafeGetDt(dtCountry, string.Format("MDivisionID = '{0}'", mDivisionID));
                    this.SetTableToRow(wks, this.sheetStart, mDivisionID, dtOneMDV);
                    this.sheetStart += 1;

                    // 4 Factory Data，這裡需要迴圈For每個工廠
                    DataTable dtFactory = this.SafeGetDt(dt1, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mDivisionID));
                    DataTable dtFactoryList = this.SafeGetDt(dtList, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mDivisionID)).DefaultView.ToTable(true, "FactoryID");
                    int ftyStart = this.sheetStart;
                    foreach (DataRow row in dtFactoryList.Rows)
                    {
                        string factoryID = row["FactoryID"].ToString();
                        wks.Cells[this.sheetStart, 1].Value = factoryID;

                        for (int mon = 1; mon < 13; mon++)
                        {
                            DataRow[] rows = dtFactory.Select(string.Format("Month = '{0}' and FactoryID = '{1}'", this.intYear.ToString() + mon.ToString("00"), factoryID));
                            wks.Cells[this.sheetStart, mon + 1].Value = (rows.Length > 0) ? rows[0]["Capacity"] : 0;
                        }

                        wks.Cells[this.sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), this.sheetStart);

                        DataRow[] tmprows = dtFactory.Select(string.Format("FactoryID = '{0}'", factoryID));
                        wks.Cells[this.sheetStart, 15].Value = tmprows.Length > 0 && tmprows[0]["Tms"] != DBNull.Value ? tmprows[0]["Tms"] : 0;
                        this.sheetStart += 1;
                    }

                    // 5 By non-sister
                    int nonSisStart = this.sheetStart;
                    DataTable dtByNonSister = this.SafeGetDt(dt2, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mDivisionID));
                    this.SetTableToRow(wks, this.sheetStart, "non-sister sub-in", dtByNonSister);
                    this.DrawBottomLine(wks, this.sheetStart, 1);
                    this.sheetStart += 1;

                    if (isSample)
                    {
                        continue;
                    }

                    // MDV total
                    mDVTotalIdx = this.sheetStart;
                    this.SetFormulaToRow(wks, this.sheetStart, mDivisionID + " total", string.Format("=SUM({{0}}{0}:{{0}}{1})", ftyStart, nonSisStart));

                    this.DrawBottomLine(wks, this.sheetStart, 1);
                    this.sheetStart += 1;

                    lisSumFtyNonSis.Add(ftyStart.ToString() + "," + nonSisStart.ToString());

                    // 6 ForecastCapacity
                    lisPercent.Add(this.sheetStart.ToString());
                    DataTable dtForecastCapacityByMDV = this.SafeGetDt(dt3, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mDivisionID));
                    wks.Cells[this.sheetStart, 1].Value = string.Format("{0} Forecast shared", mDivisionID);
                    for (int mon = 1; mon < 13; mon++)
                    {
                        var forCapa2 = dtForecastCapacityByMDV.Compute("SUM(Capacity)", string.Format("Month = '{0}'", this.intYear.ToString() + mon.ToString("00")));
                        forCapa2 = (forCapa2 == DBNull.Value) ? 0 : forCapa2;
                        wks.Cells[this.sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), mDVTotalIdx, forCapa2);
                    }

                    var sumforcapaMDV = dtForecastCapacityByMDV.Compute("SUM(Capacity)", string.Empty);
                    wks.Cells[this.sheetStart, 14].Value = string.Format("=({0}) / SUM({1}{3}:{2}{3})", (sumforcapaMDV == DBNull.Value) ? 0 : sumforcapaMDV, MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), mDVTotalIdx);
                    this.sheetStart += 1;

                    // MDV 1 Loading - CAPA
                    this.SetFormulaToRow(wks, this.sheetStart, string.Format("{0} Loading - CAPA", mDivisionID), string.Format("=({{0}}{0} - {{0}}{1})", mDVTotalIdx, mDVIdx));
                    this.sheetStart += 1;

                    // MDV FILL RATE
                    lisPercent.Add(this.sheetStart.ToString());
                    this.SetFormulaToRow(wks, this.sheetStart, string.Format("{0} FILL RATE", mDivisionID), string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", mDVIdx, mDVTotalIdx));

                    this.DrawBottomLine(wks, this.sheetStart, 1);
                    this.sheetStart += 1;

                    // Max(OutputDate)
                    // Order+FactoryOrder 的 SewCapacity
                    DataTable dtOutputMDV = this.SafeGetDt(dt4, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mDivisionID));
                    string maxSewOutPut = dtOutputMDV.Compute("MAX(SewingYYMM)", string.Empty).ToString();
                    maxSewOutPut = maxSewOutPut.Length > 0 ? maxSewOutPut.Substring(5, maxSewOutPut.Length - 5) : string.Empty;
                    this.SetTableToRow(wks, this.sheetStart, string.Format("{0} Output ({1})", mDivisionID, maxSewOutPut), dtOutputMDV);
                    this.sheetStart += 1;

                    // MDV Output  Rate
                    lisPercent.Add(this.sheetStart.ToString());
                    this.SetFormulaToRow(wks, this.sheetStart, string.Format("{0} Output  Rate", mDivisionID), string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", mDVTotalIdx, this.sheetStart - 1));

                    this.DrawBottomLine(wks, this.sheetStart, 2);

                    this.sheetStart += 1;
                }

                // CountryID Grand TTL
                mDVTotalIdx = this.sheetStart;
                lisMDVTTLIdx.Add(this.sheetStart.ToString());
                string sumFtyStr = "=";
                foreach (string str in lisSumFtyNonSis)
                {
                    sumFtyStr += string.Format("+SUM({{0}}{0}:{{0}}{1})", str.Split(',')[0], str.Split(',')[1]);
                }

                this.SetFormulaToRow(wks, this.sheetStart, string.Format("{0} Grand TTL", countryID), sumFtyStr);

                this.DrawBottomLine(wks, this.sheetStart, 1);
                this.sheetStart += 1;

                // CountryID Forecast shared
                lisPercent.Add(this.sheetStart.ToString());
                DataTable dtForecastCapacityByCty = this.SafeGetDt(dt3, string.Format("CountryID = '{0}'", countryID));
                wks.Cells[this.sheetStart, 1].Value = string.Format("{0} Forecast shared", countryID);
                for (int mon = 1; mon < 13; mon++)
                {
                    var forCapa1 = dtForecastCapacityByCty.Compute("SUM(Capacity)", string.Format("Month = '{0}'", this.intYear.ToString() + mon.ToString("00")));
                    forCapa1 = (forCapa1 == DBNull.Value) ? 0 : forCapa1;
                    wks.Cells[this.sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), mDVTotalIdx, forCapa1);
                }

                var sumforcapaCty = dtForecastCapacityByCty.Compute("SUM(Capacity)", string.Empty);
                wks.Cells[this.sheetStart, 14].Value = string.Format("=({0}) / SUM({1}{3}:{2}{3})", (sumforcapaCty == DBNull.Value) ? 0 : sumforcapaCty, MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), mDVTotalIdx);
                this.sheetStart += 1;

                // CountryID Loading - CAPA
                this.SetFormulaToRow(wks, this.sheetStart, string.Format("{0} Loading - CAPA", countryID), string.Format("=({{0}}{0} - {{0}}{1})", this.sheetStart - 2, lisCtyIdx[lisCtyIdx.Count - 1]));
                this.sheetStart += 1;

                // CountryID FILL Rate
                lisPercent.Add(this.sheetStart.ToString());
                this.SetFormulaToRow(wks, this.sheetStart, string.Format("{0} FILL Rate", countryID), string.Format("=IF({{0}}{1}>0,{{0}}{0}/{{0}}{1},0)", this.sheetStart - 3, lisCtyIdx[lisCtyIdx.Count - 1]));
                this.sheetStart += 1;

                // CountryID Output()
                lisOutputIdx.Add(this.sheetStart.ToString());
                DataTable dtOutputCty = this.SafeGetDt(dt4, string.Format("CountryID = '{0}'", countryID));
                string maxSewOutPutCty = dtOutputCty.Compute("MAX(SewingYYMM)", string.Empty).ToString();
                maxSewOutPutCty = maxSewOutPutCty.Length > 0 ? maxSewOutPutCty.Substring(5, maxSewOutPutCty.Length - 5) : string.Empty;

                this.SetTableToRow(wks, this.sheetStart, string.Format("{0} Output ({1})", countryID, maxSewOutPutCty), dtOutputCty);
                this.sheetStart += 1;

                // CountryID Output  Rate
                lisPercent.Add(this.sheetStart.ToString());
                this.SetFormulaToRow(wks, this.sheetStart, string.Format("{0} Output  Rate", countryID), string.Format("=IF({0}{1} > 0, {0}{2} / {0}{1},0)", "{0}", mDVTotalIdx, this.sheetStart - 1));

                this.DrawBottomLine(wks, this.sheetStart, 3);

                this.sheetStart += 1;
            }

            // Total Capacity
            lisBold.Add(this.sheetStart.ToString());
            string totalStr = "=";
            foreach (string str in lisCtyIdx)
            {
                totalStr += string.Format("+{0}{1}", "{0}", str);
            }

            this.SetFormulaToRow(wks, this.sheetStart, "Total Capacity", totalStr);

            this.sheetStart += 1;

            // Total Loading - CAPA
            lisBold.Add(this.sheetStart.ToString());
            string totalLoadStr = "=";
            foreach (string str in lisMDVTTLIdx)
            {
                totalLoadStr += string.Format("+{0}{1}", "{0}", str);
            }

            this.SetFormulaToRow(wks, this.sheetStart, "Total Loading", totalLoadStr);

            this.sheetStart += 1;

            // Total Forecast shared
            lisBold.Add(this.sheetStart.ToString());
            lisPercent.Add(this.sheetStart.ToString());
            DataTable dtForecastCapacity = dt3;
            wks.Cells[this.sheetStart, 1].Value = "Total FC shared";
            for (int mon = 1; mon < 14; mon++)
            {
                var forCapa = dtForecastCapacity.Compute("SUM(Capacity)", string.Format("Month = '{0}'", this.intYear.ToString() + mon.ToString("00")));
                forCapa = (forCapa == DBNull.Value) ? 0 : forCapa;
                wks.Cells[this.sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), this.sheetStart - 1, forCapa);
            }

            this.sheetStart += 1;

            // Total Loading - CAPA
            lisBold.Add(this.sheetStart.ToString());
            this.SetFormulaToRow(wks, this.sheetStart, "Loading-CAPA", string.Format("=({0}{1} - {0}{2})", "{0}", this.sheetStart - 2, this.sheetStart - 3));
            this.sheetStart += 1;

            // FILL Rate
            lisBold.Add(this.sheetStart.ToString());
            lisPercent.Add(this.sheetStart.ToString());
            this.SetFormulaToRow(wks, this.sheetStart, "FIll RATE", string.Format("=IF({0}{2}>0,{0}{1}/{0}{2},0)", "{0}", this.sheetStart - 3, this.sheetStart - 4));
            this.sheetStart += 1;

            // Output()
            lisBold.Add(this.sheetStart.ToString());
            string outPutStr = "=";
            foreach (string str in lisOutputIdx)
            {
                outPutStr += string.Format("+{{0}}{0}", str);
            }

            DataTable dtOutput = dt4;
            string maxSewOutPutT = dtOutput.Compute("MAX(SewingYYMM)", string.Empty).ToString();
            maxSewOutPutT = maxSewOutPutT.Length > 0 ? maxSewOutPutT.Substring(5, maxSewOutPutT.Length - 5) : string.Empty;

            this.SetFormulaToRow(wks, this.sheetStart, string.Format("Output ({0})", maxSewOutPutT), outPutStr);
            this.sheetStart += 1;

            // Output  Rate
            lisPercent.Add(this.sheetStart.ToString());
            lisBold.Add(this.sheetStart.ToString());
            this.SetFormulaToRow(wks, this.sheetStart, "Output  Rate", string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", this.sheetStart - 5, this.sheetStart - 1));

            // 第一排置中
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + "5", MyExcelPrg.GetExcelColumnName(1) + this.sheetStart.ToString());
            rg.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Country, MDvision, Total 第一格粗體
            foreach (string idx in lisBold)
            {
                string rgStr = string.Format("{0}{1}:{0}{1}", MyExcelPrg.GetExcelColumnName(1), idx);
                rg = wks.get_Range(rgStr);
                rg.Font.Bold = true;
            }

            // 數值格式
            string lastCell = MyExcelPrg.GetExcelColumnName(15) + this.sheetStart.ToString();
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(2) + "5", lastCell);
            rg.Cells.NumberFormat = "##,###,##0";

            // Total欄左右邊線
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(14) + "5", MyExcelPrg.GetExcelColumnName(14) + this.sheetStart.ToString());
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

            // 第一欄位Auto Fit
            rg = wks.get_Range("A:A");
            rg.Columns.AutoFit();

            return Result.True;
        }

        /// <summary>
        /// TransferReport2
        /// </summary>
        /// <param name="datas">datas</param>
        /// <param name="wks">wks</param>
        /// <returns>DualResult</returns>
        private DualResult TransferReport2(DataTable[] datas, Microsoft.Office.Interop.Excel.Worksheet wks)
        {
            int artworkStart = this.sheetStart;

            // Set Header
            DateTime startDate = new DateTime(this.intYear, this.intMonth, 1);
            for (int mon = 0; mon < 6; mon++)
            {
                DateTime nextDate = startDate.AddMonths(mon);
                wks.Cells[this.sheetStart - 1, 5 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 22).ToShortDateString();
                wks.Cells[this.sheetStart - 1, 6 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 7).AddMonths(1).ToShortDateString();
            }

            DataTable dtList = datas[0];
            DataTable dt0 = datas[1]; // [0] By Factory 最細的上下半月Capacity
            DataTable dt1 = datas[2]; // [1] By Factory Loading CPU
            DataTable dt2 = datas[3]; // [2] For Forecast shared

            DataTable dtCountryList = dtList.DefaultView.ToTable(true, "CountryID");
            List<string> lisPercent = new List<string>();

            // For Country
            for (int idxCty = 0; idxCty < dtCountryList.Rows.Count; idxCty++)
            {
                string countryID = dtCountryList.Rows[idxCty]["CountryID"].ToString();
                DataTable dtCountry = this.SafeGetDt(dt0, string.Format("CountryID = '{0}'", countryID));
                if (dtCountry.Rows.Count == 0)
                {
                    continue;
                }

                string countryName = dtCountry.Rows[0]["CountryName"].ToString();
                wks.Cells[this.sheetStart, 1].Value = countryName;

                DataTable dtMDVList = this.SafeGetDt(dtList, string.Format("CountryID = '{0}'", countryID)).DefaultView.ToTable(true, "MDivisionID");

                List<string> lisCapaCty = new List<string>();
                List<string> lisLoadingCty = new List<string>();
                this.SetColumnToBack(dtMDVList, "MDivisionID", "Sample");
                this.SetColumnToBack(dtMDVList, "MDivisionID", string.Empty);
                int idx = 0;

                for (int idxMDV = 0; idxMDV < dtMDVList.Rows.Count; idxMDV++)
                {
                    string mDivisionID = dtMDVList.Rows[idxMDV]["MDivisionID"].ToString();
                    wks.Cells[this.sheetStart, 2].Value = mDivisionID;

                    DataTable dtOneMDV = this.SafeGetDt(dtCountry, string.Format("MDivisionID = '{0}'", mDivisionID));
                    DataTable dtFactory = this.SafeGetDt(dtOneMDV, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mDivisionID));
                    DataTable dtFactoryList = this.SafeGetDt(dtList, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mDivisionID)).DefaultView.ToTable(true, "FactoryID");
                    int ftyStart = this.sheetStart;
                    List<string> lisCapa = new List<string>();
                    List<string> lisLoading = new List<string>();
                    foreach (DataRow row in dtFactoryList.Rows)
                    {
                        string factoryID = row["FactoryID"].ToString();
                        wks.Cells[this.sheetStart, 3].Value = factoryID;
                        wks.Cells[this.sheetStart, 4].Value = "Capa.";
                        lisCapa.Add(this.sheetStart.ToString());
                        lisCapaCty.Add(this.sheetStart.ToString());
                        idx = 0;
                        for (int mon = this.intMonth; mon < this.intMonth + 6; mon++)
                        {
                            DataRow[] rows = dtFactory.Select(string.Format("FactoryID = '{0}' and MONTH = '{1}'", factoryID, this.GetCurrMonth(this.intYear, mon)));
                            decimal capacity13 = 0;
                            decimal capacity23 = 0;
                            if (rows.Length > 0)
                            {
                                capacity13 = rows[0]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity1"]) : 0;
                                capacity23 = rows[0]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity2"]) : 0;
                            }

                            wks.Cells[this.sheetStart, 5 + (idx * 2)].Value = capacity13;
                            wks.Cells[this.sheetStart, 5 + (idx * 2) + 1].Value = capacity23;
                            idx += 1;
                        }

                        wks.Cells[this.sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), this.sheetStart);

                        this.sheetStart += 1;

                        lisLoading.Add(this.sheetStart.ToString());
                        lisLoadingCty.Add(this.sheetStart.ToString());
                        wks.Cells[this.sheetStart, 4].Value = "Load.";
                        idx = 0;
                        DataTable dtLoadCPU = this.SafeGetDt(dt1, string.Format("CountryID = '{0}' And MDivisionID = '{1}' and FactoryID = '{2}'", countryID, mDivisionID, factoryID));
                        for (int mon = this.intMonth; mon < this.intMonth + 6; mon++)
                        {
                            DataRow[] rows = dtLoadCPU.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(this.intYear, mon)));
                            decimal capacity12 = 0;
                            decimal capacity22 = 0;
                            if (rows.Length > 0)
                            {
                                capacity12 = rows[0]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity1"]) : 0;
                                capacity22 = rows[0]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity2"]) : 0;
                            }

                            wks.Cells[this.sheetStart, 5 + (idx * 2)].Value = capacity12;
                            wks.Cells[this.sheetStart, 5 + (idx * 2) + 1].Value = capacity22;
                            idx += 1;
                        }

                        wks.Cells[this.sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), this.sheetStart);

                        this.sheetStart += 1;

                        wks.Cells[this.sheetStart, 4].Value = "Vari.";
                        for (int i = 5; i <= 17; i++)
                        {
                            string str = string.Format("={0}{1} - {0}{2}", MyExcelPrg.GetExcelColumnName(i), this.sheetStart - 1, this.sheetStart - 2);
                            wks.Cells[this.sheetStart, i] = str;
                        }

                        this.DrawBottomLine(wks, this.sheetStart, 4, 3, 17);

                        this.sheetStart += 1;
                    }

                    // Total Capa.
                    wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Capa.", mDivisionID);
                    string totalCapa = "=";
                    for (int i = 0; i < lisCapa.Count; i++)
                    {
                        totalCapa += string.Format("+{{0}}{0}", lisCapa[i]);
                    }

                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format(totalCapa, MyExcelPrg.GetExcelColumnName(i));
                        wks.Cells[this.sheetStart, i] = str;
                    }

                    this.sheetStart += 1;

                    // Total Load.
                    wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Load.", mDivisionID);
                    string totalLoad = "=";
                    for (int i = 0; i < lisLoading.Count; i++)
                    {
                        totalLoad += string.Format("+{{0}}{0}", lisLoading[i]);
                    }

                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format(totalLoad, MyExcelPrg.GetExcelColumnName(i));
                        wks.Cells[this.sheetStart, i] = str;
                    }

                    this.sheetStart += 1;

                    // 6 ForecastCapacity
                    lisPercent.Add(this.sheetStart.ToString());
                    idx = 0;
                    DataTable dtForecastCapacityByMDV = this.SafeGetDt(dt2, string.Format("CountryID = '{0}' And MDivisionID = '{1}' ", countryID, mDivisionID));
                    wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total FC Shared", mDivisionID);
                    for (int mon = this.intMonth; mon < this.intMonth + 6; mon++)
                    {
                        DataRow[] rows = dtForecastCapacityByMDV.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(this.intYear, mon)));
                        decimal capacity11 = 0;
                        decimal capacity21 = 0;
                        if (rows.Length > 0)
                        {
                            for (int tmpRow = 0; tmpRow < rows.Length; tmpRow++)
                            {
                                capacity11 += rows[tmpRow]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[tmpRow]["Capacity1"]) : 0;
                                capacity21 += rows[tmpRow]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[tmpRow]["Capacity2"]) : 0;
                            }
                        }

                        wks.Cells[this.sheetStart, 5 + (idx * 2)].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2)), this.sheetStart - 1, capacity11);
                        wks.Cells[this.sheetStart, 5 + (idx * 2) + 1].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2) + 1), this.sheetStart - 1, capacity21);
                        idx += 1;
                    }

                    var sumforcapaMDV = dtForecastCapacityByMDV.Compute("SUM(Capacity1)+SUM(Capacity2)", string.Empty);
                    wks.Cells[this.sheetStart, 17] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(17), this.sheetStart - 1, (sumforcapaMDV == DBNull.Value) ? 0 : sumforcapaMDV);

                    this.sheetStart += 1;

                    // Total Vari.
                    wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Vari.", mDivisionID);
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format("={0}{1} - {0}{2}", MyExcelPrg.GetExcelColumnName(i), this.sheetStart - 2, this.sheetStart - 3);
                        wks.Cells[this.sheetStart, i] = str;
                    }

                    this.sheetStart += 1;

                    // Total Fill Rate
                    lisPercent.Add(this.sheetStart.ToString());
                    wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Fill Rate", mDivisionID);
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format("=IF({0}{2}>0,{0}{1} / {0}{2},0)", MyExcelPrg.GetExcelColumnName(i), this.sheetStart - 3, this.sheetStart - 4);
                        wks.Cells[this.sheetStart, i] = str;
                    }

                    this.DrawBottomLine(wks, this.sheetStart, 4, 2, 17);

                    this.sheetStart += 1;
                }

                // Country Total Capa.
                wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Capa.", countryID);
                string totalCapaCty = "={0}";
                totalCapaCty += string.Join("+{0}", lisCapaCty);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format(totalCapaCty, MyExcelPrg.GetExcelColumnName(i));
                    wks.Cells[this.sheetStart, i] = str;
                }

                this.sheetStart += 1;

                // Country Total Load.
                wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Load.", countryID);
                string totalLoadCty = "={0}";
                totalLoadCty += string.Join("+{0}", lisLoadingCty);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format(totalLoadCty, MyExcelPrg.GetExcelColumnName(i));
                    wks.Cells[this.sheetStart, i] = str;
                }

                this.sheetStart += 1;

                // Country FC Shared
                lisPercent.Add(this.sheetStart.ToString());
                idx = 0;
                DataTable dtForecastCapacity = this.SafeGetDt(dt2, string.Format("CountryID = '{0}'", countryID));
                wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total FC Shared", countryID);
                for (int mon = this.intMonth; mon < this.intMonth + 6; mon++)
                {
                    DataRow[] rows = dtForecastCapacity.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(this.intYear, mon)));
                    decimal capacity1 = 0;
                    decimal capacity2 = 0;
                    if (rows.Length > 0)
                    {
                        capacity1 = rows[0]["Capacity1"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity1"]) : 0;
                        capacity2 = rows[0]["Capacity2"] != DBNull.Value ? Convert.ToDecimal(rows[0]["Capacity2"]) : 0;
                    }

                    wks.Cells[this.sheetStart, 5 + (idx * 2)].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2)), this.sheetStart - 1, capacity1);
                    wks.Cells[this.sheetStart, 5 + (idx * 2) + 1].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2) + 1), this.sheetStart - 1, capacity2);
                    idx += 1;
                }

                var sumforcapaMDVCty = dtForecastCapacity.Compute("SUM(Capacity1)+SUM(Capacity2)", string.Empty);
                wks.Cells[this.sheetStart, 17] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(17), this.sheetStart - 1, (sumforcapaMDVCty == DBNull.Value) ? 0 : sumforcapaMDVCty);

                this.sheetStart += 1;

                // Country Total Vari.
                wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Vari.", countryID);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format("={0}{1} - {0}{2}", MyExcelPrg.GetExcelColumnName(i), this.sheetStart - 2, this.sheetStart - 3);
                    wks.Cells[this.sheetStart, i] = str;
                }

                this.sheetStart += 1;

                // Country Total Fill Rate
                lisPercent.Add(this.sheetStart.ToString());
                wks.Cells[this.sheetStart, 3].Value = string.Format("{0} Total Fill Rate", countryID);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format("=IF({0}{2}>0,{0}{1} / {0}{2},0)", MyExcelPrg.GetExcelColumnName(i), this.sheetStart - 3, this.sheetStart - 4);
                    wks.Cells[this.sheetStart, i] = str;
                }

                this.DrawBottomLine(wks, this.sheetStart, 5, 1, 17);

                this.sheetStart += 1;
            }

            this.sheetStart -= 1;

            // 欄位以直線區隔
            string lastCell = MyExcelPrg.GetExcelColumnName(17) + this.sheetStart.ToString();
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + artworkStart.ToString(), lastCell);
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;

            // 數值格式
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
        private string GetCurrMonth(int intYear, int month)
        {
            if (month > 12)
            {
                return (intYear + 1).ToString() + (month % 12).ToString("00");
            }
            else
            {
                return intYear.ToString() + month.ToString("00");
            }
        }

        private void SetColumnToBack(DataTable dt, string column, string value)
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

        private void DrawBottomLine(Microsoft.Office.Interop.Excel.Worksheet wks, int sheetStart, int lineType, int sIdx = 1, int eIdx = 15)
        {
            string rgStr = string.Format("{0}{2}:{1}{2}", MyExcelPrg.GetExcelColumnName(sIdx), MyExcelPrg.GetExcelColumnName(eIdx), sheetStart);

            if (lineType == 1)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 1.5;
            }

            if (lineType == 2)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDashDot;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
            }

            if (lineType == 3)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 4;
            }

            if (lineType == 4)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            }

            if (lineType == 5)
            {
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                wks.get_Range(rgStr).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 4;
            }
        }

        private void SetTableToRow(Microsoft.Office.Interop.Excel.Worksheet wks, int sheetStart, string cell1Str, DataTable dt)
        {
            wks.Cells[sheetStart, 1].Value = cell1Str;
            for (int mon = 1; mon < 13; mon++)
            {
                DataRow[] rows = dt.Select(string.Format("Month = '{0}'", this.intYear.ToString() + mon.ToString("00")));
                decimal v = 0;
                if (rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        decimal decCapacity;
                        decimal.TryParse(rows[i]["Capacity"].ToString(), out decCapacity);
                        v += decCapacity;
                    }
                }

                wks.Cells[sheetStart, mon + 1].Value = v;
            }

            wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);
        }

        private void SetFormulaToRow(Microsoft.Office.Interop.Excel.Worksheet wks, int sheetStart, string cell1Str, string formula)
        {
            wks.Cells[sheetStart, 1].Value = cell1Str;
            for (int i = 2; i <= 14; i++)
            {
                string str = string.Format(formula, MyExcelPrg.GetExcelColumnName(i));
                wks.Cells[sheetStart, i] = str;
            }
        }

        private DataTable SafeGetDt(DataTable dt, string filterStr)
        {
            DataRow[] rows = dt.Select(filterStr);
            DataTable dtOutput = (rows.Length > 0) ? rows.CopyToDataTable() : dt.Clone();
            return dtOutput;
        }
        #endregion

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region raProductionStatus
            if (this.radioProductionStatus.Checked == true)
            {
                if (this.dt == null || this.dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Planning_R10_ProuctionStatus.xltx", keepApp: true);
                xl.BoOpenFile = true;

                SaveXltReportCls.XltRptTable dt1 = new SaveXltReportCls.XltRptTable(this.dt);
                Microsoft.Office.Interop.Excel.Worksheet wks = xl.ExcelApp.ActiveSheet;
                xl.DicDatas.Add("##title", this.title);
                dt1.ShowHeader = false;
                xl.DicDatas.Add("##dt", dt1);

                SaveXltReportCls.XltRptTable dt2 = new SaveXltReportCls.XltRptTable(this.dt2All);
                dt2.ShowHeader = false;
                xl.DicDatas.Add("##dt2", dt2);

                SaveXltReportCls.ReplaceAction a = this.SetRow1;
                xl.DicDatas.Add("##setRow1", a);

                xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Planning_R10_ProuctionStatus"));

                int startRow = 3; // title有2列
                int lastRow = dt2.Rows.Count + 3;
                int wt = dt2.Columns.Count - 1;
                wks.Cells[lastRow, 1] = "Total:";
                string wt2, wt3;
                for (int i = 0; i < wt; i++)
                {
                    wt2 = MyExcelPrg.GetExcelColumnName(i + 2);
                    wt3 = string.Format("=SUM({0}{1}:{0}{2})", wt2, startRow, lastRow - 1);
                    wks.Cells[lastRow, i + 2] = wt3;
                }

                xl.FinishSave();
            }
            #endregion
            return true;
        }

        private void SetRow1(Microsoft.Office.Interop.Excel.Worksheet oSheet, int rowNo, int columnNo)
        {
            int idx = 0;
            foreach (DataRow row in this.dt2[0].Rows)
            {
                oSheet.Cells[1, 2 + idx].Value = row["FactoryID"];
                idx += 2;
            }
        }

        private void RdMonth_CheckedChanged(object sender, EventArgs e)
        {
            this.labelMonth.Visible = !this.radioMonthlyReport.Checked;
            this.numMonth.Visible = !this.radioMonthlyReport.Checked;

            if (this.radioMonthlyReport.Checked)
            {
                this.numMonth.Value = 0;
            }
        }

        private void RadioSemimonthlyReport_CheckedChanged(object sender, EventArgs e)
        {
            this.labelMonth.Visible = this.radioSemimonthlyReport.Checked;
            this.numMonth.Visible = this.radioSemimonthlyReport.Checked;

            if (this.radioSemimonthlyReport.Checked)
            {
                this.numMonth.Value = System.DateTime.Today.Month;
            }
        }

        private void RadioProductionStatus_CheckedChanged(object sender, EventArgs e)
        {
            this.labelMonth.Visible = this.radioProductionStatus.Checked;
            this.numMonth.Visible = this.radioProductionStatus.Checked;
            this.labelDate.Visible = !this.radioProductionStatus.Checked;
            this.comboDate.Visible = !this.radioProductionStatus.Checked;
            this.labelReport.Visible = !this.radioProductionStatus.Checked;
            this.comboReport.Visible = !this.radioProductionStatus.Checked;
            this.labelSource.Visible = !this.radioProductionStatus.Checked;
            this.checkOrder.Visible = !this.radioProductionStatus.Checked;
            this.checkForecast.Visible = !this.radioProductionStatus.Checked;
            this.checkFty.Visible = !this.radioProductionStatus.Checked;
        }
    }
}
