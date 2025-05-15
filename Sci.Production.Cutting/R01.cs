using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R01 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private string MD;
        private string Factory;
        private string cutSP;
        private DateTime? dateR_CuttingDate1;
        private DateTime? dateR_CuttingDate2;

        /// <summary>
        /// Initializes a new instance of the <see cref="R01"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtM.Text = Env.User.Keyword;
            this.txtFactory.Text = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateEstCutDate.Value1) && MyUtility.Check.Empty(this.txtCuttingSP.Text))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date>, <Cutting SP#> cannot all be empty.");
                return false;
            }

            this.Factory = this.txtFactory.Text;
            this.MD = this.txtM.Text;
            this.dateR_CuttingDate1 = this.dateEstCutDate.Value1;
            this.dateR_CuttingDate2 = this.dateEstCutDate.Value2;
            this.cutSP = this.txtCuttingSP.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string colCutRefno = string.Empty;
            string sourceReportType = string.Empty;
            string sqlMain = string.Empty;
            if (this.radioByCutRef.Checked)
            {
                sqlMain = $@"
select wo.MDivisionID
,wo.FactoryID
,wo.ID
,wo.OrderID
,wo.CutRef
,wo.CutNo
,wo.MarkerName
,wo.MarkerNo
,wo.MarkerLength
,[PatternPanel] = PatternPanel.value
,[FabricPanelCode] = FabricPanelCode.value
,[Article] = Article.value
,wo.ColorID
,wo.Tone
,[Seq] = wo.Seq1 + '-' + wo.Seq2
,wo.EstCutDate
,[SizeRatio] = SizeRatio.value
,[Layer] = sumWo.Layer
,[Qty] = Qty.value
,[Cons] = sumWo.Cons
,wo.SpreadingStatus
,[Laters] = iif( sumWo.Layer - isnull(sumSpreadingLayers.Value,0) < 0, 0 ,sumWo.Layer - isnull(sumSpreadingLayers.Value,0))
,[QtyLater] = QtyLater.value
,[Cons] = wo.ConsPC * QtySizRatio.value * (iif(sumWo.Layer - isnull(sumSpreadingLayers.Value,0) < 0, 0 , sumWo.Layer - isnull(sumSpreadingLayers.Value,0)))
/*
By Cut Ref# 增加的欄位
*/
,wsf.Refno
,wsf.ColorID
,wsf.Roll
,wsf.Dyelot
,wsf.Tone
,wsf.DamageYards
,wsf.ActCutends
,wsf.Variance
from WorkOrderForOutput wo with(nolock)
outer apply(
	SELECT value =  STUFF(
	(
		SELECT distinct ', ' + PatternPanel
		FROM WorkOrderForOutput_PatternPanel wpp with(nolock)
		left JOIN WorkOrderForOutput wo2 with(nolock) ON wpp.WorkOrderForOutputUkey = wo2.Ukey
		WHERE wo2.ID=wo.ID and wo2.CutRef = wo.CutRef
		and wpp.PatternPanel !=''
		GROUP BY wpp.PatternPanel
		FOR XML PATH('')
	), 1, 2, ''
	)
)PatternPanel
outer apply(
	SELECT value = STUFF(
	(
		SELECT distinct ',' + wpp.FabricPanelCode
		FROM WorkOrderForOutput_PatternPanel wpp with(nolock)
		left JOIN WorkOrderForOutput wo2 with(nolock) ON wpp.WorkOrderForOutputUkey = wo2.Ukey
		WHERE wo2.ID=wo.ID and wo2.CutRef = wo.CutRef
		and wpp.FabricPanelCode !=''
		GROUP BY wpp.FabricPanelCode
		FOR XML PATH('')
	), 1, 1, '')
) FabricPanelCode
outer apply(
	SELECT value = STUFF(
	(
		SELECT distinct ', ' + Article
		FROM WorkOrderForOutput_Distribute wd with(nolock)
		left JOIN WorkOrderForOutput wo2 with(nolock) ON wd.WorkOrderForOutputUkey = wo2.Ukey
		WHERE wo2.ID=wo.ID and wo2.CutRef = wo.CutRef
		and wd.Article !=''
		GROUP BY wd.Article
		FOR XML PATH('')
	), 1, 2, '')
)Article
outer apply(
	select value = stuff(
	(
		Select concat(CHAR(10), SizeCode,'/', Qty)
		from (
			select distinct ws.SizeCode, ws.Qty,os.Seq
			From WorkOrderForOutput_SizeRatio ws WITH (NOLOCK) 
			left join Order_SizeCode os with(nolock) on os.SizeCode = ws.SizeCode and os.Id=ws.ID
			left JOIN WorkOrderForOutput wo2 with(nolock) ON ws.WorkOrderForOutputUkey = wo2.Ukey
			WHERE wo2.ID=wo.ID and wo2.CutRef = wo.CutRef
		)a
		order by seq
		For XML path('')
	),1,1,'') 
)SizeRatio
outer apply(
	select value = stuff(
	(
		Select concat(CHAR(10), ws.SizeCode,'/', sum(ws.Qty * wo2.Layer))
			From WorkOrderForOutput_SizeRatio ws WITH (NOLOCK) 
			left join Order_SizeCode os with(nolock) on os.SizeCode = ws.SizeCode and os.Id=ws.ID
			left JOIN WorkOrderForOutput wo2 with(nolock) ON ws.WorkOrderForOutputUkey = wo2.Ukey
			Where wo2.ID = wo.ID and wo2.CutRef = wo.CutRef
			group by ws.SizeCode,os.Seq
			order by os.Seq
			For XML path('')
	),1,1,'') 
)Qty
outer apply(
	select value = sum(SpreadingLayers)
	from WorkOrderForOutput_SpreadingFabric wsf with(nolock)
	where wsf.CutRef = wo.CutRef
	and wsf.POID = wo.ID
)sumSpreadingLayers
outer apply(
	select f.Refno,wsf.ColorID,wsf.Roll,wsf.Dyelot,wsf.Tone
	,DamageYards = sum(wsf.DamageYards),ActCutends = sum(wsf.ActCutends)
	,Variance = (
	    sum(wsf.ActCutends) -
		    (
			    sum(wsf.TicketYards) - 
			    (
				    (
					    sum(wsf.SpreadingLayers) * dbo.MarkerLengthToYDS(wo.MarkerLength)
				    ) --@A
				    - sum(wsf.MergeFabYards) - sum(wsf.DamageYards)
			    )--@B
		    )--@C
    )
	from WorkOrderForOutput_SpreadingFabric wsf with(nolock)
	left join Fabric f with(nolock) on f.SCIRefno = wsf.SCIRefNo
	where wsf.CutRef = wo.CutRef
	and wsf.POID = wo.ID
	group by f.Refno,wsf.ColorID,wsf.Roll,wsf.Dyelot,wsf.Tone
)wsf
outer apply(
	select Layer = sum(Layer), Cons = sum(Cons) 
	from WorkOrderForOutput wo2 with(nolock)
	where wo2.ID = wo.ID and wo2.CutRef = wo.CutRef
)sumWo
outer apply(
	select value = stuff(
	(
		Select concat(CHAR(10), ws.SizeCode, '/', sum(ws.Qty) *  iif( sum(wo2.Layer) - isnull(sumSpreadingLayers.value,0) < 0, 0 ,sum(wo2.Layer) - isnull(sumSpreadingLayers.value,0)))
			From WorkOrderForOutput_SizeRatio ws WITH (NOLOCK) 
			left join Order_SizeCode os with(nolock) on os.SizeCode = ws.SizeCode and os.Id=ws.ID
			LEFT JOIN WorkOrderForOutput wo2 with(nolock) ON ws.WorkOrderForOutputUkey = wo2.Ukey
			Where wo2.ID = wo.ID and wo2.CutRef = wo.CutRef
			group by ws.SizeCode,os.Seq
			order by os.Seq
			For XML path('')
	),1,1,'') 
)QtyLater
outer apply(
	select value = sum(ws.Qty)
	from WorkOrderForOutput_SizeRatio ws with(nolock)
	where ws.WorkOrderForOutputUkey = wo.Ukey
)QtySizRatio
where 1=1
";
            }
            else
            {
                sqlMain = @"
select wo.MDivisionID
,wo.FactoryID
,wo.ID
,wo.OrderID
,wo.CutRef
,wo.CutNo
,wo.MarkerName
,wo.MarkerNo
,wo.MarkerLength
,[PatternPanel] = PatternPanel.value
,[FabricPanelCode] = FabricPanelCode.value
,[Article] = Article.value
,wo.ColorID
,wo.Tone
,[Seq] = wo.Seq1 + '-' + wo.Seq2
,wo.EstCutDate
,[SizeRatio] = SizeRatio.value
,wo.Layer
,[Qty] = Qty.value
,wo.Cons
,wo.SpreadingStatus
,[Laters] = iif( wo.Layer - isnull(sumSpreadingLayers.value,0) < 0, 0 , wo.Layer - isnull(sumSpreadingLayers.value,0))
,[QtyLater] = QtyLater.value
,[Cons] = wo.ConsPC * QtySizRatio.value * (iif( wo.Layer - isnull(sumSpreadingLayers.value,0) < 0, 0 , wo.Layer - isnull(sumSpreadingLayers.value,0)))
from WorkOrderForOutput wo with(nolock)
outer apply(
	SELECT value =  STUFF(
	(
		SELECT ', ' + PatternPanel
		FROM WorkOrderForOutput_PatternPanel wpp with(nolock)
		WHERE wpp.WorkOrderForOutputUkey = wo.Ukey
		and wpp.PatternPanel !=''
		GROUP BY wpp.PatternPanel
		ORDER BY wpp.PatternPanel
		FOR XML PATH('')
	), 1, 2, ''
	)
)PatternPanel
outer apply(
	SELECT value = STUFF(
	(
		SELECT ', ' + FabricPanelCode
		FROM WorkOrderForOutput_PatternPanel wpp with(nolock)
		WHERE wpp.WorkOrderForOutputUkey = wo.Ukey
		and wpp.FabricPanelCode !=''
		GROUP BY wpp.FabricPanelCode
		ORDER BY wpp.FabricPanelCode
		FOR XML PATH('')
	), 1, 2, '')
) FabricPanelCode
outer apply(
	SELECT value = STUFF(
	(
		SELECT ',' + Article
		FROM WorkOrderForOutput_Distribute wd with(nolock)
		WHERE wd.WorkOrderForOutputUkey=wo.Ukey
		and wd.Article !=''
		GROUP BY wd.Article
		ORDER BY wd.Article
		FOR XML PATH('')
	), 1, 2, '')
)Article
outer apply(
	select value = stuff(
	(
		Select concat(CHAR(10), ws.SizeCode,'/', ws.Qty)
		From WorkOrderForOutput_SizeRatio ws WITH (NOLOCK) 
		left join Order_SizeCode os with(nolock) on os.SizeCode = ws.SizeCode and os.Id=ws.ID
		Where ws.WorkOrderForOutputUkey = wo.Ukey
		order by os.Seq
		For XML path('')
	),1,1,'') 
)SizeRatio
outer apply(
	select value = sum(SpreadingLayers)
	from WorkOrderForOutput_SpreadingFabric wsf with(nolock)
	where wsf.CutRef = wo.CutRef
	and wsf.POID = wo.ID
)sumSpreadingLayers
outer apply(
	select value = stuff(
	(
		Select concat(CHAR(10) , ws.SizeCode,'/', ws.Qty * wo.Layer)
			From WorkOrderForOutput_SizeRatio ws WITH (NOLOCK) 
			left join Order_SizeCode os with(nolock) on os.SizeCode = ws.SizeCode and os.Id=ws.ID
			Where wo.ukey = ws.WorkOrderForOutputUkey 
			group by ws.SizeCode,ws.Qty,os.Seq
			order by os.Seq
			For XML path('')
	),1,1,'') 
)Qty
outer apply(
	select value = stuff(
	(
		Select concat(CHAR(10), ws.SizeCode, '/', sum(ws.Qty) *  iif( sum(wo2.Layer) - isnull(sumSpreadingLayers.value,0) < 0, 0 ,sum(wo2.Layer) - isnull(sumSpreadingLayers.value,0)))
			From WorkOrderForOutput_SizeRatio ws WITH (NOLOCK) 
			left join Order_SizeCode os with(nolock) on os.SizeCode = ws.SizeCode and os.Id=ws.ID
			LEFT JOIN WorkOrderForOutput wo2 with(nolock) ON ws.WorkOrderForOutputUkey = wo2.Ukey
			Where wo2.ID = wo.ID and wo2.CutRef = wo.CutRef
			group by ws.SizeCode,os.Seq
			order by os.Seq
			For XML path('')
	),1,1,'') 
)QtyLater
outer apply(
	select value = sum(ws.Qty)
	from WorkOrderForOutput_SizeRatio ws with(nolock)
	where ws.WorkOrderForOutputUkey = wo.Ukey
)QtySizRatio
where 1=1
";
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                sqlMain += Environment.NewLine + $"        and wo.FactoryID = '{this.Factory}'";
            }

            if (!MyUtility.Check.Empty(this.MD))
            {
                sqlMain += Environment.NewLine + $"        and  wo.MDivisionID >=  '{this.MD}'";
            }

            if (!MyUtility.Check.Empty(this.dateR_CuttingDate1))
            {
                sqlMain += $@" and wo.EstCutDate >= '{System.Convert.ToDateTime(this.dateR_CuttingDate1).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.dateR_CuttingDate2))
            {
                sqlMain += $@" and wo.EstCutDate <= '{System.Convert.ToDateTime(this.dateR_CuttingDate2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.cutSP))
            {
                sqlMain += $@" and wo.id = '{this.cutSP}' ";
            }

            if (this.radioByCutRef.Checked)
            {
                sqlMain += @"
group by wo.MDivisionID
,wo.FactoryID
,wo.ID
,wo.OrderID
,wo.CutRef
,wo.CutNo
,wo.MarkerName
,wo.MarkerNo
,wo.MarkerLength
,PatternPanel.value
,FabricPanelCode.value
, Article.value
,wo.ColorID
,wo.Tone
, wo.Seq1,wo.Seq2
,wo.EstCutDate
,SizeRatio.value
,Qty.value
,wo.SpreadingStatus
,QtyLater.value
,sumSpreadingLayers.Value
,QtySizRatio.value
,WO.ConsPC
,wsf.Refno
,wsf.ColorID
,wsf.Roll
,wsf.Dyelot
,wsf.Tone
,wsf.DamageYards
,wsf.ActCutends
,wsf.Variance
,sumWo.Cons,sumWo.Layer
";
            }

            DualResult result = DBProxy.Current.Select(null, sqlMain, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        private bool boolsend = false;

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string excelpath = this.radioBySP.Checked ? "Cutting_R01_CuttingStatusBySP.xltx" : "Cutting_R01_CuttingStatusByCutRef.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + excelpath); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, excelpath, 2, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(this.radioBySP.Checked ? "Cutting_R01_Cutting Status By SP" : "Cutting_R01_Cutting Status By CutRef");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
