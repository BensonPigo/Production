using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        int year, month;
        string mDivision, factory;
        DataTable printData;
        DateTime startDate;
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox2, 1, factory);
            comboBox1.Text = Sci.Env.User.Keyword;
            comboBox2.Text = Sci.Env.User.Factory;
            numericUpDown1.Value = MyUtility.Convert.GetInt(DateTime.Today.ToString("yyyy"));
            numericUpDown2.Value = MyUtility.Convert.GetInt(DateTime.Today.ToString("MM"));

        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(numericUpDown1.Value))
            {
                MyUtility.Msg.WarningBox("Year can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(numericUpDown2.Value))
            {
                MyUtility.Msg.WarningBox("Month can't empty!!");
                return false;
            }
            year = (int)numericUpDown1.Value;
            month = (int)numericUpDown2.Value;
            mDivision = comboBox1.Text;
            factory = comboBox2.Text;
            startDate = Convert.ToDateTime(string.Format("{0}/{1}/1", MyUtility.Convert.GetString(year), MyUtility.Convert.GetString(month)));

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            sqlCmd.Append(string.Format(@"DECLARE @sewinginline DATE,
		@sewingoffline DATE
SET @sewinginline = '{0}'
SET @sewingoffline = '{1}'

--先將符合條件的Sewing schedule撈出來
DECLARE cursor_sewingschedule CURSOR FOR
select s.FactoryID,s.SewingLineID,s.Inline,s.Offline,isnull(o.StyleID,'') as StyleID,
isnull(o.OrderTypeID,'') as OrderTypeID,o.SciDelivery,o.BuyerDelivery,
isnull(o.Category,'') as Category,isnull(st.CdCodeID,'') as CdCodeID
from SewingSchedule s
left join Orders o on s.OrderID = o.ID
left join Style st on st.Ukey = o.StyleUkey
where (s.Inline between @sewinginline and @sewingoffline or s.Offline between @sewinginline and @sewingoffline)
", startDate.ToString("d"), startDate.AddMonths(1).ToString("d")));
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'",mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'",factory));
            }
                sqlCmd.Append(@"
order by s.FactoryID,s.SewingLineID,s.Inline,o.StyleID

--建立tmpe table存放展開成每一天的資料
DECLARE @tempEveryData TABLE (
   FactoryID VARCHAR(8),
   SewingLineID VARCHAR(2),
   StyleID VARCHAR(15),
   SewingDate DATE,
   OrderTypeID VARCHAR(20),
   SCIDelivery DATE,
   BuyerDellivery DATE,
   Category VARCHAR(1),
   CDCodeID VARCHAR(6)
)
--宣告變數: 記錄程式中的資料
DECLARE @factory VARCHAR(8),
		@sewingline VARCHAR(2),
        @inline DATETIME,
		@offline DATETIME,
		@style VARCHAR(15),
		@ordertype VARCHAR(20),
		@scidlv DATE,
		@buyerdlv DATE,
		@category VARCHAR(1),
		@cdcode VARCHAR(6),
		@_i INT, --計算迴圈用
		@sewingdate DATE, --Sewing Date
		@workhour INT --Work Hour

--將資料展開成每一天
OPEN cursor_sewingschedule
FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@inline,@offline,@style,@ordertype,@scidlv,@buyerdlv,@category,@cdcode
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @_i = 0
	SET @sewingdate = IIF(@inline < @sewinginline,@sewinginline,@inline)
	WHILE (@_i <= DATEDIFF(DAY,IIF(@inline < @sewinginline,@sewinginline,@inline),IIF(@offline > @sewingoffline,DATEADD(DAY,-1,@sewingoffline),@offline)))
	BEGIN
		select @workhour = Hours from WorkHour where SewingLineID = @sewingline and Date = @sewingdate and FactoryID = @factory and Hours > 0
		IF @workhour is not null
			BEGIN
				INSERT INTO @tempEveryData (FactoryID,SewingLineID,StyleID,SewingDate,OrderTypeID,SCIDelivery,BuyerDellivery,Category,CDCodeID)
				VALUES (@factory,@sewingline,@style,@sewingdate,@ordertype,@scidlv,@buyerdlv,@category,@cdcode)
			END
		SET @_i = @_i + 1
		SET @sewingdate = DATEADD(DAY,1,@sewingdate)
	END
	FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@inline,@offline,@style,@ordertype,@scidlv,@buyerdlv,@category,@cdcode
END
CLOSE cursor_sewingschedule
DEALLOCATE cursor_sewingschedule

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

DECLARE @currentstyle VARCHAR(MAX), --暫存Style組合，用來比對(換線給初始值為空)
		@currentDlv DATE, --暫存同格的最小Buyer Delivery
		@currentlastmonth BIT, ----暫存同格中Style中的任一Orders.SCIDelivery是否有在本月之前
		@currentnextmonth BIT, ----暫存同格中Style中的任一Orders.SCIDelivery是否有在本月之後
		@currentissms BIT, ----暫存同格中Style中的任一Orders.OrderTypeID為SMS
		@currentisbulk BIT, ----暫存同格中Style中的任一Orders.Category為Bulk
		@tmpstyle VARCHAR(MAX),
		@tmpmindlv DATE,
		@tmplastmonth BIT,
		@tmpnextmonth BIT,
		@tmpissms BIT,
		@tmpisbulk BIT,
		@tmpinsertfactory VARCHAR(8),
		@tmpinsertline VARCHAR(2),
		@tmpinsertsewdate DATE,
		@tmpprintstyle VARCHAR(15),
		@tmpisholiday BIT
		

DECLARE cursor_factoryline CURSOR FOR
select distinct FactoryID,SewingLineID from @tempEveryData order by FactoryID,SewingLineID

OPEN cursor_factoryline
FETCH NEXT FROM cursor_factoryline INTO @factory,@sewingline
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @_i = 0
	SET @sewingdate = @sewinginline
	SET @currentstyle = ''
	SET @currentDlv = null
	SET @currentlastmonth = 0
	SET @currentnextmonth = 0
	SET @currentissms = 0
	SET @currentisbulk = 0

	WHILE (@_i <= DATEDIFF(DAY,@sewinginline,@sewingoffline)-1)
	BEGIN
		SET @tmpstyle = ''
		SET @tmpmindlv = null
		SET @tmplastmonth = 0
		SET @tmpnextmonth = 0
		SET @tmpissms = 0
		SET @tmpisbulk = 0
		
		DECLARE cursor_datedata CURSOR FOR
		select StyleID,OrderTypeID,SCIDelivery,BuyerDellivery,Category,CDCodeID from @tempEveryData where FactoryID = @factory and SewingLineID = @sewingline and SewingDate = @sewingdate order by StyleID
		OPEN cursor_datedata
		FETCH NEXT FROM cursor_datedata INTO @style,@ordertype,@scidlv,@buyerdlv,@category,@cdcode
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @category = 'B'
				SET @tmpisbulk = 1
			IF @ordertype = 'SMS'
				SET @tmpissms = 1
			IF @scidlv < @sewinginline
				SET @tmplastmonth = 1
			IF @scidlv > DATEADD(DAY,-1,@sewingoffline)
				SET @tmpnextmonth = 1
			IF @tmpmindlv is null and @category <> 'S'
				SET @tmpmindlv = @buyerdlv
			IF @category <> 'S'
				SET @tmpmindlv = IIF(@tmpmindlv > @buyerdlv,@buyerdlv,@tmpmindlv)
			IF CHARINDEX(@style,@tmpstyle) <= 0
				SET @tmpstyle = @tmpstyle + @style + '(' + @cdcode + ');'

			FETCH NEXT FROM cursor_datedata INTO @style,@ordertype,@scidlv,@buyerdlv,@category,@cdcode
		END
		CLOSE cursor_datedata
		DEALLOCATE cursor_datedata

		IF @currentstyle = '' AND @tmpstyle <> ''
			BEGIN
				INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine)
				VALUES (@factory,@sewingline,@tmpstyle,@sewingdate)
				SET @tmpinsertfactory = @factory
				SET @tmpinsertline = @sewingline
				SET @tmpinsertsewdate = @sewingdate
				SET @tmpisholiday = 0
				SET @currentstyle = @tmpstyle
				SET @currentDlv = @tmpmindlv
			END
		ELSE
			BEGIN
				IF @currentstyle <> @tmpstyle --換Style群組時
					BEGIN
						IF @currentstyle <> '' or @tmpstyle <> '' --若當天皆無排單，則不需要改變下線日期
							BEGIN
								--補下線日期
								UPDATE @tempPintData 
								SET OffLine = DATEADD(DAY,-1,@sewingdate), IsBulk = @tmpisbulk, IsSMS = @tmpissms, IsLastMonth = @tmplastmonth, IsNextMonth = @tmpnextmonth, MinBuyerDelivery = @tmpmindlv 
								where FactoryID = @tmpinsertfactory and SewingLineID = @tmpinsertline and InLine = @tmpinsertsewdate;

								SET @currentDlv = @tmpmindlv
								SET @currentlastmonth = @tmplastmonth
								SET @currentnextmonth = @tmpnextmonth
								SET @currentissms = @tmpissms
								SET @currentisbulk = @tmpisbulk
							END
						IF @tmpstyle <> '' --換線後須新增新的資料
							BEGIN
								INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine) VALUES (@factory,@sewingline,@tmpstyle,@sewingdate);
								SET @tmpinsertfactory = @factory
								SET @tmpinsertline = @sewingline
								SET @tmpinsertsewdate = @sewingdate
								SET @tmpisholiday = 0
							END
						ELSE
							BEGIN
								--若為工廠假日則補上資料
								SET @workhour = null
								select @workhour = Hours from WorkHour where SewingLineID = @sewingline and Date = @sewingdate and FactoryID = @factory and Hours > 0
								IF @workhour is null
									BEGIN
										INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine,OffLine) VALUES (@factory,@sewingline,'Holiday',@sewingdate,@sewingdate);
										SET @tmpinsertfactory = @factory
										SET @tmpinsertline = @sewingline
										SET @tmpinsertsewdate = @sewingdate
										SET @tmpisholiday = 1
									END
							END
						SET @currentstyle = @tmpstyle
					END
				ELSE
					BEGIN
						--若為工廠假日則補上資料
						SET @workhour = null
						select @workhour = Hours from WorkHour where SewingLineID = @sewingline and Date = @sewingdate and FactoryID = @factory and Hours > 0
						IF @workhour is null
							BEGIN
								INSERT INTO @tempPintData(FactoryID,SewingLineID,StyleID,InLine,OffLine) VALUES (@factory,@sewingline,'Holiday',@sewingdate,@sewingdate);
								SET @tmpisholiday = 1
								IF @tmpisholiday = 1 AND @tmpinsertline = @sewingline
								    BEGIN
								    ----補下線日期
								        UPDATE @tempPintData 
								        SET OffLine = DATEADD(DAY,-1,@sewingdate), IsBulk = @tmpisbulk, IsSMS = @tmpissms, IsLastMonth = @tmplastmonth, IsNextMonth = @tmpnextmonth, MinBuyerDelivery = @tmpmindlv 
								        where FactoryID = @tmpinsertfactory and SewingLineID = @tmpinsertline and InLine = @tmpinsertsewdate and StyleID <> 'Holiday' ;
								    END

								SET @tmpinsertfactory = @factory
								SET @tmpinsertline = @sewingline
								SET @tmpinsertsewdate = @sewingdate
								
							END
					END
			END
		--更新同格不同天中最小的Buyer Delivery
		IF @currentDlv is not null and @tmpmindlv is not null
			BEGIN
				SET @currentDlv = IIF(@currentDlv > @tmpmindlv,@tmpmindlv,@currentDlv)
			END

		--當@currentXXX為False，才根據當天的狀態判別是否更新為True
		SET @currentisbulk = IIF(@currentisbulk = 0, @tmpisbulk, 1)
		SET @currentissms = IIF(@currentissms = 0, @tmpissms, 1)
		SET @currentlastmonth = IIF(@currentlastmonth = 0, @tmplastmonth, 1)
		SET @currentnextmonth = IIF(@currentnextmonth = 0, @tmpnextmonth, 1)

		SET @_i = @_i + 1
		SET @sewingdate = DATEADD(DAY,1,@sewingdate) --天數
	END

	--在每一條Line的最後一個排程補寫入MinDlv,Bulk,SMS,LMonth,NMonth，若格子只有一種Style時，則統一以此線同Style中的MinDlv,Bulk,SMS,LMonth,NMonth為主
	IF @currentstyle <> ''
		BEGIN
			select @tmpprintstyle = StyleID from @tempPintData where FactoryID = @tmpinsertfactory and SewingLineID = @tmpinsertline and InLine = @tmpinsertsewdate
			IF @tmpprintstyle <> 'Holiday'
				UPDATE @tempPintData 
				SET IsBulk = @currentisbulk, IsSMS = @currentissms, IsLastMonth = @currentlastmonth, IsNextMonth = @currentnextmonth, MinBuyerDelivery = @currentDlv 
				where FactoryID = @tmpinsertfactory and SewingLineID = @tmpinsertline and InLine = @tmpinsertsewdate;
		END

	FETCH NEXT FROM cursor_factoryline INTO @factory,@sewingline
END
CLOSE cursor_factoryline
DEALLOCATE cursor_factoryline

select * from @tempPintData order by FactoryID,SewingLineID,InLine");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R07_SewingScheduleGanttChart.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            excel.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            //填內容值
            int intRowsStart = 1;
            string writeFty = "";
            string line = "";
            int ftyCount = 0;
            int colCount = 1;
            int monthDays = (startDate.AddMonths(1).AddDays(-1).Subtract(startDate)).Days+1;
            foreach (DataRow dr in printData.Rows)
            {
                //當有換工廠別時，要換Sheet
                if (writeFty != MyUtility.Convert.GetString(dr["FactoryID"]))
                {
                    if (ftyCount > 0)
                    {
                        //將上一間工廠最後一條Line後面沒有Schedule的格子塗黑
                        if (colCount - 1 != monthDays)
                        {
                            for (int i = colCount; i <= monthDays; i++)
                            {
                                worksheet.Range[String.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                            }
                        }
                        //刪除多出來的資料行
                        for (int i = 1; i <= 2; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[intRowsStart + 1, Type.Missing];
                            rng.Select();
                            rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
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
                        }
                    }
                }

                //換Sewing Line時，要填入Line#
                if (line != MyUtility.Convert.GetString(dr["SewingLineID"]))
                {
                    if (intRowsStart > 1)
                    {
                        //將後面沒有Schedule的格子塗黑
                        if (colCount-1 != monthDays)
                        {
                            for (int i = colCount; i <= monthDays; i++)
                            {
                                worksheet.Range[String.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i+1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                            }
                        }
                    }
                    
                    intRowsStart++;
                    colCount = 1;

                    //先插入一行
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(intRowsStart + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);

                    line = MyUtility.Convert.GetString(dr["SewingLineID"]);
                    worksheet.Cells[intRowsStart, 1] = line;
                }

                //算上下線日的總天數，用來做合併儲存格
                DateTime sewingStartDate = Convert.ToDateTime(dr["Inline"]);
                DateTime sewingEndDate = !MyUtility.Check.Empty(dr["Offline"]) ? Convert.ToDateTime(dr["Offline"]) : startDate.AddMonths(1).AddDays(-1);
                int startCol = (sewingStartDate.Subtract(startDate)).Days + 2;
                int endCol = (sewingEndDate.Subtract(startDate)).Days + 2;
                int totalDays = (sewingEndDate.Subtract(sewingStartDate)).Days+1;

                //將中間沒有Schedule的格子塗黑
                if (colCount + 1 != startCol)
                {
                    for (int i = colCount+1; i < startCol; i++)
                    {
                        worksheet.Range[String.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                    }
                }

                //算出Excel的Column的英文位置
                string excelStartColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(startCol);
                string excelEndColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(endCol);
                Microsoft.Office.Interop.Excel.Range selrng = worksheet.get_Range(String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng), Type.Missing).EntireRow;
                //合併儲存格,文字置中
                worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Merge(Type.Missing);
                worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                //顏色顯示優先權：Holiday紅色背景 > SCI Delivery為當月以前的紫色粗體 > SCI Delivery當月為以後的綠色粗體 > Bulk款式藍色粗體 > SMS款式紅色粗體 > 非以上四種情況的黑色字體
                #region 填入內容值
                if (MyUtility.Convert.GetString(dr["StyleID"]) == "Holiday")
                {
                    //設置儲存格的背景色
                    worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Cells.Interior.Color = System.Drawing.Color.Red;
                }
                else if (MyUtility.Convert.GetString(dr["IsLastMonth"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Purple;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), (MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? "" : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d")));
                }
                else if (MyUtility.Convert.GetString(dr["IsNextMonth"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Green;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), (MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? "" : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d")));
                }
                else if (MyUtility.Convert.GetString(dr["IsBulk"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Blue;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), (MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? "" : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d")));
                }
                else if (MyUtility.Convert.GetString(dr["IsSMS"]).ToUpper() == "TRUE")
                {
                    worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Red;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), (MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? "" : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d")));
                }
                else
                {
                    worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Bold = false;
                    worksheet.Range[String.Format("{0}{1}:{2}{1}", excelStartColEng, MyUtility.Convert.GetString(intRowsStart), excelEndColEng)].Font.Color = Color.Black;
                    worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), (MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? "" : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("d")));
                }
                #endregion
                colCount = colCount + (startCol - colCount-1) + totalDays;
            }

            if (colCount - 1 != monthDays)
            {
                for (int i = colCount; i <= monthDays; i++)
                {
                    worksheet.Range[String.Format("{0}{1}:{0}{1}", PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1), MyUtility.Convert.GetString(intRowsStart))].Cells.Interior.Color = System.Drawing.Color.Black;
                }
            }
            //刪除多出來的資料行
            for (int i = 1; i <= 2; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[intRowsStart + 1, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
            }

            //刪除多的Sheet
            for (int i = ftyCount + 1; i <= 10; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[ftyCount + 1];
                worksheet.Delete();
            }

            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Select();

            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
