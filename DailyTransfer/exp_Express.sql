
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

--�s�W�϶���ƨ�DateInfo name='FactoryExpress' DateStart=���ɤ���e30�� DateEnd= ����̤j��
--�T�O��X��Trade�϶���Ƶ��P��DateInfo
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
FROM [Production].dbo.Express  
WHERE upper(FreightBy) = '3RD'
and (AddDate >=@DateStart  or EditDate >=@DateStart) ORDER BY Id

SELECT B.* 
INTO Express_Detail
FROM  Pms_To_Trade.dbo.Express  A, [Production].dbo.Express_Detail  B WHERE A.ID = B.ID ORDER BY B.ID 

SELECT B.* 
INTO  Express_CTNData
FROM Pms_To_Trade.dbo.Express A, [Production].dbo.Express_CTNData B WHERE A.ID = B.ID ORDER BY B.ID 

UPDATE [Production].dbo.Express 
SET  SendDate =GETDATE()
FROM [Production].dbo.Express A  INNER JOIN Production.dbo.Express ON A.Id = Express.Id
WHERE A.SendDate IS NULL
AND A.Status in ('Approved','Junked')




END




