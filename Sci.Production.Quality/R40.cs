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

            tsql = @"declare @dRanges table(starts int , ends int, name varchar(3))
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

                    select Target,Claimed.Claimed,sh.qty[Shipped],isnull(round(sum(Claimed.Claimed)/sum(sh.qty),6)*100,0) [adiComp],Claimed.month1,Claimed.YEAR1
                    into #temp
                    from dbo.ADIDASComplainTarget 
                    outer apply(SELECT left(cast(a.StartDate as varchar(10)),7) ym , sum(b.Qty) Claimed,dateadd(MONTH,-3,a.StartDate) themonth ,MONTH(a.StartDate)[month1],YEAR(a.StartDate) [YEAR1]
			                    FROM dbo.ADIDASComplain a
			                    INNER JOIN DBO.ADIDASComplain_Detail b ON B.ID = a.ID
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate) Claimed
                    outer apply(SELECT dateadd(MONTH,5,Claimed.themonth) as six,YEAR(a.StartDate)[YEAR1]
			                    FROM dbo.ADIDASComplain a
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate) as ff 
                    outer apply (SELECT ISNULL(SUM(a.Qty),0)/6 AS Qty FROM ADIDASComplain_MonthlyQty a
			                    WHERE a.YearMonth BETWEEN claimed.themonth AND ff.six
			                    AND a.BrandID = 'ADIDAS')sh
                    where year in (@y1,@y2,@y3)
                    group by Target,Claimed.Claimed,sh.qty,Claimed.month1,Claimed.YEAR1

                    select dRanges.name,#temp.Target,SUM(year1.Claimed)[Claimed],SUM(year1.Shipped)[Shipped],SUM(year1.adiComp)[adiComp],SUM(year2.Claimed)[Claimed],SUM(year2.Shipped)[Shipped],SUM(year2.adiComp)[adiComp],SUM(year3.Claimed)[Claimed],SUM(year3.Shipped)[Shipped],SUM(year3.adiComp)[adiComp] from dbo.#temp
                    inner join @dRanges as dRanges on #temp.month1 between dRanges.starts and dRanges.ends 
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE YEAR1=@y1)AS year1
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE YEAR1=@y2)AS year2
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE YEAR1=@y3)AS year3
					GROUP BY dRanges.name,#temp.Target,year1.Claimed,year1.Shipped,year1.adiComp,year2.Claimed,year2.Shipped,year2.adiComp,year2.Claimed,year2.Shipped,year2.adiComp
                    DROP TABLE #temp";
            result = DBProxy.Current.Select("", tsql,out dtt);

            return base.ValidateInput();

        }
        

       
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
           

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
