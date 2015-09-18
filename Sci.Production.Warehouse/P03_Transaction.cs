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
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P03_Transaction : Sci.Win.Subs.Base
    {
        DataRow dr;
        bool _byroll;   // 從p20呼叫時，會傳入true

        public P03_Transaction(DataRow data, bool byRoll = false)
        {
            InitializeComponent();
            dr = data;
            _byroll = byRoll;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            button2.Enabled = !_byroll;     // 從p20呼叫時，不開啟。
            button2.Visible = !_byroll;     // 從p20呼叫時，不開啟。

            if (_byroll)
            {
                this.Text += string.Format(@" ({0}-{1}-{2}-{3}-{4})", dr["id"], dr["seq1"], dr["seq2"],dr["roll"],dr["dyelot"]);
            }
            else
            {
                this.Text += string.Format(@" ({0}-{1}-{2})", dr["id"], dr["seq1"], dr["seq2"]);
            }

            #region sql command
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(@"select *,
sum(TMP.inqty - TMP.outqty+tmp.adjust) over ( order by tmp.IssueDate,TMP.inqty desc, TMP.outqty,tmp.adjust) as [balance] 
from (
	select a.IssueDate, a.id
,Case type when 'A' then 'P35. Adjust Bulk Qty' when 'B' then 'P34. Adjust Stock Qty' end as name
,0 as inqty,0 as outqty, sum(QtyAfter - QtyBefore) adjust, remark ,'' location
from Adjust a, Adjust_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));

            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, poid, seq1,Seq2, remark,a.IssueDate,type
union all
	select a.IssueDate, a.id
,'P31. Material Borrow out' name
,0 as inqty, sum(qty) released,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where type='A' and Status='Confirmed' and FromPoId ='{0}' and FromSeq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));

            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and fromroll='{0}' and fromdyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, FromPoId, FromSeq1,FromSeq2, remark,a.IssueDate
union all
	select issuedate, a.id
,'P31. Material Borrow In' name, sum(qty) arrived,0 as ouqty,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where type='A' and Status='Confirmed' and ToPoid ='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                            , dr["seq1"].ToString()
                            , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and Toroll='{0}' and Todyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, ToPoid, ToSeq1,ToSeq2, remark,a.IssueDate
union all
	select a.IssueDate, a.id
,'P32. Return Borrowing out' name
,0 as inqty, sum(qty) released,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where type='B' and Status='Confirmed' and FromPoId ='{0}' and FromSeq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));

            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and fromroll='{0}' and fromdyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, FromPoId, FromSeq1,FromSeq2, remark,a.IssueDate
union all
	select issuedate, a.id
,'P32. Return Borrowing In' name, sum(qty) arrived,0 as ouqty,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where type='B' and Status='Confirmed' and ToPoid ='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                            , dr["seq1"].ToString()
                            , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and Toroll='{0}' and Todyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, ToPoid, ToSeq1,ToSeq2, remark,a.IssueDate
union all
	select issuedate, a.id
	,case type when 'A' then 'P10. Issue Fabric to Cutting Section' 
when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
when 'D' then 'P13. Issue Material by Item' end name
	,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from Issue a, Issue_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                            , dr["seq1"].ToString()
                            , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, poid, seq1,Seq2, remark,a.IssueDate,a.type                                                                          
union all
	select issuedate, a.id
	,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
	, 0 as inqty,sum(b.Qty) outqty ,0 as adjust, remark ,'' location
from IssueLack a, IssueLack_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, poid, seq1,Seq2, remark  ,a.IssueDate,a.FabricType                                                               
union all
	select issuedate, a.id
,'P17. R/Mtl Return' name
, 0 as inqty, sum(b.Qty) released,0 as adjust, remark,'' location
from IssueReturn a, IssueReturn_Detail b 
where status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.Id, poid, seq1,Seq2, remark,a.IssueDate                                                                                 
union all
	select case type when 'A' then a.eta else a.WhseArrival end as issuedate, a.id
	,case type when 'A' then 'P07. Material Receiving' 
                    when 'B' then 'P08. Warehouse Shopfloor Receiving' end name
	, sum(b.StockQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
from Receiving a, Receiving_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.Id, poid, seq1,Seq2,a.WhseArrival,a.Type,a.eta
union all
	select issuedate
    ,'P37. Return Receiving Material' name
    , a.id, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from ReturnReceipt a, ReturnReceipt_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, poid, seq1,Seq2, remark,a.IssueDate                                                                               
union all
	select issuedate, a.id
	,case a.type when 'A' then 'P25. Transfer Bulk to Scrap' 
                    when 'B' then 'P24. Transfer Inventory to Scrap' end as name
	,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from Scrap a, Scrap_Detail b 
where Status='Confirmed' and Poid ='{0}' and Seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, Poid, Seq1,Seq2, remark,a.IssueDate,a.Type
union all
	select issuedate, a.id
	,'P23. Transfer Inventory to Bulk' as name
	, 0 as inqty, sum(Qty) released,0 as adjust , '' remark ,'' location
from SubTransfer a, SubTransfer_Detail b 
where Status='Confirmed' and Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id and type = 'B' ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and fromroll='{0}' and fromdyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, frompoid, FromSeq1,FromSeq2,a.IssueDate,a.Type                                                                               
union all
	select issuedate, a.id
,'P23. Transfer Inventory to Bulk' name
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
where Status='Confirmed' and ToPoid='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id and type = 'B' ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and Toroll='{0}' and Todyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, ToPoid, ToSeq1,ToSeq2, remark ,a.IssueDate     
union all
	select issuedate, a.id
,'P18. TransferIn' name
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
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(@"group by a.id, poid, seq1,Seq2, remark,a.IssueDate                                                                                 
union all
	select issuedate, a.id
,'P19. TransferOut' name, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from TransferOut a, TransferOut_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id ", dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()));
            if (_byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
            }

            selectCommand1.Append(@"group by a.id, poid, Seq1,Seq2, remark,a.IssueDate) tmp
group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name
");
            #endregion

            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            if (selectResult1 == false)
            { ShowErr(selectCommand1.ToString(), selectResult1); }
            else
            {
                object inqty = selectDataTable1.Compute("sum(inqty)", null);
                object outqty = selectDataTable1.Compute("sum(outqty)", null);
                object adjust = selectDataTable1.Compute("sum(adjust)", null);
                this.numericBox1.Value = !MyUtility.Check.Empty(inqty) ? decimal.Parse(inqty.ToString()) : 0m;
                this.numericBox2.Value = !MyUtility.Check.Empty(outqty) ? decimal.Parse(outqty.ToString()) : 0m;
                this.numericBox3.Value = !MyUtility.Check.Empty(adjust) ? decimal.Parse(adjust.ToString()) : 0m;
            }

            bindingSource1.DataSource = selectDataTable1;
            MyUtility.Tool.SetGridFrozen(grid1);

            #region Farm In qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr2 = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr2) return;
                    switch (dr2["id"].ToString().Substring(3, 2))
                    {
                        case "PR":
                            //P07
                            break;
                        case "RF":
                            //	P08
                            break;
                        case "RL":
                            //	P09
                            break;
                        case "PI":
                            //	
                            break;
                        case "MB":
                            //	P31
                            break;
                        case "RB":
                            //	P32
                            break;
                        case "IP":
                            //	P12
                            break;
                        case "RT":
                            //	P37
                            break;
                        case "II":
                            //	P13
                            break;
                        case "RR":
                            //	P17
                            break;
                        case "TI":
                            //	P18
                            break;
                        case "TO":
                            //	P19
                            break;
                        case "IL":
                            //	P15
                            break;
                        case "IC":
                            //	P10
                            break;
                        case "IS":
                            //	P11
                            break;
                        case "IF":
                            //	P16
                            break;
                        case "BA":
                            //	P50
                            break;
                        case "BB":
                            //	P51
                            break;


                    }
                    //var frm = new Sci.Production.Subcon.P01_FarmInList(dr);
                    //frm.ShowDialog(this);
                }
            };
            #endregion

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("id", header: "Transaction#", width: Widths.AnsiChars(15), settings: ts2)
                .Text("name", header: "Name", width: Widths.AnsiChars(25))
                 .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("OutQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjust", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("Location", header: "Location", width: Widths.AnsiChars(20))
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
