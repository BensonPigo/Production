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

          DataTable[] alldt;
          DataTable dym = null;
          DataTable dy = null;
          string defect1 = "";
          string defect2 = "";
          DataTable[] dt;
          DataTable[] dts;

          DataTable[] allda;
          DataTable sym = null;
          DataTable sy = null;
          string style1 = "";
          string style2 = "";
          DataTable[] da;
          DataTable[] das;

          DataTable[] alldatb;
          DataTable cym = null;
          DataTable cy = null;
          string country1 = "";
          string country2 = "";
          DataTable[] datb;
          DataTable[] datbs;

          protected override bool ValidateInput()
          {
              Brand = combo_Brand.Text.ToString();
              Year = combo_Year.Text.ToString();
              return base.ValidateInput();
          }


          protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
          {
              List<SqlParameter> lis = new List<SqlParameter>();
              string sqlWhere = ""; string gb = ""; string gb1 = ""; string gb2 = ""; string ob = "";
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
              gb = "group by A.StartDate, B.DefectMainID, B.DefectSubID,C.SubName";
              ob = "order by Y,M,QTY DESC";
              gb1 = "group by A.StartDate,b.styleid";
              gb2 = "group by A.StartDate,b.SalesName";
              sqlWhere = string.Join(" and ", sqlWheres);
              if (!sqlWhere.Empty())
              {
                  sqlWhere = " where " + sqlWhere;
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
                                            left join dbo.ADIDASComplainDefect_Detail c on c.id=b.DefectMainID AND C.SubID=B.DefectSubID" + " " + sqlWhere + " " + gb + " "+

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
                  string dmonth =dym.Rows[i]["name"].ToString();
                  string a = string.Format("SELECT top 10 Defect,SUM(Qty) AS Qty, SUM(Amount) AS Amount ,row_number() over (order by SUM(Qty) desc) as rnk FROM #temp WHERE y = '{0}' and m  ='{1}' GROUP BY Defect", dyear, dmonth);
                 defect1 += a +' '+ Environment.NewLine;
              }

              result = DBProxy.Current.SelectByConn(conn,defect1, out dt);
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
                                           inner join dbo.ADIDASComplain_Detail b on a.id=b.id" + " " + sqlWhere + " " + gb1 + " " +ob+" "+

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

              return result;
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

              SaveXltReportCls sxc = new SaveXltReportCls("Quality_R41.xltx");
             
              
              #region Defect
              for (int i = 0; i < dt.Length; i++)
              {
                  string dyear = dym.Rows[i]["cd"].ToString();
                  string dmonth = dym.Rows[i]["name"].ToString();
                  
                  
                  SaveXltReportCls.xltRptTable dxt = new SaveXltReportCls.xltRptTable(dt[i]);
                  dxt.Columns.RemoveAt(dxt.Columns.Count - 1);

                  if (dxt.Rows.Count <= 0)
                  {
                      for (int j = 0; j < 10; j++)
                      {
                          dxt.Rows.Add();
                      }                      
                  }
                  //Dictionary<string, string> dic = new Dictionary<string, string>();
                  //dic.Add(month, "1,3");

                  //Dictionary<string, string> dic2 = new Dictionary<string, string>();                  
                  //dic2.Add(year, "1,3");

                  //xtbl.lisTitleMerge.Add(dic);
                  //xtbl.lisTitleMerge.Add(dic2);
                  dxt.Borders.AllCellsBorders = true;
                  dxt.ShowHeader = true;
                  sxc.dicDatas.Add("##defect" + i, dxt); 
              }

              for (int a = 0; a < dts.Length; a++)
              {
                  string dyear1 = dy.Rows[a]["yy"].ToString();

                  SaveXltReportCls.xltRptTable alldxtb = new SaveXltReportCls.xltRptTable(dts[a]);
                  alldxtb.Columns.RemoveAt(alldxtb.Columns.Count - 1);

                  if (alldxtb.Rows.Count <= 0)
                  {
                      for (int b = 0; b < 10; b++)
                      {
                          alldxtb.Rows.Add();
                      }
                  }
                  alldxtb.Borders.AllCellsBorders = true;
                  alldxtb.ShowHeader = true;
                  sxc.dicDatas.Add("##adefect" + a, alldxtb);
              }
              #endregion

              #region Style
              for (int i = 0; i < da.Length; i++)
              {
                  string syear = sym.Rows[i]["cd"].ToString();
                  string smonth = sym.Rows[i]["name"].ToString();


                  SaveXltReportCls.xltRptTable sxt = new SaveXltReportCls.xltRptTable(da[i]);
                  sxt.Columns.RemoveAt(sxt.Columns.Count - 1);

                  if (sxt.Rows.Count <= 0)
                  {
                      for (int j = 0; j < 10; j++)
                      {
                          sxt.Rows.Add();
                      }
                  }

                  sxt.Borders.AllCellsBorders = true;
                  sxt.ShowHeader = true;
                  sxc.dicDatas.Add("##style" + i, sxt);
              }

              for (int a = 0; a < das.Length; a++)
              {
                  string syear1 = sy.Rows[a]["yy"].ToString();

                  SaveXltReportCls.xltRptTable allsxtb = new SaveXltReportCls.xltRptTable(das[a]);
                  allsxtb.Columns.RemoveAt(allsxtb.Columns.Count - 1);

                  if (allsxtb.Rows.Count <= 0)
                  {
                      for (int b = 0; b < 10; b++)
                      {
                          allsxtb.Rows.Add();
                      }
                  }
                  allsxtb.Borders.AllCellsBorders = true;
                  allsxtb.ShowHeader = true;
                  sxc.dicDatas.Add("##astyle" + a, allsxtb);
              }
              #endregion

              #region Country
              for (int i = 0; i < datb.Length; i++)
              {
                  string cyear = cym.Rows[i]["cd"].ToString();
                  string cmonth = cym.Rows[i]["name"].ToString();


                  SaveXltReportCls.xltRptTable cxt = new SaveXltReportCls.xltRptTable(datb[i]);
                  cxt.Columns.RemoveAt(cxt.Columns.Count - 1);

                  if (cxt.Rows.Count <= 0)
                  {
                      for (int j = 0; j < 10; j++)
                      {
                          cxt.Rows.Add();
                      }
                  }

                  cxt.Borders.AllCellsBorders = true;
                  cxt.ShowHeader = true;
                  sxc.dicDatas.Add("##country" + i, cxt);
              }

              for (int a = 0; a < datbs.Length; a++)
              {
                  string cyear1 = cy.Rows[a]["yy"].ToString();

                  SaveXltReportCls.xltRptTable allcxtb = new SaveXltReportCls.xltRptTable(datbs[a]);
                  allcxtb.Columns.RemoveAt(allcxtb.Columns.Count - 1);

                  if (allcxtb.Rows.Count <= 0)
                  {
                      for (int b = 0; b < 10; b++)
                      {
                          allcxtb.Rows.Add();
                      }
                  }
                  allcxtb.Borders.AllCellsBorders = true;
                  allcxtb.ShowHeader = true;
                  sxc.dicDatas.Add("##acountry" + a, allcxtb);
              }
              #endregion

              sxc.Save(outpath, true);





            
              return true;
          }

    }
}
