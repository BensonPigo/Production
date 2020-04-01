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
    public partial class R43 : Sci.Win.Tems.PrintForm
    {
        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            DataTable Year = null;
            string cmd = (@"
declare @y Table (M int);

declare @StartYear int = 2013;
declare @EndYear int = datepart(year, DateAdd (Month, -1, getDate()))

while (@StartYear <= @EndYear)
begin 
	insert into @y
	(M)
	values
	(@StartYear)

	set @StartYear = @StartYear + 1
end

select *
from @y
order by M desc");
            DBProxy.Current.Select("", cmd, out Year);
            
            this.comboyear.DataSource = Year;
            this.comboyear.ValueMember = "M";
            this.comboyear.DisplayMember = "M";

            if (Year != null
                && Year.Rows.Count > 0)
            {
                this.comboyear.SelectedIndex = 0;
            }

            //DataTable Month = null;
            //string scmd = (@"select  distinct month(startdate) as md from dbo.ADIDASComplain WITH (NOLOCK) ");
            //DBProxy.Current.Select("", scmd, out Month);
            //Month.DefaultView.Sort = "md";
            //this.comboMonth.DataSource = Month;
            //this.comboMonth.ValueMember = "md";
            //this.comboMonth.DisplayMember = "md";

            this.comboMonth.SelectedIndex = 0;
            this.combobrand.SelectedIndex = 0;
            print.Enabled = false;
          
        }
        string Brand;
        string Year;
        string Month;
        DataTable dt;

        protected override bool ValidateInput()
        {
            Brand = combobrand.Text.ToString();
            Year = comboyear.Text.ToString();
            Month = comboMonth.Text.ToString();
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = "";
            List<string> sqlWheres = new List<string>();

            if (Brand != "")
            {
                sqlWheres.Add(" b.BrandID =@Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            }
            if (Year != "") 
            {
                sqlWheres.Add("year(a.StartDate)=@Year");
                lis.Add(new SqlParameter("@Year", Year));
            }

            if (Month != "")
             {
                 sqlWheres.Add("MONTH(a.StartDate)=@Month");
                 lis.Add(new SqlParameter("@Month", Month));
             }
           
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where A.Junk=0 AND " + sqlWhere;
            }
            DualResult result;

            string sqlcmd = string.Format(@"select DISTINCT
                                                   a.AGCCode [Factory ID]
                                                  ,b.SeasonId [Season]
                                                  ,b.BulkMR [DevMR]
                                                  ,b.SampleMR [BulkMR]
                                                  ,b.SuppID [Supplier]
                                                  ,b.FactoryID [Factory]
                                                  ,b.Refno [Shell]
                                                  ,b.SalesName [Sales_Org_Name]
                                                  ,b.StyleID [Style]
                                                  ,b.Article [Article_ID]
                                                  ,b.ArticleName [ArticleName]
                                                  ,b.OrderID [SP]
                                                  ,b.CustPONo [PO]
                                                  ,b.ProductionDate [ProductionDate]
                                                  ,b.DefectMainID [DefectMainID]
                                                  ,b.DefectSubID [DefectSubID]
                                                  ,b.FOB [FOB_Price]
                                                  ,b.Qty [Qty]
                                                  ,b.ValueinUSD [Complaint_Value]
                                                  ,b.ValueINExRate [Exrate]
                                                  ,c.Name [Defect_Main_Name]
                                                  ,d.SubName [Defect_Sub_Name]
                                         from dbo.ADIDASComplain a WITH (NOLOCK) 
                                         inner join dbo.ADIDASComplain_Detail b WITH (NOLOCK) on a.id=b.ID
                                         left join dbo.ADIDASComplainDefect c WITH (NOLOCK) on b.DefectMainID=c.ID
                                         left join dbo.ADIDASComplainDefect_Detail d WITH (NOLOCK) on d.ID=b.DefectMainID and d.SubID=b.DefectSubID " + sqlWhere);
           result = DBProxy.Current.Select("", sqlcmd,lis, out dt);
            

            return result; //base.OnAsyncDataLoad(e);
            }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R43.xltx"); //預先開啟excel app                         
            MyUtility.Excel.CopyToXls(dt, "", "Quality_R43.xltx", 1, true, null, objApp);      // 將datatable copy to excel
            return true;
        }
    }
}
