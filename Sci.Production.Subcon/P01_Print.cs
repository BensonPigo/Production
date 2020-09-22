using System;
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
using Sci.Production.Class;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P01_Print
    /// </summary>
    public partial class P01_Print : Sci.Win.Forms.Base
    {
        private DataRow masterData;
        private string GarTotal;
        private string TotalPoQty;

        /// <summary>
        /// P01_Print
        /// </summary>
        /// <param name="mainData">mainData</param>
        /// <param name="gRATOTAL">GRATOTAL</param>
        /// <param name="numTotalPOQty">numTotalPOQty</param>
        public P01_Print(DataRow mainData, string gRATOTAL, string numTotalPOQty)
        {
            this.InitializeComponent();
            this.radioByComb.Checked = true;
            this.masterData = mainData;
            this.GarTotal = gRATOTAL;
            this.TotalPoQty = numTotalPOQty;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string id = this.masterData["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(this.masterData["issuedate"])).ToShortDateString();
            string delivery = ((DateTime)MyUtility.Convert.GetDate(this.masterData["Delivery"])).ToShortDateString();
            string remark = this.masterData["Remark"].ToString();
            decimal tOTAL = MyUtility.Convert.GetDecimal(this.masterData["amount"].ToString());
            string currencyID = this.masterData["CurrencyID"].ToString();
            string vatRate = this.masterData["VatRate"].ToString();
            decimal vAT = MyUtility.Convert.GetDecimal(this.masterData["VAT"]);
            string handle = this.masterData["handle"].ToString();
            string name = MyUtility.GetValue.Lookup("Name", this.masterData["handle"].ToString(), "Pass1", "ID");
            string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", this.masterData["artworktypeid"])).ToString().Trim();
            string orderID = MyUtility.GetValue.Lookup(
string.Format(
@"select  b.OrderID from ArtworkPO a
inner join Artworkpo_Detail b on a.ID=b.ID
where a.id='{0}' ", this.masterData["ID"].ToString()));

            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Delivery", delivery));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TOTAL", tOTAL.ToString("#,0.00")));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("GRATOTAL", this.GarTotal));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CurrencyID", currencyID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VatRate", vatRate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VAT", vAT.ToString("#,0.00")));
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
                pars.Add(new SqlParameter("@OrderID", orderID.Substring(0, 10) + "%"));
                DataTable dtDetail;
                string sqlcmd = string.Format(
                    @"
select ART.id
       , F.nameEn
       , F.AddressEN
       , F.Tel
       , ART.LocalSuppID+'-'+L.name AS TITLETO
       , L.Tel
       , L.Address
       , L.fax
       , A.Orderid
       , O.styleID
       , A.poQty
       , A.artworkid
       , A.Stitch
       , A.Unitprice
       , A.Qtygarment
       , Amount = format(A.Amount,'#,###,###,##0.00')
       , computeAmount = A.Amount
       , a.PatternDesc
       , ART.apvName
       , A.Article
       , A.SizeCode
from DBO.artworkpo ART WITH (NOLOCK) 
LEFT JOIN dbo.factory F WITH (NOLOCK) ON  F.ID = ART.factoryid
LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) ON  L.ID = ART.LocalSuppID
LEFT JOIN dbo.Artworkpo_Detail A WITH (NOLOCK) ON  A.ID = ART.ID
LEFT JOIN dbo.Orders O WITH (NOLOCK) ON  O.id = A.OrderID 
where a.OrderID like @OrderID
      and ART.LocalSuppID = '{0}'
order by ID", this.masterData["LocalSuppID"]);
                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dtDetail);
                if (!result)
                {
                    this.ShowErr(sqlcmd, result);
                }

                if (MyUtility.Check.Empty(dtDetail) || dtDetail.Rows.Count < 1)
                {
                    MyUtility.Msg.WarningBox("Data not found");
                    this.HideWaitMessage();
                    return;
                }

                string title1 = dtDetail.Rows[0]["nameEn"].ToString().Trim();
                string title2 = dtDetail.Rows[0]["AddressEN"].ToString().Trim();
                string title3 = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string tO = dtDetail.Rows[0]["TITLETO"].ToString().Trim();
                string tEL = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string aDDRESS = dtDetail.Rows[0]["Address"].ToString().Trim();
                string fAX = dtDetail.Rows[0]["fax"].ToString().Trim().Trim();
                string style = dtDetail.Rows[0]["styleID"].ToString().Trim();
                decimal totalQty = MyUtility.Convert.GetDecimal(dtDetail.Compute("sum(poqty)", "1=1"));
                decimal totalAmount = MyUtility.Convert.GetDecimal(dtDetail.Compute("sum(computeAmount)", string.Empty));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title1", title1));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title2", title2));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title3", title3));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TO", tO));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", tEL));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ADDRESS", aDDRESS));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FAX", fAX));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", totalQty.ToString()));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalAmount", totalAmount.ToString()));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("style", style));

                // 傳 list 資料
                List<P01_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P01_PrintData()
                    {
                        OrderID = row1["Orderid"].ToString(),
                        StyleID = row1["styleID"].ToString(),
                        PoQTY = row1["poQty"].ToString(),
                        ArtworkID = row1["artworkid"].ToString(),
                        PCS = row1["Stitch"].ToString(),
                        Unitprice = row1["Unitprice"].ToString(),
                        QtyGMT = row1["Qtygarment"].ToString(),
                        Amount = row1["Amount"].ToString(),
                        CutParts = row1["PatternDesc"].ToString(),
                        ID = row1["ID"].ToString(),
                        Article = row1["Article"].ToString(),
                        Size = row1["SizeCode"].ToString(),
                    }).ToList();

                data[0].ApvName = Production.Class.UserESignature.GetUserESignature(this.masterData["apvname"].ToString(), 207, 83);
                data[0].LockName = Production.Class.UserESignature.GetUserESignature(this.masterData["LockName"].ToString(), 207, 83);
                report.ReportDataSource = data;
                #endregion

                Type reportResourceNamespace = typeof(P01_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P01_Print_Comb.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
                {
                    return;
                }

                report.ReportResource = reportresource;
                var frm1 = new Sci.Win.Subs.ReportView(report);
                frm1.MdiParent = this.MdiParent;
                frm1.TopMost = true;
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
            A.Orderid,O.styleID,A.poQty,A.artworkid,A.Stitch,A.Unitprice,A.Qtygarment,format(A.Amount,'#,###,###,##0.00')Amount ,ART.apvName
            from DBO.artworkpo ART WITH (NOLOCK) 
			LEFT JOIN dbo.factory F WITH (NOLOCK) 
			ON  F.ID = ART.factoryid
			LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) 
			ON  L.ID = ART.LocalSuppID
			LEFT JOIN dbo.Artworkpo_Detail A WITH (NOLOCK) 
			ON  A.ID = ART.ID
			LEFT JOIN dbo.Orders O WITH (NOLOCK) 
			ON  O.id = A.OrderID where ART.id= @ID";
                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dtDetail);
                if (!result)
                {
                    this.ShowErr(sqlcmd, result);
                }

                if (MyUtility.Check.Empty(dtDetail) || dtDetail.Rows.Count < 1)
                {
                    MyUtility.Msg.WarningBox("Data not found");
                    return;
                }

                string title1 = dtDetail.Rows[0]["nameEn"].ToString().Trim();
                string title2 = dtDetail.Rows[0]["AddressEN"].ToString().Trim();
                string title3 = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string tO = dtDetail.Rows[0]["TITLETO"].ToString().Trim();
                string tEL = dtDetail.Rows[0]["Tel"].ToString().Trim();
                string aDDRESS = dtDetail.Rows[0]["Address"].ToString().Trim();
                string fAX = dtDetail.Rows[0]["fax"].ToString().Trim();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title1", title1));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title2", title2));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title3", title3));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TO", tO));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", tEL));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ADDRESS", aDDRESS));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FAX", fAX));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", this.TotalPoQty));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalAmount", tOTAL.ToString("#,0.00")));

                // 傳 list 資料
                List<P01_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P01_PrintData()
                    {
                        OrderID = row1["Orderid"].ToString(),
                        StyleID = row1["styleID"].ToString(),
                        PoQTY = row1["poQty"].ToString(),
                        ArtworkID = row1["artworkid"].ToString(),
                        PCS = row1["Stitch"].ToString(),
                        Unitprice = row1["Unitprice"].ToString(),
                        QtyGMT = row1["Qtygarment"].ToString(),
                        Amount = row1["Amount"].ToString(),
                    }).ToList();

                data[0].ApvName = Production.Class.UserESignature.GetUserESignature(this.masterData["apvname"].ToString(), 207, 83);
                data[0].LockName = Production.Class.UserESignature.GetUserESignature(this.masterData["LockName"].ToString(), 185, 83);
                report.ReportDataSource = data;
                #endregion

                Type reportResourceNamespace = typeof(P01_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P01_Print.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
                {
                    return;
                }

                report.ReportResource = reportresource;
                var frm1 = new Sci.Win.Subs.ReportView(report);
                frm1.MdiParent = this.MdiParent;
                frm1.TopMost = true;
                frm1.Show();
                this.HideWaitMessage();
            }
        }
    }
}
