﻿using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace Sci.Production.Cutting
{
    public partial class P12 : Sci.Win.Tems.QueryForm
    {
        BindingList<P12_PrintData> Data = new BindingList<P12_PrintData>();
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            GridSetup();
            this.EditMode = true;
            this.comboSortBy.SelectedIndex = 0;
        }

        string Cut_Ref;
        string Cut_Ref1;
        string SP;
        string SP1;
        string POID;
        string Bundle;
        string Bundle1;
        DateTime? Est_CutDate;
        string Cell;
        string size;
        string Sort_by;
        string Extend;
        DualResult result;
        DataTable dtt;
        string Addname;
        DateTime? AddDate;
        string Cutno;
        string Comb;
        
        void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("selected", header: "Sel", width: Widths.AnsiChars(4), iseditable: true, trueValue: true, falseValue: false)
                .DateTime("PrintDate", header: "Print Date", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Date("CreateDate", header: "Create Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Bundle", header: "Bundle#", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("POID", header: "POID", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("SP", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("Group", header: "Group", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Line", header: "Line", width: Widths.AnsiChars(4), iseditingreadonly: true)
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
                ;

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {

            if (this.txtCutRefStart.Text.Empty() && this.txtCutRefEnd.Text.Empty() 
                && this.txtSPNoStart.Text.Empty() && this.txtSPNoEnd.Text.Empty() 
                && this.txtPOID.Text.Empty() 
                && this.txtBundleStart.Text.Empty() && this.txtBundleEnd.Text.Empty()
                && this.dateBox1.Value.Empty() && this.dateBundlecreatedDate.Value.Empty())
            {
                txtCutRefStart.Focus();
                if (dtt != null) dtt.Clear();
                MyUtility.Msg.WarningBox("[Cut_Ref# and SP# and POID and Bundle# and Est.Cut Date and Bundle created Date ] can not be all null !!");
                return;
            }

            if ((!this.txtCutRefStart.Text.Empty() && this.txtCutRefEnd.Text.Empty()) || (this.txtCutRefStart.Text.Empty() && !this.txtCutRefEnd.Text.Empty())
                || (!this.txtSPNoStart.Text.Empty() && this.txtSPNoEnd.Text.Empty()) || (this.txtSPNoStart.Text.Empty() && !this.txtSPNoEnd.Text.Empty())
                || (!this.txtBundleStart.Text.Empty() && this.txtBundleEnd.Text.Empty()) || (this.txtBundleStart.Text.Empty() && !this.txtBundleEnd.Text.Empty())
                )
            {
                txtCutRefStart.Focus();
                if (dtt != null) dtt.Clear();
                MyUtility.Msg.WarningBox("[Cut_Ref# and SP# and Bundle#] must enter start and end !!");
                return;
            }

            this.ShowWaitMessage("Data processing, please wait...");

            Cut_Ref = txtCutRefStart.Text.ToString();
            Cut_Ref1 = txtCutRefEnd.Text.ToString();
            SP = txtSPNoStart.Text.ToString();
            SP1 = txtSPNoEnd.Text.ToString();
            POID = txtPOID.Text.ToString();
            Bundle = txtBundleStart.Text.ToString();
            Bundle1 = txtBundleEnd.Text.ToString();
            Est_CutDate = dateBox1.Value;
            Cell = txtCell.Text.ToString();
            size = txtSize.Text.ToString();
            Sort_by = comboSortBy.SelectedIndex.ToString();
            Extend = checkExtendAllParts.Checked.ToString();
            Addname = txtuser1.TextBox1.Text;
            AddDate = dateBundlecreatedDate.Value;
            Cutno = txtCutno.Text;
            Comb = txtComb.Text;
            
            string sqlWhere = "";
            string sb = "";
            string declare = string.Empty;
            List<string> sqlWheres = new List<string>();

            sqlWheres.Add("b.MDivisionID=@Keyword");


            if (!this.txtCutRefStart.Text.Empty() && !this.txtCutRefEnd.Text.Empty())
            {
                sqlWheres.Add("b.CutRef between @Cut_Ref and @Cut_Ref1");
            }
            if (!this.txtSPNoStart.Text.Empty() && !this.txtSPNoEnd.Text.Empty())
            {
                sqlWheres.Add("b.OrderID  between @SP and @SP1");
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
                declare += $@" declare @Est_CutDate date = '{((DateTime)Est_CutDate).ToString("yyyy/MM/dd")}' ";
            }
            if (!MyUtility.Check.Empty(Addname))
            {
                sqlWheres.Add(" b.AddName = @AddName");
            }
            if (!this.dateBundlecreatedDate.Value.Empty())
            {
                sqlWheres.Add(" format(b.AddDate,'yyyy/MM/dd') = @AddDate");
                declare += $@" declare @AddDate varchar(10) = '{((DateTime)AddDate).ToString("yyyy/MM/dd")}' ";
            }
            if (!MyUtility.Check.Empty(Cutno))
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

            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            if (!checkExtendAllParts.Checked)
            {
                declare += $@" declare @extend bit = 0 ";
            }

            string sqlcmd = string.Empty;
            
            if (checkExtendAllParts.Checked)  //有勾[Extend All Parts]
            {
                #region SQL

                DBProxy.Current.DefaultTimeout = 1800;  //加長時間為30分鐘，避免timeout
                sqlcmd = $@"
declare @Keyword varchar(8) = '{Sci.Env.User.Keyword}'
declare @Cut_Ref varchar(6) = '{Cut_Ref}'
declare @Cut_Ref1 varchar(6) = '{Cut_Ref1}'
declare @SP varchar(13) = '{SP}'
declare @SP1 varchar(13) = '{SP1}'
declare @POID varchar(13) = '{POID}'
declare @Bundle varchar(13) = '{Bundle}'
declare @Bundle1 varchar(13) = '{Bundle1}'
declare @Cell varchar(3) = '{Cell}'
declare @size varchar(8) = '{size}'
declare @Addname varchar(10) = '{Addname}'
declare @Cutno varchar(6) = '{Cutno}'
declare @FtyGroup varchar(8) = '{txtfactoryByM.Text}'
declare @Comb varchar(2) = '{txtComb.Text}'
{declare}
set arithabort on
select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
    , b.Orderid [SP]
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
into #tmp
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
outer apply
(
    select SubProcess = 
    (
        select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
        for xml path('')
    )
)as SubProcess
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
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
    , b.Orderid [SP]
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
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
outer apply
(
	select distinct x.PatternCode,x.PatternDesc,x.Parts
	from Bundle_Detail_Allpart x with(nolock)
	where x.id=b.id

)bda
outer apply
(
    select SubProcess = 
    (
        select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= bda.PatternCode
        for xml path('')
    )
)as SubProcess
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
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
" + sb+ @"
OPTION (RECOMPILE)"
;                
                #endregion
            }
            else  //沒勾[Extend All Parts]
            {
                #region SQL
                sqlcmd = $@"
declare @Keyword varchar(8) = '{Sci.Env.User.Keyword}'
declare @Cut_Ref varchar(6) = '{Cut_Ref}'
declare @Cut_Ref1 varchar(6) = '{Cut_Ref1}'
declare @SP varchar(13) = '{SP}'
declare @SP1 varchar(13) = '{SP1}'
declare @POID varchar(13) = '{POID}'
declare @Bundle varchar(13) = '{Bundle}'
declare @Bundle1 varchar(13) = '{Bundle1}'
declare @Cell varchar(3) = '{Cell}'
declare @size varchar(8) = '{size}'
declare @Addname varchar(10) = '{Addname}'
declare @Cutno varchar(6) = '{Cutno}'
declare @FtyGroup varchar(8) = '{txtfactoryByM.Text}'
declare @Comb varchar(2) = '{txtComb.Text}'
{declare}
set arithabort on
select 
    Convert(bit,0) as selected
    , a.PrintDate
    , CreateDate = b.AddDate
    , a.BundleNo [Bundle]
    , b.CutRef [CutRef]
    , b.POID [POID]
    , b.Orderid [SP]
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
into #tmp
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
outer apply
(
    select SubProcess = 
    (
        select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
        for xml path('')
    )
)as SubProcess 
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
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
    , b.Orderid [SP]
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
from dbo.Bundle_Detail a WITH (NOLOCK)
inner join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
inner join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
outer apply
(
    select SubProcess = 
    (
        select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= a.PatternCode
        for xml path('')
    )
)as SubProcess 
OUTER APPLY(
	SELECT TOP 1 
		MarkerNo  
        ,EstCutDate
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
" + sb+@"
OPTION (RECOMPILE)"
;  
                #endregion
            }

            DBProxy.Current.DefaultTimeout = 1800;  //加長時間為30分鐘，避免timeout
            result = DBProxy.Current.Select("", sqlcmd, out dtt);
            if (!result)
            {
                ShowErr(result);
                this.HideWaitMessage();
                return;
            }
            if (dtt.Rows.Count == 0)  MyUtility.Msg.WarningBox("Data not found!!"); 
            listControlBindingSource1.DataSource = dtt;
            DBProxy.Current.DefaultTimeout = 300;  //恢復時間為5分鐘
            this.HideWaitMessage();
        }

        private void btnBundleCard_Click(object sender, EventArgs e)
        {
            #region report

            bool checkone = false;
            for (int i = 0; i < this.grid1.Rows.Count; i++)
            {
                if (!MyUtility.Check.Empty(this.grid1[0, i].Value)  //判斷是否為空值
                    && (bool)this.grid1[0, i].Value == true)　//判斷是否有打勾
                {
                    checkone = true;
                }
            }
            if (!checkone)
            {
                grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }

            DataTable dtSelect;

            dtSelect = dtt.DefaultView.ToTable()
                .AsEnumerable()
                .Where(row=> (bool)row["selected"])
                .CopyToDataTable();
            
            List<P12_PrintData> data = new List<P12_PrintData>();
            bool changeGroup = true;
            for (int i = 0; i< dtSelect.Rows.Count;)
            {
                string thisGroupCut;
                if (checkChangepagebyCut.Checked)
                {
                    thisGroupCut = MyUtility.Convert.GetString(dtSelect.Rows[i]["Comb"]) + MyUtility.Convert.GetString(dtSelect.Rows[i]["Cut"]);
                }
                else
                {
                    thisGroupCut = "1";
                }
                string tmpCut = "-1";
                var pdata = new P12_PrintData();
                data.Add(pdata);
                int j = 0;
                for (; j < 3 && i + j < dtSelect.Rows.Count; j++)
                {
                    DataRow dr = dtSelect.Rows[i + j];
                    if (checkChangepagebyCut.Checked)
                    {
                        tmpCut = MyUtility.Convert.GetString(dr["Comb"]) + MyUtility.Convert.GetString(dr["cut"]);
                    }
                    else
                    {
                        tmpCut = "1";
                    }
                    
                    if (changeGroup && tmpCut != thisGroupCut)
                    {
                        break;
                    }

                    if (j == 0)
                    {
                        pdata.Group_right = dr["Group"].ToString();
                        pdata.Group_left = dr["left"].ToString();
                        pdata.Line = dr["Line"].ToString();
                        pdata.Cell = dr["Cell"].ToString();
                        pdata.SP = dr["SP"].ToString();
                        pdata.Style = dr["Style"].ToString();
                        pdata.Item = dr["Item"].ToString();
                        pdata.Body_Cut = dr["Body_Cut"].ToString();
                        pdata.Parts = dr["Parts"].ToString();
                        pdata.Color = dr["Color2"].ToString();
                        pdata.Size = dr["Size"].ToString();
                        pdata.SizeSpec = dr["SizeSpec"].ToString();
                        pdata.Desc = dr["Description"].ToString();
                        pdata.SubProcess = dr["SubProcess"].ToString();
                        pdata.Qty = dr["Qty"].ToString();
                        pdata.Barcode = dr["Bundle"].ToString();
                        pdata.Patterncode = dr["Patterncode"].ToString();
                        pdata.MarkerNo = dr["MarkerNo"].ToString();
                        pdata.Season = dr["Seasonid"].ToString();
                        pdata.brand = dr["brand"].ToString();
                        pdata.item = dr["item"].ToString();
                        pdata.CutRef = tmpCut;
                    }
                    else if (j == 1)
                    {
                        pdata.Group_right2 = dr["Group"].ToString();
                        pdata.Group_left2 = dr["left"].ToString();
                        pdata.Line2 = dr["Line"].ToString();
                        pdata.Cell2 = dr["Cell"].ToString();
                        pdata.SP2 = dr["SP"].ToString();
                        pdata.Style2 = dr["Style"].ToString();
                        pdata.Item2 = dr["Item"].ToString();
                        pdata.Body_Cut2 = dr["Body_Cut"].ToString();
                        pdata.Parts2 = dr["Parts"].ToString();
                        pdata.Color2 = dr["Color2"].ToString();
                        pdata.Size2 = dr["Size"].ToString();
                        pdata.SizeSpec2 = dr["SizeSpec"].ToString();
                        pdata.Desc2 = dr["Description"].ToString();
                        pdata.SubProcess2 = dr["SubProcess"].ToString();
                        pdata.Qty2 = dr["Qty"].ToString();
                        pdata.Barcode2 = dr["Bundle"].ToString();
                        pdata.Patterncode2 = dr["Patterncode"].ToString();
                        pdata.MarkerNo2 = dr["MarkerNo"].ToString();
                        pdata.Season2 = dr["Seasonid"].ToString();
                        pdata.brand2 = dr["brand"].ToString();
                        pdata.item2 = dr["item"].ToString();
                        pdata.CutRef2 = tmpCut;
                    }
                    else
                    {
                        pdata.Group_right3 = dr["Group"].ToString();
                        pdata.Group_left3 = dr["left"].ToString();
                        pdata.Line3 = dr["Line"].ToString();
                        pdata.Cell3 = dr["Cell"].ToString();
                        pdata.SP3 = dr["SP"].ToString();
                        pdata.Style3 = dr["Style"].ToString();
                        pdata.Item3 = dr["Item"].ToString();
                        pdata.Body_Cut3 = dr["Body_Cut"].ToString();
                        pdata.Parts3 = dr["Parts"].ToString();
                        pdata.Color3 = dr["Color2"].ToString();
                        pdata.Size3 = dr["Size"].ToString();
                        pdata.SizeSpec3 = dr["SizeSpec"].ToString();
                        pdata.Desc3 = dr["Description"].ToString();
                        pdata.SubProcess3 = dr["SubProcess"].ToString();
                        pdata.Qty3 = dr["Qty"].ToString();
                        pdata.Barcode3 = dr["Bundle"].ToString();
                        pdata.Patterncode3 = dr["Patterncode"].ToString();
                        pdata.MarkerNo3 = dr["MarkerNo"].ToString();
                        pdata.Season3 = dr["Seasonid"].ToString();
                        pdata.brand3 = dr["brand"].ToString();
                        pdata.item3 = dr["item"].ToString();
                        pdata.CutRef3 = tmpCut;
                    }
                }
                
                if (changeGroup && tmpCut != thisGroupCut)
                {
                    i += j;
                }
                else
                {
                    i += 3;
                }
                
            }
            var res = data;

            //指定是哪個 RDLC

            Type ReportResourceNamespace = typeof(P12_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P12_Print.rdlc";
            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                this.ShowException(result);
                return;
            }
            ReportDefinition report = new ReportDefinition();
            report.ReportDataSource = res;
            report.ReportResource = reportresource;

            //開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            //有按才更新列印日期printdate。

            var res2 = dtt.AsEnumerable()
                .Where(row => (bool)row["selected"])
            .Select(row1 => new P12_PrintData()
            {
                SP = row1["SP"].ToString(),
                Barcode = row1["Bundle"].ToString()
            }).ToList();
            StringBuilder ups = new StringBuilder();
            foreach (var item in res2)
            {
                ups.Append(string.Format(
                    @"
update bd
set bd.PrintDate = GETDATE()
from Bundle_Detail bd WITH (NOLOCK)
where bd.BundleNo = '{0}'",
                    item.Barcode));

                ups.Append(string.Format(@"
                            update b
                            set b.PrintDate = GETDATE()
                            from Bundle b WITH (NOLOCK)
                            inner join Bundle_Detail bd WITH (NOLOCK) on b.id=bd.ID
                            where bd.BundleNo = '{1}'"
                          , item.SP, item.Barcode));
            }

            frm.viewer.Print += (s, eArgs) =>
            {
                var result3 = DBProxy.Current.Execute(null, ups.ToString());
            };
            if(MdiParent!=null)  frm.MdiParent = MdiParent;
            frm.Show();

            return;

            #endregion
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            #region excel
            bool checkone = false;
            for (int i = 0; i < this.grid1.Rows.Count; i++)
            {
                if (!MyUtility.Check.Empty(this.grid1[0, i].Value)  //判斷是否為空值
                    && (bool)this.grid1[0, i].Value == true)　//判斷是否有打勾
                {
                    checkone = true;
                }

                if (false && true)
                {
                    //execute aaa
                }
            }
            if (!checkone)
            {
                grid1.Focus();
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                return;
            }
            DataTable selects = dtt.AsEnumerable()
                .Where(row => (bool)row["selected"])
                .CopyToDataTable();

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P12.xltx"); //預先開啟excel app                         
            MyUtility.Excel.CopyToXls(selects, "", "Cutting_P12.xltx", 1, true,"Bundle,CutRef,POID,SP,Group,Line,Cell,Style,Item,Comb,Cut,Article,Color,Size,SizeSpec,Cutpart,Description,SubProcess,Parts,Qty", objApp);      // 將datatable copy to excel
            return;
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
    }
}
