using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.IO;
namespace Sci.Production.Cutting
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DataTable[] printData;
        string MD, Factory, CutCell1, CutCell2;
        string[] cuttings;
        DateTime? dateR_CuttingDate1, dateR_CuttingDate2;
        StringBuilder condition_CuttingDate = new StringBuilder();
        DataTable Cutcelltb;
        string NameEN, strExcelName;

        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable WorkOrder;
            //Set ComboM
            DBProxy.Current.Select(null, @"Select Distinct MDivisionID from WorkOrder WITH (NOLOCK) ", out WorkOrder);
            MyUtility.Tool.SetupCombox(comboM, 1, WorkOrder);
            comboM.Text = Sci.Env.User.Keyword;
            
            //Set ComboFactory
            this.setComboFactory();
            comboFactory.Text = Sci.Env.User.Factory;
        }

        private void radioByOneDayDetial_CheckedChanged(object sender, EventArgs e)
        {
            dateCuttingDate.Control2.Visible = !radioByOneDayDetial.Checked;
            if (radioByOneDayDetial.Checked)
                dateCuttingDate.Control2.Text = "";
            else
                dateCuttingDate.Value2 = dateCuttingDate.Value1;
        }

        private void dateCuttingDate_Leave(object sender, EventArgs e)
        {
            if (radioBySummary.Checked || radioByDetail.Checked)
                if (MyUtility.Check.Empty(dateCuttingDate.Value2))
                    dateCuttingDate.Value2 = dateCuttingDate.Value1;
        }
        
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateCuttingDate.Value1))
            {
                MyUtility.Msg.WarningBox("CuttingDate can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(txtCutCellStart.Text.Trim()))
            {
                MyUtility.Msg.WarningBox("CutCell can't empty!!");
                return false;
            }

            Factory = comboFactory.Text;
            MD = comboM.Text;
            dateR_CuttingDate1 = dateCuttingDate.Value1;
            dateR_CuttingDate2 = dateCuttingDate.Value2;

            //select distinct cutcellid from cutplan order by cutcellid 不只有數字,where條件要''單引號,且mask是00
            int c1, c2;
            bool bc1, bc2;
            bc1 = int.TryParse(txtCutCellStart.Text.Trim(), out c1);
            if (bc1) CutCell1 = c1.ToString("D2");
            else CutCell1 = txtCutCellStart.Text.Trim();
            //若CutCell2為空則=CutCell1
            if (!MyUtility.Check.Empty(txtCutCellEnd.Text.Trim()))
            {
                bc2 = int.TryParse(txtCutCellEnd.Text.Trim(), out c2);
                if (bc2) CutCell2 = c2.ToString("D2");
                else CutCell2 = txtCutCellEnd.Text.Trim();
            }
            else
            {
                CutCell2 = CutCell1;
            }

            return base.ValidateInput();
        }

        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            NameEN = MyUtility.GetValue.Lookup("NameEN", Sci.Env.User.Factory, "Factory ", "id");
            //準備CutCell包含非數字
            string scell;
            if (radioByDetail.Checked || radioBySummary.Checked)
            {
                scell = string.Format(@"
select  distinct CutCellID 
from Cutplan WITH (NOLOCK) 
inner join Cutting on CutPlan.CuttingID = Cutting.ID
where   Cutplan.EstCutdate >= '{0}' 
        and Cutplan.EstCutdate <= '{1}'
        and Cutplan.MDivisionID ='{2}' 
        and Cutplan.CutCellID >= '{3}' 
        and Cutplan.CutCellID <='{4}' 
        {5}
order by CutCellID"
, Convert.ToDateTime(dateR_CuttingDate1).ToString("d")
, Convert.ToDateTime(dateR_CuttingDate2).ToString("d")
, MD
, CutCell1
, CutCell2
, (Factory.Empty() ? "" : string.Format("and Cutting.FactoryID = '{0}'", Factory)));
            }else{
                scell = string.Format(@"
select  distinct CutCellID 
from Cutplan WITH (NOLOCK) 
inner join Cutting on CutPlan.CuttingID = Cutting.ID
where   Cutplan.EstCutdate = '{0}'
        and Cutplan.MDivisionID ='{1}' 
        and Cutplan.CutCellID >= '{2}' 
        and Cutplan.CutCellID <='{3}' 
        {4}
order by CutCellID"
, Convert.ToDateTime(dateR_CuttingDate1).ToString("d")
, MD
, CutCell1
, CutCell2
, (Factory.Empty() ? "" : string.Format("and Cutting.FactoryID = '{0}'", Factory)));
            }

            DBProxy.Current.Select(null, scell, out Cutcelltb);
            
            int CutCellcount = Cutcelltb.Rows.Count;//CutCel總數

            if (CutCellcount == 0)
                return Result.F("Data not found!");

            StringBuilder sqlCmd = new StringBuilder();

            #region radiobtnByM
            if (radioByDetail.Checked)
            {
                for (int i = 0; i < CutCellcount; i++)
                {
                    sqlCmd.Append(string.Format("IF OBJECT_ID('tempdb.dbo.#tmpall{0}','U')IS NOT NULL DROP TABLE #tmpall{0}", i));
                    sqlCmd.Append(@"
select distinct
	[Request#] = Cutplan.ID,
	[Cutting Date] = Cutplan.EstCutdate,
	[Line#] = Cutplan_Detail.SewingLineID,
	[SP#] = Cutplan_Detail.OrderID,
	[Seq#] = CONCAT(WorkOrder.Seq1, '-', WorkOrder.Seq2),
	[Style#] = o.StyleID,
	[Ref#] = Cutplan_Detail.CutRef,
	[Cut#] = Cutplan_Detail.CutNo,
	[Comb.] = WorkOrder.FabricCombo,
	[Fab_Code] = fab.fab,
	[Size Ratio] = sr.SizeCode,
	[Colorway] = woda.ac,
	[Color] = Cutplan_Detail.ColorID,
	[Cut Qty] = cq.SizeCode,
	[Fab Cons.] = Cutplan_Detail.Cons,
	[Fab Desc] = [Production].dbo.getMtlDesc(Cutplan_Detail.POID, WorkOrder.Seq1, WorkOrder.Seq2,2,0),
	[Remark] = Cutplan_Detail.Remark,
	[SCI Delivery] = o.SciDelivery,
	[ms] = ms.Seq,
    [total_qty] = sum(WorkOrder_SizeRatio.qty * WorkOrder.Layer) over(partition by Cutplan_Detail.WorkOrderUkey)
into #tmpall");
                    sqlCmd.Append(string.Format("{0}", i));
                    sqlCmd.Append(@"
from Cutplan WITH (NOLOCK) 
inner join Cutting cut on Cutplan.CuttingID = cut.ID
inner join Cutplan_Detail WITH (NOLOCK) on Cutplan.ID = Cutplan_Detail.ID
inner join WorkOrder WITH (NOLOCK) on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey and Cutplan_Detail.ID = WorkOrder.CutplanID
inner join WorkOrder_SizeRatio WITH (NOLOCK) on Cutplan_Detail.WorkOrderUkey = WorkOrder_SizeRatio.WorkOrderUkey
left join Orders o WITH (NOLOCK) on o.ID = Cutplan_Detail.OrderID
outer apply(
	select fab = 
	STUFF((
		select concat('+',wp.PatternPanel)
		from WorkOrder_PatternPanel wp
		where wp.WorkOrderUkey = WorkOrder.Ukey
		for xml path('')
	),1,1,'')
) fab
outer apply(
	select SizeCode = 
	STUFF((
		Select concat(',',(ws.SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrder_SizeRatio ws WITH (NOLOCK) 
		left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		ORDER BY Order_SizeCode.Seq
		for xml path('')
	 ),1,1,'')
) as sr
outer apply(
	 select AC= 
	 STUFF((
		 Select distinct concat('/', wod.Article)
		 from WorkOrder_Distribute wod WITH (NOLOCK) 
		 where WorkOrderUKey = Cutplan_Detail.WorkOrderUKey and Article != ''
		 for xml path('')
	 ),1,1,'')
) as woda
outer apply(
	select SizeCode= 
	STUFF((
		Select concat(',',SizeCode+'/'+Convert(varchar,Qty*(select Layer from WorkOrder WITH (NOLOCK) where UKey = Cutplan_Detail.WorkOrderUKey))) 
		from WorkOrder_SizeRatio  WITH (NOLOCK) where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		for xml path('')
	 ),1,1,'')
) as cq
outer apply(
	select Seq= (
		Select min(Order_SizeCode.Seq)
		from WorkOrder_SizeRatio ws WITH (NOLOCK) 
		left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
	 )
) as ms
where 1 = 1
");
                    if (!MyUtility.Check.Empty(dateR_CuttingDate1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.EstCutdate >= '{0}' ", Convert.ToDateTime(dateR_CuttingDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(dateR_CuttingDate2))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.EstCutdate <= '{0}' ", Convert.ToDateTime(dateR_CuttingDate2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(MD))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", MD));
                    }
                    if (!MyUtility.Check.Empty(Factory))
                    {
                        sqlCmd.Append(string.Format(" and cut.FactoryID = '{0}' ", Factory));
                    }
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = '{0}' ", Cutcelltb.Rows[i][0].ToString()));
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
[Fab Desc1] = case when ((Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#],[Style#],[Fab Desc],[Colorway],[Color],[Fab Desc]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#])) >1 
	and	[Seq#] = lag([Seq#],1,[Seq#]) over(order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[ms] desc,[Seq#]))then '' else [Fab Desc] end,
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
            if (radioByOneDayDetial.Checked)
            {
                for (int i = 0; i < CutCellcount; i++)
                {
                    sqlCmd.Append(string.Format("IF OBJECT_ID('tempdb.dbo.#tmpall{0}','U')IS NOT NULL DROP TABLE #tmpall{0}", i));
                    sqlCmd.Append(@"
select	distinct
	[Request#] = Cutplan.ID,
	[Fab ETA] = fe.ETA,
	[Line#] = Cutplan_Detail.SewingLineID,
	[SP#] = Cutplan_Detail.OrderID,
	[Seq#] = CONCAT(WorkOrder.Seq1, '-', WorkOrder.Seq2),
	[Style#] = o.StyleID,
	[Ref#] = Cutplan_Detail.CutRef,
	[Cut#] = Cutplan_Detail.CutNo,
	[Comb.] = WorkOrder.FabricCombo,
	[Fab_Code] = fab.fab,
	[Size Ratio] = sr.SizeCode,
	[Colorway] = woda.ac,
	[Color] = Cutplan_Detail.ColorID,
	[Cut Qty] = cq.SizeCode,
    [OrderQty] = SizeQty.SizeCode,
	[ExcessQty] = ExcessQty.SizeCode,
	[Fab Cons.] = Cutplan_Detail.Cons,
	[Fab Refno] = FabRefno.Refno,
	[Remark] = Cutplan_Detail.Remark,
	[SCI Delivery] = o.SciDelivery,
	[ms] = ms.Seq,    
    [total_qty] = sum(WorkOrder_SizeRatio.qty * WorkOrder.Layer) over(partition by Cutplan_Detail.WorkOrderUkey)
into #tmpall");
                    sqlCmd.Append(string.Format("{0}", i));
                    sqlCmd.Append(@"
from Cutplan WITH (NOLOCK) 
inner join Cutting cut on Cutplan.CuttingID = cut.ID
inner join Cutplan_Detail WITH (NOLOCK) on Cutplan.ID = Cutplan_Detail.ID
inner join WorkOrder WITH (NOLOCK) on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey and Cutplan_Detail.ID = WorkOrder.CutplanID
inner join WorkOrder_SizeRatio WITH (NOLOCK) on Cutplan_Detail.WorkOrderUkey = WorkOrder_SizeRatio.WorkOrderUkey
left join Orders o WITH (NOLOCK) on o.ID = Cutplan_Detail.OrderID
outer apply(
	select ETA = iif(FinalETA='',iif(RevisedETA='',CfmETA,RevisedETA),FinalETA)
	from PO_Supp_Detail WITH (NOLOCK) 
	where PO_Supp_Detail.ID = Cutplan_Detail.POID 
	and PO_Supp_Detail.Seq1 = WorkOrder.Seq1 
	and PO_Supp_Detail.Seq2 = WorkOrder.Seq2 
) as fe
outer apply(
	select fab = 
	STUFF((
		select concat('+',wp.PatternPanel)
		from WorkOrder_PatternPanel wp
		where wp.WorkOrderUkey = WorkOrder.Ukey
		for xml path('')
	),1,1,'')
) fab
outer apply(
	select SizeCode = 
	STUFF((
		Select concat(',',(ws.SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrder_SizeRatio ws WITH (NOLOCK) 
		left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		ORDER BY Order_SizeCode.Seq
		for xml path('')
	 ),1,1,'')
) as sr
outer apply(
	select SizeCode = 
	STUFF((
		Select concat(',',(ws.SizeCode+'/'+Convert(varchar,wd.Qty))) 
		from WorkOrder_SizeRatio ws WITH (NOLOCK) 
		left join Workorder_distribute wd on ws.WorkOrderUkey=wd.WorkOrderUkey and ws.SizeCode=wd.SizeCode
		where ws.WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		and wd.OrderID !='EXCESS'
		for xml path('')
	 ),1,1,'')
) as SizeQty
outer apply(
	select SizeCode = 
	STUFF((
		Select concat(',',(ws.SizeCode+'/'+Convert(varchar,wd.Qty))) 
		from WorkOrder_SizeRatio ws WITH (NOLOCK) 
		left join Workorder_distribute wd on ws.WorkOrderUkey=wd.WorkOrderUkey and ws.SizeCode=wd.SizeCode
		where ws.WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		and wd.OrderID ='EXCESS'
		for xml path('')
	 ),1,1,'')
) as ExcessQty
outer apply(
	 select AC= 
	 STUFF((
		 Select distinct concat('/', wod.Article)
		 from WorkOrder_Distribute wod WITH (NOLOCK) 
		 where WorkOrderUKey = Cutplan_Detail.WorkOrderUKey and Article != ''
		 for xml path('')
	 ),1,1,'')
) as woda
outer apply(
	select SizeCode= 
	STUFF((
		Select concat(',',SizeCode+'/'+Convert(varchar,Qty*(select Layer from WorkOrder WITH (NOLOCK) where UKey = Cutplan_Detail.WorkOrderUKey))) 
		from WorkOrder_SizeRatio  WITH (NOLOCK) where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		for xml path('')
	 ),1,1,'')
) as cq
outer apply(
	select PO_Supp_Detail.RefNo
	from PO_Supp_Detail WITH (NOLOCK) 
	where PO_Supp_Detail.ID = Cutplan_Detail.POID
	and PO_Supp_Detail.Seq1 = WorkOrder.Seq1 
	and PO_Supp_Detail.Seq2 = WorkOrder.Seq2
) as FabRefno
outer apply(
	select Seq= (
		Select min(Seq)
		from WorkOrder_SizeRatio ws WITH (NOLOCK) 
		left join Order_SizeCode on ws.ID = Order_SizeCode.ID AND ws.SizeCode = Order_SizeCode.SizeCode
		where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
	 )
) as ms
where 1 = 1 --??? AND fe.ETA IS NOT NULL
");
                    if (!MyUtility.Check.Empty(dateR_CuttingDate1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.EstCutdate = '{0}' ", Convert.ToDateTime(dateR_CuttingDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(MD))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", MD));
                    }
                    if (!MyUtility.Check.Empty(Factory))
                    {
                        sqlCmd.Append(string.Format(" and cut.FactoryID = '{0}' ", Factory));
                    }
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = '{0}' ", Cutcelltb.Rows[i][0].ToString()));
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
[Fab Refno] = [Fab Refno],
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

            #region radiobtn By Detail3
            if (radioBySummary.Checked)
            {
                for (int i = 0; i < CutCellcount; i++)
                {
                    sqlCmd.Append(@"
select distinct
	[Request#] = c.ID,
	[Line#] = cd.SewingLineID,
	[SP#] = cd.OrderID,
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
	[Colorway] = woda.ac,
	[Total Fab Cons] =sum(cd.Cons) over(partition by c.ID,cd.SewingLineID,cd.OrderID,w.Seq1,w.Seq2,w.FabricCombo),
	[Remark] = Remark.Remark
from Cutplan c WITH (NOLOCK)
inner join Cutting cut on c.CuttingID = cut.ID 
inner join Cutplan_Detail cd WITH (NOLOCK) on c.ID = cd.ID
inner join WorkOrder w WITH (NOLOCK) on cd.WorkOrderUkey = w.Ukey
inner join Orders o WITH (NOLOCK) on o.ID = cd.OrderID
inner join PO_Supp_Detail pd WITH (NOLOCK) on pd.ID = cd.POID and pd.Seq1 = w.Seq1 and pd.Seq2 = w.Seq2
outer apply(
	select fab =
	stuff((
		select concat('+',wp.PatternPanel)
		from WorkOrder_PatternPanel wp
		where wp.WorkOrderUkey = w.Ukey
		for xml path('')
	),1,1,'')
) fab
outer apply(
select cutno = 
	stuff((
		Select distinct concat('/', cd2.CutNo)
		from Cutplan_Detail cd2 WITH (NOLOCK) inner join WorkOrder w2 on cd2.WorkorderUkey = w2.Ukey
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
		inner join WorkOrder w2 on cd2.WorkorderUkey = w2.Ukey
		inner join WorkOrder_SizeRatio ws2 WITH (NOLOCK) on cd2.WorkOrderUKey = ws2.WorkOrderUkey
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
		inner join WorkOrder w2 WITH (NOLOCK) on cd2.WorkorderUkey = w2.Ukey
		inner join WorkOrder_SizeRatio ws2 WITH (NOLOCK) on cd2.WorkOrderUKey = ws2.WorkOrderUkey
		where cd2.ID = c.ID
		and cd2.SewingLineID = cd.SewingLineID
		and cd2.OrderID = cd.OrderID
		and w2.SEQ1 = w.SEQ1 
		and w2.SEQ2 = w.SEQ2
		for xml path('')
	),1,1,'')
) as cq
outer apply(
select AC =
	stuff((
		Select distinct concat('/', wd2.Article)
		from Cutplan_Detail cd2 WITH (NOLOCK)
		inner join WorkOrder w2 WITH (NOLOCK) on cd2.WorkorderUkey = w2.Ukey
		inner join WorkOrder_Distribute wd2 WITH (NOLOCK) on wd2.WorkOrderUKey = cd2.WorkOrderUKey
		where wd2.Article != ''
		and cd2.ID = c.ID
		and cd2.SewingLineID = cd.SewingLineID
		and cd2.OrderID = cd.OrderID
		and w2.SEQ1 = w.SEQ1
		and w2.SEQ2 = w.SEQ2
		for xml path('')
	),1,1,'')
) as woda
outer apply(
	select remark =stuff((
		select concat(char(10),Remark)
		from Cutplan_Detail cd2 WITH (NOLOCK) 
		where cd2.ID = c.ID and cd2.SewingLineID = cd.Sewinglineid and cd2.OrderID = cd.OrderID
		for xml path('')
	),1,1,'')
)remark
where 1 = 1
");
                    if (!MyUtility.Check.Empty(dateR_CuttingDate1))
                    {
                        sqlCmd.Append(string.Format(" and c.EstCutdate >= '{0}' ", Convert.ToDateTime(dateR_CuttingDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(dateR_CuttingDate2))
                    {
                        sqlCmd.Append(string.Format(" and c.EstCutdate <= '{0}' ", Convert.ToDateTime(dateR_CuttingDate2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(MD))
                    {
                        sqlCmd.Append(string.Format(" and c.MDivisionID ='{0}' ", MD));
                    }
                    if (!MyUtility.Check.Empty(Factory))
                    {
                        sqlCmd.Append(string.Format(" and cut.FactoryID = '{0}' ", Factory));
                    }
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and c.CutCellID = '{0}' ", Cutcelltb.Rows[i][0].ToString()));
                    }
                    sqlCmd.Append(" order by [Request#],[Line#],[SP#],[Seq#]");
                }
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            cuttings = new string[CutCellcount];
            for (int i = 0; i < CutCellcount; i++)
            {
                cuttings[i] = printData[i].Rows.Count.ToString() + " cuttings";                
            }
            #region Bydetail OR Byonedaydetial OR Byonedaydetial依狀況插入列
            if (radioByDetail.Checked || radioByOneDayDetial.Checked)
            {
                for (int i = 0; i < printData.Count(); i++)
                {
                    int l = 0,q =0;
                    decimal dm = 0, dsum = 0;
                    DataTable tmps = new DataTable();
                    tmps = printData[i].Copy();
                    printData[i].Clear();
                    for (int j = 0; j < tmps.Rows.Count; j++)
                    {
                        int.TryParse(tmps.Rows[j]["total_qty1"].ToString(), out q);
                        l += q;
                        decimal.TryParse(tmps.Rows[j]["Fab Cons."].ToString(), out dm);
                        dsum += dm;
                        DataRow drr = printData[i].NewRow();
                        drr = tmps.Rows[j];
                        printData[i].ImportRow(drr);

                        //做到倒數第二row
                        if (j < tmps.Rows.Count - 1)
                        {
                            //若下個SP#有值則塞row
                            if (!tmps.Rows[j + 1]["SP#1"].Empty())
                            {
                                DataRow tabrow = printData[i].NewRow();
                                tabrow["Colorway1"] = "Total Cut Qty";
                                tabrow["Color1"] = l;
                                //tabrow[13] = "Total Cons.";//此欄在Datatable是Decimal無法放入string
                                tabrow["Fab Cons."] = dsum;
                                printData[i].Rows.Add(tabrow);

                                l = 0;
                                dm = 0;
                                dsum = 0;
                            }
                        }
                        //若到最後一row塞row
                        if (j == tmps.Rows.Count - 1)
                        {
                            DataRow tabrow = printData[i].NewRow();
                            tabrow["Colorway1"] = "Total Cut Qty";
                            tabrow["Color1"] = l;
                            //tabrow[13] = "Total Cons.";
                            tabrow["Fab Cons."] = dsum;
                            printData[i].Rows.Add(tabrow);
                        }
                    }
                    printData[i].Columns.RemoveAt(printData[i].Columns.Count-1);
                }
            }
            
            #endregion
            
            return Result.True;
        }

        bool boolshowexcel = false;
        bool boolsend = false;
        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            SetCount(printData[0].Rows.Count);            
            
            int CutCellcount = Cutcelltb.Rows.Count;//CutCel總數
            bool countrow = false;
            for (int i = 0; i < CutCellcount; i++)
            {
                if (printData[i].Rows.Count > 0)
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
            if (radioByDetail.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
                objApp.Cells[1, 1] = NameEN;
                //先準備複製幾頁
                for (int i = 0; i < CutCellcount; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                        worksheet1.Copy(worksheetn);
                    }
                }

                for (int i = 0; i < CutCellcount; i++)
                {
                    if (printData[i].Rows.Count == 0)
                        continue;
                    
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表
                    MyUtility.Excel.CopyToXls(printData[i], null, "Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false);//將datatable copy to excel

                    foreach (DataRow dr in printData[i].Rows)
                    {
                        dr["Fab Desc1"] = dr["Fab Desc1"].ToString().Trim();
                    }
                    
                    for (int j = 0; j < printData[i].Rows.Count; j++)
                    {
                        if (!printData[i].Rows[j]["Request#1"].Empty())
                        {
                            objSheets.get_Range("A" + (7 + j), "B" + (7 + j)).Merge(false);//合併欄位
                            objSheets.get_Range("A" + (7 + j), "A" + (7 + j)).Font.Bold = true;//指定粗體
                            if (!MyUtility.Check.Empty(printData[i].Rows[j]["SCI Delivery"]))
                                objSheets.Cells[7 + j, 1] = "SCI Delivery: " + Convert.ToDateTime(printData[i].Rows[j]["SCI Delivery"]).ToString("d");
                        }

                        if (printData[i].Rows[j]["Ref#"].Empty())
                        {
                            objSheets.get_Range("L" + (6 + j), "O" + (6 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[6 + j, 14] = "Total Cons.";
                        }
                    }
                    objSheets.Columns["R"].Clear();
                    objSheets.Name = "Cell" + (Cutcelltb.Rows[i][0].ToString());//工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d") + "~" + Convert.ToDateTime(dateR_CuttingDate2).ToString("d"); //查詢日期
                    objSheets.Cells[3, 6] = (Cutcelltb.Rows[i][0].ToString());//cutcellID
                    objSheets.Cells[3, 9] = MD;
                    objSheets.Cells[4, 1] = cuttings[i];
                    objSheets.get_Range("A1").ColumnWidth = 14.25;
                    objSheets.get_Range("B1").ColumnWidth = 14;
                    objSheets.get_Range("C1").ColumnWidth = 7.88;
                    objSheets.get_Range("D1").ColumnWidth = 14.75;
                    objSheets.get_Range("E1").ColumnWidth = 8.5;
                    objSheets.get_Range("F1").ColumnWidth = 15.25;
                    objSheets.get_Range("G1").ColumnWidth = 8.75;
                    objSheets.get_Range("H1").ColumnWidth = 7.38;
                    objSheets.get_Range("I1").ColumnWidth = 9;
                    objSheets.get_Range("J1").ColumnWidth = 12.13;
                    objSheets.get_Range("K1").ColumnWidth = 11.75;
                    objSheets.get_Range("L1").ColumnWidth = 14;
                    objSheets.get_Range("M1").ColumnWidth = 8.13;
                    objSheets.get_Range("N1").ColumnWidth = 12.25;
                    objSheets.get_Range("O1").ColumnWidth = 12.13;
                    objSheets.get_Range("P1").ColumnWidth = 50;
                    objSheets.get_Range("Q1").ColumnWidth = 41;
                    objSheets.Rows.AutoFit();

                    Marshal.ReleaseComObject(objSheets); //釋放sheet                    
                }

                #region Save Excel
                strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R02_CuttingDailyPlanSummaryReportBydetail");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(workbook);
                #endregion 
                if (!boolsend)
                {
                    strExcelName.OpenFile();
                }
            }
            #endregion

            #region radioBtn_Byonedaydetial
            if (radioByOneDayDetial.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
                objApp.Cells[1, 1] = NameEN;
                //先準備複製幾頁
                for (int i = 0; i < CutCellcount; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                        worksheet1.Copy(worksheetn);
                    }
                }

                for (int i = 0; i < CutCellcount; i++)
                {
                    if (printData[i].Rows.Count == 0)
                        continue;

                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表                    
                    MyUtility.Excel.CopyToXls(printData[i], null, "Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false);//將datatable copy to excel

                    for (int j = 0; j < printData[i].Rows.Count; j++)
                    {
                        if (!printData[i].Rows[j]["Request#1"].Empty())
                        {
                            objSheets.get_Range("A" + (7 + j), "B" + (7 + j)).Merge(false);//合併欄位
                            objSheets.get_Range("A" + (7 + j), "A" + (7 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[7 + j, 1] = "SCI Delivery: " + Convert.ToDateTime(printData[i].Rows[j]["SCI Delivery"]).ToString("d");
                        }

                        if (printData[i].Rows[j]["Ref#"].Empty())
                        {
                            objSheets.get_Range("L" + (6 + j), "Q" + (6 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[6 + j, 16] = "Total Cons.";
                        }
                    }
                    objSheets.Columns["T"].Clear();
                    objSheets.Name = "Cell" + (Cutcelltb.Rows[i][0].ToString());//工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d"); //查詢日期
                    objSheets.Cells[3, 6] = (Cutcelltb.Rows[i][0].ToString());//cutcellID
                    objSheets.Cells[3, 9] = MD;
                    objSheets.Cells[4, 1] = cuttings[i];
                    objSheets.get_Range("A1").ColumnWidth = 15.75;
                    objSheets.get_Range("B1").ColumnWidth = 11.75;
                    objSheets.get_Range("C1").ColumnWidth = 8.25;
                    objSheets.get_Range("D1").ColumnWidth = 15.38;
                    objSheets.get_Range("E1").ColumnWidth = 9.13;
                    objSheets.get_Range("F1").ColumnWidth = 15.25;
                    objSheets.get_Range("G1").ColumnWidth = 8.75;
                    objSheets.get_Range("H1").ColumnWidth = 7.38;
                    objSheets.get_Range("I1").ColumnWidth = 9;
                    objSheets.get_Range("J1").ColumnWidth = 11.88;
                    objSheets.get_Range("K1").ColumnWidth = 12.38;
                    objSheets.get_Range("L1").ColumnWidth = 13.5;
                    objSheets.get_Range("M1").ColumnWidth = 8;
                    objSheets.get_Range("N1").ColumnWidth = 12.75;
                    objSheets.get_Range("O1").ColumnWidth = 12.75;
                    objSheets.get_Range("P1").ColumnWidth = 12.75;
                    objSheets.get_Range("Q1").ColumnWidth = 12;
                    objSheets.get_Range("R1").ColumnWidth = 12.88;
                    objSheets.get_Range("S1").ColumnWidth = 41;
                    objSheets.Rows.AutoFit();
                    
                    Marshal.ReleaseComObject(objSheets); //釋放sheet                     
                }

                #region Save Excel
                strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(workbook);
                #endregion 
                if (!boolsend)
                {
                    strExcelName.OpenFile();
                }
            }
            #endregion

            #region radioBtn_BySUMMY
            if (radioBySummary.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx"); //預先開啟excel app
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx", objApp);
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
                objApp.Cells[1, 1] = NameEN;
                //先準備複製幾頁
                for (int i = 0; i < CutCellcount; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                        worksheet1.Copy(worksheetn);
                    }
                }

                for (int i = 0; i < CutCellcount; i++)
                {
                    if (printData[i].Rows.Count == 0)
                        continue;
                    foreach (DataRow dr in printData[i].Rows)
                    {
                        dr["Fab Desc"] = dr["Fab Desc"].ToString().Trim();
                    }

                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表  
                    ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Sheets[i + 1]).Select();
                    //MyUtility.Excel.CopyToXls(printData[i], null, "Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false);//將datatable copy to excel
                    
                    com.WriteTable(printData[i],6);


                    objSheets.Name = "Cell" + (Cutcelltb.Rows[i][0].ToString());//工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d") + "~" + Convert.ToDateTime(dateR_CuttingDate2).ToString("d"); //查詢日期
                    objSheets.Cells[3, 6] = (Cutcelltb.Rows[i][0].ToString());//cutcellID
                    objSheets.Cells[3, 9] = MD;
                    //objSheets.Columns.AutoFit();
                    objSheets.Columns[7].ColumnWidth = 47;
                    objSheets.Columns[11].ColumnWidth = 8;
                    objSheets.Columns[12].ColumnWidth = 13;
                    objSheets.Columns[13].ColumnWidth = 15;
                    objSheets.Columns[14].ColumnWidth = 10;
                    objSheets.Columns[15].ColumnWidth = 20;
                    objSheets.Columns[16].ColumnWidth = 41;
                    Marshal.ReleaseComObject(objSheets);    //釋放sheet                    
                }
                #region Save Excel
                strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R02_CuttingDailyPlanSummaryReportBySummary");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(workbook);
                #endregion 
                if (!boolsend)
                {
                    strExcelName.OpenFile();
                }
            }
            #endregion

            if (boolsend)
            {
                Send_Mail();
            }

            boolsend = false;
            return true;
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            boolshowexcel = false;
            boolsend = true;
            this.toexcel.PerformClick();            
        }

        private void Send_Mail()
        {
            StringBuilder CuttingDate = new StringBuilder();
            StringBuilder cutcell = new StringBuilder();
            CuttingDate.Clear();
            if (!MyUtility.Check.Empty(dateR_CuttingDate1))
            {
                CuttingDate.Append(string.Format(@"{0}", Convert.ToDateTime(dateR_CuttingDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateR_CuttingDate2))
            {
                CuttingDate.Append(string.Format(@"~{0}", Convert.ToDateTime(dateR_CuttingDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CutCell1))
            {
                cutcell.Append(string.Format(@"{0}", CutCell1));
            }
            if (!MyUtility.Check.Empty(CutCell2))
            {
                cutcell.Append(string.Format(@"~{0}", CutCell2));
            }
            string mailcmd = "select * from mailto WITH (NOLOCK) where id = '005'";
            DataTable maildt;
            DBProxy.Current.Select(null, mailcmd, out maildt);
            string ToAddress = MyUtility.Convert.GetString(maildt.Rows[0]["ToAddress"]);
            string CcAddress = MyUtility.Convert.GetString(maildt.Rows[0]["CcAddress"]);
            string Subject = MyUtility.Convert.GetString(maildt.Rows[0]["Subject"]) +"-"+ CuttingDate;

            var email = new MailTo(Sci.Env.Cfg.MailFrom, ToAddress, CcAddress,
                Subject,
                this.strExcelName,
                "\r\nFilter as below description:\r\nCutting Date: " + CuttingDate + "\r\nCut Cell: " + cutcell + "\r\nM: " + MD, false, true);
            email.ShowDialog(this);
            //tmpFile
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

        private void setComboFactory()
        {
            string sqlCmd = string.Format(@"
select '' ID
union
Select Distinct ID 
from Factory
where   junk = 0 
        and MDivisionID = '{0}'", comboM.Text);
            DataTable Factory;
            DBProxy.Current.Select(null, sqlCmd, out Factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, Factory);
            comboFactory.Text = "";
        }

        private void comboM_TextChanged(object sender, EventArgs e)
        {
            this.setComboFactory();
        }
    }
}
