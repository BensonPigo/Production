using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P18_Print : Win.Tems.PrintForm
    {
        private DataRow mainCurrentMaintain;
        private DataTable dtResult;

        /// <inheritdoc/>
        public P18_Print(DataRow drMain)
        {
            /// <inheritdoc/>
            this.InitializeComponent();
            this.mainCurrentMaintain = drMain;
            this.radioPanel.Value = "1";
        }

        private void RadioPanel_ValueChanged(object sender, EventArgs e)
        {
            if (this.radioPanel.Value == string.Empty)
            {
                this.radioPanel.Value = "1";
            }

            switch (this.radioPanel.Value)
            {
                case "1":
                    this.IsSupportToPrint = true;
                    this.IsSupportToExcel = false;
                    break;
                case "2":
                    this.IsSupportToPrint = false;
                    this.IsSupportToExcel = true;
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            // saveDialog.ShowDialog();
            // outpa = saveDialog.FileName;
            // if (outpa.Empty())
            // {

            // return false;
            // }
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DataRow row = this.mainCurrentMaintain;
            string id = row["ID"].ToString();
            string fromFactory = row["FromFtyID"].ToString();
            string remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToShortDateString();

            if (this.radioPanel.Value == "1")
            {
                #region -- 撈表頭資料 --
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", id));
                DataTable dt;
                string cmdd = @"
select  b.name 
from dbo.Transferin  a WITH (NOLOCK) 
inner join dbo.mdivision  b WITH (NOLOCK) on b.id = a.mdivisionid
where   b.id = a.mdivisionid
        and a.id = @ID";
                DualResult result = DBProxy.Current.Select(string.Empty, cmdd, pars, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found!!!");
                }

                // 抓M的EN NAME
                DataTable dtNAME;
                DBProxy.Current.Select(
                    string.Empty,
                    string.Format(@"select NameEN from MDivision where ID='{0}'", Env.User.Keyword), out dtNAME);
                string rptTitle = dtNAME.Rows[0]["NameEN"].ToString();
                #endregion

                #region -- 撈表身資料 --
                DataTable dtDetail;
                string tmp = @"
select  a.POID
        , a.Seq1 + '-' + a.seq2 as SEQ
        , a.Roll
        , a.Dyelot 
	    , [Description] = IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
			                   AND (b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
			                   AND (b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
			                  , ''
                              , dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))
        , StockUnit = dbo.GetStockUnitBySpSeq (a.poid, a.seq1, a.seq2)
	    , a.Qty
        , a.Weight
        , dbo.Getlocation(f.ukey)[Location] 
from dbo.TransferIn_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.POID 
                                                and b.SEQ1 = a.Seq1 
                                                and b.SEQ2=a.seq2
inner join FtyInventory f WITH (NOLOCK) on  f.POID = a.poid
		                                    And f.Seq1 = a.seq1
		                                    And f.Seq2 = a.seq2
		                                    And f.Roll =  a.roll
		                                    And f.Dyelot = a.dyelot
		                                    And f.StockType = a.stocktype
where a.id = @ID";
                result = DBProxy.Current.Select(string.Empty, tmp, pars, out dtDetail);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dtDetail == null || dtDetail.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found!!!");
                }

                this.dtResult = dtDetail;

                #endregion
            }
            else
            {
                string cmd = $@"

select  a.Roll
		, a.Dyelot
		, a.POID
        , a.Seq1 + '-' + a.seq2 as SEQ
		, b.Refno
		, Color = dbo.GetColorMultipleID(b.BrandID, b.ColorID)
		, ColorName = c.Name
		, f.WeaveTypeID
		, o.BrandID
	    , [Description] = IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
			                   AND (b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
			                   AND (b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
			                  , ''
                              , dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))

		, a.Weight
		, a.ActualWeight
		, a.Qty
		, StockType =  CASE WHEN a.StockType = 'B' THEN 'Bulk'
                            WHEN a.StockType = 'I' THEN 'Inventory'
                            ELSE a.StockType 
                        END
		, a.Location
		, a.Remark
from dbo.TransferIn_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.POID 
                                                and b.SEQ1 = a.Seq1 
                                                and b.SEQ2=a.seq2
left join Color c on c.ID = b.ColorID AND c.BrandId = b.BrandId
LEFT JOIN Fabric f WITH (NOLOCK) ON b.SCIRefNo=f.SCIRefNo
left join Orders o on o.ID = a.POID
WHERE a.ID = '{id}'
";

                DualResult result = DBProxy.Current.Select(null, cmd, out this.dtResult);

                if (!result)
                {
                    this.ShowErr(result);
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool OnToPrint(ReportDefinition report)
        {
            this.SetCount(this.dtResult.Rows.Count);

            DualResult result;
            DataRow row = this.mainCurrentMaintain;
            string id = row["ID"].ToString();
            string fromFactory = row["FromFtyID"].ToString();
            string remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToShortDateString();

            #region -- 整理表頭資料 --

            // 抓M的EN NAME
            DataTable dtNAME;
            DBProxy.Current.Select(
                string.Empty,
                string.Format(@"select NameEN from MDivision where ID='{0}'", Env.User.Keyword), out dtNAME);
            string rptTitle = dtNAME.Rows[0]["NameEN"].ToString();

            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("FromFtyID", fromFactory));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("IssueDate", issuedate));

            #endregion

            #region -- 整理表身資料 --

            // 傳 list 資料
            List<P18_PrintData> data = this.dtResult.AsEnumerable()
                .Select(row1 => new P18_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    DYELOT = row1["DYELOT"].ToString().Trim(),
                    DESC = row1["Description"].ToString().Trim(),
                    Unit = row1["StockUnit"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    GW = row1["Weight"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P18_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P18_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report)
            {
                MdiParent = this.MdiParent,
            };
            frm.Show();

            return true;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtResult.Rows.Count); // 顯示筆數

            if (this.dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_P18_Print.xltx", objApp);
            com.UseInnerFormating = false;
            com.WriteTable(this.dtResult, 2);

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
