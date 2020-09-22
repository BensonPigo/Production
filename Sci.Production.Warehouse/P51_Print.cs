using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P51_Print : Win.Tems.PrintForm
    {
        public P51_Print()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(string.Empty, @"
select  iif(T.stocktype = 'B','Bulk','Inventory') AS stocktype
        ,S.poid AS OrderID,S.seq1  + '-' +S.seq2 as SEQ
        ,S.Roll,S.dyelot
        ,P.Refno
        ,P.fabrictype 
        ,P.ColorID
        ,P.StockUnit
        ,S.QtyBefore AS Bookqty
        ,dbo.Getlocation(fi.ukey) [Location]
        ,S.QtyAfter AS Actualqty 
        ,S.QtyBefore - S.QtyAfter AS Variance
        ,[Total1]=sum(S.QtyBefore) OVER (PARTITION BY S.POID ,S.SEQ1,S.SEQ2 )   
        ,[Total2]=sum(S.QtyAfter) OVER (PARTITION BY S.POID ,S.SEQ1,S.SEQ2 )   			
from dbo.Stocktaking_detail S WITH (NOLOCK) 
LEFT join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = S.POID and  P.SEQ1 = S.Seq1 and P.seq2 = S.Seq2 
left join dbo.FtyInventory Fi on s.poid = fi.poid and s.seq1 = fi.seq1 and s.seq2 = fi.seq2
    and s.roll = fi.roll and s.stocktype = fi.stocktype and s.Dyelot = fi.Dyelot
LEFT JOIN DBO.Stocktaking T  WITH (NOLOCK) ON T.ID = S.Id  WHERE S.Id = @ID", pars, out dt);

            string stockType;
            stockType = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0]["stocktype"].ToString();

            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", issuedate));
            if (this.ReportResourceName == "P51_Report2.rdlc")
            {
                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("StockType", stockType));

                List<P51_PrintData> data = dt.AsEnumerable()
                               .Select(row1 => new P51_PrintData()
                               {
                                   OrderID = row1["OrderID"].ToString(),
                                   SEQ = row1["SEQ"].ToString(),
                                   Roll = row1["Roll"].ToString(),
                                   Dyelot = row1["dyelot"].ToString(),
                                   Refno = row1["Refno"].ToString(),
                                   Type = row1["fabrictype"].ToString(),
                                   Colorid = row1["ColorID"].ToString(),
                                   Unit = row1["StockUnit"].ToString(),
                                   Bookqty = row1["Bookqty"].ToString(),
                                   Location = row1["Location"].ToString(),
                                   Actualqty = row1["Actualqty"].ToString(),
                                   Variance = row1["Variance"].ToString(),
                                   Total1 = row1["Total1"].ToString(),
                                   Total2 = row1["Total2"].ToString(),
                               }).ToList();

                e.Report.ReportDataSource = data;
            }

            return Ict.Result.True;
        }

        public DataRow CurrentDataRow { get; set; }

        private void RadioGroup1_ValueChanged(object sender, EventArgs e)
        {
            this.ReportResourceNamespace = typeof(P51_PrintData);
            this.ReportResourceAssembly = this.ReportResourceNamespace.Assembly;
            this.ReportResourceName = this.radioPanel1.Value == this.radioBackwardSocktakingForm.Value ? "P51_Report1.rdlc" : "P51_Report2.rdlc";
        }
    }
}
