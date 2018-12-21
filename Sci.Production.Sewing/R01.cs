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

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        private DateTime? _date;
        private string _factory;
        private string _team;
        private string _factoryName;
        private DataTable _printData;
        private DataTable _ttlData;
        private DataTable _subprocessData;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.dateDate.Value = DateTime.Today.AddDays(-1);
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.comboTeam.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (this.comboFactory.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Factory can't empty!!");
                return false;
            }

            this._date = this.dateDate.Value;
            this._factory = this.comboFactory.Text;
            this._team = this.comboTeam.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組撈Data SQL
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
		, [OrderCdCodeID] = isnull(o.CdCodeID,'')
		, [MockupCDCodeID] = isnull(mo.MockupID,'')
		, s.FactoryID
		, [OrderCPU] = isnull(o.CPU,0)
		, [OrderCPUFactor] = isnull(o.CPUFactor,0)
		, [MockupCPU] = isnull(mo.Cpu,0)
		, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
		, [OrderStyle] = isnull(o.StyleID,'')
		, [MockupStyle] = isnull(mo.StyleID,'')
		, [OrderSeason] = isnull(o.SeasonID,'')
		, [MockupSeason] = isnull(mo.SeasonID,'')
	    , [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id, sd.ComboType),100)/100
		, System.StdTMS
		, [InspectQty] = isnull(r.InspectQty,0)
		, [RejectQty] = isnull(r.RejectQty,0)
        , [SubconInSisterFty] = isnull(o.SubconInSisterFty,0)
into #tmpSewingDetail
from System,SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
--left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--														    and sl.Location = sd.ComboType
outer apply(
	select top 1 RejectQty
		   , InspectQty 
    from Rft r WITH (NOLOCK)  
    where r.OrderID = sd.OrderId 
    	  and r.CDate = s.OutputDate 
    	  and r.SewinglineID = s.SewingLineID 
		  and r.FactoryID = s.FactoryID 
		  and r.Shift = s.Shift 
		  and r.Team = s.Team
) as r
where s.OutputDate = '{0}'
	  and s.FactoryID = '{1}'
      and (o.CateGory != 'G' or s.Category='M')  ",
                Convert.ToDateTime(this._date).ToString("d"),
                this._factory));

            if (!MyUtility.Check.Empty(this._team))
            {
                sqlCmd.Append(string.Format(" and s.Team = '{0}'", this._team));
            }

            sqlCmd.Append(@"
select OutputDate
	   , Category
	   , Shift
	   , SewingLineID
	   , ActManPower = Round(Sum(ActManPower),2)
	   , Team
	   , OrderId
	   , ComboType
	   , WorkHour = Round(sum(WorkHour),3)
	   , QAQty = sum(QAQty) 
	   , InlineQty = sum(InlineQty) 
	   , OrderCategory
	   , LocalOrder
	   , OrderCdCodeID
	   , MockupCDCodeID
	   , FactoryID
	   , OrderCPU
	   , OrderCPUFactor
	   , MockupCPU
	   , MockupCPUFactor
	   , OrderStyle
	   , MockupStyle
	   , OrderSeason
	   , MockupSeason
	   , Rate
	   , StdTMS
	   , InspectQty
	   , RejectQty
       , SubconInSisterFty
into #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId
		 , ComboType, OrderCategory, LocalOrder, OrderCdCodeID
		 , MockupCDCodeID, FactoryID, OrderCPU, OrderCPUFactor
		 , MockupCPU, MockupCPUFactor, OrderStyle, MockupStyle
		 , OrderSeason, MockupSeason, Rate, StdTMS, InspectQty
		 , RejectQty, SubconInSisterFty
----↓計算累計天數 function table太慢直接寫在這
select distinct scOutputDate = s.OutputDate 
	   , style = IIF(t.Category <> 'M', OrderStyle, MockupStyle)
	   , t.SewingLineID
	   , t.FactoryID
	   , t.Shift
	   , t.Team
	   , t.OrderId
	   , t.ComboType
into #stmp
from #tmpSewingGroup t
inner join SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID 
										   and s.OutputDate between dateadd(day,-90,t.OutputDate) and  t.OutputDate 
										   and s.FactoryID = t.FactoryID
inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
where (o.StyleID = OrderStyle or mo.StyleID = MockupStyle) and (o.CateGory != 'G' or t.Category='M')   
order by style, s.OutputDate

select w.Hours
	   , w.Date
	   , style = IIF(t.Category <> 'M', OrderStyle, MockupStyle)
	   , t.SewingLineID
	   , t.FactoryID
	   , t.Shift
	   , t.Team
	   , t.OrderId
	   , t.ComboType
into #wtmp
from #tmpSewingGroup t
inner join  WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
										and w.SewingLineID = t.SewingLineID 
										and w.Date between dateadd(day,-90,t.OutputDate) and  t.OutputDate and isnull(w.Hours,0) != 0

select cumulate = IIF(Count(1)=0, 1, Count(1))
	   , s.style
	   , s.SewingLineID
	   , s.FactoryID
	   , s.Shift
	   , s.Team
	   , s.OrderId
	   , s.ComboType
into #cl
from #stmp s
where s.scOutputDate > isnull((select date = max(Date)
						from #wtmp w 
						left join #stmp s2 on s2.scOutputDate = w.Date 
											  and w.style = s2.style 
											  and w.SewingLineID = s2.SewingLineID 
											  and w.FactoryID = s2.FactoryID 
											  and w.Shift = s2.Shift 
											  and w.Team = s2.Team
											  and w.OrderId = s2.OrderId 
											  and w.ComboType = s2.ComboType
						where s2.scOutputDate is null
							  and w.style = s.style 
							  and w.SewingLineID = s.SewingLineID 
							  and w.FactoryID = s.FactoryID 
							  and w.Shift = s.Shift 
							  and w.Team = s.Team 
							  and w.OrderId = s.OrderId 
							  and w.ComboType = s.ComboType),'1900/01/01')
group by s.style, s.SewingLineID, s.FactoryID, s.Shift, s.Team
		 , s.OrderId, s.ComboType
-----↑計算累計天數
select t.*
	   , LastShift = CASE WHEN t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1 and t.SubconInSisterFty = 1 then 'I'
                          WHEN t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1 and t.SubconInSisterFty = 0 then 'IN'
                     ELSE t.Shift END
	   , FtyType = f.Type
	   , FtyCountry = f.CountryID
	   , CumulateDate = c.cumulate
into #tmp1stFilter
from #tmpSewingGroup t
left join #cl c on c.style = IIF(t.Category <> 'M', OrderStyle, MockupStyle) 
				   and c.SewingLineID = t.SewingLineID 
				   and c.FactoryID = t.FactoryID 
				   and c.Shift = t.Shift 
				   and c.Team = t.Team 
				   and c.OrderId = t.OrderId 
				   and c.ComboType = t.ComboType
left join Factory f WITH (NOLOCK) on t.FactoryID = f.ID
---↓最後組成
select Shift =    CASE    WHEN LastShift='D' then 'Day'
                          WHEN LastShift='N' then 'Night'
                          WHEN LastShift='O' then 'Subcon-Out'
                          WHEN LastShift='I' then 'Subcon-In(Sister)'
                          else 'Subcon-In(Non Sister)' end				
	   , Team
	   , SewingLineID
	   , OrderId
	   , Style = IIF(Category='M',MockupStyle,OrderStyle) 
	   , CDNo = IIF(Category = 'M', MockupCDCodeID, OrderCdCodeID) + '-' + ComboType
	   , ActManPower = IIF(SHIFT = 'O'
                            ,MAX(IIF(QAQty > 0, ActManPower / QAQty, ActManPower)) OVER (PARTITION BY SHIFT,Team,SewingLineID)
                            ,IIF(QAQty > 0, ActManPower / QAQty, ActManPower))
	   , WorkHour
	   , ManHour = IIF(QAQty > 0, ActManPower / QAQty, ActManPower) * WorkHour
	   , TargetCPU = ROUND(ROUND(IIF(QAQty > 0, ActManPower / QAQty, ActManPower) * WorkHour, 2) * 3600 / StdTMS, 2) 
	   , TMS = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * StdTMS
	   , CPUPrice = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)
	   , TargetQty = IIF(IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   									   , OrderCPU * OrderCPUFactor * Rate) > 0
	   					    , ROUND(ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   					    						   , ActManPower) * WorkHour, 2) * 3600 / StdTMS, 2) / IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   					    																							     , OrderCPU * OrderCPUFactor * Rate)
						    , 0) 
	   , QAQty
	   , TotalCPU = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * QAQty
	   , CPUSewer = IIF(ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   									   , ActManPower) * WorkHour, 2) > 0
   							     , ROUND((IIF(Category = 'M', MockupCPU * MockupCPUFactor
   							     						    , OrderCPU * OrderCPUFactor * Rate) * QAQty), 2) / ROUND(IIF(QAQty > 0, ActManPower / QAQty
   							     																								  , ActManPower) * WorkHour, 2)
     						     , 0) 
	   , EFF = ROUND(IIF(ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   										, ActManPower) * WorkHour, 2) > 0
	   						      , (ROUND(IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   						      							 , OrderCPU * OrderCPUFactor * Rate) * QAQty, 2) / (ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   						      							 																	   , ActManPower) * WorkHour, 2) * 3600 / StdTMS)) * 100, 0)
	   							  , 1) 
	   , RFT = IIF(InspectQty > 0, ROUND((InspectQty - RejectQty) / InspectQty * 100, 2), 0)
	   , CumulateDate
	   , InlineQty
	   , Diff = QAQty - InlineQty
	   , LastShift
	   , ComboType
from #tmp1stFilter
where 1 =1");

            sqlCmd.Append(@" order by LastShift,Team,SewingLineID,OrderId");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this._printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            #region 整理Total資料
            if (this._printData.Rows.Count > 0)
            {
                try
                {
                    DualResult resultTotal = MyUtility.Tool.ProcessWithDatatable(
                        this._printData,
                        "Shift,Team,SewingLineID,ActManPower,TMS,QAQty,RFT,LastShift",
                        string.Format(@"
;with SubMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower)
	from #tmp
	group by Shift, Team, SewingLineID
),
SubSummaryData as (
	select Shift
		   , Team
		   , TMS = sum(TMS * QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
	group by Shift, Team
),
SubTotal as (
	select s.Shift
		   , s.Team
		   , TMS = case 
						when s.QAQty = 0 then 0
						else (s.TMS/s.QAQty)
				   end
		   , s.RFT
		   , ActManPower = Round(sum(m.ActManPower),2)
	from SubSummaryData s 
	left join SubMaxActManpower m on s.Shift = m.Shift 
									 and s.Team = m.Team
	group by s.Shift, s.Team, s.RFT, s.TMS, s.QAQty
),
GrandIncludeInOutMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower) 
	from #tmp
	group by Shift, Team, SewingLineID
),
GrandIncludeInOutSummaryData as (
	select TMS = sum(TMS*QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
),
GenTotal1 as (
	select TMS = Case 
                    when s.QaQty = 0 then 0
                    else (s.TMS/s.QAQty)
                 end
		   , s.RFT
		   , ActManPower = sum(m.ActManPower) - sum(iif(shift = 'Subcon-In', 0, isnull(d.ActManPower,0))) 
	from GrandIncludeInOutSummaryData s
	left join GrandIncludeInOutMaxActManpower m on 1 = 1
	outer apply(
		select ActManPower
		from GrandIncludeInOutMaxActManpower m2
		where m2.Shift = 'Subcon-In' 
			  and m2.Team = m.Team 
			  and m2.SewingLineID = m.SewingLineID	
	) d
	group by s.TMS, s.QAQty, s.RFT
),
GrandExcludeOutMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower)
	from #tmp
	where LastShift <> 'O'
	group by Shift, Team, SewingLineID
),
GrandExcludeOutSummaryData as (
	select TMS = sum(TMS * QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
	where LastShift <> 'O'
),
GenTotal2 as (
	select TMS = case
                    when s.QaQty = 0 then 0
                    else (s.TMS / s.QAQty)
                 end
		   , s.RFT
		   , ActManPower = sum(m.ActManPower) - sum(iif(shift = 'Subcon-In', 0, isnull(d.ActManPower,0))) 
	from GrandExcludeOutSummaryData s
	left join GrandExcludeOutMaxActManpower m on 1 = 1
	outer apply(
		select ActManPower
		from GrandExcludeOutMaxActManpower m2
		where m2.Shift = 'Subcon-In' and m2.Team = m.Team 
									     and m2.SewingLineID = m.SewingLineID	
	) d
	group by s.TMS, s.QAQty, s.RFT
),
GrandExcludeInOutMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower)
	from #tmp
	where LastShift <> 'O' 
	and LastShift <> 'IN' 
	group by Shift, Team, SewingLineID
),
GrandExcludeInOutSummaryData as (
	select TMS = sum(TMS*QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
	where LastShift <> 'O'
	and LastShift <> 'IN' 
),
GenTotal3 as (
	select TMS = case 
                    when s.QaQty = 0 then 0
                    else (s.TMS/s.QAQty)
                 end
		   , s.RFT
		   , ActManPower = sum(m.ActManPower)
	from GrandExcludeInOutSummaryData s
	left join GrandExcludeInOutMaxActManpower m on 1 = 1
	group by s.TMS, s.QAQty, s.RFT
)
select Type = 'Sub'
	   , Sort = '1'
	   , * 
from SubTotal

union all
select Type = 'Grand'  
	   , Sort = '2' 
	   , Shift = '' 
	   , Team = ''
	   , TMS
	   , RFT
	   , ActManPower 
from GenTotal1

union all
select Type = 'Grand'
	   , Sort = '3'
	   , Shift = '' 
	   , Team = ''
	   , TMS
	   , RFT
	   , ActManPower 
from GenTotal2

union all
select Type = 'Grand'
	   , Sort = '4'
	   , Shift = ''
	   , Team = '' 
	   , TMS
	   , RFT
	   , ActManPower 
from GenTotal3"),
                        out this._ttlData);

                    if (resultTotal == false)
                    {
                        return resultTotal;
                    }
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query total data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subprocess資料
            if (this._printData.Rows.Count > 0)
            {
                try
                {
                    DualResult resultSubprocess = MyUtility.Tool.ProcessWithDatatable(
                        this._printData,
                        "OrderId,ComboType,QAQty,LastShift",
                        string.Format(@"
;with tmpArtwork as (
	Select  ID,
            [DecimalNumber] =case   when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
	from ArtworkType WITH (NOLOCK) 
	where Classify in ('I','A','P') 
	      and IsTtlTMS = 0
),
tmpAllSubprocess as (
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = Round(sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](o.id ,a.ComboType), 100) / 100), ta.DecimalNumber) 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category != 'G'
	inner join tmpArtwork ta on ta.ID = ot.ArtworkTypeID
--	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O')) 
          and o.LocalOrder <> 1
		  and ot.Price > 0         
    group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,[dbo].[GetOrderLocation_Rate](o.id ,a.ComboType),ta.DecimalNumber
)
select ArtworkTypeID
	   , Price = sum(Price)
	   , rs = iif(att.ProductionUnit = 'TMS','CPU',iif(att.ProductionUnit = 'QTY','AMT',''))
from tmpAllSubprocess t
left join ArtworkType att WITH (NOLOCK) on att.id = t.ArtworkTypeID
group by ArtworkTypeID,att.ProductionUnit
order by ArtworkTypeID"),
                        out this._subprocessData);

                    if (resultSubprocess == false)
                    {
                        return resultSubprocess;
                    }
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            this._factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", this._factory));
            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData.Rows.Count);

            if (this._printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Sewing_R01_DailyCMPReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 1] = this._factoryName;
            worksheet.Cells[2, 1] = string.Format("{0} Daily CMP Report, DD.{1} {2}", this._factory, Convert.ToDateTime(this._date).ToString("MM/dd"),  "(Included Subcon-IN)");

            object[,] objArray = new object[1, 19];
            string[] subTtlRowInOut = new string[8];
            string[] subTtlRowExOut = new string[8];
            string[] subTtlRowExInOut = new string[8];

            string shift = MyUtility.Convert.GetString(this._printData.Rows[0]["Shift"]);
            string team = MyUtility.Convert.GetString(this._printData.Rows[0]["Team"]);
            int insertRow = 5, startRow = 5, ttlShift = 1, subRows = 0;
            worksheet.Cells[3, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
            DataRow[] selectRow;
            foreach (DataRow dr in this._printData.Rows)
            {
                if (shift != MyUtility.Convert.GetString(dr["Shift"]) || team != MyUtility.Convert.GetString(dr["Team"]))
                {
                    // 將多出來的Record刪除
                    for (int i = 1; i <= 2; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                        rng.Select();
                        rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                        Marshal.ReleaseComObject(rng);
                    }

                    // 填入Sub Total資料
                    if (this._ttlData != null)
                    {
                        selectRow = this._ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                        if (selectRow.Length > 0)
                        {
                            worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                            worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                        }
                    }

                    worksheet.Cells[insertRow, 7] = string.Format("=SUM(G{0}:G{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 8] = string.Format("=SUM(H{0}:H{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 11] = string.Format("=SUM(K{0}:K{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 12] = string.Format("=SUM(L{0}:L{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
                    worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                    worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));

                    subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                    if (shift != "Subcon-Out")
                    {
                        subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
                    }

                    if (shift != "Subcon-Out" && shift != "Subcon-In(Non Sister)")
                    {
                        subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                    }

                    // 重置參數資料
                    shift = MyUtility.Convert.GetString(dr["Shift"]);
                    team = MyUtility.Convert.GetString(dr["Team"]);
                    worksheet.Cells[insertRow + 2, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
                    insertRow = insertRow + 4;
                    startRow = insertRow;
                    ttlShift++;
                    subRows++;
                }

                objArray[0, 0] = dr["SewingLineID"];
                objArray[0, 1] = dr["OrderId"];
                objArray[0, 2] = dr["Style"];
                objArray[0, 3] = dr["CDNo"];
                objArray[0, 4] = dr["ActManPower"];
                objArray[0, 5] = dr["WorkHour"];
                objArray[0, 6] = dr["ManHour"];
                objArray[0, 7] = dr["TargetCPU"];
                objArray[0, 8] = dr["TMS"];
                objArray[0, 9] = dr["CPUPrice"];
                objArray[0, 10] = dr["TargetQty"];
                objArray[0, 11] = dr["QAQty"];
                objArray[0, 12] = dr["TotalCPU"];
                objArray[0, 13] = dr["CPUSewer"];
                objArray[0, 14] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", insertRow);
                objArray[0, 15] = dr["RFT"];
                objArray[0, 16] = dr["CumulateDate"];
                objArray[0, 17] = dr["InlineQty"];
                objArray[0, 18] = dr["Diff"];
                worksheet.Range[string.Format("A{0}:S{0}", insertRow)].Value2 = objArray;
                insertRow++;

                // 插入一筆Record
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            // 最後一個Shift資料
            // 將多出來的Record刪除
            for (int i = 1; i <= 2; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            // 填入Sub Total資料
            if (this._ttlData != null)
            {
                selectRow = this._ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                if (selectRow.Length > 0)
                {
                    worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                    worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                    worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                }
            }

            worksheet.Cells[insertRow, 7] = string.Format("=SUM(G{0}:G{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 8] = string.Format("=SUM(H{0}:H{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 11] = string.Format("=SUM(K{0}:K{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 12] = string.Format("=SUM(L{0}:L{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
            if (shift != "Subcon-Out")
            {
                subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
            }

            if (shift != "Subcon-Out" && shift != "Subcon-In(Non Sister)")
            {
                subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
            }

            // 刪除多出來的Shift Record
            for (int i = 1; i <= (8 - ttlShift) * 6; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow + 1, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            insertRow = insertRow + 2;

            // 填Grand Total資料
            string ttlManhour, targetCPU, targetQty, qaQty, ttlCPU, prodOutput, diff;
            if (this._ttlData != null)
            {
                selectRow = this._ttlData.Select("Type = 'Grand'");
                if (selectRow.Length > 0)
                {
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[i]["ActManPower"]);
                        worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[i]["TMS"]);
                        worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[i]["RFT"]);
                        ttlManhour = "=";
                        targetCPU = "=";
                        targetQty = "=";
                        qaQty = "=";
                        ttlCPU = "=";
                        prodOutput = "=";
                        diff = "=";
                        #region 組公式
                        if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "2")
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (!MyUtility.Check.Empty(subTtlRowInOut[j]))
                                {
                                    ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowInOut[j]);
                                    targetCPU = targetCPU + string.Format("H{0}+", subTtlRowInOut[j]);
                                    targetQty = targetQty + string.Format("K{0}+", subTtlRowInOut[j]);
                                    qaQty = qaQty + string.Format("L{0}+", subTtlRowInOut[j]);
                                    ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowInOut[j]);
                                    prodOutput = prodOutput + string.Format("R{0}+", subTtlRowInOut[j]);
                                    diff = diff + string.Format("S{0}+", subTtlRowInOut[j]);
                                }
                            }
                        }
                        else if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "3")
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (!MyUtility.Check.Empty(subTtlRowExOut[j]))
                                {
                                    ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowExOut[j]);
                                    targetCPU = targetCPU + string.Format("H{0}+", subTtlRowExOut[j]);
                                    targetQty = targetQty + string.Format("K{0}+", subTtlRowExOut[j]);
                                    qaQty = qaQty + string.Format("L{0}+", subTtlRowExOut[j]);
                                    ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowExOut[j]);
                                    prodOutput = prodOutput + string.Format("R{0}+", subTtlRowExOut[j]);
                                    diff = diff + string.Format("S{0}+", subTtlRowExOut[j]);
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (!MyUtility.Check.Empty(subTtlRowExInOut[j]))
                                {
                                    ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowExInOut[j]);
                                    targetCPU = targetCPU + string.Format("H{0}+", subTtlRowExInOut[j]);
                                    targetQty = targetQty + string.Format("K{0}+", subTtlRowExInOut[j]);
                                    qaQty = qaQty + string.Format("L{0}+", subTtlRowExInOut[j]);
                                    ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowExInOut[j]);
                                    prodOutput = prodOutput + string.Format("R{0}+", subTtlRowExInOut[j]);
                                    diff = diff + string.Format("S{0}+", subTtlRowExInOut[j]);
                                }
                            }
                        }
                        #endregion

                        worksheet.Cells[insertRow, 7] = ttlManhour.Substring(0, ttlManhour.Length - 1);
                        worksheet.Cells[insertRow, 8] = targetCPU.Substring(0, targetCPU.Length - 1);
                        worksheet.Cells[insertRow, 11] = targetQty.Substring(0, targetQty.Length - 1);
                        worksheet.Cells[insertRow, 12] = qaQty.Substring(0, qaQty.Length - 1);
                        worksheet.Cells[insertRow, 13] = ttlCPU.Substring(0, ttlCPU.Length - 1);
                        worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 18] = prodOutput.Substring(0, prodOutput.Length - 1);
                        worksheet.Cells[insertRow, 19] = diff.Substring(0, diff.Length - 1);
                        insertRow++;
                    }
                }
            }

            insertRow = insertRow + 2;
            foreach (DataRow dr in this._subprocessData.Rows)
            {
                worksheet.Cells[insertRow, 3] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["rs"]));
                worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString(dr["Price"]);
                insertRow++;

                // 插入一筆Record
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Sewing_R01_DailyCMPReport");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
