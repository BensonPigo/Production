using Sci.Data;
using Sci.Production.Class.Command;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class PPIC_R03
    {
        /// <inheritdoc/>
        public PPIC_R03()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetPPICMasterList(PPIC_R03_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@BuyerDelivery1", SqlDbType.Date) { Value = (object)model.BuyerDelivery1 ?? DBNull.Value },
                new SqlParameter("@BuyerDelivery2", SqlDbType.Date) { Value = (object)model.BuyerDelivery2 ?? DBNull.Value },
                new SqlParameter("@SCIDelivery1", SqlDbType.Date) { Value = (object)model.SciDelivery1 ?? DBNull.Value },
                new SqlParameter("@SCIDelivery2", SqlDbType.Date) { Value = (object)model.SciDelivery2 ?? DBNull.Value },
                new SqlParameter("@SDPDate1", SqlDbType.Date) { Value = (object)model.SDPDate1 ?? DBNull.Value },
                new SqlParameter("@SDPDate2", SqlDbType.Date) { Value = (object)model.SDPDate2 ?? DBNull.Value },
                new SqlParameter("@CRDDate1", SqlDbType.Date) { Value = (object)model.CRDDate1 ?? DBNull.Value },
                new SqlParameter("@CRDDate2", SqlDbType.Date) { Value = (object)model.CRDDate2 ?? DBNull.Value },
                new SqlParameter("@PlanDate1", SqlDbType.Date) { Value = (object)model.PlanDate1 ?? DBNull.Value },
                new SqlParameter("@PlanDate2", SqlDbType.Date) { Value = (object)model.PlanDate2 ?? DBNull.Value },
                new SqlParameter("@CFMDate1", SqlDbType.Date) { Value = (object)model.CFMDate1 ?? DBNull.Value },
                new SqlParameter("@CFMDate2", SqlDbType.Date) { Value = (object)model.CFMDate2 ?? DBNull.Value },
                new SqlParameter("@SP1", SqlDbType.VarChar) { Value = (object)model.SP1 ?? DBNull.Value },
                new SqlParameter("@SP2", SqlDbType.VarChar) { Value = (object)model.SP2 ?? DBNull.Value },
                new SqlParameter("@StyleID", SqlDbType.VarChar) { Value = (object)model.StyleID ?? DBNull.Value },
                new SqlParameter("@Article", SqlDbType.VarChar) { Value = (object)model.Article ?? DBNull.Value },
                new SqlParameter("@SeasonID", SqlDbType.VarChar) { Value = (object)model.SeasonID ?? DBNull.Value },
                new SqlParameter("@BrandID", SqlDbType.VarChar) { Value = (object)model.BrandID ?? DBNull.Value },
                new SqlParameter("@CustCDID", SqlDbType.VarChar) { Value = (object)model.CustCDID ?? DBNull.Value },
                new SqlParameter("@Zone", SqlDbType.VarChar) { Value = (object)model.Zone ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar) { Value = (object)model.MDivisionID ?? DBNull.Value },
                new SqlParameter("@FtyGroup", SqlDbType.VarChar) { Value = (object)model.Factory ?? DBNull.Value },
            };

            List<DataTable> dts = new List<DataTable>();
            Base_ViewModel resultReport = new Base_ViewModel();
            model.P_type = "ALL";
            var result = DBProxy.Current.Select(null, this.GetMainDatas(model), listPar, out DataTable printData);
            if (!result)
            {
                resultReport.Result = result;
                return resultReport;
            }

            // 抓取forecast資料再merge回主datatable，只有forecast和seperate有勾的時候才做
            if (model.Forecast && model.SeparateByQtyBdownByShipmode)
            {
                model.P_type = "forecast";
                result = DBProxy.Current.Select(null, this.GetMainDatas(model), out DataTable printData_forecast);
                if (!result)
                {
                    resultReport.Result = result;
                    return resultReport;
                }

                printData.Merge(printData_forecast);
            }

            dts.Add(printData);

            if (printData.Rows.Count > 0)
            {
                if (model.IncludeArtworkData || model.IncludeArtworkDataKindIsPAP)
                {
                    result = MyUtility.Tool.ProcessWithDatatable(printData, "ID,Qty,CPU", this.GetArtworkTypeValue(model), out DataTable[] artworkDatas);
                    if (!result)
                    {
                        resultReport.Result = result;
                        return resultReport;
                    }

                    foreach (var dt in artworkDatas)
                    {
                        dts.Add(dt);
                    }
                }

                if (model.PrintingDetail)
                {
                    result = MyUtility.Tool.ProcessWithDatatable(printData, "ID", this.GetPrintingDetail(model), out DataTable artworkDatas);
                    if (!result)
                    {
                        resultReport.Result = result;
                        return resultReport;
                    }

                    dts.Add(artworkDatas);
                }
            }

            resultReport.Result = result;
            resultReport.DtArr = dts.ToArray();
            return resultReport;
        }

        /// <summary>
        /// BI
        /// = 沒勾選 Seperate by Qty b'down by shipmode
        /// = 沒勾選 List PO Combo
        /// = 有勾選 Include History Order
        /// = 有勾選 Include Cancel Order
        /// = Category IN ('B','S','G','')
        /// </summary>
        private string GetMainDatas(PPIC_R03_ViewModel model)
        {
            // 需要注意規則
            // 有勾選 Seperate by < Qty b'down by shipmode > 展開到 Order_QtyShip
            // 若同時勾選 forecast && Seperate by < Qty b'down by shipmode >, 則在 PPIC R03產出的Excel o.Category = '' 的資料必須在最下方
            #region 篩選條件, 不包含 List PO Combo 判斷
            string whereOrders = string.Empty;
            if (MyUtility.Check.Seek($"SELECT 1 FROM System WHERE NoRestrictOrdersDelivery = 0"))
            {
                whereOrders += "AND (o.IsForecast = 0 OR (o.IsForecast = 1 AND (o.SciDelivery <= DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 6) OR o.BuyerDelivery < DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 0))))\r\n";
            }

            if (model.IsPowerBI)
            {
                string whereSCIDelivery = "o.SCIDelivery >= @SCIDelivery1";
                if (model.SciDelivery2.HasValue)
                {
                    whereSCIDelivery += " AND o.SCIDelivery <= @SCIDelivery2";
                }

                string whereBuyerDelivery = "o.BuyerDelivery >= @BuyerDelivery1";
                if (model.BuyerDelivery2.HasValue)
                {
                    whereBuyerDelivery += " AND o.BuyerDelivery <= @BuyerDelivery2";
                }

                whereOrders += $@"
AND (
    ({whereSCIDelivery})
    OR ({whereBuyerDelivery})
    OR o.EditDate >= DATEADD(DAY, -5, GETDATE())
    OR o.Transferdate >= DATEADD(DAY, -3, GETDATE())
)
AND o.Category IN ('B','S','G','')
";
            }
            else
            {
                string sqlTableAlias = model.SeparateByQtyBdownByShipmode ? "oqs" : "o";  // Order_QtyShip oqs : Orders o
                if (model.BuyerDelivery1.HasValue)
                {
                    whereOrders += $"AND {sqlTableAlias}.BuyerDelivery >= @BuyerDelivery1\r\n";
                }

                if (model.BuyerDelivery2.HasValue)
                {
                    whereOrders += $"AND {sqlTableAlias}.BuyerDelivery <= @BuyerDelivery2\r\n";
                }

                if (model.SciDelivery1.HasValue)
                {
                    whereOrders += "AND o.SciDelivery >= @SCIDelivery1\r\n";
                }

                if (model.SciDelivery2.HasValue)
                {
                    whereOrders += "AND o.SciDelivery <= @SCIDelivery2\r\n";
                }

                if (model.SDPDate1.HasValue || model.SDPDate2.HasValue)
                {
                    string whereSDPDate = string.Empty;
                    if (model.SDPDate1.HasValue)
                    {
                        whereSDPDate = "AND oqs.SDPDate >= @SDPDate1";
                    }

                    if (model.SDPDate2.HasValue)
                    {
                        whereSDPDate = "AND oqs.SDPDate <= @SDPDate2";
                    }

                    whereOrders += $"AND EXISTS (SELECT 1 FROM Order_QtyShip oqs WITH (NOLOCK) oqs.ID = o.ID {whereSDPDate})";
                }

                if (model.CRDDate1.HasValue)
                {
                    whereOrders += "AND o.CRDDate >= @CRDDate1\r\n";
                }

                if (model.CRDDate2.HasValue)
                {
                    whereOrders += "AND o.CRDDate <= @CRDDate2\r\n";
                }

                if (model.PlanDate1.HasValue)
                {
                    whereOrders += "AND o.PlanDate >= @PlanDate1\r\n";
                }

                if (model.PlanDate2.HasValue)
                {
                    whereOrders += "AND o.PlanDate <= @PlanDate2\r\n";
                }

                if (model.CFMDate1.HasValue)
                {
                    whereOrders += "AND o.CFMDate >= @CFMDate1\r\n";
                }

                if (model.CFMDate2.HasValue)
                {
                    whereOrders += "AND o.CFMDate <= @CFMDate2\r\n";
                }

                if (!MyUtility.Check.Empty(model.SP1))
                {
                    whereOrders += "AND o.ID >= @SP1\r\n";
                }

                if (!MyUtility.Check.Empty(model.SP2))
                {
                    whereOrders += "AND o.ID <= @SP2\r\n";
                }

                if (!MyUtility.Check.Empty(model.StyleID))
                {
                    whereOrders += "AND o.StyleID = @StyleID\r\n";
                }

                if (!MyUtility.Check.Empty(model.Article))
                {
                    whereOrders += "AND (EXISTS(SELECT 1 FROM Order_Article oa WITH (NOLOCK) WHERE oa.ID = o.ID AND oa.Article = @Article) OR NOT EXISTS(SELECT 1 FROM Order_Article oa WITH (NOLOCK) WHERE oa.ID = o.ID))\r\n";
                }

                if (!MyUtility.Check.Empty(model.SeasonID))
                {
                    whereOrders += "AND o.SeasonID = @SeasonID\r\n";
                }

                if (!MyUtility.Check.Empty(model.BrandID))
                {
                    whereOrders += "AND o.BrandID = @BrandID\r\n";
                }

                if (!MyUtility.Check.Empty(model.CustCDID))
                {
                    whereOrders += "AND o.CustCDID = @CustCDID\r\n";
                }

                if (!MyUtility.Check.Empty(model.Zone))
                {
                    whereOrders += "AND EXISTS(SELECT 1 FROM Factory f WITH (NOLOCK) WHERE f.ID = o.FactoryID AND f.Zone = @Zone)\r\n";
                }

                if (!MyUtility.Check.Empty(model.MDivisionID))
                {
                    whereOrders += "AND o.MDivisionID = @MDivisionID\r\n";
                }

                if (!MyUtility.Check.Empty(model.Factory))
                {
                    whereOrders += "AND o.FtyGroup = @FtyGroup\r\n";
                }

                List<string> listCategory = new List<string>();
                if (model.Bulk)
                {
                    listCategory.Add("'B'");
                }

                if (model.Sample)
                {
                    listCategory.Add("'S'");
                }

                if (model.Material)
                {
                    listCategory.Add("'M'");
                }

                if (model.Forecast)
                {
                    listCategory.Add("''");
                }

                if (model.Garment)
                {
                    listCategory.Add("'G'");
                }

                if (model.SMTL)
                {
                    listCategory.Add("'T'");
                }

                if (listCategory.Any())
                {
                    whereOrders += $"AND(Category IN ({listCategory.JoinToString(",")}))\r\n";
                }

                if (!MyUtility.Check.Empty(model.ArtworkTypeID))
                {
                    whereOrders += "AND EXISTS(SELECT 1 FROM Style_TmsCost stc WITH (NOLOCK) WHERE stc.StyleUkey = o.StyleUkey AND stc.ArtworkTypeID = @ArtworkTypeID AND (stc.Qty > 0 OR stc.TMS > 0 AND stc.Price > 0))\r\n";
                }

                if (!model.IncludeHistoryOrder)
                {
                    whereOrders += "AND o.Finished = 0\r\n";
                }

                if (!model.IncludeCancelOrder)
                {
                    whereOrders += "AND o.Junk = 0\r\n";
                }
            }
            #endregion

            // 是否展開 Order_QtyShip, Pkey:ID, Seq
            string order_QtyShip_Columns = string.Empty;
            string order_QtyShip_Join = string.Empty;
            if (model.SeparateByQtyBdownByShipmode)
            {
                order_QtyShip_Columns = @"
    ,oqs.Seq
    ,oqs.ShipmodeID
    ,oqs.EstPulloutDate
    ,oqs.CFAIs3rdInspect
    ,oqs.CFAFinalInspectResult
    ,oqs.CFAFinalInspectDate
    ,oqs.IDD
    ,oqs.CFAFinalInspectHandle
    ,oqs.ClogReasonID";
                order_QtyShip_Join = @"
INNER JOIN Order_QtyShip oqs WITH (NOLOCK) ON oqs.ID = o.ID";
            }

            string sqlcmd = $@"
--1.先依據篩選條件找出需要資料,不做任何處理
SELECT o.*
{order_QtyShip_Columns}
INTO #tmpOrders
FROM Orders o WITH (NOLOCK)
{order_QtyShip_Join}
WHERE 1 = 1
{whereOrders}

CREATE NONCLUSTERED INDEX index_tmpOrders_ID ON #tmpOrders(ID ASC);
";

            // 以下從 PPIC R03 移動過來沒動任何規則, 只是改用tmp
            string seperCmdkpi = model.SeparateByQtyBdownByShipmode ? "oq.FtyKPI" : "o.FtyKPI";
            string seperCmdkpi2 = model.SeparateByQtyBdownByShipmode ? @" left join Order_QtyShip oq WITH (NOLOCK) on o.id=oq.Id and o.seq = oq.seq" : string.Empty;
            string order_QtyShip_Source_InspDate = model.SeparateByQtyBdownByShipmode ? "oq.CFAFinalInspectDate " : "QtyShip_InspectDate.Val";
            string order_QtyShip_Source_InspResult = model.SeparateByQtyBdownByShipmode ? "oq.CFAFinalInspectResult" : "QtyShip_Result.Val";
            string order_QtyShip_Source_InspHandle = model.SeparateByQtyBdownByShipmode ? "oq.CFAFinalInspectHandle" : "QtyShip_Handle.Val";
            string thridColumn = model.SeparateByQtyBdownByShipmode ? " iif(oq.CFAIs3rdInspect = 1,'Y','N')" : " iif(oqs.cnt > 0,'Y','N')";
            string order_QtyShip_OuterApply = model.SeparateByQtyBdownByShipmode ? string.Empty : $@"
	OUTER APPLY(
		SELECT [Val]=STUFF((
		    SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		    from Order_QtyShip oq WITH (NOLOCK)
		    WHERE ID = o.id
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_InspectDate
	OUTER APPLY(
		SELECT [Val]=STUFF((
		    SELECT  DISTINCT ','+ CFAFinalInspectResult
		    from Order_QtyShip oq WITH (NOLOCK)
		    WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_Result
	OUTER APPLY(
		SELECT [Val]=STUFF((
		SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
		    from Order_QtyShip oq WITH (NOLOCK)
			LEFT JOIN Pass1 p WITH (NOLOCK) ON oq.CFAFinalInspectHandle = p.ID 
		    WHERE oq.ID = o.id AND CFAFinalInspectHandle <> '' AND CFAFinalInspectHandle IS NOT NULL
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_Handle
";
            string seperCmd;
            if (model.SeparateByQtyBdownByShipmode && model.P_type.Equals("ALL"))
            {
                seperCmd = " ,oq.Seq,[IDD] = Format(oq.IDD, 'yyyy/MM/dd')";
            }
            else
            {
                seperCmd = @" ,[IDD] = (SELECT  Stuff((select distinct concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = o.ID FOR XML PATH('')),1,1,'') )";
            }

            string biMoreColumn = !model.IsPowerBI ? string.Empty : @"
        , o.Max_ScheETAbySP
        , o.Sew_ScheETAnoReplace
        , o.MaxShipETA_Exclude5x
        , [HalfKey] = iif(cast(format(dateadd(day,-7,o.SciDelivery), 'dd') as int) between 1 and 15
				        , format(dateadd(day,-7,o.SciDelivery), 'yyyyMM01')
				        , format(dateadd(day,-7,o.SciDelivery), 'yyyyMM02'))
        , [Buyerhalfkey] = iif(cast(format(dateadd(day,-7,o.BuyerDelivery), 'dd') as int) between 1 and 15
				        , format(dateadd(day,-7,o.BuyerDelivery), 'yyyyMM01')
				        , format(dateadd(day,-7,o.BuyerDelivery), 'yyyyMM02'))
        , [DevSample] = iif((select IsDevSample from OrderType ot WITH (NOLOCK) where ot.ID = o.OrderTypeID and ot.BrandID = o.BrandID) = 1, 'Y', '')
        , [KeepPanels] = iif(o.KeepPanels = 0, 'N', 'Y')
        , o.BuyBackReason
        , o.StyleUnit
        , [SubconInType] = iif(o.SubconInType = '1', 'Subcon-in from sister factory (same M division)'
					        , iif(o.SubconInType = '2', 'Subcon-in from sister factory (different M division)'
						        , iif(o.SubconInType = '3', 'Subcon-in from non-sister factory', '')))
        , [ProduceRgPMS] = (select ProduceRgCode from SCIFty sf WITH (NOLOCK) where sf.ID = o.FactoryID)
        , [Third_Party_Insepction] = iif(oqs.cnt > 0,1,0)
        , TMS = o.CPU * (select StdTMS from System WITH (NOLOCK))
";
            string biMoreColumn2 = !model.IsPowerBI ? string.Empty : @"
        , Max_ScheETAbySP
        , Sew_ScheETAnoReplace
        , MaxShipETA_Exclude5x
        , [HalfKey]
        , [Buyerhalfkey]
        , [DevSample]
        , [KeepPanels]
        , BuyBackReason
        , t.StyleUnit
        , [SubconInType]
        , [ProduceRgPMS]
        , [Third_Party_Insepction]
		, [SewQtybyRate] = isnull(case when t.StyleUnit = 'PCS' then (select SUM(QAQty) from #tmp_SewingOutput where OrderID = t.ID) 
							else (	select  SUM(ROUND(a.OutputQty * ol.Rate/100, 2))
									from (
										select OrderID, ComboType, [OutputQty] = SUM(QAQty)
										from #tmp_SewingOutput
										where OrderID = t.ID
										group by OrderID, ComboType
									) a
									inner join Order_Location ol WITH (NOLOCK) on ol.OrderID = a.OrderID and ol.Location = a.ComboType
							) end, 0)
        ,TMS
";

            // 第2段
            sqlcmd += $@"
select DISTINCT o.ID
        , o.MDivisionID
        , o.FtyGroup
        , o.FactoryID
        , o.BuyerDelivery
        , o.SciDelivery
        , o.POID
        , o.CRDDate
        , o.CFMDate
        , o.Dest
        , o.StyleID
        , s.StyleName
        , o.SeasonID
        , o.BrandID
        , o.ProjectID
        , Kit=(SELECT top 1 c.Kit From CustCD c WITH (NOLOCK) where c.ID=o.CustCDID AND c.BrandID=o.BrandID)
		,[PackingMethod]=d.Name 
        , o.HangerPack
        , o.Customize1
        , o.BuyMonth
        , o.CustPONo
        , o.CustCDID
        , o.ProgramID
		, [NonRevenue]=IIF(o.NonRevenue=1,'Y','N')
        , o.CdCodeID
	    , s.CDCodeNew
        , [ProductType] = r2.Name
		, [FabricType] = r1.Name
		, s.Lining
		, s.Gender
		, [Construction] = d1.Name
        , o.CPU
        , o.Qty as Qty
        , o.FOCQty
        , o.LocalOrder
        , o.PoPrice
        , o.CMPPrice
        , o.KPILETA
        , o.PFETA
        , o.LETA
        , o.MTLETA
        , o.SewETA
        , o.PackETA
        , o.MTLComplete
        , o.SewInLine
        , o.SewOffLine
        , o.CutInLine
        , o.CutOffLine
		, [CutInLine_SP] = csp.InLine
		, [CutOffLine_SP] = csp.OffLine
        , [3rd_Party_Inspection] = {thridColumn}
        , Category=case when o.Category='B'then'Bulk'
						when o.Category='G'then'Garment'
						when o.Category='M'then'Material'
						when o.Category='S'then'Sample'
						when o.Category='T'then'Sample mtl.'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
					end
        , o.PulloutDate
        , o.ActPulloutDate
        , o.SMR
        , o.MRHandle
        , o.MCHandle
        , o.OrigBuyerDelivery
        , o.DoxType
        , o.TotalCTN
        , PackErrorCtn = isnull(o.PackErrCTN,0)
        , o.FtyCTN
        , o.ClogCTN
        , CFACTN=isnull(o.CFACTN,0)
        , o.VasShas
        , o.TissuePaper
        , [MTLExport]=IIF(o.MTLExport='OK','Y',o.MTLExport)
        , o.SewLine
        , o.ShipModeList
        , o.PlanDate
        , o.FirstProduction
		, o.LastProductionDate
        , o.OrderTypeID
        , Order_ColorCombo.ColorID
        , o.SpecialMark
        , o.SampleReason
        , InspDate = {order_QtyShip_Source_InspDate}
        , InspResult = {order_QtyShip_Source_InspResult}
        , InspHandle = {order_QtyShip_Source_InspHandle}
        , o.MnorderApv2
        , o.MnorderApv
        , o.PulloutComplete
        , {seperCmdkpi}
        , o.KPIChangeReason
        , o.EachConsApv
        , o.Junk
        , o.StyleUkey
        , o.CuttingSP
        , o.RainwearTestPassed
        , o.BrandFTYCode
        , o.CPUFactor
        , o.ClogLastReceiveDate
		, [IsMixMarker]=  CASE WHEN o.IsMixMarker=0 THEN 'Is Single Marker'
							WHEN o.IsMixMarker=1 THEN 'Is Mix  Marker'		
							WHEN o.IsMixMarker=2 THEN ' Is Mix Marker - SCI'
							ELSE ''
						END
        , o.GFR 
		, isForecast = iif(isnull(o.Category,'')='','1','')
        , [AirFreightByBrand] = IIF(o.AirFreightByBrand='1','Y','')
        , [BuyBack] = iif(exists (select 1 from Order_BuyBack WITH (NOLOCK) where ID = o.ID), 'Y', '')
        , [Cancelled] = case when o.junk = 1 then 
                                case when o.NeedProduction = 1 then 'Y' 
                                    when o.KeepPanels = 1 then 'K'
                                    else 'N' end
                        else ''
                        end
        , o.Customize2
        , o.KpiMNotice
        , o.KpiEachConsCheck
        , o.LastCTNTransDate
		, ScanEditDate = scanEditDate.Val
        , o.LastCTNRecdDate
        , o.DryRoomRecdDate
        , o.DryRoomTransDate
        , o.MdRoomScanDate
        , [VasShasCutOffDate] = Format(DATEADD(DAY, -30, iif(GetMinDate.value is null, coalesce(o.BuyerDelivery, o.CRDDate, o.PlanDate, o.OrigBuyerDelivery), GetMinDate.value)), 'yyyy/MM/dd')
        , [StyleSpecialMark] = s.SpecialMark
        {seperCmd}
	    , [SewingMtlComplt]  = isnull(CompltSP.SewingMtlComplt, '')
	    , [PackingMtlComplt] = isnull(CompltSP.PackingMtlComplt, '')
        , o.OrganicCotton
        , o.DirectShip
        ,[StyleCarryover] = iif(s.NewCO = '2','V','')
        , o.PackLETA
        {biMoreColumn}
into #tmpOrders2
from #tmpOrders o WITH (NOLOCK) 
left join style s WITH (NOLOCK) on o.styleukey = s.ukey
left join DropDownList d with (nolock) ON o.CtnType=d.ID AND d.Type='PackingMethod'
left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
{seperCmdkpi2}
OUTER APPLY(
    SELECT  Name 
    FROM Pass1 WITH (NOLOCK) 
    WHERE Pass1.ID = O.InspHandle
)I
outer apply (
	select value = (
		select Min(Date)
		From (Values (o.BuyerDelivery), (o.CRDDate), (o.PlanDate), (o.OrigBuyerDelivery)) as tmp (Date)
		where tmp.Date is not null
	)
) GetMinDate
outer apply (
	select 
		[PackingMtlComplt] = max([PackingMtlComplt])
		, [SewingMtlComplt] = max([SewingMtlComplt])
	from 
	(
		select  f.ProductionType
			, [PackingMtlComplt] = iif(f.ProductionType = 'Packing' and sum(iif(f.ProductionType = 'Packing', 1, 0)) = sum(iif(f.ProductionType = 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
			, [SewingMtlComplt] = iif(f.ProductionType <> 'Packing' and sum(iif(f.ProductionType <> 'Packing', 1, 0)) = sum(iif(f.ProductionType <> 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
		from 
		(
			select f.ProductionType
				, psd.Complete
			from PO_Supp_Detail psd WITH (NOLOCK)
			inner join PO_Supp_Detail_OrderList psdo WITH (NOLOCK) on psd.ID = psdo.ID and psd.SEQ1 = psdo.SEQ1 and psd.SEQ2 = psdo.SEQ2
			outer apply (
				select [ProductionType] = iif(m.ProductionType = 'Packing', 'Packing', 'Sewing')
				from Fabric f WITH (NOLOCK)
				left join MtlType m WITH (NOLOCK) on f.MtlTypeID = m.ID
				where f.SCIRefno = psd.SCIRefno
			)f  
			where psdo.OrderID	= o.ID
			and psd.Junk = 0
		)f
		group by f.ProductionType
	)f
)CompltSP
outer apply(
		select InLine = MIN(w.EstCutDate),OffLine = MAX(w.EstCutDate) 
		from WorkOrder_Distribute wd WITH (NOLOCK)
		inner join WorkOrder w WITH (NOLOCK) on wd.WorkOrderUkey = w.Ukey
		where w.EstCutDate is not null
		and wd.OrderID = o.ID
	)csp
OUTER APPLY(
	select Val = MAX(ScanEditDate)
	from PackingList_Detail pd WITH (NOLOCK)
	where pd.OrderID = o.id
)scanEditDate
{order_QtyShip_OuterApply}    
outer apply(
    select oa.Article 
    from Order_article oa WITH (NOLOCK) 
    where oa.id = o.id
) a
outer apply(
	select ColorID = Stuff((
		select concat(',',ColorID)
		from (
                select od.id,od.seq,od.Article,oc.ColorID
                from orders 
                inner join Order_QtyShip_Detail  od on o.id = od.id
                inner join Order_ColorCombo oc on o.poid = oc.Id and od.Article = oc.Article and oc.PatternPanel = 'FA'
                where orders.id = o.id
                group by od.ID,od.Seq,od.Article,oc.ColorID
			) s
		for xml path ('')
	) , 1, 1, '')
) Order_ColorCombo
outer apply(
	select cnt = count(1)
	from Order_Qtyship
	where id=o.id
	and CFAIs3rdInspect = 1
) oqs
";

            // 第3段 有無勾選 ListPOCombo, 若有勾選,則 union 一段, BI不會走這段
            if (model.ListPOCombo && model.P_type != "forecast")
            {
                sqlcmd += $@"
select * 
into #tmpListPoCombo
from #tmpOrders2

union
select  o.ID
        , o.MDivisionID
        , o.FtyGroup
        , o.FactoryID
        , o.BuyerDelivery
        , o.SciDelivery
        , O.POID
        , o.CRDDate
        , o.CFMDate
        , o.Dest
        , o.StyleID
        , s.StyleName
        , o.SeasonID
        , o.BrandID
        , o.ProjectID
        , Kit=(SELECT top 1 c.Kit From CustCD c WITH (NOLOCK) where c.ID=o.CustCDID AND c.BrandID=o.BrandID)
		,[PackingMethod] = d.Name
        , o.HangerPack
        , o.Customize1
        , o.BuyMonth
        , o.CustPONo
        , o.CustCDID
        , o.ProgramID
        , [NonRevenue]=IIF(o.NonRevenue=1,'Y','N')
        , o.CdCodeID
        , s.CDCodeNew
        , [ProductType] = r2.Name
		, [FabricType] = r1.Name
		, s.Lining
		, s.Gender
		, [Construction] = d1.Name
        , o.CPU
        , o.Qty
        , o.FOCQty
        , o.LocalOrder
        , o.PoPrice
        , o.CMPPrice
        , o.KPILETA
        , o.PFETA
        , o.LETA
        , o.MTLETA
        , o.SewETA
        , o.PackETA
        , o.MTLComplete
        , o.SewInLine
        , o.SewOffLine
        , o.CutInLine
        , o.CutOffLine
        , [CutInLine_SP] = csp.InLine
		, [CutOffLine_SP] = csp.OffLine
        , [3rd_Party_Inspection] = {thridColumn}
        , Category=case when o.Category='B'then'Bulk'
						when o.Category='G'then'Garment'
						when o.Category='M'then'Material'
						when o.Category='S'then'Sample'
						when o.Category='T'then'Sample mtl.'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
					end
        , o.PulloutDate
        , o.ActPulloutDate
        , o.SMR
        , o.MRHandle
        , o.MCHandle
        , o.OrigBuyerDelivery
        , o.DoxType
        , o.TotalCTN
        , PackErrorCtn = isnull(o.PackErrCTN,0)
        , o.FtyCTN
        , o.ClogCTN
        , CFACTN=isnull(o.CFACTN,0)
        , o.VasShas
        , o.TissuePaper
        , o.MTLExport
        , o.SewLine
        , o.ShipModeList
        , o.PlanDate
        , o.FirstProduction
		, o.LastProductionDate
        , o.OrderTypeID
        , Order_ColorCombo.ColorID
        , o.SpecialMark
        , o.SampleReason
        , InspDate = {order_QtyShip_Source_InspDate}
        , InspResult = {order_QtyShip_Source_InspResult}
        , InspHandle = {order_QtyShip_Source_InspHandle}
        , o.MnorderApv2
        , o.MnorderApv
        , o.PulloutComplete
        , {seperCmdkpi}
        , o.KPIChangeReason
        , o.EachConsApv
        , o.Junk
        , o.StyleUkey
        , o.CuttingSP
        , o.RainwearTestPassed
        , o.BrandFTYCode
        , o.CPUFactor
        , o.ClogLastReceiveDate
		, [IsMixMarker]=  CASE WHEN o.IsMixMarker=0 THEN 'Is Single Marker'
							WHEN o.IsMixMarker=1 THEN 'Is Mix  Marker'		
							WHEN o.IsMixMarker=2 THEN ' Is Mix Marker - SCI'
							ELSE ''
						END
        , o.GFR 
		, isForecast = iif(isnull(o.Category,'')='','1','') 
        , [AirFreightByBrand] = IIF(o.AirFreightByBrand='1','Y','')
        , [BuyBack] = iif(exists (select 1 from Order_BuyBack WITH (NOLOCK) where ID = o.ID), 'Y', '')
        , [Cancelled] = case when o.junk = 1 then 
                                case when o.NeedProduction = 1 then 'Y' 
                                    when o.KeepPanels = 1 then 'K'
                                    else 'N' end
                        else ''
                        end
        , o.Customize2
        , o.KpiMNotice
        , o.KpiEachConsCheck
        , o.LastCTNTransDate
		, ScanEditDate = scanEditDate.Val
        , o.LastCTNRecdDate
        , o.DryRoomRecdDate
        , o.DryRoomTransDate
        , o.MdRoomScanDate
        , [VasShasCutOffDate] = Format(DATEADD(DAY, -30, iif(GetMinDate.value	is null, coalesce(o.BuyerDelivery, o.CRDDate, o.PlanDate, o.OrigBuyerDelivery), GetMinDate.value)), 'yyyy/MM/dd')
        , [StyleSpecialMark] = s.SpecialMark
        {seperCmd}
        , [SewingMtlComplt]  = isnull(CompltSP.SewingMtlComplt, '')
        , [PackingMtlComplt] = isnull(CompltSP.PackingMtlComplt, '')
        , o.OrganicCotton
        ,o.DirectShip
        ,[StyleCarryover] = iif(s.NewCO = '2','V','')
        ,o.PackLETA
from #tmpOrders o  WITH (NOLOCK) 
left join style s WITH (NOLOCK) on o.styleukey = s.ukey
left join DropDownList d WITH (NOLOCK) ON o.CtnType=d.ID AND d.Type='PackingMethod'
left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
outer apply (
	    select 
		    [PackingMtlComplt] = max([PackingMtlComplt])
		    , [SewingMtlComplt] = max([SewingMtlComplt])
	    from 
	    (
		    select  f.ProductionType
			    , [PackingMtlComplt] = iif(f.ProductionType = 'Packing' and sum(iif(f.ProductionType = 'Packing', 1, 0)) = sum(iif(f.ProductionType = 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
			    , [SewingMtlComplt] = iif(f.ProductionType <> 'Packing' and sum(iif(f.ProductionType <> 'Packing', 1, 0)) = sum(iif(f.ProductionType <> 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
		    from 
		    (
			    select f.ProductionType
				    , psd.Complete
			    from PO_Supp_Detail psd WITH (NOLOCK)
			    inner join PO_Supp_Detail_OrderList psdo WITH (NOLOCK) on psd.ID = psdo.ID and psd.SEQ1 = psdo.SEQ1 and psd.SEQ2 = psdo.SEQ2
			    outer apply (
				    select [ProductionType] = iif(m.ProductionType = 'Packing', 'Packing', 'Sewing')
				    from Fabric f WITH (NOLOCK)
				    left join MtlType m WITH (NOLOCK) on f.MtlTypeID = m.ID
				    where f.SCIRefno = psd.SCIRefno
			    )f  
			    where psdo.OrderID	= o.ID
			    and psd.Junk = 0
		    )f
		    group by f.ProductionType
	    )f
    )CompltSP
outer apply(
	select InLine = MIN(w.EstCutDate),OffLine = MAX(w.EstCutDate) 
	from WorkOrder_Distribute wd WITH (NOLOCK)
	inner join WorkOrder w WITH (NOLOCK) on wd.WorkOrderUkey = w.Ukey
	where w.EstCutDate is not null
	and wd.OrderID = o.ID
)csp
{seperCmdkpi2}
OUTER APPLY (
    SELECT Name 
    FROM Pass1 WITH (NOLOCK) 
    WHERE Pass1.ID=O.InspHandle
)I
outer apply (
	select value = (
		select Min(Date)
		From (Values (o.BuyerDelivery), (o.CRDDate), (o.PlanDate), (o.OrigBuyerDelivery)) as tmp (Date)
		where tmp.Date is not null
	)
) GetMinDate
OUTER APPLY(
	select Val = MAX(ScanEditDate)
	from PackingList_Detail pd WITH (NOLOCK)
	where pd.OrderID = o.id
)scanEditDate
outer apply(
	select ColorID = Stuff((
		select concat(',',ColorID)
		from (
				select 	distinct ColorID
				from dbo.Order_ColorCombo occ WITH (NOLOCK)
				where occ.ID = o.Poid 
				and occ.Patternpanel = 'FA'
			) s
		for xml path ('')
	) , 1, 1, '')
) Order_ColorCombo
outer apply(
	select cnt = count(1)
	from Order_Qtyship
	where id=o.id
	and CFAIs3rdInspect = 1
) oqs
";
            }
            else
            {
                sqlcmd += @"
select * 
into #tmpListPoCombo
from #tmpOrders2
";
            }

            sqlcmd += @"
CREATE NONCLUSTERED INDEX index_tmpListPoCombo_ID ON #tmpListPoCombo(ID ASC);
";

            // 第4段
            if (model.SeparateByQtyBdownByShipmode && model.P_type.Equals("ALL"))
            {
                sqlcmd += $@"
select pd.OrderID, pd.OrderShipmodeSeq, Sum( pd.CTNQty) PackingCTN ,
	Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) TotalCTN,
	Sum( case when p.Type in ('B', 'L') and pd.TransferDate is null then pd.CTNQty else 0 end) FtyCtn,
	Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end) ClogCTN ,
	Sum(case when p.Type <> 'F'  then pd.ShipQty else 0 end) PackingQty ,
	Sum(case when p.Type = 'F'   then pd.ShipQty else 0 end) PackingFOCQty ,
	Sum(case when p.Type in ('B', 'L') and p.INVNo <> ''  then pd.ShipQty else 0 end) BookingQty ,
	Max (ReceiveDate) ClogRcvDate,
	MAX(p.PulloutDate)  ActPulloutDate
into #tmp_PLDetial
from PackingList_Detail pd WITH (NOLOCK) 
LEFT JOIN PackingList p WITH (NOLOCK) on pd.ID = p.ID 
inner join (select distinct id, seq from #tmpListPoCombo) t on pd.OrderID = t.ID  and pd.OrderShipmodeSeq = t.Seq
group by pd.OrderID, pd.OrderShipmodeSeq 

select  t.ID
        , t.MDivisionID
        , t.FtyGroup
        , t.FactoryID
        , oq.BuyerDelivery
        , t.SciDelivery
        , t.POID
        , t.CRDDate
        , t.CFMDate
        , t.Dest
        , t.StyleID
        , t.StyleName
        , t.SeasonID
        , t.BrandID
        , t.ProjectID
		, t.PackingMethod
        , t.HangerPack
        , t.Customize1
        , t.BuyMonth
        , t.CustPONo
        , t.CustCDID
		, t.Kit
        , t.ProgramID
		, t.NonRevenue
        , t.CdCodeID
	    , t.CDCodeNew
	    , t.ProductType
	    , t.FabricType
	    , t.Lining
	    , t.Gender
	    , t.Construction
        , t.CPU
        , oq.Qty as Qty
        , t.FOCQty
        , t.LocalOrder
        , t.PoPrice
        , t.CMPPrice
        , t.KPILETA
        , t.PFETA
        , t.LETA
        , t.MTLETA
        , t.SewETA
        , t.PackETA
        , t.MTLComplete
        , t.SewInLine
        , t.SewOffLine
        , t.CutInLine
        , t.CutOffLine
		, t.CutInLine_SP
		, t.CutOffLine_SP
        , t.[3rd_Party_Inspection]
        , t.Category
        , PulloutDate = oq.EstPulloutDate
        , pdm.ActPulloutDate 
        , t.SMR
        , t.MRHandle
        , t.MCHandle
        , t.OrigBuyerDelivery
        , t.DoxType
        , t.VasShas
        , t.TissuePaper
        , t.MTLExport
        , t.SewLine
        , oq.ShipmodeID as ShipModeList
        , t.PlanDate
        , t.FirstProduction
        , t.LastProductionDate
        , t.OrderTypeID
        , t.ColorID
        , t.SpecialMark
        , t.SampleReason
        , t.InspDate
        , t.InspResult
        , t.InspHandle
        , t.MnorderApv2
        , t.MnorderApv
        , t.PulloutComplete
        , t.FtyKPI
        , t.KPIChangeReason
        , t.EachConsApv
        , t.Junk
        , t.StyleUkey
        , t.CuttingSP
        , t.RainwearTestPassed
        , t.BrandFTYCode
        , t.CPUFactor
        , oq.Seq
        , [IDD] = Format(oq.IDD, 'yyyy/MM/dd')
        , t.ClogLastReceiveDate
        , t.IsMixMarker
        , t.GFR
        , pdm.PackingQty
		, pdm.PackingFOCQty 
		, pdm.BookingQty
		, pdm.PackingCTN
		, pdm.TotalCTN as  TotalCTN1
		, pdm.FtyCtn as  FtyCtn1
		, pdm.ClogCTN as  ClogCTN1
		, pdm.ClogRcvDate
        , t.PackErrorCtn
        , t.CFACTN
		, t.isForecast
		, t.AirFreightByBrand
        , [BuyBack] = iif(exists (select 1 from Order_BuyBack with (nolock) where ID = t.ID), 'Y', '')
        , t.Cancelled
        , t.Customize2
        , t.KpiMNotice
        , t.KpiEachConsCheck
        , t.LastCTNTransDate
        , t.ScanEditDate
        , t.LastCTNRecdDate
        , t.DryRoomRecdDate
        , t.DryRoomTransDate
        , t.MdRoomScanDate
        , t.VasShasCutOffDate
        , t.StyleSpecialMark
        , t.SewingMtlComplt
        , t.PackingMtlComplt
        , t.OrganicCotton
        , t.DirectShip
        , t.[StyleCarryover]
        , t.PackLETA
into #tmpFilterSeperate
from #tmpListPoCombo t
inner join Order_QtyShip oq WITH(NOLOCK) on t.ID = oq.Id and t.Seq = oq.Seq
left join #tmp_PLDetial pdm on pdm.OrderID = t.ID  and pdm.OrderShipmodeSeq = t.Seq ;

CREATE NONCLUSTERED INDEX index_tmpFilterSeperate ON #tmpFilterSeperate(	ID ASC,seq asc);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_POID ON #tmpFilterSeperate(	POID ASC);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_CuttingSP ON #tmpFilterSeperate(	CuttingSP ASC);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_StyleUkey ON #tmpFilterSeperate(	StyleUkey ASC);


select sod.OrderId,Sum( case when sod.ComboType = 'T'  then sod.QAQty else 0 end) SewQtyTop, 
	Sum( case when sod.ComboType = 'B'  then sod.QAQty else 0 end) SewQtyBottom, 
	Sum( case when sod.ComboType = 'I'  then sod.QAQty else 0 end) SewQtyInner, 
	Sum( case when sod.ComboType = 'O'  then sod.QAQty else 0 end) SewQtyOuter,
	Min (so.OutputDate) FirstOutDate,
	Max (so.OutputDate) LastOutDate
into #tmp_sewDetial
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
inner join (select distinct ID from #tmpFilterSeperate) t on sod.OrderId = t.ID
group by sod.OrderId

select ID, Remark
into #tmp_PFRemark
from (
	select ROW_NUMBER() OVER (PARTITION BY o.ID ORDER BY o.addDate, o.Ukey) r_id
		,o.ID, o.Remark
	from Order_PFHis o WITH (NOLOCK) 
	inner join #tmpFilterSeperate t on o.ID = t.ID 
	where AddDate = (
			select Max(o.AddDate) 
			from Order_PFHis o WITH (NOLOCK) 
			where ID = t.ID
		)   
	group by o.ID, o.Remark ,o.addDate, o.Ukey
)a
where r_id = '1' 

select ed.POID,Max(e.WhseArrival) ArriveWHDate 
into #tmp_ArriveWHDate
from Export e WITH (NOLOCK) 
inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
inner join #tmpFilterSeperate t on ed.POID = t.POID
group by ed.POID 

select StyleUkey ,dbo.GetSimilarStyleList(StyleUkey) GetStyleUkey
into #tmp_StyleUkey
from #tmpFilterSeperate 
group by StyleUkey 

select POID ,dbo.GetHaveDelaySupp(POID) MTLDelay
into #tmp_MTLDelay
from #tmpFilterSeperate 
group by POID 

select pd.OrderID, pd.OrderShipmodeSeq, sum(pd.ShipQty) PulloutQty
into #tmp_PulloutQty
from PackingList_Detail pd WITH (NOLOCK)
inner join #tmpFilterSeperate t on pd.OrderID = t.ID and pd.OrderShipmodeSeq = t.Seq
inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
where p.PulloutID <> ''
group by pd.OrderID, pd.OrderShipmodeSeq

select pd.OrderID, count(distinct p.PulloutID) ActPulloutTime
into #tmp_ActPulloutTime
from PackingList p WITH (NOLOCK)
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join #tmpFilterSeperate t on t.ID = pd.OrderID
where p.PulloutID <> ''
and pd.ShipQty > 0
group by pd.OrderID

select od.ID,od.Seq,od.Article 
into #tmp_Article
from Order_QtyShip_Detail od WITH (NOLOCK) 
inner join #tmpFilterSeperate t on  od.ID = t.ID and od.Seq = t.Seq 
group by od.ID,od.Seq,od.Article 


CREATE NONCLUSTERED INDEX index_tmp_sewDetial ON #tmp_sewDetial(	OrderId ASC);
CREATE NONCLUSTERED INDEX index_tmp_PFRemark ON #tmp_PFRemark(	ID ASC);
CREATE NONCLUSTERED INDEX index_tmp_ArriveWHDate ON #tmp_ArriveWHDate(	PoID ASC);
CREATE NONCLUSTERED INDEX index_tmp_StyleUkey ON #tmp_StyleUkey(	StyleUkey ASC);
CREATE NONCLUSTERED INDEX index_tmp_MTLDelay ON #tmp_MTLDelay(	POID ASC);
CREATE NONCLUSTERED INDEX index_tmp_PulloutQty ON #tmp_PulloutQty(	OrderID ASC, OrderShipmodeSeq);
CREATE NONCLUSTERED INDEX index_tmp_ActPulloutTime ON #tmp_ActPulloutTime(	OrderID ASC);


select  t.ID
            , t.MDivisionID
            , t.FtyGroup
            , t.FactoryID
            , t.BuyerDelivery
            , t.SciDelivery
            , t.POID
            , t.CRDDate
            , t.CFMDate
            , t.Dest
            , t.StyleID
            , t.StyleName
            , t.SeasonID
            , t.BrandID
            , t.ProjectID
			, t.PackingMethod
            , t.HangerPack
            , t.Customize1
            , t.BuyMonth
            , t.CustPONo
            , t.CustCDID
			, t.Kit
            , t.ProgramID
			, t.NonRevenue
            , t.CdCodeID
	        , t.CDCodeNew
	        , t.ProductType
	        , t.FabricType
	        , t.Lining
	        , t.Gender
	        , t.Construction
            , t.CPU
            , t.Qty as Qty
            , t.FOCQty
            , t.LocalOrder
            , t.PoPrice
            , t.CMPPrice
            , t.KPILETA
            , t.PFETA
            , t.LETA
            , t.MTLETA
            , t.SewETA
            , t.PackETA
            , t.MTLComplete
            , t.SewInLine
            , t.SewOffLine
            , t.CutInLine
            , t.CutOffLine
			, t.CutInLine_SP
			, t.CutOffLine_SP
            , t.[3rd_Party_Inspection]
            , t.Category
            , t.PulloutDate
            , t.ActPulloutDate 
            , t.SMR
            , t.MRHandle
            , t.MCHandle
            , t.OrigBuyerDelivery
            , t.DoxType
            , t.VasShas
            , t.TissuePaper
            , t.MTLExport
            , t.SewLine
            , t.ShipModeList
            , t.PlanDate
            , t.FirstProduction
            , t.LastProductionDate
            , t.OrderTypeID
            , t.ColorID
            , t.SpecialMark
            , t.SampleReason
            , t.InspDate
            , t.InspResult
            , t.InspHandle
            , t.MnorderApv2
            , t.MnorderApv
            , t.PulloutComplete
            , t.FtyKPI
            , t.KPIChangeReason
            , t.EachConsApv
            , t.Junk
            , t.StyleUkey
            , t.CuttingSP
            , t.RainwearTestPassed
            , t.BrandFTYCode
            , t.CPUFactor
            , t.Seq
            , t.[IDD]
            , t.ClogLastReceiveDate
            , t.IsMixMarker
            , t.GFR
            , t.PackingQty
			, t.PackingFOCQty 
			, t.BookingQty
			, t.PackingCTN
			, t.TotalCTN1
			, t.FtyCtn1
			, t.ClogCTN1
			, t.ClogRcvDate
            , t.PackErrorCtn
            , t.CFACTN
			, t.isForecast
			, t.AirFreightByBrand
            , [BuyBack] = iif(exists (select 1 from Order_BuyBack WITH (NOLOCK) where ID = t.ID), 'Y', '')
            , t.Cancelled
            , t.Customize2
            , t.KpiMNotice
            , t.KpiEachConsCheck
        , ModularParent = isnull (s.ModularParent, '')
        , CPUAdjusted = isnull (s.CPUAdjusted * 100, 0)
        , DestAlias = isnull (c.Alias, '')
        , FactoryDisclaimer = isnull (dd.Name, '')
        , FactoryDisclaimerRemark = isnull (s.ExpectionFormRemark, '')
        , ApprovedRejectedDate  = s.ExpectionFormDate
        , WorkType = isnull (ct.WorkType, '')
        , ct.FirstCutDate
        , POSMR = isnull (p.POSMR, '')
        , POHandle = isnull (p.POHandle, '') 
        , PCHandle = isnull (p.PCHandle, '') 
        , FTYRemark = isnull (s.FTYRemark, '')
        , som.SewQtyTop
        , som.SewQtyBottom
        , som.SewQtyInner
        , som.SewQtyOuter
        , TtlSewQty = isnull (dbo.getMinCompleteSewQty (t.ID, null, null) ,0)
        , CutQty = isnull ((select SUM(Qty) 
                            from CuttingOutput_WIP WITH (NOLOCK) 
                            where OrderID = t.ID)
                          , 0)
        , PFRemark = isnull(pf.Remark,'')
        , EarliestSCIDlv =dbo.getMinSCIDelivery(t.POID,'')
        , KPIChangeReasonName = isnull ((select Name 
                                         from Reason WITH (NOLOCK) 
                                         where  ReasonTypeID = 'Order_BuyerDelivery' 
                                                and ID = t.KPIChangeReason)
                                       , '')
        , SMRName = isnull ((select Name 
                             from TPEPass1 WITH (NOLOCK) 
                             where Id = t.SMR)
                           , '')
        , MRHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK) 
                                  where Id = t.MRHandle)
                                , '')
        , POSMRName = isnull ((select Name 
                               from TPEPass1 WITH (NOLOCK) 
                               where Id = p.POSMR)
                             , '')
        , POHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK)
                                  where Id = p.POHandle)
                                , '')
        , PCHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK)
                                  where Id = p.PCHandle)
                                , '')
        , MCHandleName = isnull ((select Name 
                                  from Pass1 WITH (NOLOCK) 
                                  where Id = t.MCHandle)
                                , '')
        , SampleReasonName = isnull ((select Name 
                                      from Reason WITH (NOLOCK) 
                                      where ReasonTypeID = 'Order_reMakeSample' 
                                            and ID = t.SampleReason)
                                    , '')
        , SpecialMarkName = isnull ((select Name 
                                     from Style_SpecialMark sp WITH(NOLOCK) 
                                     where sp.ID = t.[StyleSpecialMark]
                                     and sp.BrandID = t.BrandID
                                     and sp.Junk = 0)
                                    , '') 
        , MTLExportTimes = isnull ([dbo].getMTLExport (t.POID, t.MTLExport), '')
        , GMTLT = dbo.GetGMTLT(t.BrandID, t.StyleID, t.SeasonID, t.FactoryID,t.ID)
        , SimilarStyle = su.GetStyleUkey         
        , MTLDelay = isnull(mt.MTLDelay ,0)
        , InvoiceAdjQty = dbo.getInvAdjQty (t.ID, t.Seq) 
		, FOCAdjQty = dbo.getFOCInvAdjQty (t.ID, t.Seq) 
		, NotFOCAdjQty= dbo.getInvAdjQty (t.ID, t.Seq)-dbo.getFOCInvAdjQty (t.ID, t.Seq) 
        , ct.LastCutDate
        , ArriveWHDate =　aw.ArriveWHDate
        , som.FirstOutDate
        , som.LastOutDate 
        , PulloutQty = isnull(pu.PulloutQty,0)
        , ActPulloutTime = isnull(apu.ActPulloutTime,0) 
        , Article = isnull ((select CONCAT(Article,',') 
                             from #tmp_Article a 
							 where a.ID = t.ID and a.Seq = t.Seq
							 for xml path(''))
                           , '')
        , [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='F')
        , [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='A')
		, t.Cancelled
        , t.Customize2
        , t.KpiMNotice
        , t.KpiEachConsCheck
        , LastCTNTransDate = IIF(isnull(t.FtyCtn1,0) =0 , t.LastCTNTransDate , null)
		, ScanEditDate = IIF(isnull(t.FtyCtn1,0) =0 , t.ScanEditDate , null)
		, LastCTNRecdDate = IIF(isnull(t.FtyCtn1,0) =0 , t.LastCTNRecdDate , null)
		, DryRoomRecdDate = IIF(isnull(t.FtyCtn1,0) =0 , t.DryRoomRecdDate , null)
		, DryRoomTransDate = IIF(isnull(t.FtyCtn1,0) =0 , t.DryRoomTransDate , null)
		, MdRoomScanDate = IIF(isnull(t.FtyCtn1,0) =0 , t.MdRoomScanDate , null)
        , t.VasShasCutOffDate
        , t.SewingMtlComplt
        , t.PackingMtlComplt
        , [OrganicCotton] = iif(t.OrganicCotton = 1, 'Y', 'N')
        , [DirectShip] = iif(t.DirectShip = 1, 'V','')
        ,[StyleCarryover] = iif(s.NewCO = '2','V','')
        , t.PackLETA
from #tmpFilterSeperate t
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join DropDownList dd WITH (NOLOCK) on dd.Type = 'FactoryDisclaimer' and dd.id = s.ExpectionFormStatus
left join PO p WITH (NOLOCK) on p.ID = t.POID
left join Country c WITH (NOLOCK) on c.ID = t.Dest
left join #tmp_sewDetial som on som.OrderID = t.ID
left join #tmp_PFRemark pf on pf.ID = t.ID
left join #tmp_ArriveWHDate aw on aw.PoID = t.POID
left join #tmp_StyleUkey su on su.StyleUkey = t.StyleUkey 
left join #tmp_MTLDelay mt on mt.POID = t.POID
left join #tmp_PulloutQty pu on pu.OrderID = t.ID and pu.OrderShipmodeSeq = t.Seq
left join #tmp_ActPulloutTime apu on apu.OrderID = t.ID 
order by t.ID;
drop table #tmpListPoCombo;
drop table #tmp_PLDetial,#tmpFilterSeperate,#tmp_sewDetial,#tmp_PFRemark,#tmp_ArriveWHDate,#tmp_StyleUkey,#tmp_MTLDelay,#tmp_PulloutQty,#tmp_ActPulloutTime,#tmp_Article;";
            }
            else
            {
                sqlcmd += $@"
select ID, Remark
into #tmp_PFRemark
from (
	select ROW_NUMBER() OVER (PARTITION BY o.ID ORDER BY o.addDate, o.Ukey) r_id
		,o.ID, o.Remark
	from Order_PFHis o WITH (NOLOCK) 
	inner join #tmpListPoCombo t on o.ID = t.ID 
	where AddDate = (
			select Max(o.AddDate) 
			from Order_PFHis o WITH (NOLOCK) 
			where ID = t.ID
		)   
	group by o.ID, o.Remark ,o.addDate, o.Ukey
)a
where r_id = '1'

select StyleUkey ,dbo.GetSimilarStyleList(StyleUkey) GetStyleUkey
into #tmp_StyleUkey
from #tmpListPoCombo
group by StyleUkey 

select POID ,dbo.GetHaveDelaySupp(POID) MTLDelay
into #tmp_MTLDelay
from #tmpListPoCombo
group by POID 		    

select pld.OrderID, SUM(pld.ShipQty) PackingQty
into #tmp_PackingQty
from PackingList pl WITH (NOLOCK) 
inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
inner join #tmpListPoCombo t on pld.OrderID = t.ID
where  pl.Type <> 'F'  
group by pld.OrderID  

select pld.OrderID, SUM(pld.ShipQty) PackingFOCQty 
into #tmp_PackingFOCQty
from PackingList pl WITH (NOLOCK) 
inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
inner join #tmpListPoCombo t on pld.OrderID = t.ID
where pl.Type = 'F' 
group by pld.OrderID 

select pld.OrderID, SUM(pld.ShipQty) BookingQty
into #tmp_BookingQty
from PackingList pl WITH (NOLOCK) 
inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
inner join #tmpListPoCombo t on pld.OrderID = t.ID
where   (pl.Type = 'B' or pl.Type = 'S') 
        and pl.INVNo <> ''  
group by pld.OrderID 
 
select o.ID, o.Article 
into #tmp_Article
from Order_Qty o WITH (NOLOCK) 
inner join #tmpListPoCombo t on o.ID = t.ID
group by o.ID,o.Article 

select sd.OrderId, sd.QAQty, sd.ComboType, s.OutputDate
into #tmp_SewingOutput
from SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
where exists (select 1 from #tmpOrders t where sd.OrderId = t.ID)

CREATE NONCLUSTERED INDEX index_tmp_PFRemark ON #tmp_PFRemark(	ID ASC);
CREATE NONCLUSTERED INDEX index_tmp_StyleUkey ON #tmp_StyleUkey(	StyleUkey ASC);
CREATE NONCLUSTERED INDEX index_tmp_MTLDelay ON #tmp_MTLDelay(	POID ASC);
CREATE NONCLUSTERED INDEX index_tmp_PackingQty ON #tmp_PackingQty(	OrderID ASC);
CREATE NONCLUSTERED INDEX index_tmp_PackingFOCQty ON #tmp_PackingFOCQty(	OrderID ASC);
CREATE NONCLUSTERED INDEX index_tmp_BookingQty ON #tmp_BookingQty(	OrderID ASC);

select distinct 
              t.ID
            , t.MDivisionID
            , t.FtyGroup
            , t.FactoryID
            , t.BuyerDelivery
            , t.SciDelivery
            , t.POID
            , t.CRDDate
            , t.CFMDate
            , t.Dest
            , t.StyleID
            , t.StyleName
            , t.SeasonID
            , t.BrandID
            , t.ProjectID
            , t.Kit
			,[PackingMethod] 
            , t.HangerPack
            , t.Customize1
            , t.BuyMonth
            , t.CustPONo
            , t.CustCDID
            , t.ProgramID
			, [NonRevenue]
            , t.CdCodeID
	        , t.CDCodeNew
            , [ProductType]
		    , t. [FabricType]
		    , t.Lining
		    , t.Gender
		    , t.[Construction]
            , t.CPU
            , t.Qty
            , t.FOCQty
            , t.LocalOrder
            , t.PoPrice
            , t.CMPPrice
            , t.KPILETA
            , t.PFETA
            , t.LETA
            , t.MTLETA
            , t.SewETA
            , t.PackETA
            , t.MTLComplete
            , t.SewInLine
            , t.SewOffLine
            , t.CutInLine
            , t.CutOffLine
			, t.CutInLine_SP
			, t.CutOffLine_SP
            , t.[3rd_Party_Inspection]
            , t.Category
            , t.PulloutDate
            , t.ActPulloutDate
            , t.SMR
            , t.MRHandle
            , t.MCHandle
            , t.OrigBuyerDelivery
            , t.DoxType
            , t.TotalCTN
            , PackErrorCtn
            , t.FtyCTN
            , t.ClogCTN
            , t.CFACTN
            , t.VasShas
            , t.TissuePaper
            , [MTLExport]
            , t.SewLine
            , t.ShipModeList
            , t.PlanDate
            , t.FirstProduction
			, t.LastProductionDate
            , t.OrderTypeID
            , t.ColorID
            , t.SpecialMark
            , t.SampleReason
            , InspDate 
            , InspResult 
            , InspHandle 
            , t.MnorderApv2
            , t.MnorderApv
            , t.PulloutComplete
            , t.FtyKPI
            , t.KPIChangeReason
            , t.EachConsApv
            , t.Junk
            , t.StyleUkey
            , t.CuttingSP
            , t.RainwearTestPassed
            , t.BrandFTYCode
            , t.CPUFactor
            , t.ClogLastReceiveDate
			, [IsMixMarker]
            , t.GFR 
			, isForecast
            , [AirFreightByBrand]
            , [BuyBack]
            , [Cancelled]
            , t.Customize2
            , t.KpiMNotice
            , t.KpiEachConsCheck
            , t.[IDD] 
        , ModularParent = isnull (s.ModularParent, '')  
        , CPUAdjusted = isnull(s.CPUAdjusted * 100, 0)  
        , DestAlias = isnull (c.Alias, '') 
        , FactoryDisclaimer = isnull (dd.Name, '')
        , FactoryDisclaimerRemark = isnull (s.ExpectionFormRemark, '')
        , ApprovedRejectedDate  = s.ExpectionFormDate
        , WorkType = isnull(ct.WorkType,'')
        , ct.FirstCutDate
        , POSMR = isnull (p.POSMR, '')
        , POHandle = isnull (p.POHandle, '') 
        , PCHandle = isnull (p.PCHandle, '') 
        , FTYRemark = isnull (s.FTYRemark, '')
        , SewQtyTop = isnull ((select SUM(QAQty) 
                               from SewingOutput_Detail WITH (NOLOCK) 
                               where OrderId = t.ID 
                                     and ComboType = 'T')
                             , 0)
        , SewQtyBottom = isnull ((select SUM(QAQty) 
                                  from SewingOutput_Detail WITH (NOLOCK) 
                                  where OrderId = t.ID 
                                        and ComboType = 'B')
                                , 0)
        , SewQtyInner = isnull ((select SUM(QAQty) 
                                 from SewingOutput_Detail WITH (NOLOCK) 
                                 where OrderId = t.ID 
                                       and ComboType = 'I')
                               , 0) 
        , SewQtyOuter = isnull ((select SUM(QAQty) 
                                 from SewingOutput_Detail WITH (NOLOCK) 
                                 where OrderId = t.ID 
                                       and ComboType = 'O')
                               , 0)
        , TtlSewQty = isnull (dbo.getMinCompleteSewQty (t.ID, null, null), 0)
        , CutQty = isnull ((select SUM(Qty) 
                            from CuttingOutput_WIP WITH (NOLOCK) 
                            where OrderID = t.ID)
                          , 0)
        , PFRemark = isnull(pf.Remark,'')
        , EarliestSCIDlv = dbo.getMinSCIDelivery (t.POID, '')
        , KPIChangeReasonName = isnull ((select Name 
                                         from Reason WITH (NOLOCK)  
                                         where  ReasonTypeID = 'Order_BuyerDelivery' 
                                                and ID = t.KPIChangeReason)
                                        , '')
        , SMRName = isnull ((select Name 
                             from TPEPass1 WITH (NOLOCK) 
                             where Id = t.SMR)
                            , '')
        , MRHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK) 
                                  where Id = t.MRHandle)
                                , '')
        , POSMRName = isnull ((select Name 
                               from TPEPass1 WITH (NOLOCK) 
                               where Id = p.POSMR)
                             , '')
        , POHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK) 
                                  where Id = p.POHandle)
                                , '')
        , PCHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK)
                                  where Id = p.PCHandle)
                                , '')
        , MCHandleName = isnull ((select Name 
                                  from Pass1 WITH (NOLOCK) 
                                  where Id = t.MCHandle)
                                , '')
        , SampleReasonName = isnull ((select Name 
                                      from Reason WITH (NOLOCK) 
                                      where ReasonTypeID = 'Order_reMakeSample' 
                                            and ID = t.SampleReason)
                                    , '') 
        , SpecialMarkName = isnull ((select Name 
                                     from Style_SpecialMark sp WITH(NOLOCK) 
                                     where sp.ID = t.[StyleSpecialMark] 
                                     and sp.BrandID = t.BrandID
                                     and sp.Junk = 0)
                                   , '')
        , MTLExportTimes = isnull ([dbo].getMTLExport (t.POID, t.MTLExport), '')
        , GMTLT = dbo.GetGMTLT(t.BrandID, t.StyleID, t.SeasonID, t.FactoryID, t.ID)
        , SimilarStyle = su.GetStyleUkey
        , MTLDelay = isnull(mt.MTLDelay,0)
        , PackingQty = isnull(pa.PackingQty ,0)
        , PackingFOCQty = isnull(paf.PackingFOCQty,0)
        , BookingQty = isnull(bo.BookingQty ,0)
        , InvoiceAdjQty = isnull (i.value, 0)
		, FOCAdjQty = isnull (i2.value, 0)
		, NotFOCAdjQty= isnull (i.value, 0)-isnull (i2.value, 0)
        , ct.LastCutDate
        , ArriveWHDate = (select Max(e.WhseArrival) 
                          from Export e WITH (NOLOCK) 
                          inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID 
                          where ed.POID = t.POID) 
        , FirstOutDate = (select Min(so.OutputDate) 
                          from SewingOutput so WITH (NOLOCK) 
                          inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                          where sod.OrderID = t.ID) 
        , LastOutDate = (select Max(so.OutputDate) 
                         from SewingOutput so WITH (NOLOCK) 
                         inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                         where sod.OrderID = t.ID)
        , PulloutQty = isnull ((select sum(pd.ShipQty)
                                from PackingList_Detail pd WITH (NOLOCK)
                                inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                                where p.PulloutID <> ''
                                and pd.OrderID = t.ID)
                              , 0)
        , ActPulloutTime = (select count(distinct p.PulloutID)
                            from PackingList_Detail pd WITH (NOLOCK)
                            inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                            where p.PulloutID <> ''
                            and pd.OrderID = t.ID
                            and pd.ShipQty > 0)
        , PackingCTN = isnull ((select Sum(CTNQty) 
                                from PackingList_Detail WITH (NOLOCK) 
                                where OrderID = t.ID)
                              , 0) 
        , t.TotalCTN
        , FtyCtn = isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0)
        , ClogCTN = isnull(t.ClogCTN,0)
        , ClogRcvDate = t.ClogLastReceiveDate
		, Article = isnull ((select CONCAT(a.Article, ',') 
                             from #tmp_Article a 
							 where a.ID = t.ID
							 for xml path(''))
                           , '') 
        , [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='F')
        , [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='A')
        , t.Customize2
        , t.KpiMNotice
        , t.KpiEachConsCheck
        , [LastCTNTransDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.LastCTNTransDate, null)
        , [ScanEditDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.ScanEditDate, null)
        , [LastCTNRecdDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.LastCTNRecdDate, null)
        , [DryRoomRecdDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.DryRoomRecdDate, null)
        , [DryRoomTransDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.DryRoomTransDate, null)
        , [MdRoomScanDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.MdRoomScanDate, null)
        , t.VasShasCutOffDate
        , t.SewingMtlComplt
        , t.PackingMtlComplt
        , [OrganicCotton] = iif(t.OrganicCotton = 1, 'Y', 'N')
        , [DirectShip] = iif(t.DirectShip = 1, 'V','')
        ,[StyleCarryover] = iif(s.NewCO = '2','V','')
        , t.PackLETA
        {biMoreColumn2}
from #tmpListPoCombo t
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join DropDownList dd WITH (NOLOCK) on dd.Type = 'FactoryDisclaimer' and dd.id = s.ExpectionFormStatus
left join PO p WITH (NOLOCK) on p.ID = t.POID
left join Country c WITH (NOLOCK) on c.ID = t.Dest
left join #tmp_PFRemark pf on pf.ID = t.ID
left join #tmp_StyleUkey su on su.StyleUkey = t.StyleUkey 
left join #tmp_MTLDelay mt on mt.POID = t.POID
left join #tmp_PackingQty pa on pa.OrderID = t.ID
left join #tmp_PackingFOCQty paf on paf.OrderID = t.ID
left join #tmp_BookingQty bo on bo.OrderID = t.ID
outer apply(
	select value = sum(iq.DiffQty) 
	from InvAdjust i WITH (NOLOCK) 
	inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID = iq.ID
	where i.OrderID = t.ID
)i
outer apply(
	select value = sum(iq.DiffQty) 
	from InvAdjust i WITH (NOLOCK) 
	inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID = iq.ID
	where i.OrderID = t.ID
    and iq.Price = 0
)i2
order by t.ID;

drop table #tmpListPoCombo,#tmp_PFRemark,#tmp_StyleUkey,#tmp_MTLDelay,#tmp_PackingQty,#tmp_PackingFOCQty,#tmp_BookingQty,#tmp_Article;";
            }

            return sqlcmd;
        }

        /// <summary>
        /// PPIC R03 有勾選 1 & 2 才有這段
        /// 1.Include Artwork data
        /// 2.Include Artwork data -- Kind is 'PAP'
        /// BI
        /// 1.= 有勾選 Include Artwork data
        /// 2.= 沒勾選 Include Artwork data -- Kind is 'PAP'
        /// 3.= 沒勾選 Printing Detail
        /// 4.= 沒勾選 by CPU
        /// 結果:撈出3個欄位 OrderID, ColumnName(顯示用), 對應值
        /// 使用1.BI 直接寫入 P_PPICMasterList_Extend
        /// 使用2.R03 樞紐後, 在報表最右方
        /// </summary>
        private string GetArtworkTypeValue(PPIC_R03_ViewModel model)
        {
            int byCPUsqlbit = model.ByCPU ? 1 : 0;

            #region Classify  1.Include Artwork data ||  2.'PAP'
            List<string> listClassify = new List<string>();
            if (model.IncludeArtworkData)
            {
                listClassify.Add("'I'");
                listClassify.Add("'S'");
                listClassify.Add("'A'");
                listClassify.Add("'O'");
            }

            if (model.IncludeArtworkDataKindIsPAP)
            {
                listClassify.Add("'P'");
            }

            string whereClassify = listClassify.JoinToString(",");
            #endregion

            // 勾選 Printing Detail
            string sql_printingDetail = string.Empty;
            if (model.PrintingDetail)
            {
                sql_printingDetail = @"
UNION ALL
SELECT
    ID = 'PRINTING'
    ,Seq = ''
    ,ArtworkUnit = ''
    ,ProductionUnit = ''
    ,SystemType = ''
    ,FakeID = ''
    ,ColumnN = 'Printing LT'
    ,ColumnSeq = -1 

UNION ALL
SELECT
    ID = 'PRINTING'
    ,Seq = ''
    ,ArtworkUnit = ''
    ,ProductionUnit = ''
    ,SystemType = ''
    ,FakeID = ''
    ,ColumnN = 'InkType/Color/Size'
    ,ColumnSeq = 0 
";
            }

            string sqloutput;
            if (model.IsPowerBI)
            {
                sqloutput = @"
--彙整 ColumnN 和計算 Value
SELECT
    a.ID
    ,[ColumnN] = AUnitRno.ColumnN
    ,[Val] = CAST(
        CASE
            WHEN a.AUnitRno IS NOT NULL THEN a.Qty
            ELSE NULL
        END AS VARCHAR(100))
INTO #tmp_ArtworkTypeValue
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData AUnitRno ON a.AUnitRno = AUnitRno.rno

UNION ALL
SELECT
    a.ID
    ,[ColumnN] = PUnitRno.ColumnN
    ,[Val] = CAST(
        CASE
        WHEN a.PUnitRno IS NOT NULL THEN CASE
                WHEN a.ProductionUnit = 'TMS' THEN a.TMS
                ELSE a.Price
            END
        ELSE NULL
        END AS VARCHAR(100))
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData PUnitRno ON a.PUnitRno = PUnitRno.rno

UNION ALL
SELECT
    a.ID
    ,[ColumnN] = NRno.ColumnN
    ,[Val] = CAST(
        CASE
        WHEN a.NRno IS NOT NULL THEN a.Qty
        ELSE NULL
        END AS VARCHAR(100))
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData NRno ON a.NRno = NRno.rno

UNION ALL
SELECT
    a.ID
    ,[ColumnN] = TAUnitRno.ColumnN
    ,[Val] = CAST(
        CASE
        WHEN a.TAUnitRno IS NOT NULL THEN b.Qty * a.Qty
        ELSE NULL
        END AS VARCHAR(100))
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData TAUnitRno ON a.TAUnitRno = TAUnitRno.rno
INNER JOIN #tmpOrders b ON a.ID = b.ID

UNION ALL
SELECT
    a.ID
    ,[ColumnN] = TPUnitRno.ColumnN
    ,[Val] = CAST(
        CASE
        WHEN a.TPUnitRno IS NOT NULL THEN
            CASE
            WHEN a.ProductionUnit = 'TMS' THEN b.Qty * a.TMS
            ELSE b.Qty * a.Price
            END
        ELSE NULL
        END AS VARCHAR(100))
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData TPUnitRno ON a.TPUnitRno = TPUnitRno.rno
INNER JOIN #tmpOrders b ON a.ID = b.ID

UNION ALL
SELECT
    a.ID
    ,[ColumnN] = TNRno.ColumnN
    ,[Val] = CAST(
        CASE
        WHEN a.TNRno IS NOT NULL THEN b.Qty * a.Qty
        ELSE NULL
        END AS VARCHAR(100))
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData TNRno ON a.TNRno = TNRno.rno
INNER JOIN #tmpOrders b ON a.ID = b.ID

UNION ALL
SELECT
    a.ID
    ,[ColumnN] = 'POSubCon'
    ,[Val] = a.PoSupp
FROM #tmp_LastArtworkType a
WHERE ISNULL(a.PoSupp, '') <> ''

UNION ALL
SELECT
    a.ID
    ,[ColumnN] = 'SubCon'
    ,[Val] = a.Supp
FROM #tmp_LastArtworkType a
WHERE ISNULL(a.Supp, '') <> ''

UNION ALL
SELECT
    b.ID
    ,[ColumnN] = 'TTL_TMS'
    ,[Val] = CAST(b.Qty * b.CPU * @StdTMS AS VARCHAR(100))
FROM #tmpOrders b

--BI 更新到 P_PPICMasterList_Exten
SELECT
    [OrderID] = t.ID
   ,[ColumnName] = t.ColumnN
   ,[ColumnValue] = SUM(ISNULL(TRY_CONVERT(NUMERIC(38, 6), t.Val), 0))
FROM #tmp_ArtworkTypeValue t
GROUP BY t.ID, t.ColumnN";
            }
            else
            {
                sqloutput = @"
--PPIC R03
SELECT * FROM #tmpArtworkData
SELECT * FROM #tmp_LastArtworkType
";
            }

            string sqlcmd = $@"
SELECT DISTINCT ID,Qty,CPU INTO #tmpOrders FROM #tmp

DECLARE @StdTMS INT = (SELECT StdTMS FROM System WITH (NOLOCK))

SELECT
    ID
    ,Seq
    ,FakeID = Seq + 'U1'
    ,ColumnN = RTRIM(ID) + ' (' + ArtworkUnit + ')'
    ,ColumnSeq = '1'
INTO #tmpArtworkType
FROM ArtworkType WITH (NOLOCK)
WHERE ArtworkUnit <> ''
AND Classify IN ({whereClassify})

UNION ALL
SELECT
    ID
    ,Seq
    ,FakeID = Seq + 'U2'
    ,ColumnN = RTRIM(ID) + ' (' + IIF(ProductionUnit = 'QTY', 'Price', p.PUnit) + ')'
    ,ColumnSeq = '2'
FROM ArtworkType WITH (NOLOCK)
OUTER APPLY (SELECT PUnit = IIF({byCPUsqlbit} = 1 AND ProductionUnit = 'TMS', 'CPU', ProductionUnit)) p
WHERE ProductionUnit <> ''
AND Classify IN ({whereClassify})
AND ID <> 'PRINTING PPU'

UNION ALL
SELECT
    ID
    ,Seq
    ,FakeID = Seq + 'N'
    ,ColumnN = RTRIM(ID)
    ,ColumnSeq = '3'
FROM ArtworkType WITH (NOLOCK)
WHERE ArtworkUnit = ''
AND ProductionUnit = ''
AND Classify IN ({whereClassify})

{sql_printingDetail}

UNION ALL
SELECT
    ID = 'EMBROIDERY'
    ,seq = ''
    ,FakeID = '9999ZZ'
    ,ColumnN = 'EMBROIDERY(SubCon)'
    ,ColumnSeq = '996'
UNION ALL
SELECT
    ID = 'EMBROIDERY'
    ,seq = ''
    ,FakeID = '9999ZZ'
    ,ColumnN = 'EMBROIDERY(POSubcon)'
    ,ColumnSeq = '997'
UNION ALL
SELECT
    ID = 'PrintSubCon'
    ,Seq = ''
    ,FakeID = '9999ZZ'
    ,ColumnN = 'POSubCon'
    ,ColumnSeq = '998'
UNION ALL
SELECT
    ID = 'PrintSubCon'
    ,Seq = ''
    ,FakeID = '9999ZZ'
    ,ColumnN = 'SubCon'
    ,ColumnSeq = '999'

SELECT *, rno = (ROW_NUMBER() OVER (ORDER BY a.ID, a.ColumnSeq))
INTO #tmpSubProcess
FROM #tmpArtworkType a

SELECT
    ID = 'TTL' + ID
    ,Seq
    ,FakeID = 'T' + FakeID
    ,ColumnN = 'TTL_' + ColumnN
    ,ColumnSeq
    ,rno = (ROW_NUMBER() OVER (ORDER BY ID, ColumnSeq)) + 1000
INTO #tmpTTL_Subprocess
FROM #tmpSubProcess WITH (NOLOCK)
WHERE ID <> 'PrintSubCon'
AND ColumnN <> 'Printing LT'
AND ColumnN <> 'InkType/color/size'

SELECT
    ID
   ,Seq
   ,FakeID
   ,ColumnN
   ,ColumnSeq
   ,rno = (ROW_NUMBER() OVER (ORDER BY a.rno)) + {model.ColumnsNum}
INTO #tmpArtworkData
FROM (
    SELECT *
    FROM #tmpSubProcess WITH (NOLOCK)

    UNION ALL
    SELECT *
    FROM #tmpTTL_Subprocess

    UNION ALL
    SELECT
        ID = 'TTLTMS'
       ,Seq = ''
       ,FakeID = 'TTLTMS'
       ,FakeID = 'TTL_TMS'
       ,ColumnSeq = ''
       ,rno = '999'
) a

SELECT
    ot.ID
   ,ot.ArtworkTypeID
   ,ot.ArtworkUnit
   ,ProductionUnit = p.PUnit
   ,ot.Qty
   ,ot.TMS
   ,ot.Price
   ,Supp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', l.Abb, ot.LocalSuppID), '')
   ,PoSupp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', PRT.Abb, ot.LocalSuppID), '')
   ,AUnitRno = a0.rno
   ,PUnitRno = IIF(ot.ArtworkTypeID = 'PRINTING PPU', a0.rno, a1.rno)
   ,NRno = a2.rno
   ,TAUnitRno = a3.rno
   ,TPUnitRno = IIF(ot.ArtworkTypeID = 'PRINTING PPU', a3.rno, a4.rno)
   ,TNRno = a5.rno
   ,EMBROIDERYSubcon = IIF(ot.ArtworkTypeID = 'EMBROIDERY', IIF(ot.InhouseOSP = 'O', l.Abb, ot.LocalSuppID), '')
   ,EMBROIDERYPOSubcon = IIF(ot.ArtworkTypeID = 'EMBROIDERY', IIF(ot.InhouseOSP = 'O', EMP.Abb, ot.LocalSuppID), '')
INTO #tmp_LastArtworkType
FROM Order_TmsCost ot WITH (NOLOCK)
LEFT JOIN LocalSupp l WITH (NOLOCK) ON l.ID = ot.LocalSuppID
LEFT JOIN ArtworkType at WITH (NOLOCK) ON at.ID = ot.ArtworkTypeID
LEFT JOIN #tmpArtworkData a0 ON a0.FakeID = ot.Seq + 'U1'
LEFT JOIN #tmpArtworkData a1 ON a1.FakeID = ot.Seq + 'U2'
LEFT JOIN #tmpArtworkData a2 ON a2.FakeID = ot.Seq
LEFT JOIN #tmpArtworkData a3 ON a3.FakeID = 'T' + ot.Seq + 'U1'
LEFT JOIN #tmpArtworkData a4 ON a4.FakeID = 'T' + ot.Seq + 'U2'
LEFT JOIN #tmpArtworkData a5 ON a5.FakeID = 'T' + ot.Seq
OUTER APPLY (SELECT PUnit = IIF({byCPUsqlbit} = 1 AND at.ProductionUnit = 'TMS', 'CPU', at.ProductionUnit)) p
OUTER APPLY (
    SELECT Abb = STUFF((
        SELECT DISTINCT ',' + l.Abb
        FROM ArtworkPO ap WITH (NOLOCK)
        INNER JOIN ArtworkPO_Detail apd WITH (NOLOCK) ON ap.ID = apd.ID
        INNER JOIN LocalSupp l WITH (NOLOCK) ON ap.LocalSuppID = l.ID
        WHERE ap.ArtworkTypeID = 'PRINTING'
        AND apd.OrderID = ot.ID
        FOR XML PATH ('')), 1, 1, '')
) PRT
OUTER APPLY (
    SELECT Abb = STUFF((
        SELECT DISTINCT ',' + l.Abb
        FROM ArtworkPO ap WITH (NOLOCK)
        INNER JOIN ArtworkPO_Detail apd WITH (NOLOCK) ON ap.ID = apd.ID
        INNER JOIN LocalSupp l WITH (NOLOCK) ON ap.LocalSuppID = l.ID
        WHERE ap.ArtworkTypeID = 'EMBROIDERY'
        AND apd.OrderID = ot.ID
        FOR XML PATH ('')), 1, 1, '')
) EMP
WHERE EXISTS (SELECT ID FROM #tmpOrders o WITH (NOLOCK) WHERE ot.ID = o.ID)

{sqloutput}
";

            return sqlcmd;
        }

        /// <summary>
        /// PPIC R03 有勾選 Printing Detail
        /// BI 沒有這段資訊
        /// </summary>
        private string GetPrintingDetail(PPIC_R03_ViewModel model)
        {
            return $@"
SELECT DISTINCT ID INTO #tmpOrders FROM #tmp

SELECT DISTINCT
    oa.ID
   ,InkTypecolorsize = CONCAT(oa.InkType, '/', oa.Colors, ' ', '/', IIF(s.SmallLogo = 1, 'Small logo', 'Big logo'))
   ,PrintingLT = CAST(plt.LeadTime + plt.AddLeadTime AS FLOAT)
INTO #tmpOrder_Artwork
FROM Order_Artwork oa WITH (NOLOCK)
INNER JOIN #tmpOrders o WITH (NOLOCK) ON o.ID = oa.ID
OUTER APPLY (SELECT SmallLogo = IIF(EXISTS(SELECT 1 FROM System WITH (NOLOCK) WHERE SmallLogoCM <= oa.Width OR SmallLogoCM <= oa.Length), 0, 1)) s
OUTER APPLY (SELECT tmpRTL = IIF(o.Cpu = 0, 0, s.SewlineAvgCPU / o.Cpu) FROM System s WITH (NOLOCK)) tr
OUTER APPLY (SELECT RTLQty = IIF(o.Qty < tmpRTL, o.Qty, tmpRTL)) r
OUTER APPLY (SELECT Colors = IIF(oa.Colors = '', 0, oa.Colors)) c
OUTER APPLY (SELECT ex = IIF(EXISTS (SELECT 1 FROM PrintLeadTime WITH (NOLOCK) WHERE InkType = oa.InkType), 1, 0)) e
OUTER APPLY (
    SELECT InkType, LeadTime, AddLeadTime
    FROM PrintLeadTime plt WITH (NOLOCK)
    WHERE plt.InkType = oa.InkType
    AND plt.SmallLogo = s.SmallLogo
    AND r.RTLQty BETWEEN plt.RTLQtyLowerBound AND plt.RTLQtyUpperBound
    AND c.Colors BETWEEN plt.ColorsLowerBound AND plt.ColorsUpperBound
) pEx
OUTER APPLY (
    SELECT InkType, LeadTime, AddLeadTime
    FROM PrintLeadTime plt WITH (NOLOCK)
    WHERE plt.SmallLogo = s.SmallLogo
    AND plt.IsDefault = 1
    AND r.RTLQty BETWEEN plt.RTLQtyLowerBound AND plt.RTLQtyUpperBound
    AND c.Colors BETWEEN plt.ColorsLowerBound AND plt.ColorsUpperBound
) pNEx
OUTER APPLY (
    SELECT
        InkType = IIF(e.ex = 1, pEx.InkType, pnEx.InkType)
       ,LeadTime = IIF(e.ex = 1, pEx.LeadTime, pnEx.LeadTime)
       ,AddLeadTime = IIF(e.ex = 1, pEx.AddLeadTime, pnEx.AddLeadTime)
) plt
WHERE oa.ArtworkTypeID = 'Printing'

SELECT
    t.ID
   ,a.PrintingLT
   ,b.InkTypecolorsize
FROM #tmpOrders t
OUTER APPLY (
    SELECT PrintingLT = STUFF((
        SELECT CONCAT(',', t2.PrintingLT)
        FROM #tmpOrder_Artwork t2
        WHERE t2.ID = t.id
        ORDER BY PrintingLT DESC
        FOR XML PATH ('')), 1, 1, '')
) a
OUTER APPLY (
    SELECT InkTypecolorsize = STUFF((
        SELECT CONCAT(',', t2.InkTypecolorsize)
        FROM #tmpOrder_Artwork t2
        WHERE t2.ID = t.id
        ORDER BY PrintingLT DESC
        FOR XML PATH ('')), 1, 1, '')
) b
";
        }
    }
}
