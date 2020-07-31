
CREATE PROCEDURE [dbo].[exp_Express]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.Express') IS NOT NULL
BEGIN
  DROP TABLE Express
END
IF OBJECT_ID(N'dbo.Express_Detail') IS NOT NULL
BEGIN
  DROP TABLE Express_Detail
END
IF OBJECT_ID(N'dbo.Express_CTNData') IS NOT NULL
BEGIN
  DROP TABLE Express_CTNData
END

------------------------------------------------------------------------------------------------------
declare @DateInfoName varchar(30) ='FactoryExpress';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateStart);
------------------------------------------------------------------------------------------------------


SELECT * 
INTO Express
FROM [Production].dbo.Express e
WHERE (e.AddDate >=@DateStart  or e.EditDate >=@DateStart) 
		or exists (
			select 1
			from [Production].dbo.Express_Detail ed
			inner join [Production].dbo.PackingList pl with(nolock) on pl.id = ed.PackingListID 
			inner join [Production].dbo.Pullout p with(nolock) on pl.PulloutID = p.ID
			where e.id = ed.id
					and (p.AddDate >=@DateStart  or p.EditDate >=@DateStart) 
		)
ORDER BY Id

SELECT B.* ,AirPPID=iif(isnull(b.PackingListID,'') = '',b.DutyNo , airpp.AirPPno), pl.PulloutID
INTO Express_Detail
FROM  Pms_To_Trade.dbo.Express  A
inner join [Production].dbo.Express_Detail  B on A.ID = B.ID
left join [Production].dbo.PackingList pl with(nolock) on pl.id = B.PackingListID 
outer apply(
	select top 1 AirPPno = AirPP.ID
	from [Production].dbo.PackingList_Detail pld with(nolock)
	inner join [Production].dbo.AirPP with(nolock) on AirPP.OrderID = pld.OrderID and AirPP.OrderShipmodeSeq = pld.OrderShipmodeSeq
	where pld.id = B.PackingListID and pld.OrderID = B.OrderID
	order by AirPP.AddDate desc
)airpp
 ORDER BY B.ID 

SELECT B.* 
INTO  Express_CTNData
FROM Pms_To_Trade.dbo.Express A, [Production].dbo.Express_CTNData B WHERE A.ID = B.ID ORDER BY B.ID 

UPDATE [Production].dbo.Express 
SET  SendDate =GETDATE()
FROM [Production].dbo.Express A  INNER JOIN Production.dbo.Express ON A.Id = Express.Id
WHERE A.SendDate IS NULL
AND A.Status in ('Approved','Junked')




END




