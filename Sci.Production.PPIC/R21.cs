﻿using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{

    /// <summary>
    /// R21
    /// </summary>
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        private DataTable dtPrintData;
        private PPIC_R21_ViewModel model;
        private readonly List<string> listProcess = new List<string>()
        {
                string.Empty,
                "Dry Room Receive",
                "Dry Room Transfer",
                "Transfer To Packing Error",
                "Confirm Packing Error Revise",
                "Scan & Pack",
                "MD Scan",
                "Fty Transfer To Clog",
                "Clog Receive",
                "Clog Return",
                "Clog Transfer To CFA",
                "Clog Receive From CFA",
                "CFA Receive",
                "CFA Return",
        };

        /// <summary>
        /// R21
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.comboProcess, 1, 1, this.listProcess.JoinToString(","));
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateRangeBuyerDelivery.HasValue &&
                MyUtility.Check.Empty(this.comboProcess.Text))
            {
                MyUtility.Msg.WarningBox("Please input < Buyer Delivery > or < Process >.");
                return false;
            }

            this.model = new PPIC_R21_ViewModel()
            {
                BuyerDeliveryFrom = this.dateRangeBuyerDelivery.DateBox1.Value,
                BuyerDeliveryTo = this.dateRangeBuyerDelivery.DateBox2.Value,
                DateTimeProcessFrom = this.dateTimeProcessFrom.Value,
                DateTimeProcessTo = this.dateTimeProcessTo.Value,
                ComboProcess = this.comboProcess.Text,
                MDivisionID = this.txtMdivision.Text,
                FactoryID = this.txtfactory.Text,
                ExcludeSisterTransferOut = this.chkExcludeSisterTransferOut.Checked,
                IncludeCancelOrder = this.cbIncludeCencelOrder.Checked,
                IsBI = false,
            };

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            PPIC_R21 biModel = new PPIC_R21();
            Base_ViewModel resultReport = biModel.GetCartonStatusTrackingList(this.model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.dtPrintData = resultReport.Dt;
            return resultReport.Result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrintData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtPrintData.Rows.Count); // 顯示筆數

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R21_Carton_Status_Tracking_List.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dtPrintData, string.Empty, "PPIC_R21_Carton_Status_Tracking_List.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            objApp.Visible = true;

            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
