using Ict;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R63 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DualResult result;
        private DateTime? dateCMTInvDateFrom;
        private DateTime? dateCMTInvDateTo;
        private string strGBFrom;
        private string strGBTo;
        private bool outstanding;

        /// <inheritdoc/>
        public R63(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.txtBrand.MultiSelect = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.dateCMTInvDateFrom = this.dateCMTInvDate.Value1;
            this.dateCMTInvDateTo = this.dateCMTInvDate.Value2;
            this.strGBFrom = this.txtGBFrom.Text;
            this.strGBTo = this.txtGBTo.Text;
            this.outstanding = this.chkOutstanding.Checked;

            if (MyUtility.Check.Empty(this.dateCMTInvDateFrom) && MyUtility.Check.Empty(this.dateCMTInvDateTo) &&
                MyUtility.Check.Empty(this.strGBFrom) && MyUtility.Check.Empty(this.strGBTo))
            {
                MyUtility.Msg.WarningBox($"Please input <{this.labCMTInv.Text}> or <{this.labGB.Text}> first!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlWhere = string.Empty;
            string sqlWhereForOrder = string.Empty;
            #region where
            if (!MyUtility.Check.Empty(this.dateCMTInvDateFrom) && !MyUtility.Check.Empty(this.dateCMTInvDateTo))
            {
                if (this.outstanding)
                {
                    sqlWhere += $@" and gb.ETD  between '{((DateTime)this.dateCMTInvDateFrom).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateCMTInvDateTo).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }
                else
                {
                    sqlWhere += $@" and k.InvDate  between '{((DateTime)this.dateCMTInvDateFrom).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateCMTInvDateTo).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }
            }

            if (!MyUtility.Check.Empty(this.strGBFrom))
            {
                sqlWhere += $@" and gb.ID >= '{this.strGBFrom}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strGBTo))
            {
                sqlWhere += $@" and gb.ID <= '{this.strGBTo}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                sqlWhereForOrder += $@" and o.BrandID in ({this.txtBrand.Text.Split(',').Select(s => $"'{s}'").JoinToString(",")})" + Environment.NewLine;
            }

            #endregion

            string sqlGetMainGB = $@"
select  k.InvDate,
        [CMTInv] = k.ID,
        [Client] = (SELECT NameEN FROM Company WHERE ID = gb.OrderCompanyID),
        [GBID] = gb.ID,
        [ExchangeRate] = isnull(k.ExchangeRate, 0),
        gb.BrandID,
        gb.Shipper
from    GMTBooking gb with (nolock)
left join KHCMTInvoice_Detail kd with (nolock) on kd.InvNo = gb.ID
left join KHCMTInvoice k with (nolock) on k.ID = kd.ID
where 1 = 1 {sqlWhere}
";
            DataTable dtMainGB;
            this.result = DBProxy.Current.Select(null, sqlGetMainGB, out dtMainGB);

            if (!this.result)
            {
                return this.result;
            }

            string sqlGetFinal = $@"
alter table #tmp alter column GBID varchar(25)

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
outer apply (select [val] = iif(f.LocalCMT = 1, dbo.GetLocalPurchaseStdCost(o.ID), 0)) LocalPurchase
where exists (select 1 
			  from PackingList p with (nolock)
			  inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
			  where p.INVNo in (select GBID from #tmp) and pd.OrderID = o.ID
			  ) {sqlWhereForOrder}


select  t.BrandID,
        t.Shipper,
        o.FactoryID,
        t.InvDate,
        t.CMTInv,
        t.Client,
        [CMTPO] = o.ID,
        o.CustPONo,
        t.GBID,
        o.StyleID,
        s.Description,
        [ShipQty] = sum(pd.ShipQty),
        tup.UnitPriceUSD,
        [AmountUSD] = sum(pd.ShipQty) * tup.UnitPriceUSD,
        [UnitPriceKHR] = Round(tup.UnitPriceUSD * t.ExchangeRate, 0),
        [AmountKHR] = sum(pd.ShipQty) * Round(tup.UnitPriceUSD * t.ExchangeRate, 0),
        t.ExchangeRate
from #tmp t
inner join PackingList p with (nolock) on p.InvNo = t.GBID
inner join PackingList_Detail pd with (nolock) on pd.ID = p.ID
inner join Orders o with (nolock) on o.ID = pd.OrderID
inner join Style s with (nolock) on s.Ukey = o.StyleUkey
left join #tmpUnitPriceUSD tup on tup.ID = o.ID
where 1 =1 {sqlWhereForOrder}
group by    t.BrandID,
            t.Shipper,
            o.FactoryID,
            t.InvDate,
            t.CMTInv,
            t.Client,
            o.ID,
            o.CustPONo,
            t.GBID,
            o.StyleID,
            s.Description,
            tup.UnitPriceUSD,
            t.ExchangeRate
";

            this.result = MyUtility.Tool.ProcessWithDatatable(dtMainGB, null, sqlGetFinal, out this.printData);

            if (!this.result)
            {
                return this.result;
            }

            List<string> listInvNo = new List<string>();

            if (dtMainGB.Rows.Count > 0)
            {
                listInvNo = dtMainGB.AsEnumerable().Select(s => s["GBID"].ToString()).Distinct().ToList();
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByMutiInvNo(listInvNo);

            foreach (string systemName in listPLFromRgCode)
            {
                DataTable dtA2BResult;
                DataBySql dataBySql = new DataBySql
                {
                    SqlString = sqlGetFinal,
                    TmpTable = JsonConvert.SerializeObject(dtMainGB),
                };
                this.result = PackingA2BWebAPI.GetDataBySql(systemName, dataBySql, out dtA2BResult);

                if (!this.result)
                {
                    return this.result;
                }

                this.printData.MergeBySyncColType(dtA2BResult);
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
            string reportName = "Shipping_R63.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R63");
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

        private void ChkOutstanding_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkOutstanding.Checked)
            {
                this.labCMTInv.Text = "On Board Date";
            }
            else
            {
                this.labCMTInv.Text = "CMT Invoice Date";
            }
        }
    }
}
