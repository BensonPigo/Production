using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R16 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private string BrandID;
        private string MDivisionID;
        private string FactoryID;
        private bool IsOutstanding;
        private bool IsExcludeSister;
        private bool IsExcludeCancelShortage;
        private bool IsBookingOrder;
        private DateTime? BuyerDev_S;
        private DateTime? BuyerDev_E;

        /// <summary>
        /// Initializes a new instance of the <see cref="R16"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.txtMdivision.Text = Env.User.Keyword;
            this.txtfactory.Text = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.BuyerDev_S = null;
            this.BuyerDev_E = null;

            if (!this.dateRangeByerDev.Value1.HasValue || !this.dateRangeByerDev.Value2.HasValue)
            {
                MyUtility.Msg.InfoBox("Buyer Delivery Date can not be empty.");
                return false;
            }

            if (this.dateRangeByerDev.Value1.HasValue)
            {
                this.BuyerDev_S = this.dateRangeByerDev.Value1.Value;
            }

            if (this.dateRangeByerDev.Value2.HasValue)
            {
                this.BuyerDev_E = this.dateRangeByerDev.Value2.Value;
            }

            this.MDivisionID = this.txtMdivision.Text;
            this.BrandID = this.txtbrand.Text;
            this.FactoryID = this.txtfactory.Text;
            this.IsOutstanding = this.chkOutstanding.Checked;
            this.IsExcludeSister = this.chkExcludeSis.Checked;
            this.IsExcludeCancelShortage = this.chkExcludeCancelShortage.Checked;
            this.IsBookingOrder = this.chkBookingOrder.Checked;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            StringBuilder sqlWhereOutstanding = new StringBuilder();
            #region WHERE條件
            if (!MyUtility.Check.Empty(this.BuyerDev_S))
            {
                sqlWhere.Append($"AND oq.BuyerDelivery >= '{this.BuyerDev_S.Value.ToString("yyyy/MM/dd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BuyerDev_E))
            {
                sqlWhere.Append($"AND oq.BuyerDelivery <= '{this.BuyerDev_E.Value.ToString("yyyy/MM/dd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BrandID))
            {
                sqlWhere.Append($"AND o.BrandID = '{this.BrandID}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                sqlWhere.Append($"AND o.MDivisionID = '{this.MDivisionID}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                sqlWhere.Append($"AND o.FtyGroup = '{this.FactoryID}'" + Environment.NewLine);
            }

            if (this.IsOutstanding)
            {
                sqlWhereOutstanding.Append($" where ( main.OrderQty > ISNULL(pd.PackingQty,0) OR (ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)) <> 0 ) " + Environment.NewLine);
            }

            if (this.IsExcludeSister)
            {
                sqlWhere.Append($"AND f.IsProduceFty=1" + Environment.NewLine);
            }

            if (this.IsExcludeCancelShortage)
            {
                sqlWhere.Append($"AND o.Junk = 0 and o.GMTComplete <> 'S'" + Environment.NewLine);
            }

            if (this.IsBookingOrder)
            {
                sqlWhere.Append($"and isnull(ot.IsGMTMaster,0) = 0" + Environment.NewLine);
            }
            #endregion

            #region 組SQL

            sqlcmd.Append($@"
SELECT 
	 o.FactoryID
    ,o.BrandID
	,o.ID
	,o.CustPONo
	,o.StyleID
	,oq.BuyerDelivery
	,oq.Seq
	,oq.ShipmodeID
    ,[Dest] = c.Alias
	,[Category] =  CASE WHEN o.Category='B' THEN 'Bulk' 
						WHEN o.Category='G' THEN 'Garment' 
						ELSE ''
				   END
	,[PartialShipment]=IIF(PartialShipment.Count > 1 ,'Y','')
	,[Cancelled]=IIF(o.Junk=1,'Y','N')
    ,o.PulloutComplete
	,[OrderQty]= isnull(oq.Qty,0)
    ,ShipQty=isnull(s.ShipQty,0)
    ,o.Qty
	,f.KPICode 
    ,[cbsnp] = IIF(o.NeedProduction = 0, 'N','Y')
    ,[BookingSP] = CASE WHEN o.Category='G' THEN OrderQtyGarment.value
                        WHEN ot.IsGMTMaster=1 THEN 'Y' 
                   ELSE ''
                   END
into #tmpOrderMain
FROM Orders o WITH(NOLOCK)
INNER JOIN Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
LEFT JOIN Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
LEFT JOIN OrderType ot WITH(NOLOCK) ON o.OrderTypeID=ot.ID AND o.BrandID = ot.BrandID
LEFT JOIN Country c WITH(NOLOCK) on c.id = o.dest
OUTER APPLY(
	SELECT [Count] = COUNT(ID) FROM Order_QtyShip oqq WITH(NOLOCK) WHERE oqq.Id=o.ID
)PartialShipment
outer apply(
    select ShipQty = sum(pd.ShipQty)
    from PackingList_Detail pd WITH(NOLOCK)
    inner join Order_Qty oq WITH(NOLOCK) on oq.ID = pd.OrderID and oq.Article = pd.Article and oq.SizeCode = pd.SizeCode
    inner join PackingList p WITH(NOLOCK) on p.ID = pd.ID
    where p.PulloutID <> ''
    and pd.OrderID = o.ID
)s
outer apply(
    select value = STUFF((
        select CONCAT(',',OrderIDFrom)
        from(
            select distinct OrderIDFrom
            from Order_Qty_Garment WITH(NOLOCK)
            where ID = o.ID
        )s
        for xml path('')
        ),1,1,'')
) OrderQtyGarment 
where o.Category IN ('B','G') 
      {sqlWhere}

select 
    pd.OrderID,
    pd.OrderShipmodeSeq,
    [PackingQty] = sum(isnull(pd.ShipQty,0)),
    [PackingCarton] = sum(iif(pd.CTNQty = 1,1,0)),
    [ClogReceivedCarton] = sum(iif(pd.CTNQty = 1 AND ( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL),1,0)),
    [ClogReceivedQty] = sum(iif( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL,pd.ShipQty,0))
into #tmpPackingList_Detail
from PackingList_Detail pd with (nolock)
where exists(select 1 from #tmpOrderMain main where pd.OrderID = main.ID and 
													pd.OrderShipmodeSeq = main.Seq)
group by pd.OrderID,pd.OrderShipmodeSeq


select 
*
into #tmpInspection_Step1
from openquery([ExtendServer],'
    select  ins.OrderId,
            ins.article,
            ins.size,
            [DQSQty] = count(1),
            [LastDQSOutputDate] = MAX(iif(ins.Status in (''Pass'',''Fixed''), ins.AddDate, null))
    from [ManufacturingExecution].[dbo].[Inspection] ins WITH(NOLOCK)
    where exists(
        select 1 
        from [Production].[dbo].Orders o WITH(NOLOCK)
        INNER JOIN [Production].[dbo].Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
        LEFT JOIN  [Production].[dbo].Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
        LEFT JOIN [Production].[dbo].OrderType ot WITH(NOLOCK) ON o.OrderTypeID=ot.ID AND o.BrandID = ot.BrandID
        where o.ID = ins.OrderID 
        {sqlWhere.ToString().Replace("'", "''")}
    )
    and ins.Status in (''Pass'',''Fixed'')
    group by ins.OrderId,ins.article,ins.size
')

select OrderId,
[LastDQSOutputDate] = MAX(LastDQSOutputDate)
into #tmpInspection
from #tmpInspection_Step1
group by OrderId

--- DQSQTY 計算分配
--要先找每個OrderID所有的Seq,因為可能被篩選掉
SELECT oq.Id,oq.Seq,oq.BuyerDelivery
INTO #tmpOrder_QtyShip
FROM Order_QtyShip oq
WHERE EXISTS(SELECT 1 FROM #tmpOrderMain WHERE ID = oq.Id)

SELECT m.Id,m.Seq,t.Article,t.Size, t.DQSQty,
    Qty = ISNULL(od.Qty, 0),
    RowNum = ROW_NUMBER() OVER(PARTITION BY m.Id,t.Article,t.Size ORDER BY m.BuyerDelivery, m.Seq)
INTO #PrepareRn
FROM #tmpOrder_QtyShip m
INNER JOIN #tmpInspection_Step1 t on t.OrderID = m.ID
Left JOIN Order_QtyShip_Detail od on od.Id = m.id AND od.Seq = m.Seq AND od.Article = t.Article AND od.SizeCode = t.Size
ORDER by m.Id,t.Article,t.Size,m.Seq

SELECT m.Id,m.Article,m.Size,RowNum,m.Seq,
    TTL_DQSQty_bySame_ArticleSize = m.DQSQty,
    m.Qty,
    sumQty_OrderbySeq = SUM(m.Qty) OVER(PARTITION BY m.Id,m.Article,m.Size ORDER BY m.RowNum),
    remaining_DQSQty = m.DQSQty - SUM(m.Qty) OVER(PARTITION BY m.Id,m.Article,m.Size ORDER BY m.RowNum),
    MAXRowNum = MAX(m.RowNum) OVER(PARTITION BY m.Id,m.Article,m.Size),
    MINRowNum = MIN(m.RowNum) OVER(PARTITION BY m.Id,m.Article,m.Size)    
INTO #PrepareCalculate
FROM #PrepareRn m
ORDER by m.Id,m.Article,m.Size,m.RowNum

SELECT *
    ,previous_remaining_DQSQty = ISNULL((LAG(remaining_DQSQty) OVER(PARTITION BY Id,Article,Size ORDER BY RowNum)), 0) 
    ,previous_sumQty_OrderbySeq = ISNULL((LAG(sumQty_OrderbySeq) OVER(PARTITION BY Id,Article,Size ORDER BY RowNum)), 0) 
INTO #PrepareCalculate2
FROM #PrepareCalculate

SELECT *,
    AssignedQty = CASE
        WHEN RowNum = MINRowNum AND remaining_DQSQty <= 0 THEN TTL_DQSQty_bySame_ArticleSize
        WHEN RowNum = MAXRowNum AND previous_remaining_DQSQty > sumQty_OrderbySeq THEN previous_remaining_DQSQty
        WHEN RowNum = MAXRowNum AND remaining_DQSQty > 0 THEN remaining_DQSQty
        WHEN remaining_DQSQty < 0 AND previous_remaining_DQSQty > 0 THEN previous_remaining_DQSQty
        WHEN remaining_DQSQty < 0 THEN 0
        ELSE Qty
        END
INTO #tmpAssignedQty_by_IDArtcleSizeSeq
FROM #PrepareCalculate2

SELECT ID,Seq,DQSQty = Sum(AssignedQty)
INTO #tmpDQSQty
from #tmpAssignedQty_by_IDArtcleSizeSeq
GROUP BY ID,Seq

--DQSQTY 計算結尾

select main.KPICode
	,main.FactoryID
    ,main.BrandID
	,main.ID
	,main.CustPONo
	,main.StyleID
	,main.BuyerDelivery
	,main.Seq
	,main.ShipmodeID
    ,main.dest
	,main.Category
	,main.PartialShipment   
    ,main.BookingSP
	,main.Cancelled
    ,[Cancelled but still need production] = main.cbsnp
    ,PulloutComplete = case when main.PulloutComplete=1 and main.Qty > isnull(main.ShipQty,0) then 'S'
							when main.PulloutComplete=1 and main.Qty <= isnull(main.ShipQty,0) then 'Y'
							when main.PulloutComplete=0 then 'N'
							end
	,main.OrderQty
	,[PackingCarton] = isnull(pd.PackingCarton,0)
	,[PackingQty] = isnull(pd.PackingQty,0)
	,[ClogReceivedCarton] = isnull(pd.ClogReceivedCarton,0)
	,[ClogReceivedQty]=ISNULL( pd.ClogReceivedQty,0)
	,[LastCMPOutputDate]=LastCMPOutputDate.Value
    ,[CMPQty]=IIF(PartialShipment='Y' ,'NA', CAST(ISNULL( CMPQty.Value,0)  as varchar))
	,ins.LastDQSOutputDate
	,[DQSQty] = dq.DQSQty
	,[OST Packing Qty]=IIF(main.PartialShipment='Y' , 'NA' , CAST(( ISNULL(main.OrderQty,0) -  ISNULL(pd.PackingQty,0)) as varchar))
	,[OST CMP Qty]=IIF(main.PartialShipment='Y' , 'NA' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(CMPQty.Value,0))  as varchar))
	,[OST DQS Qty] = ISNULL(main.OrderQty, 0) - ISNULL(dq.DQSQty, 0)
	,[OST Clog Qty]=(ISNULL(main.OrderQty,0) - ISNULL(pd.ClogReceivedQty,0))
	,[OST Clog Carton]= ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)
	,[CFA Inspection result]=oq.CFAFinalInspectResult
	,[3rd party Insp]=IIF(oq.CFAIs3rdInspect =1,'Y','N')
	,[3rd party inspection result]=oq.CFA3rdInspectResult
from #tmpOrderMain main
left join #tmpPackingList_Detail pd on pd.OrderID = main.id and pd.OrderShipmodeSeq = main.Seq
LEFT JOIN Order_QtyShip oq ON oq.ID = main.ID AND oq.Seq = main.Seq
left join #tmpInspection ins on ins.OrderId = main.ID
LEFT JOIN #tmpDQSQty dq on dq.ID = main.ID AND dq.Seq = main.Seq
OUTER APPLY(
	SELECT [Value]=MAX(s.OutputDate)
	FROM SewingOutput s WITH(NOLOCK)
	INNER JOIN SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
	WHERE sd.OrderId=main.ID AND sd.QAQty > 0
)LastCMPOutputDate
OUTER APPLY(
	 SELECT [Value]=[dbo].[getMinCompleteSewQty](main.ID,NULL,NULL) 
)CMPQty
{sqlWhereOutstanding}
order by main.ID

drop table #tmpOrderMain,#tmpPackingList_Detail,#tmpInspection_Step1,#tmpInspection,#PrepareCalculate,#PrepareCalculate2,#tmpAssignedQty_by_IDArtcleSizeSeq,#tmpDQSQty,#PrepareRn
");
            #endregion

            DBProxy.Current.DefaultTimeout = 600;
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd.ToString(), out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R16.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "PPIC_R16.xltx", 1, false, null, objApp); // 將datatable copy to excel

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.get_Range("J:J").ColumnWidth = 8;
            objSheets.get_Range("N:N").ColumnWidth = 8;
            objSheets.get_Range("O:O").ColumnWidth = 9;
            objSheets.get_Range("P:T").ColumnWidth = 8;
            objSheets.get_Range("V:AC").ColumnWidth = 8;
            objSheets.get_Range("G:G").ColumnWidth = 10;
            objSheets.get_Range("W:W").ColumnWidth = 10;
            objSheets.get_Range("U:U").ColumnWidth = 10;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R16");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion

            return true;
        }
    }
}
