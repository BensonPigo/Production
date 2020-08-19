using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R14
    /// </summary>
    public partial class R14 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private StringBuilder condition = new StringBuilder();
        private StringBuilder datelist = new StringBuilder();
        private List<SqlParameter> listPar = new List<SqlParameter>();
        private string strWhere = string.Empty;

        /// <summary>
        /// R14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            bool isNotInputMustField = !this.dateRangeETA.HasValue &&
                                       !this.dateRangeETD.HasValue &&
                                       !this.dateRangeFCRDate.HasValue &&
                                       !this.dateRangePulloutDate.HasValue &&
                                       MyUtility.Check.Empty(this.txtInvNo_From.Text) &&
                                       MyUtility.Check.Empty(this.txtInvNo_To.Text);
            if (isNotInputMustField)
            {
                MyUtility.Msg.WarningBox("Please enter at least one blue search term");
                return false;
            }

            #region -- sql parameters declare --
            this.strWhere = string.Empty;
            this.listPar.Clear();
            if (!MyUtility.Check.Empty(this.txtInvNo_From.Text))
            {
                this.strWhere += " and gmt.ID >= @InvBo_From";
                this.listPar.Add(new SqlParameter("@InvBo_From", this.txtInvNo_From.Text));
            }

            if (!MyUtility.Check.Empty(this.txtInvNo_To.Text))
            {
                this.strWhere += " and gmt.ID <= @InvBo_To";
                this.listPar.Add(new SqlParameter("@InvBo_To", this.txtInvNo_To.Text));
            }

            if (this.dateRangePulloutDate.HasValue1)
            {
                this.strWhere += " and pd.PulloutDate >= @PulloutDate_From";
                this.listPar.Add(new SqlParameter("@PulloutDate_From", this.dateRangePulloutDate.DateBox1.Value));
            }

            if (this.dateRangePulloutDate.HasValue2)
            {
                this.strWhere += " and pd.PulloutDate <= @PulloutDate_To";
                this.listPar.Add(new SqlParameter("@PulloutDate_To", this.dateRangePulloutDate.DateBox2.Value));
            }

            if (this.dateRangeFCRDate.HasValue1)
            {
                this.strWhere += " and gmt.FCRDate >= @FCRDate_From";
                this.listPar.Add(new SqlParameter("@FCRDate_From", this.dateRangeFCRDate.DateBox1.Value));
            }

            if (this.dateRangeFCRDate.HasValue2)
            {
                this.strWhere += " and gmt.FCRDate <= @FCRDate_To";
                this.listPar.Add(new SqlParameter("@FCRDate_To", this.dateRangeFCRDate.DateBox2.Value));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                this.strWhere += " and gmt.BrandID = @BrandID";
                this.listPar.Add(new SqlParameter("@BrandID", this.txtbrand.Text));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                this.strWhere += " and o.FtyGroup = @Factory";
                this.listPar.Add(new SqlParameter("@Factory", this.txtfactory.Text));
            }

            if (!MyUtility.Check.Empty(this.txtExportOrigin.TextBox1.Text))
            {
                this.strWhere += " and f.CountryID = @ExportOrigin";
                this.listPar.Add(new SqlParameter("@ExportOrigin", this.txtExportOrigin.TextBox1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtDest.TextBox1.Text))
            {
                this.strWhere += " and gmt.Dest = @Dest";
                this.listPar.Add(new SqlParameter("@Dest", this.txtDest.TextBox1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtcustcd.Text))
            {
                this.strWhere += " and o.CustCDID = @CustCDID";
                this.listPar.Add(new SqlParameter("@CustCDID", this.txtcustcd.Text));
            }

            if (!MyUtility.Check.Empty(this.comboInvStatus.Text))
            {
                this.strWhere += $" and gmt.Status in ({this.comboInvStatus.SelectedValue})";
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                this.strWhere += $" and o.Category in ({this.comboCategory.SelectedValue})";
            }

            if (this.dateRangeETD.HasValue1)
            {
                this.strWhere += " and gmt.ETD >= @ETD_From";
                this.listPar.Add(new SqlParameter("@ETD_From", this.dateRangeETD.DateBox1.Value));
            }

            if (this.dateRangeETD.HasValue2)
            {
                this.strWhere += " and gmt.ETD <= @ETD_To";
                this.listPar.Add(new SqlParameter("@ETD_To", this.dateRangeETD.DateBox2.Value));
            }

            if (this.dateRangeETA.HasValue1)
            {
                this.strWhere += " and gmt.ETA >= @ETA_From";
                this.listPar.Add(new SqlParameter("@ETA_From", this.dateRangeETA.DateBox1.Value));
            }

            if (this.dateRangeETA.HasValue2)
            {
                this.strWhere += " and gmt.ETA <= @ETA_To";
                this.listPar.Add(new SqlParameter("@ETA_To", this.dateRangeETA.DateBox2.Value));
            }
            #endregion
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;

            sqlCmd = $@"
select
gmt.BrandID,
o.ID,
pd.OrderShipmodeSeq,
gmt.ShipModeID,
o.CustPONo,
o.StyleID,
o.SeasonID,
[ShipQty] = sum(pd.ShipQty) over (partition by pd.OrderId,pd.OrderShipmodeSeq),
o.StyleUnit,
[InvNo] = gmt.ID,
[PODD] = pd.PulloutDate,
[IDD] = gmt.IntendDeliveryDate,
gmt.SOCFMDate,
gmt.FCRDate,
gmt.ActFCRDate,
gmt.InvoiceApproveDate,
f.CountryID,
gmt.Shipper,
o.FtyGroup,
o.CustCDID,
[Destination] = gmt.Dest +'-'+ dest.NameEN,
gmt.ETD,
gmt.ETA,
gmt.Vessel,
gmt.BLNo,
gmt.BL2No,
[DocumentRefNo] = '',
o.BrandAreaCode,
o.BrandFTYCode,
[InvoicceStatus] = dd.Name
into #ExcelDetail
from GMTBooking gmt with (nolock)
inner join Pullout_Detail pd with (nolock) on gmt.ID = pd.INVNo and gmt.ShipmodeID = pd.ShipmodeID
inner join Orders o with (nolock) on pd.OrderID = o.ID
left join Factory f with (nolock) on gmt.Shipper = f.ID
left join Country dest with (nolock)  on dest.ID = gmt.Dest
left join (select Name,[ID] = replace(ID,'''','') from DropDownList where type = 'Pms_InvoiceStatus')  dd  on dd.ID = gmt.Status
where 1=1 {this.strWhere}

select * from #ExcelDetail


select  
 [Chart1_Pass_Cnt] = sum(iif(Chart1.DiffDay >= 7,1,0)),
 [Chart1_Fail_Cnt] = sum(iif(Chart1.DiffDay < 7,1,0)),
 [Chart2_Pass_Cnt] = sum(iif(Chart2.DiffDay <= 4,1,0)),
 [Chart2_Fail_Cnt] = sum(iif(Chart2.DiffDay > 4,1,0)),
 [Chart3_Pass_Cnt] = sum(IIF(Chart3.Result = 'True',1,0)),
 [Chart3_Fail_Cnt] = sum(iif(Chart3.Result = 'Fail',1,0))
from #ExcelDetail ed
outer apply (select [value] = count(1) from Holiday h where	ed.PODD > ed.SOCFMDate and h.FactoryID = ed.FtyGroup and 
													h.HolidayDate >= ed.SOCFMDate and h.HolidayDate <= ed.PODD and DATEPART (WEEKDAY,h.HolidayDate) <> '1') HolidayCnt
outer apply(select [DiffDay] = DATEDIFF(day, ed.SOCFMDate, ed.PODD) - HolidayCnt.value - dbo.GetSundayCount(ed.SOCFMDate,ed.PODD)) Chart1
outer apply(select [DiffDay] = DATEDIFF(day, ed.FCRDate, ed.InvoiceApproveDate)) Chart2
outer apply(select [Result] = CASE	WHEN ed.PODD is null or ed.ActFCRDate is null or ed.FCRDate is null THEN 'NoCount'
									WHEN ed.PODD = ed.ActFCRDate and ed.ActFCRDate = ed.FCRDate THEN 'Pass'
									ELSE 'Fail' END) Chart3

";

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), this.listPar, out this.printData);
            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            DataTable dtDetail = this.printData[0];
            DataRow drChart = this.printData[1].Rows[0];

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(dtDetail.Rows.Count);

            if (dtDetail.Rows.Count <= 0)
            {
                MyUtility.Msg.InfoBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R14_AdidasSpecificReport.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(dtDetail, string.Empty, "Planning_R14_AdidasSpecificReport.xltx", 1, false, null, objApp);      // 將datatable copy to excel
            Excel._Worksheet objSheet = objApp.ActiveWorkbook.Sheets.get_Item(3);
            objSheet.Cells[3, 1] = drChart["Chart1_Pass_Cnt"].ToString();
            objSheet.Cells[3, 2] = drChart["Chart1_Fail_Cnt"].ToString();
            objSheet.Cells[7, 1] = drChart["Chart2_Pass_Cnt"].ToString();
            objSheet.Cells[7, 2] = drChart["Chart2_Fail_Cnt"].ToString();
            objSheet.Cells[11, 1] = drChart["Chart3_Pass_Cnt"].ToString();
            objSheet.Cells[11, 2] = drChart["Chart3_Fail_Cnt"].ToString();
            objSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Planning_R14");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(objSheet);
            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}