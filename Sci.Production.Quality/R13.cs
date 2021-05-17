using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using Ict.Win;
using Microsoft.Reporting.WinForms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R13 : Win.Tems.PrintForm
    {
        private string wkNO;
        private string receivingID;
        private string arrDate;
        private string report_Type;
        private DataTable printData;

        /// <inheritdoc/>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.wkNO = this.txtWK.Text;
            this.receivingID = this.txtReceivingID.Text;
            this.arrDate = this.dateArrWH.Value.HasValue ? this.dateArrWH.Value.Value.ToString("yyyy/MM/dd") : string.Empty;

            if (MyUtility.Check.Empty(this.wkNO) && MyUtility.Check.Empty(this.receivingID) && MyUtility.Check.Empty(this.arrDate))
            {
                MyUtility.Msg.InfoBox("WK# , Receiving ID, Arrive W/H Date can't be all empty!!");
                return false;
            }

            if (this.radio4Slot.Checked)
            {
                this.report_Type = "4Slot";
            }
            else if (this.radio8Slot.Checked)
            {
                this.report_Type = "8Slot";
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult r = new DualResult(true);
            string cmd = string.Empty;

            if (this.report_Type == "4Slot")
            {
                cmd = $@"
SELECT 
    [Invo]=c.ExportID,
    a.ReceivingID,
    a.poid,
    [seq]= (a.seq1+'-'+a.seq2) ,
	o.BrandID
	,[Style]=o.StyleID
	,a.Refno
	,[Title] = ( select NameEN from Factory WITH (NOLOCK) where id='{Sci.Env.User.Factory}' )
	,ep.Eta
	,[FactoryID]='{Sci.Env.User.Factory}'
	,[Color]=cl.Name
	,[Supp] = a.Suppid + ' '+Supp.AbbEN
	,[Packages]=(	
		 select top 1 Packages
		 from
		 (
			 select e.Packages
			 from Receiving rr with (nolock) 
			 outer apply (
				select [Packages] = sum(ee.Packages)
				from Export ee with (nolock) 
					where ee.Blno in (
					select distinct e2.BLNO
					from Export e2 with (nolock) 
					where e2.ID = rr.ExportId
				 )
			 )e
			 where id = a.ReceivingID
			 union
			 select tt.Packages
			 from TransferIn tt with (nolock) 
			 where id = a.ReceivingID
		 )a
	)
FROM FIR a WITH (NOLOCK) 
Left join Receiving c WITH (NOLOCK) on c.ID = a.ReceivingID
Left join TransferIn ti WITH (NOLOCK) on ti.id = a.receivingid
inner join PO_Supp_Detail d WITH (NOLOCK) on d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
LEFT JOIN Supp ON Supp.ID = a.SuppID
LEFT JOIN Orders o ON o.ID = a.POID
LEFT JOIN PO_Supp p ON p.ID = a.POID AND p.SEQ1 = a.SEQ1
LEFT JOIN Export ep ON ep.ID = c.ExportId
LEFT JOIN Color cl ON d.ColorID = cl.ID AND cl.BrandId = o.BrandID
WHERE 1=1
";
                if (!MyUtility.Check.Empty(this.wkNO))
                {
                    cmd += $@"AND c.ExportID='{this.wkNO}'";
                }

                if (!MyUtility.Check.Empty(this.arrDate))
                {
                    cmd += $@"AND (c.WhseArrival = '{this.arrDate}' OR ti.IssueDate = '{this.arrDate}')";
                }

                if (!MyUtility.Check.Empty(this.receivingID))
                {
                    cmd += $@"AND a.ReceivingID='{this.receivingID}'";
                }

                r = DBProxy.Current.Select(string.Empty, cmd, out this.printData);
                if (!r)
                {
                    this.ShowErr(r);
                    return r;
                }
            }

            if (this.report_Type == "8Slot")
            {
                cmd = $@"
SELECT 
    [Invo]=c.ExportID,
    a.ReceivingID,
    a.poid,
    [seq]= (a.seq1+'-'+a.seq2) ,
	o.BrandID
	,[Style]=o.StyleID
	,a.Refno
	,[Title] = ( select NameEN from Factory WITH (NOLOCK) where id='{Sci.Env.User.Factory}' )
	,ep.Eta
	,[FactoryID]='{Sci.Env.User.Factory}'
	,[Color]=cl.Name
	,[Supp] = a.Suppid + ' '+Supp.AbbEN
	,[Packages]=(	
		 select top 1 Packages
		 from
		 (
			 select e.Packages
			 from Receiving rr with (nolock) 
			 outer apply (
				select [Packages] = sum(ee.Packages)
				from Export ee with (nolock) 
					where ee.Blno in (
					select distinct e2.BLNO
					from Export e2 with (nolock) 
					where e2.ID = rr.ExportId
				 )
			 )e
			 where id = a.ReceivingID
			 union
			 select tt.Packages
			 from TransferIn tt with (nolock) 
			 where id = a.ReceivingID
		 )a
	)
	,a.ID
INTO #tmp
FROM FIR a WITH (NOLOCK) 
Left join Receiving c WITH (NOLOCK) on c.ID = a.ReceivingID
Left join TransferIn ti WITH (NOLOCK) on ti.id = a.receivingid
inner join PO_Supp_Detail d WITH (NOLOCK) on d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
LEFT JOIN Supp ON Supp.ID = a.SuppID
LEFT JOIN Orders o ON o.ID = a.POID
LEFT JOIN PO_Supp p ON p.ID = a.POID AND p.SEQ1 = a.SEQ1
LEFT JOIN Export ep ON ep.ID = c.ExportId
LEFT JOIN Color cl ON d.ColorID = cl.ID AND cl.BrandId = o.BrandID
WHERE 1=1
";

                if (!MyUtility.Check.Empty(this.wkNO))
                {
                    cmd += $@"AND c.ExportID='{this.wkNO}'";
                }

                if (!MyUtility.Check.Empty(this.arrDate))
                {
                    cmd += $@"AND (c.WhseArrival = '{this.arrDate}' OR ti.IssueDate = '{this.arrDate}')";
                }

                if (!MyUtility.Check.Empty(this.receivingID))
                {
                    cmd += $@"AND a.ReceivingID='{this.receivingID}'";
                }

                cmd += @"
SELECT t.*
	,f.Dyelot 
	,f.Roll
	,f.Ticketyds
	,[RowNumber]=RANK()Over (Partition by t.ReceivingID,t.POID,t.seq, f.Dyelot order by f.Roll,f.Ticketyds)  
FROM #tmp t
LEFT JOIN FIR_Shadebone f On t.ID = f.ID

DROP TABLE #tmp
";
                r = DBProxy.Current.Select(null, cmd, out this.printData);
                if (!r)
                {
                    this.ShowErr(r);
                    return r;
                }
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToPrint(ReportDefinition report)
        {
            if (this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            DualResult result;

            if (this.report_Type == "4Slot")
            {
                #region -- 表頭資料 --

                string title = MyUtility.Convert.GetString(this.printData.Rows[0]["Title"]);
                report.ReportParameters.Add(new ReportParameter("Title", title));

                #endregion

                #region -- 表身資料 --
                List<R13_ShadeBond4_Data> data = this.printData.AsEnumerable()
                    .Select(row1 => new R13_ShadeBond4_Data()
                    {
                        POID = row1["Poid"].ToString().Trim(),
                        FactoryID = row1["FactoryID"].ToString().Trim(),
                        Style = row1["Style"].ToString().Trim(),
                        Color = row1["Color"].ToString().Trim(),
                        BrandID = row1["BrandID"].ToString().Trim(),
                        Supp = row1["Supp"].ToString().Trim(),
                        Invo = row1["Invo"].ToString().Trim(),
                        ETA = row1["ETA"].ToString() == string.Empty ? string.Empty : DateTime.Parse(row1["ETA"].ToString()).ToString("yyyy-MM-dd").ToString().Trim(),
                        Refno = row1["Refno"].ToString().Trim(),
                        Packages = row1["Packages"].ToString().Trim(),
                        Seq = row1["Seq"].ToString().Trim(),
                        ReceivingID = row1["ReceivingID"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;
                #endregion

                Type reportResourceNamespace = typeof(R13_ShadeBond4_Data);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;

                string reportResourceName = "R13_ShadeBond4.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
                {
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report);
                frm.MdiParent = this.MdiParent;
                frm.TopMost = true;
                frm.Show();
            }

            if (this.report_Type == "8Slot")
            {

                #region -- 表頭資料 --

                string title = MyUtility.Convert.GetString(this.printData.Rows[0]["Title"]);
                report.ReportParameters.Add(new ReportParameter("Title", title));

                #endregion

                #region -- 表身資料 --
                List<R13_ShadeBond8_Data> data = this.printData.AsEnumerable()
                    .Select(row1 => new R13_ShadeBond8_Data()
                    {
                        POID = row1["Poid"].ToString().Trim(),
                        FactoryID = row1["FactoryID"].ToString().Trim(),
                        Style = row1["Style"].ToString().Trim(),
                        Color = row1["Color"].ToString().Trim(),
                        BrandID = row1["BrandID"].ToString().Trim(),
                        Supp = row1["Supp"].ToString().Trim(),
                        Invo = row1["Invo"].ToString().Trim(),
                        ETA = row1["ETA"].ToString() == string.Empty ? string.Empty : DateTime.Parse(row1["ETA"].ToString()).ToString("yyyy-MM-dd").ToString().Trim(),
                        Refno = row1["Refno"].ToString().Trim(),
                        Packages = row1["Packages"].ToString().Trim(),
                        Seq = row1["Seq"].ToString().Trim(),
                        ReceivingID = row1["ReceivingID"].ToString().Trim(),
                        Dyelot = row1["Dyelot"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        TicketYds = row1["TicketYds"].ToString().Trim(),
                        RowNumber = row1["RowNumber"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;

                #endregion

                Type reportResourceNamespace = typeof(R13_ShadeBond8_Data);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;

                string reportResourceName = "R13_ShadeBond8.rdlc";
                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();
            }

            return true;
        }
    }
}
