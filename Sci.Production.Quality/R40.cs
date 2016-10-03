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
using System.Reflection;
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
        DualResult result;
        System.Data.DataTable dtt;
        System.Data.DataTable dt;
        System.Data.DataTable dat;
        System.Data.DataTable dt_All;
        System.Data.DataTable dat_All;
        string userfactory = Sci.Env.User.Factory;

        protected override bool ValidateInput()
        {
            Brand = comboBox_brand.SelectedItem.ToString();
            Year = radiobtn_byYear.Checked.ToString();
            Factory = radiobtn_byfactory.Checked.ToString();

            return true;//base.ValidateInput();
        }



        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            dt_All = null;
            dat_All = null;
            dtt = null;
            dt = null;
            dat = null;


            if (radiobtn_byYear.Checked == true)
            {
                #region By Year
                string sqlcmd = string.Format(@"declare @dRanges table(starts int , ends int, name varchar(3))
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
                    --select @d
                    declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
                    declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
                    declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))

                    select Target,Claimed.Claimed,sh.qty[Shipped],convert(varchar(10),Claimed.month1)[month1],convert(varchar(10),Claimed.YEAR1)[YEAR1]
                    into #temp
                    from dbo.ADIDASComplainTarget 
                    outer apply(SELECT left(cast(a.StartDate as varchar(10)),7) ym , sum(b.Qty) Claimed,convert(varchar(10),dateadd(MONTH,-3,a.StartDate)) themonth ,convert(varchar(10),MONTH(a.StartDate))[month1],convert(varchar(10),YEAR(a.StartDate)) [YEAR1]
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

                    select dRanges.name[ ],dRanges.starts,#temp.Target,isnull(SUM(year1.Claimed),0)[Claimed1],isnull(SUM(year1.Shipped),0)[Shipped1],isnull(SUM(year2.Claimed),0)[Claimed2],isnull(SUM(year2.Shipped),0)[Shipped2],isnull(SUM(year3.Claimed),0)[Claimed3],isnull(SUM(year3.Shipped),0)[Shipped3] from dbo.#temp
                    inner join @dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends 
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE YEAR1=@y1 and dRanges.starts=month1)AS year1
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE YEAR1=@y2 and dRanges.starts=month1)AS year2
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE YEAR1=@y3 and dRanges.starts=month1)AS year3
					GROUP BY dRanges.name,#temp.Target,year1.Claimed,year1.Shipped,year2.Claimed,year2.Shipped,year3.Claimed,year3.Shipped,dRanges.starts
                    order by dRanges.starts
                    DROP TABLE #temp");
                result = DBProxy.Current.Select("", sqlcmd, out dtt);

                dtt.Columns.Remove("starts");
                int startIndex = 1;
                //最後一列Total

                DataRow totalrow = dtt.NewRow();
                totalrow[0] = "YTD";


                //for dt每個欄位
                decimal TTColumnAMT = 0;
                for (int colIdx = startIndex; colIdx < dtt.Columns.Count; colIdx++)
                {


                    TTColumnAMT = 0;
                    //for dt每一列
                    for (int rowIdx = 0; rowIdx < dtt.Rows.Count; rowIdx++)
                    {
                        TTColumnAMT += Convert.ToDecimal(dtt.Rows[rowIdx][colIdx]);
                    }

                    totalrow[colIdx] = TTColumnAMT;
                }

                if (null == dtt || dtt.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found");
                }


                dtt.Rows.Add(totalrow);
                DataColumn percent = dtt.Columns.Add("adiComp1");
                DataColumn percent1 = dtt.Columns.Add("adiComp2");
                DataColumn percent2 = dtt.Columns.Add("adiComp3");
                foreach (DataRow row in dtt.Rows)
                {
                    percent.SetOrdinal(4);
                    if (row["Shipped1"].Empty())
                    {
                        percent.Expression = "0";
                    }
                    else
                    {
                        percent.Expression = "[Claimed1] / [Shipped1]";
                    }

                    percent1.SetOrdinal(7);
                    if (row["Shipped2"].Empty())
                    {
                        percent1.Expression = "0";
                    }
                    else
                    {
                        percent1.Expression = "[Claimed2] / [Shipped2]";
                    }

                    percent2.SetOrdinal(10);
                    if (row["Shipped3"].Empty())
                    {
                        percent2.Expression = "0";
                    }
                    else
                    {
                        percent2.Expression = "[Claimed3] / [Shipped3]";
                    }
                }

                if (null == dt_All || 0 == dt_All.Rows.Count)
                {
                    dt_All = dtt;
                }
                else
                {
                    dt_All.Merge(dtt);
                }

                if (!result)
                {

                    return result;
                    //this.ShowErr(result);
                }
                #endregion
            }
            else
            {
               #region By Factory 橫年度 縱月份
//                string scmd = string.Format(@"declare @dRanges table(starts int , ends int, name varchar(3))
//                   insert into @dRanges values
//                    (1,1,'Jan'),
//                    (2,2,'Feb'),
//                    (3,3,'Mar'),
//                    (4,4,'Apr'),
//                    (5,5,'May'),
//                    (6,6,'Jun'),
//                    (7,7,'Jul'),
//                    (8,8,'Aug'),
//                    (9,9,'Sep'),
//                    (10,10,'Oct'),
//                    (11,11,'Nov'),
//                    (12,12,'Dec')
//
//                    declare @d date = dateadd(MONTH, -1, getdate()) 
//                    --select @d
//                    declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
//                    declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
//                    declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))
//                    select Target,Claimed.Claimed,sh.qty[Shipped],convert(varchar(10),Claimed.month1)[month1],convert(varchar(10),Claimed.YEAR1)[YEAR1],Claimed.factory
//                    into #temp
//                    from dbo.ADIDASComplainTarget 
//                    outer apply(SELECT left(cast(a.StartDate as varchar(10)),7) ym , sum(b.Qty) Claimed,convert(varchar(10),dateadd(MONTH,-3,a.StartDate)) themonth ,MONTH(a.StartDate)[month1],YEAR(a.StartDate) [YEAR1],b.FactoryID [factory]
//			                    FROM dbo.ADIDASComplain a
//			                    INNER JOIN DBO.ADIDASComplain_Detail b ON B.ID = a.ID
//			                    where a.StartDate in (@y1,@y2,@y3)
//			                    group by a.StartDate,B.FactoryID) Claimed
//                    outer apply(SELECT convert(varchar(10),dateadd(MONTH,5,Claimed.themonth)) as six,YEAR(a.StartDate)[YEAR1]
//			                    FROM dbo.ADIDASComplain a
//			                    where a.StartDate in (@y1,@y2,@y3)
//			                    group by a.StartDate) as ff 
//                    outer apply (SELECT ISNULL(SUM(a.Qty),0)/6 AS Qty FROM ADIDASComplain_MonthlyQty a
//			                    WHERE a.YearMonth BETWEEN claimed.themonth AND ff.six
//			                    AND a.BrandID = 'ADIDAS'
//								AND a.factoryid in (select id from dbo.SCIFty 
//			                    where CountryID= (select f.CountryID from dbo.Factory f where f.id=" + "'" + userfactory + "'" + @")))sh
//                    where year in (@y1,@y2,@y3)
//                    group by Target,Claimed.Claimed,sh.qty,Claimed.month1,Claimed.YEAR1,Claimed.factory
//                    select dRanges.name[ ],dRanges.starts[starts],#temp.Target,ISNULL(SUM(year1.Claimed),0)[Claimed1],ISNULL(SUM(year1.Shipped),0)[Shipped1],ISNULL(SUM(year2.Claimed),0)[Claimed2],ISNULL(SUM(year2.Shipped),0)[Shipped2],ISNULL(SUM(year3.Claimed),0)[Claimed3],ISNULL(SUM(year3.Shipped),0)[Shipped3],#temp.factory  from dbo.#temp
//                    inner join @dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends 
//					OUTER APPLY(SELECT Claimed,#temp.Shipped  FROM #temp WHERE YEAR1=@y1 AND factory=factory and dRanges.starts=month1)AS year1
//					OUTER APPLY(SELECT Claimed,#temp.Shipped  FROM #temp WHERE YEAR1=@y2 AND factory=factory and dRanges.starts=month1)AS year2
//					OUTER APPLY(SELECT Claimed,#temp.Shipped  FROM #temp WHERE YEAR1=@y3 AND factory=factory and dRanges.starts=month1)AS year3
//					GROUP BY dRanges.name,#temp.Target,year1.Claimed,year1.Shipped,year2.Claimed,year2.Shipped,year3.Claimed,year3.Shipped,#temp.factory,dRanges.starts
//					order by dRanges.starts
//                    DROP TABLE #temp");
//                result = DBProxy.Current.Select("", scmd, out dt);

//                dt.Columns.Remove("starts");
//                int startIndex = 1;
//                //最後一列Total

//                DataRow totalrow = dt.NewRow();
//                totalrow[0] = "YTD";

//                //for dt每個欄位
//                decimal TTColumnAMT = 0;
//                for (int colIdx = startIndex; colIdx < dt.Columns.Count; colIdx++)
//                {
//                    TTColumnAMT = 0;
//                    //for dt每一列
//                    for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
//                    {
//                        TTColumnAMT += Convert.ToDecimal(dt.Rows[rowIdx][colIdx]);
//                    }

//                    totalrow[colIdx] = TTColumnAMT;
//                }

//                if (null == dt || dt.Rows.Count == 0)
//                {
//                    return new DualResult(false, "Data not found");
//                }


//                dt.Rows.Add(totalrow);
//                DataColumn percent = dt.Columns.Add("adiComp1");
//                DataColumn percent1 = dt.Columns.Add("adiComp2");
//                DataColumn percent2 = dt.Columns.Add("adiComp3");
//                foreach (DataRow row in dt.Rows)
//                {
//                    percent.SetOrdinal(4);
//                    if (row["Shipped1"].Empty())
//                    {
//                        percent.Expression = "0";
//                    }
//                    else
//                    {
//                        percent.Expression = "[Claimed1] / [Shipped1]";
//                    }

//                    percent1.SetOrdinal(7);
//                    if (row["Shipped2"].Empty())
//                    {
//                        percent1.Expression = "0";
//                    }
//                    else
//                    {
//                        percent1.Expression = "[Claimed2] / [Shipped2]";
//                    }

//                    percent2.SetOrdinal(10);
//                    if (row["Shipped3"].Empty())
//                    {
//                        percent2.Expression = "0";
//                    }
//                    else
//                    {
//                        percent2.Expression = "[Claimed3] / [Shipped3]";
//                    }
//                }

//                if (null == dt_All || 0 == dt_All.Rows.Count)
//                {
//                    dt_All = dt;
//                }
//                else
//                {
//                    dt_All.Merge(dt);
//                }

//                if (!result)
//                {
//                    return result;
//                }
                #endregion

               #region By Factory 橫工廠別 縱月份
                string sqcmd = string.Format(@"declare @dRanges table(starts int , ends int, name varchar(3))
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
                    --select @d
                    declare @y1 varchar(4) = cast(datepart(year, dateadd(year,-2, @d) ) as varchar(4))
                    declare @y2 varchar(4) = cast(datepart(year, dateadd(year,-1, @d) ) as varchar(4))
                    declare @y3 varchar(4) = cast(datepart(year,@d) as varchar(4))
                    select Target,Claimed.Claimed,sh.qty[Shipped],convert(varchar(10),Claimed.month1)[month1],convert(varchar(10),Claimed.YEAR1)[YEAR1],Claimed.factory
                    into #temp
                    from dbo.ADIDASComplainTarget 
                    outer apply(SELECT left(cast(a.StartDate as varchar(10)),7) ym , sum(b.Qty) Claimed,convert(varchar(10),dateadd(MONTH,-3,a.StartDate)) themonth ,MONTH(a.StartDate)[month1],YEAR(a.StartDate) [YEAR1],b.FactoryID [factory]
			                    FROM dbo.ADIDASComplain a
			                    INNER JOIN DBO.ADIDASComplain_Detail b ON B.ID = a.ID
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate,B.FactoryID) Claimed
                    outer apply(SELECT convert(varchar(10),dateadd(MONTH,5,Claimed.themonth)) as six,YEAR(a.StartDate)[YEAR1]
			                    FROM dbo.ADIDASComplain a
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate) as ff 
                    outer apply (SELECT ISNULL(SUM(a.Qty),0)/6 AS Qty FROM ADIDASComplain_MonthlyQty a
			                    WHERE a.YearMonth BETWEEN claimed.themonth AND ff.six
			                    AND a.BrandID = 'ADIDAS'
								AND a.factoryid in (select id from dbo.SCIFty 
			                    where CountryID= (select f.CountryID from dbo.Factory f where f.id=" + "'" + userfactory + "'" + @")))sh
                    where year in (@y1,@y2,@y3)
                    group by Target,Claimed.Claimed,sh.qty,Claimed.month1,Claimed.YEAR1,Claimed.factory
                    select dRanges.name[ ],dRanges.starts,#temp.Target,ISNULL(SUM(factory1.Claimed),0)[Claimed1],ISNULL(SUM(factory1.Shipped),0)[Shipped1],ISNULL(SUM(factory2.Claimed),0)[Claimed2],ISNULL(SUM(factory2.Shipped),0)[Shipped2],ISNULL(SUM(factory3.Claimed),0)[Claimed3],ISNULL(SUM(factory3.Shipped),0)[Shipped3] from dbo.#temp
                    inner join @dRanges as dRanges on dRanges.starts between dRanges.starts and dRanges.ends 
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE factory=factory and dRanges.starts=month1 )AS factory1
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE factory=factory and dRanges.starts=month1 )AS factory2
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE factory=factory and dRanges.starts=month1 )AS factory3
					GROUP BY dRanges.name,#temp.Target,factory1.Claimed,factory1.Shipped,factory2.Claimed,factory2.Shipped,factory3.Claimed,factory3.Shipped,dRanges.starts
					order by dRanges.starts
                    DROP TABLE #temp");
                result = DBProxy.Current.Select("", sqcmd, out dat);

                dat.Columns.Remove("starts");
                int startIndex1 = 1;
                //最後一列Total

                DataRow totalrow1 = dat.NewRow();
                totalrow1[0] = "YTD";


                //for dt每個欄位
                decimal TTColumnAMT = 0;
                for (int colIdx = startIndex1; colIdx < dat.Columns.Count; colIdx++)
                {


                    TTColumnAMT = 0;
                    //for dat每一列
                    for (int rowIdx = 0; rowIdx < dat.Rows.Count; rowIdx++)
                    {
                        TTColumnAMT += Convert.ToDecimal(dat.Rows[rowIdx][colIdx]);
                    }

                    totalrow1[colIdx] = TTColumnAMT;
                }

                if (null == dat || dat.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found");
                }


                dat.Rows.Add(totalrow1);
                DataColumn percent3 = dat.Columns.Add("adiComp1");
                DataColumn percent4 = dat.Columns.Add("adiComp2");
                DataColumn percent5 = dat.Columns.Add("adiComp3");
                foreach (DataRow row in dat.Rows)
                {
                    percent3.SetOrdinal(4);
                    if (row["Shipped1"].Empty())
                    {
                        percent3.Expression = "0";
                    }
                    else
                    {
                        percent3.Expression = "[Claimed1] / [Shipped1]";
                    }

                    percent4.SetOrdinal(7);
                    if (row["Shipped2"].Empty())
                    {
                        percent4.Expression = "0";
                    }
                    else
                    {
                        percent4.Expression = "[Claimed2] / [Shipped2]";
                    }

                    percent5.SetOrdinal(10);
                    if (row["Shipped3"].Empty())
                    {
                        percent5.Expression = "0";
                    }
                    else
                    {
                        percent5.Expression = "[Claimed3] / [Shipped3]";
                    }
                }

                if (null == dat_All || 0 == dat_All.Rows.Count)
                {
                    dat_All = dat;
                }
                else
                {
                    dat_All.Merge(dat);
                }

                if (!result)
                {
                    return result;
                }
             #endregion
            }
            return result;//base.OnAsyncDataLoad(e);
        }
        string stringyear2;
        string stringyear3;
        string stringyear4;
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (radiobtn_byYear.Checked == true)
            {
                #region By Year
                if (dtt == null || dtt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }
                Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R40_ByYear.xltx");
                SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dt_All);
                DateTime newtodaty = DateTime.Today;
                int year1 = newtodaty.Year;
                int year2;
                int year3;
                int year4;
                int Month = newtodaty.Month;
                if (Month == 1)
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
                stringyear2 = Convert.ToString(year2);
                stringyear3 = Convert.ToString(year3);
                stringyear4 = Convert.ToString(year4);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(" ", "1,2");
                dic.Add(stringyear4, "3,5");
                dic.Add(stringyear3, "6,8");
                dic.Add(stringyear2, "9,11");
                xdt_All.lisTitleMerge.Add(dic);
                xdt_All.ShowHeader = true;
                xl.dicDatas.Add("##by_year", xdt_All);
                SaveXltReportCls.ReplaceAction a = AddRpt;
                xl.dicDatas.Add("##addrpt", a);
                xl.Save(outpath, true);
                #endregion
                
            }
            else if (radiobtn_byfactory.Checked == true)
            {
                #region By Factory 橫年度 縱月份
                //if (dt == null || dt.Rows.Count == 0)
                //{
                //    MyUtility.Msg.ErrorBox("Data not found");
                //    return false;
                //}
                if (dat == null || dat.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }
                Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R40_ByFactory.xltx");
                SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dat_All);
                DateTime newtodaty = DateTime.Today;
                int year1 = newtodaty.Year;
                int year2;
                int year3;
                int year4;
                int Month = newtodaty.Month;
                if (Month == 1)
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
                string stringyear2 = Convert.ToString(year2);
                string stringyear3 = Convert.ToString(year3);
                string stringyear4 = Convert.ToString(year4);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(" ", "1,2");
                dic.Add(stringyear4, "3,5");
                dic.Add(stringyear3, "6,8");
                dic.Add(stringyear2, "9,11");
                xdt_All.lisTitleMerge.Add(dic);
                xdt_All.ShowHeader = true;
                xl.dicDatas.Add("##by_factory", xdt_All);
                //SaveXltReportCls.ReplaceAction b = Addfactory;
                //xl.dicDatas.Add("##addfactory", b);
                xl.Save(outpath, true);
                //    Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Quality_R40_ByFactory.xltx");
                //    x1.CopySheet.Add(1, shm.Rows.Count - 1);
                //    x1.VarToSheetName = "##theorderid";

                //    List<string> ls = new List<string>();
                //    int idx = 0;
                //    foreach (DataRow row in shm.Rows)
                //    {
                //        string idxstr = (idx == 0) ? "" : idx.ToString(); //為了讓第一筆idx是空值
                //        id = row["id"].ToString();
                //        name = row["name"].ToString();
                //        A = row["A"].ToString();
                //        B = row["B"].ToString();
                //        C = row["C"].ToString();
                //        D = row["D"].ToString();
                //        string theorderid = row["theorderid"].ToString();
                //        if (!ls.Contains(theorderid)) //lis "不"包含 TheOrderID
                //            ls.Add(theorderid);
                //        x1.dicDatas.Add("##id" + idxstr, id);
                //        x1.dicDatas.Add("##name" + idxstr, name);
                //        x1.dicDatas.Add("##theorderid" + idxstr, theorderid);
                //        x1.dicDatas.Add("##A" + idxstr, A);
                //        x1.dicDatas.Add("##B" + idxstr, B);
                //        x1.dicDatas.Add("##C" + idxstr, C);
                //        x1.dicDatas.Add("##D" + idxstr, D);
                //        idx += 1;
                //    }
                //    x1.Save(outpath1, false);
                #endregion
            }

            return true;//base.OnToExcel(report);
        }

        void AddRpt(Worksheet mySheet, int rowNo, int columnNo)
        {
            #region By Year
            //改名字
            
            mySheet.Cells[2,3] ="Claimed";
            mySheet.Cells[2,4] ="Shipped";
            mySheet.Cells[2,5] ="adiComp";
            mySheet.Cells[2,6] ="Claimed";
            mySheet.Cells[2,7] ="Shipped";
            mySheet.Cells[2,8] ="adiComp";
            mySheet.Cells[2,9] ="Claimed";
            mySheet.Cells[2,10] ="Shipped";
            mySheet.Cells[2,11] ="adiComp";
           
            
            Microsoft.Office.Interop.Excel._Application myExcel = null;
            Microsoft.Office.Interop.Excel._Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            try
            {

                //在工作簿 新增一張 統計圖表，單獨放在一個分頁裡面
                mySheet.get_Range("B3", "B14").Select();
                myBook.Charts.Add(Type.Missing, Type.Missing, 1, Type.Missing);
                //選擇 統計圖表 的 圖表種類
                myBook.ActiveChart.Location(XlChartLocation.xlLocationAsObject, mySheet.Name);
                myBook.ActiveChart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLineMarkers;//插入折線圖
                //設定數據範圍

                Chart c = myBook.ActiveChart;
                SeriesCollection seriesCollection = c.SeriesCollection();
                
                Series series1 = seriesCollection.Item(1); //seriesCollection.NewSeries();
                series1.Name = "Target";
                series1.XValues = mySheet.Range["A3", "A14"];
                series1.Values = mySheet.Range["B3", "B14"];

                Series series2 = seriesCollection.NewSeries();
                series2.Name = stringyear4;
                series2.XValues = mySheet.Range["A3", "A14"];
                series2.Values = mySheet.Range["E3", "E14"];
               
                Series series3 = seriesCollection.NewSeries();
                series3.Name = stringyear3;
                series3.XValues = mySheet.Range["A3", "A14"];
                series3.Values = mySheet.Range["H3", "H14"];


                Series series4 = seriesCollection.NewSeries();
                series4.Name = stringyear2;
                series4.XValues = mySheet.Range["A3", "A14"];
                series4.Values = mySheet.Range["K3","K14"];

                //c.HasLegend = true;
                //((Microsoft.Office.Interop.Excel.ChartObject)c).Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                //((Microsoft.Office.Interop.Excel.ChartObject)mySheet.ChartObjects("Chart 1")).Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;
                               
                mySheet.Shapes.Item("Chart 1").Width = 690;   //調整圖表寬度
                mySheet.Shapes.Item("Chart 1").Height = 300;  //調整圖表高度
                mySheet.Shapes.Item("Chart 1").Top =280;      //調整圖表在分頁中的高度(上邊距) 位置
                mySheet.Shapes.Item("Chart 1").Left =3;    //調整圖表在分頁中的左右(左邊距) 位置
 
                //myBook.ActiveChart.PlotArea.Width =2000;   //調整圖表寬度
                //myBook.ActiveChart.PlotArea.Height =1500;  //調整圖表高度
                //myBook.ActiveChart.PlotArea.Top = 2000;      //調整圖表在分頁中的高度(上邊距) 位置
                //myBook.ActiveChart.PlotArea.Left =0;    //調整圖表在分頁中的左右(左邊距) 位置


                //設定 繪圖區 的 背景顏色
                myBook.ActiveChart.PlotArea.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                //設定 繪圖區 的 邊框線條樣式
                //myBook.ActiveChart.PlotArea.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                //設定 繪圖區 的 寬度
                myBook.ActiveChart.PlotArea.Width = 670;
                //設定 繪圖區 的 高度
                myBook.ActiveChart.PlotArea.Height = 250;
                //設定 繪圖區 在 圖表中的 高低位置(上邊距)
                myBook.ActiveChart.PlotArea.Top = 5;
                //設定 繪圖區 在 圖表中的 左右位置(左邊距)
                myBook.ActiveChart.PlotArea.Left = 2;
                //設定 繪圖區 的 x軸名稱下方 顯示y軸的 數據資料
                myBook.ActiveChart.HasDataTable = false;

                //設定 圖表的 背景顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Interior.ColorIndex = 10;
                //設定 圖表的 背景顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Interior.Color = ColorTranslator.ToOle(Color.White);
                //設定 圖表的 邊框顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Border.ColorIndex = 10;
                //設定 圖表的 邊框顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Border.Color = ColorTranslator.ToOle(Color.Black);
                //設定 圖表的 邊框樣式 
                myBook.ActiveChart.ChartArea.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                //設置Legend圖例
                myBook.ActiveChart.Legend.Top = 100;           //設定 圖例 的 上邊距
                myBook.ActiveChart.Legend.Left = 200;        //設定 圖例 的 左邊距
                //設定 圖例 的 背景色彩
                myBook.ActiveChart.Legend.Interior.Color = ColorTranslator.ToOle(Color.White);
                myBook.ActiveChart.Legend.Width = 55;        //設定 圖例 的 寬度
                myBook.ActiveChart.Legend.Height = 20;       //設定 圖例 的 高度
                myBook.ActiveChart.Legend.Font.Size = 11;    //設定 圖例 的 字體大小 
                myBook.ActiveChart.Legend.Font.Bold = true;  //設定 圖例 的 字體樣式=粗體
                myBook.ActiveChart.Legend.Font.Name = "細明體";//設定 圖例 的 字體字型=細明體
                myBook.ActiveChart.Legend.Position = Microsoft.Office.Interop.Excel.XlLegendPosition.xlLegendPositionBottom;//設訂 圖例 的 位置靠上 
                myBook.ActiveChart.Legend.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;//設定 圖例 的 邊框線條

                //設定 圖表 x 軸 內容
                //宣告
                Microsoft.Office.Interop.Excel.Axis xAxis = (Microsoft.Office.Interop.Excel.Axis)myBook.ActiveChart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                //設定 圖表 x軸 橫向線條 線條樣式
                //xAxis.MajorGridlines.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDashDotDot;
                //設定 圖表 x軸 橫向線條顏色__方法1
                xAxis.MajorGridlines.Border.ColorIndex = 8;
                //設定 圖表 x軸 橫向線條顏色__方法2
                xAxis.MajorGridlines.Border.Color = ColorTranslator.ToOle(Color.Black);
                xAxis.HasTitle = false;  //設定 x軸 座標軸標題 = false(不顯示)，不打就是不顯示
                xAxis.MinimumScale = 1;  //設定 x軸 數值 最小值      
                xAxis.MaximumScale = 10;  //設定 x軸 數值 最大值
                xAxis.TickLabels.Font.Name = "標楷體"; //設定 x軸 字體字型=標楷體
                xAxis.TickLabels.Font.Size = 14;       //設定 x軸 字體大小

                //設定 圖表 y軸 內容
                Microsoft.Office.Interop.Excel.Axis yAxis = (Microsoft.Office.Interop.Excel.Axis)myBook.ActiveChart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                yAxis.TickLabels.Font.Name = "標楷體"; //設定 y軸 字體字型=標楷體 
                yAxis.TickLabels.Font.Size = 14;       //設定 y軸 字體大小           
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


        void Addfactory(Worksheet mySheet, int rowNo, int columnNo)
        {
            mySheet.Cells[1, 1] = "Target";
            mySheet.Cells[2, 3] = "Claimed";
            mySheet.Cells[2, 4] = "Shipped";
            mySheet.Cells[2, 5] = "adiComp";
            mySheet.Cells[2, 6] = "Claimed";
            mySheet.Cells[2, 7] = "Shipped";
            mySheet.Cells[2, 8] = "adiComp";
            mySheet.Cells[2, 9] = "Claimed";
            mySheet.Cells[2, 10] = "Shipped";
            mySheet.Cells[2, 11] = "adiComp";
            mySheet.Cells[1, 1].Interior.Color = ColorTranslator.ToOle(Color.White);

            Microsoft.Office.Interop.Excel._Application myExcel = null;
            Microsoft.Office.Interop.Excel._Workbook myBook = null;
            myExcel = mySheet.Application;
            myBook = myExcel.ActiveWorkbook;

            try
            {

                //在工作簿 新增一張 統計圖表，單獨放在一個分頁裡面
                myBook.Charts.Add(Type.Missing, Type.Missing, 1, Type.Missing);
                //選擇 統計圖表 的 圖表種類
                myBook.ActiveChart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLineMarkers;//插入折線圖
                //設定數據範圍
                string strRange = "A1:K14";
                //設定 統計圖表 的 數據範圍內容
                myBook.ActiveChart.SetSourceData(mySheet.get_Range(strRange), Microsoft.Office.Interop.Excel.XlRowCol.xlColumns);

                Chart c = myBook.ActiveChart;
                SeriesCollection seriesCollection = c.SeriesCollection();
                Series series1 = seriesCollection.NewSeries();
                //series1.Name = "2014";
                series1.XValues = mySheet.Range["A3", "A14"];
                series1.Values = mySheet.Range["E3", "E14"];

                Series series2 = seriesCollection.NewSeries();
                //series2.Name = "2015";
                series1.XValues = mySheet.Range["A3", "A14"];
                series1.Values = mySheet.Range["H3", "H14"];

                Series series3 = seriesCollection.NewSeries();
                //series3.Name = "2016";
                series1.XValues = mySheet.Range["A3", "A14"];
                series1.Values = mySheet.Range["K3", "K14"];


                //series1.ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowPercent, true, true, false, true, true, true, true);
                //series2.ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowPercent, true, true, false, true, true, true, true);
                //series3.ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowPercent, true, true, false, true, true, true, true);

                myBook.ActiveChart.PlotArea.Width = 600;   //調整圖表寬度
                myBook.ActiveChart.PlotArea.Height = 400;  //調整圖表高度
                myBook.ActiveChart.PlotArea.Top = 400;      //調整圖表在分頁中的高度(上邊距) 位置
                myBook.ActiveChart.PlotArea.Left = 0;    //調整圖表在分頁中的左右(左邊距) 位置


                //設定 繪圖區 的 背景顏色
                myBook.ActiveChart.PlotArea.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                //設定 繪圖區 的 邊框線條樣式
                //myBook.ActiveChart.PlotArea.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;
                //設定 繪圖區 的 寬度
                myBook.ActiveChart.PlotArea.Width = 580;
                //設定 繪圖區 的 高度
                myBook.ActiveChart.PlotArea.Height = 350;
                //設定 繪圖區 在 圖表中的 高低位置(上邊距)
                myBook.ActiveChart.PlotArea.Top = 5;
                //設定 繪圖區 在 圖表中的 左右位置(左邊距)
                myBook.ActiveChart.PlotArea.Left = 2;
                //設定 繪圖區 的 x軸名稱下方 顯示y軸的 數據資料
                myBook.ActiveChart.HasDataTable = false;

                //設定 圖表的 背景顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Interior.ColorIndex = 10;
                //設定 圖表的 背景顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Interior.Color = ColorTranslator.ToOle(Color.White);
                //設定 圖表的 邊框顏色__方法1 使用colorIndex(放上色彩索引)
                myBook.ActiveChart.ChartArea.Border.ColorIndex = 10;
                //設定 圖表的 邊框顏色__方法2 使用color(放入色彩名稱)
                myBook.ActiveChart.ChartArea.Border.Color = ColorTranslator.ToOle(Color.Black);
                //設定 圖表的 邊框樣式 
                myBook.ActiveChart.ChartArea.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                //設置Legend圖例
                myBook.ActiveChart.Legend.Top = 0;           //設定 圖例 的 上邊距
                myBook.ActiveChart.Legend.Left = 200;        //設定 圖例 的 左邊距
                //設定 圖例 的 背景色彩
                myBook.ActiveChart.Legend.Interior.Color = ColorTranslator.ToOle(Color.White);
                myBook.ActiveChart.Legend.Width = 55;        //設定 圖例 的 寬度
                myBook.ActiveChart.Legend.Height = 20;       //設定 圖例 的 高度
                myBook.ActiveChart.Legend.Font.Size = 11;    //設定 圖例 的 字體大小 
                myBook.ActiveChart.Legend.Font.Bold = true;  //設定 圖例 的 字體樣式=粗體
                myBook.ActiveChart.Legend.Font.Name = "細明體";//設定 圖例 的 字體字型=細明體
                myBook.ActiveChart.Legend.Position = Microsoft.Office.Interop.Excel.XlLegendPosition.xlLegendPositionBottom;//設訂 圖例 的 位置靠上 
                //myBook.ActiveChart.Legend.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;//設定 圖例 的 邊框線條

                //設定 圖表 x 軸 內容
                //宣告
                Microsoft.Office.Interop.Excel.Axis xAxis = (Microsoft.Office.Interop.Excel.Axis)myBook.ActiveChart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                //設定 圖表 x軸 橫向線條 線條樣式
                //xAxis.MajorGridlines.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDashDotDot;
                //設定 圖表 x軸 橫向線條顏色__方法1
                xAxis.MajorGridlines.Border.ColorIndex = 8;
                //設定 圖表 x軸 橫向線條顏色__方法2
                xAxis.MajorGridlines.Border.Color = ColorTranslator.ToOle(Color.Black);
                xAxis.HasTitle = false;  //設定 x軸 座標軸標題 = false(不顯示)，不打就是不顯示
                xAxis.MinimumScale = 1;  //設定 x軸 數值 最小值      
                xAxis.MaximumScale = 10;  //設定 x軸 數值 最大值
                xAxis.TickLabels.Font.Name = "標楷體"; //設定 x軸 字體字型=標楷體
                xAxis.TickLabels.Font.Size = 14;       //設定 x軸 字體大小

                //設定 圖表 y軸 內容
                Microsoft.Office.Interop.Excel.Axis yAxis = (Microsoft.Office.Interop.Excel.Axis)myBook.ActiveChart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                yAxis.TickLabels.Font.Name = "標楷體"; //設定 y軸 字體字型=標楷體 
                yAxis.TickLabels.Font.Size = 14;       //設定 y軸 字體大小        
            }
            catch (Exception e)
            {
                this.ShowErr(e);
                myExcel.Visible = true;
            }
            finally
            {
                //把執行的Excel資源釋放
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
                myExcel = null;
                myBook = null;
                mySheet = null;
            }
        }


    }
}
