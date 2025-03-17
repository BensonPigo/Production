using Ict;
using Ict.Win;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P07_QRCodeSticker
    /// </summary>
    public partial class WH_Receive_QRCodeSticker : Win.Subs.Base
    {
        private DataTable dt_QRCodeSticker;
        private string printPaper;
        private string printType;
        private string callFrom;

        // P07 或者 P18
        private bool IsP07;
        private bool IsP18;
        private string rgCode;

        /// <summary>
        /// P07_QRCodeSticker
        /// </summary>
        /// <param name="dtSource">資料源</param>
        /// <param name="printPaper">使用哪種紙列印</param>
        /// <param name="printType">用哪種格式列印</param>
        /// <param name="callFrom">來源</param>
        /// <inheritdoc/>
        public WH_Receive_QRCodeSticker(DataTable dtSource, string printPaper, string printType, string callFrom)
        {
            this.InitializeComponent();
            this.dt_QRCodeSticker = dtSource;
            this.printPaper = printPaper;
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

            if (this.IsP07)
            {
                MyUtility.Tool.SetupCombox(this.comboFilterQRCode, 1, 1, "All,Create by PMS,Not create by PMS");
                this.comboFilterQRCode.Text = "Create by PMS";
                this.grid1.ColumnHeaderMouseClick += this.Grid1_ColumnHeaderMouseClick;
                this.RadioPanel1_ValueChanged(null, null);
            }
            else if (this.IsP18)
            {
                MyUtility.Tool.SetupCombox(this.comboFilterQRCode, 1, 1, $"All,Create by {this.rgCode},Not Create by {this.rgCode}");
                this.comboFilterQRCode.Text = $"Create by {this.rgCode}";
                this.grid1.ColumnHeaderMouseClick += this.Grid1_ColumnHeaderMouseClick;
                this.RadioPanel1_ValueChanged(null, null);
            }
            else if (this.callFrom == "P10" || this.callFrom == "P13" || this.callFrom == "P16" || this.callFrom == "P19" || this.callFrom == "P62")
            {
                MyUtility.Tool.SetupCombox(this.comboFilterQRCode, 1, 1, "All,Partial Release");
                this.comboFilterQRCode.Text = "Partial Release";
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
                && dtPrint.AsEnumerable().Any(row => System.Convert.ToBoolean(row["Sel"])))
            {
                DataView dv = dtPrint.DefaultView;
                dv.Sort = ((DataTable)this.listControlBindingSource.DataSource).DefaultView.Sort;
                DataTable sortedtable1 = dv.ToTable();

                var barcodeDatas = sortedtable1.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["Sel"])).ToList();

                #region Print
                this.ShowWaitMessage("Data Loading ...");

                switch (this.printPaper)
                {
                    case "Sticker":
                        PrintQRCode_RDLC(barcodeDatas, this.printType);
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

                        break;
                    case "Paper":
                        PrintQRCode_A4(barcodeDatas, this.printType);
                        break;
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
        public static void PrintQRCode_RDLC(List<DataRow> barcodeDatas, string printType, string form = "")
        {
            int qrCodeWidth;
            string rdlcName;
            switch (printType)
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

            ReportDefinition report = new ReportDefinition
            {
                ReportDataSource = GetPrintDatas(barcodeDatas, form, qrCodeWidth),
            };

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

        /// <summary>
        /// Print QRCode For A4
        /// </summary>
        /// <param name="barcodeDatas">基本資料</param>
        /// <param name="printType">直式還橫式</param>
        /// <param name="form">來源</param>
        public static void PrintQRCode_A4(List<DataRow> barcodeDatas, string printType, string form = "")
        {
            // A4 是用 7*7的方式列印，大小是90
            List<P21_PrintBarcode_Data> barcode_Datas = GetPrintDatas(barcodeDatas, form, 90);
            int maxRow = barcode_Datas.Count();
            int maxPage = (maxRow / 6) + (maxRow % 6 == 0 ? 0 : 1);
            string xltxName = "Warehouse_PrintBarcode_A4";
            string strXltName = Env.Cfg.XltPathDir + $@"\{xltxName}.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(strXltName); // 預先開啟excel app
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            Excel.Worksheet worksheet = workbook.Worksheets[printType];
            excelApp.Visible = false; // 隱藏 Excel 應用程式
            excelApp.DisplayAlerts = false; // 停用警告訊息
            switch (printType)
            {
                case "Horizontal": // 橫式
                    // 1 27 58 89
                    for (int i = 0; i < maxPage - 1; i++)
                    {
                        int nowRow = i == 0 ? 27 : 27 + (31 * i);
                        Excel.Range rangeToCopy = worksheet.get_Range("A1:A21").EntireRow; // 選取要被複製的資料
                        Excel.Range rangeToPaste = worksheet.get_Range($"A{nowRow}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rangeToCopy.Copy(Type.Missing);
                        rangeToPaste.PasteSpecial(Excel.XlPasteType.xlPasteAll);
                    }

                    for (int j = 0; j <= maxRow - 1; j++)
                    {
                        int nowRow = (j / 6) == 0 ? 1 :
                                     (j / 6) == 1 ? 27 : 27 + (31 * (j / 6));
                        nowRow += ((j / 3) % 2) * 11;
                        int nowCol = 1 + ((j % 3) * 7);
                        P21_PrintBarcode_Data data = barcode_Datas[j];

                        Excel.Range cellQrCodeLeft = worksheet.Cells[nowRow + 4, nowCol];
                        Excel.Range cellQrCodeRight = worksheet.Cells[nowRow + 4, nowCol + 4];
                        string imgPath = ExcelPrg.ConvertImgPath(data.Image);
                        int colorLength = data.Color.Length;

                        worksheet.Cells[nowRow, nowCol] = data.SP;
                        worksheet.Cells[nowRow, nowCol + 3] = data.Seq;
                        worksheet.Cells[nowRow + 1, nowCol] = data.GW;
                        worksheet.Cells[nowRow + 2, nowCol] = data.AW;
                        worksheet.Cells[nowRow + 1, nowCol + 3] = data.StockType;
                        worksheet.Cells[nowRow + 2, nowCol + 3] = data.Dyelot;
                        worksheet.Cells[nowRow + 3, nowCol] = data.Refno;
                        worksheet.Cells[nowRow + 3, nowCol + 3] = data.Location;

                        worksheet.Cells[nowRow + 4, nowCol + 2] = data.FactoryID;
                        worksheet.Shapes.AddPicture(imgPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellQrCodeLeft.Left + 5, cellQrCodeLeft.Top + 5, 85, 85);
                        worksheet.Shapes.AddPicture(imgPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellQrCodeRight.Left + 5, cellQrCodeRight.Top + 5, 85, 85);

                        worksheet.Cells[nowRow + 7, nowCol] = data.Roll;
                        worksheet.Cells[nowRow + 7, nowCol + 3] = data.Dyelot;

                        worksheet.Cells[nowRow + 8, nowCol] = data.Color;
                        worksheet.Cells[nowRow + 8, nowCol].Font.Size = colorLength > 36 ? 5 : colorLength > 31 ? 6.5 : 7;
                        worksheet.Cells[nowRow + 8, nowCol + 3] = data.Qty;

                        worksheet.Cells[nowRow + 9, nowCol] = data.WhseArrival + data.Relaxtime;
                    }

                    break;
                case "Straight": // 直式
                    // 1 40 86 132
                    for (int i = 0; i < maxPage - 1; i++)
                    {
                        int nowRow = i == 0 ? 40 : 40 + (46 * i);
                        Excel.Range rangeToCopy = worksheet.get_Range("A1:A32").EntireRow; // 選取要被複製的資料
                        Excel.Range rangeToPaste = worksheet.get_Range($"A{nowRow}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rangeToCopy.Copy(Type.Missing);
                        rangeToPaste.PasteSpecial(Excel.XlPasteType.xlPasteAll);
                    }

                    for (int i = 0; i <= maxPage - 1; i++)
                    {
                        for (int j = 0; j <= maxRow - 1; j++)
                        {
                            int nowRow = (j / 6) == 0 ? 1 :
                                            (j / 6) == 1 ? 40 : 40 + (46 * (j / 6));
                            nowRow += ((j / 2) % 3) * 11;
                            int nowCol = 1 + ((j % 2) * 7);
                            P21_PrintBarcode_Data data = barcode_Datas[j];

                            Excel.Range cellQrCodeLeft = worksheet.Cells[nowRow + 4, nowCol];
                            Excel.Range cellQrCodeRight = worksheet.Cells[nowRow + 4, nowCol + 4];
                            string imgPath = ExcelPrg.ConvertImgPath(data.Image);
                            int colorLength = data.Color.Length;

                            worksheet.Cells[nowRow, nowCol] = data.SP;
                            worksheet.Cells[nowRow, nowCol + 3] = data.Seq;
                            worksheet.Cells[nowRow + 1, nowCol] = data.GW;
                            worksheet.Cells[nowRow + 2, nowCol] = data.AW;
                            worksheet.Cells[nowRow + 1, nowCol + 3] = data.StockType;
                            worksheet.Cells[nowRow + 2, nowCol + 3] = data.Dyelot;
                            worksheet.Cells[nowRow + 3, nowCol] = data.Refno;
                            worksheet.Cells[nowRow + 3, nowCol + 3] = data.Location;

                            worksheet.Cells[nowRow + 4, nowCol + 2] = data.FactoryID;
                            worksheet.Shapes.AddPicture(imgPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellQrCodeLeft.Left + 5, cellQrCodeLeft.Top + 5, 85, 85);
                            worksheet.Shapes.AddPicture(imgPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellQrCodeRight.Left + 5, cellQrCodeRight.Top + 5, 85, 85);

                            worksheet.Cells[nowRow + 7, nowCol] = data.Roll;
                            worksheet.Cells[nowRow + 7, nowCol + 3] = data.Dyelot;

                            worksheet.Cells[nowRow + 8, nowCol] = data.Color;
                            worksheet.Cells[nowRow + 8, nowCol].Font.Size = colorLength > 36 ? 5 : colorLength > 31 ? 6.5 : 7;
                            worksheet.Cells[nowRow + 8, nowCol + 3] = data.Qty;

                            worksheet.Cells[nowRow + 9, nowCol] = data.WhseArrival + data.Relaxtime;
                        }
                    }

                    break;
            }

            worksheet.Select();
            worksheet.Cells[1, 1].Select();

            // 刪除其他sheet
            foreach (Excel.Worksheet othersheet in workbook.Worksheets)
            {
                if (othersheet.Name != printType)
                {
                    othersheet.Delete();
                    Marshal.ReleaseComObject(othersheet);
                }
            }

            string strExcelName = Class.MicrosoftFile.GetName(xltxName);
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(worksheet);
            strExcelName.OpenFile();
        }

        private void ComboFilterQRCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboFilterQRCode.SelectedIndex < 0)
            {
                return;
            }

            switch (this.comboFilterQRCode.SelectedIndex)
            {
                case 1:
                    if (this.IsP07 || this.IsP18)
                    {
                        this.listControlBindingSource.Filter = "IsQRCodeCreatedByPMS = true";
                    }
                    else
                    {
                        this.listControlBindingSource.Filter = "MINDQRCode <> From_OldBarcode";
                    }

                    break;
                case 2:
                    this.listControlBindingSource.Filter = "IsQRCodeCreatedByPMS = false";
                    break;
                default:
                    this.listControlBindingSource.Filter = string.Empty;
                    break;
            }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "<暫止>")]
        private static List<P21_PrintBarcode_Data> GetPrintDatas(List<DataRow> barcodeDatas, string form, int qrCodeWidth)
        {
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

            List<P21_PrintBarcode_Data> dataRows = barcodeDatas
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
            return dataRows;
        }
    }
}