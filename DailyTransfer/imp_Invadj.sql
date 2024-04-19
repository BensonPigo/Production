
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

	---- Check TransferDate before everything
	IF NOT  EXISTS(
		select 1 from Trade_To_PMS..DateInfo 
		where Name = 'TransferDate'
		AND DateStart in (CAST(DATEADD(DAY,-1,GETDATE()) AS date), CAST(GETDATE() AS DATE))
	)
	BEGIN
		-- �ߥX���~
		RAISERROR ('The DB transferdate is wrong. Trade_To_PMS..DateInfo  �����s�b�ŦX���� TransferDate �O���C', 16, 1); -- 16�O���~���Y���{�סA1�O���~���A	
		RETURN; 
	END
	


--��s�ϥ�Merge�g�k ----------------InvAdjust
Merge Production.dbo.InvAdjust as t
using (
	select b.* from Trade_To_Pms.dbo.InvAdjust b WITH (NOLOCK)
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
values(
       isnull(s.ID              ,'')
      ,s.IssueDate
      ,isnull(s.REASON          ,0)
      ,isnull(s.GarmentInvoiceID,'')
      ,isnull(s.OrderID         ,'')
      ,s.PullDate
      ,isnull(s.Ukey_Pullout    ,0)
      ,isnull(s.BrandID         ,'')
      ,isnull(s.FactoryID       ,'')
      ,isnull(s.ARVoucherID     ,'')
      ,isnull(s.VoucherID       ,'')
      ,isnull(s.Status          ,'')
      ,isnull(s.OrigPulloutQty  ,0)
      ,isnull(s.OrigPrice       ,0)
      ,isnull(s.OrigPulloutAmt  ,0)
      ,isnull(s.OrigSurcharge   ,0)
      ,isnull(s.OrigAddCharge   ,0)
      ,isnull(s.OrigCommission  ,0)
      ,isnull(s.OrigDocFee      ,0)
      ,isnull(s.AdjustPulloutQty,0)
      ,isnull(s.AdjustPulloutAmt,0)
      ,isnull(s.AdjustSurcharge ,0)
      ,isnull(s.AdjustAddCharge ,0)
      ,isnull(s.AdjustCommission,0)
      ,isnull(s.AdjustDocFee    ,0)
      ,isnull(s.AddName         ,'')
      ,s.AddDate
      ,isnull(s.EditName        ,'')
      ,s.EditDate
      ,isnull(s.PriceCheckID    ,'')
	  ,isnull(s.OrderShipmodeSeq,''));

--��sMerge �g�k-------------------------InvAdjust_Qty
Merge Production.dbo.InvAdjust_Qty as t
using (
	select b.* from Trade_To_Pms.dbo.InvAdjust_Qty as b WITH (NOLOCK)
	inner join Trade_To_Pms.dbo.InvAdjust as c WITH (NOLOCK) on b.ID=c.ID 
) as s
on t.id=s.id and t.Article=s.Article and t.SizeCode=s.SizeCode and t.Ukey_Pullout = s.Ukey_Pullout
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
	   isnull(s.ID       ,'')
      ,isnull(s.Article  ,'')
      ,isnull(s.SizeCode ,'')
      ,isnull(s.OrderQty ,0)
      ,isnull(s.OrigQty  ,0)
      ,isnull(s.AdjustQty,0)
      ,isnull(s.Price    ,0)
      ,isnull(s.NewItem  ,0)
      ,isnull(s.DiffQty  ,0)
      );

SELECT A.OrderID,MAX(PulloutDate) as PulloutDate --,a.ID
INTO #TMPPullout2Cdate
FROM  Production.dbo.Pullout_Detail A WITH (NOLOCK)
inner join Trade_To_Pms.dbo.InvAdjust B WITH (NOLOCK) ON A.OrderID=B.OrderID AND ShipQty <> 0
group by a.OrderID

UPDATE a
SET Status = 'C'
from Production.dbo.Pullout_Detail as a 
inner join #TMPPullout2Cdate as b ON a.PulloutDate=b.PulloutDate and a.OrderID = b.OrderID
where b.PulloutDate is not null

/*
�P�_�q��O�_�w�X�f����:
�q��ƶq = �X�f�ƶq�]Pullout_Detail.ShipQty, Status = Confirm/Lock�^+ �x�_�վ�ƶq�]InvAdjust_Qty DiffQty�^
*/
UPDATE a
SET  
 a.ActPulloutDate = (

	select iif(t.PulloutDate is null,
                 (select PullDate from Trade_To_Pms.dbo.InvAdjust i where i.OrderID = t.OrderID),
                 t.PulloutDate)
    from #TMPPullout2Cdate t
    where t.OrderID = a.ID
 )
,a.PulloutComplete = 1
,a.MDClose = GETDATE()
,a.PulloutCmplDate = CAST(GETDATE() as date)
FROM Production.dbo.Orders as a 
WHERE a.Qty = 
	(
		case when 
		(
			SELECT Qty = SUM(pd.ShipQty)
			FROM Production.dbo.Pullout p
			INNER JOIN Production.dbo.Pullout_Detail pd ON p.ID = pd.ID
			WHERE p.Status != 'New' AND pd.OrderID = a.ID
		) = 0 
		then
		(
			select sum(pd.ShipQty)
			from Production.dbo.PackingList p
			inner join Production.dbo.PackingList_Detail pd on p.ID = pd.ID
			where p.PulloutStatus <> 'New'
			and p.PulloutID <> ''
			and pd.OrderID = a.ID
		) else
		(
			SELECT Qty = SUM(pd.ShipQty)
			FROM Production.dbo.Pullout p
			INNER JOIN Production.dbo.Pullout_Detail pd ON p.ID = pd.ID
			WHERE p.Status != 'New' AND pd.OrderID = a.ID
		)
		end
	)
	+
	(
		SELECT SUM(iaq.DiffQty)
		FROM Production.dbo.InvAdjust ia
		INNER JOIN Production.dbo.InvAdjust_Qty iaq ON ia.ID = iaq.ID
		WHERE ia.OrderID = a.ID
	)
AND a.PulloutComplete = 0
AND EXISTS (
	SELECT 1 
	FROM Trade_To_Pms.dbo.InvAdjust ia
	WHERE ia.ORDERID = a.ID
)


drop table #TMPPullout2Cdate

END