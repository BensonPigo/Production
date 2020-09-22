using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R03 : Win.Tems.PrintForm
    {
        private DateTime? DateSCIStart; private DateTime? DateSCIEnd;
        private DateTime? DateSewStart; private DateTime? DateSewEnd;
        private DateTime? DateEstStart; private DateTime? DateEstEnd;
        private string Category; private string Sea; private string Brand; private string Factory;
        private List<SqlParameter> lis; private DualResult res;
        private DataTable dt; private string cmd;

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            factory.Rows.Add(new string[] { string.Empty });
            factory.DefaultView.Sort = "FTYGroup";
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            bool date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue,
                txtSeason_Empty = this.txtSeason.Text.Empty(), txtBrand_Empty = this.txtBrand.Text.Empty(),  cate_comboBox_Empty = this.comboCategory.Text.Empty(), comboFactory_Empty = this.comboFactory.Text.Empty();
            if (date_SCI_Empty && date_Sewing_Empty && date_Est_Empty && txtSeason_Empty && txtBrand_Empty)
            {
                this.dateSCIDelivery.Focus();
                MyUtility.Msg.ErrorBox("Please select 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'Season' or 'Brand' at least one field entry");
                return false;
            }

            this.DateSCIStart = this.dateSCIDelivery.Value1;
            this.DateSCIEnd = this.dateSCIDelivery.Value2;
            this.DateSewStart = this.dateSewingInLineDate.Value1;
            this.DateSewEnd = this.dateSewingInLineDate.Value2;
            this.DateEstStart = this.dateEstCuttingDate.Value1;
            this.DateEstEnd = this.dateEstCuttingDate.Value2;
            this.Sea = this.txtSeason.Text;
            this.Brand = this.txtBrand.Text;
            this.Category = this.comboCategory.Text;
            this.Factory = this.comboFactory.Text;
            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;

            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.dateSCIDelivery.Value1.Empty())
            {
                sqlWheres.Add("SciDelivery >= @SCIDate1");
                this.lis.Add(new SqlParameter("@SCIDate1", this.DateSCIStart));
            }

            if (!this.dateSCIDelivery.Value2.Empty())
            {
                sqlWheres.Add("SciDelivery <= @SCIDate2");
                this.lis.Add(new SqlParameter("@SCIDate2", this.DateSCIEnd));
            }

            if (!this.dateSewingInLineDate.Value1.Empty())
            {
                sqlWheres.Add("SewInLine >= @SewDate1");
                this.lis.Add(new SqlParameter("@SewDate1", this.DateSewStart));
            }

            if (!this.dateSewingInLineDate.Value2.Empty())
            {
                sqlWheres.Add("SewInLine <= @SewDate2");
                this.lis.Add(new SqlParameter("@SewDate2", this.DateSewEnd));
            }

            if (!this.dateEstCuttingDate.Value1.Empty())
            {
                sqlWheres.Add("CutInLine >= @Est1");
                this.lis.Add(new SqlParameter("@Est1", this.DateEstStart));
            }

            if (!this.dateEstCuttingDate.Value2.Empty())
            {
                sqlWheres.Add("CutInLine <= @Est2");
                this.lis.Add(new SqlParameter("@Est2", this.DateEstEnd));
            }

            if (!this.txtSeason.Text.Empty())
            {
                sqlWheres.Add("SeasonID = @Sea");
                this.lis.Add(new SqlParameter("@Sea", this.Sea));
            }

            if (!this.txtBrand.Text.Empty())
            {
                sqlWheres.Add("BrandID = @Brand");
                this.lis.Add(new SqlParameter("@Brand", this.Brand));
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                sqlWheres.Add($"Category in ({this.comboCategory.SelectedValue})");
            }

            if (!this.comboFactory.SelectedValue.Empty())
                {
                    sqlWheres.Add("Factoryid = @Factory");
                    this.lis.Add(new SqlParameter("@Factory", this.Factory));
                }
              #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " AND " + sqlWhere;
            }

            sqlWhere += this.chkIncludeCancelOrder.Checked ? string.Empty : " and orders.Junk = 0 ";
            #region --撈ListExcel資料--

            this.cmd = string.Format(@" 
            with rawdata as 
            (
            select distinct poid from dbo.orders WITH (NOLOCK) 
            where 1=1
           " + sqlWhere + @"
            ) 
            ,summary as
            (
            select 
[POID]=a.POID,
[total_cnt]=(select count(1) total_cnt from dbo.FIR WITH (NOLOCK) where poid = a.POID) ,
[insp_cnt]= (	select count(1) 
	from dbo.fir f WITH (NOLOCK) 
	where POID = a.poid 
	and (Result!='' or (f.Result='' and f.nonContinuity =1 and f.Nonphysical = 1 and f.nonShadebond = 1 and f.nonWeight     =1))),
[lab_cnt]= (select count(1) from dbo.FIR_Laboratory l WITH (NOLOCK) where POID = a.POID and (l.Result!='' or (l.Result=''       and l.nonCrocking=1 and l.nonHeat=1 and l.nonWash=1))),
[total_article_cnt]= (SELECT count(1) from (select distinct o.poid,q.Article from dbo.orders o WITH (NOLOCK) inner join         dbo.Order_Qty q WITH (NOLOCK) on q.id =o.id where o.poid = a.poid) t) 
,[oven_cnt]= (SELECT count(1) from dbo.oven WITH (NOLOCK) where poid=a.POID) 
,[ColorFastness_cnt]= (select count(1) from dbo.ColorFastness WITH (NOLOCK) where poid=a.POID) 
,[AIR_Total_cnt]= (select count(1) from dbo.AIR WITH (NOLOCK) where poid = a.POID) 
,[AIR_Insp_Cnt]= (select count(1) from dbo.AIR WITH (NOLOCK) where poid = a.POID and air.Status = 'Confirmed' and Result!       ='') 
,[AIR_Laboratory_Total_cnt]= (select count(1) from dbo.AIR_Laboratory WITH (NOLOCK) where poid = a.POID) 
,[AIR_Laboratory_cnt]= (select count(1) from dbo.AIR_Laboratory al WITH (NOLOCK) where poid = a.POID and (result !='' or        (al.NonOven =1 and al.NonWash = 1))) 
             from dbo.FIR a WITH (NOLOCK) inner join rawdata r on r.POID = a.POID
            )
            select distinct O.POID 
            ,[Cancel] = IIF(o.Junk=1,'Y','N')
            ,o.FactoryID
            ,o.BrandID
            ,o.StyleID
            ,o.SeasonID
            ,(select min(orders.CutInLine) from orders WITH (NOLOCK) where poid = s.POID and orders.Junk = 0) first_cutinline
            ,getsci.MinSciDelivery
            ,IIF((select min(orders.CutInLine) from orders WITH (NOLOCK) where poid = s.POID and orders.Junk = 0) > DATEADD(day,(select 0-system.MtlLeadTime from dbo.system WITH (NOLOCK) ),getsci.MinSciDelivery),DATEADD(day,(select 0-system.MtlLeadTime from dbo.system WITH (NOLOCK) ),getsci.MinSciDelivery),(select min(orders.CutInLine) from orders WITH (NOLOCK) where poid = s.POID and orders.Junk = 0)) [Target Leadtime]
            ,iif(s.total_cnt!=0, round(s.insp_cnt*1.0/s.total_cnt,4) * 100 ,0) [Insp %]
            ,iif(s.total_cnt!=0, round(s.lab_cnt*1.0/s.total_cnt,4) * 100 ,0) [Lab %]
            ,iif(s.total_article_cnt!=0, round(s.oven_cnt*1.0/s.total_article_cnt,4) * 100 ,0) [Oven %]
            ,iif(s.total_article_cnt!=0, round(s.ColorFastness_cnt*1.0/s.total_article_cnt,4) * 100 ,0) [ColorFastness %]
            ,iif(s.AIR_Total_cnt!=0, round(s.AIR_Insp_Cnt*1.0/s.AIR_Total_cnt,4) * 100 ,0) [AIR Insp %]
            ,iif(s.AIR_Laboratory_Total_cnt!=0, round(s.AIR_Laboratory_cnt*1.0/s.AIR_Laboratory_Total_cnt,4) * 100 ,0) [AIR Lab %]
            from summary s 
            inner join orders o WITH (NOLOCK) on o.ID = s.POID
            cross apply dbo.GetSCI(s.poid,o.Category) getsci");
            #endregion

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
            if (!this.res)
            {
                return this.res;
            }

            return this.res;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_R03.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Quality_R03.xltx", 2, true, null, objApp);      // 將datatable copy to excel
            return this.res;
        }
    }
}
