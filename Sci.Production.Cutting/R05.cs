using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string WorkOrder;
        string factory;
        DateTime? Est_CutDate1;
        DateTime? Est_CutDate2;

        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable WorkOrder;
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrder WITH (NOLOCK) ", out WorkOrder);
            MyUtility.Tool.SetupCombox(this.comboM, 1, WorkOrder);
            this.comboM.Text = Sci.Env.User.Keyword;

            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            this.WorkOrder = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.Est_CutDate1 = this.dateEstCutDate.Value1;
            this.Est_CutDate2 = this.dateEstCutDate.Value2;

            // if (MyUtility.Check.Empty(Est_CutDate1) && MyUtility.Check.Empty(Est_CutDate2))
            // {
            //    MyUtility.Msg.WarningBox("Can't all empty!!");
            //    return false;
            // }
            return base.ValidateInput();
        }

        // 非同步取資料
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
from WorkOrder wo WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on wo.ID = o.CuttingSP
inner join Cutting c WITH (NOLOCK) on c.ID = wo.ID
outer apply(
	Select top(1) SewingLineID 
	from SewingSchedule_Detail WITH (NOLOCK) 
	where OrderID = wo.ID
) as tmp1
outer apply(
	Select top(1) SewingLineID
	from SewingSchedule_Detail sd WITH (NOLOCK) , 
	(select top(1) OrderID, Article, SizeCode 
		from WorkOrder_Distribute WITH (NOLOCK) 
		where WorkOrderUKey = wo.UKey
	) wd
	where sd.OrderID = wd.OrderID 
	and sd.Article = wd.Article 
	and sd.SizeCode = wd.SizeCode
) as tmp2
outer apply(
	Select SewingLineID 
	from Cutplan_Detail WITH (NOLOCK) 
	where ID = wo.CutplanID 
	and WorkOrderUKey = wo.UKey
) as tmp3
outer apply(
	select SP=(
		select distinct concat('/',OrderID )
		from WorkOrder_Distribute WITH (NOLOCK) 
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	)
) as SP
outer apply(
	select Qty=(
		select concat(',',SizeCode+'/'+Convert(varchar,Qty) )
		from WorkOrder_SizeRatio WITH (NOLOCK) 
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	)
) as Qty
outer apply(
	select Article=(
		select distinct concat('/',Article )
		from WorkOrder_Distribute WITH (NOLOCK) 
		where WorkOrderUKey  = wo.UKey 
		and Article != ''
		for xml path('')
	)
) as Article
outer apply(
	select m = (
		select distinct concat(',',SizeCode+'/'+Convert(varchar,Qty*wo.Layer)  )
		from WorkOrder_SizeRatio WITH (NOLOCK) 
		where WorkOrderUKey  = wo.UKey
		for xml path('')
	)
) as m

where 1=1
");
            #region Append條件字串
            if (!MyUtility.Check.Empty(this.WorkOrder))
            {
                sqlCmd.Append(string.Format(" and wo.MDivisionID = '{0}'", this.WorkOrder));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.Est_CutDate1))
            {
                sqlCmd.Append(string.Format(" and wo.EstCutDate >= '{0}' ", Convert.ToDateTime(this.Est_CutDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Est_CutDate2))
            {
                sqlCmd.Append(string.Format(" and wo.EstCutDate <= '{0}' ", Convert.ToDateTime(this.Est_CutDate2).ToString("d")));
            }
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
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
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R05_CuttingMonthlyForecast.xltx"); // 預先開啟excel app
            foreach (DataRow dr in this.printData.Rows)
            {
                dr["Fab Desc"] = dr["Fab Desc"].ToString().Trim();
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Cutting_R05_CuttingMonthlyForecast.xltx", 2, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 2] = string.Format(@"{0} ~ {1}", Convert.ToDateTime(this.Est_CutDate1).ToString("d"), Convert.ToDateTime(this.Est_CutDate2).ToString("d")); // 條件字串寫入excel
            objSheets.Cells[1, 6] = this.WorkOrder.ToString();   // 條件字串寫入excel
            objSheets.Cells[1, 8] = this.factory.ToString();   // 條件字串寫入excel
            objSheets.Columns[8].ColumnWidth = 13.5;
            objSheets.Columns[14].ColumnWidth = 12.85;
            objSheets.Columns[15].ColumnWidth = 13.5;
            objSheets.Columns[17].ColumnWidth = 11;
            objSheets.Columns[19].ColumnWidth = 67;
            objSheets.Rows.AutoFit();

            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R05_CuttingMonthlyForecast");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            if (objSheets != null)
            {
                Marshal.FinalReleaseComObject(objSheets);    // 釋放sheet
            }

            if (objApp != null)
            {
                Marshal.FinalReleaseComObject(objApp);          // 釋放objApp
            }

            strExcelName.OpenFile();
            return true;
        }
    }
}
