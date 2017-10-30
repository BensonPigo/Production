using Ict;
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
            this.ShowWaitMessage("Data processing, please wait...");
            if (this.txtCutRefStart.Text.Empty() && this.txtCutRefEnd.Text.Empty() && this.txtSPNoStart.Text.Empty() && this.txtSPNoEnd.Text.Empty() && this.txtPOID.Text.Empty() && this.txtBundleStart.Text.Empty()
                && this.txtBundleEnd.Text.Empty() && this.dateBox1.Value.Empty() && this.dateBundlecreatedDate.Value.Empty())
            {
                txtCutRefStart.Focus();
                if (dtt != null) dtt.Clear();
                this.HideWaitMessage();
                MyUtility.Msg.ErrorBox("[Cut_Ref# and SP# and POID and Bundle# and Est.Cut Date and Bundle created Date ] can not be all null !!");
                return;
            }


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

            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string sb = "";
            List<string> sqlWheres = new List<string>();

            sqlWheres.Add("b.MDivisionID=@Keyword");
            lis.Add(new SqlParameter("@Keyword", Sci.Env.User.Keyword));


            if (!this.txtCutRefStart.Text.Empty() && !this.txtCutRefEnd.Text.Empty())
            {
                sqlWheres.Add("b.CutRef between @Cut_Ref and @Cut_Ref1");
                lis.Add(new SqlParameter("@Cut_Ref", Cut_Ref));
                lis.Add(new SqlParameter("@Cut_Ref1", Cut_Ref1));
            }
            if (!this.txtSPNoStart.Text.Empty() && !this.txtSPNoEnd.Text.Empty())
            {
                sqlWheres.Add("b.OrderID  between @SP and @SP1");
                lis.Add(new SqlParameter("@SP", SP));
                lis.Add(new SqlParameter("@SP1", SP1));
            }
            if (!this.txtPOID.Text.Empty())
            {
                sqlWheres.Add("b.POID=@POID");
                lis.Add(new SqlParameter("@POID", POID));
            }
            if (!this.txtBundleStart.Text.Empty() && !this.txtBundleEnd.Text.Empty())
            {
                sqlWheres.Add("a.BundleNo between @Bundle and @Bundle1");
                lis.Add(new SqlParameter("@Bundle", Bundle));
                lis.Add(new SqlParameter("@Bundle1", Bundle1));
            }
            if (!this.txtCell.Text.Empty())
            {
                sqlWheres.Add("b.SewingCell =@Cell");
                lis.Add(new SqlParameter("@Cell", Cell));
            }
            if (!this.txtSize.Text.Empty())
            {
                sqlWheres.Add("a.SizeCode =@Size");
                lis.Add(new SqlParameter("@Size", size));
            }

            if (!this.dateBox1.Value.Empty())
            {
                sqlWheres.Add("e.EstCutDate=@Est_CutDate");
                lis.Add(new SqlParameter("@Est_CutDate", Est_CutDate));
            }
            if (!MyUtility.Check.Empty(Addname))
            {
                sqlWheres.Add(" b.AddName = @AddName");
                lis.Add(new SqlParameter("@AddName", Addname));
            }
            if (!this.dateBundlecreatedDate.Value.Empty())
            {
                sqlWheres.Add(" format(b.AddDate,'yyyy/MM/dd') = @AddDate");
                lis.Add(new SqlParameter("@AddDate", AddDate));
            }
            if (!MyUtility.Check.Empty(Cutno))
            {
                sqlWheres.Add(" b.Cutno=@Cutno");
                lis.Add(new SqlParameter("@Cutno", Cutno));
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
                lis.Add(new SqlParameter("@FtyGroup ", txtfactoryByM.Text));
            }

            if (!this.txtComb.Text.Empty())
            {
                sqlWheres.Add(" b.PatternPanel  = @Comb ");
                lis.Add(new SqlParameter("@Comb ", txtComb.Text));
            }

            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            if (checkExtendAllParts.Checked)
                lis.Add(new SqlParameter("@extend", "1"));
            else
                lis.Add(new SqlParameter("@extend", "0"));

            string sqlcmd = string.Empty;
            
            if (checkExtendAllParts.Checked)  //有勾[Extend All Parts]
            {
                #region SQL
                sqlcmd = string.Format(@"
select *
from(
    select 
        Convert(bit,0) as selected
        ,a.BundleNo [Bundle]
        ,b.CutRef [CutRef]
        ,b.POID [POID]
        ,b.Orderid [SP]
        ,a.BundleGroup [Group]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,c.StyleID [Style]
        ,b.Item [Item]
        ,b.PatternPanel [Comb]
        ,b.cutno [Cut]
        ,b.Article [Article]
        ,b.Colorid [Color]
        ,b.Article + '\' + b.Colorid [Color2]
        ,a.SizeCode [Size]
        ,a.PatternCode [Cutpart]
        ,'('+a.Patterncode+')' [Patterncode]
        ,a.PatternDesc [Description]
        --,SubProcess.SubProcess [SubProcess]
        ,[SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
        ,a.Parts [Parts]
        ,a.Qty [Qty]
        ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
        ,c.FactoryID  [left]
        ,e.MarkerNo
    from dbo.Bundle_Detail a WITH (NOLOCK)
    left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
    left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
    left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef<>'' and b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
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
    " + sqlWhere + @" and a.Patterncode != 'ALLPARTS' 
                                        
    union all

    select DISTINCT 
        Convert(bit,0) as selected
        ,a.BundleNo [Bundle]
        ,b.CutRef [CutRef]
        ,b.POID [POID]
        ,b.Orderid [SP]
        ,a.BundleGroup [Group]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,c.StyleID [Style]
        ,b.Item [Item]
        ,b.PatternPanel [Comb]
        ,b.cutno [Cut]
        ,b.Article [Article]
        ,b.Colorid [Color]
        ,b.Article + '\' + b.Colorid [Color2]
        ,a.SizeCode [Size]
        ,qq.Cutpart [Cutpart]
        ,'('+d.Patterncode+')' [Patterncode]
        ,d.PatternDesc [Description]
        --,SubProcess.SubProcess [SubProcess]
        ,[SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
        ,d.Parts [Parts]
        ,a.Qty [Qty]
        ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
        ,c.FactoryID  [left]
        ,e.MarkerNo
    from dbo.Bundle_Detail a WITH (NOLOCK)
    left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
    left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
    left join dbo.Bundle_Detail_Allpart d WITH (NOLOCK) on d.id=a.Id
    left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef<>'' and b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
    outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',d.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
    outer apply
    (
        select SubProcess = 
        (
            select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
            from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
            where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= qq.Cutpart
            for xml path('')
        )
    )as SubProcess
    " + sqlWhere + @" and a.Patterncode = 'ALLPARTS' "
+ @"
)x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m
		inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu
" + sb);                
                #endregion
            }
            else  //沒勾[Extend All Parts]
            {
                #region SQL
                sqlcmd = string.Format(@"
select *
from(
    select 
        Convert(bit,0) as selected
        ,a.BundleNo [Bundle]
        ,b.CutRef [CutRef]
        ,b.POID [POID]
        ,b.Orderid [SP]
        ,a.BundleGroup [Group]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,c.StyleID [Style]
        ,b.Item [Item]
        ,b.PatternPanel [Comb]
        ,b.cutno [Cut]
        ,b.Article [Article]
        ,b.Colorid [Color]
        ,b.Article + '\' + b.Colorid [Color2]
        ,a.SizeCode [Size]
        ,a.PatternCode [Cutpart]
        ,'('+a.Patterncode+')' [Patterncode]
        ,a.PatternDesc [Description]
        --,SubProcess.SubProcess [SubProcess]
        ,[SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
        ,a.Parts [Parts]
        ,a.Qty [Qty]
        ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
        ,c.FactoryID  [left]
        ,e.MarkerNo
    from dbo.Bundle_Detail a WITH (NOLOCK)
    left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
    left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
    left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef<>'' and b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
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
    " + sqlWhere + @" and a.Patterncode != 'ALLPARTS' 
                                        
    union all

    select DISTINCT 
        Convert(bit,0) as selected
        ,a.BundleNo [Bundle]
        ,b.CutRef [CutRef]
        ,b.POID [POID]
        ,b.Orderid [SP]
        ,a.BundleGroup [Group]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,c.StyleID [Style]
        ,b.Item [Item]
        ,b.PatternPanel [Comb]
        ,b.cutno [Cut]
        ,b.Article [Article]
        ,b.Colorid [Color]
        ,b.Article + '\' + b.Colorid [Color2]
        ,a.SizeCode [Size]
        ,a.PatternCode [Cutpart]
        ,'('+a.Patterncode+')' [Patterncode]
        ,a.PatternDesc [Description]
        --,SubProcess.SubProcess [SubProcess]
        ,[SubProcess]= IIF(len(SubProcess.SubProcess)>43,substring(SubProcess.SubProcess,0,43),SubProcess.SubProcess)
        ,a.Parts [Parts]
        ,a.Qty [Qty]
        ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
        ,c.FactoryID  [left]
        ,e.MarkerNo
    from dbo.Bundle_Detail a WITH (NOLOCK)
    left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
    left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
    left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef<>'' and b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
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
    " + sqlWhere + @" and a.Patterncode = 'ALLPARTS' " 
+ @"
)x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m
		inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu

" + sb);  
                #endregion
            }

            result = DBProxy.Current.Select("", sqlcmd, lis, out dtt);
            if (!result)
            {
                ShowErr(sqlcmd, result);
                this.HideWaitMessage();
                return;
            }
            if (dtt.Rows.Count == 0)  MyUtility.Msg.WarningBox("Data not found!!"); 
            listControlBindingSource1.DataSource = dtt;
            
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

            DataTable dt1, dt2, dt3,dtSelect;
            //int count =dt.Rows.Count;
            int count = 1;
            dt1 = dtt.Clone();
            dt2 = dtt.Clone();
            dt3 = dtt.Clone();
            dtSelect = dtt.AsEnumerable()
                .Where(row => (bool)row["selected"]).CopyToDataTable();
            foreach (DataRow dr in dtSelect.Rows)
            {
                //第一列資料
                if (count % 3 == 1)
                {
                    dt1.ImportRow(dr);
                }
                //第二列資料
                if (count % 3 == 2)
                {
                    dt2.ImportRow(dr);
                }
                //第三列資料
                if (count % 3 == 0)
                {
                    dt3.ImportRow(dr);
                }
                count++;
            }


            //傳 list 資料            
            var res = dt1.AsEnumerable()
                .Where(row => (bool)row["selected"])
            .Select(row1 => new P12_PrintData()
            {
                Group_right = row1["Group"].ToString(),
                Group_left = row1["left"].ToString(),
                Line = row1["Line"].ToString(),
                Cell = row1["Cell"].ToString(),
                SP = row1["SP"].ToString(),
                Style = row1["Style"].ToString(),
                MarkerNo = row1["MarkerNo"].ToString(),
                Item = row1["Item"].ToString(),
                Body_Cut = row1["Body_Cut"].ToString(),
                Parts = row1["Parts"].ToString(),
                Color = row1["Color2"].ToString(),
                Size = row1["Size"].ToString(),
                SizeSpec = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? "" : "(" + row1["SizeSpec"].ToString() + ")",
                Patterncode = row1["Patterncode"].ToString(),
                Desc = row1["Description"].ToString(),
                SubProcess = row1["SubProcess"].ToString(),
                Qty = row1["Qty"].ToString(),
                Barcode = row1["Bundle"].ToString()
            }).ToList();

             res.AddRange( dt2.AsEnumerable()
               .Where(row => (bool)row["selected"])
           .Select(row1 => new P12_PrintData()
           {
               Group_right2 = row1["Group"].ToString(),
               Group_left2 = row1["left"].ToString(),
               Line2 = row1["Line"].ToString(),
               Cell2 = row1["Cell"].ToString(),
               SP2 = row1["SP"].ToString(),
               Style2 = row1["Style"].ToString(),
               MarkerNo2 = row1["MarkerNo"].ToString(),
               Item2 = row1["Item"].ToString(),
               Body_Cut2 = row1["Body_Cut"].ToString(),
               Parts2 = row1["Parts"].ToString(),
               Color2 = row1["Color2"].ToString(),
               Size2 = row1["Size"].ToString(),
               SizeSpec2 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? "" : "(" + row1["SizeSpec"].ToString() + ")",
               Patterncode2 = row1["Patterncode"].ToString(),
               Desc2 = row1["Description"].ToString(),
               SubProcess2 = row1["SubProcess"].ToString(),
               Qty2 = row1["Qty"].ToString(),
               Barcode2 = row1["Bundle"].ToString()
           }).ToList());

             res.AddRange( dt3.AsEnumerable()
               .Where(row => (bool)row["selected"])
           .Select(row1 => new P12_PrintData()
           {
               Group_right3 = row1["Group"].ToString(),
               Group_left3 = row1["left"].ToString(),
               Line3 = row1["Line"].ToString(),
               Cell3 = row1["Cell"].ToString(),
               SP3 = row1["SP"].ToString(),
               Style3 = row1["Style"].ToString(),
               MarkerNo3 = row1["MarkerNo"].ToString(),
               Item3 = row1["Item"].ToString(),
               Body_Cut3 = row1["Body_Cut"].ToString(),
               Parts3 = row1["Parts"].ToString(),
               Color3 = row1["Color2"].ToString(),
               Size3 = row1["Size"].ToString(),
               SizeSpec3 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? "" : "(" + row1["SizeSpec"].ToString() + ")",
               Patterncode3 = row1["Patterncode"].ToString(),
               Desc3 = row1["Description"].ToString(),
               SubProcess3 = row1["SubProcess"].ToString(),
               Qty3 = row1["Qty"].ToString(),
               Barcode3 = row1["Bundle"].ToString()
           }).ToList());



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
                ups.Append(string.Format(@"
                            update b
                            set b.PrintDate = GETDATE()
                            from Bundle b
                            inner join Bundle_Detail bd on b.id=bd.ID
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
