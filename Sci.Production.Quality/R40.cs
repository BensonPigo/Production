using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    public partial class R40 : Win.Tems.PrintForm
    {
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.print.Enabled = false;
            this.comboBrand.SelectedIndex = 0;
        }

        private string Brand;
        private string Year;
        private string Factory;
        private DualResult result;
        private System.Data.DataTable dtt;
        private System.Data.DataTable dt;
        private System.Data.DataTable dtt_All;
        private System.Data.DataTable alltemp;
        private System.Data.DataTable alltemp_All;
        private System.Data.DataTable[] alltemps;
        private readonly string userfactory = Env.User.Factory;

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Brand = this.comboBrand.SelectedItem.ToString();
            this.Year = this.radiobyYear.Checked.ToString();
            this.Factory = this.radiobyfactory.Checked.ToString();

            return true;
        }

        private System.Data.DataTable allFactory = null;

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.dtt_All = null;
            this.dtt = null;
            this.dt = null;
            this.alltemp = null;
            this.alltemp_All = null;

            if (this.radiobyYear.Checked == true)
            {
                #region By Year
                string sqlcmd = string.Format(
                    @"
create table #dRangesM(m varchar(2) ,m1 int,  Mname varchar(3))
insert into #dRangesM values
('01',1,'Jan'),   ('02',2,'Feb'),   ('03',3,'Mar'),
('04',4,'Apr'),   ('05',5,'May'),   ('06',6,'Jun'),
('07',7,'Jul'),   ('08',8,'Aug'),   ('09',9,'Sep'),
('10',10,'Oct'),  ('11',11,'Nov'),  ('12',12,'Dec')
create table #dRangesY( Y varchar(4))
insert into #dRangesY values
(format(dateadd(year, -2, dateadd(month, -1, getdate())), 'yyyy')), (format(dateadd(year, -1, dateadd(month, -1, getdate())), 'yyyy')), (format(dateadd(month, -1, getdate()), 'yyyy'))

select *
into #daterange
from #dRangesY cross join #dRangesM 

select 
	[Year1]= Y,
	[month]= Mname,
	[month1]= m1,
	[Target]= (a.Target/100),
	[Claimed]= isnull(Claimed.Claimed,0),
	[Shipped]= CONVERT(INT,isnull(sh.Qty,0))
into #temp
from ADIDASComplainTarget a,#daterange d
outer apply
(
	SELECT 
		sum(b.Qty) Claimed
	FROM dbo.ADIDASComplain a WITH (NOLOCK) 
	INNER JOIN DBO.ADIDASComplain_Detail b WITH (NOLOCK) ON B.ID = a.ID
	where format((a.StartDate),'yyyy') = d.y and format((a.StartDate),'MM') = d.m AND a.Junk=0
	group by a.StartDate
) Claimed
outer apply (	
	select  Qty =SUM(o.Qty)
	from orders o
	where cast(o.BuyerDelivery as date) BETWEEN convert(date,concat(d.Y,d.m,'01'))  AND dateadd(day,-1,dateadd(month,1,convert(date,concat(d.Y,d.m,'01'))))
	and o.BrandID = '{0}'
	and o.FactoryID in (select id from dbo.SCIFty where CountryID = (select CountryID from Factory where id='{1}'))	
	and o.Junk = 0
	and o.Category in ('B','S')
)sh 

declare @dRanges table(starts int , ends int, name varchar(3))
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

select dRanges.name[ ] ,
dRanges.starts,
[Target] = Target,
isnull(year1.Claimed,0)[Claimed1],
isnull(year1.Shipped,0)[Shipped1],
isnull(year1.adicomp,0)[adicomp1],
isnull(year2.Claimed,0)[Claimed2],
isnull(year2.Shipped,0)[Shipped2],
isnull(year2.adicomp,0)[adicomp2],
isnull(year3.Claimed,0)[Claimed3],
isnull(year3.Shipped,0)[Shipped3],
isnull(year3.adicomp,0)[adicomp3] 
from dbo.#temp
inner join @dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends 
OUTER APPLY(
	SELECT #temp.Claimed
		   , #temp.Shipped
		   , adicomp=iif(sum(#temp.Claimed)<>0,round(sum(#temp.Claimed)/sum(#temp.Shipped),6),0) 
	FROM #temp 
	WHERE YEAR1=format(dateadd(year,-2, dateadd(month, -1, getdate())),'yyyy') 
		  and dRanges.starts=month1 
	group by #temp.Claimed,#temp.Shipped
)AS year1
OUTER APPLY(
	SELECT #temp.Claimed
		   , #temp.Shipped
		   , adicomp=iif(sum(#temp.Claimed)<>0,round(sum(#temp.Claimed)/sum(#temp.Shipped),6),0) 
	FROM #temp 
	WHERE YEAR1=format(dateadd(year,-1, dateadd(month, -1, getdate())),'yyyy') 
		  and dRanges.starts=month1 
	group by #temp.Claimed,#temp.Shipped
)AS year2
OUTER APPLY(
	SELECT #temp.Claimed
		   , #temp.Shipped,adicomp=iif(sum(#temp.Claimed)<>0,round(sum(#temp.Claimed)/sum(#temp.Shipped),6),0) 
	FROM #temp 
	WHERE YEAR1=format(dateadd(month, -1, getdate()),'yyyy') 
		  and dRanges.starts=month1 
	group by #temp.Claimed,#temp.Shipped
)AS year3
outer apply(
	select Target1=isnull(sum(#temp.Target),0) 
	from #temp 
	where dRanges.starts=month1
)AS tg1
GROUP BY dRanges.name,Target,year1.Claimed,year1.Shipped,year2.Claimed,year2.Shipped,year3.Claimed,year3.Shipped,dRanges.starts,year1.adicomp,year2.adicomp,year3.adicomp
order by dRanges.starts

drop table #dRangesM,#dRangesY,#daterange,#temp", this.Brand, this.userfactory);
                this.result = DBProxy.Current.Select(string.Empty, sqlcmd, out this.dtt);
                if (MyUtility.Check.Empty(this.dtt))
                {
                    return new DualResult(false, "Data not found");
                }

                this.dtt.Columns.Remove("starts");
                int startIndex = 1;

                // 最後一列Total
                DataRow totalrow = this.dtt.NewRow();
                totalrow[0] = "YTD";

                // for dt每個欄位
                decimal tTColumnAMT = 0;
                for (int colIdx = startIndex; colIdx < this.dtt.Columns.Count; colIdx++)
                {
                    tTColumnAMT = 0;

                    // for dt每一列
                    for (int rowIdx = 0; rowIdx < this.dtt.Rows.Count; rowIdx++)
                    {
                        tTColumnAMT += Convert.ToDecimal(this.dtt.Rows[rowIdx][colIdx]);
                    }

                    if (colIdx == 1)
                    {
                        totalrow[colIdx] = tTColumnAMT == 0 ? 0 : tTColumnAMT / 12;
                    }
                    else if ((colIdx - 1) % 3 == 0)
                    {
                        totalrow[colIdx] = tTColumnAMT == 0 ? 0 : MyUtility.Convert.GetDecimal(totalrow[colIdx - 2]) / MyUtility.Convert.GetDecimal(totalrow[colIdx - 1]);
                    }
                    else
                    {
                        totalrow[colIdx] = tTColumnAMT;
                    }
                }

                if (this.dtt == null || this.dtt.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found");
                }

                this.dtt.Rows.Add(totalrow);

                if (this.dtt_All == null || this.dtt_All.Rows.Count == 0)
                {
                    this.dtt_All = this.dtt;
                }
                else
                {
                    this.dtt_All.Merge(this.dtt);
                }

                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }
            else
            {
                #region By Factory 橫工廠別 縱月份
                string allt = string.Format(
                    $@"
create table #dRangesM(m varchar(2) ,m1 int,  Mname varchar(3))
	insert into #dRangesM values
    ('01',1,'Jan'),   ('02',2,'Feb'),   ('03',3,'Mar'),
    ('04',4,'Apr'),   ('05',5,'May'),   ('06',6,'Jun'),
    ('07',7,'Jul'),   ('08',8,'Aug'),   ('09',9,'Sep'),
    ('10',10,'Oct'),  ('11',11,'Nov'),  ('12',12,'Dec')
create table #dRangesY( Y varchar(4))
	insert into #dRangesY values
	(format(dateadd(year,-2,getdate()),'yyyy')),(format(dateadd(year,-1,getdate()),'yyyy')),(format(getdate(),'yyyy'))
select * into #F 
from dbo.SCIFty where id in  (select id from dbo.SCIFty where CountryID = (select CountryID from Factory where id = '{Env.User.Factory}'))

select *
into #daterange
from #dRangesY cross join #dRangesM 
cross join #F

select 
	[Year1]= Y,
	[month]= Mname,
	[month1]= m1,
	[FactoryID]= id,
	[Target]= (a.Target/100),
	[Claimed]= isnull(Claimed.Claimed,0),
	[Shipped]= CONVERT(INT,isnull(sh.Qty,0))
into #AllTemp
from ADIDASComplainTarget a,#daterange d
outer apply
(
	SELECT 
		sum(b.Qty) Claimed
	FROM dbo.ADIDASComplain a WITH (NOLOCK) 
	INNER JOIN DBO.ADIDASComplain_Detail b WITH (NOLOCK) ON B.ID = a.ID
	where format((a.StartDate),'yyyy') = d.y and format((a.StartDate),'MM') = d.m AND a.Junk=0
	and FactoryID = d.id
	group by a.StartDate,B.FactoryID
) Claimed
outer apply (	
	select Qty =SUM(o.Qty)
	from orders o
	where cast(o.BuyerDelivery as date) BETWEEN convert(date,concat(d.Y,d.m,'01'))  AND dateadd(day,-1,dateadd(month,1,convert(date,concat(d.Y,d.m,'01'))))
	and o.BrandID = 'ADIDAS'	
	and o.FactoryID = d.ID
	and o.Junk = 0
	and o.Category in ('B','S')
	GROUP BY o.FactoryID
)sh 

select distinct FactoryID from #AllTemp
select distinct month from #AllTemp
select * from #AllTemp", this.Brand, this.userfactory);
                SqlConnection conn;
                this.result = DBProxy.Current.OpenConnection(string.Empty, out conn);
                if (!this.result)
                {
                    return this.result;
                }

                this.result = DBProxy.Current.SelectByConn(conn, allt, out this.alltemps);
                if (!this.result)
                {
                    return this.result;
                }

                string s = @"
select DISTINCT * 
from ( 
    select month
           , month1
           , Target 
    from #AllTemp T 
	left join #dRangesM as dRanges on dRanges.Mname = T.month 
)as s";
                s = s + Environment.NewLine;

                this.allFactory = this.alltemps[0];
                for (int i = 0; i < this.allFactory.Rows.Count; i++)
                {
                    string sss = this.allFactory.Rows[i]["FactoryID"].ToString();
                    string o = string.Format(
                        @"
outer apply (
    select {0}_Claimed = isnull(sum(claimed),0)
           , {0}_Shipped = isnull(sum(shipped),0)
           , {0}_Adicomp=iif(isnull(sum(shipped),0)=0,0,round(sum(claimed)/sum(shipped),6)) 
    from #AllTemp t 
    where t.month = s.month 
          and t.YEAR1=(select cast(datepart(year,getdate()) as varchar(4))) 
          and t.FactoryID = '{0}'
) as {0}", sss);
                    s = s + Environment.NewLine + o;
                }

                s = s + Environment.NewLine + "order by month1";
                this.result = DBProxy.Current.SelectByConn(conn, s, out this.alltemp);

                if (!this.result)
                {
                    return this.result;
                }

                this.alltemp.Columns.Remove("month1");

                int startIndex = 1;

                // 最後一列Total
                DataRow totalrow = this.alltemp.NewRow();
                totalrow[0] = "YTD";

                // for alltemp每個欄位
                decimal tTColumnAMT = 0;
                for (int colIdx = startIndex; colIdx < this.alltemp.Columns.Count; colIdx++)
                {
                    tTColumnAMT = 0;

                    // for alltemp每一列
                    for (int rowIdx = 0; rowIdx < this.alltemp.Rows.Count; rowIdx++)
                    {
                        tTColumnAMT += Convert.ToDecimal(this.alltemp.Rows[rowIdx][colIdx]);
                    }

                    if (colIdx == 1)
                    {
                        totalrow[colIdx] = tTColumnAMT == 0 ? 0 : tTColumnAMT / 12;
                    }
                    else if ((colIdx - 1) % 3 == 0)
                    {
                        totalrow[colIdx] = tTColumnAMT == 0 ? 0 : MyUtility.Convert.GetDecimal(totalrow[colIdx - 2]) / MyUtility.Convert.GetDecimal(totalrow[colIdx - 1]);
                    }
                    else
                    {
                        totalrow[colIdx] = tTColumnAMT;
                    }
                }

                if (this.alltemp == null || this.alltemp.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found");
                }

                this.alltemp.Rows.Add(totalrow);

                if (this.alltemp_All == null || this.alltemp_All.Rows.Count == 0)
                {
                    this.alltemp_All = this.alltemp;
                }
                else
                {
                    this.alltemp_All.Merge(this.alltemp);
                }

                if (!this.result)
                {
                    return this.result;
                }
                #endregion

                #region By Factory 橫年度 縱月份

                this.allFactory = this.alltemps[0];
                this.dicFTY.Clear();
                for (int i = 0; i < this.allFactory.Rows.Count; i++)
                {
                    string sss = this.allFactory.Rows[i]["Factoryid"].ToString();

                    string scmd = string.Format(
                        @"declare @dRanges table(starts int , ends int, name varchar(3))
insert into @dRanges 
values (1,1,'Jan')
       , (2,2,'Feb')
       , (3,3,'Mar')
       , (4,4,'Apr')
       , (5,5,'May')
       , (6,6,'Jun')
       , (7,7,'Jul')
       , (8,8,'Aug')
       , (9,9,'Sep')
       , (10,10,'Oct')
       , (11,11,'Nov')
       , (12,12,'Dec')
       
select dRanges.name[ ] 
       , dRanges.starts
       , [Target]=t.Target
       , [Claimed1] = isnull(year1.Claimed,0)
       , [Shipped1] = isnull(convert(int,year1.Shipped),0)
       , [adicomp1] = isnull(year1.adicomp,0)
       , [Claimed2] = isnull(year2.Claimed,0)
       , [Shipped2] = isnull(convert(int,year2.Shipped),0)
       , [adicomp2] = isnull(year2.adicomp,0)
       , [Claimed3] = isnull(year3.Claimed,0)
       , [Shipped3] = isnull(convert(int,year3.Shipped),0)
       , [adicomp3]  =isnull(year3.adicomp,0)
from #AllTemp as t
inner join @dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends 
OUTER APPLY(
     SELECT s.Claimed
            , s.Shipped
            , adicomp=iif(s.Shipped<>0 ,round(sum(s.Claimed)/sum(s.Shipped),6) ,0)
     FROM #AllTemp as s 
     WHERE YEAR1=format(dateadd(year,-2,getdate()),'yyyy') 
           and dRanges.starts = month1 
           and FactoryID = '{0}' 
     group by s.Claimed,s.Shipped
) AS year1
OUTER APPLY(
     SELECT s.Claimed
            , s.Shipped
            , adicomp=iif(s.Shipped<>0 ,round(sum(s.Claimed)/sum(s.Shipped),6) ,0)
     FROM #AllTemp as s 
     WHERE YEAR1=format(dateadd(year,-1,getdate()),'yyyy') 
           and dRanges.starts=month1 
           and FactoryID='{0}' 
     group by s.Claimed,s.Shipped
) AS year2
OUTER APPLY(
     SELECT s.Claimed
            , s.Shipped
            , adicomp=iif(s.Shipped<>0 ,round(sum(s.Claimed)/sum(s.Shipped),6) ,0)
     FROM #AllTemp as s 
     WHERE YEAR1=format(getdate(),'yyyy') 
           and dRanges.starts=month1 
           and FactoryID='{0}' 
     group by s.Claimed,s.Shipped
)AS year3		           
where t.factoryid='" + this.userfactory + @"' 
GROUP BY  dRanges.name,Target,year1.Claimed,year1.Shipped,year2.Claimed,year2.Shipped,year3.Claimed,year3.Shipped,dRanges.starts,year1.adicomp,year2.adicomp,year3.adicomp
order by dRanges.starts

drop table #dRangesM,#dRangesY,#daterange,#F
--DROP TABLE #temp", sss);

                    this.result = DBProxy.Current.SelectByConn(conn, scmd, out this.dt);

                    if (!this.result)
                    {
                        return this.result;
                    }

                    this.dt.Columns.Remove("starts");
                    int startIndex1 = 1;

                    // 最後一列Total
                    DataRow totalrow1 = this.dt.NewRow();
                    totalrow1[0] = "YTD";

                    // for dt每個欄位
                    decimal tTColumnAMT1 = 0;
                    for (int colIdx = startIndex1; colIdx < this.dt.Columns.Count; colIdx++)
                    {
                        tTColumnAMT1 = 0;

                        // for dt每一列
                        for (int rowIdx = 0; rowIdx < this.dt.Rows.Count; rowIdx++)
                        {
                            tTColumnAMT1 += Convert.ToDecimal(this.dt.Rows[rowIdx][colIdx]);
                        }

                        if (colIdx == 1)
                        {
                            totalrow1[colIdx] = tTColumnAMT1 == 0 ? 0 : tTColumnAMT1 / 12;
                        }
                        else if ((colIdx - 1) % 3 == 0)
                        {
                            totalrow1[colIdx] = tTColumnAMT1 == 0 ? 0 : MyUtility.Convert.GetDecimal(totalrow1[colIdx - 2]) / MyUtility.Convert.GetDecimal(totalrow1[colIdx - 1]);
                        }
                        else
                        {
                            totalrow1[colIdx] = tTColumnAMT1;
                        }
                    }

                    if (this.dt == null || this.dt.Rows.Count == 0)
                    {
                        return new DualResult(false, "Data not found");
                    }

                    this.dt.Rows.Add(totalrow1);
                    this.dicFTY.Add(sss, this.dt);

                    if (!this.result)
                    {
                        return this.result;
                    }
                }
                #endregion
            }

            return this.result;
        }

        private readonly Dictionary<string, System.Data.DataTable> dicFTY = new Dictionary<string, System.Data.DataTable>();
        private string stringyear2;
        private string stringyear3;
        private string stringyear4;

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.radiobyYear.Checked == true)
            {
                #region By Year

                var saveDialog = MyExcelPrg.GetSaveFileDialog(MyExcelPrg.Filter_Excel);

                SaveXltReportCls xl = new SaveXltReportCls("Quality_R40_ByYear.xltx");
                SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(this.dtt_All);
                DateTime newtodaty = DateTime.Today;
                int year1 = newtodaty.Year;
                int year2;
                int year3;
                int year4;
                int month = newtodaty.Month;
                if (month == 1)
                {
                    year2 = year1 - 1;
                    year3 = year1 - 2;
                    year4 = year1 - 3;
                }
                else
                {
                    year2 = year1;
                    year3 = year1 - 1;
                    year4 = year1 - 2;
                }

                this.stringyear2 = Convert.ToString(year2);
                this.stringyear3 = Convert.ToString(year3);
                this.stringyear4 = Convert.ToString(year4);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(" ", "1,2");
                dic.Add(this.stringyear4, "3,5");
                dic.Add(this.stringyear3, "6,8");
                dic.Add(this.stringyear2, "9,11");
                xdt_All.LisTitleMerge.Add(dic);
                xdt_All.ShowHeader = true;
                xdt_All.BoAutoFitColumn = true;
                xl.DicDatas.Add("##by_year", xdt_All);
                SaveXltReportCls.ReplaceAction a = this.AddRpt;
                xl.DicDatas.Add("##addrpt", a);

                xl.Save(Class.MicrosoftFile.GetName("Quality_R40_ByFactory"));
                #endregion

            }
            else if (this.radiobyfactory.Checked == true)
            {
                #region By Factory

                var saveDialog = MyExcelPrg.GetSaveFileDialog(MyExcelPrg.Filter_Excel);

                SaveXltReportCls xl = new SaveXltReportCls("Quality_R40_ByFactory.xltx", keepApp: true);
                SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(this.alltemp_All);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(" ", "1,2");
                for (int i = 0; i < this.allFactory.Rows.Count; i++)
                {
                    dic.Add(
                        this.allFactory.Rows[i]["FactoryID"].ToString(),
                        string.Format("{0},{1}", (i * 3) + 3, (i * 3) + 5));
                }

                xdt_All.LisTitleMerge.Add(dic);
                xdt_All.ShowHeader = true;

                foreach (var item in this.dicFTY)
                {
                    string fty = item.Key;
                    SaveXltReportCls.XltRptTable x_All = new SaveXltReportCls.XltRptTable(item.Value);
                    DateTime newtodaty = DateTime.Today;
                    int year1 = newtodaty.Year;
                    int year2;
                    int year3;
                    int year4;
                    int month = newtodaty.Month;
                    if (month == 1)
                    {
                        year2 = year1 - 1;
                        year3 = year1 - 2;
                        year4 = year1 - 3;
                    }
                    else
                    {
                        year2 = year1;
                        year3 = year1 - 1;
                        year4 = year1 - 2;
                    }

                    this.stringyear2 = Convert.ToString(year2);
                    this.stringyear3 = Convert.ToString(year3);
                    this.stringyear4 = Convert.ToString(year4);

                    Dictionary<string, string> dic1 = new Dictionary<string, string>();
                    dic1.Add(" ", "1,2");
                    dic1.Add(this.stringyear4, "3,5");
                    dic1.Add(this.stringyear3, "6,8");
                    dic1.Add(this.stringyear2, "9,11");
                    x_All.LisTitleMerge.Add(dic1);

                    xl.DicDatas.Add("##ftySheetName" + fty, item.Key);
                    xl.DicDatas.Add("##ftyDetail" + fty, x_All);
                }

                xl.VarToSheetName = "##ftySheetName";
                xdt_All.BoAutoFitColumn = true;
                xl.DicDatas.Add("##by_factory", xdt_All);

                SaveXltReportCls.ReplaceAction b = this.Addfactory;

                xl.DicDatas.Add("##addfactory", b);

                SaveXltReportCls.ReplaceAction c = this.CopySheet;
                xl.DicDatas.Add("##copyftysheet", c);

                xl.Save(Class.MicrosoftFile.GetName("Quality_R40_ByFactory"));
                ((Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
                xl.FinishSave();
                #endregion
            }

            return true; // base.OnToExcel(report);
        }

        private void AddRpt(Worksheet mySheet, int rowNo, int columnNo)
        {
            #region By Year
            // 改名字
            mySheet.Cells[2, 3] = "Claimed";
            mySheet.Cells[2, 4] = "Shipped";
            mySheet.Cells[2, 5] = "adiComp";
            mySheet.Cells[2, 6] = "Claimed";
            mySheet.Cells[2, 7] = "Shipped";
            mySheet.Cells[2, 8] = "adiComp";
            mySheet.Cells[2, 9] = "Claimed";
            mySheet.Cells[2, 10] = "Shipped";
            mySheet.Cells[2, 11] = "adiComp";

            _Application myExcel = null;
            _Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            try
            {
                // 在工作簿 新增一張 統計圖表，單獨放在一個分頁裡面
                mySheet.get_Range("a1", "a14").Select();
                myBook.Charts.Add(Type.Missing, Type.Missing, 1, Type.Missing);

                // 選擇 統計圖表 的 圖表種類
                myBook.ActiveChart.Location(XlChartLocation.xlLocationAsObject, mySheet.Name);
                myBook.ActiveChart.ChartType = XlChartType.xlLineMarkers; // 插入折線圖

                // 設定數據範圍
                Chart c = myBook.ActiveChart;
                SeriesCollection seriesCollection = c.SeriesCollection();

                Series series1 = seriesCollection.Item(1); // seriesCollection.NewSeries();
                series1.Name = "Target";
                series1.XValues = mySheet.Range["A3", "A14"];
                series1.Values = mySheet.Range["B3", "B14"];

                Series series2 = seriesCollection.NewSeries();
                series2.Name = this.stringyear4;
                series2.XValues = mySheet.Range["A3", "A14"];
                series2.Values = mySheet.Range["E3", "E14"];

                Series series3 = seriesCollection.NewSeries();
                series3.Name = this.stringyear3;
                series3.XValues = mySheet.Range["A3", "A14"];
                series3.Values = mySheet.Range["H3", "H14"];

                Series series4 = seriesCollection.NewSeries();
                series4.Name = this.stringyear2;
                series4.XValues = mySheet.Range["A3", "A14"];
                series4.Values = mySheet.Range["K3", "K14"];

                mySheet.Shapes.Item("Chart 1").Width = 690;   // 調整圖表寬度
                mySheet.Shapes.Item("Chart 1").Height = 300;  // 調整圖表高度
                mySheet.Shapes.Item("Chart 1").Top = 280;      // 調整圖表在分頁中的高度(上邊距) 位置
                mySheet.Shapes.Item("Chart 1").Left = 3;    // 調整圖表在分頁中的左右(左邊距) 位置

                // myBook.ActiveChart.PlotArea.Width =2000;   //調整圖表寬度
                // myBook.ActiveChart.PlotArea.Height =1500;  //調整圖表高度
                // myBook.ActiveChart.PlotArea.Top = 2000;      //調整圖表在分頁中的高度(上邊距) 位置
                // myBook.ActiveChart.PlotArea.Left =0;    //調整圖表在分頁中的左右(左邊距) 位置

                // 設定 繪圖區 的 背景顏色
                myBook.ActiveChart.PlotArea.Interior.Color = ColorTranslator.ToOle(Color.LightGray);

                // 設定 繪圖區 的 邊框線條樣式
                // myBook.ActiveChart.PlotArea.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                // 設定 繪圖區 的 寬度
                myBook.ActiveChart.PlotArea.Width = 670;

                // 設定 繪圖區 的 高度
                myBook.ActiveChart.PlotArea.Height = 250;

                // 設定 繪圖區 在 圖表中的 高低位置(上邊距)
                myBook.ActiveChart.PlotArea.Top = 5;

                // 設定 繪圖區 在 圖表中的 左右位置(左邊距)
                myBook.ActiveChart.PlotArea.Left = 2;

                // 設定 繪圖區 的 x軸名稱下方 顯示y軸的 數據資料
                myBook.ActiveChart.HasDataTable = false;

                // 設定 圖表的 背景顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Interior.ColorIndex = 10;

                // 設定 圖表的 背景顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Interior.Color = ColorTranslator.ToOle(Color.White);

                // 設定 圖表的 邊框顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Border.ColorIndex = 10;

                // 設定 圖表的 邊框顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Border.Color = ColorTranslator.ToOle(Color.Black);

                // 設定 圖表的 邊框樣式
                myBook.ActiveChart.ChartArea.Border.LineStyle = XlLineStyle.xlContinuous;

                // 設置Legend圖例
                myBook.ActiveChart.Legend.Top = 100;           // 設定 圖例 的 上邊距
                myBook.ActiveChart.Legend.Left = 200;        // 設定 圖例 的 左邊距

                // 設定 圖例 的 背景色彩
                myBook.ActiveChart.Legend.Interior.Color = ColorTranslator.ToOle(Color.White);
                myBook.ActiveChart.Legend.Width = 55;        // 設定 圖例 的 寬度
                myBook.ActiveChart.Legend.Height = 20;       // 設定 圖例 的 高度
                myBook.ActiveChart.Legend.Font.Size = 11;    // 設定 圖例 的 字體大小
                myBook.ActiveChart.Legend.Font.Bold = true;  // 設定 圖例 的 字體樣式=粗體
                myBook.ActiveChart.Legend.Font.Name = "Arial"; // 設定 圖例 的 字體字型=細明體
                myBook.ActiveChart.Legend.Position = XlLegendPosition.xlLegendPositionBottom; // 設訂 圖例 的 位置靠上
                myBook.ActiveChart.Legend.Border.LineStyle = XlLineStyle.xlContinuous; // 設定 圖例 的 邊框線條

                // 設定 圖表 x 軸 內容
                // 宣告
                Axis xAxis = (Axis)myBook.ActiveChart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);

                // 設定 圖表 x軸 橫向線條 線條樣式
                // xAxis.MajorGridlines.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDashDotDot;
                // 設定 圖表 x軸 橫向線條顏色__方法1
                xAxis.MajorGridlines.Border.ColorIndex = 8;

                // 設定 圖表 x軸 橫向線條顏色__方法2
                xAxis.MajorGridlines.Border.Color = ColorTranslator.ToOle(Color.Black);
                xAxis.HasTitle = false;  // 設定 x軸 座標軸標題 = false(不顯示)，不打就是不顯示

                // yAxis.MinimumScale = 0;//設定 x軸 數值 最小值
                // xAxis.MaximumScale = 10;//設定 y軸 數值 最大值
                xAxis.TickLabels.Font.Size = 14; // 設定 x軸 字體大小

                // 設定 圖表 y軸 內容
                Axis yAxis = (Axis)myBook.ActiveChart.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary);

                // yAxis.TickLabels.Font.Name = "標楷體"; //設定 y軸 字體字型=標楷體
                yAxis.TickLabels.Font.Size = 14; // 設定 y軸 字體大小

                // yAxis.MinimumScale = 0;
                // yAxis.MaximumScale = 10;
            }
            catch (Exception e)
            {
                this.ShowErr(e);
            }
            finally
            {
            }
            #endregion
        }

        private void Addfactory(Worksheet mySheet, int rowNo, int columnNo)
        {
            #region By Factory

            mySheet.Cells[2, 3] = "Claimed";
            mySheet.Cells[2, 4] = "Shipped";
            mySheet.Cells[2, 5] = "adiComp";
            mySheet.Cells[2, 6] = "Claimed";
            mySheet.Cells[2, 7] = "Shipped";
            mySheet.Cells[2, 8] = "adiComp";
            mySheet.Cells[2, 9] = "Claimed";
            mySheet.Cells[2, 10] = "Shipped";
            mySheet.Cells[2, 11] = "adiComp";

            Range formatRange;

            formatRange = mySheet.get_Range("B3", "B15");
            formatRange.NumberFormat = "0.0000%";
            formatRange = mySheet.get_Range("E3", "E15");
            formatRange.NumberFormat = "0.0000%";
            formatRange = mySheet.get_Range("H3", "H15");
            formatRange.NumberFormat = "0.0000%";
            formatRange = mySheet.get_Range("K3", "K15");
            formatRange.NumberFormat = "0.0000%";

            formatRange = mySheet.get_Range("D3", "D15");
            formatRange.NumberFormat = "#,##0";
            formatRange = mySheet.get_Range("G3", "G15");
            formatRange.NumberFormat = "#,##0";
            formatRange = mySheet.get_Range("J3", "J15");
            formatRange.NumberFormat = "#,##0";

            _Application myExcel = null;
            _Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            try
            {
                ((_Worksheet)mySheet).Activate();

                // 在工作簿 新增一張 統計圖表，單獨放在一個分頁裡面
                Range range = mySheet.get_Range("a1", "a14"); // .Select();
                range.Select();
                myBook.Charts.Add(Type.Missing, Type.Missing, 1, Type.Missing);

                // 選擇 統計圖表 的 圖表種類
                myBook.ActiveChart.Location(XlChartLocation.xlLocationAsObject, mySheet.Name);
                myBook.ActiveChart.ChartType = XlChartType.xlLineMarkers; // 插入折線圖

                // 設定數據範圍
                Chart c = myBook.ActiveChart;
                SeriesCollection seriesCollection = c.SeriesCollection();

                Series series1 = seriesCollection.Item(1); // seriesCollection.NewSeries();
                series1.Name = "Target";
                series1.XValues = mySheet.Range["A3", "A14"];
                series1.Values = mySheet.Range["B3", "B14"];

                Series series2 = seriesCollection.NewSeries();
                series2.Name = this.stringyear4;
                series2.XValues = mySheet.Range["A3", "A14"];
                series2.Values = mySheet.Range["E3", "E14"];

                Series series3 = seriesCollection.NewSeries();
                series3.Name = this.stringyear3;
                series3.XValues = mySheet.Range["A3", "A14"];
                series3.Values = mySheet.Range["H3", "H14"];

                Series series4 = seriesCollection.NewSeries();
                series4.Name = this.stringyear2;
                series4.XValues = mySheet.Range["A3", "A14"];
                series4.Values = mySheet.Range["K3", "K14"];

                mySheet.Shapes.Item("Chart 1").Width = 690;   // 調整圖表寬度
                mySheet.Shapes.Item("Chart 1").Height = 300;  // 調整圖表高度
                mySheet.Shapes.Item("Chart 1").Top = 280;      // 調整圖表在分頁中的高度(上邊距) 位置
                mySheet.Shapes.Item("Chart 1").Left = 3;    // 調整圖表在分頁中的左右(左邊距) 位置

                // 設定 繪圖區 的 背景顏色
                myBook.ActiveChart.PlotArea.Interior.Color = ColorTranslator.ToOle(Color.LightGray);

                // 設定 繪圖區 的 邊框線條樣式
                // myBook.ActiveChart.PlotArea.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                // 設定 繪圖區 的 寬度
                myBook.ActiveChart.PlotArea.Width = 670;

                // 設定 繪圖區 的 高度
                myBook.ActiveChart.PlotArea.Height = 250;

                // 設定 繪圖區 在 圖表中的 高低位置(上邊距)
                myBook.ActiveChart.PlotArea.Top = 5;

                // 設定 繪圖區 在 圖表中的 左右位置(左邊距)
                myBook.ActiveChart.PlotArea.Left = 2;

                // 設定 繪圖區 的 x軸名稱下方 顯示y軸的 數據資料
                myBook.ActiveChart.HasDataTable = false;

                // 設定 圖表的 背景顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Interior.ColorIndex = 10;

                // 設定 圖表的 背景顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Interior.Color = ColorTranslator.ToOle(Color.White);

                // 設定 圖表的 邊框顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Border.ColorIndex = 10;

                // 設定 圖表的 邊框顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Border.Color = ColorTranslator.ToOle(Color.Black);

                // 設定 圖表的 邊框樣式
                myBook.ActiveChart.ChartArea.Border.LineStyle = XlLineStyle.xlContinuous;

                // 設置Legend圖例
                myBook.ActiveChart.Legend.Top = 100;           // 設定 圖例 的 上邊距
                myBook.ActiveChart.Legend.Left = 200;        // 設定 圖例 的 左邊距

                // 設定 圖例 的 背景色彩
                myBook.ActiveChart.Legend.Interior.Color = ColorTranslator.ToOle(Color.White);
                myBook.ActiveChart.Legend.Width = 55;        // 設定 圖例 的 寬度
                myBook.ActiveChart.Legend.Height = 20;       // 設定 圖例 的 高度
                myBook.ActiveChart.Legend.Font.Size = 11;    // 設定 圖例 的 字體大小
                myBook.ActiveChart.Legend.Font.Bold = true;  // 設定 圖例 的 字體樣式=粗體
                myBook.ActiveChart.Legend.Font.Name = "Arial"; // 設定 圖例 的 字體字型=細明體
                myBook.ActiveChart.Legend.Position = XlLegendPosition.xlLegendPositionBottom; // 設訂 圖例 的 位置靠上
                myBook.ActiveChart.Legend.Border.LineStyle = XlLineStyle.xlContinuous; // 設定 圖例 的 邊框線條

                // 設定 圖表 x 軸 內容
                // 宣告
                Axis xAxis = (Axis)myBook.ActiveChart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);

                // 設定 圖表 x軸 橫向線條 線條樣式
                // xAxis.MajorGridlines.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDashDotDot;
                // 設定 圖表 x軸 橫向線條顏色__方法1
                xAxis.MajorGridlines.Border.ColorIndex = 8;

                // 設定 圖表 x軸 橫向線條顏色__方法2
                xAxis.MajorGridlines.Border.Color = ColorTranslator.ToOle(Color.Black);
                xAxis.HasTitle = false;  // 設定 x軸 座標軸標題 = false(不顯示)，不打就是不顯示

                // xAxis.MinimumScale = 0;  //設定 x軸 數值 最小值
                // xAxis.MaximumScale = 0.9;  //設定 x軸 數值 最大值
                xAxis.TickLabels.Font.Name = "Arial"; // 設定 x軸 字體字型=標楷體
                xAxis.TickLabels.Font.Size = 14;       // 設定 x軸 字體大小

                // 設定 圖表 y軸 內容
                Axis yAxis = (Axis)myBook.ActiveChart.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary);
                yAxis.TickLabels.Font.Name = "Arial"; // 設定 y軸 字體字型=標楷體
                yAxis.TickLabels.Font.Size = 14; // 設定 y軸 字體大小
            }
            catch (Exception e)
            {
                this.ShowErr(e);

                // myExcel.Visible = true;
            }
            finally
            {
                ////把執行的Excel資源釋放
                // System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
                // myExcel = null;
                // myBook = null;
                // mySheet = null;
            }
            #endregion
        }

        private void CopySheet(Worksheet mySheet, int rowNo, int columnNo)
        {
            _Application myExcel = null;
            _Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            Worksheet aftersheet = mySheet;
            int idx = 0;
            foreach (var item in this.dicFTY)
            {
                mySheet.Cells[2, 1] = " ";
                idx += 1;

                mySheet.Cells[2, idx * 3] = "Claimed";
                mySheet.Cells[2, (idx * 3) + 1] = "Shipped";
                mySheet.Cells[2, (idx * 3) + 2] = "Adicomp";

                aftersheet = myExcel.Sheets.Add(After: aftersheet);
                aftersheet.Cells[1, 1] = "##ftyDetail" + item.Key;
                aftersheet.Cells[2, 1] = "##addfactory";
                aftersheet.Cells[3, 1] = "##ftySheetName" + item.Key;
            }
        }
    }
}
