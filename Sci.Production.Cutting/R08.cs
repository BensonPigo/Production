using Ict;
using Microsoft.Office.Core;
using Sci.Data;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private string Mdivision;
        private string Factory;
        private DateTime? EstCutDate1;
        private DateTime? EstCutDate2;
        private string SpreadingNo1;
        private string SpreadingNo2;
        private string CutCell1;
        private string CutCell2;
        private string CuttingSP;

        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            this.Mdivision = txtMdivision1.Text;
            this.Factory = txtfactory1.Text;
            this.EstCutDate1 = dateEstCutDate.Value1;
            this.EstCutDate2 = dateEstCutDate.Value2;
            this.SpreadingNo1 = txtSpreadingNo1.Text;
            this.SpreadingNo2 = txtSpreadingNo2.Text;
            this.CutCell1 = txtCell1.Text;
            this.CutCell2 = txtCell2.Text;
            this.CuttingSP = txtCuttingSp.Text;
            return base.ValidateInput();
        }
        
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.Mdivision))
            {
                where += $" and w.MDivisionId = '{this.Mdivision}' ";
            }
            if (!MyUtility.Check.Empty(this.Factory))
            {
                where += $" and w.FactoryID = '{this.Factory}' ";
            }
            if (!MyUtility.Check.Empty(this.EstCutDate1))
            {
                where += $" and w.EstCutDate >= '{((DateTime)this.EstCutDate1).ToString("yyyy/MM/dd")}' ";
            }
            if (!MyUtility.Check.Empty(this.EstCutDate2))
            {
                where += $" and w.EstCutDate <=  '{((DateTime)this.EstCutDate2).ToString("yyyy/MM/dd")}' ";
            }
            if (!MyUtility.Check.Empty(this.SpreadingNo1))
            {
                where += $" and w.SpreadingNoID >= '{this.SpreadingNo1}' ";
            }
            if (!MyUtility.Check.Empty(this.SpreadingNo2))
            {
                where += $" and w.SpreadingNoID <= '{this.SpreadingNo2}' ";
            }
            if (!MyUtility.Check.Empty(this.CutCell1))
            {
                where += $" and w.CutCellid >= '{this.CutCell1}' ";
            }
            if (!MyUtility.Check.Empty(this.CutCell2))
            {
                where += $" and w.CutCellid <= '{this.CutCell2}' ";
            }
            if (!MyUtility.Check.Empty(this.CuttingSP))
            {
                where += $" and w.ID = '{this.CuttingSP}' ";
            }

            sqlcmd = $@"
select
	w.FactoryID,
	w.EstCutDate,
	w.CutCellid,
	w.SpreadingNoID,
	w.CutplanID,
	w.CutRef,
	w.ID,
	subSp.SubSP,
	o.StyleID,
	size.Size,
	sumWdQty=sumWdQty.qty,
	f.Description,
	f.MtlTypeID,
	w.FabricCombo,
	MarkerLength=iif(w.Layer=0,0,w.cons/w.Layer),
	PerimeterM=iif(w.ActCuttingPerimeter not like '%yd%',w.ActCuttingPerimeter,dbo.GetActualPerimeter(w.ActCuttingPerimeter)),
	w.Layer,
	SizeCode.SizeCode,
	w.Cons,
	sumWdQtyE=sumWdQtyE.qty,
	NoofRoll.NoofRoll,
	DyeLot=isnull(DyeLot.DyeLot,0),
	NoofWindow=w.Cons/w.Layer/1.4,
	ActSpd.ActualSpeed,
	st.PreparationTime,
	[ChangeoverTime] = iif(fr.isRoll = 0,st.ChangeOverUnRollTime,st.ChangeOverRollTime),
	SpreadingSetupTime=st.SetupTime,
	st.SpreadingTime,
	st.SeparatorTime,
	st.ForwardTime,
	CuttingSetUpTime=ct.SetUpTime,
	ct.WindowTime,
	f.WeaveTypeID,w.Refno,[nNoofRoll]=n.NoofRoll,[slLayer]=sl.Layer,[scCons]=sc.Cons,ct.WindowLength
into #tmp
from WorkOrder w with(nolock)
inner join orders o with(nolock) on o.id = w.ID
left join Fabric f with(nolock) on f.SCIRefno = w.SCIRefno
left join SpreadingTime st with(nolock) on st.WeaveTypeID = f.WeaveTypeID
left join ManufacturingExecution.dbo.RefnoRelaxtime rr WITH (NOLOCK) on rr.Refno = w.Refno
left join ManufacturingExecution.dbo.FabricRelaxation fr WITH (NOLOCK) on rr.FabricRelaxationID = fr.ID
left join CuttingTime ct WITH (NOLOCK) on ct.WeaveTypeID = f.WeaveTypeID
--outer apply(select TotalCuttingPerimeter = dbo.GetActualPerimeter(w.ActCuttingPerimeter))pm
outer apply(
	select SubSP = stuff((
		select distinct concat(',',wd.OrderID)
		from WorkOrder_Distribute wd with(nolock)
		where wd.WorkOrderUkey = w.Ukey
		For XML path('')
	),1,1,'')
)subSp
outer apply(
	select Size = stuff((
		select distinct concat(',',wd.SizeCode)
		from WorkOrder_Distribute wd with(nolock)
		where wd.WorkOrderUkey = w.Ukey
		For XML path('')
	),1,1,'')
)size
outer apply(
	select qty=sum(wd.qty)
	from WorkOrder_Distribute wd with(nolock)
	where wd.WorkOrderUkey = w.Ukey
	and wd.OrderID <> 'EXCESS'	
)sumWdQty
outer apply(
	select qty=sum(wd.qty)
	from WorkOrder_Distribute wd with(nolock)
	where wd.WorkOrderUkey = w.Ukey
	and wd.OrderID = 'EXCESS'	
)sumWdQtyE
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , wd.sizecode, '/ ', wd.qty)
		From WorkOrder_SizeRatio wd WITH (NOLOCK) 
		Where wd.WorkOrderUkey = w.Ukey 
		For XML path('')
	),1,1,'')
)SizeCode
outer apply(
	select NoofRoll = count(1)
	from(
		select distinct cr.Seq1,cr.seq2,cr.Roll,cr.Dyelot
		from CuttingOutputFabricRecord cr WITH (NOLOCK) 
		where cr.CutRef = w.CutRef and cr.MDivisionId = w.MDivisionId
	)disC
)NoofRoll
outer apply(
	select DyeLot = count(1)
	from(
		select distinct cr.Seq1,cr.seq2,cr.Dyelot
		from CuttingOutputFabricRecord cr WITH (NOLOCK) 
		where cr.CutRef = w.CutRef and cr.MDivisionId = w.MDivisionId
	)disC
)DyeLot
outer apply(	
	select  ActualSpeed
	from CuttingMachine_detail cmd WITH (NOLOCK) 
	inner join CutCell cc WITH (NOLOCK) on cc.CuttingMachineID = cmd.id
	where cc.id = w.CutCellid 
	and w.Layer between cmd.LayerLowerBound and cmd.LayerUpperBound
	and cmd.WeaveTypeID = f.WeaveTypeID 
)ActSpd
outer apply(
	select avgInQty = avg(fi.InQty)
	from PO_Supp_Detail psd with(nolock)
	left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
	where psd.ID = w.id and psd.SCIRefno = w.SCIRefno
	and fi.InQty is not null
) as fi
outer apply(select Layer = sum(w.Layer)over(partition by w.CutRef,w.MDivisionId))sl
outer apply(select Cons = sum(w.Cons)over(partition by w.CutRef,w.MDivisionId))sc
outer apply(select NoofRoll = iif(isnull(round(sc.Cons/fi.avgInQty,0),0)=0,1,round(sc.Cons/fi.avgInQty,0)))n

where 1=1
{where}

select FactoryID,EstCutDate,CutCellid,SpreadingNoID,CutplanID,CutRef,ID,SubSP,StyleID,Size,sumWdQty,Description,MtlTypeID,FabricCombo,
	MarkerLength,PerimeterM,Layer,SizeCode,Cons,sumWdQtyE,NoofRoll,DyeLot,NoofWindow,ActualSpeed,
	[PreparationTime_min] = PreparationTime/60.0,
	[ChangeoverTime_min] = ChangeoverTime/60.0,
	[SpreadingSetupTime_min] = SpreadingSetupTime/60.0,
	[MachineSpreadingTime_min]  =SpreadingTime/60.0,
	[Separatortime_min] = SeparatorTime/60.0,
	[ForwardTime_min] = ForwardTime/60.0,
	[CuttingSetupTime_min] = CuttingSetUpTime/60.0,
	MachCuttingTime_min,
	[WindowTime_min] = WindowTime/60.0,
	[TotalSpreadingTime_min] = (isnull(PreparationTime * iif(slLayer=0,0,scCons/slLayer),0) + 
						 isnull(Changeovertime * nNoofRoll,0) +
						 isnull(SpreadingSetupTime,0) +
						 isnull(SpreadingTime * scCons,0) +
						 isnull(SeparatorTime * (DyeLot -1),0) +
						 isnull(ForwardTime,0))
						 /60.0,
	[TotalCuttingTime_min] = (isnull(CuttingSetUpTime,0) + 
					   iif(isnull(ActualSpeed,0)=0,0,isnull(p.PerimeterM_num,0)/ActualSpeed)*60 + 
					   isnull(Windowtime * iif(isnull(slLayer,0)=0,0,scCons/slLayer*0.9144)/WindowLength,0))
					   /60.0
from #tmp
outer apply(select PerimeterM_num=isnumeric(PerimeterM))p
outer apply(select MachCuttingTime_min = iif(isnull(ActualSpeed,0)=0,0,p.PerimeterM_num/ActualSpeed))a

drop table #tmp

";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out printData);
            if (!result)
            {                
                return Result.F(result.ToString());
            }
            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            SetCount(printData.Rows.Count);
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            string excelName = "Cutting_R08";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");

            MyUtility.Excel.CopyToXls(printData, "", $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);// 將datatable copy to excel

            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (excelApp != null) Marshal.FinalReleaseComObject(excelApp);
            #endregion
            
            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
