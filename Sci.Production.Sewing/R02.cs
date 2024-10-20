﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Class.Command;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg;
using System.Data.SqlClient;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private string line1;
        private string line2;
        private string factory;
        private string factoryName;
        private string mDivision;
        private DateTime? date1;
        private DateTime? date2;
        private int reportType;
        private int orderby;
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
        private List<APIData> pams = new List<APIData>();
        private int workDay;
        private string totalCPUIncludeSubConIn;
        private decimal SPH_totalCPU;
        private DataTable[] SewingR04;
        private DataSet dsPams;

        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
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
            MyUtility.Tool.SetupCombox(this.comboReportType, 1, 1, "By Date,By Sewing Line,By Sewing Line By Team");
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Sewing Line,CPU/Sewer/HR");
            this.comboReportType.SelectedIndex = 0;
            this.comboFactory.Text = Env.User.Factory;
            this.comboOrderBy.SelectedIndex = 0;
            this.comboM.Text = Env.User.Keyword;
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
                this.dateDateEnd.Value = Convert.ToDateTime(this.dateDateStart.Value).AddDays(1 - Convert.ToDateTime(this.dateDateStart.Value).Day).AddMonths(1).AddDays(-1);
            }
        }

        // Report Type
        private void ComboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelOrderBy.Visible = true;
            this.comboOrderBy.Visible = true;
            switch (this.comboReportType.SelectedIndex)
            {
                case 0:
                case 2:
                    this.labelOrderBy.Visible = false;
                    this.comboOrderBy.Visible = false;
                    break;
            }
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
            if (MyUtility.Check.Empty(this.dateDateStart.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (this.comboReportType.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Report type can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.comboM.Text))
            {
                MyUtility.Msg.WarningBox("M can't empty!!");
                return false;
            }

            if (this.comboReportType.SelectedIndex == 1)
            {
                if (this.comboFactory.SelectedIndex == -1 || this.comboFactory.SelectedIndex == 0)
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    return false;
                }

                if (this.comboOrderBy.SelectedIndex == -1)
                {
                    MyUtility.Msg.WarningBox("Order by can't empty!!");
                    return false;
                }
            }

            if (this.comboReportType.SelectedIndex == 2)
            {
                if (this.comboFactory.SelectedIndex == -1 || this.comboFactory.SelectedIndex == 0)
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    return false;
                }
            }

            this.date1 = this.dateDateStart.Value;
            this.date2 = this.dateDateEnd.Value;
            this.line1 = this.txtSewingLineStart.Text;
            this.line2 = this.txtSewingLineEnd.Text;
            this.factory = this.comboFactory.Text;
            this.mDivision = this.comboM.Text;
            this.reportType = this.comboReportType.SelectedIndex;
            this.orderby = this.comboOrderBy.SelectedIndex;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // 若有變更到  Total CPU Included Subcon In 的相關計算規則&篩選條件, 要一併變更 sql的table function GetCMPDetail (這是只有FMS要用的)
            StringBuilder sqlCmd = new StringBuilder();
            DualResult failResult;
            DBProxy.Current.DefaultTimeout = 600; // 加長時間成10分鐘, 避免time out
            Sewing_R02 biModel = new Sewing_R02();

            #region 組撈全部Sewing output data SQL
            Sewing_R02_ViewModel sewing_R02_Model = new Sewing_R02_ViewModel()
            {
                StartOutputDate = this.dateDateStart.Value,
                EndOutputDate = this.dateDateEnd.Value,
                Factory = this.factory,
                M = this.mDivision,
                ReportType = this.reportType + 1,
                StartSewingLine = this.txtSewingLineStart.Text,
                EndSewingLine = this.txtSewingLineEnd.Text,
                OrderBy = this.orderby + 1,
                ExcludeNonRevenue = this.chkExcludeNonRevenue.Checked,
                ExcludeSampleFactory = this.checkSampleFty.Checked,
                ExcludeOfMockUp = this.checkExcludeOfMockUp.Checked,
            };

            Base_ViewModel resultReport = biModel.GetMonthlyProductionOutputReport(sewing_R02_Model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.SewOutPutData = resultReport.DtArr[0];
            this.printData = resultReport.DtArr[1];

            #endregion

            #region 整理Total Exclude Subcon-In & Out
            try
            {
                resultReport = biModel.GetTotalExcludeSubconIn(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.excludeInOutTotal = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Total Exclude Subcon-In & Out total data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理non Sister SubCon In
            try
            {
                resultReport = biModel.GetNoNSisterSubConIn(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.NonSisterInTotal = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Non Sister SubCon In data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理Sister SubCon In
            try
            {
                resultReport = biModel.GetSisterSubConIn(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.SisterInTotal = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Sister SubCon In data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理CPU Factor
            try
            {
                resultReport = biModel.GetCPUFactor(this.SewOutPutData);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.cpuFactor = resultReport.Dt;
            }
            catch (Exception ex)
            {
                failResult = new DualResult(false, "Query CPU factor data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理Subprocess資料
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubprocess(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subprocessData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subprocess by Company Subcon-In資料 Orders.program
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubprocessbyCompanySubconIn(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subprocessSubconInData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subprocess by Company Subcon-Out資料 SewingOutput.SubconOutFty
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubprocessbyCompanySubconOut(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subprocessSubconOutData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subcon資料
            if (this.printData.Rows.Count > 0)
            {
                try
                {
                    resultReport = biModel.GetSubcon(this.SewOutPutData);
                    if (!resultReport.Result)
                    {
                        return resultReport.Result;
                    }

                    this.subconData = resultReport.Dt;
                }
                catch (Exception ex)
                {
                    failResult = new DualResult(false, "Query subcon data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理工作天數
            sewing_R02_Model = new Sewing_R02_ViewModel()
            {
                IsCN = Env.User.Keyword.EqualString("CM1") || Env.User.Keyword.EqualString("CM2"),
                M = this.mDivision,
                Factory = this.factory,
                StartDate = this.date1.Value,
                EndDate = this.date2.Value,
            };

            resultReport = biModel.GetWorkDay(this.SewOutPutData, sewing_R02_Model);
            if (!resultReport.Result)
            {
                failResult = new DualResult(false, "Query Work Day fail\r\n" + resultReport.Result.Messages.ToString());
                return failResult;
            }

            this.workDay = resultReport.IntValue;
            #endregion

            #region Direct Manpower(From PAMS)
            sewing_R02_Model = new Sewing_R02_ViewModel()
            {
                M = this.mDivision,
                Factory = this.factory,
                StartDate = this.date1.Value,
                EndDate = this.date2.Value,
            };

            this.pams = biModel.GetPAMS(sewing_R02_Model);
            #endregion

            #region SewingR04 外發加工段計算 + SPH TotalCPU 計算
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@Md", this.mDivision),
                new SqlParameter("@F", this.factory),
                new SqlParameter("@SDate",  this.date1),
                new SqlParameter("@EDate",  this.date2),
                new SqlParameter("@Include_Artwork", true),
                new SqlParameter("@SewinglineStart", this.line1),
                new SqlParameter("@SewinglineEnd", this.line2),
            };

            string sqlwhere = string.Empty;

            if (!this.line1.Empty())
            {
                sqlwhere += " and s.SewingLineID >= @line1";
            }

            if (!this.line1.Empty())
            {
                sqlwhere += " and s.SewingLineID <= @line2";
            }

            string sqlGetSubConPo = $@"
declare @StartDate date = @SDate
declare @EndDate date = @EDate
declare @Factory varchar(10) = @F
declare @M varchar(10) = @Md
declare @line1 varchar(10) = @SewinglineStart
declare @line2 varchar(10) = @SewinglineEnd


select  s.OutputDate
		, s.Category
		, s.Shift
		, s.SewingLineID
		, [ActManPower] = s.Manpower
		, s.Team
		, sd.OrderId
		, sd.ComboType
		, sd.WorkHour
		, sd.QAQty
		, sd.InlineQty
		, [OrderCategory] = isnull(o.Category,'')
		, o.LocalOrder
		, s.FactoryID
		, f.MDivisionID
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
left join factory f WITH (NOLOCK) on f.id=s.FactoryID
where	(o.CateGory NOT IN ('G','A') or s.Category='M') 
and	s.OutputDate between @StartDate and @EndDate
and s.FactoryID = @Factory and s.MDivisionID = @M
{sqlwhere}

select OutputDate,Category
	   , Shift
	   , SewingLineID
	   , ActManPower1 = ActManPower
	   , Team
	   , OrderId
	   , ComboType
	   , WorkHour = Round(sum(WorkHour),3)
	   , QAQty = sum(QAQty)
	   , InlineQty = sum(InlineQty)
	   , OrderCategory
	   , LocalOrder
	   , FactoryID
	   , MDivisionID
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
		 , OrderCategory, LocalOrder, FactoryID, MDivisionID, OrderProgram, MockupProgram
		 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
		 , MockupStyle, Rate, StdTMS,SubconInType,isnull(SubconOutFty,'')
        ,ActManPower

select t.*
	   , isnull(w.Holiday, 0) as Holiday
	   , ActManPower1 as ActManPower
INTO #tmp1stFilter
from #tmpSewingGroup t
left join WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
									  and w.Date = t.OutputDate 
									  and w.SewingLineID = t.SewingLineID

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
	   , MDivisionID
       , SubconInType
       , SubconOutFty
into #tmp
from #tmp1stFilter

alter table #tmp alter column OrderId varchar(13)
alter table #tmp alter column ComboType varchar(1)
alter table #tmp alter column QAQty int
alter table #tmp alter column LastShift varchar(1)
alter table #tmp alter column SubconInType varchar(1)

Select ID
		, rs = iif(ProductionUnit = 'TMS', 'CPU'
		   								, iif(ProductionUnit = 'QTY', 'AMT'
		   															, '')),
        [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							    when ProductionUnit = 'TMS' then 3
							    else 0 end
into #tmpArtwork
from ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') 
		and IsTtlTMS = 0
        and IsPrintToCMP=1

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
    
select ot.ArtworkTypeID
		, a.OrderId
		, a.ComboType
        , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
into  #tmpAllSubprocess
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

--FMS傳票部分顯示AT不分Hand/Machine，是因為政策問題，但比對Sewing R02時，會有落差，請根據SP#落在Hand CPU:10 /Machine:5，則只撈出Hand CPU:10這筆，抓其大值，以便加總總和等同於FMS傳票AT
-- 當AT(Machine) = AT(Hand)時, 也要將Price歸0 (ISP20190520)
update s set s.Price = 0
    from #tmpAllSubprocess s
    inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (HAND)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
    where s.ArtworkTypeID = 'AT (MACHINE)'  and s.Price <= a.Price

update s set s.Price = 0
    from #tmpAllSubprocess s
    inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (MACHINE)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
    where s.ArtworkTypeID = 'AT (HAND)'  and s.Price <= a.Price

select ArtworkTypeID = t1.ID
	   , Price = isnull(sum(Round(t2.Price,t1.DecimalNumber)), 0)
	   , rs
into #tmpFinal#tmpArtwork 
from #tmpArtwork t1
left join #tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs
order by t1.ID

-- 取得SubConOut 數值
--先抓出SubCon_R16 
--有在Subcon_R16 [Subcon Purchase order]且狀態為Approve=>視同外發
;with cte
as
(
	select t.*,ArtworkType.ID artworktypeid
	from dbo.ArtworkType WITH (NOLOCK) ,
	(
		select distinct (select orders.poid from orders WITH (NOLOCK) where id=b.OrderId) orderid 
		from dbo.artworkpo a	WITH (NOLOCK) 
		inner join dbo.ArtworkPo_Detail b WITH (NOLOCK) on b.id = a.id
		where 1=1
		and (a.issuedate between @StartDate and @EndDate) 
		and a.mdivisionid = @M and a.factoryid = @Factory
		AND a.Status = 'Approved'
	)t
	where Artworktype.IsSubprocess=1 
)
select aa.FactoryID
,cte.artworktypeid
,aa.POID
,aa.MDivisionID
into #tmpSubConR16
from cte
left join orders aa WITH (NOLOCK) on aa.id = cte.orderid
left join Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.ID and bb.ArtworkTypeID = cte.artworktypeid
outer apply (
	select isnull(sum(t.po_amt),0.00) po_amt, isnull(sum(t.po_qty),0) po_qty from (
	select po.currencyid,
			pod.Price,
			pod.poQty po_qty
			,pod.poQty*pod.Price*dbo.getRate('FX',po.CurrencyID,'USD',po.issuedate) po_amt
			,dbo.getRate('FX',po.CurrencyID,'USD',po.issuedate) rate
	from ArtworkPo po WITH (NOLOCK) 
    inner join ArtworkPo_Detail pod WITH (NOLOCK) on pod.id = po.Id 
    inner join orders WITH (NOLOCK) on orders.id = pod.orderid
		where po.ArtworkTypeID = cte.artworktypeid and orders.POId = aa.POID    AND Orders.Category=aa.Category) t
) x		
where po_qty > 0

select  FactoryID = s.FactoryID
,OrderId = sd.OrderId
,Team = s.Team
,OutputDate = s.OutputDate
,SewingLineID = s.SewingLineID
,LastShift = IIF(s.Shift <> 'O' and s.Category <> 'M' and o.LocalOrder = 1, 'I',s.Shift) 
,Category = s.Category
,ComboType = sd.ComboType
,SubconOutFty = s.SubconOutFty
,SubConOutContractNumber = s.SubConOutContractNumber
,[Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100
,sd.QAQty
,ot.Price
,ot.ArtworkTypeID
,ttlPrice = Round(sum(sd.QAQty*isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100 * ot.Price)over(partition by s.FactoryID,sd.OrderId,s.Team,s.OutputDate,s.SewingLineID, IIF(s.Shift <> 'O' and s.Category <> 'M' and o.LocalOrder = 1, 'I',s.Shift) ,s.Category,sd.ComboType,s.SubconOutFty,s.SubConOutContractNumber),3)
into #tmpFinal
from SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
inner join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = o.id
where 1=1 
and  exists(
	select 1 from #tmpSubConR16 s 
	where s.POID = sd.OrderID
	and s.FactoryID = s.FactoryID
	and s.MDivisionID = s.MDivisionID
	and s.artworktypeid = ot.ArtworkTypeID
)
and (@StartDate is null or s.OutputDate >= @StartDate) and (@EndDate is null or s.OutputDate <= @EndDate) and
((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
{sqlwhere}

-- 取得外發清單
select ArtworkTypeID
,[ProductionUnit] = iif(a.ProductionUnit = 'TMS', 'CPU'
		   			, iif(a.ProductionUnit = 'QTY', 'AMT', ''))
,[TTL_Price] = sum(ttlPrice) 
from #tmpFinal t
left join ArtworkType a WITH (NOLOCK) on t.ArtworkTypeID = a.ID
group by t.ArtworkTypeID,a.ProductionUnit

declare　@TTLCPU float = (select sum(Price) from #tmpFinal#tmpArtwork where rs ='CPU')
declare　@AMT float = (select sum(Price) from #tmpFinal#tmpArtwork where rs ='AMT' and ArtworkTypeID in ('EMBROIDERY','Garment Dye','GMT WASH','PRINTING'))
declare @SubConOut float = (select [TTL_Price] = sum(ttlPrice) from #tmpFinal)

-- 取得SPH Total CPU
select [SPH_ttlCPU] = isnull(@TTLCPU,0) + ((isnull(@AMT,0) - isnull(@SubConOut,0))/2.5)

drop table #tmp,#tmp1stFilter,#tmpAllSubprocess,#tmpArtwork,#tmpSewingDetail,#tmpSewingGroup,#TPEtmp,#tmpFinal#tmpArtwork,#tmpFinal,#tmpSubConR16
";

            DualResult result = DBProxy.Current.Select(null, sqlGetSubConPo, listPar, out this.SewingR04);
            if (!result)
            {
                failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            #endregion

            #region 呼叫Pams API
            AttendanceSummary_APICondition attendanceSummary_API = new AttendanceSummary_APICondition()
            {
                FactoryID = this.factory,
                StartDate = ((DateTime)this.dateDateStart.Value).ToString("yyyy/MM/dd"),
                EndDate = ((DateTime)this.dateDateEnd.Value).ToString("yyyy/MM/dd"),
                IsContainShare = false,
                IsLocal = false,
            };

            this.dsPams = biModel.GetPamsAttendanceSummaryAsync(attendanceSummary_API);
            #endregion

            if (MyUtility.Check.Empty(this.factory) && !MyUtility.Check.Empty(this.mDivision))
            {
                this.factoryName = MyUtility.GetValue.Lookup(string.Format("select Name from Mdivision WITH (NOLOCK) where ID = '{0}'", this.mDivision));
            }
            else
            {
                this.factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", this.factory));
            }

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
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
            string strXltName = Env.Cfg.XltPathDir;
            switch (this.reportType)
            {
                case 0:
                    strXltName += "\\Sewing_R02_MonthlyReportByDate.xltx";
                    break;
                case 1:
                    strXltName += "\\Sewing_R02_MonthlyReportBySewingLine.xltx";
                    break;
                case 2:
                    strXltName += "\\Sewing_R02_MonthlyReportBySewingLineByTeam.xltx";
                    break;
            }

            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format("{0}", this.factoryName);
            worksheet.Cells[3, 1] = string.Format(
                "All Factory Monthly CMP Report, MTH:{1}",
                MyUtility.Check.Empty(this.factory) ? "All Factory" : this.factory,
                Convert.ToDateTime(this.date1).ToString("yyyy/MM"));

            int insertRow;
            object[,] objArray = new object[1, 15];

            // Top Table建立
            this.SetExcelTopTable(out insertRow, this.pams, worksheet, excel);

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

            // [GPH] [SPH] [VPH]
            DataTable dtOther = this.dsPams.Tables["other"];
            insertRow += 2;
            for (int i = 1; i <= 3; i++)
            {
                decimal totalCPU = 0;
                decimal totalMemory = 0;
                insertRow++;

                // [GPH]
                switch (i)
                {
                    // [GPH]
                    case 1:
                        totalCPU = MyUtility.Convert.GetDecimal(this.totalCPUIncludeSubConIn);
                        totalMemory = MyUtility.Convert.GetDecimal(dtOther.Rows[0]["GPH_Manhours"]);
                        break;

                    // [SPH]
                    case 2:
                        totalCPU = MyUtility.Convert.GetDecimal(this.SewingR04[1].Rows[0]["SPH_ttlCPU"]);
                        totalMemory = MyUtility.Convert.GetDecimal(dtOther.Rows[0]["SPH_Manhours"]);
                        break;

                    // [VPH]
                    case 3:
                        totalCPU = MyUtility.Convert.GetDecimal(this.totalCPUIncludeSubConIn) + MyUtility.Convert.GetDecimal(this.SewingR04[1].Rows[0]["SPH_ttlCPU"]);
                        totalMemory = MyUtility.Convert.GetDecimal(dtOther.Rows[0]["FtyManhours"]);
                        break;
                    default:
                        break;
                }

                worksheet.Cells[insertRow, 2] = totalCPU; // Total CPU
                worksheet.Cells[insertRow, 3] = totalMemory; // Total Manhours
                worksheet.Cells[insertRow, 4] = MyUtility.Check.Empty(totalMemory) ? 0 : Math.Round(MyUtility.Convert.GetDouble(totalCPU / totalMemory), 2); // Total CPU/Manhours
            }

            // Subprocess
            insertRow = insertRow + 3;
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

            insertRow = insertRow + 3;
            worksheet.Cells[insertRow, 1] = "Total work day:";
            worksheet.Cells[insertRow, 3] = this.workDay;

            // Subcon
            int revenueStartRow = 0;
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
                // 要保留外發加工的欄位所以加上5行
                this.DeleteExcelRow(9, insertRow + 4, excel);
            }

            #region Only Subcon Out 含外發整件成衣& 外發加工段
            insertRow += 1;
            DataTable dtSubcOut = this.SewingR04[0];
            if (dtSubcOut.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSubcOut.Rows)
                {
                    // 插入一筆record
                    rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);

                    worksheet.Cells[insertRow, 2] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["ProductionUnit"]));
                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr["TTL_Price"]);
                    insertRow++;
                }
            }
            else
            {
                // 刪除沒資料的欄位
                this.DeleteExcelRow(3, insertRow, excel);
            }

            #endregion

            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(this.reportType == 0 ? "Sewing_R02_MonthlyReportByDate" : "Sewing_R02_MonthlyReportBySewingLine");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Visible = true;
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

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

        private void SetExcelTopTable(out int insertRow, List<APIData> pams, Excel.Worksheet worksheet, Excel.Application excel)
        {
            insertRow = 5;
            Excel.Range rngToInsert;
            object[,] objArray = new object[1, 18];
            int iQAQty = 2, iTotalCPU = 3, iCPUSewer = 6, iAvgWorkHour = 7, iManHour = 9, iEff = 10;
            string sEff;
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

                if (this.reportType == 2)
                {
                    objArray[0, 9] = dr[9];

                    // EFF %  欄位公式
                    objArray[0, 10] = string.Format("=IF(J{0}=0,0,ROUND((D{0}/(J{0}*3600/1400))*100,1))", insertRow);
                }
                else
                {
                    // EFF %  欄位公式
                    objArray[0, 9] = string.Format("=IF(I{0}=0,0,ROUND((C{0}/(I{0}*3600/1400))*100,1))", insertRow);

                    if (this.reportType == 0)
                    {
                        if (pams != null && pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).Count() > 0)
                        {
                            // Total Manpower (PAMS)
                            objArray[0, 11] = pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().SewTtlManpower;

                            // Total Manhours (PAMS)
                            objArray[0, 12] = pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().SewTtlManhours;

                            string holiday = (pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().Holiday == 1) ? "Y" : string.Empty;

                            /*
                            // Test
                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerIn = 2.5M;
                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerOut = 1.5M;

                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursIn = 2.5M;
                            pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursOut = 6.7M;
                            */

                            // Holiday (PAMS)
                            objArray[0, 14] = holiday;
                            decimal transferManpower = MyUtility.Convert.GetDecimal(pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerIn
                                - pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManpowerOut);

                            decimal transferManhours = MyUtility.Convert.GetDecimal(pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursIn
                                - pams.Where(w => w.Date.ToShortDateString().EqualString(((DateTime)dr["OutputDate"]).ToShortDateString())).FirstOrDefault().TransManhoursOut);

                            // Transfer Manpower(PAMS)
                            objArray[0, 15] = transferManpower;

                            // Transfer Manhours(PAMS)]
                            objArray[0, 16] = transferManhours;
                        }
                        else
                        {
                            objArray[0, 11] = 0;
                            objArray[0, 12] = 0;
                        }

                        // Average Working Hour(PAMS)
                        objArray[0, 10] = MyUtility.Convert.GetDouble(objArray[0, 11]) == 0 ? 0 : MyUtility.Convert.GetDouble(objArray[0, 12]) / MyUtility.Convert.GetDouble(objArray[0, 11]);

                        // EFF % (PAMS) 欄位公式
                        objArray[0, 13] = string.Format("=IF(M{0}=0,0,ROUND((C{0}/(M{0}*3600/1400))*100,1))", insertRow);

                        // Remark
                        objArray[0, 17] = string.Empty;
                    }
                }

                worksheet.Range[string.Format("A{0}:R{0}", insertRow)].Value2 = objArray;
                insertRow++;

                // 插入一筆Record
                rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            // 將多出來的Record刪除
            this.DeleteExcelRow(2, insertRow, excel);
            Excel.Range rg = worksheet.UsedRange;

            // Total
            if (this.reportType == 2)
            {
                iQAQty = 3;
                iTotalCPU = 4;
                iCPUSewer = 7;
                iAvgWorkHour = 8;
                iManHour = 10;
                iEff = 11;
                worksheet.Cells[insertRow, 3] = string.Format("=SUM(C5:C{0})", MyUtility.Convert.GetString(insertRow - 1));
                this.totalCPUIncludeSubConIn = rg.Cells[insertRow, 3].Value.ToString();
                worksheet.Cells[insertRow, 4] = string.Format("=SUM(D5:D{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 5] = string.Format("=SUM(E5:E{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 6] = string.Format("=SUM(F5:F{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 7] = string.Format("=ROUND(D{0}/J{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 8] = string.Format("=ROUND(J{0}/I{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 9] = string.Format("=SUM(I5:I{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 10] = string.Format("=SUM(J5:J{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 11] = string.Format("=ROUND(D{0}/(J{0}*60*60/1400)*100,1)", insertRow);
            }
            else
            {
                worksheet.Cells[insertRow, 2] = string.Format("=SUM(B5:B{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 3] = string.Format("=SUM(C5:C{0})", MyUtility.Convert.GetString(insertRow - 1));
                this.totalCPUIncludeSubConIn = rg.Cells[insertRow, 3].Value.ToString();
                worksheet.Cells[insertRow, 4] = string.Format("=SUM(D5:D{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 5] = string.Format("=SUM(E5:E{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 6] = string.Format("=ROUND(C{0}/I{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 7] = string.Format("=ROUND(I{0}/H{0},2)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 8] = string.Format("=SUM(H5:H{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 9] = string.Format("=SUM(I5:I{0})", MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 10] = string.Format("=ROUND(C{0}/(I{0}*60*60/1400)*100,1)", insertRow);
                if (this.reportType == 0)
                {
                    worksheet.Cells[insertRow, 11] = string.Format("=ROUND(M{0}/L{0},2)", MyUtility.Convert.GetString(insertRow));
                    worksheet.Cells[insertRow, 12] = string.Format("=SUM(L5:L{0})", MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 13] = string.Format("=SUM(M5:M{0})", MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 14] = string.Format("=ROUND(C{0}/(M{0}*60*60/1400)*100,1)", insertRow);
                    worksheet.Cells[insertRow, 16] = $"=SUM(P5:P{MyUtility.Convert.GetString(insertRow - 1)})";
                    worksheet.Cells[insertRow, 17] = $"=SUM(Q5:Q{MyUtility.Convert.GetString(insertRow - 1)})";
                }
            }

            insertRow++;

            // Excluded non sister Subcon In
            sEff = this.reportType == 2 ? string.Format("=ROUND((D{0}/(J{0}*3600/1400))*100,1)", insertRow) : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            worksheet.Cells[insertRow, iQAQty] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, iTotalCPU] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, iCPUSewer] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, iAvgWorkHour] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["AvgWorkHour"]);
            worksheet.Cells[insertRow, iManHour] = MyUtility.Convert.GetString((this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : this.excludeInOutTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, iEff] = (this.excludeInOutTotal == null || this.excludeInOutTotal.Rows.Count < 1) ? string.Empty : sEff;
            insertRow++;

            // non sister Subcon In
            sEff = this.reportType == 2 ? string.Format("=ROUND((D{0}/(J{0}*3600/1400))*100,1)", insertRow) : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            worksheet.Cells[insertRow, iQAQty] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, iTotalCPU] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, iCPUSewer] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, iManHour] = MyUtility.Convert.GetString((this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : this.NonSisterInTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, iEff] = (this.NonSisterInTotal == null || this.NonSisterInTotal.Rows.Count < 1) ? string.Empty : sEff;
            insertRow++;

            // sister Subcon In
            sEff = this.reportType == 2 ? string.Format("=ROUND((D{0}/(J{0}*3600/1400))*100,1)", insertRow) : string.Format("=ROUND((C{0}/(I{0}*3600/1400))*100,1)", insertRow);
            worksheet.Cells[insertRow, iQAQty] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, iTotalCPU] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, iCPUSewer] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, iManHour] = MyUtility.Convert.GetString((this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : this.SisterInTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, iEff] = (this.SisterInTotal == null || this.SisterInTotal.Rows.Count < 1) ? string.Empty : sEff;
        }
    }
}
