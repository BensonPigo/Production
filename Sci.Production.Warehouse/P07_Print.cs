using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P07_Print : Sci.Win.Tems.PrintForm
    {
        public P07_Print()
        {
            InitializeComponent();
            CheckControlEnable();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            string Date1 = row["PackingReceive"].ToString();
            string ETA = row["ETA"].ToString();
            string Invoice = row["invno"].ToString();
            string Wk = row["exportid"].ToString();
            string FTYID = row["Mdivisionid"].ToString();
           

            List<SqlParameter> pars = new List<SqlParameter>();
            DataTable dtTitle;
            DualResult titleResult = DBProxy.Current.Select("",
           @"select m.name
             from  dbo.Receiving r
             left join dbo.MDivision m
             on m.id = r.MDivisionID 
             where m.id = r.MDivisionID
             and r.id = @ID", pars, out dtTitle);
            if (!titleResult) { this.ShowErr(titleResult); }
            string RptTitle = dtTitle.Rows[0]["name"].ToString();
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date1", Date1));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ETA", ETA));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", Invoice));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Wk", Wk));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FTYID", FTYID));
            string Date2;
            Date2 = dtTitle.Rows.Count == 0 ? "" : dtTitle.Rows[0]["WhseArrival"].ToString();
  


            if (ReportResourceName == "P07_Report2.rdlc")
            {
                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date2", Date2));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select  
			R.Roll,R.Dyelot,R.PoId,R.Seq1+'-'+R.Seq2 AS SEQ
			,DBO.getMtlDesc(R.POID,R.Seq1,R.Seq2,2,0)[Desc]
			,R.ShipQty,R.pounit,R.StockQty,R.StockUnit, R.Weight,R.ActualWeight
			,R.ActualWeight - R.Weight AS Vaniance
			,[SubQty]=sum(R.ShipQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
			,[SubGW]=sum(R.Weight) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 ) 
			,[SubAW]=sum(R.ActualWeight) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
			,[SubStockQty]=sum(R.StockQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )      			
            from dbo.Receiving_Detail R", pars, out dt); ;
            //if (!result) { return result; }
            //if (!result) { this.ShowErr(result); }
            //     string StockType = dt.Rows[0]["stocktype"].ToString();
          


              /*  List<P07_PrintData> data = dt.AsEnumerable()
                               .Select(row1 => new P07_PrintData()
                               {

                                   POID = row1["OrderID"].ToString(),
                                   SEQ = row1["SEQ"].ToString(),
                                   Roll = row1["Roll"].ToString(),
                                   Dyelot = row1["dyelot"].ToString(),
                                   Desc = row1["Refno"].ToString(),
                                   ShipQty = row1["fabrictype"].ToString(),
                                   pounit = row1["ColorID"].ToString(),
                                   GW = row1["StockUnit"].ToString(),
                                   AW = row1["Bookqty"].ToString(),
                                   Vaniance = row1["Location"].ToString(),
                                   StockQty = row1["Actualqty"].ToString(),
                                   SubAW = row1["Variance"].ToString(),
                                   SubGW = row1["Total1"].ToString(),
                                   SubVaniance = row1["Total2"].ToString(),
                                   StockUnit = row1["Variance"].ToString(),
                                   SubStockQty = row1["Total1"].ToString(),
                                   SubQty = row1["Total2"].ToString()

                               }).ToList();
                

                e.Report.ReportDataSource = data;*/
            }
           

            
            
            return Result.True;
        }

        public DataRow CurrentDataRow { get; set; }

        private void radioGroup1_ValueChanged(object sender, EventArgs e)
        {



            this.ReportResourceNamespace = typeof(P07_PrintData);
            this.ReportResourceAssembly = ReportResourceNamespace.Assembly;
            this.ReportResourceName = this.radioPanel1.Value == this.radioButton1.Value ? "P07_Report1.rdlc" : "P07_Report2.rdlc";

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            CheckControlEnable();

        }
        private void CheckControlEnable()
        {
            if (radioButton1.Checked == true)
            {
               
                textBox1.Enabled = false;
               
            }
            else
            {
               
                textBox1.Enabled = true;
            }
        }

    }
}
