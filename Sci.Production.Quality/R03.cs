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
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateSCIStart; DateTime? DateSCIEnd;
        DateTime? DateSewStart; DateTime? DateSewEnd;
        DateTime? DateEstStart; DateTime? DateEstEnd;
        string Category; string Sea; string Brand; string Factory; 
        List<SqlParameter> lis; DualResult res;
        DataTable dt; string cmd;
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable ORS = null;
            string sqlm = (@" 
                        select
                             Category=name
                        from  dbo.DropDownList
                        where type = 'Category' and id != 'O'
                        ");
            DBProxy.Current.Select("", sqlm, out ORS);
            ORS.Rows.Add(new string[] { "" });
            ORS.DefaultView.Sort = "Category";
            this.comboCategory.DataSource = ORS;
            this.comboCategory.ValueMember = "Category";
            this.comboCategory.DisplayMember = "Category";
            this.comboCategory.SelectedIndex = 1;
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory order by FTYGroup", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool date_SCI_Empty = !this.DateSCIDelivery.HasValue, date_Sewing_Empty = !this.DateSewInLine.HasValue, date_Est_Empty = !this.DateEstCutting.HasValue,
                txtSeason_Empty = this.txtSeason.Text.Empty(), txtBrand_Empty = this.txtBrand.Text.Empty(),  Cate_comboBox_Empty = this.comboCategory.Text.Empty(),comboFactory_Empty = this.comboFactory.Text.Empty();
            if (date_SCI_Empty && date_Sewing_Empty && date_Est_Empty && txtSeason_Empty && txtBrand_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'Season' or 'Brand' at least one field entry");

                DateSCIDelivery.Focus();
                return false;
            }

            DateSCIStart = DateSCIDelivery.Value1;
            DateSCIEnd = DateSCIDelivery.Value2;
            DateSewStart = DateSewInLine.Value1;
            DateSewEnd = DateSewInLine.Value2;
            DateEstStart = DateEstCutting.Value1;
            DateEstEnd = DateEstCutting.Value2;
            Sea = txtSeason.Text;
            Brand = txtBrand.Text;
            Category = comboCategory.Text;
            Factory = comboFactory.Text;
            lis = new List<SqlParameter>();
            string sqlWhere = "";
           
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.DateSCIDelivery.Value1.Empty() && !this.DateSCIDelivery.Value2.Empty())
            {
                sqlWheres.Add("SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            } if (!this.DateSewInLine.Value1.Empty() && !this.DateSewInLine.Value2.Empty())
            {
                sqlWheres.Add("SewInLine between @SewDate1 and @SewDate2");
                lis.Add(new SqlParameter("@SewDate1", DateSewStart));
                lis.Add(new SqlParameter("@SewDate2", DateSewEnd));
            } if (!this.DateEstCutting.Value1.Empty() && !this.DateEstCutting.Value2.Empty())
            {
                sqlWheres.Add("CutInLine between @Est1 and @Est2");
                lis.Add(new SqlParameter("@Est1", DateEstStart));
                lis.Add(new SqlParameter("@Est2", DateEstEnd));
            } if (!this.txtSeason.Text.Empty())
            {
                sqlWheres.Add("SeasonID = @Sea");
                lis.Add(new SqlParameter("@Sea", Sea));
            } if (!this.txtBrand.Text.Empty())
            {
                sqlWheres.Add("BrandID = @Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            } if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("Category = @Cate");
                if (Category == "Bulk")
                {
                    lis.Add(new SqlParameter("@Cate", "B"));
                }
                if (Category == "Sample")
                {
                    lis.Add(new SqlParameter("@Cate", "S"));
                }
                if (Category == "Material")
                {
                    lis.Add(new SqlParameter("@Cate", "M"));
                }
            } if (!this.comboFactory.SelectedItem.ToString().Empty())
                {
                    sqlWheres.Add("Factoryid = @Factory");
                    lis.Add(new SqlParameter("@Factory", Factory));
                }
              #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " AND " + sqlWhere;
            }
            #region --撈ListExcel資料--

            cmd = string.Format(@" 
            with rawdata as 
            (
            select distinct poid from dbo.orders
            where 1=1
           " + sqlWhere + @"
            ) 
            ,summary as
            (
            select a.POID
            ,(select count(1) total_cnt from dbo.FIR where poid = a.POID) total_cnt
            ,(select count(1) from dbo.fir f
            where POID = a.poid and (Result!='' or (f.Result='' and f.nonContinuity =1 and f.Nonphysical = 1 and f.nonShadebond = 1 and f.nonWeight =1))
            ) insp_cnt
            ,(select count(1) from dbo.FIR_Laboratory l where POID = a.POID and (l.Result!='' or (l.Result='' and l.nonCrocking=1 and l.nonHeat=1 and l.nonWash=1))
            ) lab_cnt
            ,(SELECT count(1) from (select distinct o.poid,q.Article from dbo.orders o inner join dbo.Order_Qty q on q.id =o.id where o.poid = a.poid) t
            ) total_article_cnt
            ,(SELECT count(1) from dbo.oven where poid=a.POID) oven_cnt
            ,(select count(1) from dbo.ColorFastness where poid=a.POID) ColorFastness_cnt
            ,(select count(1) from dbo.AIR where poid = a.POID) AIR_Total_cnt
            ,(select count(1) from dbo.AIR where poid = a.POID and air.Status = 'Confirmed' and Result!='') AIR_Insp_Cnt
            ,(select count(1) from dbo.AIR_Laboratory where poid = a.POID) AIR_Laboratory_Total_cnt
            ,(select count(1) from dbo.AIR_Laboratory al where poid = a.POID and (result !='' or (al.NonOven =1 and al.NonWash = 1))) AIR_Laboratory_cnt
             from dbo.FIR a inner join rawdata r on r.POID = a.POID
            )
            select distinct O.POID 
            ,o.FactoryID
            ,o.BrandID
            ,o.StyleID
            ,o.SeasonID
            ,(select min(orders.CutInLine) from orders where poid = s.POID and orders.Junk = 0) first_cutinline
            ,getsci.MinSciDelivery
            ,IIF((select min(orders.CutInLine) from orders where poid = s.POID and orders.Junk = 0) > DATEADD(day,(select 0-system.MtlLeadTime from dbo.system),getsci.MinSciDelivery),DATEADD(day,(select 0-system.MtlLeadTime from dbo.system),getsci.MinSciDelivery),(select min(orders.CutInLine) from orders where poid = s.POID and orders.Junk = 0)) [Target Leadtime]
            ,iif(s.total_cnt!=0, round(s.insp_cnt*1.0/s.total_cnt,2) * 100 ,0) [Insp %]
            ,iif(s.total_cnt!=0, round(s.lab_cnt*1.0/s.total_cnt,2) * 100 ,0) [Lab %]
            ,iif(s.total_article_cnt!=0, round(s.oven_cnt*1.0/s.total_article_cnt,2) * 100 ,0) [Oven %]
            ,iif(s.total_article_cnt!=0, round(s.ColorFastness_cnt*1.0/s.total_article_cnt,2) * 100 ,0) [ColorFastness %]
            ,iif(s.AIR_Total_cnt!=0, round(s.AIR_Insp_Cnt*1.0/s.AIR_Total_cnt,2) * 100 ,0) [AIR Insp %]
            ,iif(s.AIR_Laboratory_Total_cnt!=0, round(s.AIR_Laboratory_cnt*1.0/s.AIR_Laboratory_Total_cnt,2) * 100 ,0) [AIR Lab %]
            from summary s inner join orders o on o.ID = s.POID
            cross apply dbo.GetSCI(s.poid,o.Category) getsci");
            #endregion

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
         
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
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R03.xltx"); //預先開啟excel app                         
            MyUtility.Excel.CopyToXls(dt, "", "Quality_R03.xltx", 2, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp

            return res;
        }
    }
}
