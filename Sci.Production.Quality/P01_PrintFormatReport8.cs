using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_PrintFormatReport8 : Win.Tems.QueryForm
    {
        private readonly DataRow maindr;
        private readonly string suppid;
        private readonly string colorid;
        private readonly string styleid;

        /// <inheritdoc/>
        public P01_PrintFormatReport8(DataRow maindr, string suppid, string colorid, string styleid)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.maindr = maindr;
            this.suppid = suppid;
            this.colorid = colorid;
            this.styleid = styleid;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(7), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            DualResult result = DBProxy.Current.Select(null, $"select Selected = cast(0 as bit),Roll,Dyelot,Ticketyds from FIR_Shadebone where id={this.maindr["ID"]}", out DataTable dtFIR_Shadebone);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dtFIR_Shadebone;
        }

        /// <inheritdoc/>
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();

            // 抓Invo,ETA 資料
            string cmd = $"select id,Eta from Export where id='{this.maindr["Exportid"]}' ";
            DualResult result = DBProxy.Current.Select(string.Empty, cmd, out DataTable dt_Exp);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string title = MyUtility.GetValue.Lookup($"select NameEN from Factory WITH (NOLOCK) where id='{Env.User.Factory}'");
            string invno = dt_Exp.Rows.Count == 0 ? string.Empty : dt_Exp.Rows[0]["ID"].ToString();
            string brandID = MyUtility.GetValue.Lookup($"SELECT BrandID FROM Orders WHERE ID = '{this.maindr["POID"]}'");

            int packages = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($@"
 select top 1 Packages
 from
 (
	 select e.Packages
	 from Receiving r with (nolock) 
	 outer apply (
		select [Packages] = sum(e.Packages)
		from Export e with (nolock) 
			where e.Blno in (
			select distinct e2.BLNO
			from Export e2 with (nolock) 
			where e2.ID = r.ExportId
		 )
	 )e
	 where id = '{this.maindr["ReceivingID"]}'
	 union
	 select t.Packages
	 from TransferIn t with (nolock) 
	 where id = '{this.maindr["ReceivingID"]}'
 )a
"));
            string colorName = MyUtility.GetValue.Lookup($"SELECT Name FROM Color WHERE ID = '{this.colorid}' AND BrandId  ='{brandID}' ");

            ReportDefinition report = new ReportDefinition();

            // 設定RDLC參數
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title", title));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Poid", this.maindr["POID"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FactoryID", Env.User.Factory));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Style", this.styleid));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Color", colorName));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("BrandID", brandID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supp", this.suppid));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invo", invno));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ETA", dt_Exp.Rows.Count == 0 ? string.Empty : DateTime.Parse(dt_Exp.Rows[0]["ETA"].ToString()).ToString("yyyy-MM-dd").ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Refno", MyUtility.GetValue.Lookup($"SELECT Refno FROM PO_Supp_Detail WHERE ID='{this.maindr["POID"]}' AND Seq1='{this.maindr["Seq1"]}' AND Seq2='{this.maindr["Seq2"]}'")));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Packages", packages.ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Seq", $"{this.maindr["Seq1"]} - {this.maindr["Seq2"]}"));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Dyelot", " "));

            #region 表身資料
            DataRow[] drs = ((DataTable)this.listControlBindingSource1.DataSource).Select($"Selected = 1");
            DataTable dtFIR_Shadebone = ((DataTable)this.listControlBindingSource1.DataSource).Clone();
            if (drs.Length == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    dtFIR_Shadebone.Rows.Add(dtFIR_Shadebone.NewRow());
                }
            }
            else
            {
                dtFIR_Shadebone = drs.CopyToDataTable();
            }

            List<string> dyelotCTn = dtFIR_Shadebone.AsEnumerable().Select(o => o["Dyelot"].ToString()).Distinct().ToList();
            List<P01_ShadeBond_Data> data = new List<P01_ShadeBond_Data>();

            foreach (var dyelot in dyelotCTn)
            {
                List<DataRow> sameDyelot = dtFIR_Shadebone.AsEnumerable().Where(o => o["Dyelot"].ToString() == dyelot).ToList();

                foreach (var sameData in sameDyelot)
                {
                    P01_ShadeBond_Data r = new P01_ShadeBond_Data
                    {
                        Dyelot = MyUtility.Convert.GetString(sameData["Dyelot"]),
                        Roll = MyUtility.Convert.GetString(sameData["Roll"]),
                        TicketYds = MyUtility.Convert.GetString(sameData["TicketYds"]),
                    };
                    data.Add(r);
                }
            }

            report.ReportDataSource = data;
            #endregion

            Type reportResourceNamespace = typeof(P01_ShadeBond_Data);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;

            string reportResourceName = "P01_ShadeBond_Print_8.rdlc";
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
            {
                return;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report)
            {
                MdiParent = this.MdiParent,
            };
            frm.Show();
        }
    }
}
