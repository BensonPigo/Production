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

        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable WorkOrder, factory;
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrder", out WorkOrder);
            MyUtility.Tool.SetupCombox(cmb_M, 1, WorkOrder);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(cmb_Factory, 1, factory);
            cmb_M.Text = Sci.Env.User.Keyword;
            cmb_Factory.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {            
            WorkOrder = cmb_M.Text;
            factory = cmb_Factory.Text;
            Est_CutDate1 = dateR_EstCutDate.Value1;
            Est_CutDate2 = dateR_EstCutDate.Value2;

            if (MyUtility.Check.Empty(Est_CutDate1) && MyUtility.Check.Empty(Est_CutDate2))
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
	[Est. Inline]=c.CutInLine,
	[Est. Offline]=c.CutOffline,
	[Request#]=wo.CutplanID,
	[Cut Cell]=wo.CutCellID,
	[Line#]=iif( wo.CutplanID = '' , iif(wo.Type = 1,tmp1.SewingLineID,tmp2.SewingLineID),tmp3.SewingLineID),
	[Est. Cutting Date]=wo.EstCutDate,
	[Master SP#] = wo.ID,
	[SP#] = stuff(SP.SP,1,1,''),
	[Seq#]=(wo.Seq1+'-'+wo.Seq2),
	[Style#]=o.StyleID,
	[Ref#]=wo.CutRef,
	[Cut#]=wo.Cutno,
	[Comb.]=wo.FabricCombo,
	[Size Ratio]=stuff(Qty.Qty,1,1,''),
	[Colorway]=stuff(Article.Article,1,1,''),
	[Color]=wo.ColorID,
	[Cut Qty]=stuff(m.m,1,1,''),
	[Fab Cons.] = wo.Cons,
	[Fab Desc] = [Production].dbo.getMtlDesc(o.POID,wo.Seq1,wo.Seq2,2,0)
from WorkOrder wo 
inner join Orders o on wo.ID = o.CuttingSP
inner join Cutting c on c.ID = wo.ID
outer apply(
	Select top(1) SewingLineID 
	from SewingSchedule_Detail 
	where OrderID = wo.ID
) as tmp1
outer apply(
	Select top(1) SewingLineID
	from SewingSchedule_Detail sd, 
	(select top(1) OrderID, Article, SizeCode 
		from WorkOrder_Distribute 
		where WorkOrderUKey = wo.UKey
	) wd
	where sd.OrderID = wd.OrderID 
	and sd.Article = wd.Article 
	and sd.SizeCode = wd.SizeCode
) as tmp2
outer apply(
	Select SewingLineID 
	from Cutplan_Detail 
	where ID = wo.CutplanID 
	and WorkOrderUKey = wo.UKey
) as tmp3
outer apply(
	select SP=(
		select distinct concat('/',OrderID )
		from WorkOrder_Distribute 
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	)
) as SP
outer apply(
	select Qty=(
		select concat(',',SizeCode+'/'+Convert(varchar,Qty) )
		from WorkOrder_SizeRatio
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	)
) as Qty
outer apply(
	select Article=(
		select distinct concat('/',Article )
		from WorkOrder_Distribute 
		where WorkOrderUKey  = wo.UKey 
		and Article != ''
		for xml path('')
	)
) as Article
outer apply(
	select m = (
		select distinct concat(',',SizeCode+'/'+Convert(varchar,Qty*wo.Layer)  )
		from WorkOrder_SizeRatio 
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	)
) as m

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
            objSheets.Cells[1, 2] = string.Format(@"{0} ~ {1}", Convert.ToDateTime(Est_CutDate1).ToString("d"), Convert.ToDateTime(Est_CutDate2).ToString("d"));// 條件字串寫入excel
            objSheets.Cells[1, 6] = WorkOrder.ToString();   // 條件字串寫入excel
            objSheets.Cells[1, 8] = factory.ToString();   // 條件字串寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }

    }
}
