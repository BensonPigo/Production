using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_Seq : Sci.Win.Subs.Base
    {
        private DataRow dr;

        /// <inheritdoc/>
        public P03_Seq(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += " (" + this.dr["id"].ToString() + "-" + this.dr["seq1"].ToString() + "-" + this.dr["seq2"].ToString() + ")";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlcmd = string.Format(
                @"
select
	Date = case type
		when 'A' then a.eta
		when 'B' then a.WhseArrival end,
	a.AddDate,
	ID = a.ID,
	Name = case type
		when 'A' then 'P07. Material Receiving' 
		when 'B' then 'P08. Warehouse Shopfloor Receiving' end,
	BulkArrivedQty = case type
		when 'A' then QB.StockQty
		when 'B' then Q.StockQty end,
	BulkOutQty = Null,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = case type
		when 'A' then QI.StockQty
		when 'B' then Null end,
	InventoryOutQty = Null,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from Receiving a WITH (NOLOCK)
outer apply (
    select StockQty = sum (b.StockQty)
    from Receiving_Detail b WITH (NOLOCK)
    where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}'
) Q
outer apply (
    select StockQty = sum (b.StockQty)
    from Receiving_Detail b WITH (NOLOCK)
    where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' and b.StockType = 'B'
) QB
outer apply (
    select StockQty = sum (b.StockQty)
    from Receiving_Detail b WITH (NOLOCK)
    where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' and b.StockType = 'I'
) QI
where Status='Confirmed'
and exists (select 1 from Receiving_Detail b WITH (NOLOCK) where a.Id = b.Id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}')

union

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
	Name = 'P37. Return Receiving Material',
	BulkArrivedQty = Null,
	BulkOutQty = Null,
	BulkAdjustQty = Null,
	BulkReturnQty = QB.Qty,
	InventoryArrivedQty = Null,
	InventoryOutQty = Null,
	InventoryAdjustQty = Null,
	InventoryReturnQty = QI.Qty,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from ReturnReceipt a WITH (NOLOCK)
outer apply (
    select Qty = sum (b.Qty)
    from ReturnReceipt_Detail b WITH (NOLOCK)
    where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' and b.StockType = 'B'
) QB
outer apply (
    select Qty = sum (b.Qty)
    from ReturnReceipt_Detail b WITH (NOLOCK)
    where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' and b.StockType = 'I'
) QI
where Status='Confirmed' 
and exists (select 1 from ReturnReceipt_Detail b WITH (NOLOCK) where a.Id = b.Id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}')

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
	Name = case type 
            when 'A' then 'P10. Issue Fabric to Cutting Section' 
            when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
            when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
            when 'D' then 'P13. Issue Material by Item'
            when 'E' then 'P33. Issue Thread'
            when 'I' then 'P62. Issue Fabric for Cutting Tape' end,
	BulkArrivedQty = Null,
	BulkOutQty = Q.Qty,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = Null,
	InventoryOutQty = Null,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from Issue a WITH (NOLOCK)
outer apply (
    select Qty = sum (Qty)
    from issue_detail b WITH (NOLOCK)
    where a.id = b.id and poid='{0}' and seq1 = '{1}'and seq2 = '{2}' 
) Q
where Status='Confirmed' 
and exists (select 1 from Issue_Detail b WITH (NOLOCK)where a.Id = b.Id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}')

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
    Name = case FabricType 
		when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
		when 'F' then 'P16. Issue Fabric Lacking & Replacement' end,
	BulkArrivedQty = Null,
	BulkOutQty = Q.Qty,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = Null,
	InventoryOutQty = Null,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from IssueLack a WITH (NOLOCK)
outer apply (
    select Qty = sum (Qty)
    from IssueLack_Detail b WITH (NOLOCK)
    where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
) Q
where a.Status <> 'New' and a.Type='R'
and exists (select 1 from IssueLack_Detail b WITH (NOLOCK) where a.Id = b.Id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}')

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
    Name = 'P17. R/Mtl Return',
	BulkArrivedQty = Null,
	BulkOutQty = Q.Qty,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = Null,
	InventoryOutQty = Null,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from IssueReturn a WITH (NOLOCK)
outer apply (
    select Qty = sum (-b.Qty)
    from IssueReturn_Detail b WITH (NOLOCK)
    where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
) Q
where status='Confirmed' 
and exists (select 1 from IssueReturn_Detail b WITH (NOLOCK)where a.Id = b.Id and b.poid = '{0}' and b.seq1 = '{1}'and b.seq2 = '{2}') 

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
    name = Case type
		when 'A' then 'P35. Adjust Bulk Qty'
		when 'B' then 'P34. Adjust Stock Qty'
		when 'O' then 'P43. Adjust Scrap Qty'
		when 'R' then 'P45. Remove from Scrap Whse' end,
	BulkArrivedQty = Null,
	BulkOutQty = Null,
	BulkAdjustQty =  Case type when 'B' then Q.Qty else Null end,
	BulkReturnQty = Null,
	InventoryArrivedQty = Null,
	InventoryOutQty = Null,
	InventoryAdjustQty = Case type when 'A' then Q.Qty else Null end,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Case type when 'O' then Q.Qty when 'R'then Q.Qty  else Null end
from Adjust a WITH (NOLOCK)
outer apply (
	select Qty = sum (QtyAfter - QtyBefore)
	from Adjust_Detail b WITH (NOLOCK)
	where a.id = b.id and  b.poid='{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
) Q
where a.Status = 'Confirmed' and a.Type not in  ('R','O')
and exists (select 1 from Adjust_Detail b WITH (NOLOCK) where a.Id = b.Id and poid = '{0}' and seq1 = '{1}' and seq2 = '{2}')

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
    name = Case type
		when 'A' then 'P22. Transfer Bulk to Inventory (A2B)'
		when 'B' then 'P23. Transfer Inventory to Bulk (B2A)'
		when 'E' then 'P24. Transfer Inventory To Scrap (B2C)'
		when 'D' then 'P25. Transfer Bulk To Scrap (A2C)'
		when 'C' then 'P36. Transfer Scrap to Inventory (C2B)' end,
	BulkArrivedQty = Case type when 'B' then QT.Qty else Null end ,
	BulkOutQty = Case type
		when 'A' then QF.Qty
		when 'D' then QF.Qty
		else Null end,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty =  Case type
		when 'A' then QT.Qty
		when 'C' then QT.Qty
		else Null end,
	InventoryOutQty = Case type
		when 'B' then QF.Qty
		when 'E' then QF.Qty
		else Null end,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Case type
		when 'E' then QT.Qty
		when 'D' then QT.Qty
		else Null end,
	ScrapOutQty = Case type when 'C' then QF.Qty else Null end,
	ScrapAdjustQty = Null
from SubTransfer a WITH (NOLOCK)
outer apply (
	select Qty = sum (b.Qty)
	from SubTransfer_Detail b WITH (NOLOCK)
	where a.id = b.id and b.Frompoid = '{0}' and b.Fromseq1 = '{1}' and b.FromSeq2 = '{2}' 
) QF
outer apply (
	select Qty = sum (b.Qty)
	from SubTransfer_Detail b WITH (NOLOCK)
	where a.id = b.id and b.ToPoid = '{0}' and b.ToSeq1 = '{1}' and b.ToSeq2 = '{2}' 
) QT
where a.Status='Confirmed'
and (exists (select 1 from SubTransfer_Detail b WITH (NOLOCK)where a.Id = b.Id and b.Frompoid = '{0}' and b.Fromseq1 = '{1}' and b.FromSeq2 = '{2}')
  or exists (select 1 from SubTransfer_Detail b WITH (NOLOCK)where a.Id = b.Id  and b.ToPoid = '{0}' and b.ToSeq1 = '{1}'and b.ToSeq2 = '{2}'))
  
union all
  
select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
	Name = 'P18. TransferIn',
	BulkArrivedQty = QB.Qty,
	BulkOutQty = Null,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = QI.Qty,
	InventoryOutQty = Null,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from TransferIn a WITH (NOLOCK) 
outer apply (
    select Qty = sum (b.Qty)
    from TransferIn_Detail b WITH (NOLOCK)
    where a.id = b.id and b.Poid = '{0}' and b.Seq1 = '{1}' and b.Seq2 = '{2}' and b.StockType = 'B'
) QB
outer apply (
    select Qty = sum (b.Qty)
    from TransferIn_Detail b WITH (NOLOCK)
    where a.id = b.id and b.Poid = '{0}' and b.Seq1 = '{1}' and b.Seq2 = '{2}' and b.StockType = 'I'
) QI
where Status='Confirmed' 
and exists (select 1 from TransferIn_Detail b WITH (NOLOCK) where a.Id = b.Id and b.Poid = '{0}' and b.Seq1 = '{1}'and b.Seq2 = '{2}')

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
	Name = 'P19. TransferOut',
	BulkArrivedQty =Null,
	BulkOutQty = QB.Qty,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = Null,
	InventoryOutQty = QI.Qty,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from TransferOut a WITH (NOLOCK)
outer apply (
    select qty = sum (b.Qty) 
    from TransferOut_Detail b WITH (NOLOCK)
    where a.id = b.id and b.Poid = '{0}' and b.Seq1 = '{1}' and b.Seq2 = '{2}' and b.StockType = 'B'
) QB
outer apply (
    select qty = sum (b.Qty) 
    from TransferOut_Detail b WITH (NOLOCK)
    where a.id = b.id and b.Poid = '{0}' and b.Seq1 = '{1}' and b.Seq2 = '{2}' and b.StockType = 'I'
) QI
where Status='Confirmed' 
and exists (select 1 from TransferOut_Detail b WITH (NOLOCK)where a.Id = b.Id and b.Poid = '{0}' and b.Seq1 = '{1}'and b.Seq2 = '{2}')

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
    Name = Case type
		when 'A' then 'P31. Material Borrow out'
		when 'B' then 'P32. Return Borrowing out' end,
	BulkArrivedQty =Null,
	BulkOutQty = QFB.Qty,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = Null,
	InventoryOutQty = QFI.Qty,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from BorrowBack a WITH (NOLOCK)
outer apply (
	select Qty = sum (Qty)
	from BorrowBack_Detail b WITH (NOLOCK)
	where a.id = b.id and  b.FromPoId='{0}' and b.FromSeq1 = '{1}' and b.FromSeq2 = '{2}' and b.FromStockType = 'B'
) QFB
outer apply (
	select Qty = sum (Qty)
	from BorrowBack_Detail b WITH (NOLOCK)
	where a.id = b.id and  b.FromPoId='{0}' and b.FromSeq1 = '{1}' and b.FromSeq2 = '{2}' and b.FromStockType = 'I'
) QFI
where Status='Confirmed' and Type in('A','B')
and exists (select 1 from BorrowBack_Detail b WITH (NOLOCK) where a.Id = b.Id and b.Frompoid = '{0}' and b.Fromseq1 = '{1}' and b.Fromseq2 = '{2}')

union all

select
	Date = a.issuedate,
	a.AddDate,
	ID = a.ID,
    Name = Case type
		when 'A' then 'P31. Material Borrow in'
		when 'B' then 'P32. Return Borrowing in' end,
	BulkArrivedQty = QTB.Qty,
	BulkOutQty = Null,
	BulkAdjustQty = Null,
	BulkReturnQty = Null,
	InventoryArrivedQty = QTI.Qty,
	InventoryOutQty = Null,
	InventoryAdjustQty = Null,
	InventoryReturnQty = Null,
	ScrapArrivedQty = Null,
	ScrapOutQty = Null,
	ScrapAdjustQty = Null
from BorrowBack a WITH (NOLOCK)
outer apply (
	select Qty = sum (Qty)
	from BorrowBack_Detail b WITH (NOLOCK)
	where a.id = b.id and  b.ToPoId='{0}' and b.ToSeq1 = '{1}' and b.ToSeq2 = '{2}' and b.ToStockType = 'B'
) QTB
outer apply (
	select Qty = sum (Qty)
	from BorrowBack_Detail b WITH (NOLOCK)
	where a.id = b.id and  b.ToPoId = '{0}' and b.ToSeq1 = '{1}' and b.ToSeq2 = '{2}' and b.ToStockType = 'I'
) QTI
where Status='Confirmed' and Type in('A','B')
and exists (select 1 from BorrowBack_Detail b WITH (NOLOCK) where a.Id = b.Id and b.ToPoId = '{0}' and b.ToSeq1 = '{1}' and b.ToStockType = '{2}')

order by Date,AddDate
",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()

);
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            #region 開窗
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                var frm = new Win.Tems.Input6(null);
                var dr2 = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr2 == null)
                {
                    return;
                }

                switch (dr2["name"].ToString().Substring(0, 3))
                {
                    // Receiving
                    case "P07":
                        frm = new P07(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P08":
                        frm = new P08(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // ReturnReceipt
                    case "P37":
                        frm = new P37(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // Issue
                    case "P10":
                        frm = new P10(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P11":
                        frm = new P11(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P12":
                        frm = new P12(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P13":
                        frm = new P13(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P33":
                        frm = new P33(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P62":
                        frm = new P62(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // IssueLack
                    case "P15":
                        frm = new P15(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P16":
                        frm = new P16(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // IssueReturn
                    case "P17":
                        frm = new P17(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // Adjust
                    case "P35":
                        frm = new P35(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P34":
                        frm = new P34(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P43":
                        frm = new P43(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P45":
                        frm = new P45(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // SubTransfer
                    case "P22":
                        frm = new P22(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P23":
                        frm = new P23(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P24":
                        frm = new P24(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P25":
                        frm = new P25(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P36":
                        frm = new P36(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // TransferIn
                    case "P18":
                        // P18
                        frm = new P18(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // TransferOut
                    case "P19":
                        // P19
                        frm = new P19(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    // BorrowBack
                    case "P31":
                        frm = new P31(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P32":
                        frm = new P32(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                }
            };
            #endregion

            this.grid1.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Date("Date", header: "Date", width: Widths.AnsiChars(12))
                .Text("ID", header: "Transaction#", width: Widths.AnsiChars(15), settings: ts2)
                .Text("Name", header: "Name", width: Widths.AnsiChars(25))
                .Numeric("BulkArrivedQty", header: "Bulk\r\nArrivedQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("BulkOutQty", header: "Bulk\r\nOutQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("BulkAdjustQty", header: "Bulk\r\nAdjustQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("BulkReturnQty", header: "Bulk\r\nReturnQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("InventoryArrivedQty", header: "Inventory\r\nArrivedQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("InventoryOutQty", header: "Inventory\r\nOutQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("InventoryAdjustQty", header: "Inventory\r\nAdjustQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("InventoryReturnQty", header: "Inventory\r\nReturnQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("ScrapArrivedQty", header: "Scrap\r\nArrivedQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("ScrapOutQty", header: "Scrap\r\nOutQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
                .Numeric("ScrapAdjustQty", header: "Scrap\r\nAdjustQty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
            ;
        }
    }
}
