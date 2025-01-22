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
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg;

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
            if (this.type == "2")
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
	PS.SuppID,
	PSD.Refno,
	ColorID = isnull(psdsC.SpecValue ,''),
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
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Color c ON psd.BrandId = c.BrandId AND isnull(psdsC.SpecValue ,'') = c.ID
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
	PS.SuppID,
	PSD.Refno,
	ColorID = isnull(psdsC.SpecValue ,''),
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
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Color c ON psd.BrandId = c.BrandId AND isnull(psdsC.SpecValue ,'') = c.ID
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
	FP.ActualYds,
	FP.ActualWidth,
	Inspector = Concat (Fp.Inspector, ' ', p.Name) 
into #tmp1
from #tmpR t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
Left JOIN Pass1 p ON p.ID = Fp.Inspector

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
	Inspector = Concat (Fp.Inspector, ' ', p.Name)  
into #tmp2
from #tmpT t
Left join FIR_Physical FP on FP.ID = t.ID and FP.Roll = t.Roll and FP.Dyelot = t.Dyelot
Left JOIN Pass1 p ON p.ID = Fp.Inspector

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
	Inspection = iif(SUM(t.StockQty) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.StockQty)),
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
	Inspection  = iif(SUM(t.Qty) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.Qty)),
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
	Bulk_SP,Bulk_SEQ,TransferID,Inventory_SP,
	Inventory_SEQ,WK,ReceivingID,StyleID,Brandid,Supplier,Refno,ColorID,	
	IssueDate,	TransferQty,WeaveTypeID,Dyelot,Width,Weight,Composition,
	Description,ConstructionID,Roll,InspDate,Result,Grade,DefectRecord,
	Type,DescriptionEN,T2Points,FactoryPoints,point,Defectrate,Inspector
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
	[InspDate] = FORMAT(t.InspDate, 'yyyy/MM/dd')
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
	[InspDate] = FORMAT(t.InspDate, 'yyyy/MM/dd')
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
            if (this.type == "1")
            {
                // 資料邏輯跟BI共用
                QA_R11 biModel = new QA_R11();
                QA_R11_ViewModel qa_R11_Model = new QA_R11_ViewModel()
                {
                    ArriveWHDate1 = this.dateArriveWHDate.Value1,
                    ArriveWHDate2 = this.dateArriveWHDate.Value2,
                    SP1 = this.txtSP1.Text,
                    SP2 = this.txtSP2.Text,
                    Brand = this.txtBrand.Text,
                    Refno1 = this.txtRefno1.Text,
                    Refno2 = this.txtRefno2.Text,
                    IsPowerBI = false,
                };

                Base_ViewModel resultReport = biModel.GetQA_R11Data(qa_R11_Model);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.PrintData = resultReport.DtArr;
                return Ict.Result.True;
            }
            else
            {
                return DBProxy.Current.Select(null, this.Sqlcmd.ToString(), this.parameters, out this.PrintData);
            }
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
