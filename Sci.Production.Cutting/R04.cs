using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class R04 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string WorkOrder,CutCell1,CutCell2;
        DateTime? Est_CutDate1, Est_CutDate2;
        StringBuilder condition_Est_CutDate = new StringBuilder();

        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable WorkOrder;
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrder", out WorkOrder);
            MyUtility.Tool.SetupCombox(cmb_M, 1, WorkOrder);
            cmb_M.Text = Sci.Env.User.Keyword;
        }
        
        private void radiobtn1_ByM_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_ByM.Checked)
            {
                dateR_EstCutDate.Control2.Enabled = true;
                dateR_EstCutDate.IsRequired = true;
                txt_CutCell1.Enabled = txt_CutCell2.Enabled = false;
                txt_CutCell2.Text = "";
            }
        }

        private void radioBtn2_ByCutCell_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_ByCutCell.Checked)
            {
                dateR_EstCutDate.Control2.Enabled = true;
                dateR_EstCutDate.IsRequired = true;
                txt_CutCell1.Enabled = txt_CutCell2.Enabled = true;
            }
        }

        private void radioBtn3_ByDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_ByDetail.Checked)
            {
                dateR_EstCutDate.Control2.Enabled = false;
                dateR_EstCutDate.Control2.Text = "";
                dateR_EstCutDate.IsRequired = false;
                txt_CutCell1.Enabled = txt_CutCell2.Enabled = true;
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            WorkOrder = cmb_M.Text;
            Est_CutDate1 = dateR_EstCutDate.Value1;
            Est_CutDate2 = dateR_EstCutDate.Value2;
            CutCell1 = txt_CutCell1.Text;
            CutCell2 = txt_CutCell2.Text;

            if (MyUtility.Check.Empty(Est_CutDate1))
            {
                MyUtility.Msg.WarningBox("Est. Cut Date can't empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region radiobtnByM
            if (radiobtn_ByM.Checked)
            {
                sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#DateRanges', 'U') IS NOT NULL
  DROP TABLE #DateRanges
 IF OBJECT_ID('tempdb.dbo.#tmpWO', 'U') IS NOT NULL
  DROP TABLE #tmpWO
select distinct wo.EstCutDate 
into #DateRanges 
from workorder wo
where 1 = 1
");
                if (!MyUtility.Check.Empty(Est_CutDate1))
                {
                    sqlCmd.Append(string.Format(" and wo.EstCutDate between '{0}' and '{1}' ", Convert.ToDateTime(Est_CutDate1).ToString("d"), Convert.ToDateTime(Est_CutDate2).ToString("d")));
                }

                sqlCmd.Append(@"
select wo.id,co.Status,wo.EstCutDate, wo.mdivisionid,co.CDate,c.Finished
into #tmpWO
from MDivision
inner join dbo.WorkOrder as wo on MDivision.ID = wo.MDivisionID
inner join CuttingOutput_Detail cod on  cod.WorkOrderUKey = wo.UKey 
inner join CuttingOutput as co on co.ID = cod.ID
inner join Cutting as c on c.ID = wo.ID
where 1 = 1
");
                if (!MyUtility.Check.Empty(WorkOrder))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", WorkOrder));
                }

                sqlCmd.Append(@"
select 
	[Date] = dr.EstCutDate,
	[M] = m.MDivisionId,
	[Estimate Total# of Cuttings on schedule] = d1.ct,
	[Estimate Total# of Backlog] = d2.ct, 
	[Estimate Total# of Cuttings (C+D)] = d1.ct + d2.ct,
	[Actual Total# of Cuttings on schedule] = d3.ct,
	[Actual Total# of Backlog] = d4.ct,
	[Actual Total# of Cutting early schedule] = d5.ct,
	[Actual Total# of Cuttings (F+G+H)]= d3.ct+d4.ct+d5.ct,
	[BCS Rating % (F+G) / E] = iif(d1.ct+d2.ct = 0, 0, (Round( CAST((d3.ct+d4.ct) as float) / CAST((d1.ct+d2.ct) as float),3)))
from #DateRanges as dr 
outer apply(select distinct wo.MDivisionId from #tmpWO as WO) as M
outer apply(
	select count(*) as ct 
	from #tmpWO
	Where EstCutDate = dr.EstCutDate 
	and MDivisionId = M.MDivisionId
) as d1
outer apply(
	select count(*) as ct
	from #tmpWO 
	where (EstCutDate < dr.EstCutDate  
		 and ((EstCutDate < CDate and CDate >= dr.EstCutDate) 
			   or (CDate is null and Finished = 0 )		  
			 )
		)
		and MDivisionId = M.MDivisionId
) as d2
outer apply(
	select count(*) as ct 
	from #tmpWO 
	Where EstCutDate = dr.EstCutDate 
		and CDate < dr.EstCutDate
		and MDivisionId = M.MDivisionId
) as d3
outer apply(
	select count(*) as ct 
	from #tmpWO
	Where EstCutDate < dr.EstCutDate 
		and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
) as d4
outer apply(	
		select count(*) as ct 
		from #tmpWO 
		where Status = 'Confrimed'
			and CDate = dr.EstCutDate
			and CDate < EstCutDate
			and EstCutDate > dr.EstCutDate
			and MDivisionId = M.MDivisionId
) as d5
order by dr.EstCutDate

drop table #DateRanges
drop table #tmpWO
");         
            }
            #endregion

            #region radioBtnByCutCell
            if (radioBtn_ByCutCell.Checked)
            {
                sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#DateRanges', 'U') IS NOT NULL
  DROP TABLE #DateRanges
 IF OBJECT_ID('tempdb.dbo.#tmpWO', 'U') IS NOT NULL
  DROP TABLE #tmpWO
select distinct wo.EstCutDate 
into #DateRanges 
from workorder wo
where 1 = 1
");
                if (!MyUtility.Check.Empty(Est_CutDate1))
                {
                    sqlCmd.Append(string.Format(" and wo.EstCutDate between '{0}' and '{1}' ", Convert.ToDateTime(Est_CutDate1).ToString("d"), Convert.ToDateTime(Est_CutDate2).ToString("d")));
                }

                sqlCmd.Append(@"
select wo.id,co.Status,wo.EstCutDate, wo.mdivisionid,co.CDate,cc.id as ccid,wo.cutcellid,c.Finished
into #tmpWO
from MDivision
inner join WorkOrder as wo on MDivision.ID = wo.MDivisionID and wo.EstCutDate is not null
inner join CutCell as cc on cc.ID = wo.CutCellID and MDivision.ID = cc.MDivisionID
inner join CuttingOutput_Detail cod on  cod.WorkOrderUKey = wo.UKey 
inner join CuttingOutput as co on co.ID = cod.ID
inner join Cutting as c on c.ID = wo.ID 
where 1 = 1
");
                if (!MyUtility.Check.Empty(WorkOrder))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", WorkOrder));
                }

                if (!MyUtility.Check.Empty(CutCell1))
                {
                    sqlCmd.Append(string.Format(" and cc.ID >= {0} ", CutCell1));
                }

                if (!MyUtility.Check.Empty(CutCell2))
                {
                    sqlCmd.Append(string.Format(" and cc.ID <= {0} ", CutCell2));
                }

                sqlCmd.Append(@"
select 
	[Date] = dr.EstCutDate,
	[M] = M.MDivisionId,
	[Cut Cell] = m2.ccid,
	[Estimate Total# of Cuttings on schedule] = d1.ct,
	[Estimate Total# of Backlog] = d2.ct, 
	[Estimate Total# of Cuttings (D+E)] = d1.ct + d2.ct,
	[Actual Total# of Cuttings on schedule] = d3.ct,
	[Actual Total# of Backlog] = d4.ct,
	[Actual Total# of Cutting early schedule] = d5.ct,
	[Actual Total# of Cuttings (G+H+I)]= d3.ct+d4.ct+d5.ct,
	[BCS Rating % (G+H) / F] = iif((d1.ct+d2.ct)=0,0, Round( CAST((d3.ct+d4.ct) as float) / CAST((d1.ct+d2.ct) as float),3))
from #DateRanges as dr 
outer apply(select distinct wo.MDivisionId from #tmpWO as WO) as M
outer apply(select distinct cc.ccid from #tmpWO as cc)as M2
outer apply(
	select count(*) as ct 
	from #tmpWO 
	Where EstCutDate = dr.EstCutDate 
	and MDivisionId = M.MDivisionId
	and CutCellID = m2.ccid
) as d1
outer apply(
	select count(*) as ct
	from #tmpWO
	where (EstCutDate < dr.EstCutDate  
		 and ((EstCutDate < CDate and CDate >= dr.EstCutDate) 
			   or (CDate is null and Finished = 0 )		  
			 )
		)
		and MDivisionId = M.MDivisionId
		and CutCellID = m2.ccid
) as d2
outer apply(
	select count(*) as ct 
	from #tmpWO 
	Where EstCutDate = dr.EstCutDate 
		and CDate < dr.EstCutDate
		and MDivisionId = M.MDivisionId
		and CutCellID = m2.ccid
) as d3
outer apply(
	select count(*) as ct 
	from #tmpWO
	Where EstCutDate < dr.EstCutDate 
	and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
		and CutCellID = m2.ccid
) as d4
outer apply(	
		select count(*) as ct 
		from #tmpWO
		where Status = 'Confrimed'
			and CDate = dr.EstCutDate
			and CDate < EstCutDate
			and EstCutDate > dr.EstCutDate
			and MDivisionId = M.MDivisionId	
			and CutCellID = m2.ccid
) as d5
order by dr.EstCutDate,m2.ccid
drop table #DateRanges
drop table #tmpWO
");
            }
#endregion

            #region radiobtn By Detail3
            if (radioBtn_ByDetail.Checked)
            {
                sqlCmd.Append(@"
select
	[M]=wo.MDivisionID,
	[Est. Cutting Date]=wo.EstCutDate,
	[Act. Cutting Date]=CO.CDate,
	[Master SP#]=wo.ID,
	[SP#]=wo.OrderID,
	[Ref#]=wo.CutRef,
	[Cut#]=wo.Cutno,
	[Cut Cell]=wo.CutCellID,
	[Combination]=wo.FabricCombo,
	[ColorWay]=stuff(woda.Ac,1,1,''),
	[Color]=wo.ColorID,
	[Layers]=wo.Layer,
	[Qty]=SQty.sqty,
	[Ratio]=stuff(scadq.SCQ,1,1,''),
	[Consumption]=wo.Cons,
	[Marker Length]=wo.MarkerLength

from WorkOrder wo
	 left join CuttingOutput_Detail COD on wo.UKey = COD.WorkOrderUKey
	 left Join CuttingOutput CO on CO.ID = COD.ID and CO.Status = 'Confirmed'
	 Inner Join Cutting C on C.ID = wo.ID
outer apply(
	select AC= (
		Select distinct concat('/', WOD.Article)
		from WorkOrder_Distribute WOD
		where WOD.WorkOrderUKey = wo.UKey
			and WOD.Article != '' 
		for xml path('')
	)
) as woda
	 outer apply(
		Select Sum(Qty) as sqty 
		from WorkOrder_Distribute 
		where WorkOrderUKey = wo.UKey
	 )as SQty	 
	 outer apply(
	 select SCQ= (
		Select  concat(',',(wosr.SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrder_SizeRatio as wosr
		where wosr.WorkOrderUkey = wo.UKey
		for xml path('')
	 )
	 ) as scadq
where 1=1
	
");
                #region Append條件字串
                if (!MyUtility.Check.Empty(WorkOrder))
                {
                    sqlCmd.Append(string.Format(" and wo.MDivisionID = '{0}'", WorkOrder));
                }
                if (!MyUtility.Check.Empty(Est_CutDate1))
                {
                    sqlCmd.Append(string.Format(" and wo.EstCutDate <= '{0}' and wo.EstCutDate is not null   ", Convert.ToDateTime(Est_CutDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(CutCell1))
                {
                    sqlCmd.Append(string.Format(" and wo.CutCellid >= '{0}'", CutCell1));
                }
                if (!MyUtility.Check.Empty(CutCell2))
                {
                    sqlCmd.Append(string.Format(" and wo.CutCellid <= '{0}'", CutCell2));
                }

                string  d1= Convert.ToDateTime(Est_CutDate1).ToString("d");
                if (!MyUtility.Check.Empty(Est_CutDate1))
                {
                    sqlCmd.Append(string.Format(" and (wo.EstCutDate ='{0}' or (wo.EstCutDate< '{1}' and (CO.CDate >= '{2}' or (CO.cDate is null and C.Finished = 0))))", d1,d1,d1));
                }
                #endregion
                sqlCmd.Append(@"
order by wo.MDivisionID, wo.CutCellID, wo.OrderID, wo.CutRef, wo.Cutno
");
            }
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
            #region radiobtn_ByM
            if (radiobtn_ByM.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R04_Cutting BCSReportByFactory.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Cutting_R04_Cutting BCSReportByFactory.xltx", 3, true, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.Cells[1, 3] = string.Format(@"{0} ~ {1}", Convert.ToDateTime(Est_CutDate1).ToString("d"), Convert.ToDateTime(Est_CutDate2).ToString("d"));// 條件字串寫入excel
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            if (radioBtn_ByCutCell.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R04_CuttingBCSReportByCutCell.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Cutting_R04_CuttingBCSReportByCutCell.xltx", 3, true, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.Cells[1, 3] = string.Format(@"{0} ~ {1}", Convert.ToDateTime(Est_CutDate1).ToString("d"), Convert.ToDateTime(Est_CutDate2).ToString("d"));  // 條件字串寫入excel
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }

            if (radioBtn_ByDetail.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R04_Cutting BCS Report ByDetail.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Cutting_R04_Cutting BCS Report ByDetail.xltx", 1, true, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp            
            }

            return true;
        }        
    }
}
