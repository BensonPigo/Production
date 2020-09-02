using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using System.Linq;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P03_RollTransaction : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable dtFtyinventory;
        private DataTable dtTrans;
        private DataTable dtSummary;
        private DataSet data = new DataSet();
        private decimal useQty = 0;
        private bool bUseQty = false;

        public P03_RollTransaction(DataRow data)
        {
            this.InitializeComponent();

            this.dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Text += string.Format(@" ({0}-{1}-{2})", this.dr["id"], this.dr["seq1"], this.dr["seq2"]);  // 351: WAREHOUSE_P03_RollTransaction_Transaction Detail by Roll#，3.Tool bar要帶出SP# & Seq
            this.displaySeqNo.Text = this.dr["seq1"].ToString() + "-" + this.dr["seq2"].ToString();
            this.displayDescription.Text = MyUtility.GetValue.Lookup(string.Format("select dbo.getmtldesc('{0}','{1}','{2}',2,0)", this.dr["id"].ToString(), this.dr["seq1"].ToString(), this.dr["seq2"].ToString()));
            this.numArrivedQtyBySeq.Value = MyUtility.Check.Empty(this.dr["inqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["inqty"].ToString());
            this.numReleasedQtyBySeq.Value = MyUtility.Check.Empty(this.dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["outqty"].ToString());

            // this.numericBox3.Value = (MyUtility.Check.Empty(dr["inqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["inqty"].ToString())) -
            //  ( MyUtility.Check.Empty(dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["outqty"].ToString())) +(MyUtility.Check.Empty(dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["adjustqty"].ToString()));
            decimal iN = MyUtility.Check.Empty(this.dr["inqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["inqty"].ToString());
            decimal oUT = MyUtility.Check.Empty(this.dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["outqty"].ToString());
            decimal aDJ = MyUtility.Check.Empty(this.dr["adjustqty"]) ? decimal.Parse("0.00") : decimal.Parse(this.dr["adjustqty"].ToString());
            this.numBalQtyBySeq.Value = iN - oUT + aDJ;

            #region "顯示DTM"
            DataTable dtmDt;
            string sql = string.Format(
                @"
                select id,seq1,seq2,qty,ShipQty,Refno,ColorID,iif(qty <= shipqty, 'True','False') bUseQty
                into #tmp
                from Po_Supp_Detail
                where id = '{0}'
                and seq1 = '{1}'
                and seq2 = '{2}'
 
                select isnull(Round(dbo.getUnitQty(a.POUnit, a.StockUnit, (isnull(A.NETQty,0)+isnull(A.lossQty,0))), 2),0.0) as UseQty,b.bUseQty
                from Po_Supp_Detail a
                inner join #tmp b on a.id = b.id and a.Refno = b.Refno and a.ColorID = b.ColorID and b.bUseQty = 'True' 
                where a.id = '{0}'
                and a.seq1 = 'A1' 

                drop table #tmp
            ", this.dr["id"].ToString(), this.dr["seq1"].ToString(), this.dr["seq2"].ToString());
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
                    @"Select a.Roll,a.Dyelot
                                ,[stocktype] = case when stocktype = 'B' then 'Bulk'
                                                    when stocktype = 'I' then 'Invertory'
			                                        when stocktype = 'O' then 'Scrap' End
                                                ,a.InQty,a.OutQty,a.AdjustQty
                                                ,a.InQty - a.OutQty + a.AdjustQty as balance
                                                ,dbo.Getlocation(a.ukey)  MtlLocationID 
                                            from FtyInventory a WITH (NOLOCK) 
                                            where a.Poid = '{0}'
                                                and a.Seq1 = '{1}'
                                                and a.Seq2 = '{2}' 
                                                --and MDivisionPoDetailUkey is not null  --避免下面Relations發生問題
                                                --and MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
                                                and StockType <> 'O'  --C倉不用算
                                            order by a.dyelot,a.roll,a.stocktype",
                    this.dr["id"].ToString(),
                    this.dr["seq1"].ToString(),
                    this.dr["seq2"].ToString(),
                    Env.User.Keyword);
            #endregion

            #region Grid2 - Sql Command

            string selectCommand2
                = string.Format(
                    @"select tmp.Roll,
[stocktype] = case when stocktype = 'B' then 'Bulk'
                   when stocktype = 'I' then 'Invertory'
			       when stocktype = 'O' then 'Scrap' End
,Dyelot,IssueDate,ID,name,inqty,outqty,adjust,Remark,location,
sum(TMP.inqty - TMP.outqty+tmp.adjust) 
over (partition by tmp.stocktype,tmp.roll,tmp.dyelot order by tmp.IssueDate,tmp.stocktype,tmp.inqty desc,tmp.iD ) as [balance] 
from (
	select b.roll,b.stocktype,b.dyelot,a.IssueDate, a.id
,Case type when 'A' then 'P35. Adjust Bulk Qty' 
                when 'B' then 'P34. Adjust Stock Qty' end as name
,0 as inqty,0 as outqty, sum(QtyAfter - QtyBefore) adjust, a.remark ,'' location
from Adjust a WITH (NOLOCK) , Adjust_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,type,b.roll,b.stocktype,b.dyelot

union all
	select b.FromRoll,b.FromStockType,b.FromDyelot,a.IssueDate, a.id
,case type when 'A' then 'P31. Material Borrow From' 
                when 'B' then 'P32. Material Give Back From' end as name
,0 as inqty, sum(qty) released,0 as adjust, a.remark ,'' location
from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
where Status='Confirmed' and FromPoId ='{0}' and FromSeq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, FromPoId, FromSeq1,FromSeq2, a.remark,a.IssueDate,b.FromRoll,b.FromStockType,b.FromDyelot,a.type
union all
	select b.ToRoll,b.ToStockType,b.ToDyelot,issuedate, a.id
,case type when 'A' then 'P31. Material Borrow To' 
                when 'B' then 'P32. Material Give Back To' end as name
, sum(qty) arrived,0 as ouqty,0 as adjust, a.remark ,'' location
from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
where Status='Confirmed' and ToPoid ='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, ToPoid, ToSeq1,ToSeq2, a.remark,a.IssueDate,b.ToRoll,b.ToStockType,b.ToDyelot,a.type
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case type when 'A' then 'P10. Issue Fabric to Cutting Section' 
			when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
			when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
			when 'D' then 'P13. Issue Material by Item'
			when 'E' then 'P33. Issue Thread'
    end name
	,0 as inqty, sum(Qty) released,0 as adjust, a.remark,'' location
from Issue a WITH (NOLOCK) , Issue_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,a.type,b.roll,b.stocktype,b.dyelot,a.type                                                             
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
                              when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
	, 0 as inqty,sum(b.Qty) outqty ,0 as adjust, a.remark ,'' location
from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
where Status in ('Confirmed','Closed') and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    and a.type != 'L'  --新增MDivisionID條件，避免下面DataRelation出錯 1026新增排除Lacking
group by a.id, poid, seq1,Seq2, a.remark  ,a.IssueDate,a.FabricType,b.roll,b.stocktype,b.dyelot                        



union all

	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
                              when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
	, 0 as inqty,0 outqty ,0 as adjust, a.remark ,'' location
from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
where Status in ('Confirmed','Closed') and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
and a.type = 'L'  --20190305 新增Type= Lacking,則OutQty = 0
group by a.id, poid, seq1,Seq2, a.remark  ,a.IssueDate,a.FabricType,b.roll,b.stocktype,b.dyelot   
                                       
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id,'P17. R/Mtl Return' name, 0 as inqty, sum(0.00 - b.Qty) released,0 as adjust, remark,'' location
from IssueReturn a WITH (NOLOCK) , IssueReturn_Detail b WITH (NOLOCK) 
where status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.Id, poid, seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                           
union all
	select b.roll,b.stocktype,b.dyelot
        ,case type when 'A' then a.ETA else a.WhseArrival end as issuedate
        , a.id
	    ,case type when 'A' then 'P07. Material Receiving' 
                        when 'B' then 'P08. Warehouse Shopfloor Receiving' end name
	    , sum(b.StockQty) inqty,0 as outqty,0 as adjust,'' remark ,'' location
    from Receiving a WITH (NOLOCK) , Receiving_Detail b WITH (NOLOCK) 
    where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
        --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
    group by a.Id, poid, seq1,Seq2,a.WhseArrival,a.Type,b.roll,b.stocktype,b.dyelot,a.eta
union all
	select b.roll,b.stocktype,b.dyelot,issuedate
, a.id,'P37. Return Receiving Material' name, sum(-Qty) inqty,0 as released,0 as adjust, a.remark,'' location
from ReturnReceipt a WITH (NOLOCK) , ReturnReceipt_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯 
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                           
union all
	select b.FromRoll,b.FromStockType,b.FromDyelot,issuedate, a.id
	,case type when 'B' then 'P23. Transfer Inventory to Bulk' 
                    when 'A' then 'P22. Transfer Bulk to Inventory' 
                    when 'C' then 'P36. Transfer Scrap to Inventory' 
                    when 'D' then 'P25. Transfer Bulk to Scrap' 
                    when 'E' then 'P24. Transfer Inventory to Scrap'
    end as name
	, 0 as inqty, sum(Qty) released,0 as adjust ,isnull(a.remark,'') remark ,'' location
from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
where Status='Confirmed' and Frompoid='{0}' and Fromseq1 = '{1}' and FromSeq2 = '{2}'  and a.id = b.id
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
    and a.type <> 'C'  --排除C to B 的轉出紀錄，因目前不需要C倉交易紀錄，避免下面DataRelation出錯
group by a.id, frompoid, FromSeq1,FromSeq2,a.IssueDate,a.Type,b.FromRoll,b.FromStockType,b.FromDyelot,a.Type,a.remark
                                                                             
union all
	select b.ToRoll,b.ToStockType,b.ToDyelot,issuedate, a.id
	,case type when 'B' then 'P23. Transfer Inventory to Bulk' 
                    when 'A' then 'P22. Transfer Bulk to Inventory' 
                    when 'C' then 'P36. Transfer Scrap to Inventory' end as name
	        , sum(Qty) arrived,0 as ouqty,0 as adjust, a.remark
	        ,isnull((Select cast(tmp.ToLocation as nvarchar)+',' 
                        from (select b1.ToLocation 
                                    from SubTransfer a1 WITH (NOLOCK) 
                                    inner join SubTransfer_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                    where a1.status = 'Confirmed' and (b1.ToLocation is not null or b1.ToLocation !='')
                                        and b1.ToPoid = b.ToPoid
                                        and b1.ToSeq1 = b.ToSeq1
                                        and b1.ToSeq2 = b.ToSeq2 group by b1.ToLocation) tmp 
                        for XML PATH('')),'') as ToLocation
from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
where Status='Confirmed' and ToPoid='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id  
    AND TYPE not in ('D','E')  --570: WAREHOUSE_P03_RollTransaction。C倉不用算，所以要把TYPE為D及E的資料濾掉
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, ToPoid, ToSeq1,ToSeq2, a.remark ,a.IssueDate,b.ToRoll,b.ToStockType,b.ToDyelot,a.type	    

union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
            ,'P18. Transfer In' name
            , sum(Qty) arrived,0 as ouqty,0 as adjust, a.remark
	,(Select cast(tmp.Location as nvarchar)+',' 
                        from (select b1.Location 
                                    from TransferIn a1 WITH (NOLOCK) 
                                    inner join TransferIn_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                    where a1.status = 'Confirmed' and (b1.Location is not null or b1.Location !='')
                                        and b1.Poid = b.Poid
                                        and b1.Seq1 = b.Seq1
                                        and b1.Seq2 = b.Seq2 group by b1.Location) tmp 
                        for XML PATH('')) as Location
from TransferIn a WITH (NOLOCK) , TransferIn_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                        
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
            ,'P19. Transfer Out' name
            , 0 as inqty, sum(Qty) released,0 as adjust, a.remark,'' location
from TransferOut a WITH (NOLOCK) , TransferOut_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, Seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot

union all
select b.roll,b.stocktype,b.dyelot,issuedate, a.id
    ,case type when 'B' then 'P73. Transfer Inventory to Bulk cross M (Receive)' 
	when 'D' then 'P76. Material Borrow cross M (Receive)' 
	when 'G' then 'P78. Material Return Back cross M (Receive)'  end name
    , sum(Qty) as inqty, 0 released,0 as adjust, a.remark,'' location
from RequestCrossM a WITH (NOLOCK) , RequestCrossM_Receive b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.ToMDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,a.type,b.roll,b.stocktype,b.dyelot,a.type 

) tmp where stocktype <> 'O'
group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name,tmp.roll,tmp.stocktype,tmp.dyelot
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

            this.dtTrans.TableName = "dtTrans";
            this.data.Tables.Add(this.dtFtyinventory);
            this.data.Tables.Add(this.dtTrans);
            this.data.Tables.Add("dtSummary");

            // remove [Dyelot] DataRelation
            // DataRelation relation = new DataRelation("rel1"
            //    , new DataColumn[] { dtFtyinventory.Columns["Roll"], dtFtyinventory.Columns["Dyelot"], dtFtyinventory.Columns["StockType"] }
            //    , new DataColumn[] { dtTrans.Columns["roll"], dtTrans.Columns["dyelot"], dtTrans.Columns["stocktype"] }
            //    );
            // 105.12.23 Jimmy
            if (this.dtFtyinventory.Rows.Count == 0 || this.dtTrans.Rows.Count == 0)
            {
                // MyUtility.Msg.ErrorBox("Data not found!!");
                return;
            }

            try
            {
                DataRelation relation = new DataRelation(
                    "Rol1",
                    new DataColumn[] { this.dtFtyinventory.Columns["Roll"], this.dtFtyinventory.Columns["StockType"], this.dtFtyinventory.Columns["Dyelot"] },
                    new DataColumn[] { this.dtTrans.Columns["roll"], this.dtTrans.Columns["stocktype"], this.dtTrans.Columns["Dyelot"] });

                this.data.Relations.Add(relation);
                this.bindingSource1.DataSource = this.data;
                this.bindingSource1.DataMember = "dtFtyinventory";
                this.bindingSource2.DataSource = this.bindingSource1;
                this.bindingSource2.DataMember = "Rol1";

                // 設定Grid1的顯示欄位
                this.gridFtyinventory.IsEditingReadOnly = true;
                this.gridFtyinventory.DataSource = this.bindingSource1;
                this.Helper.Controls.Grid.Generator(this.gridFtyinventory)
                     .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8))
                     .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                     .Text("stocktype", header: "Stock Type", width: Widths.AnsiChars(10))
                     .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("OutQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 2)
                     .Text("MtlLocationID", header: "Location", width: Widths.AnsiChars(10))
                     ;

                // 設定Grid2的顯示欄位
                this.gridTrans.IsEditingReadOnly = true;
                this.gridTrans.DataSource = this.bindingSource2;
                this.Helper.Controls.Grid.Generator(this.gridTrans)
                    .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                     .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                     .Text("name", header: "Name", width: Widths.AnsiChars(13))
                     .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Adjust", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2);

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
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("DTM", header: "DTM", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     ;
                this.gridSummary.Columns["DTM"].Visible = this.bUseQty;
            }
            catch
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
                           balance = m.Sum(w => w.Field<decimal>("inqty")) - m.Sum(w => w.Field<decimal>("outqty")) + m.Sum(i => i.Field<decimal>("AdjustQty")),
                           DTM = this.numArrivedQtyBySeq.Value == 0 ? 0 : m.Sum(w => w.Field<decimal>("inqty")) / this.numArrivedQtyBySeq.Value * this.useQty,
                       };

            this.dtSummary.Rows.Clear();
            tmp.ToList().ForEach(q2 => this.dtSummary.Rows.Add(q2.roll, q2.dyelot, null, q2.inqty, q2.outQty, q2.AdjustQty, q2.balance, null, q2.rollcount, q2.DTM));
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
            string sqlcmd = string.Format(@"
select  a.id [SP]
        , a.SEQ1+'-'+a.SEQ2 [SEQ]
        , a.Refno [Ref]
        , a.ColorID [Color]
        , b.InQty [Arrived_Qty_by_Seq]
        , b.OutQty [Released_Qty_by_Seq]
        , b.InQty-b.OutQty+b.AdjustQty [Bal_Qty]
        , [Description] = dbo.getMtlDesc(a.id,a.SEQ1,a.SEQ2,2,0) 
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.MDivisionPoDetail b WITH (NOLOCK) on a.id = b.POID 
                                                    and a.SEQ1 = b.Seq1 
                                                    and a.SEQ2=b.Seq2
where   a.id = @ID 
        and a.seq1 = @seq1 
        and a.seq2=@seq2");
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            DBProxy.Current.Select(string.Empty, @"
select  c.Roll[Roll]
        , c.Dyelot [Dyelot]
        , [Stock_Type] = Case c.StockType 
                            when 'B' THEN 'Bulk' 
                            WHEN 'I' THEN 'Inventory' 
                            ELSE 'Scrap' 
                         END
        , c.InQty [Arrived_Qty]
        , c.OutQty [Released_Qty]
        , c.AdjustQty [Adjust_Qty]
        , c.InQty-c.OutQty+c.AdjustQty [Balance]
        , [Location]=dbo.Getlocation(c.Ukey)
from dbo.FtyInventory c WITH (NOLOCK) 
where   c.poid = @ID 
        and c.seq1 = @seq1 
        and c.seq2 = @seq2", pars, out dtt);
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