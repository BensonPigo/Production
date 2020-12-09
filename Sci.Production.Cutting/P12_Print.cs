using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Sci.Production.PublicPrg.Prgs;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P12_Print : Win.Tems.PrintForm
    {
        private List<P10_PrintData> p10_PrintDatas;

        /// <inheritdoc/>
        public P12_Print(List<P10_PrintData> p10_PrintDatas)
        {
            this.InitializeComponent();
            this.p10_PrintDatas = p10_PrintDatas;
            this.toexcel.Enabled = false;
            this.linkLabelRFCardEraseBeforePrinting1.SetText();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool OnToPrint(ReportDefinition report)
        {
            if (this.radioBundleCardRF.Checked)
            {
                // 是否有資料
                if (this.p10_PrintDatas == null || this.p10_PrintDatas.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                try
                {
                    bool rfCardErase = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select RFCardEraseBeforePrinting from [System]"));
                    this.ShowWaitMessage("Process Print!");
                    DualResult result = Prg.BundleRFCard.BundleRFCardPrintAndRetry(this.p10_PrintDatas, 0, rfCardErase);
                    this.HideWaitMessage();
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox(result.ToString());
                        return false;
                    }

                    MyUtility.Msg.InfoBox("Printed success, Please check result in Bin Box.");
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox(ex.ToString());
                    return false;
                }
            }
            else if (this.radioBundleErase.Checked)
            {
                this.ShowWaitMessage("Process Erase!");

                // 放在Stacker的所有卡片擦除
                DualResult result = Prg.BundleRFCard.BundleRFErase();
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    this.HideWaitMessage();
                    return false;
                }

                this.HideWaitMessage();
                MyUtility.Msg.InfoBox("Erase success, Please check result in Bin Box.");
            }

            return true;
        }

        private void P12_Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
