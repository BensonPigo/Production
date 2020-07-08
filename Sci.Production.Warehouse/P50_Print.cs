using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P50_Print : Sci.Win.Tems.PrintForm
    {
        public DataRow CurrentDataRow { get; set; }

        public P50_Print(DataRow row)
        {
            this.InitializeComponent();
            this.CurrentDataRow = row;
        }

        string selectOption;

        protected override bool ValidateInput()
        {
           return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(
                string.Empty,
                @"
select Iif(Stocktaking.stocktype='B','Bulk','Inventory') as ST
from dbo.Stocktaking WITH (NOLOCK)
where id = @ID",
                pars,
                out dt);
            if (!result)
            {
                return result;
            }

            string ST = dt.Rows[0]["ST"].ToString();
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ST", ST));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result = DBProxy.Current.Select(string.Empty, @"
select a.POID
        ,a.Seq1+'-'+a.seq2 as SEQ
		,a.Roll,a.Dyelot	        
		,Ref = b.Refno 
		,Material_Type =b.fabrictype 
		,Color =b.colorid 
		,Unit=b.stockunit 		     
		,[Book_Location]=dbo.Getlocation(fi.ukey)
from dbo.Stocktaking_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.Seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
where a.id= @ID", pars, out dd);
            if (!result)
            {
                this.ShowErr(result);
            }

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable da;
            result = DBProxy.Current.Select(string.Empty, @"
select a.POID
        ,a.Seq1+'-'+a.seq2 as SEQ
		,a.Roll,a.Dyelot	        
		,Ref = b.Refno 
		,Material_Type =b.fabrictype 
		,Color =b.colorid 
		,Unit=b.stockunit 
		,Book_Qty=a.qtybefore	
		,[Book_Location]=dbo.Getlocation(fi.ukey) 
		,Actual_Qty=a.Qtyafter
		,Variance=a.Qtyafter-a.qtybefore
		,[Total1]=sum(a.qtybefore) OVER (PARTITION BY a.POID ,a.Seq1,a.Seq2 )
		,[Total2]=sum(a.Qtyafter) OVER (PARTITION BY a.POID ,a.seq1,a.Seq2 )
from dbo.Stocktaking_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.Seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
where a.id= @ID", pars, out da);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (this.isStockList)
            {
                List<P50_PrintData> data1 = da.AsEnumerable()
               .Select(row1 => new P50_PrintData()
               {
                   POID = row1["POID"].ToString(),
                   SEQ = row1["SEQ"].ToString(),
                   Roll = row1["Roll"].ToString(),
                   Dyelot = row1["Dyelot"].ToString(),
                   Ref = row1["Ref"].ToString(),
                   Material_Type = row1["Material_Type"].ToString(),
                   Color = row1["Color"].ToString(),
                   Unit = row1["Unit"].ToString(),
                   Book_Qty = row1["Book_Qty"].ToString(),
                   Book_Location = row1["Book_Location"].ToString(),
                   Actual_Qty = row1["Actual_Qty"].ToString(),
                   Variance = row1["Variance"].ToString(),
                   Total1 = row1["Total1"].ToString(),
                   Total2 = row1["Total2"].ToString(),
               }).ToList();

                e.Report.ReportDataSource = data1;
            }
            else
            {
                List<P50_PrintData> data = dd.AsEnumerable()
                 .Select(row1 => new P50_PrintData()
                 {
                     POID = row1["POID"].ToString(),
                     SEQ = row1["SEQ"].ToString(),
                     Roll = row1["Roll"].ToString(),
                     Dyelot = row1["Dyelot"].ToString(),
                     Ref = row1["Ref"].ToString(),
                     Material_Type = row1["Material_Type"].ToString(),
                     Color = row1["Color"].ToString(),
                     Unit = row1["Unit"].ToString(),
                     Book_Location = row1["Book_Location"].ToString(),
                 }).ToList();

                e.Report.ReportDataSource = data;
            }

            return Result.True;
        }

        bool isStockList = false;

        private void radioGroup1_ValueChanged(object sender, EventArgs e)
        {
            this.selectOption = this.radioGroup1.Value;
            this.isStockList = this.selectOption == this.radioStocktakingList.Value;

            this.ReportResourceNamespace = typeof(P50_PrintData);
            this.ReportResourceAssembly = this.ReportResourceNamespace.Assembly;
            this.ReportResourceName = this.selectOption == this.radioBookQty.Value ? "P50BookQty_Print.rdlc" : "P50List_Print.rdlc";
        }
    }
}
