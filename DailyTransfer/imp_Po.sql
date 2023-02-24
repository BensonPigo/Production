
Create PROCEDURE imp_Po
AS
BEGIN
	SET NOCOUNT ON;

   SELECT b.*
INTO #Trade_To_Pms_PO --先下條件把PO成為工廠別
 FROM  Trade_To_Pms.dbo.PO b WITH (NOLOCK) inner join Production.dbo.Factory c WITH (NOLOCK) on b.FactoryID = c.ID

 --建立Trade_To_Pms.dbo.Po_Supp_Detail的index(ID, Seq1, Seq2) 避免下面執行join in 語法時卡住
 if not exists(SELECT 1 
				FROM [Trade_To_Pms].sys.indexes 
				WHERE name='IDX_Trade_To_PMS_PO_Supp_Detail' AND object_id = OBJECT_ID('Trade_To_Pms.dbo.PO_Supp_Detail'))
 begin
	CREATE CLUSTERED INDEX [IDX_Trade_To_PMS_PO_Supp_Detail] ON [Trade_To_Pms].[dbo].[PO_Supp_Detail]
	(
		[ID] ASC,
		[Seq1] ASC,
		[Seq2] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
 end

 -----Trade 的區間資料 new Eeit----	
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	declare @DateInfoName varchar(30) ='PO';
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

 If Object_ID('tempdb..#TransOrderList') Is Not Null
	Begin
		Drop Table #TransOrderList;
	End;
		
	Select Orders.ID, Orders.PoID
		 , IsNull(Style.Ukey, 0) as StyleUkey
		 , Orders.FactoryID
	  Into #TransOrderList
	  From Production.dbo.Orders
	  Left Join Production.dbo.Style
		On	   Style.BrandID = Orders.BrandID
		   And Style.ID = Orders.StyleID
		   And Style.SeasonID = Orders.SeasonID
	 Where Orders.FactoryID != ''
	  and exists (select 1 from Production.dbo.Factory where id=Orders.FactoryID)
	   And (   (	Orders.SciDelivery >= @DateStart
				And Orders.SciDelivery <= @DateEnd
			   )
			Or (	Orders.BuyerDelivery >= @DateStart
				And Orders.BuyerDelivery <= @DateEnd
			   )
		   )
	 Order by PoID, ID;

 ------------

--PO1 PO
--PMS多的
--,[FIRRemark]
--,[AIRemark]
--,[FIRLaboratoryRemark]
--,[AIRLaboratoryRemark]
--,[OvenLaboratoryRemark]
--,[ColorFastnessLaboratoryRemark]
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	    =b.ID	
      ,a.StyleID	      =b.StyleID	
      ,a.SeasonId	      =b.SeasonId	
      ,a.StyleUkey	      =b.StyleUkey	
      ,a.BrandID	      =b.BrandID	
      ,a.POSMR	      =b.POSMR	
      ,a.POHandle	      =b.POHandle	
      ,a.PCHandle	      =b.PCHandle	
      ,a.PCSMR	      =b.PCSMR	
      --,a.McHandle	      =b.McHandle	
      ,a.ShipMark	      =b.ShipMark	
      ,a.FTYMark	      =b.FTYMark	
      ,a.Complete	      =b.Complete	
      ,a.PoRemark	      =b.PoRemark	
      ,a.CostRemark	      =b.CostRemark	
      ,a.IrregularRemark	      =b.IrregularRemark	
      ,a.FirstPoError	      =b.FirstPoError	
      ,a.FirstEditName	      =b.FirstEditName	
      ,a.FirstEditDate	      =b.FirstEditDate	
      ,a.FirstAddDate	      =b.FirstAddDate	
      ,a.FirstCostDate	      =b.FirstCostDate	
      ,a.LastPoError	      =b.LastPoError	
      ,a.LastEditName	      =b.LastEditName	
      ,a.LastEditDate	      =b.LastEditDate	
      ,a.LastAddDate	      =b.LastAddDate	
      ,a.LastCostDate	      =b.LastCostDate	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.MTLDelay	      =b.MtlDelay
	  ,a.MinSciDelivery   = (select TOP 1 MinSciDelivery from [dbo].[Getsci](a.ID
																	   ,(SELECT Category FROM Orders WHERE ID = a.ID)
																	  )
							)
	  ,a.ThreadVersion    =b.ThreadVersion
from Production.dbo.PO as a inner join #Trade_To_Pms_PO as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.PO(
       ID
      ,StyleID
      ,SeasonId
      ,StyleUkey
      ,BrandID
      ,POSMR
      ,POHandle
      ,PCHandle
      ,PCSMR
      --,McHandle
      ,ShipMark
      ,FTYMark
      ,Complete
      ,PoRemark
      ,CostRemark
      ,IrregularRemark
      ,FirstPoError
      ,FirstEditName
      ,FirstEditDate
      ,FirstAddDate
      ,FirstCostDate
      ,LastPoError
      ,LastEditName
      ,LastEditDate
      ,LastAddDate
      ,LastCostDate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,MTLDelay
	  ,MinSciDelivery
	  ,ThreadVersion
)
select 
       ID
     , StyleID
      ,SeasonId
      ,StyleUkey
      ,BrandID
      ,POSMR
      ,POHandle
      ,PCHandle
      ,PCSMR
      --,McHandle
      ,ShipMark
      ,FTYMark
      ,Complete
      ,PoRemark
      ,CostRemark
      ,IrregularRemark
      ,FirstPoError
      ,FirstEditName
      ,FirstEditDate
      ,FirstAddDate
      ,FirstCostDate
      ,LastPoError
      ,LastEditName
      ,LastEditDate
      ,LastAddDate
      ,LastCostDate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,MTLDelay	  
	  ,MinSciDelivery   = (select TOP 1 MinSciDelivery from [dbo].[Getsci](b.ID
																	   ,(SELECT Category FROM Orders WHERE ID = b.ID)
																	  )
							)
	  ,ThreadVersion
from #Trade_To_Pms_PO as b WITH (NOLOCK)
where not exists(
select id from Production.dbo.PO as a WITH (NOLOCK) where a.id = b.id )

--PO2

------------------------------------------------------------------PO2 START
----------------------刪除主TABLE多的資料
Delete Production.dbo.PO_Supp
from Production.dbo.PO_Supp as a 
left join Trade_To_Pms.dbo.PO_Supp as b on a.id = b.id and a.SEQ1=b.SEQ1
where b.id is null
--and  a.id in (select id from #Trade_To_Pms_PO)
and exists (select 1 from #TransOrderList where #TransOrderList.POID=a.ID)
and not exists (select 1 from Trade_To_Pms.dbo.PO_Supp_Detail where a.ID = id and a.SEQ1 = Seq1)
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	          =b.ID	
      ,a.SEQ1	      =b.SEQ1	
      ,a.SuppID	      =b.SuppID	
      ,a.Remark	      =b.Remark	
      ,a.Description	      =b.Description	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.PO_Supp as a inner join Trade_To_Pms.dbo.PO_Supp as b ON a.id=b.id and a.SEQ1=b.SEQ1
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID


-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.PO_Supp(
       ID
      ,SEQ1
      ,SuppID
      ,Remark
      ,Description
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       b.ID
      ,SEQ1
      ,SuppID
      ,Remark
      ,Description
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate

from Trade_To_Pms.dbo.PO_Supp as b WITH (NOLOCK) inner join  #Trade_To_Pms_PO c WITH (NOLOCK) ON b.ID = c.ID
where not exists(select id from Production.dbo.PO_Supp as a WITH (NOLOCK) where a.id = b.id and a.SEQ1=b.SEQ1)


------------------------------------------------------------------PO2 END
------------------------------------------------------------------PO3 START
--Po3 pms多的欄位
--,[BrandId] ,[ColorID_Old]
--,[BomFactory]
--      ,[BomCountry]
--      ,[BomStyle]
--      ,[BomCustCD]
--      ,[BomArticle]
--,[BomBuymonth]
--,[StockUnit]
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.FactoryID	      =(select top 1 a.FactoryID from Orders a where a.POID=b.ID)
      ,a.RefNo	      =b.RefNo	
      ,a.SCIRefNo	      =b.SCIRefNo	
      ,a.FabricType	      =b.FabricType	
      ,a.Price	      =b.Price	
      ,a.UsedQty	      =b.UsedQty	
      ,a.Qty	      =b.Qty	
      ,a.POUnit	      =b.POUnit	
      ,a.Complete	      =b.Complete	
      ,a.SystemETD	      =b.SystemETD	
      ,a.CFMETD	      =b.CFMETD	
      ,a.RevisedETA	      =b.RevisedETA	
      ,a.FinalETD	      =b.FinalETD	
      ,a.ShipETA	      =b.ShipETA	
      ,a.ETA	      =b.EstETA	
      ,a.FinalETA	      =b.FinalETA	
      ,a.ShipModeID	      =b.ShipModeID	
      ,a.SystemLock	      =b.SystemLock	
      ,a.PrintDate	      =b.PrintDate	
      ,a.PINO	      =b.PINO	
      ,a.PIDate	      =b.PIDate	
      ,a.SuppColor	      =b.SuppColor	
      ,a.Remark	      =b.Remark	
      ,a.Special	      =b.Special	
      ,a.Width	      =b.Width	
      ,a.StockQty	      =b.StockQty	
      ,a.NetQty	      =b.NetQty	
      ,a.LossQty	      =b.LossQty	
      ,a.SystemNetQty	      =b.SystemNetQty	
      ,a.StockPOID	      =b.StockPOID	
      ,a.StockSeq1	      =b.StockSeq1	
      ,a.StockSeq2	      =b.StockSeq2	
      ,a.InventoryUkey	      =b.InventoryUkey	
      ,a.OutputSeq1	      =b.OutputSeq1	
      ,a.OutputSeq2	      =b.OutputSeq2	
      ,a.SystemCreate	      =b.SystemCreate	
      ,a.FOC	      =b.FOC	
      ,a.Junk	      =b.Junk	
      ,a.ColorDetail	      =b.ColorDetail	
      ,a.ShipQty	      =b.ShipQty	
      ,a.Shortage	      =b.Shortage	
      ,a.ShipFOC	      =b.ShipFOC	
      ,a.ApQty	      =b.ApQty	
      ,a.Spec	      =b.Spec	
      ,a.InputQty	      =b.InputQty	
      ,a.OutputQty	      =b.OutputQty	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
	  ,a.RevisedETD = b.RevisedETD
	  ,a.CfmETA =b.CfmETA
	  ,a.BrandId = (select top 1 a.BrandID from Orders a where a.POID=b.ID)
	  ,a.POAmt			=b.POAmt
	  ,a.ShipAmt		=b.ShipAmt
	  ,a.StockSuppID	=b.StockSuppID
	  ,a.StockOrdersFactory = isnull(b.StockOrdersFactory,'')
    ,a.CopyFromSeq1       = isnull(b.CopyFromSeq1      , '')
    ,a.CopyFromSeq2       = isnull(b.CopyFromSeq2      , '')
    ,a.BomSpecDiffReason  = isnull(b.BomSpecDiffReason , '')
    ,a.CannotOperateStock = isnull(b.CannotOperateStock, 0)
    ,a.Keyword_Original   = isnull(b.Keyword_Original  , '')
from Production.dbo.PO_Supp_Detail as a 
inner join Trade_To_Pms.dbo.PO_Supp_Detail as b ON a.id=b.id and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.PO_Supp_Detail(
ID
      ,Seq1
      ,Seq2
      ,FactoryID
      ,RefNo
      ,SCIRefNo
      ,FabricType
      ,Price
      ,UsedQty
      ,Qty
      ,POUnit
      ,Complete
      ,SystemETD
      ,CFMETD
      ,RevisedETA
      ,FinalETD
      ,ShipETA
      ,ETA
      ,FinalETA
      ,ShipModeID
      ,SystemLock
      ,PrintDate
      ,PINO
      ,PIDate
      ,SuppColor
      ,Remark
      ,Special
      ,Width
      ,StockQty
      ,NetQty
      ,LossQty
      ,SystemNetQty
      ,StockPOID
      ,StockSeq1
      ,StockSeq2
      ,InventoryUkey
      ,OutputSeq1
      ,OutputSeq2
      ,SystemCreate
      ,FOC
      ,Junk
      ,ColorDetail
      ,ShipQty
      ,Shortage
      ,ShipFOC
      ,ApQty
      ,Spec
      ,InputQty
      ,OutputQty
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
	  ,RevisedETD
	  ,CfmETA 
	  ,BrandId
	  ,POAmt
	  ,ShipAmt
	  ,StockSuppID
	  ,StockOrdersFactory
    ,CopyFromSeq1
    ,CopyFromSeq2
    ,BomSpecDiffReason
    ,CannotOperateStock
    ,Keyword_Original
)
select 
       b.ID
      ,Seq1
      ,Seq2
      ,(select top 1 a.FactoryID from Orders a where a.POID=b.ID)
      ,RefNo
      ,SCIRefNo
      ,FabricType
      ,Price
      ,UsedQty
      ,Qty
      ,POUnit
      ,b.Complete
      ,SystemETD
      ,CFMETD
      ,RevisedETA
      ,FinalETD
      ,ShipETA
      ,EstETA
      ,FinalETA
      ,ShipModeID
      ,SystemLock
      ,PrintDate
      ,PINO
      ,PIDate
      ,SuppColor
      ,Remark
      ,Special
      ,Width
      ,StockQty
      ,NetQty
      ,LossQty
      ,SystemNetQty
      ,StockPOID
      ,StockSeq1
      ,StockSeq2
      ,InventoryUkey
      ,OutputSeq1
      ,OutputSeq2
      ,SystemCreate
      ,FOC
      ,Junk
      ,ColorDetail
      ,ShipQty
      ,Shortage
      ,ShipFOC
      ,ApQty
      ,Spec
      ,InputQty
      ,OutputQty
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate
	  ,b.RevisedETD
	  ,b.CfmETA 
	  ,(select top 1 a.BrandID from Orders a WITH (NOLOCK) where a.POID=b.ID)
	  ,b.POAmt
	  ,b.ShipAmt
	  ,b.StockSuppID
	  ,isnull(b.StockOrdersFactory,'')
      ,isnull(b.CopyFromSeq1      , '')
      ,isnull(b.CopyFromSeq2      , '')
      ,isnull(b.BomSpecDiffReason , '')
      ,isnull(b.CannotOperateStock, 0)
      ,isnull(b.Keyword_Original  , '')
from Trade_To_Pms.dbo.PO_Supp_Detail as b WITH (NOLOCK) inner join  #Trade_To_Pms_PO c ON b.ID = c.ID
where not exists(select id from Production.dbo.PO_Supp_Detail as a WITH (NOLOCK) where a.id = b.id and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2	)

----------------------刪除主TABLE多的資料
Delete Production.dbo.PO_Supp_Detail
from Production.dbo.PO_Supp_Detail as a 
left join Trade_To_Pms.dbo.PO_Supp_Detail as b on a.id = b.id and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
left join MDivisionPoDetail c on a.id = c.poid and a.SEQ1=c.Seq1 and a.SEQ2=c.Seq2
where b.id is null and (c.poid is null or c.InQty = 0)
and exists (select 1 from #TransOrderList where #TransOrderList.POID = a.ID)
and a.ShipQty = 0
and not exists(select 1 from Production.dbo.Invtrans i where i.InventoryPOID = a.ID and i.InventorySeq1 = a.Seq1 and InventorySeq2 = a.Seq2 and i.Type = '1')

UPDATE a
SET  
Junk = 1,
QTY = 0
from Production.dbo.PO_Supp_Detail as a 
inner join #Trade_To_Pms_PO as b ON a.id=b.id
where not exists(select id from Trade_To_Pms.dbo.PO_Supp_Detail as c where a.id = c.id)
and InputQty <> 0

----------------------更新 StockUnit
-------需更新的資料 => StockUnit in FtyInventory 
-------比對欄位　　 => Poid, Seq1, Seq2
-------ISP20190607 去除判斷 Exists FtyInventory
update po
set po.StockUnit = Production.dbo.GetStockUnitBySPSeq(po.ID, po.SEQ1, po.SEQ2)
from Production.dbo.PO_Supp_Detail po With(NoLock)
WHERE po.StockUnit = '' OR po.StockUnit IS NULL
------------------------------------------------------------------PO3 END

----PO_Supp_Detail_Spec
UPDATE a
SET  
     [SpecValue] = isnull(b.[SpecValue], '')
    ,[AddName]   = isnull(b.[AddName]  , '')
    ,[AddDate]   = b.[AddDate]
    ,[EditName]  = isnull(b.[EditName] , '')
    ,[EditDate]  = b.[EditDate]

from Production.dbo.PO_Supp_Detail_Spec as a 
inner join Trade_To_Pms.dbo.PO_Supp_Detail_Spec as b ON a.id = b.id and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2 and a.SpecColumnID = b.SpecColumnID
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID 

INSERT INTO Production.dbo.PO_Supp_Detail_Spec
           ([ID]
           ,[Seq1]
           ,[Seq2]
           ,[SpecColumnID]
           ,[SpecValue]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select 
     isnull(b.[ID]          , '')
    ,isnull(b.[Seq1]        , '')
    ,isnull(b.[Seq2]        , '')
    ,isnull(b.[SpecColumnID], '')
    ,isnull(b.[SpecValue], '')
    ,isnull(b.[AddName]  , '')
    ,b.[AddDate]
    ,isnull(b.[EditName] , '')
    ,b.[EditDate]
from Trade_To_Pms.dbo.PO_Supp_Detail_Spec as b WITH (NOLOCK) inner join  #Trade_To_Pms_PO c ON b.ID = c.ID
where not exists(select id from Production.dbo.PO_Supp_Detail_Spec as a WITH (NOLOCK) where a.id = b.id and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2 and a.SpecColumnID = b.SpecColumnID)

--PO_Supp_Detail_Keyword
UPDATE a
SET  
     KeywordValue = isnull(b.KeywordValue, '')
    ,[AddName]   = isnull(b.[AddName]  , '')
    ,[AddDate]   = b.[AddDate]
    ,[EditName]  = isnull(b.[EditName] , '')
    ,[EditDate]  = b.[EditDate]

from Production.dbo.PO_Supp_Detail_Keyword as a 
inner join Trade_To_Pms.dbo.PO_Supp_Detail_Keyword as b ON a.id = b.id and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2 and a.KeywordField = b.KeywordField
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID 

INSERT INTO Production.dbo.PO_Supp_Detail_Keyword
           ([ID]
           ,[Seq1]
           ,[Seq2]
           ,KeywordField
           ,KeywordValue
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select 
     isnull(b.[ID]          , '')
    ,isnull(b.[Seq1]        , '')
    ,isnull(b.[Seq2]        , '')
    ,isnull(b.KeywordField, '')
    ,isnull(b.KeywordValue, '')
    ,isnull(b.[AddName]  , '')
    ,b.[AddDate]
    ,isnull(b.[EditName] , '')
    ,b.[EditDate]
from Trade_To_Pms.dbo.PO_Supp_Detail_Keyword as b WITH (NOLOCK) inner join  #Trade_To_Pms_PO c ON b.ID = c.ID
where not exists(select id from Production.dbo.PO_Supp_Detail_Keyword as a WITH (NOLOCK) where a.id = b.id and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2 and a.KeywordField = b.KeywordField)



------Delete Po from Trade PO_Delete function
-- Create #deletePo3
if exists (select 1 from Trade_To_Pms.dbo.sysobjects where name='PO_Delete')
BEGIN 
select po3.* 
into #deletePo3
from PO_Supp_Detail po3
outer apply(
	select isnull(sum(InQty+OutQty+AdjustQty-ReturnQty),0) ttlQty from FtyInventory 
	where POID=po3.ID and Seq1=po3.SEQ1 and Seq2=po3.SEQ2
	and AdjustQty =0
)fty
outer apply(
	select isnull(sum(InQty+OutQty+AdjustQty-ReturnQty),0) ttlQty from MDivisionPoDetail  
	where POID=po3.ID and Seq1=po3.SEQ1 and Seq2=po3.SEQ2
	and AdjustQty =0
)MDPoDetail
where id in (select POID from Trade_To_Pms.dbo.PO_Delete)
and (MDPoDetail.ttlQty+fty.ttlQty) = 0
and po3.ShipQty  = 0
and not exists(select 1 from Production.dbo.Invtrans i where i.InventoryPOID = po3.ID and i.InventorySeq1 = po3.Seq1 and InventorySeq2 = po3.Seq2 and i.Type = '1')

CREATE CLUSTERED INDEX IDX_PO3_index ON #deletePo3
(
	[id] asc,
	[Seq1] asc,
	[Seq2] asc
)

-- Create temp #PO_Supp_Detail_OrderList
select t.*
into #deletePo_OrderList
from PO_Supp_Detail_OrderList t
inner join #deletePo3 s on t.ID=s.ID
and t.SEQ1=s.SEQ1 and t.SEQ2=s.SEQ2


--Create temp #Po2
select distinct id,SEQ1 
into #deletePo2
from #deletePo3 t
where not exists (
	select 1 from PO_Supp_Detail t1 
	left join #deletePo3 s1 on t1.ID=s1.ID and t1.SEQ1=s1.seq1 and t1.seq2=s1.SEQ2
	where s1.ID is null 
	and t1.ID =t.ID and t1.SEQ1=t.SEQ1 
)

CREATE CLUSTERED INDEX IDX_PO2_index ON #deletePo2
(
	[id] asc,
	[seq1] asc
)

--Create temp #Po
select distinct id
into #deletePo
from #deletePo2 t
where not exists (
	select 1 from PO_Supp t1 	
	left join #deletePo2 s1 on t1.ID=s1.ID and t1.SEQ1=s1.seq1
	where s1.ID is null
	and t1.ID =t.ID
)



delete t
 from Production.dbo.PO t
inner join #deletePo s on t.id=s.id

delete t
from Production.dbo.PO_Supp t
inner join #deletePo2 s on t.id=s.id and t.seq1=s.seq1

delete t
from Production.dbo.PO_Supp_Detail t
inner join #deletePo3 s on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2

delete t
from Production.dbo.PO_Supp_Detail_Spec t
inner join #deletePo3 s on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2

delete t
from Production.dbo.PO_Supp_Detail_Keyword t
inner join #deletePo3 s on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2

delete t
from Production.dbo.PO_Supp_Detail_OrderList t
inner join #deletePo_OrderList s on t.ID=s.id and t.SEQ1=s.seq1 and t.SEQ2=s.seq2

drop table #deletePo,#deletePo2,#deletePo3,#deletePo_OrderList

END

------------------------------------------------------------------PO4 START
--PO4
----------------------刪除主TABLE多的資料
Delete Production.dbo.PO_Supp_Detail_OrderList
from Production.dbo.PO_Supp_Detail_OrderList as a 
left join Trade_To_Pms.dbo.PO_Supp_Detail_OrderList as b on a.id = b.id and a.SEQ1 = b.SEQ1 and a.SEQ2 = b.SEQ2 and a.OrderID = b.OrderID
where b.id is null
and  a.id in (select id from #Trade_To_Pms_PO)
and exists (select 1 from #TransOrderList where #TransOrderList.POID=a.ID)
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       --a.ID	     =b.ID		
      --,a.SEQ1	             =b.SEQ1		
      --,a.SEQ2	      =b.SEQ2		
      --,a.OrderID	      =b.OrderID		
      a.AddName	      =b.AddName		
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =b.EditName		
      ,a.EditDate	      =b.EditDate		

from Production.dbo.PO_Supp_Detail_OrderList as a inner join Trade_To_Pms.dbo.PO_Supp_Detail_OrderList as b 
ON a.id=b.id and a.SEQ1 = b.SEQ1 and a.SEQ2 = b.SEQ2 and a.OrderID=b.OrderID
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID


-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.PO_Supp_Detail_OrderList(
       ID
      ,SEQ1
      ,SEQ2
      ,OrderID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate


)
select 
       b.ID
      ,SEQ1
      ,SEQ2
      ,OrderID
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate


from Trade_To_Pms.dbo.PO_Supp_Detail_OrderList as b WITH (NOLOCK) inner join  #Trade_To_Pms_PO c ON b.ID = c.ID 
where not exists(select id from Production.dbo.PO_Supp_Detail_OrderList as a WITH (NOLOCK) where a.id = b.id and a.SEQ1 = b.SEQ1 and a.SEQ2 = b.SEQ2 and a.OrderID=b.OrderID)

------------------------------------------------------------------PO4 END
------------------------------------------------------

------------最後要清空多的TEMP TABLE
drop table #Trade_To_Pms_PO 
--Fabric
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.BrandID	            = b.BrandID
      , a.Refno	            = b.Refno
      , a.Width	            = b.Width
      , a.Junk	            = b.Junk
      , a.Type	            = b.Type
      , a.MtltypeId	      = b.MtltypeId
      , a.BomTypeCalculate	= b.BomTypeCalculate
      , a.Description	      = b.Description
      , a.DescDetail	      = b.DescDetail
      , a.LossType	      = b.LossType
      , a.LossPercent	      = b.LossPercent
      , a.LossQty	            = b.LossQty
      , a.LossStep	      = b.LossStep
      , a.UsageUnit	      = b.UsageUnit
      , a.Weight	            = b.Weight
      , a.WeightM2	      = b.WeightM2
      , a.CBMWeight	      = b.CBMWeight
      , a.CBM	            = b.CBM
      , a.NoSizeUnit	      = b.NoSizeUnit
      , a.BomTypeSize	      = b.BomTypeSize
      , a.BomTypeColor	      = b.BomTypeColor
      , a.ConstructionID	= b.ConstructionID
      , a.MatchFabric	      = b.MatchFabric
      , a.WeaveTypeID	      = b.WeaveTypeID
      , a.AddName	            = b.AddName
      , a.AddDate	            = b.AddDate
      , a.EditName	      = b.EditName
      , a.EditDate	      = b.EditDate
      , a.preshrink           = b.preshrink
	  , a.DWR = isnull(b.DWR,0)
      , a.RibItem           = b.RibItem 
	  , a.Clima = b.Clima
	  , a.BomTypeCalculateWeight = b.BomTypeCalculateWeight
	  , a.IsRecycled = b.IsRecycled
	  , a.Finish = isnull(b.Finish,'')
	  , a.BrandRefno = isnull(b.BrandRefno,'')
    ,a.[Keyword]                 = isnull(b.[Keyword]                , '')
    ,a.[Textile]                 = isnull(b.[Textile]                , 0)
    ,a.[BomTypeArticle]          = isnull(b.[BomTypeArticle]         , 0)
    ,a.[BomTypeCOO]              = isnull(b.[BomTypeCOO]             , 0)
    ,a.[BomTypeGender]           = isnull(b.[BomTypeGender]          , 0)
    ,a.[BomTypeCustomerSize]     = isnull(b.[BomTypeCustomerSize]    , 0)
    ,a.[BomTypeDecLabelSize]     = isnull(b.[BomTypeDecLabelSize]    , 0)
    ,a.[BomTypeBrandFactoryCode] = isnull(b.[BomTypeBrandFactoryCode], 0)
    ,a.[BomTypeStyle]            = isnull(b.[BomTypeStyle]           , 0)
    ,a.[BomTypeStyleLocation]    = isnull(b.[BomTypeStyleLocation]   , 0)
    ,a.[BomTypeSeason]           = isnull(b.[BomTypeSeason]          , 0)
    ,a.[BomTypeCareCode]         = isnull(b.[BomTypeCareCode]        , 0)
    ,a.[CannotOperateStock]      = isnull(b.[CannotOperateStock]     , 0)
    ,a.IsFOC = isnull(b.IsFOC, 0)
from Production.dbo.Fabric as a 
inner join Trade_To_Pms.dbo.Fabric as b ON a.SCIRefno=b.SCIRefno

INSERT INTO Production.dbo.Fabric(
       SCIRefno
       , BrandID
       , Refno
       , Width
       , Junk
       , Type
       , MtltypeId
       , BomTypeCalculate
       , Description
       , DescDetail
       , LossType
       , LossPercent
       , LossQty
       , LossStep
       , UsageUnit
       , Weight
       , WeightM2
       , CBMWeight
       , CBM
       , NoSizeUnit
       , BomTypeSize
       , BomTypeColor
       , ConstructionID
       , MatchFabric
       , WeaveTypeID
       , AddName
       , AddDate
       , EditName
       , EditDate
       , preshrink
	   , DWR
       , RibItem
	   , Clima
	   , BomTypeCalculateWeight
	   , IsRecycled
	   , Finish
	   , BrandRefno
        ,[Keyword]
        ,[Textile]
        ,[BomTypeArticle]
        ,[BomTypeCOO]
        ,[BomTypeGender]
        ,[BomTypeCustomerSize]
        ,[BomTypeDecLabelSize]
        ,[BomTypeBrandFactoryCode]
        ,[BomTypeStyle]
        ,[BomTypeStyleLocation]
        ,[BomTypeSeason]
        ,[BomTypeCareCode]
        ,[CannotOperateStock]
        ,IsFOC
)
select 
      SCIRefno
      , BrandID
      , Refno
      , Width
      , Junk
      , Type
      , MtltypeId
      , BomTypeCalculate
      , Description
      , DescDetail
      , LossType
      , LossPercent
      , LossQty
      , LossStep
      , UsageUnit
      , Weight
      , WeightM2
      , CBMWeight
      , CBM
      , NoSizeUnit
      , BomTypeSize
      , BomTypeColor
      , ConstructionID
      , MatchFabric
      , WeaveTypeID
      , AddName
      , AddDate
      , EditName
      , EditDate
      , preshrink
	  , isnull(DWR,0)
      , RibItem
	  , Clima
	  , BomTypeCalculateWeight
	  , IsRecycled
	  , isnull(Finish,'')
	  , isnull(BrandRefno,'')
    ,isnull([Keyword]                , '')
    ,isnull([Textile]                , 0)
    ,isnull([BomTypeArticle]         , 0)
    ,isnull([BomTypeCOO]             , 0)
    ,isnull([BomTypeGender]          , 0)
    ,isnull([BomTypeCustomerSize]    , 0)
    ,isnull([BomTypeDecLabelSize]    , 0)
    ,isnull([BomTypeBrandFactoryCode], 0)
    ,isnull([BomTypeStyle]           , 0)
    ,isnull([BomTypeStyleLocation]   , 0)
    ,isnull([BomTypeSeason]          , 0)
    ,isnull([BomTypeCareCode]        , 0)
    ,isnull([CannotOperateStock]     , 0)
    ,isnull(IsFOC, 0)
from Trade_To_Pms.dbo.Fabric as b WITH (NOLOCK)
where not exists(select SCIRefno from Production.dbo.Fabric as a WITH (NOLOCK) where a.SCIRefno = b.SCIRefno)


--Fab_Content
--Fabric_Content

----------------------刪除主TABLE多的資料
Delete Production.dbo.Fabric_Content
from Production.dbo.Fabric_Content as a left join Trade_To_Pms.dbo.Fabric_Content as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.SCIRefno	    =b.SCIRefno	
      --,a.Ukey	      =b.Ukey	
      ,a.Layerno	      =b.Layerno	
      ,a.percentage	      =b.percentage	
      ,a.MtltypeId	      =b.MtltypeId	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.OldSys_GroupKey	      =b.OldSys_GroupKey	

from Production.dbo.Fabric_Content as a inner join Trade_To_Pms.dbo.Fabric_Content as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Fabric_Content(
       SCIRefno
      ,Ukey
      ,Layerno
      ,percentage
      ,MtltypeId
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_GroupKey
)
select 
      SCIRefno
      ,Ukey
      ,Layerno
      ,percentage
      ,MtltypeId
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_GroupKey
from Trade_To_Pms.dbo.Fabric_Content as b WITH (NOLOCK)
where not exists(select Ukey from Production.dbo.Fabric_Content as a WITH (NOLOCK) where a.Ukey = b.Ukey)








--FabricTax
--Fabric_HsCode
----------------------刪除主TABLE多的資料
Delete Production.dbo.Fabric_HsCode
from Production.dbo.Fabric_HsCode as a left join Trade_To_Pms.dbo.Fabric_HsCode as b
on a.SCIRefno = b.SCIRefno and  a.SuppID=b.SuppID and a.Year =b.Year and a.HSType =b.HSType
where b.SCIRefno is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.SCIRefno	     =b.SCIRefno	
      a.Ukey	      =b.Ukey	
      --,a.SuppID	      =b.SuppID	
      --,a.Year	      =b.Year	
      ,a.HsCode	      =b.HsCode	
      ,a.ImportDuty	      =b.ImportDuty	
      ,a.ECFADuty	      =b.ECFADuty	
      ,a.ASEANDuty	      =b.ASEANDuty	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.OldSys_Ukey	      =b.OldSys_Ukey	
      ,a.OldSys_Ver	      =b.OldSys_Ver	
	  ,a.HSCodeT2         =ISNULL(b.HSCodeT2,'')

from Production.dbo.Fabric_HsCode as a inner join Trade_To_Pms.dbo.Fabric_HsCode as b ON a.SCIRefno=b.SCIRefno and  a.SuppID=b.SuppID and a.Year =b.Year and a.HSType =b.HSType
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Fabric_HsCode(
       SCIRefno
      ,Ukey
      ,SuppID
      ,Year
      ,HsCode
      ,ImportDuty
      ,ECFADuty
      ,ASEANDuty
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_Ukey
      ,OldSys_Ver
	  ,HSType	  
	  ,HSCodeT2
)
select
       SCIRefno
      ,Ukey
      ,SuppID
      ,Year
      ,HsCode
      ,ImportDuty
      ,ECFADuty
      ,ASEANDuty
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_Ukey
      ,OldSys_Ver
	  ,HSType
	  ,ISNULL(HSCodeT2,'')
from Trade_To_Pms.dbo.Fabric_HsCode as b WITH (NOLOCK)
where not exists(select SCIRefno from Production.dbo.Fabric_HsCode as a WITH (NOLOCK) where a.SCIRefno = b.SCIRefno and  a.SuppID=b.SuppID and a.Year =b.Year and a.HSType =b.HSType)

--CuttingTape
--CuttingTape_Detail

-------------------------- CuttingTape #tmptable
select b.ID as POID,d.MDivisionID,c.EachConsApv as OldEachcon,b.Seq1,b.Seq2
into #tmpCuttingTape
from Trade_To_Pms.dbo.PO_Supp_Detail as b WITH (NOLOCK)
inner join Trade_To_Pms.dbo.Orders as c WITH (NOLOCK) on b.ID=c.ID
inner join Production.dbo.Factory as d WITH (NOLOCK) on c.FactoryID=d.ID
where b.Seq1 like 'A%'

---------------------------UPDATE CuttingTape
UPDATE a
SET  	
      a.OldEachcon        =b.OldEachcon
from Production.dbo.CuttingTape as a inner join (select DISTINCT #tmpCuttingTape.POID,#tmpCuttingTape.MDivisionID,#tmpCuttingTape.OldEachcon from #tmpCuttingTape ) as b ON a.MDivisionID=b.MDivisionID and a.POID=b.POID 

-------------------------- INSERT INTO CuttingTape
INSERT INTO Production.dbo.CuttingTape(
       POID
      ,MDivisionID
      ,OldEachcon
      ,AddName
      ,AddDate
)
select
       b.POID
      ,b.MDivisionID
      ,b.OldEachcon
      ,'SCIMIS'
      ,GETDATE()
from (select DISTINCT #tmpCuttingTape.POID,#tmpCuttingTape.MDivisionID,#tmpCuttingTape.OldEachcon from #tmpCuttingTape ) as b 
where not exists(select POID from Production.dbo.CuttingTape as a WITH (NOLOCK) where a.MDivisionID = b.MDivisionID and  a.POID=b.POID)

-------------------------- INSERT INTO CuttingTape_Detail
INSERT INTO Production.dbo.CuttingTape_Detail(      
      MDivisionID
	  ,POID
      ,Seq1
      ,Seq2
)
select      
      b.MDivisionID
	  ,b.POID
      ,b.Seq1
      ,b.Seq2
from #tmpCuttingTape as b WITH (NOLOCK) 
where not exists(select POID from Production.dbo.CuttingTape_Detail as a WITH (NOLOCK) where a.MDivisionID = b.MDivisionID and  a.POID=b.POID and a.Seq1=b.Seq1 and a.Seq2=b.Seq2)

drop table #tmpCuttingTape

-------------------------- PadPrintReq

select
	 b.[ID]
	,b.[FactoryID]
	,b.[BrandID]
	,b.[Handle]
	,b.[ReqDate]
	,b.[Status]
	,b.[ApproveName]
	,b.[ApproveDate]
	,b.[Remark]
	,b.[AddName]
	,b.[AddDate]
	,b.[EditName]
	,b.[EditDate]
into #tmpPadPrintReq
from Trade_To_Pms.dbo.PadPrintReq b 
left join Production.dbo.PadPrintReq a on a.ID = b.ID
where exists(select 1 from Production.dbo.Factory where ID = b.FactoryID)
and a.id is null

update a
set
	 [FactoryID]   = b.[FactoryID]
	,[BrandID]	   = b.[BrandID]
	,[Handle]	   = b.[Handle]
	,[ReqDate]	   = b.[ReqDate]
	,[Status]	   = b.[Status]
	,[ApproveName] = b.[ApproveName]
	,[ApproveDate] = b.[ApproveDate]
	,[Remark]	   = b.[Remark]
	,[AddName]	   = b.[AddName]
	,[AddDate]	   = b.[AddDate]
	,[EditName]	   = b.[EditName]
	,[EditDate]	   = b.[EditDate]
from Production.dbo.PadPrintReq a
inner join Trade_To_Pms.dbo.PadPrintReq b on a.ID = b.ID

INSERT INTO [dbo].[PadPrintReq]
           ([ID]
           ,[FactoryID]
           ,[BrandID]
           ,[Handle]
           ,[ReqDate]
           ,[Status]
           ,[ApproveName]
           ,[ApproveDate]
           ,[Remark]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 [ID]
	,[FactoryID]
	,[BrandID]
	,[Handle]
	,[ReqDate]
	,[Status]
	,[ApproveName]
	,[ApproveDate]
	,[Remark]
	,[AddName]
	,[AddDate]
	,[EditName]
	,[EditDate]
from #tmpPadPrintReq

--------------------------PadPrintReq_Detail

update a
set
	 [Refno]      = b.[Refno]
	,[SourceID]	  = b.[SourceID]
	,[Price]	  = b.[Price]
	,[Qty]		  = b.[Qty]
	,[Foc]		  = b.[Foc]
	,[ShipModeID] = b.[ShipModeID]
	,[SuppID]	  = b.[SuppID]
	,[CurrencyID] = b.[CurrencyID]
	,[Junk]		  = b.[Junk]
	,[Remark]	  = b.[Remark]
	,[POID]		  = b.[POID]
	,[AddName]	  = b.[AddName]
	,[AddDate]	  = b.[AddDate]
	,[EditName]	  = b.[EditName]
	,[EditDate]	  = b.[EditDate]
from Production.dbo.PadPrintReq_Detail a
inner join Trade_To_Pms.dbo.PadPrintReq_Detail b on a.ID = b.ID and a.Seq2 = b.Seq2 and a.PadPrint_Ukey = b.PadPrint_Ukey and a.MoldID = b.MoldID

delete a
from Production.dbo.PadPrintReq_Detail a
left join Trade_To_Pms.dbo.PadPrintReq_Detail b on a.ID = b.ID and a.Seq2 = b.Seq2 and a.PadPrint_Ukey = b.PadPrint_Ukey and a.MoldID = b.MoldID
where exists(select 1 from #tmpPadPrintReq where id = a.id)
and b.id is null

INSERT INTO [dbo].[PadPrintReq_Detail]
           ([ID]
           ,[Seq2]
           ,[PadPrint_Ukey]
           ,[Refno]
           ,[MoldID]
           ,[SourceID]
           ,[Price]
           ,[Qty]
           ,[Foc]
           ,[ShipModeID]
           ,[SuppID]
           ,[CurrencyID]
           ,[Junk]
           ,[Remark]
           ,[POID]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 b.[ID]
	,b.[Seq2]
	,b.[PadPrint_Ukey]
	,b.[Refno]
	,b.[MoldID]
	,b.[SourceID]
	,b.[Price]
	,b.[Qty]
	,b.[Foc]
	,b.[ShipModeID]
	,b.[SuppID]
	,b.[CurrencyID]
	,b.[Junk]
	,b.[Remark]
	,b.[POID]
	,b.[AddName]
	,b.[AddDate]
	,b.[EditName]
	,b.[EditDate]
from #tmpPadPrintReq a
inner join Trade_To_Pms.dbo.PadPrintReq_Detail b on a.ID = b.ID
where not exists(select 1 from Production.dbo.PadPrintReq_Detail p where p.id = b.id and p.seq2 = b.seq2 and p.[PadPrint_Ukey] = b.[PadPrint_Ukey] and p.[MoldID] = b.[MoldID])


--------------------------PadPrintReq_Detail_spec

update a
set
	 [AddName]		 = b.[AddName]
	,[AddDate]		 = b.[AddDate]
	,[EditName]		 = b.[EditName]
	,[EditDate]		 = b.[EditDate]
from Production.dbo.PadPrintReq_Detail_spec a
inner join Trade_To_Pms.dbo.PadPrintReq_Detail_spec b on a.ID = b.ID and a.Seq2 = b.Seq2 and a.PadPrint_Ukey = b.PadPrint_Ukey and a.MoldID = b.MoldID and a.Side = b.Side

delete a
from Production.dbo.PadPrintReq_Detail_spec a
left join Trade_To_Pms.dbo.PadPrintReq_Detail_spec b on a.ID = b.ID and a.Seq2 = b.Seq2 and a.PadPrint_Ukey = b.PadPrint_Ukey and a.MoldID = b.MoldID and a.Side = b.Side
where exists(select 1 from #tmpPadPrintReq where id = a.id)
and b.id is null

INSERT INTO [dbo].PadPrintReq_Detail_spec
           ([ID]
           ,[Seq2]
           ,[PadPrint_Ukey]
           ,[MoldID]
           ,[Side]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	b.[ID]
	,b.[Seq2]
	,b.[PadPrint_Ukey]
	,b.[MoldID]
	,b.[Side]
	,b.[AddName]
	,b.[AddDate]
	,b.[EditName]
	,b.[EditDate]
from #tmpPadPrintReq a
inner join Trade_To_Pms.dbo.PadPrintReq_Detail_spec b on a.ID = b.ID
where not exists(select 1 from  Production.dbo.PadPrintReq_Detail_spec p where p.id = b.id and p.seq2 = b.seq2 and p.[PadPrint_Ukey] = b.[PadPrint_Ukey] and p.[MoldID] = b.[MoldID] and p.[Side] = b.[Side])


drop table #tmpPadPrintReq
--------------------------

END

