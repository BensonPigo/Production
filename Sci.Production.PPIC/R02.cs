using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private PPIC_R02_ViewModel viewModel;
        private DataTable printData;

        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex(true);
            MyUtility.Tool.SetupCombox(this.comboPrintType, 1, 1, "ALL,MR Not Send,MR Send Not Receive,Factory Receive");
            this.comboPrintType.SelectedIndex = 0;
            this.comboM.Enabled = false;
            this.comboM.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("SCI Delivery can't empty!!");
                return false;
            }

            this.viewModel = new PPIC_R02_ViewModel()
            {
                SciDelivery1 = this.dateSCIDelivery.Value1,
                SciDelivery2 = this.dateSCIDelivery.Value2,
                ProvideDate1 = this.dateProvideDate.Value1,
                ProvideDate2 = this.dateProvideDate.Value2,
                ReceiveDate1 = this.dateFtyMRRcvDate.Value1,
                ReceiveDate2 = this.dateFtyMRRcvDate.Value2,
                BrandID = this.txtbrand.Text,
                StyleID = this.txtstyle.Text,
                SeasonID = this.txtseason.Text,
                MDivisionID = this.comboM.Text,
                MRHandle = this.txttpeuser_caneditMR.TextBox1.Text,
                SMR = this.txttpeuser_caneditSMR.TextBox1.Text,
                PoHandle = this.txttpeuser_caneditPOHandle.TextBox1.Text,
                POSMR = this.txttpeuser_caneditPOSMR.TextBox1.Text,
                PrintType = this.comboPrintType.SelectedIndex,
            };
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            PPIC_R02 biModel = new PPIC_R02();
            Base_ViewModel resultReport = biModel.GetPPIC_R02(this.viewModel);
            this.printData = resultReport.Dt;
            return resultReport.Result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R02_ProductionKits.xltx");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "PPIC_R02_ProductionKits.xltx", 1, true, string.Empty, objApp);

            return true;
        }
    }
}
