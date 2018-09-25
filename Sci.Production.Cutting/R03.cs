using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string WorkOrder, factory, CuttingSP1, CuttingSP2;
        DateTime? Est_CutDate1, Est_CutDate2, EarliestSCIDelivery1, EarliestSCIDelivery2, EarliestSewingInline1, EarliestSewingInline2;
        StringBuilder condition = new StringBuilder();

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable WorkOrder, factory;
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrder WITH (NOLOCK) ", out WorkOrder);
            MyUtility.Tool.SetupCombox(comboM, 1, WorkOrder);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);//要預設空白
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboM.Text = Sci.Env.User.Keyword;
            comboFactory.SelectedIndex = 0;
        }
        
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            
            WorkOrder = comboM.Text;
            factory = comboFactory.Text;
            
            Est_CutDate1 = dateEstCutDate.Value1;
            Est_CutDate2 = dateEstCutDate.Value2;
            CuttingSP1 = txtCuttingSPStart.Text;
            CuttingSP2 = txtCuttingSPEnd.Text;
            EarliestSCIDelivery1 = dateEarliestSCIDelivery.Value1;
            EarliestSCIDelivery2 = dateEarliestSCIDelivery.Value2;
            EarliestSewingInline1 = dateEarliestSewingInline.Value1;
            EarliestSewingInline2 = dateEarliestSewingInline.Value2;
            //不可 Est. Cut Date, Cutting SP#, Earliest SCI Delivery, Earliest Sewing Inline四項全為空值
            if (MyUtility.Check.Empty(Est_CutDate1) && MyUtility.Check.Empty(Est_CutDate2) && MyUtility.Check.Empty(CuttingSP1) && MyUtility.Check.Empty(CuttingSP2) && MyUtility.Check.Empty(EarliestSCIDelivery1) && MyUtility.Check.Empty(EarliestSCIDelivery2) && MyUtility.Check.Empty(EarliestSewingInline1) && MyUtility.Check.Empty(EarliestSewingInline2))
            {
                MyUtility.Msg.WarningBox("Can't all empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        //非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select DISTINCT
	[M] = wo.MDivisionID,
	[Factory] = o.FtyGroup,
	[Est.Cutting Date]= wo.EstCutDate,
	[Act.Cutting Date] = MincDate.MincoDate,
	[Earliest Sewing Inline] = c.SewInLine,
	[Sewing Inline(SP)] = SewInLine.SewInLine,
	[Master SP#] = wo.ID,
	[SP#] = wo.OrderID,
	[Style#] = o.StyleID,
	[Ref#] = wo.CutRef,
	[Cut#] = wo.Cutno,
	[Cut Cell] = wo.CutCellID,
	[Sewing Line] = stuff(SewingLineID.SewingLineID,1,1,''),
	[Sewing Cell] = stuff(SewingCell.SewingCell,1,1,''),
	[Combination] = wo.FabricCombo,
	[Color Way] = stuff(Article.Article,1,1,''),
	[Color]= wo.ColorID, 
	[Layers] = wo.Layer,
    [LackingLayers] = wo.Layer - isnull(acc.AccuCuttingLayer,0),    
	[Qty] = Qty.Qty,
	[Ratio] = stuff(SQty.SQty,1,1,''),
	[Consumption] = wo.Cons,
	[Marker Length] = wo.MarkerLength
from WorkOrder wo WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.CuttingSP = wo.ID
inner join Cutting c WITH (NOLOCK) on c.ID = o.CuttingSP
outer apply(select AccuCuttingLayer = sum(aa.Layer) from cuttingoutput_Detail aa where aa.WorkOrderUkey = wo.Ukey)acc
outer apply(
	select MincoDate=(
		select min(co.cDate)
		from CuttingOutput co WITH (NOLOCK) 
		inner join CuttingOutput_Detail cod WITH (NOLOCK) on co.ID = cod.ID
		where cod.WorkOrderUkey = wo.Ukey
	)
) as MincDate
outer apply(
	select SewInLine=(
		select SewInLine 
		from Orders WITH (NOLOCK) 
		where ID = wo.OrderID
	)
)as SewInLine
outer apply(
	select SewingLineID = (
		select distinct concat('/',SewingLineID)
		from SewingSchedule WITH (NOLOCK) 
		where OrderID = wo.OrderID
		for xml path('')
	)
)as SewingLineID
outer apply(
	select SewingCell = (
		select distinct concat('/',SewingCell)
		from SewingLine,SewingSchedule WITH (NOLOCK) 
		where SewingSchedule.OrderID = wo.OrderID
		and SewingLine.ID = SewingSchedule.SewingLineID 
		and SewingLine.FactoryID = SewingSchedule.FactoryID
		for xml path('')	
	)
)as SewingCell
outer apply(
	select Article = (
		select distinct concat('/',Article)
		from WorkOrder_Distribute WITH (NOLOCK) 
		where WorkOrderUKey = wo.UKey
		and Article != ''
		for xml path('')
	)
)as Article
outer apply(
	select Qty = (
		select sum(Qty)
		from WorkOrder_Distribute WITH (NOLOCK) 
		where WorkOrderUKey = wo.UKey
	)
) as Qty
outer apply(
	select SQty = (
		select distinct concat(',',SizeCode+'/'+Convert(varchar,Qty))
		from WorkOrder_SizeRatio WITH (NOLOCK) 
		where WorkOrderUkey = wo.UKey
		for xml path('')
	)
)as SQty
outer apply(
	select MinSCI = (
		select min(SCIDelivery) 
		from Orders as o WITH (NOLOCK) 
		where o.ID = wo.OrderID
	)
) as MinSci
where 1=1
");
            #region Append畫面上的條件
            if (!MyUtility.Check.Empty(WorkOrder))
            {
                sqlCmd.Append(string.Format(" and wo.MDivisionID = '{0}'", WorkOrder));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
            }

            if (!MyUtility.Check.Empty(Est_CutDate1))
            {
                sqlCmd.Append(string.Format(" and wo.EstCutDate >= '{0}' ",Convert.ToDateTime(Est_CutDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(Est_CutDate2))
            {
                sqlCmd.Append(string.Format(" and wo.EstCutDate <= '{0}' ", Convert.ToDateTime(Est_CutDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(CuttingSP1))
            {
                sqlCmd.Append(string.Format(" and wo.ID >= '{0}'", CuttingSP1));
            }

            if (!MyUtility.Check.Empty(CuttingSP2))
            {
                sqlCmd.Append(string.Format(" and wo.ID <= '{0}'", CuttingSP2));
            }            
            
            if (!MyUtility.Check.Empty(EarliestSCIDelivery1))
            {
                sqlCmd.Append(string.Format(" and MinSci.MinSCI >= '{0}'", Convert.ToDateTime(EarliestSCIDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(EarliestSCIDelivery2))
            {
                sqlCmd.Append(string.Format(" and MinSci.MinSCI <= '{0}' ", Convert.ToDateTime(EarliestSCIDelivery2).ToString("d")));
            }
            
            if (!MyUtility.Check.Empty(EarliestSewingInline1))
            {
                sqlCmd.Append(string.Format(@" and c.SewInLine >= '{0}' ", Convert.ToDateTime(EarliestSewingInline1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(EarliestSewingInline2))
            {
                sqlCmd.Append(string.Format(" and c.SewInLine <= '{0}' ", Convert.ToDateTime(EarliestSewingInline2).ToString("d")));
            }
            #endregion
            sqlCmd.Append(@" order by wo.MDivisionID,o.FtyGroup,wo.EstCutDate,MincDate.MincoDate,c.SewInLine,wo.Cutno");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
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
            
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R03_CuttingScheduleListReport.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Cutting_R03_CuttingScheduleListReport.xltx", 2, false, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.get_Range("P2").ColumnWidth = 15.38;
            objSheets.get_Range("T2").ColumnWidth = 15.38;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R03_CuttingScheduleListReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objSheets);    //釋放sheet
            Marshal.ReleaseComObject(objApp);          //釋放objApp
            Marshal.ReleaseComObject(workbook);
            
            strExcelName.OpenFile();
            #endregion 
            return true;
        }
    }
}
