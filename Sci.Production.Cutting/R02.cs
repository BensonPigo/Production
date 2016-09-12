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
        DataSet ds_printData = new DataSet();
        DataTable[] printData;
        string WorkOrder, CutCell1, CutCell2;
        DateTime? dateR_CuttingDate1, dateR_CuttingDate2;
        StringBuilder condition_CuttingDate = new StringBuilder();
        string tmpFile;
        bool boolshowexcel = false;
        bool boolsend = false;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable WorkOrder;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct MDivisionID from WorkOrder", out WorkOrder);
            MyUtility.Tool.SetupCombox(cmb_MDivisionID, 1, WorkOrder);
            cmb_MDivisionID.Text = Sci.Env.User.Keyword;
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

            if (MyUtility.Check.Empty(txt_CutCell1.Text) && MyUtility.Check.Empty(txt_CutCell2.Text))
            {
                MyUtility.Msg.WarningBox("CutCell can't empty!!");
                return false;
            }

            WorkOrder = cmb_MDivisionID.Text;
            dateR_CuttingDate1 = dateR_CuttingDate.Value1;
            dateR_CuttingDate2 = dateR_CuttingDate.Value2;
            CutCell1 = txt_CutCell1.Text;
            CutCell2 = txt_CutCell2.Text;

            return base.ValidateInput();
        }

        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            int cutcellint1 = -1, cutcellint2 = -1;
            int.TryParse(CutCell1, out  cutcellint1);
            int.TryParse(CutCell2, out  cutcellint2);
            if (cutcellint2.Empty())
            {
                cutcellint2 = cutcellint1;
            }
            //做cutcellID1~CutCell2

            #region radiobtnByM
            if (radiobtn_Bydetail.Checked)
            {
                for (int i = cutcellint1; i < cutcellint2 + 1; i++)
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
    [wosrQ]=WorkOrder_SizeRatio.Qty,
	[woL]=WorkOrder.Layer
into #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
from Cutplan 
inner join Cutplan_Detail on Cutplan.ID = Cutplan_Detail.ID
inner join WorkOrder on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey and Cutplan_Detail.ID = WorkOrder.CutplanID
inner join WorkOrder_SizeRatio on Cutplan_Detail.WorkOrderUkey = WorkOrder_SizeRatio.WorkOrderUkey
left join Order_SizeCode on Order_SizeCode.ID = (select POID from Orders where Orders.CuttingSP = WorkOrder_SizeRatio.ID) 
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
                    if (!MyUtility.Check.Empty(WorkOrder))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", WorkOrder));
                    }
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = {0} ", i));//CutCellID1 ~ CutCellID2
                    }
                    //if (!MyUtility.Check.Empty(CutCell1))
                    //{
                    //    sqlCmd.Append(string.Format(" and Cutplan.CutCellID >= {0} ", CutCell1));
                    //}
                    //if (!MyUtility.Check.Empty(CutCell2))
                    //{
                    //    sqlCmd.Append(string.Format(" and Cutplan.CutCellID <= {0} ", CutCell2));
                    //}

                    sqlCmd.Append(@"
order by [Request#],[Line#], [Cutting Date], [SP#], [Fab_Code], WS1, WS2

select 
	[Request#]= case when a.rnR >1 then '' else a.[Request#] end,
	[Cutting Date] = case when a.rnCD >1  then '' else Convert(varchar,a.[Cutting Date]) end,
	[Line#] = case when (a.rnL >1) then '' else a.[Line#] end,
	[SP#] = case when a.rnSP >1 and a.rnSeq >1 then '' else a.[SP#] end ,
	[Seq#] = case when a.rnSeq >1 then '' else a.[Seq#] end,
	[Style#] = case when a.rnSt >1 then '' else a.[Style#] end,
	[Ref#] = a.[Ref#],
	[Cut#] = a.[Cut#],
	[Comb.] = a.[Comb.],
	[Fab_Code] = a.[Fab_Code],
	[Size Ratio] = a.[Size Ratio],
	[Colorway] = case when a.rnClrw >1   then '' else a.[Colorway] end,
	[Color] = case when a.rnClr >1  then '' else a.[Colorway] end,
	[Cut Qty] = a.[Cut Qty],
	[Fab Cons.] = a.[Fab Cons.],
	[Fab Desc] = case when a.rnFB >1and a.rnR >1  then '' else a.[Fab Desc] end,
	[Remark] = a.[Remark],
    [SCI Delivery]=a.[SCI Delivery],
	[wosrQ]=a.wosrQ,
	[woL]=a.woL

from 
	(select 	
		rnR = Row_number() over (partition by [Request#] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnCD = Row_number() over (partition by [Request#],[Cutting Date] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnL = Row_number() over (partition by [Request#],[Cutting Date],[Line#] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnSP = Row_number() over (partition by [Request#],[Cutting Date],[Line#],[SP#] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnSeq = Row_number() over (partition by [Request#],[Cutting Date],[Line#],[SP#],[Seq#] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),		
		rnSt = Row_number() over (partition by [Request#],[Cutting Date],[Line#],[Seq#],[SP#],[Style#] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnFB = Row_number() over (partition by [Request#],[Cutting Date],[Line#],[Seq#],[SP#],[Style#] ,[Fab Desc] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnClrw = Row_number() over (partition by [Request#],[Cutting Date],[Line#],[Seq#],[SP#],[Style#] ,[Fab Desc] ,[Colorway] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnClr = Row_number() over (partition by [Request#],[Cutting Date],[Line#],[Seq#],[SP#],[Style#] ,[Fab Desc] ,[Colorway] ,[Color] 
				order by [CutCellID],[Request#],[Cutting Date],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		* 
	 from #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
	) as a

drop table #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                }
            }
            #endregion

            #region radioBtnByCutCell
            if (radioBtn_Byonedaydetial.Checked)
            {
                for (int i = cutcellint1; i < cutcellint2 + 1; i++)
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
	[wosrQ]=WorkOrder_SizeRatio.Qty,
	[woL]=WorkOrder.Layer
into #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
from Cutplan 
inner join Cutplan_Detail on Cutplan.ID = Cutplan_Detail.ID
inner join WorkOrder on Cutplan_Detail.WorkOrderUkey = WorkOrder.Ukey and Cutplan_Detail.ID = WorkOrder.CutplanID
inner join WorkOrder_SizeRatio on Cutplan_Detail.WorkOrderUkey = WorkOrder_SizeRatio.WorkOrderUkey
left join Order_SizeCode on Order_SizeCode.ID = (select POID from Orders where Orders.CuttingSP = WorkOrder_SizeRatio.ID) 
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
                    if (!MyUtility.Check.Empty(WorkOrder))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", WorkOrder));
                    }
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = {0} ", i));//CutCellID1 ~ CutCellID2
                    }

                    sqlCmd.Append(@"
order by [CutCellID],[Request#],[Line#], [Fab ETA], [SP#], [Fab_Code], WS1, WS2

select 
	[Request#]= case when a.rnR >1 then '' else a.[Request#] end,
	[Fab ETA] = case when a.rnFE >1  then '' else Convert(varchar,a.[Fab ETA]) end,
	[Line#] = case when (a.rnL >1) then '' else a.[Line#] end,
	[SP#] = case when a.rnSP >1 and a.rnSeq >1 then '' else a.[SP#] end ,
	[Seq#] = case when a.rnSeq >1 then '' else a.[Seq#] end,
	[Style#] = case when a.rnSt >1 then '' else a.[Style#] end,
	[Ref#] = a.[Ref#],
	[Cut#] = a.[Cut#],
	[Comb.] = a.[Comb.],
	[Fab_Code] = a.[Fab_Code],
	[Size Ratio] = a.[Size Ratio],
	[Colorway] = case when a.rnClrw >1   then '' else a.[Colorway] end,
	[Color] = case when a.rnClr >1  then '' else a.[Colorway] end,
	[Cut Qty] = a.[Cut Qty],
	[Fab Cons.] = a.[Fab Cons.],
	[Fab Refno] = a.[Fab Refno],
	[Remark] = a.[Remark],
	[SCI Delivery]=a.[SCI Delivery],
	[wosrQ]=a.wosrQ,
	[woL]=a.woL
	
from 
	(select 	
		rnR = Row_number() over (partition by [Request#] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnFE = Row_number() over (partition by [Request#],[Fab ETA] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnL = Row_number() over (partition by [Request#],[Fab ETA],[Line#] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnSP = Row_number() over (partition by [Request#],[Fab ETA],[Line#],[SP#] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnSeq = Row_number() over (partition by [Request#],[Fab ETA],[Line#],[SP#],[Seq#] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),		
		rnSt = Row_number() over (partition by [Request#],[Fab ETA],[Line#],[Seq#],[SP#],[Style#] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnFB = Row_number() over (partition by [Request#],[Fab ETA],[Line#],[Seq#],[SP#],[Style#] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnClrw = Row_number() over (partition by [Request#],[Fab ETA],[Line#],[Seq#],[SP#],[Style#],[Colorway] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		rnClr = Row_number() over (partition by [Request#],[Fab ETA],[Line#],[Seq#],[SP#],[Style#],[Colorway] ,[Color] 
				order by [CutCellID],[Request#],[Fab ETA],[Line#],[SP#],[Fab_Code],[Cut#],WS1,WS2),
		* 
	 from #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                    sqlCmd.Append(@"
	) as a

drop table #tmpall");
                    sqlCmd.Append(string.Format("{0} ", i));
                }
            }
            #endregion

            #region radiobtn By Detail3
            if (radiobtn_BySummary.Checked)
            {
                for (int i = cutcellint1; i < cutcellint2 + 1; i++)
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
                    if (!MyUtility.Check.Empty(WorkOrder))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.MDivisionID ='{0}' ", WorkOrder));
                    }
                    if (!MyUtility.Check.Empty(CutCell1))
                    {
                        sqlCmd.Append(string.Format(" and Cutplan.CutCellID = {0} ", i));//CutCellID1 ~ CutCellID2
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

            #region Bydetail OR Byonedaydetial 依狀況插入列
            if (radiobtn_Bydetail.Checked || radioBtn_Byonedaydetial.Checked)
            {
                for (int i = 0; i < printData.Count(); i++)
                {
                    int m = 0, n = 0, l = 0;
                    decimal dm = 0, dsum = 0;
                    DataTable tmps = new DataTable();
                    tmps = printData[i].Copy();
                    printData[i].Clear();


                    for (int j = 0; j < tmps.Rows.Count; j++)
                    {
                        int.TryParse(tmps.Rows[j]["wosrQ"].ToString(), out m);
                        int.TryParse(tmps.Rows[j]["woL"].ToString(), out n);
                        decimal.TryParse(tmps.Rows[j][14].ToString(), out dm);
                        l += (m * n);
                        dsum += dm;
                        DataRow drr = printData[i].NewRow();
                        drr = tmps.Rows[j];
                        printData[i].ImportRow(drr);

                        //做到倒數第二row
                        if (j < tmps.Rows.Count - 1)
                        {
                            //若下個SP#有值則塞row
                            if (!tmps.Rows[j + 1]["SP#"].Empty())
                            {
                                DataRow tabrow = printData[i].NewRow();//12
                                tabrow[11] = "Total Cut Qty";
                                tabrow[12] = l;
                                //tabrow[13] = "Total Cons.";//此欄在Datatable是Decimal無法放入string
                                tabrow[14] = dsum;
                                printData[i].Rows.Add(tabrow);

                                l = 0;
                                m = 0;
                                n = 0;
                                dm = 0;
                                dsum = 0;
                            }
                        }
                        //若到最後一row塞row
                        if (j == tmps.Rows.Count - 1)
                        {
                            DataRow tabrow = printData[i].NewRow();
                            tabrow[11] = "Total Cut Qty";
                            tabrow[12] = l;
                            //tabrow[13] = "Total Cons.";
                            tabrow[14] = dsum;
                            printData[i].Rows.Add(tabrow);
                        }
                    }
                    printData[i].Columns.Remove("wosrQ");
                    printData[i].Columns.Remove("woL");
                }
            }
            #endregion

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            SetCount(printData[0].Rows.Count);
            if (!boolsend)
            {
                tmpFile = Path.Combine(Sci.Env.Cfg.ReportTempDir, Guid.NewGuid() + ".xlsx");//設定存檔路徑字串
                boolshowexcel = true;
            }
            if (printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            #region radiobtn_Bydetail
            if (radiobtn_Bydetail.Checked)
            {
                int cutcellint1 = -1, cutcellint2 = -1;
                int.TryParse(CutCell1, out  cutcellint1);
                int.TryParse(CutCell2, out  cutcellint2);

                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
                for (int i = 0; i < cutcellint2 - cutcellint1 + 1; i++)
                {
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表
                    MyUtility.Excel.CopyToXls(printData[i], tmpFile, "Cutting_R02_CuttingDailyPlanSummaryReportBydetail.xltx", headerRow: 3, excelApp: objApp, wSheet: objSheets, showExcel: boolshowexcel, showSaveMsg: false);//將datatable copy to excel

                    for (int j = 0; j < printData[i].Rows.Count; j++)
                    {
                        if (!printData[i].Rows[j]["Request#"].Empty())
                        {
                            objSheets.get_Range("A" + (5 + j), "B" + (5 + j)).Merge(false);//合併欄位
                            objSheets.get_Range("A" + (5 + j), "A" + (5 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[5 + j, 1] = "SCI Delivery: " + Convert.ToDateTime(printData[i].Rows[j]["SCI Delivery"]).ToString("d");
                        }

                        if (printData[i].Rows[j]["Ref#"].Empty())
                        {
                            objSheets.get_Range("L" + (4 + j), "O" + (4 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[4 + j, 14] = "Total Cons.";
                        }
                    }
                    objSheets.Columns["R"].Clear();
                    objSheets.Name = "Cell" + (i + cutcellint1);//工作表名稱
                    objSheets.Cells[1, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d") + "~" + Convert.ToDateTime(dateR_CuttingDate2).ToString("d"); //查詢日期
                    objSheets.Cells[1, 6] = (i + cutcellint1);//cutcellID
                    objSheets.Cells[1, 9] = WorkOrder;
                    if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet                    
                }

                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radioBtn_Byonedaydetial
            if (radioBtn_Byonedaydetial.Checked)
            {
                int cutcellint1 = -1, cutcellint2 = -1;
                int.TryParse(CutCell1, out  cutcellint1);
                int.TryParse(CutCell2, out  cutcellint2);

                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
                for (int i = 0; i < cutcellint2 - cutcellint1 + 1; i++)
                {
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表
                    MyUtility.Excel.CopyToXls(printData[i], tmpFile, "Cutting_R02_CuttingDailyPlanSummaryReportByonedaydetail.xltx", headerRow: 3, excelApp: objApp, wSheet: objSheets, showExcel: boolshowexcel, showSaveMsg: false);//將datatable copy to excel 

                    for (int j = 0; j < printData[i].Rows.Count; j++)
                    {
                        if (!printData[i].Rows[j]["Request#"].Empty())
                        {
                            objSheets.get_Range("A" + (5 + j), "B" + (5 + j)).Merge(false);//合併欄位
                            objSheets.get_Range("A" + (5 + j), "A" + (5 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[5 + j, 1] = "SCI Delivery: " + Convert.ToDateTime(printData[i].Rows[j]["SCI Delivery"]).ToString("d");
                        }

                        if (printData[i].Rows[j]["Ref#"].Empty())
                        {
                            objSheets.get_Range("L" + (4 + j), "O" + (4 + j)).Font.Bold = true;//指定粗體
                            objSheets.Cells[4 + j, 14] = "Total Cons.";
                        }
                    }
                    objSheets.Columns["R"].Clear();
                    objSheets.Name = "Cell" + (i + cutcellint1);//工作表名稱
                    objSheets.Cells[1, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d") + "~" + Convert.ToDateTime(dateR_CuttingDate2).ToString("d"); //查詢日期
                    objSheets.Cells[1, 6] = (i + cutcellint1);//cutcellID
                    objSheets.Cells[1, 9] = WorkOrder;
                    if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet                    
                }

                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radioBtn_BySUMMY
            if (radiobtn_BySummary.Checked)
            {
                int cutcellint1 = -1, cutcellint2 = -1;
                int.TryParse(CutCell1, out  cutcellint1);
                int.TryParse(CutCell2, out  cutcellint2);

                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx"); //預先開啟excel app
                objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
                for (int i = 0; i < cutcellint2 - cutcellint1 + 1; i++)
                {
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];   // 取得工作表
                    MyUtility.Excel.CopyToXls(printData[i], tmpFile, "Cutting_R02_CuttingDailyPlanSummaryReportBySummary.xltx", headerRow: 3, excelApp: objApp, wSheet: objSheets, showExcel: boolshowexcel, showSaveMsg: false);//將datatable copy to excel 

                    objSheets.Name = "Cell" + (i + cutcellint1);//工作表名稱
                    objSheets.Cells[1, 2] = Convert.ToDateTime(dateR_CuttingDate1).ToString("d") + "~" + Convert.ToDateTime(dateR_CuttingDate2).ToString("d"); //查詢日期
                    objSheets.Cells[1, 6] = (i + cutcellint1);//cutcellID
                    objSheets.Cells[1, 9] = WorkOrder;
                    if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet                    
                }
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            boolsend = false;
            return true;
        }

        private void btn_sendmail_Click(object sender, EventArgs e)
        {
            tmpFile = Path.Combine(Sci.Env.Cfg.ReportTempDir, Guid.NewGuid() + ".xlsx");//設定存檔路徑字串
            boolshowexcel = false;
            boolsend = true;
            this.toexcel.PerformClick();
            Send_Mail();
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
            string ToAddress = "";
            string CcAddress = "";
            string Subject = "";
            string Content = "";
            var email = new MailTo(Sci.Env.User.MailAddress, ToAddress, CcAddress,
                Subject + '-' + CuttingDate,
                tmpFile,
                Content + "\r\nFilter as below description:\r\nCutting Date: " + CuttingDate + "\r\nCut Cell: " + cutcell + "\r\nM: " + WorkOrder, false, true);
            //var email = new MailTo(Sci.Env.User.MailAddress, mailto, Sci.Env.User.MailAddress, subject, null, content.ToString(), true, true);
            //var email = new MailTo("willy.wei@sportscity.com", "willy.wei@sportscity.com", "willy.wei@sportscity.com", subject, null, content.ToString(), false, true);
            email.ShowDialog(this);
        }
    }
}
