using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class QA_R31
    {
        /// <inheritdoc/>
        public QA_R31()
        {
            DBProxy.Current.DefaultTimeout = 900;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetQA_R31Data(QA_R31_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@Buyerdelivery1", SqlDbType.Date) { Value = (object)model.BuyerDelivery1 ?? DBNull.Value },
                new SqlParameter("@Buyerdelivery2", SqlDbType.Date) { Value = (object)model.BuyerDelivery2 ?? DBNull.Value },

                new SqlParameter("@sp1", SqlDbType.VarChar, 20) { Value = model.SP1 },
                new SqlParameter("@sp2", SqlDbType.VarChar, 20) { Value = model.SP2 },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 10) { Value = model.MDivisionID },
                new SqlParameter("@FactoryID", SqlDbType.VarChar, 10) { Value = model.FactoryID },
                new SqlParameter("@Brand", SqlDbType.VarChar, 10) { Value = model.Brand },
                new SqlParameter("@Stage", SqlDbType.VarChar, 10) { Value = model.InspStaged },
                new SqlParameter("@Category", SqlDbType.VarChar, 10) { Value = model.Category },
                new SqlParameter("@Exclude_Sister_Transfer_Out", SqlDbType.Bit, 10) { Value = model.Exclude_Sister_Transfer_Out },
                new SqlParameter("@Outstanding", SqlDbType.Bit, 10) { Value = model.Outstanding },
                new SqlParameter("@IsPowerBI", SqlDbType.Bit) { Value = model.IsPowerBI },
            };

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
SELECT  
o.MDivisionID
,o.FactoryID
,oq.BuyerDelivery
,o.BrandID
,oq.ID
,Category = CASE   WHEN o.Category='B' THEN 'Bulk'
                   WHEN o.Category='S' THEN 'Sample'
                   WHEN o.Category='G' THEN 'Garment'
                   ELSE ''
              END
,o.OrderTypeID
,o.CustPoNo
,o.StyleID
,s.StyleName
,o.SeasonID
,[Dest]=c.Alias
,o.Customize1
,o.CustCDID
,oq.Seq
,oq.ShipModeID
,[ColorWay] = Articles.Val
,o.SewLine
,[TtlCtn] =  IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(TtlCtn.Val,0) as varchar)  )
,[StaggeredCtn] = IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(StaggeredCtn.Val,0) as varchar)  )
,[ClogCtn] = IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(ClogCtn.Val,0) as varchar)  )
,[ClogCtn%]= IIF( ClogCtn.Val = 0 OR o.Category ='S'
					, 'N/A' 
					, CAST( CAST(ROUND((TtlCtn.Val *1.0 / TtlCtn.Val * 100),0) as int)  as varchar)  
				)
,[LastCartonReceivedDate] = LastReceived.Val
,oq.CFAFinalInspectDate
,oq.CFA3rdInspectDate
,oq.CFARemark
INTO #tmp
FROM Order_QtyShip oq
INNER JOIN Orders o ON o.ID = oq.Id and exists (select 1 from Factory f where o.FactoryId = id and f.IsProduceFty = 1)
INNER JOIN Factory f ON o.FactoryID = f.ID
INNER JOIN Style s ON o.StyleUkey = s.Ukey
LEFT JOIN OrderType ot ON o.OrderTypeID = ot.ID AND o.BrandID = ot.BrandID
LEFT JOIN Country c ON o.Dest = c.ID
OUTER APPLY(
	SELECT [Val]=STUFF((
	SELECT DISTINCT ',' + Article
	FROM Order_QtyShip_Detail oqd
	WHERE oqd.ID = oq.Id ANd oqd.Seq = oq.Seq
	FOR XML PATH('')
	) ,1,1,'')
)Articles
OUTER APPLY(
	SELECT [Val]=Sum(pd.ShipQty)
	From PackingList_Detail pd
	INNER JOIN CFAInspectionRecord cfa on pd.StaggeredCFAInspectionRecordID	 = cfa.ID
	Where cfa.Status='Confirmed' and cfa.Stage='Stagger' and pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
)StaggeredOutput
OUTER APPLY(
	SELECT [Val] = IIF( (SELECT COUNT(oq2.Seq) FROM Order_QtyShip oq2 WHERE oq2.ID = oq.ID) > 1
						,'N/A' 
						, Cast( dbo.getMinCompleteSewQty(oq.ID,NULL,NULL) as varchar)
					)
)CMPoutput
OUTER APPLY(
	SELECT [Val] = ISNULL( SUM(IIF
				( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL
				,pd.ShipQty
				,0)
			),0)
	FROM PackingList_Detail pd
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq= oq.Seq
)ClogReceivedQty
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM PackingList_Detail pd
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq  = oq.Seq AND pd.CTNQty = 1
)TtlCtn
OUTER APPLY(
	SELECT [Val]=Count(DISTINCT pd.CTNStartNo)
	From PackingList_Detail pd 
	INNER JOIN CFAInspectionRecord CFA on pd.StaggeredCFAInspectionRecordID=CFA.ID
	Where CFA.Status='Confirmed' 
	AND CFA.Stage='Stagger'
	AND pd.CTNQty=1
	AND pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
)StaggeredCtn
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM PackingList_Detail pd 
	where pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
	AND pd.CTNQty=1
	AND (pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL)
)ClogCtn
OUTER APPLY(
	SELECT [Val] = MAX(pd.ReceiveDate) 
	FROM PackingList_Detail pd
	WHERE pd.OrderID = oq.Id AND pd.OrderShipmodeSeq  = oq. Seq
	AND NOT exists (
		-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
		select 1 
		from Production.dbo.PackingList_Detail pdCheck
		where pd.OrderID = pdCheck.OrderID 
				and pd.OrderShipmodeSeq = pdCheck.OrderShipmodeSeq
				and pdCheck.ReceiveDate is null
	)
)LastReceived 
WHERE 1=1
");

            #region Where

            if (model.IsPowerBI == true)
            {
                sqlCmd.Append(@"
and oq.BuyerDelivery between @Buyerdelivery1 and @Buyerdelivery2
AND f.IsProduceFty = 1
AND o.Category IN ('B','G') 
" + Environment.NewLine);
            }
            else
            {
                if (!MyUtility.Check.Empty(model.BuyerDelivery1))
                {
                    sqlCmd.Append($"AND oq.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.SP1))
                {
                    sqlCmd.Append($"AND oq.ID >= @sp1" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.SP2))
                {
                    sqlCmd.Append($"AND oq.ID <= @sp2" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.MDivisionID))
                {
                    sqlCmd.Append($"AND o.MDivisionID=@MDivisionID " + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.FactoryID))
                {
                    sqlCmd.Append($"AND o.FtyGroup=@FactoryID " + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.Brand))
                {
                    sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.Exclude_Sister_Transfer_Out))
                {
                    sqlCmd.Append($"AND f.IsProduceFty = 1" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.Category))
                {
                    sqlCmd.Append($"AND o.Category IN ('{model.Category}') " + Environment.NewLine);
                }
                else
                {
                    // 如果全部沒勾選，則無資料
                    sqlCmd.Append($"AND 1=0 " + Environment.NewLine);
                }
            }

            #endregion

            sqlCmd.Append($@"
----戴上要檢驗的Stage帽子
SELECT DISTINCT [Stage]='Stagger',t.*
INTO #NeedCkeck
FROM  #tmp t 
UNION 
SELECT DISTINCT [Stage]='Final',t.*
FROM  #tmp t 
UNION 
SELECT DISTINCT [Stage]='3rd party',t.*
FROM  #tmp t 
INNER JOIN Order_QtyShip oq ON oq.Id = t.Id AND oq.Seq = t.Seq
WHERE oq.CFAIs3rdInspect = 1


----取出需要的資料
SELECT *
INTO #PackingList_Detail
FROM PackingList_Detail pd
WHERE EXISTS(
	SELECT 1 
	FROM #NeedCkeck n
	WHERE n.ID = pd.OrderID AND n.Seq = pd.OrderShipmodeSeq
)

SELECT * 
INTO #CFAInspectionRecord_OrderSEQ 
FROM CFAInspectionRecord_OrderSEQ co
WHERE EXISTS(
	SELECT 1 
	FROM #NeedCkeck n
	WHERE co.OrderID = n.Id AND co.SEQ = n.Seq
)

SELECT * 
INTO #CFAInspectionRecord
FROM CFAInspectionRecord c
WHERE ID IN(
	SELECT ID 
	FROM #CFAInspectionRecord_OrderSEQ
)	 

");
            #region Outstanding WHERE
            List<string> outstandingWHERE = new List<string>();
            string colBI = string.Empty;

            if (MyUtility.Check.Empty(model.InspStaged) || model.InspStaged == "Stagger" || model.IsPowerBI)
            {
                colBI = model.IsPowerBI ? @"
,[Notyetinspqty] = (
								SELECT isnull(SUM(pd.ShipQty),0)
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND NOT EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage='Stagger' AND cf.Status='Confirmed'
									AND (	
										cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +',%'
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo
									)
								) 
							)
,[FailQty] =(
					SELECT isnull(sum(pd.ShipQty),0)
					FROM #PackingList_Detail pd
					WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
					AND pd.StaggeredCFAInspectionRecordID = ''
					AND EXISTS(
						SELECT 1
						FROM #CFAInspectionRecord cf
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
						WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
						AND cf.Stage='Stagger' AND cf.Status='Confirmed' AND cf.Result='Fail'
						AND (	
								cfo.Carton = pd.CTNStartNo
							OR cfo.Carton LIKE  pd.CTNStartNo +',%' 
							OR cfo.Carton LIKE '%,'+  pd.CTNStartNo +',%' 
							OR cfo.Carton LIKE '%,'+  pd.CTNStartNo
						)
					) 
				)
" : string.Empty;

                string stageSql = $@"

/*-----Staggered-----*/
SELECT [InspResult]=CASE WHEN EXISTS(
								-- Result  - Fail的情況
								SELECT 1
								FROM #NeedCkeck a
								INNER JOIN #PackingList_Detail pd ON pd.OrderID = a.Id ANd pd.OrderShipmodeSeq =a.Seq
								INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cfo.OrderID = pd.OrderID AND cfo.SEQ = pd.OrderShipmodeSeq
								INNER JOIN #CFAInspectionRecord cf ON cf.ID = cfo.ID
								WHERE cf.Stage = 'Stagger' AND a.Category != 'Sample' AND cf.Result = 'Fail' AND cf.Status ='Confirmed'
								AND a.Id = need.Id AND a.Seq = need.Seq
							)
						THEN 'Fail'	
					WHEN NOT EXISTS(
						-- Result  -  空白的情況
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN #PackingList_Detail pd ON pd.OrderID = a.Id ANd pd.OrderShipmodeSeq =a.Seq
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cfo.OrderID = pd.OrderID AND cfo.SEQ = pd.OrderShipmodeSeq
						INNER JOIN #CFAInspectionRecord cf ON cf.ID = cfo.ID
						WHERE cf.Stage = 'Stagger' AND a.Category != 'Sample' AND  cf.Status ='Confirmed'
						AND a.Id = need.Id AND a.Seq = need.Seq
					)
			 THEN ''
		ELSE ''
		END
	,[NotYetInspCtn#]=(SELECT STUFF((
								SELECT DISTINCT ','+CTNStartNo
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND NOT EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage='Stagger' AND cf.Status='Confirmed'
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo
									)
								) 
								FOR XML PATH('')
							),1,1,''))
	,[NotYetInspCtn]=(
								SELECT COUNT(DISTINCT CTNStartNo)
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND NOT EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage='Stagger' AND cf.Status='Confirmed'
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo
									)
								) )
	,[FailCtn#]=(SELECT STUFF((
								SELECT DISTINCT ','+CTNStartNo
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND pd.StaggeredCFAInspectionRecordID = ''
								AND EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage='Stagger' AND cf.Status='Confirmed' AND cf.Result='Fail'
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo
									)
								) 
								FOR XML PATH('')
							),1,1,''))
	,[FailCtn]=(
								SELECT COUNT(DISTINCT CTNStartNo)
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND pd.StaggeredCFAInspectionRecordID = ''
								AND EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage='Stagger' AND cf.Status='Confirmed' AND cf.Result='Fail'
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo +',%' 
										OR cfo.Carton LIKE '%,'+  pd.CTNStartNo
									)
								) )
	{colBI}
	,need.*
FROM #NeedCkeck need
WHERE need.Stage = 'Stagger' AND need.Category != 'Sample'	
AND (
		SELECT COUNT(DISTINCT CTNStartNo)
		FROM #PackingList_Detail pd
		WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
		AND EXISTS(
			SELECT 1
			FROM #CFAInspectionRecord cf
			INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
			WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
			AND cf.Stage='Stagger' AND cf.Status='Confirmed' AND cf.Result!='Pass'
			AND (	
					cfo.Carton = pd.CTNStartNo
				OR cfo.Carton LIKE  pd.CTNStartNo +',%' 
				OR cfo.Carton LIKE '%,'+  pd.CTNStartNo +',%' 
				OR cfo.Carton LIKE '%,'+  pd.CTNStartNo
			)
        )
) != 
(SELECT COUNT( DISTINCT CTNStartNo)
FROM #PackingList_Detail  
WHERE OrderID = need.ID AND OrderShipmodeSeq = need.Seq )

";
                outstandingWHERE.Add(stageSql);
            }

            if (MyUtility.Check.Empty(model.InspStaged) || model.InspStaged == "Final" || model.IsPowerBI)
            {
                colBI = model.IsPowerBI ? @"
,[Notyetinspqty]  = 0
,[FailQty] = 0
" : string.Empty;
                string stageSql = $@"

/*-----Final-----*/
SELECT [InspResult]=CASE WHEN NOT EXISTS(
						SELECT 1 
						FROM #CFAInspectionRecord cr 
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cfoq.OrderID = need.ID AND cfoq.Seq = need.Seq AND cr.Stage = 'Final' AND cr.Status = 'Confirmed' ) 
					THEN ''
					WHEN EXISTS(						
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN Order_QtyShip oq ON oq.ID = a.Id ANd oq.Seq =a.Seq
						WHERE a.Stage = 'Final' 
						AND oq.CFAFinalInspectResult  != 'Pass'
						AND a.Id = need.Id AND a.Seq = need.Seq
					)THEN 'Fail'
					ELSE ''
					END
	,[NotYetInspCtn#]=NULL
	,[NotYetInspCtn]=NULL
	,[FailCtn#]=(
		SELECt TOP 1  cfoq.Carton
		FROM #CFAInspectionRecord  cr
		INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
		WHERE cr.Stage = 'Final' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
		AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
		ORDER BY cr.AuditDate DESC, cr.EditDate DESC
	)
	,[FailCtn]=(	
		SELECT COUNT(DISTINCT data)
		FROM dbo.SplitString((
						SELECt TOP 1  cfoq.Carton
						FROM #CFAInspectionRecord  cr
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cr.Stage = 'Final' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
						AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
						ORDER BY cr.AuditDate DESC, cr.EditDate DESC
		),',')
	)
	{colBI}
	,need.*
FROM #NeedCkeck need
WHERE need.Stage = 'Final'
AND NOT EXISTS (	
	SELECT *
	FROM #CFAInspectionRecord a
	INNER JOIN #CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
	WHERE b.OrderID =need.ID AND b.SEQ = b.SEQ AND a.Stage = 'Final' AND a.Status='Confirmed' AND (a.Result = 'Pass' OR a.Result='Fail but release')
) 

";
                outstandingWHERE.Add(stageSql);
            }

            if ((MyUtility.Check.Empty(model.InspStaged) || model.InspStaged == "3rd party") && !model.IsPowerBI)
            {
                string stageSql = $@"

/*-----3rd Party-----*/
SELECT[InspResult]=CASE WHEN NOT EXISTS(
						SELECT 1 
						FROM #CFAInspectionRecord cr 
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cfoq.OrderID = need.ID AND cfoq.Seq = need.Seq AND cr.Stage = '3rd Party' AND cr.Status = 'Confirmed' 
					)
					THEN ''  ----沒有檢驗紀錄 = 空白
					WHEN EXISTS(						
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN Order_QtyShip oq ON oq.ID = a.Id ANd oq.Seq =a.Seq
						WHERE a.Stage = '3rd Party'  AND oq.CFAIs3rdInspect = 1
						AND oq.CFA3rdInspectResult  != 'Pass'
						AND a.Id = need.Id AND a.Seq = need.Seq
					)THEN 'Fail'
					ELSE ''
					END
	,[NotYetInspCtn#]=NULL
	,[NotYetInspCtn]=NULL	
	,[FailCtn#]=(
		SELECt TOP 1 cfoq.Carton
		FROM #CFAInspectionRecord  cr
		INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
		WHERE cr.Stage = '3rd party' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
		AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
		ORDER BY cr.AuditDate DESC, cr.EditDate DESC
	)
	,[FailCtn]=(	
		SELECT COUNT(DISTINCT data)
		FROM dbo.SplitString((
						SELECt TOP 1 cfoq.Carton
						FROM #CFAInspectionRecord  cr
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cr.Stage = '3rd Party' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
						AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
						ORDER BY cr.AuditDate DESC, cr.EditDate DESC
		),',')
	)
	,need.*
FROM #NeedCkeck need
WHERE need.Stage = '3rd Party'
AND NOT EXISTS (	
	SELECT *
	FROM #CFAInspectionRecord a
	INNER JOIN #CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
	WHERE b.OrderID =need.ID AND b.SEQ = b.SEQ AND a.Stage = '3rd Party' AND a.Status='Confirmed' AND (a.Result = 'Pass' OR a.Result='Fail but release')
) 
";
                outstandingWHERE.Add(stageSql);
            }
            #endregion

            sqlCmd.Append(outstandingWHERE.JoinToString("UNION") + Environment.NewLine + "DROP TABLE #tmp,#NeedCkeck,#PackingList_Detail,#CFAInspectionRecord,#CFAInspectionRecord_OrderSEQ");

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlCmd.ToString(), listPar, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }
    }
}
