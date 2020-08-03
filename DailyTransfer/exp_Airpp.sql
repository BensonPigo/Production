
-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/08/17>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[exp_Airpp] 
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.AirPP') IS NOT NULL
BEGIN
  DROP TABLE AirPP
END

IF OBJECT_ID(N'dbo.ShareExpense_APP') IS NOT NULL
BEGIN
  DROP TABLE ShareExpense_APP
END

-----------------------------------------------------AirPP-----------------------------------------------------

SELECT [ID] = a.ID
      ,[CDate]
      ,[OrderID]
      ,[OrderShipmodeSeq]
      ,[MDivisionID]
      ,[ShipQty]
      ,[ETA]
      ,[ReceiveDoxDate]
      ,[GW]
      ,[VW]
      ,[Forwarder]
      ,[Quotation]
      ,[Forwarder1]
      ,[Quotation1]
      ,[Forwarder2]
      ,[Quotation2]
      ,[EstAmount]
      ,[ActualAmount]
      ,[Rate]
      ,[SRNo]
      ,[Voucher]
      ,[PayDate]
      ,[ReasonID]
      ,[FtyDesc]
      ,[Remark]
      ,[MRComment]
      ,[ResponsibleFty]
      ,[RatioFty]
      ,[ResponsibleFtyNo]
      ,[ResponsibleSubcon]
      ,[RatioSubcon]
      ,[SubconDBCNo]
      ,[SubconDBCRemark]
      ,[SubConName]
      ,[ResponsibleSCI]
      ,[RatioSCI]
      ,[SCIICRNo]
      ,[SCIICRRemark]
      ,[ResponsibleSupp]
      ,[RatioSupp]
      ,[SuppDBCNo]
      ,[SuppDBCRemark]
      ,[ResponsibleBuyer]
      ,[RatioBuyer]
      ,[BuyerDBCNo]
      ,[BuyerDBCRemark]
      ,[BuyerICRNo]
      ,[BuyerICRRemark]
      ,[BuyerRemark]
      ,[PPICMgr]
      ,[PPICMgrApvDate]
      ,[FtyMgr]
      ,[FtyMgrApvDate]
      ,[POHandle]
      ,[POSMR]
      ,[MRHandle]
      ,[SMR]
      ,[SMRApvDate]
      ,[Task]
      ,[TaskApvDate]
      ,[Status]
      ,[FtySendDate]
      ,iif([AddName] is null,'',[AddName]) as [AddName]
      ,[AddDate]
      ,iif([EditName] is null,'',[EditName]) as [EditName]
      ,[EditDate]
      ,[TPEEditName]
      ,[TPEEditDate]
	  ,[ShipLeader]
	  ,[QuotationAVG]
	  ,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder),12) ) AS ForWardN
  	  ,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder1) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder1),12) ) AS ForWard1N
	  ,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder2) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder2),12) ) AS ForWard2N
	  , [VoucherDate] = va.VoucherDate
	  , [VoucherID] = va.VoucherID
	  ,[APPExchageRate] = va.APPExchageRate
	  ,[APPAmtUSD] = va.ActAmtUSD
INTO AirPP
FROM Production.dbo.AirPP AS A
left join Production.dbo.View_AirPP va on va.ID = A.id
WHERE	-- 30 天內新增異動的資料
		CONVERT(datetime,isnull(EditDate,AddDate)) >= DATEADD(DAY, -30, GETDATE()) 
		OR
		EXISTS(
			----包含空運會計科目的ShareExpense_APP，30天內有異動過
			SELECT 1
			FROM Production.dbo.ShareExpense_APP sea
			INNER JOIN  FinanceEN.dbo.AccountNo AN ON sea.AccountID = AN.ID
			WHERE	sea.AirPPID = A.ID
					----包含空運費
					AND AN.IsAPP = 1
					----ShareExpense_APP EditDate 在 30 天內
					AND sea.EditDate >= DATEADD(DAY, -30, GETDATE())  
		)
		OR 
		EXISTS(
			----包含空運會計科目ShareExpense_APP的ShippingAP，30天內有異動過
			SELECT 1
			FROM Production.dbo.ShippingAP SA
			INNER JOIN Production.dbo.ShareExpense_APP sea ON SA.ID = sea.ShippingAPID
			INNER JOIN FinanceEN.dbo.AccountNo AN ON sea.AccountID = AN.ID
			WHERE	sea.AirPPID = A.ID
					----包含空運費
					AND AN.IsAPP = 1
					----ShippingAP.VoucherEditDate和EditDate,AddDate 在 30 天內
					AND (
						SA.VoucherEditDate >= DATEADD(DAY, -30, GETDATE())   
						or CONVERT(datetime,isnull(sa.EditDate,sa.AddDate)) >= DATEADD(DAY, -30, GETDATE())
					)
		)
ORDER BY A.ID 

UPDATE Production.dbo.AirPP
SET FtySendDate = GETDATE()
WHERE 
CONVERT(datetime,isnull(EditDate,AddDate)) >= DATEADD(DAY, -30, GETDATE()) 
 AND CONVERT(DATETIME, isnull(TPEEditDate,'')) <= CONVERT(DATETIME, isnull(EditDate,''))
AND Status IN ('New','Checked','Approved','Junked') AND FtyMgrApvDate is not null AND FtySendDate is null


---------------------------------------------------------------------------------------------------------------



END




