﻿using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_PPICMasterListBIData
    {
        /// <inheritdoc/>
        public Base_ViewModel P_PPICMasterListBIData(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R03 biModel = new PPIC_R03();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-60).ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R03_ViewModel ppic_R03_ViewModel = new PPIC_R03_ViewModel()
                {
                    IsPowerBI = true,
                    BuyerDelivery1 = sDate,
                    SciDelivery1 = sDate,
                    IncludeHistoryOrder = true,
                    IncludeArtworkData = true,
                    PrintingDetail = false,
                    ByCPU = false,
                    IncludeArtworkDataKindIsPAP = false,
                    SeparateByQtyBdownByShipmode = false,
                    Bulk = true,
                    Sample = true,
                    Material = false,
                    Forecast = true,
                    Garment = true,
                    SMTL = false,
                    ListPOCombo = false,
                    IncludeCancelOrder = true,
                };

                Base_ViewModel resultReport = biModel.GetPPICMasterList(ppic_R03_ViewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dt_P_PPICMASTERLIST = resultReport.DtArr[0];
                DataTable dt_P_PPICMasterList_ArtworkType = resultReport.DtArr[1];

                // insert into PowerBI
                finalResult = this.UpdateBIData_P_PPICMASTERLIST(dt_P_PPICMASTERLIST, sDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = this.UpdateBIData_P_PPICMasterList_Extend(dt_P_PPICMasterList_ArtworkType);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData_P_PPICMASTERLIST(DataTable dt, DateTime sDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@sDate", sDate),
                };
                string sql = @"	
select [M] = b.MDivisionID
	, [FactoryID] = b.FactoryID
	, [Delivery] = b.BuyerDelivery
	, [Delivery(YYYYMM)] = FORMAT(b.BuyerDelivery, 'yyyyMM')
	, [Earliest SCIDlv] = b.EarliestSCIDlv
	, [SCIDlv] = b.SciDelivery
	, [KEY] = case when CAST(FORMAT(b.SciDelivery, 'dd') as int) <= 7 then FORMAT(DATEADD(MONTH, -1, b.SciDelivery), 'yyyyMM') else FORMAT(b.SciDelivery, 'yyyyMM') end
	, [IDD] = b.IDD
	, [CRD] = b.CRDDate
	, [CRD(YYYYMM)] = FORMAT(b.CRDDate, 'yyyyMM')
	, [Check CRD] = case when b.BuyerDelivery is null or b.CRDDate is null then 'Y'
						when b.BuyerDelivery <> b.CRDDate then 'Y' 
						else '' end
	, [OrdCFM] = b.CFMDate
	, [CRD-OrdCFM] = case when b.CRDDate is null or b.CFMDate is null then 0 else DATEDIFF(day, b.CRDDate, b.CFMDate) end
	, [SPNO] = b.ID
	, [Category] = b.Category
	, [Est. download date] = case when b.isForecast = '' then '' else b.BuyMonth end
	, [Buy Back] = b.BuyBack
	, [Cancelled] = case when b.Junk = 1 then 'Y' else '' end
	, [NeedProduction] = b.Cancelled
	, [Dest] = b.Dest
	, [Style] = b.StyleID
	, [Style Name] = b.StyleName
	, [Modular Parent] = b.ModularParent
	, [CPUAdjusted] = b.CPUAdjusted
	, [Similar Style] = b.SimilarStyle
	, [Season] = b.SeasonID
	, [Garment L/T] = b.GMTLT
	, [Order Type] = b.OrderTypeID
	, [Project] = b.ProjectID
	, [PackingMethod] = b.PackingMethod
	, [Hanger pack] = b.HangerPack
	, [Order#] = b.Customize1
	, [Buy Month] = case when b.isForecast = '' then b.BuyMonth else '' end
	, [PONO] = b.CustPONo
	, [VAS/SHAS] = case when b.VasShas = 1 then 'Y' else '' end
	, [VAS/SHAS Apv.] = b.MnorderApv2
	, [VAS/SHAS Cut Off Date] = b.VasShasCutOffDate
	, [M/Notice Date] = b.MnorderApv
	, [Est M/Notice Apv.] = b.KPIMNotice
	, [Tissue] = case when b.TissuePaper =1 then 'Y' else '' end 
	, [AF by adidas] = b.AirFreightByBrand
	, [Factory Disclaimer] = b.FactoryDisclaimer
	, [Factory Disclaimer Remark] = b.FactoryDisclaimerRemark
	, [Approved/Rejected Date] = b.ApprovedRejectedDate
	, [Global Foundation Range] = case when b.GFR = 1 then 'Y' else '' end
	, [Brand] = b.BrandID
	, [Cust CD] = b.CustCDID
	, [KIT] = b.Kit
	, [Fty Code] = b.BrandFTYCode
	, [Program] = b.ProgramID
	, [Non Revenue] = b.NonRevenue
	, [New CD Code] = b.CDCodeNew
	, [ProductType] = b.ProductType
	, [FabricType] = b.FabricType
	, [Lining] = b.Lining
	, [Gender] = b.Gender
	, [Construction] = b.Construction
	, [Cpu] = b.CPU
	, [Qty] = b.Qty
	, [FOC Qty] = b.FOCQty
	, [Total CPU] = b.CPU * b.Qty * b.CPUFactor
	, [SewQtyTop] = b.SewQtyTop
	, [SewQtyBottom] = b.SewQtyBottom
	, [SewQtyInner] = b.SewQtyInner
	, [SewQtyOuter] = b.SewQtyOuter
	, [Total Sewing Output] = b.TtlSewQty
	, [Cut Qty] = b.CutQty
	, [By Comb] = case when b.WorkType = 1 then 'Y' else '' end
	, [Cutting Status] = case when b.CutQty >= b.Qty then 'Y' else '' end
	, [Packing Qty] = b.PackingQty
	, [Packing FOC Qty] = b.PackingFOCQty
 	, [Booking Qty] = b.BookingQty
	, [FOC Adj Qty] = b.FOCAdjQty
	, [Not FOC Adj Qty] = b.NotFOCAdjQty -- 73
	, [Total] = b.Qty * b.PoPrice
	, [KPI L/ETA] = b.KPILETA  --BG 76
	, [PF ETA (SP)] = b.PFETA
	, [Pull Forward Remark] = b.PFRemark
	, [Pack L/ETA] = b.PackLETA
	, [SCHD L/ETA] = b.LETA
	, [Actual Mtl. ETA] = b.MTLETA
	, [Fab ETA] = b.Fab_ETA
	, [Acc ETA] = b.Acc_ETA
	, [Sewing Mtl Complt(SP)] = b.SewingMtlComplt
	, [Packing Mtl Complt(SP)] = b.PackingMtlComplt
	, [Sew. MTL ETA (SP)] = b.SewETA
	, [Pkg. MTL ETA (SP)] = b.PackETA
	, [MTL Delay] = case when b.MTLDelay = 1 then 'Y' else '' end
	, [MTL Cmplt] = case when isnull(b.MTLExport, '') = '' then b.MTLExportTimes else b.MTLExport end
	, [MTL Cmplt (SP)] = case when b.MTLComplete = 1 then 'Y' else '' end
	, [Arrive W/H Date] = b.ArriveWHDate
	, [Sewing InLine] =  b.SewInLine
	, [Sewing OffLine] = b.SewOffLine
	, [1st Sewn Date] = b.FirstOutDate
	, [Last Sewn Date] = b.LastOutDate
	, [First Production Date] = b.FirstProduction
	, [Last Production Date] = b.LastProductionDate
	, [Each Cons Apv Date] = b.EachConsApv
	, [Est Each Con Apv.] = b.KpiEachConsCheck
	, [Cutting InLine] = b.CutInLine
	, [Cutting OffLine] = b.CutOffLine
	, [Cutting InLine(SP)] = b.CutInLine_SP
	, [Cutting OffLine(SP)] = b.CutOffLine_SP
	, [1st Cut Date] = b.FirstCutDate
	, [Last Cut Date] = b.LastCutDate
	, [Est. Pullout] = b.PulloutDate
	, [Act. Pullout Date] = b.ActPulloutDate
	, [Pullout Qty] = b.PulloutQty
	, [Act. Pullout Times] = b.ActPulloutTime
	, [Act. Pullout Cmplt] = case when b.PulloutComplete = 1 then 'OK' else '' end
	, [KPI Delivery Date] = b.FtyKPI
	, [Update Delivery Reason] = case when ISNULL(b.KPIChangeReason, '') = '' then '' else concat(b.KPIChangeReason, '-', b.KPIChangeReasonName) end
	, [Plan Date] = b.PlanDate
	, [Original Buyer Delivery Date] = b.OrigBuyerDelivery
	, [SMR] = b.SMR
	, [SMR Name] = b.SMRName
	, [Handle] = b.MRHandle
	, [Handle Name] = b.MRHandleName
	, [Posmr] = b.POSMR
	, [Posmr Name] = b.POSMRName
	, [PoHandle] = b.POHandle
	, [PoHandle Name] = b.POHandleName
	, [PCHandle] = b.PCHandle
	, [PCHandle Name] = b.PCHandleName
	, [MCHandle] = b.MCHandle
	, [MCHandle Name] = b.MCHandleName
	, [DoxType] = b.DoxType
	, [Packing CTN] = b.PackingCTN
	, [TTLCTN] = b.TotalCTN
	, [Pack Error CTN] = b.PackErrorCTN
	, [FtyCTN] = b.FtyCtn
	, [cLog CTN] = b.ClogCTN
	, [CFA CTN] = b.CFACTN
	, [cLog Rec. Date] = b.ClogRcvDate
	, [Final Insp. Date] = b.InspDate
	, [Insp. Result] = b.InspResult
	, [CFA Name] = b.InspHandle
	, [Sewing Line#] = b.SewLine
	, [ShipMode] = b.ShipModeList
	, [SI#] = b.Customize2
	, [ColorWay] = b.Article
	, [Special Mark] = b.SpecialMarkName
	, [Fty Remark] = b.FTYRemark
	, [Sample Reason] = b.SampleReasonName
	, [IS MixMarker] = b.IsMixMarker
	, [Cutting SP] = b.CuttingSP
	, [Rainwear test] = case when b.RainwearTestPassed = 1 then 'Y' else '' end
	, b.[TMS]
	, [MD room scan date] = b.LastCTNTransDate
	, [Dry Room received date] = b.LastCTNRecdDate
	, [Dry room trans date] = b.DryRoomRecdDate
	, [Last ctn trans date] = b.DryRoomTransDate
	, [Last scan and pack date] = b.ScanEditDate
	, [Last ctn recvd date] = b.MdRoomScanDate
	, [OrganicCotton] = b.OrganicCotton
	, [Direct Ship] = b.DirectShip		
	, [StyleCarryover] = b.StyleCarryover
	, [SCHDL/ETA(SP)] = b.[Max_ScheETAbySP]
	, [SewingMtlETA(SPexclRepl)] = b.[Sew_ScheETAnoReplace]
	, [ActualMtlETA(exclRepl)] = b.[MaxShipETA_Exclude5x]
	, b.[HalfKey]			
	, b.[DevSample]
	, b.[POID]
	, b.[KeepPanels]
	, b.[BuyBackReason]
	, b.[SewQtybyRate]
	, [Unit] = b.[StyleUnit]
	, b.[SubconInType]
	, b.[Article]
	, b.[ProduceRgPMS]
	, b.[Buyerhalfkey]
	, [Country] = b.DestAlias
	, b.[Third_Party_Insepction]
	, b.[ColorID]
into #tmpReName
from #tmp b 

update p 
	set p.[M] = ISNULL(t.[M], '')
		, p.[FactoryID] = ISNULL(t.[FactoryID], '')
		, p.[Delivery] = t.[Delivery]
		, p.[Delivery(YYYYMM)] = ISNULL(t.[Delivery(YYYYMM)], '')
		, p.[Earliest SCIDlv] = t.[Earliest SCIDlv]
		, p.[SCIDlv] = t.[SCIDlv]
		, p.[KEY] = ISNULL(t.[KEY], '')
		, p.[IDD] = ISNULL(t.[IDD], '')
		, p.[CRD] = t.[CRD]
		, p.[CRD(YYYYMM)] = ISNULL(t.[CRD(YYYYMM)], '')
		, p.[Check CRD] = ISNULL(t.[Check CRD], '')
		, p.[OrdCFM] = t.[OrdCFM]
		, p.[CRD-OrdCFM] = ISNULL(t.[CRD-OrdCFM], 0)
		, p.[SPNO] = ISNULL(t.[SPNO], '')
		, p.[Category] = ISNULL(t.[Category], '')
		, p.[Est. download date] = ISNULL(t.[Est. download date], '')
		, p.[Buy Back] = ISNULL(t.[Buy Back], '')
		, p.[Cancelled] = ISNULL(t.[Cancelled], '')
		, p.[NeedProduction] = ISNULL(t.[NeedProduction], '')
		, p.[Dest] = ISNULL(t.[Dest], '')
		, p.[Style] = ISNULL(t.[Style], '')
		, p.[Style Name] = ISNULL(t.[Style Name], '')
		, p.[Modular Parent] = ISNULL(t.[Modular Parent], '')
		, p.[CPUAdjusted] = ISNULL(t.[CPUAdjusted], 0)
		, p.[Similar Style] = ISNULL(t.[Similar Style], '')
		, p.[Season] = ISNULL(t.[Season], '')
		, p.[Garment L/T] = ISNULL(t.[Garment L/T], 0)
		, p.[Order Type] = ISNULL(t.[Order Type], '')
		, p.[Project] = ISNULL(t.[Project], '')
		, p.[PackingMethod] = ISNULL(t.[PackingMethod], '')
		, p.[Hanger pack] = ISNULL(t.[Hanger pack], 0)
		, p.[Order#] = ISNULL(t.[Order#], '')
		, p.[Buy Month] = ISNULL(t.[Buy Month], '')
		, p.[PONO] = ISNULL(t.[PONO], '')
		, p.[VAS/SHAS] = ISNULL(t.[VAS/SHAS], '')
		, p.[VAS/SHAS Apv.] = t.[VAS/SHAS Apv.]
		, p.[VAS/SHAS Cut Off Date] = t.[VAS/SHAS Cut Off Date]
		, p.[M/Notice Date] = t.[M/Notice Date]
		, p.[Est M/Notice Apv.] = t.[Est M/Notice Apv.]
		, p.[Tissue] = ISNULL(t.[Tissue], '')
		, p.[AF by adidas] = ISNULL(t.[AF by adidas], '')
		, p.[Factory Disclaimer] = ISNULL(t.[Factory Disclaimer], '')
		, p.[Factory Disclaimer Remark] = ISNULL(t.[Factory Disclaimer Remark], '')
		, p.[Approved/Rejected Date] = t.[Approved/Rejected Date]
		, p.[Global Foundation Range] = ISNULL(t.[Global Foundation Range], '')
		, p.[Brand] = ISNULL(t.[Brand], '')
		, p.[Cust CD] = ISNULL(t.[Cust CD], '')
		, p.[KIT] = ISNULL(t.[KIT], '')
		, p.[Fty Code] = ISNULL(t.[Fty Code], '')
		, p.[Program] = ISNULL(t.[Program], '')
		, p.[Non Revenue] = ISNULL(t.[Non Revenue], '')
		, p.[New CD Code] = ISNULL(t.[New CD Code], '')
		, p.[ProductType] = ISNULL(t.[ProductType], '')
		, p.[FabricType] = ISNULL(t.[FabricType], '')
		, p.[Lining] = ISNULL(t.[Lining], '')
		, p.[Gender] = ISNULL(t.[Gender], '')
		, p.[Construction] = ISNULL(t.[Construction], '')
		, p.[Cpu] = ISNULL(t.[Cpu], 0)
		, p.[Qty] = ISNULL(t.[Qty], 0)
		, p.[FOC Qty] = ISNULL(t.[FOC Qty], 0)
		, p.[Total CPU] = ISNULL(t.[Total CPU], 0)
		, p.[SewQtyTop] = ISNULL(t.[SewQtyTop], 0)
		, p.[SewQtyBottom] = ISNULL(t.[SewQtyBottom], 0)
		, p.[SewQtyInner] = ISNULL(t.[SewQtyInner], 0)
		, p.[SewQtyOuter] = ISNULL(t.[SewQtyOuter], 0)
		, p.[Total Sewing Output] = ISNULL(t.[Total Sewing Output], 0)
		, p.[Cut Qty] = ISNULL(t.[Cut Qty], 0)
		, p.[By Comb] = ISNULL(t.[By Comb], '')
		, p.[Cutting Status] = ISNULL(t.[Cutting Status], '')
		, p.[Packing Qty] = ISNULL(t.[Packing Qty], 0)
		, p.[Packing FOC Qty] = ISNULL(t.[Packing FOC Qty], 0)
		, p.[Booking Qty] = ISNULL(t.[Booking Qty], 0)
		, p.[FOC Adj Qty] = ISNULL(t.[FOC Adj Qty], 0)
		, p.[Not FOC Adj Qty] = ISNULL(t.[Not FOC Adj Qty], 0)
		, p.[Total] = ISNULL(t.[Total], 0)
		, p.[KPI L/ETA] = t.[KPI L/ETA]
		, p.[PF ETA (SP)] = t.[PF ETA (SP)]
		, p.[Pull Forward Remark] = ISNULL(t.[Pull Forward Remark], '')
		, p.[Pack L/ETA] = t.[Pack L/ETA]
		, p.[SCHD L/ETA] = t.[SCHD L/ETA]
		, p.[Actual Mtl. ETA] = t.[Actual Mtl. ETA]
		, p.[Fab ETA] = t.[Fab ETA]
		, p.[Acc ETA] = t.[Acc ETA]
		, p.[Sewing Mtl Complt(SP)] = ISNULL(t.[Sewing Mtl Complt(SP)], '')
		, p.[Packing Mtl Complt(SP)] = ISNULL(t.[Packing Mtl Complt(SP)], '')
		, p.[Sew. MTL ETA (SP)] = t.[Sew. MTL ETA (SP)]
		, p.[Pkg. MTL ETA (SP)] = t.[Pkg. MTL ETA (SP)]
		, p.[MTL Delay] = ISNULL(t.[MTL Delay], '')
		, p.[MTL Cmplt] = ISNULL(t.[MTL Cmplt], '')
		, p.[MTL Cmplt (SP)] = ISNULL(t.[MTL Cmplt (SP)], '')
		, p.[Arrive W/H Date] = t.[Arrive W/H Date]
		, p.[Sewing InLine] = t.[Sewing InLine]
		, p.[Sewing OffLine] = t.[Sewing OffLine]
		, p.[1st Sewn Date] = t.[1st Sewn Date]
		, p.[Last Sewn Date] = t.[Last Sewn Date]
		, p.[First Production Date] = t.[First Production Date]
		, p.[Last Production Date] = t.[Last Production Date]
		, p.[Each Cons Apv Date] = t.[Each Cons Apv Date]
		, p.[Est Each Con Apv.] = t.[Est Each Con Apv.]
		, p.[Cutting InLine] = t.[Cutting InLine]
		, p.[Cutting OffLine] = t.[Cutting OffLine]
		, p.[Cutting InLine(SP)] = t.[Cutting InLine(SP)]
		, p.[Cutting OffLine(SP)] = t.[Cutting OffLine(SP)]
		, p.[1st Cut Date] = t.[1st Cut Date]
		, p.[Last Cut Date] = t.[Last Cut Date]
		, p.[Est. Pullout] = t.[Est. Pullout]
		, p.[Act. Pullout Date] = t.[Act. Pullout Date]
		, p.[Pullout Qty] = ISNULL(t.[Pullout Qty], 0)
		, p.[Act. Pullout Times] = ISNULL(t.[Act. Pullout Times], 0)
		, p.[Act. Pullout Cmplt] = ISNULL(t.[Act. Pullout Cmplt], '')
		, p.[KPI Delivery Date] = t.[KPI Delivery Date]
		, p.[Update Delivery Reason] = ISNULL(t.[Update Delivery Reason], '')
		, p.[Plan Date] = t.[Plan Date]
		, p.[Original Buyer Delivery Date] = t.[Original Buyer Delivery Date]
		, p.[SMR] = ISNULL(t.[SMR], '')
		, p.[SMR Name] = ISNULL(t.[SMR Name], '')
		, p.[Handle] = ISNULL(t.[Handle], '')
		, p.[Handle Name] = ISNULL(t.[Handle Name], '')
		, p.[Posmr] = ISNULL(t.[Posmr], '')
		, p.[Posmr Name] = ISNULL(t.[Posmr Name], '')
		, p.[PoHandle] = ISNULL(t.[PoHandle], '')
		, p.[PoHandle Name] = ISNULL(t.[PoHandle Name], '')
		, p.[PCHandle] = ISNULL(t.[PCHandle], '')
		, p.[PCHandle Name] = ISNULL(t.[PCHandle Name], '')
		, p.[MCHandle] = ISNULL(t.[MCHandle], '')
		, p.[MCHandle Name] = ISNULL(t.[MCHandle Name], '')
		, p.[DoxType] = ISNULL(t.[DoxType], '')
		, p.[Packing CTN] = ISNULL(t.[Packing CTN], 0)
		, p.[TTLCTN] = ISNULL(t.[TTLCTN], 0)
		, p.[Pack Error CTN] = ISNULL(t.[Pack Error CTN], 0)
		, p.[FtyCTN] = ISNULL(t.[FtyCTN], 0)
		, p.[cLog CTN] = ISNULL(t.[cLog CTN], 0)
		, p.[CFA CTN] = ISNULL(t.[CFA CTN], 0)
		, p.[cLog Rec. Date] = t.[cLog Rec. Date]
		, p.[Final Insp. Date] = t.[Final Insp. Date]
		, p.[Insp. Result] = ISNULL(t.[Insp. Result], '')
		, p.[CFA Name] = ISNULL(t.[CFA Name], '')
		, p.[Sewing Line#] = ISNULL(t.[Sewing Line#], '')
		, p.[ShipMode] = ISNULL(t.[ShipMode], '')
		, p.[SI#] = ISNULL(t.[SI#], '')
		, p.[ColorWay] = ISNULL(t.[ColorWay], '')
		, p.[Special Mark] = ISNULL(t.[Special Mark], '')
		, p.[Fty Remark] = ISNULL(t.[Fty Remark], '')
		, p.[Sample Reason] = ISNULL(t.[Sample Reason], '')
		, p.[IS MixMarker] = ISNULL(t.[IS MixMarker], '')
		, p.[Cutting SP] = ISNULL(t.[Cutting SP], '')
		, p.[Rainwear test] = ISNULL(t.[Rainwear test], '')
		, p.[TMS] = ISNULL(t.[TMS], 0)
		, p.[MD room scan date] = t.[MD room scan date]
		, p.[Dry Room received date] = t.[Dry Room received date]
		, p.[Dry room trans date] = t.[Dry room trans date]
		, p.[Last ctn trans date] = t.[Last ctn trans date]
		, p.[Last Scan And Pack Date] = t.[Last Scan And Pack Date]
		, p.[Last ctn recvd date] = t.[Last ctn recvd date]
		, p.[OrganicCotton] = ISNULL(t.[OrganicCotton], '')
		, p.[Direct Ship] = ISNULL(t.[Direct Ship], '')
		, p.[StyleCarryover] = ISNULL(t.[StyleCarryover], '')
		, p.[SCHDL/ETA(SP)] = t.[SCHDL/ETA(SP)]
		, p.[SewingMtlETA(SPexclRepl)] = t.[SewingMtlETA(SPexclRepl)]
		, p.[ActualMtlETA(exclRepl)] = t.[ActualMtlETA(exclRepl)]
		, p.[HalfKey] = ISNULL(t.[HalfKey], '')
		, p.[DevSample] = ISNULL(t.[DevSample], '')
		, p.[POID] = ISNULL(t.[POID], '')
		, p.[KeepPanels] = ISNULL(t.[KeepPanels], '')
		, p.[BuyBackReason] = ISNULL(t.[BuyBackReason], '')
		, p.[SewQtybyRate] = ISNULL(t.[SewQtybyRate], 0)
		, p.[Unit] = ISNULL(t.[Unit], '')
		, p.[SubconInType] = ISNULL(t.[SubconInType], '')
		, p.[Article] = ISNULL(t.[Article], '')
		, p.[ProduceRgPMS] = ISNULL(t.[ProduceRgPMS], '')
		, p.[Buyerhalfkey] = ISNULL(t.[Buyerhalfkey], '')
		, p.[Country] = ISNULL(t.[Country], '')
		, P.[Third_Party_Insepction] = ISNULL(t.[Third_Party_Insepction], '')
		, p.[ColorID] = ISNULL(t.[ColorID], '')
from P_PPICMASTERLIST p 
inner join #tmpReName t on p.[SPNO] = t.[SPNO]

insert into P_PPICMASTERLIST([M], [FactoryID], [Delivery], [Delivery(YYYYMM)], [Earliest SCIDlv], [SCIDlv], [KEY], [IDD]
	, [CRD], [CRD(YYYYMM)], [Check CRD], [OrdCFM], [CRD-OrdCFM], [SPNO], [Category], [Est. download date], [Buy Back]
	, [Cancelled], [NeedProduction], [Dest], [Style], [Style Name], [Modular Parent], [CPUAdjusted], [Similar Style]
	, [Season], [Garment L/T], [Order Type], [Project], [PackingMethod], [Hanger pack], [Order#], [Buy Month], [PONO]
	, [VAS/SHAS], [VAS/SHAS Apv.], [VAS/SHAS Cut Off Date], [M/Notice Date], [Est M/Notice Apv.], [Tissue], [AF by adidas]
	, [Factory Disclaimer], [Factory Disclaimer Remark], [Approved/Rejected Date], [Global Foundation Range], [Brand], [Cust CD]
	, [KIT], [Fty Code], [Program], [Non Revenue], [New CD Code], [ProductType], [FabricType], [Lining], [Gender], [Construction]
	, [Cpu], [Qty], [FOC Qty], [Total CPU], [SewQtyTop], [SewQtyBottom], [SewQtyInner], [SewQtyOuter], [Total Sewing Output]
	, [Cut Qty], [By Comb], [Cutting Status], [Packing Qty], [Packing FOC Qty], [Booking Qty], [FOC Adj Qty], [Not FOC Adj Qty]
	, [Total], [KPI L/ETA], [PF ETA (SP)], [Pull Forward Remark], [Pack L/ETA], [SCHD L/ETA], [Actual Mtl. ETA], [Fab ETA]
	, [Acc ETA], [Sewing Mtl Complt(SP)], [Packing Mtl Complt(SP)], [Sew. MTL ETA (SP)], [Pkg. MTL ETA (SP)], [MTL Delay], [MTL Cmplt]
	, [MTL Cmplt (SP)], [Arrive W/H Date], [Sewing InLine], [Sewing OffLine], [1st Sewn Date], [Last Sewn Date], [First Production Date]
	, [Last Production Date], [Each Cons Apv Date], [Est Each Con Apv.], [Cutting InLine], [Cutting OffLine], [Cutting InLine(SP)]
	, [Cutting OffLine(SP)], [1st Cut Date], [Last Cut Date], [Est. Pullout], [Act. Pullout Date], [Pullout Qty], [Act. Pullout Times]
	, [Act. Pullout Cmplt], [KPI Delivery Date], [Update Delivery Reason], [Plan Date], [Original Buyer Delivery Date], [SMR], [SMR Name]
	, [Handle], [Handle Name], [Posmr], [Posmr Name], [PoHandle], [PoHandle Name], [PCHandle], [PCHandle Name], [MCHandle], [MCHandle Name]
	, [DoxType], [Packing CTN], [TTLCTN], [Pack Error CTN], [FtyCTN], [cLog CTN], [CFA CTN], [cLog Rec. Date], [Final Insp. Date]
	, [Insp. Result], [CFA Name], [Sewing Line#], [ShipMode], [SI#], [ColorWay], [Special Mark], [Fty Remark], [Sample Reason], [IS MixMarker]
	, [Cutting SP], [Rainwear test], [TMS], [MD room scan date], [Dry Room received date], [Dry room trans date], [Last ctn trans date]
	, [Last Scan And Pack Date], [Last ctn recvd date], [OrganicCotton], [Direct Ship], [StyleCarryover], [SCHDL/ETA(SP)], [SewingMtlETA(SPexclRepl)]
	, [ActualMtlETA(exclRepl)], [HalfKey], [DevSample], [POID], [KeepPanels], [BuyBackReason], [SewQtybyRate], [Unit], [SubconInType]
	, [Article], [ProduceRgPMS], [Buyerhalfkey], [Country],[Third_Party_Insepction],[ColorID])
select ISNULL(t.[M], '')
	, ISNULL(t.[FactoryID], '')
	, [Delivery]
	, ISNULL(t.[Delivery(YYYYMM)], '')
	, [Earliest SCIDlv]
	, [SCIDlv]
	, ISNULL(t.[KEY], '')
	, ISNULL(t.[IDD], '')
	, [CRD]
	, ISNULL(t.[CRD(YYYYMM)], '')
	, ISNULL(t.[Check CRD], '')
	, [OrdCFM]
	, ISNULL(t.[CRD-OrdCFM], 0)
	, ISNULL(t.[SPNO], '')
	, ISNULL(t.[Category], '')
	, ISNULL(t.[Est. download date], '')
	, ISNULL(t.[Buy Back], '')
	, ISNULL(t.[Cancelled], '')
	, ISNULL(t.[NeedProduction], '')
	, ISNULL(t.[Dest], '')
	, ISNULL(t.[Style], '')
	, ISNULL(t.[Style Name], '')
	, ISNULL(t.[Modular Parent], '')
	, ISNULL(t.[CPUAdjusted], 0)
	, ISNULL(t.[Similar Style], '')
	, ISNULL(t.[Season], '')
	, ISNULL(t.[Garment L/T], 0)
	, ISNULL(t.[Order Type], '')
	, ISNULL(t.[Project], '')
	, ISNULL(t.[PackingMethod], '')
	, ISNULL(t.[Hanger pack], 0)
	, ISNULL(t.[Order#], '')
	, ISNULL(t.[Buy Month], '')
	, ISNULL(t.[PONO], '')
	, ISNULL(t.[VAS/SHAS], '')
	, [VAS/SHAS Apv.]
	, [VAS/SHAS Cut Off Date]
	, [M/Notice Date]
	, [Est M/Notice Apv.]
	, ISNULL(t.[Tissue], '')
	, ISNULL(t.[AF by adidas], '')
	, ISNULL(t.[Factory Disclaimer], '')
	, ISNULL(t.[Factory Disclaimer Remark], '')
	, [Approved/Rejected Date]
	, ISNULL(t.[Global Foundation Range], '')
	, ISNULL(t.[Brand], '')
	, ISNULL(t.[Cust CD], '')
	, ISNULL(t.[KIT], '')
	, ISNULL(t.[Fty Code], '')
	, ISNULL(t.[Program], '')
	, ISNULL(t.[Non Revenue], '')
	, ISNULL(t.[New CD Code], '')
	, ISNULL(t.[ProductType], '')
	, ISNULL(t.[FabricType], '')
	, ISNULL(t.[Lining], '')
	, ISNULL(t.[Gender], '')
	, ISNULL(t.[Construction], '')
	, ISNULL(t.[Cpu], 0)
	, ISNULL(t.[Qty], 0)
	, ISNULL(t.[FOC Qty], 0)
	, ISNULL(t.[Total CPU], 0)
	, ISNULL(t.[SewQtyTop], 0)
	, ISNULL(t.[SewQtyBottom], 0)
	, ISNULL(t.[SewQtyInner], 0)
	, ISNULL(t.[SewQtyOuter], 0)
	, ISNULL(t.[Total Sewing Output], 0)
	, ISNULL(t.[Cut Qty], 0)
	, ISNULL(t.[By Comb], '')
	, ISNULL(t.[Cutting Status], '')
	, ISNULL(t.[Packing Qty], 0)
	, ISNULL(t.[Packing FOC Qty], 0)
	, ISNULL(t.[Booking Qty], 0)
	, ISNULL(t.[FOC Adj Qty], 0)
	, ISNULL(t.[Not FOC Adj Qty], 0)
	, ISNULL(t.[Total], 0)
	, [KPI L/ETA]
	, [PF ETA (SP)]
	, ISNULL(t.[Pull Forward Remark], '')
	, [Pack L/ETA]
	, [SCHD L/ETA]
	, [Actual Mtl. ETA]
	, [Fab ETA]
	, [Acc ETA]
	, ISNULL(t.[Sewing Mtl Complt(SP)], '')
	, ISNULL(t.[Packing Mtl Complt(SP)], '')
	, [Sew. MTL ETA (SP)]
	, [Pkg. MTL ETA (SP)]
	, ISNULL(t.[MTL Delay], '')
	, ISNULL(t.[MTL Cmplt], '')
	, ISNULL(t.[MTL Cmplt (SP)], '')
	, [Arrive W/H Date]
	, [Sewing InLine]
	, [Sewing OffLine]
	, [1st Sewn Date]
	, [Last Sewn Date]
	, [First Production Date]
	, [Last Production Date]
	, [Each Cons Apv Date]
	, [Est Each Con Apv.]
	, [Cutting InLine]
	, [Cutting OffLine]
	, [Cutting InLine(SP)]
	, [Cutting OffLine(SP)]
	, [1st Cut Date]
	, [Last Cut Date]
	, [Est. Pullout]
	, [Act. Pullout Date]
	, ISNULL(t.[Pullout Qty], 0)
	, ISNULL(t.[Act. Pullout Times], 0)
	, ISNULL(t.[Act. Pullout Cmplt], '')
	, [KPI Delivery Date]
	, ISNULL(t.[Update Delivery Reason], '')
	, [Plan Date]
	, [Original Buyer Delivery Date]
	, ISNULL(t.[SMR], '')
	, ISNULL(t.[SMR Name], '')
	, ISNULL(t.[Handle], '')
	, ISNULL(t.[Handle Name], '')
	, ISNULL(t.[Posmr], '')
	, ISNULL(t.[Posmr Name], '')
	, ISNULL(t.[PoHandle], '')
	, ISNULL(t.[PoHandle Name], '')
	, ISNULL(t.[PCHandle], '')
	, ISNULL(t.[PCHandle Name], '')
	, ISNULL(t.[MCHandle], '')
	, ISNULL(t.[MCHandle Name], '')
	, ISNULL(t.[DoxType], '')
	, ISNULL(t.[Packing CTN], 0)
	, ISNULL(t.[TTLCTN], 0)
	, ISNULL(t.[Pack Error CTN], 0)
	, ISNULL(t.[FtyCTN], 0)
	, ISNULL(t.[cLog CTN], 0)
	, ISNULL(t.[CFA CTN], 0)
	, [cLog Rec. Date]
	, [Final Insp. Date]
	, ISNULL(t.[Insp. Result], '')
	, ISNULL(t.[CFA Name], '')
	, ISNULL(t.[Sewing Line#], '')
	, ISNULL(t.[ShipMode], '')
	, ISNULL(t.[SI#], '')
	, ISNULL(t.[ColorWay], '')
	, ISNULL(t.[Special Mark], '')
	, ISNULL(t.[Fty Remark], '')
	, ISNULL(t.[Sample Reason], '')
	, ISNULL(t.[IS MixMarker], '')
	, ISNULL(t.[Cutting SP], '')
	, ISNULL(t.[Rainwear test], '')
	, ISNULL(t.[TMS], 0)
	, [MD room scan date]
	, [Dry Room received date]
	, [Dry room trans date]
	, [Last ctn trans date]
	, [Last Scan And Pack Date]
	, [Last ctn recvd date]
	, ISNULL(t.[OrganicCotton], '')
	, ISNULL(t.[Direct Ship], '')
	, ISNULL(t.[StyleCarryover], '')
	, t.[SCHDL/ETA(SP)]
	, t.[SewingMtlETA(SPexclRepl)]
	, t.[ActualMtlETA(exclRepl)]
	, ISNULL(t.[HalfKey], '')
	, ISNULL(t.[DevSample], '')
	, ISNULL(t.[POID], '')
	, ISNULL(t.[KeepPanels], '')
	, ISNULL(t.[BuyBackReason], '')
	, ISNULL(t.[SewQtybyRate], 0)
	, ISNULL(t.[Unit], '')
	, ISNULL(t.[SubconInType], '')
	, ISNULL(t.[Article], '')
	, ISNULL(t.[ProduceRgPMS], '')
	, ISNULL(t.[Buyerhalfkey], '')
	, ISNULL(t.[Country], '')
	, ISNULL(t.[Third_Party_Insepction],0)
	, ISNULL(t.[ColorID],'')
from #tmpReName t
where not exists (select 1 from P_PPICMASTERLIST p where t.[SPNO] = p.[SPNO])

delete p
from P_PPICMASTERLIST p 
where (p.SCIDlv >= @sDate
	or p.Delivery >= @sDate)
and not exists (select 1 from #tmpReName t where t.[SPNO] = p.[SPNO])

delete p
from P_PPICMasterList_ArtworkType p
where not exists (select 1 from P_PPICMASTERLIST t where t.SPNO = p.[SP#])
";
                finalResult = new Base_ViewModel()
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData_P_PPICMasterList_Extend(DataTable dt)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
update p 
	set p.[ColumnValue] = ISNULL(t.[ColumnValue], 0)
from P_PPICMasterList_Extend p 
inner join #tmp t on t.OrderID = p.OrderID and t.ColumnName = p.ColumnName

insert into P_PPICMasterList_Extend(OrderID, ColumnName, ColumnValue)
select OrderID, ColumnName, isnull(ColumnValue, 0)
from #tmp t
where not exists (select 1 from P_PPICMasterList_Extend p where t.OrderID = p.OrderID and t.ColumnName = p.ColumnName)

delete p
from P_PPICMasterList_Extend p
where not exists (select 1 from P_PPICMASTERLIST t where t.SPNO = p.OrderID)

if exists (select 1 from BITableInfo b where b.id = 'P_PPICMASTERLIST')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_PPICMASTERLIST'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_PPICMASTERLIST', getdate())
end
";
                finalResult = new Base_ViewModel()
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };
            }

            return finalResult;
        }
    }
}
