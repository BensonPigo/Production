using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P16 : Sci.Win.Tems.Input6
    {
        private DataGridViewNumericBoxColumn col_NW;
        private DataGridViewNumericBoxColumn col_GW;
        private DataGridViewNumericBoxColumn col_CBM;
        private bool isProduceFty;

        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (this.CurrentMaintain == null)
            {
                return;
            }

            // from factoryid is Factory.IsproduceFty
            this.isProduceFty = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"select IsProduceFty from Factory where id ='{this.CurrentMaintain["FromFactoryID"]}'"));

            // TPE Status
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Junk"]))
            {
                this.dispTPEStatus.Text = "Junk";
            }
            else if (!MyUtility.Check.Empty(this.CurrentMaintain["Confirm"]))
            {
                this.dispTPEStatus.Text = "[Confirm] To fty can start to Transfer Out.";
            }
            else if (!MyUtility.Check.Empty(this.CurrentMaintain["Sent"]))
            {
                this.dispTPEStatus.Text = "[Sent] From fty can start to Transfer In.";
            }
            else if (MyUtility.Check.Empty(this.CurrentMaintain["Sent"]))
            {
                this.dispTPEStatus.Text = "New";
            }
            else
            {
                this.dispTPEStatus.Text = "";
            }

            switch (MyUtility.Convert.GetString(this.CurrentMaintain["Payer"]))
            {
                case "S":
                    this.displayPayer.Value = "By Sci Taipei Office(Sender)";
                    break;
                case "M":
                    this.displayPayer.Value = "By Mill(Sender)";
                    break;
                case "F":
                    this.displayPayer.Value = "By Factory(Receiver)";
                    break;
                default:
                    this.displayPayer.Value = string.Empty;
                    break;
            }

            switch (MyUtility.Convert.GetString(this.CurrentMaintain["FtyStatus"]))
            {
                case "New":
                    this.dispFtyStatus.Value = "New";
                    break;
                case "Send":
                    this.dispFtyStatus.Value = "[Send] From fty WH Confirm";
                    break;
                case "Confirm":
                    this.dispFtyStatus.Value = "[Confirm] From fty Shipping Confirm";
                    break;
                default:
                    break;
            }

            this.dispExportCountry.Value = this.CurrentMaintain["ExportPort"].ToString() + "-" + this.CurrentMaintain["ExportCountry"].ToString();

            this.dispDischarge.Value = this.CurrentMaintain["ImportPort"].ToString() + "-" + this.CurrentMaintain["ImportCountry"].ToString();

            string currencyRate = MyUtility.GetValue.Lookup($@"
select TPEPaidUSD =
isnull(cast(round(TransferExport.PrepaidFtyImportFee * (select Rate from[dbo].[GetCurrencyRate]('', (select CurrencyID from Supp where Supp.id = TransferExport.Forwarder), 'USD', '')), 2) as float),0)
from TransferExport
where id = '{this.CurrentMaintain["ID"]}'");
            this.numTPEPaidUSD.Value = MyUtility.Convert.GetDecimal(currencyRate);

            this.dispRespFty.Value = this.CurrentMaintain["OTResponsibleFty1"].ToString() + "-" + this.CurrentMaintain["OTResponsibleFty2"].ToString();

            #region Door to Door
            string chkdtd = $@"
select 1
from Door2DoorDelivery 
where ExportPort = '{this.CurrentMaintain["ExportPort"]}'
      and ExportCountry ='{this.CurrentMaintain["ExportCountry"]}'
      and ImportCountry = '{this.CurrentMaintain["ImportCountry"]}'
      and ShipModeID = '{this.CurrentMaintain["ShipModeID"]}'
      and Vessel ='{this.CurrentMaintain["Vessel"]}'
union 
select 1
from Door2DoorDelivery
where ExportPort = '{this.CurrentMaintain["ExportPort"]}'
      and ExportCountry ='{this.CurrentMaintain["ExportCountry"]}'
      and ImportCountry = '{this.CurrentMaintain["ImportCountry"]}'
      and ShipModeID = '{this.CurrentMaintain["ShipModeID"]}'
      and Vessel  =''
";
            this.ChkDoortoDoorDelivery.Checked = MyUtility.Check.Seek(chkdtd);
            #endregion

            this.labJunk.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["Junk"]) == "True" ? true : false;
            this.labFromE.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["FormE"]) == "True" ? true : false;

            this.ControlColor();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select 
ted.ID
,ted.InventoryPOID
,[InventorySEQ] = CONCAT(ted.InventorySeq1,' ',ted.InventorySeq2)
,ted.PoID
,[SEQ] = CONCAT(ted.Seq1,' ',ted.Seq2)
,ted.Seq1,ted.Seq2
,ted.SuppID
,ted.Description
,ted.UnitID
,[Color] = case when psdInv.ColorID is not null then psdInv.ColorID
			    when psd.ColorID is not null then psd.ColorID
				else '' end
,[Size] = case  when psdInv.SizeSpec is not null then psdInv.SizeSpec
			    when psd.SizeSpec is not null then psd.SizeSpec
				else '' end
,ted.PoQty
,[Export] = isnull(carton.ttlQty,0)
,[FOC] = isnull(carton.ttlFoc,0)
,[Balance] = isnull(carton.ttlQty,0) + isnull(carton.ttlFoc,0)
,ted.TransferExportReason
,[ReasonDesc] = isnull((select [Description] from WhseReason where Type = 'TE' and ID = ted.TransferExportReason),'')
,ted.NetKg
,ted.WeightKg
,ted.CBM
,ContainerType = isnull(ContainerType.Value,'')
from TransferExport_Detail ted WITH (NOLOCK) 
left join PO_Supp_Detail psdInv WITH (NOLOCK) on psdInv.ID = ted.InventoryPOID and psdInv.SEQ1 = ted.InventorySeq1 and psdInv.SEQ2 = ted.InventorySeq2
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ted.PoID and psd.SEQ1 = ted.Seq1 and psd.SEQ2 = ted.Seq2
outer apply(
	SELECT [Value] = STUFF((
    SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
    FROM TransferExport_ShipAdvice_Container esc
    WHERE esc.TransferExport_DetailUkey = ted.Ukey
        AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
    FOR XML PATH('')
     ),1,1,'')
)ContainerType 

outer apply(
	select ttlQty = sum(tedc.Qty)
	,ttlFoc = sum(tedc.Foc)
	from TransferExport_Detail_Carton tedc
	where tedc.TransferExport_DetailUkey = ted.Ukey
) carton
where ted.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("InventoryPOID", header: "From SP#", width: Ict.Win.Widths.AnsiChars(13))
                .Text("InventorySEQ", header: "From SEQ", width: Ict.Win.Widths.AnsiChars(8))
                .Text("PoID", header: "To SP#", width: Ict.Win.Widths.AnsiChars(13))
                .Text("SEQ", header: "To SEQ", width: Ict.Win.Widths.AnsiChars(8))
                .Text("SuppID", header: "Supplier", width: Ict.Win.Widths.AnsiChars(13))
                .Text("Description", header: "Description", width: Ict.Win.Widths.AnsiChars(20))
                .Text("UnitID", header: "Unit", width: Ict.Win.Widths.AnsiChars(5))
                .Text("Color", header: "Color", width: Ict.Win.Widths.AnsiChars(5))
                .Text("Size", header: "Size", width: Ict.Win.Widths.AnsiChars(5))
                .Numeric("PoQty", header: "Po  Q'ty", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(5))
                .Numeric("Export", header: "Export Q'ty", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(5))
                .Numeric("FOC", header: "F.O.C.", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(2))
                .Numeric("BalanceQty", header: "Balance", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(5))
                .Text("TransferExportReason", header: "Reason", width: Ict.Win.Widths.AnsiChars(10))
                .Text("ReasonDesc", header: "Reason Desc", width: Ict.Win.Widths.AnsiChars(10))
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2).Get(out this.col_NW)
                .Numeric("WeightKg", header: "G.W.(kg)", decimal_places: 2).Get(out this.col_GW)
                .Numeric("CBM", header: "CBM", decimal_places: 4).Get(out this.col_CBM)
                .Text("ContainerType", header: "ContainerType & No", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            // 設定detailGrid Rows 是否可以編輯
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.EditMode == false)
            {
                return;
            }

            var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            // 只有 isProduceFty = 1 才允許編輯此欄位
            if (this.isProduceFty == true)
            {
                this.col_NW.IsEditingReadOnly = false;
                this.col_GW.IsEditingReadOnly = false;
                this.col_CBM.IsEditingReadOnly = false;
            }
            else
            {
                this.col_NW.IsEditingReadOnly = false;
                this.col_GW.IsEditingReadOnly = false;
                this.col_CBM.IsEditingReadOnly = false;
            }
        }

        private void BtnExpenseData_Click(object sender, EventArgs e)
        {

        }

        private void ControlColor()
        {
            string col = MyUtility.Convert.GetString(this.CurrentMaintain["Type"]) == "3" ? "InvNo" : "WKNo";
            DataTable gridData;
            string sqlCmd = string.Empty;

            switch (col)
            {
                case "InvNo":
                    sqlCmd = string.Format(
                        @"select 1
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN SciFMS_AccountNo a on se.AccountID = a.ID
where se.InvNo = '{0}' and se.junk=0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                    break;
                case "WKNo":
                    sqlCmd = string.Format(
                        @"select 1
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN SciFMS_AccountNo a on se.AccountID = a.ID
where se.WKNo = '{0}' and se.junk=0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                    break;
                default:
                    sqlCmd = "select 1 from ShareExpense WITH (NOLOCK) where 1=2";
                    break;
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (gridData.Rows.Count > 0)
            {
                this.btnExpenseData.ForeColor = Color.Blue;
            }
            else
            {
                this.btnExpenseData.ForeColor = Color.Black;
            }
        }

        private void BtnShippingMark_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["ShipMarkDesc"]), "Shipping Mark", false, null);
            callNextForm.ShowDialog(this);
        }

        protected override bool ClickPrint()
        {
            #region To Excel
            DataTable excelTable = (DataTable)this.detailgridbs.DataSource;
            DataTable printDT = excelTable.Clone();


            #endregion

            return base.ClickPrint();
        }
    }
}
