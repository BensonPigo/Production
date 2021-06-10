CREATE FUNCTION [dbo].[getSPRatebyAPSNo]
(
	@APSNo varchar(20)=706082
)
RETURNS
@table TABLE 
(
	APSNo int,
	OrderID varchar(13),
	AlloQty int,
	Rate float
)
AS
Begin

--以下計算SP的rate
declare @APSBase Table(
	[OrderID] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL,
	[Inline] [datetime] NULL,
	[AlloQty] [int] NULL
)

Insert Into @APSBase
select
	s.OrderID,
	s.ComboType,
	s.Inline,
	s.AlloQty
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
where 1 = 1 
and s.APSNo = @APSNo
and s.APSno <> 0

-- 找出 < Suncon > 不為空的 OrderID -- < Suncon > not empty rule (ISP20210759)
declare @SunconNotEmpty TABLE([OrderID] [varchar](13) NOT NULL)
insert into @SunconNotEmpty
select distinct ot.ID
from Order_TmsCost ot WITH (NOLOCK) 
inner join ArtworkType at WITH (NOLOCK) on at.ID = ot.ArtworkTypeID and at.ID in('PRINTING','PRINTING PPU')
left join LocalSupp ls on ls.id = ot.LocalSuppID
where exists(select 1 from @APSBase where orderid = ot.id)
and ot.ArtworkTypeID = 'PRINTING'
and IIF(ot.InhouseOSP = 'O', ls.abb, ot.LocalSuppID) <> ''

-- 相同SP有多個ComboType時,只取Inline最早的ComboType -- same SP has more one ComboType,get first Inline data
declare @SP_ComboType Table(
	[OrderID] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL
)
insert into @SP_ComboType
select OrderID,ComboType
from(
	select rn =  row_number() over(partition by a.OrderID order by a.Inline),a.OrderID,a.ComboType
	from @APSBase a
	where exists(select 1 from @SunconNotEmpty where OrderID = a.OrderID)
)x
where rn = 1

declare @Printing_AlloQty int
declare @TTL_AlloQty int

declare @SP_Qty Table(
	APSNo varchar(20),
	[OrderID] [varchar](13) NOT NULL,
	AlloQty int NULL
)
insert into @SP_Qty
select @APSNo,s.OrderID, sum(AlloQty)
from SewingSchedule s
inner join @SP_ComboType c on c.OrderID = s.OrderID and c.ComboType = s.ComboType
where APSNo = @APSNo
group by s.OrderID

insert into @table
select s.APSNo,s.OrderID,AlloQty,SPRate = cast( SUM(AlloQty) over(partition by Apsno,OrderID) as float) / SUM(AlloQty) over(partition by Apsno) 
from @SP_Qty s


Return;

End