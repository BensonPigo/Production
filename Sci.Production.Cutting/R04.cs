using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class R04 : Win.Tems.PrintForm
    {
        DataTable printData;
        string MDivision;
        string Factory;
        string CutCell1;
        string CutCell2;
        DateTime? Est_CutDate1;
        DateTime? Est_CutDate2;
        StringBuilder condition_Est_CutDate = new StringBuilder();

        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable WorkOrder;
            DBProxy.Current.Select(null, "select distinct ID from MDivision WITH (NOLOCK) ", out WorkOrder);
            MyUtility.Tool.SetupCombox(this.comboM, 1, WorkOrder);
            this.comboM.Text = Env.User.Keyword;
            this.comboFactory.SetDataSource();
        }

        private void radioByM_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioByM.Checked)
            {
                this.dateEstCutDate.Control2.Enabled = true;
                this.dateEstCutDate.IsRequired = true;
                this.txtCutCellStart.Enabled = this.txtCutCellEnd.Enabled = false;
                this.txtCutCellEnd.Text = string.Empty;
            }
        }

        private void radioByCutCell_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioByCutCell.Checked)
            {
                this.dateEstCutDate.Control2.Enabled = true;
                this.dateEstCutDate.IsRequired = true;
                this.txtCutCellStart.Enabled = this.txtCutCellEnd.Enabled = true;
            }
        }

        private void radioByFactory_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioByFactory.Checked)
            {
                this.labelFactory.Visible = true;
                this.comboFactory.Visible = true;
            }
            else
            {
                this.labelFactory.Visible = false;
                this.comboFactory.Visible = false;
            }
        }

        private void comboM_TextChanged(object sender, EventArgs e)
        {
            this.comboFactory.SetDataSource(this.comboM.Text);
        }

        private void radioByDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioByDetail.Checked)
            {
                this.dateEstCutDate.Control2.Enabled = false;
                this.dateEstCutDate.Control2.Text = string.Empty;
                this.dateEstCutDate.IsRequired = false;
                this.txtCutCellStart.Enabled = this.txtCutCellEnd.Enabled = true;
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            this.MDivision = this.comboM.Text;
            this.Factory = this.comboFactory.Text;
            this.Est_CutDate1 = this.dateEstCutDate.Value1;
            this.Est_CutDate2 = this.dateEstCutDate.Value2;

            // CutCell1 = txt_CutCell1.Text;
            // CutCell2 = txt_CutCell2.Text;
            int c1, c2;
            bool bc1, bc2;
            bc1 = int.TryParse(this.txtCutCellStart.Text.Trim(), out c1);
            if (bc1)
            {
                this.CutCell1 = c1.ToString("D2");
            }
            else
            {
                this.CutCell1 = this.txtCutCellStart.Text.Trim();
            }

            // 若CutCell2為空則=CutCell1
            if (!MyUtility.Check.Empty(this.txtCutCellEnd.Text.Trim()))
            {
                bc2 = int.TryParse(this.txtCutCellEnd.Text.Trim(), out c2);
                if (bc2)
                {
                    this.CutCell2 = c2.ToString("D2");
                }
                else
                {
                    this.CutCell2 = this.txtCutCellEnd.Text.Trim();
                }
            }
            else
            {
                this.CutCell2 = this.CutCell1;
            }

            if (MyUtility.Check.Empty(this.Est_CutDate1))
            {
                MyUtility.Msg.WarningBox("Est. Cut Date can't empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        // 非同步讀取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
create table #dateranges ([EstCutDate] [date])
declare @startDate date = '{0}' 
declare @EndDate date = '{1}'
while @startDate <= @EndDate
begin
	insert into #dateranges values(@startDate)	set @startDate =  DATEADD(DAY, 1,@startDate)
end",
                Convert.ToDateTime(this.Est_CutDate1).ToString("d"), Convert.ToDateTime(this.Est_CutDate2).ToString("d")));

            #region radiobtnByM
            if (this.radioByM.Checked)
            {
                sqlCmd.Append(@"
select wo.id,co.Status,wo.EstCutDate,wo.mdivisionid,co.CDate,c.Finished
into #tmpWO
from WorkOrder WO
INNER join Cutting as c WITH (NOLOCK) on c.ID = wo.ID
outer apply(
	select cDate=min(co.cDate),Status=min(Status)
	from CuttingOutput co WITH (NOLOCK) 
	inner join CuttingOutput_Detail cod WITH (NOLOCK) on co.ID = cod.ID
	where cod.WorkOrderUkey = wo.Ukey and Status != 'New'
) as co
where 1 = 1 AND wo.EstCutDate is not null ");
                if (!MyUtility.Check.Empty(this.MDivision))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", this.MDivision));
                }

                sqlCmd.Append(@"
select wo.id,
	Status = iif(wo.Layer>sl.layer,null,max(co.Status)),
	EstCutDate = iif(wo.Layer>sl.layer,null,max(wo.EstCutDate)),
	wo.mdivisionid,
	CDate = iif(wo.Layer<>sl.layer,null,max(CO.CDate)),
	c.Finished,wo.UKey ,wo.cutref
into #tmpWO2
from WorkOrder WO
Inner Join Cutting C WITH (NOLOCK) on C.ID = wo.ID
left join CuttingOutput_Detail COD WITH (NOLOCK) on wo.UKey = COD.WorkOrderUKey
left Join CuttingOutput CO WITH (NOLOCK) on CO.ID = COD.ID and CO.Status != 'New'
outer apply(
	Select layer = sum(layer) 
	from CuttingOutput_Detail codi WITH (NOLOCK)
	left Join CuttingOutput COi WITH (NOLOCK) on COi.ID = codi.ID
	where WorkOrderUkey = WO.Ukey and COi.Status != 'New'
)sl
where 1 = 1 AND wo.EstCutDate is not null
");
                if (!MyUtility.Check.Empty(this.MDivision))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", this.MDivision));
                }

                sqlCmd.Append(@"
group by wo.id,wo.mdivisionid,c.Finished,wo.UKey,wo.Layer,sl.layer,wo.cutref

select distinct
	[Date] = dr.EstCutDate,
	[M] = m.MDivisionId,
	[Estimate Total# of Cuttings on schedule] = d1.ct,
	[Estimate Total# of Backlog] = d2.ct, 
	[Estimate Total# of Cuttings (C+D)] = d1.ct + d2.ct,
	[Actual Total# of Cuttings on schedule] = d3.ct,
	[Actual Total# of Backlog] = d4.ct,
	[Actual Total# of Cutting early schedule] = isnull(d5.ct,0),
	[Actual Total# of Cuttings (F+G+H)]= d3.ct+d4.ct+isnull(d5.ct,0),
	[BCS Rating % (F+G) / E] = iif(d1.ct+d2.ct = 0, 0, (Round( 100*CAST((d3.ct+d4.ct) as float) / CAST((d1.ct+d2.ct) as float),3)))
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
	where MDivisionId = M.MDivisionId	
	and (EstCutDate < dr.EstCutDate  
		 and ((EstCutDate < CDate and CDate >= dr.EstCutDate) 
			   or (CDate is null and Finished = 0 )		  
			 )
		)
) as d2
outer apply(
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate = dr.EstCutDate 
		and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
) as d3
outer apply(
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate < dr.EstCutDate 
		and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
) as d4
outer apply(	
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate > dr.EstCutDate 
		and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
) as d5
order by dr.EstCutDate

drop table #DateRanges
drop table #tmpWO
");
            }
            #endregion

            #region radioByFactory
            if (this.radioByFactory.Checked)
            {
                sqlCmd.Append(@"
select wo.id,co.Status,wo.EstCutDate,wo.mdivisionid,co.CDate,c.Finished,wo.FactoryID
into #tmpWO
from WorkOrder WO
INNER join Cutting as c WITH (NOLOCK) on c.ID = wo.ID
outer apply(
	select cDate=min(co.cDate),Status=min(Status)
	from CuttingOutput co WITH (NOLOCK) 
	inner join CuttingOutput_Detail cod WITH (NOLOCK) on co.ID = cod.ID
	where cod.WorkOrderUkey = wo.Ukey and Status != 'New'
) as co
where 1 = 1 AND wo.EstCutDate is not null 
");
                if (!MyUtility.Check.Empty(this.MDivision))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", this.MDivision));
                }

                if (!MyUtility.Check.Empty(this.Factory))
                {
                    sqlCmd.Append(string.Format(" and wo.FactoryID = '{0}' ", this.Factory));
                }

                sqlCmd.Append(@"
select wo.id,
	Status = iif(wo.Layer<>sl.layer,null,max(co.Status)),
	EstCutDate = iif(wo.Layer<>sl.layer,null,max(wo.EstCutDate)),
	wo.mdivisionid,
	CDate = iif(wo.Layer<>sl.layer,null,max(CO.CDate)),
	c.Finished,wo.UKey,wo.FactoryID,wo.cutref
into #tmpWO2
from WorkOrder WO
Inner Join Cutting C WITH (NOLOCK) on C.ID = wo.ID
left join CuttingOutput_Detail COD WITH (NOLOCK) on wo.UKey = COD.WorkOrderUKey
left Join CuttingOutput CO WITH (NOLOCK) on CO.ID = COD.ID and CO.Status != 'New'
outer apply(
	Select layer = sum(layer) 
	from CuttingOutput_Detail codi WITH (NOLOCK)
	left Join CuttingOutput COi WITH (NOLOCK) on COi.ID = codi.ID
	where WorkOrderUkey = WO.Ukey and COi.Status != 'New'
)sl
where 1 = 1 AND wo.EstCutDate is not null
");
                if (!MyUtility.Check.Empty(this.MDivision))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", this.MDivision));
                }

                if (!MyUtility.Check.Empty(this.Factory))
                {
                    sqlCmd.Append(string.Format(" and wo.FactoryID = '{0}' ", this.Factory));
                }

                sqlCmd.Append(@"
group by wo.id,wo.mdivisionid,c.Finished,wo.UKey,wo.FactoryID,wo.Layer,sl.layer,wo.cutref

select 
	[Date] = dr.EstCutDate,
	[M] = m.MDivisionId,
    [Factory] = m.FactoryID,
	[Estimate Total# of Cuttings on schedule] = d1.ct,
	[Estimate Total# of Backlog] = d2.ct, 
	[Estimate Total# of Cuttings (C+D)] = d1.ct + d2.ct,
	[Actual Total# of Cuttings on schedule] = d3.ct,
	[Actual Total# of Backlog] = d4.ct,
	[Actual Total# of Cutting early schedule] = isnull(d5.ct,0),
	[Actual Total# of Cuttings (F+G+H)]= d3.ct+d4.ct+isnull(d5.ct,0),
	[BCS Rating % (F+G) / E] = iif(d1.ct+d2.ct = 0, 0, (Round( 100*CAST((d3.ct+d4.ct) as float) / CAST((d1.ct+d2.ct) as float),3)))
from #DateRanges as dr 
outer apply(
	select distinct wo.MDivisionId 
		   , wo.FactoryID
	from #tmpWO as WO
) as M
outer apply(
	select count(*) as ct 
	from #tmpWO
	Where EstCutDate = dr.EstCutDate 
	      and MDivisionId = M.MDivisionId
          and FactoryId = M.FactoryId
) as d1
outer apply(
	select count(*) as ct
	from #tmpWO 
	where MDivisionId = M.MDivisionId	
          and FactoryId = M.FactoryId
	and (EstCutDate < dr.EstCutDate  
		 and ((EstCutDate < CDate and CDate >= dr.EstCutDate) 
			   or (CDate is null and Finished = 0 )		  
			 )
		)
) as d2
outer apply(
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate = dr.EstCutDate 
		  and CDate = dr.EstCutDate
		  and MDivisionId = M.MDivisionId
          and FactoryId = M.FactoryId
) as d3
outer apply(
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate < dr.EstCutDate 
		  and CDate = dr.EstCutDate
		  and MDivisionId = M.MDivisionId
          and FactoryId = M.FactoryId
) as d4
outer apply(
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate > dr.EstCutDate 
		  and CDate = dr.EstCutDate
		  and MDivisionId = M.MDivisionId
          and FactoryId = M.FactoryId
) as d5
order by dr.EstCutDate, m.MDivisionId, m.FactoryID

drop table #DateRanges
drop table #tmpWO
");
            }
            #endregion

            #region radioBtnByCutCell
            if (this.radioByCutCell.Checked)
            {
                sqlCmd.Append(@"
select wo.id,co.Status,wo.EstCutDate,wo.mdivisionid,co.CDate,c.Finished,cc.id ccid,wo.cutcellid
into #tmpWO
from  WorkOrder as wo  
inner join Cutting as c WITH (NOLOCK) on c.ID = wo.ID 
inner join CutCell as cc WITH (NOLOCK) on cc.ID = wo.CutCellID and WO.MDivisionID = cc.MDivisionID
outer apply(
	select cDate=min(co.cDate),Status=min(Status)
	from CuttingOutput co WITH (NOLOCK) 
	inner join CuttingOutput_Detail cod WITH (NOLOCK) on co.ID = cod.ID
	where cod.WorkOrderUkey = wo.Ukey and Status != 'New'
) as co
where 1 = 1  AND wo.EstCutDate is not null
");
                if (!MyUtility.Check.Empty(this.MDivision))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", this.MDivision));
                }

                if (!MyUtility.Check.Empty(this.CutCell1))
                {
                    sqlCmd.Append(string.Format(" and cc.ID >= '{0}' ", this.CutCell1));
                }

                sqlCmd.Append(@"
select wo.id,
	Status = iif(wo.Layer<>sl.layer,null,max(co.Status)),
	EstCutDate = iif(wo.Layer<>sl.layer,null,max(wo.EstCutDate)),
	wo.mdivisionid,
	CDate = iif(wo.Layer<>sl.layer,null,max(CO.CDate)),
	c.Finished,wo.UKey,ccid = cc.id,wo.cutcellid,wo.cutref
into #tmpWO2
from  WorkOrder as wo
Inner Join Cutting C WITH (NOLOCK) on C.ID = wo.ID
inner join CutCell as cc WITH (NOLOCK) on cc.ID = wo.CutCellID and WO.MDivisionID = cc.MDivisionID
left join CuttingOutput_Detail COD WITH (NOLOCK) on wo.UKey = COD.WorkOrderUKey
left Join CuttingOutput CO WITH (NOLOCK) on CO.ID = COD.ID and CO.Status != 'New'
outer apply(
	Select layer = sum(layer) 
	from CuttingOutput_Detail codi WITH (NOLOCK)
	left Join CuttingOutput COi WITH (NOLOCK) on COi.ID = codi.ID
	where WorkOrderUkey = WO.Ukey and COi.Status != 'New'
)sl
where 1 = 1 AND wo.EstCutDate is not null
");
                if (!MyUtility.Check.Empty(this.MDivision))
                {
                    sqlCmd.Append(string.Format(" and wo.mdivisionid = '{0}' ", this.MDivision));
                }

                if (!MyUtility.Check.Empty(this.CutCell1))
                {
                    sqlCmd.Append(string.Format(" and cc.ID >= '{0}' ", this.CutCell1));
                }

                if (!MyUtility.Check.Empty(this.CutCell2))
                {
                    sqlCmd.Append(string.Format(" and cc.ID <= '{0}' ", this.CutCell2));
                }

                sqlCmd.Append(@"
group by wo.id,wo.mdivisionid,c.Finished,wo.UKey,cc.id,wo.cutcellid,wo.Layer,sl.layer,wo.cutref

select DISTINCT
	[Date] = dr.EstCutDate,
	[M] = M.MDivisionId,
	[Cut Cell] = m2.ccid,
	[Estimate Total# of Cuttings on schedule] = d1.ct,
	[Estimate Total# of Backlog] = d2.ct, 
	[Estimate Total# of Cuttings (D+E)] = d1.ct + d2.ct,
	[Actual Total# of Cuttings on schedule] = d3.ct,
	[Actual Total# of Backlog] = d4.ct,
	[Actual Total# of Cutting early schedule] = isnull(d5.ct,0),
	[Actual Total# of Cuttings (G+H+I)]= d3.ct+d4.ct+isnull(d5.ct,0),
	[BCS Rating % (G+H) / F] = iif((d1.ct+d2.ct)=0,0, Round( 100*CAST((d3.ct+d4.ct) as float) / CAST((d1.ct+d2.ct) as float),3))
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
	from #tmpWO2
	Where EstCutDate = dr.EstCutDate 
		and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
		and CutCellID = m2.ccid
) as d3
outer apply(
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate < dr.EstCutDate 
		and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
		and CutCellID = m2.ccid
) as d4
outer apply(
	select count(*) as ct 
	from #tmpWO2
	Where EstCutDate > dr.EstCutDate 
		and CDate = dr.EstCutDate
		and MDivisionId = M.MDivisionId
		and CutCellID = m2.ccid
) as d5
order by m2.ccid, dr.EstCutDate
drop table #DateRanges
drop table #tmpWO
");
            }
#endregion

            #region radiobtn By Detail3
            if (this.radioByDetail.Checked)
            {
                sqlCmd.Append(@"
select
	[M]=wo.MDivisionID,
	[Est. Cutting Date]=wo.EstCutDate,
	[Act. Cutting Date]= iif(wo.Layer>sl.layer,null,Act.CDate),
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

from WorkOrder wo WITH (NOLOCK) 
    Inner Join Cutting C WITH (NOLOCK) on C.ID = wo.ID
outer apply(
    select CDate=max(CDate)
    from CuttingOutput_Detail COD WITH (NOLOCK)
    inner Join CuttingOutput CO WITH (NOLOCK) on CO.ID = COD.ID and CO.Status != 'New'
    where wo.UKey = COD.WorkOrderUKey
)Act
outer apply(Select layer = sum(layer) from CuttingOutput_Detail codi WITH (NOLOCK),CuttingOutput coi WITH (NOLOCK) where WorkOrderUkey = WO.Ukey and coi.id = codi.id and coi.status != 'New')sl
outer apply(
	select AC= (
		Select distinct concat('/', WOD.Article)
		from WorkOrder_Distribute WOD WITH (NOLOCK) 
		where WOD.WorkOrderUKey = wo.UKey
			and WOD.Article != '' 
		for xml path('')
	)
) as woda
	 outer apply(
		Select Sum(Qty) as sqty 
		from WorkOrder_Distribute WITH (NOLOCK) 
		where WorkOrderUKey = wo.UKey
	 )as SQty	 
	 outer apply(
	 select SCQ= (
		Select  concat(',',(wosr.SizeCode+'/'+Convert(varchar,Qty))) 
		from WorkOrder_SizeRatio as wosr WITH (NOLOCK) 
		where wosr.WorkOrderUkey = wo.UKey
		for xml path('')
	 )
	 ) as scadq
where 1=1
	
");
                #region Append條件字串
                if (!MyUtility.Check.Empty(this.MDivision))
                {
                    sqlCmd.Append(string.Format(" and wo.MDivisionID = '{0}'", this.MDivision));
                }

                if (!MyUtility.Check.Empty(this.Est_CutDate1))
                {
                    sqlCmd.Append(string.Format(" and wo.EstCutDate >= '{0}' ", Convert.ToDateTime(this.Est_CutDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.Est_CutDate2))
                {
                    sqlCmd.Append(string.Format(" and wo.EstCutDate <= '{0}' ", Convert.ToDateTime(this.Est_CutDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.CutCell1))
                {
                    sqlCmd.Append(string.Format(" and wo.CutCellid >= '{0}'", this.CutCell1));
                }

                if (!MyUtility.Check.Empty(this.CutCell2))
                {
                    sqlCmd.Append(string.Format(" and wo.CutCellid <= '{0}'", this.CutCell2));
                }

                string d1 = Convert.ToDateTime(this.Est_CutDate1).ToString("d");
                if (!MyUtility.Check.Empty(this.Est_CutDate1))
                {
                    sqlCmd.Append(string.Format(" and (wo.EstCutDate ='{0}' or (wo.EstCutDate< '{1}' and (act.CDate >= '{2}' or (act.cDate is null and C.Finished = 0))))", d1, d1, d1));
                }
                #endregion
                sqlCmd.Append(@"
order by wo.MDivisionID, wo.CutCellID, wo.OrderID, wo.CutRef, wo.Cutno
");
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            #region radiobtn_ByM
            if (this.radioByM.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R04_Cutting BCSReportByM.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Cutting_R04_Cutting BCSReportByM.xltx", 3, false, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.Cells[1, 3] = string.Format(@"{0} ~ {1}", Convert.ToDateTime(this.Est_CutDate1).ToString("d"), Convert.ToDateTime(this.Est_CutDate2).ToString("d")); // 條件字串寫入excel

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Cutting_R04_Cutting BCSReportByM");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objSheets);    // 釋放sheet
                Marshal.ReleaseComObject(objApp);          // 釋放objApp
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            #region ByFactory
            if (this.radioByFactory.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R04_Cutting BCSReportByFactory.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Cutting_R04_Cutting BCSReportByFactory.xltx", 3, false, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.Cells[1, 3] = string.Format(@"{0} ~ {1}", Convert.ToDateTime(this.Est_CutDate1).ToString("d"), Convert.ToDateTime(this.Est_CutDate2).ToString("d")); // 條件字串寫入excel

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Cutting_R04_Cutting BCSReportByFactory");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objSheets);    // 釋放sheet
                Marshal.ReleaseComObject(objApp);          // 釋放objApp
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            if (this.radioByCutCell.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R04_CuttingBCSReportByCutCell.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Cutting_R04_CuttingBCSReportByCutCell.xltx", 3, false, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.Cells[1, 3] = string.Format(@"{0} ~ {1}", Convert.ToDateTime(this.Est_CutDate1).ToString("d"), Convert.ToDateTime(this.Est_CutDate2).ToString("d"));  // 條件字串寫入excel

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Cutting_R04_CuttingBCSReportByCutCell");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objSheets);    // 釋放sheet
                Marshal.ReleaseComObject(objApp);          // 釋放objApp
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }

            if (this.radioByDetail.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R04_Cutting BCS Report ByDetail.xltx"); // 預先開啟excel app

                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Cutting_R04_Cutting BCS Report ByDetail.xltx", 2, false, null, objApp); // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.get_Range("J1").ColumnWidth = 13;
                objSheets.get_Range("M1").ColumnWidth = 7;
                objSheets.get_Range("N1").ColumnWidth = 25.25;
                objSheets.Rows.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Cutting_R04_Cutting BCS Report ByDetail");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objSheets);    // 釋放sheet
                Marshal.ReleaseComObject(objApp);          // 釋放objApp
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }

            return true;
        }
    }
}
