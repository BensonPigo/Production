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

                MyUtility.Msg.WarningBox("<Est Cut Date> Can not be empty!");
                return false;
            }


            if (MyUtility.Check.Empty(comboM.Text))
            {

                MyUtility.Msg.WarningBox("<M> Can not be empty!");
                return false;
            }


            //if (MyUtility.Check.Empty(comboFactory.Text))
            //{

            //    MyUtility.Msg.WarningBox("<Factory> Can not be empty!");
            //    return false;
            //}

            
            Mdivision = comboM.Text;
            Factory = comboFactory.Text;
            return base.ValidateInput();
        }

        // 驗證輸入條件
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            TimeSpan ts = new TimeSpan(estCutDate_e.Ticks - estCutDate_s.Ticks);
            string totalWorkingDays = (ts.TotalDays + 1).ToString();

            #region 組SQL

            sqlCmd.Append($@"

SELECT DISTINCT a.Ukey, b.OrderID
INTO #tmp_OrderList
FROM WorkOrder a
INNER JOIN WorkOrder_Distribute b ON a.Ukey=b.WorkOrderUkey
WHERE 1=1 ");

            #region 條件
            if (!MyUtility.Check.Empty(Mdivision))
            {
                sqlCmd.Append($" AND a.MDivisionID = '{Mdivision}'");
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlCmd.Append($" AND a.FactoryID = '{Factory}'");
            }
            if (!MyUtility.Check.Empty(dateEstCutDate.Value1.Value))
            {
                estCutDate_s = dateEstCutDate.Value1.Value;
                sqlCmd.Append($" AND a.EstCutDate >= '{dateEstCutDate.Value1.Value.ToShortDateString()}'");
            }
            if (!MyUtility.Check.Empty(dateEstCutDate.Value2.Value))
            {
                estCutDate_e = dateEstCutDate.Value2.Value;
                sqlCmd.Append($" AND a.EstCutDate <= '{dateEstCutDate.Value2.Value.ToShortDateString()}'");
            }
            if (!MyUtility.Check.Empty(txtSpreadingNo_s.Text))
            {
                sqlCmd.Append($" AND a.SpreadingNoID >= '{txtSpreadingNo_s.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtSpreadingNo_e.Text))
            {
                sqlCmd.Append($" AND a.SpreadingNoID >='{txtSpreadingNo_e.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtCutCell_s.Text))
            {
                sqlCmd.Append($" AND a.CutCellId >= '{txtCutCell_s.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtCutCell_e.Text))
            {
                sqlCmd.Append($" AND a.CutCellId <= '{txtCutCell_e.Text}' ");
            }
            if (!MyUtility.Check.Empty(txtCuttingSp.Text))
            {
                sqlCmd.Append($" AND b.OrderID = '{txtCuttingSp.Text}'");
            }
            #endregion

            sqlCmd.Append($@"

SELECT  DISTINCT
         wo.FactoryID
        ,wo.EstCutDate
        ,wo.CutCellid
        ,[SpreadingTable]=wo.SpreadingNoID
        ,wo.CutplanID
        ,wo.CutRef
        ,wo.ID
        ,[OrderID]=OrderID.value--wod.OrderID
        ,o.StyleID
        ,[Size]=Size.value--wod.SizeCode
        ,[Order Qty]= OrderQty.SumValue--Excess不需計算
        ,f.Description
        ,f.MtlTypeID
        ,wo.FabricCombo
        ,[MarkerLength(yard)] = sc.Cons/sl.Layer
        ,[Perimeter (m)]= IIF(wo.ActCuttingPerimeter NOT LIKE '%YD%',0, ROUND(dbo.GetActualPerimeter(wo.ActCuttingPerimeter),4))
        ,wo.Layer
        ,[Ratio] = Ratio.value
        ,[Total Fabric Cons.(yard)]=wo.Cons
        ,[Excess Fabirc Comb. Qty]= ExcessFabircCombQty.SumValue --只需計算Excess數量
        ,[No.ofRoll] = IIF(fi.avgInQty=0    ,0    ,ROUND(fi.avgInQty/sc.Cons,0))
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
        ,[Mach. Cutting Time (min.)]=  IIF(wo.ActCuttingPerimeter NOT LIKE '%YD%',0, ROUND(dbo.GetActualPerimeter(wo.ActCuttingPerimeter),4)) / ActualSpeed.ActualSpeed

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
								        + 
									        st.SeparatorTime  --No. of Separator
									        * (1   --Dyelot  先通通帶1
									           - 1
								               )
								        + st.ForwardTime
        ----這個是Cutting
        ,[Total Cutting Time (min.)]=ct.SetUpTime 
							        + IIF (ActualSpeed.ActualSpeed=0  ,0  , IIF(wo.ActCuttingPerimeter NOT LIKE '%YD%',0, ROUND(dbo.GetActualPerimeter(wo.ActCuttingPerimeter),4))  / ActualSpeed.ActualSpeed)
							        --同裁次週長若不一樣就是有問題
INTO #tmp
FROM WorkOrder wo WITH (NOLOCK) 
INNER JOIN WorkOrder_Distribute wod WITH (NOLOCK)  ON wod.WorkOrderUkey=wo.Ukey
INNER JOIN Orders o ON o.ID=wod.OrderID
LEFT JOIN Fabric f WITH (NOLOCK) ON  wo.SciRefno = f.SciRefno 
LEFT JOIN SpreadingTime st ON f.WeaveTypeID=st.WeaveTypeID
LEFT JOIN CuttingTime ct ON f.WeaveTypeID=ct.WeaveTypeID
OUTER APPLY(
	SELECT value = stuff(
	(
		SELECT concat(', ' , c.SizeCode, '/ ', c.Qty)
		FROM WorkOrder_SizeRatio c WITH (NOLOCK) 
		WHERE c.WorkOrderUkey =wo.Ukey 
		FOR XML PATH('')
	),1,1,'')
) AS Ratio
OUTER APPLY(
	SELECT value = stuff(
	(
		SELECT concat(',' ,a.OrderID)
		FROM ( 			
			SELECT DISTINCT c.OrderID
			FROM WorkOrder_Distribute c WITH (NOLOCK) 
			WHERE c.WorkOrderUkey =wo.Ukey 
		)a
		FOR XML PATH('')
	),1,1,'')
) AS OrderID
OUTER APPLY(
	SELECT value = stuff(
	(
		SELECT concat(',' ,a.SizeCode)
		FROM ( 			
			SELECT DISTINCT c.SizeCode
			FROM WorkOrder_Distribute c WITH (NOLOCK) 
			WHERE c.WorkOrderUkey =wo.Ukey 
		)a
		FOR XML PATH('')
	),1,1,'')
) AS Size

OUTER APPLY(
	SELECT avgInQty = avg(fi.InQty)
	FROM PO_Supp_Detail psd WITH (NOLOCK) 
	INNER JOIN FtyInventory fi WITH (NOLOCK)  ON fi.POID = psd.ID AND fi.Seq1 = psd.SEQ1 AND fi.Seq2 = psd.SEQ2
	WHERE psd.ID = wo.id AND psd.SCIRefno = wo.SCIRefno
	AND fi.InQty IS NOT NULL
) AS fi

OUTER APPLY(select Layer = sum(wo.Layer)over(partition by wo.CutRef))sl
OUTER APPLY(select Cons = sum(wo.Cons)over(partition by wo.CutRef))sc

OUTER APPLY(--不包含OrderID=Excess 數量
	SELECT [SumValue]=SUM(Qty)
	FROM WorkOrder_Distribute 
	WHERE WorkOrderUkey =wo.Ukey AND OrderID!='EXCESS'
)OrderQty
OUTER APPLY(--只需計算Excess數量
	SELECT [SumValue]=Sum(Qty)
	FROM WorkOrder_Distribute 
	WHERE WorkOrderUkey =wo.Ukey AND OrderID='EXCESS'
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
AND EXISTS (SELECT OrderID  FROM #tmp_OrderList WHERE OrderID=wod.OrderID AND Ukey=wo.Ukey)

");
            #endregion


            #region 組SQL Part 2
            sqlCmd.Append($@"

SELECT * FROM #tmp

--Spreading Capacity Forecast
SELECT 
[Spreading Table No.]= SpreadingTable
, [Work Hours/Day]=''                                                     -- 給User手動輸入
, [Total Available Spreading Time (hrs)]=0                                -- =(Work Hours/Day   *   Total Working Days) *  Avg. Efficiency %  0.8是預設的!!  Work Hours/Day 等被輸入  所以都是0，後面相關欄位也是
, [Total Spreading Yardage]=SUM([Total Fabric Cons.(yard)])
, [Total Spreading Marker Qty]= SpreadingTable.count
, [Total Spreading Time (hrs.)]= SUM([Total Spreading Time (min.)] / 60)       -- Total Spreading Time (hrs.) 換算小時  B9
, [Spreading Capacity Fulfill Rate%]= 0                                   -- =Total Spreading Time (hrs.)   /   Total Available Spreading Time (hrs)  *100
, [Capacity (hrs)]= 0                                                     -- =Total Spreading Time (hrs.)   -   Total Available Spreading Time (hrs)
FROM #tmp t
OUTER APPLY(
	SELECT [count]=COUNT(CutCellid) FROM #tmp WHERE SpreadingTable = t.SpreadingTable
)SpreadingTable
GROUP BY SpreadingTable,SpreadingTable.count


--Cutting Capacity Forecast
SELECT 
[Cut cell (Morgan No.)]= CutCellid   
,[Cutting Mach. Description]='Next 70'  
,[Work Hours/Day]=''                                          -- 給User手動輸入
,[Total Available Cutting Time (hrs)]= 0                      -- = (Work Hours/Day  *  Total Working Days) *  Avg. Efficiency %   0.8是預設的!!  Work Hours/Day 等被輸入  所以都是0，後面相關欄位也是
,[Avg. Cut Speed (m/min.)]=''                                 -- 給User手動輸入
,[Total Cutting Perimeter (m)]=SUM([Perimeter (m)])
,[Total Cut Marker Qty]=CutCell.count
,[Total Cut Fabric Yardage]=SUM([Total Fabric Cons.(yard)])
,[Total Cutting Time (hrs.)]=SUM([Total Cutting Time (min.)] / 60)  --  換算成小時
,[Cutting Capacity Fulfill Rate%]=0                            -- =Total Cutting Time (hrs.)   /    Total Available Cutting Time (hrs)  *100
,['+/- Capacity (hrs)]=  0                                     -- =Total Cutting Time (hrs.)   -    Total Available Cutting Time (hrs)

FROM #tmp t
OUTER APPLY(
	SELECT [count]=COUNT(CutCellid) FROM #tmp WHERE CutCellid=t.CutCellid
)CutCell
GROUP BY CutCellid,CutCell.count



--用來塞圖表
SELECT 
[Spreading Table No.]
,[Total Available Cutting Time]= 0
,[Capacity (hrs)]= 0  -- 0.8是預設的!!
FROM #tmp_Spreading


--Cutting Capacity Forecast
UNION ALL
SELECT 
[Cut cell (Morgan No.)]
,[TTotal Available Cutting Time]=0
,[Capacity]=  0    
FROM #tmp_Cutting

DROP TABLE #tmp,#tmp_OrderList ,#tmp_Spreading ,#tmp_Cutting



DROP TABLE #tmp,#tmp_OrderList

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
            string totalWorkingDays = (ts.TotalDays+1).ToString();
            string downloadDateTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss");

            if (printDatas.Length == 0 || printDatas[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }
            
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printDatas[0].Rows.Count);

            //往下看之前，請先打開xltx範本檔搭配看
            
            //打開範本，寫入資料
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R07_Backup.xltx");


            MyUtility.Excel.CopyToXls(printDatas[0], "", "Cutting_R07_Backup.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]);
            MyUtility.Excel.CopyToXls(printDatas[3], "", "Cutting_R07_Backup.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[3]);

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   
            Microsoft.Office.Interop.Excel.Worksheet objSheets3 = objApp.ActiveWorkbook.Worksheets[3];

            int spreadingData_Count = printDatas[1].Rows.Count;
            int cuttingData_Count = printDatas[2].Rows.Count;


            #region Step.1 資料轉向，填入表格

            for (int i = 0; i <= spreadingData_Count - 1; i++)
            {
                for (int u = 0; u <= printDatas[1].Columns.Count - 1; u++)
                {
                    DataRow item = newDt.NewRow();
                    objSheets2.Cells[4+u, 2+i] = printDatas[1].Rows[i].ItemArray[u].ToString();
                }
            }
            //設定框線
            Excel.Range to = objSheets2.get_Range((Excel.Range)objSheets2.Cells[4, 2], (Excel.Range)objSheets2.Cells[4 + printDatas[1].Columns.Count - 1, 2 + spreadingData_Count - 1]);
            to.Borders.Weight = 2;
            //合併儲存格
            //to = objSheets2.get_Range((Excel.Range)objSheets2.Cells[3,1], (Excel.Range)objSheets2.Cells[3, 2 + spreadingData_Count - 1]);
            //to.MergeCells = true;

            for (int i = 0; i <= cuttingData_Count - 1; i++)
            {
                for (int u = 0; u <= printDatas[2].Columns.Count - 1; u++)
                {
                    DataRow item = newDt.NewRow();
                    objSheets2.Cells[14 + u, 2 + i] = printDatas[2].Rows[i].ItemArray[u].ToString();
                }
            }

            to = objSheets2.get_Range((Excel.Range)objSheets2.Cells[14, 2], (Excel.Range)objSheets2.Cells[14 + printDatas[2].Columns.Count - 1, 2 + cuttingData_Count - 1]);
            to.Borders.Weight = 2;

            //to = objSheets2.get_Range((Excel.Range)objSheets2.Cells[3, 1], (Excel.Range)objSheets2.Cells[3, 2 + cuttingData_Count - 1]);
            //to.MergeCells = true;

            #endregion

            #region 固定欄位寫入
            objSheets2.Cells[1, 2] = Factory;
            objSheets2.Cells[2, 2] = downloadDateTime;
            objSheets2.Cells[1, 6] = this.dateEstCutDate.Value1.Value.ToShortDateString() + " ~ " + this.dateEstCutDate.Value2.Value.ToShortDateString();
            objSheets2.Cells[2, 6] = totalWorkingDays;

            objSheets3.Cells[1, 2] = Factory;
            objSheets3.Cells[2, 2] = downloadDateTime;
            objSheets3.Cells[1, 6] = this.dateEstCutDate.Value1.Value.ToShortDateString() + " ~ " + this.dateEstCutDate.Value2.Value.ToShortDateString();
            objSheets3.Cells[2, 6] = totalWorkingDays;
            #endregion
            
            Microsoft.Office.Interop.Excel.Worksheet speading_chartSheet = objSheets2;
            Microsoft.Office.Interop.Excel.Worksheet cutting_chartSheet = objSheets3;

            Microsoft.Office.Interop.Excel.Worksheet chartSheet = objSheets3;


            #region Step.2 圖表加入

            //新增一個Chart
            object misValue = System.Reflection.Missing.Value;
            //注意這裡的Sheet，是要放「圖表所在的位置」
            Excel.ChartObjects xlsCharts_2 = (Excel.ChartObjects)objSheets2.ChartObjects(Type.Missing);
            Excel.ChartObjects xlsCharts_3 = (Excel.ChartObjects)objSheets3.ChartObjects(Type.Missing);
            //圖表大小位置設定
            Excel.ChartObject myChart = xlsCharts_2.Add(20, 240, 250 + spreadingData_Count * 110, 300);
            Excel.Chart chartPage = myChart.Chart;


            Excel.ChartObject myChart_3 = xlsCharts_3.Add(20, 300, 250 + cuttingData_Count * 110, 300);
            Excel.Chart chartPage_3 = myChart_3.Chart;

            //建立一個Series集合：SeriesCollection
            Excel.SeriesCollection seriesCollection_Spreading = chartPage.SeriesCollection();
            Excel.SeriesCollection seriesCollection_Cutting = chartPage_3.SeriesCollection();

            //Spreading

            //長方圖---

            //集合裡面加一筆新的Serires
            Microsoft.Office.Interop.Excel.Series series1 = seriesCollection_Spreading.NewSeries();

            string columnHead=MyUtility.Excel.ConvertNumericToExcelColumn(1 + spreadingData_Count);
            //選取資料範圍
            series1.Values = chartSheet.get_Range("B2", string.Format("B{0}", 1 + spreadingData_Count));
            series1.XValues = chartSheet.get_Range("A2", string.Format("A{0}", 1 + spreadingData_Count));

            series1.Name = "No. Of Marker";
            series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;//選擇Chart種類
            series1.HasDataLabels = true;
            series1.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowValue);

            //折線圖 作法同上
            Microsoft.Office.Interop.Excel.Series series2 = seriesCollection_Spreading.NewSeries();
            series2.Values = chartSheet.get_Range("C2", string.Format("C{0}", 1 + spreadingData_Count));
            series2.XValues = chartSheet.get_Range("A2", string.Format("A{0}", 1 + spreadingData_Count));

            series2.Name = "% Capacity Fulfillment";
            series2.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

            
            //Cutting---

            //長方圖
            //集合裡面加一筆新的Serires
            Microsoft.Office.Interop.Excel.Series series3 = seriesCollection_Cutting.NewSeries();

            columnHead = MyUtility.Excel.ConvertNumericToExcelColumn(1 + cuttingData_Count);
            //選取資料範圍
            series3.Values = cutting_chartSheet.get_Range("B{0}", string.Format("B{1}", 2 + spreadingData_Count, 2 + spreadingData_Count + cuttingData_Count));
            series3.XValues = cutting_chartSheet.get_Range("A{0}", string.Format("A{1}", 2 + spreadingData_Count, 2 + spreadingData_Count + cuttingData_Count));

            series3.Name = "No. Of Marker";
            series3.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;//選擇Chart種類

            //折線圖 作法同上
            Microsoft.Office.Interop.Excel.Series series4 = seriesCollection_Cutting.NewSeries();
            series2.Values = chartSheet.get_Range("C{0}", string.Format("C{1}", 2 + spreadingData_Count, 2 + spreadingData_Count + cuttingData_Count));
            series2.XValues = chartSheet.get_Range("A{0}", string.Format("A{1}", 2 + spreadingData_Count, 2 + spreadingData_Count + cuttingData_Count));

            series4.Name = "% Capacity Fulfillment";
            series4.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;
            #endregion

            #region Step.3 圖表版面配置


            //版面配置，選擇第9種，還有其他種，可自行嘗試
            chartPage.ApplyLayout(9);
            //設定圖表標題，注意，不先Select會爆炸
            chartPage.ChartTitle.Select();
            chartPage.ChartTitle.Text = "Spreading Balance (Forecast)";


            //開啟運算列表（不知道運算列表是什麼的話，在Excel裡面點選一個圖表，看版面配置→運算列表）
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementDataTableWithLegendKeys);
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementLegendNone);
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementPrimaryCategoryAxisTitleNone);
            chartPage.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementPrimaryValueAxisTitleNone);

            //開啟資料標籤位置（也就是長條圖折線圖旁邊的數字，看版面配置→資料標籤）
            chartPage.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false
                , false, false, false, false, true, false, false, false);

            chartPage_3.ApplyLayout(9);
            //設定圖表標題，注意，不先Select會爆炸
            chartPage_3.ChartTitle.Select();
            chartPage_3.ChartTitle.Text = "Cutting Balance (Forecast)";


            //開啟運算列表（不知道運算列表是什麼的話，在Excel裡面點選一個圖表，看版面配置→運算列表）
            chartPage_3.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementDataTableWithLegendKeys);
            chartPage_3.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementLegendNone);
            chartPage_3.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementPrimaryCategoryAxisTitleNone);
            chartPage_3.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementPrimaryValueAxisTitleNone);

            //開啟資料標籤位置（也就是長條圖折線圖旁邊的數字，看版面配置→資料標籤）
            chartPage_3.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false
                , false, false, false, false, true, false, false, false);

            #endregion

            #region Step.4 開始細項的動態設定，模擬人用滑鼠點擊設定的動作（效能問題，因此不使用）

            ////長條圖的資料標籤 設定
            //objSheets2.Activate();
            //for (int i = 1; i <= printDatas[1].Rows.Count; i++)
            //{
            //    Excel.DataLabel signleDataLabel = series1.DataLabels(i);
            //    signleDataLabel.Position = Excel.XlDataLabelPosition.xlLabelPositionInsideBase;

            //    Excel.DataLabel signleDataLabel_2 = series2.DataLabels(i);
            //    signleDataLabel_2.Position = Excel.XlDataLabelPosition.xlLabelPositionAbove;
            //}

            //長條圖的資料標籤 設定
            //objSheets3.Activate();
            //for (int i = 1; i <= printDatas[2].Rows.Count; i++)
            //{
            //    //一定要先選取到那個Item，因為S開頭資料放在前面，所以用迴圈跑

            //    //Series集合，Item從1開始，1代表長條圖的圖表物件
            //    Excel.DataLabel signleDataLabel = series3.DataLabels(i);
            //    signleDataLabel.Position = Excel.XlDataLabelPosition.xlLabelPositionInsideBase;

            //    Excel.DataLabel signleDataLabel_2 = series4.DataLabels(i);
            //    signleDataLabel_2.Position = Excel.XlDataLabelPosition.xlLabelPositionAbove;
            //}

            #endregion

            #region Step.5 設定欄位的連動


            // Sheet 2起始的X軸位置
            objSheets2.Activate();
            int Spreading_Start_x = 2;   //B開始往右

            columnHead = MyUtility.Excel.ConvertNumericToExcelColumn(Spreading_Start_x);
            //設定Total Available Spreading Time (hrs)：     (Work Hours/Day   *   Total Working Days) *  Avg. Efficiency %  0.8
            objSheets2.Cells[6, Spreading_Start_x] = $"={columnHead}5 * $C$2 * $F$2";
            //設定Spreading Capacity Fulfill Rate%    ：     Total Spreading Time (hrs.)   /   Total Available Spreading Time (hrs)  *100
            objSheets2.Cells[10, Spreading_Start_x] = $"=({columnHead}9 / {columnHead}5  * $C$2 * $F$2) * 100";
            //設定+/- Capacity (hrs)                  ：     Total Spreading Time (hrs.)   -   Total Available Spreading Time (hrs)
            objSheets2.Cells[11, Spreading_Start_x] = $"={columnHead}9 - ({columnHead}5 * $C$2 *$F$2)";
            
            objSheets2.get_Range($"{columnHead}6:{columnHead}6").Copy();
            Excel.Range to1 = objSheets2.get_Range($"{columnHead}6:{MyUtility.Excel.ConvertNumericToExcelColumn(spreadingData_Count + Spreading_Start_x - 1)}6");
            to1.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            objSheets2.get_Range($"{columnHead}10:{columnHead}10").Copy();
            Excel.Range to2 = objSheets2.get_Range($"{columnHead}10:{MyUtility.Excel.ConvertNumericToExcelColumn(spreadingData_Count + Spreading_Start_x - 1)}10");

            to2.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            objSheets2.get_Range($"{columnHead}11:{columnHead}11").Copy();
            Excel.Range to3 = objSheets2.get_Range($"{columnHead}11:{MyUtility.Excel.ConvertNumericToExcelColumn(spreadingData_Count + Spreading_Start_x - 1)}11");
            to3.PasteSpecial(Excel.XlPasteType.xlPasteAll);


            //objSheets3.Activate();
            int Cutting_Start_x = 2;
            columnHead = MyUtility.Excel.ConvertNumericToExcelColumn(Cutting_Start_x);

            ////設定 Total Available Cutting Time (hrs)      ：(Work Hours/Day  *  Total Working Days) *  Avg. Efficiency %  
            //objSheets3.Cells[7, sheet3Start_x] = $"= {columnHead}6 * $C$2 * $F$2";
            ////設定Cutting Capacity Fulfill Rate%           ：Total Cutting Time(hrs.) / Total Available Cutting Time(hrs) * 100
            //objSheets3.Cells[13, sheet3Start_x] = $"=({columnHead}12 / {columnHead}6 * $C$2 * $F$2) * 100";
            ////設定+/- Capacity (hrs)                       ：Total Cutting Time (hrs.)   -    Total Available Cutting Time (hrs)
            //objSheets3.Cells[14, sheet3Start_x] = $"={columnHead}12 - ({columnHead}6 * $C$2 * $F$2)";

            //objSheets3.get_Range($"{columnHead}7:{columnHead}7").Copy();
            //Excel.Range to4 = objSheets3.get_Range($"{columnHead}7:{MyUtility.Excel.ConvertNumericToExcelColumn(cuttingData_Count + sheet3Start_x - 1)}7");
            //to4.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            //objSheets3.get_Range($"{columnHead}13:{columnHead}13").Copy();
            //Excel.Range to5 = objSheets3.get_Range($"{columnHead}13:{MyUtility.Excel.ConvertNumericToExcelColumn(cuttingData_Count + sheet3Start_x - 1)}13");
            //to5.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            //objSheets3.get_Range($"{columnHead}14:{columnHead}14").Copy();
            //Excel.Range to6 = objSheets3.get_Range($"{columnHead}14:{MyUtility.Excel.ConvertNumericToExcelColumn(cuttingData_Count + sheet3Start_x - 1)}14");
            //to6.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            columnHead = MyUtility.Excel.ConvertNumericToExcelColumn(Cutting_Start_x);
            //設定Total Available Cutting Time (hrs)：     (Work Hours/Day   *   Total Working Days) *  Avg. Efficiency %  0.8
            objSheets2.Cells[18, Cutting_Start_x] = $"={columnHead}17 * $C$2 * $F$2";
            //設定Spreading Capacity Fulfill Rate%    ：     Total Cutting Time (hrs.)   /   Total Available Cutting Time (hrs)  *100
            objSheets2.Cells[24, Cutting_Start_x] = $"=({columnHead}23 / {columnHead}17 * $C$2 * $F$2) * 100";
            //設定+/- Capacity (hrs)                  ：     Total Cutting Time (hrs.)   -   Total Available Cutting Time (hrs)
            objSheets2.Cells[25, Cutting_Start_x] = $"={columnHead}23 - ({columnHead}17 * $C$2 *$F$2)";

            objSheets2.get_Range($"{columnHead}18:{columnHead}18").Copy();
            Excel.Range to4 = objSheets2.get_Range($"{columnHead}18:{MyUtility.Excel.ConvertNumericToExcelColumn(cuttingData_Count + Cutting_Start_x - 1)}18");
            to4.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            objSheets2.get_Range($"{columnHead}24:{columnHead}24").Copy();
            Excel.Range to5 = objSheets2.get_Range($"{columnHead}24:{MyUtility.Excel.ConvertNumericToExcelColumn(cuttingData_Count + Cutting_Start_x - 1)}24");

            to5.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            objSheets2.get_Range($"{columnHead}25:{columnHead}25").Copy();
            Excel.Range to6 = objSheets2.get_Range($"{columnHead}25:{MyUtility.Excel.ConvertNumericToExcelColumn(cuttingData_Count + Cutting_Start_x - 1)}25");
            to6.PasteSpecial(Excel.XlPasteType.xlPasteAll);

            objSheets3.Activate();
            //objSheets2.get_Range($"A1:C1").Copy();
            
            for (int i = 0; i <= spreadingData_Count-1; i++)
            {
                columnHead = MyUtility.Excel.ConvertNumericToExcelColumn(2+i);
                objSheets3.Cells[2 + i, 1] = $"'Capacity Forecast Summary'!{columnHead}4";
                objSheets3.Cells[2 + i, 2] = $"'Capacity Forecast Summary'!{columnHead}6";
                objSheets3.Cells[2 + i, 3] = $"'Capacity Forecast Summary'!{columnHead}10";
            }

            for (int i = 0; i <= cuttingData_Count - 1; i++)
            {
                columnHead = MyUtility.Excel.ConvertNumericToExcelColumn(2 + i);
                objSheets3.Cells[2 + spreadingData_Count + i, 1] = $"'Capacity Forecast Summary'!{columnHead}18";
                objSheets3.Cells[2 + spreadingData_Count + i, 2] = $"'Capacity Forecast Summary'!{columnHead}24";
                objSheets3.Cells[2 + spreadingData_Count + i, 3] = $"'Capacity Forecast Summary'!{columnHead}25";
            }
            #endregion

            objSheets.Activate();
            
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
