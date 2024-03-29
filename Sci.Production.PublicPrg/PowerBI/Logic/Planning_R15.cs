using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Web.Services.Description;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Planning_R15
    {
        private StringBuilder artworktypes = new StringBuilder();

        /// <inheritdoc/>
        public Planning_R15()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSummaryBySP(Planning_R15_ViewModel model)
        {
            string sqlCmd = string.Empty;
            string strOrderBy = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StyleID", SqlDbType.VarChar, 15) { Value = (object)model.StyleID ?? DBNull.Value },

                new SqlParameter("@StartSciDelivery", SqlDbType.Date) { Value = (object)model.StartSciDelivery.Value ?? DBNull.Value },
                new SqlParameter("@EndSciDelivery", SqlDbType.Date) { Value = (object)model.EndSciDelivery.Value ?? DBNull.Value },

                new SqlParameter("@StartSewingInline", SqlDbType.Date) { Value = (object)model.StartSewingInline.Value ?? DBNull.Value },
                new SqlParameter("@EndSewingInline", SqlDbType.Date) { Value = (object)model.EndSewingInline.Value ?? DBNull.Value },

                new SqlParameter("@StartBuyerDelivery", SqlDbType.Date) { Value = (object)model.StartBuyerDelivery.Value ?? DBNull.Value },
                new SqlParameter("@EndBuyerDelivery", SqlDbType.Date) { Value = (object)model.EndBuyerDelivery.Value ?? DBNull.Value },

                new SqlParameter("@StartCutOffDate", SqlDbType.Date) { Value = (object)model.StartCutOffDate.Value ?? DBNull.Value },
                new SqlParameter("@EndCutOffDate", SqlDbType.Date) { Value = (object)model.EndCutOffDate.Value ?? DBNull.Value },

                new SqlParameter("@StartCustRQSDate", SqlDbType.Date) { Value = (object)model.StartCustRQSDate.Value ?? DBNull.Value },
                new SqlParameter("@EndCustRQSDate", SqlDbType.Date) { Value = (object)model.EndCustRQSDate.Value ?? DBNull.Value },

                new SqlParameter("@StartPlanDate", SqlDbType.Date) { Value = (object)model.StartPlanDate.Value ?? DBNull.Value },
                new SqlParameter("@EndPlanDate", SqlDbType.Date) { Value = (object)model.EndPlanDate.Value ?? DBNull.Value },

                new SqlParameter("@StartLastSewDate", SqlDbType.Date) { Value = (object)model.StartLastSewDate.Value ?? DBNull.Value },
                new SqlParameter("@EndLastSewDate", SqlDbType.Date) { Value = (object)model.EndLastSewDate.Value ?? DBNull.Value },

                new SqlParameter("@StartSP", SqlDbType.VarChar, 13) { Value = (object)model.StartSP ?? DBNull.Value },
                new SqlParameter("@EndSP", SqlDbType.VarChar, 13) { Value = (object)model.EndSP ?? DBNull.Value },

                new SqlParameter("@BrandID", SqlDbType.VarChar, 8) { Value = (object)model.BrandID ?? DBNull.Value },
                new SqlParameter("@CustCD", SqlDbType.VarChar, 16) { Value = (object)model.CustCD ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 16) { Value = (object)model.MDivisionID ?? DBNull.Value },

                new SqlParameter("@FactoryID", SqlDbType.VarChar, 16) { Value = (object)model.FactoryID ?? DBNull.Value },
            };

            strOrderBy = $@" order by {model.OrderBy}";

            sqlCmd = $@"
            select o.MDivisionID       , o.FactoryID  , o.SciDelivery     , O.CRDDate           , O.CFMDate       , OrderID = O.ID    
                   , O.Dest            , O.StyleID    , O.SeasonID        , O.ProjectID         , O.Customize1    , O.BuyMonth
                   , O.CustPONo        , O.BrandID    , O.CustCDID        , O.ProgramID         , O.CdCodeID      , O.CPU
                   , O.Qty             , O.FOCQty     , O.PoPrice         , O.CMPPrice          , O.KPILETA       , O.LETA
                   , O.MTLETA          , O.SewETA     , O.PackETA         , O.MTLComplete       , O.SewInLine     , O.SewOffLine
                   , O.CutInLine       , O.CutOffLine , O.Category        , O.IsForecast        , O.PulloutDate   , O.ActPulloutDate
                   , O.SMR             , O.MRHandle   , O.MCHandle        , O.OrigBuyerDelivery , O.DoxType       , O.TotalCTN
                   , O.FtyCTN          , O.ClogCTN    , O.VasShas         , O.TissuePaper       , O.MTLExport     , O.SewLine
                   , O.ShipModeList    , O.PlanDate   , O.FirstProduction , O.Finished          , O.FtyGroup      , O.OrderTypeID
                   , O.SpecialMark     , O.GFR        , O.SampleReason    , InspDate = QtyShip_InspectDate.Val     
                   , O.MnorderApv      , O.FtyKPI	  , O.KPIChangeReason , O.StyleUkey		    , O.POID          , OrdersBuyerDelivery = o.BuyerDelivery
                   , InspResult = QtyShip_Result.Val
                   , InspHandle = QtyShip_Handle.Val
                   , O.Junk,CFACTN=isnull(o.CFACTN,0)
                   , InStartDate = Null, InEndDate = Null, OutStartDate = Null, OutEndDate = Null
                   , s.CDCodeNew
                   , sty.ProductType
                   , sty.FabricType
                   , sty.Lining
                   , sty.Gender
                   , sty.Construction
                   , [StyleSpecialMark] = s.SpecialMark
                   , o.IsBuyBack
                   , [Cancelled but Sill] =	case when o.Junk = 1 then 
					                    case when o.NeedProduction = 1 then 'Y'
					                    when o.KeepPanels = 1 then 'K'
					                    else 'N' end
				                    else '' end
                    into #cte 
                    from dbo.Orders o WITH (NOLOCK) 
                    inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
                    left join Style s on s.Ukey = o.StyleUkey
                    left join Pass1 WITH (NOLOCK) on Pass1.ID = O.InspHandle
                    OUTER APPLY(
	                    SELECT [Val]=STUFF((
		                    SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		                    from Order_QtyShip oqs
		                    WHERE ID = o.id
		                    FOR XML PATH('')
	                    ),1,1,'')
                    )QtyShip_InspectDate
                    OUTER APPLY(
	                    SELECT [Val]=STUFF((
		                    SELECT  DISTINCT ','+ CFAFinalInspectResult 
		                    from Order_QtyShip oqs
		                    WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
		                    FOR XML PATH('')
	                    ),1,1,'')
                    )QtyShip_Result
                    OUTER APPLY(
	                    SELECT [Val]=STUFF((
		                    SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
		                    from Order_QtyShip oqs
		                    LEFT JOIN Pass1 p WITH (NOLOCK) ON oqs.CFAFinalInspectHandle = p.ID
		                    WHERE oqs.ID = o.id AND CFAFinalInspectHandle <> '' AND CFAFinalInspectHandle IS NOT NULL
		                    FOR XML PATH('')
	                    ),1,1,'')
                    )QtyShip_Handle
                    Outer apply (
	                    SELECT ProductType = r2.Name
		                    , FabricType = r1.Name
		                    , Lining
		                    , Gender
		                    , Construction = d1.Name
	                    FROM Style s WITH(NOLOCK)
	                    left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	                    left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	                    left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	                    where s.Ukey = o.StyleUkey
                    )sty
                    WHERE 1=1 {this.GetWhere(model)}";

            if (model.IncludeAtworkData)
            {
                sqlCmd += $@"
                --依取得的訂單資料取得訂單的 TMS Cost
                select aa.orderid
                       , bb.ArtworkTypeID
                       , price_tms = iif(cc.IsTMS=1,bb.tms,bb.price)  
                into #rawdata_tmscost
                from #cte aa 
                inner join dbo.Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.orderid
                inner join dbo.ArtworkType cc WITH (NOLOCK)  on cc.id = bb.ArtworkTypeID
                where IsTMS =1 or IsPrice = 1


                --將取得Tms Cost做成樞紐表
                  select * 
                  into #tmscost_pvt
                  from #rawdata_tmscost
                  pivot
                  (
                      sum(price_tms)
                      for artworktypeid in ( {this.artworktypes.ToString().Substring(0, this.artworktypes.ToString().Length - 1)})
                  )as pvt ";
            }

            return null;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSummaryByActicleSize(Planning_R15_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StyleID", SqlDbType.VarChar, 15) { Value = (object)model.StyleID ?? DBNull.Value },

                new SqlParameter("@StartSciDelivery", SqlDbType.Date) { Value = (object)model.StartSciDelivery.Value ?? DBNull.Value },
                new SqlParameter("@EndSciDelivery", SqlDbType.Date) { Value = (object)model.EndSciDelivery.Value ?? DBNull.Value },

                new SqlParameter("@StartSewingInline", SqlDbType.Date) { Value = (object)model.StartSewingInline.Value ?? DBNull.Value },
                new SqlParameter("@EndSewingInline", SqlDbType.Date) { Value = (object)model.EndSewingInline.Value ?? DBNull.Value },

                new SqlParameter("@StartBuyerDelivery", SqlDbType.Date) { Value = (object)model.StartBuyerDelivery.Value ?? DBNull.Value },
                new SqlParameter("@EndBuyerDelivery", SqlDbType.Date) { Value = (object)model.EndBuyerDelivery.Value ?? DBNull.Value },

                new SqlParameter("@StartCutOffDate", SqlDbType.Date) { Value = (object)model.StartCutOffDate.Value ?? DBNull.Value },
                new SqlParameter("@EndCutOffDate", SqlDbType.Date) { Value = (object)model.EndCutOffDate.Value ?? DBNull.Value },

                new SqlParameter("@StartCustRQSDate", SqlDbType.Date) { Value = (object)model.StartCustRQSDate.Value ?? DBNull.Value },
                new SqlParameter("@EndCustRQSDate", SqlDbType.Date) { Value = (object)model.EndCustRQSDate.Value ?? DBNull.Value },

                new SqlParameter("@StartPlanDate", SqlDbType.Date) { Value = (object)model.StartPlanDate.Value ?? DBNull.Value },
                new SqlParameter("@EndPlanDate", SqlDbType.Date) { Value = (object)model.EndPlanDate.Value ?? DBNull.Value },

                new SqlParameter("@StartLastSewDate", SqlDbType.Date) { Value = (object)model.StartLastSewDate.Value ?? DBNull.Value },
                new SqlParameter("@EndLastSewDate", SqlDbType.Date) { Value = (object)model.EndLastSewDate.Value ?? DBNull.Value },

                new SqlParameter("@StartSP", SqlDbType.VarChar, 13) { Value = (object)model.StartSP ?? DBNull.Value },
                new SqlParameter("@EndSP", SqlDbType.VarChar, 13) { Value = (object)model.EndSP ?? DBNull.Value },

                new SqlParameter("@BrandID", SqlDbType.VarChar, 8) { Value = (object)model.BrandID ?? DBNull.Value },
                new SqlParameter("@CustCD", SqlDbType.VarChar, 16) { Value = (object)model.CustCD ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 16) { Value = (object)model.MDivisionID ?? DBNull.Value },

                new SqlParameter("@FactoryID", SqlDbType.VarChar, 16) { Value = (object)model.FactoryID ?? DBNull.Value },
            };
            return null;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSummaryByLineSP(Planning_R15_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StyleID", SqlDbType.VarChar, 15) { Value = (object)model.StyleID ?? DBNull.Value },

                new SqlParameter("@StartSciDelivery", SqlDbType.Date) { Value = (object)model.StartSciDelivery.Value ?? DBNull.Value },
                new SqlParameter("@EndSciDelivery", SqlDbType.Date) { Value = (object)model.EndSciDelivery.Value ?? DBNull.Value },

                new SqlParameter("@StartSewingInline", SqlDbType.Date) { Value = (object)model.StartSewingInline.Value ?? DBNull.Value },
                new SqlParameter("@EndSewingInline", SqlDbType.Date) { Value = (object)model.EndSewingInline.Value ?? DBNull.Value },

                new SqlParameter("@StartBuyerDelivery", SqlDbType.Date) { Value = (object)model.StartBuyerDelivery.Value ?? DBNull.Value },
                new SqlParameter("@EndBuyerDelivery", SqlDbType.Date) { Value = (object)model.EndBuyerDelivery.Value ?? DBNull.Value },

                new SqlParameter("@StartCutOffDate", SqlDbType.Date) { Value = (object)model.StartCutOffDate.Value ?? DBNull.Value },
                new SqlParameter("@EndCutOffDate", SqlDbType.Date) { Value = (object)model.EndCutOffDate.Value ?? DBNull.Value },

                new SqlParameter("@StartCustRQSDate", SqlDbType.Date) { Value = (object)model.StartCustRQSDate.Value ?? DBNull.Value },
                new SqlParameter("@EndCustRQSDate", SqlDbType.Date) { Value = (object)model.EndCustRQSDate.Value ?? DBNull.Value },

                new SqlParameter("@StartPlanDate", SqlDbType.Date) { Value = (object)model.StartPlanDate.Value ?? DBNull.Value },
                new SqlParameter("@EndPlanDate", SqlDbType.Date) { Value = (object)model.EndPlanDate.Value ?? DBNull.Value },

                new SqlParameter("@StartLastSewDate", SqlDbType.Date) { Value = (object)model.StartLastSewDate.Value ?? DBNull.Value },
                new SqlParameter("@EndLastSewDate", SqlDbType.Date) { Value = (object)model.EndLastSewDate.Value ?? DBNull.Value },

                new SqlParameter("@StartSP", SqlDbType.VarChar, 13) { Value = (object)model.StartSP ?? DBNull.Value },
                new SqlParameter("@EndSP", SqlDbType.VarChar, 13) { Value = (object)model.EndSP ?? DBNull.Value },

                new SqlParameter("@BrandID", SqlDbType.VarChar, 8) { Value = (object)model.BrandID ?? DBNull.Value },
                new SqlParameter("@CustCD", SqlDbType.VarChar, 16) { Value = (object)model.CustCD ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 16) { Value = (object)model.MDivisionID ?? DBNull.Value },

                new SqlParameter("@FactoryID", SqlDbType.VarChar, 16) { Value = (object)model.FactoryID ?? DBNull.Value },
            };
            return null;
        }

        /// <summary>
        /// Get SqlCmd
        /// </summary>
        /// <param name="model">查詢資料</param>
        /// <returns>String Where</returns>
        public string GetSqlCmd(string summaryBy)
        {
            string sqlCmd = string.Empty;
            string strHead = summaryBy == "1" ? ", O.Qty , o.IsBuyBack " : ", Oq.Qty, oq.Article, oq.SizeCode, [Buy Back] = IIF(o.IsBuyBack = 1 , 'Y' , '')";
            string strJoin = summaryBy == "1" ? "left join Pass1 WITH (NOLOCK) on Pass1.ID = O.InspHandle" : "left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID";

            sqlCmd =$@"
            select o.MDivisionID       , o.FactoryID  , o.SciDelivery     , O.CRDDate           , O.CFMDate       , OrderID = O.ID    
            , O.Dest            , O.StyleID    , O.SeasonID        , O.ProjectID         , O.Customize1    , O.BuyMonth
            , O.CustPONo        , O.BrandID    , O.CustCDID        , O.ProgramID         , O.CdCodeID      , O.CPU
            , O.FOCQty     , O.PoPrice         , O.CMPPrice          , O.KPILETA       , O.LETA
            , O.MTLETA          , O.SewETA     , O.PackETA         , O.MTLComplete       , O.SewInLine     , O.SewOffLine
            , O.CutInLine       , O.CutOffLine , O.Category        , O.IsForecast        , O.PulloutDate   , O.ActPulloutDate
            , O.SMR             , O.MRHandle   , O.MCHandle        , O.OrigBuyerDelivery , O.DoxType       , O.TotalCTN
            , O.FtyCTN          , O.ClogCTN    , O.VasShas         , O.TissuePaper       , O.MTLExport     , O.SewLine
            , O.ShipModeList    , O.PlanDate   , O.FirstProduction , O.Finished          , O.FtyGroup      , O.OrderTypeID
            , O.SpecialMark     , O.GFR        , O.SampleReason    , InspDate = QtyShip_InspectDate.Val     
            , O.MnorderApv      , O.FtyKPI	   , O.KPIChangeReason , O.StyleUkey		 , O.POID          , OrdersBuyerDelivery = o.BuyerDelivery
            , InspResult = QtyShip_Result.Val
            , InspHandle = QtyShip_Handle.Val
            , O.Junk,CFACTN=isnull(o.CFACTN,0)
            , InStartDate = Null, InEndDate = Null, OutStartDate = Null, OutEndDate = Null
            , s.CDCodeNew
            , sty.ProductType
            , sty.FabricType
            , sty.Lining
            , sty.Gender
            , sty.Construction
            , [StyleSpecialMark] = s.SpecialMark
            , [Cancelled but Sill] =	case when o.Junk = 1 then 
            {strHead}
            case when o.NeedProduction = 1 then 'Y'
            when o.KeepPanels = 1 then 'K'
            else 'N' end
            else '' end
            into #cte 
            from dbo.Orders o WITH (NOLOCK) 
            inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
            left join Style s on s.Ukey = o.StyleUkey
            {strJoin}
            OUTER APPLY(
            SELECT [Val]=STUFF((
            SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
            from Order_QtyShip oqs
            WHERE ID = o.id
            FOR XML PATH('')
            ),1,1,'')
            )QtyShip_InspectDate
            OUTER APPLY(
            SELECT [Val]=STUFF((
            SELECT  DISTINCT ','+ CFAFinalInspectResult 
            from Order_QtyShip oqs
            WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
            FOR XML PATH('')
            ),1,1,'')
            )QtyShip_Result
            OUTER APPLY(
            SELECT [Val]=STUFF((
            SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
            from Order_QtyShip oqs
            LEFT JOIN Pass1 p WITH (NOLOCK) ON oqs.CFAFinalInspectHandle = p.ID
            WHERE oqs.ID = o.id AND CFAFinalInspectHandle <> '' AND CFAFinalInspectHandle IS NOT NULL
            FOR XML PATH('')
            ),1,1,'')
            )QtyShip_Handle
            Outer apply (
            SELECT ProductType = r2.Name
            , FabricType = r1.Name
            , Lining
            , Gender
            , Construction = d1.Name
            FROM Style s WITH(NOLOCK)
            left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
            left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
            left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
            where s.Ukey = o.StyleUkey
            )sty
            WHERE 1=1"

            return sqlCmd;
        }


        /// <summary>
        /// Get Where
        /// </summary>
        /// <param name="model">篩選條件</param>
        /// <returns>String Where</returns>
        public string GetWhere(Planning_R15_ViewModel model)
        {
            string strWhere = string.Empty;

            if (!model.IncludeCancelOrder)
            {
                strWhere += " and o.Junk = 0 ";
            }

            if (!model.StyleID.Empty())
            {
                strWhere += model.SummaryBy == "1" ? $@" and o.StyleID = @StyleID " : string.Empty;
            }

            if (!model.StartSciDelivery.Value.Empty() && !model.EndSciDelivery.Value.Empty())
            {
                strWhere += "and o.SciDelivery between @StartSciDelivery and @EndSciDelivery ";
            }
            else if (!model.StartSciDelivery.Value.Empty())
            {
                strWhere += $" and o.SciDelivery >= @StartSciDelivery ";
            }
            else if (!model.EndSciDelivery.Value.Empty())
            {
                strWhere += $" and o.SciDelivery <= @EndSciDelivery ";
            }

            if (!model.StartSewingInline.Value.Empty())
            {
                strWhere += $" and (o.SewInLine >= @StartSewingInline OR @StartSewingInline BETWEEN o.SewInLine AND o.SewOffLine ) ";
            }

            if (!model.EndSewingInline.Value.Empty())
            {
                strWhere += $" and (o.SewInLine <= @EndSewingInline OR @EndSewingInline BETWEEN o.SewInLine AND o.SewOffLine ) ";
            }

            if (!model.StartBuyerDelivery.Value.Empty() && !model.EndBuyerDelivery.Value.Empty())
            {
                strWhere += $@"
                and exists
                (
                    select 1 
                    from Order_QtyShip s WITH (NOLOCK) 
                    where s.id = o.ID
                    and s.BuyerDelivery between @StartBuyerDelivery and @EndBuyerDelivery
                )";
            }
            else if (!model.StartBuyerDelivery.Value.Empty())
            {
                strWhere += $@"
                and exists
                (
                    select 1 
                    from Order_QtyShip s WITH (NOLOCK) 
                    where s.id = o.ID
                    and s.BuyerDelivery >= @StartBuyerDelivery
                )";
            }
            else if (!model.EndBuyerDelivery.Value.Empty())
            {
                strWhere += $@"
                and exists
                (
                    select 1 
                    from Order_QtyShip s WITH (NOLOCK) 
                    where s.id = o.ID
                    and s.BuyerDelivery <= @EndBuyerDelivery
                )";
            }

            if (!model.StartCustRQSDate.Value.Empty())
            {
                strWhere += " and o.CRDDate >= @StartCustRQSDate ";
            }

            if (!model.EndCustRQSDate.Value.Empty())
            {
                strWhere += " and o.CRDDate <= @EndCustRQSDate ";
            }

            if (!model.StartCutOffDate.Value.Empty())
            {
                strWhere += " and o.SDPDate >= @StartCutOffDate ";
            }

            if (!model.EndCutOffDate.Value.Empty())
            {
                strWhere += " and o.SDPDate <= @EndCutOffDate ";
            }

            if (!model.StartPlanDate.Value.Empty())
            {
                strWhere += " and o.PlanDate >= @StartPlanDate ";
            }

            if (!model.EndPlanDate.Value.Empty())
            {
                strWhere += " and o.PlanDate <= @EndPlanDate ";
            }

            if (!model.StartSP.Empty())
            {
                strWhere += " and o.id >= @StartSP ";
            }

            if (!model.EndSP.Empty())
            {
                strWhere += " and o.id <= @EndSP ";
            }

            if (!model.StartLastSewDate.Value.Empty() && !model.EndLastSewDate.Value.Empty())
            {
                strWhere += $@" and exists(select 1 from View_SewingInfoSP vsis with (nolock) where vsis.OrderID = o.ID and vsis.LastSewDate between @LastSewDateFrom and @LastSewDateTo) ";
            }

            if (!model.BrandID.Empty())
            {
                strWhere += @" and o.brandid = @BrandID ";
            }

            if (!model.CustCD.Empty())
            {
                strWhere += @" and o.CustCDID = @CustCD ";
            }

            if (!model.MDivisionID.Empty())
            {
                strWhere += @" and o.mdivisionid = @MDivisionID ";
            }

            if (!model.FactoryID.Empty())
            {
                strWhere += @" and o.FtyGroup = @FactoryID ";
            }

            if (model.OnlyShowCheckedSubprocessOrder)
            {
                strWhere += $@"
                and exists(
	                select * from Pattern_GL PGL WITH (NOLOCK) 
	                inner join dbo.GetPatternUkey(o.ID,'','',s.Ukey,'') t on PGL.PatternUKEY = t.PatternUkey
	                inner join dbo.SplitString('{model.SubprocessID}',',') b on PGL.Annotation like '%'+ b.Data+'%'
                )";
            }

            if (!model.Category.Empty())
            {
                strWhere += $@" and o.Category in ({model.Category})";
            }

            strWhere += " and exists (select 1 from Factory where o.FactoryId = id and IsProduceFty = 1)";

            return strWhere;
        }
    }
}
