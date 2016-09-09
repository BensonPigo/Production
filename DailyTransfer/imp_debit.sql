

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/19>
-- Description:	<import debit>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Debit]

AS
BEGIN

declare @Sayfty table(id varchar(10)) --工廠代碼
insert @Sayfty select id from Production.dbo.Factory

-- update Production.Debit
Update pd1
Set pd1.TransidSettle = td1.TransidSettle ,
	pd1.EditName = td1.EditName,
	pd1.EditDate = td1.EditDate,
	pd1.SysDate = td1.SysDate
from Trade_To_Pms.dbo.Debit td1 
Inner join Production.dbo.Debit pd1 on td1.id= pd1.id
Where td1.editdate<>pd1.editdate 
and td1.BrandID in (select id from @Sayfty )
Or td1.sysdate<> pd1.sysdate


--新增資料到Production.Debit
Insert Into Production.dbo.Debit
		([ID]
      ,[Issuedate]
      ,[CurrencyID]
      ,[Amount]
      ,[Received]
      ,[BuyerID]
      ,[BrandID]
      ,[BankID]
      ,[MDivisionID]
      ,[LCFNO]
      ,[LCFDate]
      ,[EstPayDate]
      ,[Title]
      ,[SendFrom]
      ,[Attn]
      ,[CC]
      ,[Subject]
      ,[Handle]
      ,[SMR]
      ,[TransID]
      ,[BadID]
      ,[Status]
      ,[StatusRevise]
      ,[StatusReviseNm]
      ,[CustPayId]
      ,[Settled]
      ,[SettleDate]
      ,[Cfm]
      ,[CfmDate]
      ,[Lock]
      ,[Lockdate]
      ,[OldAmount]
      ,[Type]
      ,[ShareFob]
      ,[TransidFactory]
      ,[TransidSettle]
      ,[IsSubcon]
      ,[LCLName]
      ,[LCLCurrency]
      ,[LCLAmount]
      ,[LCLRate]     
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[SysDate])
Select  td1.[ID]
      ,td1.[CDate]
      ,td1.[CurrencyID]
      ,td1.[Amount]
      ,td1.[Received]
      ,td1.[BuyerID]
      ,td1.[BrandID]
      ,td1.[BankID]
	  ,td1.[BrandID]
      ,td1.[LCFNO]
      ,td1.[LCFDate]
      ,td1.[EstPayDate]
      ,td1.[Title]
      ,td1.[SendFrom]
      ,td1.[Attn]
      ,td1.[CC]
      ,td1.[Subject]
      ,td1.[HANDLE]
      ,td1.[SMR]
      ,td1.[TransID]
      ,td1.[BadID]
      ,td1.[Status]
      ,td1.[StatusRevise]
      ,td1.[StatusReviseNm]
      ,td1.[CustPayID]
      ,td1.[Settled]
      ,td1.[SettleDate]
      ,td1.[Cfm]
      ,td1.[CfmDate]      
      ,td1.[Lock]
      ,td1.[LockDate]
      ,td1.[OldAmount]
      ,td1.[Type]
      ,td1.[ShareFob]
      ,td1.[TransidFactory]
      ,td1.[TransidSettle]
      ,td1.[IsSubcon]
      ,td1.[LCLName]
      ,td1.[LCLCurrency]
      ,td1.[LCLAmount]
      ,td1.[LCLRate]
      ,td1.[AddName]
      ,td1.[AddDate]
      ,td1.[EditName]
      ,td1.[EditDate]
	  ,td1.[SysDate]
from Trade_To_Pms.dbo.debit td1
Where not exists  (select top 1 pd1.ID from Production.dbo.Debit pd1 where pd1.id=td1.id)
and td1.BrandID in (select id from @Sayfty )

--新增 insert into  Production.Debit_detail

Insert into Production.dbo.Debit_Detail
(ID,ORDERID,REASONID,Description,PRICE,Amount,UnitID,SOURCEID,QTY,ReasonNM)
Select
 td2.ID,td2.ORDERID,td2.REASONID,td2.Description,td2.PRICE,td2.Amount,td2.UnitID,td2.SOURCEID,td2.QTY,td2.Reason 
From Trade_To_Pms.dbo.debit_detail as td2, Trade_To_Pms.dbo.debit as td1
Where td1.Id = td2.Id

-- 新增 insert into Production.dbo.LocalDebit
Insert  into Production.dbo.LocalDebit
(TaipeiDBC,id, FactoryID, TaipeiAMT, TaipeiCurrencyID, AddDate, AddName,status)
Select '1', Id, BrandID, Amount, CurrencyID, AddDate,'SCIMIS','New' 
from Trade_To_Pms.dbo.debit as td1
where IsSubcon = 1


-- 新增 insert into Production.dbo.LocalDebit_Detail
insert into Production.dbo.LocalDebit_Detail
(id, Orderid, UnitID, qty, amount, TaipeiReason, AddDate, AddName)
select td2.Id, td2.orderid, td2.UnitID, td2.qty, td2.Amount, td2.Reason, td1.AddDate, 'SCIMIS'
from Trade_To_Pms.dbo.debit_detail as td2
inner join Trade_To_Pms.dbo.debit as td1 on td1.id = td2.id


END




