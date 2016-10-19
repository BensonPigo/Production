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
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateArrStart; DateTime? DateArrEnd;
        DateTime? DateSCIStart; DateTime? DateSCIEnd;
        DateTime? DateSewStart; DateTime? DateSewEnd;
        DateTime? DateEstStart; DateTime? DateEstEnd;
        string spStrat; string spEnd; string Season; string Brand; string RefNo; string Category; string Supp;
        string MaterialType, Factory;
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable OrdersCategory = null;
            string sqlm = (@" 
                        select
                             Category=name
                        from  dbo.DropDownList
                        where type = 'Category' and id != 'O'
                        ");
            DBProxy.Current.Select("", sqlm, out OrdersCategory);
            OrdersCategory.DefaultView.Sort = "Category";
            this.comboCategory.DataSource = OrdersCategory;
            this.comboCategory.ValueMember = "Category";
            this.comboCategory.DisplayMember = "Category";
            this.comboCategory.SelectedIndex = 0;
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
            Material.DefaultView.Sort = "fabrictype";
            this.comboMaterialType.DataSource = Material;
            this.comboMaterialType.ValueMember = "fabrictype";
            this.comboMaterialType.DisplayMember = "fabrictype";
            this.comboMaterialType.SelectedIndex = 1;
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory order by FTYGroup", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool date_Arrive_Empty = !this.dateArrive.HasValue, date_SCI_Empty = !this.dateDelivery.HasValue, date_Sewing_Empty = !this.dateSewing.HasValue, date_Est_Empty = !this.dateCutting.HasValue,
               textBox_SP_Empty = this.txtSPnoStart.Text.Empty(), textBox_SP2_Empty = this.txtSPnoEnd.Text.Empty(), txtSEA_Empty = this.txtseason.Text.Empty()
          , txtBrand_Empty = this.txtbrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), Cate_comboBox_Empty = this.comboCategory.Text.Empty(), Supp_Empty = !this.txtsupplier.Text.Empty()
          , MaterialType_Empty = !this.comboMaterialType.Text.Empty(), Factory_Empty = !this.comboFactory.Text.Empty();
            if (date_Arrive_Empty && date_SCI_Empty && date_Sewing_Empty && date_Est_Empty && textBox_SP_Empty && textBox_SP2_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#'  at least one field entry");

                dateArrive.Focus();

                return false;
            }
            DateArrStart = dateArrive.Value1;
            DateArrEnd = dateArrive.Value2;
            DateSCIStart = dateDelivery.Value1;
            DateSCIEnd = dateDelivery.Value2;
            DateSewStart = dateSewing.Value1;
            DateSewEnd = dateSewing.Value2;
            DateEstStart = dateCutting.Value1;
            DateEstEnd = dateCutting.Value2;
            spStrat = txtSPnoStart.Text.ToString();
            spEnd = txtSPnoStart.Text.ToString();
            Season = txtseason.Text;
            Brand = txtbrand.Text;
            RefNo = txtRefno.Text.ToString();
            Category = comboCategory.Text;
            Supp = txtsupplier.Text;
            MaterialType = comboMaterialType.Text;
            Factory = comboFactory.Text;
            lis = new List<SqlParameter>();
            string sqlWhere = "", RWhere = "", OWhere = "";
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            List<string> OWheres = new List<string>();
            #region --組WHERE--

            if (!this.dateArrive.Value1.Empty() && !this.dateArrive.Value2.Empty())
            {
                RWheres.Add("R.WhseArrival between @ArrDate1 and @ArrDate2");
                lis.Add(new SqlParameter("@ArrDate1", DateArrStart));
                lis.Add(new SqlParameter("@ArrDate2", DateArrEnd));
            } if (!this.dateDelivery.Value1.Empty() && !this.dateDelivery.Value2.Empty())
            {
                OWheres.Add("O.SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            } if (!this.dateSewing.Value1.Empty() && !this.dateSewing.Value2.Empty())
            {
                OWheres.Add("O.SewInLine between @SewDate1 and @SewDate2");
                lis.Add(new SqlParameter("@SewDate1", DateSewStart));
                lis.Add(new SqlParameter("@SewDate2", DateSewEnd));
            } if (!this.dateCutting.Value1.Empty() && !this.dateCutting.Value2.Empty())
            {
                OWheres.Add("O.CutInLine between @Est1 and @Est2");
                lis.Add(new SqlParameter("@Est1", DateEstStart));
                lis.Add(new SqlParameter("@Est2", DateEstEnd));
            } if (!this.txtSPnoStart.Text.Empty())
            {
                OWheres.Add("O.Id between @sp1 and @sp2");
                lis.Add(new SqlParameter("@sp1", spStrat));
                lis.Add(new SqlParameter("@sp2", spEnd));
            } if (!this.txtseason.Text.Empty())
            {
                OWheres.Add("O.SeasonID = @Sea");
                lis.Add(new SqlParameter("@Sea", Season));
            } if (!this.txtbrand.Text.Empty())
            {
                OWheres.Add("O.BrandID = @Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            } if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("psd.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", RefNo));
            } if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                OWheres.Add("O.Category = @Cate");
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
            } if (!this.txtsupplier.Text.Empty())
            {
                sqlWheres.Add("ps.SuppId = @Supp");
                lis.Add(new SqlParameter("@Supp", Supp));
            } if (!this.comboMaterialType.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("psd.fabrictype = @MaterialType");
                if (MaterialType == "Accessory")
                {
                    lis.Add(new SqlParameter("@MaterialType", "A"));
                }
                if (MaterialType == "Fabric")
                {
                    lis.Add(new SqlParameter("@MaterialType", "F"));
                }
            } if (!this.comboFactory.SelectedItem.ToString().Empty())
            {
                OWheres.Add("O.factoryid = @Factory");
                lis.Add(new SqlParameter("@Factory", Factory));
            }

            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            RWhere = string.Join("", RWheres);
            OWhere = string.Join(" and ", OWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            if (!RWhere.Empty())
            {
                RWhere = " AND " + RWhere;
            }
            if (!OWhere.Empty())
            {
                OWhere = " AND " + OWhere;
            }
            #region --撈ListExcel資料--

            cmd = string.Format(@"
            select 
            Est.inspection
            ,[First].cutinline
            ,iif(Est.inspection < [First].cutinline,DATEDIFF(day,Est.inspection,getdate())-Getdate(),datediff(day,[First].cutinline,getdate())-Getdate())[Urgent Inspection]
            ,t.PoId
            ,t.seq1+'-'+t.seq2 [Seq]
            ,O.FactoryID
            ,O.BrandId
            , case PSD.fabrictype
              when 'F' then 'Fabric' 
              when 'A' then 'Accessory'
              end MaterialType
            ,O.StyleID
            ,O.SeasonId
            ,t.ExportId
            ,t.InvNo
            ,t.WhseArrival
            ,t.stockqty
            ,w.MinSciDelivery
            ,w.MinBuyerDelivery
            ,dbo.getMtlDesc(t.poid,t.seq1,t.seq2,1,0) [description]
            ,PSD.ColorID
            ,PS.SuppID
            ,Weave.WeaveTypeID
            ,t.id [ReceivingID]
            from (select r.WhseArrival,r.InvNo,r.ExportId,r.Id,rd.PoId,rd.seq1,rd.seq2,sum(stockqty) stockqty
			             from dbo.Receiving r
			            inner join dbo.Receiving_Detail rd on rd.Id = r.Id 
			            where r.type='A'"+RWhere+@"
			            group by r.WhseArrival,r.InvNo,r.ExportId,r.Id,rd.PoId,rd.seq1,rd.seq2) t
            inner join (select distinct poid,Category,KPILETA from dbo.Orders o 
			            where 1=1
			           "+OWhere+@" ) x on x.poid = T.POID
            inner join dbo.PO_Supp ps on ps.id = T.POID and ps.SEQ1 = T.SEQ1
            inner join dbo.PO_Supp_Detail psd on psd.ID = T.POID and psd.SEQ1 = T.SEQ1 and psd.SEQ2 = T.SEQ2
            outer apply dbo.getsci(t.poid,x.category) as w
            outer apply(select case when x.Category='M' then DATEADD(day,7,t.WhseArrival)
	            when Datediff(day,w.MinSciDelivery,x.kpileta) >=21 
		            then iif(x.KPILETA <= DATEADD(day,3,t.WhseArrival)
	            ,DATEADD(day,7,t.WhseArrival),x.KPILETA)
	            when Datediff(day,w.MinSciDelivery,x.kpileta) < 21 
		            then iif(DATEADD(day,-21,w.MinSciDelivery) <= DATEADD(day,3,t.WhseArrival)
	            ,DATEADD(day,7,t.WhseArrival),DATEADD(day,-21,w.MinSciDelivery))
            end  inspection)Est
            outer apply((select min(orders.CutInLine)cutinline from dbo.orders where orders.poid= t.PoId))[First] 
            outer apply(select o.factoryid,o.BrandId,o.StyleID,o.SeasonId from dbo.orders o where o.id = t.PoId)O
            outer apply(select f.WeaveTypeID from dbo.Fabric f where f.scirefno = psd.SCIRefno)Weave" + sqlWhere);
            #endregion
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

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R07.xltx");

            string d1 = (MyUtility.Check.Empty(DateArrStart)) ? "" : Convert.ToDateTime(DateArrStart).ToString("yyyy/MM/dd");
            string d2 = (MyUtility.Check.Empty(DateArrEnd)) ? "" : Convert.ToDateTime(DateArrEnd).ToString("yyyy/MM/dd");
            string d3 = (MyUtility.Check.Empty(DateSCIStart)) ? "" : Convert.ToDateTime(DateSCIStart).ToString("yyyy/MM/dd");
            string d4 = (MyUtility.Check.Empty(DateSCIEnd)) ? "" : Convert.ToDateTime(DateSCIEnd).ToString("yyyy/MM/dd");
            string d5 = (MyUtility.Check.Empty(DateSewStart)) ? "" : Convert.ToDateTime(DateSewStart).ToString("yyyy/MM/dd");
            string d6 = (MyUtility.Check.Empty(DateSewEnd)) ? "" : Convert.ToDateTime(DateSewEnd).ToString("yyyy/MM/dd");
            string d7 = (MyUtility.Check.Empty(DateEstStart)) ? "" : Convert.ToDateTime(DateEstStart).ToString("yyyy/MM/dd");
            string d8 = (MyUtility.Check.Empty(DateEstEnd)) ? "" : Convert.ToDateTime(DateEstEnd).ToString("yyyy/MM/dd");
            xl.dicDatas.Add("##ArrDate", d1 + "~" + d2);
            xl.dicDatas.Add("##SciDelivery", d3 + "~" + d4);
            xl.dicDatas.Add("##SewingDate", d5 + "~" + d6);
            xl.dicDatas.Add("##EstCutting", d7 + "~" + d8);
            xl.dicDatas.Add("##Spno", spStrat + "~" + spEnd);
            xl.dicDatas.Add("##Season", Season);
            xl.dicDatas.Add("##Brand", Brand);
            xl.dicDatas.Add("##Refno", RefNo);
            xl.dicDatas.Add("##Category", Category);
            xl.dicDatas.Add("##Supplier", Supp);
            xl.dicDatas.Add("##Material", MaterialType);
            xl.dicDatas.Add("##Factory", Factory);
            xl.dicDatas.Add("##body", dt);
            xl.Save(outpath, false);
            return true;
        }
    }
}
