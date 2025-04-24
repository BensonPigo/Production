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
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public QA_R31()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
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
AND o.Category IN ('B','G','S') 
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
SELECT DISTINCT [Stage]='Final Internal',t.*
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

CREATE NONCLUSTERED INDEX index_#CFAInspectionRecord_OrderSEQ ON #CFAInspectionRecord_OrderSEQ([OrderID],[SEQ]);

SELECT * 
INTO #CFAInspectionRecord
FROM CFAInspectionRecord c
WHERE ID IN(
	SELECT ID 
	FROM #CFAInspectionRecord_OrderSEQ
)	 

CREATE NONCLUSTERED INDEX index_#CFAInspectionRecord ON #CFAInspectionRecord([ID],[Stage],[Status]);

");
			#region PowerBI
			string sqlBI = $@"
select nb = ROW_NUMBER() over(Partition by ID order by iif(Stage in ('Final' ,'Final Internal'),1,0) desc,AuditDate desc,EditDate desc,AddDate desc)
,*
into #tmpFinal
from 
(
	/*-----Final-----*/
	select [Stage] = 'Final'
	,c.AuditDate
	,[InspResult]=CASE WHEN NOT EXISTS(
					SELECT 1 
					FROM #CFAInspectionRecord cr 
					INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
					WHERE cfoq.OrderID = n.ID AND cfoq.Seq = n.Seq AND cr.Stage = 'Final' AND cr.Status = 'Confirmed' ) 
				THEN ''
				WHEN EXISTS(						
					SELECT 1
					FROM #NeedCkeck a
					INNER JOIN Order_QtyShip oq ON oq.ID = a.Id ANd oq.Seq =a.Seq
					WHERE a.Stage = 'Final' 
					AND oq.CFAFinalInspectResult  != 'Pass'
					AND a.Id = n.Id AND a.Seq = n.Seq
				)THEN 'Fail'
				ELSE ''
				END
	,[NotYetInspCtn#] = ''
	,[NotYetInspCtn]  = ''
	,[Notyetinspqty]  = 0
	,[FailCtn#]=(
		SELECt TOP 1  cfoq.Carton
		FROM #CFAInspectionRecord  cr
		INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
		WHERE cr.Stage = 'Final' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
		AND cfoq.OrderID=n.ID AND cfoq.SEQ=n.Seq
		ORDER BY cr.AuditDate DESC, cr.EditDate DESC
	)
	,[FailCtn]=(	
		SELECT COUNT(DISTINCT data)
		FROM dbo.SplitString((
						SELECt TOP 1  cfoq.Carton
						FROM #CFAInspectionRecord  cr
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cr.Stage = 'Final' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
						AND cfoq.OrderID=n.ID AND cfoq.SEQ=n.Seq
						ORDER BY cr.AuditDate DESC, cr.EditDate DESC
		),',')
	)
	,[FailQty] = 0
	,n.MDivisionID
	,n.FactoryID
	,n.BuyerDelivery
	,n.BrandID
	,n.ID
	,n.Category 
	,n.OrderTypeID
	,n.CustPoNo
	,n.StyleID
	,n.StyleName
	,n.SeasonID
	,n.[Dest]
	,n.Customize1
	,n.CustCDID
	,n.Seq
	,n.ShipModeID
	,n.[ColorWay]
	,n.SewLine
	,n.[TtlCtn]
	,n.[StaggeredCtn]
	,n.[ClogCtn] 
	,n.[ClogCtn%]
	,n.[LastCartonReceivedDate]
	,n.CFAFinalInspectDate
	,n.CFA3rdInspectDate
	,n.CFARemark
	,c.EditDate ,c.AddDate
	FROM #tmp n
	left join #CFAInspectionRecord_OrderSEQ co on co.OrderID = n.ID and co.SEQ = n.SEQ
	left join #CFAInspectionRecord c on c.id = co.id
	WHERE c.Stage = 'Final'

	union all

	/*-----Final Internal-----*/
	select [Stage] = 'Final Internal'
	,c.AuditDate
	,[InspResult]=CASE WHEN NOT EXISTS(
					SELECT 1 
					FROM #CFAInspectionRecord cr 
					INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
					WHERE cfoq.OrderID = n.ID AND cfoq.Seq = n.Seq AND cr.Stage = 'Final Internal' AND cr.Status = 'Confirmed' ) 
				THEN ''
				WHEN EXISTS(						
					SELECT 1
					FROM #NeedCkeck a
					INNER JOIN Order_QtyShip oq ON oq.ID = a.Id ANd oq.Seq =a.Seq
					WHERE a.Stage = 'Final Internal' 
					AND oq.CFAFinalInspectResult  != 'Pass'
					AND a.Id = n.Id AND a.Seq = n.Seq
				)THEN 'Fail'
				ELSE ''
				END
	,[NotYetInspCtn#] = ''
	,[NotYetInspCtn]  = ''
	,[Notyetinspqty]  = 0
	,[FailCtn#]=(
		SELECt TOP 1  cfoq.Carton
		FROM #CFAInspectionRecord  cr
		INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
		WHERE cr.Stage = 'Final Internal' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
		AND cfoq.OrderID=n.ID AND cfoq.SEQ=n.Seq
		ORDER BY cr.AuditDate DESC, cr.EditDate DESC
	)
	,[FailCtn]=(	
		SELECT COUNT(DISTINCT data)
		FROM dbo.SplitString((
						SELECt TOP 1  cfoq.Carton
						FROM #CFAInspectionRecord  cr
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cr.Stage = 'Final Internal' AND cr.Status='Confirmed' AND cr.Result = 'Fail'
						AND cfoq.OrderID=n.ID AND cfoq.SEQ=n.Seq
						ORDER BY cr.AuditDate DESC, cr.EditDate DESC
		),',')
	)
	,[FailQty] = 0
	,n.MDivisionID
	,n.FactoryID
	,n.BuyerDelivery
	,n.BrandID
	,n.ID
	,n.Category 
	,n.OrderTypeID
	,n.CustPoNo
	,n.StyleID
	,n.StyleName
	,n.SeasonID
	,n.[Dest]
	,n.Customize1
	,n.CustCDID
	,n.Seq
	,n.ShipModeID
	,n.[ColorWay]
	,n.SewLine
	,n.[TtlCtn]
	,n.[StaggeredCtn]
	,n.[ClogCtn] 
	,n.[ClogCtn%]
	,n.[LastCartonReceivedDate]
	,n.CFAFinalInspectDate
	,n.CFA3rdInspectDate
	,n.CFARemark
	,c.EditDate ,c.AddDate
	FROM #tmp n
	left join #CFAInspectionRecord_OrderSEQ co on co.OrderID = n.ID and co.SEQ = n.SEQ
	left join #CFAInspectionRecord c on c.id = co.id
	WHERE c.Stage = 'Final Internal'

	union all
	/*-----Staggered-----*/
	select [Stage] = 'Stagger'
	,c.AuditDate
	,[InspResult]=CASE WHEN EXISTS(
						-- Result  - Fail的情況
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN #PackingList_Detail pd ON pd.OrderID = a.Id ANd pd.OrderShipmodeSeq =a.Seq
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cfo.OrderID = pd.OrderID AND cfo.SEQ = pd.OrderShipmodeSeq
						INNER JOIN #CFAInspectionRecord cf ON cf.ID = cfo.ID
						WHERE cf.Stage = 'Stagger' AND a.Category != 'Sample' AND cf.Result = 'Fail' AND cf.Status ='Confirmed'
						AND a.Id = n.Id AND a.Seq = n.Seq
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
				AND a.Id = n.Id AND a.Seq = n.Seq
			)
		THEN ''
	ELSE ''
	END
	,[NotYetInspCtn#]=(SELECT STUFF((
						SELECT DISTINCT ','+CTNStartNo
						FROM #PackingList_Detail pd
						WHERE pd.OrderID=n.ID AND pd.OrderShipmodeSeq=n.Seq
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
						WHERE pd.OrderID=n.ID AND pd.OrderShipmodeSeq=n.Seq
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
		,[Notyetinspqty] = (
		SELECT isnull(SUM(pd.ShipQty),0)
		FROM #PackingList_Detail pd
		WHERE pd.OrderID=n.ID AND pd.OrderShipmodeSeq=n.Seq
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
	,[FailCtn#]=(SELECT STUFF((
						SELECT DISTINCT ','+CTNStartNo
						FROM #PackingList_Detail pd
						WHERE pd.OrderID=n.ID AND pd.OrderShipmodeSeq=n.Seq
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
						WHERE pd.OrderID=n.ID AND pd.OrderShipmodeSeq=n.Seq
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
	,[FailQty] =(
			SELECT isnull(sum(pd.ShipQty),0)
			FROM #PackingList_Detail pd
			WHERE pd.OrderID=n.ID AND pd.OrderShipmodeSeq=n.Seq
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
	,n.MDivisionID
	,n.FactoryID
	,n.BuyerDelivery
	,n.BrandID
	,n.ID
	,n.Category 
	,n.OrderTypeID
	,n.CustPoNo
	,n.StyleID
	,n.StyleName
	,n.SeasonID
	,n.[Dest]
	,n.Customize1
	,n.CustCDID
	,n.Seq
	,n.ShipModeID
	,n.[ColorWay]
	,n.SewLine
	,n.[TtlCtn]
	,n.[StaggeredCtn]
	,n.[ClogCtn] 
	,n.[ClogCtn%]
	,n.[LastCartonReceivedDate]
	,n.CFAFinalInspectDate
	,n.CFA3rdInspectDate
	,n.CFARemark 
	,c.EditDate ,c.AddDate
	FROM #tmp n
	left join #CFAInspectionRecord_OrderSEQ co on co.OrderID = n.ID and co.SEQ = n.SEQ
	left join #CFAInspectionRecord c on c.id = co.id
	WHERE c.Stage = 'Stagger' AND n.Category != 'Sample'	
) a

select 
		[Stage],[InspResult],[NotYetInspCtn#],[NotYetInspCtn],[FailCtn#],[FailCtn],[Notyetinspqty]
		,[FailQty],MDivisionID,FactoryID,BuyerDelivery,BrandID,ID,Category ,OrderTypeID
		,CustPoNo,StyleID,StyleName,SeasonID,[Dest],Customize1,CustCDID,Seq,ShipModeID
		,[ColorWay],SewLine,[TtlCtn],[StaggeredCtn],[ClogCtn] ,[ClogCtn%],[LastCartonReceivedDate]
		,CFAFinalInspectDate,CFA3rdInspectDate,CFARemark 
		,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
		,[BIInsertDate] = GETDATE()   
		from #tmpFinal
where nb=1
";
			#endregion
			#region Outstanding WHERE
			List<string> outstandingWHERE = new List<string>();

            if (MyUtility.Check.Empty(model.InspStaged) || model.InspStaged == "Stagger")
            {
                string stageSql = $@"
/*-----Staggered-----*/
SELECT need.[Stage]
	,[InspResult]=CASE WHEN EXISTS(
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
,need.MDivisionID
,need.FactoryID
,need.BuyerDelivery
,need.BrandID
,need.ID
,need.Category 
,need.OrderTypeID
,need.CustPoNo
,need.StyleID
,need.StyleName
,need.SeasonID
,need.[Dest]
,need.Customize1
,need.CustCDID
,need.Seq
,need.ShipModeID
,need.[ColorWay]
,need.SewLine
,need.[TtlCtn]
,need.[StaggeredCtn]
,need.[ClogCtn] 
,need.[ClogCtn%]
,need.[LastCartonReceivedDate]
,need.CFAFinalInspectDate
,need.CFA3rdInspectDate
,need.CFARemark
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

            if (MyUtility.Check.Empty(model.InspStaged) || model.InspStaged == "Final" || model.InspStaged == "Final Internal")
            {
                string stageSql = $@"

/*-----Final-----*/
SELECT need.[Stage]
	,[InspResult]=CASE WHEN NOT EXISTS(
						SELECT 1 
						FROM #CFAInspectionRecord cr 
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cfoq.OrderID = need.ID AND cfoq.Seq = need.Seq AND cr.Stage in ('Final' ,'Final Internal') AND cr.Status = 'Confirmed' ) 
					THEN ''
					WHEN EXISTS(						
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN Order_QtyShip oq ON oq.ID = a.Id ANd oq.Seq =a.Seq
						WHERE a.Stage in ('Final' ,'Final Internal')
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
		WHERE cr.Stage in ('Final' ,'Final Internal') AND cr.Status='Confirmed' AND cr.Result = 'Fail'
		AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
		ORDER BY cr.AuditDate DESC, cr.EditDate DESC
	)
	,[FailCtn]=(	
		SELECT COUNT(DISTINCT data)
		FROM dbo.SplitString((
						SELECt TOP 1  cfoq.Carton
						FROM #CFAInspectionRecord  cr
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cr.Stage in ('Final' ,'Final Internal') AND cr.Status='Confirmed' AND cr.Result = 'Fail'
						AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
						ORDER BY cr.AuditDate DESC, cr.EditDate DESC
		),',')
	)
,need.MDivisionID
,need.FactoryID
,need.BuyerDelivery
,need.BrandID
,need.ID
,need.Category 
,need.OrderTypeID
,need.CustPoNo
,need.StyleID
,need.StyleName
,need.SeasonID
,need.[Dest]
,need.Customize1
,need.CustCDID
,need.Seq
,need.ShipModeID
,need.[ColorWay]
,need.SewLine
,need.[TtlCtn]
,need.[StaggeredCtn]
,need.[ClogCtn] 
,need.[ClogCtn%]
,need.[LastCartonReceivedDate]
,need.CFAFinalInspectDate
,need.CFA3rdInspectDate
,need.CFARemark
FROM #NeedCkeck need
WHERE need.Stage in ('Final' ,'Final Internal')
AND NOT EXISTS (	
	SELECT *
	FROM #CFAInspectionRecord a
	INNER JOIN #CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
	WHERE b.OrderID =need.ID AND b.SEQ = b.SEQ AND a.Stage in ('Final' ,'Final Internal') AND a.Status='Confirmed' AND (a.Result = 'Pass' OR a.Result='Fail but release')
) 

";
                outstandingWHERE.Add(stageSql);
            }

            if (MyUtility.Check.Empty(model.InspStaged) || model.InspStaged == "3rd party")
            {
                string stageSql = $@"

/*-----3rd Party-----*/
SELECT need.[Stage]
	,[InspResult]=CASE WHEN NOT EXISTS(
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
,need.MDivisionID
,need.FactoryID
,need.BuyerDelivery
,need.BrandID
,need.ID
,need.Category 
,need.OrderTypeID
,need.CustPoNo
,need.StyleID
,need.StyleName
,need.SeasonID
,need.[Dest]
,need.Customize1
,need.CustCDID
,need.Seq
,need.ShipModeID
,need.[ColorWay]
,need.SewLine
,need.[TtlCtn]
,need.[StaggeredCtn]
,need.[ClogCtn] 
,need.[ClogCtn%]
,need.[LastCartonReceivedDate]
,need.CFAFinalInspectDate
,need.CFA3rdInspectDate
,need.CFARemark
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
			if (model.IsPowerBI == true)
			{
                 sqlCmd.Append(sqlBI + Environment.NewLine + "DROP TABLE #tmp,#NeedCkeck,#PackingList_Detail,#CFAInspectionRecord,#CFAInspectionRecord_OrderSEQ,#tmpFinal");
            }
			else
			{
                sqlCmd.Append(outstandingWHERE.JoinToString("UNION") + Environment.NewLine + "DROP TABLE #tmp,#NeedCkeck,#PackingList_Detail,#CFAInspectionRecord,#CFAInspectionRecord_OrderSEQ");
            }

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlCmd.ToString(), listPar, out DataTable dt),
            };

            resultReport.Dt = dt;
            return resultReport;
        }

        /// <summary>
        /// GetCFAMasterListReport
        /// </summary>
        /// <param name="par">par</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel GetCFAMasterListReport(QA_R31_ViewModel par)
        {
            Base_ViewModel base_ViewModel = new Base_ViewModel();
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();

            #region SQL
            string tablePackinglist_Detail = "PackingList_Detail";
            if (par.IsPowerBI)
            {
                sqlCmd.Append($@"
declare @BIFilterDate date = '{par.BIFilterDate.Value.ToString("yyyy/MM/dd")}'

select * into #BIOrderTmp
from 
(
select [OrderID] = ID from Orders with (nolock) where Adddate >= @BIFilterDate or EditDate >= @BIFilterDate
UNION
select distinct [OrderID] = ID from Order_QtyShip with (nolock) where Adddate >= @BIFilterDate or EditDate >= @BIFilterDate
UNION
select distinct cro.OrderID
from CFAInspectionRecord cr with (nolock)
inner join CFAInspectionRecord_OrderSEQ cro with (nolock) on cr.ID = cro.ID
where cr.Adddate >= @BIFilterDate or cr.EditDate >= @BIFilterDate
UNION
select distinct pd.OrderID
from PackingList p with (nolock)
inner join PackingList_Detail pd with (nolock) on pd.ID = p.ID
where p.Adddate >= @BIFilterDate or p.EditDate >= @BIFilterDate
UNION
select OrderID from ClogReceive with (nolock) where Adddate >= @BIFilterDate
UNION
select OrderID from ClogReturn with (nolock) where Adddate >= @BIFilterDate
UNION
select OrderID from CFAReturn with (nolock) where Adddate >= @BIFilterDate
UNION
select distinct sd.OrderID
from SewingOutput s with (nolock)
inner join SewingOutput_Detail sd with (nolock) on s.ID = sd.ID
where s.Adddate >= @BIFilterDate or s.EditDate >= @BIFilterDate) a

select * into #pack
from PackingList_Detail with (nolock) where OrderID in (select OrderID from #BIOrderTmp)
");
                tablePackinglist_Detail = "#pack";
            }

            sqlCmd.Append($@"

SELECT  
oq.CFAFinalInspectResult
,[CFAIs3rdInspect] = IIF(oq.CFAIs3rdInspect = 1, 'Y','N')
,oq.CFA3rdInspectResult 
,o.MDivisionID
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
,[Dest] = isnull(c.Alias, '')
,o.Customize1
,o.CustCDID
,oq.Seq
,oq.ShipModeID
,[ColorWay] = isnull(Articles.Val, '')
,o.SewLine
,oq.Qty
,[StaggeredOutput] = ISNULL(StaggeredOutput.Val, 0)
,[CMPoutput] = ISNULL(CMPoutput.Val, 0)
,[CMPOutputPercent] = IIF(
						oq.Qty = 0 OR CMPoutput.Val =  'N/A' 
						, 'N/A' 
						, Cast(  CAST(  ROUND( CAST( ISNULL(CMPoutput.Val,0) as int) * 1.0  / oq.Qty * 100 ,0) as int)  as varchar)  
					)
,[ClogReceivedQty] =IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(ClogReceivedQty.Val,0) as varchar)  )
,[ClogReceivedQtyPercent]=IIF( oq.Qty = 0 OR o.Category ='S' 
						,'N/A' 
						,Cast(CAST(  ROUND( (CAST( ISNULL(ClogReceivedQty.Val,0) as int) * 1.0 / oq.Qty * 100 ),0) as int)  as varchar)  
					)
,[TtlCtn] =  IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(TtlCtn.Val,0) as varchar)  )
,[StaggeredCtn] = IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(StaggeredCtn.Val,0) as varchar)  )
,[ClogCtn] = IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(ClogCtn.Val,0) as varchar)  )
,[ClogCtnPercent]= IIF( ClogCtn.Val = 0 OR o.Category ='S'
					, 'N/A' 
					, CAST( CAST(ROUND((TtlCtn.Val *1.0 / TtlCtn.Val * 100),0) as int)  as varchar)  
				)
,[LastCartonReceivedDate] = LastReceived.Val
,oq.CFAFinalInspectDate
,oq.CFA3rdInspectDate
,oq.CFARemark
{(par.IsPowerBI ? ",[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), [BIInsertDate] = GETDATE() " : string.Empty)}
FROM Order_QtyShip oq with (nolock)
INNER JOIN Orders o with (nolock) ON o.ID = oq.Id and exists (select 1 from Factory f where o.FactoryId = id and f.IsProduceFty = 1)
INNER JOIN Factory f with (nolock) ON o.FactoryID = f.ID
INNER JOIN Style s with (nolock) ON o.StyleUkey = s.Ukey
LEFT JOIN OrderType ot with (nolock) ON o.OrderTypeID = ot.ID AND o.BrandID = ot.BrandID
LEFT JOIN Country c with (nolock) ON o.Dest = c.ID
OUTER APPLY(
	SELECT [Val]=STUFF((
	SELECT DISTINCT ',' + Article
	FROM Order_QtyShip_Detail oqd with (nolock)
	WHERE oqd.ID = oq.Id ANd oqd.Seq = oq.Seq
	FOR XML PATH('')
	) ,1,1,'')
)Articles
OUTER APPLY(
	SELECT [Val]=Sum(pd.ShipQty)
	From {tablePackinglist_Detail} pd with (nolock)
	INNER JOIN CFAInspectionRecord cfa with (nolock) on pd.StaggeredCFAInspectionRecordID	 = cfa.ID
	Where cfa.Status='Confirmed' and cfa.Stage='Stagger' and pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
)StaggeredOutput
OUTER APPLY(
	SELECT [Val] = IIF( (SELECT COUNT(oq2.Seq) FROM Order_QtyShip oq2 with (nolock) WHERE oq2.ID = oq.ID) > 1
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
	FROM {tablePackinglist_Detail} pd with (nolock)
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq= oq.Seq
)ClogReceivedQty
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM {tablePackinglist_Detail} pd with (nolock)
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq  = oq.Seq AND pd.CTNQty = 1
)TtlCtn
OUTER APPLY(
	SELECT [Val]=Count(DISTINCT pd.CTNStartNo)
	From {tablePackinglist_Detail} pd with (nolock)
	INNER JOIN CFAInspectionRecord CFA with (nolock) on pd.StaggeredCFAInspectionRecordID=CFA.ID
	Where CFA.Status='Confirmed' 
	AND CFA.Stage='Stagger'
	AND pd.CTNQty=1
	AND pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
)StaggeredCtn
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM {tablePackinglist_Detail} pd with (nolock)
	where pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
	AND pd.CTNQty=1
	AND (pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL)
)ClogCtn
OUTER APPLY(
	SELECT [Val] = MAX(pd.ReceiveDate) 
	FROM {tablePackinglist_Detail} pd with (nolock)
	WHERE pd.OrderID = oq.Id AND pd.OrderShipmodeSeq  = oq. Seq
	AND NOT exists (
		-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
		select 1 
		from {tablePackinglist_Detail} pdCheck with (nolock)
		where pd.OrderID = pdCheck.OrderID 
				and pd.OrderShipmodeSeq = pdCheck.OrderShipmodeSeq
				and pdCheck.ReceiveDate is null
	)
)LastReceived 
WHERE 1=1
");
            #endregion

            #region Where
            if (par.IsPowerBI)
            {
                sqlCmd.Append(" and oq.ID in (select OrderID from #BIOrderTmp) ");
            }

            if (!MyUtility.Check.Empty(par.BuyerDelivery1))
            {
                sqlCmd.Append($"AND oq.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                paramList.Add(new SqlParameter("@Buyerdelivery1", par.BuyerDelivery1.Value));
                paramList.Add(new SqlParameter("@BuyerDelivery2", par.BuyerDelivery2.Value));
            }

            if (!MyUtility.Check.Empty(par.SP1))
            {
                sqlCmd.Append($"AND oq.ID >= @sp1" + Environment.NewLine);
                SqlParameter p = new SqlParameter("@sp1", SqlDbType.VarChar)
                {
                    Value = par.SP1,
                };
                paramList.Add(p);
            }

            if (!MyUtility.Check.Empty(par.SP2))
            {
                sqlCmd.Append($"AND oq.ID <= @sp2" + Environment.NewLine);
                SqlParameter p = new SqlParameter("@sp2", SqlDbType.VarChar)
                {
                    Value = par.SP2,
                };
                paramList.Add(p);
            }

            if (!MyUtility.Check.Empty(par.MDivisionID))
            {
                sqlCmd.Append($"AND o.MDivisionID=@MDivisionID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@MDivisionID", par.MDivisionID));
            }

            if (!MyUtility.Check.Empty(par.FactoryID))
            {
                sqlCmd.Append($"AND o.FtyGroup=@FactoryID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@FactoryID", par.FactoryID));
            }

            if (!MyUtility.Check.Empty(par.Brand))
            {
                sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                paramList.Add(new SqlParameter("@Brand", par.Brand));
            }

            if (par.Exclude_Sister_Transfer_Out)
            {
                sqlCmd.Append($"AND f.IsProduceFty = 1" + Environment.NewLine);
            }

            if (par.CategoryList.Count > 0)
            {
                sqlCmd.Append($"AND o.Category IN ('{par.CategoryList.JoinToString("','")}') " + Environment.NewLine);
            }
            else
            {
                // 如果全部勾選，則無資料
                sqlCmd.Append($"AND 1=0 " + Environment.NewLine);
            }

            #endregion

            DataTable dtResult;

            base_ViewModel.Result = this.DBProxy.Select("Production", sqlCmd.ToString(), paramList, out dtResult);
            if (!base_ViewModel.Result)
            {
                return base_ViewModel;
            }

            // 抓取BI需要刪除的資料
            // OrderComparisonList抓出來的是order有被刪除的紀錄
            // 如果還串的到Order_QtyShip，表示該Seq須被保留
            if (par.IsPowerBI)
            {
                DataTable dtP_QA_CFAMasterListNeedDeleteFilterBase;
                string sqlGetNeedDeleteFilterBase = $@"
		select distinct ocl.OrderID, oqs.Seq
		from OrderComparisonList ocl with (nolock)
		left join Order_QtyShip oqs with (nolock) on ocl.OrderId = oqs.Id
		where	ocl.DeleteOrder = 1	and ocl.UpdateDate >= '{par.BIFilterDate.Value.ToString("yyyy/MM/dd")}'
";
                base_ViewModel.Result = this.DBProxy.Select("Production", sqlGetNeedDeleteFilterBase, out dtP_QA_CFAMasterListNeedDeleteFilterBase);
                if (!base_ViewModel.Result)
                {
                    return base_ViewModel;
                }

                base_ViewModel.DtArr = new DataTable[] { dtResult, dtP_QA_CFAMasterListNeedDeleteFilterBase };
            }

            base_ViewModel.Dt = dtResult;

            return base_ViewModel;
        }
    }
}
