using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Word = Microsoft.Office.Interop.Word;

namespace Sci.Production.Packing
{
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        int orderQty;
        bool SP_Multiple = false;
        string reportType, ctn1, ctn2, specialInstruction;
        DataTable printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, qtyBDown;
        public P03_Print(DataRow MasterData, int OrderQty)
        {
            InitializeComponent();

            //如果是多訂單一起裝箱就不列印
            SP_Multiple = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct OrderID+OrderShipmodeSeq) from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(MasterData["ID"])))) > 1;
            if (SP_Multiple)
            {
                radioPackingGuideReport.Visible = false;                
            }

            masterData = MasterData;
            orderQty = OrderQty;
            radioPackingListReportFormA.Checked = true;
            ControlPrintFunction(false);
        }

        private void radioBarcodePrint_CheckedChanged(object sender, EventArgs e)
        {
            ControlPrintFunction(radioBarcodePrint.Checked);
        }

        //控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = isSupport;
            this.IsSupportToExcel = !isSupport;
            txtCTNStart.Enabled = isSupport;
            txtCTNEnd.Enabled = isSupport;
            if (!isSupport)
            {
                txtCTNStart.Text = "";
                txtCTNEnd.Text = "";
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioPackingListReportFormA.Checked ? "1" : radioPackingListReportFormB.Checked ? "2" : radioPackingGuideReport.Checked ? "3" : "4";
            ctn1 = txtCTNStart.Text;
            ctn2 = txtCTNEnd.Text;
            ReportResourceName = "BarcodePrint.rdlc";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (reportType == "1" || reportType == "2")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingListReportData(MyUtility.Convert.GetString(masterData["ID"]), reportType, out printData, out ctnDim, out qtyBDown);
                return result;
            }
            else if (reportType == "3")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingGuideReportData(MyUtility.Convert.GetString(masterData["ID"]), out printData, out ctnDim, out qtyCtn, out articleSizeTtlShipQty, out printGroupData, out clipData, out specialInstruction);
                return result;
            }

            return Result.True;
        }

        protected override bool ToPrint()
        {
            this.ValidateInput();
            DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(masterData["ID"]), ctn1, ctn2, out printData);
            if (!result)
            {
                return result;
            }
            else if (printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            //e.Report.ReportDataSource = printData;
            this.ShowWaitMessage("Data Loading ...");
            Microsoft.Office.Interop.Word._Application winword = new Microsoft.Office.Interop.Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
            winword.Visible = false;
            Object printFile;
            Microsoft.Office.Interop.Word._Document document;
            Word.Table tables = null;
            
            #region check Factory
            string CountryID = MyUtility.GetValue.Lookup(string.Format(@"Select CountryID from Factory where id = '{0}'", Sci.Env.User.Factory));
            switch (CountryID)
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
                                winword.Selection.InsertNewPage();
                            winword.Selection.Paste();
                        }
                        #endregion
                        #region 填入資料
                        for (int i = 0; i < page; i++)
                        {
                            tables = table[i + 1];

                            for (int p = i * 6; p < i * 6 + 6; p++)
                            {
                                if (p >= printData.Rows.Count) break;

                                #region 準備資料
                                string barcode = "*" + printData.Rows[p]["ID"] + printData.Rows[p]["CTNStartNo"] + "*";
                                string packingNo = "　　　　PackingNo.: " + printData.Rows[p]["ID"];
                                string spNo = "　　　　SP No.: " + printData.Rows[p]["OrderID"];
                                string cartonNo = "　　　　Carton No.: " + printData.Rows[p]["CTNStartNo"] + " OF " + printData.Rows[p]["CtnQty"];
                                string poNo = "　　　　PO No.: " + printData.Rows[p]["PONo"];
                                #endregion

                                tables.Cell(p % 6 * 6 + 1, 1).Range.Text = barcode;
                                tables.Cell(p % 6 * 6 + 2, 1).Range.Text = packingNo;
                                tables.Cell(p % 6 * 6 + 3, 1).Range.Text = spNo;
                                tables.Cell(p % 6 * 6 + 4, 1).Range.Text = cartonNo;
                                tables.Cell(p % 6 * 6 + 5, 1).Range.Text = poNo;
                            }
                        }
                        #endregion
                        winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");


                        winword.Visible = true;                      
                        winword = null;
                    }
                    catch (Exception ex)
                    {
                        if (null != winword)
                            winword.Quit();
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
                                winword.Selection.InsertNewPage();
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
                            #endregion

                            tables.Cell(1, 1).Range.Text = barcode;
                            tables.Cell(2, 1).Range.Text = packingNo;
                            tables.Cell(3, 1).Range.Text = spNo;
                            tables.Cell(4, 1).Range.Text = cartonNo;
                            tables.Cell(5, 1).Range.Text = poNo;
                        }
                        #endregion
                        winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");


                        winword.Visible = true;
                        winword = null;
                    }
                    catch (Exception ex)
                    {
                        if (null != winword)
                            winword.Quit();
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

            #endregion
            this.HideWaitMessage();
            return true;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.ShowWaitMessage("Data Loading....");
            if (reportType == "1" || reportType == "2")
            {
                if (SP_Multiple)
                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport_Multiple.xltx", masterData, reportType, printData, ctnDim, qtyBDown);
                else
                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport.xltx", masterData, reportType, printData, ctnDim, qtyBDown);
            }
            else if (reportType == "3")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, masterData, orderQty, specialInstruction);

            }
            this.HideWaitMessage();
            return true;
        }
    }
}
