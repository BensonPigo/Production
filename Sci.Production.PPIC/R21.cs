using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{

    /// <summary>
    /// R21
    /// </summary>
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        private DataTable dtPrintData;
        private string sqlWhere = string.Empty;
        private List<SqlParameter> listPar = new List<SqlParameter>();
        private readonly List<string> listProcess = new List<string>()
        {
                string.Empty,
                "Dry Room Receive",
                "Dry Room Transfer",
                "Transfer To Packing Error",
                "Confirm Packing Error Revise",
                "Scan & Pack",
                "MD Scan",
                "Fty Transfer To Clog",
                "Clog Receive",
                "Clog Return",
                "Clog Transfer To CFA",
                "Clog Receive From CFA",
                "CFA Receive",
                "CFA Return",
        };

        /// <summary>
        /// R21
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.comboProcess, 1, 1, this.listProcess.JoinToString(","));
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateRangeBuyerDelivery.HasValue &&
                MyUtility.Check.Empty(this.comboProcess.Text))
            {
                MyUtility.Msg.WarningBox("Please input < Buyer Delivery > or < Process >.");
                return false;
            }

            this.listPar.Clear();
            this.sqlWhere = string.Empty;

            if (this.dateRangeBuyerDelivery.HasValue1)
            {
                this.sqlWhere += " and o.BuyerDelivery >= @BuyerDeliveryFrom ";
                this.listPar.Add(new SqlParameter("@BuyerDeliveryFrom", this.dateRangeBuyerDelivery.DateBox1.Value));
            }

            if (this.dateRangeBuyerDelivery.HasValue2)
            {
                this.sqlWhere += " and o.BuyerDelivery <= @BuyerDeliveryTo ";
                this.listPar.Add(new SqlParameter("@BuyerDeliveryTo", this.dateRangeBuyerDelivery.DateBox2.Value));
            }

            this.listPar.Add(new SqlParameter("@dateTimeProcessFrom", this.dateTimeProcessFrom.Value));
            this.listPar.Add(new SqlParameter("@dateTimeProcessTo", this.dateTimeProcessTo.Value));

            switch (this.comboProcess.Text)
            {
                case "Dry Room Receive":
                    this.sqlWhere += @" and exists(select 1 from DRYReceive a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Dry Room Transfer":
                    this.sqlWhere += @" and exists(select 1 from DRYTransfer a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Transfer To Packing Error":
                    this.sqlWhere += @" and exists(select 1 from PackErrTransfer a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Confirm Packing Error Revise":
                    this.sqlWhere += @" and exists(select 1 from PackErrCFM a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Scan & Pack":
                    this.sqlWhere += @" and pld.ScanEditDate between @dateTimeProcessFrom and @dateTimeProcessTo";
                    break;
                case "MD Scan":
                    this.sqlWhere += @" and exists(select 1 from MDScan a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Fty Transfer To Clog":
                    this.sqlWhere += @" and exists(select 1 from TransferToClog a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Receive":
                    this.sqlWhere += @" and exists(select 1 from ClogReceive a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Return":
                    this.sqlWhere += @" and exists(select 1 from ClogReturn a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Transfer To CFA":
                    this.sqlWhere += @" and exists(select 1 from TransferToCFA a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Receive From CFA":
                    this.sqlWhere += @" and exists(select 1 from ClogReceiveCFA a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "CFA Receive":
                    this.sqlWhere += @" and exists(select 1 from CFAReceive a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "CFA Return":
                    this.sqlWhere += @" and exists(select 1 from CFAReturn a with (nolock) 
												where	a.AddDate between @dateTimeProcessFrom and @dateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                default:
                    break;
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                this.sqlWhere += $" and o.MDivisionID = '{this.txtMdivision.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                this.sqlWhere += $" and o.FTYGroup = '{this.txtfactory.Text}'";
            }

            if (this.chkExcludeSisterTransferOut.Checked)
            {
                this.sqlWhere += $" and f.IsProduceFty = 1";
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {

            string sqlGetDate = $@"
select  f.KPICode,
		o.FactoryID,
		[OrderID] = o.ID,
		oqs.Seq,
		o.BrandID, 
		o.StyleID,
		o.SeasonID,
		oqs.BuyerDelivery,
		pld.ID,
		pld.CTNStartNo,
		[ShipQty] = sum(pld2.ShipQty),
		[Status] = case when pld.TransferDate is null then 'Fty'
						when pld.TransferDate is not null and pld.ReceiveDate is null then 'Transit to CLOG'
						when pld.ReceiveDate is not null and 
							 ((pld.TransferCFADate is null and pld.CFAReceiveDate is null and pld.CFAReturnClogDate is null) or 
							 (pld.TransferCFADate is null and pld.CFAReceiveDate is null and pld.ClogReceiveCFADate is not null) or 
							 (pld.TransferCFADate is not null and pld.CFAReceiveDate is null)) then'Clog'
						else 'CFA' end,
		pld.ScanEditDate,
		pld.ScanQty,
		pld.ClogLocationId,
		pld.DisposeDate,
		[PulloutComplete] = iif(o.PulloutComplete = 1, 'Y', 'N'),
		p.PulloutDate,
		pld.SCICtnNo
into #tmp
from  Orders o with (nolock)
inner join Order_QtyShip oqs with (nolock) on oqs.Id = o.ID
inner join Factory f with (nolock) on f.ID = o.FactoryID
inner join PackingList_Detail pld with (nolock) on pld.OrderID = oqs.ID and pld.CTNQty = 1
inner join PackingList p with (nolock) on p.ID = pld.ID
inner join PackingList_Detail pld2 with (nolock) on pld2.ID = pld.ID and pld2.CTNStartNo = pld.CTNStartNo
where o.Category in ('B','G') {this.sqlWhere}
group by	f.KPICode,
			o.FactoryID,
			o.ID,
			oqs.Seq,
			o.BrandID, 
			o.StyleID,
			o.SeasonID,
			oqs.BuyerDelivery,
			pld.ID,
			pld.CTNStartNo,
			pld.TransferDate,
			pld.ReceiveDate,
			pld.TransferCFADate,
			pld.CFAReceiveDate,
			pld.CFAReturnClogDate,
			pld.ClogReceiveCFADate,
			pld.ScanEditDate,
			pld.ScanQty,
			pld.ClogLocationId,
			pld.DisposeDate,
			o.PulloutComplete,
			p.PulloutDate,
			pld.SCICtnNo


select	pld.KPICode,
		pld.FactoryID,
		pld.OrderID,
		pld.Seq,
		pld.BrandID, 
		pld.StyleID,
		pld.SeasonID,
		pld.BuyerDelivery,
		[PackID] = pld.ID,
		pld.CTNStartNo,
		pld.ShipQty,
		pld.Status,
		[HaulingScanTime] = Hauling.AddDate,
		[QtyPerCTN] = isnull(QtyPerCTN.value,0),--ISP20221189
		[DryRoomReceiveTime] = DRYReceive.AddDate,
		[DryRoomTransferTime] = DRYTransfer.AddDate,
		[MDScanTime] = MDScan.AddDate,
		[MDFailQty] = MDScanQty.MDFailQty,
		[Packing Audit Scan Time]  = AuditScanTime.ScanTime,--ISP20221189
		[Packing Audit Fail Qty] = isnull(AuditScanTime.AuditFailQty,0),--ISP20221189
		[M360 MD Scan Time] = MDScanTime.ScanTime,--ISP20221189
		[M360 MD Fail Qty] = isnull(MDScanTime.MDFailQty,0),--ISP20221189
		[TransferToPackingErrorTime] = PackErrTransfer.AddDate,
		[ConfirmPackingErrorReviseTime] = PackErrCFM.AddDate,
		pld.ScanEditDate,
		pld.ScanQty,
		[FtyTransferToClogTime] = TransferToClog.AddDate,
		[ClogReceiveTime] = ClogReceive.AddDate,
		pld.ClogLocationId,
		[ClogReturnTime] = ClogReturn.AddDate,
		[ClogTransferToCFATime] = TransferToCFA.AddDate,
		[CFA Receive Time] = CFAReceive.AddDate,
		[CFA Return Time] = CFAReturn.AddDate,
		[CFA Return Destination] = CFAReturn.ReturnTo,
		[Clog Receive From CFA Time] = ClogReceiveCFA.AddDate,
		pld.DisposeDate,
		pld.PulloutComplete,
		pld.PulloutDate
from #tmp pld
outer apply(
	select value = sum(QtyPerCTN)
	from PackingList_Detail pd with(nolock)
	where pd.SCICtnNo = pld.SCICtnNo
	and pd.ID = pld.ID
)QtyPerCTN
outer apply(
	select ScanTime = t.AddDate, AuditFailQty = t.Qty
	from CTNPackingAudit t with(nolock)
	where t.SCICtnNo = pld.SCICtnNo
	and t.PackingListID = pld.ID
	and t.AddDate = (
		select max(AddDate)
		from CTNPackingAudit 
		where PackingListID = pld.ID and SCICtnNo=pld.SCICtnNo
	)
)AuditScanTime
outer apply(
	select ScanTime = md.AddDate, md.MDFailQty
	from MDScan md
	where DataRemark = 'Create from M360' 
	and md.PackingListID = pld.ID and md.SCICtnNo= pld.SCICtnNo
	and AddDate = (
		select max(AddDate)
		from MDScan 
		where DataRemark = 'Create from M360' 
		and PackingListID=pld.ID and SCICtnNo=pld.SCICtnNo
	)
)MDScanTime
outer apply(select [AddDate] = max(AddDate) 
			from CTNHauling ch with (nolock) 
			where	ch.PackingListID = pld.ID and 
					ch.CTNStartNo = pld.CTNStartNo and 
					ch.OrderID = pld.OrderID ) Hauling
outer apply(select [AddDate] = max(AddDate) 
			from DRYReceive dr with (nolock) 
			where	dr.PackingListID = pld.ID and 
					dr.CTNStartNo = pld.CTNStartNo and 
					dr.OrderID = pld.OrderID ) DRYReceive
outer apply(select [AddDate] = max(AddDate)
			from DRYTransfer drt with (nolock) 
			where	drt.PackingListID = pld.ID and 
					drt.CTNStartNo = pld.CTNStartNo and 
					drt.OrderID = pld.OrderID ) DRYTransfer
outer apply(select	[AddDate] = max(AddDate)
			from MDScan md with (nolock) 
			where	md.DataRemark = 'Create from PMS' and
					md.PackingListID = pld.ID and 
					md.CTNStartNo = pld.CTNStartNo and 
					md.OrderID = pld.OrderID ) MDScan
outer apply(select	[MDFailQty] = MAX(MDFailQty)
			from MDScan md with (nolock) 
			where	md.PackingListID = pld.ID and 
					md.CTNStartNo = pld.CTNStartNo and 
					md.OrderID = pld.OrderID and
					md.AddDate = MDScan.AddDate) MDScanQty
outer apply(select [AddDate] = max(AddDate)
			from PackErrTransfer pet with (nolock) 
			where	pet.PackingListID = pld.ID and 
					pet.CTNStartNo = pld.CTNStartNo and 
					pet.OrderID = pld.OrderID ) PackErrTransfer
outer apply(select [AddDate] = max(AddDate)
			from PackErrCFM pae with (nolock) 
			where	pae.PackingListID = pld.ID and 
					pae.CTNStartNo = pld.CTNStartNo and 
					pae.OrderID = pld.OrderID ) PackErrCFM
outer apply(select [AddDate] = max(AddDate)
			from TransferToClog tc with (nolock) 
			where	tc.PackingListID = pld.ID and 
					tc.CTNStartNo = pld.CTNStartNo and 
					tc.OrderID = pld.OrderID ) TransferToClog
outer apply(select [AddDate] = max(AddDate)
			from ClogReceive cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and 
					cr.OrderID = pld.OrderID ) ClogReceive
outer apply(select [AddDate] = max(AddDate)
			from ClogReturn cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and 
					cr.OrderID = pld.OrderID ) ClogReturn
outer apply(select [AddDate] = max(AddDate)
			from TransferToCFA tc with (nolock) 
			where	tc.PackingListID = pld.ID and 
					tc.CTNStartNo = pld.CTNStartNo and 
					tc.OrderID = pld.OrderID ) TransferToCFA
outer apply(select [AddDate] = max(AddDate)
			from CFAReceive cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and 
					cr.OrderID = pld.OrderID ) CFAReceive
outer apply(select [AddDate] = max(AddDate), [ReturnTo] = max(ReturnTo)
			from CFAReturn cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and 
					cr.OrderID = pld.OrderID ) CFAReturn
outer apply(select [AddDate] = max(AddDate)
			from ClogReceiveCFA tc with (nolock) 
			where	tc.PackingListID = pld.ID and 
					tc.CTNStartNo = pld.CTNStartNo and 
					tc.OrderID = pld.OrderID ) ClogReceiveCFA
";
            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlGetDate, this.listPar, out this.dtPrintData);
            DBProxy.Current.DefaultTimeout = 300;
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrintData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtPrintData.Rows.Count); // 顯示筆數

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R21_Carton_Status_Tracking_List.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dtPrintData, string.Empty, "PPIC_R21_Carton_Status_Tracking_List.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            objApp.Visible = true;

            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
