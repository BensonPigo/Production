using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_Packing
    /// </summary>
    public partial class R03 : Win.Tems.PrintForm
    {
        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable factory;
            DBProxy.Current.Select(null, "select '' union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        private string _sp1;
        private string _sp2;
        private string _packingno1;
        private string _po1;
        private string _po2;
        private string _brand;
        private string _mDivision;
        private string _factory;
        private string _bdate1;
        private string _bdate2;
        private DataTable _printData;

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this._packingno1 = this.txtPackingStart.Text;
            this._sp1 = this.txtSPNoStart.Text;
            this._sp2 = this.txtSPNoEnd.Text;
            this._po1 = this.txtPONoStart.Text;
            this._po2 = this.txtPONoEnd.Text;

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this._bdate1 = Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("d");
            }
            else
            {
                this._bdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this._bdate2 = Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("d");
            }
            else
            {
                this._bdate2 = null;
            }

            this._brand = this.txtbrand.Text;
            this._mDivision = this.txtMdivision1.Text;
            this._factory = this.comboFactory.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this._packingno1))
            {
                where += $" and pl.id = '{this._packingno1}' ";
            }

            if (!MyUtility.Check.Empty(this._sp1))
            {
                where += $" and  o.id>= '{this._sp1}' ";
            }

            if (!MyUtility.Check.Empty(this._sp2))
            {
                where += $" and  o.id <= '{this._sp2}' ";
            }

            if (!MyUtility.Check.Empty(this._po1))
            {
                where += $" and o.CustPONo >= '{this._po1}' ";
            }

            if (!MyUtility.Check.Empty(this._po2))
            {
                where += $" and  o.CustPONo <= '{this._po2}' ";
            }

            if (!MyUtility.Check.Empty(this._bdate1))
            {
                where += $" and  o.BuyerDelivery >= '{this._bdate1}' ";
            }

            if (!MyUtility.Check.Empty(this._bdate2))
            {
                where += $" and  o.BuyerDelivery <= '{this._bdate2}' ";
            }

            if (!MyUtility.Check.Empty(this._brand))
            {
                where += $" and  pl.BrandID= '{this._brand}' ";
            }

            if (!MyUtility.Check.Empty(this._mDivision))
            {
                where += $" and pl.MDivisionID = '{this._mDivision}' ";
            }

            if (!MyUtility.Check.Empty(this._factory))
            {
                where += $" and pl.FactoryID = '{this._factory}' ";
            }

            if (this.chkFOC.Checked)
            {
                where += $@" and o.FOC = 0";
            }

            if (this.ChkLocalOrder.Checked)
            {
                where += $@" and o.LocalOrder = 0";
            }
            #endregion
            string sqlcmd = $@"
select pl.MDivisionID,pl.FactoryID,pl.ID,o.SciDelivery,o.BuyerDelivery,o.CustPONo,o.ID,
	Junk=iif(o.Junk=1,'Y','N'),
	Dest=(select Alias from Country ct where ct.id=pl.Dest),
	o.StyleID,o.BrandID,pl.CustCDID,o.SewLine,pl.ShipModeID,pl.INVNo,pl.PulloutDate,o.SewInLine,o.SewOffLine,
	pl.Status,o.Qty,
	TtlCTNS=sum(pld.CTNQty),
	TtlQty=sum(pld.ShipQty),
	TtlNw=sum(pld.NW),
	TtlGW=sum(pld.GW),
	TtlCBM=sum(cbm.CBM)*sum(pld.CTNQty),
	PurchaseCTN= iif(LocalPo.RequestID is null ,'N','Y'),
	ClogCFMStatus=iif(count(pld.ID) = count(pld.ReceiveDate), 'Y','N'),
	pl.EstCTNBooking,
	pl.EstCTNArrive,
	pl.Remark
from PackingList_Detail pld
inner join PackingList pl on pl.ID = pld.ID
inner join  Orders o on o.id = pld.OrderID
outer apply(
	select RequestID from LocalPO_Detail ld with(nolock) 
	inner join LocalPO l with(nolock) on l.id = ld.Id
	where RequestID=pl.ID and l.status = 'Approved'
)LocalPo
outer apply(select CBM from LocalItem where Refno = pld.Refno) cbm
where 1=1 and pld.DisposeFromClog= 0
{where}
group by pl.MDivisionID,pl.FactoryID,pl.ID,o.SciDelivery,o.BuyerDelivery,o.CustPONo,o.ID,
	iif(o.Junk=1,'Y','N'),pl.Dest,
	o.StyleID,o.BrandID,pl.CustCDID,o.SewLine,pl.ShipModeID,pl.INVNo,pl.PulloutDate,o.SewInLine,o.SewOffLine,
	pl.Status,o.Qty,iif(isnull(pl.LocalPOID,'')='','N','Y'),
	pl.EstCTNBooking,
	pl.EstCTNArrive,
	pl.Remark,
	LocalPo.RequestID
order by pl.MDivisionID,pl.FactoryID,pl.ID,o.ID
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this._printData);
            if (!result)
            {
                return Result.F(result.ToString());
            }

            return Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this._printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(this._printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string excelName = "Packing_R03";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            MyUtility.Excel.CopyToXls(this._printData, string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
            worksheet.Columns.AutoFit();

            #endregion
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
