

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
      a.OrderShipmodeSeq	      =b.OrderShipmodeSeq
      --,a.MDivisionID	      =b.MDivisionID
      ,a.ShipQty	      =b.ShipQty
      ,a.ETA	      =b.ETA
      ,a.ReceiveDoxDate	      =b.ReceiveDoxDate
      ,a.GW	      =b.GW
      ,a.VW	      =b.VW
      ,a.Forwarder	      =b.Forwarder
      ,a.Quotation	      =b.Quotation
      ,a.Forwarder1	      =b.Forwarder1
      ,a.Quotation1	      =b.Quotation1
      ,a.Forwarder2	      =b.Forwarder2
      ,a.Quotation2	      =b.Quotation2
      ,a.EstAmount	      =b.EstAmount
      ,a.ActualAmount	      =b.ActualAmount
      ,a.Rate	      =b.Rate
      ,a.SRNo	      =b.SRNo
      ,a.Voucher	      =b.Voucher
      ,a.PayDate	      =b.PayDate
      ,a.ReasonID	      =b.ReasonID
      ,a.FtyDesc	      =b.FtyDesc
      ,a.Remark	      =b.Remark
      ,a.MRComment	      =b.MRComment
      ,a.ResponsibleFty	      =b.ResponsibleFty
      ,a.RatioFty	      =b.RatioFty
      ,a.ResponsibleFtyNo	      =b.ResponsibleFtyNo
      ,a.ResponsibleSubcon	      =b.ResponsibleSubcon
      ,a.RatioSubcon	      =b.RatioSubcon
      ,a.SubconDBCNo	      =b.SubconDBCNo
      ,a.SubconDBCRemark	      =b.SubconDBCRemark
      ,a.SubConName	      =b.SubConName
      ,a.ResponsibleSCI	      =b.ResponsibleSCI
      ,a.RatioSCI	      =b.RatioSCI
      ,a.SCIICRNo	      =b.SCIICRNo
      ,a.SCIICRRemark	      =b.SCIICRRemark
      ,a.ResponsibleSupp	      =b.ResponsibleSupp
      ,a.RatioSupp	      =b.RatioSupp
      ,a.SuppDBCNo	      =b.SuppDBCNo
      ,a.SuppDBCRemark	      =b.SuppDBCRemark
      ,a.ResponsibleBuyer	      =b.ResponsibleBuyer
      ,a.RatioBuyer	      =b.RatioBuyer
      ,a.BuyerDBCNo	      =b.BuyerDBCNo
      ,a.BuyerDBCRemark	      =b.BuyerDBCRemark
      ,a.BuyerICRNo	      =b.BuyerICRNo
      ,a.BuyerICRRemark	      =b.BuyerICRRemark
      ,a.BuyerRemark	      =b.BuyerRemark
      ,a.PPICMgr	      =b.PPICMgr
      ,a.PPICMgrApvDate	      =b.PPICMgrApvDate
      ,a.FtyMgr	      =b.FtyMgr
      ,a.FtyMgrApvDate	      =b.FtyMgrApvDate
      ,a.POHandle	      =b.POHandle
      ,a.POSMR	      =b.POSMR
      ,a.MRHandle	      =b.MRHandle
      ,a.SMR	      =b.SMR
      ,a.SMRApvDate	      =b.SMRApvDate
      ,a.Task	      =b.Task
      ,a.TaskApvDate	      =b.TaskApvDate
      ,a.Status	      =b.Status
      --,a.FtySendDate	      =b.FtySendDate
      --,a.AddName	      =b.AddName
      --,a.AddDate	      =b.AddDate
      ,a.TPEEditName	      = b.EditName
      ,a.TPEEditDate	      = b.EditDate
	  ,a.ActETD 	      = b.ActETD 
	  ,a.CW				 = b.CW
	  ,a.APReceiveDoxDate				 = b.APReceiveDoxDate
	  ,a.APAmountEditDate				 = b.APAmountEditDate
	  ,a.ActualAmountWVAT				 = b.ActualAmountWVAT
from Production.dbo.AirPP as a 
inner join Trade_To_Pms.dbo.AirPP as b ON a.id=b.id
where isnull(a.TPEEditDate,'') != isnull(b.EditDate,'')
-------------------------- 

END




