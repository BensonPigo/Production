using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P16 : Sci.Win.Tems.Input6
    {
        private DataGridViewNumericBoxColumn col_NW;
        private DataGridViewNumericBoxColumn col_GW;
        private DataGridViewNumericBoxColumn col_CBM;
        private bool isToProduceFty;
        private bool isFromProduceFty;

        /// <inheritdoc/>
        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.GetProduceFTY();

            if (this.isToProduceFty == true && this.EditMode == true)
            {
                this.dateArrivePortDate.ReadOnly = false;
                this.dateDoxRcvDate.ReadOnly = false;
                this.txtOTResponsibleFty1.ReadOnly = false;
                this.txtOTResponsibleFty2.ReadOnly = false;
                this.chkImportChange.ReadOnly = false;
            }
            else
            {
                this.dateArrivePortDate.ReadOnly = true;
                this.dateDoxRcvDate.ReadOnly = true;
                this.txtOTResponsibleFty1.ReadOnly = true;
                this.txtOTResponsibleFty2.ReadOnly = true;
                this.chkImportChange.ReadOnly = true;
            }

            if (this.EditMode)
            {
                this.chkExportChange.ReadOnly = !this.isFromProduceFty;
                this.detailgrid.IsEditingReadOnly = false;
                if (this.isFromProduceFty && MyUtility.Convert.GetString(this.CurrentMaintain["FtyStatus"]) == "Confirmed")
                {
                    this.detailgrid.IsEditingReadOnly = true;
                }
            }
            else
            {
                this.chkExportChange.ReadOnly = true;
            }

            // TPE Status
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Junk"]))
            {
                this.dispTPEStatus.Text = "Junk";
            }
            else if (!MyUtility.Check.Empty(this.CurrentMaintain["Confirm"]))
            {
                this.dispTPEStatus.Text = "[Confirm] To fty can start to Transfer In.";
            }
            else if (!MyUtility.Check.Empty(this.CurrentMaintain["Sent"]))
            {
                this.dispTPEStatus.Text = "[Sent] From fty can start to Transfer Out.";
            }
            else if (MyUtility.Check.Empty(this.CurrentMaintain["Sent"]))
            {
                this.dispTPEStatus.Text = "New";
            }
            else
            {
                this.dispTPEStatus.Text = string.Empty;
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
                case "Confirmed":
                    this.dispFtyStatus.Value = "[Confirm] From fty Shipping Confirm";
                    break;
                default:
                    break;
            }

            // 表頭NW,GW,CBM 要從表身加總取得
            string sqlTtl = $@"
select NetKg = isnull(sum(NetKg),0) , WeightKg = isnull(sum(WeightKg),0), Cbm = isnull(sum(cbm),0)
from TransferExport_Detail
where ID = '{this.CurrentMaintain["ID"]}'
";
            if (MyUtility.Check.Seek(sqlTtl, out DataRow drTTl))
            {
                this.numCBM.Value = MyUtility.Convert.GetDecimal(drTTl["Cbm"]);
                this.numWeightKg.Value = MyUtility.Convert.GetDecimal(drTTl["WeightKg"]);
                this.numNetKg.Value = MyUtility.Convert.GetDecimal(drTTl["NetKg"]);
            }

            this.dispExportCountry.Value = this.CurrentMaintain["ExportPort"].ToString() + "-" + this.CurrentMaintain["ExportCountry"].ToString();

            this.dispDischarge.Value = this.CurrentMaintain["ImportPort"].ToString() + "-" + this.CurrentMaintain["ImportCountry"].ToString();

            string currencyRate = MyUtility.GetValue.Lookup($@"
select TPEPaidUSD =
isnull(cast(round(TransferExport.PrepaidFtyImportFee * (select Rate from[dbo].[GetCurrencyRate]('', (select CurrencyID from Supp where Supp.id = TransferExport.Forwarder), 'USD', '')), 2) as float),0)
from TransferExport
where id = '{this.CurrentMaintain["ID"]}'");
            this.numTPEPaidUSD.Value = MyUtility.Convert.GetDecimal(currencyRate);

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

        /// <inheritdoc/>
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
,[ExportQty] = isnull(carton.ttlQty,0)
,[FOC] = isnull(carton.ttlFoc,0)
,[Balance] = isnull(carton.ttlQty,0) + isnull(carton.ttlFoc,0)
,ted.TransferExportReason
,[ReasonDesc] = isnull((select [Description] from WhseReason where Type = 'TE' and ID = ted.TransferExportReason),'')
,ted.NetKg
,ted.WeightKg
,ted.CBM
,ted.Refno
,ContainerType = isnull(ContainerType.Value,'')
,[FindColumn] = rtrim(ted.InventoryPOID)+'-'+(SUBSTRING(ted.InventorySeq1,1,3)+'-'+ted.InventorySeq2)
,ted.ukey
-- print column
,o.FactoryID
,o.ProjectID
,[Supp] = (ted.SuppID+'-'+s.AbbEN) 
,[SCIDlv] = (select min(SciDelivery) from Orders WITH (NOLOCK) where POID = ted.PoID and (Category = 'B' or Category = o.Category))
,[Category] = (
	case when o.Category = 'B' then 'Bulk' 
		 when o.Category = 'S' then 'Sample' 
		 when o.Category = 'M' then 'Material'
		 when o.Category = 'T' then 'Material' 
	else '' end)
,[InspDate] =iif(o.PFOrder = 1,dateadd(day,-10,o.SciDelivery)
	,iif((select CountryID from Factory WITH (NOLOCK) where ID = o.factoryID)='PH'
	,iif((select MrTeam from Brand WITH (NOLOCK) where ID = o.BrandID) = '01',dateadd(day,-15,o.SciDelivery),dateadd(day,-24,o.SciDelivery))
	,dateadd(day,-34,o.SciDelivery)))
,[Description] = iif(ted.Description = '',isnull(f.DescDetail,''),ted.Description)
,[FabricType] = (
	case when ted.PoType = 'M' and ted.FabricType = 'M' then 'Machine' 
		 when ted.PoType = 'M' and ted.FabricType = 'P' then 'Part' 
		 when ted.PoType = 'M' and ted.FabricType = 'O' then 'Miscellaneous' 
	else '' end)
,[Preshrink] = iif(fs.Preshrink = 1, 'V' ,'')
from TransferExport_Detail ted WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = ted.PoID
left join Supp s WITH (NOLOCK) on s.id = ted.SuppID 
left join PO_Supp_Detail psdInv WITH (NOLOCK) on psdInv.ID = ted.InventoryPOID and psdInv.SEQ1 = ted.InventorySeq1 and psdInv.SEQ2 = ted.InventorySeq2
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ted.PoID and psd.SEQ1 = ted.Seq1 and psd.SEQ2 = ted.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join Fabric_Supp fs WITH (NOLOCK) on fs.SCIRefno = f.SCIRefno and fs.SuppID = s.ID
outer apply(
	SELECT [Value] = STUFF((
    SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
    FROM TransferExport_ShipAdvice_Container esc
    WHERE esc.TransferExport_Detail_Ukey = ted.Ukey
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

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings exportQtycell = new DataGridViewGeneratorNumericColumnSettings();
            exportQtycell.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                this.detailgrid.ValidateControl();
                P16_ExportMaterial frm = new P16_ExportMaterial(dr);
                frm.ShowDialog(this);
            };

            base.OnDetailGridSetup();
            this.detailgrid.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("InventoryPOID", header: "From SP#", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("InventorySEQ", header: "From SEQ", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PoID", header: "To SP#", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SEQ", header: "To SEQ", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SuppID", header: "Supplier", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Ict.Win.Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Size", header: "Size", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("PoQty", header: "Po  Q'ty", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ExportQty", header: "Export Q'ty", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(5), settings: exportQtycell, iseditingreadonly: true)
                .Numeric("FOC", header: "F.O.C.", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", decimal_places: 2, width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("TransferExportReason", header: "Reason", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ReasonDesc", header: "Reason Desc", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2, iseditingreadonly: true).Get(out this.col_NW)
                .Numeric("WeightKg", header: "G.W.(kg)", decimal_places: 2, iseditingreadonly: true).Get(out this.col_GW)
                .Numeric("CBM", header: "CBM", decimal_places: 4, iseditingreadonly: true).Get(out this.col_CBM)
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
            if (this.isFromProduceFty == true)
            {
                this.col_NW.IsEditingReadOnly = false;
                this.col_GW.IsEditingReadOnly = false;
                this.col_CBM.IsEditingReadOnly = false;
            }
            else
            {
                this.col_NW.IsEditingReadOnly = true;
                this.col_GW.IsEditingReadOnly = true;
                this.col_CBM.IsEditingReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.GetProduceFTY();

            if (string.Compare(this.CurrentMaintain["FtyStatus"].ToString(), "Send", true) == 0 && this.EditMode == false)
            {
                this.toolbar.cmdConfirm.Enabled = true;
            }
            else
            {
                this.toolbar.cmdConfirm.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.GetProduceFTY();
            if (this.isToProduceFty == false)
            {
                if (this.isFromProduceFty == false)
                {
                    MyUtility.Msg.WarningBox("Only from or to factory can use edit button.");
                    return false;
                }
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (!MyUtility.Check.Empty(this.numTPEPaidUSD.Value))
            {
                MyUtility.Msg.WarningBox("[Expense Data] already have data, please reconfirm.");
                return false;
            }

            if (MyUtility.Convert.GetDecimal(this.numTPEPaidUSD.Value) > 0)
            {
                MyUtility.Msg.WarningBox("WK has been shared expense,  [No Import Charge] shouldn't tick, please double check.");
            }

            DateTime portArrival = MyUtility.Check.Empty(this.CurrentMaintain["PortArrival"]) ? default(DateTime) : DateTime.Parse(this.CurrentMaintain["PortArrival"].ToString());
            DateTime whseArrival = MyUtility.Check.Empty(this.CurrentMaintain["WhseArrival"]) ? default(DateTime) : DateTime.Parse(this.CurrentMaintain["WhseArrival"].ToString());
            DateTime eTA = MyUtility.Check.Empty(this.CurrentMaintain["ETA"]) ? default(DateTime) : DateTime.Parse(this.CurrentMaintain["ETA"].ToString());

            // 如果 Export Country 與 Import Country 相同
            if (string.Compare(this.CurrentMaintain["ExportCountry"].ToString(), this.CurrentMaintain["ImportCountry"].ToString()) == 0)
            {
                // ShipModeID = Truck
                if (this.CurrentMaintain["ShipModeID"].ToString().ToUpper() == "TRUCK")
                {
                    // PortArrival 與 WhseArrival 皆不為空值
                    if (!MyUtility.Check.Empty(portArrival) && !MyUtility.Check.Empty(whseArrival))
                    {
                        // 到港日不可晚於到 WH 日期
                        if (DateTime.Compare(portArrival, whseArrival) == 1)
                        {
                            MyUtility.Msg.WarningBox("< Arrive Port Date > can't later than < Arrive W/H Date >");
                            return false;
                        }
                    }

                    if (!MyUtility.Check.Empty(portArrival) && !MyUtility.Check.Empty(eTA))
                    {
                        // 到港日 = ETA
                        if (DateTime.Compare(portArrival, eTA) == 0)
                        {
                            // 可正常存檔
                        }

                        // 且 PortArrival < ETA, 到港日早於 ETA 10 天內
                        if (DateTime.Compare(portArrival, eTA) < 0 &&
                            ((TimeSpan)(MyUtility.Convert.GetDate(portArrival) - MyUtility.Convert.GetDate(eTA))).Days < 10)
                        {
                            DialogResult diaR = MyUtility.Msg.QuestionBox("< Arrive Port Date > earlier than < ETA >." + Environment.NewLine + "Are you sure you want to save this data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                            if (diaR == DialogResult.No)
                            {
                                return false;
                            }
                        }

                        // 且 PortArrival > ETA, 到港日早於 ETA 10 天內
                        if (DateTime.Compare(portArrival, eTA) > 0 &&
                           ((TimeSpan)(MyUtility.Convert.GetDate(portArrival) - MyUtility.Convert.GetDate(eTA))).Days < 10)
                        {
                            DialogResult diaR = MyUtility.Msg.QuestionBox("< Arrive Port Date > later than < ETA >." + Environment.NewLine + "Are you sure you want to save this data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                            if (diaR == DialogResult.No)
                            {
                                return false;
                            }
                        }

                        if (((TimeSpan)(MyUtility.Convert.GetDate(portArrival) - MyUtility.Convert.GetDate(eTA))).Days > 10)
                        {
                            MyUtility.Msg.WarningBox("< Arrive Port Date > earlier or later more than <ETA> 10 days, Cannot be saved.");
                            return false;
                        }
                    }
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            string checkStatus = $"select 1 from TransferExport where ID = '{this.CurrentMaintain["ID"]}' and FtyStatus = 'Confirmed'";
            if (MyUtility.Check.Seek(checkStatus))
            {
                MyUtility.Msg.WarningBox("Data is already confirmed, cannot confirm again");
                this.EnsureToolbarExt();
                return;
            }

            #region 判斷轉出物料,工廠都必須填上重量資訊
            if (this.DetailDatas == null || this.DetailDatas.Count <= 0)
            {
                return;
            }

            if (this.DetailDatas.AsEnumerable().Where(r => MyUtility.Convert.GetDecimal(r["ExportQty"]) > 0 &&
                                                            (MyUtility.Check.Empty(r["NetKg"]) || MyUtility.Check.Empty(r["WeightKg"]))).Any())
            {
                MyUtility.Msg.WarningBox("N.W (kg) & G.W.(kg) cannot be empty when Export Q'ty > 0.");
                this.EnsureToolbarExt();
                return;
            }
            #endregion

            if (this.isFromProduceFty == false)
            {
                MyUtility.Msg.WarningBox("Only from factory can use Confirm button.");
                return;
            }
            else
            {
                DualResult resultAPI = Prg.APITransfer.SendTransferExport(this.CurrentMaintain["ID"].ToString());
                if (resultAPI == true)
                {
                    string sqlcmd = $@"
update TransferExport 
set FtyStatus='Confirmed'
    , editname = '{Env.User.UserID}' 
    , editdate = GETDATE()
where id = '{this.CurrentMaintain["ID"]}'
";
                    DualResult result;
                    if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
                else
                {
                    this.ShowErr(resultAPI);
                    return;
                }
            }

            base.ClickConfirm();
        }

        private void ControlColor()
        {
            DataTable gridData;
            string sqlCmd = string.Format(
                        @"select 1
from ShareExpense se WITH (NOLOCK) 
where se.WKNo = '{0}' and se.junk=0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            #region To Excel
            DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;
            if (dtDetail == null || dtDetail.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return false;
            }

            string handle;
            string ext;
            string email;

            string sqlCmd = string.Format("select * from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Handle"]));
            DataTable tPEPass1;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out tPEPass1);
            if (!result || tPEPass1.Rows.Count <= 0)
            {
                handle = string.Empty;
                ext = string.Empty;
                email = string.Empty;
            }
            else
            {
                handle = MyUtility.Convert.GetString(tPEPass1.Rows[0]["Name"]);
                ext = MyUtility.Convert.GetString(tPEPass1.Rows[0]["ExtNo"]);
                email = MyUtility.Convert.GetString(tPEPass1.Rows[0]["EMail"]);
            }

            string strXltName = Env.Cfg.XltPathDir + $"\\Shipping_P16_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 表頭NW,GW,CBM 要從表身加總取得
            decimal numCBM = 0;
            decimal numWeightKg = 0;
            decimal numNetKg = 0;
            string sqlTtl = $@"
select NetKg = isnull(sum(NetKg),0) , WeightKg = isnull(sum(WeightKg),0), Cbm = isnull(sum(cbm),0)
from TransferExport_Detail
where ID = '{this.CurrentMaintain["ID"]}'
";
            if (MyUtility.Check.Seek(sqlTtl, out DataRow drTTl))
            {
                numCBM = MyUtility.Convert.GetDecimal(drTTl["Cbm"]);
                numWeightKg = MyUtility.Convert.GetDecimal(drTTl["WeightKg"]);
                numNetKg = MyUtility.Convert.GetDecimal(drTTl["NetKg"]);
            }

            worksheet.Cells[2, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            worksheet.Cells[2, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["INVNo"]);

            worksheet.Cells[3, 2] = MyUtility.Check.Empty(this.CurrentMaintain["Eta"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["Eta"]).ToString("yyyy/MM/dd");
            worksheet.Cells[3, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Payer"]) == "S" ? "By Sci Taipei Office(Sender)" : MyUtility.Convert.GetString(this.CurrentMaintain["Payer"]) == "M" ? "By Mill(Sender)" : MyUtility.Convert.GetString(this.CurrentMaintain["Payer"]) == "F" ? "By Factory(Receiver)" : string.Empty;

            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Consignee"]);
            worksheet.Cells[4, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Blno"]);

            worksheet.Cells[5, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Packages"]);
            worksheet.Cells[5, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Vessel"]);

            worksheet.Cells[6, 2] = "TransferExport";
            worksheet.Cells[6, 6] = MyUtility.Convert.GetString(numNetKg) + " / " + MyUtility.Convert.GetString(numWeightKg);

            worksheet.Cells[7, 2] = string.Empty;
            worksheet.Cells[7, 6] = MyUtility.Convert.GetString(numCBM);

            worksheet.Cells[8, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ImportPort"] + "-" + this.CurrentMaintain["ImportCountry"]);
            worksheet.Cells[8, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Remark"]);

            worksheet.Cells[9, 2] = handle;
            worksheet.Cells[9, 6] = ext;
            worksheet.Cells[9, 8] = email;

            int rownum = 11;
            object[,] objArray = new object[1, 20];
            foreach (DataRow dr in dtDetail.Rows)
            {
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["ProjectID"];
                objArray[0, 2] = dr["POID"];
                objArray[0, 3] = dr["SCIDlv"];
                objArray[0, 4] = dr["Category"];
                objArray[0, 5] = dr["InspDate"];
                objArray[0, 6] = dr["Seq"];

                objArray[0, 7] = dr["Preshrink"];
                objArray[0, 8] = dr["Supp"];
                objArray[0, 9] = dr["Description"];
                objArray[0, 10] = dr["UnitID"];
                objArray[0, 11] = dr["Color"];
                objArray[0, 12] = dr["Size"];
                objArray[0, 13] = dr["ExportQty"];
                objArray[0, 14] = dr["FOC"];
                objArray[0, 15] = dr["Balance"];
                objArray[0, 16] = dr["NetKg"];
                objArray[0, 17] = dr["WeightKg"];

                worksheet.Range[string.Format("A{0}:T{0}", rownum)].Value2 = objArray;

                rownum++;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_P16_Print");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion

            #endregion

            return base.ClickPrint();
        }

        private void BtnExpenseData_Click(object sender, EventArgs e)
        {
            P05_ExpenseData callNextForm = new P05_ExpenseData(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "WKNo", false);
            callNextForm.ShowDialog(this);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            string poID = this.txtLocateForSP.Text + "-" + this.txtSeq1.Seq1 + "-" + this.txtSeq1.Seq2;

            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("FindColumn", poID);
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void GetProduceFTY()
        {
            this.isToProduceFty = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"select IsProduceFty from Factory where id ='{this.CurrentMaintain["FactoryID"]}' and IsProduceFty = 1"));
            this.isFromProduceFty = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"select IsProduceFty from Factory where id ='{this.CurrentMaintain["FromFactoryID"]}' and IsProduceFty = 1"));
        }
    }
}
