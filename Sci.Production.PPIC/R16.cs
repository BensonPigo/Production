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
	select ShipQty = sum(podd.ShipQty) 
	from Pullout_Detail_Detail podd WITH (NOLOCK) 
	inner join Order_Qty oq WITH (NOLOCK) on oq.id=podd.OrderID 
	and podd.Article= oq.Article and podd.SizeCode=oq.SizeCode
	where podd.OrderID = o.ID
)s
where o.Category IN ('B','G') 
      and isnull(ot.IsGMTMaster,0) = 0
      {sqlWhere.ToString()}

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
from openquery([ExtendServer],'select   ins.OrderId,
                                        ins.Location,
                                        [DQSQty] = count(1),
                                        [LastDQSOutputDate] = MAX(iif(ins.Status in (''Pass'',''Fixed''), ins.AddDate, null))
                                from [ManufacturingExecution].[dbo].[Inspection] ins WITH(NOLOCK)
                                where exists( select 1 
                                                from 
                                                [Production].[dbo].Orders o WITH(NOLOCK)
                                                INNER JOIN [Production].[dbo].Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
                                                LEFT JOIN  [Production].[dbo].Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
                                                where o.ID = ins.OrderID 
                                                      {sqlWhere.ToString().Replace("'", "''")}
                                                )
                                group by ins.OrderId,ins.Location
                                ')

select OrderId,
[DQSQty] = MIN(DQSQty),
[LastDQSOutputDate] = MAX(LastDQSOutputDate)
into #tmpInspection
from #tmpInspection_Step1
group by OrderId


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
	,main.Cancelled
    ,PulloutComplete = case when main.PulloutComplete=1 and main.Qty > isnull(main.ShipQty,0) then 'S'
							when main.PulloutComplete=1 and main.Qty <= isnull(main.ShipQty,0) then 'Y'
							when main.PulloutComplete=0 then 'N'
							end
	,main.OrderQty
	,[PackingCarton] = isnull(pd.PackingCarton,0)
	,[PackingQty] = isnull(pd.PackingQty,0)
	,[ClogReceivedCarton] = isnull(pd.ClogReceivedCarton,0)
	,[ClogReceivedQty]=IIF(main.PartialShipment='Y' ,'NA' ,CAST( ISNULL( pd.ClogReceivedQty,0)  as varchar))
	,[LastCMPOutputDate]=LastCMPOutputDate.Value
    ,[CMPQty]=IIF(PartialShipment='Y' ,'NA', CAST(ISNULL( CMPQty.Value,0)  as varchar))
	,ins.LastDQSOutputDate
	,[DQSQty]=IIF(main.PartialShipment='Y' , 'NA' , CAST( ISNULL( ins.DQSQty,0)  as varchar))
	,[OST Packing Qty]=IIF(main.PartialShipment='Y' , 'NA' , CAST(( ISNULL(main.OrderQty,0) -  ISNULL(pd.PackingQty,0)) as varchar))
	,[OST CMP Qty]=IIF(main.PartialShipment='Y' , 'NA' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(CMPQty.Value,0))  as varchar))
	,[OST DQS Qty]=IIF(main.PartialShipment='Y' , 'NA' ,  CAST(( ISNULL(main.OrderQty,0) -  ISNULL(ins.DQSQty,0))  as varchar))
	,[OST Clog Qty]=IIF(main.PartialShipment='Y' , 'NA' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(pd.ClogReceivedQty,0))  as varchar))
	,[OST Clog Carton]= ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)
	,[CFA Inspection result]=oq.CFAFinalInspectResult
	,[3rd party Insp]=IIF(oq.CFAIs3rdInspect =1,'Y','N')
	,[3rd party inspection result]=oq.CFA3rdInspectResult
from #tmpOrderMain main
left join #tmpPackingList_Detail pd on pd.OrderID = main.id and pd.OrderShipmodeSeq = main.Seq
LEFT JOIN Order_QtyShip oq ON oq.ID = main.ID AND oq.Seq = main.Seq
left join #tmpInspection ins on ins.OrderId = main.ID
OUTER APPLY(
	SELECT [Value]=MAX(s.OutputDate)
	FROM SewingOutput s WITH(NOLOCK)
	INNER JOIN SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
	WHERE sd.OrderId=main.ID AND sd.QAQty > 0
)LastCMPOutputDate
OUTER APPLY(
	 SELECT [Value]=[dbo].[getMinCompleteSewQty](main.ID,NULL,NULL) 
)CMPQty
{sqlWhereOutstanding.ToString()}
order by main.ID

DROP TABLE #tmpOrderMain,#tmpPackingList_Detail,#tmpInspection,#tmpInspection_Step1
");
            #endregion

            return DBProxy.Current.Select(string.Empty, sqlcmd.ToString(), out this.printData);
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
            objSheets.get_Range("M:M").ColumnWidth = 8;
            objSheets.get_Range("N:N").ColumnWidth = 9;
            objSheets.get_Range("O:S").ColumnWidth = 8;
            objSheets.get_Range("T:AB").ColumnWidth = 8;
            objSheets.get_Range("G:G").ColumnWidth = 10;
            objSheets.get_Range("V:V").ColumnWidth = 10;
            objSheets.get_Range("T:T").ColumnWidth = 10;

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
