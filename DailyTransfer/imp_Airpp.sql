

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
      -- a.ID	 =b.ID
      --a.CDate	      =b.CDate
      --,a.OrderID	      =b.OrderID
      a.OrderShipmodeSeq	      =isnull(b.OrderShipmodeSeq, '')
      --,a.MDivisionID	      =b.MDivisionID
      ,a.ShipQty	      =isnull(b.ShipQty, 0)
      ,a.ETA	      =b.ETA
      ,a.ReceiveDoxDate	      =b.ReceiveDoxDate
      ,a.GW	      =isnull(b.GW, 0)
      ,a.VW	      =isnull(b.VW, 0)
      ,a.Forwarder	      =isnull(b.Forwarder, '')
      ,a.Quotation	      =isnull(b.Quotation, 0)
      ,a.Forwarder1	      =isnull(b.Forwarder1, '')
      ,a.Quotation1	      =isnull(b.Quotation1, 0)
      ,a.Forwarder2	      =isnull(b.Forwarder2, '')
      ,a.Quotation2	      =isnull(b.Quotation2, 0)
      ,a.EstAmount	      =isnull(b.EstAmount, 0)
      --,a.ActualAmount	      =b.ActualAmount
      --,a.Rate	      =b.Rate
      ,a.SRNo	      =isnull(b.SRNo, '')
      ,a.Voucher	      =isnull(b.Voucher, '')
      ,a.PayDate	      =b.PayDate
      ,a.ReasonID	      =isnull(b.ReasonID, '')
      ,a.FtyDesc	      =isnull(b.FtyDesc, '')
      ,a.Remark	      =isnull(b.Remark, '')
      ,a.MRComment	      =isnull(b.MRComment, '')
      ,a.ResponsibleFty	      =isnull(b.ResponsibleFty, 0)
      ,a.RatioFty	      =isnull(b.RatioFty, 0)
      ,a.ResponsibleFtyNo	      =isnull(b.ResponsibleFtyNo, '')
      ,a.ResponsibleSubcon	      =isnull(b.ResponsibleSubcon, 0)
      ,a.RatioSubcon	      =isnull(b.RatioSubcon, 0)
      ,a.SubconDBCNo	      =isnull(b.SubconDBCNo, '')
      ,a.SubconDBCRemark	      =isnull(b.SubconDBCRemark, '')
      ,a.SubConName	      =isnull(b.SubConName, '')
      ,a.ResponsibleSCI	      =isnull(b.ResponsibleSCI, 0)
      ,a.RatioSCI	      =isnull(b.RatioSCI, 0)
      ,a.SCIICRNo	      =isnull(b.SCIICRNo, '')
      ,a.SCIICRRemark	      =isnull(b.SCIICRRemark, '')
      ,a.ResponsibleSupp	      =isnull(b.ResponsibleSupp, 0)
      ,a.RatioSupp	      =isnull(b.RatioSupp, 0)
      ,a.SuppDBCNo	      =isnull(b.SuppDBCNo, '')
      ,a.SuppDBCRemark	      =isnull(b.SuppDBCRemark, '')
      ,a.ResponsibleBuyer	      =isnull(b.ResponsibleBuyer, 0)
      ,a.RatioBuyer	      =isnull(b.RatioBuyer, 0)
      ,a.BuyerDBCNo	      =isnull(b.BuyerDBCNo, '')
      ,a.BuyerDBCRemark	      =isnull(b.BuyerDBCRemark, '')
      ,a.BuyerICRNo	      =isnull(b.BuyerICRNo, '')
      ,a.BuyerICRRemark	      =isnull(b.BuyerICRRemark, '')
      ,a.BuyerRemark	      =isnull(b.BuyerRemark, '')
      ,a.PPICMgr	      =isnull(b.PPICMgr, '')
      ,a.PPICMgrApvDate	      =b.PPICMgrApvDate
      ,a.FtyMgr	      =isnull(b.FtyMgr, '')
      ,a.FtyMgrApvDate	      =b.FtyMgrApvDate
      ,a.POHandle	      =isnull(b.POHandle, '')
      ,a.POSMR	      =isnull(b.POSMR, '')
      ,a.MRHandle	      =isnull(b.MRHandle, '')
      ,a.SMR	      =isnull(b.SMR, '')
      ,a.SMRApvDate	      =b.SMRApvDate
      ,a.Task	      =isnull(b.Task, '')
      ,a.TaskApvDate	      =b.TaskApvDate
      ,a.Status	      =isnull(b.Status, '')
      --,a.FtySendDate	      =b.FtySendDate
      --,a.AddName	      =b.AddName
      --,a.AddDate	      =b.AddDate
      ,a.TPEEditName	      = isnull(b.EditName, '')
      ,a.TPEEditDate	      = b.EditDate
	  ,a.ActETD 	      = b.ActETD 
	  ,a.CW				 = isnull(b.CW, 0)
	  ,a.APReceiveDoxDate				 = b.APReceiveDoxDate
	  ,a.APAmountEditDate				 = b.APAmountEditDate
	  ,a.ActualAmountWVAT				 = isnull(b.ActualAmountWVAT, 0)
from Production.dbo.AirPP as a 
inner join Trade_To_Pms.dbo.AirPP as b ON a.id=b.id
where isnull(a.TPEEditDate,'') != isnull(b.EditDate,'')
-------------------------- 

END




