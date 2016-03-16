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
            cbxCategory.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            selectindex = cbxCategory.SelectedIndex;
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
            String returnDate1 = dateRange_ReturnDate.Text1;
            String returnDate2 = dateRange_ReturnDate.Text2;
            String spno = tbxSP.Text.TrimEnd();
            String seq = tbxSeq.Text.PadRight(5);

            DualResult result = Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            sqlcmd.Append(@"with cte 
as
(Select a.id, frompoid, fromseq1,FromSeq2 ,FromStockType,sum(qty) qty, issuedate, estbackdate, backdate
from borrowback a inner join borrowback_detail b on b.id = a.id 
Where a.type='A'
and a.Status = 'Confirmed'");
            if (!MyUtility.Check.Empty(dateRange_ReturnDate.Value1))
            {
                sqlcmd.Append(string.Format(" and a.estbackdate between '{0}' and '{1}'", returnDate1, returnDate2));
            }
            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(string.Format(" And b.frompoid = '{0}'", spno));
            }
            if (!MyUtility.Check.Empty(seq))
            {
                sqlcmd.Append(string.Format(" And b.fromseq1 = '{0}' and b.FromSeq2 ='{1}'", seq.Substring(0, 3), seq.Substring(3)));
            }

            switch (selectindex)
            {
                case 0:
                    sqlcmd.Append(@" And a.BackDate is null ");
                    break;
                case 1:
                    sqlcmd.Append(@" And a.BackDate is not null ");
                    break;
                case 2:
                    break;
            }

            sqlcmd.Append(string.Format(@" and a.mdivisionid = '{0}'
Group by a.id, frompoid, FromSeq1,FromSeq2 
,FromStockType
, a.IssueDate, estbackdate, backdate
)
select cte.id,cte.FromPOID,cte.FromSeq1,cte.FromSeq2
,case cte.FromStockType when 'B' then 'Bulk' when 'I' then 'Inventory' else  cte.FromStockType end as stocktype
, cte.qty 
,isnull((select sum(qty) from BorrowBack x inner join BorrowBack_Detail y on y.id = x.id 
where x.mdivisionid = '{0}' and x.Status = 'Confirmed' and x.type='B' and y.ToPOID = cte.FromPOID and y.ToSeq1 = cte.FromSeq1
and y.ToSeq2 = cte.FromSeq2 and y.ToStockType = cte.FromStockType),0.00) as backQty
, cte.qty - isnull((select sum(qty) from BorrowBack x inner join BorrowBack_Detail y on y.id = x.id 
where x.mdivisionid = '{0}' and x.Status = 'Confirmed' and x.type='B' and y.ToPOID = cte.FromPOID and y.ToSeq1 = cte.FromSeq1
and y.ToSeq2 = cte.FromSeq2 and y.ToStockType = cte.FromStockType),0.00) as balance
,cte.IssueDate
,cte.EstBackDate
,cte.BackDate
from cte", Env.User.Keyword));

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
