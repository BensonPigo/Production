
Create PROCEDURE imp_Po
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

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
       a.ID	    = isnull(b.ID	                             ,'')
      ,a.StyleID	      = isnull(b.StyleID	             ,'')
      ,a.SeasonId	      = isnull(b.SeasonId	             ,'')
      ,a.StyleUkey	      = isnull(b.StyleUkey	             ,0)
      ,a.BrandID	      = isnull(b.BrandID	             ,'')
      ,a.POSMR	      = isnull(b.POSMR	                     ,'')
      ,a.POHandle	      = isnull(b.POHandle	             ,'')
      ,a.PCHandle	      = isnull(b.PCHandle	             ,'')
      ,a.PCSMR	      = isnull(b.PCSMR	                     ,'')
      --,a.McHandle	      = isnull(b.McHandle	             ,'')
      ,a.ShipMark	      = isnull(b.ShipMark	             ,'')
      ,a.FTYMark	      = isnull(b.FTYMark	             ,'')
      ,a.Complete	      = isnull(b.Complete	             ,0)
      ,a.PoRemark	      = isnull(b.PoRemark	             ,'')
      ,a.CostRemark	      = isnull(b.CostRemark	             ,'')
      ,a.IrregularRemark	      = isnull(b.IrregularRemark,'')
      ,a.FirstPoError	      = isnull(b.FirstPoError	     ,'')
      ,a.FirstEditName	      = isnull(b.FirstEditName	     ,'')
      ,a.FirstEditDate	      = b.FirstEditDate
      ,a.FirstAddDate	      = b.FirstAddDate
      ,a.FirstCostDate	      = b.FirstCostDate
      ,a.LastPoError	      = isnull(b.LastPoError	     ,'')
      ,a.LastEditName	      = isnull(b.LastEditName	     ,'')
      ,a.LastEditDate	      = b.LastEditDate
      ,a.LastAddDate	      = b.LastAddDate
      ,a.LastCostDate	      = b.LastCostDate
      ,a.AddName	      = isnull(b.AddName	             ,'')
      ,a.AddDate	      = b.AddDate
      ,a.EditName	      = isnull(b.EditName	             ,'')
      ,a.EditDate	      = b.EditDate
      ,a.MTLDelay	      = b.MtlDelay
	  ,a.MinSciDelivery   = (select TOP 1 MinSciDelivery from [dbo].[Getsci](a.ID
																	   ,(SELECT Category FROM Orders WHERE ID = a.ID)
																	  )
							)
	  ,a.ThreadVersion    = isnull(b.ThreadVersion,'')
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
       isnull(ID                ,'')
     , isnull(StyleID           ,'')
      ,isnull(SeasonId          ,'')
      ,isnull(StyleUkey         ,0)
      ,isnull(BrandID           ,'')
      ,isnull(POSMR             ,'')
      ,isnull(POHandle          ,'')
      ,isnull(PCHandle          ,'')
      ,isnull(PCSMR             ,'')
      ,isnull(ShipMark          ,'')
      ,isnull(FTYMark           ,'')
      ,isnull(Complete          ,0)
      ,isnull(PoRemark          ,'')
      ,isnull(CostRemark        ,'')
      ,isnull(IrregularRemark   ,'')
      ,isnull(FirstPoError      ,'')
      ,isnull(FirstEditName     ,'')
      ,FirstEditDate
      ,FirstAddDate
      ,FirstCostDate
      ,isnull(LastPoError       ,'')
      ,isnull(LastEditName      ,'')
      ,LastEditDate
      ,LastAddDate 
      ,LastCostDate
      ,isnull(AddName           ,'')
      ,AddDate
      ,isnull(EditName          ,'')
      ,EditDate
      ,MTLDelay
	  ,MinSciDelivery   = (select TOP 1 MinSciDelivery from [dbo].[Getsci](b.ID
																	   ,(SELECT Category FROM Orders WHERE ID = b.ID)
																	  )
							)
	  ,isnull(ThreadVersion,'')
from #Trade_To_Pms_PO as b WITH (NOLOCK)
where not exists(
select id from Production.dbo.PO as a WITH (NOLOCK) where a.id = b.id )


------------------------------------------------------------------PO3 START
--Po3 pms多的欄位
--,[BrandId]
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
      a.FactoryID = ISNULL((SELECT TOP 1 a.FactoryID FROM Orders a WHERE a.POID = b.ID), ''),
      a.RefNo = ISNULL(b.RefNo, ''),
      a.SCIRefNo = ISNULL(b.SCIRefNo, ''),
      a.FabricType = ISNULL(b.FabricType, ''),
      a.Price = ISNULL(b.Price, 0),
      a.UsedQty = ISNULL(b.UsedQty, 0),
      a.Qty = ISNULL(b.Qty, 0),
      a.POUnit = ISNULL(b.POUnit, ''),
      a.Complete = ISNULL(b.Complete, 0),
      a.SystemETD = b.SystemETD,
      a.CFMETD = b.CFMETD,
      a.RevisedETA = b.RevisedETA,
      a.FinalETD = b.FinalETD,
      a.ShipETA = b.ShipETA,
      a.ETA = b.EstETA,
      a.FinalETA = b.FinalETA,
      a.ShipModeID = ISNULL(b.ShipModeID, ''),
      a.SystemLock = b.SystemLock,
      a.PrintDate = b.PrintDate,
      a.PINO = ISNULL(b.PINO, ''),
      a.PIDate = b.PIDate,
      a.SuppColor = ISNULL(b.SuppColor, ''),
      a.Remark = ISNULL(b.Remark, ''),
      a.Special = ISNULL(b.Special, ''),
      a.Width = ISNULL(b.Width, 0),
      a.StockQty = ISNULL(b.StockQty, 0),
      a.NetQty = ISNULL(b.NetQty, 0),
      a.LossQty = ISNULL(b.LossQty, 0),
      a.SystemNetQty = ISNULL(b.SystemNetQty, 0),
      a.StockPOID = ISNULL(b.StockPOID, ''),
      a.StockSeq1 = ISNULL(b.StockSeq1, ''),
      a.StockSeq2 = ISNULL(b.StockSeq2, ''),
      a.InventoryUkey = ISNULL(b.InventoryUkey, 0),
      a.OutputSeq1 = ISNULL(b.OutputSeq1, ''),
      a.OutputSeq2 = ISNULL(b.OutputSeq2, ''),
      a.SystemCreate = ISNULL(b.SystemCreate, 0),
      a.FOC = ISNULL(b.FOC, 0),
      a.Junk = ISNULL(b.Junk, 0),
      a.ColorDetail = ISNULL(b.ColorDetail, ''),
      a.ShipQty = ISNULL(b.ShipQty, 0),
      a.Shortage = ISNULL(b.Shortage, 0),
      a.ShipFOC = ISNULL(b.ShipFOC, 0),
      a.ApQty = ISNULL(b.ApQty, 0),
      a.Spec = ISNULL(b.Spec, ''),
      a.InputQty = ISNULL(b.InputQty, 0),
      a.OutputQty = ISNULL(b.OutputQty, 0),
      a.AddName = ISNULL(b.AddName, ''),
      a.AddDate = b.AddDate,
      a.EditName = ISNULL(b.EditName, ''),
      a.EditDate = b.EditDate,
      a.RevisedETD = b.RevisedETD,
      a.CfmETA = b.CfmETA
	  ,a.BrandId = isnull((select top 1 a.BrandID from Orders a where a.POID=b.ID),'')
	  ,a.POAmt			= isnull(b.POAmt      ,0)
	  ,a.ShipAmt		= isnull(b.ShipAmt    ,0)
	  ,a.StockSuppID	= isnull(b.StockSuppID,'')
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
       isnull(b.ID,'')
      ,isnull(Seq1,'')
      ,isnull(Seq2,'')
      ,isnull((select top 1 a.FactoryID from Orders a where a.POID=b.ID),'')
      ,isnull(RefNo            ,'')
      ,isnull(SCIRefNo         ,'')
      ,isnull(FabricType       ,'')
      ,isnull(Price            ,0)
      ,isnull(UsedQty          ,0)
      ,isnull(Qty              ,0)
      ,isnull(POUnit           ,'')
      ,isnull(b.Complete       ,0)
      ,SystemETD 
      ,CFMETD    
      ,RevisedETA
      ,FinalETD  
      ,ShipETA   
      ,EstETA    
      ,FinalETA  
      ,isnull(ShipModeID       ,'')
      ,SystemLock
      ,PrintDate
      ,isnull(PINO             ,'')
      ,PIDate
      ,isnull(SuppColor    ,'')
      ,isnull(Remark       ,'')
      ,isnull(Special      ,'')
      ,isnull(Width        ,0)
      ,isnull(StockQty     ,0)
      ,isnull(NetQty       ,0)
      ,isnull(LossQty      ,0)
      ,isnull(SystemNetQty ,0)
      ,isnull(StockPOID    ,'')
      ,isnull(StockSeq1    ,'')
      ,isnull(StockSeq2    ,'')
      ,isnull(InventoryUkey,0)
      ,isnull(OutputSeq1   ,'')
      ,isnull(OutputSeq2   ,'')
      ,isnull(SystemCreate ,0)
      ,isnull(FOC          ,0)
      ,isnull(Junk         ,0)
      ,isnull(ColorDetail  ,'')
      ,isnull(ShipQty      ,0)
      ,isnull(Shortage     ,0)
      ,isnull(ShipFOC      ,0)
      ,isnull(ApQty        ,0)
      ,isnull(Spec         ,'')
      ,isnull(InputQty     ,0)
      ,isnull(OutputQty    ,0)
      ,isnull(b.AddName    ,'')
      ,b.AddDate
      ,isnull(b.EditName,'')
      ,b.EditDate
	  ,b.RevisedETD
	  ,b.CfmETA 
	  ,isnull((select top 1 a.BrandID from Orders a WITH (NOLOCK) where a.POID=b.ID),'')
	  ,isnull(b.POAmt      ,0)
	  ,isnull(b.ShipAmt    ,0)
	  ,isnull(b.StockSuppID,'')
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
and not exists (select 1 from Production.dbo.TransferExport_Detail ted where a.id = ted.poid and a.Seq1 = ted.Seq1 and a.Seq2 = ted.Seq2)

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
set po.StockUnit =isnull( Production.dbo.GetStockUnitBySPSeq(po.ID, po.SEQ1, po.SEQ2),'')
from Production.dbo.PO_Supp_Detail po With(NoLock)
WHERE po.StockUnit = '' OR po.StockUnit IS NULL
------------------------------------------------------------------PO3 END

------------------------------------------------------------------PO2 START
----------------------刪除主TABLE多的資料
Delete Production.dbo.PO_Supp
from Production.dbo.PO_Supp as a 
left join Trade_To_Pms.dbo.PO_Supp as b on a.id = b.id and a.SEQ1=b.SEQ1
where b.id is null
--and  a.id in (select id from #Trade_To_Pms_PO)
and exists (select 1 from #TransOrderList where #TransOrderList.POID=a.ID)
and not exists (select 1 from Trade_To_Pms.dbo.PO_Supp_Detail where a.ID = id and a.SEQ1 = Seq1)
and not exists (select 1 from Production.dbo.PO_Supp_Detail where a.ID = id and a.SEQ1 = Seq1 and ShipQty > 0) ---- 在PMS系統當中若有下一層的庫存數則不刪除
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	          = isnull(b.ID	    ,'')
      ,a.SEQ1	      = isnull(b.SEQ1	,'')
      ,a.SuppID	      = isnull(b.SuppID	,'')
      ,a.Remark	      = isnull(b.Remark	,'')
      ,a.Description	      = isnull(b.Description	,'')
      ,a.AddName	      = isnull(b.AddName	,'')
      ,a.AddDate	      = b.AddDate	
      ,a.EditName	      = isnull(b.EditName	,'')
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
       isnull(b.ID       ,'')
      ,isnull(SEQ1       ,'')
      ,isnull(SuppID     ,'')
      ,isnull(Remark     ,'')
      ,isnull(Description,'')
      ,isnull(b.AddName,'')
      ,b.AddDate
      ,isnull(b.EditName,'')
      ,b.EditDate

from Trade_To_Pms.dbo.PO_Supp as b WITH (NOLOCK) inner join  #Trade_To_Pms_PO c WITH (NOLOCK) ON b.ID = c.ID
where not exists(select id from Production.dbo.PO_Supp as a WITH (NOLOCK) where a.id = b.id and a.SEQ1=b.SEQ1)

------------------------------------------------------------------PO2 END

----PO_Supp_Detail_Spec
Delete a 
from Production.dbo.PO_Supp_Detail_Spec as a 
where exists (select 1 from Trade_To_Pms.dbo.PO_Supp_Detail b where a.ID = b.ID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2)
and not exists(select id from Trade_To_Pms.dbo.PO_Supp_Detail_Spec as b where a.id = b.id and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2 and a.SpecColumnID = b.SpecColumnID)

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
from Trade_To_Pms.dbo.PO_Supp_Detail_Spec as b WITH (NOLOCK) 
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID
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
and not exists (select 1 from Production.dbo.TransferExport_Detail ted where po3.id = ted.poid and po3.Seq1 = ted.Seq1 and po3.Seq2 = ted.Seq2)

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
      a.AddName	      = isnull(b.AddName,'')
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =isnull(b.EditName,'')
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
       isnull(b.ID     ,'')
      ,isnull(SEQ1     ,'')
      ,isnull(SEQ2     ,'')
      ,isnull(OrderID  ,'')
      ,isnull(b.AddName,'')
      ,b.AddDate
      ,isnull(b.EditName,'')
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
    a.BrandID	            = isnull( b.BrandID           ,'')
    , a.Refno	            = isnull( b.Refno             ,'')
    , a.Width	            = isnull( b.Width             ,0)
    , a.Junk	            = isnull( b.Junk              ,0)
    , a.Type	            = isnull( b.Type              ,'')
    , a.MtltypeId	      = isnull( b.MtltypeId           ,'')
    , a.BomTypeCalculate	= isnull( b.BomTypeCalculate  ,0)
    , a.Description	      = isnull( b.Description     ,'')
    , a.DescDetail	      = isnull( b.DescDetail      ,'')
    , a.LossType	      = isnull( b.LossType            ,0)
    , a.LossPercent	      = isnull( b.LossPercent     ,0)
    , a.LossQty	            = isnull( b.LossQty       ,0)
    , a.LossStep	      = isnull( b.LossStep            ,0)
    , a.UsageUnit	      = isnull( b.UsageUnit           ,'')
    , a.Weight	            = isnull( b.Weight        ,0)
    , a.WeightM2	      = isnull( b.WeightM2            ,0)
    , a.CBMWeight	      = isnull( b.CBMWeight           ,0)
    , a.CBM	            = isnull( b.CBM               ,0)
    , a.NoSizeUnit	      = isnull( b.NoSizeUnit      ,0)
    , a.BomTypeSize	      = isnull( b.BomTypeSize     ,0)
    , a.BomTypeColor	      = isnull( b.BomTypeColor    ,0)
    , a.ConstructionID	= isnull( b.ConstructionID    ,'')
    , a.MatchFabric	      = isnull( b.MatchFabric     ,'')
    , a.WeaveTypeID	      = isnull( b.WeaveTypeID     ,'')
    , a.AddName	            = isnull( b.AddName       ,'')
    , a.AddDate	            =  b.AddDate
    , a.EditName	      = isnull( b.EditName            ,'')
    , a.EditDate	      =  b.EditDate 
    , a.preshrink           = isnull( b.preshrink       ,0)
	, a.DWR = isnull(b.DWR,0)
    , a.RibItem           = isnull( b.RibItem ,0)
	, a.Clima = isnull( b.Clima,0)
	, a.BomTypeCalculateWeight = isnull( b.BomTypeCalculateWeight,0)
	, a.IsRecycled = isnull( b.IsRecycled,0)
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
	,a.Picture = ISNULL(b.Picture,'')
    , a.InspectionGroup = isnull(b.InspectionGroup,'')
    , a.BomTypeBuyMonth = isnull(b.BomTypeBuyMonth,0)
    , a.BomTypeBuyerDlvMonth = isnull(b.BomTypeBuyerDlvMonth,0)
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
		,Picture
       ,InspectionGroup
       ,BomTypeBuyMonth
       ,BomTypeBuyerDlvMonth
)
select 
        isnull(SCIRefno          ,'')
      , isnull(BrandID           ,'')
      , isnull(Refno             ,'')
      , isnull(Width             ,0)
      , isnull(Junk              ,0)
      , isnull(Type              ,'')
      , isnull(MtltypeId         ,'')
      , isnull(BomTypeCalculate  ,0)
      , isnull(Description       ,'')
      , isnull(DescDetail        ,'')
      , isnull(LossType          ,0)
      , isnull(LossPercent       ,0)
      , isnull(LossQty           ,0)
      , isnull(LossStep          ,0)
      , isnull(UsageUnit         ,'')
      , isnull(Weight            ,0)
      , isnull(WeightM2          ,0)
      , isnull(CBMWeight         ,0)
      , isnull(CBM               ,0)
      , isnull(NoSizeUnit        ,0)
      , isnull(BomTypeSize       ,0)
      , isnull(BomTypeColor      ,0)
      , isnull(ConstructionID    ,'')
      , isnull(MatchFabric       ,'')
      , isnull(WeaveTypeID       ,'')
      , isnull(AddName           ,'')
      , AddDate
      , isnull(EditName          ,'')
      , EditDate
      , isnull(preshrink         ,0)
	  , isnull(DWR,0)
      , isnull(RibItem,0)
	  , isnull(Clima  ,0)
	  , isnull(BomTypeCalculateWeight,0)
	  , isnull(IsRecycled,0)
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
	,ISNULL(Picture,'')
    , isnull(InspectionGroup,'')
    , isnull(BomTypeBuyMonth,0)
    , isnull(BomTypeBuyerDlvMonth,0)
from Trade_To_Pms.dbo.Fabric as b WITH (NOLOCK)
where not exists(select SCIRefno from Production.dbo.Fabric as a WITH (NOLOCK) where a.SCIRefno = b.SCIRefno)

-- ManufacturingExecution.RefnoRelaxtime
insert into [dbo].[SciMES_RefnoRelaxtime] (Refno, AddName, AddDate)
select a.Refno,'SCIMIS',GETDATE()
from (
	select distinct Refno 
	from Production.dbo.Fabric f
	where f.Type = 'F'
) a
left join [dbo].[SciMES_RefnoRelaxtime] b WITH (NOLOCK) on b.Refno = a.Refno
where b.Refno is null

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
       a.SCIRefno	    = isnull(b.SCIRefno	,'')
      ,a.Layerno	      = isnull(b.Layerno	,0)
      ,a.percentage	      = isnull(b.percentage	,0)
      ,a.MtltypeId	      = isnull(b.MtltypeId	,'')
      ,a.AddName	      = isnull(b.AddName	,'')
      ,a.AddDate	      = b.AddDate	
      ,a.EditName	      = isnull(b.EditName	,'')
      ,a.EditDate	      = b.EditDate	

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
)
select 
       isnull(SCIRefno       ,'')
      ,isnull(Ukey           ,0)
      ,isnull(Layerno        ,0)
      ,isnull(percentage     ,0)
      ,isnull(MtltypeId      ,'')
      ,isnull(AddName        ,'')
      ,AddDate
      ,isnull(EditName       ,'')
      ,EditDate
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
      a.Ukey	      = isnull(b.Ukey	            ,0)
      ,a.HsCode	      = isnull(b.HsCode	            ,'')
      ,a.ImportDuty	      = isnull(b.ImportDuty	    ,0)
      ,a.ECFADuty	      = isnull(b.ECFADuty	    ,0)
      ,a.ASEANDuty	      = isnull(b.ASEANDuty	    ,0)
      ,a.AddName	      = isnull(b.AddName	    ,'')
      ,a.AddDate	      = b.AddDate
      ,a.EditName	      = isnull(b.EditName	    ,'')
      ,a.EditDate	      = b.EditDate
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
	  ,HSType	  
	  ,HSCodeT2
)
select
       isnull(SCIRefno   ,'')
      ,isnull(Ukey       ,0)
      ,isnull(SuppID     ,'')
      ,isnull(Year       ,0)
      ,isnull(HsCode     ,'')
      ,isnull(ImportDuty ,0)
      ,isnull(ECFADuty   ,0)
      ,isnull(ASEANDuty  ,0)
      ,isnull(AddName    ,'')
      ,AddDate
      ,isnull(EditName   ,'')
      ,EditDate
	  ,isnull(HSType     ,'')
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
       isnull(b.MDivisionID,'')
	  ,isnull(b.POID       ,'')
      ,isnull(b.Seq1       ,'')
      ,isnull(b.Seq2       ,'')
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
	 [FactoryID]   = isnull( b.[FactoryID]  ,'')
	,[BrandID]	   = isnull( b.[BrandID]    ,'')
	,[Handle]	   = isnull( b.[Handle]     ,'')
	,[ReqDate]	   =  b.[ReqDate]
	,[Status]	   = isnull( b.[Status]     ,'')
	,[ApproveName] = isnull( b.[ApproveName],'')
	,[ApproveDate] =  b.[ApproveDate]
	,[Remark]	   = isnull( b.[Remark]     ,'')
	,[AddName]	   = isnull( b.[AddName]    ,'')
	,[AddDate]	   =  b.[AddDate]
	,[EditName]	   = isnull( b.[EditName]   ,'')
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
	 [Refno]      = isnull( b.[Refno]      ,'')
	,[SourceID]	  = isnull( b.[SourceID]   ,'')
	,[Price]	  = isnull( b.[Price]      ,0)
	,[Qty]		  = isnull( b.[Qty]        ,0)
	,[Foc]		  = isnull( b.[Foc]        ,0)
	,[ShipModeID] = isnull( b.[ShipModeID] ,'')
	,[SuppID]	  = isnull( b.[SuppID]     ,'')
	,[CurrencyID] = isnull( b.[CurrencyID] ,0)
	,[Junk]		  = isnull( b.[Junk]       ,0)
	,[Remark]	  = isnull( b.[Remark]     ,'')
	,[POID]		  = isnull( b.[POID]       ,'')
	,[AddName]	  = isnull( b.[AddName]    ,'')
	,[AddDate]	  =  b.[AddDate]
	,[EditName]	  = isnull( b.[EditName]   ,'')
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
         isnull(b.[ID]           ,'')
        ,isnull(b.[Seq2]         ,'')
        ,isnull(b.[PadPrint_Ukey],0)
        ,isnull(b.[Refno]        ,'')
        ,isnull(b.[MoldID]       ,'')
        ,isnull(b.[SourceID]     ,'')
        ,isnull(b.[Price]        ,0)
        ,isnull(b.[Qty]          ,0)
        ,isnull(b.[Foc]          ,0)
        ,isnull(b.[ShipModeID]   ,'')
        ,isnull(b.[SuppID]       ,'')
        ,isnull(b.[CurrencyID]   ,'')
        ,isnull(b.[Junk]         ,0)
        ,isnull(b.[Remark]       ,'')
        ,isnull(b.[POID]         ,'')
        ,isnull(b.[AddName]      ,'')
        ,b.[AddDate]
        ,isnull(b.[EditName]     ,'')
        ,b.[EditDate]
from #tmpPadPrintReq a
inner join Trade_To_Pms.dbo.PadPrintReq_Detail b on a.ID = b.ID
where not exists(select 1 from Production.dbo.PadPrintReq_Detail p where p.id = b.id and p.seq2 = b.seq2 and p.[PadPrint_Ukey] = b.[PadPrint_Ukey] and p.[MoldID] = b.[MoldID])


--------------------------PadPrintReq_Detail_spec

update a
set
	 [AddName]		 = isnull(b.[AddName],'')
	,[AddDate]		 = b.[AddDate]
	,[EditName]		 = isnull(b.[EditName],'')
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
	 isnull(b.[ID]            ,'')
	,isnull(b.[Seq2]          ,'')
	,isnull(b.[PadPrint_Ukey] ,0)
	,isnull(b.[MoldID]        ,'')
	,isnull(b.[Side]          ,'')
	,isnull(b.[AddName]       ,'')
	,b.[AddDate]
	,isnull(b.[EditName]      ,'')
	,b.[EditDate]
from #tmpPadPrintReq a
inner join Trade_To_Pms.dbo.PadPrintReq_Detail_spec b on a.ID = b.ID
where not exists(select 1 from  Production.dbo.PadPrintReq_Detail_spec p where p.id = b.id and p.seq2 = b.seq2 and p.[PadPrint_Ukey] = b.[PadPrint_Ukey] and p.[MoldID] = b.[MoldID] and p.[Side] = b.[Side])


drop table #tmpPadPrintReq
--------------------------

/*********************Clip*********************/
--------DELETE
DELETE Production.dbo.Clip
from Production.dbo.Clip pc
LEFT JOIN Trade_To_Pms.dbo.Clip tc on  tc.PKEY = pc.PKey AND tc.UniqueKey = pc.UniqueKey
WHERE pc.TableName = 'PoItem' and tc.PKey is null
--------UPDATE
UPDATE pc
SET
 pc.[TableName] = ISNULL(tc.[TableName],'')
,pc.[SourceFile] = isnull(tc.[SourceFile],'')
,pc.[Description] = ISNULL(tc.[Description],'')
from Production.dbo.Clip pc
INNER JOIN Trade_To_Pms.dbo.Clip tc on  tc.PKEY = pc.PKey AND tc.UniqueKey = pc.UniqueKey AND tc.TableName = 'PoItem'
--------INSERT
INSERT INTO Production.dbo.Clip
(
PKey
,TableName
,UniqueKey
,SourceFile
,[Description]
,AddName
,AddDate
)
select 
c.[Pkey]
,[TableName] = isnull(c.[TableName],'')
,[UniqueKey] = isnull(c.[UniqueKey],'')
,[SourceFile] = isnull(c.[SourceFile],'')
,[Description] = isnull(c.[Description],'')
,[AddName] = isnull(c.[AddName],'')
,[AddDate] = c.AddDate
from Trade_To_Pms.dbo.Clip c
inner join Production.dbo.PO_Supp ps on c.UniqueKey = ps.ID+seq1
where c.TableName = 'PoItem' AND 
NOT EXISTS(SELECT 1 FROM Production.dbo.Clip pc WHERE c.UniqueKey = pc.UniqueKey and c.pkey = pc.PKey)
/***********************************************/
END

