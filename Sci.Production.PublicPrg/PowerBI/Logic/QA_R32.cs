using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class QA_R32
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public QA_R32()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetQA_R32_Detail(QA_R32_ViewModel model)
        {
            string strWhere = this.GetWhere(model);
            List<SqlParameter> listPar = this.GetSqlParameters(model);
            string sqlcmd = string.Empty;

            sqlcmd = $@"
            ----QA R32 Detail
            SELECT c.*,co.OrderID,co.SEQ, co.Carton
            INTO #MainData1
            FROm CFAInspectionRecord  c
            INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
            INNER JOIN Orders O ON o.ID = co.OrderID
            WHERE 1=1
            and exists (select 1 from Factory f where o.FtyGroup = id and f.IsProduceFty = 1)
            {strWhere}

			SELECT c.*,co.OrderID,co.SEQ, co.Carton
			,[InspectedSP] = cfos.OrderID
			,[InspectedSeq] = cfos.Seq
			INTO #MainData
			FROm CFAInspectionRecord  c
			INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
			OUTER APPLY (
				SELECT TOP 1 OrderID, Seq
				FROM Production.dbo.CFAInspectionRecord_OrderSEQ cfos WITH (NOLOCK)
				WHERE c.ID = cfos.ID
				ORDER BY Ukey
			) cfos
			WHERE c.ID IN (SELECT ID FROM #MainData1)

			SELECT 
			c.AuditDate
			,BuyerDelivery = (SELECT BuyerDelivery FROM Orders WHERE ID = c.OrderID)
			,c.OrderID
			,CustPoNo = (SELECT CustPoNo FROM Orders WHERE ID = c.OrderID)
			,StyleID = (SELECT StyleID FROM Orders WHERE ID = c.OrderID)
			,BrandID = (SELECT BrandID FROM Orders WHERE ID = c.OrderID)
			,Dest = (SELECT Dest FROM Orders WHERE ID = c.OrderID)
			,Seq = (SELECT Seq FROM Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
			,c.SewingLineID
			,[VasShas]= (SELECT IIF(VasShas=1,'Y','N')  FROM Orders WHERE ID = c.OrderID)
			,c.ClogReceivedPercentage
			,c.MDivisionid
			,c.FactoryID
			,c.Shift
			,c.Team
			,Qty = (SELECT Qty FROM Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
			,c.Status
			,[Carton]= IIF(c.Carton ='' AND c.Stage = '3rd party','N/A',c.Carton)
			,[CFA] = dbo.getPass1(c.CFA)
			,[ReInspection] = IIF(c.ReInspection = 1, 'Y','')
			,c.stage
			,[FirstInspection] = IIF(c.FirstInspection = 1, 'Y','')
			,c.Result
			,c.InspectQty
			,[Reject Qty] = c.DefectQty
			,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty) * 100)
			,[DefectDescription] = g.Description
			,[AreaCodeDesc] = CfaArea.val
			,[NoOfDefect] = cd.Qty
			,cd.Remark
			,c.ID
			,c.IsCombinePO
			, [Action]= cd.Action
			,[CFAInspectionRecord_Detail_Key]= concat(c.ID,
				iif( isnull(cd.GarmentDefectCodeID, '') = ''
					, ''
					, cd.GarmentDefectCodeID
				)	
			)
			{(model.IsPowerBI ? ", c.[InspectedSP], c.[InspectedSeq] ,[CFAInspectionRecordID] = c.ID" : string.Empty)}
			INTO #tmp
			FROm #MainData  c
			LEFT JOIN CFAInspectionRecord_Detail cd ON c.ID = cd.ID
			LEFT JOIN GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
			OUTER APPLY
			(
				SELECT [val] = STUFF((
						SELECT ',' + ca.Id + ' - ' + ca.Description
						FROM CfaArea ca
						WHERE CHARINDEX(ca.ID, cd.CFAAreaID) > 0
						FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')
			)CfaArea

			----找出CFAInspectionRecord_OrderSEQ所對應到的PackingList_Detail
			SELECT pd.*
			INTO #PackingList_Detail
			FROM PackingList_Detail pd 
			WHERE EXISTS (SELECT 1 FROM #tmp t WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.SEQ) 

			SELECT   t.ID
					,CFAInspectionRecord_Detail_Key
					,AuditDate
					,BuyerDelivery
					,OrderID
					,CustPoNo
					,StyleID
					,BrandID
					,Dest
					,Seq
					,SewingLineID
					,VasShas
					,ClogReceivedPercentage
					,MDivisionid
					,FactoryID
					,Shift
					,Team
					,Qty
					,Status
					,[TTL CTN] = TtlCtn.Val
					,[Inspected Ctn] = InspectedCtn.Val
					,[Inspected PoQty] = InspectedPoQty.Val
					,Carton
					,CFA
					,[ReInspection]
					,Stage
					,FirstInspection
					,Result
					,InspectQty
					,[Reject Qty]
					,DefectQty = DefectQty.Val
					,SQR
					,DefectDescription
					,AreaCodeDesc
					,NoOfDefect
					,Remark
					,Action
			{(model.IsPowerBI ? ",[InspectedSP] ,[InspectedSeq] ,[CFAInspectionRecordID]" : string.Empty)}
			into #tmpFinal
			FROM  #tmp t
			OUTER APPLY(
				SELECT [Val] = COUNT(1)
				FROM #PackingList_Detail pd
				WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq AND pd.CTNQty > 0 AND pd.CTNStartNo != ''
			)TtlCtn
			OUTER APPLY(
				SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
				FROM #PackingList_Detail pd
				WHERE pd.OrderID = t.OrderID  AND pd.OrderShipmodeSeq = t.Seq
					AND (',' + t.Carton + ',') like ('%,' + pd.CTNStartNo + ',%')
					AND pd.CTNQty=1
			)InspectedCtn    --計算所有階段的總箱數
			OUTER APPLY(
				SELECT [Val] = SUM(pd.ShipQty)
				FROM #PackingList_Detail pd
				WHERE pd.OrderID = t.OrderID  AND pd.OrderShipmodeSeq = t.Seq
					AND (',' + t.Carton + ',') like ('%,' + pd.CTNStartNo + ',%')
			)InspectedPoQty   --計算所有階段的總成衣件數
			OUTER APPLY(
				SELECT [Val] = SUM(Qty)
				FROM CFAInspectionRecord_Detail cd
				WHERE cd.ID = t.ID
			)DefectQty
			Order by id

			-- 相同CFAInspectionRecord_Detail_Key合併成一筆 by ISP20240116
			select  
			t.ID
			,CFAInspectionRecord_Detail_Key
			,AuditDate
			,BuyerDelivery
			,OrderID
			,CustPoNo
			,StyleID
			,BrandID
			,Dest
			,Seq = Seq.value
			,SewingLineID
			,VasShas
			,ClogReceivedPercentage
			,MDivisionid
			,FactoryID
			,Shift
			,Team
			,Qty = sum(Qty)
			,Status
			,[TTL CTN] = sum([TTL CTN])
			,[Inspected Ctn] = sum([Inspected Ctn])
			,[Inspected PoQty] = sum([Inspected PoQty])
			,Carton = Carton.value
			,CFA
			,ReInspection
			,Stage
			,FirstInspection
			,Result
			,InspectQty
			,[Reject Qty]
			,DefectQty
			,SQR
			,DefectDescription
			,AreaCodeDesc
			,NoOfDefect
			,Remark
			,Action 
			{(model.IsPowerBI ? ",[InspectedSP] ,[InspectedSeq] ,[CFAInspectionRecordID]" : string.Empty)}
			into #tmpEnd
			FROM #tmpFinal t
			outer apply(
				select value = Stuff((
					select concat(',',seq)
					from (
							select 	distinct
								seq
							from dbo.#tmpFinal s
							where s.CFAInspectionRecord_Detail_Key = t.CFAInspectionRecord_Detail_Key
							and s.OrderID = t.OrderID
						) s
					for xml path ('')
				) , 1, 1, '')
			) Seq
			outer apply(
				select value = Stuff((
					select concat(',',Carton)
					from (
							select 	distinct
								Carton
							from dbo.#tmpFinal s
							where s.CFAInspectionRecord_Detail_Key = t.CFAInspectionRecord_Detail_Key
							and s.OrderID = t.OrderID
						) s
					for xml path ('')
				) , 1, 1, '')
			) Carton
			group by t.ID
					, CFAInspectionRecord_Detail_Key
					,AuditDate
					,BuyerDelivery
					,OrderID
					,CustPoNo
					,StyleID
					,BrandID
					,Dest
					,Seq.value
					,SewingLineID
					,VasShas
					,ClogReceivedPercentage
					,MDivisionid
					,FactoryID
					,Shift
					,Team
					,Status
					,Carton.value
					,CFA
					,ReInspection
					,Stage
					,FirstInspection
					,Result
					,InspectQty
					,[Reject Qty]
					,DefectQty
					,SQR
					,DefectDescription
					,AreaCodeDesc
					,NoOfDefect
					,Remark
					,Action 
					{(model.IsPowerBI ? ",[InspectedSP] ,[InspectedSeq] ,[CFAInspectionRecordID]" : string.Empty)}
			Select 
			* 
			{(model.IsPowerBI ? ",[BIFactoryID] = @BIFactoryID ,[BIInsertDate] = GETDATE()" : string.Empty)}
			from #tmpEnd

			DROP TABLE #tmp ,#MainData ,#PackingList_Detail,#MainData1,#tmpFinal";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, listPar, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        /// <inheritdoc/>
        private string GetWhere(QA_R32_ViewModel model)
        {
            string strWhere = string.Empty;

            if (!MyUtility.Check.Empty(model.StartAuditDate))
            {
                if (model.IsPowerBI)
                {
                    strWhere += $" AND c.AuditDate >= @StartAuditDate" + Environment.NewLine;
                }
                else
                {
                    strWhere += $" AND c.AuditDate BETWEEN @StartAuditDate AND @EndAuditDate" + Environment.NewLine;
                }
            }

            if (!MyUtility.Check.Empty(model.StartBuyerDelivery))
            {
                strWhere += $" AND o.BuyerDelivery BETWEEN @StartBuyerDelivery AND @EndBuyerDelivery" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                strWhere += $" AND o.MDivisionID=@MDivisionID " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                strWhere += $" AND o.FtyGroup =@FactoryID " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.BrandID))
            {
                strWhere += $" AND o.BrandID=@BrandID " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.StartSP))
            {
                strWhere += $" AND co.OrderID  >= @StartSP" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.EndSP))
            {
                strWhere += $" AND co.OrderID  <= @EndSP" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Stage))
            {
                strWhere += $" AND c.Stage=@Stage " + Environment.NewLine;
            }

            return strWhere;
        }

        /// <inheritdoc/>
        private List<SqlParameter> GetSqlParameters(QA_R32_ViewModel model)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@StartAuditDate", SqlDbType.Date) { Value = (object)model.StartAuditDate ?? DBNull.Value },
                new SqlParameter("@EndAuditDate", SqlDbType.Date) { Value = (object)model.EndAuditDate ?? DBNull.Value },
                new SqlParameter("@StartBuyerDelivery", SqlDbType.Date) { Value = (object)model.StartBuyerDelivery ?? DBNull.Value },
                new SqlParameter("@EndBuyerDelivery", SqlDbType.Date) { Value =(object) model.EndBuyerDelivery ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar) { Value = (object)model.MDivisionID ?? DBNull.Value },
                new SqlParameter("@FactoryID", SqlDbType.VarChar) { Value = (object)model.FactoryID ?? DBNull.Value },
                new SqlParameter("@BrandID", SqlDbType.VarChar) { Value = (object)model.BrandID ?? DBNull.Value },
                new SqlParameter("@StartSP", SqlDbType.VarChar) { Value = (object)model.StartSP ?? DBNull.Value },
                new SqlParameter("@EndSP", SqlDbType.VarChar) { Value = (object)model.EndSP ?? DBNull.Value },
                new SqlParameter("@Stage", SqlDbType.VarChar) { Value = (object)model.Stage ?? DBNull.Value },
                new SqlParameter("@BIFactoryID", SqlDbType.VarChar) { Value = (object)model.BIFactoryID ?? DBNull.Value },
            };
        }
    }
}
