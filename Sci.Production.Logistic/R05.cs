using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Packing_Packing
    /// </summary>
    public partial class R05 : Win.Tems.PrintForm
    {
        /// <summary>
        /// R05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
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
                this._bdate1 = Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            }
            else
            {
                this._bdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this._bdate2 = Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd");
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
            this._po2 = this.txtPONoEnd.Text;
            this._brand = this.txtbrand.Text;
            this._mDivision = this.txtMdivision1.Text;
            this._factory = this.comboFactory.Text;
            this._ScanName = this.txtuser1.TextBox1.Text;
            this._Barcode = this.txtBarcode.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        private DataTable printData;

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd;
            StringBuilder sqlwhere = new StringBuilder();

            #region 準備where條件, 兩段sql用相同條件
            if (!MyUtility.Check.Empty(this._sp1))
            {
                sqlwhere.Append(string.Format(" and cp.OrderID >= '{0}'", this._sp1));
            }

            if (!MyUtility.Check.Empty(this._sp2))
            {
                sqlwhere.Append(string.Format(" and cp.OrderID <= '{0}'", this._sp2));
            }

            if (!MyUtility.Check.Empty(this._packingno1))
            {
                sqlwhere.Append(string.Format(" and c.PackingListID >= '{0}'", this._packingno1));
            }

            if (!MyUtility.Check.Empty(this._packingno2))
            {
                sqlwhere.Append(string.Format(" and c.PackingListID <= '{0}'", this._packingno2));
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
                sqlwhere.Append(string.Format(" and c.AddDate >= '{0}'", this._scandate1));
            }

            if (!MyUtility.Check.Empty(this._scandate2))
            {
                sqlwhere.Append(string.Format(" and c.AddDate <= '{0}'", this._scandate2));
            }

            if (!MyUtility.Check.Empty(this._po1))
            {
                sqlwhere.Append(string.Format(" and o.CustPONo >= '{0}'", this._po1));
            }

            if (!MyUtility.Check.Empty(this._po2))
            {
                sqlwhere.Append(string.Format(" and o.CustPONo <= '{0}'", this._po2));
            }

            if (!MyUtility.Check.Empty(this._brand))
            {
                sqlwhere.Append(string.Format(" and p.BrandID = '{0}'", this._brand));
            }

            if (!MyUtility.Check.Empty(this._mDivision))
            {
                sqlwhere.Append(string.Format(" and p.MDivisionID = '{0}'", this._mDivision));
            }

            if (!MyUtility.Check.Empty(this._factory))
            {
                sqlwhere.Append(string.Format(" and p.FactoryID = '{0}'", this._factory));
            }

            if (!MyUtility.Check.Empty(this._ScanName))
            {
                sqlwhere.Append(string.Format(" and c.AddName = '{0}'", this._ScanName));
            }

            if (!MyUtility.Check.Empty(this._Barcode))
            {
                sqlwhere.Append(string.Format(" and pd.Barcode = '{0}'", this._Barcode));
            }
            #endregion

            // 看要顯示Summaary or Detail
            string sqlShow = string.Empty;
            if (this.rdbSummary.Checked)
            {
                sqlShow = "select * from #tmpFinal where [SCICtnNoNo] = 1";
            }
            else
            {
                sqlShow = "select * from #tmpFinal";
            }

            #region 先準備主要資料table
            sqlcmd = string.Format(
                @"
select distinct
 c.PackingListID
,p.FactoryID
,p.ShipModeID
,cp.OrderID
,o.StyleID
,p.BrandID
,o.SeasonID
,[Dest] = concat(p.Dest,' - ',cd.City)
,oq.BuyerDelivery
,cp.Article
,pd.Color
,cp.SizeCode
,c.CTNStartNo
,pd.SCICtnNo
,QtyPerCTN
,[Qty] = pd.ShipQty
,cp.ScanQty
,pd.Barcode
,[ScanDate] = c.AddDate
,[ScanName] = concat(c.AddName,'-',Pass1.Name)
,[Lacking] = iif(c.LackingQty > 0,'Y','')
,cp.LackingQty
,o.POID
,c.Ukey
,pd.ActCTNWeight
,pd.ClogActCTNWeight
,[OrderNo] = dense_rank() over (partition by c.PackingListID order by o.AddDate)
,[OrderListNo] = dense_rank() over (partition by c.PackingListID, o.poid order by o.ID)
into #tmp
from ClogScanPack c with(nolock)
left join ClogScanPack_Detail cp with(nolock) on cp.ClogScanPackUkey = c.Ukey
left join PackingList p with(nolock) on p.id = c.PackingListID
inner join Packinglist_Detail pd with(nolock) on pd.id = p.id
	and pd.CTNStartNo =  c.CTNStartNo and pd.orderid = cp.orderid and cp.Article = pd.Article and cp.SizeCode=pd.SizeCode
left join orders o with(nolock) on o.id = cp.OrderID
left join Order_QtyShip oq with (nolock) ON pd.OrderID = oq.ID AND pd.OrderShipModeSeq = oq.Seq
left join Pass1 with(nolock) on pass1.id = c.AddName
left join CustCD cd with(nolock) on cd.ID = o.custcdID and cd.BrandID = o.BrandID
where 1=1 
{0}

select t.PackingListID
,t.FactoryID
,t.ShipModeID
,[OrderList] = OrderList.Value
,StyleID = StyleSeason.StyleID
,t.BrandID
,SeasonID = StyleSeason.SeasonID
,t.[Dest]
,t.[BuyerDelivery]
,[ColorWay] = ColorWay.Value
,[Color] = Color.Value
,[Size] = Size.Value
,t.CTNStartNo
,[CTNBarcode] = t.SCICtnNo
,[PC_Ctn] = QtyPerCTN.Value
,Qty = sum(t.Qty)
,[ScanQty] = ScannedQty.Value
,[Barcode] = Barcode.Value
,t.[ScanDate]
,t.[ScanName]
,t.[Lacking]
,[LackingQty] = LackingQty.Value
,t.ActCTNWeight
,t.ClogActCTNWeight
,[SCICtnNoNo] = Row_Number() over (partition by t.SCICtnNo order by t.[ScanDate])
into #tmpFinal
from #tmp t
outer apply(
	select value =
	stuff((
		select concat('/',
			case when OrderListNo > 1
			then　substring(tmp.OrderID,11,len(tmp.OrderID)-10)
			else tmp.OrderID end		
		) 
		from (
			select distinct s.orderID ,OrderListNo
			from #tmp s with(nolock)			
			where s.PackingListID = t.PackingListID
			and s.CTNStartNo = t.CTNStartNo
			and s.Ukey = t.Ukey
		) tmp 
		order by orderID,OrderListNo
		for xml path('')
		)
	,1,1,''
	)
)OrderList
outer apply(
	select top 1 s.StyleID ,s.SeasonID 
	from #tmp s
	where [OrderNo] = 1
	and s.PackingListID = t.PackingListID
	and s.Ukey = t.Ukey
)StyleSeason
outer apply(
	select value =stuff(
		(select concat('/',tmp.Article) 
			from (
				select distinct s.Article 
				from #tmp s with(nolock)
				where s.PackingListID = t.PackingListID
				and s.CTNStartNo = t.CTNStartNo
				and s.Ukey = t.Ukey
			) tmp 
			order by Article
			for xml path('')
		)
	,1,1,'')
)ColorWay
outer apply(
	select value =stuff(
		(select concat('/',tmp.Color) 
			from (
				select distinct s.Color 
				from #tmp s with(nolock)
				where s.PackingListID = t.PackingListID
				and s.CTNStartNo = t.CTNStartNo
				and s.Ukey = t.Ukey
			) tmp 
			order by Color
			for xml path('')
		)
	,1,1,'')
)Color
outer apply(
	select value =stuff(
		(select concat('/',tmp.SizeCode) 
			from (
				select distinct s.SizeCode ,Seq = isnull(os.Seq,'')
				from #tmp s with(nolock)
				left join Order_SizeCode os with(nolock) on os.id = s.POID and os.SizeCode = s.SizeCode
				where s.PackingListID = t.PackingListID
				and s.CTNStartNo = t.CTNStartNo
				and s.Ukey = t.Ukey
			) tmp 
			order by Seq
			for xml path('')
		)
	,1,1,'')
)Size
outer apply(
	select value =stuff(
		(select concat('/',tmp.QtyPerCTN) 
			from (
				select s.QtyPerCTN ,Seq = isnull(os.Seq,'')
				from #tmp s with(nolock)
				left join Order_SizeCode os with(nolock) on os.id = s.POID and os.SizeCode = s.SizeCode
				where s.PackingListID = t.PackingListID
				and s.CTNStartNo = t.CTNStartNo
				and s.Ukey = t.Ukey
			) tmp
			order by Seq
			for xml path('')
		)
	,1,1,'')
)QtyPerCTN
outer apply(
	select value =stuff(
		(select concat('/',convert(varchar(100),tmp.ScanQty)) 
			from (
				select s.ScanQty ,Seq = isnull(os.Seq,'')
				from #tmp s with(nolock)
				left join Order_SizeCode os with(nolock) on os.id = s.POID and os.SizeCode = s.SizeCode
				where s.PackingListID = t.PackingListID
				and s.CTNStartNo = t.CTNStartNo		
				and s.Ukey = t.Ukey
			) tmp
			order by Seq
			for xml path('')
		)
	,1,1,'')
)ScannedQty
outer apply(
	select value =stuff(
		(select concat('/',tmp.Barcode) 
			from (
				select s.Barcode ,Seq = isnull(os.Seq,'')
				from #tmp s with(nolock)
				left join Order_SizeCode os with(nolock) on os.id = s.POID and os.SizeCode = s.SizeCode
				where s.PackingListID = t.PackingListID
				and s.CTNStartNo = t.CTNStartNo
				and s.Ukey = t.Ukey
                and s.Barcode !=''
			) tmp 
			order by Seq
			for xml path('')
		)
	,1,1,'')
)Barcode
outer apply(
	select value =stuff(
		(select concat('/',tmp.LackingQty) 
			from (
				select s.LackingQty,Seq = isnull(os.Seq,'')
				from #tmp s with(nolock)
				left join Order_SizeCode os with(nolock) on os.id = s.POID and os.SizeCode = s.SizeCode
				where s.PackingListID = t.PackingListID
				and s.CTNStartNo = t.CTNStartNo
				and s.Ukey = t.Ukey
			) tmp 
			order by Seq
			for xml path('')
		)
	,1,1,'')
)LackingQty
group by t.PackingListID
,t.FactoryID
,t.ShipModeID
,OrderList.Value
,StyleSeason.StyleID
,t.BrandID
,StyleSeason.SeasonID
,t.[Dest]
,t.[BuyerDelivery]
,ColorWay.Value
,Color.Value
,Size.Value
,t.CTNStartNo
,t.SCICtnNo
,QtyPerCTN.Value
,ScannedQty.Value
,Barcode.Value
,t.[ScanDate]
,t.[ScanName]
,t.[Lacking]
,LackingQty.Value
,t.ActCTNWeight
,t.ClogActCTNWeight
order by t.FactoryID,t.PackingListID,t.[ScanDate]

{1}
DROP TABLE #TMP,#tmpFinal
", sqlwhere.ToString()
, sqlShow.ToString());
            #endregion

            #region Get Data
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out this.printData)))
            {
                return result;
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
            if (this.printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion

            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string reportname = "Clog_R05.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + reportname);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            this.printData.Columns.Remove("SCICtnNoNo");
            if (!this.rdbSummary.Checked)
            {
                this.printData.Columns.Remove("ActCTNWeight");
                this.printData.Columns.Remove("ClogActCTNWeight");
                worksheet.Range["W:X"].Delete();
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportname, 2, showExcel: false, excelApp: objApp);
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Clog_R05");
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
