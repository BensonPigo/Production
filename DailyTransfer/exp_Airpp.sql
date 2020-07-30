
CREATE PROCEDURE [dbo].[exp_Airpp] 
AS
BEGIN
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.AirPP') IS NOT NULL
BEGIN
  DROP TABLE AirPP
END

IF OBJECT_ID(N'dbo.ShareExpense_APP') IS NOT NULL
BEGIN
  DROP TABLE ShareExpense_APP
END

declare @DateInfoName varchar(30) ='AirPP';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = NULL--(select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
if @DateEnd is Null
	set @DateEnd = NULL-- CONVERT(DATE, GETDATE())
	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);
-----------------------------------------------------AirPP-----------------------------------------------------

SELECT	app.ID
		, tmp.VoucherID 
		, tmp.VoucherDate
INTO #tmpVoucher
FROM Production.dbo.AirPP app
OUTER APPLY(
	------找出包含空運會計科目的ShareExpense_APP
	SELECT	sa.VoucherID
			, sa.VoucherDate
	FROM Production.dbo.ShippingAP sa
	WHERE EXISTS(
		SELECT 1
		FROM Production.dbo.ShareExpense_APP sea 
		INNER JOIN FinanceEN.dbo.AccountNo an ON sea.AccountID = an.ID
		WHERE	an.IsAPP=1 
				AND sea.ShippingAPID=sa.ID
				AND sea.AirPPID= app.ID
	)
)tmp
WHERE	-- 30 天內新增異動的資料
		CONVERT(datetime,isnull(app.EditDate, app.AddDate)) >= @DateStart
		or
		EXISTS(
			----包含空運會計科目的ShareExpense_APP，30天內有異動過
			SELECT 1
			FROM Production.dbo.ShareExpense_APP sea
			INNER JOIN  FinanceEN.dbo.AccountNo AN ON sea.AccountID = AN.ID
			WHERE	sea.AirPPID = app.ID
					----包含空運費
					AND AN.IsAPP = 1
					----ShareExpense_APP EditDate 在 30 天內
					AND sea.EditDate >= @DateStart
					AND sea.Junk=0
		)
		OR 
		EXISTS(
			----包含空運會計科目ShareExpense_APP的ShippingAP，30天內有異動過
			SELECT 1
			FROM Production.dbo.ShippingAP SA
			INNER JOIN Production.dbo.ShareExpense_APP sea ON SA.ID = sea.ShippingAPID
			INNER JOIN FinanceEN.dbo.AccountNo AN ON sea.AccountID = AN.ID
			WHERE	sea.AirPPID = app.ID
					----包含空運費
					AND AN.IsAPP = 1
					----ShippingAP.VoucherEditDate 在 30 天內
					AND SA.VoucherEditDate >= @DateStart 
					AND sea.Junk=0
		)

-------

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
	  ,[QuotationAVG]
	  ,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder),12) ) AS ForWardN
  	  ,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder1) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder1),12) ) AS ForWard1N
	  ,iif((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder2) is null,'',LEFT((SELECT S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder2),12) ) AS ForWard2N
	  , [VoucherDate] = Voucher.VoucherDate
	  , [VoucherID] = Voucher.VoucherID
INTO AirPP
FROM Production.dbo.AirPP AS A
OUTER APPLY (
	SELECT VoucherID = STUFF (	(	SELECT DISTINCT CONCAT (',', tV.VoucherID) 
									FROM #tmpVoucher tV
									WHERE A.ID = tV.ID
										  AND tV.VoucherID != ''
										  AND tV.VoucherID IS NOT NULL
									FOR XML PATH('')
								)
							    , 1, 1, ''
							  )
		   , VoucherDate = (
							SELECT MIN (tV.VoucherDate)
							FROM #tmpVoucher tV
							WHERE A.ID = tV.ID
								  AND tV.VoucherDate IS NOT NULL
							)
) Voucher
WHERE	-- 30 天內新增異動的資料
		CONVERT(datetime,isnull(EditDate,AddDate)) >= @DateStart
		OR EXISTS(
			SELECT 1
			FROM  #tmpVoucher tV
			WHERE A.ID = tV.ID
		)
ORDER BY Id 

UPDATE Production.dbo.AirPP
SET FtySendDate = GETDATE()
WHERE 
CONVERT(datetime,isnull(EditDate,AddDate)) >= @DateStart
 AND CONVERT(DATETIME, isnull(TPEEditDate,'')) <= CONVERT(DATETIME, isnull(EditDate,''))
AND Status IN ('New','Checked','Approved','Junked') AND FtyMgrApvDate is not null AND FtySendDate is null

drop table #tmpVoucher

---------------------------------------------------------------------------------------------------------------


-----------------------------------------------------ShareExpense_APP------------------------------------------

SELECT s.ShippingAPID
,s.InvNO
,s.PackingListID
,s.AirPPID
,s.AccountID
,s.CurrencyID
,s.NW
,s.RatioFty
,s.AmtFty
,s.RatioOther
,s.AmtOther
,s.Junk
,s.EditName
,s.EditDate
,sa.APPExchageRate
,[APPAmtUSD] =  CASE WHEN sa.APPExchageRate = 0 THEN 0
				ELSE ROUND( (s.AmtFty + s.AmtOther) / sa.APPExchageRate , 2 )
				END
INTO ShareExpense_APP
FROM Production.dbo.ShareExpense_APP s
LEFT JOIN Production.dbo.ShippingAP sa ON s.ShippingAPID = sa.ID
WHERE CONVERT(datetime,s.EditDate) >= @DateStart
---------------------------------------------------------------------------------------------------------------


END




