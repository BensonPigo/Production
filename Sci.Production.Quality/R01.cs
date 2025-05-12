using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R01 : Win.Tems.PrintForm
    {
        private DateTime? DateLastStart; private DateTime? DateLastEnd;
        private DateTime? DateArrStart;  private DateTime? DateArrEnd;
        private DateTime? DateSCIStart;  private DateTime? DateSCIEnd;
        private DateTime? DateSewStart;  private DateTime? DateSewEnd;
        private DateTime? DateEstStart;  private DateTime? DateEstEnd;
        private string spStrat; private string spEnd; private string Sea; private string Brand; private string Ref; private string Category; private string Supp; private string Over;
        private string wkStrat;
        private string wkEnd;
        private List<SqlParameter> lis;
        private DataTable dt;
        private string cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="R01"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboOverallResultStatus.SelectedIndex = 0;

            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            bool date_Last_Empty = !this.dateLastPhysicalInspDate.HasValue, date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue, textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty();
            bool textBox_WK_Empty = this.txtWK_start.Text.Empty(), textBox_WK2_Empty = this.txtWK_end.Text.Empty();

            if (date_Last_Empty && date_Arrive_Empty && date_SCI_Empty && textBox_SP_Empty && textBox_SP2_Empty && date_Sewing_Empty && date_Est_Empty && textBox_WK_Empty && textBox_WK2_Empty)
            {
                this.dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Last Inspection Date' or 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#' or 'WK#' at least one field entry");
                return false;
            }

            this.DateLastStart = this.dateLastPhysicalInspDate.Value1;
            this.DateLastEnd = this.dateLastPhysicalInspDate.Value2;
            this.DateArrStart = this.dateArriveWHDate.Value1;
            this.DateArrEnd = this.dateArriveWHDate.Value2;
            this.DateSCIStart = this.dateSCIDelivery.Value1;
            this.DateSCIEnd = this.dateSCIDelivery.Value2;
            this.DateSewStart = this.dateSewingInLineDate.Value1;
            this.DateSewEnd = this.dateSewingInLineDate.Value2;
            this.DateEstStart = this.dateEstCuttingDate.Value1;
            this.DateEstEnd = this.dateEstCuttingDate.Value2;
            this.spStrat = this.txtSPStart.Text.ToString();
            this.spEnd = this.txtSPEnd.Text.ToString();
            this.wkStrat = this.txtWK_start.Text.ToString();
            this.wkEnd = this.txtWK_end.Text.ToString();
            this.Sea = this.txtSeason.Text;
            this.Brand = this.txtBrand.Text;
            this.Ref = this.txtRefno.Text.ToString();
            this.Category = this.comboCategory.Text;
            this.Supp = this.txtsupplier.TextBox1.Text;
            this.Over = this.comboOverallResultStatus.SelectedItem.ToString();

            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty, rWhere = string.Empty, oWhere = string.Empty;

            string sqlTotalYardageArrDate = string.Empty;
            string sqlActTotalYdsArrDate = string.Empty;

            if (!this.dateArriveWHDate.Value1.HasValue && !this.dateArriveWHDate.Value2.HasValue)
            {
                sqlTotalYardageArrDate = $@" select Val = 0.0 ";
                sqlActTotalYdsArrDate = $@" select Val = 0.0 ";
            }
            else
            {
                sqlTotalYardageArrDate = $@"
select Val = Sum(ISNULL(fi.InQty,0))
from FtyInventory fi
inner join Receiving_Detail rd on rd.PoId = fi.POID and rd.Seq1 = fi.Seq1 and rd.Seq2 = fi.Seq2 AND fi.StockType=rd.StockType and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
inner join Receiving r on r.Id=rd.Id
where fi.POID = f.POID AND fi.Seq1 = f.Seq1 AND fi.Seq2 = f.Seq2 AND rd.Id=f.ReceivingID AND rd.ForInspection=1 ";

                sqlActTotalYdsArrDate = $@"
select ActualYds = Sum(fp.ActualYds) 
from FIR_Physical fp 
where fp.ID = f.ID and EXISTS(
	select 1
	from  Receiving r  
	where r.Id=f.ReceivingID
	WhseArrival_1
    WhseArrival_2
)
";
            }

            List<string> sqlWheres = new List<string>();
            List<string> rWheres = new List<string>();
            List<string> oWheres = new List<string>();
            #region --組WHERE--
            if (!this.dateLastPhysicalInspDate.Value1.Empty())
            {
                sqlWheres.Add("F.PhysicalDate >= @LastDate1");
                this.lis.Add(new SqlParameter("@LastDate1", this.DateLastStart));
            }

            if (!this.dateLastPhysicalInspDate.Value2.Empty())
            {
                sqlWheres.Add("F.PhysicalDate <= @LastDate2");
                this.lis.Add(new SqlParameter("@LastDate2", this.DateLastEnd));
            }

            if (!this.dateArriveWHDate.Value1.Empty())
            {
                rWheres.Add("rd.WhseArrival >= @ArrDate1");
                sqlTotalYardageArrDate += " AND r.WhseArrival >= @ArrDate1";
                sqlActTotalYdsArrDate = sqlActTotalYdsArrDate.Replace($@"WhseArrival_1", "AND r.WhseArrival >= @ArrDate1");
                this.lis.Add(new SqlParameter("@ArrDate1", this.DateArrStart));
            }
            else
            {
                sqlActTotalYdsArrDate.Replace("WhseArrival_1", string.Empty);
            }

            if (!this.dateArriveWHDate.Value2.Empty())
            {
                rWheres.Add("rd.WhseArrival <= @ArrDate2");
                sqlTotalYardageArrDate += " AND r.WhseArrival <= @ArrDate2";
                sqlActTotalYdsArrDate = sqlActTotalYdsArrDate.Replace($@"WhseArrival_2", "AND r.WhseArrival <= @ArrDate2");
                this.lis.Add(new SqlParameter("@ArrDate2", this.DateArrEnd));
            }
            else
            {
                sqlActTotalYdsArrDate.Replace("WhseArrival_2", string.Empty);
            }

            if (!this.dateSCIDelivery.Value1.Empty())
            {
                oWheres.Add("O.SciDelivery >= @SCIDate1");
                this.lis.Add(new SqlParameter("@SCIDate1", this.DateSCIStart));
            }

            if (!this.dateSCIDelivery.Value2.Empty())
            {
                oWheres.Add("O.SciDelivery <= @SCIDate2");
                this.lis.Add(new SqlParameter("@SCIDate2", this.DateSCIEnd));
            }

            if (!this.dateSewingInLineDate.Value1.Empty())
            {
                oWheres.Add("O.SewInLine >= @SewDate1");
                this.lis.Add(new SqlParameter("@SewDate1", this.DateSewStart));
            }

            if (!this.dateSewingInLineDate.Value2.Empty())
            {
                oWheres.Add("O.SewInLine <= @SewDate2");
                this.lis.Add(new SqlParameter("@SewDate2", this.DateSewEnd));
            }

            if (!this.dateEstCuttingDate.Value1.Empty())
            {
                oWheres.Add("O.CutInLine >= @Est1");
                this.lis.Add(new SqlParameter("@Est1", this.DateEstStart));
            }

            if (!this.dateEstCuttingDate.Value2.Empty())
            {
                oWheres.Add("O.CutInLine <= @Est2");
                this.lis.Add(new SqlParameter("@Est2", this.DateEstEnd));
            }

            if (!this.txtSPStart.Text.Empty())
            {
                oWheres.Add("O.Id between @sp1 and @sp2");
                this.lis.Add(new SqlParameter("@sp1", this.spStrat));
                this.lis.Add(new SqlParameter("@sp2", this.spEnd));
            }

            if (!MyUtility.Check.Empty(this.wkStrat))
            {
                rWheres.Add("rd.ExportId between @wk1 and @wk2");
                this.lis.Add(new SqlParameter("@wk1", this.wkStrat));
                this.lis.Add(new SqlParameter("@wk2", this.wkEnd));
            }

            if (!this.txtSeason.Text.Empty())
            {
                oWheres.Add("O.SeasonID = @Sea");
                this.lis.Add(new SqlParameter("@Sea", this.Sea));
            }

            if (!this.txtBrand.Text.Empty())
            {
                oWheres.Add("O.BrandID = @Brand");
                this.lis.Add(new SqlParameter("@Brand", this.Brand));
            }

            if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("P.Refno = @Ref");
                this.lis.Add(new SqlParameter("@Ref", this.Ref));
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                if (this.Category != string.Empty)
                {
                    oWheres.Add($"O.Category in ({this.comboCategory.SelectedValue})");
                }
            }

            if (!this.txtsupplier.TextBox1.Text.Empty())
            {
                sqlWheres.Add("SP.SuppId = @Supp");
                this.lis.Add(new SqlParameter("@Supp", this.Supp));
            }

            if (this.comboOverallResultStatus.Text == "Pass")
            {
                sqlWheres.Add("f.result = 'Pass'");
            }

            if (this.comboOverallResultStatus.Text == "Fail")
            {
                sqlWheres.Add("f.result = 'Fail'");
            }

            if (this.comboOverallResultStatus.Text == "Empty Result")
            {
                sqlWheres.Add("f.result  = '' ");
            }

            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            oWhere = string.Join(" and ", oWheres);
            rWhere = string.Join(" and ", rWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            if (!rWhere.Empty())
            {
                rWhere = " where " + rWhere;
            }

            if (!oWhere.Empty())
            {
                oWhere = " where " + oWhere;
            }
            #region --撈ListExcel資料--

            this.cmd = $@"
SET ARITHABORT ON

select 
    [BalanceQty]=sum(fit.inqty - fit.outqty + fit.adjustqty - fit.ReturnQty) 
    ,rd.poid
    ,rd.seq1
    ,rd.seq2
    ,RD.ID
INTO #balanceTmp
from dbo.View_AllReceivingDetail rd
inner join FIR f on f.POID=rd.poid AND  f.ReceivingID = rd.id AND f.seq1 = rd.seq1 and f.seq2 = rd.Seq2
inner join FtyInventory fit on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2 AND fit.StockType=rd.StockType and fit.Roll=rd.Roll and fit.Dyelot=rd.Dyelot
where 1=1 
    {rWhere.Replace("where", "AND")}
    {sqlWhere.Replace("where", "AND").Replace("SP.", "f.").Replace("P.", "f.")}
    GROUP BY rd.poid,rd.seq1,rd.seq2,RD.ID
 


select  
	F.POID
	,(F.SEQ1+'-'+F.SEQ2)SEQ
	,O.factoryid
	,O.BrandID
	,O.StyleID
	,O.SeasonID
	,t.ExportId
	,t.InvNo
	,t.WhseArrival
	,[StockQty1] = t.StockQty
    ,[InvStock] = t.InvStock
    ,[BulkStock] = t.BulkStock
	,[BalanceQty] = IIF(BalanceQty.BalanceQty=0,NULL,BalanceQty.BalanceQty)
	,[TotalRollsCalculated] = t.TotalRollsCalculated
    ,mp.ALocation
    ,LT.BulkLocationDate
	,mp.BLocation	
	,ILT.InvLocationDate
	,[MinSciDelivery] = (SELECT MinSciDelivery FROM  DBO.GetSCI(F.Poid,O.Category))
	,[MinBuyerDelivery] = (SELECT MinBuyerDelivery  FROM  DBO.GetSCI(F.Poid,O.Category))
	,F.Refno
    ,C.Description
    ,[ColorID] = ps.SpecValue
    ,[ColorName] = color.Name
    ,[SupplierCode] = SP.SuppID
    ,[SupplierName] = s.AbbEN
	,C.WeaveTypeID
    ,[InspectionGroup] = (Select InspectionGroup from Fabric where SCIRefno = p.SCIRefno)
	,[N/A Physical] = IIF(F.Nonphysical = 1,'Y',' ')
	,F.Result
	,[Cut Shadeband Qty (Roll)] = Qty.Roll
	,[Cut Shadeband] = Shadeband.sCount
	,F.Physical
	,[PhysicalInspector] = (select name from Pass1 where id = f.PhysicalInspector)
	,F.PhysicalDate
	,TotalYardage = TotalYardage.Val
    ,TotalYardageArrDate = {(!this.dateArriveWHDate.Value1.HasValue && !this.dateArriveWHDate.Value2.HasValue ? "NULL" : "TotalYardageArrDate.Val - ActTotalYdsArrDate.ActualYds")}
	,ActTotalRollsInspection.Cnt ActTotalRollsInspection
    ,fta.ActualYds
	,[InspectionRate] = ROUND(iif(t.StockQty = 0,0,CAST (fta.ActualYds/t.StockQty AS FLOAT)) ,3)
    ,TotalLotNumber
    ,InspectedLotNumber
	,ftp.TotalPoint
    ,F.CustInspNumber
	,F.Weight
	,[WeightInspector] = (select name from Pass1 where id = f.WeightInspector)
	,F.WeightDate
	,F.ShadeBond
	,[ShadeboneInspector] = (select name from Pass1 where id = f.ShadeboneInspector)
	,F.ShadeBondDate
	,[Shade_Band_Pass] =  [Shade_Band_Pass].cnt
	,[Shade_Band_Fail] =  [Shade_Band_Fail].cnt
	,F.Continuity
	,[ContinuityInspector] = (select name from Pass1 where id = f.ContinuityInspector)
	,F.ContinuityDate
	,F.Odor
	,[OdorInspector] = (select name from Pass1 where id = f.OdorInspector)
	,F.OdorDate
    ,F.Moisture
    ,F.MoistureDate
	,[Result2] = L.Result
	,[N/A Crocking] = IIF(L.nonCrocking=1,'Y',' ')
	,LC.Crocking
	,fl.CrockingInspector
	,LC.CrockingDate
	,[N/A Heat Shrinkage] = IIF(L.nonHeat=1,'Y',' ')
	,LH.Heat
	,fl.HeatInspector
	,LH.HeatDate
	,[N/A Wash Shrinkage] = IIF(L.nonWash=1,'Y',' ' )
	,LW.Wash
	,fl.WashInspector
	,LW.WashDate
	,RESULT3 = V.Result
	,[OvenInspector] = v2.Name
    ,[OvenDate] = V3.InspDate
	,RESULT4 = CFD.Result
	,[CFInspector] = cfd2.Name
    ,[CFDate] = CFD3.InspDate
	,ps1.LocalMR
    ,[Category] = ddl.Name
	,[Cutting Date] = o.CutInLine
    ,[OrderType] = O.OrderTypeID
    ,[TotalYardsBC] = isnull(fptbc.TicketYds, 0)
    ,[TotalPointBC] = isnull(fptbc.TotalPoint, 0)
    ,[TotalPointA] = isnull(fpta.TotalPoint, 0)
	,[C_Grade_TOP3Defects] = isnull(CGradT3.Value,'')
	,[A_Grade_TOP3Defects] = isnull(AGradT3.Value,'')
    ,[CutTime] = Qty.CutTime
    ,[MCHandle_id] = COALESCE(pass1_MCHandle.id, TPEPass1_MCHandle.id)
    ,[MCHandle_name] = COALESCE(pass1_MCHandle.name, TPEPass1_MCHandle.name)
    ,[MCHandle_extno] = COALESCE(pass1_MCHandle.extno, TPEPass1_MCHandle.extno)
    ,[KPI LETA] = O.KPILETA
    ,[ACT ETA] = Export.Eta
    ,[Packages(B/L No.)] = isnull(e.Packages,0)
    ,fl.ReceiveSampleDate
into #tmpFinal
from dbo.FIR F WITH (NOLOCK) 
cross apply(
	select rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2
	,[StockQty] = sum(RD.StockQty)
	,[InvStock] = iif(rd.StockType = 'I', sum(RD.StockQty), 0)
	,[BulkStock] = iif(rd.StockType = 'B', sum(RD.StockQty), 0)
    ,TotalRollsCalculated = count(1)
	from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
	where rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Id=F.ReceivingID
    {rWhere.Replace("where", "AND")}
    group by rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2,rd.StockType
) t
inner join (
    select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,id ,CutInLine, o.OrderTypeID,MCHandle,o.KPILETA
    from dbo.Orders o WITH (NOLOCK)  
    {oWhere}
) O on O.id = F.POID
left join pass1 pass1_MCHandle with(nolock) on pass1_MCHandle.id = O.MCHandle
left join TPEPass1 TPEPass1_MCHandle with(nolock) on TPEPass1_MCHandle.id = O.MCHandle
left join DropDownList ddl with(nolock) on o.Category = ddl.ID and ddl.Type = 'Category'
inner join dbo.PO_Supp SP WITH (NOLOCK) on SP.id = F.POID and SP.SEQ1 = F.SEQ1
inner join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
left join dbo.PO_Supp_Detail_Spec ps WITH (NOLOCK) on P.ID = ps.id and P.SEQ1 = ps.SEQ1 and P.SEQ2 = ps.SEQ2 and ps.SpecColumnID='Color'
inner join supp s WITH (NOLOCK) on s.id = SP.SuppID 
LEFT JOIN #balanceTmp BalanceQty ON BalanceQty.poid = f.POID and BalanceQty.seq1 = f.seq1 and BalanceQty.seq2 =f.seq2 AND BalanceQty.ID = f.ReceivingID
left join MDivisionPoDetail mp on mp.POID=f.POID and mp.Seq1=f.SEQ1 and mp.Seq2=f.SEQ2
left join Receiving on f.ReceivingID = Receiving.ID
left join Export on Receiving.ExportId = Export.ID
outer apply(
		select [Packages] = sum(e.Packages)
		from Export e with (nolock)
		where exists(
			select 1
			from export e2 with(nolock)
			where e2.Blno = e.Blno
			and Receiving.ExportId = e2.ID
		)
	)e
OUTER APPLY(
	SELECT * FROM  Fabric C WITH (NOLOCK) WHERE C.SCIRefno = F.SCIRefno
)C
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1		
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)L
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
		AND L.CrockingEncode=1 
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)LC
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1 
		AND L.HeatEncode=1 
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)LH
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
		AND L.WashEncode=1 
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)LW
OUTER APPLY(
select Result = Stuff((
		select concat(',',Result)
		from (
			select distinct od.Result 
			from dbo.Oven ov WITH (NOLOCK) 
			inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
			left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
			where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
			and ov.Status='Confirmed'
		) s
		for xml path ('')
	) , 1, 1, '')
)V
OUTER APPLY(
select [name ]= Stuff((
		select concat(',',name)
		from (
			select distinct pass1.name 
			from dbo.Oven ov WITH (NOLOCK) 
			inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
			left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
			where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
			and ov.Status='Confirmed'
		) s
		for xml path ('')
	) , 1, 1, '')
)V2
OUTER APPLY(
    SELECT InspDate = Stuff((
		    SELECT CONCAT(',', InspDate)
		    FROM (
			    SELECT DISTINCT InspDate = FORMAT(ov.InspDate, 'yyyy/MM/dd')
			    FROM dbo.Oven ov WITH (NOLOCK)
			    INNER JOIN Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
			    WHERE ov.POID = F.POID
                AND od.Seq1 = F.Seq1
                AND od.Seq2 = F.Seq2
			    AND ov.Status = 'Confirmed'
		    ) s
		    FOR XML PATH ('')
	    ), 1, 1, '')
)V3
OUTER APPLY(
select Result = Stuff((
		select concat(',',Result)
		from (
			select distinct cd.Result
			from dbo.ColorFastness CF WITH (NOLOCK) 
			inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
			left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
			where CF.Status = 'Confirmed' 
			and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
		) s
		for xml path ('')
	) , 1, 1, '')
)CFD
OUTER APPLY(
select Name = Stuff((
		select concat(',',Name)
		from (
			select distinct Pass1.Name
			from dbo.ColorFastness CF WITH (NOLOCK) 
			inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
			left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
			where CF.Status = 'Confirmed' 
			and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
		) s
		for xml path ('')
	) , 1, 1, '')
)CFD2
OUTER APPLY(
    SELECT InspDate = Stuff((
		    SELECT CONCAT(',',InspDate)
		    FROM (
			    SELECT DISTINCT InspDate = FORMAT(cf.InspDate, 'yyyy/MM/dd')
			    FROM ColorFastness cf WITH (NOLOCK) 
			    INNER JOIN ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = cf.ID
			    WHERE cf.POID = F.POID
                AND cd.SEQ1 = F.Seq1
                AND cd.seq2 = F.Seq2
                AND cf.Status = 'Confirmed'
		    ) s
		    FOR XML PATH ('')
	    ), 1, 1, '')
)CFD3
Outer apply(
	select (A.id+' - '+ A.name + ' #'+A.extno) LocalMR 
    from orders od 
    inner join pass1 a on a.id=od.LocalMR 
    where od.id=o.POID
) ps1
outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id=f.id) ftp
outer apply(
	select Val = Sum(ISNULL(fi.InQty,0))
	from FtyInventory fi
	inner join Receiving_Detail rd on rd.PoId = fi.POID and rd.Seq1 = fi.Seq1 and rd.Seq2 = fi.Seq2 AND fi.StockType=rd.StockType and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
	where fi.POID = f.POID AND fi.Seq1 = f.Seq1 AND fi.Seq2 = f.Seq2 AND rd.Id=f.ReceivingID AND rd.ForInspection=1
) TotalYardage
outer apply(
    {sqlTotalYardageArrDate}
) TotalYardageArrDate
outer apply(select ActualYds = Sum(fp.ActualYds) from FIR_Physical fp where fp.id=f.id) fta
outer apply(
    {sqlActTotalYdsArrDate}
) ActTotalYdsArrDate
outer apply(select TicketYds = Sum(fp.TicketYds), TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id = f.id and (fp.Grade = 'B' or fp.Grade = 'C')) fptbc
outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id = f.id and fp.Grade = 'A') fpta
outer apply(select count(1) Cnt from FIR_Physical fp where fp.id = f.id) ActTotalRollsInspection
outer apply(select  CrockingInspector = (select name from Pass1 where id = CrockingInspector)
	,HeatInspector = (select name from Pass1 where id = HeatInspector)
	,WashInspector = (select name from Pass1 where id = WashInspector)
    ,ReceiveSampleDate
	from FIR_Laboratory where Id=f.ID
)FL
outer apply
(
	SELECT Min(a.EditDate) BulkLocationDate
	FROM LocationTrans a WITH (NOLOCK) 
	inner join LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
	WHERE a.status = 'Confirmed' and b.stocktype='B'
	AND b.Poid=f.POID and b.Seq1=f.SEQ1 and b.Seq2=f.SEQ2
)LT
outer apply
(
	SELECT Min(a.EditDate) InvLocationDate
	FROM LocationTrans a WITH (NOLOCK) 
	inner join LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
	WHERE a.status = 'Confirmed' and b.stocktype='I'
	AND b.Poid=f.POID and b.Seq1=f.SEQ1 and b.Seq2=f.SEQ2
)ILT
outer apply
(
	select Roll = count(Roll+Dyelot),CutTime=max(fs.CutTime) from FIR_Shadebone fs where fs.ID =F.ID and fs.CutTime is not null
)
Qty 
outer apply
(
	select
		[sCount] = iif(isnull(s.Roll,0) = 0 , 0, cast(Qty.Roll as float) / cast(s.Roll as float)) 
	from
	(
		select Roll = count(Roll+Dyelot) 
		from FIR_Shadebone fs
		where fs.ID = f.ID 
	) s
) Shadeband
outer apply(
	select cnt = count(1) 
	from FIR_Shadebone t
	where UPPER(Result) = UPPER('Pass')
	and t.ID = f.ID
) [Shade_Band_Pass]
outer apply(
	select cnt = count(1) 
	from FIR_Shadebone t
	where UPPER(Result) = UPPER('Fail')
	and t.ID = f.ID
) [Shade_Band_Fail]
OUTER APPLY(
    SELECT Name 
    FROM Color c WITH (NOLOCK)
    where c.BrandId = O.BrandID 
    and c.ID = ps.SpecValue
)color
OUTER APPLY(
    SELECT TotalLotNumber = COUNT(1)
    FROM(
        SELECT DISTINCT rd.Dyelot
        FROM View_AllReceivingDetail rd WITH (NOLOCK)
        WHERE rd.Id = F.ReceivingID 
        AND rd.PoId = F.POID
        AND rd.Seq1 = F.SEQ1
        AND rd.Seq2 = F.SEQ2 
    )TotalLotNumber
)TotalLotNumber
OUTER APPLY(
    SELECT InspectedLotNumber = COUNT(1)
    FROM(
        SELECT DISTINCT rd.Dyelot
        FROM View_AllReceivingDetail rd WITH (NOLOCK)
	    INNER JOIN FIR f2 WITH (NOLOCK) on f2.ReceivingID = rd.Id and f2.POID = rd.PoId and f2.SEQ1 = rd.Seq1 and f2.SEQ2 = rd.Seq2
	    INNER JOIN FIR_Physical fp WITH (NOLOCK) on f.id = fp.ID and fp.Roll = rd.Roll and fp.Dyelot = fp.Dyelot
        WHERE rd.Id = F.ReceivingID 
        AND rd.PoId = F.POID
        AND rd.Seq1 = F.SEQ1
        AND rd.Seq2 = F.SEQ2 
    )InspectedLotNumber
)InspectedLotNumber
OUTER APPLY(
	select [Value]= Stuff((
		select concat(',',defect)
		from (
			select TOP 3 defect = concat(fd.DescriptionEN,'(',count(1),')'),count(1) as Qty
			from FIR
			INNER JOIN FIR_Physical fp ON FIR.ID= FP.ID
			inner join FIR_Physical_Defect_Realtime fpd on fp.DetailUkey = fpd.FIR_PhysicalDetailUKey
			inner join FabricDefect fd on fd.ID=fpd.FabricdefectID
			where 1=1
			and fp.Grade='C'
			AND FIR.ID = F.ID 
			group by fd.DescriptionEN
			order by Qty desc, fd.DescriptionEN asc
		) s
		for xml path ('')
	) , 1, 1, '')
)CGradT3
OUTER APPLY(
	select [Value]= Stuff((
		select concat(',',defect)
		from (
			select TOP 3 defect = concat(fd.DescriptionEN,'(',count(1),')'),count(1) as Qty
			from FIR
			INNER JOIN FIR_Physical fp ON FIR.ID= FP.ID
			inner join FIR_Physical_Defect_Realtime fpd on fp.DetailUkey = fpd.FIR_PhysicalDetailUKey
			inner join FabricDefect fd on fd.ID=fpd.FabricdefectID
			where 1=1
			and fp.Grade='A'
			AND FIR.ID=F.ID
			group by fd.DescriptionEN
			order by Qty desc, fd.DescriptionEN asc
		) s
		for xml path ('')
	) , 1, 1, '')
)AGradT3


{sqlWhere} 
ORDER BY POID,SEQ
OPTION (OPTIMIZE FOR UNKNOWN)


select
    tf.Category
    ,tf.POID
	,tf.SEQ
	,tf.factoryid
	,tf.BrandID
	,tf.StyleID
	,tf.SeasonID
	,tf.ExportId
	,tf.InvNo
    ,tf.[Cutting Date]
    ,tf.[KPI LETA]
    ,tf.[ACT ETA]
    ,tf.[Packages(B/L No.)]
	,tf.WhseArrival
    ,tf.ReceiveSampleDate
	,tf.StockQty1
    ,tf.InvStock
    ,tf.BulkStock
	,tf.BalanceQty
	,tf.TotalRollsCalculated
    ,tf.ALocation
    ,tf.BulkLocationDate
	,tf.BLocation	
    ,tf.InvLocationDate
	,tf.MinSciDelivery
	,tf.MinBuyerDelivery
	,tf.Refno
    ,tf.Description
    ,tf.ColorID
    ,tf.ColorName
    ,tf.SupplierCode
    ,tf.SupplierName
	,tf.WeaveTypeID
    ,tf.[InspectionGroup]
	,tf.[N/A Physical]
	,tf.Result
	,tf.Physical
    ,tf.TotalYardsBC
    ,tf.TotalPointBC
	,tf.C_Grade_TOP3Defects
    ,tf.TotalPointA
	,tf.A_Grade_TOP3Defects
	,tf.[PhysicalInspector]
	,tf.PhysicalDate
	,tf.TotalYardage
	,tf.TotalYardageArrDate
    ,tf.ActTotalRollsInspection
	,tf.ActualYds
    ,tf.InspectionRate
    ,tf.TotalLotNumber
    ,tf.InspectedLotNumber
	,tf.TotalPoint
    ,tf.CustInspNumber
	,tf.Weight
	,tf.WeightInspector
	,tf.WeightDate
	,tf.[Cut Shadeband Qty (Roll)]
	,tf.[Cut Shadeband]
    ,tf.CutTime
	,tf.ShadeBond
	,tf.ShadeboneInspector
	,tf.ShadeBondDate
	,tf.[Shade_Band_Pass]
	,tf.[Shade_Band_Fail]
	,tf.Continuity
	,tf.ContinuityInspector
	,tf.ContinuityDate
	,tf.Odor
	,tf.OdorInspector
	,tf.OdorDate
    ,tf.Moisture
    ,tf.MoistureDate
	,tf.Result2
	,tf.[N/A Crocking]
	,tf.Crocking
	,tf.CrockingInspector
	,tf.CrockingDate
	,tf.[N/A Heat Shrinkage]
	,tf.Heat
	,tf.HeatInspector
	,tf.HeatDate
	,tf.[N/A Wash Shrinkage]
	,tf.Wash
	,tf.WashInspector
	,tf.WashDate
	,tf.RESULT3
	,tf.OvenInspector
    ,tf.OvenDate
	,tf.RESULT4
	,tf.CFInspector
    ,tf.CFDate
    ,tf.LocalMR
    ,[MCHandle] = tf.MCHandle_id + '-' + tf.MCHandle_name + '#' + tf.MCHandle_extno
    ,tf.[OrderType]    
from #tmpFinal tf
ORDER BY POID,SEQ

";
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            int defaultTimeout = DBProxy.Current.DefaultTimeout;
            DBProxy.Current.DefaultTimeout = 1800;
            res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
            DBProxy.Current.DefaultTimeout = defaultTimeout;
            if (!res)
            {
                return res;
            }

            return res;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            _ = Utility.Excel.MyExcelPrg.GetSaveFileDialog(Utility.Excel.MyExcelPrg.Filter_Excel);

            Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R01.xltx", keepApp: true);
            Utility.Excel.SaveXltReportCls.XltRptTable dt1 = new Utility.Excel.SaveXltReportCls.XltRptTable(this.dt);
            string d1 = MyUtility.Check.Empty(this.DateArrStart) ? string.Empty : Convert.ToDateTime(this.DateArrStart).ToString("yyyy/MM/dd");
            string d2 = MyUtility.Check.Empty(this.DateArrEnd) ? string.Empty : Convert.ToDateTime(this.DateArrEnd).ToString("yyyy/MM/dd");
            string d3 = MyUtility.Check.Empty(this.DateSCIStart) ? string.Empty : Convert.ToDateTime(this.DateSCIStart).ToString("yyyy/MM/dd");
            string d4 = MyUtility.Check.Empty(this.DateSCIEnd) ? string.Empty : Convert.ToDateTime(this.DateSCIEnd).ToString("yyyy/MM/dd");
            string d5 = MyUtility.Check.Empty(this.DateSewStart) ? string.Empty : Convert.ToDateTime(this.DateSewStart).ToString("yyyy/MM/dd");
            string d6 = MyUtility.Check.Empty(this.DateSewEnd) ? string.Empty : Convert.ToDateTime(this.DateSewEnd).ToString("yyyy/MM/dd");
            string d7 = MyUtility.Check.Empty(this.DateEstStart) ? string.Empty : Convert.ToDateTime(this.DateEstStart).ToString("yyyy/MM/dd");
            string d8 = MyUtility.Check.Empty(this.DateEstEnd) ? string.Empty : Convert.ToDateTime(this.DateEstEnd).ToString("yyyy/MM/dd");
            xl.DicDatas.Add("##Arr", d1 + "~" + d2);
            xl.DicDatas.Add("##SCI", d3 + "~" + d4);
            xl.DicDatas.Add("##Sew", d5 + "~" + d6);
            xl.DicDatas.Add("##Est", d7 + "~" + d8);
            xl.DicDatas.Add("##SP", this.spStrat + "~" + this.spEnd);
            xl.DicDatas.Add("##Sea", this.Sea);
            xl.DicDatas.Add("##Brand", this.Brand);
            xl.DicDatas.Add("##Ref", this.Ref);
            xl.DicDatas.Add("##Cate", this.Category);
            xl.DicDatas.Add("##supp", this.Supp);
            xl.DicDatas.Add("##Over", this.Over);
            xl.DicDatas.Add("##body", dt1);
            dt1.ShowHeader = false;

            xl.Save(Class.MicrosoftFile.GetName("Quality_R01"));
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
        }
    }
}
