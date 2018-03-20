﻿using Ict;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using OnBarcode.Barcode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Sci.Production.Packing
{
    internal class PackingPrintBarcode
    {
        public DualResult PrintBarcode(string packingID, string ctn1, string ctn2, string print_type = "")
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

            Microsoft.Office.Interop.Word._Application winword = new Microsoft.Office.Interop.Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
            winword.Visible = false;
            object printFile;
            Microsoft.Office.Interop.Word._Document document;
            Word.Table tables = null;

            #region check Factory
            string countryID = MyUtility.GetValue.Lookup(string.Format(@"Select CountryID from Factory where id = '{0}'", Sci.Env.User.Factory));
            switch (countryID)
            {
                case "VN":
                    printFile = Sci.Env.Cfg.XltPathDir + "\\Packing_P03_BarcodeVN.dotx";
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
                    printFile = Sci.Env.Cfg.XltPathDir + (print_type.Equals("New") ? "\\Packing_P03_Barcode_New.dotx" : "\\Packing_P03_Barcode.dotx");
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
                                string packingNo = "PackingNo.: " + printData.Rows[i]["ID"];
                                string spNo = "SP No.: " + printData.Rows[i]["OrderID"];
                                string cartonNo = "Carton No.: " + printData.Rows[i]["CTNStartNo"] + " OF " + printData.Rows[i]["CtnQty"];
                                string poNo = printData.Rows[i]["PONo"].ToString();
                                string sizeQty = "Size/Qty: " + printData.Rows[i]["SizeCode"] + "/" + printData.Rows[i]["ShipQty"];
                                #endregion

                                Bitmap oriBitmap = this.NewBarcode(barcode);
                                Clipboard.SetImage(oriBitmap);
                                tables.Cell(4, 1).Range.Paste();
                                tables.Cell(4, 1).Range.InlineShapes[1].ScaleHeight = 40f;
                                tables.Cell(4, 1).Range.InlineShapes[1].ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapTight;

                                tables.Cell(1, 1).Range.Text = packingNo;
                                tables.Cell(2, 1).Range.Text = spNo;
                                tables.Cell(2, 2).Range.Text = cartonNo;
                                tables.Cell(1, 2).Range.Text = sizeQty;
                                tables.Cell(3, 2).Range.Text = poNo;
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

        /// <summary>
        /// 將 Barcode 轉換成圖片
        /// </summary>
        /// <param name="strBarcode">Barcode 內容</param>
        /// <returns>Image</returns>
        private Bitmap NewBarcode(string strBarcode)
        {
            Linear code = new Linear();
            code.Type = BarcodeType.CODE128;
            code.Data = strBarcode;
            code.Format = ImageFormat.Bmp;
            code.X = 4;
            code.Y = 160;
            code.TextFont = new Font("Arial", 18f, FontStyle.Regular);
            code.Resolution = 600;
            code.drawBarcode("c#-barcode.Bmp");
            return code.drawBarcode();
        }
    }
}
