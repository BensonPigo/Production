using Ict;
using Sci.Data;
using Sci.Production.Class.Command;
using Sci.Production.Prg;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R21 : Win.Tems.PrintForm
    {
        private string StartSPNo;
        private string EndSPNo;
        private string MDivision;
        private string Factory;
        private string StartRefno;
        private string EndRefno;
        private string Color;
        private string MT;
        private string MtlTypeID;
        private string ST;
        private string WorkNo;
        private string location1;
        private string location2;
        private bool bulk;
        private bool sample;
        private bool material;
        private bool smtl;
        private bool complete;
        private DateTime? BuyerDelivery1;
        private DateTime? BuyerDelivery2;
        private DateTime? ETA1;
        private DateTime? ETA2;
        private DateTime? OrigBuyerDelivery1;
        private DateTime? OrigBuyerDelivery2;
        private DateTime? arriveWH1;
        private DateTime? arriveWH2;

        private int _reportType;
        private bool boolCheckQty;
        private int data_cnt = 0;
        private DataTable printData;

        /// <summary>
        /// Initializes a new instance of the <see cref="R21"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            Dictionary<string, string> comboBox2_RowSource = new Dictionary<string, string>
            {
                { "All", "All" },
                { "B", "Bulk" },
                { "I", "Inventory" },
                { "O", "Scrap" },
            };
            this.cmbStockType.DataSource = new BindingSource(comboBox2_RowSource, null);
            this.cmbStockType.ValueMember = "Key";
            this.cmbStockType.DisplayMember = "Value";
            this.cmbStockType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.StartSPNo = this.textStartSP.Text;
            this.EndSPNo = this.textEndSP.Text;
            this.MDivision = this.txtMdivision1.Text;
            this.Factory = this.txtfactory1.Text;
            this.StartRefno = this.textStartRefno.Text;
            this.EndRefno = this.textEndRefno.Text;
            this.Color = this.textColor.Text;
            this.MT = this.comboxMaterialTypeAndID.comboMaterialType.SelectedValue.ToString();
            this.MtlTypeID = this.comboxMaterialTypeAndID.comboMtlTypeID.SelectedValue.ToString();
            this.ST = this.cmbStockType.SelectedValue.ToString();
            this._reportType = this.rdbtnDetail.Checked ? 0 : 1;
            this.boolCheckQty = this.checkQty.Checked;
            this.BuyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.BuyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.ETA1 = this.dateETA.Value1;
            this.ETA2 = this.dateETA.Value2;
            this.OrigBuyerDelivery1 = this.dateOrigBuyerDelivery.Value1;
            this.OrigBuyerDelivery2 = this.dateOrigBuyerDelivery.Value2;
            this.arriveWH1 = this.dateArriveDate.Value1;
            this.arriveWH2 = this.dateArriveDate.Value2;
            this.WorkNo = this.txtWorkNo.Text;
            this.location1 = this.txtMtlLocation1.Text;
            this.location2 = this.txtMtlLocation2.Text;

            this.bulk = this.checkBulk.Checked;
            this.sample = this.checkSample.Checked;
            this.material = this.checkMaterial.Checked;
            this.smtl = this.checkSMTL.Checked;
            this.complete = this.chkComplete.Checked;
            if (MyUtility.Check.Empty(this.StartSPNo) &&
                MyUtility.Check.Empty(this.EndSPNo) &&
                !this.dateETA.HasValue &&
                !this.dateArriveDate.HasValue &&
                !this.dateBuyerDelivery.HasValue &&
                !this.dateOrigBuyerDelivery.HasValue &&
                MyUtility.Check.Empty(this.StartRefno) &&
                MyUtility.Check.Empty(this.EndRefno))
            {
                MyUtility.Msg.WarningBox("<SP#>,<ETA>,<Arrive W/H Date>,<Buyer Delivery>,<Orig.Buyer Dlv>,<Refno> at least one entry is required");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 與BI共用Data Logic
            Warehouse_R21 biModel = new Warehouse_R21();
            Warehouse_R21_ViewModel warehouse_R21 = new Warehouse_R21_ViewModel()
            {
                ReportType = this._reportType,
                StartSPNo = this.StartSPNo,
                EndSPNo = this.EndSPNo,
                MDivision = this.MDivision,
                Factory = this.Factory,
                StartRefno = this.StartRefno,
                EndRefno = this.EndRefno,
                Color = this.Color,
                MT = this.MT,
                MtlTypeID = this.MtlTypeID,
                ST = this.ST,
                BoolCheckQty = this.boolCheckQty,
                BuyerDeliveryFrom = this.BuyerDelivery1,
                BuyerDeliveryTo = this.BuyerDelivery2,
                ETAFrom = this.ETA1,
                ETATo = this.ETA2,
                OrigBuyerDeliveryFrom = this.OrigBuyerDelivery1,
                OrigBuyerDeliveryTo = this.OrigBuyerDelivery2,
                Bulk = this.bulk,
                Sample = this.sample,
                Material = this.material,
                Smtl = this.smtl,
                Complete = this.complete,
                NoLocation = this.chkNoLocation.Checked,
                ArriveWHFrom = this.arriveWH1,
                ArriveWHTo = this.arriveWH2,
                WorkNo = this.WorkNo,
                StartLocation = this.location1,
                EndLocation = this.location2,
                IsPowerBI = false,
            };
            #region Get Data
            Base_ViewModel resultReport = biModel.GetWarehouse_R21Data(warehouse_R21);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.Dt;
            this.data_cnt = this.printData.Rows.Count;

            return Ict.Result.True;
            #endregion
            #endregion
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this.data_cnt == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(this.data_cnt);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            string reportname = string.Empty;
            if (this._reportType == 0)
            {
                reportname = "Warehouse_R21_Detail.xltx";
            }
            else
            {
                reportname = "Warehouse_R21_Summary.xltx";
            }

            if (this.data_cnt > 1000000)
            {
                Excel.Application objApp;
                Excel.Worksheet tmpsheep = null;
                Utility.Report.ExcelCOM com;
                string strExcelName = Class.MicrosoftFile.GetName((this._reportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary");
                int sheet_cnt = 1;
                int split_cnt = 500000;

                var sciYYYY = this.printData.AsEnumerable()
                       .Select(x => x["SciYYYY"].ToString())
                       .Distinct()
                       .ToList();
                string[] exl_name = new string[sciYYYY.Count];
                DataTable tmpTb;
                for (int i = 0; i < sciYYYY.Count; i++)
                {
                    strExcelName = Class.MicrosoftFile.GetName((this._reportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary" + sciYYYY[i].ToString());
                    exl_name[i] = strExcelName;
                    com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + reportname, null);
                    objApp = com.ExcelApp;
                    com.TransferArray_Limit = 10000;
                    com.ColumnsAutoFit = false;
                    sheet_cnt = 1;

                    var dt_yyyy = this.printData.Select($"SciYYYY = '{sciYYYY[i]}'").TryCopyToDataTable(this.printData);
                    dt_yyyy.Columns.Remove("SciYYYY");

                    // 如果筆數超過split_cnt再拆一次sheet
                    if (dt_yyyy.Rows.Count > split_cnt)
                    {
                        int max_sheet_cnt = (int)Math.Floor((decimal)(dt_yyyy.Rows.Count / split_cnt));
                        for (int j = 0; j <= max_sheet_cnt; j++)
                        {
                            if (j < max_sheet_cnt)
                            {
                                ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]).Copy(objApp.Workbooks[1].Worksheets[sheet_cnt]);
                            }

                            tmpsheep = (Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt];
                            tmpsheep.Name = sciYYYY[i].ToString() + "-" + (j + 1).ToString();
                            ((Excel.Worksheet)objApp.ActiveWorkbook.Sheets[sheet_cnt]).Select();
                            tmpTb = dt_yyyy.AsEnumerable().Skip(j * split_cnt).Take(split_cnt).CopyToDataTable();

                            DualResult ok = com.WriteTable(tmpTb, 2);

                            sheet_cnt++;
                            if (tmpTb != null)
                            {
                                tmpTb.Rows.Clear();
                                tmpTb.Constraints.Clear();
                                tmpTb.Columns.Clear();
                                tmpTb.ExtendedProperties.Clear();
                                tmpTb.ChildRelations.Clear();
                                tmpTb.ParentRelations.Clear();
                                tmpTb.Dispose();

                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();
                            }
                        }
                    }
                    else
                    {
                        // 複製sheet
                        tmpsheep = (Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt];
                        tmpsheep.Name = sciYYYY[i].ToString();
                        ((Excel.Worksheet)objApp.ActiveWorkbook.Sheets[sheet_cnt]).Select();
                        com.WriteTable(dt_yyyy, 2);
                        sheet_cnt++;
                    }

                    // 刪除多餘sheet
                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt + 1]).Delete();
                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]).Delete();
                    objApp.ActiveWorkbook.SaveAs(strExcelName);
                    if (tmpsheep != null)
                    {
                        Marshal.ReleaseComObject(tmpsheep);
                    }

                    for (int f = 1; f <= objApp.Workbooks[1].Worksheets.Count; f++)
                    {
                        Excel.Worksheet sheet = objApp.Workbooks[1].Worksheets.Item[f];
                        if (sheet != null)
                        {
                            Marshal.ReleaseComObject(sheet);
                        }
                    }

                    objApp.Workbooks[1].Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                Marshal.ReleaseComObject(tmpsheep);
                foreach (string filrstr in exl_name)
                {
                    filrstr.OpenFile();
                }
            }
            else
            {
                this.printData.Columns.Remove("SciYYYY");
                Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + reportname, null);
                Excel.Application objApp = com.ExcelApp;

                // MyUtility.Excel.CopyToXls(printData, "", reportname, 1, showExcel: false, excelApp: objApp);
                com.WriteTable(this.printData, 2);
                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName((this._reportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary");
                for (int f = 1; f <= objApp.Workbooks[1].Worksheets.Count; f++)
                {
                    Excel.Worksheet sheet = objApp.Workbooks[1].Worksheets.Item[f];
                    sheet.UsedRange.Rows.AutoFit();
                    if (sheet != null)
                    {
                        Marshal.ReleaseComObject(sheet);
                    }
                }

                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Workbooks[1].Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);

                strExcelName.OpenFile();
            }

            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void RadioGroupReportType_ValueChanged(object sender, EventArgs e)
        {
            if (this.radioGroupReportType.Value == "D")
            {
                this.chkNoLocation.ReadOnly = false;
                this.txtMtlLocation1.ReadOnly = false;
                this.txtMtlLocation2.ReadOnly = false;
            }
            else
            {
                this.txtMtlLocation1.Text = string.Empty;
                this.txtMtlLocation2.Text = string.Empty;
                this.txtMtlLocation1.ReadOnly = true;
                this.txtMtlLocation2.ReadOnly = true;
                this.chkNoLocation.ReadOnly = true;
                this.chkNoLocation.Checked = false;
            }
        }
    }
}
