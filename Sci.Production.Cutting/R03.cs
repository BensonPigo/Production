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
            DBProxy.Current.Select(null, "select '' as ID union all select distinct MDivisionID from WorkOrder", out WorkOrder);
            MyUtility.Tool.SetupCombox(comboBox1, 1, WorkOrder);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox2, 1, factory);
            comboBox1.Text = Sci.Env.User.Keyword;
            comboBox2.SelectedIndex = 0;
        }
        
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(textBox1.Text) && MyUtility.Check.Empty(textBox2.Text) && MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) && MyUtility.Check.Empty(dateRange2.Value1) && MyUtility.Check.Empty(dateRange2.Value2) && MyUtility.Check.Empty(dateRange3.Value1) && MyUtility.Check.Empty(dateRange3.Value2) && MyUtility.Check.Empty(comboBox2.Text))
            {
                MyUtility.Msg.WarningBox("Can't all empty!!");
                return false;
            }
            WorkOrder = comboBox1.Text;
            factory = comboBox2.Text;
            CuttingSP1 = textBox1.Text;
            CuttingSP2 = textBox2.Text;
            Est_CutDate1 = dateRange1.Value1;
            Est_CutDate2 = dateRange1.Value2;
            EarliestSCIDelivery1 = dateRange2.Value1;
            EarliestSCIDelivery2 = dateRange2.Value2;
            EarliestSewingInline1 = dateRange3.Value1;
            EarliestSewingInline2 = dateRange3.Value2;

            return base.ValidateInput();
        }

        //非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select
	M = w.MDivisionID,
	Factory = o.FtyGroup,
	[Est.Cutting Date]= w.EstCutDate,
	[Act.Cutting Date] = MincDate.MincoDate,
	[Earliest Sewing Inline] = c.SewInLine,
	[Sewing Inline(SP)] = o2.SewInLine,
	[Master SP#] = w.ID,
	[SP#] = w.OrderID,
	[Style#] = o.StyleID,
	[Ref#] = w.CutRef,
	[Cut#] = w.Cutno,
	[Cut Cell] = w.CutCellID,
	[Sewing Line] = stuff((select 'Sewing Line' = (
		select distinct concat('/',SewingLineID)
		from SewingSchedule 
		where OrderID = w.OrderID
		for xml path('')
	)),1,1,''),
	[Sewing Cell] = stuff((select 'Sewing Cell' = (
		select distinct concat('/',SewingCell)
		from SewingLine inner join SewingSchedule
		on SewingLine.ID = SewingSchedule.SewingLineID and SewingLine.FactoryID = SewingSchedule.FactoryID
		for xml path('')
	)),1,1,''),
	Combination = w.FabricCombo,
	[Color Way] = stuff((select 'Color Way' = (
		select distinct concat('/',Article)
		from WorkOrder_Distribute 
		where WorkOrderUKey = w.UKey and Article != ''
		for xml path('')
	)),1,1,''),
	Color = w.ColorID, 
	Layers = w.Layer,
	Qty = WODQty.WODSumQty,
	Ratio = stuff((select Ratio = (
		select distinct concat(',',SizeCode+'/'+Convert(varchar,Qty))
		from WorkOrder_SizeRatio 
		where WorkOrderUkey = w.UKey
		for xml path('')
	)),1,1,''),
	Consumption = w.Cons,
	[Marker Length] = w.MarkerLength
from WorkOrder w 
inner join Orders o on w.ID = o.CuttingSP
inner join Orders o2 on o2.ID = w.OrderID
outer apply(
	select min(SCIDelivery) as MinSCI 
	from Orders as o
	where o.ID = w.OrderID
) as MinSci
outer apply(
	select min(co.cDate) as MincoDate
	from CuttingOutput co,CuttingOutput_Detail cd 
	where co.ID = cd.ID and cd.WorkOrderUkey = w.Ukey
) as MincDate
outer apply(
	select sum(Qty) as WODSumQty
	from WorkOrder_Distribute 
	where WorkOrderUKey = w.UKey
) as WODQty
left join Cutting c on o.CuttingSP  = c.ID
where 1=1
");
            #region Append條件字串
            if (!MyUtility.Check.Empty(WorkOrder))
            {
                sqlCmd.Append(string.Format(" and w.MDivisionID = '{0}'", WorkOrder));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
            }

            if (!MyUtility.Check.Empty(CuttingSP1))
            {
                sqlCmd.Append(string.Format(" and w.ID >= '{0}'", CuttingSP1));
            }

            if (!MyUtility.Check.Empty(CuttingSP2))
            {
                sqlCmd.Append(string.Format(" and w.ID <= '{0}'", CuttingSP2));
            }

            if (!MyUtility.Check.Empty(Est_CutDate1))
            {
                sqlCmd.Append(string.Format(@" and w.EstCutDate between '{0}' and '{1}'",
                Convert.ToDateTime(Est_CutDate1).ToString("d"), Convert.ToDateTime(Est_CutDate2).ToString("d")));
            }
            
            if (!MyUtility.Check.Empty(EarliestSCIDelivery1))
            {
                sqlCmd.Append(string.Format(@" and MinSci.MinSCI between '{0}' and '{1}'",
                Convert.ToDateTime(EarliestSCIDelivery1).ToString("d"), Convert.ToDateTime(EarliestSCIDelivery2).ToString("d")));
            }
            
            if (!MyUtility.Check.Empty(EarliestSewingInline1))
            {
                sqlCmd.Append(string.Format(@" and c.SewInLine between '{0}' and '{1}'",
                Convert.ToDateTime(EarliestSewingInline1).ToString("d"), Convert.ToDateTime(EarliestSewingInline2).ToString("d")));
            }
            #endregion
                        
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            #region Append有選擇的condition
            condition.Clear();
            if (!MyUtility.Check.Empty(factory))
            {
                condition.Append(string.Format(@"Factory : {0}    "
                    , factory.ToString()
                    ));
            }
            if (!MyUtility.Check.Empty(CuttingSP1) && !MyUtility.Check.Empty(CuttingSP2))
            {
                condition.Append(string.Format(@"Cutting SP# : {0} ~ {1}    "
                    , CuttingSP1.ToString()
                    , CuttingSP1.ToString()
                    ));
            }
            if (!MyUtility.Check.Empty(Est_CutDate1) && !MyUtility.Check.Empty(Est_CutDate2))
            {
                condition.Append(string.Format(@"Est. Cut Date : {0} ~ {1}    "
                    , Convert.ToDateTime(Est_CutDate1).ToString("d")
                    , Convert.ToDateTime(Est_CutDate2).ToString("d")
                    ));
            }
            if (!MyUtility.Check.Empty(EarliestSCIDelivery1)&&!MyUtility.Check.Empty(EarliestSCIDelivery2))
            {
                condition.Append(string.Format(@"Earliest SCI Delivery : {0} ~ {1}    "
                    , Convert.ToDateTime(EarliestSCIDelivery1).ToString("d")
                    , Convert.ToDateTime(EarliestSCIDelivery2).ToString("d")
                    ));
            }
            if (!MyUtility.Check.Empty(EarliestSewingInline1)&&!MyUtility.Check.Empty(EarliestSewingInline2))
            {
                condition.Append(string.Format(@"Earliest Sewing Inline : {0} ~ {1}"
                    , Convert.ToDateTime(EarliestSewingInline1).ToString("d")
                    , Convert.ToDateTime(EarliestSewingInline2).ToString("d")
                    ));
            }
            #endregion
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
            MyUtility.Excel.CopyToXls(printData, "", "Cutting_R03_CuttingScheduleListReport.xltx", 4, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            objSheets.Cells[3, 3] = DateTime.Now.ToString();  // 列印日期寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
