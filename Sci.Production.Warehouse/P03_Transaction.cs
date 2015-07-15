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

namespace Sci.Production.Warehouse
{
    public partial class P03_Transaction : Sci.Win.Subs.Base
    {
        DataRow dr;
        public P03_Transaction(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 =
                string.Format(@"select *,
sum(TMP.inqty - TMP.outqty+tmp.adjust) over ( order by tmp.IssueDate,tmp.iD,sum(TMP.inqty - TMP.outqty+tmp.adjust) desc) as [balance] from (
	select a.IssueDate, a.id,'Adjust' name,0 as inqty,0 as outqty, sum(QtyAfter - QtyBefore) adjust, remark ,'' location
from Adjust a, Adjust_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.id, poid, seq1,Seq2, remark,a.IssueDate
union all
	select a.IssueDate, a.id,'BorrowBack' name,0 as inqty, sum(qty) released,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where Status='Confirmed' and FromPoId ='{0}' and FromSeq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id 
group by a.id, FromPoId, FromSeq1,FromSeq2, remark,a.IssueDate
union all
	select issuedate, a.id,'BorrowBack' name, sum(qty) arrived,0 as ouqty,0 as adjust, remark ,'' location
from BorrowBack a, BorrowBack_Detail b 
where Status='Confirmed' and ToPoid ='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id group by a.id, ToPoid, ToSeq1,ToSeq2, remark,a.IssueDate
union all
	select issuedate, a.id,'Issue' name,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from Issue a, Issue_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.id, poid, seq1,Seq2, remark,a.IssueDate                                                                               
union all
	select issuedate, a.id,'IssueLack' name, 0 as inqty,sum(b.Qty) outqty ,0 as adjust, remark ,'' location
from IssueLack a, IssueLack_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.id, poid, seq1,Seq2, remark  ,a.IssueDate                                                                      
union all
	select issuedate, a.id,'IssueReturn' name, 0 as inqty, sum(b.Qty) released,0 as adjust, remark,'' location
from IssueReturn a, IssueReturn_Detail b 
where status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.Id, poid, seq1,Seq2, remark,a.IssueDate                                                                                 
union all
	select a.WhseArrival, a.id,'Receiving' name, sum(b.ActualQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
from Receiving a, Receiving_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.Id, poid, seq1,Seq2,a.WhseArrival                                                                                
union all
	select issuedate,'ReturnReceipt' name, a.id, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from ReturnReceipt a, ReturnReceipt_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.id, poid, seq1,Seq2, remark,a.IssueDate                                                                               
union all
	select issuedate, a.id,'Scrap' name, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from Scrap a, Scrap_Detail b 
where Status='Confirmed' and Poid ='{0}' and Seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.id, Poid, Seq1,Seq2, remark,a.IssueDate
union all
	select issuedate, a.id,'SubTransfer' name, 0 as inqty, sum(Qty) released,0 as adjust , '' remark ,'' location
from SubTransfer a, SubTransfer_Detail b 
where Status='Confirmed' and Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id and type = 'B' 
group by a.id, frompoid, FromSeq1,FromSeq2,a.IssueDate                                                                               
union all
	select issuedate, a.id,'SubTransfer' name, sum(Qty) arrived,0 as ouqty,0 as adjust, remark
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
where Status='Confirmed' and ToPoid='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id and type = 'B' 
group by a.id, ToPoid, ToSeq1,ToSeq2, remark ,a.IssueDate     
union all
	select issuedate, a.id,'TransferIn' name, sum(Qty) arrived,0 as ouqty,0 as adjust, remark
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
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.id, poid, seq1,Seq2, remark,a.IssueDate                                                                                 
union all
	select issuedate, a.id,'TransferOut' name, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
from TransferOut a, TransferOut_Detail b 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id group by a.id, poid, Seq1,Seq2, remark,a.IssueDate) tmp
group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name
order by IssueDate
"
                , dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString());

            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);

            bindingSource1.DataSource = selectDataTable1;
            MyUtility.Tool.SetGridFrozen(grid1);
            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(13))
                 .Text("id", header: "Transaction#", width: Widths.AnsiChars(13))
                .Text("name", header: "Name", width: Widths.AnsiChars(13))
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
