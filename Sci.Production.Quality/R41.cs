using Ict;
using Microsoft.Office.Interop.Excel;
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
    public partial class R41 : Sci.Win.Tems.PrintForm
    {
        public R41(ToolStripMenuItem menuitem)
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
        string Brand;
        string Year;
        DualResult result;

        System.Data.DataTable[] alldt;
        System.Data.DataTable dym = null;
        System.Data.DataTable dy = null;
        string defect1 = "";
        string defect2 = "";
        System.Data.DataTable[] dt;
        System.Data.DataTable[] dts;

        System.Data.DataTable[] allda;
        System.Data.DataTable sym = null;
        System.Data.DataTable sy = null;
        string style1 = "";
        string style2 = "";
        System.Data.DataTable[] da;
        System.Data.DataTable[] das;

        System.Data.DataTable[] alldatb;
        System.Data.DataTable cym = null;
        System.Data.DataTable cy = null;
        string country1 = "";
        string country2 = "";
        System.Data.DataTable[] datb;
        System.Data.DataTable[] datbs;

        System.Data.DataTable[] alldatatable;
        System.Data.DataTable fm = null;
        System.Data.DataTable datatab;        
        System.Data.DataTable dt_All;

        protected override bool ValidateInput()
        {
            Brand = combo_Brand.Text.ToString();
            Year = combo_Year.Text.ToString();
            return base.ValidateInput();
        }


        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string gb = ""; string gb1 = ""; string gb2 = ""; string gb3 = ""; string ob = ""; string ob1 = ""; string sqlWh1="";
            List<string> sqlWheres = new List<string>();

            if (Brand != "")
            {
                sqlWheres.Add("b.BrandID ='" + Brand + "'");
            }
            if (Year != "")
            {
                string Year2 = (Convert.ToInt32(Year) - 2).ToString();
                sqlWheres.Add("year(a.StartDate)" + "between" + "'" + Year2 + "' " + "and" + "'" + Year + "'");

            }

            List<string> sqlWh = new List<string>();

            if (Brand != "")
            {
                sqlWh.Add("b.BrandID ='" + Brand + "'");
            }
            if (Year != "")
            {
                sqlWh.Add("year(a.StartDate)='" + Year + "'");
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
                sqlWhere = " where " + sqlWhere;
            }

            sqlWh1 = string.Join(" and ", sqlWh);
            if (!sqlWh1.Empty())
            {
                sqlWh1 = " where " + sqlWh1;
            }
            
            #region Defect
            

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

                                            select YEAR(a.StartDate)[y]
                                                  ,format(a.StartDate,'MMMMM')[m]
                                                  ,[Defect]=C.SubName
                                                  ,[Qty]=SUM(B.QTY)
                                                  ,[Amount]=SUM(B.ValueinUSD)
                                                  ,row_number() over (partition by YEAR(A.StartDate),MONTH(A.StartDate) 
					                               order by SUM(B.Qty) desc) as rnk
                                            into #temp
                                            from dbo.ADIDASComplain a
                                            inner join dbo.ADIDASComplain_Detail b on a.id=b.id
                                            left join dbo.ADIDASComplainDefect_Detail c on c.id=b.DefectMainID AND C.SubID=B.DefectSubID" + " " + sqlWhere + " " + gb + " " +

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

                                            select * from #temp", Year + "-01-01");
            SqlConnection conn;
            result = DBProxy.Current.OpenConnection("", out conn);
            if (!result) { return result; }

            //List<SqlParameter> plis = new List<SqlParameter>();
            //plis.Add(new SqlParameter("@date", Year + "-01-01"));

            result = DBProxy.Current.SelectByConn(conn, sqlcmd, out alldt);
            if (!result) { return result; }

            
            dym = alldt[0];
            for (int i = 0; i < dym.Rows.Count; i++)
            {
                string dyear = dym.Rows[i]["cd"].ToString();
                string dmonth = dym.Rows[i]["name"].ToString();
                string a = string.Format("SELECT top 10 Defect,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' and m  ='{1}' GROUP BY Defect", dyear, dmonth);
                defect1 += a + ' ' + Environment.NewLine;
            }

            result = DBProxy.Current.SelectByConn(conn, defect1, out dt);
            if (!result) { return result; }

            dy = alldt[1];
            for (int i = 0; i < dy.Rows.Count; i++)
            {
                string dyear1 = dy.Rows[i]["yy"].ToString();
                string b = string.Format("SELECT top 10 Defect,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' GROUP BY Defect", dyear1);
                defect2 += b + ' ' + Environment.NewLine;
            }

            result = DBProxy.Current.SelectByConn(conn, defect2, out dts);
            if (!result) { return result; }
           
            #endregion

            #region Style
          
            string scmd = string.Format(@"create table #dRanges(starts int , ends int, name varchar(20))
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
                                           order by SUM(B.Qty) desc) as rnk
                                           into #temp
                                           from dbo.ADIDASComplain a
                                           inner join dbo.ADIDASComplain_Detail b on a.id=b.id" + " " + sqlWhere + " " + gb1 + " " + ob + " " +

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

                                            select * from #temp", Year + "-01-01");
            SqlConnection con;
            result = DBProxy.Current.OpenConnection("", out con);
            if (!result) { return result; }

            result = DBProxy.Current.SelectByConn(con, scmd, out allda);
            if (!result) { return result; }


            sym = allda[0];
            for (int i = 0; i < sym.Rows.Count; i++)
            {
                string syear = sym.Rows[i]["cd"].ToString();
                string smonth = sym.Rows[i]["name"].ToString();
                string a = string.Format("SELECT top 10 Style,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' and m  ='{1}' GROUP BY style", syear, smonth);
                style1 += a + ' ' + Environment.NewLine;
            }

            result = DBProxy.Current.SelectByConn(con, style1, out da);
            if (!result) { return result; }

            sy = allda[1];
            for (int i = 0; i < sy.Rows.Count; i++)
            {
                string syear1 = sy.Rows[i]["yy"].ToString();
                string b = string.Format("SELECT top 10 Style,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' GROUP BY Style", syear1);
                style2 += b + ' ' + Environment.NewLine;
            }

            result = DBProxy.Current.SelectByConn(con, style2, out das);
            if (!result) { return result; }
            #endregion

            #region Country
           
            string sqcmd = string.Format(@"create table #dRanges(starts int , ends int, name varchar(20))
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
                                           order by SUM(B.Qty) desc) as rnk
                                           into #temp
                                           from dbo.ADIDASComplain a
                                           inner join dbo.ADIDASComplain_Detail b on a.id=b.id" + " " + sqlWhere + " " + gb2 + " " + ob + " " +

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

                                            select * from #temp", Year + "-01-01");
            SqlConnection connection;
            result = DBProxy.Current.OpenConnection("", out connection);
            if (!result) { return result; }

            result = DBProxy.Current.SelectByConn(connection, sqcmd, out alldatb);
            if (!result) { return result; }


            cym = alldatb[0];
            for (int i = 0; i < cym.Rows.Count; i++)
            {
                string cyear = cym.Rows[i]["cd"].ToString();
                string cmonth = cym.Rows[i]["name"].ToString();
                string a = string.Format("SELECT top 10 Country,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' and m  ='{1}' GROUP BY Country", cyear, cmonth);
                country1 += a + ' ' + Environment.NewLine;
            }

            result = DBProxy.Current.SelectByConn(connection, country1, out datb);
            if (!result) { return result; }

            cy = alldatb[1];
            for (int i = 0; i < cy.Rows.Count; i++)
            {
                string cyear1 = cy.Rows[i]["yy"].ToString();
                string b = string.Format("SELECT top 10 Country,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' GROUP BY Country", cyear1);
                country2 += b + ' ' + Environment.NewLine;
            }

            result = DBProxy.Current.SelectByConn(connection, country2, out datbs);
            if (!result) { return result; }
      
            #endregion

            #region Factory
            dicFTY.Clear();
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
                                                FROM dbo.ADIDASComplain A 
                                                INNER JOIN dbo.ADIDASComplain_Detail B on b.ID = A.ID
                                                left join (dbo.ADIDASComplainDefect c inner join dbo.ADIDASComplainDefect_Detail d on c.id = d.ID)
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
            result = DBProxy.Current.OpenConnection("", out connction);
            if (!result) { return result; }

            result = DBProxy.Current.SelectByConn(connction, scd, out alldatatable);
            if (!result) { return result; }


            fm = alldatatable[0];
            for (int i = 0; i < fm.Rows.Count; i++)
            {
                string factory = fm.Rows[i]["fty"].ToString();

                string a = string.Format(@"DECLARE @January_sumQty numeric(10,0) select @January_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='January' and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as January
DECLARE @February_sumQty numeric(10,0) select @February_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where m='February' and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as February
DECLARE @March_sumQty numeric(10,0) select @March_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='March'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as March
DECLARE @April_sumQty numeric(10,0) select @April_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='April'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as April
DECLARE @May_sumQty numeric(10,0) select @May_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='May'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as May
DECLARE @June_sumQty numeric(10,0) select @June_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='June'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as June
DECLARE @July_sumQty numeric(10,0) select @July_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where m='July'and FactoryID='{0}' and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as July
DECLARE @August_sumQty numeric(10,0) select @August_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='August'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as August
DECLARE @September_sumQty numeric(10,0) select @September_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='September' and FactoryID='{0}'and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as September
DECLARE @October_sumQty numeric(10,0) select @October_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='October'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as October
DECLARE @November_sumQty numeric(10,0) select @November_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='November'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as November
DECLARE @December_sumQty numeric(10,0) select @December_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='December'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as December
DECLARE @YTD_sumQty numeric(10,0) select @YTD_sumQty=SUM([Quality]) from (select Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select [Quality]=sum(qty) from #temp t where  m='December'and FactoryID='{0}' and t.DefectMainID+t.DefectSubID=main.Defect_Code and Description=main.Description group by t.Qty,t.Amount) as YTD


select main.Description,
main.Defect_Code,
[January_Quality]=sum(January.Quality),
[January_Percentage]=January.Percentage,
[January_Amount]=sum(January.Amount),
[February_Quality]=sum(February.Quality),
[February_Percentage]=February.Percentage,
[February_Amount]=sum(February.Amount),
[March_Quality]=sum(March.Quality),
[March_Percentage]=March.Percentage,
[March_Amount]=sum(March.Amount),
[April_Quality]=sum(April.Quality),
[April_Percentage]=April.Percentage,
[April_Amount]=sum(April.Amount),
[May_Quality]=sum(May.Quality),
[May_Percentage]=May.Percentage,
[May_Amount]=sum(May.Amount),
[June_Quality]=sum(June.Quality),
[June_Percentage]=June.Percentage,
[June_Amount]=sum(June.Amount),
[July_Quality]=sum(July.Quality),
[July_Percentage]=July.Percentage,
[July_Amount]=sum(July.Amount),
[August_Quality]=sum(August.Quality),
[August_Percentage]=August.Percentage,
[August_Amount]=sum(August.Amount),
[September_Quality]=sum(September.Quality),
[September_Percentage]=September.Percentage,
[September_Amount]=sum(September.Amount),
[October_Quality]=sum(October.Quality),
[October_Percentage]=October.Percentage,
[October_Amount]=sum(October.Amount),
[November_Quality]=sum(November.Quality),
[November_Percentage]=November.Percentage,
[November_Amount]=sum(November.Amount),
[December_Quality]=sum(December.Quality),
[December_Percentage]=December.Percentage,
[December_Amount]=sum(December.Amount),
[YTD_Quality]=sum(YTD.Quality),
[YTD_Percentage]=YTD.Percentage,
[YTD_Amount]=sum(YTD.Amount)
from (select distinct Description,Defect_Code from #defect1 where Description!='' and Defect_Code!='')as main
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@January_sumQty) ,[Amount]=t.Amount from #temp t where  m='January' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}' ) as January
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@February_sumQty) ,[Amount]=t.Amount from #temp t where  m='February' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as February
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@March_sumQty )  ,[Amount]=t.Amount from #temp t where  m='March' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as March
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@April_sumQty )  ,[Amount]=t.Amount from #temp t where  m='April' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as April
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@May_sumQty ) ,[Amount]=t.Amount from #temp t where  m='May' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as May
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@June_sumQty ) ,[Amount]=t.Amount from #temp t where  m='June' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as June
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@July_sumQty ) ,[Amount]=t.Amount from #temp t where  m='July' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as July
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@August_sumQty ),[Amount]=t.Amount from #temp t where  m='August' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as August
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@September_sumQty)  ,[Amount]=t.Amount from #temp t where  m='September' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as September
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@October_sumQty )  ,[Amount]=t.Amount from #temp t where  m='October' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as October
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@November_sumQty )  ,[Amount]=t.Amount from #temp t where  m='November' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as November
outer apply (select distinct [Quality]=t.qty,[Percentage]=(t.qty/@December_sumQty ) ,[Amount]=t.Amount from #temp t where  m='December' and t.Defect_Code=main.Defect_Code and t.Description=main.Description and t.factoryid='{0}') as December
outer apply (select distinct [Quality]=0,[Percentage]=cast(cast(0 as numeric(5,4)) as varchar(20)),[Amount]=0 ) as YTD 
group by main.Description,main.Defect_Code
,January.Percentage
,February.Percentage
,March.Percentage
,April.Percentage
,May.Percentage
,June.Percentage
,July.Percentage
,August.Percentage
,September.Percentage
,October.Percentage
,November.Percentage
,December.Percentage
,YTD.Percentage
order by Defect_Code", factory);

                result = DBProxy.Current.SelectByConn(connction, a, out datatab);


                int startIndex1 = 2;
                //最後一列Total

                DataRow totalrow1 = datatab.NewRow();
                totalrow1[0] = "GRAND TOTAL";

                //for dt每個欄位
                decimal TTColumnAMT1 = 0;
                for (int colIdx = startIndex1; colIdx < datatab.Columns.Count; colIdx++)
                {
                    TTColumnAMT1 = 0;
                    //for dt每一列
                    for (int rowIdx = 0; rowIdx < datatab.Rows.Count; rowIdx++)
                    {
                        if (datatab.Rows[rowIdx][colIdx] == DBNull.Value) continue;
                         TTColumnAMT1 += Convert.ToDecimal(datatab.Rows[rowIdx][colIdx]);
                    }

                    totalrow1[colIdx] = TTColumnAMT1;
                }
                Decimal YTD_Quality = 0;
                Decimal YTD_Percentage = 0;
                Decimal YTD_Amount = 0;
                for (int idx = datatab.Rows.Count - 1 ; idx >= 0; idx--)
                {
                    DataRow row = datatab.Rows[idx];

                    string code = row["Defect_Code"].ToString();

                    if (code.Length == 2)
                    {
                        row["YTD_Quality"] = YTD_Quality;
                        row["YTD_Percentage"] = YTD_Percentage;
                        row["YTD_Amount"] = YTD_Amount;

                        YTD_Quality = 0;
                        YTD_Percentage = 0;
                        YTD_Amount = 0;

                        continue;
                    }

                    for (int mon = 0; mon < 12; mon++)
                    {
                        YTD_Quality += row[mon * 3 + 2] == DBNull.Value ? 0 : Convert.ToDecimal(row[mon * 3 + 2]);
                        YTD_Percentage += row[mon * 3 + 3] == DBNull.Value ? 0 : Convert.ToDecimal(row[mon * 3 + 3]);
                        YTD_Amount += row[mon * 3 + 4] == DBNull.Value ? 0 : Convert.ToDecimal(row[mon * 3 + 4]);
                    }

                }

                if (null == datatab || datatab.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found");
                }


                datatab.Rows.Add(totalrow1);
                dicFTY.Add(factory, datatab);
            }


            if (!result) { return result; }

            if (null == dt_All || 0 == dt_All.Rows.Count)
            {

                dt_All = datatab;
            }
            else
            {
                dt_All.Merge(datatab);
            }

            if (!result)
            {
                //return result;
                this.ShowErr(result);
            }

            #endregion

            return result;
        }



        Dictionary<string, System.Data.DataTable> dicFTY = new Dictionary<string, System.Data.DataTable>();

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            //saveDialog.ShowDialog();
            //string outpath = saveDialog.FileName;
            //if (outpath.Empty())
            //{
            //    return false;
            //}

            SaveXltReportCls sxc = new SaveXltReportCls("Quality_R41.xltx");
            SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dt_All);

            #region Defect

            //for (int i = 0; i < dt.Rows.Count; i++)
            for (int i = 0; i < dym.Rows.Count; i++)
            {
                string dyear = dym.Rows[i]["cd"].ToString();
                string dmonth =dym.Rows[i]["name"].ToString();


                SaveXltReportCls.xltRptTable dxt = new SaveXltReportCls.xltRptTable(dt[i]);
                dxt.Columns.RemoveAt(dxt.Columns.Count - 1);
                dxt.boAddNewRow = false;

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

                dxt.lisTitleMerge.Add(dic);
                dxt.lisTitleMerge.Add(dic2);
                dxt.Borders.OutsideVertical = true;
                dxt.ShowHeader = true;
                sxc.dicDatas.Add("##defect" + i, dxt);

               
            }

            //for (int a = 0; a < dts.Length; a++)
            for (int a = 0; a < dy.Rows.Count; a++)
            {
                string dyear1 = dy.Rows[a]["yy"].ToString();

                SaveXltReportCls.xltRptTable alldxtb = new SaveXltReportCls.xltRptTable(dts[a]);
                alldxtb.Columns.RemoveAt(alldxtb.Columns.Count - 1);
                alldxtb.boAddNewRow = false;

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


                alldxtb.lisTitleMerge.Add(dic);
                alldxtb.lisTitleMerge.Add(dic2);
                alldxtb.Borders.AllCellsBorders = false;
                alldxtb.Borders.OutsideVertical = true;
                alldxtb.ShowHeader = true;
                sxc.dicDatas.Add("##adefect" + a, alldxtb);
                
            }
            SaveXltReportCls.ReplaceAction adr = addrow;
            sxc.dicDatas.Add("##addRow", adr);
            #endregion

            #region Style
            //for (int i = 0; i < da.Length; i++)
            for (int i = 0; i < sym.Rows.Count; i++)
            {
                string syear = sym.Rows[i]["cd"].ToString();
                string smonth =sym.Rows[i]["name"].ToString();


                SaveXltReportCls.xltRptTable sxt = new SaveXltReportCls.xltRptTable(da[i]);
                sxt.Columns.RemoveAt(sxt.Columns.Count - 1);
                sxt.boAddNewRow = false;

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

                sxt.lisTitleMerge.Add(dic);
                sxt.lisTitleMerge.Add(dic2);
                sxt.Borders.OutsideVertical = true;
                sxt.ShowHeader = true;
                sxc.dicDatas.Add("##style" + i, sxt);
            }

            //for (int a = 0; a < das.Length; a++)
            for (int a = 0; a < sy.Rows.Count; a++)
            {
                string syear1 = sy.Rows[a]["yy"].ToString();

                SaveXltReportCls.xltRptTable allsxtb = new SaveXltReportCls.xltRptTable(das[a]);
                allsxtb.Columns.RemoveAt(allsxtb.Columns.Count - 1);
                allsxtb.boAddNewRow = false;

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

                allsxtb.lisTitleMerge.Add(dic);
                allsxtb.lisTitleMerge.Add(dic2);
                allsxtb.ShowHeader = true;
                allsxtb.Borders.OutsideVertical = true;
                sxc.dicDatas.Add("##astyle" + a, allsxtb);
            }
            #endregion

            #region Country
            
            //for (int i = 0; i < datb.Length; i++)
            for (int i = 0; i < cym.Rows.Count; i++)
            {
                string cyear = cym.Rows[i]["cd"].ToString();
                string cmonth =cym.Rows[i]["name"].ToString();


                SaveXltReportCls.xltRptTable cxt = new SaveXltReportCls.xltRptTable(datb[i]);
                cxt.Columns.RemoveAt(cxt.Columns.Count - 1);
                cxt.boAddNewRow = false;
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

                cxt.lisTitleMerge.Add(dic);
                cxt.lisTitleMerge.Add(dic2);

                cxt.Borders.OutsideVertical = true;
                cxt.ShowHeader = true;
                sxc.dicDatas.Add("##country" + i, cxt);
            }

            //for (int a = 0; a < datbs.Length; a++)
            for (int a = 0; a < cy.Rows.Count; a++)
            {
                string cyear1 = cy.Rows[a]["yy"].ToString();

                SaveXltReportCls.xltRptTable allcxtb = new SaveXltReportCls.xltRptTable(datbs[a]);
                allcxtb.Columns.RemoveAt(allcxtb.Columns.Count - 1);
                allcxtb.boAddNewRow = false;

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

                allcxtb.lisTitleMerge.Add(dic);
                allcxtb.lisTitleMerge.Add(dic2);
                allcxtb.Borders.OutsideVertical = true;
                allcxtb.ShowHeader = true;
                sxc.dicDatas.Add("##acountry" + a, allcxtb);
            }
            #endregion

            #region Factory
            foreach (var item in dicFTY)
            {
                string fty = item.Key;
                SaveXltReportCls.xltRptTable x_All = new SaveXltReportCls.xltRptTable(item.Value);


                for (int i = 0; i < x_All.Columns.Count; i++)
                {
                    SaveXltReportCls.xlsColumnInfo xlc = new SaveXltReportCls.xlsColumnInfo(i + 1);
                    if (x_All.Columns[i].ColumnName == "January_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "February_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "March_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "April_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "May_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "June_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "July_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "August_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "September_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "October_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "November_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "December_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "YTD_Percentage")
                    {
                        xlc.NumberFormate = "0%";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "January_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "February_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "March_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "April_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "May_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "June_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "July_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "August_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "September_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "October_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "November_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "December_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    if (x_All.Columns[i].ColumnName == "YTD_Amount")
                    {
                        xlc.NumberFormate = "$#,##0.00";
                        xlc.IsNumber = false;
                    }
                    xlc.IsAutoFit = true;
                    x_All.lisColumnInfo.Add(xlc);
                }


                sxc.dicDatas.Add("##SUPSheetName" + fty, item.Key);
                sxc.dicDatas.Add("##psd" + fty, x_All);
                x_All.ShowHeader = false;
                x_All.Borders.AllCellsBorders = true;
                
            }

            sxc.VarToSheetName = "##SUPSheetName";

            SaveXltReportCls.ReplaceAction c = CopySheet;
            sxc.dicDatas.Add("##copysupsheet", c);

            SaveXltReportCls.ReplaceAction d = addfilter;
            sxc.dicDatas.Add("##addfilter", d);

            sxc.Save();
            return true;
        }
            #endregion

        void CopySheet(Worksheet mySheet, int rowNo, int columnNo)
        {
            Microsoft.Office.Interop.Excel._Application myExcel = null;
            Microsoft.Office.Interop.Excel._Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;
            
            Worksheet aftersheet = mySheet;

            List<Worksheet> lisWK = new List<Worksheet>();
            foreach (var item in dicFTY)
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
                wkSheet.Cells[1, 1] = ""; wkSheet.get_Range("A1:B1").Merge();
                wkSheet.Cells[1, 2] = "";
                wkSheet.Cells[1, 3] = "January"; wkSheet.get_Range("C1:E1").Merge();
                wkSheet.Cells[1, 6] = "Feburary"; wkSheet.get_Range("F1:H1").Merge();
                wkSheet.Cells[1, 9] = "March"; wkSheet.get_Range("I1:K1").Merge();
                wkSheet.Cells[1, 12] = "April"; wkSheet.get_Range("L1:N1").Merge();
                wkSheet.Cells[1, 15] = "May"; wkSheet.get_Range("O1:Q1").Merge();
                wkSheet.Cells[1, 18] = "June"; wkSheet.get_Range("R1:T1").Merge();
                wkSheet.Cells[1, 21] = "July"; wkSheet.get_Range("U1:W1").Merge();
                wkSheet.Cells[1, 24] = "August"; wkSheet.get_Range("X1:Z1").Merge();
                wkSheet.Cells[1, 27] = "September"; wkSheet.get_Range("AA1:AC1").Merge();
                wkSheet.Cells[1, 30] = "October"; wkSheet.get_Range("AD1:AF1").Merge();
                wkSheet.Cells[1, 33] = "November"; wkSheet.get_Range("AG1:AI1").Merge();
                wkSheet.Cells[1, 36] = "December"; wkSheet.get_Range("AJ1:AL1").Merge();
                wkSheet.Cells[1, 39] = "YTD"; wkSheet.get_Range("AM1:AO1").Merge();

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
                
                for (int idx = 1; idx < 14; idx++)
                {
                    wkSheet.Cells[2, idx * 3] = "Quality";
                    wkSheet.Cells[2, idx * 3 + 1] = "Percentage";
                    wkSheet.Cells[2, idx * 3 + 2] = "Amount US$"; 
                }                
            }

            //mySheet.Delete();

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
            Range fRow13= (Range)mySheet.Rows[23];
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


            Microsoft.Office.Interop.Excel.Range usedRange = mySheet.UsedRange;
            Microsoft.Office.Interop.Excel.Range rows = usedRange.Rows;
            int count = 0;

            foreach (Microsoft.Office.Interop.Excel.Range row in rows)
            {

                if (count > 0 && count < rows.Count - 2)
                {
                    if (row.Cells[2].value.ToString().Length == 2)
                    {
                        row.Interior.Color = System.Drawing.Color.Gold;
    
                    }
                    
                }
                count++;

                if (row.Cells[1].Value == null) continue;
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
