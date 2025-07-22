using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P08_Print : Win.Tems.PrintForm
    {
        private string _ID;
        private string _Remark;
        private DualResult result;
        private DataTable dtdata = new DataTable();

        /// <inheritdoc/>
        public P08_Print(string id, string remark)
        {
            this.InitializeComponent();
            this._ID = id;
            this._Remark = remark;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd = $@"
select
	r.PoId,
	seq = concat(Ltrim(Rtrim(r.seq1)), ' ', r.Seq2),
	r.Roll,
	r.Dyelot,
	Description = dbo.getmtldesc(r.poid,r.seq1,r.seq2,2,0),	
	po3.StockUnit,
	useqty = (
		select Round(sum(dbo.GetUnitQty(b.POUnit, StockUnit, b.Qty)), 2)
		from po_supp_detail b WITH (NOLOCK) 
		where b.id= r.poid and b.seq1 = r.seq1 and b.seq2 = r.seq2),
	r.StockQty,
    o.FactoryID
from Receiving_Detail r WITH (NOLOCK) 
left join Orders o with (nolock) on r.POID = o.ID
left join PO_Supp_Detail po3 WITH (NOLOCK) on r.PoId = po3.ID  and r.Seq1 = po3.SEQ1 and r.Seq2 = po3.SEQ2
Where r.id = '{this._ID}'
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtdata);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Get data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        private Excel.Worksheet ProcessExcel()
        {
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P08.xltx");
            Excel.Worksheet worksheet = excelApp.Sheets[1];
            for (int i = 0; i < this.dtdata.Rows.Count - 1; i++)
            {
                Excel.Range r = worksheet.get_Range("A6", "A6").EntireRow;
                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
            }

            string factoryID = this.dtdata.AsEnumerable().Select(s => s["FactoryID"].ToString()).Distinct().JoinToString(",");

            this.dtdata.Columns.Remove("FactoryID");

            worksheet.Cells[3, 1] = $"ID : {this._ID}";
            worksheet.Cells[3, 4] = $"Remarks : {this._Remark}";
            worksheet.Cells[2, 1] = factoryID;

            MyUtility.Excel.CopyToXls(this.dtdata, string.Empty, "Warehouse_P08.xltx", 4, showExcel: false, showSaveMsg: false, excelApp: excelApp);
            worksheet.get_Range((Excel.Range)worksheet.Cells[5, 1], (Excel.Range)worksheet.Cells[this.dtdata.Rows.Count + 4, 9]).Borders.Weight = 2; // 設定全框線
            worksheet.Columns[1].ColumnWidth = 18;
            worksheet.Columns[2].ColumnWidth = 8;
            worksheet.Columns[3].ColumnWidth = 8;
            worksheet.Columns[4].ColumnWidth = 8;
            worksheet.Columns[5].ColumnWidth = 46;
            worksheet.Columns[6].ColumnWidth = 9;
            worksheet.Columns[7].ColumnWidth = 12;
            worksheet.Columns[8].ColumnWidth = 12;
            worksheet.Columns[9].ColumnWidth = 45;

            excelApp.Visible = true;
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excelApp);
            return worksheet;
        }

        /// <inheritdoc/>
        protected override bool OnToPrint(Win.ReportDefinition report)
        {
            if (this.dtdata.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            try
            {
                if (this.radioTapingInformationSheet.Checked == true)
                {
                    Excel.Worksheet worksheet = this.ProcessExcel();
                    if (worksheet == null)
                    {
                        MyUtility.Msg.WarningBox("Failed to Process Excel.");
                        return false;
                    }
                    else
                    {
                        worksheet.PrintPreview(); // 預覽列印
                    }
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.Message);
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dtdata.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            try
            {
                if (this.radioTapingInformationSheet.Checked == true)
                {
                    Excel.Worksheet worksheet = this.ProcessExcel();
                    if (worksheet == null)
                    {
                        MyUtility.Msg.WarningBox("Failed to Process Excel.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.Message);
                return false;
            }

            return true;
        }
    }
}
