


CREATE PROCEDURE [dbo].[insert_Air_Fir]
(
	@ID varchar(13),
	@LoginID varchar(20)
)
AS
BEGIN
			SET NOCOUNT ON;

BEGIN TRANSACTION

BEGIN TRY

select 
[ID] = a.id,
[PoId]=a.PoId,
[SEQ1]=a.Seq1,
[SEQ2]=a.Seq2,
[SuppID]=C.SuppID,
[SCIRefno]=b.SCIRefno,
[Refno]=b.Refno,
[ReceivingID]=a.Id,
[ArriveQty]= a.StockQty,
[AddName]=@LoginID,
[AddDate]= CONVERT(date,getdate()),
[MinSciDelivery] = (select MinSciDelivery from GetSCI(a.PoId,d.Category)) ,
[KPILETA] = d.KPILETA,
[Category]=d.Category,
[WhseArrival] = e.WhseArrival,
[fabricType]=b.FabricType,
FirNonMoisture = IIF(exists(select 1 from Brand_QAMoistureStandardList where brandid = b.BrandId),0 ,1)
into #tempTableAll
from Receiving_Detail a
inner join PO_Supp_Detail b on a.PoId=b.ID and a.Seq1=b.SEQ1 and a.Seq2=b.SEQ2
inner join PO_Supp c on c.ID=a.PoId and c.SEQ1 = a.Seq1
inner join Orders d on d.id=a.PoId
inner join Receiving e on a.Id=e.Id
where a.Id = @ID

declare @fabricType varchar(2)

if exists (select 1 from #tempTableAll where fabricType='F')
	begin 
		set @fabricType ='F'		
	end
else	
	begin 
		set @fabricType ='A'			
	end

select distinct InspDeadLine,PoId,SEQ1,SEQ2 into #InspDeadLine from (
		select CONVERT(date,dateadd(DAY,+7,WhseArrival)) as InspDeadLine,* from #tempTableAll
		where Category='M'
		union 
		select  Kpileta,* from #tempTableAll
		where Category<>'M'
		and  (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) >= 21 
		and (datediff(day,convert(date,DATEADD(day,-3, Kpileta)),CONVERT(date,[WhseArrival])))<1
		union 
		select CONVERT(date,DATEADD(day,+7,WhseArrival)),* from #tempTableAll
		where Category<>'M'
		and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) >= 21 
		and (datediff(day,convert(date,DATEADD(day,-3, Kpileta)),CONVERT(date,[WhseArrival])))>=1
		union 
		select CONVERT(date,DATEADD(day,-21,[MinSciDelivery])),* from #tempTableAll
		where Category<>'M'
		and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) < 21 
		and (datediff(day,convert(date,DATEADD(day,-21, [MinSciDelivery])),CONVERT(date,[WhseArrival])))< 1
		union 
		select  CONVERT(date,DATEADD(day,+7,WhseArrival)),* from #tempTableAll
		where Category<>'M'
		and (DATEDIFF(day,CONVERT(date,Kpileta),convert(date,MinSciDelivery))) < 21 
		and (datediff(day,convert(date,DATEADD(day,-21, [MinSciDelivery])),CONVERT(date,[WhseArrival])))>= 1
		)a
		where a.fabricType=@fabricType	
		
--Fir

	declare @tempFir table(id bigint,deID bigint )	

RAISERROR('insert_Air_Fir - Starts',0,0)
Merge dbo.Fir as t
using (
	--select * from #tempTableAll	where fabricType='F'
	select 	a.ID,
			a.PoId,
			a.SEQ1,
			a.SEQ2,
			a.SuppID,
			a.SCIRefno,
			a.Refno,
			a.ReceivingID,
			SUM(a.ArriveQty) ArriveQty,
			a.AddName,
			a.AddDate,
			a.MinSciDelivery  ,
			a.KPILETA,
			a.Category,
			a.WhseArrival ,
			a.fabricType ,
			b.InspDeadLine,
			FirNonMoisture
			from #tempTableAll a
			left join #InspDeadLine b on a.PoId = b.PoId and a.Seq1 =b.Seq1 and a.Seq2 = b.Seq2
			where fabricType='F' 
			GROUP BY 
			a.ID,
			a.PoId,
			a.SEQ1,
			a.SEQ2,
			a.SuppID,
			a.SCIRefno,
			a.Refno,
			a.ReceivingID,
			a.AddName,
			a.AddDate,
			a.MinSciDelivery  ,
			a.KPILETA,
			a.Category,
			a.WhseArrival ,
			a.fabricType,
			b.InspDeadLine,
			FirNonMoisture
 ) as s
on t.poid=s.poid and t.seq1=s.seq1 and t.seq2=s.seq2 and t.receivingid=s.id 
when matched then
 update set
 t.suppid=s.suppid,
 t.scirefno=s.scirefno,
 t.refno=s.refno,
 t.ArriveQty = s.ArriveQty,
 t.InspDeadLine = s.InspDeadLine,
 t.AddName=s.AddName,
 t.AddDate=s.AddDate
 when not matched by target then
 insert([PoId],[SEQ1],[SEQ2],[SuppID],[SCIRefno],[Refno],[ReceivingID],[ArriveQty],[InspDeadLine],[NonMoisture],[AddName],[AddDate])
 values(s.PoId,s.Seq1,s.Seq2,s.SuppID,s.SCIRefno,s.Refno,s.Id,s.ArriveQty,s.InspDeadLine,FirNonMoisture,s.AddName,AddDate)
when not matched by source and t.ReceivingID=@ID then
 delete
 output inserted.id as Id ,DELETED.id as deID
 into @tempFir;
 
-------FIR_Laboratory
RAISERROR('insert_Air_Fir - Starts',0,0)
MERGE dbo.fir_laboratory AS t 
using(SELECT a.*, 
             Isnull(c.ID, '1') AS SkewnessOptionID 
      FROM   dbo.fir a 
             LEFT JOIN po b 
                    ON a.poid = b.id 
             LEFT JOIN skewnessoption c 
                    ON c.brandid = b.brandid AND Junk=0
      WHERE  a.id IN (SELECT id 
                      FROM   @tempFir)) AS s 
ON t.id = s.id 
WHEN NOT matched BY target THEN 
  INSERT (id, 
          poid, 
          seq1, 
          seq2, 
          inspdeadline, 
          skewnessoptionid) 
  VALUES(s.id, 
         s.poid, 
         s.seq1, 
         s.seq2, 
         s.inspdeadline, 
         s.skewnessoptionid) ; 

delete dbo.fir_laboratory where id in  (SELECT deID FROM @tempFir WHERE id IS NULL)

---- Air

declare @tempAir table(id bigint,deID bigint )	
RAISERROR('insert_Air_Fir - Starts',0,0)
Merge dbo.Air as t
using (
	select	a.*,
			b.InspDeadLine
	 from #tempTableAll a
		left join #InspDeadLine b on a.PoId = b.PoId and a.Seq1 =b.Seq1 and a.Seq2 = b.Seq2
		where fabricType='A'
 ) as s
on t.poid=s.poid and t.seq1=s.seq1 and t.seq2=s.seq2 and t.receivingid=s.id 
when matched then
 update set
 t.suppid=s.suppid,
 t.scirefno=s.scirefno,
 t.refno=s.refno,
 t.ArriveQty = s.ArriveQty,
 t.InspDeadLine = s.InspDeadLine,
 t.AddName=s.AddName,
 t.AddDate=s.AddDate
 when not matched by target then
 insert([PoId],[SEQ1],[SEQ2],[SuppID],[SCIRefno],[Refno],[ReceivingID],[ArriveQty],[InspDeadLine],[AddName],[AddDate])
 values(s.PoId,iif(len(s.Seq1)<=2,s.Seq1+' ',s.Seq1),s.Seq2,s.SuppID,s.SCIRefno,s.Refno,s.Id,s.ArriveQty,s.InspDeadLine,s.AddName,AddDate)
when not matched by source and t.ReceivingID=@ID then
 delete
 output inserted.id as Id ,DELETED.id as deID
 into @tempAir;

--------------AIR_Laboratory
RAISERROR('insert_Air_Fir - Starts',0,0)
Merge dbo.AIR_Laboratory as t
using( 
select * from dbo.air where id in (select id from @tempAir ) 
) as s
on t.id=s.id
when not matched by target then 
insert (id,poid,seq1,seq2,InspDeadline)
values(s.id,s.poid,s.seq1,s.seq2,s.InspDeadline);

delete dbo.AIR_Laboratory where id in  (select deID from @tempAir where id is null)

--------------FIR_Shadebone 
RAISERROR('insert_Air_Fir - Starts',0,0)

--FabricType = F�~����H�U�q��
IF EXISTS(
SELECT 1 
FROM Receiving_Detail r
INNER JOIN PO_Supp_Detail p ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2 
WHERE r.ID=@ID AND p.FabricType='F' 
)
BEGIN 

SELECT   [FirID]=f.ID
		,[Roll]=r.Roll
		,[Dyelot]=r.Dyelot
		,[StockQty]=r.StockQty
INTO #tmp_Receiving
FROM Receiving_Detail r
INNER JOIN PO_Supp_Detail p ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2 
INNER JOIN FIR f ON f.ReceivingID=r.ID AND f.POID=r.PoId AND f.SEQ1=r.Seq1 AND f.SEQ2=r.Seq2
WHERE r.ID=@ID AND p.FabricType='F'

Merge dbo.FIR_Shadebone  as t
using( 
	 SELECT * FROM #tmp_Receiving
) as s
on t.ID=s.FirID AND t.Roll=s.Roll AND t.Dyelot=s.Dyelot 

WHEN MATCHED THEN
	 UPDATE SET t.TicketYds=s.StockQty ,t.EditName=@LoginID, t.EditDate=GETDATE()

WHEN NOT MATCHED by TARGET THEN 
	insert  ([ID]           ,[Roll]           ,[Dyelot]           ,[Scale]           ,[Inspdate]           ,[Inspector]           ,[Result]
            ,[Remark]       ,[AddName]        ,[AddDate]          ,[EditName]        ,[EditDate]           ,[TicketYds])
	values(  s.FirID   ,s.Roll      ,s.Dyelot      ,''                ,NULL                 ,''                    ,''
	        ,''             ,@LoginID         ,GETDATE()          ,''                ,NULL                 ,s.StockQty )
;

END

------
	COMMIT TRANSACTION

END TRY

--drop table #InspDeadLine
--drop table #tempTableAll
--drop table #tmp_Receiving

BEGIN CATCH
	ROLLBACK TRANSACTION;
	
	EXECUTE usp_GetErrorInfo;
END CATCH

END


GO


