-- =============================================
-- Description:	
-- Import ForecastLoading in BI
-- =============================================
-- 2020/03/17 [ISP20200433] ForecastLoadingBI_Factory_Tms add import column [Half Key]
-- 2020/04/27 [ISP20200709] Table[Factory_TMS],[Factory_WorkHour] use server name to call regular Production data
-- 2020/05/18 [ISP20200840] Add Columns[Sew_Qty],[Shortage]
-- 2020/05/29 [ISP20200920] Add Columns[Buyer Key],[Buyer HalfKey]
CREATE PROCEDURE [dbo].[ImportForecastLoadingBI] 
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date_S DATE = '2019-01-08'; --���8��
	DECLARE @Date_E DATE = DATEADD(m, DATEDIFF(m,0,DATEADD(yy,1,GETDATE())),6);--�j�~7��
	DECLARE @YearMonth_S date = '2019-01-01';--���
	DECLARE @YearMonth_E date = dateadd(m, 11, getdate())--�������12�Ӥ�
	--���s�إ�Power BI��Report Table
	DECLARE @TableNameA VARCHAR(20);
	DECLARE @TableNameB VARCHAR(20);

	declare @SqlCmd1 nvarchar(max) ='';

	/*判斷當前Server後, 指定帶入正式機Server名稱*/
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	--依不同Server來抓到對應的備機ServerName
	declare @current_PMS_ServerName nvarchar(50) 
	= (
		select [value] = 
			CASE WHEN @current_ServerName= 'PHL-NEWPMS-02' THEN 'PHL-NEWPMS' -- PH1
				 WHEN @current_ServerName= 'VT1-PH2-PMS2b' THEN 'VT1-PH2-PMS2' -- PH2
				 WHEN @current_ServerName= 'system2017BK' THEN 'SYSTEM2017' -- SNP
				 WHEN @current_ServerName= 'SPS-SQL2' THEN 'SPS-SQL.spscd.com' -- SPS
				 WHEN @current_ServerName= 'SQLBK' THEN 'PMS-SXR' -- SPR
				 WHEN @current_ServerName= 'newerp-bak' THEN 'newerp' -- HZG		
				 WHEN @current_ServerName= 'SQL' THEN 'NDATA' -- HXG
				 when (select top 1 MDivisionID from Production.dbo.Factory) in ('VM2','VM1') then 'SYSTEM2016' -- ESP & SPT
			ELSE '' END
	)

	/******************************************
	   移除原本的 先Drop 在Create方式
	*******************************************/
	If Exists(Select * From POWERBIReportData.sys.tables Where Name = 'ForecastLoadingBI') TRUNCATE Table ForecastLoadingBI;
	If Exists(Select * From POWERBIReportData.sys.tables Where Name = 'ForecastLoadingBI_Factory_Tms') TRUNCATE Table ForecastLoadingBI_Factory_Tms;

	if not exists(select * from syscolumns where id=OBJECT_ID('ForecastLoadingBI_Factory_Tms') and name='Half key')
	begin
		ALTER TABLE dbo.ForecastLoadingBI_Factory_Tms 
		ADD	[Half key] [varchar](8)
	end

------------------------------T_OrderList ->  ForecastLoadingBI------------------------------
select ID
into #ArtworkTypeList
From Production.dbo.ArtworkType
where id in ('BONDING (HAND)','BONDING (MACHINE)','LASER','HEAT TRANSFER','CUTTING','DOWN','INSPECTION',
	'DIE CUT','SUBLIMATION PRINT','SUBLIMATION ROLLER','AT','AT (HAND)',
	'AT (MACHINE)','FEEDOFARM','EYEBUTTON','SMALL HOT PRESS','BIG HOT PRESS',
	'DOWN FILLING','ZIG ZAG','REAL FLATSEAM','Fusible','LASER CUTTER',
	'DIE-CUT','AUTO-TEMPLATE','SEAM TAPING MACHINE','INTENSIVE MACHINE','ULTRASONIC MACHINE',
	'ROLLER SUBLIMATION','VELCRO MACHINE','EMBROIDERY','PRINTING','EMBOSS/DEBOSS',
	'GMT WASH','PAD PRINTING','GARMENT DYE', 'B-HOT PRESS(BONDING)', 'S-HOT PRESS(BONDING)', 'S-HOT PRESS(HT)')

--準備ArtworkType的欄位名稱
Select *
into #UseArtworkType
From(
	Select ID
	, name = 'TTL_' + id + ' (' + iif(nu.nUnit = 'TMS', 'CPU', 'Price') + ')'
	, unit = nu.nUnit
	, SEQ
	FROM Production.dbo.ArtworkType
	outer apply ( select nUnit = iif(ArtworkType.ProductionUnit = 'TMS', 'TMS', 'Price')) nu 
	where Junk = 0
  union
	select ID
	, name = 'TTL_' + id + ' (' + iif(ArtworkUnit = 'STITCH', 'STITCH in thousands', ArtworkUnit) + ')'
	, unit = ArtworkUnit
	, SEQ
    from Production.dbo.ArtworkType 
    where Junk = 0
    and ArtworkUnit <> ''
) tmpArtworkType
	Where tmpArtworkType.ID in (select id from #ArtworkTypeList)

/******************************************************************************************************************************
1. Orders.Category = '' 等於 Trade.SewLastDate
2. Orders.ForecastSampleGroup 等於 Trade.SampleGroup
3. Trade.Forecast.CheckStyle = 0 等於 Orders.Category = '' and Orders.ForecastSampleGroup in ('D','S') = 0 else = 1
*******************************************************************************************************************************/
select ID, Qty, BrandID, StyleID, SeasonID, OrderTypeID, ProgramID, Category, CPU
	, BuyerDelivery, StyleUkey, FactoryID, MDivisionID
	, [CheckStyle] = case when ForecastSampleGroup in ('D','S') then 0 else 1 end
	, [SampleGroup] = ForecastSampleGroup, CdCodeID, CurrencyID, AddDate, ForecastCategory
into #tmp_Forecast
from Production.dbo.Orders
where Category = ''

/******************************************************************************************************************************
1.  LocalOrder = '1' 等於 Trade.FactoryOrder
*******************************************************************************************************************************/
select [FactoryID]
	, [ID]
	, [BrandID]
	, [StyleID]
	, [SeasonID]
	, [BuyerDelivery]
	, [SCIDelivery]
	, [CFMDate]
	, [ProgramID]
	, [CDCodeID]
	, [CPU], [Qty]
	, [StyleUnit]
	, [Junk]
	, [SubconInSisterFty]
	, [MCHandle]
	, [StyleUkey]
	, [AddName]
	, [AddDate]
	, [EditName]
	, [EditDate]
	, CustPONO [OrderID]
	, [SubconInType]
into #tmp_FactoryOrder
from Production.dbo.Orders
where LocalOrder = '1'


--計算ArtworkType
select *
into #atSource
From(

	--Orders
	Select *
	From (
		Select tmsCost.ID
		, ArtworkTypeName = at.name
		, Value = case unit When 'TMS' Then orders.Qty * tms * 1.0 / 1400 * getCPURate.CpuRate
				  When 'STITCH' Then orders.Qty * tmsCost.Qty * 1.0 / 1000 * getCPURate.CpuRate
				  When 'PCS' Then orders.Qty * tmsCost.Qty * getCPURate.CpuRate
				  Else orders.Qty * getCPURate.CpuRate
				  End
		, SubconInType = ''
		From Production.dbo.Orders
		Left join Production.dbo.Order_TmsCost tmsCost on tmsCost.Id = orders.ID
		Left join #UseArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select CpuRate From Production.dbo.GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'O')) getCPURate -- 加入
		WHERE Orders.Category IN ('B', 'S') and Orders.SciDelivery between @Date_S and @Date_E and Orders.LocalOrder <> '1'
	) as a
	PIVOT
	(
		sum(value) for a.ArtworkTypeName in ([TTL_AT (CPU)],[TTL_AT (HAND) (CPU)],[TTL_AT (MACHINE) (CPU)],[TTL_AUTO-TEMPLATE (CPU)],[TTL_B-HOT PRESS(BONDING) (CPU)],[TTL_BIG HOT PRESS (CPU)],[TTL_BONDING (HAND) (CPU)],[TTL_BONDING (MACHINE) (CPU)],[TTL_CUTTING (CPU)],[TTL_DIE CUT (CPU)],[TTL_DIE-CUT (CPU)],[TTL_DOWN (CPU)],[TTL_DOWN FILLING (CPU)],[TTL_EMBOSS/DEBOSS (Price)],[TTL_EMBROIDERY (Price)],[TTL_EMBROIDERY (STITCH in thousands)],[TTL_EYEBUTTON (CPU)],[TTL_FEEDOFARM (CPU)],[TTL_Fusible (CPU)],[TTL_GARMENT DYE (Price)],[TTL_GMT WASH (Price)],[TTL_HEAT TRANSFER (CPU)],[TTL_INSPECTION (CPU)],[TTL_INTENSIVE MACHINE (CPU)],[TTL_LASER (CPU)],[TTL_LASER CUTTER (CPU)],[TTL_PAD PRINTING (Price)],[TTL_PRINTING (PCS)],[TTL_PRINTING (Price)],[TTL_REAL FLATSEAM (CPU)],[TTL_ROLLER SUBLIMATION (CPU)],[TTL_S-HOT PRESS(BONDING) (CPU)],[TTL_S-HOT PRESS(HT) (CPU)],[TTL_SEAM TAPING MACHINE (CPU)],[TTL_SMALL HOT PRESS (CPU)],[TTL_SUBLIMATION PRINT (CPU)],[TTL_SUBLIMATION ROLLER (CPU)],[TTL_ULTRASONIC MACHINE (CPU)],[TTL_VELCRO MACHINE (CPU)],[TTL_ZIG ZAG (CPU)])
	) as P

	UNION

	--Forecast
	Select *
	From (
		Select Forecast.ID
		, ArtworkTypeName = at.name
		, Value = case unit When 'TMS' Then Forecast.Qty * tms * 1.0 / 1400 * getCPURate.CpuRate
			When 'STITCH' Then  Forecast.Qty * tmsCost.Qty * 1.0 / 1000 * getCPURate.CpuRate
			When 'PCS' Then Forecast.Qty * tmsCost.Qty * getCPURate.CpuRate
			Else Forecast.Qty * getCPURate.CpuRate
			End
		, SubconInType = ''
		From #tmp_Forecast as Forecast
		Left join Production.dbo.Style	on Forecast.BrandID=Style.BrandID and Forecast.StyleID =Style.ID and  Forecast.SeasonID = Style.SeasonID
		Left join Production.dbo.Style_TmsCost tmsCost on tmsCost.StyleUKey = Style.Ukey
		Left join #UseArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select CpuRate From Production.dbo.GetCPURate(Forecast.OrderTypeID, Forecast.ProgramID, Forecast.ForecastCategory, Forecast.BrandID, 'S')) getCPURate
		WHERE Forecast.ForecastCategory IN ('B', 'S') and Forecast.BuyerDelivery between @Date_S and @Date_E
	) as a
	PIVOT
	(
		sum(value) for a.ArtworkTypeName in ([TTL_AT (CPU)],[TTL_AT (HAND) (CPU)],[TTL_AT (MACHINE) (CPU)],[TTL_AUTO-TEMPLATE (CPU)],[TTL_B-HOT PRESS(BONDING) (CPU)],[TTL_BIG HOT PRESS (CPU)],[TTL_BONDING (HAND) (CPU)],[TTL_BONDING (MACHINE) (CPU)],[TTL_CUTTING (CPU)],[TTL_DIE CUT (CPU)],[TTL_DIE-CUT (CPU)],[TTL_DOWN (CPU)],[TTL_DOWN FILLING (CPU)],[TTL_EMBOSS/DEBOSS (Price)],[TTL_EMBROIDERY (Price)],[TTL_EMBROIDERY (STITCH in thousands)],[TTL_EYEBUTTON (CPU)],[TTL_FEEDOFARM (CPU)],[TTL_Fusible (CPU)],[TTL_GARMENT DYE (Price)],[TTL_GMT WASH (Price)],[TTL_HEAT TRANSFER (CPU)],[TTL_INSPECTION (CPU)],[TTL_INTENSIVE MACHINE (CPU)],[TTL_LASER (CPU)],[TTL_LASER CUTTER (CPU)],[TTL_PAD PRINTING (Price)],[TTL_PRINTING (PCS)],[TTL_PRINTING (Price)],[TTL_REAL FLATSEAM (CPU)],[TTL_ROLLER SUBLIMATION (CPU)],[TTL_S-HOT PRESS(BONDING) (CPU)],[TTL_S-HOT PRESS(HT) (CPU)],[TTL_SEAM TAPING MACHINE (CPU)],[TTL_SMALL HOT PRESS (CPU)],[TTL_SUBLIMATION PRINT (CPU)],[TTL_SUBLIMATION ROLLER (CPU)],[TTL_ULTRASONIC MACHINE (CPU)],[TTL_VELCRO MACHINE (CPU)],[TTL_ZIG ZAG (CPU)])
	) as P

	UNION

	--FactoryOrder
	Select *
	From (
		Select FactoryOrder.ID
		, ArtworkTypeName = at.name
		, Value = case unit When 'TMS' Then FactoryOrder.Qty * tms * 1.0 / 1400 * getCPURate.CpuRate
			When 'STITCH' Then  FactoryOrder.Qty * tmsCost.Qty * 1.0 / 1000 * getCPURate.CpuRate
			When 'PCS' Then FactoryOrder.Qty * tmsCost.Qty * getCPURate.CpuRate
			Else FactoryOrder.Qty * getCPURate.CpuRate
			End
		, FactoryOrder.SubconInType
		From #tmp_FactoryOrder FactoryOrder
		Left join Production.dbo.Style	on FactoryOrder.BrandID=Style.BrandID and FactoryOrder.StyleID =Style.ID and  FactoryOrder.SeasonID = Style.SeasonID
		Left join Production.dbo.Style_TmsCost tmsCost on tmsCost.StyleUKey = Style.Ukey
		Left join #UseArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select 1 as CpuRate ) getCPURate
		WHERE FactoryOrder.Junk = 0 and FactoryOrder.Qty > 0 and FactoryOrder.SubconInType in ('2', '3') and FactoryOrder.SCIDelivery between @Date_S and @Date_E
	) as a
	PIVOT
	(
		sum(value) for a.ArtworkTypeName in ([TTL_AT (CPU)],[TTL_AT (HAND) (CPU)],[TTL_AT (MACHINE) (CPU)],[TTL_AUTO-TEMPLATE (CPU)],[TTL_B-HOT PRESS(BONDING) (CPU)],[TTL_BIG HOT PRESS (CPU)],[TTL_BONDING (HAND) (CPU)],[TTL_BONDING (MACHINE) (CPU)],[TTL_CUTTING (CPU)],[TTL_DIE CUT (CPU)],[TTL_DIE-CUT (CPU)],[TTL_DOWN (CPU)],[TTL_DOWN FILLING (CPU)],[TTL_EMBOSS/DEBOSS (Price)],[TTL_EMBROIDERY (Price)],[TTL_EMBROIDERY (STITCH in thousands)],[TTL_EYEBUTTON (CPU)],[TTL_FEEDOFARM (CPU)],[TTL_Fusible (CPU)],[TTL_GARMENT DYE (Price)],[TTL_GMT WASH (Price)],[TTL_HEAT TRANSFER (CPU)],[TTL_INSPECTION (CPU)],[TTL_INTENSIVE MACHINE (CPU)],[TTL_LASER (CPU)],[TTL_LASER CUTTER (CPU)],[TTL_PAD PRINTING (Price)],[TTL_PRINTING (PCS)],[TTL_PRINTING (Price)],[TTL_REAL FLATSEAM (CPU)],[TTL_ROLLER SUBLIMATION (CPU)],[TTL_S-HOT PRESS(BONDING) (CPU)],[TTL_S-HOT PRESS(HT) (CPU)],[TTL_SEAM TAPING MACHINE (CPU)],[TTL_SMALL HOT PRESS (CPU)],[TTL_SUBLIMATION PRINT (CPU)],[TTL_SUBLIMATION ROLLER (CPU)],[TTL_ULTRASONIC MACHINE (CPU)],[TTL_VELCRO MACHINE (CPU)],[TTL_ZIG ZAG (CPU)])
	) as P

	UNION

	--負的FactoryOrder
	Select *
	From (
		Select FactoryOrder.ID
		, ArtworkTypeName = at.name
		, Value = case unit When 'TMS' Then FactoryOrder.Qty * tms  * 1.0 / 1400 * getCPURate.CpuRate
			When 'STITCH' Then  FactoryOrder.Qty * tmsCost.Qty * 1.0 / 1000 * getCPURate.CpuRate
			When 'PCS' Then FactoryOrder.Qty * tmsCost.Qty * getCPURate.CpuRate
			Else FactoryOrder.Qty * getCPURate.CpuRate
			End * -1
		, SubconInType = '-2' --負數的Type用-2表示以便區分2與-2
		From #tmp_FactoryOrder FactoryOrder
		Left join Production.dbo.Style	on FactoryOrder.BrandID=Style.BrandID and FactoryOrder.StyleID =Style.ID and  FactoryOrder.SeasonID = Style.SeasonID
		Left join Production.dbo.Style_TmsCost tmsCost on tmsCost.StyleUKey = Style.Ukey
		inner join #UseArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select 1 as CpuRate ) getCPURate
		WHERE FactoryOrder.Junk = 0 and FactoryOrder.Qty > 0 and FactoryOrder.SubconInType = '2' and FactoryOrder.SCIDelivery between @Date_S and @Date_E
	) as a
	PIVOT
	(
		sum(value) for a.ArtworkTypeName in ([TTL_AT (CPU)],[TTL_AT (HAND) (CPU)],[TTL_AT (MACHINE) (CPU)],[TTL_AUTO-TEMPLATE (CPU)],[TTL_B-HOT PRESS(BONDING) (CPU)],[TTL_BIG HOT PRESS (CPU)],[TTL_BONDING (HAND) (CPU)],[TTL_BONDING (MACHINE) (CPU)],[TTL_CUTTING (CPU)],[TTL_DIE CUT (CPU)],[TTL_DIE-CUT (CPU)],[TTL_DOWN (CPU)],[TTL_DOWN FILLING (CPU)],[TTL_EMBOSS/DEBOSS (Price)],[TTL_EMBROIDERY (Price)],[TTL_EMBROIDERY (STITCH in thousands)],[TTL_EYEBUTTON (CPU)],[TTL_FEEDOFARM (CPU)],[TTL_Fusible (CPU)],[TTL_GARMENT DYE (Price)],[TTL_GMT WASH (Price)],[TTL_HEAT TRANSFER (CPU)],[TTL_INSPECTION (CPU)],[TTL_INTENSIVE MACHINE (CPU)],[TTL_LASER (CPU)],[TTL_LASER CUTTER (CPU)],[TTL_PAD PRINTING (Price)],[TTL_PRINTING (PCS)],[TTL_PRINTING (Price)],[TTL_REAL FLATSEAM (CPU)],[TTL_ROLLER SUBLIMATION (CPU)],[TTL_S-HOT PRESS(BONDING) (CPU)],[TTL_S-HOT PRESS(HT) (CPU)],[TTL_SEAM TAPING MACHINE (CPU)],[TTL_SMALL HOT PRESS (CPU)],[TTL_SUBLIMATION PRINT (CPU)],[TTL_SUBLIMATION ROLLER (CPU)],[TTL_ULTRASONIC MACHINE (CPU)],[TTL_VELCRO MACHINE (CPU)],[TTL_ZIG ZAG (CPU)])
	) as P

) source

/******************************************************************************************************************************
1. 移除Amount (USD) 工廠端用不到
*******************************************************************************************************************************/

--Orders
SELECT *
into #tmpOrderList
FROM (
Select
	[StyleUkey]         = Orders.StyleUkey,
	[Factory]			= Orders.FactoryID,
	[Factory Country]	= Factory.CountryID,
	[M Division]		= Orders.MDivisionID,
	[Buyer Delivery]	= Orders.BuyerDelivery,
	[SCI Delivery]		= Orders.SCIDelivery,
	[Key]				= convert(varchar(6),dateadd(day,-7,Orders.SCIDelivery),112),
	[Half key]		    = 
	case
	when day(Orders.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, Orders.SCIDelivery),112),6) + '02'
	when day(Orders.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, Orders.SCIDelivery, 112),6) + '01'
	when day(Orders.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, Orders.SCIDelivery, 112),6) + '02'
	else ''	end,

	[Category]			= Dropdownlist.Name,	
	[Dev. Sample]		= IIF(OrderType.isDevSample = 1, 'Y', ''), 
	[PONO]				= Orders.CustPONo,
	[POID]				= Orders.POID,
	[SPNO]				= Orders.ID,
	[Style]				= Orders.StyleID,
	[Season]			= Orders.SeasonID,
	[Brand]				= Orders.BrandID,
	[Program]			= Orders.ProgramID,
	[CD]				= Orders.CdCodeID,
    [Production family] = CDCode.ProductionFamilyID,
	[CPU]				= Orders.CPU,
	[Qty]				= isnull(Orders.Qty,0),
	[TTLCPU]			= (Orders.CPU * Orders.Qty * CPURate.CPURate),
    [Junk]              = IIF(Orders.Junk = 1, 'Y', ''), 
----ArtworkType----
	[TTL_AT (CPU)] = isnull([TTL_AT (CPU)],0),
	[TTL_AT (HAND) (CPU)] = isnull([TTL_AT (HAND) (CPU)],0),
	[TTL_AT (MACHINE) (CPU)] = isnull([TTL_AT (MACHINE) (CPU)],0),
	[TTL_AUTO-TEMPLATE (CPU)] = isnull([TTL_AUTO-TEMPLATE (CPU)],0),
	[TTL_B-HOT PRESS(BONDING) (CPU)] = isnull([TTL_B-HOT PRESS(BONDING) (CPU)],0),
	[TTL_BIG HOT PRESS (CPU)] = isnull([TTL_BIG HOT PRESS (CPU)],0),
	[TTL_BONDING (HAND) (CPU)] = isnull([TTL_BONDING (HAND) (CPU)],0),
	[TTL_BONDING (MACHINE) (CPU)] = isnull([TTL_BONDING (MACHINE) (CPU)],0),
	[TTL_CUTTING (CPU)] = isnull([TTL_CUTTING (CPU)],0),
	[TTL_DIE CUT (CPU)] = isnull([TTL_DIE CUT (CPU)],0),
	[TTL_DIE-CUT (CPU)] = isnull([TTL_DIE-CUT (CPU)],0),
	[TTL_DOWN (CPU)] = isnull([TTL_DOWN (CPU)],0),
	[TTL_DOWN FILLING (CPU)] = isnull([TTL_DOWN FILLING (CPU)],0),
	[TTL_EMBOSS/DEBOSS (Price)] = isnull([TTL_EMBOSS/DEBOSS (Price)],0),
	[TTL_EMBROIDERY (Price)] = isnull([TTL_EMBROIDERY (Price)],0),
	[TTL_EMBROIDERY (STITCH in thousands)] = isnull([TTL_EMBROIDERY (STITCH in thousands)],0),
	[TTL_EYEBUTTON (CPU)] = isnull([TTL_EYEBUTTON (CPU)],0),
	[TTL_FEEDOFARM (CPU)] = isnull([TTL_FEEDOFARM (CPU)],0),
	[TTL_Fusible (CPU)] = isnull([TTL_Fusible (CPU)],0),
	[TTL_GARMENT DYE (Price)] = isnull([TTL_GARMENT DYE (Price)],0),
	[TTL_GMT WASH (Price)] = isnull([TTL_GMT WASH (Price)],0),
	[TTL_HEAT TRANSFER (CPU)] = isnull([TTL_HEAT TRANSFER (CPU)],0),
	[TTL_INSPECTION (CPU)] = isnull([TTL_INSPECTION (CPU)],0),
	[TTL_INTENSIVE MACHINE (CPU)] = isnull([TTL_INTENSIVE MACHINE (CPU)],0),
	[TTL_LASER (CPU)] = isnull([TTL_LASER (CPU)],0),
	[TTL_LASER CUTTER (CPU)] = isnull([TTL_LASER CUTTER (CPU)],0),
	[TTL_PAD PRINTING (Price)] = isnull([TTL_PAD PRINTING (Price)],0),
	[TTL_PRINTING (PCS)] = isnull([TTL_PRINTING (PCS)],0),
	[TTL_PRINTING (Price)] = isnull([TTL_PRINTING (Price)],0),
	[TTL_REAL FLATSEAM (CPU)] = isnull([TTL_REAL FLATSEAM (CPU)],0),
	[TTL_ROLLER SUBLIMATION (CPU)] = isnull([TTL_ROLLER SUBLIMATION (CPU)],0),
	[TTL_S-HOT PRESS(BONDING) (CPU)] = isnull([TTL_S-HOT PRESS(BONDING) (CPU)],0),
	[TTL_S-HOT PRESS(HT) (CPU)] = isnull([TTL_S-HOT PRESS(HT) (CPU)],0),
	[TTL_SEAM TAPING MACHINE (CPU)] = isnull([TTL_SEAM TAPING MACHINE (CPU)],0),
	[TTL_SMALL HOT PRESS (CPU)] = isnull([TTL_SMALL HOT PRESS (CPU)],0),
	[TTL_SUBLIMATION PRINT (CPU)] = isnull([TTL_SUBLIMATION PRINT (CPU)],0),
	[TTL_SUBLIMATION ROLLER (CPU)] = isnull([TTL_SUBLIMATION ROLLER (CPU)],0),
	[TTL_ULTRASONIC MACHINE (CPU)] = isnull([TTL_ULTRASONIC MACHINE (CPU)],0),
	[TTL_VELCRO MACHINE (CPU)] = isnull([TTL_VELCRO MACHINE (CPU)],0),
	[TTL_ZIG ZAG (CPU)] = isnull([TTL_ZIG ZAG (CPU)],0),
	[Sew_Qty] = isnull(Production.dbo.getMinCompleteSewQty(Orders.ID,null,null) ,0),
	[Shortage] = iif(Orders.GMTComplete = 'S', Orders.Qty - GetPulloutData.Qty, 0),
	[Buyer Key]         = convert(varchar(6),dateadd(day,-7, Orders.BuyerDelivery), 112),
	[Buyer Halfkey]		= case
						  when day(Orders.BuyerDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, Orders.BuyerDelivery),112),6) + '02'
						  when day(Orders.BuyerDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, Orders.BuyerDelivery, 112),6) + '01'
						  when day(Orders.BuyerDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, Orders.BuyerDelivery, 112),6) + '02'
						  else ''	end

From Production.dbo.Orders 
LEFT JOIN Production.dbo.Factory ON Orders.FactoryID =Factory.ID 
LEFT JOIN Production.dbo.Dropdownlist ON Orders.Category =Dropdownlist.ID and Dropdownlist.Type='Category'
LEFT JOIN Production.dbo.OrderType ON OrderType.ID = Orders.OrderTypeID and OrderType.BrandID = Orders.BrandID
LEFT JOIN Production.dbo.CDCode ON CDCode.ID = Orders.CdCodeID
outer apply (select CpuRate from Production.dbo.GetCPURate(Orders.OrderTypeID,Orders.ProgramID,Orders.Category,Orders.BrandID,'O')) as CPURate --加入
--outer apply (select * from Production.dbo.GetCurrencyRate('FX', Orders.CurrencyID, 'USD', Orders.cfmDate)) as GetCurrencyRate
outer apply (select * from Production.dbo.GetOrderAmount(orders.ID)) goa--加入
outer apply(
	select Qty = sum(ShipQty) 
	from Production.dbo.Pullout_Detail_Detail
	where OrderID=Orders.ID
)GetPulloutData
inner JOIN #atSource atSource on atSource.ID = Orders.ID
WHERE orders.Category IN ('B', 'S')
and Orders.SciDelivery between @Date_S and @Date_E
and Orders.LocalOrder <> '1'

UNION ALL

--Forecast
Select
	[StyleUkey]         = Forecast.StyleUkey,
	[Factory]           = Forecast.FactoryID,
	[FactoryCountry]    = Factory.CountryID,
	[M Division]        = Factory.MDivisionID,
	[Delivery]          = Forecast.BuyerDelivery,
	[SCIDelivery]       = Forecast.BuyerDelivery,
	[Key]               = convert(varchar(6),dateadd(day,-7, Forecast.BuyerDelivery), 112),
	[Half key]		    = 
	case
	when day(Forecast.BuyerDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, Forecast.BuyerDelivery),112),6) + '02'
	when day(Forecast.BuyerDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, Forecast.BuyerDelivery, 112),6) + '01'
	when day(Forecast.BuyerDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, Forecast.BuyerDelivery, 112),6) + '02'
	else ''	end,
	[Category]          = IIF(Forecast.ForecastCategory = 'S', ddlSampleGroup.Name, 'Bulk fc'),
	[Dev. Sample]		= IIF(OrderType.isDevSample = 1, 'Y', ''), 
	[PONO]              = '',
	[POID]              = '',
	[SPNO]              = Forecast.ID,
	[Style]             = Forecast.StyleID,
	[Season]            = Forecast.SeasonID,
	[Brand]             = Forecast.BrandID,
	[Program]           = IIF(Forecast.CheckStyle = 0, Forecast.ProgramID, Style.ProgramID),
	[CD]                = IIF(Forecast.CheckStyle = 0, Forecast.CdCodeID, Style.CdCodeID),
    [Production family] = IIF(Forecast.CheckStyle = 0, ForecastCDCode.ProductionFamilyID, StyleCDCode.ProductionFamilyID),
	[CPU]               = IIF(Forecast.CheckStyle = 0, Forecast.CPU, Style.CPU),
	[Qty]               = isnull(Forecast.Qty,0),
	[TTLCPU]            = Forecast.Qty * IIF(Forecast.CheckStyle = 0, Forecast.CPU, Style.CPU) * CPURate.CPURate, 
    [Junk]              = '', 
----ArtworkType----
	[TTL_AT (CPU)] = isnull([TTL_AT (CPU)],0),
	[TTL_AT (HAND) (CPU)] = isnull([TTL_AT (HAND) (CPU)],0),
	[TTL_AT (MACHINE) (CPU)] = isnull([TTL_AT (MACHINE) (CPU)],0),
	[TTL_AUTO-TEMPLATE (CPU)] = isnull([TTL_AUTO-TEMPLATE (CPU)],0),
	[TTL_B-HOT PRESS(BONDING) (CPU)] = isnull([TTL_B-HOT PRESS(BONDING) (CPU)],0),
	[TTL_BIG HOT PRESS (CPU)] = isnull([TTL_BIG HOT PRESS (CPU)],0),
	[TTL_BONDING (HAND) (CPU)] = isnull([TTL_BONDING (HAND) (CPU)],0),
	[TTL_BONDING (MACHINE) (CPU)] = isnull([TTL_BONDING (MACHINE) (CPU)],0),
	[TTL_CUTTING (CPU)] = isnull([TTL_CUTTING (CPU)],0),
	[TTL_DIE CUT (CPU)] = isnull([TTL_DIE CUT (CPU)],0),
	[TTL_DIE-CUT (CPU)] = isnull([TTL_DIE-CUT (CPU)],0),
	[TTL_DOWN (CPU)] = isnull([TTL_DOWN (CPU)],0),
	[TTL_DOWN FILLING (CPU)] = isnull([TTL_DOWN FILLING (CPU)],0),
	[TTL_EMBOSS/DEBOSS (Price)] = isnull([TTL_EMBOSS/DEBOSS (Price)],0),
	[TTL_EMBROIDERY (Price)] = isnull([TTL_EMBROIDERY (Price)],0),
	[TTL_EMBROIDERY (STITCH in thousands)] = isnull([TTL_EMBROIDERY (STITCH in thousands)],0),
	[TTL_EYEBUTTON (CPU)] = isnull([TTL_EYEBUTTON (CPU)],0),
	[TTL_FEEDOFARM (CPU)] = isnull([TTL_FEEDOFARM (CPU)],0),
	[TTL_Fusible (CPU)] = isnull([TTL_Fusible (CPU)],0),
	[TTL_GARMENT DYE (Price)] = isnull([TTL_GARMENT DYE (Price)],0),
	[TTL_GMT WASH (Price)] = isnull([TTL_GMT WASH (Price)],0),
	[TTL_HEAT TRANSFER (CPU)] = isnull([TTL_HEAT TRANSFER (CPU)],0),
	[TTL_INSPECTION (CPU)] = isnull([TTL_INSPECTION (CPU)],0),
	[TTL_INTENSIVE MACHINE (CPU)] = isnull([TTL_INTENSIVE MACHINE (CPU)],0),
	[TTL_LASER (CPU)] = isnull([TTL_LASER (CPU)],0),
	[TTL_LASER CUTTER (CPU)] = isnull([TTL_LASER CUTTER (CPU)],0),
	[TTL_PAD PRINTING (Price)] = isnull([TTL_PAD PRINTING (Price)],0),
	[TTL_PRINTING (PCS)] = isnull([TTL_PRINTING (PCS)],0),
	[TTL_PRINTING (Price)] = isnull([TTL_PRINTING (Price)],0),
	[TTL_REAL FLATSEAM (CPU)] = isnull([TTL_REAL FLATSEAM (CPU)],0),
	[TTL_ROLLER SUBLIMATION (CPU)] = isnull([TTL_ROLLER SUBLIMATION (CPU)],0),
	[TTL_S-HOT PRESS(BONDING) (CPU)] = isnull([TTL_S-HOT PRESS(BONDING) (CPU)],0),
	[TTL_S-HOT PRESS(HT) (CPU)] = isnull([TTL_S-HOT PRESS(HT) (CPU)],0),
	[TTL_SEAM TAPING MACHINE (CPU)] = isnull([TTL_SEAM TAPING MACHINE (CPU)],0),
	[TTL_SMALL HOT PRESS (CPU)] = isnull([TTL_SMALL HOT PRESS (CPU)],0),
	[TTL_SUBLIMATION PRINT (CPU)] = isnull([TTL_SUBLIMATION PRINT (CPU)],0),
	[TTL_SUBLIMATION ROLLER (CPU)] = isnull([TTL_SUBLIMATION ROLLER (CPU)],0),
	[TTL_ULTRASONIC MACHINE (CPU)] = isnull([TTL_ULTRASONIC MACHINE (CPU)],0),
	[TTL_VELCRO MACHINE (CPU)] = isnull([TTL_VELCRO MACHINE (CPU)],0),
	[TTL_ZIG ZAG (CPU)] = isnull([TTL_ZIG ZAG (CPU)],0),
	[Sew_Qty] = 0,
	[Shortage] = 0,
	[Buyer Key]         = convert(varchar(6),dateadd(day,-7, Forecast.BuyerDelivery), 112),
	[Buyer Halfkey]		= case
						  when day(Forecast.BuyerDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, Forecast.BuyerDelivery),112),6) + '02'
						  when day(Forecast.BuyerDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, Forecast.BuyerDelivery, 112),6) + '01'
						  when day(Forecast.BuyerDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, Forecast.BuyerDelivery, 112),6) + '02'
						  else ''	end

From #tmp_Forecast  as Forecast
LEFT JOIN Production.dbo.Style ON Forecast.BrandID=Style.BrandID and Forecast.StyleID =Style.ID and  Forecast.SeasonID = Style.SeasonID
LEFT JOIN Production.dbo.Factory ON Forecast.FactoryID =Factory.ID 
LEFT JOIN Production.dbo.Dropdownlist ddlSampleGroup ON Forecast.SampleGroup =ddlSampleGroup.ID and ddlSampleGroup.Type='ForecastSampleGroup'
LEFT JOIN Production.dbo.OrderType ON OrderType.ID = Forecast.OrderTypeID and OrderType.BrandID = Forecast.BrandID
LEFT JOIN Production.dbo.CDCode AS StyleCDCode ON StyleCDCode.ID = Style.CdCodeID
LEFT JOIN Production.dbo.CDCode AS ForecastCDCode ON ForecastCDCode.ID = Forecast.CdCodeID
inner JOIN #atSource atSource on atSource.ID = Forecast.ID
outer apply (select CpuRate from Production.dbo.GetCPURate(Forecast.OrderTypeID, Forecast.ProgramID, Forecast.Category, Forecast.BrandID, null)) as CPURate
outer apply (select cast(0 as numeric(16,4)) Price) as Price
--outer apply (select Rate from Production.dbo.GetCurrencyRate('FX', Forecast.CurrencyID, 'USD', Forecast.AddDate)) as Rate
WHERE Forecast.ForecastCategory IN ('B', 'S')
and Forecast.BuyerDelivery between @Date_S and @Date_E

UNION ALL

--FactoryOrder
Select
	[StyleUkey]         = FactoryOrder.StyleUkey,
	[Factory]           = iif(atSource.SubconInType = '-2', FactoryOrder.ProgramID, FactoryOrder.FactoryID),
	[FactoryCountry]    = iif(atSource.SubconInType = '-2', programFty.CountryID, Factory.CountryID),
	[M Division]        = iif(atSource.SubconInType = '-2', programFty.MDivisionID, Factory.MDivisionID),
	[Buyer Delivery]    = FactoryOrder.BuyerDelivery,
	[SCIDelivery]       = FactoryOrder.SCIDelivery,
	[Key]               = convert(varchar(6),dateadd(day,-7, FactoryOrder.SCIDelivery), 112),
	[Half key]		    = 
	case
	when day(FactoryOrder.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, FactoryOrder.SCIDelivery),112),6) + '02'
	when day(FactoryOrder.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, FactoryOrder.SCIDelivery, 112),6) + '01'
	when day(FactoryOrder.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, FactoryOrder.SCIDelivery, 112),6) + '02'
	else ''	end,
	[Category]          = iif(FactoryOrder.SubconInSisterFty = 0, 'Non-sister', 'Sister'),
	[Dev. Sample]		= '', 
	[PONO]              = '',
	[POID]              = FactoryOrder.OrderID,
	[SPNO]              = FactoryOrder.ID,
	[Style]             = FactoryOrder.StyleID,
	[Season]            = FactoryOrder.SeasonID,
	[Brand]             = FactoryOrder.BrandID,
	[Program]           = FactoryOrder.ProgramID,
	[CD]                = FactoryOrder.CdCodeID,
    [Production family] = CDCode.ProductionFamilyID,
	[CPU]               = FactoryOrder.CPU,
	[Qty]               = isnull(FactoryOrder.Qty,0) * iif(atSource.SubconInType = '-2', -1, 1),
	[TTLCPU]            = FactoryOrder.Qty * iif(atSource.SubconInType = '-2', -1, 1) * FactoryOrder.CPU * CPURate.CPURate, 
    [Junk]              = '', 
----ArtworkType----
	[TTL_AT (CPU)] = isnull([TTL_AT (CPU)],0),
	[TTL_AT (HAND) (CPU)] = isnull([TTL_AT (HAND) (CPU)],0),
	[TTL_AT (MACHINE) (CPU)] = isnull([TTL_AT (MACHINE) (CPU)],0),
	[TTL_AUTO-TEMPLATE (CPU)] = isnull([TTL_AUTO-TEMPLATE (CPU)],0),
	[TTL_B-HOT PRESS(BONDING) (CPU)] = isnull([TTL_B-HOT PRESS(BONDING) (CPU)],0),
	[TTL_BIG HOT PRESS (CPU)] = isnull([TTL_BIG HOT PRESS (CPU)],0),
	[TTL_BONDING (HAND) (CPU)] = isnull([TTL_BONDING (HAND) (CPU)],0),
	[TTL_BONDING (MACHINE) (CPU)] = isnull([TTL_BONDING (MACHINE) (CPU)],0),
	[TTL_CUTTING (CPU)] = isnull([TTL_CUTTING (CPU)],0),
	[TTL_DIE CUT (CPU)] = isnull([TTL_DIE CUT (CPU)],0),
	[TTL_DIE-CUT (CPU)] = isnull([TTL_DIE-CUT (CPU)],0),
	[TTL_DOWN (CPU)] = isnull([TTL_DOWN (CPU)],0),
	[TTL_DOWN FILLING (CPU)] = isnull([TTL_DOWN FILLING (CPU)],0),
	[TTL_EMBOSS/DEBOSS (Price)] = isnull([TTL_EMBOSS/DEBOSS (Price)],0),
	[TTL_EMBROIDERY (Price)] = isnull([TTL_EMBROIDERY (Price)],0),
	[TTL_EMBROIDERY (STITCH in thousands)] = isnull([TTL_EMBROIDERY (STITCH in thousands)],0),
	[TTL_EYEBUTTON (CPU)] = isnull([TTL_EYEBUTTON (CPU)],0),
	[TTL_FEEDOFARM (CPU)] = isnull([TTL_FEEDOFARM (CPU)],0),
	[TTL_Fusible (CPU)] = isnull([TTL_Fusible (CPU)],0),
	[TTL_GARMENT DYE (Price)] = isnull([TTL_GARMENT DYE (Price)],0),
	[TTL_GMT WASH (Price)] = isnull([TTL_GMT WASH (Price)],0),
	[TTL_HEAT TRANSFER (CPU)] = isnull([TTL_HEAT TRANSFER (CPU)],0),
	[TTL_INSPECTION (CPU)] = isnull([TTL_INSPECTION (CPU)],0),
	[TTL_INTENSIVE MACHINE (CPU)] = isnull([TTL_INTENSIVE MACHINE (CPU)],0),
	[TTL_LASER (CPU)] = isnull([TTL_LASER (CPU)],0),
	[TTL_LASER CUTTER (CPU)] = isnull([TTL_LASER CUTTER (CPU)],0),
	[TTL_PAD PRINTING (Price)] = isnull([TTL_PAD PRINTING (Price)],0),
	[TTL_PRINTING (PCS)] = isnull([TTL_PRINTING (PCS)],0),
	[TTL_PRINTING (Price)] = isnull([TTL_PRINTING (Price)],0),
	[TTL_REAL FLATSEAM (CPU)] = isnull([TTL_REAL FLATSEAM (CPU)],0),
	[TTL_ROLLER SUBLIMATION (CPU)] = isnull([TTL_ROLLER SUBLIMATION (CPU)],0),
	[TTL_S-HOT PRESS(BONDING) (CPU)] = isnull([TTL_S-HOT PRESS(BONDING) (CPU)],0),
	[TTL_S-HOT PRESS(HT) (CPU)] = isnull([TTL_S-HOT PRESS(HT) (CPU)],0),
	[TTL_SEAM TAPING MACHINE (CPU)] = isnull([TTL_SEAM TAPING MACHINE (CPU)],0),
	[TTL_SMALL HOT PRESS (CPU)] = isnull([TTL_SMALL HOT PRESS (CPU)],0),
	[TTL_SUBLIMATION PRINT (CPU)] = isnull([TTL_SUBLIMATION PRINT (CPU)],0),
	[TTL_SUBLIMATION ROLLER (CPU)] = isnull([TTL_SUBLIMATION ROLLER (CPU)],0),
	[TTL_ULTRASONIC MACHINE (CPU)] = isnull([TTL_ULTRASONIC MACHINE (CPU)],0),
	[TTL_VELCRO MACHINE (CPU)] = isnull([TTL_VELCRO MACHINE (CPU)],0),
	[TTL_ZIG ZAG (CPU)] = isnull([TTL_ZIG ZAG (CPU)],0),
	[Sew_Qty] = isnull(Production.dbo.getMinCompleteSewQty(FactoryOrder.ID,null,null) ,0),
	[Shortage] = 0,
	[Buyer Key]         = convert(varchar(6),dateadd(day,-7, FactoryOrder.BuyerDelivery), 112),
	[Buyer Halfkey]		= case
						  when day(FactoryOrder.BuyerDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, FactoryOrder.BuyerDelivery),112),6) + '02'
						  when day(FactoryOrder.BuyerDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, FactoryOrder.BuyerDelivery, 112),6) + '01'
						  when day(FactoryOrder.BuyerDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, FactoryOrder.BuyerDelivery, 112),6) + '02'
						  else ''	end

From #tmp_FactoryOrder FactoryOrder
LEFT JOIN Production.dbo.Style ON FactoryOrder.BrandID=Style.BrandID and FactoryOrder.StyleID =Style.ID and  FactoryOrder.SeasonID = Style.SeasonID
LEFT JOIN Production.dbo.Factory ON FactoryOrder.FactoryID = Factory.ID
LEFT JOIN Production.dbo.Factory programFty On FactoryOrder.ProgramID = programFty.ID 
LEFT JOIN Production.dbo.CDCode AS CDCode ON CDCode.ID = FactoryOrder.CdCodeID
inner JOIN #atSource atSource on atSource.ID = FactoryOrder.ID
outer apply (select 1 as CpuRate) as CPURate
outer apply (select cast(0 as numeric(16,4)) Price) as Price
WHERE FactoryOrder.Junk = 0 
and FactoryOrder.Qty > 0
and FactoryOrder.SCIDelivery between @Date_S and @Date_E
) a
 

INSERT ForecastLoadingBI
Select * 
From #tmpOrderList

------------------------------T_Factory_Tms ->  ForecastLoadingBI_Factory_Tms  ------------------------------
-- 準備資料來源
	Select *
	into #SourceA
	From(
        --Orders
		Select tmsCost.ID
		, orders.FactoryID
		, orders.MDivisionID
		, ArtworkTypeID = at.id
		, Value = case When at.ProductionUnit = 'TMS' Then isnull(orders.Qty * tms / 1400 * getCPURate.CpuRate,0)
				  When at.ArtworkUnit = 'STITCH' Then isnull(orders.Qty * tmsCost.Qty / 1000 * getCPURate.CpuRate,0)
				  When at.ProductionUnit = 'Qty' Then isnull(orders.Qty * tmsCost.Qty * getCPURate.CpuRate,0)
				  Else isnull(orders.Qty * getCPURate.CpuRate,0)
				  End
		, [Key]				= convert(varchar(6),dateadd(day,-7,Orders.SCIDelivery),112)
		, [Half key] = case
						when day(Orders.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, Orders.SCIDelivery),112),6) + '02'
						when day(Orders.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, Orders.SCIDelivery, 112),6) + '01'
						when day(Orders.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, Orders.SCIDelivery, 112),6) + '02'
						else ''	end
		From Production.dbo.Orders
		--
		Left join Production.dbo.Order_TmsCost tmsCost on tmsCost.Id = orders.ID
		Left join Production.dbo.ArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select CpuRate From Production.dbo.GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'O')) getCPURate

		WHERE Orders.Category IN ('B', 'FC', 'S') and Orders.SciDelivery between @Date_S and @Date_E
		and Orders.LocalOrder <> '1'

	UNION
        --Forecast
		Select Forecast.ID
		, Forecast.FactoryID
		, Forecast.MDivisionID
		, ArtworkTypeID = at.ID
		, Value = case When at.ProductionUnit = 'TMS' Then Forecast.Qty * tms * 1.0/ 1400 * getCPURate.CpuRate
			When at.ArtworkUnit = 'STITCH' Then  Forecast.Qty * tmsCost.Qty * 1.0/ 1000 * getCPURate.CpuRate
			When at.ProductionUnit = 'Qty' Then Forecast.Qty * tmsCost.Qty * getCPURate.CpuRate
			Else Forecast.Qty * getCPURate.CpuRate
			End
		, [Key]               =  convert(varchar(6),dateadd(day,-7, Forecast.BuyerDelivery), 112)
		, [Half key] = case
						when day(Forecast.BuyerDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, Forecast.BuyerDelivery),112),6) + '02'
						when day(Forecast.BuyerDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, Forecast.BuyerDelivery, 112),6) + '01'
						when day(Forecast.BuyerDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, Forecast.BuyerDelivery, 112),6) + '02'
						else ''	end
		From #tmp_Forecast as Forecast
		Left join Production.dbo.Style	on Forecast.BrandID=Style.BrandID and Forecast.StyleID =Style.ID and  Forecast.SeasonID = Style.SeasonID
		Left join Production.dbo.Style_TmsCost tmsCost on tmsCost.StyleUKey = Style.Ukey
		Left join Production.dbo.ArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select CpuRate From Production.dbo.GetCPURate(Forecast.OrderTypeID, Forecast.ProgramID, Forecast.ForecastCategory, Forecast.BrandID, 'S')) getCPURate
		WHERE Forecast.ForecastCategory IN ('B', 'S') and Forecast.BuyerDelivery between @Date_S and @Date_E
	UNION
		--FactoryOrder
		Select FactoryOrder.ID
		, FactoryOrder.FactoryID
		, Factory.MDivisionID
		, ArtworkTypeID = at.ID
		, Value = case When at.ProductionUnit = 'TMS' Then FactoryOrder.Qty * tms * 1.0 / 1400 * getCPURate.CpuRate
			When at.ArtworkUnit = 'STITCH' Then  FactoryOrder.Qty * tmsCost.Qty * 1.0 / 1000 * getCPURate.CpuRate
			When at.ProductionUnit = 'Qty' Then FactoryOrder.Qty * tmsCost.Qty * getCPURate.CpuRate
			Else FactoryOrder.Qty * getCPURate.CpuRate
			End
		, [Key]               =  convert(varchar(6),dateadd(day,-7, FactoryOrder.SCIDelivery), 112)
		, [Half key] = case
						when day(FactoryOrder.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, FactoryOrder.SCIDelivery),112),6) + '02'
						when day(FactoryOrder.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, FactoryOrder.SCIDelivery, 112),6) + '01'
						when day(FactoryOrder.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, FactoryOrder.SCIDelivery, 112),6) + '02'
						else ''	end
		From #tmp_FactoryOrder FactoryOrder
		Left join Production.dbo.Factory On FactoryOrder.FactoryID = Factory.ID
		Left join Production.dbo.Style	on FactoryOrder.BrandID=Style.BrandID and FactoryOrder.StyleID =Style.ID and  FactoryOrder.SeasonID = Style.SeasonID
		Left join Production.dbo.Style_TmsCost tmsCost on tmsCost.StyleUKey = Style.Ukey
		Left join Production.dbo.ArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select 1 as CpuRate ) getCPURate
		WHERE FactoryOrder.Junk = 0 and FactoryOrder.Qty > 0 and FactoryOrder.SubconInType in ('2', '3') and FactoryOrder.SCIDelivery between @Date_S and @Date_E
	UNION
		--負的FactoryOrder
		Select FactoryOrder.ID
		, FactoryID = FactoryOrder.ProgramID
		, Factory.MDivisionID
		, ArtworkTypeID = at.ID
		, Value = case When at.ProductionUnit = 'TMS' Then FactoryOrder.Qty * tms * 1.0 / 1400 * getCPURate.CpuRate
			When at.ArtworkUnit = 'STITCH' Then  FactoryOrder.Qty * tmsCost.Qty * 1.0 / 1000 * getCPURate.CpuRate
			When at.ProductionUnit = 'Qty' Then FactoryOrder.Qty * tmsCost.Qty * getCPURate.CpuRate
			Else FactoryOrder.Qty * getCPURate.CpuRate
			End * -1
		, [Key]               =  convert(varchar(6),dateadd(day,-7, FactoryOrder.SCIDelivery), 112)
		, [Half key] = case
						when day(FactoryOrder.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, FactoryOrder.SCIDelivery),112),6) + '02'
						when day(FactoryOrder.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, FactoryOrder.SCIDelivery, 112),6) + '01'
						when day(FactoryOrder.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, FactoryOrder.SCIDelivery, 112),6) + '02'
						else ''	end
		From #tmp_FactoryOrder FactoryOrder
		Left join Production.dbo.Factory On FactoryOrder.ProgramID = Factory.ID
		Left join Production.dbo.Style	on FactoryOrder.BrandID=Style.BrandID and FactoryOrder.StyleID =Style.ID and  FactoryOrder.SeasonID = Style.SeasonID
		Left join Production.dbo.Style_TmsCost tmsCost on tmsCost.StyleUKey = Style.Ukey
		Left join Production.dbo.ArtworkType at on at.id = tmsCost.ArtworkTypeID
		Outer Apply (select 1 as CpuRate ) getCPURate
		WHERE FactoryOrder.Junk = 0 and FactoryOrder.Qty > 0 and FactoryOrder.SubconInType in ('2') and FactoryOrder.SCIDelivery between @Date_S and @Date_E
	) source

select 
	 a.FactoryID
	, a.MDivisionID
	, a.ArtworkTypeID
	, a.[Key]
	, a.[Half key]
	, sumValue = sum(Value)
into #SourceB
From #SourceA a
Group by a.FactoryID, a.MDivisionID, a.ArtworkTypeID, a.[Key] , a.[Half key]

-- 建立所有的Key
Declare @KeyTable Table ([Half key] varchar(8) not null  primary key, [key] varchar(6) not null)
Declare @tmpDate DATE = @YearMonth_S
While (@tmpDate <= @YearMonth_E)
Begin
	Insert Into @KeyTable VALUES(Format(@tmpDate,'yyyyMM01'),Format(@tmpDate,'yyyyMM'))
	Insert Into @KeyTable VALUES(Format(@tmpDate,'yyyyMM02'),Format(@tmpDate,'yyyyMM'))
	Select @tmpDate = DATEADD(m, 1, @tmpDate )
End


select * into #tmpKeyTable from @KeyTable

--Insert T_Factory_Tms
--1.找所有Factory_TMS的ArtworkType與T_Order_List範圍的二大類ArtworkType
--2.Factory只顯示IsSCI = true
--3.排除Capacity(CPU)與Loading (CPU)皆為0的資料

SET @SqlCmd1 = '
INSERT ForecastLoadingBI_Factory_Tms
Select * From
(
	select Factory.mDivisionID
	, Factory.KpiCode
	, [Key] = keyTable.[Key]	
	, ArtworkTypeID = at.Id
	, [Capacity(CPU)] = SUM(iif(Right(keyTable.[Half key],2) = ''01'', isnull(GetCapacityHalf1.Capacity,0), isnull(GetCapacityHalf2.Capacity,0)))
	, [Loading (CPU)] = SUM(isnull(tmpSumValue.sumValue,0))
	, [Half key] = keyTable.[Half key]
	From Production.dbo.Factory 
	Full join Production.dbo.ArtworkType at on 1 = 1
	Full join #tmpKeyTable keyTable on 1 = 1
	Left join ['+@current_PMS_ServerName+'].Production.dbo.Factory_TMS ftms on Factory.ID = ftms.id and at.Id = ftms.ArtworkTypeID and Concat(ftms.Year, ftms.Month) = keyTable.[key] 
	Left join ['+@current_PMS_ServerName+'].Production.dbo.Factory_WorkHour fw on fw.ID = Factory.ID and fw.Year = ftms.Year and fw.Month = ftms.Month  
	outer apply (Select top 1 [StandardTMS] = StdTMS From Production.dbo.System) GetStandardTMS
	Left join #SourceB as tmpSumValue On isnull(tmpSumValue.[Half Key],'''') = keyTable.[Half Key] and tmpSumValue.FactoryID = Factory.ID and tmpSumValue.ArtworkTypeID = at.ID and tmpSumValue.MDivisionID = Factory.MDivisionID
	outer apply (Select WorkDay = fw.HalfMonth1 + fw.HalfMonth2) GetWorkingDay
	outer apply (Select Capacity = iif(at.ArtworkUnit = '''', isnull(convert(float,ftms.TMS), 0) / convert(float,GetStandardTMS.StandardTMS) * IIF(ftms.ArtworkTypeID = ''SEWING'', 3600, 60), isnull(ftms.TMS, 0))) GetCapacity
	outer apply (Select Capacity = Round(iif(GetWorkingDay.WorkDay = 0, 0, GetCapacity.Capacity * fw.HalfMonth1 / GetWorkingDay.WorkDay),6)) GetCapacityHalf1
	outer apply (Select Capacity = Round(iif(GetWorkingDay.WorkDay = 0, 0, GetCapacity.Capacity * fw.HalfMonth2 / GetWorkingDay.WorkDay),6)) GetCapacityHalf2
	Where Factory.IsSCI = 1 And at.Junk = 0
	Group by  Factory.mDivisionID, at.Id,keyTable.[Key], keyTable.[Half Key], GetStandardTMS.StandardTMS, Factory.KpiCode
)b
where b.[Capacity(CPU)] != 0 or b.[Loading (CPU)] != 0
order by b.[Key],b.mDivisionID,b.ArtworkTypeID'

EXEC sp_executesql @SqlCmd1
-- drop temp table
DROP TABLE #tmpKeyTable
DROP TABLE #ArtworkTypeList;
DROP TABLE #UseArtworkType;
DROP TABLE #atSource;
DROP TABLE #tmpOrderList;
DROP TABLE #SourceA;
DROP TABLE #SourceB;
DROP TABLE #tmp_Forecast
DROP TABLE #tmp_FactoryOrder

End

GO


