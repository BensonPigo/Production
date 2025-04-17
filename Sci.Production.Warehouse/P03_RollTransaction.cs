using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_RollTransaction : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable dtFtyinventory;
        private DataTable dtTrans;
        private DataTable dtSummary;
        private DataSet data = new DataSet();
        private decimal useQty = 0;
        private bool bUseQty = false;

        /// <inheritdoc/>
        public P03_RollTransaction(DataRow data)
        {
            this.InitializeComponent();

            this.dr = data;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Text += string.Format(@" ({0}-{1}-{2})", this.dr["id"], this.dr["seq1"], this.dr["seq2"]);  // 351: WAREHOUSE_P03_RollTransaction_Transaction Detail by Roll#，3.Tool bar要帶出SP# & Seq
            this.displaySeqNo.Text = this.dr["seq1"].ToString() + "-" + this.dr["seq2"].ToString();
            this.displayDescription.Text = MyUtility.GetValue.Lookup(string.Format("select dbo.getmtldesc('{0}','{1}','{2}',2,0)", this.dr["id"].ToString(), this.dr["seq1"].ToString(), this.dr["seq2"].ToString()));
            this.numArrivedQtyBySeq.Value = MyUtility.Check.Empty(this.dr["inqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["inqty"].ToString());
            this.numReleasedQtyBySeq.Value = MyUtility.Check.Empty(this.dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["outqty"].ToString());

            decimal iN = MyUtility.Check.Empty(this.dr["inqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["inqty"].ToString());
            decimal oUT = MyUtility.Check.Empty(this.dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["outqty"].ToString());
            decimal aDJ = MyUtility.Check.Empty(this.dr["adjustqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["adjustqty"].ToString());
            decimal rQT = MyUtility.Check.Empty(this.dr["ReturnQty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["ReturnQty"].ToString());
            this.numBalQtyBySeq.Value = iN - oUT + aDJ - rQT;

            #region "顯示DTM"
            DataTable dtmDt;
            string sql = $@"
select psd.id,psd.seq1,psd.seq2,qty,ShipQty,Refno,ColorID = isnull(psdsC.SpecValue, ''),iif(qty <= shipqty, 'True','False') bUseQty
into #tmp
from Po_Supp_Detail psd
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
where psd.id = '{this.dr["id"]}'
and psd.seq1 = '{this.dr["seq1"]}'
and psd.seq2 = '{this.dr["seq2"]}'
 
select isnull(Round(dbo.getUnitQty(psd.POUnit, psd.StockUnit, (isnull(psd.NETQty,0)+isnull(psd.lossQty,0))), 2),0.0) as UseQty,b.bUseQty
from Po_Supp_Detail psd
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
inner join #tmp b on psd.id = b.id and psd.Refno = b.Refno and isnull(psdsC.SpecValue, '') = b.ColorID and b.bUseQty = 'True' 
where psd.id = '{this.dr["id"]}'
and psd.seq1 = 'A1' 

drop table #tmp
            ";
            DualResult dtmResult = DBProxy.Current.Select(null, sql, out dtmDt);
            if (dtmResult == false)
            {
                this.ShowErr(sql, dtmResult);
            }

            if (!MyUtility.Check.Empty(dtmDt))
            {
                if (dtmDt.Rows.Count > 0)
                {
                    this.bUseQty = MyUtility.Convert.GetBool(dtmDt.Rows[0]["bUseQty"]);
                    this.useQty = this.bUseQty ? MyUtility.Convert.GetDecimal(dtmDt.Rows[0]["useQty"]) : this.useQty;
                }
            }
            #endregion

            #region Grid1 - Sql command
            string selectCommand1
                = string.Format(
                    @"
Select a.Roll,a.Dyelot
, FullRoll = FullRoll.List
, FullDyelot = FullDyelot.List
,[stocktype] = case when a.stocktype = 'B' then 'Bulk'
                when a.stocktype = 'I' then 'Invertory'
			    when a.stocktype = 'O' then 'Scrap' End
,a.InQty
,a.OutQty
,a.AdjustQty
,a.ReturnQty
,a.InQty - a.OutQty + a.AdjustQty - a.ReturnQty as balance
,dbo.Getlocation(a.ukey)  MtlLocationID 
,a.ContainerCode
,a.Tone
,[GMTWash] = a.GMTWashStatus
from FtyInventory a WITH (NOLOCK)
outer apply(
	select List = Stuff((
		select concat(',',FullRoll)
		from (
				select 	distinct
					FullRoll
				from dbo.Receiving_Detail rd WITH (NOLOCK)
				where a.POID = rd.PoId 
					and a.Seq1 = rd.Seq1 and a.Seq2 = rd.Seq2 
					and a.Dyelot = rd.Dyelot and a.Roll = rd.Roll 
					and a.StockType = rd.StockType
			) s
		for xml path ('')
	) , 1, 1, '')
)FullRoll 
outer apply(
	select List = Stuff((
		select concat(',',FullDyelot)
		from (
				select 	distinct
					FullDyelot
				from dbo.Receiving_Detail rd WITH (NOLOCK)
				where a.POID = rd.PoId 
					and a.Seq1 = rd.Seq1 and a.Seq2 = rd.Seq2 
					and a.Dyelot = rd.Dyelot and a.Roll = rd.Roll 
					and a.StockType = rd.StockType
			) s
		for xml path ('')
	) , 1, 1, '')
)FullDyelot 
where a.Poid = '{0}'
    and a.Seq1 = '{1}'
    and a.Seq2 = '{2}'
    and a.StockType <> 'O'  --C倉不用算
and not (inqty = 0 and outqty = 0 and adjustqty = 0 and returnQty = 0)
order by a.dyelot,a.roll,a.stocktype
",
                    this.dr["id"].ToString(),
                    this.dr["seq1"].ToString(),
                    this.dr["seq2"].ToString(),
                    Env.User.Keyword);
            #endregion

            #region Grid2 - Sql Command

            string selectCommand2
                = string.Format(
                    @"
select tmp.Roll
	, [stocktype] = case when stocktype = 'B' then 'Bulk'
						 when stocktype = 'I' then 'Invertory'
						 when stocktype = 'O' then 'Scrap' End
	, Dyelot
	, IssueDate
	, ID
	, name
	, inqty
	, outqty
	, adjust
    , ReturnQty
	, Remark
	, location
	, ContainerCode
	, [balance] = sum(TMP.inqty - TMP.outqty + tmp.adjust - tmp.ReturnQty) over (partition by tmp.stocktype,tmp.roll,tmp.dyelot order by tmp.IssueDate,tmp.stocktype,tmp.inqty desc,tmp.iD,tmp.ToPOID,tmp.ToSeq )
    , GW
    , ActW
    , ToPOID
    , ToSeq
from (
	select b.roll
		, b.stocktype
		, b.dyelot
		, a.IssueDate
		, a.id
		, [name] = Case type when 'A' then 'P35. Adjust Bulk Qty' 
						     when 'B' then 'P34. Adjust Stock Qty' end 
		, [inqty] = 0
		, [outqty] = 0
		, [adjust] = sum(QtyAfter - QtyBefore)
        , [ReturnQty] = 0
		, a.remark 
		, [location] = '' 
		, ContainerCode = ''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from Adjust a WITH (NOLOCK) , Adjust_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where Status = 'Confirmed'
	and b.poid = '{0}'
	and b.seq1 = '{1}'
	and b.seq2 = '{2}' 
	and a.id = b.id 
	group by a.id, b.poid, b.seq1, b.Seq2, a.remark,a.IssueDate,type,b.roll,b.stocktype,b.dyelot,fi.ContainerCode

union all

	select b.FromRoll
		, b.FromStockType
		, b.FromDyelot
		, a.IssueDate
		, a.id
		, [name] = case type when 'A' then 'P31. Material Borrow From' 
							 when 'B' then 'P32. Material Give Back From' end
		, [inqty] = 0
		, [released] = sum(qty) 
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
		, [location] = ''
		, ContainerCode = ''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.FromPOID
		and fi.Seq1 = b.FromSeq1 and fi.Seq2 = b.FromSeq2
		and fi.Roll = b.FromRoll and fi.Dyelot = b.FromDyelot
		and fi.StockType = b.FromStockType
	where Status = 'Confirmed'
	and FromPoId = '{0}'
	and FromSeq1 = '{1}'
	and FromSeq2 = '{2}'
	and a.id = b.id 
	group by a.id, FromPoId, FromSeq1,FromSeq2, a.remark,a.IssueDate,b.FromRoll,b.FromStockType,b.FromDyelot,a.type,fi.ContainerCode

union all

	select b.ToRoll
		, b.ToStockType
		, b.ToDyelot
		, issuedate
		, a.id
		, [name] = case type when 'A' then 'P31. Material Borrow To' 
							 when 'B' then 'P32. Material Give Back To' end
		, [inqty] = sum(qty)
		, [ouqty] = 0
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark 
		, [location] = b.ToLocation
        , GW = ''
        , ActW = ''
		, [ContainerCode] = b.ToContainerCode
        , ToPOID = ''
        , ToSeq = ''
	from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.ToPOID
		and fi.Seq1 = b.ToSeq1 and fi.Seq2 = b.ToSeq2
		and fi.Roll = b.ToRoll and fi.Dyelot = b.ToDyelot
		and fi.StockType = b.ToStockType
	where Status = 'Confirmed'
	and ToPoid = '{0}'
	and ToSeq1 = '{1}'
	and ToSeq2 = '{2}'
	and a.id = b.id 
	group by a.id, ToPoid, ToSeq1,ToSeq2, a.remark,a.IssueDate,b.ToRoll,b.ToStockType,b.ToDyelot,a.type,b.ToLocation,b.ToContainerCode

union all

	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
        , name = case type 
                	when 'A' then 'P10. Issue Fabric to Cutting Section' 
                	when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
                	when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
                	when 'D' then 'P13. Issue Material by Item'
                	when 'E' then 'P33. Issue Thread'
                	when 'I' then 'P62. Issue Fabric for Cutting Tape'
                end 
		, [inqty] = 0
		, [released] = sum(Qty)
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
		, [location] = '' 
		, ContainerCode = ''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from Issue a WITH (NOLOCK) , Issue_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where Status = 'Confirmed'
	and b.poid = '{0}'
	and b.seq1 = '{1}'
	and b.seq2 = '{2}'
	and a.id = b.id 
	group by a.id, b.poid, b.seq1, b.Seq2, a.remark,a.IssueDate,a.type,b.roll,b.stocktype,b.dyelot,a.type, fi.ContainerCode      
	
union all

	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
		, [name] = case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
								   when 'F' then 'P16. Issue Fabric Lacking & Replacement' 
				   end
		, [inqty] = 0
		, [outqty] = sum(b.Qty) 
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
		, [location] = '' 
		, ContainerCode = ''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where Status in ('Confirmed','Closed')
	and b.poid = '{0}' 
	and b.seq1 = '{1}'
	and b.seq2 = '{2}'
	and a.id = b.id 
	and a.type != 'L' 
	group by a.id, b.poid, b.seq1, b.Seq2, a.remark ,a.IssueDate,a.FabricType,b.roll,b.stocktype,b.dyelot,fi.ContainerCode

union all

	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
		, [name] = case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
								   when 'F' then 'P16. Issue Fabric Lacking & Replacement' 
					end
		, [inqty] = 0 
		, [outqty] = 0  
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark 
		, [location] = '' 
		, ContainerCode = ''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where Status in ('Confirmed','Closed')
	and b.poid = '{0}'
	and b.seq1 = '{1}'
	and b.seq2 = '{2}'
	and a.id = b.id 
	and a.type = 'L'
	group by a.id, b.poid, b.seq1, b.Seq2, a.remark  ,a.IssueDate,a.FabricType,b.roll,b.stocktype,b.dyelot ,fi.ContainerCode
                                       
union all

	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
		, [name] = 'P17. R/Mtl Return'
		, [inqty] = 0
		, [released] = sum(0.00 - b.Qty)
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
		, [location] = b.Location
		, b.ContainerCode
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from IssueReturn a WITH (NOLOCK) , IssueReturn_Detail b WITH (NOLOCK) 
	where status = 'Confirmed'
	and b.poid = '{0}'
	and b.seq1 = '{1}'
	and b.seq2 = '{2}'
	and a.id = b.id 	
	group by a.Id, b.poid, b.seq1, b.Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot,b.Location,b.ContainerCode

union all

	select b.roll
		, b.stocktype
		, b.dyelot
        , [issuedate] = case type when 'A' then a.ETA else a.WhseArrival end
        , a.id
	    , [name] = case type when 'A' then 'P07. Material Receiving' 
							 when 'B' then 'P08. Warehouse Shopfloor Receiving' 
				   end
	    , [inqty] = sum(b.StockQty)
		, [outqty] = 0
		, [adjust] = 0
        , [ReturnQty] = 0
		, [remark] = '' 
		, [location] = b.Location
		, b.ContainerCode
        , GW = CONVERT(varchar, b.Weight)
        , ActW = CONVERT(varchar, b.ActualWeight)
        , ToPOID = ''
        , ToSeq = ''
    from Receiving a WITH (NOLOCK) , Receiving_Detail b WITH (NOLOCK) 
    where Status = 'Confirmed' 
	and poid = '{0}' 
	and seq1 = '{1}'
	and seq2 = '{2}'
	and a.id = b.id     
    group by a.Id, poid, seq1,Seq2,a.WhseArrival,a.Type,b.roll,b.stocktype,b.dyelot,a.eta,b.Location,b.ContainerCode,b.Weight,b.ActualWeight

union all

	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
		, [name] = 'P37. Return Receiving Material'
		, [inqty] = 0
		, [released] = 0
		, [adjust] = 0
        , [ReturnQty] = b.Qty
		, a.remark
		, [location] = ''
		, [ContainerCode] = ''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from ReturnReceipt a WITH (NOLOCK) , ReturnReceipt_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where Status = 'Confirmed'
	and b.poid = '{0}'
	and b.seq1 = '{1}'
	and b.seq2 = '{2}' 
	and a.id = b.id    
	group by a.id, b.poid, b.seq1, b.Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot,b.Qty, fi.ContainerCode

union all

	select b.FromRoll
		, b.FromStockType
		, b.FromDyelot
		, issuedate
		, a.id
		, [name] = case type when 'B' then 'P23. Transfer Inventory to Bulk' 
							 when 'A' then 'P22. Transfer Bulk to Inventory' 
							 when 'C' then 'P36. Transfer Scrap to Inventory' 
							 when 'D' then 'P25. Transfer Bulk to Scrap' 
							 when 'E' then 'P24. Transfer Inventory to Scrap'
					end
		, [inqty] = 0
		, [released] = sum(Qty)
		, [adjust] = 0
        , [ReturnQty] = 0
		, [remark] = isnull(a.remark,'') 
		, [location] = ''
		, [ContainerCode] = ''
        , GW = ''
        , ActW = ''
        , ToPOID = case type when 'B' then b.ToPOID else '' end
        , ToSeq = case type when 'B' then Concat (b.ToSeq1, ' ', b.ToSeq2) else '' end
	from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.FromPOID
		and fi.Seq1 = b.FromSeq1 and fi.Seq2 = b.FromSeq2
		and fi.Roll = b.FromRoll and fi.Dyelot = b.FromDyelot
		and fi.StockType = b.FromStockType
	where Status = 'Confirmed'
	and Frompoid = '{0}'
	and Fromseq1 = '{1}'
	and FromSeq2 = '{2}'
	and a.id = b.id    
    and a.type <> 'C'  --排除C to B 的轉出紀錄，因目前不需要C倉交易紀錄，避免下面DataRelation出錯
	group by a.id, frompoid, FromSeq1,FromSeq2,a.IssueDate,a.Type,b.FromRoll,b.FromStockType,b.FromDyelot,a.Type,a.remark, fi.ContainerCode,b.ToPOID,b.ToSeq1, b.ToSeq2
                                                                             
union all
	
	select b.ToRoll
		, b.ToStockType
		, b.ToDyelot
		, issuedate
		, a.id
		, [name] = case type when 'B' then 'P23. Transfer Inventory to Bulk' 
							 when 'A' then 'P22. Transfer Bulk to Inventory' 
							 when 'C' then 'P36. Transfer Scrap to Inventory'
				   end
		, [arrived] = sum(Qty)
		, [ouqty] = 0
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
	    , [ToLocation] = isnull(stuff((Select concat(',', tmp.ToLocation)
										from (select b1.ToLocation 
											  from SubTransfer a1 WITH (NOLOCK) 
											  inner join SubTransfer_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
											  where a1.status = 'Confirmed' 
											  and b1.ToPoid = b.ToPoid
											  and b1.ToSeq1 = b.ToSeq1
											  and b1.ToSeq2 = b.ToSeq2 
											  and b1.ToRoll = b.ToRoll
											  and b1.ToDyelot = b.ToDyelot
                                              and ISNULL(b1.ToLocation, '') <> ''
											  group by b1.ToLocation) tmp 
									for XML PATH('')) ,1,1,'') ,'') 
		,[ContainerCode] = b.ToContainerCode
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
	where Status = 'Confirmed'
	and ToPoid = '{0}'
	and ToSeq1 = '{1}' 
	and ToSeq2 = '{2}'
	and a.id = b.id
	AND TYPE not in ('D','E')
	group by a.id, ToPoid, ToSeq1,ToSeq2, a.remark ,a.IssueDate,b.ToRoll,b.ToStockType,b.ToDyelot,a.type ,b.ToContainerCode	    

union all

	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
        , [name] = 'P18. Transfer In'
        , [arrived] = sum(Qty)
		, [ouqty] = 0
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
		, [Location] = ISNULL(stuff((Select concat(',', tmp.Location)
                        from (select b1.Location 
                                    from TransferIn a1 WITH (NOLOCK) 
                                    inner join TransferIn_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                    where a1.status = 'Confirmed' 
                                        and b1.Poid = b.Poid
                                        and b1.Seq1 = b.Seq1
                                        and b1.Seq2 = b.Seq2 
										and b1.Roll = b.Roll
										and b1.Dyelot = b.Dyelot
                                        and ISNULL(b1.Location, '') <> ''
										group by b1.Location) tmp 
                        for XML PATH('')) ,1,1,''), '')
		, b.ContainerCode
        , GW = CONVERT(varchar, b.Weight)
        , ActW = CONVERT(varchar, b.ActualWeight)
        , ToPOID = ''
        , ToSeq = ''
	from TransferIn a WITH (NOLOCK) , TransferIn_Detail b WITH (NOLOCK) 
	where Status = 'Confirmed'
	and poid = '{0}'
	and seq1 = '{1}'
	and seq2 = '{2}'
	and a.id = b.id 	
	group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot, b.ContainerCode,b.ActualWeight,b.Weight

union all

	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
        , [name] = 'P19. Transfer Out'
        , [inqty] = 0
		, [released] = sum(Qty)
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
		, [location] = ''
		, ContainerCode =''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from TransferOut a WITH (NOLOCK) , TransferOut_Detail b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where Status = 'Confirmed'
	and b.poid = '{0}'
	and b.seq1 = '{1}'
	and b.seq2 = '{2}'
	and a.id = b.id 		
	group by a.id, b.poid, b.Seq1, b.Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot, fi.ContainerCode

union all
	
	select b.roll
		, b.stocktype
		, b.dyelot
		, issuedate
		, a.id
		, [name] = case type when 'B' then 'P73. Transfer Inventory to Bulk cross M (Receive)' 
							 when 'D' then 'P76. Material Borrow cross M (Receive)' 
							 when 'G' then 'P78. Material Return Back cross M (Receive)'  end
		, [inqty] = sum(Qty)
		, [released] = 0
		, [adjust] = 0
        , [ReturnQty] = 0
		, a.remark
		, [location] = ''
		, ContainerCode = ''
        , GW = ''
        , ActW = ''
        , ToPOID = ''
        , ToSeq = ''
	from RequestCrossM a WITH (NOLOCK) , RequestCrossM_Receive b WITH (NOLOCK) 
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where Status = 'Confirmed'
	and b.poid = '{0}'
	and b.seq1 = '{1}'
	and b.seq2 = '{2}'
	and a.id = b.id 		
	group by a.id, b.poid, b.seq1, b.Seq2, a.remark,a.IssueDate,a.type,b.roll,b.stocktype,b.dyelot,a.type , fi.ContainerCode

) tmp
where stocktype <> 'O'
group by IssueDate,inqty,outqty,adjust,ReturnQty,id,Remark,location,tmp.name,tmp.roll,tmp.stocktype,tmp.dyelot,tmp.ContainerCode, tmp.GW, tmp.ActW,ToPOID,ToSeq
order by tmp.IssueDate,tmp.stocktype,tmp.inqty desc,tmp.iD

",
                    this.dr["id"].ToString(),
                    this.dr["seq1"].ToString(),
                    this.dr["seq2"].ToString(),
                    Env.User.Keyword);

            #endregion
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out this.dtFtyinventory);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }

            this.dtFtyinventory.TableName = "dtFtyinventory";
            this.dtSummary = this.dtFtyinventory.Clone();
            this.dtSummary.Columns.Add("rollcount", typeof(int));
            this.dtSummary.Columns.Add("DTM", typeof(decimal));
            this.bindingSource3.DataSource = this.dtSummary;

            DualResult selectResult2 = DBProxy.Current.Select(null, selectCommand2, out this.dtTrans);
            if (selectResult2 == false)
            {
                this.ShowErr(selectCommand2, selectResult2);
            }

            if (this.dtFtyinventory.Rows.Count == 0 || this.dtTrans.Rows.Count == 0)
            {
                // MyUtility.Msg.ErrorBox("Data not found!!");
                return;
            }

            try
            {
                this.bindingSource1.DataSource = this.dtFtyinventory;
                this.bindingSource2.DataSource = this.dtTrans;

                Ict.Win.UI.DataGridViewTextBoxColumn col_ContainerCode;

                // 設定Grid1的顯示欄位
                this.gridFtyinventory.IsEditingReadOnly = true;
                this.gridFtyinventory.DataSource = this.bindingSource1;
                this.Helper.Controls.Grid.Generator(this.gridFtyinventory)
                     .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8))
                     .Text("FullRoll", header: "Full Roll#", width: Widths.AnsiChars(10))
                     .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                     .Text("FullDyelot", header: "Full Dyelot", width: Widths.AnsiChars(10))
                     .Text("stocktype", header: "Stock Type", width: Widths.AnsiChars(10))
                     .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("OutQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 2)
                     .Text("MtlLocationID", header: "Location", width: Widths.AnsiChars(10))
                     .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out col_ContainerCode)
                     .Text("Tone", header: "Ton Grp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                     .Text("GMTWash", header: "GMT Wash", width: Widths.AnsiChars(10), iseditingreadonly: true)
                     ;

                // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
                col_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;

                // 設定Grid2的顯示欄位
                Ict.Win.UI.DataGridViewTextBoxColumn col_ContainerCode2;
                this.gridTrans.IsEditingReadOnly = true;
                this.gridTrans.DataSource = this.bindingSource2;
                this.Helper.Controls.Grid.Generator(this.gridTrans)
                    .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                     .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                     .Text("name", header: "Name", width: Widths.AnsiChars(13))
                     .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Adjust", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Text("GW", header: "G.W(kg)", width: Widths.AnsiChars(7))
                     .Text("ActW", header: "Act.(kg)", width: Widths.AnsiChars(7))
                     .Text("Location", header: "Location", width: Widths.AnsiChars(5))
                     .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out col_ContainerCode2)
                     .Text("ToPOID", header: "To SP#", width: Widths.AnsiChars(12), iseditingreadonly: true)
                     .Text("ToSeq", header: "To Seq", width: Widths.AnsiChars(5), iseditingreadonly: true)
                     ;

                // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
                col_ContainerCode2.Visible = Automation.UtilityAutomation.IsAutomationEnable;

                // 設定Grid3的顯示欄位
                this.gridSummary.IsEditingReadOnly = true;
                this.gridSummary.DataSource = this.bindingSource3;
                this.Helper.Controls.Grid.Generator(this.gridSummary)
                     .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                     .Numeric("rollcount", header: "# of Rolls", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                     .Text("roll", header: "Rolls", width: Widths.AnsiChars(13))
                     .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("DTM", header: "DTM", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     ;
                this.gridSummary.Columns["DTM"].Visible = this.bUseQty;
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Data error ,Please doubleclick 'Balance' field to click 'Re-Calculate' button for recalculate inventory qty, then retry to doubleclick this 'Release Qty' field.!!");
                return;
            }

            this.comboStockType.Text = "ALL";
            this.Change_Color();
        }

        private void Change_Color()
        {
            for (int i = 0; i < this.gridTrans.Rows.Count; i++)
            {
                DataRow dr = this.gridTrans.GetDataRow(i);
                if (this.gridTrans.Rows.Count <= i || i < 0)
                {
                    return;
                }

                if (dr["Name"].ToString() == "P16. Issue Fabric Lacking & Replacement")
                {
                    string sqlcmd = $@"select 1 from Issuelack where id='{dr["id"]}' and type='L'";
                    if (MyUtility.Check.Seek(sqlcmd))
                    {
                        this.gridTrans.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                    }
                    else
                    {
                        this.gridTrans.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ComboStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindingSource1_PositionChanged(sender, e);  // 687: WAREHOUSE_P03_RollTransaction_Transaction Detail by Roll#，1.Grid3值不對
            switch (this.comboStockType.SelectedIndex)
            {
                case 0:
                    this.bindingSource1.Filter = string.Empty;
                    break;
                case 1:
                    this.bindingSource1.Filter = "stocktype='Bulk'";
                    break;
                case 2:
                    this.bindingSource1.Filter = "stocktype='Invertory'";
                    break;
            }
        }

        private void GridFtyinventory_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridFtyinventory.CurrentDataRow == null)
            {
                return;
            }

            string filter = $"roll = '{this.gridFtyinventory.CurrentDataRow["roll"]}' and stocktype = '{this.gridFtyinventory.CurrentDataRow["stocktype"]}' and Dyelot = '{this.gridFtyinventory.CurrentDataRow["Dyelot"]}'";
            this.dtTrans.DefaultView.RowFilter = filter;

            this.Change_Color();
        }

        private void BindingSource1_PositionChanged(object sender, EventArgs e)
        {
            string[] tmpStocktype = new string[] { string.Empty, string.Empty };

            switch (this.comboStockType.SelectedIndex)
            {
                case -1:
                    tmpStocktype[0] = "Bulk";
                    tmpStocktype[1] = "Invertory";
                    break;
                case 0:
                    tmpStocktype[0] = "Bulk";
                    tmpStocktype[1] = "Invertory";
                    break;
                case 1:
                    tmpStocktype[0] = "Bulk";
                    break;
                case 2:
                    tmpStocktype[0] = "Invertory";
                    break;
            }

            var tmp = from b in this.dtFtyinventory.AsEnumerable()
                      where tmpStocktype.Contains(b.Field<string>("StockType"))
                      group b by new
                      {
                          Dyelot = b.Field<string>("Dyelot"),
                      }
                        into m
                      select new
                      {
                          dyelot = m.First().Field<string>("Dyelot"),
                          rollcount = m.Count(),
                          roll = string.Join(";", m.Select(r => r.Field<string>("roll")).Distinct()),
                          inqty = m.Sum(w => w.Field<decimal>("inqty")),
                          outQty = m.Sum(w => w.Field<decimal>("outqty")),
                          AdjustQty = m.Sum(i => i.Field<decimal>("AdjustQty")),
                          ReturnQty = m.Sum(i => i.Field<decimal>("ReturnQty")),
                          balance = m.Sum(w => w.Field<decimal>("inqty")) - m.Sum(w => w.Field<decimal>("outqty")) + m.Sum(i => i.Field<decimal>("AdjustQty")) - m.Sum(i => i.Field<decimal>("ReturnQty")),
                          DTM = this.numArrivedQtyBySeq.Value == 0 ? 0 : m.Sum(w => w.Field<decimal>("inqty")) / this.numArrivedQtyBySeq.Value * this.useQty,
                      };

            this.dtSummary.Rows.Clear();
            foreach (var item in tmp)
            {
                DataRow newdr = this.dtSummary.NewRow();
                newdr["roll"] = item.roll;
                newdr["dyelot"] = item.dyelot;
                newdr["inqty"] = item.inqty;
                newdr["outQty"] = item.outQty;
                newdr["AdjustQty"] = item.AdjustQty;
                newdr["ReturnQty"] = item.ReturnQty;
                newdr["balance"] = item.balance;
                newdr["rollcount"] = item.rollcount;
                newdr["DTM"] = item.DTM;
                this.dtSummary.Rows.Add(newdr);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataRow row = this.dr;
            string id = row["ID"].ToString();
            string seq1 = row["seq1"].ToString();
            string seq2 = row["seq2"].ToString();
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            pars.Add(new SqlParameter("@seq1", seq1));
            pars.Add(new SqlParameter("@seq2", seq2));
            DualResult result;
            DataTable dtt, dt;
            string sqlcmd = @"
select  a.id [SP]
        , a.SEQ1+'-'+a.SEQ2 [SEQ]
        , a.Refno [Ref]
        , isnull(psdsC.SpecValue, '') [Color]
        , b.InQty [Arrived_Qty_by_Seq]
        , b.OutQty [Released_Qty_by_Seq]
        , b.InQty-b.OutQty+b.AdjustQty-b.ReturnQty [Bal_Qty]
        , [Description] = dbo.getMtlDesc(a.id,a.SEQ1,a.SEQ2,2,0) 
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.MDivisionPoDetail b WITH (NOLOCK) on a.id = b.POID and a.SEQ1 = b.Seq1 and a.SEQ2=b.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = a.id and psdsC.seq1 = a.seq1 and psdsC.seq2 = a.seq2 and psdsC.SpecColumnID = 'Color'
where   a.id = @ID 
        and a.seq1 = @seq1 
        and a.seq2=@seq2
";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string cmdText = @"
select  c.Roll[Roll], rd.FullRoll
        , c.Dyelot [Dyelot], rd.FullDyelot		
        , [Stock_Type] = Case c.StockType 
                            when 'B' THEN 'Bulk' 
                            WHEN 'I' THEN 'Inventory' 
                            ELSE 'Scrap' 
                         END
        , c.InQty [Arrived_Qty]
        , c.OutQty [Released_Qty]
        , c.AdjustQty [Adjust_Qty]
        , c.ReturnQty [Return_Qty]
        , c.InQty-c.OutQty+c.AdjustQty-c.ReturnQty [Balance]
        , [Location]=dbo.Getlocation(c.Ukey)
from dbo.FtyInventory c WITH (NOLOCK) 
left join Receiving_Detail rd WITH (NOLOCK) on c.POID = rd.PoId and c.Seq1 = rd.Seq1
and c.Seq2 = rd.Seq2 and c.Dyelot = rd.Dyelot and c.Roll = rd.Roll and c.StockType = rd.StockType
where   c.poid = @ID 
        and c.seq1 = @seq1 
        and c.seq2 = @seq2";
            DBProxy.Current.Select(string.Empty, cmdText, pars, out dtt);
            if (dtt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P03_RollTransaction.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format(
                @"
select NameEn
from Factory
where id = '{0}'", Env.User.Keyword));
            objSheets.Cells[3, 2] = MyUtility.Convert.GetString(dt.Rows[0]["SP"].ToString());
            objSheets.Cells[3, 4] = MyUtility.Convert.GetString(dt.Rows[0]["SEQ"].ToString());
            objSheets.Cells[3, 6] = MyUtility.Convert.GetString(dt.Rows[0]["REF"].ToString());
            objSheets.Cells[3, 8] = MyUtility.Convert.GetString(dt.Rows[0]["Color"].ToString());
            objSheets.Cells[4, 2] = MyUtility.Convert.GetString(dt.Rows[0]["Arrived_Qty_by_Seq"].ToString());
            objSheets.Cells[4, 4] = MyUtility.Convert.GetString(dt.Rows[0]["Released_Qty_by_Seq"].ToString());
            objSheets.Cells[4, 6] = MyUtility.Convert.GetString(dt.Rows[0]["Bal_Qty"].ToString());
            objSheets.Cells[5, 2] = MyUtility.Convert.GetString(dt.Rows[0]["Description"].ToString());

            MyUtility.Excel.CopyToXls(dtt, string.Empty, "Warehouse_P03_RollTransaction.xltx", 6, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
            return;
        }
    }
}