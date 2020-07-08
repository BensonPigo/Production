using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Sewing
{
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        private string Factory;
        private string M;
        private string strIssueDate1;
        private string strIssueDate2;
        private string strApvDate1;
        private string strApvDate2;
        private string SubCon;
        private string ContractNumb;
        private string SP;
        private string Status;
        private DataTable printData;

        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboStatus, 2, 1, ",,New,New,Confirmed,Confirmed");
            this.comboStatus.SelectedIndex = 0;
            this.txtMdivisionM.Text = Sci.Env.User.Keyword;
        }

        protected override bool ValidateInput()
        {
            this.Factory = this.txtfactory.Text;
            this.M = this.txtMdivisionM.Text;
            this.strIssueDate1 = this.dateIssueDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateIssueDate.Value1).ToString("yyyy/MM/dd");
            this.strIssueDate2 = this.dateIssueDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateIssueDate.Value2).ToString("yyyy/MM/dd");
            this.strApvDate1 = this.dateRangeApvDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeApvDate.Value1).ToString("yyyy/MM/dd");
            this.strApvDate2 = this.dateRangeApvDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeApvDate.Value2).ToString("yyyy/MM/dd");
            this.SubCon = this.txtSubCon.Text;
            this.ContractNumb = this.txtContract.Text;
            this.SP = this.txtSPNO.Text;
            this.Status = this.comboStatus.SelectedValue.ToString();

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            List<string> listSQLFilter = new List<string>();
            #region Filter

            if (!MyUtility.Check.Empty(this.Factory))
            {
                listSQLFilter.Add($"and s.Factoryid = '{this.Factory}'");
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                listSQLFilter.Add($"and s.MDivisionID = '{this.M}'");
            }

            if (!MyUtility.Check.Empty(this.SubCon))
            {
                listSQLFilter.Add($"and s.SubConOutFty = '{this.SubCon}'");
            }

            if (!MyUtility.Check.Empty(this.ContractNumb))
            {
                listSQLFilter.Add($"and s.ContractNumber = '{this.ContractNumb}'");
            }

            if (!MyUtility.Check.Empty(this.SP))
            {
                listSQLFilter.Add($"and sd.Orderid = '{this.SP}'");
            }

            if (!MyUtility.Check.Empty(this.Status))
            {
                listSQLFilter.Add($"and s.Status = '{this.Status}'");
            }

            if (!MyUtility.Check.Empty(this.strIssueDate1))
            {
                listSQLFilter.Add($"and s.IssueDate >= '{this.strIssueDate1}'");
            }

            if (!MyUtility.Check.Empty(this.strIssueDate2))
            {
                listSQLFilter.Add($"and s.IssueDate <= '{this.strIssueDate2}'");
            }

            if (!MyUtility.Check.Empty(this.strApvDate1))
            {
                listSQLFilter.Add($"and s.ApvDate >= '{this.strApvDate1}'");
            }

            if (!MyUtility.Check.Empty(this.strApvDate2))
            {
                listSQLFilter.Add($"and s.ApvDate <= '{this.strApvDate2}'");
            }
            #endregion

            string sqlcmd = $@"
select 
[Factory] = s.Factoryid
,[Subcon Name]  = sd.SubConOutFty
,[Contract No] = sd.ContractNumber
,[Style] = o.StyleID
,[SP] = o.ID
,[Qty] = Order_Qty.Qty
,[Sewing_CPU] = ROUND(tms.SewingCPU * r.rate,4,4)
,LocalCurrencyID = LocalCurrencyID
,LocalUnitPrice = isnull(LocalUnitPrice,0)
,Vat = isnull(Vat,0)
,UPIncludeVAT = isnull(LocalUnitPrice,0)+isnull(Vat,0)
,KpiRate = isnull(KpiRate,0)
,[SubConPrice/CPU] = ROUND(sd.UnitPrice,4,4)
,[Cut] = ROUND(tms.CuttingCPU * r.rate,4,4)
,[H.T] = ROUND(tms.HeatTransfer,4,4)
,[Inspection] = ROUND(tms.InspectionCPU * r.rate ,4,4)
,[OtherCpu] = ROUND(tms.OtherCPU * r.rate,4,4)
,[EMB] = ROUND(tms.EMBPrice,4,4)
,[Print] = ROUND(tms.PrintingPrice,4,4)
,[OtherAmt] = ROUND(tms.OtherAmt,4,4)
,[Price/CPU] = ROUND(iif((tms.CuttingCPU * r.rate +tms.HeatTransfer+tms.InspectionCPU * r.rate +tms.OtherCPU * r.rate )=0,0, (sd.UnitPrice-tms.EMBPrice-tms.PrintingPrice-tms.OtherAmt) / (tms.CuttingCPU * r.rate +tms.HeatTransfer+tms.InspectionCPU * r.rate +tms.OtherCPU * r.rate )),4,4)
,[Or] = ''
,[Min Rate Cpu] = ''
,[TTLCPU] = ROUND(Order_Qty.Qty * tms.SewingCPU * r.rate ,4,4)
,[Contract Amt] = ROUND(Order_Qty.Qty * sd.UnitPrice,4,4)
,[ExchangeRate]=''
,[Contract Amt_usd]=''
,[SR]=''
,[Remark]=''
from dbo.SubconOutContract_Detail sd with (nolock)
left join Orders o with (nolock) on sd.Orderid = o.ID
left join SubconOutContract s with (nolock) 
on sd.SubConOutFty=s.SubConOutFty  and sd.ContractNumber=s.ContractNumber
outer apply(
	select isnull(sum(Qty),0) Qty
	from Order_Qty with (nolock) 
	where ID = sd.OrderID and Article = sd.Article 
)Order_Qty
outer apply (
    select  
    [SewingCPU] = sum(iif(ArtworkTypeID = 'SEWING',TMS/1400,0)),
    [CuttingCPU]= sum(iif(ArtworkTypeID = 'CUTTING',TMS/1400,0)),
    [InspectionCPU]= sum(iif(ArtworkTypeID = 'INSPECTION',TMS/1400,0)),
    [OtherCPU]= sum(iif(ArtworkTypeID in ('INSPECTION','CUTTING','SEWING'),0,TMS/1400)),
    [OtherAmt]= sum(iif(ArtworkTypeID in ('PRINTING','EMBROIDERY'),0,Price)) * sd.OutputQty,
    [EMBAmt] = sum(iif(ArtworkTypeID = 'EMBROIDERY',Price,0)) * sd.OutputQty,
    [PrintingAmt] = sum(iif(ArtworkTypeID = 'PRINTING',Price,0)) * sd.OutputQty,
    [OtherPrice]= sum(iif(ArtworkTypeID in ('PRINTING','EMBROIDERY'),0,Price)),
    [EMBPrice] = sum(iif(ArtworkTypeID = 'EMBROIDERY',Price,0)),
    [PrintingPrice] = sum(iif(ArtworkTypeID = 'PRINTING',Price,0)),
	[HeatTransfer] = sum(iif(ArtworkTypeID = 'HEAT TRANSFER',TMS/1400,0))
    from Order_TmsCost with (nolock)
    where ID = sd.OrderID
) as tms
outer apply(
	select rate = isnull(dbo.GetOrderLocation_Rate(o.ID,sd.ComboType)
	,(select rate = rate from Style_Location sl with (nolock) where sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType))/100)r
where 1=1
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd.ToString(),  out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Sewing_R07.xltx");
            objApp.Visible = false;
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Sewing_R07.xltx", 3, false, null, objApp);
            Microsoft.Office.Interop.Excel.Worksheet wks = objApp.ActiveWorkbook.Worksheets[1];
            for (int c = 1; c <= this.printData.Rows.Count; c++)
            {
                int tr = c + 3;
                wks.Cells[c + 3, 21] = $"=(M{tr}-R{tr}-S{tr}-T{tr})/(G{tr}+N{tr}+O{tr}+P{tr}+Q{tr})";
                wks.Cells[c + 3, 22] = $"=IF(P{tr}<R{tr},\"<\",\">\")";
                wks.Cells[c + 3, 27] = $"=T{tr}/U{tr}";
            }

            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
            objApp.Cells.EntireRow.AutoFit();       // 自動欄高

            // 畫框線
            int rowcnt = this.printData.Rows.Count + 3;
            Microsoft.Office.Interop.Excel.Range rg1;
            rg1 = wks.get_Range("A4", $"AC{this.printData.Rows.Count + 3}");
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;

            #region Save Excel
            string excelFile = Sci.Production.Class.MicrosoftFile.GetName("Sewing_R07");
            objApp.ActiveWorkbook.SaveAs(excelFile);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objApp);
            excelFile.OpenFile();
            #endregion

            return true;
        }
    }
}
