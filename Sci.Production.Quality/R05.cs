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
            List<SqlParameter> lis;
            DataTable dtFabricDetail, dtAccessoryDetail, dtFabricSummary, dtAccessorySummary;
            string cmdFabricDetail, cmdAccessoryDetail, str_Category, str_Material, ReportType, cmdFabricSummary, cmdAccessorySummary; 
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
            Cartegory.DefaultView.Sort = "Category";
            this.comboCategory.DataSource = Cartegory;
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
            this.comboMaterialType.SelectedIndex = 0;
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {

            bool dateSciDelivery_Empty = !this.DateSCIDelivery.HasValue, comboCategory_Empty = this.comboCategory.Text.Empty(), comboMaterial_Empty = this.comboMaterialType.Text.Empty(),
                 report_Empty = this.radioPanel.Value.Empty();
            if (dateSciDelivery_Empty)
            {
                MyUtility.Msg.ErrorBox("Please entry the 'SCI Delivery'");
                DateSCIDelivery.Focus();
                return false;
            } if (comboCategory_Empty)
            {
                MyUtility.Msg.ErrorBox("Please entry the 'Category'");
                comboCategory.Focus();
                return false;
            } if (comboMaterial_Empty)
            {
                MyUtility.Msg.ErrorBox("Please entry the 'MaterialType'");
                comboMaterialType.Focus();
                return false;
            }
            DateSCIStart = DateSCIDelivery.Value1;
            DateSCIEnd = DateSCIDelivery.Value2;
            str_Category = comboCategory.Text;
            str_Material = comboMaterialType.Text;
            ReportType = radioPanel.Value.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = ""; string sqlOrdersWhere = "";
            string CATEGORY = "";
            List<string> sqlCATEGORY = new List<string>();
            List<string> sqlWheres = new List<string>();
            List<string> sqlOrderWheres = new List<string>();
            #region --組WHERE--
            if (!this.DateSCIDelivery.Value1.Empty() && !this.DateSCIDelivery.Value2.Empty())
            {
                sqlOrderWheres.Add("SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            } 
            if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                sqlOrderWheres.Add("Category = @Cate");
                sqlCATEGORY.Add("@Cate");
                if (str_Category == "Bulk")
                {
                    lis.Add(new SqlParameter("@Cate", "B"));
                }
                if (str_Category == "Sample")
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
            CATEGORY = string.Join("", sqlCATEGORY);
            if (!sqlWhere.Empty())
            {
                sqlWhere =  sqlWhere +" AND ";
            } if (!sqlOrdersWhere.Empty())
            {
                sqlOrdersWhere = " AND " + sqlOrdersWhere;
            }
            #region --撈Fabric Detail 資料--

            cmdFabricDetail = string.Format(@" 
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
                ,iif(" + CATEGORY + @"='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = f.ReceivingID),(select scidelivery from dbo.orders where id = a.POID))<25,'Y','')
		        ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = f.ReceivingID),(select scidelivery from dbo.orders where id = a.POID) )<15,'Y','')
		                ) [Delay]
                ,psd.ID
                ,psd.seq1+'-'+psd.seq2[SEQ#]
                ,(select ExportId from dbo.Receiving where id = f.ReceivingID)[ExportId]
                ,f.ReceivingID
                ,[defectYDS]=f.TotalDefectPoint*5
                ,f.TotalInspYds

                from order_rawdata a
                inner join dbo.PO_Supp_Detail psd on psd.ID = a.POID
                inner join FIR f on f.POID = psd.ID and f.SEQ1 = psd.Seq1 and f.seq2 = psd.Seq2
                where " + sqlWhere + @" AND  psd.SEQ1 NOT BETWEEN '50'AND'79'");
            #endregion
            #region --撈 Accessory Detail 資料--
            cmdAccessoryDetail = string.Format(@" with order_rawdata as
                (
	                select distinct poid from dbo.orders
	                where Junk =0  " + sqlOrdersWhere + @"
                )
                select 
                (select p.SuppID+'-'+s.AbbEN from dbo.PO_Supp p inner join dbo.Supp s on s.ID = p.SuppID
                where p.id = psd.ID and p.seq1 = psd.SEQ1 ) [Supplier]
                ,AR.Refno
                ,dbo.getMtlDesc(psd.id,psd.seq1,psd.seq2,1,0) [description]
              	,(select sum(R.ShipQty) from dbo.Receiving m inner join dbo.Receiving_Detail R on R.Id = m.Id 
				where m.id = AR.ReceivingID and R.PoId = psd.ID and R.seq1 = psd.seq1 and R.seq2 = psd.SEQ2)[ShipQty]
				,(select sum(n.StockQty) from dbo.Receiving m inner join dbo.Receiving_Detail n on n.Id = m.Id 
				where m.id = AR.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2)[ArriveQty]
               ,AR.Result
                ,(select WhseArrival from dbo.Receiving where id = AR.ReceivingID)[WhseArrival]
                ,(select scidelivery from dbo.orders where id = AR.POID)[scidelivery]
                ,iif(" + CATEGORY + @"='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = AR.ReceivingID),(select scidelivery from dbo.orders where id = a.POID))<25,'Y','')
		        ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = AR.ReceivingID),(select scidelivery from dbo.orders where id = a.POID) )<15,'Y','')
		                ) [Delay]
                ,AR.POID
                ,AR.seq1+'-'+AR.seq2[SEQ#]
                ,(select ExportId from dbo.Receiving where id = AR.ReceivingID)[ExportId]
                ,AR.ReceivingID
            
                from order_rawdata a
                inner join dbo.PO_Supp_Detail psd on psd.ID = a.POID
                inner join AIR AR on AR.POID = psd.ID and AR.SEQ1 = psd.Seq1 and AR.seq2 = psd.Seq2
                where " + sqlWhere + @" AND  psd.SEQ1 NOT BETWEEN '50'AND'79' ");
            #endregion
            #region --撈 Fabric Summary 資料--
            cmdFabricSummary = string.Format(@"
                with order_rawdata as
                (
	                select distinct poid from dbo.orders
	                where Junk =0 " + sqlOrdersWhere + @"
                )
                select 
				[Supplier]=PSSA.SuppID+'-'+PSSA.AbbEN 
                ,[ItemsRef]=(SELECT DISTINCT   count(f.Refno) FROM FIR f)
				,[DelayItemsRef]=sum(iif(DelayItemsRef.TF='Y',1,0))OVER(PARTITION BY f.Refno)
				,[ShipQty]= SUM(ShipQty.SHIP)
                ,[ArriveQty]=SUM(ArriveQty.Stock)
                ,(ShipQty.SHIP-ArriveQty.Stock)Balance
				,100-IIF((SELECT DISTINCT count(f.Refno) FROM FIR f)!=0, ROUND((count(DelayItemsRef.TF) / (SELECT DISTINCT count(f.Refno) FROM FIR f)*100),0),0)[% On-Time Delivery]
				,[TotalDefect]=sum(f.TotalDefectPoint*5)
				,[TotalInspected]=sum(f.TotalInspYds)
				,[Total Arrived]=sum(ShipQty.SHIP)OVER(PARTITION BY f.Refno)
				,IIF(sum(f.TotalInspYds)!=0,Round(sum(f.TotalDefectPoint*5)/sum(f.TotalInspYds) * 100,0),0)[DefectPercentage]
				,IIF(SUM(ArriveQty.Stock)!=0, Round(sum(f.TotalInspYds)/SUM(ArriveQty.Stock)*100,0),0)[Total % of Inspection]
				,IIF(sum(f.TotalInspYds)!=0, 100-Round(sum(f.TotalDefectPoint*5)/sum(f.TotalInspYds) * 100,0),0)[QualityRating]

                from order_rawdata a
                inner join dbo.PO_Supp_Detail psd on psd.ID = a.POID
                inner join FIR f on f.POID = psd.ID and f.SEQ1 = psd.Seq1 and f.seq2 = psd.Seq2
				OUTER APPLY (select p.SuppID,s.AbbEN from dbo.PO_Supp p inner join dbo.Supp s on s.ID = p.SuppID where p.id = psd.ID and p.seq1 = psd.SEQ1 )PSSA
				OUTER APPLY (select n.PoUnit,n.ShipQty,n.StockUnit,n.StockQty from dbo.Receiving m inner join dbo.Receiving_Detail n on n.Id = m.Id where m.id = f.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2)N
                OUTER APPLY (select  dbo.getUnitRate(n.PoUnit,'YDS')*n.ShipQty SHIP)ShipQty
				OUTER APPLY (select dbo.getUnitRate(n.StockUnit,'YDS')*n.StockQty  Stock)ArriveQty
				OUTER APPLY (SELECT iif" + CATEGORY + @"='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = f.ReceivingID),(select scidelivery from dbo.orders where id = a.POID))<25,'Y','')
									 ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving where id = f.ReceivingID),(select scidelivery from dbo.orders where id = a.POID) )<15,'Y',''))  
								      TF)DelayItemsRef
			    where " + sqlWhere + @" AND  psd.SEQ1 NOT BETWEEN '50'AND'79'
				GROUP BY PSSA.SuppID,PSSA.AbbEN,psd.id,psd.seq1,psd.seq2,ShipQty.SHIP,ArriveQty.Stock,f.Refno,DelayItemsRef.TF");
             #endregion
            #region --撈 Accessory Summary 資料--
            cmdAccessorySummary = string.Format(@"");
            #endregion

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmdFabricDetail, lis, out dtFabricDetail);
            if (!res)
            {
                return res;
            }
            res = DBProxy.Current.Select("", cmdAccessoryDetail, lis, out dtAccessoryDetail);
            if (!res)
            {
                return res;
            }
            return res;
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            //if (dtFabric == null || dtFabric.Rows.Count == 0)
            //{
            //    MyUtility.Msg.ErrorBox("Data not found");
            //    return false;
            //} if (dtAccessory == null || dtAccessory.Rows.Count == 0)
            //{
            //    MyUtility.Msg.ErrorBox("Data not found");
            //    return false;
            //}

               var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            if ("Fabric".EqualString(this.comboMaterialType.Text))
            {
                if ("Detail".EqualString(this.radioDetail.Text))
                {
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_FabricDetail.xltx");
                    xl.dicDatas.Add("##BODY", dtFabricDetail);
                    xl.Save(outpath, false);
                }
                else {
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_FabricSummary.xltx");
                    xl.dicDatas.Add("##BODY", dtFabricSummary);
                    xl.Save(outpath, false);
                }
               
            }
            else if ("Accessory".EqualString(this.comboMaterialType.Text))
            {
                if ("Detail".EqualString(this.radioDetail.Text))
                {
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_AccessoryDetail.xltx");
                    xl.dicDatas.Add("##BODY", dtAccessoryDetail);
                    xl.Save(outpath, false);
                }
                else
                {
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_AccessorySummary.xltx");
                    xl.dicDatas.Add("##BODY", dtAccessorySummary);
                    xl.Save(outpath, false);
                }
               
            }
            return true;
        }
    }
}
