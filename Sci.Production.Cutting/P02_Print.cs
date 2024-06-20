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

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_Print : Win.Tems.PrintForm
    {
        private string S1;
        private string S2;
        private string Poid = string.Empty;
        private string cp;
        private string cr;
        private string keyword = Env.User.Keyword;
        private string cutrefSort;
        private int SheetCount = 1;
        private DataTable WorkorderTb;
        private DataTable WorkorderSizeTb;
        private DataTable WorkorderDisTb;
        private DataTable WorkorderPatternTb;
        private DataTable CutrefTb;
        private DataTable CutDisOrderIDTb;
        private DataTable CutSizeTb;
        private DataTable SizeTb;
        private DataTable CutQtyTb;
        private DataTable MarkerTB;
        private DataTable FabricComboTb;
        private DataTable IssueTb;
        private DataRow detDr;
        private DataRow OrderDr;
        private int _worktype;
        private string tableNameWorkOrder;

        /// <summary>
        /// Key = Table名稱，Value = 無欄位的清單
        /// </summary>
        private Dictionary<string, List<string>> dicTableLostColumns;
        private CuttingWorkOrder.FromCutting fromCutting;
        private DataTable[] arrDtType;
        private CuttingWorkOrder cuttingWorkOrder;
        private string printType;
        private string sortType;

        /// <summary>
        /// Initializes a new instance of the <see cref="P02_Print"/> class.
        /// </summary>
        /// <param name="fromCutting">Type FromCutting</param>
        /// <param name="workorderDr">workorder Dr，基於fromCutting參數，會有兩種：WorkOrderForPlanning、WorkOrderForOutput</param>
        /// <param name="poid">POID</param>
        /// <param name="worktype">Work Type</param>
        public P02_Print(CuttingWorkOrder.FromCutting fromCutting, DataRow workorderDr, string poid, int worktype, bool isTest = false)
        {
            this.InitializeComponent();
            this.fromCutting = fromCutting;
            this.tableNameWorkOrder = fromCutting == CuttingWorkOrder.FromCutting.P02 ? "WorkOrderForPlanning" : "WorkOrderForOutput";
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

            this.Poid = poid;
            this._worktype = worktype;
            this.radioByCutRefNo.Checked = true;
            this.txtCutRefNoStart.Text = this.detDr["CutRef"].ToString();
            this.txtCutRefNoEnd.Text = this.detDr["CutRef"].ToString();
            this.cr = this.detDr["CutRef"].ToString();
            this.cp = this.cuttingWorkOrder.CheckTableLostColumns(this.fromCutting, "CutplanID") ? string.Empty : this.detDr["CutplanID"].ToString();
            this.txtCutRefNoStart.Select();
            this.cmbSort.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 僅P02可用
            this.radioByCutplanId.Visible = this.fromCutting == CuttingWorkOrder.FromCutting.P02;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.S1 = this.txtCutRefNoStart.Text;
            this.S2 = this.txtCutRefNoEnd.Text;

            if (MyUtility.Check.Empty(this.S1) || MyUtility.Check.Empty(this.S2))
            {
                MyUtility.Msg.WarningBox("<Range> can not be empty", "Warning");
                return false;
            }

            this.printType = this.radioByCutRefNo.Checked ? "Cutref" : "Cutplanid";
            this.sortType = this.cmbSort.Text == "CutRef#" ? "Cutref" : "SpreadingNoID,CutCellID,Cutref";

            return base.ValidateInput();
        }

        private void RadioByCutplanId_CheckedChanged(object sender, EventArgs e)
        {
            this.txtCutRefNoStart.Text = this.radioByCutplanId.Checked ? this.cp : this.cr;
            this.txtCutRefNoEnd.Text = this.radioByCutplanId.Checked ? this.cp : this.cr;
            this.labelCutRefNo.Text = this.radioByCutplanId.Checked ? "Cutplan ID" : "Cut RefNo";
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.arrDtType = new DataTable[] { };
            DualResult dualResult = this.cuttingWorkOrder.GetPrintData(this.fromCutting, this.detDr, this.S1, this.S2, this.printType, this.sortType, out this.arrDtType);
            return dualResult;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string errMsg;
            bool result = this.cuttingWorkOrder.PrintToExcel(this.detDr["ID"].ToString(), this.arrDtType, this.fromCutting, this.printType, out errMsg);
            if (!result && !string.IsNullOrEmpty(errMsg))
            {
                MyUtility.Msg.ErrorBox(errMsg);
                return false;
            }

            return true;
        }
    }
}
