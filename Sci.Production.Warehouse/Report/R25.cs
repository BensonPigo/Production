using Ict;
using Sci.Data;
using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R25 : Win.Tems.PrintForm
    {
        private string sqlcmd;
        private DataTable dataTable;

        /// <inheritdoc/>
        public R25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex(true);
        }

        private void Txtfactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select distinct ID from Factory WITH (NOLOCK) order by ID";
            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, "Factory", "10", this.txtfactory.Text, null, null, null);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtfactory.Text = item.GetSelectedString();
        }

        private void Txtfactory_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtfactory.Text))
            {
                return;
            }

            string[] str_multi = this.txtfactory.Text.Split(',');
            string err_factory = string.Empty;
            foreach (string chk_str in str_multi)
            {
                if (MyUtility.Check.Seek(chk_str, "Factory", "ID", "Production") == false)
                {
                    err_factory += "," + chk_str;
                }
            }

            if (!err_factory.Equals(string.Empty))
            {
                this.txtfactory.Text = string.Empty;
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", err_factory.Substring(1)));
                return;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.datekPIETA.Value1) &&
                MyUtility.Check.Empty(this.dateWhseArrival.Value1) &&
                MyUtility.Check.Empty(this.dateETA.Value1) &&
                (MyUtility.Check.Empty(this.txtWK1.Text) && MyUtility.Check.Empty(this.txtWK2.Text)) &&
                (MyUtility.Check.Empty(this.txtSP1.Text) && MyUtility.Check.Empty(this.txtSP2.Text)))
            {
                MyUtility.Msg.WarningBox("KPI L/ETA, Arrive W/H, ETA, WK#, SP# can not all empty!");
                return false;
            }

            string kPIETA1 = !this.datekPIETA.HasValue1 ? "NULL" : "'" + ((DateTime)this.datekPIETA.Value1).ToString("yyyy/MM/dd") + "'";
            string kPIETA2 = !this.datekPIETA.HasValue2 ? "NULL" : "'" + ((DateTime)this.datekPIETA.Value2).ToString("yyyy/MM/dd") + "'";
            string whseArrival1 = !this.dateWhseArrival.HasValue1 ? "NULL" : "'" + ((DateTime)this.dateWhseArrival.Value1).ToString("yyyy/MM/dd") + "'";
            string whseArrival2 = !this.dateWhseArrival.HasValue2 ? "NULL" : "'" + ((DateTime)this.dateWhseArrival.Value2).ToString("yyyy/MM/dd") + "'";
            string eTA1 = !this.dateETA.HasValue1 ? "NULL" : "'" + ((DateTime)this.dateETA.Value1).ToString("yyyy/MM/dd") + "'";
            string eTA2 = !this.dateETA.HasValue2 ? "NULL" : "'" + ((DateTime)this.dateETA.Value2).ToString("yyyy/MM/dd") + "'";

            this.sqlcmd = $@"
SELECT
    WK
   ,ETA
   ,FactoryID
   ,Consignee
   ,ShipModeID
   ,CYCFS
   ,Blno
   ,Packages
   ,Vessel
   ,ProdFactory
   ,OrderTypeID
   ,ProjectID
   ,Category
   ,BrandID
   ,SeasonID
   ,StyleID
   ,StyleName
   ,PoID
   ,Seq
   ,FabricCombo
   ,Refno
   ,Color
   ,ColorName
   ,[Description]
   ,MtlType
   ,SubMtlType
   ,WeaveType
   ,SuppID
   ,SuppName
   ,UnitID
   ,SizeSpec
   ,ShipQty
   ,FOC
   ,NetKg
   ,WeightKg
   ,ArriveQty
   ,ArriveQtyStockUnit
   ,FirstBulkSewInLine
   ,FirstCutDate
   ,ReceiveQty
   ,TotalRollsCalculated
   ,StockUnit
   ,MCHandle
   ,ContainerType
   ,ContainerNo
   ,PortArrival
   ,WhseArrival
   ,KPILETA
   ,PFETA
   ,EarliestSCIDelivery
   ,EarlyDays
   ,EarliestPFETA
   ,MRMail
   ,SMRMail
   ,EditName
FROM dbo.Warehouse_Report_R25 (
    0--@ImportBI
    ,NULL--@ImportBI StartTime
    ,NULL--@ImportBI EndTime
    ,'{this.txtWK1.Text}'--@WK1
    ,'{this.txtWK2.Text}'--@WK2
    ,'{this.txtSP1.Text}'--@POID1
    ,'{this.txtSP2.Text}'--@POID2
    ,'{this.txtsupplier1.TextBox1.Text}'--@SuppID
    ,'{this.comboDropDownList1.SelectedValue.ToString().Replace("'", string.Empty)}'--@FabricType
    ,{whseArrival1}--@WhseArrival1
    ,{whseArrival2}--@WhseArrival2
    ,{eTA1}--@Eta1
    ,{eTA2}--@Eta2
    ,'{this.txtbrand1.Text}'--@BrandID
    ,'{this.comboMDivision1.Text}'--@MDivisionID
    ,'{this.txtfactory.Text}'--@FactoryID
    ,{kPIETA1}--@KPILETA1
    ,{kPIETA2}--@KPILETA2
    ,{(this.chkRecLessArv.Checked ? 1 : 0)}--@RecLessArv
    )
";
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 600;  // 加長時間為10分鐘，避免timeout
            DualResult result = DBProxy.Current.Select(null, this.sqlcmd, out this.dataTable);
            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dataTable.Rows.Count);
            if (this.dataTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string fileName = "Warehouse_R25";
            string fileNamexltx = fileName + ".xltx";
            Excel.Application excelApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\" + fileNamexltx, excelApp) { ColumnsAutoFit = false };
            com.WriteTable(this.dataTable, 2);
            Excel.Worksheet worksheet = excelApp.Sheets[1];
            this.CreateCustomizedExcel(ref worksheet);
            string excelfile = Class.MicrosoftFile.GetName(fileName);
            excelApp.ActiveWorkbook.SaveAs(excelfile);
            excelApp.Visible = true;
            Marshal.ReleaseComObject(excelApp);
            this.HideWaitMessage();
            return true;
        }
    }
}