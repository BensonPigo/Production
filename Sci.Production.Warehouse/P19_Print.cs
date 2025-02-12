using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P19_Print : Win.Tems.PrintForm
    {
        private DataRow mainCurrentMaintain;
        private DataTable dtResult;

        /// <inheritdoc/>
        public P19_Print(DataRow drMain)
        {
            this.InitializeComponent();
            this.PrintButtonStatusChange();
            this.mainCurrentMaintain = drMain;
            DataTable dtPMS_FabricQRCode_LabelSize;
            DualResult result = DBProxy.Current.Select(null, "select ID, Name from dropdownlist where Type = 'PMS_Fab_LabSize' order by Seq", out dtPMS_FabricQRCode_LabelSize);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.comboType.DisplayMember = "Name";
            this.comboType.ValueMember = "ID";
            this.comboType.DataSource = dtPMS_FabricQRCode_LabelSize;

            this.comboType.SelectedValue = MyUtility.GetValue.Lookup("select PMS_FabricQRCode_LabelSize from system");
        }

        private void RadioTransferOutReport_CheckedChanged(object sender, EventArgs e)
        {
            this.PrintButtonStatusChange();
        }

        private void RadioP18ExcelImport_CheckedChanged(object sender, EventArgs e)
        {
            this.PrintButtonStatusChange();
        }

        private void PrintButtonStatusChange()
        {
            if (this.radioTransferOutReport.Checked || this.radioQRCodeSticker.Checked)
            {
                this.print.Enabled = true;
                this.toexcel.Enabled = false;
            }
            else
            {
                this.print.Enabled = false;
                this.toexcel.Enabled = true;
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (this.radioTransferOutReport.Checked)
            {
                string id = this.mainCurrentMaintain["ID"].ToString();

                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", id));
                DataTable dt;
                string cmd = $@"select  b.name 
            from dbo.TransferOut a WITH (NOLOCK) 
            inner join dbo.mdivision  b WITH (NOLOCK) 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID";
                DualResult result = DBProxy.Current.Select(string.Empty, cmd, pars, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found!!!");
                }
                #endregion
                #region  抓表身資料
                pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", id));

                string tmp = @"
select a.POID
    ,a.Seq1+'-'+a.seq2 as SEQ
	,a.Roll,a.Dyelot
	,[DESC] = IIF((b.ID =   lag(b.ID,1,'') over (order by b.id, b.seq1, b.seq2, a.Dyelot, Len(a.Roll), a.Roll) 
		AND(b.seq1 = lag(b.seq1,1,'')over (order by b.id, b.seq1, b.seq2, a.Dyelot, Len(a.Roll), a.Roll))
		AND(b.seq2 = lag(b.seq2,1,'')over (order by b.id, b.seq1, b.seq2, a.Dyelot, Len(a.Roll), a.Roll))) 
		,'',dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))
    ,[ToPoid] = iif(a.ToPOID = '', '', 'TO POID: ' + a.ToPOID + ' , Seq: ' + a.ToSeq1 + '-' + a.ToSeq2)
	,CASE a.stocktype
			WHEN 'B' THEN 'Bulk'
			WHEN 'I' THEN 'Inventory'
			WHEN 'O' THEN 'Scrap'
			ELSE a.stocktype
			END
			stocktype
	,unit = b.StockUnit
	,a.Qty
	,[Location]=dbo.Getlocation(fi.ukey)
    ,fi.ContainerCode
    ,[Total]=sum(a.Qty) OVER (PARTITION BY a.POID ,a.Seq1,a.Seq2 ) 	        
    ,[RecvKG] = case when rd.ActualQty is not null 
						then case when rd.ActualQty <> a.Qty
								then ''
								else cast(iif(ISNULL(rd.ActualWeight,0) > 0, rd.ActualWeight, rd.Weight) as varchar(20))
							 end
						else case when td.ActualQty <> a.Qty
								then ''
								else cast(iif(ISNULL(td.ActualWeight,0) > 0, td.ActualWeight, td.Weight) as varchar(20))
							 end							
					end
    ,fi.Tone
from dbo.TransferOut_Detail a WITH (NOLOCK) 
LEFT join dbo.PO_Supp_Detail b WITH (NOLOCK) on  b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2 and a.Dyelot = fi.Dyelot
    and a.roll = fi.roll and a.stocktype = fi.stocktype
Outer apply (
	select [Weight] = SUM(rd.Weight)
		, [ActualWeight] = SUM(rd.ActualWeight)
		, [ActualQty] = SUM(rd.ActualQty)
	from Receiving_Detail rd WITH (NOLOCK) 
	where fi.POID = rd.PoId
	and fi.Seq1 = rd.Seq1
	and fi.Seq2 = rd.Seq2 
	and fi.Dyelot = rd.Dyelot
	and fi.Roll = rd.Roll
	and fi.StockType = rd.StockType
	and b.FabricType = 'F'
)rd
Outer apply (
	select [Weight] = SUM(td.Weight)
		, [ActualWeight] = SUM(td.ActualWeight)
		, [ActualQty] = SUM(td.Qty)
	from TransferIn_Detail td WITH (NOLOCK) 
	where fi.POID = td.PoId
	and fi.Seq1 = td.Seq1
	and fi.Seq2 = td.Seq2 
	and fi.Dyelot = td.Dyelot
	and fi.Roll = td.Roll
	and fi.StockType = td.StockType
	and b.FabricType = 'F'
)td
where a.id= @ID
order by b.id, b.seq1, b.seq2, a.Dyelot, Len(a.Roll), a.Roll
";
                result = DBProxy.Current.Select(string.Empty, tmp, pars, out this.dtResult);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (this.dtResult == null || this.dtResult.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found!!!");
                }

                #endregion
            }
            else if (this.radioQRCodeSticker.Checked)
            {
                return new DualResult(true);
            }
            else
            {
                string sql = $@"
select
    ToPOID,
    ToSeq1,
    ToSeq2,
    td.Roll,
    td.Dyelot,
    [GW] = '',
    td.Qty,	
    [StockType]='',
    [Location]='',
    [ContainerCode] = '',
    [Remark]='',
    td.POID,
    td.Seq1,
    td.Seq2,
    a.Tone,
	[MINDQRCode] = iif(td.Qty = 0, '', iif(isnull(w.To_NewBarcodeSeq, '') = '', w.To_NewBarcode, concat(w.To_NewBarcode, '-', w.To_NewBarcodeSeq)))
from TransferOut_Detail td with (nolock)
left join FtyInventory a with (nolock) on td.POID = a.POID and
						                    td.Seq1 = a.Seq1 and
						                    td.Seq2 = a.Seq2 and
						                    td.Dyelot = a.Dyelot and
						                    td.Roll = a.Roll and
						                    td.StockType = a.StockType
left join WHBarcodeTransaction w with (nolock) on td.Ukey = w.TransactionUkey and w.Action = 'Confirm' and [Function] = 'P19'
where ID = '{this.mainCurrentMaintain["ID"]}'
order by td.Dyelot, Len(td.Roll), td.Roll
                ";
                DualResult result = DBProxy.Current.Select(null, sql, out this.dtResult);
                if (!result)
                {
                    return result;
                }
            }

            return new DualResult(true);
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
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_P18_ExcelImport.xltx", objApp);
            com.UseInnerFormating = false;

            // excel不須顯示ContainerCode
            DataTable dtExcel = this.dtResult.Copy();
            dtExcel.Columns.Remove("ContainerCode");
            com.WriteTable(dtExcel, 3);
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool OnToPrint(ReportDefinition report)
        {
            if (this.radioQRCodeSticker.Checked)
            {
                WH_Print p = new WH_Print(this.mainCurrentMaintain, "P19")
                {
                    CurrentDataRow = this.mainCurrentMaintain,
                };

                DataTable dt = p.GetP19Data();
                var barcodeDatas = dt.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["MINDQRCode"]));

                if (barcodeDatas.Count() == 0)
                {
                    MyUtility.Msg.InfoBox("No Data can print");
                    return true;
                }

                var qrCodeSticker = new WH_Receive_QRCodeSticker(barcodeDatas.CopyToDataTable(), this.comboType.Text, callFrom: "P19");
                System.Windows.Forms.DialogResult dialogResult = qrCodeSticker.ShowDialog();

                // 在這裡進行任何額外的操作
                qrCodeSticker.Dispose(); // 手動釋放 qrCodeSticker 物件

                return true;
            }

            this.SetCount(this.dtResult.Rows.Count);

            DataRow row = this.mainCurrentMaintain;
            string id = row["ID"].ToString();
            string remark = row["Remark"].ToString().Trim().Replace("\r", " ").Replace("\n", " ");
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            // 抓M的EN NAME
            DataTable dtNAME;
            DBProxy.Current.Select(
                string.Empty,
                string.Format(@"select NameEN from MDivision where ID='{0}'", Env.User.Keyword), out dtNAME);
            string rptTitle = dtNAME.Rows[0]["NameEN"].ToString();

            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));

            // 傳 list 資料
            List<P19_PrintData> data = this.dtResult.AsEnumerable()
                .Select(row1 => new P19_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    Dyelot = row1["Dyelot"].ToString().Trim(),
                    DESC = (MyUtility.Check.Empty(row1["DESC"]) == false) ? row1["DESC"].ToString().Trim() + Environment.NewLine + row1["ToPoid"].ToString().Trim() + Environment.NewLine + "Recv(Kg) : " + row1["RecvKG"].ToString().Trim() : "Recv(Kg) :" + row1["RecvKG"].ToString().Trim(),
                    Tone = row1["Tone"].ToString().Trim(),
                    Stocktype = row1["stocktype"].ToString().Trim(),
                    Unit = row1["unit"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            Type reportResourceNamespace = typeof(P19_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P19_Print.rdlc";

            IReportResource reportresource;
            DualResult result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource);
            if (!result)
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            return true;
        }
    }
}