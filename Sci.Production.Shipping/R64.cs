using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R64 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private string sqlcmd;

        /// <inheritdoc/>
        public R64(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateETA.HasValue && !this.dateArrivedWHDate.HasValue)
            {
                MyUtility.Msg.WarningBox("Please input <ETA> or <Arrived WH Date> first!");
                return false;
            }

            string whereExport = string.Empty;
            string whereFtyExport = string.Empty;
            if (this.dateETA.HasValue)
            {
                whereExport += $"and e.Eta between '{(DateTime)this.dateETA.Value1:yyyy/MM/dd}' and '{(DateTime)this.dateETA.Value2:yyyy/MM/dd}'";
                whereFtyExport += $"and fe.Eta between '{(DateTime)this.dateETA.Value1:yyyy/MM/dd}' and '{(DateTime)this.dateETA.Value2:yyyy/MM/dd}'";
            }

            if (this.dateArrivedWHDate.HasValue)
            {
                whereExport += $"and e.WhseArrival between '{(DateTime)this.dateETA.Value1:yyyy/MM/dd}' and '{(DateTime)this.dateETA.Value2:yyyy/MM/dd}'";
                whereFtyExport += $"and fe.WhseArrival between '{(DateTime)this.dateETA.Value1:yyyy/MM/dd}' and '{(DateTime)this.dateETA.Value2:yyyy/MM/dd}'";
            }

            this.sqlcmd = $@"
;with tmpExport as (
	select e.Consignee,e.FactoryID,e.Eta,po3.SCIRefno,ed.UnitId,e.ID
	,Qty = sum(ed.Qty)
	,NW = sum(ed.NetKg)
	,GW = sum(ed.WeightKg)
	from Export e 
	inner join Export_Detail ed on e.id=ed.ID  
	inner join PO_Supp_Detail po3 on po3.ID = ed.PoID and ed.Seq1 = po3.SEQ1 and ed.Seq2 = po3.SEQ2
	where 1=1 
	and ed.PoType <> 'M' 
	and e.NonDeclare=0
    {whereExport}
	group by e.Consignee,e.FactoryID,e.Eta,po3.SCIRefno,ed.UnitId,e.ID
), tmpExportTypeM as (
	select
		e.Consignee,
		e.FactoryID,
		e.Eta,
		scirefno.SCIRefno,
		ed.UnitId,e.ID,
		NW = sum(ed.NetKg),
		GW = sum(ed.WeightKg),
		Qty = sum(ed.Qty)
	from Export e 
	inner join Export_Detail ed on e.id=ed.ID
	outer apply(
		select SCIRefno =
			case ed.FabricType
			when 'M' then (select mpd.MasterGroupID from SciMachine_MachinePO_Detail mpd where (mpd.id=ed.PoID or mpd.TPEPOID=ed.poid) and mpd.seq1=ed.seq1 and mpd.seq2=ed.seq2)
			when 'P' then (select top 1 ppd.PartID from SciMachine_PartPO_Detail ppd where (ppd.id=ed.PoID or ppd.TPEPOID=ed.poid) and ppd.seq1=ed.seq1 and ppd.seq2=ed.seq2)
			when 'O' then (select mspd.MiscID from SciMachine_MiscPO_Detail mspd where (mspd.id=ed.PoID or mspd.TPEPOID=ed.poid) and mspd.seq1=ed.seq1 and mspd.seq2=ed.seq2)
			when 'R' then (select mopd.MiscOtherID from SciMachine_MiscOtherPO_Detail mopd where mopd.id=ed.PoID and mopd.seq1=ed.seq1 and mopd.seq2=ed.seq2)
			else ed.refno
			end
	) scirefno

	where 1=1 
	and ed.PoType in ('M') and ed.FabricType in ('M','P','O','R')
	and e.NonDeclare=0
    {whereExport}
	group by e.Consignee,e.FactoryID,e.Eta,scirefno.SCIRefno,ed.UnitId,e.ID
)
,tmpFtyExport as (
	select fe.Consignee,f2.SCIRefno,fed.UnitId,fe.id
	,Qty = sum(fed.Qty)
	,NW = sum(fed.NetKg)
	,GW = sum(fed.WeightKg)
	from FtyExport fe 
    inner join FtyExport_Detail fed on fed.id=fe.ID 
    inner join Fabric f2 on f2.SCIRefno =fed.SCIRefno 
	where 1=1 
	and fe.NonDeclare=0
	and fe.type in ('1','2','4') 
    {whereFtyExport}
	group by fe.Consignee,f2.SCIRefno,fed.UnitId,fe.ID
) 

select distinct [ExportID] = e.ID                                                                   
             , [ShipModeID] = e.ShipModeID
             , [ETA] = e.Eta
             , [WhseArrival] = e.WhseArrival
             , [BlNo] = e.Blno
             , [FactoryID] = e.FactoryID
             , [Consignee] = e.Consignee
             , [RefNo] = kc.Refno
             , [Description] = kc.Description 
             , [QTY] = s.Qty
             , [UnitID] = s.UnitId
             , [NetKg] = s.NW 
             , [WeightKg] = s.GW
             , [CustomsType] = kd.CustomsType
             , kd.CDCCode
             , [CustomsDescription] = kd.CDCName
             , [CDCQty] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)
             , [CDCUnit] = kd.CDCUnit
             , [CDCUnitPrice] = kc.CDCUnitPrice
             , [CDCAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice
             , [ActNetKg] = s.NW
             , [ActWeightKg] = s.GW
             , [ActAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice
             , [HSCode] = kcd.HSCode
             , [ActHSCode] = kcd.HSCode
from tmpExport s
inner join Export e on s.ID = e.ID
inner join KHCustomsItem kc on kc.RefNo = s.SCIRefno
left  join KHCustomsItem_Detail kcd on kc.Ukey=kcd.KHCustomsItemUkey and kcd.Port=e.ImportPort          
left  join KHCustomsDescription kd on kd.CDCName = kc.KHCustomsDescriptionCDCName
    and kd.CustomsType in ('Fabric', 'Accessory', 'Machine')
left  join KHCustomsDescription_Detail kdd on kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = s.UnitId   
Left join KHImportDeclaration_Detail khd on khd.ExportID = e.ID
Left join KHImportDeclaration kh on kh.id = khd.id
where kh.DeclareNo is null

union all

select distinct [ExportID] = e.ID
             , [ShipModeID] = e.ShipModeID
             , [ETA] = e.Eta
             , [WhseArrival] = e.WhseArrival
             , [BlNo] = e.Blno
             , [FactoryID] = e.FactoryID
             , [Consignee] = e.Consignee
             , [RefNo] = kc.Refno
             , [Description] = kc.Description
             , [QTY] = s.Qty
             , [UnitID] = s.UnitId
             , [NetKg] = s.NW
             , [WeightKg] = s.GW
             , [CustomsType] = kd.CustomsType
             , kd.CDCCode
             , [CustomsDescription] = kd.CDCName
             , [CDCQty] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)
             , [CDCUnit] = kd.CDCUnit
             , [CDCUnitPrice] = kc.CDCUnitPrice
             , [CDCAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice
             , [ActNetKg] = s.NW
             , [ActWeightKg] = s.GW
             , [ActAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice
             , [HSCode] = kcd.HSCode
             , [ActHSCode] = kcd.HSCode
from tmpExportTypeM s
inner join Export e on s.ID = e.ID
inner join KHCustomsItem kc on kc.RefNo = s.SCIRefno
left  join KHCustomsItem_Detail kcd on kc.Ukey=kcd.KHCustomsItemUkey and kcd.Port=e.ImportPort          
left  join KHCustomsDescription kd on kd.CDCName = kc.KHCustomsDescriptionCDCName
    and kd.CustomsType in ('Fabric', 'Accessory', 'Machine')
left join KHCustomsDescription_Detail kdd on kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = s.UnitId   
Left join KHImportDeclaration_Detail khd on khd.ExportID = e.ID
Left join KHImportDeclaration kh on kh.id = khd.id
where kh.DeclareNo is null

union all

select [ExportID] = FE.ID
             , [ShipModeID] = fe.ShipModeID
             , [ETA] = null
             , [WhseArrival] = fe.WhseArrival
             , [BlNo] = fe.Blno
             , [FactoryID] = ''
             , [Consignee] = FE.Consignee
             , [RefNo] = kc.Refno
             , [Description] = kc.Description
             , [QTY] = s.Qty
             , [UnitID] = s.UnitID
             , [NetKg] = s.NW
             , [WeightKg] = s.GW
             , [CustomsType] = kd.CustomsType
             , kd.CDCCode
             , [CustomsDescription] = kd.CDCName
             , [CDCQty] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)
             , [CDCUnit] = kd.CDCUnit
             , [CDCUnitPrice] = kc.CDCUnitPrice
             , [CDCAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice
             , [ActNetKg] = s.NW
             , [ActWeightKg] = s.GW
             , [ActAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice
             , [HSCode] = kcd.HSCode
             , [ActHSCode] = kcd.HSCode
from tmpFtyExport s
inner join FtyExport fe on s.ID = fe.ID  
inner join KHCustomsItem kc on kc.RefNo = s.SCIRefno
left join KHCustomsItem_Detail kcd on kc.Ukey=kcd.KHCustomsItemUkey and kcd.Port=fe.ImportPort        
left join KHCustomsDescription kd on kd.CDCName = kc.KHCustomsDescriptionCDCName
    and kd.CustomsType in ('Fabric', 'Accessory')
left join KHCustomsDescription_Detail kdd on kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = s.UnitId 
Left join KHImportDeclaration_Detail khd on khd.ExportID = FE.ID
Left join KHImportDeclaration kh on kh.id = khd.id
where kh.DeclareNo is null
";
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd, out this.printData);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Starting EXCEL...");
            string reportName = "Shipping_R64.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R64");
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
