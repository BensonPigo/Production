using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R08
    /// </summary>
    public partial class R08 : Win.Tems.PrintForm
    {
        private string line1;
        private string line2;
        private string factory;
        private string factoryName;
        private string mDivision;
        private DateTime? date1;
        private DateTime? date2;
        private int orderby;
        private DataTable[] dts;
        private DataTable SewOutPutData;
        private DataTable printData;
        private DataTable excludeInOutTotal;
        private DataTable NonSisterInTotal;
        private DataTable SisterInTotal;
        private DataTable cpuFactor;
        private DataTable subprocessData;
        private DataTable subprocessSubconInData;
        private DataTable subprocessSubconOutData;
        private DataTable subconData;
        private DataTable vphData;
        private List<APIData> dataMode = new List<APIData>();

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.label10.Text = "** The value in this report are all excluded subcon-out,\r\n unless the column with \"included subcon-out\".";
            DBProxy.Current.Select(
                null,
                string.Format(@"select '' as FtyGroup 
union all
select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup"),
                out DataTable factory);

            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out DataTable mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Sewing Line,CPU/Sewer/HR");
            this.comboFactory.Text = Env.User.Factory;
            this.comboOrderBy.SelectedIndex = 0;
            this.comboM.Text = Env.User.Keyword;
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}", MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" where FactoryID = '{0}'", this.comboFactory.Text));
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "3", line, false, ",")
            {
                Width = 300,
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return string.Empty;
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        // Sewing Line
        private void TxtSewingLineStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineStart.Text = this.SelectSewingLine(this.txtSewingLineStart.Text);
        }

        // Sewing Line
        private void TxtSewingLineEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineEnd.Text = this.SelectSewingLine(this.txtSewingLineEnd.Text);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDateStart.Value) || MyUtility.Check.Empty(this.dateDateEnd.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.comboM.Text))
            {
                MyUtility.Msg.WarningBox("M can't empty!!");
                return false;
            }

            this.date1 = this.dateDateStart.Value;
            this.date2 = this.dateDateEnd.Value;
            this.line1 = this.txtSewingLineStart.Text;
            this.line2 = this.txtSewingLineEnd.Text;
            this.factory = this.comboFactory.Text;
            this.mDivision = this.comboM.Text;
            this.orderby = this.comboOrderBy.SelectedIndex;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult failResult;
            #region 組撈全部Sewing output data SQL
            sqlCmd.Append(string.Format(
                @"
select  s.OutputDate
		, s.Category
		, s.Shift
		, s.SewingLineID
		, [ActManPower] = IIF(sd.QAQty = 0, s.Manpower, s.Manpower * sd.QAQty)
		, s.Team
		, sd.OrderId
		, sd.ComboType
		, sd.WorkHour
		, sd.QAQty
		, sd.InlineQty
		, [OrderCategory] = isnull(o.Category,'')
		, o.LocalOrder
		, s.FactoryID
		, [OrderProgram] = isnull(o.ProgramID,'') 
		, [MockupProgram] = isnull(mo.ProgramID,'')
		, [OrderCPU] = isnull(o.CPU,0)
		, [OrderCPUFactor] = isnull(o.CPUFactor,0)
		, [MockupCPU] = isnull(mo.Cpu,0)
		, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
		, [OrderStyle] = isnull(o.StyleID,'')
		, [MockupStyle] = isnull(mo.StyleID,'')
        , [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id ,sd.ComboType),100)/100
		, System.StdTMS
        , o.SubconInType
        , [SubconOutFty] = iif(sf.id is null,'Other',s.SubconOutFty)
INTO #tmpSewingDetail
from System,SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join SCIFty sf WITH (NOLOCK) on sf.ID = s.SubconOutFty
--left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--											 and sl.Location = sd.ComboType
left join factory f WITH (NOLOCK) on f.id=s.FactoryID
where s.OutputDate between '{0}' and '{1}' and (o.CateGory NOT IN ('G','A') or s.Category='M')",
                Convert.ToDateTime(this.date1).ToString("d"),
                Convert.ToDateTime(this.date2).ToString("d")));

            if (!MyUtility.Check.Empty(this.line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'", this.line1));
            }

            if (!MyUtility.Check.Empty(this.line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'", this.line2));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (this.checkSampleFty.Checked)
            {
                sqlCmd.Append(" and f.type <> 'S' ");
            }

            sqlCmd.Append(@"
select OutputDate,Category
	   , Shift
	   , SewingLineID
	   , ActManPower1 = Sum(ActManPower)
	   , Team
	   , OrderId
	   , ComboType
	   , WorkHour = ROUND(Sum(WorkHour),3)
	   , QAQty = sum(QAQty)
	   , InlineQty = sum(InlineQty)
	   , OrderCategory
	   , LocalOrder
	   , FactoryID
	   , OrderProgram
	   , MockupProgram
	   , OrderCPU
	   , OrderCPUFactor
	   , MockupCPU
	   , MockupCPUFactor
	   , OrderStyle
	   , MockupStyle
	   , Rate
	   , StdTMS
	   , IIF(Shift <> 'O' and Category <> 'M' and LocalOrder = 1, 'I',Shift) as LastShift
       , SubconInType
       , [SubconOutFty] = isnull(SubconOutFty,'')
INTO #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType
		 , OrderCategory, LocalOrder, FactoryID, OrderProgram, MockupProgram
		 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
		 , MockupStyle, Rate, StdTMS,SubconInType,isnull(SubconOutFty,'')

select t.*
	   , isnull(w.Holiday, 0) as Holiday
	   , IIF(isnull(QAQty, 0) = 0, ActManPower1, (ActManPower1 / QAQty)) as ActManPower
INTO #tmp1stFilter
from #tmpSewingGroup t
left join WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
									  and w.Date = t.OutputDate 
									  and w.SewingLineID = t.SewingLineID
where 1 = 1");

            sqlCmd.Append(@"
select * 
into #tmp2ndFilter
from #tmp1stFilter t
where 1 = 1");

            sqlCmd.Append(@"
select OutputDate
	   , Shift = IIF(LastShift = 'D', 'Day'
									, IIF(LastShift = 'N', 'Night'
														 , IIF(LastShift = 'O', 'Subcon-Out'
														 					  , 'Subcon-In')))
	   , Team
	   , SewingLineID
	   , OrderId
	   , Style = IIF(Category = 'M', MockupStyle, OrderStyle)
	   , QAQty
	   , ActManPower
	   , Program = IIF(Category = 'M',MockupProgram,OrderProgram)
	   , WorkHour
	   , StdTMS
	   , MockupCPU
	   , MockupCPUFactor
	   , OrderCPU
	   , OrderCPUFactor
	   , Rate
	   , Category
	   , LastShift
	   , ComboType
	   , FactoryID
       , SubconInType
       , SubconOutFty
into #tmp
from #tmp2ndFilter

select * from #tmp

--
;with AllOutputDate as (
    select distinct [OutputMM] = FORMAT(OutputDate,'yyyy/MM') 
	from #tmp
),
tmpQty as (
	select [OutputMM] = FORMAT(OutputDate,'yyyy/MM')
		   , StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
	from #tmp
	where LastShift <> 'O'
	group by FORMAT(OutputDate,'yyyy/MM'), StdTMS
),
tmpTtlCPU as (
	select [OutputMM] = FORMAT(OutputDate,'yyyy/MM')
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift <> 'O'
	group by FORMAT(OutputDate,'yyyy/MM')
),
tmpSubconInCPU as (
	select [OutputMM] = FORMAT(OutputDate,'yyyy/MM')
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift = 'I'
	group by FORMAT(OutputDate,'yyyy/MM')
),
tmpSubconOutCPU as (
	select [OutputMM] = FORMAT(OutputDate,'yyyy/MM')
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) , 3)
	from #tmp
	where LastShift = 'O'
	group by FORMAT(OutputDate,'yyyy/MM')
),
tmpTtlManPower as (
	select [OutputMM] = FORMAT(OutputDate,'yyyy/MM') 
		   , ManPower = Sum(a.Manpower) - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
	from (
		select OutputDate
			   , FactoryID
			   , SewingLineID
			   , LastShift
			   , Team
			   , ManPower = Max(ActManPower)
		from #tmp
		where LastShift <> 'O'
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team
	) a
	outer apply(
		select ManPower
		from (
			select OutputDate
				   , FactoryID
				   , SewingLineID
				   , LastShift
				   , Team
				   , ManPower = Max(ActManPower)
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team
		) m2
		where m2.LastShift = 'I' 
			  and m2.Team = a.Team 
			  and m2.SewingLineID = a.SewingLineID	
			  and a.OutputDate = m2.OutputDate
	) d
	group by FORMAT(OutputDate,'yyyy/MM')
)
select aDate.OutputMM
	   , QAQty = isnull (q.QAQty, 0)
	   , TotalCPU = isnull (tc.TotalCPU, 0)
	   , SInCPU = isnull (ic.TotalCPU, 0)
	   , SoutCPU = isnull (oc.TotalCPU, 0)
	   , CPUSewer = isnull (IIF(q.ManHour = 0, 0, isnull(tc.TotalCPU, 0) / q.ManHour), 0)
	   , AvgWorkHour = isnull (IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)), 0)
	   , ManPower = isnull (mp.ManPower, 0)
	   , ManHour = isnull (q.ManHour, 0)
	   , Eff = isnull (IIF(q.ManHour * q.StdTMS = 0, 0, Round(tc.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2)), 0)
from AllOutputDate aDate
left join tmpQty q on aDate.OutputMM = q.OutputMM
left join tmpTtlCPU tc on aDate.OutputMM = tc.OutputMM
left join tmpSubconInCPU ic on aDate.OutputMM = ic.OutputMM
left join tmpSubconOutCPU oc on aDate.OutputMM = oc.OutputMM
left join tmpTtlManPower mp on aDate.OutputMM = mp.OutputMM
order by aDate.OutputMM

--整理Total Exclude Subcon-In & Out
;with tmpQty as (
	select StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift <> 'O' 
          --排除Subcon in non sister資料
          and LastShift <> 'I'
		  or (LastShift = 'I' and SubconInType in ('1','2'))
	group by StdTMS
),
tmpTtlManPower as (
	select ManPower = Sum(Manpower)
	from (
		select OutputDate
			   , FactoryID
			   , SewingLineID
			   , LastShift
			   , Team
			   , ManPower = Max(ActManPower) 
		from #tmp
		where LastShift <> 'O' 
			  --排除Subcon in non sister資料
              and LastShift <> 'I'
		      or (LastShift = 'I' and SubconInType in ('1','2'))
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team
	) a
)
select q.QAQty
	   , q.TotalCPU
	   , CPUSewer = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , AvgWorkHour = IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2))
	   , q.ManHour
	   , Eff = IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2))
from tmpQty q
left join tmpTtlManPower mp on 1 = 1

--整理non Sister SubCon In
;with tmpQty as (
	select StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift = 'I' and SubconInType in ('0','3')
	group by StdTMS
)
select q.QAQty
	   , q.TotalCPU
	   , CPUSewer = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , q.ManHour
	   , Eff = IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2))
from tmpQty q

--整理Sister SubCon In
;with tmpQty as (
	select StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift = 'I' and SubconInType in ('1','2')
	group by StdTMS
)
select q.QAQty
	   , q.TotalCPU
	   , CPUSewer = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , q.ManHour
	   , Eff = IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2))
from tmpQty q

--整理CPU Factor
;with tmpData as (
	select CPUFactor = IIF(Category = 'M', MockupCPUFactor, OrderCPUFactor)
		   , QAQty
		   , CPU = QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)
		   , Style
	from #tmp
),
tmpSumQAQty as (
	select CPUFactor
		   , QAQty = sum(QAQty)
   	from tmpData 
   	group by CPUFactor
),
tmpSumCPU as (
	select CPUFactor
		   , CPU = sum(CPU)
   	from tmpData 
   	group by CPUFactor
),
tmpCountStyle as (
	select CPUFactor
		   , Style = COUNT(distinct Style)
   	from tmpData 
   	group by CPUFactor
)
select q.* 
	   , c.CPU
	   , s.Style
from tmpSumQAQty q
left join tmpSumCPU c on q.CPUFactor = c.CPUFactor
left join tmpCountStyle s on q.CPUFactor = s.CPUFactor

--準備台北資料(須排除這些)
select ps.ID
into #TPEtmp
from PO_Supp ps
inner join PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Fabric fb on psd.SCIRefno = fb.SCIRefno 
inner join MtlType ml on ml.id = fb.MtlTypeID
where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
and ml.isThread=1 
and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

--整理Subprocess資料
;with tmpArtwork as(
	Select ID
		   , rs = iif(ProductionUnit = 'TMS', 'CPU'
		   									, iif(ProductionUnit = 'QTY', 'AMT'
		   																, '')),
           [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
	from ArtworkType WITH (NOLOCK)
	where Classify in ('I','A','P') 
		  and IsTtlTMS = 0
          and IsPrintToCMP=1
),
tmpAllSubprocess as(
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
            --排除 subcon in non sister的數值
          and ((a.LastShift <> 'I') or ( a.LastShift = 'I' and a.SubconInType not in ('0','3') ))           
          and ot.Price > 0 		    
		  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
	group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price
)
select ArtworkTypeID = t1.ID
	   , Price = isnull(Sum(ROUND(Price,t1.DecimalNumber)), 0)
	   , rs
from tmpArtwork t1
left join tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs
order by t1.ID

--整理Subprocess by Company Subcon-In資料 Orders.program
;with tmpArtwork as(
	Select ID
		   , rs = iif(ProductionUnit = 'TMS', 'CPU'
		   									, iif(ProductionUnit = 'QTY', 'AMT'
		   																, '')),
           [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
	from ArtworkType WITH (NOLOCK)
	where Classify in ('I','A','P') 
		  and IsTtlTMS = 0
          and IsPrintToCMP=1
),
tmpAllSubprocess as(
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
           , a.Program 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
--	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
			and ot.Price > 0 		    
		  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
	group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,a.Program
)
select ArtworkTypeID = t1.ID
	   , Price = isnull(Sum(ROUND(Price,t1.DecimalNumber)), 0)
	   , rs
       , [Company] = t2.Program
from tmpArtwork t1
left join tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs,t2.Program having isnull(sum(Price), 0) > 0
order by t1.ID

--整理Subprocess by Company Subcon-Out資料 SewingOutput.SubconOutFty
;with tmpArtwork as(
	Select ID
		   , rs = iif(ProductionUnit = 'TMS', 'CPU'
		   									, iif(ProductionUnit = 'QTY', 'AMT'
		   																, '')),
           [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
	from ArtworkType WITH (NOLOCK)
	where Classify in ('I','A','P') 
		  and IsTtlTMS = 0
          and IsPrintToCMP=1
),
tmpAllSubprocess as(
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
           , a.SubconOutFty 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
--	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
			and ot.Price > 0 		    
		  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
	group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,a.SubconOutFty
)
select ArtworkTypeID = t1.ID
	   , Price = isnull(Sum(ROUND(Price,t1.DecimalNumber)), 0)
	   , rs
       , [Company] = t2.SubconOutFty
from tmpArtwork t1
left join tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs,t2.SubconOutFty having isnull(sum(Price), 0) > 0
order by t1.ID

--整理Subcon資料
;with tmpSubconIn as (
	Select 'I' as Type
		   , Company = Program 
		   , TtlCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift = 'I'
	group by Program
),
tmpSubconOut as (
    Select Type = 'O'
		   , Company = t.SubconOutFty
		   , TtlCPU = ROUND(Sum(t.QAQty*IIF(t.Category = 'M', t.MockupCPU * t.MockupCPUFactor, t.OrderCPU * t.OrderCPUFactor * t.Rate)),3)
	from #tmp t
	where LastShift = 'O'
	group by t.SubconOutFty
)
select * from (
select * from tmpSubconIn
union all
select * from tmpSubconOut ) as a 
order by Type,iif(Company = 'Other','Z','A'),Company


");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.dts);
            if (!result)
            {
                failResult = new DualResult(false, "Query sewing output data fail\r\n" + result.ToString());
                return failResult;
            }
            else
            {
                this.SewOutPutData = this.dts[0];
                this.printData = this.dts[1];
                this.excludeInOutTotal = this.dts[2];
                this.NonSisterInTotal = this.dts[3];
                this.SisterInTotal = this.dts[4];
                this.cpuFactor = this.dts[5];
                this.subprocessData = this.dts[6];
                this.subprocessSubconInData = this.dts[7];
                this.subprocessSubconOutData = this.dts[8];
                this.subconData = this.dts[9];
            }

            if (MyUtility.Check.Empty(this.factory) && !MyUtility.Check.Empty(this.mDivision))
            {
                this.factoryName = MyUtility.GetValue.Lookup(string.Format("select Name from Mdivision WITH (NOLOCK) where ID = '{0}'", this.mDivision));
            }
            else
            {
                this.factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", this.factory));
            }

            sqlCmd.Clear();
            sqlCmd.Append(string.Format(
                @"
select f.id
	   , m.ActiveManpower
	   , SumActiveManpower = SUM(m.ActiveManpower) over() 
from Factory f
left join Manpower m on m.FactoryID = f.ID 
		  				and m.Year = {0} 
		  				and m.Month = {1}
where f.Junk = 0",
                this.date1.Value.Year,
                this.date1.Value.Month));

            if (this.checkSampleFty.Checked)
            {
                sqlCmd.Append(" and f.type <> 'S' ");
            }

            if (this.factory != string.Empty)
            {
                sqlCmd.Append(string.Format(" and f.id= '{0}'", this.factory));
            }

            if (this.mDivision != string.Empty)
            {
                sqlCmd.Append(string.Format(" and f.MDivisionID = '{0}'", this.mDivision));
            }

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.vphData);
            if (!result)
            {
                failResult = new DualResult(false, "Query sewing output data fail\r\n" + result.ToString());
                return failResult;
            }

            if (this.factory != string.Empty)
            {
                foreach (DataRow dr in this.vphData.Rows)
                {
                    if (dr["ActiveManpower"].Empty())
                    {
                        failResult = new DualResult(false, string.Format("{0} has not been set ActiveManpower", dr["id"].ToString()));
                        return failResult;
                    }
                }
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            Excel.Range rngToInsert;
            Excel.Range rngBorders;

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\Sewing_R08_Factory_CMP_Report.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            // excel.Visible = true;
            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format("{0}", this.factoryName);
            worksheet.Cells[3, 1] = $"All Factory CMP Report, Date:{Convert.ToDateTime(this.dateDateStart.Value).ToString("yyyy/MM/dd")}-{Convert.ToDateTime(this.dateDateEnd.Value).ToString("yyyy/MM/dd")}";

            worksheet.Cells[4, 3] = "Total CPU Included Subcon-In";

            #region Direct Manpower(From PAMS)
            List<APIData> pams = new List<APIData>();
            if (!(Env.User.Keyword.EqualString("CM1") ||
                Env.User.Keyword.EqualString("CM2") ||
                Env.User.Keyword.EqualString("CM3")))
            {
                this.dataMode = new List<APIData>();
                GetApiData.GetAPIData(this.mDivision, this.factory, (DateTime)this.date1.Value, (DateTime)this.date2.Value, out this.dataMode);
                if (this.dataMode != null)
                {
                    var datelists = this.SewOutPutData.AsEnumerable().Select(s => ((DateTime)s["OutputDate"]).ToString("yyyy/MM/dd")).Distinct().ToList();
                    pams = this.dataMode.ToList().Where(w => datelists.Contains(w.Date.ToString("yyyy/MM/dd"))).GroupBy(g => new { g.Date.Year, g.Date.Month }).
                        Select(s => new APIData
                        {
                            yyyyMM = s.First().Date.ToString("yyyy/MM"),
                            SewTtlManpower = s.Sum(c => c.SewTtlManpower),
                            SewTtlManhours = s.Sum(c => c.SewTtlManhours),
                        }).ToList();
                }
            }
            #endregion

            int insertRow = 5;
            object[,] objArray = new object[1, 15];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr[0];
                objArray[0, 1] = dr[1];
                objArray[0, 2] = dr[2];
                objArray[0, 3] = dr[3];
                objArray[0, 4] = dr[4];
                objArray[0, 5] = dr[5];
                objArray[0, 6] = dr[6];
                objArray[0, 7] = dr[7];
                objArray[0, 8] = dr[8];
                objArray[0, 9] = string.Format("=IF(I{0}=0,0,ROUND((C{0}/(I{0}*3600/1400))*100,1))", insertRow);
                if (pams != null && pams.Where(w => w.yyyyMM.EqualString(dr["OutputMM"])).Count() > 0)
                {
                    objArray[0, 11] = pams.Where(w => w.yyyyMM.EqualString(dr["OutputMM"])).FirstOrDefault().SewTtlManpower;
                    objArray[0, 12] = pams.Where(w => w.yyyyMM.EqualString(dr["OutputMM"])).FirstOrDefault().SewTtlManhours;
                }
                else
                {
                    objArray[0, 11] = 0;
                    objArray[0, 12] = 0;
                }

                objArray[0, 10] = MyUtility.Convert.GetDouble(objArray[0, 11]) == 0 ? 0 : MyUtility.Convert.GetDouble(objArray[0, 12]) / MyUtility.Convert.GetDouble(objArray[0, 11]);
                objArray[0, 13] = string.Format("=IF(M{0}=0,0,ROUND((C{0}/(M{0}*3600/1400))*100,1))", insertRow);
                objArray[0, 14] = string.Empty;
                worksheet.Range[string.Format("A{0}:O{0}", insertRow)].Value2 = objArray;
                insertRow++;

                // 插入一筆Record
                rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            // 將多出來的Record刪除
            this.DeleteExcelRow(2, insertRow, excel);

            // Total
            worksheet.Cells[insertRow, 2] = string.Format("=SUM(B5:B{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 3] = string.Format("=SUM(C5:C{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 4] = string.Format("=SUM(D5:D{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 5] = string.Format("=SUM(E5:E{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 6] = string.Format("=ROUND(C{0}/I{0},2)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 7] = string.Format("=ROUND(I{0}/H{0},2)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 8] = string.Format("=SUM(H5:H{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 9] = string.Format("=SUM(I5:I{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 10] = string.Format("=ROUND(C{0}/(I{0}*60*60/1400)*100,1)", insertRow);
            worksheet.Cells[insertRow, 11] = string.Format("=ROUND(M{0}/L{0},2)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 12] = string.Format("=SUM(L5:L{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 13] = string.Format("=SUM(M5:M{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 14] = string.Format("=ROUND(C{0}/(M{0}*60*60/1400)*100,1)", insertRow);
            insertRow++;

            // Excluded non sister Subcon In
            worksheet.Cells[insertRow, 2] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, 3] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, 7] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["AvgWorkHour"]);
            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, 10] = (this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            insertRow++;

            // non sister Subcon In
            worksheet.Cells[insertRow, 2] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, 3] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, 10] = (this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            insertRow++;

            // sister Subcon In
            worksheet.Cells[insertRow, 2] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, 3] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, 10] = (this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);

            // CPU Factor
            insertRow += 2;
            worksheet.Cells[insertRow, 3] = "Total CPU Included Subcon-In";
            insertRow++;
            if (this.cpuFactor.Rows.Count > 2)
            {
                // 插入Record
                for (int i = 0; i < this.cpuFactor.Rows.Count - 2; i++)
                {
                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            objArray = new object[1, 4];
            foreach (DataRow dr in this.cpuFactor.Rows)
            {
                objArray[0, 0] = string.Format("CPU * {0}", MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["CPUFactor"]), 1)));
                objArray[0, 1] = dr["QAQty"];
                objArray[0, 2] = dr["CPU"];
                objArray[0, 3] = dr["Style"];

                worksheet.Range[string.Format("A{0}:D{0}", insertRow)].Value2 = objArray;
                insertRow++;
                rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            this.DeleteExcelRow(2, insertRow, excel);

            // Subprocess
            insertRow += 2;
            int insertRec = 0;
            foreach (DataRow dr in this.subprocessData.Rows)
            {
                insertRec++;
                if (insertRec % 2 == 1)
                {
                    worksheet.Cells[insertRow, 2] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["rs"]));
                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr["Price"]);
                }
                else
                {
                    worksheet.Cells[insertRow, 6] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["rs"]));
                    worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(dr["Price"]);
                    insertRow++;

                    // 插入一筆Record
                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            insertRow += 3;

            #region  中國工廠自抓/其它場Pams [Total Work Day]
            if (Env.User.Keyword.EqualString("CM1") ||
                Env.User.Keyword.EqualString("CM2") ||
                Env.User.Keyword.EqualString("CM3"))
            {
                int ttlWorkDay = 0;
                string strWorkDay = @"select Distinct OutputDate from #tmp where LastShift <> 'O'";
                DualResult failResult = MyUtility.Tool.ProcessWithDatatable(this.SewOutPutData, null, strWorkDay, out DataTable dtWorkDay);
                if (failResult == false)
                {
                    MyUtility.Msg.WarningBox(failResult.ToString());
                    ttlWorkDay = 0;
                }
                else
                {
                    ttlWorkDay = dtWorkDay.Rows.Count;
                }

                worksheet.Cells[insertRow, 1] = "Total work day:";
                worksheet.Cells[insertRow, 3] = ttlWorkDay;
            }
            else
            {
                if (pams != null)
                {
                    int ttlWorkDay = pams.Where(w => w.SewTtlManhours != 0).Count();
                    worksheet.Cells[insertRow, 1] = "Total work day:";
                    worksheet.Cells[insertRow, 3] = ttlWorkDay;
                }
            }
            #endregion

            // Subcon
            int revenueStartRow = 0;
            insertRow += 2;
            int insertSubconIn = 0, insertSubconOut = 0;
            objArray = new object[1, 3];
            if (this.subconData.Rows.Count > 0)
            {
                foreach (DataRow dr in this.subconData.Rows)
                {
                    if (MyUtility.Convert.GetString(dr["Type"]) == "I")
                    {
                        insertRow++;
                        insertSubconIn = 1;
                        objArray[0, 0] = dr["Company"];
                        objArray[0, 1] = string.Empty;
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[string.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        // 插入一筆Record
                        rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);

                        #region Sub-Process Total Revenue for Company Subcon-In
                        insertRec = 0;
                        if (this.subprocessSubconInData.AsEnumerable().Where(s => s["Company"].Equals(dr["Company"])).Any())
                        {
                            insertRow++;
                            revenueStartRow = insertRow;

                            // title
                            worksheet.Cells[insertRow, 1] = "Sub-Process Total Revenue";
                            worksheet.Cells[insertRow, 9] = "(Unit:US$)";

                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            Marshal.ReleaseComObject(rngToInsert);

                            insertRow++;
                            foreach (DataRow dr_sub in this.subprocessSubconInData.Select($" Company = '{dr["Company"]}'"))
                            {
                                insertRec++;
                                if (insertRec % 2 == 1)
                                {
                                    worksheet.Cells[insertRow, 2] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                }
                                else
                                {
                                    worksheet.Cells[insertRow, 6] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                    insertRow++;

                                    // 插入一筆Record
                                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                                    Marshal.ReleaseComObject(rngToInsert);
                                }
                            }

                            // 畫框線
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{1}", MyUtility.Convert.GetString(revenueStartRow), MyUtility.Convert.GetString(insertRow)), Type.Missing);
                            rngBorders.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());     // 給單元格加邊框
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(revenueStartRow)), Type.Missing);
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

                            // 插入一筆Record
                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            Marshal.ReleaseComObject(rngToInsert);
                        }
                        #endregion
                    }
                    else
                    {
                        if (insertSubconOut == 0)
                        {
                            if (insertSubconIn == 0)
                            {
                                this.DeleteExcelRow(2, insertRow, excel);
                                insertRow += 3;
                            }
                            else
                            {
                                insertRow += 5;
                            }
                        }

                        insertSubconOut = 1;
                        insertRow++;
                        objArray[0, 0] = dr["Company"].Equals("Other") ? "SUBCON-OUT TO OTHER COMPANIES" : dr["Company"];
                        objArray[0, 1] = string.Empty;
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[string.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        #region Sub-Process Total Revenue for Company Subcon-OUT
                        insertRec = 0;
                        if (this.subprocessSubconOutData.AsEnumerable().Where(s => s["Company"].Equals(dr["Company"])).Any())
                        {
                            insertRow++;
                            revenueStartRow = insertRow;

                            // title
                            worksheet.Cells[insertRow, 1] = "Sub-Process Total Revenue";
                            worksheet.Cells[insertRow, 9] = "(Unit:US$)";

                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            Marshal.ReleaseComObject(rngToInsert);

                            insertRow++;
                            foreach (DataRow dr_sub in this.subprocessSubconOutData.Select($" Company = '{dr["Company"]}'"))
                            {
                                insertRec++;
                                if (insertRec % 2 == 1)
                                {
                                    worksheet.Cells[insertRow, 2] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                }
                                else
                                {
                                    worksheet.Cells[insertRow, 6] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr_sub["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr_sub["rs"]));
                                    worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(dr_sub["Price"]);
                                    insertRow++;

                                    // 插入一筆Record
                                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                                    Marshal.ReleaseComObject(rngToInsert);
                                }
                            }

                            // 畫框線
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{1}", MyUtility.Convert.GetString(revenueStartRow), MyUtility.Convert.GetString(insertRow)), Type.Missing);
                            rngBorders.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());     // 給單元格加邊框
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(revenueStartRow)), Type.Missing);
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                        }
                        #endregion

                        // 插入一筆Record
                        rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }
                }

                if (insertSubconOut == 0)
                {
                    // 刪除資料
                    this.DeleteExcelRow(2, insertRow + 5, excel);
                }
            }
            else
            {
                this.DeleteExcelRow(9, insertRow, excel);
            }

            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Sewing_R08_Factory_CMP_Report");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void DeleteExcelRow(int rowCount, int rowLocation, Excel.Application excel)
        {
            for (int i = 1; i <= rowCount; i++)
            {
                Excel.Range rng = (Excel.Range)excel.Rows[rowLocation];

                // rng.Select();
                rng.Delete();
            }
        }
    }
}
