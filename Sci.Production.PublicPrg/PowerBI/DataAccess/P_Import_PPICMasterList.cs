using Sci.Data;
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
        public Base_ViewModel P_PPICMasterListBIData(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R03 biModel = new PPIC_R03();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-60).ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R03_ViewModel ppic_R03_ViewModel = new PPIC_R03_ViewModel()
                {
                    IsPowerBI = true,
                    BuyerDelivery1 = item.SDate,
                    SciDelivery1 = item.SDate,
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
                finalResult = this.UpdateBIData_P_PPICMASTERLIST(dt_P_PPICMASTERLIST, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = this.UpdateBIData_P_PPICMasterList_Extend(dt_P_PPICMasterList_ArtworkType, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
                item.ClassName = "P_PPICMasterList_Extend";
                finalResult = new Base().UpdateBIData(item);
                item.ClassName = "P_PPICMASTERLIST";
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData_P_PPICMASTERLIST(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@sDate", item.SDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = @"	
update p 
	set p.[M] = ISNULL(t.[M], '')
		, p.[FactoryID] = ISNULL(t.[Factory], '')
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
		, p.[CPUAdjusted] = ISNULL(t.[CPU Adjusted %], 0)
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
		, p.[OrganicCotton] = ISNULL(t.[Organic Cotton/Recycle Polyester/Recycle Nylon], '')
		, p.[Direct Ship] = ISNULL(t.[Direct Ship], '')
		, p.[StyleCarryover] = ISNULL(t.[Style-Carryover], '')
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
		, p.[FtyToClogTransit] = ISNULL(t.[Fty To Clog Transit], 0)
		, p.[ClogToCFATansit] = ISNULL(t.[Clog To CFA Tansit], 0)
		, p.[CFAToClogTransit] = ISNULL(t.[CFA To Clog Transit], 0)
		, p.[Shortage] = ISNULL(t.[Shortage],0)
		, p.[Original CustPO] = ISNULL(t.[Original CustPO],'')
		, p.[Line Aggregator] = ISNULL(t.[Line Aggregator],'')
		, p.[JokerTag] = ISNULL(t.[JokerTag], 0)
		, p.[HeatSeal] = ISNULL(t.[HeatSeal], 0)
		, p.[CriticalStyle] = ISNULL(t.[CriticalStyle],'')
		, p.[OrderCompanyID] = ISNULL(t.[OrderCompanyID], 0)
		, p.[BIFactoryID] = @BIFactoryID
        , p.[BIInsertDate] = GETDATE()   
from P_PPICMASTERLIST p 
inner join #tmp t on p.[SPNO] = t.[SPNO]

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
	, [Article], [ProduceRgPMS], [Buyerhalfkey], [Country],[Third_Party_Insepction],[ColorID],[FtyToClogTransit],[ClogToCFATansit],[CFAToClogTransit],[Shortage]
	, [Original CustPO], [Line Aggregator], [JokerTag], [HeatSeal], [CriticalStyle], [OrderCompanyID], [BIFactoryID], [BIInsertDate])
select ISNULL(t.[M], '')
	, ISNULL(t.[Factory], '')
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
	, ISNULL(t.[CPU Adjusted %], 0)
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
	, ISNULL(t.[Organic Cotton/Recycle Polyester/Recycle Nylon], '')
	, ISNULL(t.[Direct Ship], '')
	, ISNULL(t.[Style-Carryover], '')
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
	, ISNULL([Fty To Clog Transit], 0)
    , ISNULL([Clog To CFA Tansit], 0)
    , ISNULL([CFA To Clog Transit], 0)
	, ISNULL([Shortage],0)
	, ISNULL([Original CustPO],'')
	, ISNULL([Line Aggregator],'')
	, ISNULL([JokerTag], 0)
	, ISNULL([HeatSeal], 0) 
    , ISNULL([CriticalStyle],'')
	, ISNULL([OrderCompanyID], 0)
	, @BIFactoryID
    , GETDATE()  
from #tmp t
where not exists (select 1 from P_PPICMASTERLIST p where t.[SPNO] = p.[SPNO])

if @IsTrans = 1
begin
	Insert into P_PPICMASTERLIST_History (Ukey, BIFactoryID, BIInsertDate)
	Select Ukey, BIFactoryID, [BIInsertDate]=GetDate()
	From P_PPICMASTERLIST p
	Where (p.SCIDlv >= @sDate
		or p.Delivery >= @sDate)
	And not exists (select 1 from #tmp t where t.[SPNO] = p.[SPNO])
end

delete p
from P_PPICMASTERLIST p 
where (p.SCIDlv >= @sDate
	or p.Delivery >= @sDate)
and not exists (select 1 from #tmp t where t.[SPNO] = p.[SPNO])

if @IsTrans = 1
begin
	Insert into P_PPICMasterList_ArtworkType_History ([SP#], [SubconInTypeID], [ArtworkTypeKey], BIFactoryID, BIInsertDate)
	Select p.[SP#], p.[SubconInTypeID], p.[ArtworkTypeKey], p.BIFactoryID, [BIInsertDate]=GetDate()
	From P_PPICMasterList_ArtworkType p
	Where not exists (select 1 from P_PPICMASTERLIST t where t.SPNO = p.[SP#])
end

delete p
from P_PPICMasterList_ArtworkType p
where not exists (select 1 from P_PPICMASTERLIST t where t.SPNO = p.[SP#])
";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData_P_PPICMasterList_Extend(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
alter table #tmp alter column [OrderID] varchar(13)
alter table #tmp alter column [ColumnName] varchar(50)

if @IsTrans = 1
begin
	insert into [dbo].[P_PPICMasterList_Extend_History]([ColumnName], [OrderID], [BIFactoryID], [BIInsertDate])
	select p.[ColumnName], p.[OrderID], @BIFactoryID, GETDATE()
	from P_PPICMasterList_Extend p
	inner join #tmp t on t.OrderID = p.OrderID
end

delete p
from P_PPICMasterList_Extend p
inner join #tmp t on t.OrderID = p.OrderID

insert into P_PPICMasterList_Extend(OrderID, ColumnName, ColumnValue, [BIFactoryID], [BIInsertDate])
select OrderID, ColumnName, isnull(ColumnValue, 0), @BIFactoryID, GETDATE()
from #tmp t
where not exists (select 1 from P_PPICMasterList_Extend p where t.OrderID = p.OrderID and t.ColumnName = p.ColumnName)

if @IsTrans = 1
begin
	insert into P_PPICMasterList_Extend_History([ColumnName], [OrderID], [BIFactoryID], [BIInsertDate])
	Select p.[ColumnName], p.[OrderID], @BIFactoryID, [BIInsertDate] = GetDate()
	from P_PPICMasterList_Extend p
	where not exists (select 1 from P_PPICMASTERLIST t where t.SPNO = p.OrderID)
end

delete p
from P_PPICMasterList_Extend p
where not exists (select 1 from P_PPICMASTERLIST t where t.SPNO = p.OrderID)

";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
