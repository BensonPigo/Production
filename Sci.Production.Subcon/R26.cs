using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R26 : Sci.Win.Tems.PrintForm
    {
        public R26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }
     
//        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
//        {
//            DataRow row = this.CurrentDataRow;
//            string id = row["ID"].ToString();
//            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

//            #region  抓表頭資料
//            List<SqlParameter> pars = new List<SqlParameter>();
//            pars.Add(new SqlParameter("@ID", id));
//            DataTable dt;
//            DualResult result = DBProxy.Current.Select("",
//            @"select b.nameEn 
//			        ,b.AddressEN
//			        ,b.Tel
//			        ,a.Id
//                    ,c.name
//			        ,c.Address
//			        ,c.Tel
//					,a.PaytermID+d.Name [Terms]
//					,a.InvNo [Invoice]
//					,a.Remark [Remark]
//					,e.AccountNo [AC_No]
//                    ,e.AccountName [AC_Name]
//                    ,e.BankName [Bank_Name]
//                    ,e.CountryID [Country]
//                    ,e.city [city] 
//                    ,e.swiftcode [SwiftCode]
//					,cast(cast(isnull(round(a.amount,cr.Exact) , 0 ) as float) as varchar) [Total]	
//					,cast(cast(isnull(round(a.Vat,cr.Exact) , 0 ) as float) as varchar) [Vat]				
//                    ,cast(cast(isnull(round(a.amount,cr.Exact)+round(a.Vat,cr.Exact) , 0 ) as float) as varchar) [Grand_Total]	
//                    ,a.Handle+f.Name [Prepared_by]
//                    ,a.CurrencyID[CurrencyID]
//					,a.VatRate[VatRate]
//            from dbo.LocalAP a 
//            left join dbo.factory  b on b.id = a.factoryid
//			inner join dbo.LocalSupp c on c.id=a.LocalSuppID
//			left join dbo.PayTerm d on d.id=a.PaytermID
//			left join dbo.LocalSupp_Bank e on e.IsDefault=1 and e.id=a.LocalSuppID
//			left join dbo.Pass1 f on f.id=a.Handle
//            left join dbo.Currency cr on cr.ID = a.CurrencyID
//            where a.id = @ID", pars, out dt);
//            if (!result) { this.ShowErr(result); }
//            string RptTitle = dt.Rows[0]["nameEn"].ToString();
//            string address = dt.Rows[0]["AddressEN"].ToString();
//            string Tel = dt.Rows[0]["Tel"].ToString();
//            string Barcode = dt.Rows[0]["Id"].ToString();
//            string Supplier = dt.Rows[0]["name"].ToString();
//            string Address1 = dt.Rows[0]["Address"].ToString();
//            string TEL1 = dt.Rows[0]["Tel"].ToString();
//            string Terms = dt.Rows[0]["Terms"].ToString();
//            string Invoice = dt.Rows[0]["Invoice"].ToString();
//            string Remark = dt.Rows[0]["Remark"].ToString();
//            string AC_No = dt.Rows[0]["AC_No"].ToString();
//            string AC_Name = dt.Rows[0]["AC_Name"].ToString();
//            string Bank_Name = dt.Rows[0]["Bank_Name"].ToString();
//            string Country = dt.Rows[0]["Country"].ToString();
//            string city = dt.Rows[0]["city"].ToString();
//            string SwiftCode = dt.Rows[0]["SwiftCode"].ToString();
//            string Total = dt.Rows[0]["Total"].ToString();
//            string Vat = dt.Rows[0]["Vat"].ToString();
//            string Grand_Total = dt.Rows[0]["Grand_Total"].ToString();
//            string Prepared_by = dt.Rows[0]["Prepared_by"].ToString();
//            string CurrencyID = dt.Rows[0]["CurrencyID"].ToString();
//            string VatRate = dt.Rows[0]["VatRate"].ToString();
//            ReportDefinition report = new ReportDefinition();
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("address", address));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", Tel));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", Barcode));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address1", Address1));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL1", TEL1));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("id", id));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Terms", Terms));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", Invoice));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AC_No", AC_No));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AC_Name", AC_Name));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Bank_Name", Bank_Name));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Country", Country));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("city", city));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SwiftCode", SwiftCode));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total", Total));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Vat", Vat));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Grand_Total", Grand_Total));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Prepared_by", Prepared_by));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CurrencyID", CurrencyID));
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VatRate", VatRate));
//            #endregion


//            #region  抓表身資料
//            pars = new List<SqlParameter>();
//            pars.Add(new SqlParameter("@ID", id));
//            DataTable dd;
//            result = DBProxy.Current.Select("",
//            @"select a.OrderId [SP]
//                    ,[Description]=dbo.getItemDesc(b.Category,a.Refno)
//                    ,a.price [Price]
//                    ,a.qty [Qty]
//                    ,a.unitid [Unit]
//                    ,a.price*a.Qty [Amount]
//            from dbo.LocalAP_Detail a
//            left join dbo.LocalAP b on a.id=b.Id
//            where a.id= @ID", pars, out dd);
//            if (!result) { this.ShowErr(result); }

//            // 傳 list 資料            
//            List<P35_PrintData> data = dd.AsEnumerable()
//                .Select(row1 => new P35_PrintData()
//                {
//                    SP = row1["SP"].ToString(),
//                    Description = row1["Description"].ToString(),
//                    Price = row1["Price"].ToString(),
//                    Qty = row1["Qty"].ToString(),
//                    Unit = row1["Unit"].ToString(),
//                    Amount = row1["Amount"].ToString()
//                }).ToList();

//            report.ReportDataSource = data;
//            #endregion

//            // 指定是哪個 RDLC
//            #region  指定是哪個 RDLC
//            //DualResult result;
//            Type ReportResourceNamespace = typeof(R26_PrintData);
//            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
//            string ReportResourceName = "R26_Print.rdlc";

//            IReportResource reportresource;
//            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
//            {
//                ////this.ShowException(result);
//                //return;
//            }

//            report.ReportResource = reportresource;
//            #endregion

//            // 開啟 report view
//            var frm = new Sci.Win.Subs.ReportView(report);
//            frm.MdiParent = MdiParent;
//            frm.Show();

//            //return ;
//            return base.OnAsyncDataLoad(e);
//        }
        //按件觸發
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            return base.OnToExcel(report);
        }
    }
}
