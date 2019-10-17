using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R43
    /// </summary>
    public partial class R43 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;

        /// <summary>
        /// R43
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
SELECT 
	[InvoiceNo] = G.ID
	, [Shipper] = G.Shipper
	, [CustCD] = G.CustCDID
	, [Destination] = G.Dest + '-' + C.Alias
	, [ShipMode] = G.ShipModeID
	, [BLNo] = G.BLNo
	, [VesselName] = G.Vessel
	, [SOCFMDate] = G.SOCFMDate
	, [CutOffDate] = G.CutOffDate
	, [PulloutDate] = STUFF ((select CONCAT (',', format(a.PulloutDate,'yyyy/MM/dd')) 
                            from (
                                select distinct p.PulloutDate
                                from PackingList p WITH (NOLOCK)
                                where G.ID = p.INVNo
                            ) a 
							order by a.PulloutDate
                            for xml path('')
                          ), 1, 1, '') 
	, [FCRDate] = G.FCRDate
	, [OnBoardDate] = G.ETD
FROM GMTBooking G
LEFT JOIN VNExportDeclaration V ON G.ID = V.InvNo
LEFT JOIN Country C ON G.Dest = C.ID
WHERE G.NonDeclare = 0
AND (ISNULL(V.InvNo,'') ='' OR (LEN(V.InvNo) > 0 AND ISNULL(V.DeclareNo,'') = ''))"));

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                return new DualResult(false, "Query data fail\r\n" + result.ToString()); ;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R43_NonDeclarationReportExport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "Shipping_R43_NonDeclarationReportExport.xltx", headerRow: 1, excelApp: excel, showSaveMsg: false, wSheet: worksheet);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excel);
            return true;
        }
    }
}
