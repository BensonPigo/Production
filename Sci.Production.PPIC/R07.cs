using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R07
    /// </summary>
    public partial class R07 : Win.Tems.PrintForm
    {
        private int _year;
        private int _month;
        private string _mDivision;
        private string _factory;
        private string subProcess;
        private DataTable _printData;
        private DateTime _startDate;

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);

            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(this.comboM.Text);
            this.comboM.Enabled = false;

            this.numericUpDownYear.Value = MyUtility.Convert.GetInt(DateTime.Today.ToString("yyyy"));
            this.numericUpDownMonth.Value = MyUtility.Convert.GetInt(DateTime.Today.ToString("MM"));

            DBProxy.Current.Select(null, "select '' as ID union all select ID from ArtworkType WITH (NOLOCK) where ReportDropdown = 1", out DataTable subprocess);
            MyUtility.Tool.SetupCombox(this.comboSubProcess, 1, subprocess);
            this.comboSubProcess.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.numericUpDownYear.Value))
            {
                MyUtility.Msg.WarningBox("Year can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.numericUpDownMonth.Value))
            {
                MyUtility.Msg.WarningBox("Month can't empty!!");
                return false;
            }

            this._year = (int)this.numericUpDownYear.Value;
            this._month = (int)this.numericUpDownMonth.Value;

            // ISP20240496
            if (this._year < 2023)
            {
                this._year = 2023;
                this._month = 1;
            }

            this._mDivision = this.comboM.Text;
            this._factory = this.comboFactory.Text;
            this._startDate = Convert.ToDateTime(string.Format("{0}/{1}/1", MyUtility.Convert.GetString(this._year), MyUtility.Convert.GetString(this._month)));
            this.subProcess = this.comboSubProcess.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.subProcess))
            {
                where += $@"
and exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = '{this.subProcess}' AND (st.Qty>0 or st.TMS>0 and st.Price>0) )";
            }

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            sqlCmd.Append(string.Format(
                @"
DECLARE @sewinginlineOri  DATETIME ='{0}'
DECLARE @sewingoffline DATETIME ='{1}'
DECLARE @MDivisionID Nvarchar(8)= '{2}'
DECLARE @FactoryID Nvarchar(8)= '{3}'

Declare @sewinginline DATETIME = dateadd(MONTH, -3, @sewinginlineOri) 
--整個月 table
;WITH cte AS (
    SELECT [date] = @sewinginline
    UNION ALL
    SELECT [date] + 1 FROM cte WHERE ([date] < DATEADD(DAY,-1,@sewingoffline))
)
SELECT [date] = cast([date] as date) 
into #daterange 
FROM cte
option (maxrecursion 0)
--WorkHour table
select FactoryID,SewingLineID,Date,Hours,Holiday
into #workhourtmp
from WorkHour w
where (FactoryID = @FactoryID or @FactoryID ='')
and Date between @sewinginline and DATEADD(DAY,1,@sewingoffline)
--order by Date
--準備一整個月workhour的資料判斷Holiday
select distinct d.date,w.FactoryID,w.SewingLineID
into #tmpd
from #daterange d,#workhourtmp w
--
select distinct d.FactoryID,d.SewingLineID,d.date,Holiday = iif(w.Holiday is null or w.Holiday = 1 or w.Hours = 0,1,0)
into #Holiday
from #tmpd d
left join #workhourtmp w on d.date = w.Date and d.FactoryID = w.FactoryID and d.SewingLineID = w.SewingLineID
order by FactoryID,SewingLineID,date
--先將符合條件的Sewing schedule撈出來
select
	 s.FactoryID
	,s.SewingLineID
	,o.StyleID
	,Inline = cast(s.Inline as date)
	,Offline = cast(s.Offline as date)
	,OrderTypeID = isnull(o.OrderTypeID,'')
	,o.SciDelivery
	,o.BuyerDelivery
	,Category = isnull(o.Category,'')
	,[CdCodeID] = st.CDCodeNew
into #Sewtmp
from SewingSchedule s WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on s.OrderID = o.ID
left join Style st WITH (NOLOCK) on st.Ukey = o.StyleUkey
where (s.Inline between  @sewinginline and @sewingoffline 
    or s.Offline between @sewinginline and @sewingoffline
	or @sewinginline between s.Inline and s.Offline
	or @sewingoffline between s.Inline and s.Offline
)
 and s.MDivisionID = @MDivisionID --and (s.FactoryID = @FactoryID or @FactpryID = '') --and SewingLineID = '18'
{4}

select distinct
	d.FactoryID
	,d.SewingLineID
	,d.date
	,s.StyleID
	,IsLastMonth = iif(s.SciDelivery < @sewinginline,1,0)--紫 優先度1
	,IsNextMonth = iif(s.SciDelivery > DateAdd(DAY,-1,@sewingoffline),1,0)--綠 優先度2
	,IsBulk = iif(s.Category = 'B',1,0)--藍 優先度3
	,IsSMS = iif(s.OrderTypeID = 'SMS',1,0)--紅 優先度4
    ,IsSample = iif(s.Category = 'S',1,0)
	,s.BuyerDelivery
	,s.CdCodeID
into #Stmp
from #Sewtmp s 
left join #tmpd d on d.FactoryID = s.FactoryID and d.SewingLineID = s.SewingLineID and d.date between s.Inline and s.Offline
--order by h.FactoryID,h.SewingLineID,h.date,s.StyleID

--抓出各style連續作業天數，不含假日
select distinct FactoryID, SewingLineID, date, StyleID into #tmpLongDayCheck1 from #Stmp where FactoryID is not null

select *,
	   isBegin = iif(lag(date) over (order by factoryid, sewinglineid, styleID, date) = dateadd(day, -1, date), 0, 1),
	   isEnd =  iif(lead(date) over (order by factoryid, sewinglineid, styleID, date) = dateadd(day, 1, date), 0, 1)
into #tmpLongDayCheck2
from #tmpLongDayCheck1
order by factoryid, sewinglineid, styleID, date

select rownum, factoryid, sewinglineid, styleID, BeginDate = min(date), EndDate = max(date)
into #tmpLongDayCheck3
from (
select *, rownum = ROW_NUMBER() over (order by factoryid, sewinglineid, styleID, date) from #tmpLongDayCheck2 where isBegin = 1 
union all
select *, rownum = ROW_NUMBER() over (order by factoryid, sewinglineid, styleID, date) from #tmpLongDayCheck2 where isEnd = 1
) a 
group by rownum, factoryid, sewinglineid, styleID


select *, [WorkDays] = DATEDIFF(day, BeginDate, EndDate) + 1 - holiday.val
into #tmpLongDayCheck
from #tmpLongDayCheck3 t
outer apply(select val = count(*) from #Holiday where Holiday = 1 and FactoryID = t.FactoryID and SewingLineID = t.SewingLineID and date between BeginDate and EndDate) holiday

--

select distinct FactoryID,SewingLineID,date,StyleID = a.s
into #ConcatStyle
from #Stmp s
outer apply(
	select s =(
		select distinct concat(StyleID,'(',CdCodeID,')',';')
		from #Stmp s2
		where s2.FactoryID = s.FactoryID and s2.SewingLineID = s.SewingLineID and s2.date = s.date
		for xml path('')
	)
)a
where s.date >= @sewinginlineOri

--
select	h.FactoryID,h.SewingLineID,h.date,h.Holiday,IsLastMonth,IsNextMonth,IsBulk,IsSample,IsSMS,BuyerDelivery,[OriStyle] = s.StyleID,
		[IsRepeatStyle] = iif(DENSE_RANK() OVER (PARTITION BY s.FactoryID, s.SewingLineID, s.StyleID, h.Holiday ORDER BY s.date) > 10, 1, 0)
into #c
from #Holiday h
left join #Stmp s on s.FactoryID = h.FactoryID and s.SewingLineID = h.SewingLineID and s.date = h.date
inner join(select distinct FactoryID,SewingLineID from #Stmp) x on x.FactoryID = h.FactoryID and x.SewingLineID = h.SewingLineID--排掉沒有在SewingSchedule內的資料by FactoryID,SewingLineID
order by h.FactoryID,h.SewingLineID,h.date

------------------------------------------------------------------------------------------------
DECLARE cursor_sewingschedule CURSOR FOR
select c.FactoryID
		,c.SewingLineID
		,c.date
		,StyleID = isnull(iif(c.Holiday = 1,'Holiday', cs.StyleID),'')
		,[IsLastMonth] = max(IsLastMonth)
		,[IsNextMonth] = max(IsNextMonth)
		,[IsBulk] = max(IsBulk)
		,[IsSample] = max(IsSample)
		,IsSMS
        ,[BuyerDelivery] = min(BuyerDelivery)
		,IsRepeatStyle
		,c.OriStyle
from #c c left join #ConcatStyle cs on c.FactoryID = cs.FactoryID and c.SewingLineID = cs.SewingLineID and c.date = cs.date
where c.date >= @sewinginlineOri
group by c.FactoryID,c.SewingLineID,c.date,isnull(iif(c.Holiday = 1,'Holiday', cs.StyleID),''),IsSMS,IsRepeatStyle,c.OriStyle
order by c.FactoryID, c.SewingLineID, c.date ASC, IsSample DESC

--建立tmpe table存放最後要列印的資料
DECLARE @tempPintData TABLE (
   FactoryID VARCHAR(8),
   SewingLineID VARCHAR(5),
   StyleID VARCHAR(MAX),
   InLine DATE,
   OffLine DATE,
   IsBulk BIT,
   IsSample BIT,
   IsSMS BIT,
   IsLastMonth BIT,
   IsNextMonth BIT,
   MinBuyerDelivery DATE,
   IsFirst BIT default 0,
   IsRepeatStyle BIT,
   IsSimilarStyle BIT default 0,
   IsCrossStyle BIT default 0
)

--判斷相似款所使用暫存table
DECLARE @tempSimilarStyle TABLE (
   ScheduleDate date,
   MasterStyleID VARCHAR(15),
   ChildrenStyleID VARCHAR(15)
)
--
DECLARE @factory VARCHAR(8),
		@sewingline VARCHAR(5),
		@StyleID VARCHAR(200),
		@IsLastMonth int,
		@IsNextMonth int,
		@IsBulk int,
		@IsSample int,
		@IsSMS int,
		@BuyerDelivery DATE,
		@date DATE,
		@beforefactory VARCHAR(8) = '',
		@beforesewingline VARCHAR(5) = '',
		@beforeStyleID VARCHAR(200) = '',
		@beforeStyleIDExcludeHoliday VARCHAR(200) = '',
		@beforeIsLastMonth int,
		@beforeIsNextMonth int,
		@beforeIsBulk int,
		@beforeIsSMS int,
		@beforeBuyerDelivery DATE,
		@beforedate DATE,
		@IsFirst bit = 0,
		@beforeIsSample bit = 1,
		@IsRepeatStyle bit = 1,
		@beforeIsRepeatStyle bit = 1,
		@OriStyle varchar(15) = ''

OPEN cursor_sewingschedule
FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@date,@StyleID,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSample,@IsSMS,@BuyerDelivery,@IsRepeatStyle,@OriStyle
WHILE @@FETCH_STATUS = 0
BEGIN

	if(@sewingline <> @beforesewingline)
		delete @tempSimilarStyle
	
	--換款新增資料
	IF @factory <> @beforefactory or @sewingline <> @beforesewingline or @StyleID <> @beforeStyleID
	Begin
        if(@beforeStyleIDExcludeHoliday not like '%' + @StyleID + '%' and @sewingline = @beforesewingline)
            set @IsFirst = 1
        else
			set @IsFirst = 0

		INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine,OffLine,IsLastMonth ,IsNextMonth ,IsBulk, IsSample,IsSMS ,MinBuyerDelivery, IsRepeatStyle, IsFirst) 
		VALUES					 (@factory, @sewingline, @StyleID,@date,@date  ,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSample,@IsSMS,@BuyerDelivery, @IsRepeatStyle, @IsFirst);
	END
	--有含Sample獨立顯示 or 三個月內生產超過10天
    ELSE IF @IsFirst = 1 and @date <> @beforedate and @StyleID <> 'Holiday' and @IsSample <> 1
	Begin
        set @IsFirst = 0
		INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine,OffLine,IsLastMonth ,IsNextMonth ,IsBulk, IsSample,IsSMS ,MinBuyerDelivery, IsRepeatStyle) 
		VALUES					 (@factory, @sewingline, @StyleID,@date,@date  ,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSample,@IsSMS,@BuyerDelivery, @IsRepeatStyle);
	end
	else if (@IsSample <> @beforeIsSample or @beforeStyleID = 'Holiday'  or (@IsRepeatStyle <> @beforeIsRepeatStyle and @IsSample <> 1)) and @date <> @beforedate and @StyleID <> 'Holiday'
	begin
		INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine,OffLine,IsLastMonth ,IsNextMonth ,IsBulk, IsSample,IsSMS ,MinBuyerDelivery, IsRepeatStyle) 
		VALUES					 (@factory, @sewingline, @StyleID,@date,@date  ,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSample,@IsSMS,@BuyerDelivery, @IsRepeatStyle);
	end
	else
	--同款連續生產
	Begin
		update @tempPintData set
			 OffLine = @date
			,IsLastMonth = iif(IsLastMonth = 1, IsLastMonth, @IsLastMonth)
			,IsNextMonth = iif(IsNextMonth = 1, IsNextMonth, @IsNextMonth)
			,IsBulk = iif(IsBulk = 1, IsBulk, @IsBulk)
			,IsSMS = iif(IsSMS = 1, IsSMS, @IsSMS)
			,MinBuyerDelivery = iif(MinBuyerDelivery < @BuyerDelivery,MinBuyerDelivery,@BuyerDelivery)
		where FactoryID = @factory and SewingLineID = @sewingline and StyleID = @StyleID and OffLine = @beforedate
	END
	
	if	(select count(*) from SplitString(@StyleID,';') where data <> '') > 1 and 
		@StyleID <> 'Holiday'
	begin
		--只保留當天與前一天的SimilarStyle資料
		delete @tempSimilarStyle 
		where	ScheduleDate <> (select top 1 ScheduleDate from @tempSimilarStyle where ScheduleDate <> @date order by ScheduleDate desc) and
				ScheduleDate <> @date
		
		if exists(select 1 from @tempSimilarStyle where ChildrenStyleID = @OriStyle)
			update @tempPintData set IsSimilarStyle = 1, IsCrossStyle = 1 where FactoryID = @factory and SewingLineID = @sewingline and StyleID = @StyleID and OffLine = @date
		else
			update @tempPintData set IsCrossStyle = 1 where FactoryID = @factory and SewingLineID = @sewingline and StyleID = @StyleID and OffLine = @date

		if not exists(select 1 from @tempSimilarStyle where ScheduleDate = @date and MasterStyleID = @OriStyle)
		insert into @tempSimilarStyle(ScheduleDate, MasterStyleID, ChildrenStyleID)
			select @date, MasterStyleID, ChildrenStyleID
			from Style_SimilarStyle where MasterStyleID = @OriStyle
	end

	set @beforefactory = @factory
	set @beforesewingline = @sewingline
	set @beforeStyleID = @StyleID
	set @beforeIsLastMonth = @IsLastMonth
	set @beforeIsNextMonth = @IsNextMonth
	set @beforeIsBulk = @IsBulk
	set @beforeIsSMS = @IsSMS
	set @beforeIsSample = @IsSample
	set @beforeBuyerDelivery = @BuyerDelivery
	set @beforedate = @date
	set @beforeIsRepeatStyle = @IsRepeatStyle
	set @beforeStyleIDExcludeHoliday = iif(@StyleID = 'Holiday', @beforeStyleIDExcludeHoliday, @StyleID)
	FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@date,@StyleID,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSample,@IsSMS,@BuyerDelivery,@IsRepeatStyle,@OriStyle
END
CLOSE cursor_sewingschedule
DEALLOCATE cursor_sewingschedule

select  t.*,
        [WorkDays] = isnull(workDays.val, 0)
from @tempPintData t
outer apply (   select val = max(WorkDays) 
                from #tmpLongDayCheck tdc
                where   tdc.FactoryID = t.FactoryID and
                        tdc.SewingLineID = t.SewingLineID and
                        t.StyleID like '%' + tdc.StyleID + '%'  and
                        t.InLine between tdc.BeginDate and tdc.EndDate
            ) workDays
where t.StyleID<>''

drop table #daterange,#tmpd,#Holiday,#Sewtmp,#workhourtmp,#Stmp,#c,#ConcatStyle,#tmpLongDayCheck1,#tmpLongDayCheck2,#tmpLongDayCheck3,#tmpLongDayCheck",
                this._startDate.ToString("yyyy/MM/dd"),
                this._startDate.AddMonths(1).ToString("yyyy/MM/dd"),
                this._mDivision,
                this._factory,
                where));
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this._printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData.Rows.Count);

            if (this._printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\PPIC_R07_SewingScheduleGanttChart.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            excel.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 1;
            string writeFty = string.Empty;
            string line = string.Empty;
            int ftyCount = 0;
            int colCount = 1;
#if DEBUG
            excel.Visible = true;
#endif
            int monthDays = this._startDate.AddMonths(1).AddDays(-1).Subtract(this._startDate).Days + 1;

            foreach (DataRow dr in this._printData.Rows)
            {
                // 當有換工廠別時，要換Sheet
                if (writeFty != MyUtility.Convert.GetString(dr["FactoryID"]))
                {
                    if (ftyCount > 0)
                    {
                        // 將上一間工廠最後一條Line後面沒有Schedule的格子塗黑
                        if (colCount - 1 != monthDays)
                        {
                            for (int i = colCount; i <= monthDays; i++)
                            {
                                worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = Color.Black;
                            }
                        }

                        // 刪除多出來的資料行
                        for (int i = 1; i <= 2; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[intRowsStart + 1, Type.Missing];
                            rng.Select();
                            rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                            Marshal.ReleaseComObject(rng);
                        }
                    }

                    ftyCount++;
                    worksheet = excel.ActiveWorkbook.Worksheets[ftyCount];
                    worksheet.Select();
                    worksheet.Name = MyUtility.Convert.GetString(dr["FactoryID"]);
                    intRowsStart = 1;
                    writeFty = MyUtility.Convert.GetString(dr["FactoryID"]);
                    if (monthDays < 31)
                    {
                        for (int i = monthDays + 2; i <= 32; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Columns[monthDays + 2, Type.Missing];
                            rng.Select();
                            rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                            Marshal.ReleaseComObject(rng);
                        }
                    }
                }

                // 換Sewing Line時，要填入Line#
                if (line != MyUtility.Convert.GetString(dr["SewingLineID"]))
                {
                    if (intRowsStart > 1)
                    {
                        // 將後面沒有Schedule的格子塗黑
                        if (colCount - 1 != monthDays)
                        {
                            for (int i = colCount; i <= monthDays; i++)
                            {
                                worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = Color.Black;
                            }
                        }
                    }

                    intRowsStart++;
                    colCount = 1;

                    // 先插入一行
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(intRowsStart + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);

                    line = MyUtility.Convert.GetString(dr["SewingLineID"]);
                    worksheet.Cells[intRowsStart, 1] = line;
                }

                // 算上下線日的總天數，用來做合併儲存格
                DateTime sewingStartDate = Convert.ToDateTime(dr["Inline"]);
                DateTime sewingEndDate = !MyUtility.Check.Empty(dr["Offline"]) ? Convert.ToDateTime(dr["Offline"]) : this._startDate.AddMonths(1).AddDays(-1);
                int startCol = sewingStartDate.Subtract(this._startDate).Days + 2;
                int endCol = sewingEndDate.Subtract(this._startDate).Days + 2;
                int totalDays = sewingEndDate.Subtract(sewingStartDate).Days + 1;

                // 將中間沒有Schedule的格子塗黑
                if (colCount + 1 != startCol)
                {
                    for (int i = colCount + 1; i < startCol; i++)
                    {
                        worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = Color.Black;
                    }
                }

                // 算出Excel的Column的英文位置
                string excelStartColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(startCol);
                string excelEndColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(endCol);
                Microsoft.Office.Interop.Excel.Range selrng = worksheet.get_Range(string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng), Type.Missing).EntireRow;

                // 合併儲存格,文字置中
                worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Merge(Type.Missing);
                worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // 顏色顯示優先權：Holiday紅色背景 > SCI Delivery為當月以前的紫色粗體 > SCI Delivery當月為以後的綠色粗體 > Bulk款式藍色粗體 > SMS款式紅色粗體 > 非以上四種情況的黑色字體
                #region 填入內容值
                if (MyUtility.Convert.GetString(dr["StyleID"]) == "Holiday")
                {
                    // 設置儲存格的背景色
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = Color.Red;
                }
                else if (MyUtility.Convert.GetString(dr["IsLastMonth"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Purple;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("yyyy/MM/dd"));
                }
                else if (MyUtility.Convert.GetString(dr["IsNextMonth"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Green;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("yyyy/MM/dd"));
                }
                else if (MyUtility.Convert.GetString(dr["IsBulk"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Blue;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("yyyy/MM/dd"));
                }
                else if (MyUtility.Convert.GetString(dr["IsSMS"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Red;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("yyyy/MM/dd"));
                }
                else
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Bold = false;
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Black;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("yyyy/MM/dd"));
                }

                if (MyUtility.Convert.GetString(dr["StyleID"]) != "Holiday")
                {
                    if (MyUtility.Convert.GetString(dr["IsSample"]).ToUpper() == "TRUE")
                    {
                        // 設置儲存格的背景色
                        worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = Color.Yellow;
                    }
                    else if (MyUtility.Convert.GetString(dr["IsCrossStyle"]).ToUpper() == "TRUE")
                    {
                        if (MyUtility.Convert.GetString(dr["IsSimilarStyle"]).ToUpper() == "TRUE")
                        {
                            // 設置儲存格的背景色
                            worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = Color.LightGray;
                        }
                        else
                        {
                            // 設置儲存格的背景色
                            worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = Color.White;
                        }
                    }
                    else if (MyUtility.Convert.GetString(dr["IsRepeatStyle"]).ToUpper() == "TRUE")
                    {
                        // 設置儲存格的背景色
                        worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = Color.FromArgb(175, 203, 154);
                    }
                    else if (MyUtility.Convert.GetString(dr["IsFirst"]).ToUpper() == "TRUE")
                    {
                        // 設置儲存格的背景色
                        worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = Color.White;
                    }
                    else
                    {
                        // 設置儲存格的背景色
                        worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = Color.FromArgb(134, 187, 249);
                    }
                }

                #endregion
                colCount = colCount + (startCol - colCount - 1) + totalDays;
                Marshal.ReleaseComObject(selrng);
            }

            if (colCount - 1 != monthDays)
            {
                for (int i = colCount; i <= monthDays; i++)
                {
                    worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = Color.Black;
                }
            }

            // 刪除多出來的資料行
            for (int i = 1; i <= 2; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[intRowsStart + 1, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            // 刪除多的Sheet
            for (int i = ftyCount + 1; i <= 10; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[ftyCount + 1];
                worksheet.Delete();
            }

            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Select();

            this.HideWaitMessage();
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R07_SewingScheduleGanttChart");
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
