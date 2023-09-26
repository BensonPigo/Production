Create PROCEDURE [dbo].[P_Import_QA_R11_DefectDetail]
As
Begin
	Set NoCount On;
		
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	declare @current_PMS_ServerName nvarchar(50) = 'MainServer' 

Declare @ExecSQL1 NVarChar(MAX);
Declare @ExecSQL2 NVarChar(MAX);
Declare @ExecSQL3 NVarChar(MAX);
Declare @ExecSQL4 NVarChar(MAX);
Declare @ExecSQL5 NVarChar(MAX);
Declare @ExecSQL6 NVarChar(MAX);
	Set @ExecSQL1 =
		N'
declare @Date1 date = DATEADD(MONTH, -3, GETDATE())
declare @Date2 date = GETDATE()

--Receiving
SELECT
	f.ID,
	f.ReceivingID,
	F.POID,
	F.SEQ1,F.SEQ2,
	SEQ = CONCAT(F.SEQ1, ''-''+ F.SEQ2),
	Description = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	F.Physical,
	f.NonPhysical,	
    o.FactoryID,
	o.StyleID,
	o.BrandID,
	Supplier = concat(PS.SuppID,''-''+ S.AbbEN),
	PSD.Refno,
	ColorID = isnull(psdsC.SpecValue ,''''),
	psd.SCIRefno,
	Fabric.WeaveTypeID,
	Fabric.width,
	Weight = Fabric.WeightM2,
	Fabric.ConstructionID,
	c.Name,

	[WK] = R.ExportId,
	ArriveWHDate = R.whseArrival,
	ArriveQty = RD.StockQty,
	RD.ShipQty,
	Rate = (select RateValue from [MainServer].Production.dbo.View_Unitrate v where v.FROM_U = RD.PoUnit and v.TO_U= RD.StockUnit),
	RD.Dyelot,
	RD.Roll
INTO #tmpR
from [MainServer].Production.dbo.FIR F
Inner join [MainServer].Production.dbo.Receiving R on F.ReceivingID=R.Id
Inner join [MainServer].Production.dbo.Receiving_Detail RD on R.Id=RD.Id and F.POID=RD.PoId and F.SEQ1=RD.Seq1 and F.SEQ2=RD.Seq2
Inner join [MainServer].Production.dbo.PO_Supp_Detail PSD on PSD.ID=RD.PoId and PSD.SEQ1=RD.Seq1 and PSD.SEQ2=RD.Seq2
Inner join [MainServer].Production.dbo.PO_Supp PS on PSD.ID=PS.ID and PSD.SEQ1=PS.SEQ1
Inner join [MainServer].Production.dbo.Supp S on PS.SuppID=S.ID
Inner join [MainServer].Production.dbo.Fabric on PSD.SCIRefno=Fabric.SCIRefno
Inner join [MainServer].Production.dbo.Orders O on F.POID=O.ID
left join [MainServer].Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = ''Color''
LEFT JOIN [MainServer].Production.dbo.Color c ON psd.BrandId = c.BrandId AND isnull(psdsC.SpecValue ,'''') = c.ID
Where 1=1
and (
	R.AddDate between @Date1 and @Date2
	or
	R.EditDate between @Date1 and @Date2
)
			';
	set @ExecSQL2 = N'	
	--TransferIn
SELECT
	f.ID,
	f.ReceivingID,
	F.POID,
	F.SEQ1,F.SEQ2,
	SEQ = CONCAT(F.SEQ1, ''-'' + F.SEQ2),
	Description = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	F.Physical,
	f.NonPhysical,	
    o.FactoryID,
	o.StyleID,
	o.BrandID,
	Supplier = concat(PS.SuppID,''-''+ S.AbbEN),
	PSD.Refno,
	ColorID = isnull(psdsC.SpecValue ,''''),
	psd.SCIRefno,
	Fabric.WeaveTypeID,
	Fabric.width,
	Weight = Fabric.WeightM2,
	Fabric.ConstructionID,
	c.Name,
	[WK] = '''',
	ArriveWHDate = T.IssueDate,
	TD.Qty,
	TD.Dyelot,
	TD.Roll
INTO #tmpT
from [MainServer].Production.dbo.FIR F
Inner join [MainServer].Production.dbo.TransferIn t on F.ReceivingID=t.Id
Inner join [MainServer].Production.dbo.TransferIn_Detail TD on t.Id=TD.Id and F.POID=TD.PoId and F.SEQ1=TD.Seq1 and F.SEQ2=TD.Seq2
Inner join [MainServer].Production.dbo.PO_Supp_Detail PSD on PSD.ID=TD.PoId and PSD.SEQ1=TD.Seq1 and PSD.SEQ2=TD.Seq2
Inner join [MainServer].Production.dbo.PO_Supp PS on PSD.ID=PS.ID and PSD.SEQ1=PS.SEQ1
Inner join [MainServer].Production.dbo.Supp S on PS.SuppID=S.ID
Inner join [MainServer].Production.dbo.Fabric on PSD.SCIRefno=Fabric.SCIRefno
Inner join [MainServer].Production.dbo.Orders O on F.POID=O.ID
left join [MainServer].Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = ''Color''
LEFT JOIN [MainServer].Production.dbo.Color c ON psd.BrandId = c.BrandId AND isnull(psdsC.SpecValue ,'''') = c.ID
Where 1=1
and (
	T.AddDate between @Date1 and @Date2
	or
	T.EditDate between @Date1 and @Date2
)
	'
set @ExecSQL3 = N'	
select
	t.*,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	TotalDefectyds = isnull((select SUM(Point) from [MainServer].Production.dbo.FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	FP.DetailUkey,
	FP.ActualYds,
	FP.ActualWidth,
	FP.AddDate,
	FP.EditDate,
	Composition,
	Inspector = Concat (Fp.Inspector, '' '', p.Name) 
into #tmp1
from #tmpR t
Left join [MainServer].Production.dbo.FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
Left JOIN [MainServer].Production.dbo.Pass1 p ON p.ID = Fp.Inspector
outer apply(
	select Composition = STUFF((
		select CONCAT(''+'', FLOOR(fc.percentage), ''%'', fc.MtltypeId)
		from [MainServer].Production.dbo.Fabric_Content fc
		where fc.SCIRefno = t.SCIRefno
		for xml path('''')
	),1,1,'''')
)Composition

select
	t.*,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	TotalDefectyds = isnull((select SUM(Point) from [MainServer].Production.dbo.FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	FP.DetailUkey,
	FP.ActualYds,
	FP.ActualWidth,
	FP.AddDate,
	FP.EditDate,
	Composition,
	Inspector = Concat (Fp.Inspector, '' '', p.Name) 
into #tmp2
from #tmpT t
Left join [MainServer].Production.dbo.FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
Left JOIN [MainServer].Production.dbo.Pass1 p ON p.ID = Fp.Inspector
outer apply(
	select Composition = STUFF((
		select CONCAT(''+'', FLOOR(fc.percentage), ''%'', fc.MtltypeId)
		from [MainServer].Production.dbo.Fabric_Content fc
		where fc.SCIRefno = t.SCIRefno
		for xml path('''')
	),1,1,'''')
)Composition
'

set @ExecSQL4 = N'
--Defect_detail 分頁
select
	t.POID,
	t.seq,
	WK,
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.ArriveWHDate,
	t.ArriveQty,
	t.WeaveTypeID,
	t.Dyelot,
	t.Width,
	t.Weight,
	t.Composition,
	t.Description,
	t.ConstructionID,
	t.Roll,
	t.InspDate,
	t.Result,
	t.Grade,
	Defect.DefectRecord,
	fd.Type,
	fd.DescriptionEN,
    point = isnull(Defect.point,  0),
	Defectrate = ISNULL(case when Q.PointRateOption = 1 then Defect.point / NULLIF(t.ActualYds, 0)
							when Q.PointRateOption = 2 then Defect.point * 3600 / NULLIF(t.ActualYds * t.ActualWidth , 0)
							when Q.PointRateOption = 3 then iif(t.WeaveTypeID = ''KNIT'',Defect.point * 3600 / NULLIF(t.TicketYds * t.width , 0),Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0))
							else Defect.point / NULLIF(t.ActualYds, 0)
						 end 
					, 0)
	,t.Inspector
	,t.AddDate
	,t.EditDate
INTO #Sheet2
from #tmp1 t
outer apply(
    select
	    DefectRecord = dbo.SplitDefectNum(x.Data,0),	
        point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
    from [MainServer].Production.dbo.FIR_Physical_Defect
    outer apply(select  * from SplitString(DefectRecord,''/''))x
    where FIR_PhysicalDetailUKey = t.DetailUkey
    group by dbo.SplitDefectNum(x.Data,0)
)Defect
outer apply (
	select PointRateOption
	from [MainServer].Production.dbo.QABrandSetting
	where Junk = 0 
	and BrandID = t.BrandID
)Q
left join [MainServer].Production.dbo.FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

union all
select
	t.POID,
	t.seq,
	WK = '''',
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.ArriveWHDate,
	t.Qty,
	t.WeaveTypeID,
	t.Dyelot,
	t.Width,
	t.Weight,
	t.Composition,
	t.Description,
	t.ConstructionID,
	t.Roll,
	t.InspDate,
	t.Result,
	t.Grade,
	Defect.DefectRecord,
	fd.Type,
	fd.DescriptionEN,
    point = isnull(Defect.point,  0),
	Defectrate = ISNULL(case when Q.PointRateOption = 1 then Defect.point / NULLIF(t.ActualYds, 0)
							when Q.PointRateOption = 2 then Defect.point * 3600 / NULLIF(t.ActualYds * t.ActualWidth , 0)
							when Q.PointRateOption = 3 then iif(t.WeaveTypeID = ''KNIT'',Defect.point * 3600 / NULLIF(t.TicketYds * t.width , 0),Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0))
							else Defect.point / NULLIF(t.ActualYds, 0)
						 end 
					, 0)
	,t.Inspector
	,t.AddDate
	,t.EditDate
'
set @ExecSQL5 = N'
from #tmp2 t
outer apply(
    select 
	    DefectRecord = dbo.SplitDefectNum(x.Data,0),
        point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
    from [MainServer].Production.dbo.FIR_Physical_Defect
    outer apply(select  * from SplitString(DefectRecord,''/''))x
    where FIR_PhysicalDetailUKey = t.DetailUkey
    group by dbo.SplitDefectNum(x.Data,0)
)Defect
outer apply (
	select PointRateOption
	from [MainServer].Production.dbo.QABrandSetting
	where Junk = 0 
	and BrandID = t.BrandID
)Q
left join [MainServer].Production.dbo.FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

SELECT 
	POID,seq,[Wkno] = WK,ReceivingID,StyleID,Brandid,Supplier,Refno,[Color] = ColorID,
	ArriveWHDate,ArriveQty,WeaveTypeID,Dyelot,[CutWidth] = Width,Weight,Composition,
	[Desc] = Description,[FabricConstructionID] = ConstructionID,Roll,InspDate,Result,Grade,[DefectCode] = DefectRecord,
	[DefectType] = Type, [DefectDesc] = DescriptionEN, [Points] = point,Defectrate,Inspector,AddDate	,EditDate
into #tmpFinal
FROM #Sheet2
'

set @ExecSQL6 = N'
Merge P_FabricInspReport_ReceivingTransferIn t
	using( select * from #tmpFinal) as s
	on t.POID = s.POID
	and t.SEQ = s.SEQ
	and t.ReceivingID = s.ReceivingID
	and t.Dyelot = s.Dyelot
	and t.Roll = s.Roll
	and t.DefectCode = s.DefectCode
	WHEN MATCHED then UPDATE SET	   
       t.[Wkno] = s.[Wkno]
      ,t.[StyleID] = s.[StyleID]
      ,t.[BrandID] = s.[BrandID]
      ,t.[Supplier] = s.[Supplier]
      ,t.[Refno] = s.[Refno]
      ,t.[Color] = s.[Color]
      ,t.[ArriveWHDate] = s.[ArriveWHDate]
      ,t.[ArriveQty] = s.[ArriveQty]
      ,t.[WeaveTypeID] = s.[WeaveTypeID]
      ,t.[CutWidth] = s.[CutWidth]
      ,t.[Weight] = s.[Weight]
      ,t.[Composition] = s.[Composition]
      ,t.[Desc] = s.[Desc]
      ,t.[FabricConstructionID] = s.[FabricConstructionID]
      ,t.[InspDate] = s.[InspDate]
      ,t.[Result] = s.[Result]
      ,t.[Grade] = s.[Grade]
      ,t.[DefectType] = s.[DefectType]
      ,t.[DefectDesc] = s.[DefectDesc]
      ,t.[Points] = s.[Points]
      ,t.[DefectRate] = s.[DefectRate]
      ,t.[Inspector] = s.[Inspector]
      ,t.[EditDate] = GetDate()
	WHEN NOT MATCHED BY TARGET THEN
insert (
	 [POID],[SEQ],[Wkno],[ReceivingID],[StyleID],[BrandID],[Supplier],[Refno],[Color],[ArriveWHDate],[ArriveQty],[WeaveTypeID]
	 ,[Dyelot],[CutWidth],[Weight],[Composition],[Desc],[FabricConstructionID],[Roll],[InspDate],[Result],[Grade]
      ,[DefectCode],[DefectType],[DefectDesc],[Points],[DefectRate],[Inspector],[AddDate]
)
VALUES (
       s.[POID],s.[SEQ],s.[Wkno],s.[ReceivingID],s.[StyleID],s.[BrandID],s.[Supplier],s.[Refno],s.[Color],s.[ArriveWHDate],s.[ArriveQty]
      ,s.[WeaveTypeID],s.[Dyelot],s.[CutWidth],s.[Weight],s.[Composition],s.[Desc],s.[FabricConstructionID],s.[Roll],s.[InspDate]
      ,s.[Result],s.[Grade],s.[DefectCode],s.[DefectType],s.[DefectDesc],s.[Points],s.[DefectRate],s.[Inspector]
      ,GetDate()
)
when not matched by source and t.[ReceivingID] in (select ReceivingID from #Sheet2)
	then delete;
'
	print @ExecSQL1;
	print @ExecSQL2
	print @ExecSQL3
	print @ExecSQL4
	print @ExecSQL5
	print @ExecSQL6
	Exec (@ExecSQL1 + @ExecSQL2 + @ExecSQL3+@ExecSQL4+@ExecSQL5+@ExecSQL6);

	if exists (select 1 from BITableInfo b where b.id = 'P_FabricInspReport_ReceivingTransferIn')
	begin
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = 'P_FabricInspReport_ReceivingTransferIn'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_FabricInspReport_ReceivingTransferIn', getdate())
	end
End


GO