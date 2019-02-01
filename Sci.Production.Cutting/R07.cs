using Ict;
using Microsoft.Office.Core;
using Sci.Data;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        private DataTable[] printDatas;
        private DateTime estCutDate_s = new DateTime();
        private DateTime estCutDate_e = new DateTime();
        private string Mdivision, Factory;

        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent(); DataTable WorkOrder;
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrder WITH (NOLOCK) ", out WorkOrder);
            MyUtility.Tool.SetupCombox(comboM, 1, WorkOrder);
            comboM.Text = Sci.Env.User.Keyword;

            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateEstCutDate.Value1) || MyUtility.Check.Empty(dateEstCutDate.Value2))
            {

                MyUtility.Msg.WarningBox("Est Cut Date Can not be empty!");
                return false;
            }

            Mdivision = comboM.Text;
            Factory = comboFactory.Text;
            return base.ValidateInput();
        }

        // 驗證輸入條件
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region 組SQL

            sqlCmd.Append($@"


SELECT 
         wo.FactoryID
        ,wo.EstCutDate
        ,wo.CutCellid
        ,[SpreadingTable]=wo.SpreadingNoID
        ,wo.CutplanID
        ,wo.CutRef
        ,wo.ID
        ,wod.OrderID
        ,o.StyleID
        ,wod.SizeCode
        ,[Order Qty]= OrderQty.SumValue--Excess不需計算
        ,f.Description
        ,f.MtlTypeID
        ,wo.FabricCombo
        ,[MarkerLength(yard)] = sc.Cons/sl.Layer
        ,[Perimeter (m)]= ROUND(dbo.GetActualPerimeter(wo.ActCuttingPerimeter),4)
        ,wo.Layer
        ,[Ratio] = SizeCode.SizeCode
        ,[Total Fabric Cons.(yard)]=wo.Cons
        ,[Excess Fabirc Comb. Qty]= ExcessFabircCombQty.SumValue --只需計算Excess數量
        ,[No.ofRoll] = IIF(fi.avgInQty=0    ,0    ,ROUND(sc.Cons/fi.avgInQty,0))
        ,[No.ofWindow]=sc.Cons / sl.Layer * 0.9114 / 1.4  --Marker length  /1.4 (m) (原本是碼，要轉成公尺，所以*0.9114)
        ,[Cutting Speed (m/min)]=ActualSpeed.ActualSpeed

        --這些是Spreading
        ,[Preparation Time (min.)]=st.PreparationTime
        ,[Changeover Time (min.)]= IIF(IsRoll=1, st.ChangeOverRollTime , st.ChangeOverUnRollTime)   
        ,[Spreading Setup Time (min.)]=st.SetupTime
        ,[Mach. Spreading Time (min.)]=st.SpreadingTime
        ,[Separator time (min.)]=st.SeparatorTime
        ,[Forward Time (min.)]=st.ForwardTime

        --這個是Cutting
        ,[Cutting Setup Time (min.)]=ct.SetUpTime
        --這個是Cutting
        ,[Mach. Cutting Time (min.)]= ROUND(dbo.GetActualPerimeter(wo.ActCuttingPerimeter),4) / ActualSpeed.ActualSpeed

        --這些是Spreading
        ,[Total Spreading Time (min.)]= st.PreparationTime 
								        * sc.Cons/sl.Layer --Marker Length
								        + IIF(IsRoll=1, st.ChangeOverRollTime , st.ChangeOverUnRollTime)    --Changeover time
								        * IIF(fi.avgInQty=0    ,0    ,ROUND(sc.Cons/fi.avgInQty,0)) --No. of Roll
								        + st.SetupTime--Set up time 
							            + (
									        st.SpreadingTime  --Machine Spreading Time 
									        * sc.Cons/sl.Layer --Marker Length
									        * sl.Layer         --其實可以直接*Cons，因為Layer會被抵銷掉
								          )
								        + (
									        st.SeparatorTime  --No. of Separator
									        * 1   --Dyelot  先通通帶1
									        - 1
								          )
								        + st.ForwardTime
        ----這個是Cutting
        ,[Total Cutting Time (min.)]=ct.SetUpTime 
							        + IIF (ActualSpeed.ActualSpeed=0  ,0  , round(dbo.GetActualPerimeter(wo.ActCuttingPerimeter),4) / ActualSpeed.ActualSpeed)
							        --同裁次週長若不一樣就是有問題

INTO #tmp
FROM WorkOrder wo WITH (NOLOCK) 
INNER JOIN WorkOrder_Distribute wod WITH (NOLOCK)  ON wod.WorkOrderUkey=wo.Ukey
INNER JOIN Orders o ON o.ID=wod.OrderID
LEFT JOIN Fabric f WITH (NOLOCK) ON  wo.SciRefno = f.SciRefno 
LEFT JOIN SpreadingTime st ON f.WeaveTypeID=st.WeaveTypeID
LEFT JOIN CuttingTime ct ON f.WeaveTypeID=ct.WeaveTypeID
OUTER APPLY(
	SELECT SizeCode = stuff(
	(
		SELECT concat(', ' , c.SizeCode, '/ ', c.Qty)
		FROM WorkOrder_SizeRatio c WITH (NOLOCK) 
		WHERE c.WorkOrderUkey =wo.Ukey 
		FOR XML PATH('')
	),1,1,'')
) AS SizeCode

OUTER APPLY(
	select avgInQty = avg(fi.InQty)
	from PO_Supp_Detail psd with(nolock)
	left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
	where psd.ID = wo.id and psd.SCIRefno = wo.SCIRefno
	and fi.InQty is not null
) AS fi

OUTER APPLY(select Layer = sum(wo.Layer)over(partition by wo.CutRef))sl
OUTER APPLY(select Cons = sum(wo.Cons)over(partition by wo.CutRef))sc

OUTER APPLY(
	SELECT [SumValue]=IIF(OrderID ='EXCESS' ,NULL,SUM(Qty) )
	FROM WorkOrder_Distribute 
	WHERE WorkOrderUkey = wo.Ukey 
			AND OrderID=wod.OrderID 
			AND Article=wod.Article 
			AND SizeCode=wod.SizeCode
	GROUP BY OrderID

)OrderQty


OUTER APPLY(
	SELECT [SumValue]=IIF(OrderID ='EXCESS' , Sum(Qty) ,NULL)
	FROM WorkOrder_Distribute 
	WHERE WorkOrderUkey = wo.Ukey 
			AND OrderID=wod.OrderID 
			AND Article=wod.Article 
			AND SizeCode=wod.SizeCode
	GROUP BY OrderID
)ExcessFabircCombQty


OUTER APPLY(
	select ActualSpeed
	from CuttingMachine_detail cmd
	inner join CutCell cc on cc.CuttingMachineID = cmd.id
	where cc.id = wo.CutCellid 
	and sl.Layer between cmd.LayerLowerBound and cmd.LayerUpperBound
	and cmd.WeaveTypeID = f.WeaveTypeID 
)ActualSpeed

OUTER APPLY(
		select fr.IsRoll
		from ManufacturingExecution.dbo.RefnoRelaxtime rr 
		inner join ManufacturingExecution.dbo.FabricRelaxation fr on rr.FabricRelaxationID = fr.ID
		where rr.Refno = wo.Refno
)IsRoll

WHERE 1=1



");
            #endregion

            #region 條件
            if (!MyUtility.Check.Empty(Mdivision))
            {
                sqlCmd.Append($" AND wo.MDivisionID = '{Mdivision}'");
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlCmd.Append($" AND wo.FactoryID = '{Factory}'");
            }
            if (!MyUtility.Check.Empty(dateEstCutDate.Value1.Value))
            {
                estCutDate_s = dateEstCutDate.Value1.Value;
                sqlCmd.Append($" AND wo.EstCutDate >= '{dateEstCutDate.Value1.Value.ToShortDateString()}'");
            }
            if (!MyUtility.Check.Empty(dateEstCutDate.Value2.Value))
            {
                estCutDate_e = dateEstCutDate.Value2.Value;
                sqlCmd.Append($" AND wo.EstCutDate <= '{dateEstCutDate.Value2.Value.ToShortDateString()}'");
            }
            if (!MyUtility.Check.Empty(txtSpreadingNo_s.Text))
            {
                sqlCmd.Append($" AND wo.SpreadingNoID >= '{txtSpreadingNo_s.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtSpreadingNo_e.Text))
            {
                sqlCmd.Append($" AND wo.SpreadingNoID >='{txtSpreadingNo_e.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtCutCell_s.Text))
            {
                sqlCmd.Append($" AND wo.CutCellId >= '{txtCutCell_s.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtCutCell_e.Text))
            {
                sqlCmd.Append($" AND wo.CutCellId <= '{txtCutCell_e.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtCuttingSp.Text))
            {
                sqlCmd.Append($" AND wod.OrderID = '{txtCuttingSp.Text}'");
            }
            #endregion

            #region 組SQL Part 2
            sqlCmd.Append(@"


SELECT * FROM #tmp

--Spreading Capacity Forecast
SELECT 
[Spreading Table No.]=SpreadingTable
, [Work Hours/Day]=''
, [Total Available Spreading Time (hrs)]=SpreadingTable *6 *0.8 --* TotalWorkingDays  * AvgEfficiency  B6
, [Total Spreading Yardage]=[Total Fabric Cons.(yard)]
, [Total Spreading Marker Qty]= COUNT(SpreadingTable)
, [Total Spreading Time (hrs.)]= [Total Spreading Time (min.)] / 60--Total Spreading Time (hrs.) 換算小時  B9
, [Spreading Capacity Fulfill Rate%]=  --B10=(B9/B6)*100
	IIF(SpreadingTable *6 *0.8  = 0   
		, 0  
		, [Total Spreading Time (min.)] / 60  --B9
		  / SpreadingTable *6 *0.8         --B6
		  *100
		)
,[Capacity (hrs)]= [Total Spreading Time (min.)] / 60 - SpreadingTable *6 *0.8 
FROM #tmp
GROUP BY SpreadingTable,[Total Fabric Cons.(yard)],[Total Spreading Time (min.)]



--Cutting Capacity Forecast


SELECT DISTINCT
[Cut cell (Morgan No.)]= CutCellid   --B15
,[Cutting Mach. Description]='Next 70'  
,[Work Hours/Day]=''    --manual input
,[Total Available Cutting Time (hrs)]=CutCellid * 6 * 0.8   --B18   = (B15*Total Working Days)*C2
,[Avg. Cut Speed (m/min.)]=''   --manual input
,[Total Cutting Perimeter (m)]=[Perimeter (m)]
,[Total Cut Marker Qty]=CutCell.count
,[Total Cut Fabric Yardage]=[Total Fabric Cons.(yard)]
,[Total Cutting Time (hrs.)]=[Total Cutting Time (min.)]  --B23
,[Cutting Capacity Fulfill Rate%]=IIF(CutCellid * 6 * 0.8  = 0 , 0  
									, [Total Cutting Time (min.)] / CutCellid * 6 * 0.8   )   --(B23/B18)*100
,['+/- Capacity (hrs)]=[Total Cutting Time (min.)]- CutCellid * 6 * 0.8    --B23-B18

FROM #tmp
OUTER APPLY(
SELECT [count]=COUNT(CutCellid) FROM #tmp
)CutCell



SELECT 

[Row]='S'+ CAST (ROW_NUMBER() OVER(ORDER BY ID ASC)  as varchar)
,[Total Spreading Marker Qty]= COUNT(SpreadingTable)
,[Capacity (hrs)]= [Total Spreading Time (min.)] / 60 - SpreadingTable *6 *0.8 
FROM #tmp
GROUP BY ID,SpreadingTable,[Total Fabric Cons.(yard)],[Total Spreading Time (min.)]



--Cutting Capacity Forecast

UNION ALL

SELECT 
[Row]='C'+ CAST (ROW_NUMBER() OVER(ORDER BY ID ASC)  as varchar)
,[Total Cut Fabric Yardage]
,[Capacity]
FROM (
	SELECT DISTINCT
	iD
	,[Total Cut Fabric Yardage]=[Total Fabric Cons.(yard)]
	,[Capacity]=[Total Cutting Time (min.)]- CutCellid * 6 * 0.8    --B23-B18

	FROM #tmp
	OUTER APPLY(
	SELECT [count]=COUNT(CutCellid) FROM #tmp
	)CutCell
) a


DROP TABLE  #tmp

");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printDatas);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");

            DataTable newDt = new DataTable();
            DataTable newDt2 = new DataTable();
            TimeSpan ts = new TimeSpan(estCutDate_e.Ticks - estCutDate_s.Ticks);
            string totalWorkingDays = ts.TotalDays.ToString();
            string downloadDateTime = DateTime.Now.ToShortTimeString();

            if (printDatas.Length == 0 || printDatas[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printDatas[0].Rows.Count);

            //往下看之前，請先打開xltx範本檔搭配看

            #region 將資料轉向，以符合Sheet 2 （Capacity Forecast Summary） 

            //轉向
            //先設定Column，有幾筆資料就有幾個Column，Spreading Table No.的值當作欄位名稱
            foreach (DataRow dr in printDatas[1].Rows)
            {
                newDt.ColumnsStringAdd(dr["Spreading Table No."].ToString());
            }

            foreach (DataRow dr in printDatas[1].Rows)
            {
                //縱向，先塞空的
                for (int i = 0; i <= printDatas[1].Columns.Count - 1; i++)
                {
                    DataRow item = newDt.NewRow();
                    newDt.Rows.Add(item);
                }
            }

            for (int i = 0; i <= printDatas[1].Rows.Count - 1; i++)
            {
                for (int y = 0; y <= printDatas[1].Columns.Count - 1; y++)
                {

                    newDt.Rows[y][i] = printDatas[1].Rows[i][y];
                }
            }

            #endregion

            #region 這方式保留

            //Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Cutting_R07.xltx");
            //SaveXltReportCls.XltRptTable dt_All = new SaveXltReportCls.XltRptTable(printDatas[0]);
            //SaveXltReportCls.XltRptTable dt_Summary1 = new SaveXltReportCls.XltRptTable(newDt);
            ////SaveXltReportCls.XltRptTable dt_Summary2 = new SaveXltReportCls.XltRptTable(newDt2);

            //dt_All.ShowHeader = false;
            //dt_Summary1.ShowHeader = false;
            //xl.DicDatas.Add("##DATA", dt_All);
            //xl.DicDatas.Add("##DATA1", dt_Summary1);
            ////xl.DicDatas.Add("##DATA2", dt_Summary2);


            //xl.DicDatas.Add("##Factory ", Factory);
            //xl.DicDatas.Add("##DownloadDateTime", downloadDateTime);
            //xl.DicDatas.Add("##ForecastPeriod", this.dateEstCutDate.Value1.Value.ToShortDateString() + " ~ " + this.dateEstCutDate.Value2.Value.ToShortDateString());
            //xl.DicDatas.Add("##TotalWorkingDays", totalWorkingDays);



            //xl.Save();
            #endregion


            //打開範本，寫入資料
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R07.xltx"); 
            //第一個細項大表
            MyUtility.Excel.CopyToXls(printDatas[0], "", "Cutting_R07.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]);    
            //第三個，對應圖表用的隱藏表
            MyUtility.Excel.CopyToXls(printDatas[3], "", "Cutting_R07.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[3]);   
            
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   
            Microsoft.Office.Interop.Excel.Worksheet objSheets3 = objApp.ActiveWorkbook.Worksheets[3];   
            
            //固定欄位寫入
            objSheets2.Cells[1, 2] = Factory;
            objSheets2.Cells[2, 2] = downloadDateTime;
            objSheets2.Cells[1, 6] = this.dateEstCutDate.Value1.Value.ToShortDateString() + " ~ " + this.dateEstCutDate.Value2.Value.ToShortDateString();
            objSheets2.Cells[2, 6] = totalWorkingDays;

            //寫入隱藏Sheet
            for (int i = 0; i <= printDatas[1].Rows.Count - 1; i++)
            {
                for (int y = 0; y <= printDatas[1].Columns.Count - 1; y++)
                {
                    objSheets2.Cells[4 + y, 2 + i] = printDatas[1].Rows[i][y].ToString();
                }
            }
            for (int i = 0; i <= printDatas[2].Rows.Count - 1; i++)
            {
                for (int y = 0; y <= printDatas[2].Columns.Count - 1; y++)
                {
                    objSheets2.Cells[15 + y, 2 + i] = printDatas[2].Rows[i][y].ToString();
                }
            }

            ///////////////////////////////////

            //要對應的資料擺在chartSheet
            Microsoft.Office.Interop.Excel.Worksheet chartSheet = objSheets3;

            //新增一個Chart
            object misValue = System.Reflection.Missing.Value;
            //注意這裡的Sheet，是要放「圖表所在的位置」
            Excel.ChartObjects xlsCharts = (Excel.ChartObjects)objSheets2.ChartObjects(Type.Missing);
            //圖表大小位置設定
            Excel.ChartObject myChart = xlsCharts.Add(10, 500, 850, 300);
            Excel.Chart chartPage = myChart.Chart;

            //建立一個Series集合：SeriesCollection
            Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();


            #region Step.2 圖表加入

            //長方圖

            //集合裡面加一筆新的Serires
            Microsoft.Office.Interop.Excel.Series series = seriesCollection.NewSeries();

            //選取資料範圍
            series.Values = chartSheet.get_Range("B2", string.Format("B{0}", 2 + printDatas[3].Rows.Count - 1));
            series.XValues = chartSheet.get_Range("A2", string.Format("A{0}", 2 + printDatas[3].Rows.Count - 1)); 
            
            series.Name = "No. Of Marker";
            series.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;//選擇Chart種類

            //折線圖 作法同上
            Microsoft.Office.Interop.Excel.Series series1 = seriesCollection.NewSeries();
            series1.Values = chartSheet.get_Range("C2", string.Format("C{0}", 2 + printDatas[3].Rows.Count - 1));
            series1.XValues = chartSheet.get_Range("A2", string.Format("A{0}", 2 + printDatas[3].Rows.Count - 1));

            series1.Name = "% Capacity Fulfillment";
            series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

            #endregion

            #region Step.3 圖表版面配置


            //版面配置，選擇第9種，還有其他種，可自行嘗試
            chartPage.ApplyLayout(9);
            //設定圖表標題，注意，不先Select會爆炸
            chartPage.ChartTitle.Select();
            chartPage.ChartTitle.Text = "Spreading & Cutting Balance (Forecast)";


            //開啟運算列表（不知道運算列表是什麼的話，在Excel裡面點選一個圖表，看版面配置→運算列表）
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementDataTableWithLegendKeys);
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementLegendNone);
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementPrimaryCategoryAxisTitleNone);
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementPrimaryValueAxisTitleNone);

            //開啟資料標籤位置（也就是長條圖折線圖旁邊的數字，看版面配置→資料標籤）
            chartPage.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false
                , false, false, false, false, true, false, false, false);

            #endregion

            #region Step.4 開始細項的動態設定，模擬人用滑鼠點擊設定的動作

            objSheets2.Activate();

            //Item 1=長條圖，  2 =折線圖，取決於在Step.2 時先New哪一個Series

            //將S開頭的資料，的長條圖變成綠色
            for (int i = 1; i <= printDatas[1].Rows.Count; i++)
            {
                //一定要先選取到那個Item，因為S開頭資料放在前面，所以用迴圈跑

                //Series集合，Item從1開始，1代表長條圖的圖表物件
                seriesCollection.Item(1).Select();
                //Point 1  代表第一個長條圖
                Excel.Point singleChart = seriesCollection.Item(1).Points(i);
               
                singleChart.Select();
                //顏色用 ForeColor，不要用BackColor
                singleChart.Format.Fill.ForeColor.ObjectThemeColor = MsoThemeColorIndex.msoThemeColorAccent3;
                
            }
            //長條圖的資料標籤 設定
            for (int i = 1; i <= printDatas[1].Rows.Count + printDatas[2].Rows.Count; i++)
            {
                //一定要先選取到那個Item，因為S開頭資料放在前面，所以用迴圈跑

                //Series集合，Item從1開始，1代表長條圖的圖表物件
                seriesCollection.Item(1).Select();
                Excel.Series aa = seriesCollection.Item(1);
                Excel.DataLabel signleDataLabel = aa.DataLabels(i);
                signleDataLabel.Position = Excel.XlDataLabelPosition.xlLabelPositionInsideBase;
            }
            //折線圖的資料標籤 設定
            for (int i = 1; i <= printDatas[1].Rows.Count + printDatas[2].Rows.Count; i++)
            {
                //一定要先選取到那個Item，因為S開頭資料放在前面，所以用迴圈跑

                //Series集合，Item從1開始，1代表長條圖的圖表物件
                seriesCollection.Item(2).Select();
                Excel.Series aa = seriesCollection.Item(2);
                Excel.DataLabel signleDataLabel = aa.DataLabels(i);
                signleDataLabel.Position = Excel.XlDataLabelPosition.xlLabelPositionAbove;
            }

            chartSheet.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            #endregion

            ///////////////////////////////////
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R07");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close(); ;
            objApp.Quit();

            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();

            this.HideWaitMessage();

            return true;
        }
    }
}
