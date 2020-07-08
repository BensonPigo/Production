using Ict;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R41 : Win.Tems.PrintForm
    {
        public R41(ToolStripMenuItem menuitem)
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

            this.comboBrand.SelectedIndex = 0;
            this.print.Enabled = false;
        }

        string Brand;
        string Year;
        DualResult result;

        System.Data.DataTable[] alldt;
        System.Data.DataTable dym = null;
        System.Data.DataTable dy = null;
        string defect1 = string.Empty;
        string defect2 = string.Empty;
        System.Data.DataTable[] dt;
        System.Data.DataTable[] dts;

        System.Data.DataTable[] allda;
        System.Data.DataTable sym = null;
        System.Data.DataTable sy = null;
        string style1 = string.Empty;
        string style2 = string.Empty;
        System.Data.DataTable[] da;
        System.Data.DataTable[] das;

        System.Data.DataTable[] alldatb;
        System.Data.DataTable cym = null;
        System.Data.DataTable cy = null;
        string country1 = string.Empty;
        string country2 = string.Empty;
        System.Data.DataTable[] datb;
        System.Data.DataTable[] datbs;

        System.Data.DataTable[] alldatatable;
        System.Data.DataTable fm = null;
        System.Data.DataTable datatab;
        System.Data.DataTable dt_All;

        protected override bool ValidateInput()
        {
            this.Brand = this.comboBrand.Text.ToString();
            this.Year = this.comboYear.Text.ToString();
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            string gb = string.Empty;
            string gb1 = string.Empty;
            string gb2 = string.Empty;
            string gb3 = string.Empty;
            string ob = string.Empty;
            string ob1 = string.Empty;
            string sqlWh1 = string.Empty;
            List<string> sqlWheres = new List<string>();

            if (this.Brand != string.Empty)
            {
                sqlWheres.Add("b.BrandID ='" + this.Brand + "'");
            }

            if (this.Year != string.Empty)
            {
                string Year2 = (Convert.ToInt32(this.Year) - 2).ToString();
                sqlWheres.Add("year(a.StartDate)" + "between" + "'" + Year2 + "' " + "and" + "'" + this.Year + "'");
            }

            List<string> sqlWh = new List<string>();

            if (this.Brand != string.Empty)
            {
                sqlWh.Add("b.BrandID ='" + this.Brand + "'");
            }

            if (this.Year != string.Empty)
            {
                sqlWh.Add("year(a.StartDate)='" + this.Year + "'");
            }

            gb = "group by A.StartDate, B.DefectMainID, B.DefectSubID,C.SubName";
            ob = "order by Y,M,QTY DESC";
            gb1 = "group by A.StartDate,b.styleid";
            gb2 = "group by A.StartDate,b.SalesName";
            gb3 = "group by b.FactoryID, A.StartDate, B.DefectMainID, c.Name, B.DefectSubID,d.SubName";
            ob1 = "order by b.FactoryID,Y,M,QTY DESC";
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where a.Junk=0 AND " + sqlWhere;
            }

            sqlWh1 = string.Join(" and ", sqlWh);
            if (!sqlWh1.Empty())
            {
                sqlWh1 = " where A.Junk=0 AND " + sqlWh1;
            }

            #region Defect

            string sqlcmd = string.Format(
                @"create table #dRanges(starts int , ends int, name varchar(20))
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

                                            select YEAR(a.StartDate)[y]
                                                  ,format(a.StartDate,'MMMMM')[m]
                                                  ,[Defect]=C.SubName
                                                  ,[Qty]=SUM(B.QTY)
                                                  ,[Amount]=SUM(B.ValueinUSD)
                                                  ,row_number() over (partition by YEAR(A.StartDate),MONTH(A.StartDate) 
					                               order by SUM(B.Qty) desc,SUM(B.ValueinUSD) DESC) as rnk
                                            into #temp
                                            from dbo.ADIDASComplain a WITH (NOLOCK) 
                                            inner join dbo.ADIDASComplain_Detail b WITH (NOLOCK) on a.id=b.id
                                            left join dbo.ADIDASComplainDefect_Detail c WITH (NOLOCK) on c.id=b.DefectMainID AND C.SubID=B.DefectSubID
" + " " + sqlWhere + Environment.NewLine + gb + " " +

                                         @"declare @d date = '{0}' --getdate()
											 declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
											 declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
											 declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))
                                            select  @y1 as cd,@y2 as de ,@y3 as fg,name as mon  into #temp1
											from  (select DISTINCT dRanges.name,dRanges.starts from #temp 
				                            left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends
				                            GROUP BY m ,dRanges.starts,dRanges.name)as s  
											ORDER BY starts

											select  ye.cd,am.name from (
											select distinct cd from #temp1
											union all
											select distinct de from #temp1
											union all 
											select distinct fg from #temp1) as ye
											outer apply(select top 12 mon,name from #temp1 left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends group by mon,starts,name)  as am
											
                                            select distinct yy.cd as yy from(
											select  cd from #temp1
											union all
											select de from #temp1
											union all
											select fg from #temp1) as yy

                                            select * from #temp", this.Year + "-01-01");
            SqlConnection conn;
            this.result = DBProxy.Current.OpenConnection(string.Empty, out conn);
            if (!this.result)
            {
                return this.result;
            }

            // List<SqlParameter> plis = new List<SqlParameter>();
            // plis.Add(new SqlParameter("@date", Year + "-01-01"));
            this.result = DBProxy.Current.SelectByConn(conn, sqlcmd, out this.alldt);
            if (!this.result)
            {
                return this.result;
            }

            if (this.alldt[0].Rows.Count <= 0)
            {
                return new DualResult(false, "No datas");
            }

            this.dym = this.alldt[0];
            for (int i = 0; i < this.dym.Rows.Count; i++)
            {
                string dyear = this.dym.Rows[i]["cd"].ToString();
                string dmonth = this.dym.Rows[i]["name"].ToString();
                string a = string.Format("SELECT top 10 Defect,Qty, Amount, rnk FROM #temp WHERE y = '{0}' and m  ='{1}'", dyear, dmonth);
                this.defect1 += a + ' ' + Environment.NewLine;
            }

            this.result = DBProxy.Current.SelectByConn(conn, this.defect1, out this.dt);
            if (!this.result)
            {
                return this.result;
            }

            this.dy = this.alldt[1];
            for (int i = 0; i < this.dy.Rows.Count; i++)
            {
                string dyear1 = this.dy.Rows[i]["yy"].ToString();
                string b = string.Format(
                    @"
select top 10
    [Defect] = C.SubName
    ,[Qty] = SUM(B.QTY)
    ,[Amount] = SUM(B.ValueinUSD)
	 ,row_number() over (order by SUM(Qty) desc) as rnk 
from dbo.ADIDASComplain a WITH (NOLOCK) 
inner join dbo.ADIDASComplain_Detail b WITH (NOLOCK) on a.id=b.id
left join dbo.ADIDASComplainDefect_Detail c WITH (NOLOCK) on c.id=b.DefectMainID AND C.SubID=B.DefectSubID 
where a.Junk=0
AND b.BrandID ='{1}' 
AND year(a.StartDate) = '{0}'
group by B.DefectMainID, B.DefectSubID,C.SubName 
order by  SUM(B.Qty) desc,SUM(B.ValueinUSD) desc",
                    dyear1, this.Brand);
                this.defect2 += b + ' ' + Environment.NewLine;
            }

            this.result = DBProxy.Current.SelectByConn(conn, this.defect2, out this.dts);
            if (!this.result)
            {
                return this.result;
            }

            #endregion

            #region Style

            string scmd = string.Format(
                @"create table #dRanges(starts int , ends int, name varchar(20))
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

                                           select YEAR(a.StartDate)[y]
                                          ,format(a.StartDate,'MMMMM')[m]
                                          ,[Style]=b.StyleID
                                          ,[Qty]=SUM(B.QTY)
                                          ,[Amount]=SUM(B.ValueinUSD)
                                          ,row_number() over (partition by YEAR(A.StartDate),MONTH(A.StartDate) 
                                           order by SUM(B.Qty) desc,SUM(B.ValueinUSD) desc) as rnk
                                           into #temp
                                           from dbo.ADIDASComplain a WITH (NOLOCK) 
                                           inner join dbo.ADIDASComplain_Detail b WITH (NOLOCK) on a.id=b.id" + " " + sqlWhere + " " + gb1 + " " + ob + " " +

                                         @"declare @d date = '{0}' --getdate()
											 declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
											 declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
											 declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))
                                             select  @y1 as cd,@y2 as de ,@y3 as fg,name as mon  into #temp1
											 from  (select DISTINCT dRanges.name,dRanges.starts from #temp 
				                             left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends
				                             GROUP BY m ,dRanges.starts,dRanges.name)as s  
											 ORDER BY starts

											select  ye.cd,am.name from (
											select distinct cd from #temp1
											union all
											select distinct de from #temp1
											union all 
											select distinct fg from #temp1) as ye
											outer apply(select top 12 mon,name from #temp1 left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends group by mon,starts,name)  as am
											
                                            select distinct yy.cd as yy from(
											select  cd from #temp1
											union all
											select de from #temp1
											union all
											select fg from #temp1) as yy

                                            select * from #temp", this.Year + "-01-01");
            SqlConnection con;
            this.result = DBProxy.Current.OpenConnection(string.Empty, out con);
            if (!this.result)
            {
                return this.result;
            }

            this.result = DBProxy.Current.SelectByConn(con, scmd, out this.allda);
            if (!this.result)
            {
                return this.result;
            }

            this.sym = this.allda[0];
            for (int i = 0; i < this.sym.Rows.Count; i++)
            {
                string syear = this.sym.Rows[i]["cd"].ToString();
                string smonth = this.sym.Rows[i]["name"].ToString();
                string a = string.Format("SELECT top 10 Style,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' and m  ='{1}' GROUP BY style ORDER BY SUM(Qty) DESC,SUM(Amount) DESC ", syear, smonth);
                this.style1 += a + ' ' + Environment.NewLine;
            }

            this.result = DBProxy.Current.SelectByConn(con, this.style1, out this.da);
            if (!this.result)
            {
                return this.result;
            }

            this.sy = this.allda[1];
            for (int i = 0; i < this.sy.Rows.Count; i++)
            {
                string syear1 = this.sy.Rows[i]["yy"].ToString();
                string b = string.Format("SELECT top 10 Style,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' GROUP BY Style ORDER BY SUM(Qty) DESC,SUM(Amount) DESC", syear1);
                this.style2 += b + ' ' + Environment.NewLine;
            }

            this.result = DBProxy.Current.SelectByConn(con, this.style2, out this.das);
            if (!this.result)
            {
                return this.result;
            }
            #endregion

            #region Country

            string sqcmd = string.Format(
                @"create table #dRanges(starts int , ends int, name varchar(20))
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

                                           select YEAR(a.StartDate)[y]
                                          ,format(a.StartDate,'MMMMM')[m]
                                          ,[Country]=b.SalesName
                                          ,[Qty]=SUM(B.QTY)
                                          ,[Amount]=SUM(B.ValueinUSD)
                                          ,row_number() over (partition by YEAR(A.StartDate),MONTH(A.StartDate) 
                                           order by SUM(B.Qty) desc,SUM(B.ValueinUSD) desc) as rnk
                                           into #temp
                                           from dbo.ADIDASComplain a WITH (NOLOCK) 
                                           inner join dbo.ADIDASComplain_Detail b WITH (NOLOCK) on a.id=b.id" + " " + sqlWhere + " " + gb2 + " " + ob + " " +

                                         @"declare @d date = '{0}' --getdate()
											 declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
											 declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
											 declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))
                                             select  @y1 as cd,@y2 as de ,@y3 as fg,name as mon  into #temp1
											 from  (select DISTINCT dRanges.name,dRanges.starts from #temp 
				                             left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends
				                             GROUP BY m ,dRanges.starts,dRanges.name)as s  
											 ORDER BY starts

											select  ye.cd,am.name from (
											select distinct cd from #temp1
											union all
											select distinct de from #temp1
											union all 
											select distinct fg from #temp1) as ye
											outer apply(select top 12 mon,name from #temp1 left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends group by mon,starts,name)  as am
											
                                            select distinct yy.cd as yy from(
											select  cd from #temp1
											union all
											select de from #temp1
											union all
											select fg from #temp1) as yy

                                            select * from #temp", this.Year + "-01-01");
            SqlConnection connection;
            this.result = DBProxy.Current.OpenConnection(string.Empty, out connection);
            if (!this.result)
            {
                return this.result;
            }

            this.result = DBProxy.Current.SelectByConn(connection, sqcmd, out this.alldatb);
            if (!this.result)
            {
                return this.result;
            }

            this.cym = this.alldatb[0];
            for (int i = 0; i < this.cym.Rows.Count; i++)
            {
                string cyear = this.cym.Rows[i]["cd"].ToString();
                string cmonth = this.cym.Rows[i]["name"].ToString();
                string a = string.Format("SELECT top 10 Country,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' and m  ='{1}' GROUP BY Country ORDER BY SUM(Qty) DESC,SUM(Amount) DESC", cyear, cmonth);
                this.country1 += a + ' ' + Environment.NewLine;
            }

            this.result = DBProxy.Current.SelectByConn(connection, this.country1, out this.datb);
            if (!this.result)
            {
                return this.result;
            }

            this.cy = this.alldatb[1];
            for (int i = 0; i < this.cy.Rows.Count; i++)
            {
                string cyear1 = this.cy.Rows[i]["yy"].ToString();
                string b = string.Format("SELECT top 10 Country,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' GROUP BY Country ORDER BY SUM(Qty) DESC,SUM(Amount) DESC", cyear1);
                this.country2 += b + ' ' + Environment.NewLine;
            }

            this.result = DBProxy.Current.SelectByConn(connection, this.country2, out this.datbs);
            if (!this.result)
            {
                return this.result;
            }

            #endregion

            #region Factory
            this.dicFTY.Clear();
            string scd = string.Format(@"create table #dRanges(starts int , ends int, name varchar(20))
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

                                           SELECT b.FactoryID, 
                                                 YEAR(A.StartDate)as Y, 
                                                 format(A.StartDate,'MMMMM')as M,
                                                 B.DefectMainID,
                                                 c.Name, 
                                                 B.DefectSubID,
                                                 d.SubName,
                                                 [Description]=iif(d.subname='',c.name,d.subname),
												 [Defect_Code]=b.DefectMainID+b.DefectSubID,
                                                 SUM(B.Qty) AS Qty, 
                                                 SUM(B.ValueinUSD) AS Amount 
                                                ,row_number() over (partition by YEAR(A.StartDate),MONTH(A.StartDate) 
					                                                order by SUM(B.Qty) desc) as rnk
                                                into #temp
                                                FROM dbo.ADIDASComplain A WITH (NOLOCK) 
                                                INNER JOIN dbo.ADIDASComplain_Detail B WITH (NOLOCK) on b.ID = A.ID
                                                left join (dbo.ADIDASComplainDefect c WITH (NOLOCK) inner join dbo.ADIDASComplainDefect_Detail d WITH (NOLOCK) on c.id = d.ID)
		                                                    on c.ID = b.DefectMainID  and d.SubID = b.DefectSubID" + " " + sqlWh1 + " " + gb3 + " " + ob1 + " " +

                                                @"select distinct [yn]=name,[yt]=DefectMainID into #title from #temp

											    select distinct [Description]=iif(t.subname='',yn,t.subname),[Defect_Code]=t.DefectMainID+t.DefectSubID,yn,yt,t.SubName into #defect from #title
											    inner join #temp as t on DefectMainID=yt
											    order by yt,SubName

                                                select * into #defect1 from(
												select Description,Defect_Code from #defect
											    union
											    select yn,yt from #defect) as fe
											    order by fe.Defect_Code

                                                select name as mon into #f from (select DISTINCT dRanges.name,dRanges.starts from #temp T 
												left join #dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends
												GROUP BY m ,dRanges.starts,dRanges.name)as s  ORDER BY starts
                                                 
                                              
                                                select distinct FactoryID as fty from #temp

                                                select * from #f 

												select * from #defect1");
            SqlConnection connction;
            this.result = DBProxy.Current.OpenConnection(string.Empty, out connction);
            if (!this.result)
            {
                return this.result;
            }

            this.result = DBProxy.Current.SelectByConn(connction, scd, out this.alldatatable);
            if (!this.result)
            {
                return this.result;
            }

            this.fm = this.alldatatable[0];
            for (int i = 0; i < this.fm.Rows.Count; i++)
            {
                string factory = this.fm.Rows[i]["fty"].ToString();
                #region 撈完全準備好的Datatable By factory做不同Sheet
                string a = string.Format(
                    @"
--準備主要資料
SELECT
	c.name,
    [Description]=iif(d.subname='',c.name,d.subname),
	[Defect_Code]=concat(b.DefectMainID,b.DefectSubID),
	DefectMainID,
    [M] = format(A.StartDate,'MMMMM'),
    [Qty] = SUM(B.Qty),
	[Amount] = convert(numeric(10,4),SUM(B.ValueinUSD)) 
into #tmp
FROM dbo.ADIDASComplain A WITH (NOLOCK)
INNER JOIN dbo.ADIDASComplain_Detail B WITH (NOLOCK) on b.ID = A.ID
left join (dbo.ADIDASComplainDefect c WITH (NOLOCK) inner join dbo.ADIDASComplainDefect_Detail d WITH (NOLOCK) on c.id = d.ID)
on c.ID = b.DefectMainID  and d.SubID = b.DefectSubID
where A.Junk = 0 AND b.BrandID ='{0}' and year(a.StartDate)='{1}' and FactoryID = '{2}'
group by b.FactoryID, A.StartDate, B.DefectMainID, c.Name, B.DefectSubID,d.SubName
order by B.DefectMainID,[Defect_Code]

--↓補上缺的Main,已經有的則不補上
select * into #t_all
from
(
	select distinct [Description] = name,[Defect_Code]=DefectMainID,DefectMainID,[M]=null,[Qty]=null,[Amount]=null
	from #tmp
	where DefectMainID not in (select distinct [Defect_Code] from #tmp)
	union all
	select [Description],[Defect_Code],DefectMainID,[M],[Qty],[Amount]
	from #tmp
)a
--↓準備每個月qty / Amount 
select * 
into #t_qty 
from #t_all
PIVOT(SUM(Qty) FOR [M] IN ([January],[February],[March],[April],[May],[June],[July],[August],[September],[October],[November],[December])) AS pt
select * 
into #t_Amount 
from #t_all
PIVOT(sum([Amount]) FOR [M] IN ([January],[February],[March],[April],[May],[June],[July],[August],[September],[October],[November],[December])) AS pt
--把以上準備的table先各月分sum起來，且加上最後一組YTD總和，按照報表要求排列
;WITH Amount as(
	select a.Description,a.Defect_Code,
		[January_A] = sum(a.[January]),         [February_A] = sum(a.[February]),
        [March_A] = sum(a.[March]),             [April_A] = sum(a.[April]),
		[May_A] = sum(a.[May]),                 [June_A] = sum(a.[June]),
        [July_A] = sum(a.[July]),               [August_A] = sum(a.[August]),
		[September_A] = sum(a.[September]),     [October_A] = sum(a.[October]),
        [November_A] = sum(a.[November]),       [December_A] = sum(a.[December]),
		[YTD_A] = iif(DefectMainID = Defect_Code,ta.Amount,null)
	from #t_Amount a
	outer apply(
        select Amount 
        from(select DefectMainID,sum(Amount) Amount 
        from #t_all group by DefectMainID) t where t.DefectMainID = a.DefectMainID 
    )ta
	group by a.Description,a.Defect_Code,iif(DefectMainID = Defect_Code,ta.Amount,null)
),qty as(
	select q.Description,q.Defect_Code,
		[January_Q] = sum(q.[January]),     [February_Q] = sum(q.[February]),
        [March_Q] = sum(q.[March]),         [April_Q] = sum(q.[April]),
		[May_Q] = sum(q.[May]),             [June_Q] = sum(q.[June]),
        [July_Q] = sum(q.[July]),           [August_Q] = sum(q.[August]),
		[September_Q] = sum(q.[September]), [October_Q] = sum(q.[October]),
        [November_Q] = sum(q.[November]),   [December_Q] = sum(q.[December]),
		[YTD_Q] = iif(DefectMainID=Defect_Code,tq.qty,null)
		from #t_qty  q
	outer apply(
        select qty 
        from(select DefectMainID,sum(qty) qty from #t_all group by DefectMainID) t 
        where t.DefectMainID = q.DefectMainID 
    )tq
	group by q.Description,q.Defect_Code,iif(DefectMainID=Defect_Code,tq.qty,null)
),TQTY_by_monthly as(
	select distinct 
		[January_TQ] = sum(q.[January])over(),      [February_TQ] = sum(q.[February])over(),
        [March_TQ] = sum(q.[March])over(),          [April_TQ] = sum(q.[April])over(),
		[May_TQ] = sum(q.[May])over(),              [June_TQ] = sum(q.[June])over(),
        [July_TQ] = sum(q.[July])over(),            [August_TQ] = sum(q.[August])over(),
		[September_TQ] = sum(q.[September])over(),  [October_TQ] = sum(q.[October])over(),
        [November_TQ] = sum(q.[November])over(),    [December_TQ] = sum(q.[December])over(),
		[YTD_TQ] = tq.qty
	from #t_qty  q
	outer apply(select sum(qty) qty from #t_all)tq
)
select q.Description,q.Defect_Code,
	January_Q,	[January_P] = January_Q/t.January_TQ,		[January_A],
	February_Q,	[February_P] = February_Q/t.February_TQ,	[February_A],
	March_Q,	[March_P] = March_Q/t.March_TQ,				[March_A],
	April_Q,	[April_P] = April_Q/t.April_TQ,				[April_A],
	May_Q,		[May_P] = May_Q/t.May_TQ,					[May_A],
	June_Q,		[June_P] = June_Q/t.June_TQ,				[June_A],
	July_Q,		[July_P] = July_Q/t.July_TQ,				[July_A],
	August_Q,	[August_P] = August_Q/t.August_TQ,			[August_A],
	September_Q,[September_P] = September_Q/t.September_TQ,	[September_A],
	October_Q,	[October_P] = October_Q/t.October_TQ,		[October_A],
	November_Q,	[November_P] = November_Q/t.November_TQ,	[November_A],
	December_Q,	[December_P] = December_Q/t.December_TQ,	[December_A],
	YTD_Q,		[YTD_P] = YTD_Q/t.YTD_TQ,					[YTD_A]
into #last--這邊不格式化因為下面還需要加總,格式化後會變文字
from qty q inner join Amount a on q.Defect_Code = a.Defect_Code,TQTY_by_monthly t
order by Defect_Code
---↓附上最後一列加總並且格式化
select Description,Defect_Code,
	January_Q,	format(January_P,'P2')January_P,	format(January_A,'C2')January_A,
	February_Q,	format(February_P,'P2')February_P,	format(February_A,'C2')February_A,
	March_Q,	format(March_P,'P2')March_P,		format(March_A,'C2')March_A,
	April_Q,	format(April_P,'P2')April_P,		format(April_A,'C2')April_A,
	May_Q,		format(May_P,'P2')May_P,			format(May_A,'C2')May_A,
	June_Q,		format(June_P,'P2')June_P,			format(June_A,'C2')June_A,
	July_Q,		format(July_P,'P2')July_P,			format(July_A,'C2')July_A,
	August_Q,	format(August_P,'P2')August_P,		format(August_A,'C2')August_A,
	September_Q,format(September_P,'P2')September_P,format(September_A,'C2')September_A,
	October_Q,	format(October_P,'P2')October_P,	format(October_A,'C2')October_A,
	November_Q,	format(November_P,'P2')November_P,	format(November_A,'C2')November_A,
	December_Q,	format(December_P,'P2')December_P,	format(December_A,'C2')December_A,
	YTD_Q,		format(YTD_P,'P2')YTD_P,			format(YTD_A,'C2')YTD_A
from #last
union all
select 'GRAND TOTAL',null,
	sum(January_Q),		format(sum(January_P),'P2'),	format(sum(January_A),'C2'),
	sum(February_Q),	format(sum(February_P),'P2'),	format(sum(February_A),'C2'),
	sum(March_Q),		format(sum(March_P),'P2'),		format(sum(March_A),'C2'),
	sum(April_Q),		format(sum(April_P),'P2'),		format(sum(April_A),'C2'),
	sum(May_Q),			format(sum(May_P),'P2'),		format(sum(May_A),'C2'),
	sum(June_Q),		format(sum(June_P),'P2'),		format(sum(June_A),'C2'),
	sum(July_Q),		format(sum(July_P),'P2'),		format(sum(July_A),'C2'),
	sum(August_Q),		format(sum(August_P),'P2'),		format(sum(August_A),'C2'),
	sum(September_Q),	format(sum(September_P),'P2'),	format(sum(September_A),'C2'),
	sum(October_Q),		format(sum(October_P),'P2'),	format(sum(October_A),'C2'),
	sum(November_Q),	format(sum(November_P),'P2'),	format(sum(November_A),'C2'),
	sum(December_Q),	format(sum(December_P),'P2'),	format(sum(December_A),'C2'),
	sum(YTD_Q),			format(sum(YTD_P),'P2'),		format(sum(YTD_A),'C2')
from #last
drop table #tmp,#t_all,#t_qty,#t_Amount,#last
",
                    this.Brand, this.Year, factory);
                DualResult r = DBProxy.Current.Select(null, a, out this.datatab);
                if (!r)
                {
                    MyUtility.Msg.ErrorBox("SQL command Error!");
                }
                #endregion
                this.dicFTY.Add(factory, this.datatab);
            }

            if (!this.result)
            {
                return this.result;
            }

            if (this.dt_All == null || this.dt_All.Rows.Count == 0)
            {
                this.dt_All = this.datatab;
            }
            else
            {
                this.dt_All.Merge(this.datatab);
            }

            if (!this.result)
            {
                // return result;
                this.ShowErr(this.result);
            }

            #endregion

            return this.result;
        }

        Dictionary<string, System.Data.DataTable> dicFTY = new Dictionary<string, System.Data.DataTable>();

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.Filter_Excel);

            SaveXltReportCls sxc = new SaveXltReportCls("Quality_R41.xltx");

            // SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(dt_All);
            #region Defect

            // for (int i = 0; i < dt.Rows.Count; i++)
            for (int i = 0; i < this.dym.Rows.Count; i++)
            {
                string dyear = this.dym.Rows[i]["cd"].ToString();
                string dmonth = this.dym.Rows[i]["name"].ToString();

                SaveXltReportCls.XltRptTable dxt = new SaveXltReportCls.XltRptTable(this.dt[i]);
                dxt.Columns.RemoveAt(dxt.Columns.Count - 1);
                dxt.BoAddNewRow = false;

                if (dxt.Rows.Count <= 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        dxt.Rows.Add();
                    }
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(dmonth, "1,3");

                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2.Add(dyear, "1,3");

                dxt.LisTitleMerge.Add(dic);
                dxt.LisTitleMerge.Add(dic2);
                dxt.Borders.OutsideVertical = true;
                dxt.ShowHeader = true;
                sxc.DicDatas.Add("##defect" + i, dxt);
            }

            // for (int a = 0; a < dts.Length; a++)
            for (int a = 0; a < this.dy.Rows.Count; a++)
            {
                string dyear1 = this.dy.Rows[a]["yy"].ToString();

                SaveXltReportCls.XltRptTable alldxtb = new SaveXltReportCls.XltRptTable(this.dts[a]);
                alldxtb.Columns.RemoveAt(alldxtb.Columns.Count - 1);
                alldxtb.BoAddNewRow = false;

                if (alldxtb.Rows.Count <= 0)
                {
                    for (int b = 0; b < 10; b++)
                    {
                        alldxtb.Rows.Add();
                    }
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("OVERALL", "1,3");

                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2.Add(dyear1, "1,3");

                alldxtb.LisTitleMerge.Add(dic);
                alldxtb.LisTitleMerge.Add(dic2);
                alldxtb.Borders.AllCellsBorders = false;
                alldxtb.Borders.OutsideVertical = true;
                alldxtb.ShowHeader = true;
                sxc.DicDatas.Add("##adefect" + a, alldxtb);
            }

            SaveXltReportCls.ReplaceAction adr = this.addrow;
            sxc.DicDatas.Add("##addRow", adr);
            #endregion

            #region Style

            // for (int i = 0; i < da.Length; i++)
            for (int i = 0; i < this.sym.Rows.Count; i++)
            {
                string syear = this.sym.Rows[i]["cd"].ToString();
                string smonth = this.sym.Rows[i]["name"].ToString();

                SaveXltReportCls.XltRptTable sxt = new SaveXltReportCls.XltRptTable(this.da[i]);
                sxt.Columns.RemoveAt(sxt.Columns.Count - 1);
                sxt.BoAddNewRow = false;

                if (sxt.Rows.Count <= 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        sxt.Rows.Add();
                    }
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(smonth, "1,3");

                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2.Add(syear, "1,3");

                sxt.LisTitleMerge.Add(dic);
                sxt.LisTitleMerge.Add(dic2);
                sxt.Borders.OutsideVertical = true;
                sxt.ShowHeader = true;
                sxc.DicDatas.Add("##style" + i, sxt);
            }

            // for (int a = 0; a < das.Length; a++)
            for (int a = 0; a < this.sy.Rows.Count; a++)
            {
                string syear1 = this.sy.Rows[a]["yy"].ToString();

                SaveXltReportCls.XltRptTable allsxtb = new SaveXltReportCls.XltRptTable(this.das[a]);
                allsxtb.Columns.RemoveAt(allsxtb.Columns.Count - 1);
                allsxtb.BoAddNewRow = false;

                if (allsxtb.Rows.Count <= 0)
                {
                    for (int b = 0; b < 10; b++)
                    {
                        allsxtb.Rows.Add();
                    }
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("OVERALL", "1,3");

                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2.Add(syear1, "1,3");

                allsxtb.LisTitleMerge.Add(dic);
                allsxtb.LisTitleMerge.Add(dic2);
                allsxtb.ShowHeader = true;
                allsxtb.Borders.OutsideVertical = true;
                sxc.DicDatas.Add("##astyle" + a, allsxtb);
            }
            #endregion

            #region Country

            // for (int i = 0; i < datb.Length; i++)
            for (int i = 0; i < this.cym.Rows.Count; i++)
            {
                string cyear = this.cym.Rows[i]["cd"].ToString();
                string cmonth = this.cym.Rows[i]["name"].ToString();

                SaveXltReportCls.XltRptTable cxt = new SaveXltReportCls.XltRptTable(this.datb[i]);
                cxt.Columns.RemoveAt(cxt.Columns.Count - 1);
                cxt.BoAddNewRow = false;
                if (cxt.Rows.Count <= 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        cxt.Rows.Add();
                    }
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(cmonth, "1,3");

                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2.Add(cyear, "1,3");

                cxt.LisTitleMerge.Add(dic);
                cxt.LisTitleMerge.Add(dic2);

                cxt.Borders.OutsideVertical = true;
                cxt.ShowHeader = true;
                sxc.DicDatas.Add("##country" + i, cxt);
            }

            // for (int a = 0; a < datbs.Length; a++)
            for (int a = 0; a < this.cy.Rows.Count; a++)
            {
                string cyear1 = this.cy.Rows[a]["yy"].ToString();

                SaveXltReportCls.XltRptTable allcxtb = new SaveXltReportCls.XltRptTable(this.datbs[a]);
                allcxtb.Columns.RemoveAt(allcxtb.Columns.Count - 1);
                allcxtb.BoAddNewRow = false;

                if (allcxtb.Rows.Count <= 0)
                {
                    for (int b = 0; b < 10; b++)
                    {
                        allcxtb.Rows.Add();
                    }
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("OVERALL", "1,3");

                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2.Add(cyear1, "1,3");

                allcxtb.LisTitleMerge.Add(dic);
                allcxtb.LisTitleMerge.Add(dic2);
                allcxtb.Borders.OutsideVertical = true;
                allcxtb.ShowHeader = true;
                sxc.DicDatas.Add("##acountry" + a, allcxtb);
            }
            #endregion

            #region Factory
            foreach (var item in this.dicFTY)
            {
                string fty = item.Key;
                SaveXltReportCls.XltRptTable x_All = new SaveXltReportCls.XltRptTable(item.Value);
                sxc.DicDatas.Add("##SUPSheetName" + fty, item.Key);
                sxc.DicDatas.Add("##psd" + fty, x_All);

                // 凍結窗格
                x_All.BoFreezePanes = true;
                x_All.IntFreezeColumn = 2;

                x_All.ShowHeader = false;
                x_All.Borders.AllCellsBorders = true;
                x_All.BoAutoFitColumn = true;
            }

            sxc.VarToSheetName = "##SUPSheetName";

            SaveXltReportCls.ReplaceAction c = this.CopySheet;
            sxc.DicDatas.Add("##copysupsheet", c);

            SaveXltReportCls.ReplaceAction d = this.addfilter;
            sxc.DicDatas.Add("##addfilter", d);

            sxc.Save(Sci.Production.Class.MicrosoftFile.GetName("Quality_R41"));
            #endregion
            this.clearall();
            return true;
        }

        private void clearall()
        {
            this.Brand = null;
            this.Year = null;

            this.alldt = null;
            this.dym = null;
            this.dy = null;
            this.defect1 = string.Empty;
            this.defect2 = string.Empty;
            this.dt = null;
            this.dts = null;

            this.allda = null;
            this.sym = null;
            this.sy = null;
            this.style1 = string.Empty;
            this.style2 = string.Empty;
            this.da = null;
            this.das = null;

            this.alldatb = null;
            this.cym = null;
            this.cy = null;
            this.country1 = string.Empty;
            this.country2 = string.Empty;
            this.datb = null;
            this.datbs = null;

            this.alldatatable = null;
            this.fm = null;
            this.datatab = null;
            this.dt_All = null;
        }

        void CopySheet(Worksheet mySheet, int rowNo, int columnNo)
        {
            _Application myExcel = null;
            _Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            Worksheet aftersheet = mySheet;

            List<Worksheet> lisWK = new List<Worksheet>();
            foreach (var item in this.dicFTY)
            {
                aftersheet = myExcel.Sheets.Add(After: aftersheet);
                aftersheet.Cells[3, 1] = "##psd" + item.Key;
                aftersheet.Cells[5, 1] = "##SUPSheetName" + item.Key;
                aftersheet.Cells[5, 1].Font.Color = Color.Transparent;
                aftersheet.Cells[6, 1] = "##addfilter";

                lisWK.Add(aftersheet);
            }

            foreach (var wkSheet in lisWK)
            {
                for (int idx = 1; idx < 14; idx++)
                {
                    wkSheet.Cells[2, idx * 3] = "Quality";
                    wkSheet.Columns[idx * 3].HorizontalAlignment = XlHAlign.xlHAlignRight;
                    wkSheet.Cells[2, (idx * 3) + 1] = "Percentage";
                    wkSheet.Columns[(idx * 3) + 1].HorizontalAlignment = XlHAlign.xlHAlignRight;
                    wkSheet.Cells[2, (idx * 3) + 2] = "Amount US$";
                    wkSheet.Columns[(idx * 3) + 2].HorizontalAlignment = XlHAlign.xlHAlignRight;
                }

                wkSheet.Cells[1, 1] = string.Empty;
                wkSheet.get_Range("A1:B1").Merge();
                wkSheet.Cells[1, 2] = string.Empty;
                wkSheet.Cells[1, 3] = "January";
                wkSheet.get_Range("C1:E1").Merge();
                wkSheet.Cells[1, 6] = "Feburary";
                wkSheet.get_Range("F1:H1").Merge();
                wkSheet.Cells[1, 9] = "March";
                wkSheet.get_Range("I1:K1").Merge();
                wkSheet.Cells[1, 12] = "April";
                wkSheet.get_Range("L1:N1").Merge();
                wkSheet.Cells[1, 15] = "May";
                wkSheet.get_Range("O1:Q1").Merge();
                wkSheet.Cells[1, 18] = "June";
                wkSheet.get_Range("R1:T1").Merge();
                wkSheet.Cells[1, 21] = "July";
                wkSheet.get_Range("U1:W1").Merge();
                wkSheet.Cells[1, 24] = "August";
                wkSheet.get_Range("X1:Z1").Merge();
                wkSheet.Cells[1, 27] = "September";
                wkSheet.get_Range("AA1:AC1").Merge();
                wkSheet.Cells[1, 30] = "October";
                wkSheet.get_Range("AD1:AF1").Merge();
                wkSheet.Cells[1, 33] = "November";
                wkSheet.get_Range("AG1:AI1").Merge();
                wkSheet.Cells[1, 36] = "December";
                wkSheet.get_Range("AJ1:AL1").Merge();
                wkSheet.Cells[1, 39] = "YTD";
                wkSheet.get_Range("AM1:AO1").Merge();

                wkSheet.Cells[2, 1] = "Description";
                wkSheet.Cells[2, 2] = "Defect Code";

                wkSheet.Cells[1, 3].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 6].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 9].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 12].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 15].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 18].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 21].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 24].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 27].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 30].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 33].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 36].HorizontalAlignment = XlVAlign.xlVAlignCenter;
                wkSheet.Cells[1, 39].HorizontalAlignment = XlVAlign.xlVAlignCenter;
            }

            // mySheet.Delete();
        }

        void addrow(Worksheet mySheet, int rowNo, int columnNo)
        {
            Range fRow1 = (Range)mySheet.Rows[5];
            Range fRow2 = (Range)mySheet.Rows[6];
            Range fRow3 = (Range)mySheet.Rows[7];
            Range fRow4 = (Range)mySheet.Rows[8];
            Range fRow5 = (Range)mySheet.Rows[9];
            Range fRow6 = (Range)mySheet.Rows[10];
            Range fRow7 = (Range)mySheet.Rows[11];
            Range fRow8 = (Range)mySheet.Rows[12];
            Range fRow9 = (Range)mySheet.Rows[13];
            Range fRow10 = (Range)mySheet.Rows[20];
            Range fRow11 = (Range)mySheet.Rows[21];
            Range fRow12 = (Range)mySheet.Rows[22];
            Range fRow13 = (Range)mySheet.Rows[23];
            Range fRow14 = (Range)mySheet.Rows[24];
            Range fRow15 = (Range)mySheet.Rows[25];
            Range fRow16 = (Range)mySheet.Rows[26];
            Range fRow17 = (Range)mySheet.Rows[27];
            Range fRow18 = (Range)mySheet.Rows[28];
            Range fRow19 = (Range)mySheet.Rows[35];
            Range fRow20 = (Range)mySheet.Rows[36];
            Range fRow21 = (Range)mySheet.Rows[37];
            Range fRow22 = (Range)mySheet.Rows[38];
            Range fRow23 = (Range)mySheet.Rows[39];
            Range fRow24 = (Range)mySheet.Rows[40];
            Range fRow25 = (Range)mySheet.Rows[41];
            Range fRow26 = (Range)mySheet.Rows[42];
            Range fRow27 = (Range)mySheet.Rows[43];

            fRow1.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow2.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow3.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow4.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow5.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow6.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow7.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow8.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow9.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow10.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow11.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow12.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow13.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow14.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow15.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow16.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow17.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow18.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow19.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow20.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow21.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow22.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow23.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow24.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow25.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow26.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
            fRow27.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
        }

        void addfilter(Worksheet mySheet, int rowNo, int columnNo)
        {
            Range firstRow = (Range)mySheet.Rows[2];
            firstRow.Interior.Color = Color.SkyBlue;
            firstRow.Borders.LineStyle = XlLineStyle.xlContinuous;

            Range usedRange = mySheet.UsedRange;
            Range rows = usedRange.Rows;
            int count = 0;

            foreach (Range row in rows)
            {
                if (count > 0 && count < rows.Count - 2)
                {
                    if (row.Cells[2].value.ToString().Length == 2)
                    {
                        row.Interior.Color = System.Drawing.Color.Gold;
                    }
                }

                count++;

                if (row.Cells[1].Value == null)
                {
                    continue;
                }

                if (row.Cells[1].Value.StartsWith("GRAND TOTAL"))
                {
                    row.Interior.Color = System.Drawing.Color.Aquamarine;
                    row.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    row.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                }
            }
        }
    }
}
