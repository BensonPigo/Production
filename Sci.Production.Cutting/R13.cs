using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R13 : Win.Tems.PrintForm
    {
        private string mMDivision;
        private string factory;
        private string CuttingSP1;
        private string CuttingSP2;
        private string Style;
        private DateTime? Est_CutDate1;
        private DateTime? Est_CutDate2;
        private DateTime? ActCuttingDate1;
        private DateTime? ActCuttingDate2;
        private string strWhere;
        private DataTable printData;

        /// <inheritdoc/>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrder WITH (NOLOCK) ", out DataTable workOrder);
            MyUtility.Tool.SetupCombox(this.comboMDivision, 1, workOrder);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out DataTable factory); // 要預設空白
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboMDivision.Text = Env.User.Keyword;
            this.comboFactory.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strWhere = string.Empty;
            this.mMDivision = this.comboMDivision.Text;
            this.factory = this.comboFactory.Text;
            this.Style = this.txtstyle.Text;
            this.CuttingSP1 = this.txtCuttingSPStart.Text;
            this.CuttingSP2 = this.txtCuttingSPEnd.Text;
            this.Est_CutDate1 = this.dateEstCutDate.Value1;
            this.Est_CutDate2 = this.dateEstCutDate.Value2;
            this.ActCuttingDate1 = this.dateActCuttingDate.Value1;
            this.ActCuttingDate2 = this.dateActCuttingDate.Value2;

            if (MyUtility.Check.Empty(this.Est_CutDate1) &&
                MyUtility.Check.Empty(this.Est_CutDate1) &&
                MyUtility.Check.Empty(this.CuttingSP1) &&
                MyUtility.Check.Empty(this.CuttingSP2) &&
                MyUtility.Check.Empty(this.ActCuttingDate1) &&
                MyUtility.Check.Empty(this.ActCuttingDate2))
            {
                MyUtility.Msg.WarningBox("Please input  one of [Est. Cut Date]、[Act. Cutting Date]、[Cutting SP#] first!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.mMDivision))
            {
                this.strWhere += $@" and wo.MDivisionID = '{this.mMDivision}' ";
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                this.strWhere += $@" and wo.FactoryID = '{this.factory}' ";
            }

            if (!MyUtility.Check.Empty(this.Style))
            {
                this.strWhere += $@" and o.StyleID = '{this.Style}' ";
            }

            if (!MyUtility.Check.Empty(this.CuttingSP1))
            {
                this.strWhere += $@" and wo.ID >= '{this.CuttingSP1}' ";
            }

            if (!MyUtility.Check.Empty(this.CuttingSP2))
            {
                this.strWhere += $@" and wo.ID <= '{this.CuttingSP2}' ";
            }

            if (!MyUtility.Check.Empty(this.Est_CutDate1))
            {
                this.strWhere += $@" and wo.EstCutDate >= cast('{Convert.ToDateTime(this.Est_CutDate1).ToString("yyyy/MM/dd")}' as date) ";
            }

            if (!MyUtility.Check.Empty(this.Est_CutDate2))
            {
                this.strWhere += $@" and wo.EstCutDate <= cast('{Convert.ToDateTime(this.Est_CutDate2).ToString("yyyy/MM/dd")}' as date) ";
            }

            if (!MyUtility.Check.Empty(this.ActCuttingDate1))
            {
                this.strWhere += $@" and MincDate.MincoDate >= cast('{Convert.ToDateTime(this.ActCuttingDate1).ToString("yyyy/MM/dd")}' as date) ";
            }

            if (!MyUtility.Check.Empty(this.ActCuttingDate2))
            {
                this.strWhere += $@" and MincDate.MincoDate <= cast('{Convert.ToDateTime(this.ActCuttingDate2).ToString("yyyy/MM/dd")}' as date) ";
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = $@"
            select 
            [M] = wo.MDivisionID,
            [Factory] = wo.FactoryID,
            [Fabrication] = f.WeaveTypeID,
            [Est.Cutting Date]= wo.EstCutDate,
            [Act.Cutting Date] = MincDate.MincoDate,
            [Earliest Sewing Inline] = c.SewInLine,
            [Sewing Inline(SP)] = o.SewInLine,
            [Master SP#] = wo.ID, 
            [SP#] = wo.OrderID,
            [Brand]=o.BrandID,
            [Style#] = o.StyleID,
            [FabRef#] = wo.Refno,
            [Switch to Workorder] = iif(c.WorkType='1','Combination',Iif(c.WorkType='2','By SP#','')),
            [Ref#] = wo.CutRef,
            [Cut#] = wo.Cutno,
            [SpreadingNoID]=wo.SpreadingNoID,
            [Cut Cell] = wo.CutCellID,
            [Combination] = wo.FabricCombo,
            [Layers] = sum(wo.Layer),
            [LackingLayers] = sum(wo.Layer) - sum(isnull(acc.val,0)),
            [Ratio] = stuff(SQty.val,1,1,''),
            [Consumption] = sum(wo.cons) ,
            [Act. Cons. Output] = sum(cast(isnull(iif(wo.Layer - isnull(acc.val,0) = 0, wo.Cons, acc.val * dbo.MarkerLengthToYDS(wo.MarkerLength)),0) as numeric(9,4))) ,
            [Balance Cons.] = sum(wo.cons) - sum(cast(isnull(iif(wo.Layer - isnull(acc.val,0) = 0, wo.Cons, acc.val * dbo.MarkerLengthToYDS(wo.MarkerLength)),0) as numeric(9,4))),	
            [Marker Name] = wo.Markername,
            [Marker No.] = wo.MarkerNo,
            [Marker Length] = wo.MarkerLength,
            [Cutting Perimeter] = wo.ActCuttingPerimeter,
            [Cutting Perimeter(Decimal)] = dbo.GetActualPerimeterYd(wo.ActCuttingPerimeter),
            [Straight Length] = wo.StraightLength,
            [Straight Length (Decimal)] = dbo.GetActualPerimeterYd(wo.StraightLength),
            [Curved Length] = wo.CurvedLength,
            [Curved Length (Decimal)] = dbo.GetActualPerimeterYd(wo.CurvedLength),
            [Delay Reason] =dw.[Name],
            [Remark] = wo.Remark,
            fi.avgInQty,
            f.WeaveTypeID,
            [workorderSeq1] =wo.seq1,
            [workorderSeq2] =wo.seq2
            into #tmp
            from WorkOrder wo WITH (NOLOCK) 
            left join Orders o WITH (NOLOCK) on o.id = wo.OrderID
            left join Cutting c WITH (NOLOCK) on c.ID = o.CuttingSP
            left join DropDownList dw with (nolock) on dw.Type = 'PMS_UnFinCutReason' and dw.ID = wo.UnfinishedCuttingReason
            left join fabric f WITH (NOLOCK) on f.SCIRefno = wo.SCIRefno
            left join PO_Supp_Detail psd with(nolock) on psd.id = wo.id and psd.seq1 = wo.seq1 and psd.seq2 = wo.seq2
            outer apply(select val = sum(aa.Layer) from cuttingoutput_Detail aa WITH (NOLOCK) where aa.WorkOrderUkey = wo.Ukey)acc
            outer apply(
                Select MincoDate = iif(sum(cod.Layer) = wo.Layer, Max(co.cdate),null)
	            From cuttingoutput co WITH (NOLOCK) 
	            inner join cuttingoutput_detail cod WITH (NOLOCK) on co.id = cod.id
	            Where cod.workorderukey = wo.Ukey and co.Status != 'New' 
            )MincDate
            outer apply(
	            select val = (
		            select distinct concat(',',SizeCode+'/'+Convert(varchar,Qty))
		            from WorkOrder_SizeRatio WITH (NOLOCK) 
		            where WorkOrderUkey = wo.UKey
		            for xml path('')
	            )
            )as SQty
            outer apply(
	            select avgInQty = avg(fi.InQty)
	            from PO_Supp_Detail psd with(nolock)
	            left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
	            where psd.ID = wo.id and psd.SCIRefno = wo.SCIRefno
	            and fi.InQty is not null
            )fi

            where 1=1
            {this.strWhere}
            group by wo.MDivisionId,wo.FactoryID,f.WeaveTypeID,wo.EstCutDate,MincDate.MincoDate,c.SewInLine,o.SewInLine,wo.ID,wo.OrderID,
		             o.BrandID,o.StyleID,wo.Refno,c.WorkType,wo.CutRef,wo.Cutno,wo.SpreadingNoID,wo.CutCellid,wo.FabricCombo,sqty.val,
		             wo.Markername,wo.MarkerNo,wo.MarkerLength,wo.ActCuttingPerimeter,wo.StraightLength,wo.CurvedLength,dw.[Name],wo.Remark
		            ,fi.avgInQty,wo.seq1,wo.seq2

            select 
            *, 
            [Spreading Time (mins)] =  st.st,
            [Cutting Time (mins)] =  ct.ct,
            [Marker Width] =MW.MW
            into #tmp1
            from #tmp
            outer apply
            (
	            select st =
	            cast(round(isnull(dbo.GetSpreadingTime(
						            Fabrication,
						            FabRef#,
						            iif(iif(isnull(avgInQty,0)=0,1,round(iif(isnull(Ref#,'')='',Consumption,sum(Consumption)over(partition by Ref#,M,[Master SP#]))/avgInQty,0))<1,1,
							            iif(isnull(avgInQty,0)=0,1,round(iif(isnull(Ref#,'')='',Consumption,sum(Consumption)over(partition by Ref#,M,[Master SP#]))/avgInQty,0))
							            ),
						            iif(isnull(Ref#,'')='',Layers,sum(Layers)over(partition by Ref#,M,[Master SP#])),
						            iif(isnull(Ref#,'')='',Consumption,sum(Consumption)over(partition by Ref#,M,[Master SP#])),
						            1
					            )/60.0,0),2)as float)
            )st
            outer apply
            (
	             select ct =
	             cast(round(isnull(
						            dbo.GetCuttingTime(
						            round(dbo.GetActualPerimeter(iif([Cutting Perimeter] not like '%yd%','0',[Cutting Perimeter])),4),
						            [Cut Cell],
						            iif(isnull([Ref#],'')='',Layers,sum(Layers)over(partition by Ref#,M,[Master SP#])),
						            WeaveTypeID,
						            iif(isnull([Ref#],'')='',Consumption,sum(Consumption)over(partition by  Ref#,M,[Master SP#]))
						            )/60.0,0),2)as float)
            ) ct
            outer apply(
	            SELECT top 1  MW = OBE.Width
	            FROM Order_BOF OB WITH (NOLOCK)
	            INNER JOIN Order_BOF_Expend OBE WITH (NOLOCK) ON OBE.Order_BOFUkey = OB.Ukey
	            INNER JOIN PO_Supp PS WITH (NOLOCK) ON PS.ID = OB.Id 
	            INNER JOIN PO_Supp_Detail PSD WITH (NOLOCK) ON PSD.ID= OB.Id AND PSD.RefNo = OB.Refno AND PSD.ColorID = OBE.ColorId 
	            WHERE PSD.ID = [Master SP#] AND PSD.SEQ1=workorderSeq1 AND PSD.SEQ2=workorderSeq2
            )MW
            drop table #tmp

            select 
            [M],
            [Factory],
            [Fabrication],
            [Est.Cutting Date],
            [Act.Cutting Date],
            [Earliest Sewing Inline],
            [Sewing Inline(SP)],
            [Master SP#], 
            [SP#],
            [Brand],
            [Style#],
            [FabRef#],
            [Switch to Workorder],
            [Ref#],
            [Cut#],
            [SpreadingNoID],
            [Cut Cell],
            [Combination],
			[Layer] = sum([Layers]),
            [Layers Level]= case when sum([Layers]) between 1 and 5 then '1~5'
						        when sum([Layers]) between 6 and 10 then '6~10'
						        when sum([Layers]) between 11 and 15 then '11~15'
						        when sum([Layers]) between 16 and 30 then '16~30'
						        when sum([Layers]) between 31 and 50 then '31~50'
						        else '50 above'
						        end ,
            [LackingLayers],
            [Ratio],
            sum([Consumption]),
            sum([Act. Cons. Output]),
            sum([Balance Cons.]),
            sum([Spreading Time (mins)]),
            sum([Cutting Time (mins)]),
            [Marker Name],
            [Marker No.],
            [Marker Width],
            [Marker Length],
            [Cutting Perimeter],
            [Cutting Perimeter(Decimal)],
            [Straight Length],
            [Straight Length (Decimal)],
            [Curved Length],
            [Curved Length (Decimal)],
            [Delay Reason],
            [Remark]
            from #tmp1
			
			group by [M],[Factory],[Fabrication],[Est.Cutting Date],[Act.Cutting Date],[Earliest Sewing Inline],
            [Sewing Inline(SP)],[Master SP#],[SP#],[Brand],[Style#],[FabRef#],[Switch to Workorder],[Ref#],
            [Cut#],[SpreadingNoID],[Cut Cell],[Combination],[LackingLayers],[Ratio],[Marker Name],
            [Marker No.],[Marker Width], [Marker Length],[Cutting Perimeter],[Cutting Perimeter(Decimal)],
            [Straight Length],[Straight Length (Decimal)],[Curved Length],
            [Curved Length (Decimal)],[Delay Reason],[Remark]
            drop table #tmp1";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R13.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Cutting_R13.xltx", 1, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Cutting_R13");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
