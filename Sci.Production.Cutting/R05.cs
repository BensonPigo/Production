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
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string WorkOrder, factory;
        DateTime? Est_CutDate1, Est_CutDate2;
        StringBuilder condition_M = new StringBuilder();
        StringBuilder condition_F = new StringBuilder();
        StringBuilder condition_Est_CutDate = new StringBuilder();

        public R05(ToolStripMenuItem menuitem)
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
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) && MyUtility.Check.Empty(comboBox2.Text))
            {
                MyUtility.Msg.WarningBox("Can't all empty!!");
                return false;
            }
            WorkOrder = comboBox1.Text;
            factory = comboBox2.Text;
            Est_CutDate1 = dateRange1.Value1;
            Est_CutDate2 = dateRange1.Value2;

            return base.ValidateInput();
        }

        //非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select
	[Est. Inline]=c.CutInLine,
	[Est. Offline]=c.CutOffline,
	[Request#]=wo.CutplanID,
	[Cut Cell]=wo.CutCellID,
	[Line#]=iif( wo.CutplanID = '' , iif(wo.Type = 1,tmp1.SewingLineID,tmp2.SewingLineID),tmp3.SewingLineID),		
	[Est. Cutting Date]=wo.EstCutDate,
	[Master SP#]=wo.ID,
	[SP#]=stuff((
		select distinct concat('/',OrderID )
		from WorkOrder_Distribute 
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	),1,1,''),
	[Seq#]=(wo.Seq1+'-'+wo.Seq2),
	[Style#]=o.StyleID,
	[Ref#]=wo.CutRef,
	[Cut#]=wo.Cutno,
	[Comb.]=wo.FabricCombo,
	[Size Ratio]=stuff((
		select concat(',',SizeCode+'/'+Convert(varchar,Qty) )
		from WorkOrder_SizeRatio
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	),1,1,''),
	[Colorway]=stuff((
		select distinct concat('/',Article )
		from WorkOrder_Distribute 
		where WorkOrderUKey  = wo.UKey and Article != ''
		for xml path('')
	),1,1,''),
	[Color]=wo.ColorID,
	[Cut Qty]=stuff((
		select distinct concat(',',SizeCode+'/'+Convert(varchar,Qty*wo.Layer)  )
		from WorkOrder_SizeRatio 
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	),1,1,''),
	[Fab Cons.]=wo.Cons,
	[Fab Desc]=[Production].dbo.getMtlDesc(o.POID,wo.Seq1,wo.Seq2,2,0)
from 
	WorkOrder wo 
	inner join Orders o on wo.ID = o.CuttingSP
	inner join Cutting c on c.ID = wo.ID
	outer apply(
		Select top(1) SewingLineID from SewingSchedule_Detail where OrderID = wo.ID
	) as tmp1
	outer apply(
		Select top(1) SewingLineID
		from SewingSchedule_Detail sd, 
		(select top(1) OrderID, Article, SizeCode 
			from WorkOrder_Distribute 
			where WorkOrderUKey = wo.UKey
		) wd 
	where sd.OrderID = wd.OrderID and sd.Article = wd.Article and sd.SizeCode = wd.SizeCode
	) as tmp2
	outer apply(
		Select SewingLineID from Cutplan_Detail where ID = wo.CutplanID and WorkOrderUKey = wo.UKey
	) as tmp3
where 1=1
");
            #region Append條件字串
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
                sqlCmd.Append(string.Format(@" and wo.EstCutDate between '{0}' and '{1}'",
                Convert.ToDateTime(Est_CutDate1).ToString("d"), Convert.ToDateTime(Est_CutDate2).ToString("d")));
            }
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            #region Append有選擇的condition
            condition_Est_CutDate.Clear();
            condition_M.Clear();
            condition_F.Clear();

            if (!MyUtility.Check.Empty(Est_CutDate1) && !MyUtility.Check.Empty(Est_CutDate2))
            {
                condition_Est_CutDate.Append(string.Format(@"{0} ~ {1}"
                    , Convert.ToDateTime(Est_CutDate1).ToString("d")
                    , Convert.ToDateTime(Est_CutDate2).ToString("d")
                    ));
            }

            if (!MyUtility.Check.Empty(WorkOrder))
            {
                condition_M.Append(string.Format(@"{0}"
                    , WorkOrder.ToString()
                    ));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                condition_F.Append(string.Format(@"{0}"
                    , factory.ToString()
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R05_CuttingMonthlyForecast.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Cutting_R05_CuttingMonthlyForecast.xltx", 2, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 2] = condition_Est_CutDate.ToString();   // 條件字串寫入excel
            objSheets.Cells[1, 6] = condition_M.ToString();   // 條件字串寫入excel
            objSheets.Cells[1, 8] = condition_F.ToString();   // 條件字串寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }

    }
}
