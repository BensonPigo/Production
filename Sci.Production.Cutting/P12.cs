using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

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
        private DataTable dtt;
        private string Addname;
        private DateTime? AddDate;
        private string Cutno;
        private string SpreadingNoID;

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("selected", header: "Sel", width: Widths.AnsiChars(4), iseditable: true, trueValue: true, falseValue: false)
                .DateTime("PrintDate", header: "Print Date", width: Widths.AnsiChars(18), iseditingreadonly: true)
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
                ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
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
                #region SQL

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
select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
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
into #tmp
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
    select SubProcess = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
)as SubProcess
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.PostSewingSubProcess = 1
		Order by e1.Subprocessid
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
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
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
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
	select distinct x.PatternCode,x.PatternDesc,x.Parts
	from Bundle_Detail_Allpart x with(nolock)
	where x.id=b.id

)bda
outer apply
(
    select SubProcess = 
    stuff((
        select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= bda.PatternCode
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
)as SubProcess
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= bda.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= bda.PatternCode and e1.PostSewingSubProcess = 1
		Order by e1.Subprocessid
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
                #region SQL
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
select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
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
into #tmp
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
    select SubProcess = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
)as SubProcess 
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.PostSewingSubProcess = 1
		Order by e1.Subprocessid
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
	, SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
    , a.BundleGroup [Group]
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
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
inner join brand WITH (NOLOCK) on brand.id = c.brandid
outer apply
(
    select SubProcess = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
)as SubProcess 
outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.NoBundleCardAfterSubprocess = 1
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode and e1.PostSewingSubProcess = 1
		Order by e1.Subprocessid
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
            this.contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);

            #region report RDLC 先不刪除 過段不知道要多久的時間, 再來刪除

            // bool checkone = false;
            // for (int i = 0; i < this.grid1.Rows.Count; i++)
            // {
            //    if (!MyUtility.Check.Empty(this.grid1[0, i].Value)
            //        && (bool)this.grid1[0, i].Value == true)
            //    {
            //        checkone = true;
            //    }
            // }

            // if (!checkone)
            // {
            //    this.grid1.Focus();
            //    MyUtility.Msg.ErrorBox("Grid must be chose one");
            //    return;
            // }

            // DataTable dtSelect;

            // dtSelect = this.dtt.DefaultView.ToTable()
            //    .AsEnumerable()
            //    .Where(row => (bool)row["selected"])
            //    .CopyToDataTable();

            // List<P12_PrintData> data = new List<P12_PrintData>();
            // bool changeGroup = true;
            // for (int i = 0; i < dtSelect.Rows.Count;)
            // {
            //    string thisGroupCut;
            //    if (this.checkChangepagebyCut.Checked)
            //    {
            //        thisGroupCut = MyUtility.Convert.GetString(dtSelect.Rows[i]["Comb"]) + MyUtility.Convert.GetString(dtSelect.Rows[i]["Cut"]);
            //    }
            //    else
            //    {
            //        thisGroupCut = "1";
            //    }

            // string tmpCut = "-1";
            //    var pdata = new P12_PrintData();
            //    data.Add(pdata);
            //    int j = 0;
            //    for (; j < 3 && i + j < dtSelect.Rows.Count; j++)
            //    {
            //        DataRow dr = dtSelect.Rows[i + j];
            //        if (this.checkChangepagebyCut.Checked)
            //        {
            //            tmpCut = MyUtility.Convert.GetString(dr["Comb"]) + MyUtility.Convert.GetString(dr["cut"]);
            //        }
            //        else
            //        {
            //            tmpCut = "1";
            //        }

            // if (changeGroup && tmpCut != thisGroupCut)
            //        {
            //            break;
            //        }

            // if (j == 0)
            //        {
            //            pdata.Group_right = dr["Group"].ToString();
            //            pdata.Group_left = dr["left"].ToString();
            //            pdata.Line = dr["Line"].ToString();
            //            pdata.Cell = dr["Cell"].ToString();
            //            pdata.SP = dr["SP"].ToString();
            //            pdata.Style = dr["Style"].ToString();
            //            pdata.Item = dr["Item"].ToString();
            //            pdata.Parts = dr["Parts"].ToString();
            //            pdata.Color = dr["Color2"].ToString();
            //            pdata.Size = dr["Size"].ToString();
            //            pdata.SizeSpec = dr["SizeSpec"].ToString();
            //            pdata.Desc = dr["Description"].ToString();
            //            pdata.Qty = dr["Qty"].ToString();
            //            pdata.Barcode = dr["Bundle"].ToString();
            //            pdata.Patterncode = dr["Patterncode"].ToString();
            //            pdata.MarkerNo = dr["MarkerNo"].ToString();
            //            pdata.Season = dr["Seasonid"].ToString();
            //            pdata.brand = dr["brand"].ToString();
            //            pdata.item = dr["item"].ToString();
            //            pdata.EXCESS1 = dr["IsEXCESS"].ToString();
            //            pdata.CutRef = tmpCut;
            //            pdata.NoBundleCardAfterSubprocess1 = MyUtility.Check.Empty(dr["NoBundleCardAfterSubprocess_String"]) ? string.Empty : "(X)";

            // pdata.SubProcess = dr["SubProcess"].ToString();
            //            pdata.Body_Cut = dr["Body_Cut"].ToString();
            //        }
            //        else if (j == 1)
            //        {
            //            pdata.Group_right2 = dr["Group"].ToString();
            //            pdata.Group_left2 = dr["left"].ToString();
            //            pdata.Line2 = dr["Line"].ToString();
            //            pdata.Cell2 = dr["Cell"].ToString();
            //            pdata.SP2 = dr["SP"].ToString();
            //            pdata.Style2 = dr["Style"].ToString();
            //            pdata.Item2 = dr["Item"].ToString();
            //            pdata.Body_Cut2 = dr["Body_Cut"].ToString();
            //            pdata.Parts2 = dr["Parts"].ToString();
            //            pdata.Color2 = dr["Color2"].ToString();
            //            pdata.Size2 = dr["Size"].ToString();
            //            pdata.SizeSpec2 = dr["SizeSpec"].ToString();
            //            pdata.Desc2 = dr["Description"].ToString();
            //            pdata.SubProcess2 = dr["SubProcess"].ToString();
            //            pdata.Qty2 = dr["Qty"].ToString();
            //            pdata.Barcode2 = dr["Bundle"].ToString();
            //            pdata.Patterncode2 = dr["Patterncode"].ToString();
            //            pdata.MarkerNo2 = dr["MarkerNo"].ToString();
            //            pdata.Season2 = dr["Seasonid"].ToString();
            //            pdata.brand2 = dr["brand"].ToString();
            //            pdata.item2 = dr["item"].ToString();
            //            pdata.EXCESS2 = dr["IsEXCESS"].ToString();
            //            pdata.CutRef2 = tmpCut;
            //            pdata.NoBundleCardAfterSubprocess2 = MyUtility.Check.Empty(dr["NoBundleCardAfterSubprocess_String"]) ? string.Empty : "(X)";
            //        }
            //        else
            //        {
            //            pdata.Group_right3 = dr["Group"].ToString();
            //            pdata.Group_left3 = dr["left"].ToString();
            //            pdata.Line3 = dr["Line"].ToString();
            //            pdata.Cell3 = dr["Cell"].ToString();
            //            pdata.SP3 = dr["SP"].ToString();
            //            pdata.Style3 = dr["Style"].ToString();
            //            pdata.Item3 = dr["Item"].ToString();
            //            pdata.Body_Cut3 = dr["Body_Cut"].ToString();
            //            pdata.Parts3 = dr["Parts"].ToString();
            //            pdata.Color3 = dr["Color2"].ToString();
            //            pdata.Size3 = dr["Size"].ToString();
            //            pdata.SizeSpec3 = dr["SizeSpec"].ToString();
            //            pdata.Desc3 = dr["Description"].ToString();
            //            pdata.SubProcess3 = dr["SubProcess"].ToString();
            //            pdata.Qty3 = dr["Qty"].ToString();
            //            pdata.Barcode3 = dr["Bundle"].ToString();
            //            pdata.Patterncode3 = dr["Patterncode"].ToString();
            //            pdata.MarkerNo3 = dr["MarkerNo"].ToString();
            //            pdata.Season3 = dr["Seasonid"].ToString();
            //            pdata.brand3 = dr["brand"].ToString();
            //            pdata.item3 = dr["item"].ToString();
            //            pdata.EXCESS3 = dr["IsEXCESS"].ToString();
            //            pdata.CutRef3 = tmpCut;
            //            pdata.NoBundleCardAfterSubprocess3 = MyUtility.Check.Empty(dr["NoBundleCardAfterSubprocess_String"]) ? string.Empty : "(X)";
            //        }
            //    }

            // if (changeGroup && tmpCut != thisGroupCut)
            //    {
            //        i += j;
            //    }
            //    else
            //    {
            //        i += 3;
            //    }
            // }

            // var res = data;

            //// 指定是哪個 RDLC
            // Type reportResourceNamespace = typeof(P12_PrintData);
            // Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            // string reportResourceName = "P12_Print.rdlc";
            // if (!(this.result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
            // {
            //    this.ShowException(this.result);
            //    return;
            // }

            // ReportDefinition report = new ReportDefinition
            // {
            //    ReportDataSource = res,
            //    ReportResource = reportresource,
            // };

            //// 開啟 report view
            // var frm = new Win.Subs.ReportView(report);

            // 有按才更新列印日期printdate。
//            var res2 = this.dtt.AsEnumerable()
//                .Where(row => (bool)row["selected"])
//            .Select(row1 => new P12_PrintData()
//            {
//                SP = row1["SP"].ToString(),
//                Barcode = row1["Bundle"].ToString(),
//            }).ToList();
//            StringBuilder ups = new StringBuilder();
//            foreach (var item in res2)
//            {
//                ups.Append(string.Format(
//                    @"
// update bd
// set bd.PrintDate = GETDATE()
// from Bundle_Detail bd WITH (NOLOCK)
// where bd.BundleNo = '{0}'",
//                    item.Barcode));

// ups.Append(string.Format(
//                    @"
//                            update b
//                            set b.PrintDate = GETDATE()
//                            from Bundle b WITH (NOLOCK)
//                            inner join Bundle_Detail bd WITH (NOLOCK) on b.id=bd.ID
//                            where bd.BundleNo = '{1}'",
//                    item.SP,
//                    item.Barcode));
//            }

// frm.viewer.Print += (s, eArgs) =>
//            {
//                var result3 = DBProxy.Current.Execute(null, ups.ToString());
//            };
//            if (this.MdiParent != null)
//            {
//                frm.MdiParent = this.MdiParent;
//            }

// frm.Show();
            // return;
            #endregion
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

            DataTable selects = r.CopyToDataTable();

            string fileName = "Cutting_P12";
            string fieldList = "Bundle,CutRef,POID,SP,Group,Line,SpreadingNoID,Cell,Style,Item,Comb,Cut,Article,Color,Size,SizeSpec,Cutpart,Description,SubProcess,Parts,Qty";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(selects, string.Empty, $"{fileName}.xltx", 1, true, fieldList, excelApp);
            Marshal.ReleaseComObject(excelApp);
            return;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Layout1_Click(object sender, EventArgs e)
        {
            this.PrintBarcode(1);
        }

        private void Layout2_Click(object sender, EventArgs e)
        {
            this.PrintBarcode(2);
        }

        private void PrintBarcode(int layout)
        {
            this.grid1.ValidateControl();
            DataTable dtSelect = this.dtt.DefaultView.ToTable().AsEnumerable().Where(row => (bool)row["selected"]).TryCopyToDataTable(this.dtt);
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
                Line = row1["Line"].ToString(),
                Cell = row1["Cell"].ToString(),
                POID = row1["POID"].ToString(),
                SP = row1["SP"].ToString(),
                Style = row1["Style"].ToString(),
                MarkerNo = row1["MarkerNo"].ToString(),
                Body_Cut = row1["Body_Cut"].ToString(),
                Parts = row1["Parts"].ToString(),
                Color = row1["Color2"].ToString(),
                Article = row1["Article"].ToString(),
                Size = row1["Size"].ToString(),
                SizeSpec = row1["SizeSpec"].ToString(),
                Desc = row1["Description"].ToString(),
                Artwork = row1["SubProcess"].ToString(),
                Quantity = row1["Qty"].ToString(),
                Barcode = row1["Bundle"].ToString(),
                Season = row1["Seasonid"].ToString(),
                Brand = row1["brand"].ToString(),
                Item = row1["item"].ToString(),
                EXCESS1 = MyUtility.Convert.GetBool(row1["IsEXCESS"]) ? "EXCESS" : string.Empty,
                NoBundleCardAfterSubprocess1 = MyUtility.Check.Empty(row1["NoBundleCardAfterSubprocess_String"]) ? string.Empty : "(X)",
                Replacement1 = string.Empty,
                ShipCode = MyUtility.Convert.GetString(row1["ShipCode"]),
                FabricPanelCode = MyUtility.Convert.GetString(row1["FabricPanelCode"]),
                Comb = MyUtility.Convert.GetString(row1["Comb"]),
                Cut = MyUtility.Convert.GetString(row1["cut"]),
                GroupCombCut = 0,
            }).ToList();

            string fileName = "Cutting_P10_Layout1";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx");
            Excel.Workbook workbook = excelApp.ActiveWorkbook;

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

                int page = x.Max(m => m.DenseRank);

                for (int i = 0; i < page; i++)
                {
                    if (i > 0)
                    {
                        Excel.Worksheet worksheet1 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[1];
                        Excel.Worksheet worksheetn = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[i + 1];
                        worksheet1.Copy(worksheetn);
                        Marshal.ReleaseComObject(worksheet1);
                        Marshal.ReleaseComObject(worksheetn);
                    }
                }

                for (int i = 1; i <= page; i++)
                {
                    data.Clear();
                    x.Where(w => w.DenseRank == i).ToList().ForEach(r =>
                    {
                        r.Item.GroupCombCut = r.DenseRank;
                        data.Add(r.Item);
                    });

                    Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[i];
                    this.ProcessPrint(data, worksheet, layout);
                }
            }
            else
            {
                Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
                this.ProcessPrint(data, worksheet, layout);
            }

            // 有按才更新列印日期printdate。
            StringBuilder ups = new StringBuilder();
            foreach (var item in data)
            {
                ups.Append($@"
update bd
set bd.PrintDate = GETDATE()
from Bundle_Detail bd WITH (NOLOCK)
where bd.BundleNo = '{item.Barcode}'

update b
set b.PrintDate = GETDATE()
from Bundle b WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on b.id=bd.ID
where bd.BundleNo = '{item.Barcode}'");
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
            }

            Marshal.ReleaseComObject(excelApp);
        }

        private void ProcessPrint(List<P10_PrintData> data, Excel.Worksheet worksheet, int layout)
        {
            // 範本預設 A4 紙, 分割 9 格貼紙格式, 因印表機邊界, 9 格格式有點不同
            if (data.Count > 9)
            {
                int p = (data.Count - 1) / 9;
                for (int pi = 1; pi <= p; pi++)
                {
                    Excel.Range r1 = worksheet.get_Range("A1", "A14").EntireRow;
                    Excel.Range r2 = worksheet.get_Range($"A{1 + (pi * 14)}");
                    r2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r1.Copy());
                }
            }

            int i = 0;
            int col_ref = 0;
            int row_ref = 0;
            data.ForEach(r =>
            {
                string no = P10_Print.GetNo(r.POID, r.FabricPanelCode, r.Article, r.Size, r.Barcode);
                string contian;
                if (layout == 1)
                {
                    contian = $@"Tone/Grp: {r.Group_right}  Line#: {r.Line}   {r.Group_left}  Cut/L:
SP#:{r.SP}
Style#: {r.Style}
Sea: {r.Season}     Brand: {r.ShipCode}
Marker#: {r.MarkerNo}
Cut#: {r.Body_Cut}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}
Dese: {r.Desc}
Sub Process: {r.Artwork}
Qty: {r.Quantity}     No: {no}";
                }
                else
                {
                    contian = $@"Tone/Grp: {r.Group_right}  Line#: {r.Line}   {r.Group_left}  Cut/L:
SP#:{r.SP}
Style#: {r.Style}
Sea: {r.Season}     Brand: {r.ShipCode}
Marker#: {r.MarkerNo}
Cut#: {r.Body_Cut}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}
Dese: {r.Desc}
Sub Process: {r.Artwork}
Qty: {r.Quantity}     Item: {r.Item}";
                }

                row_ref = i / 3;
                row_ref = (row_ref * 5) - (row_ref / 3);
                col_ref = (i % 3) * 4;
                int cutindex = contian.IndexOf("Cut/L");
                worksheet.Cells[1 + row_ref, 1 + col_ref] = contian;
                worksheet.Cells[1 + row_ref, 1 + col_ref].Characters(1, 8).Font.Bold = true; // 部分粗體
                worksheet.Cells[1 + row_ref, 1 + col_ref].Characters(cutindex, 6).Font.Bold = true; // 部分粗體
                worksheet.Cells[2 + row_ref, 1 + col_ref] = r.EXCESS1;
                worksheet.Cells[2 + row_ref, 2 + col_ref] = r.NoBundleCardAfterSubprocess1;
                worksheet.Cells[3 + row_ref, 1 + col_ref] = "*" + r.Barcode + "*";

                // 邊框 」貼紙裁線
                if (i % 3 != 2 && (i / 3) % 3 != 2)
                {
                    string colN = MyUtility.Excel.ConvertNumericToExcelColumn(3 + col_ref);
                    Excel.Range excelRange = worksheet.get_Range($"{colN}{4 + row_ref}");
                    excelRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                    excelRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = 1;
                }

                i++;
            });
        }

        private void P12_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
