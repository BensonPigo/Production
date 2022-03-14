
Create PROCEDURE [dbo].[exp_Airpp] 
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

------------------------------------------------------------------------------------------------------
--***��ƥ洫�����󭭨�***
--1. �u�����oProduction.dbo.DateInfo
declare @DateInfoName varchar(30) ='AirPP';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.���o�w�]��
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
	
--3.��sPms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateStart, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateStart,@Remark);
------------------------------------------------------------------------------------------------------

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
	  ,[APPExchageRate] = A.ExchangeRate
	  ,[APPAmtUSD] = A.ActAmt
INTO AirPP
FROM Production.dbo.AirPP AS A
left join Production.dbo.View_AirPP va on va.ID = A.id
WHERE	-- 30 �Ѥ��s�W���ʪ����
		CONVERT(datetime,isnull(EditDate,AddDate)) >= @DateStart
		OR
		EXISTS(
			----�]�t�ŹB�|�p��ت�ShareExpense_APP�A30�Ѥ������ʹL
			SELECT 1
			FROM Production.dbo.ShareExpense_APP sea
			INNER JOIN  FinanceEN.dbo.AccountNo AN ON sea.AccountID = AN.ID
			WHERE	sea.AirPPID = A.ID
					----�]�t�ŹB�O
					AND AN.IsAPP = 1
					----ShareExpense_APP EditDate �b 30 �Ѥ�
					AND sea.EditDate >= @DateStart 
		)
		OR 
		EXISTS(
			----�]�t�ŹB�|�p���ShareExpense_APP��ShippingAP�A30�Ѥ������ʹL
			SELECT 1
			FROM Production.dbo.ShippingAP SA
			INNER JOIN Production.dbo.ShareExpense_APP sea ON SA.ID = sea.ShippingAPID
			INNER JOIN FinanceEN.dbo.AccountNo AN ON sea.AccountID = AN.ID
			WHERE	sea.AirPPID = A.ID
					----�]�t�ŹB�O
					AND AN.IsAPP = 1
					----ShippingAP.VoucherEditDate�MEditDate,AddDate �b 30 �Ѥ�
					AND (
						SA.VoucherEditDate >= @DateStart
						or CONVERT(datetime,isnull(sa.EditDate,sa.AddDate)) >= @DateStart
					)
		)
ORDER BY A.ID 

UPDATE Production.dbo.AirPP
SET FtySendDate = GETDATE()
WHERE 
CONVERT(datetime,isnull(EditDate,AddDate)) >= @DateStart
 AND CONVERT(DATETIME, isnull(TPEEditDate,'')) <= CONVERT(DATETIME, isnull(EditDate,''))
AND Status IN ('New','Checked','Approved','Junked') AND FtyMgrApvDate is not null AND FtySendDate is null


---------------------------------------------------------------------------------------------------------------



END




