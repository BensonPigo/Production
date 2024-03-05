using Ict;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Class.Command;
using Sci.Production.Prg.Entity.NikeMercury;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Sci.Production.Prg.Entity.NikeMercury.RequestShipmentScannedCartonUploadLSP;

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

            string sqlGetLoadIndicator = $@"
select  [LoadIndicator] =   case CYCFS    when 'CFS-CY'  then 'C'
										  when 'CFS-CFS' then 'C'
                                          when 'CY-CY'  then 'F'
                            else '' end
from GMTBooking with (nolock) where ID = '{invNo}'
";

            this.displayLoadIndicator.Text = MyUtility.GetValue.Lookup(sqlGetLoadIndicator, "Production");

            this.invNo = invNo;

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
        ,ns.LoadIndicator
        ,ns.TrackingNo
        ,ns.LSPBookingNumber
        ,l.NikeLSPCode
        ,l.Abb
        ,np.TCPLocation
        ,nps.FSPDesc
        ,ns.ContainerSealNumber
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
            this.displayLoadIndicator.Text = dtResult.Rows[0]["LoadIndicator"].ToString();
            this.txtTrackingContainer.Text = dtResult.Rows[0]["TrackingNo"].ToString();

            if (MyUtility.Check.Empty(this.displayShipmentNo.Text))
            {
                this.btnCreateUpdateShipment.Text = "Create Shipment";
            }
            else
            {
                this.btnCreateUpdateShipment.Text = "Update Shipment";
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
                new SqlParameter("@LoadIndicator", this.displayLoadIndicator.Text),
                new SqlParameter("@TrackingNo", this.txtTrackingContainer.Text),
            };

            string sqlSaveNikePostScanShipment = @"
if exists(select 1 from NikePostScanShipment where InvNo = @InvNo)
begin
    update  NikePostScanShipment
    set ShipmentNo          = @ShipmentNo
        ,LoadIndicator      = @LoadIndicator
        ,TrackingNo         = @TrackingNo
    where   InvNo = @InvNo
end
else
begin
    insert into NikePostScanShipment(InvNo
                                    ,ShipmentNo
                                    ,LoadIndicator
                                    ,TrackingNo)
    values( @InvNo
             ,@ShipmentNo
             ,@LoadIndicator
             ,@TrackingNo)
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
                MyUtility.Check.Empty(this.displayLoadIndicator.Text) ||
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
                LoadIndicator = this.displayLoadIndicator.Text,
                Trackingnumber = this.txtTrackingContainer.Text,
                ContainerType = " ",
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

            return new DualResult(true);
        }

        private DualResult UpdateShipment()
        {
            if (MyUtility.Check.Empty(this.displayShipmentNo.Text) ||
                MyUtility.Check.Empty(this.txtTrackingContainer.Text))
            {
                return new DualResult(false, "<Shipment Nbr>, <Tracking#/Container#>, <FSP> cannot be empty.");
            }

            RequestShipmentShippingDetailsUpdateLSP.Input shipmentInfo = new RequestShipmentShippingDetailsUpdateLSP.Input()
            {
                LSPCode = this.displayNikeLSPCode.Text,
                ShippingDetail = new RequestShipmentShippingDetailsUpdateLSP.ShippingDetail()
                {
                    FactoryHubShipmentNumber = this.displayShipmentNo.Text,
                    LoadIndicator = this.displayLoadIndicator.Text,
                    Trackingnumber = this.txtTrackingContainer.Text,
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
