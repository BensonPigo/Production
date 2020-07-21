using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ict;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class R08 : Win.Tems.PrintForm
    {
        private DateTime? arrive_s;
        private DateTime? arrive_e;
        private string mdivision;
        private string factory;
        private string spno_s;
        private string spno_e;
        private string brand;
        private DataTable printData;

        /// <summary>
        /// Initializes a new instance of the <see cref="R08"/> class.
        /// </summary>
        /// <param name="menuitem">Q_Q</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            if ((!this.dateArrive.HasValue1 || !this.dateArrive.HasValue2) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Arrive W/H Date > or < SP# > can't be empty!!");
                return false;
            }

            this.arrive_s = this.dateArrive.Value1;
            this.arrive_e = this.dateArrive.Value2;
            this.spno_s = this.txtSPNoStart.Text;
            this.spno_e = this.txtSPNoEnd.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.brand = this.txtBrand.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Where條件
            string where = string.Empty;
            List<string> whereList = new List<string>();
            List<string> whereList_2 = new List<string>();
            List<SqlParameter> paras = new List<SqlParameter>();

            if (this.arrive_s.HasValue && this.arrive_e.HasValue)
            {
                whereList.Add("R.WhseArrival between @WhseArrival_s and @WhseArrival_e " + Environment.NewLine);
                whereList_2.Add("t.IssueDate between @WhseArrival_s and @WhseArrival_e " + Environment.NewLine);

                paras.Add(new SqlParameter("@WhseArrival_s", this.arrive_s.Value));
                paras.Add(new SqlParameter("@WhseArrival_e", this.arrive_e.Value));
            }

            if (!MyUtility.Check.Empty(this.spno_s))
            {
                whereList.Add("f.POID >= @spno_s" + Environment.NewLine);
                whereList_2.Add("f.POID >= @spno_s" + Environment.NewLine);
                paras.Add(new SqlParameter("@spno_s", this.spno_s));
            }

            if (!MyUtility.Check.Empty(this.spno_e))
            {
                whereList.Add("f.POID <= @spno_e" + Environment.NewLine);
                whereList_2.Add("f.POID <= @spno_e" + Environment.NewLine);
                paras.Add(new SqlParameter("@spno_e", this.spno_e));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                whereList.Add("o.MDivisionid = @mdivision" + Environment.NewLine);
                whereList_2.Add("o.MDivisionid = @mdivision" + Environment.NewLine);
                paras.Add(new SqlParameter("@mdivision", this.mdivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                whereList.Add("o.FtyGroup = @factory" + Environment.NewLine);
                whereList_2.Add("o.FtyGroup = @factory" + Environment.NewLine);
                paras.Add(new SqlParameter("@factory", this.factory));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                whereList.Add("o.BrandID = @brand" + Environment.NewLine);
                whereList_2.Add("o.BrandID = @brand" + Environment.NewLine);
                paras.Add(new SqlParameter("@brand", this.brand));
            }

            #endregion

            string cmd = $@"

SELECT
	f.POID
	,[Seq]=RD.Seq1+ '-'+RD.Seq2
	,o.BrandID
	,o.StyleID
	,PSD.Refno
	,PSD.ColorID
	,FC.WeaveTypeID
	,R.WhseArrival
	,F.ArriveQty
	,[ReleaseQty]=SUM(FIT.OutQty) 
	,[LastReleaseDate]=LastReleaseDate.Date
from FIR F 
inner join Receiving_Detail RD on F.ReceivingID=RD.ID and F.POID=RD.PoId and RD.Seq1=F.Seq1 and RD.Seq2=F.Seq2 
inner join Receiving R on RD.ID=R.ID
Left join Orders O on F.POID=O.ID
Left join PO_Supp_Detail PSD on PSD.ID=F.POID and PSD.Seq1=F.Seq1 and PSD.Seq2=F.Seq2
Left join FtyInventory FIT on FIT.POID=RD.POID and FIT.Seq1=RD.Seq1 and FIT.Seq2=RD.Seq2 and FIT.Roll=RD.Roll and FIT.Dyelot=RD.Dyelot and FIT.StockType=RD.StockType
Left join Fabric FC on PSD.SCIRefno=FC.SCIRefno
OUTER APPLY(
    SELECT [Date] = Max(EditDate)
    FROM (
	    SELECT a.EditDate
	    FROM dbo.Issue a INNER JOIN dbo.Issue_Detail b ON b.id = a.id
	    WHERE b.POID=o.ID AND b.Seq1=f.Seq1 AND b.Seq2=f.Seq2 AND b.Roll=rd.Roll AND b.Dyelot=rd.Dyelot AND b.StockType=rd.StockType AND a.status='Confirmed'
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.IssueLack a INNER JOIN dbo.IssueLack_Detail b ON b.id = a.id
	    WHERE a.Type = 'R' AND b.POID=o.ID AND b.Seq1=f.Seq1 AND b.Seq2=f.Seq2 AND b.Roll=rd.Roll AND b.Dyelot=rd.Dyelot AND b.StockType=rd.StockType AND a.status='Confirmed'
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.SubTransfer a INNER JOIN dbo.SubTransfer_Detail b ON b.id = a.id
	    WHERE a.Type = 'A' AND b.FromPoId=o.ID AND b.FromSeq1=f.Seq1 AND b.FromSeq2=f.Seq2 AND b.FromRoll=rd.Roll AND b.FromDyelot=rd.Dyelot AND b.FromStockType=rd.StockType AND a.status='Confirmed' 
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.BorrowBack a INNER JOIN dbo.BorrowBack_Detail b ON b.id = a.id
	    WHERE b.FromPoId=o.ID AND b.FromSeq1=f.Seq1 AND b.FromSeq2=f.Seq2 AND b.FromRoll=rd.Roll AND b.FromDyelot=rd.Dyelot AND b.FromStockType=rd.StockType AND a.status='Confirmed' 
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.TransferOut a INNER JOIN dbo.TransferOut_Detail b ON b.id = a.id
	    WHERE b.PoId=o.ID AND b.Seq1=f.Seq1 AND b.Seq2=f.Seq2 AND b.Roll=rd.Roll AND b.Dyelot=rd.Dyelot AND b.StockType=rd.StockType AND a.status='Confirmed' 
    ) a 
)LastReleaseDate

WHERE {whereList.JoinToString(" AND ")}

Group by F.POID,RD.Seq1,RD.Seq2,O.StyleID,PSD.Refno,PSD.ColorID,FC.WeaveTypeID,R.WhseArrival,F.ArriveQty,LastReleaseDate.Date,o.BrandID

UNION---------------

select 
	F.POID
	,[Seq]=TD.Seq1 + '-'+TD.Seq2
	,o.BrandID
	,O.StyleID
	,PSD.Refno
	,PSD.ColorID
	,FC.WeaveTypeID
	,t.IssueDate
	,F.ArriveQty
	,sum(FIT.OutQty) 
	,[LastReleaseDate]=LastReleaseDate.Date
from FIR F 
inner join TransferIn_Detail TD on F.ReceivingID=TD.ID and F.POID=TD.PoId and TD.Seq1=F.Seq1 and TD.Seq2=F.Seq2 
inner join TransferIn T on TD.ID=T.ID
Left join Orders O on F.POID=O.ID
Left join PO_Supp_Detail PSD on PSD.ID=F.POID and PSD.Seq1=F.Seq1 and PSD.Seq2=F.Seq2
Left join FtyInventory FIT on FIT.POID=TD.POID and FIT.Seq1=TD.Seq1 and FIT.Seq2=TD.Seq2 and FIT.Roll=TD.Roll and FIT.Dyelot=TD.Dyelot and FIT.StockType=TD.StockType
Left join Fabric FC on PSD.SCIRefno=FC.SCIRefno
OUTER APPLY(
    SELECT [Date] = Max(EditDate)
    FROM (
	    SELECT a.EditDate
	    FROM dbo.Issue a INNER JOIN dbo.Issue_Detail b ON b.id = a.id
	    WHERE b.POID=o.ID AND b.Seq1=f.Seq1 AND b.Seq2=f.Seq2 AND b.Roll=TD.Roll AND b.Dyelot=td.Dyelot AND b.StockType=td.StockType AND a.status='Confirmed' 
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.IssueLack a INNER JOIN dbo.IssueLack_Detail b ON b.id = a.id
	    WHERE a.Type = 'R' AND b.POID=o.ID AND b.Seq1=f.Seq1 AND b.Seq2=f.Seq2 AND b.Roll=TD.Roll AND b.Dyelot=td.Dyelot AND b.StockType=td.StockType AND a.status='Confirmed' 
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.SubTransfer a INNER JOIN dbo.SubTransfer_Detail b ON b.id = a.id
	    WHERE a.Type = 'A' AND b.FromPoId=o.ID AND b.FromSeq1=f.Seq1 AND b.FromSeq2=f.Seq2 AND b.FromRoll=TD.Roll AND b.FromDyelot=td.Dyelot AND b.FromStockType=td.StockType AND a.status='Confirmed' 
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.BorrowBack a INNER JOIN dbo.BorrowBack_Detail b ON b.id = a.id
	    WHERE b.FromPoId=o.ID AND b.FromSeq1=f.Seq1 AND b.FromSeq2=f.Seq2 AND b.FromRoll=TD.Roll AND b.FromDyelot=td.Dyelot AND b.FromStockType=td.StockType AND a.status='Confirmed' 
	    UNION ALL
	    SELECT a.EditDate
	    FROM dbo.TransferOut a INNER JOIN dbo.TransferOut_Detail b ON b.id = a.id
	    WHERE b.PoId=o.ID AND b.Seq1=f.Seq1 AND b.Seq2=f.Seq2 AND b.Roll=TD.Roll AND b.Dyelot=td.Dyelot AND b.StockType=td.StockType AND a.status='Confirmed' 
    ) a 
)LastReleaseDate

WHERE {whereList_2.JoinToString(" AND ")}

Group by F.POID,TD.Seq1,TD.Seq2,o.BrandID,O.StyleID,PSD.Refno,PSD.ColorID,FC.WeaveTypeID,t.IssueDate,F.ArriveQty,LastReleaseDate.Date
";

            DualResult result = DBProxy.Current.Select(null, cmd.ToString(), paras, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R08.xltx", 1);
            return true;
        }
    }
}
