using Ict;
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

            DataTable Year = null;
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
         string Brand;
         string Year;
         string Report_Type1;
         string Report_Type2;
         DualResult result;
         DataTable dt;
         DataTable dt_All;
         DataTable [] alldt;
         DataTable month12=null;
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

             if (radiobtn_pill_snagg_detail.Checked == true)
             {

                 List<SqlParameter> lis = new List<SqlParameter>();
                 string sqlWhere = ""; string rt = ""; string gb = "";//string ob = "";
                 List<string> sqlWheres = new List<string>();

                 if (Brand != "")
                 {
                     sqlWheres.Add(" b.BrandID =@Brand");
                     lis.Add(new SqlParameter("@Brand", Brand));
                 }
                 if (Year != "")
                 {
                     sqlWheres.Add("year(a.StartDate)=@Year");
                     lis.Add(new SqlParameter("@Year", Year));
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

                 gb = "group by a.StartDate,b.supplier,b.StyleID,b.refno,dRanges.name,dRanges.starts";
                 //ob = "order by dRanges.starts";
                 string sqlcmd = string.Format(@"create table #dRanges(starts int , ends int, name varchar(50))
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
                                    ,dRanges.name
                                    ,dRanges.starts
                                    into #temp
                                    FROM DBO.ADIDASComplain A 
                                    INNER JOIN DBO.ADIDASComplain_Detail B ON B.ID = A.ID
                                    INNER join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends" + " " + sqlWhere + " " + rt + " " + gb + " " +
                                    @"select name as mon from (select DISTINCT dRanges.name,dRanges.starts from #temp T 
				                      left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends
				                      GROUP BY m ,dRanges.starts,dRanges.name)as s  ORDER BY starts"
                                    +" "+ @"select * from #temp
                                        drop table #dRanges
                                        --drop table #temp");
                 SqlConnection conn;
                 result = DBProxy.Current.OpenConnection("", out conn);
                 if (!result){return result;}
                 result = DBProxy.Current.SelectByConn(conn,sqlcmd,lis,out alldt);
                 if (!result) {return result; }


                 string sp = @"select * from (select supplier from  #temp  group by supplier) as sp";
                 sp = sp + Environment.NewLine;
                 month12 = alldt[0];
                 for (int i = 0; i < month12.Rows.Count; i++)
                 {
                     string sss = month12.Rows[i]["mon"].ToString();
                     string o = string.Format("outer apply(select supplier,m,styleid as Style,Refno as Shell,sum(QTY)as Qty,sum(ValueinUSD)as Complaint_Value from #temp t where t.m ='{0}' and t.supplier=sp.supplier group by supplier,m,styleid,Refno,Qty,ValueinUSD) as {0}", sss);
                     sp = sp + Environment.NewLine + o;
                 }
                 result = DBProxy.Current.SelectByConn(conn,sp,lis, out dt);
                 if (!result) {return result;}
                 ////dt.Columns.Remove("starts");

                 //int startIndex = 1;
                 ////最後一列Total

                 //DataRow totalrow = dt.NewRow();
                 //totalrow[0] = "YTD";


                 ////for alltemp每個欄位
                 //decimal TTColumnAMT = 0;
                 //for (int colIdx = startIndex; colIdx < dt.Columns.Count; colIdx++)
                 //{

                 //    TTColumnAMT = 0;
                 //    //for alltemp每一列
                 //    for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                 //    {
                 //        TTColumnAMT += Convert.ToDecimal(dt.Rows[rowIdx][colIdx]);
                 //    }

                 //    totalrow[colIdx] = TTColumnAMT;
                 //}

                 if (null == dt || dt.Rows.Count == 0)
                 {
                     return new DualResult(false, "Data not found");
                 }

                 //dt.Rows.Add(totalrow);

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
                     return result;
                     //this.ShowErr(result);
                 }

                 else
                 {


                 }
             } 
             return result ;  //base.OnAsyncDataLoad(e);
         }
         protected override bool OnToExcel(Win.ReportDefinition report)
         {
             if(radiobtn_pill_snagg_detail.Checked==true)
             {
                 var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                 saveDialog.ShowDialog();
                 string outpath = saveDialog.FileName;
                 if (outpath.Empty())
                 {
                     return false;
                 }
                 Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R42_Pilling & Snagging Detail.xltx");
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
                 xl.dicDatas.Add("##psd", xdt_All);
                 xl.Save(outpath, true);
             }
             else
             {
             }

             return true;
         }

    }
}
