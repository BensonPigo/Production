using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R03 : Win.Tems.PrintForm
    {
        private PPIC_R03_ViewModel ppic_R03_ViewModel;
        private DataTable printData;
        private DataTable dtArtworkData;
        private DataTable dtArtworkValues;

        // Artwork 動態欄位, 文字型態欄位清單
        private List<string> list = new List<string>() { "Printing LT", "InkType/Color/Size", "EMBROIDERY(SubCon)", "EMBROIDERY(POSubcon)", "POSubCon", "SubCon", };

        /// <inheritdoc/>
        public R03(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(this.comboM.Text);
            this.comboM.Enabled = false;

            this.Text = type == "1" ? "R03. PPIC master list report" : "R031. PPIC master list report (Artwork)";
            this.checkIncludeArtworkdata.Enabled = type != "1";
            this.checkIncludeArtworkdataKindIsPAP.Enabled = type != "1";
            this.checkByCPU.Enabled = type != "1";

            string strSelectSql = @"select '' as Zone,'' as Fty
union all
select distinct f.Zone,f.Zone+' - '+(select CONCAT(ID,'/') from Factory WITH (NOLOCK) where Zone = f.Zone for XML path('')) as Fty
from Factory f WITH (NOLOCK)
where Zone <> ''";

            DBProxy.Current.Select(null, strSelectSql, out DataTable zone);
            MyUtility.Tool.SetupCombox(this.comboZone, 2, zone);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from ArtworkType WITH (NOLOCK) where ReportDropdown = 1", out DataTable subprocess);
            MyUtility.Tool.SetupCombox(this.comboSubProcess, 1, subprocess);

            this.comboZone.SelectedIndex = 0;
            this.comboSubProcess.SelectedIndex = 0;
            this.checkBulk.Checked = true;

            if (type != "1")
            {
                this.checkIncludeArtworkdata.Checked = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateCutOffDate.Value1) && MyUtility.Check.Empty(this.dateCutOffDate.Value2) &&
                MyUtility.Check.Empty(this.dateCustRQSDate.Value1) && MyUtility.Check.Empty(this.dateCustRQSDate.Value2) &&
                MyUtility.Check.Empty(this.datePlanDate.Value1) && MyUtility.Check.Empty(this.datePlanDate.Value2) &&
                MyUtility.Check.Empty(this.dateOrderCfmDate.Value1) && MyUtility.Check.Empty(this.dateOrderCfmDate.Value2))
            {
                MyUtility.Msg.WarningBox("All date can't empty!!");
                this.dateBuyerDelivery.TextBox1.Focus();
                return false;
            }

            this.ppic_R03_ViewModel = new PPIC_R03_ViewModel()
            {
                IsPowerBI = false,
                BuyerDelivery1 = this.dateBuyerDelivery.Value1,
                BuyerDelivery2 = this.dateBuyerDelivery.Value2,
                SciDelivery1 = this.dateSCIDelivery.Value1,
                SciDelivery2 = this.dateSCIDelivery.Value2,
                SDPDate1 = this.dateCutOffDate.Value1,
                SDPDate2 = this.dateCutOffDate.Value2,
                CRDDate1 = this.dateCustRQSDate.Value1,
                CRDDate2 = this.dateCustRQSDate.Value2,
                PlanDate1 = this.datePlanDate.Value1,
                PlanDate2 = this.datePlanDate.Value2,
                CFMDate1 = this.dateOrderCfmDate.Value1,
                CFMDate2 = this.dateOrderCfmDate.Value2,
                SP1 = this.txtSp1.Text.Trim(),
                SP2 = this.txtSp2.Text.Trim(),
                StyleID = this.txtstyle.Text.Trim(),
                Article = this.txtArticle.Text.Trim(),
                SeasonID = this.txtseason.Text.Trim(),
                BrandID = this.txtbrand.Text.Trim(),
                CustCDID = this.txtcustcd.Text.Trim(),
                Zone = MyUtility.Convert.GetString(this.comboZone.SelectedValue),
                MDivisionID = this.comboM.Text,
                Factory = this.comboFactory.Text,
                Bulk = this.checkBulk.Checked,
                Sample = this.checkSample.Checked,
                Material = this.checkMaterial.Checked,
                Forecast = this.checkForecast.Checked,
                Garment = this.checkGarment.Checked,
                SMTL = this.checkSMTL.Checked,
                ArtworkTypeID = this.comboSubProcess.Text,
                IncludeHistoryOrder = this.checkIncludeHistoryOrder.Checked,
                IncludeArtworkData = this.checkIncludeArtworkdata.Checked,
                PrintingDetail = this.chkPrintingDetail.Checked,
                ByCPU = this.checkByCPU.Checked,
                IncludeArtworkDataKindIsPAP = this.checkIncludeArtworkdataKindIsPAP.Checked,
                SeparateByQtyBdownByShipmode = this.checkQtyBDownByShipmode.Checked,
                ListPOCombo = this.checkListPOCombo.Checked,
                IncludeCancelOrder = this.chkIncludeCancelOrder.Checked,
            };

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            PPIC_R03 biModel = new PPIC_R03();
            Base_ViewModel resultReport = biModel.GetPPICMasterList(this.ppic_R03_ViewModel);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.DtArr.ElementAtOrDefault(0);
            this.dtArtworkValues = resultReport.DtArr.ElementAtOrDefault(1);
            this.dtArtworkData = resultReport.DtArr.ElementAtOrDefault(2);

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData == null || this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Application excelapp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\PPIC_R03_PPICMasterList.xltx", excelapp);

            if (this.printData.Columns[0].ColumnName == "Seq")
            {
                DataTable dtNoSeq = this.printData.Copy();
                dtNoSeq.Columns.Remove("Seq");
                com.WriteTable(dtNoSeq, 1, needHeader: true);
            }
            else
            {
                com.WriteTable(this.printData, 1, needHeader: true);
            }

            Excel.Worksheet worksheet = excelapp.ActiveWorkbook.Worksheets[1];
            worksheet.Name = "PPIC_Master_List";

            #region Artwork 動態欄位. 因為效能寫在這, 資料量大時一年以上那種 1.用SQL做pivot會卡死, 2.用C#的DataTable做pivot也很卡 3.用C#雙迴圈預先準備DataTable也很卡
            int artworkCount = 0;
            if (this.ppic_R03_ViewModel.IncludeArtworkData || this.ppic_R03_ViewModel.IncludeArtworkDataKindIsPAP)
            {
                artworkCount = this.dtArtworkData.Rows.Count;
                int startColumnIndex_Artwork = this.printData.Columns.Count + 1;
                int startRowIndex = 2;

                // 填充欄位名稱
                for (int i = 0; i < this.dtArtworkData.Rows.Count; i++)
                {
                    int columnIndex = startColumnIndex_Artwork + i;
                    worksheet.Cells[1, columnIndex].Value = this.dtArtworkData.Rows[i]["ColumnN"];
                }

                excelapp.Cells.EntireColumn.AutoFit();

                // 填充欄位值
                List<object[,]> rowValuesList = new List<object[,]>();
                foreach (DataRow mainRow in this.printData.Rows)
                {
                    string spNo = MyUtility.Convert.GetString(mainRow["SPNO"]);
                    string filter;
                    if (this.ppic_R03_ViewModel.SeparateByQtyBdownByShipmode)
                    {
                        filter = $"OrderID = '{spNo}' AND Seq = '{mainRow["Seq"]}'";
                    }
                    else
                    {
                        filter = $"OrderID = '{spNo}'";
                    }

                    var drsArtwork = this.dtArtworkValues.Select(filter);
                    object[,] rowValues = new object[1, this.dtArtworkData.Rows.Count];

                    // 將初始值設為 0，但對於特定的 ColumnN，不設為 0
                    for (int i = 0; i < rowValues.Length; i++)
                    {
                        string columnName = this.dtArtworkData.Rows[i]["ColumnN"].ToString();
                        if (!this.list.Contains(columnName)) // 不在指定清單中的列，預設為 0
                        {
                            rowValues[0, i] = 0;
                        }
                    }

                    foreach (DataRow dr in drsArtwork)
                    {
                        string columnName = dr["ColumnName"].ToString();
                        int columnIndex = Array.IndexOf(this.dtArtworkData.AsEnumerable().Select(r => r["ColumnN"].ToString()).ToArray(), columnName);
                        if (columnIndex != -1) // Check if the columnName exists in dtArtworkData
                        {
                            rowValues[0, columnIndex] = dr["ColumnValue"];
                        }
                    }

                    rowValuesList.Add(rowValues);
                }

                // 將所有行的值填入工作表
                for (int i = 0; i < rowValuesList.Count; i++)
                {
                    worksheet.Cells[startRowIndex + i, startColumnIndex_Artwork].Resize(1, this.dtArtworkData.Rows.Count).Value = rowValuesList[i];
                }
            }
            #endregion

            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, this.printData.Columns.Count + artworkCount]].Interior.Color = Color.FromArgb(191, 191, 191); // 底色

            // 若有 Seq 欄位,此處才移除,前面需要用
            if ((worksheet.Cells[1, 1] as Excel.Range).Value2?.ToString() == "Seq")
            {
                // Delete the entire first column
                Excel.Range entireColumn = worksheet.Columns[1];
                entireColumn.Delete();
            }

            this.CreateCustomizedExcel(ref worksheet);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R03_PPICMasterList");
            Microsoft.Office.Interop.Excel.Workbook workbook = excelapp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelapp.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelapp);
            strExcelName.OpenFile();
            this.HideWaitMessage();
            #endregion
            return true;
        }

        private void CheckIncludeArtworkdata_CheckedChanged(object sender, EventArgs e)
        {
            this.chkPrintingDetail.Enabled = this.checkIncludeArtworkdata.Checked;
            if (!this.checkIncludeArtworkdata.Checked)
            {
                this.chkPrintingDetail.Checked = false;
            }
        }
    }
}
