using Ict;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R42 : Win.Tems.PrintForm
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
        System.Data.DataTable dt_temp = new System.Data.DataTable(); // for Printing Detail Report only
        System.Data.DataTable dt_printing = new System.Data.DataTable(); // for Printing Detail Report only

        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            System.Data.DataTable Year = null;
            string cmd = @"
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
from @y
order by M desc";
            DBProxy.Current.Select(string.Empty, cmd, out Year);
            this.comboYear.DataSource = Year;
            this.comboYear.ValueMember = "M";
            this.comboYear.DisplayMember = "M";

            if (Year != null
                && Year.Rows.Count > 0)
            {
                this.comboYear.SelectedIndex = 0;
            }

            this.print.Enabled = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboYear.SelectedValue = DateTime.Today.AddMonths(-1).Year;
        }

        Dictionary<string, System.Data.DataTable> PrintingData = new Dictionary<string, System.Data.DataTable>();

        protected override bool ValidateInput()
        {
            this.Brand = this.comboBrand.Text.ToString();
            this.Year = this.comboYear.Text.ToString();
            this.Report_Type1 = this.radioPillAndSnaggDetail.Checked.ToString();
            this.Report_Type2 = this.radioPrintDetail.Checked.ToString();
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.dt = null;
            this.dt_All = null;

            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            string rt = string.Empty;
            string gb = string.Empty;
            string ob = string.Empty;
            List<string> sqlWheres = new List<string>();

            if (this.Brand != string.Empty)
            {
                sqlWheres.Add(" b.BrandID ='" + this.Brand + "'");
            }

            if (this.Year != string.Empty)
            {
                sqlWheres.Add("year(a.StartDate)=" + this.Year);
            }

            if (this.radioPillAndSnaggDetail.Checked == true)
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
                sqlWhere = " where A.Junk=0 AND " + sqlWhere;
            }

            gb = "group by a.StartDate,b.SuppID,b.StyleID,b.refno";
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

                                    SELECT iif(B.SuppID<>'',B.SuppID,'Other') as supplier  
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
            this.result = DBProxy.Current.OpenConnection(string.Empty, out conn);
            if (!this.result)
            {
                return this.result;
            }

            this.result = DBProxy.Current.SelectByConn(conn, sqlcmd, out this.alldt);
            if (!this.result)
            {
                return this.result;
            }

            this.month12 = this.alldt[0];
            string month_Columns = string.Empty;
            string Qty_SumColumns = string.Empty;
            string Complaint_SumColumns = string.Empty;
            var total_Rows = new List<DataRow>();
            if (this.alldt[0].Rows.Count <= 0)
            {
                return new DualResult(false, "Data not found! ");
            }
            #region Pilling & Snagging Summery
            if (this.Report_Type1 == "True")
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

                for (int i = 0; i < this.month12.Rows.Count; i++)
                {
                    string month = this.month12.Rows[i]["mon"].ToString();
                    string o = string.Format("left join (select {0}_idx=ROW_NUMBER()OVER(partition by supplier order by supplier),{0}_supplier=supplier,{0}_Style=styleid,{0}_Shell=Refno,{0}_Qty=qty,{0}_Complaint_Value=ValueinUSD from #temp where m = '{0}') {0} on main.Shell_Supplier = {0}.{0}_supplier and main.idx = {0}.{0}_idx", month);
                    sp = sp + Environment.NewLine + o;
                    month_Columns += month + "_Qty," + month + "_Complaint_Value,";
                    Qty_SumColumns += month + "_Qty+";
                    Complaint_SumColumns += month + "_Complaint_Value+";
                }

                Qty_SumColumns = Qty_SumColumns.Substring(0, Qty_SumColumns.Length - 1);
                Complaint_SumColumns = Complaint_SumColumns.Substring(0, Complaint_SumColumns.Length - 1);
                this.result = DBProxy.Current.SelectByConn(conn, sp, out this.dt);
                if (!this.result)
                {
                    return this.result;
                }

                this.dt.Columns.Remove("idx");
                this.dt.Columns.Remove("January_idx");
                this.dt.Columns.Remove("February_idx");
                this.dt.Columns.Remove("March_idx");
                this.dt.Columns.Remove("April_idx");
                this.dt.Columns.Remove("May_idx");
                this.dt.Columns.Remove("June_idx");
                this.dt.Columns.Remove("July_idx");
                this.dt.Columns.Remove("August_idx");
                this.dt.Columns.Remove("September_idx");
                this.dt.Columns.Remove("October_idx");
                this.dt.Columns.Remove("November_idx");
                this.dt.Columns.Remove("December_idx");
                this.dt.Columns.Remove("January_supplier");
                this.dt.Columns.Remove("February_supplier");
                this.dt.Columns.Remove("March_supplier");
                this.dt.Columns.Remove("April_supplier");
                this.dt.Columns.Remove("May_supplier");
                this.dt.Columns.Remove("June_supplier");
                this.dt.Columns.Remove("July_supplier");
                this.dt.Columns.Remove("August_supplier");
                this.dt.Columns.Remove("September_supplier");
                this.dt.Columns.Remove("October_supplier");
                this.dt.Columns.Remove("November_supplier");
                this.dt.Columns.Remove("December_supplier");

                this.dt.ColumnsDecimalAdd("Qty", expression: Qty_SumColumns);
                this.dt.ColumnsDecimalAdd("Complaint_Value", expression: Complaint_SumColumns);
                total_Rows = this.dt.Sum(month_Columns, "Shell_Supplier");
                if (!MyUtility.Check.Empty(total_Rows))
                {
                    foreach (var ttl_row in total_Rows)
                    {
                        ttl_row["Shell_Supplier"] = ttl_row["Shell_Supplier"].ToString().Trim() + "Total";
                    }

                    total_Rows[total_Rows.Count - 1]["Shell_Supplier"] = "GRAND TOTAL";
                }

                if (this.dt_All == null || this.dt_All.Rows.Count == 0)
                {
                    this.dt_All = this.dt;
                }
                else
                {
                    this.dt_All.Merge(this.dt);
                }
            }
            #endregion
            #region Printing Summery
            else
            {
                this.month12 = this.alldt[0];
                this.dt_printing = new System.Data.DataTable();
                this.dt_printing.Columns.Add("Shell_Supplier", typeof(string));
                bool First = true;

                // Type=  Pilling & Snagging Detail
                for (int i = 0; i < this.month12.Rows.Count; i++)
                {
                    DataRow dr;
                    string month = this.month12.Rows[i]["mon"].ToString();
                    this.dt_printing.Columns.Add(string.Format("{0}_Style", month), typeof(string));
                    this.dt_printing.Columns.Add(string.Format("{0}_Shell", month), typeof(string));
                    this.dt_printing.Columns.Add(string.Format("{0}_Qty", month), typeof(int));
                    this.dt_printing.Columns.Add(string.Format("{0}_Complaint_Value", month), typeof(decimal));

                    string scmd = string.Format(
                        @"
                    select 		           
		            {0}_Style=styleid,{0}_Shell=Refno,{0}_Qty=qty,{0}_Complaint_Value=ValueinUSD 
                    from #temp where m = '{0}'", month);

                    month_Columns += month + "_Qty," + month + "_Complaint_Value,";
                    Qty_SumColumns += month + "_Qty+";
                    Complaint_SumColumns += month + "_Complaint_Value+";
                    this.result = DBProxy.Current.SelectByConn(conn, scmd, out this.dt_temp);
                    int counts = this.dt_temp.Rows.Count;

                    // DataTable第一次塞值避免null, 所以要先NewRow
                    if (First)
                    {
                        First = false;
                        for (int t = 0; t < this.dt_temp.Rows.Count; t++)
                        {
                            dr = this.dt_printing.NewRow();
                            dr[month + "_Style"] = this.dt_temp.Rows[t][month + "_Style"];
                            dr[month + "_Shell"] = this.dt_temp.Rows[t][month + "_Shell"];
                            dr[month + "_Qty"] = this.dt_temp.Rows[t][month + "_Qty"];
                            dr[month + "_Complaint_Value"] = this.dt_temp.Rows[t][month + "_Complaint_Value"];
                            this.dt_printing.Rows.Add(dr);
                        }

                        this.dt_printing.Rows[0]["Shell_Supplier"] = "Other -";
                    }
                    else
                    {
                        for (int t = 0; t < this.dt_temp.Rows.Count; t++)
                        {
                            // 如果DatatTble欄位數不夠,就要NewRow
                            if (t >= this.dt_printing.Rows.Count)
                            {
                                dr = this.dt_printing.NewRow();
                                dr[month + "_Style"] = this.dt_temp.Rows[t][month + "_Style"];
                                dr[month + "_Shell"] = this.dt_temp.Rows[t][month + "_Shell"];
                                dr[month + "_Qty"] = this.dt_temp.Rows[t][month + "_Qty"];
                                dr[month + "_Complaint_Value"] = this.dt_temp.Rows[t][month + "_Complaint_Value"];
                                this.dt_printing.Rows.Add(dr);
                            }
                            else
                            {
                                this.dt_printing.Rows[t][month + "_Style"] = this.dt_temp.Rows[t][month + "_Style"];
                                this.dt_printing.Rows[t][month + "_Shell"] = this.dt_temp.Rows[t][month + "_Shell"];
                                this.dt_printing.Rows[t][month + "_Qty"] = this.dt_temp.Rows[t][month + "_Qty"];
                                this.dt_printing.Rows[t][month + "_Complaint_Value"] = this.dt_temp.Rows[t][month + "_Complaint_Value"];
                            }
                        }
                    }

                    if (!this.result)
                    {
                        return this.result;
                    }
                }

                Qty_SumColumns = Qty_SumColumns.Substring(0, Qty_SumColumns.Length - 1);
                Complaint_SumColumns = Complaint_SumColumns.Substring(0, Complaint_SumColumns.Length - 1);

                this.dt_printing.ColumnsDecimalAdd("Qty");
                this.dt_printing.ColumnsDecimalAdd("Complaint_Value");
                decimal TTLQty = 0;
                decimal TTLValue = 0;
                total_Rows = this.dt_printing.Sum(month_Columns, string.Empty);
                if (!MyUtility.Check.Empty(total_Rows))
                {
                    foreach (var ttl_row in total_Rows)
                    {
                        ttl_row["Shell_Supplier"] = ttl_row["Shell_Supplier"].ToString().Trim() + "Total";
                    }

                    total_Rows[total_Rows.Count - 1]["Shell_Supplier"] = "GRAND TOTAL";
                }

                // 加總不同月份的數量及Complaint_Value,再丟進Total
                for (int i = 1; i < this.dt_printing.Columns.Count; i++)
                {
                    if ((i + 1) % 4 == 0)
                    {
                        TTLQty += MyUtility.Convert.GetDecimal(this.dt_printing.Rows[this.dt_printing.Rows.Count - 1][i]);
                    }

                    if (i % 4 == 0)
                    {
                        TTLValue += MyUtility.Convert.GetDecimal(this.dt_printing.Rows[this.dt_printing.Rows.Count - 1][i]);
                    }
                }

                this.dt_printing.Rows[this.dt_printing.Rows.Count - 1][this.dt_printing.Columns.Count - 1] = TTLValue;
                this.dt_printing.Rows[this.dt_printing.Rows.Count - 1][this.dt_printing.Columns.Count - 2] = TTLQty;
                if (this.dt_All == null || this.dt_All.Rows.Count == 0)
                {
                    this.dt_All = this.dt_printing;
                }
                else
                {
                    this.dt_All.Merge(this.dt_printing);
                }
            }
            #endregion

            if (!this.result)
            {
                this.ShowErr(this.result);
            }

            this.allsupplier = this.alldt[3];
            this.dicSUP.Clear();
            #region Pilling & Snagging Detail
            if (this.Report_Type1 == "True")
            {
                for (int i = 0; i < this.allsupplier.Rows.Count; i++)
                {
                    string sss = this.allsupplier.Rows[i]["Shell_Supplier"].ToString();

                    string scmd = string.Format(
                        @"SELECT 
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
                    left join dbo.ADIDASComplainDefect_Detail d WITH (NOLOCK) on d.id=b.DefectMainID and d.SubID=b.DefectSubID" + " " + sqlWhere + "and b.SuppID='{0}'" + rt + " " + ob, sss == "Other" ? string.Empty : sss);

                    this.result = DBProxy.Current.SelectByConn(conn, scmd, out this.dat);
                    this.dicSUP.Add(sss, this.dat);
                    if (!this.result)
                    {
                        return this.result;
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

                this.result = DBProxy.Current.SelectByConn(conn, scmd, out this.dat);
                this.dicSUP.Add("other", this.dat);
                if (!this.result)
                {
                    return this.result;
                }
            }
            #endregion
            return this.result;  // base.OnAsyncDataLoad(e);
        }

        Dictionary<string, System.Data.DataTable> dicSUP = new Dictionary<string, System.Data.DataTable>();

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            var saveDialog = MyExcelPrg.GetSaveFileDialog(MyExcelPrg.Filter_Excel);
            SaveXltReportCls xl = new SaveXltReportCls("Quality_R42.xltx", keepApp: true);
            SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(this.dt_All);

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

            foreach (var item in this.dicSUP)
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

            // xdt_All.boAutoFitColumn = true;
            xl.DicDatas.Add("##psd", xdt_All);

            SaveXltReportCls.ReplaceAction b = this.Addcolor;
            xl.DicDatas.Add("##addcolor", b);

            SaveXltReportCls.ReplaceAction c = this.CopySheet;

            xl.DicDatas.Add("##copysupsheet", c);

            SaveXltReportCls.ReplaceAction d = this.addfilter;
            xl.DicDatas.Add("##addfilter", d);

            xl.Save(Class.MicrosoftFile.GetName("Quality_R42"));
            ((Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
        }

        void Addcolor(Worksheet mySheet, int rowNo, int columnNo)
        {
            mySheet.get_Range("A2", "AY2").Interior.Color = Color.SkyBlue;
            mySheet.get_Range("A2", "AY2").Borders.LineStyle = XlLineStyle.xlContinuous;

            Range usedRange = mySheet.UsedRange;
            Range rows = usedRange.Rows;
            int count = 0;

            foreach (Range row in rows)
            {
                if (count > 0)
                {
                    Range firstCell = row.Cells[1];

                    string firstCellValue = firstCell.Value as string;
                    if (firstCellValue == null)
                    {
                        continue;
                    }

                    if (firstCellValue.StrEndsWith("GRAND TOTAL"))
                    {
                        row.Interior.Color = Color.Aquamarine;
                        row.Borders.LineStyle = XlLineStyle.xlContinuous;
                    }
                    else if (firstCellValue.StrEndsWith("Total"))
                    {
                        row.Interior.Color = Color.Gold;
                        row.Borders.LineStyle = XlLineStyle.xlContinuous;
                    }
                }

                count++;
            }

            Range formatRange;
            Range ReplaceRange;
            Range ReplaceRange1;

            if (this.Report_Type2 == "True")
            {
                // 單獨將Total金額轉成貨幣
                formatRange = mySheet.get_Range(string.Format("AY{0}", this.dt_printing.Rows.Count + 2), string.Format("AY{0}", this.dt_printing.Rows.Count + 2));
                formatRange.NumberFormat = "$#,##0.00";

                // 只顯示total的值,其他數值為0都轉空白
                ReplaceRange = mySheet.get_Range(string.Format("AY{0}", 3), string.Format("AY{0}", this.dt_printing.Rows.Count + 1));
                ReplaceRange.Replace("0", string.Empty);
                ReplaceRange1 = mySheet.get_Range(string.Format("AX{0}", 3), string.Format("AX{0}", this.dt_printing.Rows.Count + 1));
                ReplaceRange1.Replace("0", string.Empty);
            }
            else
            {
                // 單獨將Total金額轉成貨幣
                formatRange = mySheet.get_Range(string.Format("AY{0}", 2), string.Format("AY{0}", this.dt_All.Rows.Count + 2));
                formatRange.NumberFormat = "$#,##0.00";
            }
        }

        void CopySheet(Worksheet mySheet, int rowNo, int columnNo)
        {
            _Application myExcel = null;
            _Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            Worksheet aftersheet = mySheet;

            int idx = 0;
            for (int i = 0; i < this.month12.Rows.Count; i++)
            {
                mySheet.Cells[2, 1] = "Shell Supplier";
                idx += 1;
                mySheet.Cells[2, (idx * 4) - 2] = "Style";
                mySheet.Cells[2, (idx * 4) - 1] = "Shell";
                mySheet.Cells[2, idx * 4] = "Qty";
                mySheet.Cells[2, (idx * 4) + 1] = "Complaint Value";
            }

            foreach (var item in this.dicSUP)
            {
                aftersheet = myExcel.Sheets.Add(After: aftersheet);
                aftersheet.Cells[1, 1] = "##SUPDetail" + item.Key;
                aftersheet.Cells[3, 1] = "##SUPSheetName" + item.Key;
                aftersheet.Cells[3, 1].Font.Color = Color.Transparent;

                aftersheet.Cells[4, 1] = "##addfilter";
                aftersheet.Name = item.Key; // Sheet Name
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
