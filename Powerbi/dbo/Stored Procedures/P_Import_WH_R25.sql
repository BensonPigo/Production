
CREATE PROCEDURE [dbo].[P_Import_WH_R25]
@StartDate date,
@EndDate date
AS
BEGIN
Declare @IssueDateFromString varchar(8) = Format(@StartDate, 'yyyyMMdd')
Declare @IssueDateToString varchar(8) = Format(@EndDate, 'yyyyMMdd')

SELECT *
INTO #tmpP_ImportScheduleList
FROM P_ImportScheduleList
WHERE 1 = 0

DECLARE @SqlCmd NVARCHAR(MAX) = '
INSERT INTO #tmpP_ImportScheduleList
SELECT *
FROM OPENQUERY([MainServer], ''
SET NOCOUNT ON;
SELECT * FROM production.dbo.Warehouse_Report_R25 (
    1
    ,'''''+ @IssueDateFromString +'''''
    ,'''''+ @IssueDateToString +'''''
    ,NULL--@WK1
    ,NULL--@WK2
    ,NULL--@POID1
    ,NULL--@POID2
    ,NULL--@SuppID
    ,NULL--@FabricType
    ,NULL--@WhseArrival1
    ,NULL--@WhseArrival2
    ,NULL--@Eta1
    ,NULL--@Eta2
    ,NULL--@BrandID
    ,NULL--@MDivisionID
    ,NULL--@FactoryID
    ,NULL--@KPILETA1
    ,NULL--@KPILETA2
    ,0--@RecLessArv
    )
    ''
)
'
EXEC sp_executesql @SqlCmd

DELETE p
FROM P_ImportScheduleList p
WHERE ((AddDate >= @StartDate AND AddDate <= @EndDate)
    OR (EditDate >= @StartDate AND EditDate <= @EndDate))
AND NOT EXISTS (SELECT 1 FROM #tmpP_ImportScheduleList WHERE ExportDetailUkey = p.ExportDetailUkey)

UPDATE P
SET 
     [WK]                   = t.[WK]
    ,[ETA]                  = t.[ETA]
    ,[MDivisionID]          = t.[MDivisionID]
    ,[FactoryID]            = t.[FactoryID]
    ,[Consignee]            = t.[Consignee]
    ,[ShipModeID]           = t.[ShipModeID]
    ,[CYCFS]                = t.[CYCFS]
    ,[Blno]                 = t.[Blno]
    ,[Packages]             = t.[Packages]
    ,[Vessel]               = t.[Vessel]
    ,[ProdFactory]          = t.[ProdFactory]
    ,[OrderTypeID]          = t.[OrderTypeID]
    ,[ProjectID]            = t.[ProjectID]
    ,[Category]             = t.[Category]
    ,[BrandID]              = t.[BrandID]
    ,[SeasonID]             = t.[SeasonID]
    ,[StyleID]              = t.[StyleID]
    ,[StyleName]            = t.[StyleName]
    ,[PoID]                 = t.[PoID]
    ,[Seq]                  = t.[Seq]
    ,[Refno]                = t.[Refno]
    ,[Color]                = t.[Color]
    ,[ColorName]            = t.[ColorName]
    ,[Description]          = t.[Description]
    ,[MtlType]              = t.[MtlType]
    ,[SubMtlType]           = t.[SubMtlType]
    ,[WeaveType]            = t.[WeaveType]
    ,[SuppID]               = t.[SuppID]
    ,[SuppName]             = t.[SuppName]
    ,[UnitID]               = t.[UnitID]
    ,[SizeSpec]             = t.[SizeSpec]
    ,[ShipQty]              = t.[ShipQty]
    ,[FOC]                  = t.[FOC]
    ,[NetKg]                = t.[NetKg]
    ,[WeightKg]             = t.[WeightKg]
    ,[ArriveQty]            = t.[ArriveQty]
    ,[ArriveQtyStockUnit]   = t.[ArriveQtyStockUnit]
    ,[FirstBulkSewInLine]   = t.[FirstBulkSewInLine]
    ,[FirstCutDate]         = t.[FirstCutDate]
    ,[ReceiveQty]           = t.[ReceiveQty]
    ,[TotalRollsCalculated] = t.[TotalRollsCalculated]
    ,[StockUnit]            = t.[StockUnit]
    ,[MCHandle]             = t.[MCHandle]
    ,[ContainerType]        = t.[ContainerType]
    ,[ContainerNo]          = t.[ContainerNo]
    ,[PortArrival]          = t.[PortArrival]
    ,[WhseArrival]          = t.[WhseArrival]
    ,[KPILETA]              = t.[KPILETA]
    ,[PFETA]                = t.[PFETA]
    ,[EarliestSCIDelivery]  = t.[EarliestSCIDelivery]
    ,[EarlyDays]            = t.[EarlyDays]
    ,[EarliestPFETA]        = t.[EarliestPFETA]
    ,[MRMail]               = t.[MRMail]
    ,[SMRMail]              = t.[SMRMail]
    ,[EditName]             = t.[EditName]
    ,[AddDate]              = t.[AddDate]
    ,[EditDate]             = t.[EditDate]
FROM P_ImportScheduleList P
INNER JOIN #tmpP_ImportScheduleList t ON t.ExportDetailUkey = P.ExportDetailUkey

INSERT P_ImportScheduleList
SELECT *
FROM #tmpP_ImportScheduleList t
WHERE NOT EXISTS(SELECT 1 FROM P_ImportScheduleList WHERE ExportDetailUkey = t.ExportDetailUkey)

if exists(select 1 from BITableInfo where Id = 'P_ImportScheduleList')
begin
	update BITableInfo set TransferDate = getdate()
	where Id = 'P_ImportScheduleList'
end
else
begin
	insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_ImportScheduleList', GETDATE(), 0)
end

END