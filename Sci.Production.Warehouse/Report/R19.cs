using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class R19 : Win.Tems.PrintForm
    {
        DataTable dt;

        int selectindex = 0;

        public R19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.comboCategoryAlreadyReturn.SelectedIndex = 0;
            this.txtfactory.Text = Env.User.Factory;
        }

        protected override bool ValidateInput()
        {
            this.selectindex = this.comboCategoryAlreadyReturn.SelectedIndex;
            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dt.Rows.Count);
            DualResult result = Ict.Result.True;
            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return result;
            }

            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R19_Material_Borrowing.xltx");
            return false;
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // return base.OnAsyncDataLoad(e);
            DateTime? returnDate1 = this.dateEstReturnDate.Value1;
            DateTime? returnDate2 = this.dateEstReturnDate.Value2;
            DateTime? borrowDate1 = this.dateRangeBorrowDate.Value1;
            DateTime? borrowDate2 = this.dateRangeBorrowDate.Value2;
            DateTime? buyDeliveryDate1 = this.dateRangeBuyerDlv.Value1;
            DateTime? buyDeliveryDate2 = this.dateRangeBuyerDlv.Value2;
            string factory = this.txtfactory.Text;
            string spno = this.txtBorrowSPNo.Text.TrimEnd();

            DualResult result = Ict.Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            sqlcmd.Append(@"
with cte as (
      Select  a.id
			, a.FactoryID
			, a.DepartmentID
			, a.SewingLineID
			, a.Shift
            , frompoid
            , fromseq1
            , FromSeq2
			, b.FromRoll
			, b.FromDyelot
            , FromStockType
            , sum(b.qty) qty
            , issuedate
            , estbackdate
            , backdate
            , o.MCHandle
            , [ReturnRoll] = rt.[Return Roll#]
			, [ReturnDyelot] = rt.[Return Dyelot]
    from borrowback a WITH (NOLOCK) 
    inner join borrowback_detail b WITH (NOLOCK) on b.id = a.id 
    left join Orders o WITH (NOLOCK)  on b.FromPOID=o.ID
	outer apply(
		SELECT BBD.FromRoll AS [Return Roll#] 
		,BBD.FromDyelot AS [Return Dyelot] 
		from BorrowBack ABB
		LEFT JOIN BorrowBack BBB ON BBB.BorrowId = ABB.ID
		LEFT JOIN BorrowBack_Detail BBD ON BBB.ID = BBD.ID
		WHERE ABB.ID =a.Id                              
	)rt
    Where   a.type='A'
            and a.Status = 'Confirmed'");
            if (!MyUtility.Check.Empty(returnDate1) || !MyUtility.Check.Empty(returnDate2))
            {
                if (!MyUtility.Check.Empty(returnDate1))
                {
                    sqlcmd.Append(string.Format(
                        @" 
            and '{0}' <= a.estbackdate", Convert.ToDateTime(returnDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(returnDate2))
                {
                    sqlcmd.Append(string.Format(
                        @" 
            and a.estbackdate <= '{0}'", Convert.ToDateTime(returnDate2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(borrowDate1) || !MyUtility.Check.Empty(borrowDate2))
            {
                if (!MyUtility.Check.Empty(borrowDate1))
                {
                    sqlcmd.Append($@"
            and '{Convert.ToDateTime(borrowDate1).ToString("d")}' <= a.issuedate");
                }

                if (!MyUtility.Check.Empty(borrowDate2))
                {
                    sqlcmd.Append($@" 
            and a.issuedate <= '{Convert.ToDateTime(borrowDate2).ToString("d")}'");
                }
            }

            if (!MyUtility.Check.Empty(buyDeliveryDate1) || !MyUtility.Check.Empty(buyDeliveryDate2))
            {
                if (!MyUtility.Check.Empty(buyDeliveryDate1))
                {
                    sqlcmd.Append($@"
            and '{Convert.ToDateTime(buyDeliveryDate1).ToString("d")}' <= o.BuyerDelivery");
                }

                if (!MyUtility.Check.Empty(buyDeliveryDate2))
                {
                    sqlcmd.Append($@" 
            and o.BuyerDelivery <= '{Convert.ToDateTime(buyDeliveryDate2).ToString("d")}'");
                }
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd.Append(string.Format(
                    @"
            And a.FactoryID = '{0}'", factory));
            }

            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(string.Format(
                    @"
            And b.frompoid = '{0}'", spno));
            }

            if (!this.txtSeq.CheckSeq1Empty())
            {
                sqlcmd.Append(string.Format(
                    @"
            and b.fromSeq1 = '{0}'", this.txtSeq.Seq1));
            }

            if (!this.txtSeq.CheckSeq2Empty())
            {
                sqlcmd.Append(string.Format(
                    @" 
            and b.FromSeq2 = '{0}'", this.txtSeq.Seq2));
            }

            switch (this.selectindex)
            {
                case 0:
                    sqlcmd.Append(@" 
            And a.BackDate is null ");
                    break;
                case 1:
                    sqlcmd.Append(@" 
            And a.BackDate is not null ");
                    break;
                case 2:
                    break;
            }

            sqlcmd.Append(string.Format(@" 
     Group by a.id, a.FactoryID, a.DepartmentID, a.SewingLineID
			, a.Shift, frompoid, fromseq1, FromSeq2
			, b.FromRoll, b.FromDyelot, FromStockType
            , issuedate, estbackdate, backdate, o.MCHandle
            , rt.[Return Dyelot],rt.[Return Roll#]
)
select  cte.id
		, cte.FactoryID
		, cte.DepartmentID
		, cte.SewingLineID
		, cte.Shift
		, cte.FromPOID
		, [OrderIdList] = stuff((select concat('/',tmp.OrderID) 
		                                    from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = cte.FromPOID and e.SEQ1 =cte.FromSeq1 and e.SEQ2 = cte.FromSeq2
		                                    ) tmp for xml path(''))
                                    ,1,1,'')
        
        , cte.FromSeq1
        , cte.FromSeq2
		, cte.FromRoll
		, cte.FromDyelot
        , stocktype = case cte.FromStockType 
                        when 'B' then 'Bulk' 
                        when 'I' then 'Inventory' 
                        else  cte.FromStockType 
                      end 
        , cte.qty 
        , backQty = isnull((select sum(qty) 
                            from BorrowBack x WITH (NOLOCK) 
                            inner join BorrowBack_Detail y WITH (NOLOCK) on y.id = x.id 
                            where x.Borrowid = cte.id
									and x.Status = 'Confirmed' 
                                    and x.type='B' 
                                    and y.ToPOID = cte.FromPOID 
                                    and y.ToSeq1 = cte.FromSeq1
                                    and y.ToSeq2 = cte.FromSeq2 
                                    and y.ToStockType = cte.FromStockType
                            ),0.00) 
        , balance = cte.qty - isnull((  select sum(qty) 
                                        from BorrowBack x WITH (NOLOCK) 
                                        inner join BorrowBack_Detail y WITH (NOLOCK) on y.id = x.id 
                                        where   x.Borrowid = cte.id
                                                and x.Status = 'Confirmed' 
                                                and x.type='B' 
                                                and y.ToPOID = cte.FromPOID 
                                                and y.ToSeq1 = cte.FromSeq1
                                                and y.ToSeq2 = cte.FromSeq2 
                                                and y.ToStockType = cte.FromStockType)
                                    ,0.00)
        , cte.IssueDate
        , cte.EstBackDate 
        , cte.BackDate
		, [ReturnRoll] = cte.ReturnRoll
		, [ReturnDyelot] = cte.ReturnDyelot
		, [McHandle] = dbo.getPass1_ExtNo(cte.MCHandle)
from cte"));

            #endregion

            try
            {
                DBProxy.Current.DefaultTimeout = 600;
                result = DBProxy.Current.Select(null, sqlcmd.ToString(), out this.dt);
                DBProxy.Current.DefaultTimeout = 30;
                if (!result)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
