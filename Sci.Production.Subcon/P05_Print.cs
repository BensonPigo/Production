using System;
using System.Collections.Generic;
using System.Data;

using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P05_Print
    /// </summary>
    public partial class P05_Print : Win.Forms.Base
    {
        private DataRow masterData;
        private string TotalReqQty;

        /// <summary>
        /// P05_Print
        /// </summary>
        /// <param name="mainData">mainData</param>
        /// <param name="numTotalReqQty">numTotalReqQty</param>
        public P05_Print(DataRow mainData, string numTotalReqQty)
        {
            this.InitializeComponent();
            this.radioByComb.Checked = true;
            this.masterData = mainData;
            this.TotalReqQty = numTotalReqQty;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string id = this.masterData["ID"].ToString();
            string reqdate = ((DateTime)MyUtility.Convert.GetDate(this.masterData["Reqdate"])).ToShortDateString();
            string remark = this.masterData["Remark"].ToString();
            string handle = MyUtility.GetValue.Lookup("Name", this.masterData["handle"].ToString(), "Pass1", "ID");
            string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", this.masterData["artworktypeid"])).ToString().Trim();
            string orderID = MyUtility.GetValue.Lookup(
string.Format(
@"select  b.OrderID from ArtworkReq a
inner join ArtworkReq_Detail b on a.ID=b.ID
where a.id='{0}' ", this.masterData["ID"].ToString()));

            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Reqdate", reqdate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("artworkunit", artworkunit));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("handle", handle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("orderID", orderID.Substring(0, 10)));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", this.TotalReqQty));
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
       , A.Article
       , A.SizeCode
       , A.ReqQty
       , A.artworkid	
       , A.Stitch
       , A.Qtygarment
       , a.PatternDesc
       , [MgApvName]=(SELECT NAME 
                    FROM   pass1 
                    WHERE  id = ART.mgapvname)
       , [DeptApvName]=(SELECT NAME 
                      FROM   pass1 
                      WHERE  id = ART.deptapvname) 
from DBO.ArtworkReq ART WITH (NOLOCK) 
LEFT JOIN dbo.factory F WITH (NOLOCK) ON  F.ID = ART.factoryid
LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) ON  L.ID = ART.LocalSuppID
LEFT JOIN dbo.ArtworkReq_Detail A WITH (NOLOCK) ON  A.ID = ART.ID
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
                decimal totalQty = MyUtility.Convert.GetDecimal(dtDetail.Compute("sum(Reqqty)", "1=1"));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title1", title1));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title2", title2));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title3", title3));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TO", tO));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", tEL));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ADDRESS", aDDRESS));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FAX", fAX));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", totalQty.ToString()));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("style", style));

                // 傳 list 資料
                List<P05_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P05_PrintData()
                    {
                        OrderID = row1["Orderid"].ToString(),
                        StyleID = row1["styleID"].ToString(),
                        Article = row1["Article"].ToString(),
                        Size = row1["SizeCode"].ToString(),
                        ReqQTY = row1["ReqQty"].ToString(),
                        ArtworkID = row1["artworkid"].ToString(),
                        PCS = row1["Stitch"].ToString(),
                        QtyGMT = row1["Qtygarment"].ToString(),
                        CutParts = row1["PatternDesc"].ToString(),
                        ID = row1["ID"].ToString(),
                    }).ToList();

                data[0].MgApvName = Class.UserESignature.GetUserESignature(this.masterData["MgApvName"].ToString(), 207, 83);
                data[0].DeptApvName = Class.UserESignature.GetUserESignature(this.masterData["DeptApvName"].ToString(), 207, 83);
                report.ReportDataSource = data;
                #endregion

                Type reportResourceNamespace = typeof(P05_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P05_Print_Comb.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
                {
                    return;
                }

                report.ReportResource = reportresource;
                var frm1 = new Win.Subs.ReportView(report);
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
                string sqlcmd = @"
 SELECT F.nameen, 
       F.addressen, 
       F.tel, 
       ART.localsuppid + '-' + L.NAME AS TITLETO, 
       L.tel, 
       L.address, 
       L.fax, 
       A.orderid, 
       O.styleid, 
       A.reqqty, 
       A.artworkid, 
       A.stitch, 
       A.qtygarment, 
       [MgApvName]=(SELECT NAME 
                    FROM   pass1 
                    WHERE  id = ART.mgapvname), 
       [DeptApvName]=(SELECT NAME 
                      FROM   pass1 
                      WHERE  id = ART.deptapvname) 
FROM   dbo.artworkreq ART WITH (nolock) 
       LEFT JOIN dbo.factory F WITH (nolock) 
              ON F.id = ART.factoryid 
       LEFT JOIN dbo.localsupp L WITH (nolock) 
              ON L.id = ART.localsuppid 
       LEFT JOIN dbo.artworkreq_detail A WITH (nolock) 
              ON A.id = ART.id 
       LEFT JOIN dbo.orders O WITH (nolock) 
              ON O.id = A.orderid 
WHERE  ART.id = @ID            
";
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

                // 傳 list 資料
                List<P05_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P05_PrintData()
                    {
                        OrderID = row1["Orderid"].ToString(),
                        StyleID = row1["styleID"].ToString(),
                        ReqQTY = row1["ReqQty"].ToString(),
                        ArtworkID = row1["artworkid"].ToString(),
                        PCS = row1["Stitch"].ToString(),
                        QtyGMT = row1["Qtygarment"].ToString(),
                    }).ToList();

                data[0].MgApvName = Class.UserESignature.GetUserESignature(this.masterData["MgApvName"].ToString(), 207, 83);
                data[0].DeptApvName = Class.UserESignature.GetUserESignature(this.masterData["DeptApvName"].ToString(), 185, 83);
                report.ReportDataSource = data;
                #endregion

                Type reportResourceNamespace = typeof(P05_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P05_Print.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
                {
                    return;
                }

                report.ReportResource = reportresource;
                var frm1 = new Win.Subs.ReportView(report);
                frm1.MdiParent = this.MdiParent;
                frm1.TopMost = true;
                frm1.Show();
                this.HideWaitMessage();
            }
        }
    }
}
