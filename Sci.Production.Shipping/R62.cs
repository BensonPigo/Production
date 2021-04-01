using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R62 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DualResult result;
        private DateTime? dateDecDate1;
        private DateTime? dateDecDate2;
        private DateTime? dateETD1;
        private DateTime? dateETD2;
        private string strDecNo1;
        private string strDecNo2;
        private string strInvNo1;
        private string strInvNo2;
        private bool ex;

        /// <inheritdoc/>
        public R62(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.dateDecDate1 = this.dateDecDate.Value1;
            this.dateDecDate2 = this.dateDecDate.Value2;
            this.dateETD1 = this.dateETD.Value1;
            this.dateETD2 = this.dateETD.Value2;
            this.strDecNo1 = this.txtDecNo1.Text;
            this.strDecNo2 = this.txtDecNo2.Text;
            this.strInvNo1 = this.txtInvNo1.Text;
            this.strInvNo2 = this.txtInvNo2.Text;
            this.ex = this.chkEx.Checked;

            if (MyUtility.Check.Empty(this.dateDecDate1) && MyUtility.Check.Empty(this.dateDecDate2) &&
                MyUtility.Check.Empty(this.dateETD1) && MyUtility.Check.Empty(this.dateETD1))
            {
                MyUtility.Msg.WarningBox("Please input <Declaration Date> or <ETD> first!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string where = " left join ";
            if (this.ex)
            {
                where = " inner join ";
            }

            string sqlcmd = $@"
select [Shipper] = g.Shipper 
     , [Buyer] = b.BuyerID 
     , [FTY] = o.ftygroup 
     , [Status] = kd.status 
     , [Inv#] = g.id 
     , [PO] = o.CustPONo
	 , [SP] = o.ID
     , [Style] = o.StyleID  
	 , [Product Type] = case
			when OL.Location = 'T' then 'TOP' 
			when OL.Location = 'B' then 'BOTTOM' 
			when OL.Location = 'I' then 'INNER'   
			when OL.Location = 'O' then 'OUTER'
			else '' end
     , [Qty(By SP)] = isnull(kdd.ShipModeSeqQty, pld.ShipQty)
     , [CTN(By SP)] = isnull(kdd.CTNQty, pld.CTNQty)
     , [FOB] = isnull(kdd.POPrice * isnull(OL.rate, 1) / 100, r.FOB)
	 , [Ttl FOB] = isnull(kdd.ShipModeSeqQty * kdd.POPrice * isnull(OL.rate, 1) / 100,  r.FOB * pld.ShipQty)
	 , [Act Ttl FOB] = isnull(kdd.ActTtlPOPrice * isnull(OL.rate, 1) / 100,  r.FOB * pld.ShipQty)
	 , [DiffTtlFOB] = isnull(kdd.ActTtlPOPrice * isnull(OL.rate, 1) / 100,  r.FOB * pld.ShipQty) - isnull(kdd.ShipModeSeqQty * kdd.POPrice * isnull(OL.rate, 1) / 100,  r.FOB * pld.ShipQty)
     , [Local Inv#] = kdd.LocalINVNO 
     , [Description] = kdd.Description 
     , [HS Code] = kdd.HSCode 
     , [CO Form Type] = kdd.COFormType 
     , [CO#] = kdd.COID 
     , [CO Date] = kdd.CODate 
     , [Declaration#] = kd.DeclareNo 
     , [Declaration Date] = kd.CDate 
     , [ETD] = g.ETD 
     , [Customer CD] = g.CustCDID 
     , [Shipmode] = g.ShipModeID 
     , [Forwarder] = g.Forwarder 
     , [Port of loading] = kd.ExportPort 
	 , [Dest] = g.Dest
	 , [Continent] = c.Continent+'-'+c.NameEN
     , [Export without declaration] = (case when g.NonDeclare = 1 then 'Y' else 'N' end) 
	 , [NW] = isnull(kdd.NetKg, (case when g.ShipModeID in ('A/C','A/P') then pld.NW else (pld.NW + (pld.NW * 0.05)) end)) * isnull(OL.rate, 1) / 100
	 , [GW] = isnull(kdd.WeightKg, (case when g.ShipModeID in ('A/C','A/P') then pld.GW else (pld.GW + ( pld.GW * 0.05)) end)) * isnull(OL.rate, 1) / 100
	 , [Act NW] = isnull(kdd.ActNetKg, (case when g.ShipModeID in ('A/C','A/P') then pld.NW else (pld.NW + ( pld.NW * 0.05))end)) * isnull(OL.rate, 1) / 100
	 , [Act GW] = isnull(kdd.ActWeightKg, (case when g.ShipModeID in ('A/C','A/P') then pld.GW else (pld.GW + ( pld.GW * 0.05))end)) * isnull(OL.rate, 1) / 100
	 , [Diff NW] = (isnull(kdd.NetKg, (case when g.ShipModeID in ('A/C','A/P') then pld.NW else (pld.NW + (pld.NW * 0.05)) end)) - 
					isnull(kdd.ActNetKg, (case when g.ShipModeID in ('A/C','A/P') then pld.NW else (pld.NW + ( pld.NW * 0.05))end)))* isnull(OL.rate, 1) / 100
	 , [Diff GW] = (isnull(kdd.WeightKg, (case when g.ShipModeID in ('A/C','A/P') then pld.GW else (pld.GW + ( pld.GW * 0.05)) end)) - 
					isnull(kdd.ActWeightKg, (case when g.ShipModeID in ('A/C','A/P') then pld.GW else (pld.GW + ( pld.GW * 0.05))end)))* isnull(OL.rate, 1) / 100
from GMTBooking g
{where} KHExportDeclaration_Detail kdd on kdd.Invno=g.id
{where} KHExportDeclaration kd on kd.id=kdd.id 
left join PackingList pl on pl.INVNo = g.ID
outer apply(
	select
		pld.OrderID,
		ShipQty = sum(pld.ShipQty),
		CTNQty = sum(pld.CTNQty),
		NW = sum(pld.NW),
		GW = sum(pld.GW)
	from PackingList_Detail pld
	where pld.ID = pl.ID
	group by pld.OrderID
)pld
left join Orders o on o.ID = pld.OrderID
left join Order_Location OL on OL.OrderID = pld.OrderID and ol.Location  = kdd.Location
left join Brand b on b.ID = o.BrandID
left join Country c on c.ID = kd.Dest
outer apply(
	select OrderID,AvgPrice = sum(ShipQty*POPrice)/sum(ShipQty)
	from (
		select ShipQty = sum(pd2.ShipQty),pd2.OrderID,oup.SizeCode,oup.Article ,oup.POPrice
		from PackingList_Detail pd2
		inner join PackingList p1 on pd2.ID = p1.ID
		inner join Order_UnitPrice oup on oup.Id= pd2.OrderID
		and oup.Article = pd2.Article and oup.SizeCode = pd2.SizeCode
		where INVNo = g.ID
		group by pd2.OrderID,oup.SizeCode,oup.Article,oup.POPrice
	) a
	where OrderID = o.ID
	group by OrderID
)POPrice
outer apply(select FOB = isnull(PoPrice.AvgPrice,o.PoPrice) * isnull(OL.rate, 1) / 100)r
where 1=1
";
            #region where
            if (!MyUtility.Check.Empty(this.dateDecDate1) && !MyUtility.Check.Empty(this.dateDecDate2))
            {
                sqlcmd += $@" and kd.Cdate between '{((DateTime)this.dateDecDate1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateDecDate2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dateETD1) && !MyUtility.Check.Empty(this.dateETD2))
            {
                sqlcmd += $@"   and g.ETD between '{((DateTime)this.dateETD1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateETD2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strDecNo1))
            {
                sqlcmd += $@" and kd.DeclareNo >= '{this.strDecNo1}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strDecNo2))
            {
                sqlcmd += $@" and kd.DeclareNo <= '{this.strDecNo2}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strInvNo1))
            {
                sqlcmd += $@" and g.id >= '{this.strInvNo1}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strInvNo2))
            {
                sqlcmd += $@" and g.id <= '{this.strInvNo2}'" + Environment.NewLine;
            }

            sqlcmd += @" order by kd.CDate, kd.DeclareNo, g.id  ";

            #endregion

            if (!(this.result = DBProxy.Current.Select(null, sqlcmd, out this.printData)))
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + this.result.ToString());
                return failResult;
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
            string reportName = "Shipping_R62.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R62");
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
    }
}
