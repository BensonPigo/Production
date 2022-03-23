using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// P07_QRCodeSticker
        /// </summary>
        /// <param name="dtSource">dtSource</param>
        /// <param name="printType">printType</param>
        public P07_QRCodeSticker(DataTable dtSource, string printType)
        {
            this.InitializeComponent();
            this.dtP07_QRCodeSticker = dtSource;
            this.printType = printType;
            this.listControlBindingSource.DataSource = dtSource;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Set Grid Columns
            this.grid1.IsEditingReadOnly = false;
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
                ;

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
                var barcodeDatas = dtPrint.AsEnumerable().Where(s => (int)s["Sel"] == 1);

                #region Print
                this.ShowWaitMessage("Data Loading ...");
                Word._Application winword = new Word.Application();
                Word._Document document;
                Word.Table tables = null;

                string fileName = this.printType == "5X5" ? "\\Warehouse_P07_Sticker5.dotx" : "\\Warehouse_P07_Sticker10.dotx";
                object printFile = Sci.Env.Cfg.XltPathDir + fileName;
                document = winword.Documents.Add(ref printFile);


                #region PrintBarCode
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
                            winword.Selection.InsertNewPage();
                        }

                        winword.Selection.Paste();
                    }

                    int qrCodeWidth = this.printType == "10X10" ? 180 : 90;

                    // 填入資料
                    int i = 0;
                    foreach (var printItem in barcodeDatas)
                    {
                        tables = table[i + 1];
                        tables.Cell(1, 1).Range.Text = $"SP:{printItem["PoId"]}";
                        tables.Cell(1, 2).Range.Text = $"SEQ:{printItem["SEQ"]}";
                        tables.Cell(2, 1).Range.Text = $"Ref:{printItem["RefNo"]}";
                        tables.Cell(2, 2).Range.Text = $"Lot:{printItem["Dyelot"]}";
                        Bitmap oriBitmap = printItem["MINDQRCode"].ToString().ToBitmapQRcode(qrCodeWidth, qrCodeWidth);
                        Clipboard.SetImage(oriBitmap);
                        Thread.Sleep(100);
                        tables.Cell(3, 1).Range.Paste();
                        tables.Cell(3, 2).Range.Paste();
                        tables.Cell(4, 1).Range.Text = $"Color:{printItem["ColorID"]}";
                        tables.Cell(5, 1).Range.Text = $"Roll:{printItem["Roll"]}";
                        tables.Cell(5, 2).Range.Text = $"Qty:{printItem["ActualQty"]}";
                        i++;
                    }

                    // 產生的Word檔不可編輯
                    winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyReading);
                    winword.Visible = true;
                }
                catch (Exception ex)
                {
                    if (winword != null)
                    {
                        winword.Quit();
                    }

                    MyUtility.Msg.WarningBox("Export Word error.");
                }
                finally
                {
                    Marshal.ReleaseComObject(winword);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                #endregion
                this.HideWaitMessage();
                this.Close();
                #endregion
            }
            else
            {
                MyUtility.Msg.InfoBox("Select data first.");
            }
        }
    }
}
