using Ict;
using OnBarcode.Barcode;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using Word = Microsoft.Office.Interop.Word;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    internal class PackingPrintBarcode
    {
        /// <inheritdoc/>
        public DualResult PrintBarcode(string packingID, string ctn1, string ctn2, string print_type = "", bool country = false)
        {
            DataTable printData;
            DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(packingID), ctn1, ctn2, out printData);
            if (!result)
            {
                return result;
            }
            else if (printData == null || printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            #region check Factory
            string countryID = MyUtility.GetValue.Lookup(string.Format(@"Select CountryID from Factory where id = '{0}'", Env.User.Factory));
            switch (countryID)
            {
                case "VN":
                    printFile = Env.Cfg.XltPathDir + "\\Packing_P03_BarcodeVN.dotx";
                    document = winword.Documents.Add(ref printFile);
                    #region VN
                    try
                    {
                        document.Activate();
                        Word.Tables table = document.Tables;

                        #region 計算頁數
                        winword.Selection.Tables[1].Select();
                        winword.Selection.Copy();
                        int page = (printData.Rows.Count / 6) + ((printData.Rows.Count % 6 > 0) ? 1 : 0);
                        for (int i = 1; i < page; i++)
                        {
                            winword.Selection.MoveDown();
                            if (page > 1)
                            {
                                winword.Selection.InsertNewPage();
                            }

                            winword.Selection.Paste();
                        }
                        #endregion
                        #region 填入資料
                        for (int i = 0; i < page; i++)
                        {
                            tables = table[i + 1];

                            for (int p = i * 6; p < (i * 6) + 6; p++)
                            {
                                if (p >= printData.Rows.Count)
                                {
                                    break;
                                }

                                #region 準備資料
                                string barcode = printData.Rows[p]["ID"].ToString() + printData.Rows[p]["CTNStartNo"].ToString();
                                string packingNo = "　　　　PackingNo.: " + printData.Rows[p]["ID"];
                                string spNo = "　　　　SP No.: " + printData.Rows[p]["OrderID"];
                                string cartonNo = "　　　　Carton No.: " + printData.Rows[p]["CTNStartNo"] + " OF " + printData.Rows[p]["CtnQty"];
                                string poNo = "　　　　PO No.: " + printData.Rows[p]["PONo"];
                                string sizeQty = "　　　　Size/Qty: " + printData.Rows[p]["SizeCode"] + "/" + printData.Rows[p]["ShipQty"];
                                #endregion

                                Bitmap oriBitmap = this.NewBarcode(barcode);
                                Clipboard.SetImage(oriBitmap);
                                tables.Cell(((p % 6) * 7) + 1, 1).Range.Paste();
                                tables.Cell(((p % 6) * 7) + 1, 1).Range.InlineShapes[1].ScaleHeight = 40f;
                                tables.Cell(((p % 6) * 7) + 1, 1).Range.InlineShapes[1].ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapTight;

                                tables.Cell(((p % 6) * 7) + 2, 1).Range.Text = packingNo;
                                tables.Cell(((p % 6) * 7) + 3, 1).Range.Text = spNo;
                                tables.Cell(((p % 6) * 7) + 4, 1).Range.Text = cartonNo;
                                tables.Cell(((p % 6) * 7) + 5, 1).Range.Text = poNo;
                                tables.Cell(((p % 6) * 7) + 6, 1).Range.Text = sizeQty;
                            }
                        }
                        #endregion
                        winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");

                        #region Save & Show Word
                        winword.Visible = true;
                        Marshal.ReleaseComObject(winword);
                        Marshal.ReleaseComObject(document);
                        Marshal.ReleaseComObject(table);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        if (winword != null)
                        {
                            winword.Quit();
                        }

                        return new DualResult(false, "Export word error.", ex);
                    }
                    finally
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    #endregion
                    break;
                case "PH":
                default:
                    printFile = Env.Cfg.XltPathDir + (print_type.Equals("New") ? "\\Packing_P03_Barcode_New.dotx" : "\\Packing_P03_Barcode.dotx");
                    document = winword.Documents.Add(ref printFile);
                    #region PH
                    try
                    {
                        document.Activate();
                        Word.Tables table = document.Tables;

                        #region 計算頁數
                        winword.Selection.Tables[1].Select();
                        winword.Selection.Copy();
                        for (int i = 1; i < printData.Rows.Count; i++)
                        {
                            winword.Selection.MoveDown();
                            if (printData.Rows.Count > 1)
                            {
                                winword.Selection.InsertNewPage();
                            }

                            winword.Selection.Paste();
                        }
                        #endregion
                        #region 填入資料
                        for (int i = 0; i < printData.Rows.Count; i++)
                        {
                            tables = table[i + 1];

                            if (print_type.Equals("New"))
                            {
                                #region New format
                                #region 準備資料
                                string barcode = printData.Rows[i]["ID"].ToString() + printData.Rows[i]["CTNStartNo"].ToString();
                                string packingNo = "PG#.: " + printData.Rows[i]["ID"];
                                string spNo = "SP#.: " + printData.Rows[i]["OrderID"];
                                string style = "Style#.: " + printData.Rows[i]["StyleID"];
                                string cartonNo = "CTN#.: " + printData.Rows[i]["CTNStartNo"] + " OF " + printData.Rows[i]["CtnQty"];
                                string poNo = printData.Rows[i]["PONo"].ToString();
                                string sizeQty = "Size/Qty: " + printData.Rows[i]["SizeCode"] + "/" + printData.Rows[i]["ShipQty"];
                                #endregion

                                Bitmap oriBitmap = this.NewBarcode(barcode);
                                Clipboard.SetImage(oriBitmap);
                                tables.Cell(6, 1).Range.Paste();
                                tables.Cell(6, 1).Range.InlineShapes[1].ScaleHeight = 40f;
                                tables.Cell(6, 1).Range.InlineShapes[1].LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoFalse;
                                tables.Cell(6, 1).Range.InlineShapes[1].Height = (float)45;
                                tables.Cell(6, 1).Range.InlineShapes[1].ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapTight;

                                tables.Cell(1, 1).Range.Text = packingNo;
                                tables.Cell(2, 1).Range.Text = spNo;
                                tables.Cell(3, 1).Range.Text = style;
                                tables.Cell(2, 2).Range.Text = cartonNo;
                                tables.Cell(1, 2).Range.Text = sizeQty;
                                if (country)
                                {
                                    string madein = "Made in " + MyUtility.Convert.GetString(MyUtility.GetValue.Lookup($"select Alias from country where id = (select countryid from factory where id = '{Env.User.Factory}')"));
                                    string deldate = "del date: " + (MyUtility.Check.Empty(printData.Rows[i]["BuyerDelivery"]) ? string.Empty : ((DateTime)printData.Rows[i]["BuyerDelivery"]).ToString("yyyy/MM/dd"));
                                    tables.Cell(4, 1).Range.Text = madein;
                                    tables.Cell(4, 2).Range.Text = deldate;
                                }

                                tables.Cell(5, 2).Range.Text = poNo;
                                #endregion
                            }
                            else
                            {
                                #region old format
                                #region 準備資料
                                string barcode = printData.Rows[i]["ID"].ToString() + printData.Rows[i]["CTNStartNo"].ToString();
                                string packingNo = "　　　　PackingNo.: " + printData.Rows[i]["ID"];
                                string spNo = "　　　　SP No.: " + printData.Rows[i]["OrderID"];
                                string cartonNo = "　　　　Carton No.: " + printData.Rows[i]["CTNStartNo"] + " OF " + printData.Rows[i]["CtnQty"];
                                string poNo = "　　　　PO No.: " + printData.Rows[i]["PONo"];
                                string sizeQty = "　　　　Size/Qty: " + printData.Rows[i]["SizeCode"] + "/" + printData.Rows[i]["ShipQty"];
                                #endregion

                                Bitmap oriBitmap = this.NewBarcode(barcode);
                                Clipboard.SetImage(oriBitmap);
                                tables.Cell(1, 1).Range.Paste();
                                tables.Cell(1, 1).Range.InlineShapes[1].ScaleHeight = 40f;
                                tables.Cell(1, 1).Range.InlineShapes[1].ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapTight;

                                tables.Cell(2, 1).Range.Text = packingNo;
                                tables.Cell(3, 1).Range.Text = spNo;
                                tables.Cell(4, 1).Range.Text = cartonNo;
                                tables.Cell(5, 1).Range.Text = poNo;
                                tables.Cell(6, 1).Range.Text = sizeQty;
                                #endregion
                            }
                        }
                        #endregion
                        winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");

                        #region Save & Show Word
                        winword.Visible = true;
                        Marshal.ReleaseComObject(winword);
                        Marshal.ReleaseComObject(document);
                        Marshal.ReleaseComObject(table);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        if (winword != null)
                        {
                            winword.Quit();
                        }

                        return new DualResult(false, "Export word error.", ex);
                    }
                    finally
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    #endregion
                    break;
            }

            return new DualResult(true);
            #endregion
        }

        /// <inheritdoc/>
        public DualResult PrintBarcodeOtherSize(string packingID, string ctn1, string ctn2, string print_type = "", bool country = false)
        {
            DataTable printData;
            DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(packingID), ctn1, ctn2, out printData);
            if (!result)
            {
                return result;
            }
            else if (printData == null || printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            printFile = Env.Cfg.XltPathDir + "\\Packing_P03_BarcodeOther.dotx";
            document = winword.Documents.Add(ref printFile);
            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                if (printData.Rows.Count > 1)
                {
                    for (int i = 1; i < printData.Rows.Count; i++)
                    {
                        winword.Selection.MoveDown();
                        winword.Selection.InsertNewPage();
                        winword.Selection.Paste();
                    }
                }

                #endregion
                #region 填入資料
                for (int i = 0; i < printData.Rows.Count; i++)
                {
                    tables = table[i + 1];
                    #region 準備資料
                    string barcode = printData.Rows[i]["ID"].ToString() + printData.Rows[i]["CTNStartNo"].ToString();
                    string packingNo = "P/L#.: " + printData.Rows[i]["ID"];
                    string spNo = "SP#.: " + printData.Rows[i]["OrderID"];
                    string poNo = "PO#.: " + printData.Rows[i]["PONo"].ToString();
                    string sizeQty = "Size/Qty: " + printData.Rows[i]["SizeCode"] + "/" + printData.Rows[i]["ShipQty"];
                    string cartonNo = "CTN#.: " + printData.Rows[i]["CTNStartNo"] + " OF " + printData.Rows[i]["CtnQty"];
                    #endregion

                    Bitmap oriBitmap = this.NewBarcode(barcode);
                    Bitmap resized = new Bitmap(oriBitmap, new Size(900, 145));

                    Clipboard.SetImage(resized);
                    tables.Cell(1, 1).Range.Paste();
                    tables.Cell(1, 1).Range.InlineShapes[1].ScaleHeight = 20f;
                    tables.Cell(1, 1).Range.InlineShapes[1].LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoFalse;
                    tables.Cell(1, 1).Range.InlineShapes[1].ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapTight;

                    tables.Cell(2, 1).Range.Text = packingNo + Environment.NewLine + spNo + Environment.NewLine + poNo + Environment.NewLine + sizeQty + " " + cartonNo;
                }
                #endregion
                winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");

                #region Save & Show Word
                winword.Visible = true;
                Marshal.ReleaseComObject(winword);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(table);
                #endregion
            }
            catch (Exception ex)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new DualResult(true);
        }

        /// <summary>
        /// PrintQRcode
        /// </summary>
        /// <param name="packingID">packingID</param>
        /// <param name="ctn1">ctn1</param>
        /// <param name="ctn2">ctn2</param>
        /// <param name="print_type">print_type</param>
        /// <param name="country">country</param>
        /// <returns>DualResult</returns>
        public DualResult PrintQRcode(string packingID, string ctn1, string ctn2, string print_type = "", bool country = false)
        {
            DataTable printData;
            int pageItemCount = 1;
            DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(packingID), ctn1, ctn2, out printData);
            if (!result)
            {
                return result;
            }
            else if (printData == null || printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            printFile = Env.Cfg.XltPathDir + "\\Packing_P03_QRcode.dotx";
            document = winword.Documents.Add(ref printFile);

            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                // winword.Visible = true;
                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                int page = (printData.Rows.Count / pageItemCount) + ((printData.Rows.Count % pageItemCount > 0) ? 1 : 0);
                for (int i = 1; i < page; i++)
                {
                    winword.Selection.MoveDown();
                    if (page > 1)
                    {
                        winword.Selection.InsertNewPage();
                    }

                    winword.Selection.Paste();
                }
                #endregion
                #region 填入資料
                for (int i = 0; i < page; i++)
                {
                    tables = table[i + 1];

                    for (int p = i * pageItemCount; p < (i * pageItemCount) + pageItemCount; p++)
                    {
                        if (p >= printData.Rows.Count)
                        {
                            break;
                        }

                        #region 準備資料
                        string barcode = printData.Rows[p]["ID"].ToString() + printData.Rows[p]["CTNStartNo"].ToString();
                        string packingNo = "P/L#:" + printData.Rows[p]["ID"];
                        string spNo = "SP#:" + printData.Rows[p]["OrderID"];
                        string cartonNo = "CTN:" + printData.Rows[p]["CTNStartNo"] + " OF " + printData.Rows[p]["CtnQty"];
                        string poNo = "PO#:" + printData.Rows[p]["PONo"];
                        string sizeQty = "Size/Qty:" + printData.Rows[p]["SizeCode"] + "/" + printData.Rows[p]["ShipQty"];
                        string custCTN = printData.Rows[p]["CustCTN"].ToString();
                        #endregion

                        tables.Cell(((p % pageItemCount) * 3) + 1, 1).Range.Text = packingNo + Environment.NewLine + spNo + Environment.NewLine + poNo;
                        tables.Cell(((p % pageItemCount) * 3) + 2, 1).Range.Text = sizeQty + "  " + cartonNo;
                        tables.Cell(((p % pageItemCount) * 3) + 3, 1).Range.Text = custCTN;

                        Bitmap oriBitmap = this.NewQRcode(barcode);
                        Clipboard.SetImage(oriBitmap);
                        tables.Cell(((p % pageItemCount) * 3) + 1, 2).Range.Paste();
                        Word.Shape qrCodeImg = tables.Cell(((p % pageItemCount) * 3) + 1, 2).Range.InlineShapes[1].ConvertToShape();
                        qrCodeImg.Width = 34;
                        qrCodeImg.Height = 34;

                        qrCodeImg.WrapFormat.Type = Word.WdWrapType.wdWrapBehind;
                        qrCodeImg.RelativeHorizontalPosition = Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionColumn;
                        qrCodeImg.RelativeVerticalPosition = Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionParagraph;

                        // 由於設定相對距離，因此要給個位置設定，避免漂走
                        qrCodeImg.Top = MyUtility.Convert.GetFloat(0.05);
                        qrCodeImg.Left = MyUtility.Convert.GetFloat(0.00);
                    }
                }
                #endregion
                winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");

                winword.Visible = true;
                Marshal.ReleaseComObject(winword);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(table);
            }
            catch (Exception ex)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        public DualResult PrintCustCTN(string packingID, string ctn1, string ctn2, string print_type = "", bool country = false)
        {
            DataTable printData;
            DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(packingID), ctn1, ctn2, out printData);
            if (!result)
            {
                return result;
            }
            else if (printData == null || printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            printFile = Env.Cfg.XltPathDir + "\\Packing_P03_Barcode_CustCTN.dotx";
            document = winword.Documents.Add(ref printFile);

            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                int page = (printData.Rows.Count / 24) + ((printData.Rows.Count % 24 > 0) ? 1 : 0);
                for (int i = 1; i < page; i++)
                {
                    winword.Selection.MoveDown();
                    if (page > 1)
                    {
                        winword.Selection.InsertNewPage();
                    }

                    winword.Selection.Paste();
                }
                #endregion

                for (int i = 0; i < page; i++)
                {
                    tables = table[i + 1];

                    // 直的
                    int countPerLine = 8;

                    // 橫的
                    int countPerRow = 3;

                    // 1~3行
                    for (int p = 1; p <= countPerRow; p++)
                    {
                        // 1~8列
                        for (int x = 1; x <= countPerLine; x++)
                        {
                            int dataTableIndex = (countPerLine * (p - 1)) + (x - 1) + (24 * i);

                            if (dataTableIndex > printData.Rows.Count - 1)
                            {
                                break;
                            }

                            #region 準備資料
                            string barcode = printData.Rows[dataTableIndex]["CustCTN"].ToString();
                            string cartonNo = "CTN: " + printData.Rows[dataTableIndex]["CTNStartNo"] + " of " + printData.Rows[p]["CTNQty"];
                            #endregion

                            Bitmap oriBitmap = this.NewBarcode(barcode);
                            Clipboard.SetImage(oriBitmap);

                            tables.Cell((x * 2) - 1, p).Range.Paste();
                            tables.Cell((x * 2) - 1, p).Range.InlineShapes[1].ScaleHeight = 37.5f;
                            tables.Cell((x * 2) - 1, p).Range.InlineShapes[1].ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapTight;

                            tables.Cell(x * 2, p).Range.Text = cartonNo;
                        }
                    }
                }

                winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");

                #region Save & Show Word
                winword.Visible = true;
                Marshal.ReleaseComObject(winword);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(table);
                #endregion
            }
            catch (Exception ex)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new DualResult(true);
        }

        /// <summary>
        /// 將 Barcode 轉換成圖片
        /// </summary>
        /// <param name="strBarcode">Barcode 內容</param>
        /// <returns>Image</returns>
        private Bitmap NewBarcode(string strBarcode)
        {
            Linear code = new Linear
            {
                Type = BarcodeType.CODE128,
                Data = strBarcode,
                Format = ImageFormat.Bmp,
                X = 3,
                Y = 160,
                TextFont = new Font("Arial", 20f, FontStyle.Regular),
                Resolution = 900,
                LeftMargin = -30,
            };
            code.drawBarcode("c#-barcode.Bmp");
            return code.drawBarcode();
        }

        /// <summary>
        /// 將 QR code 轉換成圖片
        /// </summary>
        /// <param name="strBarcode">Barcode 內容</param>
        /// <returns>Image</returns>
        private Bitmap NewQRcode(string strBarcode)
        {
            /*
  Level L (Low)      7%  of codewords can be restored.
  Level M (Medium)   15% of codewords can be restored.
  Level Q (Quartile) 25% of codewords can be restored.
  Level H (High)     30% of codewords can be restored.
*/
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    // Create Photo
                    Height = 120,
                    Width = 120,
                    Margin = 0,
                    CharacterSet = "UTF-8",
                    PureBarcode = true,

                    // 錯誤修正容量
                    // L水平    7%的字碼可被修正
                    // M水平    15%的字碼可被修正
                    // Q水平    25%的字碼可被修正
                    // H水平    30%的字碼可被修正
                    ErrorCorrection = ErrorCorrectionLevel.L,
                },
            };

            return writer.Write(strBarcode);
        }
    }
}
