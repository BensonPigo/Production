using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_Packing
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable factory;
            DBProxy.Current.Select(null, "select '' union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.txtMdivision1.Text = Env.User.Keyword;

            this.dateScan1.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateScan2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateScan1.Text = DateTime.Now.ToString("yyyy/MM/dd 08:00");
            this.dateScan2.Text = DateTime.Now.ToString("yyyy/MM/dd 12:00");
        }

        // 驗證輸入條件
        private string _sp1;

        // 驗證輸入條件
        private string _sp2;

        // 驗證輸入條件
        private string _packingno1;

        // 驗證輸入條件
        private string _packingno2;

        // 驗證輸入條件
        private string _po1;

        // 驗證輸入條件
        private string _po2;

        // 驗證輸入條件
        private string _brand;

        // 驗證輸入條件
        private string _mDivision;

        // 驗證輸入條件
        private string _factory;

        // 驗證輸入條件
        private string _bdate1;

        // 驗證輸入條件
        private string _bdate2;

        // 驗證輸入條件
        private string _scandate1;
        private string _scandate1e;

        // 驗證輸入條件
        private string _scandate2;
        private string _scandate2e;

        // 驗證輸入條件
        private string _ScanName;
        private string _Barcode;

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this._sp1 = this.txtSPNoStart.Text;
            this._sp2 = this.txtSPNoEnd.Text;
            this._packingno1 = this.txtPackingStart.Text;
            this._packingno2 = this.txtPackingEnd.Text;
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

            if (!MyUtility.Check.Empty(this.dateScan1.Value))
            {
                this._scandate1 = Convert.ToDateTime(this.dateScan1.Value).ToString("yyyy/MM/dd HH:mm:ss");
                this._scandate1e = Convert.ToDateTime(this.dateScan1.Value).ToString("yyyy/MM/dd HH:mm");
            }
            else
            {
                this._scandate1 = null;
                this._scandate1e = null;
            }

            if (!MyUtility.Check.Empty(this.dateScan2.Value))
            {
                this._scandate2 = Convert.ToDateTime(this.dateScan2.Value).AddMinutes(1).ToString("yyyy/MM/dd HH:mm:ss");
                this._scandate2e = Convert.ToDateTime(this.dateScan2.Value).ToString("yyyy/MM/dd HH:mm");
            }
            else
            {
                this._scandate2 = null;
                this._scandate2e = null;
            }

            this._po1 = this.txtPONoStart.Text;
            this.Po2 = this.txtPONoEnd.Text;
            this._brand = this.txtbrand.Text;
            this._mDivision = this.txtMdivision1.Text;
            this._factory = this.comboFactory.Text;
            this._ScanName = this.txtuser1.TextBox1.Text;
            this._Barcode = this.txtBarcode.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        private DataTable _printData;
        private string _columnname;

        /// <summary>
        /// Po2
        /// </summary>
        public string Po2
        {
            get
            {
                return this._po2;
            }

            set
            {
                this._po2 = value;
            }
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd;
            string sqlcmdcolumnName;
            StringBuilder sqlwhere = new StringBuilder();

            #region 準備where條件, 兩段sql用相同條件
            if (!MyUtility.Check.Empty(this._sp1))
            {
                sqlwhere.Append(string.Format(" and pld.OrderID >= '{0}'", this._sp1));
            }

            if (!MyUtility.Check.Empty(this._sp2))
            {
                sqlwhere.Append(string.Format(" and pld.OrderID <= '{0}'", this._sp2));
            }

            if (!MyUtility.Check.Empty(this._packingno1))
            {
                sqlwhere.Append(string.Format(" and pld.id >= '{0}'", this._packingno1));
            }

            if (!MyUtility.Check.Empty(this._packingno2))
            {
                sqlwhere.Append(string.Format(" and pld.id <= '{0}'", this._packingno2));
            }

            if (!MyUtility.Check.Empty(this._bdate1))
            {
                sqlwhere.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", this._bdate1));
            }

            if (!MyUtility.Check.Empty(this._bdate2))
            {
                sqlwhere.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", this._bdate2));
            }

            if (!MyUtility.Check.Empty(this._scandate1))
            {
                sqlwhere.Append(string.Format(" and pld.ScanEditDate >= '{0}'", this._scandate1));
            }

            if (!MyUtility.Check.Empty(this._scandate2))
            {
                sqlwhere.Append(string.Format(" and pld.ScanEditDate <= '{0}'", this._scandate2));
            }

            if (!MyUtility.Check.Empty(this._po1))
            {
                sqlwhere.Append(string.Format(" and o.CustPONo >= '{0}'", this._po1));
            }

            if (!MyUtility.Check.Empty(this.Po2))
            {
                sqlwhere.Append(string.Format(" and o.CustPONo <= '{0}'", this.Po2));
            }

            if (!MyUtility.Check.Empty(this._brand))
            {
                sqlwhere.Append(string.Format(" and pl.brandid = '{0}'", this._brand));
            }

            if (!MyUtility.Check.Empty(this._mDivision))
            {
                sqlwhere.Append(string.Format(" and pl.MDivisionID = '{0}'", this._mDivision));
            }

            if (!MyUtility.Check.Empty(this._factory))
            {
                sqlwhere.Append(string.Format(" and pl.FactoryID = '{0}'", this._factory));
            }

            if (!MyUtility.Check.Empty(this._ScanName))
            {
                sqlwhere.Append(string.Format(" and pld.ScanName = '{0}'", this._ScanName));
            }

            if (this.rdbtnDetail.Checked)
            {
                sqlwhere.Append(" and (pld.ScanEditDate !='' or pld.ScanEditDate is not null) and pld.Lacking = 0");
            }
            else if (this.rdbtnSummary.Checked)
            {
                sqlwhere.Append(" and (pld.ScanEditDate ='' or pld.ScanEditDate is null or pld.Lacking = 1)");
            }

            if (!MyUtility.Check.Empty(this._Barcode))
            {
                sqlwhere.Append(string.Format(" and pld.Barcode = '{0}'", this._Barcode));
            }
            #endregion

            #region 先準備主要資料table
            sqlcmd = string.Format(
                @"
select 
	[Packing#] = pld.ID
	,[Factory] = pl.FactoryID
	,[Shipmode] = pl.ShipModeID
	,[SP#] = pld.OrderID
	,[Style] = o.StyleID
	,[Brand] = pl.BrandID
	,[Season] = o.SeasonID
    ,[Sewingline] = o.SewLine
	,o.Customize1
    ,oq.BuyerDelivery
	,[Destination] = concat(pl.Dest, ' - ', c.City)
	,[P.O.#] = o.CustPONo
	,[Buyer] = b.BuyerID
	,[CTN#] = pld.CTNStartNo
	,[CTN Barcode] = pl.ID + pld.CTNStartNo
	,[Qty] = pld.ShipQty
	,[Scan Date] = pld.ScanEditDate
    ,[Scan Name] = dbo.getPass1_ExtNo(pld.ScanName)
    ,[Actual CTN Weight] = pld.ActCTNWeight
	,[Lacking] = pld.Lacking
    ,PackingError = concat(pr.ID,'-' + pr.Description)
    ,pld.ErrQty
    ,pld.AuditQCName
INTO #TMP
from PackingList_Detail pld with (nolock)
inner join PackingList pl with (nolock) on pl.ID = pld.ID
inner join  Orders o with (nolock) on o.id = pld.OrderID
INNER JOIN Order_QtyShip oq with (nolock) ON pld.OrderID = oq.ID AND pld.OrderShipModeSeq = oq.Seq
left join Brand b with (nolock) on b.ID = pl.BrandID
left join CustCD c with (nolock) on c.ID = o.CustCDID and c.BrandID = o.BrandID and c.Junk != 1
left join PackingReason pr on pr.ID = pld.PackingReasonERID and pr.type = 'ER'
where 1=1 
{0}

SELECT [Packing#],[Factory],[Shipmode],[SP#],[Style],[Brand],[Season],[Sewingline],Customize1,[P.O.#],[Buyer],[BuyerDelivery],[Destination]
	,[Colorway] = c2.colorway
	,[Color] = c3.Color
	,[Size] = c4.Size
	,[CTN#]
    ,[CTN Barcode]
	,[PC/CTN] = c5.QtyPerCTN
	,[QTY] = SUM(t.Qty)
	,[PC/CTN Scanned] = c6.ScanQty
    ,PackingError
    ,ErrQty
    ,AuditQCName
    ,[Actual CTN Weight] = MAX([Actual CTN Weight])
	,[Ref. Barcode] = c7.Barcode
	,[Scan Date]
    ,[Scan Name]
	,[Carton Status] = case when ([Scan Date] !='' or  [Scan Date] is not null) and Lacking = 0
					   then 'Complete' 
					   else 'Not Complete' end
	,[Lacking] = iif(lacking=1,'Y','N')
	,[Lacking Qty] = isnull( LackingQty.Qty,0)    
FROM #TMP T
outer apply(
	select colorway = stuff((
		select concat('/',pld2.Article)
		from PackingList_Detail pld2 with (nolock)
		where pld2.id = t.[Packing#] and pld2.OrderID = t.[SP#] and pld2.CTNStartNo = t.[CTN#]
		order by pld2.id,pld2.OrderID,pld2.CTNStartNo
		for xml path('')
	),1,1,'')
)c2
outer apply(
	select Color = stuff((
		select concat('/',pld2.Color )
		from PackingList_Detail pld2 with (nolock)
		where pld2.id = t.[Packing#] and pld2.OrderID = t.[SP#] and pld2.CTNStartNo = t.[CTN#]
		order by pld2.id,pld2.OrderID,pld2.CTNStartNo
		for xml path('')
	),1,1,'')
)c3
outer apply(
	select Size = stuff((
		select concat('/',pld2.SizeCode )
		from PackingList_Detail pld2 with (nolock)
		where pld2.id = t.[Packing#] and pld2.OrderID = t.[SP#] and pld2.CTNStartNo = t.[CTN#]
		order by pld2.id,pld2.OrderID,pld2.CTNStartNo
		for xml path('')
	),1,1,'')
)c4
outer apply(
	select QtyPerCTN = stuff((
		select concat('/',pld2.QtyPerCTN)
		from PackingList_Detail pld2 with (nolock)
		where pld2.id = t.[Packing#] and pld2.OrderID = t.[SP#] and pld2.CTNStartNo = t.[CTN#]
		order by pld2.id,pld2.OrderID,pld2.CTNStartNo
		for xml path('')
	),1,1,'')
)c5
outer apply(
	select ScanQty = stuff((
		select concat('/',pld2.ScanQty)
		from PackingList_Detail pld2 with (nolock)
		where pld2.id = t.[Packing#] and pld2.OrderID = t.[SP#] and pld2.CTNStartNo = t.[CTN#]
		order by pld2.id,pld2.OrderID,pld2.CTNStartNo 
		for xml path('')
	),1,1,'')
)c6
outer apply(
	select Barcode = stuff((
		select concat('/',pld2.Barcode)
		from PackingList_Detail pld2 with (nolock)
		where pld2.id = t.[Packing#] and pld2.OrderID = t.[SP#] and pld2.CTNStartNo = t.[CTN#]
		order by pld2.id,pld2.OrderID,pld2.CTNStartNo
		for xml path('')
	),1,1,'')
)c7
outer apply(
	select [Qty] = sum(QtyPerCTN) - sum(ScanQty) 
	from PackingList_Detail pld 
	where pld.ID=t.Packing# and pld.OrderID=t.SP# and pld.CTNStartNo=t.CTN#
	and pld.Lacking=1
)LackingQty
group by [Packing#]	,[Factory]	,[Shipmode]	,[SP#]	,[Style]	,[Brand]	,[Season], [Sewingline]	,Customize1	,[P.O.#]	,[Buyer]	,[Destination]
	,[CTN#],[CTN Barcode]	,[Scan Date]	,c2.colorway	,c3.Color	,c4.Size	,c5.QtyPerCTN	,c6.ScanQty	,c7.Barcode,[Scan Name] ,Lacking,LackingQty.Qty
    ,[BuyerDelivery]
    ,PackingError
    ,ErrQty
    ,AuditQCName
order by ROW_NUMBER() OVER(ORDER BY [Packing#],[SP#], RIGHT(REPLICATE('0', 3) + CAST([CTN#] as NVARCHAR), 3))
DROP TABLE #TMP
", sqlwhere.ToString());
            #endregion

            #region 準備動態的(欄位名稱)
            sqlcmdcolumnName = string.Format(
                @"
select Customize1 = stuff((
	select concat('/',x.Customize1)
	from
	(
		select distinct b.Customize1
		from PackingList_Detail pld with (nolock)
		inner join PackingList pl with (nolock) on pl.ID = pld.ID
        INNER JOIN Order_QtyShip oq with (nolock) ON pld.OrderID = oq.ID AND pld.OrderShipModeSeq = oq.Seq
		inner join  Orders o with (nolock) on o.id = pld.OrderID
		left join Brand b with (nolock) on b.ID = pl.BrandID
		where 1=1 
        {0}
	)x
	for xml path('')
),1,1,'')
", sqlwhere.ToString());
            #endregion

            #region Get Data
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out this._printData)))
            {
                return result;
            }

            try
            {
                this._columnname = MyUtility.GetValue.Lookup(sqlcmdcolumnName);
            }
            catch (Exception ex)
            {
                return Ict.Result.F("Get column name failed!!\r\n", ex);
            }
            #endregion

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
            if (this._printData == null || this._printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion

            this.SetCount(this._printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string reportname = "Packing_R01.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + reportname);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[2, 2] = this._sp1 + "~" + this._sp2;
            worksheet.Cells[2, 5] = this._packingno1 + "~" + this._packingno2;
            worksheet.Cells[2, 8] = this._bdate1 + "~" + this._bdate2;
            worksheet.Cells[2, 11] = this._scandate1e + "~" + this._scandate2e;
            worksheet.Cells[2, 16] = this._po1 + "~" + this.Po2;
            worksheet.Cells[2, 18] = this._brand;
            worksheet.Cells[2, 21] = this._factory;
            worksheet.Cells[2, 24] = this.rdbtnDetail.Checked ? "Complete" : (this.rdbtnSummary.Checked ? "Not Complete" : "ALL");
            worksheet.Cells[3, 9] = this._columnname;
            MyUtility.Excel.CopyToXls(this._printData, string.Empty, reportname, 3, showExcel: false, excelApp: objApp);
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Packing_R01");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
