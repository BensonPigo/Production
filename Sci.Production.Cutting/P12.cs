using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P12 : Sci.Win.Tems.QueryForm
    {
       public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            //this.GridSetup();
           
            //this.comboBox1.Text = Sci.Env.User.Keyword;
            
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
       string Size;
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
           Size = textBox9.Text.ToString();
           Sort_by = comboBox1.SelectedIndex.ToString();
           Extend=checkBox1.Checked.ToString();


           List<SqlParameter> lis = new List<SqlParameter>();
           string sqlWhere = ""; string order = ""; string sb="";
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
                sb="a.BundleNo, b.OrderID, b.PatternPanel, b.Article,a.SizeCode";
          }
          else if(this.comboBox1.Text=="SP#")
          {
          sb="b.OrderID,b.CutRef,b.PatternPanel,b.Article,a.SizeCode";
          
          }


//           order = "order by a.CurrencyID,a.LocalSuppID,a.ID";

//           sqlWhere = string.Join(" and ", sqlWheres);
//           if (!sqlWhere.Empty())
//           {
//               sqlWhere = " where " + sqlWhere;
//           }
//           DualResult result;

//           string sqlcmd = string.Format(@"select a.LocalSuppID [Supplier]
//                                                  ,b.Abb [Supplier Abb]
//                                                  ,a.MDivisionID [M]
//                                                  ,a.FactoryID [Factory]
//                                                  ,a.CurrencyID [Currency]
//                                                  ,[Amount]=SUM(a.Amount) OVER (PARTITION BY a.localsuppid)
//                                                  ,c.Name [PayTerm]
//                                           from dbo.MiscAP a
//                                           left join Production.dbo.localsupp b on b.id=a.LocalSuppID
//                                           left join Production.dbo.PayTerm c on c.id=a.PayTermID " + sqlWhere + " " + order);
//           result = DBProxy.Current.Select("", sqlcmd, lis, out dtt);
            

       }
       DataTable dtt;

        private void button4_Click(object sender, EventArgs e)
        {
           //ThisForm.Release();
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
       
    }
}
