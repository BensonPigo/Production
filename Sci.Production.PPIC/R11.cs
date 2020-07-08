using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using Sci.Utility.Excel;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R11 : Sci.Win.Tems.PrintForm
    {
        private string F;
        private string M;
        private string Gap;
        private string dateRangeReady1;
        private string dateRangeReady2;
        private string Brand;
        private DataTable dtFty;
        private DataTable[] dtList;
        private List<string> listSQLFilter;
        private string boolHoliday;
        private StringBuilder sbFtycode;
        private Color BackRed = Color.FromArgb(255, 155, 155);
        private Color FontRed = Color.FromArgb(255, 0, 0);
        private Color BackGreen = Color.FromArgb(167, 255, 190);
        private Color FontGreen = Color.FromArgb(0, 126, 15);
        private Color BackGray = Color.FromArgb(217, 217, 217);

        /// <inheritdoc/>
        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.numDateGap.Text = "2";
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.F = this.txtfactory.Text;
            this.M = this.txtMdivision.Text;
            this.Gap = this.numDateGap.Text;
            this.Brand = this.txtbrand.Text;
            this.dateRangeReady1 = MyUtility.Check.Empty(this.dateRangeReadyDate.Value1) ? string.Empty : ((DateTime)this.dateRangeReadyDate.Value1).ToString("yyyy/MM/dd");
            this.dateRangeReady2 = MyUtility.Check.Empty(this.dateRangeReadyDate.Value2) ? string.Empty : ((DateTime)this.dateRangeReadyDate.Value2).ToString("yyyy/MM/dd");

            this.boolHoliday = string.Empty;
            this.listSQLFilter = new List<string>();
            #region Sql where Filter

            if (!this.F.Empty())
            {
                this.listSQLFilter.Add($"and o.FtyGroup = '{this.F}'");
            }

            if (!this.M.Empty())
            {
                this.listSQLFilter.Add($"and o.MDivisionID = '{this.M}'");
            }

            if (!this.Brand.Empty())
            {
                this.listSQLFilter.Add($"and o.BrandID = '{this.Brand}'");
            }

            switch (this.chkHoliday.Checked)
            {
                case true:
                    this.boolHoliday = @"and not exists(select 1 from Holiday where HolidayDate = Dates and FactoryID=o.FtyGroup)	";

                    break;
                case false:
                    this.boolHoliday = string.Empty;
                    break;
            }
            #endregion

            #region Summery

            this.sbFtycode = new StringBuilder();
            string sqlFty = $@"

select distinct o.FtyGroup from sewingschedule s
inner join orders o on s.orderid=o.id
where 1=1
and convert(date,s.offline) between '{this.dateRangeReady1}' and '{this.dateRangeReady2}'
{this.listSQLFilter.JoinToString($"{Environment.NewLine} ")}";

            DBProxy.Current.Select(string.Empty, sqlFty, out this.dtFty);
            if (this.dtFty != null)
            {
                for (int i = 0; i < this.dtFty.Rows.Count; i++)
                {
                    this.sbFtycode.Append($@"[{this.dtFty.Rows[i]["FtyGroup"].ToString().TrimEnd()}],");
                }
            }

            if (this.sbFtycode.Length == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            #endregion
            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region Sql Command
            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            string sqlCmd = $@"
DECLARE  @t TABLE
(
    StartDate DATE,
	EndDate DATE
);

declare @ReadyDate1 date = '{this.dateRangeReady1}';
declare @ReadyDate2 date = '{this.dateRangeReady2}';

INSERT @t
        ( StartDate, EndDate )
VALUES  ( DATEADD(DAY,-15, @ReadyDate1), -- StartDate
          DATEADD(DAY, +15 ,@ReadyDate2)  -- EndDate
          );

-- 遞迴取得自製月曆
;WITH CTE (Dates,EndDate) AS
(
	SELECT StartDate AS Dates,EndDate AS EndDate
	FROM @t
	UNION ALL --注意這邊使用 UNION ALL
	SELECT DATEADD(DAY,1,Dates),EndDate
	FROM CTE 
	WHERE DATEADD(DAY,1,Dates) < EndDate --判斷是否目前遞迴月份小於結束日期	
)
SELECT CTE.Dates
into #Calendar
FROM CTE


/* Temple Table 1 */
SELECT distinct
[ReadyDate] = convert(date,o.SewOffLine)--NormalCalendar.Dates
,Calendar.Dates		
,o.FtyGroup
into #CalendarData	
FROM Orders o 
left join Order_QtyShip os on o.ID=os.Id	
cross apply(
	select Dates,[rows] = ROW_NUMBER() over(order by dates desc) from #Calendar			
	where  DATEPART(WEEKDAY, Dates) <> 1 --排除星期日
	and Dates < CONVERT(date, o.SewOffLine)			
    {this.boolHoliday}
)Calendar	
where 1=1
{whereIncludeCancelOrder}
and o.Category ='B'
and CONVERT(date, o.SewOffLine) between @ReadyDate1 and @ReadyDate2 -- 將offline跟ReadyDate綁再一起,方便取得RedayDate
and Calendar.rows = {MyUtility.Convert.GetInt(this.Gap)} -- GAP 
and Dates < CONVERT(date, o.SewOffLine)	



/* 先將資料邏輯1組出temple table 後在使用判斷處理資料*/
SELECT 
		[ReadyDate] = AllDate.ReadyDate
		,[M] = o.MDivisionID
		,[Factory] = o.FtyGroup		
		,[BuyerDelivery] = os.BuyerDelivery
		,[SPNO] = o.ID
		,[Category] = 
		case	when o.Category ='B' then 'Bulk'
				when o.Category ='S' then 'Sample'
				when o.Category ='M' then 'Material'
				when o.Category ='O' then 'Other'
				when o.Category ='G' then 'Garment'
				when o.Category ='T' then 'SMLT'
		end
		,[Cancelled] = o.Junk
		,[Dest] = (select alias from Country where  o.Dest=id)
		,[Style] = o.StyleID
		,[OrderType] = o.OrderTypeID
		,[PoNo] = o.CustPONo
		,[Brand] = o.BrandID
		,[Qty] = os.Qty
		,[SewingOutputQty] = isnull(SewingOutput.Qty,0)
		,[InLine] = o.SewInLine
		,[OffLine] = o.SewOffLine
		,[FirstSewnDate] = SewDate.FirstDate
		,[LastSewnDate] = SewDate.LastDate
		,[%]= iif(pdm.TotalCTN=0,0, ( isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)
		,[TtlCTN] = pdm.TotalCTN
		,[FtyCTN] = pdm.TotalCTN - isnull(Receive.ClogCTN,0)
		,[ClogCTN] = isnull(Receive.ClogCTN,0)
		,[cLogRecDate] = Receive.ClogRcvDate
		,[FinalInspDate]=cfa.FinalInsDate
		,cfa.Result
		,[CfaName] = cfa.CfaName
		,[SewingLine] = SewingLine.Line
		,[ShipMode] = os.ShipmodeID
		,[Remark]= IIF(iif(pdm.TotalCTN=0,0, (isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)=100,'Pass Ready date','Failed Ready Date')
		into #tmp1
		FROM Orders o
		left join Order_QtyShip os on o.ID=os.Id
		inner join #CalendarData AllDate on AllDate.Dates= o.SewOffLine
        and AllDate.FtyGroup = o.FtyGroup	
		outer apply(
			select top 1 [CfaName] =pass1.ID+'-'+pass1.Name
			,case when cfa.Result='P' then 'Pass'
			when cfa.Result='F' then 'Fail'
			else '' end as Result
			,cfa.EditDate as FinalInsDate
			from cfa
			left join Pass1 on cfa.CFA=pass1.ID
			where convert(date,cfa.EditDate) <=o.SewOffLine
			and datepart(HH,cfa.EditDate)<=17 -- 下午5點)
			and cfa.OrderID=o.ID
			order by cfa.EditDate desc
		)cfa
		outer apply(
			select sum(so2.QAQty) Qty 
			from SewingOutput_Detail so2
			left join SewingOutput so1 on so2.ID=so1.ID
			where so2.OrderId=o.ID
			and so1.OutputDate <= o.SewOffLine
		)SewingOutput
		outer apply(
			select MIN(OutputDate) FirstDate,MAX(OutputDate) LastDate
			from SewingOutput a
			left join SewingOutput_Detail b on a.id=b.id
			where b.OrderId =o.ID
		)SewDate
		outer apply( 
			select [TotalCTN] = Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) 
			from PackingList_Detail pd WITH (NOLOCK) 
			LEFT JOIN PackingList p on pd.ID = p.ID 
			where  pd.OrderID = os.ID 
			and pd.OrderShipmodeSeq = os.Seq
		)  pdm	
		outer apply(
			select Line = stuff((
				select concat(',',s.SewLine)
				from (
					select distinct SewLine
					from orders oo
					where oo.ID=o.ID
					) s
				for xml path ('')
				),1,1,'')
		)SewingLine		
		outer apply (
			select [ClogCTN] = Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end)
			,[ClogRcvDate] = Max (c.AddDate) 
			from PackingList_Detail pd WITH (NOLOCK) 
			LEFT JOIN PackingList p on pd.ID = p.ID 
			left join ClogReceive c on c.PackingListID=pd.ID
				and c.ReceiveDate=pd.ReceiveDate 
				and c.CTNStartNo=pd.CTNStartNo 
				and c.OrderID=pd.OrderID
			where  pd.OrderID = os.ID 
			and pd.OrderShipmodeSeq = os.Seq
			and CONVERT(date,pd.ReceiveDate) <= AllDate.ReadyDate
			and datepart(HH, c.AddDate) <= 17 -- 下午5點)
		) Receive			
		where 1=1
		and o.Category ='B' and o.junk !=1 
		and o.SewOffLine=AllDate.Dates
        {this.listSQLFilter.JoinToString($"{Environment.NewLine} ")}

/*將資料邏輯第二層 拉出來處理再下區間判斷*/

SELECT 
		[ReadyDate] = AllDate.Dates
		,[M] = o.MDivisionID
		,[Factory] = o.FtyGroup
		,[BuyerDelivery] = os.BuyerDelivery
		,[SPNO] = o.ID
		,[Category] = 
		case	when o.Category ='B' then 'Bulk'
				when o.Category ='S' then 'Sample'
				when o.Category ='M' then 'Material'
				when o.Category ='O' then 'Other'
				when o.Category ='G' then 'Garment'
				when o.Category ='T' then 'SMLT'
		end
		,[Cancelled] = o.Junk
		,[Dest] = (select alias from Country where  o.Dest=id)
		,[Style] = o.StyleID
		,[OrderType] = o.OrderTypeID
		,[PoNo] = o.CustPONo
		,[Brand] = o.BrandID
		,[Qty] = os.Qty
		,[SewingOutputQty] = isnull(SewingOutput.Qty,0)
		,[InLine] = o.SewInLine
		,[OffLine] = o.SewOffLine
		,[FirstSewnDate] = SewDate.FirstDate
		,[LastSewnDate] = SewDate.LastDate
		,[%]= iif(pdm.TotalCTN=0,0, ( isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)
		,[TtlCTN] = pdm.TotalCTN
		,[FtyCTN] = pdm.TotalCTN - isnull(Receive.ClogCTN,0)
		,[ClogCTN] = isnull(Receive.ClogCTN,0)
		,[cLogRecDate] = Receive.ClogRcvDate
		,[FinalInspDate]=cfa.FinalInsDate
		,cfa.Result
		,[CfaName] = cfa.CfaName
		,[SewingLine] = SewingLine.Line
		,[ShipMode] = os.ShipmodeID
		,[Remark]= IIF(iif(pdm.TotalCTN=0,0, (isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)=100,'Pass Ready date','Failed Ready Date')
		into #tmp2
		FROM Orders o 
		left join Order_QtyShip os on o.ID=os.Id
		inner join #Calendar AllDate on AllDate.Dates= os.BuyerDelivery
		outer apply(
			select top 1 [CfaName] =pass1.ID+'-'+pass1.Name
			,case when cfa.Result='P' then 'Pass'
			when cfa.Result='F' then 'Fail'
			else '' end as Result
			,cfa.EditDate as FinalInsDate
			from cfa
			left join Pass1 on cfa.CFA=pass1.ID
			where convert(date,cfa.EditDate) <= o.SewOffLine
			and datepart(HH,cfa.EditDate)<=17 -- 下午5點)
			and cfa.OrderID= o.ID
			order by cfa.EditDate desc
		)cfa
		outer apply(
			select sum(so2.QAQty) Qty 
			from SewingOutput_Detail so2
			left join SewingOutput so1 on so2.ID=so1.ID
			where so2.OrderId=o.ID
			and so1.OutputDate <= o.SewOffLine
		)SewingOutput
		outer apply(
			select MIN(OutputDate) FirstDate,MAX(OutputDate) LastDate
			from SewingOutput a
			left join SewingOutput_Detail b on a.id=b.id
			where b.OrderId =o.ID
		)SewDate
		outer apply( 
			select Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) TotalCTN
			from PackingList_Detail pd WITH (NOLOCK) 
			LEFT JOIN PackingList p on pd.ID = p.ID 
			where  pd.OrderID = os.ID 
			and pd.OrderShipmodeSeq = os.Seq
		)  pdm	
		outer apply(
			select Line = stuff((
				select concat(',',s.SewLine)
				from (
					select distinct SewLine
					from Orders oo
					where oo.ID=o.ID
					) s
				for xml path ('')
				),1,1,'')
		)SewingLine
		outer apply (
			select [ClogCTN] = Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end)
			,[ClogRcvDate] = Max (c.AddDate) 
			from PackingList_Detail pd WITH (NOLOCK) 
			LEFT JOIN PackingList p on pd.ID = p.ID 
			left join ClogReceive c on c.PackingListID=pd.ID
				and c.ReceiveDate=pd.ReceiveDate and c.CTNStartNo=pd.CTNStartNo 
				and c.OrderID=pd.OrderID
			where  pd.OrderID = os.ID 
			and pd.OrderShipmodeSeq = os.Seq
			and CONVERT(date,pd.ReceiveDate) <= AllDate.Dates
			and datepart(HH, c.AddDate) <= 17 -- 下午5點)
		) Receive			
		where 1=1
		and o.Category ='B' and o.junk !=1 
		and AllDate.Dates between @ReadyDate1 and @ReadyDate2		
        {this.listSQLFilter.JoinToString($"{Environment.NewLine} ")}
        {this.boolHoliday}

Select * 
into #tmp
from 
(
/*搜尋SewingOffline + GAP = ReadyDate
並且BuyerDelivery > ReadyDate。*/

	select * from #tmp1
	where BuyerDelivery >= ReadyDate	

union all

/*搜尋Ready date 等於Order_QtyShip.BuyerDelivery(今天要交貨的資料)，
且並且BuyerDelivery - sewing offline要小於GAP並包含自己 。*/

    select t.* from #tmp2 t
	outer apply (
		select Dates
		from #Calendar			
		where  DATEPART(WEEKDAY, Dates) <> 1 --排除星期日	
	)Sunday
	where ReadyDate >= OffLine
	and Sunday.dates = ReadyDate
	and DATEDIFF(day,convert(date,offline),BuyerDelivery) <= {MyUtility.Convert.GetInt(this.Gap)} -- GAP 
    and not exists(select 1 from #tmp1 where  BuyerDelivery >= ReadyDate and t.SPNO=SPNO )

union all

/*Offline > BuyerDelivery= Ready Date*/

	select * from #tmp2 t
	where OffLine > BuyerDelivery

union all


/*月曆>=BuyerDelivery ,並且日期由小大到排序
如果明天之後都是假日,都要將這些假日資料列入該天計算		 */		 	
	
select [ReadyDate] = AllDate.ReadyDate		
		,[M] = o.MDivisionID
		,[Factory] = o.FtyGroup
		,[BuyerDelivery] = oq.BuyerDelivery
		,[SPNO] = oq.id
		,[Category] = 
		case	when o.Category ='B' then 'Bulk'
				when o.Category ='S' then 'Sample'
				when o.Category ='M' then 'Material'
				when o.Category ='O' then 'Other'
				when o.Category ='G' then 'Garment'
				when o.Category ='T' then 'SMLT'
		end
		,[Cancelled] = o.Junk
		,[Dest] = (select alias from Country where  o.Dest=id)
		,[Style] = o.StyleID
		,[OrderType] = o.OrderTypeID
		,[PoNo] = o.CustPONo
		,[Brand] = o.BrandID
		,[Qty] = oq.Qty
		,[SewingOutputQty] = isnull(SewingOutput.Qty,0)
		,[InLine] = o.SewInLine
		,[OffLine] = o.SewOffLine
		,[FirstSewnDate] = SewDate.FirstDate
		,[LastSewnDate] = SewDate.LastDate
		,[%]= iif(pdm.TotalCTN=0,0, ( isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)
		,[TtlCTN] = pdm.TotalCTN
		,[FtyCTN] = pdm.TotalCTN - isnull(Receive.ClogCTN,0)
		,[ClogCTN] = isnull(Receive.ClogCTN,0)
		,[cLogRecDate] = Receive.ClogRcvDate
		,[FinalInspDate]=cfa.FinalInsDate
		,cfa.Result
		,[CfaName] = cfa.CfaName
		,[SewingLine] = SewingLine.Line
		,[ShipMode] = oq.ShipmodeID
		,[Remark]= IIF(iif(pdm.TotalCTN=0,0, (isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)=100,'Pass Ready date','Failed Ready Date')
from Orders o
inner join Order_QtyShip oq on o.id=oq.Id
cross apply(	
SELECT  distinct Ready.ReadyDate,isHoliday.HolidayDates,notHoliday.WorkDates
	FROM #Calendar c
	cross apply(
		SELECT [ReadyDate] = Dates	
		FROM #Calendar	a				
		where 1=1
		and	a.Dates between @ReadyDate1 and @ReadyDate2
        -- 排除假日
		and not exists(select dates
			from #Calendar			
			where (DATEPART(WEEKDAY, Dates) = 1  --只能是星期天
			or exists(select 1 from Holiday where HolidayDate = Dates and FactoryID=o.FtyGroup)) -- 只能是假日)		
			and Dates = a.Dates
		)
	)Ready
cross apply(
		select HolidayDates = Dates,[rows] = ROW_NUMBER() over(order by dates asc) 
		from #Calendar			
		where (DATEPART(WEEKDAY, Dates) = 1  --只能是星期天
		or exists(select 1 from Holiday where HolidayDate = Dates and FactoryID=o.FtyGroup)) -- 只能是假日)		
		and dates = oq.BuyerDelivery
		and Dates > Ready.ReadyDate
	)isHoliday
	cross apply(
		select MIN(Dates) as WorkDates
		from #Calendar			
		where  DATEPART(WEEKDAY, Dates) != 1  --排除星期天
		and not exists(select 1 from Holiday where HolidayDate = Dates and FactoryID=o.FtyGroup) -- 排除假日)
		and Dates > Ready.ReadyDate
	)notHoliday
	where isHoliday.rows=1 --取得最靠近ReadyDate 的假日
	)AllDate
outer apply(
		select top 1 [CfaName] =pass1.ID+'-'+pass1.Name
		,case when cfa.Result='P' then 'Pass'
		when cfa.Result='F' then 'Fail'
		else '' end as Result
		,cfa.EditDate as FinalInsDate
		from cfa
		left join Pass1 on cfa.CFA=pass1.ID
		where convert(date,cfa.EditDate) <= o.SewOffLine
		and datepart(HH,cfa.EditDate)<=17 -- 下午5點)
		and cfa.OrderID=oq.Id
	)cfa
	outer apply(
		select sum(so2.QAQty) Qty 
		from SewingOutput_Detail so2
		left join SewingOutput so1 on so2.ID=so1.ID
		where so2.OrderId=oq.id
	)SewingOutput
	outer apply(
		select MIN(OutputDate) FirstDate,MAX(OutputDate) LastDate
		from SewingOutput a
		left join SewingOutput_Detail b on a.id=b.id
		where b.OrderId =oq.Id
	)SewDate		
	outer apply( 
		select Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) TotalCTN
		from PackingList_Detail pd WITH (NOLOCK) 
		LEFT JOIN PackingList p on pd.ID = p.ID 
		where  pd.OrderID = oq.ID 
		and pd.OrderShipmodeSeq = oq.Seq
	)  pdm		
	outer apply(
		select Line = stuff((
			select concat(',',SewLine)
			from (
				select distinct SewLine
				from orders oo
				where oo.ID=o.ID
				) s
			for xml path ('')
			),1,1,'')
	)SewingLine
	outer apply (
		select [ClogCTN] = Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end)
		,[ClogRcvDate] = Max (c.AddDate) 
		from PackingList_Detail pd WITH (NOLOCK) 
		LEFT JOIN PackingList p on pd.ID = p.ID 
		left join ClogReceive c on c.PackingListID=pd.ID
			and c.ReceiveDate=pd.ReceiveDate and c.CTNStartNo=pd.CTNStartNo 
			and c.OrderID=pd.OrderID
		where  pd.OrderID = oq.ID 
		and pd.OrderShipmodeSeq = oq.Seq
		and CONVERT(date,pd.ReceiveDate) <= AllDate.ReadyDate
		and datepart(HH, c.AddDate) <= 17 -- 下午5點)
	) Receive	
where O.Category ='B' and o.junk !=1
and oq.BuyerDelivery = AllDate.HolidayDates  
and oq.BuyerDelivery < AllDate.WorkDates 
and not exists(select 1 from #tmp1 where  BuyerDelivery >= ReadyDate and o.ID=SPNO)
{this.listSQLFilter.JoinToString($"{Environment.NewLine} ")}

) a 

select * from #tmp order by ReadyDate,m,Factory

select ReadyDate,m,Fty,[Failed Ready Date],[Pass Ready date],[Grand Total],[Rating]= convert(varchar,Rating)+'%' 
from (
SELECT [M] = M
,[Fty] = Factory
,[Failed Ready Date] =fail.cnt
,[Pass Ready date] = pass.cnt
,[Grand Total] =count(*),
[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
,ReadyDate
FROM #tmp t
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Failed Ready Date' and Factory=t.Factory and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)fail
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Pass Ready Date' and Factory=t.Factory and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)pass
where category='Bulk' and Cancelled=0
group by ReadyDate,M,Factory ,fail.cnt,pass.cnt

union all

SELECT [M] = M
,[Fty] = 'Total'
,[Failed Ready Date] =fail.cnt
,[Pass Ready date] = pass.cnt
,[Grand Total] =count(*),
[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
,ReadyDate
FROM #tmp t
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Failed Ready Date' and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)fail
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Pass Ready Date' and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)pass
where category='Bulk' and Cancelled=0
group by ReadyDate,M ,fail.cnt,pass.cnt

union all

SELECT [M] = 'x'
,[Fty] = 'Grand Total'
,[Failed Ready Date] =fail.cnt
,[Pass Ready date] = pass.cnt
,[Grand Total] =count(*),
[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
,ReadyDate
FROM #tmp t
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Failed Ready Date' and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)fail
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Pass Ready Date' and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)pass
where category='Bulk' and Cancelled=0
group by ReadyDate,fail.cnt,pass.cnt
) a
order by ReadyDate,M,Fty

select * from 
(
	SELECT Factory, ReadyDate
	,[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
	FROM #tmp t
	outer apply(
		select count(*) cnt
		FROM #tmp
		where Remark='Failed Ready Date' and t.ReadyDate=ReadyDate
		and category='Bulk' and Cancelled=0 and Factory = t.Factory
	)fail
	outer apply(
		select count(*) cnt
		FROM #tmp
		where Remark='Pass Ready Date' and t.ReadyDate=ReadyDate
		and category='Bulk' and Cancelled=0 and Factory = t.Factory
	)pass
	where category='Bulk' and Cancelled=0
	group by Factory,ReadyDate,fail.cnt,pass.cnt
) as a
pivot 
(
	sum(rating) for factory in ({this.sbFtycode.ToString().Substring(0, this.sbFtycode.ToString().Length - 1)})
) AS PT

drop table #Calendar,#CalendarData,#tmp,#tmp1,#tmp2
";
            #endregion

            #region SQL get DataTable
            DBProxy.Current.DefaultTimeout = 2700;
            DualResult result;
            result = DBProxy.Current.Select(null, sqlCmd, out this.dtList);
            if (!result)
            {
                return result;
            }

            DBProxy.Current.DefaultTimeout = 300;
            #endregion
            return new Ict.DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            #region check resultDT
            if (MyUtility.Check.Empty(this.dtList[0]) || this.dtList[0].Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not Found.");
                return false;
            }
            #endregion
            this.SetCount(this.dtList[0].Rows.Count);
            string xltPath = System.IO.Path.Combine(Sci.Env.Cfg.XltPathDir + "\\PPIC_R11.xltx");
            sxrc sxr = new sxrc(xltPath, keepApp: true);
            sxr.BoOpenFile = true;
            sxrc.XltRptTable xrtSummery1 = new sxrc.XltRptTable(this.dtList[1]);
            xrtSummery1.ShowHeader = false;
            sxr.DicDatas.Add("##Summery", xrtSummery1);

            sxrc.XltRptTable xrtDetail = new sxrc.XltRptTable(this.dtList[0]);
            xrtDetail.ShowHeader = false;
            sxr.DicDatas.Add("##detail", xrtDetail);

            Microsoft.Office.Interop.Excel.Workbook wkb = sxr.ExcelApp.ActiveWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet wkcolor = wkb.Sheets[1];
            Microsoft.Office.Interop.Excel.Worksheet wkcolor2 = wkb.Sheets[2];

            #region Save Excel
            string excelFile = string.Empty;
            excelFile = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R11");
            sxr.Save(excelFile);
            #endregion

            #region Summery by ReadyDate 變色及調整excel格式邊框

            // 變色
            int cntSummery = this.dtList[2].Rows.Count;

            DateTime date1 = (DateTime)this.dateRangeReadyDate.Value1;
            TimeSpan ts1 = new TimeSpan(((DateTime)this.dateRangeReadyDate.Value1).Ticks);
            TimeSpan ts2 = new TimeSpan(((DateTime)this.dateRangeReadyDate.Value2).Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            int range = ts.Days + 1;

            // Summery header
            wkcolor.Cells[1, 9].Value = "Date";
            wkcolor.Cells[1, 9].Interior.Color = this.BackGray;
            wkcolor.Cells[1, 9].Font.Bold = true;
            for (int d = 0; d < range; d++)
            {
                wkcolor.Cells[2 + d, 9].Value = date1.AddDays(d);
                wkcolor.Cells[2 + d, 9].Font.Bold = true;
            }

            for (int f = 0; f < this.dtFty.Rows.Count; f++)
            {
                wkcolor.Cells[1, 10 + f].Value = this.dtFty.Rows[f]["FtyGroup"];
                wkcolor.Cells[1, 10 + f].Interior.Color = this.BackGray;
                wkcolor.Cells[1, 10 + f].Font.Bold = true;
                string fty = this.dtFty.Rows[f]["FtyGroup"].ToString();

                int cnt = 0;
                for (int i = 0; i < range; i++)
                {
                    if ((this.dtList[2].Rows.Count > cnt) && (MyUtility.Convert.GetDate((DateTime)this.dtList[2].Rows[cnt]["ReadyDate"]) == MyUtility.Convert.GetDate(date1.AddDays(i))))
                    {
                        wkcolor.Cells[2 + i, 10 + f].Value = MyUtility.Check.Empty(this.dtList[2].Rows[cnt][fty]) ? "0%" : this.dtList[2].Rows[cnt][fty] + "%";
                        if (MyUtility.Convert.GetDecimal(this.dtList[2].Rows[cnt][fty]) > 90)
                        {
                            wkcolor.Cells[2 + i, 10 + f].Interior.Color = this.BackGreen;
                            wkcolor.Cells[2 + i, 10 + f].Font.Color = this.FontGreen;
                        }
                        else
                        {
                            wkcolor.Cells[2 + i, 10 + f].Interior.Color = this.BackRed;
                            wkcolor.Cells[2 + i, 10 + f].Font.Color = this.FontRed;
                        }

                        cnt++;
                    }
                    else
                    {
                        string rowformat1 = MyExcelPrg.GetExcelColumnName(9);
                        string rowformat2 = MyExcelPrg.GetExcelColumnName(9 + this.dtFty.Rows.Count);
                        string rowformat3 = MyExcelPrg.GetExcelColumnName(10);
                        wkcolor.Range[$"{rowformat1}{2 + i}", $"{rowformat2}{2 + i}"].Interior.Color = Color.Red;
                        wkcolor.Range[$"{rowformat3}{2 + i}", $"{rowformat2}{2 + i}"].Merge();
                    }
                }
            }

            // 畫框線
            Microsoft.Office.Interop.Excel.Range rg1;
            rg1 = wkcolor.Range["I1", $"{MyExcelPrg.GetExcelColumnName(9 + this.dtFty.Rows.Count)}{range + 1}"];
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;

            #endregion

            #region Summery by M,Fty

            for (int i = 0; i < this.dtList[1].Rows.Count; i++)
            {
                if (this.dtList[1].Rows[i]["Fty"].ToString() == "Total")
                {
                    wkcolor.Range[$"C{i + 2}", $"F{i + 2}"].Font.Bold = true;
                }

                if (this.dtList[1].Rows[i]["Fty"].ToString() == "Grand Total")
                {
                    wkcolor.Cells[i + 2, 2].Value = string.Empty;
                    wkcolor.Range[$"C{i + 2}", $"G{i + 2}"].Font.Bold = true;
                }
            }

            // 畫框線
            Microsoft.Office.Interop.Excel.Range rg2;
            rg2 = wkcolor.Range["A1", $"G{this.dtList[1].Rows.Count + 1}"];
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;
            #endregion

            // 最適欄寬
            wkcolor.Cells.EntireColumn.AutoFit();
            wkcolor2.Cells.EntireColumn.AutoFit();
            sxr.FinishSave();

            return true;
        }
    }
}
