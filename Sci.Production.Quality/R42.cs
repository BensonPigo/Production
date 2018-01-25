﻿using Ict;
using Microsoft.Office.Interop.Excel;
using Sci;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R42 : Sci.Win.Tems.PrintForm
    {
        string Brand;
        string Year;
        string Report_Type1;
        string Report_Type2;
        DualResult result;
        System.Data.DataTable dt;
        System.Data.DataTable dt_All;
        System.Data.DataTable[] alldt;
        System.Data.DataTable month12 = null;
        System.Data.DataTable allsupplier = null;
        System.Data.DataTable dat;
        System.Data.DataTable dt_temp = new System.Data.DataTable(); //for Printing Detail Report only
        System.Data.DataTable dt_printing = new System.Data.DataTable(); //for Printing Detail Report only

        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            System.Data.DataTable Year = null;
            string cmd = (@"
declare @y Table (M int);

declare @StartYear int = 2013;
declare @EndYear int = datepart(year, DateAdd (Month, -1, getDate()))

while (@StartYear <= @EndYear)
begin 
	insert into @y
	(M)
	values
	(@StartYear)

	set @StartYear = @StartYear + 1
end

select *
from @y");
            DBProxy.Current.Select("", cmd, out Year);
            Year.DefaultView.Sort = "M";
            this.comboYear.DataSource = Year;
            this.comboYear.ValueMember = "M";
            this.comboYear.DisplayMember = "M";

            if (Year != null
                && Year.Rows.Count > 0)
            {
                this.comboYear.SelectedIndex = Year.Rows.Count - 1;
            }

            print.Enabled = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboYear.SelectedValue = DateTime.Today.AddMonths(-1).Year;
        }

        Dictionary<string, System.Data.DataTable> PrintingData = new Dictionary<string, System.Data.DataTable>();

        protected override bool ValidateInput()
        {
            Brand = comboBrand.Text.ToString();
            Year = comboYear.Text.ToString();
            Report_Type1 = radioPillAndSnaggDetail.Checked.ToString();
            Report_Type2 = radioPrintDetail.Checked.ToString();
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            dt = null;
            dt_All = null;


            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string rt = ""; string gb = ""; string ob = "";
            List<string> sqlWheres = new List<string>();

            if (Brand != "")
            {
                sqlWheres.Add(" b.BrandID ='" + Brand + "'");
            }
            if (Year != "")
            {
                sqlWheres.Add("year(a.StartDate)=" + Year);
            }

            if (radioPillAndSnaggDetail.Checked == true)
            {
                rt = "and 1=1 and b.DefectMainID='33' and b.DefectSubID='A'";
            }
            else
            {
                rt = "and 1=1 and b.DefectMainID='35' and b.DefectSubID='H'";

            }
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            gb = "group by a.StartDate,b.supplier,b.StyleID,b.refno";
            ob = "order by Month";
            string sqlcmd = string.Format(@"create table #dRanges(starts int , ends int, name varchar(20))
                                                 insert into #dRanges values
                                                  (1,1,'January'),
                                                  (2,2,'February'),
                                                  (3,3,'March'),
                                                  (4,4,'April'),
                                                  (5,5,'May'),
                                                  (6,6,'June'),
                                                  (7,7,'July'),
                                                  (8,8,'August'),
                                                  (9,9,'September'),
                                                  (10,10,'October'),
                                                  (11,11,'November'),
                                                  (12,12,'December')

                                    SELECT iif(B.Supplier<>'',B.Supplier,'Other') as supplier  
                                    ,format(a.startdate,'MMMMM')[m]
                                    ,b.StyleID
                                    ,b.refno
                                    ,sum(b.qty) qty
                                    ,sum(b.ValueinUSD) ValueinUSD
                                    into #temp
                                    FROM DBO.ADIDASComplain A WITH (NOLOCK) 
                                    INNER JOIN DBO.ADIDASComplain_Detail B WITH (NOLOCK) ON B.ID = A.ID" + " " + sqlWhere + " " + rt + " " + gb + " " +

                               @"select name as mon from (select DISTINCT dRanges.name,dRanges.starts from #temp T 
				                      left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends
				                      GROUP BY m ,dRanges.starts,dRanges.name)as s  ORDER BY starts

                                    select distinct supplier from #temp

                                    select * from #temp


                                    select  distinct * from (
	                         select Shell_Supplier=supplier from  #temp cc
	                         where m in (select m from (select *,idx=ROW_NUMBER()over(order by cnt desc) from (select m,cnt=count(*) from #temp where supplier = cc.supplier group by m) c) d where idx = 1)
	                         group by supplier,refno) main");
            SqlConnection conn;
            result = DBProxy.Current.OpenConnection("", out conn);
            if (!result) { return result; }
            result = DBProxy.Current.SelectByConn(conn, sqlcmd, out alldt);
            if (!result) { return result; }
            month12 = alldt[0];
            string month_Columns = "";
            string Qty_SumColumns = "";
            string Complaint_SumColumns = "";
            var total_Rows = new List<System.Data.DataRow>();
            if (alldt[0].Rows.Count <= 0)
            {
                return new DualResult(false, "Data not found! ");
            }
            #region Pilling & Snagging Summery
            if (Report_Type1 == "True")
            {
                string sp = @"select  * 
from 
(
	select [Shell_Supplier]=supplier,
	idx=ROW_NUMBER()OVER(partition by supplier order by supplier) 
	from  #temp cc
	where m in (
				select m 
				from (
						select *,
						idx=ROW_NUMBER()over(order by cnt desc) 
						from (
								select m,
								cnt=count(*) 
								from #temp 
								where supplier = cc.supplier 
								group by m
							) c
					) d where idx = 1
				)
	--group by supplier,refno --加上這一段會讓結果只剩下一筆,所以註解掉 WILLY_20170325
) main";

                for (int i = 0; i < month12.Rows.Count; i++)
                {
                    string month = month12.Rows[i]["mon"].ToString();
                    string o = string.Format("left join (select {0}_idx=ROW_NUMBER()OVER(partition by supplier order by supplier),{0}_supplier=supplier,{0}_Style=styleid,{0}_Shell=Refno,{0}_Qty=qty,{0}_Complaint_Value=ValueinUSD from #temp where m = '{0}') {0} on main.Shell_Supplier = {0}.{0}_supplier and main.idx = {0}.{0}_idx", month);
                    sp = sp + Environment.NewLine + o;
                    month_Columns += month + "_Qty," + month + "_Complaint_Value,";
                    Qty_SumColumns += month + "_Qty+";
                    Complaint_SumColumns += month + "_Complaint_Value+";
                }

                Qty_SumColumns = Qty_SumColumns.Substring(0, Qty_SumColumns.Length - 1);
                Complaint_SumColumns = Complaint_SumColumns.Substring(0, Complaint_SumColumns.Length - 1);
                result = DBProxy.Current.SelectByConn(conn, sp, out dt);
                if (!result) { return result; }


                dt.Columns.Remove("idx");
                dt.Columns.Remove("January_idx");
                dt.Columns.Remove("February_idx");
                dt.Columns.Remove("March_idx");
                dt.Columns.Remove("April_idx");
                dt.Columns.Remove("May_idx");
                dt.Columns.Remove("June_idx");
                dt.Columns.Remove("July_idx");
                dt.Columns.Remove("August_idx");
                dt.Columns.Remove("September_idx");
                dt.Columns.Remove("October_idx");
                dt.Columns.Remove("November_idx");
                dt.Columns.Remove("December_idx");
                dt.Columns.Remove("January_supplier");
                dt.Columns.Remove("February_supplier");
                dt.Columns.Remove("March_supplier");
                dt.Columns.Remove("April_supplier");
                dt.Columns.Remove("May_supplier");
                dt.Columns.Remove("June_supplier");
                dt.Columns.Remove("July_supplier");
                dt.Columns.Remove("August_supplier");
                dt.Columns.Remove("September_supplier");
                dt.Columns.Remove("October_supplier");
                dt.Columns.Remove("November_supplier");
                dt.Columns.Remove("December_supplier");

                dt.ColumnsDecimalAdd("Qty", expression: Qty_SumColumns);
                dt.ColumnsDecimalAdd("Complaint_Value", expression: Complaint_SumColumns);
                total_Rows = dt.Sum(month_Columns, "Shell_Supplier");
                if (!MyUtility.Check.Empty(total_Rows))
                {
                    foreach (var ttl_row in total_Rows)
                    {
                        ttl_row["Shell_Supplier"] = ttl_row["Shell_Supplier"].ToString().Trim() + "Total";
                    }
                    total_Rows[total_Rows.Count - 1]["Shell_Supplier"] = "GRAND TOTAL";
                }
                if (null == dt_All || 0 == dt_All.Rows.Count)
                {

                    dt_All = dt;
                }
                else
                {
                    dt_All.Merge(dt);
                }


            }
            #endregion
            #region Printing Summery
            else
            {

                month12 = alldt[0];
                dt_printing = new System.Data.DataTable();
                dt_printing.Columns.Add("Shell_Supplier", typeof(string));
                bool First = true;
                //Type=  Pilling & Snagging Detail                  
                for (int i = 0; i < month12.Rows.Count; i++)
                {
                    DataRow dr;
                    string month = month12.Rows[i]["mon"].ToString();
                    dt_printing.Columns.Add(string.Format("{0}_Style", month), typeof(string));
                    dt_printing.Columns.Add(string.Format("{0}_Shell", month), typeof(string));
                    dt_printing.Columns.Add(string.Format("{0}_Qty", month), typeof(int));
                    dt_printing.Columns.Add(string.Format("{0}_Complaint_Value", month), typeof(decimal));

                    string scmd = string.Format(@"
                    select 		           
		            {0}_Style=styleid,{0}_Shell=Refno,{0}_Qty=qty,{0}_Complaint_Value=ValueinUSD 
                    from #temp where m = '{0}'", month);

                    month_Columns += month + "_Qty," + month + "_Complaint_Value,";
                    Qty_SumColumns += month + "_Qty+";
                    Complaint_SumColumns += month + "_Complaint_Value+";
                    result = DBProxy.Current.SelectByConn(conn, scmd, out dt_temp);
                    int counts = dt_temp.Rows.Count;
                    //DataTable第一次塞值避免null, 所以要先NewRow
                    if (First)
                    {
                        First = false;
                        for (int t = 0; t < dt_temp.Rows.Count; t++)
                        {
                            dr = dt_printing.NewRow();
                            dr[month + "_Style"] = dt_temp.Rows[t][month + "_Style"];
                            dr[month + "_Shell"] = dt_temp.Rows[t][month + "_Shell"];
                            dr[month + "_Qty"] = dt_temp.Rows[t][month + "_Qty"];
                            dr[month + "_Complaint_Value"] = dt_temp.Rows[t][month + "_Complaint_Value"];
                            dt_printing.Rows.Add(dr);
                        }
                        dt_printing.Rows[0]["Shell_Supplier"] = "Other -";
                    }
                    else
                    {
                        for (int t = 0; t < dt_temp.Rows.Count; t++)
                        {
                            //如果DatatTble欄位數不夠,就要NewRow
                            if (t >= dt_printing.Rows.Count)
                            {
                                dr = dt_printing.NewRow();
                                dr[month + "_Style"] = dt_temp.Rows[t][month + "_Style"];
                                dr[month + "_Shell"] = dt_temp.Rows[t][month + "_Shell"];
                                dr[month + "_Qty"] = dt_temp.Rows[t][month + "_Qty"];
                                dr[month + "_Complaint_Value"] = dt_temp.Rows[t][month + "_Complaint_Value"];
                                dt_printing.Rows.Add(dr);
                            }
                            else
                            {
                                dt_printing.Rows[t][month + "_Style"] = dt_temp.Rows[t][month + "_Style"];
                                dt_printing.Rows[t][month + "_Shell"] = dt_temp.Rows[t][month + "_Shell"];
                                dt_printing.Rows[t][month + "_Qty"] = dt_temp.Rows[t][month + "_Qty"];
                                dt_printing.Rows[t][month + "_Complaint_Value"] = dt_temp.Rows[t][month + "_Complaint_Value"];
                            }
                        }
                    }

                    if (!result)
                    {
                        return result;
                    }
                }
                Qty_SumColumns = Qty_SumColumns.Substring(0, Qty_SumColumns.Length - 1);
                Complaint_SumColumns = Complaint_SumColumns.Substring(0, Complaint_SumColumns.Length - 1);

                dt_printing.ColumnsDecimalAdd("Qty");
                dt_printing.ColumnsDecimalAdd("Complaint_Value");
                decimal TTLQty = 0;
                decimal TTLValue = 0;
                total_Rows = dt_printing.Sum(month_Columns, "");
                if (!MyUtility.Check.Empty(total_Rows))
                {
                    foreach (var ttl_row in total_Rows)
                    {
                        ttl_row["Shell_Supplier"] = ttl_row["Shell_Supplier"].ToString().Trim() + "Total";
                    }
                    total_Rows[total_Rows.Count - 1]["Shell_Supplier"] = "GRAND TOTAL";
                }

                //加總不同月份的數量及Complaint_Value,再丟進Total
                for (int i = 1; i < dt_printing.Columns.Count; i++)
                {
                    if ((i + 1) % 4 == 0)
                    {
                        TTLQty += MyUtility.Convert.GetDecimal(dt_printing.Rows[dt_printing.Rows.Count - 1][i]);

                    }
                    if ((i) % 4 == 0)
                    {
                        TTLValue += MyUtility.Convert.GetDecimal(dt_printing.Rows[dt_printing.Rows.Count - 1][i]);
                    }

                }
                dt_printing.Rows[dt_printing.Rows.Count - 1][dt_printing.Columns.Count - 1] = TTLValue;
                dt_printing.Rows[dt_printing.Rows.Count - 1][dt_printing.Columns.Count - 2] = TTLQty;
                if (null == dt_All || 0 == dt_All.Rows.Count)
                {
                    dt_All = dt_printing;
                }
                else
                {
                    dt_All.Merge(dt_printing);
                }
            }
            #endregion

            if (!result)
            {
                this.ShowErr(result);
            }

            allsupplier = alldt[3];
            dicSUP.Clear();
            #region Pilling & Snagging Detail
            if (Report_Type1 == "True")
            {
                for (int i = 0; i < allsupplier.Rows.Count; i++)
                {
                    string sss = allsupplier.Rows[i]["Shell_Supplier"].ToString();

                    string scmd = string.Format(@"SELECT 
                    a.startdate as Month
                    ,A.AGCCode [FactoryID]
                    ,A.FactoryName [FactoryName]
                    ,B.SalesID [Sales_Org._ID]
                    ,B.SalesName [Sales_Org._Name]
                    ,B.Article [Article_ID]
                    ,B.ArticleName [Article_Name]
                    ,B.StyleID [Style]
                    ,B.Refno [Shell]
                    ,B.OrderID [SP#]
                    ,B.custPONO [PONo]
                    ,B.FactoryID [Factory]
                    ,B.ProductionDate [Prod_Date]
                    ,B.DefectMainID [Defect MainID]
                    ,c.Name [Defect_Main_Name]
                    ,B.DefectSubID [Defect_Sub_ID]
                    ,d.SubName [Defect_Sub_Name]
                    ,B.FOB [FOB_Price]
                    ,B.Qty [Qty]
                    ,B.ValueinUSD [Complaint_Value]
                    ,B.ValueINExRate[Exrate]
                    FROM 
                    DBO.ADIDASComplain A WITH (NOLOCK) 
                    INNER JOIN DBO.ADIDASComplain_Detail B WITH (NOLOCK) ON B.ID = A.ID
                    left join dbo.ADIDASComplainDefect c WITH (NOLOCK) on c.ID=b.DefectMainID
                    left join dbo.ADIDASComplainDefect_Detail d WITH (NOLOCK) on d.id=b.DefectMainID and d.SubID=b.DefectSubID" + " " + sqlWhere + "and supplier='{0}'" + rt + " " + ob, sss == "Other" ? "" : sss);

                    result = DBProxy.Current.SelectByConn(conn, scmd, out dat);
                    dicSUP.Add(sss, dat);
                    if (!result)
                    {
                        return result;
                    }
                }
            }
            #endregion
            #region Printing Detail
            else
            {

                string scmd = string.Format(@"SELECT 
                    a.startdate as Month
                    ,A.AGCCode [FactoryID]
                    ,A.FactoryName [FactoryName]
                    ,B.SalesID [Sales_Org._ID]
                    ,B.SalesName [Sales_Org._Name]
                    ,B.Article [Article_ID]
                    ,B.ArticleName [Article_Name]
                    ,B.StyleID [Style]
                    ,B.Refno [Shell]
                    ,B.OrderID [SP#]
                    ,B.custPONO [PONo]
                    ,B.FactoryID [Factory]
                    ,B.ProductionDate [Prod_Date]
                    ,B.DefectMainID [Defect MainID]
                    ,c.Name [Defect_Main_Name]
                    ,B.DefectSubID [Defect_Sub_ID]
                    ,d.SubName [Defect_Sub_Name]
                    ,B.FOB [FOB_Price]
                    ,B.Qty [Qty]
                    ,B.ValueinUSD [Complaint_Value]
                    ,B.ValueINExRate[Exrate]
                    FROM 
                    DBO.ADIDASComplain A WITH (NOLOCK) 
                    INNER JOIN DBO.ADIDASComplain_Detail B WITH (NOLOCK) ON B.ID = A.ID
                    left join dbo.ADIDASComplainDefect c WITH (NOLOCK) on c.ID=b.DefectMainID
                    left join dbo.ADIDASComplainDefect_Detail d WITH (NOLOCK) on d.id=b.DefectMainID and d.SubID=b.DefectSubID" + " " + sqlWhere + " " + rt + "  " + ob);

                result = DBProxy.Current.SelectByConn(conn, scmd, out dat);
                dicSUP.Add("other", dat);
                if (!result)
                {
                    return result;
                }
            }
            #endregion
            return result;  //base.OnAsyncDataLoad(e);
        }

        Dictionary<string, System.Data.DataTable> dicSUP = new Dictionary<string, System.Data.DataTable>();

        protected override bool OnToExcel(Win.ReportDefinition report)
        {

            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.Filter_Excel);
            Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R42.xltx", keepApp: true);
            SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(dt_All);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(" ", "1,1");
            dic.Add("January", "2,5");
            dic.Add("Feburary", "6,9");
            dic.Add("March", "10,13");
            dic.Add("April", "14,17");
            dic.Add("May", "18,21");
            dic.Add("June", "22,25");
            dic.Add("July", "26,29");
            dic.Add("August", "30,33");
            dic.Add("September", "34,37");
            dic.Add("October", "38,41");
            dic.Add("November", "42,45");
            dic.Add("December", "46,49");
            dic.Add("YTD", "50,51");
            xdt_All.LisTitleMerge.Add(dic);
            xdt_All.ShowHeader = true;


            foreach (var item in dicSUP)
            {
                string supplier = item.Key;
                SaveXltReportCls.XltRptTable x_All = new SaveXltReportCls.XltRptTable(item.Value);

                for (int i = 0; i < x_All.Columns.Count; i++)
                {
                    SaveXltReportCls.XlsColumnInfo xlc = new SaveXltReportCls.XlsColumnInfo(i + 1);
                    if (x_All.Columns[i].ColumnName == "Month")
                    {
                        xlc.NumberFormate = "MMM-yy";
                        xlc.IsNumber = false;

                    }
                    if (x_All.Columns[i].ColumnName == "Prod_Date")
                    {
                        xlc.NumberFormate = "MMM-yy";
                        xlc.IsNumber = false;

                    }
                    xlc.IsAutoFit = true;                   
                    x_All.LisColumnInfo.Add(xlc);
                }
                x_All.BoAutoFitColumn = true;
                xl.DicDatas.Add("##SUPSheetName" + supplier, item.Key);
                xl.DicDatas.Add("##SUPDetail" + supplier, x_All);
            }

            xl.VarToSheetName = "##SUPSheetName";
            //xdt_All.boAutoFitColumn = true;
            xl.DicDatas.Add("##psd", xdt_All);

            SaveXltReportCls.ReplaceAction b = Addcolor;
            xl.DicDatas.Add("##addcolor", b);

            SaveXltReportCls.ReplaceAction c = CopySheet;
            
            xl.DicDatas.Add("##copysupsheet", c);

           
            SaveXltReportCls.ReplaceAction d = addfilter;
            xl.DicDatas.Add("##addfilter", d);

            xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Quality_R42"));    
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
        }

        void Addcolor(Worksheet mySheet, int rowNo, int columnNo)
        {

            mySheet.get_Range("A2", "AY2").Interior.Color = Color.SkyBlue;
            mySheet.get_Range("A2", "AY2").Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            Microsoft.Office.Interop.Excel.Range usedRange = mySheet.UsedRange;
            Microsoft.Office.Interop.Excel.Range rows = usedRange.Rows;
            int count = 0;

            foreach (Microsoft.Office.Interop.Excel.Range row in rows)
            {

                if (count > 0)
                {
                    Microsoft.Office.Interop.Excel.Range firstCell = row.Cells[1];

                    string firstCellValue = firstCell.Value as String;
                    if (firstCellValue == null) continue;
                    if (firstCellValue.StrEndsWith("GRAND TOTAL"))
                    {
                        row.Interior.Color = System.Drawing.Color.Aquamarine;
                        row.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    }
                    else if (firstCellValue.StrEndsWith("Total"))
                    {
                        row.Interior.Color = System.Drawing.Color.Gold;
                        row.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    }
                }
                count++;
            }

            Range formatRange;
            Range ReplaceRange;
            Range ReplaceRange1;



            if (Report_Type2 == "True")
            {
                //單獨將Total金額轉成貨幣
                formatRange = mySheet.get_Range(string.Format("AY{0}", dt_printing.Rows.Count + 2), string.Format("AY{0}", dt_printing.Rows.Count + 2));
                formatRange.NumberFormat = "$#,##0.00";

                //只顯示total的值,其他數值為0都轉空白
                ReplaceRange = mySheet.get_Range(string.Format("AY{0}", 3), string.Format("AY{0}", dt_printing.Rows.Count + 1));
                ReplaceRange.Replace("0", "");
                ReplaceRange1 = mySheet.get_Range(string.Format("AX{0}", 3), string.Format("AX{0}", dt_printing.Rows.Count + 1));
                ReplaceRange1.Replace("0", "");
            }
            else
            {
                //單獨將Total金額轉成貨幣
                formatRange = mySheet.get_Range(string.Format("AY{0}", 2), string.Format("AY{0}", dt_All.Rows.Count + 2));
                formatRange.NumberFormat = "$#,##0.00";
            }


        }

        void CopySheet(Worksheet mySheet, int rowNo, int columnNo)
        {
            Microsoft.Office.Interop.Excel._Application myExcel = null;
            Microsoft.Office.Interop.Excel._Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            Worksheet aftersheet = mySheet;

            int idx = 0;
            for (int i = 0; i < month12.Rows.Count; i++)
            {
                mySheet.Cells[2, 1] = "Shell Supplier";
                idx += 1;
                mySheet.Cells[2, idx * 4 - 2] = "Style";
                mySheet.Cells[2, idx * 4 - 1] = "Shell";
                mySheet.Cells[2, idx * 4] = "Qty";
                mySheet.Cells[2, idx * 4 + 1] = "Complaint Value";
            }


            foreach (var item in dicSUP)
            {
                aftersheet = myExcel.Sheets.Add(After: aftersheet);
                aftersheet.Cells[1, 1] = "##SUPDetail" + item.Key;
                aftersheet.Cells[3, 1] = "##SUPSheetName" + item.Key;
                aftersheet.Cells[3, 1].Font.Color = Color.Transparent;

                aftersheet.Cells[4, 1] = "##addfilter";
                aftersheet.Name = item.Key;//Sheet Name
            }
        }

        void addfilter(Worksheet mySheet, int rowNo, int columnNo)
        {
            Range firstRow = (Range)mySheet.Rows[1];
            firstRow.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);
            firstRow.Interior.Color = Color.SkyBlue;

        }
    }
}
