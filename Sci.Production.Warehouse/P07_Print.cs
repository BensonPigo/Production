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
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P07_Print : Sci.Win.Tems.PrintForm
    {
        public P07_Print(List<String> polist)
        {
            InitializeComponent();
            CheckControlEnable();
            this.poidList = polist;
        }
        List<String> poidList;

        protected override bool ValidateInput()
        {

            if (ReportResourceName == "P07_Report2.rdlc")
            {
                if (!poidList.Contains(this.textBox1.Text.TrimEnd(), StringComparer.OrdinalIgnoreCase))
                {
                    MyUtility.Msg.ErrorBox("SP# is not found.");
                    return false;
                }
            }
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string Date1 = ((DateTime)MyUtility.Convert.GetDate(row["PackingReceive"])).ToShortDateString();
            string Date2 = ((DateTime)MyUtility.Convert.GetDate(row["WhseArrival"])).ToShortDateString();
            string ETA = ((DateTime)MyUtility.Convert.GetDate(row["ETA"])).ToShortDateString();
            string Invoice = row["invno"].ToString();
            string Wk = row["exportid"].ToString();
            string FTYID = row["Mdivisionid"].ToString();
         
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
           
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
          
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ETA", ETA));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", Invoice));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Wk", Wk));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FTYID", FTYID));
   
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
            ,[SubVaniance]=sum(R.ActualWeight - R.Weight)OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )  			
            from dbo.Receiving_Detail R
            where R.id = @ID", pars, out dt);
            if (!result) { return result; }
            if (ReportResourceName == "P07_Report2.rdlc")
            {
                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date2", Date2));
                List<P07_PrintData> data = dt.AsEnumerable()
                                .Select(row1 => new P07_PrintData()
                                {
                                    Roll = row1["Roll"].ToString(),
                                    Dyelot = row1["dyelot"].ToString(),
                                    POID = row1["PoId"].ToString(),
                                    SEQ = row1["SEQ"].ToString(), 
                                    Desc = row1["Desc"].ToString(),
                                    ShipQty = row1["ShipQty"].ToString(),
                                    pounit = row1["pounit"].ToString(),
                                    StockQty = row1["StockQty"].ToString(),
                                    StockUnit = row1["StockUnit"].ToString(),
                                    SubStockQty = row1["SubStockQty"].ToString(),
                                    SubQty = row1["SubQty"].ToString()

                                }).ToList();

                e.Report.ReportDataSource = data;

            }
            else
            {

                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date1", Date1));                   
                 List<P07_PrintData> data = dt.AsEnumerable()
                                .Select(row1 => new P07_PrintData()
                                {
                                    Roll = row1["Roll"].ToString(),
                                    Desc = row1["Desc"].ToString(),
                                    ShipQty = row1["ShipQty"].ToString(),
                                    pounit = row1["pounit"].ToString(),
                                    GW = row1["Weight"].ToString(),
                                    AW = row1["ActualWeight"].ToString(),
                                    Vaniance = row1["Vaniance"].ToString(),
                                    SubQty = row1["SubQty"].ToString(),
                                    SubGW = row1["SubGW"].ToString(),
                                    SubAW = row1["SubAW"].ToString(),
                                    SubVaniance = row1["SubVaniance"].ToString()
                                 
                                }).ToList();
                 e.Report.ReportDataSource = data;

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
