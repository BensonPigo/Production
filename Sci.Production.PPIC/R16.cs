using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class R16 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private string BrandID;
        private string MDivisionID;
        private string FactoryID;
        private bool IsOutstanding;
        private DateTime? BuyerDev_S;
        private DateTime? BuyerDev_E;

        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.BuyerDev_S = null;
            this.BuyerDev_E = null;

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

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlcmd = new StringBuilder();

            #region 組SQL

            sqlcmd.Append($@"
SELECT 
	 o.FactoryID
	,o.ID
	,o.CustPONo
	,o.StyleID
	,oq.BuyerDelivery

	,oq.Seq
	,oq.ShipmodeID
	,[Category] =  CASE WHEN o.Category='B' THEN 'Bulk' 
						WHEN o.Category='G' THEN 'Bulk' 
						ELSE ''
				   END
	,[PartialShipment]=IIF(PartialShipment.Count > 1 ,'Y','')
	,[Cancelled]=IIF(o.Junk=1,'Y','N')

	,[OrderQty]=OrderQty.Qty
	,[PackingCarton] = PackingCarton.Value
	,[PackingQty]=PackingQty.Value
	,[ClogReceivedCarton]=ISNULL(ClogReceivedCarton.Value ,0)

	,[LastCMPOutputDate]=LastCMPOutputDate.Value
	,[LastDQSOutputDate]=LastDQSOutputDate.Value
	,[OST Clog Carton]= ISNULL(PackingCarton.Value,0) - ISNULL(ClogReceivedCarton.Value,0)
INTO #tmp
FROM Orders o
LEFT JOIN Order_QtyShip oq ON o.ID=oq.ID
LEFT JOIN OrderType ot ON o.OrderTypeID=ot.ID AND o.BrandID = ot.BrandID
OUTER APPLY(
	SELECT [Count] = COUNT(ID) FROM Order_QtyShip oqq WHERE oqq.Id=o.ID
)PartialShipment
OUTER APPLY(
	SELECT Qty FROM Order_QtyShip oqq WHERE oqq.Id=o.ID AND oqq.Seq=oq.Seq
)OrderQty
OUTER APPLY(
	SELECT [Value]=COUNT(Ukey) FROM PackingList_Detail pd
	WHERE pd.OrderID=o.ID
	AND pd.OrderShipmodeSeq=oq.Seq
	AND pd.CTNQty=1
)PackingCarton
OUTER APPLY(
	SELECT [Value]=Sum(pd.ShipQty)
	FROM PackingList_Detail pd
	WHERE pd.OrderID=o.ID
	AND pd.OrderShipmodeSeq=oq.Seq
	AND pd.CTNQty=1
)PackingQty
OUTER APPLY(
	SELECT [Value]=COUNT(Ukey) FROM PackingList_Detail pd
	WHERE pd.OrderID=o.ID
	AND pd.OrderShipmodeSeq=oq.Seq
	AND pd.CTNQty=1
	AND ( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL)
)ClogReceivedCarton
OUTER APPLY(
	SELECT [Value]=MAX(s.OutputDate)
	FROM SewingOutput s
	INNER JOIN SewingOutput_Detail sd ON s.ID=sd.ID
	WHERE sd.OrderId=o.ID AND sd.QAQty > 0
)LastCMPOutputDate
OUTER APPLY(
	SELECT [Value]=MAX(Date)
	FROM(
		SELECT [Date]=MAX(AddDate) 
		FROM [ManufacturingExecution].[dbo].[Inspection]
		WHERE OrderId=o.ID AND Status='Pass'
		UNION 
		SELECT [Date]=MAX(EditDate)
		FROM [ManufacturingExecution].[dbo].[Inspection]
		WHERE OrderId=o.ID AND Status='Fixed'
	)Tmp
)LastDQSOutputDate
WHERE o.Category IN ('B','G') 
AND ot.IsGMTMaster=0
");
            #endregion

            #region WHERE條件
            if (!MyUtility.Check.Empty(this.BuyerDev_S))
            {
                sqlcmd.Append($"AND oq.BuyerDelivery >= '{this.BuyerDev_S.Value.ToString("yyyy/MM/dd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BuyerDev_E))
            {
                sqlcmd.Append($"AND oq.BuyerDelivery <= '{this.BuyerDev_E.Value.ToString("yyyy/MM/dd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BrandID))
            {
                sqlcmd.Append($"AND o.BrandID = '{this.BrandID}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                sqlcmd.Append($"AND o.MDivisionID = '{this.MDivisionID}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                sqlcmd.Append($"AND o.FtyGroup = '{this.FactoryID}'" + Environment.NewLine);
            }

            if (this.IsOutstanding)
            {
                sqlcmd.Append($" AND( OrderQty.Qty > PackingQty.Value OR (ISNULL(PackingCarton.Value,0) - ISNULL(ClogReceivedCarton.Value,0)) <> 0 ) " + Environment.NewLine);
            }

            #endregion

            #region SQL

            sqlcmd.Append($@"

SELECT 
	 FactoryID
	,ID
	,CustPONo
	,StyleID
	,BuyerDelivery

	,Seq
	,ShipmodeID
	,Category
	,PartialShipment
	,Cancelled

	,[OrderQty]
	,[PackingCarton]
	,[PackingQty]
	,[ClogReceivedCarton]	
	,[ClogReceivedQty]=IIF(PartialShipment='Y' ,'NA' ,CAST( ISNULL( ClogReceivedQty.Value,0)  as varchar))

	,[LastCMPOutputDate]
	,[CMPQty]=IIF(PartialShipment='Y' ,'NA', CAST(ISNULL( CMPQty.Value,0)  as varchar))
	,[LastDQSOutputDate]
	,[DQSQty]=IIF(PartialShipment='Y' , 'NA' , CAST( ISNULL( DQSQty.Value,0)  as varchar))
	,[OST Packing Qty]=IIF(PartialShipment='Y' , 'NA' , CAST(( ISNULL(OrderQty,0) -  ISNULL(PackingQty,0)) as varchar))

	,[OST CMP Qty]=IIF(PartialShipment='Y' , 'NA' , CAST((  ISNULL(OrderQty,0) -  ISNULL(CMPQty.Value,0))  as varchar))
	,[OST DQS Qty]=IIF(PartialShipment='Y' , 'NA' ,  CAST(( ISNULL(OrderQty,0) -  ISNULL(DQSQty.Value,0))  as varchar))
	,[OST Clog Qty]=IIF(PartialShipment='Y' , 'NA' , CAST((  ISNULL(OrderQty,0) -  ISNULL(ClogReceivedQty.Value,0))  as varchar))
	,[OST Clog Carton]
FROM #tmp o
OUTER APPLY(
	SELECT [Value]=Sum(pd.ShipQty) 
	FROM PackingList_Detail pd
	WHERE pd.OrderID=o.ID
	AND pd.OrderShipmodeSeq=o.Seq
	AND pd.CTNQty=1
	AND ( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL)
)ClogReceivedQty
OUTER APPLY(
	 SELECT [Value]=[dbo].[getMinCompleteSewQty](o.ID,NULL,NULL) 
)CMPQty
OUTER APPLY(
	SELECT [Value]= SUM( DISTINCT  [dbo].[getMinCompleteSewQty](i.OrderId,i.Article,i.Size))
	FROM [ManufacturingExecution].[dbo].[Inspection] i
	WHERE i.OrderId=o.ID AND (i.Status='Pass' OR i.Status='Fixed')
)DQSQty

ORDER BY O.ID

DROP TABLE #tmp

");
            #endregion

            DBProxy.Current.Select(string.Empty, sqlcmd.ToString(), out this.printData);

            return Result.True;
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

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "PPIC_R16.xltx", 1);
            return true;
        }
    }
}
