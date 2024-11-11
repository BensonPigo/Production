using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Sci.Production.Planning
{
    /// <inheritdoc/>
    public partial class P09 : Win.Tems.QueryForm
    {
        private DataTable dataTable = new DataTable();

        /// <summary>
        /// 展開: By SewingLine, Sewing Date, SP, Factory(已是必輸入條件)
        /// </summary>
        /// <inheritdoc/>
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.grid1.DataSource = this.grid1bs;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("SewingLineID", header: "Sewing Line", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Date("SewingDate", header: "Sewing Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderID", header: "SP", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Date("Inline", header: "Sewing Inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("Offline", header: "Sewing Offline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("StdQty", header: "Standard Output/Day", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("CuttingOutput", header: "Cutting Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CuttingRemark", header: "Cutting Remark", width: Widths.AnsiChars(10), iseditable: true)
                .Numeric("LoadingOutput", header: "Loading Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("LoadingRemark", header: "Loading Remark", width: Widths.AnsiChars(10), iseditable: true)
                .CheckBox("LoadingExclusion", header: "Loading Exclusion", width: Widths.AnsiChars(1), iseditable: true)
                .Numeric("ATOutput", header: "AT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("ATRemark", header: "AT Remark", width: Widths.AnsiChars(10), iseditable: true)
                .CheckBox("ATExclusion", header: "AT Exclusion", width: Widths.AnsiChars(5), iseditable: true)
                .Numeric("AUTOutput", header: "AUT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("AUTRemark", header: "AUT Remark", width: Widths.AnsiChars(10), iseditable: true)
                .CheckBox("AUTExclusion", header: "AUT Exclusion", width: Widths.AnsiChars(1), iseditable: true)
                .Numeric("HTOutput", header: "HT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("HTRemark", header: "HT Remark", width: Widths.AnsiChars(10), iseditable: true)
                .CheckBox("HTExclusion", header: "HT Exclusion", width: Widths.AnsiChars(1), iseditable: true)
                .Numeric("BOOutput", header: "BO Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("BORemark", header: "BO Remark", width: Widths.AnsiChars(10), iseditable: true)
                .CheckBox("BOExclusion", header: "BO Exclusion", width: Widths.AnsiChars(1), iseditable: true)
                .Numeric("FMOutput", header: "FM Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("FMRemark", header: "FM Remark", width: Widths.AnsiChars(10), iseditable: true)
                .CheckBox("FMExclusion", header: "FM Exclusion", width: Widths.AnsiChars(1), iseditable: true)
                .Numeric("PRTOutput", header: "PRT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("PRTRemark", header: "PRT Remark", width: Widths.AnsiChars(10), iseditable: true)
                .CheckBox("PRTExclusion", header: "PRT Exclusion", width: Widths.AnsiChars(1), iseditable: true)
                ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (!this.QueryBefore())
            {
                return;
            }

            Base_ViewModel finalResult = new Base_ViewModel();
            Planning_P09 biModel = new Planning_P09();
            try
            {
                Planning_P09_ViewModel viewModel = new Planning_P09_ViewModel()
                {
                    MDivisionID = this.txtMdivision1.Text,
                    FactoryID = this.txtfactory1.Text,
                    SewingSDate = this.dateSewingInline.Value1,
                    SewingEDate = this.dateSewingInline.Value2,
                    SewingInlineSDate = this.dateSewingInline.Value1,
                    SewingInlineEDate = this.dateSewingInline.Value2,
                    SewingOfflineSDate = this.dateSewingOffline.Value1,
                    SewingOfflineEDate = this.dateSewingOffline.Value2,
                };

                Base_ViewModel resultReport = biModel.GetPlanning_P09(viewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                this.dataTable = resultReport.Dt;
                this.grid1.DataSource = this.dataTable;
                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            if (!finalResult.Result)
            {
                this.ShowErr(finalResult.Result);
                return;
            }
        }

        private bool QueryBefore()
        {
            // MDivision, Factory 不可空
            if (MyUtility.Check.Empty(this.txtMdivision1.Text) || MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                MyUtility.Msg.WarningBox("MDivision and Factory can not be empty!");
                return false;
            }

            // 3 個日期條件至少輸入一個
            if (!this.dateSewing.HasValue1 && !this.dateSewingInline.HasValue1 && !this.dateSewingOffline.HasValue1)
            {
                MyUtility.Msg.WarningBox("Date can not all be empty!");
                return false;
            }

            return true;
        }

        private void BtnDownloadTemplate_Click(object sender, EventArgs e)
        {

        }
    }
}
