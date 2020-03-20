using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Subcon
{
    public partial class P42 : Sci.Win.Tems.QueryForm
    {
        public P42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(this.cmbSummaryBy, 2, 1, "0,SP#,1,Article / Size");
            this.txtfactory1.MDivision = txtMdivision1;
        }

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

                var frm = new Sci.Production.Subcon.P42_SubProcessStatus(drSelected, summaryType);
                frm.ShowDialog(this);
            };

            DataGridViewGeneratorTextColumnSettings BundleReplacement = new DataGridViewGeneratorTextColumnSettings();
            BundleReplacement.CellMouseDoubleClick += (s, e) =>
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
{where}
";
                DataTable dt;
                DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out dt);
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

            int subprocessStartColumn = ((DataTable)this.listControlBindingSource1.DataSource).Columns["OffLineDate"].Ordinal;
            int BundleReplacementColumn = ((DataTable)this.listControlBindingSource1.DataSource).Columns["BundleReplacement"].Ordinal;
            foreach (DataColumn column in ((DataTable)this.listControlBindingSource1.DataSource).Columns)
            {
                if (column.Ordinal > subprocessStartColumn && column.Ordinal < BundleReplacementColumn)
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
                    this.Helper.Controls.Grid.Generator(this.grid1)
                    .Text(column.ColumnName, header: column.ColumnName, width: Widths.AnsiChars(12), iseditingreadonly: true, settings: subprocess);
                }
            }

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("BundleReplacement", header: "Bundle Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: BundleReplacement)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
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
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("SP#, P.O., SCI Delivery, Inline Date and Buyer Delivery cannot all empty.");
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
                where += $" and o.SCIDelivery between '{((DateTime)this.dateSCIDelivery.Value1).ToString("d")}' and '{((DateTime)this.dateSCIDelivery.Value2).ToString("d")}' ";
            }
            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where += $" and o.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery.Value1).ToString("d")}' and '{((DateTime)this.dateBuyerDelivery.Value2).ToString("d")}' ";
            }
            if (!MyUtility.Check.Empty(this.dateInline.Value1))
            {
                if (summaryType == 0)
                {
                    where += $" and o.SewInLine between '{((DateTime)this.dateInline.Value1).ToString("d")}' and '{((DateTime)this.dateInline.Value2).ToString("d")}' ";
                }
                else
                {
                    where += $" and Inline between '{((DateTime)this.dateInline.Value1).ToString("d")}' and '{((DateTime)this.dateInline.Value2).ToString("d")}' ";
                }           
            }
            #endregion

            string sqlcmd = string.Empty;

            if (summaryType == 0)
            {
                sqlcmd = $@"select 
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
	OffLineDate=o.SewOffLine
into #tmpOrders
from orders o with(nolock)
outer apply(
	select CutCellid=STUFF((
		select concat(',',w1.CutCellid)
		from(
			select distinct wo.CutCellid
	        from WorkOrder_Distribute wd with(nolock) 
	        inner join WorkOrder wo with(nolock) on wo.Ukey = wd.WorkOrderUkey
			where wd.OrderID = o.ID and isnull(wo.CutCellid,'')<>''
		)w1
		for xml path(''))
	,1,1,'')
)w
where 1=1
{where}

select b.Orderid,bd.BundleNo,s.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
into #tmpBundleNo
from Bundle b with(nolock)
inner join #tmpOrders o on b.Orderid = o.ID and  b.MDivisionID = o.MDivisionID
inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
	from SubProcess s
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1
)s

select Orderid,BundleNo,SubProcessID,ShowSeq,InOutRule,IsRFIDDefault,NoBundleCardAfterSubprocess=0,PostSewingSubProcess=0
into #tmpBundleNo_SubProcess
from #tmpBundleNo

union

select Orderid,bda.BundleNo,bda.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,bda.NoBundleCardAfterSubprocess,bda.PostSewingSubProcess
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
	b.InOutRule 
	,[HasInComing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then 'true'
						else IIF( bio.InComing IS NOT NULL ,'true','false')
						end
	,[HasOutGoing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and InOutRule = 3  Then 'true'
						else IIF( bio.OutGoing IS NOT NULL ,'true','false')
						end
into #tmpBundleNo_Complete2
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
left join BundleInOut bunIOS with (nolock) on bunIOS.BundleNo = b.BundleNo and bunIOS.SubProcessId = 'SORTING' and isnull(bunIOS.RFIDProcessLocationID,'') = ''
left join BundleInOut bunIOL with (nolock) on bunIOL.BundleNo = b.BundleNo and bunIOL.SubProcessId = 'LOADING' and isnull(bunIOL.RFIDProcessLocationID,'') = ''
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
	AND SubProcessID=t.SubProcessID
	AND (
			(HasInComing='true' AND HasOutGoing='true') OR
			(InOutRule = '1' AND HasInComing='true') OR
			(InOutRule = '2' AND HasOutGoing='true')
		)
)CompleteCount
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND SubProcessID=t.SubProcessID
	AND (
			(HasInComing!='true' AND HasOutGoing!='true') OR
			(InOutRule = '1' AND HasInComing='false') OR
			(InOutRule = '2' AND HasOutGoing='false')
		)
)NotYetCount
OUTER APPLY(
	SELECT Value=COUNT(Orderid)
	FROM #tmpBundleNo_Complete2 
	WHERE Orderid=t.Orderid 
	AND SubProcessID=t.SubProcessID
	AND InOutRule=t.InOutRule
	AND SubProcessID=t.SubProcessID
	AND ( (HasInComing='true' AND HasOutGoing!='true') OR (HasInComing!='true' AND HasOutGoing='true'))
	AND InOutRule NOT IN ('1','2')
)OnGoingCount

select
	t.Orderid,
	t.SubProcessID,
	Status=case when CompleteCount > 0 AND OnGoingCount=0 AND NotYetCount=0 then 'Complete'
				when NotYetCount > 0 AND OnGoingCount = 0 AND CompleteCount=0 then 'Not Yet Load'
				ELSE 'OnGoing'
				end
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
						)  and isnull(bi.DefectQty,0) <> isnull(bi.ReplacementQty,0)

declare @sql nvarchar(max)=N'
select Orderid,'+@Col+N'
into #right
from #tmp
PIVOT(min(Status) for SubProcessID in('+@AllSubprocess+N'))as pt

select 
	o.FactoryID,OrderID=o.ID,o.POID,o.CustPONo,o.ProgramID,o.StyleID,o.BrandID,o.SeasonID,o.CutCellid,o.SewLine,o.InLineDate,o.OffLineDate,
	'+@Col+N'
    ,BundleReplacement=RQ
from #tmpOrders o
inner join #right r on r.Orderid=o.id
outer apply(
    select RQ=stuff((
	     select concat('','',bi.ReasonID,''('',sum(isnull(bi.DefectQty,0)-isnull(bi.ReplacementQty,0)),'')'')
	    from #tmpBundleInspection bi
		inner join Bundle_Detail bd WITH (NOLOCK) on bd.BundleNo = bi.BundleNo
        inner join Bundle b with(nolock) on b.id = bd.id
		where  o.id = b.Orderid and  o.MDivisionID = b.MDivisionID
	    group by bi.ReasonID
	    for xml path('''')
    ),1,1,'''')
)BR
'
exec(@sql)

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
	OffLineDate=s.Offline
into #tmpOrders
from orders o with(nolock)
left join Order_Qty oq with(nolock) on oq.ID = o.ID
outer apply(
	select CutCellid=STUFF((
		select concat(',',w1.CutCellid)
		from(
			select distinct wo.CutCellid
	        from WorkOrder_Distribute wd with(nolock) 
	        inner join WorkOrder wo with(nolock) on wo.Ukey = wd.WorkOrderUkey
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

select b.Orderid,b.Article,b.Sizecode,bd.BundleNo,s.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
into #tmpBundleNo
from Bundle b with(nolock)
inner join #tmpOrders o on b.Orderid = o.ID and  b.MDivisionID = o.MDivisionID and b.Article = o.Article and b.Sizecode = o.SizeCode
inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
	from SubProcess s
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1
)s


select Orderid,Article,Sizecode,BundleNo,SubProcessID,ShowSeq,InOutRule,IsRFIDDefault,NoBundleCardAfterSubprocess=0,PostSewingSubProcess=0
into #tmpBundleNo_SubProcess
from #tmpBundleNo

union

select Orderid,Article,Sizecode,bda.BundleNo,bda.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,bda.NoBundleCardAfterSubprocess,bda.PostSewingSubProcess
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
	b.InOutRule 
	,[HasInComing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then 'true'
						else IIF( bio.InComing IS NOT NULL ,'true','false')
						end
	,[HasOutGoing]=case when p.PostSewingSubProcess_SL=1 then 'true'
						when NoBundleCardAfterSubprocess=1 and InOutRule = 3  Then 'true'
						else IIF( bio.OutGoing IS NOT NULL ,'true','false')
						end
into #tmpBundleNo_Complete2
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
left join BundleInOut bunIOS with (nolock) on bunIOS.BundleNo = b.BundleNo and bunIOS.SubProcessId = 'SORTING' and isnull(bunIOS.RFIDProcessLocationID,'') = ''
left join BundleInOut bunIOL with (nolock) on bunIOL.BundleNo = b.BundleNo and bunIOL.SubProcessId = 'LOADING' and isnull(bunIOL.RFIDProcessLocationID,'') = ''
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
			(HasInComing='true' AND HasOutGoing='true') OR
			(InOutRule = '1' AND HasInComing='true') OR
			(InOutRule = '2' AND HasOutGoing='true')
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
	AND (
			(HasInComing!='true' AND HasOutGoing!='true') OR
			(InOutRule = '1' AND HasInComing='false') OR
			(InOutRule = '2' AND HasOutGoing='false')
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
	AND ( (HasInComing='true' AND HasOutGoing!='true') OR (HasInComing!='true' AND HasOutGoing='true'))
	AND InOutRule NOT IN ('1','2')
)OnGoingCount

select
	t.Orderid,
	t.Article,
	t.Sizecode,
	t.SubProcessID,
	Status=case when CompleteCount > 0 AND OnGoingCount=0 AND NotYetCount=0 then 'Complete'
				when NotYetCount > 0 AND OnGoingCount = 0 AND CompleteCount=0 then 'Not Yet Load'
				ELSE 'OnGoing'
				end
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
						)  and isnull(bi.DefectQty,0) <> isnull(bi.ReplacementQty,0)

declare @sql nvarchar(max)=N'
select Orderid,Article,Sizecode,'+@Col+N'
into #right
from #tmp
PIVOT(min(Status) for SubProcessID in('+@AllSubprocess+N'))as pt

select 
	o.FactoryID,OrderID=o.ID,o.POID,o.CustPONo,o.ProgramID,o.StyleID,o.BrandID,o.SeasonID,o.Article,o.Sizecode,o.CutCellid,o.SewLine,o.InLineDate,o.OffLineDate,
	'+@Col+N'
    ,BundleReplacement=RQ
from #tmpOrders o
inner join #right r on r.Orderid=o.id and r.Article = o.Article and r.Sizecode = o.Sizecode
outer apply(
    select RQ=stuff((
	    select concat('','',ReasonID,''('',sum(isnull(bi.DefectQty,0)-isnull(bi.ReplacementQty,0)),'')'')
	    from #tmpBundleInspection bi  with(nolock)
        inner join Bundle_Detail bd WITH (NOLOCK) on bd.BundleNo = bi.BundleNo
        inner join Bundle b with(nolock) on b.id = bd.id
	    where b.Orderid = o.ID and  b.MDivisionID = o.MDivisionID
		and b.Article = o.Article and bd.SizeCode = o.Sizecode
	    group by ReasonID
	    for xml path('''')
    ),1,1,'''')
)BR
'
exec(@sql)

drop table #tmpOrders,#tmpBundleNo,#tmpBundleNo_SubProcess,#tmpBundleNo_Complete,#tmp ,#tmpBundleNo_Complete2, #tmpBundleInspection
";
            }
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                ShowErr(result);
                return;
            }
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }
            else
            {
                this.listControlBindingSource1.DataSource = dt;
                this.GridSetup(summaryType);
            }
        }

        private void ShowBundleQtyStatus(DataRow drSelected, string subProcess, int summaryType)
        {
            string sqlcmd = string.Empty;
            string caption = string.Empty;

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
	OffLineDate=o.SewOffLine
into #tmpOrders
from orders o with(nolock)
outer apply(
	select CutCellid=STUFF((
		select concat(',',w1.CutCellid)
		from(
			select distinct wo.CutCellid
	        from WorkOrder_Distribute wd with(nolock) 
	        inner join WorkOrder wo with(nolock) on wo.Ukey = wd.WorkOrderUkey
			where wd.OrderID = o.ID and isnull(wo.CutCellid,'')<>''
		)w1
		for xml path(''))
	,1,1,'')
)w
where o.ID ='{drSelected["OrderID"]}'

select   b.Orderid
        ,bd.BundleNo
        ,s.SubProcessID
        ,s.ShowSeq
        ,s.InOutRule
        ,s.IsRFIDDefault
        ,b.IsEXCESS
        ,bd.PatternDesc
        ,b.Article
        ,bd.BundleGroup
        ,bd.SizeCode
		,b.CutRef
into #tmpBundleNo
from Bundle b with(nolock)
inner join #tmpOrders o on b.Orderid = o.ID and  b.MDivisionID = o.MDivisionID
inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
	from SubProcess s
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1
)s

select   Orderid
        ,BundleNo
        ,SubProcessID
        ,ShowSeq
        ,InOutRule
        ,IsRFIDDefault
        ,IsEXCESS
        ,PatternDesc
        ,Article
        ,BundleGroup
        ,SizeCode
		,CutRef
into #tmpBundleNo_SubProcess
from #tmpBundleNo

union

select   Orderid
        ,bda.BundleNo
        ,bda.SubProcessID
        ,s.ShowSeq
        ,s.InOutRule
        ,s.IsRFIDDefault
        ,IsEXCESS
        ,PatternDesc
        ,Article
        ,BundleGroup
        ,SizeCode
		,CutRef
from #tmpBundleNo b
inner join Bundle_Detail_art bda WITH (NOLOCK) on bda.bundleno = b.bundleno
inner join SubProcess s WITH (NOLOCK) on s.ID = bda.SubprocessId and s.IsRFIDProcess =1

select
	b.Orderid,
	b.BundleNo,
	b.subProcessid,

	IsEXCESS
    ,PatternDesc
    ,Article
    ,BundleGroup
    ,SizeCode
	,b.InOutRule 
	,[HasInComing]=IIF( bio.InComing IS NOT NULL ,'true','false')
	,[HasOutGoing]=IIF( bio.OutGoing IS NOT NULL ,'true','false')
	,bio.InComing
	,bio.OutGoing
	,b.CutRef
into #tmpBundleNo_Complete
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
where b.subProcessid='{subProcess}'

select
	 CutRef
    ,[Bundle#]=t.BundleNo
	,b.Qty
	,EXCESS=iif(IsEXCESS=1,'Y','')
    ,PatternDesc
    ,Article
    ,BundleGroup
    ,SizeCode
	,Status=case when (HasInComing='true' AND HasOutGoing='true') OR (InOutRule = '1' AND HasInComing='true' ) OR (InOutRule = '2' AND HasOutGoing='true' )then 'Complete'		
			  	 when (HasInComing='false' AND HasOutGoing='false') OR (InOutRule = '1' AND HasInComing='false' ) OR (InOutRule = '2' AND HasOutGoing='false' )then 'Not Yet Load'
			 	 when InOutRule='3' AND HasInComing='true' AND HasOutGoing='false' then 'OnGoing'				
				 when InOutRule='4' AND HasInComing='false' AND HasOutGoing='true' then 'OnGoing'				
				 ELSE 'Not Valid'
			 end
	,[InComing] = FORMAT(t.InComing,'yyyy/MM/dd HH:mm:ss')
	,[OutGoing] = FORMAT(t.OutGoing,'yyyy/MM/dd HH:mm:ss')
    ,ps.PostSewingSubProcess
    ,nb.NoBundleCardAfterSubprocess
from #tmpBundleNo_Complete t
outer apply(
	select qty=sum(bd.Qty)
	from Bundle_Detail bd with(nolock)
	where bd.BundleNo = t.BundleNo
)b
outer apply(
    select top 1 PostSewingSubProcess
    from Bundle_Detail_Art bda
    where bda.BundleNo = t.BundleNo
    and bda.subProcessid='{subProcess}'
    and bda.PostSewingSubProcess = 1
)ps
outer apply(
    select top 1 NoBundleCardAfterSubprocess
    from Bundle_Detail_Art bda
    where bda.BundleNo = t.BundleNo
    and bda.subProcessid='{subProcess}'
    and bda.NoBundleCardAfterSubprocess = 1
)nb

order by t.BundleNo

drop table #tmpOrders,#tmpBundleNo,#tmpBundleNo_SubProcess,#tmpBundleNo_Complete
";
                caption = $"SubProcess:{subProcess}";
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
	OffLineDate=s.Offline
into #tmpOrders
from orders o with(nolock)
inner join Order_Qty oq with(nolock) on oq.ID = o.ID
outer apply(
	select CutCellid=STUFF((
		select concat(',',w1.CutCellid)
		from(
			select distinct wo.CutCellid
	        from WorkOrder_Distribute wd with(nolock) 
	        inner join WorkOrder wo with(nolock) on wo.Ukey = wd.WorkOrderUkey
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
where o.ID ='{drSelected["OrderID"]}' and oq.Article='{drSelected["Article"]}' and oq.SizeCode='{drSelected["SizeCode"]}'


select   b.Orderid
        ,b.Article
        ,b.Sizecode
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
into #tmpBundleNo
from Bundle b with(nolock)
inner join #tmpOrders o on b.Orderid = o.ID and  b.MDivisionID = o.MDivisionID and b.Article = o.Article and b.Sizecode = o.SizeCode
inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
cross join(
	select SubProcessID=id,s.ShowSeq,s.InOutRule,s.IsRFIDDefault
	from SubProcess s
	where s.IsRFIDProcess=1 and s.IsRFIDDefault=1
)s


select Orderid,Article,Sizecode,BundleNo,SubProcessID,ShowSeq,InOutRule,IsRFIDDefault,IsEXCESS
    ,PatternDesc
    ,BundleGroup
    ,[BD_SizeCode]
    ,CutRef
into #tmpBundleNo_SubProcess
from #tmpBundleNo

union

select Orderid,Article,Sizecode,bda.BundleNo,bda.SubProcessID,s.ShowSeq,s.InOutRule,s.IsRFIDDefault,IsEXCESS
    ,PatternDesc
    ,BundleGroup
    ,[BD_SizeCode]
    ,CutRef
from #tmpBundleNo b
inner join Bundle_Detail_art bda WITH (NOLOCK) on bda.bundleno = b.bundleno
inner join SubProcess s WITH (NOLOCK) on s.ID = bda.SubprocessId and s.IsRFIDProcess =1

select
	b.Orderid,
	b.Article,
	b.Sizecode,
	b.BundleNo,
	b.subProcessid,
	IsEXCESS
    ,PatternDesc
    ,BundleGroup
    ,[BD_SizeCode]
	,b.InOutRule 
	,[HasInComing]=IIF( bio.InComing IS NOT NULL ,'true','false')
	,[HasOutGoing]=IIF( bio.OutGoing IS NOT NULL ,'true','false')
	,bio.InComing
	,bio.OutGoing
	,b.CutRef
into #tmpBundleNo_Complete
from #tmpBundleNo_SubProcess b
left join BundleInOut bio with (nolock) on bio.BundleNo = b.BundleNo and bio.SubProcessId = b.SubProcessID and isnull(bio.RFIDProcessLocationID,'') = ''
where b.subProcessid='{subProcess}'

select
	 CutRef
    ,[Bundle#]=t.BundleNo
	,b.Qty
	,EXCESS=iif(IsEXCESS=1,'Y','')
    ,PatternDesc
    ,Article
    ,BundleGroup
    ,SizeCode
	,Status=case when (HasInComing='true' AND HasOutGoing='true') OR (InOutRule = '1' AND HasInComing='true' ) OR (InOutRule = '2' AND HasOutGoing='true' )then 'Complete'		
			  	 when (HasInComing='false' AND HasOutGoing='false') OR (InOutRule = '1' AND HasInComing='false' ) OR (InOutRule = '2' AND HasOutGoing='false' )then 'Not Yet Load'
			 	 when InOutRule='3' AND HasInComing='true' AND HasOutGoing='false' then 'OnGoing'				
				 when InOutRule='4' AND HasInComing='false' AND HasOutGoing='true' then 'OnGoing'				
				 ELSE 'Not Valid'
			 end
	,[InComing] = FORMAT(t.InComing,'yyyy/MM/dd HH:mm:ss')
	,[OutGoing] = FORMAT(t.OutGoing,'yyyy/MM/dd HH:mm:ss')
    ,ps.PostSewingSubProcess
    ,nb.NoBundleCardAfterSubprocess
from #tmpBundleNo_Complete t
outer apply(
	select qty=sum(bd.Qty)
	from Bundle_Detail bd with(nolock)
	inner join Bundle b WITH (NOLOCK) on b.id = bd.Id
	where bd.BundleNo = t.BundleNo and b.Article = t.Article and b.Sizecode = t.Sizecode
)b
outer apply(
    select top 1 PostSewingSubProcess
    from Bundle_Detail_Art bda
    where bda.BundleNo = t.BundleNo
    and bda.subProcessid='{subProcess}'
    and bda.PostSewingSubProcess = 1
)ps
outer apply(
    select top 1 NoBundleCardAfterSubprocess
    from Bundle_Detail_Art bda
    where bda.BundleNo = t.BundleNo
    and bda.subProcessid='{subProcess}'
    and bda.NoBundleCardAfterSubprocess = 1
)nb

drop table #tmpOrders,#tmpBundleNo,#tmpBundleNo_SubProcess,#tmpBundleNo_Complete
";
                caption = $"SubProcess:{subProcess} - Article:{drSelected["Article"]} - Size:{drSelected["SizeCode"]}";
            }

            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                ShowErr(result);
                return;
            }

            #region 準備要傳入元件的Grid

            this.listControlBindingSource2.DataSource = dt;
            this.grid2.Columns.Clear();
            //準備Grid 2
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Bundle#", header: "BundleNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("PatternDesc", header: "PatternDesc", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("BundleGroup", header: "BundleGroup", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("EXCESS", header: "EXCESS", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("OutGoing", header: "Farm Out", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("InComing", header: "Farm In", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Status", header: "Status", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;

            string sqlchk = $@"select 1 from SubProcess where id = '{subProcess}' and IsSelection = 1";
            if (MyUtility.Check.Seek(sqlchk))
            {
                this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("PostSewingSubProcess", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: false)
                .CheckBox("NoBundleCardAfterSubprocess", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: false)
                ;
            }
            this.grid2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid2_CellFormatting);

            #endregion


            MyUtility.Msg.ShowMsgGrid_LockScreen(this.grid2, caption: caption,eventname:new string[] { "CellFormatting" });
        }
        
        private void grid2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {


            switch (MyUtility.Convert.GetString(e.Value))
            {
                case "OnGoing":
                    e.CellStyle.BackColor = Color.Yellow;
                    break;
                case "Not Valid":
                    e.CellStyle.BackColor = Color.Red;
                    break;
                default:
                    break;
            }

            
        }
        
    }
}
