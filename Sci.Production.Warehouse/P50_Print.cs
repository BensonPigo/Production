using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    public partial class P50_Print : Sci.Win.Tems.PrintForm
    {
        public P50_Print()
        {
            InitializeComponent();
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
            DualResult result= DBProxy.Current.Select("",
            @"select case stocktype
		    when 'B' then 'Bulk'
	        when 'I' then 'Inventory'
		    ELSE stocktype
	        end ST
		    from dbo.Stocktaking	
            where a.id = @ID", pars, out dt); ;
            if (!result) { return result; }
            string ST = dt.Rows[0]["stocktype"].ToString();
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ST", ST));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result = DBProxy.Current.Select("",
             @"select a.POID,a.Seq1+'-'+a.seq2 as SEQ
			 ,a.Roll,a.Dyelot	        
			 ,Ref = b.Refno 
			 ,Material_Type =b.fabrictype 
			 ,Color =b.colorid 
			 ,Unit=b.stockunit 		     
		     ,dbo.Getlocation(a.FtyInventoryUkey)[Book_Location] 
             from dbo.Stocktaking_detail a 
             left join dbo.PO_Supp_Detail b
             on 
             b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.Seq2
             where a.id= @ID", pars, out dd);
            if (!result) { this.ShowErr(result); }


            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable da;
            result = DBProxy.Current.Select("",
             @"select a.POID,a.Seq1+'-'+a.seq2 as SEQ
			 ,a.Roll,a.Dyelot	        
			 ,Ref = b.Refno 
			 ,Material_Type =b.fabrictype 
			 ,Color =b.colorid 
			 ,Unit=b.stockunit 
			 ,Book_Qty=a.qtybefore	
		     ,dbo.Getlocation(a.FtyInventoryUkey)[Book_Location] 
			 , Actual_Qty=a.Qtyafter
			 ,Variance=a.Qtyafter-a.qtybefore
			 ,[Total1]=sum(a.qtybefore) OVER (PARTITION BY a.POID ,a.Seq1,a.Seq2 )
			 ,[Total2]=sum(a.Qtyafter) OVER (PARTITION BY a.POID ,a.seq1,a.Seq2 )
             from dbo.Stocktaking_detail a 
             left join dbo.PO_Supp_Detail b
             on 
             b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.Seq2
             where a.id= @ID", pars, out da);
            if (!result) { this.ShowErr(result); }

            return Result.True;
        }

        public DataRow CurrentDataRow { get; set; }

        private void radioGroup1_ValueChanged(object sender, EventArgs e)
        {
            selectOption = this.radioGroup1.Value;


            this.ReportResourceNamespace = typeof(P50_PrintData);
            this.ReportResourceAssembly = ReportResourceNamespace.Assembly;
            this.ReportResourceName = selectOption == this.radioButton1.Value ? "P50BookQty_Print.rdlc" : "P50List_Print.rdlc";
            
        }
    }
}
