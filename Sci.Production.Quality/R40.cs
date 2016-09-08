using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R40 : Sci.Win.Tems.PrintForm
    {
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            print.Enabled = false;
            this.comboBox_brand.SelectedIndex = 0;
        }
        string Brand;
        string Year;
        string Factory;
        string tsql;
        DualResult result;
        DataTable dtt;
        DataTable dt_All;
        DataTable dt_Tmp;

        protected override bool ValidateInput()
        {
            Brand = comboBox_brand.SelectedItem.ToString();
            Year = radiobtn_byYear.Checked.ToString();
            Factory = radiobtn_byfactory.Checked.ToString();

            tsql =@"declare @dRanges table(starts int , ends int, name varchar(3))
                    insert into @dRanges values
                    (1,1,'Jan'),
                    (2,2,'Feb'),
                    (3,3,'Mar'),
                    (4,4,'Apr'),
                    (5,5,'May'),
                    (6,6,'Jun'),
                    (7,7,'Jul'),
                    (8,8,'Aug'),
                    (9,9,'Sep'),
                    (10,10,'Oct'),
                    (11,11,'Nov'),
                    (12,12,'Dec')

                    declare @d date = dateadd(MONTH, -1, getdate()) 
                    select @d
                    declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
                    declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
                    declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))

                    select Target,Claimed.Claimed,sh.qty[Shipped],isnull(round(sum(Claimed.Claimed)/sum(sh.qty),6)*100,0) [adiComp],Claimed.month1
                    into #temp
                    from dbo.ADIDASComplainTarget 
                    outer apply(SELECT left(cast(a.StartDate as varchar(10)),7) ym , sum(b.Qty) Claimed,dateadd(MONTH,-3,a.StartDate) themonth ,MONTH(a.StartDate)[month1]
			                    FROM dbo.ADIDASComplain a
			                    INNER JOIN DBO.ADIDASComplain_Detail b ON B.ID = a.ID
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate) Claimed
                    outer apply(SELECT dateadd(MONTH,5,Claimed.themonth) as six
			                    FROM dbo.ADIDASComplain a
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate) as ff 
                    outer apply (SELECT ISNULL(SUM(a.Qty),0)/6 AS Qty FROM ADIDASComplain_MonthlyQty a
			                    WHERE a.YearMonth BETWEEN claimed.themonth AND ff.six
			                    AND a.BrandID = 'ADIDAS' 
			                    AND a.factoryid in (select id from dbo.SCIFty 
			                    where CountryID= (select f.CountryID from dbo.Factory f where f.id='MAI')))sh
                    where year in (@y1,@y2,@y3)
                    group by Target,Claimed.Claimed,sh.qty,Claimed.month1

                    select #temp.*,dRanges.name from dbo.#temp
                    inner join @dRanges as dRanges on #temp.month1 between dRanges.starts and dRanges.ends

                    drop table #temp";


            return base.ValidateInput();

        }
        

       
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            dt_All = null;
            dt_Tmp = null;
            
            SortedList<string, string> year_All = new SortedList<string, string>();
            Dictionary<string, Dictionary<string, decimal>> month_FtyAmt = new Dictionary<string, Dictionary<string, decimal>>();           

            // 抓Query資料並list有哪些years
            DataTable years = new DataTable();
            SqlConnection conn;
            result = DBProxy.Current.OpenConnection("", out conn);
            if (!result) { return result; }
            result = DBProxy.Current.SelectByConn(conn, tsql, out years);
            //DualResult result1 = DBProxy.Current.SelectByConn(conn, tsql, out years);
            //if (!result1) { return result1; }

            //用years重組Pivot的Tsql
            if (null == years || years.Rows.Count == 0)
            {
                return new DualResult(false, "Data not fund");
            }

            string listByYear = 
@"
";

            return base.OnAsyncDataLoad(e);
        }
        
   protected override bool OnToExcel(Win.ReportDefinition report)
        {
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            return base.OnToExcel(report);
        }

    }
}
