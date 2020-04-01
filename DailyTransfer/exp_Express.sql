
-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/8/17>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Express]
	---- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
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

declare @DateStart date= CONVERT(DATE,DATEADD(day,-30,GETDATE()));
declare @DateEnd date=CONVERT(date, '9999-12-31');
declare @DateInfoName varchar(30) ='FactoryExpress';

--新增區間資料到DateInfo name='FactoryExpress' DateStart=轉檔日期前30天 DateEnd= 日期最大值
--確保轉出給Trade區間資料等同於DateInfo
If Exists (Select 1 From Pms_To_Trade.dbo.DateInfo Where Name = @DateInfoName )
Begin
	update Pms_To_Trade.dbo.dateInfo
	set DateStart=@DateStart,
	DateEnd=@DateEnd
	Where Name = @DateInfoName 
end;
Else
Begin 
	insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
	values (@DateInfoName,@DateStart,@DateEnd);
end;


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




