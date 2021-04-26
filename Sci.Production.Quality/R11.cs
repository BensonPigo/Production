using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R11 : Win.Tems.PrintForm
    {
        private readonly List<SqlParameter> parameters = new List<SqlParameter>();
        private readonly StringBuilder Sqlcmd = new StringBuilder();
        private string type;
        private DataTable[] PrintData;

        /// <inheritdoc/>
        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.dateArriveWHDate.Value1.Empty() && this.txtSP1.Text.Empty() && this.txtSP2.Text.Empty())
            {
                MyUtility.Msg.WarningBox("Arrive W/H Date and SP can't all empty!");
                return false;
            }

            this.Sqlcmd.Clear();
            this.parameters.Clear();

            string where1 = string.Empty;
            string where2 = string.Empty;
            this.type = this.radioPanelTransaction.Value;
            if (this.type == "1")
            {
                if (!this.dateArriveWHDate.Value1.Empty())
                {
                    where1 += $"and R.WhseArrival between @Date1 and @Date2" + Environment.NewLine;
                    where2 += $"and T.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@wDate1", this.dateArriveWHDate.Value1));
                    this.parameters.Add(new SqlParameter("@wDate2", this.dateArriveWHDate.Value2));
                    this.Sqlcmd.Append($@"
declare @Date1 date = @wDate1
declare @Date2 date = @wDate2
");
                }

                if (!this.txtSP1.Text.Empty())
                {
                    where1 += $"and F.POID between @SP1 and @SP2" + Environment.NewLine;
                    where2 += $"and F.POID between @SP1 and @SP2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                    this.parameters.Add(new SqlParameter("@SP2", this.txtSP2.Text));
                }

                if (!this.txtBrand.Text.Empty())
                {
                    where1 += $"and O.BrandID = @BrandID" + Environment.NewLine;
                    where2 += $"and O.BrandID = @BrandID" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@BrandID", this.txtBrand.Text));
                }

                if (!this.txtRefno1.Text.Empty())
                {
                    where1 += $"and PSD.Refno >= @Refno1" + Environment.NewLine;
                    where2 += $"and PSD.Refno >= @Refno1" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@Refno1", this.txtRefno1.Text));
                }

                if (!this.txtRefno2.Text.Empty())
                {
                    where1 += $"and PSD.Refno <= @Refno2" + Environment.NewLine;
                    where2 += $"and PSD.Refno <= @Refno2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@Refno2", this.txtRefno2.Text));
                }

                #region SQL

                // 基本資料
                this.Sqlcmd.Append($@"
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
	PSD.Refno,
	PSD.ColorID,
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
LEFT JOIN Color c ON psd.BrandId = c.BrandId AND PSD.ColorID = c.ID
Where 1=1
{where1}

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
	PSD.Refno,
	PSD.ColorID,
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
LEFT JOIN Color c ON psd.BrandId = c.BrandId AND PSD.ColorID = c.ID
Where 1=1
{where2}
");

                // 各sheet
                this.Sqlcmd.Append(@"
select
	t.*,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	TotalDefectyds = isnull((select SUM(Point) from FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	FP.DetailUkey,
	FP.ActualYds,
	Composition
into #tmp1
from #tmpR t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
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
	Composition
into #tmp2
from #tmpT t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
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
	Inspection = iif(SUM(TicketYds) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.ArriveQty)),
	t.Physical
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
	Inspection  = iif(SUM(TicketYds) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.Qty)),
	t.Physical
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
    point = isnull(Defect.point,  0),
	Defectrate = case when isnull(t.TicketYds, 0) = 0 then 0
		when t.BrandID = 'LLL' and ISNULL(t.width, 0) = 0 then 0
		when t.BrandID = 'LLL' then isnull(Defect.point, 0) * 3600 / (t.width * t.TicketYds)
		else isnull(Defect.point,  0) / t.TicketYds
		end
INTO #Sheet2
from #tmp1 t
outer apply(
    select
	    DefectRecord = dbo.SplitDefectNum(x.Data,0),	
        point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
    from FIR_Physical_Defect
    outer apply(select  * from SplitString(DefectRecord,'/'))x
    where FIR_PhysicalDetailUKey = t.DetailUkey
    group by dbo.SplitDefectNum(x.Data,0)
)Defect

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
    point = isnull(Defect.point,  0),
	Defectrate = case when isnull(t.TicketYds, 0) = 0 then 0
		when t.BrandID = 'LLL' and ISNULL(t.width, 0) = 0 then 0
		when t.BrandID = 'LLL' then isnull(Defect.point, 0) * 3600 / (t.width * t.TicketYds)
		else isnull(Defect.point,  0) / t.TicketYds 
		end
from #tmp2 t
outer apply(
    select 
	    DefectRecord = dbo.SplitDefectNum(x.Data,0),
        point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
    from FIR_Physical_Defect
    outer apply(select  * from SplitString(DefectRecord,'/'))x
    where FIR_PhysicalDetailUKey = t.DetailUkey
    group by dbo.SplitDefectNum(x.Data,0)
)Defect
left join FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

SELECT 
	POID,seq,WK,ReceivingID,StyleID,Brandid,Supplier,Refno,ColorID,
	ArriveWHDate,ArriveQty,WeaveTypeID,Dyelot,Width,Weight,Composition,
	Description,ConstructionID,Roll,InspDate,Result,Grade,DefectRecord,
	Type,DescriptionEN,point,Defectrate
FROM #Sheet2

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
	t.InspDate
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
	t.InspDate
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
	TotalPoints = SUM(TotalPoints)
FROM #DefectSummary
GROUP BY POID,SEQ,WK,ReceivingID,StyleID,BrandID,Supplier,Refno,ColorID,ArriveWHDate,ArriveQty,WeaveTypeID,
	SCIRefno,Name,ArriveRoll,NonPhysical,HowManyDyelotArrived,AlreadyInspecetedDyelot,InspectionPercentage,DefectCode
ORDER BY ReceivingID,WK,POID,SEQ,TotalPoints

drop table #tmp1,#tmp2,#tmp3,#Sheet2,#DefectSummary,#tmpR,#tmpT
");
                #endregion
            }
            else
            {
                if (!this.dateArriveWHDate.Value1.Empty())
                {
                    where1 += $"and st.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@wDate1", this.dateArriveWHDate.Value1));
                    this.parameters.Add(new SqlParameter("@wDate2", this.dateArriveWHDate.Value2));
                    this.Sqlcmd.Append($@"
declare @Date1 date = @wDate1
declare @Date2 date = @wDate2
");
                }

                if (!this.txtSP1.Text.Empty())
                {
                    where1 += $"and std.ToPOID between @SP1 and @SP2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                    this.parameters.Add(new SqlParameter("@SP2", this.txtSP2.Text));
                }

                if (!this.txtBrand.Text.Empty())
                {
                    where1 += $"and O.BrandID = @BrandID" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@BrandID", this.txtBrand.Text));
                }

                if (!this.txtRefno1.Text.Empty())
                {
                    where1 += $"and PSD.Refno >= @Refno1" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@Refno1", this.txtRefno1.Text));
                }

                if (!this.txtRefno2.Text.Empty())
                {
                    where1 += $"and PSD.Refno <= @Refno2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@Refno2", this.txtRefno2.Text));
                }

                #region SQL

                // 基本資料
                this.Sqlcmd.Append($@"
--Receiving
SELECT
	TransferID = st.Id,
	st.IssueDate,
	Inventory_SP = std.FromPOID,
	Inventory_SEQ = CONCAT(std.FromSeq1, '-' + std.FromSeq2),
	Bulk_SP = std.ToPOID,
	Bulk_SEQ = CONCAT(std.ToSeq1, '-' + std.ToSeq2),
	std.FromSeq1,
	std.FromSeq2,
	std.ToSeq1,
	std.ToSeq2,
	TransferQty = std.Qty,
	Description = dbo.getmtldesc(std.FromPOID, std.FromSeq1, std.FromSeq2, 2, 0),
	
    o.FactoryID,
	o.StyleID,
	o.BrandID,	
	Supplier = concat(PS.SuppID,'-'+ S.AbbEN),
	PSD.Refno,
	PSD.ColorID,
	PSD.SCIRefno,
	c.Name,
	F.ID,
	F.ReceivingID,
	F.Physical,
	f.NonPhysical,	
	Fabric.WeaveTypeID,
	Fabric.width,
	Weight = Fabric.WeightM2,
	Fabric.ConstructionID,

	[WK] = R.ExportId,
	Rate = (select RateValue from dbo.View_Unitrate v where v.FROM_U = RD.PoUnit and v.TO_U= RD.StockUnit),
	RD.StockQty,
	RD.ShipQty,
	RD.Dyelot,
	RD.Roll,
	Composition,
	std.Ukey
INTO #tmpR
from SubTransfer st
inner join SubTransfer_Detail std on std.id = st.id
inner join Orders O on O.ID = std.FromPOID
inner join PO_Supp_Detail PSD on PSD.ID = std.FromPOID and PSD.SEQ1 = std.FromSeq1 and PSD.SEQ2 = std.FromSeq2
inner join PO_Supp PS on PSD.ID = PS.ID and PSD.SEQ1 = PS.SEQ1
inner join Supp S on PS.SuppID = S.ID
inner join Fabric on PSD.SCIRefno = Fabric.SCIRefno
left join Receiving_Detail RD on RD.PoId = std.FromPOID and RD.Seq1 = std.FromSeq1 and RD.Seq2 = std.FromSeq2 and RD.Roll = std.FromRoll and RD.Dyelot = std. FromDyelot
left join Receiving R on R.Id = RD.Id 
left join FIR F on F.ReceivingID = R.ID and F.POID = RD.PoId and F.SEQ1 = RD.Seq1 and F.SEQ2 = RD.Seq2
left join Color c ON psd.BrandId = c.BrandId AND PSD.ColorID = c.ID
outer apply(
	select Composition = STUFF((
		select CONCAT('+', FLOOR(fc.percentage), '%', fc.MtltypeId)
		from Fabric_Content fc
		where fc.SCIRefno = PSD.SCIRefno
		for xml path('')
	),1,1,'')
)Composition
Where st.type = 'B' and st.Status = 'Confirmed' and PSD.FabricType = 'F'
{where1}

--TransferIn
SELECT
	TransferID = st.Id,
	st.IssueDate,
	Inventory_SP = std.FromPOID,
	Inventory_SEQ = CONCAT(std.FromSeq1, '-' + std.FromSeq2),
	Bulk_SP = std.ToPOID,
	Bulk_SEQ = CONCAT(std.ToSeq1, '-' + std.ToSeq2),
	std.FromSeq1,
	std.FromSeq2,
	std.ToSeq1,
	std.ToSeq2,
	TransferQty = std.Qty,
	Description = dbo.getmtldesc(std.FromPOID, std.FromSeq1, std.FromSeq2, 2, 0),
	
    o.FactoryID,
	o.StyleID,
	o.BrandID,	
	Supplier = concat(PS.SuppID,'-'+ S.AbbEN),
	PSD.Refno,
	PSD.ColorID,
	PSD.SCIRefno,
	c.Name,
	F.ID,
	F.ReceivingID,
	F.Physical,
	f.NonPhysical,	
	Fabric.WeaveTypeID,
	Fabric.width,
	Weight = Fabric.WeightM2,
	Fabric.ConstructionID,
	
	[WK] = '',
	RD.Qty,
	RD.Dyelot,
	RD.Roll,
	Composition
INTO #tmpT
from SubTransfer st
inner join SubTransfer_Detail std on std.id = st.id
inner join Orders O on O.ID = std.FromPOID
inner join PO_Supp_Detail PSD on PSD.ID = std.FromPOID and PSD.SEQ1 = std.FromSeq1 and PSD.SEQ2 = std.FromSeq2
inner join PO_Supp PS on PSD.ID = PS.ID and PSD.SEQ1 = PS.SEQ1
inner join Supp S on PS.SuppID = S.ID
inner join Fabric on PSD.SCIRefno = Fabric.SCIRefno
left join TransferIn_Detail RD on RD.PoId = std.FromPOID and RD.Seq1 = std.FromSeq1 and RD.Seq2 = std.FromSeq2 and RD.Roll = std.FromRoll and RD.Dyelot = std. FromDyelot
left join TransferIn R on R.Id = RD.Id 
left join FIR F on F.ReceivingID = R.ID and F.POID = RD.PoId and F.SEQ1 = RD.Seq1 and F.SEQ2 = RD.Seq2
left join Color c ON psd.BrandId = c.BrandId AND PSD.ColorID = c.ID
outer apply(
	select Composition = STUFF((
		select CONCAT('+', FLOOR(fc.percentage), '%', fc.MtltypeId)
		from Fabric_Content fc
		where fc.SCIRefno = PSD.SCIRefno
		for xml path('')
	),1,1,'')
)Composition
Where st.type = 'B' and st.Status = 'Confirmed' and PSD.FabricType = 'F'
and not exists(select 1 from #tmpR r where r.Ukey = std.Ukey)
{where1}
");

                // 各sheet
                this.Sqlcmd.Append(@"
select
	t.*,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	TotalDefectyds = isnull((select SUM(Point) from FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	FP.DetailUkey,
	FP.ActualYds
into #tmp1
from #tmpR t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot

select
	t.*,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	TotalDefectyds = isnull((select SUM(Point) from FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	FP.DetailUkey,
	FP.ActualYds
into #tmp2
from #tmpT t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot

--Summary分頁
select
	t.Bulk_SP,
    t.Bulk_SEQ,
	t.TransferID,
	t.Inventory_SP,
	t.Inventory_SEQ,
	t.WK,
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.IssueDate,
	TransferQty = SUM(t.TransferQty),
	t.WeaveTypeID,
	t.Dyelot,
	t.Width,
	t.Weight,
	t.Composition,
	t.Description,
	t.ConstructionID,
	ShippedQty = SUM(t.ShipQty * isnull(t.Rate,1)),
    t.FactoryID,
	RFT = iif(SUM(t.StockQty) = 0, 0, isnull(SUM(TotalDefectyds), 0) / SUM(t.StockQty)),
	TotalDefectyds = Sum(TotalDefectyds),
	Inspection = iif(SUM(StockQty) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.StockQty)),
	t.Physical
from #tmp1 t
Group by Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,WK,ReceivingID,StyleID,Brandid,Supplier,Refno,ColorID,IssueDate,
	WeaveTypeID,Dyelot,Width,Weight,Composition,Description,ConstructionID,FactoryID,Physical

union all
select
	t.Bulk_SP,
    t.Bulk_SEQ,
	t.TransferID,
	t.Inventory_SP,
	t.Inventory_SEQ,
    WK,
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.IssueDate,
	TransferQty = SUM(t.TransferQty),
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
	Inspection  = iif(SUM(Qty) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.Qty)),
	t.Physical
from #tmp2 t
Group by Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,WK,ReceivingID,StyleID,Brandid,Supplier,Refno,ColorID,IssueDate,
	WeaveTypeID,Dyelot,Width,Weight,Composition,Description,ConstructionID,FactoryID,Physical

--Defect_detail 分頁
select
	t.Bulk_SP,
    t.Bulk_SEQ,
	t.TransferID,
	t.Inventory_SP,
	t.Inventory_SEQ,
	WK,
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.IssueDate,
	t.TransferQty,
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
	Defectrate = case when isnull(t.TicketYds, 0) = 0 then 0
		when t.BrandID = 'LLL' and ISNULL(t.width, 0) = 0 then 0
		when t.BrandID = 'LLL' then isnull(Defect.point, 0) * 3600 / (t.width * t.TicketYds)
		else isnull(Defect.point,  0) / t.TicketYds
		end
INTO #Sheet2
from #tmp1 t
outer apply(
    select
	    DefectRecord = dbo.SplitDefectNum(x.Data,0),	
        point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
    from FIR_Physical_Defect
    outer apply(select  * from SplitString(DefectRecord,'/'))x
    where FIR_PhysicalDetailUKey = t.DetailUkey
    group by dbo.SplitDefectNum(x.Data,0)
)Defect

left join FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

union all
select
	t.Bulk_SP,
    t.Bulk_SEQ,
	t.TransferID,
	t.Inventory_SP,
	t.Inventory_SEQ,
	WK = '',
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.IssueDate,
	t.TransferQty,
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
	Defectrate = case when isnull(t.TicketYds, 0) = 0 then 0
		when t.BrandID = 'LLL' and ISNULL(t.width, 0) = 0 then 0
		when t.BrandID = 'LLL' then isnull(Defect.point, 0) * 3600 / (t.width * t.TicketYds)
		else isnull(Defect.point,  0) / t.TicketYds 
		end
from #tmp2 t
outer apply(
    select 
	    DefectRecord = dbo.SplitDefectNum(x.Data,0),
        point = sum(cast(dbo.SplitDefectNum(x.Data,1) as int))
    from FIR_Physical_Defect
    outer apply(select  * from SplitString(DefectRecord,'/'))x
    where FIR_PhysicalDetailUKey = t.DetailUkey
    group by dbo.SplitDefectNum(x.Data,0)
)Defect
left join FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

SELECT 
	Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,
	Inventory_SEQ,WK,ReceivingID,StyleID,Brandid,Supplier,Refno,ColorID,	
	IssueDate,	TransferQty,WeaveTypeID,Dyelot,Width,Weight,Composition,
	Description,ConstructionID,Roll,InspDate,Result,Grade,DefectRecord,
	Type,DescriptionEN,point,Defectrate
FROM #Sheet2

--Lacking yard分頁
select
	t.Bulk_SP,
    t.Bulk_SEQ,
	t.TransferID,
	t.Inventory_SP,
	t.Inventory_SEQ,
	WK,
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.IssueDate,
	t.TransferQty,
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
	t.InspDate
from #tmp1 t
where t.InspDate is not null

union all
select
	t.Bulk_SP,
    t.Bulk_SEQ,
	t.TransferID,
	t.Inventory_SP,
	t.Inventory_SEQ,
	WK = '',
	t.ReceivingID,
	t.StyleID,
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.IssueDate,
	t.TransferQty,
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
	t.InspDate
from #tmp2 t
where t.InspDate is not null

----Defect Summary Report 分頁
SELECT ID,ReceivingID,WK,Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,FromSeq1,FromSeq2,IssueDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID,TransferQty = sum(TransferQty),StockQty=sum(StockQty)
INTO #tmp3
from #tmpR
group by ID,ReceivingID,WK,Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,FromSeq1,FromSeq2,IssueDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID

union all
SELECT ID,ReceivingID,WK,Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,FromSeq1,FromSeq2,IssueDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID,TransferQty = sum(TransferQty),StockQty=sum(Qty)
from #tmpT
group by ID,ReceivingID,WK,Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,FromSeq1,FromSeq2,IssueDate,Refno,SCIRefno,ColorID,Name,Supplier,WeaveTypeID,NonPhysical,StyleID,BrandID

Select
	Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,
	WK,
	ReceivingID,
	StyleID,
	BrandID,
	Supplier,
	Refno,
	ColorID,
	IssueDate,
	TransferQty,
	WeaveTypeID,
	SCIRefno,
	Name,
	[ctRoll]=ArriveRoll.Val,
	NonPhysical,
	[HowManyDyelot] = HowManyDyelotArrived.Val,
	[AlreadyInspecetedDyelot] = AlreadyInspecetedDyelot.Val,
	[InspectionPercentage] = InspectionPercentage.Val,
	[DefectCode] = DefectCode.DefectRecord,
	[TotalPoints] = DefectCode.point
INTO #DefectSummary
from #tmp3 t
OUTER APPLY(
	SELECT Val=COUNT(1) FROM (
		select  distinct std.FromRoll,std.FromDyelot
		from SubTransfer_Detail std
		where std.id =t.TransferID and std.FromPOID = t.Inventory_SP and std.FromSeq1 = t.FromSeq1 and std.FromSeq2 =  t.FromSeq2
	)x
)ArriveRoll
OUTER APPLY(
	SELECT Val=COUNT(1) FROM (
		select distinct std.FromDyelot
		from SubTransfer_Detail std
		where std.id =t.TransferID and std.FromPOID = t.Inventory_SP and std.FromSeq1 = t.FromSeq1 and std.FromSeq2 =  t.FromSeq2
	)x
)HowManyDyelotArrived
OUTER APPLY(
	select Val=COUNT(distinct fp.Dyelot)
	from SubTransfer_Detail std
	left join Receiving_Detail rd on rd.PoId = std.FromPOID and rd.Seq1 = std.FromSeq1 and rd.Seq2 = std.FromSeq2 and rd.Roll = std.FromRoll and rd.Dyelot = std. FromDyelot
	Left join FIR_Physical fp on t.ID=fp.ID and fp.Roll=rd.Roll and fp.Dyelot=rd.Dyelot
	where std.id =t.TransferID and std.FromPOID = t.Inventory_SP and std.FromSeq1 = t.FromSeq1 and std.FromSeq2 =  t.FromSeq2
	AND fp.Result <> ''
)AlreadyInspecetedDyelot
OUTER APPLY(
	select Val = iif(t.StockQty = 0, 0, SUM(fp.TicketYds) / t.StockQty)
	from SubTransfer_Detail std
	left join Receiving_Detail rd on rd.PoId = std.FromPOID and rd.Seq1 = std.FromSeq1 and rd.Seq2 = std.FromSeq2 and rd.Roll = std.FromRoll and rd.Dyelot = std. FromDyelot
	Left join FIR_Physical fp on t.ID=fp.ID and fp.Roll=rd.Roll and fp.Dyelot=rd.Dyelot
	where std.id =t.TransferID and std.FromPOID = t.Inventory_SP and std.FromSeq1 = t.FromSeq1 and std.FromSeq2 =  t.FromSeq2
	and rd.Id = t.ReceivingID
	AND fp.Result <> ''
)InspectionPercentage
OUTER APPLY(
		select DefectRecord, point
		from #Sheet2 s
		WHERE s.ReceivingID=t.ReceivingID
		AND s.Inventory_SP = t.Inventory_SP
		AND s.Inventory_SEQ = t.Inventory_SEQ
)DefectCode

SELECT
	Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,
	WK,
	ReceivingID,
	StyleID,
	BrandID,
	Supplier,
	Refno,
	ColorID,
	IssueDate,
	TransferQty,
	WeaveTypeID,
	SCIRefno,
	Name,
	ctRoll,
	NonPhysical = IIF(NonPhysical = 1,'Y',''),
	[HowManyDyelot],
	AlreadyInspecetedDyelot,
	InspectionPercentage,
	DefectCode,
	TotalPoints = SUM(TotalPoints)
FROM #DefectSummary
GROUP BY Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,Inventory_SEQ,WK,ReceivingID,StyleID,BrandID,Supplier,Refno,
	ColorID,IssueDate,TransferQty,WeaveTypeID,SCIRefno,Name,ctRoll,NonPhysical,[HowManyDyelot],AlreadyInspecetedDyelot,
	InspectionPercentage,DefectCode
ORDER BY ReceivingID,WK,Bulk_SP,Bulk_SEQ,TotalPoints

drop table #tmp1,#tmp2,#tmp3,#Sheet2,#DefectSummary,#tmpR,#tmpT

");
                #endregion
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.Sqlcmd.ToString(), this.parameters, out this.PrintData);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.PrintData[0].Rows.Count);

            if (this.PrintData[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string excelName = "Quality_R11";
            if (this.type == "2")
            {
                excelName = "Quality_R11_B2A";
            }

            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            MyUtility.Excel.CopyToXls(this.PrintData[0], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            if (this.PrintData[1].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.PrintData[1], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[2]);
            }

            if (this.PrintData[2].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.PrintData[2], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[3]);
            }

            if (this.PrintData[3].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.PrintData[3], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[4]);
            }

            excelApp.Visible = true;

            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            Marshal.FinalReleaseComObject(excelApp);

            return true;
        }

        private void RadioPanelTransaction_ValueChanged(object sender, EventArgs e)
        {
            switch (this.radioPanelTransaction.Value)
            {
                case "1":
                    this.lbDate.Text = "Arrive W/H Date";
                    this.lbSP.Text = "SP#";
                    break;
                case "2":
                    this.lbDate.Text = "Issue Date";
                    this.lbSP.Text = "Bulk SP#";
                    break;
            }
        }
    }
}
