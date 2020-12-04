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

            this.Sqlcmd.Append($@"

select
	Supplier = concat(PS.SuppID,'-'+ S.AbbEN),
	PSD.Refno,
	PSD.ColorID,
	Fabric.WeaveTypeID,
	Composition.Composition,
	Fabric.width,
	Weight = Fabric.WeightM2,
	Fabric.ConstructionID,
	Description = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	RD.ShipQty,
	Rate = (select RateValue from dbo.View_Unitrate v where v.FROM_U = RD.PoUnit and v.TO_U= RD.StockUnit),
	RD.StockQty,
	F.POID,
	R.ExportId,
	RD.Dyelot,
	F.Physical,
	SEQ = CONCAT(F.SEQ1, '-' + F.SEQ2),
	FP.Roll,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	O.BrandID,
	TotalDefectyds = isnull((select SUM(Point) from FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	RD.ID,
	FP.DetailUkey,
	FP.ActualYds
into #tmp1
from FIR F
Inner join Receiving R on F.ReceivingID=R.Id
Inner join Receiving_Detail RD on R.Id=RD.Id and F.POID=RD.PoId and F.SEQ1=RD.Seq1 and F.SEQ2=RD.Seq2
Inner join PO_Supp_Detail PSD on PSD.ID=RD.PoId and PSD.SEQ1=RD.Seq1 and PSD.SEQ2=RD.Seq2
Inner join PO_Supp PS on PSD.ID=PS.ID and PSD.SEQ1=PS.SEQ1
Inner join Supp S on PS.SuppID=S.ID
Inner join Fabric on PSD.SCIRefno=Fabric.SCIRefno
Inner join Orders O on F.POID=O.ID
Left join FIR_Physical FP on F.ID=FP.ID and FP.Roll=RD.Roll and FP.Dyelot=RD.Dyelot
outer apply(
	select Composition = STUFF((
		select CONCAT('+', FLOOR(fc.percentage), '%', fc.MtltypeId)
		from Fabric_Content fc
		where fc.SCIRefno = Fabric.SCIRefno
		for xml path('')
	),1,1,'')
)Composition

Where 1=1
{where1}

select
	Supplier = concat(PS.SuppID,'-'+ S.AbbEN),
	PSD.Refno,
	PSD.ColorID,
	Fabric.WeaveTypeID,
	Composition.Composition,
	Fabric.width,
	Weight = Fabric.WeightM2,
	Fabric.ConstructionID,
	Description = dbo.getmtldesc(F.POID, F.SEQ1, f.SEQ2, 2, 0),
	TD.Qty,
	F.POID,
	TD.Dyelot,
	F.Physical,
	SEQ = CONCAT(F.SEQ1, '-' + F.SEQ2),
	FP.Roll,
	FP.InspDate,
	FP.Result,
	FP.Grade,
	O.BrandID,
	TotalDefectyds = isnull((select SUM(Point) from FIR_Physical_Defect fpd where FPD.FIR_PhysicalDetailUKey = FP.DetailUkey), 0) * 0.25,
	FP.TicketYds,
	TD.ID,
	FP.DetailUkey,
	FP.ActualYds
into #tmp2
from FIR F
Inner join TransferIn T on F.ReceivingID=T.Id
Inner join TransferIn_Detail TD on T.Id=TD.Id and F.POID=TD.PoId and F.SEQ1=TD.Seq1 and F.SEQ2=TD.Seq2
Inner join PO_Supp_Detail PSD on PSD.ID=TD.PoId and PSD.SEQ1=TD.Seq1 and PSD.SEQ2=TD.Seq2
Inner join PO_Supp PS on PSD.ID=PS.ID and PSD.SEQ1=PS.SEQ1
Inner join Supp S on PS.SuppID=S.ID
Inner join Fabric on PSD.SCIRefno=Fabric.SCIRefno
Inner join Orders O on F.POID=O.ID
Left join FIR_Physical FP on F.ID=FP.ID and FP.Roll=TD.Roll and FP.Dyelot=TD.Dyelot
outer apply(
	select Composition = STUFF((
		select CONCAT('+', FLOOR(fc.percentage), '%', fc.MtltypeId)
		from Fabric_Content fc
		where fc.SCIRefno = Fabric.SCIRefno
		for xml path('')
	),1,1,'')
)Composition
Where 1=1
{where2}

--Summary分頁
select
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.WeaveTypeID,
	t.Composition,
	t.Width,
	t.Weight,
	t.ConstructionID,
	t.Description,
	ShippedQty = SUM(t.ShipQty * isnull(t.Rate,1)),
	ArrivedQty = SUM(t.StockQty),
	t.POID,
	WK = t.ExportId,
    t.SEQ,
	t.Dyelot,
	TotalDefectyds = Sum(TotalDefectyds),
	RFT = iif(SUM(t.StockQty) = 0, 0, isnull(SUM(TotalDefectyds), 0) / SUM(t.StockQty)),
	Inspection  = iif(SUM(TicketYds) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.StockQty)),
	t.Physical
from #tmp1 t
Group by t.Brandid,t.Supplier,t.Refno,t.ColorID,t.WeaveTypeID,t.Composition,t.Width,t.Weight,
t.ConstructionID,t.Description,t.POID,t.ExportId,t.SEQ,t.Dyelot,t.Physical,ID

union all
select
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.WeaveTypeID,
	t.Composition,
	t.Width,
	t.Weight,
	t.ConstructionID,
	t.Description,
	ShippedQty = SUM(t.Qty),
	ArrivedQty = SUM(t.Qty),
	t.POID,
    t.SEQ,
	WK = '',
	t.Dyelot,
	TotalDefectyds = Sum(TotalDefectyds),
	RFT = iif(SUM(t.Qty) = 0, 0, isnull(SUM(TotalDefectyds), 0) / SUM(t.Qty)),
	Inspection  = iif(SUM(TicketYds) = 0, 0, isnull(SUM(TicketYds), 0) / SUM(t.Qty)),
	t.Physical
from #tmp2 t
Group by t.Brandid,t.Supplier,t.Refno,t.ColorID,t.WeaveTypeID,t.Composition,t.Width,t.Weight,
t.ConstructionID,t.Description,t.POID,t.SEQ,t.Dyelot,t.Physical,ID

--Defect_detail 分頁
select
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.WeaveTypeID,
	t.Composition,
	t.Width,
	Weight = t.Weight,
	t.ConstructionID,
	t.Description,
	t.POID,
	t.seq,
	WK = t.ExportId,
	t.Dyelot,
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
from #tmp1 t
outer apply(
	select DefectRecord = SUBSTRING(x.Data,1,1), point = sum(cast(SUBSTRING(x.Data,2,1) as int))
	from FIR_Physical_Defect
	outer apply(select  * from SplitString(DefectRecord,'/'))x
	where FIR_PhysicalDetailUKey = t.DetailUkey
	group by SUBSTRING(x.Data,1,1)
)Defect

left join FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

union all
select
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.WeaveTypeID,
	t.Composition,
	t.Width,
	Weight = t.Weight,
	t.ConstructionID,
	t.Description,
	t.POID,
	t.seq,
	WK = '',
	t.Dyelot,
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
	select DefectRecord = SUBSTRING(x.Data,1,1), point = sum(cast(SUBSTRING(x.Data,2,1) as int))
	from FIR_Physical_Defect
	outer apply(select  * from SplitString(DefectRecord,'/'))x
	where FIR_PhysicalDetailUKey = t.DetailUkey
	group by SUBSTRING(x.Data,1,1)
)Defect
left join FabricDefect fd on fd.ID = Defect.DefectRecord
where Defect.DefectRecord is not null or fd.Type is not null

--Lacking yard分頁
select
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.WeaveTypeID,
	t.Composition,
	t.Width,
	Weight = t.Weight,
	ShippedQty = t.ShipQty * isnull(t.Rate,1),
	ArrivedQty = t.StockQty,
	t.POID,
	WK = t.ExportId,
	t.Dyelot,
	t.Roll,
	t.TicketYds,
	t.ActualYds,
	isnull(t.TicketYds, 0) - isnull(t.ActualYds, 0),
	t.InspDate
from #tmp1 t
where t.InspDate is not null


union all
select
    t.Brandid,
	t.Supplier,
	t.Refno,
	t.ColorID,
	t.WeaveTypeID,
	t.Composition,
	t.Width,
	Weight = t.Weight,
	ShippedQty = t.Qty,
	ArrivedQty = t.qty,
	t.POID,
	WK = '',
	t.Dyelot,
	t.Roll,
	t.TicketYds,
	t.ActualYds,
	isnull(t.TicketYds, 0) - isnull(t.ActualYds, 0),
	t.InspDate
from #tmp2 t
where t.InspDate is not null


drop table #tmp1,#tmp2
");
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
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            MyUtility.Excel.CopyToXls(this.PrintData[0], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            if (this.PrintData[1].Rows.Count > 1)
            {
                MyUtility.Excel.CopyToXls(this.PrintData[1], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[2]);
                MyUtility.Excel.CopyToXls(this.PrintData[2], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[3]);
            }

            excelApp.Visible = true;

            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            Marshal.FinalReleaseComObject(excelApp);

            return true;
        }
    }
}
