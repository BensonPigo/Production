using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R08 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private string Excelfile;
        private string sp1;
        private string sp2;
        private string uid;
        private string brand;
        private string refno1;
        private string refno2;
        private DateTime? InspectionDate1;
        private DateTime? InspectionDate2;
        private Color green = Color.FromArgb(153, 204, 0);
        private Color blue = Color.FromArgb(101, 215, 255);
        private Color zero = Color.FromArgb(250, 191, 143);

        /// <summary>
        /// Initializes a new instance of the <see cref="R08"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtbrand.MultiSelect = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.InspectionDate1 = this.dateInspectionDate.Value1;
            this.InspectionDate2 = this.dateInspectionDate.Value2;
            this.uid = this.txtmulituser.TextBox1.Text;
            this.sp1 = this.txtSPStart.Text;
            this.sp2 = this.txtSPEnd.Text;
            this.brand = this.txtbrand.Text;
            this.refno1 = this.txtRefno1.Text;
            this.refno2 = this.txtRefno2.Text;
            if (MyUtility.Check.Empty(this.InspectionDate1) || MyUtility.Check.Empty(this.InspectionDate2))
            {
                MyUtility.Msg.WarningBox("< Inspected Date > cannot be empty!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 畫面上的條件
            List<SqlParameter> listPar = new List<SqlParameter>();

            listPar.Add(new SqlParameter("@InspectionDateFrom", this.InspectionDate1));
            listPar.Add(new SqlParameter("@InspectionDateTo", this.InspectionDate2));
            listPar.Add(new SqlParameter("@Inspectors", this.uid));
            listPar.Add(new SqlParameter("@POIDFrom", this.sp1));
            listPar.Add(new SqlParameter("@POIDTo", this.sp2));
            listPar.Add(new SqlParameter("@RefNoFrom", this.refno1));
            listPar.Add(new SqlParameter("@RefNoTo", this.refno2));
            listPar.Add(new SqlParameter("@BrandIDs", this.brand));

            #endregion
            #region 主Sql
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
Declare @InspMachine_FabPrepareTime_Woven int
Declare @InspMachine_FabPrepareTime_Other int
Declare @InspMachine_DefectPointTime int

select  @InspMachine_FabPrepareTime_Woven = InspMachine_FabPrepareTime_Woven,
        @InspMachine_FabPrepareTime_Other = InspMachine_FabPrepareTime_Other,
        @InspMachine_DefectPointTime = InspMachine_DefectPointTime
from    [ExtendServer].ManufacturingExecution.dbo.system

SELECT  InspectionStatus
        ,InspDate
        ,Inspector
        ,InspectorName
        ,BrandID
        ,Factory
        ,StyleID
        ,POID
        ,SEQ
        ,StockType
        ,Wkno
        ,SuppID
        ,SuppName
        ,ATA
        ,Roll
        ,Dyelot
        ,RefNo
        ,Color
        ,ArrivedYDS
        ,ActualYDS
        ,LthOfDiff
        ,TransactionID
        ,QCIssueQty
        ,QCIssueTransactionID
        ,CutWidth
        ,ActualWidth
        ,Speed
        ,TotalDefectPoints
		,PointRatePerRoll
        ,Grade
        ,InspectionStartTime
        ,InspectionFinishTime 
        ,MachineDownTime
        ,MachineRunTime
        ,Remark
        ,MCHandle
        ,WeaveType
	    ,ReceivingID 
        ,MachineIoTUkey
into #tmp
FROM dbo.GetQA_R08_Detail(@InspectionDateFrom, @InspectionDateTo, @Inspectors, @POIDFrom, @POIDTo, @RefNoFrom, @RefNoTo, @BrandIDs, null, null, @InspMachine_FabPrepareTime_Woven, @InspMachine_FabPrepareTime_Other, @InspMachine_DefectPointTime)
");
            if (this.radioDetail.Checked)
            {
                sqlCmd.Append($@"

DECLARE @QASortOutStandard decimal(5,2) = (SELECT QASortOutStandard FROM [SYSTEM])

select  t.InspectionStatus
        ,t.InspDate
        ,t.Inspector
        ,t.InspectorName
        ,t.BrandID
        ,t.Factory
        ,t.StyleID
        ,t.POID
        ,t.SEQ
        ,t.StockType
        ,t.Wkno
        ,t.SuppID
        ,t.SuppName
        ,t.ATA
        ,t.Roll
        ,t.Dyelot
        ,t.RefNo
        ,t.Color
        ,t.ArrivedYDS
        ,t.ActualYDS
        ,t.LthOfDiff
        ,t.TransactionID
        ,t.QCIssueQty
        ,t.QCIssueTransactionID
        ,t.CutWidth
        ,t.ActualWidth
        ,t.Speed
        ,t.TotalDefectPoints
		,t.PointRatePerRoll
        ,t.Grade
        ,[SortOut] = IIf (t.PointRatePerRoll >= @QASortOutStandard , 'Y', 'N')
        ,t.InspectionStartTime
        ,t.InspectionFinishTime 
        ,t.MachineDownTime
        ,t.MachineRunTime
        ,t.Remark
        ,t.MCHandle
        ,t.WeaveType
        ,m.MachineID
from #tmp t
left join [ExtendServer].ManufacturingExecution.dbo.MachineIoT m on t.MachineIoTUkey = m.Ukey
ORDER BY t.InspDate, t.Inspector, t.POID, t.SEQ, t.Roll, t.Dyelot");
            }
            else
            {
                sqlCmd.Append($@"
select * into #tmpGroupActualYDS from (
	select inspector
	,[QCName] = InspectorName
	,[inspected date] = InspDate
	,[Roll] = count([Roll])
	,[Actual YDS] = ROUND(sum(ActualYDS), 1)
	from #tmp
	where InspectorName is not null
	group by inspector, InspectorName, InspDate

	union all

	select inspector
	,[QCName] = (select Name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = Inspector)
	,[inspected date] = InspDate
	,[Roll] = count([Roll])
	,[Actual YDS] = ROUND(sum(ActualYDS), 1)
	from #tmp
	where InspectorName is null
	group by inspector, InspDate
) a

select * from #tmpGroupActualYDS
order by inspector,[inspected date]

-- 取得所有QC人員
select  distinct Inspector, QCName 
from #tmpGroupActualYDS
order by Inspector

-- 依日期 加總Roll and Yard
select [inspected date] = InspDate, [Roll] = count(Roll), [Actual YDS] = sum(ActualYDS) 
from #tmp
group by InspDate
order by InspDate

-- 取得Woven Yard
select [inspected date] = InspDate, [Actual YDS] = sum(ActualYDS) , WeaveTypeID = 'Woven'
from #tmp
where WeaveType = 'Woven'
group by InspDate
order by InspDate

-- 取得Knit Yard
select [inspected date] = InspDate, [Actual YDS] = sum(ActualYDS) , WeaveTypeID = 'Knit'
from #tmp
where WeaveType = 'knit'
group by InspDate
order by InspDate
 
");
            }

            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), listPar, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string xltx = string.Empty;
            if (this.radioDetail.Checked)
            {
                xltx = "Quality_R08.xltx";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, xltx, 1, true, null, objApp); // 將datatable copy to excel
            }
            else
            {
                xltx = "Quality_R08_Summery.xltx";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
                Excel.Worksheet wksheet = objApp.Sheets[1];
#if DEBUG
                objApp.Visible = true;
#endif

                int rowAdd = 0;
                int columnAdd = 2;
                string rgStr = string.Empty;

                //// 插入日期
                int daydiff = this.printData[2].Rows.Count;
                for (int i = 0; i < this.printData[2].Rows.Count; i++)
                {
                    // 寫入日期
                    wksheet.Cells[5 + i, 1] = ((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("MM/dd");
                }

                rgStr = string.Format($"A5:A{daydiff + 8}");
                Excel.Range rng = wksheet.get_Range(rgStr);
                rng.Font.Bold = true;

                // 插入QC資料
                wksheet.Cells[2, 1] = "QC";
                wksheet.Cells[2, 1].Interior.Color = this.green;
                wksheet.Cells[2, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                wksheet.Cells[3, 1] = "Date";
                wksheet.Cells[3, 1].Interior.Color = this.green;
                wksheet.Cells[3, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                if (this.printData[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in this.printData[1].Rows)
                    {
                        // QC ID
                        wksheet.Cells[2, columnAdd] = dr["Inspector"].ToString();
                        rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}2:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}2");
                        Excel.Range merge = wksheet.get_Range(rgStr);
                        merge.Interior.Color = this.green;
                        merge.Merge();
                        merge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // QC Name
                        wksheet.Cells[3, columnAdd] = dr["QCName"].ToString();
                        rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}3:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}3");
                        merge = wksheet.get_Range(rgStr);
                        merge.Interior.Color = this.blue;
                        merge.Merge();
                        merge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // Header
                        wksheet.Cells[4, columnAdd] = "Roll";
                        wksheet.Cells[4, columnAdd + 1] = "Yard";

                        // Loop天數的次數
                        rowAdd = 5;
                        int cnt = 0;
                        for (int i = 0; i < this.printData[2].Rows.Count; i++)
                        {
                            DataRow[] selected = this.printData[0].Select($@"inspector='{dr["Inspector"]}' and [inspected date]='{((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                            if (selected.Length > 0)
                            {
                                wksheet.Cells[rowAdd, columnAdd] = selected[0].ItemArray[3];
                                wksheet.Cells[rowAdd, columnAdd + 1] = selected[0].ItemArray[4];
                                cnt++;
                            }
                            else
                            {
                                wksheet.Cells[rowAdd, columnAdd] = 0;
                                wksheet.Cells[rowAdd, columnAdd].Interior.Color = this.zero;
                                wksheet.Cells[rowAdd, columnAdd + 1] = 0;
                                wksheet.Cells[rowAdd, columnAdd + 1].Interior.Color = this.zero;
                            }

                            rowAdd++;
                        }

                        // Totoal
                        wksheet.Cells[rowAdd, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";
                        wksheet.Cells[rowAdd, columnAdd + 1] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd + 1)}5:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}{rowAdd - 1})";

                        // Avg
                        wksheet.Cells[rowAdd + 1, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{cnt},1)";
                        wksheet.Cells[rowAdd + 1, columnAdd + 1] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd + 1)}5:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}{rowAdd - 1})/{cnt},1)";

                        columnAdd += 2;
                    }

                    // 加入Total Header
                    wksheet.Cells[2, columnAdd] = "Daily output";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}2:{MyExcelPrg.GetExcelColumnName(columnAdd + 3)}2");
                    Excel.Range mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = this.green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Woven
                    wksheet.Cells[3, columnAdd] = "Woven";
                    wksheet.Cells[3, columnAdd].Interior.Color = this.green;
                    wksheet.Cells[4, columnAdd] = "Yard";

                    // Loop天數的次數
                    int cntWoven = 0;
                    for (int i = 0; i < daydiff; i++)
                    {
                        DataRow[] selected = this.printData[3].Select($@"[inspected date]='{((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                        if (selected.Length > 0)
                        {
                            wksheet.Cells[5 + i, columnAdd] = selected[0].ItemArray[1];
                            cntWoven++;
                        }
                        else
                        {
                            wksheet.Cells[5 + i, columnAdd] = 0;
                            wksheet.Cells[5 + i, columnAdd].Interior.Color = this.zero;
                        }
                    }

                    // Totoal
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{cntWoven},1)";

                    // Knit
                    columnAdd++;
                    wksheet.Cells[3, columnAdd] = "Knit";
                    wksheet.Cells[3, columnAdd].Interior.Color = this.green;
                    wksheet.Cells[4, columnAdd] = "Yard";

                    for (int i = 0; i < this.printData[4].Rows.Count; i++)
                    {
                        wksheet.Cells[5 + i, columnAdd] = this.printData[4].Rows[i]["Actual YDS"];
                    }

                    // Loop天數的次數
                    int cntKnit = 0;
                    for (int i = 0; i < daydiff; i++)
                    {
                        DataRow[] selected = this.printData[4].Select($@"[inspected date]='{((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                        if (selected.Length > 0)
                        {
                            wksheet.Cells[5 + i, columnAdd] = selected[0].ItemArray[1];
                            cntKnit++;
                        }
                        else
                        {
                            wksheet.Cells[5 + i, columnAdd] = 0;
                            wksheet.Cells[5 + i, columnAdd].Interior.Color = this.zero;
                        }
                    }

                    // Totoal
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{cntKnit},1)";

                    // Total
                    columnAdd++;
                    wksheet.Cells[3, columnAdd] = "Total";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}3:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}3");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = this.green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    for (int i = 0; i < this.printData[2].Rows.Count; i++)
                    {
                        wksheet.Cells[5 + i, columnAdd] = this.printData[2].Rows[i]["Roll"];
                        wksheet.Cells[5 + i, columnAdd + 1] = this.printData[2].Rows[i]["Actual YDS"];
                    }

                    wksheet.Cells[4, columnAdd] = "Roll";

                    // Sum
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{daydiff},1)";

                    columnAdd++;
                    wksheet.Cells[4, columnAdd] = "Yard";

                    // Sum
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";
                    wksheet.Cells[daydiff + 5, columnAdd].Font.Color = Color.Red;

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{daydiff},1)";
                    wksheet.Cells[daydiff + 6, columnAdd].Font.Color = Color.FromArgb(0, 112, 192);

                    // Remark
                    columnAdd++;
                    wksheet.Cells[2, columnAdd] = "Remark";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}2:{MyExcelPrg.GetExcelColumnName(columnAdd)}4");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = this.green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Daily Output Format
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd - 4)}2:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd + 1}");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Font.Bold = true;

                    // Total,Ave Format
                    rgStr = string.Format($"A{rowAdd}:{MyExcelPrg.GetExcelColumnName(columnAdd - 1)}{rowAdd + 1}");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Font.Bold = true;
                    mergerge.Interior.Color = Color.FromArgb(192, 192, 192);
                    wksheet.Cells[daydiff + 5, 1] = "Total";
                    wksheet.Cells[daydiff + 6, 1] = "Average";

                    // 畫線
                    wksheet.get_Range($"A2:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd + 1}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    rgStr = $"A1:{MyExcelPrg.GetExcelColumnName(columnAdd)}1";
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                objApp.Columns.AutoFit();
                this.Excelfile = Class.MicrosoftFile.GetName("Quality_R08_Summery");
                objApp.ActiveWorkbook.SaveAs(this.Excelfile);
                objApp.Quit();
                Marshal.ReleaseComObject(wksheet);
                Marshal.ReleaseComObject(objApp);
                this.Excelfile.OpenFile();
            }

            return true;
        }
    }
}
