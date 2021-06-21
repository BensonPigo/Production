USE [PBIReportData]
GO
/****** Object:  StoredProcedure [dbo].[P_ImportSDPOrderDetail]    Script Date: 06/01/2021 上午 09:23:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[P_Import_QA_P09]	
	@ETA_s Date,
	@ETA_e Date,
	@LinkServerName varchar(50)
AS
BEGIN
	DECLARE @SqlCmd_Combin nvarchar(max) =''
	DECLARE @SqlCmd1 nvarchar(max) ='';
	DECLARE @SqlCmd2 nvarchar(max) ='';
	DECLARE @SqlCmd3 nvarchar(max) ='';
	DECLARE @SqlCmd4 nvarchar(max) ='';
	DECLARE @SqlCmd5 nvarchar(max) ='';
	
	DECLARE @ETA_s_varchar varchar(10) = cast( @ETA_s as varchar)
	DECLARE @ETA_e_varchar varchar(10) = cast( @ETA_e as varchar)

SET @SqlCmd1 = '
	----準備基礎資料
	Select RowNo = ROW_NUMBER() OVER(ORDER by Month), ID 
	Into #probablySeasonList
	From ['+@LinkServerName+'].Production.dbo.SeasonSCI

	select distinct
	[WK#] = ed.id,
    [Invoice#]= ed.InvoiceNo,
	[ATA] = Export.WhseArrival,
	[ETA] = Export.ETA,
    [Season] = o.SeasonID,
	[SP#] = ed.PoID,
	[Seq#] = ed.seq1+''-''+ed.seq2,	
    [Brand] = o.BrandID,
	[Supp] = ps.SuppID,
	[Supp Name] = Supp.AbbEN,
	[Ref#] = psd.Refno,
	[Color] = c.ColorName,
	Qty = isnull(ed.Qty,0) + isnull(ed.Foc,0),
	[Inspection Report_Fty Received Date] = sr.InspectionReport,
	[Inspection Report_Supp Sent Date] = sr.TPEInspectionReport,
	[Test Report_Fty Received Date] = sr.TestReport,
	[Test Report_ Check Clima] = isnull(sr.TestReportCheckClima,0),
	[Test Report_Supp Sent Date] = sr.TPETestReport,
	[Continuity Card_Fty Received Date] = sr.ContinuityCard,
	[Continuity Card_Supp Sent Date] = sr.TPEContinuityCard,
	[Continuity Card_AWB#] = sr.AWBNo,
	[1st Bulk Dyelot_Fty Received Date] = FirstDyelot.FirstDyelot,
	[1st Bulk Dyelot_Supp Sent Date]  = IIF(FirstDyelot.TPEFirstDyelot is null and f.RibItem = 1
                        ,''RIB no need first dye lot''
                        ,IIF(FirstDyelot.SeasonSCIID is null
                                ,''Still not received and under pushing T2. Please contact with PR if you need L/G first.''
                                ,format(FirstDyelot.TPEFirstDyelot,''yyyy/MM/dd'')
                            )
                    ),
	[T2 Inspected Yards] = isnull(sr.T2InspYds,0),
	[T2 Defect Points] = isnull(sr.T2DefectPoint,0),
	[Grade] = sr.T2Grade,
	[T1 Inspected Yards] = isnull(a.T1InspectedYards,0),
	[T1 Defect Points] = isnull(b.T1DefectPoints,0),
	[Fabric with clima] = isnull(f.Clima,0),
	psd.ColorID,
    ed.seq1,
    ed.seq2,
	ed.Ukey,
    [bitRefnoColor] = case when f.Clima = 1 then ROW_NUMBER() over(partition by f.Clima, ps.SuppID, psd.Refno, psd.ColorID, Format(Export.CloseDate,''yyyyMM'') order by Export.CloseDate) else 0 end,
	[FactoryID] = Export.FactoryID
into #tmpFinal
from ['+@LinkServerName+'].Production.dbo.Export_Detail ed with(nolock)
 ';
 
SET @SqlCmd2 = '
inner join ['+@LinkServerName+'].Production.dbo.Export with(nolock) on Export.id = ed.id and Export.Confirm = 1
inner join ['+@LinkServerName+'].Production.dbo.orders o with(nolock) on o.id = ed.PoID
left join ['+@LinkServerName+'].Production.dbo.SentReport sr with(nolock) on sr.Export_DetailUkey = ed.Ukey
left join ['+@LinkServerName+'].Production.dbo.Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
left join ['+@LinkServerName+'].Production.dbo.PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
left join ['+@LinkServerName+'].Production.dbo.Supp with(nolock) on Supp.ID = ps.SuppID
left join ['+@LinkServerName+'].Production.dbo.Season s with(nolock) on s.ID=o.SeasonID and s.BrandID = o.BrandID
left join ['+@LinkServerName+'].Production.dbo.Factory fty with (nolock) on fty.ID = Export.Consignee
left join ['+@LinkServerName+'].Production.dbo.Fabric f with(nolock) on f.SCIRefno =psd.SCIRefno
Left join #probablySeasonList seasonSCI on seasonSCI.ID = s.SeasonSCIID
OUTER APPLY(
	Select Top 1 FirstDyelot,TPEFirstDyelot,SeasonSCIID
	From ['+@LinkServerName+'].Production.dbo.FirstDyelot fd
	Inner join #probablySeasonList season on fd.SeasonSCIID = season.ID
	WHERE fd.Refno = psd.Refno and fd.ColorID = psd.ColorID and fd.SuppID = ps.SuppID and fd.TestDocFactoryGroup = fty.TestDocFactoryGroup
		And seasonSCI.RowNo >= season.RowNo
	Order by season.RowNo Desc
)FirstDyelot
outer apply(
	select T1InspectedYards=sum(fp.ActualYds)
	from ['+@LinkServerName+'].Production.dbo.fir f
	left join ['+@LinkServerName+'].Production.dbo.FIR_Physical fp on fp.id=f.id
	left join ['+@LinkServerName+'].Production.dbo.Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)a
outer apply(
	select  T1DefectPoints = sum(fp.TotalPoint)
	from ['+@LinkServerName+'].Production.dbo.fir f
	left join ['+@LinkServerName+'].Production.dbo.FIR_Physical fp on fp.id=f.id
	left join ['+@LinkServerName+'].Production.dbo.Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)b
outer apply(
    select [ColorName] = iif(c.Varicolored > 1, c.Name, c.ID)
    from ['+@LinkServerName+'].Production.dbo.Color c
    where c.ID = psd.ColorID
    and c.BrandID = psd.BrandID 
)c
where  Export.ETA between '''+@ETA_s_varchar+''' and '''+@ETA_e_varchar+'''
and psd.FabricType = ''F''
and (ed.qty + ed.Foc)>0
and o.Category in(''B'',''M'')
and exists(
	select * from ['+@LinkServerName+'].Production.dbo.Factory 
	where Factory.ID = Export.FactoryID
	and Factory.MDivisionID = Factory.ProduceM
)

';

SET @SqlCmd3 = '
	drop table #probablySeasonList

	-----開始Merge 
	MERGE INTO PBIReportData.dbo.P_QA_P09 t
	USING #tmpFinal s 
	ON t.WK#=s.WK# AND t.SP#=s.SP# AND t.Seq# = s.Seq#
	WHEN MATCHED THEN   
		UPDATE SET 
		t.WK# =  s.WK#,
		t.Invoice# =  s.Invoice#,
		t.ATA =  s.ATA,
		t.ETA =  s.ETA,
		t.Season =  s.Season,
		t.SP# =  s.SP#,
		t.Seq# =  s.Seq#,
		t.Brand =  s.Brand,
		t.Supp =  s.Supp,
		t.[Supp Name] =  s.[Supp Name],
		t.Ref# =  s.Ref#,
		t.Color =  s.Color,
		t.Qty =  s.Qty,
		t.[Inspection Report_Fty Received Date] =  s.[Inspection Report_Fty Received Date],
		t.[Inspection Report_Supp Sent Date] =  s.[Inspection Report_Supp Sent Date],
		t.[Test Report_Fty Received Date] =  s.[Test Report_Fty Received Date],
		t.[Test Report_ Check Clima] =  s.[Test Report_ Check Clima],
		t.[Test Report_Supp Sent Date] =  s.[Test Report_Supp Sent Date],
		t.[Continuity Card_Fty Received Date] =  s.[Continuity Card_Fty Received Date],
		t.[Continuity Card_Supp Sent Date] =  s.[Continuity Card_Supp Sent Date],
		t.[Continuity Card_AWB#] =  s.[Continuity Card_AWB#],
		t.[1st Bulk Dyelot_Fty Received Date] =  s.[1st Bulk Dyelot_Fty Received Date],
		t.[1st Bulk Dyelot_Supp Sent Date] =  s.[1st Bulk Dyelot_Supp Sent Date],
		t.[T2 Inspected Yards] =  s.[T2 Inspected Yards],
		t.[T2 Defect Points] =  s.[T2 Defect Points],
		t.[Grade] =  s.Grade,
		t.[T1 Inspected Yards] =  s.[T1 Inspected Yards],
		t.[T1 Defect Points] =  s.[T1 Defect Points],
		t.[Fabric with clima] =  s.[Fabric with clima],
		t.FactoryID = s.FactoryID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (WK#,Invoice#,ATA,ETA,Season,[SP#],[Seq#],Brand,Supp,[Supp Name],[Ref#],Color,Qty,[Inspection Report_Fty Received Date]
				,[Inspection Report_Supp Sent Date],[Test Report_Fty Received Date],[Test Report_ Check Clima],[Test Report_Supp Sent Date]
				,[Continuity Card_Fty Received Date],[Continuity Card_Supp Sent Date],[Continuity Card_AWB#],[1st Bulk Dyelot_Fty Received Date]
				,[1st Bulk Dyelot_Supp Sent Date],[T2 Inspected Yards],[T2 Defect Points],[Grade],[T1 Inspected Yards],[T1 Defect Points],[Fabric with clima]
				,FactoryID
				)
		VALUES (s.WK#,s.Invoice#,s.ATA,s.ETA,s.Season,s.[SP#],s.[Seq#],s.Brand,s.Supp,s.[Supp Name],s.[Ref#],s.Color,s.Qty,s.[Inspection Report_Fty Received Date]
				,s.[Inspection Report_Supp Sent Date],s.[Test Report_Fty Received Date],s.[Test Report_ Check Clima],s.[Test Report_Supp Sent Date]
				,s.[Continuity Card_Fty Received Date],s.[Continuity Card_Supp Sent Date],s.[Continuity Card_AWB#],s.[1st Bulk Dyelot_Fty Received Date]
				,s.[1st Bulk Dyelot_Supp Sent Date],s.[T2 Inspected Yards],s.[T2 Defect Points],s.[Grade],s.[T1 Inspected Yards],s.[T1 Defect Points],s.[Fabric with clima],s.FactoryID
				);


delete t 
from PBIReportData.dbo.P_QA_P09 t
left join #tmpFinal s on t.WK#=s.WK#  AND t.SP#=s.SP# AND t.Seq# = s.Seq#
where s.WK# is null
and T.ETA between '''+@ETA_s_varchar+''' and '''+@ETA_e_varchar+'''
and exists(
	select * from ['+@LinkServerName+'].Production.dbo.Export 
	inner join ['+@LinkServerName+'].Production.dbo.Factory on Factory.ID = Export.FactoryID
	where Factory.MDivisionID = Factory.ProduceM
	and Export.ID = t.WK#
)


	DROP TABLE #tmpFinal
';
/*
print @SqlCmd1
print @SqlCmd2
print @SqlCmd3
*/

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3
	EXEC sp_executesql @SqlCmd_Combin
END