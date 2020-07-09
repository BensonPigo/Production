using System.Data;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R23 : Win.Tems.PrintForm
    {
        public R23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1)
                && MyUtility.Check.Empty(this.dateSCIDelivery.Value2)
                && MyUtility.Check.Empty(this.txtSPNo.Text)
                && MyUtility.Check.Empty(this.txtMtlLocationStart.Text)
                && MyUtility.Check.Empty(this.txtLocationEnd.Text))
            {
                MyUtility.Msg.WarningBox("SP#, SCI Delivery, Location can't be empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        DataTable dt;

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string spno = this.txtSPNo.Text.Trim();
            string sciDelivery1 = MyUtility.Check.Empty(this.dateSCIDelivery.TextBox1.Value) ? string.Empty : this.dateSCIDelivery.TextBox1.Text;
            string sciDelivery2 = MyUtility.Check.Empty(this.dateSCIDelivery.TextBox2.Value) ? string.Empty : this.dateSCIDelivery.TextBox2.Text;
            string location1 = this.txtMtlLocationStart.Text;
            string location2 = this.txtLocationEnd.Text;
            string factory = this.txtfactory.Text;
            bool chkbalance = this.checkBalanceQty.Checked;

            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
select
    o.ftygroup ,
    a.OrderID,
	a.UnitID,
	a.Refno,
	b.Description,
	o.SciDelivery,
	Supplier = concat(c.ID, '-', c.Abb),
	a.ThreadColorID,
	a.InQty,
	a.OutQty,
	a.AdjustQty,
	Balance = a.InQty - a.OutQty + a.AdjustQty,
	a.LobQty,
	a.ALocation
from LocalInventory a with(nolock)
left join Orders o with(nolock) on o.id = a.OrderID
left join LocalItem b with(nolock) on b.RefNo = a.Refno
left join LocalSupp c with(nolock) on c.ID = b.LocalSuppid
where 1=1
");
            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append($" and a.OrderID like '{spno}%'");
            }

            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlcmd.Append($" and o.SciDelivery between '{sciDelivery1}' and '{sciDelivery2}'");
            }

            if (!MyUtility.Check.Empty(location1))
            {
                sqlcmd.Append($" and a.ALocation >= '{location1}'");
            }

            if (!MyUtility.Check.Empty(location2))
            {
                sqlcmd.Append($" and a.ALocation <= '{location2}'");
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd.Append($" and o.ftygroup = '{factory}'");
            }

            if (chkbalance)
            {
                sqlcmd.Append(" and a.InQty - a.OutQty + a.AdjustQty > 0");
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd.ToString(), out this.dt);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dt.Rows.Count);
            DualResult result = Ict.Result.True;
            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return result;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R23.Report.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R23.Report.xltx", 2, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");
            objSheets.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R23.Report");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return false;
        }
    }
}
