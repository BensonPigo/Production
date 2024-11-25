using Ict;
using Ict.Win;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P63
    /// </summary>
    public partial class P63 : Sci.Win.Tems.Input6
    {
        private readonly string minETD = "20220101";
        private bool IsChangeID = false;

        /// <summary>
        /// P63
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P63(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Location = new Point(1062, 142);
            this.gridPOListbs.DataSource = new DataTable();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (!MyUtility.Check.Empty(this.CurrentMaintain["FINMgrApvDate"]))
            {
                this.displayApproveDate.Text = ((DateTime)this.CurrentMaintain["FINMgrApvDate"]).ToString("yyyy/MM/dd hh:mm:ss");
            }

            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                this.lblStatus.Text = "FIN Manager Approve";
            }
            else
            {
                this.lblStatus.Text = this.CurrentMaintain["Status"].ToString();
            }

            this.btnImportGMTBooking.Enabled = this.EditMode;

            if (!this.EditMode)
            {
                this.comboCompany.IsOrderCompany = null;
                this.comboCompany.Junk = null;
                if (this.CurrentMaintain != null && !MyUtility.Check.Empty(this.CurrentMaintain["OrderCompanyID"]))
                {
                    this.comboCompany.SelectedValue = (object)this.CurrentMaintain["OrderCompanyID"];
                }
            }

            this.GetGridPOListData();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select  kd.*,
        GB.ETD,
        GB.ETA,
        GB.TotalShipQty
from KHCMTInvoice_Detail kd
left join GMTBooking GB with (nolock) on kd.InvNo = GB.ID
where kd.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings colInvNo = new DataGridViewGeneratorTextColumnSettings();

            #region InvNo col setting
            colInvNo.EditingMouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                string sqlGetGB = $@"
SELECT  GB.ID AS [GB#],
        GB.ETD AS [On Board Date],
        GB.ETA AS [ETA],
        GB.TotalShipQty AS [Q'ty (Pcs)] 
FROM GMTBooking GB with (nolock)
WHERE GB.ETD IS NOT NULL
AND GB.[Status] = 'Confirmed'
AND GB.ETD >= '{this.minETD}'
AND NOT Exists (SELECT 1 FROM KHCMTInvoice_Detail kd with (nolock) where GB.ID = kd.InvNo)
ORDER BY GB.ETD
";

                SelectItem selectItem = new SelectItem(sqlGetGB, string.Empty, string.Empty);
                DialogResult dialogResult = selectItem.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                this.CurrentDetailData["InvNo"] = selectItem.GetSelecteds()[0]["GB#"];
                this.CurrentDetailData["ETD"] = selectItem.GetSelecteds()[0]["On Board Date"];
                this.CurrentDetailData["ETA"] = selectItem.GetSelecteds()[0]["ETA"];
                this.CurrentDetailData["TotalShipQty"] = selectItem.GetSelecteds()[0]["Q'ty (Pcs)"];

                this.GetGridPOListData();
            };

            colInvNo.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData["InvNo"].ToString() == e.FormattedValue.ToString())
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["InvNo"] = string.Empty;
                    this.CurrentDetailData["ETD"] = DBNull.Value;
                    this.CurrentDetailData["ETA"] = DBNull.Value;
                    this.CurrentDetailData["TotalShipQty"] = 0;
                    this.GetGridPOListData();
                    return;
                }

                string gbID = e.FormattedValue.ToString();
                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@GBID", gbID) };

                string existsOtherInvNo = MyUtility.GetValue.Lookup("select ID from KHCMTInvoice_Detail with (nolock) where InvNo = @GBID", listPar);
                if (!MyUtility.Check.Empty(existsOtherInvNo))
                {
                    MyUtility.Msg.WarningBox($"{gbID} already exists CMT Invoice#{existsOtherInvNo}");
                    e.Cancel = true;
                    return;
                }

                DataRow drResult;
                string sqlGetGB = $@"
SELECT  GB.ID AS [GB#],
        GB.ETD AS [On Board Date],
        GB.ETA AS [ETA],
        GB.TotalShipQty AS [Q'ty (Pcs)] 
FROM GMTBooking GB with (nolock)
WHERE GB.ETD IS NOT NULL
and GB.ID = @GBID
AND GB.[Status] = 'Confirmed'
AND GB.ETD >= '{this.minETD}'
ORDER BY GB.ETD
";

                bool isExists = MyUtility.Check.Seek(sqlGetGB, listPar, out drResult);

                if (!isExists)
                {
                    MyUtility.Msg.WarningBox($"{gbID} does not exist.");
                    e.Cancel = true;
                    return;
                }

                this.CurrentDetailData["InvNo"] = drResult["GB#"];
                this.CurrentDetailData["ETD"] = drResult["On Board Date"];
                this.CurrentDetailData["ETA"] = drResult["ETA"];
                this.CurrentDetailData["TotalShipQty"] = drResult["Q'ty (Pcs)"];
                this.GetGridPOListData();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridCurrency)
                .Text("Currency", header: "Currency", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("InvNo", header: "GB#", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: colInvNo)
                .Date("ETD", header: "On Board Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ETA", header: "ETA", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("TotalShipQty", header: "Q'ty (Pcs)", width: Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.gridPOList)
                .Numeric("No", header: "No.", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CustPONo", header: "Buyer PO#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderID", header: "CMT PO#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("UnitPriceUSD", header: "Unit Price (USD)", width: Widths.AnsiChars(10), integer_places: 9, decimal_places: 3, iseditingreadonly: true)
                .Numeric("ShipQty", header: "Q'ty (Pcs)", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("AmountUSD", header: "Amount(USD)", width: Widths.AnsiChars(15), integer_places: 12, decimal_places: 3, iseditingreadonly: true)
                .Numeric("AmountKHR", header: "Amount(KHR)", width: Widths.AnsiChars(15), integer_places: 12, decimal_places: 3, iseditingreadonly: true)
                ;

            this.detailgrid.Columns["InvNo"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.InfoBox("This record already confirmed, can not edit");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.comboCompany.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["InvDate"] = DateTime.Now;
            this.CurrentMaintain["Status"] = "New";
            this.gridCurrency.DataSource = null;
            this.CurrentMaintain["Handle"] = Env.User.UserID;
            this.RefreshExchangeRate();

            // 只有新增時才能編輯修改
            this.comboCompany.ReadOnly = false;
            this.comboCompany.IsOrderCompany = true;
            this.comboCompany.Junk = false;
            this.comboCompany.SelectedIndex = -1;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail no data");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderCompanyID"]))
            {
                this.comboCompany.Select();
                MyUtility.Msg.WarningBox("[Order Company] cannot be empty.");
                return false;
            }

            string whereInvNo = this.DetailDatas.Select(s => $"'{s["InvNo"]}'").JoinToString(",");

            string sqlCheckInvoice = $@"
select  kd.ID as [CMT Invoice No.],
        kd.InvNo as [GB#],
        gb.ETD as [On Board Date],
        gb.ETA as [ETA]
from KHCMTInvoice_Detail kd with (nolock)
left join GMTBooking gb with (nolock) on gb.ID = kd.InvNo
where   kd.ID <> '{this.CurrentMaintain["ID"]}' and
        kd.InvNo in ({whereInvNo})
        
";
            DataTable dtCheckInvoice;
            DualResult result = DBProxy.Current.Select(null, sqlCheckInvoice, out dtCheckInvoice);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (dtCheckInvoice.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid_LockScreen(dtCheckInvoice, "The following GB# already exist in CMT Invoice#.", "P63. Save");
                return false;
            }

            #region 產生ID 序號by 年(YY)重置

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@CMTInvDate", this.CurrentMaintain["InvDate"]) };

                string idHeader = MyUtility.GetValue.Lookup("select RgCode + Format(@CMTInvDate, 'yyMM') + '-' from dbo.system", listPar);
                string sqlGetID = @"
Declare @IDkeyWord varchar(5)
select @IDkeyWord = RgCode + Format(@CMTInvDate, 'yy') from dbo.system

select ID from KHCMTInvoice where ID like @IDkeyWord + '%'
";
                DataTable dtKHCMTInvoiceID;
                result = DBProxy.Current.Select(null, sqlGetID, listPar, out dtKHCMTInvoiceID);

                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                string seq = "001";

                if (dtKHCMTInvoiceID.Rows.Count > 0)
                {
                    seq = (dtKHCMTInvoiceID.AsEnumerable()
                            .Select(s => MyUtility.Convert.GetInt(s["ID"].ToString().Split('-')[1]))
                            .Max() + 1
                          ).ToString().PadLeft(3, '0');
                }

                this.CurrentMaintain["ID"] = idHeader + seq;
                this.IsChangeID = true;
            }
            else
            {
                this.IsChangeID = false;
            }

            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            // 回填BIR Invoice 至GMTBooking.CMTInvoiceNo
            if (this.detailgridbs.DataSource != null && this.IsChangeID == true)
            {
                DataTable detail = (DataTable)this.detailgridbs.DataSource;
                string updCmd = string.Empty;
                foreach (DataRow item in detail.Rows)
                {
                    updCmd += $@"
update GMTBooking
set CMTInvoiceNo = '{this.CurrentMaintain["ID"]}'
where ID='{item["InvNo"]}'
";
                }

                DualResult reusult = DBProxy.Current.Execute(null, updCmd);
                if (!reusult)
                {
                    this.ShowErr(reusult);
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            string sqlComfirm = $"update KHCMTInvoice set FINMgrApvName = '{Env.User.UserID}', FINMgrApvDate = getdate(), Status = 'Confirmed' where ID = '{this.CurrentMaintain["ID"]}'";

            DualResult result = DBProxy.Current.Execute(null, sqlComfirm);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Finance manager approve success!!");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string sqlComfirm = $"update KHCMTInvoice set EditName = '{Env.User.UserID}', EditDate = getdate(), Status = 'New' where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, sqlComfirm);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.EditMode)
            {
                MyUtility.Msg.InfoBox("Edit mode can not print");
                return false;
            }

            this.ShowLoadingText("Loading...");

            string strXltName = string.Empty;

            if (Env.User.Keyword == "KM1")
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_P63_SPR.xltx";
            }
            else
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_P63_SPS.xltx";
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[10, 9] = $"លេខវិក្កយបត្រ៖{this.CurrentMaintain["ID"]}";
            worksheet.Cells[11, 9] = $"Invoice No:{this.CurrentMaintain["ID"]}";
            worksheet.Cells[11, 2] = $"Customer : " + MyUtility.GetValue.Lookup($@"SELECT NameEN FROM Company WHERE ID = '{this.CurrentMaintain["OrderCompanyID"]}'");

            string invDate = ((DateTime)this.CurrentMaintain["InvDate"]).ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            worksheet.Cells[12, 9] = $"កាលបរិច្ឆេត៖{invDate}";
            worksheet.Cells[13, 9] = $"Invoice Date:{invDate}";

            worksheet.Cells[18, 9] = this.CurrentMaintain["ExchangeRate"];

            int endRowNum = 0;
            List<DataRow> listReportSource = ((DataTable)this.gridPOListbs.DataSource).AsEnumerable().ToList();

            // 第一頁可以放35筆
            // 只有一頁
            if (listReportSource.Count <= 35)
            {
                endRowNum = this.ExcelWriteRowData(listReportSource, worksheet, 21);
            }
            else
            {
                Excel.Range headerRange = worksheet.get_Range($"B19", $"I20");

                // 第一頁
                endRowNum = this.ExcelWriteRowData(listReportSource.Take(35), worksheet, 21);

                int newPageCnt = MyUtility.Convert.GetInt(Math.Ceiling(MyUtility.Convert.GetDecimal((listReportSource.Count - 35) / 57.0)));

                // 第一頁以外可以放57筆
                int otherPageRowCnt = 57;
                for (int i = 0; i < newPageCnt; i++)
                {
                    int pageStartNum = 66 + (i * 64);
                    int skipRowCnt = 35 + (i * otherPageRowCnt);

                    endRowNum = this.ExcelWriteRowData(listReportSource.Skip(skipRowCnt).Take(otherPageRowCnt), worksheet, pageStartNum);
                }
            }

            // 寫入加總區域
            List<CurrencyAMT> listCurrencyAMT = (List<CurrencyAMT>)this.gridCurrency.DataSource;

            // TTL:
            worksheet.Range[worksheet.Cells[endRowNum, 6], worksheet.Cells[endRowNum, 7]].Merge();
            worksheet.Cells[endRowNum, 6] = "TTL:";
            worksheet.Cells[endRowNum, 6].HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            worksheet.Cells[endRowNum, 8] = this.numDetailTotalQty.Value;
            worksheet.Cells[endRowNum, 9] = listCurrencyAMT.Where(s => s.Currency == "USD").First().Amount;
            worksheet.Cells[endRowNum, 9].NumberFormat = "$* #,##0.00";

            // sub Total
            endRowNum += 1;
            worksheet.Range[worksheet.Cells[endRowNum, 6], worksheet.Cells[endRowNum, 8]].Merge();
            worksheet.Cells[endRowNum, 6] = "សរុប / Sub Total";
            worksheet.Cells[endRowNum, 9] = listCurrencyAMT.Where(s => s.Currency == "USD").First().Amount;
            worksheet.Cells[endRowNum, 9].NumberFormat = "$* #,##0.00";

            // Grand Total (USD)
            endRowNum += 1;
            worksheet.Range[worksheet.Cells[endRowNum, 6], worksheet.Cells[endRowNum, 8]].Merge();
            worksheet.Cells[endRowNum, 6] = "សរុបរួមអាករជាដុល្លាអាមេរិច / Grand Total (USD)";
            worksheet.Cells[endRowNum, 9] = listCurrencyAMT.Where(s => s.Currency == "USD").First().Amount;
            worksheet.Cells[endRowNum, 9].NumberFormat = "$* #,##0.00";

            // Grand Total (KHR)
            endRowNum += 1;
            worksheet.Range[worksheet.Cells[endRowNum, 6], worksheet.Cells[endRowNum, 8]].Merge();
            worksheet.Cells[endRowNum, 6] = "សរុបរួមអាករជាប្រាក់រៀល / Grand Total (KHR)";
            worksheet.Cells[endRowNum, 9] = listCurrencyAMT.Where(s => s.Currency == "KHR").First().Amount;

            // 畫線
            Excel.Range workSheet_range = worksheet.get_Range($"F{endRowNum - 3}", $"I{endRowNum}");
            workSheet_range.Cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            worksheet.Protect(Password: "Sport2006");

            excel.Visible = true;
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            this.HideLoadingText();
            return base.ClickPrint();
        }

        private int ExcelWriteRowData(IEnumerable<DataRow> rowDatas, Excel.Worksheet worksheet, int startRownum)
        {
            int curRowNum = startRownum;
            foreach (DataRow dr in rowDatas)
            {
                worksheet.Cells[curRowNum, 2] = dr["No"];
                worksheet.Cells[curRowNum, 3] = dr["CustPONo"];
                worksheet.Cells[curRowNum, 4] = dr["OrderID"];
                worksheet.Cells[curRowNum, 5] = dr["StyleID"];
                worksheet.Cells[curRowNum, 6] = dr["Description"];
                worksheet.Cells[curRowNum, 7] = dr["UnitPriceUSD"];
                worksheet.Cells[curRowNum, 8] = dr["ShipQty"];
                worksheet.Cells[curRowNum, 9] = dr["AmountUSD"];
                worksheet.Cells[curRowNum, 9].NumberFormat = "$* #,##0.00";
                curRowNum++;
            }

            // 畫線
            Excel.Range workSheet_range = worksheet.get_Range($"B{startRownum}", $"I{curRowNum - 1}");
            workSheet_range.Cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            return curRowNum;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRemoveClick()
        {
            base.OnDetailGridRemoveClick();
            this.GetGridPOListData();
        }

        private void RefreshExchangeRate()
        {
            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@InvDate", this.CurrentMaintain["InvDate"]) };

            string sqlGetExchangeRate = @"
SELECT top 1 Rate 
FROM FinanceEN..Rate 
WHERE   RateTypeID='KP'           and
        OriginalCurrency='USD'    and
        ExchangeCurrency='KHR'    and
        @InvDate between BeginDate and EndDate
";
            DataRow drResult;

            if (MyUtility.Check.Seek(sqlGetExchangeRate, listPar, out drResult))
            {
                this.CurrentMaintain["ExchangeRate"] = drResult["Rate"];
            }
            else
            {
                this.CurrentMaintain["ExchangeRate"] = 0;
            }
        }

        private void DateInvDate_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["InvDate"] = this.dateInvDate.Value;
            this.RefreshExchangeRate();
        }

        private void GetGridPOListData()
        {
            if (!this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["InvNo"])))
            {
                this.gridPOListbs.DataSource = null;
                return;
            }

            string whereInvNo = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["InvNo"])).Select(s => $"'{s["InvNo"].ToString()}'").JoinToString(",");

            // [ISP20241007] 已報帳資料不更新
            string isNewData = this.CurrentMaintain["AddDate"].ToDateTime() >= new DateTime(2024, 11, 05) ? "1" : "0";

            string sqlGetGridPOListData = $@"
Declare @ExchangeRate decimal(18, 8) = {this.CurrentMaintain["ExchangeRate"]}

select	o.ID,
		[UnitPriceUSD] = ((isnull(o.CPU, 0) + isnull(SubProcessCPU.val, 0)) * isnull(CpuCost.val, 0)) + isnull(SubProcessAMT.val, 0) + isnull(LocalPurchase.val, 0)
		into #tmpUnitPriceUSD
from Orders o with (nolock)
left join Factory f with (nolock) on f.ID = o.FactoryID
outer apply (select [val] = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.ID,'CPU')) SubProcessCPU
outer apply (select [val] = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.ID,'AMT')) SubProcessAMT
outer apply (
    select top 1 [val] = fd.CpuCost
    from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
    where fsd.BrandID = o.BrandID
    and fsd.FactoryID = o.FactoryID
    and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
    and fsd.ShipperID = fd.ShipperID
    and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and (fsd.SeasonID = o.SeasonID or fsd.SeasonID = '')
    and fd.OrderCompanyID = o.OrderCompanyID
	order by SeasonID desc
) CpuCost
outer apply (select [val] = iif(f.LocalCMT = 1 and {isNewData} = 1, dbo.GetLocalPurchaseStdCost(o.ID), 0)) LocalPurchase
where exists (select 1 
			  from PackingList p with (nolock)
			  inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
			  where p.INVNo in ({whereInvNo}) and pd.OrderID = o.ID
			  )

select	[No] = 0,
        o.CustPONo,
		[OrderID] = o.ID,
		o.StyleID,
		s.Description,
		tup.UnitPriceUSD,
		[ShipQty] = sum(pd.ShipQty),
		[AmountUSD] = sum(pd.ShipQty) * tup.UnitPriceUSD,
		[AmountKHR] = Round(sum(pd.ShipQty) * tup.UnitPriceUSD * @ExchangeRate, 0)
from PackingList p with (nolock)
inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
inner join Orders o with (nolock) on pd.OrderID = o.ID
inner join Style s with (nolock) on s.Ukey = o.StyleUkey
left join #tmpUnitPriceUSD tup on tup.ID = o.ID
where p.INVNo in ({whereInvNo})
group by    o.CustPONo,
		    o.ID,
		    o.StyleID,
		    s.Description,
		    tup.UnitPriceUSD

drop table #tmpUnitPriceUSD
";

            DataTable dtPOList;

            DualResult result = DBProxy.Current.Select(null, sqlGetGridPOListData, out dtPOList);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByMutiInvNo(this.DetailDatas.Select(s => s["InvNo"].ToString()).ToList());

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtA2BResult;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlGetGridPOListData, out dtA2BResult);

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dtPOList.MergeBySyncColType(dtA2BResult);
            }

            // 因為A2B會使用webapi抓，所以No要在資料合併完再排序
            if (dtPOList.Rows.Count > 0)
            {
                dtPOList = dtPOList.AsEnumerable().OrderBy(s => s["CustPONo"]).ThenBy(s => s["OrderID"]).ThenBy(s => s["StyleID"]).CopyToDataTable();
                int rowNum = 1;
                foreach (DataRow dr in dtPOList.Rows)
                {
                    dr["No"] = rowNum;
                    rowNum++;
                }
            }

            this.gridPOListbs.DataSource = dtPOList;
            this.numDetailTotalQty.Value = dtPOList.AsEnumerable().Sum(s => MyUtility.Convert.GetInt(s["ShipQty"]));

            if (dtPOList.Rows.Count > 0)
            {
                List<CurrencyAMT> listCurrencyAMT = new List<CurrencyAMT>();
                listCurrencyAMT.Add(new CurrencyAMT { Currency = "KHR", Amount = 0 });
                listCurrencyAMT.Add(new CurrencyAMT { Currency = "USD", Amount = 0 });
                var currencyResult = dtPOList.AsEnumerable()
                    .GroupBy(s => string.Empty)
                    .Select(s => new
                    {
                        AmtUSD = s.Sum(groupItem => MyUtility.Convert.GetDecimal(groupItem["AmountUSD"])),
                        AmtKHR = s.Sum(groupItem => MyUtility.Convert.GetDecimal(groupItem["AmountKHR"])),
                    }).First();

                foreach (CurrencyAMT itemCurrencyAMT in listCurrencyAMT)
                {
                    switch (itemCurrencyAMT.Currency)
                    {
                        case "KHR":
                            itemCurrencyAMT.Amount = currencyResult.AmtKHR;
                            break;
                        case "USD":
                            itemCurrencyAMT.Amount = currencyResult.AmtUSD;
                            break;
                        default:
                            break;
                    }
                }

                this.gridCurrency.DataSource = listCurrencyAMT;
            }
        }

        private class CurrencyAMT
        {
            public string Currency { get; set; }

            public decimal Amount { get; set; }
        }

        private void BtnExchangeRate_Click(object sender, EventArgs e)
        {
            string sqlGetExchangeRate = @"
SELECT  [Begin Date] = r.BeginDate,
        [End Date] = r.EndDate,
        [Exchange Rate] = r.Rate 
FROM FinanceEN..Rate r 
WHERE RateTypeID='KP'
AND OriginalCurrency='USD'
AND ExchangeCurrency='KHR' 
order by r.BeginDate
";
            SelectItem selectItem = new SelectItem(sqlGetExchangeRate, null, null);

            DialogResult dialogResult = selectItem.ShowDialog();

            if (!this.EditMode)
            {
                return;
            }

            this.CurrentMaintain["ExchangeRate"] = selectItem.GetSelecteds()[0]["Exchange Rate"];
            this.GetGridPOListData();
        }

        private void BtnImportGMTBooking_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = new P63_Import(this.CurrentMaintain["ID"].ToString(), (DataTable)this.detailgridbs.DataSource).ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.GetGridPOListData();
            }
        }

        private void NumExchangeRate_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Convert.GetDecimal(this.CurrentMaintain["ExchangeRate"]) == this.numExchangeRate.Value)
            {
                return;
            }

            this.CurrentMaintain["ExchangeRate"] = this.numExchangeRate.Value;

            this.GetGridPOListData();
        }

        private void ComboCompany_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboCompany.SelectedIndex < 0 || this.EditMode == false)
            {
                return;
            }

            if (this.CurrentMaintain != null && this.DetailDatas.Count != 0)
            {
                bool hasGBNo = this.DetailDatas.Where(r => !MyUtility.Check.Empty(r["InvNo"])).Any();

                // 如果表身有SP# 並且表頭的OrderCompanyID改變就要判斷
                if (MyUtility.Convert.GetInt(this.CurrentMaintain["OrderCompanyID"]) != MyUtility.Convert.GetInt(this.comboCompany.SelectedValue) && hasGBNo)
                {
                    DialogResult dioResult = MyUtility.Msg.QuestionBox(@" [Order Company] has been changed and all GB# data will be clear.", buttons: MessageBoxButtons.YesNo);

                    // Yes 就刪除所有表身資料
                    if (dioResult == DialogResult.Yes)
                    {
                        foreach (DataRow item in this.DetailDatas)
                        {
                            item.Delete();
                        }

                        foreach (DataRow dr in ((DataTable)this.gridPOListbs.DataSource).Rows)
                        {
                            dr.Delete();
                        }
                    }
                }
            }
        }
    }
}
