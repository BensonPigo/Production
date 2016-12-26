

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


SELECT * 
INTO Debit
FROM Production.dbo.Debit
WHERE EditDate 
BETWEEN Convert(DATE,DATEADD(day,-30,GETDATE()))AND CONVERT(date,Getdate()) OR SysDate BETWEEN Convert(DATE,DATEADD(day,-30,GETDATE())) AND CONVERT(date,Getdate())
ORDER BY ID ;


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
 select a.ID,a.IssueDate,a.Amount,b.VoucherID,b.AddName,b.AddDate,b.EditDate,b.EditName,b.SysDate 
 INTO Debit5
 from amount a
 inner join allDebit b on a.id=b.id and a.IssueDate=b.IssueDate 

