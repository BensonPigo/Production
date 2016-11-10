using Ict;
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
        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            System.Data.DataTable Year = null;
            string cmd = (@"declare @d date = getdate()
                            declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
                            declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
                            declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))
                            select @y1 as y1,@y2 as y2,@y3 as y3 into #temp 
                            select * from #temp 
                            unpivot(M FOR #temp IN 
                            (y1,y2,y3)) as years");
            DBProxy.Current.Select("", cmd, out Year);
            Year.DefaultView.Sort = "M";
            this.combo_Year.DataSource = Year;
            this.combo_Year.ValueMember = "M";
            this.combo_Year.DisplayMember = "M";

            this.combo_Brand.SelectedIndex = 0;
            print.Enabled = false;
        }
        string Brand; string s;
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
        protected override bool ValidateInput()
        {
            Brand = combo_Brand.Text.ToString();
            Year = combo_Year.Text.ToString();
            Report_Type1 = radiobtn_pill_snagg_detail.Checked.ToString();
            Report_Type2 = radiobtn_print_detail.Checked.ToString();
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

                if (radiobtn_pill_snagg_detail.Checked == true)
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

                                    SELECT isnull(B.Supplier,'Other') as supplier 
                                    ,format(a.startdate,'MMMMM')[m]
                                    ,b.StyleID
                                    ,b.refno
                                    ,sum(b.qty) qty
                                    ,sum(b.ValueinUSD) ValueinUSD
                                    into #temp
                                    FROM DBO.ADIDASComplain A 
                                    INNER JOIN DBO.ADIDASComplain_Detail B ON B.ID = A.ID" + " " + sqlWhere + " " + rt + " " + gb + " " +

                                   @"select name as mon from (select DISTINCT dRanges.name,dRanges.starts from #temp T 
				                      left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends
				                      GROUP BY m ,dRanges.starts,dRanges.name)as s  ORDER BY starts

                                    select distinct supplier from #temp

                                    select * from #temp


                                    select  distinct * from (
	                         select Shell_Supplier=supplier from  #temp cc
	                         where m in (select m from (select *,idx=ROW_NUMBER()over(order by cnt desc) from (select m,cnt=count(*) from #temp where supplier = cc.supplier group by m) c) d where idx = 1)
                                AND SUPPLIER != ''
	                         group by supplier,refno) main");
                SqlConnection conn;
                result = DBProxy.Current.OpenConnection("", out conn);
                if (!result) { return result; }
                result = DBProxy.Current.SelectByConn(conn, sqlcmd, out alldt);
                if (!result) { return result; }

                string sp = @"select  * from (
	                         select Shell_Supplier=supplier,idx=ROW_NUMBER()OVER(partition by supplier order by supplier) from  #temp cc
	                         where m in (select m from (select *,idx=ROW_NUMBER()over(order by cnt desc) from (select m,cnt=count(*) from #temp where supplier = cc.supplier group by m) c) d where idx = 1)
                             AND supplier!=''
	                         group by supplier,refno) main";

                month12 = alldt[0];
                string month_Columns = "";
                string Qty_SumColumns = "";
                string Complaint_SumColumns = "";
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
                
                var total_Rows = dt.Sum(month_Columns, "Shell_Supplier");
                
                foreach (var ttl_row in total_Rows)
                {
                    ttl_row["Shell_Supplier"] = ttl_row["Shell_Supplier"].ToString().Trim() + "Total";
                }
                total_Rows[total_Rows.Count - 1]["Shell_Supplier"] = "GRAND TOTAL";
    
                if (null == dt_All || 0 == dt_All.Rows.Count)
                {
                 
                    dt_All = dt;
                }
                else
                {
                    dt_All.Merge(dt);
                }

                if (!result)
                {
                    //return result;
                    this.ShowErr(result);
                }
                

                

                allsupplier = alldt[3];
                dicSUP.Clear();
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
                    DBO.ADIDASComplain A 
                    INNER JOIN DBO.ADIDASComplain_Detail B ON B.ID = A.ID
                    left join dbo.ADIDASComplainDefect c on c.ID=b.DefectMainID
                    left join dbo.ADIDASComplainDefect_Detail d on d.id=b.DefectMainID and d.SubID=b.DefectSubID" + " " + sqlWhere + "and supplier='{0}'" + rt + " " + ob, sss);

                    result = DBProxy.Current.SelectByConn(conn, scmd, out dat);
                    dicSUP.Add(sss, dat);
                    if (!result)
                    {
                        return result;
                    }
                }
                
            return result;  //base.OnAsyncDataLoad(e);
        }

        Dictionary<string, System.Data.DataTable> dicSUP = new Dictionary<string, System.Data.DataTable>();


        protected override bool OnToExcel(Win.ReportDefinition report)
        {

                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }
                Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R42.xltx");
                SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dt_All);


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
                xdt_All.lisTitleMerge.Add(dic);
                xdt_All.ShowHeader = true;
              

                foreach (var item in dicSUP)
                {
                    string supplier = item.Key;
                    SaveXltReportCls.xltRptTable x_All = new SaveXltReportCls.xltRptTable(item.Value);

                    for (int i = 0; i < x_All.Columns.Count; i++)
                    {
                        SaveXltReportCls.xlsColumnInfo xlc = new SaveXltReportCls.xlsColumnInfo(i + 1);
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
                        x_All.lisColumnInfo.Add(xlc);    
                    }
                    
                    xl.dicDatas.Add("##SUPSheetName" + supplier, item.Key);
                    xl.dicDatas.Add("##SUPDetail" + supplier, x_All);
                }


                xl.VarToSheetName = "##SUPSheetName";
                xl.dicDatas.Add("##psd", xdt_All);

                SaveXltReportCls.ReplaceAction b = Addcolor;
                xl.dicDatas.Add("##addcolor", b);

                SaveXltReportCls.ReplaceAction c = CopySheet;
                xl.dicDatas.Add("##copysupsheet", c);


                SaveXltReportCls.ReplaceAction d = addfilter;
                xl.dicDatas.Add("##addfilter", d);
               
                xl.Save(outpath, true);

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
                aftersheet.Cells[1,1] = "##SUPDetail" + item.Key;
                aftersheet.Cells[3,1] = "##SUPSheetName" + item.Key;
                aftersheet.Cells[3, 1].Font.Color = Color.Transparent;

                aftersheet.Cells[4,1] = "##addfilter";
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
