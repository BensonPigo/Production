using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R40 : Sci.Win.Tems.PrintForm
    {
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            print.Enabled = false;
            this.comboBox_brand.SelectedIndex = 0;
        }
        string Brand;
        string Year;
        string Factory;
        DualResult result;
        DataTable dtt;

        protected override bool ValidateInput()
        {
            Brand = comboBox_brand.SelectedItem.ToString();
            Year = radiobtn_byYear.Checked.ToString();
            Factory = radiobtn_byfactory.Checked.ToString();
            return base.ValidateInput();
        }

     
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string gb = ""; string ob = "";
            List<string> sqlWheres = new List<string>();
            if (DateTime.Now.Month==01)
            {
                int year=DateTime.Today.Year;
                
                sqlWheres.Add("a.startdate between @year and @year2");
                lis.Add(new SqlParameter("@year", DateTime.Today.AddYears(-3).AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day)));
                lis.Add(new SqlParameter("@year1", DateTime.Today.AddYears(-2)));
                lis.Add(new SqlParameter("@year2", DateTime.Today.AddYears(-1)));

                DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
            }
            else
            {
                int year = DateTime.Today.Year;
                sqlWheres.Add("a.startdate between @year and @year2");
                lis.Add(new SqlParameter("@year", DateTime.Today.AddYears(-2)));
                lis.Add(new SqlParameter("@year1", DateTime.Today.AddYears(-1)));
                lis.Add(new SqlParameter("@year2", DateTime.Today.AddYears(-1+1)));
            }

            gb = "group by a.StartDate,c.Target";
            ob = "order by a.StartDate,c.Target";
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            //if (checkBox1.Checked)
            //    lis.Add(new SqlParameter("@extend", "1"));
            //else
            //    lis.Add(new SqlParameter("@extend", "0"));
//            DataTable dtStartDate;
//            string sqlcmd1 = string.Format(@"select distinct left(StartDate,4) 
//	                    from dbo.ADIDASComplain"+ sqlWhere);
//            string StartDate = "";
//            result = DBProxy.Current.Select("", sqlcmd1, lis, out dtStartDate);
//            foreach (DataRow dr in dtStartDate.Rows)
//            {
//                StartDate +=  dr["StartDate"].ToString() + ",";
//            }
//            StartDate = StartDate.Substring(0, StartDate.Length - 1);


            string sqlcmd = string.Format(@"SELECT  left(cast(a.StartDate as varchar(10)),4) [Y],format(a.StartDate,'MMM') [M],c.Target [Target], sum(b.Qty) [Claimed]
                                                     FROM 
                                                     DBO.ADIDASComplain a
                                                     inner JOIN DBO.ADIDASComplain_Detail B ON B.ID = a.ID
                                                     left join dbo.ADIDASComplainTarget c ON C.Year=left(a.StartDate,4)" + sqlWhere+" "+gb+" "+ob);
            result = DBProxy.Current.Select("", sqlcmd, lis, out dtt);


            return result; //base.OnAsyncDataLoad(e);
        }

   protected override bool OnToExcel(Win.ReportDefinition report)
        {
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            return base.OnToExcel(report);
        }

    }
}
