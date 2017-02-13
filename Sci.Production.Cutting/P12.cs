using Ict;
using Ict.Win;
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
            this.comboBox1.SelectedIndex = 0;
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
                .Text("Cutpart", header: "Cutpart Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SubProcess", header: "Artwork", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Parts", header: "Parts", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(8), iseditingreadonly: true)
                ;

        }



        private void button1_Click(object sender, EventArgs e)
        {

            if (this.textBox1.Text.Empty() && this.textBox2.Text.Empty() && this.textBox4.Text.Empty() && this.textBox3.Text.Empty() && this.textBox7.Text.Empty() && this.textBox6.Text.Empty()
                && this.textBox5.Text.Empty() && this.dateBox1.Value.Empty())
            {
                MyUtility.Msg.ErrorBox("[Cut_Ref# and SP# and POID and Bundle# and Est.Cut Date] can not be all null !!");
                textBox1.Focus();
                return;
            }


            Cut_Ref = textBox1.Text.ToString();
            Cut_Ref1 = textBox2.Text.ToString();
            SP = textBox4.Text.ToString();
            SP1 = textBox3.Text.ToString();
            POID = textBox7.Text.ToString();
            Bundle = textBox6.Text.ToString();
            Bundle1 = textBox5.Text.ToString();
            Est_CutDate = dateBox1.Value;
            Cell = textBox10.Text.ToString();
            size = textBox9.Text.ToString();
            Sort_by = comboBox1.SelectedIndex.ToString();
            Extend = checkBox1.Checked.ToString();


            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string sb = "";
            List<string> sqlWheres = new List<string>();

            sqlWheres.Add("b.MDivisionID=@Factory");
            lis.Add(new SqlParameter("@Factory", Sci.Env.User.Factory));


            if (!this.textBox1.Text.Empty() && !this.textBox2.Text.Empty())
            {
                sqlWheres.Add("b.CutRef between @Cut_Ref and @Cut_Ref1");
                lis.Add(new SqlParameter("@Cut_Ref", Cut_Ref));
                lis.Add(new SqlParameter("@Cut_Ref1", Cut_Ref1));
            }
            if (!this.textBox4.Text.Empty() && !this.textBox3.Text.Empty())
            {
                sqlWheres.Add("b.OrderID  between @SP and @SP1");
                lis.Add(new SqlParameter("@SP", SP));
                lis.Add(new SqlParameter("@SP1", SP1));
            }
            if (!this.textBox7.Text.Empty())
            {
                sqlWheres.Add("b.POID=@POID");
                lis.Add(new SqlParameter("@POID", POID));
            }
            if (!this.textBox6.Text.Empty() && !this.textBox5.Text.Empty())
            {
                sqlWheres.Add("a.BundleNo between @Bundle and @Bundle1");
                lis.Add(new SqlParameter("@Bundle", Bundle));
                lis.Add(new SqlParameter("@Bundle1", Bundle1));
            }
            if (!this.textBox10.Text.Empty())
            {
                sqlWheres.Add("b.SewingCell =@Cell");
                lis.Add(new SqlParameter("@Cell", Cell));
            }
            if (!this.textBox9.Text.Empty())
            {
                sqlWheres.Add("a.SizeCode  =@Size");
                lis.Add(new SqlParameter("@Size", size));
            }

            if (!this.dateBox1.Value.Empty())
            {
                sqlWheres.Add("e.EstCutDate=@Est_CutDate");
                lis.Add(new SqlParameter("@Est_CutDate", Est_CutDate));
            }

            if (this.comboBox1.Text == "Bundle#")
            {
                sb = "order by a.BundleNo,b.OrderID,b.PatternPanel,b.Article,a.SizeCode";
            }
            else if (this.comboBox1.Text == "SP#")
            {
                sb = "order by b.OrderID,b.CutRef,b.PatternPanel,b.Article,a.SizeCode";
            }


            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            if (checkBox1.Checked)
                lis.Add(new SqlParameter("@extend", "1"));
            else
                lis.Add(new SqlParameter("@extend", "0"));

            string sqlcmd = string.Empty;
            if (checkBox1.Checked)  //有勾[Extend All Parts]
            {
                #region SQL
                sqlcmd = string.Format(@"select Convert(bit,0) as selected
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
                                                ,'('+a.Patterncode+')' [Patterncode]
                                                ,a.PatternDesc [Description]
                                                ,SubProcess.SubProcess [SubProcess]
                                                ,a.Parts [Parts]
                                                ,a.Qty [Qty]
                                                ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
                                                ,b.MDivisionid [left]
                                        from dbo.Bundle_Detail a WITH (NOLOCK)
                                        left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
                                        left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
                                        left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
                                        outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                        outer apply(select SubProcess = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
							                                                where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= qq.Cutpart
							                                                for xml path('')))as SubProcess " + sqlWhere + @" and a.Patterncode != 'ALLPARTS' 
                                        
                                        union all

                                        select Convert(bit,0) as selected
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
                                                ,SubProcess.SubProcess [SubProcess]
                                                ,d.Parts [Parts]
                                                ,a.Qty [Qty]
                                                ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
                                                ,b.MDivisionid [left]
                                        from dbo.Bundle_Detail a WITH (NOLOCK)
                                        left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
                                        left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
                                        left join dbo.Bundle_Detail_Allpart d WITH (NOLOCK) on d.id=a.Id
                                        left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
                                        outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',d.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                        outer apply(select SubProcess = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
							                                                where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= qq.Cutpart
							                                                for xml path('')))as SubProcess " + sqlWhere + @" and a.Patterncode = 'ALLPARTS' "
                                        + sb);



                #endregion
            }
            else  //沒勾[Extend All Parts]
            {
                #region SQL
                sqlcmd = string.Format(@"select Convert(bit,0) as selected
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
                                                 ,'('+a.Patterncode+')' [Patterncode]
                                                ,a.PatternDesc [Description]
                                                ,SubProcess.SubProcess [SubProcess]
                                                ,a.Parts [Parts]
                                                ,a.Qty [Qty]
                                                ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
                                                ,b.MDivisionid [left]
                                        from dbo.Bundle_Detail a WITH (NOLOCK)
                                        left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
                                        left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
                                        left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
                                        outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                        outer apply(select SubProcess = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
							                                                where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= qq.Cutpart
							                                                for xml path('')))as SubProcess " + sqlWhere + @" and a.Patterncode != 'ALLPARTS' 
                                        
                                        union all

                                        select Convert(bit,0) as selected
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
                                                ,'('+a.Patterncode+')' [Patterncode]
                                                ,a.PatternDesc [Description]
                                                ,SubProcess.SubProcess [SubProcess]
                                                ,a.Parts [Parts]
                                                ,a.Qty [Qty]
                                                ,b.PatternPanel +'-'+convert(varchar ,b.cutno) [Body_Cut]
                                                ,b.MDivisionid [left]
                                        from dbo.Bundle_Detail a WITH (NOLOCK)
                                        left join dbo.bundle b WITH (NOLOCK) on a.id=b.ID
                                        left join dbo.Orders c WITH (NOLOCK) on c.id=b.Orderid
                                        left join dbo.WorkOrder e WITH (NOLOCK) on b.CutRef=e.CutRef and e.MDivisionid=b.MDivisionid
                                        outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                        outer apply(select SubProcess = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
							                                                where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= qq.Cutpart
							                                                for xml path('')))as SubProcess " + sqlWhere + @" and a.Patterncode = 'ALLPARTS' "
                                        + sb);
                #endregion
            }

            result = DBProxy.Current.Select("", sqlcmd, lis, out dtt);
            if (!result)
            {
                ShowErr(sqlcmd, result);
                return;
            }
            if (dtt.Rows.Count == 0)  MyUtility.Msg.WarningBox("Data not found!!"); 
            listControlBindingSource1.DataSource = dtt;

        }



        private void button2_Click(object sender, EventArgs e)
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
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                grid1.Focus();
                return;
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars = new List<SqlParameter>();

            //傳 list 資料            
            var res = dtt.AsEnumerable()
                .Where(row => (bool)row["selected"])
            .Select(row1 => new P12_PrintData()
            {
                Group_right = row1["Group"].ToString(),
                Group_left = row1["left"].ToString(),
                Line = row1["Line"].ToString(),
                Cell = row1["Cell"].ToString(),
                SP = row1["SP"].ToString(),
                Style = row1["Style"].ToString(),
                Item = row1["Item"].ToString(),
                Body_Cut = row1["Body_Cut"].ToString(),
                Parts = row1["Parts"].ToString(),
                Color = row1["Color2"].ToString(),
                Size = row1["Size"].ToString(),
                Patterncode = row1["Patterncode"].ToString(),
                Desc = row1["Description"].ToString(),
                SubProcess = row1["SubProcess"].ToString(),
                Qty = row1["Qty"].ToString(),
                Barcode = row1["Bundle"].ToString()
            }).ToList();



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
            if(MdiParent!=null)  frm.MdiParent = MdiParent;
            frm.Show();

            return;

            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
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
                MyUtility.Msg.ErrorBox("Grid must be chose one");
                grid1.Focus();
                return;
            }
            DataTable selects = dtt.AsEnumerable()
                .Where(row => (bool)row["selected"])
                .CopyToDataTable();

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P12.xltx"); //預先開啟excel app                         
            MyUtility.Excel.CopyToXls(selects, "", "Cutting_P12.xltx", 1, true, "Bundle,CutRef,POID,SP,Group,Line,Cell,Style,Item,Comb,Cut,Article,Color,Size,Cutpart,Description,SubProcess,Parts,Qty", objApp);      // 將datatable copy to excel
            return;
            #endregion
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
