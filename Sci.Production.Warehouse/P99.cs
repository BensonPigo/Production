using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P99 : Sci.Win.Tems.QueryForm
    {
        private string strFunction;
        private string strMaterialType_Sheet1;
        private string strSPNo1;
        private string strSPNo2;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;

        public P99(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            MyUtility.Tool.SetupCombox(this.comboFunction, 2, 1, @"P07,P07. Material Receiving,P08,P08. Receiving from factory Supply,P18,P18. Transfer In,P11,P11. Issue Sewing Material,P12,P12. Issue Packing Material,P13,P13. Issue R/Mtl By Item,P15,P15. Issue Accessory Lacking  && Replacement,P19,P19. Transfer Out,P33,P33. Issue Thread,P45,P45. Remove from Scrap Whse,P22,P22. Transfer Bulk to Inventory (A2B),P23,P23. Transfer Inventory to Bulk (B2A),P24,P24. Transfer Inventory To Scrap (B2C),P36,P36. Transfer Scrap to Inventory (C2B),P37,P37. Return Receiving Material,P31,P31. Material Borrow,P32,P32. Return Borrowing,P34,P34. Adjust Inventory Qty,P35,P35. Adjust Bulk Qty,P43,P43. Adjust Scrap Qty");

            MyUtility.Tool.SetupCombox(this.comboMaterialType_Sheet1, 2, 1, @"A,Accessory");
            MyUtility.Tool.SetupCombox(this.comboMaterialType_Sheet2, 2, 1, @"ALL,F,Fabric,A,Accessory");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.strFunction = this.comboFunction.SelectedValue.ToString();
            this.strMaterialType_Sheet1 = this.comboMaterialType_Sheet1.SelectedValue.ToString();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.strFunction) || MyUtility.Check.Empty(this.dateCreate.Value1) || MyUtility.Check.Empty(this.dateCreate.Value2))
            {
                MyUtility.Msg.WarningBox("<Function>,<Create Date> cannot be empty!");
                this.comboFunction.Select();
                return;
            }

            this.Query();
        }

        private void Query()
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
        , (select p1.FabricType from PO_Supp_Detail p1 WITH (NOLOCK) where p1.ID = t2.PoId and p1.seq1 = t2.SEQ1 and p1.SEQ2 = t2.seq2) as fabrictype --3
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
        , t2.StockUnit--13
        , t2.StockType--14
        , t2.Location--15
        , t2.remark--16
        , [RefNo]=po3.RefNo--17
		, [ColorID]=Color.Value--18
        , o.FactoryID--19
        , o.OrderTypeID--20
		, [ContainerType]= Container.Val--21
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
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN po3.SuppColor
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
";
                    break;
                case "P08":
                    sqlcmd = @"
select    [Selected] = 0 --0
        , [SentToWMS] = iif(t2.SentToWMS=1,'V','')
        , [CompleteTime] = t2.CompleteTime
        , t2.PoId --1        
        , concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq--2
		, t2.Roll--3
        , t2.Dyelot--4
        , [Description] = dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0)--5        
		, t2.StockUnit--6
        , [useqty] = ( select Round(sum(dbo.GetUnitQty(b.POUnit, StockUnit, b.Qty)), 2) as useqty
            from po_supp_detail b WITH (NOLOCK) 
            where b.id= t2.poid and b.seq1 = t2.seq1 and b.seq2 = t2.seq2) --7
        , [Qty] = t2.StockQty--8
        , t2.Location--9
        , t2.Remark--10
from dbo.Receiving_Detail t2 WITH (NOLOCK) 
inner join Receiving t1 WITH (NOLOCK) on t2.Id = t1.Id
left join Po_Supp_Detail po3 WITH (NOLOCK)  on t2.poid = po3.id
                              and t2.seq1 = po3.seq1
                              and t2.seq2 = po3.seq2

where 1=1
and t1.Type='B'
";
                    break;
                case "P18":
                    sqlcmd = @"
select   [Selected] = 0 --0
        ,[SentToWMS] = iif(t2.SentToWMS=1,'V','')
        ,[CompleteTime] = t2.CompleteTime
        , t2.PoId--1        
        , seq = concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2)--2
		, [Fabric] = case when po3.FabricType = 'F' then 'Fabric' 
                             when po3.FabricType = 'A' then 'Accessory'
                        else '' end--3
        , t2.Roll--4
        , t2.Dyelot--5
        , [Description] = dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0)--6
		, t2.Weight--7
		, [ActualWeight] = isnull(t2.ActualWeight, 0)--8
		, t2.Qty--9
        , StockUnit = dbo.GetStockUnitBySPSeq (t2.poid, t2.seq1, t2.seq2)--10        
        , TtlQty = convert(varchar(20),
			iif(t2.CombineBarcode is null , t2.Qty, 
				iif(t2.Unoriginal is null , ttlQty.value, null))) +' '+ dbo.GetStockUnitBySPSeq (t2.poid, t2.seq1, t2.seq2)--11
        , t2.StockType--12
        , t2.location--13
		, t2.Remark--14        
        , po3.Refno--15
		, [ColorID] = Color.Value--16        
from dbo.TransferIn_Detail t2 WITH (NOLOCK) 
inner join dbo.TransferIn t1 WITH (NOLOCK)  on t2.ID=t2.Id
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
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN po3.SuppColor
		 ELSE dbo.GetColorMultipleID(po3.BrandID,po3.ColorID)
	 END
)Color
Where 1=1
";
                    break;
                case "P11":
                    sqlcmd = @"
select [Selected] = 0 --0
,[SentToWMS] = iif(t2.SentToWMS=1,'V','')
,[CompleteTime] = t2.CompleteTime
,[seq] = concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.seq2)--1
, [description] = dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0)--2
, Colorid = isnull(dbo.GetColorMultipleID(po3.BrandId, po3.ColorID), '')--3
, po3.SizeSpec--4
        , po3.UsedQty--5
        , po3.SizeUnit--6
        , [location] = dbo.Getlocation(fi.ukey) --7
        , t2.StockType
        , t2.Qty--9
        
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
        , t2.Ukey
        , balanceqty = isnull((fi.inqty - fi.outqty + fi.adjustqty),0.00)--12
        , AutoPickqty=(select SUM(AutoPickQty)  from Issue_Size iss where iss.Issue_DetailUkey = t2.ukey)--13
        , OutputAutoPick=(
			select  STUFF((
				select CONCAT(',',rtrim(iss.SizeCode),'*',iss.AutoPickQty)
				from Issue_Size iss
				where iss.Issue_DetailUkey = t2.ukey
                and iss.AutoPickQty <> 0
				for xml path('')
			),1,1,'')
			)--14
from dbo.Issue_Detail t2 WITH (NOLOCK) 
inner join dbo.issue t1 WITH (NOLOCK)  on t2.Id = t1.Id
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
";
                    break;
                case "P12":
                    sqlcmd = @"
select [Selected] = 0 --0
,[SentToWMS] = iif(t2.SentToWMS=1,'V','')
,[CompleteTime] = t2.CompleteTime
, t2.PoId--1
,[seq] = concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2)--2
,[Description] = dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0)--3
,po3.stockunit--4
,t2.Qty--5
,[location] = dbo.Getlocation(Fi.ukey) --6
from dbo.issue_detail t2 WITH (NOLOCK) 
inner join dbo.Issue t1 WITH (NOLOCK) on t1.id = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory FI on t2.POID = FI.POID and t2.Seq1 = FI.Seq1 and t2.Seq2 = FI.Seq2 and FI.Dyelot = t2.Dyelot
    and t2.Roll = FI.Roll and FI.stocktype = 'B'     
where 1=1
and t1.Type='C'
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
, t2.Roll--4
, t2.Dyelot--5
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
,dbo.Getlocation(c.ukey) location--12
, Isnull(c.inqty-c.outqty + c.adjustqty,0.00) as balance--13
from dbo.issue_detail as t2 WITH (NOLOCK) 
inner join issue t1 WITH (NOLOCK) on t2.Id=t1.Id
left join Orders o on t2.poid = o.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join PO_Supp p WITH (NOLOCK) on p.ID = po3.ID and po3.seq1 = p.SEQ1
left join dbo.ftyinventory c WITH (NOLOCK) on c.poid = t2.poid and c.seq1 = t2.seq1 and c.seq2  = t2.seq2 
and c.stocktype = 'B' and c.roll=t2.roll and t2.Dyelot = c.Dyelot
Where 1=1
and t1.Type='D'
";
                    break;
                case "P33":
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
		    , [Qty]=iis.Qty
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
";
                    if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
                    {
                        sqlcmd += $@" and t2.Poid >= '{this.txtSPNoStart.Text}'" + Environment.NewLine;
                    }

                    if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
                    {
                        sqlcmd += $@" and t2.Poid <= '{this.txtSPNoEnd.Text}'" + Environment.NewLine;
                    }

                    sqlcmd += $@" and t1.AddDate between '{Convert.ToDateTime(this.dateCreate.Value1).ToString("yyyy/MM/dd")}' and '{Convert.ToDateTime(this.dateCreate.Value2).ToString("yyyy/MM/dd")}'";

                    sqlcmd += $@" and po3.FabricType = '{this.strMaterialType_Sheet1}'";
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
FROM final t
OUTER APPLY(
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0)
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
                    break;
                case "P15":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id,t2.PoId,t2.Seq1,t2.Seq2,concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq
,po3.stockunit
,dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0) as [Description]
,t2.Qty
,t2.StockType
,dbo.Getlocation(f.Ukey)  as location
,t2.ukey
,t2.FtyInventoryUkey
from dbo.IssueLack_Detail t2 WITH (NOLOCK) 
inner join dbo.IssueLack t1 WITH (NOLOCK) on t1.Id = t2.Id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.PoId and po3.seq1 = t2.SEQ1 and po3.SEQ2 = t2.seq2
left join FtyInventory f WITH (NOLOCK) on t2.POID=f.POID and t2.Seq1=f.Seq1 and t2.Seq2=f.Seq2 and t2.Roll=f.Roll and t2.Dyelot=f.Dyelot and t2.StockType=f.StockType
where 1=1
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
,dbo.getMtlDesc(t2.poid,t2.seq1,t2.seq2,2,0) as [Description]
,po3.StockUnit
,t2.Qty
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
";
                    break;
                case "P34":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id,t2.PoId,t2.Seq1,t2.Seq2
,concat(Ltrim(Rtrim(t2.seq1)), ' ', t2.Seq2) as seq
,po3.FabricType
,po3.stockunit
,dbo.getmtldesc(t2.poid,t2.seq1,t2.seq2,2,0) as [description]
,t2.Roll
,t2.Dyelot
,qty = t2.QtyAfter
,t2.QtyBefore
,isnull(t2.QtyAfter,0.00) - isnull(t2.QtyBefore,0.00) adjustqty
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
Qty = t2.QtyAfter,
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
";
                    break;
                case "P45":
                    sqlcmd = @"
select 
	[Selected] = 0 --0
	, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
	, [CompleteTime] = t2.CompleteTime
	,t2.POID
	,seq = concat(t2.Seq1,'-',t2.Seq2)
	,t2.Roll
	,t2.Dyelot
	,Description = dbo.getmtldesc(t2.POID, t2.Seq1, t2.Seq2, 2, 0)
	,t2.QtyBefore
	,Qty = t2.QtyAfter
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
";
                    break;
                case "P22":
                    sqlcmd = @"
select [Selected] = 0 --0
, [SentToWMS] = iif(t2.SentToWMS=1,'V','')
, [CompleteTime] = t2.CompleteTime
,t2.id,t2.FromPoId,t2.FromSeq1,t2.FromSeq2
,concat(Ltrim(Rtrim(t2.FromSeq1)), ' ', t2.FromSeq2) as Fromseq
,po3.FabricType
,po3.stockunit
,dbo.getmtldesc(t2.FromPoId,t2.FromSeq1,t2.FromSeq2,2,0) as [description]
,t2.FromRoll
,t2.FromDyelot
,t2.FromStocktype
,t2.Qty
,t2.ToPoid,t2.ToSeq1,t2.ToSeq2,concat(Ltrim(Rtrim(t2.ToSeq1)), ' ', t2.ToSeq2) as toseq
,t2.ToRoll
,t2.ToDyelot
,t2.ToStocktype
,t2.ToLocation
,t2.fromftyinventoryukey
,t2.ukey
,dbo.Getlocation(fi.ukey) location
from dbo.SubTransfer_detail t2 WITH(NOLOCK) 
inner join SubTransfer t1 WITH(NOLOCK)  on t1.Id = t2.id	
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 and po3.SEQ2 = t2.FromSeq2
left join FtyInventory FI on t2.fromPoid = fi.poid and t2.fromSeq1 = fi.seq1 and t2.fromSeq2 = fi.seq2 and t2.fromDyelot = fi.Dyelot
    and t2.fromRoll = fi.roll and t2.fromStocktype = fi.stocktype
Where 1=1
and t1.Type='A'
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
    ,t2.ToPoId
    ,t2.ToSeq1
    ,t2.ToSeq2
    ,t2.ToDyelot
    ,t2.ToRoll
    ,t2.ToStockType
    ,dbo.Getlocation(f.Ukey)  as Fromlocation
    ,t2.ukey
    ,t2.tolocation
from dbo.SubTransfer_Detail t2 WITH (NOLOCK) 
inner join SubTransfer t1 WITH (NOLOCK)  on t1.Id = t2.id	
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 and po3.SEQ2 = t2.FromSeq2
left join FtyInventory f WITH (NOLOCK) on t2.FromPOID=f.POID and t2.FromSeq1=f.Seq1 and t2.FromSeq2=f.Seq2 and t2.FromRoll=f.Roll and t2.FromDyelot=f.Dyelot and t2.FromStockType=f.StockType
Where 1=1
and t1.Type = 'E'
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
,t2.ToPoId
,t2.ToSeq1
,t2.ToSeq2
,t2.ToDyelot
,t2.ToRoll
,t2.ToStockType
,t2.ToLocation
,t2.ukey
from dbo.SubTransfer_Detail t2 WITH (NOLOCK) 
inner join SubTransfer t1 WITH (NOLOCK) on t1.Id = t2.id
left join PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = t2.FromPoId and po3.seq1 = t2.FromSeq1 
and po3.SEQ2 = t2.FromSeq2
Where 1=1
and t1.Type='C'
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
";
                    break;
                default:
                    break;
            }

            if (this.strFunction != "P33")
            {
                if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
                {
                    sqlcmd += $@" and t2.Poid >= '{this.txtSPNoStart.Text}'" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
                {
                    sqlcmd += $@" and t2.Poid <= '{this.txtSPNoEnd.Text}'" + Environment.NewLine;
                }

                sqlcmd += $@" and t1.AddDate between '{Convert.ToDateTime(this.dateCreate.Value1).ToString("yyyy/MM/dd")}' and '{Convert.ToDateTime(this.dateCreate.Value2).ToString("yyyy/MM/dd")}'";

                sqlcmd += $@" and po3.FabricType = '{this.strMaterialType_Sheet1}'";
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
                this.gridUnLock.DataSource = this.listControlBindingSource1.DataSource;
                this.SetGrid();
                this.ChangeGridColor();
                this.Grid_Filter();
            }
            else
            {
                this.ShowErr(result);
            }

            this.HideWaitMessage();
        }

        private void SetGrid()
        {
            switch (this.strFunction)
            {
                case "P07":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                   .Text("poid", header: "SP#", width: Widths.AnsiChars(11),iseditingreadonly:true) // 1
                   .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                   .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(9), iseditable: false) // 3
                   .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
                   .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 7, iseditingreadonly: true) // 5
                   .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 7, iseditingreadonly: true) // 6
                   .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), iseditingreadonly: true) // 7
                   .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 8
                   .Numeric("Qty", header: "Actual Qty", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 10).Get(out this.col_Qty)
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
                    break;
                case "P08":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true)
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
                    .Numeric("useqty", header: "Use Qty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("Qty", header: "Receiving Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: false).Get(out col_Qty)
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true)
                    .EditText("Remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    ;
                    break;
                case "P18":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Fabric", header: "Fabric \r\n Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("ActualWeight", header: "Act.(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("Qty", header: "In Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10,iseditingreadonly: false).Get(out this.col_Qty)
                    .Text("stockunit", header: "Unit", iseditingreadonly: true)
                    .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), iseditable: false)
                    .Text("Location", header: "Location", iseditingreadonly: true)
                    .Text("Remark", header: "Remark", iseditingreadonly: true)
                    .Text("RefNo", header: "Ref#", iseditingreadonly: true)
                    .Text("ColorID", header: "Color", iseditingreadonly: true)
                    ;
                    break;
                case "P11":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Text("Colorid", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
                    .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
                    .Text("SizeUnit", header: "SizeUnit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("Qty", header: "Pick Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: false).Get(out this.col_Qty)
                    .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("output", header: "Pick Output", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("balanceqty", header: "Balance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("AutoPickqty", header: "AutoPick \r\n Calculation \r\n Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
                    .Text("OutputAutoPick", header: "AutoPick \r\n Calculation \r\n Output", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    ;
                    break;
                case "P12":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), checkMDivisionID: true, iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Text("stockunit", header: "Unit", iseditingreadonly: true)
                    .Numeric("Qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty)
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true)
                    ;
                    break;
                case "P13":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
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
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 6
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
                    .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                    ;
                    break;
                case "P33":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(25), iseditingreadonly: true)
                    .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
                    .Text("SuppColor", header: "SuppColor", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .EditText("DescDetail", header: "Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("@Qty", header: "@Qty", width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("AccuIssued", header: "Accu. Issued" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Numeric("Qty", header: "Issue Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: false).Get(out this.col_Qty)
                    .Numeric("Use Qty By Stock Unit", header: "Use Qty" + Environment.NewLine + "By Stock Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Text("Stock Unit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Numeric("Use Qty By Use Unit", header: "Use Qty" + Environment.NewLine + "By Use Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Text("Use Unit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Stock Unit Desc.", header: "Stock Unit Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("OutputQty", header: "Output Qty" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Numeric("Balance(Stock Unit)", header: "Balance" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                    .Text("Location", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true);
                    break;
                case "P15":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 6
                    .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
                    ;
                    break;
                case "P19":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Out Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 6
                    .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), iseditable: false) // 7
                    .Text("Location", header: "Location", iseditingreadonly: true) // 8
                    .Text("ToPOID", header: "To POID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("ToSeq", header: "To Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    ;
                    break;
                case "P34":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("qtybefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty)
                    .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4))
                    .Text("Location", header: "Location", iseditingreadonly: true)
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true)
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
                    ;
                    break;
                case "P35":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Numeric("qtybefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 6
                    .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
                    .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4)) // 8
                    .Text("Location", header: "Location", iseditingreadonly: true) // 9
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true) // 10
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 11
                    ;
                    break;
                case "P43":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("Seq", header: "Seq", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("Roll", header: "Roll", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Numeric("Qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0).Get(out this.col_Qty)
                    .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                    .Text("StockUnit", header: "Unit", iseditingreadonly: true)
                    .Text("location", header: "location", iseditingreadonly: true)
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true)
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20));
                    break;
                case "P45":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true) // 1
                    .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true) // 2
                    .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true) // 4
                    .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
                    .Numeric("Qty", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0).Get(out this.col_Qty)// 5
                    .Numeric("adjustqty", header: "Remove Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 6
                    .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 7
                    .Text("Location", header: "Location", iseditingreadonly: true) // 7
                    .Text("reasonid", header: "Reason ID", iseditingreadonly: true) // 8
                    .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 9
                    ;
                    break;
                case "P22":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 5
                    .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 7
                    .Text("toLocation", header: "To Location", iseditingreadonly: true, width: Widths.AnsiChars(18)) // 8
                    ;
                    break;
                case "P23":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "Inventory" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Inventory" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 5
                    .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 7
                    .Text("topoid", header: "Bulk" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 8
                    .Text("toseq", header: "Bulk" + Environment.NewLine + " Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 9
                    .Text("toLocation", header: "To Location", iseditingreadonly: false, width: Widths.AnsiChars(18)) // 10
                    ;
                    break;
                case "P24":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) // 4
                    .Text("fabrictype", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 6
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 7
                    .Text("FromLocation", header: "From Location", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 8
                    .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(15), iseditingreadonly: true) // 8
                    ;
                    break;
                case "P36":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("fromseq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 6
                    .Text("ToLocation", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(30)) // 7
                    ;
                    break;
                case "P37":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
                    .DateTime("CompleteTime", header: "CompleteTime", width: Widths.AnsiChars(18), iseditingreadonly: true)
                    .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                    .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                    .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                    .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                    .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                    .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                    .Text("StockType", header: "StockType", iseditingreadonly: true) // 5
                    .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true).Get(out this.col_Qty) // 6
                    .Text("Location", header: "Location", iseditingreadonly: true) // 7
                    ;
                    break;
                case "P31":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
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
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 11
                    .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
                    ;
                    break;
                case "P32":
                    this.gridUpdate.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.gridUpdate)
                    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                    .Text("SentToWMS", header: "Sen ToW MS", width: Widths.AnsiChars(6), iseditingreadonly: true)
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
                    .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10).Get(out this.col_Qty) // 11
                    .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                    .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 12
                    .ComboBox("tostocktype", header: "To" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false)
                    ;
                    break;
                default:
                    break;
            }
        }

        private void ChangeGridColor()
        {
            // 設定detailGrid Rows 是否可以編輯
            this.gridUpdate.RowEnter += this.Detailgrid_RowEnter;
            this.gridUpdate.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            for (int index = 0; index < this.gridUpdate.Rows.Count; index++)
            {
                DataRow dr = this.gridUpdate.GetDataRow(index);
                if (this.gridUpdate.Rows.Count <= index || index < 0)
                {
                    return;
                }

                int i = index;

                // 反灰不能勾選
                if (dr["SentToWMS"].ToString() == "V" && !MyUtility.Check.Empty(dr["CompleteTime"]))
                {
                    this.gridUpdate.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                }
            }
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
            if (data["SentToWMS"].ToString() == "V" && !MyUtility.Check.Empty(data["CompleteTime"]))
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

        private void ComboFunction_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboFunction.SelectedValue != null)
            {
                this.strFunction = this.comboFunction.SelectedValue.ToString();
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

        }

        private void BtnQuery2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtWKNo.Text) && MyUtility.Check.Empty(this.txtReceivingID.Text))
            {
                MyUtility.Msg.WarningBox("<SP#>,<WK#>,<Receiving ID> cannot be empty!");
                return;
            }

            this.Query_Sheet2();
        }

        private void Query_Sheet2()
        {
            string sqlcmd = @"select
 [Selected] = 0 --0
 ,fi.POID
 ,fi.Seq1,fi.Seq2
 ,[MaterialType] = case when pd.FabricType = 'F' then 'Fabric' when pd.FabricType='A' then 'Accessory' end
 ,fi.Roll,fi.Dyelot,fi.InQty,fi.OutQty,fi.AdjustQty
 ,[Balance] = fi.InQty-fi.OutQty+fi.AdjustQty
 ,[StockType] = case fi.StockType 
				when 'B' then 'Bulk'
				when 'I' then 'Inventory'
				else fi.StockType end
,[Location] = dbo.Getlocation(fi.Ukey)
from dbo.FtyInventory fi WITH (NOLOCK) 
left join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = fi.POID and pd.seq1 = fi.seq1 and pd.seq2  = fi.Seq2
left join dbo.orders o WITH (NOLOCK) on o.id = fi.POID
left join dbo.factory f WITH (NOLOCK) on o.FtyGroup=f.id
left join Receiving_Detail rd WITH (NOLOCK) on rd.PoId = fi.POID and rd.Seq1 = fi.seq1 and rd.seq2 = fi.seq2 and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
left join Receiving r WITH (NOLOCK) on r.id = rd.id
left join TransferIn_Detail TD WITH (NOLOCK) on TD.PoId = fi.POID and TD.Seq1 = fi.seq1 and TD.seq2 = fi.seq2 and TD.Roll = fi.Roll and TD.Dyelot = fi.Dyelot
left join TransferIn T WITH (NOLOCK) on T.id = TD.id
where 1=1
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
                this.SetGrid();
                this.ChangeGridColor();
                this.Grid_Filter();
            }
            else
            {
                this.ShowErr(result);
            }

            this.HideWaitMessage();
        }

        private void SetGrid2()
        {

        }

        private void ChangeGridColor2()
        {

        }
    }
}
