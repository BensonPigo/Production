using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P07_QRCodeSticker
    /// </summary>
    public partial class P07_QRCodeSticker : Win.Subs.Base
    {
        private DataTable dtP07_QRCodeSticker;
        private string printType;
        private string callFrom;

        /// <summary>
        /// P07_QRCodeSticker
        /// </summary>
        /// <param name="dtSource">dtSource</param>
        /// <param name="printType">printType</param>
        /// <param name="callFrom">callFrom</param>
        /// <inheritdoc/>
        public P07_QRCodeSticker(DataTable dtSource, string printType, string callFrom = "P07")
        {
            this.InitializeComponent();
            this.dtP07_QRCodeSticker = dtSource;
            this.printType = printType;
            this.callFrom = callFrom;
            this.labSortBy.Visible = callFrom == "P07";
            this.radioPanel1.Visible = callFrom == "P07";
            this.listControlBindingSource.DataSource = dtSource;

            this.dtP07_QRCodeSticker.Columns.Add("IsQRCodeCreatedByPMS", typeof(bool));

            foreach (DataRow dr in this.dtP07_QRCodeSticker.Rows)
            {
                dr["IsQRCodeCreatedByPMS"] = dr["MINDQRCode"].ToString().IsQRCodeCreatedByPMS();
            }

            if (callFrom == "P07")
            {
                MyUtility.Tool.SetupCombox(this.comboFilterQRCode, 1, 1, "All,Create by PMS,Not create by PMS");
                this.comboFilterQRCode.Text = "Create by PMS";
                this.grid1.ColumnHeaderMouseClick += this.Grid1_ColumnHeaderMouseClick;
                this.RadioPanel1_ValueChanged(null, null);
            }
            else
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

            if (this.callFrom == "P07")
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
                                .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(9), iseditingreadonly: true)
                                .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)
                                .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true)
                                .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
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
                    .Numeric("StockQty", header: "Issue Qty", width: Widths.AnsiChars(9), iseditingreadonly: true)
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

                var barcodeDatas = sortedtable1.AsEnumerable().Where(s => (int)s["Sel"] == 1).ToList();
                string type = this.printType;
                #region Print
                this.ShowWaitMessage("Data Loading ...");

                PrintQRCode(barcodeDatas, type);

                if (this.callFrom == "P07")
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
        public static void PrintQRCode(List<DataRow> barcodeDatas, string type, string form = "")
        {
            Word._Application winword = new Word.Application();
            Word._Document document;
            Word.Table tables = null;

            string fileName;
            float otherSize;
            switch (type)
            {
                case "5X5":
                    fileName = "\\Warehouse_P07_Sticker5.dotx";
                    otherSize = (float)6;
                    break;
                case "7X7":
                    fileName = "\\Warehouse_P07_Sticker7.dotx";
                    otherSize = (float)10;
                    break;
                default:
                    fileName = "\\Warehouse_P07_Sticker10.dotx";
                    otherSize = (float)13;
                    break;
            }

            object printFile = Sci.Env.Cfg.XltPathDir + fileName;
            document = winword.Documents.Add(ref printFile);
            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                // 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                for (int j = 1; j < barcodeDatas.Count(); j++)
                {
                    winword.Selection.MoveDown();
                    if (barcodeDatas.Count() > 1)
                    {
                        winword.Selection.InsertAfter(Environment.NewLine);
                        winword.Selection.MoveRight();
                    }

                    winword.Selection.Paste();
                }

                // 填入資料
                int i = 0;
                foreach (var printItem in barcodeDatas)
                {
                    tables = table[i + 1];
                    tables.Cell(1, 1).Range.Text = $"SP#:{printItem["PoId"]}";
                    tables.Cell(1, 2).Range.Text = $"SEQ:{printItem["SEQ"]}";

                    tables.Cell(2, 1).Range.Text = $@"GW:{printItem["Weight"]}KG
AW:{printItem["ActualWeight"]}KG";
                    tables.Cell(2, 2).Range.Text = $"Lct:{printItem["Location"]}";

                    tables.Cell(3, 1).Range.Text = $"REF#:{printItem["RefNo"]}";

                    int qrCodeWidth = type == "10X10" ? 90 : 45;
                    string qrcode = form == "P21" ? "Barcode" : "MINDQRCode";
                    Bitmap oriBitmap = printItem[qrcode].ToString().ToBitmapQRcode(qrCodeWidth, qrCodeWidth);
                    Clipboard.SetImage(oriBitmap);
                    Thread.Sleep(100);
                    tables.Cell(4, 1).Range.Paste();
                    tables.Cell(4, 3).Range.Paste();

                    tables.Cell(4, 2).Range.Text = printItem["FactoryID"].ToString();

                    Word.Paragraph pText;
                    Word.Range range;

                    range = tables.Cell(5, 1).Range;
                    range.Text = $"{printItem["Roll"]}";
                    pText = range.Paragraphs.Add(range);

                    // pText.Range.Bold = 0;
                    pText.Range.Font.Size = otherSize;
                    pText.Range.Text = $"Roll#:";

                    range = tables.Cell(5, 2).Range;
                    range.Text = $"{printItem["Dyelot"]}";
                    pText = range.Paragraphs.Add(range);

                    // pText.Range.Bold = 0;
                    pText.Range.Font.Size = otherSize;
                    pText.Range.Text = $"Lot#:";

                    range = tables.Cell(6, 1).Range;
                    range.Text = $"{printItem["ColorID"]}";
                    pText = range.Paragraphs.Add(range);

                    // pText.Range.Bold = 0;
                    pText.Range.Font.Size = otherSize;
                    pText.Range.Text = $"Color:";

                    range = tables.Cell(6, 2).Range;
                    range.Text = $"{printItem["StockQty"]}";
                    pText = range.Paragraphs.Add(range);

                    // pText.Range.Bold = 0;
                    pText.Range.Text = $"Yd#:";
                    i++;
                }

                // 產生的Word檔不可編輯
                winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyReading);
                winword.Visible = true;
            }
            catch (Exception)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                MyUtility.Msg.WarningBox("Export Word error.");
            }
            finally
            {
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(winword);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void ComboFilterQRCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboFilterQRCode.SelectedIndex < 0)
            {
                return;
            }

            switch (this.comboFilterQRCode.Text)
            {
                case "Create by PMS":
                    this.listControlBindingSource.Filter = "IsQRCodeCreatedByPMS = true";
                    break;
                case "Not create by PMS":
                    this.listControlBindingSource.Filter = "IsQRCodeCreatedByPMS = false";
                    break;
                case "Partial Release":
                    this.listControlBindingSource.Filter = "MINDQRCode <> From_OldBarcode";
                    break;
                default:
                    this.listControlBindingSource.Filter = string.Empty;
                    break;
            }
        }

        private void RadioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (this.callFrom == "P07" && this.listControlBindingSource.DataSource != null)
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
