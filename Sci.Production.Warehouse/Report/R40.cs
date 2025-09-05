using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// R40
    /// </summary>
    public partial class R40 : Win.Tems.PrintForm
    {
        private string strSp1;
        private string strSp2;
        private DateTime? arriveDateStart;
        private DateTime? arriveDateEnd;
        private string wkStart;
        private string wkEnd;
        private string updateInfo;
        private string status;
        private DataTable[] listResult;
        private DataTable mindDt;

        /// <summary>
        /// R40
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            Dictionary<string, string> updateInfo_source = new Dictionary<string, string>
            {
                { "ALL", "*" },
                { "Receiving Act. (kg)", "0" },
                { "Cut Shadeband", "1" },
                { "Fabric to Lab", "2" },
                { "Checker", "3" },
                { "Scanned by MIND", "4" },
            };
            this.comboUpdateInfo.DataSource = new BindingSource(updateInfo_source, null);
            this.comboUpdateInfo.DisplayMember = "Key";
            this.comboUpdateInfo.ValueMember = "Value";

            this.comboUpdateInfo.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateRangeArriveDate.HasValue &&
                MyUtility.Check.Empty(this.txtSPNoStart.Text) &&
                MyUtility.Check.Empty(this.txtSPNoEnd.Text) &&
                MyUtility.Check.Empty(this.txtWKStart.Text) &&
                MyUtility.Check.Empty(this.txtWKEnd.Text))
            {
                MyUtility.Msg.WarningBox("<Arrive W/H Date>, <SP#>, <WK#> can not be empty");
                return false;
            }

            this.arriveDateStart = this.dateRangeArriveDate.DateBox1.Value;
            this.arriveDateEnd = this.dateRangeArriveDate.DateBox2.Value;
            this.strSp1 = this.txtSPNoStart.Text.Trim();
            this.strSp2 = this.txtSPNoEnd.Text.Trim();
            this.wkStart = this.txtWKStart.Text.Trim();
            this.wkEnd = this.txtWKEnd.Text.Trim();
            this.updateInfo = this.comboUpdateInfo.SelectedValue.ToString().Trim();
            this.status = this.comboStatus.SelectedValue.ToString().Trim();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 與BI共用Data Logic
            Warehouse_R40 biModel = new Warehouse_R40();
            Warehouse_R40_ViewModel warehouse_R40 = new Warehouse_R40_ViewModel()
            {
                ArriveDateStart = this.arriveDateStart,
                ArriveDateEnd = this.arriveDateEnd,
                SP1 = this.strSp1,
                SP2 = this.strSp2,
                WKID1 = this.wkStart,
                WKID2 = this.wkEnd,
                UpdateInfo = this.updateInfo,
                BrandID = this.txtbrand.Text,
                Status = this.status,
                IsPowerBI = false,
            };

            Base_ViewModel resultReport = biModel.GetWarehouse_R40Data(warehouse_R40);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            if (this.updateInfo == "*" || this.updateInfo == "4")
            {
                this.mindDt = resultReport.Dt;
            }

            if (this.updateInfo != "4")
            {
                this.listResult = resultReport.DtArr;
            }
            #endregion

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            int dataCnt = 0;

            if (this.updateInfo == "*")
            {
                dataCnt = this.listResult.Sum(s => s.Rows.Count) + this.mindDt.Rows.Count;
            }
            else if (this.updateInfo != "4")
            {
                int dataNum = MyUtility.Convert.GetInt(this.updateInfo);
                dataCnt = this.listResult[dataNum].Rows.Count;
            }
            else
            {
                dataCnt = this.mindDt.Rows.Count;
            }

            this.SetCount(dataCnt);
            if (dataCnt == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            if (this.updateInfo != "4")
            {
                int serReport = 0;
                foreach (DataTable dataTable in this.listResult)
                {
                    string excelName = string.Empty;

                    if (this.updateInfo != "*" && this.updateInfo != serReport.ToString())
                    {
                        serReport++;
                        continue;
                    }

                    switch (serReport)
                    {
                        case 0:
                            excelName = "Warehouse_R40_ReceivingActkg";
                            break;
                        case 1:
                            excelName = "Warehouse_R40_CutShadeband";
                            break;
                        case 2:
                            excelName = "Warehouse_R40_FabrictoLab";
                            break;
                        case 3:
                            excelName = "Warehouse_R40_Checker";
                            break;
                        default:
                            continue;
                    }

                    Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app
                    if (dataTable.Rows.Count > 0)
                    {
                        MyUtility.Excel.CopyToXls(dataTable, null, $"{excelName}.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                    }

                    Excel.Worksheet worksheet = objApp.Sheets[1];
                    worksheet.Rows.AutoFit();
                    worksheet.Columns.AutoFit();

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName(excelName);
                    objApp.ActiveWorkbook.SaveAs(strExcelName);
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(worksheet);

                    strExcelName.OpenFile();
                    serReport++;
                    #endregion
                }
            }

            if (this.updateInfo == "*" || this.updateInfo == "4")
            {
                string excelName = "Warehouse_R40_ScannedbyMIND";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app
                if (this.mindDt.Rows.Count > 0)
                {
                    MyUtility.Excel.CopyToXls(this.mindDt, null, $"{excelName}.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                }

                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName(excelName);
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }

            this.HideWaitMessage();
            return true;
        }

        private void ComboUpdateInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Status下拉選單
            Dictionary<string, string> status_source = new Dictionary<string, string>
            {
                { "ALL", "All" },
                { "Already updated", "AlreadyUpdated" },
                { "Not yet Update", "NotYetUpdate" },
            };

            // Status下拉選單
            Dictionary<string, string> status_source_MIND = new Dictionary<string, string>
            {
                { "ALL", "All" },
                { "Already Scanned", "AlreadyScanned" },
                { "Not yet Scanned", "NotYetScanned" },
            };

            this.comboStatus.DataSource = null;
            if (this.comboUpdateInfo.SelectedValue.ToString() == "4")
            {
                this.comboStatus.DataSource = new BindingSource(status_source_MIND, null);
            }
            else
            {
                this.comboStatus.DataSource = new BindingSource(status_source, null);
            }

            this.comboStatus.DisplayMember = "Key";
            this.comboStatus.ValueMember = "Value";
            this.comboStatus.SelectedIndex = 0;
        }
    }
}
