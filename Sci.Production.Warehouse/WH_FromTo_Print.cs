using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class WH_FromTo_Print : Win.Tems.PrintForm
    {
        private DataRow CurrentMaintain;
        private string callFrom;
        private string id;

        /// <inheritdoc/>
        public bool IsPrintRDLC = false;

        /// <inheritdoc/>
        public WH_FromTo_Print(DataRow currentMaintain, string formname)
        {
            this.InitializeComponent();
            this.Text = formname + " " + currentMaintain["ID"].ToString();
            this.CurrentMaintain = currentMaintain;
            this.callFrom = formname;
            this.ButtonEnable();
            MyUtility.Tool.SetupCombox(this.comboPrint, 1, 1, "Sticker,Paper");
            this.comboPrint.Text = "Sticker";
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.id = this.CurrentDataRow["ID"].ToString();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return new DualResult(true);
        }

        /// <inheritdoc/>
        public DataRow CurrentDataRow { get; set; }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            if (this.radioTransferSlip.Checked)
            {
                this.IsPrintRDLC = true;
                this.Close();
                return true;
            }
            else
            {
                this.QRCodeSticker(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.comboPrint.Text, this.comboType.Text, this.callFrom);
            }

            return true;
        }

        /// <inheritdoc/>
        private void QRCodeSticker(string id, string print, string type, string callFormName)
        {
            string qty;
            string qtyTo;
            string qtyFrom;
            string balanceQty;
            switch (callFormName)
            {
                case "P22":
                case "P23":
                    qty = "Qty";
                    qtyTo = "sd.Qty";
                    qtyFrom = "FromBalanceQty";
                    balanceQty = "sd.Qty";
                    break;
                default:
                    qty = "qty";
                    qtyTo = "sd.Qty";
                    qtyFrom = "qty";
                    balanceQty = "f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty";
                    break;
            }

            string sqlcmd = $@"
select sd.*
    , From_Barcode = iif(w.From_NewBarcodeSeq = '', w.From_NewBarcode, concat(w.From_NewBarcode, '-', w.From_NewBarcodeSeq))
    , To_Barcode = iif(w.To_NewBarcodeSeq = '', w.To_NewBarcode, concat(w.To_NewBarcode, '-', w.To_NewBarcodeSeq))
into #tmp
from dbo.{Prgs.GetWHDetailTableName(callFormName)} sd with(nolock)
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
    , TransQty = Sum({qty})
    , Qty = {qtyFrom}
    , Barcode = From_Barcode
into #tmpFrom
from #tmp
group by FromPOID,FromSeq1,FromSeq2,FromRoll,FromDyelot,FromStockType,{qtyFrom},From_Barcode

select
    Sel = Cast(0 as bit)
    , sd.POID
    , sd.Seq 
    , sd.Seq1
    , sd.Seq2
    , sd.Roll
    , sd.Dyelot
    , sd.StockType
    , sd.TransQty
    , Qty = {balanceQty}
    , sd.Barcode
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
    , Qty = {qtyTo}
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

        private void RadioGroup1_ValueChanged(object sender, EventArgs e)
        {
            this.ButtonEnable();
        }

        private void ButtonEnable()
        {
            this.comboPrint.Enabled = this.radioQRCodeSticker.Checked;
            this.comboType.Enabled = this.radioQRCodeSticker.Checked;
        }
    }
}
