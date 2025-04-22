using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 調整欄位名稱
    /// 新增欄位
    /// 需要一併更新至 BI :  P_Import_PPICMasterListBIData
    /// **************************************
    /// ***  各為維護的大大 這支很複雜     ***
    /// ***  請不要因為趕時間硬塞,目視困難 ***
    /// ***  容易搞錯的規則請備註          ***
    /// **************************************
    /// </summary>
    public class PPIC_R03
    {
        /// <inheritdoc/>
        public PPIC_R03()
        {
            DBProxy.Current.DefaultTimeout = 7200;
        }

        /// <inheritdoc/>
        /// <summary>
        /// 資料展開欄位 OrderID
        /// 只有報表, 勾選 Seperate by <Qtyb'down by shipmode>, [部分]資訊會依據 Order_QtyShip.Seq 展開計算, 但報表上不顯示此 Seq
        /// </summary>
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
                new SqlParameter("@ArtworkTypeID", SqlDbType.VarChar) { Value = (object)model.ArtworkTypeID ?? DBNull.Value },
            };

            Base_ViewModel resultReport = new Base_ViewModel();
            var result = DBProxy.Current.Select(null, this.GetMainDatas(model), listPar, out DataTable[] printData);
            if (!result)
            {
                resultReport.Result = result;
                return resultReport;
            }

            resultReport.Result = result;
            resultReport.DtArr = printData;
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
            // 輸出 Pkey 欄位
            // 1.無勾選 Seperate by < Qty b'down by shipmode > → [SPNO] = OrderID
            // 2.有勾選 Seperate by < Qty b'down by shipmode > → [SPNO] = OrderID, [Seq] = Order_QtyShip.Seq
            #region 篩選基礎條件, 不包含 List PO Combo 判斷
            string where_NoRestrictOrdersDelivery = string.Empty;
            string whereOrders = string.Empty;
            if (MyUtility.Check.Seek($"SELECT 1 FROM System WHERE NoRestrictOrdersDelivery = 0"))
            {
                where_NoRestrictOrdersDelivery = "AND (o.IsForecast = 0 OR (o.IsForecast = 1 AND (o.SciDelivery <= DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 6) OR o.BuyerDelivery < DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 0))))\r\n";
                whereOrders += where_NoRestrictOrdersDelivery;
            }

            string order_QtyShip_Seq = string.Empty;
            string order_QtyShip_Join = string.Empty;
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
                // 是否展開 Order_QtyShip, Pkey:ID, Seq
                if (model.SeparateByQtyBdownByShipmode)
                {
                    order_QtyShip_Seq = ",Seq";
                    order_QtyShip_Join = "INNER JOIN Order_QtyShip oqs WITH (NOLOCK) ON oqs.ID = o.ID";
                    if (model.BuyerDelivery1.HasValue)
                    {
                        whereOrders += $"AND oqs.BuyerDelivery >= @BuyerDelivery1\r\n";
                    }

                    if (model.BuyerDelivery2.HasValue)
                    {
                        whereOrders += $"AND oqs.BuyerDelivery <= @BuyerDelivery2\r\n";
                    }

                    if (model.SDPDate1.HasValue)
                    {
                        whereOrders += "AND oqs.SDPDate >= @SDPDate1\r\n";
                    }

                    if (model.SDPDate2.HasValue)
                    {
                        whereOrders += "AND oqs.SDPDate <= @SDPDate2\r\n";
                    }
                }
                else
                {
                    if (model.BuyerDelivery1.HasValue)
                    {
                        whereOrders += $"AND o.BuyerDelivery >= @BuyerDelivery1\r\n";
                    }

                    if (model.BuyerDelivery2.HasValue)
                    {
                        whereOrders += $"AND o.BuyerDelivery <= @BuyerDelivery2\r\n";
                    }

                    if (model.SDPDate1.HasValue || model.SDPDate2.HasValue)
                    {
                        string whereSDPDate = string.Empty;
                        if (model.SDPDate1.HasValue)
                        {
                            whereSDPDate += "AND oqs.SDPDate >= @SDPDate1\r\n";
                        }

                        if (model.SDPDate2.HasValue)
                        {
                            whereSDPDate += "AND oqs.SDPDate <= @SDPDate2\r\n";
                        }

                        whereOrders += $"AND EXISTS (SELECT 1 FROM Order_QtyShip oqs WITH (NOLOCK) WHERE oqs.ID = o.ID {whereSDPDate})\r\n";
                    }
                }

                if (model.SciDelivery1.HasValue)
                {
                    whereOrders += "AND o.SciDelivery >= @SCIDelivery1\r\n";
                }

                if (model.SciDelivery2.HasValue)
                {
                    whereOrders += "AND o.SciDelivery <= @SCIDelivery2\r\n";
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

                if (model.Forecast)
                {
                    listCategory.Add("''");
                }

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
                    whereOrders += "AND EXISTS(SELECT 1 FROM Style_TmsCost stc WITH (NOLOCK) WHERE stc.StyleUkey = o.StyleUkey AND stc.ArtworkTypeID = @ArtworkTypeID AND ((stc.Qty > 0 AND stc.Price > 0) OR stc.TMS > 0))\r\n";
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

            #region step 1 基礎條件 取得 OrderID, POID, or (Seq)
            string sqlcmd = $@"
--1.先依據篩選條件找出需要資料,不做任何處理
SELECT
    o.ID
   ,o.POID
    {order_QtyShip_Seq}
INTO #tmpOrdersBase
FROM Orders o WITH (NOLOCK)
{order_QtyShip_Join}
WHERE 1 = 1
{whereOrders}
";
            #endregion

            #region step 2 有or無 <List PO Combo> 條件, 用 step1 資料的 POID, 找出所有 OrderID <<<除了NoRestrictOrdersDelivery此條件, 不限制基礎條件, 從原 PPIC R03 整理出來條件>>>
            if (model.ListPOCombo)
            {
                // <此篩選出的 OrderID 不受畫面上篩選條件限制>
                sqlcmd += $@"
--2. List PO Combo 有勾選, 不回找 POID = ''
SELECT
    o.ID
   ,o.POID
    {order_QtyShip_Seq}   
INTO #tmpPOComboStep
FROM Orders o WITH (NOLOCK)
{order_QtyShip_Join}
WHERE EXISTS(SELECT 1 FROM #tmpOrdersBase WHERE POID = o.POID AND POID <> '')
{where_NoRestrictOrdersDelivery}

--Category = '' 的 POID = '' 不可回找
UNION ALL
SELECT *
FROM #tmpOrdersBase b
WHERE POID = ''
";
            }
            else
            {
                sqlcmd += $@"
--2. List PO Combo 無勾選
SELECT *
INTO #tmpPOComboStep
FROM #tmpOrdersBase b
";
            }
            #endregion

            #region step 3 有or無 勾選 Seperate by < Qty b'down by shipmode >, 不同展開方式的欄位 來源( Orders / Order_QtyShip)
            if (model.SeparateByQtyBdownByShipmode)
            {
                sqlcmd += $@"
--處理 有 展開 Separate By Qty BdownBy Shipmode 的欄位
SELECT
    s.*
   ,oqs.BuyerDelivery
   ,oqs.IDD
   ,oqs.CFAIs3rdInspect
   ,oqs.Qty
   ,oqs.EstPulloutDate
   ,oqs.FtyKPI
   ,oqs.CFAFinalInspectDate
   ,oqs.CFAFinalInspectResult
   ,oqs.CFAFinalInspectHandle
   ,oqs.ShipmodeID
INTO #tmpOqs_Step
FROM #tmpPOComboStep s
INNER JOIN Order_QtyShip oqs WITH (NOLOCK) ON s.ID = oqs.ID AND s.Seq = oqs.Seq
";
            }
            else
            {
                sqlcmd += $@"
--處理 無 展開 Separate By Qty BdownBy Shipmode 的欄位
SELECT
    s.*
   ,o.BuyerDelivery
   ,IDD = STUFF((
            SELECT DISTINCT ',' + FORMAT(oqs.IDD, 'yyyy/MM/dd')
            FROM Order_QtyShip oqs WITH (NOLOCK)
            WHERE oqs.ID = o.ID
            FOR XML PATH (''))
        , 1, 1, '')
   ,CFAIs3rdInspect = (
        SELECT COUNT(1)
        FROM Order_Qtyship oqs WITH (NOLOCK)
        WHERE oqs.ID = o.ID
        AND oqs.CFAIs3rdInspect = 1)
   ,o.Qty
   ,EstPulloutDate = PulloutDate
   ,o.FtyKPI
   ,CFAFinalInspectDate = STUFF((
            SELECT DISTINCT ',' + FORMAT(CFAFinalInspectDate, 'yyyy/MM/dd')
            FROM Order_QtyShip oqs WITH (NOLOCK)
            WHERE oqs.ID = o.ID
            FOR XML PATH (''))
        , 1, 1, '')
   ,CFAFinalInspectResult = STUFF((
            SELECT DISTINCT ',' + CFAFinalInspectResult
            FROM Order_QtyShip oqs WITH (NOLOCK)
            WHERE oqs.ID = o.ID
            AND CFAFinalInspectResult <> ''
            FOR XML PATH (''))
        , 1, 1, '')
   ,CFAFinalInspectHandle = STUFF((
            SELECT DISTINCT ',' + CFAFinalInspectHandle + '-' + p.Name
            FROM Order_QtyShip oqs WITH (NOLOCK)
            LEFT JOIN Pass1 p WITH (NOLOCK) ON oqs.CFAFinalInspectHandle = p.ID
            WHERE oqs.ID = o.id
            AND CFAFinalInspectHandle <> ''
            FOR XML PATH (''))
        , 1, 1, '')
   ,ShipmodeID = o.ShipModeList
INTO #tmpOqs_Step
FROM #tmpPOComboStep s
INNER JOIN Orders o ON s.ID = o.ID
";
            }
            #endregion

            #region step 4 來源 Orders 欄位, #tmpOrders 後續處理各資料使用
            sqlcmd += $@"
SELECT
    s.*
   ,o.StyleUkey
   ,o.MDivisionID
   ,o.FactoryID
   ,o.SciDelivery
   ,o.CRDDate
   ,o.CFMDate
   ,o.Category
   ,o.ForecastSampleGroup
   ,o.BuyMonth
   ,o.Junk
   ,o.NeedProduction
   ,o.KeepPanels
   ,o.StyleID
   ,o.SeasonID
   ,o.BrandID
   ,o.OrderTypeID
   ,o.ProjectID
   ,o.HangerPack
   ,o.JokerTag
   ,o.HeatSeal
   ,o.Customize1
   ,o.CustPONo
   ,o.Customize4
   ,o.Customize5
   ,o.VasShas
   ,o.MnorderApv
   ,o.MnorderApv2
   ,o.KPIMNotice
   ,o.TissuePaper
   ,o.AirFreightByBrand
   ,o.GFR
   ,o.CustCDID
   ,o.BrandFTYCode
   ,o.ProgramID
   ,o.NonRevenue
   ,o.CPU
   ,o.FOCQty
   ,o.CPUFactor
   ,o.PoPrice
   ,o.KPILETA
   ,o.PFETA
   ,o.PackLETA
   ,o.LETA
   ,o.MTLETA
   ,o.SewETA
   ,o.PackETA
   ,o.MTLExport
   ,o.MTLComplete
   ,o.SewInLine
   ,o.SewOffLine
   ,o.FirstProduction
   ,o.LastProductionDate
   ,o.EachConsApv
   ,o.KpiEachConsCheck
   ,o.CutInLine
   ,o.CutOffLine
   ,o.PulloutComplete
   ,o.KPIChangeReason
   ,o.PlanDate
   ,o.OrigBuyerDelivery
   ,o.SMR
   ,o.MRHandle
   ,o.MCHandle
   ,o.DoxType
   ,o.SewLine
   ,o.Customize2
   ,o.IsMixMarker
   ,o.CuttingSP
   ,o.RainwearTestPassed
   ,o.MdRoomScanDate
   ,o.DryRoomRecdDate
   ,o.DryRoomTransDate
   ,o.LastCTNTransDate
   ,o.LastCTNRecdDate
   ,o.OrganicCotton
   ,o.DirectShip
   ,o.Dest
   ,o.CtnType
   ,o.SampleReason
   ,o.Max_ScheETAbySP
   ,o.Sew_ScheETAnoReplace
   ,o.MaxShipETA_Exclude5x
   ,o.BuyBackReason
   ,o.StyleUnit
   ,o.SubconInType
   ,o.isForecast
   ,o.GMTComplete
   ,o.LocalMR
INTO #tmpOrders
FROM #tmpOqs_Step s
INNER JOIN Orders o WITH (NOLOCK) ON s.ID = o.ID

CREATE NONCLUSTERED INDEX index_tmpOrders_ID ON #tmpOrders(ID ASC);
";
            #endregion

            #region step 5-1 暫存表 有or無 勾選 Seperate by < Qty b'down by shipmode > 因為效能, 需要(聚合/串接字串)的欄位先分別處理
            string sqlmaybePldSeq = model.SeparateByQtyBdownByShipmode ? "AND pld.OrderShipmodeSeq = o.Seq" : string.Empty;
            string sqlmaybeInvSeq = model.SeparateByQtyBdownByShipmode ? "AND i.OrderShipmodeSeq = o.Seq" : string.Empty;
            string sqltmp = $@"
LEFT JOIN #tmp_PackingList_Detail pld ON o.ID = pld.OrderID {sqlmaybePldSeq}
LEFT JOIN #tmp_invAdj i ON o.ID = i.OrderID {sqlmaybeInvSeq}
";
            if (model.SeparateByQtyBdownByShipmode)
            {
                sqlcmd += $@"
--暫存表 可 by OrderID,OrderShipmodeSeq 部分
SELECT
    pld.OrderID
   ,pld.OrderShipmodeSeq
   ,ScanEditDate = MAX(pld.ScanEditDate)
   ,PulloutQty = SUM(IIF(pl.PulloutID <> '', pld.ShipQty, 0))
   ,PackingCTN = SUM(pld.CTNQty)
   ,TotalCTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0, pld.CTNQty, 0))
   ,FtyCtn_Remaining = SUM(IIF(pl.Type IN ('B', 'L') AND pld.TransferDate IS NULL, pld.CTNQty, 0))--和 UpdateOrdersCTN 中的 FtyCtn 不一樣喔, 這是目前在工廠的剩餘紙箱數量
   ,ClogCTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0 AND pld.ReceiveDate IS NOT NULL AND pld.TransferCFADate IS NULL AND pld.CFAReturnClogDate IS NULL, pld.CTNQty, 0))
   ,FtyToClogTransit = SUM(IIF(pld.TransferDate IS NOT NULL AND pld.ReceiveDate  IS NULL, pld.CTNQty, 0))
   ,ClogToCFATansit = SUM(IIF(pld.TransferCFADate IS NOT NULL AND pld.CFAReceiveDate IS NULL AND pld.ClogLocationID = '2CFA', pld.CTNQty, 0))
   ,CFACTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0 AND pld.CFAReceiveDate IS NOT NULL, pld.CTNQty, 0))
   ,CFAToClogTransit = SUM(IIF(pld.TransferCFADate IS NULL AND pld.CFAReceiveDate IS NULL AND pld.CFAReturnClogDate IS NOT NULL AND pld.ClogLocationID = '2Clog', pld.CTNQty, 0))
   ,ClogLastReceiveDate = MAX(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0, ReceiveDate, NULL))
   ,PackErrCTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0 AND pld.PackErrTransferDate IS NOT NULL, pld.CTNQty, 0))
   ,PackingQty = SUM(IIF(pl.Type <> 'F', pld.ShipQty, 0))
   ,PackingFOCQty = SUM(IIF(pl.Type = 'F', pld.ShipQty, 0))
   ,BookingQty = SUM(IIF(pl.Type IN ('B', 'L') AND pl.INVNo <> '', pld.ShipQty, 0))
   ,ClogRcvDate = MAX(ReceiveDate)
   ,ActPulloutDate = MAX(pl.PulloutDate)
    ,HaulingDate = MAX(pld.HaulingDate)
    ,HaulingStatus = IIF( SUM(IIF( pld.HaulingStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,HaulingFailQty = SUM(pld.HaulingFailQty)
    ,PackingAuditDate = MAX(pld.PackingAuditDate)
    ,PackingAuditStatus = IIF( SUM(IIF( pld.PackingAuditStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,PackingAuditFailQty = SUM(pld.PackingAuditFailQty)
    ,M360MDScanDate = MAX(pld.M360MDScanDate)
    ,M360MDStatus =  IIF( SUM(IIF( pld.M360MDStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,M360MDFailQty = SUM(pld.M360MDFailQty)
    ,HangerPackScanTime = MAX(pld.HangerPackScanTime)
    ,HangerPackStatus = IIF( SUM(IIF( pld.HangerPackStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,HangerPackFailQty = SUM(pld.HangerPackFailQty)
    ,JokerTagScanTime = MAX(pld.JokerTagScanTime)
    ,JokerTagStatus = IIF( SUM(IIF( pld.JokerTagStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,JokerTagFailQty = SUM(pld.JokerTagFailQty)
    ,HeatSealScanTime = MAX(pld.HeatSealScanTime)
    ,HeatSealStatus = IIF( SUM(IIF( pld.HeatSealStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,HeatSealFailQty = SUM(pld.HeatSealFailQty)
INTO #tmp_PackingList_Detail
FROM PackingList_Detail pld WITH (NOLOCK)
INNER JOIN PackingList pl WITH (NOLOCK) ON pld.ID = pl.ID
WHERE EXISTS (SELECT 1 FROM #tmpOrders o WHERE pld.OrderID = o.ID AND pld.OrderShipmodeSeq = o.Seq)
GROUP BY pld.OrderID, pld.OrderShipmodeSeq

SELECT
    i.OrderID
   ,i.OrderShipmodeSeq
   ,InvoiceAdjQty = SUM(iq.DiffQty)
   ,FOCAdjQty = SUM(IIF(iq.Price = 0, iq.DiffQty, 0))
INTO #tmp_invAdj
FROM InvAdjust i WITH (NOLOCK)
INNER JOIN InvAdjust_Qty iq WITH (NOLOCK) ON i.ID = iq.ID
WHERE EXISTS (SELECT 1 FROM #tmpOrders o WHERE i.OrderID = o.ID AND i.OrderShipmodeSeq = o.Seq)
GROUP BY i.OrderID, i.OrderShipmodeSeq
";
            }
            else
            {
                sqlcmd += $@"
--暫存表 可 by OrderID,OrderShipmodeSeq 部分
SELECT
    pld.OrderID
   ,ScanEditDate = MAX(pld.ScanEditDate)
   ,PulloutQty = SUM(IIF(pl.PulloutID <> '', pld.ShipQty, 0))
   ,PackingCTN = SUM(pld.CTNQty)
   ,TotalCTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0, pld.CTNQty, 0))
   ,FtyCtn_Remaining = SUM(IIF(pl.Type IN ('B', 'L') AND pld.TransferDate IS NULL, pld.CTNQty, 0))--和 UpdateOrdersCTN 中的 FtyCtn 不一樣喔, 這是目前在工廠的剩餘紙箱數量
   ,ClogCTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0 AND pld.ReceiveDate IS NOT NULL AND pld.TransferCFADate IS NULL AND pld.CFAReturnClogDate IS NULL, pld.CTNQty, 0))
   ,FtyToClogTransit = SUM(IIF(pld.TransferDate IS NOT NULL AND pld.ReceiveDate  IS NULL, pld.CTNQty, 0))
   ,ClogToCFATansit = SUM(IIF(pld.TransferCFADate IS NOT NULL AND pld.CFAReceiveDate IS NULL AND pld.ClogLocationID = '2CFA', pld.CTNQty, 0))
   ,CFACTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0 AND pld.CFAReceiveDate IS NOT NULL, pld.CTNQty, 0))
   ,CFAToClogTransit = SUM(IIF(pld.TransferCFADate IS NULL AND pld.CFAReceiveDate IS NULL AND pld.CFAReturnClogDate IS NOT NULL AND pld.ClogLocationID = '2Clog', pld.CTNQty, 0))
   ,ClogLastReceiveDate = MAX(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0, ReceiveDate, NULL))
   ,PackErrCTN = SUM(IIF(pl.Type IN ('B', 'L') AND pld.DisposeFromClog = 0 AND pld.PackErrTransferDate IS NOT NULL, pld.CTNQty, 0))
   ,PackingQty = SUM(IIF(pl.Type <> 'F', pld.ShipQty, 0))
   ,PackingFOCQty = SUM(IIF(pl.Type = 'F', pld.ShipQty, 0))
   ,BookingQty = SUM(IIF(pl.Type IN ('B', 'L') AND pl.INVNo <> '', pld.ShipQty, 0))
   ,ClogRcvDate = MAX(ReceiveDate)
   ,ActPulloutDate = MAX(pl.PulloutDate)
    ,HaulingDate = MAX(pld.HaulingDate)
    ,HaulingStatus = IIF( SUM(IIF( pld.HaulingStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,HaulingFailQty = SUM(pld.HaulingFailQty)
    ,PackingAuditDate = MAX(pld.PackingAuditDate)
    ,PackingAuditStatus = IIF( SUM(IIF( pld.PackingAuditStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,PackingAuditFailQty = SUM(pld.PackingAuditFailQty)
    ,M360MDScanDate = MAX(pld.M360MDScanDate)
    ,M360MDStatus =  IIF( SUM(IIF( pld.M360MDStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,M360MDFailQty = SUM(pld.M360MDFailQty)
    ,HangerPackScanTime = MAX(pld.HangerPackScanTime)
    ,HangerPackStatus = IIF( SUM(IIF( pld.HangerPackStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,HangerPackFailQty = SUM(pld.HangerPackFailQty)
    ,JokerTagScanTime = MAX(pld.JokerTagScanTime)
    ,JokerTagStatus = IIF( SUM(IIF( pld.JokerTagStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,JokerTagFailQty = SUM(pld.JokerTagFailQty)
    ,HeatSealScanTime = MAX(pld.HeatSealScanTime)
    ,HeatSealStatus = IIF( SUM(IIF( pld.HeatSealStatus = 'Return' ,1 ,0)) > 0 ,'Return' ,'Pass')
    ,HeatSealFailQty = SUM(pld.HeatSealFailQty)
INTO #tmp_PackingList_Detail
FROM PackingList_Detail pld WITH (NOLOCK)
INNER JOIN PackingList pl WITH (NOLOCK) ON pld.ID = pl.ID
WHERE EXISTS (SELECT 1 FROM #tmpOrders o WHERE pld.OrderID = o.ID)
GROUP BY pld.OrderID

SELECT
    i.OrderID
   ,InvoiceAdjQty = SUM(iq.DiffQty)
   ,FOCAdjQty = SUM(IIF(iq.Price = 0, iq.DiffQty, 0))
INTO #tmp_invAdj
FROM InvAdjust i WITH (NOLOCK)
INNER JOIN InvAdjust_Qty iq WITH (NOLOCK) ON i.ID = iq.ID
WHERE EXISTS (SELECT 1 FROM #tmpOrders o WHERE i.OrderID = o.ID)
GROUP BY i.OrderID
";
            }

            #endregion

            #region step 5-2 暫存表 因為效能, 需要(聚合/串接字串)的欄位先分別處理

            // by StyleUkey
            sqlcmd += @"
-- by StyleUkey
SELECT
    StyleUkey
   ,GetStyleUkey = dbo.GetSimilarStyleList(StyleUkey)
INTO #tmp_StyleUkey
FROM (SELECT DISTINCT StyleUkey FROM #tmpOrders) o";

            // by OrderID
            sqlcmd += @"
--by OrderID
SELECT
    o.ID
   ,[Garment L/T] = dbo.GetGMTLT(o.BrandID, o.StyleID, o.SeasonID, o.FactoryID, o.ID)
INTO #tmpGMTLT
FROM (SELECT DISTINCT o.BrandID, o.StyleID, o.SeasonID, o.FactoryID, o.ID FROM #tmpOrders o) o

SELECT
    f.OrderID
   ,[PackingMtlComplt] = MAX([PackingMtlComplt])
   ,[SewingMtlComplt] = MAX([SewingMtlComplt])
INTO #tmpComplt
FROM (
    SELECT
        f.OrderID
       ,[PackingMtlComplt] = IIF(f.ProductionType = 'Packing' AND SUM(IIF(f.ProductionType = 'Packing', 1, 0)) = SUM(IIF(f.ProductionType = 'Packing' AND f.Complete = 1, 1, 0)), 'Y', '')
       ,[SewingMtlComplt] = IIF(f.ProductionType <> 'Packing' AND SUM(IIF(f.ProductionType <> 'Packing', 1, 0)) = SUM(IIF(f.ProductionType <> 'Packing' AND f.Complete = 1, 1, 0)), 'Y', '')
    FROM (
        SELECT
            psdo.OrderID
           ,[ProductionType] = IIF(m.ProductionType = 'Packing', 'Packing', 'Sewing')
           ,psd.Complete
        FROM PO_Supp_Detail_OrderList psdo WITH (NOLOCK)
        INNER JOIN PO_Supp_Detail psd WITH (NOLOCK) ON psd.ID = psdo.ID AND psd.SEQ1 = psdo.SEQ1 AND psd.SEQ2 = psdo.SEQ2
        LEFT JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = psd.SCIRefno
        LEFT JOIN MtlType m WITH (NOLOCK) ON f.MtlTypeID = m.ID
        WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE ID = psdo.OrderID)
        AND psd.Junk = 0
    ) f
    GROUP BY f.ProductionType, f.OrderID
) f
GROUP BY f.OrderID

SELECT
    sod.OrderID
   ,SewQtyTop = SUM(IIF(sod.ComboType = 'T', sod.QAQty, 0))
   ,SewQtyBottom = SUM(IIF(sod.ComboType = 'B', sod.QAQty, 0))
   ,SewQtyInner = SUM(IIF(sod.ComboType = 'I', sod.QAQty, 0))
   ,SewQtyOuter = SUM(IIF(sod.ComboType = 'O', sod.QAQty, 0))
   ,FirstOutDate = MIN(so.OutputDate)
   ,LastOutDate = MAX(so.OutputDate)
INTO #tmp_sewDetial
FROM SewingOutput so WITH (NOLOCK)
INNER JOIN SewingOutput_Detail sod WITH (NOLOCK) ON so.ID = sod.ID
WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE ID = sod.OrderID)
GROUP BY sod.OrderID

SELECT
    ID
   ,[Total Sewing Output] = ISNULL(dbo.getMinCompleteSewQty(o.ID, NULL, NULL), 0)
INTO #tmp_TtlSewQty
FROM (SELECT DISTINCT ID FROM #tmpOrders) o

SELECT
    OrderID
   ,CutQty = SUM(Qty)
INTO #tmpCutQty
FROM CuttingOutput_WIP wip WITH (NOLOCK)
WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE ID = wip.OrderID)
GROUP BY wip.OrderID

SELECT
    ID
   ,op.Remark
INTO #tmp_PFRemark
FROM (
    SELECT
        op.ID
       ,op.Remark
       ,rn =
        ROW_NUMBER() OVER (PARTITION BY op.ID ORDER BY op.AddDate DESC)
    FROM Order_PFHis op WITH (NOLOCK)
    WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE ID = op.ID)
    AND AddDate IS NOT NULL
) op
WHERE rn = 1

SELECT
    wd.OrderID
   ,InLine = MIN(w.EstCutDate)
   ,OffLine = MAX(w.EstCutDate) INTO #tmpEstCutDate
FROM WorkOrder_Distribute wd WITH (NOLOCK)
INNER JOIN WorkOrder w WITH (NOLOCK) ON wd.WorkOrderUkey = w.Ukey
WHERE w.EstCutDate IS NOT NULL
AND EXISTS (SELECT 1 FROM #tmpOrders WHERE ID = wd.OrderID)
GROUP BY wd.OrderID

SELECT
    o.ID
   ,Article = STUFF((
        SELECT ',' + oq.Article
        FROM Order_Qty oq WITH (NOLOCK)
        WHERE oq.ID = o.ID
        GROUP BY oq.Article
        ORDER BY oq.Article
        FOR XML PATH (''))
    , 1, 1, '')
INTO #tmp_Article
FROM (SELECT DISTINCT ID FROM #tmpOrders) o

SELECT DISTINCT
    o.ID
   ,oc.ColorID
INTO #tmpOrder_ColorCombo
FROM #tmpOrders o
INNER JOIN Order_QtyShip_Detail od WITH (NOLOCK) ON o.ID = od.ID
INNER JOIN Order_ColorCombo oc WITH (NOLOCK) ON o.POID = oc.ID AND od.Article = oc.Article
WHERE oc.PatternPanel = 'FA'

SELECT
    o.ID
   ,ColorID = STUFF((
        SELECT ',' + ColorID
        FROM #tmpOrder_ColorCombo
        WHERE id = o.ID
        FOR XML PATH (''))
    , 1, 1, '')
INTO #tmpColorCombo
FROM (SELECT DISTINCT ID FROM #tmpOrders) o
";

            // by POID
            sqlcmd += @"
--暫存表 by POID
SELECT
    POID
   ,EarliestSCIDlv = dbo.getMinSCIDelivery(POID, '')
INTO #tmp_EarliestSCIDlv
FROM (SELECT DISTINCT POID FROM #tmpOrders WHERE POID <> '') o

SELECT
    POID
   ,MTLDelay = dbo.GetHaveDelaySupp(POID)
INTO #tmp_MTLDelay
FROM (SELECT DISTINCT POID FROM #tmpOrders WHERE POID <> '') o

SELECT
    POID
   ,MTLExportTimes = CONCAT(COUNT(ID), '')
INTO #tmpMTLExportTimes
FROM (
    SELECT DISTINCT
        POID
        ,ID
    FROM Export_Detail ed WITH (NOLOCK)
    WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE POID = ed.POID AND POID <> '')
) ed
GROUP BY POID

SELECT
    psd.ID
   ,[Fab ETA] = MAX(IIF(FabricType = 'F', FinalETA, NULL))
   ,[Acc ETA] = MAX(IIF(FabricType = 'A', FinalETA, NULL))
INTO #tmpPSD
FROM PO_Supp_Detail psd WITH (NOLOCK)
WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE POID = psd.ID AND POID <> '')
GROUP BY psd.ID

SELECT
    ed.POID
   ,ArriveWHDate = MAX(e.WhseArrival)
INTO #tmp_ArriveWHDate
FROM Export e WITH (NOLOCK)
INNER JOIN Export_Detail ed WITH (NOLOCK) ON e.ID = ed.ID
WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE POID = ed.POID AND POID <> '')
GROUP BY ed.POID

SELECT
    pld.OrderID
   ,ActPulloutTimes = COUNT(DISTINCT pl.PulloutID)
INTO #tmp_Pullout
FROM PackingList pl WITH (NOLOCK)
INNER JOIN PackingList_Detail pld WITH (NOLOCK) ON pl.ID = pld.ID
WHERE pl.PulloutID <> ''
AND pld.ShipQty > 0
AND EXISTS (SELECT 1 FROM #tmpOrders WHERE ID = pld.OrderID)
GROUP BY pld.OrderID
";
            #endregion

            #region step 6 判斷各種狀況, 輸出時不同來源欄位
            string biColunn = string.Empty;
            string dest = string.Empty;
            if (model.IsPowerBI)
            {
                sqlcmd += @"
SELECT
    s.OrderID
    ,SewQty = SUM(s.QAQty)
    ,SewQtybyRate = SUM(ROUND(s.QAQty * ol.Rate / 100, 2))
INTO #tmpSewingOutput
FROM (
    SELECT
        sd.OrderId
       ,sd.ComboType
       ,QAQty = SUM(sd.QAQty)
    FROM SewingOutput s WITH (NOLOCK)
    INNER JOIN SewingOutput_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
    WHERE EXISTS (SELECT 1 FROM #tmpOrders WHERE sd.OrderId = ID)
    GROUP BY sd.OrderId, sd.ComboType
) s
INNER JOIN Order_Location ol WITH (NOLOCK) ON ol.OrderID = s.OrderID AND ol.Location = s.ComboType
GROUP BY s.OrderID
";
                biColunn = @"
    --以下是BI需要
   ,[SCHDL/ETA(SP)] = o.Max_ScheETAbySP
   ,[SewingMtlETA(SPexclRepl)] = o.Sew_ScheETAnoReplace
   ,[ActualMtlETA(exclRepl)] = o.MaxShipETA_Exclude5x
   ,[HalfKey] = IIF(CAST(FORMAT(DATEADD(DAY, -7, o.SciDelivery), 'dd') AS INT) BETWEEN 1 AND 15
        , FORMAT(DATEADD(DAY, -7, o.SciDelivery), 'yyyyMM01')
        , FORMAT(DATEADD(DAY, -7, o.SciDelivery), 'yyyyMM02'))
   ,[Buyerhalfkey] = IIF(CAST(FORMAT(DATEADD(DAY, -7, o.BuyerDelivery), 'dd') AS INT) BETWEEN 1 AND 15
        , FORMAT(DATEADD(DAY, -7, o.BuyerDelivery), 'yyyyMM01')
        , FORMAT(DATEADD(DAY, -7, o.BuyerDelivery), 'yyyyMM02'))
   ,[DevSample] = IIF((SELECT IsDevSample FROM OrderType ot WITH (NOLOCK) WHERE ot.ID = o.OrderTypeID AND ot.BrandID = o.BrandID) = 1, 'Y', '')
   ,[POID] = o.POID
   ,[KeepPanels] = IIF(o.KeepPanels = 0, 'N', 'Y')
   ,[BuyBackReason] = o.BuyBackReason
   ,[SewQtybyRate] = IIF(o.StyleUnit = 'PCS' ,(SELECT SewQty FROM #tmpSewingOutput WHERE OrderID = o.ID), (SELECT SewQtybyRate FROM #tmpSewingOutput WHERE OrderID = o.ID))
   ,[Unit] = o.StyleUnit
   ,[SubconInType] = IIF(o.SubconInType = '1', 'Subcon-in from sister factory (same M division)'
        , IIF(o.SubconInType = '2', 'Subcon-in from sister factory (different M division)'
        , IIF(o.SubconInType = '3', 'Subcon-in from non-sister factory', '')))
   ,[Article] = #tmp_Article.Article
   ,[ProduceRgPMS] = (SELECT ProduceRgCode FROM SCIFty WITH (NOLOCK) WHERE ID = o.FactoryID)
   ,[Third_Party_Insepction] = o.CFAIs3rdInspect
   ,[ColorID] = #tmpColorCombo.ColorID
   ,[NeedProduction] =
        CASE
            WHEN o.Junk = 1 THEN CASE
                    WHEN o.NeedProduction = 1 THEN 'Y'
                    WHEN o.KeepPanels = 1 THEN 'K'
                    ELSE 'N'
                END
            ELSE ''
        END
   ,[SewQtyTop] = SewQtyTop
   ,[SewQtyBottom] = SewQtyBottom
   ,[SewQtyInner] = SewQtyInner
   ,[SewQtyOuter] = SewQtyOuter
   ,[Dest] = o.Dest
   ,[Country] = Country.Alias
    ,HaulingDate         = pld.HaulingDate
    ,HaulingStatus       = pld.HaulingStatus
    ,HaulingFailQty      = ISNULL( pld.HaulingFailQty ,0)
    ,PackingAuditDate    = pld.PackingAuditDate
    ,PackingAuditStatus  = pld.PackingAuditStatus
    ,PackingAuditFailQty = ISNULL( pld.PackingAuditFailQty ,0)
    ,M360MDScanDate      = pld.M360MDScanDate
    ,M360MDStatus        = pld.M360MDStatus
    ,M360MDFailQty       = ISNULL( pld.M360MDFailQty ,0)
    ,HangerPackScanTime  = pld.HangerPackScanTime
    ,HangerPackStatus    = pld.HangerPackStatus
    ,HangerPackFailQty   = ISNULL( pld.HangerPackFailQty ,0)
    ,JokerTagScanTime    = pld.JokerTagScanTime
    ,JokerTagStatus      = pld.JokerTagStatus
    ,JokerTagFailQty     = ISNULL( pld.JokerTagFailQty ,0)
    ,HeatSealScanTime    = pld.HeatSealScanTime
    ,HeatSealStatus      = pld.HeatSealStatus
    ,HeatSealFailQty     = ISNULL( pld.HeatSealFailQty ,0)
    ,o.JokerTag
    ,o.HeatSeal
    ,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    ,[BIInsertDate] = GETDATE()
";
            }
            else
            {
                // 報表顯示[Dest] 和 匯入BI的值不一樣, BI 是寫入 Country.ID
                dest = ",[Dest] = Country.Alias";
            }

            string ttl_tms = string.Empty;
            if (!(model.IncludeArtworkData || model.IncludeArtworkDataKindIsPAP))
            {
                ttl_tms = ",[TTL_TMS] = o.QTY * o.CPU * (select StdTMS from System WITH (NOLOCK))";
            }

            string seq = string.Empty;
            string pdseq = string.Empty;
            string pdwhere = string.Empty;
            if (model.SeparateByQtyBdownByShipmode)
            {
                seq = "o.Seq,";
                pdseq = ", pd.OrderShipmodeSeq";
                pdwhere = " and pd.OrderShipmodeSeq = o.seq";
            }
            #endregion

            #region step 7 輸出, 要按照Excel欄位順序, 欄位名稱是一起輸出的
            sqlcmd += $@"
SELECT
    {seq}
    [M] = o.MDivisionID
   ,[Factory] = o.FactoryID
   ,[Delivery] = o.BuyerDelivery
   ,[Delivery(YYYYMM)] = FORMAT(o.BuyerDelivery, 'yyyyMM')
   ,[Earliest SCIDlv] = #tmp_EarliestSCIDlv.EarliestSCIDlv
   ,[SCIDlv] = o.SciDelivery
   ,[KEY] = FORMAT(IIF(DAY(SciDelivery) <= 7, DATEADD(MONTH, -1, SciDelivery), SciDelivery), 'yyyyMM')
   ,[IDD] = o.IDD-- #tmpIDD.IDD
   ,[CRD] = o.CRDDate
   ,[CRD(YYYYMM)] = FORMAT(o.CRDDate, 'yyyyMM')
   ,[Check CRD] = IIF(ISNULL(o.BuyerDelivery, '') <> ISNULL(o.CRDDate, ''), 'Y', '')
   ,[OrdCFM] = o.CFMDate
   ,[CRD-OrdCFM] = ISNULL(DATEDIFF(DAY, o.CFMDate, CRDDate), 0)
   ,[SPNO] = o.ID
   ,[3rd Party Insepction] = IIF(o.CFAIs3rdInspect > 0, 'Y', 'N')
   ,[Category] =
        CASE o.Category
            WHEN 'B' THEN 'Bulk'
            WHEN 'G' THEN 'Garment'
            WHEN 'M' THEN 'Material'
            WHEN 'S' THEN 'Sample'
            WHEN 'T' THEN 'Sample mtl.'
            WHEN '' THEN
                CASE o.ForecastSampleGroup
                    WHEN '' THEN 'Bulk fc'
                    WHEN 'D' THEN 'Dev. sample fc'
                    WHEN 'S' THEN 'Sa. sample fc'
                END
        END
   ,[Est. download date] = IIF(o.isForecast = 0, '', o.BuyMonth)--和[Buy Month]相反
   ,[Buy Back] = IIF(EXISTS (SELECT 1 FROM Order_BuyBack WITH (NOLOCK) WHERE ID = o.ID), 'Y', '')
   ,[Cancelled] = IIF(o.Junk = 1, 'Y', '')
   ,[Cancel still need to continue production] =
        CASE
            WHEN o.Junk = 1 THEN
                CASE
                    WHEN o.NeedProduction = 1 THEN 'Y'
                    WHEN o.KeepPanels = 1 THEN 'K'
                    ELSE 'N'
                END
            ELSE ''
        END
    {dest}
   ,[Style] = o.StyleID
   ,[Style Name] = s.StyleName
   ,[Modular Parent] = s.ModularParent
   ,[CPU Adjusted %] = ISNULL(s.CPUAdjusted * 100, 0)
   ,[Similar Style] = #tmp_StyleUkey.GetStyleUkey
   ,[Season] = o.SeasonID
   ,[Garment L/T] = ISNULL(#tmpGMTLT.[Garment L/T], 0)
   ,[Order Type] = o.OrderTypeID
   ,[Project] = o.ProjectID
   ,[Tech Concept] = d4.Description
   ,[PackingMethod] = d1.Name
   ,[Hanger pack] = o.HangerPack
   ,[Joker Tag] = o.JokerTag
   ,[Heat Seal] = o.HeatSeal
   ,[Order#] = o.Customize1
   ,[Buy Month] = IIF(o.isForecast = 0, o.BuyMonth, '')--和[Est. download date]相反
   ,[PONO] = o.CustPONo
   ,[Original CustPO] = o.Customize4
   ,[Line Aggregator] = o.Customize5
   ,[VAS/SHAS] = IIF(o.VasShas = 1, 'Y', '')
   ,[VAS/SHAS Apv.] = o.MnorderApv2
   ,[VAS/SHAS Cut Off Date] = FORMAT(DATEADD(DAY, -30, GetMinDate.minDate), 'yyyy/MM/dd')
   ,[M/Notice Date] = o.MnorderApv
   ,[Est M/Notice Apv.] = o.KPIMNotice
   ,[Tissue] = IIF(o.TissuePaper = 1, 'Y', '')
   ,[AF by adidas] = IIF(o.AirFreightByBrand = '1', 'Y', '')
   ,[Factory Disclaimer] = d3.Name
   ,[Factory Disclaimer Remark] = s.ExpectionFormRemark
   ,[Approved/Rejected Date] = s.ExpectionFormDate
   ,[Global Foundation Range] = IIF(o.GFR = 1, 'Y', '')
   ,[Brand] = o.BrandID
   ,[Cust CD] = o.CustCDID
   ,[KIT] = CustCD.Kit
   ,[Fty Code] = o.BrandFTYCode
   ,[Program] = o.ProgramID
   ,[Non Revenue] = IIF(o.NonRevenue = 1, 'Y', 'N')
   ,[New CD Code] = s.CDCodeNew
   ,[ProductType] = r2.Name
   ,[FabricType] = r1.Name
   ,[Lining] = s.Lining
   ,[Gender] = s.Gender
   ,[Construction] = d2.Name
   ,[Cpu] = o.CPU
   ,[Qty] = O.Qty
   ,[FOC Qty] = o.FOCQty
   ,[Total CPU] = o.CPU * o.Qty * o.CPUFactor
   ,[Shortage] = iif(o.GMTComplete ='S',o.Qty - GetPulloutData.Qty,0)
   ,[Sew_Qty -- TOP] = ISNULL(#tmp_sewDetial.SewQtyTop, 0)
   ,[Sew_Qty -- Bottom] = ISNULL(#tmp_sewDetial.SewQtyBottom, 0)
   ,[Sew_Qty -- Inner] = ISNULL(#tmp_sewDetial.SewQtyInner, 0)
   ,[Sew_Qty -- Outer] = ISNULL(#tmp_sewDetial.SewQtyOuter, 0)
   ,[Total Sewing Output] = ISNULL(#tmp_TtlSewQty.[Total Sewing Output], 0)
   ,[Cut Qty] = ISNULL(#tmpCutQty.CutQty, 0)
   ,[By Comb] = IIF(ct.WorkType = '1', 'Y', '')
   ,[Cutting Status] = IIF(#tmpCutQty.CutQty >= o.Qty, 'Y', '')
   ,[Packing Qty] = ISNULL(pld.PackingQty, 0)
   ,[Packing FOC Qty] = ISNULL(pld.PackingFOCQty, 0)
   ,[Booking Qty] = ISNULL(pld.BookingQty, 0)
   ,[FOC Adj Qty] = ISNULL(i.FOCAdjQty, 0)
   ,[Not FOC Adj Qty] = ISNULL(i.InvoiceAdjQty, 0) - ISNULL(i.FOCAdjQty, 0)
   ,[FOB] = o.PoPrice
   ,[Total] = o.Qty * o.PoPrice
   ,[KPI L/ETA] = o.KPILETA
   ,[PF ETA (SP)] = o.PFETA
   ,[Pull Forward Remark] = #tmp_PFRemark.Remark
   ,[Pack L/ETA] = o.PackLETA
   ,[SCHD L/ETA] = o.LETA
   ,[Actual Mtl. ETA] = o.MTLETA
   ,[Fab ETA] = #tmpPSD.[Fab ETA]
   ,[Acc ETA] = #tmpPSD.[Acc ETA]
   ,[Sewing Mtl Complt(SP)] = #tmpComplt.SewingMtlComplt
   ,[Packing Mtl Complt(SP)] = #tmpComplt.PackingMtlComplt
   ,[Sew. MTL ETA (SP)] = o.SewETA
   ,[Pkg. MTL ETA (SP)] = o.PackETA
   ,[MTL Delay] = IIF(#tmp_MTLDelay.MTLDelay = 1, 'Y', '')
   ,[MTL Cmplt] = IIF(o.MTLExport = '', #tmpMTLExportTimes.MTLExportTimes, IIF(o.MTLExport = 'OK', 'Y', o.MTLExport))
   ,[MTL Cmplt (SP)] = IIF(o.MTLComplete = 1, 'Y', 'N')
   ,[Arrive W/H Date] = #tmp_ArriveWHDate.ArriveWHDate
   ,[Sewing InLine] = o.SewInLine
   ,[Sewing OffLine] = o.SewOffLine
   ,[1st Sewn Date] = #tmp_sewDetial.FirstOutDate
   ,[Last Sewn Date] = #tmp_sewDetial.LastOutDate
   ,[First Production Date] = o.FirstProduction
   ,[Last Production Date] = o.LastProductionDate
   ,[Each Cons Apv Date] = o.EachConsApv
   ,[Est Each Con Apv.] = o.KpiEachConsCheck
   ,[Cutting InLine] = o.CutInLine
   ,[Cutting OffLine] = o.CutOffLine
   ,[Cutting InLine(SP)] = #tmpEstCutDate.InLine
   ,[Cutting OffLine(SP)] = #tmpEstCutDate.OffLine
   ,[1st Cut Date] = ct.FirstCutDate
   ,[Last Cut Date] = ct.LastCutDate
   ,[Est. Pullout] = o.EstPulloutDate
   ,[Act. Pullout Date] = pld.ActPulloutDate
   ,[Pullout Qty] = ISNULL(pld.PulloutQty, 0)
   ,[Act. Pullout Times] = ISNULL(#tmp_Pullout.ActPulloutTimes, 0)
   ,[Act. Pullout Cmplt] = IIF(o.PulloutComplete = 1, 'OK', '')
   ,[KPI Delivery Date] = o.FtyKPI
   ,[Update Delivery Reason] = IIF(ISNULL(o.KPIChangeReason, '') = '', '', CONCAT(o.KPIChangeReason, '-', r3.Name))
   ,[Plan Date] = o.PlanDate
   ,[Original Buyer Delivery Date] = o.OrigBuyerDelivery
   ,[SMR] = o.SMR
   ,[SMR Name] = tp1.Name
   ,[Handle] = o.MRHandle
   ,[Handle Name] = tp2.Name
   ,[Posmr] = PO.POSMR
   ,[Posmr Name] = tp3.Name
   ,[PoHandle] = PO.POHandle
   ,[PoHandle Name] = tp4.Name
   ,[PCHandle] = PO.PCHandle
   ,[PCHandle Name] = tp5.Name
   ,[MCHandle] = o.MCHandle
   ,[MCHandle Name] = p1.Name
   ,[LocalMR] = o.LocalMR
   ,[LocalMR Name] = p2.Name
   ,[DoxType] = o.DoxType
   ,[Packing CTN] = ISNULL(pld.PackingCTN, 0)
   ,[TTLCTN] = ISNULL(pld.TotalCTN, 0)
   ,[Remain carton] = isnull(PackedCarton.sumCtnQty - PackedCarton.sumPackedCartons, 0)
   ,[Pack Error CTN] = isnull(pld.PackErrCTN, 0)
   ,[FtyCTN] = ISNULL(pld.FtyCtn_Remaining, 0)--和 UpdateOrdersCTN 中的 FtyCtn 不一樣喔, 這是目前在工廠的剩餘紙箱數量
   ,[Fty To Clog Transit] = ISNULL(pld.FtyToClogTransit, 0)
   ,[cLog CTN] = ISNULL(pld.ClogCTN, 0)
   ,[Clog To CFA Tansit] = ISNULL(pld.ClogToCFATansit, 0)
   ,[CFA CTN] = Isnull(pld.CFACTN, 0)
   ,[CFA To Clog Transit] = Isnull(pld.CFAToClogTransit, 0)
   ,[cLog Rec. Date] = pld.ClogLastReceiveDate
   ,[Final Insp. Date] = o.CFAFinalInspectDate
   ,[Insp. Result] = o.CFAFinalInspectResult
   ,[CFA Name] = o.CFAFinalInspectHandle
   ,[Sewing Line#] = o.SewLine
   ,[ShipMode] = o.ShipmodeID
   ,[SI#] = o.Customize2
   ,[ColorWay] = #tmp_Article.Article
   ,[Color] = #tmpColorCombo.ColorID
   ,[Special Mark] = ssm.Name
   ,[Fty Remark] = s.FTYRemark
   ,[Sample Reason] = r4.Name
   ,[IS MixMarker] =
        CASE o.IsMixMarker
            WHEN 0 THEN 'Is Single Marker'
            WHEN 1 THEN 'Is Mix  Marker'
            WHEN 2 THEN ' Is Mix Marker - SCI'
            ELSE ''
        END
   ,[Cutting SP] = o.CuttingSP
   ,[Rainwear test] = IIF(o.RainwearTestPassed = 1, 'Y', '')
   ,[TMS] = o.CPU * (SELECT StdTMS FROM System WITH (NOLOCK))
    
    --這6個時間欄位,當紙箱全部從工廠轉出才顯示時間
   ,[MD room scan date] = IIF(ISNULL(pld.FtyCtn_Remaining, 0) = 0, o.MdRoomScanDate, NULL)
   ,[Dry Room received date] = IIF(ISNULL(pld.FtyCtn_Remaining, 0) = 0, o.DryRoomRecdDate, NULL)
   ,[Dry room trans date] = IIF(ISNULL(pld.FtyCtn_Remaining, 0) = 0, o.DryRoomTransDate, NULL)
   ,[Last ctn trans date] = IIF(ISNULL(pld.FtyCtn_Remaining, 0) = 0, o.LastCTNTransDate, NULL)
   ,[Last scan and pack date] = IIF(ISNULL(pld.FtyCtn_Remaining, 0) = 0, pld.ScanEditDate, NULL)
   ,[Last ctn recvd date] = IIF(ISNULL(pld.FtyCtn_Remaining, 0) = 0, o.LastCTNRecdDate, NULL)

   ,[Organic Cotton/Recycle Polyester/Recycle Nylon] = IIF(o.OrganicCotton = 1, 'Y', 'N')
   ,[Direct Ship] = IIF(o.DirectShip = 1, 'V', '')
   ,[Style-Carryover] = IIF(s.NewCO = '2', 'V', '')
   {ttl_tms}
   {biColunn}
FROM #tmpOrders o
LEFT JOIN Style s WITH (NOLOCK) ON o.StyleUkey = s.Ukey
LEFT JOIN CustCD WITH (NOLOCK) ON o.CustCDID = CustCD.ID AND o.BrandID = CustCD.BrandID
LEFT JOIN Country WITH (NOLOCK) ON o.Dest = Country.ID
LEFT JOIN Cutting ct WITH (NOLOCK) ON o.CuttingSP = ct.ID
LEFT JOIN Style_SpecialMark ssm WITH (NOLOCK) ON s.SpecialMark = ssm.ID AND o.BrandID = ssm.BrandID AND ssm.Junk = 0
OUTER APPLY (
    SELECT minDate = MIN(Date)
    FROM (VALUES (o.BuyerDelivery), (o.CRDDate), (o.PlanDate), (o.OrigBuyerDelivery)) AS tmp (Date)
    WHERE tmp.Date IS NOT NULL
) GetMinDate
LEFT JOIN PO WITH (NOLOCK) ON PO.ID = o.POID
LEFT JOIN TPEPass1 tp1 WITH (NOLOCK) ON o.SMR = tp1.ID
LEFT JOIN TPEPass1 tp2 WITH (NOLOCK) ON o.MRHandle = tp2.ID
LEFT JOIN TPEPass1 tp3 WITH (NOLOCK) ON PO.POSMR = tp3.ID
LEFT JOIN TPEPass1 tp4 WITH (NOLOCK) ON PO.POHandle = tp4.ID
LEFT JOIN TPEPass1 tp5 WITH (NOLOCK) ON PO.PCHandle = tp5.ID
LEFT JOIN Pass1 p1 WITH (NOLOCK) ON o.MCHandle = p1.ID
LEFT JOIN Pass1 p2 WITH (NOLOCK) ON o.LocalMR = p2.ID
outer apply (
  select Qty=sum(pd.ShipQty)
  from PackingList p with (nolock), PackingList_Detail pd with (nolock)
  where p.ID = pd.ID
  and p.PulloutID <> ''
  and pd.OrderID = o.ID
) GetPulloutData 

outer apply(
select sumCtnQty = sum(pd.CtnQty) , sumPackedCartons = sum(case when pd.ScanQty > 0 and pd.CTNQty > 0 then 1 else 0 end)
from PackingList p with (nolock), 
PackingList_Detail pd with (nolock)
where p.ID = pd.ID
and pd.OrderID = o.ID
{pdwhere}
Group by pd.OrderID {pdseq}
) PackedCarton

LEFT JOIN DropDownList d1 WITH (NOLOCK) ON d1.Type = 'PackingMethod' AND o.CtnType = d1.ID
LEFT JOIN DropDownList d2 WITH (NOLOCK) ON d2.type = 'StyleConstruction' AND s.Construction = d2.ID
LEFT JOIN DropDownList d3 WITH (NOLOCK) ON d3.Type = 'FactoryDisclaimer' AND s.ExpectionFormStatus = d3.ID
LEFT JOIN DropDownList d4 WITH (NOLOCK) ON d4.ID = s.TechConceptID AND d4.Type = 'TechConcept'
LEFT JOIN Reason r1 WITH (NOLOCK) ON r1.ReasonTypeID = 'Fabric_Kind' AND s.FabricType = r1.ID
LEFT JOIN Reason r2 WITH (NOLOCK) ON r2.ReasonTypeID = 'Style_Apparel_Type' AND s.ApparelType = r2.ID
LEFT JOIN Reason r3 WITH (NOLOCK) ON r3.ReasonTypeID = 'Order_BuyerDelivery' AND o.KPIChangeReason = r3.ID
LEFT JOIN Reason r4 WITH (NOLOCK) ON r4.ReasonTypeID = 'Order_reMakeSample' AND o.SampleReason = r4.ID

LEFT JOIN #tmp_StyleUkey ON o.StyleUkey = #tmp_StyleUkey.StyleUkey
LEFT JOIN #tmpGMTLT ON o.ID = #tmpGMTLT.ID
LEFT JOIN #tmpComplt ON o.ID = #tmpComplt.OrderID
LEFT JOIN #tmp_sewDetial ON o.ID = #tmp_sewDetial.OrderID
LEFT JOIN #tmp_TtlSewQty ON o.ID = #tmp_TtlSewQty.ID
LEFT JOIN #tmpCutQty ON o.ID = #tmpCutQty.OrderID
LEFT JOIN #tmp_PFRemark ON o.ID = #tmp_PFRemark.ID
LEFT JOIN #tmpEstCutDate ON o.ID = #tmpEstCutDate.OrderID
LEFT JOIN #tmp_Pullout ON o.ID = #tmp_Pullout.OrderID
LEFT JOIN #tmp_Article ON o.ID = #tmp_Article.ID
LEFT JOIN #tmpColorCombo ON o.ID = #tmpColorCombo.ID

--暫存表 by POID
LEFT JOIN #tmp_EarliestSCIDlv ON o.POID = #tmp_EarliestSCIDlv.POID
LEFT JOIN #tmp_MTLDelay ON o.POID = #tmp_MTLDelay.POID
LEFT JOIN #tmpMTLExportTimes ON o.POID = #tmpMTLExportTimes.POID
LEFT JOIN #tmpPSD ON o.POID = #tmpPSD.ID
LEFT JOIN #tmp_ArriveWHDate ON o.POID = #tmp_ArriveWHDate.POID

{sqltmp}

ORDER BY IIF(o.POID = '', 'Z', o.POID), o.ID

";

            // Artwork 資訊
            if (model.IncludeArtworkData || model.IncludeArtworkDataKindIsPAP)
            {
                sqlcmd += this.GetArtworkTypeValue(model);
            }

            // drop table
            sqlcmd += @"
DROP TABLE #tmpOrdersBase
, #tmpPOComboStep
, #tmpOqs_Step
, #tmpOrders
, #tmp_StyleUkey

--暫存表 by OrderID
, #tmpGMTLT
, #tmpComplt
, #tmp_sewDetial
, #tmp_TtlSewQty
, #tmpCutQty
, #tmp_PFRemark
, #tmpEstCutDate
, #tmp_Pullout
, #tmp_Article
, #tmpOrder_ColorCombo
, #tmpColorCombo

, #tmp_PackingList_Detail
, #tmp_invAdj

--暫存表 by POID
, #tmp_EarliestSCIDlv
, #tmp_MTLDelay
, #tmpMTLExportTimes
, #tmpPSD
, #tmp_ArriveWHDate
";
            #endregion

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
        /// 結果:3個欄位 OrderID, ColumnName(顯示欄位名稱), ColumnValue(對應值)
        /// 使用1.BI 直接寫入 P_PPICMasterList_Extend
        /// 使用2.R03 樞紐後, 在報表最右方
        /// </summary>
        private string GetArtworkTypeValue(PPIC_R03_ViewModel model)
        {
            int byCPUsqlbit = model.ByCPU ? 1 : 0;

            #region step 1 條件 Classify  1.Include Artwork data ||  2.'PAP'
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

            string seq = model.SeparateByQtyBdownByShipmode ? ",Seq" : string.Empty;
            #endregion

            #region step 2 準備 有or無 勾選 Printing Detail, 顯示欄位 [Printing LT] , [InkType/Color/Size], 皆為 varchar 欄位
            string sql_printingDetail_BaseUnion = string.Empty;
            string sql_printingDetail_Select = string.Empty;
            string sql_printingDetail_OutputUnion = string.Empty;
            if (model.PrintingDetail)
            {
                // 勾選 Printing Detail, 欄位 [Printing LT] , [InkType/Color/Size] 的值
                sql_printingDetail_BaseUnion = @"
UNION ALL
SELECT
    ID = 'PRINTING'
    ,FakeID = ''
    ,ColumnN = 'Printing LT'
    ,ColumnSeq = -1 
    ,''
UNION ALL
SELECT
    ID = 'PRINTING'
    ,FakeID = ''
    ,ColumnN = 'InkType/Color/Size'
    ,ColumnSeq = 0 
    ,''
";
                sql_printingDetail_Select = $@"
SELECT DISTINCT
    oa.ID
   ,InkTypeColorSize = CONCAT(oa.InkType, '/', oa.Colors, ' ', '/', IIF(s.SmallLogo = 1, 'Small logo', 'Big logo'))
   ,PrintingLT = CAST(plt.LeadTime + plt.AddLeadTime AS FLOAT)
INTO #tmpOrder_Artwork
FROM Order_Artwork oa WITH (NOLOCK)
INNER JOIN Orders o WITH (NOLOCK) ON o.ID = oa.ID
OUTER APPLY (SELECT SmallLogo = IIF(EXISTS(SELECT 1 FROM System WITH (NOLOCK) WHERE SmallLogoCM <= oa.Width OR SmallLogoCM <= oa.Length), 0, 1)) s
OUTER APPLY (SELECT tmpRTL = IIF(o.Cpu = 0, 0, s.SewlineAvgCPU / o.Cpu) FROM System s WITH (NOLOCK)) tr
OUTER APPLY (SELECT RTLQty = IIF(o.Qty < tmpRTL, o.Qty, tmpRTL)) r
OUTER APPLY (SELECT Colors = IIF(oa.Colors = '', 0, oa.Colors)) c
OUTER APPLY (SELECT ex = IIF(EXISTS (SELECT 1 FROM PrintLeadTime WITH (NOLOCK) WHERE InkType = oa.InkType), 1, 0)) e
OUTER APPLY (
    SELECT LeadTime, AddLeadTime
    FROM PrintLeadTime plt WITH (NOLOCK)
    WHERE plt.InkType = oa.InkType
    AND plt.SmallLogo = s.SmallLogo
    AND r.RTLQty BETWEEN plt.RTLQtyLowerBound AND plt.RTLQtyUpperBound
    AND c.Colors BETWEEN plt.ColorsLowerBound AND plt.ColorsUpperBound
) pEx
OUTER APPLY (
    SELECT LeadTime, AddLeadTime
    FROM PrintLeadTime plt WITH (NOLOCK)
    WHERE plt.SmallLogo = s.SmallLogo
    AND plt.IsDefault = 1
    AND r.RTLQty BETWEEN plt.RTLQtyLowerBound AND plt.RTLQtyUpperBound
    AND c.Colors BETWEEN plt.ColorsLowerBound AND plt.ColorsUpperBound
) pNEx
OUTER APPLY (
    SELECT
        LeadTime = IIF(e.ex = 1, pEx.LeadTime, pnEx.LeadTime)
       ,AddLeadTime = IIF(e.ex = 1, pEx.AddLeadTime, pnEx.AddLeadTime)
) plt
WHERE oa.ArtworkTypeID = 'Printing'
AND oa.ID IN (SELECT ID FROM #tmpOrders)

SELECT DISTINCT-- 沒有 by Order_QtyShip.Seq
    t.ID
   {seq}
   ,a.PrintingLT
   ,b.InkTypeColorSize
INTO #tmpPrintingValue
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
                sql_printingDetail_OutputUnion = $@"
UNION ALL
SELECT ID {seq}, [ColumnName] = 'Printing LT', [ColumnValue] = PrintingLT
FROM #tmpPrintingValue
UNION ALL
SELECT ID {seq}, [ColumnName] = 'InkType/Color/Size', [ColumnValue] = InkTypeColorSize
FROM #tmpPrintingValue";
            }
            #endregion

            #region step 3 處理顯示的欄位名稱 & 不同 ArtworkTypeID 不同計算規則
            string strColumnN1 = string.Empty;
            string strColumnN2 = string.Empty;
            string strcolArtworkType = string.Empty;

            if (!model.IsPowerBI)
            {
                strColumnN1 = "#tmpSubProcess.colArtworkType + ColumnN";
                strColumnN2 = "ColumnN = CASE WHEN colArtworkType IS NOT NULL AND colArtworkType <> '' THEN colArtworkType + 'TTL_' + ColumnN ELSE 'TTL_' + ColumnN END";
                strcolArtworkType = "ArtworkType.seq + '-' FROM ArtworkType WITH (NOLOCK) WHERE Junk <> 1 and ID = 'EMBROIDERY'";
            }
            else
            {
                strColumnN1 = "ColumnN";
                strColumnN2 = "'TTL_' + ColumnN";
                strcolArtworkType = "''";
            }

            // 處理顯示的欄位名稱
            string sqlcmd = $@"
{sql_printingDetail_Select}

SELECT
    ID
    ,FakeID = Seq + 'U1'
    ,ColumnN = RTRIM(ID) + ' (' + ArtworkUnit + ')'
    ,ColumnSeq = '1'
    ,colArtworkType = ArtworkType.seq + '-'
INTO #tmpArtworkType
FROM ArtworkType WITH (NOLOCK)
WHERE ArtworkUnit <> '' and Junk <> 1
AND Classify IN ({whereClassify})

UNION ALL
SELECT
    ID
    ,FakeID = Seq + 'U2'
    ,ColumnN = RTRIM(ID) + ' (' + IIF(ProductionUnit = 'QTY', 'Price', p.PUnit) + ')'
    ,ColumnSeq = '2'
    ,colArtworkType = ArtworkType.seq + '-'
FROM ArtworkType WITH (NOLOCK)
OUTER APPLY (SELECT PUnit = IIF({byCPUsqlbit} = 1 AND ProductionUnit = 'TMS', 'CPU', ProductionUnit)) p
WHERE ProductionUnit <> '' and Junk <> 1
AND Classify IN ({whereClassify})
AND ID <> 'PRINTING PPU'

UNION ALL
SELECT
    ID
    ,FakeID = Seq + 'N'
    ,ColumnN = RTRIM(ID)
    ,ColumnSeq = '3'
    ,colArtworkType = ArtworkType.seq + '-'
FROM ArtworkType WITH (NOLOCK)
WHERE ArtworkUnit = '' and Junk <> 1
AND ProductionUnit = ''
AND Classify IN ({whereClassify})

{sql_printingDetail_BaseUnion}

UNION ALL
SELECT
    ID = 'EMBROIDERY'
    ,FakeID = '9999ZZ'
    ,ColumnN = 'EMBROIDERY(SubCon)'
    ,ColumnSeq = '996'
    ,colArtworkType = {strcolArtworkType}
UNION ALL
SELECT
    ID = 'EMBROIDERY'
    ,FakeID = '9999ZZ'
    ,ColumnN = 'EMBROIDERY(POSubcon)'
    ,ColumnSeq = '997'
    ,colArtworkType = {strcolArtworkType}
UNION ALL
SELECT
    ID = 'PrintSubCon'
    ,FakeID = '9999ZZ'
    ,ColumnN = 'POSubCon'
    ,ColumnSeq = '998'
    ,colArtworkType = ''
UNION ALL
SELECT
    ID = 'PrintSubCon'
    ,FakeID = '9999ZZ'
    ,ColumnN = 'SubCon'
    ,ColumnSeq = '999'
    ,colArtworkType = ''

SELECT *, rno = (ROW_NUMBER() OVER (ORDER BY a.ID, a.ColumnSeq))
INTO #tmpSubProcess
FROM #tmpArtworkType a

SELECT
    ID
   ,FakeID
   ,ColumnN
   ,rno = (ROW_NUMBER() OVER (ORDER BY a.rno))
INTO #tmpArtworkData
FROM (
    SELECT 
    ID,
    FakeID,
    ColumnN = {strColumnN1},
    ColumnSeq,
    rno
    FROM #tmpSubProcess WITH (NOLOCK)
    
    UNION ALL
    SELECT
        ID = 'TTLTMS'
       ,FakeID = 'TTLTMS'
       ,ColumnN = 'TTL_TMS'
       ,ColumnSeq = ''
       ,rno = '999'

    UNION ALL
    SELECT
        ID = 'TTL' + ID
        ,FakeID = 'T' + FakeID
        ,{strColumnN2}
        ,ColumnSeq
        ,rno = (ROW_NUMBER() OVER (ORDER BY ID, ColumnSeq)) + 1000--在 TTL_TMS 999 之後
    FROM #tmpSubProcess WITH (NOLOCK)
    --排除文字資訊, 不需要TTL_
    WHERE ID <> 'PrintSubCon'
    AND ColumnN <> 'EMBROIDERY(POSubcon)'
    AND ColumnN <> 'EMBROIDERY(SubCon)'
    --排除額外取得資訊欄位, 不需要TTL_
    AND ColumnN <> 'Printing LT'--數值
    AND ColumnN <> 'InkType/color/size'
    --此處條件不知為啥沒有排除'EMBROIDERY(SubCon)','EMBROIDERY(POSubcon)'
) a
";

            // 不同 ArtworkTypeID 不同計算規則
            sqlcmd += $@"
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

--彙整 ColumnN 和計算 Value
SELECT
    a.ID
    {seq}
    ,[ColumnN] = AUnitRno.ColumnN
    ,[Val] = CAST(
        CASE
            WHEN a.AUnitRno IS NOT NULL THEN a.Qty
            ELSE NULL
        END AS VARCHAR(100))
INTO #tmp_ArtworkTypeValue
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData AUnitRno ON a.AUnitRno = AUnitRno.rno
INNER JOIN #tmpOrders b ON a.ID = b.ID
WHERE a.ArtworkTypeID <> 'PRINTING PPU'

UNION ALL
SELECT
    a.ID
    {seq}
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
INNER JOIN #tmpOrders b ON a.ID = b.ID

UNION ALL
SELECT
    a.ID
    {seq}
    ,[ColumnN] = NRno.ColumnN
    ,[Val] = CAST(
        CASE
        WHEN a.NRno IS NOT NULL THEN a.Qty
        ELSE NULL
        END AS VARCHAR(100))
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData NRno ON a.NRno = NRno.rno
INNER JOIN #tmpOrders b ON a.ID = b.ID

UNION ALL
 --有 by Order_QtyShip.Seq
SELECT
    a.ID
    {seq}
    ,[ColumnN] = TAUnitRno.ColumnN
    ,[Val] = CAST(
        CASE
        WHEN a.TAUnitRno IS NOT NULL THEN b.Qty * a.Qty
        ELSE NULL
        END AS VARCHAR(100))
FROM #tmp_LastArtworkType a
INNER JOIN #tmpArtworkData TAUnitRno ON a.TAUnitRno = TAUnitRno.rno
INNER JOIN #tmpOrders b ON a.ID = b.ID
WHERE a.ArtworkTypeID <> 'PRINTING PPU'

UNION ALL
 --有 by Order_QtyShip.Seq
SELECT
    a.ID
    {seq}
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
    {seq}
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
    {seq}
    ,[ColumnN] = 'POSubCon'
    ,[Val] = a.PoSupp
FROM #tmp_LastArtworkType a
INNER JOIN #tmpOrders b ON a.ID = b.ID
WHERE ISNULL(a.PoSupp, '') <> ''

UNION ALL
SELECT
    a.ID
    {seq}
    ,[ColumnN] = 'SubCon'
    ,[Val] = a.Supp
FROM #tmp_LastArtworkType a
INNER JOIN #tmpOrders b ON a.ID = b.ID
WHERE ISNULL(a.Supp, '') <> ''

UNION ALL
SELECT
    a.ID
    {seq}
    ,[ColumnN] = 'EMBROIDERY(POSubcon)'
    ,[Val] = a.EMBROIDERYPOSubcon
FROM #tmp_LastArtworkType a
INNER JOIN #tmpOrders b ON a.ID = b.ID
WHERE ISNULL(a.EMBROIDERYPOSubcon, '') <> ''

UNION ALL
SELECT
    a.ID
    {seq}
    ,[ColumnN] = 'EMBROIDERY(SubCon)'
    ,[Val] = a.EMBROIDERYSubcon
FROM #tmp_LastArtworkType a
INNER JOIN #tmpOrders b ON a.ID = b.ID
WHERE ISNULL(a.EMBROIDERYSubcon, '') <> ''

UNION ALL
 --有 by Order_QtyShip.Seq
SELECT
    b.ID
    {seq}
    ,[ColumnN] = 'TTL_TMS'
    ,[Val] = CAST(b.Qty * b.CPU * (select StdTMS from System WITH (NOLOCK)) AS VARCHAR(100))
FROM #tmpOrders b

--正確狀況不會發散只會By OrdreID, 或By OrdreID,Seq
SELECT
    [OrderID] = t.ID
    {seq}
   ,[ColumnName] = t.ColumnN
   ,[ColumnValue] = t.Val
INTO #tmpArtworkValues
FROM #tmp_ArtworkTypeValue t

{sql_printingDetail_OutputUnion}
";
            #endregion

            #region 輸出 BI 或 PPIC R03
            if (model.IsPowerBI)
            {
                // BI 更新到 P_PPICMasterList_Exten 只會By OrdreID
                // 排除文字型態欄位, BI 的 ColumnValue 是數值欄位
                sqlcmd += @"
SELECT
    [OrderID]
    ,[ColumnName]
    ,[ColumnValue] = ISNULL(TRY_CONVERT(NUMERIC(38, 6), [ColumnValue]), 0)
FROM #tmpArtworkValues
WHERE ColumnName NOT IN (
    'Printing LT'
    ,'InkType/Color/Size'
    ,'EMBROIDERY(SubCon)'
    ,'EMBROIDERY(POSubcon)'
    ,'POSubCon'
    ,'SubCon'
)
";
            }
            else
            {
                // PPIC R03
                sqlcmd += $@"
SELECT * FROM #tmpArtworkValues
SELECT * FROM #tmpArtworkData ORDER BY rno
";
            }
            #endregion

            return sqlcmd;
        }
    }
}
