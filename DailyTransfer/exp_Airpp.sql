
-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/08/17>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Airpp] 
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
SELECT [ID]
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
,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder),12) ) AS ForWardN
,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder1) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder1),12) ) AS ForWard1N
,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder2) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder2),12) ) AS ForWard2N
INTO AirPP
FROM Production.dbo.AirPP AS A
WHERE 
((EditDate >= DATEADD(DAY, -30, GETDATE()) AND CONVERT(DATETIME, TPEEditDate) <= CONVERT(DATETIME, EditDate))
OR 
(CONVERT(DATETIME, EditDate) >= DATEADD(DAY, -30, GETDATE()) AND CONVERT(DATETIME,AddDate) <= CONVERT(DATETIME, EditDate)))
AND Status IN ('New','Checked','Approved','Junked')
ORDER BY Id 


UPDATE Production.dbo.AirPP
SET FtySendDate = GETDATE()
WHERE 
((EditDate >= DATEADD(DAY, -30, GETDATE()) AND CONVERT(DATETIME, TPEEditDate) <= CONVERT(DATETIME, EditDate))
OR
(CONVERT(DATETIME, EditDate) >= DATEADD(DAY, -30, GETDATE()) 
AND CONVERT(DATETIME, AddDate) <= CONVERT(DATETIME, EditDate)))
AND Status IN ('New','Checked','Approved','Junked') AND FtyMgrApvDate is not null AND FtySendDate is null
END




