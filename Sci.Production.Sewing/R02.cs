using System;
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

namespace Sci.Production.Sewing
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        string line1, line2, factory, factoryName, mDivision;
        DateTime? date1, date2;
        int excludeHolday, excludeSubconin, reportType, orderby;
        DataTable SewOutPutData, printData, excludeInOutTotal, cpuFactor, subprocessData, subconData,vphData;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            label10.Text = "** The value in this report are all excluded \r\nsubcon-out, unless the column with \r\n\"included subcon-out\".";
            DataTable factory, mDivision;
            DBProxy.Current.Select(null, @"select '' as FtyGroup 
union all
select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);

            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            MyUtility.Tool.SetupCombox(comboHoliday, 1, 1, "Included,Excluded");
            MyUtility.Tool.SetupCombox(comboSubconIn, 1, 1, "Included,Excluded");
            MyUtility.Tool.SetupCombox(comboReportType, 1, 1, "By Date,By Sewing Line");
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(comboOrderBy, 1, 1, "Sewing Line,CPU/Sewer/HR");
            comboHoliday.SelectedIndex = 0;
            comboSubconIn.SelectedIndex = 0;
            comboReportType.SelectedIndex = 0;
            comboFactory.Text = Sci.Env.User.Factory;
            comboOrderBy.SelectedIndex = 0;
            comboM.Text = Sci.Env.User.Keyword;
        }

        //Date
        private void dateDateStart_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateDateStart.Value))
            {
                dateDateEnd.Value = null;
            }
            else
            {
                dateDateEnd.Value = (Convert.ToDateTime(dateDateStart.Value).AddDays(1 - Convert.ToDateTime(dateDateStart.Value).Day)).AddMonths(1).AddDays(-1);
            }
        }

        //Report Type
        private void comboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboReportType.SelectedIndex == 0)
            {
                labelOrderBy.Visible = false;
                comboOrderBy.Visible = false;
            }
            else
            {
                labelOrderBy.Visible = true;
                comboOrderBy.Visible = true;
            }
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}", MyUtility.Check.Empty(comboFactory.Text) ? "" : string.Format(" where FactoryID = '{0}'", comboFactory.Text));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "3", line, false, ",");
            item.Width = 300;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return "";
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        //Sewing Line
        private void txtSewingLineStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            txtSewingLineStart.Text = SelectSewingLine(txtSewingLineStart.Text);
        }

        //Sewing Line
        private void txtSewingLineEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            txtSewingLineEnd.Text = SelectSewingLine(txtSewingLineEnd.Text);
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateDateStart.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (comboHoliday.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Holiday can't empty!!");
                return false;
            }

            if (comboSubconIn.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Subcon-in can't empty!!");
                return false;
            }

            if (comboReportType.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Report type can't empty!!");
                return false;
            }

            if (comboReportType.SelectedIndex == 1)
            {
                if (comboFactory.SelectedIndex == -1 || comboFactory.SelectedIndex == 0)
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    return false;
                }

                if (comboOrderBy.SelectedIndex == -1)
                {
                    MyUtility.Msg.WarningBox("Order by can't empty!!");
                    return false;
                }
            }
            date1 = dateDateStart.Value;
            date2 = dateDateEnd.Value;
            line1 = txtSewingLineStart.Text;
            line2 = txtSewingLineEnd.Text;
            factory = comboFactory.Text;
            mDivision = comboM.Text;
            excludeHolday = comboHoliday.SelectedIndex;
            excludeSubconin = comboSubconIn.SelectedIndex;
            reportType = comboReportType.SelectedIndex;
            orderby = comboOrderBy.SelectedIndex;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult failResult;
            #region 組撈全部Sewing output data SQL
            sqlCmd.Append(string.Format(@"
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
		, [Rate] = isnull(sl.Rate,100)/100
		, System.StdTMS
INTO #tmpSewingDetail
from System,SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
											 and sl.Location = sd.ComboType
where s.OutputDate between '{0}' and '{1}' and (o.CateGory != 'G' or s.Category='M')"
                , Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
            if (!MyUtility.Check.Empty(line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'",line1));
            }
            if (!MyUtility.Check.Empty(line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'",line2));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'",factory));
            }
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", mDivision));
            }

            sqlCmd.Append(@"
select OutputDate,Category
	   , Shift
	   , SewingLineID
	   , ActManPower1 = Sum(ActManPower)
	   , Team
	   , OrderId
	   , ComboType
	   , WorkHour = sum(WorkHour)
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
INTO #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType
		 , OrderCategory, LocalOrder, FactoryID, OrderProgram, MockupProgram
		 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
		 , MockupStyle, Rate, StdTMS

select t.*
	   , isnull(w.Holiday, 0) as Holiday
	   , IIF(isnull(QAQty, 0) = 0, ActManPower1, (ActManPower1 / QAQty)) as ActManPower
INTO #tmp1stFilter
from #tmpSewingGroup t
left join WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
									  and w.Date = t.OutputDate 
									  and w.SewingLineID = t.SewingLineID
where 1 = 1");
            if (excludeSubconin == 1)
            {
                sqlCmd.Append(" and t.LastShift <> 'I'");
            }
            sqlCmd.Append(@"
select * 
into #tmp2ndFilter
from #tmp1stFilter t
where 1 = 1");
            if (excludeHolday == 1)
            {
                sqlCmd.Append(" and Holiday = 0");
            }
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
from #tmp2ndFilter");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out SewOutPutData);
            if (!result)
            {
                failResult = new DualResult(false, "Query sewing output data fail\r\n" + result.ToString());
                return failResult;
            }

            #region 整理列印資料
            if (reportType == 0)
            {
                try
                {
                    #region 組SQL
                    failResult = MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category"
                        , @"
;with AllOutputDate as (
    select distinct OutputDate		   
	from #tmp
),
tmpQty as (
	select OutputDate
		   , StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = Sum(ROUND(WorkHour * ActManPower, 2))
	from #tmp
	where LastShift <> 'O'
	group by OutputDate, StdTMS
),
tmpTtlCPU as (
	select OutputDate
		   , TotalCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2)) 
	from #tmp
	where LastShift <> 'O'
	group by OutputDate
),
tmpSubconInCPU as (
	select OutputDate
		   , TotalCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2))
	from #tmp
	where LastShift = 'I'
	group by OutputDate
),
tmpSubconOutCPU as (
	select OutputDate
		   , TotalCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2)) 
	from #tmp
	where LastShift = 'O'
	group by OutputDate
),
tmpTtlManPower as (
	select OutputDate
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
	group by OutputDate
)
select aDate.OutputDate
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
left join tmpQty q on aDate.OutputDate = q.OutputDate
left join tmpTtlCPU tc on aDate.OutputDate = tc.OutputDate
left join tmpSubconInCPU ic on aDate.OutputDate = ic.OutputDate
left join tmpSubconOutCPU oc on aDate.OutputDate = oc.OutputDate
left join tmpTtlManPower mp on aDate.OutputDate = mp.OutputDate
order by aDate.OutputDate", out printData);
                    if (failResult == false)
                    {
                        return failResult;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query print data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            else
            {
                try
                {
                    #region 組SQL
                    string sqlcommand = string.Format(@"
;with AllSewingLine as (
    select distinct SewingLineID		   
	from #tmp
),
tmpQty as (
	select SewingLineID
		   , StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = Sum(ROUND(WorkHour * ActManPower, 2))
	from #tmp
	where LastShift <> 'O'
	group by SewingLineID, StdTMS
),
tmpTtlCPU as (
	select SewingLineID
		   , TotalCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2))
	from #tmp
	where LastShift <> 'O'
	group by SewingLineID
),
tmpSubconInCPU as (
	select SewingLineID
		   , TotalCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2))
	from #tmp
	where LastShift = 'I'
	group by SewingLineID
),
tmpSubconOutCPU as (
	select SewingLineID
		   , TotalCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2))
	from #tmp
	where LastShift = 'O'
	group by SewingLineID
),
tmpTtlManPower as (
	select SewingLineID
		   , ManPower = Sum(Manpower) 
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
	group by SewingLineID
)
select aLine.SewingLineID
	   , QAQty = isnull (q.QAQty, 0)
 	   , TotalCPU = isnull (tc.TotalCPU, 0)
 	   , SInCPU = isnull(ic.TotalCPU,0)
 	   , SoutCPU = isnull(oc.TotalCPU,0)
 	   , CPUSewer = isnull (IIF(q.ManHour = 0, 0, isnull(tc.TotalCPU,0) / q.ManHour), 0)
 	   , AvgWorkHour = isnull (IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)), 0)
 	   , ManPower = isnull (mp.ManPower, 0)
 	   , ManHour = isnull (q.ManHour, 0)
 	   , Eff = isnull (IIF(q.ManHour * q.StdTMS = 0, 0, Round(tc.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2)), 0)
from AllSewingLine aLine
left join tmpQty q on aLine.SewingLineID = q.SewingLineID
left join tmpTtlCPU tc on aLine.SewingLineID = tc.SewingLineID
left join tmpSubconInCPU ic on aLine.SewingLineID = ic.SewingLineID
left join tmpSubconOutCPU oc on aLine.SewingLineID = oc.SewingLineID
left join tmpTtlManPower mp on aLine.SewingLineID = mp.SewingLineID
order by {0}", orderby == 0 ? "aLine.SewingLineID" : "CPUSewer");
                    failResult = MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category",
                        sqlcommand,  out printData);

                    if (failResult == false)
                    {
                        return failResult;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query print data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Total Exclude Subcon-In & Out
            try
            {
                #region 組SQL
                failResult = MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category",
                    @"
;with tmpQty as (
	select StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = Sum(ROUND(WorkHour * ActManPower, 2))
		   , TotalCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2))
	from #tmp
	where LastShift <> 'O' 
		  and LastShift <> 'I'
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
			  and LastShift <> 'I'
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team
	) a
)
select q.QAQty
	   , q.TotalCPU
	   , CPUSewer = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 2))
	   , AvgWorkHour = IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2))
	   , mp.ManPower
	   , q.ManHour
	   , Eff = IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2))
from tmpQty q
left join tmpTtlManPower mp on 1 = 1", out excludeInOutTotal);
                if (failResult == false)
                {
                    return failResult;
                }
                #endregion
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Query total data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion 

            #region 整理CPU Factor
            try
            {
                #region 組SQL
                failResult = MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "Category,MockupCPUFactor,OrderCPUFactor,QAQty,MockupCPU,OrderCPU,Rate,Style",
                    @"
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
left join tmpCountStyle s on q.CPUFactor = s.CPUFactor",
                    out cpuFactor);

                if (failResult == false)
                {
                    return failResult;
                }
                #endregion
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Query CPU factor data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理Subprocess資料
            if (printData.Rows.Count > 0)
            {
                try
                {
                    failResult = MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OrderId,ComboType,QAQty,LastShift",
@";with tmpArtwork as(
	Select ID
		   , rs = iif(ProductionUnit = 'TMS', 'CPU'
		   									, iif(ProductionUnit = 'QTY', 'AMT'
		   																, ''))
	from ArtworkType WITH (NOLOCK)
	where Classify in ('I','A','P') 
		  and IsTtlTMS = 0
),
tmpAllSubprocess as(
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
		   , Price = Round(sum(a.QAQty) * ot.Price * (isnull(sl.Rate, 100) / 100), 2) 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category != 'G'
	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O')) 
			and ot.Price > 0 		    
	group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price, sl.Rate
)
select ArtworkTypeID = t1.ID
	   , Price = isnull(sum(Price), 0)
	   , rs
from tmpArtwork t1
left join tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs
order by t1.ID",
                        out subprocessData);

                    if (failResult == false)
                    {
                        return failResult;
                    }
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subcon資料
            if (printData.Rows.Count > 0)
            {
                try
                {
                    failResult = MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "SewingLineID,QAQty,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,OrderId,Program,Category,FactoryID",
                        @"
;with tmpSubconIn as (
	Select 'I' as Type
		   , Company = Program 
		   , TtlCPU = Sum(ROUND(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 2))
		   , SewingLineID = ''
	from #tmp
	where LastShift = 'I'
	group by Program
),
tmpSubconOut as (
    Select Type = 'O'
		   , Company = s.Description
		   , TtlCPU = sum(ROUND(t.QAQty*IIF(t.Category = 'M', t.MockupCPU * t.MockupCPUFactor, t.OrderCPU * t.OrderCPUFactor * t.Rate),2))
		   , t.SewingLineID
	from #tmp t
	left join SewingLine s WITH (NOLOCK) on s.ID = t.SewingLineID 
											and s.FactoryID = t.FactoryID
	where LastShift = 'O'
	group by s.Description, t.SewingLineID
)
select * from tmpSubconIn
union all
select * from tmpSubconOut",
                        out subconData);

                    if (failResult == false)
                    {
                        return failResult;
                    }
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query subcon data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion
            factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", Sci.Env.User.Factory));

            sqlCmd.Clear();
            sqlCmd.Append(string.Format(@"
select f.id
	   , m.ActiveManpower
	   , SumA = SUM(m.ActiveManpower) over() 
from Factory f
left join Manpower m on m.FactoryID = f.ID 
		  				and m.Year = {0} 
		  				and m.Month = {1}
where f.Junk = 0", date1.Value.Year, date1.Value.Month));
            if (factory != "")
            {
                sqlCmd.Append(string.Format(" and f.id= '{0}'",factory));
            }
            if (mDivision!="")
            {
                sqlCmd.Append(string.Format(" and f.MDivisionID = '{0}'", mDivision));
            }
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out vphData);
            if (!result)
            {
                failResult = new DualResult(false, "Query sewing output data fail\r\n" + result.ToString());
                return failResult;
            }
            if (factory != "")
            {
                foreach (DataRow dr in vphData.Rows)
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

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + (reportType == 0 ? "\\Sewing_R02_MonthlyReportByDate.xltx" : "\\Sewing_R02_MonthlyReportBySewingLine.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            //excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format("Fm:{0}",factoryName);
            worksheet.Cells[3, 1] = string.Format("{0} Monthly CMP Report, MTH:{1} (Excluded Subcon-Out,{2}{3}) {4}"
                , MyUtility.Check.Empty(factory) ? "All Factory" : factory
                , Convert.ToDateTime(date1).ToString("yyyy/MM")
                , excludeHolday == 0 ? "Included Holiday" : "Excluded Holiday"
                , excludeSubconin == 0 ? ", Subcon-In" : ""
                , reportType == 0 ? "" : "By Sewing Line");
            worksheet.Cells[4, 3] = excludeSubconin == 0 ? "Total CPU Included Subcon-In" : "Total CPU";

            int insertRow = 5;
            object[,] objArray = new object[1, 11];
            foreach (DataRow dr in printData.Rows)
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
                objArray[0, 10] = "";
                worksheet.Range[String.Format("A{0}:K{0}", insertRow)].Value2 = objArray;
                insertRow++;
                //插入一筆Record
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }
            //將多出來的Record刪除
            DeleteExcelRow(2, insertRow, excel);

            //Total
            worksheet.Cells[insertRow, 2] = string.Format("=SUM(B5:B{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 3] = string.Format("=SUM(C5:C{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 4] = string.Format("=SUM(D5:D{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 5] = string.Format("=SUM(E5:E{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 6] = string.Format("=ROUND(C{0}/I{0},2)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 7] = string.Format("=ROUND(I{0}/H{0},2)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 8] = string.Format("=SUM(H5:H{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 9] = string.Format("=SUM(I5:I{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 10] = string.Format("=ROUND(C{0}/(I{0}*60*60/1400)*100,1)",insertRow);
            insertRow++;
            worksheet.Cells[insertRow, 2] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, 3] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, 7] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["AvgWorkHour"]);
            worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["ManPower"]);
            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, 10] = string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            
            //CPU Factor
            insertRow = insertRow + 2;
            worksheet.Cells[insertRow, 3] = excludeSubconin == 0 ? "Total CPU Included Subcon-In" : "Total CPU";
            insertRow++;
            if (cpuFactor.Rows.Count > 2)
            {
                //插入Record
                for (int i = 0; i < cpuFactor.Rows.Count - 2; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow+1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }
            objArray = new object[1, 4];
            foreach (DataRow dr in cpuFactor.Rows)
            {
                objArray[0, 0] = string.Format("CPU * {0}",MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["CPUFactor"]),1)));
                objArray[0, 1] = dr["QAQty"];
                objArray[0, 2] = dr["CPU"];
                objArray[0, 3] = dr["Style"];
                
                worksheet.Range[String.Format("A{0}:D{0}", insertRow)].Value2 = objArray;
                insertRow++;
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            DeleteExcelRow(2, insertRow, excel);
            //insertRow

            //Borders.LineStyle 儲存格邊框線
            //Microsoft.Office.Interop.Excel.Range excelRange 
            //    = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing);
            //excelRange.Borders.LineStyle = 3;
            //excelRange.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle 
            //    = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            //Subprocess
            insertRow = insertRow + 2;
            int insertRec = 0;
            foreach (DataRow dr in subprocessData.Rows)
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
                    //插入一筆Record
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }
            insertRow = insertRow + 3;
            decimal ttlm = MyUtility.Convert.GetDecimal(printData.Compute("sum(ManPower)", ""));
            decimal c = MyUtility.Convert.GetDecimal(printData.Compute("sum(TotalCPU)", "")) / MyUtility.Convert.GetDecimal(printData.Compute("sum(ManHour)", ""));
            worksheet.Cells[insertRow, 1] = "VPH";

            //[VPH]:(Total Total Manhours / Total work day * Total CPU/Sewer/HR)-->四捨五入到小數點後兩位 ，再除[Factory active ManPower])-->四捨五入到小數點後兩位。
            c = System.Math.Round(c, 2, MidpointRounding.AwayFromZero);
            // WorkDay => 
            int intWorkDay;
            DataTable dtWorkDay;
            string strWorkDay = @"
select Distinct OutputDate
from #tmp
where LastShift <> 'O'";
            DualResult failResult = MyUtility.Tool.ProcessWithDatatable(SewOutPutData, null, strWorkDay, out dtWorkDay);
            if (failResult == false)
            {
                MyUtility.Msg.WarningBox(failResult.ToString());
                intWorkDay = 0;
            }else
            {
                intWorkDay = dtWorkDay.Rows.Count;
            }
            decimal tempA = System.Math.Round(ttlm * c / intWorkDay, 2, MidpointRounding.AwayFromZero);
            worksheet.Cells[insertRow, 3] = System.Math.Round(tempA / MyUtility.Convert.GetDecimal(vphData.Rows[0]["SumA"]), 2, MidpointRounding.AwayFromZero);

            worksheet.Cells[insertRow, 6] = "Factory active ManPower:";
            worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetInt(vphData.Rows[0]["SumA"]);
            worksheet.Cells[insertRow, 9] = "/Total work day:";
            worksheet.Cells[insertRow, 11] = intWorkDay;
            //Subcon
            insertRow = insertRow + 2;
            int insertSubconIn = 0, insertSubconOut = 0;
            objArray = new object[1, 3];
            if (subconData.Rows.Count > 0)
            {
                foreach (DataRow dr in subconData.Rows)
                {
                    if (MyUtility.Convert.GetString(dr["Type"]) == "I")
                    {
                        insertRow++;
                        insertSubconIn = 1;
                        objArray[0, 0] = dr["Company"];
                        objArray[0, 1] = "";
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[String.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        //插入一筆Record
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow+1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }
                    else
                    {
                        if (insertSubconOut == 0)
                        {
                            if (insertSubconIn == 0)
                            {
                                //刪除資料
                                DeleteExcelRow(5, insertRow, excel);
                            }
                            else
                            {
                                //刪除資料
                                DeleteExcelRow(2, insertRow+1, excel);
                                insertRow = insertRow + 3;
                            }
                        }

                        insertSubconOut = 1;
                        insertRow++;
                        objArray[0, 0] = dr["Company"];
                        objArray[0, 1] = "";
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[String.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        //插入一筆Record
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);        
                    }

                }
                if (insertSubconOut == 0)
                {
                    //刪除資料
                    DeleteExcelRow(2, insertRow + 1, excel);
                    insertRow = insertRow + 3;
                    DeleteExcelRow(4, insertRow, excel);
                }
                else
                {
                    //刪除資料
                    DeleteExcelRow(2, insertRow + 1, excel);
                }
            }
            else
            {
                DeleteExcelRow(9, insertRow, excel);
            }

            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName((reportType == 0 ? "Sewing_R02_MonthlyReportByDate" : "Sewing_R02_MonthlyReportBySewingLine"));
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void DeleteExcelRow(int rowCount, int rowLocation,Microsoft.Office.Interop.Excel.Application excel)
        {
            for (int i = 1; i <= rowCount; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[rowLocation];
                //rng.Select();
                rng.Delete();
            }
        }
    }
}
