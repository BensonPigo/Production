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
        private DataTable OnRoadMaterial;
        private DataTable WHDetail;
        private DataTable WIPDetail;
        private DataTable ProdDetail;
        private DataTable OnRoadProduction;
        private DataTable ScrapDetail;
        private DataTable Outstanding;

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

            this.contract = this.txtContractNo.Text;
            this.hscode = this.txtHSCode.Text;
            this.nlcode = this.txtNLCode.Text;
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
                sqlCmd.Append(
                    @"
-- 1)在途物料(已報關但還在途)(On Road Material Qty新增報表)
select * 
into #tmpOnRoadMaterial
from 
(
	select 
	 [HSCode] = f.HSCode
	,[NLCode] = f.NLCode
	,[PoID] = ed.PoID
	,[Seq] = ed.Seq1+'-'+ed.Seq2
	,[refno] = ed.refno
	,[Description] = ed.Description
	,[Qty] =( dbo.getVNUnitTransfer(isnull(f.Type, '')
		,dbo.getStockUnit(psd.SciRefno,ed.Suppid)
		,isnull(f.CustomsUnit, '')
		,(ed.qty+ed.foc)
		,isnull(f.Width,0)
		,isnull(f.PcsWidth,0)
		,isnull(f.PcsLength,0)
		,isnull(f.PcsKg,0)
		,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
			(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),
			(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit 
			and TO_U = isnull (f.CustomsUnit,''))),1)
		,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
			(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),
			(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit 
			and TO_U = isnull(f.CustomsUnit,''))),'')
	))
	,[CustomsUnit] = f.CustomsUnit
	,[OnRaodQty] = dbo.getUnitRate(psd.PoUnit,dbo.getStockUnit(psd.SciRefno,ed.Suppid))*(ed.qty+ed.foc)
	,[StockUnit] = dbo.getStockUnit(psd.SciRefno,ed.Suppid)				
	from Export e WITH (NOLOCK) 
	inner join Export_Detail ed WITH (NOLOCK) on e.id=ed.id
	inner join VNImportDeclaration vd WITH (NOLOCK) on vd.BLNo=e.BLNo 
	inner join po_supp_detail psd WITH (NOLOCK) on psd.ID=ed.poid and psd.seq1=ed.seq1 and psd.seq2=ed.seq2
	inner join Fabric f WITH (NOLOCK) on f.SciRefno=psd.SciRefno										
	where vd.VNContractID = @contract
	and vd.Status='Confirmed'
	and not exists (select 1 from Receiving WITH (NOLOCK)
		where exportID = e.id and status='Confirmed')
	and vd.blno<>''

union all
	
	select distinct
	 [HSCode] = isnull(isnull(f.HSCode,li.HSCode),'')
	,[NLCode] = isnull(isnull(f.NLCode,li.NLCode),'')
	,[POID] = fed.PoID
	,[Seq] = fed.Seq1+'-'+fed.Seq2
	,[RefNo] = fed.refno
	,[Description] = isnull(isnull(f.Description, li.Description),'')
	,[Qty] =( dbo.getVNUnitTransfer(isnull(li.Category, '')
		,StockUnit.unit
		,isnull(li.CustomsUnit, '')
		,fed.qty * IIF(fed.UnitId = 'CONE',isnull(li.MeterToCone,0),1)
		,0
		,isnull(li.PcsWidth,0)
		,isnull(li.PcsLength,0)
		,isnull(li.PcsKg,0)
		,isnull(IIF(isnull(li.CustomsUnit, '') = 'M2',
			(select RateValue from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) and TO_U = 'M'),
			(select RateValue from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) 
			and TO_U = isnull (li.CustomsUnit,''))),1)
		,isnull(IIF(isnull(li.CustomsUnit, '') = 'M2',
			(select Rate from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) and TO_U = 'M'),
			(select Rate from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) and TO_U = isnull(f.CustomsUnit,''))),'')
	))
	,[CustomsUnit] = isnull(isnull(f.CustomsUnit,li.CustomsUnit),'')
	,[OnRaodQty] = isnull(UnitRateQty.qty,0)
	,[StockUnit] = isnull(StockUnit.unit,'')
	from FtyExport fe WITH (NOLOCK)
	inner join FtyExport_Detail fed WITH (NOLOCK) on fe.id=fed.id	
	left join Fabric f WITH (NOLOCK) on f.SciRefno=fed.SciRefno			
	left join LocalItem li WITH (NOLOCK) on li.Refno = fed.RefNo
	outer apply(
		select unit = iif(fe.type in (2,3),dbo.getStockUnit(fed.SciRefno,fed.SuppID),iif(fe.type in (1,4),li.UnitID,''))
	) StockUnit		
	outer apply(
		select Qty = iif(fe.type in (2,3),dbo.getUnitRate(fed.UnitID,StockUnit.unit)*(fed.qty),
		iif(fe.type in (1,4),fed.qty,0))
	) UnitRateQty							
	where 1=1 
    and exists (select 1 from VNImportDeclaration WITH (NOLOCK) 
        where (blno=fe.blno or wkno=fe.id) 
		and blno<>'' and vncontractid=@contract )	
	and not exists (select 1 from Receiving WITH (NOLOCK)
		where InvNo = fe.InvNo and status='Confirmed')
	and not exists (select 1 from TransferIn WITH (NOLOCK)
		where InvNo = fe.InvNo and status='Confirmed')				
	and not exists (select 1 from LocalReceiving WITH (NOLOCK)
		where InvNo = fe.InvNo and status='Confirmed')
) a
-- end --
				

--撈W/House資料
select 	distinct o.POID
		, o.MDivisionID 
into #tmpPOID
from Orders o WITH (NOLOCK) 
where o.WhseClose is null

-- 2) 料倉(AB)( W/House Qty Detail)
select * 
into #tmpWHQty
from (
		select 
	 [HSCode] = isnull(f.HSCode,'')
	,[NLCode] = isnull(f.NLCode,'')
	,[POID] = fi.POID
	,[Seq] = (fi.Seq1+'-'+fi.Seq2)
	,[Refno] = psd.Refno
	,[Description] = f.Description
	,[Roll] = fi.Roll
	,[Dyelot] = fi.Dyelot
	,[StockType] = fi.StockType
	,[Location] = isnull(
		(select CONCAT(fid.MtlLocationID,',') 
		from FtyInventory_Detail fid WITH (NOLOCK) 
		where fid.Ukey = fi.UKey 
		for xml path(''))
		,'')
	,[Qty] = IIF(fi.InQty-fi.OutQty+fi.AdjustQty > 0, dbo.getVNUnitTransfer(
			isnull(f.Type, '')
			,psd.StockUnit
			,isnull(f.CustomsUnit, '')
			,(fi.InQty-fi.OutQty+fi.AdjustQty)
			,isnull(f.Width,0)
			,isnull(f.PcsWidth,0)
			,isnull(f.PcsLength,0)
			,isnull(f.PcsKg,0)
			,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
				(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
			,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
				(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),''))
			, 0)
	,[W/House Unit] = f.CustomsUnit
	,[W/House Qty(Stock Unit)] = fi.InQty-fi.OutQty+fi.AdjustQty
	,[Stock Unit] = psd.StockUnit
	from FtyInventory fi WITH (NOLOCK)  --EDIT
	left join PO_Supp_Detail psd WITH (NOLOCK) on fi.POID = psd.ID and psd.SEQ1 = fi.Seq1 and psd.SEQ2 = fi.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
	where (fi.StockType = 'B' or fi.StockType = 'I')
	and fi.InQty-fi.OutQty+fi.AdjustQty>0 

union all

select distinct
	 [HSCode] = isnull(li.HSCode,'') 
	,[NLCode] = isnull(li.NLCode,'') 
	,[POID] = l.OrderID
	,[Seq] = ''
	,[RefNo] = l.Refno
	,[Description] = li.Description
	,[Roll] = '' 
	,[Dyelot] = '' 
	,[StockType] = 'B'
	,[Location] = '' 
	,[Qty] = IIF(l.InQty-l.OutQty+l.AdjustQty > 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
		,l.UnitId
		,li.CustomsUnit
		,(l.InQty-l.OutQty+l.AdjustQty)*IIF(l.UnitId = 'CONE',isnull(li.MeterToCone,0),1)
		,0
		,li.PcsWidth
		,li.PcsLength
		,li.PcsKg
		,isnull(IIF(li.CustomsUnit = 'M2',
			(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),1)
		,isnull(IIF(li.CustomsUnit = 'M2',
			(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),''))
		,0)
	,[W/House Unit] = li.CustomsUnit
	,[W/House Qty(Usage Unit)] =l.InQty-l.OutQty+l.AdjustQty
	,[Customs Unit] = l.UnitId
	from LocalInventory l WITH (NOLOCK) 	
	inner join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
	where l.InQty-l.OutQty+l.AdjustQty > 0	
) a


--撈已發料數量
select 	o.ID
		,o.MDivisionID
		,o.StyleID
		,o.BrandID
		,o.StyleUKey
		,o.Category
		,o.SeasonID
		,o.POID 
into #tmpWHNotClose 
from Orders o  WITH (NOLOCK) 
inner join VNConsumption vn WITH (NOLOCK) on vn.Styleid=o.Styleid 
and vn.SeasonID = o.SeasonID and vn.BrandID=o.BrandID and vn.category=o.category
where o.Category <>''
and o.Finished=0
and o.WhseClose is null
and o.Qty<>0
and o.LocalOrder = 0 
and vn.VNContractID=@contract


 -- 3) WIP在製品(WIP Qty Detail) 
--台北買的物料
select * 
into #tmpIssueQty
from 
(
	select 		 
	 [HSCode] = isnull(f.HSCode,'')
	,[NLCode] = isnull(f.NLCode,'')
	,[ID] = t.ID
	,[Qty] = IIF((mdp.OutQty-mdp.LObQty) > 0,dbo.getVNUnitTransfer(isnull(f.Type,'')
		,psd.StockUnit
		,isnull(f.CustomsUnit,'')
		,(mdp.OutQty-mdp.LObQty)
		,isnull(f.Width,0)
		,isnull(f.PcsWidth,0)
		,isnull(f.PcsLength,0)
		,isnull(f.PcsKg,0)
		,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',
			(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
			,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
		,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',
			(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
			,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),''))
		,0)
	from #tmpWHNotClose t
	inner join MDivisionPoDetail mdp WITH (NOLOCK) on mdp.POID = t.ID 
	inner join PO_Supp_Detail psd WITH (NOLOCK) on mdp.POID = psd.ID and psd.SEQ1 = mdp.Seq1 and psd.SEQ2 = mdp.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno

union all

	select 
	 [HSCode] = isnull(li.HSCode,'')
	,[NLCode] = isnull(li.NLCode,'')
	,[ID] = t.ID
	,[Qty] = IIF(l.OutQty > 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
		,l.UnitId
		,isnull(li.CustomsUnit,'')
		,l.OutQty*IIF(l.UnitId = 'CONE',isnull(li.MeterToCone,0),1)
		,0
		,isnull(li.PcsWidth,0)
		,isnull(li.PcsLength,0)
		,isnull(li.PcsKg,0)
		,isnull(IIF(isnull(li.CustomsUnit,'') = 'M2',
			(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = isnull(li.CustomsUnit,''))),1)
		,isnull(IIF(isnull(li.CustomsUnit,'') = 'M2',
			(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = isnull(li.CustomsUnit,''))),''))
		,0)
	from #tmpWHNotClose t
	inner join LocalInventory l WITH (NOLOCK) on t.ID = l.OrderID 
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
) a

--撈各Style目前最後的CustomSP
select 	 v.ID
		,v.CustomSP
		,v.StyleID
		,v.BrandID
		,v.Category 
		,v.SizeCode	
		,v.SeasonID
into #tmpCustomSP
from VNConsumption v WITH (NOLOCK) 
inner join (
	select 	vc.StyleID
			,vc.BrandID
			,vc.Category
            ,vc.sizecode
			,MAX(vc.CustomSP) as CustomSP
	from VNConsumption vc WITH (NOLOCK) 
	where vc.VNContractID = @contract
	group by vc.StyleID,vc.BrandID,vc.Category,vc.sizecode
) vc on vc.CustomSP = v.CustomSP
where v.VNContractID = @contract

--撈已Sewing數量
select 	t.ID
		,sdd.ComboType
		,sdd.Article
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
	select distinct	sdd.OrderId
			,sdd.ComboType
			,sdd.Article
			,sdd.SizeCode
			,sdd.QAQty
			,vd.HSCode
			,vd.NLCode
			,vd.UnitID
			,(ol_rate.value/100*sdd.QAQty)* (vd.Qty * (1+vd.Waste)) as Qty
			,v.CustomSP
	from #tmpWHNotClose t
	inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = t.ID	    
	inner join #tmpCustomSP v on v.StyleID = t.StyleID and v.BrandID = t.BrandID and v.Category = t.Category and v.seasonid=t.seasonid
	inner join VNConsumption_Article va WITH (NOLOCK) on va.ID = v.ID and va.Article = sdd.Article
	inner join VNConsumption_SizeCode vs WITH (NOLOCK) on vs.ID = v.ID and vs.SizeCode = sdd.SizeCode
	inner join VNConsumption_Detail vd WITH (NOLOCK) on vd.ID = v.ID
	outer apply(select value = dbo.GetOrderLocation_Rate(t.id,sdd.ComboType)) ol_rate		
) a on t.ID = a.OrderId and sdd.ComboType = a.ComboType and sdd.Article = a.Article and sdd.SizeCode = a.SizeCode
group by t.ID, sdd.ComboType,sdd.Article,sdd.SizeCode,isnull(a.HSCode,''),isnull(a.NLCode,'')
	,isnull(a.UnitID,''),isnull(a.CustomSP,''),t.POID
order by t.ID, sdd.ComboType,sdd.Article,sdd.SizeCode

--組WIP明細
select 	
 [ID] = IIF(ti.ID is null,tw.POID,ti.ID)
,[HSCode] = IIF(ti.HSCode is null,tw.HSCode,ti.HSCode)
,[NLCode] = IIF(ti.NLCode is null,tw.NLCode,ti.NLCode)
,[Qty] = (isnull(ti.Qty,0)-isnull(tw.Qty,0)) 
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
    where CustomSP <>''
	group by POID,HSCode,NLCode
) tw on tw.POID = ti.ID and tw.NLCode = ti.NLCode
order by IIF(ti.ID is null,tw.POID,ti.ID)

--4) Production成品倉(Prod. Qty Detail)

--撈尚未Pullout Complete的資料
select 	o.ID
		,o.StyleID
		,o.BrandID		
		,o.Category
		,o.SeasonID
into #tmpNoPullOutComp
from Orders o WITH (NOLOCK)
inner join VNConsumption vn WITH (NOLOCK) on vn.Styleid=o.Styleid 
and vn.SeasonID = o.SeasonID and vn.BrandID=o.BrandID and vn.category=o.category
where o.Category <>''
and o.WhseClose is null
and o.Finished=0
and o.Qty<>0
and o.LocalOrder = 0
and vn.VNContractID=@contract

;with tmpPreProdQty as(
	select distinct
	 tp.id ,tp.StyleID,tp.BrandID,tp.SeasonID,tp.Category
	,[Article] = sdd.Article
	,[SizeCode] = sdd.SizeCode
	,[SewQty] = isnull(dbo.getMinCompleteSewQty(tp.ID,sdd.Article,sdd.SizeCode),0) 
	 - isnull(dbo.getMinCompleteGMTQty(tp.ID,sdd.Article,sdd.SizeCode),0) 
	,[PullQty] = isnull(PulloutDD.ShipQty + InvAdjustQ.DiffQty,0)
	,[GMTAdjustQty] = isnull(AdjustGMT.gmtQty,0)
	from #tmpNoPullOutComp tp
	inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on tp.id=sdd.orderid	
	left join SewingOutput_Detail_Detail_Garment sddg WITH (NOLOCK) on sdd.id=sddg.OrderIDfrom
	outer apply(
		select isnull(Sum(pdd.ShipQty),0) as ShipQty
		from Pullout_Detail_Detail pdd WITH (NOLOCK)
		where pdd.OrderID = tp.ID  and pdd.Article = sdd.Article and pdd.SizeCode = sdd.SizeCode
	)PulloutDD
	outer apply(
		select isnull(Sum(iaq.DiffQty),0) as DiffQty
		from InvAdjust ia WITH (NOLOCK)
		inner join InvAdjust_qty iaq WITH (NOLOCK) on ia.id=iaq.id 	
		where ia.OrderID=sdd.OrderId and iaq.Article = sdd.Article and iaq.SizeCode=sdd.SizeCode
	)InvAdjustQ
	outer apply(
		select isnull(Sum(agd.qty),0) as gmtQty
		from AdjustGMT ag WITH (NOLOCK)
		inner join AdjustGMT_Detail agd WITH (NOLOCK) on ag.id=agd.id
		where ag.Status='Confirmed'
		and agd.orderid=sdd.orderid and agd.Article=sdd.Article and agd.SizeCode=sdd.SizeCode
	)AdjustGMT	
)
select
 [SP#] = tpq.ID
,[Article] = tpq.Article
,[SizeCode] = tpq.SizeCode
,[SewQty] = tpq.SewQty
,[PullOutQty] = tpq.PullQty
,[GMTAdjustQty] = tpq.GMTAdjustQty
,[HSCode] = vd.HSCode
,[NLCode] = vd.NLCode
,[Customs Unit] = vd.UnitID	
,[Qty]  = (tpq.SewQty-tpq.PullQty-tpq.GMTAdjustQty)*(vd.Qty * (1+vd.Waste))
,[Custom SP#] = tc.CustomSP
into #tmpProdQty
from tmpPreProdQty tpq
inner join #tmpCustomSP tc on tc.StyleID = tpq.StyleID 
                    and tc.BrandID = tpq.BrandID and tc.SeasonID = tpq.SeasonID 
                    and tpq.SizeCode = tc.SizeCode and tc.category=tpq.category
inner join VNConsumption_Article va WITH (NOLOCK) on va.ID = tc.ID and va.Article = tpq.Article
inner join VNConsumption_SizeCode vs WITH (NOLOCK) on vs.ID = tc.ID and vs.SizeCode = tpq.SizeCode
inner join VNConsumption_Detail vd WITH (NOLOCK) on tc.id=vd.id

-- 5) 在途成品(已報關但還沒出貨) (On Road Product Qty)

-- 取得Shipping P41有資料, 但Pullout沒資料 (或ShipQty=0)
select * 
into #tmpPull
from (
select a.ID from VNExportDeclaration a
where not exists (select 1 from pullout_detail
	where invno=a.invno)
union 
select a.ID from VNExportDeclaration a
where exists (select 1 from pullout_detail
	where invno=a.invno and shipqty=0)) a

-- 取得最大值customsp
select max(vdd.customsp)customsp,vd.id--,vc.sizecode
into #tmpmax
from VNExportDeclaration vd WITH (NOLOCK)
inner join VNExportDeclaration_Detail vdd WITH (NOLOCK) on vd.id=vdd.id
inner join VNConsumption vc on  vc.StyleID = vdd.StyleID and vc.BrandID=vdd.BrandID
		and vc.SeasonID=vdd.SeasonID and vc.category=vdd.category 
		and vc.sizecode=vdd.sizecode and vc.customsp=vdd.customsp
where vd.VNContractID=@contract
group by vd.id

select 
 [SP#] = vdd.OrderId
,[Article] = vdd.Article
,[SizeCode] = vdd.SizeCode
,[HSCode] = vcd.HSCode
,[NLCode] = vcd.NlCode
,[Unit] = vcd.UnitID
,[Qty] = vdds.ExportQty * (vcd.Qty * (1+vcd.Waste))
,[CustomSP] = vdd.customsp
INTO #OnRoadProductQty
from VNExportDeclaration vd WITH (NOLOCK)
inner join VNExportDeclaration_Detail vdd WITH (NOLOCK) on vd.id=vdd.id
inner join VNConsumption vc on vc.StyleID = vdd.StyleID and vc.BrandID=vdd.BrandID
							and vc.SeasonID=vdd.SeasonID and vc.category=vdd.category 
							and vc.sizecode=vdd.sizecode and vc.customsp=vdd.customsp
inner join VNConsumption_Detail vcd WITH (NOLOCK) on vcd.ID=vc.ID
outer apply (
	select sum(ExportQty) as ExportQty 
	from VNExportDeclaration_Detail WITH (NOLOCK)
	where id=vdd.id and Article=vdd.Article and sizecode=vdd.sizecode
)vdds
inner join #tmpmax tm on vd.id=tm.id and tm.customsp=vdd.customsp
where vd.status='Confirmed'
and vd.VNContractID=@contract
and exists(
	select 1 from #tmpPull where ID=vd.id)


-- 6) 料倉(C) (Scrap Qty Detail
--撈Scrap資料
select * 
into #tmpScrapQty
from (
	select 
	 [HSCode] = isnull(f.HSCode,'')
	,[NLCode] = isnull(f.NLCode,'')
	,[POID] = ft.POID
	,[Seq] = (ft.Seq1+'-'+ft.Seq2)
	,[Refno] = psd.Refno	
	,[Description] = isnull(f.Description,'')
	,[Roll] = ft.Roll
	,[Dyelot] = ft.Dyelot
	,[StockType] = ft.StockType
	,[Location] = ftd.MtlLocationID		
	,[Qty] = IIF(ft.InQty-ft.OutQty+ft.AdjustQty > 0,dbo.getVNUnitTransfer(isnull(f.Type,'')
			,psd.StockUnit
			,isnull(f.CustomsUnit,'')
			,mdp.LObQty
			,isnull(f.Width,0)
			,isnull(f.PcsWidth,0)
			,isnull(f.PcsLength,0)
			,isnull(f.PcsKg,0)
			,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',
				(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
			,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2'
				,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),'')),0)
	,[CustomsUnit] = isnull(f.CustomsUnit,'')
	,[ScrapQty] = ft.InQty-ft.OutQty+ft.AdjustQty
	,[StockUnit] = psd.StockUnit
	from FtyInventory ft WITH (NOLOCK) 
	left join FtyInventory_detail ftd WITH (NOLOCK) on ft.ukey=ftd.ukey
	inner join MDivisionPoDetail mdp WITH (NOLOCK) on mdp.Ukey=ft.MDivisionPoDetailUkey
	inner join PO_Supp_Detail psd WITH (NOLOCK) on ft.POID = psd.ID and psd.SEQ1 = ft.Seq1 and psd.SEQ2 = ft.Seq2
	inner join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
	where 1=1 and ft.StockType='O'
	and ft.InQty-ft.OutQty+ft.AdjustQty>0
union all
	select 	
	 [HSCode] = isnull(li.HSCode,'')
	,[NLCode] = isnull(li.NLCode,'')
	,[POID] = l.OrderID
	,[Seq] = ''
	,[Refno] = l.Refno	
	,[Description] = isnull(li.Description,'')
	,[Roll] = ''
	,[Dyelot] = ''
	,[StockType] = 'O'
	,[Location] = l.CLocation		
	,[Qty] = IIF(l.InQty-l.OutQty+l.AdjustQty > 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
			,l.UnitId,li.CustomsUnit
			,(l.InQty-l.OutQty+l.AdjustQty)*IIF(l.UnitId = 'CONE',isnull(li.MeterToCone,0),1)
			,0
			,li.PcsWidth
			,li.PcsLength
			,li.PcsKg
			,isnull(IIF(li.CustomsUnit = 'M2',
				(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
				,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),1)
			,isnull(IIF(li.CustomsUnit = 'M2',
				(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
				,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),'')),0)
	,[CustomsUnit] = isnull(li.CustomsUnit,'')
	,[ScrapQty] = l.InQty-l.OutQty+l.AdjustQty
	,[StockUnit] = l.UnitID
	from LocalInventory l WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = l.OrderID
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
	where 1=1
	and l.LobQty >0
) a

-- 7) Outstanding List 
-- 撈出 報表3)WIP 計算SewingoutPut 對應不到VNConsumption的資料
;with sp
 as (
	select distinct id,article,sizecode,qaqty
	from #tmpWHNotCloseSewingOutput
	where CustomSP =''
)
select o.ID
,o.Styleid
,o.Brandid
,o.Seasonid
,sp.Article 
,sp.Sizecode
,sp.QaQty
into #tmpOutstanding 
from sp
inner join orders o WITH (NOLOCK) on sp.id=o.id



--整理出Summary
select 
 [HSCode] = isnull(tc.HSCode,'')
,[NLCode] =  a.NLCode
,[Description] = isnull(vcd.DescEN,'')
,[UnitID] = isnull(tc.UnitID,'')
,[LiqQty] = isnull(tc.Qty,0) + isnull(td.Qty,0) --調整與勾選Liquidation data only相同
,[OnRoadMaterialQty] = isnull(orm.Qty,0)
,[WHQty] = isnull(tw.Qty,0)
,[WIPQty] = isnull(ti.Qty,0)
,[ProdQty] = isnull(tp.Qty,0)
,[OnRoadProductQty] = isnull(orp.Qty,0)
,[ScrapQty] = isnull(ts.Qty,0)
from (
	select 	NLCode 
	from #tmpContract 

	union 
	select	NLCode 
	from #tmpDeclare 

	--1)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpOnRoadMaterial
	) t 
	--2)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpWHQty
	) t 
	--3)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpWIPDetail
	) t 
	--4)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpProdQty
	) t 
	--5)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #OnRoadProductQty
	) t 
	--6)
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
	from #tmpOnRoadMaterial 
	group by NLCode
) orm on a.NLCode = orm.NLCode
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
	from #OnRoadProductQty 
	group by NLCode
) orp on a.NLCode = orp.NLCode
left join (
	select 	NLCode
			,sum(Qty) as Qty 
	from #tmpScrapQty 
	group by NLCode
) ts on a.NLCode = ts.NLCode
where 1 = 1 ");

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

--1)在途物料
select * from #tmpOnRoadMaterial where Qty > 0  {0} {1} order by POID,Seq

--2)W/H明細
select * from #tmpWHQty where Qty > 0 {0} {1} 
order by POID,Seq

--3)WIP明細
select * from #tmpWIPDetail where Qty > 0 {0} {1} 
order by ID

--4)Prod明細
select * from #tmpProdQty where Qty > 0 {0} {1} 
order by SP#,Article,SizeCode

--5)在途成品
select * from #OnRoadProductQty where Qty > 0 {0} {1} 
order by SP#,Article,SizeCode

--6)Scrap明細
select * from #tmpScrapQty where Qty > 0 {0} {1} 
order by POID,Seq

--7)Outstanding List 
select * from #tmpOutstanding where QaQty > 0
order by ID ", MyUtility.Check.Empty(this.hscode) ? string.Empty : string.Format("and HSCode = '{0}'", this.hscode),
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
                this.OnRoadMaterial = allData.Tables[1];
                this.WHDetail = allData.Tables[2];
                this.WIPDetail = allData.Tables[3];
                this.ProdDetail = allData.Tables[4];
                this.OnRoadProduction = allData.Tables[5];
                this.ScrapDetail = allData.Tables[6];
                this.Outstanding = allData.Tables[7];
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
            this.SetCount(this.Summary.Rows.Count + (this.liguidationonly ? 0 : this.OnRoadMaterial.Rows.Count + this.WHDetail.Rows.Count + this.WIPDetail.Rows.Count + this.ProdDetail.Rows.Count + this.ScrapDetail.Rows.Count + this.OnRoadProduction.Rows.Count + this.Outstanding.Rows.Count));

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

                if (this.OnRoadMaterial.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...OnRoadMaterial List");
                    result = MyUtility.Excel.CopyToXls(this.OnRoadMaterial, string.Empty, xltfile: "Shipping_R40_OnRoadMaterial.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
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

                if (this.OnRoadProduction.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...OnRoadProduction List");
                    result = MyUtility.Excel.CopyToXls(this.OnRoadProduction, string.Empty, xltfile: "Shipping_R40_OnRoadProduction.xltx", headerRow: 1);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.Outstanding.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Outstanding");
                    result = MyUtility.Excel.CopyToXls(this.Outstanding, string.Empty, xltfile: "Shipping_R40_OutStanding.xltx", headerRow: 1);
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
