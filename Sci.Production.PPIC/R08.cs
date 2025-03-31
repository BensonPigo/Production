using System;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R08
    /// </summary>
    public partial class R08 : Win.Tems.PrintForm
    {
        private System.Data.DataTable[] _printData;
        private DateTime? Cdate1;
        private DateTime? Cdate2;
        private DateTime? Apvdate1;
        private DateTime? Apvdate2;
        private DateTime? Lockdate1;
        private DateTime? Lockdate2;
        private DateTime? Cfmdate1;
        private DateTime? Cfmdate2;
        private DateTime? Voucher1;
        private DateTime? Voucher2;
        private string MDivisionID;
        private string FtyGroup;
        private string T;
        private string Status;
        private string Sharedept;
        private string rType;
        private bool IncludeJunk;
        private bool ReplacementReport;

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            System.Data.DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, ",,F,Fabric,A,Accessory,");
            MyUtility.Tool.SetupCombox(this.cmbStatus, 1, 1, "ALL,Approved,Auto.Lock,Checked,Confirmed,Junked,New");
            MyUtility.Tool.SetupCombox(this.cmbReportType, 2, 1, "0,Detail List,1,Resp. Dept. List");
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Env.User.Keyword;
            this.comboType.SelectedIndex = 0;
            this.cmbStatus.SelectedIndex = 0;
            this.cmbReportType.SelectedIndex = 0;
            this.comboFactory.Text = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Cdate1 = this.dateCreateDate.Value1;
            this.Cdate2 = this.dateCreateDate.Value2;
            this.Apvdate1 = this.dateApvDate.Value1;
            this.Apvdate2 = this.dateApvDate.Value2;
            this.Lockdate1 = this.dateLock.Value1;
            this.Lockdate2 = this.dateLock.Value2;
            this.Cfmdate1 = this.dateCfm.Value1;
            this.Cfmdate2 = this.dateCfm.Value2;
            this.Voucher1 = this.dateVoucher.Value1;
            this.Voucher2 = this.dateVoucher.Value2;
            this.MDivisionID = this.comboM.Text;
            this.FtyGroup = this.comboFactory.Text;
            this.T = MyUtility.Convert.GetString(this.comboType.SelectedValue);
            this.Status = this.cmbStatus.Text;
            this.Sharedept = this.txtSharedept.Text;
            this.rType = this.cmbReportType.Text;
            this.IncludeJunk = this.chkJunk.Checked;
            this.ReplacementReport = this.chkReplacementReport.Checked;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            PPIC_R08 biModel = new PPIC_R08();
            PPIC_R08_ViewModel ppic_R08 = new PPIC_R08_ViewModel()
            {
                 CDate1 = this.Cdate1,
                 CDate2 = this.Cdate2,
                 ApvDate1 = this.Apvdate1,
                 ApvDate2 = this.Apvdate2,
                 Lockdate1 = this.Lockdate1,
                 Lockdate2 = this.Lockdate2,
                 Cfmdate1 = this.Cfmdate1,
                 Cfmdate2 = this.Cfmdate2,
                 Voucher1 = this.Voucher1,
                 Voucher2 = this.Voucher2,
                 MDivisionID = this.MDivisionID,
                 FactoryID = this.FtyGroup,
                 T = this.ReportType,
                 Status = this.Status,
                 Sharedept = this.Sharedept,
                 ReportType = this.rType,
                 IncludeJunk = this.IncludeJunk,
                 IsReplacementReport = this.ReplacementReport,
                 IsPowerBI = false,
            };

            Base_ViewModel resultReport = biModel.GetPPIC_R08Data(ppic_R08);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this._printData = resultReport.DtArr;

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData[0].Rows.Count);

            if (this._printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string filename = string.Empty;
            if (this.rType == "Detail List")
            {
                filename = "PPIC_R08_DetailList";
            }
            else
            {
                filename = "PPIC_R08_RespDeptList";
            }

            this.ShowWaitMessage("Excel Processing...");
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{filename}.xltx");
            MyUtility.Excel.CopyToXls(this._printData[0], string.Empty, $"{filename}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]); // 將datatable copy to excel
            if (this._printData.Length == 2)
            {
                MyUtility.Excel.CopyToXls(this._printData[1], string.Empty, $"{filename}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[2]); // 將datatable copy to excel
            }

            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表
            worksheet.Columns.AutoFit();
            if (this._printData.Length == 2)
            {
                worksheet = excelApp.ActiveWorkbook.Worksheets[2]; // 取得工作表
                worksheet.Columns[28].ColumnWidth = 66;
            }

            worksheet.Rows.AutoFit();
            excelApp.Visible = true;
            #region 釋放上面開啟過excel物件
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excelApp);
            #endregion

            this.HideWaitMessage();
            return true;
        }

        private void TxtSharedept_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = "select ID,Name from FinanceEN.dbo.Department";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "12,15", this.txtSharedept.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSharedept.Text = item.GetSelectedString();
        }

        private void CmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isCheck = this.cmbReportType.SelectedIndex == 0;
            this.chkJunk.Enabled = isCheck;
            this.chkReplacementReport.Enabled = isCheck;

            this.chkJunk.Checked = isCheck ? this.chkJunk.Checked : isCheck;
            this.chkReplacementReport.Checked = isCheck ? this.chkReplacementReport.Checked : isCheck;
        }
    }
}
