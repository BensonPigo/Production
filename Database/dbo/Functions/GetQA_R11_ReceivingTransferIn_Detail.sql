Create function [dbo].[GetQA_R11_ReceivingTransferIn_Detail]
(
	 @EditDate1    varchar(30) = ''
	,@EditDate2    varchar(30) = ''
	,@ArriveWHDate1    varchar(30) = '' --for 第一次匯入用
	,@ArriveWHDate2    varchar(30) = '' --for 第一次匯入用 
	
)
RETURNs @RtnTable TABLE
(
	POID varchar(13),
	SEQ varchar(10),
	WKNO varchar(13),
	RECEIVINGID varchar(13),
	STYLEID varchar(15),
	BRANDID varchar(8),
	SUPPLIER varchar(25),
	REFNO varchar(36),
	COLOR varchar(50),
	ARRIVEWHDATE date,
	ARRIVEQTY numeric(11,2),
	WEAVETYPEID varchar(20),
	DYELOT varchar(8),
	CUTWIDTH numeric(5,2),
	WEIGHT numeric(5,1),
	COMPOSITION nvarchar(500),
	[DESC] nvarchar(max),
	[FABRICCONSTRUCTIONID] varchar(20),
	ROLL varchar(8),
	INSPDATE DATE,
	RESULT varchar(5),
	GRADE varchar(10),
	DEFECTRECORD varchar(2),
	DEFECTTYPE varchar(20),
	DEFECTDESC varchar(60),
	POINTS int,
	DEFECTRATE numeric(9,2),
	INSPECTOR nvarchar(40),
	AddDate datetime,
	EditDate datetime
)
As
Begin

Declare @tmpR table(
    ID  bigint,
	ReceivingID varchar(13),
	POID varchar(13),
	SEQ1 varchar(3),
	SEQ2 varchar(2),
	SEQ varchar(10),
	Description nvarchar(max),
	Physical varchar(10),
	NonPhysical bit,
	FactoryID varchar(20),
	StyleID varchar(15),
	BrandID varchar(8),
	Supplier varchar(25),
	Refno varchar(36),
	ColorID varchar(50),
	SCIRefno varchar(36),
	WeaveTypeID varchar(20),
	width numeric(5,2),
	Weight numeric(5,1),
	ConstructionID varchar(20),
	Name nvarchar(150),
	[WK] varchar(13),
	ArriveWHDate date,
	ArriveQty numeric(11,2),
	ShipQty numeric(11,2),
	Rate numeric(38,10),
	Dyelot varchar(8),
	Roll varchar(8)
)
--Receiving
insert into @tmpR
SELECT
	[ID] = f.ID,
	[ReceivingID] = f.ReceivingID,
	[POID] = F.POID,
	[SEQ1] = F.SEQ1 ,
	[SEQ2] = F.SEQ2,
	[SEQ] = CONCAT(F.SEQ1, '-' + F.SEQ2),
	[Description] = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	[Physical] = F.Physical,
	[NonPhysical] = f.NonPhysical,	
    [FactoryID] = o.FactoryID,
	[StyleID] = o.StyleID,
	[BrandID] = o.BrandID,
	[Supplier] = concat(PS.SuppID,'-'+ S.AbbEN),
	[Refno] = PSD.Refno,
	[ColorID] = isnull(psdsC.SpecValue ,''),
	[SCIRefno] = psd.SCIRefno,
	[WeaveTypeID] = Fabric.WeaveTypeID,
	[width] = Fabric.width,
	[Weight] = Fabric.WeightM2,
	[ConstructionID] = Fabric.ConstructionID,
	[Name] = c.Name,
	[WK] = R.ExportId,
	[ArriveWHDate] = R.whseArrival,
	[ArriveQty] = RD.StockQty,
	[ShipQty] = RD.ShipQty,
	[Rate] = (select RateValue from dbo.View_Unitrate v where v.FROM_U = RD.PoUnit and v.TO_U= RD.StockUnit),
	[Dyelot] = RD.Dyelot,
	[Roll] = RD.Roll
from FIR F
Inner join Receiving R on F.ReceivingID=R.Id
Inner join Receiving_Detail RD on R.Id=RD.Id and F.POID=RD.PoId and F.SEQ1=RD.Seq1 and F.SEQ2=RD.Seq2
Inner join PO_Supp_Detail PSD on PSD.ID=RD.PoId and PSD.SEQ1=RD.Seq1 and PSD.SEQ2=RD.Seq2
Inner join PO_Supp PS on PSD.ID=PS.ID and PSD.SEQ1=PS.SEQ1
Inner join Supp S on PS.SuppID=S.ID
Inner join Fabric on PSD.SCIRefno=Fabric.SCIRefno
Inner join Orders O on F.POID=O.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
LEFT JOIN Color c ON psd.BrandId = c.BrandId AND isnull(psdsC.SpecValue ,'') = c.ID
Where 1=1
and (@ArriveWHDate1 is null or R.WhseArrival >= @ArriveWHDate1)
and (@ArriveWHDate2 is null or R.WhseArrival <= @ArriveWHDate2)
and 
(
	( 
		(@EditDate1 is null or R.AddDate >= @EditDate1)
	and (@EditDate2 is null or R.AddDate <= @EditDate2)
	)
	or
	(
		(@EditDate1 is null or R.EditDate >= @EditDate1)
	and (@EditDate2 is null or R.EditDate <= @EditDate2)
	)
)


--TransferIn
Declare @tmpT table(
    ID  bigint,
	ReceivingID varchar(13),
	POID varchar(13),
	SEQ1 varchar(3),
	SEQ2 varchar(2),
	SEQ varchar(10),
	Description nvarchar(max),
	Physical varchar(10),
	NonPhysical bit,
	FactoryID varchar(20),
	StyleID varchar(15),
	BrandID varchar(8),
	Supplier varchar(25),
	Refno varchar(36),
	ColorID varchar(50),
	SCIRefno varchar(36),
	WeaveTypeID varchar(20),
	width numeric(5,2),
	Weight numeric(5,1),
	ConstructionID varchar(20),
	Name nvarchar(150),
	[WK] varchar(13),
	ArriveWHDate date,
	Qty numeric(11,2),		
	Dyelot varchar(8),
	Roll varchar(8)
)
insert into @tmpT
SELECT
	[ID] = f.ID,
	[ReceivingID] = f.ReceivingID,
	[POID] = F.POID, 
	[SEQ1] = F.SEQ1,[SEQ2] = F.SEQ2,
	[SEQ] = CONCAT(F.SEQ1, '-' + F.SEQ2),
	[Description] = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	[Physical] = F.Physical,
	[NonPhysical] = f.NonPhysical,	
    [FactoryID] = o.FactoryID,
	[StyleID] = o.StyleID,
	[BrandID] = o.BrandID,
	[Supplier] = concat(PS.SuppID,'-'+ S.AbbEN),
	[Refno] = PSD.Refno,
	[ColorID] = isnull(psdsC.SpecValue ,''),
	[SCIRefno] = psd.SCIRefno,
	[WeaveTypeID] = Fabric.WeaveTypeID,
	[width] = Fabric.width,
	[Weight] = Fabric.WeightM2,
	[ConstructionID] = Fabric.ConstructionID,
	[Name] = c.Name,
	[WK] = '',
	[ArriveWHDate] = T.IssueDate,
	[Qty] = TD.Qty,
	[Dyelot] = TD.Dyelot,
	[Roll] = TD.Roll
from FIR F
Inner join TransferIn t on F.ReceivingID=t.Id
Inner join TransferIn_Detail TD on t.Id=TD.Id and F.POID=TD.PoId and F.SEQ1=TD.Seq1 and F.SEQ2=TD.Seq2
Inner join PO_Supp_Detail PSD on PSD.ID=TD.PoId and PSD.SEQ1=TD.Seq1 and PSD.SEQ2=TD.Seq2
Inner join PO_Supp PS on PSD.ID=PS.ID and PSD.SEQ1=PS.SEQ1
Inner join Supp S on PS.SuppID=S.ID
Inner join Fabric on PSD.SCIRefno=Fabric.SCIRefno
Inner join Orders O on F.POID=O.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
LEFT JOIN Color c ON psd.BrandId = c.BrandId AND isnull(psdsC.SpecValue ,'') = c.ID
Where 1=1
and (@ArriveWHDate1 is null or T.IssueDate >= @ArriveWHDate1)
and (@ArriveWHDate2 is null or T.IssueDate <= @ArriveWHDate2)
and 
(
	( 
		(@EditDate1 is null or T.AddDate >= @EditDate1)
	and (@EditDate2 is null or T.AddDate <= @EditDate2)
	)
	or
	(
		(@EditDate1 is null or T.EditDate >= @EditDate1)
	and (@EditDate2 is null or T.EditDate <= @EditDate2)
	)
)


Declare @tmp1 table(
    ID  bigint,
	ReceivingID varchar(13),
	POID varchar(13),
	SEQ1 varchar(3),
	SEQ2 varchar(2),
	SEQ varchar(10),
	Description nvarchar(max),
	Physical varchar(10),
	NonPhysical bit,
	FactoryID varchar(20),
	StyleID varchar(15),
	BrandID varchar(8),
	Supplier varchar(25),
	Refno varchar(36),
	ColorID varchar(50),
	SCIRefno varchar(36),
	WeaveTypeID varchar(20),
	width numeric(5,2),
	Weight numeric(5,1),
	ConstructionID varchar(20),
	Name nvarchar(150),
	[WK] varchar(13),
	ArriveWHDate date,
	ArriveQty numeric(11,2),
	ShipQty numeric(11,2),
	Rate numeric(38,10),
	Dyelot varchar(8),
	Roll varchar(8),
	InspDate DATE,
	Result varchar(5),
	Grade varchar(10),
	TotalDefectyds numeric(16,4),
	TicketYds numeric(8,2),
	DetailUkey BIGINT,
	ActualYds NUMERIC(8,2),
	ActualWidth NUMERIC(5,2),
	Composition nvarchar(500),
	Inspector nvarchar(40),
	AddDate datetime,
	EditDate datetime
)
INSERT into @tmp1
select
	[ID] = t.ID,
	[ReceivingID] = t.ReceivingID,
	[POID] = t.POID,
	[SEQ1] = t.SEQ1 ,[SEQ2] = t.SEQ2,
	[SEQ] = t.SEQ,
	[Description] = t.Description,
	[Physical] = t.Physical,
	[NonPhysical] = t.NonPhysical,	
    [FactoryID] = t.FactoryID,
	[StyleID] = t.StyleID,
	[BrandID] = t.BrandID,
	[Supplier] = t.Supplier,
	[Refno] = t.Refno,
	[ColorID] = t.ColorID,
	[SCIRefno] = t.SCIRefno,
	[WeaveTypeID] = t.WeaveTypeID,
	[width] = t.width,
	[Weight] = t.Weight,
	[ConstructionID] = t.ConstructionID,
	[Name] = t.Name,
	[WK] = t.WK,
	[ArriveWHDate] = t.ArriveWHDate,
	[ArriveQty] = t.ArriveQty,
	[ShipQty] = t.ShipQty,
	[Rate] = t.Rate,
	[Dyelot] = t.Dyelot,
	[Roll] = t.Roll,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	TotalDefectyds = isnull((select SUM(Point) from FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	FP.DetailUkey,
	FP.ActualYds,
	FP.ActualWidth,
	Composition,
	Inspector = Concat (Fp.Inspector, ' ', p.Name) ,
	FP.AddDate,
	FP.EditDate
from @tmpR t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
Left JOIN Pass1 p ON p.ID = Fp.Inspector
outer apply(
	select Composition = STUFF((
		select CONCAT('+', FLOOR(fc.percentage), '%', fc.MtltypeId)
		from Fabric_Content fc
		where fc.SCIRefno = t.SCIRefno
		for xml path('')
	),1,1,'')
)Composition

Declare @tmp2 table(
    ID  bigint,
	ReceivingID varchar(13),
	POID varchar(13),
	SEQ1 varchar(3),
	SEQ2 varchar(2),
	SEQ varchar(10),
	Description nvarchar(max),
	Physical varchar(10),
	NonPhysical bit,
	FactoryID varchar(20),
	StyleID varchar(15),
	BrandID varchar(8),
	Supplier varchar(25),
	Refno varchar(36),
	ColorID varchar(50),
	SCIRefno varchar(36),
	WeaveTypeID varchar(20),
	width numeric(5,2),
	Weight numeric(5,1),
	ConstructionID varchar(20),
	Name nvarchar(150),
	[WK] varchar(13),
	ArriveWHDate date,
	Qty numeric(11,2),	
	Dyelot varchar(8),
	Roll varchar(8),
	InspDate DATE,
	Result varchar(5),
	Grade varchar(10),
	TotalDefectyds numeric(16,4),
	TicketYds numeric(8,2),
	DetailUkey BIGINT,
	ActualYds NUMERIC(8,2),
	ActualWidth NUMERIC(5,2),
	Composition nvarchar(500),
	Inspector nvarchar(40),
	AddDate datetime,
	EditDate datetime
)
insert into @tmp2
select
	[ID] = t.ID,
	[ReceivingID] = t.ReceivingID,
	[POID] = t.POID,
	[SEQ1] = t.SEQ1 ,[SEQ2] = t.SEQ2,
	[SEQ] = t.SEQ,
	[Description] = t.Description,
	[Physical] = t.Physical,
	[NonPhysical] = t.NonPhysical,	
    [FactoryID] = t.FactoryID,
	[StyleID] = t.StyleID,
	[BrandID] = t.BrandID,
	[Supplier] = t.Supplier,
	[Refno] = t.Refno,
	[ColorID] = t.ColorID,
	[SCIRefno] = t.SCIRefno,
	[WeaveTypeID] = t.WeaveTypeID,
	[width] = t.width,
	[Weight] = t.Weight,
	[ConstructionID] = t.ConstructionID,
	[Name] = t.Name,
	[WK] = t.WK,
	[ArriveWHDate] = t.ArriveWHDate,
	[Qty] = t.Qty,	
	[Dyelot] = t.Dyelot,
	[Roll] = t.Roll,
	[InspDate] = FP.InspDate,
	[Result] = FP.Result,
	[Grade] = FP.Grade,
	[TotalDefectyds] = isnull((select SUM(Point) from FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	[TicketYds] = FP.TicketYds,
	[DetailUkey] = FP.DetailUkey,
	[ActualYds] = FP.ActualYds,
	[ActualWidth] = FP.ActualWidth,
	[Composition] = Composition.Composition,
	[Inspector] = Concat (Fp.Inspector, ' ', p.Name) ,
	FP.AddDate,
	FP.EditDate
from @tmpT t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
Left JOIN Pass1 p ON p.ID = Fp.Inspector
outer apply(
	select Composition = STUFF((
		select CONCAT('+', FLOOR(fc.percentage), '%', fc.MtltypeId)
		from Fabric_Content fc
		where fc.SCIRefno = t.SCIRefno
		for xml path('')
	),1,1,'')
)Composition;



with Final as (
	select POID = isnull(POID,''),SEQ = isnull(SEQ,''),WK = isnull(WK,''),RECEIVINGID = isnull(RECEIVINGID,''),STYLEID = isnull(STYLEID,''),BRANDID = isnull(BRANDID,''),SUPPLIER = isnull(SUPPLIER,''),REFNO = isnull(REFNO,''),COLORID = isnull(COLORID,''),
			ARRIVEWHDATE,ARRIVEQTY = isnull(ARRIVEQTY,0),WEAVETYPEID = isnull(WEAVETYPEID,''),DYELOT = isnull(DYELOT,''),WIDTH = isnull(WIDTH,0),[WEIGHT] = isnull(WEIGHT,0),COMPOSITION = isnull(COMPOSITION,''),
			[DESCRIPTION] = isnull(DESCRIPTION,'') ,CONSTRUCTIONID = isnull(CONSTRUCTIONID,'') ,ROLL = isnull(ROLL,''),INSPDATE,RESULT = isnull(RESULT,''),GRADE = isnull(GRADE,''),DEFECTRECORD = isnull(DEFECTRECORD,''),
			[TYPE] = isnull(TYPE,'') ,DESCRIPTIONEN = isnull(DESCRIPTIONEN,'') ,[POINT] = isnull([POINT],0),[DEFECTRATE] = isnull([DEFECTRATE],0) ,INSPECTOR = isnull(INSPECTOR,''),AddDate,EditDate
			,RowCnt = ROW_NUMBER() over(partition by POID,SEQ,RECEIVINGID,roll,dyelot,DEFECTRECORD order by AddDate desc, EditDate Desc)
	from (
		select
			T.POID,
			T.SEQ,
			T.WK,
			T.RECEIVINGID,
			T.STYLEID,
			T.BRANDID,
			T.SUPPLIER,
			T.REFNO,
			T.COLORID,
			T.ARRIVEWHDATE,
			T.ARRIVEQTY,
			T.WEAVETYPEID,
			T.DYELOT,
			T.WIDTH,
			T.WEIGHT,
			T.COMPOSITION,
			T.DESCRIPTION,
			T.CONSTRUCTIONID,
			T.ROLL,
			T.INSPDATE,
			T.RESULT,
			T.GRADE,
			DEFECT.DEFECTRECORD,
			FD.TYPE,
			FD.DESCRIPTIONEN,
			POINT = isnull(Defect.point,  0),
			DEFECTRATE = ISNULL(case when Q.PointRateOption = 1 then round((Defect.point / NULLIF(t.ActualYds, 0)) * 100 ,2)
									when Q.PointRateOption = 2 then round( (Defect.point * 3600 / NULLIF(t.ActualYds * t.ActualWidth , 0)) * 100,2)
									when Q.PointRateOption = 3 then iif(t.WeaveTypeID = 'KNIT',round((Defect.point * 3600 / NULLIF(t.TicketYds * t.width , 0)) * 100 ,2) ,round((Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0)) *100,2))
									when Q.PointRateOption = 4 then round( (Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0)) * 100,2)
									else round((Defect.point / NULLIF(t.ActualYds, 0))*100,2)
								 end 
							, 0)
			,T.INSPECTOR
			,t.AddDate
			,t.EditDate
		from @tmp1 t
		outer apply(
			select
				DefectRecord = dbo.SplitDefectNum(x.Data,0),	
				point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
			from FIR_Physical_Defect
			outer apply(select  * from SplitString(DefectRecord,'/'))x
			where FIR_PhysicalDetailUKey = t.DetailUkey
			group by dbo.SplitDefectNum(x.Data,0)
		)Defect
		outer apply (
			select PointRateOption
			from QABrandSetting
			where Junk = 0 
			and BrandID = t.BrandID
		)Q
		left join FabricDefect fd on fd.ID = Defect.DefectRecord
		where Defect.DefectRecord is not null or fd.Type is not null

		union all
		select
			T.POID,
			T.SEQ,
			WK = '',
			T.RECEIVINGID,
			T.STYLEID,
			T.BRANDID,
			T.SUPPLIER,
			T.REFNO,
			T.COLORID,
			T.ARRIVEWHDATE,
			ARRIVEQTY = T.QTY,
			T.WEAVETYPEID,
			T.DYELOT,
			T.WIDTH,
			T.WEIGHT,
			T.COMPOSITION,
			T.DESCRIPTION,
			T.CONSTRUCTIONID,
			T.ROLL,
			T.INSPDATE,
			T.RESULT,
			T.GRADE,
			DEFECT.DEFECTRECORD,
			FD.TYPE,
			FD.DESCRIPTIONEN,
			POINT = isnull(Defect.point,  0),
			DEFECTRATE = ISNULL(case when Q.PointRateOption = 1 then round((Defect.point / NULLIF(t.ActualYds, 0)) * 100 ,2)
									when Q.PointRateOption = 2 then round( (Defect.point * 3600 / NULLIF(t.ActualYds * t.ActualWidth , 0)) * 100,2)
									when Q.PointRateOption = 3 then iif(t.WeaveTypeID = 'KNIT',round((Defect.point * 3600 / NULLIF(t.TicketYds * t.width , 0)) * 100 ,2) ,round((Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0)) *100,2))
									when Q.PointRateOption = 4 then round( (Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0)) * 100,2)
									else round((Defect.point / NULLIF(t.ActualYds, 0))*100,2)
								 end 
							, 0)
			,T.INSPECTOR
			,t.AddDate
			,t.EditDate
		from @tmp2 t
		outer apply(
			select 
				DefectRecord = dbo.SplitDefectNum(x.Data,0),
				point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
			from FIR_Physical_Defect
			outer apply(select  * from SplitString(DefectRecord,'/'))x
			where FIR_PhysicalDetailUKey = t.DetailUkey
			group by dbo.SplitDefectNum(x.Data,0)
		)Defect
		outer apply (
			select PointRateOption
			from QABrandSetting
			where Junk = 0 
			and BrandID = t.BrandID
		)Q
		left join FabricDefect fd on fd.ID = Defect.DefectRecord
		where Defect.DefectRecord is not null or fd.Type is not null
	) A
) 
INSERT into @RtnTable(
		POID,SEQ,WKNO,RECEIVINGID,STYLEID,BRANDID,SUPPLIER,REFNO,COLOR,
		ARRIVEWHDATE,ARRIVEQTY,WEAVETYPEID,DYELOT,CUTWIDTH,WEIGHT,COMPOSITION,
		[DESC] ,[FABRICCONSTRUCTIONID] ,ROLL,INSPDATE,RESULT,GRADE,DEFECTRECORD,
		[DEFECTTYPE] ,[DEFECTDESC] ,[POINTS],[DEFECTRATE] ,INSPECTOR,AddDate,EditDate
)

select 	POID,SEQ,WK,RECEIVINGID,STYLEID,BRANDID,SUPPLIER,REFNO,COLORID,
		ARRIVEWHDATE,ARRIVEQTY,WEAVETYPEID,DYELOT,WIDTH,WEIGHT,COMPOSITION,
		[DESCRIPTION] ,CONSTRUCTIONID ,ROLL,INSPDATE,RESULT,GRADE,DEFECTRECORD,
		[TYPE] ,DESCRIPTIONEN ,[POINT],[DEFECTRATE] ,INSPECTOR,AddDate,EditDate
from Final
where RowCnt = 1

return

end

