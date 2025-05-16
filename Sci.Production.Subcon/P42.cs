using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P42 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.cmbSummaryBy, 2, 1, "0,SP#,1,Article / Size");
            this.txtfactory1.MDivision = this.txtMdivision1;
            this.dispOngoing.BackColor = Color.Yellow;
            this.dispComplete.BackColor = Color.Green;
            this.dispNotYetLoad.BackColor = Color.Red;
            this.displayBox1.BackColor = Color.White;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        private void GridSetup(int summaryType = 0)
        {
            this.grid1.Columns.Clear();
            DataGridViewGeneratorTextColumnSettings sp = new DataGridViewGeneratorTextColumnSettings();
            sp.CellMouseDoubleClick += (s, e) =>
            {
                DataRow drSelected = this.grid1.GetDataRow(e.RowIndex);
                if (drSelected == null)
                {
                    return;
                }

                var frm = new P42_SubProcessStatus(drSelected, summaryType);
                frm.ShowDialog(this);
            };

            DataGridViewGeneratorTextColumnSettings bundleReplacement = new DataGridViewGeneratorTextColumnSettings();
            bundleReplacement.CellMouseDoubleClick += (s, e) =>
            {
                DataRow drSelected = this.grid1.GetDataRow(e.RowIndex);
                if (drSelected == null)
                {
                    return;
                }

                string where = string.Empty;
                string caption = $"SP:{drSelected["OrderID"]} ";
                if (summaryType == 1)
                {
                    where = $"and bd.SizeCode = '{drSelected["SizeCode"]}' and b.Article = '{drSelected["Article"]}'";
                    caption += $" Size:{drSelected["SizeCode"]}' Article :{drSelected["Article"]}";
                }

                string sqlcmd = $@"
select
	Date = bi.DefectUpdateDate,
	BundleNo=bi.BundleNo,
	SubProcess=bi.SubProcessID,
	[Reason Descripotion] = concat(bi.ReasonID, ' ', bdr.Reason),
	[Defect Qty (pcs)]=bi.DefectQty,
	[Replacement Qty (pcs)]=bi.ReplacementQty,
	Status=iif(bi.DefectQty=bi.ReplacementQty,'Complete','')
from dbo.SciMES_BundleInspection bi with(nolock)
inner join Bundle_Detail bd WITH (NOLOCK) on bd.BundleNo = bi.BundleNo
inner join Bundle b with(nolock) on b.id = bd.id
left join dbo.SciMES_BundleDefectReason bdr with(nolock) on bdr.ID = bi.ReasonID and bdr.SubProcessID = bi.SubProcessID
where b.Orderid = '{drSelected["OrderID"]}' 
and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
{where}
";
                DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                    return;
                }

                MyUtility.Msg.ShowMsgGrid_LockScreen(dt, caption: $"{caption} Bundle Replacement");
            };
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true, settings: sp)
            .Text("POID", header: "MotherSP#", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("CustPONo", header: "P.O.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("ProgramID", header: "Program", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
            ;
            if (summaryType == 1)
            {
                this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                ;
            }

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("CutCellid", header: "Cut Cell", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SewLine", header: "Inline Line", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;
            if (summaryType == 0)
            {
                this.Helper.Controls.Grid.Generator(this.grid1)
                .Date("InLineDate", header: "In Line Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("OffLineDate", header: "Off Line Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;
            }
            else
            {
                this.Helper.Controls.Grid.Generator(this.grid1)
                .DateTime("InLineDate", header: "In Line Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .DateTime("OffLineDate", header: "Off Line Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
            }

            this.Helper.Controls.Grid.Generator(this.grid1)
                .Date("LastSewDate", header: "Last Sew. Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("SewQty", header: "Sew. Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("TotalQty", header: "Total Qty", width: Widths.AnsiChars(6), iseditingreadonly: true);

            int subprocessStartColumn = ((DataTable)this.listControlBindingSource1.DataSource).Columns["TotalQty"].Ordinal;
            int bundleReplacementColumn = ((DataTable)this.listControlBindingSource1.DataSource).Columns["BundleReplacement"].Ordinal;
            foreach (DataColumn column in ((DataTable)this.listControlBindingSource1.DataSource).Columns)
            {
                if (column.Ordinal > subprocessStartColumn && column.Ordinal < bundleReplacementColumn)
                {
                    DataGridViewGeneratorTextColumnSettings subprocess = new DataGridViewGeneratorTextColumnSettings();
                    subprocess.CellMouseDoubleClick += (s, e) =>
                    {
                        DataRow drSelected = this.grid1.GetDataRow(e.RowIndex);
                        if (drSelected == null)
                        {
                            return;
                        }

                        this.ShowBundleQtyStatus(drSelected, column.ColumnName, summaryType);
                    };

					/*
                    subprocess.CellFormatting += (s, e) =>
                    {
                        DataRow drSelected = this.grid1.GetDataRow(e.RowIndex);
                        switch (MyUtility.Convert.GetString(e.Value))
                        {
                            case "Complete":
                                e.CellStyle.BackColor = Color.Green;
                                break;
                            case "OnGoing":
                                e.CellStyle.BackColor = Color.Yellow;
                                break;
                            case "Not Yet Load":
                                e.CellStyle.BackColor = Color.Red;
                                break;
                            default:
                                break;
                        }
                    };
					*/

                    subprocess.CellFormatting += (s, e) =>
                    {
                        DataRow drSelected = this.grid1.GetDataRow(e.RowIndex);
                        switch (MyUtility.Convert.GetString(drSelected[column.ColumnName]))
                        {
                            case "Complete":
                                e.CellStyle.BackColor = Color.Green;
                                break;
                            case "OnGoing":
                                e.CellStyle.BackColor = Color.Yellow;
                                break;
                            case "Not Yet Load":
                                e.CellStyle.BackColor = Color.Red;
                                break;
                            default:
                                drSelected[column.ColumnName + "_value"] = DBNull.Value;
                                e.CellStyle.BackColor = Color.White;
                                break;
                        }

                        if (int.TryParse(drSelected[column.ColumnName + "_value"].ToString(), out int processValue))
                        {
                            int.TryParse(drSelected["TotalQty"].ToString(), out int totalQty);

                            if (processValue >= totalQty)
                            {
                                e.CellStyle.BackColor = Color.Green;
                            }
                        }
                    };
                    this.Helper.Controls.Grid.Generator(this.grid1)
                    .Text(column.ColumnName + "_value", header: column.ColumnName, width: Widths.AnsiChars(12), iseditingreadonly: true, settings: subprocess);
                }
            }

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("BundleReplacement", header: "Bundle Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: bundleReplacement)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            DBProxy.Current.DefaultTimeout = 1800; // timeout時間改為30分鐘
            this.listControlBindingSource1.DataSource = null;
            if (MyUtility.Check.Empty(this.txtSp1.Text) && !MyUtility.Check.Empty(this.txtSp2.Text))
            {
                MyUtility.Msg.WarningBox("Must enter SP#1");
                this.txtSp1.Focus();
                return;
            }

            if (!MyUtility.Check.Empty(this.txtSp1.Text) && MyUtility.Check.Empty(this.txtSp2.Text))
            {
                MyUtility.Msg.WarningBox("Must enter SP#2");
                this.txtSp2.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtSp1.Text) &&
                MyUtility.Check.Empty(this.txtPO.Text) &&
                MyUtility.Check.Empty(this.dateSCIDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateInline.Value1) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                !this.dateLastSewDate.HasValue)
            {
                MyUtility.Msg.WarningBox("SP#, P.O., SCI Delivery, Inline Date, Last Sew. Date and Buyer Delivery cannot all empty.");
                return;
            }

            int summaryType = MyUtility.Convert.GetInt(this.cmbSummaryBy.SelectedValue);
            #region where
            string where = string.Empty;
            List<string> whereCategory = new List<string>();
            if (this.chkBulk.Checked)
            {
                whereCategory.Add("'B'");
            }

            if (this.chkSample.Checked)
            {
                whereCategory.Add("'S'");
            }

            if (whereCategory.Count > 0)
            {
                where += $" and o.Category in ({string.Join(",", whereCategory)}) ";
            }

            if (!MyUtility.Check.Empty(this.txtSp1.Text))
            {
                where += $" and o.id between '{this.txtSp1.Text}' and '{this.txtSp2.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtPO.Text))
            {
                where += $" and o.CustPONo  ='{this.txtPO.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtMdivision1.Text))
            {
                where += $" and o.MDivisionID  ='{this.txtMdivision1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                where += $" and o.FactoryID  ='{this.txtfactory1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                where += $" and o.SCIDelivery between '{((DateTime)this.dateSCIDelivery.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDelivery.Value2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where += $" and o.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd")}' ";
            }

            if (this.dateLastSewDate.HasValue)
            {
                where += $" and vsis.LastSewDate between '{((DateTime)this.dateLastSewDate.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateLastSewDate.Value2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.dateInline.Value1))
            {
                if (summaryType == 0)
                {
                    where += $" and o.SewInLine between '{((DateTime)this.dateInline.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateInline.Value2).ToString("yyyy/MM/dd")}' ";
                }
                else
                {
                    where += $" and Inline between '{((DateTime)this.dateInline.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateInline.Value2).ToString("yyyy/MM/dd")}' ";
                }
            }
            #endregion

            string sqlcmd = string.Empty;

            if (summaryType == 0)
            {
                sqlcmd = $@"
select 
	o.MDivisionID,
	o.FactoryID,
	o.ID,
	o.POID,
	o.CustPONo,
	o.ProgramID,
	o.StyleID,
	o.BrandID,
	o.SeasonID,
	w.CutCellid,
	o.SewLine,
	InLineDate=o.SewInLine,
	OffLineDate=o.SewOffLine,
    vsis.LastSewDate,
    vsis.SewQty,
	TotalQty = (select sum(Qty) from Order_Qty oq where oq.id = o.id)
into #tmpOrders
from orders o with(nolock)
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join dbo.View_SewingInfoSP vsis WITH (NOLOCK) on vsis.OrderID = o.ID
outer apply(
	select CutCellid=STUFF((
		select concat(',',w1.CutCellid)
		from(
			select distinct wo.CutCellid
	        from WorkOrderForOutput_Distribute wd with(nolock) 
	        inner join WorkOrderForOutput wo with(nolock) on wo.Ukey = wd.WorkOrderForOutputUkey
			where wd.OrderID = o.ID and isnull(wo.CutCellid,'')<>''
		)w1
		for xml path(''))
	,1,1,'')
)w
where 1=1
{where}

select distinct [Orderid]=o.ID,bdo.BundleNo,s.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,b.IsEXCESS
into #tmpBundleNo
from Bundle_Detail_Order bdo
inner join Bundle b WITH (NOLOCK) on b.id = bdo.Id
inner join #tmpOrders o on bdo.Orderid = o.ID and b.MDivisionID = o.MDivisionID
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
	from SubProcess s
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1 AND s.IsSelection=0
)s
where not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)

select Orderid,BundleNo,SubProcessID,ShowSeq,InOutRule,IsRFIDDefault,IsEXCESS,
	NoBundleCardAfterSubprocess=  isnull(x.NoBundleCardAfterSubprocess,0),
	PostSewingSubProcess=0
into #tmpBundleNo_SubProcess
from #tmpBundleNo b
outer apply(
	select NoBundleCardAfterSubprocess=MAX(cast(NoBundleCardAfterSubprocess as int))
	from Bundle_Detail_art bda WITH (NOLOCK) 
	where bda.bundleno = b.bundleno
)x

union

select Orderid,bda.BundleNo,bda.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,b.IsEXCESS,bda.NoBundleCardAfterSubprocess,bda.PostSewingSubProcess
from #tmpBundleNo b
inner join Bundle_Detail_art bda WITH (NOLOCK) on bda.bundleno = b.bundleno
inner join SubProcess s WITH (NOLOCK) on s.ID = bda.SubprocessId and s.IsRFIDProcess =1

declare @AllSubprocess nvarchar(max)
declare @Col nvarchar(max)
select @AllSubprocess = STUFF((
select concat(',[',s.SubProcessID,']')
from(
	select b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
	from #tmpBundleNo_SubProcess b
	group by b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
)s
order by s.ShowSeq,s.SubProcessID
for xml path('')
),1,1,'')

select @Col = STUFF((
select concat(',[',s.SubProcessID,']=isnull([',s.SubProcessID,'],''No Schedule'')')
from(
	select b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
	from #tmpBundleNo_SubProcess b
	group by b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
)s
order by s.ShowSeq,s.SubProcessID
for xml path('')
),1,1,'')

select
	b.Orderid,
	b.BundleNo,
	b.subProcessid,
	b.InOutRule,
	b.IsEXCESS,
	[HasInComing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then 'true'
						else IIF( bio.InComing IS NOT NULL ,'true','false')
						end,
	[HasOutGoing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and InOutRule = 3  Then 'true'
						else IIF( bio.OutGoing IS NOT NULL ,'true','false')
						end
into #tmpBundleNo_Complete2
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		where bd.BundleNo = bio.BundleNo
		and bio.SubProcessid =  bio.SubProcessId 
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	)
left join BundleInOut bunIOS with (nolock) on bunIOS.BundleNo = b.BundleNo and bunIOS.SubProcessId = 'SORTING' and isnull(bunIOS.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		where bd.BundleNo = bunIOS.BundleNo
		and bunIOS.SubProcessid =  bunIOS.SubProcessId 
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	)
left join BundleInOut bunIOL with (nolock) on bunIOL.BundleNo = b.BundleNo and bunIOL.SubProcessId = 'LOADING' and isnull(bunIOL.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		where bd.BundleNo = bunIOL.BundleNo
		and bunIOL.SubProcessid =  bunIOL.SubProcessId 
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	)
outer apply(select PostSewingSubProcess_SL =iif(isnull(PostSewingSubProcess,0) = 1 and bunIOS.OutGoing is not null and bunIOL.InComing is not null, 1, 0))p


SELECT Orderid,	BundleNo,InOutRule,SubProcessID
	,[CompleteCount]=CompleteCount.Value
	,[NotYetCount]=NotYetCount.Value
	,[OnGoingCount]=OnGoingCount.Value
INTO #tmpBundleNo_Complete
FROM #tmpBundleNo_Complete2 t
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND (
			(HasInComing = 'true' AND HasOutGoing = 'true') OR
			((InOutRule = '1' OR InOutRule = '4' ) AND HasInComing = 'true') OR
			((InOutRule = '2' OR InOutRule = '3' ) AND HasOutGoing = 'true')
		)
)CompleteCount
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND IsEXCESS = 0
	AND (
			(HasInComing = 'false' AND HasOutGoing = 'false') OR
			(InOutRule = '1' AND HasInComing = 'false') OR
			(InOutRule = '2' AND HasOutGoing = 'false')
		)
)NotYetCount
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND ( (HasInComing = 'true' AND HasOutGoing = 'false') OR (HasInComing = 'false' AND HasOutGoing = 'true'))
	AND InOutRule NOT IN ('1','2')
)OnGoingCount

select
	t.Orderid,
	t.SubProcessID,
	Status = CASE WHEN NotYetCount >= 0 AND OnGoingCount = 0 AND CompleteCount=0 then 'Not Yet Load'
				ELSE 'OnGoing'
				END
into #tmp
from #tmpBundleNo_Complete t

select
	bi.BundleNo,
	bi.ReasonID,
	bi.DefectQty,
	bi.ReplacementQty
into #tmpBundleInspection
from dbo.SciMES_BundleInspection bi  with(nolock)
where exists(select 1   from #tmpOrders o 
						inner join Bundle b with(nolock) on o.id = b.Orderid and  b.MDivisionID = o.MDivisionID
						inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.id
						where bi.BundleNo = bd.BundleNo
						and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
						)
and isnull(bi.DefectQty,0) <> isnull(bi.ReplacementQty,0)

declare @sql nvarchar(max)=N'
select Orderid,'+@Col+N'
into #right
from #tmp
PIVOT(min(Status) for SubProcessID in('+@AllSubprocess+N'))as pt

select 
	o.FactoryID,OrderID=o.ID,o.POID,o.CustPONo,o.ProgramID,o.StyleID,o.BrandID,o.SeasonID,o.CutCellid,o.SewLine,o.InLineDate,o.OffLineDate, o.LastSewDate,
    o.SewQty,
	o.TotalQty,
	'+@Col+N'
    ,BundleReplacement=RQ
from #tmpOrders o
inner join #right r on r.Orderid=o.id
outer apply(
    select RQ=stuff((
	     select concat('','',bi.ReasonID,''('',sum(isnull(bi.DefectQty,0)-isnull(bi.ReplacementQty,0)),'')'')
	    from #tmpBundleInspection bi
		inner join Bundle_Detail_Order bdo WITH (NOLOCK) on bdo.Orderid = o.ID and bdo.BundleNo = bi.BundleNo
        inner join Bundle b with(nolock) on b.id = bdo.id
		where o.MDivisionID = b.MDivisionID
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	    group by bi.ReasonID
	    for xml path('''')
    ),1,1,'''')
)BR
'
exec(@sql)

select distinct SubProcessID,s.ShowSeq from #tmpBundleNo_SubProcess s order by s.ShowSeq,s.SubProcessID

drop table #tmpOrders,#tmpBundleNo,#tmpBundleNo_SubProcess,#tmpBundleNo_Complete,#tmp ,#tmpBundleNo_Complete2, #tmpBundleInspection
";
            }
            else
            {
                sqlcmd = $@"
select
	o.MDivisionID,
	o.FactoryID,
	o.ID,
	o.POID,
	o.CustPONo,
	o.ProgramID,
	o.StyleID,
	o.BrandID,
	o.SeasonID,
	oq.Article,
	oq.SizeCode,
	w.CutCellid,
	o.SewLine,
	InLineDate=s.Inline,
	OffLineDate=s.Offline,
    vsis.LastSewDate,
    vsis.SewQty,
	TotalQty = oq.Qty
into #tmpOrders
from orders o with(nolock)
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join Order_Qty oq with(nolock) on oq.ID = o.ID
left join dbo.View_SewingInfoArticleSize vsis WITH (NOLOCK) on vsis.OrderID = oq.ID and vsis.Article = oq.Article and vsis.SizeCode = oq.SizeCode
outer apply(
	select CutCellid=STUFF((
		select concat(',',w1.CutCellid)
		from(
			select distinct wo.CutCellid
	        from WorkOrderForOutput_Distribute wd with(nolock) 
	        inner join WorkOrderForOutput wo with(nolock) on wo.Ukey = wd.WorkOrderForOutputUkey
			where wd.OrderID = o.ID and isnull(wo.CutCellid,'')<>''
		)w1
		for xml path(''))
	,1,1,'')
)w
outer apply(
	select Inline=Min(ss.Inline),Offline=Max(ss.Offline)
	from SewingSchedule_Detail ssd with(nolock)
	inner join SewingSchedule ss with(nolock) on ss.ID = ssd.ID
	where ssd.OrderID = o.ID and ssd.Article = oq.Article and ssd.SizeCode = oq.SizeCode
)s
where 1=1
{where}

select distinct bdo.Orderid,b.Article,bd.Sizecode,bdo.BundleNo,s.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,b.IsEXCESS
into #tmpBundleNo
from Bundle_Detail_Order bdo WITH (NOLOCK)
inner join Bundle b WITH (NOLOCK) on b.id = bdo.Id
inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
inner join #tmpOrders o on bdo.Orderid = o.ID and b.MDivisionID = o.MDivisionID and b.Article = o.Article and bd.Sizecode = o.SizeCode
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
	from SubProcess s
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1 AND s.IsSelection=0
)s
where not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)

select Orderid,Article,Sizecode,BundleNo,SubProcessID,ShowSeq,InOutRule,IsRFIDDefault,b.IsEXCESS,
	NoBundleCardAfterSubprocess= isnull(x.NoBundleCardAfterSubprocess,0),
	PostSewingSubProcess=0
into #tmpBundleNo_SubProcess
from #tmpBundleNo b
outer apply(
	select NoBundleCardAfterSubprocess=MAX(cast(NoBundleCardAfterSubprocess as int))
	from Bundle_Detail_art bda WITH (NOLOCK) 
	where bda.bundleno = b.bundleno
)x

union

select Orderid,Article,Sizecode,bda.BundleNo,bda.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,b.IsEXCESS,
    bda.NoBundleCardAfterSubprocess,bda.PostSewingSubProcess
from #tmpBundleNo b
inner join Bundle_Detail_art bda WITH (NOLOCK) on bda.bundleno = b.bundleno
inner join SubProcess s WITH (NOLOCK) on s.ID = bda.SubprocessId and s.IsRFIDProcess =1

declare @AllSubprocess nvarchar(max)
declare @Col nvarchar(max)
select @AllSubprocess = STUFF((
select concat(',[',s.SubProcessID,']')
from(
	select b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
	from #tmpBundleNo_SubProcess b
	group by b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
)s
order by s.ShowSeq,s.SubProcessID
for xml path('')
),1,1,'')

select @Col = STUFF((
select concat(',[',s.SubProcessID,']=isnull([',s.SubProcessID,'],''No Schedule'')')
from(
	select b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
	from #tmpBundleNo_SubProcess b
	group by b.SubProcessID,b.ShowSeq,b.IsRFIDDefault
)s
order by s.ShowSeq,s.SubProcessID
for xml path('')
),1,1,'')

select
	b.Orderid,
	b.Article,
	b.Sizecode,
	b.BundleNo,
	b.subProcessid,
	b.InOutRule,
	b.IsEXCESS,
	[HasInComing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then 'true'
						else IIF( bio.InComing IS NOT NULL ,'true','false')
						end,
	[HasOutGoing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and InOutRule = 3  Then 'true'
						else IIF( bio.OutGoing IS NOT NULL ,'true','false')
						end
into #tmpBundleNo_Complete2
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		where bd.BundleNo = bio.BundleNo
		and bio.SubProcessid =  bio.SubProcessId 
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	)
left join BundleInOut bunIOS with (nolock) on bunIOS.BundleNo = b.BundleNo and bunIOS.SubProcessId = 'SORTING' and isnull(bunIOS.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		where bd.BundleNo = bunIOS.BundleNo
		and bunIOS.SubProcessid =  bunIOS.SubProcessId 
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	)
left join BundleInOut bunIOL with (nolock) on bunIOL.BundleNo = b.BundleNo and bunIOL.SubProcessId = 'LOADING' and isnull(bunIOL.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		where bd.BundleNo = bunIOL.BundleNo
		and bunIOL.SubProcessid =  bunIOL.SubProcessId 
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	)
outer apply(select PostSewingSubProcess_SL =iif(isnull(PostSewingSubProcess,0) = 1 and bunIOS.OutGoing is not null and bunIOL.InComing is not null, 1, 0))p

SELECT Orderid,	Article,Sizecode,BundleNo,InOutRule,SubProcessID
	,[CompleteCount]=CompleteCount.Value
	,[NotYetCount]=NotYetCount.Value
	,[OnGoingCount]=OnGoingCount.Value
INTO #tmpBundleNo_Complete
FROM #tmpBundleNo_Complete2 t
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND Article=t.Article
	AND Sizecode=t.Sizecode
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND SubProcessID=t.SubProcessID
	AND (
			(HasInComing = 'true' AND HasOutGoing = 'true') OR
			((InOutRule = '1' OR InOutRule = '4') AND HasInComing = 'true') OR
			((InOutRule = '2' OR InOutRule = '3') AND HasOutGoing = 'true')
		)
)CompleteCount
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND Article=t.Article
	AND Sizecode=t.Sizecode
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND SubProcessID=t.SubProcessID
	AND IsEXCESS = 0
	AND (
			(HasInComing = 'false' AND HasOutGoing = 'false') OR
			(InOutRule = '1' AND HasInComing = 'false') OR
			(InOutRule = '2' AND HasOutGoing = 'false')
		)
)NotYetCount
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND Article=t.Article
	AND Sizecode=t.Sizecode
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND SubProcessID=t.SubProcessID
	AND ( (HasInComing = 'true' AND HasOutGoing = 'false') OR (HasInComing = 'false' AND HasOutGoing = 'true'))
	AND InOutRule NOT IN ('1','2')
)OnGoingCount

select
	t.Orderid,
	t.Article,
	t.Sizecode,
	t.SubProcessID,
	Status = CASE WHEN NotYetCount >= 0 AND OnGoingCount = 0 AND CompleteCount=0 then 'Not Yet Load'
				ELSE 'OnGoing'
				END
into #tmp
from #tmpBundleNo_Complete t

select
	bi.BundleNo,
	bi.ReasonID,
	bi.DefectQty,
	bi.ReplacementQty
into #tmpBundleInspection
from dbo.SciMES_BundleInspection bi  with(nolock)
where exists(select 1   from #tmpOrders o 
						inner join Bundle b with(nolock) on b.MDivisionID = o.MDivisionID
						inner join Bundle_Detail_Order bdo WITH (NOLOCK) on b.id = bdo.id and o.id = bdo.Orderid
						where bi.BundleNo = bdo.BundleNo
						and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
						)  and isnull(bi.DefectQty,0) <> isnull(bi.ReplacementQty,0)

declare @sql nvarchar(max)=N'
select Orderid,Article,Sizecode,'+@Col+N'
into #right
from #tmp
PIVOT(min(Status) for SubProcessID in('+@AllSubprocess+N'))as pt

select 
	o.FactoryID,OrderID=o.ID,o.POID,o.CustPONo,o.ProgramID,o.StyleID,o.BrandID,o.SeasonID,o.Article,o.Sizecode,o.CutCellid,o.SewLine,o.InLineDate,o.OffLineDate, o.LastSewDate,
    o.SewQty,
	o.TotalQty,
	'+@Col+N'
    ,BundleReplacement=RQ
from #tmpOrders o
inner join #right r on r.Orderid=o.id and r.Article = o.Article and r.Sizecode = o.Sizecode
outer apply(
    select RQ=stuff((
	    select concat('','',ReasonID,''('',sum(isnull(bi.DefectQty,0)-isnull(bi.ReplacementQty,0)),'')'')
	    from #tmpBundleInspection bi  with(nolock)
		inner join Bundle_Detail_Order bdo WITH (NOLOCK) on bdo.Orderid = o.ID and bdo.BundleNo = bi.BundleNo
        inner join Bundle b with(nolock) on b.id = bdo.id
		inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
		where o.MDivisionID = b.MDivisionID
		and b.Article = o.Article and bd.SizeCode = o.Sizecode
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw where cw.PatternPanel = b.PatternPanel and cw.ID = b.POID)
	    group by ReasonID
	    for xml path('''')
    ),1,1,'''')
)BR
'
exec(@sql)

select distinct SubProcessID,s.ShowSeq from #tmpBundleNo_SubProcess s order by s.ShowSeq,s.SubProcessID

drop table #tmpOrders,#tmpBundleNo,#tmpBundleNo_SubProcess,#tmpBundleNo_Complete,#tmp ,#tmpBundleNo_Complete2, #tmpBundleInspection
";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }
            else
            {
                string[] subprocessIDs = dt[1].AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SubProcessID"])).ToArray();
                bool bySP = summaryType == 0;
                string columns = bySP ? "OrderID" : "OrderID,Article,SizeCode";
                string sqlCmd = $@"
select distinct {columns}, InStartDate = Null,InEndDate = Null,OutStartDate = Null,OutEndDate = Null into #enn from #tmp

select s.OrderID
    , s.SubProcessID
    , TransferTime = MAX(s.TransferTime)
into #tmp_SetQtyBySubprocess_Last
from SetQtyBySubprocess s WITH (NOLOCK)
where exists (select 1 from #enn t where s.OrderID = t.OrderID)
group by s.OrderID, s.SubProcessID

";

                if (bySP)
                {
                    sqlCmd += $@"
select s.OrderID, s.SubprocessID 
	, FinishedQtyBySet = SUM(s.FinishedQtyBySet)
into #tmp_SetQtyBySubprocess
from 
(
    select s.OrderID
        , s.Article
        , s.SizeCode 
        , s.SubprocessID
		, [FinishedQtyBySet] = CASE WHEN p.InOutRule = 2 or p.InOutRule = 3 then MIN(s.OutQtyBySet)
									 WHEN p.InOutRule = 1 or p.InOutRule = 4 then MIN(s.InQtyBySet)
								 ELSE MIN(s.FinishedQtyBySet)
								 END
    from SetQtyBySubprocess s WITH (NOLOCK)
	inner join SubProcess p WITH (NOLOCK) on s.SubprocessID = p.Id
    where exists (select 1 from #tmp_SetQtyBySubprocess_Last t where t.OrderID = s.OrderID and t.SubProcessID = s.SubProcessID and t.TransferTime = s.TransferTime)
    and SubProcessID in ('{string.Join("','", subprocessIDs)}')
	group by s.OrderID, s.Article, s.SizeCode, s.SubprocessID, p.InOutRule
)s
group by s.OrderID, s.SubprocessID
";

                    foreach (string subprocessID in subprocessIDs)
                    {
                        string subprocessIDtmp = PublicPrg.Prgs.SubprocesstmpNoSymbol(subprocessID);
                        sqlCmd += $@"select * into #{subprocessIDtmp} from #tmp_SetQtyBySubprocess where SubprocessID = '{subprocessID}'" + Environment.NewLine;
                    }
                }
                else
                {
                    sqlCmd += $@"
 select s.OrderID
     , s.Article
     , s.SizeCode 
     , s.SubprocessID
	 , [FinishedQtyBySet] = CASE WHEN p.InOutRule = 2 or p.InOutRule = 3 then MIN(s.OutQtyBySet)
									 WHEN p.InOutRule = 1 or p.InOutRule = 4 then MIN(s.InQtyBySet)
								 ELSE MIN(s.FinishedQtyBySet)
								 END
into #tmp_SetQtyBySubprocess
from SetQtyBySubprocess s WITH (NOLOCK)
inner join SubProcess p WITH (NOLOCK) on s.SubprocessID = p.Id
where exists (select 1 from #tmp_SetQtyBySubprocess_Last t where t.OrderID = s.OrderID and t.SubProcessID = s.SubProcessID and t.TransferTime = s.TransferTime)
and SubProcessID in ('{string.Join("','", subprocessIDs)}')
group by s.OrderID, s.Article, s.SizeCode, s.SubprocessID, p.InOutRule
";

                    foreach (string subprocessID in subprocessIDs)
                    {
                        string subprocessIDtmp = PublicPrg.Prgs.SubprocesstmpNoSymbol(subprocessID);
                        sqlCmd += $@"select * into #QtyBySetPerSubprocess{subprocessIDtmp} from #tmp_SetQtyBySubprocess where SubprocessID = '{subprocessID}'" + Environment.NewLine;
                    }
                }

                sqlCmd += "select t.*" + Environment.NewLine;
                string tmpjoin = string.Empty;
                foreach (string subprocessID in subprocessIDs)
                {
                    string subprocessIDtmp = subprocessID.Replace("-", string.Empty); // 把PAD-PRT為PADPRT, 命名#table名稱用
                    sqlCmd += $",[{subprocessID}_value] = {subprocessIDtmp}.FinishedQtyBySet";

                    dt[0].Columns.Add($"{subprocessID}_value", typeof(decimal));

                    if (bySP)
                    {
                        tmpjoin += $@"
left join #{subprocessIDtmp} {subprocessIDtmp} on {subprocessIDtmp}.OrderID = t.OrderID";
                    }
                    else
                    {
                        tmpjoin += $@"
left join #QtyBySetPerSubprocess{subprocessIDtmp} {subprocessIDtmp} on {subprocessIDtmp}.OrderID = t.OrderID and {subprocessIDtmp}.Article = t.Article and  {subprocessIDtmp}.SizeCode = t.SizeCode ";
                    }
                }

                sqlCmd += $@"
from #tmp t
{tmpjoin}
";
                result = MyUtility.Tool.ProcessWithDatatable(dt[0], columns, sqlCmd, out DataTable dtp);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                foreach (DataRow dr in dt[0].Rows)
                {
                    DataRow[] drs = bySP ? dtp.Select($"OrderID = '{dr["OrderID"]}'") : dtp.Select($"OrderID = '{dr["OrderID"]}' and Article = '{dr["Article"]}' and SizeCode = '{dr["SizeCode"]}'");
                    if (drs.Length > 0)
                    {
                        foreach (string subprocessID in subprocessIDs)
                        {
                            dr[$"{subprocessID}_value"] = MyUtility.Convert.GetDecimal(drs[0][$"{subprocessID}_value"]);
                        }
                    }
                }

                this.listControlBindingSource1.DataSource = dt[0];
                this.GridSetup(summaryType);
                DBProxy.Current.DefaultTimeout = 300; // timeout時間改回5分鐘
            }
        }

        private void ShowBundleQtyStatus(DataRow drSelected, string subProcess, int summaryType)
        {
            string sqlcmd = string.Empty;
            string caption = string.Empty;

            if (summaryType == 0)
            {
                sqlcmd = $@"
select   OrderID = bdo.OrderID
        ,bd.BundleNo
        ,s.SubProcessID
        ,s.ShowSeq
        ,s.InOutRule
        ,s.IsRFIDDefault
        ,b.IsEXCESS
        ,bd.PatternDesc
		,SubProcess.SubProcess
        ,b.Article
        ,bd.BundleGroup
        ,bd.SizeCode
		,b.CutRef
		,[FabricKind] = FabricKind.val
		,s.IsSelection
into #tmpBundleNo
from Bundle_Detail_Order bdo WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on bdo.BundleNo = bd.BundleNo
inner join Bundle b with(nolock) on b.id = bd.Id 
left join Cutting_WIPExcludePatternPanel cw with(nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,s.IsSelection
	from SubProcess s with(nolock)
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1 AND s.IsSelection=0
)s
outer apply(
	SELECT top 1 [val] = DD.id + '-' + DD.NAME 
	FROM DropDownList DD  WITH (NOLOCK)
	OUTER apply(
			SELECT OB.kind, 
				OCC.id, 
				OCC.article, 
				OCC.colorid, 
				OCC.fabricpanelcode, 
				OCC.patternpanel 
			FROM order_colorcombo OCC WITH (NOLOCK)
			INNER JOIN order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		) LIST 
		WHERE LIST.id = b.poid
		AND LIST.patternpanel = b.patternpanel 
		AND DD.[type] = 'FabricKind' 
		AND DD.id = LIST.kind 
)FabricKind
outer apply(
	select SubProcess = stuff((
		Select concat('+',SubProcessID)
		From Bundle_Detail_art c WITH (NOLOCK) 
		Where c.bundleno = bd.BundleNo
		group by SubProcessID
		Order by SubProcessID
		For XML path('')
	),1,1,'')
)SubProcess
where bdo.OrderID ='{drSelected["OrderID"]}'
and exists (select 1 from Orders o with(nolock) inner join Factory f WITH (NOLOCK) on o.FactoryID = f.id and f.IsProduceFty = 1 where o.ID = bdo.OrderID and o.MDivisionID = b.MDivisionID)
and cw.ID is null

select   Orderid
        ,BundleNo
        ,SubProcessID
        ,ShowSeq
        ,InOutRule
        ,IsRFIDDefault
        ,IsEXCESS
        ,PatternDesc
	    ,SubProcess
        ,Article
        ,BundleGroup
        ,SizeCode
		,CutRef
	    ,NoBundleCardAfterSubprocess= isnull(x.NoBundleCardAfterSubprocess,0) 
	    ,PostSewingSubProcess=0
		,b.FabricKind
		,b.IsSelection
into #tmpBundleNo_SubProcess
from #tmpBundleNo b
outer apply(
	select NoBundleCardAfterSubprocess=MAX(cast(NoBundleCardAfterSubprocess as int))
	from Bundle_Detail_art bda WITH (NOLOCK) 
	where bda.bundleno = b.bundleno
)x

union

select   Orderid
        ,bda.BundleNo
        ,bda.SubProcessID
        ,s.ShowSeq
        ,s.InOutRule
        ,s.IsRFIDDefault
        ,IsEXCESS
        ,PatternDesc
	    ,SubProcess
        ,Article
        ,BundleGroup
        ,SizeCode
		,CutRef
        ,bda.NoBundleCardAfterSubprocess
        ,bda.PostSewingSubProcess
        ,b.FabricKind
		,b.IsSelection
from #tmpBundleNo b
inner join Bundle_Detail_art bda WITH (NOLOCK) on bda.bundleno = b.bundleno
inner join SubProcess s WITH (NOLOCK) on s.ID = bda.SubprocessId and s.IsRFIDProcess =1

select
	b.Orderid,
	b.BundleNo,
	b.subProcessid,

	IsEXCESS
    ,PatternDesc
	,SubProcess
    ,Article
    ,BundleGroup
    ,SizeCode
	,b.InOutRule 
	,[HasInComing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when b.NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then 'true'
						else IIF( bio.InComing IS NOT NULL ,'true','false')
						end
	,[HasOutGoing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when b.NoBundleCardAfterSubprocess=1 and InOutRule = 3  Then 'true'
						else IIF( bio.OutGoing IS NOT NULL ,'true','false')
						end
	,bio.InComing
	,bio.OutGoing
	,b.CutRef
    ,b.FabricKind
	,b.NoBundleCardAfterSubprocess
into #tmpBundleNo_Complete
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock) 
		inner join Bundle b with (nolock) on b.id = bd.Id
		left join Cutting_WIPExcludePatternPanel cw with (nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
		where bd.BundleNo = bio.BundleNo
		and bio.SubProcessid =  bio.SubProcessId 
		and cw.ID is null
	)
left join BundleInOut bunIOS with (nolock) on bunIOS.BundleNo = b.BundleNo and bunIOS.SubProcessId = 'SORTING' and isnull(bunIOS.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock) 
		inner join Bundle b with (nolock) on b.id = bd.Id
		left join Cutting_WIPExcludePatternPanel cw with (nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
		where bd.BundleNo = bunIOS.BundleNo
		and bunIOS.SubProcessid =  bunIOS.SubProcessId 
		and cw.ID is null
	)
left join BundleInOut bunIOL with (nolock) on bunIOL.BundleNo = b.BundleNo and bunIOL.SubProcessId = 'LOADING' and isnull(bunIOL.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		left join Cutting_WIPExcludePatternPanel cw with (nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
		where bd.BundleNo = bunIOL.BundleNo
		and bunIOL.SubProcessid =  bunIOL.SubProcessId 
		and cw.ID is null
	)
outer apply(select PostSewingSubProcess_SL =iif(isnull(PostSewingSubProcess,0) = 1 and bunIOS.OutGoing is not null and bunIOL.InComing is not null, 1, 0))p
where b.subProcessid='{subProcess}' and((IsSelection = 0) or (b.subProcessid= '{subProcess}' and IsSelection = 1))

select
	 CutRef
    ,[Bundle#]=t.BundleNo
	,b.Qty
	,EXCESS=iif(IsEXCESS=1,'Y','')
	,t.FabricKind
	,QAOutputPerBundle = iif(exists(select 1 from SubProInsRecord with(nolock) where BundleNo = t.BundleNo and SubProcessID = '{subProcess}')
		,isnull(b.Qty, 0) - isnull((select top 1 RejectQty from SubProInsRecord with(nolock) where BundleNo = t.BundleNo and RepairedDatetime is null and SubProcessID = '{subProcess}' order by addDate Desc), 0)
		,0)
    ,PatternDesc
	,SubProcess
    ,Article
    ,BundleGroup
    ,SizeCode
	,Status=case when (HasInComing='true' AND HasOutGoing='true') OR ((InOutRule = '1' OR InOutRule = '4') AND HasInComing='true' ) OR ((InOutRule = '2' OR InOutRule = '3') AND HasOutGoing='true' )then 'Complete'	
			  	 when (HasInComing='false' AND HasOutGoing='false') OR (InOutRule = '1' AND HasInComing='false' ) OR (InOutRule = '2' AND HasOutGoing='false' )then 'Not Yet Load'
			 	 when InOutRule='3' AND HasInComing='true' AND HasOutGoing='false' then 'OnGoing'				
				 when InOutRule='4' AND HasInComing='false' AND HasOutGoing='true' then 'OnGoing'				
				 ELSE 'Not Valid'
			 end
	,[InComing] = FORMAT(t.InComing,'yyyy/MM/dd HH:mm:ss')
	,[OutGoing] = FORMAT(t.OutGoing,'yyyy/MM/dd HH:mm:ss')
    ,ps.PostSewingSubProcess
    ,t.NoBundleCardAfterSubprocess
from #tmpBundleNo_Complete t
outer apply(
	select qty = sum(bd.Qty)
	from Bundle_Detail bd with(nolock)
	where bd.BundleNo = t.BundleNo
)b
outer apply(
    select top 1 PostSewingSubProcess
    from Bundle_Detail_Art bda with(nolock)
    where bda.BundleNo = t.BundleNo
    and bda.subProcessid='{subProcess}'
    and bda.PostSewingSubProcess = 1
)ps

order by t.BundleNo

drop table #tmpBundleNo,#tmpBundleNo_SubProcess,#tmpBundleNo_Complete
";
                caption = $"SubProcess:{subProcess}";
            }
            else
            {
                sqlcmd = $@"
select   OrderID = bdo.OrderID
		,SubProcess.SubProcess
        ,b.Article
        ,bd.Sizecode
        ,bd.BundleNo
        ,s.SubProcessID
        ,s.ShowSeq
        ,s.InOutRule
        ,s.IsRFIDDefault
        ,b.IsEXCESS
        ,bd.PatternDesc
        ,bd.BundleGroup
        ,[BD_SizeCode]=bd.SizeCode
		,b.CutRef
		,[FabricKind] = FabricKind.val
		,s.IsSelection
into #tmpBundleNo
from Bundle_Detail_Order bdo WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on bdo.BundleNo = bd.BundleNo
inner join Bundle b with(nolock) on b.id = bd.Id
left join Cutting_WIPExcludePatternPanel cw with(nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,s.IsSelection
	from SubProcess s WITH (NOLOCK)
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1 AND s.IsSelection=0
)s
outer apply(
	SELECT top 1 [val] = DD.id + '-' + DD.NAME 
	FROM dropdownlist DD WITH (NOLOCK)
	OUTER apply(
			SELECT OB.kind, 
				OCC.id, 
				OCC.article, 
				OCC.colorid, 
				OCC.fabricpanelcode, 
				OCC.patternpanel 
			FROM order_colorcombo OCC WITH (NOLOCK)
			INNER JOIN order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		) LIST 
		WHERE LIST.id = b.poid 
		AND LIST.patternpanel = b.patternpanel 
		AND DD.[type] = 'FabricKind' 
		AND DD.id = LIST.kind 
)FabricKind
outer apply(
	select SubProcess = stuff((
		Select concat('+',SubProcessID)
		From Bundle_Detail_art c WITH (NOLOCK) 
		Where c.bundleno = bd.BundleNo
		group by SubProcessID
		Order by SubProcessID
		For XML path('')
	),1,1,'')
)SubProcess
where bdo.OrderID = '{drSelected["OrderID"]}' and b.Article = '{drSelected["Article"]}' and bd.SizeCode = '{drSelected["SizeCode"]}'
and exists (select 1 from Orders o with(nolock) inner join Factory f WITH (NOLOCK) on o.FactoryID = f.id and f.IsProduceFty = 1 where o.ID = bdo.OrderID and o.MDivisionID = b.MDivisionID)
and cw.ID is null

select Orderid,SubProcess,Article,Sizecode,BundleNo,SubProcessID,ShowSeq,InOutRule,IsRFIDDefault,IsEXCESS
	,NoBundleCardAfterSubprocess= isnull(x.NoBundleCardAfterSubprocess,0) 
	,PostSewingSubProcess=0
    ,PatternDesc
    ,BundleGroup
    ,[BD_SizeCode]
    ,CutRef
	,b.FabricKind
	,b.IsSelection
into #tmpBundleNo_SubProcess
from #tmpBundleNo b
outer apply(
	select NoBundleCardAfterSubprocess=MAX(cast(NoBundleCardAfterSubprocess as int))
	from Bundle_Detail_art bda WITH (NOLOCK) 
	where bda.bundleno = b.bundleno
)x

union

select Orderid,SubProcess,Article,Sizecode,bda.BundleNo,bda.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,IsEXCESS,bda.NoBundleCardAfterSubprocess,bda.PostSewingSubProcess
    ,PatternDesc
    ,BundleGroup
    ,[BD_SizeCode]
    ,CutRef
	,b.FabricKind
	,b.IsSelection
from #tmpBundleNo b
inner join Bundle_Detail_art bda WITH (NOLOCK) on bda.bundleno = b.bundleno
inner join SubProcess s WITH (NOLOCK) on s.ID = bda.SubprocessId and s.IsRFIDProcess =1

select
	b.Orderid,
    SubProcess,
	b.Article,
	b.Sizecode,
	b.BundleNo,
	b.subProcessid,
	IsEXCESS
    ,PatternDesc
    ,BundleGroup
    ,[BD_SizeCode]
	,b.InOutRule 
	,[HasInComing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when b.NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then 'true'
						else IIF( bio.InComing IS NOT NULL ,'true','false')
						end
	,[HasOutGoing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when b.NoBundleCardAfterSubprocess=1 and InOutRule = 3  Then 'true'
						else IIF( bio.OutGoing IS NOT NULL ,'true','false')
						end
	,bio.InComing
	,bio.OutGoing
	,b.CutRef
	,b.FabricKind
	,b.NoBundleCardAfterSubprocess
into #tmpBundleNo_Complete
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		left join Cutting_WIPExcludePatternPanel cw with (nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
		where bd.BundleNo = bio.BundleNo
		and bio.SubProcessid =  bio.SubProcessId 
		and cw.ID is null
	)
left join BundleInOut bunIOS with (nolock) on bunIOS.BundleNo = b.BundleNo and bunIOS.SubProcessId = 'SORTING' and isnull(bunIOS.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		left join Cutting_WIPExcludePatternPanel cw with (nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
		where bd.BundleNo = bunIOS.BundleNo
		and bunIOS.SubProcessid =  bunIOS.SubProcessId 
		and cw.ID is null
	)
left join BundleInOut bunIOL with (nolock) on bunIOL.BundleNo = b.BundleNo and bunIOL.SubProcessId = 'LOADING' and isnull(bunIOL.RFIDProcessLocationID,'') = ''
	and exists(
		select 1
		from Bundle_Detail bd with (nolock)
		inner join Bundle b with (nolock) on b.id = bd.Id
		left join Cutting_WIPExcludePatternPanel cw with (nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
		where bd.BundleNo = bunIOL.BundleNo
		and bunIOL.SubProcessid =  bunIOL.SubProcessId 
		and cw.ID is null
	)
outer apply(select PostSewingSubProcess_SL =iif(isnull(PostSewingSubProcess,0) = 1 and bunIOS.OutGoing is not null and bunIOL.InComing is not null, 1, 0))p
where b.subProcessid='{subProcess}'  and((IsSelection = 0) or (b.subProcessid= '{subProcess}' and IsSelection = 1))

select
	 CutRef
    ,[Bundle#]=t.BundleNo
	,b.Qty
	,EXCESS=iif(IsEXCESS=1,'Y','')
	,t.FabricKind
	,QAOutputPerBundle = iif(exists(select 1 from SubProInsRecord with(nolock) where BundleNo = t.BundleNo and SubProcessID = '{subProcess}')
		,isnull(b.Qty, 0) - isnull((select top 1 RejectQty from SubProInsRecord with(nolock) where BundleNo = t.BundleNo and RepairedDatetime is null and SubProcessID = '{subProcess}' order by addDate Desc), 0)
		,0)
    ,PatternDesc
    ,SubProcess
    ,Article
    ,BundleGroup
    ,SizeCode
	,Status=case when (HasInComing='true' AND HasOutGoing='true') OR ((InOutRule = '1' OR InOutRule = '4') AND HasInComing='true' ) OR ((InOutRule = '2' OR InOutRule = '3') AND HasOutGoing='true' )then 'Complete'	
			  	 when (HasInComing='false' AND HasOutGoing='false') OR (InOutRule = '1' AND HasInComing='false' ) OR (InOutRule = '2' AND HasOutGoing='false' )then 'Not Yet Load'
			 	 when InOutRule='3' AND HasInComing='true' AND HasOutGoing='false' then 'OnGoing'				
				 when InOutRule='4' AND HasInComing='false' AND HasOutGoing='true' then 'OnGoing'				
				 ELSE 'Not Valid'
			 end
	,[InComing] = FORMAT(t.InComing,'yyyy/MM/dd HH:mm:ss')
	,[OutGoing] = FORMAT(t.OutGoing,'yyyy/MM/dd HH:mm:ss')
    ,ps.PostSewingSubProcess
    ,t.NoBundleCardAfterSubprocess
from #tmpBundleNo_Complete t
outer apply(
	select qty=sum(bd.Qty)
	from Bundle_Detail bd with(nolock)
	inner join Bundle b WITH (NOLOCK) on b.id = bd.Id
	left join Cutting_WIPExcludePatternPanel cw with (nolock) on cw.ID = b.POID and cw.PatternPanel = b.PatternPanel
	where bd.BundleNo = t.BundleNo and b.Article = t.Article and b.Sizecode = t.Sizecode
	and cw.ID is null
)b
outer apply(
    select top 1 PostSewingSubProcess
    from Bundle_Detail_Art bda with (nolock)
    where bda.BundleNo = t.BundleNo
    and bda.subProcessid='{subProcess}'
    and bda.PostSewingSubProcess = 1
)ps

drop table #tmpBundleNo,#tmpBundleNo_SubProcess,#tmpBundleNo_Complete
";
                caption = $"SubProcess:{subProcess} - Article:{drSelected["Article"]} - Size:{drSelected["SizeCode"]}";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            #region 準備要傳入元件的Grid

            this.listControlBindingSource2.DataSource = dt;
            this.grid2.Columns.Clear();

            // 準備Grid 2
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Bundle#", header: "BundleNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("PatternDesc", header: "PatternDesc", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("SubProcess", header: "SubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("BundleGroup", header: "BundleGroup", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("EXCESS", header: "EXCESS", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("FabricKind", header: "Fabric Kind", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("QAOutputPerBundle", header: "QA Output Per Bundle", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("OutGoing", header: "Farm Out", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("InComing", header: "Farm In", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Status", header: "Status", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .CheckBox("PostSewingSubProcess", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: false)
            .CheckBox("NoBundleCardAfterSubprocess", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: false)
            ;

            this.grid2.CellFormatting += new DataGridViewCellFormattingEventHandler(this.Grid2_CellFormatting);

            #endregion

            MsgGridForm msgGridForm = new MsgGridForm(this.grid2, caption: caption, eventname: new string[] { "CellFormatting" })
            {
                FormBorderStyle = FormBorderStyle.Sizable,
            };
            msgGridForm.grid1.RowHeadersVisible = true;
            msgGridForm.ControlBox = true;
            msgGridForm.ShowDialog();
        }

        private void Grid2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (MyUtility.Convert.GetString(e.Value))
            {
                case "Complete":
                    e.CellStyle.BackColor = Color.Green;
                    break;
                case "OnGoing":
                    e.CellStyle.BackColor = Color.Yellow;
                    break;
                case "Not Yet Load":
                    e.CellStyle.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }
    }
}
