-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Airpp] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   ---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.OrderShipmodeSeq = ISNULL(b.OrderShipmodeSeq, '')
      ,a.ShipQty = ISNULL(b.ShipQty, 0)
      ,a.ETA = b.ETA
      ,a.ReceiveDoxDate = b.ReceiveDoxDate
      ,a.GW = ISNULL(b.GW, 0)
      ,a.VW = ISNULL(b.VW, 0)
      ,a.Forwarder = ISNULL(b.Forwarder, '')
      ,a.Quotation = ISNULL(b.Quotation, 0)
      ,a.Forwarder1 = ISNULL(b.Forwarder1, '')
      ,a.Quotation1 = ISNULL(b.Quotation1, 0)
      ,a.Forwarder2 = ISNULL(b.Forwarder2, '')
      ,a.Quotation2 = ISNULL(b.Quotation2, 0)
      ,a.EstAmount = ISNULL(b.EstAmount, 0)
      ,a.SRNo = ISNULL(b.SRNo, '')
      ,a.Voucher = ISNULL(b.Voucher, '')
      ,a.PayDate = b.PayDate
      ,a.ReasonID = ISNULL(b.ReasonID, '')
      ,a.FtyDesc = ISNULL(b.FtyDesc, '')
      ,a.Remark = ISNULL(b.Remark, '')
      ,a.MRComment = ISNULL(b.MRComment, '')
      ,a.ResponsibleFty = ISNULL(b.ResponsibleFty, 0)
      ,a.RatioFty = ISNULL(b.RatioFty, 0)
      ,a.ResponsibleFtyNo = ISNULL(b.ResponsibleFtyNo, '')
      ,a.ResponsibleSubcon = ISNULL(b.ResponsibleSubcon, 0)
      ,a.RatioSubcon = ISNULL(b.RatioSubcon, 0)
      ,a.SubconDBCNo = ISNULL(b.SubconDBCNo, '')
      ,a.SubconDBCRemark = ISNULL(b.SubconDBCRemark, '')
      ,a.SubConName = ISNULL(b.SubConName, '')
      ,a.ResponsibleSCI = ISNULL(b.ResponsibleSCI, 0)
      ,a.RatioSCI = ISNULL(b.RatioSCI, 0)
      ,a.SCIICRNo = ISNULL(b.SCIICRNo, '')
      ,a.SCIICRRemark = ISNULL(b.SCIICRRemark, '')
      ,a.ResponsibleSupp = ISNULL(b.ResponsibleSupp, 0)
      ,a.RatioSupp = ISNULL(b.RatioSupp, 0)
      ,a.SuppDBCNo = ISNULL(b.SuppDBCNo, '')
      ,a.SuppDBCRemark = ISNULL(b.SuppDBCRemark, '')
      ,a.ResponsibleBuyer = ISNULL(b.ResponsibleBuyer, 0)
      ,a.RatioBuyer = ISNULL(b.RatioBuyer, 0)
      ,a.BuyerDBCNo = ISNULL(b.BuyerDBCNo, '')
      ,a.BuyerDBCRemark = ISNULL(b.BuyerDBCRemark, '')
      ,a.BuyerRemark = ISNULL(b.BuyerRemark, '')
      ,a.PPICMgr = ISNULL(b.PPICMgr, '')
      ,a.PPICMgrApvDate = b.PPICMgrApvDate
      ,a.FtyMgr = ISNULL(b.FtyMgr, '')
      ,a.FtyMgrApvDate = b.FtyMgrApvDate
      ,a.POHandle = ISNULL(b.POHandle, '')
      ,a.POSMR = ISNULL(b.POSMR, '')
      ,a.MRHandle = ISNULL(b.MRHandle, '')
      ,a.SMR = ISNULL(b.SMR, '')
      ,a.SMRApvDate = b.SMRApvDate
      ,a.Task = ISNULL(b.Task, '')
      ,a.TaskApvDate = b.TaskApvDate
      ,a.Status = ISNULL(b.Status, '')
      ,a.TPEEditName = ISNULL(b.EditName, '')
      ,a.TPEEditDate = b.EditDate
	  ,a.ActETD = b.ActETD 
	  ,a.CW = ISNULL(b.CW, 0)
	  ,a.APReceiveDoxDate = b.APReceiveDoxDate
	  ,a.APAmountEditDate = b.APAmountEditDate
	  ,a.ActualAmountWVAT = ISNULL(b.ActualAmountWVAT, 0)
      ,a.SCIICRNo2 = ISNULL(b.SCIICRNo2, '')
      ,a.SCIICRRemark2 = ISNULL(b.SCIICRRemark2, '')
from Production.dbo.AirPP as a 
inner join Trade_To_Pms.dbo.AirPP as b ON a.id=b.id
where isnull(a.TPEEditDate,'') != isnull(b.EditDate,'')

update t
set t.Additional = ISNULL(Additional.value,0)
from Production.dbo.AirPP as t
outer apply(
	select value = sum(ga.Additional)
	from Trade_to_PMS.dbo.GarmentInvoice_additional ga
	where ga.AdditionalReason = '02' 
	and ga.OrderID = t.orderid
	and ga.OrderShipmodeSeq = t.OrderShipmodeSeq
)Additional
where Additional.value is not null


END




