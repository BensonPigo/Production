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
using sxrc = Sci.Utility.Excel.SaveXltReportCls;

namespace Sci.Production.Quality
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateSCIStart; DateTime? DateSCIEnd;
        List<SqlParameter> lis;
        DataTable dtFabricDetail, dtAccessoryDetail, dtFabricSummary, dtAccessorySummary;
        private string cmdFabricDetail, cmdAccessoryDetail, str_Category, str_Material, ReportType, cmdFabricSummary, cmdAccessorySummary;
        private string MaterialType = string.Empty;
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {

            InitializeComponent();
            this.comboCategory.SelectedIndex = 0;
            DualResult result;
            DataTable Material = null;
            string sqlM = (@" 
                        SELECT distinct case fabrictype
                               when 'F' then 'Fabric' 
	                           when 'A' then 'Accessory'
	                           end fabrictype
                        FROM Po_supp_detail WITH (NOLOCK) 
                        where fabrictype !='O'  AND fabrictype !=''
                        ");
            if (!(result = DBProxy.Current.Select("", sqlM, out Material)))
            {
                ShowErr(result);
            }
            else
            {
                Material.DefaultView.Sort = "fabrictype";
                this.comboMaterialType.DataSource = Material;
                this.comboMaterialType.ValueMember = "fabrictype";
                this.comboMaterialType.DisplayMember = "fabrictype";
                this.comboMaterialType.SelectedIndex = 0;
                this.comboMaterialType.SelectedValue = "Fabric";
                MaterialType = this.comboMaterialType.SelectedValue.ToString();
            }
          
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {

            bool dateSciDelivery_Empty = !this.dateSCIDelivery.HasValue, comboCategory_Empty = this.comboCategory.Text.Empty(), comboMaterial_Empty = this.comboMaterialType.Text.Empty(),
                 report_Empty = this.radioPanel.Value.Empty();
            if (dateSciDelivery_Empty)
            {
                dateSCIDelivery.Focus();
                MyUtility.Msg.ErrorBox("Please entry the 'SCI Delivery'");
                return false;
            } if (comboCategory_Empty)
            {
                comboCategory.Focus();
                MyUtility.Msg.ErrorBox("Please entry the 'Category'");
                return false;
            } if (comboMaterial_Empty)
            {
                comboMaterialType.Focus();
                MyUtility.Msg.ErrorBox("Please entry the 'MaterialType'");
                return false;
            }
            DateSCIStart = dateSCIDelivery.Value1;
            DateSCIEnd = dateSCIDelivery.Value2;
            str_Category = comboCategory.Text;
            str_Material = comboMaterialType.Text;
            ReportType = radioPanel.Value.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = ""; string sqlOrdersWhere = "";
            string CATEGORY = "";
            List<string> sqlWheres = new List<string>();
            List<string> sqlOrderWheres = new List<string>();
            #region --組WHERE--
            if (!this.dateSCIDelivery.Value1.Empty())
            {
                sqlOrderWheres.Add("SciDelivery >= @SCIDate1");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
            }
            if (!this.dateSCIDelivery.Value2.Empty())
            {
                sqlOrderWheres.Add("SciDelivery <= @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            }
            if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                CATEGORY = $"({this.comboCategory.SelectedValue})";
                sqlOrderWheres.Add($"Category in ({this.comboCategory.SelectedValue})");

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
            if (!sqlOrdersWhere.Empty())
            {
                sqlOrdersWhere = " AND " + sqlOrdersWhere;
            }
            sqlOrdersWhere += this.chkIncludeCancelOrder.Checked ? string.Empty : " and orders.Junk = 0 ";
            #region --撈Fabric Detail 資料--

            cmdFabricDetail = string.Format(@" 
                with order_rawdata as
                (
	                select distinct poid from dbo.orders WITH (NOLOCK) 
	                where 1 = 1 " + sqlOrdersWhere + @"
                )
                select 
                (select p.SuppID+'-'+s.AbbEN from dbo.PO_Supp p WITH (NOLOCK) inner join dbo.Supp s WITH (NOLOCK) on s.ID = p.SuppID
                where p.id = psd.ID and p.seq1 = psd.SEQ1 ) [Supplier]
                ,psd.Refno
                ,dbo.getMtlDesc(psd.id,psd.seq1,psd.seq2,1,0) [description]
                ,[ShipQty] = (select sum(iif(m.DataFrom = 'Receiving', dbo.getUnitRate(m.PoUnit,'YDS')*m.ShipQty, m.ShipQty)) 
                              from dbo.View_AllReceivingDetail m WITH (NOLOCK) 
                              where m.id = f.ReceivingID and m.PoId = psd.ID and m.seq1 = psd.seq1 and m.seq2 = psd.SEQ2) 
                ,[ArriveQty] =  (select sum(iif(m.DataFrom = 'Receiving', dbo.getUnitRate(m.PoUnit,'YDS')*m.StockQty, m.StockQty)) 
                              from dbo.View_AllReceivingDetail m WITH (NOLOCK) 
                              where m.id = f.ReceivingID and m.PoId = psd.ID and m.seq1 = psd.seq1 and m.seq2 = psd.SEQ2) 
                ,f.Physical
                ,va.WhseArrival
                ,o.scidelivery
                ,iif('B' in " + CATEGORY + @",iif(DATEDIFF(day, va.WhseArrival,o.scidelivery)<25,'Y','')
		        ,iif(DATEDIFF(day, va.WhseArrival,o.scidelivery )<15,'Y','')
		                ) [Delay]
                ,psd.ID
                ,[Cancel] = IIF(o.Junk=1,'Y','N')
                ,psd.seq1+'-'+psd.seq2[SEQ#]
                ,va.ExportId
                ,f.ReceivingID
                ,[defectYDS]= defect.count*5
                ,f.TotalInspYds
                from order_rawdata a
                inner join Orders o with (nolock) on o.ID = a.POID
                inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID
                inner join FIR f WITH (NOLOCK) on f.POID = psd.ID and f.SEQ1 = psd.Seq1 and f.seq2 = psd.Seq2
                left join dbo.View_AllReceiving va with (nolock) on va.ID = f.ReceivingID
                outer apply(
	                select count(1) [count] from dbo.FIR_Physical x 
	                inner join dbo.FIR_Physical_Defect y on y.FIR_PhysicalDetailUKey = x.DetailUkey 
	                where x.ID = f.ID
                ) as Defect
                where " + sqlWhere + @" AND  psd.SEQ1 NOT BETWEEN '50'AND'79' and f.Physical<>'' and f.PhysicalEncode=1 ");
            #endregion
            #region --撈 Accessory Detail 資料--
            cmdAccessoryDetail = string.Format(@" with order_rawdata as
                (
	                select distinct poid from dbo.orders WITH (NOLOCK) 
	                where 1 = 1  " + sqlOrdersWhere + @"
                )
                select 
                (select p.SuppID+'-'+s.AbbEN from dbo.PO_Supp p WITH (NOLOCK) inner join dbo.Supp s WITH (NOLOCK) on s.ID = p.SuppID
                where p.id = psd.ID and p.seq1 = psd.SEQ1 ) [Supplier]
                ,AR.Refno
                ,dbo.getMtlDesc(psd.id,psd.seq1,psd.seq2,1,0) [description]
              	,[ShipQty] = (select sum(m.ShipQty) from dbo.View_AllReceivingDetail m WITH (NOLOCK)
				                where m.id = AR.ReceivingID and m.PoId = psd.ID and m.seq1 = psd.seq1 and m.seq2 = psd.SEQ2)
				,[ArriveQty] = (select sum(m.StockQty) from dbo.View_AllReceivingDetail m WITH (NOLOCK)
				                where m.id = AR.ReceivingID and m.PoId = psd.ID and m.seq1 = psd.seq1 and m.seq2 = psd.SEQ2)
               ,AR.Result
                ,va.WhseArrival
                ,o.scidelivery
                ,iif(  'B' in " + CATEGORY + @",iif(DATEDIFF(day, va.WhseArrival,o.scidelivery)<25,'Y','')
		        ,iif(DATEDIFF(day, va.WhseArrival,o.scidelivery)<15,'Y','')
		                ) [Delay]
                ,AR.POID
                ,o.Junk
                ,AR.seq1+'-'+AR.seq2[SEQ#]
                ,va.ExportId
                ,AR.ReceivingID
                from order_rawdata a
                inner join Orders o with (nolock) on o.ID = a.POID
                inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID
                inner join AIR AR WITH (NOLOCK) on AR.POID = psd.ID and AR.SEQ1 = psd.Seq1 and AR.seq2 = psd.Seq2
                left join dbo.View_AllReceiving va with (nolock) on va.ID = AR.ReceivingID
                where " + sqlWhere + @" AND  psd.SEQ1 NOT BETWEEN '50'AND'79' AND AR.Result<>''
and ar.Status='Confirmed' ");
            #endregion
            #region --撈 Fabric Summary 資料--
            cmdFabricSummary = string.Format(@"
with order_rawdata as
( 
	select distinct poid 
    from dbo.orders WITH (NOLOCK) 
	where 1 = 1
    " + sqlOrdersWhere + @"
)
select psd.ID
	   , psd.SEQ1
	   , psd.SEQ2
	   , ps.SuppID
	   , f.Refno
	   , s.AbbEN
	   , DelayItemsRef.TF
	   , n.StockUnit
	   , n.StockQty
	   , n.PoUnit
	   , n.ShipQty
	   , [TotalDefectPoint] = Defect.count
	   , f.TotalInspYds
into #tmp
from order_rawdata a
inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID
inner join FIR f WITH (NOLOCK) on f.POID = psd.ID 
								  and f.SEQ1 = psd.Seq1 
								  and f.seq2 = psd.Seq2
inner join dbo.PO_Supp as ps WITH (NOLOCK) on ps.ID = psd.ID 
											  and ps.SEQ1 = psd.SEQ1
inner join dbo.Supp as s WITH (NOLOCK) on s.ID = ps.SuppID
inner join dbo.View_AllReceivingDetail as n WITH (NOLOCK) on n.Id = f.ReceivingID
													  and n.PoId = psd.ID 
													  and n.seq1 = psd.seq1 
													  and n.seq2 = psd.SEQ2
left join dbo.View_AllReceiving va with (nolock) on va.ID = f.ReceivingID
OUTER APPLY (
	SELECT TF = iif('b' = 'B', iif(DATEDIFF(day
									   		, va.WhseArrival
									   		, (select scidelivery 
									   		   from dbo.orders WITH (NOLOCK) 
									   		   where id = a.POID)) < 25
							  	   , 'Y'
							  	   , '')
				        	 , iif(DATEDIFF(day
				        	    	   		, va.WhseArrival
				        	    	   		, (select scidelivery 
				        	    	   			from dbo.orders WITH (NOLOCK) 
				        	    	   			where id = a.POID)) < 15
					        	   , 'Y'
					        	   , ''))  
				
) DelayItemsRef
outer apply(
    select [count] = count(1)  
    from dbo.FIR_Physical x 
    inner join dbo.FIR_Physical_Defect y on y.FIR_PhysicalDetailUKey = x.DetailUkey 
    where x.ID = f.ID
) as Defect
WHERE " + sqlWhere + @" 
	  AND psd.SEQ1 NOT BETWEEN '50' AND '79' 
	  and f.Physical <> '' 
	  and f.PhysicalEncode = 1

    /*
        if want summary
    */
  --  declare @summary bit = 1
if @summary  = 1
	select [Supplier]
		   , ItemRef
		   , [TotalDelay]
		   , [ShipQty]
		   , [ArriveQty]
		   , [Balance] = convert(numeric(38, 2), isnull (ShipQty, 0) - isnull (ArriveQty, 0))
		   , [% On-Time Delivery]
		   , [TotalDefectPoint]
		   , [TotalInspYds]
		   , [Total Arrived]
		   , [DefectPercentage]
		   , [Total % of Inspection]
		   , [QualityRating]
           , [Level]
	from (
		select [Supplier] = (Suppid+'-'+AbbEN)
			   , ItemRef = max(s.counts)
			   , [TotalDelay] = (delay.DelayCount)
			   , [ShipQty] = sum([ShipQty])
			   , [ArriveQty] = sum([ArriveQty])
			   , [Balance] = sum([ShipQty]) - sum([ArriveQty])
			   , [% On-Time Delivery] = 100 - ROUND((CONVERT(float, delay.DelayCount) /  max(s.counts) * 100), 2)
			   , [TotalDefectPoint] = (Defect.TotalDefectPoint) * 5
			   , [TotalInspYds] = yds.TotalInspYds
			   , [Total Arrived] = SUM([ShipQty])
			   , [DefectPercentage] = IIF(yds.TotalInspYds != 0, ROUND((Defect.TotalDefectPoint) * 5 / yds.TotalInspYds * 100, 2)
			   												   , 0)
			   , [Total % of Inspection] = IIF(sum([ArriveQty]) != 0, ROUND(yds.TotalInspYds / sum([ArriveQty]) * 100, 2)
			   														, 0)
			   , [QualityRating] = 100 - IIF(yds.TotalInspYds != 0, ROUND((Defect.TotalDefectPoint) * 5 / yds.TotalInspYds * 100, 2)
			   													  , 0)		
               , [Level] = (select ID from SuppLevel where IIF(yds.TotalInspYds != 0, ROUND((Defect.TotalDefectPoint) * 5 / yds.TotalInspYds * 100, 2)
			   												   , 0) between Range1 and Range2)
		from #tmp tmp
		outer apply( 
			select counts = count(distinct Refno) 
			from #tmp as sTmp 
			where sTmp.suppid = tmp.SuppID 
		)as s
		outer apply(
			select ArriveQty = dbo.getUnitRate(StockUnit,'YDS') * StockQty 
		) aq
		outer apply(
			select DelayCount = count(distinct Refno)  
			from #tmp as sTmp 
			where sTmp.suppid = tmp.SuppID 
				  and tf = 'Y'
	  	) delay
		outer apply(
			select TotalInspYds = sum(TotalInspYds)
			from (
				select distinct id
					   , seq1
					   , seq2
					   , TotalInspYds 
			    from #tmp
				where SuppID = tmp.SuppID
			) a 
		) yds
		outer apply(
			select TotalDefectPoint = sum(TotalDefectPoint) 
			from (
				select distinct id
					   , seq1
					   , seq2
					   , TotalDefectPoint 
			    from #tmp
				where SuppID = tmp.SuppID
			) a 
		) Defect
		GROUP BY SuppID, AbbEN, delay.DelayCount, yds.TotalInspYds
				 , Defect.TotalDefectPoint
	) t
else
    select * from #tmp
drop table #tmp");
            #endregion
            #region --撈 Accessory Summary 資料--
            cmdAccessorySummary = string.Format(@"
            ; declare @testcategory as varchar(1);
            set @testcategory = 'B';             
            with order_rawdata as
            (
                 select distinct poid from dbo.orders WITH (NOLOCK) 
                 where 1 = 1  " + sqlOrdersWhere + @"
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
            inner join dbo.View_AllReceivingDetail as n WITH (NOLOCK) on n.Id = ai.ReceivingID and n.PoId = psd.ID and n.seq1 = psd.seq1 and n.seq2 = psd.SEQ2
            left join dbo.View_AllReceiving va with (nolock) on va.ID = ai.ReceivingID
            OUTER APPLY (SELECT iif('B' in " + CATEGORY + @",iif(DATEDIFF(day, va.WhseArrival, (select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID))<25,'Y','')
                                                     ,iif(DATEDIFF(day, va.WhseArrival, (select scidelivery from dbo.orders WITH (NOLOCK) where id = a.POID) )<15,'Y',''))  
                                                       TF)DelayItemsRef
            WHERE " + sqlWhere + @" AND psd.SEQ1 NOT BETWEEN '50'AND'79' AND Ai.Result<>''
and ai.Status='Confirmed' 

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
		,[TotalDelay]=(delay.DelayCount)
        ,sum([ShipQty])[ShipQty]
        ,sum([ArriveQty])[ArriveQty]
        ,[Balance]=sum([ShipQty])-sum([ArriveQty])        
		,100-ROUND((CONVERT(float,delay.DelayCount)/ max(s.counts)*100),2)[% On-Time Delivery]
        ,SUM(InspQty)[TotalInspectedQty]
   
    from #tmp tmp
    outer apply( select count(distinct Refno) counts from #tmp as sTmp where sTmp.suppid = tmp.SuppID )as s
    outer apply (select dbo.getUnitRate(StockUnit,'YDS')*StockQty as ArriveQty) aq
	outer apply(select count(distinct Refno) DelayCount from #tmp as sTmp where sTmp.suppid = tmp.SuppID and tf='Y') as delay
    
    GROUP BY SuppID,AbbEN,delay.DelayCount
	             ) as s 
                 outer apply( select failCount =count(*) from (select distinct  Refno from #tmp as sTmp where sTmp.suppid = s.SuppID and Result='fail' ) as f)as fr

	             else
	             select * from #tmp
	
	            drop table #tmp");
            #endregion

            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res = new DualResult(false);

            if (MaterialType.EqualString("Fabric"))
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
            else if (MaterialType.EqualString("Accessory"))
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
      
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.Filter_Excel);

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
                    string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Quality_R05_FabricDetail.xltx");
                    sxrc sxr = new sxrc(xltPath);
                    sxrc.XltRptTable dt = new sxrc.XltRptTable(dtFabricDetail);
                    dt.ShowHeader = false;
                    dt.BoAddFilter = true;
                    sxr.DicDatas.Add(sxr.VPrefix + "BODY", dt);

                    sxr.BoOpenFile = true;
                    sxr.Save();
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
                    string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Quality_R05_FabricSummary.xltx");
                    sxrc sxr = new sxrc(xltPath);
                    sxrc.XltRptTable dt = new sxrc.XltRptTable(dtFabricSummary);
                    dt.ShowHeader = false;
                    Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                    wks.Columns.AutoFit();
                    sxr.DicDatas.Add(sxr.VPrefix + "BODY", dt);

                    sxr.Save();
                }

            }
            else if ("Accessory".EqualString(this.comboMaterialType.Text))
            {
                if (radioDetail.Checked) //("Detail".EqualString(this.radioDetail.Text))
                { 
                    // 顯示筆數於PrintForm上Count欄位
                    SetCount(dtAccessoryDetail.Rows.Count);
                    if (dtAccessoryDetail == null || dtAccessoryDetail.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Quality_R05_AccessoryDetail.xltx");
                    sxrc sxr = new sxrc(xltPath);
                    sxrc.XltRptTable dt = new sxrc.XltRptTable(dtAccessoryDetail);
                    dt.ShowHeader = false;
                    dt.BoAddFilter = true;
                    sxr.DicDatas.Add(sxr.VPrefix + "BODY", dt);

                    sxr.BoOpenFile = true;
                    sxr.Save();
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
                    string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Quality_R05_AccessorySummary.xltx");
                    sxrc sxr = new sxrc(xltPath);
                    sxrc.XltRptTable dt = new sxrc.XltRptTable(dtAccessorySummary);
                    dt.ShowHeader = false;
                    Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                    wks.Columns.AutoFit();
                    sxr.DicDatas.Add(sxr.VPrefix + "BODY", dt);

                    sxr.Save();
                }

            }
            return true;
        }

        private void comboMaterialType_SelectedValueChanged(object sender, EventArgs e)
        {
            MaterialType = this.comboMaterialType.SelectedValue.ToString();
        }
    }
}
