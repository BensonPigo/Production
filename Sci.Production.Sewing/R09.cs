using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Sewing
{
    public partial class R09 : Sci.Win.Tems.PrintForm
    {
        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateConfirm.Value2 = DateTime.Today.AddDays(-1);
            this.dateConfirm.Focus1();
        }

        private string Sqlcmd;
        private DataTable printData;

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateConfirm.Value1) || MyUtility.Check.Empty(this.dateConfirm.Value2))
            {
                MyUtility.Msg.WarningBox("Confirm Date must enter!");
                this.dateConfirm.Focus1();
                return false;
            }

            string where = string.Empty;

            if (!MyUtility.Check.Empty(this.dateConfirm.Value1))
            {
                where += "\r\n" + $"and cast(oca.ConfirmedDate as date) >= '{((DateTime)this.dateConfirm.Value1).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(this.dateConfirm.Value2))
            {
                where += "\r\n" + $"and  cast(oca.ConfirmedDate as date) <= '{((DateTime)this.dateConfirm.Value2).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(this.txtMdivision1.Text))
            {
                where += "\r\n" + $"and o.MDivisionID = '{this.txtMdivision1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                where += "\r\n" + $"and o.FactoryID='{this.txtfactory1.Text}'";
            }

            if (this.chkNotComplete.Checked)
            {
                where += "\r\n" + $"and isnull(s.QAQty,0)-isnull(oq.Qty,0) < 0";
            }

            // 列出異動清單，可以讓工廠清楚的知道訂單數量的去向
            // 主要展開欄位OrderChangeApplication.ID,  OrderChangeApplication_Detail.Article,SizeCode --TransferQty以此3欄位加總
            // 串法展開最細Table為OrderChangeApplication_Detail. 欄位TransferQty用windows function加總, 直接distinct
            this.Sqlcmd = $@"
select distinct
	o.MDivisionID,
	o.FactoryID,
	oca.OrderID,
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	ocad.Article,
	ocad.SizeCode,
	oca.ToOrderID,
	TransferQty = Sum(ocad.NowQty) over(partition by oca.ID,ocad.Article,ocad.SizeCode)- sum(ocad.Qty) over(partition by oca.ID,ocad.Article,ocad.SizeCode),
	NewSPQty=isnull(oq.Qty,0),
	NewSPSewingOutputQty=isnull(s.QAQty,0), --完整的成衣數
	Balance=isnull(s.QAQty,0)-isnull(oq.Qty,0),
	oca.ID
from OrderChangeApplication oca with(nolock)
Inner Join OrderChangeApplication_Detail ocad with(nolock) on ocad.ID = oca.ID
Inner Join Orders o with(nolock) on o.ID = oca.OrderID
Left Join Order_Qty oq with(nolock) on oq.ID = oca.ToOrderID and oq.Article = ocad.Article and oq.SizeCode = ocad.SizeCode
outer apply(
	select QAQty= case when o.StyleUnit = 'PCS' then(	
			select QAQty = sum(sdd.QAQty)
			from SewingOutput_Detail_Detail sdd 
			where sdd.OrderId = oca.ToOrderID 
			and sdd.Article = ocad.Article 
			and sdd.SizeCode = ocad.SizeCode
	)
	else(
		select min(QAQty)
		from(
			select ol.Location, QAQty = sum(sdd.QAQty)
			from Order_Location ol with(nolock)
			left join SewingOutput_Detail_Detail sdd with(nolock) on sdd.ComboType = ol.Location
			where sdd.OrderId = oca.ToOrderID 
			and sdd.Article = ocad.Article 
			and sdd.SizeCode = ocad.SizeCode
			and ol.OrderID = oca.ToOrderID
			group by ol.Location
		)byLocation
	)
	end
)s
where 1=1
and oca.Status in ('Confirmed','Closed')
and IIF(oca.ToOrderID is null,'', oca.ToOrderID) <> ''
{where}
";
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.Sqlcmd, out this.printData);
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);

            this.printData.Columns.Remove("ID");

            string excelFile = "Sewing_R09";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + excelFile + ".xltx"); // 開excelapp
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, excelFile + ".xltx", 1, false, null, excelApp, false, null, false);

            #region Save Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(excelFile);
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void DateRange1_DateBox2_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.dateConfirm.Value2) && (DateTime)this.dateConfirm.Value2 >= DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Confirm Date cannot be greater than or equal to today!");
                e.Cancel = true;
                return;
            }
        }
    }
}
