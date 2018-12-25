﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R08
    /// </summary>
    public partial class R08 : Sci.Win.Tems.PrintForm
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

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.label10.Text = "** The value in this report are all excluded subcon-out,\r\n unless the column with \"included subcon-out\".";
            DataTable factory, mDivision;
            DBProxy.Current.Select(
                null,
                string.Format(@"select '' as FtyGroup 
union all
select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup"),
                out factory);

            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Sewing Line,CPU/Sewer/HR");
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.comboOrderBy.SelectedIndex = 0;
            this.comboM.Text = Sci.Env.User.Keyword;

        }

        // Date
        private void DateDateStart_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateDateStart.Value))
            {
                this.dateDateEnd.Value = null;
            }
            else
            {
                this.dateDateStart.Value = Convert.ToDateTime(this.dateDateStart.Value).GetFirstDayOfMonth();
                this.dateDateEnd.Value = Convert.ToDateTime(this.dateDateStart.Value).AddMonths(11).GetLastDayOfMonth();
            }
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}", MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" where FactoryID = '{0}'", this.comboFactory.Text));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "3", line, false, ",");
            item.Width = 300;
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
            if (MyUtility.Check.Empty(this.dateDateStart.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
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
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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
        , o.SubconInSisterFty
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
where s.OutputDate between '{0}' and '{1}' and (o.CateGory != 'G' or s.Category='M')",
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
       , SubconInSisterFty
       , [SubconOutFty] = isnull(SubconOutFty,'')
INTO #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType
		 , OrderCategory, LocalOrder, FactoryID, OrderProgram, MockupProgram
		 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
		 , MockupStyle, Rate, StdTMS,SubconInSisterFty,isnull(SubconOutFty,'')

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
       , SubconInSisterFty
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
		  or (LastShift = 'I' and SubconInSisterFty = 1)
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
		      or (LastShift = 'I' and SubconInSisterFty = 1)
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
	where LastShift = 'I' and SubconInSisterFty = 0
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
	where LastShift = 'I' and SubconInSisterFty = 1
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
),
tmpAllSubprocess as(
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category != 'G'
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
            --排除 subcon in non sister的數值
          and ((a.LastShift <> 'I') or ( a.LastShift = 'I' and a.SubconInSisterFty <> 0 ))           
          and ot.Price > 0 		    
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
),
tmpAllSubprocess as(
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
           , a.Program 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category != 'G'
--	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
			and ot.Price > 0 		    
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
),
tmpAllSubprocess as(
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
           , a.SubconOutFty 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category != 'G'
--	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
			and ot.Price > 0 		    
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
	   , SumA = SUM(m.ActiveManpower) over() 
from Factory f
left join Manpower m on m.FactoryID = f.ID 
		  				and m.Year = {0} 
		  				and m.Month = {1}
where f.Junk = 0",
                this.date1.Value.Year,
                this.date1.Value.Month));

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

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            Microsoft.Office.Interop.Excel.Range rngToInsert;
            Microsoft.Office.Interop.Excel.Range rngBorders;
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Sewing_R08_Factory_Yearly_CMP_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            // excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format("{0}", this.factoryName);
            worksheet.Cells[3, 1] = $"All Factory Yearly CMP Report, Date:{Convert.ToDateTime(this.dateDateStart.Value).ToString("yyyy/MM")}-{Convert.ToDateTime(this.dateDateEnd.Value).ToString("yyyy/MM")}";

            worksheet.Cells[4, 3] = "Total CPU Included Subcon-In";

            int insertRow = 5;
            object[,] objArray = new object[1, 11];
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
                objArray[0, 10] = string.Empty;
                worksheet.Range[string.Format("A{0}:K{0}", insertRow)].Value2 = objArray;
                insertRow++;

                // 插入一筆Record
                rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
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
            insertRow = insertRow + 2;
            worksheet.Cells[insertRow, 3] = "Total CPU Included Subcon-In";
            insertRow++;
            if (this.cpuFactor.Rows.Count > 2)
            {
                // 插入Record
                for (int i = 0; i < this.cpuFactor.Rows.Count - 2; i++)
                {
                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
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
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            this.DeleteExcelRow(2, insertRow, excel);

            // insertRow

            // Borders.LineStyle 儲存格邊框線
            // Microsoft.Office.Interop.Excel.Range excelRange
            //    = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing);
            // excelRange.Borders.LineStyle = 3;
            // excelRange.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle
            //    = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            // Subprocess
            insertRow = insertRow + 2;
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
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            insertRow = insertRow + 3;
            decimal ttlm = MyUtility.Convert.GetDecimal(this.printData.Compute("sum(ManPower)", string.Empty));
            decimal c = MyUtility.Convert.GetDecimal(this.printData.Compute("sum(TotalCPU)", string.Empty)) / MyUtility.Convert.GetDecimal(this.printData.Compute("sum(ManHour)", string.Empty));
            worksheet.Cells[insertRow, 1] = "VPH";

            // [VPH]:(Total Total Manhours / Total work day * Total CPU/Sewer/HR)-->四捨五入到小數點後兩位 ，再除[Factory active ManPower])-->四捨五入到小數點後兩位。
            c = Sci.MyUtility.Math.Round(c, 2);

            // WorkDay =>
            int intWorkDay;
            DataTable dtWorkDay;
            string strWorkDay = @"
select Distinct OutputDate
from #tmp
where LastShift <> 'O'";
            DualResult failResult = MyUtility.Tool.ProcessWithDatatable(this.SewOutPutData, null, strWorkDay, out dtWorkDay);
            if (failResult == false)
            {
                MyUtility.Msg.WarningBox(failResult.ToString());
                intWorkDay = 0;
            }
            else
            {
                intWorkDay = dtWorkDay.Rows.Count;
            }

            worksheet.Cells[insertRow, 3] = Sci.MyUtility.Math.Round(Sci.MyUtility.Math.Round(ttlm * c / intWorkDay, 2) / MyUtility.Convert.GetDecimal(this.vphData.Rows[0]["SumA"]), 2);

            worksheet.Cells[insertRow, 6] = "Factory active ManPower:";
            worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetInt(this.vphData.Rows[0]["SumA"]);
            worksheet.Cells[insertRow, 9] = "/Total work day:";
            worksheet.Cells[insertRow, 11] = intWorkDay;

            // Subcon
            int RevenueStartRow = 0;
            insertRow = insertRow + 2;
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
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);

                        #region Sub-Process Total Revenue for Company Subcon-In
                        insertRec = 0;
                        if (this.subprocessSubconInData.AsEnumerable().Where(s => s["Company"].Equals(dr["Company"])).Any())
                        {
                            insertRow++;
                            RevenueStartRow = insertRow;

                            // title
                            worksheet.Cells[insertRow, 1] = "Sub-Process Total Revenue";
                            worksheet.Cells[insertRow, 9] = "(Unit:US$)";

                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
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
                                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                    Marshal.ReleaseComObject(rngToInsert);
                                }
                            }

                            // 畫框線
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{1}", MyUtility.Convert.GetString(RevenueStartRow), MyUtility.Convert.GetString(insertRow)), Type.Missing);
                            rngBorders.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());     // 給單元格加邊框
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(RevenueStartRow)), Type.Missing);
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                            // 插入一筆Record
                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
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
                                insertRow = insertRow + 3;
                            }
                            else
                            {
                                insertRow = insertRow + 5;
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
                            RevenueStartRow = insertRow;

                            // title
                            worksheet.Cells[insertRow, 1] = "Sub-Process Total Revenue";
                            worksheet.Cells[insertRow, 9] = "(Unit:US$)";

                            rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
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
                                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                    Marshal.ReleaseComObject(rngToInsert);
                                }
                            }

                            // 畫框線
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{1}", MyUtility.Convert.GetString(RevenueStartRow), MyUtility.Convert.GetString(insertRow)), Type.Missing);
                            rngBorders.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());     // 給單元格加邊框
                            rngBorders = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(RevenueStartRow)), Type.Missing);
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                        }
                        #endregion

                        // 插入一筆Record
                        rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
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
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Sewing_R08_Factory_Yearly_CMP_Report");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void DeleteExcelRow(int rowCount, int rowLocation, Microsoft.Office.Interop.Excel.Application excel)
        {
            for (int i = 1; i <= rowCount; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[rowLocation];

                // rng.Select();
                rng.Delete();
            }
        }
    }
}
