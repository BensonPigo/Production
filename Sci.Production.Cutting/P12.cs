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

namespace Sci.Production.Cutting
{
    public partial class P12 : Sci.Win.Tems.QueryForm
    {
       public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            //this.GridSetup();  
            
        }
       string Cut_Ref;
       string Cut_Ref1;
       string SP;
       string SP1;
       string POID ;
       string Bundle;
       string Bundle1;
       DateTime? Est_CutDate;
       string Cell ;
       string size;
       string Sort_by;
       string Extend;
        
       private void button1_Click(object sender, EventArgs e)
       {
           if (!this.textBox1.Text.Empty()&&textBox2.Text.Empty()&&textBox4.Text.Empty()&&textBox3.Text.Empty()&&textBox7.Text.Empty()&&textBox6.Text.Empty()
               &&textBox5.Text.Empty()&&dateBox1.Value.Empty())
           {
               MyUtility.Msg.ErrorBox("[Cut_Ref# and SP# and POID and Bundle# and Est.Cut Date] can not is null");
               textBox1.Focus();
               textBox2.Focus();
               textBox4.Focus();
               textBox3.Focus();
               textBox7.Focus();
               textBox6.Focus();
               textBox5.Focus();
               dateBox1.Focus();
               return;
           }
           Cut_Ref = textBox1.Text.ToString();
           Cut_Ref1 = textBox2.Text.ToString();
           SP=textBox4.Text.ToString();
           SP1 = textBox3.Text.ToString();
           POID = textBox7.Text.ToString();
           Bundle=textBox6.Text.ToString();
           Bundle1 = textBox5.Text.ToString();
           Est_CutDate = dateBox1.Value;
           Cell = textBox10.Text.ToString();
           size = textBox9.Text.ToString();
           Sort_by = comboBox1.SelectedIndex.ToString();
           Extend=checkBox1.Checked.ToString();


           List<SqlParameter> lis = new List<SqlParameter>();
           string sqlWhere = "";  string sb=""; string ff="";
           List<string> sqlWheres = new List<string>();
           
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
               lis.Add(new SqlParameter("@Size", Size));
           }

           if (!this.dateBox1.Value.Empty())
           {
               sqlWheres.Add("e.EstCutDate=@Est_CutDate");
               lis.Add(new SqlParameter("@Est_CutDate", Est_CutDate));
           }

          if(this.comboBox1.Text=="Bundle#")
          {
             sb="order by a.BundleNo, b.OrderID, b.PatternPanel, b.Article,a.SizeCode";
          }
          else if(this.comboBox1.Text=="SP#")
          {
          sb=" order by b.OrderID,b.CutRef,b.PatternPanel,b.Article,a.SizeCode";
          }
           
          ff="b.MDivisionID = Sci.Env.User.Keyword";
            
          sqlWhere = string.Join(" and ", sqlWheres);
          if (!sqlWhere.Empty())
          {
              sqlWhere = " where " + sqlWhere;
          }
          DualResult result;
          if (checkBox1.Checked)
              lis.Add(new SqlParameter("@extend", "1"));
          else
              lis.Add(new SqlParameter("@extend", "0"));

          string sqlcmd = string.Format(@"select a.BundleNo [Bundle]
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
                                                ,a.SizeCode [Size]
                                                ,qq.Cutpart [Cutpart]
                                                ,[Description]=iif(a.Patterncode = 'ALLPARTS',iif(@extend='1',d.PatternDesc,a.PatternDesc),a.PatternDesc)
                                                ,SubProcess.SubProcess [SubProcess]
                                                ,[Parts]=iif(a.Patterncode = 'ALLPARTS',iif(@extend='1',d.Parts,a.Parts),a.Parts)
                                                ,a.Qty [Qty]
                                                from dbo.Bundle_Detail a
                                                left join dbo.bundle b on a.id=b.ID
                                                left join dbo.Orders c on c.id=b.Orderid
                                                left join dbo.Bundle_Detail_Allpart d on d.id=a.id and d.BundleNo=a.BundleNo
                                                outer apply(select iif(a1.PatternCode = 'ALLPARTS',iif(@extend='1',d1.PatternCode,a1.PatternCode),a1.PatternCode) [Cutpart]
                                                             from dbo.Bundle_Detail a1 
                                                             left join dbo.Bundle_Detail_Allpart d1 on d1.id=a1.Id and d1.BundleNo=a1.BundleNo 
			                                                 where a1.Id = a.ID and d1.BundleNo = d.BundleNo and d1.Patterncode = d.Patterncode)[qq]
                                                outer apply(select SubProcess = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                     from dbo.Bundle_Detail_Art e1
							                                                     where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= qq.Cutpart
							                                                     for xml path('')))as SubProcess " + ff + " " + sqlWhere + " " + sb);
          result = DBProxy.Current.Select("", sqlcmd, lis, out dtt);
            

       }
       DataTable dtt;

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            pars = new List<SqlParameter>();
           
            DataTable dd;
            DualResult result = DBProxy.Current.Select("",
            @"select a.BundleGroup [Group_right]
                    ,a.BundleNo [Group_left]
                    ,b.Sewinglineid [Line]
                    ,b.SewingCell [Cell]
                    ,b.Orderid [SP]
                    ,c.StyleID [Style]
                    ,b.Item [Item]
                    ,b.PatternPanel+''+ b.cutno [Body_Cut]
                    ,[Parts]=iif(a.Patterncode = 'ALLPARTS',iif(@extend='1',d.Parts,a.Parts),a.Parts)
                    ,b.Colorid [Color]
                    ,a.SizeCode [Size]
                    ,[Desc]=iif(a.Patterncode = 'ALLPARTS',iif(@extend='1',d.PatternDesc,a.PatternDesc),a.PatternDesc)
                    ,SubProcess.SubProcess [SubProcess]
                    ,a.Qty [Qty]
                    ,a.BundleNo []
                    from dbo.Bundle_Detail a
                    left join dbo.bundle b on a.id=b.ID
                    left join dbo.Orders c on c.id=b.Orderid
                    left join dbo.Bundle_Detail_Allpart d on d.id=a.id and d.BundleNo=a.BundleNo
                    outer apply(select iif(a1.PatternCode = 'ALLPARTS',iif(@extend='1',d1.PatternCode,a1.PatternCode),a1.PatternCode) [Cutpart]
                                 from dbo.Bundle_Detail a1 
                                 left join dbo.Bundle_Detail_Allpart d1 on d1.id=a1.Id and d1.BundleNo=a1.BundleNo 
			                     where a1.Id = a.ID and d1.BundleNo = d.BundleNo and d1.Patterncode = d.Patterncode)[qq]
                    outer apply(select SubProcess = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                         from dbo.Bundle_Detail_Art e1
							                         where e1.id=b.id and e1.Bundleno=a.BundleNo and e1.PatternCode= qq.Cutpart
							                         for xml path('')))as SubProcess", pars, out dd);
            if (!result) { this.ShowErr(result); }
           ReportDefinition report = new ReportDefinition();
            // 傳 list 資料            
            List<P12_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P12_PrintData()
                {
                    Group_right = row1["Group_right"].ToString(),
                    Group_left = row1["Group_left"].ToString(),
                    Line = row1["Line"].ToString(),
                    Cell = row1["Cell"].ToString(),
                    SP = row1["SP"].ToString(),
                    Style = row1["Style"].ToString(),
                    Item = row1["Item"].ToString(),
                    Body_Cut = row1["Body_Cut"].ToString(),
                    Parts = row1["Parts"].ToString(),
                    Color = row1["Color"].ToString(),
                    Size = row1["Size"].ToString(),
                    Desc = row1["Desc"].ToString(),
                    SubProcess = row1["SubProcess"].ToString(),
                    Qty = row1["Qty"].ToString(),
                    Barcode = row1["Barcode"].ToString()
                }).ToList();

            report.ReportDataSource = data;
        }
    

       //void GridSetup()
       //{
       //    this.grid1.IsEditingReadOnly = false;
       //    Helper.Controls.Grid.Generator(this.grid1)
       //        .CheckBox("selected", header: "Sele", width: Widths.AnsiChars(14), iseditable: true, trueValue: true, falseValue: false)
       //        .Text("POID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
       //        .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(4), iseditingreadonly: true)
       //        .Text("Roll", header: "Roll#", width: Widths.AnsiChars(5), iseditingreadonly: true)
       //        .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(9), iseditingreadonly: true)
       //        ;

       //}

        //protected override bool ValidateInput()
        //{
        //    return base.ValidateInput();
        //}
       
    }
}
