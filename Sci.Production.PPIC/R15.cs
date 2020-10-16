using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Utility.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R15 : Win.Tems.PrintForm
    {
        private DataTable[] dtPrint;
        private string sqlcmd;

        /// <inheritdoc/>
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.cboReportType.Add("Each Cons", "EC");
            this.cboReportType.Add("M/Notice", "MN");
            this.cboReportType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            string where = string.Empty;
            string type = this.cboReportType.SelectedValue2.ToString();
            string sqlSelect3 = type.EqualString("EC") ? "o.EachConsApv" : "o.MnOrderApv";
            string sqlwhere3 = string.Empty;

            where += $"where f.Type = '{type}'";

            #region 檢查必輸條件 & 加入SQL where 參數

            if (this.dateSCIDelivery.Value1 != null && this.dateSCIDelivery.Value2 != null)
            {
                where += $" and o.SciDelivery >='{((DateTime)this.dateSCIDelivery.Value1).ToString("yyyy-MM-dd")}'";
                where += $" and o.SciDelivery <='{((DateTime)this.dateSCIDelivery.Value2).ToString("yyyy-MM-dd 23:59:59")}'";
                sqlwhere3 += $" and o.SciDelivery >='{((DateTime)this.dateSCIDelivery.Value1).ToString("yyyy-MM-dd")}'";
                sqlwhere3 += $" and o.SciDelivery <='{((DateTime)this.dateSCIDelivery.Value2).ToString("yyyy-MM-dd 23:59:59")}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text) || !MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $" and f.ID >= '{this.txtSP1.Text}'";
                where += $" and f.ID <= '{this.txtSP2.Text.PadRight(13, 'Z')}'";
                sqlwhere3 += $" and o.ID >= '{this.txtSP1.Text}'";
                sqlwhere3 += $" and o.ID <= '{this.txtSP2.Text.PadRight(13, 'Z')}'";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $" and o.BrandID = '{this.txtBrand.Text}'";
                sqlwhere3 += $" and o.BrandID = '{this.txtBrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtMR.TextBox1.Text))
            {
                where += $" and o.MRHandle = '{this.txtMR.TextBox1.Text}'";
                sqlwhere3 += $" and o.MRHandle = '{this.txtMR.TextBox1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSMR.TextBox1.Text))
            {
                where += $" and o.SMR = '{this.txtSMR.TextBox1.Text}'";
                sqlwhere3 += $" and o.SMR = '{this.txtSMR.TextBox1.Text}'";
            }

            this.sqlcmd = $@"
Select 
      f.KPIFailed
    , f.FailedComment
    , f.ExpectApvDate
    , f.KPIDate
    , o.ID
    , o.POID
    , o.BrandID
    , o.StyleID
    , o.FactoryID
    , o.Qty
    , SMR = GetSMR.IdAndName
    , MR = GetMR.IdAndName
    , o.SciDelivery
    , o.BuyerDelivery
    , o.SewInLIne
    , o.MnorderApv2
    , GetGMTLT.GMTLT
    , f.Type
into #tmp
From [Order_ECMNFailed] f
Left Join Orders o on f.id	= o.ID
Outer Apply(Select GMTLT = dbo.GetStyleGMTLT(o.BrandID,o.StyleID,o.SeasonID,o.FactoryID)) as GetGMTLT
Outer Apply (
    select IdAndName
    from dbo.GetPassName(o.SMR)
)GetSMR 
Outer Apply (
    select IdAndName
    from dbo.GetPassName(o.MRHandle)
)GetMR
{where}
Order by f.ID

select *
from #tmp

declare @sql varchar(max), @para varchar(max)

select @para = concat(stuff((
	  select concat('],[',tmp.SciDeliveryDiff) 
			from (
				select [SciDeliveryDiff] = datediff(day, getdate(), t2.SciDelivery)
					,t2.SciDelivery
				from #tmp t2
			    where t2.KPIFailed = 'Alread Failed'
				group by t2.SciDelivery
			) tmp 
			order by SciDelivery
		for xml path('')),1,2,'') , ']')

if len(@para) > 2
begin
	 set @sql = '
		SELECT KPIFailed, BrandID, SMR,   
			' + @para + '
		FROM  
		(
			select t.KPIFailed, t.BrandID, t.SMR, t.ID, SciDelivery = datediff(day, getdate(), t.SciDelivery)
			from #tmp t
            where t.KPIFailed = ''Alread Failed''
		) AS SourceTable  
		PIVOT ( Count(ID) FOR SciDelivery IN (' + @para + ')) AS PivotTable
		order by KPIFailed, BrandID, SMR
	 ' 
	 print @sql
	 exec(@sql)
end
else
begin
	select t.KPIFailed, t.BrandID, t.SMR
	from #tmp t
	where t.KPIFailed = 'Alread Failed'
end

select @para = concat(stuff((
	  select concat('],[',tmp.SciDeliveryDiff) 
			from (
				select [SciDeliveryDiff] = datediff(day, getdate(), t2.SciDelivery)
					,t2.SciDelivery
				from #tmp t2
			    where t2.KPIFailed = 'Failed Next Week'
				group by t2.SciDelivery
			) tmp 
			order by SciDelivery
		for xml path('')),1,2,'') , ']')

if len(@para) > 2
begin
	 set @sql = '
		SELECT KPIFailed, BrandID, SMR,   
			' + @para + '
		FROM  
		(
			select t.KPIFailed, t.BrandID, t.SMR, t.ID, SciDelivery = datediff(day, getdate(), t.SciDelivery)
			from #tmp t
			where t.KPIFailed = ''Failed Next Week''
		) AS SourceTable  
		PIVOT ( Count(ID) FOR SciDelivery IN (' + @para + ')) AS PivotTable
		order by KPIFailed, BrandID, SMR
	 ' 
	 print @sql
	 exec(@sql)
end
else
begin
	select t.KPIFailed, t.BrandID, t.SMR
	from #tmp t
	where t.KPIFailed = 'Failed Next Week'
end

select o.FactoryID, o.BrandID 
	, [Approved] = SUM(iif({sqlSelect3} is not null, o.CPU * o.Qty, 0))
	, [unApproved] = SUM(iif({sqlSelect3} is null, o.CPU * o.Qty, 0))
	, [total] = SUM(o.CPU * o.Qty)
from Orders o 
where o.Junk = 0
{sqlwhere3}
group by o.FactoryID, o.BrandID 
order by o.FactoryID, o.BrandID 

drop table #tmp
";
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(string.Empty, this.sqlcmd, out this.dtPrint);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrint[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtPrint[0].Rows.Count); // 顯示筆數

            string type = this.dtPrint[0].Rows[0]["Type"].ToString();
            this.dtPrint[0].Columns.Remove("Type");

            string excelFile = string.Empty;
            if (type.EqualString("EC"))
            {
                excelFile = "PPIC_R15 Cutting check list (E.Cons)";
            }
            else
            {
                excelFile = "PPIC_R15 Mnotice Cutting check list (M.Notice)";
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R15 Cutting check list (E.Cons).xltx"); // 開excelapp
            if (objApp == null)
            {
                return false;
            }

            bool result = MyUtility.Excel.CopyToXls(this.dtPrint[0], string.Empty, xltfile: excelFile + ".xltx", headerRow: 2, showExcel: false, excelApp: objApp);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[2];
            this.SetExcelSheet2(this.dtPrint[1], objSheets, 0);
            this.SetExcelSheet2(this.dtPrint[2], objSheets, this.dtPrint[1].Rows.Count == 0 ? 0 : this.dtPrint[1].Rows.Count + 7);

            objSheets = objApp.ActiveWorkbook.Worksheets[3];
            this.SetExcelSheet3(this.dtPrint[3], objSheets);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R15");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.ActiveWorkbook.Close(true);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();
            return true;
        }

        private void SetExcelSheet2(DataTable dt, Excel.Worksheet objSheets, int initIdx)
        {
            if (dt.Rows.Count <= 0)
            {
                return;
            }

            Excel.Range excelrange;
            int columnsCnt, rowsCnt;
            string today = DateTime.Now.ToString("MM/dd");

            objSheets.Name = "Summary (sci del)";
            objSheets.Cells[1 + initIdx, 1] = "Count of Order SP";
            objSheets.Range[string.Format("A{0}:C{0}", 1 + initIdx)].Merge();
            objSheets.Cells[2 + initIdx, 1] = "Kpi_Fail";
            objSheets.Cells[2 + initIdx, 2] = "Brand";
            objSheets.Cells[2 + initIdx, 3] = "SMR";

            rowsCnt = dt.Rows.Count;
            columnsCnt = dt.Columns.Count;
            objSheets.Cells[3 + initIdx, 1] = dt.Rows[0]["KPIFailed"].ToString();
            objSheets.Range[string.Format("A{0}:A{1}", 3 + initIdx, rowsCnt + 2 + initIdx)].Merge();
            excelrange = objSheets.GetRanges(string.Format("D{0}:{1}{0}", 1 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt + 1)));
            excelrange.Merge();
            excelrange.HorizontalAlignment = Excel.Constants.xlCenter; // 水平置中對齊
            excelrange.VerticalAlignment = Excel.Constants.xlCenter; // 垂直置中對齊
            excelrange.WrapText = true; // 自動換列
            objSheets.Cells[1 + initIdx, 4] = $"=\"SCI Del-Today {today} checking away from sci del.\"&CHAR(10)&\"({today} checking 距離 SCI 剩下天數)\"";
            objSheets.Cells[1 + initIdx, 4].RowHeight = 30;

            for (int i = 0; i <= columnsCnt - 4; i++)
            {
                objSheets.Cells[2 + initIdx, i + 4] = dt.Columns[i + 3].ToString();
                objSheets.Cells[rowsCnt + 3 + initIdx, i + 4] = string.Format("=Sum({0}{1}:{0}{2})", MyExcelPrg.GetExcelColumnName(i + 4), 3 + initIdx, rowsCnt + 2 + initIdx);
            }

            objSheets.Cells[2 + initIdx, columnsCnt + 1] = "Grand Total";

            object[,] objArray = new object[1, columnsCnt - 1];
            for (int i = 0; i <= rowsCnt - 1; i++)
            {
                DataRow dr = dt.Rows[i];
                for (int j = 0; j <= columnsCnt - 2; j++)
                {
                    objArray[0, j] = dr[j + 1];
                }

                objSheets.Range[string.Format("B{0}:{1}{0}", i + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt))].Value2 = objArray;
                objSheets.Range[string.Format("{1}{0}", i + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt + 1))].Value = string.Format("=Sum(D{0}:{1}{0})", i + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt));
            }

            objSheets.Cells[rowsCnt + 3 + initIdx, 1] = "Already failed Total";
            objSheets.Range[string.Format("A{0}:C{0}", rowsCnt + 3 + initIdx)].Merge();
            objSheets.Cells[rowsCnt + 3 + initIdx, columnsCnt + 1] = string.Format("=Sum(D{0}:{1}{0})", rowsCnt + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt));
            objSheets.Range["A:C"].Columns.AutoFit();
            objSheets.Range[string.Format("{0}:{1}", MyExcelPrg.GetExcelColumnName(columnsCnt + 1), MyExcelPrg.GetExcelColumnName(columnsCnt + 2))].Columns.AutoFit();

            excelrange = objSheets.get_Range(string.Format("A{1}:{0}{2}", MyExcelPrg.GetExcelColumnName(columnsCnt + 1), 1 + initIdx, MyUtility.Convert.GetString(rowsCnt + 3 + initIdx)), Type.Missing);
            excelrange.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            excelrange.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            excelrange.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
            excelrange.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
            excelrange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
            excelrange.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;

            objSheets.Range["A:C"].VerticalAlignment = Excel.Constants.xlCenter;
        }

        private void SetExcelSheet3(DataTable dt, Excel.Worksheet objSheets)
        {
            if (dt.Rows.Count <= 0)
            {
                return;
            }

            object[,] objArray = new object[1, dt.Columns.Count];
            string iFactory = string.Empty;
            int iRows = 0;
            bool ichange = true;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                DataRow dr = dt.Rows[i];
                if (i == 0)
                {
                    iFactory = dr["FactoryID"].ToString();
                    iRows = i + 2;
                }

                if (!iFactory.EqualString(dr["FactoryID"].ToString()))
                {
                    objSheets.Range[string.Format("A{0}:A{1}", iRows, i + 1)].Merge();
                    iFactory = dr["FactoryID"].ToString();
                    iRows = i + 2;
                    ichange = true;
                }

                objArray[0, 0] = ichange ? dr["FactoryID"].ToString() : string.Empty;
                objArray[0, 1] = dr["BrandID"].ToString();
                objArray[0, 2] = dr["Approved"].ToString();
                objArray[0, 3] = dr["unApproved"].ToString();
                objArray[0, 4] = dr["total"].ToString();

                objSheets.Range[string.Format("A{0}:E{0}", i + 2)].Value2 = objArray;
                ichange = false;
            }

            objSheets.Range[string.Format("A{0}", dt.Rows.Count + 2)].Value = "Total";
            objSheets.Range[string.Format("A{0}:B{0}", dt.Rows.Count + 2)].Merge();
            objSheets.Range[string.Format("C{0}", dt.Rows.Count + 2)].Value = string.Format("=Sum(C2:C{0})", dt.Rows.Count + 1);
            objSheets.Range[string.Format("D{0}", dt.Rows.Count + 2)].Value = string.Format("=Sum(D2:D{0})", dt.Rows.Count + 1);
            objSheets.Range[string.Format("E{0}", dt.Rows.Count + 2)].Value = string.Format("=Sum(E2:E{0})", dt.Rows.Count + 1);
            objSheets.Range["A:E"].Columns.AutoFit();
            objSheets.Range["A:B"].VerticalAlignment = Excel.Constants.xlCenter;

            Excel.Range rngBorders = objSheets.get_Range(string.Format("A1:E{0}", dt.Rows.Count + 2), Type.Missing);
            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
            rngBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
            rngBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
            rngBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
        }
    }
}
