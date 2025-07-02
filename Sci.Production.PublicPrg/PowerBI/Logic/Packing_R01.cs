using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System.Data;
using System.Text;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Packing_R01
    {

        /// <inheritdoc/>
        public Packing_R01()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetPacking_R01Data(Packing_R01_ViewModel model)
        {
            StringBuilder sqlwhere = this.GetSQLWhere(model);

            #region 先準備主要資料table
            string sqlcmd = string.Format(
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
	from PackingList_Detail pld with (nolock)
	where pld.ID=t.[Packing#] and pld.OrderID=t.SP# and pld.CTNStartNo = t.[CTN#]
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

            #region Get Data
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlcmd, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
            #endregion
        }

        /// <inheritdoc/>
        public Base_ViewModel GetPacking_R01CustomizeData(Packing_R01_ViewModel model)
        {
            StringBuilder sqlwhere = this.GetSQLWhere(model);

            #region 準備動態的(欄位名稱)
            string sqlcmdcolumnName = string.Format(
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
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlcmdcolumnName, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
            #endregion
        }

        private StringBuilder GetSQLWhere(Packing_R01_ViewModel model)
        {
            StringBuilder sqlwhere = new StringBuilder();

            #region 準備where條件, 兩段sql用相同條件
            if (!MyUtility.Check.Empty(model.SP1))
            {
                sqlwhere.Append(string.Format(" and pld.OrderID >= '{0}'", model.SP1));
            }

            if (!MyUtility.Check.Empty(model.SP2))
            {
                sqlwhere.Append(string.Format(" and pld.OrderID <= '{0}'", model.SP2));
            }

            if (!MyUtility.Check.Empty(model.PackingID1))
            {
                sqlwhere.Append(string.Format(" and pld.id >= '{0}'", model.PackingID1));
            }

            if (!MyUtility.Check.Empty(model.PackingID2))
            {
                sqlwhere.Append(string.Format(" and pld.id <= '{0}'", model.PackingID2));
            }

            if (!MyUtility.Check.Empty(model.BuyerDelivery1))
            {
                sqlwhere.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", model.BuyerDelivery1));
            }

            if (!MyUtility.Check.Empty(model.BuyerDelivery2))
            {
                sqlwhere.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", model.BuyerDelivery2));
            }

            if (!MyUtility.Check.Empty(model.ScanEditDate1))
            {
                sqlwhere.Append(string.Format(" and pld.ScanEditDate >= '{0}'", model.ScanEditDate1));
            }

            if (!MyUtility.Check.Empty(model.ScanEditDate2))
            {
                sqlwhere.Append(string.Format(" and pld.ScanEditDate <= '{0}'", model.ScanEditDate2));
            }

            if (!MyUtility.Check.Empty(model.PO1))
            {
                sqlwhere.Append(string.Format(" and o.CustPONo >= '{0}'", model.PO1));
            }

            if (!MyUtility.Check.Empty(model.PO2))
            {
                sqlwhere.Append(string.Format(" and o.CustPONo <= '{0}'", model.PO2));
            }

            if (!MyUtility.Check.Empty(model.Brand))
            {
                sqlwhere.Append(string.Format(" and pl.brandid = '{0}'", model.Brand));
            }

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                sqlwhere.Append(string.Format(" and pl.MDivisionID = '{0}'", model.MDivisionID));
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                sqlwhere.Append(string.Format(" and pl.FactoryID = '{0}'", model.FactoryID));
            }

            if (!MyUtility.Check.Empty(model.ScanName))
            {
                sqlwhere.Append(string.Format(" and pld.ScanName = '{0}'", model.ScanName));
            }

            if (model.IsSummary)
            {
                sqlwhere.Append(" and (pld.ScanEditDate ='' or pld.ScanEditDate is null or pld.Lacking = 1)");
            }
            else if (model.IsDetail)
            {
                sqlwhere.Append(" and (pld.ScanEditDate !='' or pld.ScanEditDate is not null) and pld.Lacking = 0");
            }

            if (!MyUtility.Check.Empty(model.Barcode))
            {
                sqlwhere.Append(string.Format(" and pld.Barcode = '{0}'", model.Barcode));
            }
            #endregion

            return sqlwhere;
        }
    }
}
