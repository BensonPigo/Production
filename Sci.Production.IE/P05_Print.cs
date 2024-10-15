using Ict;
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
    /// P05_Print
    /// </summary>
    public partial class P05_Print : Sci.Win.Tems.PrintForm
    {
        private P05_PrintData printData;
        private string almID;
        /// <summary>
        /// P05_Print
        /// </summary>
        /// <param name="almID">almID</param>
        public P05_Print(string almID)
        {
            this.InitializeComponent();
            this.almID = almID;
            this.printData = new P05_PrintData();
            MyUtility.Tool.SetupCombox(this.cbDirection, 1, 1, "F,B");
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.printData.SetCondition(this.almID, this.comboDisplayBy.SelectedValue.ToString(), this.comboContentBy.SelectedValue.ToString(), this.comboLanguageBy.SelectedValue.ToString(), this.cbDirection.SelectedValue.ToString());

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
