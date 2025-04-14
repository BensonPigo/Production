using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Win;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P22_Print : Win.Tems.PrintForm
    {
        private DataRow CurrentMaintain;

        /// <inheritdoc/>
        public P22_Print(DataRow currentMaintain)
        {
            this.InitializeComponent();
            this.Text = "P22 " + currentMaintain["ID"].ToString();
            this.CurrentMaintain = currentMaintain;

            this.ButtonEnable();
            MyUtility.Tool.SetupCombox(this.comboPrint, 1, 1, "Sticker,Paper");
            this.comboPrint.Text = "Sticker";
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            if (this.radioTransferSlip.Checked)
            {
                this.TransferSlip();
            }
            else if (this.radioQRCodeSticker.Checked)
            {
                QRCodeSticker(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.comboPrint.Text, this.comboType.Text, "P22");
            }

            return true;
        }

        private void TransferSlip()
        {
            if (!MyUtility.Check.Seek($"select NameEN from MDivision where id = '{Env.User.Keyword}'", out DataRow dr))
            {
                MyUtility.Msg.WarningBox("Data not found!", "Title");
                return;
            }

            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", MyUtility.Convert.GetString(dr["NameEN"])));
            report.ReportParameters.Add(new ReportParameter("ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            report.ReportParameters.Add(new ReportParameter("Remark", MyUtility.Convert.GetString(this.CurrentMaintain["Remark"])));
            report.ReportParameters.Add(new ReportParameter("issuedate", ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["issuedate"])).ToShortDateString()));

            #region  抓表身資料
            string cmd = $@"
select a.FromPOID
        ,a.FromSeq1+'-'+a.Fromseq2 as SEQ
        ,IIF((b.ID = lag(b.ID,1,'')over (order by b.ID,b.seq1,b.seq2) 
		    AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
		    AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
		,'',dbo.getMtlDesc(a.FromPOID,a.FromSeq1,a.Fromseq2,2,0))[DESC]
		,unit = b.StockUnit
		,a.FromRoll
        ,a.FromDyelot
		,a.Qty
		,[From_Location]=dbo.Getlocation(fi.ukey)
        ,[From_ContainerCode] = fi.ContainerCode
		,a.ToLocation 
        ,a.ToContainerCode
        ,[Total]=sum(a.Qty) OVER (PARTITION BY a.FromPOID ,a.FromSeq1,a.Fromseq2 )      
from dbo.Subtransfer_detail a  WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.FromPOID and b.SEQ1=a.FromSeq1 and b.SEQ2=a.FromSeq2
left join dbo.FtyInventory FI on a.fromPoid = fi.poid and a.fromSeq1 = fi.seq1 and a.fromSeq2 = fi.seq2 and a.fromDyelot = fi.Dyelot
    and a.fromRoll = fi.roll and a.fromStocktype = fi.stocktype
where a.id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Select(string.Empty, cmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!");
                return;
            }

            // 傳 list 資料
            report.ReportDataSource = dt.AsEnumerable()
                .Select(row1 => new P22_PrintData()
                {
                    FromPOID = row1["FromPOID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["DESC"].ToString().Trim(),
                    Unit = row1["unit"].ToString().Trim(),
                    FromRoll = row1["FromRoll"].ToString().Trim(),
                    FromDyelot = row1["FromDyelot"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    From_Location = row1["From_Location"].ToString().Trim() + Environment.NewLine + row1["From_ContainerCode"].ToString().Trim(),
                    ToLocation = row1["ToLocation"].ToString().Trim() + Environment.NewLine + row1["ToContainerCode"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim(),
                }).ToList();

            #endregion

            result = ReportResources.ByEmbeddedResource(typeof(P22_PrintData), "P22_Print.rdlc", out IReportResource reportresource);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            report.ReportResource = reportresource;
            new Win.Subs.ReportView(report) { MdiParent = this.MdiParent }.Show();
        }

        /// <inheritdoc/>
        public static void QRCodeSticker(string id, string print, string type, string callFormName)
        {
            string sqlcmd = $@"
select sd.*
    , From_Barcode = iif(w.From_NewBarcodeSeq = '', w.From_NewBarcode, concat(w.From_NewBarcode, '-', w.From_NewBarcodeSeq))
    , To_Barcode = iif(w.To_NewBarcodeSeq = '', w.To_NewBarcode, concat(w.To_NewBarcode, '-', w.To_NewBarcodeSeq))
into #tmp
from SubTransfer_Detail sd with(nolock)
inner join PO_Supp_Detail psd with(nolock) on psd.id = sd.FromPOID and psd.SEQ1 = FromSeq1 and psd.SEQ2 = sd.FromSeq2 and psd.FabricType = 'F'
inner join WHBarcodeTransaction w with(nolock) on w.TransactionID = sd.id and w.TransactionUkey = sd.Ukey and w.Action = 'Confirm'
where sd.id = '{id}'

select
      POID= FromPOID
    , Seq = Concat(FromSeq1, ' ', FromSeq2)
    , Seq1 = FromSeq1
    , Seq2 = FromSeq2
    , Roll = FromRoll
    , Dyelot = FromDyelot
    , StockType = FromStockType
    , TransQty = Sum(Qty)
    , Qty = FromBalanceQty
    , Barcode = From_Barcode
into #tmpFrom
from #tmp
group by FromPOID,FromSeq1,FromSeq2,FromRoll,FromDyelot,FromStockType,FromBalanceQty,From_Barcode

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


select
    Sel = Cast(0 as bit)
    , POID = ToPOID
    , Seq = Concat(ToSeq1, ' ', ToSeq2)
    , Seq1 = ToSeq1
    , Seq2 = ToSeq2
    , Roll = ToRoll
    , Dyelot = ToDyelot
    , StockType = ToStockType
    , sd.Qty
    , Barcode = To_Barcode
    , Weight = isnull(rd.Weight, td.Weight)
    , ActualWeight = isnull(rd.ActualWeight, td.ActualWeight)
    , Location = dbo.Getlocation(f.Ukey)
    , psd.Refno
    , ColorID = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
    , o.FactoryID
    , StockTypeName = 
        case sd.ToStockType
        when 'b' then 'Bulk'
        when 'i' then 'Inventory'
        when 'o' then 'Scrap'
        end
    , o.StyleID
    , WhseArrival = isnull(rd.WhseArrival, td.IssueDate)
From #tmp sd
inner join PO_Supp_Detail psd with(nolock) on psd.id = sd.ToPOID and psd.SEQ1 = ToSeq1 and psd.SEQ2 = sd.ToSeq2 and psd.FabricType = 'F'
inner join Fabric with(nolock) on Fabric.SCIRefno = psd.SCIRefno
inner join View_WH_Orders o with(nolock) on o.id = psd.ID
inner join Ftyinventory f with (nolock) on f.PoId = sd.ToPOID
                                       and f.Seq1 = sd.ToSeq1
                                       and f.Seq2 = sd.ToSeq2
                                       and f.Roll = sd.ToRoll
                                       and f.Dyelot = sd.ToDyelot
                                       and f.StockType = sd.ToStockType
left join PO_Supp_Detail_Spec psdsC with(nolock) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
outer apply(
    select rd.Weight, rd.ActualWeight, Receiving.WhseArrival
    from Receiving_Detail rd with(nolock)
    inner join Receiving with(nolock) on Receiving.id = rd.id
    where rd.PoId = sd.ToPOID
    and rd.Seq1 = sd.ToSeq1
    and rd.Seq2 = sd.ToSeq2
    and rd.Roll = sd.ToRoll
    and rd.Dyelot = sd.ToDyelot
    and rd.StockType = sd.ToStockType
)rd
outer apply(
    select td.Weight, td.ActualWeight, TransferIn.IssueDate
    from TransferIn_Detail td with(nolock)
    inner join TransferIn with(nolock) on TransferIn.id = td.id
    where td.PoId = sd.ToPOID
    and td.Seq1 = sd.ToSeq1
    and td.Seq2 = sd.ToSeq2
    and td.Roll = sd.ToRoll
    and td.Dyelot = sd.ToDyelot
    and td.StockType = sd.ToStockType
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

            new WH_FromTo_QRCodeSticker(dts[0], dts[1], print, type, callFormName).ShowDialog();
        }

        private void RadioGroup_ValueChanged(object sender, EventArgs e)
        {
            this.ButtonEnable();
        }

        private void ButtonEnable()
        {
            this.comboPrint.Enabled = this.radioQRCodeSticker.Checked;
            this.comboType.Enabled = this.radioQRCodeSticker.Checked;
        }

        private void ComboPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboPrint.SelectedIndex != -1)
            {
                switch (this.comboPrint.SelectedValue.ToString())
                {
                    case "Paper":
                        this.BindComboTypePaper();
                        break;
                    case "Sticker":
                    default:
                        this.BindComboTypeSticker();
                        break;
                }
            }
            else
            {
                this.BindComboTypeSticker();
            }
        }

        private void BindComboTypeSticker()
        {
            this.comboType.DataSource = null;
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

        private void BindComboTypePaper()
        {
            this.comboType.DataSource = null;
            MyUtility.Tool.SetupCombox(this.comboType, 1, 1, "Horizontal,Straight");
            this.comboType.Text = "Straight";
        }
    }
}
