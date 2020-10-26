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
轉入 ICR,ICR_Detail,ICR_ReplacementReport
*/

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='ICR';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= (Select DateStart From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)
if @DateEnd is Null
	set @DateEnd = (Select DateEnd From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

-- #tmp Trade_To_PMS ICR
	SELECT *
	INTO #Trade_ICR 
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

-- ICR 刪除主TABLE多的資料
Delete Production.dbo.ICR
from Production.dbo.ICR as a 
left join #Trade_ICR as b
on a.id = b.id
where b.id is null
and (a.EditDate between @DateStart and @DateEnd
	or a.AddDate between @DateStart and @DateEnd
)
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
	a.ActFreight= b.EstFreight,
	a.OtherAmt= b.OtherAmt,
	a.RMtlAmtUSD= ROUND(b.RMtlAmt * dbo.getRate('FX','TWD','USD',GetDate()),2),
	a.EstFreightUSD= ROUND(b.EstFreight * dbo.getRate('FX','TWD','USD',GetDate()),2),
	a.ActFreightUSD= ROUND(b.EstFreight * dbo.getRate('FX','TWD','USD',GetDate()),2),
	a.OtherAmtUSD= ROUND(b.OtherAmt * dbo.getRate('FX','TWD','USD',GetDate()),2),
	a.Exchange= b.Exchange,
	a.IrregularPOCostID= b.IrregularPOCostID,
	a.Description= b.Description,
	a.Suggestion= b.Suggestion,
	a.Remark= b.Remark,
	a.AddName= b.AddName,
	a.AddDate= b.AddDate,
	a.EditName= b.EditName,
	a.EditDate= b.EditDate,
	a.BulkFTY = isnull(b.BulkFTY, '')
from Production.dbo.ICR as a 
inner join #Trade_ICR as b ON a.id=b.id


-------------------------- INSERT INTO 
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
	  ,[BulkFTY]
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
      ,[EstFreight]
      ,[OtherAmt]
      ,ROUND(RMtlAmt * dbo.getRate('FX','TWD','USD',GetDate()),2)
	  ,ROUND(EstFreight * dbo.getRate('FX','TWD','USD',GetDate()),2)
	  ,ROUND(EstFreight * dbo.getRate('FX','TWD','USD',GetDate()),2)
	  ,ROUND(OtherAmt * dbo.getRate('FX','TWD','USD',GetDate()),2)
      ,[Exchange]
      ,[IrregularPOCostID]
      ,[Description]
      ,[Suggestion]
      ,[Remark]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
	  ,isnull([BulkFTY], '')
from #Trade_ICR as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ICR as a WITH (NOLOCK) where a.id = b.id)


-- ICR_Detail 刪除主TABLE多的資料
Delete Production.dbo.ICR_Detail
from Production.dbo.ICR_Detail as a 
left join #Trade_ICR_Detail as b
on a.id = b.id and a.Seq1=b.Seq1 and a.Seq2=b.Seq2
where b.id is null
and exists(
	select 1 
	from Production.dbo.ICR t
	where t.ID = a.ID
	and t.EditDate between @DateStart and @DateEnd
)

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
	a.PriceUSD= ROUND(b.Price * dbo.getRate('FX','TWD','USD',GetDate()),2),
	a.AddName= b.AddName,
	a.AddDate= b.AddDate,
	a.EditName= b.EditName,
	a.EditDate= b.EditDate
from Production.dbo.ICR_Detail as a 
inner join #Trade_ICR_Detail as b ON a.id=b.id
and a.Seq1=b.Seq1 and a.Seq2=b.Seq2

-------------------------- INSERT INTO
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
	,ROUND(Price * dbo.getRate('FX','TWD','USD',GetDate()),2)
	,AddName
	,AddDate
	,EditName
	,EditDate
from #Trade_ICR_Detail as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ICR_Detail as a WITH (NOLOCK) where a.id = b.id 
and a.Seq1=b.Seq1 and a.Seq2=b.Seq2)


--ICR_ReplacementReport 刪除主TABLE多的資料
Delete Production.dbo.ICR_ReplacementReport
from Production.dbo.ICR_ReplacementReport as a 
left join #Trade_ICR_ReplacementReport as b
on a.id = b.id and a.ReplacementNo=b.ReplacementNo
where b.id is null
and exists(
	select 1 
	from Production.dbo.ICR t
	where t.ID = a.ID
	and t.EditDate between @DateStart and @DateEnd
)

-------------------------- INSERT INTO 
INSERT INTO Production.dbo.ICR_ReplacementReport
 (
	 ID
	,ReplacementNo
)
SELECT 
	 ID
	,ReplacementNo
from #Trade_ICR_ReplacementReport as b WITH (NOLOCK)
where not exists(
	select id 
	from Production.dbo.ICR_ReplacementReport as a WITH (NOLOCK) 
	where a.id = b.id 
	and a.ReplacementNo=b.ReplacementNo
)

--------------------------delete ICR_ResponsibilityDept
delete i
from Production.dbo.ICR_ResponsibilityDept i
where not exists (select 1 from Production.dbo.ICR where i.ID = ID)
		and not exists (select 1 from Production.dbo.ReplacementReport where i.ID = ID)


END


