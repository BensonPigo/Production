
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
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='ADIDASComplain';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
	
--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo set DateStart = @DateStart,DateEnd = @DateStart, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateStart,@Remark);
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
	Responsibility,
	IsLocalSupp
	into ADIDASComplain_Detail
FROM Production.dbo.ADIDASComplain a with (nolock)
inner join Production.dbo.ADIDASComplain_Detail b with (nolock) on a.ID = b.ID
where a.FtyApvDate >= @DateStart

END




