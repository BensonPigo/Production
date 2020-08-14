

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_debit>
-- =============================================
Create PROCEDURE [dbo].[exp_Debit]
AS

IF OBJECT_ID(N'dbo.Debit') IS NOT NULL
BEGIN
  DROP TABLE Debit
END


IF OBJECT_ID(N'Debit5') IS NOT NULL
BEGIN
  DROP TABLE Debit5
END

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='Debit';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

SELECT * 
INTO Debit
FROM Production.dbo.Debit
WHERE EditDate BETWEEN @DateStart AND @DateEnd OR SysDate BETWEEN @DateStart AND @DateEnd
ORDER BY ID ;

-------PMS  Debit_Schedule會先轉到Debit5，再透過Trade的SP轉進 Debit_Schedules-------
with amount as(
 select id,IssueDate,sum(Amount) as Amount from Production.dbo.Debit_Schedule group by id,IssueDate
 ),
allDebit as(
 Select ds.* 
from ( select * from (
 select RN = Row_number() over (partition by id,issuedate order by id,issuedate),* from Production.dbo.Debit_Schedule ) a
 where a.RN=1) ds
inner join Production.dbo.Debit d on ds.ID = d.ID
where exists (select 1 from Production.dbo.Debit_Schedule tds 
where tds.ID = ds.ID )
 )
 select a.ID,a.IssueDate,a.Amount,b.VoucherID,b.AddName,b.AddDate,b.EditDate,b.EditName,b.SysDate ,b.CurrencyID
 INTO Debit5
 from amount a
 inner join allDebit b on a.id=b.id and a.IssueDate=b.IssueDate 

