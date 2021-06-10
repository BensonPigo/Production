CREATE PROCEDURE [dbo].[P_Import_QA_R06]
	@WhseArrival_s Date,
	@WhseArrival_e Date = NULL,
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
	
	
	


	----若沒指定，則預設當月月底
	IF @WhseArrival_e IS NULL
	BEGIN
		SET @WhseArrival_e = (SELECT dateadd(day ,-1, dateadd(m, datediff(m,0,getdate())+1,0))  )
	END

	----
	DECLARE @WhseArrival_s_varchar varchar(10) = cast( @WhseArrival_s as varchar)
	DECLARE @WhseArrival_e_varchar varchar(10) = cast( @WhseArrival_e as varchar)
	
	SET @SqlCmd1='

	SELECT * INTO #View_AllReceivingDetail 	FROM ['+@LinkServerName+'].Production.dbo.View_AllReceivingDetail WHERE WhseArrival >= ''2019/01/01'';

	SELECT * INTO #SubTransfer FROM ['+@LinkServerName+'].Production.dbo.SubTransfer WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #BorrowBack FROM ['+@LinkServerName+'].Production.dbo.BorrowBack WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #Orders FROM ['+@LinkServerName+'].Production.dbo.Orders WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #Fabric FROM ['+@LinkServerName+'].Production.dbo.Fabric WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');
	
	SELECT * INTO #PO_Supp FROM ['+@LinkServerName+'].Production.dbo.PO_Supp WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #Supp FROM ['+@LinkServerName+'].Production.dbo.Supp ;--WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #FIR FROM ['+@LinkServerName+'].Production.dbo.FIR WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #FIR_Physical FROM ['+@LinkServerName+'].Production.dbo.FIR_Physical WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #Fir_shadebone FROM ['+@LinkServerName+'].Production.dbo.Fir_shadebone WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #Oven FROM ['+@LinkServerName+'].Production.dbo.Oven WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #ColorFastness FROM ['+@LinkServerName+'].Production.dbo.ColorFastness WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #Factory FROM ['+@LinkServerName+'].Production.dbo.Factory;

	SELECT * INTO #FIRSTDYELOT FROM ['+@LinkServerName+'].Production.dbo.FIRSTDYELOT

	SELECT * INTO #Season FROM ['+@LinkServerName+'].Production.dbo.Season ;

	SELECT * INTO #SeasonSCI FROM ['+@LinkServerName+'].Production.dbo.SeasonSCI ;

	SELECT * INTO #SuppLevel FROM ['+@LinkServerName+'].Production.dbo.SuppLevel ;

	SELECT * INTO #Inspweight FROM ['+@LinkServerName+'].Production.dbo.Inspweight;

	SELECT * INTO #FIR_Continuity FROM ['+@LinkServerName+'].Production.dbo.FIR_Continuity WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #FIR_Laboratory_Wash FROM ['+@LinkServerName+'].Production.dbo.FIR_Laboratory_Wash WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');

	SELECT * INTO #Export FROM ['+@LinkServerName+'].Production.dbo.Export WHERE (AddDate >= ''2019/01/01'' OR EditDate >= ''2019/01/01'');


	SELECT * INTO #SentReport FROM ['+@LinkServerName+'].Production.dbo.SentReport WHERE (InspectionReport >= ''2019/01/01'' OR 
		TPEInspectionReport >= ''2019/01/01'' OR
		TestReport >= ''2019/01/01'' OR
		TPETestReport >= ''2019/01/01'' OR
		ContinuityCard >= ''2019/01/01'' OR
		TPEContinuityCard >= ''2019/01/01'' OR
		EditDate >= ''2019/01/01''
	);




	SELECT * INTO #SubTransfer_Detail FROM ['+@LinkServerName+'].Production.dbo.SubTransfer_Detail
	WHERE ID IN (SELECT ID FROM　#SubTransfer) 

	SELECT * INTO #BorrowBack_Detail FROM ['+@LinkServerName+'].Production.dbo.BorrowBack_Detail
	WHERE ID IN (SELECT ID FROM #BorrowBack) 

	SELECT * INTO #PO_Supp_Detail FROM ['+@LinkServerName+'].Production.dbo.PO_Supp_Detail  a
	WHERE EXISTS(
		SELECT 1 FROM #PO_Supp b
		WHERE a.ID = b.ID AND a.SEQ1 = b.SEQ1
	)

	SELECT * INTO #Oven_Detail FROM ['+@LinkServerName+'].Production.dbo.Oven_Detail
	WHERE ID IN (SELECT ID FROM #Oven) 

	SELECT * INTO #Export_Detail FROM ['+@LinkServerName+'].Production.dbo.Export_Detail a
	WHERE EXISTS(
		SELECT 1 FROM ['+@LinkServerName+'].Production.dbo.Export b
		WHERE (b.AddDate >= ''2019/01/01'' OR b.EditDate >= ''2019/01/01'')
		AND a.ID = b.ID
	)

	SELECT * INTO #ColorFastness_Detail FROM ['+@LinkServerName+'].Production.dbo.ColorFastness_Detail
	WHERE ID IN (SELECT ID FROM #ColorFastness) 

	SELECT * INTO #FIR_Physical_Defect FROM ['+@LinkServerName+'].Production.dbo.FIR_Physical_Defect WHERE FIR_PhysicalDetailUKey IN (
		SELECT DetailUkey FROM #FIR_Physical
	)


	SELECT * INTO #FabricDefect FROM ['+@LinkServerName+'].Production.dbo.FabricDefect
	SELECT * INTO #FIR_Laboratory FROM ['+@LinkServerName+'].Production.dbo.FIR_Laboratory
	SELECT * INTO #FIR_Laboratory_Heat FROM ['+@LinkServerName+'].Production.dbo.FIR_Laboratory_Heat
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	';
	
	SET @SqlCmd2='
	select distinct a.PoId,a.Seq1,a.Seq2,ps.SuppID,psd.Refno ,psd.ColorID,f.Clima ,[WhseArrival]=LEFT(convert(varchar, a.WhseArrival , 111),7)
	,o.FactoryID
	into #tmp1
	from
	(
		select r.PoId,r.Seq1,r.Seq2 ,[WhseArrival] = r.WhseArrival
		from #View_AllReceivingDetail r with (nolock)
		where 1=1
		 and r.WhseArrival >= '''+@WhseArrival_s_varchar+''' and r.WhseArrival <= '''+@WhseArrival_e_varchar+'''
		and r.Status = ''Confirmed''
		union
		select sd.ToPOID as PoId,sd.ToSeq1 as Seq1,sd.ToSeq2 as Seq2 ,[WhseArrival] = s.IssueDate
		from #SubTransfer s
		inner join #SubTransfer_Detail sd on s.Id = sd.ID
		where 1=1
		 and s.IssueDate >= '''+@WhseArrival_s_varchar+''' and s.IssueDate <= '''+@WhseArrival_e_varchar+'''
		and s.Status = ''Confirmed''
		and s.Type = ''B''
		union
		select bd.ToPOID as PoId,bd.ToSeq1 as Seq1,bd.ToSeq2 as Seq2 ,[WhseArrival] = b.IssueDate
		from #BorrowBack b
		inner join #BorrowBack_Detail bd on b.Id = bd.ID
		where 1=1
		 and b.IssueDate >= '''+@WhseArrival_s_varchar+''' and b.IssueDate <= '''+@WhseArrival_e_varchar+'''
		and b.Status = ''Confirmed''
		and (b.Type = ''A'' or b.Type = ''B'')
	) a
	left join #Orders o on o.ID = a.PoId
	left join #PO_Supp ps on ps.ID = a.PoId and ps.SEQ1 = a.Seq1
	left join #PO_Supp_Detail psd on psd.ID = a.PoId and psd.SEQ1 = a.Seq1 and psd.SEQ2 = a.Seq2
	left join #Fabric f on f.SCIRefno = psd.SCIRefno 
	where psd.FabricType = ''F''

	
	------------Fabric Defect ----- 
	select DISTINCT rd.PoId,rd.Seq1,rd.Seq2,rd.ActualQty,rd.Dyelot,rd.Roll,t.SuppID,t.Refno,t.Colorid ,t.Clima  ,t.WhseArrival ,t.FactoryID
	into #tmpAllData
	from #tmp1 t
	inner join #View_AllReceivingDetail rd on t.PoId = rd.PoId and t.Seq1 = rd.Seq1 and t.Seq2 = rd.Seq2
	------------Group by Supp 
	select PoId,Seq1,Seq2,SuppID,WhseArrival,FactoryID,ActualQty = sum(ActualQty)  
	into #GroupBySupp
	from #tmpAllData
	group by PoId,Seq1,Seq2,SuppID,WhseArrival,FactoryID
	------------PhyscialEncode=1 group by Supp ---- 
	select distinct t.SuppID,t.PoId,t.Seq1,t.Seq2 
	into #tmpsuppEncode
	from #GroupBySupp t
	inner join #FIR f on t.PoId = f.POID and t.Seq1 = f.SEQ1 and t.Seq2 = f.SEQ2
	where f.PhysicalEncode = 1
	------------Count Total Orders# Group by PoID ---- 
	select t.SuppID,count(distinct o.ID)cnt 
	into #tmpCountSP
	from #tmpAllData t
	inner join #orders o on t.PoId=o.POID
	group by t.SuppID
	------------Total Dyelot group by SuppID-------------
	select distinct g.SuppID,fp.Dyelot  
	into #tmpsd
	from #FIR f
	inner join #GroupBySupp g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
	inner join #FIR_Physical fp on f.ID=fp.ID
	where f.PhysicalEncode=1

	select distinct g.SuppID,s.cnt 
	into #tmpDyelot
	from #tmpsuppEncode g
	outer apply(
		select ss.SuppID,count(ss.Dyelot) cnt
			from #tmpsd ss 
			where g.SuppID=ss.SuppID
			group by ss.SuppID
	) s
	order by g.SuppID
	------------Total dye lots accepted(Shadeband)-------------

	----從篩選過的物料，找出他們的FIR紀錄
	SELECT f.*
	INTO #FirData
	FROM (SELECT DISTINCT tt.POID,tt.Seq1,tt.Seq2,tt.SuppID,tt.Refno FROM #tmp1 tt)t 
	INNER JOIN #FIR f ON t.PoId=f.POID AND t.Seq1=f.SEQ1 AND t.Seq2 = f.Seq2 AND t.SuppID = f.Suppid AND t.Refno=f.Refno 

	----從得到的FIR紀錄，取得Fir_shadebone紀錄
	SELECT a.Suppid, b.ID,b.Roll,b.Dyelot,b.Result
	INTO #All_Fir_shadebone
	FROM #FirData a
	INNER JOIN #Fir_shadebone b ON a.id=b.id

	----統計有哪些Dyelot，是全部Pass的
	SELECT t.SuppID  ,[PassCTN]=COUNT(Dyelot)
	INTO #PassCountByDyelot
	FROM #tmpsd t
	/*OUTER APPLY(
		SELECT [PassCTN]=COUNT(a.ID)
		FROM #All_Fir_shadebone a
		WHERE a.Suppid=t.SuppID AND a.Dyelot = t.Dyelot AND a.Result = ''Pass''
	)PassCTN*/
	WHERE NOT EXISTS(SELECT * FROM #All_Fir_shadebone b WHERE  b.Suppid=t.SuppID AND b.Dyelot = t.Dyelot AND b.Result <> ''Pass'')
	GROUP BY t.SuppID 
';

	SET @SqlCmd3='
		------------Total Point----------
	select g.SuppID ,sum(fp.TotalPoint) TotalPoint
	into #tmpTotalPoint
	from #FIR f
	inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
	inner join #FIR_Physical fp on f.ID=fp.ID
	group by g.SuppID
	-----Total Roll Count----------
	select g.SuppID,count(fp.Roll) TotalRoll
	into #tmpTotalRoll
	from #FIR f
	inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
	inner join #FIR_Physical fp on f.ID=fp.ID
	group by g.SuppID
	---------Grade A Roll Count---------------------
	select g.SuppID,count(fp.Grade) GradeA_Roll
	into #tmpGrade_A
	from #FIR f
	inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
	inner join #FIR_Physical fp on f.ID=fp.ID
	where fp.Grade=''A''
	group by g.SuppID
	----------Grade B Roll Count---------------------
	select g.SuppID,count(fp.Grade) GradeB_Roll
	into #tmpGrade_B
	from #FIR f
	inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
	inner join #FIR_Physical fp on f.ID=fp.ID
	where fp.Grade=''B''
	group by g.SuppID
	----------Grade C Roll Count---------------------
	select g.SuppID,count(fp.Grade) GradeC_Roll
	into #tmpGrade_C
	from #FIR f
	inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
	inner join #FIR_Physical fp on f.ID=fp.ID
	where fp.Grade=''C''
	group by g.SuppID
	------------Kinds of Fabric Defects (Defect Name)---- 
	select DISTINCT t.SuppID,fpd.DefectRecord,t.PoId,t.Seq1,t.Seq2 
	into #tmpsuppdefect
	from #GroupBySupp t
	inner join #FIR f on t.PoId = f.POID and t.Seq1 = f.SEQ1 and t.Seq2 = f.SEQ2
	inner join #FIR_Physical fp on fp.ID = f.ID
	inner join #FIR_Physical_Defect fpd on fpd.FIR_PhysicalDetailUKey = fp.DetailUkey
	where f.PhysicalEncode = 1
	------------Group by Dyelot------------- 
	select PoId,Seq1,Seq2,SuppID,Refno,Dyelot,sum(ActualQty) as ActualQty 
	into #tmp2groupbyDyelot
	from #tmpAllData
	group by PoId,Seq1,Seq2,SuppID,Refno,Dyelot
	order by PoId,Seq1,Seq2,SuppID,Refno,Dyelot
	------------Group by Roll--------------- 
	select PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll,sum(ActualQty) as ActualQty 
	into #tmp2groupByRoll
	from #tmpAllData
	group by PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll
	order by PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll
	-----#spr 
	select distinct a.SuppID,
	Defect = STUFF((
				select concat(''/'',s.DefectRecord) 
				from (
					select distinct DefectRecord
					from #tmpsuppdefect b
					WHERE b.SuppID = a.SuppID
				)s
				for xml path('''')
			 ), 1, 1, '''')
	into #spr
	from #tmpsuppdefect a

	-------tmp  
	select distinct s.SuppID
		,ref.Refno
		,brand.brandid
		,s.AbbEN
		,TotalInspYds = (select isnull(sum(a.TotalInspYds),0) 
						from #FIR a 
						inner join #GroupBySupp b on a.POID = b.PoId and a.SEQ1 = b.Seq1 and a.Seq2 = b.Seq2 
						where a.Suppid = s.SuppID and a.refno=ref.Refno )
		,stockqty = stock.ActualQty
		,yrds = (
			select [Yrds] = count(*) * 5 
			from #tmpsuppdefect a inner join #GroupBySupp b on a.POID=b.PoId and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
			where b.SuppID = gbs.SuppID
		)
		,Point.Defect
		,Point.ID
		,Point.Point
		,ref.Clima
		,gbs.WhseArrival
		,gbs.FactoryID
	into #tmp
	from (select SuppID ,WhseArrival,FactoryID from #GroupBySupp group by SuppID,WhseArrival,FactoryID) as gbs
	outer apply(	
		SELECT Refno,Clima=cast(max(cast(Clima as int))as bit)
		FROM #tmpAllData 
		WHERE #tmpAllData.SuppID = gbs.SuppID
		group by RefNo

	) as ref
	cross apply(	
		SELECT distinct #tmpAllData.SuppID,Supp.AbbEN
		FROM #tmpAllData 
		inner join #Supp Supp WITH (NOLOCK) on  Supp.id = #tmpAllData.SuppID
		WHERE #tmpAllData.SuppID = gbs.SuppID 
	) as s
	outer apply(
		select Defect = fd.DescriptionEN, fd.ID, Point = sum(a.point)  
		from (
			select
				Defect = PBIReportData.dbo.SplitDefectNum(x.Data,0),	
				Point = cast(PBIReportData.dbo.SplitDefectNum(x.Data,1) as int)
			from dbo.SplitString((select Defect from #spr WHERE SuppID = gbs.SuppID),''/'') x
		)A 
		left join #FabricDefect fd on id = a.Defect
		group by fd.DescriptionEN,fd.ID
	) as Point
	outer apply(
		select BrandID = stuff((
			select concat('','',BrandID)
			from (
				select distinct o.BrandID 
				from #orders o
				inner join #tmpAllData t on o.poid=t.poid
				where t.refno=ref.refno
			) t
			for xml path('''')
			),1,1,'''')
	)as Brand
	outer apply(
		select isnull(sum(ActualQty),0) as ActualQty,RefNo 
		from #tmpAllData
		where Refno=ref.Refno
		and SuppID=s.SuppID
		AND WhseArrival = gbs.WhseArrival
		AND FactoryID = gbs.FactoryID
		group by RefNo 	
	)as stock
	WHERE　stock.ActualQty IS NOT NULL
	order by Refno
';

	SET @SqlCmd4='
	
	-------tmp2 
	select SuppID,defect,ID,
	point  = sum(point) over(partition by SuppID,defect)
	,ROW_NUMBER() over(partition by SuppID order by SuppID,point desc ,ID desc) RN 
	into #tmp2
	from #tmp
	order by SuppID,point desc,ID desc

	-----#SHtmp 
	select FL.POID, FL.SEQ1, FL.seq2, h.Dyelot
	into #SH1
	from #FIR_Laboratory FL WITH (NOLOCK) 
	inner join #FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = FL.ID 	
	where FL.HeatEncode=1 and h.Result = ''Fail''
	select FL.POID, FL.SEQ1, FL.seq2, W.Dyelot
	into #SH2
	from #FIR_Laboratory FL WITH (NOLOCK) 
	inner join #FIR_Laboratory_Wash W WITH (NOLOCK) on W.ID = FL.ID						
	where FL.WashEncode=1 and W.Result = ''Fail''

	select distinct SHRINKAGEyards = stockqty,SuppID
	into #SHtmp
	from(
		select Sum(rd1.stockqty) stockqty, SuppID	
		from  #tmp2groupbyDyelot rd WITH (NOLOCK) 
		inner join #View_AllReceivingDetail rd1 with (nolock) on rd.PoId = rd1.PoId and rd.Seq1 = rd1.Seq1 and rd.Seq2 = rd1.Seq2 
		Where exists(select * from #SH1 where POID = rd.PoId and SEQ1 = RD.seq1 and seq2 = seq2 and Dyelot = RD.dyelot)
		or exists(select * from #SH2 where POID = RD.poid and SEQ1 = RD.seq1 and seq2 = RD.seq2 and Dyelot = RD.dyelot)
		group by SuppID
	) a
	-----#mtmp 
	select l.POID,l.SEQ1,l.seq2, F.SuppID
	INTO #ea
	from #FIR_Laboratory l WITH (NOLOCK) 
	inner join #FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = l.ID
	INNER JOIN #FIR F WITH (NOLOCK) ON L.POID=F.POID
	where l.CrockingEncode=1 and h.Result = ''Fail'' 
	select O.poid,OD.seq1,OD.seq2,F.SuppID
	INTO #eb
	from #Oven O WITH (NOLOCK) 
	inner join #Oven_Detail OD WITH (NOLOCK) on OD.ID = O.ID
	INNER JOIN #FIR F WITH (NOLOCK) ON O.POID=F.POID 
	where O.Status =''Confirmed'' and OD.Result = ''Fail''
	select CF.poid,CFD.seq1,CFD.seq2,F.SuppID
	into #ec
	from #ColorFastness CF WITH (NOLOCK) 
	inner join #ColorFastness_Detail CFD WITH (NOLOCK) on CFD.ID = CF.ID
	INNER JOIN #FIR F WITH (NOLOCK) ON CF.POID=F.POID 
	where CF.Status =''Confirmed'' and CFD.Result = ''Fail'' 

	select [MIGRATIONyards] =sum(a.StockQty),tmp.SuppID 
	into #mtmp
	from(
		select rd.poid,rd.seq1,rd.seq2,rd.dyelot,rd.StockQty,ps.SuppID,psd.Refno
		from #View_AllReceivingDetail rd WITH (NOLOCK) 
		inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2 
		left join #PO_Supp_Detail psd on psd.id=r.PoId and psd.SEQ1 = r.Seq1 and psd.SEQ2 = r.Seq2
		left join #PO_Supp ps on ps.id=psd.Id and ps.seq1=psd.seq1
	)a
	inner join (select distinct SuppID from #tmp) tmp on a.SuppID = tmp.SuppID  
	Where exists(select * from #ea where POID = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2  AND a.SuppID = Tmp.SuppID ) 
	or exists(select * from #eb where POID = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2  AND a.SuppID = Tmp.SuppID ) 
	or exists(select * from #ec where POID = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2  AND a.SuppID = Tmp.SuppID )
	group by tmp.SuppID

	-----#Stmp 
	select f.poid,f.SEQ1,f.seq2,fs.Dyelot,f.SuppID
	into #sa
	from #FIR f WITH (NOLOCK)
	inner join #FIR_Shadebone fs WITH (NOLOCK) on fs.ID = f.ID
	where f.ShadebondEncode =1 and fs.Result = ''Fail'' 
	select f.poid,f.SEQ1,f.seq2,fc.Dyelot,f.SuppID
	into #sb
	from #FIR f WITH (NOLOCK) 
	inner join #FIR_Continuity fc WITH (NOLOCK) on fc.ID = f.ID
	where f.ContinuityEncode =1 and fc.Result = ''Fail'' 

	select [SHADINGyards] =sum(a.StockQty),tmp.SuppID
	into #Stmp
	from(
		select rd.poid,rd.seq1,rd.seq2,rd.dyelot,rd.StockQty,ps.SuppID,psd.Refno 
		from #View_AllReceivingDetail rd WITH (NOLOCK) 
		inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2 
		left join #PO_Supp_Detail psd on psd.id=r.PoId and psd.SEQ1 = r.Seq1 and psd.SEQ2 = r.Seq2
		left join #PO_Supp ps on ps.id=psd.Id and ps.seq1=psd.seq1
	)a
	inner join (select distinct SuppID from #tmp) tmp on a.SuppID = tmp.SuppID  
	Where (exists(select * from #sa where poid = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2 and Dyelot  = a.dyelot and a.SuppID = Tmp.SuppID ) 
	or exists(select * from #sb where poid = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2 and Dyelot  = a.dyelot and a.SuppID = Tmp.SuppID ))
	group by tmp.SuppID
';

	SET @SqlCmd5='
	
	-----#Ltmp 

	select ActualYds = sum(fp.TicketYds - fp.ActualYds), t.SuppID
	into #Ltmp
	from #FIR f
	inner join #FIR_Physical fp on f.ID = fp.ID
	inner join #GroupBySupp t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2
	where f.PhysicalEncode = 1 and fp.ActualYds < fp.TicketYds
	group by t.SuppID
	-----#Sdtmp 
	select SHORTWIDTH = sum(t.ActualQty)/5, f.SuppID
	into #Sdtmp
	from  #fir f WITH (NOLOCK) 
	inner join #FIR_Physical fp on f.ID = fp.ID
	inner join #tmp2groupByRoll t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2 and fp.Dyelot = t.Dyelot and fp.Roll = t.Roll
	left join #Fabric Fabric on Fabric.SCIRefno = f.SCIRefno
	where f.PhysicalEncode = 1 and fp.ActualWidth < Fabric.Width
	group by f.SuppID
	------#FabricInspDoc TestReport
	select 
		a.SuppID
		, iif(ccnt=0, 0, round(ccnt/bcnt,4)) [cnt] 
	into #tmpTestReport
	from(
		select tmp.SuppID, count(b.PoId)*1.0 bcnt, count(c.TPETestReport)*1.0 ccnt 
		from (
		select distinct SuppID
			   , poid
			   , seq1
			   , seq2
		from #tmpAllData
		) tmp
		left join #Export_Detail b on tmp.PoId =b.PoID and tmp.Seq1 =b.Seq1 and tmp.Seq2 = b.Seq2 and tmp.SuppID = b.SuppID
		left join #SentReport c on b.Ukey = c.Export_DetailUkey
		group by tmp.SuppID
	)a
	------#FabricInspDoc Inspection Report
	select 
		a.SuppID
		, iif(ccnt=0, 0, round(ccnt/bcnt,4)) [cnt] 
	into #InspReport
	from(
		select tmp.SuppID, count(b.PoId)*1.0 bcnt, count(c.TPEInspectionReport)*1.0 ccnt 
		from (
		select distinct SuppID
			   , poid
			   , seq1
			   , seq2
		from #tmpAllData
		) tmp
		left join #Export_Detail b on tmp.PoId =b.PoID and tmp.Seq1 =b.Seq1 and tmp.Seq2 = b.Seq2 and tmp.SuppID = b.SuppID
		left join #SentReport c on b.Ukey = c.Export_DetailUkey
		group by tmp.SuppID
	)a

	------#FabricInspDoc Approved Continuity Card Provided %
	select a.SuppID
		, iif(ccnt=0, 0, round(ccnt/bcnt,4)) [cnt] 
	into #tmpContinuityCard
	from(
		select tmp.SuppID, count(b.PoId)*1.0 bcnt, count(c.ContinuityCard)*1.0 ccnt 
		from (
		select distinct SuppID
			   , poid
			   , seq1
			   , seq2
		from #tmpAllData
		) tmp
		left join #Export_Detail b on tmp.PoId =b.PoID and tmp.Seq1 =b.Seq1 and tmp.Seq2 = b.Seq2 and tmp.SuppID = b.SuppID
		left join #SentReport c on b.Ukey = c.Export_DetailUkey
		group by tmp.SuppID
	)a

	------#FabricInspDoc Approved 1st Bulk Dyelot Provided %
	select b.Refno, b.ColorID, b.SuppID, d.Consignee, c.SeasonSCIID DyelotSeasion, c.FirstDyelot, e.SeasonSCIID, c.Period, f.RibItem 
	into #tmp_DyelotMain 
	from (
			select distinct SuppID
					, poid
					, seq1
					, seq2
			from #tmpAllData 
		) tmp
	outer apply (
		select a.id,a.seq1,a.seq2,a.SCIRefno,a.ColorID,b.SuppID,a.RefNo 
		from #PO_Supp_Detail a
		inner join #PO_Supp b on a.ID = b.ID and a.seq1 = b.seq1 
		where a.id =  tmp.poid
		and a.seq1 = tmp.seq1
		and a.seq2 = tmp.seq2
	)b
	LEFT JOIN #Export_Detail ED ON ED.PoID = TMP.POID AND ED.Seq1 = TMP.SEQ1 AND ED.Seq2 = TMP.SEQ2
	left join #Export d on ED.ID = D.ID AND D.Confirm = 1
	left join #Factory fty with (nolock) on fty.ID = d.Consignee
	left join #FIRSTDYELOT c on b.Refno = c.Refno and b.ColorID = c.ColorID and b.SuppID = c.SuppID  AND c.TestDocFactoryGroup = fty.TestDocFactoryGroup 
	left join #orders o on ed.PoID = o.id and o.Category in (''B'',''M'')
	left join #Season e on o.SeasonID  = e.ID and o.BrandID = e.BrandID --and e.SeasonSCIID = c.SeasonSCIID
	left join #Fabric f on f.SCIRefno = b.SCIRefno 
	group by b.Refno, b.ColorID, b.SuppID, d.Consignee, c.SeasonSCIID, c.FirstDyelot, e.SeasonSCIID,c.Period,f.RibItem 
';

	SET @SqlCmd6='
	
	 --分母
	 select SuppID,  count(*)*1.0 Mcnt
	 into #tmp_DyelotMcnt
	 from (
		 select Refno, ColorID, SuppID, Consignee, SeasonSCIID, Period, RibItem 
		 from #tmp_DyelotMain
		 where not(FirstDyelot is null and RibItem = 1) 
		 group by Refno, ColorID, SuppID, Consignee, SeasonSCIID, Period, RibItem 
	 )a
	 group by SuppID 

	--重新計算月份
	select 
	ROW_NUMBER() OVER(order by month ASC) as rid
	,* 
	into #tmp_newSeasonSCI
	from #SeasonSCI 

	select a.*, b.rid, (b.rid + a.Period -1) maxID
	into #tmp_DyelotMonth
	from #tmp_DyelotMain a
	left join  #tmp_newSeasonSCI b on a.DyelotSeasion = b.id
	where a.FirstDyelot is not null

	 --分子
	select a.SuppID ,count(*)*1.0 Dcnt
	 into #tmp_DyelotDcnt
	 from (
		 select Refno,ColorID,SuppID,Consignee,SeasonSCIID  
		 from #tmp_DyelotMain 
		 group by Refno,ColorID,SuppID,Consignee,SeasonSCIID 
	 ) a
	 inner join 
	 (
		 select a.Refno,a.ColorID,a.SuppID, a.Consignee, a.Period, a.RibItem, b.id 
		 from #tmp_DyelotMonth a
		 left join #tmp_newSeasonSCI b on b.rid between a.rid and a.maxID   
		 group by a.Refno,a.ColorID,a.SuppID, a.Consignee, a.Period, a.RibItem, b.id 
	 )b on a.Refno = b.Refno and a.ColorID = b.ColorID and a.SuppID = b.SuppID and a.Consignee = b.Consignee and a.SeasonSCIID = b.id 
	 group by a.SuppID 

	 select a.SuppID
		, iif(isnull(b.Dcnt,0)=0, 0, round(b.Dcnt/a.Mcnt ,4)) [cnt]
	 into #BulkDyelot
	 from #tmp_DyelotMcnt a
	 left join #tmp_DyelotDcnt b on a.SuppID = b.SuppID 

	------#TmpFinal 
	select --distinct
	 Tmp.SuppID , Tmp.refno 
		,Tmp.abben 
		,Tmp.BrandID
		,Tmp.stockqty 
		,totalYds.TotalInspYds
		,[Total PoCnt] = isnull(TLSP.cnt,0)
		,[Total Dyelot] =isnull(TLDyelot.cnt,0)
		 ,[Total dye lots accepted(Shadeband)] = ISNULL( PassCountByDyelot.PassCTN ,0)
		,[Insp Report] = isnull(InspReport.cnt,0)
		,[Test Report] = isnull(TestReport.cnt,0)
		,[Continuity Card] = isnull(Contcard.cnt,0)
		,[BulkDyelot] = isnull(BulkDyelot.cnt,0)
		,[Total Point] = isnull(TLPoint.TotalPoint,0)
		,[Total Roll]= isnull(TLRoll.TotalRoll,0)
		,[GradeA Roll]= isnull(GACount.GradeA_Roll,0)
		,[GradeB Roll]= isnull(GBCount.GradeB_Roll,0)
		,[GradeC Roll]= isnull(GCCount.GradeC_Roll,0)
		,[Inspected] = iif(totalStockqty.stockqty = 0, 0, round(totalYds.TotalInspYds/totalStockqty.stockqty,4)) 
		,[yds] = isnull(TLPoint.Fabric_yards,0)   
		,[Fabric(%)] = IIF(totalYds.TotalInspYds!=0, round((TLPoint.Fabric_yards/totalYds.TotalInspYds), 4), 0)        		
		,id = sl.ID
		,[Point] = point.defect
		,[SHRINKAGEyards] = isnull(SHRINKAGE.SHRINKAGEyards,0)
		,[SHRINKAGE (%)] = isnull(SHINGKAGELevel.SHINGKAGE ,0)
		,SHINGKAGELevel = sl2.ID
		,[MIGRATIONyards] = isnull(MIGRATION.MIGRATIONyards,0)
		,[MIGRATION (%)] = isnull(MIGRATIONLevel.MIGRATION,0)
		,MIGRATIONLevel = sl3.ID
		,[SHADINGyards] = isnull(SHADING.SHADINGyards,0)
		,[SHADING (%)] = isnull(SHADING,0)
		,SHADINGLevel = sl4.ID
		,[ActualYds] = isnull(LACKINGYARDAGE.ActualYds,0)
		,[LACKINGYARDAGE(%)] = isnull(LACKINGYARDAGELevel.LACKINGYARDAGE ,0)
		,LACKINGYARDAGELevel = sl5.ID
		,[SHORTWIDTH] = isnull(SHORTyards.SHORTWIDTH,0)
		,[SHORT WIDTH (%)] = isnull(SHORTWIDTHLevel.SHORTWIDTH,0)
		,SHORTWIDTHLevel = sl6.ID
		,Tmp.WhseArrival
		,Tmp.FactoryID
		,Tmp.Clima 
	into #TmpFinal
	from (
		select distinct SuppID, refno, abben, BrandID, stockqty, isnull(TotalInspYds,0)TotalInspYds,Clima ,WhseArrival, FactoryID
		, yrds	
		from #tmp		
	)Tmp
	outer apply(select SuppID, sum(stockqty)totalStockqty from (select distinct SuppID, refno, abben, BrandID, stockqty, isnull(TotalInspYds,0)TotalInspYds, yrds from #tmp)a where tmp.suppid =a.suppid group by SuppID)TmpTotal
	outer apply (select TotalPoint,[Fabric_yards] = isnull(TotalPoint,0)/4 from #tmpTotalPoint where SuppID=tmp.SuppID) TLPoint
	outer apply
	(
		select Defect = stuff(( 
			select concat('', '',Defect) 
			from #tmp2 ta2
			where ta2.SuppID = Tmp.SuppID and rn <=3 --and ta2.WhseArrival = tmp.WhseArrival
			for xml path('''')
		),1,1,'''') 
	) point
	outer apply(
	select SuppID ,sum(stockqty) stockqty from (
		select distinct suppid,refno,brandid,abben,stockqty
		from #tmp ) a
		where SuppID=tmp.SuppID 
		group by SuppID
	) totalStockqty
	outer apply(
	select SuppID ,sum(TotalInspYds) TotalInspYds from (
		select distinct suppid,refno,brandid,abben,TotalInspYds
		from #tmp ) a
		where SuppID=tmp.SuppID
		group by SuppID
	) totalYds
	outer apply(
		select id from #SuppLevel WITH (NOLOCK) where type=''F'' and Junk=0 
		and IIF(totalYds.TotalInspYds!=0, round((TLPoint.Fabric_yards/totalYds.TotalInspYds), 4), 0) * 100 between range1 and range2 )sl
	left join #SHtmp SHRINKAGE on SHRINKAGE.SuppID = tmp.SuppID
	outer apply(select SHINGKAGE = iif(TmpTotal.totalStockqty = 0 , 0, round(SHRINKAGE.SHRINKAGEyards/TmpTotal.totalStockqty,4)))SHINGKAGELevel
	outer apply(select id from #SuppLevel WITH (NOLOCK) where type=''F'' and Junk=0 and isnull(SHINGKAGELevel.SHINGKAGE,0) * 100 between range1 and range2)sl2
	left join #mtmp MIGRATION on MIGRATION.SuppID = tmp.SuppID  
	outer apply(
		select MIGRATION =  iif(TmpTotal.totalStockqty = 0, 0, round(MIGRATION.MIGRATIONyards/TmpTotal.totalStockqty,4))
	)MIGRATIONLevel 
	outer apply(select id from #SuppLevel WITH (NOLOCK) where type=''F'' and Junk=0 and isnull(MIGRATIONLevel.MIGRATION,0) * 100 between range1 and range2)sl3
	left join #Stmp SHADING on SHADING.SuppID = tmp.SuppID 
	outer apply(select SHADING = iif(TmpTotal.totalStockqty=0, 0, round(SHADING.SHADINGyards/TmpTotal.totalStockqty,4)))SHADINGLevel 
	outer apply(select id from #SuppLevel WITH (NOLOCK) where type=''F'' and Junk=0 and isnull(SHADINGLevel.SHADING,0) * 100 between range1 and range2)sl4
	left join #Ltmp LACKINGYARDAGE on LACKINGYARDAGE.SuppID= Tmp.SuppID 
	outer apply(select LACKINGYARDAGE = iif(totalYds.TotalInspYds=0, 0, round(LACKINGYARDAGE.ActualYds/totalYds.TotalInspYds,4)))LACKINGYARDAGELevel
	outer apply(select id from #SuppLevel WITH (NOLOCK) where type=''F'' and Junk=0 and isnull(LACKINGYARDAGELevel.LACKINGYARDAGE,0) * 100 between range1 and range2)sl5
	left join #Sdtmp SHORTyards on SHORTyards.SuppID = Tmp.SuppID 
	outer apply(select SHORTWIDTH = iif(totalYds.TotalInspYds=0, 0, round(SHORTyards.SHORTWIDTH/totalYds.TotalInspYds,4)))SHORTWIDTHLevel 
	outer apply(select id from #SuppLevel WITH (NOLOCK) where type=''F'' and Junk=0 
	and isnull(SHORTWIDTHLevel.SHORTWIDTH,0) * 100 between range1 and range2
	)sl6
	outer apply (select cnt from #tmpDyelot where SuppID=Tmp.SuppID ) TLDyelot
	outer apply (select PassCTN from #PassCountByDyelot where SuppID=Tmp.SuppID) PassCountByDyelot
	outer apply (select TotalRoll from #tmpTotalRoll where SuppID=tmp.SuppID) TLRoll
	outer apply (select GradeA_Roll from #tmpGrade_A where SuppID=tmp.SuppID) GACount
	outer apply (select GradeB_Roll from #tmpGrade_B where SuppID=tmp.SuppID) GBCount
	outer apply (select GradeC_Roll from #tmpGrade_C where SuppID=tmp.SuppID) GCCount
	outer apply (select cnt from #tmpCountSP where SuppID=tmp.SuppID ) TLSP
	outer apply (select cnt from #tmpTestReport where SuppID=tmp.SuppID) TestReport
	outer apply (select cnt from #InspReport where SuppID=tmp.SuppID ) InspReport
	outer apply (select cnt from #tmpContinuityCard where SuppID=tmp.SuppID ) ContCard
	outer apply (select cnt from #BulkDyelot where SuppID=tmp.SuppID) BulkDyelot
	order by Tmp.SuppID
';

	SET @SqlCmd7='
	
	--準備比重#table不然每筆資料都要重撈7次  
	select DISTINCT
		[Fabric Defect] = (select Weight from #Inspweight WITH (NOLOCK) where id =''Fabric Defect'')
		,[Lacking Yardage] =  (select Weight from #Inspweight WITH (NOLOCK) where id =''Lacking Yardage'')
		,[Migration] = (select Weight from #Inspweight WITH (NOLOCK) where id =''Migration'')
		,[Shading] = (select Weight from #Inspweight WITH (NOLOCK) where id =''Shading'')
		,[Sharinkage] =  (select Weight from #Inspweight WITH (NOLOCK) where id =''Sharinkage'')
		,[Short Width] = (select Weight from #Inspweight WITH (NOLOCK) where id =''Short Width'') 
		,sumWeight = SUM(Weight) OVER()
	into #Weight
	FROM #Inspweight


	--取加權平均數AVG & 取AVG值落在甚麼LEVEL區間 
	select 	
		 a.SuppID 
		,a.refno 
		,SupplierName =a.abben 
		,a.BrandID
		,a.StockQty 
		,a.TotalInspYds
		,TotalPoCnt= [Total PoCnt] 
		,TotalDyelot= [Total Dyelot]
		,TotalDyelotAccepted= [Total dye lots accepted(Shadeband)]
		,InspReport= [Insp Report] *100
		,TestReport= [Test Report] *100
		,ContinuityCard= [Continuity Card] *100
		,BulkDyelot =[BulkDyelot] *100
		,TotalPoint= [Total Point] 
		,TotalRoll= [Total Roll]
		,GradeARoll = [GradeA Roll]
		,GradeBRoll = [GradeB Roll]
		,GradeCRoll = [GradeC Roll]
		,Inspected = [Inspected] *100
		,Yds = [yds]
		,FabricPercent = [Fabric(%)]  *100      		
		,FabricLevel = a.id
		,Point
		,SHRINKAGEyards
		,SHRINKAGEPercent = [SHRINKAGE (%)] *100
		,SHINGKAGELevel
		,MIGRATIONyards
		,MIGRATIONPercent = [MIGRATION (%)]*100
		,MIGRATIONLevel
		,SHADINGyards
		,SHADINGPercent=[SHADING (%)]*100
		,SHADINGLevel
		,ActualYds
		,LACKINGYARDAGEPercent = [LACKINGYARDAGE(%)]*100
		,LACKINGYARDAGELevel
		,SHORTWIDTH
		,SHORTWidthPercent = [SHORT WIDTH (%)]*100
		,SHORTWIDTHLevel
		,TotalDefectRate = a.Avg *100
		,TotalLevel = s.id
		,WhseArrival
		,FactoryID
		,Clima 
	INTO #Final
	from(
		select t.*
		,[Avg] = CASE WHEN sumWeight=0 THEN 0 ELSE isnull((([Fabric(%)] * [Fabric Defect] + [LACKINGYARDAGE(%)] * [Lacking Yardage] +[MIGRATION (%)] * [Migration] + 
				   [SHADING (%)] * [Shading] + [SHRINKAGE (%)] * [Sharinkage] + [SHORT WIDTH (%)] *  [Short Width])/sumWeight ),0) END
		from #TmpFinal t,#Weight
	) a
	,#SuppLevel s
	where s.type=''F'' and s.Junk=0 and [AVG] * 100 between s.range1 and s.range2 
	--AND a.SuppID=''1166'' --AND a.Refno=''60014880''
	ORDER BY  SUPPID,Refno,WhseArrival ,FactoryID		

	MERGE INTO PBIReportData.dbo.P_QA_R06 t
	USING #Final s 
	ON t.SuppID=s.SuppID  AND t.Refno=s.Refno AND t.WhseArrival = s.WhseArrival AND t.FactoryID = s.FactoryID
	WHEN MATCHED THEN   
		UPDATE SET 
		   t.SupplierName = s.SupplierName
		  ,t.BrandID = s.BrandID
		  ,t.StockQty = s.StockQty
		  ,t.TotalInspYds = s.TotalInspYds
		  ,t.TotalPoCnt = s.TotalPoCnt
		  ,t.TotalDyelot = s.TotalDyelot
		  ,t.TotalDyelotAccepted = s.TotalDyelotAccepted
		  ,t.InspReport = s.InspReport
		  ,t.TestReport = s.TestReport
		  ,t.ContinuityCard = s.ContinuityCard
		  ,t.BulkDyelot = s.BulkDyelot
		  ,t.TotalPoint = s.TotalPoint
		  ,t.TotalRoll = s.TotalRoll
		  ,t.GradeARoll = s.GradeARoll
		  ,t.GradeBRoll = s.GradeBRoll
		  ,t.GradeCRoll = s.GradeCRoll
		  ,t.Inspected = s.Inspected
		  ,t.Yds = s.Yds
		  ,t.FabricPercent = s.FabricPercent
		  ,t.FabricLevel = s.FabricLevel
		  ,t.Point = s.Point
		  ,t.SHRINKAGEyards = s.SHRINKAGEyards
		  ,t.SHRINKAGEPercent = s.SHRINKAGEPercent
		  ,t.SHINGKAGELevel = s.SHINGKAGELevel
		  ,t.MIGRATIONyards = s.MIGRATIONyards
		  ,t.MIGRATIONPercent = s.MIGRATIONPercent
		  ,t.MIGRATIONLevel = s.MIGRATIONLevel
		  ,t.SHADINGyards = s.SHADINGyards
		  ,t.SHADINGPercent = s.SHADINGPercent
		  ,t.SHADINGLevel = s.SHADINGLevel
		  ,t.ActualYds = s.ActualYds
		  ,t.LACKINGYARDAGEPercent = s.LACKINGYARDAGEPercent
		  ,t.LACKINGYARDAGELevel = s.LACKINGYARDAGELevel
		  ,t.SHORTWIDTH = s.SHORTWIDTH
		  ,t.SHORTWidthPercent = s.SHORTWidthPercent
		  ,t.SHORTWIDTHLevel = s.SHORTWIDTHLevel
		  ,t.TotalDefectRate = s.TotalDefectRate
		  ,t.TotalLevel = s.TotalLevel
		  ,t.Clima = s.Clima
	
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (SuppID           ,Refno           ,SupplierName           ,BrandID           ,StockQty
           ,TotalInspYds           ,TotalPoCnt           ,TotalDyelot           ,TotalDyelotAccepted           ,InspReport
           ,TestReport           ,ContinuityCard           ,BulkDyelot           ,TotalPoint           ,TotalRoll
           ,GradeARoll           ,GradeBRoll           ,GradeCRoll           ,Inspected           ,Yds
           ,FabricPercent           ,FabricLevel           ,Point           ,SHRINKAGEyards           ,SHRINKAGEPercent
           ,SHINGKAGELevel           ,MIGRATIONyards           ,MIGRATIONPercent           ,MIGRATIONLevel           ,SHADINGyards
           ,SHADINGPercent           ,SHADINGLevel           ,ActualYds           ,LACKINGYARDAGEPercent           ,LACKINGYARDAGELevel
           ,SHORTWIDTH           ,SHORTWidthPercent           ,SHORTWIDTHLevel           ,TotalDefectRate           ,TotalLevel
           ,WhseArrival           ,FactoryID           ,Clima)
		VALUES (SuppID           ,s.Refno           ,s.SupplierName           ,s.BrandID           ,s.StockQty
           ,s.TotalInspYds           ,s.TotalPoCnt           ,s.TotalDyelot           ,s.TotalDyelotAccepted           ,s.InspReport
           ,s.TestReport           ,s.ContinuityCard           ,s.BulkDyelot           ,s.TotalPoint           ,s.TotalRoll
           ,s.GradeARoll           ,s.GradeBRoll           ,s.GradeCRoll           ,s.Inspected           ,s.Yds
           ,s.FabricPercent           ,s.FabricLevel           ,s.Point           ,s.SHRINKAGEyards           ,s.SHRINKAGEPercent
           ,s.SHINGKAGELevel           ,s.MIGRATIONyards           ,s.MIGRATIONPercent           ,s.MIGRATIONLevel           ,s.SHADINGyards
           ,s.SHADINGPercent           ,s.SHADINGLevel           ,s.ActualYds           ,s.LACKINGYARDAGEPercent           ,s.LACKINGYARDAGELevel
           ,s.SHORTWIDTH           ,s.SHORTWidthPercent           ,s.SHORTWIDTHLevel           ,s.TotalDefectRate           ,s.TotalLevel
           ,s.WhseArrival           ,s.FactoryID           ,s.Clima)
	WHEN NOT MATCHED BY SOURCE AND t.WhseArrival >= '''+@WhseArrival_s_varchar+'''  AND t.WhseArrival <= '''+@WhseArrival_e_varchar+'''    THEN 
		DELETE
	;
	
	drop table #tmp1,#tmp,#tmp2,#tmpAllData,#GroupBySupp,#tmpsuppdefect,#tmp2groupbyDyelot,#tmp2groupByRoll,#spr
	,#SH1,#SH2,#SHtmp,#mtmp,#ea,#eb,#ec,#sa,#sb,#Stmp,#Ltmp,#Sdtmp,#TmpFinal,#Weight
	,#tmpsd,#tmpDyelot,#tmpTotalPoint,#tmpTotalRoll,#tmpGrade_A,#tmpGrade_B,#tmpGrade_C,#tmpsuppEncode
	,#tmpCountSP,#tmpTestReport,#InspReport,#tmpContinuityCard,#BulkDyelot
	,#tmp_DyelotMain,#tmp_DyelotMcnt,#tmp_newSeasonSCI,#tmp_DyelotMonth,#tmp_DyelotDcnt
	,#PassCountByDyelot ,#FirData ,#All_Fir_shadebone,#Final
	
	
';



	SET @SqlCmd_Combin = @SqlCmd1 +  @SqlCmd2 +  @SqlCmd3 +  @SqlCmd4 +  @SqlCmd5 + @SqlCmd6 + @SqlCmd7;
	EXEC sp_executesql @SqlCmd_Combin
	--select @SqlCmd_Combin
END