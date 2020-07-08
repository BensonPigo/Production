using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    public partial class R09 : Win.Tems.PrintForm
    {
        DateTime? DateInspDateStart; DateTime? DateInspDateEnd;
        DateTime? DateArrStart; DateTime? DateArrEnd;
        string spStrat; string spEnd; string Ref; string Supp;
        List<SqlParameter> lis;
        DataTable dt; string cmd;

        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            this.DateArrStart = this.dateArriveWHDate.Value1;
            this.DateArrEnd = this.dateArriveWHDate.Value2;
            this.DateInspDateStart = this.dateInspDate.Value1;
            this.DateInspDateEnd = this.dateInspDate.Value2;
            this.spStrat = this.txtSPStart.Text.ToString();
            this.spEnd = this.txtSPEnd.Text.ToString();
            this.Ref = this.txtRefno.Text.ToString();
            this.Supp = this.txtsupplier.TextBox1.Text;

            bool date_Arrive_Empty = MyUtility.Check.Empty(this.DateArrStart) || MyUtility.Check.Empty(this.DateArrEnd),
                date_SCI_Empty = MyUtility.Check.Empty(this.DateInspDateStart) || MyUtility.Check.Empty(this.DateInspDateEnd);

            if (date_Arrive_Empty && date_SCI_Empty)
            {
                this.dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select [Last Physical Insp Date] or [Arrive W/H Date] at least one field entry.");
                return false;
            }

            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--

            if (!MyUtility.Check.Empty(this.DateInspDateStart) && !MyUtility.Check.Empty(this.DateInspDateEnd))
            {
                sqlWheres.Add("fir.PhysicalDate between @PhysicalDate1 and @PhysicalDate2");
                this.lis.Add(new SqlParameter("@PhysicalDate1", this.DateInspDateStart));
                this.lis.Add(new SqlParameter("@PhysicalDate2", this.DateInspDateEnd));
            }

            if (!MyUtility.Check.Empty(this.DateArrStart) && !MyUtility.Check.Empty(this.DateArrEnd))
            {
                sqlWheres.Add("rec.WhseArrival between @ArrDate1 and @ArrDate2");
                this.lis.Add(new SqlParameter("@ArrDate1", this.DateArrStart));
                this.lis.Add(new SqlParameter("@ArrDate2", this.DateArrEnd));
            }

            if (!MyUtility.Check.Empty(this.spStrat))
            {
                sqlWheres.Add("fir.POID >= @sp1");
                this.lis.Add(new SqlParameter("@sp1", this.spStrat));
            }

            if (!MyUtility.Check.Empty(this.spEnd))
            {
                sqlWheres.Add("fir.POID <= @sp2");
                this.lis.Add(new SqlParameter("@sp2", this.spEnd));
            }

            if (!MyUtility.Check.Empty(this.Ref))
            {
                sqlWheres.Add("fir.Refno = @Ref");
                this.lis.Add(new SqlParameter("@Ref", this.Ref));
            }

            if (!MyUtility.Check.Empty(this.Supp))
            {
                sqlWheres.Add("fir.Suppid = @Supp");
                this.lis.Add(new SqlParameter("@Supp", this.Supp));
            }

            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            if (sqlWheres.Count != 0)
            {
                sqlWhere = " where " + sqlWhere;
            }
            #region --撈ListExcel資料--

            this.cmd = string.Format(
@"
Select row_number() over(ORDER BY ord.FactoryID, fir.poid, ord.StyleID, fir.SEQ1, fir.SEQ2, firo.roll, firo.Dyelot) as [No]
	  --, ord.FactoryID [Factory]
	  , rec.whseArrival [Fabric Received Date]
	  , fir.OdorDate [Inspection Date]
	  , fir.Suppid [Supplier ID]
	  , (select supp.AbbEN from supp where id= fir.Suppid) [Supplier Name]
	  , fir.Refno [Reference No]
	  , d.ColorID [Color Code]
	  , (select name from Color where id=d.ColorID and BrandId = d.BrandId) [Color Name]
      , fir.poid [SP#]
	  , ord.StyleID as style
	  , fir.SEQ1+fir.SEQ2 as [SEQ]
	  , firo.roll [Roll]
	  , firo.Dyelot [Lot/Batch]
	  , firo.Inspector + '-' + p.Name  as Inspector 
	  , iif(fir.nonOdor=1,'no inspection',firo.Result)  [Result]
From FIR fir  
left join FIR_Odor firo on fir.id=firo.id
inner join orders ord on ord.ID = fir.POID 
inner join PO_Supp_Detail d on d.id = fir.poid and d.seq1 = fir.seq1 and d.seq2 = fir.seq2
Left join dbo.View_AllReceiving rec on rec.id = fir.receivingid
left join pass1 p on firo.Inspector = p.ID 
{0}
order by ord.FactoryID, fir.poid, ord.StyleID, fir.SEQ1, fir.SEQ2, firo.roll, firo.Dyelot
", sqlWhere);
            #endregion
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            string xltx = "Quality_R09.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, xltx, 2, true, null, objApp); // 將datatable copy to excel
            return true;
        }
    }
}
