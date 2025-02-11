using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P07_QRCodeSticker
    /// </summary>
    public partial class WH_Receive_QRCodeSticker : Win.Subs.Base
    {
        private DataTable dt_QRCodeSticker;
        private string printType;
        private string callFrom;

        // P07 或者 P18
        private bool IsP07;
        private bool IsP18;
        private string rgCode;

        /// <summary>
        /// P07_QRCodeSticker
        /// </summary>
        /// <param name="dtSource">dtSource</param>
        /// <param name="printType">printType</param>
        /// <param name="callFrom">callFrom</param>
        /// <inheritdoc/>
        public WH_Receive_QRCodeSticker(DataTable dtSource, string printType, string callFrom)
        {
            this.InitializeComponent();
            this.dt_QRCodeSticker = dtSource;
            this.printType = printType;
            this.callFrom = callFrom;
            this.IsP07 = callFrom == "P07";
            this.IsP18 = callFrom == "P18";
            this.labSortBy.Visible = this.IsP07;
            this.radioPanel1.Visible = this.IsP07;
            this.listControlBindingSource.DataSource = dtSource;
            this.rgCode = MyUtility.GetValue.Lookup("select RgCode from system");
            this.dt_QRCodeSticker.Columns.Add("IsQRCodeCreatedByPMS", typeof(bool));
            foreach (DataRow dr in this.dt_QRCodeSticker.Rows)
            {
                if (this.IsP18)
                {
                    dr["IsQRCodeCreatedByPMS"] = dr["MINDQRCode"].ToString().IsQRCodeCreatedByPMS() && dr["MINDQRCode"].ToString().Left(3) == this.rgCode;
                }
                else
                {
                    dr["IsQRCodeCreatedByPMS"] = dr["MINDQRCode"].ToString().IsQRCodeCreatedByPMS();
                }
            }
        }

        private void Grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string strSort = ((DataTable)this.listControlBindingSource.DataSource).DefaultView.Sort.ToString();
            this.radiobySP.Checked = false;
            this.radioEncodeSeq.Checked = false;
            if (this.listControlBindingSource.DataSource != null)
            {
                if (MyUtility.Check.Empty(((DataTable)this.listControlBindingSource.DataSource).DefaultView.Sort))
                {
                    ((DataTable)this.listControlBindingSource.DataSource).DefaultView.Sort = $"{strSort}";
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Set Grid Columns
            this.grid1.IsEditingReadOnly = false;

            if (this.IsP07)
            {
                this.Helper.Controls.Grid.Generator(this.grid1)
                                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                                .Text("poid", header: "SP#", iseditingreadonly: true)
                                .Text("seq", header: "Seq", iseditingreadonly: true)
                                .Text("fabrictype", header: "Material" + Environment.NewLine + "Type", iseditingreadonly: true)
                                .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                                .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(7), iseditingreadonly: true)
                                .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(7), iseditingreadonly: true)
                                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), iseditingreadonly: true)
                                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                                .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(9), decimal_places: 2, iseditingreadonly: true)
                                .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)
                                .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true)
                                .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", decimal_places: 2, width: Widths.AnsiChars(6), iseditingreadonly: true)
                                .Text("MINDQRCode", header: "QR Code", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
            }
            else if (this.IsP18)
            {
                this.Helper.Controls.Grid.Generator(this.grid1)
                                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                                .Text("POID", header: "SP#", iseditingreadonly: true)
                                .Text("SEQ", header: "Seq", iseditingreadonly: true)
                                .Text("FabricType", header: "Fabric" + Environment.NewLine + "Type", iseditingreadonly: true)
                                .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(7), iseditingreadonly: true)
                                .Numeric("ActualWeight", header: "Act.(kg)", width: Widths.AnsiChars(7), iseditingreadonly: true)
                                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), iseditingreadonly: true)
                                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                                .Numeric("StockQty", header: "In Qty", width: Widths.AnsiChars(9), decimal_places: 2, iseditingreadonly: true)
                                .Text("StockUnit", header: "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)
                                .Text("MINDQRCode", header: "QR Code", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
            }
            else
            {
                this.Helper.Controls.Grid.Generator(this.grid1)
                    .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                    .Text("poid", header: "SP#", iseditingreadonly: true)
                    .Text("seq", header: "Seq", iseditingreadonly: true)
                    .Text("fabrictype", header: "Material" + Environment.NewLine + "Type", iseditingreadonly: true)
                    .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), iseditingreadonly: true)
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Numeric("StockQty", header: "Issue Qty", width: Widths.AnsiChars(9), decimal_places: 2, iseditingreadonly: true)
                    .Text("MINDQRCode", header: "Issue QR Code", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    ;
            }

            for (int i = 0; i < this.grid1.Columns.Count; i++)
            {
                this.grid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = (DataTable)this.listControlBindingSource.DataSource;
            if (dtPrint != null
                && dtPrint.AsEnumerable().Any(row => Convert.ToBoolean(row["Sel"])))
            {
                DataView dv = dtPrint.DefaultView;
                dv.Sort = ((DataTable)this.listControlBindingSource.DataSource).DefaultView.Sort;
                DataTable sortedtable1 = dv.ToTable();

                var barcodeDatas = sortedtable1.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["Sel"])).ToList();
                string type = this.printType;
                #region Print
                this.ShowWaitMessage("Data Loading ...");

                PrintQRCode_RDLC(barcodeDatas, type);

                if (this.IsP07 || this.IsP18)
                {
                    string ukeys = barcodeDatas.Select(s => s["Ukey"].ToString()).JoinToString(",");
                    WHTableName detailTableName = Prgs.GetWHDetailTableName(this.callFrom);
                    string sqlcmd = $@"update {detailTableName} set QRCode_PrintDate = Getdate() where ukey in ({ukeys}) and QRCode_PrintDate is null";
                    DualResult result = DBProxy.Current.Execute(null, sqlcmd);
                    if (!result)
                    {
                        this.ShowErr(result);
                    }
                }

                this.HideWaitMessage();
                #endregion
            }
            else
            {
                MyUtility.Msg.InfoBox("Select data first.");
            }
        }

        /// <inheritdoc/>
        public static void PrintQRCode_RDLC(List<DataRow> barcodeDatas, string type, string form = "")
        {
            int qrCodeWidth;
            string rdlcName;
            switch (type)
            {
                case "5X5":
                    qrCodeWidth = 90;
                    rdlcName = "P21_PrintBarcode5.rdlc";
                    break;
                case "7X7":
                    qrCodeWidth = 90;
                    rdlcName = "P21_PrintBarcode7.rdlc";
                    break;
                default:
                    qrCodeWidth = 100;
                    rdlcName = "P21_PrintBarcode10.rdlc";
                    break;
            }

            string qrcode;
            string qty;
            switch (form)
            {
                case "P21":
                    qrcode = "Barcode";
                    qty = "StockQty";
                    break;
                case "P22":
                case "P23":
                    qrcode = "Barcode";
                    qty = "Qty";
                    break;
                default:
                    qrcode = "MINDQRCode";
                    qty = "StockQty";
                    break;
            }

            ReportDefinition report = new ReportDefinition();
            report.ReportDataSource = barcodeDatas
                .Select(s => new P21_PrintBarcode_Data()
                {
                    SP = "SP#:" + MyUtility.Convert.GetString(s["PoId"]),
                    Seq = "SEQ:" + (form == "P21" ? MyUtility.Convert.GetString(s["SEQ1"]) + "-" + MyUtility.Convert.GetString(s["SEQ2"]) : MyUtility.Convert.GetString(s["SEQ"])),
                    GW = "GW:" + (s.IsNull("Weight") ? " " : MyUtility.Convert.GetString(s["Weight"]) + "KG"),
                    AW = "AW:" + (s.IsNull("ActualWeight") ? " " : MyUtility.Convert.GetString(s["ActualWeight"]) + "KG"),
                    Location = "Lct:" + MyUtility.Convert.GetString(s["Location"]),
                    Refno = "REF#:" + MyUtility.Convert.GetString(s["RefNo"]),
                    Roll = "Roll#:" + MyUtility.Convert.GetString(s["Roll"]),
                    Color = "Color:" + MyUtility.Convert.GetString(s["ColorID"]),
                    Dyelot = "Lot#:" + MyUtility.Convert.GetString(s["Dyelot"]),
                    Qty = "Yd#:" + MyUtility.Convert.GetString(s[qty]),
                    FactoryID = MyUtility.Convert.GetString(s["FactoryID"]),
                    StockType = "Stock Type:" + MyUtility.Convert.GetString(s["StockTypeName"]),
                    StyleID = "ST:" + MyUtility.Convert.GetString(s["StyleID"]),
                    WhseArrival = "Arrive WH Date:" + (MyUtility.Check.Empty(s["WhseArrival"]) ? string.Empty : ((DateTime)s["WhseArrival"]).ToString("yyyy/MM/dd")),
                    Relaxtime = "RELAXATION:" + MyUtility.Convert.GetFloat(s["Relaxtime"]) + "HRS",
                    Image = Prgs.ImageToByte(MyUtility.Convert.GetString(s[qrcode]).ToBitmapQRcode(qrCodeWidth, qrCodeWidth)),
                }).ToList();

            DualResult result = ReportResources.ByEmbeddedResource(typeof(P21_PrintBarcode_Data), rdlcName, out IReportResource reportresource);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            report.ReportResource = reportresource;

            // 開啟 report view 直接列印
            new Win.Subs.ReportView(report) { DirectPrint = true }.Show();
        }

        private void RadioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (this.IsP07 && this.listControlBindingSource.DataSource != null)
            {
                if (this.radioPanel1.Value == "1")
                {
                    // SP#, Seq, Roll, Dyelot
                    ((DataTable)this.listControlBindingSource.DataSource).DefaultView.Sort = @"SortCmbPOID, SortCmbSeq1, SortCmbSeq2, SortCmbRoll, SortCmbDyelot, Unoriginal, POID, SEQ, Roll, Dyelot ";
                }
                else
                {
                    // 使用OnDetailSelectCommandPrepare預設的排序(Encode Seq)
                    ((DataTable)this.listControlBindingSource.DataSource).DefaultView.Sort = string.Empty;
                }
            }
        }
    }
}