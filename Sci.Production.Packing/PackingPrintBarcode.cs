using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace Sci.Production.Packing
{
    class PackingPrintBarcode
    {
        public DualResult PrintBarcode(string packingID, string ctn1, string ctn2)
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
                        int page = (printData.Rows.Count / 7) + ((printData.Rows.Count % 7 > 0) ? 1 : 0);
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

                            for (int p = i * 7; p < (i * 7) + 7; p++)
                            {
                                if (p >= printData.Rows.Count)
                                {
                                    break;
                                }

                                #region 準備資料
                                string barcode = "*" + printData.Rows[p]["ID"] + printData.Rows[p]["CTNStartNo"] + "*";
                                string packingNo = "　　　　PackingNo.: " + printData.Rows[p]["ID"];
                                string spNo = "　　　　SP No.: " + printData.Rows[p]["OrderID"];
                                string cartonNo = "　　　　Carton No.: " + printData.Rows[p]["CTNStartNo"] + " OF " + printData.Rows[p]["CtnQty"];
                                string poNo = "　　　　PO No.: " + printData.Rows[p]["PONo"];
                                string sizeQty = "　　　　Size/Qty: " + printData.Rows[p]["SizeCode"] + "/" + printData.Rows[p]["ShipQty"];
                                #endregion

                                tables.Cell(((p % 7) * 7) + 1, 1).Range.Text = barcode;
                                tables.Cell(((p % 7) * 7) + 2, 1).Range.Text = packingNo;
                                tables.Cell(((p % 7) * 7) + 3, 1).Range.Text = spNo;
                                tables.Cell(((p % 7) * 7) + 4, 1).Range.Text = cartonNo;
                                tables.Cell(((p % 7) * 7) + 5, 1).Range.Text = poNo;
                                tables.Cell(((p % 7) * 7) + 6, 1).Range.Text = sizeQty;
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
                    printFile = Sci.Env.Cfg.XltPathDir + "\\Packing_P03_Barcode.dotx";
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

                            #region 準備資料
                            string barcode = "*" + printData.Rows[i]["ID"] + printData.Rows[i]["CTNStartNo"] + "*";
                            string packingNo = "　　　　PackingNo.: " + printData.Rows[i]["ID"];
                            string spNo = "　　　　SP No.: " + printData.Rows[i]["OrderID"];
                            string cartonNo = "　　　　Carton No.: " + printData.Rows[i]["CTNStartNo"] + " OF " + printData.Rows[i]["CtnQty"];
                            string poNo = "　　　　PO No.: " + printData.Rows[i]["PONo"];
                            string sizeQty = "　　　　Size/Qty: " + printData.Rows[i]["SizeCode"] + "/" + printData.Rows[i]["ShipQty"];
                            #endregion

                            tables.Cell(1, 1).Range.Text = barcode;
                            tables.Cell(2, 1).Range.Text = packingNo;
                            tables.Cell(3, 1).Range.Text = spNo;
                            tables.Cell(4, 1).Range.Text = cartonNo;
                            tables.Cell(5, 1).Range.Text = poNo;
                            tables.Cell(6, 1).Range.Text = sizeQty;
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
    }
}
