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
        private bool bolReportTypeSummary;
        private string sp1;
        private string sp2;
        private string packingno1;
        private string po1;
        private string po2;
        private string brand;
        private string mDivision;
        private string factory;
        private string bdate1;
        private string bdate2;
        private DataTable _printData;

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DBProxy.Current.Select(null, "select '' union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out DataTable dtfactory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, dtfactory);
            this.comboFactory.Text = Env.User.Factory;
            this.txtMdivision1.Text = Env.User.Keyword;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.bolReportTypeSummary = this.radioSummary.Checked;
            this.packingno1 = this.txtPackingStart.Text;
            this.sp1 = this.txtSPNoStart.Text;
            this.sp2 = this.txtSPNoEnd.Text;
            this.po1 = this.txtPONoStart.Text;
            this.po2 = this.txtPONoEnd.Text;

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this.bdate1 = Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("d");
            }
            else
            {
                this.bdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this.bdate2 = Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("d");
            }
            else
            {
                this.bdate2 = null;
            }

            this.brand = this.txtbrand.Text;
            this.mDivision = this.txtMdivision1.Text;
            this.factory = this.comboFactory.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;

            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.packingno1))
            {
                where += $" and pl.id = '{this.packingno1}' ";
            }

            if (!MyUtility.Check.Empty(this.sp1))
            {
                where += $" and  PackingListDetail_Sum.id>= '{this.sp1}' ";
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                where += $" and  PackingListDetail_Sum.id <= '{this.sp2}' ";
            }

            if (!MyUtility.Check.Empty(this.po1))
            {
                where += $" and PackingListDetail_Sum.CustPONo >= '{this.po1}' ";
            }

            if (!MyUtility.Check.Empty(this.po2))
            {
                where += $" and  PackingListDetail_Sum.CustPONo <= '{this.po2}' ";
            }

            if (!MyUtility.Check.Empty(this.bdate1))
            {
                where += $" and  PackingListDetail_Sum.BuyerDelivery >= '{this.bdate1}' ";
            }

            if (!MyUtility.Check.Empty(this.bdate2))
            {
                where += $" and  PackingListDetail_Sum.BuyerDelivery <= '{this.bdate2}' ";
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                where += $" and  pl.BrandID= '{this.brand}' ";
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                where += $" and pl.MDivisionID = '{this.mDivision}' ";
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                where += $" and pl.FactoryID = '{this.factory}' ";
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

            if (this.bolReportTypeSummary)
            {
                sqlcmd = $@"
select pl.MDivisionID
	, pl.FactoryID
	, pl.ID
	, PackingListDetail_Sum.SciDelivery
	, PackingListDetail_Sum.BuyerDelivery
	, PackingListDetail_Sum.CustPONo
	, PackingListDetail_Sum.ID
	, Junk=iif(PackingListDetail_Sum.Junk = 1, 'Y', 'N')
	, Dest=(select Alias from Country ct where ct.id=pl.Dest)
	, PackingListDetail_Sum.StyleID
	, PackingListDetail_Sum.BrandID
	, pl.CustCDID
	, PackingListDetail_Sum.SewLine
	, pl.ShipModeID
	, pl.INVNo
	, pl.PulloutDate
	, PackingListDetail_Sum.SewInLine
	, PackingListDetail_Sum.SewOffLine
	, pl.Status
	, PackingListDetail_Sum.Qty
	, TtlCTNS = PackingListDetail_Sum.TtlCTNS
	, TtlQty = PackingListDetail_Sum.TtlQty
	, TtlNw = PackingListDetail_Sum.TtlNw
	, TtlGW = PackingListDetail_Sum.TtlGW
	, TtlCBM = (pl.CBM * PackingListDetail_Sum.TtlCTNS)
	, PurchaseCTN = iif(exists(select 1 from LocalPO_Detail ld with(nolock) inner join LocalPO l with(nolock) on l.id = ld.Id where ld.RequestID=pl.ID and l.status = 'Approved') ,'Y','N')
	, ClogCFMStatus = iif(PackingListDetail_Sum.CtnID = PackingListDetail_Sum.CtnRecDate, 'Y','N')
	, pl.EstCTNBooking
	, pl.EstCTNArrive
	, pl.Remark
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
	where pd.ID = pl.ID
	and pd.DisposeFromClog = 0 
	group by o.SciDelivery, o.BuyerDelivery, o.CustPONo, o.ID
		,o.Junk, o.StyleID, o.BrandID, o.SewLine
		,o.SewInLine, o.SewOffLine, o.Qty, pd.DisposeFromClog
		,o.FOC, o.LocalOrder
)PackingListDetail_Sum
where PackingListDetail_Sum.DisposeFromClog= 0
{where}

order by pl.MDivisionID,pl.FactoryID,pl.ID,PackingListDetail_Sum.ID
";
            }
            else
            {
                sqlcmd = $@"
select pl.MDivisionID
	, pl.FactoryID
	, pl.ID
	, PackingListDetail_Sum.SciDelivery
	, PackingListDetail_Sum.BuyerDelivery
	, PackingListDetail_Sum.CustPONo
	, PackingListDetail_Sum.ID
	, Junk=iif(PackingListDetail_Sum.Junk = 1, 'Y', 'N')
	, Dest=(select Alias from Country ct where ct.id=pl.Dest)
	, PackingListDetail_Sum.StyleID
	, PackingListDetail_Sum.BrandID
	, pl.CustCDID
	, PackingListDetail_Sum.SewLine
	, pl.ShipModeID
	, pl.INVNo
	, pl.PulloutDate
	, PackingListDetail_Sum.SewInLine
	, PackingListDetail_Sum.SewOffLine
	, pl.Status
	, PackingListDetail_Sum.Qty
	, TtlCTNS = PackingListDetail_Sum.TtlCTNS
	, TtlQty = PackingListDetail_Sum.TtlQty
	, TtlNw = PackingListDetail_Sum.TtlNw
	, TtlGW = PackingListDetail_Sum.TtlGW
	, TtlCBM = (pl.CBM * PackingListDetail_Sum.TtlCTNS)
	, PurchaseCTN = iif(exists(select 1 from LocalPO_Detail ld with(nolock) inner join LocalPO l with(nolock) on l.id = ld.Id where ld.RequestID=pl.ID and l.status = 'Approved') ,'Y','N')
	, ClogCFMStatus = iif(PackingListDetail_Sum.CtnID = PackingListDetail_Sum.CtnRecDate, 'Y','N')
	, pl.EstCTNBooking
	, pl.EstCTNArrive
	, pl.Remark
	, PackingListDetail_Sum.Description
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
		,d.Description
	from PackingList_Detail pd with(nolock) 
	inner join Orders o on o.ID = pd.OrderID
	outer apply(
		select Description =
		REPLACE(
			REPLACE(
				(select stuff((
					select n=Description
					from(
						select distinct l.Description -- Copy Packing P08
						from LocalItem l WITH (NOLOCK)
						inner join PackingList_Detail pd2  WITH (NOLOCK) on pd2.RefNo = l.RefNo
						where pd.Ukey = pd2.Ukey
					)d  order by Description
					for xml path('')
				),1,3,''))
			,'</n>','')
		,'<n>',CHAR(13)+char(10))
	)d
	where pd.ID = pl.ID
	and pd.DisposeFromClog = 0 
	group by o.SciDelivery, o.BuyerDelivery, o.CustPONo, o.ID
		,o.Junk, o.StyleID, o.BrandID, o.SewLine
		,o.SewInLine, o.SewOffLine, o.Qty, pd.DisposeFromClog
		,o.FOC, o.LocalOrder, d.Description
)PackingListDetail_Sum
where PackingListDetail_Sum.DisposeFromClog= 0
{where}

order by pl.MDivisionID,pl.FactoryID,pl.ID,PackingListDetail_Sum.ID
";
            }

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
            string excelName = this.bolReportTypeSummary ? "Packing_R03" : "Packing_R03Dimension";
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
