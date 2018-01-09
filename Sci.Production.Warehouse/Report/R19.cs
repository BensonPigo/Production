using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;

namespace Sci.Production.Warehouse
{
    public partial class R19 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;

        int selectindex = 0;
        public R19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
            comboCategoryAlreadyReturn.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            selectindex = comboCategoryAlreadyReturn.SelectedIndex;
            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            SetCount(dt.Rows.Count);
            DualResult result = Result.True;
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return result;
            }
            MyUtility.Excel.CopyToXls(dt, "", "Warehouse_R19_Material_Borrowing.xltx");
            return false;

        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            //return base.OnAsyncDataLoad(e);
            DateTime? returnDate1 = dateEstReturnDate.Value1;
            DateTime? returnDate2 = dateEstReturnDate.Value2;
            String spno = txtBorrowSPNo.Text.TrimEnd();

            DualResult result = Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            sqlcmd.Append(@"
with cte as (
    Select  a.id
            , frompoid
            , fromseq1
            , FromSeq2
            , FromStockType
            , sum(qty) qty
            , issuedate
            , estbackdate
            , backdate
    from borrowback a WITH (NOLOCK) 
    inner join borrowback_detail b WITH (NOLOCK) on b.id = a.id 
    Where   a.type='A'
            and a.Status = 'Confirmed'");
            if (!MyUtility.Check.Empty(returnDate1) || !MyUtility.Check.Empty(returnDate2))
            {
                if (!MyUtility.Check.Empty(returnDate1))
                    sqlcmd.Append(string.Format(@" 
            and '{0}' <= a.estbackdate", Convert.ToDateTime(returnDate1).ToString("d")));
                if (!MyUtility.Check.Empty(returnDate2))
                    sqlcmd.Append(string.Format(@" 
            and a.estbackdate <= '{0}'", Convert.ToDateTime(returnDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(string.Format(@"
            And b.frompoid = '{0}'", spno));
            }

            if (!txtSeq.checkSeq1Empty())
            {
                sqlcmd.Append(string.Format(@"
            and b.fromSeq1 = '{0}'", txtSeq.seq1));
            }
            if (!txtSeq.checkSeq2Empty())
            {
                sqlcmd.Append(string.Format(@" 
            and b.FromSeq2 = '{0}'", txtSeq.seq2));
            }

            switch (selectindex)
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
    Group by a.id, frompoid, FromSeq1, FromSeq2, FromStockType
             , a.IssueDate, estbackdate, backdate
)
select  cte.id
        , cte.FromPOID
        , cte.FromSeq1
        , cte.FromSeq2
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
from cte"));

            #endregion

            try
            {
                DBProxy.Current.DefaultTimeout = 600;
                result = DBProxy.Current.Select(null, sqlcmd.ToString(), out dt);
                DBProxy.Current.DefaultTimeout = 30;
                if (!result) return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void toexcel_Click(object sender, EventArgs e)
        {

        }

        private void R18_Load(object sender, EventArgs e)
        {

        }
    }
}
