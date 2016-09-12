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
					GROUP BY dRanges.name,#temp.Target,year1.Claimed,year1.Shipped,year1.adiComp,year2.Claimed,year2.Shipped,year2.adiComp,year3.Claimed,year3.Shipped,year3.adiComp
                    DROP TABLE #temp");
                result = DBProxy.Current.Select("", sqlcmd, out dtt);

                int startIndex = 2;
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

                dtt.Rows.Add(totalrow);
                DataColumn percent = dtt.Columns.Add("adiComp");
                percent.SetOrdinal(2);
                percent.Expression = string.Format("[收款金額] / {0}", dtt.Rows[dtt.Rows.Count - 1]["收款金額"]);
                for (int i = 0; i < kpiCodes.Rows.Count; i++)
                {
                    percent = dtt.Columns.Add("Range-" + (i + 1));
                    percent.SetOrdinal(4 + (i * 2));
                    percent.Expression = "[" + kpiCodes.Rows[i]["name"].ToString() + "] / [收款金額]";
                }


                dtt.Rows.Add(totalrow);

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
                    select Target,Claimed.Claimed,sh.qty[Shipped],isnull(round(sum(Claimed.Claimed)/sum(sh.qty),6)*100,0) [adiComp],Claimed.month1,Claimed.YEAR1,Claimed.factory
                    into #temp
                    from dbo.ADIDASComplainTarget 
                    outer apply(SELECT left(cast(a.StartDate as varchar(10)),7) ym , sum(b.Qty) Claimed,dateadd(MONTH,-3,a.StartDate) themonth ,MONTH(a.StartDate)[month1],YEAR(a.StartDate) [YEAR1],b.FactoryID [factory]
			                    FROM dbo.ADIDASComplain a
			                    INNER JOIN DBO.ADIDASComplain_Detail b ON B.ID = a.ID
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate,B.FactoryID) Claimed
                    outer apply(SELECT dateadd(MONTH,5,Claimed.themonth) as six,YEAR(a.StartDate)[YEAR1]
			                    FROM dbo.ADIDASComplain a
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate) as ff 
                    outer apply (SELECT ISNULL(SUM(a.Qty),0)/6 AS Qty FROM ADIDASComplain_MonthlyQty a
			                    WHERE a.YearMonth BETWEEN claimed.themonth AND ff.six
			                    AND a.BrandID = 'ADIDAS'
								AND a.factoryid in (select id from dbo.SCIFty 
			                    where CountryID= (select f.CountryID from dbo.Factory f where f.id="+"'"+userfactory+"'"+ @")))sh
                    where year in (@y1,@y2,@y3)
                    group by Target,Claimed.Claimed,sh.qty,Claimed.month1,Claimed.YEAR1,Claimed.factory
                    select dRanges.name,#temp.Target,SUM(year1.Claimed)[Claimed],SUM(year1.Shipped)[Shipped],SUM(year1.adiComp)[adiComp],SUM(year2.Claimed)[Claimed],SUM(year2.Shipped)[Shipped],SUM(year2.adiComp)[adiComp],SUM(year3.Claimed)[Claimed],SUM(year3.Shipped)[Shipped],SUM(year3.adiComp)[adiComp],#temp.factory from dbo.#temp
                    inner join @dRanges as dRanges on #temp.month1 between dRanges.starts and dRanges.ends 
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE YEAR1=@y1 AND factory=factory)AS year1
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE YEAR1=@y2 AND factory=factory)AS year2
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE YEAR1=@y3 AND factory=factory)AS year3
					GROUP BY dRanges.name,#temp.Target,year1.Claimed,year1.Shipped,year1.adiComp,year2.Claimed,year2.Shipped,year2.adiComp,year3.Claimed,year3.Shipped,year3.adiComp,#temp.factory
                    DROP TABLE #temp");
                result = DBProxy.Current.Select("", scmd, out dt);
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
                    select Target,Claimed.Claimed,sh.qty[Shipped],isnull(round(sum(Claimed.Claimed)/sum(sh.qty),6)*100,0) [adiComp],Claimed.month1,Claimed.YEAR1,Claimed.factory
                    into #temp
                    from dbo.ADIDASComplainTarget 
                    outer apply(SELECT left(cast(a.StartDate as varchar(10)),7) ym , sum(b.Qty) Claimed,dateadd(MONTH,-3,a.StartDate) themonth ,MONTH(a.StartDate)[month1],YEAR(a.StartDate) [YEAR1],b.FactoryID [factory]
			                    FROM dbo.ADIDASComplain a
			                    INNER JOIN DBO.ADIDASComplain_Detail b ON B.ID = a.ID
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate,B.FactoryID) Claimed
                    outer apply(SELECT dateadd(MONTH,5,Claimed.themonth) as six,YEAR(a.StartDate)[YEAR1]
			                    FROM dbo.ADIDASComplain a
			                    where a.StartDate in (@y1,@y2,@y3)
			                    group by a.StartDate) as ff 
                    outer apply (SELECT ISNULL(SUM(a.Qty),0)/6 AS Qty FROM ADIDASComplain_MonthlyQty a
			                    WHERE a.YearMonth BETWEEN claimed.themonth AND ff.six
			                    AND a.BrandID = 'ADIDAS'
								AND a.factoryid in (select id from dbo.SCIFty 
			                    where CountryID= (select f.CountryID from dbo.Factory f where f.id="+ "'" + userfactory + "'" + @")))sh
                    where year in (@y1,@y2,@y3)
                    group by Target,Claimed.Claimed,sh.qty,Claimed.month1,Claimed.YEAR1,Claimed.factory
                    select dRanges.name,#temp.Target,SUM(factory1.Claimed)[Claimed],SUM(factory1.Shipped)[Shipped],SUM(factory1.adiComp)[adiComp],SUM(factory2.Claimed)[Claimed],SUM(factory2.Shipped)[Shipped],SUM(factory2.adiComp)[adiComp],SUM(factory3.Claimed)[Claimed],SUM(factory3.Shipped)[Shipped],SUM(factory3.adiComp)[adiComp],#temp.factoryfrom dbo.#temp
                    inner join @dRanges as dRanges on #temp.month1 between dRanges.starts and dRanges.ends 
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE factory=factory )AS factory1
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE factory=factory )AS factory2
					OUTER APPLY(SELECT Claimed,#temp.Shipped,#temp.adiComp  FROM #temp WHERE factory=factory )AS factory3
					GROUP BY dRanges.name,#temp.Target,factory1.Claimed,factory1.Shipped,factory1.adiComp,factory2.Claimed,factory2.Shipped,factory2.adiComp,factory3.Claimed,factory3.Shipped,factory3.adiComp,#temp.factory
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
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            if (dat == null || dat.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            if (radiobtn_byYear.Checked == true)
            {
                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }
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
            //    Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R26_Shipping_Mark.xltx");
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

    }
}
