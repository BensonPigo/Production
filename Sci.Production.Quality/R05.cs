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

namespace Sci.Production.Quality
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {       DateTime? DateSCIStart; DateTime? DateSCIEnd;
            List<SqlParameter> lis; DualResult res;
            DataTable dt; string cmd, str_Category, str_Material, ReportType; 
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
           
            InitializeComponent();
            DataTable Cartegory = null;
            string sqlC = (@" 
                        select
                             Category=name
                        from  dbo.DropDownList
                        where type = 'Category' and id != 'O'  AND id != 'M'
                        ");
            DBProxy.Current.Select("", sqlC, out Cartegory);
            Cartegory.Rows.Add(new string[] { "" });
            Cartegory.DefaultView.Sort = "Category";
            this.comboCategory.DataSource = Cartegory;
            this.comboCategory.ValueMember = "Category";
            this.comboCategory.DisplayMember = "Category";
            this.comboCategory.SelectedIndex = 1;

            DataTable Material = null;
            string sqlM = (@" 
                        SELECT distinct case fabrictype
                               when 'F' then 'Fabric' 
	                           when 'A' then 'Accessory'
	                           end fabrictype
                        FROM Po_supp_detail 
                        where fabrictype !='O'  AND fabrictype !=''
                        ");
            DBProxy.Current.Select("", sqlM, out Material);
            Material.Rows.Add(new string[] { "" });
            Material.DefaultView.Sort = "fabrictype";
            this.comboMaterialType.DataSource = Material;
            this.comboMaterialType.ValueMember = "fabrictype";
            this.comboMaterialType.DisplayMember = "fabrictype";
            this.comboMaterialType.SelectedIndex = 2;
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {

            bool dateSciDelivery_Empty = !this.DateSCIDelivery.HasValue, comboCategory_Empty = this.comboCategory.Text.Empty(), comboMaterial_Empty = this.comboMaterialType.Empty(),
                 report_Empty = this.radioPanel.Value.Empty();
            if (dateSciDelivery_Empty)
            {
                MyUtility.Msg.ErrorBox("Please entry the 'SCI Delivery'");
                DateSCIDelivery.Focus();
                return false;
            }
            DateSCIStart = DateSCIDelivery.Value1;
            DateSCIEnd = DateSCIDelivery.Value2;
            str_Category = comboCategory.Text;
            str_Material = comboMaterialType.Text;
            ReportType = radioPanel.Value.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = "";

            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.DateSCIDelivery.Value1.Empty() && !this.DateSCIDelivery.Value2.Empty())
            {
                sqlWheres.Add("SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            }/* if (!this.DateSewInLine.Value1.Empty() && !this.DateSewInLine.Value2.Empty())
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
            }*/
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
            return base.OnAsyncDataLoad(e);
        }
    }
}
