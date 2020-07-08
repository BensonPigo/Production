using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R07
    /// </summary>
    public partial class R07 : Win.Tems.PrintForm
    {
        private int _year;
        private int _month;
        private string _mDivision;
        private string _factory;
        private DataTable _printData;
        private DateTime _startDate;

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Sci.Env.User.Keyword;
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.numericUpDownYear.Value = MyUtility.Convert.GetInt(DateTime.Today.ToString("yyyy"));
            this.numericUpDownMonth.Value = MyUtility.Convert.GetInt(DateTime.Today.ToString("MM"));
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.numericUpDownYear.Value))
            {
                MyUtility.Msg.WarningBox("Year can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.numericUpDownMonth.Value))
            {
                MyUtility.Msg.WarningBox("Month can't empty!!");
                return false;
            }

            this._year = (int)this.numericUpDownYear.Value;
            this._month = (int)this.numericUpDownMonth.Value;
            this._mDivision = this.comboM.Text;
            this._factory = this.comboFactory.Text;
            this._startDate = Convert.ToDateTime(string.Format("{0}/{1}/1", MyUtility.Convert.GetString(this._year), MyUtility.Convert.GetString(this._month)));

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            sqlCmd.Append(string.Format(
                @"
DECLARE @sewinginline DATETIME ='{0}'
DECLARE @sewingoffline DATETIME ='{1}'
DECLARE @MDivisionID Nvarchar(8)= '{2}'
DECLARE @FactoryID Nvarchar(8)= '{3}'
--整個月 table
;WITH cte AS (
    SELECT [date] = @sewinginline
    UNION ALL
    SELECT [date] + 1 FROM cte WHERE ([date] < DATEADD(DAY,-1,@sewingoffline))
)
SELECT [date] = cast([date] as date) 
into #daterange 
FROM cte
--WorkHour table
select FactoryID,SewingLineID,Date,Hours,Holiday
into #workhourtmp
from WorkHour w
where (FactoryID = @FactoryID or @FactoryID ='')
and Date between @sewinginline and DATEADD(DAY,1,@sewingoffline)
--order by Date
--準備一整個月workhour的資料判斷Holiday
select distinct d.date,w.FactoryID,w.SewingLineID
into #tmpd
from #daterange d,#workhourtmp w
--
select distinct d.FactoryID,d.SewingLineID,d.date,Holiday = iif(w.Holiday is null or w.Holiday = 1 or w.Hours = 0,1,0)
into #Holiday
from #tmpd d
left join #workhourtmp w on d.date = w.Date and d.FactoryID = w.FactoryID and d.SewingLineID = w.SewingLineID
order by FactoryID,SewingLineID,date
--先將符合條件的Sewing schedule撈出來
select
	 s.FactoryID
	,s.SewingLineID
	,o.StyleID
	,Inline = cast(s.Inline as date)
	,Offline = cast(s.Offline as date)
	,OrderTypeID = isnull(o.OrderTypeID,'')
	,o.SciDelivery
	,o.BuyerDelivery
	,Category = isnull(o.Category,'')
	,o.CdCodeID
into #Sewtmp
from SewingSchedule s WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on s.OrderID = o.ID
left join Style st WITH (NOLOCK) on st.Ukey = o.StyleUkey
where (s.Inline between  @sewinginline and @sewingoffline 
    or s.Offline between @sewinginline and @sewingoffline
	or @sewinginline between s.Inline and s.Offline
	or @sewingoffline between s.Inline and s.Offline
)
 and s.MDivisionID = @MDivisionID --and (s.FactoryID = @FactoryID or @FactpryID = '') --and SewingLineID = '18'

select distinct
	d.FactoryID
	,d.SewingLineID
	,d.date
	,s.StyleID
	,IsLastMonth = iif(s.SciDelivery < @sewinginline,1,0)--紫 優先度1
	,IsNextMonth = iif(s.SciDelivery > DateAdd(DAY,-1,@sewingoffline),1,0)--綠 優先度2
	,IsBulk = iif(s.Category = 'B',1,0)--藍 優先度3
	,IsSMS = iif(s.OrderTypeID = 'SMS',1,0)--紅 優先度4
	,s.BuyerDelivery
	,s.CdCodeID
into #Stmp
from #Sewtmp s 
left join #tmpd d on d.FactoryID = s.FactoryID and d.SewingLineID = s.SewingLineID and d.date between s.Inline and s.Offline
--order by h.FactoryID,h.SewingLineID,h.date,s.StyleID
select distinct FactoryID,SewingLineID,date,StyleID = a.s
into #ConcatStyle
from #Stmp s
outer apply(
	select s =(
		select distinct concat(StyleID,'(',CdCodeID,')',';')
		from #Stmp s2
		where s2.FactoryID = s.FactoryID and s2.SewingLineID = s.SewingLineID and s2.date = s.date
		for xml path('')
	)
)a
--
select h.FactoryID,h.SewingLineID,h.date,h.Holiday,IsLastMonth,IsNextMonth,IsBulk,IsSMS,BuyerDelivery
into #c
from #Holiday h
left join #Stmp s on s.FactoryID = h.FactoryID and s.SewingLineID = h.SewingLineID and s.date = h.date
inner join(select distinct FactoryID,SewingLineID from #Stmp) x on x.FactoryID = h.FactoryID and x.SewingLineID = h.SewingLineID--排掉沒有在SewingSchedule內的資料by FactoryID,SewingLineID
order by h.FactoryID,h.SewingLineID,h.date
------------------------------------------------------------------------------------------------
DECLARE cursor_sewingschedule CURSOR FOR
select distinct c.FactoryID,c.SewingLineID,c.date
	,StyleID = isnull(iif(c.Holiday = 1,'Holiday', cs.StyleID),'')
	,IsLastMonth,IsNextMonth,IsBulk,IsSMS,BuyerDelivery
from #c c left join #ConcatStyle cs on c.FactoryID = cs.FactoryID and c.SewingLineID = cs.SewingLineID and c.date = cs.date
order by c.FactoryID,c.SewingLineID,c.date

--建立tmpe table存放最後要列印的資料
DECLARE @tempPintData TABLE (
   FactoryID VARCHAR(8),
   SewingLineID VARCHAR(2),
   StyleID VARCHAR(MAX),
   InLine DATE,
   OffLine DATE,
   IsBulk BIT,
   IsSMS BIT,
   IsLastMonth BIT,
   IsNextMonth BIT,
   MinBuyerDelivery DATE
)
--
DECLARE @factory VARCHAR(8),
		@sewingline VARCHAR(2),
		@StyleID VARCHAR(200),
		@IsLastMonth int,
		@IsNextMonth int,
		@IsBulk int,
		@IsSMS int,
		@BuyerDelivery DATE,
		@date DATE,
		@beforefactory VARCHAR(8) = '',
		@beforesewingline VARCHAR(2) = '',
		@beforeStyleID VARCHAR(200) = '',
		@beforeIsLastMonth int,
		@beforeIsNextMonth int,
		@beforeIsBulk int,
		@beforeIsSMS int,
		@beforeBuyerDelivery DATE,
		@beforedate DATE

OPEN cursor_sewingschedule
FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@date,@StyleID,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSMS,@BuyerDelivery
WHILE @@FETCH_STATUS = 0
BEGIN
	
	IF @factory <> @beforefactory or @sewingline <> @beforesewingline or @StyleID <> @beforeStyleID
	Begin
		INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine,OffLine,IsLastMonth ,IsNextMonth ,IsBulk ,IsSMS ,MinBuyerDelivery) 
		VALUES					 (@factory, @sewingline, @StyleID,@date,@date  ,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSMS,@BuyerDelivery);
	END
	ELSE
	Begin
		update @tempPintData set
			 OffLine = @date
			,IsLastMonth = iif(IsLastMonth = 1, IsLastMonth, @IsLastMonth)
			,IsNextMonth = iif(IsNextMonth = 1, IsNextMonth, @IsNextMonth)
			,IsBulk = iif(IsBulk = 1, IsBulk, @IsBulk)
			,IsSMS = iif(IsSMS = 1, IsSMS, @IsSMS)
			,MinBuyerDelivery = iif(MinBuyerDelivery < @BuyerDelivery,MinBuyerDelivery,@BuyerDelivery)
		where FactoryID = @factory and SewingLineID = @sewingline and StyleID = @StyleID and OffLine = @beforedate
	END
	
	set @beforefactory = @factory
	set @beforesewingline = @sewingline
	set @beforeStyleID = @StyleID
	set @beforeIsLastMonth = @IsLastMonth
	set @beforeIsNextMonth = @IsNextMonth
	set @beforeIsBulk = @IsBulk
	set @beforeIsSMS = @IsSMS
	set @beforeBuyerDelivery = @BuyerDelivery
	set @beforedate = @date
	FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@date,@StyleID,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSMS,@BuyerDelivery
END
CLOSE cursor_sewingschedule
DEALLOCATE cursor_sewingschedule

select * from @tempPintData where StyleID<>''

drop table #daterange,#tmpd,#Holiday,#Sewtmp,#workhourtmp,#Stmp,#c,#ConcatStyle",
                this._startDate.ToString("d"),
                this._startDate.AddMonths(1).ToString("d"),
                this._mDivision,
                this._factory));
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this._printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData.Rows.Count);

            if (this._printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R07_SewingScheduleGanttChart.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            excel.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 1;
            string writeFty = string.Empty;
            string line = string.Empty;
            int ftyCount = 0;
            int colCount = 1;
#if DEBUG
            excel.Visible = true;
#endif
            int monthDays = this._startDate.AddMonths(1).AddDays(-1).Subtract(this._startDate).Days + 1;

            foreach (DataRow dr in this._printData.Rows)
            {
                // 當有換工廠別時，要換Sheet
                if (writeFty != MyUtility.Convert.GetString(dr["FactoryID"]))
                {
                    if (ftyCount > 0)
                    {
                        // 將上一間工廠最後一條Line後面沒有Schedule的格子塗黑
                        if (colCount - 1 != monthDays)
                        {
                            for (int i = colCount; i <= monthDays; i++)
                            {
                                worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                            }
                        }

                        // 刪除多出來的資料行
                        for (int i = 1; i <= 2; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[intRowsStart + 1, Type.Missing];
                            rng.Select();
                            rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                            Marshal.ReleaseComObject(rng);
                        }
                    }

                    ftyCount++;
                    worksheet = excel.ActiveWorkbook.Worksheets[ftyCount];
                    worksheet.Select();
                    worksheet.Name = MyUtility.Convert.GetString(dr["FactoryID"]);
                    intRowsStart = 1;
                    writeFty = MyUtility.Convert.GetString(dr["FactoryID"]);
                    if (monthDays < 31)
                    {
                        for (int i = monthDays + 2; i <= 32; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Columns[monthDays + 2, Type.Missing];
                            rng.Select();
                            rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                            Marshal.ReleaseComObject(rng);
                        }
                    }
                }

                // 換Sewing Line時，要填入Line#
                if (line != MyUtility.Convert.GetString(dr["SewingLineID"]))
                {
                    if (intRowsStart > 1)
                    {
                        // 將後面沒有Schedule的格子塗黑
                        if (colCount - 1 != monthDays)
                        {
                            for (int i = colCount; i <= monthDays; i++)
                            {
                                worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                            }
                        }
                    }

                    intRowsStart++;
                    colCount = 1;

                    // 先插入一行
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(intRowsStart + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);

                    line = MyUtility.Convert.GetString(dr["SewingLineID"]);
                    worksheet.Cells[intRowsStart, 1] = line;
                }

                // 算上下線日的總天數，用來做合併儲存格
                DateTime sewingStartDate = Convert.ToDateTime(dr["Inline"]);
                DateTime sewingEndDate = !MyUtility.Check.Empty(dr["Offline"]) ? Convert.ToDateTime(dr["Offline"]) : this._startDate.AddMonths(1).AddDays(-1);
                int startCol = sewingStartDate.Subtract(this._startDate).Days + 2;
                int endCol = sewingEndDate.Subtract(this._startDate).Days + 2;
                int totalDays = sewingEndDate.Subtract(sewingStartDate).Days + 1;

                // 將中間沒有Schedule的格子塗黑
                if (colCount + 1 != startCol)
                {
                    for (int i = colCount + 1; i < startCol; i++)
                    {
                        worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                    }
                }

                // 算出Excel的Column的英文位置
                string excelStartColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(startCol);
                string excelEndColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(endCol);
                Microsoft.Office.Interop.Excel.Range selrng = worksheet.get_Range(string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng), Type.Missing).EntireRow;

                // 合併儲存格,文字置中
                worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Merge(Type.Missing);
                worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // 顏色顯示優先權：Holiday紅色背景 > SCI Delivery為當月以前的紫色粗體 > SCI Delivery當月為以後的綠色粗體 > Bulk款式藍色粗體 > SMS款式紅色粗體 > 非以上四種情況的黑色字體
                #region 填入內容值
                if (MyUtility.Convert.GetString(dr["StyleID"]) == "Holiday")
                {
                    // 設置儲存格的背景色
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = System.Drawing.Color.Red;
                }
                else if (MyUtility.Convert.GetString(dr["IsLastMonth"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Purple;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d"));
                }
                else if (MyUtility.Convert.GetString(dr["IsNextMonth"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Green;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d"));
                }
                else if (MyUtility.Convert.GetString(dr["IsBulk"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Blue;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d"));
                }
                else if (MyUtility.Convert.GetString(dr["IsSMS"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Red;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d"));
                }
                else
                {
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Bold = false;
                    worksheet.Range[string.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Black;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d"));
                }
                #endregion
                colCount = colCount + (startCol - colCount - 1) + totalDays;
                Marshal.ReleaseComObject(selrng);
            }

            if (colCount - 1 != monthDays)
            {
                for (int i = colCount; i <= monthDays; i++)
                {
                    worksheet.Range[string.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                }
            }

            // 刪除多出來的資料行
            for (int i = 1; i <= 2; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[intRowsStart + 1, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            // 刪除多的Sheet
            for (int i = ftyCount + 1; i <= 10; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[ftyCount + 1];
                worksheet.Delete();
            }

            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Select();

            this.HideWaitMessage();
            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R07_SewingScheduleGanttChart");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
