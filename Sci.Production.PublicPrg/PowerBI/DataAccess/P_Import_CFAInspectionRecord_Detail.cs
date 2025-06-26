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
                Base_ViewModel resultReport = this.GetCFAInspectionRecord_Detail_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable);
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
    Action, AreaCodeDesc, AuditDate, BrandID, BuyerDelivery, CFA, ClogReceivedPercentage,
    DefectDescription, DefectQty, Dest, FactoryID, Carton,
    [Inspected Ctn] = InspectedCtn.Val,
    [Inspected PoQty] = InspectedPoQty.Val,
    Stage, SewingLineID, MDivisionid, NoOfDefect, Qty, CustPoNo,
    Remark, Result, InspectQty, Seq, Shift, OrderID, SQR, Status,
    StyleID, Team,
    [TTL CTN] = TtlCtn.Val, VasShas,
    FirstInspection = IIF(FirstInspection = 1, 'Y', ''),
    t.[InspectedSP], t.[InspectedSeq], t.[ReInspection],
    [BIFactoryID] = @BIFactoryID,
    [BIInsertDate] = GETDATE()
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

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
				Insert Into P_CFAInspectionRecord_Detail_History(Ukey, FactoryID, BIFactoryID, BIInsertDate)
				Select Ukey, FactoryID, BIFactoryID, BIInsertDate
				FROM P_CFAInspectionRecord_Detail T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)

				DELETE T FROM P_CFAInspectionRecord_Detail T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)

				INSERT INTO [dbo].[P_CFAInspectionRecord_Detail]
				(
					 [Action]
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
				)
				select 
					isnull(Action ,'')
					,isnull(AreaCodeDesc , '')
					,AuditDate
					,isnull(BrandID, '')
					,BuyerDelivery
					,isnull(CFA, '')
					,isnull(ClogReceivedPercentage , 0)
					,isnull(DefectDescription, '')
					,isnull(DefectQty, 0)
					,isnull(Dest ,'')
					,isnull(FactoryID, '')
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
					,isnull(Seq ,'')
					,isnull(Shift, '')
					,isnull(OrderID, '')
					,isnull(SQR, 0)
					,isnull(Status,'')
					,isnull(StyleID, '')
					,isnull(Team, '')
					,isnull([TTL CTN] , 0)
					,isnull(VasShas ,'')
					,isnull(FirstInspection ,'')
					,isnull([InspectedSP], '')
					,isnull([InspectedSeq],'') 
					,[ReInspection]
					,isnull(BIFactoryID, '')
					,isnull(BIInsertDate, GetDate())
					from #Final_P_CFAInspectionRecord_Detail";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_P_CFAInspectionRecord_Detail");
            }

            return finalResult;
        }
    }
}
