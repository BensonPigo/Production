using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    public partial class P11_Print : Sci.Win.Tems.PrintForm
    {
        private IList<DataRow> DetailDatas;
        private DataRow CurrentMaintain;
        /// <summary>
        /// P11_Print
        /// </summary>
        /// <param name="detailDatas">detailDatas</param>
        public P11_Print(DataRow currentMaintain, IList<DataRow> detailDatas)
        {
            this.InitializeComponent();
            this.DetailDatas = detailDatas;
            this.CurrentMaintain = currentMaintain;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.dateInvoice.ReadOnly = true;
            this.txtInvSerFrom.ReadOnly = true;
            this.txtInvSerTo.ReadOnly = true;
            this.txtShipper.ReadOnly = true;
        }

        protected override bool ToExcel()
        {
            if (this.radioBIRSalesInv.Checked)
            {
                return this.BIRSalesInvoice();
            }
            else
            {
                return this.BIRSalesReport();
            }
        }

        private bool BIRSalesReport()
        {
            #region get data
            string sqlGetData;
            string sqlInvDateWhere = string.Empty;
            string sqlShipperWhere = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if ((!this.dateInvoice.HasValue1 && !this.dateInvoice.HasValue2) ||
                 MyUtility.Check.Empty(this.txtShipper.Text))
            {
                MyUtility.Msg.WarningBox("Invoice Date & Shipper cannot be empty.");
                return false;
            }

            if (this.dateInvoice.HasValue1)
            {
                sqlInvDateWhere += " and gbi.FCRDate >= @InvDateFrom";
                listPar.Add(new SqlParameter("@InvDateFrom", this.dateInvoice.DateBox1.Value));
            }

            if (this.dateInvoice.HasValue2)
            {
                sqlInvDateWhere += " and gbi.FCRDate <= @InvDateTo";
                listPar.Add(new SqlParameter("@InvDateTo", this.dateInvoice.DateBox2.Value));
            }

            sqlShipperWhere += $" and gb.Shipper = '{this.txtShipper.Text}'";
            sqlInvDateWhere += $" and gbi.Shipper = '{this.txtShipper.Text}'";

            if (this.txtInvSerFrom.Text.Length > 0)
            {
                sqlShipperWhere += $" and bi.InvSerial >= '{this.txtInvSerFrom.Text}'";
            }

            if (this.txtInvSerTo.Text.Length > 0)
            {
                sqlShipperWhere += $" and bi.InvSerial <= '{this.txtInvSerTo.Text}'";
            }

            sqlGetData = $@"
select	bi.ID,
		bi.InvSerial,
		bi.BrandID,
		[GMTBooking] = gb.ID,
		[GMTFCRDate] = gb.FCRDate,
		[PackID] = pl.ID,
		[GRS_WEIGHT] = pl.GW,
		[Qty] = pl.ShipQty,
		[ShipTo] = FIRST_VALUE(ccd.BIRShipTo) OVER (Partition by bi.ID ORDER BY gb.AddDate desc),
		[DestCountry] = FIRST_VALUE(c.NameEN) OVER (Partition by bi.ID ORDER BY gb.AddDate desc)
into #tmpBIRInvoice
from BIRInvoice bi with (nolock)
inner join GMTBooking gb on bi.ID = gb.BIRID and bi.BrandID = gb.BrandID
inner join PackingList pl with (nolock) on pl.INVNo = gb.ID
inner join CustCD ccd with (nolock) on ccd.ID = gb.CustCDID and ccd.BrandID = gb.BrandID
inner join Country c with (nolock) on c.ID = ccd.CountryID
where	exists(select 1 from GMTBooking gbi with (nolock) where gbi.BIRID = bi.ID and gbi.BrandID = bi.BrandID {sqlInvDateWhere}) 
		{sqlShipperWhere}


--取得Std. Fty CMP
select [PackID] = pld.ID,
	   pld.OrderID,
	   [ShipQty] = SUM(pld.ShipQty)
into #tmpPackOrder
from PackingList_Detail pld with (nolock)
where exists( select 1 from #tmpBIRInvoice tbi where tbi.PackID = pld.ID)
group by pld.ID,pld.OrderID

select
o.ID,
o.CPU,
[SubProcessCPU] = SubProcessCPU.Value,
[SubProcessAMT] = SubProcessAMT.Value,
[CPUCost] = isnull(round(isnull(fsr_s.CpuCost,fsr_ns.CpuCost),3,1), 0),
[LocalPurchase] = LocalPurchase.Value,
[StdFtyCMP] = ROUND(std.FtyCMP,2)
into #tmpOrderStdFtyCMP
from orders o with (nolock)
inner join Factory f with (nolock) on o.FactoryID = f.ID
outer apply (select [Value] = isnull(sum(Isnull(Price,0)),0) from GetSubProcessDetailByOrderID(o.ID,'CPU')) SubProcessCPU
outer apply (select [Value] = isnull(sum(Isnull(Price,0)),0) from GetSubProcessDetailByOrderID(o.ID,'AMT')) SubProcessAMT
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	        and fsd.FactoryID = o.FactoryID
	        and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	        and fsd.ShipperID = fd.ShipperID
	        and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	        and o.OrigBuyerDelivery is not null
            and fsd.seasonID = o.seasonID
)fsr_s
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	        and fsd.FactoryID = o.FactoryID
	        and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	        and fsd.ShipperID = fd.ShipperID
	        and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	        and o.OrigBuyerDelivery is not null
            and fsd.seasonID = ''
)fsr_ns
outer apply (
    select [Value] = iif(f.LocalCMT = 1,dbo.GetLocalPurchaseStdCost(o.id),0)
) LocalPurchase
outer apply(
	select FtyCMP = Round((isnull(round(o.CPU,3,1),0) + isnull(round(SubProcessCPU.Value,3,1),0)) * isnull(round(isnull(fsr_s.CpuCost,fsr_ns.CpuCost),3,1),0) 
                            + isnull(round(subProcessAMT.Value,3,1),0) 
                            + isnull(round(LocalPurchase.Value,3,1),0)
                          , 3)
)std
where exists( select 1 from #tmpPackOrder tpack where tpack.OrderID = o.ID)

select
PackID,
[FOB_Value] = SUM(tpo.ShipQty * tos.StdFtyCMP),
[CMP_Value] = SUM(tpo.ShipQty * tos.StdFtyCMP)
into #PackCMP
from #tmpPackOrder tpo
inner join #tmpOrderStdFtyCMP tos on tpo.OrderID = tos.ID
group by tpo.PackID

select
[InvDate] = max(tbi.GMTFCRDate),
[PARTICULAR_DESCRIPTION] = 'SINTEX INTERNATIONAL LTD',
tbi.ShipTo,
tbi.InvSerial,
[GRS_WEIGHT] = sum(tbi.GRS_WEIGHT),
[Qty] = sum(tbi.Qty),
[FOB_Value] = sum(pc.FOB_Value),
[COST_OF_MATERIALS] = 0,
[CMP_Value] = sum(pc.CMP_Value),
[COUNTRY_SOLD_TO] = 'TAIWAN',
tbi.DestCountry,
[ExRate] = 51,
[CMP_ValuePH] = ''
from #tmpBIRInvoice tbi
inner join #PackCMP pc on pc.PackID = tbi.PackID
group by	tbi.ShipTo,tbi.ID,tbi.InvSerial,tbi.DestCountry


drop table #tmpBIRInvoice,#tmpPackOrder,#tmpOrderStdFtyCMP,#PackCMP
";

            DataTable dtResult;
            this.ShowWaitMessage("Excel Processing...");
            DualResult result = DBProxy.Current.Select(null, sqlGetData, listPar, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                this.HideWaitMessage();
                return false;
            }

            this.SetCount(dtResult.Rows.Count);

            if (dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No data found");
                this.HideWaitMessage();
                return false;
            }

            int rowNum = 2;
            foreach (DataRow dr in dtResult.Rows)
            {
                dr["ShipTo"] = dr["ShipTo"].ToString().Split(Convert.ToChar(10))[0];

                // 只保留前段的數值 後面其他的文字全數去除
                dr["InvSerial"] = dr["InvSerial"].ToString().TakeWhile(s => Regex.IsMatch(s.ToString(), "[0-9]")).Select(s => s.ToString()).JoinToString(string.Empty);
                dr["CMP_ValuePH"] = $"=I{rowNum}*L{rowNum}";
                rowNum++;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P11_BIRSalesReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                this.HideWaitMessage();
                return false;
            }

            MyUtility.Excel.CopyToXls(dtResult, string.Empty, "Shipping_P11_BIRSalesReport.xltx", 1, false, null, excel, wSheet: excel.Sheets[1]);

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P11_BIRSalesReport");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
            #endregion

        }

        private bool BIRSalesInvoice()
        {
            #region
            List<string> ids = new List<string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                ids.Add("'" + dr["id"] + "'");
            }

            DataTable dt;
            string sqlcmd = $@"
select 
	A=o.CustPONo,
	B=o.StyleID,
	C=s.Description,
	E=sum(pd.ShipQty),
	F='PCS',
	G=o.CurrencyID,
	H=ROUND(std.FtyCMP,2),
	J=sum(pd.ShipQty)*ROUND(isnull(std.FtyCMP,0),2)
from orders o with(nolock)
inner join PackingList_Detail pd with(nolock) on pd.OrderID = o.id
inner join PackingList p with(nolock) on p.id = pd.id
left join Style s with(nolock) on s.Ukey = o.StyleUkey
outer apply(select SubProcessCPU= sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.id,'CPU'))a
outer apply(select subProcessAMT= sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.id,'AMT'))b
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = o.seasonID
)f1
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = ''
)f
outer apply(
	select dbo.GetLocalPurchaseStdCost(o.id) price
)s3
outer apply(
	select FtyCMP = Round((isnull(round(o.CPU,3,1),0) + isnull(round(a.SubProcessCPU,3,1),0)) * 
	isnull(round(isnull(f1.CpuCost,f.CpuCost),3,1),0) + isnull(round(b.subProcessAMT,3,1),0) + isnull(round(s3.price,3,1),0), 3)
)std
where p.INVNo in({string.Join(",", ids)})
group by o.CustPONo,o.StyleID,s.Description,o.PoPrice,o.id,o.CPU,o.CurrencyID,std.FtyCMP
";
            this.ShowWaitMessage("Excel Processing...");
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                this.HideWaitMessage();
                return false;
            }
            #endregion

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P11.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                this.HideWaitMessage();
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 產生頁首頁尾資料

            // 頁首
            string top1idsql = $@"
select top 1 id
from GMTBooking
where id in({string.Join(",", ids)})
order by FCRDate desc
";
            string top1id = MyUtility.GetValue.Lookup(top1idsql);

            string bIRShipToSql = $@"
select top 1 b.BIRShipTo
from GMTBooking a
inner join CustCD b on b.id = a.CustCDID and b.BrandID = a.BrandID
where a.id = '{top1id}' 
";
            string top1BIRShipTo = MyUtility.GetValue.Lookup(bIRShipToSql);
            if (!MyUtility.Check.Empty(top1BIRShipTo))
            {
                string[] BIRShipToarry = top1BIRShipTo.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0, j = 0; i < BIRShipToarry.Length; i++)
                {
                    if (!MyUtility.Check.Empty(BIRShipToarry[i]))
                    {
                        worksheet.Cells[12 + j, 2] = BIRShipToarry[i];
                        j++;
                    }
                }
            }

            DataRow drGMT;
            string top1GMT = $@"select * from GMTBooking where id = '{top1id}'";
            if (MyUtility.Check.Seek(top1GMT, out drGMT))
            {
                worksheet.Cells[9, 10] = drGMT["FCRDate"];
                worksheet.Cells[11, 10] = drGMT["Vessel"];
                worksheet.Cells[16, 10] = MyUtility.GetValue.Lookup($@"select NameEN from Country where id = '{drGMT["Dest"]}'");
            }

            // 頁尾
            decimal sumJ = MyUtility.Convert.GetDecimal(dt.Compute("sum(J)", null));
            decimal sumM = sumJ;
            decimal sumE = MyUtility.Convert.GetDecimal(dt.Compute("sum(E)", null));

            //worksheet.Cells[48, 3] = MyUtility.Convert.USDMoney(sumI).Replace("AND CENTS", Environment.NewLine + "AND CENTS");
            worksheet.Cells[57, 3] = MyUtility.Convert.USDMoney(sumJ);

            string sumGW = $@"
select sumGW = Sum (p.GW),sumNW=sum(p.NW),sumCBM=sum(p.CBM)
from PackingList p with(nolock)
where p.INVNo in ({string.Join(",", ids)})
";
            DataRow drsum;
            if (MyUtility.Check.Seek(sumGW, out drsum))
            {
                worksheet.Cells[66, 3] = drsum["sumGW"];
                worksheet.Cells[67, 3] = drsum["sumNW"];
                worksheet.Cells[68, 3] = drsum["sumCBM"];
            }

            worksheet.Cells[63, 10] = sumJ;
            worksheet.Cells[65, 10] = 0;
            worksheet.Cells[66, 10] = sumM;
            worksheet.Cells[60, 2] = sumE;
            worksheet.Cells[1, 10] = $@"InvSerial: {this.CurrentMaintain["InvSerial"]}";
            #endregion

            #region 內容

            // 如果內容超過41筆插入新的頁面
            int insertSheetCount = dt.Rows.Count / 41;
            for (int i = 1; i <= insertSheetCount; i++)
            {
                worksheet.Copy(Type.Missing, worksheet);
            }

            int contentCount = 41;
            int ttlSheetCount = excel.ActiveWorkbook.Worksheets.Count;
            for (int i = 1; i <= ttlSheetCount; i++)
            {
                var xlNewSheet = (Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i];
                xlNewSheet.Name = "BIR Invoice-" + i.ToString();
                int intRowsStart = 25;
                int dataEnd = i * contentCount;
                int dataStart = dataEnd - contentCount;
                object[,] objArray = new object[1, 10];

                for (int j = dataStart; j < dataEnd; j++)
                {
                    if (j >= dt.Rows.Count)
                    {
                        break;
                    }

                    DataRow dr = dt.Rows[j];
                    int rownum = intRowsStart++;
                    objArray[0, 0] = dr["A"];
                    objArray[0, 1] = dr["B"];
                    objArray[0, 2] = dr["C"];
                    objArray[0, 3] = string.Empty;
                    objArray[0, 4] = dr["E"];
                    objArray[0, 5] = dr["F"];
                    objArray[0, 6] = dr["G"];
                    objArray[0, 7] = dr["H"];
                    objArray[0, 8] = string.Empty;
                    objArray[0, 9] = dr["J"];
                    xlNewSheet.Range[string.Format("A{0}:J{0}", rownum)].Value2 = objArray;
                }

                Marshal.ReleaseComObject(xlNewSheet);
            }
            #endregion

            #region Save & Show Excel
            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Activate();
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P11");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void RadioBIRSalesReport_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioBIRSalesInv.Checked)
            {
                this.dateInvoice.ReadOnly = true;
                this.txtInvSerFrom.ReadOnly = true;
                this.txtInvSerTo.ReadOnly = true;
                this.txtShipper.ReadOnly = true;
            }
            else
            {
                this.dateInvoice.ReadOnly = false;
                this.txtInvSerFrom.ReadOnly = false;
                this.txtInvSerTo.ReadOnly = false;
                this.txtShipper.ReadOnly = false;
            }
        }
    }
}
