-- =============================================
-- Description:	Import PMS system QA R31 to PowerBI
-- =============================================
CREATE PROCEDURE [dbo].[P_Import_QA_R31]
	@LinkServerName varchar(50)
AS
BEGIN
	DECLARE @SqlCmd_Combin nvarchar(max) =''
	DECLARE @SqlCmd1 nvarchar(max) ='';
	DECLARE @SqlCmd2 nvarchar(max) ='';
	DECLARE @SqlCmd3 nvarchar(max) ='';
	DECLARE @SqlCmd4 nvarchar(max) ='';
	DECLARE @SqlCmd5 nvarchar(max) ='';
	DECLARE @SqlCmd6 nvarchar(max) ='';
	DECLARE @SqlCmd7 nvarchar(max) ='';
	DECLARE @SqlCmd8 nvarchar(max) ='';
	DECLARE @SqlCmd9 nvarchar(max) ='';


	DECLARE @SDate_varchar varchar(10) = cast( (select CONVERT(date, DATEADD(DAY,-60, GETDATE())))  as varchar)
	DECLARE @EDate_varchar varchar(10) = cast( (select CONVERT(date, DATEADD(DAY,30, GETDATE())))  as varchar)

	SET @SqlCmd1 = '
SELECT  
o.MDivisionID
,o.FactoryID
,oq.BuyerDelivery
,o.BrandID
,oq.ID
,Category = CASE   WHEN o.Category=''B'' THEN ''Bulk''
                   WHEN o.Category=''S'' THEN ''Sample''
                   WHEN o.Category=''G'' THEN ''Garment''
                   ELSE ''''
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
,[TtlCtn] =  IIF(o.Category = ''S'' ,''N/A''  ,Cast( ISNULL(TtlCtn.Val,0) as varchar)  )
,[StaggeredCtn] = IIF(o.Category =''S'' ,''N/A''  ,Cast( ISNULL(StaggeredCtn.Val,0) as varchar)  )
,[ClogCtn] = IIF(o.Category = ''S'' ,''N/A''  ,Cast( ISNULL(ClogCtn.Val,0) as varchar)  )
,[ClogCtn%]= IIF( ClogCtn.Val = 0 OR o.Category =''S''
					, ''N/A'' 
					, CAST( CAST(ROUND((TtlCtn.Val *1.0 / TtlCtn.Val * 100),0) as int)  as varchar)  
				)
,[Lastcartonreceiveddate] = LastReceived.Val
,oq.CFAFinalInspectDate
,oq.CFA3rdInspectDate
,oq.CFARemark
INTO #tmp
FROM ['+@LinkServerName+'].Production.dbo.Order_QtyShip oq
INNER JOIN ['+@LinkServerName+'].Production.dbo.Orders o ON o.ID = oq.Id
LEFT  JOIN ['+@LinkServerName+'].Production.dbo.OrderType ot ON o.OrderTypeID = ot.ID AND o.BrandID = ot.BrandID
INNER JOIN ['+@LinkServerName+'].Production.dbo.Factory f ON o.FactoryID = f.ID
LEFT  JOIN ['+@LinkServerName+'].Production.dbo.Country c ON o.Dest = c.ID
INNER JOIN ['+@LinkServerName+'].Production.dbo.Style s ON o.StyleID=s.ID AND s.SeasonID = o.SeasonID
OUTER APPLY(
	SELECT [Val]=STUFF((
	SELECT DISTINCT '','' + Article
	FROM ['+@LinkServerName+'].Production.dbo.Order_QtyShip_Detail oqd
	WHERE oqd.ID = oq.Id ANd oqd.Seq = oq.Seq
	FOR XML PATH('''')
	) ,1,1,'''')
)Articles
OUTER APPLY(
	SELECT [Val]=Sum(pd.ShipQty)
	From ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd
	INNER JOIN ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord cfa on pd.StaggeredCFAInspectionRecordID	 = cfa.ID
	Where cfa.Status=''Confirmed'' and cfa.Stage=''Stagger'' and pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
)StaggeredOutput
OUTER APPLY(
	SELECT [Val] = 
	IIF( (SELECT COUNT(oq2.Seq) FROM ['+@LinkServerName+'].Production.dbo.Order_QtyShip oq2 WHERE oq2.ID = oq.ID) > 1
			,''N/A''
			, Cast(
				(
					select minSeqQty = MIN(a.QAQty)
					from (
						select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
						from ['+@LinkServerName+'].Production.dbo.Order_Location sl WITH (NOLOCK)
						left join ['+@LinkServerName+'].Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = oq.ID and sdd.ComboType = sl.Location
						where sl.OrderID = oq.ID
						group by sl.Location
					) a
				) as varchar
			)
		)
)CMPoutput
OUTER APPLY(
	SELECT [Val] = ISNULL( SUM(IIF
				( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL
				,pd.ShipQty
				,0)
			),0)
	FROM ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq= oq.Seq
)ClogReceivedQty
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq  = oq.Seq AND pd.CTNQty = 1
)TtlCtn
OUTER APPLY(
	SELECT [Val]=Count(DISTINCT pd.CTNStartNo)
	From ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd 
	INNER JOIN ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord CFA on pd.StaggeredCFAInspectionRecordID=CFA.ID
	Where CFA.Status=''Confirmed'' 
	AND CFA.Stage=''Stagger''
	AND pd.CTNQty=1
	AND pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
)StaggeredCtn
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd 
	where pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
	AND pd.CTNQty=1
	AND (pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL)
)ClogCtn

	'

	SET @SqlCmd2 = '
OUTER APPLY(
	SELECT [Val] = MAX(pd.ReceiveDate) 
	FROM ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd
	WHERE pd.OrderID = oq.Id AND pd.OrderShipmodeSeq  = oq. Seq
	AND NOT exists (
		-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
		select 1 
		from ['+@LinkServerName+'].Production.dbo.PackingList_Detail pdCheck
		where pd.OrderID = pdCheck.OrderID 
				and pd.OrderShipmodeSeq = pdCheck.OrderShipmodeSeq
				and pdCheck.ReceiveDate is null
	)
)LastReceived 
WHERE 1=1
AND oq.BuyerDelivery BETWEEN '''+@SDate_varchar+''' and '''+@EDate_varchar+'''
AND f.IsProduceFty = 1
AND o.Category IN (''B'',''G'') 

	----戴上要檢驗的Stage帽子
SELECT DISTINCT [Stage]=''Stagger'',t.*
INTO #NeedCkeck
FROM  #tmp t 
UNION 
SELECT DISTINCT [Stage]=''Final'',t.*
FROM  #tmp t 
UNION 
SELECT DISTINCT [Stage]=''Final Internal'',t.*
FROM  #tmp t 
UNION 
SELECT DISTINCT [Stage]=''3rd party'',t.*
FROM  #tmp t 
INNER JOIN ['+@LinkServerName+'].Production.dbo.Order_QtyShip oq ON oq.Id = t.Id AND oq.Seq = t.Seq
WHERE oq.CFAIs3rdInspect = 1


----取出需要的資料
SELECT *
INTO #PackingList_Detail
FROM ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd
WHERE EXISTS(
	SELECT 1 
	FROM #NeedCkeck n
	WHERE n.ID = pd.OrderID AND n.Seq = pd.OrderShipmodeSeq
)

SELECT * 
INTO #CFAInspectionRecord_OrderSEQ 
FROM ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ co
WHERE EXISTS(
	SELECT 1 
	FROM #NeedCkeck n
	WHERE co.OrderID = n.Id AND co.SEQ = n.Seq
)

SELECT * 
INTO #CFAInspectionRecord
FROM ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord c
WHERE ID IN(
	SELECT ID 
	FROM #CFAInspectionRecord_OrderSEQ
)
	'

	SET @SqlCmd3 = '
/*-----Stagger-----*/
select * into #tmpFinal from (
SELECT [InspResult]=CASE WHEN EXISTS(
								-- Result  - Fail的情況
								SELECT 1
								FROM #NeedCkeck a
								INNER JOIN #PackingList_Detail pd ON pd.OrderID = a.Id ANd pd.OrderShipmodeSeq =a.Seq
								INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cfo.OrderID = pd.OrderID AND cfo.SEQ = pd.OrderShipmodeSeq
								INNER JOIN #CFAInspectionRecord cf ON cf.ID = cfo.ID
								WHERE cf.Stage = ''Stagger'' AND a.Category != ''Sample'' AND cf.Result = ''Fail'' AND cf.Status =''Confirmed''
								AND a.Id = need.Id AND a.Seq = need.Seq
							)
						THEN ''Fail''	
					WHEN NOT EXISTS(
						-- Result  -  空白的情況
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN #PackingList_Detail pd ON pd.OrderID = a.Id ANd pd.OrderShipmodeSeq =a.Seq
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cfo.OrderID = pd.OrderID AND cfo.SEQ = pd.OrderShipmodeSeq
						INNER JOIN #CFAInspectionRecord cf ON cf.ID = cfo.ID
						WHERE cf.Stage = ''Stagger'' AND a.Category != ''Sample'' AND  cf.Status =''Confirmed''
						AND a.Id = need.Id AND a.Seq = need.Seq
					)
			 THEN ''''
		ELSE ''''
		END
	,[NotyetinspCtn#]=(SELECT STUFF((
								SELECT DISTINCT '',''+CTNStartNo
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND NOT EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage=''Stagger'' AND cf.Status=''Confirmed''
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo
									)
								) 
								FOR XML PATH('''')
							),1,1,''''))
	,[Notyetinspctn]=(
								SELECT COUNT(DISTINCT CTNStartNo)
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND NOT EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage=''Stagger'' AND cf.Status=''Confirmed''
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo
									)
								) )
	,[Notyetinspqty] = (
								SELECT isnull(SUM(pd.ShipQty),0)
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND NOT EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage=''Stagger'' AND cf.Status=''Confirmed''
									AND (	
										cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo
									)
								) 
							) '

	SET @SqlCmd4 = '
	,[FailCtn#]=(SELECT STUFF((
								SELECT DISTINCT '',''+CTNStartNo
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND pd.StaggeredCFAInspectionRecordID = ''''
								AND EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage=''Stagger'' AND cf.Status=''Confirmed'' AND cf.Result=''Fail''
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo
									)
								) 
								FOR XML PATH('''')
							),1,1,''''))
	,[FailCtn]=(
								SELECT COUNT(DISTINCT CTNStartNo)
								FROM #PackingList_Detail pd
								WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
								AND pd.StaggeredCFAInspectionRecordID = ''''
								AND EXISTS(
									SELECT 1
									FROM #CFAInspectionRecord cf
									INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
									WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
									AND cf.Stage=''Stagger'' AND cf.Status=''Confirmed'' AND cf.Result=''Fail''
									AND (	
											cfo.Carton = pd.CTNStartNo
										OR cfo.Carton LIKE  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo +'',%'' 
										OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo
									)
								) )
	,[FailQty] =(
					SELECT isnull(sum(pd.ShipQty),0)
					FROM #PackingList_Detail pd
					WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
					AND pd.StaggeredCFAInspectionRecordID = ''''
					AND EXISTS(
						SELECT 1
						FROM #CFAInspectionRecord cf
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
						WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
						AND cf.Stage=''Stagger'' AND cf.Status=''Confirmed'' AND cf.Result=''Fail''
						AND (	
								cfo.Carton = pd.CTNStartNo
							OR cfo.Carton LIKE  pd.CTNStartNo +'',%'' 
							OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo +'',%'' 
							OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo
						)
					) 
				)
	,need.*
FROM #NeedCkeck need
WHERE need.Stage = ''Stagger'' AND need.Category != ''Sample''	
AND (
		SELECT COUNT(DISTINCT CTNStartNo)
		FROM #PackingList_Detail pd
		WHERE pd.OrderID=need.ID AND pd.OrderShipmodeSeq=need.Seq
		AND EXISTS(
			SELECT 1
			FROM #CFAInspectionRecord cf
			INNER JOIN #CFAInspectionRecord_OrderSEQ cfo ON cf.ID = cfo.ID
			WHERE cfo.OrderID=pd.OrderID AND cfo.SEQ=pd.OrderShipmodeSeq
			AND cf.Stage=''Stagger'' AND cf.Status=''Confirmed'' AND cf.Result!=''Pass''
			AND (	
					cfo.Carton = pd.CTNStartNo
				OR cfo.Carton LIKE  pd.CTNStartNo +'',%'' 
				OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo +'',%'' 
				OR cfo.Carton LIKE ''%,''+  pd.CTNStartNo
			)
        )
) != 
(SELECT COUNT( DISTINCT CTNStartNo)
FROM #PackingList_Detail  
WHERE OrderID = need.ID AND OrderShipmodeSeq = need.Seq )

union	
	'

	SET @SqlCmd5 = '
/*-----Final-----*/
SELECT [InspResult]=CASE WHEN NOT EXISTS(
						SELECT 1 
						FROM #CFAInspectionRecord cr 
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cfoq.OrderID = need.ID AND cfoq.Seq = need.Seq AND cr.Stage = ''Final'' AND cr.Status = ''Confirmed'' ) 
					THEN ''''
					WHEN EXISTS(						
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN ['+@LinkServerName+'].Production.dbo.Order_QtyShip oq ON oq.ID = a.Id ANd oq.Seq =a.Seq
						WHERE a.Stage = ''Final'' 
						AND oq.CFAFinalInspectResult  != ''Pass''
						AND a.Id = need.Id AND a.Seq = need.Seq
					)THEN ''Fail''
					ELSE ''''
					END
	,[NotyetinspCtn#] = NULL
	,[Notyetinspctn]  = NULL
	,[Notyetinspqty]  = 0
	,[FailCtn#]=(
		SELECt TOP 1  cfoq.Carton
		FROM #CFAInspectionRecord  cr
		INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
		WHERE cr.Stage = ''Final'' AND cr.Status=''Confirmed'' AND cr.Result = ''Fail''
		AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
		ORDER BY cr.AuditDate DESC, cr.EditDate DESC
	)
	,[FailCtn]=(	
		SELECT COUNT(DISTINCT data)
		FROM dbo.SplitString((
						SELECt TOP 1  cfoq.Carton
						FROM #CFAInspectionRecord  cr
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cr.Stage = ''Final'' AND cr.Status=''Confirmed'' AND cr.Result = ''Fail''
						AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
						ORDER BY cr.AuditDate DESC, cr.EditDate DESC
		),'','')
	)
	,[FailQty] = 0
	,need.*
FROM #NeedCkeck need
WHERE need.Stage = ''Final''
AND NOT EXISTS (	
	SELECT *
	FROM #CFAInspectionRecord a
	INNER JOIN #CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
	WHERE b.OrderID =need.ID AND b.SEQ = b.SEQ AND a.Stage = ''Final'' AND a.Status=''Confirmed'' AND (a.Result = ''Pass'' OR a.Result=''Fail but release'')
) 
) a


DROP TABLE #tmp,#NeedCkeck,#PackingList_Detail,#CFAInspectionRecord,#CFAInspectionRecord_OrderSEQ
	'
	
	SET @SqlCmd6 = '
/*-----Final Internal-----*/
SELECT [InspResult]=CASE WHEN NOT EXISTS(
						SELECT 1 
						FROM #CFAInspectionRecord cr 
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cfoq.OrderID = need.ID AND cfoq.Seq = need.Seq AND cr.Stage = ''Final Internal'' AND cr.Status = ''Confirmed'' ) 
					THEN ''''
					WHEN EXISTS(						
						SELECT 1
						FROM #NeedCkeck a
						INNER JOIN ['+@LinkServerName+'].Production.dbo.Order_QtyShip oq ON oq.ID = a.Id ANd oq.Seq =a.Seq
						WHERE a.Stage = ''Final Internal'' 
						AND oq.CFAFinalInspectResult  != ''Pass''
						AND a.Id = need.Id AND a.Seq = need.Seq
					)THEN ''Fail''
					ELSE ''''
					END
	,[NotyetinspCtn#] = NULL
	,[Notyetinspctn]  = NULL
	,[Notyetinspqty]  = 0
	,[FailCtn#]=(
		SELECt TOP 1  cfoq.Carton
		FROM #CFAInspectionRecord  cr
		INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
		WHERE cr.Stage = ''Final Internal'' AND cr.Status=''Confirmed'' AND cr.Result = ''Fail''
		AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
		ORDER BY cr.AuditDate DESC, cr.EditDate DESC
	)
	,[FailCtn]=(	
		SELECT COUNT(DISTINCT data)
		FROM dbo.SplitString((
						SELECt TOP 1  cfoq.Carton
						FROM #CFAInspectionRecord  cr
						INNER JOIN #CFAInspectionRecord_OrderSEQ cfoq ON cr.ID = cfoq.ID
						WHERE cr.Stage = ''Final Internal'' AND cr.Status=''Confirmed'' AND cr.Result = ''Fail''
						AND cfoq.OrderID=need.ID AND cfoq.SEQ=need.Seq
						ORDER BY cr.AuditDate DESC, cr.EditDate DESC
		),'','')
	)
	,[FailQty] = 0
	,need.*
FROM #NeedCkeck need
WHERE need.Stage = ''Final Internal''
AND NOT EXISTS (	
	SELECT *
	FROM #CFAInspectionRecord a
	INNER JOIN #CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
	WHERE b.OrderID =need.ID AND b.SEQ = b.SEQ AND a.Stage = ''Final Internal'' AND a.Status=''Confirmed'' AND (a.Result = ''Pass'' OR a.Result=''Fail but release'')
) 
) a


DROP TABLE #tmp,#NeedCkeck,#PackingList_Detail,#CFAInspectionRecord,#CFAInspectionRecord_OrderSEQ
	'

	set @SqlCmd7 = '
	-----開始Merge 
	MERGE INTO PBIReportData.dbo.P_QA_R31_Original t
	USING #tmpFinal s 
	ON t.OrderID = s.ID AND t.Stage = s.Stage AND t.StyleName = s.StyleName and t.Seq = s.Seq
	WHEN MATCHED THEN   
	UPDATE SET 
		t.Stage =  s.Stage,
		t.InspResult =  s.InspResult,
		t.NotYetInspCtn# =  s.NotYetInspCtn#,
		t.NotYetInspCtn =  s.NotYetInspCtn,
		t.NotYetInspQty =  s.NotYetInspQty,
		t.FailCtn# =  s.FailCtn#,
		t.FailCtn =  s.FailCtn,
		t.FailQty =  s.FailQty,
		t.MDivisionID =  s.MDivisionID,
		t.FactoryID =  s.FactoryID,
		t.BuyerDelivery =  s.BuyerDelivery,
		t.BrandID =  s.BrandID,
		t.OrderID =  s.ID,
		t.Category =  s.Category,
		t.OrderTypeID =  s.OrderTypeID,
		t.CustPoNo =  s.CustPoNo,
		t.StyleID =  s.StyleID,
		t.StyleName =  s.StyleName,
		t.SeasonID =  s.SeasonID,
		t.Dest =  s.Dest,
		t.Customize1 =  s.Customize1,
		t.CustCDID =  s.CustCDID,
		t.Seq =  s.Seq,
		t.ShipModeID =  s.ShipModeID,
		t.ColorWay =  s.ColorWay,
		t.SewLine =  s.SewLine,
		t.TtlCtn =  s.TtlCtn,
		t.StaggeredCtn =  s.StaggeredCtn,
		t.ClogCtn =  s.ClogCtn,
		t.[ClogCtn%] =  s.[ClogCtn%],
		t.LastCartonReceivedDate =  s.LastCartonReceivedDate,
		t.CFAFinalInspectDate =  s.CFAFinalInspectDate,
		t.CFA3rdInspectDate =  s.CFA3rdInspectDate,
		t.CFARemark =  s.CFARemark
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (
		Stage,InspResult,NotYetInspCtn#,NotYetInspCtn,NotYetInspQty,FailCtn#,FailCtn,FailQty,MDivisionID,FactoryID,BuyerDelivery,BrandID,OrderID,Category
,OrderTypeID,CustPoNo,StyleID,StyleName,SeasonID,Dest,Customize1,CustCDID,Seq,ShipModeID,ColorWay,SewLine,TtlCtn,StaggeredCtn,ClogCtn,[ClogCtn%],LastCartonReceivedDate
,CFAFinalInspectDate,CFA3rdInspectDate,CFARemark)
		VALUES (
		s.Stage,s.InspResult,s.NotYetInspCtn#,s.NotYetInspCtn,s.NotYetInspQty,s.FailCtn#,s.FailCtn,s.FailQty,s.MDivisionID,s.FactoryID,s.BuyerDelivery,s.BrandID,s.ID,s.Category,
s.OrderTypeID,s.CustPoNo,s.StyleID,s.StyleName,s.SeasonID,s.Dest,s.Customize1,s.CustCDID,s.Seq,s.ShipModeID,s.ColorWay,s.SewLine,s.TtlCtn,s.StaggeredCtn,s.ClogCtn,s.[ClogCtn%],
s.LastCartonReceivedDate,s.CFAFinalInspectDate,s.CFA3rdInspectDate,s.CFARemark);


delete t 
from PBIReportData.dbo.P_QA_R31_Original t
left join #tmpFinal s on t.OrderID = s.ID AND t.Stage = s.Stage AND t.StyleName = s.StyleName and t.Seq = s.Seq
where s.ID is null
and T.BuyerDelivery BETWEEN '''+@SDate_varchar+''' and '''+@EDate_varchar+'''
and exists(
	select * from ['+@LinkServerName+'].Production.dbo.Factory  as f
	where f.ProduceM = f.MDivisionID
	and f.id = t.FactoryID
)

DROP TABLE #tmpFinal

	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.Id = ''P_QA_R31_Original''

'


SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3 + @SqlCmd4 + @SqlCmd5 + @SqlCmd6 + @SqlCmd7
EXEC sp_executesql @SqlCmd_Combin

--print @SqlCmd1
--print @SqlCmd2
--print @SqlCmd3
--print @SqlCmd4
--print @SqlCmd5
--print @SqlCmd6
--print @SqlCmd7

END