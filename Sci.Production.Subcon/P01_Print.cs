﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Subcon
{
    public partial class P01_Print : Sci.Win.Forms.Base
    {
        DataRow masterData;
        string GarTotal,TotalPoQty;        
        public P01_Print(DataRow mainData, string GRATOTAL, string numTotalPOQty)
        {
            InitializeComponent();
            this.radioByComb.Checked = true;
            masterData = mainData;
            GarTotal = GRATOTAL;
            TotalPoQty = numTotalPOQty;           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string id = masterData["ID"].ToString();
            string Issuedate = ((DateTime)MyUtility.Convert.GetDate(masterData["issuedate"])).ToShortDateString();
            string Delivery = ((DateTime)MyUtility.Convert.GetDate(masterData["Delivery"])).ToShortDateString();
            string Remark = masterData["Remark"].ToString();
            string TOTAL = masterData["amount"].ToString();
            string VAT = masterData["Vat"].ToString();
            string CurrencyID = masterData["CurrencyID"].ToString();
            string VatRate = masterData["VatRate"].ToString();
            string handle = masterData["handle"].ToString();
            string name = MyUtility.GetValue.Lookup("Name", masterData["handle"].ToString(), "Pass1", "ID");
            string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", masterData["artworktypeid"])).ToString().Trim();
            string orderID = MyUtility.GetValue.Lookup(
string.Format(
@"select  b.OrderID from ArtworkPO a
inner join Artworkpo_Detail b on a.ID=b.ID
where a.id='{0}' ", masterData["ID"].ToString()));

      


            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Delivery", Delivery));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", Issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TOTAL", TOTAL));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VAT", VAT));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("GRATOTAL", GarTotal));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CurrencyID", CurrencyID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VatRate", VatRate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("artworkunit", artworkunit));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("handle", handle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("name", name));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("orderID", orderID.Substring(0, 10)));
            #endregion

            this.ShowWaitMessage("Data is Printing...");

            // By Combinition
            if (this.radioByComb.Checked)
            {
                #region -- 撈表身資料 --
                pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@OrderID", orderID.Substring(0,10)+"%"));
                DataTable dtDetail;
                string sqlcmd = @"select             
            ART.id,F.nameEn,F.AddressEN,F.Tel,ART.LocalSuppID+'-'+L.name AS TITLETO,L.Tel,L.Address,L.fax,
            A.Orderid,O.styleID,A.poQty,A.artworkid,A.Stitch,A.Unitprice,A.Qtygarment,format(A.Amount,'#,###,###,##0.00')Amount
            ,a.PatternDesc
            from DBO.artworkpo ART WITH (NOLOCK) 
			LEFT JOIN dbo.factory F WITH (NOLOCK) 
			ON  F.ID = ART.factoryid
			LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) 
			ON  L.ID = ART.LocalSuppID
			LEFT JOIN dbo.Artworkpo_Detail A WITH (NOLOCK) 
			ON  A.ID = ART.ID
			LEFT JOIN dbo.Orders O WITH (NOLOCK) 
			ON  O.id = A.OrderID where a.OrderID like  @OrderID
            order by ID";
                result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
                if (!result) { this.ShowErr(sqlcmd, result); }
                if (MyUtility.Check.Empty(dtDetail)||dtDetail.Rows.Count<1)
                {
                    MyUtility.Msg.WarningBox("Data not found");
                    this.HideWaitMessage();
                    return; 
                }
                string Title1 = dtDetail.Rows[0]["nameEn"].ToString().Trim();
                string Title2 = dtDetail.Rows[0]["AddressEN"].ToString().Trim();
                string Title3 = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string TO = dtDetail.Rows[0]["TITLETO"].ToString().Trim();
                string TEL = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string ADDRESS = dtDetail.Rows[0]["Address"].ToString().Trim();
                string FAX = dtDetail.Rows[0]["fax"].ToString().Trim().Trim();
                string style = dtDetail.Rows[0]["styleID"].ToString().Trim();
                decimal totalQty = MyUtility.Convert.GetDecimal(dtDetail.Compute("sum(poqty)", "1=1"));
                //decimal totalQty = MyUtility.Convert.GetDecimal( MyUtility.GetValue.Lookup(
                //    string.Format(@"
                //    select sum(poqty) as TotalQty from(
                //    select distinct id,poqty from Artworkpo_Detail
                //    where OrderID like '{0}'
                //    ) a", orderID.Substring(0, 10) + "%")));
                decimal TotalAmount = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format(
                @"select convert(numeric(12,2),sum(Amount)) totalAmount  
                from Artworkpo_Detail
                where OrderID like '{0}'", orderID.Substring(0, 10) + "%")));
                decimal GrandTotal = TotalAmount + MyUtility.Convert.GetDecimal(masterData["Vat"]);
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title1", Title1));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title2", Title2));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title3", Title3));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TO", TO));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", TEL));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ADDRESS", ADDRESS));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FAX", FAX));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", totalQty.ToString()));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalAmount", TotalAmount.ToString()));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("GrandTotal", GrandTotal.ToString("#,0.00")));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("style", style));
                


                // 傳 list 資料            
                List<P01_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P01_PrintData()
                    {
                        OrderID = row1["Orderid"].ToString(),
                        StyleID = row1["styleID"].ToString(),
                        poQTY = row1["poQty"].ToString(),
                        ArtworkID = row1["artworkid"].ToString(),
                        PCS = row1["Stitch"].ToString(),
                        Unitprice = row1["Unitprice"].ToString(),
                        QtyGMT = row1["Qtygarment"].ToString(),
                        Amount = row1["Amount"].ToString(),
                        CutParts = row1["PatternDesc"].ToString(),
                        ID = row1["ID"].ToString()

                    }).ToList();

                report.ReportDataSource = data;
                #endregion


                Type ReportResourceNamespace = typeof(P01_PrintData);
                Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
                string ReportResourceName = "P01_Print_Comb.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
                {                    
                    return;
                }
                report.ReportResource = reportresource;
                var frm1 = new Sci.Win.Subs.ReportView(report);
                frm1.MdiParent = MdiParent;
                frm1.Show();
                this.HideWaitMessage();
            }
            else
            {
                #region -- 撈表身資料 --
                pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", id));
                DataTable dtDetail;
                string sqlcmd = @"select 
            F.nameEn,F.AddressEN,F.Tel,ART.LocalSuppID+'-'+L.name AS TITLETO,L.Tel,L.Address,L.fax,
            A.Orderid,O.styleID,A.poQty,A.artworkid,A.Stitch,A.Unitprice,A.Qtygarment,format(A.Amount,'#,###,###,##0.00')Amount
            from DBO.artworkpo ART WITH (NOLOCK) 
			LEFT JOIN dbo.factory F WITH (NOLOCK) 
			ON  F.ID = ART.factoryid
			LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) 
			ON  L.ID = ART.LocalSuppID
			LEFT JOIN dbo.Artworkpo_Detail A WITH (NOLOCK) 
			ON  A.ID = ART.ID
			LEFT JOIN dbo.Orders O WITH (NOLOCK) 
			ON  O.id = A.OrderID where ART.id= @ID";
                result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
                if (!result) { this.ShowErr(sqlcmd, result); }
                if (MyUtility.Check.Empty(dtDetail) || dtDetail.Rows.Count < 1)
                {
                    MyUtility.Msg.WarningBox("Data not found");
                    return;
                }
                string Title1 = dtDetail.Rows[0]["nameEn"].ToString().Trim();
                string Title2 = dtDetail.Rows[0]["AddressEN"].ToString().Trim();
                string Title3 = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string TO = dtDetail.Rows[0]["TITLETO"].ToString().Trim();
                string TEL = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string ADDRESS = dtDetail.Rows[0]["Address"].ToString().Trim();
                string FAX = dtDetail.Rows[0]["fax"].ToString().Trim();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title1", Title1));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title2", Title2));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title3", Title3));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TO", TO));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", TEL));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ADDRESS", ADDRESS));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FAX", FAX));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", TotalPoQty));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalAmount", TOTAL));


                // 傳 list 資料            
                List<P01_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P01_PrintData()
                    {
                        OrderID = row1["Orderid"].ToString(),
                        StyleID = row1["styleID"].ToString(),
                        poQTY = row1["poQty"].ToString(),
                        ArtworkID = row1["artworkid"].ToString(),
                        PCS = row1["Stitch"].ToString(),
                        Unitprice = row1["Unitprice"].ToString(),
                        QtyGMT = row1["Qtygarment"].ToString(),
                        Amount = row1["Amount"].ToString()
                    }).ToList();

                report.ReportDataSource = data;
                #endregion

                
                Type ReportResourceNamespace = typeof(P01_PrintData);
                Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
                string ReportResourceName = "P01_Print.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
                {                    
                    return ;
                }
                report.ReportResource = reportresource;
                var frm1 = new Sci.Win.Subs.ReportView(report);
                frm1.MdiParent = MdiParent;
                frm1.Show();
                this.HideWaitMessage();
            }
        }
    }
}
