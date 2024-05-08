﻿using Ict;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// P06_Print
    /// </summary>
    public partial class P06_Print : Sci.Win.Tems.PrintForm
    {
        private P06_PrintData printData;
        private string almID;
        private DataTable rightDataTable;

        /// <summary>
        /// P06_Print
        /// </summary>
        /// <param name="almID">almID</param>
        /// <param name="rightDataTable">rightDataTable</param>
        public P06_Print(string almID, DataTable rightDataTable = null)
        {
            this.InitializeComponent();
            this.almID = almID;
            this.rightDataTable = rightDataTable;
            this.printData = new P06_PrintData();
            MyUtility.Tool.SetupCombox(this.cbDirection, 1, 1, "F,B");
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.printData.SetCondition(this.almID, this.comboDisplayBy.SelectedValue.ToString(), this.comboContentBy.SelectedValue.ToString(), this.comboLanguageBy.SelectedValue.ToString(), rightDataTable: this.rightDataTable);

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            bool loadResult = this.printData.LoadData();

            if (!loadResult)
            {
                return new DualResult(false);
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Starting EXCEL...");
            bool toExcelResult = this.printData.ToExcel();
            this.HideWaitMessage();
            return toExcelResult;
        }
    }
}
