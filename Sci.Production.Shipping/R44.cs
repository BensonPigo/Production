using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R44
    /// </summary>
    public partial class R44 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;

        /// <summary>
        /// R44
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R44(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(
                @"
select
	e.ID
	,e.Consignee
	,e.InvNo
	,Loading=concat(e.ImportPort,'-',e.ImportCountry)
	,e.ShipModeID
	,e.Blno
	,e.Vessel
	,e.Eta
	,e.PackingArrival
	,e.PortArrival
	,e.WhseArrival
	,e.DocArrival
from Export e with(nolock)
outer apply(select top 1 v.ID,v.DeclareNo from VNImportDeclaration v with(nolock) where v.BLNo = e.Blno and v.IsFtyExport = 0 and isnull(DeclareNo,'')<>'')a
outer apply(select top 1 v.ID,v.DeclareNo from VNImportDeclaration v with(nolock) where v.WKNo = e.ID and v.IsFtyExport = 0and isnull(DeclareNo,'')<>'')b
outer apply(select ID=isnull(iif(isnull(e.Blno,'')<>'',a.id,b.id),''),DeclareNo= isnull(iif(isnull(e.Blno,'')<>'',a.DeclareNo,b.DeclareNo),''))c
where e.NonDeclare = 0
and (isnull(c.ID,'') ='' or isnull(c.DeclareNo,'') = '')

union all
select
	e.ID
	,e.Consignee
	,e.InvNo
	,Loading=concat(e.ImportPort,'-',e.ImportCountry)
	,e.ShipModeID
	,e.Blno
	,e.Vessel
	,e.PortArrival
	,e.DocArrival
	,e.PortArrival
	,e.WhseArrival
	,e.DocArrival
from FtyExport e with(nolock)
outer apply(select top 1 v.ID,v.DeclareNo from VNImportDeclaration v with(nolock) where v.BLNo = e.Blno and v.IsFtyExport = 1 and isnull(DeclareNo,'')<>'')a
outer apply(select top 1 v.ID,v.DeclareNo from VNImportDeclaration v with(nolock) where v.WKNo = e.ID and v.IsFtyExport = 1 and isnull(DeclareNo,'')<>'')b
outer apply(select ID=isnull(iif(isnull(e.Blno,'')<>'',a.id,b.id),''),DeclareNo= isnull(iif(isnull(e.Blno,'')<>'',a.DeclareNo,b.DeclareNo),''))c
where e.NonDeclare = 0
and (isnull(c.ID,'') ='' or isnull(c.DeclareNo,'') = '')

");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                return new DualResult(false, "Query data fail\r\n" + result.ToString());
            }

            return Result.True;
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

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R44_NonDeclarationReportImport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "Shipping_R44_NonDeclarationReportImport.xltx", headerRow: 1, excelApp: excel, showSaveMsg: false, wSheet: worksheet);
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
