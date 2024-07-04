using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Linq;
using ZXing;
using ZXing.QrCode.Internal;
using ZXing.QrCode;
using System.Text.RegularExpressions;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class Cutting_Print_P02 : Win.Tems.PrintForm
    {
        private string Range_S;
        private string Range_E;
        private string CutplanID;
        private string CutRef;
        private DataRow detDr;

        /// <summary>
        /// Key = Table名稱，Value = 無欄位的清單
        /// </summary>
        private DataTable[] arrDtType;
        private CuttingWorkOrder cuttingWorkOrder;
        private string printType;
        private string sortType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cutting_Print"/> class.
        /// </summary>
        /// <param name="fromCutting">Type FromCutting</param>
        /// <param name="workorderDr">workorder Dr，基於fromCutting參數，會有兩種：WorkOrderForPlanning、WorkOrderForOutput</param>
        /// <param name="poid">POID</param>
        /// <param name="worktype">Work Type</param>
        public Cutting_Print_P02(DataRow workorderDr, bool isTest = false)
        {
            this.InitializeComponent();

            this.cuttingWorkOrder = new CuttingWorkOrder();

            this.detDr = workorderDr;
            if (isTest)
            {
                DataTable dtTest = new DataTable();
                dtTest.ColumnsStringAdd("CutRef");
                dtTest.ColumnsStringAdd("CutplanID");
                dtTest.ColumnsStringAdd("ID");
                DataRow drTest = dtTest.NewRow();
                drTest["CutRef"] = "0000JG";
                drTest["CutplanID"] = "VM3CP18110008";
                drTest["ID"] = "18100603PP";
                this.detDr = drTest;
                dtTest.Rows.Add(drTest);
                dtTest.AcceptChanges();
            }

            this.radioByCutRefNo.Checked = true;

            this.CutRef = this.detDr["CutRef"].ToString();
            this.txtCutRefNoStart.Text = this.CutRef;
            this.txtCutRefNoEnd.Text = this.CutRef;
            this.txtCutRefNoStart.Select();

            this.CutplanID = this.cuttingWorkOrder.CheckTableLostColumns(CuttingForm.P02, "CutplanID") ? string.Empty : this.detDr["CutplanID"].ToString();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.printType = this.radioByCutRefNo.Checked ? "Cutref" : "Cutplanid";
            this.sortType = "Cutref";

            if (this.printType == "Cutref")
            {
                this.Range_S = this.txtCutRefNoStart.Text;
                this.Range_E = this.txtCutRefNoEnd.Text;
            }
            else
            {
                this.Range_S = this.txtCutPlanStart.Text;
                this.Range_E = this.txtCutPlanEnd.Text;
            }

            if (MyUtility.Check.Empty(this.Range_S) || MyUtility.Check.Empty(this.Range_E))
            {
                MyUtility.Msg.WarningBox("<Range> can not be empty", "Warning");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.arrDtType = new DataTable[] { };
            DualResult dualResult = this.cuttingWorkOrder.GetPrintData(CuttingForm.P02, this.detDr, this.Range_S, this.Range_E, this.printType, this.sortType, out this.arrDtType);
            return dualResult;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string errMsg;
            bool result = this.cuttingWorkOrder.PrintToExcel(this.detDr["ID"].ToString(), this.arrDtType, CuttingForm.P02, this.printType, out errMsg);
            if (!result && !string.IsNullOrEmpty(errMsg))
            {
                MyUtility.Msg.ErrorBox(errMsg);
                return false;
            }

            return true;
        }

        private void RadioByCutRefNo_CheckedChanged(object sender, EventArgs e)
        {
            this.txtCutRefNoStart.Text = this.CutRef;
            this.txtCutRefNoEnd.Text = this.CutRef;
            this.txtCutPlanStart.Text = string.Empty;
            this.txtCutPlanEnd.Text = string.Empty;
        }

        private void RadioByCutplanId_CheckedChanged(object sender, EventArgs e)
        {
            this.txtCutRefNoStart.Text = string.Empty;
            this.txtCutRefNoEnd.Text = string.Empty;
            this.txtCutPlanStart.Text = this.CutplanID;
            this.txtCutPlanEnd.Text = this.CutplanID;
        }
    }
}
