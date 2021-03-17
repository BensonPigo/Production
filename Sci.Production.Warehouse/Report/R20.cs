using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R20 : Win.Tems.PrintForm
    {
        private string dateFormate = "yyyy-MM-dd";
        private string StartBuyerDelivery;
        private string EndBuyerDelivery;
        private string StartDeadLine;
        private string EndDeadLine;
        private string StartETA;
        private string EndETA;
        private string StartSPNo;
        private string EndSPNo;
        private string MDivision;
        private string Factory;
        private string StartRefno;
        private string EndRefno;

        private bool boolCheckQty;
        private DataTable printData;

        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            #region Set Value
            this.StartBuyerDelivery = this.dateBuyerDelivery.Value1.ToString().Empty() ? string.Empty : ((DateTime)this.dateBuyerDelivery.Value1).ToString(this.dateFormate);
            this.EndBuyerDelivery = this.dateBuyerDelivery.Value2.ToString().Empty() ? string.Empty : ((DateTime)this.dateBuyerDelivery.Value2).ToString(this.dateFormate);
            this.StartDeadLine = this.dateDeadLine.Value1.ToString().Empty() ? string.Empty : ((DateTime)this.dateDeadLine.Value1).ToString(this.dateFormate);
            this.EndDeadLine = this.dateDeadLine.Value2.ToString().Empty() ? string.Empty : ((DateTime)this.dateDeadLine.Value2).ToString(this.dateFormate);
            this.StartETA = this.dateInventoryETA.Value1.ToString().Empty() ? string.Empty : ((DateTime)this.dateInventoryETA.Value1).ToString(this.dateFormate);
            this.EndETA = this.dateInventoryETA.Value2.ToString().Empty() ? string.Empty : ((DateTime)this.dateInventoryETA.Value2).ToString(this.dateFormate);
            this.StartSPNo = this.textStartSP.Text;
            this.EndSPNo = this.textEndSP.Text;
            this.MDivision = this.txtMdivision1.Text;
            this.Factory = this.txtfactory1.Text;
            this.StartRefno = this.textStartRefno.Text;
            this.EndRefno = this.textEndRefno.Text;
            this.boolCheckQty = this.checkQty.Checked;
            #endregion
            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQl Parameters --- _ADD10 -> SPNo 補足 10 碼
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@StartBuyerDelivery", this.StartBuyerDelivery));
            listSqlPar.Add(new SqlParameter("@EndBuyerDelivery", this.EndBuyerDelivery));
            listSqlPar.Add(new SqlParameter("@StartDeadLine", this.StartDeadLine));
            listSqlPar.Add(new SqlParameter("@EndDeadLine", this.EndDeadLine));
            listSqlPar.Add(new SqlParameter("@StartETA", this.StartETA));
            listSqlPar.Add(new SqlParameter("@EndETA", this.EndETA));
            listSqlPar.Add(new SqlParameter("@StartSPNo", this.StartSPNo + "%"));
            listSqlPar.Add(new SqlParameter("@StartSPNo_Add10", this.StartSPNo.PadRight(10, '0')));
            listSqlPar.Add(new SqlParameter("@EndSPNo", this.EndSPNo + "%"));
            listSqlPar.Add(new SqlParameter("@EndSPNo_Add10", this.EndSPNo.PadRight(10, 'Z')));
            listSqlPar.Add(new SqlParameter("@MDivision", this.MDivision));
            listSqlPar.Add(new SqlParameter("@Factory", this.Factory));
            listSqlPar.Add(new SqlParameter("@StartRefno", this.StartRefno));
            listSqlPar.Add(new SqlParameter("@EndRefno", this.EndRefno));
            #endregion
            #region SQL Filte
            List<string> filte = new List<string>();
            if (!this.StartBuyerDelivery.Empty() && !this.EndBuyerDelivery.Empty())
            {
                filte.Add("o.BuyerDelivery between @StartBuyerDelivery and @EndBuyerDelivery");
            }

            if (!this.StartDeadLine.Empty() && !this.EndDeadLine.Empty())
            {
                filte.Add("inv.Deadline between @StartDeadLine and @EndDeadLine");
            }

            if (!this.StartETA.Empty() && !this.EndETA.Empty())
            {
                filte.Add("inv.ETA between @StartETA and @EndETA");
            }

            if (!this.StartSPNo.Empty() && !this.EndSPNo.Empty())
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                filte.Add("o.ID between @StartSPNo_Add10 and @EndSPNo_Add10");
            }
            else if (!this.StartSPNo.Empty())
            {
                // 只有 sp1 輸入資料
                filte.Add("o.ID like @StartSPNo");
            }
            else if (!this.EndSPNo.Empty())
            {
                // 只有 sp2 輸入資料
                filte.Add("o.ID like @EndSPNo");
            }

            if (!this.MDivision.Empty())
            {
                filte.Add("o.MDivisionID = @MDivision");
            }

            if (!this.Factory.Empty())
            {
                filte.Add("o.FactoryID = @Factory");
            }

            if (!this.StartRefno.Empty() && !this.EndRefno.Empty())
            {
                filte.Add("inv.Refno between @StartRefno and @EndRefno");
            }

            if (this.boolCheckQty)
            {
                filte.Add("inv.Qty > 0");
            }
            #endregion
            #region SQL CMD
            string sqlCmd = string.Format(
                @"
select	SPNo = inv.POID
		, Seq1 = inv.Seq1
		, Seq2 = inv.Seq2
		, ProjectID = inv.ProjectID
		, Category = case 
						when o.Category = 'm' then 'MATERIAL'
						when o.Category = 'b' then 'BULK'
						when o.Category = 's' then 'SAMPLE'
					 end
		, Buyer = o.BrandID
		, CustomerPayable = inv.Payable
		, RefNo = inv.Refno
		, SuppID = inv.SuppID
		, Width = invRef.Width
		, [ColorID]= IIF(Fabric.MtlTypeID LIKE '%Thread%' 
			            , IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, psd.ColorID), psd.SuppColor)
			            , psd.ColorID)
		, Size = invRef.SizeSpec
		, SizeUnit = invRef.SizeUnit
		, Unit = psd.StockUnit
		, Quantity = inv.Qty
		, WHActualStockQty = isnull(mpd.LInvQty, 0)
		, StockFactory = inv.FactoryID
		, Special = invref.BomZipperInsert
		, Deadline = inv.Deadline
		, OrderComfirmDate = o.CFMDate
		, AgingDays = DATEDIFF(dd, o.CFMDate, inv.Deadline)
		, ETA = inv.ETA
		, HandleMR = inv.OrderHandle
from Orders o
inner join Inventory inv on o.ID = inv.POID
inner join Po_Supp_Detail psd on inv.POID = psd.id 
                                 and inv.seq1 = psd.seq1 
                                 and inv.seq2 = psd.seq2
left join InventoryRefno invRef on inv.InventoryRefnoID = invRef.ID
left join MDivisionPoDetail mpd on inv.POID = mpd.POID and inv.Seq2 = mpd.Seq1 and inv.Seq2 = mpd.Seq2
left join fabric WITH (NOLOCK) on fabric.SCIRefno = psd.scirefno
{0}", (filte.Count > 0) ? "Where " + filte.JoinToString("\n\r and ") : string.Empty);
            #endregion
            #region Get Data
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, listSqlPar, out this.printData))
            {
                return result;
            }
            #endregion
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R20.xltx");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R20.xltx", 2, showExcel: false, excelApp: objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R20");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
