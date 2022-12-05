﻿using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Data;
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
            StringBuilder sqlCmdW = new StringBuilder();
            sqlCmdW.Append(string.Format(
                " and FP.InspDate between '{0}' and '{1}'",
                ((DateTime)this.InspectionDate1).ToString("yyyy/MM/dd"),
                ((DateTime)this.InspectionDate2).ToString("yyyy/MM/dd")));
            if (!MyUtility.Check.Empty(this.uid))
            {
                string strUserid = string.Empty;
                string[] getUserId = this.uid.Split(',').Distinct().ToArray();
                foreach (var userID in getUserId)
                {
                    strUserid += ",'" + userID + "'";
                }

                sqlCmdW.Append(string.Format(" and FP.Inspector in ({0})", strUserid.Substring(1)));
            }

            if (!MyUtility.Check.Empty(this.sp1))
            {
                sqlCmdW.Append(string.Format(" and F.POID >= '{0}'", this.sp1));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlCmdW.Append(string.Format(" and F.POID <= '{0}'", this.sp2));
            }

            if (!MyUtility.Check.Empty(this.refno1))
            {
                sqlCmdW.Append(string.Format(" and p.RefNo >= '{0}'", this.refno1));
            }

            if (!MyUtility.Check.Empty(this.refno2))
            {
                sqlCmdW.Append(string.Format(" and p.RefNo <= '{0}'", this.refno2));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                string str_multi = string.Empty;
                foreach (string v_str in this.brand.Split(','))
                {
                    str_multi += "," + "'" + v_str + "'";
                }

                sqlCmdW.Append(string.Format(" and o.brandID in ({0})", str_multi.Substring(1)));
            }
            #endregion
            #region 主Sql
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"

SELECT [Inspected Date] = FP.InspDate
       ,[Inspector] = FP.Inspector
       ,o.brandID
       ,o.FtyGroup
       ,o.StyleID
       ,[SP#] = F.POID
       ,[SEQ#] = concat(RTRIM(F.SEQ1) ,'-',F.SEQ2)
       ,[StockType]=iif(isnull(rd. StockType, '') = '', '', (select Name from DropDownList ddl  where ddl.id like '%'+rd. StockType+'%' and ddl.Type = 'Pms_StockType'))
       ,[WK#] = rd.ExportID
	   ,[Supplier]=f.SuppID
	   ,[Supplier Name]=(select AbbEN from Supp where id = f.SuppID)
	   ,[ATA] = rd.WhseArrival
       ,[Roll#] = fp.Roll
       ,[Dyelot#] = fp.Dyelot
	   ,[Ref#] = RTRIM(p.RefNo)
	   ,[Color]=dbo.GetColorMultipleID(o.BrandID,p.ColorID)
       ,[Arrived YDS] = RD.StockQty
       ,[Actual YDS] = FP.ActualYds
       ,[LthOfDiff] = FP.ActualYds - FP.TicketYds
	   ,[TransactionID] = FP.TransactionID
	   ,[Shortage YDS] = isnull(isd.Qty, 0)
	   ,[QCIssueTransactionID] = isd.Id
       ,[Full Width] = ww.width
       ,[Actual Width] = FP.ActualWidth
       ,[Speed] = IIF((FP.QCTime- System.QCMachineDelayTime * FP.QCStopQty) <= 0, 0,
	                Round(FP.ActualYds/((FP.QCTime- System.QCMachineDelayTime * FP.QCStopQty)/60),2))
	   ,[Total Defect Points]=FP.TotalPoint
       ,[Grade] = FP.Grade
       ,[ActualInspectionTimeStart] = FP.StartTime
	   ,[inspectionTimeStart] = DATEADD(second, FP.QCTime*-1, FP.AddDate)
	   ,[inspectionTimeFinish] = FP.AddDate
       ,[Remark]=FP.Remark
       ,[MCHandle]= dbo.getPass1_ExtNo(o.MCHandle)
       ,Fabric.WeaveTypeID
	   ,[InspectorName] = Pass1.Name
       ,[QCMachineStopTime] = case when fp.AddDate is null or fp.StartTime is null then fp.QCTime
								   else DATEDIFF(SECOND,fp.StartTime,fp.AddDate) - fp.QCTime end
	   ,[QCMachineRunTime] = fp.QCTime
into #tmp
FROM System,FIR_Physical AS FP
LEFT JOIN FIR AS F ON FP.ID=F.ID
LEFT JOIN View_AllReceivingDetail RD ON RD.PoId= F.POID AND RD.Seq1 = F.SEQ1 AND RD.Seq2 = F.SEQ2
								AND RD.Roll = FP.Roll AND RD.Dyelot = FP.Dyelot
LEFT join PO_Supp_Detail p on p.ID = f.poid and p.seq1 = f.seq1 and p.seq2 = f.seq2
LEFT join orders o on o.id=f.POID
LEFT JOIN Fabric on Fabric.SCIRefno  = f.SCIRefno
LEFT JOIN Issue_Detail isd on FP.Issue_DetailUkey = isd.ukey and isd.IsQMS = 1
LEFT JOIN Pass1 on Pass1.ID = FP.Inspector
outer apply(select width from Fabric with(nolock) where SCIRefno = f.SCIRefno)ww
WHERE 1=1
{0}
ORDER BY [Inspected Date],[Inspector],[SP#],[SEQ#],[Roll#],[Dyelot#]
",
                sqlCmdW.ToString()));
            if (this.radioDetail.Checked)
            {
                sqlCmd.Append($@"
select 
        [Inspected Date] ,[Inspector] ,[InspectorName] ,brandID
       ,FtyGroup ,StyleID ,[SP#] 
       ,[SEQ#] 
       ,[StockType]
       ,[WK#] 
	   ,[Supplier]
	   ,[Supplier Name]
	   ,[ATA]
       ,[Roll#]
       ,[Dyelot#] 
	   ,[Ref#]
	   ,[Color]
       ,[Arrived YDS] 
       ,[Actual YDS] 
       ,[LthOfDiff] 
	   ,[TransactionID] 
	   ,[Shortage YDS] 
	   ,[QCIssueTransactionID] 
       ,[Full Width] 
       ,[Actual Width] 
       ,[Speed] 
	   ,[Total Defect Points]
       ,[Grade]
       ,[ActualInspectionTimeStart]
	   ,[inspectionTimeStart] 
	   ,[inspectionTimeFinish]
       ,[QCMachineStopTime]
	   ,[QCMachineRunTime]
       ,[Remark]
       ,[MCHandle]
       ,WeaveTypeID
from #tmp ORDER BY [Inspected Date],[Inspector],[SP#],[SEQ#],[Roll#],[Dyelot#]");
            }
            else
            {
                sqlCmd.Append($@"
select * from (
	select inspector
	,[QCName] = (select name from pass1 where id=inspector)
	,[inspected date]
	,[Roll] = count([Roll#])
	,[Actual YDS] = ROUND(sum([Actual YDS]),1)
	from #tmp
	where InspectorName is not null
	group by inspector,[inspected date]

	union all

	select inspector
	,[QCName] = (select Name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = inspector)
	,[inspected date]
	,[Roll] = count([Roll#])
	,[Actual YDS] = ROUND(sum([Actual YDS]),1)
	from #tmp
	where InspectorName is null
	group by inspector,[inspected date]
) a
order by inspector,[inspected date]

-- 取得所有QC人員
select * from (
	select distinct Inspector
	,[QCName] = (select name from pass1 where id=inspector) 
	from #tmp 
	where InspectorName is not null

	union all

	select distinct Inspector
	,[QCName] = (select Name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = inspector) 
	from #tmp 
	where InspectorName is null
) a
order by Inspector

-- 依日期 加總Roll and Yard
select [inspected date],count([Roll#])Roll,sum([Actual YDS])[Actual YDS]  
from #tmp
group by [inspected date]
order by [inspected date]

-- 取得Woven Yard
select [inspected date],sum([Actual YDS])[Actual YDS] , WeaveTypeID='Woven'
from #tmp
where WeaveTypeID='Woven'
group by [inspected date]
order by [inspected date]

-- 取得Knit Yard
select [inspected date],sum([Actual YDS])[Actual YDS]  , WeaveTypeID='Knit'
from #tmp
where WeaveTypeID='knit'
group by [inspected date]
order by [inspected date]
 
");
            }

            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
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
