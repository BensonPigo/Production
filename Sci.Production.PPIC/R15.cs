using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Sci.Utility.Excel;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R15 : Win.Tems.PrintForm
    {
        private SaveXltReportCls sxc;
        private DataTable dtPrint;
        private string sqlcmd;

        /// <inheritdoc/>
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.cboReportType.Add("Each Cons", "EC");
            this.cboReportType.Add("M/Notice", "MN");
            this.cboReportType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            string where = string.Empty;
            string type = this.cboReportType.SelectedValue2.ToString();
            where += $" and f.Type = '{type}'";

            #region 檢查必輸條件 & 加入SQL where 參數

            if (this.dateSCIDelivery.Value1 != null && this.dateSCIDelivery.Value2 != null)
            {
                where += $" and o.SciDelivery >='{((DateTime)this.dateSCIDelivery.Value1).ToString("yyyy-MM-dd")}'";
                where += $" and o.SciDelivery <='{((DateTime)this.dateSCIDelivery.Value2).ToString("yyyy-MM-dd 23:59:59")}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text) || !MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $" and f.ID >= '{this.txtSP1.Text}'";
                where += $" and f.ID <= '{this.txtSP2.Text.PadRight(13, 'Z')}'";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $" and o.BrandID = '{this.txtBrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtMR.TextBox1.Text))
            {
                where += $" and o.MRHandle = '{this.txtMR.TextBox1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSMR.TextBox1.Text))
            {
                where += $" and o.SMR = '{this.txtSMR.TextBox1.Text}'";
            }

            this.sqlcmd = $@"
Select 
      KPIFailed = iif(f.KPIFailed = 'N', 'Alread Failed', 'Failed Next Week') 
    , f.FailedComment
    , f.ExpectApvDate
    , f.KPIDate
    , o.ID
    , o.POID
    , o.BrandID
    , o.StyleID
    , o.FactoryID
    , o.Qty
    , SMR = GetSMR.IdAndName
    , MR = GetMR.IdAndName
    , o.SciDelivery
    , o.BuyerDelivery
    , o.SewInLIne
    , o.MnorderApv2
    , GetGMTLT.GMTLT
    , f.Type
From [Order_ECMNFailed] f
Left Join Orders o on f.id	= o.ID
Outer Apply(Select GMTLT = dbo.GetStyleGMTLT(o.BrandID,o.StyleID,o.SeasonID,o.FactoryID)) as GetGMTLT
Left join GetName as GetSMR on GetSMR.ID = o.SMR
Left join GetName as GetMR on GetMR.ID = o.MRHandle
Where 1 = 1
{where}
Order by f.ID
";
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(string.Empty, this.sqlcmd, out this.dtPrint);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrint.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtPrint.Rows.Count); // 顯示筆數

            try
            {
                string type = this.dtPrint.Rows[0]["Type"].ToString();
                this.dtPrint.Columns.Remove("Type");

                string xltPath = string.Empty;

                if (type.EqualString("EC"))
                {
                    xltPath = Path.Combine(Env.Cfg.XltPathDir, "PPIC_R15 Cutting check list (E.Cons).xltx");
                }
                else
                {
                    xltPath = Path.Combine(Env.Cfg.XltPathDir, "PPIC_R15 Mnotice Cutting check list (M.Notice).xltx");
                }

                this.sxc = new SaveXltReportCls(xltPath);
                SaveXltReportCls.XltRptTable xrt = new SaveXltReportCls.XltRptTable(this.dtPrint);
                Excel.Worksheet wks = this.sxc.ExcelApp.Sheets[1];
                wks.Cells[3, 1].Value = "##tbl";
                xrt.BoAutoFitColumn = true;
                xrt.ShowHeader = false;
                this.sxc.DicDatas.Add("##tbl", xrt);

                this.sxc.Save();
            }
            catch (Exception ex)
            {
                if (this.sxc.ExcelApp != null)
                {
                    this.sxc.ExcelApp.DisplayAlerts = false;
                    this.sxc.ExcelApp.Quit();
                }

                MyUtility.Msg.WarningBox("To Excel error." + ex);
                this.HideWaitMessage();
                return false;
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
