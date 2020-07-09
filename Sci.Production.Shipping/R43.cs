using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R43
    /// </summary>
    public partial class R43 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? dateOnBoardDate1;
        private DateTime? dateOnBoardDate2;
        private DateTime? dateFCRDate1;
        private DateTime? dateFCRDate2;

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
            this.dateOnBoardDate1 = this.dateOnBoardDate.Value1;
            this.dateOnBoardDate2 = this.dateOnBoardDate.Value2;
            this.dateFCRDate1 = this.dateFCRDate.Value1;
            this.dateFCRDate2 = this.dateFCRDate.Value2;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string sqlwhere_GMTBooking = string.Empty;
            string sqlwhere_FtyExport = string.Empty;

            if (!MyUtility.Check.Empty(this.dateOnBoardDate1))
            {
                sqlwhere_GMTBooking += $@" and G.ETD between '{((DateTime)this.dateOnBoardDate1).ToString("yyyyMMdd")}' and '{((DateTime)this.dateOnBoardDate2).ToString("yyyyMMdd")}'";
                sqlwhere_FtyExport += $@" and f.OnBoard between '{((DateTime)this.dateOnBoardDate1).ToString("yyyyMMdd")}' and '{((DateTime)this.dateOnBoardDate2).ToString("yyyyMMdd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateFCRDate1))
            {
                sqlwhere_GMTBooking += $@" and G.FCRDate between '{((DateTime)this.dateFCRDate1).ToString("yyyyMMdd")}' and '{((DateTime)this.dateFCRDate2).ToString("yyyyMMdd")}'";
                sqlwhere_FtyExport += $@" and f.PortArrival between '{((DateTime)this.dateFCRDate1).ToString("yyyyMMdd")}' and '{((DateTime)this.dateFCRDate2).ToString("yyyyMMdd")}'";
            }

            sqlCmd.Append(string.Format(
                @"SELECT 
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
                AND (ISNULL(V.InvNo,'') ='' OR (LEN(V.InvNo) > 0 AND ISNULL(V.DeclareNo,'') = ''))
                {0}
                union all
                select
	                [InvoiceNo] = f.ID
	                , [Shipper] = f.Shipper
	                , [CustCD] = null
	                , [Destination] = f.ImportCountry + '-' + c.Alias
	                , [ShipMode] = f.ShipModeID
	                , [BLNo] = f.Blno
	                , [VesselName] = f.Vessel
	                , [SOCFMDate] = null
	                , [CutOffDate] = null
	                , [PulloutDate] = format(f.PortArrival, 'yyyy/MM/dd')
	                , [FCRDate] = format(f.PortArrival, 'yyyy/MM/dd')
	                , [OnBoardDate] = f.OnBoard
                from FtyExport f
                left join VNContractQtyAdjust v on f.ID = v.WKNo 
                left join Country C ON f.ImportCountry = C.ID
                where f.Type = 3
                and isnull(v.DeclareNo,'') = ''
                and f.NonDeclare = 0
                {1} ",
                sqlwhere_GMTBooking,
                sqlwhere_FtyExport));

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                return new DualResult(false, "Query data fail\r\n" + result.ToString());
            }

            return Ict.Result.True;
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

            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_R43_NonDeclarationReportExport.xltx";
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
