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
            str_Category = comboCategory.Text.ToString();
            str_Material = comboMaterialType.Text.ToString();
            ReportType = radioPanel.Value.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = ""; string sqlOrdersWhere = "";

            List<string> sqlWheres = new List<string>();
            List<string> sqlOrderWheres = new List<string>();
            #region --組WHERE--
            if (!this.DateSCIDelivery.Value1.Empty() && !this.DateSCIDelivery.Value2.Empty())
            {
                sqlOrderWheres.Add("SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            } if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                sqlOrderWheres.Add("Category = @Cate");
                if (str_Category == "Bulk")
                {
                    lis.Add(new SqlParameter("@Cate", "B"));
                }if (str_Category == "Sample")
                {
                    lis.Add(new SqlParameter("@Cate", "S"));
                }

            } if (!this.comboMaterialType.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("psd.FabricType=@FabricType");
                if (str_Material == "Fabric")
                {
                    lis.Add(new SqlParameter("@FabricType", "F"));
                } if (str_Material == "Accessory")
                {
                    lis.Add(new SqlParameter("@FabricType", "A"));
                }             
            } 
            #endregion
            sqlOrdersWhere = string.Join(" and ", sqlOrderWheres);
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " AND " + sqlWhere;
            } if (!sqlOrdersWhere.Empty())
            {
                sqlOrdersWhere = " AND " + sqlOrdersWhere;
            }
            #region --撈Fabric Detail 資料--

            cmd = string.Format(@" 
                with order_rawdata as
                (
	                select distinct poid from dbo.orders
	                where Junk =0 " + sqlOrdersWhere + @"
                )
                select 
                (select p.SuppID+'-'+s.AbbEN from dbo.PO_Supp p inner join dbo.Supp s on s.ID = p.SuppID
                where p.id = psd.ID and p.seq1 = psd.SEQ1 ) [Supplier]
                ,psd.Refno
                ,dbo.getMtlDesc(psd.id,psd.seq1,psd.seq2,1,0) [description]
                ,(select sum(dbo.getUnitRate(n.PoUnit,'YDS')*n.ShipQty) from dbo.Receiving m inner join dbo.Receiving_Detail n on n.Id = m.Id 
                where m.id = f.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2) [ShipQty]
                ,(select sum(dbo.getUnitRate(n.StockUnit,'YDS')*n.StockQty) from dbo.Receiving m inner join dbo.Receiving_Detail n on n.Id = m.Id 
                where m.id = f.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2) [ArriveQty]
                ,f.Physical
                ,(select WhseArrival from dbo.Receiving where id = f.ReceivingID)[WhseArrival]
                ,(select scidelivery from dbo.orders where id = a.POID)[scidelivery]
                ,iif(@category='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = f.ReceivingID),(select scidelivery from dbo.orders where id = a.POID))<25,'Y','')
		                ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = f.ReceivingID),(select scidelivery from dbo.orders where id = a.POID) )<15,'Y','')
		                ) [Delay]
                ,psd.ID
                ,psd.seq1+'-'+psd.seq2[SEQ#]
                ,(select ExportId from dbo.Receiving where id = f.ReceivingID)[ExportId]
                ,f.ReceivingID
                ,(select count(1) from dbo.FIR_Physical x 
	                inner join dbo.FIR_Physical_Defect y on y.FIR_PhysicalDetailUKey = x.DetailUkey 
	                where x.ID = f.ID) * 5 [defectYDS]
                ,f.TotalInspYds

                from order_rawdata a
                inner join dbo.PO_Supp_Detail psd on psd.ID = a.POID
                inner join FIR f on f.POID = psd.ID and f.SEQ1 = psd.Seq1 and f.seq2 = psd.Seq2
                where psd.SEQ1 NOT LIKE '5%'" + sqlWheres);
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

            if ("Detail".EqualString(this.radioDetail.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_FabricDetail.xltx");
              
            
                xl.dicDatas.Add("##BODY", dt);
                xl.Save(outpath, false);
            }
          /*  else if ("Detail List".EqualString(this.comboReport.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R37_DetailList.xltx");
                string d1 = (MyUtility.Check.Empty(DebDate1)) ? "" : Convert.ToDateTime(DebDate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(DebDate2)) ? "" : Convert.ToDateTime(DebDate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(ConDate1)) ? "" : Convert.ToDateTime(ConDate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(ConDate2)) ? "" : Convert.ToDateTime(ConDate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettDate1)) ? "" : Convert.ToDateTime(SettDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettDate2)) ? "" : Convert.ToDateTime(SettDate2).ToString("yyyy/MM/dd");
                xl.dicDatas.Add("##Debitdate", d1 + "~" + d2);
                xl.dicDatas.Add("##ConfirmDate", d3 + "~" + d4);
                xl.dicDatas.Add("##SettledDate", d5 + "~" + d6);
                xl.dicDatas.Add("##DebitN", DebitNo1 + "~" + DebitNo2);
                xl.dicDatas.Add("##Handle", handle);
                xl.dicDatas.Add("##SMR", smr);
                xl.dicDatas.Add("##Fac", fac);
                xl.dicDatas.Add("##Pay", Pay);
                xl.dicDatas.Add("##Deb", dt);
                xl.Save(outpath, false);*/
            return true;
        }
    }
}
