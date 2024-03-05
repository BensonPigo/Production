﻿using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_Print : Win.Tems.PrintForm
    {
        private DataTable dt;
        private DataRow CurrentDataRow;
        private string order_by = string.Empty;
        private string junk_where = string.Empty;

        /// <inheritdoc/>
        public P03_Print(DataRow row, int sort_by, bool chk_includeJunk)
        {
            this.InitializeComponent();
            if (sort_by == 0)
            {
                this.order_by = " order by  a.[Ref#] ,a.[sp] , iif(a.Material_Type='Fabric',1,iif(a.Material_Type='Accessory',2,3)), a.[Color]";
            }
            else
            {
                this.order_by = " order by a.sp,a.[SEQ]";
            }

            if (chk_includeJunk == false)
            {
                this.junk_where = " where a.junk <> 'true' ";
            }

            this.CurrentDataRow = row;
            this.print.Visible = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));

            // string xlt;
            DualResult result;
            string sqlcmd = string.Empty;
            if (this.radioPanel1.Value == this.radioMaterialStatus.Value)
            {
                // xlt = @"Warehouse_P03_Print-1.xltx";
                // PO_Supp_tmp為了Chinese Abb Fabric_Supp 的suppID如果找不到，請找 PO_Supp_Detail.SCIRefno seq1最小的且suppID在 Fabric_Supp是有資料的那筆
                sqlcmd = $@"
--declare @ID varchar(20) ='23011761GG'
Select  c.InvNo InvNo
        ,a.POID POID
        ,a.SEQ1 SEQ1
        ,a.SEQ2 SEQ2
        , CASE 
	        when a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1 and a.nonOdor=1 then 'N/A'
	        when isnull(a.result,'')='' then 'Blank'
	        else a.result
	        END as [Result]
into #tmpQA
from dbo.FIR a WITH (NOLOCK) 
left join [dbo].[View_AllReceiving] c WITH (NOLOCK) on c.Id = a.ReceivingID
where   a.POID LIKE @id
UNION all
Select   c.InvNo InvNo
        ,a.POID POID
        ,a.SEQ1 SEQ1
        ,a.SEQ2 SEQ2
        , CASE 
	        when isnull(a.result,'')='' then 'Blank'
	        else a.result
	        END as [Result] 
from dbo.AIR a WITH (NOLOCK) 
left join [dbo].[View_AllReceiving] c WITH (NOLOCK) on c.Id = a.ReceivingID
where   a.POID like @id 
        and a.Result !=''

select psd.ID,
       psd.SCIRefno,
       SuppID = FIRST_VALUE(ps.SuppID) OVER (partition by psd.SCIRefno ORDER BY ps.SEQ1),
       psd.SEQ1,
       psd.SEQ2,
       fs.AbbCH
into #PO_Supp_tmp
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
left join PO_Supp ps WITH (NOLOCK) on ps.id=psd.id and ps.SEQ1=psd.SEQ1
left join Fabric_Supp fs on fs.SCIRefno = psd.SCIRefno and fs.SuppID = ps.SuppID 
where psd.id=@ID

select * 
INTO #tmp
from (
    select a.id [sp]
        ,b.StyleID
        ,a.SEQ1+a.SEQ2 [SEQ]
        ,c.SuppID [Supp]
        ,d.NameEN [Supp Name]
        ,substring(convert(varchar,a.cfmetd, 101),1,5) [Sup. 1st Cfm ETA]
        ,substring(convert(varchar,a.RevisedETA, 101),1,5) [RevisedETD]
        ,a.Refno [Ref#]
        ,dbo.getMtlDesc(a.id,a.SEQ1,a.SEQ2,2,0) [Description]
        ,iif(e.AbbCH is null, j.AbbCH, e.AbbCH) [Chinese Abb]
        --,f.HsCode [HS Code]
        ,[HS Code]= FabricHsCode.value
        ,concat(mt.fabrictype2,'-',Fabric.MtlTypeID) Material_Type
        ,Fabric.WeaveTypeID
        ,MTL.ProductionType
        ,dbo.GetColorMultipleID(b.BrandID,isnull(psdsC.SpecValue, '')) [Color]
        ,psdsS.SpecValue [Size]
        ,h.Currencyid [Currency]
        ,format(a.UsedQty,'#,###,###,###.####')  [Qty]
        ,format(a.Qty,'#,###,###,###.##')  [Order Qty]
        ,format(a.NETQty,'#,###,###,###.##') [Net Qty]
        ,format(a.NETQty+a.LossQty,'#,###,###,###.##') [Use Qty]
        ,format(a.ShipQty,'#,###,###,###.##') [Ship Qty]
        ,format(a.ShipFOC,'#,###,###,###.##') [F.O.C]
        ,format(a.ApQty,'#,###,###,###.##') [AP Qty]
        ,IIF(EXISTS(SELECT * FROM DBO.Export_Detail g WITH (NOLOCK) 
        WHERE g.PoID = a.id
        AND g.SEQ1 = a.seq1
        AND g.SEQ2 =a.seq2
        AND IsFormA = 1),'Y','') [FormA]
        ,format(a.InputQty,'#,###,###,###.##') [Taipei Stock Qty]
        ,a.POUnit [POUnit]
        ,a.Complete [Cmplt]
        ,format(a.FinalETA,'yyyy/MM/dd') [Act. Eta]
        ,(select id+',' from 
        (select distinct id from export_detail WITH (NOLOCK)  where poid =a.id and seq1=a.seq1 and seq2=a.seq2) t for xml path(''))  [WK#]
        ,(select orderid+',' from 
        (select ol.orderid  from PO_Supp_Detail_OrderList ol WITH (NOLOCK)  where id =a.id and seq1=a.seq1 and seq2=a.seq2) ol for xml path(''))  [Order List]
        ,i.InQty [Arrived Qty]
        ,a.StockUnit [StockUnit]
        ,i.OutQty [Released Qty]
        ,i.AdjustQty [Adjust Qty]
        ,i.ReturnQty [Return Qty]
        ,i.InQty-i.OutQty+i.AdjustQty-i.ReturnQty [Balance]
        ,format(i.LInvQty,'###,###,###.##') [Stock Qty]
        ,format(i.LObQty,'###,###,###.##') [Scrap Qty]
        ,i.ALocation [Bulk Location]
        ,i.BLocation [Stock Location]
        ,[InspectionGroup] = Fabric.InspectionGroup
        ,[FIR]= stuff((select Concat(',',t.Result) from ( SELECT seq1,seq2,Result 
                                                          FROM #tmpQA 
                                                          where   poid = a.id 
                                                                and seq1 = a.seq1 
                                                                and seq2 = a.seq2 
                                                         )t order by t.seq1,t.seq2  for xml path('')),1,1,'') 
        ,(select Remark+',' from 
        (select r.Remark  from dbo.Receiving_Detail r WITH (NOLOCK) where POID =a.id and seq1=a.seq1 and seq2=a.seq2 and remark !='') r for xml path('')) [Remark]
        ,a.junk
    from dbo.PO_Supp_Detail a WITH (NOLOCK) 
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = a.id and psdsC.seq1 = a.seq1 and psdsC.seq2 = a.seq2 and psdsC.SpecColumnID = 'Color'
    left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = a.id and psdsS.seq1 = a.seq1 and psdsS.seq2 = a.seq2 and psdsS.SpecColumnID = 'Size'
    left join View_WH_Orders b WITH (NOLOCK) on a.id=b.id
    left join dbo.PO_Supp c WITH (NOLOCK) on c.id=a.id and c.SEQ1=a.SEQ1
    left join Fabric with(nolock) on Fabric.SCIRefno = a.SCIRefno
    left join dbo.supp d WITH (NOLOCK) on d.id=c.SuppID
    left join dbo.Fabric_Supp e WITH (NOLOCK) on e.SCIRefno=a.SCIRefno and e.SuppID=c.SuppID
    left join dbo.supp h WITH (NOLOCK) on h.id=c.SuppID
    left join dbo.MDivisionPoDetail i WITH (NOLOCK) on i.POID=a.ID and a.SEQ1=i.Seq1 and a.SEQ2=i.Seq2
    left join #PO_Supp_tmp j on a.ID = j.ID and a.SEQ1 = j.SEQ1 and a.SEQ2 = j.SEQ2
    LEFT JOIN MtlType MTL WITH (NOLOCK) ON MTL.ID = fabric.MtlTypeID
    outer apply(select fabrictype2 = case a.FabricType 
	    when 'F' then 'Fabric'
	    when 'A'then 'Accessory'
	    else a.FabricType
	    end )mt
    outer apply(
        select top 1 [value] =IIF(
            (
	            SELECT TOP 1 et.ECFA FROM Export et
	            INNER JOIN Export_Detail g WITH (NOLOCK)  ON et.ID= g.id
	            WHERE g.PoID = a.id
	            AND g.SEQ1 = a.seq1
	            AND g.SEQ2 =a.seq2
            ) =1
	            AND e.IsECFA=1
            ,f.HScode--是ECFA
            , ---不是ECFA
	        IIF ( NOT EXISTS(SELECT 1 FROM Fabric_HsCode WHERE SCIRefno=f.SCIRefno AND SuppID=f.SuppID  AND Year=f.Year AND HsType=2),f.HScode,f.HSCodeT2 ))
        from Fabric_HsCode f
        where f.SCIRefno=a.SCIRefno and f.SuppID=c.SuppID and f.year=year(a.ETA) 
    )FabricHsCode
    where a.id=@ID  

    union all
    select 
        [sp] = o.POID
        , [StyleID] = '-'
        , [SEQ] = '-'
        , [Supp]  = c.ID 
        , [Supp Name] = '-'
        , [Sup. 1st Cfm ETA] = '-'
        , [RevisedETD] = '-'
        , [Ref#]		= l.Refno
        , [description]  = b.Description
        , [Chinese Abb] = '-'
        , [HS Code] = '-'
        , [Material_Type] = l.UnitID
        , [WeaveTypeID] = ''
        , [ProductionType] = ''
        , [Color] = l.ThreadColorID
        , [Size] = '-'
        , [Currency] = '-'
        , [Qty] = null
        , [Order Qty]  = '-'
        , [Net Qty] = '-'
        , [Use Qty] = '-'
        , [Ship Qty] = '-'
        , [F.O.C] = '-'
        , [AP Qty] = '-'
        , [FormA] = '-'
        , [Taipei Stock Qty] = '-'
        , [POUnit] = '-'
        , [Cmplt] = null
        , [Act. Eta] = '-'
        , [WK#] = '-'
        , [Order List] =  l.OrderID
        , [Arrived Qty] = isnull(l.InQty,0)
        , [StockUnit]  = l.UnitID
        , [Released Qty] = isnull(l.OutQty,0)
        , [Adjust Qty] =isnull( l.AdjustQty,0)
        , [Return Qty] = 0
        , [Balance]  = isnull(InQty - OutQty + AdjustQty,0)
        , [Stock Qty] = '-'
        , [Scrap Qty] = '-'
        , [Bulk Location] = l.ALocation
        , [Stock Location] = '-'
        , [InspectionGroup] = '-'
        , [FIR] = '-'
        , [Remark] = '-'
        , [junk]  = 'false'
    from LocalInventory l
    left join View_WH_Orders o on o.id = l.orderid
    left join LocalItem b on l.Refno=b.RefNo
    left join LocalSupp c on b.LocalSuppid=c.ID
    where l.OrderID = @ID
) as a 
{this.junk_where}
{this.order_by}

select 
	[sp] 
	, [StyleID] 
	, [SEQ] 
	, [Supp] 
	, [Supp Name] 
	, [Sup. 1st Cfm ETA]
	, [RevisedETD] 
	, [Ref#]	
	, [description]  
	, [Chinese Abb] 
	, [HS Code] 
	, [Material_Type] 
    , [WeaveTypeID]
    , [ProductionType]
	, [Color] 
	, [Size] 
	, [Currency] 
	, [Qty] 
	, [Order Qty] 
	, [Net Qty] 
	, [Use Qty] 
	, [Ship Qty] 
	, [F.O.C]
	, [AP Qty]
	, [FormA]
	, [Taipei Stock Qty]
	, [POUnit]
	, [Cmplt]
	, [Act. Eta]
	, [WK#]
	, [Order List]
	, [Arrived Qty]
	, [Received Rate(%)]=IIF( CanINT.val IS NOT NULL AND CanINT2.val IS NOT NULL
							, CAST(ROUND( ([Arrived Qty] / REPLACE([Ship Qty],',','') * 100),2) AS varchar) 
							, '-')
	, [StockUnit] 
	, [Released Qty]
	, [Adjust Qty]
    , [Return Qty]
	, [Balance]
	, [Stock Qty]
	, [Scrap Qty]
	, [Bulk Location]
	, [Stock Location]
    , [InspectionGroup]
	, [FIR]
	, [Remark]
	, [junk]
into #lasttmp
from #tmp
OUTER APPLY(
	SELECT val= (TRY_CONVERT(decimal, REPLACE([Ship Qty],',','')))
)CanINT
OUTER APPLY(
	SELECT val= (TRY_CONVERT(decimal, [Arrived Qty]))
)CanINT2

declare @pivotColumnsName nvarchar(max) = stuff((select concat(',[',Name,']') from BomType order by Seq for xml path('')),1,1,'')
declare @columnsName nvarchar(max) = stuff((select concat(',[Spec', char(10), Name,'] = s.[',Name,']') from BomType order by Seq for xml path('')),1,1,'')
declare @sqlcmd nvarchar(max) =N'
select poid, seq = Seq1 + Seq2,' + @pivotColumnsName + N'
into #spectmp
From(
    Select poid = psds.id,
           psds.seq1,
           psds.seq2,
           Name,
           psds.SpecValue
    from  BomType bt
    left join PO_Supp_Detail_Spec psds on  psds.SpecColumnID = bt.ID 
    where psds.id = ''' + @ID + N'''
)bt
pivot(min(SpecValue) for Name in (' + @pivotColumnsName + N')) as pt

select t.*,' + @columnsName + N'
from #lasttmp t
left join #spectmp s on s.poid = t.sp and s.seq = t.seq
'
print @sqlcmd
exec (@sqlcmd)

DROP TABLE #tmp, #PO_Supp_tmp, #lasttmp
";
            }
            else
            {
                sqlcmd = @"select * from (
                                            select a.id [sp]
                                              ,b.StyleID [style]
                                              ,a.SEQ1+a.SEQ2 [SEQ]
                                              ,[Desc]=dbo.getMtlDesc(a.id,a.SEQ1,a.seq2,2,0)
                                              ,chinese_abb=d.AbbCH
			                                  ,concat(mt.fabrictype2,'-',Fabric.MtlTypeID) Material_Type
                                              ,[WeaveTypeID] = Fabric.WeaveTypeID
                                              ,MTL.ProductionType
                                              --,Hs_code=e.HsCode
                                              ,[HS Code]= FabricHsCode.value
                                              ,supp=c.SuppID
                                              ,Supp_Name=f.AbbEN
                                              ,Currency=f.Currencyid
                                              ,Del=substring(convert(varchar, a.cfmetd, 101),1,5)
                                              ,Used_Qty=format(a.UsedQty,'#,###,###,###.##')
                                              ,Order_Qty=format(a.qty ,'#,###,###,###.##')
                                              ,Taipei_Stock= format(a.InputQty,'#,###,###,###.##')
                                              ,Unit=a.POUnit 
                                              ,TTL_Qty= format(a.ShipQty,'#,###,###,###.##')
                                              ,FOC=format(a.FOC,'#,###,###,###.##')
                                              ,ty= format( iif( isnull(a.Qty,0)=0,100,a.shipqty/a.qty*100),'##,###.##') 
                                              ,OK=a.Complete
                                              ,Exp_Date=format(a.eta,'yyyy/MM/dd') 
                                              ,FormA=IIF(EXISTS(SELECT * FROM DBO.Export_Detail g WITH (NOLOCK) 
                                                    WHERE g.PoID =a.id
                                                    AND g.SEQ1 = a.seq1
                                                    AND g.SEQ2 = a.seq2
                                                    AND IsFormA = 1)
                                              ,'Y','')
                                             ,a.junk
                                       from dbo.PO_Supp_Detail a WITH (NOLOCK) 
                                       left join View_WH_Orders b WITH (NOLOCK) on a.id=b.id
                                       left join dbo.PO_Supp c WITH (NOLOCK) on a.id=c.id and a.SEQ1=c.SEQ1
                                       left join Fabric with(nolock) on Fabric.SCIRefno = a.SCIRefno
                                       left join dbo.Fabric_Supp d WITH (NOLOCK) on d.SCIRefno=a.SCIRefno and d.SuppID=c.SuppID
                                       left join dbo.Supp f WITH (NOLOCK) on f.id=c.SuppID
                                       LEFT JOIN MtlType MTL WITH (NOLOCK) ON MTL.ID = fabric.MtlTypeID
                                        outer apply(select fabrictype2 = case a.FabricType 
			                                            when 'F' then 'Fabric'
			                                            when 'A'then 'Accessory'
			                                            else a.FabricType
			                                            end )mt
                                        outer apply(
		                                          select top 1 [value] = IIF(
                                                        (
	                                                        SELECT TOP 1 et.ECFA FROM Export et
	                                                        INNER JOIN Export_Detail g WITH (NOLOCK)  ON et.ID= g.id
	                                                        WHERE g.PoID = a.id
	                                                        AND g.SEQ1 = a.seq1
	                                                        AND g.SEQ2 =a.seq2
                                                        ) =1
                                                        AND d.IsECFA=1
                                                        ,e.HScode--是ECFA
				                                        , ---不是ECFA
	                                                        IIF ( NOT EXISTS(SELECT 1 FROM Fabric_HsCode WHERE SCIRefno=e.SCIRefno AND SuppID=e.SuppID  AND Year=e.Year AND HsType=2),e.HScode,e.HSCodeT2 ))
		                                          from Fabric_HsCode e
		                                          where e.SCIRefno=a.SCIRefno and e.SuppID=c.SuppID and e.year=year(a.ETA) 
	                                        )FabricHsCode
                                       where a.id=@ID 
                                        union all
                                    select 
                                         [sp] = l.OrderID
                                    	 , [StyleID] = '-'
                                        , [SEQ] = '-'
                                    	 , [description]  = b.Description
                                    	 , [Chinese Abb] = '-'
                                    	 , [Material_Type] = l.UnitID
                                         , [WeaveTypeID] = ''
                                         , [ProductionType] = ''
                                    	 , [HS Code] = '-'
                                    	 , [Supp]  = c.ID 
                                    	 , [Supp Name] = '-'
                                    	 , [Currency] = '-'
                                    	 , [Del] = '-'
                                    	 , [Used_Qty] = '-'
                                    	 , [Order Qty]  = '-'
                                        , [Taipei Stock Qty] = '-'
                                    	 , [POUnit] = '-'
                                    	 , [TTL_Qty] = '-'
                                    	 , [F.O.C] = '-'
                                    	 , [ty] = '-'
                                    	 , [OK] = 'true'
                                    	 , [Exp_Date] = '-'
                                    	 , [FormA] = '-'
                                    	 , [junk] = 'false'
                                    	  from LocalInventory l
                                        left join LocalItem b on l.Refno=b.RefNo
                                        left join LocalSupp c on b.LocalSuppid=c.ID
                                         where l.OrderID  = @ID    ) as a   " + this.junk_where + " order by a.sp,a.[SEQ]";
            }

            // SaveXltReportCls xl = new SaveXltReportCls(xlt);
            // xl.dicDatas.Add("##sp", dt);
            // xl.Save(outpa, false);
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out this.dt);
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);
            if (this.radioPanel1.Value == this.radioMaterialStatus.Value)
            {
                Excel.Application objApp = new Excel.Application();
                Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_P03_Print-1.xltx", objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    if (this.dt.Rows[i]["junk"].ToString().Equals("True"))
                    {
                        worksheet.Range[worksheet.Cells[1][i + 2], worksheet.Cells[43][i + 2]].Interior.ColorIndex = 15;
                    }
                }

                this.dt.Columns.Remove("Junk");
                for (int i = this.dt.Columns.IndexOf("Remark"); i < this.dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = this.dt.Columns[i].ColumnName;
                    worksheet.Cells[1, i + 1].Interior.Color = Color.FromArgb(204, 255, 204);
                    worksheet.Cells[1, i + 1].Borders.Weight = 2;
                }

                com.WriteTable(this.dt, 2);
                com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
                objApp.Visible = true;
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);
            }
            else
            {
                Excel.Application objApp = new Excel.Application();
                Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_P03_Print-2.xltx", objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    if (this.dt.Rows[i]["junk"].ToString().Equals("True"))
                    {
                        worksheet.Range[worksheet.Cells[1][i + 2], worksheet.Cells[22][i + 2]].Interior.ColorIndex = 15;
                    }
                }

                com.WriteTable(this.dt, 2);
                com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
                worksheet.Columns[23].Delete();
                objApp.Visible = true;
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);
            }

            return true;
        }
    }
}