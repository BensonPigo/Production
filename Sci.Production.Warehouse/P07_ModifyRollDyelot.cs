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
        // DataRow dr;
        DataTable source;
        DataTable dtGridDyelot;
        string docno = "";
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewTextBoxColumn col_roll;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dyelot;

        public P07_ModifyRollDyelot(object data, string data2)
        {
            InitializeComponent();
            source = (DataTable)data;
            docno = data2;
            this.Text += " - " + docno;
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
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            //設定Grid1的顯示欄位
            this.gridModifyRoll.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridModifyRoll)
            .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(7), iseditable: false).Get(out cbb_fabrictype)  //0
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: false).Get(out col_roll)    //3
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: false).Get(out col_dyelot)   //4
            .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(11), iseditingreadonly: true, decimal_places: 2, integer_places: 10)    //5
            .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)    //6
            .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))    //8
            .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)   //9
            .Text("Location", header: "Location", iseditingreadonly: true)    //10
            .Text("remark", header: "Remark", iseditingreadonly: true)    //11
            ;     //

            cbb_fabrictype.DataSource = new BindingSource(di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            
            Helper.Controls.Grid.Generator(this.gridDyelot)
            .Date("IssueDate", header: "IssueDate", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("ID", header: "ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Name", header: "Name", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("InQty", header: "InQty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("OutQty", header: "OutQty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("remark", header: "Remark", iseditingreadonly: true)
            .Text("Location", header: "Location", iseditingreadonly: true)
            .Numeric("balance", header: "balance", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            ;     //

            this.LoadDate();
            this.setCloumn();
            this.changeeditable();
        }

        private void changeeditable()
        {
            #region roll
            col_roll.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0
                || !MyUtility.Convert.GetString(dr["fabrictype"]).EqualString("F"))
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
            };
            col_roll.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0
                || !MyUtility.Convert.GetString(dr["fabrictype"]).EqualString("F"))
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            #endregion
            #region dyelot
            col_dyelot.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0
                || !MyUtility.Convert.GetString(dr["fabrictype"]).EqualString("F"))
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
            };
            col_dyelot.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0
                || !MyUtility.Convert.GetString(dr["fabrictype"]).EqualString("F"))
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void LoadDate()
        {
            this.GetgridDyelotData();
            this.btnCommit.Enabled = false;
        }

        private void setCloumn()
        {
            foreach (DataGridViewRow item in this.gridModifyRoll.Rows)
            {
                bool existsDetail = dtGridDyelot.AsEnumerable()
                 .Where(s => s["poid"].Equals(item.Cells["poid"].Value) &&
                             s["seq"].Equals(item.Cells["seq"].Value) &&
                             s["roll"].Equals(item.Cells["roll"].Value) &&
                             s["dyelot"].Equals(item.Cells["dyelot"].Value)).Any();
                if (!(existsDetail || !item.Cells["fabrictype"].Value.Equals("F")))
                {
                    this.btnCommit.Enabled = true;
                }
            }
        }

        private void GetgridDyelotData()
        {
            #region sql command
            string selectCommand1 =
                string.Format(@"
SELECT DISTINCT POID,Seq1,Seq2,Roll,Dyelot into #tmp FROM Receiving_Detail WHERE id = '{0}'

select IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name,POID,Seq1,Seq2, Roll,Dyelot,[Seq] = Seq1 + ' ' + Seq2,
            sum(TMP.inqty - TMP.outqty+tmp.adjust) over ( order by tmp.IssueDate,tmp.iD
,sum(TMP.inqty - TMP.outqty+tmp.adjust) desc) as [balance] 
from (
            select  a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot,a.IssueDate
            ,Case type when 'A' then 'P35. Adjust Bulk Qty' 
                            when 'B' then 'P34. Adjust Stock Qty' end as Name
            ,0 as InQty,0 as OutQty, sum(QtyAfter - QtyBefore) Adjust, a.Remark ,'' Location
            from Adjust a WITH (NOLOCK) , Adjust_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.Seq1 = t.Seq1 and b.Seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
            where a.id = b.id
group by a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot, a.remark,a.IssueDate,type
            union all
            	select a.id,[poid] = b.FromPoId,[Seq1] = b.FromSeq1,[Seq2] = b.FromSeq2,[Roll] = b.FromRoll,[Dyelot] = b.FromDyelot, a.IssueDate
            ,case type when 'A' then 'P31. Material Borrow From' 
                            when 'B' then 'P32. Material Give Back From' end as name
            ,0 as inqty, sum(qty) released,0 as adjust, a.remark ,'' location
            from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.FromPoId = t.PoId and b.FromSeq1 = t.Seq1 and b.FromSeq2 = t.Seq2 and b.FromRoll = t.Roll and b.FromDyelot = t.Dyelot
            where Status='Confirmed'  and a.id = b.id 
group by a.id, b.FromPoId, b.FromSeq1,b.FromSeq2,b.FromRoll,b.FromDyelot, a.remark,a.IssueDate,a.type
            union all
            	select a.id,[poid] = b.ToPoid,[Seq1] = b.ToSeq1,[Seq2] = b.ToSeq2,[Roll] = b.ToRoll,[Roll] = b.ToDyelot, a.IssueDate
            ,case type when 'A' then 'P31. Material Borrow To' 
                            when 'B' then 'P32. Material Give Back To' end as name
, sum(qty) arrived,0 as ouqty,0 as adjust, remark ,'' location
            from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.ToPoid = t.PoId and b.ToSeq1 = t.Seq1 and b.ToSeq2 = t.Seq2 and b.toroll = t.Roll and b.ToDyelot = t.Dyelot 
            where Status='Confirmed' and a.id = b.id 
group by a.id, b.ToPoid, b.ToSeq1,b.ToSeq2,b.ToRoll,b.ToDyelot, a.remark,a.IssueDate,a.type
            union all
            	select a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot,a.IssueDate
            	,case type when 'A' then 'P10. Issue Fabric to Cutting Section' 
                                when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
                                when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
                                when 'D' then 'P13. Issue Material by Item' end name
            	,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
            from Issue a WITH (NOLOCK) , Issue_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
            where Status='Confirmed' and a.id = b.id
group by a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot, a.remark,a.IssueDate,a.type                                                                          
            union all
            select a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate
            ,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
                                      when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
            , 0 as inqty,sum(b.Qty) outqty ,0 as adjust, remark ,'' location
            from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
            where Status in ('Confirmed','Closed') and a.id = b.id
            group by a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, remark  ,a.IssueDate,a.FabricType                                                               
            union all
            select a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate
            ,'P17. R/Mtl Return' name
            , 0 as inqty, sum(b.Qty) released,0 as adjust, remark,'' location
            from IssueReturn a WITH (NOLOCK) , IssueReturn_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
            where status='Confirmed'  and a.id = b.id 
group by a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, remark,a.IssueDate                                                                                 
            union all
            select a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,[IssueDate] = a.eta
            ,'P07. Material Receiving' as name
            , sum(b.StockQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
            from Receiving a WITH (NOLOCK) , Receiving_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
            where Status='Confirmed' and a.id = b.id and type='A' and a.id!='{0}'
group by a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,a.eta,a.Type
            union all
            select a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,[IssueDate] = a.WhseArrival
                    ,'P08. Warehouse Shopfloor Receiving' as name
            	    , sum(b.StockQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
            from Receiving a WITH (NOLOCK) , Receiving_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
            where Status='Confirmed'  and a.id = b.id and type='B'
group by a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,a.WhseArrival,a.Type                                                                              
            union all
            select a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate   
            ,'P37. Return Receiving Material' name
            , 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
            from ReturnReceipt a WITH (NOLOCK) , ReturnReceipt_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
            where Status='Confirmed' and a.id = b.id 
group by a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, remark,a.IssueDate                                                                               
            union all
            select a.id, b.frompoid, b.FromSeq1,b.FromSeq2, b.FromRoll,b.FromDyelot,a.IssueDate
            ,case a.type when 'A' then 'P22. Transfer Bulk to Inventory'
                              when 'B' then 'P23. Transfer Inventory to Bulk'
                              when 'C' then 'P36. Transfer Scrap to Inventory' 
                                when 'D' then 'P25. Transfer Bulk to Scrap' 
                                when 'E' then 'P24. Transfer Inventory to Scrap' 
             end as name
            , 0 as inqty, sum(Qty) released,0 as adjust , '' remark ,'' location
            from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.Frompoid = t.PoId and b.Fromseq1 = t.Seq1 and b.FromSeq2 = t.Seq2 and b.fromroll = t.Roll and b.FromDyelot = t.Dyelot
            where Status='Confirmed' and a.id = b.id
            group by a.id, b.frompoid, b.FromSeq1,b.FromSeq2, b.FromRoll,b.FromDyelot,a.IssueDate,a.Type                                                                               
            union all
            select a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate  
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
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.roll = t.Roll and b.Dyelot = t.Dyelot
            where Status='Confirmed' and a.id = b.id
group by a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.remark,a.IssueDate                                                                                 
            union all
            select a.id, b.poid, b.Seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate
                ,'P19. Transfer Out' name
                , 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
            from TransferOut a WITH (NOLOCK) , TransferOut_Detail b WITH (NOLOCK) 
			inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.roll = t.Roll and b.Dyelot = t.Dyelot
            where Status='Confirmed' and a.id = b.id 
group by a.id, b.poid, b.Seq1,b.Seq2, b.Roll,b.Dyelot, remark,a.IssueDate) tmp
            group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name,POID,Seq1,Seq2, Roll,Dyelot
            order by IssueDate,name,id
            "
                , docno);
            #endregion

            DualResult result;
            this.ShowWaitMessage("Data Loading...");
            if (!(result = DBProxy.Current.Select(null, selectCommand1, out dtGridDyelot)))
            {
                ShowErr(selectCommand1, result);
            }

            this.HideWaitMessage();
        }

        private void grid1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGridDyelot == null)
            {
                this.GetgridDyelotData();
            }

            if (dtGridDyelot.Rows.Count == 0)
            {
                return;
            }

            if (e.RowIndex == -1) return;
            DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

            var dt = dtGridDyelot.AsEnumerable()
                .Where(s => s["poid"].Equals(dr["poid"]) &&
                            s["seq1"].Equals(dr["seq1"]) &&
                            s["seq2"].Equals(dr["seq2"]) &&
                            s["roll"].Equals(dr["roll"]) &&
                            s["dyelot"].Equals(dr["dyelot"]));

            if (dt.Count() == 0)
            {
                gridDyelot.DataSource = null;
            }
            else
            {
                gridDyelot.DataSource = dt.CopyToDataTable();
            }
            
            
            gridDyelot.AutoResizeColumns();

        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            var modifyDrList = source.AsEnumerable().Where(s => s.RowState == DataRowState.Modified);
            if (modifyDrList.Count() == 0)
            {
                MyUtility.Msg.InfoBox("No data has been changed!");
                return;
            }

            if (modifyDrList.Where(s => MyUtility.Check.Empty(s["Roll"]) || MyUtility.Check.Empty(s["Dyelot"])).Any())
            {
                MyUtility.Msg.WarningBox("Roll# & Dyelot# can't be empty!!");
                return;
            }

            if (modifyDrList.GroupBy(s => new { Roll = s["Roll"].ToString(), Dyelot = s["Dyelot"].ToString() })
                .Select(g => new { g.Key.Roll, g.Key.Dyelot, ct = g.Count() }).Any(r => r.ct > 1))
            {
                MyUtility.Msg.WarningBox("Roll# & Dyelot# can not  duplicate!!");
                return;
            }            

            string sqlcmd;
            string sqlupd1 = string.Empty;
            string sqlupd2 = string.Empty;
            string newRoll;
            string newDyelot;

            foreach (var drModify in modifyDrList)
            {
                sqlcmd = string.Format(@"select 1 from dbo.Receiving_Detail WITH (NOLOCK) 
                where id='{0}' and poid='{1}' and seq1='{2}' and seq2='{3}' and roll='{4}' and dyelot='{5}'"
                    , docno, drModify["poid"], drModify["seq1"], drModify["seq2"], drModify["roll"], drModify["dyelot"]);
                if (MyUtility.Check.Seek(sqlcmd, null))
                {
                    MyUtility.Msg.WarningBox("Roll# & Dyelot# already existed!!");
                    return;
                }

                sqlupd1 += string.Format($@"update dbo.receiving_detail set roll = '{drModify["roll"]}' ,dyelot = '{drModify["dyelot"]}' where ukey = '{drModify["ukey"]}'; ");
                sqlupd2 += string.Format(@"update dbo.ftyinventory set roll='{6}', dyelot = '{7}'
where poid ='{0}' and seq1='{1}' and seq2='{2}' and roll='{3}' and dyelot='{4}' and stocktype = '{5}';"
                    , drModify["poid"], drModify["seq1"], drModify["seq2"], drModify["roll", DataRowVersion.Original], drModify["dyelot", DataRowVersion.Original], drModify["stocktype"], drModify["roll"], drModify["dyelot"]);
            }

            
            DualResult result1, result2;

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
                    _transactionscope.Dispose();
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

            DataTable dt;
            DualResult result;
            string selectCommand1 = string.Format(@"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,(select p1.FabricType from PO_Supp_Detail p1 WITH (NOLOCK) where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as fabrictype
,a.shipqty
,a.Weight
,a.ActualWeight
,a.Roll
,a.Dyelot
,a.ActualQty
,a.PoUnit
,a.StockQty
,a.StockUnit
,a.StockType
,a.Location
,a.remark
,a.ukey
from dbo.Receiving_Detail a WITH (NOLOCK) 
Where a.id = '{0}' ", docno);

            if (!(result = DBProxy.Current.Select(null, selectCommand1, out dt)))
            {
                ShowErr(selectCommand1, result);
            }
            else
            {
                source = dt;
                gridModifyRoll.DataSource = dt;
                this.LoadDate();
            }
        }
    }
}
