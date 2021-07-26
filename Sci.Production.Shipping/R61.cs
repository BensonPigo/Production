using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R61 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DualResult result;
        private DateTime? dateDecDate1;
        private DateTime? dateDecDate2;
        private DateTime? dateETA1;
        private DateTime? dateETA2;
        private DateTime? ArrWHDate1;
        private DateTime? ArrWHDate2;
        private string strDecNo1;
        private string strDecNo2;
        private string strShipMode;
        private string reporttype;

        /// <inheritdoc/>
        public R61(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.cbmReporttype.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.dateDecDate1 = this.dateDecDate.Value1;
            this.dateDecDate2 = this.dateDecDate.Value2;
            this.dateETA1 = this.dateETA.Value1;
            this.dateETA2 = this.dateETA.Value2;
            this.ArrWHDate1 = this.dateArrivWHDate.Value1;
            this.ArrWHDate2 = this.dateArrivWHDate.Value2;
            this.strDecNo1 = this.txtDecNo1.Text;
            this.strDecNo2 = this.txtDecNo2.Text;
            this.strShipMode = this.comboshipmode.Text;
            this.reporttype = this.cbmReporttype.Text;

            if (MyUtility.Check.Empty(this.dateDecDate1) && MyUtility.Check.Empty(this.dateDecDate2) &&
                MyUtility.Check.Empty(this.dateETA1) && MyUtility.Check.Empty(this.dateETA2) &&
                MyUtility.Check.Empty(this.ArrWHDate1) && MyUtility.Check.Empty(this.ArrWHDate2))
            {
                MyUtility.Msg.WarningBox("Please input <Declaration Date>, <ETA> or <Arrived WH Date> first!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd;
            if (this.reporttype == "Detail")
            {
                sqlcmd = $@"
select [WK#] = kid.ExportID
     , [Declaration Date] = ki.cdate
     , [Declaration#] = ki.DeclareNo
     , [Local Shipment] = (case when IsLocalShipment.value > 0 then 'Y' else 'N' end)
     , [Shipmode] = kid.ShipModeID
     , [ETA] = kid.ETA 
     , [Arrived WH Date] = kid.WhseArrival 
     , [B/L#] = kid.BLNo 
     , [FTY] = kid.FactoryID
     , [Consignee] = kid.Consignee 
     , [Ref#] = kid.RefNo 
     , [Description] = Kid.Description 
     , [Q'ty] = Kid.Qty 
     , [Unit] = Kid.UnitID 
     , [NW] = Kid.NetKg 
     , [GW] = Kid.WeightKg 
     , [Type] = kcdp.CustomsType 
     , kcdp.CDCCode
     , [Customs Description] = kcdp.CDCName
     , [CDC Qty] = Kid.Qty*kcdp2.Ratio
     , [CDC Unit] = kid.CDCUnit 
     , [CDC Unit Price] = kid.CDCUnitPrice 
     , [CDC Amount] = CDCAmount.value
     , [Act.NW] = kid.ActNetKg 
     , [Act.GW] = kid.ActWeightKg 
	 , [Act.Amount] = kid.ActAmount 
     , [HS Code] = kcd.HSCode 
     , [Act.HS Code] = kid.ActHSCode 
  from KHImportDeclaration ki
 inner join KHImportDeclaration_Detail kid on ki.id=kid.id
 inner join KHCustomsItem kc on kc.Refno=Kid.Refno and kc.ukey= kid.KHCustomsItemUkey
 inner join KHCustomsItem_Detail kcd on kc.ukey=kcd.KHCustomsItemUkey and kcd.Port=Ki.ImportPort
 inner join KHCustomsDescription kcdp on kc.KHCustomsDescriptionCDCName = kcdp.CDCName
 inner join KHCustomsDescription_Detail kcdp2 on kcdp2.CDCName = kcdp.CDCName and kcdp2.PurchaseUnit = kid.UnitId
 outer apply (
	select value = count(*) from FtyExport fe where fe.id=kid.exportid
 )IsLocalShipment
 outer apply(
	select value = sum(t.Qty * t.CDCUnitPrice) * kcdp2.Ratio 
	from KHImportDeclaration_Detail t
	where t.Ukey = kid.ukey
 )CDCAmount
 where 1=1
";

                #region where
                if (!MyUtility.Check.Empty(this.dateDecDate1) && !MyUtility.Check.Empty(this.dateDecDate2))
                {
                    sqlcmd += $@" and ki.cdate between '{((DateTime)this.dateDecDate1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateDecDate2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.dateETA1) && !MyUtility.Check.Empty(this.dateETA2))
                {
                    sqlcmd += $@" and kid.eta between '{((DateTime)this.dateETA1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateETA2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.ArrWHDate1) && !MyUtility.Check.Empty(this.ArrWHDate2))
                {
                    sqlcmd += $@" and kid.WhseArrival between '{((DateTime)this.ArrWHDate1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.ArrWHDate2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.strShipMode))
                {
                    sqlcmd += $@" and kid.ShipmodeID = '{this.strShipMode}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.strDecNo1))
                {
                    sqlcmd += $@" and ki.DeclareNo >= '{this.strDecNo1}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.strDecNo2))
                {
                    sqlcmd += $@" and ki.DeclareNo <= '{this.strDecNo2}'" + Environment.NewLine;
                }

                sqlcmd += @" order by ki.cdate, ki.DeclareNo, kid.ExportID";
                #endregion
            }
            else
            {
                sqlcmd = $@"
select kc.CustomsType as [Customs Type]
	, kc.CDCName as [Customs Description]
	, kc.CDCCode as [CDC Code]
	, kc.CDCUnit as [CDC Unit]
	, ks.OriTtlNetKg as [Ori Ttl N.W.]
	, ks.OriTtlWeightKg as [Ori Ttl G.W.]
	, ks.OriTtlCDCAmount as [Ori Ttl CDC Amount]
	, ks.ActTtlNetKg as [Act. Ttl N.W.]
	, ks.ActTtlWeightKg as [Act. Ttl G.W.]
	, ks.ActTtlAmount as [Act. Ttl Amount]
from KHImportDeclaration_ShareCDCExpense ks
inner join KHImportDeclaration ki on ki.id = ks.id
inner join KHCustomsDescription kc on ks.KHCustomsDescriptionCDCName=kc.CDCName
where 1=1
and kc.CustomsType in ('Fabric','Accessory','Machine')
";

                #region where
                if (!MyUtility.Check.Empty(this.dateDecDate1) && !MyUtility.Check.Empty(this.dateDecDate2))
                {
                    sqlcmd += $@" and ki.cdate between '{((DateTime)this.dateDecDate1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateDecDate2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.dateETA1) && !MyUtility.Check.Empty(this.dateETA2))
                {
                    sqlcmd += $@" and exists(select 1 from KHImportDeclaration_Detail kid where ki.id=kid.id and kid.eta between '{((DateTime)this.dateETA1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateETA2).ToString("yyyy/MM/dd")}' )" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.ArrWHDate1) && !MyUtility.Check.Empty(this.ArrWHDate2))
                {
                    sqlcmd += $@" and exists(select 1 from KHImportDeclaration_Detail kid where ki.id=kid.id and kid.WhseArrival between '{((DateTime)this.ArrWHDate1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.ArrWHDate2).ToString("yyyy/MM/dd")}')" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.strShipMode))
                {
                    sqlcmd += $@" and exists(select 1 from KHImportDeclaration_Detail kid where ki.id=kid.id and kid.ShipmodeID = '{this.strShipMode}')" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.strDecNo1))
                {
                    sqlcmd += $@" and ki.DeclareNo >= '{this.strDecNo1}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.strDecNo2))
                {
                    sqlcmd += $@" and ki.DeclareNo <= '{this.strDecNo2}'" + Environment.NewLine;
                }

                sqlcmd += @" order by ki.cdate, ki.DeclareNo";
                #endregion
            }

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
            string reportName = "Shipping_R61.xltx";
            if (this.reporttype != "Detail")
            {
                reportName = "Shipping_R61_SummaryByCustomsDescription.xltx";
            }

            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R61");
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
