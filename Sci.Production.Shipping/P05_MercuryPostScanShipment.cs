using Ict;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Class.Command;
using Sci.Production.Prg.Entity.NikeMercury;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Ict.Win.UI.DirectoryView;
using static Sci.Production.Prg.Entity.NikeMercury.RequestShipmentScannedCartonUploadLSP;
using static Sci.Production.Prg.Entity.NikeMercury.RequestShipmentShippingDetailsUpdateLSP;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05_MercuryPostScanShipment
    /// </summary>
    public partial class P05_MercuryPostScanShipment : Win.Tems.QueryForm
    {
        private string invNo;

        /// <summary>
        /// P05_MercuryPostScanShipment
        /// </summary>
        /// <param name="invNo">invNo</param>
        public P05_MercuryPostScanShipment(string invNo)
        {
            this.InitializeComponent();

            this.invNo = invNo;
            this.comboLoadIndicator.Add("F", "F");
            this.comboLoadIndicator.Add("C", "C");
            this.comboLoadIndicator.SelectedIndex = 0;

            this.comboFactoryAddress.Add("SAP Default", "SAP Default");
            this.comboFactoryAddress.Add("FACTORY", "FACTORY");
            this.comboFactoryAddress.SelectedIndex = 0;

            this.comboSellerAddress.Add("SAP Default", "SAP Default");
            this.comboSellerAddress.Add("FACTORY", "FACTORY");
            this.comboSellerAddress.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();

            if (this.btnCloseUnDo == null)
            {
                return;
            }

            if (this.EditMode)
            {
                this.btnCloseUnDo.Text = "Undo";
                this.btnEditSave.Text = "Save";
            }
            else
            {
                this.btnCloseUnDo.Text = "Close";
                this.btnEditSave.Text = "Edit";
            }
        }

        private void Query()
        {
            string sqlGetData = $@"
select  ns.ShipmentNo
        ,ns.ShippingDate
        ,ns.TCPCode
        ,ns.LoadIndicator
        ,ns.TrackingNo
        ,ns.LSPBookingNumber
        ,ns.FactoryInvoiceNbr
        ,ns.FactoryInvoiceDate
        ,ns.FSPCode
        ,ns.LCReferenceNbr
        ,ns.QAReferenceNbr
        ,l.NikeLSPCode
        ,l.Abb
        ,np.TCPLocation
        ,nps.FSPDesc
from    NikePostScanShipment ns with (nolock)
inner join  GMTBooking g with (nolock) on g.ID = ns.InvNo
left join   LocalSupp l with (nolock) on l.ID = g.Forwarder
left join   NikePortCityList np with (nolock) on np.TCPCode = ns.TCPCode
left join   NikeFSPList nps with (nolock) on nps.FSPCode = ns.FSPCode
where   ns.InvNo = '{this.invNo}'
";

            DataTable dtResult;

            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtResult.Rows.Count == 0)
            {
                return;
            }

            this.displayShipmentNo.Text = dtResult.Rows[0]["ShipmentNo"].ToString();
            this.displayNikeLSPCode.Text = dtResult.Rows[0]["NikeLSPCode"].ToString();
            this.displayLSPDesc.Text = dtResult.Rows[0]["Abb"].ToString();
            this.dateShippingDate.Value = MyUtility.Convert.GetDate(dtResult.Rows[0]["ShippingDate"]);
            this.txtPortOrgin.Text = dtResult.Rows[0]["TCPCode"].ToString();
            this.displayPortOrginDesc.Text = dtResult.Rows[0]["TCPLocation"].ToString();
            this.comboLoadIndicator.SelectedValue = dtResult.Rows[0]["LoadIndicator"].ToString();
            this.txtTrackingContainer.Text = dtResult.Rows[0]["TrackingNo"].ToString();
            this.txtLSPBookingNumber.Text = dtResult.Rows[0]["LSPBookingNumber"].ToString();
            this.txtFactoryInvoiceNbr.Text = dtResult.Rows[0]["FactoryInvoiceNbr"].ToString();
            this.dateFactoryInvoiceDate.Value = MyUtility.Convert.GetDate(dtResult.Rows[0]["FactoryInvoiceDate"]);
            this.txtFSP.Text = dtResult.Rows[0]["FSPCode"].ToString();
            this.displayFSPDesc.Text = dtResult.Rows[0]["FSPDesc"].ToString();
            this.txtLCReferenceNbr.Text = dtResult.Rows[0]["LCReferenceNbr"].ToString();
            this.txtQAReferenceNbr.Text = dtResult.Rows[0]["QAReferenceNbr"].ToString();

            if (MyUtility.Check.Empty(this.displayShipmentNo.Text))
            {
                this.btnCreateUpdateShipment.Text = "Create Shipment";
            }
            else
            {
                this.btnCreateUpdateShipment.Text = "Update Shipment";
            }
        }

        private void TxtPortOrgin_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlNikePortCityList = "select TCPCode, TCPLocation from NikePortCityList with (nolock) where junk = 0";
            SelectItem selectItem = new SelectItem(sqlNikePortCityList, null, string.Empty, headercaptions: "TCP Code,TCP Location");

            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            this.txtPortOrgin.Text = selectItem.GetSelecteds()[0]["TCPCode"].ToString();
            this.displayPortOrginDesc.Text = selectItem.GetSelecteds()[0]["TCPLocation"].ToString();
        }

        private void TxtPortOrgin_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtPortOrgin.OldValue == this.txtPortOrgin.Text)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.txtPortOrgin.Text))
            {
                this.displayPortOrginDesc.Text = string.Empty;
                return;
            }

            string portOrginDesc = MyUtility.GetValue.Lookup($"select TCPLocation from NikePortCityList with (nolock) where TCPCode = '{this.txtPortOrgin.Text}' and junk = 0");
            this.displayPortOrginDesc.Text = portOrginDesc;

            if (MyUtility.Check.Empty(portOrginDesc))
            {
                MyUtility.Msg.WarningBox($"<{this.txtPortOrgin.Text}> not exist in Nike Port City List.");
                e.Cancel = true;
                return;
            }
        }

        private void OnlyEnglishNumber_TextChanged(object sender, EventArgs e)
        {
            Win.UI.TextBox targetTextBox = (Win.UI.TextBox)sender;

            if (MyUtility.Check.Empty(targetTextBox.Text))
            {
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(targetTextBox.Text, "^[a-zA-Z0-9]+$"))
            {
                targetTextBox.Text = targetTextBox.Text.Remove(targetTextBox.Text.Length - 1);
            }
        }

        private void TxtFSP_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlNikeFSPList = "select FSPCode, FSPDesc from NikeFSPList with (nolock) where junk = 0";
            SelectItem selectItem = new SelectItem(sqlNikeFSPList, null, string.Empty, headercaptions: "FSP Code,FSP Desc");

            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            this.txtFSP.Text = selectItem.GetSelecteds()[0]["FSPCode"].ToString();
            this.displayFSPDesc.Text = selectItem.GetSelecteds()[0]["FSPDesc"].ToString();
        }

        private void TxtFSP_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtFSP.OldValue == this.txtFSP.Text)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.txtFSP.Text))
            {
                this.displayFSPDesc.Text = string.Empty;
                return;
            }

            string fSPDesc = MyUtility.GetValue.Lookup($"select FSPDesc from NikeFSPList with (nolock) where FSPCode = '{this.txtFSP.Text}' and junk = 0");
            this.displayFSPDesc.Text = fSPDesc;

            if (MyUtility.Check.Empty(fSPDesc))
            {
                MyUtility.Msg.WarningBox($"<{this.txtFSP.Text}> not exist in Nike FSP List.");
                e.Cancel = true;
                return;
            }
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            // Clck Edit
            if (!this.EditMode)
            {
                this.EditMode = true;
                return;
            }

            #region Click Save
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@InvNo", this.invNo),
                new SqlParameter("@ShipmentNo", this.displayShipmentNo.Text),
                new SqlParameter("@ShippingDate", this.dateShippingDate.Value != null ? (object)this.dateShippingDate.Value : DBNull.Value),
                new SqlParameter("@TCPCode", this.txtPortOrgin.Text),
                new SqlParameter("@LoadIndicator", this.comboLoadIndicator.Text),
                new SqlParameter("@TrackingNo", this.txtTrackingContainer.Text),
                new SqlParameter("@LSPBookingNumber", this.txtLSPBookingNumber.Text),
                new SqlParameter("@FactoryInvoiceNbr", this.txtFactoryInvoiceNbr.Text),
                new SqlParameter("@FactoryInvoiceDate", this.dateFactoryInvoiceDate.Value != null ? (object)this.dateFactoryInvoiceDate.Value : DBNull.Value),
                new SqlParameter("@FSPCode", this.txtFSP.Text),
                new SqlParameter("@LCReferenceNbr", this.txtLCReferenceNbr.Text),
                new SqlParameter("@QAReferenceNbr", this.txtQAReferenceNbr.Text),
            };

            string sqlSaveNikePostScanShipment = @"
if exists(select 1 from NikePostScanShipment where InvNo = @InvNo)
begin
    update  NikePostScanShipment
    set ShipmentNo          = @ShipmentNo
        ,ShippingDate       = @ShippingDate
        ,TCPCode            = @TCPCode
        ,LoadIndicator      = @LoadIndicator
        ,TrackingNo         = @TrackingNo
        ,LSPBookingNumber   = @LSPBookingNumber
        ,FactoryInvoiceNbr  = @FactoryInvoiceNbr
        ,FactoryInvoiceDate = @FactoryInvoiceDate
        ,FSPCode            = @FSPCode
        ,LCReferenceNbr     = @LCReferenceNbr
        ,QAReferenceNbr     = @QAReferenceNbr
    where   InvNo = @InvNo
end
else
begin
    insert into NikePostScanShipment(InvNo
                                    ,ShipmentNo
                                    ,ShippingDate
                                    ,TCPCode
                                    ,LoadIndicator
                                    ,TrackingNo
                                    ,LSPBookingNumber
                                    ,FactoryInvoiceNbr
                                    ,FactoryInvoiceDate
                                    ,FSPCode
                                    ,LCReferenceNbr
                                    ,QAReferenceNbr)
    values( @InvNo
             ,@ShipmentNo
             ,@ShippingDate
             ,@TCPCode
             ,@LoadIndicator
             ,@TrackingNo
             ,@LSPBookingNumber
             ,@FactoryInvoiceNbr
             ,@FactoryInvoiceDate
             ,@FSPCode
             ,@LCReferenceNbr
             ,@QAReferenceNbr)
end
";

            DualResult result = DBProxy.Current.Execute(null, sqlSaveNikePostScanShipment, listPar);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.EditMode = false;
            this.Query();
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            // Close
            if (!this.EditMode)
            {
                this.Close();
            }
            else
            {
                // Undo
                this.EditMode = false;
                this.Query();
                return;
            }
        }

        private void BtnCreateUpdateShipment_Click(object sender, EventArgs e)
        {
            DualResult result;
            string buttonAction = this.btnCreateUpdateShipment.Text;
            if (MyUtility.Check.Empty(this.displayShipmentNo.Text))
            {
                result = this.CreateShipment();
            }
            else
            {
                result = this.UpdateShipment();
            }

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox($"{buttonAction} successfully");
        }

        private DualResult CreateShipment()
        {
            if (MyUtility.Check.Empty(this.displayNikeLSPCode.Text) ||
                MyUtility.Check.Empty(this.comboLoadIndicator.Text) ||
                MyUtility.Check.Empty(this.txtTrackingContainer.Text))
            {
                return new DualResult(false, "<LSP>, <Load Indicator>, <Tracking#/Container#> cannot be empty.");
            }

            string sqlCheckPackingMercury = $@"
select  pd.CustCTN2
from    Packinglist p with (nolock)
inner join Packinglist_Detail pd with (nolock) on pd.ID = p.ID
where p.InvNo = '{this.invNo}'
";

            DataTable dtMercuryCtn;
            DualResult result = DBProxy.Current.Select(null, sqlCheckPackingMercury, out dtMercuryCtn);

            if (!result)
            {
                return result;
            }

            if (dtMercuryCtn.Rows.Count == 0)
            {
                return new DualResult(false, "Packing data not exists");
            }

            if (dtMercuryCtn.AsEnumerable().Any(s => MyUtility.Check.Empty(s["CustCTN2"])))
            {
                return new DualResult(false, "<Cust CTN#> cannot be empty, please back to Packing List and use function of 'Mercury - Upload PL' to get Cust CTN#.");
            }

            string shipmentNumber;
            RequestShipmentScannedCartonUploadLSP.Input shipmentInfo = new RequestShipmentScannedCartonUploadLSP.Input()
            {
                LSPCode = this.displayNikeLSPCode.Text,
                ShipmentDate = this.dateShippingDate.Value.HasValue ? this.dateShippingDate.Value.ToStringEx("yyyy-MM-dd") : null,
                PortofOrigin = this.txtPortOrgin.Text,
                LoadIndicator = this.comboLoadIndicator.Text,
                Trackingnumber = this.txtTrackingContainer.Text,
                LSPBookingNumber = this.txtLSPBookingNumber.Text,
                InvoiceNumber = this.txtFactoryInvoiceNbr.Text,
                InvoiceDate = this.dateFactoryInvoiceDate.Value.HasValue ? this.dateFactoryInvoiceDate.Value.ToStringEx("yyyy-MM-dd") : null,
                ScannerID = "Admin",
                CartonNumberList = new CartonNumberList()
                {
                    CartonNumberDetails = dtMercuryCtn.AsEnumerable().Select(s => new CartonNumberDetails() { CartonNumber = s["CustCTN2"].ToString() }).ToList(),
                },
            };

            result = WebServiceNikeMercury.StaticService.ShipmentScannedCartonUploadLSP(shipmentInfo, out shipmentNumber);

            if (!result)
            {
                return result;
            }

            if (MyUtility.Check.Empty(shipmentNumber))
            {
                return new DualResult(false, $"Mercury not return ShipmentNumber, {result.Description}");
            }

            string updateShipmentNumber = $@" update NikePostScanShipment set ShipmentNo = '{shipmentNumber}' where InvNo = '{this.invNo}'";

            result = DBProxy.Current.Execute(null, updateShipmentNumber);

            if (!result)
            {
                return result;
            }

            this.Query();

            if (MyUtility.Check.Empty(this.txtFSP.Text))
            {
                return new DualResult(true);
            }

            // 有維護FSP，繼續作UpdateShipment
            return this.UpdateShipment();
        }

        private DualResult UpdateShipment()
        {
            if (MyUtility.Check.Empty(this.displayShipmentNo.Text) ||
                MyUtility.Check.Empty(this.txtTrackingContainer.Text) ||
                MyUtility.Check.Empty(this.txtFSP.Text))
            {
                return new DualResult(false, "<Shipment Nbr>, <Tracking#/Container#>, <FSP> cannot be empty.");
            }

            RequestShipmentShippingDetailsUpdateLSP.Input shipmentInfo = new RequestShipmentShippingDetailsUpdateLSP.Input()
            {
                LSPCode = this.displayNikeLSPCode.Text,
                ShippingDetail = new RequestShipmentShippingDetailsUpdateLSP.ShippingDetail()
                {
                    FactoryHubShipmentNumber = this.displayShipmentNo.Text,
                    ShipmentDate = this.dateShippingDate.Value.HasValue ? this.dateShippingDate.Value.ToStringEx("yyyy-MM-dd") : null,
                    PortofOrigin = this.txtPortOrgin.Text,
                    LoadIndicator = this.comboLoadIndicator.Text,
                    Trackingnumber = this.txtTrackingContainer.Text,
                    FSPCode = this.txtFSP.Text,
                },
                FinancialList = new FinancialList()
                {
                    FinancialDetails = new List<FinancialDetails>()
                    {
                        new FinancialDetails()
                        {
                            LSPBookingNumber = this.txtLSPBookingNumber.Text,
                            InvoiceNumber = this.txtFactoryInvoiceNbr.Text,
                            LCReferenceNbr = this.txtLCReferenceNbr.Text,
                            OAReferenceNbr = this.txtQAReferenceNbr.Text,
                        },
                    },
                },
            };

            DualResult result = WebServiceNikeMercury.StaticService.ShipmentShippingDetailsUpdateLSP(shipmentInfo);

            if (!result)
            {
                return result;
            }

            return new DualResult(true);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            return;
            RequestShipmentCommercialDocumentsFileLocationPDF.Input docInfo = new RequestShipmentCommercialDocumentsFileLocationPDF.Input()
            {
                ShipmentNo = this.displayShipmentNo.Text
            };
            DualResult result = WebServiceNikeMercury.StaticService.ShipmentCommercialDocumentsBinaryArrayPDF(docInfo);
        }
    }
}
