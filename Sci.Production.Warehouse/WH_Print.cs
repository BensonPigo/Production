using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class WH_Print : Win.Tems.PrintForm
    {
        private DataTable dt;
        private string id;
        private DataRow curMain;
        private string callFrom;

        /// <inheritdoc/>
        public bool IsPrintRDLC = false;

        /// <inheritdoc/>
        public WH_Print(DataRow currentMain, string formname)
        {
            this.InitializeComponent();
            this.CheckControlEnable();
            this.curMain = currentMain;
            this.Text = formname + " " + this.curMain["ID"].ToString();
            DataTable dtPMS_FabricQRCode_LabelSize;
            this.callFrom = formname;
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

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.id = this.CurrentDataRow["ID"].ToString();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        public DataTable GetP19Data()
        {
            DataTable retrunDt = new DataTable();
            this.id = this.CurrentDataRow["ID"].ToString();
            DualResult result = this.LoadData();
            if (!result)
            {
                this.ShowErr(result);
                return retrunDt;
            }

            retrunDt = this.dt;
            return retrunDt;
        }

        private DualResult LoadData()
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", SqlDbType.VarChar, size: this.id.Length) { Value = this.id });
            string qty = "t.Qty";
            string minQRCode = @"
,[MINDQRCode] = case when w.To_NewBarcodeSeq = '' then w.To_NewBarcode
                        when w.To_NewBarcode = ''  then ''
                else Concat(w.To_NewBarcode, '-', w.To_NewBarcodeSeq)   
				end";
            string fromOldBarcode = string.Empty;

            switch (this.callFrom)
            {
                case "P34":
                case "P35":
                case "P43":
                case "P45":
                    qty = "isnull(t.QtyAfter,0.00)";
                    minQRCode = @"
,[MINDQRCode] = case when w.From_NewBarcodeSeq = '' then w.From_NewBarcode
                     when w.From_NewBarcode = ''  then ''
                else Concat(w.From_NewBarcode, '-', w.From_NewBarcodeSeq)   
				end";
                    break;
                case "P13":
                case "P16":
                case "P19":
                case "P62":
                    fromOldBarcode = @"
,From_OldBarcode = (
        select case when wbt.From_OldBarcodeSeq = '' then wbt.From_OldBarcode
                    when wbt.From_OldBarcode = '' then ''
                    else Concat(wbt.From_OldBarcode, '-', wbt.From_OldBarcodeSeq) end
        from WHBarcodeTransaction wbt with (nolock)
        where wbt.TransactionUkey = t.Ukey
        and wbt.TransactionID = t.id
        and wbt.Action = 'Confirm'
    )
";
                    break;
                default:
                    break;
            }

            string sql = $@"


select  
[Sel] = 0
{minQRCode}
,t2.*
into #tmp
from dbo.{Prgs.GetWHDetailTableName(this.callFrom)} t2 WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = t2.POID and  psd.SEQ1 = t2.Seq1 and psd.seq2 = t2.Seq2 
inner join WHBarcodeTransaction w with(nolock) on w.TransactionID = t2.id and w.TransactionUkey = t2.Ukey and w.Action = 'Confirm'
where t2.id = @ID

select
    Sel = Cast(0 as bit)
    , POID = t.POID
    , Seq = Concat(t.Seq1, ' ', t.Seq2)
    , Seq1 = t.Seq1
    , Seq2 = t.Seq2
    , Roll = t.Roll
    , Dyelot = t.Dyelot
    , StockType = t.StockType
    , StockQty = {qty}
    , MINDQRCode = t.MINDQRCode
    , Weight = isnull(rd.Weight, td.Weight)
    , ActualWeight = isnull(rd.ActualWeight, td.ActualWeight)
    , Location = dbo.Getlocation(f.Ukey)
    , psd.Refno
    , ColorID = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
    , o.FactoryID
    , StockTypeName = 
        case t.StockType
        when 'b' then 'Bulk'
        when 'i' then 'Inventory'
        when 'o' then 'Scrap'
        end
    , o.StyleID
    , WhseArrival = isnull(rd.WhseArrival, td.IssueDate)
    ,fr.Relaxtime
    ,[FabricType] = case when psd.FabricType = 'F' then 'Fabric'
                         when psd.FabricType = 'A' then 'Accessory'
                         else 'Other' end
    {fromOldBarcode}
From #tmp t
inner join PO_Supp_Detail psd with(nolock) on psd.id = t.POID and psd.SEQ1 = t.Seq1 and psd.SEQ2 = t.Seq2 and psd.FabricType = 'F'
inner join Fabric with(nolock) on Fabric.SCIRefno = psd.SCIRefno
inner join View_WH_Orders o with(nolock) on o.id = psd.ID
inner join Ftyinventory f with (nolock) on f.PoId = t.POID
                                       and f.Seq1 = t.Seq1
                                       and f.Seq2 = t.Seq2
                                       and f.Roll = t.Roll
                                       and f.Dyelot = t.Dyelot
                                       and f.StockType = t.StockType
left join PO_Supp_Detail_Spec psdsC with(nolock) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
outer apply(
    select rd.Weight, rd.ActualWeight, Receiving.WhseArrival
    from Receiving_Detail rd with(nolock)
    inner join Receiving with(nolock) on Receiving.id = rd.id
    where rd.PoId = t.POID
    and rd.Seq1 = t.Seq1
    and rd.Seq2 = t.Seq2
    and rd.Roll = t.Roll
    and rd.Dyelot = t.Dyelot
    and rd.StockType = t.StockType
)rd
outer apply(
    select td.Weight, td.ActualWeight, TransferIn.IssueDate
    from TransferIn_Detail td with(nolock)
    inner join TransferIn with(nolock) on TransferIn.id = td.id
    where td.PoId = t.POID
    and td.Seq1 = t.Seq1
    and td.Seq2 = t.Seq2
    and td.Roll = t.Roll
    and td.Dyelot = t.Dyelot
    and td.StockType = t.StockType
)td
LEFT JOIN [SciMES_RefnoRelaxtime] rr WITH (NOLOCK) ON rr.Refno = psd.Refno
LEFT JOIN [SciMES_FabricRelaxation] fr WITH (NOLOCK) ON rr.FabricRelaxationID = fr.ID
ORDER BY POID,Seq,Roll, Dyelot


drop table #tmp
";
            DualResult result = DBProxy.Current.Select(
                string.Empty,
                sql,
                pars,
                out this.dt);

            return result;
        }

        /// <inheritdoc/>
        public DataRow CurrentDataRow { get; set; }

        private void CheckControlEnable()
        {
            this.comboType.Enabled = this.radioQRCodeSticker.Checked;
        }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            DualResult result;
            if (this.radioPrint.Checked)
            {
                this.IsPrintRDLC = true;
                this.Close();
                return true;
            }

            this.IsPrintRDLC = false;

            if (this.radioQRCodeSticker.Checked && this.curMain["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, cannot print.");
                return false;
            }

            this.ValidateInput();
            this.ShowWaitMessage("Loading...");
            result = this.LoadData();
            this.HideWaitMessage();
            if (!result)
            {
                this.ShowErr(result);
                return true;
            }

            var barcodeDatas = this.dt.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["MINDQRCode"]));

            if (barcodeDatas.Count() == 0)
            {
                MyUtility.Msg.InfoBox("No Data can print");
                return true;
            }

            new WH_Receive_QRCodeSticker(barcodeDatas.CopyToDataTable(), this.comboType.Text, callFrom: this.callFrom).ShowDialog();

            return true;
        }

        private void RadioPrint_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckControlEnable();
        }
    }
}
