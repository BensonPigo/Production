using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R62 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DualResult result;
        private DateTime? dateDecDate1;
        private DateTime? dateDecDate2;
        private DateTime? dateETD1;
        private DateTime? dateETD2;
        private string strDecNo1;
        private string strDecNo2;
        private string strInvNo1;
        private string strInvNo2;

        /// <inheritdoc/>
        public R62(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.dateDecDate1 = this.dateDecDate.Value1;
            this.dateDecDate2 = this.dateDecDate.Value2;
            this.dateETD1 = this.dateETD.Value1;
            this.dateETD2 = this.dateETD.Value2;
            this.strDecNo1 = this.txtDecNo1.Text;
            this.strDecNo2 = this.txtDecNo2.Text;
            this.strInvNo1 = this.txtInvNo1.Text;
            this.strInvNo2 = this.txtInvNo2.Text;

            if (MyUtility.Check.Empty(this.dateDecDate1) && MyUtility.Check.Empty(this.dateDecDate2) &&
                MyUtility.Check.Empty(this.dateETD1) && MyUtility.Check.Empty(this.dateETD1))
            {
                MyUtility.Msg.WarningBox("Please input <Declaration Date> or <ETD> first!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd = $@"
select [Shipper] = kd.Shipper 
     , [Buyer] = kd.Buyer 
     , [FTY] = o.ftygroup 
     , [Status] = kd.status 
     , [Inv#] = kdd.INVNo 
     , [PO] = o.CustPONo
     , [Style] = o.StyleID  
     , [Qty] = kdd.ShipModeSeqQty 
     , [CTN] = kdd.CTNQty 
     , [FOB] = kdd.POPrice 
     , [Local Inv#] = kdd.LocalINVNO 
     , [Description] = kdd.Description 
     , [HS Code] = kdd.HSCode 
     , [CO Form Type] = kdd.COFormType 
     , [CO#] = kdd.COID 
     , [CO Date] = kdd.CODate 
     , [Declaration#] = kd.DeclareNo 
     , [Declaration Date] = kd.CDate 
     , [ETD] = kd.ETD 
     , [Customer CD] = kd.CustCDID 
     , [Destination] = kd.Dest 
     , [Shipmode] = kd.ShipModeID 
     , [Forwarder] = kd.Forwarder 
     , [Port of loading] = kd.ExportPort 
     , [Export without declaration] = (case when g.NonDeclare = 1 then 'Y' else 'N' end) 
  from KHExportDeclaration kd
 inner join KHExportDeclaration_Detail kdd on kd.id=kdd.id
 inner join GMTBooking g on kdd.Invno=g.id
 inner join orders o on o.id=kdd.orderid
 where 1=1
";
            #region where
            if (!MyUtility.Check.Empty(this.dateDecDate1) && !MyUtility.Check.Empty(this.dateDecDate2))
            {
                sqlcmd += $@" and kd.Cdate between '{((DateTime)this.dateDecDate1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateDecDate2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dateETD1) && !MyUtility.Check.Empty(this.dateETD2))
            {
                sqlcmd += $@"   and kd.ETD between '{((DateTime)this.dateETD1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateETD2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strDecNo1))
            {
                sqlcmd += $@" and kd.DeclareNo >= '{this.strDecNo1}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strDecNo2))
            {
                sqlcmd += $@" and kd.DeclareNo <= '{this.strDecNo2}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strInvNo1))
            {
                sqlcmd += $@" and kdd.INVNo >= '{this.strInvNo1}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strInvNo2))
            {
                sqlcmd += $@" and kdd.INVNo <= '{this.strInvNo2}'" + Environment.NewLine;
            }

            sqlcmd += @" order by kd.CDate, kd.DeclareNo, kdd.INVNo  ";

            #endregion

            if (!(this.result = DBProxy.Current.Select(null, sqlcmd, out this.printData)))
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + this.result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Starting EXCEL...");
            string reportName = "Shipping_R62.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R62");
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
