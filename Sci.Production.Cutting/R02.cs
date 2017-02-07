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
        string MD, CutCell1, CutCell2;
        DateTime? dateR_CuttingDate1, dateR_CuttingDate2;
        StringBuilder condition_CuttingDate = new StringBuilder();
        string tmpFile;
        bool boolshowexcel = false;
        bool boolsend = false;
        DataTable Cutcelltb;

        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable WorkOrder;
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrder", out WorkOrder);
            MyUtility.Tool.SetupCombox(cmb_MDivisionID, 1, WorkOrder);
            cmb_MDivisionID.Text = Sci.Env.User.Keyword;
            createfolder();
        }         

        private void radiobtn_Bydetail_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_Bydetail.Checked)
            {
                dateR_CuttingDate.Control2.Visible = true;
                if (MyUtility.Check.Empty(dateR_CuttingDate.Value2))
                {
                    dateR_CuttingDate.Value2 = dateR_CuttingDate.Value1;
                }
            }
        }

        private void radioBtn_Byonedaydetial_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_Byonedaydetial.Checked)
            {
                dateR_CuttingDate.Control2.Text = "";
                dateR_CuttingDate.Control2.Visible = false;

            }
        }

        private void radiobtn_BySummary_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_BySummary.Checked)
            {
                dateR_CuttingDate.Control2.Visible = true;
                if (MyUtility.Check.Empty(dateR_CuttingDate.Value2))
                {
                    dateR_CuttingDate.Value2 = dateR_CuttingDate.Value1;
                }
            }
        }

        private void Leave_CuttingDate(object sender, EventArgs e)
        {
            if (radiobtn_BySummary.Checked || radiobtn_Bydetail.Checked)
            {
                dateR_CuttingDate.Control2.Visible = true;
                if (MyUtility.Check.Empty(dateR_CuttingDate.Value2))
                {
                    dateR_CuttingDate.Value2 = dateR_CuttingDate.Value1;
                }
            }
        }
        
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateR_CuttingDate.Value1) && MyUtility.Check.Empty(dateR_CuttingDate.Value2))
            {
                MyUtility.Msg.WarningBox("CuttingDate can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(txt_CutCell1.Text.Trim()))
            {
                MyUtility.Msg.WarningBox("CutCell can't empty!!");
                return false;
            }

            MD = cmb_MDivisionID.Text;
            dateR_CuttingDate1 = dateR_CuttingDate.Value1;
            if (dateR_CuttingDate.Value2 == null)
            {
                dateR_CuttingDate2 = dateR_CuttingDate1;
            }
            else
            {
                dateR_CuttingDate2 = dateR_CuttingDate.Value2;
            }

            //select distinct cutcellid from cutplan order by cutcellid 不只數字,where條件要''單引號,且mask是00
            int c1, c2;
            bool bc1, bc2;
            bc1 = int.TryParse(txt_CutCell1.Text.Trim(), out c1);
            if (bc1) CutCell1 = c1.ToString("D2");
            else CutCell1 = txt_CutCell1.Text.Trim();
            //若CutCell2為空則=CutCell1
            if (!MyUtility.Check.Empty(txt_CutCell2.Text.Trim()))
            {
                bc2 = int.TryParse(txt_CutCell2.Text.Trim(), out c2);
                if (bc2) CutCell2 = c2.ToString("D2");
                else CutCell2 = txt_CutCell2.Text.Trim();
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
            //準備CutCell包含非數字
            DBProxy.Current.Select(null, string.Format(@"select distinct CutCellID from Cutplan 
where Cutplan.EstCutdate >= '{0}' and Cutplan.EstCutdate <= '{1}' 
and Cutplan.MDivisionID ='{2}' and Cutplan.CutCellID >= '{3}' and Cutplan.CutCellID <='{4}' order by CutCellID"
                ,Convert.ToDateTime(dateR_CuttingDate1).ToString("d")
                ,Convert.ToDateTime(dateR_CuttingDate2).ToString("d")
                ,MD, CutCell1, CutCell2), out Cutcelltb);
            
            int CutCellcount = Cutcelltb.Rows.Count;//CutCel總數

            if (CutCellcount == 0)
                return Result.F("Data not found!");

            StringBuilder sqlCmd = new StringBuilder();

            #region radiobtnByM
            if (radiobtn_Bydetail.Checked)
            {
                for (int i = 0; i < CutCellcount; i++)
                {
                    sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
', 'U') IS NOT NULL
  DROP TABLE #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
select distinct
	[Request#] = Cutplan.ID,
	[Cutting Date] = Cutplan.EstCutdate,
	[Line#] = Cutplan_Detail.SewingLineID,
	[SP#] = Cutplan_Detail.OrderID,
	[Seq#] = WorkOrder.Seq1 + '-' + WorkOrder.Seq2,
	[Style#] = o.StyleID,
	[Ref#] = Cutplan_Detail.CutRef,
	[Cut#] = Cutplan_Detail.CutNo,
	[Comb.] = WorkOrder.FabricCombo,
	[Fab_Code] = WorkOrder.FabricCode,
	[Size Ratio] = stuff(sr.SizeCode,1,1,''),
	[Colorway] = stuff(woda.ac,1,1,''),
	[Color] = Cutplan_Detail.ColorID,
	[Cut Qty] = stuff(cq.SizeCode,1,1,''),
	[Fab Cons.] = Cutplan_Detail.Cons,
	[Fab Desc] = [Production].dbo.getMtlDesc(Cutplan_Detail.POID, WorkOrder.Seq1, WorkOrder.Seq2,2,0),
	[Remark] = Cutplan_Detail.Remark,
	WS1 = WorkOrder.Seq1,
	WS2 = WorkOrder.Seq2,
	[SCI Delivery] = SCI.SciDelivery,
	[CutCellID] = Cutplan.CutCellID,
	Cutplan_Detail.WorkOrderUkey,
	WorkOrder.Ukey,
	WorkOrder.Layer
into #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
from Cutplan 
inner join Cutplan_Detail on Cutplan.ID = Cutplan_Detail.ID
inner join WorkOrder on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey and Cutplan_Detail.ID = WorkOrder.CutplanID
inner join WorkOrder_SizeRatio on Cutplan_Detail.WorkOrderUkey = WorkOrder_SizeRatio.WorkOrderUkey
left join Order_SizeCode on Order_SizeCode.ID = (select DISTINCT POID from Orders where Orders.CuttingSP = WorkOrder_SizeRatio.ID) 
					      and (Order_SizeCode.SizeCode = WorkOrder_SizeRatio.SizeCode)
outer apply(
	select Orders.StyleID 
	from Orders 
	where Orders.ID = Cutplan_Detail.OrderID
) as o
outer apply(
	select SizeCode= (
		Select concat(',',(SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrder_SizeRatio
		where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		for xml path('')
	 )
) as sr
outer apply(
	 select AC= (
		 Select distinct concat('/', wod.Article)
		 from WorkOrder_Distribute wod
		 where WorkOrderUKey = Cutplan_Detail.WorkOrderUKey and Article != ''
		 for xml path('')
	 )
) as woda
outer apply(
	select SizeCode= (
		Select concat(',',SizeCode+'/'+Convert(varchar,Qty*(select Layer from WorkOrder where UKey = Cutplan_Detail.WorkOrderUKey))) 
		from WorkOrder_SizeRatio where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		for xml path('')
	 )
) as cq
outer apply(
	select Orders.SCIDelivery
	from Orders
	where Orders.ID = Cutplan_Detail.OrderID
) as SCI

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
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = '{0}' ", Cutcelltb.Rows[i][0].ToString()));
                    }

                    sqlCmd.Append(@"order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#]");

                    sqlCmd.Append(@"
select [Request#],[Cutting Date],[Line#],[SP#],[Seq#],[Style#],[Ref#],[Cut#],[Comb.],[Fab_Code],[Size Ratio],[Colorway],[Color],[Cut Qty],[Fab Cons.],[Fab Desc],[Remark],WS1,WS2,[SCI Delivery],[CutCellID]
,[total_qty] = sum(x.total_qty)
into #tmpall2");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
from #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@" a
outer apply(
	Select [total_qty] = c.qty * a.layer From WorkOrder_SizeRatio c Where  c.WorkOrderUkey = a.WorkOrderUkey and c.WorkOrderUkey = a.Ukey
) x 
group by [Request#],[Cutting Date],[Line#],[SP#],[Seq#],[Style#],[Ref#],[Cut#],[Comb.],[Fab_Code],[Size Ratio],[Colorway],[Color],[Cut Qty],[Fab Cons.],[Fab Desc],[Remark],WS1,WS2,[SCI Delivery],[CutCellID]
drop table #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"

select 
[Request#1]= case when (Row_number() over (partition by [Line#],[Request#]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Request#] end,
[Cutting Date1] = case when (Row_number() over (partition by [Line#],[Request#],[Cutting Date]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1  then '' else Convert(varchar,[Cutting Date]) end,
[Line#1] = case when ((Row_number() over (partition by [Line#]
	order by [Line#] ,[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1) then '' else [Line#] end,
[SP#1] = case when  (Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [SP#] end ,
[Seq#1] = case when (Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Seq#] end,
[Style#1] = case when (Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#],[Style#] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Style#] end,
[Ref#] = [Ref#],
[Cut#] = [Cut#],
[Comb.] = [Comb.],
[Fab_Code] = [Fab_Code],
[Size Ratio] = [Size Ratio],
[Colorway1] = case when (Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#] ,[Seq#],[Style#],[Fab Desc],[Colorway] 
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Colorway] end,
[Color1] = case when (Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#],[Style#],[Fab Desc],[Colorway],[Color]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Color] end,
[Cut Qty] = [Cut Qty],
[Fab Cons.] = [Fab Cons.],
[Fab Desc1] = case when (Row_number() over (partition by [Line#],[Request#],[Cutting Date],[SP#],[Seq#],[Style#],[Fab Desc],[Colorway],[Color],[Fab Desc]
	order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Fab Desc] end,
[Remark] = [Remark],
[SCI Delivery]=[SCI Delivery],
[total_qty1] = sum([total_qty])
from #tmpall2");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
group by [Request#],[Cutting Date],[Line#],[SP#],[Seq#],[Style#],[Ref#],[Cut#],[Comb.],[Fab_Code],[Size Ratio],[Colorway],[Color],[Cut Qty],[Fab Cons.],[Fab Desc],[Remark],WS1,WS2,[SCI Delivery],[CutCellID]
order by [Line#],[Request#],[Cutting Date],[SP#],[Comb.],[Cut#],[Seq#]

drop table #tmpall2");
                    sqlCmd.Append(string.Format("{0} ", i));
                }
            }
            #endregion

            #region radioBtnByCutCell
            if (radioBtn_Byonedaydetial.Checked)
            {
                for (int i = 0; i < CutCellcount; i++)
                {
                    sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
', 'U') IS NOT NULL
  DROP TABLE #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
select	
	[Request#] = Cutplan.ID,
	[Fab ETA] = fe.ETA,
	[Line#] = Cutplan_Detail.SewingLineID,
	[SP#] = Cutplan_Detail.OrderID,
	[Seq#] = WorkOrder.Seq1 + '-' + WorkOrder.Seq2,
	[Style#] = o.StyleID,
	[Ref#] = Cutplan_Detail.CutRef,
	[Cut#] = Cutplan_Detail.CutNo,
	[Comb.] = WorkOrder.FabricCombo,
	[Fab_Code] = WorkOrder.FabricCode,
	[Size Ratio] = stuff(sr.SizeCode,1,1,''),
	[Colorway] = stuff(woda.ac,1,1,''),
	[Color] = Cutplan_Detail.ColorID,
	[Cut Qty] = stuff(cq.SizeCode,1,1,''),
	[Fab Cons.] = Cutplan_Detail.Cons,
	[Fab Refno] = FabRefno.Refno,
	[Remark] = Cutplan_Detail.Remark,
	WS1 = WorkOrder.Seq1,
	WS2 = WorkOrder.Seq2,
	[SCI Delivery] = SCI.SciDelivery,
	[CutCellID] = Cutplan.CutCellID,
	Cutplan_Detail.WorkOrderUkey,
	WorkOrder.Ukey,
	WorkOrder.Layer
into #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
from Cutplan 
inner join Cutplan_Detail on Cutplan.ID = Cutplan_Detail.ID
inner join WorkOrder on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey and Cutplan_Detail.ID = WorkOrder.CutplanID
inner join WorkOrder_SizeRatio on Cutplan_Detail.WorkOrderUkey = WorkOrder_SizeRatio.WorkOrderUkey
left join Order_SizeCode on Order_SizeCode.ID = (select DISTINCT POID from Orders where Orders.CuttingSP = WorkOrder_SizeRatio.ID) 
					      and (Order_SizeCode.SizeCode = WorkOrder_SizeRatio.SizeCode)
outer apply(
	select PO_Supp_Detail.ETA
	from PO_Supp_Detail
	where PO_Supp_Detail.ID = Cutplan_Detail.POID 
	and PO_Supp_Detail.Seq1 = WorkOrder.Seq1 
	and PO_Supp_Detail.Seq2 = WorkOrder.Seq2 
) as fe
outer apply(
	select Orders.StyleID 
	from Orders 
	where Orders.ID = Cutplan_Detail.OrderID
) as o
outer apply(
	select SizeCode= (
		Select concat(',',(SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrder_SizeRatio
		where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		for xml path('')
	 )
) as sr
outer apply(
	 select AC= (
		 Select distinct concat('/', wod.Article)
		 from WorkOrder_Distribute wod
		 where WorkOrderUKey = Cutplan_Detail.WorkOrderUKey and Article != ''
		 for xml path('')
	 )
) as woda
outer apply(
	select SizeCode= (
		Select concat(',',SizeCode+'/'+Convert(varchar,Qty*(select Layer from WorkOrder where UKey = Cutplan_Detail.WorkOrderUKey))) 
		from WorkOrder_SizeRatio where WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		for xml path('')
	 )
) as cq
outer apply(
	select PO_Supp_Detail.RefNo
	from PO_Supp_Detail
	where PO_Supp_Detail.ID = Cutplan_Detail.POID
	and PO_Supp_Detail.Seq1 = WorkOrder.Seq1 
	and PO_Supp_Detail.Seq2 = WorkOrder.Seq2
) as FabRefno
outer apply(
	select Orders.SCIDelivery
	from Orders
	where Orders.ID = Cutplan_Detail.OrderID
) as SCI

where 1 = 1
");
                    if (!MyUtility.Check.Empty(dateR_CuttingDate1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.EstCutdate = '{0}' ", Convert.ToDateTime(dateR_CuttingDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(MD))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", MD));
                    }
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = '{0}' ", Cutcelltb.Rows[i][0].ToString()));
                    }

                    sqlCmd.Append(@"
order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#]

select [Request#],[Fab ETA],[Line#],[SP#],[Seq#],[Style#],[Ref#],[Cut#],[Comb.],[Fab_Code],[Size Ratio],[Colorway],[Color],[Cut Qty],[Fab Cons.],[Fab Refno],[Remark],WS1,WS2,[SCI Delivery],[CutCellID]
,[total_qty] = sum(x.total_qty)
into #tmpall2");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
from #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"  a
outer apply(
	Select [total_qty] = c.qty * a.layer From WorkOrder_SizeRatio c Where  c.WorkOrderUkey = a.WorkOrderUkey and c.WorkOrderUkey = a.Ukey
) x 
group by [Request#],[Fab ETA],[Line#],[SP#],[Seq#],[Style#],[Ref#],[Cut#],[Comb.],[Fab_Code],[Size Ratio],[Colorway],[Color],[Cut Qty],[Fab Cons.],[Fab Refno],[Remark],WS1,WS2,[SCI Delivery],[CutCellID]
drop table #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"

select 
[Request#1]= case when (Row_number() over (partition by [Line#],[Request#]
	order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Request#] end,
[Fab ETA1] = case when (Row_number() over (partition by [Line#],[Request#],[Fab ETA]
	order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1  then '' else Convert(varchar,[Fab ETA]) end,
[Line#1] = case when ((Row_number() over (partition by [Line#]
	order by [Line#] ,[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1) then '' else [Line#] end,
[SP#1] = case when  (Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#] 
	order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [SP#] end ,
[Seq#1] = case when (Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#] 
	order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Seq#] end,
[Style#1] = case when (Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#],[Style#] 
	order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Style#] end,
[Ref#] = [Ref#],
[Cut#] = [Cut#],
[Comb.] = [Comb.],
[Fab_Code] = [Fab_Code],
[Size Ratio] = [Size Ratio],
[Colorway1] = case when (Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#] ,[Seq#],[Style#],[Colorway] 
	order by [Line#],[Fab ETA],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Colorway] end,
[Color1] = case when (Row_number() over (partition by [Line#],[Request#],[Fab ETA],[SP#],[Seq#],[Style#],[Colorway],[Color]
	order by [Line#],[Fab ETA],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#])) >1 then '' else [Color] end,
[Cut Qty] = [Cut Qty],
[Fab Cons.] = [Fab Cons.],
[Fab Refno] = [Fab Refno],
[Remark] = [Remark],
[SCI Delivery]=[SCI Delivery],
[total_qty1] = sum([total_qty])
	 from #tmpall2");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
group by [Request#],[Fab ETA],[Line#],[SP#],[Seq#],[Style#],[Ref#],[Cut#],[Comb.],[Fab_Code],[Size Ratio],[Colorway],[Color],[Cut Qty],[Fab Cons.],[Fab Refno],[Remark],WS1,WS2,[SCI Delivery],[CutCellID]
order by [Line#],[Request#],[Fab ETA],[SP#],[Comb.],[Cut#],[Seq#]

drop table #tmpall2");
                    sqlCmd.Append(string.Format("{0} ", i));
                }
            }
            #endregion

            #region radiobtn By Detail3
            if (radiobtn_BySummary.Checked)
            {
                for (int i = 0; i < CutCellcount; i++)
                {
                    sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
', 'U') IS NOT NULL
  DROP TABLE #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
select
	[Request#] = Cutplan.ID,
	[Line#] = Cutplan_Detail.SewingLineID,
	[SP#] = Cutplan_Detail.OrderID,
	[Seq#] = WorkOrder.Seq1 + '-' + WorkOrder.Seq2,
	[Style#] = Orders.StyleID,
	[FabRef#] = PO_Supp_Detail.RefNo,
	[Color] = Cutplan_Detail.ColorID,
	[Comb.] = WorkOrder.FabricCombo,
	[Fab_Code] = WorkOrder.FabricCode,
	[Total Fab Cons] =sum(Cutplan_Detail.Cons)
into #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
from Cutplan
inner join Cutplan_Detail on Cutplan.ID = Cutplan_Detail.ID and SewingLineID = Cutplan_Detail.SewingLineID 
inner join WorkOrder on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey 
	and WorkOrder.CutplanID = Cutplan_Detail.ID
	and Cutplan_Detail.ID = WorkOrder.CutplanID
inner join Orders on Orders.ID = Cutplan_Detail.OrderID
inner join PO_Supp_Detail on PO_Supp_Detail.ID = Cutplan_Detail.POID 
	and PO_Supp_Detail.Seq1 = WorkOrder.Seq1 
	and PO_Supp_Detail.Seq2 = WorkOrder.Seq2

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
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = '{0}' ", Cutcelltb.Rows[i][0].ToString()));
                    }

                    sqlCmd.Append(@"
Group by Cutplan.ID, Cutplan_Detail.SewingLineID, Cutplan_Detail.OrderID, WorkOrder.Seq1 + '-' + WorkOrder.Seq2,Orders.StyleID,PO_Supp_Detail.RefNo,Cutplan_Detail.ColorID,WorkOrder.FabricCombo,WorkOrder.FabricCode
order by [Request#],[Line#],[SP#],[Seq#]

select
	[Request#] =G.Request#,
	[Line#] =G.Line#,
	[SP#] =G.SP#,
	[Seq#] =Convert(varchar, G.Seq#),
	[Style#] =G.Style#,
	[FabRef#] =G.FabRef#,
	[Fab Desc] =FabDesc.FabDesc,
	[Color] = G.Color,
	[Comb.] =G.[Comb.],
	[Fab_Code] =G.Fab_Code,
	[Cut#] = stuff(cutno.cutno,1,1,''),
	[Size Ratio] =stuff(sr.SizeCode,1,1,''),
	[Cut Qty] = stuff(cq.SizeCode,1,1,''),
	[Colorway] = stuff(woda.ac,1,1,''),
	[Total Fab Cons] =G.[Total Fab Cons],
	[Remark] = stuff(Remark.Remark,1,1,'')
from #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
as G
outer apply(
	 select FabDesc= (
		 Select [Production].dbo.getMtlDesc(Cutplan_Detail.POID, WorkOrder.Seq1, WorkOrder.Seq2,2,0)
		 from Cutplan_Detail
		 inner join WorkOrder on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey and Cutplan_Detail.ID = WorkOrder.CutplanID
		 where Cutplan_Detail.ID = Request#
		 and SewingLineID = Line#
		 and Cutplan_Detail.OrderID = SP#
		 and  (select Seq1+'-'+Seq2 
			   from WorkOrder 
			   where UKey = Cutplan_Detail.WorkOrderUKey and CutplanID = Cutplan_Detail.ID) = Seq#
		 for xml path('')
	 )
) as FabDesc
outer apply(
	 select cutno= (
		 Select distinct concat('/', Cutplan_Detail.CutNo)
		 from Cutplan_Detail
		 where Cutplan_Detail.ID = Request#
		 and SewingLineID = Line#
		 and OrderID = SP#
		 and  (select Seq1+'-'+Seq2 
			   from WorkOrder 
			   where UKey = Cutplan_Detail.WorkOrderUKey and CutplanID = Cutplan_Detail.ID) = Seq#
		 for xml path('')
	 )
) as cutno
outer apply(
	select SizeCode= (
		Select concat(',',(SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrder_SizeRatio
		inner join Cutplan_Detail on WorkOrder_SizeRatio.WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		 where Cutplan_Detail.ID = Request#
		 and SewingLineID = Line#
		 and OrderID = SP#
		 and  (select Seq1+'-'+Seq2 
			   from WorkOrder 
			   where UKey = Cutplan_Detail.WorkOrderUKey and CutplanID = Cutplan_Detail.ID) = Seq#		
		for xml path('')
	 )
) as sr
outer apply(
	select SizeCode= (
		Select concat(',',SizeCode+'/'+Convert(varchar,Qty*
						(select Layer from WorkOrder where UKey = Cutplan_Detail.WorkOrderUKey))) 
		from WorkOrder_SizeRatio
		inner join Cutplan_Detail on WorkOrder_SizeRatio.WorkOrderUkey = Cutplan_Detail.WorkOrderUKey
		 where Cutplan_Detail.ID = Request#
		 and SewingLineID = Line#
		 and OrderID = SP#
		 and  (select Seq1+'-'+Seq2 
			   from WorkOrder 
			   where UKey = Cutplan_Detail.WorkOrderUKey and CutplanID = Cutplan_Detail.ID) = Seq#
		for xml path('')
	 )
) as cq
outer apply(
	 select AC= (
		 Select distinct concat('/', WorkOrder_Distribute.Article)
		 from WorkOrder_Distribute
		 inner join Cutplan_Detail on workOrder_Distribute.WorkOrderUKey = Cutplan_Detail.WorkOrderUKey
		 where Article != ''
		 and Cutplan_Detail.ID = Request#
		 and SewingLineID = Line#
		 and Cutplan_Detail.OrderID = SP#
		 and  (select Seq1+'-'+Seq2 
			   from WorkOrder 
			   where UKey = Cutplan_Detail.WorkOrderUKey and CutplanID = Cutplan_Detail.ID) = Seq#	
		 for xml path('')
	 )
) as woda
outer apply(
	 select Remark= (
		 Select distinct concat(' ', Cutplan_Detail.Remark)
		 from Cutplan_Detail
		 where Cutplan_Detail.ID = Request#
		 and SewingLineID = Line#
		 and OrderID = SP#
		 and  (select Seq1+'-'+Seq2 
			   from WorkOrder 
			   where UKey = Cutplan_Detail.WorkOrderUKey and CutplanID = Cutplan_Detail.ID) = Seq#
		 for xml path('')
	 )
) as Remark

order by [Request#],[Line#],[SP#],[Seq#]

drop table #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                }
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            #region Bydetail OR Byonedaydetial OR Byonedaydetial依狀況插入列
            if (radiobtn_Bydetail.Checked || radioBtn_Byonedaydetial.Checked)
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
                }
            }
            #endregion
            
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            createfolder();
            SetCount(printData[0].Rows.Count);
            if (!boolsend)
            {
                tmpFile = Path.Combine(Sci.Env.Cfg.ReportTempDir, Guid.NewGuid() + ".xlsx");//設定存檔路徑字串
                boolshowexcel = false;
            }

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
            if (radiobtn_Bydetail.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
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
                    MyUtility.Excel.CopyToXls(printData[i], tmpFile, "Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: boolshowexcel, showSaveMsg: false);//將datatable copy to excel

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
                    if (objSheets != null) Marshal.FinalReleaseComObject(objSheets); //釋放sheet                    
                }
                if (!boolsend)
                {
                    objApp.Visible = true;
                }
                if (objApp != null) Marshal.FinalReleaseComObject(objApp); //釋放objApp
            }
            #endregion

            #region radioBtn_Byonedaydetial
            if (radioBtn_Byonedaydetial.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
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
                    MyUtility.Excel.CopyToXls(printData[i], tmpFile, "Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: boolshowexcel, showSaveMsg: false);//將datatable copy to excel

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
                            objSheets.get_Range("L" + (6 + j), "O" + (6 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[6 + j, 14] = "Total Cons.";
                        }
                    }
                    objSheets.Columns["R"].Clear();
                    objSheets.Name = "Cell" + (Cutcelltb.Rows[i][0].ToString());//工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d") + "~" + Convert.ToDateTime(dateR_CuttingDate2).ToString("d"); //查詢日期
                    objSheets.Cells[3, 6] = (Cutcelltb.Rows[i][0].ToString());//cutcellID
                    objSheets.Cells[3, 9] = MD;
                    if (objSheets != null) Marshal.FinalReleaseComObject(objSheets); //釋放sheet                     
                }
                if (!boolsend)
                {
                    objApp.Visible = true;
                }
                if (objApp != null) Marshal.FinalReleaseComObject(objApp); //釋放objApp
            }
            #endregion

            #region radioBtn_BySUMMY
            if (radiobtn_BySummary.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
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
                    MyUtility.Excel.CopyToXls(printData[i], tmpFile, "Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx", headerRow: 5, excelApp: objApp, wSheet: objSheets, showExcel: boolshowexcel, showSaveMsg: false);//將datatable copy to excel

                    objSheets.Name = "Cell" + (Cutcelltb.Rows[i][0].ToString());//工作表名稱
                    objSheets.Cells[3, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d") + "~" + Convert.ToDateTime(dateR_CuttingDate2).ToString("d"); //查詢日期
                    objSheets.Cells[3, 6] = (Cutcelltb.Rows[i][0].ToString());//cutcellID
                    objSheets.Cells[3, 9] = MD;
                    if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet                    
                }
                if (!boolsend)
                {
                    objApp.Visible = true;
                }
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            if (boolsend)
            {
                Send_Mail();
            }

            boolsend = false;
            return true;
        }

        protected void createfolder()
        {
            if (!Directory.Exists(Sci.Env.Cfg.ReportTempDir))
            {
                Directory.CreateDirectory(Sci.Env.Cfg.ReportTempDir);
            }
        }

        private void btn_sendmail_Click(object sender, EventArgs e)
        {
            tmpFile = Path.Combine(Sci.Env.Cfg.ReportTempDir, Guid.NewGuid() + ".xlsx");//設定存檔路徑字串
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
            string mailcmd = "select * from mailto where id = '005'";
            DataTable maildt;
            DBProxy.Current.Select(null, mailcmd, out maildt);
            string ToAddress = MyUtility.Convert.GetString(maildt.Rows[0]["ToAddress"]);
            string CcAddress = MyUtility.Convert.GetString(maildt.Rows[0]["CcAddress"]);
            string Subject = MyUtility.Convert.GetString(maildt.Rows[0]["Subject"]) +"-"+ CuttingDate;

            var email = new MailTo(Sci.Env.User.MailAddress, ToAddress, CcAddress,
                Subject,
                tmpFile,
                "\r\nFilter as below description:\r\nCutting Date: " + CuttingDate + "\r\nCut Cell: " + cutcell + "\r\nM: " + MD, false, true);
            email.ShowDialog(this);
        }        
    }
}
