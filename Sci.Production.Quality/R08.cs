using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        DataTable[] printData;
        private string Excelfile;
        string sp1, sp2, uid, brand, refno1, refno2;
        DateTime? InspectionDate1, InspectionDate2;
        private Color green = Color.FromArgb(153, 204, 0);
        private Color blue = Color.FromArgb(101, 215, 255);
        private Color zero = Color.FromArgb(250, 191, 143);

        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtbrand.MultiSelect = true;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            InspectionDate1 = dateInspectionDate.Value1;
            InspectionDate2 = dateInspectionDate.Value2;
            uid = txtmulituser.TextBox1.Text;
            sp1 = txtSPStart.Text;
            sp2 = txtSPEnd.Text;
            brand = txtbrand.Text;
            refno1 = txtRefno1.Text;
            refno2 = txtRefno2.Text;
            if (MyUtility.Check.Empty(InspectionDate1) || MyUtility.Check.Empty(InspectionDate2))
            {
                MyUtility.Msg.WarningBox("< Inspected Date > cannot be empty!");
                return false;
            }
            return base.ValidateInput();
        }

        //非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 畫面上的條件
            StringBuilder sqlCmdW = new StringBuilder();
            sqlCmdW.Append(string.Format(" and FP.InspDate between '{0}' and '{1}'"
                , ((DateTime)InspectionDate1).ToString("d"), ((DateTime)InspectionDate2).ToString("d")));
            if (!MyUtility.Check.Empty(uid))
            {
                string strUserid = string.Empty;
                string[] getUserId = uid.Split(',').Distinct().ToArray();
                foreach (var userID in getUserId)
                {
                    strUserid += ",'" + userID + "'";
                }
                sqlCmdW.Append(string.Format(" and FP.Inspector in ({0})", strUserid.Substring(1)));
            }
                
            if (!MyUtility.Check.Empty(sp1))
                sqlCmdW.Append(string.Format(" and F.POID >= '{0}'", sp1));
            if (!MyUtility.Check.Empty(sp2))
                sqlCmdW.Append(string.Format(" and F.POID <= '{0}'", sp2));
            if (!MyUtility.Check.Empty(refno1))
                sqlCmdW.Append(string.Format(" and p.RefNo >= '{0}'", refno1));
            if (!MyUtility.Check.Empty(refno2))
                sqlCmdW.Append(string.Format(" and p.RefNo <= '{0}'", refno2));
            if (!MyUtility.Check.Empty(brand))
            {
                string str_multi = "";
                foreach (string v_str in brand.Split(','))
                {
                    str_multi += "," + "'" + v_str + "'";
                }
                sqlCmdW.Append(string.Format(" and o.brandID in ({0})", str_multi.Substring(1)));                
            }
            #endregion
            #region 主Sql
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"

SELECT [Inspected Date] = FP.InspDate
       ,[Inspector] = FP.Inspector
       ,o.brandID
       ,o.FtyGroup
       ,o.StyleID
       ,[SP#] = F.POID
       ,[SEQ#] = concat(RTRIM(F.SEQ1) ,'-',F.SEQ2)
       ,[StockType]=(select Name from DropDownList ddl  where ddl.id like '%'+rd. StockType+'%' and ddl.Type = 'Pms_StockType')
       ,[WK#] = rd.ExportID
	   ,[Supplier]=f.SuppID
	   ,[Supplier Name]=(select AbbEN from Supp where id = f.SuppID)
	   ,[ATA] = p.FinalETA 
       ,[Roll#] = fp.Roll
       ,[Dyelot#] = fp.Dyelot
	   ,[Ref#]=p.RefNo
	   ,[Color]=dbo.GetColorMultipleID(o.BrandID,p.ColorID)
       ,[Arrived YDS] = RD.StockQty
       ,[Actual YDS] = FP.ActualYds
       ,[Shortage YDS] = isnull(isd.Qty, 0)
       ,[Full Width] = ww.width
       ,[Actual Width] = FP.ActualWidth
       ,[Speed] = IIF((FP.QCTime- System.QCMachineDelayTime * FP.QCStopQty) <= 0, 0,
	                Round(FP.ActualYds/((FP.QCTime- System.QCMachineDelayTime * FP.QCStopQty)/60),2))
	   ,[Total Defect Points]=FP.TotalPoint
       ,[Grade] = FP.Grade
	   ,[inspectionTimeStart] = DATEADD(second, FP.QCTime*-1, FP.AddDate)
	   ,[inspectionTimeFinish] = FP.AddDate
       ,[Remark]=FP.Remark
       ,[MCHandle]= dbo.getPass1_ExtNo(o.MCHandle)
       ,Fabric.WeaveTypeID
into #tmp
FROM System,FIR_Physical AS FP
LEFT JOIN FIR AS F ON FP.ID=F.ID
LEFT JOIN View_AllReceivingDetail RD ON RD.PoId= F.POID AND RD.Seq1 = F.SEQ1 AND RD.Seq2 = F.SEQ2
								AND RD.Roll = FP.Roll AND RD.Dyelot = FP.Dyelot
LEFT join PO_Supp_Detail p on p.ID = f.poid and p.seq1 = f.seq1 and p.seq2 = f.seq2
LEFT join orders o on o.id=f.POID
LEFT JOIN Fabric on Fabric.SCIRefno  = f.SCIRefno
LEFT JOIN Issue_Detail isd on FP.Issue_DetailUkey = isd.ukey and isd.IsQMS = 1
outer apply(select width from Fabric with(nolock) where SCIRefno = f.SCIRefno)ww
WHERE 1=1
{0}
ORDER BY [Inspected Date],[Inspector],[SP#],[SEQ#],[Roll#],[Dyelot#]
"
            , sqlCmdW.ToString()));
            if (this.radioDetail.Checked)
            {
                sqlCmd.Append($@"select * from #tmp ORDER BY [Inspected Date],[Inspector],[SP#],[SEQ#],[Roll#],[Dyelot#]");
            }
            else
            {
                sqlCmd.Append($@"
select inspector,[QCName] = (select name from pass1 where id=inspector),[inspected date]
,count([Roll#])Roll,ROUND(sum([Actual YDS]),1)[Actual YDS] 
from #tmp
group by inspector,[inspected date]
order by inspector,[inspected date]

-- 取得所有QC人員
select distinct Inspector
,[QCName] = (select name from pass1 where id=inspector) 
from #tmp order by Inspector

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
            SetCount(printData[0].Rows.Count);
            StringBuilder c = new StringBuilder();
            if (printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            string xltx = string.Empty;
            if (this.radioDetail.Checked)
            {
                xltx = "Quality_R08.xltx";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltx); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData[0], "", xltx, 1, true, null, objApp);// 將datatable copy to excel
            }
            else
            {
                xltx = "Quality_R08_Summery.xltx";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltx); //預先開啟excel app
                Excel.Worksheet wksheet = objApp.Sheets[1];
#if DEBUG
                objApp.Visible = true;
#endif

                int RowAdd = 0;
                int ColumnAdd = 2;
                string rgStr = string.Empty;

                //// 插入日期 
                int daydiff = printData[2].Rows.Count;
                for (int i = 0; i < printData[2].Rows.Count; i++)
                {
                    // 寫入日期
                    wksheet.Cells[5 + i, 1] = ((DateTime)printData[2].Rows[i]["inspected date"]).ToString("MM/dd");
                }
                
                rgStr = string.Format($"A5:A{daydiff + 8}");
                Excel.Range rng = wksheet.get_Range(rgStr);
                rng.Font.Bold = true;

                // 插入QC資料
                wksheet.Cells[2, 1] = "QC";
                wksheet.Cells[2, 1].Interior.Color = green;
                wksheet.Cells[2, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                wksheet.Cells[3, 1] = "Date";
                wksheet.Cells[3, 1].Interior.Color = green;
                wksheet.Cells[3, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                if (printData[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in printData[1].Rows)
                    {
                        // QC ID
                        wksheet.Cells[2, ColumnAdd] = dr["Inspector"].ToString();
                        rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(ColumnAdd)}2:{MyExcelPrg.GetExcelColumnName(ColumnAdd + 1)}2");
                        Excel.Range merge = wksheet.get_Range(rgStr);
                        merge.Interior.Color = green;
                        merge.Merge();
                        merge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // QC Name
                        wksheet.Cells[3, ColumnAdd] = dr["QCName"].ToString();
                        rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(ColumnAdd)}3:{MyExcelPrg.GetExcelColumnName(ColumnAdd + 1)}3");
                        merge = wksheet.get_Range(rgStr);
                        merge.Interior.Color = blue;
                        merge.Merge();
                        merge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // Header
                        wksheet.Cells[4, ColumnAdd] = "Roll";
                        wksheet.Cells[4, ColumnAdd + 1] = "Yard";

                        // Loop天數的次數
                        RowAdd = 5;
                        int cnt = 0;
                        for (int i = 0; i < printData[2].Rows.Count; i++)
                        {
                            DataRow[] selected = printData[0].Select($@"inspector='{dr["Inspector"]}' and [inspected date]='{((DateTime)printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                            if (selected.Length > 0)
                            {
                                wksheet.Cells[RowAdd, ColumnAdd] = (selected[0]).ItemArray[3];
                                wksheet.Cells[RowAdd, ColumnAdd + 1] = (selected[0]).ItemArray[4];
                                cnt++;
                            }
                            else
                            {
                                wksheet.Cells[RowAdd, ColumnAdd] = 0;
                                wksheet.Cells[RowAdd, ColumnAdd].Interior.Color = zero;
                                wksheet.Cells[RowAdd, ColumnAdd + 1] = 0;
                                wksheet.Cells[RowAdd, ColumnAdd + 1].Interior.Color = zero;
                            }
                            RowAdd++;
                        }

                        // Totoal
                        wksheet.Cells[RowAdd, ColumnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})";
                        wksheet.Cells[RowAdd, ColumnAdd + 1] = $"=SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd + 1)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd + 1)}{RowAdd - 1})";

                        // Avg
                        wksheet.Cells[RowAdd + 1, ColumnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})/{cnt},1)";
                        wksheet.Cells[RowAdd + 1, ColumnAdd + 1] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd + 1)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd + 1)}{RowAdd - 1})/{cnt},1)";

                        ColumnAdd += 2;
                    }

                    // 加入Total Header
                    wksheet.Cells[2, ColumnAdd] = "Daily output";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(ColumnAdd)}2:{MyExcelPrg.GetExcelColumnName(ColumnAdd + 3)}2");
                    Excel.Range mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Woven
                    wksheet.Cells[3,ColumnAdd] = "Woven";
                    wksheet.Cells[3,ColumnAdd].Interior.Color = green;
                    wksheet.Cells[4,ColumnAdd] = "Yard";

                    // Loop天數的次數
                    int cntWoven = 0;
                    for (int i = 0; i < daydiff; i++)
                    {
                        DataRow[] selected = printData[3].Select($@"[inspected date]='{((DateTime)printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                        if (selected.Length > 0)
                        {
                            wksheet.Cells[5 + i, ColumnAdd] = (selected[0]).ItemArray[1];
                            cntWoven++;
                        }
                        else
                        {
                            wksheet.Cells[5 + i, ColumnAdd] = 0;
                            wksheet.Cells[5 + i, ColumnAdd].Interior.Color = zero;
                        }
                    }

                    // Totoal
                    wksheet.Cells[daydiff + 5, ColumnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})";
                    // Avg
                    wksheet.Cells[daydiff + 6,ColumnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})/{cntWoven},1)";

                    // Knit
                    ColumnAdd++;
                    wksheet.Cells[3, ColumnAdd] = "Knit";
                    wksheet.Cells[3, ColumnAdd].Interior.Color = green;
                    wksheet.Cells[4, ColumnAdd] = "Yard";

                    for (int i = 0; i < printData[4].Rows.Count; i++)
                    {
                        wksheet.Cells[5 + i,ColumnAdd] = printData[4].Rows[i]["Actual YDS"];
                    }

                    // Loop天數的次數
                    int cntKnit = 0;
                    for (int i = 0; i < daydiff; i++)
                    {
                        DataRow[] selected = printData[4].Select($@"[inspected date]='{((DateTime)printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                        if (selected.Length > 0)
                        {
                            wksheet.Cells[5 + i, ColumnAdd] = (selected[0]).ItemArray[1];
                            cntKnit++;
                        }
                        else
                        {
                            wksheet.Cells[5 + i, ColumnAdd] = 0;
                            wksheet.Cells[5 + i, ColumnAdd].Interior.Color = zero;
                        }
                    }

                    // Totoal
                    wksheet.Cells[daydiff + 5,ColumnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})";
                    // Avg
                    wksheet.Cells[daydiff + 6, ColumnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})/{cntKnit},1)";

                    // Total
                    ColumnAdd++;
                    wksheet.Cells[3, ColumnAdd] = "Total";                    
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(ColumnAdd)}3:{MyExcelPrg.GetExcelColumnName(ColumnAdd + 1)}3");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;               


                    for (int i = 0; i < printData[2].Rows.Count; i++)
                    {
                        wksheet.Cells[5 + i, ColumnAdd] = printData[2].Rows[i]["Roll"];
                        wksheet.Cells[5 + i, ColumnAdd + 1] = printData[2].Rows[i]["Actual YDS"];
                    }

                    wksheet.Cells[4, ColumnAdd] = "Roll";
                    // Sum
                    wksheet.Cells[daydiff + 5, ColumnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})";
                    // Avg
                    wksheet.Cells[daydiff + 6, ColumnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})/{daydiff},1)";

                    ColumnAdd++;
                    wksheet.Cells[4, ColumnAdd] = "Yard";
                    // Sum
                    wksheet.Cells[daydiff + 5, ColumnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})";
                    wksheet.Cells[daydiff + 5, ColumnAdd].Font.Color = Color.Red;
                    // Avg
                    wksheet.Cells[daydiff + 6, ColumnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(ColumnAdd)}5:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd - 1})/{daydiff},1)";
                    wksheet.Cells[daydiff + 6, ColumnAdd].Font.Color = Color.FromArgb(0, 112, 192);

                    // Remark
                    ColumnAdd++;
                    wksheet.Cells[2, ColumnAdd] = "Remark";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(ColumnAdd)}2:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}4");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Daily Output Format
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(ColumnAdd - 4)}2:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd + 1}");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Font.Bold = true;

                    // Total,Ave Format
                    rgStr = string.Format($"A{RowAdd}:{MyExcelPrg.GetExcelColumnName(ColumnAdd - 1)}{RowAdd + 1}");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Font.Bold = true;
                    mergerge.Interior.Color= Color.FromArgb(192, 192, 192);
                    wksheet.Cells[daydiff + 5, 1] = "Total";
                    wksheet.Cells[daydiff + 6, 1] = "Average";

                    // 畫線
                    wksheet.get_Range($"A2:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}{RowAdd + 1}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    rgStr = $"A1:{MyExcelPrg.GetExcelColumnName(ColumnAdd)}1";
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                }
                objApp.Columns.AutoFit();
                this.Excelfile = Sci.Production.Class.MicrosoftFile.GetName("Quality_R08_Summery");
                objApp.ActiveWorkbook.SaveAs(this.Excelfile);
                objApp.Quit();
                Marshal.ReleaseComObject(wksheet);
                Marshal.ReleaseComObject(objApp);
                Excelfile.OpenFile();
            }
            
         
            return true;
        }
    }
}
