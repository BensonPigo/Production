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
    {
        DateTime? DateSCIStart; DateTime? DateSCIEnd;
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
                        from  dbo.DropDownList WITH (NOLOCK) 
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
                        FROM Po_supp_detail WITH (NOLOCK) 
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

            if (radioSummary.Checked)
            {
                lis.Add(new SqlParameter("@summary", "1"));
            }
            else
            {
                lis.Add(new SqlParameter("@summary", "0"));
            }

            #endregion
            sqlOrdersWhere = string.Join(" and ", sqlOrderWheres);
            sqlWhere = string.Join(" and ", sqlWheres);
            CATEGORY = string.Join("", sqlCATEGORY);
            if (!sqlOrdersWhere.Empty())
            {
                sqlOrdersWhere = " AND " + sqlOrdersWhere;
            }
            #region --撈Fabric Detail 資料--

            cmdFabricDetail = string.Format(@" 
                with order_rawdata as
                (
	                select distinct poid from dbo.orders WITH (NOLOCK) 
	                where Junk =0 " + sqlOrdersWhere + @"
                )
                select 
                (select p.SuppID+'-'+s.AbbEN from dbo.PO_Supp p WITH (NOLOCK) inner join dbo.Supp s WITH (NOLOCK) on s.ID = p.SuppID
                where p.id = psd.ID and p.seq1 = psd.SEQ1 ) [Supplier]
                ,psd.Refno
                ,dbo.getMtlDesc(psd.id,psd.seq1,psd.seq2,1,0) [description]
                ,(select sum(dbo.getUnitRate(n.PoUnit,'YDS')*n.ShipQty) from dbo.Receiving m WITH (NOLOCK) inner join dbo.Receiving_Detail n WITH (NOLOCK) on n.Id = m.Id 
                where m.id = f.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2) [ShipQty]
                ,(select sum(dbo.getUnitRate(n.StockUnit,'YDS')*n.StockQty) from dbo.Receiving m WITH (NOLOCK) inner join dbo.Receiving_Detail n WITH (NOLOCK) on n.Id = m.Id 
                where m.id = f.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2) [ArriveQty]
                ,f.Physical
                ,(select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = f.ReceivingID)[WhseArrival]
                ,(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID)[scidelivery]
                ,iif(" + CATEGORY + @"='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = f.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID))<25,'Y','')
		        ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = f.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID) )<15,'Y','')
		                ) [Delay]
                ,psd.ID
                ,psd.seq1+'-'+psd.seq2[SEQ#]
                ,(select ExportId from dbo.Receiving WITH (NOLOCK) where id = f.ReceivingID)[ExportId]
                ,f.ReceivingID
                ,[defectYDS]=f.TotalDefectPoint*5
                ,f.TotalInspYds

                from order_rawdata a
                inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID
                inner join FIR f WITH (NOLOCK) on f.POID = psd.ID and f.SEQ1 = psd.Seq1 and f.seq2 = psd.Seq2
                where " + sqlWhere + @" AND  psd.SEQ1 NOT BETWEEN '50'AND'79'");
            #endregion
            #region --撈 Accessory Detail 資料--
            cmdAccessoryDetail = string.Format(@" with order_rawdata as
                (
	                select distinct poid from dbo.orders WITH (NOLOCK) 
	                where Junk =0  " + sqlOrdersWhere + @"
                )
                select 
                (select p.SuppID+'-'+s.AbbEN from dbo.PO_Supp p WITH (NOLOCK) inner join dbo.Supp s WITH (NOLOCK) on s.ID = p.SuppID
                where p.id = psd.ID and p.seq1 = psd.SEQ1 ) [Supplier]
                ,AR.Refno
                ,dbo.getMtlDesc(psd.id,psd.seq1,psd.seq2,1,0) [description]
              	,(select sum(R.ShipQty) from dbo.Receiving m WITH (NOLOCK) inner join dbo.Receiving_Detail R WITH (NOLOCK) on R.Id = m.Id 
				where m.id = AR.ReceivingID and R.PoId = psd.ID and R.seq1 = psd.seq1 and R.seq2 = psd.SEQ2)[ShipQty]
				,(select sum(n.StockQty) from dbo.Receiving m WITH (NOLOCK) inner join dbo.Receiving_Detail n WITH (NOLOCK) on n.Id = m.Id 
				where m.id = AR.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2)[ArriveQty]
               ,AR.Result
                ,(select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = AR.ReceivingID)[WhseArrival]
                ,(select scidelivery from dbo.orders WITH (NOLOCK) where id = AR.POID)[scidelivery]
                ,iif(" + CATEGORY + @"='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = AR.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID))<25,'Y','')
		        ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = AR.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID) )<15,'Y','')
		                ) [Delay]
                ,AR.POID
                ,AR.seq1+'-'+AR.seq2[SEQ#]
                ,(select ExportId from dbo.Receiving WITH (NOLOCK) where id = AR.ReceivingID)[ExportId]
                ,AR.ReceivingID
            
                from order_rawdata a
                inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID
                inner join AIR AR WITH (NOLOCK) on AR.POID = psd.ID and AR.SEQ1 = psd.Seq1 and AR.seq2 = psd.Seq2
                where " + sqlWhere + @" AND  psd.SEQ1 NOT BETWEEN '50'AND'79' ");
            #endregion
            #region --撈 Fabric Summary 資料--
            cmdFabricSummary = string.Format(@"
                with order_rawdata as
                ( 
	                select distinct poid from dbo.orders WITH (NOLOCK) 
	                where Junk =0  " + sqlOrdersWhere + @"
                )
                select
	                psd.ID,psd.SEQ1,psd.SEQ2,ps.SuppID,f.Refno,s.AbbEN,DelayItemsRef.TF
	                ,n.StockUnit,n.StockQty,n.PoUnit,n.ShipQty,f.TotalDefectPoint,f.TotalInspYds
                into #tmp
                from order_rawdata a
                inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID
                inner join FIR f WITH (NOLOCK) on f.POID = psd.ID and f.SEQ1 = psd.Seq1 and f.seq2 = psd.Seq2
                inner join dbo.PO_Supp as ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
                inner join dbo.Supp as s WITH (NOLOCK) on s.ID = ps.SuppID
                inner join dbo.Receiving as m WITH (NOLOCK) on m.id = f.ReceivingID
                inner join dbo.Receiving_Detail as n WITH (NOLOCK) on n.Id = m.Id and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2
                OUTER APPLY (SELECT iif(" + CATEGORY + @"='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = f.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID))<25,'Y','')
									                 ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = f.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID) )<15,'Y',''))  
								                      TF)DelayItemsRef
                WHERE " + sqlWhere + @" AND psd.SEQ1 NOT BETWEEN '50'AND'79'

                /*
	                if want summary
                */
              --  declare @summary bit = 1
                if @summary  = 1
	            select
		            (Suppid+'-'+AbbEN)[Supplier]
		            ,ItemRef = max(s.counts)
		            ,SUM(iif(TF='Y',1,0))[TotalDelay]
		            ,sum([ShipQty])[ShipQty]
		            ,sum([ArriveQty])[ArriveQty]
		            ,[Balance]=sum([ShipQty])-sum([ArriveQty])
		            ,100-ROUND((sum(iif(TF='Y',1,0))/sum(s.counts)*100),0)[% On-Time Delivery]
		            ,SUM(TotalDefectPoint)*5[TotalDefect]
		            ,SUM(TotalInspYds)[TotalInspected]
		            ,SUM([ShipQty])[Total Arrived]
		            ,IIF(SUM(TotalInspYds)!=0,ROUND(SUM((TotalDefectPoint)*5) /SUM(TotalInspYds)*100,0),0)[DefectPercentage]
		            ,IIF(sum([ArriveQty])!=0,ROUND(SUM(TotalInspYds)/sum([ArriveQty])*100,0),0)[Total % of Inspection]
	                ,[QualityRating]=100-IIF(SUM(TotalInspYds)!=0,ROUND(SUM(TotalDefectPoint)*5/SUM(TotalInspYds)*100,0),0)		
	            from #tmp tmp
	            outer apply( select count(distinct Refno) counts from #tmp as sTmp where sTmp.suppid = tmp.SuppID )as s
	            outer apply (select dbo.getUnitRate(StockUnit,'YDS')*StockQty as ArriveQty) aq
	            GROUP BY SuppID,AbbEN
	
            else
	            select * from #tmp

	            drop table #tmp
                ");
            #endregion
            #region --撈 Accessory Summary 資料--
            cmdAccessorySummary = string.Format(@"
            ; declare @testcategory as varchar(1);
            set @testcategory = 'B';             
            with order_rawdata as
            (
                 select distinct poid from dbo.orders WITH (NOLOCK) 
                 where Junk =0  " + sqlOrdersWhere + @"
            )
            select
                 psd.ID,psd.SEQ1,psd.SEQ2,ps.SuppID,ai.Refno,s.AbbEN,DelayItemsRef.TF
                 ,n.StockUnit,n.StockQty,n.PoUnit,n.ShipQty,ai.InspQty,ai.Result
            into #tmp
            from order_rawdata a
            inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID
            inner join AIR ai WITH (NOLOCK) on  ai.POID = psd.ID and ai.SEQ1 = psd.Seq1 and ai.seq2 = psd.Seq2
            inner join dbo.PO_Supp as ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
            inner join dbo.Supp as s WITH (NOLOCK) on s.ID = ps.SuppID
            inner join dbo.Receiving as m WITH (NOLOCK) on m.id = ai.ReceivingID
            inner join dbo.Receiving_Detail as n WITH (NOLOCK) on n.Id = m.Id and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2
            OUTER APPLY (SELECT iif(" + CATEGORY + @"='B',iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = ai.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID))<25,'Y','')
                                                     ,iif(DATEDIFF(day, (select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = ai.ReceivingID),(select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID) )<15,'Y',''))  
                                                       TF)DelayItemsRef
            WHERE " + sqlWhere + @" AND psd.SEQ1 NOT BETWEEN '50'AND'79'

            /*
                 if want summary
            */

            -- declare @summary bit = 1
            if @summary  = 1
	            select (Suppid+'-'+AbbEN)[Supplier]
		            ,s.*
		            ,fr.*
		            ,[Quality Rating]=iif(s.ItemRef!=0,round((fr.failCount /cast(s.ItemRef AS decimal))*100,0,1),0)
	            from (
                 select
                     SuppID,AbbEN
                     ,ItemRef = max(s.counts)
                     ,SUM(iif(TF='Y',1,0))[TotalDelay]
                     ,sum([ShipQty])[ShipQty]
                     ,sum([ArriveQty])[ArriveQty]
                     ,[Balance]=sum([ShipQty])-sum([ArriveQty])
                     ,100-ROUND((sum(iif(TF='Y',1,0))/sum(s.counts)*100),0)[% On-Time Delivery]
                     ,SUM(InspQty)[TotalInspectedQty]
   
                 from #tmp tmp
                 outer apply( select count(distinct Refno) counts from #tmp as sTmp where sTmp.suppid = tmp.SuppID )as s
                 outer apply (select dbo.getUnitRate(StockUnit,'YDS')*StockQty as ArriveQty) aq
    
                 GROUP BY SuppID,AbbEN
	             ) as s 
                 outer apply( select failCount =count(*) from (select distinct  Refno from #tmp as sTmp where sTmp.suppid = s.SuppID and Result='fail' ) as f)as fr

	             else
	             select * from #tmp
	
	            drop table #tmp");
            #endregion

            return base.ValidateInput();
        }
        string MaterialType = "";
        private void toexcel_Click(object sender, EventArgs e)
        {
            MaterialType = this.comboMaterialType.Text;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res = new DualResult(false);

            if ("Fabric".EqualString(MaterialType))
            {
                if (radioDetail.Checked)
                {                    
                    res = DBProxy.Current.Select("", cmdFabricDetail, lis, out dtFabricDetail);
                }
                if (radioSummary.Checked)
                {                    
                    res = DBProxy.Current.Select("", cmdFabricSummary, lis, out dtFabricSummary);
                }

            }
            else if ("Accessory".EqualString(MaterialType))
            {
                if (radioDetail.Checked) 
                {
                    res = DBProxy.Current.Select("", cmdAccessoryDetail, lis, out dtAccessoryDetail);
                }
                if (radioSummary.Checked)
                {
                    res = DBProxy.Current.Select("", cmdAccessorySummary, lis, out dtAccessorySummary);
                    dtAccessorySummary.Columns.Remove("SuppID");
                    dtAccessorySummary.Columns.Remove("AbbEN");
                }
            }

            return res;
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
      
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            //saveDialog.ShowDialog();
            //string outpath = saveDialog.FileName;
            //if (outpath.Empty())
            //{
            //    return false;
            //}

            if ("Fabric".EqualString(this.comboMaterialType.Text))
            {
                if (radioDetail.Checked) //("Detail".EqualString(this.radioDetail.Text))
                {
                    // 顯示筆數於PrintForm上Count欄位
                    SetCount(dtFabricDetail.Rows.Count);
                    if (dtFabricDetail == null || dtFabricDetail.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_FabricDetail.xltx");
                    xl.dicDatas.Add("##BODY", dtFabricDetail);
                    xl.Save();
                }
                if (radioSummary.Checked)//("Summary".EqualString(this.radioSummary.Text))
                {
                    // 顯示筆數於PrintForm上Count欄位
                    SetCount(dtFabricSummary.Rows.Count);
                    if (dtFabricSummary == null || dtFabricSummary.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_FabricSummary.xltx");
                    xl.dicDatas.Add("##BODY", dtFabricSummary);
                    xl.Save();
                }

            }
            else if ("Accessory".EqualString(this.comboMaterialType.Text))
            {
                // 顯示筆數於PrintForm上Count欄位
                SetCount(dtAccessoryDetail.Rows.Count);
                if (radioDetail.Checked) //("Detail".EqualString(this.radioDetail.Text))
                {
                    if (dtAccessoryDetail == null || dtAccessoryDetail.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_AccessoryDetail.xltx");
                    xl.dicDatas.Add("##BODY", dtAccessoryDetail);
                    xl.Save();
                }
                if (radioSummary.Checked)//("Summary".EqualString(this.radioSummary.Text))
                {
                    // 顯示筆數於PrintForm上Count欄位
                    SetCount(dtAccessorySummary.Rows.Count);
                    if (dtAccessorySummary == null || dtAccessorySummary.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R05_AccessorySummary.xltx");
                    xl.dicDatas.Add("##BODY", dtAccessorySummary);
                    xl.Save();
                }

            }
            return true;
        }


    }
}
