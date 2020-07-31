
CREATE PROCEDURE [dbo].[exp_adicomp] 
AS
BEGIN
SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.ADIDASComplain') IS NOT NULL
BEGIN
  DROP TABLE ADIDASComplain
END

IF OBJECT_ID(N'dbo.ADIDASComplain_Detail') IS NOT NULL
BEGIN
  DROP TABLE ADIDASComplain_Detail
END

------------------------------------------------------------------------------------------------------
declare @DateInfoName varchar(30) ='ADIDASComplain';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateStart);
------------------------------------------------------------------------------------------------------

SELECT
	ID
	,TPEApvDate
	,FtyApvName
	,FtyApvDate
	into ADIDASComplain
FROM Production.dbo.ADIDASComplain with (nolock)
where FtyApvDate >= @DateStart

SELECT
	b.UKey,
	b.SuppID,
	b.Refno,
	b.IsEM,
	Responsibility
	into ADIDASComplain_Detail
FROM Production.dbo.ADIDASComplain a with (nolock)
inner join Production.dbo.ADIDASComplain_Detail b with (nolock) on a.ID = b.ID
where a.FtyApvDate >= @DateStart

END




