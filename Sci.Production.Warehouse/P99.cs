using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P99 : Sci.Win.Tems.QueryForm
    {
        private string strFunction;
        private string strMaterialType_Sheet1;
        private string strTransID;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty = new Ict.Win.UI.DataGridViewNumericBoxColumn();

        /// <inheritdoc/>
        public P99(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.Initl(true);
            this.TabPage_UnLock.Parent = this.tabControl1; // 顯示
        }

        /// <inheritdoc/>
        public P99(ToolStripMenuItem menuitem, string transID, string formNo)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.SetFilter(transID, formNo);
            this.Initl(false);
            this.Query();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            // Click 按鈕[Query]需要設定, 不然Query無法判別是哪個function
            this.strFunction = this.comboFunction.SelectedValue.ToString();
            if (MyUtility.Check.Empty(this.strFunction) || MyUtility.Check.Empty(this.dateCreate.Value1) || MyUtility.Check.Empty(this.dateCreate.Value2))
            {
                MyUtility.Msg.WarningBox("<Function>,<Create Date> cannot be empty!");
                this.comboFunction.Select();
                return;
            }

            this.Query();
        }

        /// <inheritdoc/>
        public void Query()
        {
            string sqlcmd = string.Empty;
            switch (this.strFunction)
            {
                case "P07":
                    sqlcmd = $@"
select    [Selected] = 0 --0
        , [SentToWMS] = iif(t2.SentToWMS=1,'V','')
        , [CompleteTime] = t2.CompleteTime
        , t2.id
        , t2.PoId --1        
		, concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq --2
        , t2.seq1  ,t2.seq2
        , po3.FabricType 
		, t2.shipqty --4
        , t2.Weight --5
        , t2.ActualWeight --6
        , t2.Roll --7
        , t2.Dyelot --8
        , [Qty] = t2.ActualQty --9
		, t2.PoUnit --10
		, TtlQty = convert(varchar(20), --11
			iif(t2.CombineBarcode is null , t2.ActualQty, 
				iif(t2.Unoriginal is  null , ttlQty.value, null))) +' '+ t2.PoUnit
        
        , t2.StockQty--12
        , [Old_StockQty] = t2.StockQty
        , [diffQty] = 0.00
        , t2.StockUnit--13
        , t2.StockType--14
        , t2.Location--15
        , t2.remark--16
        , [RefNo]=po3.RefNo--17
		, [ColorID]=Color.Value--18
        , o.FactoryID--19
        , o.OrderTypeID--20
		, [ContainerType]= Container.Val--21
        , t2.Ukey
        ,Barcode = isnull(ft.barcode,'')
        ,t1.InvNo,t1.ETA,t1.WhseArrival
from dbo.Receiving_Detail t2 WITH (NOLOCK) 
INNER JOIN Receiving t1 WITH (NOLOCK) ON t2.id= t1.Id
left join orders o WITH (NOLOCK) on o.id = t2.PoId
LEFT JOIN PO_Supp_Detail po3  WITH (NOLOCK) ON po3.ID=t2.PoId AND po3.SEQ1=t2.Seq1 AND po3.SEQ2 = t2.Seq2
LEFT JOIN Fabric f WITH (NOLOCK) ON po3.SCIRefNo=f.SCIRefNo
left join FtyInventory ft on ft.POID = t2.PoId
                            and ft.Seq1 = t2.Seq1 
                            and ft.Seq2 = t2.Seq2
                            and ft.StockType = t2.StockType 
                            and ft.Roll =t2.Roll 
                            and ft.Dyelot = t2.Dyelot
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(isnull(po3.SuppColor,'') = '',dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
		 ELSE dbo.GetColorMultipleID(o.BrandID,po3.ColorID)
	 END
)Color
OUTER APPLY(
	SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_Detail_Ukey IN (
			SELECT Ukey
			FROM Export_Detail ed
			WHERE ed.ID = t1.ExportId
		)
		AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
		FOR XML PATH('')
	),1,1,'')
)Container
outer apply(
	select value = sum(t.ActualQty)
	from Receiving_Detail t WITH (NOLOCK) 
	where t.ID=t2.ID
	and t.CombineBarcode=t2.CombineBarcode
	and t.CombineBarcode is not null
)ttlQty
where 1=1
and t1.Type='A'
and t1.Status = 'Confirmed'
";
                    break;
                case "P08":
                    sqlcmd = @"
select    [Selected] = 0 --0
        , [SentToWMS] = iif(t2.SentToWMS=1,'V','')
        , [CompleteTime] = t2.CompleteTime
        , t2.id
        , t2.PoId --1        
        , po3.FabricType 
        , concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq--2
        , t2.seq1,t2.seq2
		, t2.Roll--3
        , t2.Dyelot--4
        , [Description] = dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0)--5        
		, t2.StockUnit--6
        , [useqty] = ( select Round(sum(dbo.GetUnitQty(b.POUnit, StockUnit, b.Qty)), 2) as useqty
            from po_supp_detail b WITH (NOLOCK) 
            where b.id= t2.poid and b.seq1 = t2.seq1 and b.seq2 = t2.seq2) --7
        , [Qty] = t2.StockQty--8
        , t2.StockQty
        , [Old_StockQty] = t2.StockQty
        , [diffQty] = 0.00
        , t2.Location--9
        , t2.Remark--10
        , t2.Ukey
        , t2.StockType
        ,t1.InvNo,t1.ETA,t1.WhseArrival
from dbo.Receiving_Detail t2 WITH (NOLOCK) 
inner join Receiving t1 WITH (NOLOCK) on t2.Id = t1.Id
left join Po_Supp_Detail po3 WITH (NOLOCK)  on t2.poid = po3.id
                              and t2.seq1 = po3.seq1
                              and t2.seq2 = po3.seq2
where 1=1
and t1.Type='B'
and t1.Status = 'Confirmed'
";
                    break;
                case "P18":
                    sqlcmd = @"
select distinct  [Selected] = 0 --0
        ,[SentToWMS] = iif(t2.SentToWMS=1,'V','')
        ,[CompleteTime] = t2.CompleteTime
        ,t2.id
        , t2.PoId--1        
        , seq = concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2)--2
        , t2.seq1,t2.seq2
		, [FabricType] = case when po3.FabricType = 'F' then 'Fabric' 
                             when po3.FabricType = 'A' then 'Accessory'
                        else '' end--3
        , t2.Roll--4
        , t2.Dyelot--5
        , [Description] = dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0)--6
		, t2.Weight--7
		, [ActualWeight] = isnull(t2.ActualWeight, 0)--8
		, t2.Qty--9
        , [Old_StockQty] = t2.Qty
        , [diffQty] = 0.00
        , StockUnit = dbo.GetStockUnitBySPSeq (t2.poid, t2.seq1, t2.seq2)--10        
        , TtlQty = convert(varchar(20),
			iif(t2.CombineBarcode is null , t2.Qty, 
				iif(t2.Unoriginal is null , ttlQty.value, null))) +' '+ dbo.GetStockUnitBySPSeq (t2.poid, t2.seq1, t2.seq2)--11
        , t2.StockType--12
        , t2.location--13
		, t2.Remark--14        
        , po3.Refno--15
		, [ColorID] = Color.Value--16        
        , t2.Ukey        
from dbo.TransferIn_Detail t2 WITH (NOLOCK) 
inner join dbo.TransferIn t1 WITH (NOLOCK)  on t1.ID=t2.Id
left join Po_Supp_Detail po3 WITH (NOLOCK)  on t2.poid = po3.id
                              and t2.seq1 = po3.seq1
                              and t2.seq2 = po3.seq2
LEFT JOIN Fabric f WITH (NOLOCK) ON po3.SCIRefNo=f.SCIRefNo
outer apply(
	select value = sum(Qty)
	from TransferIn_Detail t WITH (NOLOCK) 
	where t.ID=t2.ID
	and t.CombineBarcode=t2.CombineBarcode
	and t.CombineBarcode is not null
)ttlQty
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(isnull(po3.SuppColor,'') = '',dbo.GetColorMultipleID(po3.BrandID,po3.ColorID),po3.SuppColor)
		 ELSE dbo.GetColorMultipleID(po3.BrandID,po3.ColorID)
	 END
)Color
Where 1=1
and t1.Status = 'Confirmed'
";
                    break;
                case "P11":
                    sqlcmd = @"
        select [Selected] = 0 --0
        ,[SentToWMS] = iif(t2.SentToWMS=1,'V','')
        ,[CompleteTime] = t2.CompleteTime
        ,[seq] = concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.seq2)--1
        , t2.id
        , t2.poid
        , t2.seq1,t2.seq2
        , t2.Roll,t2.Dyelot
		, t1.Type
        , [description] = dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0)--2
        , Colorid = isnull(dbo.GetColorMultipleID(po3.BrandId, po3.ColorID), '')--3
        , po3.SizeSpec--4
        , po3.UsedQty--5
        , po3.SizeUnit--6
        , [location] = dbo.Getlocation(fi.ukey) --7
        , t2.StockType
        , [Ttl_Qty] = t2.Qty--9        
        , [Old_Qty] = t2.Qty
        , [diffQty] = 0.00
        , po3.StockUnit--10     
        , [accu_issue] = isnull(( select sum(Issue_Detail.qty) 
                                  from dbo.issue WITH (NOLOCK) 
                                  inner join dbo.Issue_Detail WITH (NOLOCK) on Issue_Detail.id = Issue.Id 
                                  where Issue.type = 'B' and Issue.Status = 'Confirmed' and issue.id != t2.Id 
                                         and Issue_Detail.poid = t2.poid and Issue_Detail.seq1 = t2.seq1 and Issue_Detail.seq2 = t2.seq2
                                         and Issue_Detail.roll = t2.roll and Issue_Detail.stocktype = t2.stocktype),0.00) --8
        , [output] = isnull ((select v.sizeqty+', ' 
                              from (
                                select (rtrim(Issue_Size.SizeCode) +'*'+convert(varchar,Issue_Size.Qty)) as sizeqty 
                                from dbo.Issue_Size WITH (NOLOCK) 
                                where   Issue_Size.Issue_DetailUkey = t2.ukey and qty <>0
                             ) v for xml path(''))
                            ,'') --11
		, t3.SizeCode
		, t3.Qty
        , t2.Ukey
        , balanceqty = isnull((fi.inqty - fi.outqty + fi.adjustqty - fi.ReturnQty),0.00)--12
        , AutoPickqty=(select SUM(AutoPickQty)  from Issue_Size iss where iss.Issue_DetailUkey = t2.ukey)--13
        , OutputAutoPick=(
			select  STUFF((
				select CONCAT(',',rtrim(iss.SizeCode),'*',iss.AutoPickQty)
				from Issue_Size iss
				where iss.Issue_DetailUkey = t2.ukey
                and iss.AutoPickQty <> 0
				for xml path('')
			),1,1,''))--14
from dbo.Issue_Detail t2 WITH (NOLOCK) 
inner join dbo.issue t1 WITH (NOLOCK)  on t2.Id = t1.Id
inner join dbo.Issue_Size t3 WITH (NOLOCK) on t2.ukey = t3.Issue_DetailUkey
left join dbo.po_supp_detail po3 WITH (NOLOCK) on po3.id  = t2.poid 
                                                and po3.seq1= t2.seq1 
                                                and po3.seq2 =t2.seq2
left join dbo.FtyInventory FI on    t2.Poid = Fi.Poid 
                                    and t2.Seq1 = fi.seq1 
                                    and t2.seq2 = fi.seq2 
                                    and t2.roll = fi.roll 
                                    and t2.stocktype = fi.stocktype
                                    and t2.Dyelot = fi.Dyelot
where  1=1  
and t1.Type='B'
and t1.Status = 'Confirmed'
";
                    break;
                case "P12":
                    sqlcmd = @"
select [Selected] = 0 --0
,[SentToWMS] = iif(t2.SentToWMS=1,'V','')
,[CompleteTime] = t2.CompleteTime
, t2.PoId--1
,[seq] = concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2)--2
, t2.seq1,t2.seq2
,t2.roll,t2.dyelot,t2.StockType
, t1.Type
,[Description] = dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0)--3
,po3.stockunit--4
,t2.Qty--5
, [Old_Qty] = t2.Qty
, [diffQty] = 0.00
,[location] = dbo.Getlocation(Fi.ukey) --6
, t2.Ukey
, t2.id
from dbo.issue_detail t2 WITH (NOLOCK) 
inner join dbo.Issue t1 WITH (NOLOCK) on t1.id = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory FI on t2.POID = FI.POID and t2.Seq1 = FI.Seq1 and t2.Seq2 = FI.Seq2 and FI.Dyelot = t2.Dyelot
    and t2.Roll = FI.Roll and FI.stocktype = 'B'   
where 1=1
and t1.Type='C'
and t1.Status = 'Confirmed'
";
                    break;
                case "P13":
                    sqlcmd = @"
select  [Selected] = 0 --0
,[SentToWMS] = iif(t2.SentToWMS=1,'V','')
,[CompleteTime] = t2.CompleteTime
, o.FtyGroup --1
, t2.PoId--2
, concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq--3
, t2.seq1,t2.seq2
, t2.Roll--4
, t2.Dyelot--5
, t2.StockType
, t1.Type
, dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0) as [description]--6
, po3.stockunit--7
, [Article] = case  when t2.Seq1 like 'T%' then Stuff((Select distinct concat( ',',tcd.Article) 
	From dbo.Orders as o 
	Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
	Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
	Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey 
	where	o.POID = t2.PoId and
	tcd.SuppId = p.SuppId and
	tcd.SCIRefNo   = po3.SCIRefNo	and
	tcd.ColorID	   = po3.ColorID
	FOR XML PATH('')),1,1,'') 
	else '' end--8
, po3.NetQty--9
, po3.LossQty--10
, t2.Qty--11, 
, [Old_Qty] = t2.Qty
, [diffQty] = 0.00
,t2.id
,dbo.Getlocation(c.ukey) location--12
, Isnull(c.inqty - c.outqty + c.adjustqty - c.ReturnQty, 0.00) as balance--13
, t2.Ukey
from dbo.issue_detail as t2 WITH (NOLOCK) 
inner join issue t1 WITH (NOLOCK) on t2.Id=t1.Id
left join Orders o on t2.poid = o.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join PO_Supp p WITH (NOLOCK) on p.ID = po3.ID and po3.seq1 = p.SEQ1
left join dbo.ftyinventory c WITH (NOLOCK) on c.poid = t2.poid and c.seq1 = t2.seq1 and c.seq2  = t2.seq2 
and c.stocktype = 'B' and c.roll=t2.roll and t2.Dyelot = c.Dyelot
Where 1=1
and t1.Type='D'
and t1.Status = 'Confirmed'
";
                    break;
                case "P33":
                    #region P33 sqlcmd
                    sqlcmd = @"
----  先By Article撈取資料再加總
WITH BreakdownByArticle as (

         SELECT   DISTINCT
			  [Selected] = 0 --0
			, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
			, [CompleteTime] = t2.CompleteTime
            , iis.SCIRefno
            , [Refno]=Refno.Refno
            , iis.ColorID
		    , iis.SuppColor
		    , f.DescDetail
		    , [@Qty]= ISNULL(ThreadUsedQtyByBOT.Val,0)
		    , [AccuIssued] = (
					    select isnull(sum([IS].qty),0)
					    from dbo.issue I2 WITH (NOLOCK) 
					    inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I2.id = [IS].Id 
					    where I2.type = 'E' and I2.Status = 'Confirmed' 
					    and [IS].Poid=iis.POID AND [IS].SCIRefno=iis.SCIRefno AND [IS].ColorID=iis.ColorID and i2.[EditDate]<t1.AddDate AND i2.ID <> t1.ID
				    )
		    , [Ttl_Qty]=iis.Qty
		    , [Use Qty By Stock Unit] = CEILING( ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ISNULL(ThreadUsedQtyByBOT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1))
		    , [Stock Unit]=StockUnit.StockUnit
		    , [Use Unit]='CM'
		    , [Use Qty By Use Unit]=(ThreadUsedQtyByBOT.Qty * ISNULL(ThreadUsedQtyByBOT.Val,0) )
		    , [Stock Unit Desc.]=StockUnit.Description
		    , [OutputQty] = ISNULL(ThreadUsedQtyByBOT.Qty,0)
		    , [Balance(Stock Unit)]= 0
		    , [Location]=''
            , [POID]=iis.POID
            , t1.MDivisionID
            , t1.ID
            , iis.Ukey
			, [Qty] = t2.Qty			
			, t2.Seq1,t2.Seq2,t2.Roll,t2.Dyelot,t2.StockType, t1.Type
    FROM Issue t1
    INNER JOIN Issue_Summary iis ON t1.ID= iis.Id
    LEFT JOIN Issue_Detail t2 ON t2.Issue_SummaryUkey=iis.Ukey
	LEFT JOIN PO_Supp_Detail po3 on po3.ID = t2.POID and po3.SEQ1 = t2.Seq1 and po3.SEQ2 = t2.Seq2 
    LEFT JOIN Fabric f ON f.SCIRefno = iis.SCIRefno
    OUTER APPLY(
	    SELECT DISTINCT Refno
	    FROM PO_Supp_Detail psd
	    WHERE psd.ID = iis.POID AND psd.SCIRefno = iis.SCIRefno 
	    AND psd.ColorID=iis.ColorID
    )Refno
    OUTER APPLY(
	    SELECT SCIRefNo
		    ,ColorID
		    ,[Val]=SUM(((SeamLength  * Frequency * UseRatio ) + (Allowance*Segment))) 
		    ,[Qty] = (	
			    SELECt [Qty]=SUM(b.Qty)
			    FROM (
					    Select distinct o.ID,tcd.SCIRefNo, tcd.ColorID ,tcd.Article 
					    --INTO #step1
					    From dbo.Orders as o
					    Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
					    Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
					    Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
					    WHERE O.ID=iis.POID AND tcd.Article IN ( SELECT Article FROM Issue_Breakdown WHERE ID=t1.ID)
					    ) a
			    INNER JOIN (		
							    SELECt Article,[Qty]=SUM(Qty) 
							    FROM Issue_Breakdown
							    WHERE ID=t1.ID
							    GROUP BY Article
						    ) b ON a.Article = b.Article
			    WHERE SCIRefNo=iis.SCIRefNo AND  ColorID= iis.ColorID AND a.Article=g.Article
			    GROUP BY a.Article
		    )
	    FROM DBO.GetThreadUsedQtyByBOT(iis.POID) g
	    WHERE SCIRefNo= iis.SCIRefNo AND ColorID = iis.ColorID  
	    AND Article IN (		
            SELECt Article
            FROM Issue_Breakdown
            WHERE ID=t1.ID
	    )
	    GROUP BY SCIRefNo,ColorID , Article
    )ThreadUsedQtyByBOT
    OUTER APPLY(
	    SELECT TOP 1 psd2.StockUnit ,u.Description
	    FROM PO_Supp_Detail psd2
	    LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	    WHERE psd2.ID = t1.OrderId 
	    AND psd2.SCIRefno = iis.SCIRefno 
	    AND psd2.ColorID = iis.ColorID
    )StockUnit
    OUTER APPLY(
	    SELECT RateValue
	    FROM Unit_Rate
	    WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
    )UnitRate
    WHERE 1=1
    AND iis.ColorID <> ''
    AND t1.Type='E'
    and t1.Status = 'Confirmed'
";
                    if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
                    {
                        sqlcmd += $@" and t2.Poid >= '{this.txtSPNoStart.Text}'" + Environment.NewLine;
                    }

                    if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
                    {
                        sqlcmd += $@" and t2.Poid <= '{this.txtSPNoEnd.Text}'" + Environment.NewLine;
                    }

                    if (!MyUtility.Check.Empty(this.strTransID))
                    {
                        sqlcmd += $@" and t2.id = '{this.strTransID}'";
                    }

                    if (!MyUtility.Check.Empty(this.dateCreate.Value1) && !MyUtility.Check.Empty(this.dateCreate.Value2))
                    {
                        sqlcmd += $@" and t1.AddDate between '{Convert.ToDateTime(this.dateCreate.Value1).ToString("yyyy/MM/dd")}' and '{Convert.ToDateTime(this.dateCreate.Value2).ToString("yyyy/MM/dd")}'";
                    }

                    if (!MyUtility.Check.Empty(this.strMaterialType_Sheet1))
                    {
                        sqlcmd += $@" and po3.FabricType = '{this.strMaterialType_Sheet1}'";
                    }

                    sqlcmd +=
                   @")

, Final as(

	SELECt 
		[Selected] = 0 --0
		, [SentToWMS]
		, [CompleteTime]
		, SCIRefno
		, Refno
        , ColorID
		, SuppColor
		, DescDetail
		, [@Qty] = SUM([@Qty])
		, [AccuIssued]
		, [Qty] = t.Qty
		, Ttl_Qty
		, [Use Qty By Stock Unit] = CEILING (SUM([Use Qty By Stock Unit] ))
		, [Stock Unit]
		, [Use Unit]='CM'
		, [Use Qty By Use Unit] = SUM([Use Qty By Use Unit])
		, [Stock Unit Desc.]
		, [OutputQty] = SUM([OutputQty])
		, [Balance(Stock Unit)] 
		, [Location]
		, [POID]
		, MDivisionID
		, ID
		, Ukey
		, Seq1,Seq2,Roll,Dyelot,StockType,Type
	FROM BreakdownByArticle t
	GROUP BY SCIRefno
		, Refno
        , ColorID
		, SuppColor
		, DescDetail
		, [AccuIssued]
		, [Qty]
		, [Stock Unit]
		, [Use Unit]
		, [Stock Unit Desc.]
		, [Balance(Stock Unit)]
		, [Location]
		, [POID]
		, MDivisionID
		, ID
		, Ukey
		,[Selected]
		, [SentToWMS]
		, [CompleteTime]
		, Seq1,Seq2,Roll,Dyelot,StockType,Type
		,Ttl_Qty

)

SELECt [Selected] = 0 --0
		, [SentToWMS]
		, [CompleteTime]
		, SCIRefno
		, Refno
        , ColorID
		, SuppColor
		, DescDetail
		, [@Qty]
		, [AccuIssued]
		, [Qty] = t.Qty
		, Ttl_Qty
		, [Old_Qty] = t.Qty
		, [diffQty] = 0.00
		, [Use Qty By Stock Unit]
		, [Stock Unit]
		, [Use Unit]='CM'
		, [Use Qty By Use Unit]
		, [Stock Unit Desc.]
		, [OutputQty]
		, [Balance(Stock Unit)] = Balance.Qty
		, [Location]=Location.MtlLocationID
		, [POID]
		, MDivisionID
		, ID
		, Ukey
		, Seq1,Seq2,Roll,Dyelot,StockType,Type
FROM final t
OUTER APPLY(
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty )) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID=t.ColorID AND psd.ID=t.POID
)Balance
OUTER APPLY(

	    SELECT   [MtlLocationID] = STUFF(
	    (
		    SELECT DISTINCT ',' +fid.MtlLocationID 
		    FROM Issue_Detail 
		    INNER JOIN FtyInventory FI ON FI.POID=Issue_Detail.POID AND FI.Seq1=Issue_Detail.Seq1 AND FI.Seq2=Issue_Detail.Seq2
		    INNER JOIN FtyInventory_Detail FID ON FID.Ukey= FI.Ukey
		    WHERE Issue_Detail.ID = t.ID AND  FI.StockType='B' AND  fid.MtlLocationID  <> '' AND Issue_SummaryUkey= t.Ukey 
		    FOR XML PATH('')
	    ), 1, 1, '') 
)Location

";
                    #endregion
                    break;
                case "P15":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id,t2.PoId,t2.Seq1,t2.Seq2,concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq
,t2.Roll,t2.Dyelot,t2.StockType
,po3.stockunit
,dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0) as [Description]
,t2.Qty
, [Old_Qty] = t2.Qty
, [diffQty] = 0.00
,t2.StockType
,dbo.Getlocation(f.Ukey)  as location
,t2.ukey
,t2.FtyInventoryUkey
,t1.Type,t1.FabricType
from dbo.IssueLack_Detail t2 WITH (NOLOCK) 
inner join dbo.IssueLack t1 WITH (NOLOCK) on t1.Id = t2.Id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory f WITH (NOLOCK) on t2.POID=f.POID and t2.Seq1=f.Seq1 and t2.Seq2=f.Seq2 and t2.Roll=f.Roll and t2.Dyelot=f.Dyelot and t2.StockType=f.StockType
where 1=1
and po3.FabricType='A'
and t1.Status = 'Confirmed'
";
                    break;
                case "P16":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id
, t2.PoId
, t2.Seq1
, t2.Seq2
, t2.StockType
, seq = concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2)
, t2.Roll
, t2.Dyelot
, po3.stockunit
, [Description] = dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0) 
, t2.Qty
, [Old_Qty] = t2.Qty
, [diffQty] = 0.00
, t2.StockType
, location = dbo.Getlocation(f.Ukey) 
, t2.ukey
, t2.FtyInventoryUkey
, t2.Remark
from dbo.IssueLack_Detail t2 WITH (NOLOCK) 
inner join dbo.IssueLack t1 WITH (NOLOCK) on t1.Id = t2.Id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId 	and po3.seq1 = t2.SEQ1 
										and po3.SEQ2 = t2.seq2
left join FtyInventory f WITH (NOLOCK) on t2.POID = f.POID 
									and t2.Seq1 = f.Seq1 
									and t2.Seq2 = f.Seq2 
									and t2.Roll = f.Roll 
									and t2.Dyelot = f.Dyelot 
									and t2.StockType = f.StockType
where 1=1
and po3.FabricType='F'
and t1.Status = 'Confirmed'
";
                    break;
                case "P19":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id,t2.PoId,t2.Seq1,t2.Seq2,concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq
,t2.Roll
,t2.Dyelot
,t2.StockType
,dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0) as [Description]
,po3.StockUnit
,t2.Qty
, [Old_Qty] = t2.Qty
, [diffQty] = 0.00
,t2.StockType
,t2.ukey
,dbo.Getlocation(fi.ukey) location
,t2.ftyinventoryukey
,t2.ToPOID
,t2.ToSeq1
,t2.ToSeq2
,[ToSeq] = t2.ToSeq1 +' ' + t2.ToSeq2
from dbo.TransferOut_Detail t2 WITH (NOLOCK) 
inner join TransferOut t1 WITH (NOLOCK) on t1.Id = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory FI on t2.POID = FI.POID and t2.Seq1 = FI.Seq1 and t2.Seq2 = FI.Seq2 and t2.Dyelot = FI.Dyelot
    and t2.Roll = FI.Roll and t2.StockType = FI.StockType
where 1=1
and t1.Status = 'Confirmed'
";
                    break;
                case "P34":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id,t2.PoId,t2.Seq1,t2.Seq2,t2.Roll,t2.Dyelot
,concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq
,po3.FabricType
,po3.stockunit
,dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0) as [description]
,qty = t2.QtyAfter
, [Old_Qty] = t2.QtyAfter
, [diffQty] = 0.00
,t2.QtyBefore
,[adjustqty] = isnull(t2.QtyAfter,0.00) - isnull(t2.QtyBefore,0.00)
,t2.ReasonId
,(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND ID= t2.ReasonId) reason_nm
,t2.StockType
,dbo.Getlocation(fi.ukey) location
,t2.ukey
,t2.ftyinventoryukey
,ColorID =dbo.GetColorMultipleID(po3.BrandId, po3.ColorID)
from dbo.Adjust_Detail t2 WITH (NOLOCK) 
inner join dbo.Adjust t1 WITH (NOLOCK) on t1.ID = t2.ID
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory FI on t2.poid = fi.poid and t2.seq1 = fi.seq1 and t2.seq2 = fi.seq2
    and t2.roll = fi.roll and t2.stocktype = fi.stocktype and t2.Dyelot = fi.Dyelot
where 1=1
and t1.Type = 'B'
and t1.Status = 'Confirmed'
";
                    break;
                case "P35":
                    sqlcmd = @"
select [Selected] = 0 --0
			, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
			, [CompleteTime] = t2.CompleteTime
	,t2.id,t2.PoId,t2.Seq1,t2.Seq2
    ,concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq
    ,po3.FabricType
    ,po3.stockunit
    ,t2.Roll
    ,t2.Dyelot
    ,[qty] = t2.QtyAfter
    , [Old_Qty] = t2.QtyAfter
    , [diffQty] = 0.00
    ,t2.QtyBefore
    ,isnull(t2.QtyAfter,0.00) - isnull(t2.QtyBefore,0.00) adjustqty
    ,t2.ReasonId
    ,(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND ID= t2.ReasonId) reason_nm
    ,t2.StockType
    ,t2.ukey
    ,t2.ftyinventoryukey
    ,[location] = Getlocation.Value 
    ,[ColorID] = ColorID.Value
    ,[Description] = dbo.getmtldesc(po3.id,po3.seq1,po3.seq2,2,0)
from dbo.Adjust_Detail t2 WITH (NOLOCK) 
inner join dbo.Adjust t1 WITH (NOLOCK)  on t1.ID = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory FI on t2.poid = fi.poid and t2.seq1 = fi.seq1 and t2.seq2 = fi.seq2
    and t2.roll = fi.roll and t2.stocktype = fi.stocktype and t2.Dyelot = fi.Dyelot
outer apply (
    select   [Value] = stuff((	select ',' + MtlLocationID
									from (	
										select d.MtlLocationID	
										from dbo.FtyInventory_Detail d WITH (NOLOCK) 
										where	ukey =   fi.ukey
												and d.MtlLocationID != ''
												and d.MtlLocationID is not null) t
									for xml path('')) 
								, 1, 1, '')
) as Getlocation
outer apply (
	select	[Value] = stuff((select '/' + m.ColorID 
			   from dbo.Color as c 
			   LEFT join dbo.Color_multiple as m on m.ID = c.ID 
				  								    and m.BrandID = c.BrandId 
			   where c.ID = po3.ColorID and c.BrandId =  po3.BrandId 
			   order by m.Seqno 
			   for xml path('') ) 
			 , 1, 1, '')  
) as ColorID
Where 1=1
and t1.Type='A'
and t1.Status = 'Confirmed'
";
                    break;
                case "P43":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime,
t2.id,
[Seq]= t2.Seq1+' '+t2.Seq2,
t2.seq1,
t2.seq2,
t2.POID,
t2.Roll,
t2.Dyelot,
t2.StockType,
t2.MDivisionID,
Fa.Description,
t2.QtyBefore,
t2.QtyAfter,
Qty = t2.QtyAfter,
[Old_Qty] = t2.QtyAfter,
[diffQty] = 0.00,
[AdjustQty] = t2.QtyAfter - t2.QtyBefore,
Po3.StockUnit,
[location]= dbo.Getlocation(FTI.Ukey),
t2.ReasonId,
[reason_nm]=Reason.Name,
t2.Ukey,
ColorID =dbo.GetColorMultipleID(PO3.BrandId, PO3.ColorID)
from Adjust_Detail t2
inner join Adjust t1 on t2.ID = t1.id
inner join PO_Supp_Detail PO3 on PO3.ID=t2.POID 
inner join FtyInventory FTI on FTI.POID=t2.POID and FTI.Seq1=t2.Seq1
	and FTI.Seq2=t2.Seq2 and FTI.Roll=t2.Roll and FTI.StockType='O'
and PO3.SEQ1=t2.Seq1 and PO3.SEQ2=t2.Seq2 and FTI.Dyelot = t2.Dyelot
outer apply (
	select Description from Fabric where SCIRefno=PO3.SCIRefno
) Fa
outer apply(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND ID= t2.ReasonId and junk=0 ) Reason
where 1=1
and t1.Type = 'O'
and t1.Status = 'Confirmed'
";
                    break;
                case "P45":
                    sqlcmd = @"
select 
	[Selected] = 0 --0
	, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
	, [CompleteTime] = t2.CompleteTime
	,t2.POID
    ,t1.id
	,seq = concat(t2.Seq1,'-',t2.Seq2)
    ,t2.Seq1,t2.Seq2
	,t2.Roll
	,t2.Dyelot
    ,t2.StockType
	,Description = dbo.getmtldesc(t2.POID, t2.Seq1, t2.Seq2, 2, 0)
	,t2.QtyBefore
	,Qty = t2.QtyAfter
    ,[Old_Qty] = t2.QtyAfter
    ,[diffQty] = 0.00
    ,t2.QtyAfter
	,adjustqty= t2.QtyBefore-t2.QtyAfter
	,po3.StockUnit
	,Location = dbo.Getlocation(fi.ukey)
	,t2.ReasonId
	,reason_nm = (select Name FROM Reason WHERE id=ReasonId AND junk = 0 and ReasonTypeID='Stock_Remove')
    ,ColorID =dbo.GetColorMultipleID(po3.BrandId, po3.ColorID)
    ,t2.ukey
from Adjust_detail t2 WITH (NOLOCK) 
inner join Adjust t1 WITH (NOLOCK) on t1.ID = t2.ID
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.id = t2.POID and po3.SEQ1 = t2.Seq1 and po3.SEQ2 = t2.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = po3.SCIRefno
left join FtyInventory fi WITH (NOLOCK) on fi.POID = t2.POID and fi.Seq1 = t2.Seq1 and fi.Seq2 = t2.Seq2 and fi.Roll = t2.Roll and fi.StockType = t2.StockType and fi.Dyelot = t2.Dyelot
where 1=1
and t1.Type = 'R'
and t1.Status = 'Confirmed'
";
                    break;
                case "P22":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id
,t2.FromPoId
,t2.FromSeq1,t2.FromSeq2
,t2.FromRoll
,t2.FromDyelot
,t2.FromStocktype
,concat(Ltrim(Rtrim(t2.FromSeq1)), ' ', t2.FromSeq2) as Fromseq
,po3.FabricType
,po3.stockunit
,dbo.getmtldesc(t2.FromPoId,t2.FromSeq1,t2.FromSeq2,2,0) as [description]
,t2.Qty
,[Old_Qty] = t2.Qty
,[diffQty] = 0.00
,t2.ToPoid,t2.ToSeq1,t2.ToSeq2
,concat(Ltrim(Rtrim(t2.ToSeq1)), ' ', t2.ToSeq2) as toseq
,t2.ToRoll
,t2.ToDyelot
,t2.ToStocktype
,t2.ToLocation
,t2.fromftyinventoryukey
,t2.ukey
,dbo.Getlocation(fi.ukey) location
,t1.Type
from dbo.SubTransfer_detail t2 WITH(NOLOCK) 
inner join SubTransfer t1 WITH(NOLOCK)  on t1.Id = t2.id	
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 and po3.SEQ2 = t2.FromSeq2
left join FtyInventory FI on t2.fromPoid = fi.poid and t2.fromSeq1 = fi.seq1 and t2.fromSeq2 = fi.seq2 and t2.fromDyelot = fi.Dyelot
    and t2.fromRoll = fi.roll and t2.fromStocktype = fi.stocktype
Where 1=1
and t1.Type='A'
and t1.Status = 'Confirmed'
";
                    break;
                case "P23":
                    sqlcmd = @"
select    [Selected] = 0 --0
		, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
		, [CompleteTime] = t2.CompleteTime
		,t2.id    
        , t2.FromPoId
        , t2.FromSeq1
        , t2.FromSeq2
        , Fromseq = concat (Ltrim (Rtrim (t2.FromSeq1)), ' ', t2.FromSeq2)
        , po3.FabricType
        , stockunit = dbo.GetStockUnitBySPSeq (po3.ID, po3.seq1, po3.seq2)
        , [description] = dbo.getmtldesc (t2.FromPoId, t2.FromSeq1, t2.FromSeq2, 2, 0)
        , t2.FromRoll
        , t2.FromDyelot
        , t2.FromStocktype
        , t2.Qty
        , [Old_Qty] = t2.Qty
        , [diffQty] = 0.00
        , t2.ToPoid
        , t2.ToSeq1
        , t2.ToSeq2
        , toseq = concat (Ltrim (Rtrim (t2.ToSeq1)), ' ', t2.ToSeq2)
        , t2.ToRoll
        , t2.ToDyelot
        , t2.ToStocktype
        , t2.ToLocation
        , t2.fromftyinventoryukey
        , t2.ukey
        , location = dbo.Getlocation (fi.ukey)
        , t1.Type
from dbo.SubTransfer_detail t2 WITH (NOLOCK) 
inner join dbo.SubTransfer t1 WITH (NOLOCK)  on t1.Id = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId 
                                             and po3.seq1 = t2.FromSeq1 
                                             and po3.SEQ2 = t2.FromSeq2
left join FtyInventory FI on t2.FromPoid = fi.poid 
                             and t2.fromSeq1 = fi.seq1 
                             and t2.fromSeq2 = fi.seq2
                             and t2.fromRoll = fi.roll 
                             and t2.fromStocktype = fi.stocktype
                             and t2.fromDyelot = fi.Dyelot
Where 1=1
and t1.Type = 'B'
and t1.Status = 'Confirmed'
";
                    break;
                case "P24":
                    sqlcmd = @"
select [Selected] = 0 --0
	, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
	, [CompleteTime] = t2.CompleteTime 
    , t2.id
    ,t2.FromFtyinventoryUkey
    ,t2.FromPoId
    ,t2.FromSeq1
    ,t2.FromSeq2
    ,FromSeq = concat(Ltrim(Rtrim(t2.FromSeq1)), ' ', t2.FromSeq2)
    ,FabricType = Case po3.FabricType WHEN 'F' THEN 'Fabric' WHEN 'A' THEN 'Accessory' ELSE 'Other'  END 
    ,po3.stockunit
    ,description = dbo.getmtldesc(t2.FromPoId,t2.FromSeq1,t2.FromSeq2,2,0)
    ,t2.FromRoll
    ,t2.FromDyelot
    ,t2.FromStockType
    ,t2.Qty
    ,[Old_Qty] = t2.Qty
    ,[diffQty] = 0.00
    ,t2.ToPoId
    ,t2.ToSeq1
    ,t2.ToSeq2
    ,t2.ToDyelot
    ,t2.ToRoll
    ,t2.ToStockType
    ,dbo.Getlocation(f.Ukey)  as Fromlocation
    ,t2.ukey
    ,t2.tolocation
    ,t1.Type
from dbo.SubTransfer_Detail t2 WITH (NOLOCK) 
inner join SubTransfer t1 WITH (NOLOCK)  on t1.Id = t2.id	
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 and po3.SEQ2 = t2.FromSeq2
left join FtyInventory f WITH (NOLOCK) on t2.FromPOID=f.POID and t2.FromSeq1=f.Seq1 and t2.FromSeq2=f.Seq2 and t2.FromRoll=f.Roll and t2.FromDyelot=f.Dyelot and t2.FromStockType=f.StockType
Where 1=1
and t1.Type = 'E'
and t1.Status = 'Confirmed'
";
                    break;
                case "P36":
                    sqlcmd = @"
select 
[Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id
,t2.FromFtyinventoryUkey
,t2.FromPoId
,t2.FromSeq1
,t2.FromSeq2
,concat(Ltrim(Rtrim(t2.FromSeq1)), ' ', t2.FromSeq2) as FromSeq
,po3.FabricType
,po3.stockunit
,dbo.getmtldesc(t2.FromPoId,t2.FromSeq1,t2.FromSeq2,2,0) as [description]
,t2.FromRoll
,t2.FromDyelot
,t2.FromStockType
,t2.Qty
,[Old_Qty] = t2.Qty
,[diffQty] = 0.00
,t2.ToPoId
,t2.ToSeq1
,t2.ToSeq2
,t2.ToDyelot
,t2.ToRoll
,t2.ToStockType
,t2.ToLocation
,t2.ukey
,t1.Type
from dbo.SubTransfer_Detail t2 WITH (NOLOCK) 
inner join SubTransfer t1 WITH (NOLOCK) on t1.Id = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 
and po3.SEQ2 = t2.FromSeq2
Where 1=1
and t1.Type='C'
and t1.Status = 'Confirmed'
";
                    break;
                case "P37":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id,t2.PoId,t2.Seq1,t2.Seq2,concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq
,t2.Roll
,t2.Dyelot
,po3.stockunit
,dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0) as [Description]
,t2.Qty
,[Old_Qty] = t2.Qty
,[diffQty] = 0.00
,t2.StockType
,dbo.Getlocation(fi.ukey) location
,t2.ukey
,t2.FtyInventoryUkey
from dbo.ReturnReceipt_Detail t2 WITH (NOLOCK) 
inner join ReturnReceipt t1 WITH (NOLOCK)  on t1.Id = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory FI on t2.poid = fi.poid and t2.seq1 = fi.seq1 and t2.seq2 = fi.seq2
    and t2.roll = fi.roll and t2.stocktype = fi.stocktype and t2.Dyelot = fi.Dyelot 
Where 1=1
and t1.Status = 'Confirmed'
";
                    break;
                case "P31":
                    sqlcmd = @"
select  [Selected] = 0 --0
		, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
		, [CompleteTime] = t2.CompleteTime
		,t2.id
        ,t2.FromPoId
        ,t2.FromSeq1
        ,t2.FromSeq2
        ,Fromseq = concat(Ltrim(Rtrim(t2.FromSeq1)), ' ', t2.FromSeq2) 
        ,po3.FabricType
        ,po3.stockunit
        ,[description] = dbo.getmtldesc(t2.FromPoId,t2.FromSeq1,t2.FromSeq2,2,0) 
        ,t2.FromRoll
        ,t2.FromDyelot
        ,t2.FromStocktype
        ,t2.Qty
        ,[Old_Qty] = t2.Qty
        ,[diffQty] = 0.00
        ,t2.ToPoid
        ,t2.ToSeq1
        ,t2.ToSeq2
        ,toseq = concat(Ltrim(Rtrim(t2.ToSeq1)), ' ', t2.ToSeq2)
        ,t2.ToRoll
        ,t2.ToDyelot
        ,t2.ToStocktype
        ,t2.fromftyinventoryukey
        ,t2.ukey
        ,location = stuff((select ',' + mtllocationid 
                           from (select mtllocationid 
                                 from dbo.ftyinventory_detail fd WITH (NOLOCK) 
                                 where ukey= FI.ukey)t 
                           for xml path(''))
                          , 1, 1, '') 
        ,t2.ToLocation
from dbo.BorrowBack_detail t2 WITH (NOLOCK) 
inner join BorrowBack t1 WITH (NOLOCK) on t1.Id = t2.id	
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 and po3.SEQ2 = t2.FromSeq2
left join FtyInventory FI on t2.FromPoid = Fi.Poid and t2.FromSeq1 = Fi.Seq1 and t2.FromSeq2 = Fi.Seq2 
    and t2.FromRoll = Fi.Roll and t2.FromDyelot = Fi.Dyelot and t2.FromStockType = StockType
Where 1=1
and t1.Type='A'
and t1.Status = 'Confirmed'
";
                    break;
                case "P32":
                    sqlcmd = @"
select  [Selected] = 0 --0
		, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
		, [CompleteTime] = t2.CompleteTime
		,t2.id
        ,t2.FromFtyinventoryUkey
        ,t2.FromPoId
        ,t2.FromSeq1
        ,t2.FromSeq2
        ,Fromseq = concat(Ltrim(Rtrim(t2.FromSeq1)), ' ', t2.FromSeq2) 
        ,po3.FabricType
        ,po3.stockunit
        ,[description] = dbo.getmtldesc(t2.FromPoId,t2.FromSeq1,t2.FromSeq2,2,0) 
        ,t2.FromRoll
        ,t2.FromDyelot
        ,t2.FromStocktype
        ,t2.Qty
        ,[Old_Qty] = t2.Qty
        ,[diffQty] = 0.00
        ,t2.ToPoid
        ,t2.ToSeq1
        ,t2.ToSeq2
        ,toseq = concat(Ltrim(Rtrim(t2.ToSeq1)), ' ', t2.ToSeq2)
        ,t2.ToRoll
        ,t2.ToDyelot
        ,t2.ToStocktype
        ,t2.ukey
        ,location = dbo.Getlocation(fi.ukey)
        ,t2.ToLocation
from dbo.BorrowBack_detail t2 WITH (NOLOCK) 
inner join dbo.BorrowBack t1 WITH (NOLOCK) on t1.Id = t2.ID
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 and po3.SEQ2 = t2.FromSeq2
left join FtyInventory FI on t2.fromPoid = fi.poid and t2.fromSeq1 = fi.seq1 and t2.fromSeq2 = fi.seq2 and t2.fromDyelot = fi.Dyelot 
    and t2.fromRoll = fi.roll and t2.fromStocktype = fi.stocktype
Where 1=1
and t1.Type='B'
and t1.Status = 'Confirmed'
";
                    break;
                case "P62":
                    sqlcmd = @"
select  [Selected] = 0 --0
		, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
		, [CompleteTime] = t2.CompleteTime
	,[FabricType] = isnull(po3.FabricType,'')
	,[GridPoID] = s.Poid
	,[GridSeq1] = s.seq1
	,[GridSeq2] = s.seq2
	,POID = isnull(t2.POID,'')
	,[Seq1] = isnull(t2.Seq1,'')
	,[Seq2] = isnull(t2.Seq2,'')
	,[Roll] = isnull(t2.Roll,'')
	,[Dyelot] = isnull(t2.Dyelot,'')
    ,[StockType] = isnull(t2.StockType,'')
	,s.Ukey
	,s.Id
	,[Ttl_Qty] = s.Qty
	,[Qty] = isnull(t2.Qty,0.00)
	,[Old_Qty] = isnull(t2.Qty,0.00)
    ,[diffQty] = 0.00
    , f.Refno
	, [description] = f.DescDetail
	, [requestqty] = isnull(ec.RequestQty, 0.00)
	, [accu_issue] =isnull (accu.accu_issue, 0.00)
    , unit = (select top 1 StockUnit from Po_Supp_Detail psd where psd.Id = s.Poid and psd.SciRefno = s.SciRefno)
	, NetQty = isnull( Net.NETQty, 0)
    , [FinalFIR] = (
		SELECT Stuff((
			select concat( '/',isnull(Result,' '))   
			from dbo.FIR f with (nolock) 
			where f.poid = s.poid and f.SCIRefno = s.SCIRefno
			and exists(select 1 from Issue_Detail with (nolock) where Issue_SummaryUkey = s.Ukey and f.seq1 = seq1 and f.seq2 = seq2)
			FOR XML PATH('')
		),1,1,'') )
	, arqty = ec.RequestQty + AccuReq.ReqQty
	, aiqqty = AccuIssue.aiqqty
	, avqty = (ec.RequestQty + AccuReq.ReqQty) - AccuIssue.aiqqty	
from dbo.Issue_Summary s WITH (NOLOCK) 
left join Issue_Detail t2 WITH (NOLOCK) on t2.Id = s.Id and t2.Issue_SummaryUkey = s.Ukey
inner join issue t1 WITH (NOLOCK) on t1.Id = s.Id
left join Fabric f on s.SciRefno = f.SciRefno
outer apply(
	select top 1 po.FabricType
	from PO_Supp_Detail po
	inner join Issue_Detail is2 on po.ID = is2.POID and po.SEQ1 = is2.Seq1
	and po.SEQ2 = is2.Seq2 
	where is2.Issue_SummaryUkey = s.Ukey and is2.Id = s.Id
)po3
outer apply(
	select RequestQty = sum(c.ReleaseQty) 
	from dbo.CutTapePlan_Detail c WITH (NOLOCK) 
	where c.id = t1.CutplanID
	and c.Seq1 = s.Seq1
	and c.Seq2 = s.Seq2
) ec
outer apply(
	select accu_issue = isNull(sum(qty) , 0.00)
	from Issue a WITH (NOLOCK) 
	inner join Issue_Summary b WITH (NOLOCK) on a.Id=b.Id 
	where b.poid = s.poid 
			and a.CutplanID = t1.CutplanID
			and b.SCIRefno = s.SCIRefno 
			and b.Colorid = s.Colorid 
			and a.status = 'Confirmed'
			and a.id != s.id
			and a.Type = 'I'
) accu
outer apply(
	select top 1 p.NETQty
	from PO_Supp_Detail p
	where p.ID = s.POID and p.SCIRefno = s.SCIRefno and p.ColorID = s.ColorID
			and p.SEQ1 like 'A%' and p.NETQty <> 0
) Net
outer apply(select CuttingID from CutTapePlan where Id = t1.CutplanID )c
outer apply(
	select ReqQty = isNull(sum(cd.ReleaseQty), 0)
	from CutTapePlan c
	inner join CutTapePlan_Detail cd on c.id = cd.id
	inner join Issue i on i.CutplanID = c.ID
	where c.CuttingID = c.CuttingID
	and c.id <> t1.CutplanID
	and i.Status = 'Confirmed'
	and i.Type = 'I'
)AccuReq
outer apply(
	select aiqqty = isnull(sum(sm.Qty), 0)
    from Issue_Summary sm WITH (NOLOCK) 
    inner join Issue i WITH (NOLOCK) on sm.Id = i.Id 
    where sm.Poid = s.poid 
            and sm.SCIRefno = s.SCIRefno 
            and sm.Colorid = s.ColorID 
            and i.status = 'Confirmed'
			and i.Type = 'I'
)AccuIssue
where t1.Status = 'Confirmed'
and t1.Type='I'
";
                    break;
                default:
                    break;
            }

            if (this.strFunction != "P33")
            {
                switch (this.strFunction)
                {
                    case "P22":
                    case "P23":
                    case "P24":
                    case "P36":
                    case "P31":
                    case "P32":
                        if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
                        {
                            sqlcmd += $@" and t2.FromPoId >= '{this.txtSPNoStart.Text}'" + Environment.NewLine;
                        }

                        if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
                        {
                            sqlcmd += $@" and t2.FromPoId <= '{this.txtSPNoEnd.Text}'" + Environment.NewLine;
                        }

                        break;
                    default:
                        if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
                        {
                            sqlcmd += $@" and t2.Poid >= '{this.txtSPNoStart.Text}'" + Environment.NewLine;
                        }

                        if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
                        {
                            sqlcmd += $@" and t2.Poid <= '{this.txtSPNoEnd.Text}'" + Environment.NewLine;
                        }

                        break;
                }

                if (!MyUtility.Check.Empty(this.strTransID))
                {
                    sqlcmd += $@" and t2.id = '{this.strTransID}'";
                }

                if (!MyUtility.Check.Empty(this.dateCreate.Value1) && !MyUtility.Check.Empty(this.dateCreate.Value2))
                {
                    sqlcmd += $@" and convert(date, t1.AddDate) between '{Convert.ToDateTime(this.dateCreate.Value1).ToString("yyyy/MM/dd")}' and '{Convert.ToDateTime(this.dateCreate.Value2).ToString("yyyy/MM/dd")}'";
                }

                if (!MyUtility.Check.Empty(this.strMaterialType_Sheet1))
                {
                    sqlcmd += $@" and po3.FabricType = '{this.strMaterialType_Sheet1}'";
                }
            }

            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            DataTable dtSource;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtSource))
            {
                if (dtSource.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                // 重新設定表身欄位
                if (this.gridUpdate != null)
                {
                    if (this.gridUpdate.Rows.Count > 0)
                    {
                        this.listControlBindingSource1.DataSource = null;
                    }

                    if (this.gridUpdate.Columns.Count > 0)
                    {
                        this.gridUpdate.Columns.Clear();
                    }
                }

                this.listControlBindingSource1.DataSource = dtSource;
                this.gridUpdate.DataSource = this.listControlBindingSource1.DataSource;
                this.SetGrid();
                this.Grid_Filter();
                this.ChangeGridColor();
            }
            else
            {
                this.ShowErr(result);
            }

            this.HideWaitMessage();
        }

        private void SetGrid()
        {
            DataGridViewGeneratorNumericColumnSettings chk_qty = new DataGridViewGeneratorNumericColumnSettings();

            // 設定Qty Setting
            switch (this.strFunction)
            {
                case "P07":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);
                            if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                            {
                                dr["Qty"] = e.FormattedValue;
                                if (!MyUtility.Check.Empty(dr["pounit"]) && !MyUtility.Check.Empty(dr["stockunit"]))
                                {
                                    string rate = MyUtility.GetValue.Lookup($@"select RateValue from dbo.View_Unitrate v where v.FROM_U ='{dr["pounit"]}' and v.TO_U='{dr["stockunit"]}'");
                                    dr["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
                                }

                                // 計算差額, 用來更新庫存
                                dr["diffQty"] = MyUtility.Convert.GetDecimal(dr["stockqty"]) - MyUtility.Convert.GetDecimal(dr["Old_StockQty"]);
                                dr["Selected"] = "1";
                            }

                            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["Ukey"]}'").CopyToDataTable();
                            decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                            if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                            {
                                dr["Selected"] = "0";
                                e.Cancel = true;
                            }

                            dr.EndEdit();
                        }
                    };
                    break;
                case "P08":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);
                            if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                            {
                                dr["Qty"] = e.FormattedValue;
                                dr["StockQty"] = e.FormattedValue;

                                // 計算差額, 用來更新庫存
                                dr["diffQty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["Old_StockQty"]);
                                dr["Selected"] = "1";
                            }

                            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["Ukey"]}'").CopyToDataTable();
                            decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                            if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                            {
                                dr["Selected"] = "0";
                                e.Cancel = true;
                            }

                            dr.EndEdit();
                        }
                    };
                    break;
                case "P18":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);
                            if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                            {
                                dr["Qty"] = e.FormattedValue;

                                // 計算差額, 用來更新庫存
                                dr["diffQty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["Old_StockQty"]);
                                dr["Selected"] = "1";
                            }

                            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["Ukey"]}'").CopyToDataTable();
                            decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                            if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                            {
                                dr["Selected"] = "0";
                                e.Cancel = true;
                            }

                            dr.EndEdit();
                        }
                    };
                    break;
                case "P11":
                case "P33":
                case "P62":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);
                            DataTable dtDetail = (DataTable)this.listControlBindingSource1.DataSource;
                            if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                            {
                                dr["Qty"] = e.FormattedValue;

                                // 加總第三層Qty ,並更新到第二層Qty
                                decimal ttlQty = Math.Round(
                                        dtDetail.AsEnumerable()
                                        .Where(r => r["ukey"].ToString() == dr["ukey"].ToString())
                                        .Sum(r => MyUtility.Convert.GetDecimal(r["Qty"])), 2);

                                // 計算差額, 用來更新庫存
                                foreach (DataRow item in dtDetail.Rows)
                                {
                                    if (item["Ukey"].ToString() == dr["Ukey"].ToString())
                                    {
                                        item["Ttl_Qty"] = ttlQty;
                                        if (this.strFunction == "P11")
                                        {
                                            item["diffQty"] = ttlQty - MyUtility.Convert.GetDecimal(item["Old_Qty"]);
                                        }
                                        else
                                        {
                                            item["diffQty"] = MyUtility.Convert.GetDecimal(item["Qty"]) - MyUtility.Convert.GetDecimal(item["Old_Qty"]);
                                        }
                                    }
                                }

                                dr["Selected"] = "1";
                            }

                            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["Ukey"]}'").CopyToDataTable();
                            decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                            if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                            {
                                dr["Selected"] = "0";
                                e.Cancel = true;
                            }

                            dr.EndEdit();
                        }
                    };
                    break;
                case "P12":
                case "P13":
                case "P15":
                case "P16":
                case "P19":
                case "P37":
                case "P31":
                case "P32":
                case "P22":
                case "P23":
                case "P24":
                case "P36":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);
                            if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                            {
                                dr["Qty"] = e.FormattedValue;

                                // 計算差額, 用來更新庫存
                                dr["diffQty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["Old_Qty"]);
                                dr["Selected"] = "1";
                            }

                            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["Ukey"]}'").CopyToDataTable();
                            decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                            if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                            {
                                dr["Selected"] = "0";
                                e.Cancel = true;
                            }

                            dr.EndEdit();
                        }
                    };
                    break;
                case "P34":
                case "P35":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);

                            if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                            {
                                // Qty = QtyAfter
                                dr["Qty"] = e.FormattedValue;

                                // 計算差額, 用來更新庫存
                                dr["diffQty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["Old_Qty"]);
                                dr["adjustqty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["qtybefore"]);
                                dr["Selected"] = "1";
                            }

                            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["Ukey"]}'").CopyToDataTable();
                            decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                            if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                            {
                                dr["Selected"] = "0";
                                e.Cancel = true;
                            }

                            dr.EndEdit();
                        }
                    };
                    break;
                case "P43":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);
                            if (MyUtility.Convert.GetDecimal(e.FormattedValue) == MyUtility.Convert.GetDecimal(dr["QtyBefore"]))
                            {
                                MyUtility.Msg.WarningBox(@"Current Qty cannot be equal Original Qty!!");
                                e.Cancel = true;
                                return;
                            }

                            if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                            {
                                if (string.Compare(dr["qtyafter"].ToString(), e.FormattedValue.ToString()) != 0)
                                {
                                    dr["Qty"] = e.FormattedValue; // 計算差額, 用來更新庫存
                                    dr["diffQty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["Old_Qty"]);
                                    dr["adjustqty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["qtybefore"]);
                                }

                                dr["Selected"] = "1";
                            }

                            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["ukey"]}'").CopyToDataTable();
                            decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                            if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                            {
                                dr["Selected"] = "0";
                                e.Cancel = true;
                            }

                            dr.EndEdit();
                        }
                    };
                    break;
                case "P45":
                    chk_qty.CellValidating += (s, e) =>
                    {
                        if (this.EditMode && this.gridUpdate != null && this.gridUpdate.Rows.Count > 0 && this.listControlBindingSource1.DataSource != null)
                        {
                            DataRow dr = this.gridUpdate.GetDataRow(e.RowIndex);

                            if (MyUtility.Convert.GetDecimal(e.FormattedValue) == MyUtility.Convert.GetDecimal(dr["QtyBefore"]))
                            {
                                MyUtility.Msg.WarningBox(@"Current Qty cannot be equal Original Qty!!");
                                e.Cancel = true;
                                return;
                            }

                            if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                            {
                                if (MyUtility.Convert.GetDecimal(dr["Qty"]) != MyUtility.Convert.GetDecimal(e.FormattedValue))
                                {
                                    if (MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue) <= 0)
                                    {
                                        dr["Qty"] = 0;
                                    }
                                    else
                                    {
                                        dr["Qty"] = e.FormattedValue; // 計算差額, 用來更新庫存
                                        dr["diffQty"] = MyUtility.Convert.GetDecimal(dr["Qty"]) - MyUtility.Convert.GetDecimal(dr["Old_Qty"]);
                                    }

                                    dr["adjustqty"] = MyUtility.Convert.GetDecimal(dr["qtybefore"]) - MyUtility.Convert.GetDecimal(dr["Qty"]);
                                    dr["Selected"] = "1";
                                }

                                DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select($"ukey = '{dr["ukey"]}'").CopyToDataTable();
                                decimal diffQty = MyUtility.Convert.GetDecimal(dt.Rows[0]["diffQty"]);
                                if (!this.ChkFtyinventory_Balance(dt, diffQty >= 0, true))
                                {
                                    dr["Selected"] = "0";
                                    e.Cancel = true;
                                }

                                dr.EndEdit();
                            }
                        }
                    };
                    break;
            }

            // 設定Grid 欄位
            switch (this.strFunction)
            {
                case "P07":
                    #region P07 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                   .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                   .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                   .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                   .Text("poid", header: "SP#", width: Widths.AnsiChars(11), iseditingreadonly: true) // 1
                   .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                   .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(9), iseditable: false) // 3
                   .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
                   .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 7, iseditingreadonly: true) // 5
                   .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 7, iseditingreadonly: true) // 6
                   .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), iseditingreadonly: true) // 7
                   .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 8
                   .Numeric("Qty", header: "Actual Qty", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty)
                   .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true) // 10
                   .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true)
                   .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 12
                   .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 13
                   .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", width: Widths.AnsiChars(8), iseditable: false) // 14
                   .Text("Location", header: "Location", iseditingreadonly: true)
                   .Text("remark", header: "Remark", iseditingreadonly: true)
                   .Text("RefNo", header: "Ref#", iseditingreadonly: true) // 17
                   .Text("ColorID", header: "Color", iseditingreadonly: true) // 18
                   .Text("FactoryID", header: "Prod. Factory", iseditingreadonly: true) // 19
                   .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(15), iseditingreadonly: true) // 20
                   .Text("ContainerType", header: "ContainerType & No", width: Widths.AnsiChars(15), iseditingreadonly: true) // 21
                   ;
                    #endregion
                    break;
                case "P08":
                    #region P08 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true)
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
                    .Numeric("useqty", header: "Use Qty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("Qty", header: "Receiving Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: false, settings: chk_qty).Get(out this.col_Qty)
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true)
                    .EditText("Remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                case "P18":
                    #region P18 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("FabricType", header: "Fabric \r\n Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("ActualWeight", header: "Act.(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("Qty", header: "In Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: false, settings: chk_qty).Get(out this.col_Qty)
                    .Text("stockunit", header: "Unit", iseditingreadonly: true)
                    .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), iseditable: false)
                    .Text("Location", header: "Location", iseditingreadonly: true)
                    .Text("Remark", header: "Remark", iseditingreadonly: true)
                    .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                case "P11":
                    #region P11 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("poid", header: "SP#", width: Widths.AnsiChars(11), iseditingreadonly: true) // 1
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Text("Colorid", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
                    .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
                    .Text("SizeUnit", header: "SizeUnit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Numeric("Qty", header: "Pick Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: false, settings: chk_qty).Get(out this.col_Qty)
                    .Numeric("Ttl_Qty", header: "Ttl Qty", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("output", header: "Pick Output", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("balanceqty", header: "Balance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("AutoPickqty", header: "AutoPick \r\n Calculation \r\n Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
                    .Text("OutputAutoPick", header: "AutoPick \r\n Calculation \r\n Output", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                case "P12":
                    #region P12 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), checkMDivisionID: true, iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                    .Text("stockunit", header: "Unit", iseditingreadonly: true)
                    .Numeric("Qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty)
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                case "P13":
                    #region P13 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("FtyGroup", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .EditText("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 8
                    .Numeric("NetQty", header: "Used Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                    .Numeric("LossQty", header: "Loss Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 6
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
                    .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                    ;
                    #endregion
                    break;
                case "P33":
                    #region P33 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("poid", header: "SP#", width: Widths.AnsiChars(11), iseditingreadonly: true)
                    .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(25), iseditingreadonly: true)
                    .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
                    .Text("SuppColor", header: "SuppColor", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .EditText("DescDetail", header: "Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("@Qty", header: "@Qty", width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("AccuIssued", header: "Accu. Issued" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Numeric("Qty", header: "Issue Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: false, settings: chk_qty).Get(out this.col_Qty)
                    .Numeric("Ttl_Qty", header: "Ttl Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Numeric("Use Qty By Stock Unit", header: "Use Qty" + Environment.NewLine + "By Stock Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Text("Stock Unit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Numeric("Use Qty By Use Unit", header: "Use Qty" + Environment.NewLine + "By Use Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Text("Use Unit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Stock Unit Desc.", header: "Stock Unit Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("OutputQty", header: "Output Qty" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Numeric("Balance(Stock Unit)", header: "Balance" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Text("Location", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                case "P15":
                    #region P15 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 6
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
                    ;
                    #endregion
                    break;
                case "P16":
                    #region P16 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 6
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
                    .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true);
                    #endregion
                    break;
                case "P19":
                    #region P19 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Out Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 6
                    .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), iseditable: false) // 7
                    .Text("Location", header: "Location", iseditingreadonly: true) // 8
                    .Text("ToPOID", header: "To POID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("ToSeq", header: "To Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                case "P34":
                    #region P34 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("qtybefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty)
                    .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4))
                    .Text("Location", header: "Location", iseditingreadonly: true)
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true)
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
                    ;
                    #endregion
                    break;
                case "P35":
                    #region P35 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Numeric("qtybefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 6
                    .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
                    .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4)) // 8
                    .Text("Location", header: "Location", iseditingreadonly: true) // 9
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true) // 10
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 11
                    ;
                    #endregion
                    break;
                case "P43":
                    #region P43 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("Seq", header: "Seq", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("Qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0, settings: chk_qty).Get(out this.col_Qty)
                    .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("StockUnit", header: "Unit", iseditingreadonly: true)
                    .Text("location", header: "location", iseditingreadonly: true)
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true)
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
                    ;
                    #endregion
                    break;
                case "P45":
                    #region P45 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true) // 1
                    .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true) // 2
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true) // 4
                    .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
                    .Numeric("Qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0, settings: chk_qty).Get(out this.col_Qty) // 5
                    .Numeric("adjustqty", header: "Remove Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 6
                    .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 7
                    .Text("Location", header: "Location", iseditingreadonly: true) // 7
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true) // 8
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 9
                    ;
                    #endregion
                    break;
                case "P22":
                    #region P22 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 5
                    .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 7
                    .Text("toLocation", header: "To Location", iseditingreadonly: true, width: Widths.AnsiChars(18)) // 8
                    ;
                    #endregion
                    break;
                case "P23":
                    #region P23 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "Inventory" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Inventory" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 5
                    .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 7
                    .Text("topoid", header: "Bulk" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 8
                    .Text("toseq", header: "Bulk" + Environment.NewLine + " Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 9
                    .Text("toLocation", header: "To Location", iseditingreadonly: false, width: Widths.AnsiChars(18)) // 10
                    ;
                    #endregion
                    break;
                case "P24":
                    #region P24 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) // 4
                    .Text("fabrictype", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 6
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 7
                    .Text("FromLocation", header: "From Location", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 8
                    .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(15), iseditingreadonly: true) // 8
                    ;
                    #endregion
                    break;
                case "P36":
                    #region P36 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 6
                    .Text("ToLocation", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(30)) // 7
                    ;
                    #endregion
                    break;
                case "P37":
                    #region P37 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .Text("StockType", header: "StockType", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true, settings: chk_qty).Get(out this.col_Qty) // 6
                    .Text("Location", header: "Location", iseditingreadonly: true) // 7
                    ;
                    #endregion
                    break;
                case "P31":
                    #region P31 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "From" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "From" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "From" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false) // 5
                    .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
                    .Text("topoid", header: "To" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 7
                    .Text("toseq", header: "To" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 8
                    .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 9
                    .Text("todyelot", header: "To" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true) // 10
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 11
                    .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                case "P32":
                    #region P32 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0    
                    .Text("fromseq", header: "From" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "From" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "From" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false)
                    .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
                    .Text("topoid", header: "To" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 7
                    .Text("toseq", header: "To" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 8
                    .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 9
                    .Text("todyelot", header: "To" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true) // 10
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty) // 11
                    .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 12
                    .ComboBox("tostocktype", header: "To" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false)
                    ;
                    #endregion
                    break;
                case "P62":
                    #region P62 Grid
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Send To WMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("GridPoID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("GridSeq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                    .Text("GridSeq2", header: "Seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)
                    .Text("Roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(40), iseditingreadonly: true)
                    .Numeric("requestqty", header: "Request", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric(string.Empty, name: "bal_qty", header: "Bal. Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: chk_qty).Get(out this.col_Qty)
                    .Numeric("Ttl_Qty", header: "Ttl Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("FinalFIR", header: "Final FIR", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Numeric(string.Empty, name: "var_qty", header: "Var Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("arqty", header: "Accu Req. Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("aiqqty", header: "Accu Issue Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("avqty", header: "Accu Var by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("unit", header: "unit", width: Widths.AnsiChars(4), iseditingreadonly: true)
                    .Numeric("netqty", header: "Net Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    ;
                    #endregion
                    break;
                default:
                    break;
            }

            this.gridUpdate.Columns[0].Frozen = true;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var data = ((DataRowView)this.gridUpdate.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            // ErrorMsg 是空值,代表可以confirmed才能勾選
            if (!MyUtility.Check.Empty(data["CompleteTime"]))
            {
                this.col_chk.IsEditable = false;
                this.col_Qty.IsEditingReadOnly = true;
            }
            else
            {
                this.col_chk.IsEditable = true;
                this.col_Qty.IsEditingReadOnly = false;
            }
        }

        private void Grid_Filter()
        {
            string filter = string.Empty;
            if (this.gridUpdate.RowCount > 0)
            {
                switch (this.checkIncludeCompleteItem.Checked)
                {
                    case false:
                        if (MyUtility.Check.Empty(this.gridUpdate))
                        {
                            break;
                        }

                        filter = " CompleteTime is null";
                        break;
                    case true:
                        if (MyUtility.Check.Empty(this.gridUpdate))
                        {
                            break;
                        }

                        filter = string.Empty;
                        break;
                }

                ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
            }
        }

        private void CheckIncludeCompleteItem_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
            this.ChangeGridColor();
        }

        private void ComboMaterialType_Sheet1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboMaterialType_Sheet1.SelectedValue != null)
            {
                this.strMaterialType_Sheet1 = this.comboMaterialType_Sheet1.SelectedValue.ToString();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.strFunction))
            {
                var upd_list = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(x => x["Selected"].EqualDecimal(1)).ToList();
                if (upd_list.Count == 0)
                {
                    return;
                }

                string sqlError = string.Empty;

                // Qty不可為0
                var zero_list = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(x => x["Selected"].EqualDecimal(1) && MyUtility.Check.Empty(x["Qty"])).ToList();
                if (zero_list.Any())
                {
                    MyUtility.Msg.WarningBox("Qty Cannot be Empty!");
                    return;
                }

                string upd_sql = string.Empty;
                string chk_sql = string.Empty;
                string upd_Fty_2T = string.Empty;
                string sqlcmd = string.Empty;
                DataTable result_upd_qty;

                DualResult result;
                DataTable dtDistID = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(x => x["Selected"].EqualDecimal(1) && !MyUtility.Check.Empty(x["Qty"])).CopyToDataTable().DefaultView.ToTable(true, "ID");
                switch (this.strFunction)
                {
                    case "P07":
                    case "P08":
                    case "P18":
                        string strTable = (this.strFunction == "P18") ? "TransferIn_Detail" : "Receiving_Detail";
                        #region Receive_Detail & TransferIn_Detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), true))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), strTable))
                        {
                            return;
                        }

                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentReceive_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Revise", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentReceive_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Revise", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        TransactionScope transactionscope = new TransactionScope();
                        DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                        using (transactionscope)
                        using (sqlConn)
                        {
                            try
                            {
                                /*
                                 * 更新庫存, 直接呼叫ReCalculate
                                 */

                                #region update Qty
                                string strcmd = (this.strFunction == "P07") ? ",t.ActualQty = s.Qty" : string.Empty;
                                if (this.strFunction != "P18")
                                {
                                    sqlcmd = $@" 
update t
set t.StockQty = s.StockQty
{strcmd}
from Receiving_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set 
t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Receiving t
inner join Receiving_Detail t2 on t2.id = t.id
inner join #tmp s on t2.Ukey = s.Ukey 
";
                                }
                                else
                                {
                                    sqlcmd = $@"
update t
set t.Qty = s.Qty
from TransferIn_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set 
t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from TransferIn t
inner join TransferIn_Detail t2 on t2.id = t.id
inner join #tmp s on t2.Ukey = s.Ukey 
";
                                }

                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                #endregion

                                #region 更新FIR,AIR資料
                                if (this.strFunction == "P07" && this.strFunction == "P18")
                                {
                                    string strCallSP = (this.strFunction == "P07") ? "dbo.insert_Air_Fir" : "dbo.insert_Air_Fir_TnsfIn";
                                    if (MyUtility.Check.Empty(this.strTransID))
                                    {
                                        foreach (DataRow dr in dtDistID.Rows)
                                        {
                                            List<SqlParameter> fir_Air_Proce = new List<SqlParameter>
                                            {
                                                new SqlParameter("@ID", dr["ID"]),
                                                new SqlParameter("@LoginID", Env.User.UserID),
                                            };

                                            if (!(result = DBProxy.Current.ExecuteSP(string.Empty, strCallSP, fir_Air_Proce)))
                                            {
                                                Exception ex = result.GetException();
                                                transactionscope.Dispose();
                                                MyUtility.Msg.InfoBox(ex.Message.Substring(ex.Message.IndexOf("Error Message:") + "Error Message:".Length));
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        List<SqlParameter> fir_Air_Proce_single = new List<SqlParameter>
                                        {
                                            new SqlParameter("@ID", this.strTransID),
                                            new SqlParameter("@LoginID", Env.User.UserID),
                                        };

                                        if (!(result = DBProxy.Current.ExecuteSP(string.Empty, strCallSP, fir_Air_Proce_single)))
                                        {
                                            Exception ex = result.GetException();
                                            transactionscope.Dispose();
                                            MyUtility.Msg.InfoBox(ex.Message.Substring(ex.Message.IndexOf("Error Message:") + "Error Message:".Length));
                                            return;
                                        }
                                    }
                                }

                                #endregion

                                // Re Calculate
                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, this.ReCalculate(), out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Revise successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                            finally
                            {
                                transactionscope.Dispose();
                            }
                        }

                        #endregion
                        break;
                    case "P11":
                    case "P12":
                    case "P13":
                    case "P15":
                    case "P16":
                    case "P19":
                    case "P33":
                    case "P62":
                        #region Issue_Detail & IssueLack_Detail & TransferOut_Detail & ReturnReceipt_Detail

                        strTable = string.Empty;
                        switch (this.strFunction)
                        {
                            case "P11":
                            case "P12":
                            case "P13":
                            case "P33":
                            case "P62":
                                strTable = "Issue_Detail";
                                break;
                            case "P15":
                            case "P16":
                                strTable = "IssueLack_Detail";
                                break;
                            case "P19":
                                strTable = "TransferOut_Detail";
                                break;
                            default:
                                break;
                        }

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), strTable))
                        {
                            return;
                        }

                        #endregion

                        // 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), true))
                        {
                            return;
                        }

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentIssue_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Revise", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentIssue_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Revise", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                #region update Qty
                                switch (this.strFunction)
                                {
                                    case "P11":
                                        sqlcmd = $@" 
-- 第一層
update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Issue t
inner join #tmp s on t.id = s.id

-- 第二層
update t
set t.Qty = s.Ttl_Qty
from Issue_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

-- 第三層
update Issue_Size
set Qty = s.Qty
from Issue_Size t
inner join #tmp s on t.Id = s.id	
and t.Issue_DetailUkey = s.ukey
and t.SizeCode = s.SizeCode
";
                                        break;
                                    case "P33":
                                    case "P62":
                                        sqlcmd = $@" 
-- 第一層
update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Issue t
inner join #tmp t2 on t2.id = t.id

-- 第二層
update t
set Qty = s.Ttl_Qty
from Issue_Summary t
inner join #tmp s on t.Id = s.id	
and t.ukey = s.ukey

-- 第三層
update t
set t.Qty = s.Qty
from Issue_Detail t
inner join #tmp s on t.id = s.id
and t.poid = s.poid and t.seq1 = s.seq1 
and t.seq2 = s.seq2 and t.roll = s.roll
and t.dyelot = s.dyelot and t.stocktype = s.stocktype
";
                                        break;
                                    case "P15":
                                    case "P16":
                                        sqlcmd = $@" 
update t
set t.Qty = s.Qty
from IssueLack_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from IssueLack t
inner join #tmp s on t.id = s.id
";
                                        break;
                                    case "P19":
                                        sqlcmd = $@" 
update t
set t.Qty = s.Qty
from TransferOut_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from TransferOut t
inner join #tmp s on t.id = s.id
";
                                        break;
                                    default:
                                        sqlcmd = $@" 
update t
set t.Qty = s.Qty
from Issue_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Issue t
inner join #tmp s on t.id = s.id
";
                                        break;
                                }

                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                #endregion

                                // Re Calculate
                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, this.ReCalculate(), out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Revise successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P34":
                    case "P35":
                        #region Adjust_detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), true))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "Adjust_Detail"))
                        {
                            return;
                        }
                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentAdjust_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentAdjust_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                #region update Qty
                                sqlcmd = $@" 
update t
set t.qtyafter = s.Qty
from Adjust_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Adjust t
inner join #tmp s on t.id = s.id
";

                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                #endregion

                                // Re Calculate
                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, this.ReCalculate(), out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Revise successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P43":
                    case "P45":
                        #region Adjust_Detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), true))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "Adjust_Detail"))
                        {
                            return;
                        }
                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            switch (this.strFunction)
                            {
                                case "P43":
                                    if (!Vstrong_AutoWHAccessory.SentAdjust_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                                    {
                                        return;
                                    }

                                    if (!Gensong_AutoWHFabric.SentAdjust_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                                    {
                                        return;
                                    }

                                    break;
                                case "P45":
                                    if (!Vstrong_AutoWHAccessory.SentRemoveC_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                                    {
                                        return;
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                #region update Qty
                                sqlcmd = $@" 
update t
set t.QtyAfter = s.Qty
from Adjust_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Adjust t
inner join #tmp s on t.id = s.id
";

                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                                {
                                    this.ShowErr(result);
                                    return;
                                }

                                #endregion

                                // Re Calculate
                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, this.ReCalculate(), out result_upd_qty)))
                                {
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Revise successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;

                        #endregion
                        break;
                    case "P37":
                        #region ReturnReceipt_Detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), true))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "ReturnReceipt_Detail"))
                        {
                            return;
                        }
                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentReturnReceipt_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentReturnReceipt_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                #region update Qty
                                sqlcmd = $@" 
update t
set t.Qty = s.Qty
from ReturnReceipt_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from ReturnReceipt t
inner join #tmp s on t.id = s.id
";

                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                #endregion

                                // Re Calculate
                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, this.ReCalculate(), out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Revise successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P31":
                    case "P32":
                        #region BorrowBack_detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), true))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "BorrowBack_Detail_From"))
                        {
                            return;
                        }
                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentBorrowBack_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentBorrowBack_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                #region update Qty
                                sqlcmd = $@" 
update t
set t.Qty = s.Qty
from BorrowBack_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from BorrowBack t
inner join #tmp s on t.id = s.id
";

                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                #endregion

                                // Re Calculate
                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, this.ReCalculate(), out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Revise successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P22":
                    case "P23":
                    case "P24":
                    case "P36":
                        #region SubTransfer_detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), true))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "SubTransfer_Detail_From"))
                        {
                            return;
                        }
                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentSubTransfer_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentSubTransfer_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Revise", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                #region update Qty
                                sqlcmd = $@" 
update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from SubTransfer t
inner join #tmp s on t.id = s.id

update t
set t.Qty = s.Qty
from SubTransfer_Detail t
inner join #tmp s on t.Ukey = s.Ukey 
";

                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                #endregion

                                // Re Calculate
                                if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, this.ReCalculate(), out result_upd_qty)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Revise successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                }

                this.Query();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.strFunction))
            {
                var upd_list = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(x => x["Selected"].EqualDecimal(1)).ToList();
                if (upd_list.Count == 0)
                {
                    return;
                }

                string sqlError = string.Empty;

                string upd_sql = string.Empty;
                string chk_sql = string.Empty;
                DualResult result;
                string strTable = string.Empty;
                string strMainTable = string.Empty;

                switch (this.strFunction)
                {
                    case "P07":
                    case "P08":
                    case "P18":
                        #region Receive_Detail && TransferIn_Detail
                        strTable = (this.strFunction == "P18") ? "TransferIn_Detail" : "Receiving_Detail";
                        strMainTable = (this.strFunction == "P18") ? "TransferIn" : "Receiving";
                        #region 檢查負庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), false))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), strTable))
                        {
                            return;
                        }

                        #endregion

                        #region 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentReceive_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Delete", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentReceive_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region 更新庫存數量 po_supp_detail & ftyinventory
                        var data_MD_2F = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                          group b by new
                                          {
                                              poid = b.Field<string>("poid"),
                                              seq1 = b.Field<string>("seq1"),
                                              seq2 = b.Field<string>("seq2"),
                                              stocktype = b.Field<string>("stocktype"),
                                          }
                                            into m
                                          select new Prgs_POSuppDetailData
                                          {
                                              Poid = m.First().Field<string>("poid"),
                                              Seq1 = m.First().Field<string>("seq1"),
                                              Seq2 = m.First().Field<string>("seq2"),
                                              Stocktype = m.First().Field<string>("stocktype"),
                                              Qty = -m.Sum(w => w.Field<decimal>("Old_StockQty")),
                                          }).ToList();

                        var data_MD_8F = (from b in upd_list.CopyToDataTable().AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                                          group b by new
                                          {
                                              poid = b.Field<string>("poid"),
                                              seq1 = b.Field<string>("seq1"),
                                              seq2 = b.Field<string>("seq2"),
                                              stocktype = b.Field<string>("stocktype"),
                                          }
                                            into m
                                          select new Prgs_POSuppDetailData
                                          {
                                              Poid = m.First().Field<string>("poid"),
                                              Seq1 = m.First().Field<string>("seq1"),
                                              Seq2 = m.First().Field<string>("seq2"),
                                              Stocktype = m.First().Field<string>("stocktype"),
                                              Qty = -m.Sum(w => w.Field<decimal>("Old_StockQty")),
                                          }).ToList();

                        var data_Fty_2F = (from m in upd_list.CopyToDataTable().AsEnumerable()
                                           select new
                                           {
                                               poid = m.Field<string>("poid"),
                                               seq1 = m.Field<string>("seq1"),
                                               seq2 = m.Field<string>("seq2"),
                                               stocktype = m.Field<string>("stocktype"),
                                               qty = -m.Field<decimal>("Old_StockQty"),
                                               location = m.Field<string>("location"),
                                               roll = m.Field<string>("roll"),
                                               dyelot = m.Field<string>("dyelot"),
                                           }).ToList();

                        string upd_Fty_2F = Prgs.UpdateFtyInventory_IO_P99(2);
                        #endregion

                        #region 刪除Barcode
                        string upd_Fty_Barcode_V1 = string.Empty;
                        string upd_Fty_Barcode_V2 = string.Empty;
                        var data_Fty_Barcode = (from m in upd_list.AsEnumerable().Where(s => s["FabricType"].ToString() == "F")
                                                select new
                                                {
                                                    TransactionID = m.Field<string>("ID"),
                                                    poid = m.Field<string>("poid"),
                                                    seq1 = m.Field<string>("seq1"),
                                                    seq2 = m.Field<string>("seq2"),
                                                    stocktype = m.Field<string>("stocktype"),
                                                    roll = m.Field<string>("roll"),
                                                    dyelot = m.Field<string>("dyelot"),
                                                    Barcode = m.Field<string>("Barcode"),
                                                }).ToList();

                        upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, false);
                        upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, false);

                        #endregion

                        #region Delete data
                        string sqlcmd = $@" 
update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from {strMainTable} t
inner join #tmp s on t.ID = s.ID

delete t
from {strTable} t
inner join #tmp s on t.Ukey = s.Ukey 
";
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out DataTable result_upd_qty)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        #endregion

                        DataTable resulttb;
                        TransactionScope transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                /*
                                 * 直接Call ReCalculate
                                 */
                                #region MdivisionPoDetail
                                if (data_MD_2F.Count > 0)
                                {
                                    string upd_MD_2F = Prgs.UpdateMPoDetail_P99(2, false);
                                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource")))
                                    {
                                        transactionscope.Dispose();
                                        this.ShowErr(result);
                                        return;
                                    }
                                }

                                if (data_MD_8F.Count > 0)
                                {
                                    string upd_MD_8F = Prgs.UpdateMPoDetail_P99(8, false);
                                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F, out resulttb, "#TmpSource")))
                                    {
                                        transactionscope.Dispose();
                                        this.ShowErr(result);
                                        return;
                                    }
                                }
                                #endregion

                                #region FtyInventory
                                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource")))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                // 更新FtyInventory Barcode
                                if (data_Fty_Barcode.Count >= 1 && this.strFunction == "P07")
                                {
                                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V1, out resulttb, "#TmpSource")))
                                    {
                                        transactionscope.Dispose();
                                        this.ShowErr(result);
                                        return;
                                    }

                                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V2, out resulttb, "#TmpSource")))
                                    {
                                        transactionscope.Dispose();
                                        this.ShowErr(result);
                                        return;
                                    }
                                }
                                #endregion

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Delete successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P11":
                    case "P12":
                    case "P13":
                    case "P15":
                    case "P16":
                    case "P19":
                    case "P33":
                    case "P62":
                        #region Issue_Detail
                        strTable = string.Empty;
                        strMainTable = string.Empty;
                        switch (this.strFunction)
                        {
                            case "P11":
                            case "P12":
                            case "P13":
                            case "P33":
                            case "P62":
                                strTable = "Issue_Detail";
                                strMainTable = "Issue";
                                break;
                            case "P15":
                            case "P16":
                                strTable = "IssueLack_Detail";
                                strMainTable = "IssueLack";
                                break;
                            case "P19":
                                strTable = "TransferOut_Detail";
                                strMainTable = "TransferOut";
                                break;
                            default:
                                break;
                        }

                        #region 檢查負庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), false))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), strTable))
                        {
                            return;
                        }
                        #endregion

                        #region 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentIssue_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Delete", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentIssue_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), this.strFunction, "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region 更新庫存數量  ftyinventory

                        // ftyinventory
                        var bsFty_v2 = (from b in upd_list.AsEnumerable()
                                        group b by new
                                        {
                                            poid = b.Field<string>("poid"),
                                            seq1 = b.Field<string>("seq1"),
                                            seq2 = b.Field<string>("seq2"),
                                            stocktype = b.Field<string>("stocktype"),
                                            roll = b.Field<string>("roll"),
                                            dyelot = b.Field<string>("dyelot"),
                                        }
                                    into m
                                        select new Prgs_FtyInventoryData
                                        {
                                            Poid = m.First().Field<string>("poid"),
                                            Seq1 = m.First().Field<string>("seq1"),
                                            Seq2 = m.First().Field<string>("seq2"),
                                            Stocktype = m.First().Field<string>("stocktype"),
                                            Roll = m.First().Field<string>("roll"),
                                            Dyelot = m.First().Field<string>("Dyelot"),
                                            Qty = -m.Sum(w => w.Field<decimal>("Old_Qty")),
                                        }).ToList();

                        var bs1_v2 = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                      group b by new
                                      {
                                          poid = b.Field<string>("poid"),
                                          seq1 = b.Field<string>("seq1"),
                                          seq2 = b.Field<string>("seq2"),
                                          stocktype = b.Field<string>("stocktype"),
                                      }
                                    into m
                                      select new Prgs_POSuppDetailData
                                      {
                                          Poid = m.First().Field<string>("poid"),
                                          Seq1 = m.First().Field<string>("seq1"),
                                          Seq2 = m.First().Field<string>("seq2"),
                                          Stocktype = m.First().Field<string>("stocktype"),
                                          Qty = -m.Sum(w => w.Field<decimal>("Old_Qty")),
                                      }).ToList();
                        StringBuilder sqlupd2_B_v2 = new StringBuilder();
                        sqlupd2_B_v2.Append(Prgs.UpdateMPoDetail_P99(4, false));
                        string sqlupd2_FIO_v2 = Prgs.UpdateFtyInventory_IO_P99(4);
                        #endregion

                        #region Delete data
                        switch (this.strFunction)
                        {
                            case "P11":
                                sqlcmd = $@"
update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from {strMainTable} t
inner join #tmp s on t.ID = s.ID

 delete t
 from Issue_Size t
 inner join #tmp s on t.Id = s.Id
 and t.Issue_DetailUkey = s.Ukey
 and t.SizeCode = s.SizeCode

delete t
from Issue_Detail t
outer apply(
	select qty = sum(s1.Qty) 
	from Issue_Size s1	
	where s1.Issue_DetailUkey = t.ukey
) sizeQty
where exists(
	select 1 from #tmp s
	where t.ukey = s.ukey
)
and (sizeQty.qty = 0 or sizeQty.qty is null)

update t
set t.Qty = sizeQty.qty
from Issue_Detail t
outer apply(
	select qty = sum(s1.Qty) 
	from Issue_Size s1	
	where s1.Issue_DetailUkey = t.ukey
) sizeQty
where exists(
	select 1 from #tmp s
	where t.ukey = s.ukey
)
and (sizeQty.qty != 0 or sizeQty.qty is not null)
";
                                break;
                            case "P33":
                            case "P62":
                                sqlcmd = $@"
-- 第三層
delete t
from Issue_Detail t
inner join #tmp s on t.id = s.id
and t.poid = s.poid and t.seq1 = s.seq1 
and t.seq2 = s.seq2 and t.roll = s.roll
and t.dyelot = s.dyelot and t.stocktype = s.stocktype

-- 第二層
delete t
from Issue_Summary t
outer apply(
	select qty = sum(s1.Qty) 
	from Issue_Detail s1	
	where t.Id = s1.Id
	and t.Ukey = s1.Issue_SummaryUkey
) is2
where exists(
	select 1 from #tmp s
	where t.ukey = s.ukey
	and s.id = t.id
)
and (is2.qty = 0 or is2.qty is null)

update t
set t.Qty = is2.qty
from Issue_Summary t
outer apply(
	select qty = sum(s1.Qty) 
	from Issue_Detail s1	
	where t.Id = s1.Id
	and t.Ukey = s1.Issue_SummaryUkey
) is2
where exists(
	select 1 from #tmp s
	where t.ukey = s.ukey
	and s.id = t.id
)
and (is2.qty != 0 or is2.qty is not null)

-- 第一層
update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from {strMainTable} t
inner join #tmp s on t.ID = s.ID
";
                                break;
                            case "P15":
                            case "P16":
                                sqlcmd = $@" 
delete t
from IssueLack_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from {strMainTable} t
inner join #tmp s on t.ID = s.ID

";
                                break;
                            case "P19":
                                sqlcmd = $@" 
delete t
from TransferOut_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from {strMainTable} t
inner join #tmp s on t.ID = s.ID
";
                                break;
                            default:
                                sqlcmd = $@" 
delete t
from Issue_detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from {strMainTable} t
inner join #tmp s on t.ID = s.ID
";
                                break;
                        }

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                if (!(result = MyUtility.Tool.ProcessWithObject(bs1_v2, string.Empty, sqlupd2_B_v2.ToString(), out resulttb, "#TmpSource")))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                if (!(result = MyUtility.Tool.ProcessWithObject(
                                    bsFty_v2, string.Empty, sqlupd2_FIO_v2, out resulttb, "#TmpSource")))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Delete successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P34":
                    case "P35":
                        #region Adjust_detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), false))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "Adjust_Detail"))
                        {
                            return;
                        }
                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentAdjust_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentAdjust_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region 更新 MdivisionPoDetail --
                        var data_MD_8F32F = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                             group b by new
                                             {
                                                 poid = b.Field<string>("poid").Trim(),
                                                 seq1 = b.Field<string>("seq1").Trim(),
                                                 seq2 = b.Field<string>("seq2").Trim(),
                                                 stocktype = b.Field<string>("stocktype").Trim(),
                                             }
                                             into m
                                             select new Prgs_POSuppDetailData
                                             {
                                                 Poid = m.First().Field<string>("poid"),
                                                 Seq1 = m.First().Field<string>("seq1"),
                                                 Seq2 = m.First().Field<string>("seq2"),
                                                 Stocktype = m.First().Field<string>("stocktype"),
                                                 Qty = -m.Sum(w => w.Field<decimal>("Old_Qty") - w.Field<decimal>("QtyBefore")),
                                             }).ToList();

                        string upd_MD_8F_v2 = Prgs.UpdateMPoDetail_P99(8, false);
                        string upd_MD_32F = Prgs.UpdateMPoDetail_P99(32, false);

                        #endregion

                        #region -- 更新庫存數量  ftyinventory --

                        // ftyinventory
                        var data_Fty_8F = (from b in upd_list.AsEnumerable()
                                           group b by new
                                           {
                                               poid = b.Field<string>("poid"),
                                               seq1 = b.Field<string>("seq1"),
                                               seq2 = b.Field<string>("seq2"),
                                               stocktype = b.Field<string>("stocktype"),
                                               roll = b.Field<string>("roll"),
                                               dyelot = b.Field<string>("dyelot"),
                                           }
                                    into m
                                           select new Prgs_FtyInventoryData
                                           {
                                               Poid = m.First().Field<string>("poid"),
                                               Seq1 = m.First().Field<string>("seq1"),
                                               Seq2 = m.First().Field<string>("seq2"),
                                               Stocktype = m.First().Field<string>("stocktype"),
                                               Roll = m.First().Field<string>("roll"),
                                               Dyelot = m.First().Field<string>("Dyelot"),
                                               Qty = -m.Sum(w => w.Field<decimal>("Old_Qty") - w.Field<decimal>("QtyBefore")),
                                           }).ToList();

                        string upd_Fty_8F = Prgs.UpdateFtyInventory_IO_P99(8);

                        #endregion 更新庫存數量  ftyinventory

                        #region delete
                        sqlcmd = $@" 
delete t
from Adjust_detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Adjust t
inner join #tmp s on t.ID = s.ID
";

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        #endregion

                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F32F, string.Empty, upd_MD_8F_v2, out resulttb, "#TmpSource")))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F32F, string.Empty, upd_MD_32F, out resulttb, "#TmpSource")))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_8F, string.Empty, upd_Fty_8F, out resulttb, "#TmpSource")))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Delete successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P43":
                        #region P43

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "Adjust_Detail"))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查負數庫存 & 更新庫存

                        string ids = string.Empty;
                        sqlcmd = @"
SELECT AD2.poid, [Seq]= AD2.Seq1+' '+AD2.Seq2,
        AD2.Seq1,AD2.Seq2,
        AD2.Roll,AD2.Dyelot,AD2.id,
       [CheckQty] =  (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty) - ( AD2.Old_Qty - AD2.qtybefore ) , 
       [FTYLobQty] = (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty),
       [AdjustQty]= (AD2.qty - AD2.qtybefore )       
FROM    FtyInventory FTI
inner join #tmp AD2 on FTI.POID=AD2.POID 
and FTI.Seq1=AD2.Seq1
and FTI.Seq2=AD2.Seq2 
and FTI.Roll=AD2.Roll
and FTI.Dyelot = AD2.Dyelot
WHERE FTI.StockType='O' and AD2.ID = '{0}' ";
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out DataTable datacheck)))
                        {
                            this.ShowErr(chk_sql, result);
                            return;
                        }
                        else
                        {
                            if (datacheck.Rows.Count > 0)
                            {
                                foreach (DataRow tmp in datacheck.Rows)
                                {
                                    if (MyUtility.Convert.GetDecimal(tmp["CheckQty"]) >= 0)
                                    {
                                        #region 更新表頭狀態資料 and 數量
                                        // 更新FtyInventory
                                        string sqlupdHeader = $@"
                            update FtyInventory  
                            set  AdjustQty = AdjustQty - ({MyUtility.Convert.GetDecimal(tmp["AdjustQty"])})
                            where POID = '{tmp["Poid"]}' AND SEQ1='{tmp["seq1"].ToString()}' AND SEQ2='{tmp["seq2"]}' and StockType='O'  and Roll = '{tmp["Roll"]}' and Dyelot = '{tmp["Dyelot"]}'
                            ";

                                        // 更新Adjust
                                        sqlupdHeader = sqlupdHeader + $@"
                            update Adjust
                            set editname = '{Env.User.UserID}' , editdate = GETDATE() where id = '{tmp["id"]}'";
                                        if (!(result = DBProxy.Current.Execute(null, sqlupdHeader)))
                                        {
                                            this.ShowErr(sqlupdHeader, result);
                                            return;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        ids += string.Format(
                                            "SP#: {0} SEQ#:{1} Roll#:{2} Dyelot:{3}'s balance: {4} is less than Adjust qty: {5}" + Environment.NewLine + "Balacne Qty is not enough!!",
                                            tmp["poid"],
                                            tmp["Seq"],
                                            tmp["Roll"],
                                            tmp["Dyelot"],
                                            tmp["FTYLobQty"],
                                            tmp["AdjustQty"]) + Environment.NewLine;
                                    }
                                }

                                if (!MyUtility.Check.Empty(ids))
                                {
                                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                                    return;
                                }

                                // 更新MDivisionPoDetail
                                this.UpdMDivisionPoDetail(upd_list.CopyToDataTable());
                            }
                        }
                        #endregion 檢查負數庫存

                        #region 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentAdjust_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentAdjust_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region delete
                        sqlcmd = $@" 
delete t
from Adjust_detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Adjust t
inner join #tmp s on t.ID = s.ID
";

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        #endregion

                        MyUtility.Msg.InfoBox("Delete successful");
                        #endregion
                        break;
                    case "P45":
                        #region RemoveC

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "Adjust_Detail"))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), false))
                        {
                            return;
                        }
                        #endregion

                        #region 先檢查WMS是否傳送成功
                        if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable && upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentRemoveC_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region delete Data
                        string upcmd =
                        $@"
declare @POID varchar(13)
		, @seq1 varchar(3)
		, @seq2 varchar(3)
		, @Roll varchar(8)
		, @Dyelot varchar(8)
		, @StockType varchar(1)
		, @AdjustQty numeric(11, 2)


DECLARE _cursor CURSOR FOR
select ad.POID, ad.Seq1, ad.Seq2, ad.Roll, ad.Dyelot, ad.StockType, [AdjustQty] = (ad.old_qty - ad.QtyBefore) 
from #tmp ad


OPEN _cursor
FETCH NEXT FROM _cursor INTO @POID, @seq1, @seq2, @Roll, @Dyelot, @StockType, @AdjustQty
WHILE @@FETCH_STATUS = 0
BEGIN	
	update f
		set [AdjustQty] = f.AdjustQty - @AdjustQty
	from FtyInventory f
	where f.POID = @POID
	and f.Seq1 = @seq1
	and f.Seq2 = @seq2
	and f.Roll = @Roll
	and f.Dyelot = @Dyelot
	and f.StockType = 'O'

	update m
		set [LObQty] = m.LObQty - @AdjustQty  
	from MDivisionPoDetail m
	where m.POID = @POID
	and m.Seq1 = @seq1
	and m.Seq2 = @seq2

	FETCH NEXT FROM _cursor INTO @POID, @seq1, @seq2, @Roll, @Dyelot, @StockType, @AdjustQty
END
CLOSE _cursor
DEALLOCATE _cursor

delete t
from Adjust_detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from Adjust t
inner join #tmp s on t.ID = s.ID
";
                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, upcmd, out result_upd_qty)))
                        {
                            this.ShowErr(result);
                            MyUtility.Msg.WarningBox("Delete datas error!!");
                            return;
                        }
                        #endregion

                        MyUtility.Msg.InfoBox("Delete successful");
                        #endregion
                        break;
                    case "P37":
                        #region ReturnReceipt_Detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), false))
                        {
                            return;
                        }

                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "ReturnReceipt_Detail"))
                        {
                            return;
                        }
                        #endregion

                        #region update 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentReturnReceipt_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentReturnReceipt_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region -- 更新庫存數量  ftyinventory --

                        var data_Fty_2T_v2 = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                              select new
                                              {
                                                  poid = b.Field<string>("poid"),
                                                  seq1 = b.Field<string>("seq1"),
                                                  seq2 = b.Field<string>("seq2"),
                                                  stocktype = b.Field<string>("stocktype"),
                                                  qty = b.Field<decimal>("qty"),
                                                  roll = b.Field<string>("roll"),
                                                  dyelot = b.Field<string>("dyelot"),
                                              }).ToList();
                        string upd_Fty_2T = Prgs.UpdateFtyInventory_IO_P99(2);
                        #endregion

                        #region -- update mdivisionPoDetail --
                        var data_MD_2T_v2 = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                             group b by new
                                             {
                                                 poid = b.Field<string>("poid"),
                                                 seq1 = b.Field<string>("seq1"),
                                                 seq2 = b.Field<string>("seq2"),
                                                 stocktype = b.Field<string>("stocktype"),
                                             }
                                    into m
                                             select new
                                             {
                                                 poid = m.First().Field<string>("poid"),
                                                 Seq1 = m.First().Field<string>("seq1"),
                                                 Seq2 = m.First().Field<string>("seq2"),
                                                 Stocktype = m.First().Field<string>("stocktype"),
                                                 Qty = m.Sum(w => w.Field<decimal>("qty")),
                                             }).ToList();
                        var data_MD_8T_v2 = (from b in upd_list.CopyToDataTable().AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                                             group b by new
                                             {
                                                 poid = b.Field<string>("poid").Trim(),
                                                 seq1 = b.Field<string>("seq1").Trim(),
                                                 seq2 = b.Field<string>("seq2").Trim(),
                                                 stocktype = b.Field<string>("stocktype").Trim(),
                                             }
                                    into m
                                             select new Prgs_POSuppDetailData
                                             {
                                                 Poid = m.First().Field<string>("poid"),
                                                 Seq1 = m.First().Field<string>("seq1"),
                                                 Seq2 = m.First().Field<string>("seq2"),
                                                 Stocktype = m.First().Field<string>("stocktype"),
                                                 Location = m.First().Field<string>("location"),
                                                 Qty = m.Sum(w => w.Field<decimal>("qty")),
                                             }).ToList();

                        #endregion

                        #region delete Qty
                        sqlcmd = $@" 
delete t
from ReturnReceipt_Detail t
inner join #tmp s on t.ukey = s.ukey

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from ReturnReceipt t
inner join #tmp s on t.ID = s.ID
";

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        #endregion

                        string upd_MD_2T = string.Empty;
                        string upd_MD_8T = string.Empty;
                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                #region FtyInventory
                                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T_v2, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource")))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(result);
                                    return;
                                }
                                #endregion

                                #region MDivisionPoDetail
                                if (data_MD_2T_v2.Count > 0)
                                {
                                    upd_MD_2T = Prgs.UpdateMPoDetail_P99(2, false);
                                }

                                if (data_MD_8T_v2.Count > 0)
                                {
                                    upd_MD_8T = Prgs.UpdateMPoDetail_P99(8, false);
                                }

                                if (data_MD_2T_v2.Count > 0)
                                {
                                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T_v2, string.Empty, upd_MD_2T, out resulttb, "#TmpSource")))
                                    {
                                        transactionscope.Dispose();
                                        this.ShowErr(result);
                                        return;
                                    }
                                }

                                if (data_MD_8T_v2.Count > 0)
                                {
                                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T_v2, string.Empty, upd_MD_8T, out resulttb, "#TmpSource")))
                                    {
                                        transactionscope.Dispose();
                                        this.ShowErr(result);
                                        return;
                                    }
                                }
                                #endregion

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Delete successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                    case "P31":
                    case "P32":
                        #region BorrowBack_detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), false))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "BorrowBack_Detail_From"))
                        {
                            return;
                        }
                        #endregion

                        #region 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentBorrowBack_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentBorrowBack_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region -- 更新MdivisionPoDetail 借出數 --
                        var data_MD_4F_BorrowBack = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                                     group b by new
                                                     {
                                                         poid = b.Field<string>("frompoid").Trim(),
                                                         seq1 = b.Field<string>("fromseq1").Trim(),
                                                         seq2 = b.Field<string>("fromseq2").Trim(),
                                                         stocktype = b.Field<string>("fromstocktype").Trim(),
                                                     }
                                              into m
                                                     select new
                                                     {
                                                         poid = m.First().Field<string>("frompoid"),
                                                         Seq1 = m.First().Field<string>("fromseq1"),
                                                         Seq2 = m.First().Field<string>("fromseq2"),
                                                         Stocktype = m.First().Field<string>("fromstocktype"),
                                                         Qty = -m.Sum(w => w.Field<decimal>("Old_Qty")),
                                                     }).ToList();
                        var data_MD_8F_BorrowBack = (from b in upd_list.CopyToDataTable().AsEnumerable().Where(w => w.Field<string>("fromstocktype").Trim() == "I")
                                                     group b by new
                                                     {
                                                         poid = b.Field<string>("frompoid").Trim(),
                                                         seq1 = b.Field<string>("fromseq1").Trim(),
                                                         seq2 = b.Field<string>("fromseq2").Trim(),
                                                         stocktype = b.Field<string>("fromstocktype").Trim(),
                                                     }
                                              into m
                                                     select new Prgs_POSuppDetailData
                                                     {
                                                         Poid = m.First().Field<string>("frompoid"),
                                                         Seq1 = m.First().Field<string>("fromseq1"),
                                                         Seq2 = m.First().Field<string>("fromseq2"),
                                                         Stocktype = m.First().Field<string>("fromstocktype"),
                                                         Qty = m.Sum(w => w.Field<decimal>("Old_Qty")),
                                                     }).ToList();

                        var data_MD_8F_BorrowBack_P32 = (from b in upd_list.CopyToDataTable().AsEnumerable().Where(w => w.Field<string>("fromstocktype").Trim() == "I")
                                                         group b by new
                                                         {
                                                             poid = b.Field<string>("frompoid").Trim(),
                                                             seq1 = b.Field<string>("fromseq1").Trim(),
                                                             seq2 = b.Field<string>("fromseq2").Trim(),
                                                             stocktype = b.Field<string>("fromstocktype").Trim(),
                                                         }
                                            into m
                                                         select new Prgs_POSuppDetailData
                                                         {
                                                             Poid = m.First().Field<string>("frompoid"),
                                                             Seq1 = m.First().Field<string>("fromseq1"),
                                                             Seq2 = m.First().Field<string>("fromseq2"),
                                                             Stocktype = m.First().Field<string>("fromstocktype"),
                                                             Qty = -m.Sum(w => w.Field<decimal>("Old_Qty")),
                                                         }).ToList();

                        #endregion
                        #region -- 更新MdivisionPoDetail 借入數 --
                        var data_MD_2F_BorrowBack = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                                     group b by new
                                                     {
                                                         poid = b.Field<string>("topoid").Trim(),
                                                         seq1 = b.Field<string>("toseq1").Trim(),
                                                         seq2 = b.Field<string>("toseq2").Trim(),
                                                         stocktype = b.Field<string>("tostocktype").Trim(),
                                                     }
                                              into m
                                                     select new Prgs_POSuppDetailData
                                                     {
                                                         Poid = m.First().Field<string>("topoid"),
                                                         Seq1 = m.First().Field<string>("toseq1"),
                                                         Seq2 = m.First().Field<string>("toseq2"),
                                                         Stocktype = m.First().Field<string>("tostocktype"),
                                                         Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                                     }).ToList();

                        #endregion
                        #region -- 更新庫存數量  ftyinventory --
                        var data_Fty_4F_BorrowBack = (from m in upd_list.CopyToDataTable().AsEnumerable()
                                                      select new
                                                      {
                                                          poid = m.Field<string>("frompoid"),
                                                          seq1 = m.Field<string>("fromseq1"),
                                                          seq2 = m.Field<string>("fromseq2"),
                                                          stocktype = m.Field<string>("fromstocktype"),
                                                          qty = -m.Field<decimal>("Old_Qty"),
                                                          location = m.Field<string>("location"),
                                                          roll = m.Field<string>("fromroll"),
                                                          dyelot = m.Field<string>("fromdyelot"),
                                                      }).ToList();
                        var data_Fty_2F_BorrowBack = (from m in upd_list.CopyToDataTable().AsEnumerable()
                                                      select new
                                                      {
                                                          poid = m.Field<string>("topoid"),
                                                          seq1 = m.Field<string>("toseq1"),
                                                          seq2 = m.Field<string>("toseq2"),
                                                          stocktype = m.Field<string>("tostocktype"),
                                                          qty = -m.Field<decimal>("Old_Qty"),
                                                          location = m.Field<string>("location"),
                                                          roll = m.Field<string>("toroll"),
                                                          dyelot = m.Field<string>("todyelot"),
                                                      }).ToList();

                        #endregion 更新庫存數量  ftyinventory

                        #region delete Qty
                        sqlcmd = $@" 
delete t
from BorrowBack_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from BorrowBack t
inner join #tmp s on t.ID = s.ID
";

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        #endregion

                        string upd_Fty_4F_BorrowBack = Prgs.UpdateFtyInventory_IO_P99(4);
                        string upd_Fty_2F_BorrowBack = Prgs.UpdateFtyInventory_IO_P99(2);
                        string upd_MD_2F_BorrowBack = Prgs.UpdateMPoDetail_P99(2, false);
                        string upd_MD_4F_BorrowBack = Prgs.UpdateMPoDetail_P99(4, false);
                        string upd_MD_8F_BorrowBack = Prgs.UpdateMPoDetail_P99(8, false);
                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                switch (this.strFunction)
                                {
                                    case "P31":
                                        #region FtyInventory
                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F_BorrowBack, string.Empty, upd_Fty_4F_BorrowBack, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F_BorrowBack, string.Empty, upd_Fty_2F_BorrowBack, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion

                                        #region MDivisionPoDetail
                                        if (data_MD_4F_BorrowBack.Count > 0)
                                        {
                                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F_BorrowBack, string.Empty, upd_MD_4F_BorrowBack, out resulttb, "#TmpSource")))
                                            {
                                                transactionscope.Dispose();
                                                this.ShowErr(result);
                                                return;
                                            }
                                        }

                                        if (data_MD_8F_BorrowBack.Count > 0)
                                        {
                                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F_BorrowBack, string.Empty, upd_MD_8F_BorrowBack, out resulttb, "#TmpSource")))
                                            {
                                                transactionscope.Dispose();
                                                this.ShowErr(result);
                                                return;
                                            }
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F_BorrowBack, string.Empty, upd_MD_2F_BorrowBack, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion

                                        break;
                                    case "P32":

                                        #region MDivisionPoDetail
                                        if (data_MD_4F_BorrowBack.Count > 0)
                                        {
                                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F_BorrowBack, string.Empty, upd_MD_4F_BorrowBack, out resulttb, "#TmpSource")))
                                            {
                                                transactionscope.Dispose();
                                                this.ShowErr(result);
                                                return;
                                            }
                                        }

                                        if (data_MD_8F_BorrowBack.Count > 0)
                                        {
                                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F_BorrowBack_P32, string.Empty, upd_MD_8F_BorrowBack, out resulttb, "#TmpSource")))
                                            {
                                                transactionscope.Dispose();
                                                this.ShowErr(result);
                                                return;
                                            }
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F_BorrowBack, string.Empty, upd_MD_2F_BorrowBack, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        #endregion

                                        #region FtyInventory
                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F_BorrowBack, string.Empty, upd_Fty_4F_BorrowBack, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F_BorrowBack, string.Empty, upd_Fty_2F_BorrowBack, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion
                                        break;
                                    default:
                                        break;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Delete successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;

                    case "P22":
                    case "P23":
                    case "P24":
                    case "P36":
                        #region SubTransfer_detail

                        #region 檢查庫存
                        if (!this.ChkFtyinventory_Balance(upd_list.CopyToDataTable(), false))
                        {
                            return;
                        }
                        #endregion

                        #region 檢查資料有任一筆WMS已完成
                        if (!Prgs.ChkWMSCompleteTime(upd_list.CopyToDataTable(), "SubTransfer_Detail_From"))
                        {
                            return;
                        }
                        #endregion

                        #region 先檢查WMS是否傳送成功
                        if (upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).ToList().Count > 0)
                        {
                            if (!Vstrong_AutoWHAccessory.SentSubTransfer_Detail_delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }

                            if (!Gensong_AutoWHFabric.SentSubTransfer_Detail_Delete(upd_list.CopyToDataTable().AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable(), "Delete", true))
                            {
                                return;
                            }
                        }
                        #endregion

                        #region -- 更新mdivisionpodetail A倉數 --
                        var data_MD_2F_SubTransfer = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                                      group b by new
                                                      {
                                                          poid = b.Field<string>("Topoid"),
                                                          seq1 = b.Field<string>("Toseq1"),
                                                          seq2 = b.Field<string>("Toseq2"),
                                                          stocktype = b.Field<string>("Tostocktype"),
                                                      }
                                    into m
                                                      select new Prgs_POSuppDetailData
                                                      {
                                                          Poid = m.First().Field<string>("Topoid"),
                                                          Seq1 = m.First().Field<string>("Toseq1"),
                                                          Seq2 = m.First().Field<string>("Toseq2"),
                                                          Stocktype = m.First().Field<string>("Tostocktype"),
                                                          Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                                          Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                                                      }).ToList();

                        #endregion

                        #region -- 更新MdivisionPoDetail B倉數量 --
                        var data_MD_8F_SubTransfer = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                                      group b by new
                                                      {
                                                          poid = b.Field<string>("frompoid"),
                                                          seq1 = b.Field<string>("fromseq1"),
                                                          seq2 = b.Field<string>("fromseq2"),
                                                          stocktype = b.Field<string>("fromstocktype"),
                                                      }
                                    into m
                                                      select new Prgs_POSuppDetailData
                                                      {
                                                          Poid = m.First().Field<string>("frompoid"),
                                                          Seq1 = m.First().Field<string>("fromseq1"),
                                                          Seq2 = m.First().Field<string>("fromseq2"),
                                                          Stocktype = m.First().Field<string>("fromstocktype"),
                                                          Qty = -m.Sum(w => w.Field<decimal>("old_qty")),
                                                      }).ToList();

                        #endregion

                        #region -- 更新mdivisionpodetail Inventory數 --
                        var data_MD_4F_SubTransfer = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                                      group b by new
                                                      {
                                                          poid = b.Field<string>("frompoid"),
                                                          seq1 = b.Field<string>("fromseq1"),
                                                          seq2 = b.Field<string>("fromseq2"),
                                                          stocktype = b.Field<string>("fromstocktype"),
                                                      }
                                    into m
                                                      select new Prgs_POSuppDetailData
                                                      {
                                                          Poid = m.First().Field<string>("frompoid"),
                                                          Seq1 = m.First().Field<string>("fromseq1"),
                                                          Seq2 = m.First().Field<string>("fromseq2"),
                                                          Stocktype = m.First().Field<string>("fromstocktype"),
                                                          Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                                      }).ToList();

                        #endregion

                        #region -- 更新mdivisionpodetail Scrap數 --
                        var data_MD_16F_SubTransfer = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                                       group b by new
                                                       {
                                                           poid = b.Field<string>("topoid"),
                                                           seq1 = b.Field<string>("toseq1"),
                                                           seq2 = b.Field<string>("toseq2"),
                                                           stocktype = b.Field<string>("tostocktype"),
                                                       }
                                into m
                                                       select new
                                                       {
                                                           poid = m.First().Field<string>("topoid"),
                                                           Seq1 = m.First().Field<string>("toseq1"),
                                                           Seq2 = m.First().Field<string>("toseq2"),
                                                           Stocktype = m.First().Field<string>("tostocktype"),
                                                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                                       }).ToList();

                        #endregion

                        #region -- 更新mdivisionpodetail Scrap數 4F+16F --
                        var data_MD_4F16F_SubTransfer = (from b in upd_list.CopyToDataTable().AsEnumerable()
                                                         group b by new
                                                         {
                                                             poid = b.Field<string>("frompoid"),
                                                             seq1 = b.Field<string>("fromseq1"),
                                                             seq2 = b.Field<string>("fromseq2"),
                                                             stocktype = b.Field<string>("fromstocktype"),
                                                         }
                                    into m
                                                         select new
                                                         {
                                                             poid = m.First().Field<string>("frompoid"),
                                                             Seq1 = m.First().Field<string>("fromseq1"),
                                                             Seq2 = m.First().Field<string>("fromseq2"),
                                                             Stocktype = m.First().Field<string>("fromstocktype"),
                                                             Qty = m.Sum(w => w.Field<decimal>("qty")),
                                                         }).ToList();

                        #endregion

                        #region -- 更新庫存數量  ftyinventory --
                        var data_Fty_4F_SubTransfer = (from m in upd_list.CopyToDataTable().AsEnumerable()
                                                       select new
                                                       {
                                                           poid = m.Field<string>("frompoid"),
                                                           seq1 = m.Field<string>("fromseq1"),
                                                           seq2 = m.Field<string>("fromseq2"),
                                                           stocktype = m.Field<string>("fromstocktype"),
                                                           qty = -m.Field<decimal>("old_qty"),
                                                           location = m.Field<string>("tolocation"),
                                                           roll = m.Field<string>("fromroll"),
                                                           dyelot = m.Field<string>("fromdyelot"),
                                                       }).ToList();
                        var data_Fty_2F_SubTransfer = (from m in upd_list.CopyToDataTable().AsEnumerable()
                                                       select new
                                                       {
                                                           poid = m.Field<string>("topoid"),
                                                           seq1 = m.Field<string>("toseq1"),
                                                           seq2 = m.Field<string>("toseq2"),
                                                           stocktype = m.Field<string>("tostocktype"),
                                                           qty = -m.Field<decimal>("old_qty"),
                                                           location = m.Field<string>("tolocation"),
                                                           roll = m.Field<string>("toroll"),
                                                           dyelot = m.Field<string>("todyelot"),
                                                       }).ToList();

                        #endregion 更新庫存數量  ftyinventory

                        #region delete Qty
                        sqlcmd = $@" 
delete t
from SubTransfer_Detail t
inner join #tmp s on t.Ukey = s.Ukey 

update t
set t.editname = '{Env.User.UserID}'
,t.editdate = GETDATE()
from SubTransfer t
inner join #tmp s on t.ID = s.ID
";

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(upd_list.CopyToDataTable(), string.Empty, sqlcmd, out result_upd_qty)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        #endregion

                        string upd_Fty_4F_SubTransfer = Prgs.UpdateFtyInventory_IO_P99(4);
                        string upd_Fty_2F_SubTransfer = Prgs.UpdateFtyInventory_IO_P99(2);

                        string upd_MD_2F_SubTransfer = Prgs.UpdateMPoDetail_P99(2, false);
                        string upd_MD_4F_SubTransfer = Prgs.UpdateMPoDetail_P99(4, false);
                        string upd_MD_8F_SubTransfer = Prgs.UpdateMPoDetail_P99(8, false);
                        string upd_MD_8T_SubTransfer = Prgs.UpdateMPoDetail_P99(8, true);
                        string upd_MD_16F_SubTransfer = Prgs.UpdateMPoDetail_P99(16, false);
                        transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                switch (this.strFunction)
                                {
                                    case "P22":
                                        #region FtyInventory
                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F_SubTransfer, string.Empty, upd_Fty_4F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F_SubTransfer, string.Empty, upd_Fty_2F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion

                                        #region MDivisionPoDetail
                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F_SubTransfer, string.Empty, upd_MD_8F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        #endregion
                                        break;
                                    case "P23":
                                        #region FtyInventory
                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F_SubTransfer, string.Empty, upd_Fty_4F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F_SubTransfer, string.Empty, upd_Fty_2F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion

                                        #region MDeivisionPoDetail

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F_SubTransfer, string.Empty, upd_MD_4F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F_SubTransfer, string.Empty, upd_MD_8F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F_SubTransfer, string.Empty, upd_MD_2F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion
                                        break;
                                    case "P24":
                                        #region FtyInventory
                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F_SubTransfer, string.Empty, upd_Fty_4F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F_SubTransfer, string.Empty, upd_Fty_2F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion

                                        #region MDivisionPoDetail

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F_SubTransfer, string.Empty, upd_MD_4F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F_SubTransfer, string.Empty, upd_MD_8F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_16F_SubTransfer, string.Empty, upd_MD_16F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion
                                        break;

                                    case "P36":
                                        #region FtyInventory
                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F_SubTransfer, string.Empty, upd_Fty_4F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F_SubTransfer, string.Empty, upd_Fty_2F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion

                                        #region MDivisionPoDetail

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F16F_SubTransfer, string.Empty, upd_MD_4F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F16F_SubTransfer, string.Empty, upd_MD_16F_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }

                                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_16F_SubTransfer, string.Empty, upd_MD_8T_SubTransfer, out resulttb, "#TmpSource")))
                                        {
                                            transactionscope.Dispose();
                                            this.ShowErr(result);
                                            return;
                                        }
                                        #endregion
                                        break;
                                }

                                transactionscope.Complete();
                                transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Delete successful");
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        transactionscope.Dispose();
                        transactionscope = null;
                        #endregion
                        break;
                }

                this.Query();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Sheet 2 [UnLock]

        private void BtnQuery2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtWKNo.Text) && MyUtility.Check.Empty(this.txtReceivingID.Text))
            {
                MyUtility.Msg.WarningBox("<SP#>, <WK#>, <Receiving ID> cannot be empty!");
                return;
            }

            this.Query_Sheet2();
        }

        private void Query_Sheet2()
        {
            string sqlcmd = @"
select distinct
 [Selected] = 0 --0
 ,fi.POID
 ,fi.Seq1,fi.Seq2
 ,[MaterialType] = case when pd.FabricType = 'F' then 'Fabric' when pd.FabricType='A' then 'Accessory' end
 ,fi.Roll,fi.Dyelot,fi.InQty,fi.OutQty,fi.AdjustQty,fi.ReturnQty
 ,[Balance] = fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty
 ,[StockType] = case fi.StockType 
				when 'B' then 'Bulk'
				when 'I' then 'Inventory'
				else fi.StockType end
,[Location] = dbo.Getlocation(fi.Ukey)
,fi.Ukey
from dbo.FtyInventory fi WITH (NOLOCK) 
left join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = fi.POID and pd.seq1 = fi.seq1 and pd.seq2  = fi.Seq2
left join dbo.orders o WITH (NOLOCK) on o.id = fi.POID
left join dbo.factory f WITH (NOLOCK) on o.FtyGroup=f.id
left join Receiving_Detail rd WITH (NOLOCK) on rd.PoId = fi.POID and rd.Seq1 = fi.seq1 and rd.seq2 = fi.seq2 and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
left join Receiving r WITH (NOLOCK) on r.id = rd.id
left join TransferIn_Detail TD WITH (NOLOCK) on TD.PoId = fi.POID and TD.Seq1 = fi.seq1 and TD.seq2 = fi.seq2 and TD.Roll = fi.Roll and TD.Dyelot = fi.Dyelot
left join TransferIn T WITH (NOLOCK) on T.id = TD.id
where 1=1
and fi.WMSLock = 1
";

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlcmd += $@" and fi.Poid = '{this.txtSPNo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSeq.Seq1))
            {
                sqlcmd += $@" and fi.Seq1 = '{this.txtSeq.Seq1}'";
            }

            if (!MyUtility.Check.Empty(this.txtSeq.Seq2))
            {
                sqlcmd += $@" and fi.seq2 = '{this.txtSeq.Seq2}'";
            }

            if (!MyUtility.Check.Empty(this.txtWKNo.Text))
            {
                sqlcmd += $@" and r.ExportID = '{this.txtWKNo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtReceivingID.Text))
            {
                sqlcmd += $@" and (r.ID = '{this.txtReceivingID.Text}' or TD.ID = '{this.txtReceivingID.Text}')";
            }

            if (!MyUtility.Check.Empty(this.comboMaterialType_Sheet2.SelectedValue))
            {
                sqlcmd += $@" and pd.FabricType = '{this.comboMaterialType_Sheet2.SelectedValue.ToString()}'";
            }

            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            DataTable dtSource;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtSource))
            {
                if (dtSource.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                // 重新設定表身欄位
                if (this.gridUnLock != null)
                {
                    if (this.gridUnLock.Rows.Count > 0)
                    {
                        this.listControlBindingSource2.DataSource = null;
                    }

                    if (this.gridUnLock.Columns.Count > 0)
                    {
                        this.gridUnLock.Columns.Clear();
                    }
                }

                this.listControlBindingSource2.DataSource = dtSource;
                this.gridUnLock.DataSource = this.listControlBindingSource2.DataSource;
                this.SetGrid2();
            }
            else
            {
                this.ShowErr(result);
            }

            this.HideWaitMessage();
        }

        private void SetGrid2()
        {
            this.gridUnLock.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridUnLock)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("MaterialType", header: "Material Type", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("OutQty", header: "Out Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("BalanceQty", header: "Balance Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("StockType", header: "Stock Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;
        }

        private void BtnClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            var upd_list = ((DataTable)this.listControlBindingSource2.DataSource).AsEnumerable().Where(x => x["Selected"].EqualDecimal(1)).ToList();
            string upd_sql = string.Empty;
            if (upd_list.Count == 0)
            {
                return;
            }

            foreach (DataRow dr in upd_list)
            {
                upd_sql += $@"update Ftyinventory set WMSLock = 0 where ukey = '{dr["ukey"]}'" + Environment.NewLine;
            }

            DualResult result = DBProxy.Current.Execute(null, upd_sql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("UnLock successfully!");
            this.Query_Sheet2();
        }
        #endregion

        #region 共用Function

        private bool ChkFty_Lock(DataTable dt, bool isConfirmed = true)
        {
            DualResult result;
            DataTable datacheck;
            string sql_chk = string.Empty;
            switch (this.strFunction)
            {
                case "P31":
                case "P32":
                case "P22":
                case "P23":
                case "P24":
                case "P36":
                    if (isConfirmed)
                    {
                        sql_chk = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.FromPoId
and t.FromSeq1 = f.Seq1 and t.FromSeq2 = f.Seq2
and t.FromRoll = f.Roll and t.FromDyelot = f.Dyelot
and t.FromStockType = f.StockType
where   1=1
and f.lock=1
";
                    }
                    else
                    {
                        sql_chk = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.ToPoId
and t.ToSeq1 = f.Seq1 and t.ToSeq2 = f.Seq2
and t.ToRoll = f.Roll and t.ToDyelot = f.Dyelot
and t.ToStockType = f.StockType
where 1=1
and f.lock=1
";
                    }

                    break;
                default:
                    sql_chk = @"
select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot       
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.PoId
and t.Seq1 = f.Seq1 and t.Seq2 = f.Seq2
and t.Roll = f.Roll and t.Dyelot = f.Dyelot
and t.StockType = f.StockType
where   1=1
and f.Lock = 1
";
                    break;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sql_chk, out datacheck)))
            {
                this.ShowErr(sql_chk, result);
                return false;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked" + Environment.NewLine,
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 檢查正/負庫存
        /// </summary>
        /// <returns>bool</returns>
        private bool ChkFtyinventory_Balance(DataTable dt, bool isConfirmed, bool isDetail = false)
        {
            string chk_sql = string.Empty;
            DualResult result;

            // 如果是Revise, 就用差異值來判斷, 因為通常Confirmed時庫存數量已被扣掉, 但現在做法是庫存並未扣除,所以只能用差異的去更新庫存
            // 如果是delete就扣除原本的數量就行
            // 如果是表身Validating觸發, 判斷diffQty為負就用UnConfirmed檢查, 並且要用取正號diffQty去計算Balance
            string symbol = string.Empty;
            string symbol2 = string.Empty;
            DataTable datacheck;

            switch (this.strFunction)
            {
                case "P07":
                case "P08":
                    symbol = isConfirmed ? "+ (t.diffQty)" : isDetail ? "+ (t.diffQty)" : "- (t.Old_StockQty)";
                    chk_sql = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , StockQty = convert(decimal(10,2), t.StockQty)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.PoId
and t.Seq1 = f.Seq1 and t.Seq2 = f.Seq2
and t.Roll = f.Roll and t.Dyelot = f.Dyelot
and t.StockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) {symbol} < 0)  

";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4}" + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["stockqty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }

                    break;

                case "P18":
                    symbol = isConfirmed ? "+ (t.diffQty)" : isDetail ? "+ (t.diffQty)" : "- (t.Old_StockQty)";
                    chk_sql = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , Qty = convert(decimal(10,2), t.Qty)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.PoId
and t.Seq1 = f.Seq1 and t.Seq2 = f.Seq2
and t.Roll = f.Roll and t.Dyelot = f.Dyelot
and t.StockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) {symbol} < 0)  

";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4}" + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["qty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }

                    break;
                case "P11":
                case "P33":
                case "P62":
                    symbol = isConfirmed ? "- (t.diffQty)" : isDetail ? "- (t.diffQty)" : "+ (t.Old_Qty)";
                    chk_sql = $@"

select  distinct  f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , Qty = convert(decimal(10,2), t.Ttl_Qty)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.PoId
and t.Seq1 = f.Seq1 and t.Seq2 = f.Seq2
and t.Roll = f.Roll and t.Dyelot = f.Dyelot
and t.StockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) {symbol} < 0)  

";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} " + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["Qty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }

                    break;
                case "P12":
                case "P13":
                case "P15":
                case "P16":
                case "P19":
                case "P37":
                    symbol = isConfirmed ? "- (t.diffQty)" : isDetail ? "- (t.diffQty)" : "+ (t.Old_Qty)";
                    chk_sql = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , Qty = convert(decimal(10,2), t.Qty)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.PoId
and t.Seq1 = f.Seq1 and t.Seq2 = f.Seq2
and t.Roll = f.Roll and t.Dyelot = f.Dyelot
and t.StockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) {symbol} < 0)  

";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4}" + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["qty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }

                    break;
                case "P34":
                case "P35":
                case "P45":
                    symbol = isConfirmed ? "+ (t.diffQty)" : isDetail ? "+ (t.diffQty)" : "- (t.Old_Qty - t.QtyBefore)";
                    chk_sql = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , Qty = convert(decimal(10,2), t.Qty) - convert(decimal(10,2), t.QtyBefore)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t on f.POID = t.PoId
and t.Seq1 = f.Seq1 and t.Seq2 = f.Seq2
and t.Roll = f.Roll and t.Dyelot = f.Dyelot
and t.StockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) {symbol} < 0)  

";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4}" + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["qty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }

                    break;

                case "P31":
                case "P32":
                    symbol2 = isDetail ? "+ (t.diffQty)" : "- (t.Old_Qty)";
                    symbol = isConfirmed ? @"
on f.POID = t.FromPoId
and t.FromSeq1 = f.Seq1 and t.FromSeq2 = f.Seq2
and t.FromRoll = f.Roll and t.FromDyelot = f.Dyelot
and t.FromStockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) - (t.diffQty) < 0)  
" : $@"
on f.POID = t.ToPoId
and t.ToSeq1 = f.Seq1 and t.ToSeq2 = f.Seq2
and t.ToRoll = f.Roll and t.ToDyelot = f.Dyelot
and t.ToStockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)  {symbol2} < 0)  
";
                    chk_sql = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , Qty = convert(decimal(10,2), t.Qty)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t 
{symbol}
";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} " + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["qty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }

                    break;

                case "P22":
                case "P23":
                case "P24":
                case "P36":
                    #region Bulk Balance
                    symbol2 = isDetail ? "- (t.diffQty)" : "+ (t.Old_Qty)";
                    symbol = isConfirmed ? @"
on f.POID = t.FromPoId
and t.FromSeq1 = f.Seq1 and t.FromSeq2 = f.Seq2
and t.FromRoll = f.Roll and t.FromDyelot = f.Dyelot
and t.FromStockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) - (t.diffQty) < 0)  
and t.Qty > 0
" : $@"
on f.POID = t.FromPoId
and t.FromSeq1 = f.Seq1 and t.FromSeq2 = f.Seq2
and t.FromRoll = f.Roll and t.FromDyelot = f.Dyelot
and t.FromStockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) {symbol2} < 0)  
and t.Qty < 0
";
                    chk_sql = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , Qty = convert(decimal(10,2), t.Qty)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t 
{symbol}
";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4}" + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["qty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Bulk balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }
                    #endregion

                    #region Inventory balacne
                    symbol2 = isDetail ? "+ (t.diffQty)" : "- (t.Old_Qty)";
                    symbol = isConfirmed ? @"
on f.POID = t.ToPoId
and t.ToSeq1 = f.Seq1 and t.ToSeq2 = f.Seq2
and t.ToRoll = f.Roll and t.ToDyelot = f.Dyelot
and t.ToStockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) + (t.diffQty) < 0)  
and t.Qty < 0
" : $@"
on f.POID = t.ToPoId
and t.ToSeq1 = f.Seq1 and t.ToSeq2 = f.Seq2
and t.ToRoll = f.Roll and t.ToDyelot = f.Dyelot
and t.ToStockType = f.StockType
where   1=1
and (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0) {symbol2} < 0)  
";
                    chk_sql = $@"

select    f.poid
        , f.seq1
        , f.seq2
        , f.Roll
		, f.Dyelot
        , Qty = convert(decimal(10,2), t.Qty)
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
from FtyInventory f WITH (NOLOCK)
inner join #tmp t 
{symbol}
";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                ids += string.Format(
                                    "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4}" + Environment.NewLine,
                                    tmp["poid"],
                                    tmp["seq1"],
                                    tmp["seq2"],
                                    tmp["roll"],
                                    tmp["balanceqty"],
                                    tmp["qty"],
                                    tmp["Dyelot"]);
                            }

                            MyUtility.Msg.WarningBox("Inventory balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                            return false;
                        }
                    }
                    #endregion

                    break;

                case "P43":
                    symbol = isConfirmed ? "+ (t.diffQty)" : isDetail ? "+ (t.diffQty)" : "- (t.Old_Qty - t.QtyBefore)";
                    chk_sql = $@"
SELECT t.poid, [Seq]= t.Seq1+' '+t.Seq2,
        t.Seq1,t.Seq2,
        t.Roll,t.Dyelot,
       [CheckQty] =  (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty) {symbol} , 
       [FTYLobQty] = (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty),
       [AdjustQty]= (t.qty - t.qtybefore )       
FROM    FtyInventory FTI
inner join #tmp t on FTI.POID=t.POID 
and FTI.Seq1=t.Seq1
and FTI.Seq2=t.Seq2 
and FTI.Roll=t.Roll
and FTI.Dyelot = t.Dyelot
WHERE FTI.StockType='O' 
";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chk_sql, out datacheck)))
                    {
                        this.ShowErr(chk_sql, result);
                        return false;
                    }
                    else
                    {
                        if (datacheck.Rows.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (DataRow tmp in datacheck.Rows)
                            {
                                if (MyUtility.Convert.GetDecimal(tmp["CheckQty"]) < 0)
                                {
                                    ids += string.Format(
                                          "SP#: {0} SEQ#:{1} Roll#:{2} Dyelot:{3}'s balance: {4}" + Environment.NewLine,
                                          tmp["poid"],
                                          tmp["Seq"],
                                          tmp["Roll"],
                                          tmp["Dyelot"],
                                          tmp["FTYLobQty"],
                                          tmp["AdjustQty"]) + Environment.NewLine;
                                }
                            }

                            if (!MyUtility.Check.Empty(ids))
                            {
                                MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                                return false;
                            }
                        }
                    }

                    break;
            }

            return true;
        }

        /// <summary>
        /// 初始化, 設定表頭控制像是否能編輯
        /// </summary>
        /// <param name="canEdit">是否能編輯</param>
        public void Initl(bool canEdit)
        {
            MyUtility.Tool.SetupCombox(this.comboFunction, 2, 1, @"P07,P07. Material Receiving,P08,P08. Receiving from factory Supply,P18,P18. Transfer In,P11,P11. Issue Sewing Material,P12,P12. Issue Packing Material,P13,P13. Issue R/Mtl By Item,P15,P15. Issue Accessory Lacking  && Replacement,P16,P16. Issue Fabric Lacking  && Replacement,P19,P19. Transfer Out,P33,P33. Issue Thread,P45,P45. Remove from Scrap Whse,P22,P22. Transfer Bulk to Inventory (A2B),P23,P23. Transfer Inventory to Bulk (B2A),P24,P24. Transfer Inventory To Scrap (B2C),P36,P36. Transfer Scrap to Inventory (C2B),P37,P37. Return Receiving Material,P31,P31. Material Borrow,P32,P32. Return Borrowing,P34,P34. Adjust Inventory Qty,P35,P35. Adjust Bulk Qty,P43,P43. Adjust Scrap Qty,P62,P62. Issue Fabric for Cutting Tape");

            MyUtility.Tool.SetupCombox(this.comboMaterialType_Sheet2, 2, 1, @"F,Fabric,A,Accessory");
            MyUtility.Tool.SetupCombox(this.comboMaterialType_Sheet1, 2, 1, @"F,Fabric,A,Accessory");
            if (!canEdit)
            {
                this.TabPage_UnLock.Parent = null; // 隱藏Unlock Tab
                this.dateCreate.Value1 = null;
                this.dateCreate.Value2 = null;
                this.txtSPNoStart.Text = string.Empty;
                this.txtSPNoEnd.Text = string.Empty;
                this.comboFunction.SelectedIndex = 0;
                this.comboFunction.SelectedIndex = 0;
                string strcomboType = string.Empty;
                switch (this.strFunction)
                {
                    case "P07":
                        strcomboType = "P07. Material Receiving";
                        break;
                    case "P08":
                        strcomboType = "P08. Receiving from factory Supply";
                        break;
                    case "P18":
                        strcomboType = "P18. Transfer In";
                        break;
                    case "P11":
                        strcomboType = "P11. Issue Sewing Material";
                        break;
                    case "P12":
                        strcomboType = "P12. Issue Packing Material";
                        break;
                    case "P13":
                        strcomboType = "P13. Issue R/Mtl By Item";
                        break;
                    case "P15":
                        strcomboType = "P15. Issue Accessory Lacking  && Replacement";
                        break;
                    case "P16":
                        strcomboType = "P16. Issue Fabric Lacking  && Replacement";
                        break;
                    case "P19":
                        strcomboType = "P19. Transfer Out";
                        break;
                    case "P33":
                        strcomboType = "P33. Issue Thread";
                        break;
                    case "P45":
                        strcomboType = "P45. Remove from Scrap Whse";
                        break;
                    case "P22":
                        strcomboType = "P22. Transfer Bulk to Inventory (A2B)";
                        break;
                    case "P23":
                        strcomboType = "P23. Transfer Inventory to Bulk (B2A)";
                        break;
                    case "P24":
                        strcomboType = "P24. Transfer Inventory To Scrap (B2C)";
                        break;
                    case "P36":
                        strcomboType = "P36. Transfer Scrap to Inventory (C2B)";
                        break;
                    case "P37":
                        strcomboType = "P37. Return Receiving Material";
                        break;
                    case "P31":
                        strcomboType = "P31. Material Borrow";
                        break;
                    case "P32":
                        strcomboType = "P32. Return Borrowing";
                        break;
                    case "P34":
                        strcomboType = "P34. Adjust Inventory Qty";
                        break;
                    case "P35":
                        strcomboType = "P35. Adjust Bulk Qty";
                        break;
                    case "P43":
                        strcomboType = "P43. Adjust Scrap Qty";
                        break;
                    case "P62":
                        strcomboType = "P62. Issue Fabric for Cutting Tape";
                        break;
                    default:
                        break;
                }

                MyUtility.Tool.SetupCombox(this.comboFunction, 2, 1, $"{this.strFunction},{strcomboType}");
            }
            else
            {
                this.TabPage_UnLock.Parent = this.tabControl1; // 顯示
                this.strFunction = this.comboFunction.SelectedValue.ToString();
                this.strMaterialType_Sheet1 = this.comboMaterialType_Sheet1.SelectedValue.ToString();
            }

            this.comboFunction.Enabled = canEdit;
            this.checkIncludeCompleteItem.Enabled = canEdit;
            this.dateCreate.Enabled = canEdit;
            this.comboMaterialType_Sheet1.Enabled = canEdit;
            this.txtSPNoStart.Enabled = canEdit;
            this.txtSPNoEnd.Enabled = canEdit;
            this.btnQuery.Visible = canEdit;
            this.EditMode = true;
        }

        /// <summary>
        /// 設定從其他功能跳轉過來, 預先設定ID,FormName
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="formName">程式代號</param>
        public void SetFilter(string id, string formName)
        {
            this.strTransID = id;
            this.strFunction = formName;
        }

        /// <summary>
        /// Change Grid1 Color
        /// </summary>
        private void ChangeGridColor()
        {
            // 設定detailGrid Rows 是否可以編輯
            this.gridUpdate.RowEnter += this.Detailgrid_RowEnter;
            for (int index = 0; index < this.gridUpdate.Rows.Count; index++)
            {
                DataRow dr = this.gridUpdate.GetDataRow(index);
                if (this.gridUpdate.Rows.Count <= index || index < 0)
                {
                    return;
                }

                this.gridUpdate.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
                int i = index;

                // 反灰不能勾選
                if (!MyUtility.Check.Empty(dr["CompleteTime"]))
                {
                    this.gridUpdate.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                }
            }
        }

        /// <summary>
        /// get Re Calculate sql command
        /// </summary>
        /// <returns>sqlcmd</returns>
        private string ReCalculate()
        {
            string sqlcmd = string.Empty;
            switch (this.strFunction)
            {
                case "P22":
                case "P23":
                case "P24":
                case "P36":
                case "P31":
                case "P32":
                    sqlcmd = @"
use Production

declare @POID as varchar(13) 
	, @SEQ1 as varchar(3) 
	, @SEQ2 as varchar(2) 
	, @ukey as bigint 

-- From
DECLARE P99_tempTable_From_cur 
CURSOR FOR 
	select distinct  t.FromPOID, t.FromSeq1, t.FromSeq2, m.Ukey
	from #tmp t
	left join MDivisionPODetail m on t.FromPoId = m.POID and t.FromSeq1 = m.Seq1 and t.FromSeq2 = m.Seq2

OPEN P99_tempTable_From_cur --開始run cursor                   
FETCH NEXT FROM P99_tempTable_From_cur INTO @POID,@SEQ1,@SEQ2,@ukey
WHILE @@FETCH_STATUS = 0
BEGIN
	exec dbo.usp_SingleItemRecaculate @ukey,@POID,@SEQ1,@SEQ2
	
	FETCH NEXT FROM P99_tempTable_From_cur INTO @POID,@SEQ1,@SEQ2,@ukey
END
CLOSE P99_tempTable_From_cur
DEALLOCATE P99_tempTable_From_cur

-- To
DECLARE P99_tempTable_To_cur 
CURSOR FOR 
	select distinct  t.ToPOID, t.ToSeq1, t.ToSeq2, m.Ukey
	from #tmp t
	left join MDivisionPODetail m on t.ToPoId = m.POID and t.ToSeq1 = m.Seq1 and t.ToSeq2 = m.Seq2

OPEN P99_tempTable_To_cur --開始run cursor                   
FETCH NEXT FROM P99_tempTable_To_cur INTO @POID,@SEQ1,@SEQ2,@ukey
WHILE @@FETCH_STATUS = 0
BEGIN
	exec dbo.usp_SingleItemRecaculate @ukey,@POID,@SEQ1,@SEQ2
	
	FETCH NEXT FROM P99_tempTable_To_cur INTO @POID,@SEQ1,@SEQ2,@ukey
END
CLOSE P99_tempTable_To_cur
DEALLOCATE P99_tempTable_To_cur

drop table #tmp
";
                    break;
                default:
                    sqlcmd = @"
use Production

declare @POID as varchar(13) 
	, @SEQ1 as varchar(3) 
	, @SEQ2 as varchar(2) 
	, @ukey as bigint 

DECLARE P99_tempTable_cur 
CURSOR FOR 
	select distinct  t.POID, t.Seq1, t.Seq2, m.Ukey
	from #tmp t
	left join MDivisionPODetail m on t.PoId = m.POID and t.Seq1 = m.Seq1 and t.Seq2 = m.Seq2

OPEN P99_tempTable_cur --開始run cursor                   
FETCH NEXT FROM P99_tempTable_cur INTO @POID,@SEQ1,@SEQ2,@ukey
WHILE @@FETCH_STATUS = 0
BEGIN
	exec dbo.usp_SingleItemRecaculate @ukey,@POID,@SEQ1,@SEQ2
	
	FETCH NEXT FROM P99_tempTable_cur INTO @POID,@SEQ1,@SEQ2,@ukey
END
CLOSE P99_tempTable_cur
DEALLOCATE P99_tempTable_cur

drop table #tmp
";
                    break;
            }

            return sqlcmd;
        }

        private void UpdMDivisionPoDetail(DataTable dtDetail)
        {
            DataTable datacheck;
            DualResult result;
            string sqlcmd =
                @"
SELECT 
md.POID
,md.Seq1
,md.seq2
,[FTYLobQty] = sum(FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty)
FROM    FtyInventory FTI
inner join #tmp AD2 on FTI.POID=AD2.POID 
and FTI.Seq1=AD2.Seq1
and FTI.Seq2=AD2.Seq2 
and FTI.Roll=AD2.Roll
and FTI.Dyelot = AD2.Dyelot
inner join MDivisionPoDetail md on FTI.POID=md.POID and fti.Seq1=md.Seq1 and fti.Seq2=md.Seq2
WHERE FTI.StockType='O' 
group by md.POID,md.Seq1,md.Seq2";
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, string.Empty, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    if (!(result = DBProxy.Current.Execute(null, string.Format(
                        @"
Update MDivisionPoDetail
set LobQty = {0}
where POID = '{1}'
and Seq1 = '{2}' and Seq2 = '{3}' ",
                        MyUtility.Convert.GetDecimal(dr["FTYLobQty"]),
                        dr["poid"].ToString(),
                        dr["seq1"].ToString(),
                        dr["seq2"].ToString()))))
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
            }
        }

        #endregion
    }
}