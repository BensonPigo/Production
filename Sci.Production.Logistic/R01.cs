using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Sci.Production.PublicPrg;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private DateTime? buyerDelivery1;
        private DateTime? buyerDelivery2;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private string mDivision;
        private string brand;
        private DataTable printData;

        /// <summary>
        /// get,set SciDelivery1
        /// </summary>
        public DateTime? SciDelivery1
        {
            get
            {
                return this.sciDelivery1;
            }

            set
            {
                this.sciDelivery1 = value;
            }
        }

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Env.User.Keyword;
        }

        /// <summary>
        /// 驗證輸入條件
        /// </summary>
        /// <returns>base.ValidateInput()</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery or SCI Delivery can't be empty!!");
                return false;
            }

            this.buyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.buyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.SciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.mDivision = this.comboM.Text;
            this.brand = this.txtbrand.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">Win.ReportEventArgs</param>
        /// <returns>Result</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string chkWhere = string.Empty;

            sqlCmd.Append(@"
select  o.FactoryID
        , o.MCHandle
        , o.SewLine
        , o.ID
        , o.BrandId
        , o.StyleID
        , s.StyleName
        , o.CustPONo
        , o.Customize1
		, o.Junk
        , oq.BuyerDelivery
        , oq.ShipmodeID
		, oq.Seq
        , o.SciDelivery
        , o.TotalCTN
        , o.PackErrCTN
        , o.ClogCTN
		, o.CfaCTN
        , o.PulloutCTNQty
		, o.StyleUkey
		, [OrderQty] = o.Qty
		, [ShipmodeQty] = oq.Qty
		, o.GMTComplete
into #tmp_Orders
from Orders o WITH (NOLOCK) 
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
inner join Style s  WITH (NOLOCK) on o.StyleUkey = s.Ukey
where o.Category = 'B'");

            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.SciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.SciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", this.mDivision));
            }

            if (this.chkExcludeGMTComplete.Checked)
            {
                ////『勾選』請排除訂單總數量『等於』車縫總數量『等於』出貨總數量
                chkWhere = "AND NOT ( t.OrderQty = t.sewQty AND t.sewQty = t.TtlPullGMTQty) ";
            }

            sqlCmd.Append($@"

select distinct pd.OrderID,pd.OrderShipmodeSeq,pd.ClogLocationId
into #tmp_ClogLocationId
from PackingList_Detail pd WITH (NOLOCK) 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq
where ClogLocationId !='' 
and ClogLocationId is not null and pd.DisposeFromClog= 0  

select pd.OrderID,pd.OrderShipmodeSeq,sum(CTNQty) CTNQty, sum(ShipQty) GMTQty
into #tmp_CTNQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq 
where pd.DisposeFromClog= 0
group by pd.OrderID,pd.OrderShipmodeSeq

select pd.OrderID,pd.OrderShipmodeSeq,sum(CTNQty) ClogQty, sum(ShipQty) ClogGMTQty
into #tmp_ClogQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq 
where pd.ReceiveDate is not null and pd.DisposeFromClog= 0
group by pd.OrderID,pd.OrderShipmodeSeq
 
select  pd.OrderID,pd.OrderShipmodeSeq,sum (pd.CTNQty) PullQty
into #tmp_PullQty
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq  
where p.PulloutID != '' and pd.DisposeFromClog= 0
group by pd.OrderID,pd.OrderShipmodeSeq

select pd.OrderID,sum(ShipQty) TtlGMTQty
into #tmp_TtlGMTQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join (select distinct id from #tmp_Orders) o on pd.OrderID = o.ID   
where pd.DisposeFromClog= 0
group by pd.OrderID
 
select pd.OrderID,sum(ShipQty) TtlClogGMTQty
into #tmp_TtlClogGMTQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join (select distinct id from #tmp_Orders) o on pd.OrderID = o.ID   
where ReceiveDate is not null and pd.DisposeFromClog= 0
group by pd.OrderID

select pd.OrderID,pd.OrderShipmodeSeq,sum(ScanQty) ScanQtybyShipmode
into #tmp_ScanQtybyShipmode
from PackingList_Detail pd WITH (NOLOCK) 
inner join (select distinct id,Seq from #tmp_Orders) o on pd.OrderID = o.ID and pd.OrderShipmodeSeq = o.Seq
group by pd.OrderID,pd.OrderShipmodeSeq

select pd.OrderID,pd.OrderShipmodeSeq,[Date]=MAX(t.AddDate)
into #tmp_LastTransferToClog
from PackingList_Detail pd WITH (NOLOCK) 
inner join (select distinct id,Seq from #tmp_Orders) o on pd.OrderID = o.ID and pd.OrderShipmodeSeq = o.Seq
inner join TransferToClog t  WITH (NOLOCK) on pd.ID = t.PackingListID and pd.OrderID = t.OrderID and pd.CTNStartNo = t.CTNStartNo
group by pd.OrderID,pd.OrderShipmodeSeq

select pd.OrderID,pd.OrderShipmodeSeq,[Date]=MAX(c.AddDate)
into #tmp_LastCartonReceived
from PackingList_Detail pd WITH (NOLOCK) 
inner join (select distinct id,Seq from #tmp_Orders) o on pd.OrderID = o.ID and pd.OrderShipmodeSeq = o.Seq
inner join ClogReceive c  WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
group by pd.OrderID,pd.OrderShipmodeSeq


select pd.OrderID,sum(ShipQty) TtlPullGMTQty
into #tmp_TtlPullGMTQty
from Pullout p WITH (NOLOCK) 
inner join Pullout_Detail pd WITH (NOLOCK) on pd.ID = p.ID 
inner join (select distinct id from #tmp_Orders) o on pd.OrderID = o.ID  
where p.Status <> 'New'
group by pd.OrderID
 
select pd.OrderID,pd.OrderShipmodeSeq,sum(ShipQty) PullGMTQty
into #tmp_PullGMTQty
from Pullout p
inner join Pullout_Detail pd WITH (NOLOCK) on pd.ID = p.ID 
inner join #tmp_Orders o on pd.OrderID = o.ID and pd.OrderShipmodeSeq = o.Seq 
where p.Status <> 'New'
group by pd.OrderID,pd.OrderShipmodeSeq

select Orderid=id,sewQty=sum(QAQty)
into #tmp_sewQty
from(
	select id,Article, SizeCode, QAQty = MIN(QAQty)
	from(
		select
			o.id
			, oq.Article
			, oq.SizeCode
			, oq.Qty
			, ComboType = sl.Location
			, QAQty = isnull(sum(sdd.QAQty),0)
		from (select distinct id,StyleUkey from #tmp_Orders) o
		inner join Style_Location sl WITH (NOLOCK) on o.StyleUkey = sl.StyleUkey
		inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
		left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
			on sdd.OrderId = o.ID and sdd.Article = oq.Article and sdd.SizeCode = oq.SizeCode and sdd.ComboType = sl.Location
		group by o.id,oq.Article,oq.SizeCode,oq.Qty,sl.Location
	)m
	group by id,Article,SizeCode
)s
group by id

select o.*,
	 Location = stuff ((select concat(',',ClogLocationId)  from #tmp_ClogLocationId
				where   OrderID = o.ID 
                and OrderShipmodeSeq = o.Seq
				order by ClogLocationId
							for xml path('')
					) ,1,1,'') 
	,CTNQty = isnull(ctn.CTNQty, 0)
	,ClogQty = isnull(clog.ClogQty,0)
	,PullQty = isnull(pull.PullQty,0)
	,TtlGMTQty = isnull(ttlg.TtlGMTQty,0)
	,TtlClogGMTQty = isnull(ttlc.TtlClogGMTQty,0)
	,TtlPullGMTQty = isnull(ttlp.TtlPullGMTQty,0)
	,ScanQtybyShipmode=isnull(ttls.ScanQtybyShipmode,0)
	,GMTQty = isnull(ctn.GMTQty,0)
	,ClogGMTQty = isnull(clog.ClogGMTQty,0)
	,PullGMTQty = isnull(pullG.PullGMTQty,0)
	,sewQty=isnull(tlsq.sewQty,0)
	,LastTransferToClog= a.Date
	,LastCartonReceived= b.Date
into #tmp 
from #tmp_Orders o
left join #tmp_CTNQty ctn on o.ID = ctn.OrderID and o.Seq = ctn.OrderShipmodeSeq
left join #tmp_ClogQty clog on o.ID = clog.OrderID and o.Seq = clog.OrderShipmodeSeq
left join #tmp_PullQty pull on o.ID = pull.OrderID and o.Seq = pull.OrderShipmodeSeq
left join #tmp_TtlGMTQty ttlg on o.ID = ttlg.OrderID 
left join #tmp_TtlClogGMTQty ttlc on o.ID = ttlc.OrderID
left join #tmp_TtlPullGMTQty ttlp on o.ID = ttlp.OrderID 
left join #tmp_ScanQtybyShipmode ttls on o.ID = ttls.OrderID and ttls.OrderShipmodeSeq = pull.OrderShipmodeSeq
left join #tmp_PullGMTQty pullG on o.ID = pullG.OrderID and o.Seq = pullG.OrderShipmodeSeq 
left join #tmp_sewQty tlsq on o.ID = tlsq.Orderid
LEFT JOIN #tmp_LastTransferToClog a ON o.ID = a.OrderID and o.Seq  = a.OrderShipmodeSeq
LEFT JOIN #tmp_LastCartonReceived b ON o.ID = b.OrderID and o.Seq  = b.OrderShipmodeSeq

drop table #tmp_Orders,#tmp_ClogLocationId,#tmp_CTNQty,#tmp_ClogQty,#tmp_PullQty,#tmp_TtlGMTQty
,#tmp_TtlClogGMTQty,#tmp_TtlPullGMTQty,#tmp_PullGMTQty,#tmp_ScanQtybyShipmode,#tmp_sewQty,#tmp_LastTransferToClog,#tmp_LastCartonReceived

select t.ID,RetCtnBySP = count(cr.ID)
into #tmp2
from (select distinct ID from #tmp) t 
left join ClogReturn cr on cr.OrderID = t.ID
group by t.ID

select 
     t.FactoryID
    ,t.MCHandle
    ,t.SewLine
    ,t.ID
    ,t.BrandID
    ,t.StyleID
    ,t.StyleName
    ,t.CustPONo
    ,t.Customize1
	,t.GMTComplete
    ,t.SciDelivery
    ,[Junk]= IIF(t.Junk=1,'Y','')
    ,t.BuyerDelivery
    ,t.ShipmodeID
    ,t.Location
	,t.LastTransferToClog
	,t.LastCartonReceived
	,t.TotalCTN
	,PackErrCTN = isnull(t.PackErrCTN,0)
	,ClogCTN=isnull(t.ClogCTN,0)
	,CfaCTN=isnull(t.CfaCTN,0)
	,t2.RetCtnBySP
	,[Bal Ctn by SP#]=isnull(t.TotalCTN,0)-isnull(t.ClogCTN,0) -isnull(t.CfaCTN,0) - isnull(t.PackErrCTN,0)
	,[% by SP#]=iif(isnull(t.TtlGMTQty,0)=0,0,Round(1-((t.TtlGMTQty-isnull(t.TtlClogGMTQty,0))/t.TtlGMTQty),2)*100)
	,[Ctn SDP by SP#]=iif(isnull(t.TotalCTN,0)=0, 0,ROUND(isnull(t.ClogCTN,0)/t.TotalCTN,2)*100)
	,t.PulloutCTNQty
	,t.OrderQty
	,[Pack Qty by SP#]=t.TtlGMTQty
	,[PackingStatus]= iif(
		(select cnt = count(1) FROM PackingList p
		INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
		WHERE pd.OrderID=t.id ) = 0,'N',PackingStatus.HasNotYetConfirmed) 
	,[Dispose Qty by SP#]= ISNULL(DisposeQty.Value,0)
	,[Sewing Qty by SP#]=sewQty
	,t.TtlClogGMTQty
	,[Bal Qty by SP#] = isnull(t.TtlGMTQty,0)-isnull(t.TtlClogGMTQty,0)
	,[Qty SDP by SP#]=iiF(isnull(t.TtlGMTQty,0)=0,0,ROUND(isnull(t.TtlClogGMTQty,0)/t.TtlGMTQty,2)*100)
	,t.TtlPullGMTQty
	,t.CTNQty
	,t.ClogQty
	,[Bal Ctn by Shipmode]=isnull(t.CTNQty,0)-isnull(t.ClogQty,0)
	,[Ctn SDP by Shipmode]=iiF(isnull(t.CTNQty,0)=0,0,ROUND(isnull(t.ClogQty,0)/t.CTNQty,2)*100)
	,t.PullQty
	,[CTN in CLOG]=isnull(t.ClogQty,0)-isnull(t.PullQty,0)
	,t.ShipmodeQty
	,t.GMTQty
	,[Scan Qty by Shipmode]=ScanQtybyShipmode
	,t.ClogGMTQty
	,[Bal Qty by Shipmode]=isnull(t.GMTQty,0)-isnull(t.PullGMTQty,0)
	,[Qty SDP by Shipmode]=iif(isnull(t.GMTQty,0)=0,0,ROUND(isnull(t.ClogGMTQty,0)/t.GMTQty,2)*100)
	,t.PullGMTQty
from #tmp t
INNER JOIN #tmp2 t2 ON t.id = t2.ID
OUTER APPLY(
	SELECT [HasNotYetConfirmed]=IIF(NotYetConfirmed > 0,'N','Y')
	FROM (
		SELECT NotYetConfirmed=COUNT(1)
		FROM PackingList p
		INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
		WHERE pd.OrderID=t.id AND p.Status <> 'Confirmed'
	)a
)PackingStatus
OUTER APPLY(
    ----計算方式參照Clog P11
	SELECT  [Value]=sum(ISNULL(pd.QtyPerCTN,0))
	FROM ClogGarmentDispose cd  with (nolock) 
	INNER JOIN ClogGarmentDispose_Detail cdd with (nolock) ON  cd.ID=cdd.ID
	left join PackingList_Detail pd with (nolock) on  pd.ID = cdd.PackingListID and pd.CTNStartNO = cdd.CTNStartNO
	where cd.Status='Confirmed' AND pd.OrderID=t.id 
)DisposeQty

where 1=1
{chkWhere}
order by t.FactoryID,t.ID,t.BuyerDelivery

drop table #tmp,#tmp2
                

");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">Win.ReportDefinition</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            // 檢查是否擁有Clog R01的Confirm 權限
            bool canConfrim = Prgs.GetAuthority(Env.User.UserID, "P01. Clog Master List", "CanConfirm");

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\Logistic_R01_CartonStatusReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[2, 1] = string.Format(
                "Buyer Delivery: {0} ~ {1}             SCI Delivery: {2} ~ {3}             M: {4}             Brand: {5}",
                MyUtility.Check.Empty(this.buyerDelivery1) ? string.Empty : Convert.ToDateTime(this.buyerDelivery1).ToString("d"),
                MyUtility.Check.Empty(this.buyerDelivery2) ? string.Empty : Convert.ToDateTime(this.buyerDelivery2).ToString("d"),
                MyUtility.Check.Empty(this.SciDelivery1) ? string.Empty : Convert.ToDateTime(this.SciDelivery1).ToString("d"),
                MyUtility.Check.Empty(this.sciDelivery2) ? string.Empty : Convert.ToDateTime(this.sciDelivery2).ToString("d"),
                this.mDivision,
                this.brand);

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Logistic_R01_CartonStatusReport.xltx", 3, false, null, excel); // 將datatable copy to excel

            ////此欄位只有 Clog R01 擁有 Confirm 權限的使用者可以『看到』，其餘的則移除該欄位。
            if (!canConfrim)
            {
                worksheet.get_Range("AD:AD").EntireColumn.Delete();
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.CreateCustomizedExcel(ref worksheet);

            #region Save & show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Logistic_R01_CartonStatusReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excel);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
