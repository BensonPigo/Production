using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;
using System.Runtime.InteropServices;

namespace Sci.Production.Basic
{
    /// <inheritdoc/>
    public partial class R01 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private StringBuilder sqlWhere = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="R01"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            #region combo box預設值

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("All", string.Empty);
            dic.Add("New", "New");
            dic.Add("Approved", "Confirmed");
            this.comboStatus.DataSource = new BindingSource(dic, null);
            this.comboStatus.ValueMember = "Value";
            this.comboStatus.DisplayMember = "Key";
            #endregion
        }

        private void TxtCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item;
            string sqlcmd;
            sqlcmd = @"
select l.id ,l.abb ,l.currencyid 
from LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0 AND l.IsFactory = 0
order by ID
";

            item = new Win.Tools.SelectItem(sqlcmd, "10,15,5", null);
            item.Size = new System.Drawing.Size(480, 500);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.txtCode.Text = selectedData[0]["id"].ToString();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlWhere.Clear();
            if (this.dateAdd.Value1.HasValue)
            {
                this.sqlWhere.Append($"AND l.AddDate >= '{this.dateAdd.Value1.Value.ToShortDateString()}' " + Environment.NewLine);
            }

            if (this.dateAdd.Value2.HasValue)
            {
                this.sqlWhere.Append($"AND l.AddDate <= '{this.dateAdd.Value2.Value.AddDays(1).AddSeconds(-1).ToShortDateString()}' " + Environment.NewLine);
            }

            if (this.dateApv.Value1.HasValue)
            {
                this.sqlWhere.Append($"AND lb.ApproveDate >= '{this.dateApv.Value1.Value.ToShortDateString()}' " + Environment.NewLine);
            }

            if (this.dateApv.Value2.HasValue)
            {
                this.sqlWhere.Append($"AND lb.ApproveDate <= '{this.dateApv.Value2.Value.AddDays(1).AddSeconds(-1).ToShortDateString()}' " + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(this.txtCode.Text))
            {
                this.sqlWhere.Append($"AND l.ID='{this.txtCode.Text}' " + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(this.comboStatus.SelectedValue.ToString()))
            {
                this.sqlWhere.Append($"AND lb.Status='{this.comboStatus.SelectedValue.ToString()}' " + Environment.NewLine);
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
SELECT DISTINCT
l.ID
, l.Name
, [ByCheck]=IIF(lb.ByCheck=1,'Y','N')
, [IsApprove]=IIF(lb.Status='Confirmed','Y','N') 
, lb.ApproveName
, lb.ApproveDate
, l.Address
, l.Tel
, l.Fax
, l.PayTermID
, l.CurrencyID
, l.Remark
, lbd.AccountNo
, lbd.AccountName
, lbd.BankName
, lbd.SWIFTCode
, lbd.BranchCode
, lbd.BranchName
, [Country]=c.Alias --lbd.CountryID
, lbd.City

FROM LocalSupp l
INNER JOIN LocalSupp_Bank lb ON l.ID=lb.ID
INNER JOIN LocalSupp_Bank_Detail lbd ON lb.ID = lbd.ID AND lb.PKey=lbd.Pkey
LEFT JOIN Country c ON lbd.CountryID=c.ID
WHERE 1=1

");
            sqlCmd.Append(this.sqlWhere);
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);

            this.ShowWaitMessage("Excel Processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Basic_R01.xltx"); // 預先開啟excel app

            MyUtility.Excel.CopyToXls(this.printData, null, "Basic_R01.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Basic_R01");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();

            return true;
        }
    }
}
