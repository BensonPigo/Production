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
    public class QA_R11
    {
        /// <inheritdoc/>
        public QA_R11()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetQA_R11Data(QA_R11_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@Date1", SqlDbType.Date) { Value = (object)model.ArriveWHDate1 ?? DBNull.Value },
                new SqlParameter("@Date2", SqlDbType.Date) { Value = (object)model.ArriveWHDate2 ?? DBNull.Value },

                new SqlParameter("@sp1", SqlDbType.VarChar, 20) { Value = model.SP1 },
                new SqlParameter("@sp2", SqlDbType.VarChar, 20) { Value = model.SP2 },
                new SqlParameter("@Refno1", SqlDbType.VarChar, 36) { Value = model.Refno1 },
                new SqlParameter("@Refno2", SqlDbType.VarChar, 36) { Value = model.Refno2 },
                new SqlParameter("@Brand", SqlDbType.VarChar, 10) { Value = model.Brand },
                new SqlParameter("@IsPowerBI", SqlDbType.Bit) { Value = model.IsPowerBI },
            };

            StringBuilder sqlCmd = new StringBuilder();
            #region Where

            string where1 = string.Empty;
            string where2 = string.Empty;
            string wherecommon = string.Empty;
            string whereBI1 = string.Empty;
            string whereBI2 = string.Empty;

            if (model.IsPowerBI == false)
            {
				if (!MyUtility.Check.Empty(model.ArriveWHDate1))
				{
                    where1 += $"and R.WhseArrival >= @Date1" + Environment.NewLine;
                    where2 += $"and T.IssueDate >= @Date1" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.ArriveWHDate2))
                {
                    where1 += $"and R.WhseArrival <= @Date2" + Environment.NewLine;
                    where2 += $"and T.IssueDate <= @Date2" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.SP1))
                {
                    wherecommon += $"and F.POID between @SP1 and @SP2" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.Brand))
                {
                    wherecommon += $"and O.BrandID = @Brand" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.Refno1))
                {
                    wherecommon += $"and PSD.Refno >= @Refno1" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.Refno2))
                {
                    wherecommon += $"and PSD.Refno <= @Refno2" + Environment.NewLine;
                }
            }
            else
            {
                whereBI1 = @"
and 
(
	(R.AddDate between @Date1 and @Date2)
	or
	(R.EditDate between @Date1 and @Date2)
)
" + Environment.NewLine;

                whereBI2 = @"
and 
(
	(T.AddDate between @Date1 and @Date2)
	or
	(T.EditDate between @Date1 and @Date2)
)
" + Environment.NewLine;

            }

            #endregion

            #region SQL

            string biColumn = MyUtility.Check.Empty(model.IsPowerBI) ? "" : @",AddDate,EditDate";

            // 基本資料
            sqlCmd.Append($@"
--Receiving
SELECT
	f.ID,
	f.ReceivingID,
	F.POID,
	F.SEQ1,F.SEQ2,
	SEQ = CONCAT(F.SEQ1, '-' + F.SEQ2),
	Description = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	F.Physical,
	f.NonPhysical,	
    o.FactoryID,
	o.StyleID,
	o.BrandID,
	Supplier = concat(PS.SuppID,'-'+ S.AbbEN),
	PS.SuppID,
	PSD.Refno,
	ColorID = isnull(psdsC.SpecValue ,''),
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
	Rate = (select RateValue from dbo.View_Unitrate v where v.FROM_U = RD.PoUnit and v.TO_U= RD.StockUnit),
	RD.Dyelot,
	RD.Roll
INTO #tmpR
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
{whereBI1}
{where1}
{wherecommon}

--TransferIn
SELECT
	f.ID,
	f.ReceivingID,
	F.POID,
	F.SEQ1,F.SEQ2,
	SEQ = CONCAT(F.SEQ1, '-' + F.SEQ2),
	Description = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	F.Physical,
	f.NonPhysical,	
    o.FactoryID,
	o.StyleID,
	o.BrandID,
	Supplier = concat(PS.SuppID,'-'+ S.AbbEN),
	PS.SuppID,
	PSD.Refno,
	ColorID = isnull(psdsC.SpecValue ,''),
	psd.SCIRefno,
	Fabric.WeaveTypeID,
	Fabric.width,
	Weight = Fabric.WeightM2,
	Fabric.ConstructionID,
	c.Name,

	[WK] = '',
	ArriveWHDate = T.IssueDate,
	TD.Qty,
	TD.Dyelot,
	TD.Roll
INTO #tmpT
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
{whereBI2}
{where2}
{wherecommon}
");

            // 各sheet
            sqlCmd.Append($@"
select
	t.*,
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
into #tmp1
from #tmpR t
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


select
	t.*,
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
into #tmp2
from #tmpT t
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


--Summary分頁
select
	t.POID,
    t.SEQ,
	t.WK,
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.ArriveWHDate,
	ArriveQty = SUM(t.ArriveQty),
	t.WeaveTypeID,
	t.Dyelot,
	t.Width,
	t.Weight,
	t.Composition,
	t.Description,
	t.ConstructionID,
	ShippedQty = SUM(t.ShipQty * isnull(t.Rate,1)),
    t.FactoryID,
	RFT = iif(SUM(t.ArriveQty) = 0, 0, isnull(SUM(TotalDefectyds), 0) / SUM(t.ArriveQty)),
	TotalDefectyds = Sum(TotalDefectyds),
	Inspection = iif(SUM(t.ArriveQty) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.ArriveQty)),
	t.Physical,
	[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), 
    [BIInsertDate] = GETDATE()
from #tmp1 t
Group by t.POID,t.SEQ,t.WK,t.ReceivingID,t.StyleID,t.Brandid,t.Supplier,t.Refno,t.ColorID,t.ArriveWHDate,t.WeaveTypeID,
	t.Dyelot,t.Width,t.Weight,t.Composition,t.Description,t.ConstructionID,t.FactoryID,t.Physical

union all
select
	t.POID,
    t.SEQ,
    WK,
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.ArriveWHDate,
	ArrivedQty = SUM(t.Qty),
	t.WeaveTypeID,
	t.Dyelot,
	t.Width,
	t.Weight,
	t.Composition,
	t.Description,
	t.ConstructionID,
	ShippedQty = SUM(t.Qty),
    t.FactoryID,
	RFT = iif(SUM(t.Qty) = 0, 0, isnull(SUM(TotalDefectyds), 0) / SUM(t.Qty)),
	TotalDefectyds = Sum(TotalDefectyds),
	Inspection  = iif(SUM(t.Qty) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.Qty)),
	t.Physical,
	[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), 
    [BIInsertDate] = GETDATE()
from #tmp2 t
Group by t.POID,t.SEQ,t.WK,t.ReceivingID,t.StyleID,t.Brandid,t.Supplier,t.Refno,t.ColorID,t.ArriveWHDate,t.WeaveTypeID,
	t.Dyelot,t.Width,t.Weight,t.Composition,t.Description,t.ConstructionID,t.FactoryID,t.Physical

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
	Defect.T2Points,
	Defect.FactoryPoints,
    point = isnull(Defect.point,  0),
	Defectrate = ISNULL(case when Q.PointRateOption = 1 then Defect.point / NULLIF(t.ActualYds, 0)
							when Q.PointRateOption = 2 then (Defect.point * 3600 / NULLIF(t.ActualYds * t.ActualWidth , 0))/100
							when Q.PointRateOption = 3 then iif(t.WeaveTypeID = 'KNIT',(Defect.point * 3600 / NULLIF(t.TicketYds * t.width , 0))/100,(Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0))/100)
							 when Q.PointRateOption = 4 and SpecialSupp_Formula.HasValue =  'True' then (Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0))/100
							 when Q.PointRateOption = 4 and SpecialSupp_Formula.HasValue <> 'True' then Defect.point / NULLIF(t.ActualYds, 0)
							else Defect.point / NULLIF(t.ActualYds, 0)
						 end 
					, 0)
	,t.Inspector
	,RowCnt = ROW_NUMBER() over(partition by t.POID,t.SEQ,t.RECEIVINGID,t.roll,t.dyelot,DEFECT.DEFECTRECORD order by t.AddDate desc, t.EditDate Desc)
	,t.AddDate
	,t.EditDate
INTO #Sheet2
from #tmp1 t
outer apply(
    SELECT 
    FabricdefectID AS DefectRecord,
    COUNT(CASE WHEN T2 = '1' THEN 1 END) AS T2Points,
    COUNT(CASE WHEN T2 = '0' THEN 1 END) AS FactoryPoints,
	COUNT(T2) AS point
FROM FIR_Physical_Defect_Realtime
WHERE FIR_PhysicalDetailUkey = t.DetailUkey
GROUP BY FIR_PhysicalDetailUkey, FabricdefectID
)Defect
outer apply (
	select PointRateOption
	from QABrandSetting
	where Junk = 0 
	and BrandID = t.BrandID
)Q
outer apply(
	select top 1 HasValue = 'True'
	from FIR_PointRateFormula
	where BrandID= t.Brandid
	and SuppID=t.SuppID
	and WeaveTypeID = t.WeaveTypeID
)SpecialSupp_Formula
left join FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

union all
select
	t.POID,
	t.seq,
	WK = '',
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
	Defect.T2Points,
	Defect.FactoryPoints,
    point = isnull(Defect.point,  0),
	Defectrate = ISNULL(case when Q.PointRateOption = 1 then Defect.point / NULLIF(t.ActualYds, 0)
							when Q.PointRateOption = 2 then (Defect.point * 3600 / NULLIF(t.ActualYds * t.ActualWidth , 0))/100
							when Q.PointRateOption = 3 then iif(t.WeaveTypeID = 'KNIT',(Defect.point * 3600 / NULLIF(t.TicketYds * t.width , 0))/100,(Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0))/100)
							 when Q.PointRateOption = 4 and SpecialSupp_Formula.HasValue =  'True' then (Defect.point * 3600 / NULLIF(t.ActualYds * t.width , 0))/100
							 when Q.PointRateOption = 4 and SpecialSupp_Formula.HasValue <> 'True' then Defect.point / NULLIF(t.ActualYds, 0)
							else Defect.point / NULLIF(t.ActualYds, 0)
						 end 
					, 0)
	,t.Inspector
	,RowCnt = ROW_NUMBER() over(partition by t.POID,t.SEQ,t.RECEIVINGID,t.roll,t.dyelot,DEFECT.DEFECTRECORD order by t.AddDate desc, t.EditDate Desc)
	,t.AddDate
	,t.EditDate
from #tmp2 t
outer apply(
    SELECT 
    FabricdefectID AS DefectRecord,
    COUNT(CASE WHEN T2 = '1' THEN 1 END) AS T2Points,
    COUNT(CASE WHEN T2 = '0' THEN 1 END) AS FactoryPoints,
	COUNT(T2) AS point
FROM FIR_Physical_Defect_Realtime
WHERE FIR_PhysicalDetailUkey = t.DetailUkey
GROUP BY FIR_PhysicalDetailUkey, FabricdefectID
)Defect
outer apply (
	select PointRateOption
	from QABrandSetting
	where Junk = 0 
	and BrandID = t.BrandID
)Q
outer apply(
	select top 1 HasValue = 'True'
	from FIR_PointRateFormula
	where BrandID= t.Brandid
	and SuppID=t.SuppID
	and WeaveTypeID = t.WeaveTypeID
)SpecialSupp_Formula
left join FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

SELECT 
	POID,seq,Wkno = WK,ReceivingID,StyleID,BrandID,Supplier,Refno,Color = ColorID,
	ArriveWHDate,ArriveQty,WeaveTypeID,Dyelot, CUTWIDTH = Width,Weight,Composition,
	[DESC] = [Description],[FABRICCONSTRUCTIONID] = ConstructionID,Roll,InspDate,Result,Grade,[DefectCode] = DefectRecord,
	[DefectType] = [Type],[DEFECTDESC] = DescriptionEN,[T2 Points] = T2Points,[Factory Points] = FactoryPoints,[Points] = point,Defectrate,Inspector {biColumn}
	,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    ,[BIInsertDate] = GETDATE()
FROM #Sheet2
where RowCnt = 1

--Lacking yard分頁
select
	t.POID,
	t.SEQ,
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
	ShippedQty = t.ShipQty * isnull(t.Rate,1),
	t.Roll,
	t.TicketYds,
	t.ActualYds,
	LackingYard = isnull(t.TicketYds, 0) - isnull(t.ActualYds, 0),
	[InspDate] = FORMAT(t.InspDate, 'yyyy/MM/dd'),
	[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]),
    [BIInsertDate] = GETDATE()
from #tmp1 t
where t.InspDate is not null

union all
select
	t.POID,
	t.SEQ,
	WK = '',
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.ArriveWHDate,
	ArrivedQty = t.qty,
	t.WeaveTypeID,
	t.Dyelot,
	t.Width,
	t.Weight,
	t.Composition,
	ShippedQty = t.Qty,
	t.Roll,
	t.TicketYds,
	t.ActualYds,
	LackingYard = isnull(t.TicketYds, 0) - isnull(t.ActualYds, 0),
	[InspDate] = FORMAT(t.InspDate, 'yyyy/MM/dd'),
	[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]),
	[BIInsertDate] = GETDATE()
from #tmp2 t
where t.InspDate is not null

----Defect Summary Report 分頁
SELECT ID,ReceivingID,WK,POID,SEQ1,SEQ2,Seq,ArriveWHDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID,ArriveQty = sum(ArriveQty)
INTO #tmp3
from #tmpR
group by ID,ReceivingID,WK,POID,SEQ1,SEQ2,Seq,ArriveWHDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID

union all
SELECT ID,ReceivingID,WK,POID,SEQ1,SEQ2,Seq,ArriveWHDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID,ArriveQty = sum(Qty)
from #tmpT
group by ID,ReceivingID,WK,POID,SEQ1,SEQ2,Seq,ArriveWHDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID

Select
	POID,
	SEQ,
	WK,
	ReceivingID,
	StyleID,
	BrandID,
	Supplier,
	Refno,
	ColorID,
	ArriveWHDate,
	ArriveQty,
	WeaveTypeID,
	SCIRefno,
	Name,
	[ArriveRoll]=ArriveRoll.Val,
	NonPhysical,
	[HowManyDyelotArrived] = HowManyDyelotArrived.Val,
	[AlreadyInspecetedDyelot] = AlreadyInspecetedDyelot.Val,
	[InspectionPercentage] = InspectionPercentage.Val,
	[DefectCode] = DefectCode.DefectRecord,
	[TotalPoints] = DefectCode.point
INTO #DefectSummary
from #tmp3 t
OUTER APPLY(
	SELECT Val=COUNT(1) FROM (
		select  distinct Roll,Dyelot
		from Receiving_Detail rd
		where rd.Id = t.ReceivingID AND rd.PoId = t.POID AND rd.Seq1 = t.SEQ1 AND rd.Seq2 = t.SEQ2 
	)x
)ArriveRoll
OUTER APPLY(
	SELECT Val=COUNT(1) FROM (
		select  distinct Dyelot
		from Receiving_Detail rd
		where rd.Id = t.ReceivingID AND rd.PoId = t.POID AND rd.Seq1 = t.SEQ1 AND rd.Seq2 = t.SEQ2 
	)x
)HowManyDyelotArrived
OUTER APPLY(
	select Val=COUNT(distinct fp.Dyelot)
	from Receiving_Detail rd
	Left join FIR_Physical fp on t.ID=fp.ID and fp.Roll=rd.Roll and fp.Dyelot=rd.Dyelot
	where rd.Id = t.ReceivingID AND rd.PoId = t.POID AND rd.Seq1 = t.SEQ1 AND rd.Seq2 = t.SEQ2 
	AND fp.Result != '' AND fp.Result IS NOT NULL
)AlreadyInspecetedDyelot
OUTER APPLY(
	select Val = SUM(fp.TicketYds) / t.ArriveQty
	from Receiving_Detail rd
	Left join FIR_Physical fp on t.ID=fp.ID and fp.Roll=rd.Roll and fp.Dyelot=rd.Dyelot
	where rd.Id = t.ReceivingID AND rd.PoId = t.POID AND rd.Seq1 = t.SEQ1 AND rd.Seq2 = t.SEQ2 
	AND fp.Result != '' AND fp.Result IS NOT NULL
)InspectionPercentage
OUTER APPLY(
		select DefectRecord, point
		from #Sheet2 s
		WHERE s.ReceivingID=t.ReceivingID
		AND s.POID=t.POID
		AND s.SEQ=t.SEQ
)DefectCode

SELECT
	POID,
	SEQ,
	WK,
	ReceivingID,
	StyleID,
	BrandID,
	Supplier,
	Refno,
	ColorID,
	ArriveWHDate,
	ArriveQty,
	WeaveTypeID,
	SCIRefno,
	Name,
	ArriveRoll,
	NonPhysical = IIF(NonPhysical = 1,'Y',''),
	HowManyDyelotArrived,
	AlreadyInspecetedDyelot,
	InspectionPercentage,
	DefectCode,
	TotalPoints = SUM(TotalPoints),
	[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]),
    [BIInsertDate] = GETDATE()
FROM #DefectSummary
GROUP BY POID,SEQ,WK,ReceivingID,StyleID,BrandID,Supplier,Refno,ColorID,ArriveWHDate,ArriveQty,WeaveTypeID,
	SCIRefno,Name,ArriveRoll,NonPhysical,HowManyDyelotArrived,AlreadyInspecetedDyelot,InspectionPercentage,DefectCode
ORDER BY ReceivingID,WK,POID,SEQ,TotalPoints

drop table #tmp1,#tmp2,#tmp3,#Sheet2,#DefectSummary,#tmpR,#tmpT
");
            #endregion

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
