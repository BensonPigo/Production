using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class QA_R12
    {
        private string baseReceivingSql = @"
select  [Category] = ddl.[Name],
        who.SeasonID,
        f.POID,
		[Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2),
        o.FactoryID,
		a.ExportId,
        [Invno] = iif( isnull(a.id, '')  = '','' ,a. Invno) ,
		f.ReceivingID,
		o.StyleID,
		o.BrandID,
		f.Suppid,
		psd.Refno,
		ColorID = isnull(psdsC.SpecValue ,''),
        [Cutting Date] = o.CutInLine,
		[ArriveWH_Date] = a.WhseArrival,
        f.ArriveQty,
        fa.WeaveTypeID,
        [TotalRoll] = ct.Roll,
        [TotalLot] = ct2.Dyelot,
        {0}
        ,o.OrderTypeID 
        {4}
from Fir f with (nolock)
inner join Receiving a with (nolock) on a.Id = f.ReceivingID
inner join Orders o with (nolock) on o.ID = f.POID
left join View_WH_Orders who with(nolock) on o.id = who.id
left join DropDownList ddl with(nolock) on o.Category = ddl.id and ddl.[Type] = 'Category'
left join PO_Supp_Detail psd with (nolock) on psd.ID = f.POID and psd.SEQ1 = f.SEQ1 and psd.SEQ2 = f.SEQ2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
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
outer apply(
    select Tone = count(1)
    from(
        select distinct s.Tone
        from Receiving_Detail b with (nolock)
		left join FIR_Shadebone s with (nolock) on s.ID = f.ID and s.Roll = b.Roll and s.Dyelot = b.Dyelot
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ctTone
{1}
{3}
where 1 = 1
{2}
";

        private string baseTransferInSql = @"
select	[Category] = ddl.[Name],
        who.SeasonID,
        f.POID,
		[Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2),
        o.FactoryID,
		[ExportId] = '',
        [Invno] = iif( isnull(a.id, '')  = '','' ,a. Invno) ,
		f.ReceivingID,
		o.StyleID,
		o.BrandID,
		f.Suppid,
		psd.Refno,
		ColorID = isnull(psdsC.SpecValue ,''),
        [Cutting Date] = o.CutInLine,
		[ArriveWH_Date] = a.IssueDate,
        f.ArriveQty,
        fa.WeaveTypeID,
        ct.Roll,
        ct2.Dyelot,
        {0}
        ,o.OrderTypeID 
        {4}
from Fir f with (nolock)
inner join TransferIn a with (nolock) on a.Id = f.ReceivingID
inner join Orders o with (nolock) on o.ID = f.POID
left join View_WH_Orders who with(nolock) on o.id = who.id
left join DropDownList ddl with(nolock) on o.Category = ddl.id and ddl.[Type] = 'Category'
left join PO_Supp_Detail psd with (nolock) on psd.ID = f.POID and psd.SEQ1 = f.SEQ1 and psd.SEQ2 = f.SEQ2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
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
outer apply(
    select Tone = count(1)
    from(
        select distinct s.Tone
        from TransferIn_Detail b with (nolock)
		left join FIR_Shadebone s with (nolock) on s.ID = f.ID and s.Roll = b.Roll and s.Dyelot = b.Dyelot
        where b.id = a.id and b.POID = f.POID and b.SEQ1 = f.SEQ1 and b.SEQ2 = f.SEQ2
    )x
)ctTone
{1}
{3}
where 1 = 1
{2}
";

        private string B2A_sqlcmd = @"
select
	TransferID = st.Id,
who.SeasonID,
	st.IssueDate,
	std.FromPOID,
	FromSeq = CONCAT(std.FromSeq1, '-' + std.FromSeq2),
    [FromFty] = o.FactoryID,
    [ToFty] = ToOrder.FactoryID,
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
    [Invno] = iif( isnull(a.id, '')  = '','' ,a. Invno) ,
	f.ReceivingID,
	f.Suppid,
	Approve = Concat (f.Approve, '-', p2.Name),
	f.ApproveDate,
	f.TotalInspYds,
	FirID = f.ID,
    f.ArriveQty,

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
    ColorID = isnull(psdsC.SpecValue ,''),
	ctRoll = ct.Roll,
	ctDyelot = ct2.Dyelot,
	std.Ukey
	,[Category] = ddl.Name
	,[CuttingData] = o.CutInLine
	,[ArriveWHData] = a.{6}
    ,o.OrderTypeID 
	,ctTone = ctTone.Tone
into #tmp{2}
from SubTransfer st
inner join SubTransfer_Detail std on std.id = st.id
inner join Orders O on O.ID = std.FromPOID
left join View_WH_Orders who with(nolock) on o.id = who.id
left join DropDownList ddl with(nolock) on o.Category = ddl.id and ddl.[Type] = 'Category'
left  join Orders ToOrder on ToOrder.id = std.ToPOID
inner join PO_Supp_Detail PSD on PSD.ID = std.FromPOID and PSD.SEQ1 = std.FromSeq1 and PSD.SEQ2 = std.FromSeq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
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
        select distinct std2.FromRoll, std2.FromDyelot
        from SubTransfer_Detail std2 with (nolock)
		inner join {0} b2 on b2.PoId = std2.FromPOID 
                             and b2.Seq1 = std2.FromSeq1 
                             and b2.Seq2 = std2.FromSeq2
                             and b2.Roll = std2.FromRoll 
                             and b2.Dyelot = std2. FromDyelot
        where std2.id = st.id 
                and std.ToPOID = std2.ToPOID 
                and std.ToSeq1 = std2.ToSeq1 
                and std.ToSeq2 = std2.ToSeq2
		        and std.FromPOID = std2.FromPOID 
                and std.FromSeq1 = std2.FromSeq1 
                and std.FromSeq2 = std2.FromSeq2 
		        and b2.id = a.id
    )x
)ct
outer apply(
    select Dyelot = count(1)
    from(
        select distinct std2.FromDyelot
        from SubTransfer_Detail std2 with (nolock)
		inner join {0} b2 on b2.PoId = std2.FromPOID 
                             and b2.Seq1 = std2.FromSeq1 
                             and b2.Seq2 = std2.FromSeq2
                             and b2.Roll = std2.FromRoll 
                             and b2.Dyelot = std2. FromDyelot
        where std2.id = st.id 
                and std.ToPOID = std2.ToPOID 
                and std.ToSeq1 = std2.ToSeq1 
                and std.ToSeq2 = std2.ToSeq2
                and std.FromPOID = std2.FromPOID 
                and std.FromSeq1 = std2.FromSeq1 
                and std.FromSeq2 = std2.FromSeq2 
                and b2.id = a.id
    )x
)ct2
outer apply(
    select Tone = count(1)
    from(
        select distinct Fir_s.Tone
        from SubTransfer_Detail std2 with (nolock)
		inner join {0} b2 on b2.PoId = std2.FromPOID 
                             and b2.Seq1 = std2.FromSeq1 
                             and b2.Seq2 = std2.FromSeq2
                             and b2.Roll = std2.FromRoll 
                             and b2.Dyelot = std2. FromDyelot
		left join FIR_Shadebone Fir_s with (nolock) on Fir_s.ID = f.ID and Fir_s.Roll = b2.Roll and Fir_s.Dyelot = b2.Dyelot
        where std2.id = st.id 
                and std.ToPOID = std2.ToPOID 
                and std.ToSeq1 = std2.ToSeq1 
                and std.ToSeq2 = std2.ToSeq2
                and std.FromPOID = std2.FromPOID 
                and std.FromSeq1 = std2.FromSeq1 
                and std.FromSeq2 = std2.FromSeq2 
                and b2.id = a.id
    )x
)ctTone
Where st.type = 'B' and st.Status = 'Confirmed' and PSD.FabricType = 'F'
{3}
";

        private string B2A_select = @"
select
    f.SeasonID,
	f.ToPOID,
    f.Category,
	f.ToSeq,
    f.ToFty,
	f.TransferID,
	f.FromPOID,
	f.FromSeq,
	f.WK,
    f.[Invno],
	f.ReceivingID,
	f.StyleID,
	f.BrandID,
	f.Suppid,
	f.Refno,
	f.ColorID,
	f.CuttingData,
	f.ArriveWHData,
	f.IssueDate,
	TransferQty = sum(f.TransferQty),
	f.WeaveTypeID,
	f.ctRoll,
	f.ctDyelot,

	{3}

	Non{1} = IIF(f.Non{1} = 1,'Y','') ,
	f.{1},

	{4}

    {9}
	{1}Inspector = Concat (f.{1}Inspector, '-', p1.Name),

	{5}f.{1}Date,

	f.Approve,
	f.ApproveDate

    {6}
    ,OrderTypeID 
    {11}
from #tmp{0} f
left join pass1 p1 with (nolock) on p1.id = f.{1}Inspector
{2}
where 1=1
{8}
group by f.ToPOID,f.ToSeq,f.FromFty,f.ToFty,f.TransferID,f.FromPOID,f.FromSeq,f.WK,f.ReceivingID,f.StyleID,f.BrandID,f.Suppid,
	f.Refno,f.ColorID,f.IssueDate,f.WeaveTypeID,f.ctRoll,f.ctDyelot,f.Approve,f.ApproveDate,p1.Name,f.Category,f.ArriveWHData,f.CuttingData,
    f.OrderTypeID,f.[Invno],f.SeasonID,
	f.Non{1},f.{1},f.{1}Inspector,f.{1}Date
	{7},f.ArriveQty {10}

";

        /// <inheritdoc/>
        public QA_R12()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <summary>
        /// 根據指定的篩選條件取得 QA R12 資料。
        /// </summary>
        /// <remarks>
        /// 此方法會根據提供的篩選條件執行資料庫查詢，取得 QA R12 資料。
        /// 篩選條件包含日期區間、檢驗類型（如 Physical、Weight、Shade Band、Continuity、Odor）及其他參數（如品牌、出口 ID 等）。
        /// 回傳的 <see cref="Base_ViewModel"/> 會包含查詢結果及相關的資料表。若無符合條件的資料，回傳的資料表將為空。
        /// </remarks>
        /// <param name="model">一個 <see cref="QA_R12_ViewModel"/> 實例，包含篩選條件，如日期區間、檢驗類型及其他參數。</param>
        /// <returns>一個 <see cref="Base_ViewModel"/>，包含查詢結果及符合條件的資料表。</returns>
        /// </summary>
        public Base_ViewModel GetQA_R12Data(QA_R12_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@Date1", SqlDbType.Date) { Value = (object)model.ArriveWHDate1 ?? DBNull.Value },
                new SqlParameter("@Date2", SqlDbType.Date) { Value = (object)model.ArriveWHDate2 ?? DBNull.Value },
                new SqlParameter("@SP1", model.SP1),
                new SqlParameter("@SP2", model.SP2),
                new SqlParameter("@ExportID1", model.WK1),
                new SqlParameter("@ExportID2", model.WK2),
                new SqlParameter("@InsDate1", SqlDbType.Date) { Value = (object)model.InspectionDate1 ?? DBNull.Value },
                new SqlParameter("@InsDate2", SqlDbType.Date) { Value = (object)model.InspectionDate2 ?? DBNull.Value },
                new SqlParameter("@BrandID", model.Brand),
            };

            string sqlcmd = string.Empty;
            string where1 = string.Empty;
            string where2 = string.Empty;

            if (model.Transaction == 1)
            {
                if (model.ArriveWHDate1.HasValue)
                {
                    where1 += $"and a.WhseArrival between @Date1 and @Date2" + Environment.NewLine;
                    where2 += $"and a.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                }

                if (!model.SP1.Empty())
                {
                    where1 += $"and f.POID between @SP1 and @SP2" + Environment.NewLine;
                    where2 += $"and f.POID between @SP1 and @SP2" + Environment.NewLine;
                }

                if (!model.WK1.Empty())
                {
                    where1 += $"and a.ExportID between @ExportID1 and @ExportID2" + Environment.NewLine;

                    // Trandfer In 沒有 WK#
                    where2 += $"and 1=0" + Environment.NewLine;
                }

                if (!model.Brand.Empty())
                {
                    where1 += $"and o.BrandID = @BrandID" + Environment.NewLine;
                    where2 += $"and o.BrandID = @BrandID" + Environment.NewLine;
                }

                #region Physical
                if (model.Inspection == "Physical" || model.Inspection == "All")
                {
                    string joinPhysical = @"
left join pass1 p1 with (nolock) on p1.id = f.PhysicalInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
outer apply(select ttlActualYds = sum(ActualYds) from FIR_Physical fp where fp.id = f.ID ) fp

";

                    string colPhysicalWKSeqOnly = model.ByWKSeq ? "f.TotalInspYds," : string.Empty;
                    string colPhysicalDateWKSeqOnly = model.ByWKSeq ? "f.PhysicalDate," : string.Empty;
                    string colPhysicalDateWKSeqOnly2 = model.ByWKSeq ? "[Inspection_Percent] = CONCAT(convert(decimal(20,2),ROUND(isnull(fp.ttlActualYds, 0) / f.ArriveQty * 100.0, 2)), '%')," : string.Empty;

                    string colPhysical = $@"
[InspectedTotalLot] = ct3.Dyelot,
[NotInspectedDyelot]= ct2.Dyelot - ct3.Dyelot,
[NonPhysical] = iif(f.NonPhysical = 1, 'Y', ''),
f.Physical,
{colPhysicalWKSeqOnly}
{colPhysicalDateWKSeqOnly2}
[PhysicalInspector] = Concat(p1.ID, '-', p1.Name),
{colPhysicalDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate";
                    if (model.ByRollDyelot)
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

                    sqlcmd += $@"
{string.Format(this.baseReceivingSql, colPhysical, this.AddJoinByReportType(model, "Receiving", "FIR_Physical", "Physical"), this.AddInspectionWhere(model, where1, "Physical"), joinPhysical, string.Empty)}
union all
{string.Format(this.baseTransferInSql, colPhysical, this.AddJoinByReportType(model, "TransferIn", "FIR_Physical", "Physical"), this.AddInspectionWhere(model, where2, "Physical"), joinPhysical, string.Empty)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Weight
                if (model.Inspection == "Weight" || model.Inspection == "All")
                {
                    string joinWeight = @"
left join pass1 p1 with (nolock) on p1.id = f.WeightInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colWeightDateWKSeqOnly = model.ByWKSeq ? "f.WeightDate," : string.Empty;

                    string colWeight = $@"
[NonWeight] = iif(f.NonWeight = 1, 'Y', ''),
f.Weight,
[WeightInspector] = Concat(p1.ID, '-', p1.Name),
{colWeightDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    if (model.ByRollDyelot)
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

                    sqlcmd += $@"
{string.Format(this.baseReceivingSql, colWeight, this.AddJoinByReportType(model, "Receiving", "FIR_Weight"), this.AddInspectionWhere(model, where1, "Weight"), joinWeight, string.Empty)}
union all
{string.Format(this.baseTransferInSql, colWeight, this.AddJoinByReportType(model, "TransferIn", "FIR_Weight"), this.AddInspectionWhere(model, where2, "Weight"), joinWeight, string.Empty)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Shade Band
                if (model.Inspection == "Shade Band" || model.Inspection == "All")
                {
                    string joinShadeBond = @"
left join pass1 p1 with (nolock) on p1.id = f.ShadeboneInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colShadeBondDateWKSeqOnly = model.ByWKSeq ? "f.ShadebondDate," : string.Empty;

                    string colShadeBond = $@"
[NonShadeBond] = iif(f.NonShadeBond = 1, 'Y', ''),
f.Shadebond,
[ShadeboneInspector] = Concat(p1.ID, '-', p1.Name),
{colShadeBondDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    string ttlColorTone = string.Empty;
                    if (model.ByRollDyelot)
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
                    else
                    {
                        ttlColorTone = ",ctTone.Tone";
                    }

                    sqlcmd += $@"
{string.Format(this.baseReceivingSql, colShadeBond, this.AddJoinByReportType(model, "Receiving", "FIR_Shadebone"), this.AddInspectionWhere(model, where1, "ShadeBond"), joinShadeBond, ttlColorTone)}
union all
{string.Format(this.baseTransferInSql, colShadeBond, this.AddJoinByReportType(model, "TransferIn", "FIR_Shadebone"), this.AddInspectionWhere(model, where2, "ShadeBond"), joinShadeBond, ttlColorTone)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Continuity
                if (model.Inspection == "Continuity" || model.Inspection == "All")
                {
                    string joinContinuity = @"
left join pass1 p1 with (nolock) on p1.id = f.ContinuityInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colContinuityDateWKSeqOnly = model.ByWKSeq ? "f.ContinuityDate," : string.Empty;

                    string colContinuity = $@"
[NonContinuity] = iif(f.NonContinuity = 1, 'Y', ''),
f.Continuity,
[ContinuityInspector] = Concat(p1.ID, '-', p1.Name),
{colContinuityDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    if (model.ByRollDyelot)
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

                    sqlcmd += $@"
{string.Format(this.baseReceivingSql, colContinuity, this.AddJoinByReportType(model, "Receiving", "FIR_Continuity"), this.AddInspectionWhere(model, where1, "Continuity"), joinContinuity, string.Empty)}
union all
{string.Format(this.baseTransferInSql, colContinuity, this.AddJoinByReportType(model, "TransferIn", "FIR_Continuity"), this.AddInspectionWhere(model, where2, "Continuity"), joinContinuity, string.Empty)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion

                #region Odor
                if (model.Inspection == "Odor" || model.Inspection == "All")
                {
                    string joinOdor = @"
left join pass1 p1 with (nolock) on p1.id = f.OdorInspector
left join pass1 p2 with (nolock) on p2.id = f.Approve
";
                    string colOdorDateWKSeqOnly = model.ByWKSeq ? "f.OdorDate," : string.Empty;

                    string colOdor = $@"
[NonOdor] = iif(f.NonOdor = 1, 'Y', ''),
f.Odor,
[OdorInspector] = Concat(p1.ID, '-', p1.Name),
{colOdorDateWKSeqOnly}
[Approver] = Concat(p2.ID, '-', p2.Name),
f.ApproveDate
";
                    if (model.ByRollDyelot)
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

                    sqlcmd += $@"
{string.Format(this.baseReceivingSql, colOdor, this.AddJoinByReportType(model, "Receiving", "FIR_Odor"), this.AddInspectionWhere(model, where1, "Odor"), joinOdor, string.Empty)}
union all
{string.Format(this.baseTransferInSql, colOdor, this.AddJoinByReportType(model, "TransferIn", "FIR_Odor"), this.AddInspectionWhere(model, where2, "Odor"), joinOdor, string.Empty)}
order by POID, Seq, ExportId, ReceivingID
";
                }
                #endregion
            }

            // B2A
            else
            {
                where2 = "and not exists(select 1 from #tmpR r where r.Ukey = std.Ukey)" + Environment.NewLine;
                if (model.ArriveWHDate1.HasValue)
                {
                    where1 += $"and st.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                    where2 += $"and st.IssueDate between @Date1 and @Date2" + Environment.NewLine;
                }

                if (!model.SP1.Empty())
                {
                    where1 += $"and std.ToPOID between @SP1 and @SP2" + Environment.NewLine;
                    where2 += $"and std.ToPOID between @SP1 and @SP2" + Environment.NewLine;
                }

                if (!model.WK1.Empty())
                {
                    where1 += $"and st.ID between @TID1 and @TID2" + Environment.NewLine;
                    where2 += $"and st.ID between @TID1 and @TID2" + Environment.NewLine; // Trandfer In 沒有 WK#
                }

                if (!model.Brand.Empty())
                {
                    where1 += $"and o.BrandID = @BrandID" + Environment.NewLine;
                    where2 += $"and o.BrandID = @BrandID" + Environment.NewLine;
                }

                // 基本資料
                sqlcmd = string.Format(this.B2A_sqlcmd, "Receiving_Detail", "Receiving", "R", where1, "a.ExportId", "inner", "WhseArrival") +
                    string.Format(this.B2A_sqlcmd, "TransferIn_Detail", "TransferIn", "T", where2, "''", "left", "IssueDate");

                // 要撈哪些
                switch (model.Inspection)
                {
                    case "Physical":
                        sqlcmd += this.B2A_Select(model, "Physical");
                        break;
                    case "Weight":
                        sqlcmd += this.B2A_Select(model, "Weight");
                        break;
                    case "Shade Band":
                        sqlcmd += this.B2A_Select(model, "ShadeBond");
                        break;
                    case "Continuity":
                        sqlcmd += this.B2A_Select(model, "Continuity");
                        break;
                    case "Odor":
                        sqlcmd += this.B2A_Select(model, "Odor");
                        break;
                    default:
                        sqlcmd += this.B2A_Select(model, "Physical") + this.B2A_Select(model, "Weight") + this.B2A_Select(model, "ShadeBond") + this.B2A_Select(model, "Continuity") + this.B2A_Select(model, "Odor");
                        break;
                }
            }

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlcmd, listPar, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }

        private string AddInspectionWhere(QA_R12_ViewModel model, string srcWhere, string inspectionType)
        {
            string returnResult = srcWhere;

            if (model.InspectionDate1.HasValue)
            {
                if (model.ByWKSeq)
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

            switch (model.InspectionResult)
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

        private string AddJoinByReportType(QA_R12_ViewModel model, string fromType, string inspectionTypeTable, string inspectionType = "")
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

            if (model.ByWKSeq)
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

        private string B2A_Select(QA_R12_ViewModel model, string inspectionType)
        {
            string columnSource = string.Empty;
            string column1 = string.Empty;
            string column2 = string.Empty;
            string column3 = string.Empty;
            string column4 = string.Empty;
            string column5 = string.Empty;
            string column6 = string.Empty;
            string groupColumn = string.Empty;
            string groupColumn2 = string.Empty;
            string joinStr1 = string.Empty;
            string where = this.AddInspectionWhere(model, string.Empty, inspectionType);
            if (inspectionType == "Physical")
            {
                if (model.ByWKSeq)
                {
                    column5 = "[Inspection_Percent] = CONCAT(convert(decimal(20,2),ROUND(isnull(fp.ttlActualYds, 0) / f.ArriveQty * 100.0, 2)), '%'),";
                    groupColumn2 = ",fp.ttlActualYds";
                    joinStr1 = "outer apply(select ttlActualYds = sum(ActualYds) from FIR_Physical fp where fp.id = f.FirID ) fp";
                }

                columnSource = $@"
outer apply(
    select Dyelot = count(1)
    from(
        select distinct std.FromDyelot
        from SubTransfer_Detail	std
		inner join FIR_Physical i with (nolock) on i.Roll = std.FromRoll and i.Dyelot = std.FromDyelot and i.Result <> ''
        where i.ID = f.FirID and std.id = f.TransferID
		and std.ToPOID = f.ToPOID and std.ToSeq1 = f.ToSeq1 and std.ToSeq2 = f.ToSeq2
		and std.FromPOID = f.FromPOID and std.FromSeq1 = f.FromSeq1 and std.FromSeq2 = f.FromSeq2 
    )x
)ct3
{joinStr1}

";
                column1 = "ct3.Dyelot,[NotInspectedDyelot]= f.ctDyelot - ct3.Dyelot,";
                groupColumn = ",ct3.Dyelot,f.ctDyelot";
            }

            if (model.ByWKSeq)
            {
                if (inspectionType == "Physical")
                {
                    column2 = "f.TotalInspYds,";
                    groupColumn += ",f.TotalInspYds";
                }

                if (inspectionType == "ShadeBond")
                {
                    column6 = ",f.ctTone";
                    groupColumn += ",f.ctTone";
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

            return string.Format(this.B2A_select, "R", inspectionType, string.Format(columnSource, "Receiving"), column1, column2, column3, column4, groupColumn, where, column5, groupColumn2, column6) +
                "\r\nunion all\r\n" +
                string.Format(this.B2A_select, "T", inspectionType, string.Format(columnSource, "TransferIn"), column1, column2, column3, column4, groupColumn, where, column5, groupColumn2, column6);
        }
    }
}
