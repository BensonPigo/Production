using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Cutting
{
    public partial class R09 : Sci.Win.Tems.PrintForm
    {
        private string strM;
        private string strFty;
        private DateTime? dateEstCut1;
        private DateTime? dateEstCut2;
        private string strSpreadingNo1;
        private string strSpreadingNo2;
        private string strCutCell1;
        private string strCutCell2;
        private string strCuttingSPNo;


        private DataTable printData;

        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            strM = this.txtMdivision.Text;
            strFty = this.txtfactory.Text;
            dateEstCut1 = this.dateEstCutDate.Value1;
            dateEstCut2 = this.dateEstCutDate.Value2;
            strSpreadingNo1 = this.txtSpreadingNo1.Text;
            strSpreadingNo2 = this.txtSpreadingNo2.Text;
            strCutCell1 = this.txtCutCell1.Text;
            strCutCell2 = this.txtCutCell2.Text;
            strCuttingSPNo = this.txtCuttingSP.Text;


            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            StringBuilder strSqlCmd = new StringBuilder();
            strSqlCmd.Append($@"
select [Factory] = wo_Before.FactoryID
,wo_Before.WorkOrderUkey
,[CutCell] = wo_Before.CutCellid
,[SpreadingNo] = wo_Before.SpreadingNoID
,[CuttingPlanID] = wo_Before.CutplanID
,[SP] = wo_Before.ID
,[SubSP] = wo_Before.OrderID
,[Style] = o.StyleID
,[Size] = SizeCode.SizeCode
,[OrderQty] = OrderQty_Before.qty
,[CutRefer#] = wo_Before.CutRef
,[RefNo_Desc] = wo_Before.SCIRefno
,[FabricType] = f.WeaveTypeID
,[FabricDesc] = f.Description
,[Combination] = wo_Before.FabricCombo
,[MarkerLength_Before] = wo_Before.MarkerLength
,[MarkerLength_After] = MarkerLength_After.strLength
,[Layer_Before] = ISNULL(wo_Before.Layer,0)
,[Layer_After] = ISNULL( Layer_After.strLayer,0)
,[Ratio_Before] = ISNULL( Ratio_Before.qty,0)
,[Ratio_After] = isnull( Ratio_After.strQty,0)
,[Cons_Before] = cast(round(ISNULL(sc.Cons,0),2) as float)
,[ExcessQty_Before] = isnull( Excess_Before.qty,0)
,[ExcessQty_After] = isnull( Excess_After.strQty,0)
,[Roll] = iif(isnull(n.NoofRoll,0)<1,1,n.NoofRoll)
,[NoOfWindow] = CONVERT(float,round((wo_Before.Cons/ wo_Before.Layer),2))
,[Perimeter_Before] = cast(iif(wo_before.ActCuttingPerimeter not like '%yd%','0',ROUND(dbo.GetActualPerimeterYd(wo_before.ActCuttingPerimeter),2)) as float)
,[Perimeter_After] = Perimeter_After.strPerimeter
,[CuttingSpeed_Before] = ISNULL(  ActSpeed_Before.ActualSpeed,0)
,[CuttingSpeed_After] =ISNULL( ActSpeed_After.strActualSpeed,0)
,[SpreadingTime_Before] = isnull(cast(round(dbo.GetSpreadingTime(f.WeaveTypeID,wo_before.Refno,iif(isnull(n.NoofRoll,0)<1,1,n.NoofRoll),sl.Layer,sc.Cons,1)/60,2)as float),0)
,[CuttingTime_Before] = ROUND(cast( ISNULL( dbo.GetCuttingTime( ROUND(dbo.GetActualPerimeterYd(iif(wo_before.ActCuttingPerimeter not like '%yd%','0',wo_before.ActCuttingPerimeter)),4),wo_Before.CutCellid,sl.Layer,f.WeaveTypeID,sc.Cons),0 )as Float) /60,2)
into #tmp
from WorkOrderRevisedMarkerOriginalData wo_Before
left join orders o on o.ID=wo_Before.ID
left join Fabric f on f.SCIRefno=wo_Before.SCIRefno
left join PO_Supp_Detail po3 on po3.ID=wo_Before.ID and po3.SEQ1=wo_Before.SEQ1 and po3.SEQ2=wo_Before.SEQ2
outer apply(select Layer = sum(wo_Before.Layer)over(partition by wo_Before.CutRef))sl
outer apply(select Cons = sum(wo_Before.Cons)over(partition by wo_Before.CutRef))sc
outer apply(
select strLength = stuff(
(
	select concat(',', MarkerLength)
	from WorkOrder
	where Ukey in (	select Data from dbo.SplitString(wo_Before.WorkOrderUkey,','))
	For XML path('')
	),1,1,'')
)MarkerLength_After
outer apply(
	select sum(Qty) qty from WorkOrder_DistributeRevisedMarkerOriginalData
	where WorkOrderRevisedMarkerOriginalDataUkey = wo_Before.Ukey
	and OrderID !='EXCESS'
)OrderQty_Before
outer apply(
select strLayer = stuff(
(
	select concat(',', Layer)
	from WorkOrder
	where Ukey in (	select Data from dbo.SplitString(wo_Before.WorkOrderUkey,','))
	For XML path('')
	),1,1,'')
)Layer_After
outer apply(
select strCons = stuff(
(
	select concat(',', convert(varchar(100), isnull(sum(Cons)over(partition by cutref),0)))
	from WorkOrder
	where Ukey in (	select Data from dbo.SplitString(wo_Before.WorkOrderUkey,','))
	For XML path('')
	),1,1,'')
)Cons_After
outer apply(
select strPerimeter = stuff(
(
	select concat(',',isnull(convert(varchar(100),iif(ActCuttingPerimeter not like '%yd%','0',ROUND(dbo.GetActualPerimeterYd(ActCuttingPerimeter),2))),0))
	from WorkOrder
	where Ukey in (	select Data from dbo.SplitString(wo_Before.WorkOrderUkey,','))
	For XML path('')
	),1,1,'')
)Perimeter_After
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , SizeCode, '/ ', qty) from(
		select c.SizeCode,sum(qty)qty
		From WorkOrder_DistributeRevisedMarkerOriginalData c
		where WorkOrderRevisedMarkerOriginalDataUkey = wo_Before.Ukey
		and c.OrderID !='EXCESS'
		group by c.SizeCode
		)a
		For XML path('')
	),1,1,'')
) as SizeCode
outer apply(
	select sum(qty) qty 
	from WorkOrder_SizeRatioRevisedMarkerOriginalData ws	
	where ws.WorkOrderRevisedMarkerOriginalDataUkey = wo_Before.Ukey
)Ratio_Before
outer apply(
select strQty = stuff(
	(
		select concat(',',qty) from 
		(
			select isnull( sum(qty),0) qty ,WorkOrderUkey
			from WorkOrder_SizeRatio ws	
			inner join (select data from  dbo.SplitString((select WorkOrderUkey from WorkOrderRevisedMarkerOriginalData where Ukey=wo_Before.Ukey),',') )
			s on ws.WorkOrderUkey=s.Data
			group by WorkOrderUkey	
		) a
		For XML path('')
	),1,1,'')
)Ratio_After
outer apply(
	select Roll = stuff(
	(
		Select concat(', ' , fi.Roll)
		From FtyInventory fi WITH (NOLOCK) 
		Where fi.POID=wo_Before.ID and fi.Seq1=wo_Before.SEQ1 and fi.Seq2=wo_Before.SEQ2
		For XML path('')
	),1,1,'')
)FtyinvRoll
outer apply(
	select Dyelot = stuff(
	(
		Select concat(', ' , fi.Dyelot)
		From FtyInventory fi WITH (NOLOCK) 
		Where fi.POID=wo_Before.ID and fi.Seq1=wo_Before.SEQ1 and fi.Seq2=wo_Before.SEQ2
		For XML path('')
	),1,1,'')
)FtyinvDyelot
outer apply(
	select isnull( ActualSpeed,0)ActualSpeed
	from CuttingMachine_detail cmd
	inner join CutCell cc on cc.CuttingMachineID = cmd.id
	where 1=1
	and cc.id = wo_Before.CutCellid
	and wo_Before.Layer between cmd.LayerLowerBound and cmd.LayerUpperBound
	and cmd.WeaveTypeID = f.WeaveTypeID
)ActSpeed_Before
outer apply(
	select strActualSpeed = stuff(
	(
		select concat(',', convert(varchar(100),isnull(ActualSpeed,0)))
		from CuttingMachine_detail cmd
		inner join CutCell cc on cc.CuttingMachineID = cmd.id
		inner join (
			select * from WorkOrder 
			where ukey in (select data from dbo.SplitString(
			(select WorkOrderUkey from WorkOrderRevisedMarkerOriginalData where convert(varchar(100),ukey) = wo_Before.Ukey)
			,',')) 
		)s on s.CutCellid= cc.ID 
		where 1=1	
		and CAST(s.Layer as int) between CAST( cmd.LayerLowerBound as int) and CAST(cmd.LayerUpperBound as int)
		and cmd.WeaveTypeID = f.WeaveTypeID
		For XML path('')
	),1,1,'')	
)ActSpeed_After
outer apply(
	select isnull(sum(qty),0) qty
	from WorkOrder_DistributeRevisedMarkerOriginalData wod WITH (NOLOCK)
	where CONVERT(varchar(100), wod.WorkOrderRevisedMarkerOriginalDataUkey) = wo_Before.ukey
	and wod.OrderID='EXCESS'
)Excess_Before
outer apply(
select strQty = stuff(
	(
		select concat(',',qty) from 
		(
			select isnull(sum(qty),0) qty ,wod.WorkOrderUkey
			from Workorder_distribute wod WITH (NOLOCK)	
			inner join (select data from  dbo.SplitString((select WorkOrderUkey from WorkOrderRevisedMarkerOriginalData where Ukey=wo_Before.Ukey),',') )
			s on wod.WorkOrderUkey=s.Data
			where wod.OrderID='EXCESS'
			group by WorkOrderUkey	
		) a
		For XML path('')
	),1,1,'')
)Excess_After
outer apply(
	select avgInQty = avg(fi.InQty)
	from PO_Supp_Detail psd with(nolock)
	left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
	where psd.ID = wo_Before.id and psd.SCIRefno = wo_Before.SCIRefno
	and fi.InQty is not null
) as fi
outer apply(select NoofRoll = iif(isnull(fi.avgInQty,0)=0,1,round(sc.Cons/fi.avgInQty,0)))n
where 1=1
");

            #region Filter 條件字串

            if (!MyUtility.Check.Empty(strM))
            {
                strSqlCmd.Append($" and wo_Before.MdivisionID = '{strM}'");
            }

            if (!MyUtility.Check.Empty(strFty))
            {
                strSqlCmd.Append($" and wo_Before.FactoryID = '{strFty}'");
            }

            if (!MyUtility.Check.Empty(dateEstCut1) && !MyUtility.Check.Empty(dateEstCut2))
            {
                strSqlCmd.Append($@" and wo_Before.EstCutDate between '{dateEstCut1}' and '{dateEstCut2}'");
            }

            if (!MyUtility.Check.Empty(strSpreadingNo1))
            {
                strSqlCmd.Append($@" and wo_Before.SpreadingNoID >='{strSpreadingNo1}'");
            }

            if (!MyUtility.Check.Empty(strSpreadingNo2))
            {
                strSqlCmd.Append($@" and wo_Before.SpreadingNoID <='{strSpreadingNo2}'");
            }

            if (!MyUtility.Check.Empty(strCutCell1))
            {
                strSqlCmd.Append($@" and wo_Before.CutCellid >='{strCutCell1}'");
            }

            if (!MyUtility.Check.Empty(strCutCell2))
            {
                strSqlCmd.Append($@" and wo_Before.CutCellid <='{strCutCell2}'");
            }

            if (!MyUtility.Check.Empty(strCuttingSPNo))
            {
                strSqlCmd.Append($@" and wo_Before.id ='{strCuttingSPNo}'");
            }

            strSqlCmd.Append($@"

select [Factory],[CutCell] ,[SpreadingNo] ,[CuttingPlanID] 
,[SP] ,[SubSP],[Style],[Size] 
,[OrderQty] ,[CutRefer#] ,[RefNo_Desc]
,[FabricType] ,[FabricDesc] ,[Combination]
,[MarkerLength_Before],[MarkerLength_After] 
,[Layer_Before],[Layer_After] 
,[Ratio_Before],[Ratio_After] 
,[Cons_Before] ,[Cons_After]  = Cons_After.strCons
,[ExcessQty_Before] ,[ExcessQty_After]
,[Roll] ,[NoOfWindow] 
,[Perimeter_Before] ,[Perimeter_After] 
,[CuttingSpeed_Before],[CuttingSpeed_After] 
,[SpreadingTime_Before],[SpreadingTime_After] = SpreadingTime_After.Time
,[CuttingTime_Before] ,[CuttingTime_After]  = Cutting_After.Time
,[SpeadingTime]  = TotalSpreadingTime.SpreadingTime
,[CuttingTime] = TotalCutting.CuttingTime
,[TotalFabricCons] = TotalCons.Cons
from #tmp t
outer apply(
select Cons= cast(round((sum(cons) - t.Cons_Before),2) as float)  
	from (
		select cons= isnull(round((sum(Cons)over(partition by cutref) ),2) ,0)
		from WorkOrder
		where Ukey in (	select Data from dbo.SplitString(t.WorkOrderUkey,','))
	) a
)TotalCons
outer apply(
select strCons = stuff(
(
	select concat(',', convert(varchar(100), cast(round((sum(Cons)over(partition by cutref)),2)as float)))
	from WorkOrder
	where Ukey in (	select Data from dbo.SplitString(t.WorkOrderUkey,','))
	For XML path('')
	),1,1,'')
)Cons_After
outer apply(

	select SpreadingTime = cast(round((sum(SpreadingTime) /60) - t.SpreadingTime_Before,2)as float)  from 
	(
		select SpreadingTime = cast(round((isnull(dbo.GetSpreadingTime(t.FabricType,Refno,iif(isnull(n.NoofRoll,0)<1,1,n.NoofRoll),sl.Layer,sc.Cons,1),0)),2)  as float)
		from WorkOrder a
		outer apply(select Layer = sum(Layer)over(partition by CutRef))sl
		outer apply(select Cons = sum(Cons)over(partition by CutRef))sc
		outer apply(
			select avgInQty = avg(fi.InQty)
			from PO_Supp_Detail psd with(nolock)
			left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
			where psd.ID = a.id and psd.SCIRefno = a.SCIRefno
			and fi.InQty is not null
		) as fi
		outer apply(select NoofRoll = iif(isnull(fi.avgInQty,0)=0,1,round(sc.Cons/fi.avgInQty,0)))n
		where Ukey in (	select Data from dbo.SplitString(t.WorkOrderUkey,','))
	) a
)TotalSpreadingTime
outer apply(
select Time = stuff(
(
	select concat(',',SpreadingTime ) from 
	(
		select SpreadingTime = cast(round( (isnull(dbo.GetSpreadingTime(t.FabricType,Refno,iif(isnull(n.NoofRoll,0)<1,1,n.NoofRoll),sl.Layer,sc.Cons,1),0))/60,2) as float)
		from WorkOrder a
		outer apply(select Layer = sum(Layer)over(partition by CutRef))sl
		outer apply(select Cons = sum(Cons)over(partition by CutRef))sc
		outer apply(
			select avgInQty = avg(fi.InQty)
			from PO_Supp_Detail psd with(nolock)
			left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
			where psd.ID = a.id and psd.SCIRefno = a.SCIRefno
			and fi.InQty is not null
		) as fi
		outer apply(select NoofRoll = iif(isnull(fi.avgInQty,0)=0,1,round(sc.Cons/fi.avgInQty,0)))n
		where Ukey in (	select Data from dbo.SplitString(t.WorkOrderUkey,','))
		) a
	For XML path('')
	),1,1,'')
)SpreadingTime_After
outer apply(

	select  CuttingTime = cast(round((sum(CuttingTime) /60) ,2)- t.CuttingTime_Before as float)  from 
	(
		select CuttingTime = 
 ROUND(cast( ISNULL( dbo.GetCuttingTime( ROUND(dbo.GetActualPerimeterYd(iif(ActCuttingPerimeter not like '%yd%','0',ActCuttingPerimeter)),2)
 ,CutCellid,sl.Layer,t.FabricType,sc.Cons),0 )as Float) ,2)
		from WorkOrder
		outer apply(select Layer = sum(Layer)over(partition by CutRef))sl
		outer apply(select Cons = sum(Cons)over(partition by CutRef))sc
		where Ukey in (	select Data from dbo.SplitString(t.WorkOrderUkey,','))
		) a
)TotalCutting
outer apply(
select Time = stuff(
(
	select concat(',',CuttingTime ) from 
	(
select CuttingTime = 
 ROUND(cast( ISNULL( dbo.GetCuttingTime( ROUND(dbo.GetActualPerimeterYd(iif(ActCuttingPerimeter not like '%yd%','0',ActCuttingPerimeter)),2)
 ,CutCellid,sl.Layer,t.FabricType,sc.Cons),0 )as Float) /60,2)
		from WorkOrder
		outer apply(select Layer = sum(Layer)over(partition by CutRef))sl
		outer apply(select Cons = sum(Cons)over(partition by CutRef))sc
		where Ukey in (	select Data from dbo.SplitString(t.WorkOrderUkey,','))
		) a
	For XML path('')
	),1,1,'')
)Cutting_After


drop table #tmp");
            #endregion

            DualResult result = DBProxy.Current.Select(null, strSqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Cutting_R09.xltx", objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            com.WriteTable(this.printData, 3);
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            objApp.Columns.AutoFit();
            // 調整欄寬
            worksheet.Columns["O:O"].ColumnWidth = 14;
            worksheet.Columns["P:P"].ColumnWidth = 25;
            worksheet.Columns["Q:T"].ColumnWidth = 10;
            worksheet.Columns["U:V"].ColumnWidth = 12;
            worksheet.Columns["W:X"].ColumnWidth = 10;
            worksheet.Columns["AA:AH"].ColumnWidth = 12;
            objApp.Rows.AutoFit();
            string Excelfile = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R09");
            objApp.ActiveWorkbook.SaveAs(Excelfile);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            return true;
        }
    }
}
