using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R02 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private string MD;
        private string Factory;
        private string CutCellFrom;
        private string CutCellTo;
        private string SpreadingNo1;
        private string SpreadingNo2;
        private string[] cuttings;
        private DateTime? dateR_CuttingDate1;
        private DateTime? dateR_CuttingDate2;
        private StringBuilder condition_CuttingDate = new StringBuilder();
        private DataTable Maintb;
        private string NameEN;
        private string strExcelName;
        private string selected_splitWorksheet = "CutCell";

        /// <summary>
        /// Initializes a new instance of the <see cref="R02"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // Set ComboM
            DBProxy.Current.Select(null, @"Select Distinct MDivisionID from WorkOrderForPlanning WITH (NOLOCK) ", out DataTable workOrder);
            MyUtility.Tool.SetupCombox(this.comboM, 1, workOrder);
            this.comboM.Text = Env.User.Keyword;
            this.SetCutCell();

            // Set ComboFactory
            this.SetComboFactory();
            this.comboFactory.Text = Env.User.Factory;
        }

        private void RadioByOneDayDetial_CheckedChanged(object sender, EventArgs e)
        {
            this.dateCuttingDate.Control2.Visible = !this.radioByOneDayDetial.Checked;
            if (this.radioByOneDayDetial.Checked)
            {
                this.dateCuttingDate.Control2.Text = string.Empty;
            }
            else
            {
                this.dateCuttingDate.Value2 = this.dateCuttingDate.Value1;
            }
        }

        private void DateCuttingDate_Leave(object sender, EventArgs e)
        {
            if (this.radioBySummary.Checked || this.radioByDetail.Checked)
            {
                if (MyUtility.Check.Empty(this.dateCuttingDate.Value2))
                {
                    this.dateCuttingDate.Value2 = this.dateCuttingDate.Value1;
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateCuttingDate.Value1))
            {
                MyUtility.Msg.WarningBox("Cutting Date can't empty!!");
                return false;
            }

            this.Factory = this.comboFactory.Text;
            this.MD = this.comboM.Text;
            this.dateR_CuttingDate1 = this.dateCuttingDate.Value1;
            this.dateR_CuttingDate2 = this.dateCuttingDate.Value2;
            this.CutCellFrom = this.txtCellFrom.Text;
            this.CutCellTo = this.txtCellTo.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result;
            this.NameEN = MyUtility.GetValue.Lookup("NameEN", Env.User.Factory, "Factory ", "id");

            // 準備CutCell包含非數字
            string scell;
            scell = $@"
select  distinct  {(this.selected_splitWorksheet == "CutCell" ? "CutCellID" : "SpreadingNoID")}
from Cutplan WITH (NOLOCK) 
inner join Cutting on CutPlan.CuttingID = Cutting.ID
where   1 = 1 ";

            if (!MyUtility.Check.Empty(this.MD))
            {
                scell += $" and Cutplan.MDivisionID ='{this.MD}'";
            }

            if (this.radioByDetail.Checked || this.radioBySummary.Checked)
            {
                scell += Environment.NewLine + $"        and Cutplan.EstCutdate >= '{Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd")}'";
                scell += Environment.NewLine + $"        and Cutplan.EstCutdate <= '{Convert.ToDateTime(this.dateR_CuttingDate2).ToString("yyyy/MM/dd")}'";
            }
            else
            {
                scell += Environment.NewLine + $"        and Cutplan.EstCutdate = '{Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                scell += Environment.NewLine + $"        and Cutting.FactoryID = '{this.Factory}'";
            }

            if (!MyUtility.Check.Empty(this.CutCellFrom))
            {
                scell += Environment.NewLine + $"        and  Cutplan.CutCellID >=  '{this.CutCellFrom}'";
            }

            if (!MyUtility.Check.Empty(this.CutCellTo))
            {
                scell += Environment.NewLine + $"        and  Cutplan.CutCellID <=  '{this.CutCellTo}'";
            }

            if (!MyUtility.Check.Empty(this.SpreadingNo1))
            {
                scell += Environment.NewLine + $"        and  Cutplan.SpreadingNoID >=  '{this.SpreadingNo1}'";
            }

            if (!MyUtility.Check.Empty(this.SpreadingNo2))
            {
                scell += Environment.NewLine + $"        and  Cutplan.SpreadingNoID <=  '{this.SpreadingNo2}'";
            }

            scell += Environment.NewLine + "ORDER BY " + (this.selected_splitWorksheet == "CutCell" ? "CutCellID" : "SpreadingNoID");

            result = DBProxy.Current.Select(null, scell, out this.Maintb);

            if (!result)
            {
                return result;
            }

            int sheetsCount = this.Maintb.Rows.Count; // CutCel總數

            if (sheetsCount == 0)
            {
                return Ict.Result.F("Data not found!");
            }

            StringBuilder sqlCmd = new StringBuilder();

            #region radiobtnByM
            if (this.radioByDetail.Checked)
            {
                for (int i = 0; i < sheetsCount; i++)
                {
                    sqlCmd.Append(string.Format("IF OBJECT_ID('tempdb.dbo.#tmpall{0}','U')IS NOT NULL DROP TABLE #tmpall{0}", i));
                    sqlCmd.Append(@"
                    select distinct
	                [Request#] = Cutplan.ID,
	                [Cutting Date] = Cutplan.EstCutdate,
	                [Line#] = cd.SewingLineID,
	                [SP#] = cd.OrderID,
	                [Seq#] = CONCAT(wofp.Seq1, '-', wofp.Seq2),
	                [Style#] = o.StyleID,
                    [FabRef#] = wofp.refno,
	                [Ref#] = cd.CutRef,
	                [Cut#] = cd.CutNo,
	                [Comb.] = wofp.FabricCombo,
	                [Fab_Code] = fab.fab,
	                [Size Ratio] = sr.SizeCode,
	                [Colorway] = woda.AC,
	                [Color] = cd.ColorID,
	                [Cut Qty] = cq.SizeCode,
	                [Fab Cons.] = cd.Cons,
                    [Layer] = wofp.Layer,
                    [FabLength] = iif(wofp.Layer = 0, '', STR(cd.Cons / wofp.Layer, 12, 2) ),
	                [Fab Desc] = [Production].dbo.getMtlDesc(cd.POID, wofp.Seq1, wofp.Seq2,2,0),
                    [Shift]='',
	                [Remark] = cd.Remark,
	                [SCI Delivery] = o.SciDelivery,
	                [ms] = ms.Seq,
                    [total_qty] = sum(wofps.qty * wofp.Layer) over(partition by cd.WorkOrderForPlanningUkey)
                    into #tmpall");
                    sqlCmd.Append(string.Format("{0}", i));
                    sqlCmd.Append(@"
                    from Cutplan WITH (NOLOCK) 
                    inner join Cutting cut on Cutplan.CuttingID = cut.ID
                    inner join Cutplan_Detail cd WITH (NOLOCK) on Cutplan.ID = cd.ID
                    inner join WorkOrderForPlanning wofp WITH (NOLOCK) on cd.WorkOrderForPlanningUkey = wofp.Ukey and cd.ID = wofp.CutplanID
                    inner join WorkOrderForPlanning_SizeRatio wofps WITH (NOLOCK) on cd.WorkOrderForPlanningUkey = wofps.WorkOrderForPlanningUkey
                    left join Orders o WITH (NOLOCK) on o.ID = cd.OrderID
                    outer apply(
                        select AC= 
                           STUFF((
                           Select distinct concat('/', wd.Article)
                           from WorkOrderForPlanning_Distribute wd WITH (NOLOCK) 
                           where WorkOrderForPlanningUKey = cd.WorkOrderForPlanningUKey and Article != ''
                           for xml path('')
                        ),1,1,'')
                    ) as woda
                    outer apply(
	                    select fab = 
	                    STUFF((
		                    select concat('+',wp.PatternPanel)
		                    from WorkOrderForPlanning_PatternPanel wp
		                    where wp.WorkOrderForPlanningUkey = wofp.Ukey
		                    for xml path('')
	                    ),1,1,'')
                    ) fab
                   outer apply(
	                    select SizeCode = 
	                    STUFF((
		                    Select concat(',',(ws.SizeCode+'/'+Convert(varchar,Qty))) 
		                    from WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) 
		                    left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		                    where WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUkey
		                    ORDER BY Order_SizeCode.Seq
		                    for xml path('')
	                     ),1,1,'')
                    ) as sr
                    outer apply(
	                    select SizeCode= 
	                    STUFF((
		                    Select concat(',',SizeCode+'/'+Convert(varchar,Qty*(select Layer from WorkOrderForPlanning WITH (NOLOCK) where UKey = cd.WorkOrderForPlanningUKey))) 
		                    from WorkOrderForPlanning_SizeRatio  WITH (NOLOCK) where WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUKey
		                    for xml path('')
	                     ),1,1,'')
                    ) as cq
                    outer apply(
	                    select Seq= (
		                    Select min(Order_SizeCode.Seq)
		                    from WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) 
		                    left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		                    where WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUKey
	                     )
                    ) as ms
                    where 1 = 1
");
                    if (!MyUtility.Check.Empty(this.dateR_CuttingDate1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.EstCutdate >= '{0}' ", Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd")));
                    }

                    if (!MyUtility.Check.Empty(this.dateR_CuttingDate2))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.EstCutdate <= '{0}' ", Convert.ToDateTime(this.dateR_CuttingDate2).ToString("yyyy/MM/dd")));
                    }

                    if (!MyUtility.Check.Empty(this.MD))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", this.MD));
                    }

                    if (!MyUtility.Check.Empty(this.Factory))
                    {
                        sqlCmd.Append(string.Format(" and cut.FactoryID = '{0}' ", this.Factory));
                    }

                    if (!MyUtility.Check.Empty(this.CutCellFrom) || !MyUtility.Check.Empty(this.CutCellTo))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = '{0}' ", this.Maintb.Rows[i][0].ToString()));
                    }

                    if (!MyUtility.Check.Empty(this.SpreadingNo1) || !MyUtility.Check.Empty(this.SpreadingNo2))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.SpreadingNoID = '{0}' ", this.Maintb.Rows[i][0].ToString()));
                    }

                    sqlCmd.Append(@"order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]");

                    sqlCmd.Append(@"
select 
[Request#1] = case when ((Row_number() over (partition by [Line#],[Request#]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1
	and [SP#] = LAG([SP#],1,[SP#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) then '' else [Request#] end,
[Cutting Date1] = case when (Row_number() over (partition by [Cutting Date]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1  then '' else Convert(varchar,[Cutting Date]) end,
[Line#1] = case when (((Row_number() over (partition by [Line#],[Request#]
	order by [Line#] ,[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1)
	and [SP#] = LAG([SP#],1,[SP#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) then '' else [Line#] end,
[SP#1] = case when  ((Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]))then '' else [SP#] end ,
[Seq#1] = case when ((Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]))then '' else [Seq#] end,
[Style#1] = case when ((Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#],[Style#] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]))then '' else [Style#] end,
[FabRef#] = [FabRef#],
[Ref#] = [Ref#],
[Cut#] = [Cut#],
[Comb.] = [Comb.],
[Fab_Code] = [Fab_Code],
[Size Ratio] = [Size Ratio],
[Colorway1] = case when ((Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#] ,[Seq#],[Style#],[Fab Desc],[Colorway] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]))then '' else [Colorway] end,
[Color1] = case when ((Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#],[Style#],[Fab Desc],[Colorway],[Color]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]))then '' else [Color] end,
[Cut Qty] = [Cut Qty],
[Fab Cons.] = [Fab Cons.],
[Layer],
[FabLength],
[Fab Desc1] = case when ((Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#],[Style#],[Fab Desc],[Colorway],[Color],[Fab Desc]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]))then '' else [Fab Desc] end,
--[Shift]=[Shift],
[Remark] = [Remark],
[SCI Delivery] = [SCI Delivery],
[total_qty1] = [total_qty]
from #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]

drop table #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                }
            }
            #endregion

            #region radioBtnByCutCell
            if (this.radioByOneDayDetial.Checked)
            {
                for (int i = 0; i < sheetsCount; i++)
                {
                    sqlCmd.Append(string.Format("IF OBJECT_ID('tempdb.dbo.#tmpall{0}','U')IS NOT NULL DROP TABLE #tmpall{0}", i));
                    sqlCmd.Append(@"
select	distinct
	[Request#] = Cutplan.ID,
	[Fab ETA] = fe.ETA,
	[Line#] = cd.SewingLineID,
	[SP#] = cd.OrderID,
	[Seq#] = CONCAT(wofp.Seq1, '-', wofp.Seq2),
	[Style#] = o.StyleID,
    [FabRef#] = wofp.Refno,
	[Ref#] = cd.CutRef,
	[Cut#] = cd.CutNo,
	[Comb.] = wofp.FabricCombo,
	[Fab_Code] = fab.fab,
	[Size Ratio] = sr.SizeCode,
	[Colorway] = woda.AC,
	[Color] = cd.ColorID,
	[Cut Qty] = cq.SizeCode,
    [OrderQty] = SizeQty.SizeCode,
	[ExcessQty] = ExcessQty.SizeCode,
	[Fab Cons.] = cd.Cons,
    [Layer] = wofp.Layer,
    [Length] = iif(wofp.Layer = 0, '', STR(cd.Cons / wofp.Layer, 12, 2) ),
	[Fab Refno] = FabRefno.Refno,
    --[Shift]=wofp.Shift,
	[Remark] = cd.Remark,
	[SCI Delivery] = o.SciDelivery,
	[ms] = ms.Seq,    
    [total_qty] = sum(wofps.qty * wofp.Layer) over(partition by cd.WorkOrderForPlanningUkey)
into #tmpall");
                    sqlCmd.Append(string.Format("{0}", i));
                    sqlCmd.Append(@"
from Cutplan WITH (NOLOCK) 
inner join Cutting cut on Cutplan.CuttingID = cut.ID
inner join Cutplan_Detail cd WITH (NOLOCK) on Cutplan.ID = cd.ID
inner join WorkOrderForPlanning wofp WITH (NOLOCK) on cd.WorkOrderForPlanningUkey = wofp.Ukey and cd.ID = wofp.CutplanID
inner join WorkOrderForPlanning_SizeRatio wofps WITH (NOLOCK) on cd.WorkOrderForPlanningUkey = wofps.WorkOrderForPlanningUkey
left join Orders o WITH (NOLOCK) on o.ID = cd.OrderID
outer apply(
    select AC= 
       STUFF((
       Select distinct concat('/', wd.Article)
       from WorkOrderForPlanning_Distribute wd WITH (NOLOCK) 
       where WorkOrderForPlanningUKey = cd.WorkOrderForPlanningUKey and Article != ''
       for xml path('')
    ),1,1,'')
) as woda
outer apply(
	select ETA = iif(FinalETA='',iif(RevisedETA='',CfmETA,RevisedETA),FinalETA)
	from PO_Supp_Detail WITH (NOLOCK) 
	where PO_Supp_Detail.ID = cd.POID 
	and PO_Supp_Detail.Seq1 = wofp.Seq1 
	and PO_Supp_Detail.Seq2 = wofp.Seq2 
) as fe
outer apply(
	select fab = 
	STUFF((
		select concat('+',wp.PatternPanel)
		from WorkOrderForPlanning_PatternPanel wp
		where wp.WorkOrderForPlanningUkey = wofp.Ukey
		for xml path('')
	),1,1,'')
) fab
outer apply(
	select SizeCode = 
	STUFF((
		Select concat(',',(ws.SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) 
		left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		where WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUKey
		ORDER BY Order_SizeCode.Seq
		for xml path('')
	 ),1,1,'')
) as sr
outer apply(
    select SizeCode = 
        STUFF((
            Select concat(',',(ws.SizeCode+'/'+Convert(varchar, sum(wd.Qty)))) 
            from WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) 
            left join WorkOrderForPlanning_Distribute wd on ws.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey 
                and  ws.SizeCode=wd.SizeCode
            where ws.WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUKey
            and wd.OrderID !='EXCESS'
            group by ws.SizeCode
        for xml path('')
    ),1,1,'')
) as SizeQty
outer apply(
	select SizeCode= 
	STUFF((
		Select concat(',',SizeCode+'/'+Convert(varchar,Qty*(select Layer from WorkOrderForPlanning WITH (NOLOCK) where UKey = cd.WorkOrderForPlanningUKey))) 
		from WorkOrderForPlanning_SizeRatio  WITH (NOLOCK) where WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUKey
		for xml path('')
	 ),1,1,'')
) as cq
outer apply(
	select PO_Supp_Detail.RefNo
	from PO_Supp_Detail WITH (NOLOCK) 
	where PO_Supp_Detail.ID = cd.POID
	and PO_Supp_Detail.Seq1 = wofp.Seq1 
	and PO_Supp_Detail.Seq2 = wofp.Seq2
) as FabRefno
outer apply(
	select Seq= (
		Select min(Seq)
		from WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) 
		left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		where WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUKey
	 )
) as ms
outer apply(
    select SizeCode = 
        STUFF((
            Select concat(',',(ws.SizeCode+'/'+Convert(varchar, sum(wd.Qty)))) 
            from WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) 
            left join WorkOrderForPlanning_Distribute wd on ws.WorkOrderForPlanningUkey=wd.WorkOrderForPlanningUkey and ws.SizeCode=wd.SizeCode
            where ws.WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUKey
            and wd.OrderID ='EXCESS'
            group by ws.SizeCode
        for xml path('')
    ),1,1,'')
) as ExcessQty
where 1 = 1 --??? AND fe.ETA IS NOT NULL
");
                    if (!MyUtility.Check.Empty(this.dateR_CuttingDate1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.EstCutdate = '{0}' ", Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd")));
                    }

                    if (!MyUtility.Check.Empty(this.MD))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", this.MD));
                    }

                    if (!MyUtility.Check.Empty(this.Factory))
                    {
                        sqlCmd.Append(string.Format(" and cut.FactoryID = '{0}' ", this.Factory));
                    }

                    if (!MyUtility.Check.Empty(this.CutCellFrom) || !MyUtility.Check.Empty(this.CutCellTo))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = '{0}' ", this.Maintb.Rows[i][0].ToString()));
                    }

                    if (!MyUtility.Check.Empty(this.SpreadingNo1) || !MyUtility.Check.Empty(this.SpreadingNo2))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.SpreadingNoID = '{0}' ", this.Maintb.Rows[i][0].ToString()));
                    }

                    sqlCmd.Append(@"order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[ms] desc,[Seq#]");

                    sqlCmd.Append(@"
select 
[Request#1] = case when ((Row_number() over (partition by [Line#],[Request#]
	order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)) >1 
    and [SP#] = LAG([SP#],1,[SP#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)) then '' else [Request#] end,
[Fab ETA1] = case when ((Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#] 
	order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)) >1  
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc))then '' else Convert(varchar,[Fab ETA]) end,
[Line#1] = case when ((Row_number() over (partition by [Line#],[Request#]
	order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)) >1)
     and [SP#] = LAG([SP#],1,[SP#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)then '' else [Line#] end,
[SP#1] = case when  ((Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#] 
	order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)) >1 
    and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc))then '' else [SP#] end ,
[Seq#1] = case when ((Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#] 
	order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)) >1 
    and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc))then '' else [Seq#] end,
[Style#1] = case when ((Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#],[Style#] 
	order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc)) >1 
    and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc))then '' else [Style#] end,
[FabRef#] = [FabRef#],
[Ref#] = [Ref#],
[Cut#] = [Cut#],
[Comb.] = [Comb.],
[Fab_Code] = [Fab_Code],
[Size Ratio] = [Size Ratio],
[Colorway1] = case when ((Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#] ,[Seq#],[Style#],[Colorway] 
	order by [Line#],[Fab ETA],[Fab ETA],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
    and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc))then '' else [Colorway] end,
[Color1] = case when ((Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#],[Style#],[Colorway],[Color]
	order by [Line#],[Fab ETA],[Fab ETA],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
    and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc))then '' else [Color] end,
[Cut Qty] = [Cut Qty],
[OrderQty] = [OrderQty],
[ExcessQty] = [ExcessQty],
[Fab Cons.] = [Fab Cons.],
[Layer] = [Layer],
[Length] = [Length],
--[Shift]=[Shift],
[Remark] = [Remark],
[SCI Delivery] = [SCI Delivery],
[total_qty1] = [total_qty]
from #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
order by [Line#],[Request#],[SP#],[Seq#],[Fab ETA],[Comb.],[ms] desc
drop table #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                }
            }
            #endregion

            #region radiobtn By Summary
            if (this.radioBySummary.Checked)
            {
                for (int i = 0; i < sheetsCount; i++)
                {
                    sqlCmd.Append($@"
select distinct
	[Request#] = c.ID,
	[Factory]=o.FactoryID,
	[Line#] = cd.SewingLineID,
	[SP#] = cd.OrderID,
	o.SewInline,
	[Seq#] = Concat(w.Seq1, '-', w.Seq2),
	[Style#] = o.StyleID,
	[FabRef#] = pd.RefNo,
	[Fab Desc] =[Production].dbo.getMtlDesc(c.POID, w.Seq1, w.Seq2,2,0),
	[Color] = cd.ColorID,
	[Comb.] = w.FabricCombo,
	[Fab_Code] = fab.fab,
	[Cut#] = cutno.cutno,
	[Size Ratio] =sr.SizeCode,
	[Cut Qty] = cq.SizeCode,
	[Colorway] = woda.AC,
	[Total Fab Cons] =sum(cd.Cons) over(partition by c.ID,cd.SewingLineID,cd.OrderID,w.Seq1,w.Seq2,w.FabricCombo),
    --[Shift]=w.Shift,
	[Remark] = Remark.Remark,
	p.patternUKey,
	w.FabricPanelCode
into #tmp{i}
from Cutplan c WITH (NOLOCK)
inner join Cutting cut on c.CuttingID = cut.ID 
inner join Cutplan_Detail cd WITH (NOLOCK) on c.ID = cd.ID
inner join WorkOrderForPlanning w WITH (NOLOCK) on cd.WorkOrderForPlanningUkey = w.Ukey
inner join Orders o WITH (NOLOCK) on o.ID = cd.OrderID
inner join PO_Supp_Detail pd WITH (NOLOCK) on pd.ID = cd.POID and pd.Seq1 = w.Seq1 and pd.Seq2 = w.Seq2
outer apply(
select AC =
    stuff((
        Select distinct concat('/', wd.Article)
        from Cutplan_Detail cd2 WITH (NOLOCK)
        inner join WorkOrderForPlanning w2 WITH (NOLOCK) on cd2.WorkOrderForPlanningUkey = w2.Ukey
        inner join WorkOrderForPlanning_Distribute wd WITH (NOLOCK) on wd.WorkOrderForPlanningUKey = cd2.WorkOrderForPlanningUKey
        where wd.Article != ''
        and cd2.ID = c.ID
        and cd2.SewingLineID = cd.SewingLineID--一個外層一個內層
        and cd2.OrderID = cd.OrderID--一個外層一個內層
        and w.SEQ1 = w2.SEQ1--一個外層一個內層
        and w.SEQ2 = w2.SEQ2--一個外層一個內層
        for xml path('')
    ),1,1,'')
) as woda
outer apply(
	select fab =
	stuff((
		select concat('+',wp.PatternPanel)
		from WorkOrderForPlanning_PatternPanel wp
		where wp.WorkOrderForPlanningUkey = w.Ukey
		for xml path('')
	),1,1,'')
) fab
outer apply(
select cutno = 
	stuff((
		Select distinct concat('/', cd2.CutNo)
		from Cutplan_Detail cd2 WITH (NOLOCK) 
		inner join WorkOrderForPlanning w2 on cd2.WorkOrderForPlanningUkey = w2.Ukey
		where cd2.ID = c.ID
		and cd2.SewingLineID = cd.SewingLineID
		and cd2.OrderID = cd.OrderID
		and w2.SEQ1 = w.SEQ1 
		and w2.SEQ2 = w.SEQ2
		for xml path('')
	),1,1,'')
) as cutno
outer apply(
	select SizeCode = 
	stuff((
		Select distinct concat(',',(SizeCode+'/'+ Convert(varchar,Qty))) 
		from Cutplan_Detail cd2 WITH (NOLOCK) 		
		inner join WorkOrderForPlanning w2 on cd2.WorkOrderForPlanningUkey = w2.Ukey
		inner join WorkOrderForPlanning_SizeRatio ws2 WITH (NOLOCK) on cd2.WorkOrderForPlanningUKey = ws2.WorkOrderForPlanningUkey
		where cd2.ID = c.ID
		and cd2.SewingLineID = cd.SewingLineID
		and cd2.OrderID = cd.OrderID
		and w2.SEQ1 = w.SEQ1 
		and w2.SEQ2 = w.SEQ2		 	
		for xml path('')
	),1,1,'')
) as sr
outer apply(
select SizeCode = 
stuff((
		Select distinct concat(',',ws2.SizeCode+'/'+ Convert(varchar,ws2.Qty * w2.Layer)) 
		from Cutplan_Detail cd2 WITH (NOLOCK)
		inner join WorkOrderForPlanning w2 WITH (NOLOCK) on cd2.WorkOrderForPlanningUkey = w2.Ukey
		inner join WorkOrderForPlanning_SizeRatio ws2 WITH (NOLOCK) on cd2.WorkOrderForPlanningUKey = ws2.WorkOrderForPlanningUkey
		where cd2.ID = c.ID
		and cd2.SewingLineID = cd.SewingLineID
		and cd2.OrderID = cd.OrderID
		and w2.SEQ1 = w.SEQ1 
		and w2.SEQ2 = w.SEQ2
		for xml path('')
	),1,1,'')
) as cq
outer apply(
	select remark =stuff((
		select concat(char(10),Remark)
		from Cutplan_Detail cd2 WITH (NOLOCK) 
		where cd2.ID = c.ID and cd2.SewingLineID = cd.Sewinglineid and cd2.OrderID = cd.OrderID
		for xml path('')
	),1,1,'')
)remark
outer apply(
	SELECT TOP 1 SizeGroup=IIF(ISNULL(SizeGroup,'')='','N',SizeGroup)
	FROM Order_SizeCode 
	WHERE ID = o.POID and SizeCode IN 
	(
		Select distinct SizeCode
		from Cutplan_Detail cd2 WITH (NOLOCK) 		
		inner join WorkOrderForPlanning w2 on cd2.WorkOrderForPlanningUkey = w2.Ukey
		inner join WorkOrderForPlanning_SizeRatio ws2 WITH (NOLOCK) on cd2.WorkOrderForPlanningUKey = ws2.WorkOrderForPlanningUkey
		where cd2.ID = c.ID
		and cd2.SewingLineID = cd.SewingLineID
		and cd2.OrderID = cd.OrderID
		and w2.SEQ1 = w.SEQ1 
		and w2.SEQ2 = w.SEQ2
	)
) as ss
outer apply(select p.PatternUkey from dbo.GetPatternUkey(o.POID,'',w.MarkerNo,o.StyleUkey,ss.SizeGroup) p)p

where 1 = 1
");
                    if (!MyUtility.Check.Empty(this.dateR_CuttingDate1))
                    {
                        sqlCmd.Append(string.Format(" and c.EstCutdate >= '{0}' ", Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd")));
                    }

                    if (!MyUtility.Check.Empty(this.dateR_CuttingDate2))
                    {
                        sqlCmd.Append(string.Format(" and c.EstCutdate <= '{0}' ", Convert.ToDateTime(this.dateR_CuttingDate2).ToString("yyyy/MM/dd")));
                    }

                    if (!MyUtility.Check.Empty(this.MD))
                    {
                        sqlCmd.Append(string.Format(" and c.MDivisionID ='{0}' ", this.MD));
                    }

                    if (!MyUtility.Check.Empty(this.Factory))
                    {
                        sqlCmd.Append(string.Format(" and cut.FactoryID = '{0}' ", this.Factory));
                    }

                    if (!MyUtility.Check.Empty(this.CutCellFrom) || !MyUtility.Check.Empty(this.CutCellTo))
                    {
                        sqlCmd.Append(string.Format(" and c.CutCellID = '{0}' ", this.Maintb.Rows[i][0].ToString()));
                    }

                    if (!MyUtility.Check.Empty(this.SpreadingNo1) || !MyUtility.Check.Empty(this.SpreadingNo2))
                    {
                        sqlCmd.Append(string.Format(" and c.SpreadingNoID = '{0}' ", this.Maintb.Rows[i][0].ToString()));
                    }

                    sqlCmd.Append($@"
select 	
	[Request#],[Factory],[Line#],[SP#],[SewInline],[Seq#],[Style#],[FabRef#],[Fab Desc],[Color],Artwork.Artwork
	,[Comb.],[Fab_Code],[Cut#],[Size Ratio],[Cut Qty],[Colorway],[Total Fab Cons],[Remark]
from #tmp{i} t
outer apply(
	select Artwork=stuff((
	select distinct concat('+',s.data)
	from(
		select distinct pg.Annotation
		from Pattern_GL_LectraCode pgl
		inner join Pattern_GL pg on pgl.PatternUKEY = pg.PatternUKEY
									and pgl.seq = pg.SEQ
									and pg.Annotation is not null
									and pg.Annotation!=''
		where pgl.PatternUKEY = t.patternUKey and pgl.FabricPanelCode = t.FabricPanelCode
	)a
	outer apply(select data=RTRIM(LTRIM(data)) from SplitString(dbo.[RemoveNumericCharacters](a.Annotation),'+'))s
	where exists(select 1 from SubProcess where id = s.data)
	for xml path(''))
	,1,1,'')
)Artwork
order by [Request#],Factory,[Line#],[SP#],[Seq#]
drop table #tmp{i}
");
                }
            }
            #endregion

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            this.cuttings = new string[sheetsCount];
            for (int i = 0; i < sheetsCount; i++)
            {
                this.cuttings[i] = this.printData[i].Rows.Count.ToString() + " cuttings";
            }
            #region Bydetail OR Byonedaydetial OR Byonedaydetial依狀況插入列
            if (this.radioByDetail.Checked || this.radioByOneDayDetial.Checked)
            {
                for (int i = 0; i < this.printData.Count(); i++)
                {
                    int l = 0, q = 0;
                    decimal dm = 0, dsum = 0;
                    DataTable tmps = new DataTable();
                    tmps = this.printData[i].Copy();
                    this.printData[i].Clear();
                    for (int j = 0; j < tmps.Rows.Count; j++)
                    {
                        int.TryParse(tmps.Rows[j]["total_qty1"].ToString(), out q);
                        l += q;
                        decimal.TryParse(tmps.Rows[j]["Fab Cons."].ToString(), out dm);
                        dsum += dm;
                        DataRow drr = this.printData[i].NewRow();
                        drr = tmps.Rows[j];
                        this.printData[i].ImportRow(drr);

                        // 做到倒數第二row
                        if (j < tmps.Rows.Count - 1)
                        {
                            // 若下個SP#有值則塞row
                            if (!tmps.Rows[j + 1]["SP#1"].Empty())
                            {
                                DataRow tabrow = this.printData[i].NewRow();
                                tabrow["Colorway1"] = "Total Cut Qty";
                                tabrow["Color1"] = l;

                                // tabrow[13] = "Total Cons.";//此欄在Datatable是Decimal無法放入string
                                tabrow["Fab Cons."] = dsum;
                                this.printData[i].Rows.Add(tabrow);

                                l = 0;
                                dm = 0;
                                dsum = 0;
                            }
                        }

                        // 若到最後一row塞row
                        if (j == tmps.Rows.Count - 1)
                        {
                            DataRow tabrow = this.printData[i].NewRow();
                            tabrow["Colorway1"] = "Total Cut Qty";
                            tabrow["Color1"] = l;

                            // tabrow[13] = "Total Cons.";
                            tabrow["Fab Cons."] = dsum;
                            this.printData[i].Rows.Add(tabrow);
                        }
                    }

                    this.printData[i].Columns.RemoveAt(this.printData[i].Columns.Count - 1);
                }
            }

            #endregion
            #region By summary
            if (this.radioBySummary.Checked)
            {
                for (int i = 0; i < this.printData.Count(); i++)
                {
                    // int l = 0, q = 0;
                    decimal dm = 0, dsum = 0;
                    DataTable tmps = new DataTable();
                    tmps = this.printData[i].Copy();
                    this.printData[i].Clear();
                    for (int j = 0; j < tmps.Rows.Count; j++)
                    {
                        decimal.TryParse(tmps.Rows[j]["Total Fab Cons"].ToString(), out dm);
                        dsum += dm;
                        DataRow drr = this.printData[i].NewRow();
                        drr = tmps.Rows[j];
                        this.printData[i].ImportRow(drr);

                        string id = MyUtility.Convert.GetString(tmps.Rows[j]["Request#"]);
                        string sp = MyUtility.Convert.GetString(tmps.Rows[j]["SP#"]);
                        string seq = MyUtility.Convert.GetString(tmps.Rows[j]["Seq#"]);

                        string id2 = string.Empty;
                        string sp2 = string.Empty;
                        string seq2 = string.Empty;
                        if (j < tmps.Rows.Count - 1)
                        {
                            id2 = MyUtility.Convert.GetString(tmps.Rows[j + 1]["Request#"]);
                            sp2 = MyUtility.Convert.GetString(tmps.Rows[j + 1]["SP#"]);
                            seq2 = MyUtility.Convert.GetString(tmps.Rows[j + 1]["Seq#"]);
                        }

                        if (id != id2 || sp != sp2 || seq != seq2)
                        {
                            DataRow tabrow = this.printData[i].NewRow();
                            tabrow["Colorway"] = "Subtotal";
                            tabrow["Total Fab Cons"] = dsum;
                            this.printData[i].Rows.Add(tabrow);

                            dm = 0;
                            dsum = 0;
                        }
                    }

                    this.printData[i].Columns.RemoveAt(this.printData[i].Columns.Count - 1);
                }
            }
            #endregion
            return Ict.Result.True;
        }

        private bool boolsend = false;

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData[0].Rows.Count);

            int cutCellcount = this.Maintb.Rows.Count; // CutCel總數
            bool countrow = false;
            for (int i = 0; i < cutCellcount; i++)
            {
                if (this.printData[i].Rows.Count > 0)
                {
                    countrow = true;
                }
            }

            if (!countrow)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            #region radiobtn_Bydetail
            if (this.radioByDetail.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx"); // 預先開啟excel app
                objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
                objApp.Cells[1, 1] = this.NameEN;

                // 先準備複製幾頁
                for (int i = 0; i < cutCellcount; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                        worksheet1.Copy(worksheet1);
                    }
                }

                for (int i = 0; i < cutCellcount; i++)
                {
                    if (this.printData[i].Rows.Count == 0)
                    {
                        continue;
                    }

                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表
                    MyUtility.Excel.CopyToXls(this.printData[i], null, "Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false); // 將datatable copy to excel

                    foreach (DataRow dr in this.printData[i].Rows)
                    {
                        dr["Fab Desc1"] = dr["Fab Desc1"].ToString().Trim();
                    }

                    for (int j = 0; j < this.printData[i].Rows.Count; j++)
                    {
                        if (!this.printData[i].Rows[j]["Request#1"].Empty())
                        {
                            objSheets.get_Range("A" + (8 + j), "B" + (8 + j)).Merge(false); // 合併欄位
                            objSheets.get_Range("A" + (8 + j), "A" + (8 + j)).Font.Bold = true; // 指定粗體
                            if (!MyUtility.Check.Empty(this.printData[i].Rows[j]["SCI Delivery"]))
                            {
                                objSheets.Cells[7 + j, 1] = "SCI Delivery: " + Convert.ToDateTime(this.printData[i].Rows[j]["SCI Delivery"]).ToString("yyyy/MM/dd");
                            }
                        }

                        if (this.printData[i].Rows[j]["Ref#"].Empty())
                        {
                            objSheets.get_Range("M" + (6 + j), "P" + (6 + j)).Font.Bold = true; // 指定粗體
                            objSheets.Cells[6 + j, 14] = "Total Cons.";
                        }
                    }

                    objSheets.Columns["U"].Clear();
                    objSheets.Name = (this.selected_splitWorksheet == "CutCell" ? "Cell" : "SpreadingNo") + this.Maintb.Rows[i][0].ToString(); // 工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd") + "~" + Convert.ToDateTime(this.dateR_CuttingDate2).ToString("yyyy/MM/dd"); // 查詢日期
                    objSheets.Cells[3, 5] = this.selected_splitWorksheet == "CutCell" ? "Cut Cell" : "Spreading No"; // CutCell或SpreadingNo
                    objSheets.Cells[3, 6] = this.Maintb.Rows[i][0].ToString(); // CutCell或SpreadingNo
                    objSheets.Cells[3, 9] = this.MD;
                    objSheets.Cells[4, 1] = this.cuttings[i];
                    objSheets.Columns.AutoFit();
                    objSheets.get_Range("A1").ColumnWidth = 14.25;
                    objSheets.get_Range("B1").ColumnWidth = 14;
                    objSheets.get_Range("C1").ColumnWidth = 7.88;
                    objSheets.get_Range("D1").ColumnWidth = 14.75;
                    objSheets.get_Range("E1").ColumnWidth = 13;
                    objSheets.get_Range("F1").ColumnWidth = 15.25;
                    objSheets.get_Range("G1").ColumnWidth = 12;
                    objSheets.get_Range("H1").ColumnWidth = 8.75;
                    objSheets.get_Range("I1").ColumnWidth = 7.38;
                    objSheets.get_Range("J1").ColumnWidth = 9;
                    objSheets.get_Range("K1").ColumnWidth = 12.13;
                    objSheets.get_Range("L1").ColumnWidth = 11.75;
                    objSheets.get_Range("M1").ColumnWidth = 14;
                    objSheets.get_Range("N1").ColumnWidth = 8.13;
                    objSheets.get_Range("O1").ColumnWidth = 12.25;
                    objSheets.get_Range("P1").ColumnWidth = 12.13;
                    objSheets.get_Range("Q1").ColumnWidth = 12.13;
                    objSheets.get_Range("R1").ColumnWidth = 20.3;
                    objSheets.get_Range("T1").ColumnWidth = 50;
                    objSheets.get_Range("U1").ColumnWidth = 41;
                    objSheets.Rows.AutoFit();

                    Marshal.ReleaseComObject(objSheets); // 釋放sheet
                }

                #region Save Excel
                this.strExcelName = Class.MicrosoftFile.GetName("Cutting_R02_CuttingDailyPlanSummaryReportBydetail");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(this.strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(workbook);
                #endregion
                if (!this.boolsend)
                {
                    this.strExcelName.OpenFile();
                }
            }
            #endregion

            #region radioBtn_Byonedaydetial
            if (this.radioByOneDayDetial.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx"); // 預先開啟excel app
                objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
                objApp.Cells[1, 1] = this.NameEN;

                // 先準備複製幾頁
                for (int i = 0; i < cutCellcount; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                        worksheet1.Copy(worksheet1);
                    }
                }

                for (int i = 0; i < cutCellcount; i++)
                {
                    if (this.printData[i].Rows.Count == 0)
                    {
                        continue;
                    }

                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表
                    MyUtility.Excel.CopyToXls(this.printData[i], null, "Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false); // 將datatable copy to excel

                    for (int j = 0; j < this.printData[i].Rows.Count; j++)
                    {
                        if (!this.printData[i].Rows[j]["Request#1"].Empty())
                        {
                            objSheets.get_Range("A" + (8 + j), "B" + (8 + j)).Merge(false); // 合併欄位
                            objSheets.get_Range("A" + (8 + j), "A" + (8 + j)).Font.Bold = true; // 指定粗體
                            if (!MyUtility.Check.Empty(this.printData[i].Rows[j]["SCI Delivery"]))
                            {
                                objSheets.Cells[7 + j, 1] = "SCI Delivery: " + Convert.ToDateTime(this.printData[i].Rows[j]["SCI Delivery"]).ToString("yyyy/MM/dd");
                            }
                        }

                        if (this.printData[i].Rows[j]["Ref#"].Empty())
                        {
                            objSheets.get_Range("M" + (6 + j), "R" + (6 + j)).Font.Bold = true; // 指定粗體
                            objSheets.Cells[6 + j, 16] = "Total Cons.";
                        }
                    }

                    objSheets.Columns["V"].Clear();
                    objSheets.Name = (this.selected_splitWorksheet == "CutCell" ? "Cell" : "SpreadingNo") + this.Maintb.Rows[i][0].ToString(); // 工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd"); // 查詢日期
                    objSheets.Cells[3, 5] = this.selected_splitWorksheet == "CutCell" ? "Cut Cell" : "Spreading No"; // CutCell或SpreadingNo
                    objSheets.Cells[3, 6] = this.Maintb.Rows[i][0].ToString(); // CutCell或SpreadingNo
                    objSheets.Cells[3, 9] = this.MD;
                    objSheets.Cells[4, 1] = this.cuttings[i];
                    objSheets.get_Range("A1").ColumnWidth = 15.75;
                    objSheets.get_Range("B1").ColumnWidth = 11.75;
                    objSheets.get_Range("C1").ColumnWidth = 8.25;
                    objSheets.get_Range("D1").ColumnWidth = 15.38;
                    objSheets.get_Range("E1").ColumnWidth = 13;
                    objSheets.get_Range("F1").ColumnWidth = 15.25;
                    objSheets.get_Range("G1").ColumnWidth = 8.75;
                    objSheets.get_Range("H1").ColumnWidth = 12;
                    objSheets.get_Range("I1").ColumnWidth = 7.38;
                    objSheets.get_Range("J1").ColumnWidth = 9;
                    objSheets.get_Range("K1").ColumnWidth = 11.88;
                    objSheets.get_Range("L1").ColumnWidth = 12.38;
                    objSheets.get_Range("M1").ColumnWidth = 13.5;
                    objSheets.get_Range("N1").ColumnWidth = 8;
                    objSheets.get_Range("O1").ColumnWidth = 12.75;
                    objSheets.get_Range("P1").ColumnWidth = 12.75;
                    objSheets.get_Range("Q1").ColumnWidth = 12.75;
                    objSheets.get_Range("R1").ColumnWidth = 12;
                    objSheets.get_Range("S1").ColumnWidth = 9.25;
                    objSheets.get_Range("T1").ColumnWidth = 21.38;
                    objSheets.get_Range("V1").ColumnWidth = 12.88;
                    objSheets.get_Range("W1").ColumnWidth = 41;
                    objSheets.Rows.AutoFit();

                    Marshal.ReleaseComObject(objSheets); // 釋放sheet
                }

                #region Save Excel
                this.strExcelName = Class.MicrosoftFile.GetName("Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(this.strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(workbook);
                #endregion
                if (!this.boolsend)
                {
                    this.strExcelName.OpenFile();
                }
            }
            #endregion

            #region radioBtn_BySUMMY
            if (this.radioBySummary.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx"); // 預先開啟excel app
                Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx", objApp);
                objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
                objApp.Cells[1, 1] = this.NameEN;

                // 先準備複製幾頁
                for (int i = 0; i < cutCellcount; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                        worksheet1.Copy(worksheet1);
                    }
                }

                for (int i = 0; i < cutCellcount; i++)
                {
                    if (this.printData[i].Rows.Count == 0)
                    {
                        continue;
                    }

                    foreach (DataRow dr in this.printData[i].Rows)
                    {
                        dr["Fab Desc"] = dr["Fab Desc"].ToString().Trim();
                    }

                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表
                    ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Sheets[i + 1]).Select();

                    // MyUtility.Excel.CopyToXls(printData[i], null, "Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false);//將datatable copy to excel
                    com.WriteTable(this.printData[i], 6);

                    objSheets.Name = (this.selected_splitWorksheet == "CutCell" ? "Cell" : "SpreadingNo") + this.Maintb.Rows[i][0].ToString(); // 工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd") + "~" + Convert.ToDateTime(this.dateR_CuttingDate2).ToString("yyyy/MM/dd"); // 查詢日期
                    objSheets.Cells[3, 7] = this.selected_splitWorksheet == "Cut Cell" ? "Cut" : "Spreading No"; // CutCell或SpreadingNo
                    objSheets.Cells[3, 8] = this.Maintb.Rows[i][0].ToString(); // CutCell或SpreadingNo
                    objSheets.Cells[3, 11] = this.MD;

                    // objSheets.Columns.AutoFit();
                    objSheets.Columns[7].ColumnWidth = 47;
                    objSheets.Columns[11].ColumnWidth = 8;
                    objSheets.Columns[12].ColumnWidth = 13;
                    objSheets.Columns[13].ColumnWidth = 15;
                    objSheets.Columns[14].ColumnWidth = 10;
                    objSheets.Columns[15].ColumnWidth = 20;
                    objSheets.Columns[16].ColumnWidth = 41;
                    Marshal.ReleaseComObject(objSheets);    // 釋放sheet
                }
                #region Save Excel
                this.strExcelName = Class.MicrosoftFile.GetName("Cutting_R02_CuttingDailyPlanSummaryReportBySummary");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(this.strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(workbook);
                #endregion
                if (!this.boolsend)
                {
                    this.strExcelName.OpenFile();
                }
            }
            #endregion

            if (this.boolsend)
            {
                this.Send_Mail();
            }

            this.boolsend = false;
            return true;
        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            this.boolsend = true;
            this.toexcel.PerformClick();
        }

        private void Send_Mail()
        {
            StringBuilder cuttingDate = new StringBuilder();
            StringBuilder cutcell = new StringBuilder();
            cuttingDate.Clear();
            if (!MyUtility.Check.Empty(this.dateR_CuttingDate1))
            {
                cuttingDate.Append(string.Format(@"{0}", Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateR_CuttingDate2))
            {
                cuttingDate.Append(string.Format(@"~{0}", Convert.ToDateTime(this.dateR_CuttingDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.CutCellFrom))
            {
                cutcell.Append(string.Format(@"{0}", this.CutCellFrom));
            }

            if (!MyUtility.Check.Empty(this.CutCellTo))
            {
                cutcell.Append(string.Format(@"~{0}", this.CutCellTo));
            }

            string mailcmd = "select * from mailto WITH (NOLOCK) where id = '005'";
            DataTable maildt;
            DBProxy.Current.Select(null, mailcmd, out maildt);
            string toAddress = MyUtility.Convert.GetString(maildt.Rows[0]["ToAddress"]);
            string ccAddress = MyUtility.Convert.GetString(maildt.Rows[0]["CcAddress"]);
            string subject = MyUtility.Convert.GetString(maildt.Rows[0]["Subject"]) + "-" + cuttingDate;

            var email = new MailTo(
                Env.Cfg.MailFrom,
                toAddress,
                ccAddress,
                subject,
                this.strExcelName,
                "\r\nFilter as below description:\r\nCutting Date: " + cuttingDate + "\r\nCut Cell: " + cutcell + "\r\nM: " + this.MD,
                false,
                true);
            email.ShowDialog(this);

            // tmpFile
            System.IO.FileInfo fi = new System.IO.FileInfo(this.strExcelName);
            try
            {
                fi.Delete();
            }
            catch (System.IO.IOException e)
            {
                MyUtility.Msg.ErrorBox(e.Message);
            }
        }

        private void SetComboFactory()
        {
            string sqlCmd = string.Format(
                @"
select '' ID
union
Select Distinct ID 
from Factory
where   junk = 0 
        and MDivisionID = '{0}'", this.comboM.Text);
            DataTable factory;
            DBProxy.Current.Select(null, sqlCmd, out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = string.Empty;
        }

        private void SetCutCell()
        {
            this.txtCellFrom.Text = string.Empty;
            this.txtCellTo.Text = string.Empty;
            this.txtCellFrom.MDivisionID = this.comboM.Text;
            this.txtCellTo.MDivisionID = this.comboM.Text;
        }

        private void ComboM_TextChanged(object sender, EventArgs e)
        {
            this.SetComboFactory();
            this.SetCutCell();
        }

        private void RadioGroup1_Paint(object sender, PaintEventArgs e)
        {
            // 把GroupBox的框線改成透明
            GroupBox box = sender as GroupBox;
            this.DrawGroupBox(box, e.Graphics, Color.Transparent, Color.Transparent);
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(
                    box.ClientRectangle.X,
                    box.ClientRectangle.Y + (int)(strSize.Height / 2),
                    box.ClientRectangle.Width - 1,
                    box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                // Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));

                // Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));

                // Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));

                // Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));

                // Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)strSize.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
    }
}
