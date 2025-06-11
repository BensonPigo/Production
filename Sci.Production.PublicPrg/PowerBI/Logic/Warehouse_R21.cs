using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Warehouse_R21
    {
        private DBProxy DBProxy;
        private string sqlcolumn = @"
select f.POID, f.SEQ1, f.SEQ2, fp.Dyelot, fp.Roll, [Grade] = MIN(fp.Grade)
into #tmp_FIR
from FIR F with (nolock) 
inner join FIR_Physical FP with (nolock) on fp.id = f.ID
group by f.POID, f.SEQ1, f.SEQ2, fp.Dyelot, fp.Roll

    select
	 [MDivisionID] = isnull(o.MDivisionID, '')
	,[FactoryID] = isnull(o.FactoryID, '')
    ,[SewLine] = isnull(o.SewLine, '')
	,[POID] = psd.id
    ,[Category] = case when o.Category='B'then'Bulk'
						when o.Category='G'then'Garment'
						when o.Category='M'then'Material'
						when o.Category='S'then'Sample'
						when o.Category='T'then'Sample mtl.'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
						else ''
					end
    ,[OrderCompay] = isnull(Company.NameEN, '')
	,[OrderTypeID] = isnull(o.OrderTypeID, '')
	,[WeaveTypeID] = isnull(d.WeaveTypeID, '')
    ,[BuyerDelivery] = o.BuyerDelivery
    ,[OrigBuyerDelivery] = o.OrigBuyerDelivery
    ,[MaterialComplete] = case when psd.Complete = 1 then 'Y' else '' end
    ,[ETA] = psd.FinalETA
    ,[ArriveWHDate] = stuff((
                    select distinct concat(';',isnull(Format(a.date,'yyyy/MM/dd'),'　'))
                    from (
	                    select date = Export.whsearrival
	                    from Export_Detail with (nolock) 
	                    inner join Export with (nolock) on Export.ID = Export_Detail.ID
	                    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2

	                    union all

	                    select date = ts.IssueDate
	                    from TransferIn ts with (nolock) 
	                    inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	                    and ts.Status='Confirmed'

                        union all

                        select date = r.WhseArrival
                        from Receiving r with (nolock) 
                        inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	                    where r.Status = 'Confirmed' 
	                    and r.Type = 'B' 
	                    and rd.POID = psd.ID 
	                    and rd.Seq1 = psd.Seq1 
	                    and rd.Seq2 = psd.Seq2
                    )a
                    for xml path('')
	            ),1,1,'')
    ,[ExportID] = isnull(stuff((
	            	select concat(';',ID)
	            	from Export_Detail with (nolock) 
	            	where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2 order by Export_Detail.ID
	            	for xml path('')
	            ),1,1,''), '')
    ,[Packages] = isnull(concat(
        stuff((
            select concat(';',Packages)
            from(
                select e2.Blno,Packages = sum(e2.Packages)
                from Export e2 with (nolock) 
                where exists (
                    select 1
	                from Export_Detail ed with (nolock)
                    inner join Export e with (nolock) on e.id = ed.id
	                where ed.POID = psd.id and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
                    and e.blno = e2.Blno)
                group by e2.Blno
            )x
	        for xml path('')
        ),1,1,'')
        ,
        iif(exists (
                select 1
	            from Export_Detail ed with (nolock)
                inner join Export e with (nolock) on e.id = ed.id
	            where ed.POID = psd.id and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2)                
            and
            exists(select 1
                from TransferIn ts with (nolock) 
	            where ts.Status='Confirmed'
                and exists(
                    select 1 from TransferIn_Detail tsd with (nolock)
                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
                    and tsd.ID = ts.ID))
            ,';','')
        ,
        stuff((
            select concat(';',Packages)
            from(
	            select ts.id,Packages = sum(Packages)
                from TransferIn ts with (nolock) 
	            where ts.Status='Confirmed'
                and exists(
                    select 1 from TransferIn_Detail tsd with (nolock)
                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
                    and tsd.ID = ts.ID)
                group by ts.id
            )x
	        for xml path('')
        ),1,1,'')
		), '')
    ,ContainerNo = isnull(stuff((
        select concat(';' , ContainerNo)
        from(
            select distinct ContainerNo = esc.ContainerType + '-' + esc.ContainerNo
	        from Export_Detail ed with (nolock)
            inner join Export_ShipAdvice_Container esc with (nolock) on esc.Export_DetailUkey = ed.Ukey
	        where ed.POID = psd.id and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
            and esc.ContainerType <> '' and esc.ContainerNo  <> ''
        )x
        for xml path('')
    ),1,1,''), '')
	,[BrandID] = isnull(o.BrandID, '')
	,[StyleID] = isnull(o.StyleID, '')
	,[SeasonID] = isnull(o.SeasonID, '')
	,[ProjectID] = isnull(o.ProjectID, '')
	,[ProgramID] = isnull(o.ProgramID, '')
	,[SEQ1] = psd.SEQ1
	,[SEQ2] = psd.SEQ2
	,[MaterialType] = concat(case when psd.FabricType = 'F' then 'Fabric'
                            when psd.FabricType = 'A' then 'Accessory'
                            when psd.FabricType = 'O' then 'Orher'
                            else psd.FabricType end
                       , '-' + Fabric.MtlTypeID)
    ,[StockPOID]=psd.StockPOID
    ,[StockSeq1]=psd.StockSeq1
    ,[StockSeq2]=psd.StockSeq2
	,[Refno] = psd.Refno
	,[SCIRefno] = psd.SCIRefno
	,[Description] = isnull(d.Description, '')
	,[ColorID] = isnull(CASE WHEN Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
				    THEN IIF(psd.SuppColor = '',dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')) , psd.SuppColor)
				    ELSE isnull(psdsC.SpecValue, '')  
			     END, '')
    ,[ColorName] = isnull(c.Name, '')
	,[Size] = isnull(psdsS.SpecValue, '')
	,[StockUnit] = psd.StockUnit
	,[PurchaseQty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.Qty)
    ,[OrderQty] = isnull(o.Qty, 0)
	,[ShipQty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.ShipQty)
	,[Roll] = isnull(fi.Roll, '')
	,[Dyelot] = isnull(fi.Dyelot, '')
	,[StockType] = case when fi.StockType = 'B' then 'Bulk'
						 when fi.StockType = 'I' then 'Inventory'
						 when fi.StockType = 'O' then 'Scrap'
						 else ''
						 end
	,[InQty] = round(isnull(fi.InQty, 0),2)
	,[OutQty] = round(isnull(fi.OutQty, 0),2)
	,[AdjustQty] = round(isnull(fi.AdjustQty, 0),2)
    ,[ReturnQty] = round(isnull(fi.ReturnQty, 0),2)
	,[BalanceQty] = round(isnull(fi.InQty, 0),2) - round(isnull(fi.OutQty, 0),2) + round(isnull(fi.AdjustQty, 0),2) - round(isnull(fi.ReturnQty, 0),2)
	,[MtlLocationID] = isnull(f.MtlLocationID, '')
    ,[Checker] = rdcheck.checker
    ,[Checker Date] = isnull(rdcheck.MINDCheckEditDate, rdcheck.MINDCheckAddDate)
    ,[MCHandle] = isnull(dbo.getPassEmail(o.MCHandle) ,'')
	,[POHandle] = isnull(dbo.getPassEmail(p.POHandle) ,'')
	,[POSMR] = isnull(dbo.getPassEmail(p.POSMR) ,'')
    ,[Supplier] = isnull(concat(Supp.ID, '-' + Supp.AbbEN), '')
    ,[VID] = isnull(VID.CustPONoList,'')
    ,[Grade] = isnull(fp.Grade, '')
    ,[Material Lock/ Unlock] = case when fi.Lock = 0 then 'Unlocked' 
                                    when fi.Lock = 1 then 'Locked' end
    ,[Material Lock/ Unlock Remark] = fi.Remark
    ";

        private string sqlcolumn_sum = @"select
	[M] = o.MDivisionID
	,[Factory] = o.FactoryID
	,[SP#] = psd.id
	,[OrderType] = o.OrderTypeID
    ,[OrderCompay] = isnull(Company.NameEN, '')
	,[WeaveType] = d.WeaveTypeID
    ,[BuyerDelivery]=o.BuyerDelivery
    ,[OrigBuyerDelivery]=o.OrigBuyerDelivery
    ,[ETA] = psd.FinalETA
    ,[ArriveWHDate] = stuff((
                    select distinct concat(';',isnull(Format(a.date,'yyyy/MM/dd'),'　'))
                    from (
	                    select date = Export.whsearrival
	                    from Export_Detail with (nolock) 
	                    inner join Export with (nolock) on Export.ID = Export_Detail.ID
	                    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2

	                    union all

	                    select date = ts.IssueDate
	                    from TransferIn ts with (nolock) 
	                    inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	                    and ts.Status='Confirmed'

                        union all

                        select date = r.WhseArrival
                        from Receiving r with (nolock) 
                        inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	                    where r.Status = 'Confirmed' 
	                    and r.Type = 'B' 
	                    and rd.POID = psd.ID 
	                    and rd.Seq1 = psd.Seq1 
	                    and rd.Seq2 = psd.Seq2
                    )a
                    for xml path('')
	            ),1,1,'')
    ,[WK] = stuff((
	            	select concat(';',ID)
	            	from Export_Detail with (nolock) 
	            	where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2 order by Export_Detail.ID
	            	for xml path('')
	            ),1,1,'')
    ,[Packages] =concat(
        stuff((
            select concat(';',Packages)
            from(
                select e2.Blno,Packages = sum(e2.Packages)
                from Export e2 with (nolock) 
                where exists (
                    select 1
	                from Export_Detail ed with (nolock)
                    inner join Export e with (nolock) on e.id = ed.id
	                where ed.POID = psd.id and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
                    and e.blno = e2.Blno)
                group by e2.Blno
            )x
	        for xml path('')
        ),1,1,'')
        ,
        iif(exists (
                select 1
	            from Export_Detail ed with (nolock)
                inner join Export e with (nolock) on e.id = ed.id
	            where ed.POID = psd.id and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2)                
            and
            exists(select 1
                from TransferIn ts with (nolock) 
	            where ts.Status='Confirmed'
                and exists(
                    select 1 from TransferIn_Detail tsd with (nolock)
                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
                    and tsd.ID = ts.ID))
            ,';','')
        ,
        stuff((
            select concat(';',Packages)
            from(
	            select ts.id,Packages = sum(Packages)
                from TransferIn ts with (nolock) 
	            where ts.Status='Confirmed'
                and exists(
                    select 1 from TransferIn_Detail tsd with (nolock)
                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
                    and tsd.ID = ts.ID)
                group by ts.id
            )x
	        for xml path('')
        ),1,1,'')
		)
    ,ContainerNo = stuff((
        select concat(';' , ContainerNo)
        from(
            select distinct ContainerNo = esc.ContainerType + '-' + esc.ContainerNo
	        from Export_Detail ed with (nolock)
            inner join Export_ShipAdvice_Container esc with (nolock) on esc.Export_DetailUkey = ed.Ukey
	        where ed.POID = psd.id and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
            and esc.ContainerType <> '' and esc.ContainerNo  <> ''
        )x
        for xml path('')
    ),1,1,'')
	,[Brand] = o.BrandID
	,[Style] = o.StyleID
	,[Season] = o.SeasonID
	,[Project] = o.ProjectID
	,[Program] = o.ProgramID
	,[Seq1] = psd.SEQ1
	,[Seq2] = psd.SEQ2
	,[Material Type] = concat(case when psd.FabricType = 'F' then 'Fabric'
                            when psd.FabricType = 'A' then 'Accessory'
                            when psd.FabricType = 'O' then 'Orher'
                            else psd.FabricType end,
                       '-' + Fabric.MtlTypeID)
    ,[stock sp]=psd.StockPOID
    ,[StockSeq1]=psd.StockSeq1
    ,[StockSeq2]=psd.StockSeq2
	,[Refno] = psd.Refno
	,[SCI Refno] = psd.SCIRefno
    ,[Description] = d.Description
	,[Color] = CASE WHEN Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
				    THEN IIF(psd.SuppColor = '',dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')) , psd.SuppColor)
				    ELSE isnull(psdsC.SpecValue, '')  
			   END
	,[Size] = isnull(psdsS.SpecValue, '')
	,[Stock Unit] = psd.StockUnit
	,[Purchase Qty] = round(ISNULL(r.RateValue,1) * psd.Qty,2)
    ,[Order Qty] = o.Qty
	,[Ship Qty] = round(ISNULL(r.RateValue,1) * psd.ShipQty,2)
	,[In Qty] = round(mpd.InQty,2)
	,[Out Qty] = round(mpd.OutQty,2)
	,[Adjust Qty] = round(mpd.AdjustQty,2)
    ,[Return Qty] = round(mpd.ReturnQty,2)
	,[Balance Qty] = round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2) - round(mpd.ReturnQty,2)
	,[Bulk Qty] = (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.ReturnQty,2) - round(mpd.LInvQty,2)
	,[Inventory Qty] = round(mpd.LInvQty,2)
	,[Scrap Qty] = round(mpd.LObQty ,2)
	,[Bulk Location] = mpd.ALocation
	,[Inventory Location] = mpd.BLocation
    ,[MCHandle] = isnull(dbo.getPassEmail(o.MCHandle) ,'')
	,[POHandle] = isnull(dbo.getPassEmail(p.POHandle) ,'')
	,[POSMR] = isnull(dbo.getPassEmail(p.POSMR) ,'') 
    ,[Supplier] = concat(Supp.ID, '-' + Supp.AbbEN)
    ,[VID] = isnull(VID.CustPONoList,'')
    ";

        /// <inheritdoc/>
        public Warehouse_R21()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 7200,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetWarehouse_R21Data(Warehouse_R21_ViewModel model)
        {
            StringBuilder sqlcmd = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (model.ReportType == 0)
            {
                #region 主要sql Detail
                sqlcmd.Append($@" 
from View_WH_Orders o with (nolock)
inner join Company on Company.ID = o.OrderCompanyID
inner join PO p with (nolock) on o.id = p.id
inner join PO_Supp ps with (nolock) on p.id = ps.id
inner join PO_Supp_Detail psd with (nolock) on p.id = psd.id and ps.seq1 = psd.seq1
{(!string.IsNullOrEmpty(model.WorkNo) ? $"INNER JOIN Export_Detail ed with (nolock) ON ed.POID=psd.ID AND ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2 AND ed.ID='{model.WorkNo}'" : string.Empty)}
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join FtyInventory fi with (nolock) on fi.POID = psd.id and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
left join Fabric WITH (NOLOCK) on psd.SCIRefno = fabric.SCIRefno
left join Supp with (nolock) on Supp.id = ps.SuppID 
left join Color c with(nolock) on c.id = isnull(psdsc.SpecValue,'')　and c.BrandId = psd.BrandId
left join #tmp_FIR fp on psd.ID = fp.POID and psd.SEQ1 = fp.SEQ1 and psd.SEQ2 = fp.SEQ2 and fi.Dyelot = fp.Dyelot and fi.Roll = fp.Roll
outer apply(
    select checker = iif(isnull(pass1.Name, '') != '', rd.MINDChecker + '-' + pass1.Name, rd.MINDChecker), 
           rd.MINDCheckAddDate, 
           rd.MINDCheckEditDate 
    from Receiving r with (nolock) 
    inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
    left join SciMES_Pass1 pass1 with (nolock) on rd.MINDChecker = pass1.ID
    where r.Status = 'Confirmed' 
    and rd.POID = fi.POID 
    and rd.Seq1 = fi.Seq1 
    and rd.Seq2 = fi.Seq2 
    and rd.Roll = fi.Roll 
    and rd.Dyelot = fi.Dyelot 
    and rd.StockType = fi.StockType 
    and Fabric.Type = 'F'
) rdcheck
outer apply
(
	select MtlLocationID = stuff(
	(
		select concat(',',MtlLocationID)
		from FtyInventory_Detail fid with (nolock) 
		where fid.Ukey = fi.Ukey
		for xml path('')
	),1,1,'')
)f
outer apply
(
	select Description ,WeaveTypeID
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
outer apply(
	select CustPONoList = Stuff((
		select concat(',',CustPONo)
		from (
				select distinct o.CustPONo 
				from PO_Supp_Detail_OrderList spdo with (nolock)
				inner join Fabric f with (nolock) on f.SCIRefno = psd.SCIRefno and f.SCIRefno like '%VID%' and f.Type = 'A' and f.MtlTypeID='LABEL' and f.Junk =0
				inner join orders o with (nolock) on o.ID = spdo.OrderID
				where spdo.id = psd.ID and spdo.SEQ1 =psd.SEQ1 and spdo.SEQ2 =psd.SEQ2 and spdo.OrderID = o.id
			) s
		for xml path ('')
	) , 1, 1, '')
) VID
where 1=1
");
                #endregion
            }
            else
            {
                #region 主要sql summary
                sqlcmd.Append($@"
from View_WH_Orders o with (nolock)
inner join Company on Company.ID = o.OrderCompanyID
inner join PO p with (nolock) on o.id = p.id
inner join PO_Supp ps with (nolock) on p.id = ps.id
inner join PO_Supp_Detail psd with (nolock) on p.id = psd.id and ps.seq1 = psd.seq1
{(!string.IsNullOrEmpty(model.WorkNo) ? $"INNER JOIN Export_Detail ed with (nolock) ON ed.POID=psd.ID AND ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2 AND ed.ID='{model.WorkNo}'" : string.Empty)}
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join MDivisionPoDetail mpd with (nolock) on mpd.POID = psd.id and mpd.Seq1 = psd.SEQ1 and mpd.seq2 = psd.SEQ2
left join Fabric WITH (NOLOCK) on psd.SCIRefno = fabric.SCIRefno
left join Supp WITH (NOLOCK) on Supp.id = ps.SuppID 
outer apply
(
	select Description ,WeaveTypeID
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
outer apply
(
	select RateValue = IIF(Denominator = 0,0, Numerator / Denominator)
	from Unit_Rate WITH (NOLOCK) 
	where UnitFrom = psd.PoUnit and UnitTo = psd.StockUnit
)r
outer apply(
	select CustPONoList = Stuff((
		select concat(',',CustPONo)
		from (
				select distinct o.CustPONo 
				from PO_Supp_Detail_OrderList spdo with (nolock) 
				inner join Fabric f with (nolock) on f.SCIRefno = psd.SCIRefno and f.SCIRefno like '%VID%' and f.Type = 'A' and f.MtlTypeID='LABEL' and f.Junk =0
				inner join orders o with (nolock) on o.ID = spdo.OrderID
				where spdo.id = psd.ID and spdo.SEQ1 =psd.SEQ1 and spdo.SEQ2 =psd.SEQ2 and spdo.OrderID = o.id
			) s
		for xml path ('')
	) , 1, 1, '')
) VID
where 1=1
");
                #endregion
            }

            if (!model.IsPowerBI)
            {
                #region where條件
                if (!MyUtility.Check.Empty(model.StartSPNo))
                {
                    sqlcmd.Append(string.Format(" and psd.id >= '{0}'", model.StartSPNo));
                }

                if (!MyUtility.Check.Empty(model.EndSPNo))
                {
                    sqlcmd.Append(string.Format(" and (psd.id <= '{0}' or psd.id like '{0}%')", model.EndSPNo));
                }

                if (!MyUtility.Check.Empty(model.MDivision))
                {
                    sqlcmd.Append(string.Format(" and o.MDivisionID = '{0}'", model.MDivision));
                }

                if (!MyUtility.Check.Empty(model.Factory))
                {
                    sqlcmd.Append(string.Format(" and o.FtyGroup = '{0}'", model.Factory));
                }

                if (!MyUtility.Check.Empty(model.StartRefno))
                {
                    sqlcmd.Append(string.Format(" and psd.Refno >= '{0}'", model.StartRefno));
                }

                if (!MyUtility.Check.Empty(model.EndRefno))
                {
                    sqlcmd.Append(string.Format(" and (psd.Refno <= '{0}' or psd.Refno like '{0}%')", model.EndRefno));
                }

                if (!MyUtility.Check.Empty(model.Color))
                {
                    sqlcmd.Append(string.Format(" and psdsC.SpecValue = '{0}'", model.Color));
                }

                if (!MyUtility.Check.Empty(model.MT))
                {
                    if (model.MT != "All")
                    {
                        sqlcmd.Append(string.Format(" and psd.FabricType = '{0}'", model.MT));
                    }
                }

                if (!MyUtility.Check.Empty(model.MtlTypeID))
                {
                    sqlcmd.Append(string.Format(" and fabric.MtlTypeID = '{0}'", model.MtlTypeID));
                }

                if (!MyUtility.Check.Empty(model.ST))
                {
                    if (model.ST != "All")
                    {
                        if (model.ReportType == 0)
                        {
                            {
                                sqlcmd.Append(string.Format(" and fi.StockType = '{0}'", model.ST));
                            }
                        }
                        else
                        {
                            if (model.ST == "B")
                            {
                                sqlcmd.Append(" and (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.ReturnQty,2) - round(mpd.LInvQty,2)>0");
                            }
                            else if (model.ST == "I")
                            {
                                sqlcmd.Append(" and round(mpd.LInvQty,2) > 0");
                            }
                            else if (model.ST == "O")
                            {
                                sqlcmd.Append(" and round(mpd.LObQty ,2) > 0");
                            }
                        }
                    }
                }

                if (!MyUtility.Check.Empty(model.BuyerDeliveryFrom))
                {
                    sqlcmd.Append($" and o.BuyerDelivery >='{((DateTime)model.BuyerDeliveryFrom).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.BuyerDeliveryTo))
                {
                    sqlcmd.Append($" and o.BuyerDelivery <='{((DateTime)model.BuyerDeliveryTo).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.ETAFrom))
                {
                    sqlcmd.Append($" and psd.FinalETA >='{((DateTime)model.ETAFrom).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.ETATo))
                {
                    sqlcmd.Append($" and psd.FinalETA <='{((DateTime)model.ETATo).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.OrigBuyerDeliveryFrom))
                {
                    sqlcmd.Append($" and o.OrigBuyerDelivery >='{((DateTime)model.OrigBuyerDeliveryFrom).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.OrigBuyerDeliveryTo))
                {
                    sqlcmd.Append($" and o.OrigBuyerDelivery <='{((DateTime)model.OrigBuyerDeliveryTo).ToString("yyyy/MM/dd")}'");
                }

                if (model.Bulk || model.Sample || model.Material || model.Smtl)
                {
                    sqlcmd.Append(" and (1=0");
                    if (model.Bulk)
                    {
                        sqlcmd.Append(" or o.Category = 'B'");
                    }

                    if (model.Sample)
                    {
                        sqlcmd.Append(" or o.Category = 'S'");
                    }

                    if (model.Material)
                    {
                        sqlcmd.Append(" or o.Category = 'M'");
                    }

                    if (model.Smtl)
                    {
                        sqlcmd.Append(" or o.Category = 'T'");
                    }

                    sqlcmd.Append(")");
                }

                if (model.Complete)
                {
                    sqlcmd.Append(" and psd.Complete = '1'");
                }

                if (model.NoLocation)
                {
                    sqlcmd.Append(@"
and not exists(
    select 1
    from FtyInventory_Detail fid with (nolock) 
    where fid.Ukey = fi.Ukey and isnull(fid.MtlLocationID, '') <> ''
)
");
                }

                if (!MyUtility.Check.Empty(model.StartLocation) && !MyUtility.Check.Empty(model.EndLocation))
                {
                    parameters.Add(new SqlParameter("@location1", model.StartLocation));
                    parameters.Add(new SqlParameter("@location2", model.EndLocation));
                    sqlcmd.Append(
                        @" 
        and exists ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where fi.ukey = ukey
                        and mtllocationid >= @location1
                        and mtllocationid <= @location2 ) " + Environment.NewLine);
                }
                else if (!MyUtility.Check.Empty(model.StartLocation))
                {
                    parameters.Add(new SqlParameter("@location1", model.StartLocation));
                    sqlcmd.Append(
                        @" 
        and exists ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where fi.ukey = ukey
                        and mtllocationid = @location1) " + Environment.NewLine);
                }
                else if (!MyUtility.Check.Empty(model.EndLocation))
                {
                    parameters.Add(new SqlParameter("@location2", model.EndLocation));
                    sqlcmd.Append(
                        @" 
        and exists ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where fi.ukey = ukey
                        and mtllocationid = @location2) " + Environment.NewLine);
                }
                #endregion
            }

            if (model.BoolCheckQty)
            {
                if (model.ReportType == 0)
                {
                    sqlcmd.Append(" and (round(fi.InQty,2) - round(fi.OutQty,2) + round(fi.AdjustQty,2)) - round(fi.ReturnQty,2) > 0");
                }
                else
                {
                    sqlcmd.Append(" and ((round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2) - round(mpd.ReturnQty,2) > 0) or mpd.LInvQty >0 or mpd.LObQty >0)  ");
                }
            }

            if (!MyUtility.Check.Empty(model.ArriveWHFrom) && !MyUtility.Check.Empty(model.ArriveWHTo))
            {
                sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival between '{((DateTime)model.ArriveWHFrom).ToString("yyyy/MM/dd")}' and '{((DateTime)model.ArriveWHTo).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate between '{((DateTime)model.ArriveWHFrom).ToString("yyyy/MM/dd")}' and '{((DateTime)model.ArriveWHTo).ToString("yyyy/MM/dd")}') 
or 
    exists ( 
	select 1 
    from Receiving r with (nolock) 
    inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	where r.WhseArrival between '{((DateTime)model.ArriveWHFrom).ToString("yyyy/MM/dd")}' and '{((DateTime)model.ArriveWHTo).ToString("yyyy/MM/dd")}'
    and r.Status = 'Confirmed' 
    and r.Type = 'B' 
	and rd.POID = psd.ID 
	and rd.Seq1 = psd.Seq1 
	and rd.Seq2 = psd.Seq2 )
)
");
            }
            else if (!MyUtility.Check.Empty(model.ArriveWHFrom))
            {
                sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival >= '{((DateTime)model.ArriveWHFrom).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate >= '{((DateTime)model.ArriveWHFrom).ToString("yyyy/MM/dd")}' ) 
or 
    exists ( 
	select 1 
    from Receiving r with (nolock) 
    inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	where r.WhseArrival >= '{((DateTime)model.ArriveWHFrom).ToString("yyyy/MM/dd")}'
    and r.Status = 'Confirmed' 
    and r.Type = 'B' 
	and rd.POID = psd.ID 
	and rd.Seq1 = psd.Seq1 
	and rd.Seq2 = psd.Seq2 )
)
");
            }
            else if (!MyUtility.Check.Empty(model.ArriveWHTo))
            {
                sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival <= '{((DateTime)model.ArriveWHTo).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate <= '{((DateTime)model.ArriveWHTo).ToString("yyyy/MM/dd")}' ) 
or 
    exists ( 
	select 1 
    from Receiving r with (nolock) 
    inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	where r.WhseArrival <= '{((DateTime)model.ArriveWHTo).ToString("yyyy/MM/dd")}'
    and r.Status = 'Confirmed' 
    and r.Type = 'B' 
	and rd.POID = psd.ID 
	and rd.Seq1 = psd.Seq1 
	and rd.Seq2 = psd.Seq2 )
)
");
            }

            string sql = string.Empty;
            if (model.IsPowerBI)
            {
                string columns = @"
	, [AddDate] = o.AddDate
	, [EditDate] =o.EditDate
    , [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    , [BIInsertDate] = GETDATE()
";
                sql = this.sqlcolumn + columns + sqlcmd.ToString();
            }
            else
            {
                string columns = @"
, left(CONVERT(CHAR(8),o.SciDelivery, 112),4) as SciYYYY
";
                if (model.ReportType == 0)
                {
                    sql = this.sqlcolumn + columns + sqlcmd.ToString();
                }
                else
                {
                    sql = this.sqlcolumn_sum + columns + sqlcmd.ToString();
                }
            }

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sql, parameters, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }
    }
}
