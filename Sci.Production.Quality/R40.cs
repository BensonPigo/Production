using Ict;
using Sci.Data;
using Sci.Utility.Excel;
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
        DualResult result;
        DataTable dtt;
        DataTable dt;
        DataTable dat;       
        DataTable dt_All;
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

                    select dRanges.name,dRanges.starts,#temp.Target,isnull(SUM(year1.Claimed),0)[Claimed1],isnull(SUM(year1.Shipped),0)[Shipped1],isnull(SUM(year2.Claimed),0)[Claimed2],isnull(SUM(year2.Shipped),0)[Shipped2],isnull(SUM(year3.Claimed),0)[Claimed3],isnull(SUM(year3.Shipped),0)[Shipped3] from dbo.#temp
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
               DataColumn percent2= dtt.Columns.Add("adiComp3");
               DataColumn percent3 = dtt.Columns.Add(" ");
                foreach( DataRow row in dtt.Rows)
                {
                    if (row["Shipped1"].Empty())
                    {
                        percent.SetOrdinal(4);
                        percent.Expression = "0";
                    }
                    else
                    {   
                        percent.SetOrdinal(4);
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
                string scmd = string.Format(@"declare @dRanges table(starts int , ends int, name varchar(3))
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
                    select dRanges.name,#temp.Target,SUM(year1.Claimed)[Claimed1],SUM(year1.Shipped)[Shipped1],SUM(year2.Claimed)[Claimed2],SUM(year2.Shipped)[Shipped2],SUM(year3.Claimed)[Claimed3],SUM(year3.Shipped)[Shipped3],#temp.factory from dbo.#temp
                    inner join @dRanges as dRanges on #temp.month1 between dRanges.starts and dRanges.ends 
					OUTER APPLY(SELECT Claimed,#temp.Shipped  FROM #temp WHERE YEAR1=@y1 AND factory=factory)AS year1
					OUTER APPLY(SELECT Claimed,#temp.Shipped  FROM #temp WHERE YEAR1=@y2 AND factory=factory)AS year2
					OUTER APPLY(SELECT Claimed,#temp.Shipped  FROM #temp WHERE YEAR1=@y3 AND factory=factory)AS year3
					GROUP BY dRanges.name,#temp.Target,year1.Claimed,year1.Shipped,year2.Claimed,year2.Shipped,year3.Claimed,year3.Shipped,#temp.factory
                    DROP TABLE #temp");
                result = DBProxy.Current.Select("", scmd, out dt);

                int startIndex = 2;
                //最後一列Total

                DataRow totalrow = dt.NewRow();
                totalrow[0] = "YTD";

                //for dt每個欄位
                decimal TTColumnAMT = 0;
                for (int colIdx = startIndex; colIdx < dt.Columns.Count; colIdx++)
                {
                    TTColumnAMT = 0;
                    //for dt每一列
                    for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                    {
                        TTColumnAMT += Convert.ToDecimal(dt.Rows[rowIdx][colIdx]);
                    }

                    totalrow[colIdx] = TTColumnAMT;
                }

                if (null == dt || dt.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found");
                }


                dt.Rows.Add(totalrow);
                DataColumn percent = dt.Columns.Add("adiComp");
                percent.SetOrdinal(4);
                percent.Expression = "[Claimed1] / [Shipped1]";
                percent.SetOrdinal(7);
                percent.Expression = "[Claimed2] / [Shipped2]";
                percent.SetOrdinal(10);
                percent.Expression = "[Claimed3] / [Shipped3]";

                dt.Rows.Add(totalrow);

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
                }
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
                    select dRanges.name,#temp.Target,SUM(factory1.Claimed)[Claimed],SUM(factory1.Shipped)[Shipped],SUM(factory2.Claimed)[Claimed],SUM(factory2.Shipped)[Shipped],SUM(factory3.Claimed)[Claimed],SUM(factory3.Shipped)[Shipped],#temp.factory from dbo.#temp
                    inner join @dRanges as dRanges on #temp.month1 between dRanges.starts and dRanges.ends 
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE factory=factory )AS factory1
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE factory=factory )AS factory2
					OUTER APPLY(SELECT Claimed,#temp.Shipped FROM #temp WHERE factory=factory )AS factory3
					GROUP BY dRanges.name,#temp.Target,factory1.Claimed,factory1.Shipped,factory2.Claimed,factory2.Shipped,factory3.Claimed,factory3.Shipped,#temp.factory
                    DROP TABLE #temp");
                result = DBProxy.Current.Select("", sqcmd, out dat);
                if (!result)
                {
                    return result;
                }
                #endregion
            }
            return result;//base.OnAsyncDataLoad(e);
        }
        
   protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dtt == null || dtt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            //if (dt == null || dt.Rows.Count == 0)
            //{
            //    MyUtility.Msg.ErrorBox("Data not found");
            //    return false;
            //}
            //if (dat == null || dat.Rows.Count == 0)
            //{
            //    MyUtility.Msg.ErrorBox("Data not found");
            //    return false;
            //}

            if (radiobtn_byYear.Checked == true)
            {
               
                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }
                Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R40_ByYear.xltx");
                SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dt_All);
                DateTime newtodaty =DateTime.Today;
                int year1 =newtodaty.Year;
                int year2;
                int year3;
                int year4;
                int Month =newtodaty.Month;
                if(Month==1)
                {
                    year2=year1-1;
                    year3=year1-2;
                    year4=year1-3;
                }else
                {
                    year2=year1;
                    year3=year1-1;
                    year4=year1-2;
                }
                string  stringyear2=Convert.ToString(year2); 
                string  stringyear3=Convert.ToString(year3);
                string  stringyear4=Convert.ToString(year4);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(" ","1,2");
                dic.Add(stringyear4,"3,5");
                dic.Add(stringyear3, "6,8");
                dic.Add(stringyear2, "9,11");
                xdt_All.lisTitleMerge.Add(dic);

                xdt_All.ShowHeader = true;


                xl.dicDatas.Add("##by_year", xdt_All);
                xl.Save(outpath, true);
            }
            else if(radiobtn_byfactory.Checked==true)
            {

                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }
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

            }

            return true;//base.OnToExcel(report);
        }

   private void button1_Click(object sender, EventArgs e)
   {
       Microsoft.Office.Interop.Excel._Application myExcel = null;
       Microsoft.Office.Interop.Excel._Workbook myBook = null;
       Microsoft.Office.Interop.Excel._Worksheet mySheet = null;

       try
       {
           myExcel = new Microsoft.Office.Interop.Excel.Application();    //開啟一個新的應用程式
           myExcel.DisplayAlerts = false;        //停用警告訊息
           myBook = myExcel.Workbooks.Add(true); //新增活頁簿
           mySheet = (Microsoft.Office.Interop.Excel._Worksheet)myBook.Worksheets[1];//引用第一張工作表
           myExcel.Visible = true;               //顯示Excel程式

           mySheet.Cells.Font.Name = "標楷體";   //設定Excel資料字體字型
           mySheet.Cells.Font.Size = 20;         //設定Excel資料字體大小

           //Excel寫入資料
          
           //mySheet.Cells[1, 1] = "A";
           //mySheet.Cells[1, 2] = "5";
           //mySheet.Cells[1, 3] = "ㄅ";

           //mySheet.Cells[2, 1] = "B";
           //mySheet.Cells[2, 2] = "6";
           //mySheet.Cells[2, 3] = "ㄆ";

           //mySheet.Cells[3, 1] = "C";
           //mySheet.Cells[3, 2] = "7";
           //mySheet.Cells[3, 3] = "ㄇ";

           //mySheet.Cells[4, 1] = "D";
           //mySheet.Cells[4, 2] = "8";
           //mySheet.Cells[4, 3] = "ㄈ";


           //在工作簿 新增一張 統計圖表，單獨放在一個分頁裡面
           myBook.Charts.Add(Type.Missing, Type.Missing, 1, Type.Missing);
           //選擇 統計圖表 的 圖表種類
           myBook.ActiveChart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLineMarkers;//插入折線圖
           //設定數據範圍
           string strRange = "A1:K14";
           //設定 統計圖表 的 數據範圍內容
           myBook.ActiveChart.SetSourceData(mySheet.get_Range(strRange), Microsoft.Office.Interop.Excel.XlRowCol.xlColumns);
           //將新增的統計圖表 插入到 指定位置(可以從單獨的分頁放到一個分頁裡面)
           myBook.ActiveChart.Location(Microsoft.Office.Interop.Excel.XlChartLocation.xlLocationAsObject, mySheet.Name);

           mySheet.Shapes.Item("Chart 1").Width = 600;   //調整圖表寬度
           mySheet.Shapes.Item("Chart 1").Height = 400;  //調整圖表高度
           mySheet.Shapes.Item("Chart 1").Top = 400;      //調整圖表在分頁中的高度(上邊距) 位置
           mySheet.Shapes.Item("Chart 1").Left = 0;    //調整圖表在分頁中的左右(左邊距) 位置

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

           //設定 圖表 標題 顯示 = false(關閉)
           myBook.ActiveChart.HasTitle = false;
           //設定 圖表 標題 = 匯率
           myBook.ActiveChart.ChartTitle.Text = "匯率";
           //設定 圖表 標題 陰影 = false(關閉)
           myBook.ActiveChart.ChartTitle.Shadow = false;
           //設定 圖表 標題 邊框樣式
           //myBook.ActiveChart.ChartTitle.Border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDash;

           ////選擇統計圖表的 圖表種類=3D類型的統計圖表 Floor才可以使用
           //myBook.ActiveChart.ChartType = Excel.XlChartType.xl3DColumn;//插入3D統計圖表
           ////設定 圖表的 Floor顏色__方法1 使用colorIndex(放上色彩索引)
           //myBook.ActiveChart.Floor.Interior.ColorIndex = 1;
           ////設定 圖表的 Floor顏色__方法2 使用color(放入色彩名稱)
           //myBook.ActiveChart.Floor.Interior.Color = ColorTranslator.ToOle(Color.LightGreen);           
       }
       catch (Exception)
       {
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

   private void button2_Click(object sender, EventArgs e)
   {
       
   }
}
}
