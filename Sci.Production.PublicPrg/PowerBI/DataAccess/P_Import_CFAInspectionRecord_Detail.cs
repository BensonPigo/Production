using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/QA/R32 已脫鉤 待討論
    /// </summary>
    public class P_Import_CFAInspectionRecord_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_CFAInspectionRecord_Detail(ExecutedList item)
        {
            QA_R32 biModel = new QA_R32();
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.Year.ToString());
            }

            try
            {
                QA_R32_ViewModel qaR32 = new QA_R32_ViewModel()
                {
                    StartAuditDate = item.SDate.Value,
                    EndAuditDate = null,
                    StartBuyerDelivery = null,
                    EndBuyerDelivery = null,
                    StartSP = string.Empty,
                    EndSP = string.Empty,
                    BrandID = string.Empty,
                    Format = 1,
                    Stage = string.Empty,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    IsPowerBI = true,
                    BIFactoryID = item.RgCode,
                };

                Base_ViewModel resultReport = biModel.GetQA_R32_Detail(qaR32);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetCFAInspectionRecord_Detail_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"

-- Step 1: 初步資料過濾
SELECT 
    c.[ID], c.[AuditDate], c.[FactoryID], c.[MDivisionid], c.[SewingLineID],
    c.[Team], c.[Shift], c.[Stage], co.[Carton], c.[InspectQty], c.[DefectQty],
    c.[ClogReceivedPercentage], c.[Result], c.[CFA], c.[Status], c.[Remark],
    c.[AddName], c.[AddDate], c.[EditName], c.[EditDate], c.[IsCombinePO],
    c.[FirstInspection], co.OrderID, co.SEQ
INTO #MainData1
FROM Production.dbo.CFAInspectionRecord c WITH (NOLOCK)
JOIN Production.dbo.CFAInspectionRecord_OrderSEQ co WITH (NOLOCK) ON c.ID = co.ID
JOIN Production.dbo.Orders o WITH (NOLOCK) ON o.ID = co.OrderID
WHERE c.AuditDate >= @sDate

-- Step 2: 進一步展開資料，加入 ReInspection, InspectedSP/Seq
SELECT 
    c.[ID], c.[AuditDate], c.[FactoryID], c.[MDivisionid], c.[SewingLineID],
    c.[Team], c.[Shift], c.[Stage], co.[Carton], c.[InspectQty], c.[DefectQty],
    c.[ClogReceivedPercentage], c.[Result], c.[CFA], c.[Status], c.[Remark],
    c.[AddName], c.[AddDate], c.[EditName], c.[EditDate], c.[IsCombinePO],
    c.[FirstInspection], co.OrderID, co.SEQ,
    [InspectedSP] = cfos.OrderID,
    [InspectedSeq] = cfos.Seq,
    [ReInspection] = IIF(c.ReInspection = 1, 1, 0)
INTO #MainData
FROM Production.dbo.CFAInspectionRecord c WITH (NOLOCK)
JOIN Production.dbo.CFAInspectionRecord_OrderSEQ co WITH (NOLOCK) ON c.ID = co.ID
OUTER APPLY (
    SELECT TOP 1 OrderID, Seq
    FROM Production.dbo.CFAInspectionRecord_OrderSEQ cfos WITH (NOLOCK)
    WHERE c.ID = cfos.ID
    ORDER BY Ukey
) cfos
WHERE c.ID IN (SELECT ID FROM #MainData1)

-- Step 3: 加入明細資料與缺陷資訊
SELECT 
    c.AuditDate,
    o.BuyerDelivery,
    c.OrderID,
    o.CustPoNo,
    o.StyleID,
    o.BrandID,
    o.Dest,
    oqs.Seq,
    c.SewingLineID,
    [VasShas] = IIF(o.VasShas = 1, 'Y', 'N'),
    c.ClogReceivedPercentage,
    c.MDivisionid,
    c.FactoryID,
    c.Shift,
    c.Team,
    oqs.Qty,
    c.Status,
    [Carton] = IIF(c.Carton = '' AND c.Stage = '3rd party', 'N/A', c.Carton),
    [CfA] = ISNULL(CONCAT(c.CFA, ':', p1.Name), ''),
    c.Stage,
    c.Result,
    c.InspectQty,
    c.DefectQty,
    [SQR] = IIF(c.InspectQty = 0, 0, (c.DefectQty * 1.0 / c.InspectQty) * 100),
    [DefectDescription] = g.Description,
    [AreaCodeDesc] = cd.CFAAreaID + ' - ' + ca.Description,
    [NoOfDefect] = cd.Qty,
    cd.Remark,
    c.ID,
    c.IsCombinePO,
    [InsCtn] = IIF(
        c.Stage IN ('Final', 'Final Internal', '3rd party'),
        (
            SELECT COUNT(DISTINCT cr.ID) + 1
            FROM Production.dbo.CFAInspectionRecord cr WITH (NOLOCK)
            JOIN Production.dbo.CFAInspectionRecord_OrderSEQ crd WITH (NOLOCK) ON cr.ID = crd.ID
            WHERE crd.OrderID = c.OrderID AND crd.SEQ = c.SEQ
              AND cr.Status = 'Confirmed'
              AND cr.Stage = c.Stage
              AND cr.AuditDate <= c.AuditDate
              AND cr.ID != c.ID
        ),
        NULL
    ),
    [Action] = cd.Action,
    [CFAInspectionRecord_Detail_Key] = CONCAT(
        c.ID,
        IIF(ISNULL(cd.GarmentDefectCodeID, '') = '', CONCAT(ROW_NUMBER() OVER (ORDER BY c.ID), ''), cd.GarmentDefectCodeID)
    ),
    c.FirstInspection,
    c.[InspectedSP],
    c.[InspectedSeq],
    c.[ReInspection]
INTO #tmp
FROM #MainData c
LEFT JOIN Production.dbo.Orders o WITH (NOLOCK) ON c.OrderID = o.ID
LEFT JOIN Production.dbo.Order_QtyShip oqs WITH (NOLOCK) ON c.OrderID = oqs.ID AND c.SEQ = oqs.Seq
LEFT JOIN Production.dbo.Pass1 p1 WITH (NOLOCK) ON c.CFA = p1.ID
LEFT JOIN Production.dbo.CFAInspectionRecord_Detail cd WITH (NOLOCK) ON c.ID = cd.ID
LEFT JOIN Production.dbo.GarmentDefectCode g WITH (NOLOCK) ON g.ID = cd.GarmentDefectCodeID
LEFT JOIN Production.dbo.CfaArea ca WITH (NOLOCK) ON ca.ID = cd.CFAAreaID

-- Step 4: 取得包裝明細
SELECT pd.*
INTO #PackingList_Detail
FROM Production.dbo.PackingList_Detail pd WITH (NOLOCK)
WHERE EXISTS (
    SELECT 1
    FROM #tmp t
    WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.SEQ
)

-- Step 5: 最終報表輸出
SELECT  
    Action
, AreaCodeDesc
, AuditDate
, BrandID
, BuyerDelivery
, CFA
, ClogReceivedPercentage
,DefectDescription
, DefectQty
, Dest
, FactoryID
, Carton
,[Inspected Ctn] = InspectedCtn.Val
,[Inspected PoQty] = InspectedPoQty.Val
,Stage
, SewingLineID
, MDivisionid
, NoOfDefect
, Qty
, CustPoNo
, Remark
, Result
, InspectQty
, Seq
, Shift
, OrderID
, SQR
, Status
,StyleID
, Team,
[TTL CTN] = TtlCtn.Val
, VasShas
, FirstInspection = IIF(FirstInspection = 1, 'Y', '')
,t.[InspectedSP]
, t.[InspectedSeq]
, t.[ReInspection]
,[BIFactoryID] = @BIFactoryID
,[BIInsertDate] = GETDATE()
FROM #tmp t
OUTER APPLY (
    SELECT [Val] = COUNT(1)
    FROM #PackingList_Detail pd
    WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq 
      AND pd.CTNQty > 0 AND pd.CTNStartNo != ''
) TtlCtn
OUTER APPLY (
    SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
    FROM #PackingList_Detail pd
    WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq
      AND (',' + t.Carton + ',') LIKE ('%,' + pd.CTNStartNo + ',%')
      AND pd.CTNQty = 1
) InspectedCtn
OUTER APPLY (
    SELECT [Val] = SUM(pd.ShipQty)
    FROM #PackingList_Detail pd
    WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq
      AND (',' + t.Carton + ',') LIKE ('%,' + pd.CTNStartNo + ',%')
) InspectedPoQty
ORDER BY ID

-- 清除暫存表
DROP TABLE #tmp, #PackingList_Detail, #MainData, #MainData1
";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
                if @IsTrans = 1
                begin
				    Insert Into P_CFAInspectionRecord_Detail_History(CFAInspectionRecord_Detail_Key,SPNO,Seq, FactoryID, BIFactoryID, BIInsertDate)
				    Select CFAInspectionRecord_Detail_Key,SPNO,Seq, FactoryID, BIFactoryID, GETDATE()
				    FROM P_CFAInspectionRecord_Detail P 
                    WHERE P.AuditDate >= '{item.SDate.Value.ToString("yyyy/MM/dd")}'  
                    AND NOT EXISTS(
                        SELECT 1 FROM #Final_P_CFAInspectionRecord_Detail T 
                        WHERE P.CFAInspectionRecord_Detail_Key = T.CFAInspectionRecord_Detail_Key 
                            AND P.SPNO = T.OrderID
                            AND P.Seq = T.Seq 
                            AND P.FACTORYID = T.FACTORYID
                    )     
                end

				DELETE P FROM P_CFAInspectionRecord_Detail P 
                WHERE P.AuditDate >= '{item.SDate.Value.ToString("yyyy/MM/dd")}'  
                AND NOT EXISTS(
                    SELECT 1 FROM #Final_P_CFAInspectionRecord_Detail T 
                    WHERE P.CFAInspectionRecord_Detail_Key = T.CFAInspectionRecord_Detail_Key 
                        AND P.SPNO = T.OrderID
                        AND P.Seq = T.Seq 
                        AND P.FACTORYID = T.FACTORYID
                )                                        

                UPDATE P SET
                    P.Action                     = T.Action
                    , P.AreaCodeDesc               = T.AreaCodeDesc
                    , P.AuditDate                  = T.AuditDate
                    , P.BrandID                    = T.BrandID
                    , P.BuyerDelivery              = T.BuyerDelivery
                    , P.CfaName                    = T.CFA
                    , P.ClogReceivedPercentage     = T.ClogReceivedPercentage
                    , P.DefectDesc                 = T.DefectDescription
                    , P.DefectQty                  = T.DefectQty
                    , P.Destination                = T.Dest
                    , P.Carton                     = T.Carton
                    , P.InspectedCtn               = T.[Inspected Ctn]
                    , P.InspectedPoQty             = T.[Inspected PoQty]
                    , P.InspectionStage            = T.Stage
                    , P.SewingLineID               = T.SewingLineID
                    , P.Mdivisionid                = T.Mdivisionid
                    , P.NoOfDefect                 = T.NoOfDefect
                    , P.OrderQty                   = T.Qty
                    , P.PONO                       = T.CustPoNo
                    , P.Remark                     = T.Remark
                    , P.Result                     = T.Result
                    , P.SampleLot                  = T.InspectQty
                    , P.Shift                      = T.Shift
                    , P.SQR                        = T.SQR
                    , P.Status                     = T.Status
                    , P.StyleID                    = T.StyleID
                    , P.Team                       = T.Team
                    , P.TtlCTN                     = T.[TTL CTN]
                    , P.VasShas                    = T.VasShas
                    , P.[1st_Inspection]            = T.[FirstInspection]
                    , P.InspectedSP                = T.InspectedSP
                    , P.InspectedSeq               = T.InspectedSeq
                    , P.ReInspection               = iif(T.ReInspection = 'Y',1,0)
                    , P.BIFactoryID                = T.BIFactoryID
                    , P.BIInsertDate               = T.BIInsertDate
                    , P.BIStatus                   = 'New'
                FROM P_CFAInspectionRecord_Detail P
                INNER JOIN #Final_P_CFAInspectionRecord_Detail T ON P.CFAInspectionRecord_Detail_Key = T.CFAInspectionRecord_Detail_Key AND
                                                                P.SPNO = T.OrderID AND 
                                                                P.Seq = T.Seq AND
                                                                P.FACTORYID = T.FACTORYID
                WHERE 
                    ISNULL(P.Action, '')                    <> ISNULL(T.Action, '') OR
                    ISNULL(P.AreaCodeDesc, '')              <> ISNULL(T.AreaCodeDesc, '') OR
                    ISNULL(P.AuditDate, '1900-01-01')       <> ISNULL(T.AuditDate, '1900-01-01') OR
                    ISNULL(P.BrandID, '')                   <> ISNULL(T.BrandID, '') OR
                    ISNULL(P.BuyerDelivery, '1900-01-01')   <> ISNULL(T.BuyerDelivery, '1900-01-01') OR
                    ISNULL(P.CfaName, '')                   <> ISNULL(T.CFA, '') OR
                    ISNULL(P.ClogReceivedPercentage, 0)     <> ISNULL(T.ClogReceivedPercentage, 0) OR
                    ISNULL(P.DefectDesc, '')                <> ISNULL(T.DefectDescription, '') OR
                    ISNULL(P.DefectQty, 0)                  <> ISNULL(T.DefectQty, 0) OR
                    ISNULL(P.Destination, '')               <> ISNULL(T.Dest, '') OR
                    ISNULL(P.Carton, '')                    <> ISNULL(T.Carton, '') OR
                    ISNULL(P.InspectedCtn, 0)               <> ISNULL(T.[Inspected Ctn], 0) OR
                    ISNULL(P.InspectedPoQty, 0)             <> ISNULL(T.[Inspected PoQty], 0) OR
                    ISNULL(P.InspectionStage, '')           <> ISNULL(T.Stage, '') OR
                    ISNULL(P.SewingLineID, '')              <> ISNULL(T.SewingLineID, '') OR
                    ISNULL(P.Mdivisionid, '')               <> ISNULL(T.Mdivisionid, '') OR
                    ISNULL(P.NoOfDefect, 0)                 <> ISNULL(T.NoOfDefect, 0) OR
                    ISNULL(P.OrderQty, 0)                   <> ISNULL(T.Qty, 0) OR
                    ISNULL(P.PONO, '')                      <> ISNULL(T.CustPoNo, '') OR
                    ISNULL(P.Remark, '')                    <> ISNULL(T.Remark, '') OR
                    ISNULL(P.Result, '')                    <> ISNULL(T.Result, '') OR
                    ISNULL(P.SampleLot, 0)                  <> ISNULL(T.InspectQty, 0) OR
                    ISNULL(P.Shift, '')                     <> ISNULL(T.Shift, '') OR
                    ISNULL(P.SQR, 0)                        <> ISNULL(T.SQR, 0) OR
                    ISNULL(P.Status, '')                    <> ISNULL(T.Status, '') OR
                    ISNULL(P.StyleID, '')                   <> ISNULL(T.StyleID, '') OR
                    ISNULL(P.Team, '')                      <> ISNULL(T.Team, '') OR
                    ISNULL(P.TtlCTN, 0)                     <> ISNULL(T.[TTL CTN], 0) OR
                    ISNULL(P.VasShas, '')                   <> ISNULL(T.VasShas, '') OR
                    ISNULL(P.[1st_Inspection], '')          <> ISNULL(T.[FirstInspection], '') OR
                    ISNULL(P.InspectedSP, '')               <> ISNULL(T.InspectedSP, '') OR
                    ISNULL(P.InspectedSeq, '')              <> ISNULL(T.InspectedSeq, '') OR
                    ISNULL(P.ReInspection, 0)               <> IIF(T.ReInspection = 'Y', 1, 0)

                INSERT INTO [dbo].[P_CFAInspectionRecord_Detail]
                (
                CFAInspectionRecord_Detail_Key    
                ,[Action]
                ,[AreaCodeDesc]
                ,[AuditDate]
                ,[BrandID]
                ,[BuyerDelivery]
                ,[CfaName]
                ,[ClogReceivedPercentage]
                ,[DefectDesc]
                ,[DefectQty]
                ,[Destination]
                ,[FactoryID]
                ,[Carton]
                ,[InspectedCtn]
                ,[InspectedPoQty]
                ,[InspectionStage]
                ,[SewingLineID]
                ,[Mdivisionid]
                ,[NoOfDefect]
                ,[OrderQty]
                ,[PONO]
                ,[Remark]
                ,[Result]
                ,[SampleLot]
                ,[Seq]
                ,[Shift]
                ,[SPNO]
                ,[SQR]
                ,[Status]
                ,[StyleID]
                ,[Team]
                ,[TtlCTN]
                ,[VasShas]
                ,[1st_Inspection]
                ,[InspectedSP]
                ,[InspectedSeq] 
                ,[ReInspection]
                ,[BIFactoryID]
                ,[BIInsertDate]
                ,[BIStatus]
                )
                select 
                 CFAInspectionRecord_Detail_Key
                ,isnull(Action ,'')
                ,isnull(AreaCodeDesc , '')
                ,AuditDate
                ,isnull(BrandID, '')
                ,BuyerDelivery
                ,isnull(CFA, '')
                ,isnull(ClogReceivedPercentage , 0)
                ,isnull(DefectDescription, '')
                ,isnull(DefectQty, 0)
                ,isnull(Dest ,'')
                ,FactoryID
                ,isnull(Carton, '')
                ,isnull([Inspected Ctn], 0)
                ,isnull([Inspected PoQty], 0)
                ,isnull(Stage ,'')
                ,isnull(SewingLineID, '')
                ,isnull(MDivisionid, '')
                ,isnull(NoOfDefect, 0)
                ,isnull(Qty, 0)
                ,isnull(CustPoNo, '')
                ,isnull(Remark, '')
                ,isnull(Result, '')
                ,isnull(InspectQty, 0)
                ,Seq
                ,isnull(Shift, '')
                ,OrderID
                ,isnull(SQR, 0)
                ,isnull(Status,'')
                ,isnull(StyleID, '')
                ,isnull(Team, '')
                ,isnull([TTL CTN] , 0)
                ,isnull(VasShas ,'')
                ,isnull(FirstInspection ,'')
                ,isnull([InspectedSP], '')
                ,isnull([InspectedSeq],'') 
                ,iif(T.ReInspection = 'Y',1,0)
                ,BIFactoryID
                ,BIInsertDate
                ,'New'
                FROM #Final_P_CFAInspectionRecord_Detail T
                WHERE NOT EXISTS(
                    SELECT 1 FROM P_CFAInspectionRecord_Detail P 
                    WHERE P.CFAInspectionRecord_Detail_Key = T.CFAInspectionRecord_Detail_Key 
                    AND P.SPNO = T.OrderID 
                    AND P.Seq = T.Seq
                    AND P.FACTORYID = T.FACTORYID
                )";

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_P_CFAInspectionRecord_Detail", paramters: sqlParameters);
            }

            return finalResult;
        }
    }
}
