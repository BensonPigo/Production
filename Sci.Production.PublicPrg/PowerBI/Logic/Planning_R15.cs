using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Planning_R15
    {
        private int subprocessInoutColumnCount;
        private List<string> notExistsBundle_Detail_Art = new List<string>() { "SORTING", "LOADING", "SEWINGLINE" };

        /// <inheritdoc/>
        public Planning_R15()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <summary>
        /// Get SqlCmd
        /// </summary>
        /// <param name="model">查詢資料</param>
        /// <param name="dtArtworkType">ArtworkType DataTable</param>
        /// <returns>String Where</returns>
        public Base_ViewModel GetPlanning_R15(Planning_R15_ViewModel model, DataTable dtArtworkType)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StartSciDelivery", SqlDbType.Date) { Value = model.StartSciDelivery.HasValue ? (object)model.StartSciDelivery.Value : DBNull.Value },
                new SqlParameter("@EndSciDelivery", SqlDbType.Date) { Value = model.EndSciDelivery.HasValue ? (object)model.EndSciDelivery.Value : DBNull.Value },

                new SqlParameter("@StartBuyerDelivery", SqlDbType.Date) { Value = model.StartBuyerDelivery.HasValue ? (object)model.StartBuyerDelivery.Value : DBNull.Value },
                new SqlParameter("@EndBuyerDelivery", SqlDbType.Date) { Value = model.EndBuyerDelivery.HasValue ? (object)model.EndBuyerDelivery.Value : DBNull.Value },

                new SqlParameter("@StartSewingInline", SqlDbType.Date) { Value = model.StartSewingInline.HasValue ? (object)model.StartSewingInline.Value : DBNull.Value },
                new SqlParameter("@EndSewingInline", SqlDbType.Date) { Value = model.EndSewingInline.HasValue ? (object)model.EndSewingInline.Value : DBNull.Value },

                new SqlParameter("@StartCutOffDate", SqlDbType.Date) { Value = model.StartCutOffDate.HasValue ? (object)model.StartCutOffDate.Value : DBNull.Value },
                new SqlParameter("@EndCutOffDate", SqlDbType.Date) { Value = model.EndCutOffDate.HasValue ? (object)model.EndCutOffDate.Value : DBNull.Value },

                new SqlParameter("@StartCustRQSDate", SqlDbType.Date) { Value = model.StartCustRQSDate.HasValue ? (object)model.StartCustRQSDate.Value : DBNull.Value },
                new SqlParameter("@EndCustRQSDate", SqlDbType.Date) { Value = model.EndCustRQSDate.HasValue ? (object)model.EndCustRQSDate.Value : DBNull.Value },

                new SqlParameter("@StartPlanDate", SqlDbType.Date) { Value = model.StartPlanDate.HasValue ? (object)model.StartPlanDate.Value : DBNull.Value },
                new SqlParameter("@EndPlanDate", SqlDbType.Date) { Value = model.EndPlanDate.HasValue ? (object)model.EndPlanDate.Value : DBNull.Value },

                new SqlParameter("@StartSP", SqlDbType.VarChar, 13) { Value = (object)model.StartSP ?? DBNull.Value },
                new SqlParameter("@EndSP", SqlDbType.VarChar, 13) { Value = (object)model.EndSP ?? DBNull.Value },

                new SqlParameter("@StartLastSewDate", SqlDbType.Date) { Value = model.StartLastSewDate.HasValue ? (object)model.StartLastSewDate.Value : DBNull.Value },
                new SqlParameter("@EndLastSewDate", SqlDbType.Date) { Value = model.EndLastSewDate.HasValue ? (object)model.EndLastSewDate.Value : DBNull.Value },

                new SqlParameter("@StyleID", SqlDbType.VarChar, 15) { Value = (object)model.StyleID ?? DBNull.Value },

                new SqlParameter("@BrandID", SqlDbType.VarChar, 8) { Value = (object)model.BrandID ?? DBNull.Value },
                new SqlParameter("@CustCD", SqlDbType.VarChar, 16) { Value = (object)model.CustCD ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 16) { Value = (object)model.MDivisionID ?? DBNull.Value },

                new SqlParameter("@FactoryID", SqlDbType.VarChar, 16) { Value = (object)model.FactoryID ?? DBNull.Value },
            };

            var itemWhere = this.GetWhere(model);
            string sqlCmd = string.Empty;
            string strHead = model.SummaryBy == "1" ? ", O.Qty " : ", Oq.Qty, oq.Article, oq.SizeCode";
            string strJoin = model.SummaryBy == "1" ? "left join Pass1 WITH (NOLOCK) on Pass1.ID = O.InspHandle" : "left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID";
            bool isR15 = model.FormParameter == "1";

            // 先取得加工段對照表，後續做動態推算
            DualResult dualResult = DBProxy.Current.Select(null, this.SqlTMP_SubProcess(false, isR15, model.SubprocessID), out DataTable dtTMP_SubProcess);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return null;
            }

            #region 組SQL
            sqlCmd = this.SqlTMP_SubProcess(true, isR15, model.SubprocessID); // 產生加工段暫存表
            sqlCmd += $@"            
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
            , [Buy Back] = IIF(o.IsBuyBack = 1 , 'Y' , '')
            {strHead}
            , [Cancelled but Sill] = case when o.Junk = 1 then 
                                     case when o.NeedProduction = 1 then 'Y'
                                     when o.KeepPanels = 1 then 'K'
                                     else 'N' end
                                     else '' end
            into #cte 
            from dbo.Orders o WITH (NOLOCK)
            inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
            left join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
            {strJoin}
            OUTER APPLY(
                SELECT [Val]=STUFF((
                SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
                from Order_QtyShip oqs WITH (NOLOCK)
                WHERE ID = o.id
                FOR XML PATH('')
                ),1,1,'')
            )QtyShip_InspectDate
            OUTER APPLY(
                SELECT [Val]=STUFF((
                SELECT  DISTINCT ','+ CFAFinalInspectResult 
                from Order_QtyShip oqs WITH (NOLOCK)
                WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
                FOR XML PATH('')
                ),1,1,'')
            )QtyShip_Result
            OUTER APPLY(
                SELECT [Val]=STUFF((
                SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
                from Order_QtyShip oqs WITH (NOLOCK)
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
            WHERE 1=1 {itemWhere.Item1}
            ";

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
                    for artworktypeid in ( {model.ArtworkTypes})
                )as pvt 
                ";
            }

            string subProcessStatusWithBI = model.IsBI ? "and #AUT.InQtyBySet is null and #AUT.OutQtyBySet is null and #FM.InQtyBySet is null and #FM.OutQtyBySet is null " : string.Empty;
            string subProcessStatusOtherWithBI = model.IsBI ? "and AUT_i.v = 1 and AUT_o.v = 1 and FM_i.v = 1 and FM_o.v = 1 " : string.Empty;

            string subprocessQtyColumns = $@"
            , [RFID Cut Qty] = #SORTING.OutQtyBySet
            , [RFID SewingLine In Qty] = #SewingLine.InQtyBySet
            , [RFID Loading Qty] = #loading.InQtyBySet
            , [RFID Emb Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBROIDERY')),ISNULL( #Emb.InQtyBySet ,0) ,#Emb.InQtyBySet)
            , [RFID Emb Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBROIDERY')),ISNULL( #Emb.OutQtyBySet ,0) ,#Emb.OutQtyBySet)
            , [RFID Bond Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('BO')),ISNULL( #BO.InQtyBySet ,0) ,#BO.InQtyBySet)	
            , [RFID Bond Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('BO')),ISNULL( #BO.OutQtyBySet ,0) ,#BO.OutQtyBySet)
            , [RFID Print Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PRINTING')),ISNULL( #prt.InQtyBySet,0) ,#prt.InQtyBySet)
            , [RFID Print Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PRINTING')),ISNULL( #prt.OutQtyBySet,0) ,#prt.OutQtyBySet)
            , [RFID AT Farm In Qty] =IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AT')),ISNULL(#AT.InQtyBySet,0) ,#AT.InQtyBySet)
            , [RFID AT Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AT')),ISNULL(#AT.OutQtyBySet,0) ,#AT.OutQtyBySet)
            , [RFID Pad Print Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PAD PRINTING')),ISNULL(#PADPRT.InQtyBySet ,0) ,#PADPRT.InQtyBySet)
            , [RFID Pad Print Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PAD PRINTING')),ISNULL( #PADPRT.OutQtyBySet,0) ,#PADPRT.OutQtyBySet)
            , [RFID Emboss Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBOSS/DEBOSS')),ISNULL( #SUBCONEMB.InQtyBySet ,0) ,#SUBCONEMB.InQtyBySet)
            , [RFID Emboss Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBOSS/DEBOSS')),ISNULL( #SUBCONEMB.OutQtyBySet ,0) ,#SUBCONEMB.OutQtyBySet)
            , [RFID HT Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('HEAT TRANSFER')),ISNULL( #HT.InQtyBySet ,0) ,#HT.InQtyBySet)
            , [RFID HT Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('HEAT TRANSFER')),ISNULL( #HT.OutQtyBySet ,0) ,#HT.OutQtyBySet)
            , [RFID AUT Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AUT')),ISNULL( #AUT.InQtyBySet ,0) ,#AUT.InQtyBySet)
            , [RFID AUT Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AUT')),ISNULL( #AUT.OutQtyBySet ,0) ,#AUT.OutQtyBySet)
            , [RFID FM Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('FM')),ISNULL( #FM.InQtyBySet ,0) ,#FM.InQtyBySet)
            , [RFID FM Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('FM')),ISNULL( #FM.OutQtyBySet ,0) ,#FM.OutQtyBySet)
            , SubProcessStatus= case when t.Junk = 1 then null 
                                     when #SORTING.OutQtyBySet is null and #loading.InQtyBySet is null 
                                     and #SewingLine.InQtyBySet is null
                                     and #Emb.InQtyBySet is null and #Emb.OutQtyBySet is null
                                     and #BO.InQtyBySet is null and #BO.OutQtyBySet  is null 
                                     and #prt.InQtyBySet  is null and #prt.OutQtyBySet  is null 
                                     and #AT.InQtyBySet  is null and #AT.OutQtyBySet  is null 
                                     and #PADPRT.InQtyBySet is null and #PADPRT.OutQtyBySet is null
                                     and #SUBCONEMB.InQtyBySet is null and #SUBCONEMB.OutQtyBySet is null
                                     and #HT.InQtyBySet is null and #HT.OutQtyBySet is null
                                     {subProcessStatusWithBI}
                                     then null
		                             when SORTINGStatus.v = 1 and loadingStatus.v = 1 --判斷有做加工段的數量=訂單qty,則為1,全部為1才為Y
		                             and SewingLineStatus.v = 1
		                             and Emb_i.v = 1 and Emb_o.v = 1
		                             and BO_i.v = 1 and BO_o.v = 1
		                             and prt_i.v = 1 and prt_o.v = 1
		                             and AT_i.v = 1 and AT_o.v = 1
		                             and PADPRT_i.v = 1 and PADPRT_o.v = 1
		                             and SUBCONEMB_i.v = 1 and SUBCONEMB_o.v = 1
		                             and HT_i.v = 1 and HT_o.v = 1
                                     {subProcessStatusOtherWithBI}
		                             then 'Y' end

";

            string subprocessQtyColumnsSource = string.Empty;
            foreach (DataRow dr in dtTMP_SubProcess.Rows)
            {
                string subprocessIDtmp = Prgs.SubprocesstmpNoSymbol(dr["ID"].ToString());
                if (model.SummaryBy == "1")
                {
                    subprocessQtyColumnsSource += $@"left join #{subprocessIDtmp} on #{subprocessIDtmp}.OrderID = t.OrderID" + Environment.NewLine;
                }
                else
                {
                    subprocessQtyColumnsSource += $@"left join #{subprocessIDtmp} on #{subprocessIDtmp}.OrderID = t.OrderID and #{subprocessIDtmp}.Article = t.Article and #{subprocessIDtmp}.Sizecode = t.SizeCode" + Environment.NewLine;
                }

                if (subprocessIDtmp.ToUpper() != "SORTING" || subprocessIDtmp.ToUpper() != "SEWINGLINE" || subprocessIDtmp.ToUpper() != "LOADING")
                {
                    subprocessQtyColumnsSource += $@"outer apply(select v = case when #{subprocessIDtmp}.InQtyBySet is null or #{subprocessIDtmp}.InQtyBySet >= t.Qty then 1 else 0 end){subprocessIDtmp}_i" + Environment.NewLine;
                    subprocessQtyColumnsSource += $@"outer apply(select v = case when #{subprocessIDtmp}.OutQtyBySet is null or #{subprocessIDtmp}.OutQtyBySet >= t.Qty then 1 else 0 end){subprocessIDtmp}_o" + Environment.NewLine;
                }
            }

            subprocessQtyColumnsSource += $@"
            outer apply(select v = case when #SORTING.OutQtyBySet is null or #SORTING.OutQtyBySet >= t.Qty then 1 else 0 end)SORTINGStatus--null即不用判斷此加工段 標記1, 數量=訂單數 標記1
            outer apply(select v = case when #SewingLine.InQtyBySet is null or #SewingLine.InQtyBySet >= t.Qty then 1 else 0 end)SewingLineStatus
            outer apply(select v = case when #loading.InQtyBySet is null or #loading.InQtyBySet >= t.Qty then 1 else 0 end)loadingStatus";

            List<string> subprocessIDs = dtTMP_SubProcess.AsEnumerable().Select(x => x.Field<string>("ID")).ToList();
            List<string> subprocessArtworkTypeIds = dtTMP_SubProcess.AsEnumerable().Select(x => x.Field<string>("ArtworkTypeId")).ToList();

            // R15-1的 subprocessQtyColumnsSource 需要另外處理。
            if (!isR15)
            {
                int iType = model.SummaryBy == "1" ? 0 : 1;
                subprocessIDs = model.SubprocessID.Split(',').ToList();
                subprocessQtyColumns = subprocessIDs.Count > 1 ? this.MultiSuboricessColumns(iType, model.SubprocessID, MyUtility.Convert.GetInt(model.SummaryBy))
                                                                : this.SingleSubprocessColumn(iType, model.SubprocessID,  MyUtility.Convert.GetInt(model.SummaryBy));
                subprocessQtyColumnsSource = string.Empty;
                foreach (var item in subprocessIDs)
                {
                    string subprocessIDtmp = Prgs.SubprocesstmpNoSymbol(item);
                    if (model.SummaryBy == "1")
                    {
                        subprocessQtyColumnsSource += $@"left join #{subprocessIDtmp} on #{subprocessIDtmp}.OrderID = t.OrderID" + Environment.NewLine;
                    }
                    else
                    {
                        subprocessQtyColumnsSource += $@"left join #{subprocessIDtmp} on #{subprocessIDtmp}.OrderID = t.OrderID and #{subprocessIDtmp}.Article = t.Article and #{subprocessIDtmp}.SizeCode = t.SizeCode " + Environment.NewLine;
                    }
                }
            }

            bool isSP = model.SummaryBy == "1" ? true : false;

            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs.ToArray(), "#cte", bySP: isSP, isNeedCombinBundleGroup: true, isMorethenOrderQty: "0", rfidProcessLocationID: model.RFIDProcessLocation);
            string dropTmp = "drop table ";
            if (string.IsNullOrEmpty(model.RFIDProcessLocation))
            {
                qtyBySetPerSubprocess = $@"
                select s.OrderID
                    , s.SubProcessID
                    , TransferTime = MAX(s.TransferTime)
                into #tmp_SetQtyBySubprocess_Last
                from SetQtyBySubprocess s WITH (NOLOCK)
                where exists (select 1 from #cte t where s.OrderID = t.OrderID)
                group by s.OrderID, s.SubProcessID
                ";

                // R15需看到 By ArtworkTypeID，R15-1需看到 By SubProcessID
                string sqlWhere = isR15 ? $@"and ArtworkTypeId in ('{string.Join("','", subprocessArtworkTypeIds)}')" :
                                          $@"and SubProcessID in ('{string.Join("','", subprocessIDs)}')";
                if (model.SummaryBy == "1")
                {
                    qtyBySetPerSubprocess += $@"
                    select s.OrderID
                        {(isR15 ? ",s.ArtworkTypeId" : ", s.SubprocessID")}
                        , InQtyBySet = SUM(s.InQtyBySet)
	                    , OutQtyBySet = SUM(s.OutQtyBySet)
	                    , FinishedQtyBySet = SUM(s.FinishedQtyBySet)
                    into #tmp_SetQtyBySubprocess
                    from 
                    (
                        select s.OrderID
                            , s.Article
                            , s.SizeCode 
                            {(isR15 ? ",t.ArtworkTypeId" : ", s.SubprocessID")}
	                        , InQtyBySet = MIN(s.InQtyBySet)
	                        , OutQtyBySet = MIN(s.OutQtyBySet)
	                        , FinishedQtyBySet = MIN(s.FinishedQtyBySet)
                        from SetQtyBySubprocess s WITH (NOLOCK)
                        inner join #tmp_SubProcess_ArtworkTypeID_Map t on t.ID = s.SubprocessID
                        where exists (select 1 from #tmp_SetQtyBySubprocess_Last t where t.OrderID = s.OrderID and t.SubProcessID = s.SubProcessID and t.TransferTime = s.TransferTime)
                        {sqlWhere}
                        group by s.OrderID, s.Article, s.SizeCode{(isR15 ? ",t.ArtworkTypeId" : ", s.SubprocessID")}
                    )s
                    group by s.OrderID{(isR15 ? ",s.ArtworkTypeId" : ", s.SubprocessID")}" + Environment.NewLine;
                }
                else
                {
                    qtyBySetPerSubprocess += $@"
                    select s.OrderID
                        , s.Article
                        , s.SizeCode 
                        {(isR15 ? ",t.ArtworkTypeId" : ", s.SubprocessID")}
                        , InQtyBySet = MIN(s.InQtyBySet)
                        , OutQtyBySet = MIN(s.OutQtyBySet)
                        , FinishedQtyBySet = MIN(s.FinishedQtyBySet)
                    into #tmp_SetQtyBySubprocess
                    from SetQtyBySubprocess s WITH (NOLOCK)
                    inner join #tmp_SubProcess_ArtworkTypeID_Map t on t.ID = s.SubprocessID
                    where exists (select 1 from #tmp_SetQtyBySubprocess_Last t where t.OrderID = s.OrderID and t.SubProcessID = s.SubProcessID and t.TransferTime = s.TransferTime)
                    {sqlWhere}
                    group by s.OrderID, s.Article, s.SizeCode{(isR15 ? ",t.ArtworkTypeId" : ", s.SubprocessID")}" + Environment.NewLine;
                }

                if (isR15)
                {
                    foreach (DataRow item in dtTMP_SubProcess.Rows)
                    {
                        string subprocessIDtmp = Prgs.SubprocesstmpNoSymbol(item["ID"].ToString());
                        dropTmp += $"#{subprocessIDtmp},";
                        qtyBySetPerSubprocess += $@"select * into #{subprocessIDtmp} from #tmp_SetQtyBySubprocess where ArtworkTypeId = '{item["ArtworkTypeId"].ToString()}'" + Environment.NewLine;
                    }
                }
                else
                {
                    foreach (var item in subprocessIDs)
                    {
                        string subprocessIDtmp = Prgs.SubprocesstmpNoSymbol(item);
                        dropTmp += $"#{subprocessIDtmp},";
                        qtyBySetPerSubprocess += $@"select * into #{subprocessIDtmp} from #tmp_SetQtyBySubprocess where SubprocessID = '{item}'" + Environment.NewLine;
                    }
                }
            }

            if (model.SummaryBy == "1")
            {
                sqlCmd += $@"
                -- 依撈出來的order資料(cte)去找各製程的WIP
                SELECT X.OrderId
	                   , firstSewingDate = min(X.OutputDate) 
                       , lastestSewingDate = max(X.OutputDate) 
                       , QAQTY = sum(X.QAQty) 
                       , AVG_QAQTY = AVG(X.QAQTY)
                into #tmp_SEWOUTPUT
                from (
                    SELECT b.OrderId, a.OutputDate
                           , QAQty = sum(a.QAQty) 
                    FROM DBO.SewingOutput a WITH (NOLOCK) 
                    inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
	                inner join (select distinct OrderID from #cte) t on b.OrderId = t.OrderID 
                    group by b.OrderId,a.OutputDate 
                ) X
                group by X.OrderId

                SELECT c.OrderID, MIN(a.cDate) first_cut_date
                into #tmp_first_cut_date
                from dbo.CuttingOutput a WITH (NOLOCK) 
                inner join dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
                inner join dbo.WorkOrderForOutput_Distribute c WITH (NOLOCK) on c.WorkOrderForOutputUkey = b.WorkOrderForOutputUkey
                inner join (select distinct OrderID from #cte) t on c.OrderID = t.OrderID
                group by c.OrderID

                select pd.OrderId, pd.ScanQty
                into #tmp_PackingList_Detail
                from PackingList_Detail pd WITH (NOLOCK)
                inner join #cte t on pd.OrderID = t.OrderID

                select t.OrderID
                , cut_qty = (SELECT SUM(CWIP.Qty) 
                            FROM DBO.CuttingOutput_WIP CWIP WITH (NOLOCK) 
                            WHERE CWIP.OrderID = T.OrderID)
	            ,f.first_cut_date
                , sewing_output = (select MIN(isnull(tt.qaqty,0)) 
                                    from dbo.style_location sl WITH (NOLOCK) 
                                    left join (
                                        SELECT b.ComboType
                                                , qaqty = sum(b.QAQty)  
                                        FROM DBO.SewingOutput a WITH (NOLOCK) 
                                        inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
                                        where b.OrderId = t.OrderID
                                        group by ComboType 
                                    ) tt on tt.ComboType = sl.Location
                                    where sl.StyleUkey = t.StyleUkey) 
                , t.StyleUkey
                , EMBROIDERY_qty = (select qty = min(qty)  
                                    from (
                                        select qty = sum(b.Qty) 
                                                , c.PatternCode
                                                , c.ArtworkID 
                                        from dbo.farmin a WITH (NOLOCK) 
                                        inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                        right join (
                                            select distinct v.ArtworkTypeID
                                                    , v.Article
                                                    , v.ArtworkID
                                                    , v.PatternCode 
                                            from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                            where v.ID=t.OrderID
                                        ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                                and c.PatternCode = b.PatternCode 
                                                and c.ArtworkID = b.ArtworkID
                                        where a.ArtworkTypeId='EMBROIDERY' 
                                                and b.Orderid = t.OrderID
                                        group by c.PatternCode,c.ArtworkID
                                    ) x) 
                , BONDING_qty = (select qty = min(qty)  
                                from (
                                    select qty = sum(b.Qty)  
                                            , c.PatternCode
                                            , c.ArtworkID 
                                    from dbo.farmin a WITH (NOLOCK) 
                                    inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                    right join (
                                        select distinct v.ArtworkTypeID
                                                , v.ArtworkID
                                                , v.PatternCode 
                                        from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                        where v.ID = t.OrderID
                                    ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                            and c.PatternCode = b.PatternCode 
                                            and c.ArtworkID = b.ArtworkID
                                    where a.ArtworkTypeId='BONDING' 
                                            and b.Orderid = t.OrderID
                                    group by c.PatternCode, c.ArtworkID
                                ) x) 
                , PRINTING_qty = (select qty = min(qty) 
                                    from (
                                    select qty = sum(b.Qty) 
                                            , c.PatternCode
                                            , c.ArtworkID 
                                    from dbo.farmin a WITH (NOLOCK) 
                                    inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                    right join (
                                        select distinct v.ArtworkTypeID
                                                , v.ArtworkID
                                                , v.PatternCode 
                                        from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                        where v.ID=t.OrderID
                                    ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                            and c.PatternCode = b.PatternCode 
                                            and c.ArtworkID = b.ArtworkID
                                    where a.ArtworkTypeId = 'PRINTING' 
                                            and b.Orderid = t.OrderID
                                    group by c.PatternCode, c.ArtworkID
                                    ) x) 
                , s.firstSewingDate
	            , s.lastestSewingDate
	            , s.QAQTY
	            , s.AVG_QAQTY
                into #cte2 
                from #cte t
                left join #tmp_first_cut_date f on t.OrderID = f.OrderID
                left join #tmp_SEWOUTPUT s on t.OrderID = s.OrderID 

                drop table #tmp_first_cut_date,#tmp_SEWOUTPUT

                select sod.OrderID ,Max(so.OutputDate) LastSewnDate
                into #imp_LastSewnDate
                from SewingOutput so WITH (NOLOCK) 
                inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                inner join #cte t on sod.OrderID = t.OrderID 
                group by sod.OrderID 

                {qtyBySetPerSubprocess}

                select 
                t.MDivisionID
                , t.FactoryID
                , t.SewLine
                , t.OrdersBuyerDelivery
                , t.SciDelivery
                , t.SewInLine
                , t.SewOffLine
                , IDD.val
                , t.BrandID
                , t.OrderID
                , t.POID
	            , t.[Buy Back]
	            , [Cancelled] = IIF(t.Junk=1 ,'Y' ,'')
	            , T.[Cancelled but Sill]
                , Dest = Country.Alias
                , t.StyleID
                , t.OrderTypeID
                , t.ShipModeList
	            , [PartialShipping]=IIF( (SELECT COUNT(ID) FROM Order_QtyShip WITH (NOLOCK) WHERE ID = t.OrderID) >=2 ,'Y' ,'')
                , [OrderNo] = t.Customize1
                , t.CustPONo
                , t.CustCDID
                , t.ProgramID
                , t.CdCodeID
	            , t.CDCodeNew
	            , t.ProductType
	            , t.FabricType
	            , t.Lining
	            , t.Gender
	            , t.Construction
                , t.KPILETA
                , t.LETA
                , t.MTLETA
                , t.SewETA
                , t.PackETA
                , t.CPU
	            , [TTL CPU] = t.CPU * t.Qty
	            , [CPU Closed]= t.CPU * #cte2.sewing_output
	            , [CPU bal]= t.CPU * (t.qty + t.FOCQty - #cte2.sewing_output )
                , article_list = (select article + ',' 
                                    from (
                                        select distinct q.Article  
                                        from dbo.Order_Qty q WITH (NOLOCK) 
                                        where q.ID = t.OrderID
                                    ) t 
                                    for xml path('')) 
                , t.Qty
                ,StandardOutput.StandardOutput
	            ,oriArtwork = ann.Artwork
	            ,AddedArtwork = EXa.Artwork
                ,Artwork.Artwork
                ,spdX.SubProcessDest
                ,EstCutDate.EstimatedCutDate
                , #cte2.first_cut_date
                , #cte2.cut_qty

                {subprocessQtyColumns}

                , #cte2.EMBROIDERY_qty
                , #cte2.BONDING_qty
                , #cte2.PRINTING_qty
                , #cte2.sewing_output
                , [Balance] = t.qty + t.FOCQty - #cte2.sewing_output 
                , #cte2.firstSewingDate
	            , [Last Sewn Date] = l.LastSewnDate
                , #cte2.AVG_QAQTY
                , [Est_offline] = DATEADD(DAY
                                        , iif(isnull(#cte2.AVG_QAQTY, 0) = 0, 0
                                        , ceiling((t.qty+t.FOCQty - #cte2.sewing_output) / (#cte2.AVG_QAQTY*1.0)))
                                        , #cte2.firstSewingDate) 
                , [Scanned_Qty] = PackDetail.ScanQty
                , [pack_rate] = IIF(isnull(t.TotalCTN, 0) = 0, 0, round(t.ClogCTN / (t.TotalCTN * 1.0), 4) * 100 ) 
                , t.TotalCTN
                , FtyCtn = t.TotalCTN - t.FtyCTN
                , t.ClogCTN
                , t.CFACTN
                , t.InspDate
                , InspResult
                , [CFA Name] = InspHandle
                , t.ActPulloutDate
                , t.FtyKPI                
                , KPIChangeReason = KPIChangeReason.KPIChangeReason  
                , t.PlanDate
                , dbo.getTPEPass1(t.SMR) [SMR]
                , dbo.getTPEPass1(T.MRHandle) [Handle]
                , [PO SMR] = (select dbo.getTPEPass1(p.POSMR) 
                                from dbo.PO p WITH (NOLOCK) 
                                where p.ID = t.POID) 
                , [PO Handle] = (select dbo.getTPEPass1(p.POHandle) 
                                from dbo.PO p WITH (NOLOCK) 
                                where p.ID = t.POID)   
                , [MC Handle] = dbo.getTPEPass1(t.McHandle) 
                , t.DoxType
                , [SpecMark] = (select Name 
                                from Style_SpecialMark sp WITH(NOLOCK) 
                                where sp.ID = t.[StyleSpecialMark]
                                and sp.BrandID = t.BrandID
                                and sp.Junk = 0) 
                , t.GFR
                , t.SampleReason
                , [TMS] = (select s.StdTms * t.CPU from System s WITH (NOLOCK)) 
";
            }
            else
            {
                sqlCmd += $@"
                -- 依撈出來的order資料(cte)去找各製程的WIP
                SELECT OrderId,Article,SizeCode
		                , firstSewingDate = min(X.OutputDate) 
                        , lastestSewingDate = max(X.OutputDate) 
                        , QAQTY = sum(X.QAQty) 
                        , AVG_QAQTY = AVG(X.QAQTY)
                into #tmp_SEWOUTPUT
                from (
                     SELECT b.OrderId,a.OutputDate,c.Article,c.SizeCode
                            , QAQty = sum(c.QAQty)
                     FROM DBO.SewingOutput a WITH (NOLOCK) 
                     inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
	                 inner join SewingOutput_Detail_Detail c WITH (NOLOCK) on c.SewingOutput_DetailUKey = b.UKey
	                 inner join (select distinct OrderID,Article,SizeCode from #cte) t on b.OrderId = t.OrderID and c.Article = t.Article and c.SizeCode = t.SizeCode 
                     group by b.OrderId,a.OutputDate ,c.Article,c.SizeCode
                ) X 
                group by OrderId,Article,SizeCode

                select c.OrderID
	                ,c.Article
	                ,c.SizeCode
	                ,first_cut_date = MIN(a.cDate)
                into #tmp_first_cut_date
                from dbo.CuttingOutput a WITH (NOLOCK) 
                inner join dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
                inner join dbo.WorkOrderForOutput_Distribute c WITH (NOLOCK) on c.WorkOrderForOutputUkey = b.WorkOrderForOutputUkey
                inner join (select distinct OrderID,Article,SizeCode from #cte) t on c.OrderID = t.OrderID and c.Article = t.Article and c.SizeCode = t.SizeCode
                group by c.OrderID,c.Article,c.SizeCode  

                select pd.OrderId,  pd.Article, pd.SizeCode, pd.ScanQty
                into #tmp_PackingList_Detail
                from PackingList_Detail pd WITH (NOLOCK)
                inner join #cte t on pd.OrderId = t.OrderID and pd.Article = t.Article and pd.SizeCode = t.SizeCode

                select 
                t.OrderID,t.Article,t.SizeCode
                , cut_qty = (SELECT SUM(CWIP.Qty) 
                            FROM DBO.CuttingOutput_WIP CWIP WITH (NOLOCK) 
                            WHERE CWIP.OrderID = T.OrderID and CWIP.Article = t.Article and CWIP.Size = t.SizeCode)
	            , f.first_cut_date
                , sewing_output = (select MIN(isnull(tt.qaqty,0)) 
                                    from dbo.style_location sl WITH (NOLOCK) 
                                    left join (
                                        SELECT c.ComboType
                                                , qaqty = sum(c.QAQty)  
                                        FROM DBO.SewingOutput a WITH (NOLOCK) 
                                        inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
								        inner join SewingOutput_Detail_Detail c WITH (NOLOCK) on c.SewingOutput_DetailUKey = b.UKey
								        where b.OrderId = t.OrderID and c.Article = t.Article and c.SizeCode = t.SizeCode
                                        group by c.ComboType 
                                    ) tt on tt.ComboType = sl.Location
                                    where sl.StyleUkey = t.StyleUkey) 
                , t.StyleUkey
                , EMBROIDERY_qty = (select qty = min(qty)  
                                    from (
                                        select qty = sum(b.Qty) 
                                                , c.PatternCode
                                                , c.ArtworkID ,c.Article,c.SizeCode
                                        from dbo.farmin a WITH (NOLOCK) 
                                        inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                        right join (
                                            select distinct v.ArtworkTypeID
                                                    , v.Article
										            , v.SizeCode
                                                    , v.ArtworkID
                                                    , v.PatternCode 
                                            from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                            where v.ID=t.OrderID and v.Article = t.Article and v.SizeCode = t.SizeCode
                                        ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                                and c.PatternCode = b.PatternCode 
                                                and c.ArtworkID = b.ArtworkID
                                        where a.ArtworkTypeId='EMBROIDERY'
                                                and b.Orderid = t.OrderID
                                        group by c.PatternCode,c.ArtworkID,c.Article,c.SizeCode
                                    ) x) 
                , BONDING_qty = (select qty = min(qty)  
                                from (
                                    select qty = sum(b.Qty)  
                                            , c.PatternCode
                                            , c.ArtworkID ,c.Article,c.SizeCode
                                    from dbo.farmin a WITH (NOLOCK) 
                                    inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                    right join (
                                        select distinct v.ArtworkTypeID, v.Article, v.SizeCode
                                                , v.ArtworkID
                                                , v.PatternCode 
                                        from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                        where v.ID=t.OrderID and v.Article = t.Article and v.SizeCode = t.SizeCode
                                    ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                            and c.PatternCode = b.PatternCode 
                                            and c.ArtworkID = b.ArtworkID
                                    where a.ArtworkTypeId='BONDING' 
                                            and b.Orderid = t.OrderID
                                    group by c.PatternCode,c.ArtworkID,c.Article,c.SizeCode
                                ) x) 
                , PRINTING_qty = (select qty = min(qty) 
                                    from (
                                    select qty = sum(b.Qty) 
                                            , c.PatternCode
                                            , c.ArtworkID ,c.Article,c.SizeCode
                                    from dbo.farmin a WITH (NOLOCK) 
                                    inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                    right join (
                                        select distinct v.ArtworkTypeID, v.Article, v.SizeCode
                                                , v.ArtworkID
                                                , v.PatternCode 
                                        from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                        where v.ID=t.OrderID and v.Article = t.Article and v.SizeCode = t.SizeCode
                                    ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                            and c.PatternCode = b.PatternCode 
                                            and c.ArtworkID = b.ArtworkID
                                    where a.ArtworkTypeId = 'PRINTING' 
                                            and b.Orderid = t.OrderID
                                    group by c.PatternCode,c.ArtworkID,c.Article,c.SizeCode
                                    ) x)
                , s.firstSewingDate
	            , s.lastestSewingDate
	            , s.QAQTY
	            , s.AVG_QAQTY
                into #cte2 
                from #cte t
                left join #tmp_first_cut_date f on f.OrderId = t.OrderID and f.Article = t.Article and f.SizeCode = t.SizeCode 
                left join #tmp_SEWOUTPUT s on s.OrderId = t.OrderID and s.Article = t.Article and s.SizeCode = t.SizeCode 

                drop table #tmp_first_cut_date, #tmp_SEWOUTPUT

                {qtyBySetPerSubprocess}

                select 
                t.MDivisionID
                , t.FactoryID
                , SewingSchedule.SewingLineID
                , t.OrdersBuyerDelivery
                , t.SciDelivery
                , SewingSchedule2.Inline
                , SewingSchedule2.Offline
                , IDD.val
                , t.BrandID
                , t.OrderID
                , t.POID
	            , T.[Buy Back]
	            , [Cancelled] = IIF(t.Junk=1 ,'Y' ,'')
	            , T.[Cancelled but Sill]
                , Dest = Country.Alias
                , t.StyleID
                , t.OrderTypeID
                , t.ShipModeList
	            , [PartialShipping]=IIF( (SELECT COUNT(ID) FROM Order_QtyShip WITH (NOLOCK) WHERE ID = t.OrderID) >=2 ,'Y' ,'')
                , [OrderNo] = t.Customize1
                , t.CustPONo
                , t.CustCDID
                , t.ProgramID
                , t.CdCodeID
	            , t.CDCodeNew
	            , t.ProductType
	            , t.FabricType
	            , t.Lining
	            , t.Gender
	            , t.Construction
                , t.KPILETA
                , t.LETA
                , t.MTLETA
                , t.SewETA
                , t.PackETA
                , t.CPU
	            , [TTL CPU] = t.CPU * t.Qty
	            , [CPU Closed]= t.CPU * #cte2.sewing_output
	            , [CPU bal]= t.CPU * (t.qty + t.FOCQty - #cte2.sewing_output)
                , t.Article
	            , t.SizeCode
                , t.Qty
                ,StandardOutput.StandardOutput
	            ,oriArtwork = ann.Artwork
	            ,AddedArtwork = EXa.Artwork
                ,Artwork.Artwork
                ,spdX.SubProcessDest
                ,EstCutDate.EstimatedCutDate
                , #cte2.first_cut_date
                , #cte2.cut_qty
                {subprocessQtyColumns}
                , #cte2.EMBROIDERY_qty
                , #cte2.BONDING_qty
                , #cte2.PRINTING_qty
                , #cte2.sewing_output
                , [Balance] = t.qty + t.FOCQty - #cte2.sewing_output 
                , #cte2.firstSewingDate              
	            , [Last Sewn Date] = vsis.LastSewDate
                , #cte2.AVG_QAQTY
                , [Est_offline] = DATEADD(DAY
                                , iif(isnull(#cte2.AVG_QAQTY, 0) = 0, 0
                                , ceiling((t.qty+t.FOCQty - #cte2.sewing_output) / (#cte2.AVG_QAQTY*1.0)))
                                , #cte2.firstSewingDate) 
                , [Scanned_Qty] = PackDetail.ScanQty
                , [pack_rate] = IIF(isnull(t.TotalCTN, 0) = 0, 0, round(t.ClogCTN / (t.TotalCTN * 1.0), 4) * 100 ) 
                , t.TotalCTN
                , FtyCtn = t.TotalCTN - t.FtyCTN
                , t.ClogCTN
                , t.CFACTN
                , t.InspDate
                , InspResult
                , [CFA Name] = InspHandle
                , t.ActPulloutDate
                , t.FtyKPI                
                , KPIChangeReason = KPIChangeReason.KPIChangeReason  
                , t.PlanDate
                , dbo.getTPEPass1(t.SMR) [SMR]
                , dbo.getTPEPass1(T.MRHandle) [Handle]
                , [PO SMR] = (select dbo.getTPEPass1(p.POSMR) 
                                from dbo.PO p WITH (NOLOCK) 
                                where p.ID = t.POID) 
                , [PO Handle] = (select dbo.getTPEPass1(p.POHandle) 
                                from dbo.PO p WITH (NOLOCK) 
                                where p.ID = t.POID)   
                , [MC Handle] = dbo.getTPEPass1(t.McHandle) 
                , t.DoxType
                , [SpecMark] = (select Name 
                                from Style_SpecialMark sp WITH(NOLOCK) 
                                where sp.ID = t.[StyleSpecialMark]
                                and sp.BrandID = t.BrandID
                                and sp.Junk = 0) 
                , t.GFR
                , t.SampleReason
                , [TMS] = (select s.StdTms * t.CPU  from System s WITH (NOLOCK))
";
            }

            if (model.IncludeAtworkData)
            {
                sqlCmd += $@",{model.ArtworkTypes.ToString()} ";
            }

            if (model.SummaryBy == "1")
            {
                sqlCmd += @" 
                from #cte t 
                inner join #cte2 on #cte2.OrderID = t.OrderID 
                left join Country with (Nolock) on Country.id= t.Dest
                left join #imp_LastSewnDate l on t.OrderID = l.OrderID";
            }
            else
            {
                string tmpbyline = model.SummaryBy == "3" ? @"
                ,t.FOCQty
                ,AlloQty =(select sum(sdd.AlloQty) from SewingSchedule_Detail sdd WITH (NOLOCK) where sdd.OrderID  = t.OrderID and sdd.Article =t.Article and sdd.SizeCode = t.SizeCode)
                into #lasttmp" : string.Empty;

                sqlCmd += $@"
                {tmpbyline}
                from #cte t 
                left join #cte2 on #cte2.OrderID = t.OrderID and #cte2.Article = t.Article and #cte2.SizeCode = t.SizeCode
                left join Country with (Nolock) on Country.id= t.Dest
                left join View_SewingInfoArticleSize vsis WITH (NOLOCK) on t.OrderID = vsis.OrderID and t.Article = vsis.Article and t.SizeCode = vsis.SizeCode";
            }

            if (model.IncludeAtworkData)
            {
                 sqlCmd += $@" left join #tmscost_pvt on #tmscost_pvt.orderid = t.orderid ";
            }

            if (model.SummaryBy == "1")
            {
                sqlCmd += $@"
                outer apply ( 
                    select KPIChangeReason = ID + '-' + Name   
                    from Reason  WITH (NOLOCK) 
                    where ReasonTypeID = 'Order_BuyerDelivery' 
                          and ID = t.KPIChangeReason 
                          and t.KPIChangeReason != '' 
                          and t.KPIChangeReason is not null 
                ) KPIChangeReason 
                outer apply (SELECT val =  Stuff((select concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = t.OrderID and oqs.IDD is not null FOR XML PATH('')),1,1,'') 
                  ) IDD
                {subprocessQtyColumnsSource}
                outer apply(
	                select StandardOutput =stuff((
		                  select distinct concat(',',ComboType,':',StandardOutput)
		                  from [SewingSchedule] WITH (NOLOCK) 
		                  where orderid = t.OrderID 
		                  for xml path('')
	                  ),1,1,'')
                )StandardOutput
                outer apply(select PatternUkey from dbo.GetPatternUkey(t.POID,'','',t.StyleUkey,''))gp
                outer apply(
	                select Artwork = STUFF((
		                select CONCAT('+', ArtworkTypeId)
		                from(
			                select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			                from(
				                SELECT bda1.SubprocessId
				                FROM Bundle_Detail_Order bd1 WITH (NOLOCK)
				                INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
				                WHERE bd1.Orderid=t.OrderID
	
				                EXCEPT
				                select s.Data
				                from(
					                Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
					                from Pattern_GL a WITH (NOLOCK) 
					                Where a.PatternUkey = gp.PatternUkey
					                and a.Annotation <> ''
				                )x
				                outer apply(select * from SplitString(x.Annotation ,'+'))s
			                )x
			                INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = x.SubprocessId
		                )x
		                order by ArtworkTypeID
		                for xml path('')
	                ),1,1,'')
                )EXa
                outer apply(
	                select Artwork = stuff((
		                select CONCAT('+',ArtworkTypeID)
		                from(
			                select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			                from(
				                Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
				                from Pattern_GL a WITH (NOLOCK) 
				                Where a.PatternUkey = gp.PatternUkey
				                and a.Annotation <> ''
			                )x
			                outer apply(select * from SplitString(x.Annotation ,'+'))s
			                INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = s.Data
		                )x
		                order by ArtworkTypeID
		                for xml path('')
	                ),1,1,'')
                )ann
                outer apply(
	                select Artwork =stuff((	
		                select concat('+',ArtworkTypeID)
		                from(
			                SELECT DISTINCT [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			                FROM Bundle_Detail_Order bd1 WITH (NOLOCK)
			                INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
			                INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID=bda1.SubprocessId
			                WHERE bd1.Orderid=t.OrderID
		                )tmpartwork
		                for xml path('')
	                ),1,1,'')
                )Artwork
                outer apply(
	                select SubProcessDest = concat('Inhouse:'+stuff((
		                select concat(',',ot.ArtworkTypeID)
		                from order_tmscost ot WITH (NOLOCK)
		                inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
		                where ot.id = t.OrderID and ot.InhouseOSP = 'I' 
		                and artworktype.isSubprocess = 1
		                for xml path('')
	                ),1,1,'')
	                ,'; '+(
	                select opsc=stuff((
		                select concat('; ',ospA.abb+':'+ospB.spdO)
		                from
		                (
			                select distinct abb = isnull(l.abb,'')
			                from order_tmscost ot WITH (NOLOCK)
			                inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
			                left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
			                where ot.id = t.OrderID and ot.InhouseOSP = 'o'
			                and artworktype.isSubprocess = 1
		                )ospA
		                outer apply(
			                select spdO = stuff((
				                select concat(',',ot.ArtworkTypeID) 
				                from order_tmscost ot WITH (NOLOCK)
				                inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
				                left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
				                where ot.id = t.OrderID and ot.InhouseOSP = 'o'and isnull(l.Abb,'') = ospA.abb
			                    and artworktype.isSubprocess = 1
				                for xml path('')
			                ),1,1,'')
		                )ospB
		                for xml path('')
	                ),1,1,'')))
                )spdX
                outer apply(select EstimatedCutDate = min(EstCutDate) from WorkOrderForOutput wo WITH (NOLOCK) where t.POID = wo.id)EstCutDate
                outer apply(
                    select ScanQty = sum(pd.ScanQty)
                    from #tmp_PackingList_Detail pd
                    where pd.OrderId = t.OrderID
                )PackDetail";
                sqlCmd += $@" order by {model.OrderBy}" + Environment.NewLine;
                sqlCmd += $@" drop table #cte, #cte2, #tmp_PackingList_Detail, #imp_LastSewnDate;" + Environment.NewLine;
            }
            else
            {
                sqlCmd += $@"
                outer apply ( 
                    select KPIChangeReason = ID + '-' + Name   
                    from Reason  WITH (NOLOCK) 
                    where ReasonTypeID = 'Order_BuyerDelivery' 
                          and ID = t.KPIChangeReason 
                          and t.KPIChangeReason != '' 
                          and t.KPIChangeReason is not null 
                ) KPIChangeReason 
                outer apply (SELECT val =  Stuff((select concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = t.OrderID and oqs.IDD is not null FOR XML PATH('')),1,1,'') 
                ) IDD
                {subprocessQtyColumnsSource}
                outer apply(
	                select SewingLineID =stuff((
		                  select distinct concat('/',ssd.SewingLineID)
		                  from [SewingSchedule] ss WITH (NOLOCK) 
		                  inner join SewingSchedule_Detail ssd  WITH (NOLOCK) on ssd.id = ss.id
		                  where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
		                  for xml path('')
	                  ),1,1,'')
                )SewingSchedule
                outer apply(
	                select Inline = MIN(ss.Inline),Offline = max(SS.Offline)
	                from [SewingSchedule] ss WITH (NOLOCK) 
	                inner join SewingSchedule_Detail ssd  WITH (NOLOCK) on ssd.id = ss.id
	                where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
                )SewingSchedule2
                outer apply(
	                select StandardOutput =stuff((
		                  select distinct concat(',',ssd.ComboType,':',StandardOutput)
		                  from [SewingSchedule] ss WITH (NOLOCK) 
		                  inner join SewingSchedule_Detail ssd  WITH (NOLOCK) on ssd.id = ss.id
		                  where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
		                  for xml path('')
	                  ),1,1,'')
                )StandardOutput
                outer apply(select PatternUkey from dbo.GetPatternUkey(t.POID,'','',t.StyleUkey,t.SizeCode))gp
                outer apply(
	                select Artwork = STUFF((
		                select CONCAT('+', ArtworkTypeId)
		                from(
			                select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			                from(
				                SELECT bda1.SubprocessId
				                FROM Bundle b1 WITH (NOLOCK)
				                INNER JOIN Bundle_Detail_Order bd1 WITH (NOLOCK) ON b1.ID = bd1.iD
				                INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
				                WHERE bd1.Orderid=t.OrderID AND b1.Article=t.Article AND b1.SizeCode=t.SizeCode
	
				                EXCEPT
				                select s.Data
				                from(
					                Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
					                from Pattern_GL a WITH (NOLOCK) 
					                Where a.PatternUkey = gp.PatternUkey
					                and a.Annotation <> ''
				                )x
				                outer apply(select * from SplitString(x.Annotation ,'+'))s
			                )x
			                INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = x.SubprocessId
		                )x
		                order by ArtworkTypeID
		                for xml path('')
	                ),1,1,'')
                )EXa
                outer apply(
	                select Artwork = stuff((
		                select CONCAT('+',ArtworkTypeID)
		                from(
			                select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			                from(
				                Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
				                from Pattern_GL a WITH (NOLOCK) 
				                Where a.PatternUkey = gp.PatternUkey
				                and a.Annotation <> ''
			                )x
			                outer apply(select * from SplitString(x.Annotation ,'+'))s
			                INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = s.Data
		                )x
		                order by ArtworkTypeID
		                for xml path('')
	                ),1,1,'')
                )ann
                outer apply(
	                select Artwork =stuff((	
		                select concat('+',ArtworkTypeID)
		                from(
			                SELECT DISTINCT [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			                FROM Bundle b1 WITH (NOLOCK)
			                INNER JOIN Bundle_Detail_Order bd1 WITH (NOLOCK) ON b1.ID = bd1.iD
			                INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
			                INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID=bda1.SubprocessId
			                WHERE bd1.Orderid=t.OrderID AND b1.Article=t.Article AND b1.SizeCode=t.SizeCode
		                )tmpartwork
		                for xml path('')
	                ),1,1,'')
                )Artwork
                outer apply(
	                select SubProcessDest = concat('Inhouse:'+stuff((
		                select concat(',',ot.ArtworkTypeID)
		                from order_tmscost ot WITH (NOLOCK)
		                inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
		                where ot.id = t.OrderID and ot.InhouseOSP = 'I' 
		                and artworktype.isSubprocess = 1
		                for xml path('')
	                ),1,1,'')
	                ,'; '+(
	                select opsc=stuff((
		                select concat('; ',ospA.abb+':'+ospB.spdO)
		                from
		                (
			                select distinct abb = isnull(l.abb,'')
			                from order_tmscost ot WITH (NOLOCK)
			                inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
			                left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
			                where ot.id = t.OrderID and ot.InhouseOSP = 'o'
			                and artworktype.isSubprocess = 1
		                )ospA
		                outer apply(
			                select spdO = stuff((
				                select concat(',',ot.ArtworkTypeID) 
				                from order_tmscost ot WITH (NOLOCK)
				                inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
				                left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
				                where ot.id = t.OrderID and ot.InhouseOSP = 'o'and isnull(l.Abb,'') = ospA.abb
			                    and artworktype.isSubprocess = 1
				                for xml path('')
			                ),1,1,'')
		                )ospB
		                for xml path('')
	                ),1,1,'')))
                )spdX
                outer apply(select EstimatedCutDate = min(EstCutDate) from WorkOrderForOutput wo WITH (NOLOCK) where t.POID = wo.id)EstCutDate
                outer apply(
                    select ScanQty = sum(pd.ScanQty)
                    from #tmp_PackingList_Detail pd
                    where pd.OrderId = t.OrderID
                    and pd.Article = t.Article
	                and pd.SizeCode = t.SizeCode
                )PackDetail";

                sqlCmd += $@" order by {model.OrderBy}, t.Article, t.SizeCode" + Environment.NewLine;
                sqlCmd += $@" drop table #cte, #cte2, #tmp_PackingList_Detail,#TMP_SubProcess,#tmp_SubProcess_ArtworkTypeID_Map,#tmp_SetQtyBySubprocess_Last,#tmp_SetQtyBySubprocess;" + Environment.NewLine;
                sqlCmd += dropTmp.Substring(0, dropTmp.Length - 1) + Environment.NewLine;
            }

            if (model.SummaryBy == "3")
            {
                string ars = string.Empty;

                if (model.IncludeAtworkData)
                {
                    foreach (DataRow dr in dtArtworkType.Rows)
                    {
                        ars += $@",[{dr["id"]}]=sum([{dr["id"]}])";
                    }
                }

                string subprocessQtyColumns_Line = @"
                , [RFID Cut Qty]                = SUM([RFID Cut Qty])
                , [RFID SewingLine In Qty]      = SUM([RFID SewingLine In Qty])
                , [RFID Loading Qty]            = SUM([RFID Loading Qty])
                , [RFID Emb Farm In Qty]        = SUM([RFID Emb Farm In Qty])
                , [RFID Emb Farm Out Qty]       = SUM([RFID Emb Farm Out Qty])
                , [RFID Bond Farm In Qty]       = SUM([RFID Bond Farm In Qty])
                , [RFID Bond Farm Out Qty]      = SUM([RFID Bond Farm Out Qty])
                , [RFID Print Farm In Qty]      = SUM([RFID Print Farm In Qty])
                , [RFID Print Farm Out Qty]     = SUM([RFID Print Farm Out Qty])
                , [RFID AT Farm In Qty]         = SUM([RFID AT Farm In Qty])
                , [RFID AT Farm Out Qty]        = SUM([RFID AT Farm Out Qty])
                , [RFID Pad Print Farm In Qty]  = SUM([RFID Pad Print Farm In Qty])
                , [RFID Pad Print Farm Out Qty] = SUM([RFID Pad Print Farm Out Qty])
                , [RFID Emboss Farm In Qty]     = SUM([RFID Emboss Farm In Qty])
                , [RFID Emboss Farm Out Qty]    = SUM([RFID Emboss Farm Out Qty])
                , [RFID HT Farm In Qty]         = SUM([RFID HT Farm In Qty])
                , [RFID HT Farm Out Qty]        = SUM([RFID HT Farm Out Qty])
                , [RFID AUT Farm In Qty]        = SUM([RFID AUT Farm In Qty])
                , [RFID AUT Farm Out Qty]       = SUM([RFID AUT Farm Out Qty])
                , [RFID FM Farm In Qty]         = SUM([RFID FM Farm In Qty])
                , [RFID FM Farm Out Qty]        = SUM([RFID FM Farm Out Qty])
                , ss.SubProcessStatus";
                string subprocessQtyColumnsSource_Line = @"
                outer apply(
	                select SubProcessStatus = IIF(
		                exists(
			                select 1
			                from #lasttmp t2 
			                where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			                and SubProcessStatus is null
		                )
		                or not exists(
			                select 1
			                from #lasttmp t2 
			                where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
		                ),
		                null , 'Y')
                )ss
                ";
                string subprocessQtyColumnGroup = ", ss.SubProcessStatus";

                // R15-1
                if (!isR15)
                {
                    subprocessQtyColumns_Line = model.SubprocessID.Split(',').Length > 1 ? this.MultiSuboricessColumns(2, model.SubprocessID, 2) : this.SingleSubprocessColumn(2, model.SubprocessID, 2);
                    subprocessQtyColumnsSource_Line = string.Empty;
                    subprocessQtyColumnGroup = string.Empty;
                }

                sqlCmd += $@"
                select
                t.MDivisionID
                , t.FactoryID
                , t.SewingLineID
                , t.OrdersBuyerDelivery
                , t.SciDelivery
                , Inline = min(t.Inline)
                , Offline = max(t.Offline)
                , t.val
                , t.BrandID
                , t.OrderID
                , t.POID
                , T.[Buy Back]
                , t.Cancelled
                , T.[Cancelled but Sill]
                , t.Dest
                , t.StyleID
                , t.OrderTypeID
                , t.ShipModeList
                , t.[PartialShipping]
                , t.[OrderNo]
                , t.CustPONo
                , t.CustCDID
                , t.ProgramID
                , t.CdCodeID
                , t.CDCodeNew
                , t.ProductType
                , t.FabricType
                , t.Lining
                , t.Gender
                , t.Construction
                , t.KPILETA
                , t.LETA
                , t.MTLETA
                , t.SewETA
                , t.PackETA
                , t.CPU
                , [TTL CPU] = t.CPU * SUM(t.Qty)
                , [CPU Closed] = t.CPU  * sum(t.sewing_output)
                , [CPU bal] = t.CPU * (SUM(t.Qty) + t.FOCQty - sum(t.sewing_output) )
                , article_list = al2.article_list
                , Qty = SUM(t.Qty)
                , AlloQty = sum(t.AlloQty)
                , st2.StandardOutput
                , oriArtwork = oann.oriArtwork
                , AddedArtwork = aann.AddedArtwork
                , Artwork.Artwork
                , t.SubProcessDest
                , EstimatedCutDate = MIN(t.EstimatedCutDate)
                , first_cut_date = MIN(t.first_cut_date)
                , cut_qty = SUM(cut_qty)
                {subprocessQtyColumns_Line}
                , EMBROIDERY_qty = SUM(t.EMBROIDERY_qty)
                , BONDING_qty = SUM(t.BONDING_qty)
                , PRINTING_qty = SUM(t.PRINTING_qty)
                , sewing_output = SUM(t.sewing_output)
                , [Balance] = SUM(t.Qty) + t.FOCQty - sum(t.sewing_output) 
                , firstSewingDate = MIN(firstSewingDate)
                , [Last Sewn Date] = MAX([Last Sewn Date])
                , AVG_QAQTY = AVG(AVG_QAQTY)
                , [Est_offline] = DATEADD(DAY
                                            , iif(isnull(AVG(AVG_QAQTY), 0) = 0, 0
                                                                                , ceiling(SUM(t.Qty) + t.FOCQty - sum(t.sewing_output)  / (AVG(AVG_QAQTY)*1.0)))
                                            , MIN(firstSewingDate)) 
                , [Scanned_Qty] = SUM(t.[Scanned_Qty])
                -- 以下來源 即by OrderID
                , t.[pack_rate]
                , t.TotalCTN
                , t.FtyCtn
                , t.ClogCTN
                , t.CFACTN
                , InspDate
                , InspResult
                , [CFA Name]
                , ActPulloutDate
                , FtyKPI
                , KPIChangeReason
                , PlanDate
                , SMR
                , Handle
                , [PO SMR]
                , [PO Handle]
                , [MC Handle]
                , DoxType
                , [SpecMark]
                , GFR
                , SampleReason
                , [TMS]
                {ars}
               from #lasttmp t
                outer apply(
	                select article_list = stuff((
		                select concat(',' , article)
		                from (
			                select distinct t2.Article 
			                from #lasttmp t2 
			                where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
		                ) t 
		                for xml path('')
	                ),1,1,'')
                )al2
                outer apply(
	                select StandardOutput = stuff((
		                select concat(',' , Data)
		                from (
			                select distinct s.Data
			                from #lasttmp t2 
			                outer apply(select * from SplitString(t2.StandardOutput ,','))s
			                where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
		                ) t 
		                for xml path('')
	                ),1,1,'')
                )st2
                outer apply(
	                select oriArtwork = stuff((
			                select concat('+' , Data)
			                from (
				                select distinct s.Data
				                from #lasttmp t2 
				                outer apply(select * from SplitString(t2.oriArtwork ,'+'))s
				                where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			                ) t 
			                for xml path('')
		                ),1,1,'')
                )oann
                outer apply(
	                select AddedArtwork = stuff((
			                select concat('+' , Data)
			                from (
				                select distinct s.Data
				                from #lasttmp t2 
				                outer apply(select * from SplitString(t2.AddedArtwork ,'+'))s
				                where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			                ) t 
			                for xml path('')
		                ),1,1,'')
                )aann
                outer apply(
	                select Artwork = stuff((
			                select concat('+' , Data)
			                from (
				                select distinct s.Data
				                from #lasttmp t2 
				                outer apply(select * from SplitString(t2.Artwork ,'+'))s
				                where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			                ) t 
			                for xml path('')
		                ),1,1,'')
                )Artwork
                {subprocessQtyColumnsSource_Line}

                group by 
                t.MDivisionID
                , t.FactoryID
                , t.SewingLineID
                , t.OrdersBuyerDelivery
                , t.SciDelivery
                , t.val
                , t.BrandID
                , t.OrderID
                , t.POID
                , t.Cancelled
                , t.Dest
                , t.StyleID
                , t.OrderTypeID
                , t.ShipModeList
                , t.[PartialShipping]
                , t.[OrderNo]
                , t.CustPONo
                , t.CustCDID
                , t.ProgramID
                , t.CdCodeID
                , t.CDCodeNew
                , t.ProductType
                , t.FabricType
                , t.Lining
                , t.Gender
                , t.Construction
                , t.KPILETA
                , t.LETA
                , t.MTLETA
                , t.SewETA
                , t.PackETA
                , t.CPU
                , t.FOCQty 
                , al2.article_list
                , st2.StandardOutput
                , oann.oriArtwork
                , aann.AddedArtwork
                , Artwork.Artwork
                , t.SubProcessDest
                {subprocessQtyColumnGroup}
                , t.[pack_rate]
                , t.TotalCTN
                , t.FtyCtn
                , t.ClogCTN
                , t.CFACTN
                , t.InspDate
                , InspResult
                , [CFA Name]
                , ActPulloutDate
                , FtyKPI
                , KPIChangeReason
                , PlanDate
                , SMR
                , Handle
                , [PO SMR]
                , [PO Handle]
                , [MC Handle]
                , DoxType
                , [SpecMark]
                , GFR
                , SampleReason
                , [TMS]
                ,T.[Buy Back]
                , T.[Cancelled but Sill]";
            }

            sqlCmd += $@"Select [subprocessInoutColumnCount] = {this.subprocessInoutColumnCount}";
            #endregion 組SQL

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlCmd, listPar, out System.Data.DataTable[] dataTables),
            };

            resultReport.DtArr = dataTables;
            return resultReport;
        }

        /// <summary>
        /// Get Where
        /// </summary>
        /// <param name="model">篩選條件</param>
        /// <returns>String Where</returns>
        public Tuple<string,string> GetWhere(Planning_R15_ViewModel model)
        {
            string strWhere1 = string.Empty;
            string strWhere2 = string.Empty;

            if (!model.IncludeCancelOrder)
            {
                strWhere1 += model.IsBI ? string.Empty : " and o.Junk = 0 ";
            }

            if (!model.StyleID.Empty())
            {
                strWhere1 += model.SummaryBy == "1" ? $@" and o.StyleID = @StyleID " : string.Empty;
            }

            if (!MyUtility.Check.Empty(model.StartSciDelivery) && !MyUtility.Check.Empty(model.EndSciDelivery))
            {
                strWhere1 += "and o.SciDelivery between @StartSciDelivery and @EndSciDelivery ";
            }
            else if (!MyUtility.Check.Empty(model.StartSciDelivery))
            {
                strWhere1 += $" and o.SciDelivery >= @StartSciDelivery ";
            }
            else if (!MyUtility.Check.Empty(model.EndSciDelivery))
            {
                strWhere1 += $" and o.SciDelivery <= @EndSciDelivery ";
            }

            if (!MyUtility.Check.Empty(model.StartSewingInline))
            {
                strWhere1 += $" and (o.SewInLine >= @StartSewingInline OR @StartSewingInline BETWEEN o.SewInLine AND o.SewOffLine ) ";
            }

            if (!MyUtility.Check.Empty(model.EndSewingInline))
            {
                strWhere1 += $" and (o.SewInLine <= @EndSewingInline OR @EndSewingInline BETWEEN o.SewInLine AND o.SewOffLine ) ";
            }

            if (!MyUtility.Check.Empty(model.StartBuyerDelivery) && !MyUtility.Check.Empty(model.EndBuyerDelivery))
            {
                strWhere1 += $@"
                and exists
                (
                    select 1 
                    from Order_QtyShip s WITH (NOLOCK) 
                    where s.id = o.ID
                    and s.BuyerDelivery between @StartBuyerDelivery and @EndBuyerDelivery
                )";
            }
            else if (!MyUtility.Check.Empty(model.StartBuyerDelivery))
            {
                strWhere1 += $@"
                and exists
                (
                    select 1 
                    from Order_QtyShip s WITH (NOLOCK) 
                    where s.id = o.ID
                    and s.BuyerDelivery >= @StartBuyerDelivery
                )";
            }

            if (!MyUtility.Check.Empty(model.StartCustRQSDate))
            {
                strWhere1 += " and o.CRDDate >= @StartCustRQSDate ";
            }

            if (!MyUtility.Check.Empty(model.EndCustRQSDate))
            {
                strWhere1 += " and o.CRDDate <= @EndCustRQSDate ";
            }

            if (!MyUtility.Check.Empty(model.StartCutOffDate))
            {
                strWhere1 += " and o.SDPDate >= @StartCutOffDate ";
            }

            if (!MyUtility.Check.Empty(model.EndCutOffDate))
            {
                strWhere1 += " and o.SDPDate <= @EndCutOffDate ";
            }

            if (!MyUtility.Check.Empty(model.StartPlanDate))
            {
                strWhere1 += " and o.PlanDate >= @StartPlanDate ";
            }

            if (!MyUtility.Check.Empty(model.EndPlanDate))
            {
                strWhere1 += " and o.PlanDate <= @EndPlanDate ";
            }

            if (!model.StartSP.Empty())
            {
                strWhere1 += " and o.id >= @StartSP ";
            }

            if (!model.EndSP.Empty())
            {
                strWhere1 += " and o.id <= @EndSP ";
            }

            if (!MyUtility.Check.Empty(model.StartLastSewDate) && !MyUtility.Check.Empty(model.EndLastSewDate))
            {
                strWhere1 += $@" and exists(select 1 from View_SewingInfoSP vsis with (nolock) where vsis.OrderID = o.ID and vsis.LastSewDate between @LastSewDateFrom and @LastSewDateTo) ";
            }

            if (!model.BrandID.Empty())
            {
                strWhere1 += @" and o.brandid = @BrandID ";
            }

            if (!model.CustCD.Empty())
            {
                strWhere1 += @" and o.CustCDID = @CustCD ";
            }

            if (!model.MDivisionID.Empty())
            {
                strWhere1 += @" and o.mdivisionid = @MDivisionID ";
            }

            if (!model.FactoryID.Empty())
            {
                strWhere1 += @" and o.FtyGroup = @FactoryID ";
            }

            if (model.OnlyShowCheckedSubprocessOrder)
            {
                strWhere1 += $@"
                and exists(
	                select * from Pattern_GL PGL WITH (NOLOCK) 
	                inner join dbo.GetPatternUkey(o.ID,'','',s.Ukey,'') t on PGL.PatternUKEY = t.PatternUkey
	                inner join dbo.SplitString('{model.SubprocessID}',',') b on PGL.Annotation like '%'+ b.Data+'%'
                )";
            }

            if (!model.Category.Empty())
            {
                strWhere1 += $@" and o.Category in ({model.Category})";
            }

            strWhere1 += " and exists (select 1 from Factory with (nolock) where o.FactoryId = id and IsProduceFty = 1)";

            return Tuple.Create(strWhere1, strWhere2);
        }

        private string SingleSubprocessColumn(int type = 0, string subprocessID = "", int summaryBy = 0)
        {
            string subprocessInoutRule = MyUtility.GetValue.Lookup($"select inoutRule from subprocess where id = '{subprocessID}'");
            string subprocessColumnName = MyUtility.GetValue.Lookup($"select ID from subprocess where id = '{subprocessID}'");
            switch (subprocessInoutRule)
            {
                case "1":
                    this.subprocessInoutColumnCount = 1;
                    return this.FarmInColmun(subprocessColumnName, subprocessID, summaryBy, type);
                case "2":
                    this.subprocessInoutColumnCount = 1;
                    return this.FarmOutColumn(subprocessColumnName, subprocessID, summaryBy, type);
                default:
                    this.subprocessInoutColumnCount = 2;
                    return this.FarmInColmun(subprocessColumnName, subprocessID, summaryBy, type) + this.FarmOutColumn(subprocessColumnName, subprocessID, summaryBy, type);
            }
        }

        private string MultiSuboricessColumns(int type = 0, string subprocessID = "", int summaryBy = 0)
        {
            string strReturn = string.Empty;
            string[] strSubprocess = subprocessID.Split(',');
            this.subprocessInoutColumnCount = 0;
            foreach (var item in strSubprocess)
            {
                string subprocessInoutRule = MyUtility.GetValue.Lookup($"select inoutRule from subprocess where id = '{item}'");
                string subprocessColumnName = MyUtility.GetValue.Lookup($"select ID from subprocess where id = '{item}'");
                switch (subprocessInoutRule)
                {
                    case "1":
                        this.subprocessInoutColumnCount += 1;
                        strReturn += this.FarmInColmun(subprocessColumnName, item.ToString(), summaryBy, type) + Environment.NewLine;
                        break;
                    case "2":
                        this.subprocessInoutColumnCount += 1;
                        strReturn += this.FarmOutColumn(subprocessColumnName, item.ToString(), summaryBy, type) + Environment.NewLine;
                        break;
                    default:
                        this.subprocessInoutColumnCount += 2;
                        strReturn += this.FarmInColmun(subprocessColumnName, item.ToString(), summaryBy, type) + this.FarmOutColumn(subprocessColumnName, item.ToString(), summaryBy, type) + Environment.NewLine;
                        break;
                }
            }

            return strReturn;
        }

        private string FarmInColmun(string subprocessColumnName, string strSubprocessID, int summaryBy = 0, int type = 0)
        {
            string subprocessIDtmp = Prgs.SubprocesstmpNoSymbol(strSubprocessID);
            if (type == 2)
            {
                return $@"
                , [RFID {subprocessColumnName} Farm In Qty] = SUM([RFID {subprocessColumnName} Farm In Qty])";
            }
            else
            {
                if (this.notExistsBundle_Detail_Art.Contains(strSubprocessID))
                {
                    return $@"
                    , [RFID {subprocessColumnName} Farm In Qty] = #{subprocessIDtmp}.InQtyBySet";
                }
                else
                {
                    return $@"
                    , [RFID {subprocessColumnName} Farm In Qty] =
                    IIF(EXISTS
                    (
                        SELECT 1
                        FROM Bundle_Detail_Order bd1 WITH (NOLOCK)
                        INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
                        INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID=bda1.SubprocessId
                        WHERE bd1.Orderid=t.OrderID
                    )
                    ,ISNULL( #{subprocessIDtmp}.InQtyBySet ,0),
                    #{subprocessIDtmp}.InQtyBySet)";
                }
            }
        }

        private string FarmOutColumn(string subprocessColumnName, string strSubprocessID, int summaryBy = 0, int type = 0)
        {
            string subprocessIDtmp = Prgs.SubprocesstmpNoSymbol(strSubprocessID);

            if (type == 2)
            {
                return $@"
                , [RFID {subprocessColumnName} Farm Out Qty] = SUM([RFID {subprocessColumnName} Farm Out Qty])";
            }
            else
            {
                if (this.notExistsBundle_Detail_Art.Contains(strSubprocessID))
                {
                    return $@"
                    , [RFID {subprocessColumnName} Farm Out Qty] = #{subprocessIDtmp}.OutQtyBySet";
                }
                else
                {
                    return $@"
                    , [RFID {subprocessColumnName} Farm Out Qty] =
                    IIF(EXISTS 
                    (
                        SELECT 1
                        FROM Bundle_Detail_Order bd1 WITH (NOLOCK)
                        INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
                        INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID=bda1.SubprocessId
                        WHERE bd1.Orderid=t.OrderID
                    )
                    , ISNULL( #{subprocessIDtmp}.OutQtyBySet ,0)
                    , #{subprocessIDtmp}.OutQtyBySet)";
                }
            }
        }

        /// <summary>
        /// 產生 SubProcess.ID 對應到 ArtworkTypeID 的對照表
        /// </summary>
        /// <param name="isNeedToTemp">是否須為暫存表</param>
        /// <returns>SQL String</returns>
        private string SqlTMP_SubProcess(bool isNeedToTemp = false, bool isR15 = true, string subprocessID = "")
        {
            string subProcess = "'Sorting', 'Loading'";
            if (isR15)
            {
                subProcess += ", 'Emb', 'BO', 'PRT', 'AT', 'PAD-PRT', 'SubCONEMB', 'HT', 'AUT', 'FM', 'SewingLine'";
            }
            else
            {
                subProcess += ", '" + subprocessID.Split(',').ToList().JoinToString("','") + "'";
            }

            return $@"
            SELECT [ArtworkTypeId] = IIF(S.ArtworkTypeId = '', S.ID, S.ArtworkTypeId), S.ID
            INTO #tmp_SubProcess
            FROM SubProcess S
            WHERE S.Id IN ({subProcess})
            SELECT *
            {(isNeedToTemp ? "INTO #tmp_SubProcess_ArtworkTypeID_Map" : string.Empty)}
            FROM (
                SELECT ID, ArtworkTypeId
                FROM #tmp_SubProcess
                UNION 
                SELECT S.ID, [ArtworkTypeId] = IIF(S.ArtworkTypeId = '', S.ID, S.ArtworkTypeId)
                FROM SubProcess S
                INNER JOIN #tmp_SubProcess T ON S.ArtworkTypeId = T.ArtworkTypeId
                WHERE T.ArtworkTypeId <> ''
            ) A";
        }
    }
}
