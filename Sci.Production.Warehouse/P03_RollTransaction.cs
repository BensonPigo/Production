using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P03_RollTransaction : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable dtFtyinventory, dtTrans, dtSummary;
        DataSet data = new DataSet();
        public P03_RollTransaction(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.displayBox1.Text = dr["seq1"].ToString() + "-" + dr["seq2"].ToString();
            this.displayBox2.Text = MyUtility.GetValue.Lookup(string.Format("select dbo.getmtldesc('{0}','{1}','{2}',2,0)", dr["id"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString()));
            this.numericBox1.Value = decimal.Parse(dr["inqty"].ToString());
            this.numericBox2.Value = decimal.Parse(dr["outqty"].ToString());
            this.numericBox3.Value = decimal.Parse(dr["inqty"].ToString()) - decimal.Parse(dr["outqty"].ToString()) + decimal.Parse(dr["adjustqty"].ToString());

            #region Grid1 - Sql command
            string selectCommand1
                = string.Format(@"Select a.Roll,a.Dyelot,a.StockType,a.InQty,a.OutQty,a.AdjustQty
                                            ,a.InQty - a.OutQty + a.AdjustQty as balance
                                            , (Select cast(tmp.MtlLocationID as nvarchar)+',' 
                                                                    from (select b.MtlLocationID 
                                                                                from FtyInventory_Detail b
                                                                                where a.Ukey = b.Ukey 
                                                                                    group by b.MtlLocationID) tmp 
                                                                    for XML PATH('')) as  MtlLocationID 
                                            from FtyInventory a 
                                            where a.Poid = '{0}'
                                            and a.Seq1 = '{1}'
                                            and a.Seq2 = '{2}' order by a.dyelot,a.roll,a.stocktype"
                , dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString());
            #endregion

            #region Grid2 - Sql Command
            
            string selectCommand2
                = string.Format(@"select *,
sum(TMP.inqty - TMP.outqty+tmp.adjust) 
over (partition by tmp.stocktype,tmp.roll,tmp.dyelot order by tmp.stocktype,tmp.IssueDate,tmp.iD ) as [balance] 
from (
	select b.roll,b.stocktype,b.dyelot,a.IssueDate, a.id
,Case type when 'A' then 'P35. Adjust Bulk Qty' when 'B' then 'P34. Adjust Stock Qty' end as name
,0 as inqty,0 as outqty, sum(QtyAfter - QtyBefore) adjust, remark ,'' location
from Adjust a, Adjust_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, poid, seq1,Seq2, remark,a.IssueDate,type,b.roll,b.stocktype,b.dyelot
union all
	select b.FromRoll,b.FromStock,b.FromDyelot,a.IssueDate, a.id
,case type when 'A' then 'P31. Material Borrow From' 
                            when 'B' then 'P32. Material Give Back From' end as name
,0 as inqty, sum(qty) released,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where Status='Confirmed' and FromPoId ='{0}' and FromSeq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id 
group by a.id, FromPoId, FromSeq1,FromSeq2, remark,a.IssueDate,b.FromRoll,b.FromStock,b.FromDyelot,a.type
union all
	select b.ToRoll,b.ToStock,b.ToDyelot,issuedate, a.id
,case type when 'A' then 'P31. Material Borrow To' 
                            when 'B' then 'P32. Material Give Back To' end as name
, sum(qty) arrived,0 as ouqty,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where Status='Confirmed' and ToPoid ='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id 
group by a.id, ToPoid, ToSeq1,ToSeq2, remark,a.IssueDate,b.ToRoll,b.ToStock,b.ToDyelot,a.type
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case type when 'A' then 'P10. Issue Fabric to Cutting Section' 
                    when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
                    when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
                    when 'D' then 'P13. Issue Material by Item' end name
	,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from Issue a, Issue_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, poid, seq1,Seq2, remark,a.IssueDate,a.type,b.roll,b.stocktype,b.dyelot,a.type                                                             
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
                            when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
	, 0 as inqty,sum(b.Qty) outqty ,0 as adjust, remark ,'' location
from IssueLack a, IssueLack_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, poid, seq1,Seq2, remark  ,a.IssueDate,a.FabricType,b.roll,b.stocktype,b.dyelot                                                               
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id,'P17. R/Mtl Return' name, 0 as inqty, sum(0.00 - b.Qty) released,0 as adjust, remark,'' location
from IssueReturn a, IssueReturn_Detail b 
where status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.Id, poid, seq1,Seq2, remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                           
union all
	select b.roll,b.stocktype,b.dyelot
        ,case type when 'A' then a.ETA else a.WhseArrival end as issuedate, a.id
	    ,case type when 'A' then 'P07. Material Receiving' 
                        when 'B' then 'P08. Warehouse Shopfloor Receiving' end name
	    , sum(b.StockQty) inqty,0 as outqty,0 as adjust,'' remark ,'' location
    from Receiving a, Receiving_Detail b 
    where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    group by a.Id, poid, seq1,Seq2,a.WhseArrival,a.Type,b.roll,b.stocktype,b.dyelot,a.eta
union all
	select b.roll,b.stocktype,b.dyelot,issuedate,'P37. Return Receiving Material' name, a.id, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from ReturnReceipt a, ReturnReceipt_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, poid, seq1,Seq2, remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                           
union all
	select b.roll,case a.type when 'A' then 'B' when 'B' then 'I' end as stocktype,b.dyelot,issuedate, a.id
	,case a.type when 'A' then 'P25. Transfer Bulk to Scrap' when 'B' then 'P24. Transfer Inventory to Scrap' end as name
	,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from Scrap a, Scrap_Detail b 
where Status='Confirmed' and Poid ='{0}' and Seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, Poid, Seq1,Seq2, remark,a.IssueDate,a.Type,b.roll,b.dyelot
union all
	select b.FromRoll,b.FromStock,b.FromDyelot,issuedate, a.id
	,case type when 'B' then 'P23. Transfer Inventory to Bulk' 
                    when 'A' then 'P22. Transfer Bulk to Inventory' 
                    when 'C' then 'P36. Transfer Scrap to Inventory' end as name
	, 0 as inqty, sum(Qty) released,0 as adjust , '' remark ,'' location
from SubTransfer a, SubTransfer_Detail b 
where Status='Confirmed' and Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id
group by a.id, frompoid, FromSeq1,FromSeq2,a.IssueDate,a.Type,b.FromRoll,b.FromStock,b.FromDyelot,a.Type                                                                             
union all
	select b.ToRoll,b.ToStock,b.ToDyelot,issuedate, a.id
	        ,'P23. Transfer Inventory to Bulk' 
	        , sum(Qty) arrived,0 as ouqty,0 as adjust, remark
	,(Select cast(tmp.ToLocation as nvarchar)+',' 
                        from (select b1.ToLocation 
                                    from SubTransfer a1 
                                    inner join SubTransfer_Detail b1 on a1.id = b1.id 
                                    where a1.status = 'Confirmed' and (b1.ToLocation is not null or b1.ToLocation !='')
                                        and b1.ToPoid = b.ToPoid
                                        and b1.ToSeq1 = b.ToSeq1
                                        and b1.ToSeq2 = b.ToSeq2 group by b1.ToLocation) tmp 
                        for XML PATH('')) as ToLocation
from SubTransfer a, SubTransfer_Detail b 
where Status='Confirmed' and ToPoid='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id and type='B'
group by a.id, ToPoid, ToSeq1,ToSeq2, remark ,a.IssueDate,b.ToRoll,b.ToStock,b.ToDyelot,a.type	    
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
            ,'P18. Transfer In' name
            , sum(Qty) arrived,0 as ouqty,0 as adjust, remark
	,(Select cast(tmp.Location as nvarchar)+',' 
                        from (select b1.Location 
                                    from TransferIn a1 
                                    inner join TransferIn_Detail b1 on a1.id = b1.id 
                                    where a1.status = 'Confirmed' and (b1.Location is not null or b1.Location !='')
                                        and b1.Poid = b.Poid
                                        and b1.Seq1 = b.Seq1
                                        and b1.Seq2 = b.Seq2 group by b1.Location) tmp 
                        for XML PATH('')) as Location
from TransferIn a, TransferIn_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, poid, seq1,Seq2, remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                        
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
            ,'P19. Transfer Out' name
            , 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from TransferOut a, TransferOut_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, poid, Seq1,Seq2, remark,a.IssueDate,b.roll,b.stocktype,b.dyelot) tmp
group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name,tmp.roll,tmp.stocktype,tmp.dyelot
order by stocktype,IssueDate,name,id,Dyelot,Roll"
                , dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString());

            #endregion

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out dtFtyinventory);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);
            dtFtyinventory.TableName = "dtFtyinventory";
            dtSummary = dtFtyinventory.Clone();
            dtSummary.Columns.Add("rollcount", typeof(int));
            bindingSource3.DataSource = dtSummary;

            DualResult selectResult2 = DBProxy.Current.Select(null, selectCommand2, out dtTrans);
            dtTrans.TableName = "dtTrans";
            if (selectResult2 == false) ShowErr(selectCommand2, selectResult1);
            data.Tables.Add(dtFtyinventory);
            data.Tables.Add(dtTrans);
            //data.Tables.Add("dtSummary");

            DataRelation relation = new DataRelation("rel1"
                , new DataColumn[] { dtFtyinventory.Columns["Roll"], dtFtyinventory.Columns["Dyelot"], dtFtyinventory.Columns["StockType"] }
                , new DataColumn[] { dtTrans.Columns["roll"], dtTrans.Columns["dyelot"], dtTrans.Columns["stocktype"] }
                );
            data.Relations.Add(relation);
            bindingSource1.DataSource = data;
            bindingSource1.DataMember = "dtFtyinventory";
            bindingSource2.DataSource = bindingSource1;
            bindingSource2.DataMember = "rel1";

            //設定Grid1的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid1);
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8))
                 .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(4))
                 .Text("stocktype", header: "Stock Type", width: Widths.AnsiChars(10))
                 .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("OutQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 2)
                 .Text("MtlLocationID", header: "Location", width: Widths.AnsiChars(10))
                 ;

            //設定Grid2的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid2);
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = bindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                 .Text("name", header: "Name", width: Widths.AnsiChars(13))
                 .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjust", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2);

            //設定Grid3的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid3);
            this.grid3.IsEditingReadOnly = true;
            this.grid3.DataSource = bindingSource3;
            Helper.Controls.Grid.Generator(this.grid3)
                 .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6))
                 .Numeric("rollcount", header: "# of Rolls", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Text("roll", header: "Rolls", width: Widths.AnsiChars(13))
                 .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjust", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    bindingSource1.Filter = "";
                    break;
                case 1:
                    bindingSource1.Filter = "stocktype='B'";
                    break;
                case 2:
                    bindingSource1.Filter = "stocktype='I'";
                    break;
            }
        }

        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            var tmp = (from b in dtFtyinventory.AsEnumerable()
                       group b by new
                       {
                           Dyelot = b.Field<string>("Dyelot")
                       } into m
                       select new
                       {
                           dyelot = m.First().Field<string>("Dyelot"),
                           rollcount = m.Count(),
                           roll = string.Join(";", m.Select(r => r.Field<string>("roll")).Distinct()),
                           inqty = m.Sum(w => w.Field<decimal>("inqty")),
                           outQty = m.Sum(w => w.Field<decimal>("outqty")),
                           Adjust = m.Sum(i => i.Field<decimal>("AdjustQty")),
                           balance = m.Sum(w => w.Field<decimal>("inqty")) - m.Sum(w => w.Field<decimal>("outqty")) + m.Sum(i => i.Field<decimal>("AdjustQty"))
                       });

            dtSummary.Rows.Clear();
            tmp.ToList().ForEach(q2 => dtSummary.Rows.Add(q2.roll, q2.dyelot, null, q2.inqty, q2.outQty, q2.Adjust, q2.balance, null, q2.rollcount));
            
        }
    }
}
