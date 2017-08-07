-- =============================================
-- Author:		JIMMY
-- Create date: 2017/05/06
-- Description:	<SP#>
-- =============================================
Create FUNCTION [dbo].[getstdq]
(
	@tSP varchar(20)
)
RETURNS Table
AS
Return
(
WITH cte (DD,num, INLINE,OrderID,sewinglineid,FactoryID,WorkDay,StandardOutput,ComboType,Hours,WDAY) AS (  
      SELECT DATEDIFF(DAY,A.Inline,A.Offline)+1 AS DD
                    , 1 as num
                    , convert(date,A.Inline) inline 
                    ,A.OrderID
                    ,A.sewinglineid
                    ,a.FactoryID
					,a.WorkDay,
					a.StandardOutput
                    ,a.ComboType,W.Hours
					,IIF(W.Hours > 0,1,0) AS WDAY
	  FROM SewingSchedule A WITH (NOLOCK)
	  LEFT JOIN WorkHour W ON A.FactoryID=W.FactoryID AND A.SewingLineID=W.SewingLineID
	  WHERE ORDERID=@tSP and w.Date between convert(date,A.Inline) and convert(date,A.Offline)
      UNION ALL  
      SELECT DD,num + 1, DATEADD(DAY,1,INLINE) ,ORDERID,sewinglineid,FactoryID,WorkDay,StandardOutput,ComboType,Hours,WDAY
      FROM cte a where num < DD  AND ORDERID=@tSP
    )  
	,
	temp as( 
	select SUM(Hours)h,sum(WDAY)wday,WorkDay,StandardOutput
	from cte
	group by WorkDay,StandardOutput
	)

	select iif(WorkDay=0,0,avgh.avghours * StandardOutput) stdq
	from temp
	outer apply(select avghours=iif(wday=0,0,h/wday) from temp)avgh
)