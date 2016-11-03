
-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE imp_Prokit
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--	StyleC
--[Style_ProductionKits]
--PMS2多的,[SendToQA]
--      ,[QAReceived]
--,[MDivisionID]

----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_ProductionKits
from Production.dbo.Style_ProductionKits as a left join Trade_To_Pms.dbo.Style_ProductionKits as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.Ukey	 =b.Ukey
      a.StyleUkey	      =b.StyleUkey
      ,a.ProductionKitsGroup	      =b.ProductionKitsGroup
      ,a.FactoryID	      =b.FactoryID
      ,a.Article	      =b.Article
      ,a.DOC	      =b.DOC
      ,a.SendDate	      =b.SendDate
      --,a.ReceiveDate	      =b.ReceiveDate
      ,a.ProvideDate	      =b.ProvideDate
      ,a.SendName	      =b.SendName
      --,a.FtyHandle	      =b.FtyHandle
      ,a.MRHandle	      =b.MRHandle
      ,a.SMR	      =b.SMR
      ,a.PoHandle	      =b.PoHandle
      ,a.POSMR	      =b.POSMR
      ,a.OrderId	      =b.OrderId
      ,a.SCIDelivery	      =b.SCIDelivery
      ,a.IsPF	      =b.IsPF
      ,a.BuyerDelivery	      =b.BuyerDelivery
      ,a.AddOrderId	      =b.AddOrderId
      ,a.AddSCIDelivery	      =b.AddSCIDelivery
      ,a.AddIsPF	      =b.AddIsPF
      ,a.AddBuyerDelivery	      =b.AddBuyerDelivery
      ,a.MRLastDate	      =b.MRLastDate
      --,a.FtyLastDate	      =b.FtyLastDate
      ,a.MRRemark	      =b.MRRemark
      --,a.FtyRemark	      =b.FtyRemark
      ,a.FtyList	      =b.FtyList
      ,a.Reasonid	      =b.Reasonid
      ,a.StyleCUkey1_Old	      =b.StyleCUkey1_Old
from Production.dbo.Style_ProductionKits as a inner join Trade_To_Pms.dbo.Style_ProductionKits as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_ProductionKits(
       Ukey
      ,StyleUkey
      ,ProductionKitsGroup
      ,FactoryID
      ,Article
      ,DOC
      ,SendDate
      --,ReceiveDate
      ,ProvideDate
      ,SendName
      --,FtyHandle
      ,MRHandle
      ,SMR
      ,PoHandle
      ,POSMR
      ,OrderId
      ,SCIDelivery
      ,IsPF
      ,BuyerDelivery
      ,AddOrderId
      ,AddSCIDelivery
      ,AddIsPF
      ,AddBuyerDelivery
      ,MRLastDate
      --,FtyLastDate
      ,MRRemark
      --,FtyRemark
      ,FtyList
      ,Reasonid
      ,StyleCUkey1_Old

)
select 
      Ukey
      ,StyleUkey
      ,ProductionKitsGroup
      ,FactoryID
      ,Article
      ,DOC
      ,SendDate
     -- ,ReceiveDate
      ,ProvideDate
      ,SendName
      --,FtyHandle
      ,MRHandle
      ,SMR
      ,PoHandle
      ,POSMR
      ,OrderId
      ,SCIDelivery
      ,IsPF
      ,BuyerDelivery
      ,AddOrderId
      ,AddSCIDelivery
      ,AddIsPF
      ,AddBuyerDelivery
      ,MRLastDate
      --,FtyLastDate
      ,MRRemark
      --,FtyRemark
      ,FtyList
      ,Reasonid
      ,StyleCUkey1_Old

from Trade_To_Pms.dbo.Style_ProductionKits as b
where not exists(select Ukey from Production.dbo.Style_ProductionKits as a where a.Ukey = b.Ukey)




  
END

