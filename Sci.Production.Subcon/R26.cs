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

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {

            return base.OnAsyncDataLoad(e);
        }
        //按件觸發
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            return base.OnToExcel(report);
        }
        
        private void print_Click(object sender, EventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            
            #region  
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select b.NameEN [Title1] 
                    ,b.AddressEN [Title2]
                    ,b.Tel [Title3]
                    ,a.LocalSuppID [To]
                    ,d.Tel [Tel]
                    ,d.Fax [Fax]
                    ,a.IssueDate [Issue_Date]
                    ,c.OrderId [PO]
                    ,c.Refno [Code]
                    ,c.ThreadColorID [Color_Shade]
                    ,[Description]=dbo.getItemDesc(a.Category,c.Refno)
                    ,c.Qty [Quantity]
                    ,c.UnitId [Unit]
                    ,c.Price[Unit_Price]
                    ,c.Qty*c.Price [Amount]
                    ,[Total_Quantity]=sum(c.Qty) OVER (PARTITION BY c.OrderId,c.Refno) 
                    ,a.Remark [Remark] 
                    ,a.CurrencyId [Total1] 
                    ,a.Amount [Total2]
                    ,a.CurrencyId [currencyid]
                    ,a.Vat [vat]
                    ,a.Amount+a.Vat [Grand_Total]     
	          from dbo.localpo a 
              left join dbo.Factory  b  on b.id = a.factoryid 
	          left join dbo.LocalPO_Detail c on a.id=c.Id
              left join dbo.LocalSupp d on a.LocalSuppID=d.ID
              where a.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string Title1 = dt.Rows[0]["NameEN"].ToString();
            string Title2 = dt.Rows[0]["AddressEN"].ToString();
            string Title3 = dt.Rows[0]["Tel"].ToString();
            string To = dt.Rows[0]["LocalSuppID"].ToString();
            string Tel = dt.Rows[0]["Tel"].ToString();
            string Fax = dt.Rows[0]["Fax"].ToString();
            string Issue_Date = dt.Rows[0]["Issue_Date"].ToString();
            string Total1 = dt.Rows[0]["CurrencyId"].ToString();
            string Total2 = dt.Rows[0]["Amount"].ToString();
            string CurrencyId = dt.Rows[0]["currencyid"].ToString();
            string vat = dt.Rows[0]["vat"].ToString();
            string Grand_Total = dt.Rows[0]["Grand_Total"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title1", Title1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title2", Title2));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title3", Title3));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("To", To));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", Tel));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Fax", Fax));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issue_Date", Issue_Date));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total1", Total1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total2", Total2));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CurrencyId", CurrencyId));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vat", vat));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Grand_Total", Grand_Total));
            

            // 傳 list 資料            
            List<R26_PrintData> data = dt.AsEnumerable()
                .Select(row1 => new R26_PrintData()
                {
                    PO = row1["PO"].ToString(),
                    Code = row1["Code"].ToString(),
                    Color_Shade = row1["Color_Shade"].ToString(),
                    Description = row1["Description"].ToString(),
                    Quantity = row1["Quantity"].ToString(),
                    Unit = row1["Unit"].ToString(),
                    Unit_Price = row1["Unit_Price"].ToString(),
                    Amount = row1["Amount"].ToString(),
                    Total_Quantity = row1["Total_Quantity"].ToString(),
                    Remark = row1["Remark"].ToString()
                })ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(R26_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "R26_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                ////this.ShowException(result);
                //return;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            //return ;
        }

    }
}
