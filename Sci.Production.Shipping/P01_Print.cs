using Ict;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Sci.Production.CallPmsAPI;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Ict.BaseResult;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P01_Print
    /// </summary>
    public partial class P01_Print : Win.Tems.PrintForm
    {
        private DataRow masterData;
        private System.Data.DataTable[] dts;

        private string reportType;

        /// <summary>
        /// P01_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P01_Print(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.radioRequest.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.radioRequest.Checked)
            {
                this.reportType = "1";
            }

            if (this.radioDetail.Checked)
            {
                this.reportType = "2";
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.reportType == "2")
            {
                var result = this.GetExcelData();
                if (result)
                {
                    return Ict.Result.True;
                }
                else
                {
                    if (!result.Description.Empty())
                    {
                        return result;
                    }
                }

                string sqlcmd = $@"
select pd.OrderID, pd.OrderShipmodeSeq, p.INVNo
into #tmp
from PackingList p
inner join PackingList_Detail pd on p.ID = pd.ID
inner join GMTBooking g on g.ID = p.INVNo
where exists 
(
	select 1
	from PackingList_Detail pd2
	inner join PackingList p2 on pd2.ID = p2.ID
	inner join GMTBooking g2 on g2.ID = p2.INVNo
	where pd2.OrderID = '{this.masterData["OrderID"]}'
	and pd2.OrderShipmodeSeq = '{this.masterData["OrderShipmodeSeq"]}'
	and (g2.BLNo = g.BLNo or g2.BL2No = g.BL2No)
)
and g.ShipModeID like '%P%'


select se.Description,a.Qty,se.UnitID,a.CurrencyID,a.Price
, Amount = cast((a.Qty * a.Price) AS decimal(18, 2)), a.Rate, a.PayCurrency, PayAmount= a.Amount
, a.CW
into #table1
from (
	select sd.*,s.CurrencyID as PayCurrency,s.CW,s.CDate
	from ShippingAP s
	inner join ShippingAP_Detail sd on s.ID = sd.ID
	where exists(
		select 1
		from GMTBooking g
		where g.ID in (select INVNo from #tmp)
		and (g.BLNo = s.BLNo  or g.BL2No = s.BLNo)
	)
) a
left join ShipExpense se on se.ID = a.ShipExpenseID

select Description,Qty,UnitID,CurrencyID,Price,Amount,Rate,PayCurrency,PayAmount from #table1

select p.INVNo
,pd.OrderID
,o.CustPONo
,[APPID] = ap.ID
,[Qty] = sum(pd.ShipQty)
,[GW] = p.GW * gw.value
,[CW] = (select min(isnull(cw,0)) from #table1) * gw.value
,[Air_FREIGHT] = af.total
,[EQV] = ap.Additional
from PackingList p
inner join PackingList_Detail pd on p.ID = pd.ID
left join Orders o on o.ID = pd.OrderID
left join AirPP ap on ap.OrderID= pd.OrderID and ap.OrderShipmodeSeq = pd.OrderShipmodeSeq
outer apply (
	select value = sum(t.ShipQty * t.NWPerPcs) / (t2.value)
	from PackingList_Detail t
	outer apply(
		select value = sum(tt.ShipQty * tt.NWPerPcs) 
		from PackingList_Detail tt
		where tt.id=t.id
	)t2
	where t.OrderID = '{this.masterData["OrderID"]}'
	and t.OrderShipmodeSeq = '{this.masterData["OrderShipmodeSeq"]}'
	GROUP by t2.value
)gw
outer apply(
	select total = sum(cast(iif(sap.APPExchageRate = 0, 0, (se.AmtFty + se.AmtOther) / sap.APPExchageRate) AS decimal(18, 2)))
	from ShareExpense_APP se
	left join ShippingAP sap on sap.ID = se.ShippingAPID
	where se.AirPPID = ap.id and se.Junk = 0
)af
where exists(
	select 1 from #tmp
	where OrderID = pd.OrderID
	and OrderShipmodeSeq = pd.OrderShipmodeSeq
)
group by p.INVNo
,pd.OrderID
,o.CustPONo
,ap.ID
,p.GW ,gw.value
,af.total
,ap.Additional
order by p.INVNo,pd.OrderID

drop TABLE #tmp,#table1
";
                DBProxy.Current.Select(null, sqlcmd, out this.dts);
            }

            return Ict.Result.True;
        }

        private DualResult GetExcelData()
        {
            string sqlGetA2BPack = $@"
select distinct  p.INVNo
from PackingList p
inner join PackingList_Detail pd on p.ID = pd.ID
where   pd.OrderID = '{this.masterData["OrderID"]}' and
        pd.OrderShipmodeSeq = '{this.masterData["OrderShipmodeSeq"]}'
";
            System.Data.DataTable dtA2BInvNo;
            DualResult result = DBProxy.Current.Select(null, sqlGetA2BPack, out dtA2BInvNo);
            if (!result)
            {
                return result;
            }

            if (dtA2BInvNo.Rows.Count == 0)
            {
                return new DualResult(false, description: "InvoiceNo not find!");
            }

            string targetFty = MyUtility.GetValue.Lookup(string.Format(
                @"
select NegoRegion from Factory 
where ID in (select FactoryID from Orders where Id = '{0}') 
and IsProduceFty = 1 
and NegoRegion <> (select RgCode from System)"
, this.masterData["OrderID"].ToString()));
            if (targetFty.Empty())
            {
                return new DualResult(false, description: string.Empty);
            }

            string whereInvNo = dtA2BInvNo.AsEnumerable().Select(s => $"'{s["INVNo"]}'").JoinToString(",");
            string sqlGetGMTBookingByBL = $@"
select  g.ID
from GMTBooking g with (nolock)
where   exists( select 1 from GMTBooking g2 with (nolock) where
                g2.ID in ({whereInvNo}) and (g2.BLNo = g.BLNo or g2.BL2No = g.BL2No)
                )
        and g.ShipModeID like '%P%'
";

            System.Data.DataTable dtTargetInvNo;
            result = PackingA2BWebAPI.GetDataBySql(targetFty, sqlGetGMTBookingByBL, out dtTargetInvNo);
            if (!result)
            {
                return result;
            }

            string whereTargetInvNo = dtTargetInvNo.AsEnumerable().Select(s => $"'{s["ID"]}'").JoinToString(",");
            string sqlTargetPackingList = $@"
select pd.ID, pd.OrderID, pd.OrderShipmodeSeq, p.INVNo, pd.ShipQty, p.GW, pd.NWPerPcs
from PackingList p
inner join PackingList_Detail pd on p.ID = pd.ID
where p.INVNo in ({whereTargetInvNo})";
            System.Data.DataTable dtPackingList = new System.Data.DataTable();
            List<string> listPLFromRgCode = PackingA2BWebAPI.GetAllPLFromRgCode();
            foreach (string plFromRgCode in listPLFromRgCode)
            {
                System.Data.DataTable dtTargetPackingList;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlTargetPackingList, out dtTargetPackingList);
                if (!result)
                {
                    return result;
                }

                dtTargetPackingList.MergeTo(ref dtPackingList);
            }

            System.Data.DataTable dtOwnPackingList;
            result = DBProxy.Current.Select(null, sqlTargetPackingList, out dtOwnPackingList);
            if (!result)
            {
                return result;
            }

            dtOwnPackingList.MergeTo(ref dtPackingList);

            string sqlGetShippAP = $@"
declare @CurrencyID varchar(4)
Select @CurrencyID = CurrencyID From System
select se.Description,a.Qty,se.UnitID,'USD' as CurrencyID
, Price = iif(a.CurrencyID != 'USD', a.Price / a.APPExchageRate, a.Price) 
, Amount = Cast(iif(a.CurrencyID != 'USD', (a.Qty * a.Price) / a.APPExchageRate,a.Qty * a.Price) AS decimal(18, 2))
, Rate = iif(a.CurrencyID != 'USD', a.APPExchageRate, a.Rate)
, a.PayCurrency
, PayAmount = iif(a.PayCurrency != @CurrencyID, a.Amount * a.APPExchageRate, a.Amount)
, a.CW
into #table1
from (
	select sd.Qty
    ,sd.Price
    ,sd.CurrencyID
    ,sd.Amount
    ,sd.Rate
    ,sd.ShipExpenseID
    ,s.CurrencyID as PayCurrency
    ,s.CW
    ,s.CDate
	,s.APPExchageRate
	from ShippingAP s
	inner join ShippingAP_Detail sd on s.ID = sd.ID
	where exists(
		select 1
		from GMTBooking g
		inner join ShipMode sm with (nolock) on g.ShipModeID = sm.ID
		where g.ID in ({whereTargetInvNo})
		and (g.BLNo = s.BLNo  or g.BL2No = s.BLNo)
		and sm.NeedCreateAPP = 1
	)
    and s.Reason <> 'AP007'
	and (SELECT IsFreightForwarder From LocalSupp where ID = s.LocalSuppID) = 1
) a
left join ShipExpense se on se.ID = a.ShipExpenseID
where not (
			dbo.GetAccountNoExpressType(se.AccountID,'Vat') = 1 
			or dbo.GetAccountNoExpressType(se.AccountID,'SisFty') = 1
		)
		and dbo.GetAccountNoExpressType(se.AccountID,'IsApp') = 1 
		and se.Junk = 0

select Description,Qty,UnitID,CurrencyID,Price,Amount,Rate,PayCurrency,PayAmount,CW from #table1
Drop Table #table1
";
            System.Data.DataTable dtShippAP;
            result = PackingA2BWebAPI.GetDataBySql(targetFty, sqlGetShippAP, out dtShippAP);
            if (!result)
            {
                return result;
            }

            string sqlShareExpenseAPP = $@"
select se.AirPPID,total = sum(cast(iif(sap.APPExchageRate = 0, 0, (se.AmtFty + se.AmtOther) / sap.APPExchageRate) AS decimal(18, 2)))
	from ShareExpense_APP se
	left join ShippingAP sap on sap.ID = se.ShippingAPID
	where se.AirPPID = '{this.masterData["ID"]}' and se.Junk = 0
    group by se.AirPPID
";
            System.Data.DataTable dtShareExpenseAPP;
            result = PackingA2BWebAPI.GetDataBySql(targetFty, sqlShareExpenseAPP, out dtShareExpenseAPP);
            if (!result)
            {
                return result;
            }

            SqlConnection conn;
            DBProxy._OpenConnection("Production", out conn);
            using (conn)
            {
                result = MyUtility.Tool.ProcessWithDatatable(dtShippAP, null, "select * from #ShippAP", out dtShippAP, "#ShippAP", conn);
                if (!result)
                {
                    return result;
                }

                result = MyUtility.Tool.ProcessWithDatatable(dtShareExpenseAPP, null, "select * from #ShareExpenseAPP", out dtShareExpenseAPP, "#ShareExpenseAPP", conn);
                if (!result)
                {
                    return result;
                }

                result = MyUtility.Tool.ProcessWithDatatable(dtPackingList, null, "select * from #PackingList", out dtPackingList, "#PackingList", conn);
                if (!result)
                {
                    return result;
                }

                string sqlcmd = $@"
select Description,Qty,UnitID,CurrencyID,Price,Amount,Rate,PayCurrency,PayAmount from #ShippAP

Alter Table #PackingList Alter Column ID Varchar(13)
Alter Table #PackingList Alter Column OrderID Varchar(13)
Alter Table #PackingList Alter Column OrderShipmodeSeq Varchar(2)

select p.INVNo
,p.OrderID
,o.CustPONo
,[APPID] = ap.ID
,[Qty] = sum(p.ShipQty)
,[GW] = p.GW * gw.value
,[CW] = (select min(isnull(cw,0)) from #ShippAP) * gw.value
,[Air_FREIGHT] = af.total
,[EQV] = ap.Additional
from #PackingList p
left join Orders o on o.ID = p.OrderID
left join AirPP ap on ap.OrderID= p.OrderID and ap.OrderShipmodeSeq = p.OrderShipmodeSeq
outer apply (
	select value = sum(p1.ShipQty * p1.NWPerPcs) / (p2.value)
	from #PackingList p1
	outer apply(
		select value = sum(pp.ShipQty * pp.NWPerPcs) 
		from #PackingList pp
		where pp.id=p1.id
	)p2
	GROUP by p2.value
)gw
outer apply(
	select total from #ShareExpenseAPP se
	where se.AirPPID = ap.id
)af
group by p.INVNo
,p.OrderID
,o.CustPONo
,ap.ID
,p.GW ,gw.value
,af.total
,ap.Additional
order by p.INVNo,p.OrderID

drop TABLE #PackingList,#ShippAP,#ShareExpenseAPP
";
                result = DBProxy.Current.SelectByConn(conn, sqlcmd, out this.dts);
                if (!result)
                {
                    return result;
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.reportType == "1")
            {
                #region Detail List
                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P01.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                string status = MyUtility.GetValue.Lookup(string.Format("select Status from AirPP where id = '{0}'", this.masterData["id"].ToString()));
                switch (status)
                {
                    case "New":
                        status = "New";
                        break;
                    case "Junked":
                        status = "Junk";
                        break;
                    case "Checked":
                        status = "PPIC Checked";
                        break;
                    case "Approved":
                        status = "FTY Approved";
                        break;
                    case "Confirmed":
                        status = "SMR Comfirmed";
                        break;
                    case "Locked":
                        status = "GM Team Locked";
                        break;
                    default:
                        status = string.Empty;
                        break;
                }

                string strFactory = string.Empty;
                string strBrand = string.Empty;
                string strCountryDestination = string.Empty;
                string strStyleNo = string.Empty;
                string strDescription = string.Empty;
                int numOrderQty = 0;

                DualResult result = DBProxy.Current.Select(
                    null,
                        $@"
select o.FactoryID,o.BrandID,o.StyleID,o.Dest,isnull(oq.ShipmodeID,'') as ShipmodeID
,isnull(oq.Qty,0) as Qty,oq.BuyerDelivery,isnull(s.Description,'') as Description
from Orders o WITH (NOLOCK) 
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID and oq.Seq = '{this.masterData["OrderShipmodeSeq"]}'
left join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
where o.Id = '{this.masterData["OrderID"]}'",
                    out System.Data.DataTable dt);
                if (result && dt.Rows.Count != 0)
                {
                    strFactory = MyUtility.Convert.GetString(dt.Rows[0]["FactoryID"]);
                    strBrand = MyUtility.Convert.GetString(dt.Rows[0]["BrandID"]);
                    strCountryDestination = MyUtility.Convert.GetString(dt.Rows[0]["Dest"]);
                    strStyleNo = MyUtility.Convert.GetString(dt.Rows[0]["StyleID"]);
                    strDescription = MyUtility.Convert.GetString(dt.Rows[0]["Description"]);
                    numOrderQty = MyUtility.Convert.GetInt(dt.Rows[0]["Qty"]);
                }

                Color colorY_Mark = Color.FromArgb(249, 249, 164);

                worksheet.Cells[3, 3] = MyUtility.Convert.GetString(this.masterData["ID"]);
                worksheet.Cells[2, 6] = status;
                worksheet.Cells[2, 9] = System.Convert.ToDateTime(DateTime.Today).ToString("yyyy/MM/dd");
                worksheet.Cells[4, 3] = MyUtility.GetValue.Lookup($"select Format(BuyerDelivery, 'yyyy/MM/dd') from Order_QtyShip with (nolock) where ID = '{this.masterData["OrderID"]}' and Seq = '{this.masterData["OrderShipmodeSeq"]}'");
                worksheet.Cells[4, 8] = MyUtility.GetValue.Lookup(string.Format("select Name from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["MRHandle"])));
                worksheet.Cells[5, 3] = MyUtility.Convert.GetString(this.masterData["OrderID"]);
                worksheet.Cells[5, 7] = strFactory;
                worksheet.Cells[6, 3] = strBrand;
                worksheet.Cells[6, 5] = MyUtility.GetValue.Lookup(string.Format("select Alias from Country WITH (NOLOCK) where ID = '{0}'", strCountryDestination));
                worksheet.Cells[7, 3] = strStyleNo;
                worksheet.Cells[7, 5] = strDescription;
                worksheet.Cells[8, 3] = numOrderQty;
                worksheet.Cells[9, 3] = MyUtility.Convert.GetString(this.masterData["ShipQty"]);
                worksheet.Cells[9, 5] = MyUtility.Convert.GetString(this.masterData["Forwarder"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["Forwarder"])));
                worksheet.Cells[9, 7] = MyUtility.Convert.GetDecimal(this.masterData["Quotation"]);
                worksheet.Cells[9, 8] = MyUtility.Convert.GetString(this.masterData["EstAmount"]);
                worksheet.Cells[9, 9] = MyUtility.Convert.GetString(this.masterData["ActualAmountWVAT"]);
                worksheet.Cells[10, 3] = MyUtility.Convert.GetString(this.masterData["GW"]);
                worksheet.Cells[10, 5] = MyUtility.Convert.GetString(this.masterData["Forwarder1"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["Forwarder1"])));
                worksheet.Cells[10, 7] = MyUtility.Convert.GetDecimal(this.masterData["Quotation1"]);
                worksheet.Cells[11, 3] = MyUtility.Convert.GetString(this.masterData["VW"]);
                worksheet.Cells[11, 5] = MyUtility.Convert.GetString(this.masterData["Forwarder2"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["Forwarder2"])));
                worksheet.Cells[11, 7] = MyUtility.Convert.GetDecimal(this.masterData["Quotation2"]);
                worksheet.Cells[12, 3] = MyUtility.Convert.GetString(this.masterData["ReasonID"]) + "." + MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Air_Prepaid_Reason' and ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ReasonID"])));
                worksheet.Cells[13, 3] = MyUtility.Convert.GetString(this.masterData["ResponsibleFty"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                worksheet.Range["C13:I13"].Interior.Color = MyUtility.Convert.GetString(this.masterData["ResponsibleFty"]).ToUpper() == "TRUE" ? colorY_Mark : Color.White;
                worksheet.Range["C13:I13"].Font.Bold = MyUtility.Convert.GetString(this.masterData["ResponsibleFty"]).ToUpper() == "TRUE";
                worksheet.Cells[13, 5] = MyUtility.Convert.GetString(this.masterData["RatioFty"]) + "%";
                worksheet.Cells[13, 7] = MyUtility.Convert.GetString(this.masterData["ResponsibleFtyNo"]);
                worksheet.Cells[14, 3] = MyUtility.Convert.GetString(this.masterData["ResponsibleSubcon"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                worksheet.Range["C14:I14"].Interior.Color = MyUtility.Convert.GetString(this.masterData["ResponsibleSubcon"]).ToUpper() == "TRUE" ? colorY_Mark : Color.White;
                worksheet.Range["C14:I14"].Font.Bold = MyUtility.Convert.GetString(this.masterData["ResponsibleSubcon"]).ToUpper() == "TRUE";
                worksheet.Cells[14, 5] = MyUtility.Convert.GetString(this.masterData["RatioSubcon"]) + "%";
                worksheet.Cells[14, 7] = MyUtility.Convert.GetString(this.masterData["SubconDBCNo"]);
                worksheet.Cells[14, 9] = MyUtility.Convert.GetString(this.masterData["SubConName"]);
                worksheet.Cells[15, 3] = MyUtility.Convert.GetString(this.masterData["ResponsibleSCI"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                worksheet.Range["C15:I16"].Interior.Color = MyUtility.Convert.GetString(this.masterData["ResponsibleSCI"]).ToUpper() == "TRUE" ? colorY_Mark : Color.White;
                worksheet.Range["C15:I16"].Font.Bold = MyUtility.Convert.GetString(this.masterData["ResponsibleSCI"]).ToUpper() == "TRUE";
                worksheet.Cells[15, 5] = MyUtility.Convert.GetString(this.masterData["RatioSCI"]) + "%";
                worksheet.Cells[15, 7] = MyUtility.Convert.GetString(this.masterData["SCIICRNo"]);
                worksheet.Cells[15, 9] = MyUtility.Convert.GetString(this.masterData["SCIICRRemark"]);
                worksheet.Cells[16, 7] = MyUtility.Convert.GetString(this.masterData["SCIICRNo2"]);
                worksheet.Cells[16, 9] = MyUtility.Convert.GetString(this.masterData["SCIICRRemark2"]);
                worksheet.Cells[17, 3] = MyUtility.Convert.GetString(this.masterData["ResponsibleSupp"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                worksheet.Range["C17:I17"].Interior.Color = MyUtility.Convert.GetString(this.masterData["ResponsibleSupp"]).ToUpper() == "TRUE" ? colorY_Mark : Color.White;
                worksheet.Range["C17:I17"].Font.Bold = MyUtility.Convert.GetString(this.masterData["ResponsibleSupp"]).ToUpper() == "TRUE";
                worksheet.Cells[17, 5] = MyUtility.Convert.GetString(this.masterData["RatioSupp"]) + "%";
                worksheet.Cells[17, 7] = MyUtility.Convert.GetString(this.masterData["SuppDBCNo"]);
                worksheet.Cells[17, 9] = MyUtility.Convert.GetString(this.masterData["SuppDBCRemark"]);
                worksheet.Cells[18, 3] = MyUtility.Convert.GetString(this.masterData["ResponsibleBuyer"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                worksheet.Range["C18:I19"].Interior.Color = MyUtility.Convert.GetString(this.masterData["ResponsibleBuyer"]).ToUpper() == "TRUE" ? colorY_Mark : Color.White;
                worksheet.Range["C18:I19"].Font.Bold = MyUtility.Convert.GetString(this.masterData["ResponsibleBuyer"]).ToUpper() == "TRUE";
                worksheet.Cells[18, 5] = MyUtility.Convert.GetString(this.masterData["RatioBuyer"]) + "%";
                worksheet.Cells[18, 7] = MyUtility.Convert.GetString(this.masterData["BuyerDBCNo"]);
                worksheet.Cells[18, 9] = MyUtility.Convert.GetString(this.masterData["BuyerDBCRemark"]);
                worksheet.Cells[19, 7] = MyUtility.Convert.GetString(this.masterData["BuyerRemark"]);
                worksheet.Cells[20, 3] = MyUtility.Convert.GetString(this.masterData["FtyDesc"]);
                worksheet.Cells[21, 3] = MyUtility.Convert.GetString(this.masterData["MRComment"]);

                Microsoft.Office.Interop.Excel.Range screenshotRange = worksheet.Range["A1:J34"];
                screenshotRange.CopyPicture(Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen, Microsoft.Office.Interop.Excel.XlCopyPictureFormat.xlBitmap);

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P01_Request_Approval");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                #endregion
            }
            else if (this.reportType == "2")
            {
                #region Air pre-paid Payment Detial List

                if (this.dts[0].Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found");
                    return false;
                }

                this.SetCount(this.dts[0].Rows.Count);
                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P01_Print_PaymentDetail.xltx";
                Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                string strBlno1 = string.Empty;
                string strBlno2 = string.Empty;
                string sqlBlno = $@"
select BLNo,BL2No
from GMTBooking
where ID in (
	select INVNo
	from PackingList
	where ID in (
		select top 1 ID
		from PackingList_Detail
		where OrderID ='{this.masterData["OrderID"]}'
		and OrderShipmodeSeq ='{this.masterData["OrderShipmodeSeq"]}'
	)
)
";
                if (MyUtility.Check.Seek(sqlBlno, out DataRow drBlNo))
                {
                    strBlno1 = drBlNo["BLNo"].ToString();
                    strBlno2 = drBlNo["BL2No"].ToString();
                }

                worksheet.Cells[4, 2].Value = strBlno1;
                worksheet.Cells[5, 2].Value = strBlno2;

                int startRow01 = 9;
                this.InsertDataTableToExcel(worksheet, this.dts[0], startRow01, 1);

                // Excel SUM USD_AMOUNT
                worksheet.Cells[startRow01 + this.dts[0].Rows.Count, 6].Value = $"=SUM(F{startRow01}:F{startRow01 + this.dts[0].Rows.Count - 1}";

                // Excel PayCurreny
                worksheet.Cells[startRow01 + this.dts[0].Rows.Count, 7].Value = MyUtility.GetValue.Lookup("Select CurrencyID From System");

                // Excel SUM PHP_AMOUNT
                worksheet.Cells[startRow01 + this.dts[0].Rows.Count, 9].Value = $"=SUM(I{startRow01}:I{startRow01 + this.dts[0].Rows.Count - 1}";

                int startRow02 = 14;
                startRow02 += this.dts[0].Rows.Count;
                this.InsertDataTableToExcel(worksheet, this.dts[1], startRow02, 2);

                // Excel SUM QTY
                worksheet.Cells[startRow02 + this.dts[1].Rows.Count, 5].Value = $"=SUM(E{startRow02}:E{startRow02 + this.dts[1].Rows.Count - 1}";

                // Excel SUM G.W.
                worksheet.Cells[startRow02 + this.dts[1].Rows.Count, 6].Value = $"=SUM(F{startRow02}:F{startRow02 + this.dts[1].Rows.Count - 1}";

                // Excel SUM C.W
                worksheet.Cells[startRow02 + this.dts[1].Rows.Count, 7].Value = $"=SUM(G{startRow02}:G{startRow02 + this.dts[1].Rows.Count - 1}";

                // Excel SUM AIR FREIGHT (USD)
                worksheet.Cells[startRow02 + this.dts[1].Rows.Count, 8].Value = $"=SUM(H{startRow02}:H{startRow02 + this.dts[1].Rows.Count - 1}";

                // Excel SUM EQV
                worksheet.Cells[startRow02 + this.dts[1].Rows.Count, 9].Value = $"=SUM(I{startRow02}:I{startRow02 + this.dts[1].Rows.Count - 1}";

                // Excel G.TOTAL AIRFREIGHT (USD)
                worksheet.Cells[startRow02 + this.dts[1].Rows.Count, 10].Value = $"=H{startRow02 + this.dts[1].Rows.Count}-I{startRow02 + this.dts[1].Rows.Count - 1}";

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P01_Print_PaymentDetail");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Visible = true;
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                #endregion
                #endregion
            }

            return true;
        }

        private void P01_Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void InsertDataTableToExcel(Worksheet worksheet, System.Data.DataTable dataTable, int startRow, int type)
        {
            int cnt = startRow;

            // 將 DataTable 的每一行插入到 Excel
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    // 設定 Excel 單元格的值
                    worksheet.Cells[startRow + i, j + 1].Value = dataTable.Rows[i][j];
                }

                if (type == 2)
                {
                    worksheet.Cells[startRow + i, 10].Value = $"=H{startRow + i}-i{startRow + i}";
                }

                Excel.Range rngToInsert = worksheet.get_Range($"A{MyUtility.Convert.GetString(cnt + 1)}:A{MyUtility.Convert.GetString(cnt + 1)}", Type.Missing).EntireRow;
                rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                cnt++;
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                // 刪除多餘的行數
                Range rowRange = worksheet.Rows[startRow + dataTable.Rows.Count, Type.Missing];

                // 刪除整行
                rowRange.Delete(XlDeleteShiftDirection.xlShiftUp);

                // 畫框線
                Excel.Range rngBorders;
                string strRange = type == 1 ? "A{0}:I{1}" : "A{0}:j{1}";
                rngBorders = worksheet.get_Range(string.Format(strRange, MyUtility.Convert.GetString(startRow - 1), MyUtility.Convert.GetString(startRow + dataTable.Rows.Count)), Type.Missing);

                // 畫內框
                rngBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = 1;
                rngBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                rngBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = 1;
                rngBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;

                // 畫外框
                rngBorders.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
            }
        }
    }
}
