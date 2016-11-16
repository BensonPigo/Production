-- =============================================
-- Author:		
-- Create date: <Create Date,,>
-- Description:	新增,更新 AIR,FIR資料
-- =============================================
CREATE PROCEDURE [dbo].[insert_Air_Fir]
(
	@ID varchar(13),
	@LoginID varchar(20)
)
AS
BEGIN
	
	SET NOCOUNT ON;

--Drop table #tempTableAll
select 
[ID] = a.id,
[PoId]=a.PoId,
[SEQ1]=a.Seq1,
[SEQ2]=a.Seq2,
[SuppID]=C.SuppID,
[SCIRefno]=b.SCIRefno,
[Refno]=b.Refno,
[FactoryID]=b.FactoryID,
[ReceivingID]=a.Id,
[ArriveQty]= a.StockQty,
[AddName]=@LoginID,
[AddDate]= CONVERT(date,getdate()),
[MinSciDelivery] = (select MinSciDelivery from GetSCI(a.PoId,d.Category)) ,
[KPILETA] = d.KPILETA,
[Category]=d.Category,
[WhseArrival] = e.WhseArrival,
[fabricType]=b.FabricType
into #tempTableAll
from Receiving_Detail a
inner join PO_Supp_Detail b on a.PoId=b.ID and a.Seq1=b.SEQ1 and a.Seq2=b.SEQ2
inner join PO_Supp c on c.ID=a.PoId and c.SEQ1 = a.Seq1
inner join Orders d on d.Id=a.PoId
inner join Receiving e on a.Id=e.Id
where a.Id = @ID


--Fir

	declare @tempFir table(id bigint,deID bigint )	

Merge Production.dbo.Fir as t
using (
select * from (
select CONVERT(date,dateadd(DAY,+7,WhseArrival)) as InspDeadLine,* from #tempTableAll
where Category='M'
union all
select  Kpileta,* from #tempTableAll
where Category<>'M'
and  (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) >= 21 
and (datediff(day,convert(date,DATEADD(day,-3, Kpileta)),CONVERT(date,[WhseArrival])))<1
union all
select CONVERT(date,DATEADD(day,+7,WhseArrival)),* from #tempTableAll
where Category<>'M'
and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) >= 21 
and (datediff(day,convert(date,DATEADD(day,-3, Kpileta)),CONVERT(date,[WhseArrival])))>=1
union all
select CONVERT(date,DATEADD(day,-21,[MinSciDelivery])),* from #tempTableAll
where Category<>'M'
and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) < 21 
and (datediff(day,convert(date,DATEADD(day,-21, [MinSciDelivery])),CONVERT(date,[WhseArrival])))< 1
union all
select  CONVERT(date,DATEADD(day,+7,WhseArrival)),* from #tempTableAll
where Category<>'M'
and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) < 21 
and (datediff(day,convert(date,DATEADD(day,-21, [MinSciDelivery])),CONVERT(date,[WhseArrival])))>= 1
)a
where a.fabricType='F'
 ) as s
on t.poid=s.poid and t.seq1=s.seq1 and t.seq2=s.seq2 and t.receivingid=s.id 
when matched then
 update set
 t.suppid=s.suppid,
 t.scirefno=s.scirefno,
 t.refno=s.refno,
 --t.factoryid=s.factoryid,
 t.ArriveQty = s.ArriveQty,
 t.InspDeadLine = s.InspDeadLine,
 t.AddName=s.AddName,
 t.AddDate=s.AddDate
 when not matched by target then
 insert([PoId],[SEQ1],[SEQ2],[SuppID],[SCIRefno],[Refno],/*[FactoryID],*/[ReceivingID],[ArriveQty],[InspDeadLine],[AddName],[AddDate])
 values(s.PoId,s.Seq1,s.Seq2,s.SuppID,s.SCIRefno,s.Refno,/*s.FactoryID,*/s.Id,s.ArriveQty,s.InspDeadLine,s.AddName,AddDate)
when not matched by source and t.ReceivingID=@ID then
 delete
 output inserted.id as Id ,DELETED.id as deID
 into @tempFir;
 
-------FIR_Laboratory
Merge Production.dbo.FIR_Laboratory as t
using( 
select * from Production.dbo.FIR where id in (select id from @tempFir )
) as s
on t.id=s.id
when not matched by target then 
insert (id,poid,seq1,seq2,InspDeadline)
values(s.id,s.poid,s.seq1,s.seq2,s.InspDeadline)
when not matched by source and t.id in (select deID from @tempFir where id is null)  then
delete ;



---- Air

declare @tempAir table(id bigint,deID bigint )	

Merge Production.dbo.Air as t
using (
select * from (
select CONVERT(date,dateadd(DAY,+7,WhseArrival)) as InspDeadLine,* from #tempTableAll
where Category='M'
union all
select  Kpileta,* from #tempTableAll
where Category<>'M'
and  (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) >= 21 
and (datediff(day,convert(date,DATEADD(day,-3, Kpileta)),CONVERT(date,[WhseArrival])))<1
union all
select CONVERT(date,DATEADD(day,+7,WhseArrival)),* from #tempTableAll
where Category<>'M'
and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) >= 21 
and (datediff(day,convert(date,DATEADD(day,-3, Kpileta)),CONVERT(date,[WhseArrival])))>=1
union all
select CONVERT(date,DATEADD(day,-21,[MinSciDelivery])),* from #tempTableAll
where Category<>'M'
and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) < 21 
and (datediff(day,convert(date,DATEADD(day,-21, [MinSciDelivery])),CONVERT(date,[WhseArrival])))< 1
union all
select  CONVERT(date,DATEADD(day,+7,WhseArrival)),* from #tempTableAll
where Category<>'M'
and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) < 21 
and (datediff(day,convert(date,DATEADD(day,-21, [MinSciDelivery])),CONVERT(date,[WhseArrival])))>= 1
)a
where a.fabricType='A'
 ) as s
on t.poid=s.poid and t.seq1=s.seq1 and t.seq2=s.seq2 and t.receivingid=s.id 
when matched then
 update set
 t.suppid=s.suppid,
 t.scirefno=s.scirefno,
 t.refno=s.refno,
 --t.factoryid=s.factoryid,
 t.ArriveQty = s.ArriveQty,
 t.InspDeadLine = s.InspDeadLine,
 t.AddName=s.AddName,
 t.AddDate=s.AddDate
 when not matched by target then
 insert([PoId],[SEQ1],[SEQ2],[SuppID],[SCIRefno],[Refno],/*[FactoryID],*/[ReceivingID],[ArriveQty],[InspDeadLine],[AddName],[AddDate])
 values(s.PoId,s.Seq1,s.Seq2,s.SuppID,s.SCIRefno,s.Refno,/*s.FactoryID,*/s.Id,s.ArriveQty,s.InspDeadLine,s.AddName,AddDate)
when not matched by source and t.ReceivingID=@ID then
 delete
 output inserted.id as Id ,DELETED.id as deID
 into @tempAir;

--------------AIR_Laboratory
Merge Production.dbo.AIR_Laboratory as t
using( 
select * from Production.dbo.air where id in (select id from @tempAir ) 
) as s
on t.id=s.id
when not matched by target then 
insert (id,poid,seq1,seq2,InspDeadline)
values(s.id,s.poid,s.seq1,s.seq2,s.InspDeadline)
when not matched by source and t.id in (select deID from @tempAir where id is null)  then
delete ;


drop table #tempTableAll

END