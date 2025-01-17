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
                QRCodeSticker(MyUtility.Convert.GetString(this.mainCurrentMaintain["ID"]), this.comboType.Text, "P19");
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

        /// <summary>
        /// Print QRCode Sticker for P19 & P45
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="type">type</param>
        /// <param name="callFormName">callFormName</param>
        public static void QRCodeSticker(string id, string type, string callFormName)
        {
            string qty;
            string qtyTo;
            switch (callFormName)
            {
                case "P45":
                    qty = "isnull(QtyBefore,0.00) - isnull(QtyAfter,0.00)";
                    qtyTo = "isnull(sd.QtyBefore,0.00) - isnull(sd.QtyAfter,0.00)";
                    break;
                default:
                    qty = "Qty";
                    qtyTo = "sd.Qty";
                    break;
            }

            string sqlcmd = $@"
select sd.*
    , From_Barcode = iif(w.From_NewBarcodeSeq = '', w.From_NewBarcode, concat(w.From_NewBarcode, '-', w.From_NewBarcodeSeq))
    , To_Barcode = iif(w.To_NewBarcodeSeq = '', w.To_NewBarcode, concat(w.To_NewBarcode, '-', w.To_NewBarcodeSeq))
into #tmp
from dbo.{Prgs.GetWHDetailTableName(callFormName)} sd with(nolock)
inner join PO_Supp_Detail psd with(nolock) on psd.id = sd.POID and psd.SEQ1 = sd.Seq1 and psd.SEQ2 = sd.Seq2 and psd.FabricType = 'F'
inner join WHBarcodeTransaction w with(nolock) on w.TransactionID = sd.id and w.TransactionUkey = sd.Ukey and w.Action = 'Confirm'
where sd.id = '{id}'

-- From
select
      POID= POID
    , Seq = Concat(Seq1, ' ', Seq2)
    , Seq1 = Seq1
    , Seq2 = Seq2
    , Roll = Roll
    , Dyelot = Dyelot
    , StockType = StockType
    , Qty = Sum({qty})
    , Barcode = From_Barcode
into #tmpFrom
from #tmp
group by POID,Seq1,Seq2,Roll,Dyelot,StockType,From_Barcode

select
    Sel = Cast(0 as bit)
    , sd.*
    , Weight = isnull(rd.Weight, td.Weight)
    , ActualWeight = isnull(rd.ActualWeight, td.ActualWeight)
    , Location = dbo.Getlocation(f.Ukey)
    , psd.Refno
    , ColorID = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
    , o.FactoryID
    , StockTypeName = 
        case sd.StockType
        when 'b' then 'Bulk'
        when 'i' then 'Inventory'
        when 'o' then 'Scrap'
        end
    , o.StyleID
    , WhseArrival = isnull(rd.WhseArrival, td.IssueDate)
    , fr.Relaxtime
from #tmpFrom sd
inner join PO_Supp_Detail psd with(nolock) on psd.id = sd.POID and psd.SEQ1 = sd.Seq1 and psd.SEQ2 = sd.Seq2 and psd.FabricType = 'F'
inner join Fabric with(nolock) on Fabric.SCIRefno = psd.SCIRefno
inner join View_WH_Orders o with(nolock) on o.id = psd.ID
inner join Ftyinventory f with (nolock) on f.PoId = sd.POID
                                       and f.Seq1 = sd.Seq1
                                       and f.Seq2 = sd.Seq2
                                       and f.Roll = sd.Roll
                                       and f.Dyelot = sd.Dyelot
                                       and f.StockType = sd.StockType
left join PO_Supp_Detail_Spec psdsC with(nolock) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
outer apply(
    select rd.Weight, rd.ActualWeight, Receiving.WhseArrival
    from Receiving_Detail rd with(nolock)
    inner join Receiving with(nolock) on Receiving.id = rd.id
    where rd.PoId = sd.POID
    and rd.Seq1 = sd.Seq1
    and rd.Seq2 = sd.Seq2
    and rd.Roll = sd.Roll
    and rd.Dyelot = sd.Dyelot
    and rd.StockType = sd.StockType
)rd
outer apply(
    select td.Weight, td.ActualWeight, TransferIn.IssueDate
    from TransferIn_Detail td with(nolock)
    inner join TransferIn with(nolock) on TransferIn.id = td.id
    where td.PoId = sd.POID
    and td.Seq1 = sd.Seq1
    and td.Seq2 = sd.Seq2
    and td.Roll = sd.Roll
    and td.Dyelot = sd.Dyelot
    and td.StockType = sd.StockType
)td
LEFT JOIN [SciMES_RefnoRelaxtime] rr WITH (NOLOCK) ON rr.Refno = psd.Refno
LEFT JOIN [SciMES_FabricRelaxation] fr WITH (NOLOCK) ON rr.FabricRelaxationID = fr.ID
ORDER BY POID,Seq,Roll, Dyelot

-- To
select
    Sel = Cast(0 as bit)
    , POID = ToPOID
    , Seq = Concat(ToSeq1, ' ', ToSeq2)
    , Seq1 = ToSeq1
    , Seq2 = ToSeq2
    , Roll = ''
    , Dyelot = ''
    , StockType = ''
    , [Qty] = {qtyTo}
    , Barcode = To_Barcode
    , Weight = isnull(rd.Weight, td.Weight)
    , ActualWeight = isnull(rd.ActualWeight, td.ActualWeight)
    , Location = dbo.Getlocation(f.Ukey)
    , psd.Refno
    , ColorID = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
    , o.FactoryID
    , StockTypeName = ''
    , o.StyleID
    , WhseArrival = isnull(rd.WhseArrival, td.IssueDate)
From #tmp sd
left join PO_Supp_Detail psd with(nolock) on psd.id = sd.ToPOID and psd.SEQ1 = ToSeq1 and psd.SEQ2 = sd.ToSeq2 and psd.FabricType = 'F'
left join Fabric with(nolock) on Fabric.SCIRefno = psd.SCIRefno
left join View_WH_Orders o with(nolock) on o.id = psd.ID
left join Ftyinventory f with (nolock) on f.PoId = sd.ToPOID
                                       and f.Seq1 = sd.ToSeq1
                                       and f.Seq2 = sd.ToSeq2
left join PO_Supp_Detail_Spec psdsC with(nolock) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
outer apply(
    select rd.Weight, rd.ActualWeight, Receiving.WhseArrival
    from Receiving_Detail rd with(nolock)
    inner join Receiving with(nolock) on Receiving.id = rd.id
    where rd.PoId = sd.ToPOID
    and rd.Seq1 = sd.ToSeq1
    and rd.Seq2 = sd.ToSeq2
)rd
outer apply(
    select td.Weight, td.ActualWeight, TransferIn.IssueDate
    from TransferIn_Detail td with(nolock)
    inner join TransferIn with(nolock) on TransferIn.id = td.id
    where td.PoId = sd.ToPOID
    and td.Seq1 = sd.ToSeq1
    and td.Seq2 = sd.ToSeq2
)td
ORDER BY POID,Seq,Roll, Dyelot

drop table #tmp,#tmpFrom
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable[] dts);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            if (dts[0].Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No Data can print");
                return;
            }

            var barcodeDatasFrom = dts[0].AsEnumerable().Where(s => !MyUtility.Check.Empty(s["Barcode"]));
            var barcodeDatasTo = dts[1].AsEnumerable().Where(s => !MyUtility.Check.Empty(s["Barcode"]));

            if (barcodeDatasFrom.Count() == 0 && barcodeDatasTo.Count() == 0)
            {
                MyUtility.Msg.InfoBox("No Data can print");
                return;
            }

            new WH_FromTo_QRCodeSticker(dts[0], dts[1], type, callFormName).ShowDialog();
        }
    }
}