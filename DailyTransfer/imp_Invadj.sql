
-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Invadj]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

--  Invadj1    invadjust
--PMS¦h,[OrderShipmodeSeq]
--      ,[MDivisionID]

select * from Production.dbo.InvAdjust

-------------------------- INSERT INTO §ì
INSERT INTO Production.dbo.InvAdjust(
        ID
      ,IssueDate
      ,REASON
      ,GarmentInvoiceID
      ,OrderID
      ,PullDate
      ,Ukey_Pullout
      ,BrandID
      ,FactoryID
      ,ARVoucherID
      ,VoucherID
      ,Status
      ,OrigPulloutQty
      ,OrigPrice
      ,OrigPulloutAmt
      ,OrigSurcharge
      ,OrigAddCharge
      ,OrigCommission
      ,OrigDocFee
      ,AdjustPulloutQty
      ,AdjustPulloutAmt
      ,AdjustSurcharge
      ,AdjustAddCharge
      ,AdjustCommission
      ,AdjustDocFee
      ,AddName
      ,AddDate
      ,Edit_Name
      ,EditDate
      ,PriceCheckID

)
select 
        ID
      ,IssueDate
      ,REASON
      ,GarmentInvoiceID
      ,OrderID
      ,PullDate
      ,Ukey_Pullout
      ,BrandID
      ,FactoryID
      ,ARVoucherID
      ,VoucherID
      ,Status
      ,OrigPulloutQty
      ,OrigPrice
      ,OrigPulloutAmt
      ,OrigSurcharge
      ,OrigAddCharge
      ,OrigCommission
      ,OrigDocFee
      ,AdjustPulloutQty
      ,AdjustPulloutAmt
      ,AdjustSurcharge
      ,AdjustAddCharge
      ,AdjustCommission
      ,AdjustDocFee
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,PriceCheckID

from Trade_To_Pms.dbo.InvAdjust as b
where not exists(select id from Production.dbo.InvAdjust as a where a.id = b.id)

--InvAdjust_Qty
--PMS¦h
--,[Pullout3Qty]
-------------------------- INSERT INTO §ì
INSERT INTO Production.dbo.InvAdjust_Qty(
      ID
      ,Article
      ,SizeCode
      ,OrderQty
      ,OrigQty
      ,AdjustQty
      ,Price
      ,NewItem
      ,DiffQty
)
select 
     b.ID
      ,b.Article
      ,b.SizeCode
      ,b.OrderQty
      ,b.OrigQty
      ,b.AdjustQty
      ,b.Price
      ,b.NewItem
      ,b.DiffQty
from Trade_To_Pms.dbo.InvAdjust_Qty as b inner join Trade_To_Pms.dbo.InvAdjust as c on b.ID=c.ID 
where not exists(select id from Production.dbo.InvAdjust_Qty as a where a.id = b.id and a.Article=b.Article and a.SizeCode = b.SizeCode)

--pullout2
--select SUM(ShipQty) from Production.dbo.Pullout_Detail A inner join Trade_To_Pms.dbo.InvAdjust B ON A.OrderID=B.OrderID


--SELECT sum(DiffQty) FROM  Production.dbo.InvAdjust_Qty A inner join Production.dbo.InvAdjust B ON  A.ID = B.ID

SELECT MAX(PulloutDate) as PulloutDate --,a.ID
INTO #TMPPullout2Cdate
FROM  Production.dbo.Pullout_Detail A
inner join Trade_To_Pms.dbo.InvAdjust B ON A.OrderID=B.OrderID AND ShipQty <> 0


UPDATE a
SET  
Status = 'C'
from  Production.dbo.Pullout_Detail as a 
inner join #TMPPullout2Cdate as b ON a.PulloutDate=b.PulloutDate
inner join Trade_To_Pms.dbo.InvAdjust c ON  A.OrderID = c.OrderID
where b.PulloutDate is not null


UPDATE a

SET  
 a.ActPulloutDate = (select IIF(PulloutDate is null,(select PullDate from Trade_To_Pms.dbo.InvAdjust b where a.ID=b.ID),PulloutDate) from #TMPPullout2Cdate)
,a.PulloutComplete = 1
,a.MDClose = getdate()

from Production.dbo.Orders as a 
--inner join Trade_To_Pms.dbo.TTTTTTTTTTTTTTTTTTTTT as b ON a.id=b.id
where a.Qty = (select SUM(ShipQty) from Production.dbo.Pullout_Detail A inner join Trade_To_Pms.dbo.InvAdjust B ON A.OrderID=B.OrderID)
+(SELECT sum(DiffQty) FROM  Production.dbo.InvAdjust_Qty A inner join Production.dbo.InvAdjust B ON  A.ID = B.ID)
and a.PulloutComplete = 0



drop table #TMPPullout2Cdate








END




