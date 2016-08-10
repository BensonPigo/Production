using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        string sp1; string sp2; DateTime? EstBook1; DateTime? EstBook2; DateTime? EstArr1; DateTime? EstArr2; string fac; string M;
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory = null;
            string sqlcmd = (@"select DISTINCT FTYGroup FROM DBO.Factory");
            DBProxy.Current.Select("", sqlcmd, out factory);
            factory.Rows.Add(new string[] { "" });
            factory.DefaultView.Sort = "FTYGroup";
            this.comboBox1.DataSource = factory;
            this.comboBox1.ValueMember = "FTYGroup";
            this.comboBox1.DisplayMember = "FTYGroup";
            this.comboBox1.SelectedIndex = 0;
            this.comboBox1.Text = Sci.Env.User.Factory;

            DataTable m = null;
            string sqlm = (@"select ID FROM DBO.MDivision");
            DBProxy.Current.Select("", sqlm, out m);
            m.Rows.Add(new string[] { "" });
            m.DefaultView.Sort = "ID";
            this.comboBox2.DataSource = m;
            this.comboBox2.ValueMember = "ID";
            this.comboBox2.DisplayMember = "ID";
            this.comboBox2.SelectedIndex = 0;
            this.comboBox2.Text = Sci.Env.User.Keyword;
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            bool sp_Empty1 = !this.textBox1.Text.Empty(), sp_Empty2 = !this.textBox2.Text.Empty(), dateRange1_Empty = !this.dateRange_book.HasValue, dateRange2_Empty = !this.dateRange_Arr.HasValue;
            if (sp_Empty1 && sp_Empty2 && dateRange1_Empty && dateRange2_Empty)
            {
                MyUtility.Msg.ErrorBox("You must enter the SP No,Est.booking,Est.Arrived");

                textBox1.Focus();

                return false;
            }
            sp1 = textBox1.Text.ToString();
            sp2 = textBox2.Text.ToString();
            EstBook1 = dateRange_book.Value1;
            EstBook2 = dateRange_book.Value2;
            EstArr1 = dateRange_Arr.Value1;
            EstArr2 = dateRange_Arr.Value2;
            fac = comboBox1.SelectedItem.ToString();
            M = comboBox2.SelectedItem.ToString();
            lis = new List<SqlParameter>();
            string sqlWhere = ""; string order = "order by ThreadTypeID,td.ThreadColorID,t.StyleID,t.OrderID";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.textBox1.Text.Empty())
            {
                sqlWheres.Add("t.OrderID between @spNo1 and @spNo2");
                lis.Add(new SqlParameter("@spNo1", sp1));
                lis.Add(new SqlParameter("@spNo2", sp2));
            }
            if (!this.dateRange_book.Value1.Empty() && !this.dateRange_book.Value2.Empty())
            {
                sqlWheres.Add("t.EstBookDate between @EstBook1 and @EstBook2");
                lis.Add(new SqlParameter("@EstBook1", EstBook1));
                lis.Add(new SqlParameter("@EstBook2", EstBook2));
            } 
            if (!this.dateRange_Arr.Value1.Empty() && !this.dateRange_Arr.Value2.Empty())
            {
                sqlWheres.Add("t.EstArriveDate between @EstArr1 and @EstArr2");
                lis.Add(new SqlParameter("@EstArr1", EstArr1));
                lis.Add(new SqlParameter("@EstArr2", EstArr2));
            } 
            if (!this.comboBox1.Text.Empty())
            {
                sqlWheres.Add("t.FactoryID = @fac");
                lis.Add(new SqlParameter("@fac", fac));
                
            }
            if (!this.comboBox2.Text.Empty())
            {
                sqlWheres.Add("t.MDivisionID = @M");
                lis.Add(new SqlParameter("@M", M));
            }

            #endregion

            cmd = string.Format(@"
             select 
               t.FactoryID,t.BrandID,t.StyleID,t.SeasonID,t.EstBookDate,t.EstArriveDate,t.OrderID,o.SciDelivery,o.SewInLine
               ,(select li.ThreadTypeID from dbo.LocalItem li where li.RefNo = td.Refno) [ThreadTypeID]
               ,td.ConsumptionQty,td.Refno,td.ThreadColorID
               ,(select c.Description from dbo.ThreadColor c where c.id = td.ThreadColorID) [color_desc]
               ,td.PurchaseQty,td.TotalQty,td.AllowanceQty,td.UseStockQty
             from dbo.ThreadRequisition t
             inner join dbo.ThreadRequisition_Detail td on td.orderid = t.OrderID
             left join dbo.orders o on o.id = t.OrderID" + sqlWhere + ' ' + order);

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dt);
            if (!res)
            {
                return res;
            }
            return res;
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Thread_R20.xltx");
            xl.dicDatas.Add("##FAC", dt);
            xl.Save(outpath, false);
            return true;
        }
    }
    
}
