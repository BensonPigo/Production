using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Sewing
{
    public partial class R09 : Win.Tems.PrintForm
    {
        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateConfirm.Focus1();
        }

        private string Sqlcmd;
        private DataTable printData;

        protected override bool ValidateInput()
        {
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
                where += "\r\n" + $"and o.FtyGroup='{this.txtfactory1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text) || !MyUtility.Check.Empty(this.txtSP2.Text))
            {
                if (!MyUtility.Check.Empty(this.txtSP1.Text) && !MyUtility.Check.Empty(this.txtSP2.Text))
                {
                    where += string.Format(
                        @" and ((oca.OrderID = '{0}' or oca.ToOrderID = '{0}')
                                or (oca.OrderID = '{1}' or oca.ToOrderID = '{1}'))",
                        this.txtSP1.Text,
                        this.txtSP2.Text);
                }
                else if (!MyUtility.Check.Empty(this.txtSP1.Text))
                {
                    where += string.Format(" and (oca.OrderID = '{0}' or oca.ToOrderID = '{0}')", this.txtSP1.Text);
                }
                else if (!MyUtility.Check.Empty(this.txtSP2.Text))
                {
                    where += string.Format(" and (oca.OrderID = '{0}' or oca.ToOrderID = '{0}')", this.txtSP2.Text);
                }
            }

            if (this.chkOnlyshowBalanceQty.Checked)
            {
                where += "\r\n" + $"and isnull(sF.QAQty,0)-isnull(oqF.Qty,0) > 0";
            }

            // 列出異動清單，可以讓工廠清楚的知道訂單數量的去向
            // 主要展開欄位OrderChangeApplication.ID,  OrderChangeApplication_Detail.Article,SizeCode --TransferQty以此3欄位加總
            // 串法展開最細Table為OrderChangeApplication_Detail. 欄位TransferQty用windows function加總, 直接distinct
            this.Sqlcmd = $@"
select distinct
	o.MDivisionID,
	o.FactoryID,
    oca.ID,
    Reason = (SELECT ID + '-' + Name as Name FROM Reason WHERE ReasonTypeID = 'OMQtychange' and ID = oca.ReasonID),
	oca.OrderID,
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	ocad.Article,
	ocad.SizeCode,
	oca.ToOrderID,
	OriSPQty=isnull(oqF.Qty,0),
	OriSPSewingOutputQty=isnull(sF.QAQty,0), --完整的成衣數
	TransferQty = Sum(ocad.NowQty) over(partition by oca.ID,ocad.Article,ocad.SizeCode)- sum(ocad.Qty) over(partition by oca.ID,ocad.Article,ocad.SizeCode),
	NewSPQty=isnull(oq.Qty,0),
	NewSPSewingOutputQty=isnull(s.QAQty,0) --完整的成衣數
into #tmp
from OrderChangeApplication oca with(nolock)
Inner Join OrderChangeApplication_Detail ocad with(nolock) on ocad.ID = oca.ID
Inner Join Orders o with(nolock) on o.ID = oca.OrderID
Left Join Order_Qty oqF with(nolock) on oqF.ID = oca.OrderID and oqF.Article = ocad.Article and oqF.SizeCode = ocad.SizeCode
Left Join Order_Qty oq with(nolock) on oq.ID = oca.ToOrderID and oq.Article = ocad.Article and oq.SizeCode = ocad.SizeCode
outer apply(select QAQty= dbo.getMinCompleteSewQty(oca.OrderID ,ocad.Article ,ocad.SizeCode))sF
outer apply(select QAQty= dbo.getMinCompleteSewQty(oca.ToOrderID ,ocad.Article ,ocad.SizeCode))s
where 1=1
and oca.Status in ('Confirmed','Closed')
and isnull(oca.GMCheck,0) = 0
and IIF(oca.ToOrderID is null,'', oca.ToOrderID) <> ''
{where}

select *,
	Balance=concat('=MAX(0,(M',
        ROW_NUMBER() over(order by 	MDivisionID,FactoryID,ID,OrderID,StyleID,SeasonID,BrandID)+1
        ,'-L',
        ROW_NUMBER() over(order by 	MDivisionID,FactoryID,ID,OrderID,StyleID,SeasonID,BrandID)+1,'))')	
from #tmp
order by MDivisionID,FactoryID,ID,OrderID,StyleID,SeasonID,BrandID

drop table #tmp
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

            string excelFile = "Sewing_R09";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + excelFile + ".xltx"); // 開excelapp
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, excelFile + ".xltx", 1, false, null, excelApp, false, null, false);

            #region Save Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelFile);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);

            excelApp.Visible = true;
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);

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
