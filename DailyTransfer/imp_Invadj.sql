
-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[imp_Invadj]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


--更新使用Merge寫法 ----------------InvAdjust
Merge Production.dbo.InvAdjust as t
using (
	select b.*,p.OrderShipmodeSeq from Trade_To_Pms.dbo.InvAdjust b WITH (NOLOCK) 
	inner join Production.dbo.Factory c WITH (NOLOCK) on b.FactoryID=c.ID
	outer apply(
		select OrderShipmodeSeq from Production.dbo.Pullout_Detail
		where invno=b.GarmentInvoiceID
		and orderid=b.orderid
		and pulldate=b.pulldate
	) as p	
) as s
on t.id=s.id
when not matched by target then 
insert(ID
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
	  ,OrderShipmodeSeq)
values( s.ID
      ,s.IssueDate
      ,s.REASON
      ,s.GarmentInvoiceID
      ,s.OrderID
      ,s.PullDate
      ,s.Ukey_Pullout
      ,s.BrandID
      ,s.FactoryID
      ,s.ARVoucherID
      ,s.VoucherID
      ,s.Status
      ,s.OrigPulloutQty
      ,s.OrigPrice
      ,s.OrigPulloutAmt
      ,s.OrigSurcharge
      ,s.OrigAddCharge
      ,s.OrigCommission
      ,s.OrigDocFee
      ,s.AdjustPulloutQty
      ,s.AdjustPulloutAmt
      ,s.AdjustSurcharge
      ,s.AdjustAddCharge
      ,s.AdjustCommission
      ,s.AdjustDocFee
      ,s.AddName
      ,s.AddDate
      ,s.EditName
      ,s.EditDate
      ,s.PriceCheckID
	  ,isnull(s.OrderShipmodeSeq,''));

--更新Merge 寫法-------------------------InvAdjust_Qty
Merge Production.dbo.InvAdjust_Qty as t
using (
	select b.* from Trade_To_Pms.dbo.InvAdjust_Qty as b WITH (NOLOCK)
	inner join Trade_To_Pms.dbo.InvAdjust as c WITH (NOLOCK) on b.ID=c.ID 
) as s
on t.id=s.id and t.Article=s.Article and t.SizeCode=s.SizeCode
when not matched by target then
insert( ID
      ,Article
      ,SizeCode
      ,OrderQty
      ,OrigQty
      ,AdjustQty
      ,Price
      ,NewItem
      ,DiffQty)
values( 
	   s.ID
      ,s.Article
      ,s.SizeCode
      ,s.OrderQty
      ,s.OrigQty
      ,s.AdjustQty
      ,s.Price
      ,s.NewItem
      ,s.DiffQty);


SELECT MAX(PulloutDate) as PulloutDate --,a.ID
INTO #TMPPullout2Cdate
FROM  Production.dbo.Pullout_Detail A WITH (NOLOCK)
inner join Trade_To_Pms.dbo.InvAdjust B WITH (NOLOCK) ON A.OrderID=B.OrderID AND ShipQty <> 0


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
where a.Qty = (select SUM(ShipQty) from Production.dbo.Pullout_Detail A inner join Trade_To_Pms.dbo.InvAdjust B ON A.OrderID=B.OrderID)
+(SELECT sum(DiffQty) FROM  Production.dbo.InvAdjust_Qty A inner join Production.dbo.InvAdjust B ON  A.ID = B.ID)
and a.PulloutComplete = 0



drop table #TMPPullout2Cdate








END




