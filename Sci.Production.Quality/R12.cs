using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R12 : Win.Tems.PrintForm
    {
        private readonly List<SqlParameter> parameters = new List<SqlParameter>();
        private string Sqlcmd;

        private DataTable[] PrintData;

        private string baseReceivingSql = @"
select	f.POID,
		[Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2),
		a.ExportId,
		f.ReceivingID,
		o.StyleID,
		o.BrandID,
		f.Suppid,
		psd.Refno,
		psd.ColorID,
		[ArriveWH_Date] = a.WhseArrival,
        f.ArriveQty,
        fa.WeaveTypeID,
        ct.Roll,
        ct2.Dyelot,
        {0}
from Fir f with (nolock)
inner join Receiving a with (nolock) on a.Id = f.ReceivingID
left join Orders o with (nolock) on o.ID = f.POID
left join PO_Supp_Detail psd with (nolock) on psd.ID = f.POID and psd.SEQ1 = f.SEQ1 and psd.SEQ2 = f.SEQ2
left join Fabric fa with (nolock) on fa.SCIRefno = psd.SCIRefno
outer apply(
    select Roll = count(1)
    from(
        select distinct b.Roll, b.Dyelot
        from Receiving_Detail b with (nolock)
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ct
outer apply(
    select Dyelot = count(1)
    from(
        select distinct b.Dyelot
        from Receiving_Detail b with (nolock)
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ct2
{1}
{3}
where 1 = 1
{2}
";

        private string baseTransferInSql = @"
select	f.POID,
		[Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2),
		[ExportId] = '',
		f.ReceivingID,
		o.StyleID,
		o.BrandID,
		f.Suppid,
		psd.Refno,
		psd.ColorID,
		[ArriveWH_Date] = a.IssueDate,
        f.ArriveQty,
        fa.WeaveTypeID,
        ct.Roll,
        ct2.Dyelot,
        {0}
from Fir f with (nolock)
inner join TransferIn a with (nolock) on a.Id = f.ReceivingID
left join Orders o with (nolock) on o.ID = f.POID
left join PO_Supp_Detail psd with (nolock) on psd.ID = f.POID and psd.SEQ1 = f.SEQ1 and psd.SEQ2 = f.SEQ2
left join Fabric fa with (nolock) on fa.SCIRefno = psd.SCIRefno
outer apply(
    select Roll = count(1)
    from(
        select distinct b.Roll, b.Dyelot
        from TransferIn_Detail b with (nolock)
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ct
outer apply(
    select Dyelot = count(1)
    from(
        select distinct b.Dyelot
        from TransferIn_Detail b with (nolock)
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ct2
{1}
{3}
where 1 = 1
{2}
";

        private string B2A_sqlcmd = @"
select
	TransferID = st.Id,
	st.IssueDate,
	std.FromPOID,
	FromSeq = CONCAT(std.FromSeq1, '-' + std.FromSeq2),
	std.ToPOID,
	ToSeq = CONCAT(std.ToSeq1, '-' + std.ToSeq2),
	std.FromSeq1,
	std.FromSeq2,
	std.ToSeq1,
	std.ToSeq2,
	std.FromRoll,
	std.FromDyelot,
	TransferQty = std.Qty,	
	fa.WeaveTypeID,	
	fa.Width,
	WK = {4},
	f.ReceivingID,
	f.Suppid,
	Approve = Concat (f.Approve, '-', p2.Name),
	f.ApproveDate,
	f.TotalInspYds,
	FirID = f.ID,

	F.NonPhysical,
	f.Physical,
	f.PhysicalInspector,
	f.PhysicalDate,
		
	F.nonWeight,
	f.Weight,
	f.WeightInspector,
	f.WeightDate,
	
	F.nonShadebond,
	f.ShadeBond,
	ShadeBondInspector = f.ShadeboneInspector,
	f.ShadeBondDate,
	
	F.nonContinuity,
	f.Continuity,
	f.ContinuityInspector,
	f.ContinuityDate,
	
	F.nonOdor,
	f.Odor,
	f.OdorInspector,
	f.OdorDate,

	o.StyleID,
	o.BrandID,
	psd.Refno,
	psd.ColorID,
	ctRoll = ct.Roll,
	ctDyelot = ct2.Dyelot,
	std.Ukey
into #tmp{2}
from SubTransfer st
inner join SubTransfer_Detail std on std.id = st.id
inner join Orders O on O.ID = std.FromPOID
inner join PO_Supp_Detail PSD on PSD.ID = std.FromPOID and PSD.SEQ1 = std.FromSeq1 and PSD.SEQ2 = std.FromSeq2
inner join PO_Supp PS on PSD.ID = PS.ID and PSD.SEQ1 = PS.SEQ1
inner join Supp S on PS.SuppID = S.ID
inner join Fabric fa on PSD.SCIRefno = fa.SCIRefno
{5} join {0} b on b.PoId = std.FromPOID and b.Seq1 = std.FromSeq1 and b.Seq2 = std.FromSeq2 and b.Roll = std.FromRoll and b.Dyelot = std. FromDyelot
left join {1} a on a.Id = b.Id 
left join FIR F on F.ReceivingID = a.ID and F.POID = b.PoId and F.SEQ1 = b.Seq1 and F.SEQ2 = b.Seq2
left join pass1 p2 with (nolock) on p2.id = f.Approve
outer apply(
    select Roll = count(1)
    from(
        select distinct b.Roll, b.Dyelot
        from {0} b with (nolock)
        where b.id = a.id and b.PoId = std.ToPOID and b.Seq1 = std.ToSeq1 and b.Seq2 = std.ToSeq2 and b.Roll = std.FromRoll and b.Dyelot = std. FromDyelot
    )x
)ct
outer apply(
    select Dyelot = count(1)
    from(
        select distinct b.Dyelot
        from {0} b with (nolock)
        where b.id = a.id and b.PoId = std.ToPOID and b.Seq1 = std.ToSeq1 and b.Seq2 = std.ToSeq2 and b.Roll = std.FromRoll and b.Dyelot = std. FromDyelot
    )x
)ct2
Where st.type = 'B' and st.Status = 'Confirmed' and PSD.FabricType = 'F'
{3}
";

        private string B2A_select = @"
select
	f.ToPOID,
	f.ToSeq,
	f.TransferID,
	f.FromPOID,
	f.FromSeq,
	f.WK,
	f.ReceivingID,
	f.StyleID,
	f.BrandID,
	f.Suppid,
	f.Refno,
	f.ColorID,
	f.IssueDate,
	TransferQty = sum(f.TransferQty),
	f.WeaveTypeID,
	f.ctRoll,
	f.ctDyelot,

	{3}

	Non{1} = IIF(f.Non{1} = 1,'Y','') ,
	f.{1},

	{4}

	{1}Inspector = Concat (f.{1}Inspector, '-', p1.Name),

	{5}f.{1}Date,

	f.Approve,
	f.ApproveDate

    {6}
from #tmp{0} f
left join pass1 p1 with (nolock) on p1.id = f.{1}Inspector
{2}
where 1=1
{8}
group by f.ToPOID,f.ToSeq,f.TransferID,f.FromPOID,f.FromSeq,f.WK,f.ReceivingID,f.StyleID,f.BrandID,f.Suppid,
	f.Refno,f.ColorID,f.IssueDate,f.WeaveTypeID,f.ctRoll,f.ctDyelot,f.Approve,f.ApproveDate,p1.Name,
	f.Non{1},f.{1},f.{1}Inspector,f.{1}Date
	{7}
";

        /// <inheritdoc/>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboInspection.SelectedIndex = 0;
            this.comboInspectionResult.SelectedIndex = 0;
        }

        private string AddInspectionWhere(string srcWhere, string inspectionType)
        {
            string returnResult = srcWhere;

            if (this.dateInspectionDate.HasValue)
            {
                if (this.radioWKSeq.Checked)
                {
                    switch (inspectionType)
                    {
                        case "Physical":
                            returnResult += $"and f.PhysicalDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                            break;
                        case "Weight":
                            returnResult += $"and f.WeightDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                            break;
                        case "ShadeBond":
                            returnResult += $"and f.ShadeBondDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                            break;
                        case "Continuity":
                            returnResult += $"and f.ContinuityDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                            break;
                        case "Odor":
                            returnResult += $"and f.OdorDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    returnResult += $"and i.InspDate between @InsDate1 and @InsDate2" + Environment.NewLine;
                }
            }

            switch (this.comboInspectionResult.Text)
            {
                case "Pass":
                    returnResult += $"and f.{inspectionType} = 'Pass'" + Environment.NewLine;
                    break;
                case "Fail":
                    returnResult += $"and f.{inspectionType} = 'Fail'" + Environment.NewLine;
                    break;
                case "Pass/Fail":
                    returnResult += $"and f.{inspectionType} in ('Pass', 'Fail')" + Environment.NewLine;
                    break;
                case "Not yet inspected":
                    returnResult += $"and f.Non{inspectionType} = 0  and (f.{inspectionType} = '' or f.{inspectionType} is null)" + Environment.NewLine;
                    break;
                default:
                    break;
            }

            return returnResult;
        }

        private string AddJoinByReportType(string fromType, string inspectionTypeTable, string inspectionType = "")
        {
            string joinString = string.Empty;
            if (inspectionType == "Physical")
            {
                if (fromType == "TransferIn")
                {
                    joinString = $@"
outer apply(
    select Dyelot = count(1)
    from(
        select distinct b.Dyelot
        from TransferIn_Detail b with (nolock)
		inner join FIR_Physical i with (nolock) on i.ID = f.ID and i.Roll = b.Roll and i.Dyelot = b.Dyelot and i.Result <> ''
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ct3
";
                }
                else
                {
                    joinString = $@"
outer apply(
    select Dyelot = count(1)
    from(
        select distinct b.Dyelot
        from Receiving_Detail b with (nolock)
		inner join FIR_Physical i with (nolock) on i.ID = f.ID and i.Roll = b.Roll and i.Dyelot = b.Dyelot and i.Result <> ''
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ct3
";
                }
            }

            if (this.radioWKSeq.Checked)
            {
                return joinString;
            }

            if (fromType == "TransferIn")
            {
                joinString += $@"
inner join TransferIn_Detail b with (nolock) on a.id = b.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
left join {inspectionTypeTable} i with (nolock) on i.ID = f.ID and i.Roll = b.Roll and i.Dyelot = b.Dyelot";
            }
            else
            {
                joinString += $@"
inner join Receiving_Detail b with (nolock) on b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
left join {inspectionTypeTable} i with (nolock) on i.ID = f.ID and i.Roll = b.Roll and i.Dyelot = b.Dyelot";
            }

            return joinString;
        }

        private string B2A_Select(string inspectionType)
        {
            string columnSource = string.Empty;
            string column1 = string.Empty;
            string column2 = string.Empty;
            string column3 = string.Empty;
            string column4 = string.Empty;
            string groupColumn = string.Empty;
            string where = this.AddInspectionWhere(string.Empty, inspectionType);
            if (inspectionType == "Physical")
            {
                columnSource = @"
outer apply(
    select Dyelot = count(1)
    from(
        select distinct b.Dyelot
        from {0}_Detail b with (nolock)
		inner join FIR_Physical i with (nolock) on i.ID = f.FirID and i.Roll = b.Roll and i.Dyelot = b.Dyelot and i.Result <> ''
        where b.id = f.ReceivingID and b.PoId = f.ToPOID and b.Seq1 = f.ToSeq1 and b.Seq2 = f.ToSeq2 and b.Roll = f.FromRoll and b.Dyelot = f. FromDyelot
    )x
)ct3
";
                column1 = "ct3.Dyelot,";
                groupColumn = ",ct3.Dyelot";
            }

            if (this.radioWKSeq.Checked)
            {
                if (inspectionType == "Physical")
                {
                    column2 = "f.TotalInspYds,";
                    groupColumn += ",f.TotalInspYds";
                }
            }
            else
            {
                string firtable = inspectionType == "ShadeBond" ? "ShadeBone" : inspectionType;
                columnSource += $@"
left join FIR_{firtable} i with (nolock) on i.ID = f.FirID and i.Roll = f.FromRoll and i.Dyelot = f.FromDyelot
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";
                column3 = "--"; // -- f.{1}Date
                column4 = @"
	,f.FromRoll,f.FromDyelot,
";
                groupColumn += @"
	,f.FromRoll,f.FromDyelot
";
                switch (inspectionType)
                {
                    case "Physical":
                        column4 += @"
	i.TicketYds,
	i.ActualYds,
	[DiffLth] = i.ActualYds - i.TicketYds,
	i.TransactionID,
	f.Width,
	i.FullWidth,
	i.ActualWidth,
	i.TotalPoint,
	i.PointRate,
	i.Result,
	i.Grade,
	[Moisture] = iif(i.Moisture = 1, 'Y', ''),
	i.Remark,
	i.InspDate,
	Inspector = Concat(i.Inspector, '-', p3.Name)
";
                        groupColumn += @"
	,i.TicketYds,i.ActualYds,i.TransactionID,f.Width,
	i.FullWidth,i.ActualWidth,i.TotalPoint,i.PointRate,i.Result,i.Grade,i.Moisture,i.Remark,i.InspDate,i.Inspector,p3.Name
";
                        break;
                    case "Weight":
                        column4 += @"
    i.WeightM2,
    i.AverageWeightM2,
    i.Difference,
    i.Result,
    i.InspDate,
	Inspector = Concat(i.Inspector, '-', p3.Name),
    i.Remark
";
                        groupColumn += @"
   ,i.WeightM2,i.AverageWeightM2,i.Difference,i.Result,i.InspDate,i.Inspector,p3.Name,i.Remark
";
                        break;
                    case "ShadeBond":
                        column4 += @"
    i.TicketYds,
    i.Scale,
    i.Result,
    i.Tone,
    i.InspDate,
	Inspector = Concat(i.Inspector, '-', p3.Name),
    i.Remark
";
                        groupColumn += @"
   ,i.TicketYds,i.Scale,i.Result,i.Tone,i.InspDate,i.Inspector,p3.Name,i.Remark
";
                        break;
                    case "Continuity":
                        column4 += @"
    i.TicketYds,
    i.Scale,
    i.Result,
    i.InspDate,
	Inspector = Concat(i.Inspector, '-', p3.Name),
    i.Remark
";
                        groupColumn += @"
    ,i.TicketYds,i.Scale,i.Result,i.InspDate,i.Inspector,p3.Name,i.Remark
";
                        break;
                    case "Odor":
                        column4 += @"
    i.Result,
    i.InspDate,
	Inspector = Concat(i.Inspector, '-', p3.Name),
    i.Remark
";
                        groupColumn += @"
   ,i.Result,i.InspDate,i.Inspector,p3.Name,i.Remark
";
                        break;
                }
            }

            return string.Format(this.B2A_select, "R", inspectionType, string.Format(columnSource, "Receiving"), column1, column2, column3, column4, groupColumn, where) +
                "\r\nunion all\r\n" +
                string.Format(this.B2A_select, "T", inspectionType, string.Format(columnSource, "TransferIn"), column1, column2, column3, column4, groupColumn, where);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.radioPanelTransaction.Value == "1")
            {
                if (this.dateArriveWHDate.Value1.Empty() &&
                    this.txtSP1.Text.Empty() && this.txtSP2.Text.Empty() &&
                    this.txtWK1.Text.Empty() && this.txtWK2.Text.Empty() &&
                    !this.dateInspectionDate.HasValue)
                {
                    MyUtility.Msg.WarningBox("Arrive W/H Date, SP#, WK# and Inspection Date can't all empty!");
                    return false;
                }
            }
            else
            {
                if (this.dateArriveWHDate.Value1.Empty() &&
                    this.txtSP1.Text.Empty() && this.txtSP2.Text.Empty() &&
                    this.txtWK1.Text.Empty() && this.txtWK2.Text.Empty())
                {
                    MyUtility.Msg.WarningBox("Arrive W/H Date, SP#, WK# can't all empty!");
                    return false;
                }
            }

            this.Sqlcmd = string.Empty;
            this.parameters.Clear();

            string where1 = string.Empty;
            string where2 = "and not exists(select 1 from #tmpR r where r.Ukey = std.Ukey)" + Environment.NewLine;

            if (this.radioPanelTransaction.Value == "1")
            {
                if (!this.dateArriveWHDate.Value1.Empty())
                {
                    where1 += $"and a.WhseArrival between @Date1 and @Date2" + Environment.NewLine;
                    where2 += $"and a.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@Date1", this.dateArriveWHDate.Value1));
                    this.parameters.Add(new SqlParameter("@Date2", this.dateArriveWHDate.Value2));
                }

                if (!this.txtSP1.Text.Empty())
                {
                    where1 += $"and f.POID between @SP1 and @SP2" + Environment.NewLine;
                    where2 += $"and f.POID between @SP1 and @SP2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                    this.parameters.Add(new SqlParameter("@SP2", this.txtSP2.Text));
                }

                if (!this.txtWK1.Text.Empty())
                {
                    where1 += $"and a.ExportID between @ExportID1 and @ExportID2" + Environment.NewLine;

                    // Trandfer In 沒有 WK#
                    where2 += $"and 1=0" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@ExportID1", this.txtWK1.Text));
                    this.parameters.Add(new SqlParameter("@ExportID2", this.txtWK2.Text));
                }

                if (this.dateInspectionDate.HasValue)
                {
                    this.parameters.Add(new SqlParameter("@InsDate1", this.dateInspectionDate.Value1));
                    this.parameters.Add(new SqlParameter("@InsDate2", this.dateInspectionDate.Value2));
                }

                #region Physical
                if (this.comboInspection.Text == "Physical" || this.comboInspection.Text == "All")
                {
                    string joinPhysical = @"
left join pass1 p1 with (nolock) on p1.id = f.PhysicalInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve

";

                    string colPhysicalWKSeqOnly = this.radioWKSeq.Checked ? "f.TotalInspYds," : string.Empty;
                    string colPhysicalDateWKSeqOnly = this.radioWKSeq.Checked ? "f.PhysicalDate," : string.Empty;

                    string colPhysical = $@"
Dyelot2=ct3.Dyelot,
[NonPhysical] = iif(f.NonPhysical = 1, 'Y', ''),
f.Physical,
{colPhysicalWKSeqOnly}
[PhysicalInspector] = Concat(p1.ID, '-', p1.Name),
{colPhysicalDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate";
                    if (this.radioRollDyelot.Checked)
                    {
                        joinPhysical += @"
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";
                        colPhysical += @",
b.Roll,
b.Dyelot,
i.TicketYds,
i.ActualYds,
[DiffLth] = i.ActualYds - i.TicketYds,
i.TransactionID,
fa.Width,
i.FullWidth,
i.ActualWidth,
i.TotalPoint,
i.PointRate,
i.Result,
i.Grade,
[Moisture] = iif(i.Moisture = 1, 'Y', ''),
i.Remark,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name)
";
                    }

                    this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colPhysical, this.AddJoinByReportType("Receiving", "FIR_Physical", "Physical"), this.AddInspectionWhere(where1, "Physical"), joinPhysical)}
union all
{string.Format(this.baseTransferInSql, colPhysical, this.AddJoinByReportType("TransferIn", "FIR_Physical", "Physical"), this.AddInspectionWhere(where2, "Physical"), joinPhysical)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Weight
                if (this.comboInspection.Text == "Weight" || this.comboInspection.Text == "All")
                {
                    string joinWeight = @"
left join pass1 p1 with (nolock) on p1.id = f.WeightInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colWeightDateWKSeqOnly = this.radioWKSeq.Checked ? "f.WeightDate," : string.Empty;

                    string colWeight = $@"
[NonWeight] = iif(f.NonWeight = 1, 'Y', ''),
f.Weight,
[WeightInspector] = Concat(p1.ID, '-', p1.Name),
{colWeightDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    if (this.radioRollDyelot.Checked)
                    {
                        joinWeight += @"
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";
                        colWeight += @",
b.Roll,
b.Dyelot,
i.WeightM2,
i.AverageWeightM2,
i.Difference,
i.Result,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                    }

                    this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colWeight, this.AddJoinByReportType("Receiving", "FIR_Weight"), this.AddInspectionWhere(where1, "Weight"), joinWeight)}
union all
{string.Format(this.baseTransferInSql, colWeight, this.AddJoinByReportType("TransferIn", "FIR_Weight"), this.AddInspectionWhere(where2, "Weight"), joinWeight)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Shade Band
                if (this.comboInspection.Text == "Shade Band" || this.comboInspection.Text == "All")
                {
                    string joinShadeBond = @"
left join pass1 p1 with (nolock) on p1.id = f.ShadeboneInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colShadeBondDateWKSeqOnly = this.radioWKSeq.Checked ? "f.ShadebondDate," : string.Empty;

                    string colShadeBond = $@"
[NonShadeBond] = iif(f.NonShadeBond = 1, 'Y', ''),
f.Shadebond,
[ShadeboneInspector] = Concat(p1.ID, '-', p1.Name),
{colShadeBondDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    if (this.radioRollDyelot.Checked)
                    {
                        joinShadeBond += @"
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";
                        colShadeBond += @",
b.Roll,
b.Dyelot,
i.TicketYds,
i.Scale,
i.Result,
i.Tone,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                    }

                    this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colShadeBond, this.AddJoinByReportType("Receiving", "FIR_Shadebone"), this.AddInspectionWhere(where1, "ShadeBond"), joinShadeBond)}
union all
{string.Format(this.baseTransferInSql, colShadeBond, this.AddJoinByReportType("TransferIn", "FIR_Shadebone"), this.AddInspectionWhere(where2, "ShadeBond"), joinShadeBond)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Continuity
                if (this.comboInspection.Text == "Continuity" || this.comboInspection.Text == "All")
                {
                    string joinContinuity = @"
left join pass1 p1 with (nolock) on p1.id = f.ContinuityInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colContinuityDateWKSeqOnly = this.radioWKSeq.Checked ? "f.ContinuityDate," : string.Empty;

                    string colContinuity = $@"
[NonContinuity] = iif(f.NonContinuity = 1, 'Y', ''),
f.Continuity,
[ContinuityInspector] = Concat(p1.ID, '-', p1.Name),
{colContinuityDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    if (this.radioRollDyelot.Checked)
                    {
                        joinContinuity += @"
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";
                        colContinuity += @",
b.Roll,
b.Dyelot,
i.TicketYds,
i.Scale,
i.Result,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                    }

                    this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colContinuity, this.AddJoinByReportType("Receiving", "FIR_Continuity"), this.AddInspectionWhere(where1, "Continuity"), joinContinuity)}
union all
{string.Format(this.baseTransferInSql, colContinuity, this.AddJoinByReportType("TransferIn", "FIR_Continuity"), this.AddInspectionWhere(where2, "Continuity"), joinContinuity)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Odor
                if (this.comboInspection.Text == "Odor" || this.comboInspection.Text == "All")
                {
                    string joinOdor = @"
left join pass1 p1 with (nolock) on p1.id = f.OdorInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colOdorDateWKSeqOnly = this.radioWKSeq.Checked ? "f.OdorDate," : string.Empty;

                    string colOdor = $@"
[NonOdor] = iif(f.NonOdor = 1, 'Y', ''),
f.Odor,
[OdorInspector] = Concat(p1.ID, '-', p1.Name),
{colOdorDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    if (this.radioRollDyelot.Checked)
                    {
                        joinOdor += @"
left join pass1 p3 with (nolock) on p3.id = i.Inspector
";
                        colOdor += @",
b.Roll,
b.Dyelot,
i.Result,
i.InspDate,
Inspector = Concat(p3.ID, '-', p3.Name),
i.Remark
";
                    }

                    this.Sqlcmd += $@"
{string.Format(this.baseReceivingSql, colOdor, this.AddJoinByReportType("Receiving", "FIR_Odor"), this.AddInspectionWhere(where1, "Odor"), joinOdor)}
union all
{string.Format(this.baseTransferInSql, colOdor, this.AddJoinByReportType("TransferIn", "FIR_Odor"), this.AddInspectionWhere(where2, "Odor"), joinOdor)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion
            }

            // B2A
            else
            {
                if (!this.dateArriveWHDate.Value1.Empty())
                {
                    where1 += $"and st.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                    where2 += $"and st.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@Date1", this.dateArriveWHDate.Value1));
                    this.parameters.Add(new SqlParameter("@Date2", this.dateArriveWHDate.Value2));
                }

                if (!this.txtSP1.Text.Empty())
                {
                    where1 += $"and std.ToPOID between @SP1 and @SP2" + Environment.NewLine;
                    where2 += $"and std.ToPOID between @SP1 and @SP2" + Environment.NewLine;
                    this.parameters.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                    this.parameters.Add(new SqlParameter("@SP2", this.txtSP2.Text));
                }

                if (!this.txtWK1.Text.Empty())
                {
                    where1 += $"and st.ID between @TID1 and @TID2" + Environment.NewLine;
                    where2 += $"and st.ID between @TID1 and @TID2" + Environment.NewLine; // Trandfer In 沒有 WK#
                    this.parameters.Add(new SqlParameter("@TID1", this.txtWK1.Text));
                    this.parameters.Add(new SqlParameter("@TID2", this.txtWK2.Text));
                }

                if (this.dateInspectionDate.HasValue)
                {
                    this.parameters.Add(new SqlParameter("@InsDate1", this.dateInspectionDate.Value1));
                    this.parameters.Add(new SqlParameter("@InsDate2", this.dateInspectionDate.Value2));
                }

                // 基本資料
                this.Sqlcmd = string.Format(this.B2A_sqlcmd, "Receiving_Detail", "Receiving", "R", where1, "a.ExportId", "inner") +
                    string.Format(this.B2A_sqlcmd, "TransferIn_Detail", "TransferIn", "T", where2, "''", "left");

                // 要撈哪些
                switch (this.comboInspection.Text)
                {
                    case "Physical":
                        this.Sqlcmd += this.B2A_Select("Physical");
                        break;
                    case "Weight":
                        this.Sqlcmd += this.B2A_Select("Weight");
                        break;
                    case "Shade Band":
                        this.Sqlcmd += this.B2A_Select("ShadeBond");
                        break;
                    case "Continuity":
                        this.Sqlcmd += this.B2A_Select("Continuity");
                        break;
                    case "Odor":
                        this.Sqlcmd += this.B2A_Select("Odor");
                        break;
                    default:
                        this.Sqlcmd += this.B2A_Select("Physical") + this.B2A_Select("Weight") + this.B2A_Select("ShadeBond") + this.B2A_Select("Continuity") + this.B2A_Select("Odor");
                        break;
                }
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.Sqlcmd.ToString(), this.parameters, out this.PrintData);
        }

        private void CreateExcel(string filename, string excelTitle, DataTable dtSrc)
        {
            int excelMaxRow = 1000000;
            int maxLoadRow = 250000;
            int rowCnt = dtSrc.Rows.Count;
            int loadTimes = Convert.ToInt32(Math.Ceiling((decimal)excelMaxRow / (decimal)maxLoadRow));
            DataTable dt;

            if (rowCnt > excelMaxRow)
            {
                Excel.Application excel = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);

                excel.DisplayAlerts = false;
                Utility.Report.ExcelCOM comDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excel);

                int sheetCnt = Convert.ToInt32(Math.Ceiling((decimal)rowCnt / (decimal)excelMaxRow));

                for (int i = 0; i < sheetCnt; i++)
                {
                    Excel.Worksheet worksheetA = (Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1];
                    Excel.Worksheet worksheetB = (Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 2];
                    worksheetA.Copy(worksheetB);
                    ((Excel.Worksheet)excel.Sheets[i + 1]).Select();

                    for (int j = 0; j < loadTimes; j++)
                    {
                        int startRow = (i * excelMaxRow) + (j * maxLoadRow);
                        dt = dtSrc.AsEnumerable().Skip(startRow).Take(maxLoadRow).CopyToDataTable();

                        if (startRow > rowCnt)
                        {
                            break;
                        }

                        comDetail.WriteTable(dt, (j * maxLoadRow) + 2);
                    }
                }

                ((Excel.Worksheet)excel.Sheets[sheetCnt + 1]).Delete();
                ((Excel.Worksheet)excel.Sheets[1]).Select();
                this.SaveExcelwithName(excel, excelTitle);
            }
            else
            {
                dt = dtSrc;
                Excel.Application excelDetail = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                Utility.Report.ExcelCOM comDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelDetail)
                {
                    ColumnsAutoFit = true,
                };

                comDetail.WriteTable(dt, 2);
                this.SaveExcelwithName(excelDetail, excelTitle);
            }
        }

        private void SaveExcelwithName(Excel.Application excelapp, string filename)
        {
            string strExcelName = Class.MicrosoftFile.GetName(filename);
            Excel.Workbook workbook = excelapp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelapp.Quit();
            Marshal.ReleaseComObject(excelapp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);
            strExcelName.OpenFile();
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.PrintData.Sum(s => s.Rows.Count));

            if (!this.PrintData.Where(s => s.Rows.Count > 0).Any())
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string transaction = this.radioPanelTransaction.Value == "1" ? string.Empty : "B2A_";
            string reportType = this.radioWKSeq.Checked ? "WKSeq" : "RollDyelot";
            string reportTitle = this.radioWKSeq.Checked ? " By WK, Seq" : " By Roll, Dyelot";

            if (this.comboInspection.Text == "All")
            {
                this.CreateExcel($"Quality_R12_{transaction}Physical{reportType}.xltx", $"R12 Fabric Physical Inspection List{reportTitle}", this.PrintData[0]);
                this.CreateExcel($"Quality_R12_{transaction}Weight{reportType}.xltx", $"R12 Fabric Weight Test List{reportTitle}", this.PrintData[1]);
                this.CreateExcel($"Quality_R12_{transaction}ShadeBond{reportType}.xltx", $"R12 Fabric Shade Band Test List{reportTitle}", this.PrintData[2]);
                this.CreateExcel($"Quality_R12_{transaction}Continuity{reportType}.xltx", $"R12 Fabric Continuilty Test List{reportTitle}", this.PrintData[3]);
                this.CreateExcel($"Quality_R12_{transaction}Odor{reportType}.xltx", $"R12 Fabric Odor Test List{reportTitle}", this.PrintData[4]);
            }
            else
            {
                switch (this.comboInspection.Text)
                {
                    case "Physical":
                        this.CreateExcel($"Quality_R12_{transaction}Physical{reportType}.xltx", $"R12 Fabric Physical Inspection List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Weight":
                        this.CreateExcel($"Quality_R12_{transaction}Weight{reportType}.xltx", $"R12 Fabric Weight Test List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Shade Band":
                        this.CreateExcel($"Quality_R12_{transaction}ShadeBond{reportType}.xltx", $"R12 Fabric Shade Band Test List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Continuity":
                        this.CreateExcel($"Quality_R12_{transaction}Continuity{reportType}.xltx", $"R12 Fabric Continuilty Test List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Odor":
                        this.CreateExcel($"Quality_R12_{transaction}Odor{reportType}.xltx", $"R12 Fabric Odor Test List{reportTitle}", this.PrintData[0]);
                        break;
                    default:
                        break;
                }
            }

            this.HideWaitMessage();
            return true;
        }

        private void RadioPanelTransaction_ValueChanged(object sender, EventArgs e)
        {
            switch (this.radioPanelTransaction.Value)
            {
                case "1":
                    this.lbDate.Text = "Arrive W/H Date";
                    this.lbSP.Text = "SP#";
                    this.label3.Text = "WK#";
                    this.label6.RectStyle.Color = Color.SkyBlue;
                    this.label6.TextStyle.Color = Color.Black;
                    break;
                case "2":
                    this.lbDate.Text = "Issue Date";
                    this.lbSP.Text = "Bulk SP#";
                    this.label3.Text = "Transfer ID";
                    this.label6.RectStyle.Color = Color.Gray;
                    this.label6.TextStyle.Color = Color.White;
                    break;
            }
        }
    }
}
