

-- =============================================
-- Author:		LEO
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Airboat]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       --a.ID	     =b.ID
      --a.MDivisionID	      =b.MDivisionID
      --,a.ShipMark	      =b.ShipMark
      --,a.FromTag	      =b.FromTag
      --,a.FromSite	      =b.FromSite
      --,a.ToTag	      =b.ToTag
      --,a.ToSite	      =b.ToSite
      --,a.Dest	      =b.Dest
      --,a.PortAir	      =b.PortAir
      --,a.ShipDate	      =b.ShipDate
      --,a.ETD	      =b.ETD
      --,a.ETA	      =b.ETA
      --,a.CTNQty	      =b.CTNQty
      --,a.Handle	      =b.Handle
      --,a.Manager	      =b.Manager
      --,a.NW	      =b.NW
      --,a.CTNNW	      =b.CTNNW
      --,a.VW	      =b.VW
      --,a.CarrierID	      =b.CarrierID
     -- ,a.ExpressACNo	      =b.ExpressACNo
      --,a.BLNo	      =b.BLNo
      --,a.Remark	      =b.Remark
      --,a.FtyInvNo	      =b.FtyInvNo
      --,a.Status	      =b.Status
      --,a.StatusUpdateDate	      =b.StatusUpdateDate
      --,a.SendDate	      =b.SendDate
       a.PayDate	      =b.PayDate
      ,a.CurrencyID	      =isnull(b.CurrencyID, '')
      ,a.Amount	      =isnull(b.Amount, 0)
      ,a.InvNo	      =isnull(b.InvNo, '')
      --,a.AddName	      =b.AddName
      --,a.AddDate	      =b.AddDate
      --,a.EditName	      =b.EditName
      --,a.EditDate	      =b.EditDate
from Production.dbo.Express as a inner join Trade_To_Pms.dbo.FactoryExpress as b ON a.id=b.id

END




