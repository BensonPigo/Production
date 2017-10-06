using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            DataTable factory;
            DBProxy.Current.Select(null, "select '' union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;

        }

        // 驗證輸入條件
        string _sp1, _sp2, _packingno1, _packingno2, _po1, _po2, _brand, _mDivision, _factory, _bdate1, _bdate2, _scandate1, _scandate2;
        protected override bool ValidateInput()
        {
            _sp1 = txtSPNoStart.Text;
            _sp2 = txtSPNoEnd.Text;
            _packingno1 = txtPackingStart.Text;
            _packingno2 = txtPackingEnd.Text;
            if (!MyUtility.Check.Empty(dateBuyerDelivery.Value1))
                _bdate1 = Convert.ToDateTime(dateBuyerDelivery.Value1).ToString("d");
            else _bdate1 = null;
            if (!MyUtility.Check.Empty(dateBuyerDelivery.Value2))
                _bdate2 = Convert.ToDateTime(dateBuyerDelivery.Value2).ToString("d");
            else _bdate2 = null;
            if (!MyUtility.Check.Empty(dateSacnDate.Value1))
                _scandate1 = Convert.ToDateTime(dateSacnDate.Value1).ToString("d");
            else _scandate1 = null;
            if (!MyUtility.Check.Empty(dateSacnDate.Value2))
                _scandate2 = Convert.ToDateTime(dateSacnDate.Value2).ToString("d") + " 23:59:59";
            else _scandate2 = null;
            _po1 = txtPONoStart.Text;
            _po2 = txtPONoEnd.Text;
            _brand = txtbrand.Text;
            _mDivision = txtMdivision1.Text;
            _factory = comboFactory.Text;

            return base.ValidateInput();
        }

         // 非同步取資料
        DataTable _printData;
        string _columnname;
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd;
            string sqlcmdcolumnName;
            StringBuilder sqlwhere = new StringBuilder();

            #region 準備where條件, 兩段sql用相同條件
            if (!MyUtility.Check.Empty(_sp1))
                sqlwhere.Append(string.Format(" and pld.OrderID >= '{0}'", _sp1));

            if (!MyUtility.Check.Empty(_sp2))
                sqlwhere.Append(string.Format(" and pld.OrderID <= '{0}'",_sp2));

            if (!MyUtility.Check.Empty(_packingno1))
                sqlwhere.Append(string.Format(" and pld.id >= '{0}'",_packingno1));

            if (!MyUtility.Check.Empty(_packingno2))
                sqlwhere.Append(string.Format(" and pld.id <= '{0}'",_packingno2));

            if (!MyUtility.Check.Empty(_bdate1))
                sqlwhere.Append(string.Format(" and o.BuyerDelivery >= '{0}'",_bdate1));

            if (!MyUtility.Check.Empty(_bdate2))
                sqlwhere.Append(string.Format(" and o.BuyerDelivery <= '{0}'",_bdate2));

            if (!MyUtility.Check.Empty(_scandate1))
                sqlwhere.Append(string.Format(" and pld.ScanEditDate >= '{0}'",_scandate1));

            if (!MyUtility.Check.Empty(_scandate2))
                sqlwhere.Append(string.Format(" and pld.ScanEditDate <= '{0}'",_scandate2));

            if (!MyUtility.Check.Empty(_po1))
                sqlwhere.Append(string.Format(" and o.CustPONo >= '{0}'",_po1));

            if (!MyUtility.Check.Empty(_po2))
                sqlwhere.Append(string.Format(" and o.CustPONo <= '{0}'",_po2));

            if (!MyUtility.Check.Empty(_brand))
                sqlwhere.Append(string.Format(" and pl.brandid = '{0}'",_brand));

            if (!MyUtility.Check.Empty(_mDivision))
                sqlwhere.Append(string.Format(" and pl.MDivisionID = '{0}'",_mDivision));

            if (!MyUtility.Check.Empty(_factory))
                sqlwhere.Append(string.Format(" and pl.FactoryID = '{0}'",_factory));

            if (rdbtnDetail.Checked)
                sqlwhere.Append(" and (pld.ScanEditDate !='' or pld.ScanEditDate is not null)");
            else if (rdbtnSummary.Checked)
                sqlwhere.Append(" and (pld.ScanEditDate ='' or pld.ScanEditDate is null)");
            #endregion

            #region 先準備主要資料table
            sqlcmd = string.Format(@"
select 
	[Packing#] = pld.ID
	,[Factory] = pl.FactoryID
	,[Shipmode] = pl.ShipModeID
	,[SP#] = pld.OrderID
	,[Style] = o.StyleID
	,[Brand] = pl.BrandID
	,[Season] = o.SeasonID
	,o.Customize1
	,[Destination] = concat(pl.Dest, ' - ', c.City)
	,[P.O.#] = o.CustPONo
	,[Buyer] = b.BuyerID
	,[CTN#] = pld.CTNStartNo
	,[Qty] = pld.ShipQty
	,[Scan Date] = format(pld.ScanEditDate,'yyyy/MM/dd')
INTO #TMP
from PackingList_Detail pld with (nolock)
inner join PackingList pl with (nolock) on pl.ID = pld.ID
inner join  Orders o with (nolock) on o.id = pld.OrderID
left join Brand b with (nolock) on b.ID = pl.BrandID
left join CustCD c with (nolock) on c.ID = o.CustCDID and c.BrandID = o.BrandID and c.Junk != 1
where 1=1 
{0}

SELECT [Packing#],[Factory],[Shipmode],[SP#],[Style],[Brand],[Season],Customize1,[P.O.#],[Buyer],[Destination]
	,[Colorway] = c2.colorway
	,[Color] = c3.Color
	,[Size] = c4.Size
	,[CTN#]
	,[PC/CTN] = c5.QtyPerCTN
	,[QTY] = SUM(t.Qty)
	,[PC/CTN Scanned] = c6.ScanQty
	,[Ref. Barcode] = c7.Barcode
	,[Scan Date]
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
group by [Packing#]	,[Factory]	,[Shipmode]	,[SP#]	,[Style]	,[Brand]	,[Season]	,Customize1	,[P.O.#]	,[Buyer]	,[Destination]
	,[CTN#]	,[Scan Date]	,c2.colorway	,c3.Color	,c4.Size	,c5.QtyPerCTN	,c6.ScanQty	,c7.Barcode
order by ROW_NUMBER() OVER(ORDER BY [Packing#],[SP#], RIGHT(REPLICATE('0', 3) + CAST([CTN#] as NVARCHAR), 3))
DROP TABLE #TMP
", sqlwhere.ToString());
            #endregion

            #region 準備動態的(欄位名稱)
            sqlcmdcolumnName = string.Format(@"
select Customize1 = stuff((
	select concat('/',x.Customize1)
	from
	(
		select distinct b.Customize1
		from PackingList_Detail pld with (nolock)
		inner join PackingList pl with (nolock) on pl.ID = pld.ID
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
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out _printData)))
            {
                return result;
            }

            try
            {
                _columnname = MyUtility.GetValue.Lookup(sqlcmdcolumnName);
            }
            catch (Exception ex)
            {
                return Result.F("Get column name failed!!\r\n", ex);
            }
            #endregion
            
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (_printData == null || _printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion

            this.SetCount(_printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string reportname = "Packing_R01.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + reportname);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[2, 2] = _sp1 + "~" + _sp2;
            worksheet.Cells[2, 5] = _packingno1 + "~" + _packingno2;
            worksheet.Cells[2, 8] = _bdate1 + "~" + _bdate2;
            worksheet.Cells[2, 11] = _scandate1 + "~" + _scandate2;
            worksheet.Cells[2, 14] = _po1 + "~" + _po2;
            worksheet.Cells[2, 16] = _brand;
            worksheet.Cells[2, 18] = _factory;
            worksheet.Cells[2, 20] = rdbtnDetail.Checked ? "Complete" : (rdbtnSummary.Checked ? "Not Complete" : "ALL");
            worksheet.Cells[3, 8] = _columnname;
            MyUtility.Excel.CopyToXls(_printData, "", reportname, 3, showExcel: false, excelApp: objApp);
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Packing_R01");
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
