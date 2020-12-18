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
            this.comboFactory.Text = Env.User.Factory;
            this.txtMdivision1.Text = Env.User.Keyword;
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
                where += $" and  PackingListDetail_Sum.id>= '{this._sp1}' ";
            }

            if (!MyUtility.Check.Empty(this._sp2))
            {
                where += $" and  PackingListDetail_Sum.id <= '{this._sp2}' ";
            }

            if (!MyUtility.Check.Empty(this._po1))
            {
                where += $" and PackingListDetail_Sum.CustPONo >= '{this._po1}' ";
            }

            if (!MyUtility.Check.Empty(this._po2))
            {
                where += $" and  PackingListDetail_Sum.CustPONo <= '{this._po2}' ";
            }

            if (!MyUtility.Check.Empty(this._bdate1))
            {
                where += $" and  PackingListDetail_Sum.BuyerDelivery >= '{this._bdate1}' ";
            }

            if (!MyUtility.Check.Empty(this._bdate2))
            {
                where += $" and  PackingListDetail_Sum.BuyerDelivery <= '{this._bdate2}' ";
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
                where += $@" and PackingListDetail_Sum.FOC = 0";
            }

            if (this.ChkLocalOrder.Checked)
            {
                where += $@" and PackingListDetail_Sum.LocalOrder = 0";
            }
            #endregion
            string sqlcmd = $@"
select  pl.MDivisionID,pl.FactoryID,pl.ID,PackingListDetail_Sum.SciDelivery,PackingListDetail_Sum.BuyerDelivery,PackingListDetail_Sum.CustPONo,PackingListDetail_Sum.ID,
	Junk=iif(PackingListDetail_Sum.Junk=1,'Y','N'),
	Dest=(select Alias from Country ct where ct.id=pl.Dest),
	PackingListDetail_Sum.StyleID,PackingListDetail_Sum.BrandID,pl.CustCDID,PackingListDetail_Sum.SewLine,pl.ShipModeID,pl.INVNo,pl.PulloutDate,PackingListDetail_Sum.SewInLine,PackingListDetail_Sum.SewOffLine,
	pl.Status,PackingListDetail_Sum.Qty,
	TtlCTNS= PackingListDetail_Sum.TtlCTNS,
	TtlQty=PackingListDetail_Sum.TtlQty,
	TtlNw=PackingListDetail_Sum.TtlNw,
	TtlGW=PackingListDetail_Sum.TtlGW,
	TtlCBM=(pl.CBM*PackingListDetail_Sum.TtlCTNS),
	PurchaseCTN= iif(exists(select 1 from LocalPO_Detail ld with(nolock) inner join LocalPO l with(nolock) on l.id = ld.Id where ld.RequestID=pl.ID and l.status = 'Approved') ,'Y','N'),
	ClogCFMStatus=iif(PackingListDetail_Sum.CtnID = PackingListDetail_Sum.CtnRecDate, 'Y','N'),
	pl.EstCTNBooking,
	pl.EstCTNArrive,
	pl.Remark
from PackingList pl 
outer apply(
	select [TtlCTNS]=sum(pd.CTNQty) 
	    ,[TtlQty]=SUM(pd.ShipQty) 
	    ,[TtlNw]=sum(pd.NW)
	    ,[TtlGW]=sum(pd.GW)
	    ,[CtnID]=count(pd.ID)
	    ,[CtnRecDate]=count(pd.ReceiveDate)
	    ,o.SciDelivery,o.BuyerDelivery,o.CustPONo,o.ID
	    ,o.Junk,o.StyleID,o.BrandID,o.SewLine,o.SewInLine,o.SewOffLine
	    ,o.Qty
	    ,pd.DisposeFromClog
        ,o.FOC,o.LocalOrder
	from PackingList_Detail pd with(nolock) 
	inner join Orders o on o.ID = pd.OrderID
	where pd.ID=pl.ID
	and pd.DisposeFromClog = 0 
	group by o.SciDelivery,o.BuyerDelivery,o.CustPONo,o.ID
	,o.Junk,o.StyleID,o.BrandID,o.SewLine,o.SewInLine,o.SewOffLine
	,o.Qty
	,pd.DisposeFromClog
	,o.FOC,o.LocalOrder
)PackingListDetail_Sum
where 1=1 
and PackingListDetail_Sum.DisposeFromClog= 0
{where}

order by pl.MDivisionID,pl.FactoryID,pl.ID,PackingListDetail_Sum.ID
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this._printData);
            if (!result)
            {
                return Ict.Result.F(result.ToString());
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
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
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
