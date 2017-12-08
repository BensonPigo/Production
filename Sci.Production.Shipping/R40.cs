using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R40
    /// </summary>
    public partial class R40 : Sci.Win.Tems.PrintForm
    {
        private string contract;
        private string hscode;
        private string nlcode;
        private string sp;
        private bool liguidationonly;
        private DataTable Summary;
        private DataTable WHDetail;
        private DataTable WIPDetail;
        private DataTable ProdDetail;
        private DataTable ScrapDetail;

        /// <summary>
        /// R40
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.checkLiquidationDataOnly.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.txtContractNo.Text))
            {
                this.txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }

            // if (checkLiquidationDataOnly.Checked)
            // {
            //    if (MyUtility.Check.Empty(txtSPNoStartFrom.Text))
            //    {
            //        txtSPNoStartFrom.Focus();
            //        MyUtility.Msg.WarningBox("SP# start from can't empty!!");
            //        return false;
            //    }
            // }
            this.contract = this.txtContractNo.Text;
            this.hscode = this.txtHSCode.Text;
            this.nlcode = this.txtNLCode.Text;
            this.sp = this.txtSPNoStartFrom.Text;
            this.liguidationonly = this.checkLiquidationDataOnly.Checked;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            /*
             * VNContract_Detail.Qty + sum(import數量) - sum(export數量) + sum(adjust數量)
             * export 在 SQL 帶負數
             * import & export Status = confirm
             * adjust Status != new
             */
            sqlCmd.Append(string.Format(
                @"
DECLARE @contract VARCHAR(15)
		,@mdivision VARCHAR(8)
SET @contract = '{0}';
SET @mdivision = '{1}';

--撈合約資料
select 	HSCode
		,NLCode
		,Qty
		,UnitID 
into #tmpContract
from VNContract_Detail WITH (NOLOCK) 
where ID = @contract

--撈進/出口與調整資料
select 	a.NLCode
		,sum(a.Qty) as Qty 
into #tmpDeclare 
from (
	select 	vid.NLCode
			,Qty = round(vid.Qty,6)
	from VNImportDeclaration vi WITH (NOLOCK) 
	inner join VNImportDeclaration_Detail vid WITH (NOLOCK) on vid.ID = vi.ID
	where vi.VNContractID = @contract and vi.Status = 'Confirmed'

	union all
	select 	vcd.NLCode
			, 0 - round(((vcd.Qty * ved.ExportQty)+(vcd.Qty * ved.ExportQty)* vcd.Waste),6)
	from VNExportDeclaration ve WITH (NOLOCK) 
	inner join VNExportDeclaration_Detail ved WITH (NOLOCK) on ved.ID = ve.ID
	inner join VNConsumption vc WITH (NOLOCK) on vc.VNContractID = ve.VNContractID and ved.CustomSP = vc.CustomSP
	inner join VNConsumption_Detail vcd WITH (NOLOCK) on vcd.ID = vc.ID
    inner join VNContract_Detail vctd on vctd.id=vc.VNContractID and vcd.NLCode=vctd.NLCode
	where ve.VNContractID = @contract and ve.Status = 'Confirmed'

	union all
	select 	vcd.NLCode
			,Qty = round(vcd.Qty,6)
	from VNContractQtyAdjust vc WITH (NOLOCK) 
	inner join VNContractQtyAdjust_Detail vcd WITH (NOLOCK) on vc.ID = vcd.ID
	where vc.VNContractID = @contract and vc.Status != 'New'
) a
group by a.NLCode;",
                this.contract,
                Sci.Env.User.Keyword));

            if (this.liguidationonly)
            {
                #region liguidationonly = true
                sqlCmd.Append(@"
select 	isnull(tc.HSCode,'') as HSCode
		,a.NLCode
		,isnull(vcd.DescEN,'') as Description
		,isnull(tc.UnitID,'') as UnitID
		,isnull(tc.Qty,0) + isnull(td.Qty,0) as LiqQty
from (
	select NLCode 
	from #tmpContract 

	union 
	select NLCode 
	from #tmpDeclare
) a
left join #tmpContract tc on a.NLCode = tc.NLCode
left join VNNLCodeDesc vcd WITH (NOLOCK) on a.NLCode = vcd.NLCode
left join #tmpDeclare td on a.NLCode = td.NLCode
where 1 = 1");

                if (!MyUtility.Check.Empty(this.hscode))
                {
                    sqlCmd.Append(string.Format(" and tc.HSCode = '{0}'", this.hscode));
                }

                if (!MyUtility.Check.Empty(this.nlcode))
                {
                    sqlCmd.Append(string.Format(" and a.NLCode = '{0}'", this.nlcode));
                }

                sqlCmd.Append(@"                                                                                                       
order by CONVERT(int,SUBSTRING(a.NLCode,3,3))

drop table #tmpContract;
drop table #tmpDeclare;");
                #endregion
            }
            else
            {
                #region liguidationonly = false
                sqlCmd.Append(string.Format(
                    @"
--撈W/House資料
select 	distinct o.POID
		, o.MDivisionID 
into #tmpPOID
from Orders o WITH (NOLOCK) 
where o.WhseClose is null


select * 
into #tmpWHQty
from (
	select 	isnull(f.HSCode,'') as HSCode
			,isnull(f.NLCode,'') as NLCode
			,t.POID,(fi.Seq1+'-'+fi.Seq2) as Seq
            ,[Refno]=psd.Refno
            ,[Description]=f.Description
			,fi.Roll,fi.Dyelot,fi.StockType
			,isnull((select CONCAT(fid.MtlLocationID,',') 
					 from FtyInventory_Detail fid WITH (NOLOCK) 
					 where fid.Ukey = fi.UKey 
					 for xml path(''))
					,'') as Location
			,IIF(fi.InQty-fi.OutQty+fi.AdjustQty <> 0, dbo.getVNUnitTransfer(isnull(f.Type, '')
																			 ,psd.StockUnit
																			 ,isnull(f.CustomsUnit, '')
																			 ,(fi.InQty-fi.OutQty+fi.AdjustQty)
																			 ,isnull(f.Width,0)
																			 ,isnull(f.PcsWidth,0)
																			 ,isnull(f.PcsLength,0)
																			 ,isnull(f.PcsKg,0)
																			 ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
																			 											,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,'')))
																		 			 ,1)
																			 ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
																			 											,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,'')))
																		 			 ,'')
																			 )
													 , 0) as Qty
    ,[W/House Unit] = psd.POUnit
    ,[W/House Qty(Usage Unit)] = fi.InQty-fi.OutQty+fi.AdjustQty
    ,[Customs Unit] = f.CustomsUnit
	from #tmpPOID t
	inner join FtyInventory fi WITH (NOLOCK) on fi.POID = t.POID --and fi.MDivisionID = t.MDivisionID
	inner join PO_Supp_Detail psd WITH (NOLOCK) on t.POID = psd.ID and psd.SEQ1 = fi.Seq1 and psd.SEQ2 = fi.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
	where (fi.StockType = 'B' or fi.StockType = 'I')

	union all
	select 	isnull(li.HSCode,'') as HSCode
			,isnull(li.NLCode,'') as NLCode
			,l.OrderID as POID,'' as Seq
            ,[RefNo]=l.Refno
            ,[Description]=li.Description
			,''as Roll
			,'' as Dyelot
			,'B' as StockType
			,'' as Location
			,IIF(l.InQty-l.OutQty+l.AdjustQty <> 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
																		 ,l.UnitId
																		 ,li.CustomsUnit
																		 ,(l.InQty-l.OutQty+l.AdjustQty)*IIF(l.UnitId = 'CONE',isnull(li.MeterToCone,0)
																		 													  ,1)
																		 ,0
																		 ,li.PcsWidth
																		 ,li.PcsLength
																		 ,li.PcsKg
																		 ,isnull(IIF(li.CustomsUnit = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
																	 									  ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit))
																	 			 ,1)
																		 ,isnull(IIF(li.CustomsUnit = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
																		 								  ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit))
																		 		 ,''))
												   ,0) as Qty
    ,[W/House Unit] = ''
    ,[W/House Qty(Usage Unit)] = 0.00
    ,[Customs Unit] = ''
	from LocalInventory l WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = l.OrderID
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
	where 1=1
    and o.WhseClose is null
) a

--撈Scrap資料
select * 
into #tmpScrapQty
from (
	select 	isnull(f.HSCode,'') as HSCode
			,isnull(f.NLCode,'') as NLCode
			,t.POID,(mdp.Seq1+'-'+mdp.Seq2) as Seq
			,psd.Refno
			,isnull(f.Type,'') as Type
			,isnull(f.Description,'') as Description
			,psd.StockUnit,mdp.LObQty
			,isnull(f.CustomsUnit,'') as CustomsUnit
			,IIF(mdp.LObQty <> 0,dbo.getVNUnitTransfer(isnull(f.Type,'')
													   ,psd.StockUnit
													   ,isnull(f.CustomsUnit,'')
													   ,mdp.LObQty
													   ,isnull(f.Width,0)
													   ,isnull(f.PcsWidth,0)
													   ,isnull(f.PcsLength,0)
													   ,isnull(f.PcsKg,0)
													   ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
									   															  ,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,'')))
													   		  ,1)
													   ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
													   											  ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),''))
															  ,0) as Qty
	from #tmpPOID t
	inner join MDivisionPoDetail mdp WITH (NOLOCK) on mdp.POID = t.POID --and mdp.MDivisionID = t.MDivisionID
	inner join PO_Supp_Detail psd WITH (NOLOCK) on t.POID = psd.ID and psd.SEQ1 = mdp.Seq1 and psd.SEQ2 = mdp.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
    inner join FtyInventory ft on mdp.Ukey=MDivisionPoDetailUkey
	where 1=1 and ft.StockType='O'
    {0}", this.sp == string.Empty ? string.Empty : string.Format("and t.POID >= '{0}'", this.sp)));
                sqlCmd.Append(string.Format(
                    @"
	union all
	select 	isnull(li.HSCode,'') as HSCode
			,isnull(li.NLCode,'') as NLCode
			,l.OrderID as POID
			,'' as Seq
			,l.Refno
			,isnull(li.Category,'') as Type
			,isnull(li.Description,'') as Description
			,l.UnitId,l.InQty-l.OutQty+l.AdjustQty as LObQty
			,isnull(li.CustomsUnit,'') as CustomsUnit
			,IIF(l.InQty-l.OutQty+l.AdjustQty <> 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
																		 ,l.UnitId,li.CustomsUnit
																		 ,(l.InQty-l.OutQty+l.AdjustQty)*IIF(l.UnitId = 'CONE',isnull(li.MeterToCone,0)
																		 													  ,1)
																		 ,0
																		 ,li.PcsWidth
																		 ,li.PcsLength
																		 ,li.PcsKg
																		 ,isnull(IIF(li.CustomsUnit = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
																		 								  ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit))
																		 		,1)
																		 ,isnull(IIF(li.CustomsUnit = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
																	 									  ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit))
																		  		 ,''))
												  ,0) as Qty
	from LocalInventory l WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = l.OrderID
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
	where 1=1 and o.WhseClose is not null
    {0}", this.sp == string.Empty ? string.Empty : string.Format("and o.ID >= '{0}'", this.sp)));
                sqlCmd.Append(string.Format(
                    @"
) a

--撈已發料數量
select 	o.ID
		,o.MDivisionID
		,o.StyleID
		,o.BrandID
		,o.StyleUKey
		,o.Category
		,o.POID 
into #tmpWHNotClose 
from Orders o  WITH (NOLOCK) 
where o.Category <>''
and o.Finished=0
and o.WhseClose is null
and o.Qty<>0
and o.LocalOrder = 0 



--台北買的物料
select * 
into #tmpIssueQty
from (
	select 	isnull(f.HSCode,'') as HSCode
			,isnull(f.NLCode,'') as NLCode
			,t.ID,(mdp.Seq1+'-'+mdp.Seq2) as Seq
			,psd.Refno,isnull(f.Type,'') as Type
			,isnull(f.Description,'') as Description
			,psd.StockUnit,mdp.OutQty-mdp.LObQty as IssueQty
			,isnull(f.CustomsUnit,'') as CustomsUnit
			,IIF((mdp.OutQty-mdp.LObQty) <> 0,dbo.getVNUnitTransfer(isnull(f.Type,'')
																	,psd.StockUnit
																	,isnull(f.CustomsUnit,'')
																	,(mdp.OutQty-mdp.LObQty)
																	,isnull(f.Width,0)
																	,isnull(f.PcsWidth,0)
																	,isnull(f.PcsLength,0)
																	,isnull(f.PcsKg,0)
																	,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
																											   ,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,'')))
																		    ,1)
																	,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
																											   ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,'')))
																			,''))
				 ,0) as Qty
	from #tmpWHNotClose t
	inner join MDivisionPoDetail mdp WITH (NOLOCK) on mdp.POID = t.ID 
	inner join PO_Supp_Detail psd WITH (NOLOCK) on mdp.POID = psd.ID and psd.SEQ1 = mdp.Seq1 and psd.SEQ2 = mdp.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno

	union all
	select 	isnull(li.HSCode,'') as HSCode
			,isnull(li.NLCode,'') as NLCode
			,t.ID
			,'' as Seq
			,l.Refno
			,isnull(li.Category,'') as Type
			,isnull(li.Description,'') as Description
			,l.UnitId,l.OutQty as IssueQty
			,isnull(li.CustomsUnit,'') as CustomsUnit
			,IIF(l.OutQty <> 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
													 ,l.UnitId
													 ,isnull(li.CustomsUnit,'')
													 ,l.OutQty*IIF(l.UnitId = 'CONE',isnull(li.MeterToCone,0)
													 								,1)
													 ,0
													 ,isnull(li.PcsWidth,0)
													 ,isnull(li.PcsLength,0)
													 ,isnull(li.PcsKg,0)
													 ,isnull(IIF(isnull(li.CustomsUnit,'') = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
													 											 ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = isnull(li.CustomsUnit,'')))
													 		 ,1)
													 ,isnull(IIF(isnull(li.CustomsUnit,'') = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
												 												 ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = isnull(li.CustomsUnit,'')))
													 		,''))
				 ,0) as Qty
	from #tmpWHNotClose t
	inner join LocalInventory l WITH (NOLOCK) on t.ID = l.OrderID 
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
) a

--撈各Style目前最後的CustomSP
select 	v.ID
		,v.CustomSP
		,v.StyleID
		,v.BrandID
		,v.Category 
into #tmpCustomSP
from VNConsumption v WITH (NOLOCK) 
inner join (
	select 	vc.StyleID
			,vc.BrandID
			,vc.Category
			,MAX(vc.CustomSP) as CustomSP
	from VNConsumption vc WITH (NOLOCK) where vc.VNContractID = @contract
	group by vc.StyleID,vc.BrandID,vc.Category
) vc on vc.CustomSP = v.CustomSP
where v.VNContractID = @contract

--撈已Sewing數量
select 	t.ID
		, sdd.ComboType
		, sdd.Article
		,sdd.SizeCode
		,sum(sdd.QAQty) as QAQty
		,isnull(a.HSCode,'') as HSCode
		,isnull(a.NLCode,'') as NLCode
		,isnull(a.UnitID,'') as UnitID
		,sum(isnull(a.Qty ,0)) as Qty
		,isnull(a.CustomSP,'') as CustomSP
		,t.POID
into #tmpWHNotCloseSewingOutput
from #tmpWHNotClose t 
inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = t.ID
left join (
	select 	sdd.OrderId
			,sdd.ComboType
			,sdd.Article
			,sdd.SizeCode
			,sdd.QAQty
			,vd.HSCode
			,vd.NLCode
			,vd.UnitID
			,(ol.rate/100*sdd.QAQty)*vd.Qty as Qty
			,tc.CustomSP
	from #tmpWHNotClose t
	inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = t.ID
	--inner join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = t.StyleUKey and sl.Location = sdd.ComboType
    inner join order_location ol WITH (NOLOCK) on ol.orderid = t.id and ol.location = sdd.combotype
	inner join #tmpCustomSP tc on tc.StyleID = t.StyleID and tc.BrandID = t.BrandID and tc.Category = t.Category
	inner join VNConsumption_Article va WITH (NOLOCK) on va.ID = tc.ID and va.Article = sdd.Article
	inner join VNConsumption_SizeCode vs WITH (NOLOCK) on vs.ID = tc.ID and vs.SizeCode = sdd.SizeCode
	inner join VNConsumption_Detail vd WITH (NOLOCK) on vd.ID = tc.ID
) a on t.ID = a.OrderId and sdd.ComboType = a.ComboType and sdd.Article = a.Article and sdd.SizeCode = a.SizeCode
group by t.ID, sdd.ComboType,sdd.Article,sdd.SizeCode,isnull(a.HSCode,''),isnull(a.NLCode,'')
	,isnull(a.UnitID,''),isnull(a.CustomSP,''),t.POID
order by t.ID, sdd.ComboType,sdd.Article,sdd.SizeCode

--組WIP明細
select 	IIF(ti.ID is null,tw.POID
						 ,ti.ID) as ID
		,IIF(ti.HSCode is null,tw.HSCode
							  ,ti.HSCode) as HSCode
		,IIF(ti.NLCode is null,tw.NLCode
							  ,ti.NLCode) as NLCode
		,(isnull(ti.Qty,0)-isnull(tw.Qty,0)) as Qty
into #tmpWIPDetail
from (
	select 	ID
			,HSCode
			,NLCode
			,SUM(Qty) as Qty 
	from #tmpIssueQty 
	group by ID,HSCode,NLCode
) ti

full outer 
join (
	select 	POID
			,HSCode
			,NLCode
			,SUM(Qty) as Qty 
	from #tmpWHNotCloseSewingOutput 
	group by POID,HSCode,NLCode
) tw on tw.POID = ti.ID and tw.NLCode = ti.NLCode
order by IIF(ti.ID is null,tw.POID
						  ,ti.ID)

--撈尚未Pullout Complete的資料
select 	ID,StyleID
		,BrandID
		,StyleUkey
		,Category
into #tmpNoPullOutComp
from Orders WITH (NOLOCK) 
where Category <>''
and WhseClose is null
and Finished=0
and Qty<>0
and LocalOrder = 0


select 	a.*
		,a.SewQty - a.PullQty as Qty 
into #tmpPreProdQty
from (
	select 	t.ID
			,t.StyleID
			,t.BrandID
			,t.StyleUkey
			,t.Category
			,oq.Article
			,oq.SizeCode
			,isnull(dbo.getMinCompleteSewQty(t.ID,oq.Article,oq.SizeCode),0) as SewQty
			,isnull((select Sum(pdd.ShipQty) 
					 from Pullout_Detail_Detail pdd WITH (NOLOCK)
					 where pdd.OrderID = t.ID  and pdd.Article = oq.Article and pdd.SizeCode = oq.SizeCode)
					,0) as PullQty
	from #tmpNoPullOutComp t
	inner join Order_Qty oq WITH (NOLOCK) on t.ID = oq.ID
) a

select 	t.ID
		,t.Article
		,t.SizeCode
		,t.SewQty
		,t.PullQty
		,isnull(a.HSCode,'') as HSCode
		,isnull(a.NLCode,'') as NLCode
		,isnull(a.UnitID,'') as UnitID
		,isnull(a.Qty ,0) as Qty
		,isnull(a.CustomSP,'') as CustomSP
into #tmpProdQty
from #tmpPreProdQty t 
left join (
	select 	t.ID
			,t.Article
			,t.SizeCode
			,t.SewQty
			,t.PullQty
			,vd.HSCode
			,vd.NLCode
			,vd.UnitID
			,t.qty*vd.Qty as Qty
			,tc.CustomSP
	from #tmpPreProdQty t
	inner join #tmpCustomSP tc on tc.StyleID = t.StyleID and tc.BrandID = t.BrandID and tc.Category = t.Category
	inner join VNConsumption_Article va WITH (NOLOCK) on va.ID = tc.ID and va.Article = t.Article
	inner join VNConsumption_SizeCode vs WITH (NOLOCK) on vs.ID = tc.ID and vs.SizeCode = t.SizeCode
	inner join VNConsumption_Detail vd WITH (NOLOCK) on vd.ID = tc.ID
) a on t.ID = a.ID and t.Article = a.Article and t.SizeCode = a.SizeCode
order by t.ID,t.Article,t.SizeCode,t.SewQty,t.PullQty", this.sp));

                sqlCmd.Append(@"
--整理出Summary
select 	isnull(tc.HSCode,'') as HSCode
		, a.NLCode
		, isnull(vcd.DescEN,'') as Description
		, isnull(tc.UnitID,'') as UnitID
		, isnull(tc.Qty,0) + isnull(td.Qty,0) as LiqQty --調整與勾選Liquidation data only相同
		, isnull(tw.Qty,0) as WHQty
		, isnull(ti.Qty,0) as WIPQty
		, isnull(tp.Qty,0) as ProdQty
		, isnull(ts.Qty,0) as ScrapQty
from (
	select 	NLCode 
	from #tmpContract 

	union 
	select	NLCode 
	from #tmpDeclare 

	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpWHQty
	) t 

	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpWIPDetail
	) t 

	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpProdQty
	) t 

	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpScrapQty
	) t
) a
left join #tmpContract tc on a.NLCode = tc.NLCode
left join VNNLCodeDesc vcd WITH (NOLOCK) on a.NLCode = vcd.NLCode
left join #tmpDeclare td on a.NLCode = td.NLCode
left join (
	select 	NLCode
			,SUM(Qty) as Qty 
	from #tmpWHQty 
	group by NLCode
) tw on a.NLCode = tw.NLCode
left join (
	select 	NLCode
			,SUM(Qty) as Qty 
	from #tmpWIPDetail 
	group by NLCode
) ti on a.NLCode = ti.NLCode
left join (
	select 	NLCode
			,sum(Qty) as Qty 
	from #tmpProdQty 
	group by NLCode
) tp on a.NLCode = tp.NLCode
left join (
	select 	NLCode
			,sum(Qty) as Qty 
	from #tmpScrapQty 
	group by NLCode
) ts on a.NLCode = ts.NLCode
where 1 = 1");

                if (!MyUtility.Check.Empty(this.hscode))
                {
                    sqlCmd.Append(string.Format(" and tc.HSCode = '{0}'", this.hscode));
                }

                if (!MyUtility.Check.Empty(this.nlcode))
                {
                    sqlCmd.Append(string.Format(" and a.NLCode = '{0}'", this.nlcode));
                }

                sqlCmd.Append(string.Format(
                    @"                                                                                                       
order by CONVERT(int,SUBSTRING(a.NLCode,3,3))

--W/H明細
select * from #tmpWHQty where Qty > 0 {0} {1} order by POID,Seq

--WIP明細
select * from #tmpWIPDetail where Qty > 0 {0} {1} order by ID

--Prod明細
select * from #tmpProdQty where Qty > 0 {0} {1} order by ID,Article,SizeCode

--Scrap明細
select * from #tmpScrapQty where Qty > 0 {0} {1} order by POID,Seq", MyUtility.Check.Empty(this.hscode) ? string.Empty : string.Format("and HSCode = '{0}'", this.hscode),
                                                                       MyUtility.Check.Empty(this.nlcode) ? string.Empty : string.Format("and NLCode = '{0}'", this.nlcode)));
                #endregion
            }
            #endregion

            DataSet allData;

            if (!SQL.Selects(string.Empty, sqlCmd.ToString(), out allData))
            {
                DualResult failResult = new DualResult(false, "Query data fail");
                return failResult;
            }

            this.Summary = allData.Tables[0];

            if (!this.liguidationonly)
            {
                this.WHDetail = allData.Tables[1];
                this.WIPDetail = allData.Tables[2];
                this.ProdDetail = allData.Tables[3];
                this.ScrapDetail = allData.Tables[4];
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.Summary.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.Summary.Rows.Count + (this.liguidationonly ? 0 : this.WHDetail.Rows.Count + this.WIPDetail.Rows.Count + this.ProdDetail.Rows.Count + this.ScrapDetail.Rows.Count));

            this.ShowWaitMessage("Starting EXCEL...");
            bool result;
            this.ShowWaitMessage("Starting EXCEL...Summary");

            if (!this.liguidationonly)
            {
                result = MyUtility.Excel.CopyToXls(this.Summary, string.Empty, xltfile: "Shipping_R40_Summary.xltx", headerRow: 1);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                }

                if (this.WHDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...WHouse Qty Detail");
                    result = MyUtility.Excel.CopyToXls(this.WHDetail, string.Empty, xltfile: "Shipping_R40_WHQtyDetail.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.WIPDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...WIP Qty Detail");
                    result = MyUtility.Excel.CopyToXls(this.WIPDetail, string.Empty, xltfile: "Shipping_R40_WIPQtyDetail.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.ProdDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Prod. Qty Detail");
                    result = MyUtility.Excel.CopyToXls(this.ProdDetail, string.Empty, xltfile: "Shipping_R40_ProdQtyDetail.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.ScrapDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Scrap Qty Detail");
                    result = MyUtility.Excel.CopyToXls(this.ScrapDetail, string.Empty, xltfile: "Shipping_R40_ScrapQtyDetail.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.ProdDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Prod. Qty Detail");
                    result = MyUtility.Excel.CopyToXls(this.ProdDetail, string.Empty, xltfile: "Shipping_R40_ProdQtyDetail.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.ScrapDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Scrap Qty Detail");
                    result = MyUtility.Excel.CopyToXls(this.ScrapDetail, string.Empty, xltfile: "Shipping_R40_ScrapQtyDetail.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }
            }
            else
            {
                result = MyUtility.Excel.CopyToXls(this.Summary, string.Empty, xltfile: "Shipping_R40_Summary(Only Liquidation).xltx", headerRow: 1);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                }
            }

            this.HideWaitMessage();
            return true;
        }

        private void TxtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"select id,startdate,EndDate from [Production].[dbo].[VNContract]", "20,10,10", this.Text, false, ",", headercaptions: "Contract No, Start Date, End Date");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtContractNo.Text = item.GetSelectedString();
        }
    }
}
