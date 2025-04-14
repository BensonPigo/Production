using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Sci.Production.PublicPrg;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class R13 : Win.Tems.PrintForm
    {
        private System.Data.DataTable printData;
        private DateTime? CreateDate;
        private string FileType;

        /// <summary>
        /// Initializes a new instance of the <see cref="R13"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.FileType = "SewingR13";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateGenerateDate.Value) || MyUtility.Check.Empty(this.dateCreateDate.Value))
            {
                MyUtility.Msg.WarningBox("Please fill in Generate Date!");
                this.dateGenerateDate.Focus();
                return false;
            }

            if (this.dateGenerateDate.Value > this.dateCreateDate.Value)
            {
                MyUtility.Msg.WarningBox("Generate Date1 cannot greater than Generate Date2.");
                this.dateGenerateDate.Focus();
                return false;
            }

            this.CreateDate = this.dateCreateDate.Value;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            #region WHERE條件
            if (!MyUtility.Check.Empty(this.CreateDate))
            {
                sqlWhere.Append($"AND CreateDate <= '{this.CreateDate.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            #endregion

            this.printData = Prgs.GetVNCustomsReportData(sqlWhere, this.FileType);
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            DualResult result;

            if (!(result = Prgs.GetVNCustomsReport(this.printData, this.FileType)))
            {
                this.ShowErr(result.ToString());
                return false;
            }

            return true;
        }

        private void txtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"select id,startdate,EndDate from [Production].[dbo].[VNContract] WITH (NOLOCK)", "20,10,10", this.Text, false, ",", headercaptions: "Contract No, Start Date, End Date");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtContractNo.Text = item.GetSelectedString();
        }
    }
}
