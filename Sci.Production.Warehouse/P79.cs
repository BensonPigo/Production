using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P79 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P79(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.dateRange1.Value1 = DateTime.Today;
            this.dateRange1.Value2 = DateTime.Today;
            this.SetupGrid();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.grid1.DataSource = null;
            if (!this.dateRange1.Value1.HasValue && string.IsNullOrWhiteSpace(this.txtBeginPOID.Text))
            {
                MyUtility.Msg.ErrorBox("Date and SP# can't all be empty");
                return;
            }

            #region 篩選條件
            var whereParams = new List<object>();
            var whereConditions = new List<string>();
            if (this.dateRange1.Value1.HasValue)
            {
                whereParams.Add("@BeginDate");
                whereParams.Add(this.dateRange1.Value1.Value.ToShortDateString());

                whereParams.Add("@EndDate");
                whereParams.Add(this.dateRange1.Value2.Value.ToShortDateString());
            }

            if (!string.IsNullOrWhiteSpace(this.txtBeginPOID.Text) && string.IsNullOrWhiteSpace(this.txtEndPOID.Text))
            {
                whereParams.Add("@BeginPOID");
                whereParams.Add(this.txtBeginPOID.Text.Trim());

                whereParams.Add("@EndPOID");
                whereParams.Add(this.txtBeginPOID.Text.Trim());
            }

            if (string.IsNullOrWhiteSpace(this.txtBeginPOID.Text) && !string.IsNullOrWhiteSpace(this.txtEndPOID.Text))
            {
                whereParams.Add("@BeginPOID");
                whereParams.Add(this.txtEndPOID.Text.Trim());

                whereParams.Add("@EndPOID");
                whereParams.Add(this.txtEndPOID.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(this.txtBeginPOID.Text) && !string.IsNullOrWhiteSpace(this.txtEndPOID.Text))
            {
                whereParams.Add("@BeginPOID");
                whereParams.Add(this.txtBeginPOID.Text.Trim());

                whereParams.Add("@EndPOID");
                whereParams.Add(this.txtEndPOID.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(this.txtMdivisionID.Text))
            {
                whereParams.Add("@MDivisionID");
                whereParams.Add(this.txtMdivisionID.Text.Trim());

                whereConditions.Add("o.MDivisionID = @MDivisionID");
            }

            if (!string.IsNullOrWhiteSpace(this.txtFactoryID.Text))
            {
                whereParams.Add("@FactoryID");
                whereParams.Add(this.txtFactoryID.Text.Trim());

                whereConditions.Add("o.FtyGroup = @FactoryID");
            }

            if (!string.IsNullOrWhiteSpace(this.txtBrandID.Text))
            {
                whereParams.Add("@BrandID");
                whereParams.Add(this.txtBrandID.Text.Trim());

                whereConditions.Add("o.BrandID = @BrandID");
            }

            var whereCondition = whereConditions.Any() ? Environment.NewLine + "  and " + whereConditions.JoinToString(Environment.NewLine + "  and ") : string.Empty;
            #endregion

            this.ShowWaitMessage("Data Loading....");
            var sqlcmd = this.GetSqlcmd(whereCondition);
            using (var result = DBProxy.Current.SelectEx(sqlcmd, whereParams.ToArray()))
            {
                if (!result)
                {
                    this.HideWaitMessage();
                    this.ShowErr(sqlcmd, result.InnerResult);
                    return;
                }

                var dt = result.ExtendedData;
                if (dt.Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                    return;
                }

                this.grid1.DataSource = dt.Copy();
            }

            this.HideWaitMessage();
        }

        private string GetSqlcmd(string whereCondition)
        {
            var sqlcmd = string.Empty;

            #region Step1.依篩選條件取出SP#清單

            // 有輸入SP#條件則直接至Orders篩選SP#清單
            if (!string.IsNullOrWhiteSpace(this.txtBeginPOID.Text) || !string.IsNullOrWhiteSpace(this.txtEndPOID.Text))
            {
                sqlcmd = @"
select POID = o.ID
     , o.MDivisionID
     , o.FactoryID
     , o.BrandID
	 , o.StyleID
into #OrderDatas
from View_WH_Orders o with(nolock)
where o.ID between @BeginPOID and @EndPOID
  and o.ID = o.POID";
            }

            // 沒有輸入SP#條件則至所有交易紀錄表篩選日期並篩出SP#清單
            else
            {
                sqlcmd = @"
select POID = o.ID
     , o.MDivisionID
     , o.FactoryID
     , o.BrandID
	 , o.StyleID
into #OrderDatas
from (
	select d.PoId
	from Receiving m with(nolock)
	inner join Receiving_Detail d with(nolock) on m.Id = d.Id
	where (m.Type = 'A' and m.ETA between @BeginDate and @EndDate)
	   or (m.Type <> 'A' and m.WhseArrival between @BeginDate and @EndDate)

	union

	select d.PoId
	from ReturnReceipt m with(nolock)
	inner join ReturnReceipt_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.PoId
	from Issue m with(nolock)
	inner join Issue_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.PoId
	from IssueLack m with(nolock)
	inner join IssueLack_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.PoId
	from IssueReturn m with(nolock)
	inner join IssueReturn_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.PoId
	from Adjust m with(nolock)
	inner join Adjust_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.FromPOID
	from SubTransfer m with(nolock)
	inner join SubTransfer_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.ToPOID
	from SubTransfer m with(nolock)
	inner join SubTransfer_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.PoId
	from TransferIn m with(nolock)
	inner join TransferIn_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.PoId
	from TransferOut m with(nolock)
	inner join TransferOut_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.FromPOID
	from BorrowBack m with(nolock)
	inner join BorrowBack_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate

	union

	select d.ToPOID
	from BorrowBack m with(nolock)
	inner join BorrowBack_Detail d with(nolock) on m.Id = d.Id
	where m.IssueDate between @BeginDate and @EndDate
) t
inner join View_WH_Orders o with(nolock) on t.PoId = o.ID
where 1 = 1";
            }

            sqlcmd = @"
IF Object_id('tempdb.dbo.#OrderDatas') IS NOT NULL
BEGIN
	DROP TABLE #OrderDatas
END
IF Object_id('tempdb.dbo.#TransactionDatas') IS NOT NULL
BEGIN
	DROP TABLE #TransactionDatas
END
" + sqlcmd + whereCondition;
            #endregion

            #region Step2.根據 SP# 清單找出所有 A 倉主料的交易紀錄
            sqlcmd += @"

select *
	 , BalanceQty = sum(t.ArrivedQty - t.ReleasedQty + t.AdjustQty - t.ReturnQty) over (partition by t.POID, t.Seq1, t.Seq2 order by t.Date, t.ArrivedQty desc, t.TransactionID)
into #TransactionDatas
from (
	select TransactionID = m.ID
		 , Date = iif(m.Type = 'A', m.ETA, m.WhseArrival)
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = case m.Type when 'A' then 'P07. Material Receiving' 
							  when 'B' then 'P08. Warehouse Shopfloor Receiving' 
				  end
		 , ArrivedQty = sum(d.StockQty)
		 , ReleasedQty = 0
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from Receiving m with(nolock)
	inner join Receiving_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status = 'Confirmed'
	  and d.StockType = 'B'
	group by m.ID, m.ETA, m.WhseArrival, d.PoId, d.Seq1, d.Seq2, m.Type, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = 'P37. Return Receiving Material'
		 , ArrivedQty = 0
		 , ReleasedQty = 0
		 , AdjustQty = 0
		 , ReturnQty = sum(d.Qty)
		 , m.Remark
	from ReturnReceipt m with(nolock)
	inner join ReturnReceipt_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status = 'Confirmed'
	  and d.StockType = 'B'
	group by m.ID, m.IssueDate, d.PoId, d.Seq1, d.Seq2, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = case m.Type 
                	when 'A' then 'P10. Issue Fabric to Cutting Section'
                	when 'D' then 'P13. Issue Material by Item'
                	when 'I' then 'P62. Issue Fabric for Cutting Tape'
                  end 
		 , ArrivedQty = 0
		 , ReleasedQty = sum(d.Qty)
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from Issue m with(nolock)
	inner join Issue_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status = 'Confirmed'
	  and m.Type in ('A','D','I')
	group by m.ID, m.IssueDate, d.PoId, d.Seq1, d.Seq2, m.Type, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = 'P16. Issue Fabric Lacking & Replacement'
		 , ArrivedQty = 0
		 , ReleasedQty = sum(d.Qty)
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from IssueLack m with(nolock)
	inner join IssueLack_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status in ('Confirmed','Locked')
	  and m.FabricType = 'F'
	  and m.Type = 'R'
	group by m.ID, m.IssueDate, d.PoId, d.Seq1, d.Seq2, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = 'P17. R/Mtl Return'
		 , ArrivedQty = 0
		 , ReleasedQty = sum(0.00 - d.Qty)
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from IssueReturn m with(nolock)
	inner join IssueReturn_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status = 'Confirmed'
	group by m.ID, m.IssueDate, d.PoId, d.Seq1, d.Seq2, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = 'P35. Adjust Bulk Qty'
		 , ArrivedQty = 0
		 , ReleasedQty = 0
		 , AdjustQty = sum(d.QtyAfter - d.QtyBefore)
		 , ReturnQty = 0
		 , m.Remark
	from Adjust m with(nolock)
	inner join Adjust_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status = 'Confirmed'
	  and m.Type = 'A'
	  and d.StockType = 'B'
	group by m.ID, m.IssueDate, d.PoId, d.Seq1, d.Seq2, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.FromPOID
		 , d.FromSeq1
		 , d.FromSeq2
		 , Name = case m.Type when 'A' then 'P22. Transfer Bulk to Inventory'
							  when 'D' then 'P25. Transfer Bulk to Scrap'
				  end
		 , ArrivedQty = 0
		 , ReleasedQty = sum(d.Qty)
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from SubTransfer m with(nolock)
	inner join SubTransfer_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.FromPOID = od.POID
	where m.Status = 'Confirmed'
	  and m.Type in ('A','D')
	  and d.FromStockType = 'B'
	group by m.ID, m.IssueDate, d.FromPOID, d.FromSeq1, d.FromSeq2, m.Type, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.ToPOID
		 , d.ToSeq1
		 , d.ToSeq2
		 , Name = 'P23. Transfer Inventory to Bulk'
		 , ArrivedQty = iif(m.Type = 'B', sum(d.Qty), 0)
		 , ReleasedQty = iif(m.Type in ('A','D'), sum(d.Qty), 0)
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from SubTransfer m with(nolock)
	inner join SubTransfer_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.ToPOID = od.POID
	where m.Status = 'Confirmed'
	  and m.Type = 'B'
	  and d.ToStockType = 'B'
	group by m.ID, m.IssueDate, d.ToPOID, d.ToSeq1, d.ToSeq2, m.Type, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = 'P18. Transfer In'
		 , ArrivedQty = sum(d.Qty)
		 , ReleasedQty = 0
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from TransferIn m with(nolock)
	inner join TransferIn_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status = 'Confirmed'
	  and d.StockType = 'B'
	group by m.ID, m.IssueDate, d.PoId, d.Seq1, d.Seq2, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.PoId
		 , d.Seq1
		 , d.Seq2
		 , Name = 'P19. Transfer Out'
		 , ArrivedQty = 0
		 , ReleasedQty = sum(d.Qty)
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from TransferOut m with(nolock)
	inner join TransferOut_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.PoId = od.POID
	where m.Status = 'Confirmed'
	  and d.StockType = 'B'
	group by m.ID, m.IssueDate, d.PoId, d.Seq1, d.Seq2, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.FromPOID
		 , d.FromSeq1
		 , d.FromSeq2
		 , Name = case m.Type when 'A' then 'P31. Material Borrow From' 
							  when 'B' then 'P32. Material Give Back From' 
				  end
		 , ArrivedQty = 0
		 , ReleasedQty = sum(d.Qty)
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from BorrowBack m with(nolock)
	inner join BorrowBack_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.FromPOID = od.POID
	where m.Status = 'Confirmed' 
	  and d.FromStockType = 'B'
	group by m.ID, m.IssueDate, d.FromPOID, d.FromSeq1, d.FromSeq2, m.Type, m.Remark

	union

	select TransactionID = m.ID
		 , Date = m.IssueDate
		 , d.ToPOID
		 , d.ToSeq1
		 , d.ToSeq2
		 , Name = case m.Type when 'A' then 'P31. Material Borrow From' 
							  when 'B' then 'P32. Material Give Back From' 
				  end
		 , ArrivedQty = sum(d.Qty)
		 , ReleasedQty = 0
		 , AdjustQty = 0
		 , ReturnQty = 0
		 , m.Remark
	from BorrowBack m with(nolock)
	inner join BorrowBack_Detail d with(nolock) on m.Id = d.Id
	inner join #OrderDatas od on d.ToPOID = od.POID
	where m.Status = 'Confirmed'
	  and d.ToStockType = 'B'
	group by m.ID, m.IssueDate, d.ToPOID, d.ToSeq1, d.ToSeq2, m.Type, m.Remark
) t
group by TransactionID, Date, PoId, Seq1, Seq2, Name, ArrivedQty, ReleasedQty, AdjustQty, ReturnQty, Remark
";
            #endregion

            #region Step3.最後的資料整理，篩選 Date並排序
            sqlcmd += @"
select t.PoId
	 , Seq = CONCAT(t.Seq1, ' ' , t.Seq2)
	 , o.BrandID
	 , o.StyleID
	 , psd.Refno
	 , Color = dbo.GetColorMultipleID(o.BrandID, isnull(psds.SpecValue, ''))
	 , t.Date
	 , t.TransactionID
	 , t.Name
	 , f.MtlTypeID
	 , t.ArrivedQty
	 , t.ReleasedQty
	 , t.AdjustQty
	 , t.ReturnQty
	 , t.BalanceQty
	 , m.ALocation
	 , t.Remark
from #TransactionDatas t
inner join #OrderDatas o on t.PoId = o.POID
inner join MDivisionPoDetail m with(nolock) on t.PoId = m.POID and t.Seq1 = m.Seq1 and t.Seq2 = m.Seq2
inner join PO_Supp_Detail psd with(nolock) on t.PoId = psd.ID and t.Seq1 = psd.SEQ1 and t.Seq2 = psd.SEQ2 and psd.FabricType = 'F'
inner join Fabric f with(nolock) on psd.SCIRefno = f.SCIRefno
left join PO_Supp_Detail_Spec psds with(nolock) on psd.ID = psds.ID and psd.SEQ1 = psds.Seq1 and psd.SEQ2 = psds.Seq2 and psds.SpecColumnID = 'Color'";

            if (this.dateRange1.Value1.HasValue)
            {
                sqlcmd += @"
where t.Date between @BeginDate and @EndDate";
            }

            sqlcmd += @"
order by t.PoId, Seq, t.Date, t.ArrivedQty desc, t.TransactionID";
            #endregion

            return sqlcmd;
        }

        private void SetupGrid()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("PoId", header: "SP#", width: Widths.AnsiChars(12))
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(5))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(10))
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(15))
                .Text("Color", header: "Color", width: Widths.AnsiChars(13))
                .Date("Date", header: "Date", width: Widths.AnsiChars(11))
                .Text("TransactionID", header: "Transaction ID", width: Widths.AnsiChars(15))
                .Text("Name", header: "Name", width: Widths.AnsiChars(18))
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(13))
                .Numeric("ArrivedQty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                .Numeric("ReleasedQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                .Numeric("BalanceQty", header: "Balance Qty", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 2)
                .Text("ALocation", header: "Location", width: Widths.AnsiChars(13))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));
        }
    }
}
