-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<ICR >
-- =============================================
Create PROCEDURE [dbo].[imp_ICR]	
AS
BEGIN
	SET NOCOUNT ON;
/*
轉入 ICR,ICR_Detail,ICR_ReplacementReport,ICR_ResponsibilityDept
*/

-- #tmp Trade_To_PMS ICR
	SELECT *
	INTO #Trade_ICR --先下條件把PO成為工廠別
	FROM  Trade_To_Pms.dbo.ICR b WITH (NOLOCK) 
	where exists(
		select 1 from Production.dbo.Factory
		where IsProduceFty=1
		and id = b.Department
	)
-- #tmp Trade_To_PMS ICR_Detail
	select * 
	into #Trade_ICR_Detail
	FROM  Trade_To_Pms.dbo.ICR_Detail b WITH (NOLOCK) 
	where exists(
		select 1 from #Trade_ICR
		where id = b.ID
	)

-- #tmp Trade_To_PMS ICR_ReplacementReport
	select * 
	into #Trade_ICR_ReplacementReport
	FROM  Trade_To_Pms.dbo.ICR_ReplacementReport b WITH (NOLOCK) 
	where exists(
		select 1 from #Trade_ICR
		where id = b.ID
	)

-- #tmp Trade_To_PMS ICR_ResponsibilityDept
	select * 
	into #Trade_ICR_ResponsibilityDept
	FROM  Trade_To_Pms.dbo.ICR_ResponsibilityDept b WITH (NOLOCK) 
	where exists(
		select 1 from #Trade_ICR
		where id = b.ID
	)


-- ICR
--刪除主TABLE多的資料
Delete Production.dbo.ICR
from Production.dbo.ICR as a 
left join #Trade_ICR as b
on a.id = b.id
where b.id is null

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET 
	a.Responsible= b.Responsible,
	a.Department= b.Department,
	a.OrderID= b.OrderID,
	a.Status= b.Status,
	a.StatusUpdate= b.StatusUpdate,
	a.Handle= b.Handle,
	a.SMR= b.SMR,
	a.ReceiveHandle= b.ReceiveHandle,
	a.ReceiveDate= b.ReceiveDate,
	a.CFMDate= b.CFMDate,
	a.CFMHandle= b.CFMHandle,
	a.DutyHandle= b.DutyHandle,
	a.DutySMR= b.DutySMR,
	a.DutyManager= b.DutyManager,
	a.unpayable= b.unpayable,
	a.Deadline= b.Deadline,
	a.DutyStatus= b.DutyStatus,
	a.DutyStatusUpdate= b.DutyStatusUpdate,
	a.RMtlAmt= b.RMtlAmt,
	a.EstFreight= b.EstFreight,
	a.ActFreight= b.ActFreight,
	a.OtherAmt= b.OtherAmt,
	a.RMtlAmtUSD= ROUND(b.RMtlAmt * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2),
	a.EstFreightUSD= ROUND(b.EstFreight * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2),
	a.ActFreightUSD= ROUND(b.ActFreight * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2),
	a.OtherAmtUSD= ROUND(b.OtherAmt * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2),
	a.Exchange= b.Exchange,
	a.IrregularPOCostID= b.IrregularPOCostID,
	a.Description= b.Description,
	a.Suggestion= b.Suggestion,
	a.Remark= b.Remark,
	a.AddName= b.AddName,
	a.AddDate= b.AddDate,
	a.EditName= b.EditName,
	a.EditDate= b.EditDate,
	a.VoucherID= b.VoucherID,
	a.VoucherDate= b.VoucherDate
from Production.dbo.ICR as a 
inner join #Trade_ICR as b ON a.id=b.id


-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.ICR
 (
	   [Id]
      ,[Responsible]
      ,[Department]
      ,[OrderID]
      ,[Status]
      ,[StatusUpdate]
      ,[Handle]
      ,[SMR]
      ,[ReceiveHandle]
      ,[ReceiveDate]
      ,[CFMDate]
      ,[CFMHandle]
      ,[DutyHandle]
      ,[DutySMR]
      ,[DutyManager]
      ,[unpayable]
      ,[Deadline]
      ,[DutyStatus]
      ,[DutyStatusUpdate]
      ,[RMtlAmt]
      ,[EstFreight]
      ,[ActFreight]
      ,[OtherAmt]
      ,[RMtlAmtUSD]
      ,[EstFreightUSD]
      ,[ActFreightUSD]
      ,[OtherAmtUSD]
      ,[Exchange]
      ,[IrregularPOCostID]
      ,[Description]
      ,[Suggestion]
      ,[Remark]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[VoucherID]
      ,[VoucherDate]
)
SELECT 
	  [Id]
      ,[Responsible]
      ,[Department]
      ,[OrderID]
      ,[Status]
      ,[StatusUpdate]
      ,[Handle]
      ,[SMR]
      ,[ReceiveHandle]
      ,[ReceiveDate]
      ,[CFMDate]
      ,[CFMHandle]
      ,[DutyHandle]
      ,[DutySMR]
      ,[DutyManager]
      ,[unpayable]
      ,[Deadline]
      ,[DutyStatus]
      ,[DutyStatusUpdate]
      ,[RMtlAmt]
      ,[EstFreight]
      ,[ActFreight]
      ,[OtherAmt]
      ,ROUND(RMtlAmt * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2)
	  ,ROUND(EstFreight * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2)
	  ,ROUND(ActFreight * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2)
	  ,ROUND(OtherAmt * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2)
      ,[Exchange]
      ,[IrregularPOCostID]
      ,[Description]
      ,[Suggestion]
      ,[Remark]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[VoucherID]
      ,[VoucherDate]
from #Trade_ICR as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ICR as a WITH (NOLOCK) where a.id = b.id)


-- ICR_Detail
--刪除主TABLE多的資料
Delete Production.dbo.ICR_Detail
from Production.dbo.ICR_Detail as a 
left join #Trade_ICR_Detail as b
on a.id = b.id and a.Seq1=b.Seq1 and a.Seq2=b.Seq2
where b.id is null

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣
UPDATE a
SET 
	a.ID= b.ID,
	a.MtltypeID= b.MtltypeID,
	a.Seq1= b.Seq1,
	a.Seq2= b.Seq2,
	a.ICRQty= b.ICRQty,
	a.ICRFoc= b.ICRFoc,
	a.Price= b.Price,
	a.PriceUSD= ROUND(b.Price * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2),
	a.AddName= b.AddName,
	a.AddDate= b.AddDate,
	a.EditName= b.EditName,
	a.EditDate= b.EditDate
from Production.dbo.ICR_Detail as a 
inner join #Trade_ICR_Detail as b ON a.id=b.id
and a.Seq1=b.Seq1 and a.Seq2=b.Seq2

-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.ICR_Detail
 (
	 ID
	,MtltypeID
	,Seq1
	,Seq2
	,ICRQty
	,ICRFoc
	,Price
	,PriceUSD
	,AddName
	,AddDate
	,EditName
	,EditDate
)
SELECT 
	 ID
	,MtltypeID
	,Seq1
	,Seq2
	,ICRQty
	,ICRFoc
	,Price
	,ROUND(Price * (select Rate from dbo.GetCurrencyRate('FX','TWD','USD',GetDate())),2)
	,AddName
	,AddDate
	,EditName
	,EditDate
from #Trade_ICR_Detail as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ICR_Detail as a WITH (NOLOCK) where a.id = b.id 
and a.Seq1=b.Seq1 and a.Seq2=b.Seq2)

-- ICR_ReplacementReport

--刪除主TABLE多的資料
Delete Production.dbo.ICR_ReplacementReport
from Production.dbo.ICR_ReplacementReport as a 
left join #Trade_ICR_ReplacementReport as b
on a.id = b.id and a.ReplacementNo=b.ReplacementNo
where b.id is null

-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.ICR_ReplacementReport
 (
	 ID
	,ReplacementNo
)
SELECT 
	 ID
	,ReplacementNo
from #Trade_ICR_ReplacementReport as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ICR_ReplacementReport as a WITH (NOLOCK) where a.id = b.id 
and a.ReplacementNo=b.ReplacementNo)


-- ICR_ResponsibilityDept

--刪除主TABLE多的資料
Delete Production.dbo.ICR_ResponsibilityDept
from Production.dbo.ICR_ResponsibilityDept as a 
left join #Trade_ICR_ResponsibilityDept as b
on a.id = b.id and a.FactoryID = b.FactoryID and a.DepartmentID = b.DepartmentID
where b.id is null

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣
UPDATE a
SET 
	a.Percentage= b.Percentage,
	a.Amount= b.Amount
from Production.dbo.ICR_ResponsibilityDept as a 
inner join #Trade_ICR_ResponsibilityDept as b ON a.id=b.id
and a.FactoryID = b.FactoryID and a.DepartmentID = b.DepartmentID

-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.ICR_ResponsibilityDept
 (
	 ID
	,FactoryID
	,DepartmentID
	,Percentage
	,Amount
)
SELECT 
	 ID
	,FactoryID
	,DepartmentID
	,Percentage
	,Amount
from #Trade_ICR_ResponsibilityDept as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ICR_ResponsibilityDept as a WITH (NOLOCK) where a.id = b.id 
and a.DepartmentID = b.DepartmentID and a.FactoryID = b.FactoryID)


END
GO
