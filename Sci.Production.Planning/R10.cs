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
using System.Drawing;
using System.IO;
using Sci.Production.Prg;

// 此程式前兩個用統計和半月統計是複製Trade Planning R10
namespace Sci.Production.Planning
{
    /// <summary>
    /// R10
    /// </summary>
    public partial class R10 : Win.Tems.PrintForm
    {
        private DateTime currentTime = DateTime.Now;

        private int reportType = 1;
        private string BrandID = string.Empty;
        private string ArtWorkType = string.Empty;
        private bool isSCIDelivery = true;
        private int intYear;
        private int intMonth;
        private string SourceStr;
        private string mDivisionID;
        private string zone;
        private string Fty;
        private bool byCPU;
        private bool byBrand;
        private bool IncludeCancelOrder;
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

        private DataTable[] dt2;
        private DataTable dt3fty;
        private DataTable dt2Factory = null;
        private DataTable dt2All = null;

        /// <inheritdoc/>
        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.print.Visible = false;
            this.txtMDivision.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.numYear.Text == string.Empty)
            {
                this.ShowErr("Year can't be  blank");
                return false;
            }

            if (this.rdHalfMonth.Checked)
            {
                if (this.numMonth.Text == string.Empty)
                {
                    this.ShowErr("Month can't be  blank");
                    return false;
                }
            }

            if (!this.chkOrder.Checked && !this.chkForecast.Checked && !this.chkFty.Checked)
            {
                this.ShowErr("Order, Forecast , Fty Local Order must select one at least ");
                return false;
            }

            this.reportType = this.rdMonth.Checked ? 1 : 2;
            this.BrandID = this.txtBrand1.Text;
            this.ArtWorkType = this.cbReportType.SelectedValue.ToString();
            this.isSCIDelivery = (this.cbDateType.SelectedItem.ToString() == "SCI Delivery") ? true : false;

            this.intYear = Convert.ToInt32(this.numYear.Value);
            this.intMonth = Convert.ToInt32(this.numMonth.Value);
            this.SourceStr = (this.chkOrder.Checked ? "Order," : string.Empty)
                + (this.chkForecast.Checked ? "Forecast," : string.Empty)
                + (this.chkFty.Checked ? "Fty Local Order," : string.Empty);
            this.mDivisionID = this.txtMDivision.Text;
            this.Fty = this.txtFactory.Text;
            this.zone = this.TxtZone.Text;
            this.byCPU = this.chkByCPU.Checked;
            this.byBrand = this.chkByBrand.Checked;
            this.IncludeCancelOrder = this.chkIncludeCancelOrder.Checked;
            return true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.IsFormClosed)
            {
                return;
            }

            this.lbMonth.Visible = false;
            this.numMonth.Visible = false;

            this.chkOrder.Checked = true;
            this.chkForecast.Checked = true;
            this.chkFty.Checked = true;

            this.numYear.Value = this.currentTime.Year;
            this.numMonth.Value = this.currentTime.Month;
            this.numMonth.Visible = false;

            this.cbDateType.Add("SCI Delivery", "S");
            this.cbDateType.Add("Buyer Delivery", "B");
            this.cbDateType.SelectedIndex = 0;

            #region 取得 Report 資料
            string sql = @"Select ID,SEQ + ' - ' + ID as NAME, SEQ From ArtworkType WITH (NOLOCK) where ReportDropdown = 1 And Junk = 0 union Select 'All', 'ALL', '0000' order by Seq";
            DataTable dt_ref = null;
            DualResult result = DBProxy.Current.Select(null, sql, out dt_ref);

            this.cbReportType.DataSource = dt_ref;
            this.cbReportType.DisplayMember = "NAME";
            this.cbReportType.ValueMember = "ID";
            this.cbReportType.SelectedValue = "SEWING";
            #endregion

        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (this.rdMonth.Checked || this.rdHalfMonth.Checked)
            {
                DualResult result = Ict.Result.True;
                try
                {
                    List<string> artworkLis = new List<string>();

                    if (this.ArtWorkType == "All")
                    {
                        DataTable dt = (DataTable)this.cbReportType.DataSource;
                        artworkLis = dt.AsEnumerable()
                            .Where(row => row.Field<string>("ID") != "All")
                            .Select(row => row.Field<string>("ID").ToString()).ToList();
                    }
                    else
                    {
                        artworkLis.Add(this.ArtWorkType);
                    }

                    // string xltPath = string.Empty;
                    // string strHeaderRange = string.Empty;
                    // if (this.reportType == 1)
                    // {
                    //    xltPath = @"Planning_R10_01.xltx";
                    //    strHeaderRange = "A2:O4";
                    // }
                    // else
                    // {
                    //    xltPath = @"Planning_R10_02.xltx";
                    //    strHeaderRange = "A2:Q5";
                    // }

                    // SaveXltReportCls sxrc = new SaveXltReportCls(xltPath);
                    // Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;

                    // Dictionary<string, DataTable[]> dic = new Dictionary<string, DataTable[]>();
                    // foreach (string art in artworkLis)
                    // {
                    //    DataTable[] datas;
                    //    DualResult res = DBProxy.Current.SelectSP(string.Empty, "Planning_Report_R10", this.NewMethod(art), out datas);

                    // if (res && datas[1].Rows.Count > 0 && datas[0].Rows.Count > 0)
                    //    {
                    //        dic.Add(art, datas);
                    //    }
                    //    else
                    //    {
                    //        dic.Add(art, null);
                    //    }
                    // }
                    Dictionary<string, object> paras = new Dictionary<string, object>
                    {
                        { "ReportType", this.reportType },
                        { "BrandID", this.BrandID },
                        { "isSCIDelivery", this.isSCIDelivery },
                        { "Year", this.intYear },
                        { "Month", this.intMonth },
                        { "SourceStr", this.SourceStr },
                        { "MDivisionID", this.mDivisionID },
                        { "Fty", this.Fty },
                        { "HideFoundry", this.chkHideFoundry.Checked },
                        { "Zone", this.zone },
                        { "CalculateCPU", this.byCPU },
                        { "CalculateByBrand", this.byBrand },
                        { "IncludeCancelOrder", this.IncludeCancelOrder },
                    };

                    result = this.RunPlanningR10Report(false, this.chkByCPU.Checked, this.reportType, artworkLis, paras);

                    // this.sheetStart = 6; // 起始位置
                    //                    int artWorkStart = 2;

                    // #if DEBUG
                    //                    sxrc.ExcelApp.Visible = false;
                    // #endif

                    // foreach (string art in artworkLis)
                    //                    {
                    //                        string artworkUnit = MyUtility.GetValue.Lookup($"select ArtworkUnit from ArtworkType where Id = '{art}'");
                    //                        string artworkUnitStr = artworkUnit == "PCS" ? "(PCS)": artworkUnit == "STITCH" ? "(STITCH in thousands)" : "TMS/Min";
                    //                        // CopyHeader
                    //                        if (artworkLis.IndexOf(art) > 0)
                    //                        {
                    //                            artWorkStart = this.sheetStart;

                    // Microsoft.Office.Interop.Excel.Range desRg = wks.get_Range(string.Format("A{0}:A{0}", this.sheetStart.ToString()));
                    //                            wks.get_Range(strHeaderRange).Copy();
                    //                            desRg.PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAll);
                    //                            this.sheetStart += 3;
                    //                        }

                    // if (dic[art] != null)
                    //                        {
                    //                            if (this.reportType == 1)
                    //                            {
                    //                                this.TransferReport1(dic[art], sxrc.ExcelApp.ActiveSheet);
                    //                            }
                    //                            else
                    //                            {
                    //                                this.TransferReport2(dic[art], sxrc.ExcelApp.ActiveSheet);
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            wks.Cells[this.sheetStart, 1].Value = string.Format("{0} data not found.", art);

                    // // 載入失敗
                    //                        }

                    // // 修改Header
                    //                        if (this.reportType == 1)
                    //                        {
                    //                            wks.Cells[artWorkStart, 1].Value = string.Format("Factory Capacity by Month Report  {0}", art + " " + artworkUnitStr);
                    //                            wks.Cells[artWorkStart + 1, 1].Value = string.Format("Year:{0}", this.intYear);
                    //                            wks.Cells[artWorkStart + 1, 3].Value = string.Format("Print Type:< {0} >", this.SourceStr);
                    //                            wks.Cells[artWorkStart + 1, 8].Value = string.Format("By {0}                             Buyer : {1}", this.isSCIDelivery ? "Sci Delivery" : "Buyer Delivery", this.BrandID);
                    //                        }
                    //                        else
                    //                        {
                    //                            wks.Cells[artWorkStart, 1].Value = string.Format("Factory Capacity by Month Report  (Half Month)", art + " " + artworkUnitStr);
                    //                            wks.Cells[artWorkStart + 1, 1].Value = string.Format("Year:{0} Month:{1}", this.intYear, this.intMonth);
                    //                            wks.Cells[artWorkStart + 1, 8].Value = "By " + (this.isSCIDelivery ? "Sci Delivery" : "Buyer Delivery");
                    //                        }

                    // this.sheetStart += 3; // 每個Artwork間隔 n - 1 格
                    //                    }

                    // sxrc.Save(Sci.Production.Class.MicrosoftFile.GetName("Planning_Report_R10"));
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
                if (!this.txtMDivision.Text.Empty())
                {
                    sqlWheres.Add(" f.MDivisionID = '" + this.mDivisionID + "'");
                    loadingWheres.Add("o.MDivisionID ='" + this.mDivisionID + "'");

                    if (!this.txtFactory.Text.Empty())
                    {
                        workWheres.Add(" w.FactoryID ='" + this.Fty + "'");
                    }

                    if (this.txtFactory.Text.Empty())
                    {
                        workWheres.Add(" exists (select 1 from Factory WITH (NOLOCK) where MDivisionID = '" + this.mDivisionID + "' and ID = w.FactoryID)");
                    }
                }

                if (!this.txtFactory.Text.Empty())
                {
                    sqlWheres.Add(" f.ID = '" + this.Fty + "'");
                    loadingWheres.Add(" o.Factoryid ='" + this.Fty + "'");
                    if (this.txtMDivision.Text.Empty())
                    {
                        workWheres.Add(" w.FactoryID ='" + this.Fty + "'");
                    }
                }

                if (!this.TxtZone.Text.Empty())
                {
                    sqlWheres.Add(" f.Zone = '" + this.zone + "'");
                    loadingWheres.Add(" f.Zone ='" + this.zone + "'");
                    if (this.txtMDivision.Text.Empty())
                    {
                        workWheres.Add(" exists (select 1 from Factory WITH (NOLOCK) where zone = '" + this.zone + "' and ID = w.FactoryID)");
                    }
                }

                if (!this.txtBrand1.Text.Empty())
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

                #region --Production Status Excel第一個頁籤SQL
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
                                  Left Join Factory f WITH (NOLOCK) on f.id = o.factoryID
                	              Cross Apply getOutputInformation(o.ID, '{3}') si
                	              Where o.BuyerDelivery between '{2}' and '{3}'
                	              And o.Junk = 0
                	              And o.SubconInType in ('1','2')
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
                                   IIF(AccuHours* MonthHours=0,0,round(a.LoadCPU/AccuHours* MonthHours,10)) as AccuLoad
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

                #region --Production Statuss Excel第二個頁籤SQL
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

        // private List<SqlParameter> NewMethod(string art)
        // {
        //    return new List<SqlParameter>
        //                    {
        //                        new SqlParameter("@ReportType", this.reportType),
        //                        new SqlParameter("@BrandID", this.BrandID),
        //                        new SqlParameter("@ArtWorkType", art),
        //                        new SqlParameter("@isSCIDelivery", this.isSCIDelivery),
        //                        new SqlParameter("@Year", this.intYear),
        //                        new SqlParameter("@Month", this.intMonth),
        //                        new SqlParameter("@SourceStr", this.SourceStr),
        //                        new SqlParameter("@M", this.mDivisionID),
        //                        new SqlParameter("@Fty", this.Fty),
        //                        new SqlParameter("@Zone", this.zone)
        //                    };
        // }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            #region raProductionStatus
            if (this.radioProductionStatus.Checked == true)
            {
                if (this.dt == null || this.dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                SaveXltReportCls xl = new SaveXltReportCls("Planning_R10_ProuctionStatus.xltx", keepApp: true);
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

                xl.Save(Class.MicrosoftFile.GetName("Planning_R10_ProuctionStatus"));

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

        /// <inheritdoc/>
        public DualResult RunPlanningR10Report(bool saveReport, bool isByCPU, int reportType, List<string> artworkLis, Dictionary<string, object> paras)
        {
            var dic = new Dictionary<string, Tuple<DataTable[], DualResult>>();
            var tmpDir = Env.Cfg.ReportTempDir;
            var intYear = MyUtility.Convert.GetInt(paras["Year"]);
            var intMonth = MyUtility.Convert.GetInt(paras["Month"]);
            var sourceStr = paras["SourceStr"].ToString();
            var isSCIDelivery = MyUtility.Convert.GetBool(paras["isSCIDelivery"]);
            var brandID = paras["BrandID"].ToString();
            var mdivisionID = paras["MDivisionID"].ToString();
            var Fty = paras["Fty"].ToString();
            var byBrand = paras.ContainsKey("CalculateByBrand") ? Convert.ToBoolean(paras["CalculateByBrand"]) : false;
            var zone = paras["Zone"].ToString();
            DualResult result = new DualResult(true);

            var isErrorOccured = false;
            var errorMsg = string.Empty;

            string xltPath = string.Empty;
            string strHeaderRange = string.Empty;
            if (reportType == 1)
            {
                xltPath = @"Planning_R10_01.xltx";
                strHeaderRange = "A2:O4";
            }
            else
            {
                xltPath = @"Planning_R10_02.xltx";
                strHeaderRange = "A2:Q5";
            }

            SaveXltReportCls sxrc = new SaveXltReportCls(xltPath);
            Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;

            dic.Clear();

            var exelis = artworkLis
                .AsParallel()
                .Select(art => this.ExecSP1(art, paras, dic))
                .AsSequential()
                .ToList();

            int titleHeigh = (reportType == 1) ? 3 : 4;
            int sheetStart = 2; // 起始位置
            int sheetStart_sum = 5; // 起始位置
            int spaceRow = 3; // 每個Artwork間隔 n - 1 格空白列

            /*#if DEBUG
                            sxrc.ExcelApp.Visible = true;
            #endif*/

            try
            {
                foreach (string art in artworkLis)
                {
                    string artSeq = MyUtility.GetValue.Lookup($"select Seq from ArtworkType where Id = '{art}'");
                    string artworkUnit = MyUtility.GetValue.Lookup($"select ArtworkUnit from ArtworkType where Id = '{art}'");
                    string artworkUnit2 = MyUtility.GetValue.Lookup($"select ProductionUnit from ArtworkType where Id = '{art}'");
                    string artworkUnitStr =
                        art == "SEWING" ? "CPU"
                        : artworkUnit == "STITCH" ? "STITCH in thousands"
                        : artworkUnit == "PCS" ? "PCS"
                        : artworkUnit2 == "QTY" ? "QTY"
                        : isByCPU == true ? "CPU"
                        : "TMS/Min";

                    // CopyHeader
                    if (artworkLis.IndexOf(art) > 0)
                    {
                        Microsoft.Office.Interop.Excel.Range desRg = wks.get_Range(string.Format("A{0}:A{0}", sheetStart.ToString()));
                        wks.get_Range(strHeaderRange).Copy();
                        desRg.PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAll); // Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAllExceptBorders
                    }

                    // 修改Header
                    if (reportType == 1)
                    {
                        wks.Cells[sheetStart, 1].Value = string.Format("Factory Capacity by Month Report  {0}", art + " " + artworkUnitStr);
                        wks.Cells[sheetStart + 1, 1].Value = string.Format("Year:{0}", intYear);
                        wks.Cells[sheetStart + 1, 3].Value = string.Format("Print Type:< {0} >", sourceStr);
                        wks.Cells[sheetStart + 1, 8].Value = string.Format(
                            "By {0}                             Buyer : {1}",
                            isSCIDelivery ? "Sci Delivery" : "Buyer Delivery",
                            brandID);

                        if (sheetStart < 5)
                        {
                            sxrc.ExcelApp.Worksheets[2].Cells[sheetStart + 1, 1].Value = string.Format("Year:{0}", intYear);
                            sxrc.ExcelApp.Worksheets[2].Cells[sheetStart + 1, 5].Value = string.Format("Print Type:< {0} >", sourceStr);
                            sxrc.ExcelApp.Worksheets[2].Cells[sheetStart + 1, 10].Value = string.Format(
                                "By {0}                             Buyer : {1}",
                                isSCIDelivery ? "Sci Delivery" : "Buyer Delivery",
                                brandID);
                        }
                    }
                    else
                    {
                        wks.Cells[sheetStart, 1].Value = string.Format("Factory Capacity by Month Report {0}   (Half Month)", artSeq + " - " + art + " " + artworkUnitStr);
                        wks.Cells[sheetStart + 1, 1].Value = string.Format("Year:{0} Month:{1}", intYear, intMonth);
                        wks.Cells[sheetStart + 1, 8].Value = "By " + (isSCIDelivery ? "Sci Delivery" : "Buyer Delivery");
                    }

                    sheetStart += titleHeigh;

                    // 填入表身
                    if (dic[art].Item2 == true)
                    {
                        if (reportType == 1)
                        {
                            this.TransferReport1(dic[art].Item1, reportType, intYear, byBrand, zone, mdivisionID, Fty, sxrc.ExcelApp.Worksheets[1], ref sheetStart);
                            this.TransferReport1_Summary(dic[art].Item1, intYear, sxrc.ExcelApp.Worksheets[2], ref sheetStart_sum, art, artworkUnitStr);
                        }
                        else
                        {
                            this.TransferReport2(dic[art].Item1, reportType, intYear, intMonth, isSCIDelivery, zone, mdivisionID, Fty, sxrc.ExcelApp.Worksheets[1], ref sheetStart);
                        }
                    }
                    else
                    {
                        // 載入失敗
                        wks.Cells[sheetStart, 1].Value = $"get data error : {dic[art].Item2.ToString()}";
                        isErrorOccured = true;
                        errorMsg = $"get data error : {dic[art].Item2.ToString()}";
                    }

                    sheetStart += spaceRow;
                }
            }
            catch (Exception ex)
            {
                return Ict.Result.F(ex.ToString());
            }

            if (saveReport)
            {
                sxrc.ExcelApp.Visible = false;
                sxrc.BoOpenFile = false;

                // 2020/02/21 [IST20200294] modify by Anderson 經由批次轉廠呼叫時，當發生錯誤時回報錯誤，不產生報表
                if (isErrorOccured)
                {
                    result = Ict.Result.F(errorMsg);
                }
                else
                {
                    var savePath = Path.Combine(tmpDir, $"{mdivisionID}_Planning-R10_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.xlsx");
                    sxrc.Save(savePath);
                    result = Ict.Result.True;
                }
            }
            else
            {
                sxrc.ExcelApp.Visible = true;
            }

            return result;
        }

        /// <inheritdoc/>
        private bool ExecSP1(string art, Dictionary<string, object> paras, Dictionary<string, Tuple<DataTable[], DualResult>> dic)
        {
            var plis = new List<SqlParameter>
            {
                new SqlParameter("ArtWorkType", art),
            };

            paras.AsEnumerable().ToList().ForEach(x =>
            {
                plis.Add(new SqlParameter(x.Key, x.Value));
            });

            DataTable[] datas;
            DualResult res = DBProxy.Current.SelectSP(string.Empty, "Planning_Report_R10", plis, out datas);

            lock (dic)
            {
                dic.Add(art, new Tuple<DataTable[], DualResult>(datas, res));
            }

            return true;
        }

        /// <summary>
        /// Report1，開啟xlt填入資料
        /// </summary>
        /// <param name="datas"> dataset from procedures </param>
        /// <param name="reportType">reportType</param>
        /// <param name="intYear">intYear</param>
        /// <param name="byBrand">byBrand</param>
        /// <param name="zone">zone</param>
        /// <param name="mdivision">mdivision</param>
        /// <param name="fty">fty</param>
        /// <param name="wks"> work sheet </param>
        /// <param name="sheetStart"> sheet index </param>
        /// <returns> excute success or not </returns>
        private DualResult TransferReport1(DataTable[] datas, int reportType, int intYear, bool byBrand, string zone, string mdivision, string fty, Microsoft.Office.Interop.Excel.Worksheet wks, ref int sheetStart)
        {
            wks.Activate();
            int artworkStart = sheetStart;
            //// Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;
            //// wks.Application.Visible = true;

            // For Country
            int mdvIdx = 0; // 每個MDV所在的Index，抓sheetStart，在Country下面
            int mdvTotalIdx = 0;
            List<string> lisCtyIdx = new List<string>();
            List<string> lisMDVTTLIdx = new List<string>();
            List<string> lisOutputIdx = new List<string>(); // By Country

            DataTable dtList = datas[0];
            DataTable dt1 = datas[1]; // [1] By Factory Capacity
            DataTable dt2 = datas[3].Select("SubconInSisterFty = 0 AND isOrder = 0").TryCopyToDataTable(datas[3]); // [2] non Sister
            DataTable dt3 = datas[2]; // [3] For Forecast shared
            DataTable dt4 = datas[3].Select("SubconInSisterFty = 0 AND SubconInType <> '2'").TryCopyToDataTable(datas[3]); // [4] For Output, 及Output後面的Max日期
            DataTable dt5 = datas[3].Select("SubconInType = '2'").TryCopyToDataTable(datas[3]); // [4] For Output, 及Output後面的Max日期

            string filterZoneMdivisionAdd =
                (zone == string.Empty ? string.Empty : $" and Zone = '{zone}'")
                + (mdivision == string.Empty ? string.Empty : $" and MdivisionID = '{mdivision}'")
                + (fty == string.Empty ? string.Empty : $" and FactoryID = '{fty}'");

            string filterZoneMdivisionDiff =
                (zone == string.Empty ? string.Empty : $" and Zone2 = '{zone}'")
                + (mdivision == string.Empty ? string.Empty : $" and MdivisionID2 = '{mdivision}'")
                + (fty == string.Empty ? string.Empty : $" and FactoryID = '{fty}'");

            DataTable dtCountryList = dtList.DefaultView.ToTable(true, "CountryID", "CountryName");
            List<string> lisBold = new List<string>();
            List<string> lisPercent = new List<string>();
            for (int idxCty = 0; idxCty < dtCountryList.Rows.Count; idxCty++)
            {
                lisBold.Add(sheetStart.ToString());
                string countryID = dtCountryList.Rows[idxCty]["CountryID"].ToString();
                DataTable dtCountry = this.SafeGetDt(dt1, $"CountryID = '{countryID}'");

                // if (dtCountry.Rows.Count == 0) continue;
                // string CountryName = dtCountry.Rows[0]["CountryName"].ToString();
                string countryName = dtCountryList.Rows[idxCty]["CountryName"].ToString();

                lisCtyIdx.Add(sheetStart.ToString());
                this.SetTableToRow(wks, intYear, sheetStart, countryName, dtCountry, "FtyTmsCapa");
                sheetStart += 1;

                DataTable dtMDVList = this.SafeGetDt(dtList, $"CountryID = '{countryID}'").DefaultView.ToTable(true, "MDivisionID");
                List<string> lisSumFtyNonSis = new List<string>();
                List<string> shortageLis = new List<string>();

                this.SetColumnToBack(dtMDVList, "MDivisionID", "Sample");
                this.SetColumnToBack(dtMDVList, "MDivisionID", string.Empty);
                bool isSample = false;
                for (int idxMDV = 0; idxMDV < dtMDVList.Rows.Count; idxMDV++)
                {
                    lisBold.Add(sheetStart.ToString());

                    // 3 單一某個MDV加總
                    mdvIdx = sheetStart;
                    string mdivisionID = dtMDVList.Rows[idxMDV]["MDivisionID"].ToString();

                    isSample = mdivisionID == "Sample";

                    DataTable dtOneMDV = this.SafeGetDt(dtCountry, $"MDivisionID = '{mdivisionID}'");
                    this.SetTableToRow(wks, intYear, sheetStart, mdivisionID, dtOneMDV, "FtyTmsCapa");
                    sheetStart += 1;

                    // 4 Factory Data，這裡需要迴圈For每個工廠
                    DataTable dtFactoryList = this.SafeGetDt(dtList, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'").DefaultView.ToTable(true, "FactoryID");

                    int ftyStart = sheetStart;
                    foreach (DataRow row in dtFactoryList.Rows)
                    {
                        string factoryID = row["FactoryID"].ToString();
                        wks.Cells[sheetStart, 1].Value = factoryID;

                        DataTable dtFactory = this.SafeGetDt(dt1, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}' and FactoryID = '{factoryID}'");
                        DataTable dtFactoryAdd = this.SafeGetDt(dt5, $"CountryID = '{countryID}' and MDivisionID = '{mdivisionID}' and FactoryID = '{factoryID}'");
                        DataTable dtFactoryDiff = this.SafeGetDt(dt5, $"CountryID2 = '{countryID}' and MDivisionID2 = '{mdivisionID}' and FactoryID2 = '{factoryID}'");

                        this.SetTableToRow_SubconInType(
                            wks,
                            intYear,
                            sheetStart,
                            factoryID,
                            dtFactory,
                            dtFactoryAdd,
                            dtFactoryDiff,
                            "Capacity",
                            "OrderCapacity");

                        DataRow[] tmprows = dtFactory.Select($"FactoryID = '{factoryID}'");

                        // var tms = dtFactory.Select($"FactoryID = '{FactoryID}'")[0]["Tms"];
                        wks.Cells[sheetStart, 15].Value = tmprows.Length > 0 && byBrand == false ? MyUtility.Convert.GetDecimal(tmprows[0]["Tms"]) : 0;
                        sheetStart += 1;
                    }

                    // 5 By non-sister
                    int nonSisStart = sheetStart;
                    DataTable dtByNonSister = SafeGetDt(dt2, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    SetTableToRow(wks, intYear, sheetStart, "non-sister sub-in", dtByNonSister, "OrderCapacity");
                    DrawBottomLine(wks, sheetStart, 1);
                    sheetStart += 1;

                    lisSumFtyNonSis.Add(ftyStart.ToString() + "," + nonSisStart.ToString());

                    // Shortage
                    int shortageStart = sheetStart;
                    DataTable dtByShortage = SafeGetDt(dt1, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    SetTableToRow(wks, intYear, sheetStart, "Shortage", dtByShortage, "OrderShortage");
                    DrawBottomLine(wks, sheetStart, 1);
                    shortageLis.Add(sheetStart.ToString());
                    sheetStart += 1;

                    if (isSample)
                    {
                        continue;
                    }

                    // MDV total
                    mdvTotalIdx = sheetStart;
                    this.SetFormulaToRow(wks, reportType, sheetStart, mdivisionID + " total", string.Format("=SUM({{0}}{0}:{{0}}{1}) - {{0}}{2}", ftyStart, nonSisStart, shortageStart));

                    this.DrawBottomLine(wks, sheetStart, 1);
                    sheetStart += 1;

                    // 6 ForecastCapacity
                    lisPercent.Add(sheetStart.ToString());
                    DataTable dtForecastCapacityByMDV = this.SafeGetDt(dt3, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    wks.Cells[sheetStart, 1].Value = $"{mdivisionID} Forecast shared";
                    for (int mon = 1; mon < 13; mon++)
                    {
                        var forCapa = dtForecastCapacityByMDV.Compute("SUM(Capacity)", string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                        forCapa = (forCapa == DBNull.Value) ? 0 : forCapa;
                        wks.Cells[sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), mdvTotalIdx, forCapa);
                    }

                    var sumforcapaMDV = dtForecastCapacityByMDV.Compute("SUM(Capacity)", string.Empty);
                    wks.Cells[sheetStart, 14].Value = string.Format("=if({0} > 0,({0}) / SUM({1}{3}:{2}{3}),0)", (sumforcapaMDV == DBNull.Value) ? 0 : sumforcapaMDV, MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), mdvTotalIdx);
                    sheetStart += 1;

                    // MDV 1 Loading - CAPA
                    this.SetFormulaToRow(wks, reportType, sheetStart, $"{mdivisionID} Loading - CAPA", string.Format("=({{0}}{0} - {{0}}{1})", mdvTotalIdx, mdvIdx), EnuDrawColor.Normal);
                    sheetStart += 1;

                    // MDV FILL RATE
                    lisPercent.Add(sheetStart.ToString());
                    this.SetFormulaToRow(wks, reportType, sheetStart, $"{mdivisionID} FILL RATE", string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", mdvIdx, mdvTotalIdx));

                    this.DrawBottomLine(wks, sheetStart, 1);
                    sheetStart += 1;

                    // Max(OutputDate)
                    // Order+FactoryOrder 的 SewCapacity
                    DataTable dtOutputMDV = this.SafeGetDt(dt4, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    DataTable dtOutputMDVAdd = this.SafeGetDt(dt5, $"CountryID = '{countryID}' and MdivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    DataTable dtOutputMDVDiff = this.SafeGetDt(dt5, $"CountryID2 = '{countryID}' and MdivisionID2 = '{mdivisionID}'" + filterZoneMdivisionDiff);
                    string maxSewOutPut = dtOutputMDV.Compute("MAX(SewingYYMM)", string.Empty).ToString();
                    maxSewOutPut = maxSewOutPut.Length > 0 ? maxSewOutPut.Substring(5, maxSewOutPut.Length - 5) : string.Empty;

                    this.SetTableToRow_SubconInType(
                        wks,
                        intYear,
                        sheetStart,
                        $"{mdivisionID} Output ({maxSewOutPut})",
                        dtOutputMDV,
                        dtOutputMDVAdd,
                        dtOutputMDVDiff,
                        "SewCapacity",
                        "SewCapacity");

                    sheetStart += 1;

                    // MDV Output  Rate
                    lisPercent.Add(sheetStart.ToString());
                    this.SetFormulaToRow(wks, reportType, sheetStart, $"{mdivisionID} Output  Rate", string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", mdvTotalIdx, sheetStart - 1));

                    this.DrawBottomLine(wks, sheetStart, 2);

                    sheetStart += 1;
                }

                // CountryID Grand TTL
                mdvTotalIdx = sheetStart;
                lisMDVTTLIdx.Add(sheetStart.ToString());
                string sumFtyStr = "=";
                for (int i = 0; i < lisSumFtyNonSis.Count; i++)
                {
                    string str = lisSumFtyNonSis[i];
                    string str2 = shortageLis[i];
                    sumFtyStr += string.Format("+SUM({{0}}{0}:{{0}}{1}) - {{0}}{2}", str.Split(',')[0], str.Split(',')[1], str2);
                }

                this.SetFormulaToRow(wks, reportType, sheetStart, $"{countryID} Grand TTL", sumFtyStr);

                this.DrawBottomLine(wks, sheetStart, 1);
                sheetStart += 1;

                // CountryID Forecast shared
                lisPercent.Add(sheetStart.ToString());
                DataTable dtForecastCapacityByCty = this.SafeGetDt(dt3, $"CountryID = '{countryID}'" + filterZoneMdivisionAdd);
                wks.Cells[sheetStart, 1].Value = $"{countryID} Forecast shared";
                for (int mon = 1; mon < 13; mon++)
                {
                    var forCapa = dtForecastCapacityByCty.Compute("SUM(Capacity)", string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                    forCapa = (forCapa == DBNull.Value) ? 0 : forCapa;
                    wks.Cells[sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), mdvTotalIdx, forCapa);
                }

                var sumforcapaCty = dtForecastCapacityByCty.Compute("SUM(Capacity)", string.Empty);
                wks.Cells[sheetStart, 14].Value = string.Format("=if({0} > 0, ({0}) / SUM({1}{3}:{2}{3}), 0)", (sumforcapaCty == DBNull.Value) ? 0 : sumforcapaCty, MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), mdvTotalIdx);
                sheetStart += 1;

                // CountryID Loading - CAPA
                this.SetFormulaToRow(wks, reportType, sheetStart, $"{countryID} Loading - CAPA", string.Format("=({{0}}{0} - {{0}}{1})", sheetStart - 2, lisCtyIdx[lisCtyIdx.Count - 1]), EnuDrawColor.Normal);
                sheetStart += 1;

                // CountryID FILL Rate
                lisPercent.Add(sheetStart.ToString());
                this.SetFormulaToRow(wks, reportType, sheetStart, $"{countryID} FILL Rate", string.Format("=IF({{0}}{1}>0,{{0}}{0}/{{0}}{1},0)", sheetStart - 3, lisCtyIdx[lisCtyIdx.Count - 1]));
                sheetStart += 1;

                // CountryID Output()
                lisOutputIdx.Add(sheetStart.ToString());
                DataTable dtOutputCty = this.SafeGetDt(dt4, $"CountryID = '{countryID}'" + filterZoneMdivisionAdd);
                DataTable dtOutputCtyAdd = this.SafeGetDt(dt5, $"CountryID = '{countryID}'" + filterZoneMdivisionAdd);
                DataTable dtOutputCtyDiff = this.SafeGetDt(dt5, $"CountryID2 = '{countryID}'" + filterZoneMdivisionDiff);
                string maxSewOutPutCty = dtOutputCty.Compute("MAX(SewingYYMM)", string.Empty).ToString();
                maxSewOutPutCty = maxSewOutPutCty.Length > 0 ? maxSewOutPutCty.Substring(5, maxSewOutPutCty.Length - 5) : string.Empty;

                this.SetTableToRow_SubconInType(
                    wks,
                    intYear,
                    sheetStart,
                    $"{countryID} Output ({maxSewOutPutCty})",
                    dtOutputCty,
                    dtOutputCtyAdd,
                    dtOutputCtyDiff,
                    "SewCapacity",
                    "SewCapacity");

                sheetStart += 1;

                // CountryID Output  Rate
                lisPercent.Add(sheetStart.ToString());
                this.SetFormulaToRow(wks, reportType, sheetStart, $"{countryID} Output  Rate", string.Format("=IF({0}{1} > 0, {0}{2} / {0}{1},0)", "{0}", mdvTotalIdx, sheetStart - 1));

                this.DrawBottomLine(wks, sheetStart, 3);

                sheetStart += 1;
            }

            // Total Capacity
            lisBold.Add(sheetStart.ToString());
            string totalStr = "=";
            foreach (string str in lisCtyIdx)
            {
                totalStr += string.Format("+{0}{1}", "{0}", str);
            }

            this.SetFormulaToRow(wks, reportType, sheetStart, "Total Capacity", totalStr);

            sheetStart += 1;

            // Total Loading
            lisBold.Add(sheetStart.ToString());
            string totalLoadStr = "=";
            foreach (string str in lisMDVTTLIdx)
            {
                totalLoadStr += string.Format("+{0}{1}", "{0}", str);
            }

            this.SetFormulaToRow(wks, reportType, sheetStart, "Total Loading", totalLoadStr);

            sheetStart += 1;

            // Total Forecast shared
            lisBold.Add(sheetStart.ToString());
            lisPercent.Add(sheetStart.ToString());
            DataTable dtForecastCapacity = dt3.Select("1 = 1" + filterZoneMdivisionAdd).TryCopyToDataTable(dt3);
            wks.Cells[sheetStart, 1].Value = "Total FC shared";
            decimal sumforCapa = 0;
            for (int mon = 1; mon < 13; mon++)
            {
                var capa = dtForecastCapacity.Compute("SUM(Capacity)", string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                decimal forCapa = capa == DBNull.Value ? 0 : Convert.ToDecimal(capa);
                sumforCapa += forCapa;
                wks.Cells[sheetStart, mon + 1] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(mon + 1), sheetStart - 1, forCapa);
            }

            wks.Cells[sheetStart, 14] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(14), sheetStart - 1, sumforCapa);
            sheetStart += 1;

            // Total Loading - CAPA
            lisBold.Add(sheetStart.ToString());
            this.SetFormulaToRow(wks, reportType, sheetStart, "Loading-CAPA", string.Format("=({0}{1} - {0}{2})", "{0}", sheetStart - 2, sheetStart - 3), EnuDrawColor.Normal);
            sheetStart += 1;

            // FILL Rate
            lisBold.Add(sheetStart.ToString());
            lisPercent.Add(sheetStart.ToString());
            this.SetFormulaToRow(wks, reportType, sheetStart, "FIll RATE", string.Format("=IF({0}{2}>0,{0}{1}/{0}{2},0)", "{0}", sheetStart - 3, sheetStart - 4));
            sheetStart += 1;

            // Output()
            lisBold.Add(sheetStart.ToString());
            string outPutStr = "=";
            foreach (string str in lisOutputIdx)
            {
                outPutStr += string.Format("+{{0}}{0}", str);
            }

            DataTable dtOutput = dt4;
            string maxSewOutPutT = dtOutput.Compute("MAX(SewingYYMM)", string.Empty).ToString();
            maxSewOutPutT = maxSewOutPutT.Length > 0 ? maxSewOutPutT.Substring(5, maxSewOutPutT.Length - 5) : string.Empty;

            this.SetFormulaToRow(wks, reportType, sheetStart, $"Output ({maxSewOutPutT})", outPutStr);
            sheetStart += 1;

            // Output  Rate
            lisPercent.Add(sheetStart.ToString());
            lisBold.Add(sheetStart.ToString());
            this.SetFormulaToRow(wks, reportType, sheetStart, "Output  Rate", string.Format("=IF({{0}}{0} > 0, {{0}}{1} / {{0}}{0},0)", sheetStart - 5, sheetStart - 1));

            // 第一排置中
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + artworkStart.ToString(), MyExcelPrg.GetExcelColumnName(1) + sheetStart.ToString());
            rg.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Country, MDvision, Total 第一格粗體
            foreach (string idx in lisBold)
            {
                string rgStr = string.Format("{0}{1}:{0}{1}", MyExcelPrg.GetExcelColumnName(1), idx);
                rg = wks.get_Range(rgStr);
                rg.Font.Bold = true;
            }

            // 數值格式
            string lastCell = MyExcelPrg.GetExcelColumnName(15) + sheetStart.ToString();
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(2) + artworkStart, lastCell);
            rg.Cells.NumberFormat = "##,###,##0";

            // Total欄左右邊線
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(14) + artworkStart, MyExcelPrg.GetExcelColumnName(14) + sheetStart.ToString());
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

            return Ict.Result.True;
        }

        /// <summary>
        /// Report1，開啟xlt填入資料
        /// </summary>
        /// <param name="datas"> dataset from procedures </param>
        /// <param name="intYear">intYear</param>
        /// <param name="wks"> work sheet </param>
        /// <param name="sheetStart"> sheet index </param>
        /// <param name="artworkName"> artwork name </param>
        /// <param name="artworkUnit"> artwork unit </param>
        /// <returns> excute success or not </returns>
        private DualResult TransferReport1_Summary(DataTable[] datas, int intYear, Microsoft.Office.Interop.Excel.Worksheet wks, ref int sheetStart, string artworkName, string artworkUnit)
        {
            int artworkStart = sheetStart;
            string artworkSeq = MyUtility.GetValue.Lookup($"select Seq from ArtworkType where Id = '{artworkName}'");

            // wks.Application.Visible = true;
            // For Country
            List<string> lisCtyIdx = new List<string>();

            DataTable dtList = datas[0];
            DataTable dt1 = datas[1]; // [1] By Factory Capacity
            DataTable dt5 = datas[3].Select("SubconInType = '2'").TryCopyToDataTable(datas[3]);
            DataTable dt6 = datas[3].Select("SubconInSisterFty = 0 AND isOrder = 0").TryCopyToDataTable(datas[3]);

            // 列出Sample的工廠，並且移除掉
            dtList = dtList.AsEnumerable().Where(rr => rr["MdivisionID"].ToString() != "Sample").TryCopyToDataTable(dtList);
            dt1 = dt1.AsEnumerable().Where(rr => rr["MdivisionID"].ToString() != "Sample").TryCopyToDataTable(dt1);

            DataTable dtCountryList = dtList.DefaultView.ToTable(true, "CountryID", "CountryName");
            List<string> lisBold = new List<string>();
            List<string> lisPercent = new List<string>();
            List<string> lisCty = new List<string>();
            List<string> lisM = new List<string>();
            for (int idxCty = 0; idxCty < dtCountryList.Rows.Count; idxCty++)
            {
                lisCty.Add(sheetStart.ToString());
                lisBold.Add(sheetStart.ToString());
                string countryID = dtCountryList.Rows[idxCty]["CountryID"].ToString();
                DataTable dtCountry = this.SafeGetDt(dt1, string.Format("CountryID = '{0}'", countryID));

                // if (dtCountry.Rows.Count == 0) continue;
                // string CountryName = dtCountry.Rows[0]["CountryName"].ToString();
                string countryName = dtCountryList.Rows[idxCty]["CountryName"].ToString();

                lisCtyIdx.Add(sheetStart.ToString());
                this.SetTableToRow_sum(wks, intYear, sheetStart, countryName, artworkSeq + " - " + artworkName, artworkUnit, "-", dtCountry, "FtyTmsCapa");
                sheetStart += 1;

                DataTable dtM = this.SafeGetDt(dtList, string.Format("CountryID = '{0}'", countryID)).DefaultView.ToTable(true, "MdivisionID");
                DataTable dtFactorySB2 = this.SafeGetDt(dt5, string.Format("CountryID = '{0}'", countryID));
                DataTable dtNonSister = this.SafeGetDt(dt6, string.Format("CountryID = '{0}'", countryID));
                bool isSample = false;
                for (int idxM = 0; idxM < dtM.Rows.Count; idxM++)
                {
                    // 3 BY MdivisionID
                    lisM.Add(sheetStart.ToString());

                    string mID = dtM.Rows[idxM]["MdivisionID"].ToString();

                    isSample = mID == "Sample";

                    DataTable dtOneM = this.SafeGetDt(dtCountry, string.Format("MdivisionID = '{0}'", mID));

                    // Capacity(CPU)
                    this.SetTableToRow_sum(wks, intYear, sheetStart, mID, artworkSeq + " - " + artworkName, artworkUnit, "Capacity", dtOneM, "FtyTmsCapa");
                    sheetStart += 1;

                    // Loading(CPU)
                    wks.Cells[sheetStart, 1].Value = mID;
                    wks.Cells[sheetStart, 2].Value = artworkSeq + " - " + artworkName;
                    wks.Cells[sheetStart, 3].Value = artworkUnit;
                    wks.Cells[sheetStart, 4].Value = "Loading";
                    for (int mon = 1; mon < 13; mon++)
                    {
                        DataRow[] rows = dtOneM.Select(string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                        DataRow[] rowsSB2add = dtFactorySB2.Select(string.Format("MdivisionID = '{0}' and Month = '{1}'", mID, intYear.ToString() + mon.ToString("00")));
                        DataRow[] rowsSB2diff = dtFactorySB2.Select(string.Format("MdivisionID2 = '{0}' and Month = '{1}'", mID, intYear.ToString() + mon.ToString("00")));
                        DataRow[] rowsSB2addNonSis = dtNonSister.Select(string.Format("MdivisionID = '{0}' and Month = '{1}'", mID, intYear.ToString() + mon.ToString("00")));
                        decimal capa = rows.Length > 0 ? rows.Sum(rr => MyUtility.Convert.GetDecimal(rr["trueCapa"])) : 0;
                        decimal vSB2add = rowsSB2add.Length > 0 ? rowsSB2add.Sum(rr => MyUtility.Convert.GetDecimal(rr["OrderCapacity"])) : 0;
                        decimal vSB2diff = rowsSB2diff.Length > 0 ? rowsSB2diff.Sum(rr => MyUtility.Convert.GetDecimal(rr["OrderCapacity"])) : 0;
                        decimal vSB2addNonSis = rowsSB2addNonSis.Length > 0 ? rowsSB2addNonSis.Sum(rr => MyUtility.Convert.GetDecimal(rr["OrderCapacity"])) : 0;
                        /*string sb2Str = string.Empty;

                        if (vSB2add > 0)
                        {
                            sb2Str += $" + {vSB2add}";
                        }

                        if (vSB2diff > 0)
                        {
                            sb2Str += $" - {vSB2diff}";
                        }

                        if (sb2Str != string.Empty)
                        {
                            wks.Cells[sheetStart, mon + 4].Interior.Color = Color.Yellow;
                        }

                        wks.Cells[sheetStart, mon + 4].Value = $"= {capa}{sb2Str}";
                        */
                        capa = capa + vSB2add - vSB2diff + vSB2addNonSis;
                        wks.Cells[sheetStart, mon + 4].Value = capa;
                    }

                    wks.Cells[sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), sheetStart);
                    sheetStart += 1;

                    // Fill Rate(%)
                    wks.Cells[sheetStart, 1] = mID;
                    wks.Cells[sheetStart, 2] = artworkSeq + " - " + artworkName;
                    wks.Cells[sheetStart, 3] = artworkUnit;
                    wks.Cells[sheetStart, 4] = "Fill Rate(%)";
                    wks.Cells[sheetStart, 4].Interior.Color = Color.FromArgb(235, 241, 222);

                    for (int i = 5; i < 18; i++)
                    {
                        wks.Cells[sheetStart, i] = $"=if({MyExcelPrg.GetExcelColumnName(i)}{sheetStart - 2} = 0, 0, {MyExcelPrg.GetExcelColumnName(i)}{sheetStart - 1} / {MyExcelPrg.GetExcelColumnName(i)}{sheetStart - 2})";
                        Microsoft.Office.Interop.Excel.Range cell = wks.Cells[sheetStart, i];
                        if (cell.Value > 1)
                        {
                            cell.Interior.Color = Color.FromArgb(250, 191, 143);
                        }
                        else
                        {
                            cell.Interior.Color = Color.FromArgb(235, 241, 222);
                        }
                    }

                    lisPercent.Add(sheetStart.ToString());
                    sheetStart += 1;
                }
            }

            sheetStart -= 1;

            // 第一至三排置中
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + artworkStart.ToString(), MyExcelPrg.GetExcelColumnName(3) + sheetStart.ToString());
            rg.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Country, MDvision, Total 第一格粗體
            foreach (string idx in lisBold)
            {
                string rgStr = string.Format("{0}{1}:{0}{1}", MyExcelPrg.GetExcelColumnName(1), idx);
                rg = wks.get_Range(rgStr);
                rg.Font.Bold = true;

                rgStr = string.Format("{0}{1}:{0}{1}", MyExcelPrg.GetExcelColumnName(4), idx);
                rg = wks.get_Range(rgStr);
                rg.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            }

            // 數值格式
            string lastCell = MyExcelPrg.GetExcelColumnName(17) + sheetStart.ToString();
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(5) + artworkStart, lastCell);
            rg.Cells.NumberFormat = "##,###,##0";

            // 框線
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + artworkStart, lastCell);
            rg.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg.Borders.Weight = 2;

            foreach (var item in lisCty)
            {
                string rgStr = $"A{item}:{MyExcelPrg.GetExcelColumnName(17)}{item}";
                Microsoft.Office.Interop.Excel.Range tmprg = wks.get_Range(rgStr);
                tmprg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                tmprg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 3;
            }

            foreach (string idx in lisPercent)
            {
                string rgStr = string.Format("{0}{2}:{1}{2}", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(17), idx);
                rg = wks.get_Range(rgStr);
                rg.Cells.NumberFormat = "###,##0.00%";
            }

            // Auto Fit
            rg = wks.get_Range("A:D");
            rg.Columns.AutoFit();

            sheetStart += 1;

            return Ict.Result.True;
        }

        /// <summary>
        /// Report2，開啟xlt填入資料
        /// </summary>
        /// <param name="datas"> dataset from procedure </param>
        /// <param name="reportType">reportType</param>
        /// <param name="intYear">intYear</param>
        /// <param name="intMonth">intMonth</param>
        /// <param name="isSCIDelivery">isSCIDelivery</param>
        /// <param name="zone">zone</param>
        /// <param name="mdivision">mdivision</param>
        /// <param name="fty">fty</param>
        /// <param name="wks"> work sheet </param>
        /// <param name="sheetStart"> sheet index </param>
        /// <returns> execute success or not </returns>
        private DualResult TransferReport2(DataTable[] datas, int reportType, int intYear, int intMonth, bool isSCIDelivery, string zone, string mdivision, string fty, Microsoft.Office.Interop.Excel.Worksheet wks, ref int sheetStart)
        {
            // string xltPath = @"Planning_R02_02.xlt";
            // SaveXltReportCls sxrc = new SaveXltReportCls(xltPath);
            // Microsoft.Office.Interop.Excel.Worksheet wks = sxrc.ExcelApp.ActiveSheet;
            // sxrc.ExcelApp.Visible = true;
            int artworkStart = sheetStart;
            int mdvTotalIdx = 0;

            // Set Header
            DateTime startDate = new DateTime(intYear, intMonth, 1);
            for (int mon = 0; mon < 6; mon++)
            {
                if (isSCIDelivery)
                {
                    DateTime nextDate = startDate.AddMonths(mon);
                    wks.Cells[sheetStart - 2, 5 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 1).ToString("yyyy/MM");
                    wks.Cells[sheetStart - 1, 5 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 22).ToShortDateString();
                    wks.Cells[sheetStart - 1, 6 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 7).AddMonths(1).ToShortDateString();
                }
                else
                {
                    DateTime nextDate = startDate.AddMonths(mon);
                    wks.Cells[sheetStart - 2, 5 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 1).ToString("yyyy/MM");
                    wks.Cells[sheetStart - 1, 5 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 15).ToShortDateString();
                    wks.Cells[sheetStart - 1, 6 + (mon * 2)].Value = new DateTime(nextDate.Year, nextDate.Month, 1).GetLastDayOfMonth().ToShortDateString();
                }
            }

            DataTable dtList = datas[0];

            // DataTable dt0 = datas[1]; // [0] By Factory 最細的上下半月Capacity
            DataTable dt1 = datas[1]; // [1] By Factory Loading CPU
            DataTable dt2 = datas[2]; // [2] For Forecast shared
            DataTable dt3 = datas[3].Select("SubconInType <> '2'").TryCopyToDataTable(datas[3]); // [3] For Output, 及Output後面的Max日期
            DataTable dt5 = datas[3].Select("SubconInType = '2'").TryCopyToDataTable(datas[3]); // [3] SubconInType = '2'

            string filterZoneMdivisionAdd =
                (zone == string.Empty ? string.Empty : $" and Zone = '{zone}'")
                + (mdivision == string.Empty ? string.Empty : $" and MdivisionID = '{mdivision}'")
                + (fty == string.Empty ? string.Empty : $" and FactoryID = '{fty}'");

            string filterZoneMdivisionDiff =
                (zone == string.Empty ? string.Empty : $" and Zone2 = '{zone}'")
                + (mdivision == string.Empty ? string.Empty : $" and MdivisionID2 = '{mdivision}'")
                + (fty == string.Empty ? string.Empty : $" and FactoryID = '{fty}'");

            DataTable dtCountryList = dtList.DefaultView.ToTable(true, "CountryID");
            List<string> lisPercent = new List<string>();

            // For Country
            for (int idxCty = 0; idxCty < dtCountryList.Rows.Count; idxCty++)
            {
                string countryID = dtCountryList.Rows[idxCty]["CountryID"].ToString();
                DataTable dtCountry = this.SafeGetDt(dt1, string.Format("CountryID = '{0}'", countryID));
                if (dtCountry.Rows.Count == 0)
                {
                    continue;
                }

                string countryName = dtCountry.Rows[0]["CountryName"].ToString();
                wks.Cells[sheetStart, 1].Value = countryName;

                DataTable dtMDVList = this.SafeGetDt(dtList, string.Format("CountryID = '{0}'", countryID)).DefaultView.ToTable(true, "MDivisionID");

                List<string> lisCapaCty = new List<string>();
                List<string> lisLoadingCty = new List<string>();
                List<string> lisShortage = new List<string>();
                this.SetColumnToBack(dtMDVList, "MDivisionID", "Sample");
                this.SetColumnToBack(dtMDVList, "MDivisionID", string.Empty);
                int idx = 0;

                for (int idxMDV = 0; idxMDV < dtMDVList.Rows.Count; idxMDV++)
                {
                    string mdivisionID = dtMDVList.Rows[idxMDV]["MDivisionID"].ToString();
                    wks.Cells[sheetStart, 2].Value = mdivisionID;

                    DataTable dtOneMDV = this.SafeGetDt(dtCountry, string.Format("MDivisionID = '{0}'", mdivisionID));
                    DataTable dtFactory = this.SafeGetDt(dtOneMDV, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mdivisionID));
                    DataTable dtFactoryList = this.SafeGetDt(dtList, string.Format("CountryID = '{0}' And MDivisionID = '{1}'", countryID, mdivisionID)).DefaultView.ToTable(true, "FactoryID");
                    int ftyStart = sheetStart;
                    List<string> lisCapa = new List<string>();
                    List<string> lisLoading = new List<string>();
                    foreach (DataRow row in dtFactoryList.Rows)
                    {
                        string factoryID = row["FactoryID"].ToString();
                        wks.Cells[sheetStart, 3].Value = factoryID;
                        wks.Cells[sheetStart, 4].Value = "Capa.";
                        lisCapa.Add(sheetStart.ToString());
                        lisCapaCty.Add(sheetStart.ToString());
                        idx = 0;
                        for (int mon = intMonth; mon < intMonth + 6; mon++)
                        {
                            DataRow[] rows = dtFactory.Select(string.Format("FactoryID = '{0}' and MONTH = '{1}'", factoryID, this.GetCurrMonth(intYear, mon)));
                            decimal capacity1 = 0;
                            decimal capacity2 = 0;
                            if (rows.Length > 0)
                            {
                                for (int tmpRow = 0; tmpRow < rows.Length; tmpRow++)
                                {
                                    capacity1 += MyUtility.Convert.GetDecimal(rows[tmpRow]["FtyTmsCapa1"]);
                                    capacity2 += MyUtility.Convert.GetDecimal(rows[tmpRow]["FtyTmsCapa2"]);
                                }
                            }

                            wks.Cells[sheetStart, 5 + (idx * 2)].Value = capacity1;
                            wks.Cells[sheetStart, 5 + (idx * 2) + 1].Value = capacity2;
                            idx += 1;
                        }

                        wks.Cells[sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), sheetStart);

                        sheetStart += 1;

                        lisLoading.Add(sheetStart.ToString());
                        lisLoadingCty.Add(sheetStart.ToString());
                        wks.Cells[sheetStart, 4].Value = "Load.";
                        idx = 0;
                        DataTable dtLoadCPU = this.SafeGetDt(dt1, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}' and FactoryID = '{factoryID}'");
                        DataTable dtFactoryAdd = this.SafeGetDt(dt5, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}' and FactoryID = '{factoryID}'");
                        DataTable dtFactoryDiff = this.SafeGetDt(dt5, $"CountryID2 = '{countryID}' And MDivisionID2 = '{mdivisionID}' and FactoryID2 = '{factoryID}'");
                        for (int mon = intMonth; mon < intMonth + 6; mon++)
                        {
                            DataRow[] rows = dtLoadCPU.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon)));
                            DataRow[] rowsSB2add1 = dtFactoryAdd.Select($"Month = '{this.GetCurrMonth(intYear, mon) + "1"}'");
                            DataRow[] rowsSB2add2 = dtFactoryAdd.Select($"Month = '{this.GetCurrMonth(intYear, mon) + "2"}'");
                            DataRow[] rowsSB2diff1 = dtFactoryDiff.Select($"Month = '{this.GetCurrMonth(intYear, mon) + "1"}'");
                            DataRow[] rowsSB2diff2 = dtFactoryDiff.Select($"Month = '{this.GetCurrMonth(intYear, mon) + "2"}'");
                            decimal capacity1 = (rows.Length > 0) ? rows.Sum(rr => MyUtility.Convert.GetDecimal(rr["Capacity1"])) : 0;
                            decimal capacity2 = (rows.Length > 0) ? rows.Sum(rr => MyUtility.Convert.GetDecimal(rr["Capacity2"])) : 0;
                            decimal? vSB2add1 = (rowsSB2add1.Length > 0) ? rowsSB2add1.Sum(rr => MyUtility.Convert.GetDecimal(rr["OrderCapacity"])) : 0;
                            decimal? vSB2add2 = (rowsSB2add2.Length > 0) ? rowsSB2add2.Sum(rr => MyUtility.Convert.GetDecimal(rr["OrderCapacity"])) : 0;
                            decimal? vSB2diff1 = (rowsSB2diff1.Length > 0) ? rowsSB2diff1.Sum(rr => MyUtility.Convert.GetDecimal(rr["OrderCapacity"])) : 0;
                            decimal? vSB2diff2 = (rowsSB2diff2.Length > 0) ? rowsSB2diff2.Sum(rr => MyUtility.Convert.GetDecimal(rr["OrderCapacity"])) : 0;
                            int colIdx1 = 5 + (idx * 2);
                            int colIdx2 = 5 + (idx * 2) + 1;

                            string sb2Str1 = string.Empty;
                            string sb2Str2 = string.Empty;

                            if (vSB2add1 > 0)
                            {
                                sb2Str1 += $" + {vSB2add1}";
                            }

                            if (vSB2diff1 > 0)
                            {
                                sb2Str1 += $" - {vSB2diff1}";
                            }

                            if (sb2Str1 != string.Empty)
                            {
                                wks.Cells[sheetStart, colIdx1].Interior.Color = Color.Yellow;
                            }

                            if (vSB2add2 > 0)
                            {
                                sb2Str2 += $" + {vSB2add2}";
                            }

                            if (vSB2diff2 > 0)
                            {
                                sb2Str2 += $" - {vSB2diff2}";
                            }

                            if (sb2Str2 != string.Empty)
                            {
                                wks.Cells[sheetStart, colIdx2].Interior.Color = Color.Yellow;
                            }

                            wks.Cells[sheetStart, colIdx1].Value = $"= {capacity1}{sb2Str1}";
                            wks.Cells[sheetStart, colIdx2].Value = $"= {capacity2}{sb2Str2}";
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

                        this.DrawBottomLine(wks, sheetStart, 4, 3, 17);

                        sheetStart += 1;
                    }

                    // Shortage
                    int shortageStart = sheetStart;
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Shortage.", mdivisionID);
                    idx = 0;
                    DataTable dtShortage = this.SafeGetDt(dt1, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    for (int mon = intMonth; mon < intMonth + 6; mon++)
                    {
                        DataRow[] rows = dtShortage.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon)));
                        decimal orderShortage1 = 0;
                        decimal orderShortage2 = 0;
                        if (rows.Length > 0)
                        {
                            for (int tmpRow = 0; tmpRow < rows.Length; tmpRow++)
                            {
                                orderShortage1 += MyUtility.Convert.GetDecimal(rows[tmpRow]["OrderShortage1"]);
                                orderShortage2 += MyUtility.Convert.GetDecimal(rows[tmpRow]["OrderShortage2"]);
                            }
                        }

                        wks.Cells[sheetStart, 5 + (idx * 2)].Value = orderShortage1;
                        wks.Cells[sheetStart, 5 + (idx * 2) + 1].Value = orderShortage2;
                        idx += 1;
                    }

                    wks.Cells[sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), sheetStart);

                    lisShortage.Add(sheetStart.ToString());

                    sheetStart += 1;

                    // Total Capa.
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Capa.", mdivisionID);
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

                    // Total Load.
                    mdvTotalIdx = sheetStart;
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Load.", mdivisionID);
                    string totalLoad = "=";
                    for (int i = 0; i < lisLoading.Count; i++)
                    {
                        totalLoad += string.Format("+{{0}}{0}", lisLoading[i]);
                    }

                    totalLoad += string.Format("-{{0}}{0}", shortageStart);

                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format(totalLoad, MyExcelPrg.GetExcelColumnName(i));
                        wks.Cells[sheetStart, i] = str;
                    }

                    sheetStart += 1;

                    // 6 ForecastCapacity
                    lisPercent.Add(sheetStart.ToString());
                    idx = 0;
                    DataTable dtForecastCapacityByMDV = this.SafeGetDt(dt2, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}' " + filterZoneMdivisionAdd);
                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Total FC Shared", mdivisionID);
                    for (int mon = intMonth; mon < intMonth + 6; mon++)
                    {
                        DataRow[] rows = dtForecastCapacityByMDV.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon)));
                        decimal capacity1 = 0;
                        decimal capacity2 = 0;
                        if (rows.Length > 0)
                        {
                            for (int tmpRow = 0; tmpRow < rows.Length; tmpRow++)
                            {
                                capacity1 += MyUtility.Convert.GetDecimal(rows[tmpRow]["Capacity1"]);
                                capacity2 += MyUtility.Convert.GetDecimal(rows[tmpRow]["Capacity2"]);
                            }
                        }

                        wks.Cells[sheetStart, 5 + (idx * 2)].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2)), sheetStart - 1, capacity1);
                        wks.Cells[sheetStart, 5 + (idx * 2) + 1].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2) + 1), sheetStart - 1, capacity2);
                        idx += 1;
                    }

                    var sumforcapaMDV = dtForecastCapacityByMDV.Compute("SUM(Capacity1)+SUM(Capacity2)", string.Empty);
                    wks.Cells[sheetStart, 17] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(17), sheetStart - 1, (sumforcapaMDV == DBNull.Value) ? 0 : sumforcapaMDV);

                    sheetStart += 1;

                    // Total Vari.
                    this.SetFormulaToRow(wks, reportType, sheetStart, string.Format("{0} Total Vari.", mdivisionID), string.Format("={0}{1} - {0}{2}", "{0}", sheetStart - 2, sheetStart - 3), EnuDrawColor.Normal);

                    sheetStart += 1;

                    // Total Fill Rate
                    lisPercent.Add(sheetStart.ToString());
                    this.SetFormulaToRow(wks, reportType, sheetStart, string.Format("{0} Total Fill Rate", mdivisionID), string.Format("=IF({0}{2}>0,{0}{1} / {0}{2},0)", "{0}", sheetStart - 3, sheetStart - 4), EnuDrawColor.Normal);

                    sheetStart += 1;

                    // Output()
                    DataTable dtOutputMDV = this.SafeGetDt(dt3, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    DataTable dtOutputMDVAdd = this.SafeGetDt(dt5, $"CountryID = '{countryID}' And MDivisionID = '{mdivisionID}'" + filterZoneMdivisionAdd);
                    DataTable dtOutputMDVDiff = this.SafeGetDt(dt5, $"CountryID2 = '{countryID}' And MDivisionID2 = '{mdivisionID}'" + filterZoneMdivisionDiff);
                    string maxSewOutPut = dtOutputMDV.Compute("MAX(SewingYYMM)", string.Empty).ToString();
                    maxSewOutPut = maxSewOutPut.Length > 0 ? maxSewOutPut.Substring(5, maxSewOutPut.Length - 5) : string.Empty;
                    wks.Cells[sheetStart, 3].Value = $"{mdivisionID} Output ({maxSewOutPut})";
                    string zoneFilter = zone == string.Empty ? string.Empty : $" And Zone = '{zone}'";

                    idx = 0;
                    for (int mon = intMonth; mon < intMonth + 6; mon++)
                    {
                        DataRow[] rows1 = dtOutputMDV.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon) + "1") + zoneFilter);
                        DataRow[] rows2 = dtOutputMDV.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon) + "2") + zoneFilter);
                        DataRow[] rowsSB2add1 = dtOutputMDVAdd.Select(string.Format("Month = '{1}'", mdivisionID, this.GetCurrMonth(intYear, mon) + "1"));
                        DataRow[] rowsSB2add2 = dtOutputMDVAdd.Select(string.Format("Month = '{1}'", mdivisionID, this.GetCurrMonth(intYear, mon) + "2"));
                        DataRow[] rowsSB2diff1 = dtOutputMDVDiff.Select(string.Format("Month = '{1}'", mdivisionID, this.GetCurrMonth(intYear, mon) + "1"));
                        DataRow[] rowsSB2diff2 = dtOutputMDVDiff.Select(string.Format("Month = '{1}'", mdivisionID, this.GetCurrMonth(intYear, mon) + "2"));
                        decimal? capacity1 = (rows1.Length > 0) ? rows1.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                        decimal? capacity2 = (rows2.Length > 0) ? rows2.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                        decimal? vSB2add1 = (rowsSB2add1.Length > 0) ? rowsSB2add1.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                        decimal? vSB2add2 = (rowsSB2add2.Length > 0) ? rowsSB2add2.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                        decimal? vSB2diff1 = (rowsSB2diff1.Length > 0) ? rowsSB2diff1.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                        decimal? vSB2diff2 = (rowsSB2diff2.Length > 0) ? rowsSB2diff2.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                        int colIdx1 = 5 + (idx * 2);
                        int colIdx2 = 5 + (idx * 2) + 1;

                        string sb2Str1 = string.Empty;
                        string sb2Str2 = string.Empty;

                        if (vSB2add1 > 0)
                        {
                            sb2Str1 += $" + {vSB2add1}";
                        }

                        if (vSB2diff1 > 0)
                        {
                            sb2Str1 += $" - {vSB2diff1}";
                        }

                        if (sb2Str1 != string.Empty)
                        {
                            wks.Cells[sheetStart, colIdx1].Interior.Color = Color.Yellow;
                        }

                        if (vSB2add2 > 0)
                        {
                            sb2Str2 += $" + {vSB2add2}";
                        }

                        if (vSB2diff2 > 0)
                        {
                            sb2Str2 += $" - {vSB2diff2}";
                        }

                        if (sb2Str2 != string.Empty)
                        {
                            wks.Cells[sheetStart, colIdx2].Interior.Color = Color.Yellow;
                        }

                        wks.Cells[sheetStart, colIdx1].Value = $"= {capacity1}{sb2Str1}";
                        wks.Cells[sheetStart, colIdx2].Value = $"= {capacity2}{sb2Str2}";
                        idx += 1;
                    }

                    decimal capacity1Total = 0;
                    if (dtOutputMDV.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtOutputMDV.Rows)
                        {
                            capacity1Total += MyUtility.Convert.GetDecimal(row["SewCapacity"]);
                        }
                    }

                    wks.Cells[sheetStart, 17].Value = capacity1Total;

                    sheetStart += 1;

                    // MDV Output  Rate
                    lisPercent.Add(sheetStart.ToString());

                    wks.Cells[sheetStart, 3].Value = string.Format("{0} Output  Rate", mdivisionID);
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format("=IF({0}{1} > 0, {0}{2} / {0}{1},0)", MyExcelPrg.GetExcelColumnName(i), mdvTotalIdx, sheetStart - 1);
                        wks.Cells[sheetStart, i] = str;
                    }

                    this.DrawBottomLine(wks, sheetStart, 4, 2, 17);

                    sheetStart += 1;
                }

                // Country Total Capa.
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Capa.", countryID);
                string totalCapaCty = "={0}";
                totalCapaCty += string.Join("+{0}", lisCapaCty);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format(totalCapaCty, MyExcelPrg.GetExcelColumnName(i));
                    wks.Cells[sheetStart, i] = str;
                }

                sheetStart += 1;

                // Country Total Load.
                mdvTotalIdx = sheetStart;
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total Load.", countryID);
                string totalLoadCty = "={0}";
                totalLoadCty += string.Join("+{0}", lisLoadingCty);
                totalLoadCty += "-{0}";
                totalLoadCty += string.Join("-{0}", lisShortage);

                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format(totalLoadCty, MyExcelPrg.GetExcelColumnName(i));
                    wks.Cells[sheetStart, i] = str;
                }

                sheetStart += 1;

                // Country FC Shared
                lisPercent.Add(sheetStart.ToString());
                idx = 0;
                DataTable dtForecastCapacity = this.SafeGetDt(dt2, $"CountryID = '{countryID}'" + filterZoneMdivisionAdd);
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Total FC Shared", countryID);
                for (int mon = intMonth; mon < intMonth + 6; mon++)
                {
                    var capa1 = dtForecastCapacity.Compute("SUM(Capacity1)", string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon)));
                    var capa2 = dtForecastCapacity.Compute("SUM(Capacity2)", string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon)));
                    decimal capacity1 = capa1 == DBNull.Value ? 0 : Convert.ToDecimal(capa1);
                    decimal capacity2 = capa2 == DBNull.Value ? 0 : Convert.ToDecimal(capa2);
                    wks.Cells[sheetStart, 5 + (idx * 2)].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2)), sheetStart - 1, capacity1);
                    wks.Cells[sheetStart, 5 + (idx * 2) + 1].Value = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(5 + (idx * 2) + 1), sheetStart - 1, capacity2);
                    idx += 1;
                }

                var sumforcapaMDVCty = dtForecastCapacity.Compute("SUM(Capacity1)+SUM(Capacity2)", string.Empty);
                wks.Cells[sheetStart, 17] = string.Format("=IF({0}{1}>0,{2}/{0}{1},0)", MyExcelPrg.GetExcelColumnName(17), sheetStart - 1, (sumforcapaMDVCty == DBNull.Value) ? 0 : sumforcapaMDVCty);

                sheetStart += 1;

                // Country Total Vari.
                this.SetFormulaToRow(wks, reportType, sheetStart, string.Format("{0} Total Vari.", countryID), string.Format("={0}{1} - {0}{2}", "{0}", sheetStart - 2, sheetStart - 3), EnuDrawColor.Normal);

                sheetStart += 1;

                // Country Total Fill Rate
                lisPercent.Add(sheetStart.ToString());
                this.SetFormulaToRow(wks, reportType, sheetStart, string.Format("{0} Total Fill Rate", countryID), string.Format("=IF({0}{2}>0,{0}{1} / {0}{2},0)", "{0}", sheetStart - 3, sheetStart - 4), EnuDrawColor.Normal);
                sheetStart += 1;

                // CountryID Output()
                DataTable dtOutputCty = this.SafeGetDt(dt3, $"CountryID = '{countryID}'" + filterZoneMdivisionAdd);
                DataTable dtOutputCtyAdd = this.SafeGetDt(dt5, $"CountryID = '{countryID}'" + filterZoneMdivisionAdd);
                DataTable dtOutputCtyDiff = this.SafeGetDt(dt5, $"CountryID2 = '{countryID}'" + filterZoneMdivisionDiff);
                string maxSewOutPutCty = dtOutputCty.Compute("MAX(SewingYYMM)", string.Empty).ToString();
                maxSewOutPutCty = maxSewOutPutCty.Length > 0 ? maxSewOutPutCty.Substring(5, maxSewOutPutCty.Length - 5) : string.Empty;
                string zoneFilterCty = zone == string.Empty ? string.Empty : $" And Zone = '{zone}'";

                wks.Cells[sheetStart, 3].Value = $"{countryID} Output ({maxSewOutPutCty})";

                idx = 0;
                for (int mon = intMonth; mon < intMonth + 6; mon++)
                {
                    DataRow[] rows1 = dtOutputCty.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon) + "1") + zoneFilterCty);
                    DataRow[] rows2 = dtOutputCty.Select(string.Format("MONTH = '{0}'", this.GetCurrMonth(intYear, mon) + "2") + zoneFilterCty);
                    DataRow[] rowsSB2add1 = dtOutputCtyAdd.Select(string.Format("Month = '{0}'", this.GetCurrMonth(intYear, mon) + "1"));
                    DataRow[] rowsSB2add2 = dtOutputCtyAdd.Select(string.Format("Month = '{0}'", this.GetCurrMonth(intYear, mon) + "2"));
                    DataRow[] rowsSB2diff1 = dtOutputCtyDiff.Select(string.Format("Month = '{0}'", this.GetCurrMonth(intYear, mon) + "1"));
                    DataRow[] rowsSB2diff2 = dtOutputCtyDiff.Select(string.Format("Month = '{0}'", this.GetCurrMonth(intYear, mon) + "2"));
                    decimal? capacity1 = (rows1.Length > 0) ? rows1.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                    decimal? capacity2 = (rows2.Length > 0) ? rows2.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                    decimal? vSB2add1 = (rowsSB2add1.Length > 0) ? rowsSB2add1.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                    decimal? vSB2add2 = (rowsSB2add2.Length > 0) ? rowsSB2add2.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                    decimal? vSB2diff1 = (rowsSB2diff1.Length > 0) ? rowsSB2diff1.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                    decimal? vSB2diff2 = (rowsSB2diff2.Length > 0) ? rowsSB2diff2.Sum(rr => MyUtility.Convert.GetDecimal(rr["SewCapacity"])) : 0;
                    int colIdx1 = 5 + (idx * 2);
                    int colIdx2 = 5 + (idx * 2) + 1;

                    string sb2Str1 = string.Empty;
                    string sb2Str2 = string.Empty;

                    if (vSB2add1 > 0)
                    {
                        sb2Str1 += $" + {vSB2add1}";
                    }

                    if (vSB2diff1 > 0)
                    {
                        sb2Str1 += $" - {vSB2diff1}";
                    }

                    if (sb2Str1 != string.Empty)
                    {
                        wks.Cells[sheetStart, colIdx1].Interior.Color = Color.Yellow;
                    }

                    if (vSB2add2 > 0)
                    {
                        sb2Str2 += $" + {vSB2add2}";
                    }

                    if (vSB2diff2 > 0)
                    {
                        sb2Str2 += $" - {vSB2diff2}";
                    }

                    if (sb2Str2 != string.Empty)
                    {
                        wks.Cells[sheetStart, colIdx2].Interior.Color = Color.Yellow;
                    }

                    wks.Cells[sheetStart, colIdx1].Value = $"= {capacity1}{sb2Str1}";
                    wks.Cells[sheetStart, colIdx2].Value = $"= {capacity2}{sb2Str2}";
                    idx += 1;
                }

                decimal capacity1CtyTotal = 0;
                if (dtOutputCty.Rows.Count > 0)
                {
                    foreach (DataRow row in dtOutputCty.Rows)
                    {
                        capacity1CtyTotal += MyUtility.Convert.GetDecimal(row["SewCapacity"]);
                    }
                }

                wks.Cells[sheetStart, 17].Value = capacity1CtyTotal;

                sheetStart += 1;

                // CountryID Output  Rate
                lisPercent.Add(sheetStart.ToString());
                wks.Cells[sheetStart, 3].Value = string.Format("{0} Output  Rate", countryID);
                for (int i = 5; i <= 17; i++)
                {
                    string str = string.Format("=IF({0}{1} > 0, {0}{2} / {0}{1},0)", MyExcelPrg.GetExcelColumnName(i), mdvTotalIdx, sheetStart - 1);
                    wks.Cells[sheetStart, i] = str;
                }

                this.DrawBottomLine(wks, sheetStart, 5, 1, 17);

                sheetStart += 1;
            }

            sheetStart -= 1;

            // 欄位以直線區隔
            string lastCell = MyExcelPrg.GetExcelColumnName(17) + sheetStart.ToString();
            Microsoft.Office.Interop.Excel.Range rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(1) + artworkStart.ToString(), lastCell);
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;

            // 數值格式
            rg = wks.get_Range(MyExcelPrg.GetExcelColumnName(5) + artworkStart, lastCell);
            rg.Cells.NumberFormat = "##,###,##0";

            foreach (string idx in lisPercent)
            {
                string rgStr = string.Format("{0}{2}:{1}{2}", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(17), idx);
                rg = wks.get_Range(rgStr);
                rg.Cells.NumberFormat = "###,##0.00%";
            }

            // sxrc.dicDatas.Add("##Year", intYear);
            // sxrc.dicDatas.Add("##Month", intMonth);
            // sxrc.dicDatas.Add("##ArtworkType", ArtWorkType == "CPU" ? ArtWorkType : ArtWorkType + " TMS/Min");
            // string dTypeStr = isSCIDelivery ? "Sci Delivery" : "Buyer Delivery";
            // sxrc.dicDatas.Add("##DateType", dTypeStr);

            // sxrc.Save();

            // wks = null;
            // sxrc = null;
            GC.Collect();

            return Ict.Result.True;
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

        private void SetTableToRow(Microsoft.Office.Interop.Excel.Worksheet wks, int intYear, int sheetStart, string cell1Str, DataTable dt, string valueCol = "Capacity")
        {
            wks.Cells[sheetStart, 1].Value = cell1Str;
            for (int mon = 1; mon < 13; mon++)
            {
                DataRow[] rows = dt.Select(string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                decimal v = 0;
                if (rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        v += MyUtility.Convert.GetDecimal(rows[i][valueCol]);
                    }
                }

                wks.Cells[sheetStart, mon + 1].Value = v;
            }

            wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);
        }

        private void SetTableToRow_SubconInType(Microsoft.Office.Interop.Excel.Worksheet wks, int intYear, int sheetStart, string cell1Str, DataTable dt1, DataTable dt2, DataTable dt3, string capa1, string capa2)
        {
            wks.Cells[sheetStart, 1].Value = cell1Str;
            for (int mon = 1; mon < 13; mon++)
            {
                DataRow[] rows = dt1.Select(string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                DataRow[] rowsSB2add = dt2.Select(string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                DataRow[] rowsSB2diff = dt3.Select(string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                decimal? capa = (rows.Length > 0) ? rows.Sum(rr => MyUtility.Convert.GetDecimal(rr[capa1])) : 0;
                decimal? vSB2add = (rowsSB2add.Length > 0) ? rowsSB2add.Sum(rr => MyUtility.Convert.GetDecimal(rr[capa2])) : 0;
                decimal? vSB2diff = (rowsSB2diff.Length > 0) ? rowsSB2diff.Sum(rr => MyUtility.Convert.GetDecimal(rr[capa2])) : 0;

                string sb2Str = string.Empty;

                if (vSB2add > 0)
                {
                    sb2Str += $" + {vSB2add}";
                }

                if (vSB2diff > 0)
                {
                    sb2Str += $" - {vSB2diff}";
                }

                if (sb2Str != string.Empty)
                {
                    wks.Cells[sheetStart, mon + 1].Interior.Color = Color.Yellow;
                }

                wks.Cells[sheetStart, mon + 1].Value = $"= {capa}{sb2Str}";
            }

            wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);
        }

        private void SetTableToRow_sum(Microsoft.Office.Interop.Excel.Worksheet wks, int intYear, int sheetStart, string cell1Str, string cell2Str, string cell3Str, string cell4Str, DataTable dt, string valueCol = "Capacity")
        {
            wks.Cells[sheetStart, 1].Value = cell1Str;
            wks.Cells[sheetStart, 2].Value = cell2Str;
            wks.Cells[sheetStart, 3].Value = cell3Str;
            wks.Cells[sheetStart, 4].Value = cell4Str;
            for (int mon = 1; mon < 13; mon++)
            {
                DataRow[] rows = dt.Select(string.Format("Month = '{0}'", intYear.ToString() + mon.ToString("00")));
                decimal v = 0;
                if (rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        v += MyUtility.Convert.GetDecimal(rows[i][valueCol]);
                    }
                }

                wks.Cells[sheetStart, mon + 4].Value = v;
            }

            wks.Cells[sheetStart, 17] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(5), MyExcelPrg.GetExcelColumnName(16), sheetStart);
        }

        private void SetFormulaToRow(Microsoft.Office.Interop.Excel.Worksheet wks, int reportType, int sheetStart, string cell1Str, string formula, EnuDrawColor color = EnuDrawColor.None)
        {
            if (reportType == 1)
            {
                wks.Cells[sheetStart, 1].Value = cell1Str;
                for (int i = 2; i <= 14; i++)
                {
                    string str = string.Format(formula, MyExcelPrg.GetExcelColumnName(i));
                    wks.Cells[sheetStart, i] = str;
                    if (color == EnuDrawColor.Normal)
                    {
                        decimal value = 0;
                        decimal.TryParse(wks.Cells[sheetStart, i].Value.ToString(), out value);
                        if (value >= 0)
                        {
                            wks.Cells[sheetStart, i].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FF99CC"));
                        }
                        else if (value < 0)
                        {
                            wks.Cells[sheetStart, i].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#CCFFCC"));
                        }
                    }
                }
            }
            else
            {
                wks.Cells[sheetStart, 3].Value = cell1Str;

                // Total Fill Rate的顏色依Total Vari上色
                if (cell1Str.Contains("Total Fill Rate"))
                {
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format(formula, MyExcelPrg.GetExcelColumnName(i));
                        wks.Cells[sheetStart, i] = str;
                        if (color == EnuDrawColor.Normal)
                        {
                            wks.Cells[sheetStart, i].Interior.Color = wks.Cells[sheetStart - 1, i].Interior.Color;
                        }
                    }
                }
                else
                {
                    for (int i = 5; i <= 17; i++)
                    {
                        string str = string.Format(formula, MyExcelPrg.GetExcelColumnName(i));
                        wks.Cells[sheetStart, i] = str;
                        if (color == EnuDrawColor.Normal)
                        {
                            decimal value = 0;
                            decimal.TryParse(wks.Cells[sheetStart, i].Value.ToString(), out value);
                            if (value >= 0)
                            {
                                wks.Cells[sheetStart, i].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FF99CC"));
                            }
                            else if (value < 0)
                            {
                                wks.Cells[sheetStart, i].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#CCFFCC"));
                            }
                        }
                    }
                }
            }

            // wks.Cells[sheetStart, 14] = string.Format("=SUM({0}{2}:{1}{2})", MyExcelPrg.GetExcelColumnName(2), MyExcelPrg.GetExcelColumnName(13), sheetStart);
        }

        private DataTable SafeGetDt(DataTable dt, string filterStr)
        {
            DataRow[] rows = dt.Select(filterStr);
            DataTable dtOutput = (rows.Length > 0) ? rows.CopyToDataTable() : dt.Clone();
            return dtOutput;
        }

        #endregion

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
            this.lbMonth.Visible = !this.rdMonth.Checked;
            this.numMonth.Visible = !this.rdMonth.Checked;

            if (this.rdMonth.Checked)
            {
                this.numMonth.Value = 1;
            }
        }

        private void RadioSemimonthlyReport_CheckedChanged(object sender, EventArgs e)
        {
            this.lbMonth.Visible = this.rdHalfMonth.Checked;
            this.numMonth.Visible = this.rdHalfMonth.Checked;

            if (this.rdHalfMonth.Checked)
            {
                this.numMonth.Value = DateTime.Today.Month;
            }
        }

        private void RadioProductionStatus_CheckedChanged(object sender, EventArgs e)
        {
            this.lbMonth.Visible = this.radioProductionStatus.Checked;
            this.numMonth.Visible = this.radioProductionStatus.Checked;
            this.labelDate.Visible = !this.radioProductionStatus.Checked;
            this.cbDateType.Visible = !this.radioProductionStatus.Checked;
            this.labelReport.Visible = !this.radioProductionStatus.Checked;
            this.cbReportType.Visible = !this.radioProductionStatus.Checked;
            this.labelSource.Visible = !this.radioProductionStatus.Checked;
            this.chkOrder.Visible = !this.radioProductionStatus.Checked;
            this.chkForecast.Visible = !this.radioProductionStatus.Checked;
            this.chkFty.Visible = !this.radioProductionStatus.Checked;
            this.LbAdditional.Visible = !this.radioProductionStatus.Checked;
            this.chkHideFoundry.Visible = !this.radioProductionStatus.Checked;
            this.chkByCPU.Visible = !this.radioProductionStatus.Checked;
            this.chkByBrand.Visible = !this.radioProductionStatus.Checked;
        }

        private enum EnuDrawColor
        {
            /// <summary>
            /// 無配色
            /// </summary>
            None,

            /// <summary>
            /// 正數紫色、負數綠色
            /// </summary>
            Normal,
        }

        private void TxtZone_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select distinct zone from Factory WITH (NOLOCK) where isSCI=1 and junk=0 ", "8", this.Text, false, ",");
            item.Size = new Size(300, 250);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.TxtZone.Text = item.GetSelectedString();
        }

        private void TxtZone_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string strZone = this.TxtZone.Text;
            if (!string.IsNullOrWhiteSpace(strZone) && strZone != this.TxtZone.OldValue)
            {
                if (MyUtility.Check.Seek(string.Format("select distinct zone from Factory WITH (NOLOCK) where Zone = '{0}'", strZone)) == false)
                {
                    this.TxtZone.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Zone : {0} > not found!!!", strZone));
                    return;
                }
            }
        }
    }
}
