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
using System.Transactions;

namespace Sci.Production.Warehouse
{
    public partial class P07_ModifyRollDyelot : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable source;
        string docno = "";
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P07_ModifyRollDyelot(object data, string data2)
        {
            InitializeComponent();
            source = (DataTable)data;
            docno = data2;
            this.Text += " - "+docno;
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            

            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");

            listControlBindingSource1.DataSource = source;
            //MyUtility.Tool.SetGridFrozen(grid1);
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            MyUtility.Tool.SetGridFrozen(grid1);

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
            .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(7), iseditable: false).Get(out cbb_fabrictype)  //0
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13))  //1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6))  //2
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9))    //3
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5))    //4
            .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10)    //5
            .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)    //6
            .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true,width: Widths.AnsiChars(5))    //8
            .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)   //9
            .Text("Location", header: "Location")    //10
            .Text("remark", header: "Remark")    //11
            ;     //

            cbb_fabrictype.DataSource = new BindingSource(di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void grid1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            displayBox1.Text = source.Rows[e.RowIndex]["poid"].ToString();
            displayBox2.Text = source.Rows[e.RowIndex]["seq"].ToString();
            textBox1.Text = source.Rows[e.RowIndex]["roll"].ToString();
            textBox2.Text = source.Rows[e.RowIndex]["dyelot"].ToString();
            string selectCommand1;
            #region sql command
            selectCommand1 =
                string.Format(@"select *,
            sum(TMP.inqty - TMP.outqty+tmp.adjust) over ( order by tmp.IssueDate,tmp.iD
,sum(TMP.inqty - TMP.outqty+tmp.adjust) desc) as [balance] 
from (
            select a.IssueDate, a.ID
            ,Case type when 'A' then 'P35. Adjust Bulk Qty' 
                            when 'B' then 'P34. Adjust Stock Qty' end as Name
            ,0 as InQty,0 as OutQty, sum(QtyAfter - QtyBefore) Adjust, Remark ,'' Location
            from Adjust a, Adjust_Detail b 
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id
and roll='{3}' and dyelot='{4}'
group by a.id, poid, seq1,Seq2, remark,a.IssueDate,type
            union all
            	select a.IssueDate, a.id
            ,case type when 'A' then 'P31. Material Borrow From' 
                            when 'B' then 'P32. Material Give Back From' end as name
            ,0 as inqty, sum(qty) released,0 as adjust, remark ,'' location
            from BorrowBack a, BorrowBack_Detail b 
            where Status='Confirmed' and FromPoId ='{0}' and FromSeq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id 
and fromroll='{3}' and fromdyelot='{4}'
group by a.id, FromPoId, FromSeq1,FromSeq2, remark,a.IssueDate,a.type
            union all
            	select issuedate, a.id
            ,case type when 'A' then 'P31. Material Borrow To' 
                            when 'B' then 'P32. Material Give Back To' end as name
, sum(qty) arrived,0 as ouqty,0 as adjust, remark ,'' location
            from BorrowBack a, BorrowBack_Detail b 
            where Status='Confirmed' and ToPoid ='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id 
and toroll='{3}' and todyelot='{4}'
group by a.id, ToPoid, ToSeq1,ToSeq2, remark,a.IssueDate,a.type
            union all
            	select issuedate, a.id
            	,case type when 'A' then 'P10. Issue Fabric to Cutting Section' 
                                when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
                                when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
                                when 'D' then 'P13. Issue Material by Item' end name
            	,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
            from Issue a, Issue_Detail b 
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
group by a.id, poid, seq1,Seq2, remark,a.IssueDate,a.type                                                                          
            union all
            select issuedate, a.id
            ,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
                                      when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
            , 0 as inqty,sum(b.Qty) outqty ,0 as adjust, remark ,'' location
            from IssueLack a, IssueLack_Detail b 
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id
and roll='{3}' and dyelot='{4}'
            group by a.id, poid, seq1,Seq2, remark  ,a.IssueDate,a.FabricType                                                               
            union all
            select issuedate, a.id
            ,'P17. R/Mtl Return' name
            , 0 as inqty, sum(b.Qty) released,0 as adjust, remark,'' location
            from IssueReturn a, IssueReturn_Detail b 
            where status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
and roll='{3}' and dyelot='{4}'
group by a.Id, poid, seq1,Seq2, remark,a.IssueDate                                                                                 
            union all
            select a.eta, a.id
            ,'P07. Material Receiving' as name
            , sum(b.StockQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
            from Receiving a, Receiving_Detail b 
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id and type='A' and a.id!='{5}'
and roll='{3}' and dyelot='{4}'
group by a.Id, poid, seq1,Seq2,a.eta,a.Type
            union all
            select a.WhseArrival, a.id
                    ,'P08. Warehouse Shopfloor Receiving' as name
            	    , sum(b.StockQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
            from Receiving a, Receiving_Detail b 
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id and type='B'
and roll='{3}' and dyelot='{4}'
group by a.Id, poid, seq1,Seq2,a.WhseArrival,a.Type                                                                              
            union all
            select issuedate, a.id
            ,'P37. Return Receiving Material' name
            , 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
            from ReturnReceipt a, ReturnReceipt_Detail b 
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
and roll='{3}' and dyelot='{4}'
group by a.id, poid, seq1,Seq2, remark,a.IssueDate                                                                               
            union all
            select issuedate, a.id
            ,case a.type when 'A' then 'P22. Transfer Bulk to Inventory'
                              when 'B' then 'P23. Transfer Inventory to Bulk'
                              when 'C' then 'P36. Transfer Scrap to Inventory' 
                                when 'D' then 'P25. Transfer Bulk to Scrap' 
                                when 'E' then 'P24. Transfer Inventory to Scrap' 
             end as name
            , 0 as inqty, sum(Qty) released,0 as adjust , '' remark ,'' location
            from SubTransfer a, SubTransfer_Detail b 
            where Status='Confirmed' and Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id
and fromroll='{3}' and fromdyelot='{4}'
            group by a.id, frompoid, FromSeq1,FromSeq2,a.IssueDate,a.Type                                                                               
            union all
            select issuedate, a.id
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
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}' and seq2 = '{2}'  and a.id = b.id
and roll='{3}' and dyelot='{4}'
group by a.id, poid, seq1,Seq2, remark,a.IssueDate                                                                                 
            union all
            select issuedate, a.id
                ,'P19. Transfer Out' name
                , 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
            from TransferOut a, TransferOut_Detail b 
            where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
and roll='{3}' and dyelot='{4}'
group by a.id, poid, Seq1,Seq2, remark,a.IssueDate) tmp
            group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name
            order by IssueDate,name,id
            "
                , source.Rows[e.RowIndex]["poid"].ToString()
                , source.Rows[e.RowIndex]["seq1"].ToString()
                , source.Rows[e.RowIndex]["seq2"].ToString()
                , source.Rows[e.RowIndex]["roll"].ToString()
                , source.Rows[e.RowIndex]["dyelot"].ToString()
                ,docno);
            #endregion

            DataTable dt;
            DualResult result;
            MyUtility.Msg.WaitWindows("Data Loading...");
            if (!(result = DBProxy.Current.Select(null, selectCommand1,out dt)))
            {
                ShowErr(selectCommand1, result);
            }
            else
            {
                grid2.DataSource = dt;
                grid2.AutoGenerateColumns = true;
                grid2.AutoResizeColumns();
            }
            button3.Enabled = !MyUtility.Check.Empty(dt) && dt.Rows.Count == 0 && source.Rows[e.RowIndex]["fabrictype"].ToString()=="F";
            MyUtility.Msg.WaitClear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sqlcmd,sqlupd1, sqlupd2, newRoll, newDyelot;
            DualResult result1, result2;
            newRoll = textBox1.Text.TrimEnd();
            newDyelot = textBox2.Text.TrimEnd();

            if (MyUtility.Check.Empty(textBox1.Text) || MyUtility.Check.Empty(textBox2.Text))
            {
                MyUtility.Msg.WarningBox("Roll# & Dyelot# can't be empty!!");
                return;
            }
            DataRow dr = grid1.GetDataRow(grid1.GetSelectedRowIndex());
            sqlcmd = string.Format(@"select 1 from dbo.Receiving_Detail 
                where id='{0}' and poid='{1}' and seq1='{2}' and seq2='{3}' and roll='{4}' and dyelot='{5}'"
                , docno, dr["poid"], dr["seq1"], dr["seq2"], newRoll, newDyelot);
            if (MyUtility.Check.Seek(sqlcmd,null))
            {
                MyUtility.Msg.WarningBox("Roll# & Dyelot# already existed!!");
                return;
            }
            
            sqlupd1 = string.Format(@"update dbo.receiving_detail set roll = '{6}' ,dyelot = '{7}' 
where id='{0}' and poid ='{1}' and seq1='{2}' and seq2='{3}' and roll='{4}' and dyelot='{5}'"
                , docno, dr["poid"], dr["seq1"], dr["seq2"], dr["roll"], dr["dyelot"],newRoll,newDyelot);
            sqlupd2 = string.Format(@"update dbo.ftyinventory set roll='{6}', dyelot = '{7}'
where poid ='{0}' and seq1='{1}' and seq2='{2}' and roll='{3}' and dyelot='{4}' and stocktype = '{5}'"
                , dr["poid"], dr["seq1"], dr["seq2"], dr["roll"], dr["dyelot"],dr["stocktype"],newRoll,newDyelot);

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result1 = DBProxy.Current.Execute(null, sqlupd1)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd1, result1);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Commit successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
        }
    }
}
