﻿using Ict;
using Ict.Win;
using Microsoft.Office.Interop.Word;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static Sci.Production.PublicPrg.Prgs;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P12 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GridSetup();
            this.EditMode = true;
            this.comboSortBy.SelectedIndex = 0;
        }

        private string Cut_Ref;
        private string Cut_Ref1;
        private string SP;
        private string SP1;
        private string POID;
        private string Bundle;
        private string Bundle1;
        private DateTime? Est_CutDate;
        private string Cell;
        private string size;
        private DualResult result;
        private System.Data.DataTable dtt;
        private string Addname;
        private DateTime? AddDate;
        private string Cutno;
        private string SpreadingNoID;
        private int strPagetype;

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("selected", header: "Sel", width: Widths.AnsiChars(4), iseditable: true, trueValue: true, falseValue: false)
                .DateTime("PrintDate", header: "Print Date\r\n(Bundle Card)", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .DateTime("RFPrintDate", header: "Print Date\r\n(RF Card)", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Date("CreateDate", header: "Create Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Bundle", header: "Bundle#", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("POID", header: "POID", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("SP", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("Group", header: "Group", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Line", header: "Line", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Cell", header: "Cell", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Style", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Item", header: "Item", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Comb", header: "Comb", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Cut", header: "Cut#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Size", header: "Size", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SizeSpec", header: "SizeSpec", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Cutpart", header: "Cutpart Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SubProcess", header: "Artwork", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Parts", header: "Parts", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 0, integer_places: 10, width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            if (this.txtCutRefStart.Text.Empty() && this.txtCutRefEnd.Text.Empty()
                && this.txtSPNoStart.Text.Empty() && this.txtSPNoEnd.Text.Empty()
                && this.txtPOID.Text.Empty()
                && this.txtBundleStart.Text.Empty() && this.txtBundleEnd.Text.Empty()
                && this.dateBox1.Value.Empty() && this.dateBundlecreatedDate.Value.Empty())
            {
                this.txtCutRefStart.Focus();
                if (this.dtt != null)
                {
                    this.dtt.Clear();
                }

                MyUtility.Msg.WarningBox("[Cut_Ref# and SP# and POID and Bundle# and Est.Cut Date and Bundle created Date ] can not be all null !!");
                return;
            }

            if ((!this.txtCutRefStart.Text.Empty() && this.txtCutRefEnd.Text.Empty()) || (this.txtCutRefStart.Text.Empty() && !this.txtCutRefEnd.Text.Empty())
                || (!this.txtSPNoStart.Text.Empty() && this.txtSPNoEnd.Text.Empty()) || (this.txtSPNoStart.Text.Empty() && !this.txtSPNoEnd.Text.Empty())
                || (!this.txtBundleStart.Text.Empty() && this.txtBundleEnd.Text.Empty()) || (this.txtBundleStart.Text.Empty() && !this.txtBundleEnd.Text.Empty()))
            {
                this.txtCutRefStart.Focus();
                if (this.dtt != null)
                {
                    this.dtt.Clear();
                }

                MyUtility.Msg.WarningBox("[Cut_Ref# and SP# and Bundle#] must enter start and end !!");
                return;
            }

            this.ShowWaitMessage("Data processing, please wait...");

            this.Cut_Ref = this.txtCutRefStart.Text.ToString();
            this.Cut_Ref1 = this.txtCutRefEnd.Text.ToString();
            this.SP = this.txtSPNoStart.Text.ToString();
            this.SP1 = this.txtSPNoEnd.Text.ToString();
            this.POID = this.txtPOID.Text.ToString();
            this.Bundle = this.txtBundleStart.Text.ToString();
            this.Bundle1 = this.txtBundleEnd.Text.ToString();
            this.Est_CutDate = this.dateBox1.Value;
            this.Cell = this.txtCell.Text.ToString();
            this.size = this.txtSize.Text.ToString();
            this.Addname = this.txtuser1.TextBox1.Text;
            this.AddDate = this.dateBundlecreatedDate.Value;
            this.Cutno = this.txtCutno.Text;
            this.SpreadingNoID = this.txtSpreadingNo1.Text;
            string sb = string.Empty;
            string declare = string.Empty;
            List<string> sqlWheres = new List<string>
            {
                "b.MDivisionID=@Keyword",
            };

            if (!this.txtCutRefStart.Text.Empty() && !this.txtCutRefEnd.Text.Empty())
            {
                sqlWheres.Add("b.CutRef between @Cut_Ref and @Cut_Ref1");
            }

            if (!this.txtSPNoStart.Text.Empty() && !this.txtSPNoEnd.Text.Empty())
            {
                sqlWheres.Add(@"exists(select 1 from Bundle_Detail_Order bdo WITH (NOLOCK) where bdo.ID = b.ID and bdo.OrderID between @SP and @SP1)");
            }

            if (!this.txtPOID.Text.Empty())
            {
                sqlWheres.Add("b.POID=@POID");
            }

            if (!this.txtBundleStart.Text.Empty() && !this.txtBundleEnd.Text.Empty())
            {
                sqlWheres.Add("a.BundleNo between @Bundle and @Bundle1");
            }

            if (!this.txtCell.Text.Empty())
            {
                sqlWheres.Add("b.SewingCell =@Cell");
            }

            if (!this.txtSize.Text.Empty())
            {
                sqlWheres.Add("a.SizeCode =@Size");
            }

            if (!this.dateBox1.Value.Empty())
            {
                sqlWheres.Add("WorkOrder.EstCutDate=@Est_CutDate");
                declare += $@" declare @Est_CutDate date = '{((DateTime)this.Est_CutDate).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.Addname))
            {
                sqlWheres.Add(" b.AddName = @AddName");
            }

            if (!this.dateBundlecreatedDate.Value.Empty())
            {
                sqlWheres.Add(" format(b.AddDate,'yyyy/MM/dd') = @AddDate");
                declare += $@" declare @AddDate varchar(10) = '{((DateTime)this.AddDate).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.Cutno))
            {
                sqlWheres.Add(" b.Cutno=@Cutno");
            }

            if (this.comboSortBy.Text == "Bundle#")
            {
                sb = "order by x.Bundle,x.[SP],x.[Comb],x.Article,x.[Size]";
            }
            else if (this.comboSortBy.Text == "SP#")
            {
                sb = "order by x.[SP],x.[CutRef],x.[Comb],x.Article,x.Size";
            }

            if (!this.txtfactoryByM.Text.Empty())
            {
                sqlWheres.Add(" c.FtyGroup  = @FtyGroup ");
            }

            if (!this.txtComb.Text.Empty())
            {
                sqlWheres.Add(" b.PatternPanel  = @Comb ");
            }

            if (!MyUtility.Check.Empty(this.SpreadingNoID))
            {
                sqlWheres.Add(" WorkOrder.SpreadingNoID=@SpreadingNoID");
            }

            string sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            if (!this.checkExtendAllParts.Checked)
            {
                declare += $@" declare @extend bit = 0 ";
            }

            string sqlcmd;
            if (this.checkExtendAllParts.Checked)
            {
                #region 有勾[Extend All Parts]

                DBProxy.Current.DefaultTimeout = 1800;  // 加長時間為30分鐘，避免timeout
                sqlcmd = $@"
declare @Keyword varchar(8) = '{Env.User.Keyword}'
declare @Cut_Ref varchar(6) = '{this.Cut_Ref}'
declare @Cut_Ref1 varchar(6) = '{this.Cut_Ref1}'
declare @SP varchar(13) = '{this.SP}'
declare @SP1 varchar(13) = '{this.SP1}'
declare @POID varchar(13) = '{this.POID}'
declare @Bundle varchar(13) = '{this.Bundle}'
declare @Bundle1 varchar(13) = '{this.Bundle1}'
declare @Cell varchar(3) = '{this.Cell}'
declare @size varchar(8) = '{this.size}'
declare @Addname varchar(10) = '{this.Addname}'
declare @Cutno varchar(6) = '{this.Cutno}'
declare @SpreadingNoID varchar(6) = '{this.SpreadingNoID}'
declare @FtyGroup varchar(8) = '{this.txtfactoryByM.Text}'
declare @Comb varchar(2) = '{this.txtComb.Text}'
{declare}
set arithabort on

SELECT e1.*
,Seq1 = Isnull(sd.Seq, s.Seq)
,Seq2 = IsNull(sd.SubProcessID, s.Id)
INTO #TempSubProcessData 
FROM dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
LEFT JOIN SubProcessSeq_Detail sd WITH (NOLOCK) ON sd.SubProcessID = e1.SubprocessId
LEFT JOIN SubProcess s WITH (NOLOCK) ON s.Id = e1.SubprocessId
where  EXISTS(
	select b.id,a.BundleNo, a.PatternCode
	 from dbo.Bundle_Detail a WITH (NOLOCK)
	inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
    inner join dbo.Orders c WITH (NOLOCK) on c.id = b.OrderID -- 引入 c 表
    OUTER APPLY (
        SELECT TOP 1 
            MarkerNo, EstCutDate, SpreadingNoID
        FROM dbo.WorkOrder WITH (NOLOCK) 
        WHERE CutRef=b.CutRef and ID=b.POID and b.CutRef<>'' and b.CutRef is not null
    ) WorkOrder -- 引入 WorkOrder 資料至 EXISTS 子查詢
    {sqlWhere} 
        and a.Patterncode != 'ALLPARTS' 
        and e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
)
order by Seq1 , Seq2


select distinct x.ID, x.PatternCode,x.PatternDesc,x.Parts
INTO #Bundle_Detail_Allpart
from Bundle_Detail_Allpart x with(nolock)
where  EXISTS(
	select b.id,a.BundleNo, a.PatternCode
	 from dbo.Bundle_Detail a WITH (NOLOCK)
	inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
    inner join dbo.Orders c WITH (NOLOCK) on c.id = b.OrderID -- 引入 c 表
    OUTER APPLY (
        SELECT TOP 1 
            MarkerNo, EstCutDate, SpreadingNoID
        FROM dbo.WorkOrder WITH (NOLOCK) 
        WHERE CutRef=b.CutRef and ID=b.POID and b.CutRef<>'' and b.CutRef is not null
    ) WorkOrder -- 引入 WorkOrder 資料至 EXISTS 子查詢
    {sqlWhere} 
        and a.Patterncode != 'ALLPARTS' 
        and x.id=b.id
)


select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
    , b.SubCutNo
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
    , a.Tone
    , b.Sewinglineid [Line]
    , b.SewingCell [Cell]
    , c.StyleID [Style]
    , b.Item [Item]
    , b.PatternPanel [Comb]
    , b.cutno [Cut]
    , b.Article [Article]
    , b.Colorid [Color]
    , b.Article + '\' + b.Colorid [Color2]
    , a.SizeCode [Size]
    , a.PatternCode [Cutpart]
    , '('+a.Patterncode+')' [Patterncode]
    , a.PatternDesc [Description]
    , [SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
    , a.Parts [Parts]
    , a.Qty [Qty]
    , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
    , c.FactoryID  [left]
    , [MarkerNo]=WorkOrder.MarkerNo
    , SeasonID = concat(c.SeasonID,' ', c.dest)
    , brand=c.brandid
    ,brand.ShipCode
    , b.IsEXCESS
    , WorkOrder.SpreadingNoID
    , ps.NoBundleCardAfterSubprocess_String
    , nbs.PostSewingSubProcess_String
    ,b.FabricPanelCode
	, [BundleID] = b.ID
    , a.RFPrintDate
    , a.RFIDScan
    , CutCell = (select top 1 CutCellID from workorder w where w.cutref = b.cutref)
    , a.Dyelot
    , a.ID
into #tmp
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
inner join dbo.Orders c WITH (NOLOCK) on c.id = bdo.OrderID and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
    select SubProcess = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
)as SubProcess
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.PostSewingSubProcess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as nbs
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
        ,SpreadingNoID
	FROM  dbo.WorkOrder WITH (NOLOCK) WHERE CutRef=b.CutRef and ID=b.POID and b.CutRef<>''  and b.CutRef is not null
)WorkOrder
" + sqlWhere + $@" and a.Patterncode != 'ALLPARTS' 

union all

select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
    , b.SubCutNo
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
    , a.Tone
    , b.Sewinglineid [Line]
    , b.SewingCell [Cell]
    , c.StyleID [Style]
    , b.Item [Item]
    , b.PatternPanel [Comb]
    , b.cutno [Cut]
    , b.Article [Article]
    , b.Colorid [Color]
    , b.Article + '\' + b.Colorid [Color2]
    , a.SizeCode [Size]
    , bda.PatternCode [Cutpart]
    , '('+bda.Patterncode+')' [Patterncode]
    , bda.PatternDesc [Description]
    , [SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
    , bda.Parts [Parts]
    , a.Qty [Qty]
    , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
    , c.FactoryID  [left]
    , [MarkerNo]=WorkOrder.MarkerNo
    , SeasonID = concat(c.SeasonID,' ', c.dest)
    , brand=c.brandid
    , brand.ShipCode
    , b.IsEXCESS
    , WorkOrder.SpreadingNoID
    , ps.NoBundleCardAfterSubprocess_String
    , nbs.PostSewingSubProcess_String
    ,b.FabricPanelCode
	, [BundleID] = b.ID
    , a.RFPrintDate
    , a.RFIDScan
    , CutCell = (select top 1 CutCellID from workorder w where w.cutref = b.cutref)
    , a.Dyelot
    , a.ID
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
left join #Bundle_Detail_Allpart bda on bda.ID = b.ID
outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
inner join dbo.Orders c WITH (NOLOCK) on c.id = bdo.OrderID and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
    select SubProcess = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= bda.PatternCode
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
)as SubProcess
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= bda.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= bda.PatternCode and e1.PostSewingSubProcess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as nbs
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
        ,SpreadingNoID
	FROM  dbo.WorkOrder WITH (NOLOCK) WHERE CutRef=b.CutRef and ID=b.POID and b.CutRef<>''  and b.CutRef is not null
)WorkOrder
" + sqlWhere + @" and a.Patterncode = 'ALLPARTS' 
OPTION (RECOMPILE)

select  *
from #tmp x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m WITH (NOLOCK)
		inner join Production.dbo.MNOrder_SizeItem msi WITH (NOLOCK) on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc WITH (NOLOCK) on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss WITH (NOLOCK) on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso WITH (NOLOCK) on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu
" + sb + @"
OPTION (RECOMPILE)"
;
                #endregion
            }
            else
            {
                #region 沒勾[Extend All Parts]
                sqlcmd = $@"
declare @Keyword varchar(8) = '{Env.User.Keyword}'
declare @Cut_Ref varchar(6) = '{this.Cut_Ref}'
declare @Cut_Ref1 varchar(6) = '{this.Cut_Ref1}'
declare @SP varchar(13) = '{this.SP}'
declare @SP1 varchar(13) = '{this.SP1}'
declare @POID varchar(13) = '{this.POID}'
declare @Bundle varchar(13) = '{this.Bundle}'
declare @Bundle1 varchar(13) = '{this.Bundle1}'
declare @Cell varchar(3) = '{this.Cell}'
declare @size varchar(8) = '{this.size}'
declare @Addname varchar(10) = '{this.Addname}'
declare @Cutno varchar(6) = '{this.Cutno}'
declare @SpreadingNoID varchar(6) = '{this.SpreadingNoID}'
declare @FtyGroup varchar(8) = '{this.txtfactoryByM.Text}'
declare @Comb varchar(2) = '{this.txtComb.Text}'
{declare}
set arithabort on

SELECT e1.*
,Seq1 = Isnull(sd.Seq, s.Seq)
,Seq2 = IsNull(sd.SubProcessID, s.Id)
INTO #TempSubProcessData 
FROM dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
LEFT JOIN SubProcessSeq_Detail sd WITH (NOLOCK) ON sd.SubProcessID = e1.SubprocessId
LEFT JOIN SubProcess s WITH (NOLOCK) ON s.Id = e1.SubprocessId
where  EXISTS(
	select b.id,a.BundleNo, a.PatternCode
	 from dbo.Bundle_Detail a WITH (NOLOCK)
	inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
    inner join dbo.Orders c WITH (NOLOCK) on c.id = b.OrderID -- 引入 c 表
    OUTER APPLY (
        SELECT TOP 1 
            MarkerNo, EstCutDate, SpreadingNoID
        FROM dbo.WorkOrder WITH (NOLOCK) 
        WHERE CutRef=b.CutRef and ID=b.POID and b.CutRef<>'' and b.CutRef is not null
    ) WorkOrder -- 引入 WorkOrder 資料至 EXISTS 子查詢
    {sqlWhere} 
        and a.Patterncode != 'ALLPARTS' 
        and e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
)
order by Seq1 , Seq2

select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
    , b.SubCutNo
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
    , a.Tone
    , b.Sewinglineid [Line]
    , b.SewingCell [Cell]
    , c.StyleID [Style]
    , b.Item [Item]
    , b.PatternPanel [Comb]
    , b.cutno [Cut]
    , b.Article [Article]
    , b.Colorid [Color]
    , b.Article + '\' + b.Colorid [Color2]
    , a.SizeCode [Size]
    , a.PatternCode [Cutpart]
    , '('+a.Patterncode+')' [Patterncode]
    , a.PatternDesc [Description]
    , [SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
    , a.Parts [Parts]
    , a.Qty [Qty]
    , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
    , c.FactoryID  [left]
    , [MarkerNo]=WorkOrder.MarkerNo
    , SeasonID = concat(c.SeasonID,' ', c.dest)
    , brand=c.brandid
    , brand.ShipCode
    , b.IsEXCESS
    , WorkOrder.SpreadingNoID
    , ps.NoBundleCardAfterSubprocess_String
    , nbs.PostSewingSubProcess_String
    ,b.FabricPanelCode
	, [BundleID] = b.ID
    , a.RFPrintDate
    , a.RFIDScan
    , CutCell = (select top 1 CutCellID from workorder w where w.cutref = b.cutref)
    , a.Dyelot
    , a.ID
into #tmp
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
inner join dbo.Orders c WITH (NOLOCK) on c.id = bdo.Orderid and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
    select SubProcess = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
)as SubProcess 
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.PostSewingSubProcess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as nbs
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
        ,SpreadingNoID
	FROM  dbo.WorkOrder WITH (NOLOCK) WHERE CutRef=b.CutRef and ID=b.POID and b.CutRef<>''  and b.CutRef is not null
)WorkOrder
" + sqlWhere + $@" and a.Patterncode != 'ALLPARTS' 
                                        
union all

select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
    , b.SubCutNo
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
    , a.Tone
    , b.Sewinglineid [Line]
    , b.SewingCell [Cell]
    , c.StyleID [Style]
    , b.Item [Item]
    , b.PatternPanel [Comb]
    , b.cutno [Cut]
    , b.Article [Article]
    , b.Colorid [Color]
    , b.Article + '\' + b.Colorid [Color2]
    , a.SizeCode [Size]
    , a.PatternCode [Cutpart]
    , '('+a.Patterncode+')' [Patterncode]
    , a.PatternDesc [Description]
    , [SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
    , a.Parts [Parts]
    , a.Qty [Qty]
    , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
    , c.FactoryID  [left]
    , [MarkerNo]=WorkOrder.MarkerNo
    , SeasonID = concat(c.SeasonID,' ', c.dest)
    , brand=c.brandid
    , brand.ShipCode
    , b.IsEXCESS
    , WorkOrder.SpreadingNoID
    , ps.NoBundleCardAfterSubprocess_String
    , nbs.PostSewingSubProcess_String
    ,b.FabricPanelCode
	, [BundleID] = b.ID
    , a.RFPrintDate
    , a.RFIDScan
    , CutCell = (select top 1 CutCellID from workorder w where w.cutref = b.cutref)
    , a.Dyelot
    , a.ID
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
inner join dbo.Orders c WITH (NOLOCK) on c.id = bdo.OrderID and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
    select SubProcess = 
    stuff((
        select concat('+',iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', '')))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
)as SubProcess 
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
        from #TempSubProcessData e1
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.PostSewingSubProcess = 1
		order by Seq1 , Seq2
        for xml path('')
    ),1,1,'')
) as nbs
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
        ,SpreadingNoID
	FROM  dbo.WorkOrder WITH (NOLOCK) WHERE CutRef=b.CutRef and ID=b.POID and b.CutRef<>''  and b.CutRef is not null
)WorkOrder
" + sqlWhere + @" 
and a.Patterncode = 'ALLPARTS' 
OPTION (RECOMPILE)

select *
from #tmp x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m WITH (NOLOCK)
		inner join Production.dbo.MNOrder_SizeItem msi WITH (NOLOCK) on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc WITH (NOLOCK) on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss WITH (NOLOCK) on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso WITH (NOLOCK) on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu
" + sb + @"
OPTION (RECOMPILE)"
;
                #endregion
            }

            DBProxy.Current.DefaultTimeout = 1800;  // 加長時間為30分鐘，避免timeout
            this.result = DBProxy.Current.Select(string.Empty, sqlcmd, out this.dtt);
            if (!this.result)
            {
                this.ShowErr(this.result);
                this.HideWaitMessage();
                return;
            }

            if (this.dtt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            this.listControlBindingSource1.DataSource = this.dtt;
            DBProxy.Current.DefaultTimeout = 300;  // 恢復時間為5分鐘
            this.HideWaitMessage();
        }

        private void BtnBundleCard_Click(object sender, EventArgs e)
        {
            this.PrintBarcode();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            var r = this.dtt.AsEnumerable().Where(row => (bool)row["selected"]).ToList();
            if (r.Count == 0)
            {
                this.grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }

            System.Data.DataTable selects = r.CopyToDataTable();

            string fileName = "Cutting_P12";
            string fieldList = "Bundle,CutRef,POID,SP,Group,Dyelot,Line,SpreadingNoID,Cell,Style,Item,Comb,Cut,Article,Color,Size,SizeSpec,Cutpart,Description,SubProcess,Parts,Qty";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(selects, string.Empty, $"{fileName}.xltx", 1, true, fieldList, excelApp);
            Marshal.ReleaseComObject(excelApp);
            return;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintBarcode()
        {
            this.grid1.ValidateControl();
            if (this.dtt == null)
            {
                return;
            }

            // DefaultView 是依據使用者排序列印
            System.Data.DataTable dtSelect = this.dtt.DefaultView.ToTable().AsEnumerable().Where(row => (bool)row["selected"]).TryCopyToDataTable(this.dtt);
            if (dtSelect.Rows.Count == 0)
            {
                this.grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }

            this.ShowWaitMessage("Process Excel!");
            List<P10_PrintData> data = dtSelect.AsEnumerable().Select(row1 => new P10_PrintData()
            {
                Group_right = row1["Group"].ToString(),
                Group_left = row1["left"].ToString(),
                CutRef = row1["CutRef"].ToString(),
                Tone = MyUtility.Convert.GetString(row1["Tone"]),
                Line = row1["Line"].ToString(),
                Cell = row1["Cell"].ToString(),
                POID = row1["POID"].ToString(),
                SP = row1["SP"].ToString(),
                Style = row1["Style"].ToString(),
                MarkerNo = row1["MarkerNo"].ToString(),
                Body_Cut = row1["Body_Cut"].ToString() + (MyUtility.Check.Empty(row1["SubCutNo"]) ? string.Empty : $"-{row1["SubCutNo"]}"),
                Parts = row1["Parts"].ToString(),
                Color = row1["Color2"].ToString(),
                Article = row1["Article"].ToString(),
                Size = row1["Size"].ToString(),
                SizeSpec = row1["SizeSpec"].ToString(),
                Desc = row1["Patterncode"].ToString() + row1["Description"].ToString(),
                Artwork = row1["SubProcess"].ToString(),
                Quantity = row1["Qty"].ToString(),
                Barcode = row1["Bundle"].ToString(),
                Season = row1["Seasonid"].ToString(),
                Brand = row1["brand"].ToString(),
                Item = row1["item"].ToString(),
                EXCESS1 = MyUtility.Convert.GetBool(row1["IsEXCESS"]) ? "EXCESS" : string.Empty,
                NoBundleCardAfterSubprocess1 = MyUtility.Check.Empty(row1["NoBundleCardAfterSubprocess_String"]) ? string.Empty : "X",
                Replacement1 = string.Empty,
                ShipCode = MyUtility.Convert.GetString(row1["ShipCode"]),
                FabricPanelCode = MyUtility.Convert.GetString(row1["FabricPanelCode"]),
                Comb = MyUtility.Convert.GetString(row1["Comb"]),
                Cut = MyUtility.Convert.GetString(row1["cut"]),
                GroupCombCut = 0,
                BundleID = row1["BundleID"].ToString(),
                CutCell = row1["CutCell"].ToString(),
                Dyelot = row1["Dyelot"].ToString(),
                BundleNo = row1["Bundle"].ToString(),
            }).ToList();
            string fileName = "Cutting_P10_Layout1";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx");
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            this.strPagetype = 9;

            if (this.checkChangepagebyCut.Checked)
            {
                var x = data.GroupBy(g => new { g.Comb, g.Cut })
                     .Where(@group => @group.Any())
                     .OrderBy(@group => @group.Key.Comb ?? string.Empty)
                     .ThenBy(@group => @group.Key.Cut ?? string.Empty)
                     .AsEnumerable()
                     .Select((@group, i) => new
                     {
                         Items = @group,
                         Rank = ++i,
                     })
                     .SelectMany(v => v.Items, (s, i) => new
                     {
                         Item = i,
                         DenseRank = s.Rank,
                     }).ToList();

                int page = x.GroupBy(g => g.DenseRank).Select(s => ((s.Count() - 1) / this.strPagetype) + 1).Sum();
                for (int i = 1; i < page; i++)
                {
                    Excel.Worksheet worksheet1 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[1];
                    Excel.Worksheet worksheetn = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[i + 1];
                    worksheet1.Copy(worksheetn);
                    Marshal.ReleaseComObject(worksheet1);
                    Marshal.ReleaseComObject(worksheetn);
                }

                // 先批次取得 BundleNo, No 資料, by POID, FabricPanelCode, Article, Size
                System.Data.DataTable allNoDatas = null;
                data.Select(s => new { s.POID, s.FabricPanelCode, s.Article, s.Size }).Distinct().ToList().ForEach(r =>
                {
                    if (allNoDatas == null)
                    {
                        allNoDatas = GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size);
                    }
                    else
                    {
                        allNoDatas.Merge(GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size));
                    }
                });

                int pp = 1;
                x.Select(s => s.DenseRank).Distinct().OrderBy(o => o).ToList().ForEach(denseRank =>
                {
                    data.Clear();
                    x.Where(w => w.DenseRank == denseRank).ToList().ForEach(r =>
                    {
                        r.Item.GroupCombCut = r.DenseRank;
                        data.Add(r.Item);
                    });

                    for (int s = 1; s <= ((data.Count - 1) / this.strPagetype) + 1; s++)
                    {
                        var writedata = data.Skip((s - 1) * this.strPagetype).Take(this.strPagetype).ToList();
                        Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[pp];
                        P10_Print.ProcessPrint(writedata, worksheet, allNoDatas, this.strPagetype);
                        pp++;
                    }
                });
            }
            else
            {
                P10_Print.RunPagePrint(data, excelApp, this.strPagetype);
            }

            // 有按才更新列印日期printdate。
            StringBuilder ups = new StringBuilder();
            foreach (DataRow dr in dtSelect.Rows)
            {
                ups.Append($@"
update bd
set bd.PrintDate = GETDATE()
from Bundle_Detail bd
where bd.BundleNo = '{dr["Bundle"]}'

update b
set b.PrintDate = GETDATE()
from Bundle b
inner join Bundle_Detail bd on b.id=bd.ID
where bd.BundleNo = '{dr["Bundle"]}'
");
            }

            this.HideWaitMessage();
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == DialogResult.OK)
            {
                string printer = pd.PrinterSettings.PrinterName;
                workbook.PrintOutEx(ActivePrinter: printer);

                DualResult result = DBProxy.Current.Execute(null, ups.ToString());
                if (!result)
                {
                    this.ShowErr("Update PrintDate Error!", result);
                    return;
                }

                int pos = this.listControlBindingSource1.Position;
                this.Query();
                if (this.listControlBindingSource1.Count >= pos + 1)
                {
                    this.listControlBindingSource1.Position = pos;
                }
            }

            string excelName = Class.MicrosoftFile.GetName(fileName);
            excelApp.ActiveWorkbook.SaveAs(excelName);
            workbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
            File.Delete(excelName);
        }

        private void P12_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void BtnBundleCardRF_Click(object sender, EventArgs e)
        {
            if (this.dtt == null || this.dtt.Rows.Count == 0)
            {
                this.grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }

            var querydr = this.dtt.AsEnumerable().Where(row => (bool)row["selected"]);
            if (!querydr.Any() || querydr.ToList().Count() == 0)
            {
                this.grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }

            System.Data.DataTable dtSelect = querydr.ToList().CopyToDataTable();
            System.Data.DataTable allNoDatas = null;
            dtSelect.AsEnumerable().Select(dr => new P10_PrintData()
            {
                POID = dr["POID"].ToString(),
                FabricPanelCode = dr["FabricPanelCode"].ToString(),
                Article = dr["Article"].ToString(),
                Size = dr["Size"].ToString(),
            })
            .Select(s => new { s.POID, s.FabricPanelCode, s.Article, s.Size }).Distinct().ToList().ForEach(r =>
            {
                if (allNoDatas == null)
                {
                    allNoDatas = GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size);
                }
                else
                {
                    allNoDatas.Merge(GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size));
                }
            });

            List<P10_PrintData> data = dtSelect.AsEnumerable().Select(dr => new P10_PrintData()
            {
                Group_right = dr["Group"].ToString(),
                Group_left = dr["left"].ToString(),
                CutRef = dr["CutRef"].ToString(),
                Tone = dr["Tone"].ToString(),
                Line = dr["Line"].ToString(),
                Cell = dr["Cell"].ToString(),
                POID = dr["POID"].ToString(),
                SP = dr["SP"].ToString(),
                Style = dr["Style"].ToString(),
                MarkerNo = dr["MarkerNo"].ToString(),
                Body_Cut = dr["Body_Cut"].ToString() + (MyUtility.Check.Empty(dr["SubCutNo"]) ? string.Empty : $"-{dr["SubCutNo"]}"),
                Parts = dr["Parts"].ToString(),
                Color = dr["Color2"].ToString(),
                Article = dr["Article"].ToString(),
                Size = dr["Size"].ToString(),
                SizeSpec = dr["SizeSpec"].ToString(),
                Desc = dr["Patterncode"].ToString() + dr["Description"].ToString(),
                Artwork = dr["SubProcess"].ToString(),
                Quantity = dr["Qty"].ToString(),
                Barcode = dr["Bundle"].ToString(),
                Season = dr["Seasonid"].ToString(),
                Brand = dr["brand"].ToString(),
                Item = dr["item"].ToString(),
                EXCESS1 = MyUtility.Convert.GetBool(dr["IsEXCESS"]) ? "EXCESS" : string.Empty,
                NoBundleCardAfterSubprocess1 = MyUtility.Check.Empty(dr["NoBundleCardAfterSubprocess_String"]) ? string.Empty : "X",
                Replacement1 = string.Empty,
                ShipCode = dr["ShipCode"].ToString(),
                FabricPanelCode = dr["FabricPanelCode"].ToString(),
                Comb = dr["Comb"].ToString(),
                Cut = dr["cut"].ToString(),
                GroupCombCut = 0,
                No = GetNo(dr["Bundle"].ToString(), allNoDatas),
                BundleID = dr["BundleID"].ToString(),
                BundleNo = dr["Bundle"].ToString(),
                RFIDScan = MyUtility.Convert.GetBool(dr["RFIDScan"]),
                CutCell = dr["CutCell"].ToString(),
            }).ToList();
            if (data.Count > 0)
            {
                P12_Print p = new P12_Print(data);
                p.ShowDialog();
                this.Query();
                this.Grid_Filter();
            }
        }

        private void CheckBoxOnlyCompleted_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void Grid_Filter()
        {
            string filter = string.Empty;
            if (this.grid1.RowCount > 0)
            {
                switch (this.checkBoxOnlyNotYetCompleted.Checked)
                {
                    case true:
                        if (MyUtility.Check.Empty(this.grid1))
                        {
                            break;
                        }

                        filter = $@" RFPrintDate is null";
                        ((System.Data.DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;

                    case false:
                        if (MyUtility.Check.Empty(this.grid1))
                        {
                            break;
                        }

                        filter = string.Empty;
                        ((System.Data.DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;
                }
            }
        }

        private void BtnBundlewithQR_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            if (this.dtt == null || this.dtt.Rows.Count == 0)
            {
                this.grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }

            // DefaultView 是依據使用者排序列印
            System.Data.DataTable dtSelect = this.dtt.DefaultView.ToTable().AsEnumerable().Where(row => (bool)row["selected"]).TryCopyToDataTable(this.dtt);
            if (dtSelect.Rows.Count == 0)
            {
                this.grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }

            this.ShowWaitMessage("Process File!");
            List<P10_PrintData> data = dtSelect.AsEnumerable().Select(row1 => new P10_PrintData()
            {
                Group_right = row1["Group"].ToString(),
                Group_left = row1["left"].ToString(),
                CutRef = row1["CutRef"].ToString(),
                Tone = MyUtility.Convert.GetString(row1["Tone"]),
                Line = row1["Line"].ToString(),
                Cell = row1["Cell"].ToString(),
                POID = row1["POID"].ToString(),
                SP = row1["SP"].ToString(),
                Style = row1["Style"].ToString(),
                MarkerNo = row1["MarkerNo"].ToString(),
                Body_Cut = row1["Body_Cut"].ToString() + (MyUtility.Check.Empty(row1["SubCutNo"]) ? string.Empty : $"-{row1["SubCutNo"]}"),
                Parts = row1["Parts"].ToString(),
                Color = row1["Color2"].ToString(),
                Article = row1["Article"].ToString(),
                Size = row1["Size"].ToString(),
                SizeSpec = row1["SizeSpec"].ToString(),
                Desc = row1["Patterncode"].ToString() + row1["Description"].ToString(),
                Artwork = row1["SubProcess"].ToString(),
                Quantity = row1["Qty"].ToString(),
                Barcode = row1["Bundle"].ToString(),
                Season = row1["Seasonid"].ToString(),
                Brand = row1["brand"].ToString(),
                Item = row1["item"].ToString(),
                EXCESS1 = MyUtility.Convert.GetBool(row1["IsEXCESS"]) ? "EXCESS" : string.Empty,
                NoBundleCardAfterSubprocess1 = MyUtility.Check.Empty(row1["NoBundleCardAfterSubprocess_String"]) ? string.Empty : "X",
                Replacement1 = string.Empty,
                ShipCode = MyUtility.Convert.GetString(row1["ShipCode"]),
                FabricPanelCode = MyUtility.Convert.GetString(row1["FabricPanelCode"]),
                Comb = MyUtility.Convert.GetString(row1["Comb"]),
                Cut = MyUtility.Convert.GetString(row1["cut"]),
                GroupCombCut = 0,
                BundleID = row1["BundleID"].ToString(),
                CutCell = row1["CutCell"].ToString(),
                Dyelot = row1["Dyelot"].ToString(),
                ID = row1["ID"].ToString(),
                BundleNo = row1["Bundle"].ToString(),
                PatternDesc = row1["Description"].ToString(),
            }).ToList();

            // 使用Word來產生QRCode
            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };

            if (this.checkChangepagebyCut.Checked)
            {
                var x = data.GroupBy(g => new { g.Comb, g.Cut })
                     .Where(@group => @group.Any())
                     .OrderBy(@group => @group.Key.Comb ?? string.Empty)
                     .ThenBy(@group => @group.Key.Cut ?? string.Empty)
                     .AsEnumerable()
                     .Select((@group, i) => new
                     {
                         Items = @group,
                         Rank = ++i,
                     })
                     .SelectMany(v => v.Items, (s, i) => new
                     {
                         Item = i,
                         DenseRank = s.Rank,
                     }).ToList();

                // 先批次取得 BundleNo, No 資料, by POID, FabricPanelCode, Article, Size
                System.Data.DataTable allNoDatas = null;
                data.Select(s => new { s.POID, s.FabricPanelCode, s.Article, s.Size }).Distinct().ToList().ForEach(r =>
                {
                    if (allNoDatas == null)
                    {
                        allNoDatas = GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size);
                    }
                    else
                    {
                        allNoDatas.Merge(GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size));
                    }
                });

                x.Select(s => s.DenseRank).Distinct().OrderBy(o => o).ToList().ForEach(denseRank =>
                {
                    data.Clear();
                    x.Where(w => w.DenseRank == denseRank).ToList().ForEach(r =>
                    {
                        r.Item.GroupCombCut = r.DenseRank;
                        data.Add(r.Item);
                    });

                    P10_Print.Print_Word_QRCode(data, winword, allNoDatas);
                });
            }
            else
            {
                P10_Print.Print_Word_QRCode(data, winword);
            }

            // 有按才更新列印日期printdate。
            StringBuilder ups = new StringBuilder();
            foreach (DataRow dr in dtSelect.Rows)
            {
                ups.Append($@"
update bd
set bd.PrintDate = GETDATE()
from Bundle_Detail bd
where bd.BundleNo = '{dr["Bundle"]}'

update b
set b.PrintDate = GETDATE()
from Bundle b
inner join Bundle_Detail bd on b.id=bd.ID
where bd.BundleNo = '{dr["Bundle"]}'
");
            }

            this.HideWaitMessage();
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == DialogResult.OK)
            {
                string printer = pd.PrinterSettings.PrinterName;
                winword.ActivePrinter = printer;
                winword.PrintOut();

                Marshal.ReleaseComObject(winword);
                DualResult result = DBProxy.Current.Execute(null, ups.ToString());
                if (!result)
                {
                    this.ShowErr("Update PrintDate Error!", result);
                    return;
                }

                int pos = this.listControlBindingSource1.Position;
                this.Query();
                if (this.listControlBindingSource1.Count >= pos + 1)
                {
                    this.listControlBindingSource1.Position = pos;
                }
            }
        }
    }
}
